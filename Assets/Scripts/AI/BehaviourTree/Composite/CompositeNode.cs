using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateNodeMenu("")]
public abstract class CompositeNode : BehaviourNode
{
    [Output(dynamicPortList = true)]
    public BehaviourNode[] children;
}
