using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class StartUI : MonoBehaviour
{
    [SerializeField] GameState gameState;
    [SerializeField] GameEvent gameStartEvent;
    [SerializeField] GameEvent gameCreateEvent;
    [SerializeField] GameEvent gameJoinEvent;

    void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        root.Q<Label>("title-game-error").text = string.Empty;
        var nameInput = root.Q<TextField>("title-game-name-input");
        
        var codeInput = root.Q<TextField>("title-game-options-join-input");
        codeInput.SetEnabled(false);

        var hostButton = root.Q<Button>("title-game-host");
        hostButton.SetEnabled(false);
        
        var joinButton = root.Q<Button>("title-game-options-join-button");
        joinButton.SetEnabled(false);
        
        nameInput.RegisterCallback<KeyUpEvent>(e =>
        {
            var hasName = !string.IsNullOrWhiteSpace(nameInput.value);
            hostButton.SetEnabled(hasName);
            codeInput.SetEnabled(hasName);
            
            if (!hasName)
            {
                joinButton.SetEnabled(false);
            }
            else
            {
                codeInput.value = codeInput.value.ToUpper();
                var validCode = HasValidGameCode(codeInput.value);
                joinButton.SetEnabled(validCode);
            }
        });
        
        codeInput.RegisterCallback<KeyUpEvent>(e =>
        {
            codeInput.value = codeInput.value.ToUpper();
            var validCode = HasValidGameCode(codeInput.value);
            joinButton.SetEnabled(validCode);
        });

        hostButton.clicked += () =>
        {
            // This is the only time outside RandomNumberGenerator we should use UnityEngine.Random directly.
            gameState.randomSeed = Random.Range(0, Rules.maxRandomSeed);

            var index = 0;
            gameState.localPlayerIndex = index;
            
            var playerName = nameInput.value;
            gameState.players[index] = new Player(index, playerName);
            
            gameCreateEvent.Invoke();
            gameStartEvent.Invoke();
        };

        joinButton.clicked += () =>
        {
            var gameCode = codeInput.value.ToUpper();
            gameState.randomSeed = GameCodeUtility.GameCodeToRandomSeed(gameCode);

            var index = 1;
            gameState.localPlayerIndex = index;
            
            var playerName = nameInput.value;
            gameState.players[index] = new Player(index, playerName);
            
            gameJoinEvent.Invoke();
        };
    }

    public void OnGameAvailable()
    {
        Debug.Log("Game available!");
        gameStartEvent.Invoke();
    }
    
    public void OnGameUnavailable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        var seed = gameState.randomSeed;
        var gameCode = GameCodeUtility.RandomSeedToGameCode(seed);
        var error = $"Unable to find game with code {gameCode}";
        root.Q<Label>("title-game-error").text = error;
    }

    bool HasValidGameCode(string gameCode)
    {
        Regex r = new Regex(@"^[A-Fa-f0-9xXyYwW]{4}$");
        return r.IsMatch(gameCode.Trim());
    }
}
