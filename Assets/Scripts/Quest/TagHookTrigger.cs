using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagHookTrigger : MonoBehaviour
{
    public TagHook tagHook;

    public void setActive() {
        tagHook.SetActive();
    }
    public void CollectTag() {
        tagHook.Collect();
    }
}
