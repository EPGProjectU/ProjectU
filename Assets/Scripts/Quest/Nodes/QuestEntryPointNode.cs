using UnityEngine;
using XNode;

[CreateNodeMenu("Quest/Flow/Entry Point")]
public class QuestEntryPointNode : Node, NotifyNodeInterface
{
    [Input]
    public NotifyNodeInterface.EmptyPort input;

    [Output]
    public NotifyNodeInterface.EmptyPort output;
    
    public override object GetValue(NodePort port) => null;

    protected override void Init() => name = "Entry Point";

    public bool Notify(object payload)
    {
        Debug.Log($"Quest {graph.name} started.");

        NotifyNodeHelper.SendNotify(GetOutputPort(nameof(output)), null);

        return true;
    }
}