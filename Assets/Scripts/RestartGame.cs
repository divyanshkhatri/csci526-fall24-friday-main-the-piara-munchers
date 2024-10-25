using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public string sceneName;

    public void Restart()
    {
        SceneManager.LoadScene(sceneName);
    }
}
