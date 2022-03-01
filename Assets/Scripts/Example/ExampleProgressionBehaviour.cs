using System;
using Debug;
using UnityEngine;

public class ExampleProgressionBehaviour : MonoBehaviour
{
    [SerializeField]
    private TagHook tagHook;
    private TagHook tagHook2 = TagHook.Create("MAIN/P1");

    // Start is called before the first frame update
    private void Awake()
    {
        tagHook.onUpdate += OnTagUpdate;

        tagHook2.onUpdate += LogP1;
    }

    private void Start()
    {
        if (tagHook.IsLinked())
        {
            DebugTooltip.Draw(transform.position, tagHook.TagName, Color.magenta, 6);
            
            OnTagUpdate(tagHook.GetDummyTagEvent());
        }
    }

    private void OnDestroy()
    {
        tagHook.Release();
    }

    private void OnTagUpdate(TagHook.TagEvent e)
    {
        GetComponent<SpriteRenderer>().color = e.newState switch
        {
            ProgressionTag.TagState.Unavailable => Color.gray,
            ProgressionTag.TagState.Available => new Color(0.27f, 0.39f, 0.28f),
            ProgressionTag.TagState.Collected => new Color(0.13f, 0.2f, 0.14f),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private void LogP1(TagHook.TagEvent e)
    {
        UnityEngine.Debug.Log($"P1: {e.oldState} -> {e.newState}");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            tagHook.Collect();
    }
}