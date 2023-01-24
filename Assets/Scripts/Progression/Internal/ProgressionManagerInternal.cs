using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using ProjectU.Core;

// Internal functionality of the ProgressionManager
public static partial class ProgressionManager
{
    /// <summary>
    /// Location of the <see cref="ProgressionManagerData"/> file inside of Resource folder
    /// </summary>
    public const string ResourceDataPath = "Progression/ProgressionManagerData";

    private static readonly DataCache<ProgressionManagerData> DataCache = new DataCache<ProgressionManagerData>(() => Resources.Load<ProgressionManagerData>(ResourceDataPath));

    public static ProgressionManagerData Data => DataCache.Get();


    private static readonly DataCache<HashSet<TagNode>> TagCache = new DataCache<HashSet<TagNode>>(() =>
    {
        return new HashSet<TagNode>(
            DataCache.Get().graph.nodes
                .OfType<TagNode>()
        );
    });

    /// <summary>
    /// Dictionary of cached <see cref="TagNode"/>s for faster access time
    /// </summary>
    private static readonly DataCache<Dictionary<string, TagNode>> TagNameCache = new DataCache<Dictionary<string, TagNode>>(() =>
    {
        return
            TagCache.Get()
                .Where(tagNode => !string.IsNullOrEmpty(tagNode.Name))
                .ToDictionary(tagNode => tagNode.Name);
    });

    /// <summary>
    /// Dictionary that stores <see cref="TagHook"/>s indexed by linked <see cref="TagNode"/>
    /// </summary>
    private static readonly Dictionary<TagNode, Action<TagEvent>> UpdateCallList = new Dictionary<TagNode, Action<TagEvent>>();

    private static readonly Dictionary<TagNode, TagNode.TagState> ProgressionStateSnapshot = new Dictionary<TagNode, TagNode.TagState>();

    /// <summary>
    /// Indicates if the <see cref="ProgressionManager"/> has been initialized
    /// </summary>
    private static bool _initialized;

    /// <summary>
    /// Initializes <see cref="ProgressionManager"/>
    /// </summary>
    [Awake]
    [OnSceneLoaded]
    [UsedImplicitly]
    private static void Init()
    {
        _initialized = true;
        DataCache.Get();
    }

    /// <summary>
    /// Resets all flags on <see cref="TagNode"/>
    /// </summary>
    [OnExitingPlayMode]
    private static void ResetTagStates()
    {
        foreach (var tagNode in TagCache.Get())
        {
            tagNode.flags.collected = false;
            tagNode.flags.active = false;
        }
    }

    private static void CreateSnapshot()
    {
        ProgressionStateSnapshot.Clear();

        foreach (var tagNode in TagCache.Get())
            ProgressionStateSnapshot.Add(tagNode, tagNode.State);
    }

    internal static void BindUpdate(TagNode target, Action<TagEvent> callback)
    {
        if (UpdateCallList.ContainsKey(target))
        {
            UpdateCallList[target] -= callback;
            UpdateCallList[target] += callback;
        }
        else
            UpdateCallList.Add(target, callback);
    }

    internal static void UnbindUpdate(TagNode target, Action<TagEvent> callback)
    {
        if (!UpdateCallList.ContainsKey(target))
            return;

        UpdateCallList[target] -= callback;
    }

    /// <summary>
    /// Sends update event to all <see cref="TagHook"/>s  linked to <see cref="TagNode"/>s that had changed their state
    /// </summary>
    private static void SendTagUpdateEvents()
    {
        foreach (var kvp in ProgressionStateSnapshot)
        {
            var tagNode = kvp.Key;
            var oldState = kvp.Value;

            if (tagNode.State == oldState)
                continue;

            if (!UpdateCallList.TryGetValue(tagNode, out var updateDelegate))
                continue;

            updateDelegate?.Invoke(new TagEvent
            {
                tagNode = tagNode,
                newState = tagNode.State,
                oldState = oldState
            });
        }
    }
    

    public static void ClearCache()
    {
        TagCache.Clear();
        TagNameCache.Clear();
        ProgressionStateSnapshot.Clear();
    }
}