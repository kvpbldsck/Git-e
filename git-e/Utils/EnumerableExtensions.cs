namespace GitE.Utils;

public static class EnumerableExtensions
{
    public static string JoinAsString<T>(this IEnumerable<T> source, string separator = ", ") =>
        string.Join(separator, source);
}
