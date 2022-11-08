using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateNodeMenu("BehaviourTree/Leaf/TaskAttackObject")]
public class TaskAttackObject : LeafNode {

    [Input]
    public AIObject aiObject;

    public override NodeState Evaluate(AIController controller) {
        AttackObject(controller);
        state = NodeState.RUNNING;
        return state;
    }

    private void AttackObject(AIController controller) {
        aiObject = GetInputValue<AIObject>(nameof(aiObject));
        controller.Attack(aiObject);
    }
}
