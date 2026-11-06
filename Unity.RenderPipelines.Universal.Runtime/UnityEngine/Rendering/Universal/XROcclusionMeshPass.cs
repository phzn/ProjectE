using System;
using UnityEngine.Experimental.Rendering.RenderGraphModule;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000B8 RID: 184
	public class XROcclusionMeshPass : ScriptableRenderPass
	{
		// Token: 0x060005F0 RID: 1520 RVA: 0x00024260 File Offset: 0x00022460
		public XROcclusionMeshPass(RenderPassEvent evt)
		{
			base.profilingSampler = new ProfilingSampler("XROcclusionMeshPass");
			base.renderPassEvent = evt;
			this.m_PassData = new XROcclusionMeshPass.PassData();
			this.m_IsActiveTargetBackBuffer = false;
			base.profilingSampler = new ProfilingSampler("XR Occlusion Pass");
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x000242AC File Offset: 0x000224AC
		private static void ExecutePass(ScriptableRenderContext context, XROcclusionMeshPass.PassData data)
		{
			CommandBuffer commandBuffer = data.renderingData.commandBuffer;
			if (data.renderingData.cameraData.xr.hasValidOcclusionMesh)
			{
				if (data.isActiveTargetBackBuffer)
				{
					commandBuffer.SetViewport(data.renderingData.cameraData.xr.GetViewport(0));
				}
				data.renderingData.cameraData.xr.RenderOcclusionMesh(commandBuffer, !data.isActiveTargetBackBuffer);
			}
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x0002431F File Offset: 0x0002251F
		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			this.m_PassData.renderingData = renderingData;
			this.m_PassData.isActiveTargetBackBuffer = this.m_IsActiveTargetBackBuffer;
			XROcclusionMeshPass.ExecutePass(context, this.m_PassData);
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x00024350 File Offset: 0x00022550
		internal void Render(RenderGraph renderGraph, in TextureHandle cameraColorAttachment, in TextureHandle cameraDepthAttachment, ref RenderingData renderingData)
		{
			XROcclusionMeshPass.PassData passData;
			using (RenderGraphBuilder renderGraphBuilder = renderGraph.AddRenderPass<XROcclusionMeshPass.PassData>("XR Occlusion Pass", out passData, base.profilingSampler))
			{
				passData.renderingData = renderingData;
				passData.cameraColorAttachment = renderGraphBuilder.UseColorBuffer(in cameraColorAttachment, 0);
				passData.cameraDepthAttachment = renderGraphBuilder.UseDepthBuffer(in cameraDepthAttachment, DepthAccess.Write);
				passData.isActiveTargetBackBuffer = this.m_IsActiveTargetBackBuffer;
				renderGraphBuilder.AllowPassCulling(false);
				renderGraphBuilder.SetRenderFunc<XROcclusionMeshPass.PassData>(delegate(XROcclusionMeshPass.PassData data, RenderGraphContext context)
				{
					XROcclusionMeshPass.ExecutePass(context.renderContext, data);
				});
			}
		}

		// Token: 0x040004B7 RID: 1207
		private XROcclusionMeshPass.PassData m_PassData;

		// Token: 0x040004B8 RID: 1208
		public bool m_IsActiveTargetBackBuffer;

		// Token: 0x020001A8 RID: 424
		private class PassData
		{
			// Token: 0x04000B19 RID: 2841
			internal RenderingData renderingData;

			// Token: 0x04000B1A RID: 2842
			internal TextureHandle cameraColorAttachment;

			// Token: 0x04000B1B RID: 2843
			internal TextureHandle cameraDepthAttachment;

			// Token: 0x04000B1C RID: 2844
			internal bool isActiveTargetBackBuffer;
		}
	}
}
