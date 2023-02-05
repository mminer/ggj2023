using System.Collections.Generic;
using Firebase.Database;
using UnityEngine;

public static class Database
{
  private static DatabaseReference database;
  
  public static void Init()
  {
    database = FirebaseDatabase.DefaultInstance.RootReference;
  }
  public static void CreateGame(GameSchema game, PlayerSchema player)
  {
    var path = $"/{GameSchema.pathKey}/{game.gameCode}";
    var childUpdates = new Dictionary<string, object>
    {
      [path] = game.ToDict(),
    };
    
    Debug.Log("[Database] Running CreateGame Transaction: " + path);
    database.UpdateChildrenAsync(childUpdates);
    
    AddDefaultPlayer(game, player);
  }

  static void AddDefaultPlayer(GameSchema game, PlayerSchema player)
  {
    var playersRef = database.Child(GameSchema.pathKey).Child(game.gameCode).Child(PlayerSchema.pathKey);
    var key = playersRef.Push().Key;
    var path = $"/{GameSchema.pathKey}/{game.gameCode}/{PlayerSchema.pathKey}/{key}";
   
    var childUpdates = new Dictionary<string, object>
    {
      [path] = player.ToDict(),
    };
    
    Debug.Log("[Database] Running AddDefaultPlayer Transaction: " + path);
    database.UpdateChildrenAsync(childUpdates);
  }
  
  public static void JoinGame(string gameCode, PlayerSchema player)
  {
    var playersRef = database.Child(GameSchema.pathKey).Child(gameCode).Child(PlayerSchema.pathKey);
    var key = playersRef.Push().Key;
    var path = $"/{GameSchema.pathKey}/{gameCode}/{PlayerSchema.pathKey}/{key}";
   
    var childUpdates = new Dictionary<string, object>
    {
      [path] = player.ToDict(),
    };
    
    Debug.Log("[Database] Running JoinGame Transaction: " + path);
    database.UpdateChildrenAsync(childUpdates);
  }
 
  public static void EndGame(GameSchema game)
  {
    Debug.Log("[Database] Running EndGame Transaction: " + game.gameCode);
    database.Child(GameSchema.pathKey).Child(game.gameCode).Child("ended").SetValueAsync(game.ended);
  }
  
  public static void AddHistory(string gameCode, HistorySchema history)
  {
    var historyRef = database.Child(GameSchema.pathKey).Child(gameCode).Child(HistorySchema.pathKey);
    var key = historyRef.Push().Key;
    var path = $"/{GameSchema.pathKey}/{gameCode}/{HistorySchema.pathKey}/{key}";
   
    var childUpdates = new Dictionary<string, object>
    {
      [path] = history.ToDict(),
    };
    
    Debug.Log("[Database] Running AddHistory Transaction: " + path);
    database.UpdateChildrenAsync(childUpdates);
  }
}
