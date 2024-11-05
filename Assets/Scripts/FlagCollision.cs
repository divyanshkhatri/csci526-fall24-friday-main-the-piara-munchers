using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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

    private bool hasTriggered = false;

    void Start()
    {
        nextLevelButton.onClick.AddListener(NextLevel);
        startTime = Time.time;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTriggered && other.gameObject.CompareTag("Player") && Collectible.collectiblesRemaining == 0)
        {
            hasTriggered = true;
            Debug.Log("Current Build Index: " + SceneManager.GetActiveScene().buildIndex);

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
                SessionManager.Instance.PostSessionDataToFireBase();
                nextLevelButton.gameObject.SetActive(false);
            }
            playerController.canMove = false;
        }
    }

    public void NextLevel()
    {
        Debug.Log("Next Level");
        SceneController.instance.NextLevel();
    }
}
