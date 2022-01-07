using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class responsible for controlling actions of an actor
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public abstract class ActorController : MonoBehaviour
{
    /// <summary>
    /// Base movement speed in units per second
    /// </summary>
    public float BaseSpeed = 1f;

    /// <summary>
    /// Alternative movement speed in units per second
    /// </summary>
    public float RunningSpeed = 20f;

    /// <summary>
    /// Cached reference to rigidBody to which all movement is applied
    /// </summary>
    private Rigidbody2D _rigidBody;

    /// <summary>
    /// Amount of health that actor currently haves
    /// </summary>
    public int health;

    /// <summary>
    /// Setup 
    /// </summary>
    public void Setup()
    {
        // Caching phase
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Updates velocity of the rigidBody allowing actor to move
    /// </summary>
    /// <param name="velocity">velocity with already applied direction and speed</param>
    protected void UpdateVelocity(Vector2 velocity)
    {
        _rigidBody.velocity = velocity;
    }
}
