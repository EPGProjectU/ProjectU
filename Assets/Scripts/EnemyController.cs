using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyController : ActorController
{    
    public Transform currentTarget;  //change to private and calculate based on AI 
    UnityEngine.AI.NavMeshAgent agent;



    void Start()
    {
        base.Setup();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.speed = BaseSpeed;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.destination = currentTarget.position;
  
    }

    void Update() {
        // Update agent destination if the target moves one unit
        if (Vector3.Distance(agent.destination, currentTarget.position) > 1.0f) {
            agent.destination = currentTarget.position;
        }
    }
}
