using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentController : MonoBehaviour
{
    public Equipment equipment;

    public void PickupSingle(Item item)
    {
        equipment.items.Add(item);
        equipment.ShowEquipment();
    }
    public void PickupMultiple(Item item)
    {
        equipment.items.Add(item);
        equipment.ShowEquipment();
    }
    public void DropSingle(Item item)
    {
        equipment.items.Remove(item);
        equipment.ShowEquipment();
    }
    public void DropMultiple(Item item)
    {
        equipment.items.Remove(item);
        equipment.ShowEquipment();
    }

}
