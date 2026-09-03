using System.ComponentModel;
using ErrorOr;
using gite.Extensions;
using gite.Git;
using LibGit2Sharp;
using Spectre.Console;
using Spectre.Console.Cli;

namespace gite.Commands;

public sealed class StashSettings : GitCommandSettings
{
    public const string CommandName = "stash";
    public const string Alias = "s";
    public const string Description = "Stash local changes";

    [CommandOption("-m|--message")]
    [Description("Stash message")]
    public string? Message { get; init; }

    [CommandOption("-u|--include-untracked")]
    [Description("Also stash untracked files")]
    [DefaultValue(false)]
    public bool IncludeUntracked { get; init; }

    [CommandOption("--keep-index")]
    [Description("Keep staged changes in the index")]
    [DefaultValue(false)]
    public bool KeepIndex { get; init; }
}

public sealed class StashCommand(GitWrapper git, IAnsiConsole console) : Command<StashSettings>
{
    protected override int Execute(
        CommandContext context,
        StashSettings settings,
        CancellationToken cancellationToken)
        => git
            .Stash(settings.PathOrCurrentDirectory, settings.Message, GetModifiers(settings))
            .Match(HandleSuccess, HandleErrors);

    private static StashModifiers GetModifiers(StashSettings settings)
    {
        var modifiers = StashModifiers.Default;
        if (settings.KeepIndex)
        {
            modifiers |= StashModifiers.KeepIndex;
        }

        if (settings.IncludeUntracked)
        {
            modifiers |= StashModifiers.IncludeUntracked;
        }

        return modifiers;
    }

    private int HandleSuccess(string stashRef)
    {
        console.WriteLine($"Saved working directory: {stashRef}");
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
