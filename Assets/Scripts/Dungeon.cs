using RogueSharp;
using RogueSharp.MapCreation;
using UnityEngine;

/// <summary>
/// Represents an environment (map + items).
/// </summary>
public class Dungeon
{
    readonly Map<Cell> map;
    readonly RandomNumberGenerator rng;

    public Cell this[Vector3Int position] => map[position];

    public Dungeon(int seed, int width, int height, int maxRooms, int roomMaxSize, int roomMinSize)
    {
        rng = new RandomNumberGenerator(seed);

        var mapCreationStrategy = new RandomRoomsMapCreationStrategy<Map<Cell>, Cell>(
            width,
            height,
            maxRooms,
            roomMaxSize,
            roomMinSize,
            rng);

        map = Map.Create(mapCreationStrategy);
    }

    public override string ToString()
    {
        return map.ToString();
    }
}
