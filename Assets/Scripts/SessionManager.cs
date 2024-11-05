using UnityEngine;

public class SessionManager : MonoBehaviour
{
    public static SessionManager Instance { get; private set; }
    public SessionData sessionData { get; private set; }
    public PlayerMovement playerController;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        sessionData = new SessionData();
        Debug.Log("Session ID: " + sessionData.sessionID);
    }

    public void AddCheckpoint(string checkpointID)
    {
        if (playerController == null)
        {
            Debug.LogError("PlayerController is not assigned.");
            return;
        }

        sessionData.AddCheckpoint(checkpointID, playerController.hitCount);
    }

    public void PostSessionDataToFireBase()
    {
        sessionData.PostToFireBase();
    }
}
