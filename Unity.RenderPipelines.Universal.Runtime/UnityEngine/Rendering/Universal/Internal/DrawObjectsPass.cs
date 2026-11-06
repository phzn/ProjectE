using System;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.RenderGraphModule;

namespace UnityEngine.Rendering.Universal.Internal
{
	// Token: 0x02000123 RID: 291
	public class DrawObjectsPass : ScriptableRenderPass
	{
		// Token: 0x0600099D RID: 2461 RVA: 0x0003FD14 File Offset: 0x0003DF14
		public DrawObjectsPass(string profilerTag, ShaderTagId[] shaderTagIds, bool opaque, RenderPassEvent evt, RenderQueueRange renderQueueRange, LayerMask layerMask, StencilState stencilState, int stencilReference)
		{
			base.profilingSampler = new ProfilingSampler("DrawObjectsPass");
			this.m_PassData = new DrawObjectsPass.PassData();
			this.m_ProfilerTag = profilerTag;
			this.m_ProfilingSampler = new ProfilingSampler(profilerTag);
			foreach (ShaderTagId shaderTagId in shaderTagIds)
			{
				this.m_ShaderTagIdList.Add(shaderTagId);
			}
			base.renderPassEvent = evt;
			this.m_FilteringSettings = new FilteringSettings(new RenderQueueRange?(renderQueueRange), layerMask, uint.MaxValue, 0);
			this.m_RenderStateBlock = new RenderStateBlock(RenderStateMask.Nothing);
			this.m_IsOpaque = opaque;
			this.m_ShouldTransparentsReceiveShadows = false;
			this.m_IsActiveTargetBackBuffer = false;
			if (stencilState.enabled)
			{
				this.m_RenderStateBlock.stencilReference = stencilReference;
				this.m_RenderStateBlock.mask = RenderStateMask.Stencil;
				this.m_RenderStateBlock.stencilState = stencilState;
			}
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x0003FDF8 File Offset: 0x0003DFF8
		public DrawObjectsPass(string profilerTag, bool opaque, RenderPassEvent evt, RenderQueueRange renderQueueRange, LayerMask layerMask, StencilState stencilState, int stencilReference)
			: this(profilerTag, new ShaderTagId[]
			{
				new ShaderTagId("SRPDefaultUnlit"),
				new ShaderTagId("UniversalForward"),
				new ShaderTagId("UniversalForwardOnly")
			}, opaque, evt, renderQueueRange, layerMask, stencilState, stencilReference)
		{
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x0003FE4F File Offset: 0x0003E04F
		internal DrawObjectsPass(URPProfileId profileId, bool opaque, RenderPassEvent evt, RenderQueueRange renderQueueRange, LayerMask layerMask, StencilState stencilState, int stencilReference)
			: this(profileId.GetType().Name, opaque, evt, renderQueueRange, layerMask, stencilState, stencilReference)
		{
			this.m_ProfilingSampler = ProfilingSampler.Get<URPProfileId>(profileId);
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x0003FE80 File Offset: 0x0003E080
		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			this.m_PassData.m_IsOpaque = this.m_IsOpaque;
			this.m_PassData.m_RenderingData = renderingData;
			this.m_PassData.m_RenderStateBlock = this.m_RenderStateBlock;
			this.m_PassData.m_FilteringSettings = this.m_FilteringSettings;
			this.m_PassData.m_ShaderTagIdList = this.m_ShaderTagIdList;
			this.m_PassData.m_ProfilingSampler = this.m_ProfilingSampler;
			this.m_PassData.m_IsActiveTargetBackBuffer = this.m_IsActiveTargetBackBuffer;
			this.m_PassData.pass = this;
			DrawObjectsPass.CameraSetup(renderingData.commandBuffer, this.m_PassData, ref renderingData);
			DrawObjectsPass.ExecutePass(context, this.m_PassData, ref renderingData, renderingData.cameraData.IsCameraProjectionMatrixFlipped());
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x0003FF3C File Offset: 0x0003E13C
		private static void CameraSetup(CommandBuffer cmd, DrawObjectsPass.PassData data, ref RenderingData renderingData)
		{
			if (renderingData.cameraData.renderer.useDepthPriming && data.m_IsOpaque && (renderingData.cameraData.renderType == CameraRenderType.Base || renderingData.cameraData.clearDepth))
			{
				data.m_RenderStateBlock.depthState = new DepthState(false, CompareFunction.Equal);
				data.m_RenderStateBlock.mask = data.m_RenderStateBlock.mask | RenderStateMask.Depth;
				return;
			}
			if (data.m_RenderStateBlock.depthState.compareFunction == CompareFunction.Equal)
			{
				data.m_RenderStateBlock.depthState = new DepthState(true, CompareFunction.LessEqual);
				data.m_RenderStateBlock.mask = data.m_RenderStateBlock.mask | RenderStateMask.Depth;
			}
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x0003FFE0 File Offset: 0x0003E1E0
		private static void ExecutePass(ScriptableRenderContext context, DrawObjectsPass.PassData data, ref RenderingData renderingData, bool yFlip)
		{
			CommandBuffer commandBuffer = renderingData.commandBuffer;
			using (new ProfilingScope(commandBuffer, data.m_ProfilingSampler))
			{
				Vector4 vector = new Vector4(0f, 0f, 0f, data.m_IsOpaque ? 1f : 0f);
				commandBuffer.SetGlobalVector(DrawObjectsPass.s_DrawObjectPassDataPropID, vector);
				if (data.m_RenderingData.cameraData.xr.enabled && data.m_IsActiveTargetBackBuffer)
				{
					commandBuffer.SetViewport(data.m_RenderingData.cameraData.xr.GetViewport(0));
				}
				float num = (yFlip ? (-1f) : 1f);
				Vector4 vector2 = ((num < 0f) ? new Vector4(num, 1f, -1f, 1f) : new Vector4(num, 0f, 1f, 1f));
				commandBuffer.SetGlobalVector(ShaderPropertyId.scaleBiasRt, vector2);
				float num2 = ((renderingData.cameraData.cameraTargetDescriptor.msaaSamples > 1 && data.m_IsOpaque) ? 1f : 0f);
				commandBuffer.SetGlobalFloat(ShaderPropertyId.alphaToMaskAvailable, num2);
				data.pass.OnExecute(commandBuffer);
				context.ExecuteCommandBuffer(commandBuffer);
				commandBuffer.Clear();
				SortingCriteria sortingCriteria = (data.m_IsOpaque ? renderingData.cameraData.defaultOpaqueSortFlags : SortingCriteria.CommonTransparent);
				if (renderingData.cameraData.renderer.useDepthPriming && data.m_IsOpaque && (renderingData.cameraData.renderType == CameraRenderType.Base || renderingData.cameraData.clearDepth))
				{
					sortingCriteria = SortingCriteria.SortingLayer | SortingCriteria.RenderQueue | SortingCriteria.OptimizeStateChanges | SortingCriteria.CanvasOrder;
				}
				FilteringSettings filteringSettings = data.m_FilteringSettings;
				DrawingSettings drawingSettings = RenderingUtils.CreateDrawingSettings(data.m_ShaderTagIdList, ref renderingData, sortingCriteria);
				DebugHandler activeDebugHandler = ScriptableRenderPass.GetActiveDebugHandler(ref renderingData);
				if (activeDebugHandler != null)
				{
					activeDebugHandler.DrawWithDebugRenderState(context, commandBuffer, ref renderingData, ref drawingSettings, ref filteringSettings, ref data.m_RenderStateBlock, delegate(ScriptableRenderContext ctx, ref RenderingData data, ref DrawingSettings ds, ref FilteringSettings fs, ref RenderStateBlock rsb)
					{
						ctx.DrawRenderers(data.cullResults, ref ds, ref fs, ref rsb);
					});
				}
				else
				{
					context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref filteringSettings, ref data.m_RenderStateBlock);
				}
				CoreUtils.SetKeyword(commandBuffer, "_WRITE_RENDERING_LAYERS", false);
				context.ExecuteCommandBuffer(commandBuffer);
				commandBuffer.Clear();
			}
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x00040220 File Offset: 0x0003E420
		internal void Render(RenderGraph renderGraph, TextureHandle colorTarget, TextureHandle depthTarget, TextureHandle mainShadowsTexture, TextureHandle additionalShadowsTexture, ref RenderingData renderingData)
		{
			DrawObjectsPass.PassData passData;
			using (RenderGraphBuilder renderGraphBuilder = renderGraph.AddRenderPass<DrawObjectsPass.PassData>("Draw Objects Pass", out passData, this.m_ProfilingSampler))
			{
				passData.m_Albedo = renderGraphBuilder.UseColorBuffer(in colorTarget, 0);
				passData.m_Depth = renderGraphBuilder.UseDepthBuffer(in depthTarget, DepthAccess.Write);
				if (mainShadowsTexture.IsValid())
				{
					renderGraphBuilder.ReadTexture(in mainShadowsTexture);
				}
				if (additionalShadowsTexture.IsValid())
				{
					renderGraphBuilder.ReadTexture(in additionalShadowsTexture);
				}
				passData.m_RenderingData = renderingData;
				renderGraphBuilder.AllowPassCulling(false);
				passData.m_IsOpaque = this.m_IsOpaque;
				passData.m_RenderStateBlock = this.m_RenderStateBlock;
				passData.m_FilteringSettings = this.m_FilteringSettings;
				passData.m_ShaderTagIdList = this.m_ShaderTagIdList;
				passData.m_ProfilingSampler = this.m_ProfilingSampler;
				passData.m_ShouldTransparentsReceiveShadows = this.m_ShouldTransparentsReceiveShadows;
				passData.m_IsActiveTargetBackBuffer = this.m_IsActiveTargetBackBuffer;
				passData.pass = this;
				renderGraphBuilder.SetRenderFunc<DrawObjectsPass.PassData>(delegate(DrawObjectsPass.PassData data, RenderGraphContext context)
				{
					ref RenderingData ptr = ref data.m_RenderingData;
					if (ptr.cameraData.xr.enabled)
					{
						bool flag = data.m_Albedo != ptr.cameraData.xr.renderTarget;
						ptr.cameraData.PushBuiltinShaderConstantsXR(ptr.commandBuffer, flag);
						XRSystemUniversal.MarkShaderProperties(ptr.commandBuffer, ptr.cameraData.xrUniversal, flag);
					}
					if (!data.m_IsOpaque && !data.m_ShouldTransparentsReceiveShadows)
					{
						TransparentSettingsPass.ExecutePass(context.cmd, data.m_ShouldTransparentsReceiveShadows);
					}
					bool flag2 = ptr.cameraData.IsRenderTargetProjectionMatrixFlipped(data.m_Albedo, data.m_Depth);
					DrawObjectsPass.CameraSetup(context.cmd, data, ref ptr);
					DrawObjectsPass.ExecutePass(context.renderContext, data, ref ptr, flag2);
				});
			}
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x00040340 File Offset: 0x0003E540
		protected virtual void OnExecute(CommandBuffer cmd)
		{
		}

		// Token: 0x04000898 RID: 2200
		private FilteringSettings m_FilteringSettings;

		// Token: 0x04000899 RID: 2201
		private RenderStateBlock m_RenderStateBlock;

		// Token: 0x0400089A RID: 2202
		private List<ShaderTagId> m_ShaderTagIdList = new List<ShaderTagId>();

		// Token: 0x0400089B RID: 2203
		private string m_ProfilerTag;

		// Token: 0x0400089C RID: 2204
		private ProfilingSampler m_ProfilingSampler;

		// Token: 0x0400089D RID: 2205
		private bool m_IsOpaque;

		// Token: 0x0400089E RID: 2206
		public bool m_IsActiveTargetBackBuffer;

		// Token: 0x0400089F RID: 2207
		public bool m_ShouldTransparentsReceiveShadows;

		// Token: 0x040008A0 RID: 2208
		private DrawObjectsPass.PassData m_PassData;

		// Token: 0x040008A1 RID: 2209
		private bool m_UseDepthPriming;

		// Token: 0x040008A2 RID: 2210
		private static readonly int s_DrawObjectPassDataPropID = Shader.PropertyToID("_DrawObjectPassData");

		// Token: 0x020001EC RID: 492
		private class PassData
		{
			// Token: 0x04000CB2 RID: 3250
			internal TextureHandle m_Albedo;

			// Token: 0x04000CB3 RID: 3251
			internal TextureHandle m_Depth;

			// Token: 0x04000CB4 RID: 3252
			internal RenderingData m_RenderingData;

			// Token: 0x04000CB5 RID: 3253
			internal bool m_IsOpaque;

			// Token: 0x04000CB6 RID: 3254
			internal RenderStateBlock m_RenderStateBlock;

			// Token: 0x04000CB7 RID: 3255
			internal FilteringSettings m_FilteringSettings;

			// Token: 0x04000CB8 RID: 3256
			internal List<ShaderTagId> m_ShaderTagIdList;

			// Token: 0x04000CB9 RID: 3257
			internal ProfilingSampler m_ProfilingSampler;

			// Token: 0x04000CBA RID: 3258
			internal bool m_ShouldTransparentsReceiveShadows;

			// Token: 0x04000CBB RID: 3259
			internal bool m_IsActiveTargetBackBuffer;

			// Token: 0x04000CBC RID: 3260
			internal DrawObjectsPass pass;
		}
	}
}
