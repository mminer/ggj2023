using RogueSharp;
using RogueSharp.MapCreation;
using UnityEngine;

/// <summary>
/// Represents an environment (map + items).
/// </summary>
public class Dungeon
{
    public class Cell : RogueSharp.Cell
    {
        public Vector3Int Position => new(X, 0, Y);
    }

    readonly Map<Cell> map;
    readonly RandomNumberGenerator rng;

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
        Debug.Log("Dungeon map:\n" + map);
    }
}
