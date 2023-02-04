using UnityEngine;
using UnityEngine.UIElements;

public class PlayUI : MonoBehaviour
{
    [SerializeField] GameState gameState;

    Label gameCodeLabel;
    VisualElement handWrapper;

    void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        gameCodeLabel = root.Q<Label>("game-code");
        handWrapper = root.Q("hand-wrapper");
    }

    public void RefreshCardUI()
    {
        Debug.Log("Refreshing card UI.");
        handWrapper.Clear();

        foreach (var card in gameState.localHand)
        {
            handWrapper.Add(new Label(card.ToString()));
        }
    }

    public void RefreshGameCodeUI()
    {
        Debug.Log($"Refreshing game code UI for random seed {gameState.randomSeed}.");
        gameCodeLabel.text = GameCodeUtility.RandomSeedToGameCode(gameState.randomSeed);
    }
}
