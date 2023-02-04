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

        CustomEditorUtility.Header("Random Number Generation");
        EditorGUILayout.LabelField("Random Seed", gameState.rng != null ? gameState.randomSeed.ToString() : "Unassigned");
        EditorGUILayout.LabelField("Game Code", gameState.rng != null ? GameCodeUtility.RandomSeedToGameCode(gameState.randomSeed) : "Unassigned");

        CustomEditorUtility.Header("Players");
        EditorGUILayout.LabelField("Local Player", GetPlayerName(gameState.localPlayer));
        EditorGUILayout.LabelField("Remote Player", GetPlayerName(gameState.remotePlayer));

        PrintCards("Deck", gameState.deck);
        PrintCards("Discard Pile", gameState.discardPile);
        PrintCards("Player 1 Hand", gameState.player1Hand);
        PrintCards("Player 2 Hand", gameState.player2Hand);
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

    static void PrintCards(string headerText, IEnumerable<Card> cards)
    {
        CustomEditorUtility.Header(headerText);

        foreach (var card in cards)
        {
            GUILayout.Label(card.ToString());
        }
    }
}
