using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000065 RID: 101
	internal class DecalPreviewPass : ScriptableRenderPass
	{
		// Token: 0x060003B5 RID: 949 RVA: 0x0001602C File Offset: 0x0001422C
		public DecalPreviewPass()
		{
			base.renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
			base.ConfigureInput(ScriptableRenderPassInput.Depth);
			this.m_ProfilingSampler = new ProfilingSampler("Decal Preview Render");
			this.m_FilteringSettings = new FilteringSettings(new RenderQueueRange?(RenderQueueRange.opaque), -1, uint.MaxValue, 0);
			this.m_ShaderTagIdList = new List<ShaderTagId>();
			this.m_ShaderTagIdList.Add(new ShaderTagId("DecalScreenSpaceMesh"));
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x0001609C File Offset: 0x0001429C
		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			SortingCriteria defaultOpaqueSortFlags = renderingData.cameraData.defaultOpaqueSortFlags;
			DrawingSettings drawingSettings = base.CreateDrawingSettings(this.m_ShaderTagIdList, ref renderingData, defaultOpaqueSortFlags);
			CommandBuffer commandBuffer = renderingData.commandBuffer;
			using (new ProfilingScope(commandBuffer, this.m_ProfilingSampler))
			{
				context.ExecuteCommandBuffer(commandBuffer);
				commandBuffer.Clear();
				context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref this.m_FilteringSettings);
			}
		}

		// Token: 0x040002BF RID: 703
		private FilteringSettings m_FilteringSettings;

		// Token: 0x040002C0 RID: 704
		private List<ShaderTagId> m_ShaderTagIdList;

		// Token: 0x040002C1 RID: 705
		private ProfilingSampler m_ProfilingSampler;
	}
}
