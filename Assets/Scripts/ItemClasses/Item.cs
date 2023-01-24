using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item", fileName = "New Item")]
public class Item : ScriptableObject
{
    public string Name;
    public Sprite sprite;
    public string description;

    public override string ToString()
    {
        return "Nazwa: " + Name;
    }

    public virtual string getStats() { return "Item has no statistics"; }

    public virtual void Use(GameObject target)
    {

    }





    //Item is turned-off from the scene
    //If it has a TagHook, it is collected(to be changed)
    //gone is probably mock-up (to be deleted)
    //DestroyItem
}
