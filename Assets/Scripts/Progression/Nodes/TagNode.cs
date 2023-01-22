using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using XNode;


[Serializable]
[CreateNodeMenu("Progression/Tag", 0)]
public class TagNode : Node
{
    [Input]
    public bool input;

    [Output]
    public bool output;

    [Serializable]
    public struct Flags
    {
        public bool active;
        public bool collected;
    }

    public Flags flags;

    /// <summary>
    /// Possible states of <see cref="TagNode"/>
    /// </summary>
    public enum TagState
    {
        Unavailable,
        Available,
        Active,
        Collected
    }

    [field: FormerlySerializedAs("tagName")]
    [field: SerializeField]
    public string Name { get; set; }

    /// <summary>
    /// State is determined base on flags <see cref="active"/> and <see cref="collected"/>, if neither is set <see cref="input"/> is checked
    /// </summary>
    public TagState State =>
        IsCollected() ? TagState.Collected :
        IsActive() ? TagState.Active :
        IsAvailable() ? TagState.Available :
        TagState.Unavailable;

    public override object GetValue(NodePort port)
    {
        if (port.fieldName == "output")
            return IsCollected();

        return null;
    }

    public bool IsAvailable()
    {
        if (flags.active)
            return true;

        var values = GetInputPort("input").GetInputValues<bool>();

        return values.Length == 0 || values.Any(b => b);
    }

    public bool IsActive()
    {
        return flags.active || IsCollected();
    }

    public bool IsCollected()
    {
        return flags.collected;
    }


    public static Action<TagNode> QuestCreationCallback;

    [ContextMenu("Create Quest", false, 0)]
    public void CreateQuest()
    {
        QuestCreationCallback?.Invoke(this);
    }
}