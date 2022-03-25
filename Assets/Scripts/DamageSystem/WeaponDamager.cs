using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamager : Damager
{
    public Ally Owner;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<HealthSystem>())
        {
            if (!collision.gameObject.GetComponent<HealthSystem>().allies.Contains(Owner)) collision.gameObject.GetComponent<HealthSystem>().TakeDamage(damage);
        }
    }
}
