using XNode;
    
/// <summary>
/// Allows to access specific <see cref="ProgressionTag"/> from anywhere in graph
/// </summary>
[CreateNodeMenu("Progression/TagHook", 4)]
public class TagHookNode : Node, NotifyNodeInterface
{
    [Output]
    public ProgressionTag.TagState output;
    
    [Input]
    public ProgressionTag.TagState input;

    public TagHook tagHook = new TagHook();
    
    protected override void Init()
    {
        LinkHook();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.playModeStateChanged += x => LinkHook();
#endif
    }
    
    private void LinkHook()
    {
        ProgressionManager.RegisterTagHook(tagHook);
        tagHook.onUpdate += OnTagUpdate;
    }
    
    public override object GetValue(NodePort port)
    {
        if (port.fieldName == nameof(output))
            return tagHook.Tag?.IsCollected();

        return false;
    }

    private void OnTagUpdate(TagHook.TagEvent e)
    {
        input = e.newState;

        if (e.newState == ProgressionTag.TagState.Collected)
            NotifyNodeHelper.SendNotify(GetOutputPort(nameof(output)), e);
    }

    public bool Notify(object payload)
    {
        if (payload is bool b && b)
            return tagHook.Collect();

        return false;
    }
}