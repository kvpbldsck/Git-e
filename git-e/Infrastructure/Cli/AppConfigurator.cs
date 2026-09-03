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

            cfg.AddCommand<StashCommand>(StashSettings.CommandName)
                .WithAlias(StashSettings.Alias)
                .WithDescription(StashSettings.Description);

            cfg.AddCommand<UnstashCommand>(UnstashSettings.CommandName)
                .WithAlias(UnstashSettings.Alias)
                .WithDescription(UnstashSettings.Description);
        });

        return app;
    }
}
