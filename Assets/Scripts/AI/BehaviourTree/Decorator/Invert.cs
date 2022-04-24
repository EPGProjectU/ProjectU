using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("BehaviourTree/Decorator/Invert")]
public class Invert : DecoratorNode
{
    public override NodeState Evaluate(AIController controller) {

        NodePort nodePort = GetOutputPort("child");
        BehaviourNode node;

        if (nodePort.IsConnected) {

            node = nodePort.Connection.node as BehaviourNode;

            switch (node.Evaluate(controller)) {
                case NodeState.FAILURE:
                    state = NodeState.SUCCESS;
                    return state;
                case NodeState.SUCCESS:
                    state = NodeState.FAILURE;
                    return state;
            }
        }
        
        throw new System.Exception(this.name + " node not connected!");
    }
}
