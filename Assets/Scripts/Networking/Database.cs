using System.Collections.Generic;
using System.Linq;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public static class Database
{
  static DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
  static DatabaseReference database;

  public static bool IsInitialized()
  {
    return database != null;
  }
  
  public static void CheckDependenciesAndInitialize() {
    FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
      dependencyStatus = task.Result;
      if (dependencyStatus == DependencyStatus.Available)
      {
        Debug.Log("[Database] Firebase dependencies are OK");
        InitializeFirebase();
      } else {
        Debug.LogError("[Database] Could not resolve all Firebase dependencies: " + dependencyStatus);
      }
    });
  }
  
  static void InitializeFirebase() {
    FirebaseApp app = FirebaseApp.DefaultInstance;
    FirebaseDatabase.DefaultInstance.GoOnline();
    database = FirebaseDatabase.DefaultInstance.RootReference;
    Debug.Log("[Database] Firebase initialized!");
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
  
  public static void JoinGame(string gameCode, PlayerSchema player, System.Action<string, bool> callback)
  {
    FirebaseDatabase.DefaultInstance
      .GetReference($"/{GameSchema.pathKey}/{gameCode}")
      .GetValueAsync().ContinueWithOnMainThread(task => {
        if (task.IsFaulted) {
          Debug.LogError("[Database] Failed to lookup game code: " + gameCode);
          callback(gameCode, false);
        } else if (task.IsCompleted) {
          DataSnapshot snapshot = task.Result;
          
          if (snapshot is not { Exists: true })
          {
            Debug.Log("[Database] Game code not found: " + gameCode);
            callback(gameCode, false);
            return;
          }
          
          var playersRef = database.Child(GameSchema.pathKey).Child(gameCode).Child(PlayerSchema.pathKey);
          var key = playersRef.Push().Key;
          var path = $"/{GameSchema.pathKey}/{gameCode}/{PlayerSchema.pathKey}/{key}";
   
          var childUpdates = new Dictionary<string, object>
          {
            [path] = player.ToDict(),
          };
    
          Debug.Log("[Database] Running JoinGame Transaction: " + path);
          database.UpdateChildrenAsync(childUpdates);
          
          callback(gameCode, true);
        }
      });
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

  public static void ListenForPlayerJoin(string gameCode, System.Action<string, List<PlayerSchema>> onPlayerJoined)
  {
    var path = $"{GameSchema.pathKey}/{gameCode}/{PlayerSchema.pathKey}";
    
    FirebaseDatabase.DefaultInstance.GetReference(path).ValueChanged += (object sender2, ValueChangedEventArgs e2) => {
      if (e2.DatabaseError != null) {
        Debug.LogError(e2.DatabaseError.Message);
        return;
      }
      
      Debug.Log("[Database] Received new player value.");

      var players = new List<PlayerSchema>();

      if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0)
      {
        players = e2.Snapshot.Children.Select((player, index) =>
        {
          var icon = (long)player.Child("icon").Value;
          var created = player.Child("created").Value.ToString();
          var name = player.Child("name").Value.ToString();
          return new PlayerSchema(index, name, (int)icon, created);
        }).ToList();
      }

      onPlayerJoined(gameCode, players);
    };
  }
  
  public static void ListenForHistory(string gameCode, System.Action<string, List<HistorySchema>> onHistoryChanged)
  {
    var path = $"{GameSchema.pathKey}/{gameCode}/{HistorySchema.pathKey}";
    
    FirebaseDatabase.DefaultInstance.GetReference(path).ValueChanged += (object sender2, ValueChangedEventArgs e2) => {
      if (e2.DatabaseError != null) {
        Debug.LogError(e2.DatabaseError.Message);
        return;
      }
      
      Debug.Log("[Database] Received new history value.");

      var history = new List<HistorySchema>();

      if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0)
      {
        history = e2.Snapshot.Children.Select((record) =>
        {
          var playerId = (long)record.Child("player_id").Value;
          var created = record.Child("created").Value.ToString();
          var actionId = (long)record.Child("action_id").Value;
          var phase = record.Child("phase").Value.ToString();
          
          var dataAsObjectList = record.Child("data").Value as List<object>;
          var data = dataAsObjectList.Select((d) => (int)(long)d).ToList();
          
          return new HistorySchema((int)playerId, (int)actionId, phase, data, created);
        }).ToList();
      }

      onHistoryChanged(gameCode, history);
    };
  }
}
