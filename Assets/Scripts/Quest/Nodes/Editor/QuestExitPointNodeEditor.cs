using UnityEditor;
using UnityEngine;
using XNodeEditor;


[CustomNodeEditor(typeof(QuestExitPointNode))]
public class QuestExitPointNodeEditor : NodeEditor
{
    public override void OnBodyGUI()
    {
        serializedObject.Update();

        var input = target.GetInputPort(nameof(QuestExitPointNode.input));
        var output = target.GetOutputPort(nameof(QuestExitPointNode.output));

        EditorGUI.BeginChangeCheck();

        GUILayout.BeginHorizontal();
        NodeEditorGUILayout.PortField(new GUIContent(""), input, GUILayout.MinWidth(0));

        NodeEditorGUILayout.PortField(new GUIContent(""), output, GUILayout.MinWidth(0));


        GUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }

    public override int GetWidth() => 100;
}