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

            cfg.AddCommand<ShowCommitsCommand>("commits")
                .WithAlias("c")
                .WithDescription("List last commits");
        });

        return app;
    }
}
