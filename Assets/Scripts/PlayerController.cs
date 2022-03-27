using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ActorController dedicated to the player with implemented input handling
/// </summary>
public class PlayerController : ActorController
{
    // Start is called before the first frame update
    void Start()
    {
        // Calling setup of ActorController
        Setup();
    }

    // Update is called once per frame
    void Update()
    {

        //This will work only, if game is not frozen
        if (Time.timeScale != 0)
        {
            // Updating player speed base on the input
            UpdateVelocity(GetVelocityFromInput());

            //Loads Pause menu (later there'll be a need to lock all input when game is paused
            if (Input.GetButtonDown("Cancel"))
            {
                if (!SceneManager.GetSceneByBuildIndex(5).isLoaded)
                {
                    Time.timeScale = 0;
                    this.enabled = false;
                    AudioListener.pause = true;
                    SceneManager.LoadScene(5, LoadSceneMode.Additive);
                }
            }
        }


    }

    /// <summary>
    /// Applies speed and (if need) normalizes input to get current velocity of the player
    /// </summary>
    /// <returns>Velocity with applied speed and direction from the input</returns>
    private Vector2 GetVelocityFromInput()
    {
        var inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // Normalizing inputVector to avoid difference between max speeds of cardinal and diagonal directions
        if (inputVector.magnitude > 1.0)
            inputVector.Normalize();

        // Multiplying input vector by the selected movement speed
        return inputVector * (Input.GetKey("left shift") ? RunningSpeed : BaseSpeed);
    }
}
