#if UNITY_EDITOR
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using XNode;

public partial class ProgressionManager
{
    [DidReloadScripts]
    private static void OnScriptReload() => Init();

    [InitializeOnEnterPlayMode]
    private static void OnEnterPlayMode() => Reset();

    static ProgressionManager()
    {
        EditorApplication.delayCall += CreateDataFile;
    }

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

    public static void StartEditorChange()
    {
        if (!_initialized)
            return;

        StartChange();
    }

    public static void EndEditorChange(bool changeHappened = true, bool fireEvents = true)
    {
        if (!_initialized || !changeHappened)
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

    public static void HardRefresh()
    {
        LoadData();

        SoftRefresh();
    }

    public static void SoftRefresh()
    {
        InitTagReferences();

        if (_initialized)
            InitTagEventBuilders();
    }

    //TODO Access ProgressionManager in testing code through reflection
#if UNITY_INCLUDE_TESTS
    public static void InitForTest()
    {
        Init();
    }

    public static void ResetForTest()
    {
        Reset();
    }

    public static int NumberOfRegisteredHooks()
    {
        return HookRegistry.Count;
    }
#endif
}
#endif