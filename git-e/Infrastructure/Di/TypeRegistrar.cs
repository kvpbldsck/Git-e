using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace gite.Infrastructure.Di;

internal sealed class TypeRegistrar(IServiceCollection services) : ITypeRegistrar
{
    public void Register(Type service, Type implementation) 
        => services.AddSingleton(service, implementation);

    public void RegisterInstance(Type service, object implementation)
        => services.AddSingleton(service, implementation);

    public void RegisterLazy(Type service, Func<object> factory)
        => services.AddSingleton(service, _ => factory());

    public ITypeResolver Build()
        => new TypeResolver(services.BuildServiceProvider());
}
