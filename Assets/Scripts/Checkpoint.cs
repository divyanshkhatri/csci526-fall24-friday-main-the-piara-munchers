using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public string checkpointID;
    private bool isActivated = false;

    private void Start()
    {
        SessionManager.Instance.RegisterCheckpoint(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isActivated)
        {
            LogCheckpointData();
            isActivated = true;
        }
    }

    private void LogCheckpointData()
    {
        SessionManager.Instance.AddCheckpoint(checkpointID);
        Debug.Log("Session ID: " + SessionManager.Instance.sessionData.sessionID + ", Checkpoint reached: " + checkpointID);
    }
}
