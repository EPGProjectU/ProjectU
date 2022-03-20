using UnityEngine;

[CreateAssetMenu(menuName = "ProjectU/Actor Motion Data")]
public class ActorMotionData : ScriptableObject
{
    public float baseSpeed = 5;
    public float runningSpeed = 10;
    
    public float rotationSpeed = 360f;
}
