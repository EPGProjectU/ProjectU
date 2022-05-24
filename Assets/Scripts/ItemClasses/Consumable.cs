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

    public override void Use(GameObject player)
    {
        bool wasUsed = player.GetComponent<PlayerHealthSystem>().Heal(Value);
        if (wasUsed) Debug.Log("Healed");
        else Debug.Log("Not Healed");
    }
}
