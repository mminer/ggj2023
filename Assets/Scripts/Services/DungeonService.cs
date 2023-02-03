using System.Collections.Generic;
using UnityEngine;

public class DungeonService : Services.Service
{
    [SerializeField] int enemyCount = 4;
    [SerializeField] int randomSeed = 42;

    [Header("Map")]
    [SerializeField] int width = 32;
    [SerializeField] int height = 32;
    [SerializeField] int maxRooms = 30;
    [SerializeField] int roomMaxSize = 10;
    [SerializeField] int roomMinSize = 3;

    [Header("Character Prefabs")]
    [SerializeField] Transform enemyPrefab;
    [SerializeField] Transform heroPrefab;

    [Header("Cell Prefabs")]
    [SerializeField] Transform groundPrefab;
    [SerializeField] Transform wallPrefab;

    public readonly List<Transform> enemies = new();
    public Transform hero { get; private set; }

    Dungeon dungeon;

    public void GenerateDungeon()
    {
        dungeon = new Dungeon(randomSeed, width, height, maxRooms, roomMaxSize, roomMinSize);
        Debug.Log("Dungeon:\n" + dungeon);

        // Spawn cell prefabs:

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

        // Spawn enemies:

        var enemyPositions = new HashSet<Vector3Int>();

        for (var i = 0; i < enemyCount; i++)
        {
            Cell enemyCell;

            // Avoid spawning enemy in same cell as other enemy.
            do
            {
                enemyCell = dungeon.GetRandomWalkableCell();
            } while (enemyPositions.Contains(enemyCell.position));

            var enemy = Instantiate(enemyPrefab, enemyCell.position, Quaternion.identity, transform);
            enemies.Add(enemy);
            enemyPositions.Add(enemyCell.position);
        }

        // Spawn hero:

        Cell heroCell;

        // Avoid spawning hero in same cell as enemy.
        do
        {
            heroCell = dungeon.GetRandomWalkableCell();
        } while (enemyPositions.Contains(heroCell.position));

        hero = Instantiate(heroPrefab, heroCell.position, Quaternion.identity, transform);
    }
}
