using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameEvent : ScriptableObject
{
    readonly List<GameEventListener> listeners = new();

    public void Invoke()
    {
        Debug.Log($"Game event: {name}");

        for (var i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventInvoked();
        }
    }

    public void RegisterListener(GameEventListener listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(GameEventListener listener)
    {
        listeners.Remove(listener);
    }
}
