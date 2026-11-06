using System;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Experimental.Rendering.RenderGraphModule;

namespace UnityEngine.Rendering.Universal.Internal
{
	// Token: 0x02000120 RID: 288
	public class DepthNormalOnlyPass : ScriptableRenderPass
	{
		// Token: 0x17000237 RID: 567
		// (get) Token: 0x0600097A RID: 2426 RVA: 0x0003F4F6 File Offset: 0x0003D6F6
		// (set) Token: 0x0600097B RID: 2427 RVA: 0x0003F4FE File Offset: 0x0003D6FE
		internal List<ShaderTagId> shaderTagIds { get; set; }

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x0600097C RID: 2428 RVA: 0x0003F507 File Offset: 0x0003D707
		// (set) Token: 0x0600097D RID: 2429 RVA: 0x0003F50F File Offset: 0x0003D70F
		private RTHandle depthHandle { get; set; }

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x0600097E RID: 2430 RVA: 0x0003F518 File Offset: 0x0003D718
		// (set) Token: 0x0600097F RID: 2431 RVA: 0x0003F520 File Offset: 0x0003D720
		private RTHandle normalHandle { get; set; }

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000980 RID: 2432 RVA: 0x0003F529 File Offset: 0x0003D729
		// (set) Token: 0x06000981 RID: 2433 RVA: 0x0003F531 File Offset: 0x0003D731
		private RTHandle renderingLayersHandle { get; set; }

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000982 RID: 2434 RVA: 0x0003F53A File Offset: 0x0003D73A
		// (set) Token: 0x06000983 RID: 2435 RVA: 0x0003F542 File Offset: 0x0003D742
		internal bool enableRenderingLayers { get; set; }

