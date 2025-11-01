using System.Diagnostics;

using GitE.Models.Result;

namespace GitE.Git;

public sealed class GitWrapper
{
    public Result<string> Log(string format, int commitsAmount) =>
        RunGitCommand($"log --pretty=format:\"{format}\" -n {commitsAmount}");

    private static Result<string> RunGitCommand(string arguments)
    {
        var gitStartInfo = new ProcessStartInfo
        {
            FileName = "git",
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(gitStartInfo)!;
        var output = process.StandardOutput.ReadToEnd();
        var error = process.StandardError.ReadToEnd();
        process.WaitForExit();

        return process.ExitCode != 0
            ? GitResultParser.ParseError(error, process.ExitCode)
            : GitResultParser.ParseSuccess(output);
    }
}
