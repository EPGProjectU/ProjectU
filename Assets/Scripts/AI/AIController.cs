using System;
using System.Collections.Generic;
using UnityEngine;


public class AIController : ActorController {

    [HideInInspector] public UnityEngine.AI.NavMeshAgent agent;
    [HideInInspector] public int nextWayPoint;
    public BehaviourTree behaviourTree;

    public Transform target;
    public List<Transform> wayPointList;

    private void Start() {
        base.Setup();
        SetupAgent();
        behaviourTree.SetupTree();
    }

    private void Update() {
        behaviourTree.Evaluate(this);
        UpdateAgent();
    }

    private void UpdateAgent() {

        MovementVector = agent.velocity / CurrentMaxSpeed;
        LookVector = MovementVector;
        agent.nextPosition = transform.position;
    }

    private void SetupAgent() {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.speed = motionData.baseSpeed;
        agent.updateRotation = false; //rotation to face towards target will be handled by animation system
        agent.updateUpAxis = false;
        agent.updatePosition = false;
    }

    //should be removed when enemy will have weapon
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.GetComponent<HealthSystem>()) {
            if (!collision.gameObject.GetComponent<HealthSystem>().allies.Contains(Ally.Enemy)) collision.gameObject.GetComponent<PlayerHealthSystem>().TakeDamage(new DamageInfo(1));
        }
    }

    public new void Attack() => base.Attack();
}