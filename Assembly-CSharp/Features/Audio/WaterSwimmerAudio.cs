using System;
using System.Collections;
using UnityEngine;

namespace Features.Audio
{
	// Token: 0x0200152C RID: 5420
	[DisallowMultipleComponent]
	public class WaterSwimmerAudio : MonoBehaviour
	{
		// Token: 0x0600A7D8 RID: 42968 RVA: 0x00461D01 File Offset: 0x0045FF01
		private void Awake()
		{
			if (this.autoResolveClips)
			{
				this.ResolveClips();
			}
			this._source = this.CreateSource();
		}

		// Token: 0x0600A7D9 RID: 42969 RVA: 0x00461D20 File Offset: 0x0045FF20
		private AudioSource CreateSource()
		{
			GameObject gameObject = new GameObject("WaterAudioSource");
			gameObject.transform.SetParent(base.transform, false);
			gameObject.transform.localPosition = Vector3.zero;
			AudioSource audioSource = gameObject.AddComponent<AudioSource>();
			audioSource.playOnAwake = false;
			audioSource.loop = true;
			audioSource.spatialBlend = this.spatialBlend;
			audioSource.rolloffMode = AudioRolloffMode.Linear;
			audioSource.minDistance = this.minDistance;
			audioSource.maxDistance = this.maxDistance;
			audioSource.volume = 0f;
			audioSource.dopplerLevel = 0f;
			return audioSource;
		}

		// Token: 0x0600A7DA RID: 42970 RVA: 0x00461DAD File Offset: 0x0045FFAD
		public void PlaySplash()
		{
			if (this.splashClip == null)
			{
				return;
			}
			this._source.PlayOneShot(this.splashClip, this.splashVolume);
		}

		// Token: 0x0600A7DB RID: 42971 RVA: 0x00461DD5 File Offset: 0x0045FFD5
		public void PlaySplashAt(Vector3 worldPos)
		{
			if (this.splashClip == null)
			{
				return;
			}
			this._source.transform.position = worldPos;
			this._source.PlayOneShot(this.splashClip, this.splashVolume);
		}

		// Token: 0x0600A7DC RID: 42972 RVA: 0x00461E10 File Offset: 0x00460010
		public void EnterWater()
		{
			if (!this.enableUnderwaterLoop)
			{
				return;
			}
			if (this._inWater)
			{
				return;
			}
			this._inWater = true;
			if (this.underwaterClip == null)
			{
				return;
			}
			this._source.clip = this.underwaterClip;
			this._source.loop = true;
			if (!this._source.isPlaying)
			{
				this._source.Play();
			}
			base.StopAllCoroutines();
			base.StartCoroutine(this.FadeVolume(this.underwaterVolume));
		}

		// Token: 0x0600A7DD RID: 42973 RVA: 0x00461E94 File Offset: 0x00460094
		public void ExitWater()
		{
			if (!this._inWater)
			{
				return;
			}
			this._inWater = false;
			if (this.exitClip != null)
			{
				this._source.PlayOneShot(this.exitClip, this.exitVolume);
			}
			base.StopAllCoroutines();
			base.StartCoroutine(this.FadeOutAndStop());
		}

		// Token: 0x0600A7DE RID: 42974 RVA: 0x00461EE9 File Offset: 0x004600E9
		private IEnumerator FadeVolume(float target)
		{
			float start = this._source.volume;
			float t = 0f;
			if (this.loopFade <= 0f)
			{
				this._source.volume = target;
				yield break;
			}
			while (t < this.loopFade)
			{
				t += Time.deltaTime;
				this._source.volume = Mathf.Lerp(start, target, t / this.loopFade);
				yield return null;
			}
			this._source.volume = target;
			yield break;
		}

		// Token: 0x0600A7DF RID: 42975 RVA: 0x00461EFF File Offset: 0x004600FF
		private IEnumerator FadeOutAndStop()
		{
			float start = this._source.volume;
			float t = 0f;
			if (this.loopFade > 0f)
			{
				while (t < this.loopFade)
				{
					t += Time.deltaTime;
					this._source.volume = Mathf.Lerp(start, 0f, t / this.loopFade);
					yield return null;
				}
			}
			this._source.volume = 0f;
			this._source.loop = false;
			this._source.clip = null;
			yield break;
		}

