using UnityEngine;

public class OpenGate : MonoBehaviour
{
    public TagHook hook;

    void Update()
    {
        hook.onCollect += () => { gameObject.SetActive(false); };
    }

    public void CollectTag() => hook.Collect();
}