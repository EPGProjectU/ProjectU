using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Start is called before the first frame update
    public void ChangeScene(int sceneName)
    {
        SceneManager.LoadScene(sceneName);
        if(sceneName == (int)SceneEnum.SampleScene)
        {
            Time.timeScale = 1;
        }

    }
    public void EndGame()
    {
        Application.Quit();
    }
}
