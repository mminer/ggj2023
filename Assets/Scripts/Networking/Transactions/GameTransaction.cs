using System;
using System.Collections.Generic;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class GameTransaction
{
  private GameSchema game;

  public GameTransaction(GameSchema game)
  {
    this.game = game;
  }
  
  TransactionResult CreateGameTransaction(MutableData mutableData) {
      Debug.Log("Mutating games: " + mutableData);
      
      var games = mutableData.Value as Dictionary<string, object>;

      if (games == null)
      {
        Debug.Log("No games found, creating new object");
        games = new Dictionary<string, object>();
      }

      games.Add(game.gameCode, game.ToDict());

      // You must set the Value to indicate data at that location has changed.
      mutableData.Value = games;
      return TransactionResult.Success(mutableData);
  }

  public void CreateGame()
  {
    var path = GameSchema.parentPath;
    DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference(path);

    Debug.Log("Running CreateGame Transaction...");
    
    // Use a transaction to ensure that we do not encounter issues with simultaneous updates.
    reference.RunTransaction(CreateGameTransaction)
      .ContinueWithOnMainThread(task => {
        if (task.Exception != null) {
          Debug.Log(task.Exception.ToString());
        } else if (task.IsCompleted) {
          Debug.Log("CreateGame Transaction complete.");
        }
      });
  }
}
