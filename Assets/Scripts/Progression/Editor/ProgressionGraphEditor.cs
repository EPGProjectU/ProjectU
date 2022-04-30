using System;
using System.Linq;
using XNodeEditor;

[CustomNodeGraphEditor(typeof(ProgressionGraph))]
public class ProgressionGraphEditor : NodeGraphEditor
{
    private static readonly Type[] AllowedNodes =
    {
        typeof(BoolNode),
        typeof(BranchNode),
        typeof(LockNode),
        typeof(TagNode),
        typeof(TagHookNode)
    };

    public override string GetNodeMenuName(Type type)
    {
        return AllowedNodes.Any(nodeType => nodeType.IsAssignableFrom(type)) ? base.GetNodeMenuName(type) : null;
    }
}