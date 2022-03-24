using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple implementation of Damager class deals damage to anything that can be damaged
/// </summary>
public class SimpleDamager : Damager
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<HealthSystem>())
        {
            collision.gameObject.GetComponent<HealthSystem>().TakeDamage(damage);
        }
    }
}
