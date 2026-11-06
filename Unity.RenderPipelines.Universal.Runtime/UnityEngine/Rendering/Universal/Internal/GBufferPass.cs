using System;
using Unity.Collections;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Experimental.Rendering.RenderGraphModule;

namespace UnityEngine.Rendering.Universal.Internal
{
	// Token: 0x02000125 RID: 293
	internal class GBufferPass : ScriptableRenderPass
	{
		// Token: 0x060009B0 RID: 2480 RVA: 0x00040970 File Offset: 0x0003EB70
		public GBufferPass(RenderPassEvent evt, RenderQueueRange renderQueueRange, LayerMask layerMask, StencilState stencilState, int stencilReference, DeferredLights deferredLights)
		{
			base.profilingSampler = new ProfilingSampler("GBufferPass");
			base.renderPassEvent = evt;
			this.m_PassData = new GBufferPass.PassData();
			this.m_DeferredLights = deferredLights;
			this.m_FilteringSettings = new FilteringSettings(new RenderQueueRange?(renderQueueRange), layerMask, uint.MaxValue, 0);
			this.m_RenderStateBlock = new RenderStateBlock(RenderStateMask.Nothing);
			this.m_RenderStateBlock.stencilState = stencilState;
			this.m_RenderStateBlock.stencilReference = stencilReference;
			this.m_RenderStateBlock.mask = RenderStateMask.Stencil;
			if (GBufferPass.s_ShaderTagValues == null)
			{
				GBufferPass.s_ShaderTagValues = new ShaderTagId[5];
				GBufferPass.s_ShaderTagValues[0] = GBufferPass.s_ShaderTagLit;
				GBufferPass.s_ShaderTagValues[1] = GBufferPass.s_ShaderTagSimpleLit;
				GBufferPass.s_ShaderTagValues[2] = GBufferPass.s_ShaderTagUnlit;
				GBufferPass.s_ShaderTagValues[3] = GBufferPass.s_ShaderTagComplexLit;
				GBufferPass.s_ShaderTagValues[4] = default(ShaderTagId);
			}
			if (GBufferPass.s_RenderStateBlocks == null)
			{
				GBufferPass.s_RenderStateBlocks = new RenderStateBlock[5];
				GBufferPass.s_RenderStateBlocks[0] = DeferredLights.OverwriteStencil(this.m_RenderStateBlock, 96, 32);
				GBufferPass.s_RenderStateBlocks[1] = DeferredLights.OverwriteStencil(this.m_RenderStateBlock, 96, 64);
				GBufferPass.s_RenderStateBlocks[2] = DeferredLights.OverwriteStencil(this.m_RenderStateBlock, 96, 0);
				GBufferPass.s_RenderStateBlocks[3] = DeferredLights.OverwriteStencil(this.m_RenderStateBlock, 96, 0);
				GBufferPass.s_RenderStateBlocks[4] = GBufferPass.s_RenderStateBlocks[0];
			}
		}

