using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using XNode;

public class Start : BehaviourNode {

    public override NodeState Evaluate(EnemyController controller) {

        NodePort childPort = GetOutputPort("child");
        if (childPort.IsConnected) {
            BehaviourNode node = childPort.Connection.node as BehaviourNode;
            return node.Evaluate(controller);
        }

        return NodeState.FAILURE;
    }

    public override bool IsStartingNode() {
        return true;
    }
}


