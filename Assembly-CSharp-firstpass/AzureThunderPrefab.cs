using System;

namespace UnityEngine.AzureSky
{
	// Token: 0x0200001E RID: 30
	public sealed class AzureThunderPrefab : MonoBehaviour
	{
		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600013E RID: 318 RVA: 0x00004C9A File Offset: 0x00002E9A
		// (set) Token: 0x0600013F RID: 319 RVA: 0x00004CA2 File Offset: 0x00002EA2
		public AnimationCurve lightFrequency
		{
			get
			{
				return this.m_lightFrequency;
			}
			set
			{
				this.m_lightFrequency = value;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000140 RID: 320 RVA: 0x00004CAB File Offset: 0x00002EAB
		// (set) Token: 0x06000141 RID: 321 RVA: 0x00004CB3 File Offset: 0x00002EB3
		public float audioDelay
		{
			get
			{
				return this.m_audioDelay;
			}
			set
			{
				this.m_audioDelay = value;
			}
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00004CBC File Offset: 0x00002EBC
		private void Start()
		{
			this.m_audioSource = base.GetComponent<AudioSource>();
			AzureSoundFX.NotifySourceVolumeApplied(this.m_audioSource, this.m_audioSource.volume);
			this.m_light = base.GetComponent<Light>();
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00004CEC File Offset: 0x00002EEC
		private void Update()
		{
			this.m_time += Time.deltaTime;
			this.m_light.intensity = this.m_lightFrequency.Evaluate(this.m_time / this.m_audioSource.clip.length);
			Shader.SetGlobalFloat(AzureShaderUniforms.ThunderLightningEffect, this.m_light.intensity);
			if (this.m_time >= this.m_audioDelay && this.m_canPlayAudioClip)
			{
				this.m_audioSource.Play();
				this.m_canPlayAudioClip = false;
			}
			if (this.m_time >= this.m_audioDelay + this.m_audioSource.clip.length)
			{
				Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x04000115 RID: 277
		[SerializeField]
		private AnimationCurve m_lightFrequency;

		// Token: 0x04000116 RID: 278
		[SerializeField]
		private float m_audioDelay;

		// Token: 0x04000117 RID: 279
		private AudioSource m_audioSource;

		// Token: 0x04000118 RID: 280
		private Light m_light;

		// Token: 0x04000119 RID: 281
		private float m_time;

		// Token: 0x0400011A RID: 282
		private bool m_canPlayAudioClip = true;
	}
}
