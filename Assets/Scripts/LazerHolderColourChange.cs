using UnityEngine;
using System.Collections;

public class LazerHolderColourChange : MonoBehaviour
{
    public GameObject featureToToggle;
    private bool isPaused = false;
    private Renderer featureRenderer;
    private Color initialColor = Color.white;
    private Color blinkColor = Color.red;

    void Start()
    {
        PauseManager.OnPause += HandlePause;

        if (featureToToggle != null)
        {
            featureRenderer = featureToToggle.GetComponent<Renderer>();
            if (featureRenderer != null)
            {
                featureRenderer.material.color = initialColor; // Set initial color to white
                StartCoroutine(StartBlinkingAfterDelay(3f)); // Start blinking after 3 seconds (initial delay)
            }
            else
            {
                Debug.LogError("featureToToggle does not have a Renderer component.");
            }
        }
        else
        {
            Debug.LogError("featureToToggle is not assigned in the Inspector.");
        }
    }

    void OnDestroy()
    {
        PauseManager.OnPause -= HandlePause;
    }

    void HandlePause(bool pauseStatus)
    {
        isPaused = pauseStatus;
    }

    IEnumerator StartBlinkingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(ToggleFeatureCoroutine());
    }

    IEnumerator ToggleFeatureCoroutine()
    {
        while (true)
        {
            if (!isPaused && featureRenderer != null)
            {
                // Change color to red for 2 seconds
                featureRenderer.material.color = blinkColor;
                yield return new WaitForSeconds(2f);

                // Change color back to white for 8 seconds
                featureRenderer.material.color = initialColor;
                yield return new WaitForSeconds(8f);
            }
            else
            {
                yield return null; // Ensures the loop keeps checking for pause status
            }
        }
    }
}
