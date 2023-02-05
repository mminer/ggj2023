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
    class Cell : RogueSharp.Cell
    {
        public Vector3Int position => new(X, 0, Y);
    }

    public readonly List<Vector3Int> enemySpawnPositions = new();
    public readonly Vector3Int exitPosition;
    public readonly Vector3Int heroSpawnPosition;

    readonly Map<Cell> map;
    readonly RandomNumberGenerator rng;
    readonly PathFinder<Cell> pathFinder;

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
        pathFinder = new PathFinder<Cell>(map);

        // Reserve spawn positions.
        var walkablePositions = GetRandomWalkablePositions(rules.enemyCount + 2);
        enemySpawnPositions.AddRange(walkablePositions.Take(rules.enemyCount));
        exitPosition = walkablePositions[^2];
        heroSpawnPosition = walkablePositions[^1];
    }

    public IEnumerable<Vector3Int> GetAdjacentPositions(Vector3Int position, bool includeDiagonals = false)
    {
        return map.GetAdjacentCells(position, includeDiagonals).Select(cell => cell.position);
    }

    public bool IsWalkable(Vector3Int position)
    {
        return map[position].IsWalkable;
    }

    public bool TryGetPath(Vector3Int start, Vector3Int finish, out IEnumerable<Vector3Int> path)
    {
        var shortestPath = pathFinder.TryFindShortestPath(map[start], map[finish]);

        if (shortestPath == null)
        {
            path = Enumerable.Empty<Vector3Int>();
            return false;
        }

        path = shortestPath.Steps
            .Cast<Cell>()
            .Select(cell => cell.position)
            .Skip(1);

        return true;
    }

    public override string ToString()
    {
        return map.ToString();
    }

    Vector3Int[] GetRandomWalkablePositions(int amount)
    {
        var positions = new HashSet<Vector3Int>();
        var walkableCells = map.walkableCells.ToArray();

        for (var i = 0; i < amount; i++)
        {
            while (true)
            {
                var cell = walkableCells[rng.Next(walkableCells.Length)];

                if (positions.Add(cell.position))
                {
                    break;
                }
            }
        }

        return positions.ToArray();
    }
}
