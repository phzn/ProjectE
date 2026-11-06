using System;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering.Universal.Internal;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000036 RID: 54
	internal class Renderer2D : ScriptableRenderer
	{
		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060001E8 RID: 488 RVA: 0x000108D6 File Offset: 0x0000EAD6
		internal bool createColorTexture
		{
			get
			{
				return this.m_CreateColorTexture;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x000108DE File Offset: 0x0000EADE
		internal bool createDepthTexture
		{
			get
			{
				return this.m_CreateDepthTexture;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060001EA RID: 490 RVA: 0x000108E6 File Offset: 0x0000EAE6
		internal ColorGradingLutPass colorGradingLutPass
		{
			get
			{
				return this.m_PostProcessPasses.colorGradingLutPass;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060001EB RID: 491 RVA: 0x000108F3 File Offset: 0x0000EAF3
		internal PostProcessPass postProcessPass
		{
			get
			{
				return this.m_PostProcessPasses.postProcessPass;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060001EC RID: 492 RVA: 0x00010900 File Offset: 0x0000EB00
		internal PostProcessPass finalPostProcessPass
		{
			get
			{
				return this.m_PostProcessPasses.finalPostProcessPass;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060001ED RID: 493 RVA: 0x0001090D File Offset: 0x0000EB0D
		internal RTHandle afterPostProcessColorHandle
		{
			get
			{
				return this.m_PostProcessPasses.afterPostProcessColor;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060001EE RID: 494 RVA: 0x0001091A File Offset: 0x0000EB1A
		internal RTHandle colorGradingLutHandle
		{
			get
			{
				return this.m_PostProcessPasses.colorGradingLut;
			}
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00010927 File Offset: 0x0000EB27
		public override int SupportedCameraStackingTypes()
		{
			return 3;
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0001092C File Offset: 0x0000EB2C
		public Renderer2D(Renderer2DData data)
			: base(data)
		{
			this.m_BlitMaterial = CoreUtils.CreateEngineMaterial(data.coreBlitPS);
			this.m_BlitHDRMaterial = CoreUtils.CreateEngineMaterial(data.blitHDROverlay);
			this.m_SamplingMaterial = CoreUtils.CreateEngineMaterial(data.samplingShader);
			this.m_Render2DLightingPass = new Render2DLightingPass(data, this.m_BlitMaterial, this.m_SamplingMaterial);
			this.m_PixelPerfectBackgroundPass = new PixelPerfectBackgroundPass(RenderPassEvent.AfterRenderingTransparents);
			this.m_UpscalePass = new UpscalePass(RenderPassEvent.AfterRenderingPostProcessing, this.m_BlitMaterial);
			this.m_FinalBlitPass = new FinalBlitPass((RenderPassEvent)1001, this.m_BlitMaterial, this.m_BlitHDRMaterial);
			this.m_DrawOffscreenUIPass = new DrawScreenSpaceUIPass(RenderPassEvent.BeforeRenderingPostProcessing, true);
			this.m_DrawOverlayUIPass = new DrawScreenSpaceUIPass((RenderPassEvent)1002, false);
			this.m_ColorBufferSystem = new RenderTargetBufferSystem("_CameraColorAttachment");
			PostProcessParams postProcessParams = PostProcessParams.Create();
			postProcessParams.blitMaterial = this.m_BlitMaterial;
			postProcessParams.requestHDRFormat = GraphicsFormat.B10G11R11_UFloatPack32;
			this.m_PostProcessPasses = new PostProcessPasses(data.postProcessData, ref postProcessParams);
			this.m_UseDepthStencilBuffer = data.useDepthStencilBuffer;
			this.m_Renderer2DData = data;
			base.supportedRenderingFeatures = new ScriptableRenderer.RenderingFeatures();
			this.m_LightCullResult = new Light2DCullResult();
			this.m_Renderer2DData.lightCullResult = this.m_LightCullResult;
			bool flag = Blitter.GetBlitMaterial(TextureDimension.Tex2D, false) == null;
			UniversalRenderPipelineAsset asset = UniversalRenderPipeline.asset;
			if (asset != null)
			{
				ScriptableRendererData[] rendererDataList = asset.m_RendererDataList;
				for (int i = 0; i < rendererDataList.Length; i++)
				{
					if (rendererDataList[i] is UniversalRendererData)
					{
						flag = false;
						break;
					}
				}
			}
			if (flag)
			{
				Blitter.Initialize(data.coreBlitPS, data.coreBlitColorAndDepthPS);
			}
			LensFlareCommonSRP.mergeNeeded = 0;
			LensFlareCommonSRP.maxLensFlareWithOcclusionTemporalSample = 1;
			LensFlareCommonSRP.Initialize();
			Light2DManager.Initialize();
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00010AE0 File Offset: 0x0000ECE0
		protected override void Dispose(bool disposing)
		{
			this.m_Renderer2DData.Dispose();
			this.m_PostProcessPasses.Dispose();
			RTHandle colorTextureHandle = this.m_ColorTextureHandle;
			if (colorTextureHandle != null)
			{
				colorTextureHandle.Release();
			}
			RTHandle depthTextureHandle = this.m_DepthTextureHandle;
			if (depthTextureHandle != null)
			{
				depthTextureHandle.Release();
			}
			this.ReleaseRenderTargets();
			this.m_UpscalePass.Dispose();
			FinalBlitPass finalBlitPass = this.m_FinalBlitPass;
			if (finalBlitPass != null)
			{
				finalBlitPass.Dispose();
			}
			DrawScreenSpaceUIPass drawOffscreenUIPass = this.m_DrawOffscreenUIPass;
			if (drawOffscreenUIPass != null)
			{
				drawOffscreenUIPass.Dispose();
			}
			DrawScreenSpaceUIPass drawOverlayUIPass = this.m_DrawOverlayUIPass;
			if (drawOverlayUIPass != null)
			{
				drawOverlayUIPass.Dispose();
			}
			Light2DManager.Dispose();
			CoreUtils.Destroy(this.m_BlitMaterial);
			CoreUtils.Destroy(this.m_BlitHDRMaterial);
			CoreUtils.Destroy(this.m_SamplingMaterial);
			Blitter.Cleanup();
			base.Dispose(disposing);
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00010B9B File Offset: 0x0000ED9B
		internal override void ReleaseRenderTargets()
		{
			this.m_ColorBufferSystem.Dispose();
			this.m_PostProcessPasses.ReleaseRenderTargets();
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00010BB3 File Offset: 0x0000EDB3
		public Renderer2DData GetRenderer2DData()
		{
			return this.m_Renderer2DData;
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00010BBC File Offset: 0x0000EDBC
		private Renderer2D.RenderPassInputSummary GetRenderPassInputs(ref RenderingData renderingData, ref CameraData cameraData)
		{
			Renderer2D.RenderPassInputSummary renderPassInputSummary = default(Renderer2D.RenderPassInputSummary);
			for (int i = 0; i < base.activeRenderPassQueue.Count; i++)
			{
				ScriptableRenderPass scriptableRenderPass = base.activeRenderPassQueue[i];
				bool flag = (scriptableRenderPass.input & ScriptableRenderPassInput.Depth) > ScriptableRenderPassInput.None;
				bool flag2 = (scriptableRenderPass.input & ScriptableRenderPassInput.Color) > ScriptableRenderPassInput.None;
				renderPassInputSummary.requiresDepthTexture = renderPassInputSummary.requiresDepthTexture || flag;
				renderPassInputSummary.requiresColorTexture = renderPassInputSummary.requiresColorTexture || flag2;
			}
			renderPassInputSummary.requiresColorTexture |= cameraData.postProcessEnabled || cameraData.isHdrEnabled || cameraData.isSceneViewCamera || !cameraData.isDefaultViewport || cameraData.requireSrgbConversion || !cameraData.resolveFinalTarget || this.m_Renderer2DData.useCameraSortingLayerTexture || !Mathf.Approximately(cameraData.renderScale, 1f) || (base.DebugHandler != null && base.DebugHandler.WriteToDebugScreenTexture(ref cameraData));
			renderPassInputSummary.requiresDepthTexture |= !cameraData.resolveFinalTarget && this.m_UseDepthStencilBuffer;
			return renderPassInputSummary;
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x00010CB4 File Offset: 0x0000EEB4
		private void CreateRenderTextures(ref Renderer2D.RenderPassInputSummary renderPassInputs, CommandBuffer cmd, ref CameraData cameraData, bool forceCreateColorTexture, FilterMode colorTextureFilterMode, out RTHandle colorTargetHandle, out RTHandle depthTargetHandle)
		{
			ref RenderTextureDescriptor ptr = ref cameraData.cameraTargetDescriptor;
			RenderTextureDescriptor renderTextureDescriptor = ptr;
			renderTextureDescriptor.depthBufferBits = 0;
			this.m_ColorBufferSystem.SetCameraSettings(renderTextureDescriptor, colorTextureFilterMode);
			if (cameraData.renderType == CameraRenderType.Base)
			{
				this.m_CreateColorTexture = renderPassInputs.requiresColorTexture;
				this.m_CreateDepthTexture = renderPassInputs.requiresDepthTexture;
				this.m_CreateColorTexture = this.m_CreateColorTexture || forceCreateColorTexture;
				this.m_CreateDepthTexture |= this.createColorTexture;
				if (this.createColorTexture)
				{
					if (this.m_ColorBufferSystem.PeekBackBuffer() == null || this.m_ColorBufferSystem.PeekBackBuffer().nameID != BuiltinRenderTextureType.CameraTarget)
					{
						this.m_ColorTextureHandle = this.m_ColorBufferSystem.GetBackBuffer(cmd);
						cmd.SetGlobalTexture("_CameraColorTexture", this.m_ColorTextureHandle.nameID);
						cmd.SetGlobalTexture("_AfterPostProcessTexture", this.m_ColorTextureHandle.nameID);
					}
					this.m_ColorTextureHandle = this.m_ColorBufferSystem.PeekBackBuffer();
				}
				if (this.createDepthTexture)
				{
					RenderTextureDescriptor renderTextureDescriptor2 = ptr;
					renderTextureDescriptor2.colorFormat = RenderTextureFormat.Depth;
					renderTextureDescriptor2.graphicsFormat = GraphicsFormat.None;
					renderTextureDescriptor2.depthBufferBits = 32;
					if (!cameraData.resolveFinalTarget && this.m_UseDepthStencilBuffer)
					{
						renderTextureDescriptor2.bindMS = renderTextureDescriptor2.msaaSamples > 1 && !SystemInfo.supportsMultisampleAutoResolve && SystemInfo.supportsMultisampledTextures != 0;
					}
					RenderingUtils.ReAllocateIfNeeded(ref this.m_DepthTextureHandle, in renderTextureDescriptor2, FilterMode.Point, TextureWrapMode.Clamp, false, 1, 0f, "_CameraDepthAttachment");
				}
				colorTargetHandle = (this.createColorTexture ? this.m_ColorTextureHandle : ScriptableRenderer.k_CameraTarget);
				depthTargetHandle = (this.createDepthTexture ? this.m_DepthTextureHandle : ScriptableRenderer.k_CameraTarget);
				return;
			}
			UniversalAdditionalCameraData universalAdditionalCameraData;
			cameraData.baseCamera.TryGetComponent<UniversalAdditionalCameraData>(out universalAdditionalCameraData);
			Renderer2D renderer2D = (Renderer2D)universalAdditionalCameraData.scriptableRenderer;
			if (this.m_ColorBufferSystem != renderer2D.m_ColorBufferSystem)
			{
				this.m_ColorBufferSystem.Dispose();
				this.m_ColorBufferSystem = renderer2D.m_ColorBufferSystem;
			}
			this.m_CreateColorTexture = true;
			this.m_CreateDepthTexture = true;
			this.m_ColorTextureHandle = renderer2D.m_ColorTextureHandle;
			this.m_DepthTextureHandle = renderer2D.m_DepthTextureHandle;
			colorTargetHandle = this.m_ColorTextureHandle;
			depthTargetHandle = this.m_DepthTextureHandle;
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x00010ED0 File Offset: 0x0000F0D0
		public override void Setup(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			ref CameraData ptr = ref renderingData.cameraData;
			ref RenderTextureDescriptor ptr2 = ref ptr.cameraTargetDescriptor;
			bool flag = renderingData.postProcessingEnabled && this.m_PostProcessPasses.isCreated;
			bool flag2 = renderingData.cameraData.postProcessEnabled && this.m_PostProcessPasses.isCreated;
			bool resolveFinalTarget = ptr.resolveFinalTarget;
			FilterMode filterMode = FilterMode.Bilinear;
			PixelPerfectCamera pixelPerfectCamera = null;
			bool flag3 = false;
			bool flag4 = false;
			if (base.DebugHandler != null)
			{
				if (base.DebugHandler.AreAnySettingsActive)
				{
					flag = flag && base.DebugHandler.IsPostProcessingAllowed;
					flag2 = flag2 && base.DebugHandler.IsPostProcessingAllowed;
				}
				if (base.DebugHandler.IsActiveForCamera(ref ptr))
				{
					if (base.DebugHandler.WriteToDebugScreenTexture(ref ptr))
					{
						RenderTextureDescriptor cameraTargetDescriptor = ptr.cameraTargetDescriptor;
						DebugHandler.ConfigureColorDescriptorForDebugScreen(ref cameraTargetDescriptor, ptr.pixelWidth, ptr.pixelHeight);
						RenderingUtils.ReAllocateIfNeeded(base.DebugHandler.DebugScreenColorHandle, in cameraTargetDescriptor, FilterMode.Point, TextureWrapMode.Repeat, false, 1, 0f, "_DebugScreenColor");
						RenderTextureDescriptor cameraTargetDescriptor2 = ptr.cameraTargetDescriptor;
						DebugHandler.ConfigureDepthDescriptorForDebugScreen(ref cameraTargetDescriptor2, 32, ptr.pixelWidth, ptr.pixelHeight);
						RenderingUtils.ReAllocateIfNeeded(base.DebugHandler.DebugScreenDepthHandle, in cameraTargetDescriptor2, FilterMode.Point, TextureWrapMode.Repeat, false, 1, 0f, "_DebugScreenDepth");
					}
					if (base.DebugHandler.HDRDebugViewIsActive(ref ptr))
					{
						base.DebugHandler.hdrDebugViewPass.Setup(ref ptr, base.DebugHandler.DebugDisplaySettings.lightingSettings.hdrDebugMode);
						base.EnqueuePass(base.DebugHandler.hdrDebugViewPass);
					}
				}
			}
			if (ptr.renderType == CameraRenderType.Base && resolveFinalTarget)
			{
				ptr.camera.TryGetComponent<PixelPerfectCamera>(out pixelPerfectCamera);
				if (pixelPerfectCamera != null && pixelPerfectCamera.enabled)
				{
					if (pixelPerfectCamera.offscreenRTSize != Vector2Int.zero)
					{
						flag3 = true;
						ptr2.width = pixelPerfectCamera.offscreenRTSize.x;
						ptr2.height = pixelPerfectCamera.offscreenRTSize.y;
						FullScreenPassRendererFeature.FullScreenRenderPass fullScreenRenderPass = base.activeRenderPassQueue.Find((ScriptableRenderPass x) => x is FullScreenPassRendererFeature.FullScreenRenderPass) as FullScreenPassRendererFeature.FullScreenRenderPass;
						if (fullScreenRenderPass != null)
						{
							fullScreenRenderPass.ReAllocate(ptr2);
						}
					}
					filterMode = FilterMode.Point;
					flag4 = pixelPerfectCamera.gridSnapping == PixelPerfectCamera.GridSnapping.UpscaleRenderTexture || pixelPerfectCamera.requiresUpscalePass;
				}
			}
			Renderer2D.RenderPassInputSummary renderPassInputs = this.GetRenderPassInputs(ref renderingData, ref ptr);
			CommandBuffer commandBuffer = renderingData.commandBuffer;
			RTHandle rthandle;
			RTHandle rthandle2;
			using (new ProfilingScope(commandBuffer, Renderer2D.m_ProfilingSampler))
			{
				this.CreateRenderTextures(ref renderPassInputs, commandBuffer, ref ptr, flag3, filterMode, out rthandle, out rthandle2);
			}
			context.ExecuteCommandBuffer(commandBuffer);
			commandBuffer.Clear();
			base.ConfigureCameraTarget(rthandle, rthandle2);
			if (flag2)
			{
				RenderTextureDescriptor renderTextureDescriptor;
				FilterMode filterMode2;
				this.colorGradingLutPass.ConfigureDescriptor(in renderingData.postProcessingData, out renderTextureDescriptor, out filterMode2);
				RenderingUtils.ReAllocateIfNeeded(ref this.m_PostProcessPasses.m_ColorGradingLut, in renderTextureDescriptor, filterMode2, TextureWrapMode.Clamp, false, 1, 0f, "_InternalGradingLut");
				ColorGradingLutPass colorGradingLutPass = this.colorGradingLutPass;
				RTHandle rthandle3 = this.colorGradingLutHandle;
				colorGradingLutPass.Setup(in rthandle3);
				base.EnqueuePass(this.colorGradingLutPass);
			}
			this.m_Render2DLightingPass.Setup(renderPassInputs.requiresDepthTexture || this.m_UseDepthStencilBuffer);
			this.m_Render2DLightingPass.ConfigureTarget(rthandle, rthandle2);
			base.EnqueuePass(this.m_Render2DLightingPass);
			bool rendersOverlayUI = ptr.rendersOverlayUI;
			bool isHDROutputActive = ptr.isHDROutputActive;
			if (rendersOverlayUI && isHDROutputActive)
			{
				this.m_DrawOffscreenUIPass.Setup(ref ptr, 32);
				base.EnqueuePass(this.m_DrawOffscreenUIPass);
			}
			bool flag5 = ptr.antialiasing == AntialiasingMode.FastApproximateAntialiasing && !isHDROutputActive;
			bool flag6 = resolveFinalTarget && !flag4 && flag && flag5;
			bool flag7 = base.activeRenderPassQueue.Find((ScriptableRenderPass x) => x.renderPassEvent == RenderPassEvent.AfterRenderingPostProcessing) != null;
			bool flag8 = base.DebugHandler == null || !base.DebugHandler.HDRDebugViewIsActive(ref ptr);
			bool flag9 = pixelPerfectCamera != null && pixelPerfectCamera.enabled;
			bool flag10 = ptr.captureActions != null && resolveFinalTarget;
			bool flag11 = resolveFinalTarget && !flag10 && !flag7 && !flag6 && !flag9;
			if (flag2)
			{
				RenderTextureDescriptor compatibleDescriptor = PostProcessPass.GetCompatibleDescriptor(ptr2, ptr2.width, ptr2.height, ptr2.graphicsFormat, DepthBits.None);
				RenderingUtils.ReAllocateIfNeeded(ref this.m_PostProcessPasses.m_AfterPostProcessColor, in compatibleDescriptor, FilterMode.Point, TextureWrapMode.Clamp, false, 1, 0f, "_AfterPostProcessTexture");
				PostProcessPass postProcessPass = this.postProcessPass;
				ref RenderTextureDescriptor ptr3 = ref ptr2;
				bool flag12 = flag11;
				RTHandle rthandle3 = this.colorGradingLutHandle;
				RTHandle rthandle4 = null;
				postProcessPass.Setup(in ptr3, in rthandle, flag12, in rthandle2, in rthandle3, in rthandle4, flag6, this.afterPostProcessColorHandle.nameID == ScriptableRenderer.k_CameraTarget.nameID && flag8);
				base.EnqueuePass(this.postProcessPass);
			}
			RTHandle rthandle5 = rthandle;
			if (flag9 && pixelPerfectCamera.cropFrame != PixelPerfectCamera.CropFrame.None)
			{
				base.EnqueuePass(this.m_PixelPerfectBackgroundPass);
				if (pixelPerfectCamera.requiresUpscalePass)
				{
					int num = pixelPerfectCamera.refResolutionX * pixelPerfectCamera.pixelRatio;
					int num2 = pixelPerfectCamera.refResolutionY * pixelPerfectCamera.pixelRatio;
					this.m_UpscalePass.Setup(rthandle, num, num2, pixelPerfectCamera.finalBlitFilterMode, ref renderingData, out rthandle5);
					base.EnqueuePass(this.m_UpscalePass);
				}
			}
			if (flag6)
			{
				this.finalPostProcessPass.SetupFinalPass(in rthandle5, flag7, flag8);
				base.EnqueuePass(this.finalPostProcessPass);
			}
			if (!flag6 && (!flag2 || flag7 || flag10 || flag9) && !(rthandle.nameID == ScriptableRenderer.k_CameraTarget.nameID))
			{
				this.m_FinalBlitPass.Setup(ptr2, rthandle5);
				base.EnqueuePass(this.m_FinalBlitPass);
			}
			if (rendersOverlayUI && !isHDROutputActive)
			{
				base.EnqueuePass(this.m_DrawOverlayUIPass);
			}
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x00011480 File Offset: 0x0000F680
		public override void SetupCullingParameters(ref ScriptableCullingParameters cullingParameters, ref CameraData cameraData)
		{
			cullingParameters.cullingOptions = CullingOptions.None;
			cullingParameters.isOrthographic = cameraData.camera.orthographic;
			cullingParameters.shadowDistance = 0f;
			this.m_LightCullResult.SetupCulling(ref cullingParameters, cameraData.camera);
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x000114B8 File Offset: 0x0000F6B8
		internal override void SwapColorBuffer(CommandBuffer cmd)
		{
			this.m_ColorBufferSystem.Swap();
			if (this.m_DepthTextureHandle.nameID != BuiltinRenderTextureType.CameraTarget)
			{
				base.ConfigureCameraTarget(this.m_ColorBufferSystem.GetBackBuffer(cmd), this.m_DepthTextureHandle);
			}
			else
			{
				base.ConfigureCameraColorTarget(this.m_ColorBufferSystem.GetBackBuffer(cmd));
			}
			this.m_ColorTextureHandle = this.m_ColorBufferSystem.GetBackBuffer(cmd);
			cmd.SetGlobalTexture("_CameraColorTexture", this.m_ColorTextureHandle.nameID);
			cmd.SetGlobalTexture("_AfterPostProcessTexture", this.m_ColorTextureHandle.nameID);
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x00011552 File Offset: 0x0000F752
		internal override RTHandle GetCameraColorFrontBuffer(CommandBuffer cmd)
		{
			return this.m_ColorBufferSystem.GetFrontBuffer(cmd);
		}

		// Token: 0x060001FA RID: 506 RVA: 0x00011560 File Offset: 0x0000F760
		internal override RTHandle GetCameraColorBackBuffer(CommandBuffer cmd)
		{
			return this.m_ColorBufferSystem.GetBackBuffer(cmd);
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0001156E File Offset: 0x0000F76E
		internal override void EnableSwapBufferMSAA(bool enable)
		{
			this.m_ColorBufferSystem.EnableMSAA(enable);
		}

		// Token: 0x0400013A RID: 314
		internal const int k_DepthBufferBits = 32;

		// Token: 0x0400013B RID: 315
		private const int k_FinalBlitPassQueueOffset = 1;

		// Token: 0x0400013C RID: 316
		private const int k_AfterFinalBlitPassQueueOffset = 2;

		// Token: 0x0400013D RID: 317
		private Render2DLightingPass m_Render2DLightingPass;

		// Token: 0x0400013E RID: 318
		private PixelPerfectBackgroundPass m_PixelPerfectBackgroundPass;

		// Token: 0x0400013F RID: 319
		private UpscalePass m_UpscalePass;

		// Token: 0x04000140 RID: 320
		private FinalBlitPass m_FinalBlitPass;

		// Token: 0x04000141 RID: 321
		private DrawScreenSpaceUIPass m_DrawOffscreenUIPass;

		// Token: 0x04000142 RID: 322
		private DrawScreenSpaceUIPass m_DrawOverlayUIPass;

		// Token: 0x04000143 RID: 323
		private Light2DCullResult m_LightCullResult;

		// Token: 0x04000144 RID: 324
		internal RenderTargetBufferSystem m_ColorBufferSystem;

		// Token: 0x04000145 RID: 325
		private static readonly ProfilingSampler m_ProfilingSampler = new ProfilingSampler("Create Camera Textures");

		// Token: 0x04000146 RID: 326
		private bool m_UseDepthStencilBuffer = true;

		// Token: 0x04000147 RID: 327
		private bool m_CreateColorTexture;

		// Token: 0x04000148 RID: 328
		private bool m_CreateDepthTexture;

		// Token: 0x04000149 RID: 329
		private RTHandle m_ColorTextureHandle;

		// Token: 0x0400014A RID: 330
		private RTHandle m_DepthTextureHandle;

		// Token: 0x0400014B RID: 331
		private Material m_BlitMaterial;

		// Token: 0x0400014C RID: 332
		private Material m_BlitHDRMaterial;

		// Token: 0x0400014D RID: 333
		private Material m_SamplingMaterial;

		// Token: 0x0400014E RID: 334
		private Renderer2DData m_Renderer2DData;

		// Token: 0x0400014F RID: 335
		private PostProcessPasses m_PostProcessPasses;

		// Token: 0x0200015F RID: 351
		private struct RenderPassInputSummary
		{
			// Token: 0x0400099E RID: 2462
			internal bool requiresDepthTexture;

			// Token: 0x0400099F RID: 2463
			internal bool requiresColorTexture;
		}
	}
}
