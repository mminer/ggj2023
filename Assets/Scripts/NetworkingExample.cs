using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkingExample : MonoBehaviour
{
    public string roomCode = "test";
    private bool createdRoom = false;

    // Update is called once per frame
    void Update()
    {
        var network = Services.Get<NetworkingService>();
        
        if (network.initialized && !createdRoom)
        {
            createdRoom = true;
            CreateTopic();
        }
    }

    async void CreateTopic()
    {
        var network = Services.Get<NetworkingService>();
        var createRoomResult = await network.CreateTopicViaCloudFunction(roomCode);
        
        network.SubscribeToTopic(roomCode);
        InvokeRepeating("SendTestMessage", 10f, 5f);
    }

    async void SendTestMessage()
    {
        var network = Services.Get<NetworkingService>();
        var data = new WWWForm();
        data.AddField("hello", "world");
        data.AddField("someInt", (int)Time.time);
        var result = await network.SendMessageToCloudFunction(roomCode, "test-event", "1", data);
    }
}
