using System;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Experimental.Rendering.RenderGraphModule;

namespace UnityEngine.Rendering.Universal.Internal
{
	// Token: 0x02000121 RID: 289
	public class DepthOnlyPass : ScriptableRenderPass
	{
		// Token: 0x1700023C RID: 572
		// (get) Token: 0x0600098E RID: 2446 RVA: 0x0003F991 File Offset: 0x0003DB91
		// (set) Token: 0x0600098F RID: 2447 RVA: 0x0003F999 File Offset: 0x0003DB99
		private RTHandle destination { get; set; }

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000990 RID: 2448 RVA: 0x0003F9A2 File Offset: 0x0003DBA2
		// (set) Token: 0x06000991 RID: 2449 RVA: 0x0003F9AA File Offset: 0x0003DBAA
		internal ShaderTagId shaderTagId { get; set; } = DepthOnlyPass.k_ShaderTagId;

		// Token: 0x06000992 RID: 2450 RVA: 0x0003F9B4 File Offset: 0x0003DBB4
		public DepthOnlyPass(RenderPassEvent evt, RenderQueueRange renderQueueRange, LayerMask layerMask)
		{
			base.profilingSampler = new ProfilingSampler("DepthOnlyPass");
			this.m_PassData = new DepthOnlyPass.PassData();
			this.m_FilteringSettings = new FilteringSettings(new RenderQueueRange?(renderQueueRange), layerMask, uint.MaxValue, 0);
			base.renderPassEvent = evt;
			base.useNativeRenderPass = false;
			this.shaderTagId = DepthOnlyPass.k_ShaderTagId;
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x0003FA1F File Offset: 0x0003DC1F
		public void Setup(RenderTextureDescriptor baseDescriptor, RTHandle depthAttachmentHandle)
		{
			this.destination = depthAttachmentHandle;
			this.depthStencilFormat = baseDescriptor.depthStencilFormat;
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x0003FA38 File Offset: 0x0003DC38
		public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
		{
			if (renderingData.cameraData.renderer.useDepthPriming && (renderingData.cameraData.renderType == CameraRenderType.Base || renderingData.cameraData.clearDepth))
			{
				base.ConfigureTarget(renderingData.cameraData.renderer.cameraDepthTargetHandle);
				base.ConfigureClear(ClearFlag.Depth, Color.black);
				return;
			}
			base.useNativeRenderPass = true;
			base.ConfigureTarget(this.destination);
			base.ConfigureClear(ClearFlag.All, Color.black);
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x0003FAB4 File Offset: 0x0003DCB4
		private static void ExecutePass(ScriptableRenderContext context, DepthOnlyPass.PassData passData, ref RenderingData renderingData)
		{
			CommandBuffer commandBuffer = renderingData.commandBuffer;
			ShaderTagId shaderTagId = passData.shaderTagId;
			FilteringSettings filteringSettings = passData.filteringSettings;
			using (new ProfilingScope(commandBuffer, ProfilingSampler.Get<URPProfileId>(URPProfileId.DepthPrepass)))
			{
				context.ExecuteCommandBuffer(commandBuffer);
				commandBuffer.Clear();
				SortingCriteria defaultOpaqueSortFlags = renderingData.cameraData.defaultOpaqueSortFlags;
				DrawingSettings drawingSettings = RenderingUtils.CreateDrawingSettings(shaderTagId, ref renderingData, defaultOpaqueSortFlags);
				drawingSettings.perObjectData = PerObjectData.None;
				context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref filteringSettings);
			}
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x0003FB44 File Offset: 0x0003DD44
		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			this.m_PassData.shaderTagId = this.shaderTagId;
			this.m_PassData.filteringSettings = this.m_FilteringSettings;
			DepthOnlyPass.ExecutePass(context, this.m_PassData, ref renderingData);
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x0003FB78 File Offset: 0x0003DD78
		internal void Render(RenderGraph renderGraph, out TextureHandle cameraDepthTexture, ref RenderingData renderingData)
		{
			DepthOnlyPass.PassData passData;
			using (RenderGraphBuilder renderGraphBuilder = renderGraph.AddRenderPass<DepthOnlyPass.PassData>("DepthOnly Prepass", out passData, base.profilingSampler))
			{
				RenderTextureDescriptor cameraTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
				cameraTargetDescriptor.graphicsFormat = GraphicsFormat.None;
				cameraTargetDescriptor.depthStencilFormat = GraphicsFormat.D32_SFloat_S8_UInt;
				cameraTargetDescriptor.depthBufferBits = 32;
				cameraTargetDescriptor.msaaSamples = 1;
				cameraDepthTexture = UniversalRenderer.CreateRenderGraphTexture(renderGraph, cameraTargetDescriptor, "_CameraDepthTexture", true, FilterMode.Point, TextureWrapMode.Clamp);
				passData.cameraDepthTexture = renderGraphBuilder.UseDepthBuffer(in cameraDepthTexture, DepthAccess.Write);
				passData.renderingData = renderingData;
				passData.shaderTagId = this.shaderTagId;
				passData.filteringSettings = this.m_FilteringSettings;
				renderGraphBuilder.AllowPassCulling(false);
				renderGraphBuilder.SetRenderFunc<DepthOnlyPass.PassData>(delegate(DepthOnlyPass.PassData data, RenderGraphContext context)
				{
					DepthOnlyPass.ExecutePass(context.renderContext, data, ref data.renderingData);
				});
			}
		}

		// Token: 0x04000890 RID: 2192
		private static readonly ShaderTagId k_ShaderTagId = new ShaderTagId("DepthOnly");

		// Token: 0x04000892 RID: 2194
		private GraphicsFormat depthStencilFormat;

		// Token: 0x04000894 RID: 2196
		private DepthOnlyPass.PassData m_PassData;

		// Token: 0x04000895 RID: 2197
		private FilteringSettings m_FilteringSettings;

		// Token: 0x020001EA RID: 490
		private class PassData
		{
			// Token: 0x04000CAC RID: 3244
			internal TextureHandle cameraDepthTexture;

			// Token: 0x04000CAD RID: 3245
			internal RenderingData renderingData;

			// Token: 0x04000CAE RID: 3246
			internal ShaderTagId shaderTagId;

			// Token: 0x04000CAF RID: 3247
			internal FilteringSettings filteringSettings;
		}
	}
}
