using System;
using UnityEngine;

namespace ProjectExhibitionist.Core.Rendering
{
	// Token: 0x02000AA0 RID: 2720
	[Serializable]
	public class GraphicsSettingsModel
	{
		// Token: 0x06004D8D RID: 19853 RVA: 0x001E1329 File Offset: 0x001DF529
		public GraphicsSettingsModel Clone()
		{
			return (GraphicsSettingsModel)base.MemberwiseClone();
		}

		// Token: 0x06004D8E RID: 19854 RVA: 0x001E1336 File Offset: 0x001DF536
		public string ToJson()
		{
			return JsonUtility.ToJson(this);
		}

		// Token: 0x06004D8F RID: 19855 RVA: 0x001E133E File Offset: 0x001DF53E
		public void SetFrameRateLimit(int fps)
		{
			this.fpsCap = ((fps > 0) ? Mathf.Clamp(fps, 30, 360) : 0);
			if (this.fpsCap > 0)
			{
				this.vSync = false;
			}
		}

		// Token: 0x06004D90 RID: 19856 RVA: 0x001E136A File Offset: 0x001DF56A
		public void SetVerticalSync(bool enabled)
		{
			this.vSync = enabled;
			if (enabled)
			{
				this.fpsCap = 0;
			}
		}

		// Token: 0x06004D91 RID: 19857 RVA: 0x001E137D File Offset: 0x001DF57D
		public static float SanitizeBrightness(float value)
		{
			if (!float.IsNaN(value) && !float.IsInfinity(value))
			{
				return Mathf.Clamp(value, 0.5f, 2f);
			}
			return 1f;
		}

		// Token: 0x06004D92 RID: 19858 RVA: 0x001E13A8 File Offset: 0x001DF5A8
		public static GraphicsSettingsModel FromJson(string json)
		{
			if (string.IsNullOrEmpty(json))
			{
				return null;
			}
			GraphicsSettingsModel graphicsSettingsModel2;
			try
			{
				GraphicsSettingsModel graphicsSettingsModel = JsonUtility.FromJson<GraphicsSettingsModel>(json);
				if (graphicsSettingsModel == null)
				{
					graphicsSettingsModel2 = null;
				}
				else
				{
					if (graphicsSettingsModel.fpsCap > 0)
					{
						graphicsSettingsModel.SetFrameRateLimit(graphicsSettingsModel.fpsCap);
					}
					graphicsSettingsModel.brightness = ((json.IndexOf("\"brightness\"", StringComparison.Ordinal) < 0) ? 1f : GraphicsSettingsModel.SanitizeBrightness(graphicsSettingsModel.brightness));
					if (json.IndexOf("\"mapLighting\"", StringComparison.Ordinal) < 0)
					{
						bool flag = ((json.IndexOf("\"performanceMap\"", StringComparison.Ordinal) >= 0) ? JsonUtility.FromJson<GraphicsSettingsModel.LegacyMapSettings>(json).performanceMap : (!graphicsSettingsModel.shadowsEnabled && graphicsSettingsModel.renderScale <= 0.8f && !graphicsSettingsModel.postProcessing));
						graphicsSettingsModel.mapLighting = (flag ? GraphicsSettingsModel.MapLightingQuality.Performance : GraphicsSettingsModel.MapLightingQuality.Original);
					}
					graphicsSettingsModel.mapLighting = (GraphicsSettingsModel.MapLightingQuality)Mathf.Clamp((int)graphicsSettingsModel.mapLighting, 0, 2);
					if (json.IndexOf("\"magicaClothEnabled\"", StringComparison.Ordinal) < 0)
					{
						graphicsSettingsModel.magicaClothEnabled = true;
					}
					if (json.IndexOf("\"opaqueTexture\"", StringComparison.Ordinal) < 0)
					{
						bool flag2 = !graphicsSettingsModel.shadowsEnabled && graphicsSettingsModel.renderScale <= 0.8f && !graphicsSettingsModel.postProcessing;
						graphicsSettingsModel.opaqueTexture = !flag2;
						graphicsSettingsModel.decals = !flag2;
						if (flag2)
						{
							graphicsSettingsModel.textureQuality = 0;
						}
					}
					graphicsSettingsModel.additionalLightsPerObject = Mathf.Clamp(graphicsSettingsModel.additionalLightsPerObject, 1, 8);
					graphicsSettingsModel2 = graphicsSettingsModel;
				}
			}
			catch
			{
				graphicsSettingsModel2 = null;
			}
			return graphicsSettingsModel2;
		}

