<<<<<<< HEAD
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
=======
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
>>>>>>> ebe98c7e745507b165b5fbe38e12d5ddb222c013

public class FlagCollision : MonoBehaviour
{
    public GameObject levelCompletePanel;
    public PlayerMovement playerController;
<<<<<<< HEAD

    private float startTime;
    public Button nextLevelButton;
    private bool isAnalyticsInitialized = false;

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
                    float completionTime = Time.time - startTime;
                    CustomEvent myEvent = new CustomEvent("level_completed") {
                        { "level_index", SceneManager.GetActiveScene().buildIndex },
                        { "completion_time", completionTime},
                        { "hit_count", playerController.hitCount},
                        { "lives_remaining", (3 - playerController.hitCount)},
                    };


                    AnalyticsService.Instance.RecordEvent(myEvent);

                    Debug.Log("Analytics event 'level_completed' sent successfully");
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("Failed to send analytics event: " + ex.Message);
                }
            }
            else
            {
                Debug.LogWarning("Analytics is not initialized or enabled.");
            }

            levelCompletePanel.SetActive(true);
            if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                nextLevelButton.gameObject.SetActive(false);
            }
            playerController.canMove = false;
        }
        else
        {
=======
    public Button nextLevelButton;
    void Start() {
        nextLevelButton.onClick.AddListener(NextLevel);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> 9012d2579bda88cba695b229e29cda736f046d4b
        Debug.Log("Current Build Index: " + SceneManager.GetActiveScene().buildIndex);
        
        if (other.gameObject.CompareTag("Player") && Collectible.collectiblesRemaining == 0)
        {
            levelCompletePanel.SetActive(true);
            if (SceneManager.GetActiveScene().buildIndex == 2) {
<<<<<<< HEAD
=======
=======
        if (other.gameObject.CompareTag("Player") && Collectible.collectiblesRemaining == 0)
        {
            levelCompletePanel.SetActive(true);
            if (SceneManager.GetActiveScene().buildIndex == 1) {
>>>>>>> 074fd6c3a85606bdc5b933751e7cdfc8c8c50d00
>>>>>>> 9012d2579bda88cba695b229e29cda736f046d4b
              nextLevelButton.gameObject.SetActive(false);
            }
            playerController.canMove = false;

        } else {
>>>>>>> ebe98c7e745507b165b5fbe38e12d5ddb222c013
            playerController.canMove = true;
        }
    }

<<<<<<< HEAD
    public void NextLevel()
    {
        Debug.Log("Next Level");
        SceneController.instance.NextLevel();
    }
}
=======
    public void NextLevel() {
      Debug.Log("Next Level");
      SceneController.instance.NextLevel();
    }
}
>>>>>>> ebe98c7e745507b165b5fbe38e12d5ddb222c013
