using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FlagCollision : MonoBehaviour
{
    public GameObject levelCompletePanel;
    public PlayerMovement playerController;
    public Button nextLevelButton;
    void Start() {
        nextLevelButton.onClick.AddListener(NextLevel);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && Collectible.collectiblesRemaining == 0)
        {
            levelCompletePanel.SetActive(true);
            if (SceneManager.GetActiveScene().buildIndex == 1) {
              nextLevelButton.gameObject.SetActive(false);
            }
            playerController.canMove = false;

        } else {
            playerController.canMove = true;
        }
    }

    public void NextLevel() {
      Debug.Log("Next Level");
      SceneController.instance.NextLevel();
    }
}
