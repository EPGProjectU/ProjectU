using System.Collections.Generic;
using ProjectU.Core;
using UnityEngine;

public class TagModifierZone : ModifierZone
{
    public TagList tags;

    protected override void OnEnter(GameObject target)
    {
        base.OnEnter(target);

        target._AddTags(tags);
    }

    protected override void OnExit(GameObject target)
    {
        base.OnExit(target);

        target._RemoveTags(tags);
    }
}