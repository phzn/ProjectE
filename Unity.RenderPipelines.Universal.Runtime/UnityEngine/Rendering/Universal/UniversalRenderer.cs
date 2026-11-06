using System;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal.Internal;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000F0 RID: 240
	public sealed class UniversalRenderer : ScriptableRenderer
	{
		// Token: 0x0600076D RID: 1901 RVA: 0x0002D4AC File Offset: 0x0002B6AC
		public override int SupportedCameraStackingTypes()
		{
			switch (this.m_RenderingMode)
			{
			case RenderingMode.Forward:
			case RenderingMode.ForwardPlus:
				return 3;
			case RenderingMode.Deferred:
				return 1;
			default:
				return 0;
			}
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x0600076E RID: 1902 RVA: 0x0002D4D9 File Offset: 0x0002B6D9
		internal RenderingMode renderingModeRequested
		{
			get
			{
				return this.m_RenderingMode;
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x0600076F RID: 1903 RVA: 0x0002D4E4 File Offset: 0x0002B6E4
		internal RenderingMode renderingModeActual
		{
			get
			{
				if (this.renderingModeRequested != RenderingMode.Deferred || (!GL.wireframe && (base.DebugHandler == null || !base.DebugHandler.IsActiveModeUnsupportedForDeferred) && this.m_DeferredLights != null && this.m_DeferredLights.IsRuntimeSupportedThisFrame() && !this.m_DeferredLights.IsOverlay))
				{
					return this.renderingModeRequested;
				}
				return RenderingMode.Forward;
			}
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000770 RID: 1904 RVA: 0x0002D540 File Offset: 0x0002B740
		internal bool accurateGbufferNormals
		{
			get
			{
				return this.m_DeferredLights != null && this.m_DeferredLights.AccurateGbufferNormals;
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000771 RID: 1905 RVA: 0x0002D557 File Offset: 0x0002B757
		// (set) Token: 0x06000772 RID: 1906 RVA: 0x0002D55F File Offset: 0x0002B75F
		public DepthPrimingMode depthPrimingMode
		{
			get
			{
				return this.m_DepthPrimingMode;
			}
			set
			{
				this.m_DepthPrimingMode = value;
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000773 RID: 1907 RVA: 0x0002D568 File Offset: 0x0002B768
		internal ColorGradingLutPass colorGradingLutPass
		{
			get
			{
				return this.m_PostProcessPasses.colorGradingLutPass;
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000774 RID: 1908 RVA: 0x0002D575 File Offset: 0x0002B775
		internal PostProcessPass postProcessPass
		{
			get
			{
				return this.m_PostProcessPasses.postProcessPass;
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000775 RID: 1909 RVA: 0x0002D582 File Offset: 0x0002B782
		internal PostProcessPass finalPostProcessPass
		{
			get
			{
				return this.m_PostProcessPasses.finalPostProcessPass;
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000776 RID: 1910 RVA: 0x0002D58F File Offset: 0x0002B78F
		internal RTHandle colorGradingLut
		{
			get
			{
				return this.m_PostProcessPasses.colorGradingLut;
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000777 RID: 1911 RVA: 0x0002D59C File Offset: 0x0002B79C
		internal DeferredLights deferredLights
		{
			get
			{
				return this.m_DeferredLights;
			}
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x0002D5A4 File Offset: 0x0002B7A4
		public UniversalRenderer(UniversalRendererData data)
			: base(data)
		{
			PlatformAutoDetect.Initialize();
			XRSystem.Initialize(new Func<XRPassCreateInfo, XRPass>(XRPassUniversal.Create), data.xrSystemData.shaders.xrOcclusionMeshPS, data.xrSystemData.shaders.xrMirrorViewPS);
			this.m_BlitMaterial = CoreUtils.CreateEngineMaterial(data.shaders.coreBlitPS);
			this.m_BlitHDRMaterial = CoreUtils.CreateEngineMaterial(data.shaders.blitHDROverlay);
			this.m_CopyDepthMaterial = CoreUtils.CreateEngineMaterial(data.shaders.copyDepthPS);
			this.m_SamplingMaterial = CoreUtils.CreateEngineMaterial(data.shaders.samplingPS);
			this.m_StencilDeferredMaterial = CoreUtils.CreateEngineMaterial(data.shaders.stencilDeferredPS);
			this.m_CameraMotionVecMaterial = CoreUtils.CreateEngineMaterial(data.shaders.cameraMotionVector);
			this.m_ObjectMotionVecMaterial = CoreUtils.CreateEngineMaterial(data.shaders.objectMotionVector);
			StencilStateData defaultStencilState = data.defaultStencilState;
			this.m_DefaultStencilState = StencilState.defaultValue;
			this.m_DefaultStencilState.enabled = defaultStencilState.overrideStencilState;
			this.m_DefaultStencilState.SetCompareFunction(defaultStencilState.stencilCompareFunction);
			this.m_DefaultStencilState.SetPassOperation(defaultStencilState.passOperation);
			this.m_DefaultStencilState.SetFailOperation(defaultStencilState.failOperation);
			this.m_DefaultStencilState.SetZFailOperation(defaultStencilState.zFailOperation);
			this.m_IntermediateTextureMode = data.intermediateTextureMode;
			UniversalRenderPipelineAsset asset = UniversalRenderPipeline.asset;
			if (asset != null && asset.supportsLightCookies)
			{
				LightCookieManager.Settings settings = LightCookieManager.Settings.Create();
				UniversalRenderPipelineAsset asset2 = UniversalRenderPipeline.asset;
				if (asset2)
				{
					settings.atlas.format = asset2.additionalLightsCookieFormat;
					settings.atlas.resolution = asset2.additionalLightsCookieResolution;
				}
				this.m_LightCookieManager = new LightCookieManager(ref settings);
			}
			base.stripShadowsOffVariants = true;
			base.stripAdditionalLightOffVariants = true;
			ForwardLights.InitParams initParams;
			initParams.lightCookieManager = this.m_LightCookieManager;
			initParams.forwardPlus = data.renderingMode == RenderingMode.ForwardPlus;
			this.m_Clustering = data.renderingMode == RenderingMode.ForwardPlus;
			this.m_ForwardLights = new ForwardLights(initParams);
			this.m_RenderingMode = data.renderingMode;
			this.m_DepthPrimingMode = data.depthPrimingMode;
			this.m_CopyDepthMode = data.copyDepthMode;
			this.m_DepthPrimingRecommended = true;
			this.m_MainLightShadowCasterPass = new MainLightShadowCasterPass(RenderPassEvent.BeforeRenderingShadows);
			this.m_AdditionalLightsShadowCasterPass = new AdditionalLightsShadowCasterPass(RenderPassEvent.BeforeRenderingShadows);
			this.m_XROcclusionMeshPass = new XROcclusionMeshPass(RenderPassEvent.BeforeRenderingOpaques);
			this.m_XRCopyDepthPass = new CopyDepthPass((RenderPassEvent)1002, this.m_CopyDepthMaterial, false, false, false);
			this.m_DepthPrepass = new DepthOnlyPass(RenderPassEvent.BeforeRenderingPrePasses, RenderQueueRange.opaque, data.opaqueLayerMask);
			this.m_DepthNormalPrepass = new DepthNormalOnlyPass(RenderPassEvent.BeforeRenderingPrePasses, RenderQueueRange.opaque, data.opaqueLayerMask);
			if (this.renderingModeRequested == RenderingMode.Forward || this.renderingModeRequested == RenderingMode.ForwardPlus)
			{
				this.m_PrimedDepthCopyPass = new CopyDepthPass(RenderPassEvent.AfterRenderingPrePasses, this.m_CopyDepthMaterial, true, false, false);
			}
			if (this.renderingModeRequested == RenderingMode.Deferred)
			{
				this.m_DeferredLights = new DeferredLights(new DeferredLights.InitParams
				{
					stencilDeferredMaterial = this.m_StencilDeferredMaterial,
					lightCookieManager = this.m_LightCookieManager
				}, this.useRenderPassEnabled);
				this.m_DeferredLights.AccurateGbufferNormals = data.accurateGbufferNormals;
				this.m_GBufferPass = new GBufferPass(RenderPassEvent.BeforeRenderingGbuffer, RenderQueueRange.opaque, data.opaqueLayerMask, this.m_DefaultStencilState, defaultStencilState.stencilReference, this.m_DeferredLights);
				StencilState stencilState = DeferredLights.OverwriteStencil(this.m_DefaultStencilState, 96);
				ShaderTagId[] array = new ShaderTagId[]
				{
					new ShaderTagId("UniversalForwardOnly"),
					new ShaderTagId("SRPDefaultUnlit"),
					new ShaderTagId("LightweightForward")
				};
				int num = defaultStencilState.stencilReference | 0;
				this.m_GBufferCopyDepthPass = new CopyDepthPass((RenderPassEvent)211, this.m_CopyDepthMaterial, true, false, false);
				this.m_DeferredPass = new DeferredPass(RenderPassEvent.BeforeRenderingDeferredLights, this.m_DeferredLights);
				this.m_RenderOpaqueForwardOnlyPass = new DrawObjectsPass("Render Opaques Forward Only", array, true, RenderPassEvent.BeforeRenderingOpaques, RenderQueueRange.opaque, data.opaqueLayerMask, stencilState, num);
			}
			this.m_RenderOpaqueForwardPass = new DrawObjectsPass(URPProfileId.DrawOpaqueObjects, true, RenderPassEvent.BeforeRenderingOpaques, RenderQueueRange.opaque, data.opaqueLayerMask, this.m_DefaultStencilState, defaultStencilState.stencilReference);
			this.m_RenderOpaqueForwardWithRenderingLayersPass = new DrawObjectsWithRenderingLayersPass(URPProfileId.DrawOpaqueObjects, true, RenderPassEvent.BeforeRenderingOpaques, RenderQueueRange.opaque, data.opaqueLayerMask, this.m_DefaultStencilState, defaultStencilState.stencilReference);
			bool flag = this.m_CopyDepthMode == CopyDepthMode.AfterTransparents;
			RenderPassEvent renderPassEvent = (flag ? RenderPassEvent.AfterRenderingTransparents : RenderPassEvent.AfterRenderingSkybox);
			this.m_CopyDepthPass = new CopyDepthPass(renderPassEvent, this.m_CopyDepthMaterial, true, false, RenderingUtils.MultisampleDepthResolveSupported() && SystemInfo.supportsMultisampleAutoResolve && flag);
			this.m_MotionVectorPass = new MotionVectorRenderPass(renderPassEvent + 1, this.m_CameraMotionVecMaterial, this.m_ObjectMotionVecMaterial, data.opaqueLayerMask);
			this.m_DrawSkyboxPass = new DrawSkyboxPass(RenderPassEvent.BeforeRenderingSkybox);
			this.m_CopyColorPass = new CopyColorPass(RenderPassEvent.AfterRenderingSkybox, this.m_SamplingMaterial, this.m_BlitMaterial);
			this.m_TransparentSettingsPass = new TransparentSettingsPass(RenderPassEvent.BeforeRenderingTransparents, data.shadowTransparentReceive);
			this.m_RenderTransparentForwardPass = new DrawObjectsPass(URPProfileId.DrawTransparentObjects, false, RenderPassEvent.BeforeRenderingTransparents, RenderQueueRange.transparent, data.transparentLayerMask, this.m_DefaultStencilState, defaultStencilState.stencilReference);
			this.m_OnRenderObjectCallbackPass = new InvokeOnRenderObjectCallbackPass(RenderPassEvent.BeforeRenderingPostProcessing);
			this.m_DrawOffscreenUIPass = new DrawScreenSpaceUIPass(RenderPassEvent.BeforeRenderingPostProcessing, true);
			this.m_DrawOverlayUIPass = new DrawScreenSpaceUIPass((RenderPassEvent)1002, false);
			PostProcessParams postProcessParams = PostProcessParams.Create();
			postProcessParams.blitMaterial = this.m_BlitMaterial;
			postProcessParams.requestHDRFormat = GraphicsFormat.B10G11R11_UFloatPack32;
			UniversalRenderPipelineAsset asset3 = UniversalRenderPipeline.asset;
			if (asset3)
			{
				postProcessParams.requestHDRFormat = UniversalRenderPipeline.MakeRenderTextureGraphicsFormat(asset3.supportsHDR, asset3.hdrColorBufferPrecision, false);
			}
			this.m_PostProcessPasses = new PostProcessPasses(data.postProcessData, ref postProcessParams);
			this.m_CapturePass = new CapturePass(RenderPassEvent.AfterRendering);
			this.m_FinalBlitPass = new FinalBlitPass((RenderPassEvent)1001, this.m_BlitMaterial, this.m_BlitHDRMaterial);
			this.m_ColorBufferSystem = new RenderTargetBufferSystem("_CameraColorAttachment");
			base.supportedRenderingFeatures = new ScriptableRenderer.RenderingFeatures();
			if (this.renderingModeRequested == RenderingMode.Deferred)
			{
				base.supportedRenderingFeatures.msaa = false;
				base.unsupportedGraphicsDeviceTypes = new GraphicsDeviceType[]
				{
					GraphicsDeviceType.OpenGLCore,
					GraphicsDeviceType.OpenGLES2,
					GraphicsDeviceType.OpenGLES3
				};
			}
			LensFlareCommonSRP.mergeNeeded = 0;
			LensFlareCommonSRP.maxLensFlareWithOcclusionTemporalSample = 1;
			LensFlareCommonSRP.Initialize();
			this.m_VulkanEnablePreTransform = GraphicsSettings.HasShaderDefine(BuiltinShaderDefine.UNITY_PRETRANSFORM_TO_DISPLAY_ORIENTATION);
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x0002DBE0 File Offset: 0x0002BDE0
		protected override void Dispose(bool disposing)
		{
			this.m_ForwardLights.Cleanup();
			GBufferPass gbufferPass = this.m_GBufferPass;
			if (gbufferPass != null)
			{
				gbufferPass.Dispose();
			}
			this.m_PostProcessPasses.Dispose();
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
			RTHandle xrtargetHandleAlias = this.m_XRTargetHandleAlias;
			if (xrtargetHandleAlias != null)
			{
				xrtargetHandleAlias.Release();
			}
			this.ReleaseRenderTargets();
			base.Dispose(disposing);
			CoreUtils.Destroy(this.m_BlitMaterial);
			CoreUtils.Destroy(this.m_BlitHDRMaterial);
			CoreUtils.Destroy(this.m_CopyDepthMaterial);
			CoreUtils.Destroy(this.m_SamplingMaterial);
			CoreUtils.Destroy(this.m_StencilDeferredMaterial);
			CoreUtils.Destroy(this.m_CameraMotionVecMaterial);
			CoreUtils.Destroy(this.m_ObjectMotionVecMaterial);
			this.CleanupRenderGraphResources();
			LensFlareCommonSRP.Dispose();
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x0002DCC0 File Offset: 0x0002BEC0
		internal override void ReleaseRenderTargets()
		{
			this.m_ColorBufferSystem.Dispose();
			if (this.m_DeferredLights != null && !this.m_DeferredLights.UseRenderPass)
			{
				GBufferPass gbufferPass = this.m_GBufferPass;
				if (gbufferPass != null)
				{
					gbufferPass.Dispose();
				}
			}
			this.m_PostProcessPasses.ReleaseRenderTargets();
			MainLightShadowCasterPass mainLightShadowCasterPass = this.m_MainLightShadowCasterPass;
			if (mainLightShadowCasterPass != null)
			{
				mainLightShadowCasterPass.Dispose();
			}
			AdditionalLightsShadowCasterPass additionalLightsShadowCasterPass = this.m_AdditionalLightsShadowCasterPass;
			if (additionalLightsShadowCasterPass != null)
			{
				additionalLightsShadowCasterPass.Dispose();
			}
			RTHandle cameraDepthAttachment = this.m_CameraDepthAttachment;
			if (cameraDepthAttachment != null)
			{
				cameraDepthAttachment.Release();
			}
			RTHandle cameraDepthAttachment_D3d_ = this.m_CameraDepthAttachment_D3d_11;
			if (cameraDepthAttachment_D3d_ != null)
			{
				cameraDepthAttachment_D3d_.Release();
			}
			RTHandle depthTexture = this.m_DepthTexture;
			if (depthTexture != null)
			{
				depthTexture.Release();
			}
			RTHandle normalsTexture = this.m_NormalsTexture;
			if (normalsTexture != null)
			{
				normalsTexture.Release();
			}
			RTHandle decalLayersTexture = this.m_DecalLayersTexture;
			if (decalLayersTexture != null)
			{
				decalLayersTexture.Release();
			}
			RTHandle opaqueColor = this.m_OpaqueColor;
			if (opaqueColor != null)
			{
				opaqueColor.Release();
			}
			RTHandle motionVectorColor = this.m_MotionVectorColor;
			if (motionVectorColor != null)
			{
				motionVectorColor.Release();
			}
			RTHandle motionVectorDepth = this.m_MotionVectorDepth;
			if (motionVectorDepth != null)
			{
				motionVectorDepth.Release();
			}
			this.hasReleasedRTs = true;
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x0002DDBC File Offset: 0x0002BFBC
		private void SetupFinalPassDebug(ref CameraData cameraData)
		{
			if (base.DebugHandler != null && base.DebugHandler.IsActiveForCamera(ref cameraData))
			{
				DebugFullScreenMode debugFullScreenMode;
				int num;
				if (base.DebugHandler.TryGetFullscreenDebugMode(out debugFullScreenMode, out num) && (debugFullScreenMode != DebugFullScreenMode.ReflectionProbeAtlas || this.m_Clustering))
				{
					Camera camera = cameraData.camera;
					float num2 = (float)camera.pixelWidth;
					float num3 = (float)camera.pixelHeight;
					float num4 = Mathf.Clamp01((float)num / 100f);
					float num5 = num4 * num3;
					float num6 = num4 * num2;
					if (debugFullScreenMode == DebugFullScreenMode.ReflectionProbeAtlas)
					{
						RenderTexture atlasRT = this.m_ForwardLights.reflectionProbeManager.atlasRT;
						float num7 = num5 * (float)atlasRT.width / (float)atlasRT.height;
						if (num7 > num6)
						{
							num5 = num6 * (float)atlasRT.height / (float)atlasRT.width;
						}
						else
						{
							num6 = num7;
						}
					}
					float num8 = num6 / num2;
					float num9 = num5 / num3;
					Rect rect = new Rect(1f - num8, 1f - num9, num8, num9);
					switch (debugFullScreenMode)
					{
					case DebugFullScreenMode.Depth:
						base.DebugHandler.SetDebugRenderTarget(this.m_DepthTexture.nameID, rect, true);
						return;
					case DebugFullScreenMode.AdditionalLightsShadowMap:
						base.DebugHandler.SetDebugRenderTarget(this.m_AdditionalLightsShadowCasterPass.m_AdditionalLightsShadowmapHandle, rect, false);
						return;
					case DebugFullScreenMode.MainLightShadowMap:
						base.DebugHandler.SetDebugRenderTarget(this.m_MainLightShadowCasterPass.m_MainLightShadowmapTexture, rect, false);
						return;
					case DebugFullScreenMode.ReflectionProbeAtlas:
						base.DebugHandler.SetDebugRenderTarget(this.m_ForwardLights.reflectionProbeManager.atlasRT, rect, false);
						return;
					default:
						return;
					}
				}
				else
				{
					base.DebugHandler.ResetDebugRenderTarget();
				}
			}
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x0002DF50 File Offset: 0x0002C150
		public static bool IsOffscreenDepthTexture(in CameraData cameraData)
		{
			return cameraData.targetTexture != null && cameraData.targetTexture.format == RenderTextureFormat.Depth;
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x0002DF70 File Offset: 0x0002C170
		private bool IsDepthPrimingEnabled(ref CameraData cameraData)
		{
			if (!this.CanCopyDepth(ref cameraData))
			{
				return false;
			}
			bool flag = !this.IsWebGL();
			bool flag2 = (this.m_DepthPrimingRecommended && this.m_DepthPrimingMode == DepthPrimingMode.Auto) || this.m_DepthPrimingMode == DepthPrimingMode.Forced;
			bool flag3 = this.m_RenderingMode == RenderingMode.Forward || this.m_RenderingMode == RenderingMode.ForwardPlus;
			bool flag4 = cameraData.renderType == CameraRenderType.Base || cameraData.clearDepth;
			bool flag5 = cameraData.cameraType != CameraType.Reflection;
			bool flag6 = !UniversalRenderer.IsOffscreenDepthTexture(in cameraData);
			return flag2 && flag3 && flag4 && flag5 && flag6 && flag;
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x0002DFFA File Offset: 0x0002C1FA
		private bool IsWebGL()
		{
			return false;
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x0002DFFD File Offset: 0x0002C1FD
		private bool IsGLESDevice()
		{
			return SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES2 || SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES3;
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x0002E012 File Offset: 0x0002C212
		private bool IsGLDevice()
		{
			return this.IsGLESDevice() || SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLCore;
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x0002E028 File Offset: 0x0002C228
		public override void Setup(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			this.m_ForwardLights.PreSetup(ref renderingData);
			ref CameraData ptr = ref renderingData.cameraData;
			Camera camera = ptr.camera;
			RenderTextureDescriptor cameraTargetDescriptor = ptr.cameraTargetDescriptor;
			CommandBuffer commandBuffer = renderingData.commandBuffer;
			if (base.DebugHandler != null && base.DebugHandler.IsActiveForCamera(ref ptr))
			{
				if (base.DebugHandler.WriteToDebugScreenTexture(ref ptr))
				{
					RenderTextureDescriptor cameraTargetDescriptor2 = ptr.cameraTargetDescriptor;
					DebugHandler.ConfigureColorDescriptorForDebugScreen(ref cameraTargetDescriptor2, ptr.pixelWidth, ptr.pixelHeight);
					RenderingUtils.ReAllocateIfNeeded(base.DebugHandler.DebugScreenColorHandle, in cameraTargetDescriptor2, FilterMode.Point, TextureWrapMode.Repeat, false, 1, 0f, "_DebugScreenColor");
					RenderTextureDescriptor cameraTargetDescriptor3 = ptr.cameraTargetDescriptor;
					DebugHandler.ConfigureDepthDescriptorForDebugScreen(ref cameraTargetDescriptor3, 32, ptr.pixelWidth, ptr.pixelHeight);
					RenderingUtils.ReAllocateIfNeeded(base.DebugHandler.DebugScreenDepthHandle, in cameraTargetDescriptor3, FilterMode.Point, TextureWrapMode.Repeat, false, 1, 0f, "_DebugScreenDepth");
				}
				if (base.DebugHandler.HDRDebugViewIsActive(ref ptr))
				{
					base.DebugHandler.hdrDebugViewPass.Setup(ref ptr, base.DebugHandler.DebugDisplaySettings.lightingSettings.hdrDebugMode);
					base.EnqueuePass(base.DebugHandler.hdrDebugViewPass);
				}
			}
			if (ptr.cameraType != CameraType.Game)
			{
				this.useRenderPassEnabled = false;
			}
			base.useDepthPriming = this.IsDepthPrimingEnabled(ref ptr);
			if (UniversalRenderer.IsOffscreenDepthTexture(in ptr))
			{
				base.ConfigureCameraTarget(ScriptableRenderer.k_CameraTarget, ScriptableRenderer.k_CameraTarget);
				base.SetupRenderPasses(in renderingData);
				base.EnqueuePass(this.m_RenderOpaqueForwardPass);
				base.EnqueuePass(this.m_RenderTransparentForwardPass);
				return;
			}
			bool isPreviewCamera = ptr.isPreviewCamera;
			bool flag = (base.rendererFeatures.Count != 0 && this.m_IntermediateTextureMode == IntermediateTextureMode.Always && !isPreviewCamera) || (Application.isEditor && this.m_Clustering);
			UniversalRenderer.RenderPassInputSummary renderPassInputs = this.GetRenderPassInputs(ref renderingData);
			RenderingLayerUtils.Event @event;
			RenderingLayerUtils.MaskSize maskSize;
			bool flag2 = RenderingLayerUtils.RequireRenderingLayers(this, base.rendererFeatures, cameraTargetDescriptor.msaaSamples, out @event, out maskSize);
			if (this.IsGLDevice())
			{
				flag2 = false;
			}
			bool flag3 = false;
			bool flag4 = false;
			if (flag2 && this.renderingModeActual != RenderingMode.Deferred)
			{
				if (@event != RenderingLayerUtils.Event.DepthNormalPrePass)
				{
					if (@event != RenderingLayerUtils.Event.Opaque)
					{
						throw new ArgumentOutOfRangeException();
					}
					flag4 = true;
				}
				else
				{
					flag3 = true;
				}
			}
			if (flag3)
			{
				renderPassInputs.requiresNormalsTexture = true;
			}
			if (this.m_DeferredLights != null)
			{
				this.m_DeferredLights.RenderingLayerMaskSize = maskSize;
				this.m_DeferredLights.UseDecalLayers = flag2;
				this.m_DeferredLights.HasNormalPrepass = renderPassInputs.requiresNormalsTexture;
				this.m_DeferredLights.ResolveMixedLightingMode(ref renderingData);
				this.m_DeferredLights.IsOverlay = ptr.renderType == CameraRenderType.Overlay;
				if (this.m_DeferredLights.UseRenderPass)
				{
					foreach (ScriptableRenderPass scriptableRenderPass in base.activeRenderPassQueue)
					{
						if (scriptableRenderPass.renderPassEvent >= RenderPassEvent.AfterRenderingGbuffer && scriptableRenderPass.renderPassEvent <= RenderPassEvent.BeforeRenderingDeferredLights)
						{
							this.m_DeferredLights.DisableFramebufferFetchInput();
							break;
						}
					}
				}
			}
			bool flag5 = ptr.postProcessEnabled && this.m_PostProcessPasses.isCreated;
			bool flag6 = renderingData.postProcessingEnabled && this.m_PostProcessPasses.isCreated;
			bool flag7 = flag5 && ptr.postProcessingRequiresDepthTexture;
			bool flag8 = ptr.postProcessEnabled && this.m_PostProcessPasses.isCreated;
			bool flag9 = ptr.isSceneViewCamera || ptr.isPreviewCamera;
			object obj = ptr.requiresDepthTexture || renderPassInputs.requiresDepthTexture || this.m_DepthPrimingMode == DepthPrimingMode.Forced;
			bool flag10 = false;
			bool flag11 = this.m_MainLightShadowCasterPass.Setup(ref renderingData);
			bool flag12 = this.m_AdditionalLightsShadowCasterPass.Setup(ref renderingData);
			bool flag13 = this.m_TransparentSettingsPass.Setup();
			bool flag14 = this.m_CopyDepthMode == CopyDepthMode.ForcePrepass;
			object obj2 = obj;
			bool flag15 = (obj2 | flag7) != null && (!this.CanCopyDepth(ref renderingData.cameraData) || flag14);
			flag15 = flag15 || flag9;
			flag15 = flag15 || flag10;
			flag15 = flag15 || isPreviewCamera;
			flag15 |= renderPassInputs.requiresDepthPrepass;
			flag15 |= renderPassInputs.requiresNormalsTexture;
			if (flag15 && this.renderingModeActual == RenderingMode.Deferred && !renderPassInputs.requiresNormalsTexture)
			{
				flag15 = false;
			}
			flag15 |= base.useDepthPriming;
			if (obj2 != null)
			{
				RenderPassEvent renderPassEvent = ((this.m_CopyDepthMode == CopyDepthMode.AfterTransparents) ? RenderPassEvent.AfterRenderingTransparents : RenderPassEvent.AfterRenderingOpaques);
				if (renderPassInputs.requiresDepthTexture)
				{
					renderPassEvent = (RenderPassEvent)Mathf.Min(500, renderPassInputs.requiresDepthTextureEarliestEvent - (RenderPassEvent)1);
				}
				this.m_CopyDepthPass.renderPassEvent = renderPassEvent;
				if (renderPassEvent < RenderPassEvent.AfterRenderingTransparents)
				{
					this.m_CopyDepthPass.m_CopyResolvedDepth = false;
					this.m_CopyDepthMode = CopyDepthMode.AfterOpaques;
				}
			}
			else if (flag7 || flag9 || flag10)
			{
				this.m_CopyDepthPass.renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
			}
			flag |= this.RequiresIntermediateColorTexture(ref ptr);
			flag |= renderPassInputs.requiresColorTexture;
			flag |= renderPassInputs.requiresColorTextureCreated;
			flag &= !isPreviewCamera;
			bool flag16 = (obj2 | flag7) != null && !flag15;
			flag16 |= !ptr.resolveFinalTarget;
			flag16 |= this.renderingModeActual == RenderingMode.Deferred && !this.useRenderPassEnabled;
			flag16 |= base.useDepthPriming;
			flag16 = flag16 || flag4;
			if (ptr.xr.enabled)
			{
				flag = flag || flag16;
			}
			if (RTHandles.rtHandleProperties.rtHandleScale.x != 1f || RTHandles.rtHandleProperties.rtHandleScale.y != 1f)
			{
				flag = flag || flag16;
			}
			if (this.useRenderPassEnabled || base.useDepthPriming)
			{
				flag = flag || flag16;
			}
			RenderTextureDescriptor renderTextureDescriptor = cameraTargetDescriptor;
			renderTextureDescriptor.useMipMap = false;
			renderTextureDescriptor.autoGenerateMips = false;
			renderTextureDescriptor.depthBufferBits = 0;
			this.m_ColorBufferSystem.SetCameraSettings(renderTextureDescriptor, FilterMode.Bilinear);
			if (ptr.renderType == CameraRenderType.Base)
			{
				bool flag17 = camera.sceneViewFilterMode == Camera.SceneViewFilterMode.ShowFiltered;
				bool flag18 = (flag || flag16) && !flag17;
				flag16 = flag16 || flag;
				RenderTargetIdentifier renderTargetIdentifier = BuiltinRenderTextureType.CameraTarget;
				if (ptr.xr.enabled)
				{
					renderTargetIdentifier = ptr.xr.renderTarget;
				}
				if (this.m_XRTargetHandleAlias == null)
				{
					this.m_XRTargetHandleAlias = RTHandles.Alloc(renderTargetIdentifier);
				}
				else if (this.m_XRTargetHandleAlias.nameID != renderTargetIdentifier)
				{
					RTHandleStaticHelpers.SetRTHandleUserManagedWrapper(ref this.m_XRTargetHandleAlias, renderTargetIdentifier);
				}
				if (flag18)
				{
					this.CreateCameraRenderTarget(context, ref cameraTargetDescriptor, base.useDepthPriming, commandBuffer, ref ptr);
					if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.Direct3D11)
					{
						commandBuffer.CopyTexture(this.m_CameraDepthAttachment, this.m_CameraDepthAttachment_D3d_11);
					}
				}
				this.m_RenderOpaqueForwardPass.m_IsActiveTargetBackBuffer = !flag18;
				this.m_RenderTransparentForwardPass.m_IsActiveTargetBackBuffer = !flag18;
				this.m_DrawSkyboxPass.m_IsActiveTargetBackBuffer = !flag18;
				this.m_XROcclusionMeshPass.m_IsActiveTargetBackBuffer = !flag18;
				this.m_ActiveCameraColorAttachment = (flag ? this.m_ColorBufferSystem.PeekBackBuffer() : this.m_XRTargetHandleAlias);
				this.m_ActiveCameraDepthAttachment = (flag16 ? this.m_CameraDepthAttachment : this.m_XRTargetHandleAlias);
			}
			else
			{
				UniversalAdditionalCameraData universalAdditionalCameraData;
				ptr.baseCamera.TryGetComponent<UniversalAdditionalCameraData>(out universalAdditionalCameraData);
				UniversalRenderer universalRenderer = (UniversalRenderer)universalAdditionalCameraData.scriptableRenderer;
				if (this.m_ColorBufferSystem != universalRenderer.m_ColorBufferSystem)
				{
					this.m_ColorBufferSystem.Dispose();
					this.m_ColorBufferSystem = universalRenderer.m_ColorBufferSystem;
				}
				this.m_ActiveCameraColorAttachment = this.m_ColorBufferSystem.PeekBackBuffer();
				this.m_ActiveCameraDepthAttachment = universalRenderer.m_ActiveCameraDepthAttachment;
				this.m_XRTargetHandleAlias = universalRenderer.m_XRTargetHandleAlias;
			}
			if (base.rendererFeatures.Count != 0 && !isPreviewCamera)
			{
				base.ConfigureCameraColorTarget(this.m_ColorBufferSystem.PeekBackBuffer());
			}
			bool flag19 = renderingData.cameraData.requiresOpaqueTexture || renderPassInputs.requiresColorTexture;
			flag19 &= !isPreviewCamera;
			base.ConfigureCameraTarget(this.m_ActiveCameraColorAttachment, this.m_ActiveCameraDepthAttachment);
			bool flag20 = base.activeRenderPassQueue.Find((ScriptableRenderPass x) => x.renderPassEvent == RenderPassEvent.AfterRenderingPostProcessing) != null;
			if (flag11)
			{
				base.EnqueuePass(this.m_MainLightShadowCasterPass);
			}
			if (flag12)
			{
				base.EnqueuePass(this.m_AdditionalLightsShadowCasterPass);
			}
			bool flag21 = !flag15 && (renderingData.cameraData.requiresDepthTexture || flag7 || renderPassInputs.requiresDepthTexture) && flag16;
			if (base.DebugHandler != null && base.DebugHandler.IsActiveForCamera(ref ptr))
			{
				DebugFullScreenMode debugFullScreenMode;
				base.DebugHandler.TryGetFullscreenDebugMode(out debugFullScreenMode);
				if (debugFullScreenMode == DebugFullScreenMode.Depth)
				{
					flag15 = true;
				}
				if (!base.DebugHandler.IsLightingActive)
				{
					flag11 = false;
					flag12 = false;
					if (!flag9)
					{
						flag15 = false;
						base.useDepthPriming = false;
						flag8 = false;
						flag19 = false;
						flag21 = false;
					}
				}
				if (this.useRenderPassEnabled)
				{
					this.useRenderPassEnabled = base.DebugHandler.IsRenderPassSupported;
				}
			}
			ptr.renderer.useDepthPriming = base.useDepthPriming;
			if (this.renderingModeActual == RenderingMode.Deferred && this.m_DeferredLights.UseRenderPass && (RenderPassEvent.AfterRenderingGbuffer == renderPassInputs.requiresDepthNormalAtEvent || !this.useRenderPassEnabled))
			{
				this.m_DeferredLights.DisableFramebufferFetchInput();
			}
			if ((this.renderingModeActual == RenderingMode.Deferred && !this.useRenderPassEnabled) || flag15 || flag21)
			{
				RenderTextureDescriptor renderTextureDescriptor2 = cameraTargetDescriptor;
				if ((flag15 && this.renderingModeActual != RenderingMode.Deferred) || !RenderingUtils.SupportsGraphicsFormat(GraphicsFormat.R32_SFloat, FormatUsage.Render))
				{
					renderTextureDescriptor2.graphicsFormat = GraphicsFormat.None;
					renderTextureDescriptor2.depthStencilFormat = GraphicsFormat.D32_SFloat_S8_UInt;
					renderTextureDescriptor2.depthBufferBits = 32;
				}
				else
				{
					renderTextureDescriptor2.graphicsFormat = GraphicsFormat.R32_SFloat;
					renderTextureDescriptor2.depthStencilFormat = GraphicsFormat.None;
					renderTextureDescriptor2.depthBufferBits = 0;
				}
				renderTextureDescriptor2.msaaSamples = 1;
				RenderingUtils.ReAllocateIfNeeded(ref this.m_DepthTexture, in renderTextureDescriptor2, FilterMode.Point, TextureWrapMode.Clamp, false, 1, 0f, "_CameraDepthTexture");
				commandBuffer.SetGlobalTexture(this.m_DepthTexture.name, this.m_DepthTexture.nameID);
				context.ExecuteCommandBuffer(commandBuffer);
				commandBuffer.Clear();
			}
			if (flag2 || (this.renderingModeActual == RenderingMode.Deferred && this.m_DeferredLights.UseRenderingLayers))
			{
				ref RTHandle ptr2 = ref this.m_DecalLayersTexture;
				string text = "_CameraRenderingLayersTexture";
				if (this.renderingModeActual == RenderingMode.Deferred && this.m_DeferredLights.UseRenderingLayers)
				{
					ptr2 = ref this.m_DeferredLights.GbufferAttachments[this.m_DeferredLights.GBufferRenderingLayers];
					text = ptr2.name;
				}
				RenderTextureDescriptor renderTextureDescriptor3 = cameraTargetDescriptor;
				renderTextureDescriptor3.depthBufferBits = 0;
				if (!flag4)
				{
					renderTextureDescriptor3.msaaSamples = 1;
				}
				if (this.renderingModeActual == RenderingMode.Deferred && this.m_DeferredLights.UseRenderingLayers)
				{
					renderTextureDescriptor3.graphicsFormat = this.m_DeferredLights.GetGBufferFormat(this.m_DeferredLights.GBufferRenderingLayers);
				}
				else
				{
					renderTextureDescriptor3.graphicsFormat = RenderingLayerUtils.GetFormat(maskSize);
				}
				if (this.renderingModeActual == RenderingMode.Deferred && this.m_DeferredLights.UseRenderingLayers)
				{
					this.m_DeferredLights.ReAllocateGBufferIfNeeded(renderTextureDescriptor3, this.m_DeferredLights.GBufferRenderingLayers);
				}
				else
				{
					RenderingUtils.ReAllocateIfNeeded(ref ptr2, in renderTextureDescriptor3, FilterMode.Point, TextureWrapMode.Clamp, false, 1, 0f, text);
				}
				commandBuffer.SetGlobalTexture(ptr2.name, ptr2.nameID);
				RenderingLayerUtils.SetupProperties(commandBuffer, maskSize);
				if (this.renderingModeActual == RenderingMode.Deferred)
				{
					commandBuffer.SetGlobalTexture("_CameraRenderingLayersTexture", ptr2.nameID);
				}
				context.ExecuteCommandBuffer(commandBuffer);
				commandBuffer.Clear();
			}
			if (flag15 && renderPassInputs.requiresNormalsTexture)
			{
				ref RTHandle ptr3 = ref this.m_NormalsTexture;
				string text2 = "_CameraNormalsTexture";
				if (this.renderingModeActual == RenderingMode.Deferred)
				{
					ptr3 = ref this.m_DeferredLights.GbufferAttachments[this.m_DeferredLights.GBufferNormalSmoothnessIndex];
					text2 = ptr3.name;
				}
				RenderTextureDescriptor renderTextureDescriptor4 = cameraTargetDescriptor;
				renderTextureDescriptor4.depthBufferBits = 0;
				renderTextureDescriptor4.msaaSamples = (base.useDepthPriming ? cameraTargetDescriptor.msaaSamples : 1);
				if (this.renderingModeActual == RenderingMode.Deferred)
				{
					renderTextureDescriptor4.graphicsFormat = this.m_DeferredLights.GetGBufferFormat(this.m_DeferredLights.GBufferNormalSmoothnessIndex);
				}
				else
				{
					renderTextureDescriptor4.graphicsFormat = DepthNormalOnlyPass.GetGraphicsFormat();
				}
				if (this.renderingModeActual == RenderingMode.Deferred)
				{
					this.m_DeferredLights.ReAllocateGBufferIfNeeded(renderTextureDescriptor4, this.m_DeferredLights.GBufferNormalSmoothnessIndex);
				}
				else
				{
					RenderingUtils.ReAllocateIfNeeded(ref ptr3, in renderTextureDescriptor4, FilterMode.Point, TextureWrapMode.Clamp, false, 1, 0f, text2);
				}
				commandBuffer.SetGlobalTexture(ptr3.name, ptr3.nameID);
				if (this.renderingModeActual == RenderingMode.Deferred)
				{
					commandBuffer.SetGlobalTexture("_CameraNormalsTexture", ptr3.nameID);
				}
				context.ExecuteCommandBuffer(commandBuffer);
				commandBuffer.Clear();
			}
			if (flag15)
			{
				if (renderPassInputs.requiresNormalsTexture)
				{
					if (this.renderingModeActual == RenderingMode.Deferred)
					{
						int gbufferNormalSmoothnessIndex = this.m_DeferredLights.GBufferNormalSmoothnessIndex;
						if (this.m_DeferredLights.UseRenderingLayers)
						{
							this.m_DepthNormalPrepass.Setup(this.m_ActiveCameraDepthAttachment, this.m_DeferredLights.GbufferAttachments[gbufferNormalSmoothnessIndex], this.m_DeferredLights.GbufferAttachments[this.m_DeferredLights.GBufferRenderingLayers]);
						}
						else if (flag3)
						{
							this.m_DepthNormalPrepass.Setup(this.m_ActiveCameraDepthAttachment, this.m_DeferredLights.GbufferAttachments[gbufferNormalSmoothnessIndex], this.m_DecalLayersTexture);
						}
						else
						{
							this.m_DepthNormalPrepass.Setup(this.m_ActiveCameraDepthAttachment, this.m_DeferredLights.GbufferAttachments[gbufferNormalSmoothnessIndex]);
						}
						if (RenderPassEvent.AfterRenderingGbuffer <= renderPassInputs.requiresDepthNormalAtEvent && renderPassInputs.requiresDepthNormalAtEvent <= RenderPassEvent.BeforeRenderingOpaques)
						{
							this.m_DepthNormalPrepass.shaderTagIds = UniversalRenderer.k_DepthNormalsOnly;
						}
					}
					else if (flag3)
					{
						this.m_DepthNormalPrepass.Setup(this.m_DepthTexture, this.m_NormalsTexture, this.m_DecalLayersTexture);
					}
					else
					{
						this.m_DepthNormalPrepass.Setup(this.m_DepthTexture, this.m_NormalsTexture);
					}
					base.EnqueuePass(this.m_DepthNormalPrepass);
				}
				else if (this.renderingModeActual != RenderingMode.Deferred)
				{
					this.m_DepthPrepass.Setup(cameraTargetDescriptor, this.m_DepthTexture);
					base.EnqueuePass(this.m_DepthPrepass);
				}
			}
			if (base.useDepthPriming)
			{
				this.m_PrimedDepthCopyPass.Setup(this.m_ActiveCameraDepthAttachment, this.m_DepthTexture);
				base.EnqueuePass(this.m_PrimedDepthCopyPass);
			}
			if (flag8)
			{
				RenderTextureDescriptor renderTextureDescriptor5;
				FilterMode filterMode;
				this.colorGradingLutPass.ConfigureDescriptor(in renderingData.postProcessingData, out renderTextureDescriptor5, out filterMode);
				RenderingUtils.ReAllocateIfNeeded(ref this.m_PostProcessPasses.m_ColorGradingLut, in renderTextureDescriptor5, filterMode, TextureWrapMode.Clamp, false, 0, 0f, "_InternalGradingLut");
				ColorGradingLutPass colorGradingLutPass = this.colorGradingLutPass;
				RTHandle rthandle = this.colorGradingLut;
				colorGradingLutPass.Setup(in rthandle);
				base.EnqueuePass(this.colorGradingLutPass);
			}
			if (ptr.xr.hasValidOcclusionMesh)
			{
				base.EnqueuePass(this.m_XROcclusionMeshPass);
			}
			bool resolveFinalTarget = ptr.resolveFinalTarget;
			if (this.renderingModeActual == RenderingMode.Deferred)
			{
				if (this.m_DeferredLights.UseRenderPass && (RenderPassEvent.AfterRenderingGbuffer == renderPassInputs.requiresDepthNormalAtEvent || !this.useRenderPassEnabled))
				{
					this.m_DeferredLights.DisableFramebufferFetchInput();
				}
				this.EnqueueDeferred(ref renderingData, flag15, renderPassInputs.requiresNormalsTexture, flag3, flag11, flag12);
			}
			else
			{
				RenderBufferStoreAction renderBufferStoreAction = RenderBufferStoreAction.Store;
				if (cameraTargetDescriptor.msaaSamples > 1)
				{
					renderBufferStoreAction = (flag19 ? RenderBufferStoreAction.StoreAndResolve : RenderBufferStoreAction.Store);
				}
				RenderBufferStoreAction renderBufferStoreAction2 = ((flag19 || flag21 || !resolveFinalTarget) ? RenderBufferStoreAction.Store : RenderBufferStoreAction.DontCare);
				if (ptr.xr.enabled && ptr.xr.copyDepth)
				{
					renderBufferStoreAction2 = RenderBufferStoreAction.Store;
				}
				if (flag21 && cameraTargetDescriptor.msaaSamples > 1 && RenderingUtils.MultisampleDepthResolveSupported() && this.m_CopyDepthPass.renderPassEvent == RenderPassEvent.AfterRenderingTransparents && !flag19)
				{
					if (renderBufferStoreAction2 == RenderBufferStoreAction.Store)
					{
						renderBufferStoreAction2 = RenderBufferStoreAction.StoreAndResolve;
					}
					else if (renderBufferStoreAction2 == RenderBufferStoreAction.DontCare)
					{
						renderBufferStoreAction2 = RenderBufferStoreAction.Resolve;
					}
				}
				DrawObjectsPass drawObjectsPass;
				if (flag4)
				{
					drawObjectsPass = this.m_RenderOpaqueForwardWithRenderingLayersPass;
					this.m_RenderOpaqueForwardWithRenderingLayersPass.Setup(this.m_ActiveCameraColorAttachment, this.m_DecalLayersTexture, this.m_ActiveCameraDepthAttachment);
				}
				else
				{
					drawObjectsPass = this.m_RenderOpaqueForwardPass;
				}
				drawObjectsPass.ConfigureColorStoreAction(renderBufferStoreAction, 0U);
				drawObjectsPass.ConfigureDepthStoreAction(renderBufferStoreAction2);
				ClearFlag clearFlag = ((base.activeRenderPassQueue.Find((ScriptableRenderPass x) => x.renderPassEvent <= RenderPassEvent.BeforeRenderingOpaques && !x.overrideCameraTarget) != null || ptr.renderType != CameraRenderType.Base) ? ClearFlag.None : ClearFlag.Color);
				if (SystemInfo.usesLoadStoreActions)
				{
					drawObjectsPass.ConfigureClear(clearFlag, Color.black);
				}
				base.EnqueuePass(drawObjectsPass);
			}
			Skybox skybox;
			if (camera.clearFlags == CameraClearFlags.Skybox && ptr.renderType != CameraRenderType.Overlay && (RenderSettings.skybox != null || (camera.TryGetComponent<Skybox>(out skybox) && skybox.material != null)))
			{
				base.EnqueuePass(this.m_DrawSkyboxPass);
			}
			if (flag21 && (this.renderingModeActual != RenderingMode.Deferred || !this.useRenderPassEnabled || renderPassInputs.requiresDepthTexture))
			{
				this.m_CopyDepthPass.Setup(this.m_ActiveCameraDepthAttachment, this.m_DepthTexture);
				base.EnqueuePass(this.m_CopyDepthPass);
			}
			if (ptr.renderType == CameraRenderType.Base && !flag15 && !flag21)
			{
				Shader.SetGlobalTexture("_CameraDepthTexture", SystemInfo.usesReversedZBuffer ? Texture2D.blackTexture : Texture2D.whiteTexture);
			}
			if (flag19)
			{
				Downsampling opaqueDownsampling = UniversalRenderPipeline.asset.opaqueDownsampling;
				RenderTextureDescriptor renderTextureDescriptor6 = cameraTargetDescriptor;
				FilterMode filterMode2;
				CopyColorPass.ConfigureDescriptor(opaqueDownsampling, ref renderTextureDescriptor6, out filterMode2);
				RenderingUtils.ReAllocateIfNeeded(ref this.m_OpaqueColor, in renderTextureDescriptor6, filterMode2, TextureWrapMode.Clamp, false, 1, 0f, "_CameraOpaqueTexture");
				this.m_CopyColorPass.Setup(this.m_ActiveCameraColorAttachment, this.m_OpaqueColor, opaqueDownsampling);
				base.EnqueuePass(this.m_CopyColorPass);
			}
			if (renderPassInputs.requiresMotionVectors)
			{
				RenderTextureDescriptor renderTextureDescriptor7 = cameraTargetDescriptor;
				renderTextureDescriptor7.graphicsFormat = GraphicsFormat.R16G16_SFloat;
				renderTextureDescriptor7.depthBufferBits = 0;
				renderTextureDescriptor7.msaaSamples = 1;
				RenderingUtils.ReAllocateIfNeeded(ref this.m_MotionVectorColor, in renderTextureDescriptor7, FilterMode.Point, TextureWrapMode.Clamp, false, 1, 0f, "_MotionVectorTexture");
				RenderTextureDescriptor renderTextureDescriptor8 = cameraTargetDescriptor;
				renderTextureDescriptor8.graphicsFormat = GraphicsFormat.None;
				renderTextureDescriptor8.msaaSamples = 1;
				RenderingUtils.ReAllocateIfNeeded(ref this.m_MotionVectorDepth, in renderTextureDescriptor8, FilterMode.Point, TextureWrapMode.Clamp, false, 1, 0f, "_MotionVectorDepthTexture");
				this.m_MotionVectorPass.Setup(this.m_MotionVectorColor, this.m_MotionVectorDepth);
				base.EnqueuePass(this.m_MotionVectorPass);
			}
			if (flag13)
			{
				base.EnqueuePass(this.m_TransparentSettingsPass);
			}
			RenderBufferStoreAction renderBufferStoreAction3 = ((cameraTargetDescriptor.msaaSamples > 1 && resolveFinalTarget) ? RenderBufferStoreAction.Resolve : RenderBufferStoreAction.Store);
			RenderBufferStoreAction renderBufferStoreAction4 = (resolveFinalTarget ? RenderBufferStoreAction.DontCare : RenderBufferStoreAction.Store);
			if (flag21 && this.m_CopyDepthPass.renderPassEvent >= RenderPassEvent.AfterRenderingTransparents)
			{
				renderBufferStoreAction4 = RenderBufferStoreAction.Store;
				if (cameraTargetDescriptor.msaaSamples > 1 && RenderingUtils.MultisampleDepthResolveSupported())
				{
					renderBufferStoreAction4 = RenderBufferStoreAction.Resolve;
				}
			}
			this.m_RenderTransparentForwardPass.ConfigureColorStoreAction(renderBufferStoreAction3, 0U);
			this.m_RenderTransparentForwardPass.ConfigureDepthStoreAction(renderBufferStoreAction4);
			base.EnqueuePass(this.m_RenderTransparentForwardPass);
			base.EnqueuePass(this.m_OnRenderObjectCallbackPass);
			bool rendersOverlayUI = ptr.rendersOverlayUI;
			bool isHDROutputActive = ptr.isHDROutputActive;
			if (rendersOverlayUI && isHDROutputActive)
			{
				this.m_DrawOffscreenUIPass.Setup(ref ptr, 32);
				base.EnqueuePass(this.m_DrawOffscreenUIPass);
			}
			bool flag22 = renderingData.cameraData.captureActions != null && resolveFinalTarget;
			bool flag23 = flag6 && resolveFinalTarget && (renderingData.cameraData.antialiasing == AntialiasingMode.FastApproximateAntialiasing || (renderingData.cameraData.imageScalingMode == ImageScalingMode.Upscaling && renderingData.cameraData.upscalingFilter != ImageUpscalingFilter.Linear) || (renderingData.cameraData.IsTemporalAAEnabled() && renderingData.cameraData.taaSettings.contrastAdaptiveSharpening > 0f));
			bool flag24 = !flag22 && !flag20 && !flag23;
			bool flag25 = base.DebugHandler == null || !base.DebugHandler.HDRDebugViewIsActive(ref ptr);
			if (flag5)
			{
				RenderTextureDescriptor compatibleDescriptor = PostProcessPass.GetCompatibleDescriptor(cameraTargetDescriptor, cameraTargetDescriptor.width, cameraTargetDescriptor.height, cameraTargetDescriptor.graphicsFormat, DepthBits.None);
				RenderingUtils.ReAllocateIfNeeded(ref this.m_PostProcessPasses.m_AfterPostProcessColor, in compatibleDescriptor, FilterMode.Point, TextureWrapMode.Clamp, false, 1, 0f, "_AfterPostProcessTexture");
			}
			if (resolveFinalTarget)
			{
				this.SetupFinalPassDebug(ref ptr);
				if (flag5)
				{
					bool flag26 = flag24 && flag25;
					PostProcessPass postProcessPass = this.postProcessPass;
					bool flag27 = flag24;
					RTHandle rthandle = this.colorGradingLut;
					postProcessPass.Setup(in cameraTargetDescriptor, in this.m_ActiveCameraColorAttachment, flag27, in this.m_ActiveCameraDepthAttachment, in rthandle, in this.m_MotionVectorColor, flag23, flag26);
					base.EnqueuePass(this.postProcessPass);
				}
				RTHandle activeCameraColorAttachment = this.m_ActiveCameraColorAttachment;
				if (flag23)
				{
					this.finalPostProcessPass.SetupFinalPass(in activeCameraColorAttachment, true, flag25);
					base.EnqueuePass(this.finalPostProcessPass);
				}
				if (renderingData.cameraData.captureActions != null)
				{
					base.EnqueuePass(this.m_CapturePass);
				}
				if (!flag23 && (!flag5 || flag20 || flag22) && !(this.m_ActiveCameraColorAttachment.nameID == this.m_XRTargetHandleAlias.nameID))
				{
					this.m_FinalBlitPass.Setup(cameraTargetDescriptor, activeCameraColorAttachment);
					base.EnqueuePass(this.m_FinalBlitPass);
				}
				if (rendersOverlayUI && !isHDROutputActive)
				{
					base.EnqueuePass(this.m_DrawOverlayUIPass);
				}
				if (ptr.xr.enabled && !(this.m_ActiveCameraDepthAttachment.nameID == ptr.xr.renderTarget) && ptr.xr.copyDepth)
				{
					this.m_XRCopyDepthPass.Setup(this.m_ActiveCameraDepthAttachment, this.m_XRTargetHandleAlias);
					this.m_XRCopyDepthPass.CopyToDepth = true;
					base.EnqueuePass(this.m_XRCopyDepthPass);
					return;
				}
			}
			else if (flag5)
			{
				PostProcessPass postProcessPass2 = this.postProcessPass;
				bool flag28 = false;
				RTHandle rthandle = this.colorGradingLut;
				postProcessPass2.Setup(in cameraTargetDescriptor, in this.m_ActiveCameraColorAttachment, flag28, in this.m_ActiveCameraDepthAttachment, in rthandle, in this.m_MotionVectorColor, false, false);
				base.EnqueuePass(this.postProcessPass);
			}
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x0002F468 File Offset: 0x0002D668
		public override void SetupLights(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			this.m_ForwardLights.Setup(context, ref renderingData);
			if (this.renderingModeActual == RenderingMode.Deferred)
			{
				this.m_DeferredLights.SetupLights(context, ref renderingData);
			}
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x0002F490 File Offset: 0x0002D690
		public override void SetupCullingParameters(ref ScriptableCullingParameters cullingParameters, ref CameraData cameraData)
		{
			if (this.renderingModeActual == RenderingMode.ForwardPlus)
			{
				cullingParameters.cullingOptions |= CullingOptions.DisablePerObjectCulling;
			}
			bool flag = !UniversalRenderPipeline.asset.supportsMainLightShadows && !UniversalRenderPipeline.asset.supportsAdditionalLightShadows;
			bool flag2 = Mathf.Approximately(cameraData.maxShadowDistance, 0f);
			if (flag || flag2)
			{
				cullingParameters.cullingOptions &= ~CullingOptions.ShadowCasters;
			}
			if (this.renderingModeActual == RenderingMode.Deferred)
			{
				cullingParameters.maximumVisibleLights = 65535;
			}
			else if (this.renderingModeActual == RenderingMode.ForwardPlus)
			{
				cullingParameters.maximumVisibleLights = UniversalRenderPipeline.maxVisibleAdditionalLights;
				cullingParameters.reflectionProbeSortingCriteria = ReflectionProbeSortingCriteria.None;
			}
			else
			{
				cullingParameters.maximumVisibleLights = UniversalRenderPipeline.maxVisibleAdditionalLights + 1;
			}
			cullingParameters.shadowDistance = cameraData.maxShadowDistance;
			cullingParameters.conservativeEnclosingSphere = UniversalRenderPipeline.asset.conservativeEnclosingSphere;
			cullingParameters.numIterationsEnclosingSphere = UniversalRenderPipeline.asset.numIterationsEnclosingSphere;
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x0002F561 File Offset: 0x0002D761
		public override void FinishRendering(CommandBuffer cmd)
		{
			this.m_ColorBufferSystem.Clear();
			this.m_ActiveCameraColorAttachment = null;
			this.m_ActiveCameraDepthAttachment = null;
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x0002F57C File Offset: 0x0002D77C
		private void EnqueueDeferred(ref RenderingData renderingData, bool hasDepthPrepass, bool hasNormalPrepass, bool hasRenderingLayerPrepass, bool applyMainShadow, bool applyAdditionalShadow)
		{
			this.m_DeferredLights.Setup(ref renderingData, applyAdditionalShadow ? this.m_AdditionalLightsShadowCasterPass : null, hasDepthPrepass, hasNormalPrepass, hasRenderingLayerPrepass, this.m_DepthTexture, this.m_ActiveCameraDepthAttachment, this.m_ActiveCameraColorAttachment);
			if (this.useRenderPassEnabled && this.m_DeferredLights.UseRenderPass)
			{
				this.m_GBufferPass.Configure(null, renderingData.cameraData.cameraTargetDescriptor);
				this.m_DeferredPass.Configure(null, renderingData.cameraData.cameraTargetDescriptor);
			}
			base.EnqueuePass(this.m_GBufferPass);
			if (!this.useRenderPassEnabled || !this.m_DeferredLights.UseRenderPass)
			{
				this.m_GBufferCopyDepthPass.Setup(this.m_CameraDepthAttachment, this.m_DepthTexture);
				base.EnqueuePass(this.m_GBufferCopyDepthPass);
			}
			base.EnqueuePass(this.m_DeferredPass);
			base.EnqueuePass(this.m_RenderOpaqueForwardOnlyPass);
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x0002F658 File Offset: 0x0002D858
		private UniversalRenderer.RenderPassInputSummary GetRenderPassInputs(ref RenderingData renderingData)
		{
			RenderPassEvent renderPassEvent = ((this.m_RenderingMode == RenderingMode.Deferred) ? RenderPassEvent.BeforeRenderingGbuffer : RenderPassEvent.BeforeRenderingOpaques);
			UniversalRenderer.RenderPassInputSummary renderPassInputSummary = default(UniversalRenderer.RenderPassInputSummary);
			renderPassInputSummary.requiresDepthNormalAtEvent = RenderPassEvent.BeforeRenderingOpaques;
			renderPassInputSummary.requiresDepthTextureEarliestEvent = RenderPassEvent.BeforeRenderingPostProcessing;
			for (int i = 0; i < base.activeRenderPassQueue.Count; i++)
			{
				ScriptableRenderPass scriptableRenderPass = base.activeRenderPassQueue[i];
				bool flag = (scriptableRenderPass.input & ScriptableRenderPassInput.Depth) > ScriptableRenderPassInput.None;
				bool flag2 = (scriptableRenderPass.input & ScriptableRenderPassInput.Normal) > ScriptableRenderPassInput.None;
				bool flag3 = (scriptableRenderPass.input & ScriptableRenderPassInput.Color) > ScriptableRenderPassInput.None;
				bool flag4 = (scriptableRenderPass.input & ScriptableRenderPassInput.Motion) > ScriptableRenderPassInput.None;
				bool flag5 = scriptableRenderPass.renderPassEvent <= renderPassEvent;
				if (scriptableRenderPass is DBufferRenderPass)
				{
					renderPassInputSummary.requiresColorTextureCreated = true;
				}
				renderPassInputSummary.requiresDepthTexture = renderPassInputSummary.requiresDepthTexture || flag;
				renderPassInputSummary.requiresDepthPrepass |= flag2 || (flag && flag5);
				renderPassInputSummary.requiresNormalsTexture = renderPassInputSummary.requiresNormalsTexture || flag2;
				renderPassInputSummary.requiresColorTexture = renderPassInputSummary.requiresColorTexture || flag3;
				renderPassInputSummary.requiresMotionVectors = renderPassInputSummary.requiresMotionVectors || flag4;
				if (flag)
				{
					renderPassInputSummary.requiresDepthTextureEarliestEvent = (RenderPassEvent)Mathf.Min((int)scriptableRenderPass.renderPassEvent, (int)renderPassInputSummary.requiresDepthTextureEarliestEvent);
				}
				if (flag2 || flag)
				{
					renderPassInputSummary.requiresDepthNormalAtEvent = (RenderPassEvent)Mathf.Min((int)scriptableRenderPass.renderPassEvent, (int)renderPassInputSummary.requiresDepthNormalAtEvent);
				}
			}
			if (renderingData.cameraData.IsTemporalAAEnabled())
			{
				renderPassInputSummary.requiresMotionVectors = true;
			}
			if (renderPassInputSummary.requiresMotionVectors)
			{
				renderPassInputSummary.requiresDepthTexture = true;
				renderPassInputSummary.requiresDepthTextureEarliestEvent = (RenderPassEvent)Mathf.Min((int)this.m_MotionVectorPass.renderPassEvent, (int)renderPassInputSummary.requiresDepthTextureEarliestEvent);
			}
			return renderPassInputSummary;
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x0002F7E0 File Offset: 0x0002D9E0
		private void CreateCameraRenderTarget(ScriptableRenderContext context, ref RenderTextureDescriptor descriptor, bool primedDepth, CommandBuffer cmd, ref CameraData cameraData)
		{
			using (new ProfilingScope(null, UniversalRenderer.Profiling.createCameraRenderTarget))
			{
				if (this.m_ColorBufferSystem.PeekBackBuffer() == null || this.m_ColorBufferSystem.PeekBackBuffer().nameID != BuiltinRenderTextureType.CameraTarget)
				{
					this.m_ActiveCameraColorAttachment = this.m_ColorBufferSystem.GetBackBuffer(cmd);
					base.ConfigureCameraColorTarget(this.m_ActiveCameraColorAttachment);
					cmd.SetGlobalTexture("_CameraColorTexture", this.m_ActiveCameraColorAttachment.nameID);
					cmd.SetGlobalTexture("_AfterPostProcessTexture", this.m_ActiveCameraColorAttachment.nameID);
				}
				if (this.m_CameraDepthAttachment == null || this.m_CameraDepthAttachment.nameID != BuiltinRenderTextureType.CameraTarget)
				{
					RenderTextureDescriptor renderTextureDescriptor = descriptor;
					renderTextureDescriptor.useMipMap = false;
					renderTextureDescriptor.autoGenerateMips = false;
					renderTextureDescriptor.bindMS = false;
					if (renderTextureDescriptor.msaaSamples > 1 && SystemInfo.supportsMultisampledTextures != 0)
					{
						if (this.IsDepthPrimingEnabled(ref cameraData))
						{
							renderTextureDescriptor.bindMS = true;
						}
						else
						{
							renderTextureDescriptor.bindMS = !RenderingUtils.MultisampleDepthResolveSupported() || !SystemInfo.supportsMultisampleAutoResolve || this.m_CopyDepthMode != CopyDepthMode.AfterTransparents;
						}
					}
					if (this.IsGLESDevice())
					{
						renderTextureDescriptor.bindMS = false;
					}
					renderTextureDescriptor.graphicsFormat = GraphicsFormat.None;
					renderTextureDescriptor.depthStencilFormat = GraphicsFormat.D32_SFloat_S8_UInt;
					RenderingUtils.ReAllocateIfNeeded(ref this.m_CameraDepthAttachment, in renderTextureDescriptor, FilterMode.Point, TextureWrapMode.Clamp, false, 1, 0f, "_CameraDepthAttachment");
					if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.Direct3D11)
					{
						RenderingUtils.ReAllocateIfNeeded(ref this.m_CameraDepthAttachment_D3d_11, in renderTextureDescriptor, FilterMode.Point, TextureWrapMode.Clamp, false, 1, 0f, "_CameraDepthAttachment_Temp");
						cmd.SetGlobalTexture(this.m_CameraDepthAttachment.name, this.m_CameraDepthAttachment_D3d_11.nameID);
					}
					else
					{
						cmd.SetGlobalTexture(this.m_CameraDepthAttachment.name, this.m_CameraDepthAttachment.nameID);
					}
					descriptor.depthStencilFormat = renderTextureDescriptor.depthStencilFormat;
					descriptor.depthBufferBits = renderTextureDescriptor.depthBufferBits;
				}
			}
			context.ExecuteCommandBuffer(cmd);
			cmd.Clear();
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x0002F9F8 File Offset: 0x0002DBF8
		private bool PlatformRequiresExplicitMsaaResolve()
		{
			return (!SystemInfo.supportsMultisampleAutoResolve || !Application.isMobilePlatform) && SystemInfo.graphicsDeviceType != GraphicsDeviceType.Metal;
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x0002FA18 File Offset: 0x0002DC18
		private bool RequiresIntermediateColorTexture(ref CameraData cameraData)
		{
			if (cameraData.renderType == CameraRenderType.Base && !cameraData.resolveFinalTarget)
			{
				return true;
			}
			if (this.renderingModeActual == RenderingMode.Deferred)
			{
				return true;
			}
			bool isSceneViewCamera = cameraData.isSceneViewCamera;
			RenderTextureDescriptor cameraTargetDescriptor = cameraData.cameraTargetDescriptor;
			int msaaSamples = cameraTargetDescriptor.msaaSamples;
			bool flag = cameraData.imageScalingMode > ImageScalingMode.None;
			bool flag2 = cameraTargetDescriptor.dimension == TextureDimension.Tex2D;
			bool flag3 = msaaSamples > 1 && this.PlatformRequiresExplicitMsaaResolve();
			bool flag4 = cameraData.targetTexture != null && !isSceneViewCamera;
			bool flag5 = cameraData.captureActions != null;
			if (cameraData.xr.enabled)
			{
				flag = false;
				flag2 = cameraData.xr.renderTargetDesc.dimension == cameraTargetDescriptor.dimension;
			}
			bool flag6 = (cameraData.postProcessEnabled && this.m_PostProcessPasses.isCreated) || cameraData.requiresOpaqueTexture || flag3 || !cameraData.isDefaultViewport;
			if (flag4)
			{
				return flag6;
			}
			return flag6 || isSceneViewCamera || flag || cameraData.isHdrEnabled || !flag2 || flag5 || cameraData.requireSrgbConversion;
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x0002FB24 File Offset: 0x0002DD24
		private bool CanCopyDepth(ref CameraData cameraData)
		{
			bool flag = cameraData.cameraTargetDescriptor.msaaSamples > 1;
			bool flag2 = SystemInfo.copyTextureSupport > CopyTextureSupport.None;
			bool flag3 = RenderingUtils.SupportsRenderTextureFormat(RenderTextureFormat.Depth);
			bool flag4 = !flag && (flag3 || flag2);
			bool flag5 = flag && SystemInfo.supportsMultisampledTextures != 0;
			return (!this.IsGLESDevice() || !flag5) && (flag4 || flag5);
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x0002FB78 File Offset: 0x0002DD78
		internal override void SwapColorBuffer(CommandBuffer cmd)
		{
			this.m_ColorBufferSystem.Swap();
			if (this.m_ActiveCameraDepthAttachment.nameID != BuiltinRenderTextureType.CameraTarget)
			{
				base.ConfigureCameraTarget(this.m_ColorBufferSystem.GetBackBuffer(cmd), this.m_ActiveCameraDepthAttachment);
			}
			else
			{
				base.ConfigureCameraColorTarget(this.m_ColorBufferSystem.GetBackBuffer(cmd));
			}
			this.m_ActiveCameraColorAttachment = this.m_ColorBufferSystem.GetBackBuffer(cmd);
			cmd.SetGlobalTexture("_CameraColorTexture", this.m_ActiveCameraColorAttachment.nameID);
			cmd.SetGlobalTexture("_AfterPostProcessTexture", this.m_ActiveCameraColorAttachment.nameID);
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x0002FC12 File Offset: 0x0002DE12
		internal override RTHandle GetCameraColorFrontBuffer(CommandBuffer cmd)
		{
			return this.m_ColorBufferSystem.GetFrontBuffer(cmd);
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x0002FC20 File Offset: 0x0002DE20
		internal override RTHandle GetCameraColorBackBuffer(CommandBuffer cmd)
		{
			return this.m_ColorBufferSystem.GetBackBuffer(cmd);
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x0002FC2E File Offset: 0x0002DE2E
		internal override void EnableSwapBufferMSAA(bool enable)
		{
			this.m_ColorBufferSystem.EnableMSAA(enable);
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x0002FC3C File Offset: 0x0002DE3C
		private void CleanupRenderGraphResources()
		{
			RTHandle renderGraphCameraColorHandle = UniversalRenderer.m_RenderGraphCameraColorHandle;
			if (renderGraphCameraColorHandle != null)
			{
				renderGraphCameraColorHandle.Release();
			}
			RTHandle renderGraphCameraDepthHandle = UniversalRenderer.m_RenderGraphCameraDepthHandle;
			if (renderGraphCameraDepthHandle == null)
			{
				return;
			}
			renderGraphCameraDepthHandle.Release();
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x0002FC60 File Offset: 0x0002DE60
		internal static TextureHandle CreateRenderGraphTexture(RenderGraph renderGraph, RenderTextureDescriptor desc, string name, bool clear, FilterMode filterMode = FilterMode.Point, TextureWrapMode wrapMode = TextureWrapMode.Clamp)
		{
			TextureDesc textureDesc = new TextureDesc(desc.width, desc.height, false, false);
			textureDesc.dimension = desc.dimension;
			textureDesc.clearBuffer = clear;
			textureDesc.bindTextureMS = desc.bindMS;
			textureDesc.colorFormat = desc.graphicsFormat;
			textureDesc.depthBufferBits = (DepthBits)desc.depthBufferBits;
			textureDesc.slices = desc.volumeDepth;
			textureDesc.msaaSamples = (MSAASamples)desc.msaaSamples;
			textureDesc.name = name;
			textureDesc.enableRandomWrite = false;
			textureDesc.filterMode = filterMode;
			textureDesc.wrapMode = wrapMode;
			return renderGraph.CreateTexture(in textureDesc);
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x0002FD0C File Offset: 0x0002DF0C
		private bool RequiresColorAndDepthTextures(out bool createColorTexture, out bool createDepthTexture, ref RenderingData renderingData, UniversalRenderer.RenderPassInputSummary renderPassInputs)
		{
			bool isPreviewCamera = renderingData.cameraData.isPreviewCamera;
			bool flag = renderingData.cameraData.requiresDepthTexture || renderPassInputs.requiresDepthTexture || this.m_DepthPrimingMode == DepthPrimingMode.Forced;
			bool flag2 = false;
			bool flag3 = flag && !this.CanCopyDepth(ref renderingData.cameraData);
			flag3 |= renderingData.cameraData.isSceneViewCamera;
			flag3 = flag3 || flag2;
			flag3 = flag3 || isPreviewCamera;
			flag3 |= renderPassInputs.requiresDepthPrepass;
			flag3 |= renderPassInputs.requiresNormalsTexture;
			createColorTexture = base.rendererFeatures.Count != 0 && this.m_IntermediateTextureMode == IntermediateTextureMode.Always && !isPreviewCamera;
			createColorTexture |= this.RequiresIntermediateColorTexture(ref renderingData.cameraData);
			createColorTexture &= !isPreviewCamera;
			createDepthTexture = flag && !flag3;
			createDepthTexture |= !renderingData.cameraData.resolveFinalTarget;
			createDepthTexture |= this.renderingModeActual == RenderingMode.Deferred && !this.useRenderPassEnabled;
			createDepthTexture |= this.m_DepthPrimingMode == DepthPrimingMode.Forced;
			if (renderingData.cameraData.xr.enabled)
			{
				createColorTexture |= createDepthTexture;
			}
			bool flag4 = this.IsDepthPrimingEnabled(ref renderingData.cameraData);
			flag4 &= flag3 && (createDepthTexture | createColorTexture) && this.m_RenderingMode == RenderingMode.Forward && (renderingData.cameraData.renderType == CameraRenderType.Base || renderingData.cameraData.clearDepth);
			if (this.useRenderPassEnabled || flag4)
			{
				createColorTexture |= createDepthTexture;
			}
			if (renderingData.cameraData.renderType == CameraRenderType.Base)
			{
				bool flag5 = renderingData.cameraData.camera.sceneViewFilterMode == Camera.SceneViewFilterMode.ShowFiltered;
				bool flag6 = (createColorTexture | createDepthTexture) && !flag5;
				createDepthTexture = flag6;
			}
			return createColorTexture | createDepthTexture;
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x0002FEB4 File Offset: 0x0002E0B4
		private void CreateRenderGraphCameraRenderTargets(RenderGraph renderGraph, ScriptableRenderContext context, ref RenderingData renderingData)
		{
			ref CameraData ptr = ref renderingData.cameraData;
			RenderTargetIdentifier renderTargetIdentifier = ((ptr.targetTexture != null) ? new RenderTargetIdentifier(ptr.targetTexture) : BuiltinRenderTextureType.CameraTarget);
			if (ptr.xr.enabled)
			{
				renderTargetIdentifier = ptr.xr.renderTarget;
			}
			this.frameResources.backBufferColor = renderGraph.ImportBackbuffer(renderTargetIdentifier);
			UniversalRenderer.RenderPassInputSummary renderPassInputs = this.GetRenderPassInputs(ref renderingData);
			bool flag = false;
			bool flag2 = false;
			if (ptr.renderType == CameraRenderType.Base)
			{
				UniversalRenderer.m_UseIntermediateTexture = this.RequiresColorAndDepthTextures(out flag, out flag2, ref renderingData, renderPassInputs);
			}
			if (UniversalRenderer.m_UseIntermediateTexture)
			{
				RenderTextureDescriptor cameraTargetDescriptor = ptr.cameraTargetDescriptor;
				cameraTargetDescriptor.useMipMap = false;
				cameraTargetDescriptor.autoGenerateMips = false;
				cameraTargetDescriptor.depthBufferBits = 0;
				RenderingUtils.ReAllocateIfNeeded(ref UniversalRenderer.m_RenderGraphCameraColorHandle, in cameraTargetDescriptor, FilterMode.Bilinear, TextureWrapMode.Clamp, false, 1, 0f, "_CameraTargetAttachment");
			}
			if (UniversalRenderer.m_UseIntermediateTexture)
			{
				RenderTextureDescriptor cameraTargetDescriptor2 = ptr.cameraTargetDescriptor;
				cameraTargetDescriptor2.useMipMap = false;
				cameraTargetDescriptor2.autoGenerateMips = false;
				cameraTargetDescriptor2.bindMS = false;
				if (cameraTargetDescriptor2.msaaSamples > 1 && SystemInfo.supportsMultisampledTextures != 0)
				{
					cameraTargetDescriptor2.bindMS = true;
				}
				if (this.IsGLESDevice())
				{
					cameraTargetDescriptor2.bindMS = false;
				}
				cameraTargetDescriptor2.graphicsFormat = GraphicsFormat.None;
				cameraTargetDescriptor2.depthStencilFormat = GraphicsFormat.D32_SFloat_S8_UInt;
				RenderingUtils.ReAllocateIfNeeded(ref UniversalRenderer.m_RenderGraphCameraDepthHandle, in cameraTargetDescriptor2, FilterMode.Point, TextureWrapMode.Clamp, false, 1, 0f, "_CameraDepthAttachment");
			}
			if (UniversalRenderer.m_UseIntermediateTexture)
			{
				this.frameResources.cameraDepth = renderGraph.ImportTexture(UniversalRenderer.m_RenderGraphCameraDepthHandle);
				this.frameResources.cameraColor = renderGraph.ImportTexture(UniversalRenderer.m_RenderGraphCameraColorHandle);
				if (this.frameResources.cameraColor.IsValid())
				{
					UniversalRenderer.m_ActiveRenderGraphColor = this.frameResources.cameraColor;
					this.m_TargetIsBackbuffer = false;
				}
				if (this.frameResources.cameraDepth.IsValid())
				{
					UniversalRenderer.m_ActiveRenderGraphDepth = this.frameResources.cameraDepth;
				}
			}
			else
			{
				UniversalRenderer.m_ActiveRenderGraphColor = this.frameResources.backBufferColor;
				UniversalRenderer.m_ActiveRenderGraphDepth = this.frameResources.backBufferColor;
				this.m_TargetIsBackbuffer = true;
			}
			SupportedRenderingFeatures.active.motionVectors = true;
			RenderTextureDescriptor cameraTargetDescriptor3 = ptr.cameraTargetDescriptor;
			cameraTargetDescriptor3.graphicsFormat = GraphicsFormat.R16G16_SFloat;
			cameraTargetDescriptor3.depthBufferBits = 0;
			cameraTargetDescriptor3.msaaSamples = 1;
			this.frameResources.motionVectorColor = UniversalRenderer.CreateRenderGraphTexture(renderGraph, cameraTargetDescriptor3, "_MotionVectorTexture", true, FilterMode.Point, TextureWrapMode.Clamp);
			RenderTextureDescriptor cameraTargetDescriptor4 = ptr.cameraTargetDescriptor;
			cameraTargetDescriptor4.graphicsFormat = GraphicsFormat.None;
			cameraTargetDescriptor4.depthBufferBits = ((cameraTargetDescriptor4.depthBufferBits != 0) ? cameraTargetDescriptor4.depthBufferBits : 32);
			cameraTargetDescriptor4.msaaSamples = 1;
			this.frameResources.motionVectorDepth = UniversalRenderer.CreateRenderGraphTexture(renderGraph, cameraTargetDescriptor4, "_MotionVectorDepthTexture", true, FilterMode.Point, TextureWrapMode.Clamp);
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x0003013C File Offset: 0x0002E33C
		internal override void OnRecordRenderGraph(RenderGraph renderGraph, ScriptableRenderContext context, ref RenderingData renderingData)
		{
			this.useRenderPassEnabled = false;
			this.CreateRenderGraphCameraRenderTargets(renderGraph, context, ref renderingData);
			base.SetupRenderGraphCameraProperties(renderGraph, ref renderingData, this.m_TargetIsBackbuffer);
			base.ProcessVFXCameraCommand(renderGraph, ref renderingData);
			this.OnBeforeRendering(renderGraph, context, ref renderingData);
			this.OnMainRendering(renderGraph, context, ref renderingData);
			this.OnAfterRendering(renderGraph, context, ref renderingData);
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x0003018A File Offset: 0x0002E38A
		internal override void OnFinishRenderGraphRendering(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			if (this.renderingModeActual == RenderingMode.Deferred)
			{
				this.m_DeferredPass.OnCameraCleanup(renderingData.commandBuffer);
			}
			this.m_CopyDepthPass.OnCameraCleanup(renderingData.commandBuffer);
			this.m_DepthNormalPrepass.OnCameraCleanup(renderingData.commandBuffer);
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x000301C8 File Offset: 0x0002E3C8
		private void OnBeforeRendering(RenderGraph renderGraph, ScriptableRenderContext context, ref RenderingData renderingData)
		{
			bool flag = false;
			if (this.m_MainLightShadowCasterPass.Setup(ref renderingData))
			{
				flag = true;
				this.frameResources.mainShadowsTexture = this.m_MainLightShadowCasterPass.Render(renderGraph, ref renderingData);
			}
			if (this.m_AdditionalLightsShadowCasterPass.Setup(ref renderingData))
			{
				flag = true;
				this.frameResources.additionalShadowsTexture = this.m_AdditionalLightsShadowCasterPass.Render(renderGraph, ref renderingData);
			}
			if (flag)
			{
				base.SetupRenderGraphCameraProperties(renderGraph, ref renderingData, this.m_TargetIsBackbuffer);
			}
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x00030238 File Offset: 0x0002E438
		private void OnMainRendering(RenderGraph renderGraph, ScriptableRenderContext context, ref RenderingData renderingData)
		{
			RTClearFlags rtclearFlags = RTClearFlags.None;
			if (renderingData.cameraData.renderType == CameraRenderType.Base)
			{
				rtclearFlags = RTClearFlags.All;
			}
			else if (renderingData.cameraData.clearDepth)
			{
				rtclearFlags = RTClearFlags.Depth;
			}
			if (rtclearFlags != RTClearFlags.None)
			{
				ClearTargetsPass.Render(renderGraph, this, rtclearFlags, renderingData.cameraData.backgroundColor);
			}
			base.RecordCustomRenderGraphPasses(renderGraph, context, ref renderingData, RenderPassEvent.BeforeRenderingPrePasses);
			this.m_DepthNormalPrepass.Render(renderGraph, out this.frameResources.cameraDepthTexture, out this.frameResources.cameraNormalsTexture, ref renderingData);
			this.m_DepthPrepass.Render(renderGraph, out this.frameResources.cameraDepthTexture, ref renderingData);
			if (this.m_PostProcessPasses.isCreated)
			{
				this.m_PostProcessPasses.colorGradingLutPass.Render(renderGraph, out this.frameResources.internalColorLut, ref renderingData);
			}
			if (renderingData.cameraData.xr.hasValidOcclusionMesh)
			{
				this.m_XROcclusionMeshPass.m_IsActiveTargetBackBuffer = this.m_TargetIsBackbuffer;
				this.m_XROcclusionMeshPass.Render(renderGraph, in this.frameResources.cameraColor, in this.frameResources.cameraDepth, ref renderingData);
			}
			if (this.renderingModeActual == RenderingMode.Deferred)
			{
				this.m_DeferredLights.Setup(this.m_AdditionalLightsShadowCasterPass);
				if (this.m_DeferredLights != null)
				{
					this.m_DeferredLights.UseRenderPass = false;
					this.m_DeferredLights.ResolveMixedLightingMode(ref renderingData);
					this.m_DeferredLights.IsOverlay = renderingData.cameraData.renderType == CameraRenderType.Overlay;
				}
				this.m_GBufferPass.Render(renderGraph, UniversalRenderer.m_ActiveRenderGraphColor, UniversalRenderer.m_ActiveRenderGraphDepth, ref renderingData, ref this.frameResources);
				this.m_GBufferCopyDepthPass.Render(renderGraph, out this.frameResources.cameraDepthTexture, in this.frameResources.cameraDepth, ref renderingData);
				this.m_DeferredPass.Render(renderGraph, UniversalRenderer.m_ActiveRenderGraphColor, UniversalRenderer.m_ActiveRenderGraphDepth, this.frameResources.gbuffer, ref renderingData);
				this.m_RenderOpaqueForwardOnlyPass.Render(renderGraph, UniversalRenderer.m_ActiveRenderGraphColor, UniversalRenderer.m_ActiveRenderGraphDepth, this.frameResources.mainShadowsTexture, this.frameResources.additionalShadowsTexture, ref renderingData);
			}
			else
			{
				this.m_RenderOpaqueForwardPass.m_IsActiveTargetBackBuffer = this.m_TargetIsBackbuffer;
				this.m_RenderOpaqueForwardPass.Render(renderGraph, UniversalRenderer.m_ActiveRenderGraphColor, UniversalRenderer.m_ActiveRenderGraphDepth, this.frameResources.mainShadowsTexture, this.frameResources.additionalShadowsTexture, ref renderingData);
			}
			if (renderingData.cameraData.renderType == CameraRenderType.Base)
			{
				this.m_DrawSkyboxPass.Render(renderGraph, UniversalRenderer.m_ActiveRenderGraphColor, UniversalRenderer.m_ActiveRenderGraphDepth, ref renderingData);
			}
			this.m_CopyDepthPass.Render(renderGraph, out this.frameResources.cameraDepthTexture, in UniversalRenderer.m_ActiveRenderGraphDepth, ref renderingData);
			Downsampling opaqueDownsampling = UniversalRenderPipeline.asset.opaqueDownsampling;
			this.m_CopyColorPass.Render(renderGraph, out this.frameResources.cameraOpaqueTexture, in UniversalRenderer.m_ActiveRenderGraphColor, opaqueDownsampling, ref renderingData);
			this.m_MotionVectorPass.Render(renderGraph, ref this.frameResources.cameraDepth, in this.frameResources.motionVectorColor, in this.frameResources.motionVectorDepth, ref renderingData);
			this.m_RenderTransparentForwardPass.m_ShouldTransparentsReceiveShadows = !this.m_TransparentSettingsPass.Setup();
			this.m_RenderTransparentForwardPass.Render(renderGraph, UniversalRenderer.m_ActiveRenderGraphColor, UniversalRenderer.m_ActiveRenderGraphDepth, this.frameResources.mainShadowsTexture, this.frameResources.additionalShadowsTexture, ref renderingData);
			this.m_OnRenderObjectCallbackPass.Render(renderGraph, UniversalRenderer.m_ActiveRenderGraphColor, UniversalRenderer.m_ActiveRenderGraphDepth, ref renderingData);
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x00030554 File Offset: 0x0002E754
		private void OnAfterRendering(RenderGraph renderGraph, ScriptableRenderContext context, ref RenderingData renderingData)
		{
			bool flag = DebugDisplaySettings<UniversalRenderPipelineDebugDisplaySettings>.Instance.renderingSettings.sceneOverrideMode == DebugSceneOverrideMode.None;
			if (flag)
			{
				base.DrawRenderGraphGizmos(renderGraph, UniversalRenderer.m_ActiveRenderGraphColor, UniversalRenderer.m_ActiveRenderGraphDepth, GizmoSubset.PreImageEffects, ref renderingData);
			}
			if (flag)
			{
				base.DrawRenderGraphGizmos(renderGraph, UniversalRenderer.m_ActiveRenderGraphColor, UniversalRenderer.m_ActiveRenderGraphDepth, GizmoSubset.PostImageEffects, ref renderingData);
			}
			if (!this.m_TargetIsBackbuffer && renderingData.cameraData.resolveFinalTarget)
			{
				this.m_FinalBlitPass.Render(renderGraph, ref renderingData, this.frameResources.cameraColor, this.frameResources.backBufferColor, this.frameResources.overlayUITexture);
			}
		}

		// Token: 0x040005FE RID: 1534
		private const GraphicsFormat k_DepthStencilFormat = GraphicsFormat.D32_SFloat_S8_UInt;

		// Token: 0x040005FF RID: 1535
		private const int k_DepthBufferBits = 32;

		// Token: 0x04000600 RID: 1536
		private const int k_FinalBlitPassQueueOffset = 1;

		// Token: 0x04000601 RID: 1537
		private const int k_AfterFinalBlitPassQueueOffset = 2;

		// Token: 0x04000602 RID: 1538
		private static readonly List<ShaderTagId> k_DepthNormalsOnly = new List<ShaderTagId>
		{
			new ShaderTagId("DepthNormalsOnly")
		};

		// Token: 0x04000603 RID: 1539
		private bool m_Clustering;

		// Token: 0x04000604 RID: 1540
		private DepthOnlyPass m_DepthPrepass;

		// Token: 0x04000605 RID: 1541
		private DepthNormalOnlyPass m_DepthNormalPrepass;

		// Token: 0x04000606 RID: 1542
		private CopyDepthPass m_PrimedDepthCopyPass;

		// Token: 0x04000607 RID: 1543
		private MotionVectorRenderPass m_MotionVectorPass;

		// Token: 0x04000608 RID: 1544
		private MainLightShadowCasterPass m_MainLightShadowCasterPass;

		// Token: 0x04000609 RID: 1545
		private AdditionalLightsShadowCasterPass m_AdditionalLightsShadowCasterPass;

		// Token: 0x0400060A RID: 1546
		private GBufferPass m_GBufferPass;

		// Token: 0x0400060B RID: 1547
		private CopyDepthPass m_GBufferCopyDepthPass;

		// Token: 0x0400060C RID: 1548
		private DeferredPass m_DeferredPass;

		// Token: 0x0400060D RID: 1549
		private DrawObjectsPass m_RenderOpaqueForwardOnlyPass;

		// Token: 0x0400060E RID: 1550
		private DrawObjectsPass m_RenderOpaqueForwardPass;

		// Token: 0x0400060F RID: 1551
		private DrawObjectsWithRenderingLayersPass m_RenderOpaqueForwardWithRenderingLayersPass;

		// Token: 0x04000610 RID: 1552
		private DrawSkyboxPass m_DrawSkyboxPass;

		// Token: 0x04000611 RID: 1553
		private CopyDepthPass m_CopyDepthPass;

		// Token: 0x04000612 RID: 1554
		private CopyColorPass m_CopyColorPass;

		// Token: 0x04000613 RID: 1555
		private TransparentSettingsPass m_TransparentSettingsPass;

		// Token: 0x04000614 RID: 1556
		private DrawObjectsPass m_RenderTransparentForwardPass;

		// Token: 0x04000615 RID: 1557
		private InvokeOnRenderObjectCallbackPass m_OnRenderObjectCallbackPass;

		// Token: 0x04000616 RID: 1558
		private FinalBlitPass m_FinalBlitPass;

		// Token: 0x04000617 RID: 1559
		private CapturePass m_CapturePass;

		// Token: 0x04000618 RID: 1560
		private XROcclusionMeshPass m_XROcclusionMeshPass;

		// Token: 0x04000619 RID: 1561
		private CopyDepthPass m_XRCopyDepthPass;

		// Token: 0x0400061A RID: 1562
		private DrawScreenSpaceUIPass m_DrawOffscreenUIPass;

		// Token: 0x0400061B RID: 1563
		private DrawScreenSpaceUIPass m_DrawOverlayUIPass;

		// Token: 0x0400061C RID: 1564
		internal RenderTargetBufferSystem m_ColorBufferSystem;

		// Token: 0x0400061D RID: 1565
		internal RTHandle m_ActiveCameraColorAttachment;

		// Token: 0x0400061E RID: 1566
		private RTHandle m_ColorFrontBuffer;

		// Token: 0x0400061F RID: 1567
		internal RTHandle m_ActiveCameraDepthAttachment;

		// Token: 0x04000620 RID: 1568
		internal RTHandle m_CameraDepthAttachment;

		// Token: 0x04000621 RID: 1569
		private RTHandle m_XRTargetHandleAlias;

		// Token: 0x04000622 RID: 1570
		internal RTHandle m_CameraDepthAttachment_D3d_11;

		// Token: 0x04000623 RID: 1571
		internal RTHandle m_DepthTexture;

		// Token: 0x04000624 RID: 1572
		private RTHandle m_NormalsTexture;

		// Token: 0x04000625 RID: 1573
		private RTHandle m_DecalLayersTexture;

		// Token: 0x04000626 RID: 1574
		private RTHandle m_OpaqueColor;

		// Token: 0x04000627 RID: 1575
		private RTHandle m_MotionVectorColor;

		// Token: 0x04000628 RID: 1576
		private RTHandle m_MotionVectorDepth;

		// Token: 0x04000629 RID: 1577
		private ForwardLights m_ForwardLights;

		// Token: 0x0400062A RID: 1578
		private DeferredLights m_DeferredLights;

		// Token: 0x0400062B RID: 1579
		private RenderingMode m_RenderingMode;

		// Token: 0x0400062C RID: 1580
		private DepthPrimingMode m_DepthPrimingMode;

		// Token: 0x0400062D RID: 1581
		private CopyDepthMode m_CopyDepthMode;

		// Token: 0x0400062E RID: 1582
		private bool m_DepthPrimingRecommended;

		// Token: 0x0400062F RID: 1583
		private StencilState m_DefaultStencilState;

		// Token: 0x04000630 RID: 1584
		private LightCookieManager m_LightCookieManager;

		// Token: 0x04000631 RID: 1585
		private IntermediateTextureMode m_IntermediateTextureMode;

		// Token: 0x04000632 RID: 1586
		private bool m_VulkanEnablePreTransform;

		// Token: 0x04000633 RID: 1587
		private Material m_BlitMaterial;

		// Token: 0x04000634 RID: 1588
		private Material m_BlitHDRMaterial;

		// Token: 0x04000635 RID: 1589
		private Material m_CopyDepthMaterial;

		// Token: 0x04000636 RID: 1590
		private Material m_SamplingMaterial;

		// Token: 0x04000637 RID: 1591
		private Material m_StencilDeferredMaterial;

		// Token: 0x04000638 RID: 1592
		private Material m_CameraMotionVecMaterial;

		// Token: 0x04000639 RID: 1593
		private Material m_ObjectMotionVecMaterial;

		// Token: 0x0400063A RID: 1594
		private PostProcessPasses m_PostProcessPasses;

		// Token: 0x0400063B RID: 1595
		private static RTHandle m_RenderGraphCameraColorHandle;

		// Token: 0x0400063C RID: 1596
		private static RTHandle m_RenderGraphCameraDepthHandle;

		// Token: 0x0400063D RID: 1597
		internal static TextureHandle m_ActiveRenderGraphColor;

		// Token: 0x0400063E RID: 1598
		internal static TextureHandle m_ActiveRenderGraphDepth;

		// Token: 0x0400063F RID: 1599
		internal bool m_TargetIsBackbuffer;

		// Token: 0x04000640 RID: 1600
		internal UniversalRenderer.RenderGraphFrameResources frameResources = new UniversalRenderer.RenderGraphFrameResources();

		// Token: 0x04000641 RID: 1601
		private static bool m_UseIntermediateTexture = false;

		// Token: 0x020001C1 RID: 449
		private static class Profiling
		{
			// Token: 0x04000BAB RID: 2987
			private const string k_Name = "UniversalRenderer";

			// Token: 0x04000BAC RID: 2988
			public static readonly ProfilingSampler createCameraRenderTarget = new ProfilingSampler("UniversalRenderer.CreateCameraRenderTarget");
		}

		// Token: 0x020001C2 RID: 450
		private struct RenderPassInputSummary
		{
			// Token: 0x04000BAD RID: 2989
			internal bool requiresDepthTexture;

			// Token: 0x04000BAE RID: 2990
			internal bool requiresDepthPrepass;

			// Token: 0x04000BAF RID: 2991
			internal bool requiresNormalsTexture;

			// Token: 0x04000BB0 RID: 2992
			internal bool requiresColorTexture;

			// Token: 0x04000BB1 RID: 2993
			internal bool requiresColorTextureCreated;

			// Token: 0x04000BB2 RID: 2994
			internal bool requiresMotionVectors;

			// Token: 0x04000BB3 RID: 2995
			internal RenderPassEvent requiresDepthNormalAtEvent;

			// Token: 0x04000BB4 RID: 2996
			internal RenderPassEvent requiresDepthTextureEarliestEvent;
		}

		// Token: 0x020001C3 RID: 451
		internal class RenderGraphFrameResources
		{
			// Token: 0x04000BB5 RID: 2997
			internal TextureHandle backBufferColor;

			// Token: 0x04000BB6 RID: 2998
			internal TextureHandle cameraColor;

			// Token: 0x04000BB7 RID: 2999
			internal TextureHandle cameraDepth;

			// Token: 0x04000BB8 RID: 3000
			internal TextureHandle mainShadowsTexture;

			// Token: 0x04000BB9 RID: 3001
			internal TextureHandle additionalShadowsTexture;

			// Token: 0x04000BBA RID: 3002
			internal TextureHandle[] gbuffer;

			// Token: 0x04000BBB RID: 3003
			internal TextureHandle cameraOpaqueTexture;

			// Token: 0x04000BBC RID: 3004
			internal TextureHandle cameraDepthTexture;

			// Token: 0x04000BBD RID: 3005
			internal TextureHandle cameraNormalsTexture;

			// Token: 0x04000BBE RID: 3006
			internal TextureHandle motionVectorColor;

			// Token: 0x04000BBF RID: 3007
			internal TextureHandle motionVectorDepth;

			// Token: 0x04000BC0 RID: 3008
			internal TextureHandle internalColorLut;

			// Token: 0x04000BC1 RID: 3009
			internal TextureHandle overlayUITexture;
		}
	}
}
