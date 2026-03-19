using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace gite.Commands;

public abstract class GitCommandSettings : CommandSettings
{
    [CommandArgument(0, "[path]")]
    [Description("Path to git repository. By default set to current folder")]
    public string? Path { get; init; }

    public string PathOrCurrentDirectory => Path ?? Environment.CurrentDirectory;
    
    public override ValidationResult Validate()
    {
        return Path is null || Directory.Exists(Path) 
            ? base.Validate() 
            : ValidationResult.Error($"Directory {Path} not found");
    }
}
