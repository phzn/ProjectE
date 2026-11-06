using System;
using UnityEngine.Experimental.Rendering.RenderGraphModule;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000F3 RID: 243
	internal class ClearTargetsPass
	{
		// Token: 0x060007B1 RID: 1969 RVA: 0x00030778 File Offset: 0x0002E978
		internal static void Render(RenderGraph graph, UniversalRenderer renderer, RTClearFlags clearFlags, Color clearColor)
		{
			ClearTargetsPass.PassData passData;
			using (RenderGraphBuilder renderGraphBuilder = graph.AddRenderPass<ClearTargetsPass.PassData>("Clear Targets Pass", out passData, ClearTargetsPass.s_ClearProfilingSampler))
			{
				passData.color = renderGraphBuilder.UseColorBuffer(in UniversalRenderer.m_ActiveRenderGraphColor, 0);
				passData.depth = renderGraphBuilder.UseDepthBuffer(in UniversalRenderer.m_ActiveRenderGraphDepth, DepthAccess.Write);
				passData.clearFlags = clearFlags;
				passData.clearColor = clearColor;
				renderGraphBuilder.AllowPassCulling(false);
				renderGraphBuilder.SetRenderFunc<ClearTargetsPass.PassData>(delegate(ClearTargetsPass.PassData data, RenderGraphContext context)
				{
					context.cmd.ClearRenderTarget(data.clearFlags, data.clearColor, 1f, 0U);
				});
			}
		}

		// Token: 0x04000654 RID: 1620
		private static ProfilingSampler s_ClearProfilingSampler = new ProfilingSampler("Clear Targets");

		// Token: 0x020001C6 RID: 454
		private class PassData
		{
			// Token: 0x04000BD6 RID: 3030
			internal TextureHandle color;

			// Token: 0x04000BD7 RID: 3031
			internal TextureHandle depth;

			// Token: 0x04000BD8 RID: 3032
			internal RTClearFlags clearFlags;

			// Token: 0x04000BD9 RID: 3033
			internal Color clearColor;
		}
	}
}
