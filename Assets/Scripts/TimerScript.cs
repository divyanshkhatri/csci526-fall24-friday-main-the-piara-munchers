using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime;
    private float initialTime;
    private bool isWarningActive = false;
    private float blinkInterval = 0.5f;
    private float blinkTimer = 0f;
    private bool isTextVisible = true;
    public GameObject levelFailPanel;
    public PlayerMovement playerController;
    private bool isTimerStopped = false;
    private bool isPaused = false;
    public ClockRotation clockRotation;

    void Start()
    {
        initialTime = remainingTime;
        PauseManager.OnPause += HandlePause;
    }

    void OnDestroy()
    {
        PauseManager.OnPause -= HandlePause;
    }

    void HandlePause(bool pauseStatus)
    {
        isPaused = pauseStatus;
    }

    void Update()
    {
        if (!isTimerStopped && !isPaused && remainingTime > 0) {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= initialTime * 0.2f) {
                HandleWarningState();
            }
        }
        else if (remainingTime <= 0) {
            remainingTime = 0;
            isWarningActive = false;
            if (clockRotation != null) {
                clockRotation.StopRotation();
            }
            timerText.enabled = true;
            timerText.color = Color.red;
            levelFailPanel.SetActive(true);
            playerController.canMove = false;
            SessionManager.Instance.PostSessionDataToFireBase();
        }

        int mins = Mathf.FloorToInt(remainingTime / 60);
        int sec = Mathf.FloorToInt(remainingTime % 60);

        timerText.text = string.Format("{0:00}:{1:00}", mins, sec);

        if (!isWarningActive && remainingTime > 0) {
            timerText.color = Color.white;
            timerText.enabled = true;
        }
    }

    private void HandleWarningState()
    {
        isWarningActive = true;
        timerText.color = Color.red;

        blinkTimer += Time.deltaTime;
        if (blinkTimer >= blinkInterval)
        {
            blinkTimer = 0f;
            isTextVisible = !isTextVisible;
            timerText.enabled = isTextVisible;
        }
    }

    public void StopTimer()
    {
        isTimerStopped = true;
        Debug.Log("Timer stopped at: " + remainingTime);
    }

    void updateTimer(float currentTime) {
        currentTime += 1;

        float mins = Mathf.FloorToInt(currentTime / 60);
        float sec = Mathf.FloorToInt(currentTime % 60);

        timerText.text = string.Format("{0:00}:{1:00}", mins, sec);
    }
}
