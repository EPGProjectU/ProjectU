using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public TagHook tagHook;
    public string Name;
    private static bool gone = false;

    public Item(string name)
    {
        Name = name;
    }


    public abstract void Pickup();
    public abstract void Drop();

    public void DestroyItem()
    {
        this.gameObject.SetActive(false);
        tagHook?.Collect();
        if (!gone)
        {
            gone = true;
        }
    }
}
