using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        // Updating player speed base on the input
        UpdateVelocity(GetVelocityFromInput());
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
