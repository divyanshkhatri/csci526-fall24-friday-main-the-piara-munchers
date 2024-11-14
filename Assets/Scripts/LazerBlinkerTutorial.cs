using UnityEngine;
using System.Collections;

public class LazerBlinkerTutorial : MonoBehaviour
{
    public GameObject featureToToggle;
    private bool isFeatureEnabled = true;
    private bool isPaused = false;

    void Start()
    {
        PauseManager.OnPause += HandlePause;
        if (featureToToggle != null)
        {
            featureToToggle.SetActive(isFeatureEnabled);

            StartCoroutine(ToggleFeatureCoroutine());
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
    IEnumerator ToggleFeatureCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);

            if (isPaused)
            {
                continue;
            }

            if (featureToToggle != null)
            {
                isFeatureEnabled = !isFeatureEnabled;
                featureToToggle.SetActive(isFeatureEnabled);
            }
            else
            {
                Debug.LogWarning("featureToToggle has been destroyed or is not available.");
                break;
            }
        }
    }
}
