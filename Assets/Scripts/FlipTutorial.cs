using UnityEngine;
using TMPro;
using System.Collections;

public class FlipTutorial : MonoBehaviour
{
    public GameObject player;
    public TextMeshProUGUI messageText;
    public GameObject messageBackground;
    private bool hasFlipped = false;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (!hasFlipped)
            {
                ShowMessage("Flip the world to traverse.", 450);

                // Wait for player input in Update
                StartCoroutine(WaitForFlipInput());
            }
        }
    }

    private IEnumerator WaitForFlipInput()
    {
        while (!hasFlipped)
        {
            if (PlayerHasFlipped())
            {
                hasFlipped = true;
                ShowMessage("Move forward to get to the flag", 550);
                yield return new WaitForSeconds(3f);
                HideMessage();
            }
            yield return null;
        }
    }

    private void ShowMessage(string message, float backgroundWidth = 0)
    {
        messageText.text = message;
        messageText.gameObject.SetActive(true);
        messageBackground.SetActive(true);
        RectTransform rt = messageBackground.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(backgroundWidth, rt.sizeDelta.y);
    }

    private void HideMessage()
    {
        messageText.gameObject.SetActive(false);
        messageBackground.SetActive(false);
    }

    private bool PlayerHasFlipped()
    {
        return Input.GetKeyDown(KeyCode.LeftArrow);
    }
}
