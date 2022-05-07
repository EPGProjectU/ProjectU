using System.Collections;
using System.Collections.Generic;
using ProjectU.Core;
using UnityEngine;

[CreateNodeMenu("BehaviourTree/Leaf/CheckEnemyInFovRange")]
public class CheckEnemyInFOVRange : LeafNode {


    public float range;

    public override NodeState Evaluate(AIController controller) {
      
        if (controller.target == null) {
            if (detectEnemy(controller)) {
                state = NodeState.SUCCESS;
                return state;
            }
            else {
                state = NodeState.FAILURE;
                return state;
            }
        }

        if (Vector2.Distance(controller.target.position, controller.transform.position) > range)
            controller.target = null;

        state = NodeState.SUCCESS;
        return state;

    }

    private bool detectEnemy(AIController controller) {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(controller.transform.position, range);
        
        if (colliders.Length > 0) {

            foreach (Collider2D collider in colliders) {

                if (collider._CompareTag("Player")) {

                    controller.target = collider.gameObject.transform;
                    state = NodeState.SUCCESS;
                    return true;
                }
            }
        }

        return false;
    }


}
