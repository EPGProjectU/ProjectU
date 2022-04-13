using System.Collections.Generic;
using UnityEngine;

public class DebugU
{
    private struct Settings
    {
        public Color _color;
        public float _duration;
        public bool _depthTest;
    }

    private static Stack<Settings> DebugSettings = new Stack<Settings>();

    private static readonly AnimationCurve ConstantCurve = AnimationCurve.Constant(0f, 1f, 1f);

    static DebugU()
    {
        DebugSettings.Push(new Settings { _color = Color.white, _duration = 0f, _depthTest = true });
    }

    public static void PushSettings(Color color, float duration = 0f, bool depthTest = true)
    {
        DebugSettings.Push(new Settings { _color = color, _duration = duration, _depthTest = depthTest });
    }

    public static void PopSettings()
    {
        DebugSettings.Pop();
    }

    public static void DrawLine(Vector3 start, Vector3 end)
    {
        Debug.DrawLine(start, end, DebugSettings.Peek()._color, DebugSettings.Peek()._duration, DebugSettings.Peek()._depthTest);
    }

    public static void DrawCircle(Vector3 center, float radius = 1f, int numberOfSegments = 32)
    {
        DrawModifiedCircle(center, ConstantCurve, 0f, radius, numberOfSegments);
    }

    public static void DrawModifiedCircle(Vector3 center, AnimationCurve curve, float rotation, float radius = 1f, int numberOfSegments = 32)
    {
        var circlePoint = Quaternion.Euler(0f, 0f, -rotation) * Vector3.up * radius;
        var angle = 360f / numberOfSegments;


        var pointModifier = curve.Evaluate(0f);

        for (var i = 1; i <= numberOfSegments; i++)
        {
            var nextCirclePoint = Quaternion.Euler(0f, 0f, angle) * circlePoint;
            var nextPointModifier = curve.Evaluate((float)i / numberOfSegments);

            DrawLine(circlePoint * pointModifier + center, nextCirclePoint * nextPointModifier + center);

            circlePoint = nextCirclePoint;
            pointModifier = nextPointModifier;
        }
    }

    public static void Tooltip(Vector3 position, string text, float fontSize = 1)
    {
        var gameObject = new GameObject("Debug Text");
        gameObject.AddComponent<MeshRenderer>();

        // prevents text from appearing in the scene hierarchy
        gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;

        gameObject.transform.position = position;

        var textMesh = gameObject.AddComponent<TextMesh>();

        textMesh.color = DebugSettings.Peek()._color;
        textMesh.text = text;

        // To prevent blurriness of rendered text, character size is scaled down while font size is scaled up by the same amount 
        const int fontScale = 50;
        var floorFontSize = Mathf.FloorToInt(fontSize);

        textMesh.fontSize = floorFontSize * fontScale;
        textMesh.characterSize = 2.0f / fontScale * (1 + fontSize - floorFontSize);

        // If duration is non negative, schedule destruction of the text
        if (DebugSettings.Peek()._duration >= 0)
            Object.Destroy(gameObject, DebugSettings.Peek()._duration);
    }
}