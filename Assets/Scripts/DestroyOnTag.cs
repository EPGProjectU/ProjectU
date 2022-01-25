using System;
using UnityEngine;

public class DestroyOnTag : MonoBehaviour
{
    public TagHook tagHook;
    public ProgressionTag.TagState triggerState = ProgressionTag.TagState.Collected;
    
    // Start is called before the first frame update
    private void Start()
    {
        tagHook.OnUpdate += OnTagUpdate;
    }

    private void OnDestroy()
    {
        tagHook.OnUpdate -= OnTagUpdate;
    }

    private void OnTagUpdate(TagHook.TagEvent e)
    {
        if (e.NewState == triggerState)
            Destroy(gameObject);
    }
}
