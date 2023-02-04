using System;
using System.Collections.Generic;
using System.Linq;
using RogueSharp;
using UnityEngine;

/// <summary>
/// Subclass of <see cref="RogueSharp.Map{TCell}"/> that translates XY to XZ orientation.
/// </summary>
public class Map<TCell> : RogueSharp.Map<TCell> where TCell : ICell
{
    public IEnumerable<TCell> walkableCells => GetAllCells().Where(cell => cell.IsWalkable);

    public TCell this[Vector3Int position] => this[position.x, position.z];

    public override string ToString()
    {
        // RogueSharp's map origin starts in the top-left while Unity's starts at the bottom-left,
        // so we need to flip its output to see a representation that will match a tilemap.
        var rows = base.ToString().Split(Environment.NewLine).Reverse();
        return string.Join(Environment.NewLine, rows);
    }

    public bool IsTransparent(Vector3Int position) => IsTransparent(position.x, position.z);
    public bool IsWalkable(Vector3Int position) => IsWalkable(position.x, position.z);
    public void SetCellProperties(Vector3Int position, bool isTransparent, bool isWalkable) => SetCellProperties(position.x, position.z, isTransparent, isWalkable);
    public IEnumerable<TCell> GetCellsAlongLine(Vector3Int origin, Vector3Int destination) => GetCellsAlongLine(origin.x, origin.z, destination.x, destination.z);
    public IEnumerable<TCell> GetCellsInCircle(Vector3Int center, int radius) => GetCellsInCircle(center.x, center.z, radius);
    public IEnumerable<TCell> GetCellsInDiamond(Vector3Int center, int distance) => GetCellsInDiamond(center.x, center.z, distance);
    public IEnumerable<TCell> GetCellsInSquare(Vector3Int center, int distance) => GetCellsInSquare(center.x, center.z, distance);
    public IEnumerable<TCell> GetCellsInRectangle(Vector3Int topLeft, int width, int height) => GetCellsInRectangle(topLeft.x, topLeft.z, width, height);
    public IEnumerable<TCell> GetBorderCellsInCircle(Vector3Int center, int radius) => GetBorderCellsInCircle(center.x, center.z, radius);
    public IEnumerable<TCell> GetBorderCellsInDiamond(Vector3Int center, int distance) => GetBorderCellsInDiamond(center.x, center.z, distance);
    public IEnumerable<TCell> GetBorderCellsInSquare(Vector3Int center, int distance) => GetBorderCellsInSquare(center.x, center.z, distance);
    public IEnumerable<TCell> GetAdjacentCells(Vector3Int center) => GetAdjacentCells(center.x, center.z);
    public IEnumerable<TCell> GetAdjacentCells(Vector3Int center, bool includeDiagonals) => GetAdjacentCells(center.x, center.z, includeDiagonals);
    public TCell GetCell(Vector3Int position) => GetCell(position.x, position.z);
    public int IndexFor(Vector3Int position) => IndexFor(position.x, position.z);
}
