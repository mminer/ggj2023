using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RemotePlayerSimulatorWindow : EditorWindow
{
    GameState gameState;

    readonly List<Card> cardQueue = new();

    void OnGUI()
    {
        if (gameState == null)
        {
            gameState = LoadGameState();
        }

        if (!Application.isPlaying || gameState.randomSeed == -1)
        {
            return;
        }

        var playerIndex = gameState.localPlayerIndex == 0 ? 1 : 0;
        var player = gameState.players[playerIndex];

        GUILayout.Label($"Player {playerIndex} Hand:", EditorStyles.boldLabel);

        foreach (var card in player.hand)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label(ObjectNames.NicifyVariableName(card.ToString()), GUILayout.Width(200));

                using (new EditorGUI.DisabledScope(gameState.phase != Phase.Discard))
                {
                    if (GUILayout.Button("Discard"))
                    {
                        var roundAction = new RoundAction_Discard(playerIndex, new[] { card });
                        gameState.SetRoundAction(roundAction);
                    }
                }

                using (new EditorGUI.DisabledScope(gameState.phase != Phase.CreateQueue))
                {
                    if (GUILayout.Button("Add to Queue"))
                    {
                        cardQueue.Add(card);
                    }
                }
            }
        }

        using (new EditorGUI.DisabledScope(gameState.phase != Phase.CreateQueue))
        {
            EditorGUILayout.Space();
            GUILayout.Label("Queue:", EditorStyles.boldLabel);

            using (new EditorGUILayout.HorizontalScope())
            {
                foreach (var card in cardQueue)
                {
                    if (GUILayout.Button(ObjectNames.NicifyVariableName(card.ToString())))
                    {
                        cardQueue.Remove(card);
                    }
                }
            }

            if (GUILayout.Button("Submit Queue"))
            {
                var roundAction = new RoundAction_SubmitQueue(playerIndex, cardQueue);
                gameState.SetRoundAction(roundAction);
            }
        }
    }

    static GameState LoadGameState()
    {
        return AssetDatabase.LoadAssetAtPath<GameState>("Assets/GameState.asset");
    }

    [MenuItem("Window/Remote Player Simulator")]
    static void Open()
    {
        var window = GetWindow<RemotePlayerSimulatorWindow>();
        window.titleContent = new GUIContent("Remote Player Simulator");
    }
}
