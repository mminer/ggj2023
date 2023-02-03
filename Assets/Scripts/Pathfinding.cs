using System;
using System.Linq;
using System.Collections.Generic;

public static class Pathfinding
{
    /// <summary>
    /// A* implementation. Finds a path from start to goal.
    /// </summary>
    /// <param name="start">Where to begin.</param>
    /// <param name="goal">Where we want to end up.</param>
    /// <param name="getCostEstimate">Heuristic function that estimates the cost to reach the goal from node n.</param>
    /// <param name="getEdgeWeight">Gets the weight of the edge from current to neighbor.</param>
    /// <param name="getNeighbors">Gets nodes adjacent to the given one.</param>
    /// <returns>Shortest path between the two nodes, or null if no path exists.</returns>
    public static List<T> FindShortestPath<T>(
        T start,
        T goal,
        Func<T, int> getCostEstimate,
        Func<T, T, int> getEdgeWeight,
        Func<T, IEnumerable<T>> getNeighbors) where T : IEquatable<T>
    {
        // Adapted from https://en.wikipedia.org/wiki/A*_search_algorithm#Pseudocode.
        // I translated the pseudocode as directly as possible and kept variable names the same.
        // Except for h and d, which are terrible names. I renamed them to getCostEstimate and getEdgeWeight respectively.

        // The set of discovered nodes that may need to be (re-)expanded.
        // Initially, only the start node is known.
        // This is usually implemented as a min-heap or priority queue rather than a hash-set.
        var openSet = new HashSet<T> {start};

        // For node n, cameFrom[n] is the node immediately preceding it on the cheapest path from start to n currently known.
        var cameFrom = new Dictionary<T, T>();

        // For node n, gScore[n] is the cost of the cheapest path from start to n currently known.
        var gScore = new Dictionary<T, int>();
        gScore[start] = 0;

        // For node n, fScore[n] = gScore[n] + getCostEstimate(n).
        // fScore[n] represents our current best guess as to how short a path from start to finish can be if it goes through n.
        var fScore = new Dictionary<T, int>();
        fScore[start] = getCostEstimate(start);

        while (openSet.Count > 0)
        {
            // Find the node in openSet with the lowest fScore value.
            // This operation can occur in O(1) time if openSet is a min-heap or a priority queue.
            var current = openSet
                .OrderBy(position => fScore.TryGetValue(position, out var fValue) ? fValue : int.MaxValue)
                .First();

            if (current.Equals(goal))
            {
                return ReconstructPath(cameFrom, current);
            }

            openSet.Remove(current);
            var neighbors = getNeighbors(current);

            foreach (var neighbor in neighbors)
            {
                // tentativeGScore is the distance from start to the neighbor through current.
                var tentativeGScore = gScore[current]+ getEdgeWeight(current, neighbor);
                var neighborGScore = gScore.TryGetValue(neighbor, out var gValue) ? gValue : int.MaxValue;

                if (tentativeGScore < neighborGScore)
                {
                    // This path to neighbor is better than any previous one. Record it!
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + getCostEstimate(neighbor);

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        // Open set is empty but goal was never reached.
        return null;
    }

    static List<T> ReconstructPath<T>(Dictionary<T, T> cameFrom, T current)
    {
        var totalPath = new List<T> {current};

        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            totalPath.Insert(0, current);
        }

        return totalPath;
    }
}
