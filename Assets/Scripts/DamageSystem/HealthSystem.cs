using System;
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

    public List<Ally> allies = new List<Ally>(); 

    /// <summary>
    /// Calculate amount of damage that will be taken by gameobject
    /// </summary>
    public abstract void TakeDamage(DamageInfo damage);

    protected void OnDeath()
    {
        deathCallback.Invoke(this);
    }
}