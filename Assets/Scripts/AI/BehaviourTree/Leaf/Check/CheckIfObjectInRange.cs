using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateNodeMenu("BehaviourTree/Leaf/CheckIfObjectInRange")]

public class CheckIfObjectInRange : LeafNode
{
    public float range;
    public Transform targetObject;

    public override NodeState Evaluate(AIController controller) {

        if (controller.target == null) {
            if (isInRange(controller, targetObject)) {
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

    private bool isInRange(AIController controller, Transform target) {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(controller.transform.position, range);

        if (colliders.Length > 0) {

            foreach (Collider2D collider in colliders) {

                if (collider.gameObject.transform.Equals(target)) {

                    state = NodeState.SUCCESS;
                    return true;
                }
            }
        }
        else {
            return false;
        }

        return false;
    }

}
