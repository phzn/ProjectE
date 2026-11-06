using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;

namespace UnityEngine.Rendering.Universal.Internal
{
	// Token: 0x0200011A RID: 282
	public class ForwardLights
	{
		// Token: 0x0600093D RID: 2365 RVA: 0x0003C8E4 File Offset: 0x0003AAE4
		public ForwardLights()
			: this(ForwardLights.InitParams.Create())
		{
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x0003C8F4 File Offset: 0x0003AAF4
		internal ForwardLights(ForwardLights.InitParams initParams)
		{
			this.m_UseStructuredBuffer = RenderingUtils.useStructuredBuffer;
			this.m_UseForwardPlus = initParams.forwardPlus;
			ForwardLights.LightConstantBuffer._MainLightPosition = Shader.PropertyToID("_MainLightPosition");
			ForwardLights.LightConstantBuffer._MainLightColor = Shader.PropertyToID("_MainLightColor");
			ForwardLights.LightConstantBuffer._MainLightOcclusionProbesChannel = Shader.PropertyToID("_MainLightOcclusionProbes");
			ForwardLights.LightConstantBuffer._MainLightLayerMask = Shader.PropertyToID("_MainLightLayerMask");
			ForwardLights.LightConstantBuffer._AdditionalLightsCount = Shader.PropertyToID("_AdditionalLightsCount");
			if (this.m_UseStructuredBuffer)
			{
				this.m_AdditionalLightsBufferId = Shader.PropertyToID("_AdditionalLightsBuffer");
				this.m_AdditionalLightsIndicesId = Shader.PropertyToID("_AdditionalLightsIndices");
			}
			else
			{
				ForwardLights.LightConstantBuffer._AdditionalLightsPosition = Shader.PropertyToID("_AdditionalLightsPosition");
				ForwardLights.LightConstantBuffer._AdditionalLightsColor = Shader.PropertyToID("_AdditionalLightsColor");
				ForwardLights.LightConstantBuffer._AdditionalLightsAttenuation = Shader.PropertyToID("_AdditionalLightsAttenuation");
				ForwardLights.LightConstantBuffer._AdditionalLightsSpotDir = Shader.PropertyToID("_AdditionalLightsSpotDir");
				ForwardLights.LightConstantBuffer._AdditionalLightOcclusionProbeChannel = Shader.PropertyToID("_AdditionalLightsOcclusionProbes");
				ForwardLights.LightConstantBuffer._AdditionalLightsLayerMasks = Shader.PropertyToID("_AdditionalLightsLayerMasks");
				int maxVisibleAdditionalLights = UniversalRenderPipeline.maxVisibleAdditionalLights;
				this.m_AdditionalLightPositions = new Vector4[maxVisibleAdditionalLights];
				this.m_AdditionalLightColors = new Vector4[maxVisibleAdditionalLights];
				this.m_AdditionalLightAttenuations = new Vector4[maxVisibleAdditionalLights];
				this.m_AdditionalLightSpotDirections = new Vector4[maxVisibleAdditionalLights];
				this.m_AdditionalLightOcclusionProbeChannels = new Vector4[maxVisibleAdditionalLights];
				this.m_AdditionalLightsLayerMasks = new float[maxVisibleAdditionalLights];
			}
			if (this.m_UseForwardPlus)
			{
				this.CreateForwardPlusBuffers();
				this.m_ReflectionProbeManager = ReflectionProbeManager.Create();
			}
			this.m_LightCookieManager = initParams.lightCookieManager;
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x0003CA64 File Offset: 0x0003AC64
		private void CreateForwardPlusBuffers()
		{
			this.m_ZBins = new NativeArray<uint>(UniversalRenderPipeline.maxZBinWords, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.m_ZBinsBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Constant, UniversalRenderPipeline.maxZBinWords / 4, UnsafeUtility.SizeOf<float4>());
			this.m_ZBinsBuffer.name = "URP Z-Bin Buffer";
			this.m_TileMasks = new NativeArray<uint>(UniversalRenderPipeline.maxTileWords, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.m_TileMasksBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Constant, UniversalRenderPipeline.maxTileWords / 4, UnsafeUtility.SizeOf<float4>());
			this.m_TileMasksBuffer.name = "URP Tile Buffer";
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000940 RID: 2368 RVA: 0x0003CAED File Offset: 0x0003ACED
		internal ReflectionProbeManager reflectionProbeManager
		{
			get
			{
				return this.m_ReflectionProbeManager;
			}
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x0003CAF5 File Offset: 0x0003ACF5
		private static int AlignByteCount(int count, int align)
		{
			return align * ((count + align - 1) / align);
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x0003CB00 File Offset: 0x0003AD00
		private void GetViewParams(Camera camera, float4x4 viewToClip, out float viewPlaneBot, out float viewPlaneTop, out float4 viewToViewportScaleBias)
		{
			float2 @float = math.float2(viewToClip[0][0], viewToClip[1][1]);
			float2 float2 = math.rcp(@float);
			float2 float3 = (camera.orthographic ? (-math.float2(viewToClip[3][0], viewToClip[3][1])) : math.float2(viewToClip[2][0], viewToClip[2][1]));
			viewPlaneBot = float3.y * float2.y - float2.y;
			viewPlaneTop = float3.y * float2.y + float2.y;
			viewToViewportScaleBias = math.float4(@float * 0.5f, -float3 * 0.5f + 0.5f);
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x0003CBE8 File Offset: 0x0003ADE8
		internal void PreSetup(ref RenderingData renderingData)
		{
			if (this.m_UseForwardPlus)
			{
				using (new ProfilingScope(null, ForwardLights.m_ProfilingSamplerFPSetup))
				{
					if (!this.m_CullingHandle.IsCompleted)
					{
						throw new InvalidOperationException("Forward+ jobs have not completed yet.");
					}
					if (this.m_TileMasks.Length != UniversalRenderPipeline.maxTileWords)
					{
						this.m_ZBins.Dispose();
						this.m_ZBinsBuffer.Dispose();
						this.m_TileMasks.Dispose();
						this.m_TileMasksBuffer.Dispose();
						this.CreateForwardPlusBuffers();
					}
					else
					{
						UnsafeUtility.MemClear(this.m_ZBins.GetUnsafePtr<uint>(), (long)(this.m_ZBins.Length * 4));
						UnsafeUtility.MemClear(this.m_TileMasks.GetUnsafePtr<uint>(), (long)(this.m_TileMasks.Length * 4));
					}
					ref CameraData ptr = ref renderingData.cameraData;
					Camera camera = ptr.camera;
					int2 @int = math.int2(ptr.pixelWidth, ptr.pixelHeight);
					int num = ((ptr.xr.enabled && ptr.xr.singlePassEnabled) ? 2 : 1);
					this.m_LightCount = renderingData.lightData.visibleLights.Length;
					int num2 = 0;
					while (num2 < this.m_LightCount && renderingData.lightData.visibleLights[num2].lightType == LightType.Directional)
					{
						num2++;
					}
					this.m_LightCount -= num2;
					this.m_DirectionalLightCount = num2;
					if (renderingData.lightData.mainLightIndex != -1 && this.m_DirectionalLightCount != 0)
					{
						this.m_DirectionalLightCount--;
					}
					NativeArray<VisibleLight> subArray = renderingData.lightData.visibleLights.GetSubArray(num2, this.m_LightCount);
					NativeArray<VisibleReflectionProbe> visibleReflectionProbes = renderingData.cullResults.visibleReflectionProbes;
					int num3 = math.min(visibleReflectionProbes.Length, UniversalRenderPipeline.maxVisibleReflectionProbes);
					int num4 = subArray.Length + num3;
					this.m_WordsPerTile = (num4 + 31) / 32;
					this.m_ActualTileWidth = 4;
					do
					{
						this.m_ActualTileWidth <<= 1;
						this.m_TileResolution = (@int + this.m_ActualTileWidth - 1) / this.m_ActualTileWidth;
					}
					while (this.m_TileResolution.x * this.m_TileResolution.y * this.m_WordsPerTile * num > UniversalRenderPipeline.maxTileWords);
					if (!camera.orthographic)
					{
						this.m_ZBinScale = (float)(UniversalRenderPipeline.maxZBinWords / num) / ((math.log2(camera.farClipPlane) - math.log2(camera.nearClipPlane)) * (float)(2 + this.m_WordsPerTile));
						this.m_ZBinOffset = -math.log2(camera.nearClipPlane) * this.m_ZBinScale;
						this.m_BinCount = (int)(math.log2(camera.farClipPlane) * this.m_ZBinScale + this.m_ZBinOffset);
					}
					else
					{
						this.m_ZBinScale = (float)(UniversalRenderPipeline.maxZBinWords / num) / ((camera.farClipPlane - camera.nearClipPlane) * (float)(2 + this.m_WordsPerTile));
						this.m_ZBinOffset = -camera.nearClipPlane * this.m_ZBinScale;
						this.m_BinCount = (int)(camera.farClipPlane * this.m_ZBinScale + this.m_ZBinOffset);
					}
					this.m_BinCount = Math.Max(this.m_BinCount, 0);
					Fixed2<float4x4> @fixed = new Fixed2<float4x4>(ptr.GetViewMatrix(0), ptr.GetViewMatrix(math.min(1, num - 1)));
					Fixed2<float4x4> fixed2 = new Fixed2<float4x4>(ptr.GetProjectionMatrix(0), ptr.GetProjectionMatrix(math.min(1, num - 1)));
					for (int i = 1; i < num3; i++)
					{
						VisibleReflectionProbe visibleReflectionProbe = visibleReflectionProbes[i];
						int num5 = i - 1;
						while (num5 >= 0 && ForwardLights.<PreSetup>g__IsProbeGreater|40_0(visibleReflectionProbes[num5], visibleReflectionProbe))
						{
							visibleReflectionProbes[num5 + 1] = visibleReflectionProbes[num5];
							num5--;
						}
						visibleReflectionProbes[num5 + 1] = visibleReflectionProbe;
					}
					NativeArray<float2> nativeArray = new NativeArray<float2>(num4 * num, Allocator.TempJob, NativeArrayOptions.ClearMemory);
					JobHandle jobHandle = new LightMinMaxZJob
					{
						worldToViews = @fixed,
						lights = subArray,
						minMaxZs = nativeArray.GetSubArray(0, this.m_LightCount * num)
					}.ScheduleParallel(this.m_LightCount * num, 32, default(JobHandle));
					JobHandle jobHandle2 = new ReflectionProbeMinMaxZJob
					{
						worldToViews = @fixed,
						reflectionProbes = visibleReflectionProbes,
						minMaxZs = nativeArray.GetSubArray(this.m_LightCount * num, num3 * num)
					}.ScheduleParallel(num3 * num, 32, jobHandle);
					int num6 = (this.m_BinCount + 128 - 1) / 128;
					JobHandle jobHandle3 = new ZBinningJob
					{
						bins = this.m_ZBins,
						minMaxZs = nativeArray,
						zBinScale = this.m_ZBinScale,
						zBinOffset = this.m_ZBinOffset,
						binCount = this.m_BinCount,
						wordsPerTile = this.m_WordsPerTile,
						lightCount = this.m_LightCount,
						reflectionProbeCount = num3,
						batchCount = num6,
						viewCount = num,
						isOrthographic = camera.orthographic
					}.ScheduleParallel(num6 * num, 1, jobHandle2);
					jobHandle2.Complete();
					float num7;
					float num8;
					float4 @float;
					this.GetViewParams(camera, fixed2[0], out num7, out num8, out @float);
					float num9;
					float num10;
					float4 float2;
					this.GetViewParams(camera, fixed2[1], out num9, out num10, out float2);
					int num11 = ForwardLights.AlignByteCount((1 + this.m_TileResolution.y) * UnsafeUtility.SizeOf<InclusiveRange>(), 128) / UnsafeUtility.SizeOf<InclusiveRange>();
					NativeArray<InclusiveRange> nativeArray2 = new NativeArray<InclusiveRange>(num11 * num4 * num, Allocator.TempJob, NativeArrayOptions.ClearMemory);
					JobHandle jobHandle4 = new TilingJob
					{
						lights = subArray,
						reflectionProbes = visibleReflectionProbes,
						tileRanges = nativeArray2,
						itemsPerTile = num4,
						rangesPerItem = num11,
						worldToViews = @fixed,
						tileScale = @int / (float)this.m_ActualTileWidth,
						tileScaleInv = (float)this.m_ActualTileWidth / @int,
						viewPlaneBottoms = new Fixed2<float>(num7, num9),
						viewPlaneTops = new Fixed2<float>(num8, num10),
						viewToViewportScaleBiases = new Fixed2<float4>(@float, float2),
						tileCount = this.m_TileResolution,
						near = camera.nearClipPlane,
						isOrthographic = camera.orthographic
					}.ScheduleParallel(num4 * num, 1, jobHandle2);
					JobHandle jobHandle5 = new TileRangeExpansionJob
					{
						tileRanges = nativeArray2,
						tileMasks = this.m_TileMasks,
						rangesPerItem = num11,
						itemsPerTile = num4,
						wordsPerTile = this.m_WordsPerTile,
						tileResolution = this.m_TileResolution
					}.ScheduleParallel(this.m_TileResolution.y * num, 1, jobHandle4);
					this.m_CullingHandle = JobHandle.CombineDependencies(nativeArray.Dispose(jobHandle3), nativeArray2.Dispose(jobHandle5));
					JobHandle.ScheduleBatchedJobs();
				}
			}
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x0003D2F4 File Offset: 0x0003B4F4
		public void Setup(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			int additionalLightsCount = renderingData.lightData.additionalLightsCount;
			bool shadeAdditionalLightsPerVertex = renderingData.lightData.shadeAdditionalLightsPerVertex;
			CommandBuffer commandBuffer = renderingData.commandBuffer;
			using (new ProfilingScope(null, ForwardLights.m_ProfilingSampler))
			{
				if (this.m_UseForwardPlus)
				{
					this.m_ReflectionProbeManager.UpdateGpuData(commandBuffer, ref renderingData);
					using (new ProfilingScope(null, ForwardLights.m_ProfilingSamplerFPComplete))
					{
						this.m_CullingHandle.Complete();
					}
					using (new ProfilingScope(null, ForwardLights.m_ProfilingSamplerFPUpload))
					{
						this.m_ZBinsBuffer.SetData<float4>(this.m_ZBins.Reinterpret<float4>(UnsafeUtility.SizeOf<uint>()));
						this.m_TileMasksBuffer.SetData<float4>(this.m_TileMasks.Reinterpret<float4>(UnsafeUtility.SizeOf<uint>()));
						commandBuffer.SetGlobalConstantBuffer(this.m_ZBinsBuffer, "URP_ZBinBuffer", 0, UniversalRenderPipeline.maxZBinWords * 4);
						commandBuffer.SetGlobalConstantBuffer(this.m_TileMasksBuffer, "urp_TileBuffer", 0, UniversalRenderPipeline.maxTileWords * 4);
					}
					commandBuffer.SetGlobalVector("_FPParams0", math.float4(this.m_ZBinScale, this.m_ZBinOffset, (float)this.m_LightCount, (float)this.m_DirectionalLightCount));
					commandBuffer.SetGlobalVector("_FPParams1", math.float4(renderingData.cameraData.pixelRect.size / (float)this.m_ActualTileWidth, (float)this.m_TileResolution.x, (float)this.m_WordsPerTile));
					commandBuffer.SetGlobalVector("_FPParams2", math.float4((float)this.m_BinCount, (float)(this.m_TileResolution.x * this.m_TileResolution.y), 0f, 0f));
				}
				this.SetupShaderLightConstants(commandBuffer, ref renderingData);
				bool flag = (renderingData.cameraData.renderer.stripAdditionalLightOffVariants && renderingData.lightData.supportsAdditionalLights) || additionalLightsCount > 0;
				CoreUtils.SetKeyword(commandBuffer, "_ADDITIONAL_LIGHTS_VERTEX", flag && shadeAdditionalLightsPerVertex && !this.m_UseForwardPlus);
				CoreUtils.SetKeyword(commandBuffer, "_ADDITIONAL_LIGHTS", flag && !shadeAdditionalLightsPerVertex && !this.m_UseForwardPlus);
				CoreUtils.SetKeyword(commandBuffer, "_FORWARD_PLUS", this.m_UseForwardPlus);
				bool flag2 = renderingData.lightData.supportsMixedLighting && this.m_MixedLightingSetup == MixedLightingSetup.ShadowMask;
				bool flag3 = flag2 && QualitySettings.shadowmaskMode == ShadowmaskMode.Shadowmask;
				bool flag4 = renderingData.lightData.supportsMixedLighting && this.m_MixedLightingSetup == MixedLightingSetup.Subtractive;
				CoreUtils.SetKeyword(commandBuffer, "LIGHTMAP_SHADOW_MIXING", flag4 || flag3);
				CoreUtils.SetKeyword(commandBuffer, "SHADOWS_SHADOWMASK", flag2);
				CoreUtils.SetKeyword(commandBuffer, "_MIXED_LIGHTING_SUBTRACTIVE", flag4);
				CoreUtils.SetKeyword(commandBuffer, "_REFLECTION_PROBE_BLENDING", renderingData.lightData.reflectionProbeBlending);
				CoreUtils.SetKeyword(commandBuffer, "_REFLECTION_PROBE_BOX_PROJECTION", renderingData.lightData.reflectionProbeBoxProjection);
				ShEvalMode shEvalMode = PlatformAutoDetect.ShAutoDetect(UniversalRenderPipeline.asset.shEvalMode);
				CoreUtils.SetKeyword(commandBuffer, "EVALUATE_SH_MIXED", shEvalMode == ShEvalMode.Mixed);
				CoreUtils.SetKeyword(commandBuffer, "EVALUATE_SH_VERTEX", shEvalMode == ShEvalMode.PerVertex);
				bool supportsLightLayers = renderingData.lightData.supportsLightLayers;
				CoreUtils.SetKeyword(commandBuffer, "_LIGHT_LAYERS", supportsLightLayers && !CoreUtils.IsSceneLightingDisabled(renderingData.cameraData.camera));
				if (this.m_LightCookieManager != null)
				{
					this.m_LightCookieManager.Setup(context, commandBuffer, ref renderingData.lightData);
				}
				else
				{
					CoreUtils.SetKeyword(commandBuffer, "_LIGHT_COOKIES", false);
				}
			}
			context.ExecuteCommandBuffer(commandBuffer);
			commandBuffer.Clear();
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x0003D6B8 File Offset: 0x0003B8B8
		internal void Cleanup()
		{
			if (this.m_UseForwardPlus)
			{
				this.m_CullingHandle.Complete();
				this.m_ZBins.Dispose();
				this.m_TileMasks.Dispose();
				this.m_ZBinsBuffer.Dispose();
				this.m_ZBinsBuffer = null;
				this.m_TileMasksBuffer.Dispose();
				this.m_TileMasksBuffer = null;
				this.m_ReflectionProbeManager.Dispose();
			}
			LightCookieManager lightCookieManager = this.m_LightCookieManager;
			if (lightCookieManager != null)
			{
				lightCookieManager.Dispose();
			}
			this.m_LightCookieManager = null;
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x0003D738 File Offset: 0x0003B938
		private void InitializeLightConstants(NativeArray<VisibleLight> lights, int lightIndex, out Vector4 lightPos, out Vector4 lightColor, out Vector4 lightAttenuation, out Vector4 lightSpotDir, out Vector4 lightOcclusionProbeChannel, out uint lightLayerMask, out bool isSubtractive)
		{
			UniversalRenderPipeline.InitializeLightConstants_Common(lights, lightIndex, out lightPos, out lightColor, out lightAttenuation, out lightSpotDir, out lightOcclusionProbeChannel);
			lightLayerMask = 0U;
			isSubtractive = false;
			if (lightIndex < 0)
			{
				return;
			}
			ref VisibleLight ptr = ref lights.UnsafeElementAtMutable<VisibleLight>(lightIndex);
			Light light = ptr.light;
			LightBakingOutput bakingOutput = light.bakingOutput;
			isSubtractive = bakingOutput.isBaked && bakingOutput.lightmapBakeType == LightmapBakeType.Mixed && bakingOutput.mixedLightingMode == MixedLightingMode.Subtractive;
			if (light == null)
			{
				return;
			}
			if (bakingOutput.lightmapBakeType == LightmapBakeType.Mixed && ptr.light.shadows != LightShadows.None && this.m_MixedLightingSetup == MixedLightingSetup.None)
			{
				MixedLightingMode mixedLightingMode = bakingOutput.mixedLightingMode;
				if (mixedLightingMode != MixedLightingMode.Subtractive)
				{
					if (mixedLightingMode == MixedLightingMode.Shadowmask)
					{
						this.m_MixedLightingSetup = MixedLightingSetup.ShadowMask;
					}
				}
				else
				{
					this.m_MixedLightingSetup = MixedLightingSetup.Subtractive;
				}
			}
			UniversalAdditionalLightData universalAdditionalLightData = light.GetUniversalAdditionalLightData();
			lightLayerMask = RenderingLayerUtils.ToValidRenderingLayers(universalAdditionalLightData.renderingLayers);
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x0003D7F9 File Offset: 0x0003B9F9
		private void SetupShaderLightConstants(CommandBuffer cmd, ref RenderingData renderingData)
		{
			this.m_MixedLightingSetup = MixedLightingSetup.None;
			this.SetupMainLightConstants(cmd, ref renderingData.lightData);
			this.SetupAdditionalLightConstants(cmd, ref renderingData);
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x0003D818 File Offset: 0x0003BA18
		private void SetupMainLightConstants(CommandBuffer cmd, ref LightData lightData)
		{
			Vector4 vector;
			Vector4 vector2;
			Vector4 vector3;
			Vector4 vector4;
			Vector4 vector5;
			uint num;
			bool flag;
			this.InitializeLightConstants(lightData.visibleLights, lightData.mainLightIndex, out vector, out vector2, out vector3, out vector4, out vector5, out num, out flag);
			vector2.w = (flag ? 0f : 1f);
			cmd.SetGlobalVector(ForwardLights.LightConstantBuffer._MainLightPosition, vector);
			cmd.SetGlobalVector(ForwardLights.LightConstantBuffer._MainLightColor, vector2);
			cmd.SetGlobalVector(ForwardLights.LightConstantBuffer._MainLightOcclusionProbesChannel, vector5);
			cmd.SetGlobalInt(ForwardLights.LightConstantBuffer._MainLightLayerMask, (int)num);
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x0003D890 File Offset: 0x0003BA90
		private void SetupAdditionalLightConstants(CommandBuffer cmd, ref RenderingData renderingData)
		{
			ref LightData ptr = ref renderingData.lightData;
			CullingResults cullResults = renderingData.cullResults;
			NativeArray<VisibleLight> visibleLights = ptr.visibleLights;
			int maxVisibleAdditionalLights = UniversalRenderPipeline.maxVisibleAdditionalLights;
			int num = this.SetupPerObjectLightIndices(cullResults, ref ptr);
			if (num > 0)
			{
				if (this.m_UseStructuredBuffer)
				{
					NativeArray<ShaderInput.LightData> nativeArray = new NativeArray<ShaderInput.LightData>(num, Allocator.Temp, NativeArrayOptions.ClearMemory);
					int num2 = 0;
					int num3 = 0;
					while (num2 < visibleLights.Length && num3 < maxVisibleAdditionalLights)
					{
						VisibleLight visibleLight = visibleLights[num2];
						if (ptr.mainLightIndex != num2)
						{
							ShaderInput.LightData lightData;
							bool flag;
							this.InitializeLightConstants(visibleLights, num2, out lightData.position, out lightData.color, out lightData.attenuation, out lightData.spotDirection, out lightData.occlusionProbeChannels, out lightData.layerMask, out flag);
							nativeArray[num3] = lightData;
							num3++;
						}
						num2++;
					}
					ComputeBuffer lightDataBuffer = ShaderData.instance.GetLightDataBuffer(num);
					lightDataBuffer.SetData<ShaderInput.LightData>(nativeArray);
					int lightAndReflectionProbeIndexCount = cullResults.lightAndReflectionProbeIndexCount;
					ComputeBuffer lightIndicesBuffer = ShaderData.instance.GetLightIndicesBuffer(lightAndReflectionProbeIndexCount);
					cmd.SetGlobalBuffer(this.m_AdditionalLightsBufferId, lightDataBuffer);
					cmd.SetGlobalBuffer(this.m_AdditionalLightsIndicesId, lightIndicesBuffer);
					nativeArray.Dispose();
				}
				else
				{
					int num4 = 0;
					int num5 = 0;
					while (num4 < visibleLights.Length && num5 < maxVisibleAdditionalLights)
					{
						if (ptr.mainLightIndex != num4)
						{
							uint num6;
							bool flag2;
							this.InitializeLightConstants(visibleLights, num4, out this.m_AdditionalLightPositions[num5], out this.m_AdditionalLightColors[num5], out this.m_AdditionalLightAttenuations[num5], out this.m_AdditionalLightSpotDirections[num5], out this.m_AdditionalLightOcclusionProbeChannels[num5], out num6, out flag2);
							this.m_AdditionalLightsLayerMasks[num5] = math.asfloat(num6);
							this.m_AdditionalLightColors[num5].w = (flag2 ? 1f : 0f);
							num5++;
						}
						num4++;
					}
					cmd.SetGlobalVectorArray(ForwardLights.LightConstantBuffer._AdditionalLightsPosition, this.m_AdditionalLightPositions);
					cmd.SetGlobalVectorArray(ForwardLights.LightConstantBuffer._AdditionalLightsColor, this.m_AdditionalLightColors);
					cmd.SetGlobalVectorArray(ForwardLights.LightConstantBuffer._AdditionalLightsAttenuation, this.m_AdditionalLightAttenuations);
					cmd.SetGlobalVectorArray(ForwardLights.LightConstantBuffer._AdditionalLightsSpotDir, this.m_AdditionalLightSpotDirections);
					cmd.SetGlobalVectorArray(ForwardLights.LightConstantBuffer._AdditionalLightOcclusionProbeChannel, this.m_AdditionalLightOcclusionProbeChannels);
					cmd.SetGlobalFloatArray(ForwardLights.LightConstantBuffer._AdditionalLightsLayerMasks, this.m_AdditionalLightsLayerMasks);
				}
				cmd.SetGlobalVector(ForwardLights.LightConstantBuffer._AdditionalLightsCount, new Vector4((float)ptr.maxPerObjectAdditionalLightsCount, 0f, 0f, 0f));
				return;
			}
			cmd.SetGlobalVector(ForwardLights.LightConstantBuffer._AdditionalLightsCount, Vector4.zero);
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x0003DB04 File Offset: 0x0003BD04
		private int SetupPerObjectLightIndices(CullingResults cullResults, ref LightData lightData)
		{
			if (lightData.additionalLightsCount == 0 || this.m_UseForwardPlus)
			{
				return lightData.additionalLightsCount;
			}
			NativeArray<int> lightIndexMap = cullResults.GetLightIndexMap(Allocator.Temp);
			int num = 0;
			int num2 = 0;
			int maxVisibleAdditionalLights = UniversalRenderPipeline.maxVisibleAdditionalLights;
			int length = lightData.visibleLights.Length;
			int num3 = 0;
			while (num3 < length && num2 < maxVisibleAdditionalLights)
			{
				if (num3 == lightData.mainLightIndex)
				{
					lightIndexMap[num3] = -1;
					num++;
				}
				else
				{
					ref NativeArray<int> ptr = ref lightIndexMap;
					int num4 = num3;
					ptr[num4] -= num;
					num2++;
				}
				num3++;
			}
			for (int i = num + num2; i < lightIndexMap.Length; i++)
			{
				lightIndexMap[i] = -1;
			}
			cullResults.SetLightIndexMap(lightIndexMap);
			if (this.m_UseStructuredBuffer && num2 > 0)
			{
				int lightAndReflectionProbeIndexCount = cullResults.lightAndReflectionProbeIndexCount;
				cullResults.FillLightAndReflectionProbeIndices(ShaderData.instance.GetLightIndicesBuffer(lightAndReflectionProbeIndexCount));
			}
			lightIndexMap.Dispose();
			return num2;
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x0003DC30 File Offset: 0x0003BE30
		[CompilerGenerated]
		internal static bool <PreSetup>g__IsProbeGreater|40_0(VisibleReflectionProbe probe, VisibleReflectionProbe otherProbe)
		{
			return probe.importance < otherProbe.importance || (probe.importance == otherProbe.importance && probe.bounds.extents.sqrMagnitude > otherProbe.bounds.extents.sqrMagnitude);
		}

		// Token: 0x0400084D RID: 2125
		private int m_AdditionalLightsBufferId;

		// Token: 0x0400084E RID: 2126
		private int m_AdditionalLightsIndicesId;

		// Token: 0x0400084F RID: 2127
		private const string k_SetupLightConstants = "Setup Light Constants";

		// Token: 0x04000850 RID: 2128
		private static readonly ProfilingSampler m_ProfilingSampler = new ProfilingSampler("Setup Light Constants");

		// Token: 0x04000851 RID: 2129
		private static readonly ProfilingSampler m_ProfilingSamplerFPSetup = new ProfilingSampler("Forward+ Setup");

		// Token: 0x04000852 RID: 2130
		private static readonly ProfilingSampler m_ProfilingSamplerFPComplete = new ProfilingSampler("Forward+ Complete");

		// Token: 0x04000853 RID: 2131
		private static readonly ProfilingSampler m_ProfilingSamplerFPUpload = new ProfilingSampler("Forward+ Upload");

		// Token: 0x04000854 RID: 2132
		private MixedLightingSetup m_MixedLightingSetup;

		// Token: 0x04000855 RID: 2133
		private Vector4[] m_AdditionalLightPositions;

		// Token: 0x04000856 RID: 2134
		private Vector4[] m_AdditionalLightColors;

		// Token: 0x04000857 RID: 2135
		private Vector4[] m_AdditionalLightAttenuations;

		// Token: 0x04000858 RID: 2136
		private Vector4[] m_AdditionalLightSpotDirections;

		// Token: 0x04000859 RID: 2137
		private Vector4[] m_AdditionalLightOcclusionProbeChannels;

		// Token: 0x0400085A RID: 2138
		private float[] m_AdditionalLightsLayerMasks;

		// Token: 0x0400085B RID: 2139
		private bool m_UseStructuredBuffer;

		// Token: 0x0400085C RID: 2140
		private bool m_UseForwardPlus;

		// Token: 0x0400085D RID: 2141
		private int m_DirectionalLightCount;

		// Token: 0x0400085E RID: 2142
		private int m_ActualTileWidth;

		// Token: 0x0400085F RID: 2143
		private int2 m_TileResolution;

		// Token: 0x04000860 RID: 2144
		private JobHandle m_CullingHandle;

		// Token: 0x04000861 RID: 2145
		private NativeArray<uint> m_ZBins;

		// Token: 0x04000862 RID: 2146
		private GraphicsBuffer m_ZBinsBuffer;

		// Token: 0x04000863 RID: 2147
		private NativeArray<uint> m_TileMasks;

		// Token: 0x04000864 RID: 2148
		private GraphicsBuffer m_TileMasksBuffer;

		// Token: 0x04000865 RID: 2149
		private LightCookieManager m_LightCookieManager;

		// Token: 0x04000866 RID: 2150
		private ReflectionProbeManager m_ReflectionProbeManager;

		// Token: 0x04000867 RID: 2151
		private int m_WordsPerTile;

		// Token: 0x04000868 RID: 2152
		private float m_ZBinScale;

		// Token: 0x04000869 RID: 2153
		private float m_ZBinOffset;

		// Token: 0x0400086A RID: 2154
		private int m_LightCount;

		// Token: 0x0400086B RID: 2155
		private int m_BinCount;

		// Token: 0x020001DD RID: 477
		private static class LightConstantBuffer
		{
			// Token: 0x04000C58 RID: 3160
			public static int _MainLightPosition;

			// Token: 0x04000C59 RID: 3161
			public static int _MainLightColor;

			// Token: 0x04000C5A RID: 3162
			public static int _MainLightOcclusionProbesChannel;

			// Token: 0x04000C5B RID: 3163
			public static int _MainLightLayerMask;

			// Token: 0x04000C5C RID: 3164
			public static int _AdditionalLightsCount;

			// Token: 0x04000C5D RID: 3165
			public static int _AdditionalLightsPosition;

			// Token: 0x04000C5E RID: 3166
			public static int _AdditionalLightsColor;

			// Token: 0x04000C5F RID: 3167
			public static int _AdditionalLightsAttenuation;

			// Token: 0x04000C60 RID: 3168
			public static int _AdditionalLightsSpotDir;

			// Token: 0x04000C61 RID: 3169
			public static int _AdditionalLightOcclusionProbeChannel;

			// Token: 0x04000C62 RID: 3170
			public static int _AdditionalLightsLayerMasks;
		}

		// Token: 0x020001DE RID: 478
		internal struct InitParams
		{
			// Token: 0x06000B80 RID: 2944 RVA: 0x0004C298 File Offset: 0x0004A498
			internal static ForwardLights.InitParams Create()
			{
				LightCookieManager.Settings settings = LightCookieManager.Settings.Create();
				UniversalRenderPipelineAsset asset = UniversalRenderPipeline.asset;
				if (asset)
				{
					settings.atlas.format = asset.additionalLightsCookieFormat;
					settings.atlas.resolution = asset.additionalLightsCookieResolution;
				}
				ForwardLights.InitParams initParams;
				initParams.lightCookieManager = new LightCookieManager(ref settings);
				initParams.forwardPlus = false;
				return initParams;
			}

			// Token: 0x04000C63 RID: 3171
			public LightCookieManager lightCookieManager;

			// Token: 0x04000C64 RID: 3172
			public bool forwardPlus;
		}
	}
}
