using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelNavigator : MonoBehaviour
{
    public void BackToStartScreen()
    {
        SceneManager.LoadScene("StartScene"); // Replace with your start screen scene name
    }

    public void LoadTutorialLevel()
    {
        SceneManager.LoadScene("LevelTutorial"); // Replace with your tutorial level scene name
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene("Level1"); // Replace with your Level 1 scene name
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("Level2"); // Replace with your Level 2 scene name
    }

    public void LoadLevel3()
    {
        SceneManager.LoadScene("Level3"); // Replace with your Level 3 scene name
    }
}
