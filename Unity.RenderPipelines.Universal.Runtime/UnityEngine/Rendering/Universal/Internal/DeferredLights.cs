using System;
using Unity.Collections;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Experimental.Rendering.RenderGraphModule;

namespace UnityEngine.Rendering.Universal.Internal
{
	// Token: 0x02000118 RID: 280
	internal class DeferredLights
	{
		// Token: 0x17000210 RID: 528
		// (get) Token: 0x060008CE RID: 2254 RVA: 0x00038753 File Offset: 0x00036953
		internal int GBufferAlbedoIndex
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x060008CF RID: 2255 RVA: 0x00038756 File Offset: 0x00036956
		internal int GBufferSpecularMetallicIndex
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x060008D0 RID: 2256 RVA: 0x00038759 File Offset: 0x00036959
		internal int GBufferNormalSmoothnessIndex
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x060008D1 RID: 2257 RVA: 0x0003875C File Offset: 0x0003695C
		internal int GBufferLightingIndex
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x060008D2 RID: 2258 RVA: 0x0003875F File Offset: 0x0003695F
		internal int GbufferDepthIndex
		{
			get
			{
				if (!this.UseRenderPass)
				{
					return -1;
				}
				return this.GBufferLightingIndex + 1;
			}
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x060008D3 RID: 2259 RVA: 0x00038773 File Offset: 0x00036973
		internal int GBufferRenderingLayers
		{
			get
			{
				if (!this.UseRenderingLayers)
				{
					return -1;
				}
				return this.GBufferLightingIndex + (this.UseRenderPass ? 1 : 0) + 1;
			}
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x060008D4 RID: 2260 RVA: 0x00038794 File Offset: 0x00036994
		internal int GBufferShadowMask
		{
			get
			{
				if (!this.UseShadowMask)
				{
					return -1;
				}
				return this.GBufferLightingIndex + (this.UseRenderPass ? 1 : 0) + (this.UseRenderingLayers ? 1 : 0) + 1;
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x060008D5 RID: 2261 RVA: 0x000387C2 File Offset: 0x000369C2
		internal int GBufferSliceCount
		{
			get
			{
				return 4 + (this.UseRenderPass ? 1 : 0) + (this.UseShadowMask ? 1 : 0) + (this.UseRenderingLayers ? 1 : 0);
			}
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x000387EC File Offset: 0x000369EC
		internal GraphicsFormat GetGBufferFormat(int index)
		{
			if (index == this.GBufferAlbedoIndex)
			{
				if (QualitySettings.activeColorSpace != ColorSpace.Linear)
				{
					return GraphicsFormat.R8G8B8A8_UNorm;
				}
				return GraphicsFormat.R8G8B8A8_SRGB;
			}
			else
			{
				if (index == this.GBufferSpecularMetallicIndex)
				{
					return GraphicsFormat.R8G8B8A8_UNorm;
				}
				if (index == this.GBufferNormalSmoothnessIndex)
				{
					if (!this.AccurateGbufferNormals)
					{
						return DepthNormalOnlyPass.GetGraphicsFormat();
					}
					return GraphicsFormat.R8G8B8A8_UNorm;
				}
				else
				{
					if (index == this.GBufferLightingIndex)
					{
						return GraphicsFormat.None;
					}
					if (index == this.GbufferDepthIndex)
					{
						return GraphicsFormat.R32_SFloat;
					}
					if (index == this.GBufferShadowMask)
					{
						return GraphicsFormat.B8G8R8A8_UNorm;
					}
					if (index == this.GBufferRenderingLayers)
					{
						return RenderingLayerUtils.GetFormat(this.RenderingLayerMaskSize);
					}
					return GraphicsFormat.None;
				}
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x060008D7 RID: 2263 RVA: 0x0003886B File Offset: 0x00036A6B
		internal bool UseShadowMask
		{
			get
			{
				return this.MixedLightingSetup > MixedLightingSetup.None;
			}
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x060008D8 RID: 2264 RVA: 0x00038876 File Offset: 0x00036A76
		internal bool UseRenderingLayers
		{
			get
			{
				return this.UseLightLayers || this.UseDecalLayers;
			}
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x060008D9 RID: 2265 RVA: 0x00038888 File Offset: 0x00036A88
		// (set) Token: 0x060008DA RID: 2266 RVA: 0x00038890 File Offset: 0x00036A90
		internal RenderingLayerUtils.MaskSize RenderingLayerMaskSize { get; set; }

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x060008DB RID: 2267 RVA: 0x00038899 File Offset: 0x00036A99
		// (set) Token: 0x060008DC RID: 2268 RVA: 0x000388A1 File Offset: 0x00036AA1
		internal bool UseDecalLayers { get; set; }

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x060008DD RID: 2269 RVA: 0x000388AA File Offset: 0x00036AAA
		internal bool UseLightLayers
		{
			get
			{
				return UniversalRenderPipeline.asset.useRenderingLayers;
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x060008DE RID: 2270 RVA: 0x000388B6 File Offset: 0x00036AB6
		// (set) Token: 0x060008DF RID: 2271 RVA: 0x000388BE File Offset: 0x00036ABE
		internal bool UseRenderPass { get; set; }

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x060008E0 RID: 2272 RVA: 0x000388C7 File Offset: 0x00036AC7
		// (set) Token: 0x060008E1 RID: 2273 RVA: 0x000388CF File Offset: 0x00036ACF
		internal bool HasDepthPrepass { get; set; }

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x060008E2 RID: 2274 RVA: 0x000388D8 File Offset: 0x00036AD8
		// (set) Token: 0x060008E3 RID: 2275 RVA: 0x000388E0 File Offset: 0x00036AE0
		internal bool HasNormalPrepass { get; set; }

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x060008E4 RID: 2276 RVA: 0x000388E9 File Offset: 0x00036AE9
		// (set) Token: 0x060008E5 RID: 2277 RVA: 0x000388F1 File Offset: 0x00036AF1
		internal bool HasRenderingLayerPrepass { get; set; }

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x060008E6 RID: 2278 RVA: 0x000388FA File Offset: 0x00036AFA
		// (set) Token: 0x060008E7 RID: 2279 RVA: 0x00038902 File Offset: 0x00036B02
		internal bool IsOverlay { get; set; }

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x060008E8 RID: 2280 RVA: 0x0003890B File Offset: 0x00036B0B
		// (set) Token: 0x060008E9 RID: 2281 RVA: 0x00038913 File Offset: 0x00036B13
		internal bool AccurateGbufferNormals { get; set; }

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x060008EA RID: 2282 RVA: 0x0003891C File Offset: 0x00036B1C
		// (set) Token: 0x060008EB RID: 2283 RVA: 0x00038924 File Offset: 0x00036B24
		internal MixedLightingSetup MixedLightingSetup { get; set; }

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x060008EC RID: 2284 RVA: 0x0003892D File Offset: 0x00036B2D
		// (set) Token: 0x060008ED RID: 2285 RVA: 0x00038935 File Offset: 0x00036B35
		internal bool UseJobSystem { get; set; }

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x060008EE RID: 2286 RVA: 0x0003893E File Offset: 0x00036B3E
		// (set) Token: 0x060008EF RID: 2287 RVA: 0x00038946 File Offset: 0x00036B46
		internal int RenderWidth { get; set; }

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x060008F0 RID: 2288 RVA: 0x0003894F File Offset: 0x00036B4F
		// (set) Token: 0x060008F1 RID: 2289 RVA: 0x00038957 File Offset: 0x00036B57
		internal int RenderHeight { get; set; }

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x060008F2 RID: 2290 RVA: 0x00038960 File Offset: 0x00036B60
		// (set) Token: 0x060008F3 RID: 2291 RVA: 0x00038968 File Offset: 0x00036B68
		internal RTHandle[] GbufferAttachments { get; set; }

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x060008F4 RID: 2292 RVA: 0x00038971 File Offset: 0x00036B71
		// (set) Token: 0x060008F5 RID: 2293 RVA: 0x00038979 File Offset: 0x00036B79
		internal TextureHandle[] GbufferTextureHandles { get; set; }

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x060008F6 RID: 2294 RVA: 0x00038982 File Offset: 0x00036B82
		// (set) Token: 0x060008F7 RID: 2295 RVA: 0x0003898A File Offset: 0x00036B8A
		internal RTHandle[] DeferredInputAttachments { get; set; }

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x060008F8 RID: 2296 RVA: 0x00038993 File Offset: 0x00036B93
		// (set) Token: 0x060008F9 RID: 2297 RVA: 0x0003899B File Offset: 0x00036B9B
		internal bool[] DeferredInputIsTransient { get; set; }

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x060008FA RID: 2298 RVA: 0x000389A4 File Offset: 0x00036BA4
		// (set) Token: 0x060008FB RID: 2299 RVA: 0x000389AC File Offset: 0x00036BAC
		internal RTHandle DepthAttachment { get; set; }

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x060008FC RID: 2300 RVA: 0x000389B5 File Offset: 0x00036BB5
		// (set) Token: 0x060008FD RID: 2301 RVA: 0x000389BD File Offset: 0x00036BBD
		internal RTHandle DepthCopyTexture { get; set; }

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x060008FE RID: 2302 RVA: 0x000389C6 File Offset: 0x00036BC6
		// (set) Token: 0x060008FF RID: 2303 RVA: 0x000389CE File Offset: 0x00036BCE
		internal GraphicsFormat[] GbufferFormats { get; set; }

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000900 RID: 2304 RVA: 0x000389D7 File Offset: 0x00036BD7
		// (set) Token: 0x06000901 RID: 2305 RVA: 0x000389DF File Offset: 0x00036BDF
		internal RTHandle DepthAttachmentHandle { get; set; }

		// Token: 0x06000902 RID: 2306 RVA: 0x000389E8 File Offset: 0x00036BE8
		internal DeferredLights(DeferredLights.InitParams initParams, bool useNativeRenderPass = false)
		{
			DeferredConfig.IsOpenGL = SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLCore || SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES2 || SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES3;
			DeferredConfig.IsDX10 = SystemInfo.graphicsDeviceType == GraphicsDeviceType.Direct3D11 && SystemInfo.graphicsShaderLevel <= 40;
			this.m_StencilDeferredMaterial = initParams.stencilDeferredMaterial;
			this.m_StencilDeferredPasses = new int[DeferredLights.k_StencilDeferredPassNames.Length];
			this.InitStencilDeferredMaterial();
			this.AccurateGbufferNormals = true;
			this.UseJobSystem = true;
			this.UseRenderPass = useNativeRenderPass;
			this.m_LightCookieManager = initParams.lightCookieManager;
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x00038ABC File Offset: 0x00036CBC
		internal void SetupLights(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			Camera camera = renderingData.cameraData.camera;
			this.RenderWidth = (camera.allowDynamicResolution ? Mathf.CeilToInt(ScalableBufferManager.widthScaleFactor * (float)renderingData.cameraData.cameraTargetDescriptor.width) : renderingData.cameraData.cameraTargetDescriptor.width);
			this.RenderHeight = (camera.allowDynamicResolution ? Mathf.CeilToInt(ScalableBufferManager.heightScaleFactor * (float)renderingData.cameraData.cameraTargetDescriptor.height) : renderingData.cameraData.cameraTargetDescriptor.height);
			this.PrecomputeLights(out this.m_stencilVisLights, out this.m_stencilVisLightOffsets, ref renderingData.lightData.visibleLights, renderingData.lightData.additionalLightsCount != 0 || renderingData.lightData.mainLightIndex >= 0, renderingData.cameraData.camera.worldToCameraMatrix, renderingData.cameraData.camera.orthographic, renderingData.cameraData.camera.nearClipPlane);
			CommandBuffer commandBuffer = renderingData.commandBuffer;
			using (new ProfilingScope(commandBuffer, DeferredLights.m_ProfilingSetupLightConstants))
			{
				this.SetupShaderLightConstants(commandBuffer, ref renderingData);
				bool supportsMixedLighting = renderingData.lightData.supportsMixedLighting;
				CoreUtils.SetKeyword(commandBuffer, "_GBUFFER_NORMALS_OCT", this.AccurateGbufferNormals);
				bool flag = supportsMixedLighting && this.MixedLightingSetup == MixedLightingSetup.ShadowMask;
				bool flag2 = flag && QualitySettings.shadowmaskMode == ShadowmaskMode.Shadowmask;
				bool flag3 = supportsMixedLighting && this.MixedLightingSetup == MixedLightingSetup.Subtractive;
				CoreUtils.SetKeyword(commandBuffer, "LIGHTMAP_SHADOW_MIXING", flag3 || flag2);
				CoreUtils.SetKeyword(commandBuffer, "SHADOWS_SHADOWMASK", flag);
				CoreUtils.SetKeyword(commandBuffer, "_MIXED_LIGHTING_SUBTRACTIVE", flag3);
				CoreUtils.SetKeyword(commandBuffer, "_RENDER_PASS_ENABLED", this.UseRenderPass && renderingData.cameraData.cameraType == CameraType.Game);
				CoreUtils.SetKeyword(commandBuffer, "_LIGHT_LAYERS", this.UseLightLayers && !CoreUtils.IsSceneLightingDisabled(camera));
				RenderingLayerUtils.SetupProperties(commandBuffer, this.RenderingLayerMaskSize);
			}
			context.ExecuteCommandBuffer(commandBuffer);
			commandBuffer.Clear();
		}

		// Token: 0x06000904 RID: 2308 RVA: 0x00038CCC File Offset: 0x00036ECC
		internal void ResolveMixedLightingMode(ref RenderingData renderingData)
		{
			this.MixedLightingSetup = MixedLightingSetup.None;
			if (renderingData.lightData.supportsMixedLighting)
			{
				NativeArray<VisibleLight> visibleLights = renderingData.lightData.visibleLights;
				int num = 0;
				while (num < renderingData.lightData.visibleLights.Length && this.MixedLightingSetup == MixedLightingSetup.None)
				{
					Light light = visibleLights.UnsafeElementAtMutable<VisibleLight>(num).light;
					if (light != null && light.bakingOutput.lightmapBakeType == LightmapBakeType.Mixed && light.shadows != LightShadows.None)
					{
						MixedLightingMode mixedLightingMode = light.bakingOutput.mixedLightingMode;
						if (mixedLightingMode != MixedLightingMode.Subtractive)
						{
							if (mixedLightingMode == MixedLightingMode.Shadowmask)
							{
								this.MixedLightingSetup = MixedLightingSetup.ShadowMask;
							}
						}
						else
						{
							this.MixedLightingSetup = MixedLightingSetup.Subtractive;
						}
					}
					num++;
				}
			}
			this.CreateGbufferResources();
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x00038D77 File Offset: 0x00036F77
		internal void DisableFramebufferFetchInput()
		{
			this.UseRenderPass = false;
			this.CreateGbufferResources();
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x00038D88 File Offset: 0x00036F88
		internal void ReleaseGbufferResources()
		{
			if (this.GbufferRTHandles != null)
			{
				for (int i = 0; i < this.GbufferRTHandles.Length; i++)
				{
					RTHandles.Release(this.GbufferRTHandles[i]);
				}
			}
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x00038DC0 File Offset: 0x00036FC0
		internal void ReAllocateGBufferIfNeeded(RenderTextureDescriptor gbufferSlice, int gbufferIndex)
		{
			if (this.GbufferRTHandles != null)
			{
				if (this.GbufferRTHandles[gbufferIndex].GetInstanceID() != this.GbufferAttachments[gbufferIndex].GetInstanceID())
				{
					return;
				}
				gbufferSlice.depthBufferBits = 0;
				gbufferSlice.stencilFormat = GraphicsFormat.None;
				gbufferSlice.graphicsFormat = this.GetGBufferFormat(gbufferIndex);
				RenderingUtils.ReAllocateIfNeeded(ref this.GbufferRTHandles[gbufferIndex], in gbufferSlice, FilterMode.Point, TextureWrapMode.Clamp, false, 1, 0f, DeferredLights.k_GBufferNames[gbufferIndex]);
				this.GbufferAttachments[gbufferIndex] = this.GbufferRTHandles[gbufferIndex];
			}
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x00038E44 File Offset: 0x00037044
		internal void CreateGbufferResources()
		{
			int gbufferSliceCount = this.GBufferSliceCount;
			if (this.GbufferRTHandles == null || this.GbufferRTHandles.Length != gbufferSliceCount)
			{
				this.ReleaseGbufferResources();
				this.GbufferAttachments = new RTHandle[gbufferSliceCount];
				this.GbufferRTHandles = new RTHandle[gbufferSliceCount];
				this.GbufferFormats = new GraphicsFormat[gbufferSliceCount];
				this.GbufferTextureHandles = new TextureHandle[gbufferSliceCount];
				for (int i = 0; i < gbufferSliceCount; i++)
				{
					this.GbufferRTHandles[i] = RTHandles.Alloc(DeferredLights.k_GBufferNames[i], DeferredLights.k_GBufferNames[i]);
					this.GbufferAttachments[i] = this.GbufferRTHandles[i];
					this.GbufferFormats[i] = this.GetGBufferFormat(i);
				}
			}
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x00038EF0 File Offset: 0x000370F0
		internal void UpdateDeferredInputAttachments()
		{
			this.DeferredInputAttachments[0] = this.GbufferAttachments[0];
			this.DeferredInputAttachments[1] = this.GbufferAttachments[1];
			this.DeferredInputAttachments[2] = this.GbufferAttachments[2];
			this.DeferredInputAttachments[3] = this.GbufferAttachments[4];
			if (this.UseShadowMask)
			{
				this.DeferredInputAttachments[4] = this.GbufferAttachments[this.GBufferShadowMask];
			}
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x00038F5A File Offset: 0x0003715A
		internal bool IsRuntimeSupportedThisFrame()
		{
			return this.GBufferSliceCount <= SystemInfo.supportedRenderTargetCount && !DeferredConfig.IsOpenGL && !DeferredConfig.IsDX10;
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x00038F7C File Offset: 0x0003717C
		public void Setup(ref RenderingData renderingData, AdditionalLightsShadowCasterPass additionalLightsShadowCasterPass, bool hasDepthPrepass, bool hasNormalPrepass, bool hasRenderingLayerPrepass, RTHandle depthCopyTexture, RTHandle depthAttachment, RTHandle colorAttachment)
		{
			this.m_AdditionalLightsShadowCasterPass = additionalLightsShadowCasterPass;
			this.HasDepthPrepass = hasDepthPrepass;
			this.HasNormalPrepass = hasNormalPrepass;
			this.HasRenderingLayerPrepass = hasRenderingLayerPrepass;
			this.DepthCopyTexture = depthCopyTexture;
			this.GbufferAttachments[this.GBufferLightingIndex] = colorAttachment;
			this.DepthAttachment = depthAttachment;
			int num = 4 + (this.UseShadowMask ? 1 : 0);
			if ((this.DeferredInputAttachments == null && this.UseRenderPass && this.GbufferAttachments.Length >= 3) || (this.DeferredInputAttachments != null && num != this.DeferredInputAttachments.Length))
			{
				this.DeferredInputAttachments = new RTHandle[num];
				this.DeferredInputIsTransient = new bool[num];
				int num2 = 0;
				int i = 0;
				while (i < num)
				{
					if (num2 == this.GBufferLightingIndex)
					{
						num2++;
					}
					this.DeferredInputAttachments[i] = this.GbufferAttachments[num2];
					this.DeferredInputIsTransient[i] = num2 != this.GbufferDepthIndex;
					i++;
					num2++;
				}
			}
			this.DepthAttachmentHandle = this.DepthAttachment;
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x0003906A File Offset: 0x0003726A
		internal void Setup(AdditionalLightsShadowCasterPass additionalLightsShadowCasterPass)
		{
			this.m_AdditionalLightsShadowCasterPass = additionalLightsShadowCasterPass;
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x00039073 File Offset: 0x00037273
		public void OnCameraCleanup(CommandBuffer cmd)
		{
			CoreUtils.SetKeyword(cmd, "_GBUFFER_NORMALS_OCT", false);
			if (this.m_stencilVisLights.IsCreated)
			{
				this.m_stencilVisLights.Dispose();
			}
			if (this.m_stencilVisLightOffsets.IsCreated)
			{
				this.m_stencilVisLightOffsets.Dispose();
			}
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x000390B4 File Offset: 0x000372B4
		internal static StencilState OverwriteStencil(StencilState s, int stencilWriteMask)
		{
			if (!s.enabled)
			{
				return new StencilState(true, 0, (byte)stencilWriteMask, CompareFunction.Always, StencilOp.Replace, StencilOp.Keep, StencilOp.Keep, CompareFunction.Always, StencilOp.Replace, StencilOp.Keep, StencilOp.Keep);
			}
			CompareFunction compareFunction = ((s.compareFunctionFront != CompareFunction.Disabled) ? s.compareFunctionFront : CompareFunction.Always);
			CompareFunction compareFunction2 = ((s.compareFunctionBack != CompareFunction.Disabled) ? s.compareFunctionBack : CompareFunction.Always);
			StencilOp passOperationFront = s.passOperationFront;
			StencilOp failOperationFront = s.failOperationFront;
			StencilOp zFailOperationFront = s.zFailOperationFront;
			StencilOp passOperationBack = s.passOperationBack;
			StencilOp failOperationBack = s.failOperationBack;
			StencilOp zFailOperationBack = s.zFailOperationBack;
			return new StencilState(true, s.readMask & 15, (byte)((int)s.writeMask | stencilWriteMask), compareFunction, passOperationFront, failOperationFront, zFailOperationFront, compareFunction2, passOperationBack, failOperationBack, zFailOperationBack);
		}

		// Token: 0x0600090F RID: 2319 RVA: 0x00039160 File Offset: 0x00037360
		internal static RenderStateBlock OverwriteStencil(RenderStateBlock block, int stencilWriteMask, int stencilRef)
		{
			if (!block.stencilState.enabled)
			{
				block.stencilState = new StencilState(true, 0, (byte)stencilWriteMask, CompareFunction.Always, StencilOp.Replace, StencilOp.Keep, StencilOp.Keep, CompareFunction.Always, StencilOp.Replace, StencilOp.Keep, StencilOp.Keep);
			}
			else
			{
				StencilState stencilState = block.stencilState;
				CompareFunction compareFunction = ((stencilState.compareFunctionFront != CompareFunction.Disabled) ? stencilState.compareFunctionFront : CompareFunction.Always);
				CompareFunction compareFunction2 = ((stencilState.compareFunctionBack != CompareFunction.Disabled) ? stencilState.compareFunctionBack : CompareFunction.Always);
				StencilOp passOperationFront = stencilState.passOperationFront;
				StencilOp failOperationFront = stencilState.failOperationFront;
				StencilOp zFailOperationFront = stencilState.zFailOperationFront;
				StencilOp passOperationBack = stencilState.passOperationBack;
				StencilOp failOperationBack = stencilState.failOperationBack;
				StencilOp zFailOperationBack = stencilState.zFailOperationBack;
				block.stencilState = new StencilState(true, stencilState.readMask & 15, (byte)((int)stencilState.writeMask | stencilWriteMask), compareFunction, passOperationFront, failOperationFront, zFailOperationFront, compareFunction2, passOperationBack, failOperationBack, zFailOperationBack);
			}
			block.mask |= RenderStateMask.Stencil;
			block.stencilReference = (block.stencilReference & 15) | stencilRef;
			return block;
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x00039254 File Offset: 0x00037454
		internal void ClearStencilPartial(CommandBuffer cmd)
		{
			if (this.m_FullscreenMesh == null)
			{
				this.m_FullscreenMesh = DeferredLights.CreateFullscreenMesh();
			}
			using (new ProfilingScope(cmd, this.m_ProfilingSamplerClearStencilPartialPass))
			{
				cmd.DrawMesh(this.m_FullscreenMesh, Matrix4x4.identity, this.m_StencilDeferredMaterial, 0, this.m_StencilDeferredPasses[5]);
			}
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x000392C8 File Offset: 0x000374C8
		internal void ExecuteDeferredPass(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			if (this.m_StencilDeferredPasses[0] < 0)
			{
				this.InitStencilDeferredMaterial();
			}
			CommandBuffer commandBuffer = renderingData.commandBuffer;
			using (new ProfilingScope(commandBuffer, DeferredLights.m_ProfilingDeferredPass))
			{
				CoreUtils.SetKeyword(commandBuffer, "_DEFERRED_MIXED_LIGHTING", this.UseShadowMask);
				this.SetupMatrixConstants(commandBuffer, ref renderingData);
				if (!this.HasStencilLightsOfType(LightType.Directional))
				{
					this.RenderSSAOBeforeShading(commandBuffer, ref renderingData);
				}
				this.RenderStencilLights(context, commandBuffer, ref renderingData);
				CoreUtils.SetKeyword(commandBuffer, "_DEFERRED_MIXED_LIGHTING", false);
				this.RenderFog(context, commandBuffer, ref renderingData);
			}
			CoreUtils.SetKeyword(commandBuffer, "_ADDITIONAL_LIGHT_SHADOWS", renderingData.shadowData.isKeywordAdditionalLightShadowsEnabled);
			ShadowUtils.SetSoftShadowQualityShaderKeywords(commandBuffer, ref renderingData.shadowData);
			CoreUtils.SetKeyword(commandBuffer, "_LIGHT_COOKIES", this.m_LightCookieManager != null && this.m_LightCookieManager.IsKeywordLightCookieEnabled);
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x000393A8 File Offset: 0x000375A8
		private void SetupShaderLightConstants(CommandBuffer cmd, ref RenderingData renderingData)
		{
			this.SetupMainLightConstants(cmd, ref renderingData.lightData);
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x000393B8 File Offset: 0x000375B8
		private void SetupMainLightConstants(CommandBuffer cmd, ref LightData lightData)
		{
			if (lightData.mainLightIndex < 0)
			{
				return;
			}
			Vector4 vector;
			Vector4 vector2;
			Vector4 vector3;
			Vector4 vector4;
			Vector4 vector5;
			UniversalRenderPipeline.InitializeLightConstants_Common(lightData.visibleLights, lightData.mainLightIndex, out vector, out vector2, out vector3, out vector4, out vector5);
			uint num = RenderingLayerUtils.ToValidRenderingLayers(lightData.visibleLights[lightData.mainLightIndex].light.GetUniversalAdditionalLightData().renderingLayers);
			cmd.SetGlobalVector(DeferredLights.ShaderConstants._MainLightPosition, vector);
			cmd.SetGlobalVector(DeferredLights.ShaderConstants._MainLightColor, vector2);
			cmd.SetGlobalInt(DeferredLights.ShaderConstants._MainLightLayerMask, (int)num);
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x0003943C File Offset: 0x0003763C
		internal Matrix4x4[] GetScreenToWorldMatrix(CameraData cameraData)
		{
			int num = ((cameraData.xr.enabled && cameraData.xr.singlePassEnabled) ? 2 : 1);
			Matrix4x4[] screenToWorld = this.m_ScreenToWorld;
			Matrix4x4 matrix4x = new Matrix4x4(new Vector4(2f / (float)this.RenderWidth, 0f, 0f, 0f), new Vector4(0f, 2f / (float)this.RenderHeight, 0f, 0f), new Vector4(0f, 0f, 1f, 0f), new Vector4(-1f, -1f, 0f, 1f));
			if (DeferredConfig.IsOpenGL)
			{
				matrix4x = new Matrix4x4(new Vector4(1f, 0f, 0f, 0f), new Vector4(0f, 1f, 0f, 0f), new Vector4(0f, 0f, 2f, 0f), new Vector4(0f, 0f, -1f, 1f)) * matrix4x;
			}
			for (int i = 0; i < num; i++)
			{
				Matrix4x4 viewMatrix = cameraData.GetViewMatrix(i);
				Matrix4x4 gpuprojectionMatrix = cameraData.GetGPUProjectionMatrix(false, i);
				screenToWorld[i] = Matrix4x4.Inverse(gpuprojectionMatrix * viewMatrix) * matrix4x;
			}
			return screenToWorld;
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x000395A0 File Offset: 0x000377A0
		private void SetupMatrixConstants(CommandBuffer cmd, ref RenderingData renderingData)
		{
			cmd.SetGlobalMatrixArray(DeferredLights.ShaderConstants._ScreenToWorld, this.GetScreenToWorldMatrix(renderingData.cameraData));
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x000395BC File Offset: 0x000377BC
		private void PrecomputeLights(out NativeArray<ushort> stencilVisLights, out NativeArray<ushort> stencilVisLightOffsets, ref NativeArray<VisibleLight> visibleLights, bool hasAdditionalLights, Matrix4x4 view, bool isOrthographic, float zNear)
		{
			if (!hasAdditionalLights)
			{
				stencilVisLights = new NativeArray<ushort>(0, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
				stencilVisLightOffsets = new NativeArray<ushort>(5, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
				for (int i = 0; i < 5; i++)
				{
					stencilVisLightOffsets[i] = DeferredLights.k_InvalidLightOffset;
				}
				return;
			}
			NativeArray<int> nativeArray = new NativeArray<int>(5, Allocator.Temp, NativeArrayOptions.ClearMemory);
			stencilVisLightOffsets = new NativeArray<ushort>(5, Allocator.Temp, NativeArrayOptions.ClearMemory);
			int length = visibleLights.Length;
			ushort num = 0;
			while ((int)num < length)
			{
				ref VisibleLight ptr = ref visibleLights.UnsafeElementAtMutable<VisibleLight>((int)num);
				ref NativeArray<ushort> ptr2 = ref stencilVisLightOffsets;
				int num2 = (int)ptr.lightType;
				ushort num3 = ptr2[num2] + 1;
				ptr2[num2] = num3;
				num += 1;
			}
			int num4 = (int)(stencilVisLightOffsets[0] + stencilVisLightOffsets[1] + stencilVisLightOffsets[2]);
			stencilVisLights = new NativeArray<ushort>(num4, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			int j = 0;
			int num5 = 0;
			while (j < stencilVisLightOffsets.Length)
			{
				if (stencilVisLightOffsets[j] == 0)
				{
					stencilVisLightOffsets[j] = DeferredLights.k_InvalidLightOffset;
				}
				else
				{
					int num6 = (int)stencilVisLightOffsets[j];
					stencilVisLightOffsets[j] = (ushort)num5;
					num5 += num6;
				}
				j++;
			}
			ushort num7 = 0;
			while ((int)num7 < length)
			{
				ref VisibleLight ptr3 = ref visibleLights.UnsafeElementAtMutable<VisibleLight>((int)num7);
				int num2 = (int)ptr3.lightType;
				int num8 = nativeArray[num2];
				nativeArray[num2] = num8 + 1;
				int num9 = num8;
				stencilVisLights[(int)stencilVisLightOffsets[(int)ptr3.lightType] + num9] = num7;
				num7 += 1;
			}
			nativeArray.Dispose();
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x00039738 File Offset: 0x00037938
		private bool HasStencilLightsOfType(LightType type)
		{
			return this.m_stencilVisLightOffsets[(int)type] != DeferredLights.k_InvalidLightOffset;
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x00039750 File Offset: 0x00037950
		private void RenderStencilLights(ScriptableRenderContext context, CommandBuffer cmd, ref RenderingData renderingData)
		{
			if (this.m_stencilVisLights.Length == 0)
			{
				return;
			}
			if (this.m_StencilDeferredMaterial == null)
			{
				Debug.LogErrorFormat("Missing {0}. {1} render pass will not execute. Check for missing reference in the renderer resources.", new object[]
				{
					this.m_StencilDeferredMaterial,
					base.GetType().Name
				});
				return;
			}
			using (new ProfilingScope(cmd, this.m_ProfilingSamplerDeferredStencilPass))
			{
				NativeArray<VisibleLight> visibleLights = renderingData.lightData.visibleLights;
				if (this.HasStencilLightsOfType(LightType.Directional))
				{
					this.RenderStencilDirectionalLights(cmd, ref renderingData, visibleLights, renderingData.lightData.mainLightIndex);
				}
				if (this.HasStencilLightsOfType(LightType.Point))
				{
					this.RenderStencilPointLights(cmd, ref renderingData, visibleLights);
				}
				if (this.HasStencilLightsOfType(LightType.Spot))
				{
					this.RenderStencilSpotLights(cmd, ref renderingData, visibleLights);
				}
			}
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x0003981C File Offset: 0x00037A1C
		private void SetAdditionalLightsShadowsKeyword(ref CommandBuffer cmd, ref RenderingData renderingData, bool hasDeferredShadows)
		{
			bool additionalLightShadowsEnabled = renderingData.shadowData.additionalLightShadowsEnabled;
			bool flag = !renderingData.cameraData.renderer.stripShadowsOffVariants;
			bool flag2 = additionalLightShadowsEnabled && (!flag || hasDeferredShadows);
			CoreUtils.SetKeyword(cmd, "_ADDITIONAL_LIGHT_SHADOWS", flag2);
		}

		// Token: 0x0600091A RID: 2330 RVA: 0x00039864 File Offset: 0x00037A64
		private void RenderStencilDirectionalLights(CommandBuffer cmd, ref RenderingData renderingData, NativeArray<VisibleLight> visibleLights, int mainLightIndex)
		{
			if (this.m_FullscreenMesh == null)
			{
				this.m_FullscreenMesh = DeferredLights.CreateFullscreenMesh();
			}
			cmd.EnableShaderKeyword("_DIRECTIONAL");
			bool flag = true;
			for (int i = (int)this.m_stencilVisLightOffsets[1]; i < this.m_stencilVisLights.Length; i++)
			{
				ushort num = this.m_stencilVisLights[i];
				ref VisibleLight ptr = ref visibleLights.UnsafeElementAtMutable<VisibleLight>((int)num);
				if (ptr.lightType != LightType.Directional)
				{
					break;
				}
				Light light = ptr.light;
				Vector4 vector;
				Vector4 vector2;
				Vector4 vector3;
				Vector4 vector4;
				Vector4 vector5;
				UniversalRenderPipeline.InitializeLightConstants_Common(visibleLights, (int)num, out vector, out vector2, out vector3, out vector4, out vector5);
				int num2 = 0;
				if (light.bakingOutput.lightmapBakeType == LightmapBakeType.Mixed)
				{
					num2 |= 4;
				}
				uint num3 = RenderingLayerUtils.ToValidRenderingLayers(light.GetUniversalAdditionalLightData().renderingLayers);
				bool flag2;
				if ((int)num == mainLightIndex)
				{
					flag2 = light && light.shadows > LightShadows.None;
				}
				else
				{
					int num4 = ((this.m_AdditionalLightsShadowCasterPass != null) ? this.m_AdditionalLightsShadowCasterPass.GetShadowLightIndexFromLightIndex((int)num) : (-1));
					flag2 = light && light.shadows != LightShadows.None && num4 >= 0;
					cmd.SetGlobalInt(DeferredLights.ShaderConstants._ShadowLightIndex, num4);
				}
				this.SetAdditionalLightsShadowsKeyword(ref cmd, ref renderingData, flag2);
				bool flag3 = flag2 && renderingData.shadowData.supportsSoftShadows && light.shadows == LightShadows.Soft;
				ShadowUtils.SetPerLightSoftShadowKeyword(cmd, flag3);
				CoreUtils.SetKeyword(cmd, "_DEFERRED_FIRST_LIGHT", flag);
				CoreUtils.SetKeyword(cmd, "_DEFERRED_MAIN_LIGHT", (int)num == mainLightIndex);
				cmd.SetGlobalVector(DeferredLights.ShaderConstants._LightColor, vector2);
				cmd.SetGlobalVector(DeferredLights.ShaderConstants._LightDirection, vector);
				cmd.SetGlobalInt(DeferredLights.ShaderConstants._LightFlags, num2);
				cmd.SetGlobalInt(DeferredLights.ShaderConstants._LightLayerMask, (int)num3);
				cmd.DrawMesh(this.m_FullscreenMesh, Matrix4x4.identity, this.m_StencilDeferredMaterial, 0, this.m_StencilDeferredPasses[3]);
				cmd.DrawMesh(this.m_FullscreenMesh, Matrix4x4.identity, this.m_StencilDeferredMaterial, 0, this.m_StencilDeferredPasses[4]);
				flag = false;
			}
			cmd.DisableShaderKeyword("_DIRECTIONAL");
		}

		// Token: 0x0600091B RID: 2331 RVA: 0x00039A58 File Offset: 0x00037C58
		private void RenderStencilPointLights(CommandBuffer cmd, ref RenderingData renderingData, NativeArray<VisibleLight> visibleLights)
		{
			if (this.m_SphereMesh == null)
			{
				this.m_SphereMesh = DeferredLights.CreateSphereMesh();
			}
			cmd.EnableShaderKeyword("_POINT");
			for (int i = (int)this.m_stencilVisLightOffsets[2]; i < this.m_stencilVisLights.Length; i++)
			{
				ushort num = this.m_stencilVisLights[i];
				ref VisibleLight ptr = ref visibleLights.UnsafeElementAtMutable<VisibleLight>((int)num);
				if (ptr.lightType != LightType.Point)
				{
					break;
				}
				Light light = ptr.light;
				Vector3 vector = ptr.localToWorldMatrix.GetColumn(3);
				Matrix4x4 matrix4x = new Matrix4x4(new Vector4(ptr.range, 0f, 0f, 0f), new Vector4(0f, ptr.range, 0f, 0f), new Vector4(0f, 0f, ptr.range, 0f), new Vector4(vector.x, vector.y, vector.z, 1f));
				Vector4 vector2;
				Vector4 vector3;
				Vector4 vector4;
				Vector4 vector5;
				Vector4 vector6;
				UniversalRenderPipeline.InitializeLightConstants_Common(visibleLights, (int)num, out vector2, out vector3, out vector4, out vector5, out vector6);
				uint num2 = RenderingLayerUtils.ToValidRenderingLayers(light.GetUniversalAdditionalLightData().renderingLayers);
				int num3 = 0;
				if (light.bakingOutput.lightmapBakeType == LightmapBakeType.Mixed)
				{
					num3 |= 4;
				}
				int num4 = ((this.m_AdditionalLightsShadowCasterPass != null) ? this.m_AdditionalLightsShadowCasterPass.GetShadowLightIndexFromLightIndex((int)num) : (-1));
				bool flag = light && light.shadows != LightShadows.None && num4 >= 0;
				this.SetAdditionalLightsShadowsKeyword(ref cmd, ref renderingData, flag);
				bool flag2 = flag && renderingData.shadowData.supportsSoftShadows && light.shadows == LightShadows.Soft;
				ShadowUtils.SetPerLightSoftShadowKeyword(cmd, flag2);
				if (this.m_LightCookieManager != null)
				{
					int lightCookieShaderDataIndex = this.m_LightCookieManager.GetLightCookieShaderDataIndex((int)num);
					CoreUtils.SetKeyword(cmd, "_LIGHT_COOKIES", lightCookieShaderDataIndex >= 0);
					cmd.SetGlobalInt(DeferredLights.ShaderConstants._CookieLightIndex, lightCookieShaderDataIndex);
				}
				cmd.SetGlobalVector(DeferredLights.ShaderConstants._LightPosWS, vector2);
				cmd.SetGlobalVector(DeferredLights.ShaderConstants._LightColor, vector3);
				cmd.SetGlobalVector(DeferredLights.ShaderConstants._LightAttenuation, vector4);
				cmd.SetGlobalVector(DeferredLights.ShaderConstants._LightOcclusionProbInfo, vector6);
				cmd.SetGlobalInt(DeferredLights.ShaderConstants._LightFlags, num3);
				cmd.SetGlobalInt(DeferredLights.ShaderConstants._ShadowLightIndex, num4);
				cmd.SetGlobalInt(DeferredLights.ShaderConstants._LightLayerMask, (int)num2);
				cmd.DrawMesh(this.m_SphereMesh, matrix4x, this.m_StencilDeferredMaterial, 0, this.m_StencilDeferredPasses[0]);
				cmd.DrawMesh(this.m_SphereMesh, matrix4x, this.m_StencilDeferredMaterial, 0, this.m_StencilDeferredPasses[1]);
				cmd.DrawMesh(this.m_SphereMesh, matrix4x, this.m_StencilDeferredMaterial, 0, this.m_StencilDeferredPasses[2]);
			}
			cmd.DisableShaderKeyword("_POINT");
		}

		// Token: 0x0600091C RID: 2332 RVA: 0x00039CF8 File Offset: 0x00037EF8
		private void RenderStencilSpotLights(CommandBuffer cmd, ref RenderingData renderingData, NativeArray<VisibleLight> visibleLights)
		{
			if (this.m_HemisphereMesh == null)
			{
				this.m_HemisphereMesh = DeferredLights.CreateHemisphereMesh();
			}
			cmd.EnableShaderKeyword("_SPOT");
			for (int i = (int)this.m_stencilVisLightOffsets[0]; i < this.m_stencilVisLights.Length; i++)
			{
				ushort num = this.m_stencilVisLights[i];
				ref VisibleLight ptr = ref visibleLights.UnsafeElementAtMutable<VisibleLight>((int)num);
				if (ptr.lightType != LightType.Spot)
				{
					break;
				}
				Light light = ptr.light;
				float num2 = 0.017453292f * ptr.spotAngle * 0.5f;
				float num3 = Mathf.Cos(num2);
				float num4 = Mathf.Sin(num2);
				float num5 = Mathf.Lerp(1f, DeferredLights.kStencilShapeGuard, num4);
				Vector4 vector;
				Vector4 vector2;
				Vector4 vector3;
				Vector4 vector4;
				Vector4 vector5;
				UniversalRenderPipeline.InitializeLightConstants_Common(visibleLights, (int)num, out vector, out vector2, out vector3, out vector4, out vector5);
				uint num6 = RenderingLayerUtils.ToValidRenderingLayers(light.GetUniversalAdditionalLightData().renderingLayers);
				int num7 = 0;
				if (light.bakingOutput.lightmapBakeType == LightmapBakeType.Mixed)
				{
					num7 |= 4;
				}
				int num8 = ((this.m_AdditionalLightsShadowCasterPass != null) ? this.m_AdditionalLightsShadowCasterPass.GetShadowLightIndexFromLightIndex((int)num) : (-1));
				bool flag = light && light.shadows != LightShadows.None && num8 >= 0;
				this.SetAdditionalLightsShadowsKeyword(ref cmd, ref renderingData, flag);
				bool flag2 = flag && renderingData.shadowData.supportsSoftShadows && light.shadows == LightShadows.Soft;
				ShadowUtils.SetPerLightSoftShadowKeyword(cmd, flag2);
				if (this.m_LightCookieManager != null)
				{
					int lightCookieShaderDataIndex = this.m_LightCookieManager.GetLightCookieShaderDataIndex((int)num);
					CoreUtils.SetKeyword(cmd, "_LIGHT_COOKIES", lightCookieShaderDataIndex >= 0);
					cmd.SetGlobalInt(DeferredLights.ShaderConstants._CookieLightIndex, lightCookieShaderDataIndex);
				}
				cmd.SetGlobalVector(DeferredLights.ShaderConstants._SpotLightScale, new Vector4(num4, num4, 1f - num3, ptr.range));
				cmd.SetGlobalVector(DeferredLights.ShaderConstants._SpotLightBias, new Vector4(0f, 0f, num3, 0f));
				cmd.SetGlobalVector(DeferredLights.ShaderConstants._SpotLightGuard, new Vector4(num5, num5, num5, num3 * ptr.range));
				cmd.SetGlobalVector(DeferredLights.ShaderConstants._LightPosWS, vector);
				cmd.SetGlobalVector(DeferredLights.ShaderConstants._LightColor, vector2);
				cmd.SetGlobalVector(DeferredLights.ShaderConstants._LightAttenuation, vector3);
				cmd.SetGlobalVector(DeferredLights.ShaderConstants._LightDirection, new Vector3(vector4.x, vector4.y, vector4.z));
				cmd.SetGlobalVector(DeferredLights.ShaderConstants._LightOcclusionProbInfo, vector5);
				cmd.SetGlobalInt(DeferredLights.ShaderConstants._LightFlags, num7);
				cmd.SetGlobalInt(DeferredLights.ShaderConstants._ShadowLightIndex, num8);
				cmd.SetGlobalInt(DeferredLights.ShaderConstants._LightLayerMask, (int)num6);
				cmd.DrawMesh(this.m_HemisphereMesh, ptr.localToWorldMatrix, this.m_StencilDeferredMaterial, 0, this.m_StencilDeferredPasses[0]);
				cmd.DrawMesh(this.m_HemisphereMesh, ptr.localToWorldMatrix, this.m_StencilDeferredMaterial, 0, this.m_StencilDeferredPasses[1]);
				cmd.DrawMesh(this.m_HemisphereMesh, ptr.localToWorldMatrix, this.m_StencilDeferredMaterial, 0, this.m_StencilDeferredPasses[2]);
			}
			cmd.DisableShaderKeyword("_SPOT");
		}

		// Token: 0x0600091D RID: 2333 RVA: 0x00039FD8 File Offset: 0x000381D8
		private void RenderSSAOBeforeShading(CommandBuffer cmd, ref RenderingData renderingData)
		{
			if (this.m_FullscreenMesh == null)
			{
				this.m_FullscreenMesh = DeferredLights.CreateFullscreenMesh();
			}
			cmd.DrawMesh(this.m_FullscreenMesh, Matrix4x4.identity, this.m_StencilDeferredMaterial, 0, this.m_StencilDeferredPasses[7]);
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x0003A014 File Offset: 0x00038214
		private void RenderFog(ScriptableRenderContext context, CommandBuffer cmd, ref RenderingData renderingData)
		{
			if (!RenderSettings.fog || renderingData.cameraData.camera.orthographic)
			{
				return;
			}
			if (this.m_FullscreenMesh == null)
			{
				this.m_FullscreenMesh = DeferredLights.CreateFullscreenMesh();
			}
			using (new ProfilingScope(cmd, this.m_ProfilingSamplerDeferredFogPass))
			{
				cmd.DrawMesh(this.m_FullscreenMesh, Matrix4x4.identity, this.m_StencilDeferredMaterial, 0, this.m_StencilDeferredPasses[6]);
			}
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x0003A0A4 File Offset: 0x000382A4
		private void InitStencilDeferredMaterial()
		{
			if (this.m_StencilDeferredMaterial == null)
			{
				return;
			}
			for (int i = 0; i < DeferredLights.k_StencilDeferredPassNames.Length; i++)
			{
				this.m_StencilDeferredPasses[i] = this.m_StencilDeferredMaterial.FindPass(DeferredLights.k_StencilDeferredPassNames[i]);
			}
			this.m_StencilDeferredMaterial.SetFloat(DeferredLights.ShaderConstants._StencilRef, 0f);
			this.m_StencilDeferredMaterial.SetFloat(DeferredLights.ShaderConstants._StencilReadMask, 96f);
			this.m_StencilDeferredMaterial.SetFloat(DeferredLights.ShaderConstants._StencilWriteMask, 16f);
			this.m_StencilDeferredMaterial.SetFloat(DeferredLights.ShaderConstants._LitPunctualStencilRef, 48f);
			this.m_StencilDeferredMaterial.SetFloat(DeferredLights.ShaderConstants._LitPunctualStencilReadMask, 112f);
			this.m_StencilDeferredMaterial.SetFloat(DeferredLights.ShaderConstants._LitPunctualStencilWriteMask, 16f);
			this.m_StencilDeferredMaterial.SetFloat(DeferredLights.ShaderConstants._SimpleLitPunctualStencilRef, 80f);
			this.m_StencilDeferredMaterial.SetFloat(DeferredLights.ShaderConstants._SimpleLitPunctualStencilReadMask, 112f);
			this.m_StencilDeferredMaterial.SetFloat(DeferredLights.ShaderConstants._SimpleLitPunctualStencilWriteMask, 16f);
			this.m_StencilDeferredMaterial.SetFloat(DeferredLights.ShaderConstants._LitDirStencilRef, 32f);
			this.m_StencilDeferredMaterial.SetFloat(DeferredLights.ShaderConstants._LitDirStencilReadMask, 96f);
			this.m_StencilDeferredMaterial.SetFloat(DeferredLights.ShaderConstants._LitDirStencilWriteMask, 0f);
			this.m_StencilDeferredMaterial.SetFloat(DeferredLights.ShaderConstants._SimpleLitDirStencilRef, 64f);
			this.m_StencilDeferredMaterial.SetFloat(DeferredLights.ShaderConstants._SimpleLitDirStencilReadMask, 96f);
			this.m_StencilDeferredMaterial.SetFloat(DeferredLights.ShaderConstants._SimpleLitDirStencilWriteMask, 0f);
			this.m_StencilDeferredMaterial.SetFloat(DeferredLights.ShaderConstants._ClearStencilRef, 0f);
			this.m_StencilDeferredMaterial.SetFloat(DeferredLights.ShaderConstants._ClearStencilReadMask, 96f);
			this.m_StencilDeferredMaterial.SetFloat(DeferredLights.ShaderConstants._ClearStencilWriteMask, 96f);
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x0003A268 File Offset: 0x00038468
		private static Mesh CreateSphereMesh()
		{
			Vector3[] array = new Vector3[]
			{
				new Vector3(0f, 0f, -1.07f),
				new Vector3(0.174f, -0.535f, -0.91f),
				new Vector3(-0.455f, -0.331f, -0.91f),
				new Vector3(0.562f, 0f, -0.91f),
				new Vector3(-0.455f, 0.331f, -0.91f),
				new Vector3(0.174f, 0.535f, -0.91f),
				new Vector3(-0.281f, -0.865f, -0.562f),
				new Vector3(0.736f, -0.535f, -0.562f),
				new Vector3(0.296f, -0.91f, -0.468f),
				new Vector3(-0.91f, 0f, -0.562f),
				new Vector3(-0.774f, -0.562f, -0.478f),
				new Vector3(0f, -1.07f, 0f),
				new Vector3(-0.629f, -0.865f, 0f),
				new Vector3(0.629f, -0.865f, 0f),
				new Vector3(-1.017f, -0.331f, 0f),
				new Vector3(0.957f, 0f, -0.478f),
				new Vector3(0.736f, 0.535f, -0.562f),
				new Vector3(1.017f, -0.331f, 0f),
				new Vector3(1.017f, 0.331f, 0f),
				new Vector3(-0.296f, -0.91f, 0.478f),
				new Vector3(0.281f, -0.865f, 0.562f),
				new Vector3(0.774f, -0.562f, 0.478f),
				new Vector3(-0.736f, -0.535f, 0.562f),
				new Vector3(0.91f, 0f, 0.562f),
				new Vector3(0.455f, -0.331f, 0.91f),
				new Vector3(-0.174f, -0.535f, 0.91f),
				new Vector3(0.629f, 0.865f, 0f),
				new Vector3(0.774f, 0.562f, 0.478f),
				new Vector3(0.455f, 0.331f, 0.91f),
				new Vector3(0f, 0f, 1.07f),
				new Vector3(-0.562f, 0f, 0.91f),
				new Vector3(-0.957f, 0f, 0.478f),
				new Vector3(0.281f, 0.865f, 0.562f),
				new Vector3(-0.174f, 0.535f, 0.91f),
				new Vector3(0.296f, 0.91f, -0.478f),
				new Vector3(-1.017f, 0.331f, 0f),
				new Vector3(-0.736f, 0.535f, 0.562f),
				new Vector3(-0.296f, 0.91f, 0.478f),
				new Vector3(0f, 1.07f, 0f),
				new Vector3(-0.281f, 0.865f, -0.562f),
				new Vector3(-0.774f, 0.562f, -0.478f),
				new Vector3(-0.629f, 0.865f, 0f)
			};
			int[] array2 = new int[]
			{
				0, 1, 2, 0, 3, 1, 2, 4, 0, 0,
				5, 3, 0, 4, 5, 1, 6, 2, 3, 7,
				1, 1, 8, 6, 1, 7, 8, 9, 4, 2,
				2, 6, 10, 10, 9, 2, 8, 11, 6, 6,
				12, 10, 11, 12, 6, 7, 13, 8, 8, 13,
				11, 10, 14, 9, 10, 12, 14, 3, 15, 7,
				5, 16, 3, 3, 16, 15, 15, 17, 7, 17,
				13, 7, 16, 18, 15, 15, 18, 17, 11, 19,
				12, 13, 20, 11, 11, 20, 19, 17, 21, 13,
				13, 21, 20, 12, 19, 22, 12, 22, 14, 17,
				23, 21, 18, 23, 17, 21, 24, 20, 23, 24,
				21, 20, 25, 19, 19, 25, 22, 24, 25, 20,
				26, 18, 16, 18, 27, 23, 26, 27, 18, 28,
				24, 23, 27, 28, 23, 24, 29, 25, 28, 29,
				24, 25, 30, 22, 25, 29, 30, 14, 22, 31,
				22, 30, 31, 32, 28, 27, 26, 32, 27, 33,
				29, 28, 30, 29, 33, 33, 28, 32, 34, 26,
				16, 5, 34, 16, 14, 31, 35, 14, 35, 9,
				31, 30, 36, 30, 33, 36, 35, 31, 36, 37,
				33, 32, 36, 33, 37, 38, 32, 26, 34, 38,
				26, 38, 37, 32, 5, 39, 34, 39, 38, 34,
				4, 39, 5, 9, 40, 4, 9, 35, 40, 4,
				40, 39, 35, 36, 41, 41, 36, 37, 41, 37,
				38, 40, 35, 41, 40, 41, 39, 41, 38, 39
			};
			return new Mesh
			{
				indexFormat = IndexFormat.UInt16,
				vertices = array,
				triangles = array2
			};
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x0003A73C File Offset: 0x0003893C
		private static Mesh CreateHemisphereMesh()
		{
			Vector3[] array = new Vector3[]
			{
				new Vector3(0f, 0f, 0f),
				new Vector3(1f, 0f, 0f),
				new Vector3(0.92388f, 0.382683f, 0f),
				new Vector3(0.707107f, 0.707107f, 0f),
				new Vector3(0.382683f, 0.92388f, 0f),
				new Vector3(-0f, 1f, 0f),
				new Vector3(-0.382684f, 0.92388f, 0f),
				new Vector3(-0.707107f, 0.707107f, 0f),
				new Vector3(-0.92388f, 0.382683f, 0f),
				new Vector3(-1f, -0f, 0f),
				new Vector3(-0.92388f, -0.382683f, 0f),
				new Vector3(-0.707107f, -0.707107f, 0f),
				new Vector3(-0.382683f, -0.92388f, 0f),
				new Vector3(0f, -1f, 0f),
				new Vector3(0.382684f, -0.923879f, 0f),
				new Vector3(0.707107f, -0.707107f, 0f),
				new Vector3(0.92388f, -0.382683f, 0f),
				new Vector3(0f, 0f, 1f),
				new Vector3(0.707107f, 0f, 0.707107f),
				new Vector3(0f, -0.707107f, 0.707107f),
				new Vector3(0f, 0.707107f, 0.707107f),
				new Vector3(-0.707107f, 0f, 0.707107f),
				new Vector3(0.816497f, -0.408248f, 0.408248f),
				new Vector3(0.408248f, -0.408248f, 0.816497f),
				new Vector3(0.408248f, -0.816497f, 0.408248f),
				new Vector3(0.408248f, 0.816497f, 0.408248f),
				new Vector3(0.408248f, 0.408248f, 0.816497f),
				new Vector3(0.816497f, 0.408248f, 0.408248f),
				new Vector3(-0.816497f, 0.408248f, 0.408248f),
				new Vector3(-0.408248f, 0.408248f, 0.816497f),
				new Vector3(-0.408248f, 0.816497f, 0.408248f),
				new Vector3(-0.408248f, -0.816497f, 0.408248f),
				new Vector3(-0.408248f, -0.408248f, 0.816497f),
				new Vector3(-0.816497f, -0.408248f, 0.408248f),
				new Vector3(0f, -0.92388f, 0.382683f),
				new Vector3(0.92388f, 0f, 0.382683f),
				new Vector3(0f, -0.382683f, 0.92388f),
				new Vector3(0.382683f, 0f, 0.92388f),
				new Vector3(0f, 0.92388f, 0.382683f),
				new Vector3(0f, 0.382683f, 0.92388f),
				new Vector3(-0.92388f, 0f, 0.382683f),
				new Vector3(-0.382683f, 0f, 0.92388f)
			};
			int[] array2 = new int[]
			{
				0, 2, 1, 0, 3, 2, 0, 4, 3, 0,
				5, 4, 0, 6, 5, 0, 7, 6, 0, 8,
				7, 0, 9, 8, 0, 10, 9, 0, 11, 10,
				0, 12, 11, 0, 13, 12, 0, 14, 13, 0,
				15, 14, 0, 16, 15, 0, 1, 16, 22, 23,
				24, 25, 26, 27, 28, 29, 30, 31, 32, 33,
				14, 24, 34, 35, 22, 16, 36, 23, 37, 2,
				27, 35, 38, 25, 4, 37, 26, 39, 6, 30,
				38, 40, 28, 8, 39, 29, 41, 10, 33, 40,
				34, 31, 12, 41, 32, 36, 15, 22, 24, 18,
				23, 22, 19, 24, 23, 3, 25, 27, 20, 26,
				25, 18, 27, 26, 7, 28, 30, 21, 29, 28,
				20, 30, 29, 11, 31, 33, 19, 32, 31, 21,
				33, 32, 13, 14, 34, 15, 24, 14, 19, 34,
				24, 1, 35, 16, 18, 22, 35, 15, 16, 22,
				17, 36, 37, 19, 23, 36, 18, 37, 23, 1,
				2, 35, 3, 27, 2, 18, 35, 27, 5, 38,
				4, 20, 25, 38, 3, 4, 25, 17, 37, 39,
				18, 26, 37, 20, 39, 26, 5, 6, 38, 7,
				30, 6, 20, 38, 30, 9, 40, 8, 21, 28,
				40, 7, 8, 28, 17, 39, 41, 20, 29, 39,
				21, 41, 29, 9, 10, 40, 11, 33, 10, 21,
				40, 33, 13, 34, 12, 19, 31, 34, 11, 12,
				31, 17, 41, 36, 21, 32, 41, 19, 36, 32
			};
			return new Mesh
			{
				indexFormat = IndexFormat.UInt16,
				vertices = array,
				triangles = array2
			};
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x0003AC10 File Offset: 0x00038E10
		private static Mesh CreateFullscreenMesh()
		{
			Vector3[] array = new Vector3[]
			{
				new Vector3(-1f, 1f, 0f),
				new Vector3(-1f, -3f, 0f),
				new Vector3(3f, 1f, 0f)
			};
			int[] array2 = new int[] { 0, 1, 2 };
			return new Mesh
			{
				indexFormat = IndexFormat.UInt16,
				vertices = array,
				triangles = array2
			};
		}

		// Token: 0x040007F9 RID: 2041
		internal static readonly string[] k_GBufferNames = new string[] { "_GBuffer0", "_GBuffer1", "_GBuffer2", "_GBuffer3", "_GBuffer4", "_GBuffer5", "_GBuffer6" };

		// Token: 0x040007FA RID: 2042
		private static readonly string[] k_StencilDeferredPassNames = new string[] { "Stencil Volume", "Deferred Punctual Light (Lit)", "Deferred Punctual Light (SimpleLit)", "Deferred Directional Light (Lit)", "Deferred Directional Light (SimpleLit)", "ClearStencilPartial", "Fog", "SSAOOnly" };

		// Token: 0x040007FB RID: 2043
		private static readonly ushort k_InvalidLightOffset = ushort.MaxValue;

		// Token: 0x040007FC RID: 2044
		private static readonly string k_SetupLights = "SetupLights";

		// Token: 0x040007FD RID: 2045
		private static readonly string k_DeferredPass = "Deferred Pass";

		// Token: 0x040007FE RID: 2046
		private static readonly string k_DeferredStencilPass = "Deferred Shading (Stencil)";

		// Token: 0x040007FF RID: 2047
		private static readonly string k_DeferredFogPass = "Deferred Fog";

		// Token: 0x04000800 RID: 2048
		private static readonly string k_ClearStencilPartial = "Clear Stencil Partial";

		// Token: 0x04000801 RID: 2049
		private static readonly string k_SetupLightConstants = "Setup Light Constants";

		// Token: 0x04000802 RID: 2050
		private static readonly float kStencilShapeGuard = 1.06067f;

		// Token: 0x04000803 RID: 2051
		private static readonly ProfilingSampler m_ProfilingSetupLights = new ProfilingSampler(DeferredLights.k_SetupLights);

		// Token: 0x04000804 RID: 2052
		private static readonly ProfilingSampler m_ProfilingDeferredPass = new ProfilingSampler(DeferredLights.k_DeferredPass);

		// Token: 0x04000805 RID: 2053
		private static readonly ProfilingSampler m_ProfilingSetupLightConstants = new ProfilingSampler(DeferredLights.k_SetupLightConstants);

		// Token: 0x04000813 RID: 2067
		private RTHandle[] GbufferRTHandles;

		// Token: 0x0400081B RID: 2075
		private NativeArray<ushort> m_stencilVisLights;

		// Token: 0x0400081C RID: 2076
		private NativeArray<ushort> m_stencilVisLightOffsets;

		// Token: 0x0400081D RID: 2077
		private AdditionalLightsShadowCasterPass m_AdditionalLightsShadowCasterPass;

		// Token: 0x0400081E RID: 2078
		private Mesh m_SphereMesh;

		// Token: 0x0400081F RID: 2079
		private Mesh m_HemisphereMesh;

		// Token: 0x04000820 RID: 2080
		private Mesh m_FullscreenMesh;

		// Token: 0x04000821 RID: 2081
		private Material m_StencilDeferredMaterial;

		// Token: 0x04000822 RID: 2082
		private int[] m_StencilDeferredPasses;

		// Token: 0x04000823 RID: 2083
		private Matrix4x4[] m_ScreenToWorld = new Matrix4x4[2];

		// Token: 0x04000824 RID: 2084
		private ProfilingSampler m_ProfilingSamplerDeferredStencilPass = new ProfilingSampler(DeferredLights.k_DeferredStencilPass);

		// Token: 0x04000825 RID: 2085
		private ProfilingSampler m_ProfilingSamplerDeferredFogPass = new ProfilingSampler(DeferredLights.k_DeferredFogPass);

		// Token: 0x04000826 RID: 2086
		private ProfilingSampler m_ProfilingSamplerClearStencilPartialPass = new ProfilingSampler(DeferredLights.k_ClearStencilPartial);

		// Token: 0x04000827 RID: 2087
		private LightCookieManager m_LightCookieManager;

		// Token: 0x020001D6 RID: 470
		internal static class ShaderConstants
		{
			// Token: 0x04000C0E RID: 3086
			public static readonly int _LitStencilRef = Shader.PropertyToID("_LitStencilRef");

			// Token: 0x04000C0F RID: 3087
			public static readonly int _LitStencilReadMask = Shader.PropertyToID("_LitStencilReadMask");

			// Token: 0x04000C10 RID: 3088
			public static readonly int _LitStencilWriteMask = Shader.PropertyToID("_LitStencilWriteMask");

			// Token: 0x04000C11 RID: 3089
			public static readonly int _SimpleLitStencilRef = Shader.PropertyToID("_SimpleLitStencilRef");

			// Token: 0x04000C12 RID: 3090
			public static readonly int _SimpleLitStencilReadMask = Shader.PropertyToID("_SimpleLitStencilReadMask");

			// Token: 0x04000C13 RID: 3091
			public static readonly int _SimpleLitStencilWriteMask = Shader.PropertyToID("_SimpleLitStencilWriteMask");

			// Token: 0x04000C14 RID: 3092
			public static readonly int _StencilRef = Shader.PropertyToID("_StencilRef");

			// Token: 0x04000C15 RID: 3093
			public static readonly int _StencilReadMask = Shader.PropertyToID("_StencilReadMask");

			// Token: 0x04000C16 RID: 3094
			public static readonly int _StencilWriteMask = Shader.PropertyToID("_StencilWriteMask");

			// Token: 0x04000C17 RID: 3095
			public static readonly int _LitPunctualStencilRef = Shader.PropertyToID("_LitPunctualStencilRef");

			// Token: 0x04000C18 RID: 3096
			public static readonly int _LitPunctualStencilReadMask = Shader.PropertyToID("_LitPunctualStencilReadMask");

			// Token: 0x04000C19 RID: 3097
			public static readonly int _LitPunctualStencilWriteMask = Shader.PropertyToID("_LitPunctualStencilWriteMask");

			// Token: 0x04000C1A RID: 3098
			public static readonly int _SimpleLitPunctualStencilRef = Shader.PropertyToID("_SimpleLitPunctualStencilRef");

			// Token: 0x04000C1B RID: 3099
			public static readonly int _SimpleLitPunctualStencilReadMask = Shader.PropertyToID("_SimpleLitPunctualStencilReadMask");

			// Token: 0x04000C1C RID: 3100
			public static readonly int _SimpleLitPunctualStencilWriteMask = Shader.PropertyToID("_SimpleLitPunctualStencilWriteMask");

			// Token: 0x04000C1D RID: 3101
			public static readonly int _LitDirStencilRef = Shader.PropertyToID("_LitDirStencilRef");

			// Token: 0x04000C1E RID: 3102
			public static readonly int _LitDirStencilReadMask = Shader.PropertyToID("_LitDirStencilReadMask");

			// Token: 0x04000C1F RID: 3103
			public static readonly int _LitDirStencilWriteMask = Shader.PropertyToID("_LitDirStencilWriteMask");

			// Token: 0x04000C20 RID: 3104
			public static readonly int _SimpleLitDirStencilRef = Shader.PropertyToID("_SimpleLitDirStencilRef");

			// Token: 0x04000C21 RID: 3105
			public static readonly int _SimpleLitDirStencilReadMask = Shader.PropertyToID("_SimpleLitDirStencilReadMask");

			// Token: 0x04000C22 RID: 3106
			public static readonly int _SimpleLitDirStencilWriteMask = Shader.PropertyToID("_SimpleLitDirStencilWriteMask");

			// Token: 0x04000C23 RID: 3107
			public static readonly int _ClearStencilRef = Shader.PropertyToID("_ClearStencilRef");

			// Token: 0x04000C24 RID: 3108
			public static readonly int _ClearStencilReadMask = Shader.PropertyToID("_ClearStencilReadMask");

			// Token: 0x04000C25 RID: 3109
			public static readonly int _ClearStencilWriteMask = Shader.PropertyToID("_ClearStencilWriteMask");

			// Token: 0x04000C26 RID: 3110
			public static readonly int _ScreenToWorld = Shader.PropertyToID("_ScreenToWorld");

			// Token: 0x04000C27 RID: 3111
			public static int _MainLightPosition = Shader.PropertyToID("_MainLightPosition");

			// Token: 0x04000C28 RID: 3112
			public static int _MainLightColor = Shader.PropertyToID("_MainLightColor");

			// Token: 0x04000C29 RID: 3113
			public static int _MainLightLayerMask = Shader.PropertyToID("_MainLightLayerMask");

			// Token: 0x04000C2A RID: 3114
			public static int _SpotLightScale = Shader.PropertyToID("_SpotLightScale");

			// Token: 0x04000C2B RID: 3115
			public static int _SpotLightBias = Shader.PropertyToID("_SpotLightBias");

			// Token: 0x04000C2C RID: 3116
			public static int _SpotLightGuard = Shader.PropertyToID("_SpotLightGuard");

			// Token: 0x04000C2D RID: 3117
			public static int _LightPosWS = Shader.PropertyToID("_LightPosWS");

			// Token: 0x04000C2E RID: 3118
			public static int _LightColor = Shader.PropertyToID("_LightColor");

			// Token: 0x04000C2F RID: 3119
			public static int _LightAttenuation = Shader.PropertyToID("_LightAttenuation");

			// Token: 0x04000C30 RID: 3120
			public static int _LightOcclusionProbInfo = Shader.PropertyToID("_LightOcclusionProbInfo");

			// Token: 0x04000C31 RID: 3121
			public static int _LightDirection = Shader.PropertyToID("_LightDirection");

			// Token: 0x04000C32 RID: 3122
			public static int _LightFlags = Shader.PropertyToID("_LightFlags");

			// Token: 0x04000C33 RID: 3123
			public static int _ShadowLightIndex = Shader.PropertyToID("_ShadowLightIndex");

			// Token: 0x04000C34 RID: 3124
			public static int _LightLayerMask = Shader.PropertyToID("_LightLayerMask");

			// Token: 0x04000C35 RID: 3125
			public static int _CookieLightIndex = Shader.PropertyToID("_CookieLightIndex");
		}

		// Token: 0x020001D7 RID: 471
		internal enum StencilDeferredPasses
		{
			// Token: 0x04000C37 RID: 3127
			StencilVolume,
			// Token: 0x04000C38 RID: 3128
			PunctualLit,
			// Token: 0x04000C39 RID: 3129
			PunctualSimpleLit,
			// Token: 0x04000C3A RID: 3130
			DirectionalLit,
			// Token: 0x04000C3B RID: 3131
			DirectionalSimpleLit,
			// Token: 0x04000C3C RID: 3132
			ClearStencilPartial,
			// Token: 0x04000C3D RID: 3133
			Fog,
			// Token: 0x04000C3E RID: 3134
			SSAOOnly
		}

		// Token: 0x020001D8 RID: 472
		internal struct InitParams
		{
			// Token: 0x04000C3F RID: 3135
			public Material stencilDeferredMaterial;

			// Token: 0x04000C40 RID: 3136
			public LightCookieManager lightCookieManager;
		}
	}
}
