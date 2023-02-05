using System.Collections.Generic;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public static class PlayerTransaction
{
  public static void JoinGame(string gameCode, PlayerSchema player)
  {
    var database = FirebaseDatabase.DefaultInstance.RootReference;
    var playersRef = database.Child(GameSchema.pathKey).Child(gameCode).Child(PlayerSchema.pathKey);
    var key = playersRef.Push().Key;
    var path = $"/{GameSchema.pathKey}/{gameCode}/{PlayerSchema.pathKey}/{key}";
   
    var childUpdates = new Dictionary<string, object>
    {
      [path] = player.ToDict(),
    };
    
    Debug.Log("Running JoinGame Transaction: " + path);
    database.UpdateChildrenAsync(childUpdates);
  }
}
