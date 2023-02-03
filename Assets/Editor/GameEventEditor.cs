using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameEvent))]
public class GameEventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Invoke"))
        {
            var gameEvent = (GameEvent)target;
            gameEvent.Invoke();
        }
    }
}
