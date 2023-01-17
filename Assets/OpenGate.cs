using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGate : MonoBehaviour
{
    public TagHook hook;
   
    public void CollectTag()
    {
        ProgressionManager.CollectTag(hook.TagName, true);
        this.gameObject.SetActive(false);
    }
}
