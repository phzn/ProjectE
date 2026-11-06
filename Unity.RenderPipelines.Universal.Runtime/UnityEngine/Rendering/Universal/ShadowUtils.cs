using System;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal.Internal;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000D6 RID: 214
	public static class ShadowUtils
	{
		// Token: 0x060006AF RID: 1711 RVA: 0x00027EE8 File Offset: 0x000260E8
		public static bool ExtractDirectionalLightMatrix(ref CullingResults cullResults, ref ShadowData shadowData, int shadowLightIndex, int cascadeIndex, int shadowmapWidth, int shadowmapHeight, int shadowResolution, float shadowNearPlane, out Vector4 cascadeSplitDistance, out ShadowSliceData shadowSliceData, out Matrix4x4 viewMatrix, out Matrix4x4 projMatrix)
		{
			bool flag = ShadowUtils.ExtractDirectionalLightMatrix(ref cullResults, ref shadowData, shadowLightIndex, cascadeIndex, shadowmapWidth, shadowmapHeight, shadowResolution, shadowNearPlane, out cascadeSplitDistance, out shadowSliceData);
			viewMatrix = shadowSliceData.viewMatrix;
			projMatrix = shadowSliceData.projectionMatrix;
			return flag;
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x00027F28 File Offset: 0x00026128
		public static bool ExtractDirectionalLightMatrix(ref CullingResults cullResults, ref ShadowData shadowData, int shadowLightIndex, int cascadeIndex, int shadowmapWidth, int shadowmapHeight, int shadowResolution, float shadowNearPlane, out Vector4 cascadeSplitDistance, out ShadowSliceData shadowSliceData)
		{
			bool flag = cullResults.ComputeDirectionalShadowMatricesAndCullingPrimitives(shadowLightIndex, cascadeIndex, shadowData.mainLightShadowCascadesCount, shadowData.mainLightShadowCascadesSplit, shadowResolution, shadowNearPlane, out shadowSliceData.viewMatrix, out shadowSliceData.projectionMatrix, out shadowSliceData.splitData);
			cascadeSplitDistance = shadowSliceData.splitData.cullingSphere;
			shadowSliceData.offsetX = cascadeIndex % 2 * shadowResolution;
			shadowSliceData.offsetY = cascadeIndex / 2 * shadowResolution;
			shadowSliceData.resolution = shadowResolution;
			shadowSliceData.shadowTransform = ShadowUtils.GetShadowTransform(shadowSliceData.projectionMatrix, shadowSliceData.viewMatrix);
			shadowSliceData.splitData.shadowCascadeBlendCullingFactor = 1f;
			if (shadowData.mainLightShadowCascadesCount > 1)
			{
				ShadowUtils.ApplySliceTransform(ref shadowSliceData, shadowmapWidth, shadowmapHeight);
			}
			return flag;
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x00027FD7 File Offset: 0x000261D7
		public static bool ExtractSpotLightMatrix(ref CullingResults cullResults, ref ShadowData shadowData, int shadowLightIndex, out Matrix4x4 shadowMatrix, out Matrix4x4 viewMatrix, out Matrix4x4 projMatrix, out ShadowSplitData splitData)
		{
			bool flag = cullResults.ComputeSpotShadowMatricesAndCullingPrimitives(shadowLightIndex, out viewMatrix, out projMatrix, out splitData);
			shadowMatrix = ShadowUtils.GetShadowTransform(projMatrix, viewMatrix);
			return flag;
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x00028000 File Offset: 0x00026200
		public static bool ExtractPointLightMatrix(ref CullingResults cullResults, ref ShadowData shadowData, int shadowLightIndex, CubemapFace cubemapFace, float fovBias, out Matrix4x4 shadowMatrix, out Matrix4x4 viewMatrix, out Matrix4x4 projMatrix, out ShadowSplitData splitData)
		{
			bool flag = cullResults.ComputePointShadowMatricesAndCullingPrimitives(shadowLightIndex, cubemapFace, fovBias, out viewMatrix, out projMatrix, out splitData);
			viewMatrix.m10 = -viewMatrix.m10;
			viewMatrix.m11 = -viewMatrix.m11;
			viewMatrix.m12 = -viewMatrix.m12;
			viewMatrix.m13 = -viewMatrix.m13;
			shadowMatrix = ShadowUtils.GetShadowTransform(projMatrix, viewMatrix);
			return flag;
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x00028074 File Offset: 0x00026274
		public static void RenderShadowSlice(CommandBuffer cmd, ref ScriptableRenderContext context, ref ShadowSliceData shadowSliceData, ref ShadowDrawingSettings settings, Matrix4x4 proj, Matrix4x4 view)
		{
			cmd.SetGlobalDepthBias(1f, 2.5f);
			cmd.SetViewport(new Rect((float)shadowSliceData.offsetX, (float)shadowSliceData.offsetY, (float)shadowSliceData.resolution, (float)shadowSliceData.resolution));
			cmd.SetViewProjectionMatrices(view, proj);
			context.ExecuteCommandBuffer(cmd);
			cmd.Clear();
			context.DrawShadows(ref settings);
			cmd.DisableScissorRect();
			context.ExecuteCommandBuffer(cmd);
			cmd.Clear();
			cmd.SetGlobalDepthBias(0f, 0f);
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x000280F9 File Offset: 0x000262F9
		public static void RenderShadowSlice(CommandBuffer cmd, ref ScriptableRenderContext context, ref ShadowSliceData shadowSliceData, ref ShadowDrawingSettings settings)
		{
			ShadowUtils.RenderShadowSlice(cmd, ref context, ref shadowSliceData, ref settings, shadowSliceData.projectionMatrix, shadowSliceData.viewMatrix);
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x00028110 File Offset: 0x00026310
		public static int GetMaxTileResolutionInAtlas(int atlasWidth, int atlasHeight, int tileCount)
		{
			int num = Mathf.Min(atlasWidth, atlasHeight);
			for (int i = atlasWidth / num * atlasHeight / num; i < tileCount; i = atlasWidth / num * atlasHeight / num)
			{
				num >>= 1;
			}
			return num;
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x00028140 File Offset: 0x00026340
		public static void ApplySliceTransform(ref ShadowSliceData shadowSliceData, int atlasWidth, int atlasHeight)
		{
			Matrix4x4 identity = Matrix4x4.identity;
			float num = 1f / (float)atlasWidth;
			float num2 = 1f / (float)atlasHeight;
			identity.m00 = (float)shadowSliceData.resolution * num;
			identity.m11 = (float)shadowSliceData.resolution * num2;
			identity.m03 = (float)shadowSliceData.offsetX * num;
			identity.m13 = (float)shadowSliceData.offsetY * num2;
			shadowSliceData.shadowTransform = identity * shadowSliceData.shadowTransform;
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x000281B8 File Offset: 0x000263B8
		public static Vector4 GetShadowBias(ref VisibleLight shadowLight, int shadowLightIndex, ref ShadowData shadowData, Matrix4x4 lightProjectionMatrix, float shadowResolution)
		{
			if (shadowLightIndex < 0 || shadowLightIndex >= shadowData.bias.Count)
			{
				Debug.LogWarning(string.Format("{0} is not a valid light index.", shadowLightIndex));
				return Vector4.zero;
			}
			float num;
			if (shadowLight.lightType == LightType.Directional)
			{
				num = 2f / lightProjectionMatrix.m00;
			}
			else if (shadowLight.lightType == LightType.Spot)
			{
				num = Mathf.Tan(shadowLight.spotAngle * 0.5f * 0.017453292f) * shadowLight.range;
			}
			else if (shadowLight.lightType == LightType.Point)
			{
				float pointLightShadowFrustumFovBiasInDegrees = AdditionalLightsShadowCasterPass.GetPointLightShadowFrustumFovBiasInDegrees((int)shadowResolution, shadowLight.light.shadows == LightShadows.Soft);
				num = Mathf.Tan((90f + pointLightShadowFrustumFovBiasInDegrees) * 0.5f * 0.017453292f) * shadowLight.range;
			}
			else
			{
				Debug.LogWarning("Only point, spot and directional shadow casters are supported in universal pipeline");
				num = 0f;
			}
			float num2 = num / shadowResolution;
			float num3 = -shadowData.bias[shadowLightIndex].x * num2;
			float num4 = -shadowData.bias[shadowLightIndex].y * num2;
			if (shadowLight.lightType == LightType.Point)
			{
				num4 = 0f;
			}
			if (shadowData.supportsSoftShadows && shadowLight.light.shadows == LightShadows.Soft)
			{
				SoftShadowQuality softShadowQuality = SoftShadowQuality.Medium;
				UniversalAdditionalLightData universalAdditionalLightData;
				if (shadowLight.light.TryGetComponent<UniversalAdditionalLightData>(out universalAdditionalLightData))
				{
					softShadowQuality = universalAdditionalLightData.softShadowQuality;
				}
				float num5 = 2.5f;
				switch (softShadowQuality)
				{
				case SoftShadowQuality.Low:
					num5 = 1.5f;
					break;
				case SoftShadowQuality.Medium:
					num5 = 2.5f;
					break;
				case SoftShadowQuality.High:
					num5 = 3.5f;
					break;
				}
				num3 *= num5;
				num4 *= num5;
			}
			return new Vector4(num3, num4, 0f, 0f);
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x00028348 File Offset: 0x00026548
		internal static void GetScaleAndBiasForLinearDistanceFade(float fadeDistance, float border, out float scale, out float bias)
		{
			if (border < 0.0001f)
			{
				float num = 1000f;
				scale = num;
				bias = -fadeDistance * num;
				return;
			}
			border = 1f - border;
			border *= border;
			float num2 = border * fadeDistance;
			scale = 1f / (fadeDistance - num2);
			bias = -num2 / (fadeDistance - num2);
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x00028394 File Offset: 0x00026594
		public static void SetupShadowCasterConstantBuffer(CommandBuffer cmd, ref VisibleLight shadowLight, Vector4 shadowBias)
		{
			ShadowUtils.SetShadowBias(cmd, shadowBias);
			Vector3 vector = -shadowLight.localToWorldMatrix.GetColumn(2);
			ShadowUtils.SetLightDirection(cmd, vector);
			Vector3 vector2 = shadowLight.localToWorldMatrix.GetColumn(3);
			ShadowUtils.SetLightPosition(cmd, vector2);
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x000283E5 File Offset: 0x000265E5
		internal static void SetShadowBias(CommandBuffer cmd, Vector4 shadowBias)
		{
			cmd.SetGlobalVector(ShaderPropertyId.shadowBias, shadowBias);
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x000283F3 File Offset: 0x000265F3
		internal static void SetLightDirection(CommandBuffer cmd, Vector3 lightDirection)
		{
			cmd.SetGlobalVector(ShaderPropertyId.lightDirection, new Vector4(lightDirection.x, lightDirection.y, lightDirection.z, 0f));
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x0002841C File Offset: 0x0002661C
		internal static void SetLightPosition(CommandBuffer cmd, Vector3 lightPosition)
		{
			cmd.SetGlobalVector(ShaderPropertyId.lightPosition, new Vector4(lightPosition.x, lightPosition.y, lightPosition.z, 1f));
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x00028445 File Offset: 0x00026645
		internal static void SetCameraPosition(CommandBuffer cmd, Vector3 worldSpaceCameraPos)
		{
			cmd.SetGlobalVector(ShaderPropertyId.worldSpaceCameraPos, worldSpaceCameraPos);
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x00028458 File Offset: 0x00026658
		internal static void SetWorldToCameraAndCameraToWorldMatrices(CommandBuffer cmd, Matrix4x4 viewMatrix)
		{
			Matrix4x4 matrix4x = Matrix4x4.Scale(new Vector3(1f, 1f, -1f)) * viewMatrix;
			Matrix4x4 inverse = matrix4x.inverse;
			cmd.SetGlobalMatrix(ShaderPropertyId.worldToCameraMatrix, matrix4x);
			cmd.SetGlobalMatrix(ShaderPropertyId.cameraToWorldMatrix, inverse);
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x000284A8 File Offset: 0x000266A8
		private static RenderTextureDescriptor GetTemporaryShadowTextureDescriptor(int width, int height, int bits)
		{
			GraphicsFormat depthStencilFormat = GraphicsFormatUtility.GetDepthStencilFormat(bits, 0);
			return new RenderTextureDescriptor(width, height, GraphicsFormat.None, depthStencilFormat)
			{
				shadowSamplingMode = ((RenderingUtils.SupportsRenderTextureFormat(RenderTextureFormat.Shadowmap) && SystemInfo.graphicsDeviceType != GraphicsDeviceType.OpenGLES2) ? ShadowSamplingMode.CompareDepths : ShadowSamplingMode.None)
			};
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x000284E4 File Offset: 0x000266E4
		[Obsolete("Use AllocShadowRT or ShadowRTReAllocateIfNeeded")]
		public static RenderTexture GetTemporaryShadowTexture(int width, int height, int bits)
		{
			RenderTexture temporary = RenderTexture.GetTemporary(ShadowUtils.GetTemporaryShadowTextureDescriptor(width, height, bits));
			temporary.filterMode = (ShadowUtils.m_ForceShadowPointSampling ? FilterMode.Point : FilterMode.Bilinear);
			temporary.wrapMode = TextureWrapMode.Clamp;
			return temporary;
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x0002850C File Offset: 0x0002670C
		public static bool ShadowRTNeedsReAlloc(RTHandle handle, int width, int height, int bits, int anisoLevel, float mipMapBias, string name)
		{
			if (handle == null || handle.rt == null)
			{
				return true;
			}
			RenderTextureDescriptor temporaryShadowTextureDescriptor = ShadowUtils.GetTemporaryShadowTextureDescriptor(width, height, bits);
			if (ShadowUtils.m_ForceShadowPointSampling)
			{
				if (handle.rt.filterMode != FilterMode.Point)
				{
					return true;
				}
			}
			else if (handle.rt.filterMode != FilterMode.Bilinear)
			{
				return true;
			}
			TextureDesc textureDesc = RTHandleResourcePool.CreateTextureDesc(temporaryShadowTextureDescriptor, TextureSizeMode.Explicit, anisoLevel, mipMapBias, ShadowUtils.m_ForceShadowPointSampling ? FilterMode.Point : FilterMode.Bilinear, TextureWrapMode.Clamp, name);
			return RenderingUtils.RTHandleNeedsReAlloc(handle, in textureDesc, false);
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x00028580 File Offset: 0x00026780
		public static RTHandle AllocShadowRT(int width, int height, int bits, int anisoLevel, float mipMapBias, string name)
		{
			RenderTextureDescriptor temporaryShadowTextureDescriptor = ShadowUtils.GetTemporaryShadowTextureDescriptor(width, height, bits);
			return RTHandles.Alloc(in temporaryShadowTextureDescriptor, ShadowUtils.m_ForceShadowPointSampling ? FilterMode.Point : FilterMode.Bilinear, TextureWrapMode.Clamp, true, 1, 0f, name);
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x000285B2 File Offset: 0x000267B2
		public static bool ShadowRTReAllocateIfNeeded(ref RTHandle handle, int width, int height, int bits, int anisoLevel = 1, float mipMapBias = 0f, string name = "")
		{
			if (ShadowUtils.ShadowRTNeedsReAlloc(handle, width, height, bits, anisoLevel, mipMapBias, name))
			{
				RTHandle rthandle = handle;
				if (rthandle != null)
				{
					rthandle.Release();
				}
				handle = ShadowUtils.AllocShadowRT(width, height, bits, anisoLevel, mipMapBias, name);
				return true;
			}
			return false;
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x000285E8 File Offset: 0x000267E8
		private static Matrix4x4 GetShadowTransform(Matrix4x4 proj, Matrix4x4 view)
		{
			if (SystemInfo.usesReversedZBuffer)
			{
				proj.m20 = -proj.m20;
				proj.m21 = -proj.m21;
				proj.m22 = -proj.m22;
				proj.m23 = -proj.m23;
			}
			Matrix4x4 matrix4x = proj * view;
			Matrix4x4 identity = Matrix4x4.identity;
			identity.m00 = 0.5f;
			identity.m11 = 0.5f;
			identity.m22 = 0.5f;
			identity.m03 = 0.5f;
			identity.m23 = 0.5f;
			identity.m13 = 0.5f;
			return identity * matrix4x;
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x00028694 File Offset: 0x00026894
		internal static float SoftShadowQualityToShaderProperty(Light light, bool softShadowsEnabled)
		{
			float num = (softShadowsEnabled ? 1f : 0f);
			UniversalAdditionalLightData universalAdditionalLightData;
			if (light.TryGetComponent<UniversalAdditionalLightData>(out universalAdditionalLightData))
			{
				SoftShadowQuality? softShadowQuality;
				if (universalAdditionalLightData.softShadowQuality != SoftShadowQuality.UsePipelineSettings)
				{
					softShadowQuality = new SoftShadowQuality?(universalAdditionalLightData.softShadowQuality);
				}
				else
				{
					UniversalRenderPipelineAsset asset = UniversalRenderPipeline.asset;
					softShadowQuality = ((asset != null) ? new SoftShadowQuality?(asset.softShadowQuality) : null);
				}
				SoftShadowQuality? softShadowQuality2 = softShadowQuality;
				num *= (float)Math.Max((int)softShadowQuality2.Value, 1);
			}
			return num;
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x00028701 File Offset: 0x00026901
		internal static bool SupportsPerLightSoftShadowQuality()
		{
			return true;
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x00028704 File Offset: 0x00026904
		internal static void SetPerLightSoftShadowKeyword(CommandBuffer cmd, bool hasSoftShadows)
		{
			if (ShadowUtils.SupportsPerLightSoftShadowQuality())
			{
				CoreUtils.SetKeyword(cmd, "_SHADOWS_SOFT", hasSoftShadows);
			}
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x0002871C File Offset: 0x0002691C
		internal static void SetSoftShadowQualityShaderKeywords(CommandBuffer cmd, ref ShadowData shadowData)
		{
			CoreUtils.SetKeyword(cmd, "_SHADOWS_SOFT", shadowData.isKeywordSoftShadowsEnabled);
			if (ShadowUtils.SupportsPerLightSoftShadowQuality())
			{
				CoreUtils.SetKeyword(cmd, "_SHADOWS_SOFT_LOW", false);
				CoreUtils.SetKeyword(cmd, "_SHADOWS_SOFT_MEDIUM", false);
				CoreUtils.SetKeyword(cmd, "_SHADOWS_SOFT_HIGH", false);
				return;
			}
			if (shadowData.isKeywordSoftShadowsEnabled)
			{
				UniversalRenderPipelineAsset asset = UniversalRenderPipeline.asset;
				if (asset != null && asset.softShadowQuality == SoftShadowQuality.Low)
				{
					CoreUtils.SetKeyword(cmd, "_SHADOWS_SOFT_LOW", true);
					CoreUtils.SetKeyword(cmd, "_SHADOWS_SOFT_MEDIUM", false);
					CoreUtils.SetKeyword(cmd, "_SHADOWS_SOFT_HIGH", false);
					CoreUtils.SetKeyword(cmd, "_SHADOWS_SOFT", false);
					return;
				}
			}
			if (shadowData.isKeywordSoftShadowsEnabled)
			{
				UniversalRenderPipelineAsset asset2 = UniversalRenderPipeline.asset;
				if (asset2 != null && asset2.softShadowQuality == SoftShadowQuality.Medium)
				{
					CoreUtils.SetKeyword(cmd, "_SHADOWS_SOFT_LOW", false);
					CoreUtils.SetKeyword(cmd, "_SHADOWS_SOFT_MEDIUM", true);
					CoreUtils.SetKeyword(cmd, "_SHADOWS_SOFT_HIGH", false);
					CoreUtils.SetKeyword(cmd, "_SHADOWS_SOFT", false);
					return;
				}
			}
			if (shadowData.isKeywordSoftShadowsEnabled)
			{
				UniversalRenderPipelineAsset asset3 = UniversalRenderPipeline.asset;
				if (asset3 != null && asset3.softShadowQuality == SoftShadowQuality.High)
				{
					CoreUtils.SetKeyword(cmd, "_SHADOWS_SOFT_LOW", false);
					CoreUtils.SetKeyword(cmd, "_SHADOWS_SOFT_MEDIUM", false);
					CoreUtils.SetKeyword(cmd, "_SHADOWS_SOFT_HIGH", true);
					CoreUtils.SetKeyword(cmd, "_SHADOWS_SOFT", false);
				}
			}
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x00028854 File Offset: 0x00026A54
		internal static bool IsValidShadowCastingLight(ref LightData lightData, int i)
		{
			if (i == lightData.mainLightIndex)
			{
				return false;
			}
			ref VisibleLight ptr = ref lightData.visibleLights.UnsafeElementAt<VisibleLight>(i);
			if (ptr.lightType == LightType.Directional)
			{
				return false;
			}
			Light light = ptr.light;
			return light != null && light.shadows != LightShadows.None && !Mathf.Approximately(light.shadowStrength, 0f);
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x000288B4 File Offset: 0x00026AB4
		internal static int GetPunctualLightShadowSlicesCount(in LightType lightType)
		{
			LightType lightType2 = lightType;
			if (lightType2 == LightType.Spot)
			{
				return 1;
			}
			if (lightType2 != LightType.Point)
			{
				return 0;
			}
			return 6;
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x000288D2 File Offset: 0x00026AD2
		internal static int MinimalPunctualLightShadowResolution(bool softShadow)
		{
			if (!softShadow)
			{
				return 8;
			}
			return 16;
		}

		// Token: 0x0400055A RID: 1370
		internal static readonly bool m_ForceShadowPointSampling = SystemInfo.graphicsDeviceType == GraphicsDeviceType.Metal && GraphicsSettings.HasShaderDefine(Graphics.activeTier, BuiltinShaderDefine.UNITY_METAL_SHADOWS_USE_POINT_FILTERING);

		// Token: 0x0400055B RID: 1371
		private static int _ShadowBias = Shader.PropertyToID("_ShadowBias");

		// Token: 0x0400055C RID: 1372
		private static int _LightDirection = Shader.PropertyToID("_LightDirection");

		// Token: 0x0400055D RID: 1373
		private static int _LightPosition = Shader.PropertyToID("_LightPosition");

		// Token: 0x0400055E RID: 1374
		internal const int kMinimumPunctualLightHardShadowResolution = 8;

		// Token: 0x0400055F RID: 1375
		internal const int kMinimumPunctualLightSoftShadowResolution = 16;
	}
}
