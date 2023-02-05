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
            // Discard:

            SetPhase(Phase.Discard);
            gameState.DrawCardsFromDeck();
            yield return WaitToReceiveAllRoundActions();

            // Create Queue:

            SetPhase(Phase.CreateQueue);
            yield return WaitToReceiveAllRoundActions();

            // Apply Cards:

            SetPhase(Phase.ApplyCards);

            foreach (var card in gameState.InterleaveCardQueue())
            {
                yield return new WaitForSeconds(cardApplyDelaySeconds);
                gameState.hero.ApplyCard(card);
            }

            gameState.IncrementStartingPlayerIndex();
        }
    }

    bool HaveReceivedAllRoundActions()
    {
        var group = gameState.roundActions[^1];
        return group.All(networkAction => networkAction != null);
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
        Debug.Log($"Waiting to receive all round actions; index: {gameState.roundActions.Count - 1}");
        yield return new WaitUntil(HaveReceivedAllRoundActions);
    }
}
