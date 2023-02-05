using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayUI : MonoBehaviour
{
    [SerializeField] GameState gameState;
    [SerializeField] GameEvent roundActionSubmittedEvent;

    VisualElement root;

    Button discardButton;
    VisualElement discardChoicesContainer;
    VisualElement discardPhaseContainer;
    Label gameCodeLabel;
    VisualElement handContainer;
    Label phaseLabel;
    VisualElement queueChoicesContainer;
    VisualElement queuePhaseContainer;
    Button queueSubmitButton;

    void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        discardButton = root.Q<Button>("discard-button");
        discardButton.clicked += HandleDiscardButtonClick;
        discardButton.SetEnabled(false);

        discardChoicesContainer = root.Q("discard-choices-container");
        discardPhaseContainer = root.Q("discard-phase-container");
        gameCodeLabel = root.Q<Label>("game-code-label");
        handContainer = root.Q("hand-container");
        phaseLabel = root.Q<Label>("phase-label");
        queueChoicesContainer = root.Q("queue-choices-container");

        queuePhaseContainer = root.Q("queue-phase-container");
        queuePhaseContainer.visible = false;

        queueSubmitButton = root.Q<Button>("queue-submit-button");
        queueSubmitButton.SetEnabled(false);
        queueSubmitButton.clicked += HandleSubmitQueueButtonClick;
    }

    public void RefreshCardUI()
    {
        Debug.Log("Refreshing card UI.");

        discardChoicesContainer.Clear();
        handContainer.Clear();
        queueChoicesContainer.Clear();

        foreach (var card in gameState.localPlayer.hand)
        {
            var cardButton = new Button
            {
                text = card.ToString(),
                userData = card,
            };

            cardButton.clicked += () => MoveCardBetweenHandAndChoicesContainer(cardButton);
            handContainer.Add(cardButton);
        }
    }

    public void RefreshGameCodeUI()
    {
        Debug.Log($"Refreshing game code UI for random seed {gameState.randomSeed}.");
        gameCodeLabel.text = GameCodeUtility.RandomSeedToGameCode(gameState.randomSeed);
    }

    public void RefreshPhaseUI()
    {
        phaseLabel.text = $"Phase: {gameState.phase}";
        discardPhaseContainer.visible = gameState.phase == Phase.Discard;
        queuePhaseContainer.visible = gameState.phase == Phase.Queue;
        UnfreezeUI();
    }

    void HandleDiscardButtonClick()
    {
        Debug.Assert(discardChoicesContainer.childCount == gameState.rules.drawCount);

        var cards = discardChoicesContainer
            .Children()
            .Select(child => (Card)child.userData)
            .ToArray();

        var roundAction = new RoundAction_Discard(gameState.localPlayerIndex, cards);
        gameState.SetRoundAction(roundAction);
        roundActionSubmittedEvent.Invoke();

        discardButton.SetEnabled(false);
        FreezeUI();
    }

    void FreezeUI()
    {
        root.SetEnabled(false);
    }

    void HandleSubmitQueueButtonClick()
    {
        Debug.Assert(queueChoicesContainer.childCount == gameState.rules.queueSize);

        var cards = queueChoicesContainer
            .Children()
            .Select(child => (Card)child.userData)
            .ToArray();

        var roundAction = new RoundAction_SubmitQueue(gameState.localPlayerIndex, cards);
        gameState.SetRoundAction(roundAction);
        roundActionSubmittedEvent.Invoke();

        queueSubmitButton.SetEnabled(false);
        FreezeUI();
    }

    void MoveCardBetweenHandAndChoicesContainer(Button cardButton)
    {
        VisualElement newContainer;

        if (handContainer.Contains(cardButton))
        {
            newContainer = gameState.phase switch
            {
                Phase.Discard => discardChoicesContainer,
                Phase.Queue => queueChoicesContainer,
                _ => throw new ArgumentOutOfRangeException(),
            };
        }
        else
        {
            newContainer = handContainer;
        }

        cardButton.RemoveFromHierarchy();
        newContainer.Add(cardButton);
        RefreshButtonEnabledStates();
    }

    void RefreshButtonEnabledStates()
    {
        discardButton.SetEnabled(discardChoicesContainer.childCount == gameState.rules.drawCount);
        queueSubmitButton.SetEnabled(queueChoicesContainer.childCount == gameState.rules.queueSize);
    }

    void UnfreezeUI()
    {
        root.SetEnabled(true);
    }
}
