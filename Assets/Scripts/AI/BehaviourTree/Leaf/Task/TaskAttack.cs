using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskAttack : LeafNode {
    public override NodeState Evaluate(AIController controller) {
        throw new NotImplementedException();
        /*Attack(controller);
        state = NodeState.RUNNING;
        return state;*/
    }

    private void Attack(AIController controller) {
        throw new NotImplementedException();
        //controller.Attack();

    }
}
