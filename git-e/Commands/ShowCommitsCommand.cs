using System.ComponentModel;
using ErrorOr;
using gite.Git;
using gite.Models;
using Humanizer;
using Spectre.Console;
using Spectre.Console.Cli;

namespace gite.Commands;

public sealed class ShowCommitsCommand : Command<ShowCommitsSettings>
{
    private readonly GitWrapper _git;
    private readonly IAnsiConsole _console;

    public ShowCommitsCommand(GitWrapper git, IAnsiConsole console)
    {
        _git = git;
        _console = console;
    }

    protected override int Execute(
        CommandContext context, 
        ShowCommitsSettings settings, 
        CancellationToken cancellationToken)
    {
        try
        {
            return _git
                .GetLastCommits(settings.CommitsCount, Path.Combine(Environment.CurrentDirectory, "../../../../"))
                .Match(PrintCommits, PrintErrors);
        }
        catch (Exception e)
        {
            return PrintError(e);
        }
    }

    private static string FormatCommit(Commit commit)
        => $"{commit.Id[..7]} - {commit.AuthorName}, {commit.Date.Humanize()} - {commit.MessageShort}";

    private int PrintCommits(Commit[] commits)
    {
        foreach (var commit in commits)
        {
            _console.WriteLine(FormatCommit(commit));
        }

        return 0;
    }

    private int PrintErrors(List<Error> errors)
    {
        foreach (var e in errors)
        {
            _console.MarkupLine($"[red]{e.Description}[/]");
        }
                    
        return 1;
    }

    private int PrintError(Exception error)
    {
        _console.MarkupLine($"[red]{error.Message}[/]");
                    
        return 1;
    }
}

public sealed class ShowCommitsSettings : CommandSettings
{
    private const int DefaultCommitsCount = 10;
    
    [CommandOption("-c|--count")]
    [Description("Number of commits to show")]
    [DefaultValue(DefaultCommitsCount)]
    public int CommitsCount { get; init; } = DefaultCommitsCount;
}
