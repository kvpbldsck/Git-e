using GitE.Models.Result;

namespace GitE.Git;

public static class GitResultParser
{
    public static Result<string> ParseSuccess(string commandOutput) =>
        new SuccessResult<string>(commandOutput);

    public static Result<string> ParseError(string errorOutput, int processExitCode)
    {
        if (errorOutput.Contains("not a git repository", StringComparison.InvariantCultureIgnoreCase))
        {
            return ErrorResult.Custom(
                ErrorType.Conflict,
                ErrorCodes.Git.NotAGitRepository,
                "Current directory does not contain a git repository.");
        }

        return ErrorResult.Failure(description: "Git command failed with exit code " + processExitCode);
    }
}
