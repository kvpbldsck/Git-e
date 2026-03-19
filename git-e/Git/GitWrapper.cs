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
