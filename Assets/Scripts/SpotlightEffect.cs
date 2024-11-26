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
        if (focusTargets.Length > 0)
        {
            FocusOnTarget(currentTargetIndex);
        }
    }

    public void FocusOnTarget(int index)
    {
        if (index < 0 || index >= focusTargets.Length) return;

        Debug.Log("Focusing on: " + focusTargets[index].name);

        // Convert target's world position to local canvas space
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            RectTransformUtility.WorldToScreenPoint(Camera.main, focusTargets[index].position),
            canvas.worldCamera,
            out localPoint
        );

        StartCoroutine(MoveSpotlight(localPoint));
        currentTargetIndex = index;
    }

    public void NextFocus()
    {
        int nextIndex = (currentTargetIndex + 1) % focusTargets.Length;
        FocusOnTarget(nextIndex);
    }

    private IEnumerator MoveSpotlight(Vector2 targetPosition)
    {
        Debug.Log("Starting MoveSpotlight");

        while (Vector2.Distance(spotlightMask.anchoredPosition, targetPosition) > 0.1f)
        {
            spotlightMask.anchoredPosition = Vector2.Lerp(spotlightMask.anchoredPosition, targetPosition, Time.deltaTime * transitionSpeed);
            Debug.Log($"Moving to: {targetPosition} | Current: {spotlightMask.anchoredPosition}");
            yield return null;
        }

        spotlightMask.anchoredPosition = targetPosition;
        Debug.Log("Spotlight moved to target position");
    }
}
