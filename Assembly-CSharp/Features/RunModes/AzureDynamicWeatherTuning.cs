using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AzureSky;

namespace Features.RunModes
{
	// Token: 0x02000C81 RID: 3201
	public static class AzureDynamicWeatherTuning
	{
		// Token: 0x06005CF9 RID: 23801 RVA: 0x0024816A File Offset: 0x0024636A
		public static AzureDynamicWeatherTuning.Snapshot Capture(AzureWeatherSystem weather)
		{
			if (weather != null)
			{
				return new AzureDynamicWeatherTuning.Snapshot(weather);
			}
			return null;
		}

		// Token: 0x06005CFA RID: 23802 RVA: 0x00248178 File Offset: 0x00246378
		public static void Apply(AzureWeatherSystem weather)
		{
			if (weather == null || weather.globalWeatherList == null)
			{
				return;
			}
			for (int i = 0; i < weather.globalWeatherList.Count; i++)
			{
				AzureGlobalWeather azureGlobalWeather = weather.globalWeatherList[i];
				AzureWeatherPreset azureWeatherPreset = ((azureGlobalWeather != null) ? azureGlobalWeather.weatherPreset : null);
				if (!(azureWeatherPreset == null))
				{
					string text = DynamicWeatherPolicy.NormalizeName(azureWeatherPreset.name);
					azureGlobalWeather.transitionTime = AzureDynamicWeatherTuning.TransitionSecondsFor(text);
					AzureDynamicWeatherTuning.TunePrecipitation(azureWeatherPreset, text);
					if (DynamicWeatherPolicy.IsConstellationPreset(text))
					{
						AzureDynamicWeatherTuning.TuneConstellation(azureWeatherPreset);
					}
					else if (text.Contains("SUNNY"))
					{
						AzureDynamicWeatherTuning.TuneSunny(azureWeatherPreset);
					}
					else if (text.Contains("OVERCAST") || text.Contains("CLOUD"))
					{
						AzureDynamicWeatherTuning.TuneOvercast(azureWeatherPreset);
					}
					else if (text.Contains("FOG"))
					{
						AzureDynamicWeatherTuning.TuneFog(azureWeatherPreset);
					}
					else if (text.Contains("STORM") || text.Contains("THUNDER"))
					{
						AzureDynamicWeatherTuning.TuneStormAudio(azureWeatherPreset);
					}
					AzureDynamicWeatherTuning.TuneNightLighting(azureWeatherPreset, text);
				}
			}
		}

		// Token: 0x06005CFB RID: 23803 RVA: 0x00248278 File Offset: 0x00246478
		private static float TransitionSecondsFor(string normalizedName)
		{
			if (normalizedName.Contains("STORM") || normalizedName.Contains("THUNDER"))
			{
				return 35f;
			}
			if (normalizedName.Contains("RAIN") || normalizedName.Contains("SNOW"))
			{
				return 30f;
			}
			if (normalizedName.Contains("FOG"))
			{
				return 45f;
			}
			return 20f;
		}

		// Token: 0x06005CFC RID: 23804 RVA: 0x002482E0 File Offset: 0x002464E0
		private static void TuneNightLighting(AzureWeatherPreset preset, string normalizedName)
		{
			float num = 0.22f;
			float num2 = 0.32f;
			if (normalizedName.Contains("STORM") || normalizedName.Contains("THUNDER"))
			{
				num = 0f;
				num2 = 0.23f;
			}
			else if (normalizedName.Contains("RAIN"))
			{
				num = 0f;
				num2 = 0.27f;
			}
			else if (normalizedName.Contains("FOG"))
			{
				num = 0.045f;
				num2 = 0.3f;
			}
			else if (normalizedName.Contains("OVERCAST") || normalizedName.Contains("CLOUD") || normalizedName.Contains("SNOW"))
			{
				num = 0.06f;
				num2 = 0.29f;
			}
			else if (normalizedName.Contains("SUNNY"))
			{
				num2 = 0.34f;
			}
			AzureDynamicWeatherTuning.SetNightAwareCurve(preset, "Directional Light", "Intensity", num);
			AzureDynamicWeatherTuning.SetNightAwareCurve(preset, "Environment", "Intensity Multiplier", num2);
			AzureDynamicWeatherTuning.TuneNightSkyColors(preset, normalizedName);
		}

