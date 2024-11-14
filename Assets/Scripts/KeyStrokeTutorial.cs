using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class KeyStrokeTutorial : MonoBehaviour
{
    public TextMeshProUGUI tutorialText;
    public Image background;
    private bool firstLeftKeyPress = true;
    public TutorialTeleport tutorialTeleport;
    private float rightKeyPressTime = 0f;
    private int previousLevel = -1;

    void Start()
    {
        if (tutorialText != null)
        {
            tutorialText.gameObject.SetActive(false);
        }
        if (background != null)
        {
            background.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (tutorialTeleport != null)
        {
            if (tutorialTeleport.currentLevel != previousLevel)
            {
                previousLevel = tutorialTeleport.currentLevel;
                if (tutorialText != null)
                {
                    tutorialText.gameObject.SetActive(false);
                }
                if (background != null)
                {
                    background.gameObject.SetActive(false);
                }
            }

            if (tutorialTeleport.currentLevel == 0)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (firstLeftKeyPress)
                    {
                        if (tutorialText != null)
                        {
                            tutorialText.text = "Seems like the world flipped, press left key again to flip";
                            tutorialText.gameObject.SetActive(true);
                            StartCoroutine(HideTextAfterDelay(3));
                        }
                        if (background != null)
                        {
                            background.gameObject.SetActive(true);
                        }
                        firstLeftKeyPress = false;
                    }
                    else
                    {
                        if (tutorialText != null)
                        {
                            tutorialText.text = "Great! You've flipped the world back to normal";
                            tutorialText.gameObject.SetActive(true);
                            StartCoroutine(HideTextAfterDelay(3));
                        }
                        if (background != null)
                        {
                            background.gameObject.SetActive(true);
                        }
                        firstLeftKeyPress = true;
                    }
                }

                if (Input.GetKey(KeyCode.RightArrow) && tutorialTeleport.currentLevel == 0)
                {
                    rightKeyPressTime += Time.deltaTime;
                    if (rightKeyPressTime > 0.5f)
                    {
                        if (tutorialText != null)
                        {
                            tutorialText.text = "Touch the flag to complete the level";
                            tutorialText.gameObject.SetActive(true);
                        }
                        if (background != null)
                        {
                            background.gameObject.SetActive(true);
                        }
                    }
                }
                else
                {
                    rightKeyPressTime = 0f;
                    if (tutorialText != null && tutorialText.text == "Touch the flag to complete the level")
                    {
                        tutorialText.gameObject.SetActive(false);
                    }
                    if (background != null && tutorialText.text == "Touch the flag to complete the level")
                    {
                        background.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    private IEnumerator HideTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (tutorialText != null)
        {
            tutorialText.gameObject.SetActive(false);
        }
        if (background != null)
        {
            background.gameObject.SetActive(false);
        }
    }
}
