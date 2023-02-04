using UnityEngine;

/// <summary>
/// Represents a tile on the map.
/// </summary>
public class Cell : RogueSharp.Cell
{
    public Item item;
    public Vector3Int position => new(X, 0, Y);
}
