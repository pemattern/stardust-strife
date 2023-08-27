using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ViewSpaceDepthNormalsTextureRendererFeature : ScriptableRendererFeature
{
    class ViewSpaceNormalsTextureRenderPass : ScriptableRenderPass
    {
        private RTHandle _normalsRTHandle; 
        private RTHandle _colorRTHandle;
        private RenderTextureDescriptor _normalsRenderTextureDescriptor;
        private Material _normalsMaterial;
        private List<ShaderTagId> _shaderTagIdList;
        private LayerMask _filteringLayerMask;
        private int _depthBufferBits;
        private RenderTextureFormat _renderTextureFormat;
        private FilterMode _filterMode;

        public ViewSpaceNormalsTextureRenderPass(RenderPassEvent renderPassEvent,
            Material normalsMaterial,
            LayerMask filteringLayerMask,
            int depthBufferBits,
            RenderTextureFormat renderTextureFormat,
            FilterMode filterMode)
        {
            this.renderPassEvent = renderPassEvent;
            _normalsRTHandle = RTHandles.Alloc(new RenderTargetIdentifier("_NormalsTexture"), name: "_NormalsTexture");
            _normalsMaterial = normalsMaterial;
            _filteringLayerMask = filteringLayerMask;
            _depthBufferBits = depthBufferBits;
            _renderTextureFormat = renderTextureFormat;
            _shaderTagIdList = new List<ShaderTagId>
            {
                new ShaderTagId("UniversalForward"),
                new ShaderTagId("UniversalForwardOnly"),
                new ShaderTagId("LightweightForward"),
                new ShaderTagId("SRPDefaultUnlit"),
            };
        }
        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            RenderTextureDescriptor normalsTextureDescriptor = cameraTextureDescriptor;
            normalsTextureDescriptor.colorFormat = _renderTextureFormat;
            normalsTextureDescriptor.depthBufferBits = _depthBufferBits;
            cmd.GetTemporaryRT
            (
                Shader.PropertyToID(_normalsRTHandle.name), 
                normalsTextureDescriptor,
                _filterMode
            );
            ConfigureTarget(_normalsRTHandle);
        }
        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            _colorRTHandle = renderingData.cameraData.renderer.cameraColorTargetHandle;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get();
            using(new ProfilingScope(cmd, new ProfilingSampler("ViewSpaceNormalsTexture")))
            {
                DrawingSettings drawingSettings = CreateDrawingSettings(_shaderTagIdList, ref renderingData, renderingData.cameraData.defaultOpaqueSortFlags);
                drawingSettings.overrideMaterial = _normalsMaterial;
                FilteringSettings filteringSettings = FilteringSettings.defaultValue;
                filteringSettings.layerMask = _filteringLayerMask;

                context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref filteringSettings);
            }   
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(Shader.PropertyToID(_normalsRTHandle.name));
        }

        void Dispose()
        {
            _normalsRTHandle?.Release();
        }
    }

    [SerializeField] private RenderPassEvent _renderPassEvent;
    [SerializeField] private DepthNormalsTextureMode _depthNormalsTextureMode;
    [SerializeField] private LayerMask _filteringLayerMask;
    [SerializeField] private int _depthBufferBits;
    [SerializeField] private RenderTextureFormat _renderTextureFormat;
    [SerializeField] private FilterMode _filterMode;
    [SerializeField] private Shader _normalsShader;


    private ViewSpaceNormalsTextureRenderPass _viewSpaceNormalsTextureRenderPass;
    private Material _normalsMaterial;

    public override void Create()
    {
        _normalsMaterial = CoreUtils.CreateEngineMaterial(_normalsShader ?? Shader.Find("Shader Graphs/ViewSpaceNormalsTexture"));

        _viewSpaceNormalsTextureRenderPass = new ViewSpaceNormalsTextureRenderPass
        (
            _renderPassEvent,
            _normalsMaterial,
            _filteringLayerMask,
            _depthBufferBits,
            _renderTextureFormat,
            _filterMode
        );
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (_depthNormalsTextureMode.HasFlag(DepthNormalsTextureMode.Depth))
            renderer.EnqueuePass(_viewSpaceNormalsTextureRenderPass);
    }

    protected override void Dispose(bool disposing)
    {
        CoreUtils.Destroy(_normalsMaterial);
    }

    [Flags]
    public enum DepthNormalsTextureMode
    {
        Depth = 0x2,
        Normals = 0x1,
    }
}


