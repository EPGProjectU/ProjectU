using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(ActorController))]
public class AIController : MonoBehaviour
{
    [HideInInspector]
    public NavMeshAgent agent;
    [HideInInspector]
    public int nextWayPoint;
    
    public BehaviourTree behaviourTree;

    public Transform target;
    public List<Transform> wayPointList;

    private ActorController actor;

    private void Awake()
    {
        actor = GetComponent<ActorController>();
    }

    private void Start()
    {
        SetupAgent();
        behaviourTree.SetupTree();
    }

    private void Update()
    {
        behaviourTree.Evaluate(this);
        UpdateAgent();
    }

    private void UpdateAgent()
    {
        actor.MovementVector = agent.velocity / actor.CurrentMaxSpeed;
        actor.LookVector = actor.MovementVector;
        agent.nextPosition = actor.transform.position;
    }

    private void SetupAgent()
    {
        agent = gameObject.AddComponent<NavMeshAgent>();
        agent.speed = actor.motionData.baseSpeed;
        agent.updateRotation = false; //rotation to face towards target will be handled by animation system
        agent.updateUpAxis = false;
        agent.updatePosition = false;
    }

    public void Attack() => actor.Attack();

    public bool IsInConversation() => actor.IsInConversation();
    public void StopConversation() => actor.StopConversation();
    public void StartConversation() => actor.StartConversation();



}