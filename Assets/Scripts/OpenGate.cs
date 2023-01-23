using UnityEngine;

public class OpenGate : MonoBehaviour
{
    public TagHook hook;
    public TagNode.TagState hookStateThatOpensGate;

    void Update()
    {
        hook.onUpdate += (TagEvent e) => { 
            
            if (e.newState == hookStateThatOpensGate)
                gameObject.SetActive(false); 
        };
    }

    public void CollectTag() => hook.Collect();
}