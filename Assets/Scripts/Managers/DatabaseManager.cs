using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;


// Handler for UI buttons on the scene.  Also performs some
// necessary setup (initializing the firebase app, etc) on
// startup.
public class DatabaseManager : MonoBehaviour {
  
  DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
  protected bool isFirebaseInitialized = false;

  // When the app starts, check to make sure that we have
  // the required dependencies to use Firebase, and if not,
  // add them if possible.
  protected virtual void Start() {
    FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
      dependencyStatus = task.Result;
      if (dependencyStatus == DependencyStatus.Available) {
        InitializeFirebase();
      } else {
        Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
      }
    });
  }

  // Initialize the Firebase database:
  protected virtual void InitializeFirebase() {
    FirebaseApp app = FirebaseApp.DefaultInstance;
    FirebaseDatabase.DefaultInstance.GoOnline();
    StartListener();
    isFirebaseInitialized = true;
  }

  protected void StartListener() {
    FirebaseDatabase.DefaultInstance
      .GetReference("Rooms/ABCD/history")//.OrderByChild("created")
      .ValueChanged += (object sender2, ValueChangedEventArgs e2) => {
        if (e2.DatabaseError != null) {
          Debug.LogError(e2.DatabaseError.Message);
          return;
        }
        
        Debug.Log("Received values for rooms.");
        
        // rooms.Clear();
        
        if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0) {
          foreach (var childSnapshot in e2.Snapshot.Children)
          {
            var code = childSnapshot.Child("playerId").Value.ToString();
            // var created = childSnapshot.Child("created").Value.ToString();
            // var label = code + " :: " + created;
            // Debug.Log("room entry: " + label);
            // rooms.Insert(0, code);
          }
        }
    };
  }
}
