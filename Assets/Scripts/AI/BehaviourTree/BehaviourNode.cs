using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNode;

namespace BehaviourTree {

    public enum NodeState {
        RUNNING,
        SUCCESS,
        FAILURE
    }

    public class BehaviourNode : XNode.Node{
        protected NodeState state;

        [Input]
        public BehaviourNode parent;

        /* [Output(dynamicPortList = true)]
         public BehaviourNode child;*/
        [Output]
        public BehaviourNode child;

        [Output]
        public BehaviourNode child1;

        [Output]
        public BehaviourNode child2;

        //protected List<Node> children = new List<Node>();

        /*  public Node() {
              parent = null;
          }
          public Node(List<Node> children) {
              foreach (Node child in children)
                  _Attach(child);
          }

          private void _Attach(Node node) {
              node.parent = this;
              children.Add(node);
          }*/

        //public abstract NodeState Evaluate(EnemyController controller);

        public virtual NodeState Evaluate(EnemyController controller) => NodeState.FAILURE;
        //public virtual NodeState Evaluate() => NodeState.FAILURE;
        public virtual bool IsStartingNode() => false;

        public override object GetValue(NodePort port) {
            return child;
        }
    }

}
