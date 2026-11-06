using System;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Experimental.Rendering.RenderGraphModule;

namespace UnityEngine.Rendering.Universal.Internal
{
	// Token: 0x0200011E RID: 286
	public class CopyDepthPass : ScriptableRenderPass
	{
		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000966 RID: 2406 RVA: 0x0003ECE1 File Offset: 0x0003CEE1
		// (set) Token: 0x06000967 RID: 2407 RVA: 0x0003ECE9 File Offset: 0x0003CEE9
		private RTHandle source { get; set; }

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000968 RID: 2408 RVA: 0x0003ECF2 File Offset: 0x0003CEF2
		// (set) Token: 0x06000969 RID: 2409 RVA: 0x0003ECFA File Offset: 0x0003CEFA
		private RTHandle destination { get; set; }

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x0600096A RID: 2410 RVA: 0x0003ED03 File Offset: 0x0003CF03
		// (set) Token: 0x0600096B RID: 2411 RVA: 0x0003ED0B File Offset: 0x0003CF0B
		internal int MssaSamples { get; set; }

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x0600096C RID: 2412 RVA: 0x0003ED14 File Offset: 0x0003CF14
		// (set) Token: 0x0600096D RID: 2413 RVA: 0x0003ED1C File Offset: 0x0003CF1C
		internal bool CopyToDepth { get; set; }

