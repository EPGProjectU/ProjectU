using XNode;

/// <summary>
/// Allows to access specific <see cref="TagNode"/> from anywhere in graph
/// </summary>
[CreateNodeMenu("Progression/TagHook", 4)]
public class TagHookNode : Node, NotifyNodeInterface
{
    [Output]
    public TagNode.TagState output;

    [Input]
    public TagNode.TagState input;

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
        tagHook.onUpdate -= OnTagUpdate;
        tagHook.onUpdate += OnTagUpdate;
    }

    public override object GetValue(NodePort port)
    {
        return tagHook.Tag?.State;
    }

    private void OnTagUpdate(TagEvent e)
    {
        if (!GetOutputPort(nameof(output)).IsConnected)
            return;

        if (e.newState == input)
            NotifyNodeHelper.SendNotify(GetOutputPort(nameof(output)), e);
    }

    public bool Notify(object payload)
    {
        if (payload is bool b && b)
            return tagHook.Collect();

        return false;
    }
}