namespace gite.Models;

public sealed record Commit(
    string Id,
    string AuthorEmail,
    string AuthorName,
    DateTimeOffset Date,
    string MessageShort)
{
    public static Commit FromGit(LibGit2Sharp.Commit commit)
        => new(
            commit.Id.Sha,
            commit.Author.Email,
            commit.Author.Name,
            commit.Author.When,
            commit.MessageShort);
}
