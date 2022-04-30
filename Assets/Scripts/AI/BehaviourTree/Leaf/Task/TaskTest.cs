using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateNodeMenu("BehaviourTree/Leaf/TaskTest")]
public class TaskTest : LeafNode {
    public override NodeState Evaluate(AIController controller) {
        Debug.Log("Task failed succesfully");
        return NodeState.SUCCESS;
    }
}
