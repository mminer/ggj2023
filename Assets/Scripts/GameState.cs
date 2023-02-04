using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class GameState : ScriptableObject
{
    public Rules rules;
    public int randomSeed;

    public Player? localPlayer;
    public Phase player1Phase;
    public Phase player2Phase;

    public Stack<Card> deck { get; private set; }
    public readonly List<Card> player1Hand = new();
    public readonly List<Card> player2Hand = new();

    public void Init()
    {
        // Generate deck from rules:

        var cards = rules.deckConfig
            .SelectMany(cardConfig => Enumerable.Repeat(cardConfig.card, cardConfig.count))
            .Shuffle();

        deck = new Stack<Card>(cards);

        // Deal player hands:

        player1Hand.Clear();
        player2Hand.Clear();

        for (var i = 0; i < rules.handSize; i++)
        {
            player1Hand.Add(deck.Pop());
            player2Hand.Add(deck.Pop());
        }
    }
}