		// Token: 0x06005CFD RID: 23805 RVA: 0x002483CC File Offset: 0x002465CC
		private static void TuneNightSkyColors(AzureWeatherPreset preset, string normalizedName)
		{
			bool flag = AzureDynamicWeatherTuning.IsAdverseWeather(normalizedName);
			bool flag2 = normalizedName.Contains("FOG");
			AzureDynamicWeatherTuning.SetNightAwareGradient(preset, "Directional Light", "Color", AzureDynamicWeatherTuning.NightDirectionalColor);
			AzureDynamicWeatherTuning.SetNightAwareGradient(preset, "Scattering", "Rayleigh Color", flag ? AzureDynamicWeatherTuning.AdverseNightRayleighColor : AzureDynamicWeatherTuning.ClearNightRayleighColor);
			AzureDynamicWeatherTuning.SetNightAwareGradient(preset, "Scattering", "Mie Color", flag ? AzureDynamicWeatherTuning.AdverseNightMieColor : AzureDynamicWeatherTuning.ClearNightMieColor);
			AzureDynamicWeatherTuning.SetNightAwareGradient(preset, "Dynamic Clouds", "Color 1", flag ? AzureDynamicWeatherTuning.AdverseNightCloudColor1 : AzureDynamicWeatherTuning.ClearNightCloudColor1);
			AzureDynamicWeatherTuning.SetNightAwareGradient(preset, "Dynamic Clouds", "Color 2", flag ? AzureDynamicWeatherTuning.AdverseNightCloudColor2 : AzureDynamicWeatherTuning.ClearNightCloudColor2);
			Color color = (flag2 ? AzureDynamicWeatherTuning.FogNightEnvironmentSkyColor : (flag ? AzureDynamicWeatherTuning.AdverseNightEnvironmentSkyColor : AzureDynamicWeatherTuning.ClearNightEnvironmentSkyColor));
			Color color2 = (flag2 ? AzureDynamicWeatherTuning.FogNightEnvironmentEquatorColor : (flag ? AzureDynamicWeatherTuning.AdverseNightEnvironmentEquatorColor : AzureDynamicWeatherTuning.ClearNightEnvironmentEquatorColor));
			Color color3 = (flag2 ? AzureDynamicWeatherTuning.FogNightEnvironmentGroundColor : (flag ? AzureDynamicWeatherTuning.AdverseNightEnvironmentGroundColor : AzureDynamicWeatherTuning.ClearNightEnvironmentGroundColor));
			AzureDynamicWeatherTuning.SetNightAwareGradient(preset, "Environment", "Sky Color", color);
			AzureDynamicWeatherTuning.SetNightAwareGradient(preset, "Environment", "Equator Color", color2);
			AzureDynamicWeatherTuning.SetNightAwareGradient(preset, "Environment", "Ground Color", color3);
		}

		// Token: 0x06005CFE RID: 23806 RVA: 0x00248504 File Offset: 0x00246704
		private static bool IsAdverseWeather(string normalizedName)
		{
			return normalizedName.Contains("STORM") || normalizedName.Contains("THUNDER") || normalizedName.Contains("RAIN") || normalizedName.Contains("FOG") || normalizedName.Contains("OVERCAST") || normalizedName.Contains("CLOUD") || normalizedName.Contains("SNOW");
		}

