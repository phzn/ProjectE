using System;
using UnityEngine.Experimental.Rendering.RenderGraphModule;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000B0 RID: 176
	internal class InvokeOnRenderObjectCallbackPass : ScriptableRenderPass
	{
		// Token: 0x060005B2 RID: 1458 RVA: 0x00020EEE File Offset: 0x0001F0EE
		public InvokeOnRenderObjectCallbackPass(RenderPassEvent evt)
		{
			base.profilingSampler = new ProfilingSampler("InvokeOnRenderObjectCallbackPass");
			base.renderPassEvent = evt;
			base.useNativeRenderPass = false;
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x00020F14 File Offset: 0x0001F114
		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			context.InvokeOnRenderObjectCallback();
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x00020F20 File Offset: 0x0001F120
		internal void Render(RenderGraph renderGraph, TextureHandle colorTarget, TextureHandle depthTarget, ref RenderingData renderingData)
		{
			InvokeOnRenderObjectCallbackPass.PassData passData;
			using (RenderGraphBuilder renderGraphBuilder = renderGraph.AddRenderPass<InvokeOnRenderObjectCallbackPass.PassData>("OnRenderObject Callback Pass", out passData, base.profilingSampler))
			{
				passData.colorTarget = renderGraphBuilder.UseColorBuffer(in colorTarget, 0);
				passData.depthTarget = renderGraphBuilder.UseDepthBuffer(in depthTarget, DepthAccess.ReadWrite);
				renderGraphBuilder.AllowPassCulling(false);
				renderGraphBuilder.SetRenderFunc<InvokeOnRenderObjectCallbackPass.PassData>(delegate(InvokeOnRenderObjectCallbackPass.PassData data, RenderGraphContext context)
				{
					context.renderContext.InvokeOnRenderObjectCallback();
				});
			}
		}

		// Token: 0x020001A0 RID: 416
		private class PassData
		{
			// Token: 0x04000AC5 RID: 2757
			internal TextureHandle colorTarget;

			// Token: 0x04000AC6 RID: 2758
			internal TextureHandle depthTarget;
		}
	}
}
