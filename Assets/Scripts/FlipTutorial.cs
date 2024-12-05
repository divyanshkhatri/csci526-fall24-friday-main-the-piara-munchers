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
        Debug.Log("Collision detected with: " + collision.collider.name);
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("Collision detected with Player");
            if (!hasFlipped)
            {
                ShowMessage("Press left key to flip the world!", 650);

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
                ShowMessage("Move forward to get to the flag", 650);
                yield return new WaitForSeconds(10f);
                HideMessage();
            }
            yield return null;
        }
    }

    private void ShowMessage(string message, float backgroundWidth = 0)
    {
        messageText.text = message;
        messageText.alignment = TextAlignmentOptions.Center;
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
