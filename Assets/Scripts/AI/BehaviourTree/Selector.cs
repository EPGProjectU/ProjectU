using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;


namespace BehaviourTree {
    public class Selector : BehaviourNode {
        public Selector() : base() { }

 
        /*  public Selector(List<Node> children) : base(children) { }

 public override NodeState Evaluate(EnemyController controller) 
     foreach (Node node in children) {
         switch (node.Evaluate(controller)) {
             case NodeState.FAILURE:
                 continue;
             case NodeState.SUCCESS:
                 state = NodeState.SUCCESS;
                 return state;
             case NodeState.RUNNING:
                 state = NodeState.RUNNING;
                 return state;
             default:
                 continue;
         }
     }

     state = NodeState.FAILURE;
     return state;
 }*/

        public override NodeState Evaluate(EnemyController controller) {

            foreach (NodePort nodePort in Outputs) {

                BehaviourNode node = null;

                if (nodePort.IsConnected) {

                    node = nodePort.Connection.node as BehaviourNode;

                    switch (node.Evaluate(controller)) {
                        case NodeState.FAILURE:
                            continue;
                        case NodeState.SUCCESS:
                            state = NodeState.SUCCESS;
                            return state;
                        case NodeState.RUNNING:
                            state = NodeState.RUNNING;
                            return state;
                        default:
                            continue;
                    }
                }

            }
            state = NodeState.FAILURE;
            return state;
        }
    }
}