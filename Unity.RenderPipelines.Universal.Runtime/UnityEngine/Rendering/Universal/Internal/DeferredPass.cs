using System;
using UnityEngine.Experimental.Rendering.RenderGraphModule;

namespace UnityEngine.Rendering.Universal.Internal
{
	// Token: 0x0200011F RID: 287
	internal class DeferredPass : ScriptableRenderPass
	{
		// Token: 0x06000975 RID: 2421 RVA: 0x0003F374 File Offset: 0x0003D574
		public DeferredPass(RenderPassEvent evt, DeferredLights deferredLights)
		{
			base.profilingSampler = new ProfilingSampler("DeferredPass");
			base.renderPassEvent = evt;
			this.m_DeferredLights = deferredLights;
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x0003F39C File Offset: 0x0003D59C
		public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescripor)
		{
			RTHandle rthandle = this.m_DeferredLights.GbufferAttachments[this.m_DeferredLights.GBufferLightingIndex];
			RTHandle depthAttachmentHandle = this.m_DeferredLights.DepthAttachmentHandle;
			if (this.m_DeferredLights.UseRenderPass)
			{
				base.ConfigureInputAttachments(this.m_DeferredLights.DeferredInputAttachments, this.m_DeferredLights.DeferredInputIsTransient);
			}
			base.ConfigureTarget(rthandle, depthAttachmentHandle);
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x0003F3FE File Offset: 0x0003D5FE
		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			this.m_DeferredLights.ExecuteDeferredPass(context, ref renderingData);
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x0003F410 File Offset: 0x0003D610
		internal void Render(RenderGraph renderGraph, TextureHandle color, TextureHandle depth, TextureHandle[] gbuffer, ref RenderingData renderingData)
		{
			DeferredPass.PassData passData;
			using (RenderGraphBuilder renderGraphBuilder = renderGraph.AddRenderPass<DeferredPass.PassData>("Deferred Lighting Pass", out passData, base.profilingSampler))
			{
				passData.color = renderGraphBuilder.UseColorBuffer(in color, 0);
				passData.depth = renderGraphBuilder.UseDepthBuffer(in depth, DepthAccess.ReadWrite);
				passData.deferredLights = this.m_DeferredLights;
				passData.renderingData = renderingData;
				for (int i = 0; i < gbuffer.Length; i++)
				{
					if (i != this.m_DeferredLights.GBufferLightingIndex)
					{
						renderGraphBuilder.ReadTexture(in gbuffer[i]);
					}
				}
				renderGraphBuilder.AllowPassCulling(false);
				renderGraphBuilder.SetRenderFunc<DeferredPass.PassData>(delegate(DeferredPass.PassData data, RenderGraphContext context)
				{
					data.deferredLights.ExecuteDeferredPass(context.renderContext, ref data.renderingData);
				});
			}
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x0003F4E8 File Offset: 0x0003D6E8
		public override void OnCameraCleanup(CommandBuffer cmd)
		{
			this.m_DeferredLights.OnCameraCleanup(cmd);
		}

		// Token: 0x04000885 RID: 2181
		private DeferredLights m_DeferredLights;

		// Token: 0x020001E6 RID: 486
		private class PassData
		{
			// Token: 0x04000C9E RID: 3230
			internal TextureHandle color;

			// Token: 0x04000C9F RID: 3231
			internal TextureHandle depth;

			// Token: 0x04000CA0 RID: 3232
			internal RenderingData renderingData;

			// Token: 0x04000CA1 RID: 3233
			internal DeferredLights deferredLights;
		}
	}
}
