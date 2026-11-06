using System;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal.Internal;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200007B RID: 123
	internal class DecalScreenSpaceRenderPass : ScriptableRenderPass
	{
		// Token: 0x0600044B RID: 1099 RVA: 0x00019220 File Offset: 0x00017420
		public DecalScreenSpaceRenderPass(DecalScreenSpaceSettings settings, DecalDrawScreenSpaceSystem drawSystem, bool decalLayers)
		{
			base.renderPassEvent = RenderPassEvent.AfterRenderingSkybox;
			ScriptableRenderPassInput scriptableRenderPassInput = ScriptableRenderPassInput.Depth;
			base.ConfigureInput(scriptableRenderPassInput);
			this.m_DrawSystem = drawSystem;
			this.m_Settings = settings;
			this.m_ProfilingSampler = new ProfilingSampler("Decal Screen Space Render");
			this.m_FilteringSettings = new FilteringSettings(new RenderQueueRange?(RenderQueueRange.opaque), -1, uint.MaxValue, 0);
			this.m_DecalLayers = decalLayers;
			this.m_ShaderTagIdList = new List<ShaderTagId>();
			if (this.m_DrawSystem == null)
			{
				this.m_ShaderTagIdList.Add(new ShaderTagId("DecalScreenSpaceProjector"));
				return;
			}
			this.m_ShaderTagIdList.Add(new ShaderTagId("DecalScreenSpaceMesh"));
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x000192C4 File Offset: 0x000174C4
		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			SortingCriteria sortingCriteria = SortingCriteria.None;
			DrawingSettings drawingSettings = base.CreateDrawingSettings(this.m_ShaderTagIdList, ref renderingData, sortingCriteria);
			CommandBuffer commandBuffer = renderingData.commandBuffer;
			using (new ProfilingScope(commandBuffer, this.m_ProfilingSampler))
			{
				context.ExecuteCommandBuffer(commandBuffer);
				commandBuffer.Clear();
				RenderingUtils.SetScaleBiasRt(commandBuffer, in renderingData);
				NormalReconstruction.SetupProperties(commandBuffer, in renderingData.cameraData);
				CoreUtils.SetKeyword(commandBuffer, "_DECAL_NORMAL_BLEND_LOW", this.m_Settings.normalBlend == DecalNormalBlend.Low);
				CoreUtils.SetKeyword(commandBuffer, "_DECAL_NORMAL_BLEND_MEDIUM", this.m_Settings.normalBlend == DecalNormalBlend.Medium);
				CoreUtils.SetKeyword(commandBuffer, "_DECAL_NORMAL_BLEND_HIGH", this.m_Settings.normalBlend == DecalNormalBlend.High);
				if (!DecalRendererFeature.isGLDevice)
				{
					CoreUtils.SetKeyword(commandBuffer, "_DECAL_LAYERS", this.m_DecalLayers);
				}
				context.ExecuteCommandBuffer(commandBuffer);
				commandBuffer.Clear();
				DecalDrawScreenSpaceSystem drawSystem = this.m_DrawSystem;
				if (drawSystem != null)
				{
					drawSystem.Execute(commandBuffer);
				}
				context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref this.m_FilteringSettings);
			}
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x000193D0 File Offset: 0x000175D0
		public override void OnCameraCleanup(CommandBuffer cmd)
		{
			if (cmd == null)
			{
				throw new ArgumentNullException("cmd");
			}
			CoreUtils.SetKeyword(cmd, "_DECAL_NORMAL_BLEND_LOW", false);
			CoreUtils.SetKeyword(cmd, "_DECAL_NORMAL_BLEND_MEDIUM", false);
			CoreUtils.SetKeyword(cmd, "_DECAL_NORMAL_BLEND_HIGH", false);
			CoreUtils.SetKeyword(cmd, "_DECAL_LAYERS", false);
		}

		// Token: 0x04000349 RID: 841
		private FilteringSettings m_FilteringSettings;

		// Token: 0x0400034A RID: 842
		private ProfilingSampler m_ProfilingSampler;

		// Token: 0x0400034B RID: 843
		private List<ShaderTagId> m_ShaderTagIdList;

		// Token: 0x0400034C RID: 844
		private DecalDrawScreenSpaceSystem m_DrawSystem;

		// Token: 0x0400034D RID: 845
		private DecalScreenSpaceSettings m_Settings;

		// Token: 0x0400034E RID: 846
		private bool m_DecalLayers;
	}
}
