using System;
using Debug;
using UnityEngine;

public class ExampleProgressionBehaviour : MonoBehaviour
{
    [SerializeField]
    private TagHook tagHook;
    private TagHook tagHook2 = TagHook.Create();
    
    private SerializableGUID guid = SerializableGUID.Generate();

    // Start is called before the first frame update
    private void Awake()
    {
        tagHook.OnInitialization += OnTagUpdate;
        tagHook.OnUpdate += OnTagUpdate;

        tagHook2.TagName = "MAIN/P1";

        tagHook2.OnUpdate += OnP1;

        DebugTooltip.Draw(transform.position, tagHook.TagName, Color.magenta, 6);
    }

    private void OnDestroy()
    {
        tagHook.OnInitialization -= OnTagUpdate;
        tagHook.OnUpdate -= OnTagUpdate;
    }

    private void OnTagUpdate(TagHook.TagEvent e)
    {
        GetComponent<SpriteRenderer>().color = e.NewState switch
        {
            ProgressionTag.TagState.Inactive => Color.gray,
            ProgressionTag.TagState.Active => new Color(0.27f, 0.39f, 0.28f),
            ProgressionTag.TagState.Collected => new Color(0.13f, 0.2f, 0.14f),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private void OnP1(TagHook.TagEvent e)
    {
        UnityEngine.Debug.Log(e.NewState);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            tagHook.Collect();
    }
}