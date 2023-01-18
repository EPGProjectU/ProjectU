using System;
using ProjectU.Core;
using UnityEngine;

public class ProgressionTagTrigger : MonoBehaviour
{
    [SerializeField]
    private TagHook tagHook;
    
    private void OnDestroy()
    {
        tagHook.Release();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other._CompareTag("Player"))
            tagHook.Collect();
    }
}