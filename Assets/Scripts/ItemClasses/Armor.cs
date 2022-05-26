using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Armor", fileName = "New Armor")]
public class Armor : Item
{
    // Start is called before the first frame update
    public int ArmorValue;
    public float Durability;

    public override string ToString()
    {
        return base.ToString() + "\n"
            + "ArmorValue: " + ArmorValue + "\n"
            + "Durability: " + Durability;
    }
    public override void Use(GameObject player)
    {
        player.GetComponent<PlayerHealthSystem>().UpdateDefence(ArmorValue,Durability);
    }
}
