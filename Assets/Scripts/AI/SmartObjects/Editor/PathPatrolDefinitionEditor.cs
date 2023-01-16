using UnityEditor;
using UnityEngine;
using UnityEngine.AI;


[CustomEditor(typeof(PathPatrolDefinition))]
public class PathPatrolDefinitionEditor : Editor
{
    private void OnSceneGUI()
    {
        EditorGUI.BeginChangeCheck();

        var defaultHandleColor = Handles.color;

        var points = serializedObject.FindProperty(nameof(PathPatrolDefinition.path));

        if (points.arraySize == 0)
            return;

        var firstPoint = points.GetArrayElementAtIndex(0).vector3Value;
        var lastPoint = firstPoint;

        // Used for checking which points are invalid
        var validPaths = new bool[points.arraySize];

        // Draws paths
        for (var i = 1; i < points.arraySize; i++)
        {
            var point = points.GetArrayElementAtIndex(i).vector3Value;

            validPaths[i] = DrawPath(point, lastPoint);

            lastPoint = point;
        }

        // If path is not looped, last non-existent path is treated as valid
        validPaths[0] = true;

        // Draw last path if path is looped
        if (serializedObject.FindProperty(nameof(PathPatrolDefinition.loopPath)).boolValue)
            validPaths[0] = DrawPath(lastPoint, firstPoint);

        // Draw points
        for (var i = 0; i < points.arraySize; i++)
        {
            var point = points.GetArrayElementAtIndex(i).vector3Value;

            // If path from or to the point is invalid, the point is also treated as invalid
            Handles.color = validPaths[i] & validPaths[(i + 1) % points.arraySize] ? Color.white : Color.red;

            points.GetArrayElementAtIndex(i).vector3Value = Handles.FreeMoveHandle(point, Quaternion.identity, 0.2f, Vector3.zero, Handles.SphereHandleCap);
        }

        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();

        Handles.color = defaultHandleColor;
    }

    /// <summary>
    /// Draws patrol path on the NavMesh
    /// </summary>
    /// <param name="startPoint"></param>
    /// <param name="endPoint"></param>
    /// <returns>If NavMesh path exist</returns>
    /// <remarks>If path does not exist red dashed line is drawn between the points</remarks>
    private static bool DrawPath(Vector3 startPoint, Vector3 endPoint)
    {
        var navPath = new NavMeshPath();

        var pathComplete = NavMesh.CalculatePath(startPoint, endPoint, NavMesh.AllAreas, navPath);

        pathComplete &= navPath.status == NavMeshPathStatus.PathComplete;
        
        Handles.color = pathComplete ? Color.white : Color.red;

        Handles.DrawDottedLine(startPoint, endPoint, 3.0f);

        if (!pathComplete)
            return false;

        for (var j = 0; j < navPath.corners.Length - 1; j++)
        {
            Handles.DrawLine(navPath.corners[j], navPath.corners[j + 1], 3.0f);
        }

        return true;
    }
}