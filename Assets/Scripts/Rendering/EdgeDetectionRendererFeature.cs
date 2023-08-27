using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EdgeDetectionRendererFeature : ScriptableRendererFeature
{
    class EdgeDetectionRenderPass : ScriptableRenderPass
    {
        private RenderPassEvent _renderPassEvent;
        private RTHandle _cameraColorRTHandle;
        private RenderTextureDescriptor _cameraColorDescriptor;
        private RTHandle _tempRTHandle;
        private Material _edgeDetectionMaterial;

        public EdgeDetectionRenderPass(RenderPassEvent renderPassEvent, Material edgeDetectionMaterial)
        {
            this.renderPassEvent = renderPassEvent;
            _edgeDetectionMaterial = edgeDetectionMaterial;
            _tempRTHandle = RTHandles.Alloc
            (
                new RenderTargetIdentifier("_TempRTHandle"),
                name: "_TempRTHandle"
            );
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            _cameraColorRTHandle = renderingData.cameraData.renderer.cameraColorTargetHandle;
            _cameraColorDescriptor = renderingData.cameraData.cameraTargetDescriptor;
            _cameraColorDescriptor.depthBufferBits = 0;

            cmd.GetTemporaryRT
            (
                Shader.PropertyToID(_tempRTHandle.name),
                _cameraColorDescriptor,
                FilterMode.Bilinear
            );
            RenderingUtils.ReAllocateIfNeeded(ref _tempRTHandle, _cameraColorDescriptor, name: "_TempRTHamdle");
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get();
            using (new ProfilingScope(cmd, new ProfilingSampler("Edge Detection")))
            {
                Blitter.BlitCameraTexture(cmd, _cameraColorRTHandle, _tempRTHandle, _edgeDetectionMaterial, 0);
                Blitter.BlitCameraTexture(cmd, _tempRTHandle, _cameraColorRTHandle);
            }
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(Shader.PropertyToID(_tempRTHandle.name));
        }

        void Dispose()
        {
            _tempRTHandle?.Release();
        }
    }

    [SerializeField] private RenderPassEvent renderPassEvent;
    [SerializeField] private Shader edgeDetectionShader;

    private EdgeDetectionRenderPass _edgeDetectionRenderPass;
    private Material _edgeDetectionMaterial;

    public override void Create()
    {
        _edgeDetectionMaterial = CoreUtils.CreateEngineMaterial(edgeDetectionShader);
        // _edgeDetectionRenderPass = new EdgeDetectionRenderPass
        // (
        //     renderPassEvent,
        //     _edgeDetectionMaterial
        // );
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        //renderer.EnqueuePass(_edgeDetectionRenderPass);
    }

    protected override void Dispose(bool disposing)
    {
        CoreUtils.Destroy(_edgeDetectionMaterial);
    }
}


