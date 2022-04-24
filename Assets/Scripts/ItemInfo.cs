using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo 
{
    public float distance;
    public GameObject item;

    public ItemInfo(float distance, GameObject item)
    {
        this.distance = distance;
        this.item = item;
        
    }
}
