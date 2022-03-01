/// <summary>
/// Object used for monitoring and referencing <see cref="ProgressionTag"/> in <see cref="ProgressionGraph"/>> managed by <see cref="ProgressionManager"/>
/// Is able to change <see cref="ProgressionTag"/> state and receive update callbacks whenever its state changes
/// </summary>
public partial class TagHook
{
    /// <summary>
    /// Creates new instance of the TagHook
    /// </summary>
    /// <param name="tagName">Name of the Tag to be tracked</param>
    /// <returns>Instantiated TagHook</returns>
    public static TagHook Create(string tagName = "") => Create_Impl(tagName);

    /// <summary>
    /// Breaks all links and removes hook from <see cref="ProgressionManager"/>
    /// </summary>
    public void Release() => Release_Impl();

    /// <summary>
    /// Name of the Tag to track
    /// </summary>
    /// <remarks>
    /// Renaming existing Hook allows to change tracked <see cref="Tag"/>
    /// </remarks>
    public string TagName
    {
        get => tagName;
        set => SetTagName_Impl(value);
    }

    /// <summary>
    /// Reference to the tracked Tag
    /// </summary>
    /// <remarks>
    /// The reference is set during runtime and only if there is a <see cref="ProgressionTag"/> with the given <see cref="TagName"/> exist in the current context
    /// </remarks>
    public ProgressionTag Tag { get; internal set; }

    /// <summary>
    /// Subscribe to receive event callbacks whenever state of the tracked Tag is updated
    /// </summary>
    /// <remarks>
    /// Method must be in format void(<see cref="TagEvent"/>) to be able to subscribe
    /// </remarks>
    public SerializableDelegate<TagDelegate> onUpdate;

    /// <summary>
    /// Set collected <see cref="ProgressionTag.State"/> on the <see cref="ProgressionTag"/>
    /// </summary>
    /// <param name="force">Collect  <see cref="ProgressionTag"/> even when it is not active</param>
    /// <returns>If <see cref="ProgressionTag"/> was successfully collected<br/>When <see cref="ProgressionTag"/> is forced return indicates if it was necessary </returns>
    public bool Collect(bool force = false) => ProgressionManager.CollectTag(Tag, force);

    /// <summary>
    /// Check if linked to <see cref="ProgressionTag"/>
    /// </summary>
    /// <returns>If linked to <see cref="ProgressionTag"/></returns>
    public bool IsLinked() => Tag != null;

    /// <summary>
    /// Returns <see cref="TagEvent"/> for the current  
    /// </summary>
    /// <returns><see cref="TagEvent"/> with <see cref="TagEvent.oldState"/> being the same as <see cref="TagEvent.newState"/></returns>
    /// <remarks>Should not be used before <see cref="TagHook"/> is linked in Awake phase</remarks>
    public TagEvent GetDummyTagEvent() => GetDummyTagEvent_Impl();

    /// <summary>
    /// Stores information about change to a <see cref="progressionTag"/>
    /// Used as parameter of <see cref="TagHook.onUpdate"/> events
    /// <br/><br/>
    /// hook - Hook that called the event <br/>
    /// progressionTag - Reference to the tracked <see cref="ProgressionTag"/> <br/>
    /// oldState - <see cref="ProgressionTag.TagState"/> of <see cref="ProgressionTag"/> before change<br/>
    /// newState -  <see cref="ProgressionTag.TagState"/> of <see cref="ProgressionTag"/> after change<br/>
    /// </summary>
    public struct TagEvent
    {
        public TagHook hook;
        public ProgressionTag progressionTag;
        public ProgressionTag.TagState oldState;
        public ProgressionTag.TagState newState;
    }
}