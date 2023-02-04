using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameState))]
public class GameStateEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var gameState = (GameState)target;

        EditorGUILayout.Space();
        GUILayout.Label("Deck", EditorStyles.boldLabel);

        foreach (var card in gameState.deck)
        {
            GUILayout.Label(card.ToString());
        }
    }
}
