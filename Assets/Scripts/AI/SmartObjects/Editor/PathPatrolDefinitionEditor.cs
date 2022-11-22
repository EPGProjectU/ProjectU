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


        for (var i = 0; i < points.arraySize; i++)
        {
            var point = points.GetArrayElementAtIndex(i).vector3Value;
            points.GetArrayElementAtIndex(i).vector3Value = Handles.FreeMoveHandle(point, Quaternion.identity, 0.2f, Vector3.zero, Handles.SphereHandleCap);

            DrawPath(point, lastPoint);

            lastPoint = point;
        }

        if (serializedObject.FindProperty(nameof(PathPatrolDefinition.loopPath)).boolValue)
            DrawPath(lastPoint, firstPoint);

        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();

        Handles.color = defaultHandleColor;
    }


    /// <summary>
    /// Draws patrol path on the NavMesh
    /// </summary>
    /// <param name="startPoint"></param>
    /// <param name="endPoint"></param>
    private void DrawPath(Vector3 startPoint, Vector3 endPoint)
    {
        var _navPath = new NavMeshPath();

        var pathExist = NavMesh.CalculatePath(startPoint, endPoint, NavMesh.AllAreas, _navPath);

        Handles.color = pathExist ? Color.white : Color.red;

        for (int j = 0; j < _navPath.corners.Length - 1; j++)
        {
            Handles.DrawLine(_navPath.corners[j], _navPath.corners[j + 1], 3.0f);
        }

        Handles.DrawDottedLine(startPoint, endPoint, 3.0f);
    }
}