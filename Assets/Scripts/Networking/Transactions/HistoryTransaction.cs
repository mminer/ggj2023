using System.Collections.Generic;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class HistoryTransaction
{
  private HistorySchema history;

  public HistoryTransaction(HistorySchema history)
  {
    this.history = history;
  }
  
  TransactionResult AddHistoryTransaction(MutableData mutableData) {
      var history = mutableData.Value as List<object>;
      
      if (history == null)
      {
        Debug.Log("No history found, creating new object");
        history = new List<object>();
      }
      
      history.Add(this.history.ToDict());
      
      // You must set the Value to indicate data at that location has changed.
      mutableData.Value = history;
      return TransactionResult.Success(mutableData);
  }

  public void AddHistory(string gameCode)
  {
    var path = $"{GameSchema.pathKey}/{gameCode}/{HistorySchema.pathKey}";
    DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference(path);

    Debug.Log("Running AddHistory Transaction: " + path);
    
    // Use a transaction to ensure that we do not encounter issues with simultaneous updates.
    reference.RunTransaction(AddHistoryTransaction)
      .ContinueWithOnMainThread(task => {
        if (task.Exception != null) {
          Debug.Log(task.Exception.ToString());
        } else if (task.IsCompleted) {
          Debug.Log("AddHistory Transaction complete: " + path);
        }
      });
  }
}
