using UnityEngine;
using TMPro;

public class JumpTutorial : MonoBehaviour
{
    public TextMeshProUGUI tutorialText;
    public GameObject backgroundImage;

    void Start()
    {
        if (tutorialText == null)
        {
            Debug.LogError("TextMeshProUGUI component is not assigned.");
            return;
        }
        tutorialText.enabled = false;

        if (backgroundImage != null)
        {
            backgroundImage.SetActive(false);
        }

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
        if (collision.collider.CompareTag("Player") && collision.transform.position.y >= -50 && collision.transform.position.y <= -41)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                Vector2 normal = contact.normal;
                if (Mathf.Abs(normal.x) > Mathf.Abs(normal.y))
                {
                    tutorialText.text = "Press Space to Jump";
                    tutorialText.enabled = true;
                    if (backgroundImage != null)
                    {
                        backgroundImage.SetActive(true);
                    }
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
            if (backgroundImage != null)
            {
                backgroundImage.SetActive(false);
            }
        }
    }
}
