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
    var path = GameSchema.pathKey;
    DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference(path);

    Debug.Log("Running CreateGame Transaction: " + path);
    
    // Use a transaction to ensure that we do not encounter issues with simultaneous updates.
    reference.RunTransaction(CreateGameTransaction)
      .ContinueWithOnMainThread(task => {
        if (task.Exception != null) {
          Debug.Log(task.Exception.ToString());
        } else if (task.IsCompleted) {
          Debug.Log("CreateGame Transaction complete: " + path);
        }
      });
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