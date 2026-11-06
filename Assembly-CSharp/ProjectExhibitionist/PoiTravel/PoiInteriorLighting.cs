using System;
using UnityEngine;
using UnityEngine.AzureSky;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace ProjectExhibitionist.PoiTravel
{
	// Token: 0x0200095C RID: 2396
	[DisallowMultipleComponent]
	public sealed class PoiInteriorLighting : MonoBehaviour
	{
		// Token: 0x06004152 RID: 16722 RVA: 0x00183A14 File Offset: 0x00181C14
		public static float EvaluateDaylightFactor(float evaluationHour)
		{
			float num = Mathf.Repeat(evaluationHour, 24f);
			float num2 = Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(5.5f, 8f, num));
			float num3 = 1f - Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(17f, 19.5f, num));
			return num2 * num3;
		}

		// Token: 0x06004153 RID: 16723 RVA: 0x00183A74 File Offset: 0x00181C74
		private void OnEnable()
		{
			AzureNotificationCenter.OnAfterOverrideUpdate += this.OnWeatherOverridesApplied;
			SceneManager.activeSceneChanged += this.OnActiveSceneChanged;
			this.ApplyAmbient();
		}

		// Token: 0x06004154 RID: 16724 RVA: 0x00183A9E File Offset: 0x00181C9E
		private void OnDisable()
		{
			AzureNotificationCenter.OnAfterOverrideUpdate -= this.OnWeatherOverridesApplied;
			SceneManager.activeSceneChanged -= this.OnActiveSceneChanged;
			this.RestoreDirectional();
			this.RestoreExteriorSkyReflection();
		}

		// Token: 0x06004155 RID: 16725 RVA: 0x00183ACE File Offset: 0x00181CCE
		private void OnActiveSceneChanged(Scene previous, Scene next)
		{
			if (next == base.gameObject.scene)
			{
				this.ApplyAmbient();
				return;
			}
			this.RestoreDirectional();
			this.RestoreExteriorSkyReflection();
		}

		// Token: 0x06004156 RID: 16726 RVA: 0x00183AF8 File Offset: 0x00181CF8
		private void ApplyWeatherLighting(Light exteriorDirectional)
		{
			if (SceneManager.GetActiveScene() != base.gameObject.scene)
			{
				return;
			}
			if (exteriorDirectional != null)
			{
				if (this._exteriorDirectional != null && this._exteriorDirectional != exteriorDirectional)
				{
					this.RestoreDirectional();
				}
				this._exteriorDirectional = exteriorDirectional;
				this._lastExteriorIntensity = exteriorDirectional.intensity;
				exteriorDirectional.intensity = (this.receivesDaylight ? (this._lastExteriorIntensity * this.daylightSunTransmission * this._daylightFactor) : 0f);
			}
			this.ApplyAmbient();
		}

		// Token: 0x06004157 RID: 16727 RVA: 0x00183B8A File Offset: 0x00181D8A
		private void OnWeatherOverridesApplied(AzureWeatherSystem weather)
		{
			this._daylightFactor = (this.receivesDaylight ? PoiInteriorLighting.EvaluateDaylightFactor(weather.evaluationTime) : 0f);
			this.ApplyWeatherLighting(RenderSettings.sun);
		}

		// Token: 0x06004158 RID: 16728 RVA: 0x00183BB8 File Offset: 0x00181DB8
		private void ApplyAmbient()
		{
			if (SceneManager.GetActiveScene() != base.gameObject.scene)
			{
				return;
			}
			RenderSettings.ambientMode = AmbientMode.Trilight;
			RenderSettings.ambientIntensity = 1f;
			RenderSettings.ambientSkyColor = Color.Lerp(this.skyColor, this.daylightSkyColor, this._daylightFactor);
			RenderSettings.ambientEquatorColor = Color.Lerp(this.equatorColor, this.daylightEquatorColor, this._daylightFactor);
			RenderSettings.ambientGroundColor = Color.Lerp(this.groundColor, this.daylightGroundColor, this._daylightFactor);
			RenderSettings.fog = false;
			if (this.suppressExteriorSkyReflection)
			{
				this.SuppressExteriorSkyReflection((this._exteriorSkyProbe != null) ? this._exteriorSkyProbe : PoiInteriorLighting.FindExteriorSkyReflection());
				return;
			}
			this.RestoreExteriorSkyReflection();
		}

		// Token: 0x06004159 RID: 16729 RVA: 0x00183C78 File Offset: 0x00181E78
		private static ReflectionProbe FindExteriorSkyReflection()
		{
			foreach (AzureCoreSystem azureCoreSystem in Object.FindObjectsOfType<AzureCoreSystem>())
			{
				if (azureCoreSystem.isActiveAndEnabled && azureCoreSystem.gameObject.scene.name == "_Core" && azureCoreSystem.reflectionProbe != null && azureCoreSystem.reflectionProbe.cullingMask == 0)
				{
					return azureCoreSystem.reflectionProbe;
				}
			}
			return null;
		}

		// Token: 0x0600415A RID: 16730 RVA: 0x00183CE8 File Offset: 0x00181EE8
		private void SuppressExteriorSkyReflection(ReflectionProbe skyProbe)
		{
			if (!this.suppressExteriorSkyReflection || SceneManager.GetActiveScene() != base.gameObject.scene || skyProbe == null || skyProbe.cullingMask != 0)
			{
				return;
			}
			if (this._exteriorSkyProbe != skyProbe)
			{
				this.RestoreExteriorSkyReflection();
				this._exteriorSkyProbe = skyProbe;
				this._originalExteriorSkyIntensity = skyProbe.intensity;
			}
			skyProbe.intensity = 0f;
		}

		// Token: 0x0600415B RID: 16731 RVA: 0x00183D58 File Offset: 0x00181F58
		private void RestoreExteriorSkyReflection()
		{
			if (this._exteriorSkyProbe != null)
			{
				this._exteriorSkyProbe.intensity = this._originalExteriorSkyIntensity;
			}
			this._exteriorSkyProbe = null;
		}

		// Token: 0x0600415C RID: 16732 RVA: 0x00183D80 File Offset: 0x00181F80
		private void RestoreDirectional()
		{
			if (this._exteriorDirectional != null)
			{
				this._exteriorDirectional.intensity = this._lastExteriorIntensity;
			}
			this._exteriorDirectional = null;
		}

		// Token: 0x0400413C RID: 16700
		[SerializeField]
		private Color skyColor = new Color(0.28f, 0.3f, 0.33f);

		// Token: 0x0400413D RID: 16701
		[SerializeField]
		private Color equatorColor = new Color(0.19f, 0.21f, 0.23f);

		// Token: 0x0400413E RID: 16702
		[SerializeField]
		private Color groundColor = new Color(0.09f, 0.1f, 0.12f);

		// Token: 0x0400413F RID: 16703
		[SerializeField]
		private bool receivesDaylight;

		// Token: 0x04004140 RID: 16704
		[Tooltip("Enclosed interior: remove Azure's outdoor sky reflection while preserving local room probes.")]
		[SerializeField]
		private bool suppressExteriorSkyReflection;

		// Token: 0x04004141 RID: 16705
		[SerializeField]
		private Color daylightSkyColor = new Color(0.82f, 0.85f, 0.88f);

		// Token: 0x04004142 RID: 16706
		[SerializeField]
		private Color daylightEquatorColor = new Color(0.67f, 0.71f, 0.74f);

		// Token: 0x04004143 RID: 16707
		[SerializeField]
		private Color daylightGroundColor = new Color(0.42f, 0.46f, 0.49f);

		// Token: 0x04004144 RID: 16708
		[SerializeField]
		[Range(0f, 1f)]
		private float daylightSunTransmission = 1f;

		// Token: 0x04004145 RID: 16709
		private Light _exteriorDirectional;

		// Token: 0x04004146 RID: 16710
		private float _lastExteriorIntensity;

		// Token: 0x04004147 RID: 16711
		private float _daylightFactor;

		// Token: 0x04004148 RID: 16712
		private ReflectionProbe _exteriorSkyProbe;

		// Token: 0x04004149 RID: 16713
		private float _originalExteriorSkyIntensity;
	}
}
