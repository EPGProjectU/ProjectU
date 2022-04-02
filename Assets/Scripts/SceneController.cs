using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Start is called before the first frame update
    public void ChangeScene(int sceneName)
    {
        if (sceneName == 1)
        {
            SceneManager.LoadScene(1);

        }
        else if (sceneName == 0)
        {
            if (SceneManager.GetSceneByBuildIndex(5).isLoaded)
                SceneManager.UnloadSceneAsync(5);
            if (!SceneManager.GetSceneByBuildIndex(0).isLoaded)
                SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }

        if (SceneManager.GetSceneByBuildIndex(5).isLoaded)
        {
            Time.timeScale = 0f;
            Debug.Log("timescale = 0");
            AudioListener.pause = true;
        }
        else
        {
            Time.timeScale = 1;
            Debug.Log("timescale = 1");
            AudioListener.pause = false;
        }
    }
    public void EndGame()
    {
        Application.Quit();
    }
}
