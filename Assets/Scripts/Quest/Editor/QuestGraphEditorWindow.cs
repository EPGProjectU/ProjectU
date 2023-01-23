using UnityEditor;
using UnityEditor.Callbacks;


/// <summary>
/// <see cref="QuestGraph"/> editor window
/// </summary>
public class QuestGraphEditorWindow : GraphEditorWindow
{
    [OnOpenAsset(-1)]
    public static bool HandleGraphOpen(int instanceID, int line)
    {
        var graph = EditorUtility.InstanceIDToObject(instanceID) as QuestGraph;

        return Open<QuestGraphEditorWindow>(graph) != null;
    }
}