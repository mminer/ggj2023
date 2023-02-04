using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameState gameState;
    [SerializeField] GameEvent cardsUpdatedEvent;

    public void ShuffleDeckAndDealHands()
    {
        // Reset card state. In normal play it won't be necessary to do this, but if invoke the game start event
        // manually (e.g. through the inspector) we need to clear the previous game's data.
        gameState.deck.Clear();
        gameState.discardPile.Clear();
        gameState.player1Hand.Clear();
        gameState.player2Hand.Clear();

        // Generate deck from rules:

        var cards = gameState.rules.deckConfig
            .SelectMany(cardConfig => Enumerable.Repeat(cardConfig.card, cardConfig.count))
            .Shuffle(gameState.rng);

        gameState.deck.AddRange(cards);

        // Deal player hands:

        for (var i = 0; i < gameState.rules.handSize; i++)
        {
            gameState.player1Hand.Add(gameState.deck.Pop());
            gameState.player2Hand.Add(gameState.deck.Pop());
        }

        gameState.playerThatGoesFirst = gameState.rng.NextBool() ? Player.Player1 : Player.Player2;
        cardsUpdatedEvent.Invoke();
    }

    public void Discard(List<Card> hand, List<Card> cards)
    {
        Debug.Assert(cards.Count == gameState.rules.drawCount, "Must discard same number of cards drawn.");

        foreach (var card in cards)
        {
            hand.Remove(card);
            gameState.discardPile.Push(card);
        }

        cardsUpdatedEvent.Invoke();
    }

    public void Draw(List<Card> hand)
    {
        for (var i = 0; i < gameState.rules.drawCount; i++)
        {
            if (gameState.deck.Count == 0)
            {
                ShuffleDiscardPile();
            }

            hand.Add(gameState.deck.Pop());
        }

        cardsUpdatedEvent.Invoke();
    }

    public void NextRound()
    {
        gameState.playerThatGoesFirst = gameState.playerThatGoesFirst == Player.Player1
            ? Player.Player2
            : Player.Player1;
    }

    void ShuffleDiscardPile()
    {
        var cards = gameState.discardPile.Shuffle(gameState.rng);
        gameState.deck.AddRange(cards);
        gameState.discardPile.Clear();
    }
}
