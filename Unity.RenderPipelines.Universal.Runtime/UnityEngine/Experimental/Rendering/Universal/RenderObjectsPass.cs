using System;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace UnityEngine.Experimental.Rendering.Universal
{
	// Token: 0x02000007 RID: 7
	public class RenderObjectsPass : ScriptableRenderPass
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000033 RID: 51 RVA: 0x000027C4 File Offset: 0x000009C4
		// (set) Token: 0x06000034 RID: 52 RVA: 0x000027CC File Offset: 0x000009CC
		public Material overrideMaterial { get; set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000035 RID: 53 RVA: 0x000027D5 File Offset: 0x000009D5
		// (set) Token: 0x06000036 RID: 54 RVA: 0x000027DD File Offset: 0x000009DD
		public int overrideMaterialPassIndex { get; set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000037 RID: 55 RVA: 0x000027E6 File Offset: 0x000009E6
		// (set) Token: 0x06000038 RID: 56 RVA: 0x000027EE File Offset: 0x000009EE
		public Shader overrideShader { get; set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000039 RID: 57 RVA: 0x000027F7 File Offset: 0x000009F7
		// (set) Token: 0x0600003A RID: 58 RVA: 0x000027FF File Offset: 0x000009FF
		public int overrideShaderPassIndex { get; set; }

		// Token: 0x0600003B RID: 59 RVA: 0x00002808 File Offset: 0x00000A08
		public void SetDetphState(bool writeEnabled, CompareFunction function = CompareFunction.Less)
		{
			this.m_RenderStateBlock.mask = this.m_RenderStateBlock.mask | RenderStateMask.Depth;
			this.m_RenderStateBlock.depthState = new DepthState(writeEnabled, function);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002830 File Offset: 0x00000A30
		public void SetStencilState(int reference, CompareFunction compareFunction, StencilOp passOp, StencilOp failOp, StencilOp zFailOp)
		{
			StencilState defaultValue = StencilState.defaultValue;
			defaultValue.enabled = true;
			defaultValue.SetCompareFunction(compareFunction);
			defaultValue.SetPassOperation(passOp);
			defaultValue.SetFailOperation(failOp);
			defaultValue.SetZFailOperation(zFailOp);
			this.m_RenderStateBlock.mask = this.m_RenderStateBlock.mask | RenderStateMask.Stencil;
			this.m_RenderStateBlock.stencilReference = reference;
			this.m_RenderStateBlock.stencilState = defaultValue;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002898 File Offset: 0x00000A98
		public RenderObjectsPass(string profilerTag, RenderPassEvent renderPassEvent, string[] shaderTags, RenderQueueType renderQueueType, int layerMask, RenderObjects.CustomCameraSettings cameraSettings)
		{
			base.profilingSampler = new ProfilingSampler("RenderObjectsPass");
			this.m_ProfilerTag = profilerTag;
			this.m_ProfilingSampler = new ProfilingSampler(profilerTag);
			base.renderPassEvent = renderPassEvent;
			this.renderQueueType = renderQueueType;
			this.overrideMaterial = null;
			this.overrideMaterialPassIndex = 0;
			this.overrideShader = null;
			this.overrideShaderPassIndex = 0;
			RenderQueueRange renderQueueRange = ((renderQueueType == RenderQueueType.Transparent) ? RenderQueueRange.transparent : RenderQueueRange.opaque);
			this.m_FilteringSettings = new FilteringSettings(new RenderQueueRange?(renderQueueRange), layerMask, uint.MaxValue, 0);
			if (shaderTags != null && shaderTags.Length != 0)
			{
				foreach (string text in shaderTags)
				{
					this.m_ShaderTagIdList.Add(new ShaderTagId(text));
				}
			}
			else
			{
				this.m_ShaderTagIdList.Add(new ShaderTagId("SRPDefaultUnlit"));
				this.m_ShaderTagIdList.Add(new ShaderTagId("UniversalForward"));
				this.m_ShaderTagIdList.Add(new ShaderTagId("UniversalForwardOnly"));
			}
			this.m_RenderStateBlock = new RenderStateBlock(RenderStateMask.Nothing);
			this.m_CameraSettings = cameraSettings;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x000029AC File Offset: 0x00000BAC
		internal RenderObjectsPass(URPProfileId profileId, RenderPassEvent renderPassEvent, string[] shaderTags, RenderQueueType renderQueueType, int layerMask, RenderObjects.CustomCameraSettings cameraSettings)
			: this(profileId.GetType().Name, renderPassEvent, shaderTags, renderQueueType, layerMask, cameraSettings)
		{
			this.m_ProfilingSampler = ProfilingSampler.Get<URPProfileId>(profileId);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x000029D8 File Offset: 0x00000BD8
		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			SortingCriteria sortingCriteria = ((this.renderQueueType == RenderQueueType.Transparent) ? SortingCriteria.CommonTransparent : renderingData.cameraData.defaultOpaqueSortFlags);
			DrawingSettings drawingSettings = base.CreateDrawingSettings(this.m_ShaderTagIdList, ref renderingData, sortingCriteria);
			drawingSettings.overrideMaterial = this.overrideMaterial;
			drawingSettings.overrideMaterialPassIndex = this.overrideMaterialPassIndex;
			drawingSettings.overrideShader = this.overrideShader;
			drawingSettings.overrideShaderPassIndex = this.overrideShaderPassIndex;
			ref CameraData ptr = ref renderingData.cameraData;
			Camera camera = ptr.camera;
			Rect pixelRect = renderingData.cameraData.pixelRect;
			float num = pixelRect.width / pixelRect.height;
			CommandBuffer commandBuffer = renderingData.commandBuffer;
			using (new ProfilingScope(commandBuffer, this.m_ProfilingSampler))
			{
				if (this.m_CameraSettings.overrideCamera)
				{
					if (ptr.xr.enabled)
					{
						Debug.LogWarning("RenderObjects pass is configured to override camera matrices. While rendering in stereo camera matrices cannot be overridden.");
					}
					else
					{
						Matrix4x4 matrix4x = Matrix4x4.Perspective(this.m_CameraSettings.cameraFieldOfView, num, camera.nearClipPlane, camera.farClipPlane);
						matrix4x = GL.GetGPUProjectionMatrix(matrix4x, ptr.IsCameraProjectionMatrixFlipped());
						Matrix4x4 viewMatrix = ptr.GetViewMatrix(0);
						Vector4 column = viewMatrix.GetColumn(3);
						viewMatrix.SetColumn(3, column + this.m_CameraSettings.offset);
						RenderingUtils.SetViewAndProjectionMatrices(commandBuffer, viewMatrix, matrix4x, false);
					}
				}
				DebugHandler activeDebugHandler = ScriptableRenderPass.GetActiveDebugHandler(ref renderingData);
				if (activeDebugHandler != null)
				{
					activeDebugHandler.DrawWithDebugRenderState(context, commandBuffer, ref renderingData, ref drawingSettings, ref this.m_FilteringSettings, ref this.m_RenderStateBlock, delegate(ScriptableRenderContext ctx, ref RenderingData data, ref DrawingSettings ds, ref FilteringSettings fs, ref RenderStateBlock rsb)
					{
						ctx.DrawRenderers(data.cullResults, ref ds, ref fs, ref rsb);
					});
				}
				else
				{
					context.ExecuteCommandBuffer(commandBuffer);
					commandBuffer.Clear();
					context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref this.m_FilteringSettings, ref this.m_RenderStateBlock);
				}
				if (this.m_CameraSettings.overrideCamera && this.m_CameraSettings.restoreCamera && !ptr.xr.enabled)
				{
					RenderingUtils.SetViewAndProjectionMatrices(commandBuffer, ptr.GetViewMatrix(0), ptr.GetGPUProjectionMatrix(0), false);
				}
			}
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002BF4 File Offset: 0x00000DF4
		internal override void RecordRenderGraph(RenderGraph renderGraph, ref RenderingData renderingData)
		{
			UniversalRenderer universalRenderer = (UniversalRenderer)renderingData.cameraData.renderer;
			RenderObjectsPass.PassData passData;
			using (RenderGraphBuilder renderGraphBuilder = renderGraph.AddRenderPass<RenderObjectsPass.PassData>("Render Objects Pass", out passData, this.m_ProfilingSampler))
			{
				TextureHandle activeRenderGraphColor = UniversalRenderer.m_ActiveRenderGraphColor;
				renderGraphBuilder.UseColorBuffer(in activeRenderGraphColor, 0);
				renderGraphBuilder.UseDepthBuffer(in UniversalRenderer.m_ActiveRenderGraphDepth, DepthAccess.Write);
				renderGraphBuilder.ReadTexture(in universalRenderer.frameResources.mainShadowsTexture);
				renderGraphBuilder.AllowPassCulling(false);
				passData.pass = this;
				passData.renderingData = renderingData;
				renderGraphBuilder.SetRenderFunc<RenderObjectsPass.PassData>(delegate(RenderObjectsPass.PassData data, RenderGraphContext rgContext)
				{
					data.pass.Execute(rgContext.renderContext, ref data.renderingData);
				});
			}
		}

		// Token: 0x04000013 RID: 19
		private RenderQueueType renderQueueType;

		// Token: 0x04000014 RID: 20
		private FilteringSettings m_FilteringSettings;

		// Token: 0x04000015 RID: 21
		private RenderObjects.CustomCameraSettings m_CameraSettings;

		// Token: 0x04000016 RID: 22
		private string m_ProfilerTag;

		// Token: 0x04000017 RID: 23
		private ProfilingSampler m_ProfilingSampler;

		// Token: 0x0400001C RID: 28
		private List<ShaderTagId> m_ShaderTagIdList = new List<ShaderTagId>();

		// Token: 0x0400001D RID: 29
		private RenderStateBlock m_RenderStateBlock;

		// Token: 0x0200014A RID: 330
		private class PassData
		{
			// Token: 0x0400094D RID: 2381
			internal RenderObjectsPass pass;

			// Token: 0x0400094E RID: 2382
			internal RenderingData renderingData;
		}
	}
}
