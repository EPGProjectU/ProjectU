using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct DamageInfo
{
    public int damage;

    public DamageInfo(int damage)
    {
        this.damage = damage;
    }

}
