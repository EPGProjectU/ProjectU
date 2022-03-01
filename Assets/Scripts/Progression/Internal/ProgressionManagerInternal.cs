using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using ProjectU.Core;

public partial class ProgressionManager
{
    public const string DataPath = "Progression/ProgressionManagerData";

    public static ProgressionManagerData Data { get; private set; }

    private static readonly List<TagHook> HookRegistry = new List<TagHook>();

    private static readonly Dictionary<string, ProgressionTag> Tags = new Dictionary<string, ProgressionTag>();

    private static readonly Dictionary<ProgressionTag, List<TagHook>> HookCallList = new Dictionary<ProgressionTag, List<TagHook>>();

    private static readonly List<TagEventBuilder> TagEventBuilders = new List<TagEventBuilder>();

    private static bool _initialized;

    private static void LoadData()
    {
        Data = Resources.Load<ProgressionManagerData>(DataPath);
    }

    [Awake]
    [OnSceneLoaded]
    private static void Init()
    {
        LoadData();

        if (Data.graph == null)
        {
            Debug.LogWarning("Progression graph is not set in ProjectU/Progression/Settings!");

            return;
        }

        ResetTagStates();

        InitTagReferences();

        InitTagEventBuilders();

        LinkAllHooks();

        _initialized = true;
    }

    /// <summary>
    /// Resets static fields
    /// </summary>
    [OnExitingPlayMode]
    private static void Reset()
    {
        HookRegistry.Clear();

        HookCallList.Clear();
        TagEventBuilders.Clear();

        Tags.Clear();

        _initialized = false;
    }

    internal static void RegisterTagHook(TagHook hook)
    {
        //Assert.IsFalse(HookRegistry.Contains(hook), $"Hook {hook.tagName} is already in the registry!");

        if (hook.TagName == "" || HookRegistry.Contains(hook))
            return;

        HookRegistry.Add(hook);
    }

    internal static void UnRegisterTagHook(TagHook hook)
    {
        // There is no need to unregister hooks when ProgressionManger is inactive
        if (!_initialized)
            return;

        Assert.IsTrue(HookRegistry.Contains(hook), $"UnRegisterTagHook was called while hook {hook.tagName} is not in the registry!");

        UnLinkHook(hook);
        HookRegistry.Remove(hook);
    }


    /// <summary>
    /// Breaks old <see cref="TagHook"/> to <see cref="ProgressionTag"/> link and creates a new one based on current <see cref="TagHook"/> state
    /// </summary>
    /// <param name="hook"></param>
    internal static void ReLinkTagHook(TagHook hook)
    {
        if (!_initialized)
            return;

        Assert.IsTrue(HookRegistry.Contains(hook), $"Hook {hook.tagName} is not in the registry!");

        UnLinkHook(hook);
        LinkHook(hook);
    }

    private class TagEventBuilder
    {
        public ProgressionTag.TagState oldState;
        public ProgressionTag.TagState newState;
        public ProgressionTag progressionTag;

        public TagHook.TagEvent GetEvent(TagHook hook)
        {
            return new TagHook.TagEvent
            {
                hook = hook,
                progressionTag = progressionTag,
                oldState = oldState,
                newState = newState
            };
        }
    }

    private static void ResetTagStates()
    {
        foreach (var node in Data.graph.nodes.OfType<TagNode>())
        {
            node.collected = false;
            node.active = false;
        }
    }

    private static void UnLinkHook(TagHook hook)
    {
        if (hook.Tag == null)
            return;

        Assert.IsTrue(HookCallList.ContainsKey(hook.Tag));

        // Removing hook under the tag reference from the call list
        HookCallList[hook.Tag].Remove(hook);
    }

    private static void LinkAllHooks()
    {
        HookCallList.Clear();

        // Populate _hookCallList with empty list for each progression tag
        foreach (var pair in Tags)
            HookCallList[pair.Value] = new List<TagHook>();

        foreach (var hook in HookRegistry)
            LinkHook(hook);
    }

    private static void LinkHook(TagHook hook)
    {
        if (hook.TagName == "")
            return;

        if (!Tags.ContainsKey(hook.tagName))
        {
            Debug.LogWarning($"Tag {hook.tagName} could not be found in the current context!");

            return;
        }

        hook.Tag = Tags[hook.TagName];
        HookCallList[hook.Tag].Add(hook);
    }

    private static void StartChange()
    {
        UpdateTagEventBuilders();
    }

    private static void EndChange(bool fireEvents = true)
    {
        UpdateTagEventBuilders();

        if (fireEvents)
            SendTagUpdateEvents();
    }

    /// <summary>
    /// Sends update event to all <see cref="TagHook"/>s  linked to <see cref="ProgressionTag"/>s that had changed their state
    /// </summary>
    private static void SendTagUpdateEvents()
    {
        foreach (var tagEventBuilder in TagEventBuilders)
        {
            // Ignore unchanged tags
            if (tagEventBuilder.newState == tagEventBuilder.oldState)
                continue;

            // Using calssic for to allow hook re-registration during the event handling, which would invalidate any iterator
            for (var i = 0; i < HookCallList[tagEventBuilder.progressionTag].Count; i++)
            {
                var hook = HookCallList[tagEventBuilder.progressionTag][i];
                hook.FireOnUpdate(tagEventBuilder.GetEvent(hook));
            }
        }
    }

    /// <summary>
    /// Stores references to all <see cref="ProgressionTag"/>s in the <see cref="ProgressionGraph"/> in dictionary
    /// </summary>
    private static void InitTagReferences()
    {
        Tags.Clear();

        if (Data == null || Data.graph == null)
            return;

        var nodeList = Data.graph.nodes.OfType<ProgressionTag>().ToList();

        nodeList.Sort((left, right) => string.CompareOrdinal(left.Name, right.Name));

        foreach (var progressionTag in nodeList.Where(progressionTag => !string.IsNullOrEmpty(progressionTag.Name)))
            Tags.Add(progressionTag.Name, progressionTag);
    }

    private static void InitTagEventBuilders()
    {
        TagEventBuilders.Clear();

        foreach (var progressionTag in Tags)
            TagEventBuilders.Add(new TagEventBuilder { newState = progressionTag.Value.State, progressionTag = progressionTag.Value });
    }

    private static void UpdateTagEventBuilders()
    {
        foreach (var tagEventBuilder in TagEventBuilders)
        {
            tagEventBuilder.oldState = tagEventBuilder.newState;
            tagEventBuilder.newState = tagEventBuilder.progressionTag.State;
        }
    }
}