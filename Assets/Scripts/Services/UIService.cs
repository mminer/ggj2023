using UnityEngine;
using UnityEngine.UIElements;

public class UIService : Services.Service
{
    UIDocument[] uiDocuments;

    void Awake()
    {
        uiDocuments = GetComponentsInChildren<UIDocument>();
    }

    public void ShowUI(UIDocument ui)
    {
        foreach (var otherUI in uiDocuments)
        {
            otherUI.enabled = false;
        }

        Debug.Log($"Showing UI: {ui.name}");
        ui.enabled = true;
    }
}
