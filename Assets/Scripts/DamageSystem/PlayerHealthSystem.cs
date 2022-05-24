using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerHealthSystem : HealthSystem
{
    public int defence;
    public float armorDurability;
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
            int tmp = damage.damage;
            isInvincible = true;
            if (armorDurability > 0) 
            { 
                tmp -= defence;
                if (tmp < 0) armorDurability += tmp;
                else armorDurability -= defence;
            }
            if (tmp > 0)
            {
                health -= tmp;
                Debug.Log("Health = " + health);
            }
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
