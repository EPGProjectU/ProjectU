using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateNodeMenu("BehaviourTree/Leaf/TaskAttack")]
public class TaskAttack : LeafNode {
    public override NodeState Evaluate(AIController controller) {
        Attack(controller);
        state = NodeState.RUNNING;
        return state;
    }

    private void Attack(AIController controller) {
        //if enemy target health is above 0 then attack
        controller.Attack();
        Debug.Log(controller.agent.name + " is Attacking!");
    }
}