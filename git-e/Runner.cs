using System.CommandLine;

using GitE.Cli.Commands;

namespace GitE;

public sealed class Runner(IEnumerable<ICommand> commands)
{
    public int Run(string[] args)
    {
        RootCommand rootCommand = new("GitE - A Git Extension Tool");

        foreach (ICommand command in commands)
        {
            rootCommand.Add(command.PrepareCommand());
        }

        return rootCommand
            .Parse(args)
            .Invoke();
    }
}
