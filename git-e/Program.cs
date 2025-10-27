using GitE;
using GitE.Repositories;
using GitE.Utils;

using Microsoft.Extensions.DependencyInjection;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

IServiceCollection services = new ServiceCollection()
    .AddSingleton(PrepareYamlDeserializer())
    .AddSingleton(PrepareYamlSerializer())
    .AddSingleton<SettingsRepository>()
    .AddSingleton<Runner>();

ServiceProvider serviceProvider = services.BuildServiceProvider();

Runner runner = serviceProvider.GetRequiredService<Runner>();
runner.Run();
return;

IDeserializer PrepareYamlDeserializer()
{
    return new StaticDeserializerBuilder(new YamlStaticContext())
        .WithNamingConvention(UnderscoredNamingConvention.Instance)
        .Build();
}

ISerializer PrepareYamlSerializer()
{
    return new StaticSerializerBuilder(new YamlStaticContext())
        .WithNamingConvention(UnderscoredNamingConvention.Instance)
        .Build();
}

