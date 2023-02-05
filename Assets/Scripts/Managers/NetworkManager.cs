using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    [SerializeField] GameState gameState;

    void Start()
    {
      Database.CheckDependenciesAndInitialize();
    }

    public void CreateGame()
    {
      var playerSchema = new PlayerSchema(gameState.localPlayerIndex, "Player1", 0);
      var gameCode = GetGameCode();
      var gameSchema = new GameSchema(gameCode);
      Database.CreateGame(gameSchema, playerSchema);
      Database.ListenForPlayerJoin(gameCode, OnPlayerJoined);
      Database.ListenForHistory(gameCode, OnHistoryAdded);
    }

    public void JoinGame()
    {
      var playerSchema = new PlayerSchema(1, "Player2", 0);
      var gameCode = GetGameCode();
      Database.JoinGame(gameCode, playerSchema);
      Database.ListenForHistory(gameCode, OnHistoryAdded);
    }
    
    public void EndGame()
    {
      var gameCode = GetGameCode();
      var gameSchema = new GameSchema(gameCode);
      gameSchema.EndGame();
      Database.EndGame(gameSchema);
    }
    
    public void RecordAction()
    {
      var gameCode = GetGameCode();
      var playerId = gameState.localPlayerIndex;
      var actionId = gameState.roundActions.Count - 1;
      var phase = gameState.phase.ToString();
      var roundAction = gameState.latestRoundActionGroup[gameState.localPlayerIndex];

      var cards = roundAction switch
      {
        RoundAction_Discard discardAction => discardAction.cards,
        RoundAction_SubmitQueue submitQueueAction => submitQueueAction.cards,
        _ => throw new ArgumentOutOfRangeException(),
      };

      var data = cards.Cast<int>().ToList();
      var historySchema = new HistorySchema(playerId, actionId, phase, data);
      Database.AddHistory(gameCode, historySchema);
    }
    
    private string GetGameCode()
    {
      var seed = gameState.randomSeed;
      return GameCodeUtility.RandomSeedToGameCode(seed);
    }

    void OnPlayerJoined(string gameCode, List<PlayerSchema> players)
    {
      if (!ValidateUpdateIsRelevant(gameCode, "OnPlayerJoined"))
      {
        return;
      }
      
      Debug.LogWarning("Not Implemented: Add new player to `gameState`");
    }
    
    void OnHistoryAdded(string gameCode, List<HistorySchema> history)
    {
      if (!ValidateUpdateIsRelevant(gameCode, "OnHistoryAdded"))
      {
        return;
      }

      for (var i = 0; i < history.Count; i++)
      {
        var item = history[i];
        var roundGroup = gameState.roundActions[i];

        // If we already know about this round action, skip deserializing it again.
        if (roundGroup[item.playerId] != null)
        {
          continue;
        }

        roundGroup[item.playerId] = HistoryItemToRoundAction(item);
      }
    }

    private bool ValidateUpdateIsRelevant(string gameCode, string context)
    {
      var currentGameCode = GetGameCode();
      
      if (gameCode != GetGameCode())
      {
        Debug.Log($"{context} triggered for game `{gameCode}` but we are in game `{currentGameCode}`");
        return false;
      }

      return true;
    }

    static IRoundAction HistoryItemToRoundAction(HistorySchema item)
    {
        var cards = item.data.Cast<Card>();
        Enum.TryParse(item.phase, out Phase phase);

        return phase switch
        {
          Phase.Discard => new RoundAction_Discard(item.playerId, cards),
          Phase.Queue => new RoundAction_SubmitQueue(item.playerId, cards),
          _ => throw new ArgumentOutOfRangeException(),
        };
    }
}
