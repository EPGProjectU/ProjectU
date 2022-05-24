using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item", fileName = "New Item")]
public class Item : ScriptableObject
{
    public string Name;
    public Sprite sprite;

    public override string ToString()
    {
        return "Nazwa: " + Name;
    }


    public virtual void Use(GameObject player)
    {

    }





    //Item is turned-off from the scene
    //If it has a TagHook, it is collected(to be changed)
    //gone is probably mock-up (to be deleted)
    //DestroyItem
}
