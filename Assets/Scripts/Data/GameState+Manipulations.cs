using System.Collections.Generic;
using System.Linq;

public partial class GameState
{
    public void AddRoundActionGroup()
    {
        var group = new IRoundAction[players.Length];
        roundActions.Add(group);
    }

    /*
    public void Discard(List<Card> hand, List<Card> cards)
    {
        Debug.Assert(cards.Count == rules.drawCount, "Must discard same number of cards drawn.");

        foreach (var card in cards)
        {
            hand.Remove(card);
            discardPile.Push(card);
        }
    }
    */

    public void DrawCardsFromDeck()
    {
        for (var i = 0; i < rules.drawCount; i++)
        {
            foreach (var playerIndex in playerOrder)
            {
                var isDeckDepleted = deck.Count == 0;

                if (isDeckDepleted)
                {
                    ShuffleDiscardPile();
                }

                var card = deck.Pop();
                players[playerIndex].hand.Add(card);
            }
        }
    }

    public void GenerateDeckAndDealHands()
    {
        GenerateDeck();
        DealHands();
    }

    public IEnumerable<Card> InterleaveCardQueue()
    {
        for (var queueIndex = 0; queueIndex < rules.queueSize; queueIndex++)
        {
            foreach (var playerIndex in playerOrder)
            {
                yield return players[playerIndex].queue[queueIndex];
            }
        }

        ClearCardQueues();
    }

    public void IncrementStartingPlayerIndex()
    {
        startingPlayerIndex = (startingPlayerIndex + 1) % players.Length;
    }

    public void RandomizeStartingPlayer()
    {
        startingPlayerIndex = rng.Next(players.Length);
    }

    public void SetRoundAction(IRoundAction roundAction, int playerIndex)
    {
        var group = roundActions[^1];
        group[playerIndex] = roundAction;
    }

    public void SetRoundActionForLocalPlayer(IRoundAction roundAction)
    {
        SetRoundAction(roundAction, localPlayerIndex);
    }

    void ClearCardQueues()
    {
        foreach (var player in players)
        {
            player.queue.Clear();
        }
    }

    void DealHands()
    {
        foreach (var player in players)
        {
            player.hand.Clear();
            player.queue.Clear();
        }

        for (var i = 0; i < rules.handSize; i++)
        {
            foreach (var player in players)
            {
                var card = deck.Pop();
                player.hand.Add(card);
            }
        }
    }

    void GenerateDeck()
    {
        deck.Clear();
        discardPile.Clear();

        var cards = rules.deckConfig
            .SelectMany(cardConfig => Enumerable.Repeat(cardConfig.card, cardConfig.count))
            .Shuffle(rng);

        deck.AddRange(cards);
    }

    void ShuffleDiscardPile()
    {
        var cards = discardPile.Shuffle(rng);
        deck.AddRange(cards);
        discardPile.Clear();
    }
}
