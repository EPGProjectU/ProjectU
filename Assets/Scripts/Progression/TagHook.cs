using System;

[Serializable]
public partial class TagHook
{
    /// <summary>
    /// Tag name that is used to link tag hook with corresponding progression tag in progression manager
    /// </summary>
    /// <remarks>
    /// It is possible to change TagName during runtime to change progression tag that is tracked
    /// </remarks>
    public ProgressionTag Tag { get; set; }

    public SerializableDelegate<TagDelegate> OnUpdate;
    public SerializableDelegate<TagDelegate> OnInitialization;

    /// <summary>
    /// Set collect flag on the tag
    /// </summary>
    /// <param name="force">Collect tag even when it is not available</param>
    /// <returns> If tag was marked successfully as collected, If tag was forced return indicates if it was necessary </returns>
    public bool Collect(bool force = false)
    {
        return ProgressionManager.CollectTag(Tag, force);
    }

    public partial struct TagEvent
    {
        public TagHook Hook;
        public ProgressionTag ProgressionTag;
        public ProgressionTag.TagState OldState;
        public ProgressionTag.TagState NewState;
    }
}