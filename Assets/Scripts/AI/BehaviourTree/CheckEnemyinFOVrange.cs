using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CheckEnemyInFOVRange : BehaviourNode {
   
    public CheckEnemyInFOVRange() {}

    public override NodeState Evaluate(EnemyController controller) {
      
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

        if (Vector2.Distance(controller.target.position, controller.transform.position) > controller.sightRadius)
            controller.target = null;

        state = NodeState.SUCCESS;
        return state;

    }

    private bool detectEnemy(EnemyController controller) {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(controller.transform.position, controller.sightRadius);
        
        if (colliders.Length > 0) {

            foreach (Collider2D collider in colliders) {

                if (collider.gameObject.tag.Equals("Player")) {

                    controller.target = collider.gameObject.transform;
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
