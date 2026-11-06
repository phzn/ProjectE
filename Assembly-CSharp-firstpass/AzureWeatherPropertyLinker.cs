using System;

namespace UnityEngine.AzureSky
{
	// Token: 0x02000026 RID: 38
	public sealed class AzureWeatherPropertyLinker : MonoBehaviour
	{
		// Token: 0x14000010 RID: 16
		// (add) Token: 0x06000208 RID: 520 RVA: 0x00007B64 File Offset: 0x00005D64
		// (remove) Token: 0x06000209 RID: 521 RVA: 0x00007B98 File Offset: 0x00005D98
		public static event Action<Light> EnvironmentLightingUpdated;

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x0600020A RID: 522 RVA: 0x00007BCB File Offset: 0x00005DCB
		// (set) Token: 0x0600020B RID: 523 RVA: 0x00007BD2 File Offset: 0x00005DD2
		public static bool SuppressPrecipitation { get; set; }

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x0600020C RID: 524 RVA: 0x00007BDA File Offset: 0x00005DDA
		// (set) Token: 0x0600020D RID: 525 RVA: 0x00007BE1 File Offset: 0x00005DE1
		public static Func<float, float> RainVolumeFilter { get; set; }

		// Token: 0x0600020E RID: 526 RVA: 0x00007BE9 File Offset: 0x00005DE9
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetSceneOverrides()
		{
			AzureWeatherPropertyLinker.SuppressPrecipitation = false;
			AzureWeatherPropertyLinker.RainVolumeFilter = null;
			AzureWeatherPropertyLinker.EnvironmentLightingUpdated = null;
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x0600020F RID: 527 RVA: 0x00007BFD File Offset: 0x00005DFD
		// (set) Token: 0x06000210 RID: 528 RVA: 0x00007C05 File Offset: 0x00005E05
		public AzureSkyRenderer azureSkyRenderer
		{
			get
			{
				return this.m_azureSkyRenderer;
			}
			set
			{
				this.m_azureSkyRenderer = value;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000211 RID: 529 RVA: 0x00007C0E File Offset: 0x00005E0E
		// (set) Token: 0x06000212 RID: 530 RVA: 0x00007C16 File Offset: 0x00005E16
		public Light directionalLight
		{
			get
			{
				return this.m_directionalLight;
			}
			set
			{
				this.m_directionalLight = value;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000213 RID: 531 RVA: 0x00007C1F File Offset: 0x00005E1F
		// (set) Token: 0x06000214 RID: 532 RVA: 0x00007C27 File Offset: 0x00005E27
		public AzureParticleFX rainParticleFX
		{
			get
			{
				return this.m_rainParticleFX;
			}
			set
			{
				this.m_rainParticleFX = value;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000215 RID: 533 RVA: 0x00007C30 File Offset: 0x00005E30
		// (set) Token: 0x06000216 RID: 534 RVA: 0x00007C38 File Offset: 0x00005E38
		public AzureParticleFX stormParticleFX
		{
			get
			{
				return this.m_stormParticleFX;
			}
			set
			{
				this.m_stormParticleFX = value;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000217 RID: 535 RVA: 0x00007C41 File Offset: 0x00005E41
		// (set) Token: 0x06000218 RID: 536 RVA: 0x00007C49 File Offset: 0x00005E49
		public AzureParticleFX snowParticleFX
		{
			get
			{
				return this.m_snowParticleFX;
			}
			set
			{
				this.m_snowParticleFX = value;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000219 RID: 537 RVA: 0x00007C52 File Offset: 0x00005E52
		// (set) Token: 0x0600021A RID: 538 RVA: 0x00007C5A File Offset: 0x00005E5A
		public AzureSoundFX calmRainSoundFX
		{
			get
			{
				return this.m_calmRainSoundFX;
			}
			set
			{
				this.m_calmRainSoundFX = value;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600021B RID: 539 RVA: 0x00007C63 File Offset: 0x00005E63
		// (set) Token: 0x0600021C RID: 540 RVA: 0x00007C6B File Offset: 0x00005E6B
		public AzureSoundFX mediumRainSoundFX
		{
			get
			{
				return this.m_mediumRainSoundFX;
			}
			set
			{
				this.m_mediumRainSoundFX = value;
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x0600021D RID: 541 RVA: 0x00007C74 File Offset: 0x00005E74
		// (set) Token: 0x0600021E RID: 542 RVA: 0x00007C7C File Offset: 0x00005E7C
		public AzureSoundFX stormRainSoundFX
		{
			get
			{
				return this.m_stormRainSoundFX;
			}
			set
			{
				this.m_stormRainSoundFX = value;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x0600021F RID: 543 RVA: 0x00007C85 File Offset: 0x00005E85
		// (set) Token: 0x06000220 RID: 544 RVA: 0x00007C8D File Offset: 0x00005E8D
		public AzureSoundFX calmWindSoundFX
		{
			get
			{
				return this.m_calmWindSoundFX;
			}
			set
			{
				this.m_calmWindSoundFX = value;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000221 RID: 545 RVA: 0x00007C96 File Offset: 0x00005E96
		// (set) Token: 0x06000222 RID: 546 RVA: 0x00007C9E File Offset: 0x00005E9E
		public AzureSoundFX mediumWindSoundFX
		{
			get
			{
				return this.m_mediumWindSoundFX;
			}
			set
			{
				this.m_mediumWindSoundFX = value;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000223 RID: 547 RVA: 0x00007CA7 File Offset: 0x00005EA7
		// (set) Token: 0x06000224 RID: 548 RVA: 0x00007CAF File Offset: 0x00005EAF
		public AzureSoundFX stormWindSoundFX
		{
			get
			{
				return this.m_stormWindSoundFX;
			}
			set
			{
				this.m_stormWindSoundFX = value;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000225 RID: 549 RVA: 0x00007CB8 File Offset: 0x00005EB8
		// (set) Token: 0x06000226 RID: 550 RVA: 0x00007CC0 File Offset: 0x00005EC0
		public Material rainMaterial
		{
			get
			{
				return this.m_rainMaterial;
			}
			set
			{
				this.m_rainMaterial = value;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000227 RID: 551 RVA: 0x00007CC9 File Offset: 0x00005EC9
		// (set) Token: 0x06000228 RID: 552 RVA: 0x00007CD1 File Offset: 0x00005ED1
		public Material stormMaterial
		{
			get
			{
				return this.m_stormMaterial;
			}
			set
			{
				this.m_stormMaterial = value;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000229 RID: 553 RVA: 0x00007CDA File Offset: 0x00005EDA
		// (set) Token: 0x0600022A RID: 554 RVA: 0x00007CE2 File Offset: 0x00005EE2
		public Material snowMaterial
		{
			get
			{
				return this.m_snowMaterial;
			}
			set
			{
				this.m_snowMaterial = value;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x0600022B RID: 555 RVA: 0x00007CEB File Offset: 0x00005EEB
		// (set) Token: 0x0600022C RID: 556 RVA: 0x00007CF3 File Offset: 0x00005EF3
		public WindZone windZone
		{
			get
			{
				return this.m_windZone;
			}
			set
			{
				this.m_windZone = value;
			}
		}

		// Token: 0x0600022D RID: 557 RVA: 0x00007CFC File Offset: 0x00005EFC
		private void OnEnable()
		{
			AzureNotificationCenter.OnAfterWeatherSystemUpdate += this.OnAfterWeatherSystemUpdate;
		}

		// Token: 0x0600022E RID: 558 RVA: 0x00007D0F File Offset: 0x00005F0F
		private void OnDisable()
		{
			AzureNotificationCenter.OnAfterWeatherSystemUpdate -= this.OnAfterWeatherSystemUpdate;
		}

		// Token: 0x0600022F RID: 559 RVA: 0x00007D24 File Offset: 0x00005F24
		private void OnAfterWeatherSystemUpdate(AzureWeatherSystem azureWeatherSystem)
		{
			this.m_azureSkyRenderer.molecularDensity = azureWeatherSystem.GetFloatOutput(0, 0);
			this.m_azureSkyRenderer.rayleigh = azureWeatherSystem.GetFloatOutput(0, 1);
			this.m_azureSkyRenderer.mie = azureWeatherSystem.GetFloatOutput(0, 2);
			this.m_azureSkyRenderer.rayleighColor = azureWeatherSystem.GetColorOutput(0, 3);
			this.m_azureSkyRenderer.mieColor = azureWeatherSystem.GetColorOutput(0, 4);
			this.m_azureSkyRenderer.starsIntensity = azureWeatherSystem.GetFloatOutput(1, 0);
			this.m_azureSkyRenderer.milkyWayIntensity = azureWeatherSystem.GetFloatOutput(1, 1);
			this.m_azureSkyRenderer.constellationIntensity = azureWeatherSystem.GetFloatOutput(1, 2);
			this.m_azureSkyRenderer.sunOpacity = azureWeatherSystem.GetFloatOutput(1, 3);
			this.m_azureSkyRenderer.moonOpacity = azureWeatherSystem.GetFloatOutput(1, 4);
			this.m_azureSkyRenderer.globalFogDistance = azureWeatherSystem.GetFloatOutput(2, 0);
			this.m_azureSkyRenderer.globalFogSmoothStep = azureWeatherSystem.GetFloatOutput(2, 1);
			this.m_azureSkyRenderer.globalFogDensity = azureWeatherSystem.GetFloatOutput(2, 2);
			this.m_azureSkyRenderer.heightFogDistance = azureWeatherSystem.GetFloatOutput(2, 3);
			this.m_azureSkyRenderer.heightFogSmoothStep = azureWeatherSystem.GetFloatOutput(2, 4);
			this.m_azureSkyRenderer.heightFogDensity = azureWeatherSystem.GetFloatOutput(2, 5);
			this.m_azureSkyRenderer.heightFogScatteringMultiplier = azureWeatherSystem.GetFloatOutput(2, 6);
			this.m_directionalLight.intensity = azureWeatherSystem.GetFloatOutput(3, 0);
			this.m_directionalLight.color = azureWeatherSystem.GetColorOutput(3, 1);
			this.m_directionalLight.shadowStrength = azureWeatherSystem.GetFloatOutput(3, 2);
			RenderSettings.ambientIntensity = azureWeatherSystem.GetFloatOutput(4, 0);
			RenderSettings.ambientSkyColor = azureWeatherSystem.GetColorOutput(4, 1);
			RenderSettings.ambientEquatorColor = azureWeatherSystem.GetColorOutput(4, 2);
			RenderSettings.ambientGroundColor = azureWeatherSystem.GetColorOutput(4, 3);
			Action<Light> environmentLightingUpdated = AzureWeatherPropertyLinker.EnvironmentLightingUpdated;
			if (environmentLightingUpdated != null)
			{
				environmentLightingUpdated(this.m_directionalLight);
			}
			this.m_rainParticleFX.intensity = (AzureWeatherPropertyLinker.SuppressPrecipitation ? 0f : azureWeatherSystem.GetFloatOutput(5, 0));
			this.m_stormParticleFX.intensity = (AzureWeatherPropertyLinker.SuppressPrecipitation ? 0f : azureWeatherSystem.GetFloatOutput(5, 1));
			this.m_snowParticleFX.intensity = (AzureWeatherPropertyLinker.SuppressPrecipitation ? 0f : azureWeatherSystem.GetFloatOutput(5, 2));
			this.m_rainMaterial.SetColor("_TintColor", azureWeatherSystem.GetColorOutput(5, 3));
			this.m_stormMaterial.SetColor("_TintColor", azureWeatherSystem.GetColorOutput(5, 4));
			this.m_snowMaterial.SetColor("_TintColor", azureWeatherSystem.GetColorOutput(5, 5));
			this.m_calmRainSoundFX.volume = AzureWeatherPropertyLinker.FilterRainVolume(azureWeatherSystem.GetFloatOutput(6, 0));
			this.m_mediumRainSoundFX.volume = AzureWeatherPropertyLinker.FilterRainVolume(azureWeatherSystem.GetFloatOutput(6, 1));
			this.m_stormRainSoundFX.volume = AzureWeatherPropertyLinker.FilterRainVolume(azureWeatherSystem.GetFloatOutput(6, 2));
			this.m_calmWindSoundFX.volume = azureWeatherSystem.GetFloatOutput(6, 3);
			this.m_mediumWindSoundFX.volume = azureWeatherSystem.GetFloatOutput(6, 4);
			this.m_stormWindSoundFX.volume = azureWeatherSystem.GetFloatOutput(6, 5);
			this.m_azureSkyRenderer.dynamicCloudDensity = azureWeatherSystem.GetFloatOutput(7, 0);
			this.m_azureSkyRenderer.dynamicCloudAltitude = azureWeatherSystem.GetFloatOutput(7, 1);
			this.m_azureSkyRenderer.dynamicCloudSpeed = azureWeatherSystem.GetFloatOutput(7, 2);
			this.m_azureSkyRenderer.dynamicCloudDirection = azureWeatherSystem.GetFloatOutput(7, 3);
			this.m_azureSkyRenderer.dynamicCloudColor1 = azureWeatherSystem.GetColorOutput(7, 4);
			this.m_azureSkyRenderer.dynamicCloudColor2 = azureWeatherSystem.GetColorOutput(7, 5);
			this.m_windZone.windMain = azureWeatherSystem.GetFloatOutput(8, 0);
			this.m_windZone.transform.eulerAngles = azureWeatherSystem.GetVector3Output(8, 1);
		}

		// Token: 0x06000230 RID: 560 RVA: 0x000080C0 File Offset: 0x000062C0
		private static float FilterRainVolume(float volume)
		{
			if (AzureWeatherPropertyLinker.RainVolumeFilter == null)
			{
				return volume;
			}
			return AzureWeatherPropertyLinker.RainVolumeFilter(volume);
		}

		// Token: 0x04000182 RID: 386
		[SerializeField]
		private AzureSkyRenderer m_azureSkyRenderer;

		// Token: 0x04000183 RID: 387
		[SerializeField]
		private Light m_directionalLight;

		// Token: 0x04000184 RID: 388
		[SerializeField]
		private AzureParticleFX m_rainParticleFX;

		// Token: 0x04000185 RID: 389
		[SerializeField]
		private AzureParticleFX m_stormParticleFX;

		// Token: 0x04000186 RID: 390
		[SerializeField]
		private AzureParticleFX m_snowParticleFX;

		// Token: 0x04000187 RID: 391
		[SerializeField]
		private AzureSoundFX m_calmRainSoundFX;

		// Token: 0x04000188 RID: 392
		[SerializeField]
		private AzureSoundFX m_mediumRainSoundFX;

		// Token: 0x04000189 RID: 393
		[SerializeField]
		private AzureSoundFX m_stormRainSoundFX;

		// Token: 0x0400018A RID: 394
		[SerializeField]
		private AzureSoundFX m_calmWindSoundFX;

		// Token: 0x0400018B RID: 395
		[SerializeField]
		private AzureSoundFX m_mediumWindSoundFX;

		// Token: 0x0400018C RID: 396
		[SerializeField]
		private AzureSoundFX m_stormWindSoundFX;

		// Token: 0x0400018D RID: 397
		[SerializeField]
		private Material m_rainMaterial;

		// Token: 0x0400018E RID: 398
		[SerializeField]
		private Material m_stormMaterial;

		// Token: 0x0400018F RID: 399
		[SerializeField]
		private Material m_snowMaterial;

		// Token: 0x04000190 RID: 400
		[SerializeField]
		private WindZone m_windZone;
	}
}
