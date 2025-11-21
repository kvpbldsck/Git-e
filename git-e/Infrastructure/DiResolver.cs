using Spectre.Console.Cli;

namespace GitE.Infrastructure;

public sealed class DiResolver(IServiceProvider provider) : ITypeResolver, IDisposable
{
    public object? Resolve(Type? type) =>
        type is null
            ? null
            : provider.GetService(type);

    public void Dispose()
    {
        if (provider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}
