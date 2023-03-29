/// <summary>
/// Tag used for storing progression states
/// </summary>
public interface TagNode2
{
    /// <summary>
    /// Possible states of <see cref="TagNode"/>
    /// </summary>
    public enum TagState
    {
        Unavailable,
        Available,
        Active,
        Collected
    }

    /// <summary>
    /// Identifier of the <see cref="TagNode"/>
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Current <see cref="TagState"/> of <see cref="TagNode"/>
    /// </summary>
    /// Also returns true for <see cref="TagState.Active"/> <see cref="TagState"/>
    public TagState State { get; }

    /// <summary>
    /// Check if tag is available for collection
    /// </summary>
    bool IsAvailable();

    /// <summary>
    /// Check if tag is active
    /// </summary>
    /// <remarks>
    /// Also returns true for <see cref="TagState.Collected"/> <see cref="TagState"/>
    /// </remarks>
    bool IsActive();

    /// <summary>
    /// Check if tag is collected
    /// </summary>
    bool IsCollected();
}