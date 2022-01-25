using UnityEngine;
using XNodeEditor;

[CustomNodeEditor(typeof(BranchNode))]
public class BranchNodeEditor : NodeEditor
{
    private BranchNode _node;
    private BranchNode Node => _node ??= target as BranchNode;

    public override void OnBodyGUI()
    {
        serializedObject.Update();

        GUILayout.BeginHorizontal();
        NodeEditorGUILayout.PortField(new GUIContent("In"), target.GetInputPort("input"), GUILayout.MinWidth(0));
        NodeEditorGUILayout.PortField(new GUIContent("Out"), target.GetOutputPort("output"), GUILayout.MinWidth(0));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Limit");

        if (int.TryParse(GUILayout.TextField(Node.activeBranchLimit.ToString()), out var newBranchLimit))
            Node.activeBranchLimit = newBranchLimit;

        GUILayout.EndHorizontal();


        serializedObject.ApplyModifiedProperties();
    }

    public override int GetWidth() => 100;

    public override Color GetTint() => Node.IsLocked() ? new Color(0.42f, 0.44f, 0.08f) : Node.IsAvailable() ? new Color(0.27f, 0.39f, 0.28f) : base.GetTint();
}