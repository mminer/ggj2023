using System;
using Firebase.Extensions;
using Firebase.Messaging;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkingService : Services.Service
{
    Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
    public bool initialized = false;
    
    // Log the result of the specified task, returning true if the task
    // completed successfully, false otherwise.
    protected bool LogTaskCompletion(Task task, string operation) {
      bool complete = false;
      if (task.IsCanceled) {
        Debug.Log(operation + " canceled.");
      } else if (task.IsFaulted) {
        Debug.Log(operation + " encountered an error.");
        foreach (Exception exception in task.Exception.Flatten().InnerExceptions) {
          string errorCode = "";
          Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
          if (firebaseEx != null) {
            errorCode = String.Format("Error.{0}: ",
              ((Error)firebaseEx.ErrorCode).ToString());
          }
          Debug.Log(errorCode + exception.ToString());
        }
      } else if (task.IsCompleted) {
        Debug.Log(operation + " completed");
        complete = true;
      }
      return complete;
    }
    
    // When the app starts, check to make sure that we have
    // the required dependencies to use Firebase, and if not,
    // add them if possible.
    protected virtual void Start()
    {
      InitializeFirebase();
    }

    // Setup message event handlers.
    void InitializeFirebase() {
      Debug.Log("Init Firebase App: " + Firebase.FirebaseApp.DefaultInstance.Name);
      Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
        dependencyStatus = task.Result;
        
        if (dependencyStatus == Firebase.DependencyStatus.Available) {
          FirebaseMessaging.MessageReceived += OnMessageReceived;
          FirebaseMessaging.TokenReceived += OnTokenReceived;
          
          Debug.Log("Firebase Messaging Initialized");
          
          // This will display the prompt to request permission to receive
          // notifications if the prompt has not already been displayed before. (If
          // the user already responded to the prompt, their decision is cached by
          // the OS and can be changed in the OS settings).
          FirebaseMessaging.RequestPermissionAsync().ContinueWithOnMainThread(
            task => {
              LogTaskCompletion(task, "RequestPermissionAsync");
            }
          );
          initialized = true;
        } else {
          Debug.LogError(
            "Could not resolve all Firebase dependencies: " + dependencyStatus);
        }
      });
    }

    public async Task<bool> SendMessageToCloudFunction(string topic, string eventName, string playerId, WWWForm data)
    {
      // This is a Firebase Clound Function that expects a topic to have already been created
      // The data sent in this request must be sent to a topic that clients are subscribed to
      // There is very little validation on the server, only that these fields exist
      // Any additional fields are also passed along to the message service
      var uri = "https://us-central1-ggj2023-cbcf5.cloudfunctions.net/sendMessage";
      data.AddField("topic", topic);
      data.AddField("event", eventName);
      data.AddField("playerId", playerId);

      try
      {
        using var www = UnityWebRequest.Post(uri, data);
        var operation = www.SendWebRequest();

        while (!operation.isDone)
        {
          await Task.Yield();
        }

        if (www.result == UnityWebRequest.Result.Success) return true;
        Debug.LogError($"Failed to send message: {www.error}");
        return false;

      }
      catch (Exception e)
      {
        Debug.LogError("Request failed");
        return false;
      }
    }
    
    public async Task<bool> CreateTopicViaCloudFunction(string topic)
    {
      // This is a Firebase Clound Function that expects a unique topic that does not yet exist.
      // There is very little validation on the server, only that these fields exist
      var uri = "https://us-central1-ggj2023-cbcf5.cloudfunctions.net/createTopic";

      try
      {
        var data = new WWWForm();
        data.AddField("topic", topic);
        
        using var www = UnityWebRequest.Post(uri, data);
        var operation = www.SendWebRequest();

        while (!operation.isDone)
        {
          await Task.Yield();
        }

        if (www.result == UnityWebRequest.Result.Success) return true;
        Debug.LogError($"Failed to create topic: {www.error}");
        return false;

      }
      catch (Exception e)
      {
        Debug.LogError("Request failed");
        return false;
      }
    }

    public void SubscribeToTopic(string topic)
    {
      if (!initialized)
      {
        Debug.LogWarning("Ignoring subscription because Firebase is not initialized");
        return;
      }

      FirebaseMessaging.SubscribeAsync(topic).ContinueWithOnMainThread(task => {
        Debug.Log("SubscribeAsync: " + task);
      });
    }
    
    public void UnsubscribeFromTopic(string topic)
    {
      if (!initialized)
      {
        Debug.LogWarning("Ignoring unsubscribe because Firebase is not initialized");
        return;
      }

      FirebaseMessaging.UnsubscribeAsync(topic).ContinueWithOnMainThread(task => {
        Debug.Log("UnsubscribeAsync: " + task);
      });
    }
    
    // End our messaging session when the program exits.
    public void OnDestroy() {
      if (initialized)
      {
        FirebaseMessaging.MessageReceived -= OnMessageReceived;
        FirebaseMessaging.TokenReceived -= OnTokenReceived; 
      }
    }

    public virtual void OnMessageReceived(object sender, MessageReceivedEventArgs e) {
      Debug.Log("Received a new message");

      if (e.Message.From.Length > 0)
      {
        Debug.Log("from: " + e.Message.From);
      }
      
      if (e.Message.Data.Count > 0) {
        Debug.Log("data: " + e.Message.Data);
        
        foreach (KeyValuePair<string, string> iter in e.Message.Data) {
          Debug.Log(iter.Key + ": " + iter.Value);
        }
      }
    }

    public virtual void OnTokenReceived(object sender, TokenReceivedEventArgs token) {
      Debug.Log("Received Registration Token: " + token.Token);
    }
}
