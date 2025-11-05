namespace GitE.Models.Result;

// Code have been taken from https://github.com/amantinband/error-or

/// <summary>
/// Error types.
/// </summary>
public enum ErrorType
{
    Failure,
    Unexpected,
    Validation,
    Conflict,
    NotFound,
    Forbidden,
}
