using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateNodeMenu("BehaviourTree/SubtreeNode")]
public class SubtreeNode : BehaviourNode { 
    public BehaviourTree subTree;
    
    public void init() {
        subTree.SetupTree();
    }

    public override NodeState Evaluate(AIController controller) {
        return subTree.Evaluate(controller);
    }

    public override bool IsSubtreeNode() {
        return true;
    }


}
