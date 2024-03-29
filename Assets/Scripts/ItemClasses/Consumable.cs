using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Consumable", fileName = "New Consumable")]
public class Consumable : Item
{
    // Start is called before the first frame update
    public int Value;

    public override string getStats() {
        return "Item value: " + Value;
    }

    public override void Use(GameObject target)
    {
        if (target.GetComponent<HealthSystem>().Heal(Value)) Debug.Log("Healed");
        else Debug.Log("Not Healed");
    }
}
