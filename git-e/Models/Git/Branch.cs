namespace gite.Models.Git;

public sealed record Branch(string Name, bool IsActive)
{
    public static Branch FromGit(LibGit2Sharp.Branch branch)
        => new(
            branch.FriendlyName, 
            branch.IsCurrentRepositoryHead);
}
