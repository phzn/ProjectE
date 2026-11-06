using System;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal.Internal;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000079 RID: 121
	internal class DecalGBufferRenderPass : ScriptableRenderPass
	{
		// Token: 0x06000444 RID: 1092 RVA: 0x00018EA4 File Offset: 0x000170A4
		public DecalGBufferRenderPass(DecalScreenSpaceSettings settings, DecalDrawGBufferSystem drawSystem, bool decalLayers)
		{
			base.renderPassEvent = RenderPassEvent.AfterRenderingGbuffer;
			this.m_DrawSystem = drawSystem;
			this.m_Settings = settings;
			this.m_ProfilingSampler = new ProfilingSampler("Decal GBuffer Render");
			this.m_FilteringSettings = new FilteringSettings(new RenderQueueRange?(RenderQueueRange.opaque), -1, uint.MaxValue, 0);
			this.m_DecalLayers = decalLayers;
			this.m_ShaderTagIdList = new List<ShaderTagId>();
			if (drawSystem == null)
			{
				this.m_ShaderTagIdList.Add(new ShaderTagId("DecalGBufferProjector"));
			}
			else
			{
				this.m_ShaderTagIdList.Add(new ShaderTagId("DecalGBufferMesh"));
			}
			this.m_GbufferAttachments = new RTHandle[4];
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x00018F45 File Offset: 0x00017145
		internal void Setup(DeferredLights deferredLights)
		{
			this.m_DeferredLights = deferredLights;
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x00018F50 File Offset: 0x00017150
		public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
		{
			if (this.m_DeferredLights.UseRenderPass)
			{
				this.m_GbufferAttachments[0] = this.m_DeferredLights.GbufferAttachments[0];
				this.m_GbufferAttachments[1] = this.m_DeferredLights.GbufferAttachments[1];
				this.m_GbufferAttachments[2] = this.m_DeferredLights.GbufferAttachments[2];
				this.m_GbufferAttachments[3] = this.m_DeferredLights.GbufferAttachments[3];
				if (this.m_DecalLayers)
				{
					RTHandle[] array = new RTHandle[]
					{
						this.m_DeferredLights.GbufferAttachments[this.m_DeferredLights.GbufferDepthIndex],
						this.m_DeferredLights.GbufferAttachments[this.m_DeferredLights.GBufferRenderingLayers]
					};
					bool[] array2 = new bool[2];
					array2[0] = true;
					bool[] array3 = array2;
					base.ConfigureInputAttachments(array, array3);
				}
				else
				{
					RTHandle[] array4 = new RTHandle[] { this.m_DeferredLights.GbufferAttachments[this.m_DeferredLights.GbufferDepthIndex] };
					bool[] array5 = new bool[] { true };
					base.ConfigureInputAttachments(array4, array5);
				}
			}
			else
			{
				this.m_GbufferAttachments[0] = this.m_DeferredLights.GbufferAttachments[0];
				this.m_GbufferAttachments[1] = this.m_DeferredLights.GbufferAttachments[1];
				this.m_GbufferAttachments[2] = this.m_DeferredLights.GbufferAttachments[2];
				this.m_GbufferAttachments[3] = this.m_DeferredLights.GbufferAttachments[3];
			}
			base.ConfigureTarget(this.m_GbufferAttachments, this.m_DeferredLights.DepthAttachmentHandle);
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x000190C0 File Offset: 0x000172C0
		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			SortingCriteria defaultOpaqueSortFlags = renderingData.cameraData.defaultOpaqueSortFlags;
			DrawingSettings drawingSettings = base.CreateDrawingSettings(this.m_ShaderTagIdList, ref renderingData, defaultOpaqueSortFlags);
			CommandBuffer commandBuffer = renderingData.commandBuffer;
			using (new ProfilingScope(commandBuffer, this.m_ProfilingSampler))
			{
				context.ExecuteCommandBuffer(commandBuffer);
				commandBuffer.Clear();
				NormalReconstruction.SetupProperties(commandBuffer, in renderingData.cameraData);
				CoreUtils.SetKeyword(commandBuffer, "_DECAL_NORMAL_BLEND_LOW", this.m_Settings.normalBlend == DecalNormalBlend.Low);
				CoreUtils.SetKeyword(commandBuffer, "_DECAL_NORMAL_BLEND_MEDIUM", this.m_Settings.normalBlend == DecalNormalBlend.Medium);
				CoreUtils.SetKeyword(commandBuffer, "_DECAL_NORMAL_BLEND_HIGH", this.m_Settings.normalBlend == DecalNormalBlend.High);
				CoreUtils.SetKeyword(commandBuffer, "_DECAL_LAYERS", this.m_DecalLayers);
				context.ExecuteCommandBuffer(commandBuffer);
				commandBuffer.Clear();
				DecalDrawGBufferSystem drawSystem = this.m_DrawSystem;
				if (drawSystem != null)
				{
					drawSystem.Execute(commandBuffer);
				}
				context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref this.m_FilteringSettings);
			}
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x000191C8 File Offset: 0x000173C8
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

		// Token: 0x04000341 RID: 833
		private FilteringSettings m_FilteringSettings;

		// Token: 0x04000342 RID: 834
		private ProfilingSampler m_ProfilingSampler;

		// Token: 0x04000343 RID: 835
		private List<ShaderTagId> m_ShaderTagIdList;

		// Token: 0x04000344 RID: 836
		private DecalDrawGBufferSystem m_DrawSystem;

		// Token: 0x04000345 RID: 837
		private DecalScreenSpaceSettings m_Settings;

		// Token: 0x04000346 RID: 838
		private DeferredLights m_DeferredLights;

		// Token: 0x04000347 RID: 839
		private RTHandle[] m_GbufferAttachments;

		// Token: 0x04000348 RID: 840
		private bool m_DecalLayers;
	}
}
