using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;


[CreateAssetMenu(menuName = "ProjectU/AI/BehaviourTree", fileName = "BehaviourTree")]
public class BehaviourTree : Graph {

    private BehaviourNode _root;

    public void SetupTree() {

        foreach (BehaviourNode node in nodes) {
            if (node.IsStartingNode())
                _root = node;
        }

        if (_root == null)
            throw new Exception("Behaviour Tree must contain Start node");

        //tmp 
        Debug.Log("Behaviour tree nodes count: " + nodes.Count);
    }

    public void Evaluate(AIController controller) {
            if (_root != null)
                _root.Evaluate(controller);
    }

    //periodically tick every node activating evaluation and helping to enable time dependant behaviours and be more intuitive
    //composite nodes decide which node to tick
    //decorators alter tick
    //action (task,check) nodes alter actor or return condition result
    private void Tick() { throw new NotImplementedException(); }

}

