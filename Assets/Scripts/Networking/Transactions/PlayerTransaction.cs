using System.Collections.Generic;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class PlayerTransaction
{
  private PlayerSchema player;

  public PlayerTransaction(PlayerSchema player)
  {
    this.player = player;
  }
  
  TransactionResult AddPlayerTransaction(MutableData mutableData) {
      if (mutableData.Value == null)
      {
        Debug.LogError("Player list was not found");
        return TransactionResult.Abort();
      }
      
      var players = mutableData.Value as List<object>;

      if (players == null)
      {
        Debug.LogWarning("Players object is null. Aborting.");
        Debug.Log("Child count: " + mutableData.Key + " - " + mutableData.ChildrenCount);
        return TransactionResult.Abort();
      } else if (players.Count == 0)
      {
        Debug.Log("No players found in game, aborting");
        Debug.Log("Child count: " + mutableData.ChildrenCount);
        return TransactionResult.Abort();
      }
      
      players.Add(player.ToDict());
      
      // You must set the Value to indicate data at that location has changed.
      mutableData.Value = players;
      return TransactionResult.Success(mutableData);
  }

  public void JoinGame(string gameCode)
  {
    var path = $"{GameSchema.pathKey}/{gameCode}/{PlayerSchema.pathKey}";
    DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference(path);

    Debug.Log("Running JoinGame Transaction: " + path);
    
    // Use a transaction to ensure that we do not encounter issues with simultaneous updates.
    reference.RunTransaction(AddPlayerTransaction)
      .ContinueWithOnMainThread(task => {
        if (task.Exception != null) {
          Debug.Log(task.Exception.ToString());
        } else if (task.IsCompleted) {
          Debug.Log("JoinGame Transaction complete: " + path);
        }
      });
  }
}
