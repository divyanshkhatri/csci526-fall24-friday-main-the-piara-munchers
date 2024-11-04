using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Add this for Text
using Proyecto26; // Add this for RestClient
using UnityEngine.SceneManagement; // Add this for SceneManager

public class FireBaseAnalytics : MonoBehaviour
{

    public float completionTime; // Add this for completion time
    public PlayerMovement playerController; // Ensure PlayerController is correctly referenced


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PostToFireBase() // Make this method public
    {
        // Create an instance of EventData and populate it with the event details
        EventData myEvent = new EventData
        {
            level_index = SceneManager.GetActiveScene().buildIndex,
            completion_time = completionTime,
            hit_count = playerController.hitCount,
            lives_remaining = 3 - playerController.hitCount
        };

        // Convert the myEvent object to JSON format
        string json = JsonUtility.ToJson(myEvent);

        // Post the JSON data to Firebase
        RestClient.Post("https://flipquest-9db7e-default-rtdb.firebaseio.com/events.json", json);
    }

}
