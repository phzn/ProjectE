using System;
using UnityEngine.Rendering.Universal.Internal;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000B7 RID: 183
	internal class TransparentSettingsPass : ScriptableRenderPass
	{
		// Token: 0x060005EB RID: 1515 RVA: 0x000241BC File Offset: 0x000223BC
		public TransparentSettingsPass(RenderPassEvent evt, bool shadowReceiveSupported)
		{
			base.profilingSampler = new ProfilingSampler("TransparentSettingsPass");
			base.renderPassEvent = evt;
			this.m_shouldReceiveShadows = shadowReceiveSupported;
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x000241E2 File Offset: 0x000223E2
		public bool Setup()
		{
			return !this.m_shouldReceiveShadows;
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x000241ED File Offset: 0x000223ED
		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			TransparentSettingsPass.ExecutePass(renderingData.commandBuffer, this.m_shouldReceiveShadows);
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x00024200 File Offset: 0x00022400
		public static void ExecutePass(CommandBuffer cmd, bool shouldReceiveShadows)
		{
			using (new ProfilingScope(cmd, TransparentSettingsPass.m_ProfilingSampler))
			{
				MainLightShadowCasterPass.SetEmptyMainLightShadowParams(cmd);
				AdditionalLightsShadowCasterPass.SetEmptyAdditionalLightShadowParams(cmd, AdditionalLightsShadowCasterPass.s_EmptyAdditionalLightIndexToShadowParams);
			}
		}

		// Token: 0x040004B4 RID: 1204
		private bool m_shouldReceiveShadows;

		// Token: 0x040004B5 RID: 1205
		private const string m_ProfilerTag = "Transparent Settings Pass";

		// Token: 0x040004B6 RID: 1206
		private static readonly ProfilingSampler m_ProfilingSampler = new ProfilingSampler("Transparent Settings Pass");
	}
}
