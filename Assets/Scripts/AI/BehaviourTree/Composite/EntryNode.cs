using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("BehaviourTree/Entry", 1)]
public class EntryNode : CompositeNode {

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


