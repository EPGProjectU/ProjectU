using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Consumable", fileName = "New Consumable")]
public class Consumable : Item
{
    // Start is called before the first frame update
    public int Value;

    public override string ToString()
    {
        return base.ToString() + "\n"
            + "Value: " + Value;
    }
}
