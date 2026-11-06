using System;
using Features.CielOS.Apps;
using Features.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.AzureSky;
using UnityEngine.UI;

namespace ProjektSumperk
{
	// Token: 0x02000411 RID: 1041
	public class WeatherController : MonoBehaviour
	{
		// Token: 0x06001A6E RID: 6766 RVA: 0x000A7090 File Offset: 0x000A5290
		private void OnEnable()
		{
			this.RefreshFromAzure();
		}

		// Token: 0x06001A6F RID: 6767 RVA: 0x000A7098 File Offset: 0x000A5298
		private void Update()
		{
			if (Time.unscaledTime >= this._nextRefresh)
			{
				this.RefreshFromAzure();
			}
		}

		// Token: 0x06001A70 RID: 6768 RVA: 0x000A70B0 File Offset: 0x000A52B0
		private void RefreshFromAzure()
		{
			this._nextRefresh = Time.unscaledTime + 0.5f;
			if (this._azure == null)
			{
				this._azure = Object.FindObjectOfType<AzureCoreSystem>();
			}
			AzureWeatherSystem azureWeatherSystem = ((this._azure != null) ? this._azure.weatherSystem : null);
			string text = ((azureWeatherSystem != null && azureWeatherSystem.currentWeatherPreset != null) ? azureWeatherSystem.currentWeatherPreset.name : string.Empty);
			string text2 = ((azureWeatherSystem != null && azureWeatherSystem.targetWeatherPreset != null) ? azureWeatherSystem.targetWeatherPreset.name : string.Empty);
			int num = ((this._azure != null && this._azure.timeSystem != null) ? this._azure.timeSystem.hour : DateTime.Now.Hour);
			string text3 = CielOSWeatherClockApp.ResolveDisplayedCondition(text, text2, azureWeatherSystem != null && azureWeatherSystem.isWeatherChanging, (azureWeatherSystem != null) ? azureWeatherSystem.weatherTransitionProgress : 0f, num);
			int num2 = CielOSWeatherClockApp.EstimateTemperatureCelsius(this.ResolvePresentationDate(num), text3);
			this.SetWeather("Tokyo", (float)num2, text3);
		}

		// Token: 0x06001A71 RID: 6769 RVA: 0x000A71CC File Offset: 0x000A53CC
		private DateTime ResolvePresentationDate(int hour)
		{
			if (this._azure == null || this._azure.timeSystem == null)
			{
				return DateTime.Now;
			}
			DateTime dateTime;
			try
			{
				dateTime = new DateTime(this._azure.timeSystem.year, this._azure.timeSystem.month, this._azure.timeSystem.day, Mathf.Clamp(hour, 0, 23), Mathf.Clamp(this._azure.timeSystem.minute, 0, 59), 0);
			}
			catch (ArgumentOutOfRangeException)
			{
				dateTime = DateTime.Now.Date.AddHours((double)Mathf.Clamp(hour, 0, 23));
			}
			return dateTime;
		}

		// Token: 0x06001A72 RID: 6770 RVA: 0x000A728C File Offset: 0x000A548C
		public void SetWeather(string city, float temperature, string weatherCondition)
		{
			if (this.cityText != null)
			{
				LocalizedGameText.BindSource(this.cityText, "phone_story", city, null);
			}
			if (this.temperatureValue != null)
			{
				this.temperatureValue.text = temperature.ToString("F0") + "°C";
			}
			string text = (weatherCondition ?? string.Empty).ToLowerInvariant();
			Sprite sprite;
			if (text.Contains("rain") || text.Contains("storm") || text.Contains("snow"))
			{
				sprite = this.rainyIcon;
			}
			else if (text.Contains("cloud") || text.Contains("overcast") || text.Contains("fog") || text.Contains("mist"))
			{
				sprite = this.overcastIcon;
			}
			else
			{
				sprite = this.sunnyIcon;
			}
			if (this.wIcon != null)
			{
				this.wIcon.sprite = sprite;
			}
			if (this.condition != null)
			{
				LocalizedGameText.BindComputed(this.condition, () => GameLocalization.GetSource("phone_story", string.IsNullOrWhiteSpace(weatherCondition) ? "Weather unavailable" : weatherCondition));
			}
		}

		// Token: 0x04001B81 RID: 7041
		public TMP_Text cityText;

		// Token: 0x04001B82 RID: 7042
		public TMP_Text temperatureValue;

		// Token: 0x04001B83 RID: 7043
		public Sprite sunnyIcon;

		// Token: 0x04001B84 RID: 7044
		public Sprite rainyIcon;

		// Token: 0x04001B85 RID: 7045
		public Sprite overcastIcon;

		// Token: 0x04001B86 RID: 7046
		public Image wIcon;

		// Token: 0x04001B87 RID: 7047
		public TMP_Text condition;

		// Token: 0x04001B88 RID: 7048
		private AzureCoreSystem _azure;

		// Token: 0x04001B89 RID: 7049
		private float _nextRefresh;
	}
}
