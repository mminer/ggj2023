using System.Collections.Generic;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public static class GameTransaction
{
 public static void CreateGame(GameSchema game, PlayerSchema player)
  {
    var database = FirebaseDatabase.DefaultInstance.RootReference;
    var path = $"/{GameSchema.pathKey}/{game.gameCode}";
    var childUpdates = new Dictionary<string, object>
    {
      [path] = game.ToDict(),
    };
    
    Debug.Log("Running CreateGame Transaction: " + path);
    database.UpdateChildrenAsync(childUpdates);
    
    AddDefaultPlayer(game, player);
  }

 static void AddDefaultPlayer(GameSchema game, PlayerSchema player)
 {
   var database = FirebaseDatabase.DefaultInstance.RootReference;
   var playersRef = database.Child(GameSchema.pathKey).Child(game.gameCode).Child(PlayerSchema.pathKey);
   var key = playersRef.Push().Key;
   var path = $"/{GameSchema.pathKey}/{game.gameCode}/{PlayerSchema.pathKey}/{key}";
   
   var childUpdates = new Dictionary<string, object>
   {
     [path] = player.ToDict(),
   };
    
   Debug.Log("Running AddDefaultPlayer Transaction: " + path);
   database.UpdateChildrenAsync(childUpdates);
 }
 
 public static void EndGame(GameSchema game)
 {
   Debug.Log("Running EndGame Transaction: " + game.gameCode);
   var database = FirebaseDatabase.DefaultInstance.RootReference;
   database.Child(GameSchema.pathKey).Child(game.gameCode).Child("ended").SetValueAsync(game.ended);
 }
}
/*
 TransactionResult AddScoreTransaction(MutableData mutableData) {
      Debug.Log(mutableData.Value.ToString());
      var rooms = mutableData.Value as List<object>;
      Debug.Log("mutable children: " + mutableData.ChildrenCount);

      if (rooms == null)
      {
        Debug.Log("rooms is null :(");
        rooms = new List<object>();
      }

      // } else if (mutableData.ChildrenCount >= MaxScores) {
      //   // If the current list of scores is greater or equal to our maximum allowed number,
      //   // we see if the new score should be added and remove the lowest existing score.
      //   long minScore = long.MaxValue;
      //   object minVal = null;
      //   foreach (var child in rooms) {
      //     if (!(child is Dictionary<string, object>))
      //       continue;
      //     long childScore = (long)((Dictionary<string, object>)child)["score"];
      //     if (childScore < minScore) {
      //       minScore = childScore;
      //       minVal = child;
      //     }
      //   }
      //   // If the new score is lower than the current minimum, we abort.
      //   if (minScore > score) {
      //     return TransactionResult.Abort();
      //   }
      //   // Otherwise, we remove the current lowest to be replaced with the new score.
      //   rooms.Remove(minVal);
      // }

      var sampleEntry = new Dictionary<string, object>
      {
        ["playerId"] = roomCode,
        ["cards"] = new List<string>(){"card_a", "card_b"}.ToArray(),
      };
      
      // Now we add the new score as a new entry that contains the email address and score.
      // Dictionary<string, object> newRoomMap = new Dictionary<string, object>
      // {
      //   ["created"] = DateTime.Now.ToString(),
      //   // ["code"] = roomCode,
      //   ["history"] = new List<Dictionary<string, object>>
      //   {
      //     sampleEntry
      //   }
      // };
      // var roomList = history.ToList<Dictionary<string, object>>();
      rooms.Add(sampleEntry);

      // You must set the Value to indicate data at that location has changed.
      mutableData.Value = rooms;
      return TransactionResult.Success(mutableData);
    }
*/