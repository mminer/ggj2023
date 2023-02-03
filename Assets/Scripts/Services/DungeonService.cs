using UnityEngine;

public class DungeonService : Services.Service
{
    [SerializeField] int seed = 42;

    [Header("Map")]
    [SerializeField] int width = 32;
    [SerializeField] int height = 32;
    [SerializeField] int maxRooms = 30;
    [SerializeField] int roomMaxSize = 10;
    [SerializeField] int roomMinSize = 3;

    Dungeon dungeon;

    public void GenerateDungeon()
    {
        dungeon = new Dungeon(seed, width, height, maxRooms, roomMaxSize, roomMinSize);
    }
}
