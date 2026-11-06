using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200002D RID: 45
	internal class PixelPerfectBackgroundPass : ScriptableRenderPass
	{
		// Token: 0x06000196 RID: 406 RVA: 0x0000DC69 File Offset: 0x0000BE69
		public PixelPerfectBackgroundPass(RenderPassEvent evt)
		{
			base.renderPassEvent = evt;
		}

		// Token: 0x06000197 RID: 407 RVA: 0x0000DC78 File Offset: 0x0000BE78
		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			CommandBuffer commandBuffer = renderingData.commandBuffer;
			using (new ProfilingScope(commandBuffer, PixelPerfectBackgroundPass.m_ProfilingScope))
			{
				CoreUtils.SetRenderTarget(commandBuffer, BuiltinRenderTextureType.CameraTarget, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, ClearFlag.Color, Color.black);
			}
		}

		// Token: 0x040000E9 RID: 233
		private static readonly ProfilingSampler m_ProfilingScope = new ProfilingSampler("Pixel Perfect Background Pass");
	}
}
