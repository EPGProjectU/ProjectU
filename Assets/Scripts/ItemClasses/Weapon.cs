using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    // Start is called before the first frame update
    public int Damage { get; set; }
    public Weapon(string name, int damage) : base(name)
    {
        Damage = damage;
    }

    public override void Pickup()
    {
        EquipmentController.PickupSingle();
    }
    public override void Drop()
    {
        EquipmentController.DropSingle();
    }
}
