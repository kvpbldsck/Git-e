using System.CommandLine;

using GitE.Git;

namespace GitE.Cli.Commands;

public sealed class ShowCommitsCommand(GitWrapper gitWrapper) : ICommand
{
    private static readonly Option<int> AmountOption = GetAmountOption();
    private static readonly Argument<DirectoryInfo> RepositoryArgument = GetRepositoryArgument();

    public Command PrepareCommand()
    {
        var command = new Command("show-commits", "Show the list of commits in the repository.")
        {
            Aliases = { "c", "commits" },
            Options = { AmountOption },
            Arguments = { RepositoryArgument }
        };

        command.SetAction(r =>
        {
            var commitsAmount = r.GetRequiredValue(AmountOption);

            gitWrapper
                .Log(commitsAmount)
                .Switch(
                    success => Console.WriteLine(success.Value),
                    failure => Console.Error.WriteLine(failure.Description)
                );
        });

        return command;
    }

    private static Option<int> GetAmountOption()
    {
        var option = new Option<int>(name: "--amount", aliases: "-a")
        {
            Arity = ArgumentArity.ZeroOrOne,
            Description = "The number of commits to show.",
            HelpName = "amount",
            Required = false,
            DefaultValueFactory = _ => 15
        };

        option.Validators.Add(result =>
        {
            var value = result.GetValue(option);
            switch (value)
            {
                case < 1:
                    result.AddError("Must be greater than 0");
                    break;
                case > 100:
                    result.AddError("Must be less than or equal to 100");
                    break;
                default:
                    break;
            }
        });

        return option;
    }

    private static Argument<DirectoryInfo> GetRepositoryArgument()
    {
        var argument = new Argument<DirectoryInfo>(name: "repository")
        {
            Arity = ArgumentArity.ZeroOrOne,
            Description = "The path to the git repository.",
            HelpName = "repository",
            DefaultValueFactory = _ => new DirectoryInfo(Environment.CurrentDirectory)
        };

        argument.Validators.Add(result =>
        {
            DirectoryInfo value = result.GetValue(argument)!;
            if (!value.Exists)
            {
                result.AddError("The specified directory does not exist.");
            }
        });

        return argument;
    }
}
