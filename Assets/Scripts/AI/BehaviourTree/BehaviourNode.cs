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

[CreateNodeMenu("")]
public abstract class BehaviourNode : Node{
     protected NodeState state;

     [Input]
     public BehaviourNode parent;

     /* [Output(dynamicPortList = true)]
      public BehaviourNode child;*/


    //public virtual NodeState Evaluate() => NodeState.FAILURE;
    public abstract NodeState Evaluate(AIController controller);

    public virtual bool IsStartingNode() => false;

       /* public override object GetValue(NodePort port) {
            return child;
        }*/
    }

    




