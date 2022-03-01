using System;
using UnityEngine;
using UnityEngine.Serialization;
using XNode;

[Serializable]
[CreateNodeMenu("Progression/Tag", 0)]
public class TagNode: Node, ProgressionTag
{
    [Input]
    public bool input;

    [Output]
    public bool output;

    public bool active;
    public bool collected;

    [FormerlySerializedAs("_name")]
    [SerializeField]
    private string tagName;

    public string Name
    {
        get => tagName;
        set
        {
#if UNITY_EDITOR
            ProgressionManager.SendTagNameChangeNotifications(tagName, value, graph);
#endif
            tagName = value;
        }
    }

    public ProgressionTag.TagState State =>
        IsCollected() ? ProgressionTag.TagState.Collected :
        IsActive() ? ProgressionTag.TagState.Active :
        IsAvailable() ? ProgressionTag.TagState.Available :
        ProgressionTag.TagState.Unavailable;

    public override object GetValue(NodePort port)
    {
        if (port.fieldName == "output")
            return IsCollected();

        return null;
    }

    public bool IsAvailable()
    {
        var values = GetInputPort("input").GetInputValues<bool>();

        return values.Length == 0 || MathHelper.Or(values);
    }

    public bool IsActive()
    {
        return active || IsCollected();
    }

    public bool IsCollected()
    {
        return collected;
    }
}