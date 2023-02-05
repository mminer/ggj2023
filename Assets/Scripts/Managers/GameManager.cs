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
            AddRoundActionGroup();
            yield return new WaitUntil(HaveReceivedAllRoundActions);

            // Create Queue:

            SetPhase(Phase.CreateQueue);
            AddRoundActionGroup();
            yield return new WaitUntil(HaveReceivedAllRoundActions);

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

    void AddRoundActionGroup()
    {
        var group = new IRoundAction[gameState.players.Length];
        gameState.roundActions.Add(group);
        Debug.Log($"Round action index: {gameState.roundActions.Count - 1}");
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
}
