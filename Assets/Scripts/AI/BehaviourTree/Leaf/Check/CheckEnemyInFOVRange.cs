using System;
using System.Collections;
using System.Collections.Generic;
using ProjectU.Core;
using UnityEngine;

[CreateNodeMenu("BehaviourTree/Leaf/CheckEnemyInSight")]
public class CheckEnemyInFOVRange : LeafNode {


    public override NodeState Evaluate(AIController controller) {
        if (controller.currentTarget == null) {
            if (detectEnemy(controller)) {
                state = NodeState.SUCCESS;
                return state;
            }
            else {
                state = NodeState.FAILURE;
                return state;
            }
        }

        if (Vector2.Distance(controller.currentTarget.position, controller.transform.position) > controller.getSightRange())
            controller.currentTarget = null;

        state = NodeState.SUCCESS;
        return state;
    }

    private bool detectEnemy(AIController controller) {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(controller.transform.position, controller.getSightRange());

         if (colliders.Length > 0) {

             foreach (Collider2D collider in colliders) {
                //string colliderTag = collider.tag; //dbg

                foreach (string tag in controller.factionData.enemyTags) {
                    if (collider._CompareTag(tag)) {
                        controller.currentTarget = collider.gameObject.transform;
                        state = NodeState.SUCCESS;
                        return true;
                    }
                }
             }
         }

         return false;
    }


}
