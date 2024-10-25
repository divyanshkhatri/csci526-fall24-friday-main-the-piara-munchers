using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Analytics;
using Unity.Services.Core;

public class UserConsent : MonoBehaviour
{
    public GameObject consentPanel;  // The UI panel where you ask for consent
    public Button acceptButton;
    public Button declineButton;

    private void Start()
    {
        // Check if user has already given consent
        if (PlayerPrefs.HasKey("AnalyticsConsent"))
        {
            bool consentGiven = PlayerPrefs.GetInt("AnalyticsConsent") == 1;
            if (consentGiven)
            {
                InitializeAnalytics();
            }
        }
        else
        {
            // Show consent UI if no decision has been made
            consentPanel.SetActive(true);
        }

        acceptButton.onClick.AddListener(OnAcceptConsent);
        declineButton.onClick.AddListener(OnDeclineConsent);
    }

    private async void InitializeAnalytics()
    {
        try
        {
            await UnityServices.InitializeAsync();
            AnalyticsService.Instance.StartDataCollection();
            Debug.Log("Analytics has been started");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to initialize analytics: " + ex.Message);
        }
    }

    private void OnAcceptConsent()
    {
        // Save consent status
        PlayerPrefs.SetInt("AnalyticsConsent", 1);
        consentPanel.SetActive(false);

        // Start analytics tracking
        InitializeAnalytics();
    }

    private void OnDeclineConsent()
    {
        // Save that consent was denied
        PlayerPrefs.SetInt("AnalyticsConsent", 0);
        consentPanel.SetActive(false);

        // Do not start analytics
        Debug.Log("Analytics declined by user.");
    }
}
