using System.Collections.Generic;
using System.Linq;

public static class EnumerableExtensions
{
    /// <summary>
    /// Fisher-Yates shuffle.
    /// Adapted from https://stackoverflow.com/a/5807238
    /// </summary>
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, RandomNumberGenerator rng)
    {
        var buffer = source.ToList();

        for (var i = 0; i < buffer.Count; i++)
        {
            var j = rng.Next(i, buffer.Count);
            yield return buffer[j];
            buffer[j] = buffer[i];
        }
    }
}
