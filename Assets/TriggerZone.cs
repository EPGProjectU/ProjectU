using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    // Start is called before the first frame update
    private TagHook hook;
    void Start()
    {
        hook = gameObject.GetComponent<OpenGate>().hook;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CollectTag();
    }

    public void CollectTag()
    {
        hook.Collect();
    }
}
