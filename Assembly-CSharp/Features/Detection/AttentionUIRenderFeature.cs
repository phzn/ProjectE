using System;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Features.Detection
{
	// Token: 0x0200123E RID: 4670
	public sealed class AttentionUIRenderFeature : ScriptableRendererFeature
	{
		// Token: 0x06009148 RID: 37192 RVA: 0x003B7885 File Offset: 0x003B5A85
		public override void Create()
		{
			this._pass = new AttentionUIRenderFeature.AttentionPass();
		}

		// Token: 0x06009149 RID: 37193 RVA: 0x003B7892 File Offset: 0x003B5A92
		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			renderer.EnqueuePass(this._pass);
		}

		// Token: 0x04009568 RID: 38248
		public const string ShaderPassName = "AttentionUI";

		// Token: 0x04009569 RID: 38249
		private AttentionUIRenderFeature.AttentionPass _pass;

		// Token: 0x0200123F RID: 4671
		private sealed class AttentionPass : ScriptableRenderPass
		{
			// Token: 0x0600914B RID: 37195 RVA: 0x003B78A8 File Offset: 0x003B5AA8
			public AttentionPass()
			{
				base.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
			}

			// Token: 0x0600914C RID: 37196 RVA: 0x003B78E4 File Offset: 0x003B5AE4
			public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
			{
				ScriptableRenderer renderer = renderingData.cameraData.renderer;
				base.ConfigureTarget(renderer.cameraColorTargetHandle, renderer.cameraDepthTargetHandle);
			}

			// Token: 0x0600914D RID: 37197 RVA: 0x003B7910 File Offset: 0x003B5B10
			public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
			{
				DrawingSettings drawingSettings = base.CreateDrawingSettings(this._tag, ref renderingData, SortingCriteria.CommonTransparent);
				context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref this._filter);
			}

			// Token: 0x0400956A RID: 38250
			private readonly ShaderTagId _tag = new ShaderTagId("AttentionUI");

			// Token: 0x0400956B RID: 38251
			private FilteringSettings _filter = new FilteringSettings(new RenderQueueRange?(RenderQueueRange.transparent), -1, uint.MaxValue, 0);
		}
	}
}
