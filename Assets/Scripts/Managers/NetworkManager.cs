using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    [SerializeField] GameState gameState;
  
    public void CreateGame()
    {
      var playerSchema = new PlayerSchema(0, "Player1", 0);
      var gameCode = GetGameCode();
      var gameSchema = new GameSchema(gameCode, new List<PlayerSchema>(){playerSchema}, new List<HistorySchema>());
      var gameTransaction = new GameTransaction(gameSchema);
      gameTransaction.CreateGame();
    }

    public void JoinGame()
    {
      var playerSchema = new PlayerSchema(1, "Player2", 0);
      var gameCode = GetGameCode();
      var playerTransaction = new PlayerTransaction(playerSchema);
      playerTransaction.JoinGame(gameCode);
    }
    
    public void RecordAction()
    {
      var gameCode = GetGameCode();
      // TODO: Figure out how to populate this schema
      // See: gameState.lastRoundActionGroup[gameState.localPlayerIndex]
      // var historySchema = new HistorySchema(gameState.phase.);
      // var historyTransaction = new HistoryTransaction(historySchema);
      // historyTransaction.AddHistory(gameCode);
    }

    private string GetGameCode()
    {
      var seed = gameState.randomSeed;
      return GameCodeUtility.RandomSeedToGameCode(seed);
    }
}

/*
{
  "Games": {
    "ABCD": {
      "created": "2023-02-05T05:30:15",
      "ended": "22023-02-05T05:30:15",
      "players": [
        {
          "created": "2023-02-05T05:30:15",
          "name": "Matthew",
          "icon": 0,
        }
      ],
      "history": [
        {
          "created": "2023-02-05T05:30:15",
          "player_id": 0,
          "action_id": 0,
          "data": [0],
        }
      ]
    }
  }
}
*/
