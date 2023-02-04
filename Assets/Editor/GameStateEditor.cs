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

        EditorGUILayout.LabelField("Local Player", GetPlayerName(gameState.localPlayer));
        EditorGUILayout.LabelField("Remote Player", GetPlayerName(gameState.remotePlayer));

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

    static string GetPlayerName(Player? player)
    {
        return player switch
        {
            Player.Player1 => "Player 1",
            Player.Player2 => "Player 2",
            _ => "Unassigned",
        };
    }
}
