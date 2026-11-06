using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.VFX;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000F4 RID: 244
	public sealed class UniversalRenderPipeline : RenderPipeline
	{
		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x060007B4 RID: 1972 RVA: 0x00030835 File Offset: 0x0002EA35
		public static float maxShadowBias
		{
			get
			{
				return 10f;
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x060007B5 RID: 1973 RVA: 0x0003083C File Offset: 0x0002EA3C
		public static float minRenderScale
		{
			get
			{
				return 0.1f;
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x060007B6 RID: 1974 RVA: 0x00030843 File Offset: 0x0002EA43
		public static float maxRenderScale
		{
			get
			{
				return 2f;
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x060007B7 RID: 1975 RVA: 0x0003084A File Offset: 0x0002EA4A
		public static int maxNumIterationsEnclosingSphere
		{
			get
			{
				return 1000;
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x060007B8 RID: 1976 RVA: 0x00030851 File Offset: 0x0002EA51
		public static int maxPerObjectLights
		{
			get
			{
				if (SystemInfo.graphicsDeviceType != GraphicsDeviceType.OpenGLES2)
				{
					return 8;
				}
				return 4;
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x060007B9 RID: 1977 RVA: 0x00030860 File Offset: 0x0002EA60
		public static int maxVisibleAdditionalLights
		{
			get
			{
				bool flag = GraphicsSettings.HasShaderDefine(BuiltinShaderDefine.SHADER_API_MOBILE);
				if (flag && (SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES2 || (SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES3 && Graphics.minOpenGLESVersion <= OpenGLESVersion.OpenGLES30)))
				{
					return 16;
				}
				if (!flag && SystemInfo.graphicsDeviceType != GraphicsDeviceType.OpenGLCore && SystemInfo.graphicsDeviceType != GraphicsDeviceType.OpenGLES2 && SystemInfo.graphicsDeviceType != GraphicsDeviceType.OpenGLES3)
				{
					return 256;
				}
				return 32;
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x060007BA RID: 1978 RVA: 0x000308B9 File Offset: 0x0002EAB9
		internal static int lightsPerTile
		{
			get
			{
				return (UniversalRenderPipeline.maxVisibleAdditionalLights + 31) / 32 * 32;
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x060007BB RID: 1979 RVA: 0x000308C9 File Offset: 0x0002EAC9
		internal static int maxZBinWords
		{
			get
			{
				return 4096;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x060007BC RID: 1980 RVA: 0x000308D0 File Offset: 0x0002EAD0
		internal static int maxTileWords
		{
			get
			{
				return ((UniversalRenderPipeline.maxVisibleAdditionalLights <= 32) ? 1024 : 4096) * 4;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x060007BD RID: 1981 RVA: 0x000308E9 File Offset: 0x0002EAE9
		internal static int maxVisibleReflectionProbes
		{
			get
			{
				return Math.Min(UniversalRenderPipeline.maxVisibleAdditionalLights, 64);
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x060007BE RID: 1982 RVA: 0x000308F7 File Offset: 0x0002EAF7
		public override RenderPipelineGlobalSettings defaultSettings
		{
			get
			{
				return this.m_GlobalSettings;
			}
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x000308FF File Offset: 0x0002EAFF
		public override string ToString()
		{
			UniversalRenderPipelineAsset universalRenderPipelineAsset = this.pipelineAsset;
			if (universalRenderPipelineAsset == null)
			{
				return null;
			}
			return universalRenderPipelineAsset.ToString();
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x00030914 File Offset: 0x0002EB14
		public UniversalRenderPipeline(UniversalRenderPipelineAsset asset)
		{
			this.pipelineAsset = asset;
			this.m_GlobalSettings = UniversalRenderPipelineGlobalSettings.instance;
			UniversalRenderPipeline.SetSupportedRenderingFeatures(this.pipelineAsset);
			RTHandles.Initialize(Screen.width, Screen.height);
			GraphicsSettings.useScriptableRenderPipelineBatching = asset.useSRPBatcher;
			if (((QualitySettings.antiAliasing > 0) ? QualitySettings.antiAliasing : 1) != asset.msaaSampleCount)
			{
				QualitySettings.antiAliasing = asset.msaaSampleCount;
			}
			XRSystem.SetDisplayMSAASamples((MSAASamples)Mathf.Clamp(Mathf.NextPowerOfTwo(QualitySettings.antiAliasing), 1, 8));
			XRSystem.SetRenderScale(asset.renderScale);
			Shader.globalRenderPipeline = "UniversalPipeline";
			Lightmapping.SetDelegate(UniversalRenderPipeline.lightsDelegate);
			CameraCaptureBridge.enabled = true;
			RenderingUtils.ClearSystemInfoCache();
			DecalProjector.defaultMaterial = asset.decalMaterial;
			UniversalRenderPipeline.s_RenderGraph = new RenderGraph("URPRenderGraph");
			UniversalRenderPipeline.useRenderGraph = false;
			UniversalRenderPipeline.s_RTHandlePool = new RTHandleResourcePool();
			DebugManager.instance.RefreshEditor();
			this.m_DebugDisplaySettingsUI.RegisterDebug(DebugDisplaySettings<UniversalRenderPipelineDebugDisplaySettings>.Instance);
			QualitySettings.enableLODCrossFade = asset.enableLODCrossFade;
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x00030A4C File Offset: 0x0002EC4C
		protected override void Dispose(bool disposing)
		{
			this.m_DebugDisplaySettingsUI.UnregisterDebug();
			Blitter.Cleanup();
			base.Dispose(disposing);
			this.pipelineAsset.DestroyRenderers();
			Shader.globalRenderPipeline = string.Empty;
			SupportedRenderingFeatures.active = new SupportedRenderingFeatures();
			ShaderData.instance.Dispose();
			XRSystem.Dispose();
			UniversalRenderPipeline.s_RenderGraph.Cleanup();
			UniversalRenderPipeline.s_RenderGraph = null;
			UniversalRenderPipeline.s_RTHandlePool.Cleanup();
			UniversalRenderPipeline.s_RTHandlePool = null;
			Lightmapping.ResetDelegate();
			CameraCaptureBridge.enabled = false;
			this.DisposeAdditionalCameraData();
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x00030AD0 File Offset: 0x0002ECD0
		private void DisposeAdditionalCameraData()
		{
			Camera[] allCameras = Camera.allCameras;
			for (int i = 0; i < allCameras.Length; i++)
			{
				UniversalAdditionalCameraData universalAdditionalCameraData;
				if (allCameras[i].TryGetComponent<UniversalAdditionalCameraData>(out universalAdditionalCameraData))
				{
					TaaPersistentData taaPersistentData = universalAdditionalCameraData.taaPersistentData;
					if (taaPersistentData != null)
					{
						taaPersistentData.DeallocateTargets();
					}
				}
			}
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x00030B0E File Offset: 0x0002ED0E
		protected override void Render(ScriptableRenderContext renderContext, Camera[] cameras)
		{
			this.Render(renderContext, new List<Camera>(cameras));
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x00030B20 File Offset: 0x0002ED20
		protected override void Render(ScriptableRenderContext renderContext, List<Camera> cameras)
		{
			UniversalRenderPipeline.useRenderGraph = false;
			this.SetHDRState(cameras);
			SupportedRenderingFeatures.active.rendersUIOverlay = UniversalRenderPipeline.HDROutputForAnyDisplayIsActive();
			using (new ProfilingScope(null, ProfilingSampler.Get<URPProfileId>(URPProfileId.UniversalRenderTotal)))
			{
				using (new ProfilingScope(null, UniversalRenderPipeline.Profiling.Pipeline.beginContextRendering))
				{
					RenderPipeline.BeginContextRendering(renderContext, cameras);
				}
				GraphicsSettings.lightsUseLinearIntensity = QualitySettings.activeColorSpace == ColorSpace.Linear;
				GraphicsSettings.lightsUseColorTemperature = true;
				GraphicsSettings.defaultRenderingLayerMask = 1U;
				UniversalRenderPipeline.SetupPerFrameShaderConstants();
				XRSystem.SetDisplayMSAASamples((MSAASamples)UniversalRenderPipeline.asset.msaaSampleCount);
				RTHandles.SetHardwareDynamicResolutionState(true);
				this.SortCameras(cameras);
				for (int i = 0; i < cameras.Count; i++)
				{
					Camera camera = cameras[i];
					if (UniversalRenderPipeline.IsGameCamera(camera))
					{
						UniversalRenderPipeline.RenderCameraStack(renderContext, camera);
					}
					else
					{
						using (new ProfilingScope(null, UniversalRenderPipeline.Profiling.Pipeline.beginCameraRendering))
						{
							RenderPipeline.BeginCameraRendering(renderContext, camera);
						}
						VFXManager.PrepareCamera(camera);
						UniversalRenderPipeline.UpdateVolumeFramework(camera, null);
						UniversalRenderPipeline.RenderSingleCameraInternal(renderContext, camera);
						using (new ProfilingScope(null, UniversalRenderPipeline.Profiling.Pipeline.endCameraRendering))
						{
							RenderPipeline.EndCameraRendering(renderContext, camera);
						}
					}
				}
				UniversalRenderPipeline.s_RenderGraph.EndFrame();
				UniversalRenderPipeline.s_RTHandlePool.PurgeUnusedResources(Time.frameCount);
				using (new ProfilingScope(null, UniversalRenderPipeline.Profiling.Pipeline.endContextRendering))
				{
					RenderPipeline.EndContextRendering(renderContext, cameras);
				}
			}
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x00030D00 File Offset: 0x0002EF00
		protected override bool IsRenderRequestSupported<RequestData>(Camera camera, RequestData data)
		{
			return data is RenderPipeline.StandardRequest || data is UniversalRenderPipeline.SingleCameraRequest;
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x00030D24 File Offset: 0x0002EF24
		protected override void ProcessRenderRequests<RequestData>(ScriptableRenderContext context, Camera camera, RequestData renderRequest)
		{
			RenderPipeline.StandardRequest standardRequest = renderRequest as RenderPipeline.StandardRequest;
			UniversalRenderPipeline.SingleCameraRequest singleCameraRequest = renderRequest as UniversalRenderPipeline.SingleCameraRequest;
			if (standardRequest != null || singleCameraRequest != null)
			{
				RenderTexture renderTexture = ((standardRequest != null) ? standardRequest.destination : singleCameraRequest.destination);
				int num = ((standardRequest != null) ? standardRequest.mipLevel : singleCameraRequest.mipLevel);
				int num2 = ((standardRequest != null) ? standardRequest.slice : singleCameraRequest.slice);
				int num3 = (int)((standardRequest != null) ? standardRequest.face : singleCameraRequest.face);
				RenderTexture targetTexture = camera.targetTexture;
				RenderTexture renderTexture2 = null;
				RenderTextureDescriptor renderTextureDescriptor = renderTexture.descriptor;
				if (renderTexture.dimension == TextureDimension.Cube)
				{
					renderTextureDescriptor = default(RenderTextureDescriptor);
				}
				renderTextureDescriptor.colorFormat = renderTexture.format;
				renderTextureDescriptor.volumeDepth = 1;
				renderTextureDescriptor.msaaSamples = renderTexture.descriptor.msaaSamples;
				renderTextureDescriptor.dimension = TextureDimension.Tex2D;
				renderTextureDescriptor.width = renderTexture.width / (int)Math.Pow(2.0, (double)num);
				renderTextureDescriptor.height = renderTexture.height / (int)Math.Pow(2.0, (double)num);
				renderTextureDescriptor.width = Mathf.Max(1, renderTextureDescriptor.width);
				renderTextureDescriptor.height = Mathf.Max(1, renderTextureDescriptor.height);
				if (renderTexture.dimension != TextureDimension.Tex2D || num != 0)
				{
					renderTexture2 = RenderTexture.GetTemporary(renderTextureDescriptor);
				}
				camera.targetTexture = (renderTexture2 ? renderTexture2 : renderTexture);
				List<Camera> list;
				using (ListPool<Camera>.Get(out list))
				{
					list.Add(camera);
					if (standardRequest != null)
					{
						this.Render(context, list.ToArray());
					}
					else
					{
						using (new ProfilingScope(null, UniversalRenderPipeline.Profiling.Pipeline.beginContextRendering))
						{
							RenderPipeline.BeginContextRendering(context, list);
						}
						using (new ProfilingScope(null, UniversalRenderPipeline.Profiling.Pipeline.beginCameraRendering))
						{
							RenderPipeline.BeginCameraRendering(context, camera);
						}
						UniversalAdditionalCameraData universalAdditionalCameraData;
						camera.gameObject.TryGetComponent<UniversalAdditionalCameraData>(out universalAdditionalCameraData);
						UniversalRenderPipeline.RenderSingleCameraInternal(context, camera, ref universalAdditionalCameraData);
						using (new ProfilingScope(null, UniversalRenderPipeline.Profiling.Pipeline.endCameraRendering))
						{
							RenderPipeline.EndCameraRendering(context, camera);
						}
						using (new ProfilingScope(null, UniversalRenderPipeline.Profiling.Pipeline.endContextRendering))
						{
							RenderPipeline.EndContextRendering(context, list);
						}
					}
				}
				if (renderTexture2)
				{
					switch (renderTexture.dimension)
					{
					case TextureDimension.Tex2D:
					case TextureDimension.Tex3D:
					case TextureDimension.Tex2DArray:
						Graphics.CopyTexture(renderTexture2, 0, 0, renderTexture, num2, num);
						break;
					case TextureDimension.Cube:
					case TextureDimension.CubeArray:
						Graphics.CopyTexture(renderTexture2, 0, 0, renderTexture, num3 + num2 * 6, num);
						break;
					}
				}
				camera.targetTexture = targetTexture;
				Graphics.SetRenderTarget(targetTexture);
				RenderTexture.ReleaseTemporary(renderTexture2);
				return;
			}
			Debug.LogWarning("The given RenderRequest type: " + typeof(RequestData).FullName + ", is either invalid or unsupported by the current pipeline");
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x0003102C File Offset: 0x0002F22C
		[Obsolete("RenderSingleCamera is obsolete, please use RenderPipeline.SubmitRenderRequest with UniversalRenderer.SingleCameraRequest as RequestData type", false)]
		public static void RenderSingleCamera(ScriptableRenderContext context, Camera camera)
		{
			UniversalRenderPipeline.RenderSingleCameraInternal(context, camera);
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x00031038 File Offset: 0x0002F238
		internal static void RenderSingleCameraInternal(ScriptableRenderContext context, Camera camera)
		{
			UniversalAdditionalCameraData universalAdditionalCameraData = null;
			if (UniversalRenderPipeline.IsGameCamera(camera))
			{
				camera.gameObject.TryGetComponent<UniversalAdditionalCameraData>(out universalAdditionalCameraData);
			}
			UniversalRenderPipeline.RenderSingleCameraInternal(context, camera, ref universalAdditionalCameraData);
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x00031068 File Offset: 0x0002F268
		internal static void RenderSingleCameraInternal(ScriptableRenderContext context, Camera camera, ref UniversalAdditionalCameraData additionalCameraData)
		{
			if (additionalCameraData != null && additionalCameraData.renderType != CameraRenderType.Base)
			{
				Debug.LogWarning("Only Base cameras can be rendered with standalone RenderSingleCamera. Camera will be skipped.");
				return;
			}
			CameraData cameraData;
			UniversalRenderPipeline.InitializeCameraData(camera, additionalCameraData, true, out cameraData);
			UniversalRenderPipeline.InitializeAdditionalCameraData(camera, additionalCameraData, true, ref cameraData);
			UniversalRenderPipeline.RenderSingleCamera(context, ref cameraData);
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x000310B4 File Offset: 0x0002F2B4
		private static bool TryGetCullingParameters(CameraData cameraData, out ScriptableCullingParameters cullingParams)
		{
			if (cameraData.xr.enabled)
			{
				cullingParams = cameraData.xr.cullingParams;
				if (!cameraData.camera.usePhysicalProperties && !XRGraphicsAutomatedTests.enabled)
				{
					cameraData.camera.fieldOfView = 57.29578f * Mathf.Atan(1f / cullingParams.stereoProjectionMatrix.m11) * 2f;
				}
				return true;
			}
			return cameraData.camera.TryGetCullingParameters(false, out cullingParams);
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x00031134 File Offset: 0x0002F334
		private static void RenderSingleCamera(ScriptableRenderContext context, ref CameraData cameraData)
		{
			Camera camera = cameraData.camera;
			ScriptableRenderer renderer = cameraData.renderer;
			if (renderer == null)
			{
				Debug.LogWarning(string.Format("Trying to render {0} with an invalid renderer. Camera rendering will be skipped.", camera.name));
				return;
			}
			ScriptableCullingParameters scriptableCullingParameters;
			if (!UniversalRenderPipeline.TryGetCullingParameters(cameraData, out scriptableCullingParameters))
			{
				return;
			}
			ScriptableRenderer.current = renderer;
			bool isSceneViewCamera = cameraData.isSceneViewCamera;
			CommandBuffer commandBuffer = CommandBufferPool.Get();
			CommandBuffer commandBuffer2 = (cameraData.xr.enabled ? null : commandBuffer);
			ProfilingSampler profilingSampler = UniversalRenderPipeline.Profiling.TryGetOrAddCameraSampler(camera);
			using (new ProfilingScope(commandBuffer2, profilingSampler))
			{
				renderer.Clear(cameraData.renderType);
				using (new ProfilingScope(null, UniversalRenderPipeline.Profiling.Pipeline.Renderer.setupCullingParameters))
				{
					renderer.OnPreCullRenderPasses(in cameraData);
					renderer.SetupCullingParameters(ref scriptableCullingParameters, ref cameraData);
				}
				context.ExecuteCommandBuffer(commandBuffer);
				commandBuffer.Clear();
				UniversalRenderPipeline.SetupPerCameraShaderConstants(commandBuffer);
				if (camera.cameraType == CameraType.Reflection || camera.cameraType == CameraType.Preview)
				{
					ScriptableRenderContext.EmitGeometryForCamera(camera);
				}
				UniversalAdditionalCameraData universalAdditionalCameraData;
				if (camera.TryGetComponent<UniversalAdditionalCameraData>(out universalAdditionalCameraData))
				{
					universalAdditionalCameraData.motionVectorsPersistentData.Update(ref cameraData);
				}
				if (cameraData.taaPersistentData != null)
				{
					UniversalRenderPipeline.UpdateTemporalAATargets(ref cameraData);
				}
				RTHandles.SetReferenceSize(cameraData.cameraTargetDescriptor.width, cameraData.cameraTargetDescriptor.height);
				CullingResults cullingResults = context.Cull(ref scriptableCullingParameters);
				RenderingData renderingData;
				UniversalRenderPipeline.InitializeRenderingData(UniversalRenderPipeline.asset, ref cameraData, ref cullingResults, commandBuffer, out renderingData);
				renderer.AddRenderPasses(ref renderingData);
				if (UniversalRenderPipeline.useRenderGraph)
				{
					UniversalRenderPipeline.RecordAndExecuteRenderGraph(UniversalRenderPipeline.s_RenderGraph, context, ref renderingData);
					renderer.FinishRenderGraphRendering(context, ref renderingData);
				}
				else
				{
					using (new ProfilingScope(null, UniversalRenderPipeline.Profiling.Pipeline.Renderer.setup))
					{
						renderer.Setup(context, ref renderingData);
					}
					renderer.Execute(context, ref renderingData);
				}
			}
			context.ExecuteCommandBuffer(commandBuffer);
			CommandBufferPool.Release(commandBuffer);
			using (new ProfilingScope(null, UniversalRenderPipeline.Profiling.Pipeline.Context.submit))
			{
				if (renderer.useRenderPassEnabled && !context.SubmitForRenderPassValidation())
				{
					renderer.useRenderPassEnabled = false;
					CoreUtils.SetKeyword(commandBuffer, "_RENDER_PASS_ENABLED", false);
					Debug.LogWarning("Rendering command not supported inside a native RenderPass found. Falling back to non-RenderPass rendering path");
				}
				context.Submit();
			}
			ScriptableRenderer.current = null;
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x000313A4 File Offset: 0x0002F5A4
		private static void RenderCameraStack(ScriptableRenderContext context, Camera baseCamera)
		{
			using (new ProfilingScope(null, ProfilingSampler.Get<URPProfileId>(URPProfileId.RenderCameraStack)))
			{
				UniversalAdditionalCameraData universalAdditionalCameraData;
				baseCamera.TryGetComponent<UniversalAdditionalCameraData>(out universalAdditionalCameraData);
				if (!(universalAdditionalCameraData != null) || universalAdditionalCameraData.renderType != CameraRenderType.Overlay)
				{
					ScriptableRenderer scriptableRenderer = ((universalAdditionalCameraData != null) ? universalAdditionalCameraData.scriptableRenderer : null);
					List<Camera> list = ((scriptableRenderer != null && scriptableRenderer.SupportsCameraStackingType(CameraRenderType.Base)) ? ((universalAdditionalCameraData != null) ? universalAdditionalCameraData.cameraStack : null) : null);
					bool flag = universalAdditionalCameraData != null && universalAdditionalCameraData.renderPostProcessing;
					bool flag2 = UniversalRenderPipeline.HDROutputForMainDisplayIsActive();
					int num = UniversalRenderPipeline.asset.m_RendererDataList.Length;
					int num2 = -1;
					if (list != null)
					{
						Type type = ((universalAdditionalCameraData != null) ? universalAdditionalCameraData.scriptableRenderer.GetType() : null);
						bool flag3 = false;
						UniversalRenderPipeline.cameraStackRequiresDepthForPostprocessing = false;
						for (int i = 0; i < list.Count; i++)
						{
							Camera camera = list[i];
							if (camera == null)
							{
								flag3 = true;
							}
							else if (camera.isActiveAndEnabled)
							{
								UniversalAdditionalCameraData universalAdditionalCameraData2;
								camera.TryGetComponent<UniversalAdditionalCameraData>(out universalAdditionalCameraData2);
								Type type2 = ((universalAdditionalCameraData2 != null) ? universalAdditionalCameraData2.scriptableRenderer.GetType() : null);
								if (type2 != type)
								{
									Debug.LogWarning(string.Concat(new string[] { "Only cameras with compatible renderer types can be stacked. The camera: ", camera.name, " are using the renderer ", type2.Name, ", but the base camera: ", baseCamera.name, " are using ", type.Name, ". Will skip rendering" }));
								}
								else if ((universalAdditionalCameraData2.scriptableRenderer.SupportedCameraStackingTypes() & 2) == 0)
								{
									Debug.LogWarning(string.Concat(new string[]
									{
										"The camera: ",
										camera.name,
										" is using a renderer of type ",
										scriptableRenderer.GetType().Name,
										" which does not support Overlay cameras in it's current state."
									}));
								}
								else if (universalAdditionalCameraData2 == null || universalAdditionalCameraData2.renderType != CameraRenderType.Overlay)
								{
									Debug.LogWarning("Stack can only contain Overlay cameras. The camera: " + camera.name + " " + string.Format("has a type {0} that is not supported. Will skip rendering.", universalAdditionalCameraData2.renderType));
								}
								else
								{
									UniversalRenderPipeline.cameraStackRequiresDepthForPostprocessing |= UniversalRenderPipeline.CheckPostProcessForDepth();
									flag |= universalAdditionalCameraData2.renderPostProcessing;
									num2 = i;
								}
							}
						}
						if (flag3)
						{
							universalAdditionalCameraData.UpdateCameraStack();
						}
					}
					flag &= SystemInfo.graphicsDeviceType != GraphicsDeviceType.OpenGLES2;
					bool flag4 = num2 != -1;
					bool flag5 = false;
					bool flag6 = universalAdditionalCameraData == null || universalAdditionalCameraData.allowXRRendering;
					XRLayout xrlayout = XRSystem.NewLayout();
					xrlayout.AddCamera(baseCamera, flag6);
					foreach (ValueTuple<Camera, XRPass> valueTuple in xrlayout.GetActivePasses())
					{
						XRPass item = valueTuple.Item2;
						if (item.enabled)
						{
							flag5 = true;
							UniversalRenderPipeline.UpdateCameraStereoMatrices(baseCamera, item);
						}
						using (new ProfilingScope(null, UniversalRenderPipeline.Profiling.Pipeline.beginCameraRendering))
						{
							RenderPipeline.BeginCameraRendering(context, baseCamera);
						}
						UniversalRenderPipeline.UpdateVolumeFramework(baseCamera, universalAdditionalCameraData);
						CameraData cameraData;
						UniversalRenderPipeline.InitializeCameraData(baseCamera, universalAdditionalCameraData, !flag4, out cameraData);
						RenderTextureDescriptor cameraTargetDescriptor = cameraData.cameraTargetDescriptor;
						if (item.enabled)
						{
							cameraData.xr = item;
							XRPass xr = cameraData.xr;
							UniversalRenderPipeline.UpdateCameraData(ref cameraData, in xr);
							xrlayout.ReconfigurePass(cameraData.xr, baseCamera);
							XRSystemUniversal.BeginLateLatching(baseCamera, cameraData.xrUniversal);
						}
						UniversalRenderPipeline.InitializeAdditionalCameraData(baseCamera, universalAdditionalCameraData, !flag4, ref cameraData);
						VFXManager.PrepareCamera(baseCamera);
						cameraData.postProcessingRequiresDepthTexture |= UniversalRenderPipeline.cameraStackRequiresDepthForPostprocessing;
						bool flag7 = flag2;
						if (item.enabled)
						{
							flag7 = item.isHDRDisplayOutputActive;
						}
						bool flag8 = UniversalRenderPipeline.asset.supportsHDR && flag7 && baseCamera.targetTexture == null && (baseCamera.cameraType == CameraType.Game || baseCamera.cameraType == CameraType.VR) && cameraData.allowHDROutput;
						cameraData.stackAnyPostProcessingEnabled = flag;
						cameraData.stackLastCameraOutputToHDR = flag8;
						UniversalRenderPipeline.RenderSingleCamera(context, ref cameraData);
						using (new ProfilingScope(null, UniversalRenderPipeline.Profiling.Pipeline.endCameraRendering))
						{
							RenderPipeline.EndCameraRendering(context, baseCamera);
						}
						if (cameraData.xr.enabled)
						{
							XRSystemUniversal.EndLateLatching(baseCamera, cameraData.xrUniversal);
						}
						if (flag4)
						{
							for (int j = 0; j < list.Count; j++)
							{
								Camera camera2 = list[j];
								if (camera2.isActiveAndEnabled)
								{
									UniversalAdditionalCameraData universalAdditionalCameraData3;
									camera2.TryGetComponent<UniversalAdditionalCameraData>(out universalAdditionalCameraData3);
									if (universalAdditionalCameraData3 != null)
									{
										CameraData cameraData2 = cameraData;
										cameraData2.camera = camera2;
										cameraData2.baseCamera = baseCamera;
										UniversalRenderPipeline.UpdateCameraStereoMatrices(universalAdditionalCameraData3.camera, item);
										using (new ProfilingScope(null, UniversalRenderPipeline.Profiling.Pipeline.beginCameraRendering))
										{
											RenderPipeline.BeginCameraRendering(context, camera2);
										}
										VFXManager.PrepareCamera(camera2);
										UniversalRenderPipeline.UpdateVolumeFramework(camera2, universalAdditionalCameraData3);
										bool flag9 = j == num2;
										UniversalRenderPipeline.InitializeAdditionalCameraData(camera2, universalAdditionalCameraData3, flag9, ref cameraData2);
										cameraData2.stackAnyPostProcessingEnabled = flag;
										cameraData2.stackLastCameraOutputToHDR = flag8;
										xrlayout.ReconfigurePass(cameraData2.xr, camera2);
										UniversalRenderPipeline.RenderSingleCamera(context, ref cameraData2);
										using (new ProfilingScope(null, UniversalRenderPipeline.Profiling.Pipeline.endCameraRendering))
										{
											RenderPipeline.EndCameraRendering(context, camera2);
										}
									}
								}
							}
						}
						if (cameraData.xr.enabled)
						{
							cameraData.cameraTargetDescriptor = cameraTargetDescriptor;
						}
					}
					if (flag5)
					{
						CommandBuffer commandBuffer = CommandBufferPool.Get();
						XRSystem.RenderMirrorView(commandBuffer, baseCamera);
						context.ExecuteCommandBuffer(commandBuffer);
						context.Submit();
						CommandBufferPool.Release(commandBuffer);
					}
					XRSystem.EndLayout();
				}
			}
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x000319C0 File Offset: 0x0002FBC0
		private static void UpdateCameraData(ref CameraData baseCameraData, in XRPass xr)
		{
			Rect rect = baseCameraData.camera.rect;
			Rect viewport = xr.GetViewport(0);
			baseCameraData.pixelRect = new Rect(rect.x * viewport.width + viewport.x, rect.y * viewport.height + viewport.y, rect.width * viewport.width, rect.height * viewport.height);
			Rect pixelRect = baseCameraData.pixelRect;
			baseCameraData.pixelWidth = (int)Math.Round((double)(pixelRect.width + pixelRect.x)) - (int)Math.Round((double)pixelRect.x);
			baseCameraData.pixelHeight = (int)Math.Round((double)(pixelRect.height + pixelRect.y)) - (int)Math.Round((double)pixelRect.y);
			baseCameraData.aspectRatio = (float)baseCameraData.pixelWidth / (float)baseCameraData.pixelHeight;
			RenderTextureDescriptor cameraTargetDescriptor = baseCameraData.cameraTargetDescriptor;
			baseCameraData.cameraTargetDescriptor = xr.renderTargetDesc;
			if (baseCameraData.isHdrEnabled)
			{
				baseCameraData.cameraTargetDescriptor.graphicsFormat = cameraTargetDescriptor.graphicsFormat;
			}
			baseCameraData.cameraTargetDescriptor.msaaSamples = cameraTargetDescriptor.msaaSamples;
			if (baseCameraData.isDefaultViewport)
			{
				baseCameraData.cameraTargetDescriptor.useDynamicScale = true;
				return;
			}
			baseCameraData.cameraTargetDescriptor.width = baseCameraData.pixelWidth;
			baseCameraData.cameraTargetDescriptor.height = baseCameraData.pixelHeight;
			baseCameraData.cameraTargetDescriptor.useDynamicScale = false;
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x00031B30 File Offset: 0x0002FD30
		private static void UpdateVolumeFramework(Camera camera, UniversalAdditionalCameraData additionalCameraData)
		{
			using (new ProfilingScope(null, ProfilingSampler.Get<URPProfileId>(URPProfileId.UpdateVolumeFramework)))
			{
				if (!((camera.cameraType == CameraType.SceneView) | (additionalCameraData != null && additionalCameraData.requiresVolumeFrameworkUpdate)) && additionalCameraData)
				{
					if (additionalCameraData.volumeStack == null)
					{
						camera.UpdateVolumeStack(additionalCameraData);
					}
					VolumeManager.instance.stack = additionalCameraData.volumeStack;
				}
				else
				{
					if (additionalCameraData && additionalCameraData.volumeStack != null)
					{
						camera.DestroyVolumeStack(additionalCameraData);
					}
					LayerMask layerMask;
					Transform transform;
					camera.GetVolumeLayerMaskAndTrigger(additionalCameraData, out layerMask, out transform);
					VolumeManager.instance.ResetMainStack();
					VolumeManager.instance.Update(transform, layerMask);
				}
			}
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x00031BEC File Offset: 0x0002FDEC
		private static bool CheckPostProcessForDepth(ref CameraData cameraData)
		{
			return cameraData.postProcessEnabled && (((cameraData.antialiasing == AntialiasingMode.SubpixelMorphologicalAntiAliasing || cameraData.IsTemporalAAEnabled()) && cameraData.renderType == CameraRenderType.Base) || UniversalRenderPipeline.CheckPostProcessForDepth());
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x00031C18 File Offset: 0x0002FE18
		private static bool CheckPostProcessForDepth()
		{
			VolumeStack stack = VolumeManager.instance.stack;
			return stack.GetComponent<DepthOfField>().IsActive() || stack.GetComponent<MotionBlur>().IsActive();
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x00031C4F File Offset: 0x0002FE4F
		private static void SetSupportedRenderingFeatures(UniversalRenderPipelineAsset pipelineAsset)
		{
			SupportedRenderingFeatures.active.supportsHDR = pipelineAsset.supportsHDR;
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x00031C64 File Offset: 0x0002FE64
		private static void InitializeCameraData(Camera camera, UniversalAdditionalCameraData additionalCameraData, bool resolveFinalTarget, out CameraData cameraData)
		{
			using (new ProfilingScope(null, UniversalRenderPipeline.Profiling.Pipeline.initializeCameraData))
			{
				cameraData = default(CameraData);
				UniversalRenderPipeline.InitializeStackedCameraData(camera, additionalCameraData, ref cameraData);
				cameraData.camera = camera;
				ScriptableRenderer scriptableRenderer = ((additionalCameraData != null) ? additionalCameraData.scriptableRenderer : null);
				bool flag = scriptableRenderer != null && scriptableRenderer.supportedRenderingFeatures.msaa;
				int num = 1;
				if (camera.allowMSAA && UniversalRenderPipeline.asset.msaaSampleCount > 1 && flag)
				{
					num = ((camera.targetTexture != null) ? camera.targetTexture.antiAliasing : UniversalRenderPipeline.asset.msaaSampleCount);
				}
				if (cameraData.xrRendering && flag && camera.targetTexture == null)
				{
					num = (int)XRSystem.GetDisplayMSAASamples();
				}
				bool preserveFramebufferAlpha = Graphics.preserveFramebufferAlpha;
				cameraData.hdrColorBufferPrecision = (UniversalRenderPipeline.asset ? UniversalRenderPipeline.asset.hdrColorBufferPrecision : HDRColorBufferPrecision._32Bits);
				cameraData.cameraTargetDescriptor = UniversalRenderPipeline.CreateRenderTextureDescriptor(camera, ref cameraData, cameraData.isHdrEnabled, cameraData.hdrColorBufferPrecision, num, preserveFramebufferAlpha, cameraData.requiresOpaqueTexture);
			}
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x00031D80 File Offset: 0x0002FF80
		private static void InitializeStackedCameraData(Camera baseCamera, UniversalAdditionalCameraData baseAdditionalCameraData, ref CameraData cameraData)
		{
			using (new ProfilingScope(null, UniversalRenderPipeline.Profiling.Pipeline.initializeStackedCameraData))
			{
				UniversalRenderPipelineAsset asset = UniversalRenderPipeline.asset;
				cameraData.targetTexture = baseCamera.targetTexture;
				cameraData.cameraType = baseCamera.cameraType;
				if (cameraData.isSceneViewCamera)
				{
					cameraData.volumeLayerMask = 1;
					cameraData.volumeTrigger = null;
					cameraData.isStopNaNEnabled = false;
					cameraData.isDitheringEnabled = false;
					cameraData.antialiasing = AntialiasingMode.None;
					cameraData.antialiasingQuality = AntialiasingQuality.High;
					cameraData.xrRendering = false;
					cameraData.allowHDROutput = false;
				}
				else if (baseAdditionalCameraData != null)
				{
					cameraData.volumeLayerMask = baseAdditionalCameraData.volumeLayerMask;
					cameraData.volumeTrigger = ((baseAdditionalCameraData.volumeTrigger == null) ? baseCamera.transform : baseAdditionalCameraData.volumeTrigger);
					cameraData.isStopNaNEnabled = baseAdditionalCameraData.stopNaN && SystemInfo.graphicsShaderLevel >= 35;
					cameraData.isDitheringEnabled = baseAdditionalCameraData.dithering;
					cameraData.antialiasing = baseAdditionalCameraData.antialiasing;
					cameraData.antialiasingQuality = baseAdditionalCameraData.antialiasingQuality;
					cameraData.xrRendering = baseAdditionalCameraData.allowXRRendering && XRSystem.displayActive;
					cameraData.allowHDROutput = baseAdditionalCameraData.allowHDROutput;
				}
				else
				{
					cameraData.volumeLayerMask = 1;
					cameraData.volumeTrigger = null;
					cameraData.isStopNaNEnabled = false;
					cameraData.isDitheringEnabled = false;
					cameraData.antialiasing = AntialiasingMode.None;
					cameraData.antialiasingQuality = AntialiasingQuality.High;
					cameraData.xrRendering = XRSystem.displayActive;
					cameraData.allowHDROutput = true;
				}
				cameraData.isHdrEnabled = baseCamera.allowHDR && asset.supportsHDR;
				cameraData.allowHDROutput &= asset.supportsHDR;
				Rect rect = baseCamera.rect;
				cameraData.pixelRect = baseCamera.pixelRect;
				cameraData.pixelWidth = baseCamera.pixelWidth;
				cameraData.pixelHeight = baseCamera.pixelHeight;
				cameraData.aspectRatio = (float)cameraData.pixelWidth / (float)cameraData.pixelHeight;
				cameraData.isDefaultViewport = Math.Abs(rect.x) <= 0f && Math.Abs(rect.y) <= 0f && Math.Abs(rect.width) >= 1f && Math.Abs(rect.height) >= 1f;
				bool flag = cameraData.cameraType == CameraType.SceneView || cameraData.cameraType == CameraType.Preview || cameraData.cameraType == CameraType.Reflection;
				cameraData.renderScale = ((Mathf.Abs(1f - asset.renderScale) < 0.05f || flag) ? 1f : asset.renderScale);
				cameraData.upscalingFilter = UniversalRenderPipeline.ResolveUpscalingFilterSelection(new Vector2((float)cameraData.pixelWidth, (float)cameraData.pixelHeight), cameraData.renderScale, asset.upscalingFilter);
				if (cameraData.renderScale > 1f)
				{
					cameraData.imageScalingMode = ImageScalingMode.Downscaling;
				}
				else if (cameraData.renderScale < 1f || (!flag && cameraData.upscalingFilter == ImageUpscalingFilter.FSR))
				{
					cameraData.imageScalingMode = ImageScalingMode.Upscaling;
				}
				else
				{
					cameraData.imageScalingMode = ImageScalingMode.None;
				}
				cameraData.fsrOverrideSharpness = asset.fsrOverrideSharpness;
				cameraData.fsrSharpness = asset.fsrSharpness;
				cameraData.xr = XRSystem.emptyPass;
				XRSystem.SetRenderScale(cameraData.renderScale);
				SortingCriteria sortingCriteria = SortingCriteria.CommonOpaque;
				SortingCriteria sortingCriteria2 = SortingCriteria.SortingLayer | SortingCriteria.RenderQueue | SortingCriteria.OptimizeStateChanges | SortingCriteria.CanvasOrder;
				bool hasHiddenSurfaceRemovalOnGPU = SystemInfo.hasHiddenSurfaceRemovalOnGPU;
				cameraData.defaultOpaqueSortFlags = (((baseCamera.opaqueSortMode == OpaqueSortMode.Default && hasHiddenSurfaceRemovalOnGPU) || baseCamera.opaqueSortMode == OpaqueSortMode.NoDistanceSort) ? sortingCriteria2 : sortingCriteria);
				cameraData.captureActions = CameraCaptureBridge.GetCaptureActions(baseCamera);
			}
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x00032100 File Offset: 0x00030300
		private static void InitializeAdditionalCameraData(Camera camera, UniversalAdditionalCameraData additionalCameraData, bool resolveFinalTarget, ref CameraData cameraData)
		{
			using (new ProfilingScope(null, UniversalRenderPipeline.Profiling.Pipeline.initializeAdditionalCameraData))
			{
				UniversalRenderPipelineAsset asset = UniversalRenderPipeline.asset;
				bool flag = asset.supportsMainLightShadows || asset.supportsAdditionalLightShadows;
				cameraData.maxShadowDistance = Mathf.Min(asset.shadowDistance, camera.farClipPlane);
				cameraData.maxShadowDistance = ((flag && cameraData.maxShadowDistance >= camera.nearClipPlane) ? cameraData.maxShadowDistance : 0f);
				bool isSceneViewCamera = cameraData.isSceneViewCamera;
				if (isSceneViewCamera)
				{
					cameraData.renderType = CameraRenderType.Base;
					cameraData.clearDepth = true;
					cameraData.postProcessEnabled = CoreUtils.ArePostProcessesEnabled(camera);
					cameraData.requiresDepthTexture = asset.supportsCameraDepthTexture;
					cameraData.requiresOpaqueTexture = asset.supportsCameraOpaqueTexture;
					cameraData.renderer = UniversalRenderPipeline.asset.scriptableRenderer;
					cameraData.useScreenCoordOverride = false;
					cameraData.screenSizeOverride = cameraData.pixelRect.size;
					cameraData.screenCoordScaleBias = Vector2.one;
				}
				else if (additionalCameraData != null)
				{
					cameraData.renderType = additionalCameraData.renderType;
					cameraData.clearDepth = additionalCameraData.renderType == CameraRenderType.Base || additionalCameraData.clearDepth;
					cameraData.postProcessEnabled = additionalCameraData.renderPostProcessing;
					cameraData.maxShadowDistance = (additionalCameraData.renderShadows ? cameraData.maxShadowDistance : 0f);
					cameraData.requiresDepthTexture = additionalCameraData.requiresDepthTexture;
					cameraData.requiresOpaqueTexture = additionalCameraData.requiresColorTexture;
					cameraData.renderer = additionalCameraData.scriptableRenderer;
					cameraData.useScreenCoordOverride = additionalCameraData.useScreenCoordOverride;
					cameraData.screenSizeOverride = additionalCameraData.screenSizeOverride;
					cameraData.screenCoordScaleBias = additionalCameraData.screenCoordScaleBias;
				}
				else
				{
					cameraData.renderType = CameraRenderType.Base;
					cameraData.clearDepth = true;
					cameraData.postProcessEnabled = false;
					cameraData.requiresDepthTexture = asset.supportsCameraDepthTexture;
					cameraData.requiresOpaqueTexture = asset.supportsCameraOpaqueTexture;
					cameraData.renderer = UniversalRenderPipeline.asset.scriptableRenderer;
					cameraData.useScreenCoordOverride = false;
					cameraData.screenSizeOverride = cameraData.pixelRect.size;
					cameraData.screenCoordScaleBias = Vector2.one;
				}
				cameraData.postProcessEnabled &= SystemInfo.graphicsDeviceType != GraphicsDeviceType.OpenGLES2;
				cameraData.requiresDepthTexture = cameraData.requiresDepthTexture || isSceneViewCamera;
				cameraData.postProcessingRequiresDepthTexture = UniversalRenderPipeline.CheckPostProcessForDepth(ref cameraData);
				cameraData.resolveFinalTarget = resolveFinalTarget;
				bool flag2 = cameraData.renderType == CameraRenderType.Overlay;
				if (flag2)
				{
					cameraData.requiresOpaqueTexture = false;
				}
				if (additionalCameraData != null)
				{
					UniversalRenderPipeline.UpdateTemporalAAData(ref cameraData, additionalCameraData);
				}
				Matrix4x4 projectionMatrix = camera.projectionMatrix;
				if (flag2 && !camera.orthographic && cameraData.pixelRect != camera.pixelRect)
				{
					float num = camera.projectionMatrix.m00 * camera.aspect / cameraData.aspectRatio;
					projectionMatrix.m00 = num;
				}
				UniversalRenderPipeline.ApplyTaaRenderingDebugOverrides(ref cameraData.taaSettings);
				Matrix4x4 matrix4x = TemporalAA.CalculateJitterMatrix(ref cameraData);
				cameraData.SetViewProjectionAndJitterMatrix(camera.worldToCameraMatrix, projectionMatrix, matrix4x);
				cameraData.worldSpaceCameraPos = camera.transform.position;
				Color backgroundColor = camera.backgroundColor;
				cameraData.backgroundColor = CoreUtils.ConvertSRGBToActiveColorSpace(backgroundColor);
				cameraData.stackAnyPostProcessingEnabled = cameraData.postProcessEnabled;
				cameraData.stackLastCameraOutputToHDR = cameraData.isHDROutputActive;
			}
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x00032424 File Offset: 0x00030624
		private static void InitializeRenderingData(UniversalRenderPipelineAsset settings, ref CameraData cameraData, ref CullingResults cullResults, CommandBuffer cmd, out RenderingData renderingData)
		{
			using (new ProfilingScope(null, UniversalRenderPipeline.Profiling.Pipeline.initializeRenderingData))
			{
				UniversalRenderer universalRenderer = cameraData.renderer as UniversalRenderer;
				bool flag = universalRenderer != null && universalRenderer.renderingModeActual == RenderingMode.ForwardPlus;
				NativeArray<VisibleLight> visibleLights = cullResults.visibleLights;
				int mainLightIndex = UniversalRenderPipeline.GetMainLightIndex(settings, visibleLights);
				bool flag2 = false;
				bool flag3 = false;
				if (cameraData.maxShadowDistance > 0f)
				{
					flag2 = mainLightIndex != -1 && visibleLights[mainLightIndex].light != null && visibleLights[mainLightIndex].light.shadows > LightShadows.None;
					if (settings.supportsAdditionalLightShadows && (settings.additionalLightsRenderingMode == LightRenderingMode.PerPixel || flag))
					{
						for (int i = 0; i < visibleLights.Length; i++)
						{
							if (i != mainLightIndex)
							{
								ref VisibleLight ptr = ref visibleLights.UnsafeElementAtMutable<VisibleLight>(i);
								Light light = ptr.light;
								if ((ptr.lightType == LightType.Spot || ptr.lightType == LightType.Point) && light != null && light.shadows != LightShadows.None)
								{
									flag3 = true;
									break;
								}
							}
						}
					}
				}
				renderingData.cullResults = cullResults;
				renderingData.cameraData = cameraData;
				UniversalRenderPipeline.InitializeLightData(settings, visibleLights, mainLightIndex, out renderingData.lightData);
				UniversalRenderPipeline.InitializeShadowData(settings, visibleLights, flag2, flag3 && !renderingData.lightData.shadeAdditionalLightsPerVertex, flag, out renderingData.shadowData);
				UniversalRenderPipeline.InitializePostProcessingData(settings, cameraData.stackLastCameraOutputToHDR, out renderingData.postProcessingData);
				renderingData.supportsDynamicBatching = settings.supportsDynamicBatching;
				renderingData.perObjectData = UniversalRenderPipeline.GetPerObjectLightFlags(renderingData.lightData.additionalLightsCount, flag);
				renderingData.postProcessingEnabled = cameraData.stackAnyPostProcessingEnabled;
				renderingData.commandBuffer = cmd;
				UniversalRenderPipeline.CheckAndApplyDebugSettings(ref renderingData);
			}
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x000325FC File Offset: 0x000307FC
		private static void InitializeShadowData(UniversalRenderPipelineAsset settings, NativeArray<VisibleLight> visibleLights, bool mainLightCastShadows, bool additionalLightsCastShadows, bool isForwardPlus, out ShadowData shadowData)
		{
			using (new ProfilingScope(null, UniversalRenderPipeline.Profiling.Pipeline.initializeShadowData))
			{
				UniversalRenderPipeline.m_ShadowBiasData.Clear();
				UniversalRenderPipeline.m_ShadowResolutionData.Clear();
				for (int i = 0; i < visibleLights.Length; i++)
				{
					Light light = visibleLights.UnsafeElementAtMutable<VisibleLight>(i).light;
					UniversalAdditionalLightData universalAdditionalLightData = null;
					if (light != null)
					{
						light.gameObject.TryGetComponent<UniversalAdditionalLightData>(out universalAdditionalLightData);
					}
					if (universalAdditionalLightData && !universalAdditionalLightData.usePipelineSettings)
					{
						UniversalRenderPipeline.m_ShadowBiasData.Add(new Vector4(light.shadowBias, light.shadowNormalBias, 0f, 0f));
					}
					else
					{
						UniversalRenderPipeline.m_ShadowBiasData.Add(new Vector4(settings.shadowDepthBias, settings.shadowNormalBias, 0f, 0f));
					}
					if (universalAdditionalLightData && universalAdditionalLightData.additionalLightsShadowResolutionTier == UniversalAdditionalLightData.AdditionalLightsShadowResolutionTierCustom)
					{
						UniversalRenderPipeline.m_ShadowResolutionData.Add((int)light.shadowResolution);
					}
					else if (universalAdditionalLightData && universalAdditionalLightData.additionalLightsShadowResolutionTier != UniversalAdditionalLightData.AdditionalLightsShadowResolutionTierCustom)
					{
						int num = Mathf.Clamp(universalAdditionalLightData.additionalLightsShadowResolutionTier, UniversalAdditionalLightData.AdditionalLightsShadowResolutionTierLow, UniversalAdditionalLightData.AdditionalLightsShadowResolutionTierHigh);
						UniversalRenderPipeline.m_ShadowResolutionData.Add(settings.GetAdditionalLightsShadowResolution(num));
					}
					else
					{
						UniversalRenderPipeline.m_ShadowResolutionData.Add(settings.GetAdditionalLightsShadowResolution(UniversalAdditionalLightData.AdditionalLightsShadowDefaultResolutionTier));
					}
				}
				shadowData.bias = UniversalRenderPipeline.m_ShadowBiasData;
				shadowData.resolution = UniversalRenderPipeline.m_ShadowResolutionData;
				shadowData.mainLightShadowsEnabled = settings.supportsMainLightShadows && settings.mainLightRenderingMode == LightRenderingMode.PerPixel;
				shadowData.supportsMainLightShadows = SystemInfo.supportsShadows && shadowData.mainLightShadowsEnabled && mainLightCastShadows;
				shadowData.requiresScreenSpaceShadowResolve = false;
				shadowData.mainLightShadowCascadesCount = ((SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES2) ? 1 : settings.shadowCascadeCount);
				shadowData.mainLightShadowmapWidth = settings.mainLightShadowmapResolution;
				shadowData.mainLightShadowmapHeight = settings.mainLightShadowmapResolution;
				switch (shadowData.mainLightShadowCascadesCount)
				{
				case 1:
					shadowData.mainLightShadowCascadesSplit = new Vector3(1f, 0f, 0f);
					break;
				case 2:
					shadowData.mainLightShadowCascadesSplit = new Vector3(settings.cascade2Split, 1f, 0f);
					break;
				case 3:
					shadowData.mainLightShadowCascadesSplit = new Vector3(settings.cascade3Split.x, settings.cascade3Split.y, 0f);
					break;
				default:
					shadowData.mainLightShadowCascadesSplit = settings.cascade4Split;
					break;
				}
				shadowData.mainLightShadowCascadeBorder = settings.cascadeBorder;
				shadowData.additionalLightShadowsEnabled = settings.supportsAdditionalLightShadows && (settings.additionalLightsRenderingMode == LightRenderingMode.PerPixel || isForwardPlus);
				shadowData.supportsAdditionalLightShadows = SystemInfo.supportsShadows && shadowData.additionalLightShadowsEnabled && additionalLightsCastShadows;
				shadowData.additionalLightsShadowmapWidth = (shadowData.additionalLightsShadowmapHeight = settings.additionalLightsShadowmapResolution);
				shadowData.supportsSoftShadows = settings.supportsSoftShadows && (shadowData.supportsMainLightShadows || shadowData.supportsAdditionalLightShadows);
				shadowData.shadowmapDepthBufferBits = 16;
				shadowData.isKeywordAdditionalLightShadowsEnabled = false;
				shadowData.isKeywordSoftShadowsEnabled = false;
			}
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x00032920 File Offset: 0x00030B20
		private static void InitializePostProcessingData(UniversalRenderPipelineAsset settings, bool isHDROutputActive, out PostProcessingData postProcessingData)
		{
			postProcessingData.gradingMode = (settings.supportsHDR ? settings.colorGradingMode : ColorGradingMode.LowDynamicRange);
			if (isHDROutputActive)
			{
				postProcessingData.gradingMode = ColorGradingMode.HighDynamicRange;
			}
			postProcessingData.lutSize = settings.colorGradingLutSize;
			postProcessingData.useFastSRGBLinearConversion = settings.useFastSRGBLinearConversion;
			postProcessingData.supportDataDrivenLensFlare = settings.supportDataDrivenLensFlare;
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x00032974 File Offset: 0x00030B74
		private static void InitializeLightData(UniversalRenderPipelineAsset settings, NativeArray<VisibleLight> visibleLights, int mainLightIndex, out LightData lightData)
		{
			using (new ProfilingScope(null, UniversalRenderPipeline.Profiling.Pipeline.initializeLightData))
			{
				int maxPerObjectLights = UniversalRenderPipeline.maxPerObjectLights;
				int maxVisibleAdditionalLights = UniversalRenderPipeline.maxVisibleAdditionalLights;
				lightData.mainLightIndex = mainLightIndex;
				if (settings.additionalLightsRenderingMode != LightRenderingMode.Disabled)
				{
					lightData.additionalLightsCount = Math.Min((mainLightIndex != -1) ? (visibleLights.Length - 1) : visibleLights.Length, maxVisibleAdditionalLights);
					lightData.maxPerObjectAdditionalLightsCount = Math.Min(settings.maxAdditionalLightsCount, maxPerObjectLights);
				}
				else
				{
					lightData.additionalLightsCount = 0;
					lightData.maxPerObjectAdditionalLightsCount = 0;
				}
				lightData.supportsAdditionalLights = settings.additionalLightsRenderingMode > LightRenderingMode.Disabled;
				lightData.shadeAdditionalLightsPerVertex = settings.additionalLightsRenderingMode == LightRenderingMode.PerVertex;
				lightData.visibleLights = visibleLights;
				lightData.supportsMixedLighting = settings.supportsMixedLighting;
				lightData.reflectionProbeBlending = settings.reflectionProbeBlending;
				lightData.reflectionProbeBoxProjection = settings.reflectionProbeBoxProjection;
				lightData.supportsLightLayers = RenderingUtils.SupportsLightLayers(SystemInfo.graphicsDeviceType) && settings.useRenderingLayers;
			}
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x00032A74 File Offset: 0x00030C74
		private static void ApplyTaaRenderingDebugOverrides(ref TemporalAA.Settings taaSettings)
		{
			switch (DebugDisplaySettings<UniversalRenderPipelineDebugDisplaySettings>.Instance.renderingSettings.taaDebugMode)
			{
			case DebugDisplaySettingsRendering.TaaDebugMode.ShowRawFrame:
				taaSettings.m_FrameInfluence = 1f;
				return;
			case DebugDisplaySettingsRendering.TaaDebugMode.ShowRawFrameNoJitter:
				taaSettings.m_FrameInfluence = 1f;
				taaSettings.jitterScale = 0f;
				return;
			case DebugDisplaySettingsRendering.TaaDebugMode.ShowClampedHistory:
				taaSettings.m_FrameInfluence = 0f;
				return;
			default:
				return;
			}
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x00032AD4 File Offset: 0x00030CD4
		private static void UpdateTemporalAAData(ref CameraData cameraData, UniversalAdditionalCameraData additionalCameraData)
		{
			ref RenderTextureDescriptor ptr = ref cameraData.cameraTargetDescriptor;
			cameraData.taaPersistentData = additionalCameraData.taaPersistentData;
			cameraData.taaPersistentData.Init(ptr.width, ptr.height, ptr.volumeDepth, ptr.graphicsFormat, ptr.vrUsage, ptr.dimension);
			ref TemporalAA.Settings taaSettings = ref additionalCameraData.taaSettings;
			cameraData.taaSettings = taaSettings;
			taaSettings.resetHistoryFrames -= ((taaSettings.resetHistoryFrames > 0) ? 1 : 0);
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x00032B50 File Offset: 0x00030D50
		private static void UpdateTemporalAATargets(ref CameraData cameraData)
		{
			if (cameraData.IsTemporalAAEnabled())
			{
				bool flag = cameraData.xr.enabled && !cameraData.xr.singlePassEnabled;
				if (cameraData.taaPersistentData.AllocateTargets(flag))
				{
					cameraData.taaSettings.resetHistoryFrames = cameraData.taaSettings.resetHistoryFrames + (flag ? 2 : 1);
					return;
				}
			}
			else
			{
				cameraData.taaPersistentData.DeallocateTargets();
			}
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x00032BB8 File Offset: 0x00030DB8
		private static void UpdateCameraStereoMatrices(Camera camera, XRPass xr)
		{
			if (xr.enabled)
			{
				if (xr.singlePassEnabled)
				{
					for (int i = 0; i < Mathf.Min(2, xr.viewCount); i++)
					{
						camera.SetStereoProjectionMatrix((Camera.StereoscopicEye)i, xr.GetProjMatrix(i));
						camera.SetStereoViewMatrix((Camera.StereoscopicEye)i, xr.GetViewMatrix(i));
					}
					return;
				}
				camera.SetStereoProjectionMatrix((Camera.StereoscopicEye)xr.multipassId, xr.GetProjMatrix(0));
				camera.SetStereoViewMatrix((Camera.StereoscopicEye)xr.multipassId, xr.GetViewMatrix(0));
			}
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x00032C30 File Offset: 0x00030E30
		private static PerObjectData GetPerObjectLightFlags(int additionalLightsCount, bool isForwardPlus)
		{
			PerObjectData perObjectData2;
			using (new ProfilingScope(null, UniversalRenderPipeline.Profiling.Pipeline.getPerObjectLightFlags))
			{
				PerObjectData perObjectData = PerObjectData.LightProbe | PerObjectData.Lightmaps | PerObjectData.OcclusionProbe | PerObjectData.ShadowMask;
				if (!isForwardPlus)
				{
					perObjectData |= PerObjectData.ReflectionProbes | PerObjectData.LightData;
				}
				if (additionalLightsCount > 0 && !isForwardPlus && !RenderingUtils.useStructuredBuffer)
				{
					perObjectData |= PerObjectData.LightIndices;
				}
				perObjectData2 = perObjectData;
			}
			return perObjectData2;
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x00032C90 File Offset: 0x00030E90
		private static int GetMainLightIndex(UniversalRenderPipelineAsset settings, NativeArray<VisibleLight> visibleLights)
		{
			int num;
			using (new ProfilingScope(null, UniversalRenderPipeline.Profiling.Pipeline.getMainLightIndex))
			{
				int length = visibleLights.Length;
				if (length == 0 || settings.mainLightRenderingMode != LightRenderingMode.PerPixel)
				{
					num = -1;
				}
				else
				{
					Light sun = RenderSettings.sun;
					int num2 = -1;
					float num3 = 0f;
					for (int i = 0; i < length; i++)
					{
						ref VisibleLight ptr = ref visibleLights.UnsafeElementAtMutable<VisibleLight>(i);
						Light light = ptr.light;
						if (light == null)
						{
							break;
						}
						if (ptr.lightType == LightType.Directional)
						{
							if (light == sun)
							{
								return i;
							}
							if (light.intensity > num3)
							{
								num3 = light.intensity;
								num2 = i;
							}
						}
					}
					num = num2;
				}
			}
			return num;
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x00032D58 File Offset: 0x00030F58
		private static void SetupPerFrameShaderConstants()
		{
			using (new ProfilingScope(null, UniversalRenderPipeline.Profiling.Pipeline.setupPerFrameShaderConstants))
			{
				Shader.SetGlobalColor(ShaderPropertyId.rendererColor, Color.white);
				if (UniversalRenderPipeline.asset.lodCrossFadeDitheringType == LODCrossFadeDitheringType.BayerMatrix)
				{
					Shader.SetGlobalFloat(ShaderPropertyId.ditheringTextureInvSize, 1f / (float)UniversalRenderPipeline.asset.textures.bayerMatrixTex.width);
					Shader.SetGlobalTexture(ShaderPropertyId.ditheringTexture, UniversalRenderPipeline.asset.textures.bayerMatrixTex);
				}
				else if (UniversalRenderPipeline.asset.lodCrossFadeDitheringType == LODCrossFadeDitheringType.BlueNoise)
				{
					Shader.SetGlobalFloat(ShaderPropertyId.ditheringTextureInvSize, 1f / (float)UniversalRenderPipeline.asset.textures.blueNoise64LTex.width);
					Shader.SetGlobalTexture(ShaderPropertyId.ditheringTexture, UniversalRenderPipeline.asset.textures.blueNoise64LTex);
				}
			}
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x00032E38 File Offset: 0x00031038
		private static void SetupPerCameraShaderConstants(CommandBuffer cmd)
		{
			using (new ProfilingScope(null, UniversalRenderPipeline.Profiling.Pipeline.setupPerCameraShaderConstants))
			{
				SphericalHarmonicsL2 ambientProbe = RenderSettings.ambientProbe;
				Color color = CoreUtils.ConvertLinearToActiveColorSpace(new Color(ambientProbe[0, 0], ambientProbe[1, 0], ambientProbe[2, 0]) * RenderSettings.reflectionIntensity);
				cmd.SetGlobalVector(ShaderPropertyId.glossyEnvironmentColor, color);
				cmd.SetGlobalTexture(ShaderPropertyId.glossyEnvironmentCubeMap, ReflectionProbe.defaultTexture);
				cmd.SetGlobalVector(ShaderPropertyId.glossyEnvironmentCubeMapHDR, ReflectionProbe.defaultTextureHDRDecodeValues);
				cmd.SetGlobalVector(ShaderPropertyId.ambientSkyColor, CoreUtils.ConvertSRGBToActiveColorSpace(RenderSettings.ambientSkyColor));
				cmd.SetGlobalVector(ShaderPropertyId.ambientEquatorColor, CoreUtils.ConvertSRGBToActiveColorSpace(RenderSettings.ambientEquatorColor));
				cmd.SetGlobalVector(ShaderPropertyId.ambientGroundColor, CoreUtils.ConvertSRGBToActiveColorSpace(RenderSettings.ambientGroundColor));
				cmd.SetGlobalVector(ShaderPropertyId.subtractiveShadowColor, CoreUtils.ConvertSRGBToActiveColorSpace(RenderSettings.subtractiveShadowColor));
			}
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x00032F48 File Offset: 0x00031148
		private static void CheckAndApplyDebugSettings(ref RenderingData renderingData)
		{
			UniversalRenderPipelineDebugDisplaySettings instance = DebugDisplaySettings<UniversalRenderPipelineDebugDisplaySettings>.Instance;
			ref CameraData ptr = ref renderingData.cameraData;
			if (instance.AreAnySettingsActive && !ptr.isPreviewCamera)
			{
				DebugDisplaySettingsRendering renderingSettings = instance.renderingSettings;
				int num = ptr.cameraTargetDescriptor.msaaSamples;
				if (!renderingSettings.enableMsaa)
				{
					num = 1;
				}
				if (!renderingSettings.enableHDR)
				{
					ptr.isHdrEnabled = false;
				}
				if (!instance.IsPostProcessingAllowed)
				{
					ptr.postProcessEnabled = false;
				}
				ptr.hdrColorBufferPrecision = (UniversalRenderPipeline.asset ? UniversalRenderPipeline.asset.hdrColorBufferPrecision : HDRColorBufferPrecision._32Bits);
				ptr.cameraTargetDescriptor.graphicsFormat = UniversalRenderPipeline.MakeRenderTextureGraphicsFormat(ptr.isHdrEnabled, ptr.hdrColorBufferPrecision, true);
				ptr.cameraTargetDescriptor.msaaSamples = num;
			}
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x00032FFC File Offset: 0x000311FC
		private static ImageUpscalingFilter ResolveUpscalingFilterSelection(Vector2 imageSize, float renderScale, UpscalingFilterSelection selection)
		{
			ImageUpscalingFilter imageUpscalingFilter = ImageUpscalingFilter.Linear;
			if (selection == UpscalingFilterSelection.FSR && !FSRUtils.IsSupported())
			{
				selection = UpscalingFilterSelection.Auto;
			}
			switch (selection)
			{
			case UpscalingFilterSelection.Auto:
			{
				float num = 1f / renderScale;
				if (Mathf.Approximately(num - Mathf.Floor(num), 0f))
				{
					float num2 = imageSize.x / num;
					float num3 = imageSize.y / num;
					if (Mathf.Approximately(num2 - Mathf.Floor(num2), 0f) && Mathf.Approximately(num3 - Mathf.Floor(num3), 0f))
					{
						imageUpscalingFilter = ImageUpscalingFilter.Point;
					}
				}
				break;
			}
			case UpscalingFilterSelection.Point:
				imageUpscalingFilter = ImageUpscalingFilter.Point;
				break;
			case UpscalingFilterSelection.FSR:
				imageUpscalingFilter = ImageUpscalingFilter.FSR;
				break;
			}
			return imageUpscalingFilter;
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x00033094 File Offset: 0x00031294
		internal static bool HDROutputForMainDisplayIsActive()
		{
			bool flag = SystemInfo.hdrDisplaySupportFlags.HasFlag(HDRDisplaySupportFlags.Supported) && UniversalRenderPipeline.asset.supportsHDR;
			bool flag2 = HDROutputSettings.main.available && HDROutputSettings.main.active;
			return !Application.isMobilePlatform && flag && flag2;
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x000330EC File Offset: 0x000312EC
		internal static bool HDROutputForAnyDisplayIsActive()
		{
			bool flag = UniversalRenderPipeline.HDROutputForMainDisplayIsActive();
			if (XRSystem.displayActive)
			{
				flag |= XRSystem.isHDRDisplayOutputActive;
			}
			return flag;
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x00033110 File Offset: 0x00031310
		private void SetHDRState(List<Camera> cameras)
		{
			bool flag = HDROutputSettings.main.available && HDROutputSettings.main.active;
			if (SystemInfo.hdrDisplaySupportFlags.HasFlag(HDRDisplaySupportFlags.RuntimeSwitchable) && !UniversalRenderPipeline.asset.supportsHDR && flag)
			{
				HDROutputSettings.main.RequestHDRModeChange(false);
			}
			if (flag)
			{
				HDROutputSettings.main.automaticHDRTonemapping = false;
			}
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x0003317C File Offset: 0x0003137C
		internal static void GetHDROutputLuminanceParameters(HDROutputUtils.HDRDisplayInformation hdrDisplayInformation, ColorGamut hdrDisplayColorGamut, Tonemapping tonemapping, out Vector4 hdrOutputParameters)
		{
			float num = (float)hdrDisplayInformation.minToneMapLuminance;
			float num2 = (float)hdrDisplayInformation.maxToneMapLuminance;
			float num3 = hdrDisplayInformation.paperWhiteNits;
			if (!tonemapping.detectPaperWhite.value)
			{
				num3 = tonemapping.paperWhite.value;
			}
			if (!tonemapping.detectBrightnessLimits.value)
			{
				num = tonemapping.minNits.value;
				num2 = tonemapping.maxNits.value;
			}
			hdrOutputParameters = new Vector4(num, num2, num3, 1f / num3);
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x000331F4 File Offset: 0x000313F4
		internal static void GetHDROutputGradingParameters(Tonemapping tonemapping, out Vector4 hdrOutputParameters)
		{
			int num = 0;
			float num2 = 0f;
			TonemappingMode value = tonemapping.mode.value;
			if (value != TonemappingMode.Neutral)
			{
				if (value == TonemappingMode.ACES)
				{
					num = (int)tonemapping.acesPreset.value;
				}
			}
			else
			{
				num = (int)tonemapping.neutralHDRRangeReductionMode.value;
				num2 = tonemapping.hueShiftAmount.value;
			}
			hdrOutputParameters = new Vector4((float)num, num2, 0f, 0f);
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x0003325D File Offset: 0x0003145D
		public static bool IsGameCamera(Camera camera)
		{
			if (camera == null)
			{
				throw new ArgumentNullException("camera");
			}
			return camera.cameraType == CameraType.Game || camera.cameraType == CameraType.VR;
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x00033287 File Offset: 0x00031487
		[Obsolete("Please use CameraData.xr.enabled instead.", true)]
		public static bool IsStereoEnabled(Camera camera)
		{
			if (camera == null)
			{
				throw new ArgumentNullException("camera");
			}
			return UniversalRenderPipeline.IsGameCamera(camera) && camera.stereoTargetEye == StereoTargetEyeMask.Both;
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x060007EA RID: 2026 RVA: 0x000332B0 File Offset: 0x000314B0
		public static UniversalRenderPipelineAsset asset
		{
			get
			{
				return GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
			}
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x000332BC File Offset: 0x000314BC
		private void SortCameras(List<Camera> cameras)
		{
			if (cameras.Count > 1)
			{
				cameras.Sort(this.cameraComparison);
			}
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x000332D3 File Offset: 0x000314D3
		internal static GraphicsFormat MakeRenderTextureGraphicsFormat(bool isHdrEnabled, HDRColorBufferPrecision requestHDRColorBufferPrecision, bool needsAlpha)
		{
			if (!isHdrEnabled)
			{
				return SystemInfo.GetGraphicsFormat(DefaultFormat.LDR);
			}
			if (!needsAlpha && requestHDRColorBufferPrecision != HDRColorBufferPrecision._64Bits && RenderingUtils.SupportsGraphicsFormat(GraphicsFormat.B10G11R11_UFloatPack32, FormatUsage.Blend))
			{
				return GraphicsFormat.B10G11R11_UFloatPack32;
			}
			if (RenderingUtils.SupportsGraphicsFormat(GraphicsFormat.R16G16B16A16_SFloat, FormatUsage.Blend))
			{
				return GraphicsFormat.R16G16B16A16_SFloat;
			}
			return SystemInfo.GetGraphicsFormat(DefaultFormat.HDR);
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x00033306 File Offset: 0x00031506
		internal static GraphicsFormat MakeUnormRenderTextureGraphicsFormat()
		{
			if (RenderingUtils.SupportsGraphicsFormat(GraphicsFormat.A2B10G10R10_UNormPack32, FormatUsage.Blend))
			{
				return GraphicsFormat.A2B10G10R10_UNormPack32;
			}
			return GraphicsFormat.R8G8B8A8_UNorm;
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x00033318 File Offset: 0x00031518
		internal static RenderTextureDescriptor CreateRenderTextureDescriptor(Camera camera, ref CameraData cameraData, bool isHdrEnabled, HDRColorBufferPrecision requestHDRColorBufferPrecision, int msaaSamples, bool needsAlpha, bool requiresOpaqueTexture)
		{
			RenderTextureDescriptor descriptor;
			if (camera.targetTexture == null)
			{
				descriptor = new RenderTextureDescriptor(cameraData.scaledWidth, cameraData.scaledHeight);
				descriptor.graphicsFormat = UniversalRenderPipeline.MakeRenderTextureGraphicsFormat(isHdrEnabled, requestHDRColorBufferPrecision, needsAlpha);
				descriptor.depthBufferBits = 32;
				descriptor.msaaSamples = msaaSamples;
				descriptor.sRGB = QualitySettings.activeColorSpace == ColorSpace.Linear;
			}
			else
			{
				descriptor = camera.targetTexture.descriptor;
				descriptor.msaaSamples = msaaSamples;
				descriptor.width = cameraData.scaledWidth;
				descriptor.height = cameraData.scaledHeight;
				if (camera.cameraType == CameraType.SceneView && !isHdrEnabled)
				{
					descriptor.graphicsFormat = SystemInfo.GetGraphicsFormat(DefaultFormat.LDR);
				}
			}
			descriptor.enableRandomWrite = false;
			descriptor.bindMS = false;
			descriptor.useDynamicScale = camera.allowDynamicResolution;
			descriptor.msaaSamples = SystemInfo.GetRenderTextureSupportedMSAASampleCount(descriptor);
			if (!SystemInfo.supportsStoreAndResolveAction)
			{
				descriptor.msaaSamples = 1;
			}
			return descriptor;
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x000333FB File Offset: 0x000315FB
		public static void GetLightAttenuationAndSpotDirection(LightType lightType, float lightRange, Matrix4x4 lightLocalToWorldMatrix, float spotAngle, float? innerSpotAngle, out Vector4 lightAttenuation, out Vector4 lightSpotDir)
		{
			lightAttenuation = UniversalRenderPipeline.k_DefaultLightAttenuation;
			lightSpotDir = UniversalRenderPipeline.k_DefaultLightSpotDirection;
			if (lightType != LightType.Directional)
			{
				UniversalRenderPipeline.GetPunctualLightDistanceAttenuation(lightRange, ref lightAttenuation);
				if (lightType == LightType.Spot)
				{
					UniversalRenderPipeline.GetSpotDirection(ref lightLocalToWorldMatrix, out lightSpotDir);
					UniversalRenderPipeline.GetSpotAngleAttenuation(spotAngle, innerSpotAngle, ref lightAttenuation);
				}
			}
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x00033438 File Offset: 0x00031638
		internal static void GetPunctualLightDistanceAttenuation(float lightRange, ref Vector4 lightAttenuation)
		{
			float num = lightRange * lightRange;
			float num2 = 0.64000005f * num - num;
			float num3 = -num / num2;
			float num4 = 1f / Mathf.Max(0.0001f, num);
			lightAttenuation.x = num4;
			lightAttenuation.y = num3;
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x00033478 File Offset: 0x00031678
		internal static void GetSpotAngleAttenuation(float spotAngle, float? innerSpotAngle, ref Vector4 lightAttenuation)
		{
			float num = Mathf.Cos(0.017453292f * spotAngle * 0.5f);
			float num2;
			if (innerSpotAngle != null)
			{
				num2 = Mathf.Cos(innerSpotAngle.Value * 0.017453292f * 0.5f);
			}
			else
			{
				num2 = Mathf.Cos(2f * Mathf.Atan(Mathf.Tan(spotAngle * 0.5f * 0.017453292f) * 46f / 64f) * 0.5f);
			}
			float num3 = Mathf.Max(0.001f, num2 - num);
			float num4 = 1f / num3;
			float num5 = -num * num4;
			lightAttenuation.z = num4;
			lightAttenuation.w = num5;
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x0003351C File Offset: 0x0003171C
		internal static void GetSpotDirection(ref Matrix4x4 lightLocalToWorldMatrix, out Vector4 lightSpotDir)
		{
			Vector4 column = lightLocalToWorldMatrix.GetColumn(2);
			lightSpotDir = new Vector4(-column.x, -column.y, -column.z, 0f);
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x00033558 File Offset: 0x00031758
		public static void InitializeLightConstants_Common(NativeArray<VisibleLight> lights, int lightIndex, out Vector4 lightPos, out Vector4 lightColor, out Vector4 lightAttenuation, out Vector4 lightSpotDir, out Vector4 lightOcclusionProbeChannel)
		{
			lightPos = UniversalRenderPipeline.k_DefaultLightPosition;
			lightColor = UniversalRenderPipeline.k_DefaultLightColor;
			lightOcclusionProbeChannel = UniversalRenderPipeline.k_DefaultLightsProbeChannel;
			lightAttenuation = UniversalRenderPipeline.k_DefaultLightAttenuation;
			lightSpotDir = UniversalRenderPipeline.k_DefaultLightSpotDirection;
			if (lightIndex < 0)
			{
				return;
			}
			ref VisibleLight ptr = ref lights.UnsafeElementAtMutable<VisibleLight>(lightIndex);
			Light light = ptr.light;
			Matrix4x4 localToWorldMatrix = ptr.localToWorldMatrix;
			LightType lightType = ptr.lightType;
			if (lightType == LightType.Directional)
			{
				Vector4 vector = -localToWorldMatrix.GetColumn(2);
				lightPos = new Vector4(vector.x, vector.y, vector.z, 0f);
			}
			else
			{
				Vector4 column = localToWorldMatrix.GetColumn(3);
				lightPos = new Vector4(column.x, column.y, column.z, 1f);
				UniversalRenderPipeline.GetPunctualLightDistanceAttenuation(ptr.range, ref lightAttenuation);
				if (lightType == LightType.Spot)
				{
					UniversalRenderPipeline.GetSpotAngleAttenuation(ptr.spotAngle, (light != null) ? new float?(light.innerSpotAngle) : null, ref lightAttenuation);
					UniversalRenderPipeline.GetSpotDirection(ref localToWorldMatrix, out lightSpotDir);
				}
			}
			lightColor = ptr.finalColor;
			if (light != null && light.bakingOutput.lightmapBakeType == LightmapBakeType.Mixed && 0 <= light.bakingOutput.occlusionMaskChannel && light.bakingOutput.occlusionMaskChannel < 4)
			{
				lightOcclusionProbeChannel[light.bakingOutput.occlusionMaskChannel] = 1f;
			}
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x000336C5 File Offset: 0x000318C5
		private static void RecordRenderGraph(RenderGraph renderGraph, ScriptableRenderContext context, ref RenderingData renderingData)
		{
			renderingData.cameraData.renderer.RecordRenderGraph(renderGraph, context, ref renderingData);
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x000336DC File Offset: 0x000318DC
		private static void RecordAndExecuteRenderGraph(RenderGraph renderGraph, ScriptableRenderContext context, ref RenderingData renderingData)
		{
			CommandBuffer commandBuffer = renderingData.commandBuffer;
			Camera camera = renderingData.cameraData.camera;
			RenderGraphParameters renderGraphParameters = new RenderGraphParameters
			{
				executionName = UniversalRenderPipeline.Profiling.TryGetOrAddCameraSampler(camera).name,
				commandBuffer = commandBuffer,
				scriptableRenderContext = context,
				currentFrameIndex = Time.frameCount
			};
			using (renderGraph.RecordAndExecute(in renderGraphParameters))
			{
				UniversalRenderPipeline.RecordRenderGraph(renderGraph, context, ref renderingData);
			}
		}

		// Token: 0x04000655 RID: 1621
		public const string k_ShaderTagName = "UniversalPipeline";

		// Token: 0x04000656 RID: 1622
		internal const int k_DefaultRenderingLayerMask = 1;

		// Token: 0x04000657 RID: 1623
		private readonly DebugDisplaySettingsUI m_DebugDisplaySettingsUI = new DebugDisplaySettingsUI();

		// Token: 0x04000658 RID: 1624
		private UniversalRenderPipelineGlobalSettings m_GlobalSettings;

		// Token: 0x04000659 RID: 1625
		internal static bool cameraStackRequiresDepthForPostprocessing = false;

		// Token: 0x0400065A RID: 1626
		internal static RenderGraph s_RenderGraph;

		// Token: 0x0400065B RID: 1627
		internal static RTHandleResourcePool s_RTHandlePool;

		// Token: 0x0400065C RID: 1628
		private static bool useRenderGraph;

		// Token: 0x0400065D RID: 1629
		private readonly UniversalRenderPipelineAsset pipelineAsset;

		// Token: 0x0400065E RID: 1630
		internal bool enableHDROnce = true;

		// Token: 0x0400065F RID: 1631
		private static Vector4 k_DefaultLightPosition = new Vector4(0f, 0f, 1f, 0f);

		// Token: 0x04000660 RID: 1632
		private static Vector4 k_DefaultLightColor = Color.black;

		// Token: 0x04000661 RID: 1633
		private static Vector4 k_DefaultLightAttenuation = new Vector4(0f, 1f, 0f, 1f);

		// Token: 0x04000662 RID: 1634
		private static Vector4 k_DefaultLightSpotDirection = new Vector4(0f, 0f, 1f, 0f);

		// Token: 0x04000663 RID: 1635
		private static Vector4 k_DefaultLightsProbeChannel = new Vector4(0f, 0f, 0f, 0f);

		// Token: 0x04000664 RID: 1636
		private static List<Vector4> m_ShadowBiasData = new List<Vector4>();

		// Token: 0x04000665 RID: 1637
		private static List<int> m_ShadowResolutionData = new List<int>();

		// Token: 0x04000666 RID: 1638
		private Comparison<Camera> cameraComparison = (Camera camera1, Camera camera2) => (int)camera1.depth - (int)camera2.depth;

		// Token: 0x04000667 RID: 1639
		private static Lightmapping.RequestLightsDelegate lightsDelegate = delegate(Light[] requests, NativeArray<LightDataGI> lightsOutput)
		{
			LightDataGI lightDataGI = default(LightDataGI);
			if (!SupportedRenderingFeatures.active.enlighten || (SupportedRenderingFeatures.active.lightmapBakeTypes | LightmapBakeType.Realtime) == (LightmapBakeType)0)
			{
				for (int i = 0; i < requests.Length; i++)
				{
					Light light = requests[i];
					lightDataGI.InitNoBake(light.GetInstanceID());
					lightsOutput[i] = lightDataGI;
				}
				return;
			}
			for (int j = 0; j < requests.Length; j++)
			{
				Light light2 = requests[j];
				switch (light2.type)
				{
				case LightType.Spot:
				{
					SpotLight spotLight = default(SpotLight);
					LightmapperUtils.Extract(light2, ref spotLight);
					spotLight.innerConeAngle = light2.innerSpotAngle * 0.017453292f;
					spotLight.angularFalloff = AngularFalloffType.AnalyticAndInnerAngle;
					lightDataGI.Init(ref spotLight);
					break;
				}
				case LightType.Directional:
				{
					DirectionalLight directionalLight = default(DirectionalLight);
					LightmapperUtils.Extract(light2, ref directionalLight);
					lightDataGI.Init(ref directionalLight);
					break;
				}
				case LightType.Point:
				{
					PointLight pointLight = default(PointLight);
					LightmapperUtils.Extract(light2, ref pointLight);
					lightDataGI.Init(ref pointLight);
					break;
				}
				case LightType.Area:
					lightDataGI.InitNoBake(light2.GetInstanceID());
					break;
				case LightType.Disc:
					lightDataGI.InitNoBake(light2.GetInstanceID());
					break;
				default:
					lightDataGI.InitNoBake(light2.GetInstanceID());
					break;
				}
				lightDataGI.falloff = FalloffType.InverseSquared;
				lightsOutput[j] = lightDataGI;
			}
		};

		// Token: 0x020001C8 RID: 456
		internal static class Profiling
		{
			// Token: 0x06000B44 RID: 2884 RVA: 0x0004BA44 File Offset: 0x00049C44
			public static ProfilingSampler TryGetOrAddCameraSampler(Camera camera)
			{
				ProfilingSampler profilingSampler = null;
				int hashCode = camera.GetHashCode();
				if (!UniversalRenderPipeline.Profiling.s_HashSamplerCache.TryGetValue(hashCode, out profilingSampler))
				{
					profilingSampler = new ProfilingSampler("UniversalRenderPipeline.RenderSingleCameraInternal: " + camera.name);
					UniversalRenderPipeline.Profiling.s_HashSamplerCache.Add(hashCode, profilingSampler);
				}
				return profilingSampler;
			}

			// Token: 0x04000BDC RID: 3036
			private static Dictionary<int, ProfilingSampler> s_HashSamplerCache = new Dictionary<int, ProfilingSampler>();

			// Token: 0x04000BDD RID: 3037
			public static readonly ProfilingSampler unknownSampler = new ProfilingSampler("Unknown");

			// Token: 0x02000235 RID: 565
			public static class Pipeline
			{
				// Token: 0x04000D4B RID: 3403
				public static readonly ProfilingSampler beginContextRendering = new ProfilingSampler("RenderPipeline.BeginContextRendering");

				// Token: 0x04000D4C RID: 3404
				public static readonly ProfilingSampler endContextRendering = new ProfilingSampler("RenderPipeline.EndContextRendering");

				// Token: 0x04000D4D RID: 3405
				public static readonly ProfilingSampler beginCameraRendering = new ProfilingSampler("RenderPipeline.BeginCameraRendering");

				// Token: 0x04000D4E RID: 3406
				public static readonly ProfilingSampler endCameraRendering = new ProfilingSampler("RenderPipeline.EndCameraRendering");

				// Token: 0x04000D4F RID: 3407
				private const string k_Name = "UniversalRenderPipeline";

				// Token: 0x04000D50 RID: 3408
				public static readonly ProfilingSampler initializeCameraData = new ProfilingSampler("UniversalRenderPipeline.InitializeCameraData");

				// Token: 0x04000D51 RID: 3409
				public static readonly ProfilingSampler initializeStackedCameraData = new ProfilingSampler("UniversalRenderPipeline.InitializeStackedCameraData");

				// Token: 0x04000D52 RID: 3410
				public static readonly ProfilingSampler initializeAdditionalCameraData = new ProfilingSampler("UniversalRenderPipeline.InitializeAdditionalCameraData");

				// Token: 0x04000D53 RID: 3411
				public static readonly ProfilingSampler initializeRenderingData = new ProfilingSampler("UniversalRenderPipeline.InitializeRenderingData");

				// Token: 0x04000D54 RID: 3412
				public static readonly ProfilingSampler initializeShadowData = new ProfilingSampler("UniversalRenderPipeline.InitializeShadowData");

				// Token: 0x04000D55 RID: 3413
				public static readonly ProfilingSampler initializeLightData = new ProfilingSampler("UniversalRenderPipeline.InitializeLightData");

				// Token: 0x04000D56 RID: 3414
				public static readonly ProfilingSampler getPerObjectLightFlags = new ProfilingSampler("UniversalRenderPipeline.GetPerObjectLightFlags");

				// Token: 0x04000D57 RID: 3415
				public static readonly ProfilingSampler getMainLightIndex = new ProfilingSampler("UniversalRenderPipeline.GetMainLightIndex");

				// Token: 0x04000D58 RID: 3416
				public static readonly ProfilingSampler setupPerFrameShaderConstants = new ProfilingSampler("UniversalRenderPipeline.SetupPerFrameShaderConstants");

				// Token: 0x04000D59 RID: 3417
				public static readonly ProfilingSampler setupPerCameraShaderConstants = new ProfilingSampler("UniversalRenderPipeline.SetupPerCameraShaderConstants");

				// Token: 0x02000236 RID: 566
				public static class Renderer
				{
					// Token: 0x04000D5A RID: 3418
					private const string k_Name = "ScriptableRenderer";

					// Token: 0x04000D5B RID: 3419
					public static readonly ProfilingSampler setupCullingParameters = new ProfilingSampler("ScriptableRenderer.SetupCullingParameters");

					// Token: 0x04000D5C RID: 3420
					public static readonly ProfilingSampler setup = new ProfilingSampler("ScriptableRenderer.Setup");
				}

				// Token: 0x02000237 RID: 567
				public static class Context
				{
					// Token: 0x04000D5D RID: 3421
					private const string k_Name = "ScriptableRenderContext";

					// Token: 0x04000D5E RID: 3422
					public static readonly ProfilingSampler submit = new ProfilingSampler("ScriptableRenderContext.Submit");
				}
			}
		}

		// Token: 0x020001C9 RID: 457
		public class SingleCameraRequest
		{
			// Token: 0x04000BDE RID: 3038
			public RenderTexture destination;

			// Token: 0x04000BDF RID: 3039
			public int mipLevel;

			// Token: 0x04000BE0 RID: 3040
			public CubemapFace face = CubemapFace.Unknown;

			// Token: 0x04000BE1 RID: 3041
			public int slice;
		}
	}
}
