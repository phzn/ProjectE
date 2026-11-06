using System;
using UnityEngine;

namespace Invector
{
	// Token: 0x020004AF RID: 1199
	[RequireComponent(typeof(AudioSource))]
	public class vPlayRandomClip : MonoBehaviour
	{
		// Token: 0x06001D38 RID: 7480 RVA: 0x000B3A24 File Offset: 0x000B1C24
		private void Start()
		{
			if (!this.audioSource)
			{
				this.audioSource = base.GetComponent<AudioSource>();
			}
			Random.InitState(Random.Range(0, DateTime.Now.Millisecond));
			if (this.playOnStart)
			{
				this.Play();
			}
		}

		// Token: 0x06001D39 RID: 7481 RVA: 0x000B3A70 File Offset: 0x000B1C70
		public void Play()
		{
			if (this.audioSource)
			{
				int num = Random.Range(0, this.clips.Length - 1);
				if (this.clips.Length != 0)
				{
					this.audioSource.PlayOneShot(this.clips[num]);
				}
			}
		}

		// Token: 0x04001E3C RID: 7740
		public AudioClip[] clips;

		// Token: 0x04001E3D RID: 7741
		public AudioSource audioSource;

		// Token: 0x04001E3E RID: 7742
		public bool playOnStart = true;
	}
}
