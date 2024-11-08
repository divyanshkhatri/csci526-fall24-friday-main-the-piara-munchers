using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseManager : MonoBehaviour
{
    public static event Action<bool> OnPause;
    private bool isPaused = false;
    public KeyCode pauseKey = KeyCode.P;
    public GameObject pauseMenuCanvas;
    public Button playButton;
    public Button mainMenuButton;

    void Start()
    {
        if (FindObjectOfType<EventSystem>() == null)
        {
            Debug.LogError("No EventSystem found in the scene. Please add one.");
        }
        CanvasGroup canvasGroup = pauseMenuCanvas.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.interactable = true;
        }

        if (playButton != null)
        {
            playButton.onClick.AddListener(ResumeGame);
            playButton.interactable = true;
        } else {
          Debug.LogError("playButton is not assigned in the Inspector.");
        }

        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(GoToMainMenu);
            mainMenuButton.interactable = true;
        } else {
          Debug.LogError("mainMenuButton is not assigned in the Inspector.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        if (pauseMenuCanvas != null)
        {
            pauseMenuCanvas.SetActive(isPaused);
        }
        if (OnPause != null)
        {
            OnPause.Invoke(isPaused);
        }
    }

    void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        if (pauseMenuCanvas != null)
        {
            pauseMenuCanvas.SetActive(false);
        }
        if (OnPause != null)
        {
            OnPause.Invoke(isPaused);
        }
    }

    void GoToMainMenu()
    {
        ResetPauseState();
        SessionManager.Instance.ResetCrossedCheckpoints();
        SceneManager.LoadScene("StartScene");
    }

    void ResetPauseState()
    {
        isPaused = false;
        Time.timeScale = 1;
        if (pauseMenuCanvas != null)
        {
            pauseMenuCanvas.SetActive(false);
        }
        if (OnPause != null)
        {
            OnPause.Invoke(isPaused);
        }
        SessionManager.Instance.ResetCheckpointTimer();
    }
}
