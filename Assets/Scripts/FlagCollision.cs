using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
// using UnityEngine.Analytics;
// using Unity.Services.Analytics;
using Unity.Services.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

public class FlagCollision : MonoBehaviour
{
    public GameObject levelCompletePanel;
    public PlayerMovement playerController;

    private float startTime;
    public Button nextLevelButton;
    public FireBaseAnalytics firebaseAnalytics;

    void Start()
    {
        nextLevelButton.onClick.AddListener(NextLevel);
        startTime = Time.time;

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Current Build Index: " + SceneManager.GetActiveScene().buildIndex);
        if (other.gameObject.CompareTag("Player") && Collectible.collectiblesRemaining == 0)
        {
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
