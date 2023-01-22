#if UNITY_EDITOR
using System.IO;
using ProjectU.Core;
using UnityEditor;
using UnityEngine;

// Editor only functionality of the ProgressionManager
public static partial class ProgressionManager
{
    [AfterHotReload]
    private static void AfterHotReload() => Init();

    [OnExitingPlayMode]
    private static void OnExitPlayMode()
    {
        _initialized = false;

        Reset();

        // Make progression graph dirty for unity to save it
        EditorUtility.SetDirty(DataCache.Get().graph);
    }

    /// <summary>
    /// Resets static fields
    /// </summary>
    [InitializeOnEnterPlayMode]
    private static void Reset()
    {
        UpdateCallList.Clear();
        ProgressionStateSnapshot.Clear();

        ClearCache();
    }

    static ProgressionManager()
    {
        EditorApplication.delayCall += CreateDataFile;
    }


    /// <summary>
    /// Creates <see cref="ProgressionManagerData"/> file if it does not exist
    /// </summary>
    private static void CreateDataFile()
    {
        if (DataCache.Get())
            return;

        Debug.Log("ProgressionManagerData does not exist. Creating a new instance.");

        var fullDataPath = $"Assets/Resources/{ResourceDataPath}.asset";

        Directory.CreateDirectory(Path.GetDirectoryName(fullDataPath)!);
        var data = ScriptableObject.CreateInstance<ProgressionManagerData>();
        AssetDatabase.CreateAsset(data, fullDataPath);

        DataCache.Clear();
        DataCache.Get();
    }

    /// <summary>
    /// Editor version of <see cref="StartChange"/>
    /// </summary>
    public static void StartEditorChange() => CreateSnapshot();

    /// <summary>
    /// Editor version of <see cref="EndChange"/>
    /// </summary>
    /// <param name="fireEvents">If events for change should be triggered</param>
    public static void EndEditorChange(bool fireEvents = true)
    {
        if (_initialized && fireEvents)
            SendTagUpdateEvents();
    }

    /// <summary>
    /// Reloads <see cref="ProgressionManagerData"/>
    /// </summary>
    public static void Reload()
    {
        ClearCache();
    }
}
#endif