using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

namespace Rendering
{
    /// <summary>
    /// URP feature for rending pixelated layers
    /// </summary>
    public class PixelRenderFeature : ScriptableRendererFeature
    {
        [System.Serializable]
        public class PixelFeatureSettings
        {
            public LayerMask layerMask = 0;

            public RenderPassEvent @event = RenderPassEvent.BeforeRenderingTransparents;

            public Material blitMaterial = null;
            public Color outlineColor = Color.black;

            [FormerlySerializedAs("pixels")]
            [Range(4f, 512f)]
            public float pixelsPerUnit = 1f;
        }

        public PixelFeatureSettings settings = new PixelFeatureSettings();

        PixelFeatureRenderPass pass;

        public override void Create()
        {
            pass = new PixelFeatureRenderPass(settings.@event, settings.blitMaterial, settings.outlineColor, settings.pixelsPerUnit, settings.layerMask);
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            renderer.EnqueuePass(pass);
        }
    }
}