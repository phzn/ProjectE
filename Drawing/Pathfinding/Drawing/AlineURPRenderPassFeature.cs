using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Pathfinding.Drawing
{
	// Token: 0x02000006 RID: 6
	public class AlineURPRenderPassFeature : ScriptableRendererFeature
	{
		// Token: 0x06000005 RID: 5 RVA: 0x000020C8 File Offset: 0x000002C8
		public override void Create()
		{
			this.m_ScriptablePass = new AlineURPRenderPassFeature.AlineURPRenderPass();
			this.m_ScriptablePass.renderPassEvent = (RenderPassEvent)549;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020E5 File Offset: 0x000002E5
		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			this.AddRenderPasses(renderer);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000020EE File Offset: 0x000002EE
		public void AddRenderPasses(ScriptableRenderer renderer)
		{
			renderer.EnqueuePass(this.m_ScriptablePass);
		}

		// Token: 0x04000006 RID: 6
		private AlineURPRenderPassFeature.AlineURPRenderPass m_ScriptablePass;

		// Token: 0x02000007 RID: 7
		public class AlineURPRenderPass : ScriptableRenderPass
		{
			// Token: 0x06000009 RID: 9 RVA: 0x00002104 File Offset: 0x00000304
			public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
			{
			}

			// Token: 0x0600000A RID: 10 RVA: 0x00002106 File Offset: 0x00000306
			public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
			{
				DrawingManager.instance.ExecuteCustomRenderPass(context, renderingData.cameraData.camera);
			}

			// Token: 0x0600000B RID: 11 RVA: 0x0000211E File Offset: 0x0000031E
			public AlineURPRenderPass()
			{
				base.profilingSampler = new ProfilingSampler("ALINE");
			}

			// Token: 0x0600000C RID: 12 RVA: 0x00002104 File Offset: 0x00000304
			public override void FrameCleanup(CommandBuffer cmd)
			{
			}
		}
	}
}
