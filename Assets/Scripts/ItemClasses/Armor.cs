using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : Item
{
    // Start is called before the first frame update
    public int ArmorValue;
    public float Durability;
    public Armor(string name, int armorValue, int durability) : base(name)
    {
        ArmorValue = armorValue;
        Durability = durability;
    }
    public override void Pickup()
    {

    }
    public override void Drop()
    {

    }
}
