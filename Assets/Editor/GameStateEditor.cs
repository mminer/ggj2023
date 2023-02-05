using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameState))]
public class GameStateEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (!Application.isPlaying)
        {
            return;
        }

        var gameState = (GameState)target;
        var isGameActive = gameState.rng != null;

        CustomEditorUtility.Header("Random Number Generation");
        EditorGUILayout.LabelField("Random Seed", isGameActive ? gameState.randomSeed.ToString() : "");
        EditorGUILayout.LabelField("Game Code", isGameActive ? GameCodeUtility.RandomSeedToGameCode(gameState.randomSeed) : "");

        CustomEditorUtility.Header("Players");
        EditorGUILayout.LabelField("Local Player", isGameActive ? gameState.localPlayerIndex.ToString() : "");

        CustomEditorUtility.Header("Game");
        EditorGUILayout.ObjectField("Hero", gameState.hero, gameState.hero != null ? gameState.hero.GetType() : null, false);

        foreach (var enemy in gameState.enemies)
        {
            EditorGUILayout.ObjectField(enemy.name, enemy, enemy.GetType(), false);
        }

        EditorGUILayout.LabelField("Phase", isGameActive ? gameState.phase.ToString() : "");
        EditorGUILayout.LabelField("Player Order", isGameActive ? string.Join(", ", gameState.playerOrder) : "");

        PrintCards("Deck", gameState.deck);
        PrintCards("Discard Pile", gameState.discardPile);

        foreach (var player in gameState.players)
        {
            PrintCards($"Player {player.index} Hand", player.hand);
        }
    }

    static void PrintCards(string headerText, IEnumerable<Card> cards)
    {
        CustomEditorUtility.Header(headerText);

        foreach (var card in cards)
        {
            GUILayout.Label(ObjectNames.NicifyVariableName(card.ToString()));
        }
    }
}
