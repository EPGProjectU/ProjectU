using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ActorController dedicated to the player with implemented input handling
/// </summary>
public class PlayerController : ActorController
{
    public int health = 3;
    private bool isDead;
    // Start is called before the first frame update
    void Start()
    {
        // Calling setup of ActorController
        Setup();
        isDead = false;
        GameEventSystem.Instance.OnPlayerTakesDamage += takeDamage;
    }

    // Update is called once per frame
    void Update()
    {
        // Updating player speed base on the input
        if(!isDead)UpdateVelocity(GetVelocityFromInput());
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

    public void takeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Health = " + health);
        if (health < 1)
        {
            isDead = true;
            GameEventSystem.Instance.PlayerIsDead();
            Debug.Log("Player is Dead");
        }
        
    }
}
