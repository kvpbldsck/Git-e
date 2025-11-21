using System.CommandLine;

using GitE.Commands;

using Microsoft.Extensions.DependencyInjection;

namespace GitE;

public sealed class Runner(IEnumerable<ICommand> commands)
{
    public int Run(IServiceCollection services, string[] args)
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
