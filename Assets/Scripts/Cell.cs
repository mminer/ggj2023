using UnityEngine;

public class Cell : RogueSharp.Cell
{
    public Item item;
    public Vector3Int position => new(X, 0, Y);
}
