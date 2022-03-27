using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerFreezeCheck : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Checks, if PauseMenu is open, and unlocks Player if it's not
        if (!SceneManager.GetSceneByBuildIndex(5).isLoaded && this.GetComponent<PlayerController>().enabled == false)
        {
            this.GetComponent<PlayerController>().enabled = true;
            Time.timeScale = 1;
            AudioListener.pause = false;
        }
    }
}
