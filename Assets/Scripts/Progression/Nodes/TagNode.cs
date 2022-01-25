using System;
using UnityEngine;
using XNode;

[Serializable]
[CreateNodeMenu("Progression/Tag", 0)]
public class TagNode : Node, ProgressionTag
{
    [Input]
    public bool input;

    [Output]
    public bool output;

    public bool collected;

    [SerializeField]
    private string _name;

    public string Name
    {
        get => _name;
        set
        {
            ProgressionManager.SendTagNameChangeNotifications(_name, value, graph);
            _name = value;
        }
    }

    public ProgressionTag.TagState State => IsCollected() ? ProgressionTag.TagState.Collected : IsActive() ? ProgressionTag.TagState.Active : ProgressionTag.TagState.Inactive;

    public override object GetValue(NodePort port)
    {
        if (port.fieldName == "output")
            return collected;

        return null;
    }

    public bool IsActive()
    {
        var values = GetInputPort("input").GetInputValues<bool>();

        return values.Length == 0 || MathHelper.Or(values);
    }

    public bool IsCollected()
    {
        return collected;
    }
}