		// Token: 0x060009B1 RID: 2481 RVA: 0x00040AFC File Offset: 0x0003ECFC
		public void Dispose()
		{
			this.m_DeferredLights.ReleaseGbufferResources();
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x00040B0C File Offset: 0x0003ED0C
		public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
		{
			RTHandle[] gbufferAttachments = this.m_DeferredLights.GbufferAttachments;
			if (cmd != null)
			{
				bool flag = true;
				if (this.m_DeferredLights.UseRenderPass && this.m_DeferredLights.DepthCopyTexture != null && this.m_DeferredLights.DepthCopyTexture.rt != null)
				{
					this.m_DeferredLights.GbufferAttachments[this.m_DeferredLights.GbufferDepthIndex] = this.m_DeferredLights.DepthCopyTexture;
					flag = false;
				}
				for (int i = 0; i < gbufferAttachments.Length; i++)
				{
					if (i != this.m_DeferredLights.GBufferLightingIndex && (i != this.m_DeferredLights.GBufferNormalSmoothnessIndex || !this.m_DeferredLights.HasNormalPrepass) && (i != this.m_DeferredLights.GbufferDepthIndex || flag) && (!this.m_DeferredLights.UseRenderPass || i == this.m_DeferredLights.GBufferRenderingLayers || i == this.m_DeferredLights.GbufferDepthIndex || this.m_DeferredLights.HasDepthPrepass))
					{
						this.m_DeferredLights.ReAllocateGBufferIfNeeded(cameraTextureDescriptor, i);
						cmd.SetGlobalTexture(this.m_DeferredLights.GbufferAttachments[i].name, this.m_DeferredLights.GbufferAttachments[i].nameID);
					}
				}
			}
			if (this.m_DeferredLights.UseRenderPass)
			{
				this.m_DeferredLights.UpdateDeferredInputAttachments();
			}
			base.ConfigureTarget(this.m_DeferredLights.GbufferAttachments, this.m_DeferredLights.DepthAttachment, this.m_DeferredLights.GbufferFormats);
			base.ConfigureClear(ClearFlag.None, Color.black);
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x00040C8C File Offset: 0x0003EE8C
		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			CommandBuffer commandBuffer = renderingData.commandBuffer;
			this.m_PassData.filteringSettings = this.m_FilteringSettings;
			using (new ProfilingScope(commandBuffer, this.m_ProfilingSampler))
			{
				this.m_PassData.deferredLights = this.m_DeferredLights;
				ShaderTagId shaderTagId = GBufferPass.s_ShaderTagUniversalGBuffer;
				this.m_PassData.drawingSettings = base.CreateDrawingSettings(shaderTagId, ref renderingData, renderingData.cameraData.defaultOpaqueSortFlags);
				GBufferPass.ExecutePass(context, this.m_PassData, ref renderingData, false);
				if (!this.m_DeferredLights.UseRenderPass)
				{
					renderingData.commandBuffer.SetGlobalTexture(GBufferPass.s_CameraNormalsTextureID, this.m_DeferredLights.GbufferAttachments[this.m_DeferredLights.GBufferNormalSmoothnessIndex]);
				}
			}
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x00040D5C File Offset: 0x0003EF5C
		private static void ExecutePass(ScriptableRenderContext context, GBufferPass.PassData data, ref RenderingData renderingData, bool useRenderGraph = false)
		{
			bool flag = data.deferredLights.UseRenderingLayers && !data.deferredLights.HasRenderingLayerPrepass;
			if (flag)
			{
				CoreUtils.SetKeyword(renderingData.commandBuffer, "_WRITE_RENDERING_LAYERS", true);
			}
			context.ExecuteCommandBuffer(renderingData.commandBuffer);
			renderingData.commandBuffer.Clear();
			if (data.deferredLights.IsOverlay)
			{
				data.deferredLights.ClearStencilPartial(renderingData.commandBuffer);
				context.ExecuteCommandBuffer(renderingData.commandBuffer);
				renderingData.commandBuffer.Clear();
			}
			NativeArray<ShaderTagId> nativeArray = new NativeArray<ShaderTagId>(GBufferPass.s_ShaderTagValues, Allocator.Temp);
			NativeArray<RenderStateBlock> nativeArray2 = new NativeArray<RenderStateBlock>(GBufferPass.s_RenderStateBlocks, Allocator.Temp);
			context.DrawRenderers(renderingData.cullResults, ref data.drawingSettings, ref data.filteringSettings, GBufferPass.s_ShaderTagUniversalMaterialType, false, nativeArray, nativeArray2);
			nativeArray.Dispose();
			nativeArray2.Dispose();
			if (!data.deferredLights.UseRenderPass)
			{
				renderingData.commandBuffer.SetGlobalTexture(GBufferPass.s_CameraNormalsTextureID, data.deferredLights.GbufferAttachments[data.deferredLights.GBufferNormalSmoothnessIndex]);
			}
			if (flag)
			{
				CoreUtils.SetKeyword(renderingData.commandBuffer, "_WRITE_RENDERING_LAYERS", false);
				context.ExecuteCommandBuffer(renderingData.commandBuffer);
				renderingData.commandBuffer.Clear();
			}
		}

		// Token: 0x060009B5 RID: 2485 RVA: 0x00040E98 File Offset: 0x0003F098
		internal void Render(RenderGraph renderGraph, TextureHandle cameraColor, TextureHandle cameraDepth, ref RenderingData renderingData, ref UniversalRenderer.RenderGraphFrameResources frameResources)
		{
			GBufferPass.PassData passData;
			using (RenderGraphBuilder renderGraphBuilder = renderGraph.AddRenderPass<GBufferPass.PassData>("GBuffer Pass", out passData, this.m_ProfilingSampler))
			{
				passData.gbuffer = (frameResources.gbuffer = this.m_DeferredLights.GbufferTextureHandles);
				for (int i = 0; i < this.m_DeferredLights.GBufferSliceCount; i++)
				{
					RenderTextureDescriptor cameraTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
					cameraTargetDescriptor.depthBufferBits = 0;
					cameraTargetDescriptor.stencilFormat = GraphicsFormat.None;
					if (i != this.m_DeferredLights.GBufferLightingIndex)
					{
						cameraTargetDescriptor.graphicsFormat = this.m_DeferredLights.GetGBufferFormat(i);
						frameResources.gbuffer[i] = UniversalRenderer.CreateRenderGraphTexture(renderGraph, cameraTargetDescriptor, DeferredLights.k_GBufferNames[i], true, FilterMode.Point, TextureWrapMode.Clamp);
					}
					else
					{
						frameResources.gbuffer[i] = cameraColor;
					}
					passData.gbuffer[i] = renderGraphBuilder.UseColorBuffer(in frameResources.gbuffer[i], i);
				}
				passData.deferredLights = this.m_DeferredLights;
				passData.depth = renderGraphBuilder.UseDepthBuffer(in cameraDepth, DepthAccess.Write);
				passData.renderingData = renderingData;
				renderGraphBuilder.AllowPassCulling(false);
				passData.filteringSettings = this.m_FilteringSettings;
				ShaderTagId shaderTagId = GBufferPass.s_ShaderTagUniversalGBuffer;
				passData.drawingSettings = base.CreateDrawingSettings(shaderTagId, ref passData.renderingData, renderingData.cameraData.defaultOpaqueSortFlags);
				renderGraphBuilder.SetRenderFunc<GBufferPass.PassData>(delegate(GBufferPass.PassData data, RenderGraphContext context)
				{
					GBufferPass.ExecutePass(context.renderContext, data, ref data.renderingData, true);
				});
			}
			GBufferPass.PassData passData2;
			using (RenderGraphBuilder renderGraphBuilder2 = renderGraph.AddRenderPass<GBufferPass.PassData>("Set GBuffer Globals", out passData2, this.m_ProfilingSampler))
			{
				passData2.gbuffer = (frameResources.gbuffer = this.m_DeferredLights.GbufferTextureHandles);
				for (int j = 0; j < this.m_DeferredLights.GBufferSliceCount; j++)
				{
					passData2.gbuffer[j] = renderGraphBuilder2.UseColorBuffer(in frameResources.gbuffer[j], j);
				}
				passData2.depth = renderGraphBuilder2.UseDepthBuffer(in cameraDepth, DepthAccess.Read);
				passData2.renderingData = renderingData;
				passData2.deferredLights = this.m_DeferredLights;
				renderGraphBuilder2.AllowPassCulling(false);
				renderGraphBuilder2.SetRenderFunc<GBufferPass.PassData>(delegate(GBufferPass.PassData data, RenderGraphContext context)
				{
					for (int k = 0; k < data.gbuffer.Length; k++)
					{
						if (k != data.deferredLights.GBufferLightingIndex)
						{
							data.renderingData.commandBuffer.SetGlobalTexture(DeferredLights.k_GBufferNames[k], data.gbuffer[k]);
						}
					}
				});
			}
		}

		// Token: 0x040008A6 RID: 2214
		private static readonly int s_CameraNormalsTextureID = Shader.PropertyToID("_CameraNormalsTexture");

		// Token: 0x040008A7 RID: 2215
		private static ShaderTagId s_ShaderTagLit = new ShaderTagId("Lit");

		// Token: 0x040008A8 RID: 2216
		private static ShaderTagId s_ShaderTagSimpleLit = new ShaderTagId("SimpleLit");

		// Token: 0x040008A9 RID: 2217
		private static ShaderTagId s_ShaderTagUnlit = new ShaderTagId("Unlit");

		// Token: 0x040008AA RID: 2218
		private static ShaderTagId s_ShaderTagComplexLit = new ShaderTagId("ComplexLit");

		// Token: 0x040008AB RID: 2219
		private static ShaderTagId s_ShaderTagUniversalGBuffer = new ShaderTagId("UniversalGBuffer");

		// Token: 0x040008AC RID: 2220
		private static ShaderTagId s_ShaderTagUniversalMaterialType = new ShaderTagId("UniversalMaterialType");

		// Token: 0x040008AD RID: 2221
		private ProfilingSampler m_ProfilingSampler = new ProfilingSampler("Render GBuffer");

		// Token: 0x040008AE RID: 2222
		private DeferredLights m_DeferredLights;

		// Token: 0x040008AF RID: 2223
		private static ShaderTagId[] s_ShaderTagValues;

		// Token: 0x040008B0 RID: 2224
		private static RenderStateBlock[] s_RenderStateBlocks;

		// Token: 0x040008B1 RID: 2225
		private FilteringSettings m_FilteringSettings;

		// Token: 0x040008B2 RID: 2226
		private RenderStateBlock m_RenderStateBlock;

		// Token: 0x040008B3 RID: 2227
		private GBufferPass.PassData m_PassData;

		// Token: 0x020001F3 RID: 499
		private class PassData
		{
			// Token: 0x04000CD2 RID: 3282
			internal TextureHandle[] gbuffer;

			// Token: 0x04000CD3 RID: 3283
			internal TextureHandle depth;

			// Token: 0x04000CD4 RID: 3284
			internal RenderingData renderingData;

			// Token: 0x04000CD5 RID: 3285
			internal DeferredLights deferredLights;

			// Token: 0x04000CD6 RID: 3286
			internal FilteringSettings filteringSettings;

			// Token: 0x04000CD7 RID: 3287
			internal DrawingSettings drawingSettings;
		}
	}
}
