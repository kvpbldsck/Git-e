using System.ComponentModel;
using ErrorOr;
using gite.Extensions;
using gite.Git;
using gite.Models.Git;
using Spectre.Console;
using Spectre.Console.Cli;

namespace gite.Commands;

public sealed class ShowBranchesSettings : GitCommandSettings
{
    public const string CommandName = "branches";
    public const string Alias = "b";
    public const string Description = "List all local branches";
    
    [CommandOption("-r|--including-remote")]
    [Description("Show also remote branches")]
    [DefaultValue(false)]
    public bool ShowRemoteBranches { get; init; }
}

public sealed class ShowBranchesCommand(GitWrapper git, IAnsiConsole console) : Command<ShowBranchesSettings>
{
    protected override int Execute(
        CommandContext context, 
        ShowBranchesSettings settings, 
        CancellationToken cancellationToken)
        => git
            .GetBranches(settings.PathOrCurrentDirectory, settings.ShowRemoteBranches)
            .Match(HandleBranches, HandleErrors);

    private int HandleBranches(Branch[] branches)
    {
        foreach (var branch in branches)
        {
            console.MarkupLine(FormatBranch(branch));
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

    private static string FormatBranch(Branch branch)
        => branch.IsActive 
            ? $"[green]{branch.Name}[/]" 
            : branch.Name;
}