		// Token: 0x06005CFF RID: 23807 RVA: 0x0024856C File Offset: 0x0024676C
		private static bool SetNightAwareGradient(AzureWeatherPreset preset, string groupName, string propertyName, Color nightColor)
		{
			AzurePropertyData azurePropertyData = AzureDynamicWeatherTuning.FindProperty(preset, groupName, propertyName);
			Gradient gradient = ((azurePropertyData != null) ? azurePropertyData.gradientData : null);
			if (gradient == null)
			{
				return false;
			}
			Gradient gradient2 = new Gradient();
			gradient2.SetKeys(new GradientColorKey[]
			{
				new GradientColorKey(nightColor, 0f),
				new GradientColorKey(nightColor, 0.20833333f),
				new GradientColorKey(gradient.Evaluate(0.29166666f), 0.29166666f),
				new GradientColorKey(gradient.Evaluate(0.5f), 0.5f),
				new GradientColorKey(gradient.Evaluate(0.7083333f), 0.7083333f),
				new GradientColorKey(nightColor, 0.7916667f),
				new GradientColorKey(nightColor, 1f)
			}, gradient.alphaKeys);
			gradient2.mode = gradient.mode;
			gradient2.colorSpace = gradient.colorSpace;
			azurePropertyData.gradientData = gradient2;
			return true;
		}

		// Token: 0x06005D00 RID: 23808 RVA: 0x00248668 File Offset: 0x00246868
		private static bool SetNightAwareCurve(AzureWeatherPreset preset, string groupName, string propertyName, float nightValue)
		{
			AzurePropertyData azurePropertyData = AzureDynamicWeatherTuning.FindProperty(preset, groupName, propertyName);
			AnimationCurve animationCurve = ((azurePropertyData != null) ? azurePropertyData.curveData : null);
			if (animationCurve == null || animationCurve.length == 0)
			{
				return false;
			}
			float num = animationCurve.Evaluate(7f);
			float num2 = animationCurve.Evaluate(12f);
			float num3 = animationCurve.Evaluate(17f);
			AnimationCurve animationCurve2 = new AnimationCurve(new Keyframe[]
			{
				new Keyframe(0f, nightValue),
				new Keyframe(5f, nightValue),
				new Keyframe(6f, Mathf.Lerp(nightValue, num, 0.35f)),
				new Keyframe(7f, num),
				new Keyframe(12f, num2),
				new Keyframe(17f, num3),
				new Keyframe(18f, Mathf.Lerp(nightValue, num3, 0.35f)),
				new Keyframe(19f, nightValue),
				new Keyframe(24f, nightValue)
			})
			{
				preWrapMode = animationCurve.preWrapMode,
				postWrapMode = animationCurve.postWrapMode
			};
			azurePropertyData.curveData = animationCurve2;
			return true;
		}

		// Token: 0x06005D01 RID: 23809 RVA: 0x002487A8 File Offset: 0x002469A8
		private static void TuneSunny(AzureWeatherPreset preset)
		{
			AzureDynamicWeatherTuning.SetFloat(preset, "Fog Scattering", "Global Fog Distance", 5000f);
			AzureDynamicWeatherTuning.SetFloat(preset, "Fog Scattering", "Global Fog Smooth Step", 0.18f);
			AzureDynamicWeatherTuning.SetFloat(preset, "Fog Scattering", "Height Fog Distance", 250f);
			AzureDynamicWeatherTuning.SetFloat(preset, "Fog Scattering", "Height Fog Density", 0.12f);
			AzureDynamicWeatherTuning.SetFloat(preset, "Dynamic Clouds", "Density", 0.08f);
			AzureDynamicWeatherTuning.SetCurve(preset, "Directional Light", "Intensity", new AnimationCurve(new Keyframe[]
			{
				new Keyframe(0f, 0.3f),
				new Keyframe(5.5f, 0.3f),
				new Keyframe(7f, 0.8f),
				new Keyframe(12f, 1f),
				new Keyframe(17f, 0.9f),
				new Keyframe(18.5f, 0.35f),
				new Keyframe(24f, 0.3f)
			}));
			AzureDynamicWeatherTuning.SetCurve(preset, "Environment", "Intensity Multiplier", new AnimationCurve(new Keyframe[]
			{
				new Keyframe(0f, 0.35f),
				new Keyframe(6f, 0.4f),
				new Keyframe(8f, 1f),
				new Keyframe(12f, 1f),
				new Keyframe(18f, 0.8f),
				new Keyframe(20f, 0.4f),
				new Keyframe(24f, 0.35f)
			}));
		}

