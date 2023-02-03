using UnityEngine;
using UnityEngine.UIElements;

public class UIService : Services.Service
{
    [SerializeField] GameEvent gameStartEvent;

    void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        var startGameButton = root.Q<Button>("start-game");

        startGameButton.clicked += () =>
        {
            startGameButton.visible = false;
            gameStartEvent.Invoke();
        };
    }
}
