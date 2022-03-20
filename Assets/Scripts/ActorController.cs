using UnityEngine;
using UnityEngine.Serialization;


/// <summary>
/// Abstract class responsible for controlling actions of an actor
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public abstract class ActorController : MonoBehaviour
{
    private Animator _actorAnimator;

    public bool running;
    
    /// <summary>
    /// Vector representing actor's movement
    /// </summary>
    public Vector2 MovementVector { get; protected set; }
    /// <summary>
    /// Vector representing actor's rotation
    /// </summary>
    public Vector2 LookVector { get; protected set; }
    
    /// <summary>
    /// Used to determinate if look vector should be equal MovementVector
    /// </summary>
    protected bool useMovementVectorForLook = true;

    public float rotationSpeed = 360f;

    // Animator properties
    private static readonly int SpeedAnimatorProperty = Animator.StringToHash("Speed");
    private static readonly int AttackAnimatorProperty = Animator.StringToHash("Attack");

    /// <summary>
    /// Retrieves and sets rotation on game object with animator
    /// </summary>
    public float CharacterRotation
    {
        get => _actorAnimator.transform.localEulerAngles.y;
        protected set => _actorAnimator.transform.localRotation = Quaternion.Euler(0, value, 0);
    }

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
    /// Setup 
    /// </summary>
    public void Setup()
    {
        // Caching phase
        _rigidBody = GetComponent<Rigidbody2D>();
        _actorAnimator = GetComponentInChildren<Animator>();

        // If animator was not found a dummy animator is set to avoid exceptions
        if (!_actorAnimator)
            _actorAnimator = gameObject.AddComponent<Animator>();
    }
    
    private void FixedUpdate()
    {
        var rotationDelta = Mathf.DeltaAngle(CharacterRotation, Vector2.SignedAngle(LookVector, Vector2.up));

        var deltaRotationSpeed = rotationSpeed * Time.fixedDeltaTime;
        
        CharacterRotation += Mathf.Clamp(rotationDelta, -deltaRotationSpeed, deltaRotationSpeed);
        
        _actorAnimator.SetFloat(SpeedAnimatorProperty, _rigidBody.velocity.magnitude);

        // Rigid body velocity does not use delta time
        _rigidBody.velocity = MovementVector * (running ? RunningSpeed : BaseSpeed);
    }

    protected void Attack()
    {
        _actorAnimator.SetBool(AttackAnimatorProperty, true);
    }
}