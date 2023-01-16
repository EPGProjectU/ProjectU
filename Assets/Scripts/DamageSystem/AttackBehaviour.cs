using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackBehaviour : StateMachineBehaviour
{
    /// <summary>
    /// Percent of animation's startup time with inactive collider
    /// </summary>
    public float startupPercent;

    /// <summary>
    /// Percent of animation's end time with inactive collider
    /// </summary>
    public float recoveryPercent;

    private WeaponSlot _weaponSlot;
    private Coroutine _attackCoroutine;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _weaponSlot = animator.GetComponentInChildren<WeaponSlot>();

        var startupSeconds = startupPercent * stateInfo.length;
        var lengthSeconds = stateInfo.length * (1 - startupPercent - recoveryPercent);

        _attackCoroutine = _weaponSlot.StartCoroutine(Attack(startupSeconds, lengthSeconds));
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _weaponSlot.StopCoroutine(_attackCoroutine);
        _weaponSlot.weapon.GetComponent<Collider2D>().enabled = false;
    }

    protected virtual IEnumerator Attack(float delaySeconds, float lengthSeconds)
    {
        var collider = _weaponSlot.weapon.GetComponent<Collider2D>();
        yield return new WaitForSeconds(delaySeconds);

        collider.enabled = true;
        yield return new WaitForSeconds(lengthSeconds);

        collider.enabled = false;
    }
}