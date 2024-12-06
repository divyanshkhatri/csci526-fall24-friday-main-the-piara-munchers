using UnityEngine;
using TMPro;
using System.Collections;

public class FlipTutorial : MonoBehaviour
{
    public GameObject player;
    public TextMeshProUGUI messageText;
    public GameObject messageBackground;
    private bool hasFlipped = false;

    // Trigger method to detect when the player enters the trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger detected with: " + other.name);
        if (other.CompareTag("Player"))
        {
            Debug.Log("Trigger detected with Player");
            if (!hasFlipped)
            {
                ShowMessage("Press left key to flip the world!", 650);

                StartCoroutine(WaitForFlipInput());
            }
        }
    }

    // Coroutine to wait for player input
    private IEnumerator WaitForFlipInput()
    {
        while (!hasFlipped)
        {
            if (PlayerHasFlipped())
            {
                hasFlipped = true;
                ShowMessage("Move forward to get to the flag", 650);
                yield return new WaitForSeconds(10f);
                HideMessage();
            }
            yield return null;
        }
    }

    // Method to show a message on the screen
    private void ShowMessage(string message, float backgroundWidth = 0)
    {
        messageText.text = message;
        messageText.alignment = TextAlignmentOptions.Center;
        messageText.gameObject.SetActive(true);
        messageBackground.SetActive(true);

        RectTransform rt = messageBackground.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(backgroundWidth, rt.sizeDelta.y);
    }

    // Method to hide the message from the screen
    private void HideMessage()
    {
        messageText.gameObject.SetActive(false);
        messageBackground.SetActive(false);
    }

    // Check if the player has pressed the left arrow key to "flip the world"
    private bool PlayerHasFlipped()
    {
        return Input.GetKeyDown(KeyCode.LeftArrow);
    }
}
