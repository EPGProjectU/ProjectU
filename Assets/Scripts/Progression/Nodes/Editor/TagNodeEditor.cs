using UnityEngine;
using XNodeEditor;

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
        

        Node.Name = GUILayout.TextField(Node.Name);

        var oldState = Node.collected;
        Node.collected = GUILayout.Toggle(Node.collected, "Collected");

        var changedHappened = serializedObject.ApplyModifiedProperties();
        ProgressionManager.EndEditorChange(changedHappened, oldState != Node.State);
    }

    public override int GetWidth() => 160;

    public override Color GetTint()
    {
        return Node.State switch
        {
            ProgressionTag.TagState.Active => new Color(0.27f, 0.39f, 0.28f),
            ProgressionTag.TagState.Collected => new Color(0.13f, 0.2f, 0.14f),
            _ => base.GetTint()
        };
    }
}