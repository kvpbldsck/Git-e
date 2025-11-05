using Humanizer;

using LibGit2Sharp;

namespace GitE.Git;

public interface IFormatter<in T>
{
    string Format(T commit);
}

public sealed class DefaultCommitFormatter : IFormatter<Commit>
{
    public string Format(Commit commit) =>
        $"{commit.Id.Sha[..7]} - {commit.Author.Name}, {commit.Author.When.Humanize()} - {commit.MessageShort}";
}
