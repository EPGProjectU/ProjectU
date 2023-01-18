using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGate : MonoBehaviour
{
    
    public TagHook hook;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hook.Tag.IsCollected())
        {
            this.gameObject.SetActive(false);
        }
    }
    public void CollectTag()
    {
        hook.Collect();
    }
}
