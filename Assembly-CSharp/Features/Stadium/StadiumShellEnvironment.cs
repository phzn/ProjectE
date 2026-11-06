using System;
using UnityEngine;

namespace Features.Stadium
{
	// Token: 0x02000C51 RID: 3153
	[DisallowMultipleComponent]
	public sealed class StadiumShellEnvironment : MonoBehaviour
	{
		// Token: 0x06005B16 RID: 23318 RVA: 0x0023A9FB File Offset: 0x00238BFB
		internal Object ReadPoiGameplayReference(UniversityGameplayField field, int elementIndex)
		{
			if (field == UniversityGameplayField.StadiumShellEnvironment_daySignMaterial)
			{
				return this.daySignMaterial;
			}
			if (field != UniversityGameplayField.StadiumShellEnvironment_nightSignMaterial)
			{
				throw new InvalidOperationException("Unsupported StadiumShellEnvironment POI binding: " + field.ToString());
			}
			return this.nightSignMaterial;
		}

		// Token: 0x06005B17 RID: 23319 RVA: 0x0023AA34 File Offset: 0x00238C34
		internal void WritePoiGameplayReference(UniversityGameplayField field, Object asset, int elementIndex)
		{
			if (field == UniversityGameplayField.StadiumShellEnvironment_daySignMaterial)
			{
				this.daySignMaterial = (Material)asset;
				return;
			}
			if (field != UniversityGameplayField.StadiumShellEnvironment_nightSignMaterial)
			{
				throw new InvalidOperationException("Unsupported StadiumShellEnvironment POI binding: " + field.ToString());
			}
			this.nightSignMaterial = (Material)asset;
		}

		// Token: 0x06005B18 RID: 23320 RVA: 0x0023AA83 File Offset: 0x00238C83
		private void OnEnable()
		{
			this.nextRefresh = 0f;
			this.signStateApplied = false;
			this.SetNightPresentation(StreetLightManager.Instance != null && StreetLightManager.Instance.IsNight);
			this.TryStartCrowd();
		}

		// Token: 0x06005B19 RID: 23321 RVA: 0x0023AAC0 File Offset: 0x00238CC0
		private void Update()
		{
			if (Time.unscaledTime < this.nextRefresh)
			{
				return;
			}
			this.nextRefresh = Time.unscaledTime + 0.3f;
			this.SetNightPresentation(StreetLightManager.Instance != null && StreetLightManager.Instance.IsNight);
			this.TryStartCrowd();
		}

		// Token: 0x06005B1A RID: 23322 RVA: 0x0023AB14 File Offset: 0x00238D14
		private void TryStartCrowd()
		{
			if (this.crowd == null || this.crowd.clip == null || !this.crowd.isActiveAndEnabled)
			{
				return;
			}
			if (this.crowd.clip.loadState == AudioDataLoadState.Unloaded)
			{
				this.crowd.clip.LoadAudioData();
			}
			if (this.crowd.clip.loadState != AudioDataLoadState.Loaded || AudioListener.pause || Time.timeScale <= 0f || this.crowd.isPlaying)
			{
				return;
			}
			this.crowd.time = Random.value * this.crowd.clip.length;
			this.crowd.Play();
		}

		// Token: 0x06005B1B RID: 23323 RVA: 0x0023ABD3 File Offset: 0x00238DD3
		private void OnDisable()
		{
			if (this.crowd != null)
			{
				this.crowd.Stop();
			}
			this.SetNightPresentation(false);
		}

		// Token: 0x06005B1C RID: 23324 RVA: 0x0023ABF8 File Offset: 0x00238DF8
		public void SetNightPresentation(bool night)
		{
			if (this.nightVisuals != null && this.nightVisuals.activeSelf != night)
			{
				this.nightVisuals.SetActive(night);
			}
			if (this.signStateApplied && this.signIsNight == night)
			{
				return;
			}
			if (this.signRenderers == null || this.daySignMaterial == null || this.nightSignMaterial == null)
			{
				return;
			}
			Material material = (night ? this.nightSignMaterial : this.daySignMaterial);
			foreach (Renderer renderer in this.signRenderers)
			{
				if (!(renderer == null))
				{
					Material[] sharedMaterials = renderer.sharedMaterials;
					bool flag = false;
					for (int j = 0; j < sharedMaterials.Length; j++)
					{
						if ((sharedMaterials[j] == this.daySignMaterial || sharedMaterials[j] == this.nightSignMaterial) && sharedMaterials[j] != material)
						{
							sharedMaterials[j] = material;
							flag = true;
						}
					}
					if (flag)
					{
						renderer.sharedMaterials = sharedMaterials;
					}
				}
			}
			this.signIsNight = night;
			this.signStateApplied = true;
		}

		// Token: 0x04005C41 RID: 23617
		public const int MaximumRealtimeLights = 24;

		// Token: 0x04005C42 RID: 23618
		[SerializeField]
		private AudioSource crowd;

		// Token: 0x04005C43 RID: 23619
		[SerializeField]
		private GameObject nightVisuals;

		// Token: 0x04005C44 RID: 23620
		[SerializeField]
		private Renderer[] signRenderers;

		// Token: 0x04005C45 RID: 23621
		[SerializeField]
		private Material daySignMaterial;

		// Token: 0x04005C46 RID: 23622
		[SerializeField]
		private Material nightSignMaterial;

		// Token: 0x04005C47 RID: 23623
		private float nextRefresh;

		// Token: 0x04005C48 RID: 23624
		private bool signStateApplied;

		// Token: 0x04005C49 RID: 23625
		private bool signIsNight;
	}
}
