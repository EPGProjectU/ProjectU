using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Weapon", fileName = "New Weapon")]
public class Weapon : Item
{
    // Start is called before the first frame update
    public int Damage;
    public float Range;

    public override string ToString()
    {
        return base.ToString() + "\n"
            + "Damage: " + Damage + "\n"
            + "Range: " + Range;
    }
}