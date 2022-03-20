using UnityEngine;


/// <summary>
/// Abstract class responsible for controlling actions of an actor
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public abstract class ActorController : MonoBehaviour
{
    private Animator _actorAnimator;

    public bool running;

    public ActorMotionData motionData;

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

    /// <summary>
    /// Retrieves and sets rotation on game object with animator
    /// </summary>
    public float CharacterRotation
    {
        get => _actorAnimator.transform.localEulerAngles.y;
        protected set => _actorAnimator.transform.localRotation = Quaternion.Euler(0, value, 0);
    }

    /// <summary>
    /// Cached reference to rigidBody to which all movement is applied
    /// </summary>
    private Rigidbody2D _rigidBody;
    
    // Animator properties
    private static readonly int SpeedAnimatorProperty = Animator.StringToHash("Speed");
    private static readonly int AttackAnimatorProperty = Animator.StringToHash("Attack");

    
    /// <summary>
    /// Caches references
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

        var deltaRotationSpeed = motionData.rotationSpeed * Time.fixedDeltaTime;

        //CharacterRotation += 

        _actorAnimator.SetFloat(SpeedAnimatorProperty, _rigidBody.velocity.magnitude);

        CharacterRotation += Mathf.Clamp(rotationDelta, -deltaRotationSpeed, deltaRotationSpeed);
        // Rigid body velocity does not use delta time
        _rigidBody.velocity = MovementVector * (running ? motionData.runningSpeed : motionData.baseSpeed);
    }

    protected void Attack()
    {
        _actorAnimator.SetBool(AttackAnimatorProperty, true);
    }
}