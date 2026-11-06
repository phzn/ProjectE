using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine.Experimental.Rendering.RenderGraphModule;

namespace UnityEngine.Rendering.Universal.Internal
{
	// Token: 0x02000119 RID: 281
	public class AdditionalLightsShadowCasterPass : ScriptableRenderPass
	{
		// Token: 0x06000924 RID: 2340 RVA: 0x0003ADB8 File Offset: 0x00038FB8
		public AdditionalLightsShadowCasterPass(RenderPassEvent evt)
		{
			base.profilingSampler = new ProfilingSampler("AdditionalLightsShadowCasterPass");
			base.renderPassEvent = evt;
			AdditionalLightsShadowCasterPass.AdditionalShadowsConstantBuffer._AdditionalLightsWorldToShadow = Shader.PropertyToID("_AdditionalLightsWorldToShadow");
			AdditionalLightsShadowCasterPass.AdditionalShadowsConstantBuffer._AdditionalShadowParams = Shader.PropertyToID("_AdditionalShadowParams");
			AdditionalLightsShadowCasterPass.AdditionalShadowsConstantBuffer._AdditionalShadowOffset0 = Shader.PropertyToID("_AdditionalShadowOffset0");
			AdditionalLightsShadowCasterPass.AdditionalShadowsConstantBuffer._AdditionalShadowOffset1 = Shader.PropertyToID("_AdditionalShadowOffset1");
			AdditionalLightsShadowCasterPass.AdditionalShadowsConstantBuffer._AdditionalShadowFadeParams = Shader.PropertyToID("_AdditionalShadowFadeParams");
			AdditionalLightsShadowCasterPass.AdditionalShadowsConstantBuffer._AdditionalShadowmapSize = Shader.PropertyToID("_AdditionalShadowmapSize");
			this.m_AdditionalLightsShadowmapID = Shader.PropertyToID("_AdditionalLightsShadowmapTexture");
			AdditionalLightsShadowCasterPass.m_AdditionalLightsWorldToShadow_SSBO = Shader.PropertyToID("_AdditionalLightsWorldToShadow_SSBO");
			AdditionalLightsShadowCasterPass.m_AdditionalShadowParams_SSBO = Shader.PropertyToID("_AdditionalShadowParams_SSBO");
			this.m_UseStructuredBuffer = RenderingUtils.useStructuredBuffer;
			int maxVisibleAdditionalLights = UniversalRenderPipeline.maxVisibleAdditionalLights;
			int num = maxVisibleAdditionalLights + 1;
			int num2 = (this.m_UseStructuredBuffer ? num : Math.Min(num, maxVisibleAdditionalLights));
			this.m_AdditionalLightIndexToVisibleLightIndex = new int[num2];
			this.m_VisibleLightIndexToAdditionalLightIndex = new int[num];
			this.m_VisibleLightIndexToSortedShadowResolutionRequestsFirstSliceIndex = new int[num];
			this.m_AdditionalLightIndexToShadowParams = new Vector4[num2];
			this.m_VisibleLightIndexToCameraSquareDistance = new float[num];
			AdditionalLightsShadowCasterPass.s_EmptyAdditionalLightIndexToShadowParams = new Vector4[num2];
			for (int i = 0; i < AdditionalLightsShadowCasterPass.s_EmptyAdditionalLightIndexToShadowParams.Length; i++)
			{
				AdditionalLightsShadowCasterPass.s_EmptyAdditionalLightIndexToShadowParams[i] = AdditionalLightsShadowCasterPass.c_DefaultShadowParams;
			}
			if (!this.m_UseStructuredBuffer)
			{
				this.m_AdditionalLightShadowSliceIndexTo_WorldShadowMatrix = new Matrix4x4[maxVisibleAdditionalLights];
				this.m_UnusedAtlasSquareAreas.Capacity = maxVisibleAdditionalLights;
				this.m_ShadowResolutionRequests.Capacity = maxVisibleAdditionalLights;
			}
			this.m_EmptyAdditionalLightShadowmapTexture = RTHandles.Alloc(Texture2D.blackTexture);
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x0003AF7E File Offset: 0x0003917E
		public void Dispose()
		{
			RTHandle additionalLightsShadowmapHandle = this.m_AdditionalLightsShadowmapHandle;
			if (additionalLightsShadowmapHandle == null)
			{
				return;
			}
			additionalLightsShadowmapHandle.Release();
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x0003AF90 File Offset: 0x00039190
		private int GetPunctualLightShadowSlicesCount(in LightType lightType)
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

		// Token: 0x06000927 RID: 2343 RVA: 0x0003AFB0 File Offset: 0x000391B0
		internal static float CalcGuardAngle(float frustumAngleInDegrees, float guardBandSizeInTexels, float sliceResolutionInTexels)
		{
			float num = frustumAngleInDegrees * 0.017453292f / 2f;
			float num2 = Mathf.Tan(num);
			float num3 = sliceResolutionInTexels / 2f;
			float num4 = guardBandSizeInTexels / 2f;
			float num5 = 1f + num4 / num3;
			float num6 = Mathf.Atan(num2 * num5) - num;
			return 2f * num6 * 57.29578f;
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x0003B004 File Offset: 0x00039204
		private int MinimalPunctualLightShadowResolution(bool softShadow)
		{
			if (!softShadow)
			{
				return 8;
			}
			return 16;
		}

		// Token: 0x06000929 RID: 2345 RVA: 0x0003B010 File Offset: 0x00039210
		internal static float GetPointLightShadowFrustumFovBiasInDegrees(int shadowSliceResolution, bool shadowFiltering)
		{
			float num = 4f;
			if (shadowSliceResolution > 8)
			{
				if (shadowSliceResolution <= 16)
				{
					num = 43f;
				}
				else if (shadowSliceResolution <= 32)
				{
					num = 18.55f;
				}
				else if (shadowSliceResolution <= 64)
				{
					num = 8.63f;
				}
				else if (shadowSliceResolution <= 128)
				{
					num = 4.13f;
				}
				else if (shadowSliceResolution <= 256)
				{
					num = 2.03f;
				}
				else if (shadowSliceResolution <= 512)
				{
					num = 1f;
				}
				else if (shadowSliceResolution <= 1024)
				{
					num = 0.5f;
				}
			}
			if (shadowFiltering && shadowSliceResolution > 16)
			{
				if (shadowSliceResolution <= 32)
				{
					num += 9.35f;
				}
				else if (shadowSliceResolution <= 64)
				{
					num += 4.07f;
				}
				else if (shadowSliceResolution <= 128)
				{
					num += 1.77f;
				}
				else if (shadowSliceResolution <= 256)
				{
					num += 0.85f;
				}
				else if (shadowSliceResolution <= 512)
				{
					num += 0.39f;
				}
				else if (shadowSliceResolution <= 1024)
				{
					num += 0.17f;
				}
			}
			return num;
		}

		// Token: 0x0600092A RID: 2346 RVA: 0x0003B0FC File Offset: 0x000392FC
		internal void InsertionSort(AdditionalLightsShadowCasterPass.ShadowResolutionRequest[] array, int startIndex, int lastIndex)
		{
			for (int i = startIndex + 1; i < lastIndex; i++)
			{
				AdditionalLightsShadowCasterPass.ShadowResolutionRequest shadowResolutionRequest = array[i];
				int num = i - 1;
				while (num >= 0 && (shadowResolutionRequest.requestedResolution > array[num].requestedResolution || (shadowResolutionRequest.requestedResolution == array[num].requestedResolution && !shadowResolutionRequest.softShadow && array[num].softShadow) || (shadowResolutionRequest.requestedResolution == array[num].requestedResolution && shadowResolutionRequest.softShadow == array[num].softShadow && !shadowResolutionRequest.pointLightShadow && array[num].pointLightShadow) || (shadowResolutionRequest.requestedResolution == array[num].requestedResolution && shadowResolutionRequest.softShadow == array[num].softShadow && shadowResolutionRequest.pointLightShadow == array[num].pointLightShadow && this.m_VisibleLightIndexToCameraSquareDistance[shadowResolutionRequest.visibleLightIndex] < this.m_VisibleLightIndexToCameraSquareDistance[array[num].visibleLightIndex]) || (shadowResolutionRequest.requestedResolution == array[num].requestedResolution && shadowResolutionRequest.softShadow == array[num].softShadow && shadowResolutionRequest.pointLightShadow == array[num].pointLightShadow && this.m_VisibleLightIndexToCameraSquareDistance[shadowResolutionRequest.visibleLightIndex] == this.m_VisibleLightIndexToCameraSquareDistance[array[num].visibleLightIndex] && shadowResolutionRequest.visibleLightIndex < array[num].visibleLightIndex) || (shadowResolutionRequest.requestedResolution == array[num].requestedResolution && shadowResolutionRequest.softShadow == array[num].softShadow && shadowResolutionRequest.pointLightShadow == array[num].pointLightShadow && this.m_VisibleLightIndexToCameraSquareDistance[shadowResolutionRequest.visibleLightIndex] == this.m_VisibleLightIndexToCameraSquareDistance[array[num].visibleLightIndex] && shadowResolutionRequest.visibleLightIndex == array[num].visibleLightIndex && shadowResolutionRequest.perLightShadowSliceIndex < array[num].perLightShadowSliceIndex)))
				{
					array[num + 1] = array[num];
					num--;
				}
				array[num + 1] = shadowResolutionRequest;
			}
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x0003B330 File Offset: 0x00039530
		private int EstimateScaleFactorNeededToFitAllShadowsInAtlas(in AdditionalLightsShadowCasterPass.ShadowResolutionRequest[] shadowResolutionRequests, int endIndex, int atlasWidth)
		{
			long num = (long)(atlasWidth * atlasWidth);
			long num2 = 0L;
			for (int i = 0; i < endIndex; i++)
			{
				num2 += (long)(shadowResolutionRequests[i].requestedResolution * shadowResolutionRequests[i].requestedResolution);
			}
			int num3 = 1;
			while (num2 > num * (long)num3 * (long)num3)
			{
				num3 *= 2;
			}
			return num3;
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x0003B384 File Offset: 0x00039584
		private void AtlasLayout(int atlasSize, int totalShadowSlicesCount, int estimatedScaleFactor)
		{
			bool flag = false;
			bool flag2 = false;
			int num = estimatedScaleFactor;
			while (!flag && !flag2)
			{
				this.m_UnusedAtlasSquareAreas.Clear();
				this.m_UnusedAtlasSquareAreas.Add(new RectInt(0, 0, atlasSize, atlasSize));
				flag = true;
				for (int i = 0; i < totalShadowSlicesCount; i++)
				{
					int num2 = this.m_SortedShadowResolutionRequests[i].requestedResolution / num;
					if (num2 < this.MinimalPunctualLightShadowResolution(this.m_SortedShadowResolutionRequests[i].softShadow))
					{
						flag2 = true;
						break;
					}
					bool flag3 = false;
					for (int j = 0; j < this.m_UnusedAtlasSquareAreas.Count; j++)
					{
						RectInt rectInt = this.m_UnusedAtlasSquareAreas[j];
						int width = rectInt.width;
						int height = rectInt.height;
						int x = rectInt.x;
						int y = rectInt.y;
						if (width >= num2)
						{
							this.m_SortedShadowResolutionRequests[i].offsetX = x;
							this.m_SortedShadowResolutionRequests[i].offsetY = y;
							this.m_SortedShadowResolutionRequests[i].allocatedResolution = num2;
							this.m_UnusedAtlasSquareAreas.RemoveAt(j);
							int num3 = totalShadowSlicesCount - i - 1;
							int k = 0;
							int num4 = num2;
							int num5 = num2;
							int num6 = x;
							int num7 = y;
							while (k < num3)
							{
								num6 += num4;
								if (num6 + num4 > x + width)
								{
									num6 = x;
									num7 += num5;
									if (num7 + num5 > y + height)
									{
										break;
									}
								}
								this.m_UnusedAtlasSquareAreas.Insert(j + k, new RectInt(num6, num7, num4, num5));
								k++;
							}
							flag3 = true;
							break;
						}
					}
					if (!flag3)
					{
						flag = false;
						break;
					}
				}
				if (!flag && !flag2)
				{
					num *= 2;
				}
			}
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x0003B53A File Offset: 0x0003973A
		private ulong ResolutionLog2ForHash(int resolution)
		{
			if (resolution <= 1024)
			{
				if (resolution == 512)
				{
					return 9UL;
				}
				if (resolution == 1024)
				{
					return 10UL;
				}
			}
			else
			{
				if (resolution == 2048)
				{
					return 11UL;
				}
				if (resolution == 4096)
				{
					return 12UL;
				}
			}
			return 8UL;
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x0003B578 File Offset: 0x00039778
		private ulong ComputeShadowRequestHash(ref RenderingData renderingData)
		{
			ulong num = 0UL;
			ulong num2 = 0UL;
			ulong num3 = 0UL;
			ulong num4 = 0UL;
			ulong num5 = 0UL;
			ulong num6 = 0UL;
			ulong num7 = 0UL;
			ulong num8 = 0UL;
			NativeArray<VisibleLight> visibleLights = renderingData.lightData.visibleLights;
			for (int i = 0; i < visibleLights.Length; i++)
			{
				if (this.IsValidShadowCastingLight(ref renderingData.lightData, i))
				{
					ref VisibleLight ptr = ref visibleLights.UnsafeElementAt<VisibleLight>(i);
					if (ptr.lightType == LightType.Point)
					{
						num += 1UL;
					}
					if (ptr.light.shadows == LightShadows.Soft)
					{
						num2 += 1UL;
					}
					if (renderingData.shadowData.resolution[i] == 128)
					{
						num3 += 1UL;
					}
					if (renderingData.shadowData.resolution[i] == 256)
					{
						num4 += 1UL;
					}
					if (renderingData.shadowData.resolution[i] == 512)
					{
						num5 += 1UL;
					}
					if (renderingData.shadowData.resolution[i] == 1024)
					{
						num6 += 1UL;
					}
					if (renderingData.shadowData.resolution[i] == 2048)
					{
						num7 += 1UL;
					}
					if (renderingData.shadowData.resolution[i] == 4096)
					{
						num8 += 1UL;
					}
				}
			}
			return (this.ResolutionLog2ForHash(renderingData.shadowData.additionalLightsShadowmapWidth) - 8UL) | (num << 3) | (num2 << 11) | (num3 << 19) | (num4 << 27) | (num5 << 35) | (num6 << 43) | (num7 << 50) | (num8 << 57);
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x0003B704 File Offset: 0x00039904
		public bool Setup(ref RenderingData renderingData)
		{
			bool flag;
			using (new ProfilingScope(null, this.m_ProfilingSetupSampler))
			{
				if (!renderingData.shadowData.additionalLightShadowsEnabled)
				{
					flag = false;
				}
				else if (!renderingData.shadowData.supportsAdditionalLightShadows)
				{
					flag = this.SetupForEmptyRendering(ref renderingData);
				}
				else
				{
					this.Clear();
					this.renderTargetWidth = renderingData.shadowData.additionalLightsShadowmapWidth;
					this.renderTargetHeight = renderingData.shadowData.additionalLightsShadowmapHeight;
					NativeArray<VisibleLight> visibleLights = renderingData.lightData.visibleLights;
					int additionalLightsShadowmapWidth = renderingData.shadowData.additionalLightsShadowmapWidth;
					int num = 0;
					this.m_ShadowResolutionRequests.Clear();
					if (this.m_VisibleLightIndexToAdditionalLightIndex.Length < visibleLights.Length)
					{
						this.m_VisibleLightIndexToAdditionalLightIndex = new int[visibleLights.Length];
						this.m_VisibleLightIndexToCameraSquareDistance = new float[visibleLights.Length];
						this.m_VisibleLightIndexToSortedShadowResolutionRequestsFirstSliceIndex = new int[visibleLights.Length];
					}
					int num2 = (this.m_UseStructuredBuffer ? visibleLights.Length : Math.Min(visibleLights.Length, UniversalRenderPipeline.maxVisibleAdditionalLights));
					if (this.m_AdditionalLightIndexToVisibleLightIndex.Length < num2)
					{
						this.m_AdditionalLightIndexToVisibleLightIndex = new int[num2];
						this.m_AdditionalLightIndexToShadowParams = new Vector4[num2];
					}
					for (int i = 0; i < this.m_VisibleLightIndexToCameraSquareDistance.Length; i++)
					{
						this.m_VisibleLightIndexToCameraSquareDistance[i] = float.MaxValue;
					}
					for (int j = 0; j < visibleLights.Length; j++)
					{
						if (j != renderingData.lightData.mainLightIndex && this.IsValidShadowCastingLight(ref renderingData.lightData, j))
						{
							ref VisibleLight ptr = ref visibleLights.UnsafeElementAt<VisibleLight>(j);
							LightType lightType = ptr.lightType;
							int punctualLightShadowSlicesCount = this.GetPunctualLightShadowSlicesCount(in lightType);
							num += punctualLightShadowSlicesCount;
							for (int k = 0; k < punctualLightShadowSlicesCount; k++)
							{
								this.m_ShadowResolutionRequests.Add(new AdditionalLightsShadowCasterPass.ShadowResolutionRequest(j, k, renderingData.shadowData.resolution[j], ptr.light.shadows == LightShadows.Soft, ptr.lightType == LightType.Point));
							}
							this.m_VisibleLightIndexToCameraSquareDistance[j] = (renderingData.cameraData.camera.transform.position - ptr.light.transform.position).sqrMagnitude;
						}
					}
					if (this.m_SortedShadowResolutionRequests == null || this.m_SortedShadowResolutionRequests.Length < num)
					{
						this.m_SortedShadowResolutionRequests = new AdditionalLightsShadowCasterPass.ShadowResolutionRequest[num];
					}
					for (int l = 0; l < this.m_ShadowResolutionRequests.Count; l++)
					{
						this.m_SortedShadowResolutionRequests[l] = this.m_ShadowResolutionRequests[l];
					}
					for (int m = num; m < this.m_SortedShadowResolutionRequests.Length; m++)
					{
						this.m_SortedShadowResolutionRequests[m].requestedResolution = 0;
					}
					this.InsertionSort(this.m_SortedShadowResolutionRequests, 0, num);
					int num3 = (this.m_UseStructuredBuffer ? num : Math.Min(num, UniversalRenderPipeline.maxVisibleAdditionalLights));
					bool flag2 = false;
					int num4 = 1;
					while (!flag2 && num3 > 0)
					{
						num4 = this.EstimateScaleFactorNeededToFitAllShadowsInAtlas(in this.m_SortedShadowResolutionRequests, num3, additionalLightsShadowmapWidth);
						if (this.m_SortedShadowResolutionRequests[num3 - 1].requestedResolution >= num4 * this.MinimalPunctualLightShadowResolution(this.m_SortedShadowResolutionRequests[num3 - 1].softShadow))
						{
							flag2 = true;
						}
						else
						{
							int num5 = num3;
							LightType lightType = (this.m_SortedShadowResolutionRequests[num3 - 1].pointLightShadow ? LightType.Point : LightType.Spot);
							num3 = num5 - this.GetPunctualLightShadowSlicesCount(in lightType);
						}
					}
					for (int n = num3; n < this.m_SortedShadowResolutionRequests.Length; n++)
					{
						this.m_SortedShadowResolutionRequests[n].requestedResolution = 0;
					}
					for (int num6 = 0; num6 < this.m_VisibleLightIndexToSortedShadowResolutionRequestsFirstSliceIndex.Length; num6++)
					{
						this.m_VisibleLightIndexToSortedShadowResolutionRequestsFirstSliceIndex[num6] = -1;
					}
					for (int num7 = num3 - 1; num7 >= 0; num7--)
					{
						this.m_VisibleLightIndexToSortedShadowResolutionRequestsFirstSliceIndex[this.m_SortedShadowResolutionRequests[num7].visibleLightIndex] = num7;
					}
					this.AtlasLayout(additionalLightsShadowmapWidth, num3, num4);
					if (this.m_AdditionalLightsShadowSlices == null || this.m_AdditionalLightsShadowSlices.Length < num3)
					{
						this.m_AdditionalLightsShadowSlices = new ShadowSliceData[num3];
					}
					if (this.m_AdditionalLightShadowSliceIndexTo_WorldShadowMatrix == null || (this.m_UseStructuredBuffer && this.m_AdditionalLightShadowSliceIndexTo_WorldShadowMatrix.Length < num3))
					{
						this.m_AdditionalLightShadowSliceIndexTo_WorldShadowMatrix = new Matrix4x4[num3];
					}
					for (int num8 = 0; num8 < num2; num8++)
					{
						this.m_AdditionalLightIndexToShadowParams[num8] = AdditionalLightsShadowCasterPass.c_DefaultShadowParams;
					}
					int num9 = 0;
					bool supportsSoftShadows = renderingData.shadowData.supportsSoftShadows;
					int num10 = 0;
					for (int num11 = 0; num11 < visibleLights.Length; num11++)
					{
						ref VisibleLight ptr2 = ref visibleLights.UnsafeElementAt<VisibleLight>(num11);
						if (num11 == renderingData.lightData.mainLightIndex)
						{
							this.m_VisibleLightIndexToAdditionalLightIndex[num11] = -1;
						}
						else
						{
							int num12 = num10++;
							if (num12 < this.m_AdditionalLightIndexToVisibleLightIndex.Length)
							{
								this.m_AdditionalLightIndexToVisibleLightIndex[num12] = num11;
								this.m_VisibleLightIndexToAdditionalLightIndex[num11] = num12;
								if (this.m_ShadowSliceToAdditionalLightIndex.Count < num3 && num12 < num2)
								{
									LightType lightType2 = ptr2.lightType;
									int punctualLightShadowSlicesCount2 = this.GetPunctualLightShadowSlicesCount(in lightType2);
									if (this.m_ShadowSliceToAdditionalLightIndex.Count + punctualLightShadowSlicesCount2 > num3 && this.IsValidShadowCastingLight(ref renderingData.lightData, num11))
									{
										break;
									}
									int count = this.m_ShadowSliceToAdditionalLightIndex.Count;
									bool flag3 = false;
									for (int num13 = 0; num13 < punctualLightShadowSlicesCount2; num13++)
									{
										int count2 = this.m_ShadowSliceToAdditionalLightIndex.Count;
										Bounds bounds;
										if (renderingData.cullResults.GetShadowCasterBounds(num11, out bounds) && renderingData.shadowData.supportsAdditionalLightShadows && this.IsValidShadowCastingLight(ref renderingData.lightData, num11) && this.m_VisibleLightIndexToSortedShadowResolutionRequestsFirstSliceIndex[num11] != -1)
										{
											if (lightType2 == LightType.Spot)
											{
												Matrix4x4 matrix4x;
												if (ShadowUtils.ExtractSpotLightMatrix(ref renderingData.cullResults, ref renderingData.shadowData, num11, out matrix4x, out this.m_AdditionalLightsShadowSlices[count2].viewMatrix, out this.m_AdditionalLightsShadowSlices[count2].projectionMatrix, out this.m_AdditionalLightsShadowSlices[count2].splitData))
												{
													this.m_ShadowSliceToAdditionalLightIndex.Add(num12);
													this.m_GlobalShadowSliceIndexToPerLightShadowSliceIndex.Add(num13);
													Light light = ptr2.light;
													float shadowStrength = light.shadowStrength;
													float num14 = ShadowUtils.SoftShadowQualityToShaderProperty(light, supportsSoftShadows && light.shadows == LightShadows.Soft);
													Vector4 vector = new Vector4(shadowStrength, num14, 0f, (float)count);
													this.m_AdditionalLightShadowSliceIndexTo_WorldShadowMatrix[count2] = matrix4x;
													this.m_AdditionalLightIndexToShadowParams[num12] = vector;
													flag3 = true;
												}
											}
											else if (lightType2 == LightType.Point)
											{
												float pointLightShadowFrustumFovBiasInDegrees = AdditionalLightsShadowCasterPass.GetPointLightShadowFrustumFovBiasInDegrees(this.m_SortedShadowResolutionRequests[this.m_VisibleLightIndexToSortedShadowResolutionRequestsFirstSliceIndex[num11]].allocatedResolution, ptr2.light.shadows == LightShadows.Soft);
												Matrix4x4 matrix4x2;
												if (ShadowUtils.ExtractPointLightMatrix(ref renderingData.cullResults, ref renderingData.shadowData, num11, (CubemapFace)num13, pointLightShadowFrustumFovBiasInDegrees, out matrix4x2, out this.m_AdditionalLightsShadowSlices[count2].viewMatrix, out this.m_AdditionalLightsShadowSlices[count2].projectionMatrix, out this.m_AdditionalLightsShadowSlices[count2].splitData))
												{
													this.m_ShadowSliceToAdditionalLightIndex.Add(num12);
													this.m_GlobalShadowSliceIndexToPerLightShadowSliceIndex.Add(num13);
													Light light2 = ptr2.light;
													float shadowStrength2 = light2.shadowStrength;
													float num15 = ShadowUtils.SoftShadowQualityToShaderProperty(light2, supportsSoftShadows && light2.shadows == LightShadows.Soft);
													Vector4 vector2 = new Vector4(shadowStrength2, num15, 1f, (float)count);
													this.m_AdditionalLightShadowSliceIndexTo_WorldShadowMatrix[count2] = matrix4x2;
													this.m_AdditionalLightIndexToShadowParams[num12] = vector2;
													flag3 = true;
												}
											}
										}
									}
									if (flag3)
									{
										num9++;
									}
								}
							}
						}
					}
					if (num9 == 0)
					{
						flag = this.SetupForEmptyRendering(ref renderingData);
					}
					else
					{
						int count3 = this.m_ShadowSliceToAdditionalLightIndex.Count;
						int num16 = 0;
						int num17 = 0;
						for (int num18 = 0; num18 < num3; num18++)
						{
							AdditionalLightsShadowCasterPass.ShadowResolutionRequest shadowResolutionRequest = this.m_SortedShadowResolutionRequests[num18];
							num16 = Mathf.Max(num16, shadowResolutionRequest.offsetX + shadowResolutionRequest.allocatedResolution);
							num17 = Mathf.Max(num17, shadowResolutionRequest.offsetY + shadowResolutionRequest.allocatedResolution);
						}
						this.renderTargetWidth = Mathf.NextPowerOfTwo(num16);
						this.renderTargetHeight = Mathf.NextPowerOfTwo(num17);
						float num19 = 1f / (float)this.renderTargetWidth;
						float num20 = 1f / (float)this.renderTargetHeight;
						for (int num21 = 0; num21 < count3; num21++)
						{
							int num22 = this.m_ShadowSliceToAdditionalLightIndex[num21];
							if (!Mathf.Approximately(this.m_AdditionalLightIndexToShadowParams[num22].x, 0f) && !Mathf.Approximately(this.m_AdditionalLightIndexToShadowParams[num22].w, -1f))
							{
								int num23 = this.m_AdditionalLightIndexToVisibleLightIndex[num22];
								int num24 = this.m_VisibleLightIndexToSortedShadowResolutionRequestsFirstSliceIndex[num23];
								int num25 = this.m_GlobalShadowSliceIndexToPerLightShadowSliceIndex[num21];
								AdditionalLightsShadowCasterPass.ShadowResolutionRequest shadowResolutionRequest2 = this.m_SortedShadowResolutionRequests[num24 + num25];
								int allocatedResolution = shadowResolutionRequest2.allocatedResolution;
								Matrix4x4 identity = Matrix4x4.identity;
								identity.m00 = (float)allocatedResolution * num19;
								identity.m11 = (float)allocatedResolution * num20;
								this.m_AdditionalLightsShadowSlices[num21].offsetX = shadowResolutionRequest2.offsetX;
								this.m_AdditionalLightsShadowSlices[num21].offsetY = shadowResolutionRequest2.offsetY;
								this.m_AdditionalLightsShadowSlices[num21].resolution = allocatedResolution;
								identity.m03 = (float)this.m_AdditionalLightsShadowSlices[num21].offsetX * num19;
								identity.m13 = (float)this.m_AdditionalLightsShadowSlices[num21].offsetY * num20;
								this.m_AdditionalLightShadowSliceIndexTo_WorldShadowMatrix[num21] = identity * this.m_AdditionalLightShadowSliceIndexTo_WorldShadowMatrix[num21];
							}
						}
						ShadowUtils.ShadowRTReAllocateIfNeeded(ref this.m_AdditionalLightsShadowmapHandle, this.renderTargetWidth, this.renderTargetHeight, 16, 1, 0f, "_AdditionalLightsShadowmapTexture");
						this.m_MaxShadowDistanceSq = renderingData.cameraData.maxShadowDistance * renderingData.cameraData.maxShadowDistance;
						this.m_CascadeBorder = renderingData.shadowData.mainLightShadowCascadeBorder;
						this.m_CreateEmptyShadowmap = false;
						base.useNativeRenderPass = true;
						flag = true;
					}
				}
			}
			return flag;
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x0003C138 File Offset: 0x0003A338
		private bool SetupForEmptyRendering(ref RenderingData renderingData)
		{
			if (!renderingData.cameraData.renderer.stripShadowsOffVariants)
			{
				return false;
			}
			renderingData.shadowData.isKeywordAdditionalLightShadowsEnabled = true;
			ShadowUtils.ShadowRTReAllocateIfNeeded(ref this.m_EmptyAdditionalLightShadowmapTexture, 1, 1, 16, 1, 0f, "_EmptyAdditionalLightShadowmapTexture");
			this.m_CreateEmptyShadowmap = true;
			base.useNativeRenderPass = false;
			for (int i = 0; i < this.m_AdditionalLightIndexToShadowParams.Length; i++)
			{
				this.m_AdditionalLightIndexToShadowParams[i] = AdditionalLightsShadowCasterPass.c_DefaultShadowParams;
			}
			return true;
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x0003C1B3 File Offset: 0x0003A3B3
		public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
		{
			if (this.m_CreateEmptyShadowmap)
			{
				base.ResetTarget();
				return;
			}
			base.ConfigureTarget(this.m_AdditionalLightsShadowmapHandle);
			base.ConfigureClear(ClearFlag.All, Color.black);
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x0003C1DC File Offset: 0x0003A3DC
		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			if (this.m_CreateEmptyShadowmap)
			{
				this.SetEmptyAdditionalShadowmapAtlas(ref context, ref renderingData);
				renderingData.commandBuffer.SetGlobalTexture(this.m_AdditionalLightsShadowmapID, this.m_EmptyAdditionalLightShadowmapTexture);
				return;
			}
			if (renderingData.shadowData.supportsAdditionalLightShadows)
			{
				this.RenderAdditionalShadowmapAtlas(ref context, ref renderingData);
				renderingData.commandBuffer.SetGlobalTexture(this.m_AdditionalLightsShadowmapID, this.m_AdditionalLightsShadowmapHandle.nameID);
			}
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x0003C249 File Offset: 0x0003A449
		public int GetShadowLightIndexFromLightIndex(int visibleLightIndex)
		{
			if (visibleLightIndex < 0 || visibleLightIndex >= this.m_VisibleLightIndexToAdditionalLightIndex.Length)
			{
				return -1;
			}
			return this.m_VisibleLightIndexToAdditionalLightIndex[visibleLightIndex];
		}

		// Token: 0x06000934 RID: 2356 RVA: 0x0003C264 File Offset: 0x0003A464
		private void Clear()
		{
			this.m_ShadowSliceToAdditionalLightIndex.Clear();
			this.m_GlobalShadowSliceIndexToPerLightShadowSliceIndex.Clear();
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x0003C27C File Offset: 0x0003A47C
		private void SetEmptyAdditionalShadowmapAtlas(ref ScriptableRenderContext context, ref RenderingData renderingData)
		{
			CommandBuffer commandBuffer = renderingData.commandBuffer;
			CoreUtils.SetKeyword(commandBuffer, "_ADDITIONAL_LIGHT_SHADOWS", true);
			AdditionalLightsShadowCasterPass.SetEmptyAdditionalLightShadowParams(commandBuffer, this.m_AdditionalLightIndexToShadowParams);
			context.ExecuteCommandBuffer(commandBuffer);
			commandBuffer.Clear();
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x0003C2B8 File Offset: 0x0003A4B8
		internal static void SetEmptyAdditionalLightShadowParams(CommandBuffer cmd, Vector4[] lightIndexToShadowParams)
		{
			if (RenderingUtils.useStructuredBuffer)
			{
				ComputeBuffer additionalLightShadowParamsStructuredBuffer = ShaderData.instance.GetAdditionalLightShadowParamsStructuredBuffer(lightIndexToShadowParams.Length);
				additionalLightShadowParamsStructuredBuffer.SetData(lightIndexToShadowParams);
				cmd.SetGlobalBuffer(AdditionalLightsShadowCasterPass.m_AdditionalShadowParams_SSBO, additionalLightShadowParamsStructuredBuffer);
				return;
			}
			cmd.SetGlobalVectorArray(AdditionalLightsShadowCasterPass.AdditionalShadowsConstantBuffer._AdditionalShadowParams, lightIndexToShadowParams);
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x0003C2FC File Offset: 0x0003A4FC
		private void RenderAdditionalShadowmapAtlas(ref ScriptableRenderContext context, ref RenderingData renderingData)
		{
			CullingResults cullResults = renderingData.cullResults;
			LightData lightData = renderingData.lightData;
			NativeArray<VisibleLight> visibleLights = lightData.visibleLights;
			bool flag = false;
			CommandBuffer commandBuffer = renderingData.commandBuffer;
			using (new ProfilingScope(commandBuffer, ProfilingSampler.Get<URPProfileId>(URPProfileId.AdditionalLightsShadow)))
			{
				ShadowUtils.SetWorldToCameraAndCameraToWorldMatrices(commandBuffer, renderingData.cameraData.GetViewMatrix(0));
				bool flag2 = false;
				int count = this.m_ShadowSliceToAdditionalLightIndex.Count;
				for (int i = 0; i < count; i++)
				{
					int num = this.m_ShadowSliceToAdditionalLightIndex[i];
					if (!Mathf.Approximately(this.m_AdditionalLightIndexToShadowParams[num].x, 0f) && !Mathf.Approximately(this.m_AdditionalLightIndexToShadowParams[num].w, -1f))
					{
						int num2 = this.m_AdditionalLightIndexToVisibleLightIndex[num];
						ref VisibleLight ptr = ref visibleLights.UnsafeElementAt<VisibleLight>(num2);
						ShadowSliceData shadowSliceData = this.m_AdditionalLightsShadowSlices[i];
						ShadowDrawingSettings shadowDrawingSettings = new ShadowDrawingSettings(cullResults, num2, BatchCullingProjectionType.Perspective);
						shadowDrawingSettings.useRenderingLayerMaskTest = UniversalRenderPipeline.asset.useRenderingLayers;
						shadowDrawingSettings.splitData = shadowSliceData.splitData;
						Vector4 shadowBias = ShadowUtils.GetShadowBias(ref ptr, num2, ref renderingData.shadowData, shadowSliceData.projectionMatrix, (float)shadowSliceData.resolution);
						ShadowUtils.SetupShadowCasterConstantBuffer(commandBuffer, ref ptr, shadowBias);
						CoreUtils.SetKeyword(commandBuffer, "_CASTING_PUNCTUAL_LIGHT_SHADOW", true);
						ShadowUtils.RenderShadowSlice(commandBuffer, ref context, ref shadowSliceData, ref shadowDrawingSettings);
						flag |= ptr.light.shadows == LightShadows.Soft;
						flag2 = true;
					}
				}
				bool flag3 = renderingData.shadowData.supportsMainLightShadows && lightData.mainLightIndex != -1 && visibleLights[lightData.mainLightIndex].light.shadows == LightShadows.Soft;
				bool flag4 = !renderingData.cameraData.renderer.stripShadowsOffVariants;
				renderingData.shadowData.isKeywordAdditionalLightShadowsEnabled = !flag4 || flag2;
				CoreUtils.SetKeyword(commandBuffer, "_ADDITIONAL_LIGHT_SHADOWS", renderingData.shadowData.isKeywordAdditionalLightShadowsEnabled);
				bool flag5 = renderingData.shadowData.supportsSoftShadows && (flag3 || flag);
				renderingData.shadowData.isKeywordSoftShadowsEnabled = flag5;
				ShadowUtils.SetSoftShadowQualityShaderKeywords(commandBuffer, ref renderingData.shadowData);
				if (flag2)
				{
					this.SetupAdditionalLightsShadowReceiverConstants(commandBuffer, flag5);
				}
			}
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x0003C54C File Offset: 0x0003A74C
		private void SetupAdditionalLightsShadowReceiverConstants(CommandBuffer cmd, bool softShadows)
		{
			if (this.m_UseStructuredBuffer)
			{
				ComputeBuffer additionalLightShadowParamsStructuredBuffer = ShaderData.instance.GetAdditionalLightShadowParamsStructuredBuffer(this.m_AdditionalLightIndexToShadowParams.Length);
				additionalLightShadowParamsStructuredBuffer.SetData(this.m_AdditionalLightIndexToShadowParams);
				cmd.SetGlobalBuffer(AdditionalLightsShadowCasterPass.m_AdditionalShadowParams_SSBO, additionalLightShadowParamsStructuredBuffer);
				ComputeBuffer additionalLightShadowSliceMatricesStructuredBuffer = ShaderData.instance.GetAdditionalLightShadowSliceMatricesStructuredBuffer(this.m_AdditionalLightShadowSliceIndexTo_WorldShadowMatrix.Length);
				additionalLightShadowSliceMatricesStructuredBuffer.SetData(this.m_AdditionalLightShadowSliceIndexTo_WorldShadowMatrix);
				cmd.SetGlobalBuffer(AdditionalLightsShadowCasterPass.m_AdditionalLightsWorldToShadow_SSBO, additionalLightShadowSliceMatricesStructuredBuffer);
			}
			else
			{
				cmd.SetGlobalVectorArray(AdditionalLightsShadowCasterPass.AdditionalShadowsConstantBuffer._AdditionalShadowParams, this.m_AdditionalLightIndexToShadowParams);
				cmd.SetGlobalMatrixArray(AdditionalLightsShadowCasterPass.AdditionalShadowsConstantBuffer._AdditionalLightsWorldToShadow, this.m_AdditionalLightShadowSliceIndexTo_WorldShadowMatrix);
			}
			float num;
			float num2;
			ShadowUtils.GetScaleAndBiasForLinearDistanceFade(this.m_MaxShadowDistanceSq, this.m_CascadeBorder, out num, out num2);
			cmd.SetGlobalVector(AdditionalLightsShadowCasterPass.AdditionalShadowsConstantBuffer._AdditionalShadowFadeParams, new Vector4(num, num2, 0f, 0f));
			if (softShadows)
			{
				Vector2Int referenceSize = this.m_AdditionalLightsShadowmapHandle.referenceSize;
				Vector2 vector = Vector2.one / referenceSize;
				Vector2 vector2 = vector * 0.5f;
				cmd.SetGlobalVector(AdditionalLightsShadowCasterPass.AdditionalShadowsConstantBuffer._AdditionalShadowOffset0, new Vector4(-vector2.x, -vector2.y, vector2.x, -vector2.y));
				cmd.SetGlobalVector(AdditionalLightsShadowCasterPass.AdditionalShadowsConstantBuffer._AdditionalShadowOffset1, new Vector4(-vector2.x, vector2.y, vector2.x, vector2.y));
				cmd.SetGlobalVector(AdditionalLightsShadowCasterPass.AdditionalShadowsConstantBuffer._AdditionalShadowmapSize, new Vector4(vector.x, vector.y, (float)referenceSize.x, (float)referenceSize.y));
			}
		}

		// Token: 0x06000939 RID: 2361 RVA: 0x0003C6CC File Offset: 0x0003A8CC
		private bool IsValidShadowCastingLight(ref LightData lightData, int i)
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

		// Token: 0x0600093A RID: 2362 RVA: 0x0003C72C File Offset: 0x0003A92C
		internal TextureHandle Render(RenderGraph graph, ref RenderingData renderingData)
		{
			AdditionalLightsShadowCasterPass.PassData passData;
			TextureHandle shadowmapTexture;
			using (RenderGraphBuilder renderGraphBuilder = graph.AddRenderPass<AdditionalLightsShadowCasterPass.PassData>("Additional Lights Shadowmap", out passData, base.profilingSampler))
			{
				this.InitPassData(ref passData, ref renderingData, ref graph);
				if (!this.m_CreateEmptyShadowmap)
				{
					passData.shadowmapTexture = UniversalRenderer.CreateRenderGraphTexture(graph, this.m_AdditionalLightsShadowmapHandle.rt.descriptor, "Additional Shadowmap", true, ShadowUtils.m_ForceShadowPointSampling ? FilterMode.Point : FilterMode.Bilinear, TextureWrapMode.Clamp);
					renderGraphBuilder.UseDepthBuffer(in passData.shadowmapTexture, DepthAccess.Write);
				}
				renderGraphBuilder.AllowPassCulling(false);
				renderGraphBuilder.SetRenderFunc<AdditionalLightsShadowCasterPass.PassData>(delegate(AdditionalLightsShadowCasterPass.PassData data, RenderGraphContext context)
				{
					if (!data.emptyShadowmap)
					{
						data.pass.RenderAdditionalShadowmapAtlas(ref context.renderContext, ref data.renderingData);
					}
				});
				shadowmapTexture = passData.shadowmapTexture;
			}
			AdditionalLightsShadowCasterPass.PassData passData2;
			TextureHandle shadowmapTexture2;
			using (RenderGraphBuilder renderGraphBuilder2 = graph.AddRenderPass<AdditionalLightsShadowCasterPass.PassData>("Set Additional Shadow Globals", out passData2, base.profilingSampler))
			{
				this.InitPassData(ref passData2, ref renderingData, ref graph);
				passData2.shadowmapTexture = shadowmapTexture;
				if (shadowmapTexture.IsValid())
				{
					renderGraphBuilder2.UseDepthBuffer(in shadowmapTexture, DepthAccess.Read);
				}
				renderGraphBuilder2.AllowPassCulling(false);
				renderGraphBuilder2.SetRenderFunc<AdditionalLightsShadowCasterPass.PassData>(delegate(AdditionalLightsShadowCasterPass.PassData data, RenderGraphContext context)
				{
					if (data.emptyShadowmap)
					{
						data.pass.SetEmptyAdditionalShadowmapAtlas(ref context.renderContext, ref data.renderingData);
						data.shadowmapTexture = data.graph.defaultResources.defaultShadowTexture;
					}
					data.renderingData.commandBuffer.SetGlobalTexture(data.shadowmapID, data.shadowmapTexture);
				});
				shadowmapTexture2 = passData2.shadowmapTexture;
			}
			return shadowmapTexture2;
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x0003C884 File Offset: 0x0003AA84
		private void InitPassData(ref AdditionalLightsShadowCasterPass.PassData passData, ref RenderingData renderingData, ref RenderGraph graph)
		{
			passData.pass = this;
			passData.graph = graph;
			passData.emptyShadowmap = this.m_CreateEmptyShadowmap;
			passData.shadowmapID = this.m_AdditionalLightsShadowmapID;
			passData.renderingData = renderingData;
		}

		// Token: 0x04000828 RID: 2088
		[Obsolete("AdditionalLightsShadowCasterPass.m_AdditionalShadowsBufferId was deprecated. Shadow slice matrix is now passed to the GPU using an entry in buffer m_AdditionalLightsWorldToShadow_SSBO", false)]
		public static int m_AdditionalShadowsBufferId;

		// Token: 0x04000829 RID: 2089
		[Obsolete("AdditionalLightsShadowCasterPass.m_AdditionalShadowsIndicesId was deprecated. Shadow slice index is now passed to the GPU using last member of an entry in buffer m_AdditionalShadowParams_SSBO", false)]
		public static int m_AdditionalShadowsIndicesId;

		// Token: 0x0400082A RID: 2090
		private static readonly Vector4 c_DefaultShadowParams = new Vector4(0f, 0f, 0f, -1f);

		// Token: 0x0400082B RID: 2091
		private static int m_AdditionalLightsWorldToShadow_SSBO;

		// Token: 0x0400082C RID: 2092
		private static int m_AdditionalShadowParams_SSBO;

		// Token: 0x0400082D RID: 2093
		private bool m_UseStructuredBuffer;

		// Token: 0x0400082E RID: 2094
		private const int k_ShadowmapBufferBits = 16;

		// Token: 0x0400082F RID: 2095
		private int m_AdditionalLightsShadowmapID;

		// Token: 0x04000830 RID: 2096
		internal RTHandle m_AdditionalLightsShadowmapHandle;

		// Token: 0x04000831 RID: 2097
		private bool m_CreateEmptyShadowmap;

		// Token: 0x04000832 RID: 2098
		private RTHandle m_EmptyAdditionalLightShadowmapTexture;

		// Token: 0x04000833 RID: 2099
		private const int k_EmptyShadowMapDimensions = 1;

		// Token: 0x04000834 RID: 2100
		private const string k_AdditionalLightShadowMapTextureName = "_AdditionalLightsShadowmapTexture";

		// Token: 0x04000835 RID: 2101
		private const string k_EmptyAdditionalLightShadowMapTextureName = "_EmptyAdditionalLightShadowmapTexture";

		// Token: 0x04000836 RID: 2102
		internal static Vector4[] s_EmptyAdditionalLightIndexToShadowParams = null;

		// Token: 0x04000837 RID: 2103
		private float m_MaxShadowDistanceSq;

		// Token: 0x04000838 RID: 2104
		private float m_CascadeBorder;

		// Token: 0x04000839 RID: 2105
		private ShadowSliceData[] m_AdditionalLightsShadowSlices;

		// Token: 0x0400083A RID: 2106
		private int[] m_VisibleLightIndexToAdditionalLightIndex;

		// Token: 0x0400083B RID: 2107
		private int[] m_AdditionalLightIndexToVisibleLightIndex;

		// Token: 0x0400083C RID: 2108
		private List<int> m_ShadowSliceToAdditionalLightIndex = new List<int>();

		// Token: 0x0400083D RID: 2109
		private List<int> m_GlobalShadowSliceIndexToPerLightShadowSliceIndex = new List<int>();

		// Token: 0x0400083E RID: 2110
		private Vector4[] m_AdditionalLightIndexToShadowParams;

		// Token: 0x0400083F RID: 2111
		private Matrix4x4[] m_AdditionalLightShadowSliceIndexTo_WorldShadowMatrix;

		// Token: 0x04000840 RID: 2112
		private List<AdditionalLightsShadowCasterPass.ShadowResolutionRequest> m_ShadowResolutionRequests = new List<AdditionalLightsShadowCasterPass.ShadowResolutionRequest>();

		// Token: 0x04000841 RID: 2113
		private float[] m_VisibleLightIndexToCameraSquareDistance;

		// Token: 0x04000842 RID: 2114
		private AdditionalLightsShadowCasterPass.ShadowResolutionRequest[] m_SortedShadowResolutionRequests;

		// Token: 0x04000843 RID: 2115
		private int[] m_VisibleLightIndexToSortedShadowResolutionRequestsFirstSliceIndex;

		// Token: 0x04000844 RID: 2116
		private List<RectInt> m_UnusedAtlasSquareAreas = new List<RectInt>();

		// Token: 0x04000845 RID: 2117
		private int renderTargetWidth;

		// Token: 0x04000846 RID: 2118
		private int renderTargetHeight;

		// Token: 0x04000847 RID: 2119
		private ProfilingSampler m_ProfilingSetupSampler = new ProfilingSampler("Setup Additional Shadows");

		// Token: 0x04000848 RID: 2120
		private const float LightTypeIdentifierInShadowParams_Spot = 0f;

		// Token: 0x04000849 RID: 2121
		private const float LightTypeIdentifierInShadowParams_Point = 1f;

		// Token: 0x0400084A RID: 2122
		private const int kMinimumPunctualLightHardShadowResolution = 8;

		// Token: 0x0400084B RID: 2123
		private const int kMinimumPunctualLightSoftShadowResolution = 16;

		// Token: 0x0400084C RID: 2124
		private Dictionary<int, ulong> m_ShadowRequestsHashes = new Dictionary<int, ulong>();

		// Token: 0x020001D9 RID: 473
		private static class AdditionalShadowsConstantBuffer
		{
			// Token: 0x04000C41 RID: 3137
			public static int _AdditionalLightsWorldToShadow;

			// Token: 0x04000C42 RID: 3138
			public static int _AdditionalShadowParams;

			// Token: 0x04000C43 RID: 3139
			public static int _AdditionalShadowOffset0;

			// Token: 0x04000C44 RID: 3140
			public static int _AdditionalShadowOffset1;

			// Token: 0x04000C45 RID: 3141
			public static int _AdditionalShadowFadeParams;

			// Token: 0x04000C46 RID: 3142
			public static int _AdditionalShadowmapSize;
		}

		// Token: 0x020001DA RID: 474
		internal struct ShadowResolutionRequest
		{
			// Token: 0x06000B7A RID: 2938 RVA: 0x0004C1B9 File Offset: 0x0004A3B9
			public ShadowResolutionRequest(int _visibleLightIndex, int _perLightShadowSliceIndex, int _requestedResolution, bool _softShadow, bool _pointLightShadow)
			{
				this.visibleLightIndex = _visibleLightIndex;
				this.perLightShadowSliceIndex = _perLightShadowSliceIndex;
				this.requestedResolution = _requestedResolution;
				this.softShadow = _softShadow;
				this.pointLightShadow = _pointLightShadow;
				this.offsetX = 0;
				this.offsetY = 0;
				this.allocatedResolution = 0;
			}

			// Token: 0x04000C47 RID: 3143
			public int visibleLightIndex;

			// Token: 0x04000C48 RID: 3144
			public int perLightShadowSliceIndex;

			// Token: 0x04000C49 RID: 3145
			public int requestedResolution;

			// Token: 0x04000C4A RID: 3146
			public bool softShadow;

			// Token: 0x04000C4B RID: 3147
			public bool pointLightShadow;

			// Token: 0x04000C4C RID: 3148
			public int offsetX;

			// Token: 0x04000C4D RID: 3149
			public int offsetY;

			// Token: 0x04000C4E RID: 3150
			public int allocatedResolution;
		}

		// Token: 0x020001DB RID: 475
		private class PassData
		{
			// Token: 0x04000C4F RID: 3151
			internal AdditionalLightsShadowCasterPass pass;

			// Token: 0x04000C50 RID: 3152
			internal RenderGraph graph;

			// Token: 0x04000C51 RID: 3153
			internal TextureHandle shadowmapTexture;

			// Token: 0x04000C52 RID: 3154
			internal RenderingData renderingData;

			// Token: 0x04000C53 RID: 3155
			internal int shadowmapID;

			// Token: 0x04000C54 RID: 3156
			internal bool emptyShadowmap;
		}
	}
}
