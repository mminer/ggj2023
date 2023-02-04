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
        var isPlaying = gameState.rng != null;

        CustomEditorUtility.Header("Random Number Generation");
        EditorGUILayout.LabelField("Random Seed", isPlaying ? gameState.randomSeed.ToString() : "");
        EditorGUILayout.LabelField("Game Code", isPlaying ? GameCodeUtility.RandomSeedToGameCode(gameState.randomSeed) : "");

        CustomEditorUtility.Header("Players");
        EditorGUILayout.LabelField("Local Player", isPlaying ? ObjectNames.NicifyVariableName(gameState.localPlayer.ToString()) : "");
        EditorGUILayout.LabelField("Remote Player", isPlaying ? ObjectNames.NicifyVariableName(gameState.remotePlayer.ToString()) : "");

        CustomEditorUtility.Header("Game");
        EditorGUILayout.LabelField("Phase", isPlaying ? gameState.phase.ToString() : "");
        EditorGUILayout.LabelField("Player Who Goes First", isPlaying ? ObjectNames.NicifyVariableName(gameState.playerWhoGoesFirst.ToString()) : "");

        PrintCards("Deck", gameState.deck);
        PrintCards("Discard Pile", gameState.discardPile);
        PrintCards("Player 1 Hand", gameState.player1Hand);
        PrintCards("Player 2 Hand", gameState.player2Hand);
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
