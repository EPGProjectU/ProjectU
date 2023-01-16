using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateNodeMenu("BehaviourTree/Leaf/CheckIfObjectInRange")]

public class CheckIfObjectInRange : LeafNode
{

    [Input]
    public float range;

    [Input]
    public AIObject targetObject;

    public override NodeState Evaluate(AIController controller) {

        if (isInRange(controller)) {
            state = NodeState.SUCCESS;
            return state;
        }
        else {
            state = NodeState.FAILURE;
            return state;
        }
    }

    private bool isInRange(AIController controller) {
        range = GetInputValue<float>(nameof(range));
        targetObject = GetInputValue<AIObject>(nameof(targetObject));


        if (Vector3.Distance(controller.transform.position, targetObject.transform.position) > range)
            return false;
        else
            return true;

    }

}
