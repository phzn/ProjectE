using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Invector
{
	// Token: 0x02000456 RID: 1110
	[RequireComponent(typeof(AudioSource))]
	public class vAudioSurfaceControl : MonoBehaviour
	{
		// Token: 0x06001BAC RID: 7084 RVA: 0x000AE510 File Offset: 0x000AC710
		public void PlayOneShot(AudioClip clip, float volume)
		{
			if (!this.source)
			{
				this.source = base.GetComponent<AudioSource>();
			}
			this.source.volume = volume;
			this.source.PlayOneShot(clip, volume);
			this.isWorking = true;
		}

		// Token: 0x06001BAD RID: 7085 RVA: 0x000AE54B File Offset: 0x000AC74B
		private void Update()
		{
			if (this.isWorking && !this.source.isPlaying)
			{
				Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x1700051D RID: 1309
		// (set) Token: 0x06001BAE RID: 7086 RVA: 0x000AE56D File Offset: 0x000AC76D
		public AudioMixerGroup outputAudioMixerGroup
		{
			set
			{
				if (!this.source)
				{
					this.source = base.GetComponent<AudioSource>();
				}
				this.source.outputAudioMixerGroup = value;
			}
		}

		// Token: 0x04001CF2 RID: 7410
		private AudioSource source;

		// Token: 0x04001CF3 RID: 7411
		private bool isWorking;
	}
}
