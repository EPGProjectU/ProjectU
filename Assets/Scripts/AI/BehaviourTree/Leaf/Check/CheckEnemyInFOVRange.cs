using System;
using System.Collections;
using System.Collections.Generic;
using ProjectU.Core;
using UnityEngine;

[CreateNodeMenu("BehaviourTree/Leaf/CheckEnemyInSight")]
public class CheckEnemyInFOVRange : LeafNode {


    public override NodeState Evaluate(AIController controller) {         

        if (detectEnemy(controller)) {
            state = NodeState.SUCCESS;
            return state;
        }
        else {
            state = NodeState.FAILURE;
            return state;
        }
    }

    private bool detectEnemy(AIController controller) {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(controller.transform.position, controller.getSightRange());

         if (colliders.Length > 0) {

             foreach (Collider2D collider in colliders) {

                 if (collider._CompareTag("Player")) {
                     state = NodeState.SUCCESS;
                     return true;
                 }
             }
         }

         return false;
    }


}
