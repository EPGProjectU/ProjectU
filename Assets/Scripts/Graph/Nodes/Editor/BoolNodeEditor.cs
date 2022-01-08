using UnityEngine;
using XNodeEditor;

[CustomNodeEditor(typeof(BoolNode))]
public class BoolNodeEditor : NodeEditor
{
    private BoolNode _node;
    public BoolNode Node => _node ??= target as BoolNode;

    public override void OnBodyGUI()
    {
        serializedObject.Update();

        GUILayout.BeginHorizontal();
        NodeEditorGUILayout.PortField(new GUIContent("In"), target.GetInputPort("input"), GUILayout.MinWidth(0));
        NodeEditorGUILayout.PortField(new GUIContent("Out"), target.GetOutputPort("output"), GUILayout.MinWidth(0));
        GUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }

    public override int GetWidth() => 100;

    public override Color GetTint() => Node.GetValue() ? new Color(0.27f, 0.39f, 0.28f) : base.GetTint();
}