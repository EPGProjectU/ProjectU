using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("BehaviourTree/Decorator/Invert")]
public class Invert : DecoratorNode
{
    public override NodeState Evaluate(AIController controller) {

        NodePort nodePort = GetOutputPort("child");
        BehaviourNode childNode;

        if (nodePort.IsConnected) {

            childNode = nodePort.Connection.node as BehaviourNode;

            var result = childNode.Evaluate(controller);

            if (result == NodeState.SUCCESS) {
                return NodeState.FAILURE;
            }
            else if (result == NodeState.FAILURE) {
                return NodeState.SUCCESS;
            }
            else
                return result;
        }
        
        throw new System.Exception(this.name + " node not connected!");
    }
}
