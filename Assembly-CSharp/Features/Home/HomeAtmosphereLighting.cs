using System;
using Features.Rank;
using UnityEngine;
using UnityEngine.AzureSky;

namespace Features.Home
{
	// Token: 0x02001198 RID: 4504
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(21000)]
	public sealed class HomeAtmosphereLighting : MonoBehaviour
	{
		// Token: 0x060089EA RID: 35306 RVA: 0x00385E22 File Offset: 0x00384022
		private void OnEnable()
		{
			this._appliedNight = null;
			this._nextRefresh = 0f;
			this.RefreshClock();
		}

		// Token: 0x060089EB RID: 35307 RVA: 0x00385E41 File Offset: 0x00384041
		private void LateUpdate()
		{
			if (Time.unscaledTime < this._nextRefresh)
			{
				return;
			}
			this._nextRefresh = Time.unscaledTime + 0.2f;
			this.RefreshClock();
		}

		// Token: 0x060089EC RID: 35308 RVA: 0x00385E68 File Offset: 0x00384068
		private void RefreshClock()
		{
			if (this._azure == null)
			{
				this._azure = Object.FindObjectOfType<AzureCoreSystem>(true);
			}
			if (this._azure != null && this._azure.timeSystem != null)
			{
				this.ApplyHour(this._azure.timeSystem.timeOfDay);
			}
		}

		// Token: 0x060089ED RID: 35309 RVA: 0x00385EC0 File Offset: 0x003840C0
		public void ApplyHour(float hour)
		{
			bool flag = NightVisibilityController.IsNight(hour, this.nightStartHour, this.nightEndHour);
			this.ApplyState(flag);
		}

		// Token: 0x060089EE RID: 35310 RVA: 0x00385EE8 File Offset: 0x003840E8
		private void ApplyState(bool night)
		{
			bool? appliedNight = this._appliedNight;
			if ((appliedNight.GetValueOrDefault() == night) & (appliedNight != null))
			{
				return;
			}
			this._appliedNight = new bool?(night);
			foreach (Light light in this.fixtures)
			{
				if (light != null)
				{
					light.enabled = night;
				}
			}
			foreach (Light light2 in this.interiorFixtures)
			{
				if (light2 != null)
				{
					light2.enabled = true;
				}
			}
			if (this._properties == null)
			{
				this._properties = new MaterialPropertyBlock();
			}
			foreach (HomeAtmosphereLighting.EmissionBinding emissionBinding in this.emissions)
			{
				if (emissionBinding != null && !(emissionBinding.renderer == null))
				{
					emissionBinding.renderer.GetPropertyBlock(this._properties, emissionBinding.materialIndex);
					if (this._properties.isEmpty)
					{
						emissionBinding.renderer.GetPropertyBlock(this._properties);
					}
					this._properties.SetColor(HomeAtmosphereLighting.EmissionColor, (night || emissionBinding.alwaysOn) ? emissionBinding.nightEmission : Color.black);
					emissionBinding.renderer.SetPropertyBlock(this._properties, emissionBinding.materialIndex);
					this._properties.Clear();
				}
			}
		}

		// Token: 0x060089EF RID: 35311 RVA: 0x0038604C File Offset: 0x0038424C
		private void OnDisable()
		{
			this.ApplyState(false);
		}

		// Token: 0x04008C9A RID: 35994
		[SerializeField]
		private Light[] fixtures = Array.Empty<Light>();

		// Token: 0x04008C9B RID: 35995
		[Tooltip("Windowless rooms remain lit during daylight as well.")]
		[SerializeField]
		private Light[] interiorFixtures = Array.Empty<Light>();

		// Token: 0x04008C9C RID: 35996
		[SerializeField]
		private HomeAtmosphereLighting.EmissionBinding[] emissions = Array.Empty<HomeAtmosphereLighting.EmissionBinding>();

		// Token: 0x04008C9D RID: 35997
		[SerializeField]
		private float nightStartHour = 17f;

		// Token: 0x04008C9E RID: 35998
		[SerializeField]
		private float nightEndHour = 6f;

		// Token: 0x04008C9F RID: 35999
		private AzureCoreSystem _azure;

		// Token: 0x04008CA0 RID: 36000
		private MaterialPropertyBlock _properties;

		// Token: 0x04008CA1 RID: 36001
		private bool? _appliedNight;

		// Token: 0x04008CA2 RID: 36002
		private float _nextRefresh;

		// Token: 0x04008CA3 RID: 36003
		private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

		// Token: 0x02001199 RID: 4505
		[Serializable]
		public sealed class EmissionBinding
		{
			// Token: 0x04008CA4 RID: 36004
			public Renderer renderer;

			// Token: 0x04008CA5 RID: 36005
			public int materialIndex;

			// Token: 0x04008CA6 RID: 36006
			[ColorUsage(false, true)]
			public Color nightEmission = Color.white;

			// Token: 0x04008CA7 RID: 36007
			public bool alwaysOn;
		}
	}
}
