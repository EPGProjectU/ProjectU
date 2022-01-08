public interface ProgressionTag
{
    public enum TagState
    {
        Inactive,
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

    bool IsActive();
    bool IsCollected();
}
