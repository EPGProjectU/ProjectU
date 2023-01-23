using UnityEngine;

/// <summary>
/// Object used for monitoring and referencing <see cref="TagNode"/> in <see cref="ProgressionGraph"/>> managed by <see cref="ProgressionManager"/>
/// Is able to change <see cref="TagNode"/> state and receive update callbacks whenever its state changes
/// </summary>
public partial class TagHook
{
    /// <summary>
    /// Creates new instance of the <see cref="TagHook"/>
    /// </summary>
    /// <param name="tagName">Name of the <see cref="TagNode"/> to be tracked</param>
    /// <returns>Instantiated <see cref="TagHook"/></returns>
    public static TagHook Create(string tagName = "") => Create_Impl(tagName);

    /// <summary>
    /// Breaks all links, removes hook from <see cref="ProgressionManager"/> and frees all callbacks of <see cref="onUpdate"/>
    /// </summary>
    public void Release() => Release_Impl();

    /// <summary>
    /// Reference to the tracked <see cref="TagNode"/>
    /// </summary>
    /// <remarks>
    /// The reference is set during runtime and only if there is a <see cref="TagNode"/> with the given <see cref="TagName"/> exist in the current context
    /// </remarks>
    [field: SerializeField]
    public TagNode Tag { get; set; }

    /// <summary>
    /// Subscribe to receive event callbacks whenever state of the tracked Tag is updated
    /// </summary>
    /// <remarks>
    /// Method must be in format void(<see cref="TagEvent"/>) to be able to subscribe
    /// </remarks>
    public HookUpdateDelegate<UpdateDelegateWithEvent> onUpdate;

    public HookUpdateDelegate<UpdateDelegate> onCollect;
    public HookUpdateDelegate<UpdateDelegate> onActivate;


    /// <summary>
    /// Set collected <see cref="TagNode.State"/> on the <see cref="TagNode"/>
    /// </summary>
    /// <param name="force">Collect <see cref="TagNode"/> even when it is not available or active</param>
    /// <returns>If <see cref="TagNode"/> was successfully collected<br/>When <see cref="TagNode"/> is forced return indicates if it was necessary</returns>
    public bool Collect(bool force = false) => ProgressionManager.CollectTag(Tag, force);

    /// <summary>
    /// Set active <see cref="TagNode.State"/> on the <see cref="TagNode"/>
    /// </summary>
    /// <param name="state">True to activate False to inactivate</param>
    /// <returns>If <see cref="TagNode"/> had successfully changed active state</returns>
    public bool SetActive(bool state = true) => ProgressionManager.SetActiveTag(Tag, state);
}