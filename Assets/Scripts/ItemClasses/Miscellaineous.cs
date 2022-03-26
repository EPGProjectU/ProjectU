using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miscellaineous : Item
{
    // Start is called before the first frame update
    enum ItemTypes
    {
        Quest,
        Parcel,
        Other

    }

    public int ItemType;

    public Miscellaineous(string name, int itemType) : base(name)
    {
        ItemType = itemType;
    }
    public override void Pickup()
    {

    }
    public override void Drop()
    {

    }
}
