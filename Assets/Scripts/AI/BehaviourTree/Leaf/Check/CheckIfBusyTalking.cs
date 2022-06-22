using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateNodeMenu("BehaviourTree/Leaf/CheckIfBusyTalking")]
public class CheckIfBusyTalking : LeafNode {
    public override NodeState Evaluate(AIController controller) {

        if (controller.IsInConversation()) {
            state = NodeState.SUCCESS;
            return state;
        }
        else {
            state = NodeState.FAILURE;
            return state;
        }
    }
}