		// Token: 0x06004D93 RID: 19859 RVA: 0x001E1518 File Offset: 0x001DF718
		public bool QualityEquals(GraphicsSettingsModel other)
		{
			return other != null && (Mathf.Approximately(this.renderScale, other.renderScale) && this.upscaling == other.upscaling && (this.upscaling != GraphicsSettingsModel.UpscalingMode.FSR || Mathf.Approximately(this.fsrSharpness, other.fsrSharpness)) && this.hdr == other.hdr && this.opaqueTexture == other.opaqueTexture && this.decals == other.decals && this.msaa == other.msaa && this.postAA == other.postAA && this.shadowsEnabled == other.shadowsEnabled && this.softShadows == other.softShadows && this.additionalLightsPerObject == other.additionalLightsPerObject && Mathf.Approximately(this.shadowDistance, other.shadowDistance) && this.shadowCascades == other.shadowCascades && this.mainShadowResolution == other.mainShadowResolution && this.ssao == other.ssao && this.textureQuality == other.textureQuality && Mathf.Approximately(this.lodBias, other.lodBias) && this.anisotropic == other.anisotropic && this.realtimeReflectionProbes == other.realtimeReflectionProbes && this.postProcessing == other.postProcessing && this.bloom == other.bloom && this.motionBlur == other.motionBlur && this.depthOfField == other.depthOfField && this.filmGrain == other.filmGrain && this.chromaticAberration == other.chromaticAberration && Mathf.Approximately(this.viewDistanceScale, other.viewDistanceScale)) && this.mapLighting == other.mapLighting;
		}

		// Token: 0x06004D94 RID: 19860 RVA: 0x001E1700 File Offset: 0x001DF900
		public void CopyQualityFrom(GraphicsSettingsModel preset)
		{
			this.renderScale = preset.renderScale;
			this.upscaling = preset.upscaling;
			this.fsrSharpness = preset.fsrSharpness;
			this.hdr = preset.hdr;
			this.opaqueTexture = preset.opaqueTexture;
			this.decals = preset.decals;
			this.msaa = preset.msaa;
			this.postAA = preset.postAA;
			this.shadowsEnabled = preset.shadowsEnabled;
			this.softShadows = preset.softShadows;
			this.additionalLightsPerObject = preset.additionalLightsPerObject;
			this.shadowDistance = preset.shadowDistance;
			this.shadowCascades = preset.shadowCascades;
			this.mainShadowResolution = preset.mainShadowResolution;
			this.ssao = preset.ssao;
			this.textureQuality = preset.textureQuality;
			this.lodBias = preset.lodBias;
			this.anisotropic = preset.anisotropic;
			this.realtimeReflectionProbes = preset.realtimeReflectionProbes;
			this.postProcessing = preset.postProcessing;
			this.bloom = preset.bloom;
			this.motionBlur = preset.motionBlur;
			this.depthOfField = preset.depthOfField;
			this.filmGrain = preset.filmGrain;
			this.chromaticAberration = preset.chromaticAberration;
			this.viewDistanceScale = preset.viewDistanceScale;
			this.mapLighting = preset.mapLighting;
		}

		// Token: 0x04004ED6 RID: 20182
		public const float DefaultBrightness = 1f;

		// Token: 0x04004ED7 RID: 20183
		public const float MinBrightness = 0.5f;

		// Token: 0x04004ED8 RID: 20184
		public const float MaxBrightness = 2f;

		// Token: 0x04004ED9 RID: 20185
		[Header("Display")]
		[Tooltip("0 = risoluzione desktop corrente")]
		public int resolutionWidth;

		// Token: 0x04004EDA RID: 20186
		public int resolutionHeight;

		// Token: 0x04004EDB RID: 20187
		public int fullScreenMode = 1;

		// Token: 0x04004EDC RID: 20188
		public bool vSync;

		// Token: 0x04004EDD RID: 20189
		[Tooltip("0 = nessun cap")]
		public int fpsCap;

		// Token: 0x04004EDE RID: 20190
		[Header("Rendering")]
		[Range(0.5f, 1.5f)]
		public float renderScale = 1f;

		// Token: 0x04004EDF RID: 20191
		public GraphicsSettingsModel.UpscalingMode upscaling;

		// Token: 0x04004EE0 RID: 20192
		[Range(0f, 1f)]
		public float fsrSharpness = 0.92f;

		// Token: 0x04004EE1 RID: 20193
		public bool hdr = true;

		// Token: 0x04004EE2 RID: 20194
		[Tooltip("Genera la Camera Opaque Texture. Disabilitarla evita una copia full-screen.")]
		public bool opaqueTexture = true;

		// Token: 0x04004EE3 RID: 20195
		[Tooltip("Abilita la renderer feature dei decal URP.")]
		public bool decals = true;

		// Token: 0x04004EE4 RID: 20196
		[Tooltip("1 = off, poi 2/4/8")]
		public int msaa = 4;

		// Token: 0x04004EE5 RID: 20197
		public GraphicsSettingsModel.PostAAMode postAA;

		// Token: 0x04004EE6 RID: 20198
		[Header("Shadows")]
		public bool shadowsEnabled = true;

		// Token: 0x04004EE7 RID: 20199
		public bool softShadows = true;

		// Token: 0x04004EE8 RID: 20200
		[Tooltip("Legacy per-object quota for Forward photo rendering. World Forward+ lighting ignores this value.")]
		[Range(1f, 8f)]
		public int additionalLightsPerObject = 4;

		// Token: 0x04004EE9 RID: 20201
		[Range(20f, 200f)]
		public float shadowDistance = 100f;

