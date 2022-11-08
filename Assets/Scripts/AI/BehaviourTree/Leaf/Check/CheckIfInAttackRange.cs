using System.Collections;
using System.Collections.Generic;
using ProjectU.Core;
using UnityEngine;

[CreateNodeMenu("BehaviourTree/Leaf/CheckEnemyInAttackRange")]
public class CheckIfInAttackRange : LeafNode {
    public override NodeState Evaluate(AIController controller) {

        if (controller.target == null) {
            if (checkAttackRange(controller)) {
                state = NodeState.SUCCESS;
                return state;
            }
            else {
                state = NodeState.FAILURE;
                return state;
            }
        }

        if (Vector2.Distance(controller.target.position, controller.transform.position) > controller.getAttackRange())
            controller.target = null;

        state = NodeState.SUCCESS;
        return state;

    }

    private bool checkAttackRange(AIController controller) {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(controller.transform.position, controller.getAttackRange());

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
