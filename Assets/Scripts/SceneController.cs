using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Start is called before the first frame update
    public void ChangeScene(string sceneName)
    {
        if (sceneName.Equals("MainMenu"))
        {
            SceneManager.LoadScene("MainMenu");

        }
        else if (sceneName.Equals("SampleScene"))
        {
            if (!SceneManager.GetSceneByName("PauseMenu").isLoaded)
                SceneManager.LoadScene("SampleScene");
            if (SceneManager.GetSceneByName("PauseMenu").isLoaded)
                SceneManager.UnloadSceneAsync("PauseMenu");

        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }

        if (SceneManager.GetSceneByName("PauseMenu").isLoaded)
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
