using UnityEngine;

// Debug tooltip for drawing text in the game world
namespace Debug
{
    public class DebugTooltip
    {
        private readonly GameObject _gameObject;
        private DebugTooltip(Vector3 position, string text, Color color, float fontSize = 1, float duration = -1.0f)
        {
            _gameObject = new GameObject("Debug Text");
            _gameObject.AddComponent<MeshRenderer>();

            // prevents text from appearing in the scene hierarchy
            _gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;

            _gameObject.transform.position = position;

            var textMesh = _gameObject.AddComponent<TextMesh>();

            textMesh.color = color;
            textMesh.text = text;

            // To prevent blurriness of rendered text, character size is scaled down while font size is scaled up by the same amount 
            const int fontScale = 50;
            var floorFontSize = Mathf.FloorToInt(fontSize);

            textMesh.fontSize = floorFontSize * fontScale;
            textMesh.characterSize = 2.0f / fontScale * (1 + fontSize - floorFontSize);

            // If duration is non negative, schedule destruction of the text
            if (duration >= 0)
                Destroy(duration);
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

        // Destroys the text after a delay
        public void Destroy(float delay = 0.0f)
        {
            Object.Destroy(_gameObject, delay);
        }
    }
}