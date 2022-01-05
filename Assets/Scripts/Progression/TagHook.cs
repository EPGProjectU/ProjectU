using System;

[Serializable]
public partial class TagHook
{
    /// <summary>
    /// Tag name that is used to link tag hook with corresponding progression tag in progression manager
    /// </summary>
    /// <remarks>
    /// Setting it during runtime re-registers tracked tag
    /// </remarks>
    public string TagName
    {
        get => tagName;
        set => TagNameSetInternal(value);
    }

    public ProgressionTag Tag { get; internal set; }

    public SerializableDelegate<TagDelegate> OnUpdate;
    public SerializableDelegate<TagDelegate> OnInitialization;

    public static TagHook Create(string tagName = "") => CreateInternal(tagName);

    /// <summary>
    /// Set collect flag on the tag
    /// </summary>
    /// <param name="force">Collect tag even when it is not available</param>
    /// <returns> If tag was marked successfully as collected, If tag was forced return indicates if it was necessary </returns>
    public bool Collect(bool force = false) => ProgressionManager.CollectTag(Tag, force);

    public partial struct TagEvent
    {
        public TagHook Hook;
        public ProgressionTag ProgressionTag;
        public ProgressionTag.TagState OldState;
        public ProgressionTag.TagState NewState;
    }
}