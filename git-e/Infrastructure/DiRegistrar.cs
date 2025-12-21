using Microsoft.Extensions.DependencyInjection;

using Spectre.Console.Cli;

namespace GitE.Infrastructure;

public sealed class DiRegistrar(IServiceCollection container) : ITypeRegistrar
{
    public void Register(Type service, Type implementation) =>
        container.AddSingleton(service, implementation);

    public void RegisterInstance(Type service, object implementation) =>
        container.AddSingleton(service, implementation);

    public void RegisterLazy(Type service, Func<object> factory) =>
        container.AddSingleton(service, factory);

    public ITypeResolver Build() =>
        new DiResolver(container.BuildServiceProvider());
}
