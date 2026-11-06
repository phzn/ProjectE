using System;
using System.Runtime.CompilerServices;
using UnityEngine.Experimental.Rendering;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000B3 RID: 179
	internal class PostProcessPass : ScriptableRenderPass
	{
		// Token: 0x060005C1 RID: 1473 RVA: 0x000214A4 File Offset: 0x0001F6A4
		public PostProcessPass(RenderPassEvent evt, PostProcessData data, ref PostProcessParams postProcessParams)
		{
			base.profilingSampler = new ProfilingSampler("PostProcessPass");
			base.renderPassEvent = evt;
			this.m_Data = data;
			this.m_Materials = new PostProcessPass.MaterialLibrary(data);
			if (SystemInfo.IsFormatSupported(GraphicsFormat.R8G8_UNorm, FormatUsage.Render) && SystemInfo.graphicsDeviceVendor.ToLowerInvariant().Contains("arm"))
			{
				this.m_SMAAEdgeFormat = GraphicsFormat.R8G8_UNorm;
			}
			else
			{
				this.m_SMAAEdgeFormat = GraphicsFormat.R8G8B8A8_UNorm;
			}
			if (SystemInfo.IsFormatSupported(GraphicsFormat.R16_UNorm, FormatUsage.Blend))
			{
				this.m_GaussianCoCFormat = GraphicsFormat.R16_UNorm;
			}
			else if (SystemInfo.IsFormatSupported(GraphicsFormat.R16_SFloat, FormatUsage.Blend))
			{
				this.m_GaussianCoCFormat = GraphicsFormat.R16_SFloat;
			}
			else
			{
				this.m_GaussianCoCFormat = GraphicsFormat.R8_UNorm;
			}
			PostProcessPass.ShaderConstants._BloomMipUp = new int[16];
			PostProcessPass.ShaderConstants._BloomMipDown = new int[16];
			this.m_BloomMipUp = new RTHandle[16];
			this.m_BloomMipDown = new RTHandle[16];
			for (int i = 0; i < 16; i++)
			{
				PostProcessPass.ShaderConstants._BloomMipUp[i] = Shader.PropertyToID("_BloomMipUp" + i.ToString());
				PostProcessPass.ShaderConstants._BloomMipDown[i] = Shader.PropertyToID("_BloomMipDown" + i.ToString());
				this.m_BloomMipUp[i] = RTHandles.Alloc(PostProcessPass.ShaderConstants._BloomMipUp[i], "_BloomMipUp" + i.ToString());
				this.m_BloomMipDown[i] = RTHandles.Alloc(PostProcessPass.ShaderConstants._BloomMipDown[i], "_BloomMipDown" + i.ToString());
			}
			this.m_MRT2 = new RenderTargetIdentifier[2];
			base.useNativeRenderPass = false;
			this.m_BlitMaterial = postProcessParams.blitMaterial;
			if (SystemInfo.IsFormatSupported(postProcessParams.requestHDRFormat, FormatUsage.Blend))
			{
				this.m_DefaultHDRFormat = postProcessParams.requestHDRFormat;
				this.m_UseRGBM = false;
				return;
			}
			if (SystemInfo.IsFormatSupported(GraphicsFormat.B10G11R11_UFloatPack32, FormatUsage.Blend))
			{
				this.m_DefaultHDRFormat = GraphicsFormat.B10G11R11_UFloatPack32;
				this.m_UseRGBM = false;
				return;
			}
			this.m_DefaultHDRFormat = ((QualitySettings.activeColorSpace == ColorSpace.Linear) ? GraphicsFormat.R8G8B8A8_SRGB : GraphicsFormat.R8G8B8A8_UNorm);
			this.m_UseRGBM = true;
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x00021686 File Offset: 0x0001F886
		public void Cleanup()
		{
			this.m_Materials.Cleanup();
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x00021694 File Offset: 0x0001F894
		public void Dispose()
		{
			foreach (RTHandle rthandle in this.m_BloomMipDown)
			{
				if (rthandle != null)
				{
					rthandle.Release();
				}
			}
			foreach (RTHandle rthandle2 in this.m_BloomMipUp)
			{
				if (rthandle2 != null)
				{
					rthandle2.Release();
				}
			}
			RTHandle scalingSetupTarget = this.m_ScalingSetupTarget;
			if (scalingSetupTarget != null)
			{
				scalingSetupTarget.Release();
			}
			RTHandle upscaledTarget = this.m_UpscaledTarget;
			if (upscaledTarget != null)
			{
				upscaledTarget.Release();
			}
			RTHandle fullCoCTexture = this.m_FullCoCTexture;
			if (fullCoCTexture != null)
			{
				fullCoCTexture.Release();
			}
			RTHandle halfCoCTexture = this.m_HalfCoCTexture;
			if (halfCoCTexture != null)
			{
				halfCoCTexture.Release();
			}
			RTHandle pingTexture = this.m_PingTexture;
			if (pingTexture != null)
			{
				pingTexture.Release();
			}
			RTHandle pongTexture = this.m_PongTexture;
			if (pongTexture != null)
			{
				pongTexture.Release();
			}
			RTHandle blendTexture = this.m_BlendTexture;
			if (blendTexture != null)
			{
				blendTexture.Release();
			}
			RTHandle edgeColorTexture = this.m_EdgeColorTexture;
			if (edgeColorTexture != null)
			{
				edgeColorTexture.Release();
			}
			RTHandle edgeStencilTexture = this.m_EdgeStencilTexture;
			if (edgeStencilTexture != null)
			{
				edgeStencilTexture.Release();
			}
			RTHandle tempTarget = this.m_TempTarget;
			if (tempTarget != null)
			{
				tempTarget.Release();
			}
			RTHandle tempTarget2 = this.m_TempTarget2;
			if (tempTarget2 == null)
			{
				return;
			}
			tempTarget2.Release();
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x000217A4 File Offset: 0x0001F9A4
		public void Setup(in RenderTextureDescriptor baseDescriptor, in RTHandle source, bool resolveToScreen, in RTHandle depth, in RTHandle internalLut, in RTHandle motionVectors, bool hasFinalPass, bool enableColorEncoding)
		{
			this.m_Descriptor = baseDescriptor;
			this.m_Descriptor.useMipMap = false;
			this.m_Descriptor.autoGenerateMips = false;
			this.m_Source = source;
			this.m_Depth = depth;
			this.m_InternalLut = internalLut;
			this.m_MotionVectors = motionVectors;
			this.m_IsFinalPass = false;
			this.m_HasFinalPass = hasFinalPass;
			this.m_EnableColorEncodingIfNeeded = enableColorEncoding;
			this.m_ResolveToScreen = resolveToScreen;
			this.m_Destination = ScriptableRenderPass.k_CameraTarget;
			this.m_UseSwapBuffer = true;
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x00021828 File Offset: 0x0001FA28
		public void SetupFinalPass(in RTHandle source, bool useSwapBuffer = false, bool enableColorEncoding = true)
		{
			this.m_Source = source;
			this.m_Destination = ScriptableRenderPass.k_CameraTarget;
			this.m_IsFinalPass = true;
			this.m_HasFinalPass = false;
			this.m_EnableColorEncodingIfNeeded = enableColorEncoding;
			this.m_UseSwapBuffer = useSwapBuffer;
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x00021859 File Offset: 0x0001FA59
		public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
		{
			base.overrideCameraTarget = true;
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x00021862 File Offset: 0x0001FA62
		public bool CanRunOnTile()
		{
			return false;
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x00021868 File Offset: 0x0001FA68
		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			VolumeStack stack = VolumeManager.instance.stack;
			this.m_DepthOfField = stack.GetComponent<DepthOfField>();
			this.m_MotionBlur = stack.GetComponent<MotionBlur>();
			this.m_PaniniProjection = stack.GetComponent<PaniniProjection>();
			this.m_Bloom = stack.GetComponent<Bloom>();
			this.m_LensDistortion = stack.GetComponent<LensDistortion>();
			this.m_ChromaticAberration = stack.GetComponent<ChromaticAberration>();
			this.m_Vignette = stack.GetComponent<Vignette>();
			this.m_ColorLookup = stack.GetComponent<ColorLookup>();
			this.m_ColorAdjustments = stack.GetComponent<ColorAdjustments>();
			this.m_Tonemapping = stack.GetComponent<Tonemapping>();
			this.m_FilmGrain = stack.GetComponent<FilmGrain>();
			this.m_UseFastSRGBLinearConversion = renderingData.postProcessingData.useFastSRGBLinearConversion;
			this.m_SupportDataDrivenLensFlare = renderingData.postProcessingData.supportDataDrivenLensFlare;
			CommandBuffer commandBuffer = renderingData.commandBuffer;
			if (this.m_IsFinalPass)
			{
				using (new ProfilingScope(commandBuffer, PostProcessPass.m_ProfilingRenderFinalPostProcessing))
				{
					this.RenderFinalPass(commandBuffer, ref renderingData);
					return;
				}
			}
			if (!this.CanRunOnTile())
			{
				using (new ProfilingScope(commandBuffer, PostProcessPass.m_ProfilingRenderPostProcessing))
				{
					this.Render(commandBuffer, ref renderingData);
				}
			}
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x000219A4 File Offset: 0x0001FBA4
		private RenderTextureDescriptor GetCompatibleDescriptor()
		{
			return this.GetCompatibleDescriptor(this.m_Descriptor.width, this.m_Descriptor.height, this.m_Descriptor.graphicsFormat, DepthBits.None);
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x000219CE File Offset: 0x0001FBCE
		private RenderTextureDescriptor GetCompatibleDescriptor(int width, int height, GraphicsFormat format, DepthBits depthBufferBits = DepthBits.None)
		{
			return PostProcessPass.GetCompatibleDescriptor(this.m_Descriptor, width, height, format, depthBufferBits);
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x000219E0 File Offset: 0x0001FBE0
		internal static RenderTextureDescriptor GetCompatibleDescriptor(RenderTextureDescriptor desc, int width, int height, GraphicsFormat format, DepthBits depthBufferBits = DepthBits.None)
		{
			desc.depthBufferBits = (int)depthBufferBits;
			desc.msaaSamples = 1;
			desc.width = width;
			desc.height = height;
			desc.graphicsFormat = format;
			return desc;
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x00021A0C File Offset: 0x0001FC0C
		private bool RequireSRGBConversionBlitToBackBuffer(ref CameraData cameraData)
		{
			return cameraData.requireSrgbConversion && this.m_EnableColorEncodingIfNeeded;
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x00021A1E File Offset: 0x0001FC1E
		private bool RequireHDROutput(ref CameraData cameraData)
		{
			return cameraData.isHDROutputActive && cameraData.captureActions == null;
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x00021A34 File Offset: 0x0001FC34
		private unsafe void Render(CommandBuffer cmd, ref RenderingData renderingData)
		{
			PostProcessPass.<>c__DisplayClass68_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.cmd = cmd;
			ref CameraData ptr = ref renderingData.cameraData;
			ref ScriptableRenderer ptr2 = ref ptr.renderer;
			bool isSceneViewCamera = ptr.isSceneViewCamera;
			bool flag = ptr.isStopNaNEnabled && this.m_Materials.stopNaN != null;
			bool flag2 = ptr.antialiasing == AntialiasingMode.SubpixelMorphologicalAntiAliasing && SystemInfo.graphicsDeviceType != GraphicsDeviceType.OpenGLES2;
			Material material = ((this.m_DepthOfField.mode.value == DepthOfFieldMode.Gaussian) ? this.m_Materials.gaussianDepthOfField : this.m_Materials.bokehDepthOfField);
			bool flag3 = this.m_DepthOfField.IsActive() && !isSceneViewCamera && material != null;
			bool flag4 = !LensFlareCommonSRP.Instance.IsEmpty() && this.m_SupportDataDrivenLensFlare;
			bool flag5 = this.m_MotionBlur.IsActive() && !isSceneViewCamera;
			bool flag6 = this.m_PaniniProjection.IsActive() && !isSceneViewCamera;
			flag5 = flag5 && Application.isPlaying;
			bool flag7 = ptr.IsTemporalAAEnabled();
			if (ptr.antialiasing == AntialiasingMode.TemporalAntiAliasing && !flag7)
			{
				TemporalAA.ValidateAndWarn(ref ptr);
			}
			CS$<>8__locals1.amountOfPassesRemaining = (flag ? 1 : 0) + (flag2 ? 1 : 0) + (flag3 ? 1 : 0) + (flag4 ? 1 : 0) + (flag7 ? 1 : 0) + (flag5 ? 1 : 0) + (flag6 ? 1 : 0);
			if (this.m_UseSwapBuffer && CS$<>8__locals1.amountOfPassesRemaining > 0)
			{
				ptr2.EnableSwapBufferMSAA(false);
			}
			CS$<>8__locals1.source = (this.m_UseSwapBuffer ? ptr2.cameraColorTargetHandle : this.m_Source);
			CS$<>8__locals1.destination = (this.m_UseSwapBuffer ? ptr2.GetCameraColorFrontBuffer(CS$<>8__locals1.cmd) : null);
			CS$<>8__locals1.cmd.SetGlobalMatrix(PostProcessPass.ShaderConstants._FullscreenProjMat, GL.GetGPUProjectionMatrix(Matrix4x4.identity, true));
			if (flag)
			{
				using (new ProfilingScope(CS$<>8__locals1.cmd, ProfilingSampler.Get<URPProfileId>(URPProfileId.StopNaNs)))
				{
					Blitter.BlitCameraTexture(CS$<>8__locals1.cmd, this.<Render>g__GetSource|68_0(ref CS$<>8__locals1), this.<Render>g__GetDestination|68_1(ref CS$<>8__locals1), RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, this.m_Materials.stopNaN, 0);
					this.<Render>g__Swap|68_2(ref ptr2, ref CS$<>8__locals1);
				}
			}
			if (flag2)
			{
				using (new ProfilingScope(CS$<>8__locals1.cmd, ProfilingSampler.Get<URPProfileId>(URPProfileId.SMAA)))
				{
					this.DoSubpixelMorphologicalAntialiasing(ref ptr, CS$<>8__locals1.cmd, this.<Render>g__GetSource|68_0(ref CS$<>8__locals1), this.<Render>g__GetDestination|68_1(ref CS$<>8__locals1));
					this.<Render>g__Swap|68_2(ref ptr2, ref CS$<>8__locals1);
				}
			}
			if (flag3)
			{
				URPProfileId urpprofileId = ((this.m_DepthOfField.mode.value == DepthOfFieldMode.Gaussian) ? URPProfileId.GaussianDepthOfField : URPProfileId.BokehDepthOfField);
				using (new ProfilingScope(CS$<>8__locals1.cmd, ProfilingSampler.Get<URPProfileId>(urpprofileId)))
				{
					this.DoDepthOfField(ptr.camera, CS$<>8__locals1.cmd, this.<Render>g__GetSource|68_0(ref CS$<>8__locals1), this.<Render>g__GetDestination|68_1(ref CS$<>8__locals1), ptr.pixelRect);
					this.<Render>g__Swap|68_2(ref ptr2, ref CS$<>8__locals1);
				}
			}
			if (flag7)
			{
				using (new ProfilingScope(CS$<>8__locals1.cmd, ProfilingSampler.Get<URPProfileId>(URPProfileId.TemporalAA)))
				{
					TemporalAA.ExecutePass(CS$<>8__locals1.cmd, this.m_Materials.temporalAntialiasing, ref ptr, CS$<>8__locals1.source, CS$<>8__locals1.destination, this.m_MotionVectors.rt);
					this.<Render>g__Swap|68_2(ref ptr2, ref CS$<>8__locals1);
				}
			}
			if (flag5)
			{
				using (new ProfilingScope(CS$<>8__locals1.cmd, ProfilingSampler.Get<URPProfileId>(URPProfileId.MotionBlur)))
				{
					this.DoMotionBlur(CS$<>8__locals1.cmd, this.<Render>g__GetSource|68_0(ref CS$<>8__locals1), this.<Render>g__GetDestination|68_1(ref CS$<>8__locals1), ref ptr);
					this.<Render>g__Swap|68_2(ref ptr2, ref CS$<>8__locals1);
				}
			}
			if (flag6)
			{
				using (new ProfilingScope(CS$<>8__locals1.cmd, ProfilingSampler.Get<URPProfileId>(URPProfileId.PaniniProjection)))
				{
					this.DoPaniniProjection(ptr.camera, CS$<>8__locals1.cmd, this.<Render>g__GetSource|68_0(ref CS$<>8__locals1), this.<Render>g__GetDestination|68_1(ref CS$<>8__locals1));
					this.<Render>g__Swap|68_2(ref ptr2, ref CS$<>8__locals1);
				}
			}
			using (new ProfilingScope(CS$<>8__locals1.cmd, ProfilingSampler.Get<URPProfileId>(URPProfileId.UberPostProcess)))
			{
				this.m_Materials.uber.shaderKeywords = null;
				if (this.m_Bloom.IsActive())
				{
					using (new ProfilingScope(CS$<>8__locals1.cmd, ProfilingSampler.Get<URPProfileId>(URPProfileId.Bloom)))
					{
						this.SetupBloom(CS$<>8__locals1.cmd, this.<Render>g__GetSource|68_0(ref CS$<>8__locals1), this.m_Materials.uber);
					}
				}
				if (flag4)
				{
					bool flag8;
					float num;
					float num2;
					if (this.m_PaniniProjection.IsActive())
					{
						flag8 = true;
						num = this.m_PaniniProjection.distance.value;
						num2 = this.m_PaniniProjection.cropToFit.value;
					}
					else
					{
						flag8 = false;
						num = 1f;
						num2 = 1f;
					}
					using (new ProfilingScope(CS$<>8__locals1.cmd, ProfilingSampler.Get<URPProfileId>(URPProfileId.LensFlareDataDrivenComputeOcclusion)))
					{
						this.LensFlareDataDrivenComputeOcclusion(ptr.camera, CS$<>8__locals1.cmd, this.<Render>g__GetSource|68_0(ref CS$<>8__locals1), flag8, num, num2);
					}
					using (new ProfilingScope(CS$<>8__locals1.cmd, ProfilingSampler.Get<URPProfileId>(URPProfileId.LensFlareDataDriven)))
					{
						this.LensFlareDataDriven(ptr.camera, CS$<>8__locals1.cmd, this.<Render>g__GetSource|68_0(ref CS$<>8__locals1), flag8, num, num2);
					}
				}
				this.SetupLensDistortion(this.m_Materials.uber, isSceneViewCamera);
				this.SetupChromaticAberration(this.m_Materials.uber);
				this.SetupVignette(this.m_Materials.uber, ptr.xr);
				this.SetupColorGrading(CS$<>8__locals1.cmd, ref renderingData, this.m_Materials.uber);
				this.SetupGrain(ref ptr, this.m_Materials.uber);
				this.SetupDithering(ref ptr, this.m_Materials.uber);
				if (this.RequireSRGBConversionBlitToBackBuffer(ref ptr))
				{
					this.m_Materials.uber.EnableKeyword("_LINEAR_TO_SRGB_CONVERSION");
				}
				if (this.RequireHDROutput(ref ptr))
				{
					HDROutputUtils.Operation operation = ((!this.m_HasFinalPass && this.m_EnableColorEncodingIfNeeded) ? HDROutputUtils.Operation.ColorEncoding : HDROutputUtils.Operation.None);
					this.SetupHDROutput(ptr.hdrDisplayInformation, ptr.hdrDisplayColorGamut, this.m_Materials.uber, operation);
				}
				if (this.m_UseFastSRGBLinearConversion)
				{
					this.m_Materials.uber.EnableKeyword("_USE_FAST_SRGB_LINEAR_CONVERSION");
				}
				DebugHandler activeDebugHandler = ScriptableRenderPass.GetActiveDebugHandler(ref renderingData);
				bool flag9 = activeDebugHandler != null && activeDebugHandler.WriteToDebugScreenTexture(ref ptr);
				RenderBufferLoadAction renderBufferLoadAction = RenderBufferLoadAction.DontCare;
				if (this.m_Destination == ScriptableRenderPass.k_CameraTarget && !ptr.isDefaultViewport)
				{
					renderBufferLoadAction = RenderBufferLoadAction.Load;
				}
				RenderTargetIdentifier renderTargetIdentifier = BuiltinRenderTextureType.CameraTarget;
				if (ptr.xr.enabled)
				{
					renderTargetIdentifier = ptr.xr.renderTarget;
				}
				if (!this.m_UseSwapBuffer)
				{
					this.m_ResolveToScreen = ptr.resolveFinalTarget || this.m_Destination.nameID == renderTargetIdentifier || this.m_HasFinalPass;
				}
				if (this.m_UseSwapBuffer && !this.m_ResolveToScreen)
				{
					if (!this.m_HasFinalPass)
					{
						ptr2.EnableSwapBufferMSAA(true);
						CS$<>8__locals1.destination = ptr2.GetCameraColorFrontBuffer(CS$<>8__locals1.cmd);
					}
					Blitter.BlitCameraTexture(CS$<>8__locals1.cmd, this.<Render>g__GetSource|68_0(ref CS$<>8__locals1), CS$<>8__locals1.destination, renderBufferLoadAction, RenderBufferStoreAction.Store, this.m_Materials.uber, 0);
					ptr2.ConfigureCameraColorTarget(CS$<>8__locals1.destination);
					this.<Render>g__Swap|68_2(ref ptr2, ref CS$<>8__locals1);
				}
				else if (!this.m_UseSwapBuffer)
				{
					RTHandle rthandle = this.<Render>g__GetSource|68_0(ref CS$<>8__locals1);
					Blitter.BlitCameraTexture(CS$<>8__locals1.cmd, rthandle, this.<Render>g__GetDestination|68_1(ref CS$<>8__locals1), renderBufferLoadAction, RenderBufferStoreAction.Store, this.m_Materials.uber, 0);
					CommandBuffer cmd2 = CS$<>8__locals1.cmd;
					RTHandle rthandle2 = this.<Render>g__GetDestination|68_1(ref CS$<>8__locals1);
					RTHandle destination = this.m_Destination;
					RenderBufferLoadAction renderBufferLoadAction2 = RenderBufferLoadAction.DontCare;
					RenderBufferStoreAction renderBufferStoreAction = RenderBufferStoreAction.Store;
					Material blitMaterial = this.m_BlitMaterial;
					RenderTexture rt = this.m_Destination.rt;
					Blitter.BlitCameraTexture(cmd2, rthandle2, destination, renderBufferLoadAction2, renderBufferStoreAction, blitMaterial, (rt != null && rt.filterMode == FilterMode.Bilinear) ? 1 : 0);
				}
				else if (this.m_ResolveToScreen)
				{
					if (flag9)
					{
						Blitter.BlitCameraTexture(CS$<>8__locals1.cmd, this.<Render>g__GetSource|68_0(ref CS$<>8__locals1), *activeDebugHandler.DebugScreenColorHandle, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store, this.m_Materials.uber, 0);
						ptr2.ConfigureCameraTarget(*activeDebugHandler.DebugScreenColorHandle, *activeDebugHandler.DebugScreenDepthHandle);
					}
					else
					{
						RTHandleStaticHelpers.SetRTHandleStaticWrapper((ptr.targetTexture != null) ? new RenderTargetIdentifier(ptr.targetTexture) : renderTargetIdentifier);
						RTHandle s_RTHandleWrapper = RTHandleStaticHelpers.s_RTHandleWrapper;
						RenderingUtils.FinalBlit(CS$<>8__locals1.cmd, ref ptr, this.<Render>g__GetSource|68_0(ref CS$<>8__locals1), s_RTHandleWrapper, renderBufferLoadAction, RenderBufferStoreAction.Store, this.m_Materials.uber, 0);
						ptr2.ConfigureCameraColorTarget(s_RTHandleWrapper);
					}
				}
			}
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x00022394 File Offset: 0x00020594
		private void DoSubpixelMorphologicalAntialiasing(ref CameraData cameraData, CommandBuffer cmd, RTHandle source, RTHandle destination)
		{
			Rect rect = new Rect(Vector2.zero, new Vector2((float)cameraData.cameraTargetDescriptor.width, (float)cameraData.cameraTargetDescriptor.height));
			Material subpixelMorphologicalAntialiasing = this.m_Materials.subpixelMorphologicalAntialiasing;
			RenderTextureDescriptor renderTextureDescriptor;
			RTHandle rthandle;
			if (this.m_Depth.nameID == BuiltinRenderTextureType.CameraTarget || this.m_Descriptor.msaaSamples > 1)
			{
				renderTextureDescriptor = this.GetCompatibleDescriptor(this.m_Descriptor.width, this.m_Descriptor.height, GraphicsFormat.None, DepthBits.Depth24);
				RenderingUtils.ReAllocateIfNeeded(ref this.m_EdgeStencilTexture, in renderTextureDescriptor, FilterMode.Bilinear, TextureWrapMode.Clamp, false, 1, 0f, "_EdgeStencilTexture");
				rthandle = this.m_EdgeStencilTexture;
			}
			else
			{
				rthandle = this.m_Depth;
			}
			renderTextureDescriptor = this.GetCompatibleDescriptor(this.m_Descriptor.width, this.m_Descriptor.height, this.m_SMAAEdgeFormat, DepthBits.None);
			RenderingUtils.ReAllocateIfNeeded(ref this.m_EdgeColorTexture, in renderTextureDescriptor, FilterMode.Bilinear, TextureWrapMode.Clamp, false, 1, 0f, "_EdgeColorTexture");
			renderTextureDescriptor = this.GetCompatibleDescriptor(this.m_Descriptor.width, this.m_Descriptor.height, GraphicsFormat.R8G8B8A8_UNorm, DepthBits.None);
			RenderingUtils.ReAllocateIfNeeded(ref this.m_BlendTexture, in renderTextureDescriptor, FilterMode.Point, TextureWrapMode.Clamp, false, 1, 0f, "_BlendTexture");
			Vector2Int vector2Int = (this.m_EdgeColorTexture.useScaling ? this.m_EdgeColorTexture.rtHandleProperties.currentRenderTargetSize : new Vector2Int(this.m_EdgeColorTexture.rt.width, this.m_EdgeColorTexture.rt.height));
			subpixelMorphologicalAntialiasing.SetVector(PostProcessPass.ShaderConstants._Metrics, new Vector4(1f / (float)vector2Int.x, 1f / (float)vector2Int.y, (float)vector2Int.x, (float)vector2Int.y));
			subpixelMorphologicalAntialiasing.SetTexture(PostProcessPass.ShaderConstants._AreaTexture, this.m_Data.textures.smaaAreaTex);
			subpixelMorphologicalAntialiasing.SetTexture(PostProcessPass.ShaderConstants._SearchTexture, this.m_Data.textures.smaaSearchTex);
			subpixelMorphologicalAntialiasing.SetFloat(PostProcessPass.ShaderConstants._StencilRef, 64f);
			subpixelMorphologicalAntialiasing.SetFloat(PostProcessPass.ShaderConstants._StencilMask, 64f);
			subpixelMorphologicalAntialiasing.shaderKeywords = null;
			switch (cameraData.antialiasingQuality)
			{
			case AntialiasingQuality.Low:
				subpixelMorphologicalAntialiasing.EnableKeyword("_SMAA_PRESET_LOW");
				break;
			case AntialiasingQuality.Medium:
				subpixelMorphologicalAntialiasing.EnableKeyword("_SMAA_PRESET_MEDIUM");
				break;
			case AntialiasingQuality.High:
				subpixelMorphologicalAntialiasing.EnableKeyword("_SMAA_PRESET_HIGH");
				break;
			}
			RenderingUtils.Blit(cmd, source, rect, this.m_EdgeColorTexture, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, rthandle, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, ClearFlag.ColorStencil, Color.clear, subpixelMorphologicalAntialiasing, 0);
			RenderingUtils.Blit(cmd, this.m_EdgeColorTexture, rect, this.m_BlendTexture, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, ClearFlag.Color, Color.clear, subpixelMorphologicalAntialiasing, 1);
			cmd.SetGlobalTexture(PostProcessPass.ShaderConstants._BlendTexture, this.m_BlendTexture.nameID);
			Blitter.BlitCameraTexture(cmd, source, destination, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, subpixelMorphologicalAntialiasing, 2);
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x00022644 File Offset: 0x00020844
		private void DoDepthOfField(Camera camera, CommandBuffer cmd, RTHandle source, RTHandle destination, Rect pixelRect)
		{
			if (this.m_DepthOfField.mode.value == DepthOfFieldMode.Gaussian)
			{
				this.DoGaussianDepthOfField(camera, cmd, source, destination, pixelRect);
				return;
			}
			if (this.m_DepthOfField.mode.value == DepthOfFieldMode.Bokeh)
			{
				this.DoBokehDepthOfField(cmd, source, destination, pixelRect);
			}
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x00022694 File Offset: 0x00020894
		private void DoGaussianDepthOfField(Camera camera, CommandBuffer cmd, RTHandle source, RTHandle destination, Rect pixelRect)
		{
			int num = 2;
			Material gaussianDepthOfField = this.m_Materials.gaussianDepthOfField;
			int num2 = this.m_Descriptor.width / num;
			int num3 = this.m_Descriptor.height / num;
			float value = this.m_DepthOfField.gaussianStart.value;
			float num4 = Mathf.Max(value, this.m_DepthOfField.gaussianEnd.value);
			float num5 = this.m_DepthOfField.gaussianMaxRadius.value * ((float)num2 / 1080f);
			num5 = Mathf.Min(num5, 2f);
			CoreUtils.SetKeyword(gaussianDepthOfField, "_HIGH_QUALITY_SAMPLING", this.m_DepthOfField.highQualitySampling.value);
			gaussianDepthOfField.SetVector(PostProcessPass.ShaderConstants._CoCParams, new Vector3(value, num4, num5));
			RenderTextureDescriptor renderTextureDescriptor = this.GetCompatibleDescriptor(this.m_Descriptor.width, this.m_Descriptor.height, this.m_GaussianCoCFormat, DepthBits.None);
			RenderingUtils.ReAllocateIfNeeded(ref this.m_FullCoCTexture, in renderTextureDescriptor, FilterMode.Bilinear, TextureWrapMode.Clamp, false, 1, 0f, "_FullCoCTexture");
			renderTextureDescriptor = this.GetCompatibleDescriptor(num2, num3, this.m_GaussianCoCFormat, DepthBits.None);
			RenderingUtils.ReAllocateIfNeeded(ref this.m_HalfCoCTexture, in renderTextureDescriptor, FilterMode.Bilinear, TextureWrapMode.Clamp, false, 1, 0f, "_HalfCoCTexture");
			renderTextureDescriptor = this.GetCompatibleDescriptor(num2, num3, GraphicsFormat.R16G16B16A16_SFloat, DepthBits.None);
			RenderingUtils.ReAllocateIfNeeded(ref this.m_PingTexture, in renderTextureDescriptor, FilterMode.Bilinear, TextureWrapMode.Clamp, false, 1, 0f, "_PingTexture");
			renderTextureDescriptor = this.GetCompatibleDescriptor(num2, num3, GraphicsFormat.R16G16B16A16_SFloat, DepthBits.None);
			RenderingUtils.ReAllocateIfNeeded(ref this.m_PongTexture, in renderTextureDescriptor, FilterMode.Bilinear, TextureWrapMode.Clamp, false, 1, 0f, "_PongTexture");
			PostProcessUtils.SetSourceSize(cmd, this.m_Descriptor);
			cmd.SetGlobalVector(PostProcessPass.ShaderConstants._DownSampleScaleFactor, new Vector4(1f / (float)num, 1f / (float)num, (float)num, (float)num));
			Blitter.BlitCameraTexture(cmd, source, this.m_FullCoCTexture, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, gaussianDepthOfField, 0);
			this.m_MRT2[0] = this.m_HalfCoCTexture.nameID;
			this.m_MRT2[1] = this.m_PingTexture.nameID;
			cmd.SetGlobalTexture(PostProcessPass.ShaderConstants._FullCoCTexture, this.m_FullCoCTexture.nameID);
			CoreUtils.SetRenderTarget(cmd, this.m_MRT2, this.m_HalfCoCTexture);
			Vector2 vector = (source.useScaling ? new Vector2(source.rtHandleProperties.rtHandleScale.x, source.rtHandleProperties.rtHandleScale.y) : Vector2.one);
			Blitter.BlitTexture(cmd, source, vector, gaussianDepthOfField, 1);
			cmd.SetGlobalTexture(PostProcessPass.ShaderConstants._HalfCoCTexture, this.m_HalfCoCTexture.nameID);
			cmd.SetGlobalTexture(PostProcessPass.ShaderConstants._ColorTexture, source);
			Blitter.BlitCameraTexture(cmd, this.m_PingTexture, this.m_PongTexture, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, gaussianDepthOfField, 2);
			Blitter.BlitCameraTexture(cmd, this.m_PongTexture, this.m_PingTexture, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, gaussianDepthOfField, 3);
			cmd.SetGlobalTexture(PostProcessPass.ShaderConstants._ColorTexture, this.m_PingTexture.nameID);
			cmd.SetGlobalTexture(PostProcessPass.ShaderConstants._FullCoCTexture, this.m_FullCoCTexture.nameID);
			Blitter.BlitCameraTexture(cmd, source, destination, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, gaussianDepthOfField, 4);
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x00022980 File Offset: 0x00020B80
		private void PrepareBokehKernel(float maxRadius, float rcpAspect)
		{
			if (this.m_BokehKernel == null)
			{
				this.m_BokehKernel = new Vector4[42];
			}
			int num = 0;
			float num2 = (float)this.m_DepthOfField.bladeCount.value;
			float num3 = 1f - this.m_DepthOfField.bladeCurvature.value;
			float num4 = this.m_DepthOfField.bladeRotation.value * 0.017453292f;
			for (int i = 1; i < 4; i++)
			{
				float num5 = 0.14285715f;
				float num6 = ((float)i + num5) / (3f + num5);
				int num7 = i * 7;
				for (int j = 0; j < num7; j++)
				{
					float num8 = 6.2831855f * (float)j / (float)num7;
					float num9 = Mathf.Cos(3.1415927f / num2);
					float num10 = Mathf.Cos(num8 - 6.2831855f / num2 * Mathf.Floor((num2 * num8 + 3.1415927f) / 6.2831855f));
					float num11 = num6 * Mathf.Pow(num9 / num10, num3);
					float num12 = num11 * Mathf.Cos(num8 - num4);
					float num13 = num11 * Mathf.Sin(num8 - num4);
					float num14 = num12 * maxRadius;
					float num15 = num13 * maxRadius;
					float num16 = num14 * num14;
					float num17 = num15 * num15;
					float num18 = Mathf.Sqrt(num16 + num17);
					float num19 = num14 * rcpAspect;
					this.m_BokehKernel[num] = new Vector4(num14, num15, num18, num19);
					num++;
				}
			}
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x00022ADB File Offset: 0x00020CDB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static float GetMaxBokehRadiusInPixels(float viewportHeight)
		{
			return Mathf.Min(0.05f, 14f / viewportHeight);
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x00022AF0 File Offset: 0x00020CF0
		private void DoBokehDepthOfField(CommandBuffer cmd, RTHandle source, RTHandle destination, Rect pixelRect)
		{
			int num = 2;
			Material bokehDepthOfField = this.m_Materials.bokehDepthOfField;
			int num2 = this.m_Descriptor.width / num;
			int num3 = this.m_Descriptor.height / num;
			float num4 = this.m_DepthOfField.focalLength.value / 1000f;
			float num5 = this.m_DepthOfField.focalLength.value / this.m_DepthOfField.aperture.value;
			float value = this.m_DepthOfField.focusDistance.value;
			float num6 = num5 * num4 / (value - num4);
			float maxBokehRadiusInPixels = PostProcessPass.GetMaxBokehRadiusInPixels((float)this.m_Descriptor.height);
			float num7 = 1f / ((float)num2 / (float)num3);
			CoreUtils.SetKeyword(bokehDepthOfField, "_USE_FAST_SRGB_LINEAR_CONVERSION", this.m_UseFastSRGBLinearConversion);
			cmd.SetGlobalVector(PostProcessPass.ShaderConstants._CoCParams, new Vector4(value, num6, maxBokehRadiusInPixels, num7));
			int hashCode = this.m_DepthOfField.GetHashCode();
			if (hashCode != this.m_BokehHash || maxBokehRadiusInPixels != this.m_BokehMaxRadius || num7 != this.m_BokehRCPAspect)
			{
				this.m_BokehHash = hashCode;
				this.m_BokehMaxRadius = maxBokehRadiusInPixels;
				this.m_BokehRCPAspect = num7;
				this.PrepareBokehKernel(maxBokehRadiusInPixels, num7);
			}
			cmd.SetGlobalVectorArray(PostProcessPass.ShaderConstants._BokehKernel, this.m_BokehKernel);
			RenderTextureDescriptor renderTextureDescriptor = this.GetCompatibleDescriptor(this.m_Descriptor.width, this.m_Descriptor.height, GraphicsFormat.R8_UNorm, DepthBits.None);
			RenderingUtils.ReAllocateIfNeeded(ref this.m_FullCoCTexture, in renderTextureDescriptor, FilterMode.Bilinear, TextureWrapMode.Clamp, false, 1, 0f, "_FullCoCTexture");
			renderTextureDescriptor = this.GetCompatibleDescriptor(num2, num3, GraphicsFormat.R16G16B16A16_SFloat, DepthBits.None);
			RenderingUtils.ReAllocateIfNeeded(ref this.m_PingTexture, in renderTextureDescriptor, FilterMode.Bilinear, TextureWrapMode.Clamp, false, 1, 0f, "_PingTexture");
			renderTextureDescriptor = this.GetCompatibleDescriptor(num2, num3, GraphicsFormat.R16G16B16A16_SFloat, DepthBits.None);
			RenderingUtils.ReAllocateIfNeeded(ref this.m_PongTexture, in renderTextureDescriptor, FilterMode.Bilinear, TextureWrapMode.Clamp, false, 1, 0f, "_PongTexture");
			PostProcessUtils.SetSourceSize(cmd, this.m_Descriptor);
			cmd.SetGlobalVector(PostProcessPass.ShaderConstants._DownSampleScaleFactor, new Vector4(1f / (float)num, 1f / (float)num, (float)num, (float)num));
			float num8 = 1f / (float)this.m_Descriptor.height * (float)num;
			cmd.SetGlobalVector(PostProcessPass.ShaderConstants._BokehConstants, new Vector4(num8, num8 * 2f));
			Blitter.BlitCameraTexture(cmd, source, this.m_FullCoCTexture, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, bokehDepthOfField, 0);
			cmd.SetGlobalTexture(PostProcessPass.ShaderConstants._FullCoCTexture, this.m_FullCoCTexture.nameID);
			Blitter.BlitCameraTexture(cmd, source, this.m_PingTexture, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, bokehDepthOfField, 1);
			Blitter.BlitCameraTexture(cmd, this.m_PingTexture, this.m_PongTexture, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, bokehDepthOfField, 2);
			Blitter.BlitCameraTexture(cmd, this.m_PongTexture, this.m_PingTexture, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, bokehDepthOfField, 3);
			cmd.SetGlobalTexture(PostProcessPass.ShaderConstants._DofTexture, this.m_PingTexture.nameID);
			Blitter.BlitCameraTexture(cmd, source, destination, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, bokehDepthOfField, 4);
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x00022D9C File Offset: 0x00020F9C
		private static float GetLensFlareLightAttenuation(Light light, Camera cam, Vector3 wo)
		{
			if (!(light != null))
			{
				return 1f;
			}
			switch (light.type)
			{
			case LightType.Spot:
				return LensFlareCommonSRP.ShapeAttenuationSpotConeLight(light.transform.forward, wo, light.spotAngle, light.innerSpotAngle / 180f);
			case LightType.Directional:
				return LensFlareCommonSRP.ShapeAttenuationDirLight(light.transform.forward, wo);
			case LightType.Point:
				return LensFlareCommonSRP.ShapeAttenuationPointLight();
			default:
				return 1f;
			}
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x00022E14 File Offset: 0x00021014
		private void LensFlareDataDrivenComputeOcclusion(Camera camera, CommandBuffer cmd, RenderTargetIdentifier source, bool usePanini, float paniniDistance, float paniniCropToFit)
		{
			Matrix4x4 matrix4x = GL.GetGPUProjectionMatrix(camera.projectionMatrix, true) * camera.worldToCameraMatrix;
			cmd.SetGlobalTexture(this.m_Depth.name, this.m_Depth.nameID);
			LensFlareCommonSRP.ComputeOcclusion(this.m_Materials.lensFlareDataDriven, camera, (float)this.m_Descriptor.width, (float)this.m_Descriptor.height, usePanini, paniniDistance, paniniCropToFit, true, camera.transform.position, matrix4x, cmd, false, false, null, null, PostProcessPass.ShaderConstants._FlareOcclusionTex, -1, PostProcessPass.ShaderConstants._FlareOcclusionIndex, PostProcessPass.ShaderConstants._FlareTex, PostProcessPass.ShaderConstants._FlareColorValue, -1, PostProcessPass.ShaderConstants._FlareData0, PostProcessPass.ShaderConstants._FlareData1, PostProcessPass.ShaderConstants._FlareData2, PostProcessPass.ShaderConstants._FlareData3, PostProcessPass.ShaderConstants._FlareData4);
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x00022EC8 File Offset: 0x000210C8
		private void LensFlareDataDriven(Camera camera, CommandBuffer cmd, RenderTargetIdentifier source, bool usePanini, float paniniDistance, float paniniCropToFit)
		{
			Matrix4x4 matrix4x = GL.GetGPUProjectionMatrix(camera.projectionMatrix, true) * camera.worldToCameraMatrix;
			LensFlareCommonSRP.DoLensFlareDataDrivenCommon(this.m_Materials.lensFlareDataDriven, camera, (float)this.m_Descriptor.width, (float)this.m_Descriptor.height, usePanini, paniniDistance, paniniCropToFit, true, camera.transform.position, matrix4x, cmd, false, false, null, null, source, (Light light, Camera cam, Vector3 wo) => PostProcessPass.GetLensFlareLightAttenuation(light, cam, wo), PostProcessPass.ShaderConstants._FlareOcclusionRemapTex, PostProcessPass.ShaderConstants._FlareOcclusionTex, PostProcessPass.ShaderConstants._FlareOcclusionIndex, 0, 0, PostProcessPass.ShaderConstants._FlareTex, PostProcessPass.ShaderConstants._FlareColorValue, PostProcessPass.ShaderConstants._FlareData0, PostProcessPass.ShaderConstants._FlareData1, PostProcessPass.ShaderConstants._FlareData2, PostProcessPass.ShaderConstants._FlareData3, PostProcessPass.ShaderConstants._FlareData4, false);
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x00022F84 File Offset: 0x00021184
		internal static void UpdateMotionBlurMatrices(ref Material material, Camera camera, XRPass xr)
		{
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
			if (xr.enabled && xr.singlePassEnabled)
			{
				material.SetMatrixArray(PostProcessPass.k_ShaderPropertyId_PrevViewProjMStereo, motionVectorsPersistentData.previousViewProjectionStereo);
				material.SetMatrixArray(PostProcessPass.k_ShaderPropertyId_ViewProjMStereo, motionVectorsPersistentData.viewProjectionStereo);
				return;
			}
			int num = 0;
			if (xr.enabled)
			{
				num = xr.multipassId;
			}
			material.SetMatrix(PostProcessPass.k_ShaderPropertyId_PrevViewProjM, motionVectorsPersistentData.previousViewProjectionStereo[num]);
			material.SetMatrix(PostProcessPass.k_ShaderPropertyId_ViewProjM, motionVectorsPersistentData.viewProjectionStereo[num]);
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x00023020 File Offset: 0x00021220
		private void DoMotionBlur(CommandBuffer cmd, RTHandle source, RTHandle destination, ref CameraData cameraData)
		{
			Material cameraMotionBlur = this.m_Materials.cameraMotionBlur;
			PostProcessPass.UpdateMotionBlurMatrices(ref cameraMotionBlur, cameraData.camera, cameraData.xr);
			cameraMotionBlur.SetFloat("_Intensity", this.m_MotionBlur.intensity.value);
			cameraMotionBlur.SetFloat("_Clamp", this.m_MotionBlur.clamp.value);
			PostProcessUtils.SetSourceSize(cmd, this.m_Descriptor);
			Blitter.BlitCameraTexture(cmd, source, destination, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, cameraMotionBlur, (int)this.m_MotionBlur.quality.value);
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x000230AC File Offset: 0x000212AC
		private void DoPaniniProjection(Camera camera, CommandBuffer cmd, RTHandle source, RTHandle destination)
		{
			float value = this.m_PaniniProjection.distance.value;
			Vector2 vector = this.CalcViewExtents(camera);
			Vector2 vector2 = this.CalcCropExtents(camera, value);
			float num = vector2.x / vector.x;
			float num2 = vector2.y / vector.y;
			float num3 = Mathf.Min(num, num2);
			float num4 = value;
			float num5 = Mathf.Lerp(1f, Mathf.Clamp01(num3), this.m_PaniniProjection.cropToFit.value);
			Material paniniProjection = this.m_Materials.paniniProjection;
			paniniProjection.SetVector(PostProcessPass.ShaderConstants._Params, new Vector4(vector.x, vector.y, num4, num5));
			paniniProjection.EnableKeyword((1f - Mathf.Abs(num4) > float.Epsilon) ? "_GENERIC" : "_UNIT_DISTANCE");
			Blitter.BlitCameraTexture(cmd, source, destination, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, paniniProjection, 0);
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x00023188 File Offset: 0x00021388
		private Vector2 CalcViewExtents(Camera camera)
		{
			float num = camera.fieldOfView * 0.017453292f;
			float num2 = (float)this.m_Descriptor.width / (float)this.m_Descriptor.height;
			float num3 = Mathf.Tan(0.5f * num);
			return new Vector2(num2 * num3, num3);
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x000231D0 File Offset: 0x000213D0
		private Vector2 CalcCropExtents(Camera camera, float d)
		{
			float num = 1f + d;
			Vector2 vector = this.CalcViewExtents(camera);
			float num2 = Mathf.Sqrt(vector.x * vector.x + 1f);
			float num3 = 1f / num2;
			float num4 = num3 + d;
			return vector * num3 * (num / num4);
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x00023224 File Offset: 0x00021424
		private void SetupBloom(CommandBuffer cmd, RTHandle source, Material uberMaterial)
		{
			BloomDownscaleMode value = this.m_Bloom.downscale.value;
			int num;
			if (value != BloomDownscaleMode.Half)
			{
				if (value != BloomDownscaleMode.Quarter)
				{
					throw new ArgumentOutOfRangeException();
				}
				num = 2;
			}
			else
			{
				num = 1;
			}
			int num2 = this.m_Descriptor.width >> num;
			int num3 = this.m_Descriptor.height >> num;
			int num4 = Mathf.Clamp(Mathf.FloorToInt(Mathf.Log((float)Mathf.Max(num2, num3), 2f) - 1f), 1, this.m_Bloom.maxIterations.value);
			float value2 = this.m_Bloom.clamp.value;
			float num5 = Mathf.GammaToLinearSpace(this.m_Bloom.threshold.value);
			float num6 = num5 * 0.5f;
			float num7 = Mathf.Lerp(0.05f, 0.95f, this.m_Bloom.scatter.value);
			Material bloom = this.m_Materials.bloom;
			bloom.SetVector(PostProcessPass.ShaderConstants._Params, new Vector4(num7, value2, num5, num6));
			CoreUtils.SetKeyword(bloom, "_BLOOM_HQ", this.m_Bloom.highQualityFiltering.value);
			CoreUtils.SetKeyword(bloom, "_USE_RGBM", this.m_UseRGBM);
			RenderTextureDescriptor compatibleDescriptor = this.GetCompatibleDescriptor(num2, num3, this.m_DefaultHDRFormat, DepthBits.None);
			for (int i = 0; i < num4; i++)
			{
				RenderingUtils.ReAllocateIfNeeded(ref this.m_BloomMipUp[i], in compatibleDescriptor, FilterMode.Bilinear, TextureWrapMode.Clamp, false, 1, 0f, this.m_BloomMipUp[i].name);
				RenderingUtils.ReAllocateIfNeeded(ref this.m_BloomMipDown[i], in compatibleDescriptor, FilterMode.Bilinear, TextureWrapMode.Clamp, false, 1, 0f, this.m_BloomMipDown[i].name);
				compatibleDescriptor.width = Mathf.Max(1, compatibleDescriptor.width >> 1);
				compatibleDescriptor.height = Mathf.Max(1, compatibleDescriptor.height >> 1);
			}
			Blitter.BlitCameraTexture(cmd, source, this.m_BloomMipDown[0], RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, bloom, 0);
			RTHandle rthandle = this.m_BloomMipDown[0];
			for (int j = 1; j < num4; j++)
			{
				Blitter.BlitCameraTexture(cmd, rthandle, this.m_BloomMipUp[j], RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, bloom, 1);
				Blitter.BlitCameraTexture(cmd, this.m_BloomMipUp[j], this.m_BloomMipDown[j], RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, bloom, 2);
				rthandle = this.m_BloomMipDown[j];
			}
			for (int k = num4 - 2; k >= 0; k--)
			{
				RTHandle rthandle2 = ((k == num4 - 2) ? this.m_BloomMipDown[k + 1] : this.m_BloomMipUp[k + 1]);
				RTHandle rthandle3 = this.m_BloomMipDown[k];
				RTHandle rthandle4 = this.m_BloomMipUp[k];
				cmd.SetGlobalTexture(PostProcessPass.ShaderConstants._SourceTexLowMip, rthandle2);
				Blitter.BlitCameraTexture(cmd, rthandle3, rthandle4, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store, bloom, 3);
			}
			Color color = this.m_Bloom.tint.value.linear;
			float num8 = ColorUtils.Luminance(in color);
			color = ((num8 > 0f) ? (color * (1f / num8)) : Color.white);
			Vector4 vector = new Vector4(this.m_Bloom.intensity.value, color.r, color.g, color.b);
			uberMaterial.SetVector(PostProcessPass.ShaderConstants._Bloom_Params, vector);
			uberMaterial.SetFloat(PostProcessPass.ShaderConstants._Bloom_RGBM, this.m_UseRGBM ? 1f : 0f);
			cmd.SetGlobalTexture(PostProcessPass.ShaderConstants._Bloom_Texture, this.m_BloomMipUp[0]);
			Texture texture = ((this.m_Bloom.dirtTexture.value == null) ? Texture2D.blackTexture : this.m_Bloom.dirtTexture.value);
			float num9 = (float)texture.width / (float)texture.height;
			float num10 = (float)this.m_Descriptor.width / (float)this.m_Descriptor.height;
			Vector4 vector2 = new Vector4(1f, 1f, 0f, 0f);
			float value3 = this.m_Bloom.dirtIntensity.value;
			if (num9 > num10)
			{
				vector2.x = num10 / num9;
				vector2.z = (1f - vector2.x) * 0.5f;
			}
			else if (num10 > num9)
			{
				vector2.y = num9 / num10;
				vector2.w = (1f - vector2.y) * 0.5f;
			}
			uberMaterial.SetVector(PostProcessPass.ShaderConstants._LensDirt_Params, vector2);
			uberMaterial.SetFloat(PostProcessPass.ShaderConstants._LensDirt_Intensity, value3);
			uberMaterial.SetTexture(PostProcessPass.ShaderConstants._LensDirt_Texture, texture);
			if (this.m_Bloom.highQualityFiltering.value)
			{
				uberMaterial.EnableKeyword((value3 > 0f) ? "_BLOOM_HQ_DIRT" : "_BLOOM_HQ");
				return;
			}
			uberMaterial.EnableKeyword((value3 > 0f) ? "_BLOOM_LQ_DIRT" : "_BLOOM_LQ");
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x000236E8 File Offset: 0x000218E8
		private void SetupLensDistortion(Material material, bool isSceneView)
		{
			float num = 1.6f * Mathf.Max(Mathf.Abs(this.m_LensDistortion.intensity.value * 100f), 1f);
			float num2 = 0.017453292f * Mathf.Min(160f, num);
			float num3 = 2f * Mathf.Tan(num2 * 0.5f);
			Vector2 vector = this.m_LensDistortion.center.value * 2f - Vector2.one;
			Vector4 vector2 = new Vector4(vector.x, vector.y, Mathf.Max(this.m_LensDistortion.xMultiplier.value, 0.0001f), Mathf.Max(this.m_LensDistortion.yMultiplier.value, 0.0001f));
			Vector4 vector3 = new Vector4((this.m_LensDistortion.intensity.value >= 0f) ? num2 : (1f / num2), num3, 1f / this.m_LensDistortion.scale.value, this.m_LensDistortion.intensity.value * 100f);
			material.SetVector(PostProcessPass.ShaderConstants._Distortion_Params1, vector2);
			material.SetVector(PostProcessPass.ShaderConstants._Distortion_Params2, vector3);
			if (this.m_LensDistortion.IsActive() && !isSceneView)
			{
				material.EnableKeyword("_DISTORTION");
			}
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x0002383C File Offset: 0x00021A3C
		private void SetupChromaticAberration(Material material)
		{
			material.SetFloat(PostProcessPass.ShaderConstants._Chroma_Params, this.m_ChromaticAberration.intensity.value * 0.05f);
			if (this.m_ChromaticAberration.IsActive())
			{
				material.EnableKeyword("_CHROMATIC_ABERRATION");
			}
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x00023878 File Offset: 0x00021A78
		private void SetupVignette(Material material, XRPass xrPass)
		{
			Color value = this.m_Vignette.color.value;
			Vector2 vector = this.m_Vignette.center.value;
			float num = (float)this.m_Descriptor.width / (float)this.m_Descriptor.height;
			if (xrPass != null && xrPass.enabled)
			{
				if (xrPass.singlePassEnabled)
				{
					material.SetVector(PostProcessPass.ShaderConstants._Vignette_ParamsXR, xrPass.ApplyXRViewCenterOffset(vector));
				}
				else
				{
					vector = xrPass.ApplyXRViewCenterOffset(vector);
				}
			}
			Vector4 vector2 = new Vector4(value.r, value.g, value.b, this.m_Vignette.rounded.value ? num : 1f);
			Vector4 vector3 = new Vector4(vector.x, vector.y, this.m_Vignette.intensity.value * 3f, this.m_Vignette.smoothness.value * 5f);
			material.SetVector(PostProcessPass.ShaderConstants._Vignette_Params1, vector2);
			material.SetVector(PostProcessPass.ShaderConstants._Vignette_Params2, vector3);
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x00023980 File Offset: 0x00021B80
		private void SetupColorGrading(CommandBuffer cmd, ref RenderingData renderingData, Material material)
		{
			bool flag = renderingData.postProcessingData.gradingMode == ColorGradingMode.HighDynamicRange;
			int lutSize = renderingData.postProcessingData.lutSize;
			int num = lutSize * lutSize;
			float num2 = Mathf.Pow(2f, this.m_ColorAdjustments.postExposure.value);
			cmd.SetGlobalTexture(PostProcessPass.ShaderConstants._InternalLut, this.m_InternalLut.nameID);
			material.SetVector(PostProcessPass.ShaderConstants._Lut_Params, new Vector4(1f / (float)num, 1f / (float)lutSize, (float)lutSize - 1f, num2));
			material.SetTexture(PostProcessPass.ShaderConstants._UserLut, this.m_ColorLookup.texture.value);
			material.SetVector(PostProcessPass.ShaderConstants._UserLut_Params, (!this.m_ColorLookup.IsActive()) ? Vector4.zero : new Vector4(1f / (float)this.m_ColorLookup.texture.value.width, 1f / (float)this.m_ColorLookup.texture.value.height, (float)this.m_ColorLookup.texture.value.height - 1f, this.m_ColorLookup.contribution.value));
			if (flag)
			{
				material.EnableKeyword("_HDR_GRADING");
				return;
			}
			TonemappingMode value = this.m_Tonemapping.mode.value;
			if (value == TonemappingMode.Neutral)
			{
				material.EnableKeyword("_TONEMAP_NEUTRAL");
				return;
			}
			if (value != TonemappingMode.ACES)
			{
				return;
			}
			material.EnableKeyword("_TONEMAP_ACES");
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x00023AE7 File Offset: 0x00021CE7
		private void SetupGrain(ref CameraData cameraData, Material material)
		{
			if (!this.m_HasFinalPass && this.m_FilmGrain.IsActive())
			{
				material.EnableKeyword("_FILM_GRAIN");
				PostProcessUtils.ConfigureFilmGrain(this.m_Data, this.m_FilmGrain, cameraData.pixelWidth, cameraData.pixelHeight, material);
			}
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x00023B28 File Offset: 0x00021D28
		private void SetupDithering(ref CameraData cameraData, Material material)
		{
			if (!this.m_HasFinalPass && cameraData.isDitheringEnabled)
			{
				material.EnableKeyword("_DITHERING");
				this.m_DitheringTextureIndex = PostProcessUtils.ConfigureDithering(this.m_Data, this.m_DitheringTextureIndex, cameraData.pixelWidth, cameraData.pixelHeight, material);
			}
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x00023B74 File Offset: 0x00021D74
		private void SetupHDROutput(HDROutputUtils.HDRDisplayInformation hdrDisplayInformation, ColorGamut hdrDisplayColorGamut, Material material, HDROutputUtils.Operation hdrOperations)
		{
			Vector4 vector;
			UniversalRenderPipeline.GetHDROutputLuminanceParameters(hdrDisplayInformation, hdrDisplayColorGamut, this.m_Tonemapping, out vector);
			material.SetVector(ShaderPropertyId.hdrOutputLuminanceParams, vector);
			HDROutputUtils.ConfigureHDROutput(material, hdrDisplayColorGamut, hdrOperations);
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x00023BA8 File Offset: 0x00021DA8
		private unsafe void RenderFinalPass(CommandBuffer cmd, ref RenderingData renderingData)
		{
			ref CameraData ptr = ref renderingData.cameraData;
			Material finalPass = this.m_Materials.finalPass;
			finalPass.shaderKeywords = null;
			PostProcessUtils.SetSourceSize(cmd, ptr.cameraTargetDescriptor);
			this.SetupGrain(ref ptr, finalPass);
			this.SetupDithering(ref ptr, finalPass);
			if (this.RequireSRGBConversionBlitToBackBuffer(ref ptr))
			{
				finalPass.EnableKeyword("_LINEAR_TO_SRGB_CONVERSION");
			}
			bool flag = this.RequireHDROutput(ref ptr);
			if (flag)
			{
				HDROutputUtils.Operation operation = (this.m_EnableColorEncodingIfNeeded ? HDROutputUtils.Operation.ColorEncoding : HDROutputUtils.Operation.None);
				if (!ptr.postProcessEnabled)
				{
					operation |= HDROutputUtils.Operation.ColorConversion;
				}
				this.SetupHDROutput(ptr.hdrDisplayInformation, ptr.hdrDisplayColorGamut, finalPass, operation);
			}
			DebugHandler activeDebugHandler = ScriptableRenderPass.GetActiveDebugHandler(ref renderingData);
			bool flag2 = activeDebugHandler != null && activeDebugHandler.WriteToDebugScreenTexture(ref ptr);
			if (this.m_UseSwapBuffer)
			{
				this.m_Source = ptr.renderer.GetCameraColorBackBuffer(cmd);
			}
			RTHandle rthandle = this.m_Source;
			RenderBufferLoadAction renderBufferLoadAction = (ptr.isDefaultViewport ? RenderBufferLoadAction.DontCare : RenderBufferLoadAction.Load);
			bool flag3 = ptr.antialiasing == AntialiasingMode.FastApproximateAntialiasing;
			bool flag4 = ptr.imageScalingMode == ImageScalingMode.Upscaling && ptr.upscalingFilter == ImageUpscalingFilter.FSR;
			bool flag5 = ptr.IsTemporalAAEnabled() && ptr.taaSettings.contrastAdaptiveSharpening > 0f && !flag4;
			if (ptr.imageScalingMode != ImageScalingMode.None)
			{
				bool flag6 = flag3 || flag4;
				RenderTextureDescriptor cameraTargetDescriptor = ptr.cameraTargetDescriptor;
				cameraTargetDescriptor.msaaSamples = 1;
				cameraTargetDescriptor.depthBufferBits = 0;
				if (!flag)
				{
					cameraTargetDescriptor.graphicsFormat = UniversalRenderPipeline.MakeUnormRenderTextureGraphicsFormat();
				}
				this.m_Materials.scalingSetup.shaderKeywords = null;
				if (flag6)
				{
					if (flag)
					{
						this.SetupHDROutput(ptr.hdrDisplayInformation, ptr.hdrDisplayColorGamut, this.m_Materials.scalingSetup, HDROutputUtils.Operation.None);
					}
					if (flag3 && flag4)
					{
						this.m_Materials.scalingSetup.EnableKeyword("_FXAA_AND_GAMMA_20");
					}
					else if (flag3)
					{
						this.m_Materials.scalingSetup.EnableKeyword("_FXAA");
					}
					else if (flag4)
					{
						this.m_Materials.scalingSetup.EnableKeyword("_GAMMA_20");
					}
					RenderingUtils.ReAllocateIfNeeded(ref this.m_ScalingSetupTarget, in cameraTargetDescriptor, FilterMode.Point, TextureWrapMode.Clamp, false, 1, 0f, "_ScalingSetupTexture");
					Blitter.BlitCameraTexture(cmd, this.m_Source, this.m_ScalingSetupTarget, renderBufferLoadAction, RenderBufferStoreAction.Store, this.m_Materials.scalingSetup, 0);
					rthandle = this.m_ScalingSetupTarget;
				}
				ImageScalingMode imageScalingMode = ptr.imageScalingMode;
				if (imageScalingMode != ImageScalingMode.Upscaling)
				{
					if (imageScalingMode == ImageScalingMode.Downscaling)
					{
						flag5 = false;
					}
				}
				else
				{
					switch (ptr.upscalingFilter)
					{
					case ImageUpscalingFilter.Point:
						if (!flag5)
						{
							finalPass.EnableKeyword("_POINT_SAMPLING");
						}
						break;
					case ImageUpscalingFilter.FSR:
					{
						this.m_Materials.easu.shaderKeywords = null;
						RenderTextureDescriptor cameraTargetDescriptor2 = ptr.cameraTargetDescriptor;
						cameraTargetDescriptor2.msaaSamples = 1;
						cameraTargetDescriptor2.depthBufferBits = 0;
						cameraTargetDescriptor2.width = ptr.pixelWidth;
						cameraTargetDescriptor2.height = ptr.pixelHeight;
						RenderingUtils.ReAllocateIfNeeded(ref this.m_UpscaledTarget, in cameraTargetDescriptor2, FilterMode.Point, TextureWrapMode.Clamp, false, 1, 0f, "_UpscaledTexture");
						Vector2 vector = new Vector2((float)ptr.cameraTargetDescriptor.width, (float)ptr.cameraTargetDescriptor.height);
						Vector2 vector2 = new Vector2((float)ptr.pixelWidth, (float)ptr.pixelHeight);
						FSRUtils.SetEasuConstants(cmd, vector, vector, vector2);
						Blitter.BlitCameraTexture(cmd, rthandle, this.m_UpscaledTarget, renderBufferLoadAction, RenderBufferStoreAction.Store, this.m_Materials.easu, 0);
						float num = (ptr.fsrOverrideSharpness ? ptr.fsrSharpness : 0.92f);
						if (ptr.fsrSharpness > 0f)
						{
							finalPass.EnableKeyword(flag ? "_EASU_RCAS_AND_HDR_INPUT" : "_RCAS");
							FSRUtils.SetRcasConstantsLinear(cmd, num);
						}
						rthandle = this.m_UpscaledTarget;
						PostProcessUtils.SetSourceSize(cmd, cameraTargetDescriptor2);
						break;
					}
					}
				}
			}
			else if (flag3)
			{
				finalPass.EnableKeyword("_FXAA");
			}
			if (flag5)
			{
				finalPass.EnableKeyword("_RCAS");
				FSRUtils.SetRcasConstantsLinear(cmd, ptr.taaSettings.contrastAdaptiveSharpening);
			}
			RenderTargetIdentifier cameraTargetIdentifier = RenderingUtils.GetCameraTargetIdentifier(ref renderingData);
			if (flag2)
			{
				Blitter.BlitCameraTexture(cmd, rthandle, *activeDebugHandler.DebugScreenColorHandle, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store, finalPass, 0);
				ptr.renderer.ConfigureCameraTarget(*activeDebugHandler.DebugScreenColorHandle, *activeDebugHandler.DebugScreenDepthHandle);
				return;
			}
			RTHandleStaticHelpers.SetRTHandleStaticWrapper(cameraTargetIdentifier);
			RTHandle s_RTHandleWrapper = RTHandleStaticHelpers.s_RTHandleWrapper;
			RenderingUtils.FinalBlit(cmd, ref ptr, rthandle, s_RTHandleWrapper, renderBufferLoadAction, RenderBufferStoreAction.Store, finalPass, 0);
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x00024023 File Offset: 0x00022223
		[CompilerGenerated]
		private RTHandle <Render>g__GetSource|68_0(ref PostProcessPass.<>c__DisplayClass68_0 A_1)
		{
			return A_1.source;
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x0002402C File Offset: 0x0002222C
		[CompilerGenerated]
		private RTHandle <Render>g__GetDestination|68_1(ref PostProcessPass.<>c__DisplayClass68_0 A_1)
		{
			if (A_1.destination == null)
			{
				RenderTextureDescriptor renderTextureDescriptor = this.GetCompatibleDescriptor();
				RenderingUtils.ReAllocateIfNeeded(ref this.m_TempTarget, in renderTextureDescriptor, FilterMode.Bilinear, TextureWrapMode.Clamp, false, 1, 0f, "_TempTarget");
				A_1.destination = this.m_TempTarget;
			}
			else if (A_1.destination == this.m_Source && this.m_Descriptor.msaaSamples > 1)
			{
				RenderTextureDescriptor renderTextureDescriptor = this.GetCompatibleDescriptor();
				RenderingUtils.ReAllocateIfNeeded(ref this.m_TempTarget2, in renderTextureDescriptor, FilterMode.Bilinear, TextureWrapMode.Clamp, false, 1, 0f, "_TempTarget2");
				A_1.destination = this.m_TempTarget2;
			}
			return A_1.destination;
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x000240C4 File Offset: 0x000222C4
		[CompilerGenerated]
		private void <Render>g__Swap|68_2(ref ScriptableRenderer r, ref PostProcessPass.<>c__DisplayClass68_0 A_2)
		{
			int num = A_2.amountOfPassesRemaining - 1;
			A_2.amountOfPassesRemaining = num;
			if (this.m_UseSwapBuffer)
			{
				r.SwapColorBuffer(A_2.cmd);
				A_2.source = r.cameraColorTargetHandle;
				if (A_2.amountOfPassesRemaining == 0 && !this.m_HasFinalPass)
				{
					r.EnableSwapBufferMSAA(true);
				}
				A_2.destination = r.GetCameraColorFrontBuffer(A_2.cmd);
				return;
			}
			CoreUtils.Swap<RTHandle>(ref A_2.source, ref A_2.destination);
		}

		// Token: 0x0400045F RID: 1119
		private RenderTextureDescriptor m_Descriptor;

		// Token: 0x04000460 RID: 1120
		private RTHandle m_Source;

		// Token: 0x04000461 RID: 1121
		private RTHandle m_Destination;

		// Token: 0x04000462 RID: 1122
		private RTHandle m_Depth;

		// Token: 0x04000463 RID: 1123
		private RTHandle m_InternalLut;

		// Token: 0x04000464 RID: 1124
		private RTHandle m_MotionVectors;

		// Token: 0x04000465 RID: 1125
		private RTHandle m_FullCoCTexture;

		// Token: 0x04000466 RID: 1126
		private RTHandle m_HalfCoCTexture;

		// Token: 0x04000467 RID: 1127
		private RTHandle m_PingTexture;

		// Token: 0x04000468 RID: 1128
		private RTHandle m_PongTexture;

		// Token: 0x04000469 RID: 1129
		private RTHandle[] m_BloomMipDown;

		// Token: 0x0400046A RID: 1130
		private RTHandle[] m_BloomMipUp;

		// Token: 0x0400046B RID: 1131
		private RTHandle m_BlendTexture;

		// Token: 0x0400046C RID: 1132
		private RTHandle m_EdgeColorTexture;

		// Token: 0x0400046D RID: 1133
		private RTHandle m_EdgeStencilTexture;

		// Token: 0x0400046E RID: 1134
		private RTHandle m_TempTarget;

		// Token: 0x0400046F RID: 1135
		private RTHandle m_TempTarget2;

		// Token: 0x04000470 RID: 1136
		private const string k_RenderPostProcessingTag = "Render PostProcessing Effects";

		// Token: 0x04000471 RID: 1137
		private const string k_RenderFinalPostProcessingTag = "Render Final PostProcessing Pass";

		// Token: 0x04000472 RID: 1138
		private static readonly ProfilingSampler m_ProfilingRenderPostProcessing = new ProfilingSampler("Render PostProcessing Effects");

		// Token: 0x04000473 RID: 1139
		private static readonly ProfilingSampler m_ProfilingRenderFinalPostProcessing = new ProfilingSampler("Render Final PostProcessing Pass");

		// Token: 0x04000474 RID: 1140
		private PostProcessPass.MaterialLibrary m_Materials;

		// Token: 0x04000475 RID: 1141
		private PostProcessData m_Data;

		// Token: 0x04000476 RID: 1142
		private DepthOfField m_DepthOfField;

		// Token: 0x04000477 RID: 1143
		private MotionBlur m_MotionBlur;

		// Token: 0x04000478 RID: 1144
		private PaniniProjection m_PaniniProjection;

		// Token: 0x04000479 RID: 1145
		private Bloom m_Bloom;

		// Token: 0x0400047A RID: 1146
		private LensDistortion m_LensDistortion;

		// Token: 0x0400047B RID: 1147
		private ChromaticAberration m_ChromaticAberration;

		// Token: 0x0400047C RID: 1148
		private Vignette m_Vignette;

		// Token: 0x0400047D RID: 1149
		private ColorLookup m_ColorLookup;

		// Token: 0x0400047E RID: 1150
		private ColorAdjustments m_ColorAdjustments;

		// Token: 0x0400047F RID: 1151
		private Tonemapping m_Tonemapping;

		// Token: 0x04000480 RID: 1152
		private FilmGrain m_FilmGrain;

		// Token: 0x04000481 RID: 1153
		private const int k_MaxPyramidSize = 16;

		// Token: 0x04000482 RID: 1154
		private readonly GraphicsFormat m_DefaultHDRFormat;

		// Token: 0x04000483 RID: 1155
		private bool m_UseRGBM;

		// Token: 0x04000484 RID: 1156
		private readonly GraphicsFormat m_SMAAEdgeFormat;

		// Token: 0x04000485 RID: 1157
		private readonly GraphicsFormat m_GaussianCoCFormat;

		// Token: 0x04000486 RID: 1158
		private int m_DitheringTextureIndex;

		// Token: 0x04000487 RID: 1159
		private RenderTargetIdentifier[] m_MRT2;

		// Token: 0x04000488 RID: 1160
		private Vector4[] m_BokehKernel;

		// Token: 0x04000489 RID: 1161
		private int m_BokehHash;

		// Token: 0x0400048A RID: 1162
		private float m_BokehMaxRadius;

		// Token: 0x0400048B RID: 1163
		private float m_BokehRCPAspect;

		// Token: 0x0400048C RID: 1164
		private bool m_IsFinalPass;

		// Token: 0x0400048D RID: 1165
		private bool m_HasFinalPass;

		// Token: 0x0400048E RID: 1166
		private bool m_EnableColorEncodingIfNeeded;

		// Token: 0x0400048F RID: 1167
		private bool m_UseFastSRGBLinearConversion;

		// Token: 0x04000490 RID: 1168
		private bool m_SupportDataDrivenLensFlare;

		// Token: 0x04000491 RID: 1169
		private bool m_ResolveToScreen;

		// Token: 0x04000492 RID: 1170
		private bool m_UseSwapBuffer;

		// Token: 0x04000493 RID: 1171
		private RTHandle m_ScalingSetupTarget;

		// Token: 0x04000494 RID: 1172
		private RTHandle m_UpscaledTarget;

		// Token: 0x04000495 RID: 1173
		private Material m_BlitMaterial;

		// Token: 0x04000496 RID: 1174
		internal static readonly int k_ShaderPropertyId_ViewProjM = Shader.PropertyToID("_ViewProjM");

		// Token: 0x04000497 RID: 1175
		internal static readonly int k_ShaderPropertyId_PrevViewProjM = Shader.PropertyToID("_PrevViewProjM");

		// Token: 0x04000498 RID: 1176
		internal static readonly int k_ShaderPropertyId_ViewProjMStereo = Shader.PropertyToID("_ViewProjMStereo");

		// Token: 0x04000499 RID: 1177
		internal static readonly int k_ShaderPropertyId_PrevViewProjMStereo = Shader.PropertyToID("_PrevViewProjMStereo");

		// Token: 0x020001A4 RID: 420
		private class MaterialLibrary
		{
			// Token: 0x06000AFD RID: 2813 RVA: 0x00049FC8 File Offset: 0x000481C8
			public MaterialLibrary(PostProcessData data)
			{
				this.stopNaN = this.Load(data.shaders.stopNanPS);
				this.subpixelMorphologicalAntialiasing = this.Load(data.shaders.subpixelMorphologicalAntialiasingPS);
				this.gaussianDepthOfField = this.Load(data.shaders.gaussianDepthOfFieldPS);
				this.bokehDepthOfField = this.Load(data.shaders.bokehDepthOfFieldPS);
				this.cameraMotionBlur = this.Load(data.shaders.cameraMotionBlurPS);
				this.paniniProjection = this.Load(data.shaders.paniniProjectionPS);
				this.bloom = this.Load(data.shaders.bloomPS);
				this.temporalAntialiasing = this.Load(data.shaders.temporalAntialiasingPS);
				this.scalingSetup = this.Load(data.shaders.scalingSetupPS);
				this.easu = this.Load(data.shaders.easuPS);
				this.uber = this.Load(data.shaders.uberPostPS);
				this.finalPass = this.Load(data.shaders.finalPostPassPS);
				this.lensFlareDataDriven = this.Load(data.shaders.LensFlareDataDrivenPS);
			}

			// Token: 0x06000AFE RID: 2814 RVA: 0x0004A106 File Offset: 0x00048306
			private Material Load(Shader shader)
			{
				if (shader == null)
				{
					Debug.LogErrorFormat("Missing shader. PostProcessing render passes will not execute. Check for missing reference in the renderer resources.", Array.Empty<object>());
					return null;
				}
				if (!shader.isSupported)
				{
					return null;
				}
				return CoreUtils.CreateEngineMaterial(shader);
			}

			// Token: 0x06000AFF RID: 2815 RVA: 0x0004A134 File Offset: 0x00048334
			internal void Cleanup()
			{
				CoreUtils.Destroy(this.stopNaN);
				CoreUtils.Destroy(this.subpixelMorphologicalAntialiasing);
				CoreUtils.Destroy(this.gaussianDepthOfField);
				CoreUtils.Destroy(this.bokehDepthOfField);
				CoreUtils.Destroy(this.cameraMotionBlur);
				CoreUtils.Destroy(this.paniniProjection);
				CoreUtils.Destroy(this.bloom);
				CoreUtils.Destroy(this.temporalAntialiasing);
				CoreUtils.Destroy(this.scalingSetup);
				CoreUtils.Destroy(this.easu);
				CoreUtils.Destroy(this.uber);
				CoreUtils.Destroy(this.finalPass);
				CoreUtils.Destroy(this.lensFlareDataDriven);
			}

			// Token: 0x04000AD2 RID: 2770
			public readonly Material stopNaN;

			// Token: 0x04000AD3 RID: 2771
			public readonly Material subpixelMorphologicalAntialiasing;

			// Token: 0x04000AD4 RID: 2772
			public readonly Material gaussianDepthOfField;

			// Token: 0x04000AD5 RID: 2773
			public readonly Material bokehDepthOfField;

			// Token: 0x04000AD6 RID: 2774
			public readonly Material cameraMotionBlur;

			// Token: 0x04000AD7 RID: 2775
			public readonly Material paniniProjection;

			// Token: 0x04000AD8 RID: 2776
			public readonly Material bloom;

			// Token: 0x04000AD9 RID: 2777
			public readonly Material temporalAntialiasing;

			// Token: 0x04000ADA RID: 2778
			public readonly Material scalingSetup;

			// Token: 0x04000ADB RID: 2779
			public readonly Material easu;

			// Token: 0x04000ADC RID: 2780
			public readonly Material uber;

			// Token: 0x04000ADD RID: 2781
			public readonly Material finalPass;

			// Token: 0x04000ADE RID: 2782
			public readonly Material lensFlareDataDriven;
		}

		// Token: 0x020001A5 RID: 421
		private static class ShaderConstants
		{
			// Token: 0x04000ADF RID: 2783
			public static readonly int _TempTarget = Shader.PropertyToID("_TempTarget");

			// Token: 0x04000AE0 RID: 2784
			public static readonly int _TempTarget2 = Shader.PropertyToID("_TempTarget2");

			// Token: 0x04000AE1 RID: 2785
			public static readonly int _StencilRef = Shader.PropertyToID("_StencilRef");

			// Token: 0x04000AE2 RID: 2786
			public static readonly int _StencilMask = Shader.PropertyToID("_StencilMask");

			// Token: 0x04000AE3 RID: 2787
			public static readonly int _FullCoCTexture = Shader.PropertyToID("_FullCoCTexture");

			// Token: 0x04000AE4 RID: 2788
			public static readonly int _HalfCoCTexture = Shader.PropertyToID("_HalfCoCTexture");

			// Token: 0x04000AE5 RID: 2789
			public static readonly int _DofTexture = Shader.PropertyToID("_DofTexture");

			// Token: 0x04000AE6 RID: 2790
			public static readonly int _CoCParams = Shader.PropertyToID("_CoCParams");

			// Token: 0x04000AE7 RID: 2791
			public static readonly int _BokehKernel = Shader.PropertyToID("_BokehKernel");

			// Token: 0x04000AE8 RID: 2792
			public static readonly int _BokehConstants = Shader.PropertyToID("_BokehConstants");

			// Token: 0x04000AE9 RID: 2793
			public static readonly int _PongTexture = Shader.PropertyToID("_PongTexture");

			// Token: 0x04000AEA RID: 2794
			public static readonly int _PingTexture = Shader.PropertyToID("_PingTexture");

			// Token: 0x04000AEB RID: 2795
			public static readonly int _Metrics = Shader.PropertyToID("_Metrics");

			// Token: 0x04000AEC RID: 2796
			public static readonly int _AreaTexture = Shader.PropertyToID("_AreaTexture");

			// Token: 0x04000AED RID: 2797
			public static readonly int _SearchTexture = Shader.PropertyToID("_SearchTexture");

			// Token: 0x04000AEE RID: 2798
			public static readonly int _EdgeTexture = Shader.PropertyToID("_EdgeTexture");

			// Token: 0x04000AEF RID: 2799
			public static readonly int _BlendTexture = Shader.PropertyToID("_BlendTexture");

			// Token: 0x04000AF0 RID: 2800
			public static readonly int _ColorTexture = Shader.PropertyToID("_ColorTexture");

			// Token: 0x04000AF1 RID: 2801
			public static readonly int _Params = Shader.PropertyToID("_Params");

			// Token: 0x04000AF2 RID: 2802
			public static readonly int _SourceTexLowMip = Shader.PropertyToID("_SourceTexLowMip");

			// Token: 0x04000AF3 RID: 2803
			public static readonly int _Bloom_Params = Shader.PropertyToID("_Bloom_Params");

			// Token: 0x04000AF4 RID: 2804
			public static readonly int _Bloom_RGBM = Shader.PropertyToID("_Bloom_RGBM");

			// Token: 0x04000AF5 RID: 2805
			public static readonly int _Bloom_Texture = Shader.PropertyToID("_Bloom_Texture");

			// Token: 0x04000AF6 RID: 2806
			public static readonly int _LensDirt_Texture = Shader.PropertyToID("_LensDirt_Texture");

			// Token: 0x04000AF7 RID: 2807
			public static readonly int _LensDirt_Params = Shader.PropertyToID("_LensDirt_Params");

			// Token: 0x04000AF8 RID: 2808
			public static readonly int _LensDirt_Intensity = Shader.PropertyToID("_LensDirt_Intensity");

			// Token: 0x04000AF9 RID: 2809
			public static readonly int _Distortion_Params1 = Shader.PropertyToID("_Distortion_Params1");

			// Token: 0x04000AFA RID: 2810
			public static readonly int _Distortion_Params2 = Shader.PropertyToID("_Distortion_Params2");

			// Token: 0x04000AFB RID: 2811
			public static readonly int _Chroma_Params = Shader.PropertyToID("_Chroma_Params");

			// Token: 0x04000AFC RID: 2812
			public static readonly int _Vignette_Params1 = Shader.PropertyToID("_Vignette_Params1");

			// Token: 0x04000AFD RID: 2813
			public static readonly int _Vignette_Params2 = Shader.PropertyToID("_Vignette_Params2");

			// Token: 0x04000AFE RID: 2814
			public static readonly int _Vignette_ParamsXR = Shader.PropertyToID("_Vignette_ParamsXR");

			// Token: 0x04000AFF RID: 2815
			public static readonly int _Lut_Params = Shader.PropertyToID("_Lut_Params");

			// Token: 0x04000B00 RID: 2816
			public static readonly int _UserLut_Params = Shader.PropertyToID("_UserLut_Params");

			// Token: 0x04000B01 RID: 2817
			public static readonly int _InternalLut = Shader.PropertyToID("_InternalLut");

			// Token: 0x04000B02 RID: 2818
			public static readonly int _UserLut = Shader.PropertyToID("_UserLut");

			// Token: 0x04000B03 RID: 2819
			public static readonly int _DownSampleScaleFactor = Shader.PropertyToID("_DownSampleScaleFactor");

			// Token: 0x04000B04 RID: 2820
			public static readonly int _FlareOcclusionRemapTex = Shader.PropertyToID("_FlareOcclusionRemapTex");

			// Token: 0x04000B05 RID: 2821
			public static readonly int _FlareOcclusionTex = Shader.PropertyToID("_FlareOcclusionTex");

			// Token: 0x04000B06 RID: 2822
			public static readonly int _FlareOcclusionIndex = Shader.PropertyToID("_FlareOcclusionIndex");

			// Token: 0x04000B07 RID: 2823
			public static readonly int _FlareTex = Shader.PropertyToID("_FlareTex");

			// Token: 0x04000B08 RID: 2824
			public static readonly int _FlareColorValue = Shader.PropertyToID("_FlareColorValue");

			// Token: 0x04000B09 RID: 2825
			public static readonly int _FlareData0 = Shader.PropertyToID("_FlareData0");

			// Token: 0x04000B0A RID: 2826
			public static readonly int _FlareData1 = Shader.PropertyToID("_FlareData1");

			// Token: 0x04000B0B RID: 2827
			public static readonly int _FlareData2 = Shader.PropertyToID("_FlareData2");

			// Token: 0x04000B0C RID: 2828
			public static readonly int _FlareData3 = Shader.PropertyToID("_FlareData3");

			// Token: 0x04000B0D RID: 2829
			public static readonly int _FlareData4 = Shader.PropertyToID("_FlareData4");

			// Token: 0x04000B0E RID: 2830
			public static readonly int _FlareData5 = Shader.PropertyToID("_FlareData5");

			// Token: 0x04000B0F RID: 2831
			public static readonly int _FullscreenProjMat = Shader.PropertyToID("_FullscreenProjMat");

			// Token: 0x04000B10 RID: 2832
			public static int[] _BloomMipUp;

			// Token: 0x04000B11 RID: 2833
			public static int[] _BloomMipDown;
		}
	}
}
