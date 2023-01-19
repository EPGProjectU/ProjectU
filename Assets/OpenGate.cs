using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGate : MonoBehaviour
{
    
    public TagHook hook;
    void Update()
    {
        if (hook.Tag.IsCollected())
        {
            this.gameObject.SetActive(false);
        }
    }
    public void CollectTag()
    {
        ProgressionManager.CollectTag(hook.TagName, true);
    }
}
