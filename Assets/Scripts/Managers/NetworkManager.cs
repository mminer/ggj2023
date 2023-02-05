using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    [SerializeField] GameState gameState;
  
    public void CreateGame()
    {
      var playerSchema = new PlayerSchema(0, "Player1", 0);
      var seed = gameState.randomSeed;
      var gameCode = GameCodeUtility.RandomSeedToGameCode(seed);
      var gameSchema = new GameSchema(gameCode, new List<PlayerSchema>(){playerSchema}, new List<HistorySchema>());
      var gameTransaction = new GameTransaction(gameSchema);
      gameTransaction.CreateGame();
    }

    public void JoinGame()
    {
      var playerSchema = new PlayerSchema(1, "Player2", 0);
      var seed = gameState.randomSeed;
      var gameCode = GameCodeUtility.RandomSeedToGameCode(seed);
      var playerTransaction = new PlayerTransaction(playerSchema);
      playerTransaction.JoinGame(gameCode);
    }
}

/*
{
  "Games": {
    "ABCD": {
      "created": "2023-02-03 11:22:52 PM",
      "ended": "2023-02-03 11:22:52 PM",
      "players": [
      {
        "created": "2023-02-03 11:22:52 PM",
        "name": "Matthew",
        "icon": 0,
      }
      ],
      "history": [
      {
        "created": "2023-02-03 11:22:52 PM",
        "player_id": 0,
        "action_id": 0,
        "data": [0],
      },
      ]
    }
  }
}
*/
