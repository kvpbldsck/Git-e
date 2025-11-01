using System.CommandLine;

namespace GitE.Cli.Commands;

public interface ICommand
{
    Command PrepareCommand();
}
