using UnityEngine;

[CreateAssetMenu(menuName = "ProjectU/Actor Motion Data")]
public class ActorMotionData : ScriptableObject
{
    public float baseSpeed = 5;
    public float runningSpeed = 10;

    public float rotationSpeed = 360f;

    [SerializeField]
    private AnimationCurve movementCurve = AnimationCurve.Constant(0f, 1f, 1f);

    public float EvaluateMotionForAngle(float angle)
    {
        return movementCurve.Evaluate((180f + angle) / 360f);
    }

    public AnimationCurve GetMovementCurve()
    {
        return movementCurve;
    }
}