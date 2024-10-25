using UnityEngine;
using System.Collections;

public class Lazerblinker : MonoBehaviour
{
    // Reference to the feature you want to toggle (e.g., a GameObject or a Component)
    public GameObject featureToToggle;
    private bool isFeatureEnabled = true;

    // Start is called before the first frame update
    void Start()
    {
        if (featureToToggle != null)
        {
            // Ensure the initial state is set
            featureToToggle.SetActive(isFeatureEnabled);

            // Start the toggling coroutine
            StartCoroutine(ToggleFeatureCoroutine());
        }
        else
        {
            Debug.LogError("featureToToggle is not assigned in the Inspector.");
        }
    }

    // Coroutine that toggles the feature every 5 seconds
    IEnumerator ToggleFeatureCoroutine()
    {
        while (true)
        {
            // Wait for 5 seconds
            yield return new WaitForSeconds(5f);

            // Check if the feature still exists before toggling
            if (featureToToggle != null)
            {
                // Toggle the feature
                isFeatureEnabled = !isFeatureEnabled;
                featureToToggle.SetActive(isFeatureEnabled); // Enable/Disable the feature
            }
            else
            {
                Debug.LogWarning("featureToToggle has been destroyed or is not available.");
                break;
            }
        }
    }
}
