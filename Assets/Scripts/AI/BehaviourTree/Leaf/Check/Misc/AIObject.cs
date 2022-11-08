using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Mono behaviour that allows objects to be added in parametrized graph so that AI actors can 'see' those objects
/// </summary>
[ParameterType(FriendlyName = "AIObject")]
public class AIObject : MonoBehaviour
{
    Transform transform;
    
    void Start()
    {
        transform = this.transform.parent.gameObject.transform;
    }

    void Update()
    {
        
    }
}