		// Token: 0x0600A7E0 RID: 42976 RVA: 0x00004F31 File Offset: 0x00003131
		private void ResolveClips()
		{
		}

		// Token: 0x0600A7E1 RID: 42977 RVA: 0x00461F0E File Offset: 0x0046010E
		public void SetUnderwaterLoopEnabled(bool enabled)
		{
			this.enableUnderwaterLoop = enabled;
			if (!enabled && this._inWater)
			{
				this.ExitWater();
			}
		}

		// Token: 0x0600A7E2 RID: 42978 RVA: 0x00461F28 File Offset: 0x00460128
		public void SetSplashVolume(float v)
		{
			this.splashVolume = Mathf.Clamp01(v);
		}

		// Token: 0x0600A7E3 RID: 42979 RVA: 0x00461F36 File Offset: 0x00460136
		public void AssignClips(AudioClip splash, AudioClip underwater, AudioClip exit)
		{
			if (splash != null)
			{
				this.splashClip = splash;
			}
			if (underwater != null)
			{
				this.underwaterClip = underwater;
			}
			if (exit != null)
			{
				this.exitClip = exit;
			}
		}

		// Token: 0x0400AC82 RID: 44162
		[Header("Clips")]
		[Tooltip("One-shot played when the body hits the water (the dive splash). Invector waterSplash.")]
		[SerializeField]
		private AudioClip splashClip;

		// Token: 0x0400AC83 RID: 44163
		[Tooltip("Looping bed while in the water. Invector underwater.")]
		[SerializeField]
		private AudioClip underwaterClip;

		// Token: 0x0400AC84 RID: 44164
		[Tooltip("One-shot played when leaving the water. Invector exitWater.")]
		[SerializeField]
		private AudioClip exitClip;

		// Token: 0x0400AC85 RID: 44165
		[Header("Volume")]
		[SerializeField]
		[Range(0f, 1f)]
		private float splashVolume = 0.5f;

		// Token: 0x0400AC86 RID: 44166
		[SerializeField]
		[Range(0f, 1f)]
		private float underwaterVolume = 0.7f;

		// Token: 0x0400AC87 RID: 44167
		[SerializeField]
		[Range(0f, 1f)]
		private float exitVolume = 0.9f;

		// Token: 0x0400AC88 RID: 44168
		[Header("Layers")]
		[Tooltip("When OFF, EnterWater() never starts the looping underwater bed (splash/exit one-shots still play). Used for pool NPCs, which should not carry an underwater loop.")]
		[SerializeField]
		private bool enableUnderwaterLoop = true;

		// Token: 0x0400AC89 RID: 44169
		[Header("Spatial 3D")]
		[Tooltip("0 = 2D, 1 = fully spatial 3D. Keep at 1 for positional pool audio.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float spatialBlend = 1f;

		// Token: 0x0400AC8A RID: 44170
		[Tooltip("Distance (m) at which the sound is at full volume.")]
		[SerializeField]
		private float minDistance = 2f;

		// Token: 0x0400AC8B RID: 44171
		[Tooltip("Distance (m) beyond which the sound is fully attenuated.")]
		[SerializeField]
		private float maxDistance = 25f;

		// Token: 0x0400AC8C RID: 44172
		[Tooltip("Seconds to fade the underwater loop in/out.")]
		[SerializeField]
		private float loopFade = 0.35f;

		// Token: 0x0400AC8D RID: 44173
		[Header("Auto-resolve clips")]
		[Tooltip("If clips are unassigned, load them from the Invector Swimming add-on by name.")]
		[SerializeField]
		private bool autoResolveClips = true;

		// Token: 0x0400AC8E RID: 44174
		private AudioSource _source;

		// Token: 0x0400AC8F RID: 44175
		private bool _inWater;
	}
}
