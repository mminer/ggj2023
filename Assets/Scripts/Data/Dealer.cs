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
}
