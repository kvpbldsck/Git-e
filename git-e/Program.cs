using GitE.Commands;
using GitE.Git;
using GitE.Infrastructure;
using GitE.Repositories;
using GitE.Utils;

using Microsoft.Extensions.DependencyInjection;

using Spectre.Console.Cli;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

using ICommand = GitE.Commands.ICommand;

namespace GitE;

internal static class Program
{
    public static int Main(string[] args)
    {
        var registrar = new DiRegistrar(PrepareDiContainer());

        var app = new CommandApp(registrar);
        app.Configure(config =>
        {
#if DEBUG
            config.PropagateExceptions();
            config.ValidateExamples();
#endif
        });

        return app.Run(args);
    }

    private static IServiceCollection PrepareDiContainer()
    {
        return new ServiceCollection()
            .AddSingleton(PrepareYamlDeserializer())
            .AddSingleton(PrepareYamlSerializer())
            .AddSingleton<SettingsRepository>()
            .AddSingleton<Runner>()
            .AddSingleton<GitWrapper>()
            .AddSingleton<ICommand, ShowCommitsCommand>();
    }

    private static IDeserializer PrepareYamlDeserializer()
    {
        return new StaticDeserializerBuilder(new YamlStaticContext())
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();
    }

    private static ISerializer PrepareYamlSerializer()
    {
        return new StaticSerializerBuilder(new YamlStaticContext())
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();
    }
}
