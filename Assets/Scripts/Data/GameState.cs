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

    public Player localPlayer { get; set; }
    public Player remotePlayer => localPlayer == Player.Player1 ? Player.Player2 : Player.Player1;

    // Game:

    public Phase phase { get; set; }
    public Player playerWhoGoesFirst { get; set; }

    // Cards:

    public readonly Stack<Card> deck = new();
    public readonly Stack<Card> discardPile = new();
    public readonly List<Card> player1Hand = new();
    public readonly List<Card> player2Hand = new();

    public IEnumerable<Card> localHand => localPlayer == Player.Player1 ? player1Hand : player2Hand;
}
