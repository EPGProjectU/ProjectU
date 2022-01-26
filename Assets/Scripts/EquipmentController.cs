using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentController : MonoBehaviour
{
    public Equipment equipment;
    private int rigged = -1;

    public void PickupSingle(Item item)
    {
        if (rigged >= 0)
            equipment.items.Add(item);
        else
            rigged = 0;
        equipment.ShowEquipment();
        item.DestroyItem();
    }
    public void PickupMultiple(Item item)
    {
        equipment.items.Add(item);
        item.DestroyItem();
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
