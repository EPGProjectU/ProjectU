using System.Collections.Generic;
using System.Linq;


public partial class ProgressionManager
{
    public static ProgressionTag GetTag(string tagName)
    {
        return !Tags.ContainsKey(tagName) ? null : Tags[tagName];
    }

    public static List<ProgressionTag> GetAllTags()
    {
        return (from e in Tags select e.Value).ToList();
    }

    public static List<ProgressionTag> GetInactiveTags()
    {
        return (from e in Tags where !e.Value.IsActive() && !e.Value.IsCollected() select e.Value).ToList();
    }

    public static List<ProgressionTag> GetActiveTags()
    {
        return (from e in Tags where e.Value.IsActive() && !e.Value.IsCollected() select e.Value).ToList();
    }

    public static List<ProgressionTag> GetCollectedTags()
    {
        return (from e in Tags where e.Value.IsCollected() select e.Value).ToList();
    }

    public static bool CollectTag(string tagName, bool force = false)
    {
        return CollectTag(Tags[tagName], force);
    }

    public static bool CollectTag(ProgressionTag progressionTag, bool force = false)
    {
        if (!(progressionTag is TagNode tagNode))
            return false;

        if (!progressionTag.IsAvailable() && !force)
            return false;

        StartChange();

        tagNode.collected = true;

        EndChange();

        // Return if tag is available as a feedback to force flag
        return tagNode.IsAvailable();
    }

    public static bool SetActiveTag(ProgressionTag progressionTag, bool state)
    {
        if (!(progressionTag is TagNode tagNode))
            return false;

        if (!progressionTag.IsAvailable())
            return false;
        
        StartChange();

        tagNode.active = true;

        EndChange();

        return true;
    }
}