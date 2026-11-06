using System;
using System.Globalization;
using Features.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.AzureSky;

namespace Features.CielOS.Apps
{
	// Token: 0x0200141A RID: 5146
	public class CielOSWeatherClockApp : MonoBehaviour
	{
		// Token: 0x06009ED2 RID: 40658 RVA: 0x00415D18 File Offset: 0x00413F18
		private void OnEnable()
		{
			CielOSContentText.Bind(this.cityLabel, this.city);
			LocalizedGameText.BindFontOnly(this.timeLabel);
			LocalizedGameText.BindFontOnly(this.tempLabel);
			LocalizedGameText.BindComputed(this.dateLabel, () => CielOSWeatherClockApp.FormatLocalDate(DateTime.Now));
			this.FindAzure();
			this.UpdateNow();
		}

		// Token: 0x06009ED3 RID: 40659 RVA: 0x00415D85 File Offset: 0x00413F85
		private void Update()
		{
			if (Time.unscaledTime >= this._nextRefresh)
			{
				this.UpdateNow();
			}
		}

		// Token: 0x06009ED4 RID: 40660 RVA: 0x00415D9C File Offset: 0x00413F9C
		private void UpdateNow()
		{
			this._nextRefresh = Time.unscaledTime + Mathf.Max(0.1f, this.refreshInterval);
			if (this._azure == null)
			{
				this.FindAzure();
			}
			DateTime now = DateTime.Now;
			if (this.timeLabel != null)
			{
				this.timeLabel.text = now.ToString("HH:mm");
			}
			if (this.dateLabel != null)
			{
				this.dateLabel.text = CielOSWeatherClockApp.FormatLocalDate(now);
			}
			AzureWeatherSystem azureWeatherSystem = ((this._azure != null) ? this._azure.weatherSystem : null);
			string text = ((azureWeatherSystem != null && azureWeatherSystem.currentWeatherPreset != null) ? azureWeatherSystem.currentWeatherPreset.name : string.Empty);
			string text2 = ((azureWeatherSystem != null && azureWeatherSystem.targetWeatherPreset != null) ? azureWeatherSystem.targetWeatherPreset.name : string.Empty);
			int hour = now.Hour;
			string text3 = CielOSWeatherClockApp.ResolveDisplayedCondition(text, text2, azureWeatherSystem != null && azureWeatherSystem.isWeatherChanging, (azureWeatherSystem != null) ? azureWeatherSystem.weatherTransitionProgress : 0f, hour);
			LocalizedGameText.BindSource(this.conditionLabel, "phone_story", text3, null);
			if (this.tempLabel != null)
			{
				this.tempLabel.text = ((text3 == "Weather unavailable") ? "--°C" : (CielOSWeatherClockApp.EstimateTemperatureCelsius(now, text3).ToString() + "°C"));
			}
		}

		// Token: 0x06009ED5 RID: 40661 RVA: 0x00415F14 File Offset: 0x00414114
		private static string FormatLocalDate(DateTime localNow)
		{
			string currentCode = GameLocalization.CurrentCode;
			if (currentCode == "en")
			{
				return localNow.ToString("dddd, dd MMMM yyyy");
			}
			CultureInfo cultureInfo = CultureInfo.GetCultureInfo((currentCode == "zh-Hans") ? "zh-CN" : currentCode);
			return localNow.ToString("D", cultureInfo);
		}

		// Token: 0x06009ED6 RID: 40662 RVA: 0x00415F69 File Offset: 0x00414169
		private void FindAzure()
		{
			this._azure = Object.FindObjectOfType<AzureCoreSystem>();
		}

		// Token: 0x06009ED7 RID: 40663 RVA: 0x00415F78 File Offset: 0x00414178
		public static string ResolveDisplayedCondition(string currentPresetName, string targetPresetName, bool isWeatherChanging, float transitionProgress, int hour)
		{
			string text = currentPresetName;
			bool flag = !string.IsNullOrWhiteSpace(targetPresetName);
			if (string.IsNullOrWhiteSpace(text) && flag)
			{
				text = targetPresetName;
			}
			else if (isWeatherChanging && flag && Mathf.Clamp01(transitionProgress) >= 0.5f)
			{
				text = targetPresetName;
			}
			return CielOSWeatherClockApp.ResolveCondition(text, hour);
		}

