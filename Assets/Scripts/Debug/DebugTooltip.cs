using UnityEngine;

namespace DebugU
{
    /// <summary>
    /// Draws in-game text for debug purposes
    /// </summary>
    public class DebugTooltip
    {
        private DebugTooltip(Vector3 position, string text, Color color, float fontSize = 1, float duration = -1.0f)
        {
            var gameObject = new GameObject("Debug Text");
            gameObject.AddComponent<MeshRenderer>();

            // prevents text from appearing in the scene hierarchy
            gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;

            gameObject.transform.position = position;

            var textMesh = gameObject.AddComponent<TextMesh>();

            textMesh.color = color;
            textMesh.text = text;

            // To prevent blurriness of rendered text, character size is scaled down while font size is scaled up by the same amount 
            const int fontScale = 50;
            var floorFontSize = Mathf.FloorToInt(fontSize);

            textMesh.fontSize = floorFontSize * fontScale;
            textMesh.characterSize = 2.0f / fontScale * (1 + fontSize - floorFontSize);

            // If duration is non negative, schedule destruction of the text
            if (duration >= 0)
                Object.Destroy(gameObject, duration);
        }

        public static DebugTooltip Draw(Vector3 position, string text)
        {
            return new DebugTooltip(position, text, Color.white);
        }

        public static DebugTooltip Draw(Vector3 position, string text, Color color)
        {
            return new DebugTooltip(position, text, color);
        }

        public static DebugTooltip Draw(Vector3 position, string text, Color color, float fontSize, float duration = -1.0f)
        {
            return new DebugTooltip(position, text, color, fontSize, duration);
        }
    }
}