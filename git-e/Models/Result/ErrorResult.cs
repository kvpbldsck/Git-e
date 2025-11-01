namespace GitE.Models.Result;

// Code have been taken from https://github.com/amantinband/error-or

/// <summary>
/// Represents an error.
/// </summary>
public readonly partial record struct ErrorResult
{
    private ErrorResult(string code, string description, ErrorType type, Dictionary<string, object>? metadata)
    {
        Code = code;
        Description = description;
        Type = type;
        NumericType = (int)type;
        Metadata = metadata;
    }

    /// <summary>
    /// Gets the unique error code.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Gets the error description.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets the error type.
    /// </summary>
    public ErrorType Type { get; }

    /// <summary>
    /// Gets the numeric value of the type.
    /// </summary>
    public int NumericType { get; }

    /// <summary>
    /// Gets the metadata.
    /// </summary>
    public Dictionary<string, object>? Metadata { get; }

    /// <summary>
    /// Creates an <see cref="Error"/> with the given numeric <paramref name="type"/>,
    /// <paramref name="code"/>, and <paramref name="description"/>.
    /// </summary>
    /// <param name="type">An integer value which represents the type of error that occurred.</param>
    /// <param name="code">The unique error code.</param>
    /// <param name="description">The error description.</param>
    /// <param name="metadata">A dictionary which provides optional space for information.</param>
    public static ErrorResult Custom(
        ErrorType type,
        string code,
        string description,
        Dictionary<string, object>? metadata = null) =>
            new(code, description, type, metadata);

    public bool Equals(ErrorResult other)
    {
        if (Type != other.Type ||
            NumericType != other.NumericType ||
            Code != other.Code ||
            Description != other.Description)
        {
            return false;
        }

        if (Metadata is null)
        {
            return other.Metadata is null;
        }

        return other.Metadata is not null && CompareMetadata(Metadata, other.Metadata);
    }

    public override int GetHashCode() =>
        Metadata is null ? HashCode.Combine(Code, Description, Type, NumericType) : ComposeHashCode();

    private int ComposeHashCode()
    {
        var hashCode = new HashCode();

        hashCode.Add(Code);
        hashCode.Add(Description);
        hashCode.Add(Type);
        hashCode.Add(NumericType);

        foreach (var (key, value) in Metadata!)
        {
            hashCode.Add(key);
            hashCode.Add(value);
        }

        return hashCode.ToHashCode();
    }

    private static bool CompareMetadata(Dictionary<string, object> metadata, Dictionary<string, object> otherMetadata)
    {
        if (ReferenceEquals(metadata, otherMetadata))
        {
            return true;
        }

        if (metadata.Count != otherMetadata.Count)
        {
            return false;
        }

        foreach (var (key, value) in metadata)
        {
            if (!otherMetadata.TryGetValue(key, out var otherValue)
                || !value.Equals(otherValue))
            {
                return false;
            }
        }

        return true;
    }
}