		// Token: 0x06009ED8 RID: 40664 RVA: 0x00415FBC File Offset: 0x004141BC
		public static string ResolveCondition(string presetName, int hour)
		{
			string text = (presetName ?? string.Empty).Trim().ToLowerInvariant();
			if (text.Contains("storm") || text.Contains("thunder"))
			{
				return "Thunderstorm";
			}
			if (text.Contains("calmrain") || text.Contains("light rain") || text.Contains("lightrain") || text.Contains("drizzle"))
			{
				return "Light rain";
			}
			if (text.Contains("rain"))
			{
				return "Rain";
			}
			if (text.Contains("snow") || text.Contains("blizzard"))
			{
				return "Snow";
			}
			if (text.Contains("fog") || text.Contains("mist"))
			{
				return "Foggy";
			}
			if (text.Contains("overcast"))
			{
				return "Overcast";
			}
			if (text.Contains("cloud"))
			{
				return "Cloudy";
			}
			if (text.Contains("wind"))
			{
				return "Windy";
			}
			if (string.IsNullOrEmpty(text))
			{
				return "Weather unavailable";
			}
			if (text.Contains("default"))
			{
				return "Partly cloudy";
			}
			if (!text.Contains("sunny") && !text.Contains("clear"))
			{
				return presetName.Trim();
			}
			if (hour >= 6 && hour < 19)
			{
				return "Clear";
			}
			return "Clear night";
		}

		// Token: 0x06009ED9 RID: 40665 RVA: 0x0041611C File Offset: 0x0041431C
		public static int EstimateTemperatureCelsius(DateTime localTime, string condition)
		{
			int num = (DateTime.IsLeapYear(localTime.Year) ? 366 : 365);
			float num2 = (float)(15.0 + 10.0 * Math.Cos(6.283185307179586 * ((double)localTime.DayOfYear - 200.0) / (double)num));
			double num3 = (double)localTime.Hour + (double)localTime.Minute / 60.0;
			double num4 = 3.0 * Math.Cos(6.283185307179586 * (num3 - 15.0) / 24.0);
			double num5 = (double)((localTime.DayOfYear * 17 + localTime.Year) % 7 - 3) * 0.45;
			string text = (condition ?? string.Empty).Trim().ToLowerInvariant();
			double num6 = 0.0;
			if (text.Contains("thunder"))
			{
				num6 = -4.0;
			}
			else if (text.Contains("rain"))
			{
				num6 = -3.0;
			}
			else if (text.Contains("snow"))
			{
				num6 = -6.0;
			}
			else if (text.Contains("fog"))
			{
				num6 = -2.0;
			}
			else if (text.Contains("overcast"))
			{
				num6 = -1.5;
			}
			else if (text.Contains("cloud"))
			{
				num6 = -0.75;
			}
			else if (text.Contains("clear"))
			{
				num6 = 1.0;
			}
			int num7 = Mathf.RoundToInt((float)((double)num2 + num4 + num5 + num6));
			if (text.Contains("snow"))
			{
				num7 = Mathf.Min(num7, 1);
			}
			return Mathf.Clamp(num7, -12, 39);
		}

		// Token: 0x0400A2F7 RID: 41719
		[Header("Clock")]
		[SerializeField]
		private TMP_Text timeLabel;

		// Token: 0x0400A2F8 RID: 41720
		[SerializeField]
		private TMP_Text dateLabel;

		// Token: 0x0400A2F9 RID: 41721
		[Header("Weather")]
		[SerializeField]
		private TMP_Text cityLabel;

		// Token: 0x0400A2FA RID: 41722
		[SerializeField]
		private TMP_Text conditionLabel;

		// Token: 0x0400A2FB RID: 41723
		[SerializeField]
		private TMP_Text tempLabel;

		// Token: 0x0400A2FC RID: 41724
		[SerializeField]
		private string city = "Ciel City";

		// Token: 0x0400A2FD RID: 41725
		[SerializeField]
		[Min(0.1f)]
		private float refreshInterval = 0.5f;

		// Token: 0x0400A2FE RID: 41726
		private AzureCoreSystem _azure;

		// Token: 0x0400A2FF RID: 41727
		private float _nextRefresh;
	}
}
