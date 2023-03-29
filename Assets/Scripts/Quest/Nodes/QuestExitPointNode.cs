using UnityEngine;
using XNode;

[CreateNodeMenu("Quest/Flow/Exit Point")]
public class QuestExitPointNode : Node, NotifyNodeInterface
{
    [Input]
    public NotifyNodeInterface.EmptyPort input;
    
    [Output]
    public NotifyNodeInterface.EmptyPort output;

    protected override void Init() => name = "Exit Point";

    public override object GetValue(NodePort port) => null;
    

    public bool Notify(object payload)
    {
        Debug.Log($"Quest {graph.name} completed.");
        
        NotifyNodeHelper.SendNotify(GetOutputPort(nameof(output)), true);

        return true;
    }
}