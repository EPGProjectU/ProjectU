using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;




    [CreateAssetMenu(menuName = "ProjectU/AI/BehaviourTree", fileName = "BehaviourTree")]
    public class BehaviourTree : NodeGraph {

        private BehaviourNode _root;

        public void SetupTree() {

            foreach(BehaviourNode node in nodes) {
                if (node.IsStartingNode())
                    _root = node;
            }

            if (_root == null)
                throw new Exception("Behaviour Tree must contain Start node");

            //tmp 
            Debug.Log("Behaviour tree nodes count: " + nodes.Count);
        }

        public void Evaluate(EnemyController controller) {
            if (_root != null)
                _root.Evaluate(controller);
        }
    }