		// Token: 0x06005D02 RID: 23810 RVA: 0x00248990 File Offset: 0x00246B90
		private static void TuneOvercast(AzureWeatherPreset preset)
		{
			AzureDynamicWeatherTuning.SetFloat(preset, "Scattering", "Molecular Density", 0.35f);
			AzureDynamicWeatherTuning.SetCurve(preset, "Scattering", "Rayleigh", new AnimationCurve(new Keyframe[]
			{
				new Keyframe(0f, 3f),
				new Keyframe(5.5f, 3f),
				new Keyframe(7f, 0.9f),
				new Keyframe(17f, 0.9f),
				new Keyframe(18.5f, 3f),
				new Keyframe(24f, 3f)
			}));
			AzureDynamicWeatherTuning.SetCurve(preset, "Scattering", "Mie", new AnimationCurve(new Keyframe[]
			{
				new Keyframe(0f, 0.15f),
				new Keyframe(5.5f, 0.15f),
				new Keyframe(7f, 0.45f),
				new Keyframe(17f, 0.45f),
				new Keyframe(18.5f, 0.15f),
				new Keyframe(24f, 0.15f)
			}));
			AzureDynamicWeatherTuning.SetCurve(preset, "Directional Light", "Intensity", new AnimationCurve(new Keyframe[]
			{
				new Keyframe(0f, 0.28f),
				new Keyframe(5.5f, 0.28f),
				new Keyframe(7f, 0.62f),
				new Keyframe(12f, 0.72f),
				new Keyframe(17f, 0.62f),
				new Keyframe(18.5f, 0.28f),
				new Keyframe(24f, 0.28f)
			}));
			AzureDynamicWeatherTuning.SetFloat(preset, "Directional Light", "Shadows Strength", 0f);
			AzureDynamicWeatherTuning.SetCurve(preset, "Environment", "Intensity Multiplier", new AnimationCurve(new Keyframe[]
			{
				new Keyframe(0f, 0.35f),
				new Keyframe(6f, 0.4f),
				new Keyframe(8f, 0.82f),
				new Keyframe(12f, 0.88f),
				new Keyframe(18f, 0.75f),
				new Keyframe(20f, 0.4f),
				new Keyframe(24f, 0.35f)
			}));
		}

		// Token: 0x06005D03 RID: 23811 RVA: 0x00248C78 File Offset: 0x00246E78
		private static void TuneConstellation(AzureWeatherPreset preset)
		{
			AzureDynamicWeatherTuning.TuneSunny(preset);
			AzureDynamicWeatherTuning.SetFloat(preset, "Dynamic Clouds", "Density", 0.02f);
			AzureDynamicWeatherTuning.SetCurve(preset, "Outer Space", "Stars Intensity", AzureDynamicWeatherTuning.NightSkyCurve(1.8f));
			AzureDynamicWeatherTuning.SetCurve(preset, "Outer Space", "Milky Way Intensity", AzureDynamicWeatherTuning.NightSkyCurve(0.65f));
			AzureDynamicWeatherTuning.SetCurve(preset, "Outer Space", "Constellation Intensity", AzureDynamicWeatherTuning.NightSkyCurve(0.85f));
		}

		// Token: 0x06005D04 RID: 23812 RVA: 0x00248CF4 File Offset: 0x00246EF4
		private static AnimationCurve NightSkyCurve(float peak)
		{
			return new AnimationCurve(new Keyframe[]
			{
				new Keyframe(0f, peak),
				new Keyframe(5.5f, peak),
				new Keyframe(6.5f, 0f),
				new Keyframe(17.5f, 0f),
				new Keyframe(18.5f, peak),
				new Keyframe(24f, peak)
			});
		}

