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
        isInvincible = false;
    }
   
    /// <summary>
    /// Calculate amount of damage that will be taken by Player if he isn't invincilbe and when health drops below 1 uses OnDeath
    /// </summary>
    public override void TakeDamage(DamageInfo damage)
    {
        if (!isInvincible)
        {
            isInvincible = true;
            health -= damage.damage;
            Debug.Log("Health = " + health);
            if (health < 1) OnDeath();
            else StartCoroutine(InvincibleTimer());
        }
    }
    protected new void OnDeath()
    {
        base.OnDeath();
        gameObject.GetComponent<PlayerController>().enabled = false;
        Debug.Log("Player is Dead");
    }

    //returns false if healing was unnecessary and potion shouldn't be discarded
    public bool Heal(int healAmount)
    {
        if (healAmount + health < maxHealth) health += healAmount;
        else if(health != maxHealth) health = maxHealth;
        else return false;
        return true;
    }

}
