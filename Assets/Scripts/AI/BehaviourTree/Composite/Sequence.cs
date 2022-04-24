using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

    public class Sequence : CompositeNode {
        public Sequence() : base() { }

      
        //public Sequence(List<Node> children) : base(children) { }

        public override NodeState Evaluate(AIController controller) {
            bool anyChildIsRunning = false;

            foreach (NodePort nodePort in Outputs) {

                BehaviourNode node = null;

                if (nodePort.IsConnected) {
                    node = nodePort.Connection.node as BehaviourNode;

                    switch (node.Evaluate(controller)) {
                        case NodeState.FAILURE:
                            state = NodeState.FAILURE;
                            return state;
                        case NodeState.SUCCESS:
                            continue;
                        case NodeState.RUNNING:
                            anyChildIsRunning = true;
                            continue;
                        default:
                            state = NodeState.SUCCESS;
                            return state;
                    }
                }
            }

            state = anyChildIsRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            return state;
        }

    }


