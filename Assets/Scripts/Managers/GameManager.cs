using System.Collections;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameState gameState;
    [SerializeField] float cardApplyDelaySeconds = 1;

    [Header("Events")]
    [SerializeField] GameEvent cardsUpdatedEvent;
    [SerializeField] GameEvent phaseChangedEvent;

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

    IEnumerator GameLoop()
    {
        // TODO: break when player reaches exit
        while (true)
        {
            // Discard:

            SetPhase(Phase.Discard);
            dealer.Draw();
            yield return WaitToReceiveAllRoundActions();

            // Create Queue:

            SetPhase(Phase.CreateQueue);
            yield return WaitToReceiveAllRoundActions();

            // Apply Cards:

            SetPhase(Phase.ApplyCards);

            foreach (var card in dealer.GetInterleavedQueue())
            {
                yield return new WaitForSeconds(cardApplyDelaySeconds);
                gameState.hero.ApplyCard(card);
            }

            dealer.ClearQueues();
            IncrementStartingPlayerIndex();
        }
    }

    bool HaveReceivedAllRoundActions()
    {
        var group = gameState.roundActions[^1];
        return group.All(networkAction => networkAction != null);
    }

    void IncrementStartingPlayerIndex()
    {
        gameState.startingPlayerIndex++;
        gameState.startingPlayerIndex %= gameState.players.Length;
    }

    void SetPhase(Phase phase)
    {
        Debug.Log($"Phase: {phase}");
        gameState.phase = phase;
        phaseChangedEvent.Invoke();
    }

    IEnumerator WaitToReceiveAllRoundActions()
    {
        var group = new IRoundAction[gameState.players.Length];
        gameState.roundActions.Add(group);
        Debug.Log($"Waiting to receive all round actions; index: {gameState.roundActions.Count - 1}");
        yield return new WaitUntil(HaveReceivedAllRoundActions);
    }
}