		// Token: 0x0600096E RID: 2414 RVA: 0x0003ED28 File Offset: 0x0003CF28
		public CopyDepthPass(RenderPassEvent evt, Material copyDepthMaterial, bool shouldClear = false, bool copyToDepth = false, bool copyResolvedDepth = false)
		{
			base.profilingSampler = new ProfilingSampler("CopyDepthPass");
			this.m_PassData = new CopyDepthPass.PassData();
			this.CopyToDepth = copyToDepth;
			this.m_CopyDepthMaterial = copyDepthMaterial;
			base.renderPassEvent = evt;
			this.m_CopyResolvedDepth = copyResolvedDepth;
			this.m_ShouldClear = shouldClear;
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x0003ED7B File Offset: 0x0003CF7B
		public void Setup(RTHandle source, RTHandle destination)
		{
			this.source = source;
			this.destination = destination;
			this.MssaSamples = -1;
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x0003ED94 File Offset: 0x0003CF94
		public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
		{
			RenderTextureDescriptor cameraTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
			cameraTargetDescriptor.graphicsFormat = ((this.destination.rt && this.destination.rt.graphicsFormat == GraphicsFormat.None) ? GraphicsFormat.D32_SFloat_S8_UInt : GraphicsFormat.R32_SFloat);
			cameraTargetDescriptor.msaaSamples = 1;
			base.ConfigureTarget(this.destination);
			if (this.m_ShouldClear)
			{
				base.ConfigureClear(ClearFlag.All, Color.black);
			}
		}

		// Token: 0x06000971 RID: 2417 RVA: 0x0003EE10 File Offset: 0x0003D010
		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			this.m_PassData.copyDepthMaterial = this.m_CopyDepthMaterial;
			this.m_PassData.msaaSamples = this.MssaSamples;
			this.m_PassData.copyResolvedDepth = this.m_CopyResolvedDepth;
			this.m_PassData.copyToDepth = this.CopyToDepth || !RenderingUtils.SupportsGraphicsFormat(GraphicsFormat.R32_SFloat, FormatUsage.Render);
			renderingData.commandBuffer.SetGlobalTexture("_CameraDepthAttachment", this.source.nameID);
			CopyDepthPass.ExecutePass(context, this.m_PassData, ref renderingData.commandBuffer, ref renderingData.cameraData, this.source, this.destination);
		}

		// Token: 0x06000972 RID: 2418 RVA: 0x0003EEB0 File Offset: 0x0003D0B0
		private static void ExecutePass(ScriptableRenderContext context, CopyDepthPass.PassData passData, ref CommandBuffer cmd, ref CameraData cameraData, RTHandle source, RTHandle destination)
		{
			Material copyDepthMaterial = passData.copyDepthMaterial;
			int msaaSamples = passData.msaaSamples;
			bool copyResolvedDepth = passData.copyResolvedDepth;
			bool copyToDepth = passData.copyToDepth;
			if (copyDepthMaterial == null)
			{
				Debug.LogErrorFormat("Missing {0}. Copy Depth render pass will not execute. Check for missing reference in the renderer resources.", new object[] { copyDepthMaterial });
				return;
			}
			using (new ProfilingScope(cmd, ProfilingSampler.Get<URPProfileId>(URPProfileId.CopyDepth)))
			{
				int num;
				if (msaaSamples == -1)
				{
					RenderTextureDescriptor cameraTargetDescriptor = cameraData.cameraTargetDescriptor;
					num = cameraTargetDescriptor.msaaSamples;
				}
				else
				{
					num = msaaSamples;
				}
				if (SystemInfo.supportsMultisampledTextures == 0 || copyResolvedDepth)
				{
					num = 1;
				}
				if (num != 2)
				{
					if (num != 4)
					{
						if (num == 8)
						{
							cmd.DisableShaderKeyword("_DEPTH_MSAA_2");
							cmd.DisableShaderKeyword("_DEPTH_MSAA_4");
							cmd.EnableShaderKeyword("_DEPTH_MSAA_8");
						}
						else
						{
							cmd.DisableShaderKeyword("_DEPTH_MSAA_2");
							cmd.DisableShaderKeyword("_DEPTH_MSAA_4");
							cmd.DisableShaderKeyword("_DEPTH_MSAA_8");
						}
					}
					else
					{
						cmd.DisableShaderKeyword("_DEPTH_MSAA_2");
						cmd.EnableShaderKeyword("_DEPTH_MSAA_4");
						cmd.DisableShaderKeyword("_DEPTH_MSAA_8");
					}
				}
				else
				{
					cmd.EnableShaderKeyword("_DEPTH_MSAA_2");
					cmd.DisableShaderKeyword("_DEPTH_MSAA_4");
					cmd.DisableShaderKeyword("_DEPTH_MSAA_8");
				}
				if (copyToDepth || destination.rt.graphicsFormat == GraphicsFormat.None)
				{
					cmd.EnableShaderKeyword("_OUTPUT_DEPTH");
				}
				else
				{
					cmd.DisableShaderKeyword("_OUTPUT_DEPTH");
				}
				Vector2 vector = (source.useScaling ? new Vector2(source.rtHandleProperties.rtHandleScale.x, source.rtHandleProperties.rtHandleScale.y) : Vector2.one);
				bool flag = cameraData.cameraType == CameraType.Game && destination.nameID == BuiltinRenderTextureType.CameraTarget;
				if (cameraData.xr.enabled)
				{
					if (cameraData.xr.supportsFoveatedRendering)
					{
						cmd.SetFoveatedRenderingMode(FoveatedRenderingMode.Disabled);
					}
					flag |= new RenderTargetIdentifier(destination.nameID, 0, CubemapFace.Unknown, 0) == new RenderTargetIdentifier(cameraData.xr.renderTarget, 0, CubemapFace.Unknown, 0);
				}
				Vector4 vector2 = ((cameraData.IsHandleYFlipped(source) != cameraData.IsHandleYFlipped(destination)) ? new Vector4(vector.x, -vector.y, 0f, vector.y) : new Vector4(vector.x, vector.y, 0f, 0f));
				if (flag)
				{
					cmd.SetViewport(cameraData.pixelRect);
				}
				Blitter.BlitTexture(cmd, source, vector2, copyDepthMaterial, 0);
			}
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x0003F14C File Offset: 0x0003D34C
		public override void OnCameraCleanup(CommandBuffer cmd)
		{
			if (cmd == null)
			{
				throw new ArgumentNullException("cmd");
			}
			this.destination = ScriptableRenderPass.k_CameraTarget;
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x0003F168 File Offset: 0x0003D368
		internal void Render(RenderGraph renderGraph, out TextureHandle destination, in TextureHandle source, ref RenderingData renderingData)
		{
			this.MssaSamples = -1;
			CopyDepthPass.PassData passData;
			using (RenderGraphBuilder renderGraphBuilder = renderGraph.AddRenderPass<CopyDepthPass.PassData>("Setup Global Depth", out passData, base.profilingSampler))
			{
				passData.source = renderGraphBuilder.ReadTexture(in source);
				renderGraphBuilder.AllowPassCulling(false);
				renderGraphBuilder.SetRenderFunc<CopyDepthPass.PassData>(delegate(CopyDepthPass.PassData data, RenderGraphContext context)
				{
					context.cmd.SetGlobalTexture("_CameraDepthAttachment", data.source);
				});
			}
			CopyDepthPass.PassData passData2;
			using (RenderGraphBuilder renderGraphBuilder2 = renderGraph.AddRenderPass<CopyDepthPass.PassData>("Copy Depth", out passData2, base.profilingSampler))
			{
				RenderTextureDescriptor cameraTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
				cameraTargetDescriptor.graphicsFormat = GraphicsFormat.R32_SFloat;
				cameraTargetDescriptor.depthStencilFormat = GraphicsFormat.None;
				cameraTargetDescriptor.depthBufferBits = 0;
				cameraTargetDescriptor.msaaSamples = 1;
				destination = UniversalRenderer.CreateRenderGraphTexture(renderGraph, cameraTargetDescriptor, "_CameraDepthTexture", true, FilterMode.Point, TextureWrapMode.Clamp);
				passData2.copyDepthMaterial = this.m_CopyDepthMaterial;
				passData2.msaaSamples = this.MssaSamples;
				passData2.cameraData = renderingData.cameraData;
				passData2.cmd = renderingData.commandBuffer;
				passData2.copyResolvedDepth = this.m_CopyResolvedDepth;
				passData2.copyToDepth = this.CopyToDepth;
				passData2.source = renderGraphBuilder2.ReadTexture(in source);
				passData2.destination = renderGraphBuilder2.UseColorBuffer(in destination, 0);
				renderGraphBuilder2.AllowPassCulling(false);
				renderGraphBuilder2.SetRenderFunc<CopyDepthPass.PassData>(delegate(CopyDepthPass.PassData data, RenderGraphContext context)
				{
					CopyDepthPass.ExecutePass(context.renderContext, data, ref data.cmd, ref data.cameraData, data.source, data.destination);
				});
			}
			CopyDepthPass.PassData passData3;
			using (RenderGraphBuilder renderGraphBuilder3 = renderGraph.AddRenderPass<CopyDepthPass.PassData>("Setup Global Copy Depth", out passData3, base.profilingSampler))
			{
				passData3.cmd = renderingData.commandBuffer;
				passData3.destination = renderGraphBuilder3.UseColorBuffer(in destination, 0);
				renderGraphBuilder3.AllowPassCulling(false);
				renderGraphBuilder3.SetRenderFunc<CopyDepthPass.PassData>(delegate(CopyDepthPass.PassData data, RenderGraphContext context)
				{
					data.cmd.SetGlobalTexture("_CameraDepthTexture", data.destination);
				});
			}
		}

		// Token: 0x04000881 RID: 2177
		private Material m_CopyDepthMaterial;

		// Token: 0x04000882 RID: 2178
		internal bool m_CopyResolvedDepth;

		// Token: 0x04000883 RID: 2179
		internal bool m_ShouldClear;

		// Token: 0x04000884 RID: 2180
		private CopyDepthPass.PassData m_PassData;

		// Token: 0x020001E4 RID: 484
		private class PassData
		{
			// Token: 0x04000C92 RID: 3218
			internal TextureHandle source;

			// Token: 0x04000C93 RID: 3219
			internal TextureHandle destination;

			// Token: 0x04000C94 RID: 3220
			internal CommandBuffer cmd;

			// Token: 0x04000C95 RID: 3221
			internal CameraData cameraData;

			// Token: 0x04000C96 RID: 3222
			internal Material copyDepthMaterial;

			// Token: 0x04000C97 RID: 3223
			internal int msaaSamples;

			// Token: 0x04000C98 RID: 3224
			internal bool copyResolvedDepth;

			// Token: 0x04000C99 RID: 3225
			internal bool copyToDepth;
		}
	}
}
