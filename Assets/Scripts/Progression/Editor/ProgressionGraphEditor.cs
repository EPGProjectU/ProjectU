using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using XNode;
using XNodeEditor;

public class ProgressionGraphEditor : NodeEditorWindow
{
    [MenuItem("ProjectU/Progression/Show Graph")]
    public static void ShowCurrentContextProgressionGraph()
    {
        Open(ProgressionManager.Data.graph);
    }

    [OnOpenAsset(-1)]
    public static bool HandleGraphOpen(int instanceID, int line)
    {
        var graph = EditorUtility.InstanceIDToObject(instanceID) as ProgressionGraph;

        if (!graph)
            return false;

        Open(graph);

        return true;
    }

    public new static ProgressionGraphEditor Open(NodeGraph graph)
    {
        var window = NodeEditorWindow.Open(graph);
        var icon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Gizmos/ProgressionGraph Icon.png");
        window.titleContent = new GUIContent("Progression Graph", icon);

        return window as ProgressionGraphEditor;
    }
}