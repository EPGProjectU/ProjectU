using System.Collections;
using System.Collections.Generic;
using ProjectU.Core;
using UnityEngine;



[CreateNodeMenu("BehaviourTree/Leaf/CheckObjectInAttackRange")]
public class CheckIfInAttackRange : LeafNode {

    //If target isn't provided then it is taken from ai controller current target evaluated from faction data and any ai target setting operations
    [Input]
    public AIObject attackTarget;

    public override NodeState Evaluate(AIController controller) {
         
         if (controller.currentTarget!=null && checkAttackRange(controller)) {
             state = NodeState.SUCCESS;
             return state;
         }
         else {
             state = NodeState.FAILURE;
             return state;
         }
    }

    private bool checkAttackRange(AIController controller) {
        Transform targetTrnsfrm;

        if (attackTarget != null)
            targetTrnsfrm = GetInputValue<AIObject>(nameof(attackTarget)).transform;
        else
            targetTrnsfrm = controller.currentTarget.transform;
            
        if (Vector3.Distance(controller.transform.position, targetTrnsfrm.position) > controller.getAttackRange())
            return false;
        else
            return true;
    }

}
