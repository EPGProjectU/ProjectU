using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Armor", fileName = "New Armor")]
public class Armor : Item
{
    // Start is called before the first frame update
    public int ArmorValue;
    public float Durability;

    public override string getStats() {
        return "Armor defense: " + ArmorValue +
            "\nArmor durability: " + Durability;
    }

    public override void Use(GameObject target)
    {
        target.GetComponent<HealthSystem>().UpdateDefence(ArmorValue,Durability);
    }
}
