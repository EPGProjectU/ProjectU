using System.Collections.Generic;
using System.Linq;


/// <summary>
/// Intermediate class for accessing progression functionality
/// </summary>
public static partial class ProgressionManager
{
    /// <param name="tagName">Name identifying <see cref="TagNode"/></param>
    /// <returns><see cref="TagNode"/> with given name in the current <see cref="ProgressionGraph"/></returns>
    public static TagNode GetTag(string tagName)
    {
        return TagNameCache.Get().TryGetValue(tagName, out var tag) ? tag : null;
    }

    /// <returns>List of <see cref="TagNode"/>s for the current <see cref="ProgressionGraph"/></returns>
    public static List<TagNode> GetAllTags()
    {
        return (from e in TagNameCache.Get() select e.Value).ToList();
    }

    /// <summary>
    /// Sets <see cref="TagNode"/> to Collected <see cref="TagNode.TagState"/>
    /// </summary>
    /// <param name="tagName">Name identifying <see cref="TagNode"/></param>
    /// <param name="force">If condition for collecting tag should be ignored</param>
    /// <returns>If collection was successful
    /// <br/>In case of forced collection, if force was needed</returns>
    public static bool CollectTag(string tagName, bool force = false)
    {
        return CollectTag(TagNameCache.Get()[tagName], force);
    }

    /// <summary>
    /// Sets <see cref="TagNode"/> to Collected <see cref="TagNode.TagState"/>
    /// </summary>
    /// <param name="tagNode"><see cref="TagNode"/> to be collected</param>
    /// <param name="force">If condition for collecting tag should be ignored</param>
    /// <returns>If collection was successful
    /// <br/>In case of forced collection, if force was needed</returns>
    public static bool CollectTag(TagNode tagNode, bool force = false)
    {
        if (!tagNode || !tagNode.IsAvailable() && !force)
            return false;

        CreateSnapshot();

        tagNode.flags.collected = true;

        SendTagUpdateEvents();

        // Return if tag is available as a feedback to force flag
        return tagNode.IsAvailable();
    }

    /// <summary>
    /// Sets or reset <see cref="TagNode"/> Active <see cref="TagNode.TagState"/>
    /// </summary>
    /// <param name="tagName">Name identifying <see cref="TagNode"/></param>
    /// <param name="state">True to activate, false to deactivate</param>
    /// <returns>If active state was successful changed</returns>
    public static bool SetActiveTag(string tagName, bool state)
    {
        return SetActiveTag(TagNameCache.Get()[tagName], state);
    }

    /// <summary>
    /// Sets or reset <see cref="TagNode"/> Active <see cref="TagNode.TagState"/>
    /// </summary>
    /// <param name="tagNode"><see cref="TagNode"/> to be activate/deactivate</param>
    /// <param name="state">True to activate, false to deactivate</param>
    /// <returns>If active state was successful changed</returns>
    public static bool SetActiveTag(TagNode tagNode, bool state)
    {
        if (!tagNode.IsAvailable())
            return false;

        CreateSnapshot();

        tagNode.flags.active = state;

        SendTagUpdateEvents();

        return true;
    }
}