using RogueSharp;
using RogueSharp.MapCreation;
using System.Linq;
using UnityEngine;

/// <summary>
/// Represents an environment (map + items).
/// </summary>
public class Dungeon
{
    readonly Map<Cell> map;
    readonly RandomNumberGenerator rng;

    public Cell this[Vector3Int position] => map[position];

    public Dungeon(RandomNumberGenerator rng, Rules rules)
    {
        var mapCreationStrategy = new RandomRoomsMapCreationStrategy<Map<Cell>, Cell>(
            rules.mapWidth,
            rules.mapHeight,
            rules.maxRooms,
            rules.roomMaxSize,
            rules.roomMinSize,
            rng);

        this.map = Map.Create(mapCreationStrategy);
        this.rng = rng;
    }

    public override string ToString()
    {
        return map.ToString();
    }

    public Cell GetRandomWalkableCell()
    {
        var walkableCells = map.walkableCells.ToArray();
        return walkableCells[rng.Next(walkableCells.Length)];
    }
}
