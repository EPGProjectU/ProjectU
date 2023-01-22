using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Weapon", fileName = "New Weapon")]
public class Weapon : Item
{
    // Start is called before the first frame update
    public int Damage;
    public float Range;

    public override string getStats() {
        return "Weapon damage: " + Damage + "\n"
           + "Weapon range: " + Range;
    }

    public override void Use(GameObject target)
    {
        target.GetComponentInChildren<WeaponSlot>().UpdateWeaponDamage(new DamageInfo{damage = Damage});
    }
}
