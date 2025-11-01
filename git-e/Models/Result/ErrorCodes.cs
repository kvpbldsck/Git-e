namespace GitE.Models.Result;

public static class ErrorCodes
{
    public static class General
    {
        private const string Prefix = "General.";

        public const string Failure = Prefix + nameof(Failure);
        public const string Unexpected = Prefix + nameof(Unexpected);
        public const string Validation = Prefix + nameof(Validation);
        public const string Conflict = Prefix + nameof(Conflict);
        public const string NotFound = Prefix + nameof(NotFound);
        public const string Unauthorized = Prefix + nameof(Unauthorized);
        public const string Forbidden = Prefix + nameof(Forbidden);
    }

    public static class Git
    {
        private const string Prefix = "Git.";

        public const string NotAGitRepository = Prefix + nameof(NotAGitRepository);
    }
}
