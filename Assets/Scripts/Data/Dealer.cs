using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Operates on the deck of cards.
/// </summary>
public class Dealer
{
    readonly GameState gameState;

    public Dealer(GameState gameState)
    {
        this.gameState = gameState;
    }

    public void ClearQueues()
    {
        foreach (var player in gameState.players)
        {
            player.queue.Clear();
        }
    }

    /*
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
    */

    public void Draw()
    {
        for (var i = 0; i < gameState.rules.drawCount; i++)
        {
            foreach (var playerIndex in gameState.playerOrder)
            {
                if (gameState.deck.Count == 0)
                {
                    ShuffleDiscardPile();
                }

                var card = gameState.deck.Pop();
                gameState.players[playerIndex].hand.Add(card);
            }
        }
    }

    public void GenerateDeckAndDealHands()
    {
        GenerateDeck();
        DealHands();
    }

    public IEnumerable<Card> GetInterleavedQueue()
    {
        for (var queueIndex = 0; queueIndex < gameState.rules.queueSize; queueIndex++)
        {
            foreach (var playerIndex in gameState.playerOrder)
            {
                yield return gameState.players[playerIndex].queue[queueIndex];
            }
        }
    }

    void DealHands()
    {
        foreach (var player in gameState.players)
        {
            player.hand.Clear();
            player.queue.Clear();
        }

        for (var i = 0; i < gameState.rules.handSize; i++)
        {
            foreach (var player in gameState.players)
            {
                var card = gameState.deck.Pop();
                player.hand.Add(card);
            }
        }
    }

    void GenerateDeck()
    {
        gameState.deck.Clear();
        gameState.discardPile.Clear();

        var cards = gameState.rules.deckConfig
            .SelectMany(cardConfig => Enumerable.Repeat(cardConfig.card, cardConfig.count))
            .Shuffle(gameState.rng);

        gameState.deck.AddRange(cards);
    }

    void ShuffleDiscardPile()
    {
        var cards = gameState.discardPile.Shuffle(gameState.rng);
        gameState.deck.AddRange(cards);
        gameState.discardPile.Clear();
    }
}
