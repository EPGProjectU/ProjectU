using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    // Start is called before the first frame update
    public int Damage;
    public float Range;
    public Weapon(string name, int damage, float range) : base(name)
    {
        Damage = damage;
        Range = range;
    }

    public override void Pickup()
    {

    }
    public override void Drop()
    {

    }
}
