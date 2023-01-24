using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public OpenGate gate;
    private void OnDestroy()
    {
        if (gate != null)
        {
            if (gate.hookStateThatOpensGate == TagNode.TagState.Collected) gate.CollectTag();
            if (gate.hookStateThatOpensGate == TagNode.TagState.Active) gate.hook.SetActive();
        }
    }
}
