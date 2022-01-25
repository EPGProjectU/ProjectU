using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : Item
{
    // Start is called before the first frame update
    public int ArmorValue;
    public Armor(string name, int armorValue) : base(name)
    {
        ArmorValue = armorValue;
    }
    public override void Pickup()
    {

    }
    public override void Drop()
    {

    }
}
