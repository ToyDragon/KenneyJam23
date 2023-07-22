using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

internal class CustomPPRendererFeature : ScriptableRendererFeature
{
    public static CustomPPRendererFeature instance;
    public List<Shader> blitShaders;
    public List<ColorBlitPass> colorBlitPasses = new List<ColorBlitPass>();
    public List<Material> colorBlitMaterials = new List<Material>();
    public override void AddRenderPasses(ScriptableRenderer renderer,
                                    ref RenderingData renderingData)
    {
        if (renderingData.cameraData.cameraType == CameraType.Game) {
            foreach (var pass in colorBlitPasses) {
                renderer.EnqueuePass(pass);
            }
        }
    }

    public override void SetupRenderPasses(ScriptableRenderer renderer,
                                        in RenderingData renderingData)
    {
        if (renderingData.cameraData.cameraType == CameraType.Game)
        {
            foreach (var pass in colorBlitPasses) {
                pass.ConfigureInput(ScriptableRenderPassInput.Color | ScriptableRenderPassInput.Depth);
                pass.SetTarget(renderer.cameraColorTargetHandle);
            }
        }
    }

    public override void Create()
    {
        instance = this;
        colorBlitPasses.Clear();
        colorBlitMaterials.Clear();
        foreach (var shader in blitShaders) {
            if (shader == null) {
                continue;
            }
            Debug.Log("Creating mat for shader " + shader.name);
            var mat = CoreUtils.CreateEngineMaterial(shader);
            colorBlitMaterials.Add(mat);
            var newPass = new ColorBlitPass(mat);
            colorBlitPasses.Add(newPass);
        }
    }

    protected override void Dispose(bool disposing)
    {
        foreach (var material in colorBlitMaterials) {
            CoreUtils.Destroy(material);
        }
    }
}
