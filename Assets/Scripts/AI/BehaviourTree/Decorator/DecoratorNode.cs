using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateNodeMenu("")]
public abstract class DecoratorNode : BehaviourNode
{
    [Output]
    public BehaviourNode child;

}
