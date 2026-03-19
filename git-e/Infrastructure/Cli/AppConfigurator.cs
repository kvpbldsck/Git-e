using gite.Commands;
using Spectre.Console.Cli;

namespace gite.Infrastructure.Cli;

public static class AppConfigurator
{
    public static CommandApp Configure(this CommandApp app)
    {
        app.Configure(cfg =>
        {
            cfg.SetApplicationName("git-e");

            cfg.AddCommand<ShowCommitsCommand>(ShowCommitsSettings.CommandName)
                .WithAlias(ShowCommitsSettings.Alias)
                .WithDescription(ShowCommitsSettings.Description);

            cfg.AddCommand<ShowBranchesCommand>(ShowBranchesSettings.CommandName)
                .WithAlias(ShowBranchesSettings.Alias)
                .WithDescription(ShowBranchesSettings.Description);
        });

        return app;
    }
}
