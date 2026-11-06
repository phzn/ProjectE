using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200002F RID: 47
	internal class UpscalePass : ScriptableRenderPass
	{
		// Token: 0x060001A4 RID: 420 RVA: 0x0000E940 File Offset: 0x0000CB40
		public UpscalePass(RenderPassEvent evt, Material blitMaterial)
		{
			base.renderPassEvent = evt;
			UpscalePass.m_BlitMaterial = blitMaterial;
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x0000E958 File Offset: 0x0000CB58
		public void Setup(RTHandle colorTargetHandle, int width, int height, FilterMode mode, ref RenderingData renderingData, out RTHandle upscaleHandle)
		{
			this.m_Source = colorTargetHandle;
			RenderTextureDescriptor cameraTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
			cameraTargetDescriptor.width = width;
			cameraTargetDescriptor.height = height;
			cameraTargetDescriptor.depthBufferBits = 0;
			RenderingUtils.ReAllocateIfNeeded(ref this.m_UpscaleHandle, in cameraTargetDescriptor, mode, TextureWrapMode.Clamp, false, 1, 0f, "_UpscaleTexture");
			upscaleHandle = this.m_UpscaleHandle;
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x0000E9B7 File Offset: 0x0000CBB7
		public void Dispose()
		{
			RTHandle upscaleHandle = this.m_UpscaleHandle;
			if (upscaleHandle == null)
			{
				return;
			}
			upscaleHandle.Release();
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x0000E9CC File Offset: 0x0000CBCC
		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			CommandBuffer commandBuffer = renderingData.commandBuffer;
			using (new ProfilingScope(commandBuffer, UpscalePass.m_ProfilingScope))
			{
				CoreUtils.SetRenderTarget(commandBuffer, this.m_UpscaleHandle, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, ClearFlag.None, Color.clear, 0, CubemapFace.Unknown, -1);
				base.Blit(commandBuffer, this.m_Source, this.m_UpscaleHandle, UpscalePass.m_BlitMaterial, 0);
			}
		}

		// Token: 0x040000FD RID: 253
		private static readonly ProfilingSampler m_ProfilingScope = new ProfilingSampler("Upscale Pass");

		// Token: 0x040000FE RID: 254
		private RTHandle m_Source;

		// Token: 0x040000FF RID: 255
		private RTHandle m_UpscaleHandle;

		// Token: 0x04000100 RID: 256
		private static Material m_BlitMaterial;
	}
}
