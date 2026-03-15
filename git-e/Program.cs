using gite.Commands;
using gite.Git;
using gite.Infrastructure.Cli;
using gite.Infrastructure.Di;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace gite;

public static class Program
{
    public static int Main(string[] args)
    {
        var services = new ServiceCollection();
        services.AddSingleton<GitWrapper>();
        services.AddSingleton<ShowCommitsCommand>();
  
        var registrar = new TypeRegistrar(services);
        
        return new CommandApp(registrar)
            .Configure()
            .Run(args);
    }
}
