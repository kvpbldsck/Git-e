using System.ComponentModel;
using gite.Git;
using Humanizer;
using LibGit2Sharp;
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
        var commits = _git.GetLastCommits(settings.CommitsCount);

        foreach (var commit in commits.Select(FormatCommit))
        {
            _console.Write(commit);
        }

        return 0;
    }
    
    private static string FormatCommit(Commit commit)
        => $"{commit.Id.Sha[..7]} - {commit.Author.Name}, {commit.Author.When.Humanize()} - {commit.MessageShort}";
}

public sealed class ShowCommitsSettings : CommandSettings
{
    private const int DefaultCommitsCount = 10;
    
    [CommandOption("-c|--count")]
    [Description("Number of commits to show")]
    [DefaultValue(DefaultCommitsCount)]
    public int CommitsCount { get; init; } = DefaultCommitsCount;
}
