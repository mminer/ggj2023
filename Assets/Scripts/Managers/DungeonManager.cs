using System.Linq;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    [SerializeField] GameState gameState;

    [Header("Character Prefabs")]
    [SerializeField] Enemy enemyPrefab;
    [SerializeField] Hero heroPrefab;

    [Header("Cell Prefabs")]
    [SerializeField] Transform exitPrefab;
    [SerializeField] Transform groundPrefab;
    [SerializeField] Transform wallPrefab;

    public void GenerateDungeon()
    {
        // Clear existing dungeon objects.
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        gameState.dungeon = new Dungeon(gameState.rng, gameState.rules);
        Debug.Log("Dungeon:\n" + gameState.dungeon);

        // Spawn cell prefabs:

        for (var x = 0; x < gameState.rules.mapWidth; x++)
        {
            for (var z = 0; z < gameState.rules.mapHeight; z++)
            {
                var position = new Vector3Int(x, 0, z);

                if (!gameState.dungeon.IsWalkable(position))
                {
                    Instantiate(wallPrefab, position, Quaternion.identity, transform);
                }

                Instantiate(groundPrefab, position, Quaternion.identity, transform);
            }
        }

        Instantiate(exitPrefab, gameState.dungeon.exitPosition, Quaternion.identity, transform);

        // Spawn enemies:

        gameState.enemies.Clear();

        foreach (var position in gameState.dungeon.enemySpawnPositions)
        {
            var enemy = Instantiate(enemyPrefab, position, Quaternion.identity, transform);
            gameState.enemies.Add(enemy);
        }

        // Spawn hero:

        gameState.hero = Instantiate(heroPrefab, gameState.dungeon.heroSpawnPosition, Quaternion.identity, transform);
    }
}
