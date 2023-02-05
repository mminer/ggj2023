using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameState gameState;
    [SerializeField] float cardApplyDelaySeconds = 1;

    [Header("Events")]
    [SerializeField] GameEvent cardsUpdatedEvent;
    [SerializeField] GameEvent phaseChangedEvent;

    public void StartGame()
    {
        gameState.RandomizeStartingPlayer();
        gameState.GenerateDeckAndDealHands();
        cardsUpdatedEvent.Invoke();
        StartCoroutine(GameLoop());
    }

    IEnumerator GameLoop()
    {
        // TODO: break when player reaches exit
        while (true)
        {
            // Draw and discard:

            gameState.DrawCardsFromDeck();
            SetPhase(Phase.Discard);
            yield return WaitToReceiveAllRoundActions();

            // Resolve discards:

            foreach (var playerIndex in gameState.playerOrder)
            {
                var roundAction = (RoundAction_Discard)gameState.latestRoundActionGroup[playerIndex];
                gameState.Discard(playerIndex, roundAction.cards);
            }

            // Create queue:

            SetPhase(Phase.CreateQueue);
            yield return WaitToReceiveAllRoundActions();

            // Interleave card queues and apply to hero:

            SetPhase(Phase.ApplyCards);

            for (var queueIndex = 0; queueIndex < gameState.rules.queueSize; queueIndex++)
            {
                foreach (var playerIndex in gameState.playerOrder)
                {
                    yield return new WaitForSeconds(cardApplyDelaySeconds);
                    var submitQueueAction = (RoundAction_SubmitQueue)gameState.latestRoundActionGroup[playerIndex];
                    var card = submitQueueAction.cards[queueIndex];
                    gameState.hero.ApplyCard(card);
                }
            }

            // Prepare for next round:

            gameState.IncrementStartingPlayerIndex();
        }
    }

    bool IsLatestRoundActionGroupComplete()
    {
        foreach (var roundAction in gameState.latestRoundActionGroup)
        {
            if (roundAction == null)
            {
                return false;
            }
        }

        return true;
    }

    void SetPhase(Phase phase)
    {
        Debug.Log($"Phase: {phase}");
        gameState.phase = phase;
        phaseChangedEvent.Invoke();
    }

    IEnumerator WaitToReceiveAllRoundActions()
    {
        gameState.AddRoundActionGroup();
        Debug.Log($"Waiting to receive all round actions for group: {gameState.roundActions.Count - 1}");
        yield return new WaitUntil(IsLatestRoundActionGroupComplete);
    }
}