		// Token: 0x06005D05 RID: 23813 RVA: 0x00248D80 File Offset: 0x00246F80
		private static void TuneFog(AzureWeatherPreset preset)
		{
			AzureDynamicWeatherTuning.SetFloat(preset, "Fog Scattering", "Global Fog Distance", 10f);
			AzureDynamicWeatherTuning.SetFloat(preset, "Scattering", "Molecular Density", 0.55f);
			AzureDynamicWeatherTuning.SetCurve(preset, "Scattering", "Mie", AnimationCurve.Linear(0f, 1f, 24f, 1f));
			AzureDynamicWeatherTuning.SetFloat(preset, "Fog Scattering", "Global Fog Smooth Step", 0.05f);
			AzureDynamicWeatherTuning.SetFloat(preset, "Fog Scattering", "Global Fog Density", 1f);
			AzureDynamicWeatherTuning.SetFloat(preset, "Fog Scattering", "Height Fog Distance", 15f);
			AzureDynamicWeatherTuning.SetFloat(preset, "Fog Scattering", "Height Fog Smooth Step", 0.12f);
			AzureDynamicWeatherTuning.SetFloat(preset, "Fog Scattering", "Height Fog Density", 0.15f);
			AzureDynamicWeatherTuning.SetCurve(preset, "Fog Scattering", "Height Fog Scattering Multiplier", AnimationCurve.Linear(0f, 0.2f, 24f, 0.2f));
			AzureDynamicWeatherTuning.SetFloat(preset, "Dynamic Clouds", "Density", 0.7f);
		}

		// Token: 0x06005D06 RID: 23814 RVA: 0x00248E94 File Offset: 0x00247094
		private static void TunePrecipitation(AzureWeatherPreset preset, string normalizedName)
		{
			float num;
			float num2;
			if (DynamicWeatherPolicy.IsStormPreset(normalizedName))
			{
				num = 1100f;
				num2 = 1300f;
			}
			else if (normalizedName.Contains("CALMRAIN") || normalizedName.Contains("LIGHTRAIN") || normalizedName.Contains("DRIZZLE"))
			{
				num = 180f;
				num2 = 0f;
			}
			else
			{
				if (!normalizedName.Contains("RAIN"))
				{
					return;
				}
				num = 700f;
				num2 = 0f;
			}
			AzureDynamicWeatherTuning.SetFloat(preset, "Particles", "Rain Intensity", num);
			AzureDynamicWeatherTuning.SetFloat(preset, "Particles", "Storm Intensity", num2);
		}

		// Token: 0x06005D07 RID: 23815 RVA: 0x00248F2C File Offset: 0x0024712C
		private static void TuneStormAudio(AzureWeatherPreset preset)
		{
			AzureDynamicWeatherTuning.SetFloat(preset, "Sound FX", "Calm Rain Volume", 0.2f);
			AzureDynamicWeatherTuning.SetFloat(preset, "Sound FX", "Medium Rain Volume", 0.2f);
			AzureDynamicWeatherTuning.SetFloat(preset, "Sound FX", "Storm Rain Volume", 0.4f);
			AzureDynamicWeatherTuning.SetFloat(preset, "Sound FX", "Medium Wind Volume", 0.2f);
			AzureDynamicWeatherTuning.SetFloat(preset, "Sound FX", "Storm Wind Volume", 0.12f);
		}

		// Token: 0x06005D08 RID: 23816 RVA: 0x00248FA8 File Offset: 0x002471A8
		public static bool SetFloat(AzureWeatherPreset preset, string groupName, string propertyName, float value)
		{
			AzurePropertyData azurePropertyData = AzureDynamicWeatherTuning.FindProperty(preset, groupName, propertyName);
			if (azurePropertyData == null)
			{
				return false;
			}
			azurePropertyData.floatData = value;
			return true;
		}

