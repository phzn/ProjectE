using System;
using Features.RunModes;
using UnityEngine;
using UnityEngine.AzureSky;
using UnityEngine.SceneManagement;

namespace Features.Rank
{
	// Token: 0x02000D9D RID: 3485
	[DefaultExecutionOrder(20000)]
	public sealed class NightVisibilityController : MonoBehaviour
	{
		// Token: 0x060066A0 RID: 26272 RVA: 0x002902D0 File Offset: 0x0028E4D0
		public void Configure(AzureCoreSystem azure, float nightStartHour, float nightEndHour, Color nightAmbientFillColor, float minimumAmbientIntensity, float minimumDirectionalIntensity)
		{
			this._azure = azure;
			this._nightStartHour = Mathf.Clamp(nightStartHour, 0f, 24f);
			this._nightEndHour = Mathf.Clamp(nightEndHour, 0f, 24f);
			this._nightAmbientFillColor = nightAmbientFillColor;
			this._minimumAmbientIntensity = Mathf.Max(0f, minimumAmbientIntensity);
			this._minimumDirectionalIntensity = Mathf.Max(0f, minimumDirectionalIntensity);
			base.enabled = true;
			this.ApplyReadabilityFloor();
		}

		// Token: 0x060066A1 RID: 26273 RVA: 0x00290349 File Offset: 0x0028E549
		private void LateUpdate()
		{
			this.ApplyReadabilityFloor();
		}

		// Token: 0x060066A2 RID: 26274 RVA: 0x00290354 File Offset: 0x0028E554
		private void ApplyReadabilityFloor()
		{
			if (this._azure == null || this._azure.timeSystem == null)
			{
				return;
			}
			if (!NightVisibilityController.ShouldApplyLegacyFloor(SceneManager.GetActiveScene().name))
			{
				return;
			}
			float num = NightVisibilityController.EvaluateNightBlend(this._azure.timeSystem.timeOfDay, this._nightStartHour, this._nightEndHour, 1f);
			if (num <= 0f)
			{
				return;
			}
			float num2 = this._minimumAmbientIntensity * num;
			if (RenderSettings.ambientIntensity < num2)
			{
				RenderSettings.ambientIntensity = num2;
			}
			Transform directionalLight = this._azure.timeSystem.directionalLight;
			if (directionalLight == null)
			{
				return;
			}
			if (this._directionalLight == null || this._directionalLightTransform != directionalLight)
			{
				this._directionalLightTransform = directionalLight;
				this._directionalLight = directionalLight.GetComponent<Light>();
			}
			float num3 = this._minimumDirectionalIntensity * num;
			if (this._directionalLight != null && this._directionalLight.intensity < num3)
			{
				this._directionalLight.intensity = num3;
			}
		}

		// Token: 0x060066A3 RID: 26275 RVA: 0x00290455 File Offset: 0x0028E655
		public static bool ShouldApplyLegacyFloor(string activeSceneName)
		{
			return !DynamicWorldEnvironmentController.IsSupportedWorldScene(activeSceneName);
		}

		// Token: 0x060066A4 RID: 26276 RVA: 0x00290460 File Offset: 0x0028E660
		public static float EvaluateNightBlend(float hour, float nightStartHour, float nightEndHour, float transitionHours = 1f)
		{
			hour = Mathf.Repeat(hour, 24f);
			nightStartHour = Mathf.Repeat(nightStartHour, 24f);
			nightEndHour = Mathf.Repeat(nightEndHour, 24f);
			float num = Mathf.Repeat(nightEndHour - nightStartHour, 24f);
			if (num <= Mathf.Epsilon)
			{
				return 0f;
			}
			float num2 = Mathf.Repeat(hour - nightStartHour, 24f);
			if (num2 > num)
			{
				return 0f;
			}
			float num3 = Mathf.Min(Mathf.Max(transitionHours, 0.001f), num * 0.5f);
			float num4 = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(num2 / num3));
			float num5 = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01((num - num2) / num3));
			return Mathf.Min(num4, num5);
		}

		// Token: 0x060066A5 RID: 26277 RVA: 0x00290518 File Offset: 0x0028E718
		public static bool IsNight(float hour, float nightStartHour, float nightEndHour)
		{
			hour = Mathf.Repeat(hour, 24f);
			nightStartHour = Mathf.Repeat(nightStartHour, 24f);
			nightEndHour = Mathf.Repeat(nightEndHour, 24f);
			if (nightStartHour <= nightEndHour)
			{
				return hour >= nightStartHour && hour <= nightEndHour;
			}
			return hour >= nightStartHour || hour <= nightEndHour;
		}

		// Token: 0x04006916 RID: 26902
		private const float NightTransitionHours = 1f;

		// Token: 0x04006917 RID: 26903
		private AzureCoreSystem _azure;

		// Token: 0x04006918 RID: 26904
		private Light _directionalLight;

		// Token: 0x04006919 RID: 26905
		private Transform _directionalLightTransform;

		// Token: 0x0400691A RID: 26906
		private float _nightStartHour = 18.5f;

		// Token: 0x0400691B RID: 26907
		private float _nightEndHour = 6f;

		// Token: 0x0400691C RID: 26908
		private Color _nightAmbientFillColor = new Color(0.06f, 0.08f, 0.13f, 1f);

		// Token: 0x0400691D RID: 26909
		private float _minimumAmbientIntensity = 0.55f;

		// Token: 0x0400691E RID: 26910
		private float _minimumDirectionalIntensity = 0.25f;
	}
}
