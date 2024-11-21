using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowFlipMessage : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public Image backgroundImage;
    public bool collisionOccured = false;

    void Start()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if (collider == null)
        {
            collider = gameObject.AddComponent<BoxCollider2D>();
        }
        collider.isTrigger = true;

        if (GetComponent<Rigidbody2D>() == null)
        {
            Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
            rb.isKinematic = true;
        }

        messageText.enabled = false;
        backgroundImage.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!collisionOccured && other.CompareTag("Player"))
        {
            Debug.Log("Player triggered the message display");
            ShowMessage();
            collisionOccured = true;
        }
    }

    void ShowMessage()
    {
        messageText.text = "Hitting the lazer kills you instantly!";
        backgroundImage.enabled = true;
        messageText.enabled = true;
        StartCoroutine(HideMessageAfterDelay());
    }

    IEnumerator HideMessageAfterDelay()
    {
        yield return new WaitForSeconds(3);
        messageText.enabled = false;
        backgroundImage.enabled = false;
    }
}
