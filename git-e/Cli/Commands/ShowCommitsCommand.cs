using System.CommandLine;

using GitE.Git;

namespace GitE.Cli.Commands;

public sealed class ShowCommitsCommand(GitWrapper gitWrapper) : ICommand
{
    private static readonly Option<int> AmountOption = GetAmountOption();

    public Command PrepareCommand()
    {
        var command = new Command("show-commits", "Show the list of commits in the repository.")
        {
            Aliases = { "c", "commits" },
            Options = { AmountOption }
        };

        command.SetAction(r =>
        {
            var commitsAmount = r.GetRequiredValue(AmountOption);

            gitWrapper
                .Log("%h - %s (%an, %ar)", commitsAmount)
                .Switch(
                    success => Console.WriteLine(success.Value),
                    failure => Console.Error.WriteLine($"{failure.Code}: {failure.Description}")
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
            DefaultValueFactory = result => 15
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
}
