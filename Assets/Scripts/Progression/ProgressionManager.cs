using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public partial class ProgressionManager : MonoBehaviour
{
    public ProgressionGraph progressionGraph;

    public static ProgressionTag GetTag(string tagName)
    {
        return Instance._tags[tagName];
    }

    public static List<ProgressionTag> GetAllTags()
    {
        return (from e in Instance._tags select e.Value).ToList();
    }

    public static List<ProgressionTag> GetInactiveTags()
    {
        return (from e in Instance._tags where !e.Value.IsActive() && !e.Value.IsCollected() select e.Value).ToList();
    }

    public static List<ProgressionTag> GetActiveTags()
    {
        return (from e in Instance._tags where e.Value.IsActive() && !e.Value.IsCollected() select e.Value).ToList();
    }

    public static List<ProgressionTag> GetCollectedTags()
    {
        return (from e in Instance._tags where e.Value.IsCollected() select e.Value).ToList();
    }

    public static bool CollectTag(string tagName, bool force = false)
    {
        return CollectTag(Instance._tags[tagName], force);
    }

    public static bool CollectTag(ProgressionTag progressionTag, bool force = false)
    {
        if (!(progressionTag is TagNode tagNode))
            return false;

        if (!progressionTag.IsActive() && !force)
            return false;

        Instance.StartChange();

        tagNode.collected = true;

        Instance.EndChange();

        // Return if tag is active as a feedback to force flag
        return tagNode.IsActive();
    }
}