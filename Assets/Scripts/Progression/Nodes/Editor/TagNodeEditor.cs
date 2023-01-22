using UnityEngine;
using XNodeEditor;

/// <summary>
/// Draws <see cref="TagNode"/> in <see cref="NodeGraphEditor"/>
/// </summary>
[CustomNodeEditor(typeof(TagNode))]
public class TagNodeEditor : NodeEditor
{
    private TagNode _node;
    private TagNode Node => _node ??= target as TagNode;

    public override void OnBodyGUI()
    {
        serializedObject.Update();

        ProgressionManager.StartEditorChange();
        var oldState = Node.State;

        GUILayout.BeginHorizontal();
        NodeEditorGUILayout.PortField(new GUIContent("In"), target.GetInputPort("input"), GUILayout.MinWidth(0));
        NodeEditorGUILayout.PortField(new GUIContent("Out"), target.GetOutputPort("output"), GUILayout.MinWidth(0));
        GUILayout.EndHorizontal();

        // Draw input field for tag name
        Node.Name = GUILayout.TextField(Node.Name);

        // Draw checkboxes for active and collected
        GUILayout.BeginHorizontal();
        Node.flags.active = GUILayout.Toggle(Node.flags.active, "Active");
        Node.flags.collected = GUILayout.Toggle(Node.flags.collected, "Collected");
        GUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();

        ProgressionManager.EndEditorChange(oldState != Node.State);
    }

    public override int GetWidth() => 160;

    public override Color GetTint()
    {
        return Node.State switch
        {
            TagNode.TagState.Available => new Color(0.27f, 0.39f, 0.28f),
            TagNode.TagState.Collected => new Color(0.13f, 0.2f, 0.14f),
            TagNode.TagState.Active => new Color(0.13f, 0.22f, 0.33f),
            _ => base.GetTint()
        };
    }
}