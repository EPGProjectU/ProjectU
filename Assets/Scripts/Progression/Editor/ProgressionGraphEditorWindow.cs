using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using XNode;

/// <summary>
/// <see cref="ProgressionGraph"/> editor window
/// </summary>
public class ProgressionGraphEditorWindow : GraphEditorWindow
{
    [MenuItem("ProjectU/Progression/Show Graph")]
    public static void ShowCurrentContextProgressionGraph()
    {
        if (ProgressionManager.Data.graph == null)
        {
            var openSettings = EditorUtility.DisplayDialog("Progression Graph is not set!", "Set ProgressionGraph in progression settings", "Open Settings", "Dismiss");

            if (!openSettings)
                return;

            ProgressionSettingsEditor.ShowWindow();
        }

        Open(ProgressionManager.Data.graph);
    }

    [OnOpenAsset(-1)]
    public static bool HandleGraphOpen(int instanceID, int line)
    {
        var graph = EditorUtility.InstanceIDToObject(instanceID) as ProgressionGraph;

        return Open(graph) != null;
    }

    public new static ProgressionGraphEditorWindow Open(NodeGraph graph)
    {
        var window = GraphEditorWindow.Open<ProgressionGraphEditorWindow>(graph);
        
        if (!window)
            return null;

        // Set custom icon for the tab
        var icon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Gizmos/ProgressionGraph Icon.png");
        window.titleContent = new GUIContent("Progression Graph", icon);

        return window;
    }

    protected override void OnGUI()
    {
        ProgressionManager.ClearCache();
        base.OnGUI();
    }
}