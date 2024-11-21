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
        // Ensure the object has a trigger collider
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if (collider == null)
        {
            collider = gameObject.AddComponent<BoxCollider2D>();
        }
        collider.isTrigger = true; // Make it a trigger

        // Ensure the object has a Rigidbody2D
        if (GetComponent<Rigidbody2D>() == null)
        {
            Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
            rb.isKinematic = true; // Kinematic to avoid physics-based interactions
        }

        // Initially hide the message
        messageText.enabled = false;
        backgroundImage.enabled = false;
    }

    // Use trigger detection
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!collisionOccured && other.CompareTag("Player"))
        {
            Debug.Log("Player triggered the message display");
            ShowMessage();
            collisionOccured = true; // Prevent repeat triggering
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
