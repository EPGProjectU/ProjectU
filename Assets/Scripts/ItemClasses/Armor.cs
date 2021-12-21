using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : Item
{
    // Start is called before the first frame update
    public int ArmorValue { get; set; }
    public Armor(string name, int armorValue) : base(name)
    {
        ArmorValue = armorValue;
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
