using UnityEngine;

public class OpenGate : MonoBehaviour
{
    public TagHook hook;
    public TagNode.TagState hookStateThatOpensGate;

    void Start()
    {
        hook.onUpdate += Open;
    }
    private void OnDestroy()
    {
        hook.onUpdate -= Open;
    }

    private void Open(TagEvent e)
    {
        if (e.newState == hookStateThatOpensGate)
            gameObject.SetActive(false);
    }

    public void CollectTag() => hook.Collect();
}