using UnityEngine;
using TMPro;
using System.Collections;

public class FlipTutorial : MonoBehaviour
{
    public GameObject player;
    public TextMeshProUGUI messageText;
    public GameObject messageBackground;
    private bool hasFlipped = false;

    void Update()
    {
        if (player.transform.position.y >= -80 && player.transform.position.y <= -71)
        {
            if (!hasFlipped)
            {
                ShowMessage("Flip the world to traverse.", 450);
                if (PlayerHasFlipped())
                {
                    hasFlipped = true;
                    ShowMessage("Move forward to get to the flag", 550);
                    StartCoroutine(HideMessageAfterDelay(3f));
                }
            }
        }
    }

    private void ShowMessage(string message, float backgroundWidth = 0)
    {
        messageText.text = message;
        messageText.gameObject.SetActive(true);
        messageBackground.SetActive(true);
        RectTransform rt = messageBackground.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(backgroundWidth, rt.sizeDelta.y);
        StartCoroutine(HideMessageAfterDelay(3f));
    }

    private bool PlayerHasFlipped()
    {
        return Input.GetKeyDown(KeyCode.LeftArrow);
    }

    private IEnumerator HideMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        messageText.gameObject.SetActive(false);
        messageBackground.SetActive(false);
    }
}
