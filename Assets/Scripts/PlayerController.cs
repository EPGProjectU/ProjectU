using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// Quick hack to implement input toggle
// TODO write input manager and move this code there
public enum ToggleState
{
    Default,
    ToggledOn,
    Active,
    ToggledOff
}

public static class ToggleStateExtensions
{
    public static bool ToBoolean(this ToggleState value)
    {
        return value switch
        {
            ToggleState.ToggledOn => true,
            ToggleState.Active => true,
            _ => false
        };
    }
}

/// <summary>
/// ActorController dedicated to the player with implemented input handling
/// </summary>
public class PlayerController : ActorController
{
    private static readonly int Attack = Animator.StringToHash("Attack");

    public ToggleState sprinting;

    private bool isDead;
    public bool IsDead { get => isDead; set => isDead = value; }

    // Start is called before the first frame update
    void Start()
    {
        // Calling setup of ActorController
        Setup();
        IsDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        //This will work only, if game is not frozen
        if (Time.timeScale != 0f)
        {
            // Updating player speed base on the input
            if (!IsDead)
            {
                var velocity = GetVelocityFromInput();

                if (velocity.magnitude > 0)
                    UpdateModelRotation(Vector2.SignedAngle(velocity, Vector2.down));

                ActorAnimator.SetBool(Attack, Input.GetAxisRaw("Fire1") > 0f);

                UpdateVelocity(velocity);
            }
            //Loads Pause menu (later there'll be a need to lock all input when game is paused
            if (Input.GetButtonDown("Cancel"))
            {
                if (!SceneManager.GetSceneByBuildIndex(5).isLoaded)
                {



                    this.enabled = false;
                    AudioListener.pause = true;
                    SceneManager.LoadSceneAsync(5, LoadSceneMode.Additive);
                    Debug.Log("Scena za³adowana");
                    Time.timeScale = 0f;
                    Debug.Log("timescale = 0");
                }
            }
        }
    }

    void UpdateModelRotation(float facingAngle)
    {
        const float snapAngle = 360f / 32f;
        var round = Mathf.Round(facingAngle / snapAngle);

        // Set rotation of character model base on current input
        ActorAnimator.transform.localRotation = Quaternion.Euler(0, round * snapAngle, 0);
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

        // Hack to get input toggle functionality
        // TODO remove when input manager is done
        switch (sprinting)
        {
            case ToggleState.Default:
                if (Input.GetAxisRaw("ToggleSprint") > 0)
                    sprinting = ToggleState.ToggledOn;

                break;
            case ToggleState.ToggledOn:
                if (Input.GetAxisRaw("ToggleSprint") == 0)
                    sprinting = ToggleState.Active;

                break;
            case ToggleState.Active:
                if (Input.GetAxisRaw("ToggleSprint") > 0)
                    sprinting = ToggleState.ToggledOff;

                break;
            case ToggleState.ToggledOff:
                if (Input.GetAxisRaw("ToggleSprint") == 0)
                    sprinting = ToggleState.Default;

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        // Disable sprint on released input
        if (inputVector.magnitude < 0.5)
            sprinting = ToggleState.Default;

        // Multiplying input vector by the selected movement speed
        return inputVector * (sprinting.ToBoolean() ? RunningSpeed : BaseSpeed);
    }

}
