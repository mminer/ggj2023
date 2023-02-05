using UnityEngine;
using UnityEngine.UIElements;

public class StartUI : MonoBehaviour
{
    [SerializeField] GameState gameState;
    [SerializeField] GameEvent gameStartEvent;

    void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        root.Q<Button>("start-game").clicked += () =>
        {
            // This is the only time outside RandomNumberGenerator we should use UnityEngine.Random directly.
            gameState.randomSeed = Random.Range(0, Rules.maxRandomSeed);
            gameState.localPlayerIndex = 0;
            gameStartEvent.Invoke();
        };

        root.Q<Button>("join-game").clicked += () =>
        {
            var gameCodeField = root.Q<TextField>("game-code");
            gameState.randomSeed = GameCodeUtility.GameCodeToRandomSeed(gameCodeField.value);
            gameState.localPlayerIndex = 1;
            gameStartEvent.Invoke();
        };
    }
}
