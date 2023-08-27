using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BloomRendererFeature : ScriptableRendererFeature
{
    class BloomRenderPass : ScriptableRenderPass
    {   
        private ToonBloomVolumeComponent _toonBloom;

        private Material _defaultBloomMaterial;
        private Material _bloomMaterial;

        private RTHandle _cameraColorTarget;
        private RTHandle _cameraDepthTarget;

        private RenderTextureDescriptor _descriptor;

        const int _maxPyramidSize = 16;
        private int[] _bloomMipUp;
        private int[] _bloomMipDown;
        private RTHandle[] _bloomMipUpRT;
        private RTHandle[] _bloomMipDownRT;
        private GraphicsFormat hdrFormat;

        public BloomRenderPass(Material defaultBloomMaterial, Material bloomMaterial)
        {
            _defaultBloomMaterial = defaultBloomMaterial;
            _bloomMaterial = bloomMaterial;

            _bloomMipUp = new int[_maxPyramidSize];
            _bloomMipDown = new int[_maxPyramidSize];
            _bloomMipUpRT = new RTHandle[_maxPyramidSize];
            _bloomMipDownRT = new RTHandle[_maxPyramidSize];

            for (int i = 0; i < _maxPyramidSize; i++)
            {
                _bloomMipUp[i] = Shader.PropertyToID("_BloomMipUp" + i);
                _bloomMipDown[i] = Shader.PropertyToID("_BloomMipDown" + i);

                _bloomMipUpRT[i] = RTHandles.Alloc(_bloomMipUp[i], name: "_BloomMipUp" + i);
                _bloomMipDownRT[i] = RTHandles.Alloc(_bloomMipDown[i], name: "_BloomMipDown" + i);
            }

            const FormatUsage usage = FormatUsage.Linear | FormatUsage.Render;
            if (SystemInfo.IsFormatSupported(GraphicsFormat.B10G11R11_UFloatPack32, usage))
            {
                hdrFormat = GraphicsFormat.B10G11R11_UFloatPack32;
            }
            else
            {
                hdrFormat = QualitySettings.activeColorSpace == ColorSpace.Linear ?
                    GraphicsFormat.R8G8B8A8_SRGB :
                    GraphicsFormat.R8G8B8A8_UNorm;
            }
        }

        private void SetupBloom(CommandBuffer cmd, RTHandle source)
        {
            int downres = 1;
            int tw = _descriptor.width >> downres;
            int th = _descriptor.height >> downres;

            int maxSize = Mathf.Max(tw, th);
            int iterations = Mathf.FloorToInt(Mathf.Log(maxSize, 2f) - 1);
            int mipCount = Mathf.Clamp(iterations, 1, _toonBloom.maxIterations.value);

            float clamp = _toonBloom.clamp.value;
            float threshold = Mathf.GammaToLinearSpace(_toonBloom.threshold.value);
            float thresholdKnee = threshold * 0.5f;

            float scatter = Mathf.Lerp(0.05f, 0.95f, _toonBloom.scatter.value);
            var bloomMaterial = _defaultBloomMaterial;

            bloomMaterial.SetVector("_Params", new Vector4(scatter, clamp, threshold, thresholdKnee));

            var desc = GetCompatibleDescriptor(tw, th, hdrFormat);
            for (int i = 0; i < mipCount; i++)
            {
                RenderingUtils.ReAllocateIfNeeded(ref _bloomMipUpRT[i], desc, FilterMode.Bilinear, TextureWrapMode.Clamp, name: _bloomMipUpRT[i].name);
                RenderingUtils.ReAllocateIfNeeded(ref _bloomMipDownRT[i], desc, FilterMode.Bilinear, TextureWrapMode.Clamp, name: _bloomMipDownRT[i].name);
                desc.width = Mathf.Max(1, desc.width >> 1);
                desc.height = Mathf.Max(1, desc.height >> 1);
            }

            Blitter.BlitCameraTexture(cmd, source, _bloomMipDownRT[0], RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, bloomMaterial, 0);

            var lastDown = _bloomMipDownRT[0];
            for (int i = 1; i < mipCount; i++)
            {
                Blitter.BlitCameraTexture(cmd, lastDown, _bloomMipUpRT[i], RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, bloomMaterial, 1);
                Blitter.BlitCameraTexture(cmd, _bloomMipUpRT[i], _bloomMipDownRT[i], RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, bloomMaterial, 2);

                lastDown = _bloomMipDownRT[i];
            }

            for (int i = mipCount - 2; i >= 0; i--)
            {
                var lowMip = (i == mipCount - 2) ? _bloomMipDownRT[i + 1] : _bloomMipUpRT[i + 1];
                var highMip = _bloomMipDownRT[i];
                var dst = _bloomMipUpRT[i];

                cmd.SetGlobalTexture("_SourceTexLowMip", lowMip);
                Blitter.BlitCameraTexture(cmd, highMip, dst, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, bloomMaterial, 3);
            }

            cmd.SetGlobalTexture("_BloomTexture", _bloomMipUpRT[0]);
            cmd.SetGlobalFloat("_BloomIntensity", _toonBloom.intensity.value);
        }

        RenderTextureDescriptor GetCompatibleDescriptor() => GetCompatibleDescriptor(_descriptor.width, _descriptor.height, _descriptor.graphicsFormat);
        RenderTextureDescriptor GetCompatibleDescriptor(int width, int height, GraphicsFormat format, DepthBits depthBufferBits = DepthBits.None) => GetCompatibleDescriptor(_descriptor, width, height, format, depthBufferBits);

        internal static RenderTextureDescriptor GetCompatibleDescriptor(RenderTextureDescriptor desc, int width, int height, GraphicsFormat format, DepthBits depthBufferBits = DepthBits.None)
        {
            desc.depthBufferBits = (int) depthBufferBits;
            desc.msaaSamples = 1;
            desc.width = width;
            desc.height = height;
            desc.graphicsFormat = format;
            return desc;
        }

        public void SetTarget(RTHandle colorHandle, RTHandle depthHandle)
        {
            _cameraColorTarget = colorHandle;
            _cameraDepthTarget = depthHandle;
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            _descriptor = renderingData.cameraData.cameraTargetDescriptor;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            VolumeStack stack = VolumeManager.instance.stack;
            _toonBloom = stack.GetComponent<ToonBloomVolumeComponent>();

            CommandBuffer cmd = CommandBufferPool.Get();

            using (new ProfilingScope(cmd, new ProfilingSampler("Toon Bloom")))
            {
                SetupBloom(cmd, _cameraColorTarget);

                _bloomMaterial.SetFloat("_Cutoff", _toonBloom.cutoff.value);

                Blitter.BlitCameraTexture(cmd, _cameraColorTarget, _cameraColorTarget, _bloomMaterial, 0);
            }

            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();

            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
        }
    }

    private BloomRenderPass _bloomRenderPass;

    private Material _defaultBloomMaterial;
    private Material _bloomMaterial;

    [SerializeField] private RenderPassEvent _renderPassEvent;

    [SerializeField] private Shader _defaultBloomShader;
    [SerializeField] private Shader _bloomShader;

    public override void Create()
    {
        _defaultBloomMaterial = CoreUtils.CreateEngineMaterial(_defaultBloomShader);
        _bloomMaterial = CoreUtils.CreateEngineMaterial(_bloomShader);

        _bloomRenderPass = new BloomRenderPass(_defaultBloomMaterial, _bloomMaterial);
        _bloomRenderPass.renderPassEvent = _renderPassEvent;
    }

    protected override void Dispose(bool disposing)
    {
        CoreUtils.Destroy(_defaultBloomMaterial);
        CoreUtils.Destroy(_bloomMaterial);
    }

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        if (renderingData.cameraData.cameraType == CameraType.Game)
        {
            _bloomRenderPass.ConfigureInput(ScriptableRenderPassInput.Depth);
            _bloomRenderPass.ConfigureInput(ScriptableRenderPassInput.Color);
            _bloomRenderPass.SetTarget(renderer.cameraColorTargetHandle, renderer.cameraDepthTargetHandle);
        }
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(_bloomRenderPass);
    }
}


