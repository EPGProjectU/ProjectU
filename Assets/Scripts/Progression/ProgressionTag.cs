public interface ProgressionTag
{
    public enum TagState
    {
        Unavailable,
        Available,
        Active,
        Collected
    }

    public string Name
    {
        get;
    }

    public TagState State
    {
        get;
    }

    /// <summary>
    /// Check if tag is available
    /// </summary>
    /// <remarks>
    /// It does not matter if tag is already in <see cref="TagState.Active"/> or <see cref="TagState.Collected"/> <see cref="TagState"/>, only if the condition for tag activation are met
    /// </remarks>
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
