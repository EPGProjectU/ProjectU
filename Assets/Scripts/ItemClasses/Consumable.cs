using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : Item
{
    // Start is called before the first frame update
    public int Value;
    public Consumable(string name, int value) : base(name)
    {
        Value = value;
    }
    public override void Pickup()
    {

    }
    public override void Drop()
    {

    }
}
