using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateNodeMenu("BehaviourTree/Leaf/TaskGoToTarget")]
public class TaskGoToTarget : LeafNode {

    [Input]
    public AIObject aiObject;


    public override NodeState Evaluate(AIController controller) {
        Transform target = GetInputValue<AIObject>(nameof(aiObject)).transform;

        
        if (target != null && Vector3.Distance(controller.transform.position, target.position) > 0.01f) 
            controller.agent.SetDestination(target.position);

        state = NodeState.RUNNING;
        return state;
    }
}