using System;
using UnityEngine.Experimental.Rendering.RenderGraphModule;

namespace UnityEngine.Rendering.Universal.Internal
{
	// Token: 0x0200011D RID: 285
	public class CopyColorPass : ScriptableRenderPass
	{
		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000957 RID: 2391 RVA: 0x0003E649 File Offset: 0x0003C849
		// (set) Token: 0x06000958 RID: 2392 RVA: 0x0003E651 File Offset: 0x0003C851
		private RTHandle source { get; set; }

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000959 RID: 2393 RVA: 0x0003E65A File Offset: 0x0003C85A
		// (set) Token: 0x0600095A RID: 2394 RVA: 0x0003E662 File Offset: 0x0003C862
		private RTHandle destination { get; set; }

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x0600095B RID: 2395 RVA: 0x0003E66B File Offset: 0x0003C86B
		// (set) Token: 0x0600095C RID: 2396 RVA: 0x0003E673 File Offset: 0x0003C873
		private int destinationID { get; set; }

		// Token: 0x0600095D RID: 2397 RVA: 0x0003E67C File Offset: 0x0003C87C
		public CopyColorPass(RenderPassEvent evt, Material samplingMaterial, Material copyColorMaterial = null)
		{
			base.profilingSampler = new ProfilingSampler("CopyColorPass");
			this.m_PassData = new CopyColorPass.PassData();
			this.m_SamplingMaterial = samplingMaterial;
			this.m_CopyColorMaterial = copyColorMaterial;
			this.m_SampleOffsetShaderHandle = Shader.PropertyToID("_SampleOffset");
			base.renderPassEvent = evt;
			this.m_DownsamplingMethod = Downsampling.None;
			base.useNativeRenderPass = false;
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x0003E6E0 File Offset: 0x0003C8E0
		public static void ConfigureDescriptor(Downsampling downsamplingMethod, ref RenderTextureDescriptor descriptor, out FilterMode filterMode)
		{
			descriptor.msaaSamples = 1;
			descriptor.depthBufferBits = 0;
			if (downsamplingMethod == Downsampling._2xBilinear)
			{
				descriptor.width /= 2;
				descriptor.height /= 2;
			}
			else if (downsamplingMethod == Downsampling._4xBox || downsamplingMethod == Downsampling._4xBilinear)
			{
				descriptor.width /= 4;
				descriptor.height /= 4;
			}
			filterMode = ((downsamplingMethod == Downsampling.None) ? FilterMode.Point : FilterMode.Bilinear);
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x0003E74A File Offset: 0x0003C94A
		[Obsolete("Use RTHandles for source and destination.")]
		public void Setup(RenderTargetIdentifier source, RenderTargetHandle destination, Downsampling downsampling)
		{
			this.source = RTHandles.Alloc(source);
			this.destination = RTHandles.Alloc(destination.Identifier());
			this.destinationID = destination.id;
			this.m_DownsamplingMethod = downsampling;
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x0003E77E File Offset: 0x0003C97E
		public void Setup(RTHandle source, RTHandle destination, Downsampling downsampling)
		{
			this.source = source;
			this.destination = destination;
			this.m_DownsamplingMethod = downsampling;
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x0003E798 File Offset: 0x0003C998
		public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
		{
			if (this.destination.rt == null)
			{
				RenderTextureDescriptor cameraTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
				cameraTargetDescriptor.msaaSamples = 1;
				cameraTargetDescriptor.depthBufferBits = 0;
				if (this.m_DownsamplingMethod == Downsampling._2xBilinear)
				{
					cameraTargetDescriptor.width /= 2;
					cameraTargetDescriptor.height /= 2;
				}
				else if (this.m_DownsamplingMethod == Downsampling._4xBox || this.m_DownsamplingMethod == Downsampling._4xBilinear)
				{
					cameraTargetDescriptor.width /= 4;
					cameraTargetDescriptor.height /= 4;
				}
				cmd.GetTemporaryRT(this.destinationID, cameraTargetDescriptor, (this.m_DownsamplingMethod == Downsampling.None) ? FilterMode.Point : FilterMode.Bilinear);
				return;
			}
			cmd.SetGlobalTexture(this.destination.name, this.destination.nameID);
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x0003E868 File Offset: 0x0003CA68
		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			this.m_PassData.samplingMaterial = this.m_SamplingMaterial;
			this.m_PassData.copyColorMaterial = this.m_CopyColorMaterial;
			this.m_PassData.downsamplingMethod = this.m_DownsamplingMethod;
			this.m_PassData.clearFlag = base.clearFlag;
			this.m_PassData.clearColor = base.clearColor;
			this.m_PassData.sampleOffsetShaderHandle = this.m_SampleOffsetShaderHandle;
			CommandBuffer commandBuffer = renderingData.commandBuffer;
			if (this.source == renderingData.cameraData.renderer.GetCameraColorFrontBuffer(commandBuffer))
			{
				this.source = renderingData.cameraData.renderer.cameraColorTargetHandle;
			}
			bool enabled = renderingData.cameraData.xr.enabled;
			bool flag = enabled && renderingData.cameraData.xr.supportsFoveatedRendering;
			ScriptableRenderer.SetRenderTarget(commandBuffer, this.destination, ScriptableRenderPass.k_CameraTarget, base.clearFlag, base.clearColor);
			CopyColorPass.ExecutePass(this.m_PassData, this.source, this.destination, ref renderingData.commandBuffer, enabled, flag);
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x0003E978 File Offset: 0x0003CB78
		private static void ExecutePass(CopyColorPass.PassData passData, RTHandle source, RTHandle destination, ref CommandBuffer cmd, bool useDrawProceduralBlit, bool disableFoveatedRenderingForPass)
		{
			Material samplingMaterial = passData.samplingMaterial;
			Material copyColorMaterial = passData.copyColorMaterial;
			Downsampling downsamplingMethod = passData.downsamplingMethod;
			ClearFlag clearFlag = passData.clearFlag;
			Color clearColor = passData.clearColor;
			int sampleOffsetShaderHandle = passData.sampleOffsetShaderHandle;
			if (disableFoveatedRenderingForPass)
			{
				cmd.SetFoveatedRenderingMode(FoveatedRenderingMode.Disabled);
			}
			if (samplingMaterial == null)
			{
				Debug.LogErrorFormat("Missing {0}. Copy Color render pass will not execute. Check for missing reference in the renderer resources.", new object[] { samplingMaterial });
				return;
			}
			using (new ProfilingScope(cmd, ProfilingSampler.Get<URPProfileId>(URPProfileId.CopyColor)))
			{
				ScriptableRenderer.SetRenderTarget(cmd, destination, ScriptableRenderPass.k_CameraTarget, clearFlag, clearColor);
				switch (downsamplingMethod)
				{
				case Downsampling.None:
					Blitter.BlitCameraTexture(cmd, source, destination, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, copyColorMaterial, 0);
					break;
				case Downsampling._2xBilinear:
					Blitter.BlitCameraTexture(cmd, source, destination, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, copyColorMaterial, 1);
					break;
				case Downsampling._4xBox:
					samplingMaterial.SetFloat(sampleOffsetShaderHandle, 2f);
					Blitter.BlitCameraTexture(cmd, source, destination, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, samplingMaterial, 0);
					break;
				case Downsampling._4xBilinear:
					Blitter.BlitCameraTexture(cmd, source, destination, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, copyColorMaterial, 1);
					break;
				}
			}
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x0003EA7C File Offset: 0x0003CC7C
		internal TextureHandle Render(RenderGraph renderGraph, out TextureHandle destination, in TextureHandle source, Downsampling downsampling, ref RenderingData renderingData)
		{
			this.m_DownsamplingMethod = downsampling;
			CopyColorPass.PassData passData;
			using (RenderGraphBuilder renderGraphBuilder = renderGraph.AddRenderPass<CopyColorPass.PassData>("Copy Color", out passData, base.profilingSampler))
			{
				RenderTextureDescriptor cameraTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
				FilterMode filterMode;
				CopyColorPass.ConfigureDescriptor(downsampling, ref cameraTargetDescriptor, out filterMode);
				destination = UniversalRenderer.CreateRenderGraphTexture(renderGraph, cameraTargetDescriptor, "_CameraOpaqueTexture", true, filterMode, TextureWrapMode.Clamp);
				passData.destination = renderGraphBuilder.UseColorBuffer(in destination, 0);
				passData.source = renderGraphBuilder.ReadTexture(in source);
				passData.cmd = renderingData.commandBuffer;
				passData.useProceduralBlit = renderingData.cameraData.xr.enabled;
				passData.disableFoveatedRenderingForPass = renderingData.cameraData.xr.enabled && renderingData.cameraData.xr.supportsFoveatedRendering;
				passData.samplingMaterial = this.m_SamplingMaterial;
				passData.copyColorMaterial = this.m_CopyColorMaterial;
				passData.downsamplingMethod = this.m_DownsamplingMethod;
				passData.clearFlag = base.clearFlag;
				passData.clearColor = base.clearColor;
				passData.sampleOffsetShaderHandle = this.m_SampleOffsetShaderHandle;
				renderGraphBuilder.AllowPassCulling(false);
				renderGraphBuilder.SetRenderFunc<CopyColorPass.PassData>(delegate(CopyColorPass.PassData data, RenderGraphContext context)
				{
					CopyColorPass.ExecutePass(data, data.source, data.destination, ref data.cmd, data.useProceduralBlit, data.disableFoveatedRenderingForPass);
				});
			}
			CopyColorPass.PassData passData2;
			using (RenderGraphBuilder renderGraphBuilder2 = renderGraph.AddRenderPass<CopyColorPass.PassData>("Set Global Copy Color", out passData2, base.profilingSampler))
			{
				RenderTextureDescriptor cameraTargetDescriptor2 = renderingData.cameraData.cameraTargetDescriptor;
				FilterMode filterMode2;
				CopyColorPass.ConfigureDescriptor(downsampling, ref cameraTargetDescriptor2, out filterMode2);
				passData2.destination = renderGraphBuilder2.UseColorBuffer(in destination, 0);
				passData2.cmd = renderingData.commandBuffer;
				renderGraphBuilder2.AllowPassCulling(false);
				renderGraphBuilder2.SetRenderFunc<CopyColorPass.PassData>(delegate(CopyColorPass.PassData data, RenderGraphContext context)
				{
					data.cmd.SetGlobalTexture("_CameraOpaqueTexture", data.destination);
				});
			}
			return destination;
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x0003EC8C File Offset: 0x0003CE8C
		public override void OnCameraCleanup(CommandBuffer cmd)
		{
			if (cmd == null)
			{
				throw new ArgumentNullException("cmd");
			}
			if (this.destination.rt == null && this.destinationID != -1)
			{
				cmd.ReleaseTemporaryRT(this.destinationID);
				this.destination.Release();
				this.destination = null;
			}
		}

		// Token: 0x04000875 RID: 2165
		private int m_SampleOffsetShaderHandle;

		// Token: 0x04000876 RID: 2166
		private Material m_SamplingMaterial;

		// Token: 0x04000877 RID: 2167
		private Downsampling m_DownsamplingMethod;

		// Token: 0x04000878 RID: 2168
		private Material m_CopyColorMaterial;

		// Token: 0x0400087C RID: 2172
		private CopyColorPass.PassData m_PassData;

		// Token: 0x020001E2 RID: 482
		private class PassData
		{
			// Token: 0x04000C84 RID: 3204
			internal TextureHandle source;

			// Token: 0x04000C85 RID: 3205
			internal TextureHandle destination;

			// Token: 0x04000C86 RID: 3206
			internal bool useProceduralBlit;

			// Token: 0x04000C87 RID: 3207
			internal bool disableFoveatedRenderingForPass;

			// Token: 0x04000C88 RID: 3208
			internal CommandBuffer cmd;

			// Token: 0x04000C89 RID: 3209
			internal Material samplingMaterial;

			// Token: 0x04000C8A RID: 3210
			internal Material copyColorMaterial;

			// Token: 0x04000C8B RID: 3211
			internal Downsampling downsamplingMethod;

			// Token: 0x04000C8C RID: 3212
			internal ClearFlag clearFlag;

			// Token: 0x04000C8D RID: 3213
			internal Color clearColor;

			// Token: 0x04000C8E RID: 3214
			internal int sampleOffsetShaderHandle;
		}
	}
}
