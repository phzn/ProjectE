using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace ProjectExhibitionist.Core.Rendering
{
	// Token: 0x02000A99 RID: 2713
	public static class GraphicsSettingsApplier
	{
		// Token: 0x06004D2C RID: 19756 RVA: 0x001DF284 File Offset: 0x001DD484
		public static void ApplyPipeline(GraphicsSettingsModel m, UniversalRenderPipelineAsset urp, bool forceLinearUpscaling = false)
		{
			if (m == null || urp == null)
			{
				return;
			}
			urp.renderScale = Mathf.Clamp(m.renderScale, 0.5f, 2f);
			urp.upscalingFilter = ((!forceLinearUpscaling && m.upscaling == GraphicsSettingsModel.UpscalingMode.FSR) ? UpscalingFilterSelection.FSR : UpscalingFilterSelection.Linear);
			urp.fsrOverrideSharpness = true;
			urp.fsrSharpness = Mathf.Clamp01(m.fsrSharpness);
			urp.supportsHDR = m.hdr;
			urp.supportsCameraOpaqueTexture = m.opaqueTexture;
			urp.msaaSampleCount = GraphicsSettingsApplier.SanitizeMsaa(m.msaa);
			FieldInfo additionalLightsModeField = GraphicsSettingsApplier.AdditionalLightsModeField;
			if (additionalLightsModeField != null)
			{
				additionalLightsModeField.SetValue(urp, LightRenderingMode.PerPixel);
			}
			urp.maxAdditionalLightsCount = Mathf.Clamp(m.additionalLightsPerObject, 1, 8);
			urp.shadowDistance = (m.shadowsEnabled ? Mathf.Clamp(m.shadowDistance, 0f, 500f) : 0f);
			urp.shadowCascadeCount = Mathf.Clamp(m.shadowCascades, 1, 4);
			if (GraphicsSettingsApplier.MainShadowResField != null)
			{
				int num = Mathf.ClosestPowerOfTwo(Mathf.Clamp(m.mainShadowResolution, 512, 4096));
				GraphicsSettingsApplier.MainShadowResField.SetValue(urp, Enum.ToObject(GraphicsSettingsApplier.MainShadowResField.FieldType, num));
			}
			FieldInfo additionalShadowsField = GraphicsSettingsApplier.AdditionalShadowsField;
			if (additionalShadowsField != null)
			{
				additionalShadowsField.SetValue(urp, false);
			}
			FieldInfo mainShadowsField = GraphicsSettingsApplier.MainShadowsField;
			if (mainShadowsField != null)
			{
				mainShadowsField.SetValue(urp, true);
			}
			FieldInfo softShadowsField = GraphicsSettingsApplier.SoftShadowsField;
			if (softShadowsField == null)
			{
				return;
			}
			softShadowsField.SetValue(urp, m.softShadows);
		}

		// Token: 0x06004D2D RID: 19757 RVA: 0x001DF404 File Offset: 0x001DD604
		public static void ApplyEngineQuality(GraphicsSettingsModel m, bool startupPresentation = false)
		{
			if (m == null)
			{
				return;
			}
			QualitySettings.globalTextureMipmapLimit = 3 - Mathf.Clamp(m.textureQuality, 0, 3);
			QualitySettings.lodBias = Mathf.Clamp(m.lodBias, 0.25f, 4f);
			QualitySettings.realtimeReflectionProbes = m.realtimeReflectionProbes;
			switch (m.anisotropic)
			{
			case GraphicsSettingsModel.AnisoMode.Disabled:
				QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
				Texture.SetGlobalAnisotropicFilteringLimits(-1, -1);
				break;
			case GraphicsSettingsModel.AnisoMode.PerTexture:
				QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
				Texture.SetGlobalAnisotropicFilteringLimits(-1, -1);
				break;
			case GraphicsSettingsModel.AnisoMode.Force8x:
				QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
				Texture.SetGlobalAnisotropicFilteringLimits(8, 8);
				break;
			case GraphicsSettingsModel.AnisoMode.Force16x:
				QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
				Texture.SetGlobalAnisotropicFilteringLimits(16, 16);
				break;
			}
			GraphicsSettingsApplier.ApplyFramePacing(m, startupPresentation);
		}

		// Token: 0x06004D2E RID: 19758 RVA: 0x001DF4B0 File Offset: 0x001DD6B0
		public static int GetTargetFrameRate(GraphicsSettingsModel m, bool startupPresentation = false)
		{
			if (m.fpsCap > 0)
			{
				return Mathf.Clamp(m.fpsCap, 30, 360);
			}
			if (!startupPresentation)
			{
				return -1;
			}
			return 60;
		}

		// Token: 0x06004D2F RID: 19759 RVA: 0x001DF4D5 File Offset: 0x001DD6D5
		public static int GetVSyncCount(GraphicsSettingsModel m, bool startupPresentation = false)
		{
			if (startupPresentation || !m.vSync || m.fpsCap > 0)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x06004D30 RID: 19760 RVA: 0x001DF4EE File Offset: 0x001DD6EE
		public static void ApplyFramePacing(GraphicsSettingsModel m, bool startupPresentation = false)
		{
			if (m == null)
			{
				return;
			}
			QualitySettings.vSyncCount = GraphicsSettingsApplier.GetVSyncCount(m, startupPresentation);
			Application.targetFrameRate = GraphicsSettingsApplier.GetTargetFrameRate(m, startupPresentation);
		}

		// Token: 0x06004D31 RID: 19761 RVA: 0x001DF50C File Offset: 0x001DD70C
		public static void ApplyCamera(GraphicsSettingsModel m, Camera cam)
		{
			if (m == null || cam == null)
			{
				return;
			}
			ChunkManager instance = ChunkManager.Instance;
			if (instance == null || !instance.isActiveAndEnabled || instance.IsPoiTravelSuspended)
			{
				cam.farClipPlane = Mathf.Lerp(300f, 2000f, Mathf.InverseLerp(0.35f, 2f, Mathf.Clamp(m.viewDistanceScale, 0.35f, 2f)));
			}
			UniversalAdditionalCameraData component = cam.GetComponent<UniversalAdditionalCameraData>();
			if (component == null)
			{
				return;
			}
			bool flag = CharacterPreviewRendering.IsTransparentPreview(cam);
			if (flag)
			{
				cam.clearFlags = CameraClearFlags.Color;
				Color backgroundColor = cam.backgroundColor;
				backgroundColor.a = 0f;
				cam.backgroundColor = backgroundColor;
			}
			component.renderPostProcessing = !flag && GraphicsSettingsApplier.RequiresPostProcessingPasses(m, cam.gameObject.scene.name == "MainMenu");
			component.stopNaN = true;
			UniversalAdditionalCameraData universalAdditionalCameraData = component;
			AntialiasingMode antialiasingMode;
			if (flag)
			{
				antialiasingMode = AntialiasingMode.None;
			}
			else
			{
				GraphicsSettingsModel.PostAAMode postAA = m.postAA;
				AntialiasingMode antialiasingMode2;
				if (postAA != GraphicsSettingsModel.PostAAMode.FXAA)
				{
					if (postAA != GraphicsSettingsModel.PostAAMode.SMAA)
					{
						antialiasingMode2 = AntialiasingMode.None;
					}
					else
					{
						antialiasingMode2 = AntialiasingMode.SubpixelMorphologicalAntiAliasing;
					}
				}
				else
				{
					antialiasingMode2 = AntialiasingMode.FastApproximateAntialiasing;
				}
				antialiasingMode = antialiasingMode2;
			}
			universalAdditionalCameraData.antialiasing = antialiasingMode;
			component.antialiasingQuality = AntialiasingQuality.High;
			if (instance == null || !instance.isActiveAndEnabled || instance.IsPoiTravelSuspended)
			{
				GraphicsSettingsApplier.ApplyTerrainQuality(m, cam, null);
			}
		}

		// Token: 0x06004D32 RID: 19762 RVA: 0x001DF654 File Offset: 0x001DD854
		public static bool RequiresPostProcessingPasses(GraphicsSettingsModel m, bool forceLinearUpscaling = false)
		{
			if (m == null)
			{
				return false;
			}
			float num = Mathf.Clamp(m.renderScale, 0.5f, 2f);
			bool flag = num <= 1f || Mathf.Abs(1f - num) < 0.05f;
			return m.postProcessing || !Mathf.Approximately(GraphicsSettingsModel.SanitizeBrightness(m.brightness), 1f) || m.postAA != GraphicsSettingsModel.PostAAMode.None || (!forceLinearUpscaling && m.upscaling == GraphicsSettingsModel.UpscalingMode.FSR && flag) || (m.colorVision != GraphicsSettingsModel.ColorVisionMode.Normal && m.colorVisionStrength > 0.001f);
		}

		// Token: 0x06004D33 RID: 19763 RVA: 0x001DF6F0 File Offset: 0x001DD8F0
		public static void ApplyTerrainQuality(GraphicsSettingsModel m, Camera cam, GameObject root = null)
		{
			if (m == null || cam == null)
			{
				return;
			}
			Terrain[] array = ((root != null) ? root.GetComponentsInChildren<Terrain>(true) : Object.FindObjectsOfType<Terrain>(true));
			for (int i = 0; i < array.Length; i++)
			{
				float num = Mathf.Clamp(m.viewDistanceScale, 0.35f, 2f);
				float num2 = Mathf.Max(40f, cam.farClipPlane);
				array[i].detailObjectDistance = Mathf.Min(80f * num, num2 * 0.8f);
				array[i].treeDistance = Mathf.Min(1000f * num, num2 * 1.05f);
				array[i].treeBillboardDistance = Mathf.Min(120f * num, num2 * 0.65f);
			}
		}

		// Token: 0x06004D34 RID: 19764 RVA: 0x001DF7AC File Offset: 0x001DD9AC
		public static void ApplyRendererFeatures(GraphicsSettingsModel m, ScriptableRendererFeature[] rendererFeatures)
		{
			if (m == null || rendererFeatures == null)
			{
				return;
			}
			foreach (ScriptableRendererFeature scriptableRendererFeature in rendererFeatures)
			{
				if (!(scriptableRendererFeature == null))
				{
					string name = scriptableRendererFeature.GetType().Name;
					bool? flag = null;
					if (name.IndexOf("ScreenSpaceAmbientOcclusion", StringComparison.OrdinalIgnoreCase) >= 0)
					{
						flag = new bool?(m.ssao);
					}
					else if (name.IndexOf("DecalRendererFeature", StringComparison.OrdinalIgnoreCase) >= 0)
					{
						flag = new bool?(true);
					}
					if (flag != null && scriptableRendererFeature.isActive != flag.Value)
					{
						scriptableRendererFeature.SetActive(flag.Value);
					}
				}
			}
		}

		// Token: 0x06004D35 RID: 19765 RVA: 0x001DF850 File Offset: 0x001DDA50
		public static void ApplyDecalProjectors(GraphicsSettingsModel m, GameObject root = null)
		{
			if (m == null)
			{
				return;
			}
			GraphicsSettingsApplier.PruneDestroyedDecalProjectors();
			foreach (DecalProjector decalProjector in (root != null) ? root.GetComponentsInChildren<DecalProjector>(true) : Object.FindObjectsOfType<DecalProjector>(true))
			{
				if (!(decalProjector == null))
				{
					bool enabled;
					if (!GraphicsSettingsApplier.DecalProjectorAuthoredState.TryGetValue(decalProjector, out enabled))
					{
						enabled = decalProjector.enabled;
						GraphicsSettingsApplier.DecalProjectorAuthoredState.Add(decalProjector, enabled);
					}
					bool flag = m.decals && enabled;
					if (decalProjector.enabled != flag)
					{
						decalProjector.enabled = flag;
					}
				}
			}
		}

		// Token: 0x06004D36 RID: 19766 RVA: 0x001DF8D8 File Offset: 0x001DDAD8
		private static void PruneDestroyedDecalProjectors()
		{
			GraphicsSettingsApplier.StaleDecalProjectors.Clear();
			foreach (KeyValuePair<DecalProjector, bool> keyValuePair in GraphicsSettingsApplier.DecalProjectorAuthoredState)
			{
				if (keyValuePair.Key == null)
				{
					GraphicsSettingsApplier.StaleDecalProjectors.Add(keyValuePair.Key);
				}
			}
			for (int i = 0; i < GraphicsSettingsApplier.StaleDecalProjectors.Count; i++)
			{
				GraphicsSettingsApplier.DecalProjectorAuthoredState.Remove(GraphicsSettingsApplier.StaleDecalProjectors[i]);
			}
			GraphicsSettingsApplier.StaleDecalProjectors.Clear();
		}

		// Token: 0x06004D37 RID: 19767 RVA: 0x001DF984 File Offset: 0x001DDB84
		public static void ApplyWorld(GraphicsSettingsModel m)
		{
			if (m == null)
			{
				return;
			}
			ChunkManager instance = ChunkManager.Instance;
			if (instance == null || !instance.isActiveAndEnabled || instance.IsPoiTravelSuspended)
			{
				return;
			}
			float num = ((instance.CellSize > 1f) ? instance.CellSize : ((GameSystemsBootstrapper.Instance != null) ? GameSystemsBootstrapper.Instance.CellSize : 60f));
			float num2 = ((float)((instance.Config != null) ? Mathf.Max(1, instance.Config.maxVisibleRadius) : 1) + 0.5f) * num;
			float num3 = Mathf.Clamp((m.viewDistanceScale <= 0f) ? 1f : m.viewDistanceScale, 0.4f, 1.5f);
			float num4 = Mathf.Min(num2 * 0.72f * num3, num2 * 0.8f);
			RenderSettings.fog = m.edgeFog;
			if (m.edgeFog)
			{
				RenderSettings.fogMode = FogMode.Linear;
				RenderSettings.fogStartDistance = Mathf.Max(0f, num4 - 8f);
				RenderSettings.fogEndDistance = num4;
			}
			Camera main = Camera.main;
			if (main != null)
			{
				main.farClipPlane = Mathf.Max(num4 * 1.05f, 40f);
			}
			GraphicsSettingsApplier.ApplyTerrainQuality(m, main, null);
		}

		// Token: 0x06004D38 RID: 19768 RVA: 0x001DFAC0 File Offset: 0x001DDCC0
		public static void ApplyDisplay(GraphicsSettingsModel m)
		{
			if (m == null)
			{
				return;
			}
			FullScreenMode fullScreenMode = (FullScreenMode)m.fullScreenMode;
			int num = m.resolutionWidth;
			int num2 = m.resolutionHeight;
			if (num <= 0 || num2 <= 0)
			{
				Resolution currentResolution = Screen.currentResolution;
				num = currentResolution.width;
				num2 = currentResolution.height;
			}
			if (Screen.width != num || Screen.height != num2 || Screen.fullScreenMode != fullScreenMode)
			{
				Screen.SetResolution(num, num2, fullScreenMode);
			}
		}

		// Token: 0x06004D39 RID: 19769 RVA: 0x001DFB24 File Offset: 0x001DDD24
		public static int SanitizeMsaa(int msaa)
		{
			if (msaa >= 8)
			{
				return 8;
			}
			if (msaa >= 4)
			{
				return 4;
			}
			if (msaa >= 2)
			{
				return 2;
			}
			return 1;
		}

		// Token: 0x04004EA0 RID: 20128
		private const string MainMenuSceneName = "MainMenu";

		// Token: 0x04004EA1 RID: 20129
		private static readonly Dictionary<DecalProjector, bool> DecalProjectorAuthoredState = new Dictionary<DecalProjector, bool>();

		// Token: 0x04004EA2 RID: 20130
		private static readonly List<DecalProjector> StaleDecalProjectors = new List<DecalProjector>();

		// Token: 0x04004EA3 RID: 20131
		private static readonly FieldInfo MainShadowResField = typeof(UniversalRenderPipelineAsset).GetField("m_MainLightShadowmapResolution", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04004EA4 RID: 20132
		private static readonly FieldInfo AdditionalShadowsField = typeof(UniversalRenderPipelineAsset).GetField("m_AdditionalLightShadowsSupported", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04004EA5 RID: 20133
		private static readonly FieldInfo AdditionalLightsModeField = typeof(UniversalRenderPipelineAsset).GetField("m_AdditionalLightsRenderingMode", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04004EA6 RID: 20134
		private static readonly FieldInfo MainShadowsField = typeof(UniversalRenderPipelineAsset).GetField("m_MainLightShadowsSupported", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04004EA7 RID: 20135
		private static readonly FieldInfo SoftShadowsField = typeof(UniversalRenderPipelineAsset).GetField("m_SoftShadowsSupported", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04004EA8 RID: 20136
		private const float FogFadeBandMeters = 8f;

		// Token: 0x04004EA9 RID: 20137
		private const float FogEndRatio = 0.72f;

		// Token: 0x04004EAA RID: 20138
		private const float FarClipFogMargin = 1.05f;

		// Token: 0x04004EAB RID: 20139
		private const float MaxFogEndRatio = 0.8f;
	}
}
