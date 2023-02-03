using UnityEngine;
using UnityEngine.UIElements;

public class StartGameUI : MonoBehaviour
{
    [SerializeField] GameEvent gameStartEvent;

    void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        var startGameButton = root.Q<Button>("start-game");
        startGameButton.clicked += gameStartEvent.Invoke;
    }
}
