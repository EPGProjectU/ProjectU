using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskGoToTarget : BehaviourNode {

    public override NodeState Evaluate(EnemyController controller) {
        Transform target = controller.target;

        
        if (target != null && Vector3.Distance(controller.transform.position, target.position) > 0.01f) 
            controller.agent.SetDestination(target.position);

        state = NodeState.RUNNING;
        return state;
    }
}