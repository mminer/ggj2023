using System.Collections.Generic;
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
      var actionId = gameState.roundActions.IndexOf(gameState.latestRoundActionGroup);
      // var action = gameState.latestRoundActionGroup[playerId];
      var data = new List<int>(); // TODO: populate data
      var historySchema = new HistorySchema(playerId, actionId, data);
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

      Debug.LogWarning("Not Implemented: Add new history/actions to `gameState`");
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
}
