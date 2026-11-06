using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Experimental.Rendering;

namespace UnityEngine.Rendering
{
	// Token: 0x020000A0 RID: 160
	public sealed class LensFlareCommonSRP
	{
		// Token: 0x0600050D RID: 1293 RVA: 0x0001910E File Offset: 0x0001730E
		private LensFlareCommonSRP()
		{
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x00019116 File Offset: 0x00017316
		public static bool IsOcclusionRTCompatible()
		{
			return SystemInfo.graphicsDeviceType != GraphicsDeviceType.OpenGLES3 && SystemInfo.graphicsDeviceType != GraphicsDeviceType.OpenGLCore && SystemInfo.graphicsDeviceType != GraphicsDeviceType.Null && LensFlareCommonSRP.s_SupportsLensFlareTexFormat;
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x0001913C File Offset: 0x0001733C
		public static void Initialize()
		{
			LensFlareCommonSRP.frameIdx = 0;
			if (LensFlareCommonSRP.IsOcclusionRTCompatible() && LensFlareCommonSRP.occlusionRT == null)
			{
				LensFlareCommonSRP.occlusionRT = RTHandles.Alloc(LensFlareCommonSRP.maxLensFlareWithOcclusion, Mathf.Max(LensFlareCommonSRP.mergeNeeded * (LensFlareCommonSRP.maxLensFlareWithOcclusionTemporalSample + 1), 1), TextureXR.slices, DepthBits.None, GraphicsFormat.R32_SFloat, FilterMode.Point, TextureWrapMode.Repeat, TextureDimension.Tex2DArray, true, false, true, false, 1, 0f, MSAASamples.None, false, false, RenderTextureMemoryless.None, VRTextureUsage.None, "");
			}
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x0001919E File Offset: 0x0001739E
		public static void Dispose()
		{
			if (LensFlareCommonSRP.IsOcclusionRTCompatible() && LensFlareCommonSRP.occlusionRT != null)
			{
				RTHandles.Release(LensFlareCommonSRP.occlusionRT);
				LensFlareCommonSRP.occlusionRT = null;
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000511 RID: 1297 RVA: 0x000191C0 File Offset: 0x000173C0
		public static LensFlareCommonSRP Instance
		{
			get
			{
				if (LensFlareCommonSRP.m_Instance == null)
				{
					object padlock = LensFlareCommonSRP.m_Padlock;
					lock (padlock)
					{
						if (LensFlareCommonSRP.m_Instance == null)
						{
							LensFlareCommonSRP.m_Instance = new LensFlareCommonSRP();
						}
					}
				}
				return LensFlareCommonSRP.m_Instance;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000512 RID: 1298 RVA: 0x00019218 File Offset: 0x00017418
		private List<LensFlareCommonSRP.LensFlareCompInfo> Data
		{
			get
			{
				return LensFlareCommonSRP.m_Data;
			}
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x0001921F File Offset: 0x0001741F
		public bool IsEmpty()
		{
			return this.Data.Count == 0;
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x00019230 File Offset: 0x00017430
		private int GetNextAvailableIndex()
		{
			if (LensFlareCommonSRP.m_AvailableIndicies.Count == 0)
			{
				return LensFlareCommonSRP.m_Data.Count;
			}
			int num = LensFlareCommonSRP.m_AvailableIndicies[LensFlareCommonSRP.m_AvailableIndicies.Count - 1];
			LensFlareCommonSRP.m_AvailableIndicies.RemoveAt(LensFlareCommonSRP.m_AvailableIndicies.Count - 1);
			return num;
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x00019280 File Offset: 0x00017480
		public void AddData(LensFlareComponentSRP newData)
		{
			if (!LensFlareCommonSRP.m_Data.Exists((LensFlareCommonSRP.LensFlareCompInfo x) => x.comp == newData))
			{
				LensFlareCommonSRP.m_Data.Add(new LensFlareCommonSRP.LensFlareCompInfo(this.GetNextAvailableIndex(), newData));
			}
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x000192D0 File Offset: 0x000174D0
		public void RemoveData(LensFlareComponentSRP data)
		{
			LensFlareCommonSRP.LensFlareCompInfo lensFlareCompInfo = LensFlareCommonSRP.m_Data.Find((LensFlareCommonSRP.LensFlareCompInfo x) => x.comp == data);
			if (lensFlareCompInfo != null)
			{
				int index = lensFlareCompInfo.index;
				LensFlareCommonSRP.m_Data.Remove(lensFlareCompInfo);
				LensFlareCommonSRP.m_AvailableIndicies.Add(index);
				if (LensFlareCommonSRP.m_Data.Count == 0)
				{
					LensFlareCommonSRP.m_AvailableIndicies.Clear();
				}
			}
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x00019338 File Offset: 0x00017538
		public static float ShapeAttenuationPointLight()
		{
			return 1f;
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x0001933F File Offset: 0x0001753F
		public static float ShapeAttenuationDirLight(Vector3 forward, Vector3 wo)
		{
			return Mathf.Max(Vector3.Dot(forward, wo), 0f);
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x00019354 File Offset: 0x00017554
		public static float ShapeAttenuationSpotConeLight(Vector3 forward, Vector3 wo, float spotAngle, float innerSpotPercent01)
		{
			float num = Mathf.Max(Mathf.Cos(0.5f * spotAngle * 0.017453292f), 0f);
			float num2 = Mathf.Max(Mathf.Cos(0.5f * spotAngle * 0.017453292f * innerSpotPercent01), 0f);
			return Mathf.Clamp01((Mathf.Max(Vector3.Dot(forward, wo), 0f) - num) / (num2 - num));
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x000193B9 File Offset: 0x000175B9
		public static float ShapeAttenuationSpotBoxLight(Vector3 forward, Vector3 wo)
		{
			return Mathf.Max(Mathf.Sign(Vector3.Dot(forward, wo)), 0f);
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x000193D1 File Offset: 0x000175D1
		public static float ShapeAttenuationSpotPyramidLight(Vector3 forward, Vector3 wo)
		{
			return LensFlareCommonSRP.ShapeAttenuationSpotBoxLight(forward, wo);
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x000193DC File Offset: 0x000175DC
		public static float ShapeAttenuationAreaTubeLight(Vector3 lightPositionWS, Vector3 lightSide, float lightWidth, Camera cam)
		{
			Vector3 vector = lightPositionWS + lightSide * lightWidth * 0.5f;
			Vector3 vector2 = lightPositionWS - lightSide * lightWidth * 0.5f;
			Vector3 vector3 = lightPositionWS + cam.transform.right * lightWidth * 0.5f;
			Vector3 vector4 = lightPositionWS - cam.transform.right * lightWidth * 0.5f;
			Vector3 vector5 = cam.transform.InverseTransformPoint(vector);
			Vector3 vector6 = cam.transform.InverseTransformPoint(vector2);
			Vector3 vector7 = cam.transform.InverseTransformPoint(vector3);
			Vector3 vector8 = cam.transform.InverseTransformPoint(vector4);
			float num = LensFlareCommonSRP.<ShapeAttenuationAreaTubeLight>g__DiffLineIntegral|28_2(vector7, vector8);
			float num2 = LensFlareCommonSRP.<ShapeAttenuationAreaTubeLight>g__DiffLineIntegral|28_2(vector5, vector6);
			if (num <= 0f)
			{
				return 1f;
			}
			return num2 / num;
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x000194B7 File Offset: 0x000176B7
		public static float ShapeAttenuationAreaRectangleLight(Vector3 forward, Vector3 wo)
		{
			return LensFlareCommonSRP.ShapeAttenuationDirLight(forward, wo);
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x000194C0 File Offset: 0x000176C0
		public static float ShapeAttenuationAreaDiscLight(Vector3 forward, Vector3 wo)
		{
			return LensFlareCommonSRP.ShapeAttenuationDirLight(forward, wo);
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x000194CC File Offset: 0x000176CC
		private static bool IsLensFlareSRPHidden(Camera cam, LensFlareComponentSRP comp, LensFlareDataSRP data)
		{
			return !comp.enabled || !comp.gameObject.activeSelf || !comp.gameObject.activeInHierarchy || data == null || data.elements == null || data.elements.Length == 0 || comp.intensity <= 0f || (cam.cullingMask & (1 << comp.gameObject.layer)) == 0;
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x00019540 File Offset: 0x00017740
		public static Vector4 GetFlareData0(Vector2 screenPos, Vector2 translationScale, Vector2 rayOff0, Vector2 vLocalScreenRatio, float angleDeg, float position, float angularOffset, Vector2 positionOffset, bool autoRotate)
		{
			if (!SystemInfo.graphicsUVStartsAtTop)
			{
				angleDeg *= -1f;
				positionOffset.y *= -1f;
			}
			float num = Mathf.Cos(-angularOffset * 0.017453292f);
			float num2 = Mathf.Sin(-angularOffset * 0.017453292f);
			Vector2 vector = -translationScale * (screenPos + screenPos * (position - 1f));
			vector = new Vector2(num * vector.x - num2 * vector.y, num2 * vector.x + num * vector.y);
			float num3 = angleDeg;
			num3 += 180f;
			if (autoRotate)
			{
				Vector2 vector2 = vector.normalized * vLocalScreenRatio * translationScale;
				num3 += -57.29578f * Mathf.Atan2(vector2.y, vector2.x);
			}
			num3 *= 0.017453292f;
			float num4 = Mathf.Cos(-num3);
			float num5 = Mathf.Sin(-num3);
			return new Vector4(num4, num5, positionOffset.x + rayOff0.x * translationScale.x, -positionOffset.y + rayOff0.y * translationScale.y);
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x00019664 File Offset: 0x00017864
		private static Vector2 GetLensFlareRayOffset(Vector2 screenPos, float position, float globalCos0, float globalSin0)
		{
			Vector2 vector = -(screenPos + screenPos * (position - 1f));
			return new Vector2(globalCos0 * vector.x - globalSin0 * vector.y, globalSin0 * vector.x + globalCos0 * vector.y);
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x000196B1 File Offset: 0x000178B1
		private static Vector3 WorldToViewport(Camera camera, bool isLocalLight, bool isCameraRelative, Matrix4x4 viewProjMatrix, Vector3 positionWS)
		{
			if (isLocalLight)
			{
				return LensFlareCommonSRP.WorldToViewportLocal(isCameraRelative, viewProjMatrix, camera.transform.position, positionWS);
			}
			return LensFlareCommonSRP.WorldToViewportDistance(camera, positionWS);
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x000196D4 File Offset: 0x000178D4
		private static Vector3 WorldToViewportLocal(bool isCameraRelative, Matrix4x4 viewProjMatrix, Vector3 cameraPosWS, Vector3 positionWS)
		{
			Vector3 vector = positionWS;
			if (isCameraRelative)
			{
				vector -= cameraPosWS;
			}
			Vector4 vector2 = viewProjMatrix * vector;
			Vector3 vector3 = new Vector3(vector2.x, vector2.y, 0f);
			vector3 /= vector2.w;
			vector3.x = vector3.x * 0.5f + 0.5f;
			vector3.y = vector3.y * 0.5f + 0.5f;
			vector3.y = 1f - vector3.y;
			vector3.z = vector2.w;
			return vector3;
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x00019774 File Offset: 0x00017974
		private static Vector3 WorldToViewportDistance(Camera cam, Vector3 positionWS)
		{
			Vector4 vector = cam.worldToCameraMatrix * positionWS;
			Vector4 vector2 = cam.projectionMatrix * vector;
			Vector3 vector3 = new Vector3(vector2.x, vector2.y, 0f);
			vector3 /= vector2.w;
			vector3.x = vector3.x * 0.5f + 0.5f;
			vector3.y = vector3.y * 0.5f + 0.5f;
			vector3.z = vector2.w;
			return vector3;
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x00019808 File Offset: 0x00017A08
		public static bool IsCloudLayerOpacityNeeded(Camera cam)
		{
			if (LensFlareCommonSRP.Instance.IsEmpty())
			{
				return false;
			}
			foreach (LensFlareCommonSRP.LensFlareCompInfo lensFlareCompInfo in LensFlareCommonSRP.Instance.Data)
			{
				if (lensFlareCompInfo != null && !(lensFlareCompInfo.comp == null))
				{
					LensFlareComponentSRP comp = lensFlareCompInfo.comp;
					LensFlareDataSRP lensFlareData = comp.lensFlareData;
					if (!LensFlareCommonSRP.IsLensFlareSRPHidden(cam, comp, lensFlareData) && comp.useOcclusion && (!comp.useOcclusion || comp.sampleCount != 0U) && comp.useBackgroundCloudOcclusion)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x000198B8 File Offset: 0x00017AB8
		private static void SetOcclusionPermutation(CommandBuffer cmd, bool useBackgroundCloudOcclusion, bool volumetricCloudOcclusion, bool hasCloudLayer, int _FlareCloudOpacity, int _FlareSunOcclusionTex, Texture cloudOpacityTexture, Texture sunOcclusionTexture)
		{
			if (useBackgroundCloudOcclusion && hasCloudLayer)
			{
				cmd.EnableShaderKeyword("FLARE_CLOUD_BACKGROUND_OCCLUSION");
				cmd.SetGlobalTexture(_FlareCloudOpacity, cloudOpacityTexture);
			}
			else
			{
				cmd.DisableShaderKeyword("FLARE_CLOUD_BACKGROUND_OCCLUSION");
			}
			if (!(sunOcclusionTexture != null))
			{
				cmd.DisableShaderKeyword("FLARE_VOLUMETRIC_CLOUD_OCCLUSION");
				return;
			}
			if (volumetricCloudOcclusion)
			{
				cmd.EnableShaderKeyword("FLARE_VOLUMETRIC_CLOUD_OCCLUSION");
				cmd.SetGlobalTexture(_FlareSunOcclusionTex, sunOcclusionTexture);
				return;
			}
			cmd.DisableShaderKeyword("FLARE_VOLUMETRIC_CLOUD_OCCLUSION");
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x00019930 File Offset: 0x00017B30
		public static void ComputeOcclusion(Material lensFlareShader, Camera cam, float actualWidth, float actualHeight, bool usePanini, float paniniDistance, float paniniCropToFit, bool isCameraRelative, Vector3 cameraPositionWS, Matrix4x4 viewProjMatrix, CommandBuffer cmd, bool taaEnabled, bool hasCloudLayer, Texture cloudOpacityTexture, Texture sunOcclusionTexture, int _FlareOcclusionTex, int _FlareCloudOpacity, int _FlareOcclusionIndex, int _FlareTex, int _FlareColorValue, int _FlareSunOcclusionTex, int _FlareData0, int _FlareData1, int _FlareData2, int _FlareData3, int _FlareData4)
		{
			if (!LensFlareCommonSRP.IsOcclusionRTCompatible())
			{
				return;
			}
			if (LensFlareCommonSRP.Instance.IsEmpty())
			{
				return;
			}
			Vector2 vector = new Vector2(actualWidth, actualHeight);
			float num = vector.x / vector.y;
			Vector2 vector2 = new Vector2(num, 1f);
			CoreUtils.SetRenderTarget(cmd, LensFlareCommonSRP.occlusionRT, ClearFlag.None, 0, CubemapFace.Unknown, -1);
			if (!taaEnabled)
			{
				cmd.ClearRenderTarget(false, true, Color.black);
			}
			float num2 = 1f / (float)LensFlareCommonSRP.maxLensFlareWithOcclusion;
			float num3 = 1f / (float)(LensFlareCommonSRP.maxLensFlareWithOcclusionTemporalSample + LensFlareCommonSRP.mergeNeeded);
			float num4 = 0.5f / (float)LensFlareCommonSRP.maxLensFlareWithOcclusion;
			float num5 = 0.5f / (float)(LensFlareCommonSRP.maxLensFlareWithOcclusionTemporalSample + LensFlareCommonSRP.mergeNeeded);
			int num6 = (taaEnabled ? 1 : 0);
			foreach (LensFlareCommonSRP.LensFlareCompInfo lensFlareCompInfo in LensFlareCommonSRP.m_Data)
			{
				if (lensFlareCompInfo != null && !(lensFlareCompInfo.comp == null))
				{
					LensFlareComponentSRP comp = lensFlareCompInfo.comp;
					LensFlareDataSRP lensFlareData = comp.lensFlareData;
					if (!LensFlareCommonSRP.IsLensFlareSRPHidden(cam, comp, lensFlareData) && comp.useOcclusion && (!comp.useOcclusion || comp.sampleCount != 0U))
					{
						Light component = comp.GetComponent<Light>();
						bool flag = false;
						Vector3 vector3;
						if (component != null && component.type == LightType.Directional)
						{
							vector3 = -component.transform.forward * cam.farClipPlane;
							flag = true;
						}
						else
						{
							vector3 = comp.transform.position;
						}
						Vector3 vector4 = LensFlareCommonSRP.WorldToViewport(cam, !flag, isCameraRelative, viewProjMatrix, vector3);
						if (usePanini && cam == Camera.main)
						{
							vector4 = LensFlareCommonSRP.DoPaniniProjection(vector4, actualWidth, actualHeight, cam.fieldOfView, paniniCropToFit, paniniDistance);
						}
						if (vector4.z >= 0f && (comp.allowOffScreen || (vector4.x >= 0f && vector4.x <= 1f && vector4.y >= 0f && vector4.y <= 1f)))
						{
							Vector3 vector5 = vector3 - cameraPositionWS;
							if (Vector3.Dot(cam.transform.forward, vector5) >= 0f)
							{
								float magnitude = vector5.magnitude;
								float num7 = magnitude / comp.maxAttenuationDistance;
								float num8 = magnitude / comp.maxAttenuationScale;
								float num9 = ((!flag && comp.distanceAttenuationCurve.length > 0) ? comp.distanceAttenuationCurve.Evaluate(num7) : 1f);
								if (!flag && comp.scaleByDistanceCurve.length >= 1)
								{
									comp.scaleByDistanceCurve.Evaluate(num8);
								}
								Vector3 vector6;
								if (flag)
								{
									vector6 = comp.transform.forward;
								}
								else
								{
									vector6 = (cam.transform.position - comp.transform.position).normalized;
								}
								Vector3 vector7 = LensFlareCommonSRP.WorldToViewport(cam, !flag, isCameraRelative, viewProjMatrix, vector3 + vector6 * comp.occlusionOffset);
								float num10 = (flag ? comp.celestialProjectedOcclusionRadius(cam) : comp.occlusionRadius);
								Vector2 vector8 = vector4;
								float magnitude2 = (LensFlareCommonSRP.WorldToViewport(cam, !flag, isCameraRelative, viewProjMatrix, vector3 + cam.transform.up * num10) - vector8).magnitude;
								cmd.SetGlobalVector(_FlareData1, new Vector4(magnitude2, comp.sampleCount, vector7.z, actualHeight / actualWidth));
								LensFlareCommonSRP.SetOcclusionPermutation(cmd, comp.useBackgroundCloudOcclusion, comp.volumetricCloudOcclusion, hasCloudLayer, _FlareCloudOpacity, _FlareSunOcclusionTex, cloudOpacityTexture, sunOcclusionTexture);
								cmd.EnableShaderKeyword("FLARE_COMPUTE_OCCLUSION");
								Vector2 vector9 = new Vector2(2f * vector4.x - 1f, 2f * vector4.y - 1f);
								if (SystemInfo.graphicsUVStartsAtTop)
								{
									vector9.y = -vector9.y;
								}
								Vector2 vector10 = new Vector2(Mathf.Abs(vector9.x), Mathf.Abs(vector9.y));
								float num11 = Mathf.Max(vector10.x, vector10.y);
								float num12 = ((comp.radialScreenAttenuationCurve.length > 0) ? comp.radialScreenAttenuationCurve.Evaluate(num11) : 1f);
								if (comp.intensity * num12 * num9 > 0f)
								{
									float num13 = Mathf.Cos(0f);
									float num14 = Mathf.Sin(0f);
									float num15 = 0f;
									float num16 = Mathf.Clamp01(0.999999f);
									cmd.SetGlobalVector(_FlareData3, new Vector4(comp.allowOffScreen ? 1f : (-1f), num16, Mathf.Exp(Mathf.Lerp(0f, 4f, 1f)), 0.33333334f));
									Vector2 lensFlareRayOffset = LensFlareCommonSRP.GetLensFlareRayOffset(vector9, num15, num13, num14);
									Vector4 flareData = LensFlareCommonSRP.GetFlareData0(vector9, Vector2.one, lensFlareRayOffset, vector2, 0f, num15, 0f, Vector2.zero, false);
									cmd.SetGlobalVector(_FlareData0, flareData);
									cmd.SetGlobalVector(_FlareData2, new Vector4(vector9.x, vector9.y, 0f, 0f));
									Rect rect = new Rect
									{
										x = (float)lensFlareCompInfo.index,
										y = (float)((LensFlareCommonSRP.frameIdx + LensFlareCommonSRP.mergeNeeded) * num6),
										width = 1f,
										height = 1f
									};
									cmd.SetViewport(rect);
									Blitter.DrawQuad(cmd, lensFlareShader, 4);
								}
							}
						}
					}
				}
			}
			if (taaEnabled)
			{
				cmd.SetRenderTarget(LensFlareCommonSRP.occlusionRT);
				cmd.SetViewport(new Rect
				{
					x = (float)LensFlareCommonSRP.m_Data.Count,
					y = 0f,
					width = (float)(LensFlareCommonSRP.maxLensFlareWithOcclusion - LensFlareCommonSRP.m_Data.Count),
					height = (float)(LensFlareCommonSRP.maxLensFlareWithOcclusionTemporalSample + LensFlareCommonSRP.mergeNeeded)
				});
				cmd.ClearRenderTarget(false, true, Color.black);
			}
			LensFlareCommonSRP.frameIdx++;
			LensFlareCommonSRP.frameIdx %= LensFlareCommonSRP.maxLensFlareWithOcclusionTemporalSample;
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x00019F98 File Offset: 0x00018198
		public static void DoLensFlareDataDrivenCommon(Material lensFlareShader, Camera cam, float actualWidth, float actualHeight, bool usePanini, float paniniDistance, float paniniCropToFit, bool isCameraRelative, Vector3 cameraPositionWS, Matrix4x4 viewProjMatrix, CommandBuffer cmd, bool taaEnabled, bool hasCloudLayer, Texture cloudOpacityTexture, Texture sunOcclusionTexture, RenderTargetIdentifier colorBuffer, Func<Light, Camera, Vector3, float> GetLensFlareLightAttenuation, int _FlareOcclusionRemapTex, int _FlareOcclusionTex, int _FlareOcclusionIndex, int _FlareCloudOpacity, int _FlareSunOcclusionTex, int _FlareTex, int _FlareColorValue, int _FlareData0, int _FlareData1, int _FlareData2, int _FlareData3, int _FlareData4, bool debugView)
		{
			if (LensFlareCommonSRP.Instance.IsEmpty())
			{
				return;
			}
			Vector2 vector = new Vector2(actualWidth, actualHeight);
			float num = vector.x / vector.y;
			Vector2 vector2 = new Vector2(num, 1f);
			CoreUtils.SetRenderTarget(cmd, colorBuffer, ClearFlag.None, 0, CubemapFace.Unknown, -1);
			cmd.SetViewport(new Rect
			{
				width = vector.x,
				height = vector.y
			});
			if (debugView)
			{
				cmd.ClearRenderTarget(false, true, Color.black);
			}
			foreach (LensFlareCommonSRP.LensFlareCompInfo lensFlareCompInfo in LensFlareCommonSRP.m_Data)
			{
				if (lensFlareCompInfo != null && !(lensFlareCompInfo.comp == null))
				{
					LensFlareComponentSRP comp = lensFlareCompInfo.comp;
					LensFlareDataSRP lensFlareData = comp.lensFlareData;
					if (!LensFlareCommonSRP.IsLensFlareSRPHidden(cam, comp, lensFlareData))
					{
						Light component = comp.GetComponent<Light>();
						bool flag = false;
						Vector3 vector3;
						if (component != null && component.type == LightType.Directional)
						{
							vector3 = -component.transform.forward * cam.farClipPlane;
							flag = true;
						}
						else
						{
							vector3 = comp.transform.position;
						}
						Vector3 vector4 = LensFlareCommonSRP.WorldToViewport(cam, !flag, isCameraRelative, viewProjMatrix, vector3);
						if (usePanini && cam == Camera.main)
						{
							vector4 = LensFlareCommonSRP.DoPaniniProjection(vector4, actualWidth, actualHeight, cam.fieldOfView, paniniCropToFit, paniniDistance);
						}
						if (vector4.z >= 0f && (comp.allowOffScreen || (vector4.x >= 0f && vector4.x <= 1f && vector4.y >= 0f && vector4.y <= 1f)))
						{
							Vector3 vector5 = vector3 - cameraPositionWS;
							if (Vector3.Dot(cam.transform.forward, vector5) >= 0f)
							{
								float magnitude = vector5.magnitude;
								float num2 = magnitude / comp.maxAttenuationDistance;
								float num3 = magnitude / comp.maxAttenuationScale;
								float num4 = ((!flag && comp.distanceAttenuationCurve.length > 0) ? comp.distanceAttenuationCurve.Evaluate(num2) : 1f);
								float num5 = ((!flag && comp.scaleByDistanceCurve.length >= 1) ? comp.scaleByDistanceCurve.Evaluate(num3) : 1f);
								Color color = Color.white;
								if (component != null && comp.attenuationByLightShape)
								{
									color *= GetLensFlareLightAttenuation(component, cam, -vector5.normalized);
								}
								LensFlareCommonSRP.<>c__DisplayClass40_0 CS$<>8__locals1;
								CS$<>8__locals1.screenPos = new Vector2(2f * vector4.x - 1f, -(2f * vector4.y - 1f));
								if (!SystemInfo.graphicsUVStartsAtTop && flag)
								{
									CS$<>8__locals1.screenPos.y = -CS$<>8__locals1.screenPos.y;
								}
								Vector2 vector6 = new Vector2(Mathf.Abs(CS$<>8__locals1.screenPos.x), Mathf.Abs(CS$<>8__locals1.screenPos.y));
								float num6 = Mathf.Max(vector6.x, vector6.y);
								float num7 = ((comp.radialScreenAttenuationCurve.length > 0) ? comp.radialScreenAttenuationCurve.Evaluate(num6) : 1f);
								float num8 = comp.intensity * num7 * num4;
								if (num8 > 0f)
								{
									color *= num4;
									Vector3 normalized = (cam.transform.position - comp.transform.position).normalized;
									Vector3 vector7 = LensFlareCommonSRP.WorldToViewport(cam, !flag, isCameraRelative, viewProjMatrix, vector3 + normalized * comp.occlusionOffset);
									float num9 = (flag ? comp.celestialProjectedOcclusionRadius(cam) : comp.occlusionRadius);
									Vector2 vector8 = vector4;
									float magnitude2 = (LensFlareCommonSRP.WorldToViewport(cam, !flag, isCameraRelative, viewProjMatrix, vector3 + cam.transform.up * num9) - vector8).magnitude;
									cmd.SetGlobalVector(_FlareData1, new Vector4(magnitude2, comp.sampleCount, vector7.z, actualHeight / actualWidth));
									if (comp.useOcclusion)
									{
										cmd.SetGlobalTexture(_FlareOcclusionTex, LensFlareCommonSRP.occlusionRT);
										cmd.EnableShaderKeyword("FLARE_HAS_OCCLUSION");
									}
									else
									{
										cmd.DisableShaderKeyword("FLARE_HAS_OCCLUSION");
									}
									if (LensFlareCommonSRP.IsOcclusionRTCompatible())
									{
										cmd.DisableShaderKeyword("FLARE_OPENGL3_OR_OPENGLCORE");
									}
									else
									{
										cmd.EnableShaderKeyword("FLARE_OPENGL3_OR_OPENGLCORE");
									}
									cmd.SetGlobalVector(_FlareOcclusionIndex, new Vector4((float)lensFlareCompInfo.index, 0f, 0f, 0f));
									cmd.SetGlobalTexture(_FlareOcclusionRemapTex, comp.occlusionRemapCurve.GetTexture());
									LensFlareDataElementSRP[] elements = lensFlareData.elements;
									for (int i = 0; i < elements.Length; i++)
									{
										LensFlareCommonSRP.<>c__DisplayClass40_1 CS$<>8__locals2;
										CS$<>8__locals2.element = elements[i];
										if (CS$<>8__locals2.element != null && CS$<>8__locals2.element.visible && (!(CS$<>8__locals2.element.lensFlareTexture == null) || CS$<>8__locals2.element.flareType != SRPLensFlareType.Image) && CS$<>8__locals2.element.localIntensity > 0f && CS$<>8__locals2.element.count > 0 && CS$<>8__locals2.element.localIntensity > 0f)
										{
											Color color2 = color;
											if (component != null && CS$<>8__locals2.element.modulateByLightColor)
											{
												if (component.useColorTemperature)
												{
													color2 *= component.color * Mathf.CorrelatedColorTemperatureToRGB(component.colorTemperature);
												}
												else
												{
													color2 *= component.color;
												}
											}
											Color color3 = color2;
											float num10 = CS$<>8__locals2.element.localIntensity * num8;
											if (num10 > 0f)
											{
												Texture lensFlareTexture = CS$<>8__locals2.element.lensFlareTexture;
												LensFlareCommonSRP.<>c__DisplayClass40_2 CS$<>8__locals3;
												if (CS$<>8__locals2.element.flareType == SRPLensFlareType.Image)
												{
													CS$<>8__locals3.usedAspectRatio = (CS$<>8__locals2.element.preserveAspectRatio ? ((float)lensFlareTexture.height / (float)lensFlareTexture.width) : 1f);
												}
												else
												{
													CS$<>8__locals3.usedAspectRatio = 1f;
												}
												float rotation = CS$<>8__locals2.element.rotation;
												Vector2 vector9;
												if (CS$<>8__locals2.element.preserveAspectRatio)
												{
													if (CS$<>8__locals3.usedAspectRatio >= 1f)
													{
														vector9 = new Vector2(CS$<>8__locals2.element.sizeXY.x / CS$<>8__locals3.usedAspectRatio, CS$<>8__locals2.element.sizeXY.y);
													}
													else
													{
														vector9 = new Vector2(CS$<>8__locals2.element.sizeXY.x, CS$<>8__locals2.element.sizeXY.y * CS$<>8__locals3.usedAspectRatio);
													}
												}
												else
												{
													vector9 = new Vector2(CS$<>8__locals2.element.sizeXY.x, CS$<>8__locals2.element.sizeXY.y);
												}
												float num11 = 0.1f;
												Vector2 vector10 = new Vector2(vector9.x, vector9.y);
												CS$<>8__locals3.combinedScale = num5 * num11 * CS$<>8__locals2.element.uniformScale * comp.scale;
												vector10 *= CS$<>8__locals3.combinedScale;
												color3 *= CS$<>8__locals2.element.tint;
												color3 *= num10;
												float num12 = (SystemInfo.graphicsUVStartsAtTop ? CS$<>8__locals2.element.angularOffset : (-CS$<>8__locals2.element.angularOffset));
												CS$<>8__locals3.globalCos0 = Mathf.Cos(-num12 * 0.017453292f);
												CS$<>8__locals3.globalSin0 = Mathf.Sin(-num12 * 0.017453292f);
												CS$<>8__locals3.position = 2f * CS$<>8__locals2.element.position;
												SRPLensFlareBlendMode blendMode = CS$<>8__locals2.element.blendMode;
												int num13;
												if (blendMode == SRPLensFlareBlendMode.Additive)
												{
													num13 = 0;
												}
												else if (blendMode == SRPLensFlareBlendMode.Screen)
												{
													num13 = 1;
												}
												else if (blendMode == SRPLensFlareBlendMode.Premultiply)
												{
													num13 = 2;
												}
												else if (blendMode == SRPLensFlareBlendMode.Lerp)
												{
													num13 = 3;
												}
												else
												{
													num13 = 0;
												}
												if (CS$<>8__locals2.element.flareType == SRPLensFlareType.Image)
												{
													cmd.DisableShaderKeyword("FLARE_CIRCLE");
													cmd.DisableShaderKeyword("FLARE_POLYGON");
												}
												else if (CS$<>8__locals2.element.flareType == SRPLensFlareType.Circle)
												{
													cmd.EnableShaderKeyword("FLARE_CIRCLE");
													cmd.DisableShaderKeyword("FLARE_POLYGON");
												}
												else if (CS$<>8__locals2.element.flareType == SRPLensFlareType.Polygon)
												{
													cmd.DisableShaderKeyword("FLARE_CIRCLE");
													cmd.EnableShaderKeyword("FLARE_POLYGON");
												}
												if (CS$<>8__locals2.element.flareType == SRPLensFlareType.Circle || CS$<>8__locals2.element.flareType == SRPLensFlareType.Polygon)
												{
													if (CS$<>8__locals2.element.inverseSDF)
													{
														cmd.EnableShaderKeyword("FLARE_INVERSE_SDF");
													}
													else
													{
														cmd.DisableShaderKeyword("FLARE_INVERSE_SDF");
													}
												}
												else
												{
													cmd.DisableShaderKeyword("FLARE_INVERSE_SDF");
												}
												if (CS$<>8__locals2.element.lensFlareTexture != null)
												{
													cmd.SetGlobalTexture(_FlareTex, CS$<>8__locals2.element.lensFlareTexture);
												}
												float num14 = Mathf.Clamp01(1f - CS$<>8__locals2.element.edgeOffset - 1E-06f);
												if (CS$<>8__locals2.element.flareType == SRPLensFlareType.Polygon)
												{
													num14 = Mathf.Pow(num14 + 1f, 5f);
												}
												float sdfRoundness = CS$<>8__locals2.element.sdfRoundness;
												cmd.SetGlobalVector(_FlareData3, new Vector4(comp.allowOffScreen ? 1f : (-1f), num14, Mathf.Exp(Mathf.Lerp(0f, 4f, Mathf.Clamp01(1f - CS$<>8__locals2.element.fallOff))), 1f / (float)CS$<>8__locals2.element.sideCount));
												if (CS$<>8__locals2.element.flareType == SRPLensFlareType.Polygon)
												{
													float num15 = 1f / (float)CS$<>8__locals2.element.sideCount;
													float num16 = Mathf.Cos(3.1415927f * num15);
													float num17 = num16 * sdfRoundness;
													float num18 = num16 - num17;
													float num19 = 6.2831855f * num15;
													float num20 = num18 * Mathf.Tan(0.5f * num19);
													cmd.SetGlobalVector(_FlareData4, new Vector4(sdfRoundness, num18, num19, num20));
												}
												else
												{
													cmd.SetGlobalVector(_FlareData4, new Vector4(sdfRoundness, 0f, 0f, 0f));
												}
												if (!CS$<>8__locals2.element.allowMultipleElement || CS$<>8__locals2.element.count == 1)
												{
													Vector2 vector11 = vector10;
													Vector2 lensFlareRayOffset = LensFlareCommonSRP.GetLensFlareRayOffset(CS$<>8__locals1.screenPos, CS$<>8__locals3.position, CS$<>8__locals3.globalCos0, CS$<>8__locals3.globalSin0);
													if (CS$<>8__locals2.element.enableRadialDistortion)
													{
														Vector2 lensFlareRayOffset2 = LensFlareCommonSRP.GetLensFlareRayOffset(CS$<>8__locals1.screenPos, 0f, CS$<>8__locals3.globalCos0, CS$<>8__locals3.globalSin0);
														vector11 = LensFlareCommonSRP.<DoLensFlareDataDrivenCommon>g__ComputeLocalSize|40_0(lensFlareRayOffset, lensFlareRayOffset2, vector11, CS$<>8__locals2.element.distortionCurve, ref CS$<>8__locals1, ref CS$<>8__locals2, ref CS$<>8__locals3);
													}
													Vector4 flareData = LensFlareCommonSRP.GetFlareData0(CS$<>8__locals1.screenPos, CS$<>8__locals2.element.translationScale, lensFlareRayOffset, vector2, rotation, CS$<>8__locals3.position, num12, CS$<>8__locals2.element.positionOffset, CS$<>8__locals2.element.autoRotate);
													cmd.SetGlobalVector(_FlareData0, flareData);
													cmd.SetGlobalVector(_FlareData2, new Vector4(CS$<>8__locals1.screenPos.x, CS$<>8__locals1.screenPos.y, vector11.x, vector11.y));
													cmd.SetGlobalVector(_FlareColorValue, color3);
													Blitter.DrawQuad(cmd, lensFlareShader, num13);
												}
												else
												{
													float num21 = 2f * CS$<>8__locals2.element.lengthSpread / (float)(CS$<>8__locals2.element.count - 1);
													if (CS$<>8__locals2.element.distribution == SRPLensFlareDistribution.Uniform)
													{
														float num22 = 0f;
														for (int j = 0; j < CS$<>8__locals2.element.count; j++)
														{
															Vector2 vector12 = vector10;
															Vector2 lensFlareRayOffset3 = LensFlareCommonSRP.GetLensFlareRayOffset(CS$<>8__locals1.screenPos, CS$<>8__locals3.position, CS$<>8__locals3.globalCos0, CS$<>8__locals3.globalSin0);
															if (CS$<>8__locals2.element.enableRadialDistortion)
															{
																Vector2 lensFlareRayOffset4 = LensFlareCommonSRP.GetLensFlareRayOffset(CS$<>8__locals1.screenPos, 0f, CS$<>8__locals3.globalCos0, CS$<>8__locals3.globalSin0);
																vector12 = LensFlareCommonSRP.<DoLensFlareDataDrivenCommon>g__ComputeLocalSize|40_0(lensFlareRayOffset3, lensFlareRayOffset4, vector12, CS$<>8__locals2.element.distortionCurve, ref CS$<>8__locals1, ref CS$<>8__locals2, ref CS$<>8__locals3);
															}
															float num23 = ((CS$<>8__locals2.element.count >= 2) ? ((float)j / (float)(CS$<>8__locals2.element.count - 1)) : 0.5f);
															Color color4 = CS$<>8__locals2.element.colorGradient.Evaluate(num23);
															Vector4 flareData2 = LensFlareCommonSRP.GetFlareData0(CS$<>8__locals1.screenPos, CS$<>8__locals2.element.translationScale, lensFlareRayOffset3, vector2, rotation + num22, CS$<>8__locals3.position, num12, CS$<>8__locals2.element.positionOffset, CS$<>8__locals2.element.autoRotate);
															cmd.SetGlobalVector(_FlareData0, flareData2);
															cmd.SetGlobalVector(_FlareData2, new Vector4(CS$<>8__locals1.screenPos.x, CS$<>8__locals1.screenPos.y, vector12.x, vector12.y));
															cmd.SetGlobalVector(_FlareColorValue, color3 * color4);
															Blitter.DrawQuad(cmd, lensFlareShader, num13);
															CS$<>8__locals3.position += num21;
															num22 += CS$<>8__locals2.element.uniformAngle;
														}
													}
													else if (CS$<>8__locals2.element.distribution == SRPLensFlareDistribution.Random)
													{
														Random.State state = Random.state;
														Random.InitState(CS$<>8__locals2.element.seed);
														Vector2 vector13 = new Vector2(CS$<>8__locals3.globalSin0, CS$<>8__locals3.globalCos0);
														vector13 *= CS$<>8__locals2.element.positionVariation.y;
														for (int k = 0; k < CS$<>8__locals2.element.count; k++)
														{
															float num24 = LensFlareCommonSRP.<DoLensFlareDataDrivenCommon>g__RandomRange|40_1(-1f, 1f) * CS$<>8__locals2.element.intensityVariation + 1f;
															Vector2 lensFlareRayOffset5 = LensFlareCommonSRP.GetLensFlareRayOffset(CS$<>8__locals1.screenPos, CS$<>8__locals3.position, CS$<>8__locals3.globalCos0, CS$<>8__locals3.globalSin0);
															Vector2 vector14 = vector10;
															if (CS$<>8__locals2.element.enableRadialDistortion)
															{
																Vector2 lensFlareRayOffset6 = LensFlareCommonSRP.GetLensFlareRayOffset(CS$<>8__locals1.screenPos, 0f, CS$<>8__locals3.globalCos0, CS$<>8__locals3.globalSin0);
																vector14 = LensFlareCommonSRP.<DoLensFlareDataDrivenCommon>g__ComputeLocalSize|40_0(lensFlareRayOffset5, lensFlareRayOffset6, vector14, CS$<>8__locals2.element.distortionCurve, ref CS$<>8__locals1, ref CS$<>8__locals2, ref CS$<>8__locals3);
															}
															vector14 += vector14 * (CS$<>8__locals2.element.scaleVariation * LensFlareCommonSRP.<DoLensFlareDataDrivenCommon>g__RandomRange|40_1(-1f, 1f));
															Color color5 = CS$<>8__locals2.element.colorGradient.Evaluate(LensFlareCommonSRP.<DoLensFlareDataDrivenCommon>g__RandomRange|40_1(0f, 1f));
															Vector2 vector15 = CS$<>8__locals2.element.positionOffset + LensFlareCommonSRP.<DoLensFlareDataDrivenCommon>g__RandomRange|40_1(-1f, 1f) * vector13;
															float num25 = rotation + LensFlareCommonSRP.<DoLensFlareDataDrivenCommon>g__RandomRange|40_1(-3.1415927f, 3.1415927f) * CS$<>8__locals2.element.rotationVariation;
															if (num24 > 0f)
															{
																Vector4 flareData3 = LensFlareCommonSRP.GetFlareData0(CS$<>8__locals1.screenPos, CS$<>8__locals2.element.translationScale, lensFlareRayOffset5, vector2, num25, CS$<>8__locals3.position, num12, vector15, CS$<>8__locals2.element.autoRotate);
																cmd.SetGlobalVector(_FlareData0, flareData3);
																cmd.SetGlobalVector(_FlareData2, new Vector4(CS$<>8__locals1.screenPos.x, CS$<>8__locals1.screenPos.y, vector14.x, vector14.y));
																cmd.SetGlobalVector(_FlareColorValue, color3 * color5 * num24);
																Blitter.DrawQuad(cmd, lensFlareShader, num13);
															}
															CS$<>8__locals3.position += num21;
															CS$<>8__locals3.position += 0.5f * num21 * LensFlareCommonSRP.<DoLensFlareDataDrivenCommon>g__RandomRange|40_1(-1f, 1f) * CS$<>8__locals2.element.positionVariation.x;
														}
														Random.state = state;
													}
													else if (CS$<>8__locals2.element.distribution == SRPLensFlareDistribution.Curve)
													{
														for (int l = 0; l < CS$<>8__locals2.element.count; l++)
														{
															float num26 = ((CS$<>8__locals2.element.count >= 2) ? ((float)l / (float)(CS$<>8__locals2.element.count - 1)) : 0.5f);
															Color color6 = CS$<>8__locals2.element.colorGradient.Evaluate(num26);
															float num27 = ((CS$<>8__locals2.element.positionCurve.length > 0) ? CS$<>8__locals2.element.positionCurve.Evaluate(num26) : 1f);
															float num28 = CS$<>8__locals3.position + 2f * CS$<>8__locals2.element.lengthSpread * num27;
															Vector2 lensFlareRayOffset7 = LensFlareCommonSRP.GetLensFlareRayOffset(CS$<>8__locals1.screenPos, num28, CS$<>8__locals3.globalCos0, CS$<>8__locals3.globalSin0);
															Vector2 vector16 = vector10;
															if (CS$<>8__locals2.element.enableRadialDistortion)
															{
																Vector2 lensFlareRayOffset8 = LensFlareCommonSRP.GetLensFlareRayOffset(CS$<>8__locals1.screenPos, 0f, CS$<>8__locals3.globalCos0, CS$<>8__locals3.globalSin0);
																vector16 = LensFlareCommonSRP.<DoLensFlareDataDrivenCommon>g__ComputeLocalSize|40_0(lensFlareRayOffset7, lensFlareRayOffset8, vector16, CS$<>8__locals2.element.distortionCurve, ref CS$<>8__locals1, ref CS$<>8__locals2, ref CS$<>8__locals3);
															}
															float num29 = ((CS$<>8__locals2.element.scaleCurve.length > 0) ? CS$<>8__locals2.element.scaleCurve.Evaluate(num26) : 1f);
															vector16 *= num29;
															float num30 = CS$<>8__locals2.element.uniformAngleCurve.Evaluate(num26) * (180f - 180f / (float)CS$<>8__locals2.element.count);
															Vector4 flareData4 = LensFlareCommonSRP.GetFlareData0(CS$<>8__locals1.screenPos, CS$<>8__locals2.element.translationScale, lensFlareRayOffset7, vector2, rotation + num30, num28, num12, CS$<>8__locals2.element.positionOffset, CS$<>8__locals2.element.autoRotate);
															cmd.SetGlobalVector(_FlareData0, flareData4);
															cmd.SetGlobalVector(_FlareData2, new Vector4(CS$<>8__locals1.screenPos.x, CS$<>8__locals1.screenPos.y, vector16.x, vector16.y));
															cmd.SetGlobalVector(_FlareColorValue, color3 * color6);
															Blitter.DrawQuad(cmd, lensFlareShader, num13);
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x0001B228 File Offset: 0x00019428
		private static Vector2 DoPaniniProjection(Vector2 screenPos, float actualWidth, float actualHeight, float fieldOfView, float paniniProjectionCropToFit, float paniniProjectionDistance)
		{
			Vector2 vector = LensFlareCommonSRP.CalcViewExtents(actualWidth, actualHeight, fieldOfView);
			Vector2 vector2 = LensFlareCommonSRP.CalcCropExtents(actualWidth, actualHeight, fieldOfView, paniniProjectionDistance);
			float num = vector2.x / vector.x;
			float num2 = vector2.y / vector.y;
			float num3 = Mathf.Min(num, num2);
			float num4 = Mathf.Lerp(1f, Mathf.Clamp01(num3), paniniProjectionCropToFit);
			Vector2 vector3 = LensFlareCommonSRP.Panini_Generic_Inv(new Vector2(2f * screenPos.x - 1f, 2f * screenPos.y - 1f) * vector, paniniProjectionDistance) / (vector * num4);
			return new Vector2(0.5f * vector3.x + 0.5f, 0.5f * vector3.y + 0.5f);
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x0001B2F4 File Offset: 0x000194F4
		private static Vector2 CalcViewExtents(float actualWidth, float actualHeight, float fieldOfView)
		{
			float num = fieldOfView * 0.017453292f;
			float num2 = actualWidth / actualHeight;
			float num3 = Mathf.Tan(0.5f * num);
			return new Vector2(num2 * num3, num3);
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x0001B324 File Offset: 0x00019524
		private static Vector2 CalcCropExtents(float actualWidth, float actualHeight, float fieldOfView, float d)
		{
			float num = 1f + d;
			Vector2 vector = LensFlareCommonSRP.CalcViewExtents(actualWidth, actualHeight, fieldOfView);
			float num2 = Mathf.Sqrt(vector.x * vector.x + 1f);
			float num3 = 1f / num2;
			float num4 = num3 + d;
			return vector * num3 * (num / num4);
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x0001B378 File Offset: 0x00019578
		private static Vector2 Panini_Generic_Inv(Vector2 projPos, float d)
		{
			float num = 1f + d;
			float num2 = Mathf.Sqrt(projPos.x * projPos.x + 1f);
			float num3 = 1f / num2;
			float num4 = num3 + d;
			return projPos * num3 * (num / num4);
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x0001B424 File Offset: 0x00019624
		[CompilerGenerated]
		internal static float <ShapeAttenuationAreaTubeLight>g__Fpo|28_0(float d, float l)
		{
			return l / (d * (d * d + l * l)) + Mathf.Atan(l / d) / (d * d);
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x0001B43E File Offset: 0x0001963E
		[CompilerGenerated]
		internal static float <ShapeAttenuationAreaTubeLight>g__Fwt|28_1(float d, float l)
		{
			return l * l / (d * (d * d + l * l));
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x0001B450 File Offset: 0x00019650
		[CompilerGenerated]
		internal static float <ShapeAttenuationAreaTubeLight>g__DiffLineIntegral|28_2(Vector3 p1, Vector3 p2)
		{
			Vector3 normalized = (p2 - p1).normalized;
			float num;
			if ((double)p1.z <= 0.0 && (double)p2.z <= 0.0)
			{
				num = 0f;
			}
			else
			{
				if ((double)p1.z < 0.0)
				{
					p1 = (p1 * p2.z - p2 * p1.z) / (p2.z - p1.z);
				}
				if ((double)p2.z < 0.0)
				{
					p2 = (-p1 * p2.z + p2 * p1.z) / (-p2.z + p1.z);
				}
				float num2 = Vector3.Dot(p1, normalized);
				float num3 = Vector3.Dot(p2, normalized);
				Vector3 vector = p1 - num2 * normalized;
				float magnitude = vector.magnitude;
				num = ((LensFlareCommonSRP.<ShapeAttenuationAreaTubeLight>g__Fpo|28_0(magnitude, num3) - LensFlareCommonSRP.<ShapeAttenuationAreaTubeLight>g__Fpo|28_0(magnitude, num2)) * vector.z + (LensFlareCommonSRP.<ShapeAttenuationAreaTubeLight>g__Fwt|28_1(magnitude, num3) - LensFlareCommonSRP.<ShapeAttenuationAreaTubeLight>g__Fwt|28_1(magnitude, num2)) * normalized.z) / 3.1415927f;
			}
			return num;
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x0001B590 File Offset: 0x00019790
		[CompilerGenerated]
		internal static Vector2 <DoLensFlareDataDrivenCommon>g__ComputeLocalSize|40_0(Vector2 rayOff, Vector2 rayOff0, Vector2 curSize, AnimationCurve distortionCurve, ref LensFlareCommonSRP.<>c__DisplayClass40_0 A_4, ref LensFlareCommonSRP.<>c__DisplayClass40_1 A_5, ref LensFlareCommonSRP.<>c__DisplayClass40_2 A_6)
		{
			LensFlareCommonSRP.GetLensFlareRayOffset(A_4.screenPos, A_6.position, A_6.globalCos0, A_6.globalSin0);
			float num;
			if (!A_5.element.distortionRelativeToCenter)
			{
				Vector2 vector = (rayOff - rayOff0) * 0.5f;
				num = Mathf.Clamp01(Mathf.Max(Mathf.Abs(vector.x), Mathf.Abs(vector.y)));
			}
			else
			{
				num = Mathf.Clamp01((A_4.screenPos + (rayOff + new Vector2(A_5.element.positionOffset.x, -A_5.element.positionOffset.y)) * A_5.element.translationScale).magnitude);
			}
			float num2 = Mathf.Clamp01(distortionCurve.Evaluate(num));
			return new Vector2(Mathf.Lerp(curSize.x, A_5.element.targetSizeDistortion.x * A_6.combinedScale / A_6.usedAspectRatio, num2), Mathf.Lerp(curSize.y, A_5.element.targetSizeDistortion.y * A_6.combinedScale, num2));
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x0001B6BF File Offset: 0x000198BF
		[CompilerGenerated]
		internal static float <DoLensFlareDataDrivenCommon>g__RandomRange|40_1(float min, float max)
		{
			return Random.Range(min, max);
		}

		// Token: 0x0400036F RID: 879
		private static LensFlareCommonSRP m_Instance = null;

		// Token: 0x04000370 RID: 880
		private static readonly object m_Padlock = new object();

		// Token: 0x04000371 RID: 881
		private static List<LensFlareCommonSRP.LensFlareCompInfo> m_Data = new List<LensFlareCommonSRP.LensFlareCompInfo>();

		// Token: 0x04000372 RID: 882
		private static List<int> m_AvailableIndicies = new List<int>();

		// Token: 0x04000373 RID: 883
		public static int maxLensFlareWithOcclusion = 128;

		// Token: 0x04000374 RID: 884
		public static int maxLensFlareWithOcclusionTemporalSample = 8;

		// Token: 0x04000375 RID: 885
		public static int mergeNeeded = 1;

		// Token: 0x04000376 RID: 886
		public static RTHandle occlusionRT = null;

		// Token: 0x04000377 RID: 887
		private static int frameIdx = 0;

		// Token: 0x04000378 RID: 888
		private static readonly bool s_SupportsLensFlareTexFormat = SystemInfo.IsFormatSupported(GraphicsFormat.R32_SFloat, FormatUsage.Render);

		// Token: 0x020001B5 RID: 437
		internal class LensFlareCompInfo
		{
			// Token: 0x06000B3A RID: 2874 RVA: 0x0002F379 File Offset: 0x0002D579
			internal LensFlareCompInfo(int idx, LensFlareComponentSRP cmp)
			{
				this.index = idx;
				this.comp = cmp;
			}

			// Token: 0x04000730 RID: 1840
			internal int index;

			// Token: 0x04000731 RID: 1841
			internal LensFlareComponentSRP comp;
		}
	}
}
