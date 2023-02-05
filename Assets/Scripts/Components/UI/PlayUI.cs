using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayUI : MonoBehaviour
{
    [SerializeField] GameState gameState;

    Label gameCodeLabel;
    VisualElement handContainer;
    VisualElement queueContainer;
    Button submitButton;

    void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        gameCodeLabel = root.Q<Label>("game-code");
        handContainer = root.Q("hand-container");
        queueContainer = root.Q("queue-container");
        submitButton = root.Q<Button>("submit");

        submitButton.clicked += () =>
        {
            Debug.Assert(queueContainer.childCount == gameState.rules.queueSize);

            var queue = queueContainer
                .Children()
                .Select(child => (Card)child.userData)
                .ToArray();

            var roundAction = new RoundAction_SubmitQueue(gameState.localPlayerIndex, queue);
            gameState.SetRoundAction(roundAction);
        };

        submitButton.SetEnabled(false);
    }

    public void RefreshCardUI()
    {
        Debug.Log("Refreshing card UI.");
        handContainer.Clear();

        foreach (var card in gameState.localPlayer.hand)
        {
            var cardButton = new Button
            {
                text = card.ToString(),
                userData = card,
            };

            cardButton.clicked += () =>
            {
                var newContainer = handContainer.Contains(cardButton)
                    ? queueContainer
                    : handContainer;

                cardButton.RemoveFromHierarchy();
                newContainer.Add(cardButton);
                submitButton.SetEnabled(queueContainer.childCount == gameState.rules.queueSize);
            };

            handContainer.Add(cardButton);
        }
    }

    public void RefreshGameCodeUI()
    {
        Debug.Log($"Refreshing game code UI for random seed {gameState.randomSeed}.");
        gameCodeLabel.text = GameCodeUtility.RandomSeedToGameCode(gameState.randomSeed);
    }
}
