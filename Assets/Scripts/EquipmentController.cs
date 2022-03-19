using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentController : MonoBehaviour
{
    public Equipment equipment;


    //Item is first added to equipment, then equipment status is updated, and at the end item on Scene is destroyed
    public void PickupSingle(Item item)
    {
        equipment.items.Add(item);
        equipment.ShowEquipment();
        item.DestroyItem();
    }

    //To be commented
    public void PickupMultiple(Item item)
    {
        equipment.items.Add(item);
        equipment.ShowEquipment();
        item.DestroyItem();
    }

    //To be commented
    public void DropSingle(Item item)
    {
        equipment.items.Remove(item);
        equipment.ShowEquipment();
    }

    //To be commented
    public void DropMultiple(Item item)
    {
        equipment.items.Remove(item);
        equipment.ShowEquipment();
    }

}
