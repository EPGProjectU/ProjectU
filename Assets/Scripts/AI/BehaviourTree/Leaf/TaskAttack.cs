using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskAttack : LeafNode {
    public override NodeState Evaluate(AIController controller) {
        Attack(controller);
        state = NodeState.RUNNING;
        return state;
    }

    private void Attack(AIController controller) {
        //if enemy target health is above 0 then attack
        controller.AttackEnemy();
        Debug.Log("Attacking");
    }
}