		// Token: 0x04004EEA RID: 20202
		[Range(1f, 4f)]
		public int shadowCascades = 4;

		// Token: 0x04004EEB RID: 20203
		[Tooltip("512/1024/2048/4096")]
		public int mainShadowResolution = 2048;

		// Token: 0x04004EEC RID: 20204
		[Header("World")]
		[Tooltip("Requires a new run. Performance: 4 street lights per zone prefab; Balanced: 16; Original: authored lighting.")]
		public GraphicsSettingsModel.MapLightingQuality mapLighting = GraphicsSettingsModel.MapLightingQuality.Original;

		// Token: 0x04004EED RID: 20205
		[Tooltip("DEPRECATO: i chunk spawnati sono governati SOLO dal ChunkManagerConfig, il menu non li tocca")]
		[HideInInspector]
		public int viewDistance;

		// Token: 0x04004EEE RID: 20206
		[Tooltip("Foschia sul bordo mappa, agganciata al raggio chunk corrente")]
		public bool edgeFog = true;

		// Token: 0x04004EEF RID: 20207
		[Tooltip("Scala il far clip e le distanze Terrain; nella mappa procedurale rispetta il limite coperto dalla foschia senza cambiare i chunk.")]
		[Range(0.35f, 2f)]
		public float viewDistanceScale = 1f;

		// Token: 0x04004EF0 RID: 20208
		[Header("Quality")]
		public bool ssao = true;

		// Token: 0x04004EF1 RID: 20209
		[Tooltip("0 = 1/8, 1 = 1/4, 2 = 1/2, 3 = full resolution (texture con mipmap)")]
		[Range(0f, 3f)]
		public int textureQuality = 3;

		// Token: 0x04004EF2 RID: 20210
		[Range(0.5f, 2f)]
		public float lodBias = 1f;

		// Token: 0x04004EF3 RID: 20211
		public GraphicsSettingsModel.AnisoMode anisotropic = GraphicsSettingsModel.AnisoMode.Force8x;

		// Token: 0x04004EF4 RID: 20212
		public bool realtimeReflectionProbes = true;

		// Token: 0x04004EF5 RID: 20213
		[Header("Post FX")]
		public bool postProcessing = true;

		// Token: 0x04004EF6 RID: 20214
		public bool bloom = true;

		// Token: 0x04004EF7 RID: 20215
		public bool motionBlur;

		// Token: 0x04004EF8 RID: 20216
		public bool depthOfField = true;

		// Token: 0x04004EF9 RID: 20217
		public bool filmGrain = true;

		// Token: 0x04004EFA RID: 20218
		public bool chromaticAberration = true;

		// Token: 0x04004EFB RID: 20219
		[Header("Accessibility")]
		[Tooltip("Monitor brightness correction. 1 = authored appearance; higher values reveal dark tones.")]
		[Range(0.5f, 2f)]
		public float brightness = 1f;

		// Token: 0x04004EFC RID: 20220
		public GraphicsSettingsModel.ColorVisionMode colorVision;

		// Token: 0x04004EFD RID: 20221
		[Range(0f, 1f)]
		public float colorVisionStrength = 1f;

		// Token: 0x04004EFE RID: 20222
		[Header("Gameplay Performance")]
		[Tooltip("Attiva o sospende globalmente MagicaCloth; la frequenza resta stabile.")]
		public bool magicaClothEnabled = true;

		// Token: 0x02000AA1 RID: 2721
		public enum UpscalingMode
		{
			// Token: 0x04004F00 RID: 20224
			Linear,
			// Token: 0x04004F01 RID: 20225
			FSR
		}

		// Token: 0x02000AA2 RID: 2722
		public enum PostAAMode
		{
			// Token: 0x04004F03 RID: 20227
			None,
			// Token: 0x04004F04 RID: 20228
			FXAA,
			// Token: 0x04004F05 RID: 20229
			SMAA
		}

		// Token: 0x02000AA3 RID: 2723
		public enum AnisoMode
		{
			// Token: 0x04004F07 RID: 20231
			Disabled,
			// Token: 0x04004F08 RID: 20232
			PerTexture,
			// Token: 0x04004F09 RID: 20233
			Force8x,
			// Token: 0x04004F0A RID: 20234
			Force16x
		}

		// Token: 0x02000AA4 RID: 2724
		public enum ColorVisionMode
		{
			// Token: 0x04004F0C RID: 20236
			Normal,
			// Token: 0x04004F0D RID: 20237
			Protanopia,
			// Token: 0x04004F0E RID: 20238
			Deuteranopia,
			// Token: 0x04004F0F RID: 20239
			Tritanopia
		}

		// Token: 0x02000AA5 RID: 2725
		public enum MapLightingQuality
		{
			// Token: 0x04004F11 RID: 20241
			Performance,
			// Token: 0x04004F12 RID: 20242
			Balanced,
			// Token: 0x04004F13 RID: 20243
			Original
		}

		// Token: 0x02000AA6 RID: 2726
		[Serializable]
		private sealed class LegacyMapSettings
		{
			// Token: 0x04004F14 RID: 20244
			public bool performanceMap;
		}
	}
}
