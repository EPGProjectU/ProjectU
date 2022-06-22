using UnityEngine.SceneManagement;
using XNode;

public class WaypointNode : Node
{
    [Input]
    public bool input;
    
    [Output]
    public bool reached;

    public Scene scene;
    public Waypoint waypoint;
}
