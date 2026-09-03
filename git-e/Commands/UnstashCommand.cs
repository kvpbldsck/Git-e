using System.ComponentModel;
using ErrorOr;
using gite.Extensions;
using gite.Git;
using Spectre.Console;
using Spectre.Console.Cli;

namespace gite.Commands;

public sealed class UnstashSettings : GitCommandSettings
{
    public const string CommandName = "unstash";
    public const string Alias = "us";
    public const string Description = "Restore stashed changes";

    [CommandOption("-i|--index")]
    [Description("Stash index to restore (0 is most recent)")]
    [DefaultValue(0)]
    public int Index { get; init; }

    [CommandOption("--keep")]
    [Description("Apply stash but keep it in the stash list")]
    [DefaultValue(false)]
    public bool KeepStash { get; init; }
}

public sealed class UnstashCommand(GitWrapper git, IAnsiConsole console) : Command<UnstashSettings>
{
    protected override int Execute(
        CommandContext context,
        UnstashSettings settings,
        CancellationToken cancellationToken)
        => git
            .Unstash(settings.PathOrCurrentDirectory, settings.Index, settings.KeepStash)
            .Match(HandleSuccess, HandleErrors);

    private int HandleSuccess(string message)
    {
        console.WriteLine(message);
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
