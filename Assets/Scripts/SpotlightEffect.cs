using System.Collections;
using UnityEngine;

public class SpotlightEffect : MonoBehaviour
{
    public RectTransform spotlightMask; // Spotlight mask RectTransform
    public RectTransform[] focusTargets; // Array of targets to focus on
    public Canvas canvas; // Assign your Canvas here (required for proper space conversion)
    public float transitionSpeed = 5f; // Speed of spotlight transition
    private int currentTargetIndex = 0;

    void Start()
    {
        // Hide spotlight initially
        spotlightMask.gameObject.SetActive(false);
    }

    public void FocusOnTarget(int index)
    {
        if (index < 0 || index >= focusTargets.Length) return;

        Debug.Log("Focusing on: " + focusTargets[index].name);

        // Convert target's position to canvas local space
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            RectTransformUtility.WorldToScreenPoint(Camera.main, focusTargets[index].position),
            canvas.worldCamera,
            out localPoint
        );

        // Show the spotlight and start moving it
        spotlightMask.gameObject.SetActive(true);
        StartCoroutine(MoveSpotlight(localPoint));
        currentTargetIndex = index;
    }

    private IEnumerator MoveSpotlight(Vector2 targetPosition)
    {
        Debug.Log("Starting MoveSpotlight");

        while (Vector2.Distance(spotlightMask.anchoredPosition, targetPosition) > 0.1f)
        {
            // Smoothly move the spotlight
            spotlightMask.anchoredPosition = Vector2.Lerp(
                spotlightMask.anchoredPosition,
                targetPosition,
                Time.deltaTime * transitionSpeed
            );
            yield return null;
        }

        spotlightMask.anchoredPosition = targetPosition;
        Debug.Log("Spotlight moved to target position");

        // Wait for a brief moment to highlight the target, then hide the spotlight
        yield return new WaitForSeconds(1f);
        spotlightMask.gameObject.SetActive(false);
    }

    public void TriggerFocusOnHeart()
    {
        // Assume the heart UI element is the first in the focusTargets array
        FocusOnTarget(0); // Update the index if the heart is not at 0
    }
}
