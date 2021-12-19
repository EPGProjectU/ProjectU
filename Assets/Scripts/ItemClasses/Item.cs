using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public string Name { get; set; }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public abstract void Pickup();
    public abstract void Drop();
}
