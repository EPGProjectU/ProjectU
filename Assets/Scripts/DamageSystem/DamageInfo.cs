using System;
using UnityEngine;


[Serializable]
public class DamageInfo
{
    public int damage;
    public int specialDamage;
    public float effectDuration;
    public DamageType type = DamageType.Normal;

    public KnockBack knockBack = new KnockBack();

    [HideInInspector]
    public GameObject source;

    [HideInInspector]
    public GameObject target;

    public DamageInfo() {}

    public DamageInfo(DamageInfo info)
    {
        damage = info.damage;
        specialDamage = info.specialDamage;
        effectDuration = info.effectDuration;
        type = info.type;
        knockBack = info.knockBack;
        source = info.source;
        target = info.target;
    }

    public override int GetHashCode()
    {
        return 0;
    }
}

public enum DamageType
{
    Normal,
    Poison,
    Corrosion
}

[Serializable]
public class KnockBack
{
    public float force = 5;
    public float distance = 1;
    public float recoveryTime = 0.5f;

    [HideInInspector]
    public Vector2 direction;
}