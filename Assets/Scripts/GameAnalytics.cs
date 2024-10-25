using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;  // Required for file I/O

public class GameAnalytics : MonoBehaviour
{
    public static GameAnalytics instance;
    private int deathCount = 0;
    private int attemptCount = 0;
    private bool levelCompleted = false;
    
    private List<GameSessionData> gameSessions = new List<GameSessionData>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void RecordDeath()
    {
        deathCount++;
    }

    public void RecordAttempt()
    {
        attemptCount++;
        // Each new attempt adds a new session
        gameSessions.Add(new GameSessionData { Attempts = attemptCount, Deaths = deathCount, LevelCompleted = levelCompleted });
    }

    public void RecordCompletion()
    {
        levelCompleted = true;
        SaveData();  // Optionally save data at the end of the session
    }

    public void SaveData()
    {
        // Save session data as CSV
        string filePath = Path.Combine(Application.persistentDataPath, "game_analytics.csv");
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine("Attempt,Deaths,LevelCompleted");
            foreach (var session in gameSessions)
            {
                writer.WriteLine($"{session.Attempts},{session.Deaths},{session.LevelCompleted}");
            }
        }

        Debug.Log($"Analytics data saved to {filePath}");
    }

    [System.Serializable]
    public class GameSessionData
    {
        public int Attempts;
        public int Deaths;
        public bool LevelCompleted;
    }
}
