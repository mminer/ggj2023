using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameOverUI : MonoBehaviour
{
    void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        var playAgainButton = root.Q<Button>("play-again");
        playAgainButton.clicked += () => SceneManager.LoadScene(0);
    }
}
