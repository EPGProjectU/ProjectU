using UnityEngine;

public partial class ActorController
{
    private void OnDrawGizmos()
    {
        var playerPosition = transform.position;

        var playerMoveAngle = Vector2.SignedAngle(MovementVector, Vector2.down);

        var debugRadius = (running ? motionData.runningSpeed : motionData.baseSpeed) / 5f;

        DebugU.PushSettings(Color.white);
        DebugU.DrawLine(playerPosition + Vector3.left * debugRadius, playerPosition + Vector3.right * debugRadius);
        DebugU.DrawLine(playerPosition + Vector3.down * debugRadius, playerPosition + Vector3.up * debugRadius);

        DebugU.DrawCircle(playerPosition, debugRadius);
        DebugU.PopSettings();

        DebugU.PushSettings(Color.blue);
        DebugU.DrawLine(playerPosition, playerPosition + (Vector3)LookVector * debugRadius);
        DebugU.PopSettings();


        var motionVectorLength = motionData.EvaluateMotionForAngle(Mathf.DeltaAngle(playerMoveAngle, CharacterRotation));

        DebugU.PushSettings(Color.red);
        DebugU.DrawLine(playerPosition, playerPosition + (Vector3)MovementVector * debugRadius * motionVectorLength);
        DebugU.PopSettings();

        DebugU.PushSettings(Color.magenta);
        DebugU.DrawModifiedCircle(playerPosition, motionData.GetMovementCurve(), CharacterRotation, debugRadius);
        DebugU.PopSettings();
    }
}