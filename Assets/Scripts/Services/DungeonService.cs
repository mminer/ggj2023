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

    [Header("Cell Prefabs")]
    [SerializeField] Transform groundPrefab;
    [SerializeField] Transform wallPrefab;

    Dungeon dungeon;

    public void GenerateDungeon()
    {
        dungeon = new Dungeon(seed, width, height, maxRooms, roomMaxSize, roomMinSize);
        Debug.Log("Dungeon:\n" + dungeon);

        // Instantiate cell prefabs.
        for (var x = 0; x < width; x++)
        {
            for (var z = 0; z < height; z++)
            {
                var position = new Vector3Int(x, 0, z);
                var cell = dungeon[position];

                var prefab = cell switch
                {
                    { IsWalkable: true } => groundPrefab,
                    _ => wallPrefab,
                };

                Instantiate(prefab, position, Quaternion.identity, transform);
            }
        }
    }
}
