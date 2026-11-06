using System;
using UnityEngine.Events;
using UnityEngine.Profiling;
using UnityEngine.UI;

namespace UnityEngine.AzureSky
{
	// Token: 0x0200002A RID: 42
	public class AzureDemoUI : MonoBehaviour
	{
		// Token: 0x0600026A RID: 618 RVA: 0x0000A09B File Offset: 0x0000829B
		private void Start()
		{
			Screen.SetResolution(1920, 1080, true);
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000A0AD File Offset: 0x000082AD
		private void OnEnable()
		{
			AzureNotificationCenter.OnWeatherTransitionEnd += this.OnWeatherTransitionEnd;
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000A0C0 File Offset: 0x000082C0
		private void OnDisable()
		{
			AzureNotificationCenter.OnWeatherTransitionEnd -= this.OnWeatherTransitionEnd;
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000A0D4 File Offset: 0x000082D4
		private void Update()
		{
			this.m_allocatedMemoryText.text = ((int)((float)Profiler.GetTotalAllocatedMemoryLong() / 1048576f)).ToString() + "MB";
			this.m_reservedMemoryText.text = ((int)((float)Profiler.GetTotalReservedMemoryLong() / 1048576f)).ToString() + "MB";
			this.m_monoMemoryText.text = ((int)((float)Profiler.GetMonoUsedSizeLong() / 1048576f)).ToString() + "MB";
			this.m_deltaTime += Time.unscaledDeltaTime;
			this.m_frameCount++;
			if (this.m_deltaTime > 1f / (float)this.m_updateRate)
			{
				this.m_fps = (float)this.m_frameCount / this.m_deltaTime;
				this.m_ms = this.m_deltaTime / (float)this.m_frameCount * 1000f;
				this.m_fpsText.text = Mathf.RoundToInt(this.m_fps).ToString() + "fps";
				this.m_msText.text = this.m_ms.ToString("0.0") + "ms";
				this.m_deltaTime = 0f;
				this.m_frameCount = 0;
			}
			if (this.m_azureCoreSystem.weatherSystem.isWeatherChanging)
			{
				this.m_weatherTransitionBar.transform.localScale = new Vector3(this.m_azureCoreSystem.weatherSystem.weatherTransitionProgress, 1f, 1f);
			}
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				Application.Quit();
			}
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000A273 File Offset: 0x00008473
		public void EnableDisableTerrain(Toggle toggle)
		{
			this.m_terrainTransform.gameObject.SetActive(toggle.isOn);
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000A28C File Offset: 0x0000848C
		public void SetScreenResolution(Dropdown dropdown)
		{
			switch (dropdown.value)
			{
			case 0:
				Screen.SetResolution(3840, 2160, true);
				return;
			case 1:
				Screen.SetResolution(2560, 1440, true);
				return;
			case 2:
				Screen.SetResolution(1920, 1080, true);
				return;
			case 3:
				Screen.SetResolution(1280, 720, true);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000A2FC File Offset: 0x000084FC
		public void SetFogScatteringResolution(Dropdown dropdown)
		{
			switch (dropdown.value)
			{
			case 0:
				this.m_fogScattering.SetFogScatteringResolution(3840, 2160);
				return;
			case 1:
				this.m_fogScattering.SetFogScatteringResolution(2560, 1440);
				return;
			case 2:
				this.m_fogScattering.SetFogScatteringResolution(1920, 1080);
				return;
			case 3:
				this.m_fogScattering.SetFogScatteringResolution(1280, 720);
				return;
			case 4:
				this.m_fogScattering.SetFogScatteringResolution(960, 540);
				return;
			case 5:
				this.m_fogScattering.SetFogScatteringResolution(640, 360);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000A3B4 File Offset: 0x000085B4
		public void SetTimeMode(Dropdown dropdown)
		{
			int value = dropdown.value;
			if (value == 0)
			{
				this.m_dayInputField.transform.parent.gameObject.SetActive(false);
				this.m_monthInputField.transform.parent.gameObject.SetActive(false);
				this.m_yearInputField.transform.parent.gameObject.SetActive(false);
				this.m_latitudeInputField.transform.parent.gameObject.SetActive(false);
				this.m_longitudeInputField.transform.parent.gameObject.SetActive(false);
				this.m_utcInputField.transform.parent.gameObject.SetActive(false);
				this.m_azureCoreSystem.timeSystem.timeMode = AzureTimeMode.Simple;
				this.m_azureCoreSystem.timeSystem.latitude = 0f;
				this.m_azureCoreSystem.timeSystem.longitude = 0f;
				this.m_azureCoreSystem.timeSystem.utc = 0f;
				this.m_azureCoreSystem.timeSystem.SetDate(1, 1, 2024);
				return;
			}
			if (value != 1)
			{
				return;
			}
			this.m_dayInputField.transform.parent.gameObject.SetActive(true);
			this.m_monthInputField.transform.parent.gameObject.SetActive(true);
			this.m_yearInputField.transform.parent.gameObject.SetActive(true);
			this.m_latitudeInputField.transform.parent.gameObject.SetActive(true);
			this.m_longitudeInputField.transform.parent.gameObject.SetActive(true);
			this.m_utcInputField.transform.parent.gameObject.SetActive(true);
			this.m_azureCoreSystem.timeSystem.timeMode = AzureTimeMode.Realistic;
			this.m_azureCoreSystem.timeSystem.latitude = (float)int.Parse(this.m_latitudeInputField.text);
			this.m_azureCoreSystem.timeSystem.longitude = (float)int.Parse(this.m_longitudeInputField.text);
			this.m_azureCoreSystem.timeSystem.utc = (float)int.Parse(this.m_utcInputField.text);
			this.m_azureCoreSystem.timeSystem.SetDate(int.Parse(this.m_dayInputField.text), int.Parse(this.m_monthInputField.text), int.Parse(this.m_yearInputField.text));
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000A634 File Offset: 0x00008834
		public void UpdateTimeSettings()
		{
			this.m_azureCoreSystem.timeSystem.latitude = (float)int.Parse(this.m_latitudeInputField.text);
			this.m_azureCoreSystem.timeSystem.longitude = (float)int.Parse(this.m_longitudeInputField.text);
			this.m_azureCoreSystem.timeSystem.utc = (float)int.Parse(this.m_utcInputField.text);
			this.m_azureCoreSystem.timeSystem.SetDate(int.Parse(this.m_dayInputField.text), int.Parse(this.m_monthInputField.text), int.Parse(this.m_yearInputField.text));
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000A6E4 File Offset: 0x000088E4
		public void ApplicationQuit()
		{
			Application.Quit();
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000A6EB File Offset: 0x000088EB
		public void OnUpdateDynamicGIToggleChanged(Toggle toggle)
		{
			if (toggle.isOn)
			{
				this.m_azureCoreSystem.onCoreUpdateEvent.SetPersistentListenerState(1, UnityEventCallState.RuntimeOnly);
				return;
			}
			this.m_azureCoreSystem.onCoreUpdateEvent.SetPersistentListenerState(1, UnityEventCallState.Off);
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000A71A File Offset: 0x0000891A
		public void OnUpdateReflectionProbeToggleChanged(Toggle toggle)
		{
			if (toggle.isOn)
			{
				this.m_azureCoreSystem.onCoreUpdateEvent.SetPersistentListenerState(2, UnityEventCallState.EditorAndRuntime);
				return;
			}
			this.m_azureCoreSystem.onCoreUpdateEvent.SetPersistentListenerState(2, UnityEventCallState.Off);
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000A749 File Offset: 0x00008949
		public void OnVSyncToggleChanged(Toggle toggle)
		{
			if (toggle.isOn)
			{
				QualitySettings.vSyncCount = 1;
				return;
			}
			QualitySettings.vSyncCount = 0;
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000A760 File Offset: 0x00008960
		public void EnableDisableVolumetricLights(Toggle toggle)
		{
			this.m_volumetricLightRenderer.enabled = toggle.isOn;
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000A773 File Offset: 0x00008973
		public void EnableDisableFogScattering(Toggle toggle)
		{
			this.m_fogScatteringRenderer.enabled = toggle.isOn;
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000A788 File Offset: 0x00008988
		public void EnableDisableParticleSubEmitters(Toggle toggle)
		{
			this.m_rainParticle.subEmitters.enabled = toggle.isOn;
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000A7AE File Offset: 0x000089AE
		public void SetGlobalWeather(int index)
		{
			this.m_azureCoreSystem.weatherSystem.SetGlobalWeather(index);
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000A7C1 File Offset: 0x000089C1
		public void InstantiateThunder()
		{
			this.m_azureCoreSystem.weatherSystem.InstantiateThunderPrefab(0);
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000A7D4 File Offset: 0x000089D4
		public void SetTimeline(Slider slider)
		{
			this.m_azureCoreSystem.timeSystem.timeline = slider.value;
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000A7EC File Offset: 0x000089EC
		public void SetCameraAltitude(Slider slider)
		{
			this.m_camera.transform.position = new Vector3(0f, slider.value, 0f);
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000A813 File Offset: 0x00008A13
		private void OnWeatherTransitionEnd(AzureWeatherSystem azureWeatherSystem)
		{
			this.m_weatherTransitionBar.transform.localScale = new Vector3(0f, 1f, 1f);
		}

		// Token: 0x040001AE RID: 430
		[Header("Main References")]
		[SerializeField]
		private Camera m_camera;

		// Token: 0x040001AF RID: 431
		[SerializeField]
		private AzureCoreSystem m_azureCoreSystem;

		// Token: 0x040001B0 RID: 432
		[SerializeField]
		private AzureFogScatteringRenderer m_fogScattering;

		// Token: 0x040001B1 RID: 433
		[SerializeField]
		private Transform m_terrainTransform;

		// Token: 0x040001B2 RID: 434
		[SerializeField]
		private AzureVolumetricLightRenderer m_volumetricLightRenderer;

		// Token: 0x040001B3 RID: 435
		[SerializeField]
		private AzureFogScatteringRenderer m_fogScatteringRenderer;

		// Token: 0x040001B4 RID: 436
		[SerializeField]
		private ParticleSystem m_rainParticle;

		// Token: 0x040001B5 RID: 437
		[Header("Time UI References")]
		[SerializeField]
		private InputField m_dayInputField;

		// Token: 0x040001B6 RID: 438
		[SerializeField]
		private InputField m_monthInputField;

		// Token: 0x040001B7 RID: 439
		[SerializeField]
		private InputField m_yearInputField;

		// Token: 0x040001B8 RID: 440
		[SerializeField]
		private InputField m_latitudeInputField;

		// Token: 0x040001B9 RID: 441
		[SerializeField]
		private InputField m_longitudeInputField;

		// Token: 0x040001BA RID: 442
		[SerializeField]
		private InputField m_utcInputField;

		// Token: 0x040001BB RID: 443
		[Header("Stats UI References")]
		[SerializeField]
		private Text m_fpsText;

		// Token: 0x040001BC RID: 444
		[SerializeField]
		private Text m_msText;

		// Token: 0x040001BD RID: 445
		[SerializeField]
		private Text m_reservedMemoryText;

		// Token: 0x040001BE RID: 446
		[SerializeField]
		private Text m_allocatedMemoryText;

		// Token: 0x040001BF RID: 447
		[SerializeField]
		private Text m_monoMemoryText;

		// Token: 0x040001C0 RID: 448
		private int m_updateRate = 4;

		// Token: 0x040001C1 RID: 449
		private int m_frameCount;

		// Token: 0x040001C2 RID: 450
		private float m_deltaTime;

		// Token: 0x040001C3 RID: 451
		private float m_fps;

		// Token: 0x040001C4 RID: 452
		private float m_ms;

		// Token: 0x040001C5 RID: 453
		[Header("Weather UI References")]
		[SerializeField]
		private Image m_weatherTransitionBar;

		// Token: 0x040001C6 RID: 454
		private Vector3 m_scale = Vector3.one;
	}
}
