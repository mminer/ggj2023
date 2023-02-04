using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    [SerializeField] GameState gameState;

    [Header("Character Prefabs")]
    [SerializeField] Transform enemyPrefab;
    [SerializeField] Hero heroPrefab;

    [Header("Cell Prefabs")]
    [SerializeField] Transform groundPrefab;
    [SerializeField] Transform wallPrefab;

    public readonly List<Transform> enemies = new();
    public Hero hero { get; private set; }

    Dungeon dungeon;

    public void GenerateDungeon()
    {
        dungeon = new Dungeon(gameState.rng, gameState.rules);
        Debug.Log("Dungeon:\n" + dungeon);

        // Spawn cell prefabs:

        for (var x = 0; x < gameState.rules.mapWidth; x++)
        {
            for (var z = 0; z < gameState.rules.mapHeight; z++)
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

        for (var i = 0; i < gameState.rules.enemyCount; i++)
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
        hero.gameState = gameState;
    }
}
