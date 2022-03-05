using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;
using ProjectU.Core;

// Internal functionality of the ProgressionManager
public partial class ProgressionManager
{
    /// <summary>
    /// Location of the <see cref="ProgressionManagerData"/> file inside of Resource folder
    /// </summary>
    public const string DataPath = "Progression/ProgressionManagerData";

    public static string GraphStateSavePath = "/Progression/progression.data";

    /// <summary>
    /// <see cref="ProgressionManagerData"/>
    /// </summary>
    public static ProgressionManagerData Data { get; private set; }

    /// <summary>
    /// List of live <see cref="TagHook"/>s
    /// </summary>
    private static readonly List<TagHook> HookRegistry = new List<TagHook>();

    /// <summary>
    /// Dictionary of cached <see cref="ProgressionTag"/>s for faster access time
    /// </summary>
    private static readonly Dictionary<string, ProgressionTag> Tags = new Dictionary<string, ProgressionTag>();

    /// <summary>
    /// Dictionary that stores <see cref="TagHook"/>s indexed by linked <see cref="ProgressionTag"/>
    /// </summary>
    /// TODO Might be better to store delegates instead of list, maybe even stored directly by ProgressionTag
    private static readonly Dictionary<ProgressionTag, List<TagHook>> HookCallList = new Dictionary<ProgressionTag, List<TagHook>>();

    /// <summary>
    /// Stores <see cref="TagEventBuilder"/>s used for checking for changes of <see cref="ProgressionTag"/>.<see cref="ProgressionTag.State"/>
    /// </summary>
    private static readonly List<TagEventBuilder> TagEventBuilders = new List<TagEventBuilder>();

    /// <summary>
    /// Indicates if the <see cref="ProgressionManager"/> has been initialized
    /// </summary>
    private static bool _initialized;

    /// <summary>
    /// Loads <see cref="ProgressionManagerData"/> from <see cref="DataPath"/>
    /// </summary>
    private static void LoadData()
    {
        Data = Resources.Load<ProgressionManagerData>(DataPath);
    }

    /// <summary>
    /// Initializes <see cref="ProgressionManager"/>
    /// </summary>
    [Awake]
    [OnSceneLoaded]
    [UsedImplicitly]
    private static void Init()
    {
        LoadData();

        if (Data == null || Data.graph == null)
        {
            Debug.LogWarning("Progression graph is not set in ProjectU/Progression/Settings!");

            return;
        }

        Data.graph.LoadState(Application.persistentDataPath + GraphStateSavePath);

        InitTagReferences();

        InitTagEventBuilders();

        LinkAllHooks();

        _initialized = true;
    }

    /// <summary>
    /// Adds <see cref="TagHook"/> to <see cref="HookRegistry"/>
    /// </summary>
    /// <param name="hook"></param>
    internal static void RegisterTagHook(TagHook hook)
    {
        //Assert.IsFalse(HookRegistry.Contains(hook), $"Hook {hook.tagName} is already in the registry!");

        // Ignore empty and already registered TagHooks
        if (hook.TagName == "" || HookRegistry.Contains(hook))
            return;

        HookRegistry.Add(hook);
    }

    /// <summary>
    /// Removes <see cref="TagHook"/> to <see cref="HookRegistry"/>
    /// </summary>
    /// <param name="hook"></param>
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

    /// <summary>
    /// Stores a snapshot of <see cref="progressionTag"/>
    /// </summary>
    private class TagEventBuilder
    {
        public ProgressionTag.TagState oldState;
        public ProgressionTag.TagState newState;
        public ProgressionTag progressionTag;

        /// <summary>
        /// Creates event for <see cref="TagHook"/>
        /// </summary>
        /// <param name="hook"></param>
        /// <returns><see cref="TagHook.TagEvent"/> ready to be used with <see cref="TagHook.onUpdate"/></returns>
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

    /// <summary>
    /// Resets all flags on <see cref="ProgressionTag"/>
    /// </summary>
    /// TODO Serialization of tag states instead of resetting
    [OnExitingPlayMode]
    private static void ResetTagStates()
    {
        foreach (var node in Data.graph.nodes.OfType<TagNode>())
        {
            node.flags.collected = false;
            node.flags.active = false;
        }
    }

    /// <summary>
    /// Removes <see cref="TagHook"/> from <see cref="HookCallList"/> and removes link to <see cref="ProgressionTag"/>
    /// </summary>
    /// <param name="hook"></param>
    private static void UnLinkHook(TagHook hook)
    {
        if (hook.Tag == null)
            return;

        Assert.IsTrue(HookCallList.ContainsKey(hook.Tag));

        // Removing hook under the tag reference from the call list
        HookCallList[hook.Tag].Remove(hook);
    }

    /// <summary>
    /// Links all <see cref="TagHook"/>s currently stored in <see cref="HookRegistry"/>
    /// </summary>
    private static void LinkAllHooks()
    {
        HookCallList.Clear();

        // Populate _hookCallList with empty list for each progression tag
        foreach (var pair in Tags)
            HookCallList[pair.Value] = new List<TagHook>();

        foreach (var hook in HookRegistry)
            LinkHook(hook);
    }

    /// <summary>
    /// Adds <see cref="TagHook"/> to <see cref="HookCallList"/> and creates link to <see cref="ProgressionTag"/>
    /// </summary>
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

    /// <summary>
    /// Creates a snapshot of current <see cref="ProgressionTag"/> state stored in <see cref="TagEventBuilders"/>
    /// </summary>
    private static void StartChange()
    {
        UpdateTagEventBuilders();
    }

    /// <summary>
    /// Updates snapshot of <see cref="ProgressionTag"/> state stored in <see cref="TagEventBuilders"/> with the new <see cref="ProgressionTag.State"/>
    /// </summary>
    /// <param name="fireEvents">If events should be fired for the <see cref="ProgressionTag"/>s that changed states</param>
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
        foreach (var tagEventBuilder in TagEventBuilders.Where(tagEventBuilder => tagEventBuilder.newState != tagEventBuilder.oldState))
        {
            // Using raw for loop to allow hook re-registration during the event handling, which would invalidate any iterator
            for (var i = 0; i < HookCallList[tagEventBuilder.progressionTag].Count; i++)
            {
                var hook = HookCallList[tagEventBuilder.progressionTag][i];
                hook.FireOnUpdate(tagEventBuilder.GetEvent(hook));
            }
        }
    }

    /// <summary>
    /// Stores references to all <see cref="ProgressionTag"/>s in the <see cref="ProgressionGraph"/> in <see cref="Tags"/> dictionary
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

    /// <summary>
    /// Initializes <see cref="TagEventBuilders"/>
    /// </summary>
    private static void InitTagEventBuilders()
    {
        TagEventBuilders.Clear();

        foreach (var progressionTag in Tags)
            TagEventBuilders.Add(new TagEventBuilder { newState = progressionTag.Value.State, progressionTag = progressionTag.Value });
    }

    /// <summary>
    /// Updates <see cref="ProgressionTag.State"/>s of <see cref="TagEventBuilders"/>
    /// </summary>
    private static void UpdateTagEventBuilders()
    {
        foreach (var tagEventBuilder in TagEventBuilders)
        {
            tagEventBuilder.oldState = tagEventBuilder.newState;
            tagEventBuilder.newState = tagEventBuilder.progressionTag.State;
        }
    }
}