using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNode;



    public enum NodeState {
        RUNNING,
        SUCCESS,
        FAILURE
    }

    public class BehaviourNode : Node{
        protected NodeState state;

        [Input]
        public BehaviourNode parent;

        /* [Output(dynamicPortList = true)]
         public BehaviourNode child;*/


    //public virtual NodeState Evaluate() => NodeState.FAILURE;
    public virtual NodeState Evaluate(AIController controller) => NodeState.FAILURE;

    public virtual bool IsStartingNode() => false;

       /* public override object GetValue(NodePort port) {
            return child;
        }*/
    }

    




