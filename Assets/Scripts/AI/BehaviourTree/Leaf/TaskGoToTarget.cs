using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskGoToTarget : LeafNode {

    public override NodeState Evaluate(AIController controller) {
        Transform target = controller.target;

        
        if (target != null && Vector3.Distance(controller.transform.position, target.position) > 0.01f) 
            controller.agent.SetDestination(target.position);

        state = NodeState.RUNNING;
        return state;
    }
}