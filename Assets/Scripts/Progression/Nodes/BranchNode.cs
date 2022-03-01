using System.Linq;
using XNode;

[CreateNodeMenu("Progression/Branch", 1)]
public class BranchNode : Node
{
    [Input]
    public bool input;

    [Output]
    public bool output;

    public int activeBranchLimit = 1;

    // Return the correct value of an output port when requested
    public override object GetValue(NodePort port)
    {
        if (port.fieldName == "output")
            return IsAvailable() && !IsLocked();

        return null;
    }

    public bool IsAvailable()
    {
        var values = GetInputPort("input").GetInputValues<bool>();

        return values.Length == 0 || MathHelper.Or(values);
    }

    public bool IsLocked()
    {
        var branchLimit = activeBranchLimit;

        var outputPort = GetOutputPort("output");

        foreach (var tag in outputPort.GetConnections().Select(conn => conn.node).OfType<TagNode>().Where(tag => tag.IsActive()))
        {
            --branchLimit;
        }

        return branchLimit <= 0;
    }

    public override void OnCreateConnection(NodePort from, NodePort to)
    {
        // TODO replace TagNode with general interface
        if (ReferenceEquals(from.node, this) && to.node.GetType() != typeof(TagNode))
            from.Disconnect(to);

        base.OnCreateConnection(from, to);
    }
}