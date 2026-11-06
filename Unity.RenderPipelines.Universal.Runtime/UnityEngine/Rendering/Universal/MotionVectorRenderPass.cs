using System;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Experimental.Rendering.RenderGraphModule;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000B1 RID: 177
	internal sealed class MotionVectorRenderPass : ScriptableRenderPass
	{
		// Token: 0x060005B5 RID: 1461 RVA: 0x00020FB0 File Offset: 0x0001F1B0
		internal MotionVectorRenderPass(RenderPassEvent evt, Material cameraMaterial, Material objectMaterial, LayerMask opaqueLayerMask)
		{
			base.renderPassEvent = evt;
			this.m_CameraMaterial = cameraMaterial;
			this.m_ObjectMaterial = objectMaterial;
			this.m_FilteringSettings = new FilteringSettings(new RenderQueueRange?(RenderQueueRange.opaque), opaqueLayerMask, uint.MaxValue, 0);
			this.m_PassData = new MotionVectorRenderPass.PassData();
			base.profilingSampler = ProfilingSampler.Get<URPProfileId>(URPProfileId.MotionVectors);
			base.ConfigureInput(ScriptableRenderPassInput.Depth);
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x00021015 File Offset: 0x0001F215
		internal void Setup(RTHandle color, RTHandle depth)
		{
			this.m_Color = color;
			this.m_Depth = depth;
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x00021028 File Offset: 0x0001F228
		public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
		{
			cmd.SetGlobalTexture(this.m_Color.name, this.m_Color.nameID);
			cmd.SetGlobalTexture(this.m_Depth.name, this.m_Depth.nameID);
			base.ConfigureTarget(this.m_Color, this.m_Depth);
			base.ConfigureClear(ClearFlag.Color | ClearFlag.Depth, Color.black);
			base.ConfigureDepthStoreAction(RenderBufferStoreAction.DontCare);
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x00021094 File Offset: 0x0001F294
		private static void ExecutePass(ScriptableRenderContext context, MotionVectorRenderPass.PassData passData, ref RenderingData renderingData)
		{
			Material cameraMaterial = passData.cameraMaterial;
			Material objectMaterial = passData.objectMaterial;
			if (cameraMaterial == null || objectMaterial == null)
			{
				return;
			}
			ref CameraData ptr = ref renderingData.cameraData;
			Camera camera = ptr.camera;
			MotionVectorsPersistentData motionVectorsPersistentData = null;
			UniversalAdditionalCameraData universalAdditionalCameraData;
			if (camera.TryGetComponent<UniversalAdditionalCameraData>(out universalAdditionalCameraData))
			{
				motionVectorsPersistentData = universalAdditionalCameraData.motionVectorsPersistentData;
			}
			if (motionVectorsPersistentData == null)
			{
				return;
			}
			if (camera.cameraType == CameraType.Preview)
			{
				return;
			}
			CommandBuffer commandBuffer = renderingData.commandBuffer;
			using (new ProfilingScope(commandBuffer, ProfilingSampler.Get<URPProfileId>(URPProfileId.MotionVectors)))
			{
				int xrmultiPassId = motionVectorsPersistentData.GetXRMultiPassId(ref ptr);
				context.ExecuteCommandBuffer(commandBuffer);
				commandBuffer.Clear();
				if (ptr.xr.enabled && ptr.xr.singlePassEnabled)
				{
					commandBuffer.SetGlobalMatrixArray("_PrevViewProjMatrixStereo", motionVectorsPersistentData.previousViewProjectionStereo);
					commandBuffer.SetGlobalMatrixArray("_NonJitteredViewProjMatrixStereo", motionVectorsPersistentData.viewProjectionStereo);
				}
				else
				{
					commandBuffer.SetGlobalMatrix("_PrevViewProjMatrix", motionVectorsPersistentData.previousViewProjectionStereo[xrmultiPassId]);
					commandBuffer.SetGlobalMatrix("_NonJitteredViewProjMatrix", motionVectorsPersistentData.viewProjectionStereo[xrmultiPassId]);
				}
				camera.depthTextureMode |= DepthTextureMode.Depth | DepthTextureMode.MotionVectors;
				MotionVectorRenderPass.DrawCameraMotionVectors(context, commandBuffer, ref renderingData, camera, cameraMaterial);
				MotionVectorRenderPass.DrawObjectMotionVectors(context, ref renderingData, camera, objectMaterial, commandBuffer, ref passData.filteringSettings);
			}
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x000211E8 File Offset: 0x0001F3E8
		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			this.m_PassData.cameraMaterial = this.m_CameraMaterial;
			this.m_PassData.objectMaterial = this.m_ObjectMaterial;
			this.m_PassData.filteringSettings = this.m_FilteringSettings;
			MotionVectorRenderPass.ExecutePass(context, this.m_PassData, ref renderingData);
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x00021238 File Offset: 0x0001F438
		private static DrawingSettings GetDrawingSettings(ref RenderingData renderingData, Material objectMaterial)
		{
			Camera camera = renderingData.cameraData.camera;
			SortingSettings sortingSettings = new SortingSettings(camera)
			{
				criteria = SortingCriteria.CommonOpaque
			};
			DrawingSettings drawingSettings = new DrawingSettings(ShaderTagId.none, sortingSettings)
			{
				perObjectData = PerObjectData.MotionVectors,
				enableDynamicBatching = renderingData.supportsDynamicBatching,
				enableInstancing = true
			};
			for (int i = 0; i < MotionVectorRenderPass.s_ShaderTags.Length; i++)
			{
				drawingSettings.SetShaderPassName(i, new ShaderTagId(MotionVectorRenderPass.s_ShaderTags[i]));
			}
			drawingSettings.fallbackMaterial = objectMaterial;
			return drawingSettings;
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x000212C8 File Offset: 0x0001F4C8
		private static void DrawCameraMotionVectors(ScriptableRenderContext context, CommandBuffer cmd, ref RenderingData renderingData, Camera camera, Material cameraMaterial)
		{
			bool supportsFoveatedRendering = renderingData.cameraData.xr.supportsFoveatedRendering;
			bool flag = supportsFoveatedRendering && XRSystem.foveatedRenderingCaps.HasFlag(FoveatedRenderingCaps.NonUniformRaster);
			if (supportsFoveatedRendering)
			{
				if (flag)
				{
					cmd.SetFoveatedRenderingMode(FoveatedRenderingMode.Disabled);
				}
				else
				{
					cmd.SetFoveatedRenderingMode(FoveatedRenderingMode.Enabled);
				}
			}
			cmd.DrawProcedural(Matrix4x4.identity, cameraMaterial, 0, MeshTopology.Triangles, 3, 1);
			if (supportsFoveatedRendering && !flag)
			{
				cmd.SetFoveatedRenderingMode(FoveatedRenderingMode.Disabled);
			}
			context.ExecuteCommandBuffer(cmd);
			cmd.Clear();
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x00021344 File Offset: 0x0001F544
		private static void DrawObjectMotionVectors(ScriptableRenderContext context, ref RenderingData renderingData, Camera camera, Material objectMaterial, CommandBuffer cmd, ref FilteringSettings filteringSettings)
		{
			bool supportsFoveatedRendering = renderingData.cameraData.xr.supportsFoveatedRendering;
			if (supportsFoveatedRendering)
			{
				cmd.SetFoveatedRenderingMode(FoveatedRenderingMode.Enabled);
				context.ExecuteCommandBuffer(cmd);
				cmd.Clear();
			}
			DrawingSettings drawingSettings = MotionVectorRenderPass.GetDrawingSettings(ref renderingData, objectMaterial);
			RenderStateBlock renderStateBlock = new RenderStateBlock(RenderStateMask.Nothing);
			context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref filteringSettings, ref renderStateBlock);
			if (supportsFoveatedRendering)
			{
				cmd.SetFoveatedRenderingMode(FoveatedRenderingMode.Disabled);
				context.ExecuteCommandBuffer(cmd);
				cmd.Clear();
			}
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x000213BC File Offset: 0x0001F5BC
		internal void Render(RenderGraph renderGraph, ref TextureHandle cameraDepthTexture, in TextureHandle motionVectorColor, in TextureHandle motionVectorDepth, ref RenderingData renderingData)
		{
			MotionVectorRenderPass.PassData passData;
			using (RenderGraphBuilder renderGraphBuilder = renderGraph.AddRenderPass<MotionVectorRenderPass.PassData>("Motion Vector Pass", out passData, base.profilingSampler))
			{
				renderGraphBuilder.AllowPassCulling(false);
				passData.motionVectorColor = renderGraphBuilder.UseColorBuffer(in motionVectorColor, 0);
				passData.motionVectorDepth = renderGraphBuilder.UseDepthBuffer(in motionVectorDepth, DepthAccess.Write);
				passData.cameraDepth = renderGraphBuilder.ReadTexture(in cameraDepthTexture);
				passData.renderingData = renderingData;
				passData.cameraMaterial = this.m_CameraMaterial;
				passData.objectMaterial = this.m_ObjectMaterial;
				passData.filteringSettings = this.m_FilteringSettings;
				renderGraphBuilder.SetRenderFunc<MotionVectorRenderPass.PassData>(delegate(MotionVectorRenderPass.PassData data, RenderGraphContext context)
				{
					MotionVectorRenderPass.ExecutePass(context.renderContext, data, ref data.renderingData);
					data.renderingData.commandBuffer.SetGlobalTexture("_MotionVectorTexture", data.motionVectorColor);
				});
			}
		}

		// Token: 0x04000453 RID: 1107
		private const string kPreviousViewProjectionNoJitter = "_PrevViewProjMatrix";

		// Token: 0x04000454 RID: 1108
		private const string kViewProjectionNoJitter = "_NonJitteredViewProjMatrix";

		// Token: 0x04000455 RID: 1109
		private const string kPreviousViewProjectionNoJitterStereo = "_PrevViewProjMatrixStereo";

		// Token: 0x04000456 RID: 1110
		private const string kViewProjectionNoJitterStereo = "_NonJitteredViewProjMatrixStereo";

		// Token: 0x04000457 RID: 1111
		internal const GraphicsFormat k_TargetFormat = GraphicsFormat.R16G16_SFloat;

		// Token: 0x04000458 RID: 1112
		private static readonly string[] s_ShaderTags = new string[] { "MotionVectors" };

		// Token: 0x04000459 RID: 1113
		private RTHandle m_Color;

		// Token: 0x0400045A RID: 1114
		private RTHandle m_Depth;

		// Token: 0x0400045B RID: 1115
		private readonly Material m_CameraMaterial;

		// Token: 0x0400045C RID: 1116
		private readonly Material m_ObjectMaterial;

		// Token: 0x0400045D RID: 1117
		private readonly FilteringSettings m_FilteringSettings;

		// Token: 0x0400045E RID: 1118
		private MotionVectorRenderPass.PassData m_PassData;

		// Token: 0x020001A2 RID: 418
		private class PassData
		{
			// Token: 0x04000AC9 RID: 2761
			internal TextureHandle motionVectorColor;

			// Token: 0x04000ACA RID: 2762
			internal TextureHandle motionVectorDepth;

			// Token: 0x04000ACB RID: 2763
			internal TextureHandle cameraDepth;

			// Token: 0x04000ACC RID: 2764
			internal RenderingData renderingData;

			// Token: 0x04000ACD RID: 2765
			internal Material cameraMaterial;

			// Token: 0x04000ACE RID: 2766
			internal Material objectMaterial;

			// Token: 0x04000ACF RID: 2767
			internal FilteringSettings filteringSettings;
		}
	}
}
