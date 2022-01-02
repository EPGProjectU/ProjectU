using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public string Name;

    public Item(string name)
    {
        Name = name;
    }


    public abstract void Pickup();
    public abstract void Drop();
}
