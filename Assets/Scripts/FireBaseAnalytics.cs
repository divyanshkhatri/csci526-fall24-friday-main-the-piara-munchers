using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Proyecto26;
using UnityEngine.SceneManagement;

public class FireBaseAnalytics : MonoBehaviour
{

    public float completionTime;
    public PlayerMovement playerController;

    void Start()
    {

    }

    void Update()
    {

    }

    public void PostToFireBase(bool finish)
    {
        EventData myEvent = new EventData
        {
            session_id = SessionManager.Instance.sessionData.sessionID,
            level_index = SceneManager.GetActiveScene().buildIndex,
            completion_time = completionTime,
            hit_count = playerController.hitCount,
            lives_remaining = 3 - playerController.hitCount,
            flips = playerController.flipCount,
            finish = finish
        };

        string json = JsonUtility.ToJson(myEvent);

        RestClient.Post("https://flipquest-9db7e-default-rtdb.firebaseio.com/events.json", json);
    }

}
