using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LazerScript : MonoBehaviour
{
    public PlayerMovement playerController;
    public Button restartButton;
    public GameObject levelFailPanel;

    void Update() {
        if (!playerController.canMove) {
            playerController.rb.velocity = Vector2.zero;
            return;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerController.spriteRenderer.color = Color.red;
            playerController.canMove = false;
            playerController.rb.velocity = Vector2.zero;
            levelFailPanel.SetActive(true);
            Debug.Log("Level fail panel shown");
        }
        else
        {
            playerController.canMove = true;
        }
    }

    public void RestartScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
