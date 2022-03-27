using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyController : ActorController
{    
    public Transform currentTarget;  //change to private and calculate based on AI module
    private UnityEngine.AI.NavMeshAgent agent;

    void Start()
    {
        base.Setup();
        SetupAgent();
    }

    void Update() {
        UpdateAgent();
    }

    private void SetupAgent() {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.speed = BaseSpeed;
        agent.updateRotation = false; //rotation to face towards target will be handled by animation system
        agent.updateUpAxis = false;
    }

    private void UpdateAgent() {
        // Update agent destination if the target moves one unit
        if (Vector3.Distance(agent.destination, currentTarget.position) > 1.0f) {
            agent.destination = currentTarget.position;
        }
    }

    //should be removed when enemy will have weapon
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<HealthSystem>())
        {
            if (!collision.gameObject.GetComponent<HealthSystem>().allies.Contains(Ally.Enemy)) collision.gameObject.GetComponent<PlayerHealthSystem>().TakeDamage(new DamageInfo(1));
        }
    }
}
