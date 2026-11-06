using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001D5 RID: 469
	[AddComponentMenu("")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Audio/AudioSource")]
	[FeedbackHelp("This feedback lets you play a target audio source, with some elements at random.")]
	public class MMF_AudioSource : MMF_Feedback
	{
		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000BE1 RID: 3041 RVA: 0x00034076 File Offset: 0x00032276
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000BE2 RID: 3042 RVA: 0x00034079 File Offset: 0x00032279
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000BE3 RID: 3043 RVA: 0x0003407C File Offset: 0x0003227C
		protected override void AutomateTargetAcquisition()
		{
			this.TargetAudioSource = this.FindAutomatedTarget<AudioSource>();
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000BE4 RID: 3044 RVA: 0x0003408A File Offset: 0x0003228A
		// (set) Token: 0x06000BE5 RID: 3045 RVA: 0x00034092 File Offset: 0x00032292
		public override float FeedbackDuration
		{
			get
			{
				return this._duration;
			}
			set
			{
				this._duration = value;
			}
		}

		// Token: 0x06000BE6 RID: 3046 RVA: 0x0003409C File Offset: 0x0003229C
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_AudioSource.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			switch (this.Mode)
			{
			case MMF_AudioSource.Modes.Play:
			{
				if (this.RandomSfx.Length != 0)
				{
					this._randomClip = this.RandomSfx[Random.Range(0, this.RandomSfx.Length)];
					this.TargetAudioSource.clip = this._randomClip;
				}
				float num2 = Random.Range(this.MinVolume, this.MaxVolume) * num;
				float num3 = Random.Range(this.MinPitch, this.MaxPitch);
				this._duration = this.TargetAudioSource.clip.length;
				this.PlayAudioSource(this.TargetAudioSource, num2, num3);
				return;
			}
			case MMF_AudioSource.Modes.Pause:
				this._duration = 0.1f;
				this.TargetAudioSource.Pause();
				return;
			case MMF_AudioSource.Modes.UnPause:
				this._duration = 0.1f;
				this.TargetAudioSource.UnPause();
				return;
			case MMF_AudioSource.Modes.Stop:
				this._duration = 0.1f;
				this.TargetAudioSource.Stop();
				return;
			default:
				return;
			}
		}

		// Token: 0x06000BE7 RID: 3047 RVA: 0x000341A4 File Offset: 0x000323A4
		protected virtual void PlayAudioSource(AudioSource audioSource, float volume, float pitch)
		{
			audioSource.volume = volume;
			audioSource.pitch = pitch;
			audioSource.timeSamples = 0;
			if (!this.NormalPlayDirection)
			{
				audioSource.pitch = -1f;
				audioSource.timeSamples = audioSource.clip.samples - 1;
			}
			audioSource.Play();
		}

		// Token: 0x06000BE8 RID: 3048 RVA: 0x000341F2 File Offset: 0x000323F2
		public override void Stop(Vector3 position, float feedbacksIntensity = 1f)
		{
			base.Stop(position, feedbacksIntensity);
			if (this.TargetAudioSource != null)
			{
				AudioSource targetAudioSource = this.TargetAudioSource;
				if (targetAudioSource == null)
				{
					return;
				}
				targetAudioSource.Stop();
			}
		}

		// Token: 0x04000D99 RID: 3481
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000D9A RID: 3482
		[MMFInspectorGroup("Audiosource", true, 28, true, false)]
		[Tooltip("the target audio source to play")]
		public AudioSource TargetAudioSource;

		// Token: 0x04000D9B RID: 3483
		[Tooltip("whether we should play the audio source or stop it or pause it")]
		public MMF_AudioSource.Modes Mode;

		// Token: 0x04000D9C RID: 3484
		[Header("Random Sound")]
		[Tooltip("an array to pick a random sfx from")]
		public AudioClip[] RandomSfx;

		// Token: 0x04000D9D RID: 3485
		[MMFInspectorGroup("Audio Settings", true, 29, false, false)]
		[Header("Volume")]
		[Tooltip("the minimum volume to play the sound at")]
		public float MinVolume = 1f;

		// Token: 0x04000D9E RID: 3486
		[Tooltip("the maximum volume to play the sound at")]
		public float MaxVolume = 1f;

		// Token: 0x04000D9F RID: 3487
		[Header("Pitch")]
		[Tooltip("the minimum pitch to play the sound at")]
		public float MinPitch = 1f;

		// Token: 0x04000DA0 RID: 3488
		[Tooltip("the maximum pitch to play the sound at")]
		public float MaxPitch = 1f;

		// Token: 0x04000DA1 RID: 3489
		[Header("Mixer")]
		[Tooltip("the audiomixer to play the sound with (optional)")]
		public AudioMixerGroup SfxAudioMixerGroup;

		// Token: 0x04000DA2 RID: 3490
		protected AudioClip _randomClip;

		// Token: 0x04000DA3 RID: 3491
		protected float _duration;

		// Token: 0x020004AC RID: 1196
		public enum Modes
		{
			// Token: 0x040021C7 RID: 8647
			Play,
			// Token: 0x040021C8 RID: 8648
			Pause,
			// Token: 0x040021C9 RID: 8649
			UnPause,
			// Token: 0x040021CA RID: 8650
			Stop
		}
	}
}
