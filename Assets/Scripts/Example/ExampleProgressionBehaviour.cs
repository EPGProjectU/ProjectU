using System;
using UnityEngine;

/// <summary>
/// Example of <see cref="TagHook"/> use in a gameobject
/// </summary>
public class ExampleProgressionBehaviour : MonoBehaviour
{
    // TagHook can be either exposed to editor
    [SerializeField]
    private TagHook tagHook;

    // or set in code
    private TagHook tagHook2 = TagHook.Create("MAIN/P1");

    /// <summary>
    /// Adding callbacks to <see cref="TagHook"/>.<see cref="TagHook.onUpdate"/> is advised to be done in Awake
    /// </summary>
    /// <remarks>There should not be bigger issue with adding callbacks during <see cref="Start"/>, but some change events of <see cref="ProgressionTag"/> could be missed</remarks>
    private void Awake()
    {
        // Callbacks are added like a normal delegates
        tagHook.onUpdate += ChangeColorOnTagUpdate;

        tagHook2.onUpdate += LogP1;
    }

    /// <summary>
    /// If you need to access progression tag for initialization do it post <see cref="Awake"/> as it is guaranteed that correctly configured tag will be linked post that phase, Start is a good place for that
    /// </summary>
    private void Start()
    {
        // Checking if tagHook is linked in case it was not setup
        if (tagHook.IsLinked())
        {
            DebugU.PushSettings(Color.magenta, -1f);
            // Name could be also be accessed through tagHook.TagName, in which case it is accessible even without being linked to a tag
            DebugU.Tooltip(transform.position + Vector3.back, tagHook.Tag.Name, 6);
            DebugU.PopSettings();

            // Using DummyTagEvent to offload portion of initialization to callback created for getting updates from tagHook
            ChangeColorOnTagUpdate(tagHook.GetDummyTagEvent());
        }
    }

    /// <summary>
    /// <see cref="TagHook"/> need to be released with <see cref="TagHook.Release"/>
    /// </summary>
    /// <remarks>No need to remove callback functions from <see cref="TagHook"/>.<see cref="TagHook.onUpdate"/>, it is taken care of by <see cref="TagHook.Release"/></remarks>
    private void OnDestroy()
    {
        tagHook.Release();
    }

    /// <summary>
    /// Example of <see cref="TagHook"/>.<see cref="TagHook.onUpdate"/> callback function
    /// </summary>
    /// <param name="tagEvent">Event with old and current state of the ><see cref="ProgressionTag"/></param>
    /// <remarks>All callbacks for <see cref="TagHook"/>.<see cref="TagHook.onUpdate"/> need to be void functions with single <see cref="TagHook.TagEvent"/> parameter</remarks>
    private void ChangeColorOnTagUpdate(TagHook.TagEvent tagEvent)
    {
        GetComponent<SpriteRenderer>().color = tagEvent.newState switch
        {
            ProgressionTag.TagState.Unavailable => Color.gray,
            ProgressionTag.TagState.Available => new Color(0.27f, 0.39f, 0.28f),
            ProgressionTag.TagState.Collected => new Color(0.13f, 0.2f, 0.14f),
            ProgressionTag.TagState.Active => new Color(0.13f, 0.22f, 0.33f),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    /// <summary>
    /// Second example of <see cref="TagHook"/>.<see cref="TagHook.onUpdate"/> callback function
    /// </summary>
    /// <param name="tagEvent">Event with old and current state of the <see cref="ProgressionTag"/></param>
    private void LogP1(TagHook.TagEvent tagEvent)
    {
        Debug.Log($"P1: {tagEvent.oldState} -> {tagEvent.newState}");
    }

    /// <summary>
    /// Example of use <see cref="TagHook"/>.<see cref="TagHook.Collect"/>
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            // Collect can be called multiple times without issues
            // Only when conditions are meet (tag is available) collection goes through
            tagHook.Collect();
    }
}