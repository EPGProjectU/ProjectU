using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//must get the actual instance of the behaviour tree connected to the actor to debug the tree or at least show which one is active
public interface NodeDebugger {
    public void Debug();
}
