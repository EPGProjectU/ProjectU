using System.Collections.Generic;
using System.Linq;


/// <summary>
/// Intermediate class for accessing progression functionality
/// </summary>
public static partial class ProgressionManager
{
    /// <param name="tagName">Name identifying <see cref="ProgressionTag"/></param>
    /// <returns><see cref="ProgressionTag"/> with given name in the current <see cref="ProgressionGraph"/></returns>
    public static ProgressionTag GetTag(string tagName)
    {
        return !Tags.ContainsKey(tagName) ? null : Tags[tagName];
    }

    /// <returns>List of <see cref="ProgressionTag"/>s for the current <see cref="ProgressionGraph"/></returns>
    public static List<ProgressionTag> GetAllTags()
    {
        return (from e in Tags select e.Value).ToList();
    }

    /// <summary>
    /// Sets <see cref="ProgressionTag"/> to Collected <see cref="ProgressionTag.TagState"/>
    /// </summary>
    /// <param name="tagName">Name identifying <see cref="ProgressionTag"/></param>
    /// <param name="force">If condition for collecting tag should be ignored</param>
    /// <returns>If collection was successful
    /// <br/>In case of forced collection, if force was needed</returns>
    public static bool CollectTag(string tagName, bool force = false)
    {
        return CollectTag(Tags[tagName], force);
    }

    /// <summary>
    /// Sets <see cref="ProgressionTag"/> to Collected <see cref="ProgressionTag.TagState"/>
    /// </summary>
    /// <param name="progressionTag"><see cref="ProgressionTag"/> to be collected</param>
    /// <param name="force">If condition for collecting tag should be ignored</param>
    /// <returns>If collection was successful
    /// <br/>In case of forced collection, if force was needed</returns>
    public static bool CollectTag(ProgressionTag progressionTag, bool force = false)
    {
        if (!(progressionTag is TagNode tagNode))
            return false;

        if (!progressionTag.IsAvailable() && !force)
            return false;

        StartChange();

        tagNode.flags.collected = true;

        EndChange();

        // Return if tag is available as a feedback to force flag
        return tagNode.IsAvailable();
    }

    /// <summary>
    /// Sets or reset <see cref="ProgressionTag"/> Active <see cref="ProgressionTag.TagState"/>
    /// </summary>
    /// <param name="tagName">Name identifying <see cref="ProgressionTag"/></param>
    /// <param name="state">True to activate, false to deactivate</param>
    /// <returns>If active state was successful changed</returns>
    public static bool SetActiveTag(string tagName, bool state)
    {
        return SetActiveTag(Tags[tagName], state);
    }

    /// <summary>
    /// Sets or reset <see cref="ProgressionTag"/> Active <see cref="ProgressionTag.TagState"/>
    /// </summary>
    /// <param name="progressionTag"><see cref="ProgressionTag"/> to be activate/deactivate</param>
    /// <param name="state">True to activate, false to deactivate</param>
    /// <returns>If active state was successful changed</returns>
    public static bool SetActiveTag(ProgressionTag progressionTag, bool state)
    {
        if (!(progressionTag is TagNode tagNode))
            return false;

        if (!progressionTag.IsAvailable())
            return false;

        StartChange();

        tagNode.flags.active = state;

        EndChange();

        return true;
    }
}