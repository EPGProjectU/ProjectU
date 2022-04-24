using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskAttack : BehaviourNode {
    public override NodeState Evaluate(EnemyController controller) {
        Attack(controller);
        state = NodeState.RUNNING;
        return state;
    }

    private void Attack(EnemyController controller) {
        //if enemy target health is above 0 then attack
        controller.AttackEnemy();
        Debug.Log("Attacking");
    }
}
