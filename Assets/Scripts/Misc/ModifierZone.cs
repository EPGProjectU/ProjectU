using System.Collections.Generic;
using ProjectU.Core;
using UnityEngine;

public class ModifierZone : MonoBehaviour
{
    // TODO create more advance filtering system
    public TagList tagFilter;

    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.GetComponent<ActorController>() != null && !target._CompareAnyTag(tagFilter))
            OnEnter(target.gameObject);
    }

    private void OnTriggerExit2D(Collider2D target)
    {
        if (target.GetComponent<ActorController>() != null && !target._CompareAnyTag(tagFilter))
            OnExit(target.gameObject);
    }

    protected virtual void OnEnter(GameObject target)
    {
    }

    protected virtual void OnExit(GameObject target)
    {
        
    }
}
