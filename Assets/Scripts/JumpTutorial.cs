using UnityEngine;
using TMPro;

public class JumpTutorial : MonoBehaviour
{
    public TextMeshProUGUI tutorialText;

    void Start()
    {
        if (tutorialText == null)
        {
            Debug.LogError("TextMeshProUGUI component is not assigned.");
            return;
        }
        tutorialText.enabled = false;
        
        if (GetComponent<Rigidbody2D>() == null)
        {
            gameObject.AddComponent<Rigidbody2D>().isKinematic = true;
        }
        if (GetComponent<Collider2D>() == null)
        {
            gameObject.AddComponent<BoxCollider2D>();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                Vector2 normal = contact.normal;
                if (Mathf.Abs(normal.x) > Mathf.Abs(normal.y))
                {
                    tutorialText.text = "Press Space to Jump";
                    tutorialText.enabled = true;
                    return;
                }
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            tutorialText.enabled = false;
        }
    }
}
