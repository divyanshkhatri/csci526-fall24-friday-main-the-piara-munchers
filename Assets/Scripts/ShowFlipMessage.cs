using System.Collections;
using System.Collections.Generic;
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
        if (GetComponent<Collider2D>() == null)
        {
            gameObject.AddComponent<BoxCollider2D>();
        }

        if (GetComponent<Rigidbody2D>() == null)
        {
            Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
            rb.isKinematic = true;
        }
    }

    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collisionOccured && collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collision detected");
            StartCoroutine(ShowMessage());
            collisionOccured = true;
        }
    }

    IEnumerator ShowMessage()
    {
        messageText.text = "Guess, you should flip the world!";
        backgroundImage.enabled = true;
        messageText.enabled = true;
        yield return new WaitForSeconds(2);
        messageText.enabled = false;
        backgroundImage.enabled = false;
    }
}