		// Token: 0x06000984 RID: 2436 RVA: 0x0003F54C File Offset: 0x0003D74C
		public DepthNormalOnlyPass(RenderPassEvent evt, RenderQueueRange renderQueueRange, LayerMask layerMask)
		{
			base.profilingSampler = new ProfilingSampler("DepthNormalOnlyPass");
			this.m_PassData = new DepthNormalOnlyPass.PassData();
			this.m_FilteringSettings = new FilteringSettings(new RenderQueueRange?(renderQueueRange), layerMask, uint.MaxValue, 0);
			base.renderPassEvent = evt;
			base.useNativeRenderPass = false;
			this.shaderTagIds = DepthNormalOnlyPass.k_DepthNormals;
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x0003F5AC File Offset: 0x0003D7AC
		public static GraphicsFormat GetGraphicsFormat()
		{
			if (RenderingUtils.SupportsGraphicsFormat(GraphicsFormat.R8G8B8A8_SNorm, FormatUsage.Render))
			{
				return GraphicsFormat.R8G8B8A8_SNorm;
			}
			if (RenderingUtils.SupportsGraphicsFormat(GraphicsFormat.R16G16B16A16_SFloat, FormatUsage.Render))
			{
				return GraphicsFormat.R16G16B16A16_SFloat;
			}
			return GraphicsFormat.R32G32B32A32_SFloat;
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x0003F5CA File Offset: 0x0003D7CA
		public void Setup(RTHandle depthHandle, RTHandle normalHandle)
		{
			this.depthHandle = depthHandle;
			this.normalHandle = normalHandle;
			this.enableRenderingLayers = false;
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x0003F5E1 File Offset: 0x0003D7E1
		public void Setup(RTHandle depthHandle, RTHandle normalHandle, RTHandle decalLayerHandle)
		{
			this.Setup(depthHandle, normalHandle);
			this.renderingLayersHandle = decalLayerHandle;
			this.enableRenderingLayers = true;
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x0003F5FC File Offset: 0x0003D7FC
		public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
		{
			RTHandle[] array;
			if (this.enableRenderingLayers)
			{
				DepthNormalOnlyPass.k_ColorAttachment2[0] = this.normalHandle;
				DepthNormalOnlyPass.k_ColorAttachment2[1] = this.renderingLayersHandle;
				array = DepthNormalOnlyPass.k_ColorAttachment2;
			}
			else
			{
				DepthNormalOnlyPass.k_ColorAttachment1[0] = this.normalHandle;
				array = DepthNormalOnlyPass.k_ColorAttachment1;
			}
			if (renderingData.cameraData.renderer.useDepthPriming && (renderingData.cameraData.renderType == CameraRenderType.Base || renderingData.cameraData.clearDepth))
			{
				base.ConfigureTarget(array, renderingData.cameraData.renderer.cameraDepthTargetHandle);
			}
			else
			{
				base.ConfigureTarget(array, this.depthHandle);
			}
			base.ConfigureClear(ClearFlag.All, Color.black);
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x0003F6A4 File Offset: 0x0003D8A4
		private static void ExecutePass(ScriptableRenderContext context, DepthNormalOnlyPass.PassData passData, ref RenderingData renderingData)
		{
			CommandBuffer commandBuffer = renderingData.commandBuffer;
			List<ShaderTagId> shaderTagIds = passData.shaderTagIds;
			FilteringSettings filteringSettings = passData.filteringSettings;
			using (new ProfilingScope(commandBuffer, ProfilingSampler.Get<URPProfileId>(URPProfileId.DepthNormalPrepass)))
			{
				context.ExecuteCommandBuffer(commandBuffer);
				commandBuffer.Clear();
				if (passData.enableRenderingLayers)
				{
					CoreUtils.SetKeyword(commandBuffer, "_WRITE_RENDERING_LAYERS", true);
					context.ExecuteCommandBuffer(commandBuffer);
					commandBuffer.Clear();
				}
				SortingCriteria defaultOpaqueSortFlags = renderingData.cameraData.defaultOpaqueSortFlags;
				DrawingSettings drawingSettings = RenderingUtils.CreateDrawingSettings(shaderTagIds, ref renderingData, defaultOpaqueSortFlags);
				drawingSettings.perObjectData = PerObjectData.None;
				context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref filteringSettings);
				if (passData.enableRenderingLayers)
				{
					CoreUtils.SetKeyword(commandBuffer, "_WRITE_RENDERING_LAYERS", false);
					context.ExecuteCommandBuffer(commandBuffer);
					commandBuffer.Clear();
				}
			}
		}

		// Token: 0x0600098A RID: 2442 RVA: 0x0003F778 File Offset: 0x0003D978
		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			this.m_PassData.shaderTagIds = this.shaderTagIds;
			this.m_PassData.filteringSettings = this.m_FilteringSettings;
			this.m_PassData.enableRenderingLayers = this.enableRenderingLayers;
			DepthNormalOnlyPass.ExecutePass(context, this.m_PassData, ref renderingData);
		}

		// Token: 0x0600098B RID: 2443 RVA: 0x0003F7C5 File Offset: 0x0003D9C5
		public override void OnCameraCleanup(CommandBuffer cmd)
		{
			if (cmd == null)
			{
				throw new ArgumentNullException("cmd");
			}
			this.normalHandle = null;
			this.depthHandle = null;
			this.renderingLayersHandle = null;
			this.shaderTagIds = DepthNormalOnlyPass.k_DepthNormals;
		}

		// Token: 0x0600098C RID: 2444 RVA: 0x0003F7F8 File Offset: 0x0003D9F8
		internal void Render(RenderGraph renderGraph, out TextureHandle cameraNormalsTexture, out TextureHandle cameraDepthTexture, ref RenderingData renderingData)
		{
			DepthNormalOnlyPass.PassData passData;
			using (RenderGraphBuilder renderGraphBuilder = renderGraph.AddRenderPass<DepthNormalOnlyPass.PassData>("DepthNormals Prepass", out passData, base.profilingSampler))
			{
				RenderTextureDescriptor cameraTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
				cameraTargetDescriptor.graphicsFormat = GraphicsFormat.None;
				cameraTargetDescriptor.depthStencilFormat = GraphicsFormat.D32_SFloat_S8_UInt;
				cameraTargetDescriptor.depthBufferBits = 32;
				cameraTargetDescriptor.msaaSamples = 1;
				cameraDepthTexture = UniversalRenderer.CreateRenderGraphTexture(renderGraph, cameraTargetDescriptor, "_CameraDepthTexture", true, FilterMode.Point, TextureWrapMode.Clamp);
				RenderTextureDescriptor cameraTargetDescriptor2 = renderingData.cameraData.cameraTargetDescriptor;
				cameraTargetDescriptor2.depthBufferBits = 0;
				cameraTargetDescriptor2.msaaSamples = 1;
				cameraTargetDescriptor2.graphicsFormat = DepthNormalOnlyPass.GetGraphicsFormat();
				cameraNormalsTexture = UniversalRenderer.CreateRenderGraphTexture(renderGraph, cameraTargetDescriptor2, "_CameraNormalsTexture", true, FilterMode.Point, TextureWrapMode.Clamp);
				passData.cameraNormalsTexture = renderGraphBuilder.UseColorBuffer(in cameraNormalsTexture, 0);
				passData.cameraDepthTexture = renderGraphBuilder.UseDepthBuffer(in cameraDepthTexture, DepthAccess.Write);
				passData.renderingData = renderingData;
				passData.shaderTagIds = this.shaderTagIds;
				passData.filteringSettings = this.m_FilteringSettings;
				passData.enableRenderingLayers = this.enableRenderingLayers;
				renderGraphBuilder.AllowPassCulling(false);
				renderGraphBuilder.SetRenderFunc<DepthNormalOnlyPass.PassData>(delegate(DepthNormalOnlyPass.PassData data, RenderGraphContext context)
				{
					DepthNormalOnlyPass.ExecutePass(context.renderContext, data, ref data.renderingData);
				});
			}
		}

		// Token: 0x0400088B RID: 2187
		private FilteringSettings m_FilteringSettings;

		// Token: 0x0400088C RID: 2188
		private DepthNormalOnlyPass.PassData m_PassData;

		// Token: 0x0400088D RID: 2189
		private static readonly List<ShaderTagId> k_DepthNormals = new List<ShaderTagId>
		{
			new ShaderTagId("DepthNormals"),
			new ShaderTagId("DepthNormalsOnly")
		};

		// Token: 0x0400088E RID: 2190
		private static readonly RTHandle[] k_ColorAttachment1 = new RTHandle[1];

		// Token: 0x0400088F RID: 2191
		private static readonly RTHandle[] k_ColorAttachment2 = new RTHandle[2];

		// Token: 0x020001E8 RID: 488
		private class PassData
		{
			// Token: 0x04000CA4 RID: 3236
			internal TextureHandle cameraDepthTexture;

			// Token: 0x04000CA5 RID: 3237
			internal TextureHandle cameraNormalsTexture;

			// Token: 0x04000CA6 RID: 3238
			internal RenderingData renderingData;

			// Token: 0x04000CA7 RID: 3239
			internal List<ShaderTagId> shaderTagIds;

			// Token: 0x04000CA8 RID: 3240
			internal FilteringSettings filteringSettings;

			// Token: 0x04000CA9 RID: 3241
			internal bool enableRenderingLayers;
		}
	}
}
