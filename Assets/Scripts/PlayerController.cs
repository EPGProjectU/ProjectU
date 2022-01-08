using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ActorController dedicated to the player with implemented input handling
/// </summary>
public class PlayerController : ActorController
{
    private bool isDead;
    public GameObject weaponPrefab;
    // Start is called before the first frame update
    void Start()
    {
        // Calling setup of ActorController
        Setup();
        isDead = false;
        health = 3;
        DamageEventSystem.Instance.OnPlayerTakesDamage += TakeDamage;
        DamageEventSystem.Instance.OnPlayerDealsDamage += DealDamage;
    }

    // Update is called once per frame
    void Update()
    {
        // Updating player speed base on the input
        if (!isDead) 
        { 
            UpdateVelocity(GetVelocityFromInput());
            Attack();
        }
    }

    private void OnDestroy()
    {
        DamageEventSystem.Instance.OnPlayerTakesDamage -= TakeDamage;
        DamageEventSystem.Instance.OnPlayerDealsDamage -= DealDamage;
    }

    /// <summary>
    /// Applies speed and (if need) normalizes input to get current velocity of the player
    /// </summary>
    /// <returns>Velocity with applied speed and direction from the input</returns>
    private Vector2 GetVelocityFromInput()
    {
        var inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // Normalizing inputVector to avoid difference between max speeds of cardinal and diagonal directions
        if (inputVector.magnitude > 1.0)
            inputVector.Normalize();

        // Multiplying input vector by the selected movement speed
        return inputVector * (Input.GetKey("left shift") ? RunningSpeed : BaseSpeed);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Health = " + health);
        if (health < 1)
        {
            isDead = true;
            DamageEventSystem.Instance.PlayerIsDead();
            Debug.Log("Player is Dead");
        }
    }

    public void DealDamage()
    {
        Debug.Log("Player Dealt "+1+" Damage");
        DamageEventSystem.Instance.EnemyTakesDamage(1);
    }

    void Attack()
    {
        if (Input.GetButtonDown("Jump"))
        {
            float xDisplacement = Input.GetAxis("Horizontal");
            float yDisplacement = Input.GetAxis("Vertical");
            Vector3 pos = transform.position;
            pos.x += 1;
            pos.y += 0;
            float angle = Mathf.Atan2(yDisplacement, xDisplacement) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            GameObject weapon = Instantiate(weaponPrefab, pos, q);
        }
    }
}
