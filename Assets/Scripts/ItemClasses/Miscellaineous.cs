using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miscellaineous : Item
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void Pickup()
    {
        EquipmentController.PickupMultiple();
    }
    public override void Drop()
    {
        EquipmentController.DropMultiple();
    }
}
