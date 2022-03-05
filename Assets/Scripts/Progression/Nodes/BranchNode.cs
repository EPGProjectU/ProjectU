using System.Linq;
using XNode;

/// <summary>
/// Node that allows only limited number of <see cref="TagNode"/>s to be active/collected
/// </summary>
[CreateNodeMenu("Progression/Branch", 1)]
public class BranchNode: Node
{
    [Input]
    public bool input;

    [Output]
    public bool output;

    public int activeBranchLimit = 1;

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

    /// <summary>
    /// Check if number of active/collected <see cref="TagNode"/>s reached limit
    /// </summary>
    /// <returns></returns>
    public bool IsLocked()
    {
        var branchLimit = activeBranchLimit;

        var outputPort = GetOutputPort("output");

        foreach (var _ in outputPort.GetConnections().Select(conn => conn.node).OfType<TagNode>().Where(tag => tag.IsActive()))
        {
            --branchLimit;
        }

        return branchLimit <= 0;
    }

    public override void OnCreateConnection(NodePort from, NodePort to)
    {
        // TODO replace TagNode with general interface that would work with other node types
        // Allow only TagNode to be connected
        if (ReferenceEquals(from.node, this) && to.node.GetType() != typeof(TagNode))
            from.Disconnect(to);

        base.OnCreateConnection(from, to);
    }
}