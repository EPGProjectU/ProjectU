#if UNITY_EDITOR
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using XNode;

// Editor only functionality of the ProgressionManager
public partial class ProgressionManager
{
    [DidReloadScripts]
    private static void OnScriptReload()
    {
        // For now OnScriptReload mimics Init function with difference of not resetting tags
        // TODO make it call Init() again when serialization of tag states is done
        LoadData();

        if (Data == null || Data.graph == null)
        {
            Debug.LogWarning("Progression graph is not set in ProjectU/Progression/Settings!");

            return;
        }

        InitTagReferences();

        InitTagEventBuilders();

        LinkAllHooks();

        _initialized = true;
    }

    [InitializeOnEnterPlayMode]
    private static void OnEnterPlayMode() => Reset();

    static ProgressionManager() => EditorApplication.delayCall += CreateDataFile;

    /// <summary>
    /// Creates <see cref="ProgressionManagerData"/> file if it does not exist
    /// </summary>
    private static void CreateDataFile()
    {
        var fullDataPath = $"Assets/Resources/{DataPath}.asset";

        if (AssetDatabase.LoadAssetAtPath<ProgressionManagerData>(fullDataPath))
            return;

        Debug.Log("ProgressionManagerData does not exist. Creating a new instance.");
        Directory.CreateDirectory(Path.GetDirectoryName(fullDataPath)!);
        var data = ScriptableObject.CreateInstance<ProgressionManagerData>();
        AssetDatabase.CreateAsset(data, fullDataPath);

        LoadData();
    }

    /// <summary>
    /// Editor version of <see cref="StartChange"/>
    /// </summary>
    public static void StartEditorChange()
    {
        if (!_initialized)
            return;

        StartChange();
    }

    /// <summary>
    /// Editor version of <see cref="EndChange"/>
    /// </summary>
    /// <param name="fireEvents">If events for change should be triggered</param>
    public static void EndEditorChange(bool fireEvents = true)
    {
        if (!_initialized)
            return;

        EndChange(fireEvents);
    }

    /// <summary>
    /// Updates registered <see cref="TagHook"/>s that were using old tag name with a new one
    /// </summary>
    /// <param name="oldName">Name that will be used to find <see cref="TagHook"/>s to update</param>
    /// <param name="newName">Name that found <see cref="TagHook"/>s will be updated to</param>
    /// <param name="nodeGraph">Used for checking if name was changed in the used graph</param>
    public static void SendTagNameChangeNotifications(string oldName, string newName, NodeGraph nodeGraph)
    {
        if (Data == null || nodeGraph != Data.graph)
            return;

        foreach (var hook in HookRegistry.Where(hook => hook.TagName == oldName))
            hook.tagName = newName;
    }

    /// <summary>
    /// Reloads <see cref="ProgressionManagerData"/> and does reinitialize references
    /// </summary>
    /// <seealso cref="SoftRefresh"/>
    public static void HardRefresh()
    {
        LoadData();

        SoftRefresh();
    }

    /// <summary>
    /// Only reinitialize references
    /// </summary>
    /// <seealso cref="HardRefresh"/>
    public static void SoftRefresh()
    {
        InitTagReferences();

        if (_initialized)
            InitTagEventBuilders();
    }
}
#endif