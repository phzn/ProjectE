using System;
using UnityEngine;

namespace Features.PrisonIntro
{
	// Token: 0x02000DCC RID: 3532
	public class AnimationEventAudioRelay : MonoBehaviour
	{
		// Token: 0x0600685D RID: 26717 RVA: 0x0029BD27 File Offset: 0x00299F27
		private void Awake()
		{
			if (this.audioSource == null)
			{
				this.audioSource = base.GetComponentInChildren<AudioSource>();
			}
		}

		// Token: 0x17001129 RID: 4393
		// (get) Token: 0x0600685E RID: 26718 RVA: 0x0029BD43 File Offset: 0x00299F43
		public AudioClip DefaultClip
		{
			get
			{
				return this.defaultClip;
			}
		}

		// Token: 0x1700112A RID: 4394
		// (get) Token: 0x0600685F RID: 26719 RVA: 0x0029BD4B File Offset: 0x00299F4B
		public AudioSource Output
		{
			get
			{
				return this.audioSource;
			}
		}

		// Token: 0x1700112B RID: 4395
		// (get) Token: 0x06006860 RID: 26720 RVA: 0x0029BD53 File Offset: 0x00299F53
		public float Volume
		{
			get
			{
				return this.volume;
			}
		}

		// Token: 0x06006861 RID: 26721 RVA: 0x0029BD5B File Offset: 0x00299F5B
		public void Configure(AudioClip clip, AudioSource output, float oneShotVolume = 1f)
		{
			this.defaultClip = clip;
			this.audioSource = output;
			this.volume = Mathf.Clamp01(oneShotVolume);
		}

		// Token: 0x06006862 RID: 26722 RVA: 0x0029BD77 File Offset: 0x00299F77
		public void PlayDefaultClip()
		{
			this.PlayClip(this.defaultClip);
		}

		// Token: 0x06006863 RID: 26723 RVA: 0x0029BD88 File Offset: 0x00299F88
		public void PlayClipFromEvent(AnimationEvent evt)
		{
			AudioClip audioClip = ((evt != null) ? (evt.objectReferenceParameter as AudioClip) : null);
			this.PlayClip((audioClip != null) ? audioClip : this.defaultClip);
		}

		// Token: 0x06006864 RID: 26724 RVA: 0x0029BDC0 File Offset: 0x00299FC0
		private void PlayClip(AudioClip clip)
		{
			if (clip == null)
			{
				if (this.debugLogs)
				{
					Debug.LogWarning("[AudioRelay] Nessun AudioClip assegnato: niente da riprodurre.");
				}
				return;
			}
			if (this.audioSource == null)
			{
				this.audioSource = base.GetComponentInChildren<AudioSource>();
			}
			if (this.audioSource == null)
			{
				if (this.debugLogs)
				{
					Debug.LogWarning("[AudioRelay] Nessun AudioSource disponibile.");
				}
				return;
			}
			this.audioSource.PlayOneShot(clip, this.volume);
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[AudioRelay] PlayOneShot '{0}' (vol {1}) su '{2}'.", clip.name, this.volume, this.audioSource.gameObject.name));
			}
		}

		// Token: 0x04006AD2 RID: 27346
		[Header("Clip")]
		[Tooltip("Clip di default riprodotta da PlayDefaultClip (AnimationEvent senza parametri).")]
		[SerializeField]
		private AudioClip defaultClip;

		// Token: 0x04006AD3 RID: 27347
		[Header("Output")]
		[Tooltip("AudioSource su cui riprodurre il one-shot. Se null cerca un AudioSource su questo GameObject o nei figli.")]
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x04006AD4 RID: 27348
		[Tooltip("Volume del one-shot (moltiplicato per il volume dell'AudioSource).")]
		[Range(0f, 1f)]
		[SerializeField]
		private float volume = 1f;

		// Token: 0x04006AD5 RID: 27349
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;
	}
}
