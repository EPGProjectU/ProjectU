using UnityEditor;
using UnityEngine;
using XNodeEditor;

[CustomNodeEditor(typeof(LockNode))]
public class LockNodeEditor : NodeEditor
{
    private LockNode _node;
    private LockNode Node => _node ??= target as LockNode;

    public override void OnBodyGUI()
    {
        serializedObject.Update();

        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();

        NodeEditorGUILayout.PortField(new GUIContent("Lock  "), target.GetInputPort("inLock"), GUILayout.MinWidth(0));
        NodeEditorGUILayout.PortField(new GUIContent("Unlock"), target.GetInputPort("inUnlock"), GUILayout.MinWidth(0));

        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        
        EditorGUILayout.Popup(0, new[] { "Any", "All" }, GUILayout.Width(45));
        EditorGUILayout.Popup(1, new[] { "Any", "All" }, GUILayout.Width(45));

        GUILayout.EndVertical();
        GUILayout.BeginVertical();

        NodeEditorGUILayout.PortField(new GUIContent("Out"), target.GetOutputPort("output"), GUILayout.MinWidth(0));

        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }

    public override int GetWidth() => 170;

    public override Color GetTint() => Node.CheckUnlocks() ? new Color(0.13f, 0.2f, 0.14f) : !Node.CheckLocks() ? new Color(0.27f, 0.39f, 0.28f) : new Color(0.42f, 0.44f, 0.08f);
}