using ErrorOr;
using LibGit2Sharp;

namespace gite.Git;

public sealed class GitWrapper
{
    public ErrorOr<Models.Commit[]> GetLastCommits(int commitsCount, string path)
    {
        try
        {
            using var repo = new Repository(path);
            return repo.Commits
                .Take(commitsCount)
                .Select(Models.Commit.FromGit)
                .ToArray();
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
