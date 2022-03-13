using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

namespace Rendering
{
    /// <summary>
    /// 
    /// </summary>
    public class PixelRenderFeature : ScriptableRendererFeature
    {
        [System.Serializable]
        public class PixelFeatureSettings
        {
            public LayerMask layerMask = 0;

            public RenderPassEvent Event = RenderPassEvent.BeforeRenderingTransparents;

            public Material blitMat = null;

            [FormerlySerializedAs("pixels")]
            [Range(4f, 512f)]
            public float pixelsPerUnit = 1f;
        }

        public PixelFeatureSettings settings = new PixelFeatureSettings();

        PixelFeatureRenderPass pass;

        public override void Create()
        {
            pass = new PixelFeatureRenderPass(settings.Event, settings.blitMat, settings.pixelsPerUnit, settings.layerMask);
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            renderer.EnqueuePass(pass);
        }
    }
}