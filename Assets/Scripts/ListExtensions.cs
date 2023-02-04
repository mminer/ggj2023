using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    /// <summary>
    /// In-place Fisher-Yates shuffle.
    /// https://en.wikipedia.org/wiki/Fisher–Yates_shuffle#The_modern_algorithm
    /// </summary>
    public static void Shuffle<T>(this IList<T> list)
    {
        for (var i = list.Count - 1; i >= 1; i--)
        {
            var j = Random.Range(0, i + 1);
            (list[j], list[i]) = (list[i], list[j]);
        }
    }
}
