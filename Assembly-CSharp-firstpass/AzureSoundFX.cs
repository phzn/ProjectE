using System;

namespace UnityEngine.AzureSky
{
	// Token: 0x0200001D RID: 29
	[RequireComponent(typeof(AudioSource))]
	[AddComponentMenu("Azure[Sky] Dynamic Skybox/Azure Sound FX")]
	public sealed class AzureSoundFX : MonoBehaviour
	{
		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06000136 RID: 310 RVA: 0x00004B68 File Offset: 0x00002D68
		// (remove) Token: 0x06000137 RID: 311 RVA: 0x00004B9C File Offset: 0x00002D9C
		public static event Action<AudioSource, float> SourceVolumeApplied;

		// Token: 0x06000138 RID: 312 RVA: 0x00004BCF File Offset: 0x00002DCF
		public static void NotifySourceVolumeApplied(AudioSource source, float authoredVolume)
		{
			if (source != null)
			{
				Action<AudioSource, float> sourceVolumeApplied = AzureSoundFX.SourceVolumeApplied;
				if (sourceVolumeApplied == null)
				{
					return;
				}
				sourceVolumeApplied(source, authoredVolume);
			}
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00004BEB File Offset: 0x00002DEB
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetAudioObservers()
		{
			AzureSoundFX.SourceVolumeApplied = null;
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600013A RID: 314 RVA: 0x00004BF3 File Offset: 0x00002DF3
		// (set) Token: 0x0600013B RID: 315 RVA: 0x00004BFC File Offset: 0x00002DFC
		public float volume
		{
			get
			{
				return this.m_volume;
			}
			set
			{
				this.m_volume = value;
				if (this.m_audioSource)
				{
					this.m_audioSource.volume = value;
					AzureSoundFX.NotifySourceVolumeApplied(this.m_audioSource, value);
					if (value > 0f)
					{
						if (!this.m_audioSource.isPlaying)
						{
							this.m_audioSource.Play();
							return;
						}
					}
					else if (this.m_audioSource.isPlaying)
					{
						this.m_audioSource.Stop();
					}
				}
			}
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00004C6E File Offset: 0x00002E6E
		private void Awake()
		{
			this.m_audioSource = base.GetComponent<AudioSource>();
			AzureSoundFX.NotifySourceVolumeApplied(this.m_audioSource, this.m_audioSource.volume);
		}

		// Token: 0x04000113 RID: 275
		private AudioSource m_audioSource;

		// Token: 0x04000114 RID: 276
		[SerializeField]
		private float m_volume;
	}
}
