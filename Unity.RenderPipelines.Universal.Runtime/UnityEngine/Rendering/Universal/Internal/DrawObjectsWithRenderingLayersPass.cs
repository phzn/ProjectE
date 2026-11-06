using System;

namespace UnityEngine.Rendering.Universal.Internal
{
	// Token: 0x02000122 RID: 290
	internal class DrawObjectsWithRenderingLayersPass : DrawObjectsPass
	{
		// Token: 0x06000999 RID: 2457 RVA: 0x0003FC71 File Offset: 0x0003DE71
		public DrawObjectsWithRenderingLayersPass(URPProfileId profilerTag, bool opaque, RenderPassEvent evt, RenderQueueRange renderQueueRange, LayerMask layerMask, StencilState stencilState, int stencilReference)
			: base(profilerTag, opaque, evt, renderQueueRange, layerMask, stencilState, stencilReference)
		{
			this.m_ColorTargetIndentifiers = new RTHandle[2];
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x0003FC90 File Offset: 0x0003DE90
		public void Setup(RTHandle colorAttachment, RTHandle renderingLayersTexture, RTHandle depthAttachment)
		{
			if (colorAttachment == null)
			{
				throw new ArgumentException("Color attachment can not be null", "colorAttachment");
			}
			if (renderingLayersTexture == null)
			{
				throw new ArgumentException("Rendering layers attachment can not be null", "renderingLayersTexture");
			}
			if (depthAttachment == null)
			{
				throw new ArgumentException("Depth attachment can not be null", "depthAttachment");
			}
			this.m_ColorTargetIndentifiers[0] = colorAttachment;
			this.m_ColorTargetIndentifiers[1] = renderingLayersTexture;
			this.m_DepthTargetIndentifiers = depthAttachment;
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x0003FCEF File Offset: 0x0003DEEF
		public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
		{
			base.ConfigureTarget(this.m_ColorTargetIndentifiers, this.m_DepthTargetIndentifiers);
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x0003FD03 File Offset: 0x0003DF03
		protected override void OnExecute(CommandBuffer cmd)
		{
			CoreUtils.SetKeyword(cmd, "_WRITE_RENDERING_LAYERS", true);
		}

		// Token: 0x04000896 RID: 2198
		private RTHandle[] m_ColorTargetIndentifiers;

		// Token: 0x04000897 RID: 2199
		private RTHandle m_DepthTargetIndentifiers;
	}
}
