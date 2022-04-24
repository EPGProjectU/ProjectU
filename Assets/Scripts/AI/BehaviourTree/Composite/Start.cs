using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("BehaviourTree/Start", 1)]
public class Start : CompositeNode {

    public override NodeState Evaluate(AIController controller) {

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


