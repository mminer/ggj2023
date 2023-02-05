using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameState gameState;
    [SerializeField] GameEvent cardsUpdatedEvent;

    Dealer dealer;

    void Awake()
    {
        dealer = new Dealer(gameState);
    }

    public void StartGame()
    {
        dealer.GenerateDeckAndDealHands();
        cardsUpdatedEvent.Invoke();
        gameState.startingPlayerIndex = gameState.rng.Next(gameState.players.Length);
        StartCoroutine(GameLoop());
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

    IEnumerator GameLoop()
    {
        // TODO: break when player reaches exit
        while (true)
        {
            // TODO: handle pickup / discard phase

            Debug.Log("Game loop: waiting for players to submit card queues.");
            yield return new WaitUntil(() => gameState.players.All(player => player.queue.Count > 0));

            Debug.Log("Game loop: applying cards.");

            var interleavedQueue = dealer.GetInterleavedQueue();

            foreach (var card in interleavedQueue)
            {
                yield return new WaitForSeconds(1);
                gameState.hero.ApplyCard(card);
            }

            dealer.ClearQueues();
            NextStartingPlayer();
        }
    }

    void NextStartingPlayer()
    {
        gameState.startingPlayerIndex = (gameState.startingPlayerIndex + 1) % gameState.players.Length;
    }

    void ShuffleDiscardPile()
    {
        var cards = gameState.discardPile.Shuffle(gameState.rng);
        gameState.deck.AddRange(cards);
        gameState.discardPile.Clear();
    }
}
