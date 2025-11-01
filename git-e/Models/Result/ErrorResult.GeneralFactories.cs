namespace GitE.Models.Result;

public readonly partial record struct ErrorResult
{
    /// <summary>
    /// Creates an <see cref="Error"/> of type <see cref="ErrorType.Failure"/> from a code and description.
    /// </summary>
    /// <param name="code">The unique error code.</param>
    /// <param name="description">The error description.</param>
    /// <param name="metadata">A dictionary which provides optional space for information.</param>
    public static ErrorResult Failure(
        string code = ErrorCodes.General.Failure,
        string description = "A failure has occurred.",
        Dictionary<string, object>? metadata = null) =>
        new(code, description, ErrorType.Failure, metadata);

    /// <summary>
    /// Creates an <see cref="Error"/> of type <see cref="ErrorType.Unexpected"/> from a code and description.
    /// </summary>
    /// <param name="code">The unique error code.</param>
    /// <param name="description">The error description.</param>
    /// <param name="metadata">A dictionary which provides optional space for information.</param>
    public static ErrorResult Unexpected(
        string code = ErrorCodes.General.Unexpected,
        string description = "An unexpected error has occurred.",
        Dictionary<string, object>? metadata = null) =>
        new(code, description, ErrorType.Unexpected, metadata);

    /// <summary>
    /// Creates an <see cref="Error"/> of type <see cref="ErrorType.Validation"/> from a code and description.
    /// </summary>
    /// <param name="code">The unique error code.</param>
    /// <param name="description">The error description.</param>
    /// <param name="metadata">A dictionary which provides optional space for information.</param>
    public static ErrorResult Validation(
        string code = ErrorCodes.General.Validation,
        string description = "A validation error has occurred.",
        Dictionary<string, object>? metadata = null) =>
        new(code, description, ErrorType.Validation, metadata);

    /// <summary>
    /// Creates an <see cref="Error"/> of type <see cref="ErrorType.Conflict"/> from a code and description.
    /// </summary>
    /// <param name="code">The unique error code.</param>
    /// <param name="description">The error description.</param>
    /// <param name="metadata">A dictionary which provides optional space for information.</param>
    public static ErrorResult Conflict(
        string code = ErrorCodes.General.Conflict,
        string description = "A conflict error has occurred.",
        Dictionary<string, object>? metadata = null) =>
        new(code, description, ErrorType.Conflict, metadata);

    /// <summary>
    /// Creates an <see cref="Error"/> of type <see cref="ErrorType.NotFound"/> from a code and description.
    /// </summary>
    /// <param name="code">The unique error code.</param>
    /// <param name="description">The error description.</param>
    /// <param name="metadata">A dictionary which provides optional space for information.</param>
    public static ErrorResult NotFound(
        string code = ErrorCodes.General.NotFound,
        string description = "A 'Not Found' error has occurred.",
        Dictionary<string, object>? metadata = null) =>
        new(code, description, ErrorType.NotFound, metadata);

    /// <summary>
    /// Creates an <see cref="Error"/> of type <see cref="ErrorType.Unauthorized"/> from a code and description.
    /// </summary>
    /// <param name="code">The unique error code.</param>
    /// <param name="description">The error description.</param>
    /// <param name="metadata">A dictionary which provides optional space for information.</param>
    public static ErrorResult Unauthorized(
        string code = ErrorCodes.General.Unauthorized,
        string description = "An 'Unauthorized' error has occurred.",
        Dictionary<string, object>? metadata = null) =>
        new(code, description, ErrorType.Unauthorized, metadata);

    /// <summary>
    /// Creates an <see cref="Error"/> of type <see cref="ErrorType.Forbidden"/> from a code and description.
    /// </summary>
    /// <param name="code">The unique error code.</param>
    /// <param name="description">The error description.</param>
    /// <param name="metadata">A dictionary which provides optional space for information.</param>
    public static ErrorResult Forbidden(
        string code = ErrorCodes.General.Forbidden,
        string description = "A 'Forbidden' error has occurred.",
        Dictionary<string, object>? metadata = null) =>
        new(code, description, ErrorType.Forbidden, metadata);
}
