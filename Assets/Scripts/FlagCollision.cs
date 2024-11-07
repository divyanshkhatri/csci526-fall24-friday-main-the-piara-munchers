using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FlagCollision : MonoBehaviour
{
    public GameObject levelCompletePanel;
    public PlayerMovement playerController;

    private float startTime;
    public Button nextLevelButton;
    public FireBaseAnalytics firebaseAnalytics;
    public TimerScript timerScript;
    private bool hasTriggered = false;

    void Start()
    {
        nextLevelButton.onClick.AddListener(NextLevel);
        startTime = Time.time;
        if (timerScript == null)
        {
            Debug.LogError("TimerScript reference is missing in FlagCollision.");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTriggered && other.gameObject.CompareTag("Player") && Collectible.collectiblesRemaining == 0)
        {
            Debug.Log("Player reached the flag, stopping timer.");
            if (timerScript != null)
            {
                Debug.Log("Calling StopTimer from FlagCollision.");
                timerScript.StopTimer();
            }
            else
            {
                Debug.LogError("TimerScript reference is missing in FlagCollision.");
            }
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
        SessionManager.Instance.isDataPosted = false;
        SceneController.instance.NextLevel();
    }
}
