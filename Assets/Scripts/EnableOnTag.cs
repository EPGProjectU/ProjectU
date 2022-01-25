using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnTag : MonoBehaviour
{
    public TagHook tagHook;
    public ProgressionTag.TagState triggerState = ProgressionTag.TagState.Collected;
    
    // Start is called before the first frame update
    private void Start()
    {
        tagHook.OnUpdate += OnTagUpdate;
        this.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        tagHook.OnUpdate -= OnTagUpdate;
    }

    private void OnTagUpdate(TagHook.TagEvent e)
    {
        if (e.NewState == triggerState)
            this.gameObject.SetActive(true);
    }
}
