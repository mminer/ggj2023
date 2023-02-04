using UnityEngine;

/// <summary>
/// Functions that don't fit anywhere else.
/// </summary>
public static class MiscUtility
{
    static readonly Vector3Int[] directions =
    {
        Vector3Int.back,
        Vector3Int.forward,
        Vector3Int.left,
        Vector3Int.right,
    };

    public static Vector3Int GetRandomDirection(RandomNumberGenerator rng)
    {
        return directions[rng.Next(0, 4)];
    }
}
