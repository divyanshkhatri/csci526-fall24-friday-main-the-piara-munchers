using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenManager : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("LevelT1"); 
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
