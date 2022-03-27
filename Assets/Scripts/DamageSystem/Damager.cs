using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class responsible for dealing damage
/// </summary>
[RequireComponent(typeof(Collider2D))]
public abstract class Damager : MonoBehaviour
{
    public DamageInfo damage;
    
}
