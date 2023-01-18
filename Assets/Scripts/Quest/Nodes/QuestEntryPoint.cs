using UnityEngine;
using XNode;

public class QuestEntryPoint : Node, NotifyNodeInterface
{
    [Input]
    public NotifyNodeInterface.EmptyPort input;

    [Output]
    public NotifyNodeInterface.EmptyPort output;
    
    public override object GetValue(NodePort port) => null;

    public bool Notify(object payload)
    {
        if (!graph)
            return false;
        Debug.Log($"Quest {graph.name} started.");

        NotifyNodeHelper.SendNotify(GetOutputPort(nameof(output)), null);

        
        return true;
    }
}