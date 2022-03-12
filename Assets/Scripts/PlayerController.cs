using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ActorController dedicated to the player with implemented input handling
/// </summary>
public class PlayerController : ActorController
{
    private bool isDead, isAttacking, isInvincible;
    public GameObject weaponPrefab;
    public int damage = 1;
    public float invincibleTime = 0.5f ;
    // Start is called before the first frame update
    void Start()
    {
        // Calling setup of ActorController
        Setup();
        isDead = false;
        isAttacking = false;
        isInvincible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Updating player speed base on the input
        if (!isDead) 
        { 
            UpdateVelocity(GetVelocityFromInput());
            if(!isAttacking)Attack();
        }
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

    /// <summary>
    /// Calculate amount of damage that will be taken by Player if he isn't invincilbe and change isdead to true when health drops below 1 (player is dead)
    /// </summary>
    public void TakeDamage(int damage)
    {
        if (!isInvincible) 
        {
            isInvincible = true;
            health -= damage;
            UnityEngine.Debug.Log("Health = " + health);
            if (health < 1)
            {
                isDead = true;
                UnityEngine.Debug.Log("Player is Dead");
            }
            StartCoroutine(InvincibleTimer());
        }
    }

    /// <summary>
    /// Calculate amount of damage that will be dealt to enemy
    /// </summary>
    /// <returns>Amount of Damage (int)</returns>
    public int DealDamage()
    {
        UnityEngine.Debug.Log("Player Dealt "+ damage + " Damage");
        return damage;
    }

    void Attack()
    {
        if (Input.GetMouseButtonDown(0)) //left mouse button
        {
            if(weaponPrefab != null)
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0f;
                Vector3 attackDir = (mousePosition - transform.position).normalized;
                //UnityEngine.Debug.Log(attackDir);

                GameObject weapon = Instantiate(weaponPrefab);
                weapon.GetComponent<DamageInfo>().damage = DealDamage();
                weapon.transform.SetParent(transform);

                if (attackDir.x < 0.7 && attackDir.x > -0.7 && attackDir.y > 0) weapon.GetComponent<Animator>().Play("Prototype_Swing_Up");
                else if (attackDir.x < 0.7 && attackDir.x > -0.7 && attackDir.y < 0) weapon.GetComponent<Animator>().Play("Prototype_Swing_Down");
                else if (attackDir.x < 0 && attackDir.y < 0.7 && attackDir.y > -0.7) weapon.GetComponent<Animator>().Play("Prototype_Swing_Left");
                else if (attackDir.x > 0 && attackDir.y < 0.7 && attackDir.y > -0.7) weapon.GetComponent<Animator>().Play("Prototype_Swing_Right");
                StartCoroutine(DestroyWeapon(weapon));
                isAttacking = true;
            }
        }
    }

    IEnumerator DestroyWeapon(GameObject weapon) {
        yield return new WaitForSeconds(0.3f);
        Destroy(weapon);
        isAttacking = false;
    }

    IEnumerator InvincibleTimer()
    {
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }
}
