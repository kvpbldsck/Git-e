using ErrorOr;
using LibGit2Sharp;
using Commit = gite.Models.Git.Commit;
using Branch = gite.Models.Git.Branch;

namespace gite.Git;

public sealed class GitWrapper
{
    public ErrorOr<Commit[]> GetLastCommits(int commitsCount, string path)
        => DoGitOperation(
            path,
            repo => repo.Commits
                .Take(commitsCount)
                .Select(Commit.FromGit)
                .ToArray());

    public ErrorOr<Branch[]> GetBranches(string path, bool showRemoteBranches)
        => DoGitOperation(
            path,
            repo => repo.Branches
                .Where(b => showRemoteBranches || !b.IsRemote)
                .Select(Branch.FromGit)
                .ToArray());

    public ErrorOr<string> Stash(string path, string? message, StashModifiers modifiers)
        => DoGitOperation(
            path,
            repo =>
            {
                if (!repo.RetrieveStatus().IsDirty)
                {
                    throw new InvalidOperationException("No local changes to save");
                }

                var stash = repo.Stashes.Add(
                    CreateSignature(repo),
                    message,
                    modifiers);

                return stash is null
                    ? throw new InvalidOperationException("No local changes to save")
                    : FormatStashRef(0, stash.Message);
            });

    public ErrorOr<string> Unstash(string path, int index, bool keepStash)
        => DoGitOperation(
            path,
            repo =>
            {
                if (!repo.Stashes.Any())
                {
                    throw new InvalidOperationException("No stash entries found");
                }

                var status = keepStash
                    ? repo.Stashes.Apply(index)
                    : repo.Stashes.Pop(index);

                return status switch
                {
                    StashApplyStatus.Applied => keepStash
                        ? $"Applied {FormatStashRef(index, repo.Stashes[index].Message)}"
                        : $"Popped {FormatStashRef(index)}",
                    StashApplyStatus.Conflicts => throw new InvalidOperationException(
                        "Stash applied with merge conflicts; resolve conflicts and try again"),
                    StashApplyStatus.NotFound => throw new InvalidOperationException($"Stash entry stash@{{{index}}} not found"),
                    StashApplyStatus.UncommittedChanges => throw new InvalidOperationException(
                        "Cannot restore stash: you have uncommitted changes in your working tree"),
                    _ => throw new InvalidOperationException($"Failed to restore stash: {status}")
                };
            });

    private static string FormatStashRef(int index, string? message = null)
        => string.IsNullOrWhiteSpace(message)
            ? $"stash@{{{index}}}"
            : $"stash@{{{index}}}: {message}";

    private static Signature CreateSignature(Repository repo)
    {
        var name = repo.Config.Get<string>("user.name")?.Value ?? "git-e";
        var email = repo.Config.Get<string>("user.email")?.Value ?? "git-e@local";
        return new Signature(name, email, DateTimeOffset.Now);
    }

    private ErrorOr<T> DoGitOperation<T>(string path, Func<Repository, T> operation)
    {
        try
        {
            using var repo = new Repository(path);
            return operation(repo);
        }
        catch (RepositoryNotFoundException)
        {
            return Error.NotFound(description: $"Repository at {path} not found");
        }
        catch (Exception e)
        {
            return Error.Unexpected(description: e.Message);
        }
    }
}
