using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public TagHook tagHook;
    public string Name;

    public Item(string name)
    {
        Name = name;
    }


    public abstract void Pickup();
    public abstract void Drop();


    //Item is turned-off from the scene
    //If it has a TagHook, it is collected(to be changed)
    //gone is probably mock-up (to be deleted)
    public void DestroyItem()
    {
        this.gameObject.SetActive(false);
        tagHook?.Collect();
    }
}
