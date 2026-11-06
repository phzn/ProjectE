using System;
using UnityEngine;
using UnityEngine.AzureSky;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace Features.Stadium
{
	// Token: 0x02000C42 RID: 3138
	[DisallowMultipleComponent]
	public sealed class StadiumInteriorLighting : MonoBehaviour
	{
		// Token: 0x06005A81 RID: 23169 RVA: 0x00236CEA File Offset: 0x00234EEA
		private void OnEnable()
		{
			AzureNotificationCenter.OnAfterOverrideUpdate += this.OnWeatherOverridesApplied;
			SceneManager.activeSceneChanged += this.OnActiveSceneChanged;
			this.ApplyNightFill(StadiumInteriorLighting.IsNight, false);
		}

		// Token: 0x06005A82 RID: 23170 RVA: 0x00236D1C File Offset: 0x00234F1C
		private void OnDisable()
		{
			AzureNotificationCenter.OnAfterOverrideUpdate -= this.OnWeatherOverridesApplied;
			SceneManager.activeSceneChanged -= this.OnActiveSceneChanged;
			if (SceneManager.GetActiveScene() == base.gameObject.scene)
			{
				this.RestoreWeather();
			}
		}

		// Token: 0x17000EE2 RID: 3810
		// (get) Token: 0x06005A83 RID: 23171 RVA: 0x00236D68 File Offset: 0x00234F68
		private static bool IsNight
		{
			get
			{
				return StreetLightManager.Instance != null && StreetLightManager.Instance.IsNight;
			}
		}

		// Token: 0x06005A84 RID: 23172 RVA: 0x00236D83 File Offset: 0x00234F83
		private void OnWeatherOverridesApplied(AzureWeatherSystem weather)
		{
			this.ApplyNightFill(StadiumInteriorLighting.IsNight, true);
		}

		// Token: 0x06005A85 RID: 23173 RVA: 0x00236D91 File Offset: 0x00234F91
		private void OnActiveSceneChanged(Scene previous, Scene next)
		{
			if (next == base.gameObject.scene)
			{
				this.ApplyNightFill(StadiumInteriorLighting.IsNight, false);
			}
		}

		// Token: 0x06005A86 RID: 23174 RVA: 0x00236DB4 File Offset: 0x00234FB4
		private void ApplyNightFill(bool night, bool freshWeather)
		{
			if (SceneManager.GetActiveScene() != base.gameObject.scene)
			{
				return;
			}
			if (!this.capturedMode)
			{
				this.weatherMode = RenderSettings.ambientMode;
				this.capturedMode = true;
			}
			if (freshWeather || !this.ownsFill)
			{
				this.weatherSky = RenderSettings.ambientSkyColor;
				this.weatherEquator = RenderSettings.ambientEquatorColor;
				this.weatherGround = RenderSettings.ambientGroundColor;
				this.weatherIntensity = RenderSettings.ambientIntensity;
			}
			if (!night)
			{
				this.RestoreWeather();
				return;
			}
			RenderSettings.ambientMode = AmbientMode.Trilight;
			RenderSettings.ambientIntensity = 1f;
			RenderSettings.ambientSkyColor = this.skyFill;
			RenderSettings.ambientEquatorColor = this.equatorFill;
			RenderSettings.ambientGroundColor = this.groundFill;
			this.ownsFill = true;
		}

		// Token: 0x06005A87 RID: 23175 RVA: 0x00236E6C File Offset: 0x0023506C
		private void RestoreWeather()
		{
			if (!this.ownsFill)
			{
				return;
			}
			RenderSettings.ambientMode = this.weatherMode;
			RenderSettings.ambientIntensity = this.weatherIntensity;
			RenderSettings.ambientSkyColor = this.weatherSky;
			RenderSettings.ambientEquatorColor = this.weatherEquator;
			RenderSettings.ambientGroundColor = this.weatherGround;
			this.ownsFill = false;
		}

		// Token: 0x04005B71 RID: 23409
		[SerializeField]
		private Color skyFill = new Color(0.46f, 0.49f, 0.54f);

		// Token: 0x04005B72 RID: 23410
		[SerializeField]
		private Color equatorFill = new Color(0.34f, 0.37f, 0.41f);

		// Token: 0x04005B73 RID: 23411
		[SerializeField]
		private Color groundFill = new Color(0.2f, 0.22f, 0.25f);

		// Token: 0x04005B74 RID: 23412
		private bool capturedMode;

		// Token: 0x04005B75 RID: 23413
		private bool ownsFill;

		// Token: 0x04005B76 RID: 23414
		private AmbientMode weatherMode;

		// Token: 0x04005B77 RID: 23415
		private Color weatherSky;

		// Token: 0x04005B78 RID: 23416
		private Color weatherEquator;

		// Token: 0x04005B79 RID: 23417
		private Color weatherGround;

		// Token: 0x04005B7A RID: 23418
		private float weatherIntensity;
	}
}