		// Token: 0x06005D09 RID: 23817 RVA: 0x00248FCC File Offset: 0x002471CC
		public static bool SetCurve(AzureWeatherPreset preset, string groupName, string propertyName, AnimationCurve value)
		{
			AzurePropertyData azurePropertyData = AzureDynamicWeatherTuning.FindProperty(preset, groupName, propertyName);
			if (azurePropertyData == null)
			{
				return false;
			}
			azurePropertyData.curveData = value;
			return true;
		}

		// Token: 0x06005D0A RID: 23818 RVA: 0x00248FF0 File Offset: 0x002471F0
		private static AzurePropertyData FindProperty(AzureWeatherPreset preset, string groupName, string propertyName)
		{
			if (preset == null || preset.propertyGroupDataList == null)
			{
				return null;
			}
			for (int i = 0; i < preset.propertyGroupDataList.Count; i++)
			{
				AzurePropertyGroupData azurePropertyGroupData = preset.propertyGroupDataList[i];
				if (azurePropertyGroupData != null && !(azurePropertyGroupData.name != groupName) && azurePropertyGroupData.propertyDataList != null)
				{
					for (int j = 0; j < azurePropertyGroupData.propertyDataList.Count; j++)
					{
						AzurePropertyData azurePropertyData = azurePropertyGroupData.propertyDataList[j];
						if (azurePropertyData != null && azurePropertyData.weatherPropertyOwner != null && azurePropertyData.weatherPropertyOwner.name == propertyName)
						{
							return azurePropertyData;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06005D0B RID: 23819 RVA: 0x00249090 File Offset: 0x00247290
		private static AnimationCurve CloneCurve(AnimationCurve source)
		{
			if (source == null)
			{
				return null;
			}
			return new AnimationCurve(source.keys)
			{
				preWrapMode = source.preWrapMode,
				postWrapMode = source.postWrapMode
			};
		}

		// Token: 0x06005D0C RID: 23820 RVA: 0x002490BA File Offset: 0x002472BA
		private static Gradient CloneGradient(Gradient source)
		{
			if (source == null)
			{
				return null;
			}
			Gradient gradient = new Gradient();
			gradient.SetKeys(source.colorKeys, source.alphaKeys);
			gradient.mode = source.mode;
			gradient.colorSpace = source.colorSpace;
			return gradient;
		}

		// Token: 0x04005E4A RID: 24138
		public const float LightRainParticleRate = 180f;

		// Token: 0x04005E4B RID: 24139
		public const float NormalRainParticleRate = 700f;

		// Token: 0x04005E4C RID: 24140
		public const float StormRainParticleRate = 1100f;

		// Token: 0x04005E4D RID: 24141
		public const float StormFxParticleRate = 1300f;

		// Token: 0x04005E4E RID: 24142
		public const float FogWeatherTransitionSeconds = 45f;

		// Token: 0x04005E4F RID: 24143
		public const float ClearNightDirectionalIntensity = 0.22f;

		// Token: 0x04005E50 RID: 24144
		public const float ClearNightEnvironmentIntensity = 0.32f;

		// Token: 0x04005E51 RID: 24145
		public const float SunnyNightEnvironmentIntensity = 0.34f;

		// Token: 0x04005E52 RID: 24146
		public const float CloudyNightDirectionalIntensity = 0.06f;

		// Token: 0x04005E53 RID: 24147
		public const float CloudyNightEnvironmentIntensity = 0.29f;

		// Token: 0x04005E54 RID: 24148
		public const float FogNightDirectionalIntensity = 0.045f;

		// Token: 0x04005E55 RID: 24149
		public const float FogNightEnvironmentIntensity = 0.3f;

		// Token: 0x04005E56 RID: 24150
		public const float RainNightEnvironmentIntensity = 0.27f;

		// Token: 0x04005E57 RID: 24151
		public const float StormNightEnvironmentIntensity = 0.23f;

		// Token: 0x04005E58 RID: 24152
		public static readonly Color NightDirectionalColor = new Color(0.4f, 0.44f, 0.52f);

		// Token: 0x04005E59 RID: 24153
		public static readonly Color ClearNightRayleighColor = new Color(0.3f, 0.43f, 0.62f);

		// Token: 0x04005E5A RID: 24154
		public static readonly Color ClearNightMieColor = new Color(0.36f, 0.42f, 0.5f);

		// Token: 0x04005E5B RID: 24155
		public static readonly Color ClearNightCloudColor1 = new Color(0.27f, 0.32f, 0.4f);

		// Token: 0x04005E5C RID: 24156
		public static readonly Color ClearNightCloudColor2 = new Color(0.23f, 0.28f, 0.36f);

		// Token: 0x04005E5D RID: 24157
		public static readonly Color AdverseNightRayleighColor = new Color(0.22f, 0.31f, 0.45f);

		// Token: 0x04005E5E RID: 24158
		public static readonly Color AdverseNightMieColor = new Color(0.28f, 0.32f, 0.39f);

		// Token: 0x04005E5F RID: 24159
		public static readonly Color AdverseNightCloudColor1 = new Color(0.18f, 0.2f, 0.25f);

		// Token: 0x04005E60 RID: 24160
		public static readonly Color AdverseNightCloudColor2 = new Color(0.16f, 0.18f, 0.23f);

		// Token: 0x04005E61 RID: 24161
		public static readonly Color ClearNightEnvironmentSkyColor = new Color(0.18f, 0.23f, 0.34f);

		// Token: 0x04005E62 RID: 24162
		public static readonly Color ClearNightEnvironmentEquatorColor = new Color(0.14f, 0.17f, 0.23f);

		// Token: 0x04005E63 RID: 24163
		public static readonly Color ClearNightEnvironmentGroundColor = new Color(0.1f, 0.11f, 0.13f);

		// Token: 0x04005E64 RID: 24164
		public static readonly Color AdverseNightEnvironmentSkyColor = new Color(0.16f, 0.19f, 0.26f);

		// Token: 0x04005E65 RID: 24165
		public static readonly Color AdverseNightEnvironmentEquatorColor = new Color(0.13f, 0.15f, 0.2f);

		// Token: 0x04005E66 RID: 24166
		public static readonly Color AdverseNightEnvironmentGroundColor = new Color(0.1f, 0.105f, 0.12f);

		// Token: 0x04005E67 RID: 24167
		public static readonly Color FogNightEnvironmentSkyColor = new Color(0.19f, 0.21f, 0.25f);

		// Token: 0x04005E68 RID: 24168
		public static readonly Color FogNightEnvironmentEquatorColor = new Color(0.16f, 0.175f, 0.2f);

		// Token: 0x04005E69 RID: 24169
		public static readonly Color FogNightEnvironmentGroundColor = new Color(0.12f, 0.125f, 0.14f);

		// Token: 0x02000C82 RID: 3202
		public sealed class Snapshot
		{
			// Token: 0x06005D0E RID: 23822 RVA: 0x002492C0 File Offset: 0x002474C0
			internal Snapshot(AzureWeatherSystem weather)
			{
				if (weather == null || weather.globalWeatherList == null)
				{
					return;
				}
				HashSet<AzurePropertyData> hashSet = new HashSet<AzurePropertyData>();
				for (int i = 0; i < weather.globalWeatherList.Count; i++)
				{
					AzureGlobalWeather azureGlobalWeather = weather.globalWeatherList[i];
					if (azureGlobalWeather != null)
					{
						this._transitions.Add(new AzureDynamicWeatherTuning.Snapshot.TransitionState(azureGlobalWeather, azureGlobalWeather.transitionTime));
						AzureWeatherPreset weatherPreset = azureGlobalWeather.weatherPreset;
						if (!(weatherPreset == null) && weatherPreset.propertyGroupDataList != null)
						{
							for (int j = 0; j < weatherPreset.propertyGroupDataList.Count; j++)
							{
								AzurePropertyGroupData azurePropertyGroupData = weatherPreset.propertyGroupDataList[j];
								if (azurePropertyGroupData != null && azurePropertyGroupData.propertyDataList != null)
								{
									for (int k = 0; k < azurePropertyGroupData.propertyDataList.Count; k++)
									{
										AzurePropertyData azurePropertyData = azurePropertyGroupData.propertyDataList[k];
										if (azurePropertyData != null && hashSet.Add(azurePropertyData))
										{
											this._properties.Add(new AzureDynamicWeatherTuning.Snapshot.PropertyState(azurePropertyData, azurePropertyData.floatData, AzureDynamicWeatherTuning.CloneCurve(azurePropertyData.curveData), AzureDynamicWeatherTuning.CloneGradient(azurePropertyData.gradientData)));
										}
									}
								}
							}
						}
					}
				}
			}

			// Token: 0x06005D0F RID: 23823 RVA: 0x00249408 File Offset: 0x00247608
			public void Restore()
			{
				for (int i = 0; i < this._transitions.Count; i++)
				{
					AzureDynamicWeatherTuning.Snapshot.TransitionState transitionState = this._transitions[i];
					if (transitionState.Entry != null)
					{
						transitionState.Entry.transitionTime = transitionState.TransitionTime;
					}
				}
				for (int j = 0; j < this._properties.Count; j++)
				{
					AzureDynamicWeatherTuning.Snapshot.PropertyState propertyState = this._properties[j];
					if (propertyState.Data != null)
					{
						propertyState.Data.floatData = propertyState.FloatData;
						propertyState.Data.curveData = AzureDynamicWeatherTuning.CloneCurve(propertyState.CurveData);
						propertyState.Data.gradientData = AzureDynamicWeatherTuning.CloneGradient(propertyState.GradientData);
					}
				}
			}

			// Token: 0x04005E6A RID: 24170
			private readonly List<AzureDynamicWeatherTuning.Snapshot.TransitionState> _transitions = new List<AzureDynamicWeatherTuning.Snapshot.TransitionState>();

			// Token: 0x04005E6B RID: 24171
			private readonly List<AzureDynamicWeatherTuning.Snapshot.PropertyState> _properties = new List<AzureDynamicWeatherTuning.Snapshot.PropertyState>();

			// Token: 0x02000C83 RID: 3203
			private sealed class TransitionState
			{
				// Token: 0x06005D10 RID: 23824 RVA: 0x002494B9 File Offset: 0x002476B9
				public TransitionState(AzureGlobalWeather entry, float transitionTime)
				{
					this.Entry = entry;
					this.TransitionTime = transitionTime;
				}

				// Token: 0x04005E6C RID: 24172
				public readonly AzureGlobalWeather Entry;

				// Token: 0x04005E6D RID: 24173
				public readonly float TransitionTime;
			}

			// Token: 0x02000C84 RID: 3204
			private sealed class PropertyState
			{
				// Token: 0x06005D11 RID: 23825 RVA: 0x002494CF File Offset: 0x002476CF
				public PropertyState(AzurePropertyData data, float floatData, AnimationCurve curveData, Gradient gradientData)
				{
					this.Data = data;
					this.FloatData = floatData;
					this.CurveData = curveData;
					this.GradientData = gradientData;
				}

				// Token: 0x04005E6E RID: 24174
				public readonly AzurePropertyData Data;

				// Token: 0x04005E6F RID: 24175
				public readonly float FloatData;

				// Token: 0x04005E70 RID: 24176
				public readonly AnimationCurve CurveData;

				// Token: 0x04005E71 RID: 24177
				public readonly Gradient GradientData;
			}
		}
	}
}
