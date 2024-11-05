using System;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;

[Serializable]
public class CheckpointEntry
{
    public string checkpointID;
    public int hitCount;

    public CheckpointEntry(string checkpointID, int hitCount)
    {
        this.checkpointID = checkpointID;
        this.hitCount = hitCount;
    }
}

[Serializable]
public class SessionData
{
    public string sessionID;
    public List<CheckpointEntry> crossedCheckpoints;
    public string sessionTime;

    public SessionData()
    {
        sessionID = Guid.NewGuid().ToString();
        crossedCheckpoints = new List<CheckpointEntry>();
        sessionTime = DateTime.Now.ToString("o");
    }

    public void AddCheckpoint(string checkpointID, int hitCount)
    {
        Debug.Log("AddCheckpoint called for: " + checkpointID);
        var checkpoint = crossedCheckpoints.Find(c => c.checkpointID == checkpointID);
        if (checkpoint != null)
        {
            checkpoint.hitCount += hitCount;
        }
        else
        {
            crossedCheckpoints.Add(new CheckpointEntry(checkpointID, hitCount));
        }
        Debug.Log("Checkpoint " + checkpointID + " crossed with " + hitCount + " hits.");
    }

    public void PostToFireBase()
    {
        sessionTime = DateTime.Now.ToString("o");
        Debug.Log("Session ID: " + sessionID);
        Debug.Log("Session Time: " + sessionTime);

        string json = JsonUtility.ToJson(this);
        Debug.Log("JSON: " + json);

        RestClient.Post("https://flipquest-9db7e-default-rtdb.firebaseio.com/sessions.json", json)
            .Then(response => Debug.Log("Data posted successfully."))
            .Catch(error => Debug.LogError("Error posting data: " + error));
    }
}
