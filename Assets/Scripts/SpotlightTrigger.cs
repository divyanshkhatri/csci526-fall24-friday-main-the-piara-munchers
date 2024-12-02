using System.Collections;
using UnityEngine;

public class SpotlightTrigger : MonoBehaviour
{
    public GameObject spotlightOverlay;       // The spotlight overlay GameObject
    public Transform targetTransform;        // The target (e.g., heart) to focus on
    public float fadeDuration = 0.5f;        // Duration for fading in/out
    public float spotlightDuration = 2f;     // Duration to keep the spotlight active
    public float moveSpeed = 5f;             // Speed for spotlight movement
    public float scaleFactor = 1.5f;         // Amount by which the spotlight scales up

    private CanvasGroup canvasGroup;
    private RectTransform spotlightRectTransform;

    private void Start()
    {
        canvasGroup = spotlightOverlay.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = spotlightOverlay.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 0; // Ensure it's initially invisible

        spotlightRectTransform = spotlightOverlay.GetComponent<RectTransform>();
        if (spotlightRectTransform == null)
        {
            spotlightRectTransform = spotlightOverlay.AddComponent<RectTransform>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(ShowSpotlight());
        }
    }

    private IEnumerator ShowSpotlight()
    {
        Vector3 originalScale = spotlightRectTransform.localScale;
        Vector3 targetScale = originalScale * scaleFactor;

        // Fade in and move to the target
        yield return StartCoroutine(FadeAndMoveSpotlight(canvasGroup, 0, 1, originalScale, targetScale, fadeDuration));

        // Keep the spotlight active for the duration
        yield return new WaitForSeconds(spotlightDuration);

        // Fade out and move back to the original state
        yield return StartCoroutine(FadeAndMoveSpotlight(canvasGroup, 1, 0, targetScale, originalScale, fadeDuration));
    }

    private IEnumerator FadeAndMoveSpotlight(CanvasGroup cg, float startAlpha, float endAlpha, Vector3 startScale, Vector3 endScale, float duration)
    {
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            // Smoothly interpolate alpha (fade)
            cg.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);

            // Smoothly interpolate scale
            spotlightRectTransform.localScale = Vector3.Lerp(startScale, endScale, elapsed / duration);

            // Smoothly move the spotlight to the target position
            Vector3 targetPosition = Camera.main.WorldToScreenPoint(targetTransform.position);
            spotlightRectTransform.position = Vector3.Lerp(spotlightRectTransform.position, targetPosition, Time.deltaTime * moveSpeed);

            yield return null;
        }

        cg.alpha = endAlpha;
        spotlightRectTransform.localScale = endScale;
    }
}
