/// <summary>
/// Stores information about change to a <see cref="TagNode"/>
/// Used as parameter of <see cref="TagHook.onUpdate"/> events
/// <br/><br/>
/// hook - Hook that called the event <br/>
/// TagNode - Reference to the tracked <see cref="TagNode"/> <br/>
/// oldState - <see cref="TagNode.TagState"/> of <see cref="TagNode"/> before change<br/>
/// newState -  <see cref="TagNode.TagState"/> of <see cref="TagNode"/> after change<br/>
/// </summary>
public struct TagEvent
{
    public TagNode tagNode;
    public TagNode.TagState oldState;
    public TagNode.TagState newState;
}