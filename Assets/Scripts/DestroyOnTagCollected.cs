using System;
using UnityEngine;

public class DestroyOnTagCollected : MonoBehaviour
{
    public TagHook tagHook;
    
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
        if (e.NewState == ProgressionTag.TagState.Collected)
            Destroy(gameObject);
    }
}
