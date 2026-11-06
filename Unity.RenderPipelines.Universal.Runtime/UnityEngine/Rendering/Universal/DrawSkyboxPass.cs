using System;
using UnityEngine.Experimental.Rendering.RenderGraphModule;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000AE RID: 174
	public class DrawSkyboxPass : ScriptableRenderPass
	{
		// Token: 0x060005A4 RID: 1444 RVA: 0x00020854 File Offset: 0x0001EA54
		public DrawSkyboxPass(RenderPassEvent evt)
		{
			base.profilingSampler = new ProfilingSampler("DrawSkyboxPass");
			base.renderPassEvent = evt;
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x00020874 File Offset: 0x0001EA74
		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			ref CameraData ptr = ref renderingData.cameraData;
			DebugHandler activeDebugHandler = ScriptableRenderPass.GetActiveDebugHandler(ref renderingData);
			if (activeDebugHandler != null && activeDebugHandler.IsScreenClearNeeded)
			{
				return;
			}
			RendererList rendererList = this.CreateSkyboxRendererList(context, ptr);
			if (ptr.xr.enabled)
			{
				if (ptr.xr.singlePassEnabled)
				{
					renderingData.commandBuffer.SetSinglePassStereo(SystemInfo.supportsMultiview ? SinglePassStereoMode.Multiview : SinglePassStereoMode.Instancing);
				}
				if (this.m_IsActiveTargetBackBuffer)
				{
					renderingData.commandBuffer.SetViewport(ptr.xr.GetViewport(0));
				}
			}
			renderingData.commandBuffer.DrawRendererList(rendererList);
			if (ptr.xr.enabled && ptr.xr.singlePassEnabled)
			{
				renderingData.commandBuffer.SetSinglePassStereo(SinglePassStereoMode.None);
			}
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x0002092C File Offset: 0x0001EB2C
		private RendererList CreateSkyboxRendererList(ScriptableRenderContext context, CameraData cameraData)
		{
			RendererList rendererList = default(RendererList);
			if (cameraData.xr.enabled)
			{
				if (cameraData.xr.singlePassEnabled)
				{
					rendererList = context.CreateSkyboxRendererList(cameraData.camera, cameraData.GetProjectionMatrix(0), cameraData.GetViewMatrix(0), cameraData.GetProjectionMatrix(1), cameraData.GetViewMatrix(1));
				}
				else
				{
					rendererList = context.CreateSkyboxRendererList(cameraData.camera, cameraData.GetProjectionMatrix(0), cameraData.GetViewMatrix(0));
				}
			}
			else
			{
				rendererList = context.CreateSkyboxRendererList(cameraData.camera);
			}
			return rendererList;
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x000209BC File Offset: 0x0001EBBC
		internal void Render(RenderGraph renderGraph, TextureHandle colorTarget, TextureHandle depthTarget, ref RenderingData renderingData)
		{
			DrawSkyboxPass.PassData passData;
			using (RenderGraphBuilder renderGraphBuilder = renderGraph.AddRenderPass<DrawSkyboxPass.PassData>("Draw Skybox Pass", out passData, base.profilingSampler))
			{
				passData.color = renderGraphBuilder.UseColorBuffer(in colorTarget, 0);
				passData.depth = renderGraphBuilder.UseDepthBuffer(in depthTarget, DepthAccess.Read);
				passData.renderingData = renderingData;
				passData.pass = this;
				renderGraphBuilder.AllowPassCulling(false);
				renderGraphBuilder.SetRenderFunc<DrawSkyboxPass.PassData>(delegate(DrawSkyboxPass.PassData data, RenderGraphContext context)
				{
					data.pass.Execute(context.renderContext, ref data.renderingData);
				});
			}
		}

		// Token: 0x0400044E RID: 1102
		public bool m_IsActiveTargetBackBuffer;

		// Token: 0x0200019B RID: 411
		private class PassData
		{
			// Token: 0x04000AB3 RID: 2739
			internal TextureHandle color;

			// Token: 0x04000AB4 RID: 2740
			internal TextureHandle depth;

			// Token: 0x04000AB5 RID: 2741
			internal RenderingData renderingData;

			// Token: 0x04000AB6 RID: 2742
			internal DrawSkyboxPass pass;
		}
	}
}
