using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenManager : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("LevelTutorial"); 
    }

    public void OpenLevels()
    {
        SceneManager.LoadScene("LevelsScene"); 
    }

    public void ShowInstructions()
    {
        SceneManager.LoadScene("InstructionsScene"); 
    }
}
