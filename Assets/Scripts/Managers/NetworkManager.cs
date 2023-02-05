using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    [SerializeField] GameState gameState;
  
    public void CreateGame()
    {
      var playerSchema = new PlayerSchema(gameState.localPlayerIndex, "Player1", 0);
      var gameCode = GetGameCode();
      var gameSchema = new GameSchema(gameCode);
      Database.CreateGame(gameSchema, playerSchema);
    }

    public void JoinGame()
    {
      var playerSchema = new PlayerSchema(1, "Player2", 0);
      var gameCode = GetGameCode();
      Database.JoinGame(gameCode, playerSchema);
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
}
