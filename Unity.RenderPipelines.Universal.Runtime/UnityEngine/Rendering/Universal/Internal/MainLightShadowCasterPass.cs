using System;
using UnityEngine.Experimental.Rendering.RenderGraphModule;

namespace UnityEngine.Rendering.Universal.Internal
{
	// Token: 0x02000126 RID: 294
	public class MainLightShadowCasterPass : ScriptableRenderPass
	{
		// Token: 0x060009B7 RID: 2487 RVA: 0x000411B0 File Offset: 0x0003F3B0
		public MainLightShadowCasterPass(RenderPassEvent evt)
		{
			base.profilingSampler = new ProfilingSampler("MainLightShadowCasterPass");
			base.renderPassEvent = evt;
			this.m_MainLightShadowMatrices = new Matrix4x4[5];
			this.m_CascadeSlices = new ShadowSliceData[4];
			this.m_CascadeSplitDistances = new Vector4[4];
			MainLightShadowCasterPass.MainLightShadowConstantBuffer._WorldToShadow = Shader.PropertyToID("_MainLightWorldToShadow");
			MainLightShadowCasterPass.MainLightShadowConstantBuffer._ShadowParams = Shader.PropertyToID("_MainLightShadowParams");
			MainLightShadowCasterPass.MainLightShadowConstantBuffer._CascadeShadowSplitSpheres0 = Shader.PropertyToID("_CascadeShadowSplitSpheres0");
			MainLightShadowCasterPass.MainLightShadowConstantBuffer._CascadeShadowSplitSpheres1 = Shader.PropertyToID("_CascadeShadowSplitSpheres1");
			MainLightShadowCasterPass.MainLightShadowConstantBuffer._CascadeShadowSplitSpheres2 = Shader.PropertyToID("_CascadeShadowSplitSpheres2");
			MainLightShadowCasterPass.MainLightShadowConstantBuffer._CascadeShadowSplitSpheres3 = Shader.PropertyToID("_CascadeShadowSplitSpheres3");
			MainLightShadowCasterPass.MainLightShadowConstantBuffer._CascadeShadowSplitSphereRadii = Shader.PropertyToID("_CascadeShadowSplitSphereRadii");
			MainLightShadowCasterPass.MainLightShadowConstantBuffer._ShadowOffset0 = Shader.PropertyToID("_MainLightShadowOffset0");
			MainLightShadowCasterPass.MainLightShadowConstantBuffer._ShadowOffset1 = Shader.PropertyToID("_MainLightShadowOffset1");
			MainLightShadowCasterPass.MainLightShadowConstantBuffer._ShadowmapSize = Shader.PropertyToID("_MainLightShadowmapSize");
			this.m_MainLightShadowmapID = Shader.PropertyToID("_MainLightShadowmapTexture");
			this.m_EmptyMainLightShadowmapTexture = RTHandles.Alloc(Texture2D.blackTexture);
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x000412C4 File Offset: 0x0003F4C4
		public void Dispose()
		{
			RTHandle mainLightShadowmapTexture = this.m_MainLightShadowmapTexture;
			if (mainLightShadowmapTexture != null)
			{
				mainLightShadowmapTexture.Release();
			}
			RTHandle emptyMainLightShadowmapTexture = this.m_EmptyMainLightShadowmapTexture;
			if (emptyMainLightShadowmapTexture == null)
			{
				return;
			}
			emptyMainLightShadowmapTexture.Release();
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x000412E8 File Offset: 0x0003F4E8
		public bool Setup(ref RenderingData renderingData)
		{
			if (!renderingData.shadowData.mainLightShadowsEnabled)
			{
				return false;
			}
			bool flag;
			using (new ProfilingScope(null, this.m_ProfilingSetupSampler))
			{
				if (!renderingData.shadowData.supportsMainLightShadows)
				{
					flag = this.SetupForEmptyRendering(ref renderingData);
				}
				else
				{
					this.Clear();
					int mainLightIndex = renderingData.lightData.mainLightIndex;
					if (mainLightIndex == -1)
					{
						flag = this.SetupForEmptyRendering(ref renderingData);
					}
					else
					{
						VisibleLight visibleLight = renderingData.lightData.visibleLights[mainLightIndex];
						Light light = visibleLight.light;
						if (light.shadows == LightShadows.None)
						{
							flag = this.SetupForEmptyRendering(ref renderingData);
						}
						else
						{
							if (visibleLight.lightType != LightType.Directional)
							{
								Debug.LogWarning("Only directional lights are supported as main light.");
							}
							Bounds bounds;
							if (!renderingData.cullResults.GetShadowCasterBounds(mainLightIndex, out bounds))
							{
								flag = this.SetupForEmptyRendering(ref renderingData);
							}
							else
							{
								this.m_ShadowCasterCascadesCount = renderingData.shadowData.mainLightShadowCascadesCount;
								int maxTileResolutionInAtlas = ShadowUtils.GetMaxTileResolutionInAtlas(renderingData.shadowData.mainLightShadowmapWidth, renderingData.shadowData.mainLightShadowmapHeight, this.m_ShadowCasterCascadesCount);
								this.renderTargetWidth = renderingData.shadowData.mainLightShadowmapWidth;
								this.renderTargetHeight = ((this.m_ShadowCasterCascadesCount == 2) ? (renderingData.shadowData.mainLightShadowmapHeight >> 1) : renderingData.shadowData.mainLightShadowmapHeight);
								for (int i = 0; i < this.m_ShadowCasterCascadesCount; i++)
								{
									if (!ShadowUtils.ExtractDirectionalLightMatrix(ref renderingData.cullResults, ref renderingData.shadowData, mainLightIndex, i, this.renderTargetWidth, this.renderTargetHeight, maxTileResolutionInAtlas, light.shadowNearPlane, out this.m_CascadeSplitDistances[i], out this.m_CascadeSlices[i]))
									{
										return this.SetupForEmptyRendering(ref renderingData);
									}
								}
								this.m_MaxShadowDistanceSq = renderingData.cameraData.maxShadowDistance * renderingData.cameraData.maxShadowDistance;
								this.m_CascadeBorder = renderingData.shadowData.mainLightShadowCascadeBorder;
								this.m_CreateEmptyShadowmap = false;
								base.useNativeRenderPass = true;
								ShadowUtils.ShadowRTReAllocateIfNeeded(ref this.m_MainLightShadowmapTexture, this.renderTargetWidth, this.renderTargetHeight, 16, 1, 0f, "_MainLightShadowmapTexture");
								flag = true;
							}
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x0004151C File Offset: 0x0003F71C
		private bool SetupForEmptyRendering(ref RenderingData renderingData)
		{
			if (!renderingData.cameraData.renderer.stripShadowsOffVariants)
			{
				return false;
			}
			this.m_CreateEmptyShadowmap = true;
			base.useNativeRenderPass = false;
			ShadowUtils.ShadowRTReAllocateIfNeeded(ref this.m_EmptyMainLightShadowmapTexture, 1, 1, 16, 1, 0f, "_EmptyMainLightShadowmapTexture");
			return true;
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x0004155C File Offset: 0x0003F75C
		public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
		{
			if (this.m_CreateEmptyShadowmap)
			{
				base.ResetTarget();
				return;
			}
			base.ConfigureTarget(this.m_MainLightShadowmapTexture);
			base.ConfigureClear(ClearFlag.All, Color.black);
		}

		// Token: 0x060009BC RID: 2492 RVA: 0x00041588 File Offset: 0x0003F788
		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			if (this.m_CreateEmptyShadowmap)
			{
				this.SetEmptyMainLightCascadeShadowmap(ref context, ref renderingData);
				renderingData.commandBuffer.SetGlobalTexture(this.m_MainLightShadowmapID, this.m_EmptyMainLightShadowmapTexture.nameID);
				return;
			}
			this.RenderMainLightCascadeShadowmap(ref context, ref renderingData);
			renderingData.commandBuffer.SetGlobalTexture(this.m_MainLightShadowmapID, this.m_MainLightShadowmapTexture.nameID);
		}

		// Token: 0x060009BD RID: 2493 RVA: 0x000415E8 File Offset: 0x0003F7E8
		private void Clear()
		{
			for (int i = 0; i < this.m_MainLightShadowMatrices.Length; i++)
			{
				this.m_MainLightShadowMatrices[i] = Matrix4x4.identity;
			}
			for (int j = 0; j < this.m_CascadeSplitDistances.Length; j++)
			{
				this.m_CascadeSplitDistances[j] = new Vector4(0f, 0f, 0f, 0f);
			}
			for (int k = 0; k < this.m_CascadeSlices.Length; k++)
			{
				this.m_CascadeSlices[k].Clear();
			}
		}

		// Token: 0x060009BE RID: 2494 RVA: 0x00041678 File Offset: 0x0003F878
		private void SetEmptyMainLightCascadeShadowmap(ref ScriptableRenderContext context, ref RenderingData renderingData)
		{
			CommandBuffer commandBuffer = renderingData.commandBuffer;
			CoreUtils.SetKeyword(commandBuffer, "_MAIN_LIGHT_SHADOWS", true);
			MainLightShadowCasterPass.SetEmptyMainLightShadowParams(commandBuffer);
			context.ExecuteCommandBuffer(commandBuffer);
			commandBuffer.Clear();
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x000416AB File Offset: 0x0003F8AB
		internal static void SetEmptyMainLightShadowParams(CommandBuffer cmd)
		{
			cmd.SetGlobalVector(MainLightShadowCasterPass.MainLightShadowConstantBuffer._ShadowParams, MainLightShadowCasterPass.s_EmptyShadowParams);
			cmd.SetGlobalVector(MainLightShadowCasterPass.MainLightShadowConstantBuffer._ShadowmapSize, MainLightShadowCasterPass.s_EmptyShadowmapSize);
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x000416D0 File Offset: 0x0003F8D0
		private void RenderMainLightCascadeShadowmap(ref ScriptableRenderContext context, ref RenderingData renderingData)
		{
			CullingResults cullResults = renderingData.cullResults;
			LightData lightData = renderingData.lightData;
			int mainLightIndex = lightData.mainLightIndex;
			if (mainLightIndex == -1)
			{
				return;
			}
			VisibleLight visibleLight = lightData.visibleLights[mainLightIndex];
			CommandBuffer commandBuffer = renderingData.commandBuffer;
			using (new ProfilingScope(commandBuffer, ProfilingSampler.Get<URPProfileId>(URPProfileId.MainLightShadow)))
			{
				ShadowUtils.SetCameraPosition(commandBuffer, renderingData.cameraData.worldSpaceCameraPos);
				ShadowUtils.SetWorldToCameraAndCameraToWorldMatrices(commandBuffer, renderingData.cameraData.GetViewMatrix(0));
				ShadowDrawingSettings shadowDrawingSettings = new ShadowDrawingSettings(cullResults, mainLightIndex, BatchCullingProjectionType.Orthographic);
				shadowDrawingSettings.useRenderingLayerMaskTest = UniversalRenderPipeline.asset.useRenderingLayers;
				for (int i = 0; i < this.m_ShadowCasterCascadesCount; i++)
				{
					shadowDrawingSettings.splitData = this.m_CascadeSlices[i].splitData;
					Vector4 shadowBias = ShadowUtils.GetShadowBias(ref visibleLight, mainLightIndex, ref renderingData.shadowData, this.m_CascadeSlices[i].projectionMatrix, (float)this.m_CascadeSlices[i].resolution);
					ShadowUtils.SetupShadowCasterConstantBuffer(commandBuffer, ref visibleLight, shadowBias);
					CoreUtils.SetKeyword(commandBuffer, "_CASTING_PUNCTUAL_LIGHT_SHADOW", false);
					ShadowUtils.RenderShadowSlice(commandBuffer, ref context, ref this.m_CascadeSlices[i], ref shadowDrawingSettings, this.m_CascadeSlices[i].projectionMatrix, this.m_CascadeSlices[i].viewMatrix);
				}
				renderingData.shadowData.isKeywordSoftShadowsEnabled = visibleLight.light.shadows == LightShadows.Soft && renderingData.shadowData.supportsSoftShadows;
				CoreUtils.SetKeyword(commandBuffer, "_MAIN_LIGHT_SHADOWS", renderingData.shadowData.mainLightShadowCascadesCount == 1);
				CoreUtils.SetKeyword(commandBuffer, "_MAIN_LIGHT_SHADOWS_CASCADE", renderingData.shadowData.mainLightShadowCascadesCount > 1);
				ShadowUtils.SetSoftShadowQualityShaderKeywords(commandBuffer, ref renderingData.shadowData);
				this.SetupMainLightShadowReceiverConstants(commandBuffer, ref visibleLight, ref renderingData.shadowData);
			}
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x000418C0 File Offset: 0x0003FAC0
		private void SetupMainLightShadowReceiverConstants(CommandBuffer cmd, ref VisibleLight shadowLight, ref ShadowData shadowData)
		{
			Light light = shadowLight.light;
			bool flag = shadowLight.light.shadows == LightShadows.Soft && shadowData.supportsSoftShadows;
			int shadowCasterCascadesCount = this.m_ShadowCasterCascadesCount;
			for (int i = 0; i < shadowCasterCascadesCount; i++)
			{
				this.m_MainLightShadowMatrices[i] = this.m_CascadeSlices[i].shadowTransform;
			}
			Matrix4x4 zero = Matrix4x4.zero;
			zero.m22 = (SystemInfo.usesReversedZBuffer ? 1f : 0f);
			for (int j = shadowCasterCascadesCount; j <= 4; j++)
			{
				this.m_MainLightShadowMatrices[j] = zero;
			}
			float num = 1f / (float)this.renderTargetWidth;
			float num2 = 1f / (float)this.renderTargetHeight;
			float num3 = 0.5f * num;
			float num4 = 0.5f * num2;
			float num5 = ShadowUtils.SoftShadowQualityToShaderProperty(light, flag);
			float num6;
			float num7;
			ShadowUtils.GetScaleAndBiasForLinearDistanceFade(this.m_MaxShadowDistanceSq, this.m_CascadeBorder, out num6, out num7);
			cmd.SetGlobalMatrixArray(MainLightShadowCasterPass.MainLightShadowConstantBuffer._WorldToShadow, this.m_MainLightShadowMatrices);
			cmd.SetGlobalVector(MainLightShadowCasterPass.MainLightShadowConstantBuffer._ShadowParams, new Vector4(light.shadowStrength, num5, num6, num7));
			if (this.m_ShadowCasterCascadesCount > 1)
			{
				cmd.SetGlobalVector(MainLightShadowCasterPass.MainLightShadowConstantBuffer._CascadeShadowSplitSpheres0, this.m_CascadeSplitDistances[0]);
				cmd.SetGlobalVector(MainLightShadowCasterPass.MainLightShadowConstantBuffer._CascadeShadowSplitSpheres1, this.m_CascadeSplitDistances[1]);
				cmd.SetGlobalVector(MainLightShadowCasterPass.MainLightShadowConstantBuffer._CascadeShadowSplitSpheres2, this.m_CascadeSplitDistances[2]);
				cmd.SetGlobalVector(MainLightShadowCasterPass.MainLightShadowConstantBuffer._CascadeShadowSplitSpheres3, this.m_CascadeSplitDistances[3]);
				cmd.SetGlobalVector(MainLightShadowCasterPass.MainLightShadowConstantBuffer._CascadeShadowSplitSphereRadii, new Vector4(this.m_CascadeSplitDistances[0].w * this.m_CascadeSplitDistances[0].w, this.m_CascadeSplitDistances[1].w * this.m_CascadeSplitDistances[1].w, this.m_CascadeSplitDistances[2].w * this.m_CascadeSplitDistances[2].w, this.m_CascadeSplitDistances[3].w * this.m_CascadeSplitDistances[3].w));
			}
			if (shadowData.supportsSoftShadows)
			{
				cmd.SetGlobalVector(MainLightShadowCasterPass.MainLightShadowConstantBuffer._ShadowOffset0, new Vector4(-num3, -num4, num3, -num4));
				cmd.SetGlobalVector(MainLightShadowCasterPass.MainLightShadowConstantBuffer._ShadowOffset1, new Vector4(-num3, num4, num3, num4));
				cmd.SetGlobalVector(MainLightShadowCasterPass.MainLightShadowConstantBuffer._ShadowmapSize, new Vector4(num, num2, (float)this.renderTargetWidth, (float)this.renderTargetHeight));
			}
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x00041B40 File Offset: 0x0003FD40
		internal TextureHandle Render(RenderGraph graph, ref RenderingData renderingData)
		{
			MainLightShadowCasterPass.PassData passData;
			TextureHandle shadowmapTexture;
			using (RenderGraphBuilder renderGraphBuilder = graph.AddRenderPass<MainLightShadowCasterPass.PassData>("Main Light Shadowmap", out passData, base.profilingSampler))
			{
				this.InitPassData(ref passData, ref renderingData, ref graph);
				if (!this.m_CreateEmptyShadowmap)
				{
					passData.shadowmapTexture = UniversalRenderer.CreateRenderGraphTexture(graph, this.m_MainLightShadowmapTexture.rt.descriptor, "Main Shadowmap", true, ShadowUtils.m_ForceShadowPointSampling ? FilterMode.Point : FilterMode.Bilinear, TextureWrapMode.Clamp);
					renderGraphBuilder.UseDepthBuffer(in passData.shadowmapTexture, DepthAccess.Write);
				}
				renderGraphBuilder.AllowPassCulling(false);
				renderGraphBuilder.SetRenderFunc<MainLightShadowCasterPass.PassData>(delegate(MainLightShadowCasterPass.PassData data, RenderGraphContext context)
				{
					if (!data.emptyShadowmap)
					{
						data.pass.RenderMainLightCascadeShadowmap(ref context.renderContext, ref data.renderingData);
					}
				});
				shadowmapTexture = passData.shadowmapTexture;
			}
			MainLightShadowCasterPass.PassData passData2;
			TextureHandle shadowmapTexture2;
			using (RenderGraphBuilder renderGraphBuilder2 = graph.AddRenderPass<MainLightShadowCasterPass.PassData>("Set Main Shadow Globals", out passData2, base.profilingSampler))
			{
				this.InitPassData(ref passData2, ref renderingData, ref graph);
				passData2.shadowmapTexture = shadowmapTexture;
				if (shadowmapTexture.IsValid())
				{
					renderGraphBuilder2.UseDepthBuffer(in shadowmapTexture, DepthAccess.Read);
				}
				renderGraphBuilder2.AllowPassCulling(false);
				renderGraphBuilder2.SetRenderFunc<MainLightShadowCasterPass.PassData>(delegate(MainLightShadowCasterPass.PassData data, RenderGraphContext context)
				{
					if (data.emptyShadowmap)
					{
						data.pass.SetEmptyMainLightCascadeShadowmap(ref context.renderContext, ref data.renderingData);
						data.shadowmapTexture = data.graph.defaultResources.defaultShadowTexture;
					}
					data.renderingData.commandBuffer.SetGlobalTexture(data.shadowmapID, data.shadowmapTexture);
				});
				shadowmapTexture2 = passData2.shadowmapTexture;
			}
			return shadowmapTexture2;
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x00041C98 File Offset: 0x0003FE98
		private void InitPassData(ref MainLightShadowCasterPass.PassData passData, ref RenderingData renderingData, ref RenderGraph graph)
		{
			passData.pass = this;
			passData.graph = graph;
			passData.emptyShadowmap = this.m_CreateEmptyShadowmap;
			passData.shadowmapID = this.m_MainLightShadowmapID;
			passData.renderingData = renderingData;
		}

		// Token: 0x040008B4 RID: 2228
		private const int k_MaxCascades = 4;

		// Token: 0x040008B5 RID: 2229
		private const int k_ShadowmapBufferBits = 16;

		// Token: 0x040008B6 RID: 2230
		private float m_CascadeBorder;

		// Token: 0x040008B7 RID: 2231
		private float m_MaxShadowDistanceSq;

		// Token: 0x040008B8 RID: 2232
		private int m_ShadowCasterCascadesCount;

		// Token: 0x040008B9 RID: 2233
		private int m_MainLightShadowmapID;

		// Token: 0x040008BA RID: 2234
		internal RTHandle m_MainLightShadowmapTexture;

		// Token: 0x040008BB RID: 2235
		private RTHandle m_EmptyMainLightShadowmapTexture;

		// Token: 0x040008BC RID: 2236
		private const int k_EmptyShadowMapDimensions = 1;

		// Token: 0x040008BD RID: 2237
		private const string k_MainLightShadowMapTextureName = "_MainLightShadowmapTexture";

		// Token: 0x040008BE RID: 2238
		private const string k_EmptyMainLightShadowMapTextureName = "_EmptyMainLightShadowmapTexture";

		// Token: 0x040008BF RID: 2239
		private static readonly Vector4 s_EmptyShadowParams = new Vector4(1f, 0f, 1f, 0f);

		// Token: 0x040008C0 RID: 2240
		private static readonly Vector4 s_EmptyShadowmapSize = (MainLightShadowCasterPass.s_EmptyShadowmapSize = new Vector4(1f, 1f, 1f, 1f));

		// Token: 0x040008C1 RID: 2241
		private Matrix4x4[] m_MainLightShadowMatrices;

		// Token: 0x040008C2 RID: 2242
		private ShadowSliceData[] m_CascadeSlices;

		// Token: 0x040008C3 RID: 2243
		private Vector4[] m_CascadeSplitDistances;

		// Token: 0x040008C4 RID: 2244
		private bool m_CreateEmptyShadowmap;

		// Token: 0x040008C5 RID: 2245
		private int renderTargetWidth;

		// Token: 0x040008C6 RID: 2246
		private int renderTargetHeight;

		// Token: 0x040008C7 RID: 2247
		private ProfilingSampler m_ProfilingSetupSampler = new ProfilingSampler("Setup Main Shadowmap");

		// Token: 0x020001F5 RID: 501
		private static class MainLightShadowConstantBuffer
		{
			// Token: 0x04000CDB RID: 3291
			public static int _WorldToShadow;

			// Token: 0x04000CDC RID: 3292
			public static int _ShadowParams;

			// Token: 0x04000CDD RID: 3293
			public static int _CascadeShadowSplitSpheres0;

			// Token: 0x04000CDE RID: 3294
			public static int _CascadeShadowSplitSpheres1;

			// Token: 0x04000CDF RID: 3295
			public static int _CascadeShadowSplitSpheres2;

			// Token: 0x04000CE0 RID: 3296
			public static int _CascadeShadowSplitSpheres3;

			// Token: 0x04000CE1 RID: 3297
			public static int _CascadeShadowSplitSphereRadii;

			// Token: 0x04000CE2 RID: 3298
			public static int _ShadowOffset0;

			// Token: 0x04000CE3 RID: 3299
			public static int _ShadowOffset1;

			// Token: 0x04000CE4 RID: 3300
			public static int _ShadowmapSize;
		}

		// Token: 0x020001F6 RID: 502
		private class PassData
		{
			// Token: 0x04000CE5 RID: 3301
			internal MainLightShadowCasterPass pass;

			// Token: 0x04000CE6 RID: 3302
			internal RenderGraph graph;

			// Token: 0x04000CE7 RID: 3303
			internal TextureHandle shadowmapTexture;

			// Token: 0x04000CE8 RID: 3304
			internal RenderingData renderingData;

			// Token: 0x04000CE9 RID: 3305
			internal int shadowmapID;

			// Token: 0x04000CEA RID: 3306
			internal bool emptyShadowmap;
		}
	}
}
