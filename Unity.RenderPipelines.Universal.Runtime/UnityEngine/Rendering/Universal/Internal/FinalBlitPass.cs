using System;
using UnityEngine.Experimental.Rendering.RenderGraphModule;

namespace UnityEngine.Rendering.Universal.Internal
{
	// Token: 0x02000124 RID: 292
	public class FinalBlitPass : ScriptableRenderPass
	{
		// Token: 0x060009A6 RID: 2470 RVA: 0x00040354 File Offset: 0x0003E554
		public FinalBlitPass(RenderPassEvent evt, Material blitMaterial, Material blitHDRMaterial)
		{
			base.profilingSampler = new ProfilingSampler("FinalBlitPass");
			base.useNativeRenderPass = false;
			this.m_PassData = new FinalBlitPass.PassData();
			base.renderPassEvent = evt;
			this.m_BlitMaterialData = new FinalBlitPass.BlitMaterialData[2];
			for (int i = 0; i < 2; i++)
			{
				this.m_BlitMaterialData[i].material = ((i == 0) ? blitMaterial : blitHDRMaterial);
				FinalBlitPass.BlitMaterialData[] blitMaterialData = this.m_BlitMaterialData;
				int num = i;
				Material material = this.m_BlitMaterialData[i].material;
				blitMaterialData[num].nearestSamplerPass = ((material != null) ? material.FindPass("NearestDebugDraw") : (-1));
				FinalBlitPass.BlitMaterialData[] blitMaterialData2 = this.m_BlitMaterialData;
				int num2 = i;
				Material material2 = this.m_BlitMaterialData[i].material;
				blitMaterialData2[num2].bilinearSamplerPass = ((material2 != null) ? material2.FindPass("BilinearDebugDraw") : (-1));
			}
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x0004042C File Offset: 0x0003E62C
		public void Dispose()
		{
		}

		// Token: 0x060009A8 RID: 2472 RVA: 0x00040430 File Offset: 0x0003E630
		[Obsolete("Use RTHandles for colorHandle")]
		public void Setup(RenderTextureDescriptor baseDescriptor, RenderTargetHandle colorHandle)
		{
			RTHandle source = this.m_Source;
			RenderTargetIdentifier? renderTargetIdentifier = ((source != null) ? new RenderTargetIdentifier?(source.nameID) : null);
			RenderTargetIdentifier renderTargetIdentifier2 = colorHandle.Identifier();
			if (renderTargetIdentifier == null || (renderTargetIdentifier != null && renderTargetIdentifier.GetValueOrDefault() != renderTargetIdentifier2))
			{
				this.m_Source = RTHandles.Alloc(colorHandle.Identifier());
			}
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x0004049E File Offset: 0x0003E69E
		public void Setup(RenderTextureDescriptor baseDescriptor, RTHandle colorHandle)
		{
			this.m_Source = colorHandle;
		}

		// Token: 0x060009AA RID: 2474 RVA: 0x000404A7 File Offset: 0x0003E6A7
		private static void SetupHDROutput(ColorGamut hdrDisplayColorGamut, Material material, HDROutputUtils.Operation hdrOperation, Vector4 hdrOutputParameters)
		{
			material.SetVector(ShaderPropertyId.hdrOutputLuminanceParams, hdrOutputParameters);
			HDROutputUtils.ConfigureHDROutput(material, hdrDisplayColorGamut, hdrOperation);
		}

		// Token: 0x060009AB RID: 2475 RVA: 0x000404C0 File Offset: 0x0003E6C0
		public unsafe override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
		{
			ref CameraData ptr = ref renderingData.cameraData;
			DebugHandler activeDebugHandler = ScriptableRenderPass.GetActiveDebugHandler(ref renderingData);
			if (activeDebugHandler != null && activeDebugHandler.WriteToDebugScreenTexture(ref ptr))
			{
				base.ConfigureTarget(*activeDebugHandler.DebugScreenColorHandle, *activeDebugHandler.DebugScreenDepthHandle);
			}
		}

		// Token: 0x060009AC RID: 2476 RVA: 0x00040500 File Offset: 0x0003E700
		public unsafe override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			bool isHDROutputActive = renderingData.cameraData.isHDROutputActive;
			this.InitPassData(ref renderingData, ref this.m_PassData, isHDROutputActive ? FinalBlitPass.BlitType.HDR : FinalBlitPass.BlitType.Core);
			if (this.m_PassData.blitMaterialData.material == null)
			{
				Debug.LogErrorFormat("Missing {0}. {1} render pass will not execute. Check for missing reference in the renderer resources.", new object[]
				{
					this.m_PassData.blitMaterialData,
					base.GetType().Name
				});
				return;
			}
			ref CameraData ptr = ref renderingData.cameraData;
			RenderTargetIdentifier cameraTargetIdentifier = RenderingUtils.GetCameraTargetIdentifier(ref renderingData);
			DebugHandler activeDebugHandler = ScriptableRenderPass.GetActiveDebugHandler(ref renderingData);
			bool flag = activeDebugHandler != null && activeDebugHandler.WriteToDebugScreenTexture(ref ptr);
			RTHandleStaticHelpers.SetRTHandleStaticWrapper(cameraTargetIdentifier);
			RTHandle s_RTHandleWrapper = RTHandleStaticHelpers.s_RTHandleWrapper;
			CommandBuffer commandBuffer = renderingData.commandBuffer;
			if (this.m_Source == ptr.renderer.GetCameraColorFrontBuffer(commandBuffer))
			{
				this.m_Source = renderingData.cameraData.renderer.cameraColorTargetHandle;
			}
			using (new ProfilingScope(commandBuffer, ProfilingSampler.Get<URPProfileId>(URPProfileId.FinalBlit)))
			{
				this.m_PassData.blitMaterialData.material.enabledKeywords = null;
				CoreUtils.SetKeyword(commandBuffer, "_LINEAR_TO_SRGB_CONVERSION", this.m_PassData.requireSrgbConversion);
				if (isHDROutputActive)
				{
					Tonemapping component = VolumeManager.instance.stack.GetComponent<Tonemapping>();
					Vector4 vector;
					UniversalRenderPipeline.GetHDROutputLuminanceParameters(ptr.hdrDisplayInformation, ptr.hdrDisplayColorGamut, component, out vector);
					HDROutputUtils.Operation operation = HDROutputUtils.Operation.None;
					if (activeDebugHandler == null || !activeDebugHandler.HDRDebugViewIsActive(ref ptr))
					{
						operation |= HDROutputUtils.Operation.ColorEncoding;
					}
					if (!ptr.postProcessEnabled)
					{
						operation |= HDROutputUtils.Operation.ColorConversion;
					}
					FinalBlitPass.SetupHDROutput(ptr.hdrDisplayColorGamut, this.m_PassData.blitMaterialData.material, operation, vector);
				}
				if (flag)
				{
					RenderTexture rt = this.m_Source.rt;
					int num = ((rt != null && rt.filterMode == FilterMode.Bilinear) ? this.m_PassData.blitMaterialData.bilinearSamplerPass : this.m_PassData.blitMaterialData.nearestSamplerPass);
					Vector2 vector2 = (this.m_Source.useScaling ? new Vector2(this.m_Source.rtHandleProperties.rtHandleScale.x, this.m_Source.rtHandleProperties.rtHandleScale.y) : Vector2.one);
					Blitter.BlitTexture(commandBuffer, this.m_Source, vector2, this.m_PassData.blitMaterialData.material, num);
					ptr.renderer.ConfigureCameraTarget(*activeDebugHandler.DebugScreenColorHandle, *activeDebugHandler.DebugScreenDepthHandle);
				}
				else
				{
					FinalBlitPass.ExecutePass(ref renderingData, in this.m_PassData.blitMaterialData, s_RTHandleWrapper, this.m_Source);
					ptr.renderer.ConfigureCameraTarget(s_RTHandleWrapper, s_RTHandleWrapper);
				}
			}
		}

		// Token: 0x060009AD RID: 2477 RVA: 0x000407A0 File Offset: 0x0003E9A0
		private static void ExecutePass(ref RenderingData renderingData, in FinalBlitPass.BlitMaterialData blitMaterialData, RTHandle cameraTarget, RTHandle source)
		{
			CameraData cameraData = renderingData.cameraData;
			CommandBuffer commandBuffer = renderingData.commandBuffer;
			RenderBufferLoadAction renderBufferLoadAction = RenderBufferLoadAction.DontCare;
			if (!cameraData.isSceneViewCamera && !cameraData.isDefaultViewport)
			{
				renderBufferLoadAction = RenderBufferLoadAction.Load;
			}
			if (cameraData.xr.enabled)
			{
				renderBufferLoadAction = RenderBufferLoadAction.Load;
			}
			RenderTexture rt = source.rt;
			int num = ((rt != null && rt.filterMode == FilterMode.Bilinear) ? blitMaterialData.bilinearSamplerPass : blitMaterialData.nearestSamplerPass);
			RenderingUtils.FinalBlit(commandBuffer, ref cameraData, source, cameraTarget, renderBufferLoadAction, RenderBufferStoreAction.Store, blitMaterialData.material, num);
		}

		// Token: 0x060009AE RID: 2478 RVA: 0x00040817 File Offset: 0x0003EA17
		private void InitPassData(ref RenderingData renderingData, ref FinalBlitPass.PassData passData, FinalBlitPass.BlitType blitType)
		{
			passData.renderingData = renderingData;
			passData.requireSrgbConversion = renderingData.cameraData.requireSrgbConversion;
			passData.blitMaterialData = this.m_BlitMaterialData[(int)blitType];
		}

		// Token: 0x060009AF RID: 2479 RVA: 0x0004084C File Offset: 0x0003EA4C
		internal void Render(RenderGraph renderGraph, ref RenderingData renderingData, TextureHandle src, TextureHandle dest, TextureHandle overlayUITexture)
		{
			FinalBlitPass.PassData passData;
			using (RenderGraphBuilder renderGraphBuilder = renderGraph.AddRenderPass<FinalBlitPass.PassData>("Final Blit", out passData, base.profilingSampler))
			{
				bool isHDROutputActive = renderingData.cameraData.isHDROutputActive;
				passData.source = src;
				passData.destination = dest;
				passData.sourceID = ShaderPropertyId.sourceTex;
				this.InitPassData(ref renderingData, ref passData, isHDROutputActive ? FinalBlitPass.BlitType.HDR : FinalBlitPass.BlitType.Core);
				renderGraphBuilder.UseColorBuffer(in passData.destination, 0);
				renderGraphBuilder.ReadTexture(in passData.source);
				if (isHDROutputActive && overlayUITexture.IsValid())
				{
					Tonemapping component = VolumeManager.instance.stack.GetComponent<Tonemapping>();
					ref CameraData ptr = ref renderingData.cameraData;
					UniversalRenderPipeline.GetHDROutputLuminanceParameters(ptr.hdrDisplayInformation, ptr.hdrDisplayColorGamut, component, out passData.hdrOutputLuminanceParams);
					renderGraphBuilder.ReadTexture(in overlayUITexture);
				}
				else
				{
					passData.hdrOutputLuminanceParams = new Vector4(-1f, -1f, -1f, -1f);
				}
				renderGraphBuilder.SetRenderFunc<FinalBlitPass.PassData>(delegate(FinalBlitPass.PassData data, RenderGraphContext context)
				{
					data.blitMaterialData.material.enabledKeywords = null;
					CoreUtils.SetKeyword(context.cmd, "_LINEAR_TO_SRGB_CONVERSION", data.requireSrgbConversion);
					data.blitMaterialData.material.SetTexture(data.sourceID, data.source);
					DebugHandler activeDebugHandler = ScriptableRenderPass.GetActiveDebugHandler(ref data.renderingData);
					bool flag = activeDebugHandler != null && activeDebugHandler.WriteToDebugScreenTexture(ref data.renderingData.cameraData);
					if (data.hdrOutputLuminanceParams.w >= 0f)
					{
						HDROutputUtils.Operation operation = HDROutputUtils.Operation.None;
						if (activeDebugHandler == null || !activeDebugHandler.HDRDebugViewIsActive(ref data.renderingData.cameraData))
						{
							operation |= HDROutputUtils.Operation.ColorEncoding;
						}
						if (!data.renderingData.cameraData.postProcessEnabled)
						{
							operation |= HDROutputUtils.Operation.ColorConversion;
						}
						FinalBlitPass.SetupHDROutput(data.renderingData.cameraData.hdrDisplayColorGamut, data.blitMaterialData.material, operation, data.hdrOutputLuminanceParams);
					}
					if (flag)
					{
						RTHandle rthandle = data.source;
						Vector2 vector = (rthandle.useScaling ? new Vector2(rthandle.rtHandleProperties.rtHandleScale.x, rthandle.rtHandleProperties.rtHandleScale.y) : Vector2.one);
						RenderTexture rt = rthandle.rt;
						int num = ((rt != null && rt.filterMode == FilterMode.Bilinear) ? data.blitMaterialData.bilinearSamplerPass : data.blitMaterialData.nearestSamplerPass);
						Blitter.BlitTexture(context.cmd, rthandle, vector, data.blitMaterialData.material, num);
						return;
					}
					FinalBlitPass.ExecutePass(ref data.renderingData, in data.blitMaterialData, data.destination, data.source);
				});
			}
		}

		// Token: 0x040008A3 RID: 2211
		private RTHandle m_Source;

		// Token: 0x040008A4 RID: 2212
		private FinalBlitPass.PassData m_PassData;

		// Token: 0x040008A5 RID: 2213
		private FinalBlitPass.BlitMaterialData[] m_BlitMaterialData;

		// Token: 0x020001EE RID: 494
		private static class BlitPassNames
		{
			// Token: 0x04000CC0 RID: 3264
			public const string NearestSampler = "NearestDebugDraw";

			// Token: 0x04000CC1 RID: 3265
			public const string BilinearSampler = "BilinearDebugDraw";
		}

		// Token: 0x020001EF RID: 495
		private enum BlitType
		{
			// Token: 0x04000CC3 RID: 3267
			Core,
			// Token: 0x04000CC4 RID: 3268
			HDR,
			// Token: 0x04000CC5 RID: 3269
			Count
		}

		// Token: 0x020001F0 RID: 496
		private struct BlitMaterialData
		{
			// Token: 0x04000CC6 RID: 3270
			public Material material;

			// Token: 0x04000CC7 RID: 3271
			public int nearestSamplerPass;

			// Token: 0x04000CC8 RID: 3272
			public int bilinearSamplerPass;
		}

		// Token: 0x020001F1 RID: 497
		private class PassData
		{
			// Token: 0x04000CC9 RID: 3273
			internal TextureHandle source;

			// Token: 0x04000CCA RID: 3274
			internal TextureHandle destination;

			// Token: 0x04000CCB RID: 3275
			internal int sourceID;

			// Token: 0x04000CCC RID: 3276
			internal Vector4 hdrOutputLuminanceParams;

			// Token: 0x04000CCD RID: 3277
			internal bool requireSrgbConversion;

			// Token: 0x04000CCE RID: 3278
			internal FinalBlitPass.BlitMaterialData blitMaterialData;

			// Token: 0x04000CCF RID: 3279
			internal RenderingData renderingData;
		}
	}
}
