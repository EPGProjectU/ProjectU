using UnityEngine;


/// <summary>
/// Abstract class responsible for controlling actions of an actor
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public abstract class ActorController : MonoBehaviour
{
    public Animator ActorAnimator;

    public float speed;

    private Vector2 oldPosition;

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

    private static readonly int SpeedProperty = Animator.StringToHash("Speed");

    /// <summary>
    /// Setup 
    /// </summary>
    public void Setup()
    {
        // Caching phase
        _rigidBody = GetComponent<Rigidbody2D>();
        oldPosition = transform.position;
    }

    /// <summary>
    /// Updates velocity of the rigidBody allowing actor to move
    /// </summary>
    /// <param name="velocity">velocity with already applied direction and speed</param>
    protected void UpdateVelocity(Vector2 velocity)
    {
        _rigidBody.velocity = velocity;
    }

    private void FixedUpdate()
    {
        var position = (Vector2)transform.position;
        var realVelocity = (oldPosition - position) / Time.fixedDeltaTime;

        speed = realVelocity.magnitude;
        ActorAnimator.SetFloat(SpeedProperty, speed);
        oldPosition = position;
    }
}