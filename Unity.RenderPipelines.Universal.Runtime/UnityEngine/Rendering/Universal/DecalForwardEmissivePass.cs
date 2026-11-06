using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000063 RID: 99
	internal class DecalForwardEmissivePass : ScriptableRenderPass
	{
		// Token: 0x060003B0 RID: 944 RVA: 0x00015EA8 File Offset: 0x000140A8
		public DecalForwardEmissivePass(DecalDrawFowardEmissiveSystem drawSystem)
		{
			base.renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
			base.ConfigureInput(ScriptableRenderPassInput.Depth);
			this.m_DrawSystem = drawSystem;
			this.m_ProfilingSampler = new ProfilingSampler("Decal Forward Emissive Render");
			this.m_FilteringSettings = new FilteringSettings(new RenderQueueRange?(RenderQueueRange.opaque), -1, uint.MaxValue, 0);
			this.m_ShaderTagIdList = new List<ShaderTagId>();
			this.m_ShaderTagIdList.Add(new ShaderTagId("DecalMeshForwardEmissive"));
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x00015F1C File Offset: 0x0001411C
		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			SortingCriteria defaultOpaqueSortFlags = renderingData.cameraData.defaultOpaqueSortFlags;
			DrawingSettings drawingSettings = base.CreateDrawingSettings(this.m_ShaderTagIdList, ref renderingData, defaultOpaqueSortFlags);
			CommandBuffer commandBuffer = renderingData.commandBuffer;
			using (new ProfilingScope(commandBuffer, this.m_ProfilingSampler))
			{
				context.ExecuteCommandBuffer(commandBuffer);
				commandBuffer.Clear();
				this.m_DrawSystem.Execute(commandBuffer);
				context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref this.m_FilteringSettings);
			}
		}

		// Token: 0x040002BA RID: 698
		private FilteringSettings m_FilteringSettings;

		// Token: 0x040002BB RID: 699
		private ProfilingSampler m_ProfilingSampler;

		// Token: 0x040002BC RID: 700
		private List<ShaderTagId> m_ShaderTagIdList;

		// Token: 0x040002BD RID: 701
		private DecalDrawFowardEmissiveSystem m_DrawSystem;
	}
}
