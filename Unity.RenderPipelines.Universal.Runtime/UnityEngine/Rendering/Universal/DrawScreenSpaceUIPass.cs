using System;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Experimental.Rendering.RenderGraphModule;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000AD RID: 173
	internal class DrawScreenSpaceUIPass : ScriptableRenderPass
	{
		// Token: 0x0600059B RID: 1435 RVA: 0x00020435 File Offset: 0x0001E635
		public DrawScreenSpaceUIPass(RenderPassEvent evt, bool renderOffscreen)
		{
			base.profilingSampler = new ProfilingSampler("DrawScreenSpaceUIPass");
			base.renderPassEvent = evt;
			base.useNativeRenderPass = false;
			this.m_RenderOffscreen = renderOffscreen;
			this.m_PassData = new DrawScreenSpaceUIPass.PassData();
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x0002046D File Offset: 0x0001E66D
		public static void ConfigureColorDescriptor(ref RenderTextureDescriptor descriptor, int cameraWidth, int cameraHeight)
		{
			descriptor.graphicsFormat = GraphicsFormat.R8G8B8A8_SRGB;
			descriptor.depthBufferBits = 0;
			descriptor.width = cameraWidth;
			descriptor.height = cameraHeight;
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x0002048B File Offset: 0x0001E68B
		public static void ConfigureDepthDescriptor(ref RenderTextureDescriptor descriptor, int depthBufferBits, int cameraWidth, int cameraHeight)
		{
			descriptor.graphicsFormat = GraphicsFormat.None;
			descriptor.depthBufferBits = depthBufferBits;
			descriptor.width = cameraWidth;
			descriptor.height = cameraHeight;
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x000204A9 File Offset: 0x0001E6A9
		private static void ExecutePass(ScriptableRenderContext context, DrawScreenSpaceUIPass.PassData passData)
		{
			context.ExecuteCommandBuffer(passData.cmd);
			passData.cmd.Clear();
			context.DrawUIOverlay(passData.camera);
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x000204D0 File Offset: 0x0001E6D0
		public void Dispose()
		{
			RTHandle colorTarget = this.m_ColorTarget;
			if (colorTarget != null)
			{
				colorTarget.Release();
			}
			RTHandle depthTarget = this.m_DepthTarget;
			if (depthTarget == null)
			{
				return;
			}
			depthTarget.Release();
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x000204F4 File Offset: 0x0001E6F4
		public void Setup(ref CameraData cameraData, int depthBufferBits)
		{
			if (this.m_RenderOffscreen)
			{
				RenderTextureDescriptor cameraTargetDescriptor = cameraData.cameraTargetDescriptor;
				DrawScreenSpaceUIPass.ConfigureColorDescriptor(ref cameraTargetDescriptor, cameraData.pixelWidth, cameraData.pixelHeight);
				RenderingUtils.ReAllocateIfNeeded(ref this.m_ColorTarget, in cameraTargetDescriptor, FilterMode.Point, TextureWrapMode.Repeat, false, 1, 0f, "_OverlayUITexture");
				RenderTextureDescriptor cameraTargetDescriptor2 = cameraData.cameraTargetDescriptor;
				DrawScreenSpaceUIPass.ConfigureDepthDescriptor(ref cameraTargetDescriptor2, depthBufferBits, cameraData.pixelWidth, cameraData.pixelHeight);
				RenderingUtils.ReAllocateIfNeeded(ref this.m_DepthTarget, in cameraTargetDescriptor2, FilterMode.Point, TextureWrapMode.Repeat, false, 1, 0f, "_OverlayUITexture_Depth");
			}
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x00020578 File Offset: 0x0001E778
		public unsafe override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			this.m_PassData.cmd = renderingData.commandBuffer;
			this.m_PassData.camera = renderingData.cameraData.camera;
			if (this.m_RenderOffscreen)
			{
				CoreUtils.SetRenderTarget(renderingData.commandBuffer, this.m_ColorTarget, this.m_DepthTarget, ClearFlag.Color, Color.clear, 0, CubemapFace.Unknown, -1);
				renderingData.commandBuffer.SetGlobalTexture(ShaderPropertyId.overlayUITexture, this.m_ColorTarget);
			}
			else
			{
				DebugHandler activeDebugHandler = ScriptableRenderPass.GetActiveDebugHandler(ref renderingData);
				RenderTargetIdentifier cameraTargetIdentifier = RenderingUtils.GetCameraTargetIdentifier(ref renderingData);
				if (activeDebugHandler != null && activeDebugHandler.WriteToDebugScreenTexture(ref renderingData.cameraData))
				{
					CoreUtils.SetRenderTarget(renderingData.commandBuffer, *activeDebugHandler.DebugScreenColorHandle, *activeDebugHandler.DebugScreenDepthHandle, 0, CubemapFace.Unknown, -1);
				}
				else
				{
					RTHandleStaticHelpers.SetRTHandleStaticWrapper(cameraTargetIdentifier);
					RTHandle s_RTHandleWrapper = RTHandleStaticHelpers.s_RTHandleWrapper;
					CoreUtils.SetRenderTarget(renderingData.commandBuffer, s_RTHandleWrapper, ClearFlag.None, 0, CubemapFace.Unknown, -1);
				}
			}
			using (new ProfilingScope(renderingData.commandBuffer, ProfilingSampler.Get<URPProfileId>(URPProfileId.DrawScreenSpaceUI)))
			{
				DrawScreenSpaceUIPass.ExecutePass(context, this.m_PassData);
			}
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x00020690 File Offset: 0x0001E890
		internal void RenderOffscreen(RenderGraph renderGraph, int depthBufferBits, out TextureHandle output, ref RenderingData renderingData)
		{
			DrawScreenSpaceUIPass.PassData passData;
			using (RenderGraphBuilder renderGraphBuilder = renderGraph.AddRenderPass<DrawScreenSpaceUIPass.PassData>("Draw Screen Space UI Pass - Offscreen", out passData, base.profilingSampler))
			{
				RenderTextureDescriptor cameraTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
				DrawScreenSpaceUIPass.ConfigureColorDescriptor(ref cameraTargetDescriptor, renderingData.cameraData.pixelWidth, renderingData.cameraData.pixelHeight);
				output = UniversalRenderer.CreateRenderGraphTexture(renderGraph, cameraTargetDescriptor, "_OverlayUITexture", true, FilterMode.Point, TextureWrapMode.Clamp);
				renderGraphBuilder.UseColorBuffer(in output, 0);
				RenderTextureDescriptor cameraTargetDescriptor2 = renderingData.cameraData.cameraTargetDescriptor;
				DrawScreenSpaceUIPass.ConfigureDepthDescriptor(ref cameraTargetDescriptor2, depthBufferBits, renderingData.cameraData.pixelWidth, renderingData.cameraData.pixelHeight);
				TextureHandle textureHandle = UniversalRenderer.CreateRenderGraphTexture(renderGraph, cameraTargetDescriptor2, "_OverlayUITexture_Depth", false, FilterMode.Point, TextureWrapMode.Clamp);
				renderGraphBuilder.UseDepthBuffer(in textureHandle, DepthAccess.ReadWrite);
				passData.cmd = renderingData.commandBuffer;
				passData.camera = renderingData.cameraData.camera;
				passData.offscreenTexture = output;
				renderGraphBuilder.SetRenderFunc<DrawScreenSpaceUIPass.PassData>(delegate(DrawScreenSpaceUIPass.PassData data, RenderGraphContext context)
				{
					DrawScreenSpaceUIPass.ExecutePass(context.renderContext, data);
					data.cmd.SetGlobalTexture(ShaderPropertyId.overlayUITexture, data.offscreenTexture);
				});
			}
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x000207B8 File Offset: 0x0001E9B8
		internal void RenderOverlay(RenderGraph renderGraph, in TextureHandle colorBuffer, in TextureHandle depthBuffer, ref RenderingData renderingData)
		{
			DrawScreenSpaceUIPass.PassData passData;
			using (RenderGraphBuilder renderGraphBuilder = renderGraph.AddRenderPass<DrawScreenSpaceUIPass.PassData>("Draw Screen Space UI Pass - Overlay", out passData, base.profilingSampler))
			{
				renderGraphBuilder.UseColorBuffer(in colorBuffer, 0);
				renderGraphBuilder.UseDepthBuffer(in depthBuffer, DepthAccess.ReadWrite);
				passData.cmd = renderingData.commandBuffer;
				passData.camera = renderingData.cameraData.camera;
				renderGraphBuilder.SetRenderFunc<DrawScreenSpaceUIPass.PassData>(delegate(DrawScreenSpaceUIPass.PassData data, RenderGraphContext context)
				{
					DrawScreenSpaceUIPass.ExecutePass(context.renderContext, data);
				});
			}
		}

		// Token: 0x0400044A RID: 1098
		private DrawScreenSpaceUIPass.PassData m_PassData;

		// Token: 0x0400044B RID: 1099
		private RTHandle m_ColorTarget;

		// Token: 0x0400044C RID: 1100
		private RTHandle m_DepthTarget;

		// Token: 0x0400044D RID: 1101
		private bool m_RenderOffscreen;

		// Token: 0x02000199 RID: 409
		private class PassData
		{
			// Token: 0x04000AAD RID: 2733
			internal CommandBuffer cmd;

			// Token: 0x04000AAE RID: 2734
			internal Camera camera;

			// Token: 0x04000AAF RID: 2735
			internal TextureHandle offscreenTexture;
		}
	}
}
