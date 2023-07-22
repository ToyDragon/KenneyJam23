using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

internal class ColorBlitPass : ScriptableRenderPass
{
    public Material material;
    private RTHandle m_CameraColorTarget;
    public ColorBlitPass(Material material)
    {
        this.material = material;
        renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
    }

    public void SetTarget(RTHandle colorHandle)
    {
        m_CameraColorTarget = colorHandle;
    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
    }
    

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
    	// Skipping post processing rendering inside the scene View
        if(renderingData.cameraData.isSceneViewCamera)
            return;
        
        CommandBuffer cmd = CommandBufferPool.Get("Custom Post Processing");
        cmd.Clear();

		// This holds all the current Volumes information
		// which we will need later
        // var stack = VolumeManager.instance.stack;


        //---Custom effect here---

        Blitter.BlitCameraTexture(cmd, m_CameraColorTarget, m_CameraColorTarget, material, 0);
       
        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    public override void OnCameraCleanup(CommandBuffer cmd)
    {
    }
}
