using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class GameState : ScriptableObject
{
    [SerializeField] public Rules rules;
    [SerializeField] GameEvent cardsUpdatedEvent;

    public int randomSeed
    {
        get => rng.randomSeed;
        set => rng = new RandomNumberGenerator(value);
    }

    public Player? localPlayer { get; set; }

    public Player? remotePlayer => localPlayer switch
    {
        Player.Player1 => Player.Player2,
        Player.Player2 => Player.Player1,
        _ => null,
    };

    public Player? playerThatGoesFirst { get; set; }

    public IEnumerable<Card> localHand => localPlayer switch
    {
        Player.Player1 => player1Hand,
        Player.Player2 => player2Hand,
        _ => Array.Empty<Card>(),
    };

    public Phase player1Phase;
    public Phase player2Phase;

    public readonly Stack<Card> deck = new();
    public readonly Stack<Card> discardPile = new();
    public readonly List<Card> player1Hand = new();
    public readonly List<Card> player2Hand = new();

    public RandomNumberGenerator rng { get; private set; }

    public void Init()
    {
        deck.Clear();
        discardPile.Clear();
        player1Hand.Clear();
        player2Hand.Clear();

        playerThatGoesFirst = rng.NextBool() ? Player.Player1 : Player.Player2;

        // Generate deck from rules:

        var cards = rules.deckConfig
            .SelectMany(cardConfig => Enumerable.Repeat(cardConfig.card, cardConfig.count))
            .Shuffle(rng);

        deck.AddRange(cards);

        // Deal player hands:

        for (var i = 0; i < rules.handSize; i++)
        {
            player1Hand.Add(deck.Pop());
            player2Hand.Add(deck.Pop());
        }

        cardsUpdatedEvent.Invoke();
    }

    public void Discard(List<Card> hand, List<Card> cards)
    {
        Debug.Assert(cards.Count == rules.drawCount, "Must discard same number of cards drawn.");

        foreach (var card in cards)
        {
            hand.Remove(card);
            discardPile.Push(card);
        }

        cardsUpdatedEvent.Invoke();
    }

    public void Draw(List<Card> hand)
    {
        for (var i = 0; i < rules.drawCount; i++)
        {
            if (deck.Count == 0)
            {
                ShuffleDiscardPile();
            }

            hand.Add(deck.Pop());
        }

        cardsUpdatedEvent.Invoke();
    }

    public void NextRound()
    {
        playerThatGoesFirst = playerThatGoesFirst == Player.Player1 ? Player.Player2 : Player.Player1;
    }

    void ShuffleDiscardPile()
    {
        var cards = discardPile.Shuffle(rng);
        deck.AddRange(cards);
        discardPile.Clear();
    }
}
