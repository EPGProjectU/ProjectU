using System.Collections;
using System.Collections.Generic;
using ProjectU.Core;
using UnityEngine;

[CreateNodeMenu("BehaviourTree/Leaf/CheckEnemyInAttackRange")]
public class CheckIfInAttackRange : LeafNode {

    [Input]
    public AIObject attackTarget;

    public override NodeState Evaluate(AIController controller) {

         if (checkAttackRange(controller)) {
             state = NodeState.SUCCESS;
             return state;
         }
         else {
             state = NodeState.FAILURE;
             return state;
         }
      
    }

    private bool checkAttackRange(AIController controller) {
        
        attackTarget = GetInputValue<AIObject>(nameof(attackTarget));

        if (Vector3.Distance(controller.transform.position, attackTarget.transform.position) > controller.getAttackRange())
            return false;
        else
            return true;

   
    }

}
