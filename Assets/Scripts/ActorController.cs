using UnityEngine;
using DialogueEditor;


/// <summary>
/// Abstract class responsible for controlling actions of an actor
/// </summary>
public partial class ActorController : MonoBehaviour
{
    private Animator _actorAnimator;

    [HideInInspector]
    public bool running;

    public ActorMotionData motionData;

    public float CurrentMaxSpeed => running ? motionData.runningSpeed : motionData.baseSpeed;

    private bool _isTalking;

    /// <summary>
    /// Vector representing actor's movement
    /// </summary>
    public Vector2 MovementVector { get; set; }

    /// <summary>
    /// Vector representing actor's rotation
    /// </summary>
    public Vector2 LookVector { get; set; }

    /// <summary>
    /// Cached reference to rigidBody to which all movement is applied
    /// </summary>
    private Rigidbody2D _rigidBody;

    private bool _dead;

    public bool Dead
    {
        get { return _dead; }
        set
        {
            _actorAnimator.SetBool(DeadAnimatorProperty, value);
            _dead = value;
        }
    }


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

    // Animator properties
    private static readonly int SpeedAnimatorProperty = Animator.StringToHash("Speed");
    private static readonly int AttackAnimatorProperty = Animator.StringToHash("Attack");
    private static readonly int DeadAnimatorProperty = Animator.StringToHash("Dead");

    private void Awake() => Setup();

    /// <summary>
    /// Caches references
    /// </summary>
    public void Setup()
    {
        // Caching phase
        _rigidBody = gameObject.AddComponent<Rigidbody2D>();
        _rigidBody.bodyType = RigidbodyType2D.Dynamic;
        _rigidBody.gravityScale = 0;
        _rigidBody.freezeRotation = true;

        OnValidate();

        _isTalking = false;
    }

    private void FixedUpdate()
    {
        if (Dead)
        {
            LookVector = Vector2.zero;
            MovementVector = Vector2.zero;
        }

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

    public void Attack()
    {
        if (_dead)
            return;
        _actorAnimator.SetBool(AttackAnimatorProperty, true);
    }

    public void StartConversation()
    {
        _isTalking = true;
        Debug.Log("TEST");
        ConversationManager.Instance.StartConversation(this.gameObject.GetComponent<NPCConversation>());
        //do DialogueManager things
        //DialogueManager.StartConversation()
    }

    public void StopConversation()
    {
        _isTalking = false;
        //DialogueManager.StopConversation();
    }

    public bool IsInConversation()
    {
        if (_isTalking)
            return true;
        else
            return false;
    }
}