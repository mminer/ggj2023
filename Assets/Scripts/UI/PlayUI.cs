using UnityEngine;
using UnityEngine.UIElements;

public class PlayUI : MonoBehaviour
{
    [SerializeField] GameState gameState;

    VisualElement root;

    void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
    }

    void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        Debug.Log("Refreshing hand UI.");
        root.Clear();

        foreach (var card in gameState.localHand)
        {
            root.Add(new Label(card.ToString()));
        }
    }
}
