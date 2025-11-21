namespace GitE.Models.Git;

public sealed record CommitDto(
    string Id,
    string AuthorName,
    DateTimeOffset Date,
    string Message);

public static class CommitMappingExtensions
{
    public static CommitDto ToDto(this LibGit2Sharp.Commit commit) =>
        new CommitDto(
            commit.Id.Sha,
            commit.Author.Name,
            commit.Author.When,
            commit.Message);
}
