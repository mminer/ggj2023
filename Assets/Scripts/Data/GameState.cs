using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameState : ScriptableObject
{
    [SerializeField] public Rules rules;

    // Cards:

    public readonly Stack<Card> deck = new();
    public readonly Stack<Card> discardPile = new();
    public readonly List<Card> player1Hand = new();
    public readonly List<Card> player2Hand = new();

    public IEnumerable<Card> localHand => localPlayer switch
    {
        Player.Player1 => player1Hand,
        Player.Player2 => player2Hand,
        _ => Array.Empty<Card>(),
    };

    // Players:

    public Player? localPlayer { get; set; }

    public Player? remotePlayer => localPlayer switch
    {
        Player.Player1 => Player.Player2,
        Player.Player2 => Player.Player1,
        _ => null,
    };

    public Player? playerThatGoesFirst { get; set; }

    // Random Number Generation:

    public int randomSeed
    {
        get => rng?.randomSeed ?? -1;
        set => rng = new RandomNumberGenerator(value);
    }

    public RandomNumberGenerator rng { get; private set; }
}
