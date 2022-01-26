using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ActorController dedicated to the player with implemented input handling
/// </summary>
public class PlayerController : ActorController
{
    private bool isDead, isAttacking;
    public GameObject weaponPrefab;
    // Start is called before the first frame update
    void Start()
    {
        // Calling setup of ActorController
        Setup();
        isDead = false;
        isAttacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Updating player speed base on the input
        if (!isDead)
        {
            UpdateVelocity(GetVelocityFromInput());
            if (!isAttacking) Attack();

        }
        if (Input.GetButtonDown("Cancel"))
        {
            SceneManager.LoadScene("PauseMenu");
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

    public void TakeDamage(int damage)
    {
        health -= damage;
        UnityEngine.Debug.Log("Health = " + health);
        if (health < 1)
        {
            isDead = true;
            DamageEventSystem.Instance.PlayerIsDead();
            UnityEngine.Debug.Log("Player is Dead");
        }
    }

    public int DealDamage()
    {
        UnityEngine.Debug.Log("Player Dealt " + 1 + " Damage");
        //DamageEventSystem.Instance.EnemyTakesDamage(1);
        return 1;
    }

    void Attack()
    {
        if (Input.GetButtonDown("Jump"))
        {
            GameObject weapon = Instantiate(weaponPrefab);
            weapon.GetComponent<DamageInfo>().damage = DealDamage();
            weapon.transform.SetParent(transform);
            if (Input.GetKey(KeyCode.W)) weapon.GetComponent<Animator>().Play("Prototype_Swing_Up");
            if (Input.GetKey(KeyCode.S)) weapon.GetComponent<Animator>().Play("Prototype_Swing_Down");
            if (Input.GetKey(KeyCode.A)) weapon.GetComponent<Animator>().Play("Prototype_Swing_Left");
            if (Input.GetKey(KeyCode.D)) weapon.GetComponent<Animator>().Play("Prototype_Swing_Right");
            StartCoroutine(DestroyWeapon(weapon));
            isAttacking = true;
        }
    }


    IEnumerator DestroyWeapon(GameObject weapon)
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(weapon);
        isAttacking = false;
    }

}
