using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

namespace Rendering
{
    public class PixelFeatureRenderPass : ScriptableRenderPass
    {
        private readonly Material _blitMat;

        private readonly float _pixelDensity;

        private readonly ProfilingSampler _mProfilingSampler;
        private RenderStateBlock _mRenderStateBlock;
        private readonly List<ShaderTagId> _mShaderTagIdList = new List<ShaderTagId>();
        private FilteringSettings _mFilteringSettings;

        // Cache property IDs
        private static readonly int PixelTexID = Shader.PropertyToID("_PixelTexture");
        private static readonly int PixelDepthID = Shader.PropertyToID("_DepthTex");
        private static readonly int CameraID = Shader.PropertyToID("_CameraColorTexture");
        private static readonly int OrthographicSize = Shader.PropertyToID("_OrthographicSize");
        private static readonly int OutlineColor = Shader.PropertyToID("_OutlineColor");
        private static readonly int PixelDensity = Shader.PropertyToID("_PixelDensity");

        /// <summary>
        /// Setup for render pass
        /// </summary>
        /// <param name="renderEvent"></param>
        /// <param name="blitMaterial"></param>
        /// <param name="outlineColor"></param>
        /// <param name="ppu">Pixels Per Unit</param>
        /// <param name="layerMask"></param>
        public PixelFeatureRenderPass(RenderPassEvent renderEvent, Material blitMaterial
            , Color outlineColor , float ppu, int layerMask)
        {
            _mProfilingSampler = new ProfilingSampler("BasicFeature");
            this.renderPassEvent = renderEvent;
            _blitMat = blitMaterial;
            
            // Converts pixel per unit to pixel density
            _pixelDensity = 1 / ppu;
            _blitMat.SetFloat(PixelDensity, _pixelDensity);
            _blitMat.SetColor(OutlineColor, outlineColor);

            _mFilteringSettings = new FilteringSettings(RenderQueueRange.opaque, layerMask);

            _mShaderTagIdList.Add(new ShaderTagId("UniversalForward"));
            _mShaderTagIdList.Add(new ShaderTagId("LightweightForward"));
            _mShaderTagIdList.Add(new ShaderTagId("SRPDefaultUnlit"));

            _mRenderStateBlock = new RenderStateBlock(RenderStateMask.Nothing);
        }

        /// <summary>
        /// Renders target layer at lower resolution and blends them back together with the rest of the render
        /// </summary>
        /// <param name="context"></param>
        /// <param name="renderingData"></param>
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            const SortingCriteria sortingCriteria = SortingCriteria.CommonTransparent;

            var drawingSettings = CreateDrawingSettings(_mShaderTagIdList, ref renderingData, sortingCriteria);
            ref var cameraData = ref renderingData.cameraData;
            var camera = cameraData.camera;

            // If camera is orthographic get current size to keep ppu constant for all camera sizes
            var camSize = camera.orthographic ? camera.orthographicSize * 2 : 1;

            // Send camera size to shader
            _blitMat.SetFloat(OrthographicSize, camSize);
            
            // Gets resolution of texture for given pixel density
            var pixelWidth = Mathf.RoundToInt(camSize * camera.aspect / _pixelDensity);
            var pixelHeight = Mathf.RoundToInt(camSize / _pixelDensity);

            var cmd = CommandBufferPool.Get("BasicFeature");

            using (new ProfilingScope(cmd, _mProfilingSampler))
            {
                // Create render texture for color
                cmd.GetTemporaryRT(PixelTexID, pixelWidth, pixelHeight, 0, FilterMode.Point);
                // Create render texture for depth
                cmd.GetTemporaryRT(PixelDepthID, pixelWidth, pixelHeight, 24, FilterMode.Point, RenderTextureFormat.Depth);

                // Set render target to created textures
                cmd.SetRenderTarget(PixelTexID, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store,
                    PixelDepthID, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store);

                // Clear render textures
                cmd.ClearRenderTarget(true, true, Color.clear);

                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();

                context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref _mFilteringSettings, ref _mRenderStateBlock);

                // Set camera texture as render target
                cmd.SetRenderTarget(CameraID, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store);

                // Combine pixelated layer with the rest
                cmd.Blit(new RenderTargetIdentifier(PixelTexID), BuiltinRenderTextureType.CurrentActive, _blitMat);

                cmd.ReleaseTemporaryRT(PixelTexID);
                cmd.ReleaseTemporaryRT(PixelDepthID);

                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }
}