using LibGit2Sharp;

namespace gite.Git;

public sealed class GitWrapper
{
    public Commit[] GetLastCommits(int commitsCount, string? path = null)
    {
        try
        {
            using var repo = new Repository(path ?? Environment.CurrentDirectory);
            return repo.Commits
                .Take(commitsCount)
                .ToArray();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
