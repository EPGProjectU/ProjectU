using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompositeNode : BehaviourNode
{
    //preferably dynamic output ports if they can work please 
    [Output]
    public BehaviourNode child;

    [Output]
    public BehaviourNode child1;

    [Output]
    public BehaviourNode child2;
}
