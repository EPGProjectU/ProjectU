using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(ActorController))]
public class AIController : MonoBehaviour
{
    [HideInInspector]
    public NavMeshAgent agent;
    
    public PGraph<BehaviourTree> behaviourTree;

    private ActorController actor;

    public FactionData factionData;

    [HideInInspector]
    public Transform currentTarget;

    private void Awake()
    {
        actor = GetComponent<ActorController>();
    }

    private void Start()
    {
        SetupAgent();
        behaviourTree.Graph.SetupTree();
    }

    private void Update()
    {
        behaviourTree.Graph.Evaluate(this);
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

    /*public void Attack(AIObject target) {
        if (target != null) {
            actor.Attack();
            Debug.DrawLine(transform.position, target.transform.position, Color.yellow, 10.0f, false);
        }
    }*/

    public void Attack() => actor.Attack();

    public bool IsInConversation() => actor.IsInConversation();
    public void StopConversation() => actor.StopConversation();
    public void StartConversation() => actor.StartConversation();

    public float getSightRange() => actor.sightRange;
    public float getAttackRange() => actor.weaponRange;



}