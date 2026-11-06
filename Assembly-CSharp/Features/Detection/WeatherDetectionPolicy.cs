using System;
using Features.RunModes;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x02001310 RID: 4880
	public static class WeatherDetectionPolicy
	{
		// Token: 0x06009754 RID: 38740 RVA: 0x003E0FA0 File Offset: 0x003DF1A0
		public static WeatherDetectionKind Classify(string presetName)
		{
			string text = DynamicWeatherPolicy.NormalizeName(presetName);
			if (DynamicWeatherPolicy.IsStormPreset(text))
			{
				return WeatherDetectionKind.Storm;
			}
			if (text.Contains("CALMRAIN") || text.Contains("LIGHTRAIN") || text.Contains("DRIZZLE"))
			{
				return WeatherDetectionKind.LightRain;
			}
			if (text.Contains("RAIN"))
			{
				return WeatherDetectionKind.Rain;
			}
			return WeatherDetectionKind.Clear;
		}

		// Token: 0x06009755 RID: 38741 RVA: 0x003E0FF8 File Offset: 0x003DF1F8
		public static WeatherDetectionModifiers Evaluate(string currentPresetName, string targetPresetName, bool isWeatherChanging, float transitionProgress, float fogBlend)
		{
			WeatherDetectionPolicy.RainProfile rainProfile = WeatherDetectionPolicy.ProfileFor(WeatherDetectionPolicy.Classify(currentPresetName));
			bool flag = !string.IsNullOrWhiteSpace(targetPresetName);
			WeatherDetectionPolicy.RainProfile rainProfile2 = ((isWeatherChanging && flag) ? WeatherDetectionPolicy.ProfileFor(WeatherDetectionPolicy.Classify(targetPresetName)) : rainProfile);
			float num = ((isWeatherChanging && flag) ? Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(transitionProgress)) : 0f);
			return new WeatherDetectionModifiers(Mathf.Lerp(rainProfile.ViewRadius, rainProfile2.ViewRadius, num), Mathf.Lerp(rainProfile.VisualRate, rainProfile2.VisualRate, num), fogBlend);
		}

		// Token: 0x06009756 RID: 38742 RVA: 0x003E107C File Offset: 0x003DF27C
		public static float ApplyViewRadius(float authoredViewRadius, WeatherDetectionModifiers modifiers, bool alreadySeeingPlayer = false)
		{
			float num = Mathf.Max(0f, authoredViewRadius) * modifiers.ViewRadiusMultiplier;
			float num2 = Mathf.Min(num, 5.5f);
			float num3 = Mathf.SmoothStep(0f, 1f, modifiers.FogBlend);
			float num4 = Mathf.Lerp(num, num2, num3);
			if (!alreadySeeingPlayer)
			{
				return num4;
			}
			return Mathf.Min(num, num4 + 0.5f * num3);
		}

		// Token: 0x06009757 RID: 38743 RVA: 0x003E10E0 File Offset: 0x003DF2E0
		public static float GetVisualRateMultiplier(WeatherDetectionModifiers modifiers, float distanceMeters)
		{
			float num = Mathf.Lerp(1f, 0.45f, modifiers.FogBlend);
			if (modifiers.FogBlend > 0f)
			{
				float num2 = Mathf.InverseLerp(3f, 5.5f, Mathf.Max(0f, distanceMeters));
				float num3 = 1f - Mathf.SmoothStep(0f, 1f, num2);
				num = Mathf.Lerp(num, 1f, num3 * modifiers.FogBlend);
			}
			return modifiers.VisualRateMultiplier * num;
		}

		// Token: 0x06009758 RID: 38744 RVA: 0x003E1164 File Offset: 0x003DF364
		public static bool IsWithinFieldOfView(Vector3 forward, Vector3 toPlayer, float cosineHalfAngle)
		{
			if (toPlayer.sqrMagnitude > 9f)
			{
				return Vector3.Dot(forward, toPlayer.normalized) >= cosineHalfAngle;
			}
			forward.y = 0f;
			toPlayer.y = 0f;
			return toPlayer.sqrMagnitude < 0.0001f || (forward.sqrMagnitude >= 0.0001f && Vector3.Dot(forward.normalized, toPlayer.normalized) >= cosineHalfAngle);
		}

		// Token: 0x06009759 RID: 38745 RVA: 0x003E11E4 File Offset: 0x003DF3E4
		private static WeatherDetectionPolicy.RainProfile ProfileFor(WeatherDetectionKind kind)
		{
			switch (kind)
			{
			case WeatherDetectionKind.LightRain:
				return new WeatherDetectionPolicy.RainProfile(0.9f, 0.75f);
			case WeatherDetectionKind.Rain:
				return new WeatherDetectionPolicy.RainProfile(0.78f, 0.55f);
			case WeatherDetectionKind.Storm:
				return new WeatherDetectionPolicy.RainProfile(0.62f, 0.35f);
			default:
				return new WeatherDetectionPolicy.RainProfile(1f, 1f);
			}
		}

		// Token: 0x04009C8B RID: 40075
		public const float FogMaximumViewDistanceMeters = 5.5f;

		// Token: 0x04009C8C RID: 40076
		public const float FogContactFullRateDistanceMeters = 3f;

		// Token: 0x04009C8D RID: 40077
		public const float FogEdgeVisualRateMultiplier = 0.45f;

		// Token: 0x04009C8E RID: 40078
		public const float FogSightRetentionMeters = 0.5f;

		// Token: 0x04009C8F RID: 40079
		public const float NearFacingDistanceMeters = 3f;

		// Token: 0x04009C90 RID: 40080
		public const float LightRainViewRadiusMultiplier = 0.9f;

		// Token: 0x04009C91 RID: 40081
		public const float LightRainVisualRateMultiplier = 0.75f;

		// Token: 0x04009C92 RID: 40082
		public const float RainViewRadiusMultiplier = 0.78f;

		// Token: 0x04009C93 RID: 40083
		public const float RainVisualRateMultiplier = 0.55f;

		// Token: 0x04009C94 RID: 40084
		public const float StormViewRadiusMultiplier = 0.62f;

		// Token: 0x04009C95 RID: 40085
		public const float StormVisualRateMultiplier = 0.35f;

		// Token: 0x02001311 RID: 4881
		private readonly struct RainProfile
		{
			// Token: 0x0600975A RID: 38746 RVA: 0x003E1246 File Offset: 0x003DF446
			public RainProfile(float viewRadius, float visualRate)
			{
				this.ViewRadius = viewRadius;
				this.VisualRate = visualRate;
			}

			// Token: 0x04009C96 RID: 40086
			public readonly float ViewRadius;

			// Token: 0x04009C97 RID: 40087
			public readonly float VisualRate;
		}
	}
}
