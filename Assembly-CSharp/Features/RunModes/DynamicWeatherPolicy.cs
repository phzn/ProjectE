using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Features.RunModes
{
	// Token: 0x02000CA5 RID: 3237
	public static class DynamicWeatherPolicy
	{
		// Token: 0x17000F35 RID: 3893
		// (get) Token: 0x06005DF4 RID: 24052 RVA: 0x00252666 File Offset: 0x00250866
		public static string[] AuthoredPresetNames
		{
			get
			{
				return (string[])DynamicWeatherPolicy.AuthoredNames.Clone();
			}
		}

		// Token: 0x06005DF5 RID: 24053 RVA: 0x00252678 File Offset: 0x00250878
		public static string NormalizeName(string value)
		{
			if (!string.IsNullOrWhiteSpace(value))
			{
				return value.Replace("_", string.Empty).Replace("-", string.Empty).Replace(" ", string.Empty)
					.ToUpperInvariant()
					.Replace("CONSTALLATION", "CONSTELLATION");
			}
			return string.Empty;
		}

		// Token: 0x06005DF6 RID: 24054 RVA: 0x002526D8 File Offset: 0x002508D8
		public static bool IsConstellationPreset(string value)
		{
			string text = DynamicWeatherPolicy.NormalizeName(value);
			return text.Contains("SUNNYCONSTELLATION") || text.Contains("SUNNYCONSTALLATION");
		}

		// Token: 0x06005DF7 RID: 24055 RVA: 0x00252708 File Offset: 0x00250908
		public static bool IsStormPreset(string value)
		{
			string text = DynamicWeatherPolicy.NormalizeName(value);
			return text.Contains("STORM") || text.Contains("THUNDER");
		}

		// Token: 0x06005DF8 RID: 24056 RVA: 0x00252736 File Offset: 0x00250936
		public static bool IsFogPreset(string value)
		{
			return DynamicWeatherPolicy.NormalizeName(value).Contains("FOG");
		}

		// Token: 0x06005DF9 RID: 24057 RVA: 0x00252748 File Offset: 0x00250948
		public static bool AllowsFogInScene(string sceneName)
		{
			return string.IsNullOrEmpty(sceneName) || !sceneName.StartsWith("POI - ", StringComparison.Ordinal);
		}

		// Token: 0x06005DFA RID: 24058 RVA: 0x00252764 File Offset: 0x00250964
		public static int ResolveAllowedIndex(IReadOnlyList<string> names, int requestedIndex, bool allowFog)
		{
			DynamicWeatherPolicy.<>c__DisplayClass12_0 CS$<>8__locals1;
			CS$<>8__locals1.names = names;
			CS$<>8__locals1.allowFog = allowFog;
			if (CS$<>8__locals1.names == null || CS$<>8__locals1.names.Count == 0)
			{
				return -1;
			}
			if (DynamicWeatherPolicy.<ResolveAllowedIndex>g__Eligible|12_0(requestedIndex, ref CS$<>8__locals1))
			{
				return requestedIndex;
			}
			int num = -1;
			for (int i = 0; i < CS$<>8__locals1.names.Count; i++)
			{
				if (DynamicWeatherPolicy.<ResolveAllowedIndex>g__Eligible|12_0(i, ref CS$<>8__locals1))
				{
					if (DynamicWeatherPolicy.NormalizeName(CS$<>8__locals1.names[i]) == "DEFAULT")
					{
						return i;
					}
					if (num < 0)
					{
						num = i;
					}
				}
			}
			return num;
		}

		// Token: 0x06005DFB RID: 24059 RVA: 0x002527EC File Offset: 0x002509EC
		public static bool IsNightHour(float hour)
		{
			float num = Mathf.Repeat(hour, 24f);
			return num < 6.5f || num >= 18.5f;
		}

		// Token: 0x06005DFC RID: 24060 RVA: 0x0025281C File Offset: 0x00250A1C
		public static float GetSelectionWeight(string value)
		{
			string text = DynamicWeatherPolicy.NormalizeName(value);
			if (string.IsNullOrEmpty(text))
			{
				return 0f;
			}
			if (text.Contains("DEFAULT"))
			{
				return 28f;
			}
			if (DynamicWeatherPolicy.IsConstellationPreset(text))
			{
				return 6f;
			}
			if (text.Contains("SUNNY"))
			{
				return 24f;
			}
			if (text.Contains("OVERCAST") || text.Contains("CLOUD"))
			{
				return 18f;
			}
			if (text.Contains("STORM") || text.Contains("THUNDER"))
			{
				return 2f;
			}
			if (text.Contains("CALMRAIN") || text.Contains("LIGHTRAIN") || text.Contains("DRIZZLE"))
			{
				return 11f;
			}
			if (text.Contains("RAIN"))
			{
				return 8f;
			}
			if (text.Contains("FOG"))
			{
				return 5f;
			}
			if (text.Contains("SNOW"))
			{
				return 2f;
			}
			return 4f;
		}

		// Token: 0x06005DFD RID: 24061 RVA: 0x0025291D File Offset: 0x00250B1D
		public static float GetSelectionWeight(string value, float timelineHour)
		{
			if (DynamicWeatherPolicy.IsConstellationPreset(value) && !DynamicWeatherPolicy.IsNightHour(timelineHour))
			{
				return 0f;
			}
			return DynamicWeatherPolicy.GetSelectionWeight(value);
		}

		// Token: 0x06005DFE RID: 24062 RVA: 0x0025293B File Offset: 0x00250B3B
		public static int SelectNextIndex(IReadOnlyList<string> presetNames, int currentIndex, float roll01)
		{
			return DynamicWeatherPolicy.SelectNextIndex(presetNames, currentIndex, roll01, 0f, false, null, true);
		}

		// Token: 0x06005DFF RID: 24063 RVA: 0x0025294D File Offset: 0x00250B4D
		public static int SelectNextIndex(IReadOnlyList<string> presetNames, int currentIndex, float roll01, float timelineHour)
		{
			return DynamicWeatherPolicy.SelectNextIndex(presetNames, currentIndex, roll01, timelineHour, true, null, true);
		}

		// Token: 0x06005E00 RID: 24064 RVA: 0x0025295B File Offset: 0x00250B5B
		public static int SelectNextIndex(IReadOnlyList<string> presetNames, int currentIndex, float roll01, float timelineHour, IReadOnlyList<string> allowedPresetNames, bool allowFog = true)
		{
			return DynamicWeatherPolicy.SelectNextIndex(presetNames, currentIndex, roll01, timelineHour, true, allowedPresetNames, allowFog);
		}

		// Token: 0x06005E01 RID: 24065 RVA: 0x0025296C File Offset: 0x00250B6C
		private static int SelectNextIndex(IReadOnlyList<string> presetNames, int currentIndex, float roll01, float timelineHour, bool filterByTimeline, IReadOnlyList<string> allowedPresetNames, bool allowFog = true)
		{
			if (presetNames == null || presetNames.Count == 0)
			{
				return -1;
			}
			HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
			if (allowedPresetNames != null)
			{
				for (int i = 0; i < allowedPresetNames.Count; i++)
				{
					string text = DynamicWeatherPolicy.NormalizeName(allowedPresetNames[i]);
					if (!string.IsNullOrEmpty(text))
					{
						hashSet.Add(text);
					}
				}
			}
			bool flag = false;
			if (hashSet.Count > 0)
			{
				for (int j = 0; j < presetNames.Count; j++)
				{
					if ((allowFog || !DynamicWeatherPolicy.IsFogPreset(presetNames[j])) && hashSet.Contains(DynamicWeatherPolicy.NormalizeName(presetNames[j])))
					{
						flag = true;
						break;
					}
				}
			}
			float num = 0f;
			for (int k = 0; k < presetNames.Count; k++)
			{
				if (k != currentIndex && (allowFog || !DynamicWeatherPolicy.IsFogPreset(presetNames[k])) && (!flag || hashSet.Contains(DynamicWeatherPolicy.NormalizeName(presetNames[k]))))
				{
					num += (filterByTimeline ? DynamicWeatherPolicy.GetSelectionWeight(presetNames[k], timelineHour) : DynamicWeatherPolicy.GetSelectionWeight(presetNames[k]));
				}
			}
			if (num <= 0f)
			{
				return DynamicWeatherPolicy.ResolveAllowedIndex(presetNames, Mathf.Clamp(currentIndex, 0, presetNames.Count - 1), allowFog);
			}
			float num2 = Mathf.Min(Mathf.Clamp01(roll01), 0.999999f) * num;
			int num3 = Mathf.Clamp(currentIndex, 0, presetNames.Count - 1);
			for (int l = 0; l < presetNames.Count; l++)
			{
				if (l != currentIndex && (allowFog || !DynamicWeatherPolicy.IsFogPreset(presetNames[l])) && (!flag || hashSet.Contains(DynamicWeatherPolicy.NormalizeName(presetNames[l]))))
				{
					float num4 = (filterByTimeline ? DynamicWeatherPolicy.GetSelectionWeight(presetNames[l], timelineHour) : DynamicWeatherPolicy.GetSelectionWeight(presetNames[l]));
					if (num4 > 0f)
					{
						num3 = l;
						if (num2 < num4)
						{
							return l;
						}
						num2 -= num4;
					}
				}
			}
			return num3;
		}

		// Token: 0x06005E02 RID: 24066 RVA: 0x00252B50 File Offset: 0x00250D50
		public static float GetClassicDelaySeconds(float roll01, bool initialForecast)
		{
			float num = (initialForecast ? 120f : 180f);
			float num2 = (initialForecast ? 240f : 360f);
			return Mathf.Lerp(num, num2, Mathf.Clamp01(roll01));
		}

		// Token: 0x06005E03 RID: 24067 RVA: 0x00252B88 File Offset: 0x00250D88
		public static float GetThunderDelaySeconds(float roll01)
		{
			return Mathf.Lerp(12f, 26f, Mathf.Clamp01(roll01));
		}

		// Token: 0x06005E05 RID: 24069 RVA: 0x00252C04 File Offset: 0x00250E04
		[CompilerGenerated]
		internal static bool <ResolveAllowedIndex>g__Eligible|12_0(int index, ref DynamicWeatherPolicy.<>c__DisplayClass12_0 A_1)
		{
			return index >= 0 && index < A_1.names.Count && !string.IsNullOrEmpty(A_1.names[index]) && (A_1.allowFog || !DynamicWeatherPolicy.IsFogPreset(A_1.names[index]));
		}

		// Token: 0x04005EFC RID: 24316
		private static readonly string[] AuthoredNames = new string[] { "Default", "Sunny", "Overcast", "CalmRain", "Rain", "Storm", "Fogness", "Snow", "Sunny-Constallation" };

		// Token: 0x04005EFD RID: 24317
		private const float InitialDelayMinSeconds = 120f;

		// Token: 0x04005EFE RID: 24318
		private const float InitialDelayMaxSeconds = 240f;

		// Token: 0x04005EFF RID: 24319
		private const float RegularDelayMinSeconds = 180f;

		// Token: 0x04005F00 RID: 24320
		private const float RegularDelayMaxSeconds = 360f;
	}
}
