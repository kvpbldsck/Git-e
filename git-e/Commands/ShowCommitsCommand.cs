using System.ComponentModel;
using ErrorOr;
using gite.Extensions;
using gite.Git;
using gite.Models.Git;
using Humanizer;
using Spectre.Console;
using Spectre.Console.Cli;

namespace gite.Commands;

public sealed class ShowCommitsSettings : GitCommandSettings
{
    public const string CommandName = "commits";
    public const string Alias = "c";
    public const string Description = "List last commits";
    
    private const int DefaultCommitsCount = 10;
    
    [CommandOption("-c|--count")]
    [Description("Number of commits to show")]
    [DefaultValue(DefaultCommitsCount)]
    public int CommitsCount { get; init; }
}

public sealed class ShowCommitsCommand(GitWrapper git, IAnsiConsole console) : Command<ShowCommitsSettings>
{
    
    protected override int Execute(
        CommandContext context, 
        ShowCommitsSettings settings, 
        CancellationToken cancellationToken) 
        => git
            .GetLastCommits(settings.CommitsCount, settings.PathOrCurrentDirectory)
            .Match(HandleCommits, HandleErrors);

    private static string FormatCommit(Commit commit)
        => $"{commit.Id[..7]} - {commit.AuthorName}, {commit.Date.Humanize()} - {commit.MessageShort}";

    private int HandleCommits(Commit[] commits)
    {
        foreach (var commit in commits)
        {
            console.WriteLine(FormatCommit(commit));
        }

        return 0;
    }

    private int HandleErrors(List<Error> errors)
    {
        foreach (var e in errors)
        {
            console.WriteErrorLine(e.Description);
        }
                    
        return 1;
    }
}
