using UnityEngine.UIElements;

public class UIService : Services.Service
{
    void Awake()
    {
        var rootVisualElement = GetComponent<UIDocument>().rootVisualElement;
    }
}
