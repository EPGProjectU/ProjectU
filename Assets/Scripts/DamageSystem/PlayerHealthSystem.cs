using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerHealthSystem : HealthSystem
{
    void Awake()
    {
        GetComponentInChildren<WeaponSlot>().Owner = myGroup;
    }
    // Start is called before the first frame update
    void Start()
    {
        allies.Add(myGroup);
    }
   
    /// <summary>
    /// Calculate amount of damage that will be taken by Player if he isn't invincilbe and when health drops below 1 uses OnDeath
    /// </summary>
    public override void TakeDamage(DamageInfo damage)
    {
        if (!isInvincible)
        {
            int dmg = damage.damage;
            isInvincible = true;
            if (armorDurability > 0) 
            { 
                dmg -= defence;
                if (dmg < 0) armorDurability += dmg;
                else armorDurability -= defence;
            }
            if (dmg > 0)
            {
                health -= dmg;
                Debug.Log("Health = " + health);
            }
            if (health < 1) OnDeath();
            else StartCoroutine(InvincibleTimer());
            if (damage.type == DamageType.Poison && !isPoisoned)
            {
                isPoisoned = true;
                StartCoroutine(Poisoned(damage.effectDuration, damage.specialDamage));
            }
            else if (damage.type == DamageType.Corrosion && !isCorroding)
            {
                isCorroding = true;
                StartCoroutine(Corroding(damage.effectDuration, damage.specialDamage));
            }
        }
    }
    protected new void OnDeath()
    {
        base.OnDeath();
        gameObject.GetComponent<PlayerController>().enabled = false;
        Debug.Log("Player is Dead");
    }
        
}
