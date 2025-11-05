using GitE.Models.Result;
using GitE.Utils;

using LibGit2Sharp;

namespace GitE.Git;

public sealed class GitWrapper
{
    public Result<string> Log(int commitsAmount, IFormatter<Commit>? formatter = null)
    {
        formatter ??= new DefaultCommitFormatter();

        Repository? repo = null;
        try
        {
            repo = new(Environment.CurrentDirectory);

            return repo.Commits
                .Take(commitsAmount)
                .Select(formatter.Format)
                .JoinAsString("\n");
        }
        catch (RepositoryNotFoundException e)
        {
            return ErrorResult.Custom(
                ErrorType.NotFound,
                ErrorCodes.Git.NotAGitRepository,
                "The current directory is not a git repository.");
        }
        catch (Exception e)
        {
            return ErrorResult.Failure($"Failed to get git log: {e.Message}");
        }
        finally
        {
            repo?.Dispose();
        }
    }
}
