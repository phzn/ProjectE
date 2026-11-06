using System;
using Features.RunModes;
using ProjectExhibitionist;
using UnityEngine;
using UnityEngine.AzureSky;

namespace Features.RainCoat
{
	// Token: 0x02000DBB RID: 3515
	public class AzureRainWetBridge : MonoBehaviour
	{
		// Token: 0x060067A8 RID: 26536 RVA: 0x00296F76 File Offset: 0x00295176
		private void Awake()
		{
			this._wetOwner = string.Format("AzureRainWetBridge:{0}", base.GetInstanceID());
			if (this.wetController == null)
			{
				this.wetController = base.GetComponent<CielWetController>();
			}
		}

		// Token: 0x060067A9 RID: 26537 RVA: 0x00296FB0 File Offset: 0x002951B0
		private void Update()
		{
			if (Time.time < this._nextPoll)
			{
				return;
			}
			this._nextPoll = Time.time + Mathf.Max(0.25f, this.pollInterval);
			if (this._azure == null)
			{
				this._azure = Object.FindObjectOfType<AzureCoreSystem>();
				if (this._azure == null)
				{
					return;
				}
			}
			AzureWeatherSystem weatherSystem = this._azure.weatherSystem;
			if (weatherSystem == null || this.wetController == null)
			{
				return;
			}
			AzureWeatherPreset azureWeatherPreset = ((weatherSystem.isWeatherChanging && weatherSystem.targetWeatherPreset != null) ? weatherSystem.targetWeatherPreset : weatherSystem.currentWeatherPreset);
			bool flag = azureWeatherPreset != null && this.IsRainy(azureWeatherPreset.name) && !RainShelterRuntime.IsShelteredAt(base.transform.position);
			if (flag != this._rainy && this.debugLogs)
			{
				Debug.Log(string.Format("[AzureRainWetBridge] Meteo '{0}' → rainy={1}", (azureWeatherPreset != null) ? azureWeatherPreset.name : "null", flag), this);
			}
			if (flag != this._rainy || flag)
			{
				this._rainy = flag;
				if (flag)
				{
					this.wetController.AcquireWet(this._wetOwner);
					return;
				}
				this.wetController.ReleaseWet(this._wetOwner);
			}
		}

		// Token: 0x060067AA RID: 26538 RVA: 0x002970FC File Offset: 0x002952FC
		private bool IsRainy(string presetName)
		{
			if (string.IsNullOrEmpty(presetName) || this.rainyPresetTokens == null)
			{
				return false;
			}
			string text = presetName.ToLowerInvariant();
			foreach (string text2 in this.rainyPresetTokens)
			{
				if (!string.IsNullOrEmpty(text2) && text.Contains(text2.ToLowerInvariant()))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060067AB RID: 26539 RVA: 0x00297154 File Offset: 0x00295354
		private void OnDisable()
		{
			if (this.wetController != null)
			{
				this.wetController.ReleaseWet(this._wetOwner);
			}
			this._nextPoll = 0f;
			this._rainy = false;
		}

		// Token: 0x04006A23 RID: 27171
		[Tooltip("Wet controller pilotato (auto: sullo stesso GameObject).")]
		[SerializeField]
		private CielWetController wetController;

		// Token: 0x04006A24 RID: 27172
		[Tooltip("Il preset meteo è 'piovoso' se il suo nome contiene uno di questi token (case-insensitive).")]
		[SerializeField]
		private string[] rainyPresetTokens = new string[] { "rain", "storm" };

		// Token: 0x04006A25 RID: 27173
		[Tooltip("Ogni quanti secondi leggere lo stato meteo di Azure.")]
		[SerializeField]
		private float pollInterval = 1f;

		// Token: 0x04006A26 RID: 27174
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04006A27 RID: 27175
		private AzureCoreSystem _azure;

		// Token: 0x04006A28 RID: 27176
		private float _nextPoll;

		// Token: 0x04006A29 RID: 27177
		private bool _rainy;

		// Token: 0x04006A2A RID: 27178
		private string _wetOwner;
	}
}
