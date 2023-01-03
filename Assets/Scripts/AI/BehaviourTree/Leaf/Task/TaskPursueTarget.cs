using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateNodeMenu("BehaviourTree/Leaf/TaskPursueTarget")]
public class TaskPursueTarget : LeafNode {

    public override NodeState Evaluate(AIController controller) {
        Transform target = controller.currentTarget;

        if (target != null && Vector3.Distance(controller.transform.position, target.position) > 0.01f)
            controller.agent.SetDestination(target.position);

        state = NodeState.RUNNING;
        return state;
    }
}