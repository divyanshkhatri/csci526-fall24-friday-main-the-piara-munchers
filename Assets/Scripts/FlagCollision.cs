// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.SceneManagement;
// using UnityEngine.Analytics;
// using System.Collections.Generic;  
// public class FlagCollision : MonoBehaviour
// {
//     public GameObject levelCompletePanel;
//     public PlayerMovement playerController;
//     public Button nextLevelButton;
//     void Start() {
//         nextLevelButton.onClick.AddListener(NextLevel);
//     }
//     void OnTriggerEnter2D(Collider2D other)
//     {
//         Debug.Log("Current Build Index: " + SceneManager.GetActiveScene().buildIndex);

//         if (other.gameObject.CompareTag("Player") && Collectible.collectiblesRemaining == 0)
//         {
//            AnalyticsResult analyticsResult =  Analytics.CustomEvent("Level Win", new Dictionary<string, object>
//             {
//                 { "Level", SceneManager.GetActiveScene().buildIndex }
//             });
//            Debug.Log("Analytics Result from FlagCollision : " + analyticsResult);
//             levelCompletePanel.SetActive(true);
//             if (SceneManager.GetActiveScene().buildIndex == 2) {
//               nextLevelButton.gameObject.SetActive(false);
//             }
//             playerController.canMove = false;

//         } else {
//             playerController.canMove = true;
//         }
//     }

//     public void NextLevel() {
//       Debug.Log("Next Level");
//       SceneController.instance.NextLevel();
//     }
// }


using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;
using Unity.Services.Analytics;
using Unity.Services.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

public class FlagCollision : MonoBehaviour
{
    public GameObject levelCompletePanel;
    public PlayerMovement playerController;

    private float startTime;
    public Button nextLevelButton;
    private bool isAnalyticsInitialized = false;

    public FireBaseAnalytics firebaseAnalytics;

    async void Start()
    {
        nextLevelButton.onClick.AddListener(NextLevel);
        startTime = Time.time;
        await InitializeUnityServices();

    }

    async Task InitializeUnityServices()
    {
        try
        {
            await UnityServices.InitializeAsync();
            // Check if Analytics service is available
            if (AnalyticsService.Instance != null)
            {
                AnalyticsService.Instance.StartDataCollection();
                isAnalyticsInitialized = true;
                Debug.Log("Unity Services and Analytics initialized successfully.");
            }
            else
            {
                Debug.LogWarning("Analytics service is not available.");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to initialize Unity Services: " + ex.Message);
            isAnalyticsInitialized = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Current Build Index: " + SceneManager.GetActiveScene().buildIndex);
        if (other.gameObject.CompareTag("Player") && Collectible.collectiblesRemaining == 0)
        {
            if (isAnalyticsInitialized)
            {
                try
                {
                    // Log with Unity Analytics
                    float completionTime = Time.time - startTime;
                    CustomEvent myEvent = new CustomEvent("level_completed") {
                    { "level_index", SceneManager.GetActiveScene().buildIndex },
                    { "completion_time", completionTime},
                    { "hit_count", playerController.hitCount},
                    { "lives_remaining", 3 - playerController.hitCount},
                };

                    AnalyticsService.Instance.RecordEvent(myEvent);
                    Debug.Log("Analytics event 'level_completed' sent successfully");
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("Failed to send analytics event: " + ex.Message);
                }
            }

            // Post event to Firebase
            if (firebaseAnalytics != null)
            {
                firebaseAnalytics.completionTime = Time.time - startTime;
                firebaseAnalytics.PostToFireBase();
                Debug.Log("Posted to Firebase");
            }
            else
            {
                Debug.LogError("FirebaseAnalytics reference is missing in FlagCollision.");
            }

            levelCompletePanel.SetActive(true);
            if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                nextLevelButton.gameObject.SetActive(false);
            }
            playerController.canMove = false;
        }
        else
        {
            playerController.canMove = true;
        }
    }
    public void NextLevel()
    {
        Debug.Log("Next Level");
        SceneController.instance.NextLevel();
    }
}