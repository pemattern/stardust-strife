using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DistanceFogRendererFeature : ScriptableRendererFeature
{
    class DistanceFogRenderPass : ScriptableRenderPass
    {
        private Material _distanceFogMaterial;
        private Color _color;
        private float _density;

        public DistanceFogRenderPass(Material distanceFogMaterial, Color color, float density)
        {
            _distanceFogMaterial = distanceFogMaterial;
            _color = color;
            _density = density;
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {

        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
        }
    }

    private DistanceFogRenderPass _distanceFogRenderPass;
    private Material _distanceFogMaterial;

    [SerializeField] private RenderPassEvent _renderPassEvent;
    [SerializeField] private Color _color;
    [SerializeField] private float _density;
    [SerializeField] private Shader _distanceFogShader;
    
    public override void Create()
    {
        _distanceFogMaterial = CoreUtils.CreateEngineMaterial(_distanceFogShader);

        _distanceFogRenderPass = new DistanceFogRenderPass(_distanceFogMaterial, _color, _density);
        _distanceFogRenderPass.renderPassEvent = _renderPassEvent;
    }

    protected override void Dispose(bool disposing)
    {
        CoreUtils.Destroy(_distanceFogMaterial);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(_distanceFogRenderPass);
    }
}


