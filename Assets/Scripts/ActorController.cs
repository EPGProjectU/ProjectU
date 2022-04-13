using UnityEngine;


/// <summary>
/// Abstract class responsible for controlling actions of an actor
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public abstract partial class ActorController : MonoBehaviour
{
    private Animator _actorAnimator;

    protected bool running;

    public ActorMotionData motionData;

    protected float CurrentMaxSpeed => running ? motionData.runningSpeed : motionData.baseSpeed;

    /// <summary>
    /// Vector representing actor's movement
    /// </summary>
    public Vector2 MovementVector { get; protected set; }

    /// <summary>
    /// Vector representing actor's rotation
    /// </summary>
    public Vector2 LookVector { get; protected set; }

    /// <summary>
    /// Retrieves and sets rotation on game object with animator
    /// </summary>
    public float CharacterRotation
    {
        get => _actorAnimator.transform.localEulerAngles.y;
        protected set => _actorAnimator.transform.localRotation = Quaternion.Euler(0, value, 0);
    }

    private void OnValidate()
    {
        _actorAnimator = GetComponentInChildren<Animator>();

        // If animator was not found a dummy animator is set to avoid exceptions
        if (!_actorAnimator)
            _actorAnimator = gameObject.AddComponent<Animator>();
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

        OnValidate();
    }

    private void FixedUpdate()
    {
        if (LookVector.magnitude > 0.1)
        {
            var rotationDelta = Mathf.DeltaAngle(CharacterRotation, Vector2.SignedAngle(LookVector, Vector2.up));

            var deltaRotationSpeed = motionData.rotationSpeed * Time.fixedDeltaTime;

            CharacterRotation += Mathf.Clamp(rotationDelta, -deltaRotationSpeed, deltaRotationSpeed) * Mathf.Sqrt(LookVector.magnitude);

        }

        var playerMoveAngle = Vector2.SignedAngle(MovementVector, Vector2.down);

        _actorAnimator.SetFloat(SpeedAnimatorProperty, _rigidBody.velocity.magnitude);

        // Rigid body velocity does not use delta time
        _rigidBody.velocity = MovementVector * CurrentMaxSpeed * motionData.EvaluateMotionForAngle(Mathf.DeltaAngle(playerMoveAngle, CharacterRotation));
    }

    protected void Attack()
    {
        _actorAnimator.SetBool(AttackAnimatorProperty, true);
    }
}