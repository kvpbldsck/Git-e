using GitE.Cli.Commands;
using GitE.Git;
using GitE.Repositories;
using GitE.Utils;

using Microsoft.Extensions.DependencyInjection;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace GitE;

internal static class Program
{
    public static int Main(string[] args)
    {
        IServiceCollection services = new ServiceCollection()
            .AddSingleton(PrepareYamlDeserializer())
            .AddSingleton(PrepareYamlSerializer())
            .AddSingleton<SettingsRepository>()
            .AddSingleton<Runner>()
            .AddSingleton<GitWrapper>()
            .AddSingleton<ICommand, ShowCommitsCommand>();

        ServiceProvider serviceProvider = services.BuildServiceProvider();

        Runner runner = serviceProvider.GetRequiredService<Runner>();
        return runner.Run(args);
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
