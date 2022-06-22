using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;


[CreateNodeMenu("BehaviourTree/Leaf/CheckTest")]
public class CheckTest : LeafNode {

   
    private int range = 2;

    public override NodeState Evaluate(AIController controller) {


        Collider2D[] colliders = Physics2D.OverlapCircleAll(controller.transform.position, range);

        if (colliders.Length > 0) {

            foreach (Collider2D collider in colliders) {

                if (collider.gameObject.tag.Equals("Player")) {

                    controller.target = collider.gameObject.transform;
                    return NodeState.SUCCESS;
                }
            }
        }

        return NodeState.FAILURE;

    }

}
