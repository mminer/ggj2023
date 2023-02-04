// Copyright 2016 Google Inc. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Linq;

namespace Firebase.Sample.Database {
  using Firebase;
  using Firebase.Database;
  using Firebase.Extensions;
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;

  // Handler for UI buttons on the scene.  Also performs some
  // necessary setup (initializing the firebase app, etc) on
  // startup.
  public class UIHandler : MonoBehaviour {

    ArrayList rooms = new ArrayList();
    private string roomCode = "";
    private int score = 100;
    private const int MaxScores = 5;
    
    Vector2 scrollPosition = Vector2.zero;
    private Vector2 controlsScrollViewVector = Vector2.zero;
    public GUISkin fb_GUISkin;
    private Vector2 scrollViewVector = Vector2.zero;
    protected bool UIEnabled = true;
    private string logText = "";
    const int kMaxLogSize = 16382;
    
    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    protected bool isFirebaseInitialized = false;

    // When the app starts, check to make sure that we have
    // the required dependencies to use Firebase, and if not,
    // add them if possible.
    protected virtual void Start() {
      rooms.Clear();

      FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
        dependencyStatus = task.Result;
        if (dependencyStatus == DependencyStatus.Available) {
          InitializeFirebase();
        } else {
          Debug.LogError(
            "Could not resolve all Firebase dependencies: " + dependencyStatus);
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
          
          rooms.Clear();
          
          if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0) {
            foreach (var childSnapshot in e2.Snapshot.Children)
            {
              var code = childSnapshot.Child("playerId").Value.ToString();
              // var created = childSnapshot.Child("created").Value.ToString();
              // var label = code + " :: " + created;
              // Debug.Log("room entry: " + label);
              rooms.Insert(0, code);
            }
          }
      };
    }

    // Output text to the debug log text field, as well as the console.
    public void DebugLog(string s) {
      Debug.Log(s);
      logText += s + "\n";

      while (logText.Length > kMaxLogSize) {
        int index = logText.IndexOf("\n");
        logText = logText.Substring(index + 1);
      }

      scrollViewVector.y = int.MaxValue;
    }

    // A realtime database transaction receives MutableData which can be modified
    // and returns a TransactionResult which is either TransactionResult.Success(data) with
    // modified data or TransactionResult.Abort() which stops the transaction with no changes.
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

    public void AddScore() {
      if (string.IsNullOrEmpty(roomCode)) {
        DebugLog("invalid room code");
        return;
      }
      DebugLog(String.Format("Attempting to add room code {0}", roomCode));

      DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("Rooms/ABCD/history");

      DebugLog("Running Transaction...");
      // Use a transaction to ensure that we do not encounter issues with
      // simultaneous updates that otherwise might create more than MaxScores top scores.
      reference.RunTransaction(AddScoreTransaction)
        .ContinueWithOnMainThread(task => {
          if (task.Exception != null) {
            DebugLog(task.Exception.ToString());
          } else if (task.IsCompleted) {
            DebugLog("Transaction complete.");
          }
        });
    }

    // Render the log output in a scroll view.
    void GUIDisplayLog() {
      scrollViewVector = GUILayout.BeginScrollView(scrollViewVector);
      GUILayout.Label(logText);
      GUILayout.EndScrollView();
    }

    // Render the buttons and other controls.
    void GUIDisplayControls() {
      if (UIEnabled) {
        controlsScrollViewVector =
            GUILayout.BeginScrollView(controlsScrollViewVector);
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Room Code:", GUILayout.Width(Screen.width * 0.20f));
        roomCode = GUILayout.TextField(roomCode);
        GUILayout.EndHorizontal();

        GUILayout.Space(20);

        // GUILayout.BeginHorizontal();
        // GUILayout.Label("Score:", GUILayout.Width(Screen.width * 0.20f));
        // int.TryParse(GUILayout.TextField(score.ToString()), out score);
        // GUILayout.EndHorizontal();

        GUILayout.Space(20);

        if (!String.IsNullOrEmpty(roomCode) && GUILayout.Button("Create Room")) {
          AddScore();
        }

        GUILayout.Space(20);

        // if (GUILayout.Button("Go Offline")) {
        //   FirebaseDatabase.DefaultInstance.GoOffline();
        // }
        //
        // GUILayout.Space(20);

        if (GUILayout.Button("Go Online")) {
          FirebaseDatabase.DefaultInstance.GoOnline();
        }

        GUILayout.EndVertical();
        GUILayout.EndScrollView();
      }
    }

    void GUIDisplayLeaders() {
      GUI.skin.box.fontSize = 36;
      scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true);
      GUILayout.BeginVertical(GUI.skin.box);

      foreach (string item in rooms) {
        GUILayout.Label(item, GUI.skin.box, GUILayout.ExpandWidth(true));
      }

      GUILayout.EndVertical();
      GUILayout.EndScrollView();
    }

    // Render the GUI:
    void OnGUI() {
      GUI.skin = fb_GUISkin;
      if (dependencyStatus != DependencyStatus.Available) {
        GUILayout.Label("One or more Firebase dependencies are not present.");
        GUILayout.Label("Current dependency status: " + dependencyStatus.ToString());
        return;
      }
      Rect logArea, controlArea, leaderBoardArea;

      if (Screen.width < Screen.height) {
        // Portrait mode
        controlArea = new Rect(0.0f, 0.0f, Screen.width, Screen.height * 0.5f);
        leaderBoardArea = new Rect(0, Screen.height * 0.5f, Screen.width * 0.5f, Screen.height * 0.5f);
        logArea = new Rect(Screen.width * 0.5f, Screen.height * 0.5f, Screen.width * 0.5f, Screen.height * 0.5f);
      } else {
        // Landscape mode
        controlArea = new Rect(0.0f, 0.0f, Screen.width * 0.5f, Screen.height);
        leaderBoardArea = new Rect(Screen.width * 0.5f, 0, Screen.width * 0.5f, Screen.height * 0.5f);
        logArea = new Rect(Screen.width * 0.5f, Screen.height * 0.5f, Screen.width * 0.5f, Screen.height * 0.5f);
      }

      GUILayout.BeginArea(leaderBoardArea);
      GUIDisplayLeaders();
      GUILayout.EndArea();

      GUILayout.BeginArea(logArea);
      GUIDisplayLog();
      GUILayout.EndArea();

      GUILayout.BeginArea(controlArea);
      GUIDisplayControls();
      GUILayout.EndArea();
    }
  }
}
