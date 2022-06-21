using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public OpenGate gate;
    private void OnDestroy()
    {
        gate.CollectTag();
    }
}
