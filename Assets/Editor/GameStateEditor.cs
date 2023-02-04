using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameState))]
public class GameStateEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var gameState = (GameState)target;

        PrintCards("Deck", gameState.deck);
        PrintCards("Player 1 Hand", gameState.player1Hand);
        PrintCards("Player 2 Hand", gameState.player2Hand);
    }

    static void PrintCards(string label, IEnumerable<Card> cards)
    {
        EditorGUILayout.Space();
        GUILayout.Label(label, EditorStyles.boldLabel);

        foreach (var card in cards)
        {
            GUILayout.Label(card.ToString());
        }
    }
}
