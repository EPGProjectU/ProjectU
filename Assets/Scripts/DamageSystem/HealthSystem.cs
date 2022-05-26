using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Ally
{
    Player,
    Enemy
}

public abstract class HealthSystem : MonoBehaviour
{
    public SerializableDelegate<Action<HealthSystem>> deathCallback;

    /// <summary>
    /// Amount of health (in hearts) that actor currently haves
    /// </summary>
    public int health;
    public int maxHealth;
    protected bool isInvincible;
    public float invincibleTime;
    public int defence;
    public float armorDurability;

    public Ally myGroup;
    public List<Ally> allies = new List<Ally>(); 

    /// <summary>
    /// Calculate amount of damage that will be taken by gameobject
    /// </summary>
    public abstract void TakeDamage(DamageInfo damage);

    protected void OnDeath()
    {
        deathCallback.Invoke(this);
    }

    public void UpdateDefence(int defence, float armorDurability)
    {
        this.defence = defence;
        this.armorDurability = armorDurability;
    }

    protected IEnumerator InvincibleTimer()
    {
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }
}