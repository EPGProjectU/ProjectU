using System;
using System.Linq;
using XNodeEditor;

[CustomNodeGraphEditor(typeof(QuestGraph))]
public class QuestGraphEditor : NodeGraphEditor
{
    private static readonly Type[] AllowedNodes =
    {
        typeof(BoolNode),
        typeof(TagHookNode),
        typeof(QuestEntryPoint),
        typeof(QuestExitPointNode),
        typeof(DefeatObjectiveNode)
    };

    public override string GetNodeMenuName(Type type)
    {
        return AllowedNodes.Any(nodeType => nodeType.IsAssignableFrom(type)) ? base.GetNodeMenuName(type) : null;
    }
}