using System.Collections.Generic;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public static class HistoryTransaction
{
  public static void AddHistory(string gameCode, HistorySchema history)
  {
    var database = FirebaseDatabase.DefaultInstance.RootReference;
    var historyRef = database.Child(GameSchema.pathKey).Child(gameCode).Child(HistorySchema.pathKey);
    var key = historyRef.Push().Key;
    var path = $"/{GameSchema.pathKey}/{gameCode}/{HistorySchema.pathKey}/{key}";
   
    var childUpdates = new Dictionary<string, object>
    {
      [path] = history.ToDict(),
    };
    
    Debug.Log("Running AddHistory Transaction: " + path);
    database.UpdateChildrenAsync(childUpdates);
  }
}
