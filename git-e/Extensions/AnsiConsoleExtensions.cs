using Spectre.Console;

namespace gite.Extensions;

public static class AnsiConsoleExtensions
{
    public static void WriteErrorLine(this IAnsiConsole console, string errorMessage)
    {
        console.MarkupLine($"[red]Error:[/] {errorMessage}");
    }
}
