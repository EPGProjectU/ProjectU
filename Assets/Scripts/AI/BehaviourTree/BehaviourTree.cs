using System;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(menuName = "ProjectU/AI/BehaviourTree", fileName = "BehaviourTree")]
public class BehaviourTree : Graph
{
    private BehaviourNode _root;

    public void SetupTree()
    {
        var entryNodes = nodes.OfType<EntryNode>().ToArray();

        switch (entryNodes.Length)
        {
            case 0:
                Debug.LogError($"{GetType()} {name} is missing entry node!", this);
                break;
            case 1:
                _root = entryNodes[0];
                break;
            default:
                Debug.LogError($"{GetType()} {name} has more than one entry node!", this);
                break;
        }
    }

    public void Evaluate(AIController controller)
    {
        if (_root != null)
            _root.Evaluate(controller);
    }

    //periodically tick every node activating evaluation and helping to enable time dependant behaviours and be more intuitive
    //composite nodes decide which node to tick
    //decorators alter tick
    //action (task,check) nodes alter actor or return condition result
    private void Tick()
    {
        throw new NotImplementedException();
    }
}