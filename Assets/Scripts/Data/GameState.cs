using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameState : ScriptableObject
{
    [SerializeField] public Rules rules;

    // Random Number Generation:

    public int randomSeed
    {
        get => rng?.randomSeed ?? -1;
        set => rng = new RandomNumberGenerator(value);
    }

    public RandomNumberGenerator rng { get; private set; }

    // Players:

    public readonly Player[] players = { new(0), new(1) };
    public int localPlayerIndex { get; set; }
    public Player localPlayer => players[localPlayerIndex];

    // Game:

    public Dungeon dungeon { get; set; }
    public readonly List<Transform> enemies = new();
    public Hero hero { get; set; }
    public Phase phase { get; set; }
    public int startingPlayerIndex;

    public IEnumerable<int> playerOrder
    {
        get
        {
            for (var i = 0; i < players.Length; i++)
            {
                yield return (i + startingPlayerIndex) % players.Length;
            }
        }
    }

    // Cards:

    public readonly Stack<Card> deck = new();
    public readonly Stack<Card> discardPile = new();
}
