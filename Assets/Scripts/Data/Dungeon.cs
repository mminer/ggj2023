using RogueSharp;
using RogueSharp.MapCreation;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Represents an environment (map + items).
/// </summary>
public class Dungeon
{
    public Cell this[Vector3Int position] => map[position];

    public readonly HashSet<Vector3Int> enemySpawnPositions = new();
    public readonly Vector3Int exitPosition;
    public readonly Vector3Int heroSpawnPosition;

    readonly Map<Cell> map;
    readonly RandomNumberGenerator rng;

    public Dungeon(RandomNumberGenerator rng, Rules rules)
    {
        this.rng = rng;

        var mapCreationStrategy = new CaveMapCreationStrategy<Map<Cell>, Cell>(
            rules.mapWidth,
            rules.mapHeight,
            rules.mapFillProbability,
            rules.mapTotalIterations,
            rules.mapCutoffOfBigAreaFill,
            rng);

        map = Map.Create(mapCreationStrategy);

        for (var i = 0; i < rules.enemyCount; i++)
        {
            var position = ReserveRandomSpawnPosition();
            enemySpawnPositions.Add(position);
        }

        exitPosition = ReserveRandomSpawnPosition();
        heroSpawnPosition = ReserveRandomSpawnPosition();
    }

    public override string ToString()
    {
        return map.ToString();
    }

    Vector3Int ReserveRandomSpawnPosition()
    {
        var walkableCells = map.walkableCells.ToArray();

        while (true)
        {
            var cell = walkableCells[rng.Next(walkableCells.Length)];

            if (cell.freeToSpawnOn)
            {
                cell.freeToSpawnOn = false;
                return cell.position;
            }
        }
    }
}
