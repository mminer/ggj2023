using UnityEngine;

public static class Util
{
    static readonly Vector3Int[] directions =
    {
        Vector3Int.back,
        Vector3Int.forward,
        Vector3Int.left,
        Vector3Int.right,
    };

    public static Vector3Int GetRandomDirection()
    {
        return directions[Random.Range(0, 4)];
    }

}
