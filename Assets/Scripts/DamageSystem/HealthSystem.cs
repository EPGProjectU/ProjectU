using System;
using System.Collections;
using System.Collections.Generic;
using ProjectU;
using UnityEngine;
public enum Ally
{
    Player,
    Enemy,
    NPC
}

public abstract class HealthSystem : MonoBehaviour
{
    public SerializableDelegate<Action<HealthSystem>> deathCallback;

    /// <summary>
    /// Amount of health (in hearts) that actor currently haves
    /// </summary>
    public int health;
    public int maxHealth;
    protected bool isInvincible = false;
    protected bool isPoisoned = false;
    protected bool isCorroding = false;
    public float invincibleTime;
    public int defence;
    public float armorDurability;

    public Ally myGroup;
    public List<Ally> allies = new List<Ally>();

    /// <summary>
    /// Calculate amount of damage that will be taken by gameobject
    /// </summary>
    public virtual void TakeDamage(DamageInfo damage)
    {
        if (!isInvincible)
        {
            StartCoroutine(InvincibleTimer());
            health -= damage.damage;
            if (health < 1) OnDeath();
            ApplySpecialEffect(damage);
        }
    }
    protected void ApplySpecialEffect(DamageInfo damage)
    {
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

    protected virtual void OnDeath()
    {
        
        var controller = gameObject.GetComponent<ActorController>();
        
        controller.LookVector = Vector2.zero;
        controller.MovementVector = Vector2.zero;
        var colliders = gameObject.GetComponents<Collider2D>();
        
        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }
        
        GetComponent<ActorController>().Dead = true;
//        NotificationManger.Call("OnDeath", new NotificationManger.Payload(gameObject, this));
        deathCallback.Invoke(this);
        
        NotificationManger.CallOnDeath(gameObject);
    }

    public void UpdateDefence(int defence, float armorDurability)
    {
        this.defence = defence;
        this.armorDurability = armorDurability;
    }

    //returns false if healing was unnecessary and potion shouldn't be discarded
    public bool Heal(int healAmount)
    {
        if (healAmount + health < maxHealth) health += healAmount;
        else if (health != maxHealth) health = maxHealth;
        else return false;
        return true;
    }

    protected IEnumerator Poisoned(float time, int damage)
    {
        float duration = 0;
        while (time > duration)
        {
            if (health - damage < 2) 
            {
                health = 1;
                break;
            }
            health -= damage;
            yield return new WaitForSeconds(1);
            duration++;
        }
        isPoisoned = false;
    }

    protected virtual IEnumerator Corroding(float time, int damage)
    {
        float duration = 0;
        while (time > duration)
        {            
            if (armorDurability - damage < 0)
            {
                armorDurability = 0;
                break;
            }
            armorDurability -= damage;
            yield return new WaitForSeconds(1);
            duration++;
        }
        isCorroding = false;
    }

    protected IEnumerator InvincibleTimer()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }
}