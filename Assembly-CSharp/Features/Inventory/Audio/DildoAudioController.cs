using System;
using UnityEngine;

namespace Features.Inventory.Audio
{
	// Token: 0x02001161 RID: 4449
	public class DildoAudioController : MonoBehaviour
	{
		// Token: 0x1700156B RID: 5483
		// (get) Token: 0x0600877C RID: 34684 RVA: 0x003771F9 File Offset: 0x003753F9
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
		}

		// Token: 0x1700156C RID: 5484
		// (get) Token: 0x0600877D RID: 34685 RVA: 0x00377201 File Offset: 0x00375401
		// (set) Token: 0x0600877E RID: 34686 RVA: 0x00377209 File Offset: 0x00375409
		public float MasterVolume
		{
			get
			{
				return this.masterVolume;
			}
			set
			{
				this.masterVolume = Mathf.Clamp01(value);
			}
		}

		// Token: 0x0600877F RID: 34687 RVA: 0x00377217 File Offset: 0x00375417
		private void Awake()
		{
			this.SetupAudioSource();
		}

		// Token: 0x06008780 RID: 34688 RVA: 0x0037721F File Offset: 0x0037541F
		public void Activate()
		{
			if (this._audioSource == null)
			{
				this.SetupAudioSource();
			}
			this._isActive = true;
			if (this.debugLogs)
			{
				Debug.Log("[DildoAudioController] Activated", this);
			}
		}

		// Token: 0x06008781 RID: 34689 RVA: 0x0037724F File Offset: 0x0037544F
		public void ConfigureInteractionClips(AudioClip[] clips, float volume)
		{
			if (clips != null && clips.Length != 0)
			{
				this.interactionClips = clips;
				if (this.debugLogs)
				{
					Debug.Log(string.Format("[DildoAudioController] Configured {0} interaction clip(s)", clips.Length), this);
				}
			}
			this.interactionVolume = Mathf.Clamp01(volume);
		}

		// Token: 0x06008782 RID: 34690 RVA: 0x0037728B File Offset: 0x0037548B
		public void ConfigurePleasureClips(AudioClip[] clips, float volume)
		{
			this.pleasureVolume = Mathf.Clamp01(volume);
		}

		// Token: 0x06008783 RID: 34691 RVA: 0x00377299 File Offset: 0x00375499
		public void Deactivate()
		{
			this._isActive = false;
			if (this._audioSource != null)
			{
				this._audioSource.Stop();
			}
			if (this.debugLogs)
			{
				Debug.Log("[DildoAudioController] Deactivated", this);
			}
		}

		// Token: 0x06008784 RID: 34692 RVA: 0x003772D0 File Offset: 0x003754D0
		public void PlayInteractionFromAnimationEvent(AnimationEvent evt)
		{
			if (!this._isActive)
			{
				return;
			}
			if (this.interactionClips == null || this.interactionClips.Length == 0)
			{
				return;
			}
			if (evt != null && evt.animatorClipInfo.weight < this.minAnimationWeight)
			{
				if (this.debugLogs)
				{
					Debug.Log("[DildoAudioController] Skipped interaction sound - animation weight too low", this);
				}
				return;
			}
			this.PlayInteractionSound(-1f);
		}

		// Token: 0x06008785 RID: 34693 RVA: 0x00377330 File Offset: 0x00375530
		public void PlayInteractionWithIntensityFromAnimationEvent(AnimationEvent evt)
		{
			if (!this._isActive)
			{
				return;
			}
			if (this.interactionClips == null || this.interactionClips.Length == 0)
			{
				return;
			}
			if (evt != null && evt.animatorClipInfo.weight < this.minAnimationWeight)
			{
				return;
			}
			float num = ((evt != null && evt.floatParameter > 0f) ? evt.floatParameter : 1f);
			this.PlayInteractionSound(this.interactionVolume * Mathf.Clamp(num, 0f, 2f));
		}

		// Token: 0x06008786 RID: 34694 RVA: 0x003773B0 File Offset: 0x003755B0
		public void PlayPleasureSound(AudioClip clip, float volume = 1f, float pitch = 1f)
		{
			if (!this._isActive)
			{
				return;
			}
			if (clip == null)
			{
				return;
			}
			if (this._audioSource == null)
			{
				return;
			}
			float num = Mathf.Clamp01(volume * this.masterVolume);
			float num2 = Mathf.Clamp(pitch, 0.5f, 2f);
			this._audioSource.pitch = num2;
			this._audioSource.PlayOneShot(clip, num);
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[DildoAudioController] Pleasure sound: {0} | Volume:{1:F2} | Pitch:{2:F2}", clip.name, num, num2), this);
			}
		}

		// Token: 0x06008787 RID: 34695 RVA: 0x00377444 File Offset: 0x00375644
		public void PlayPleasureSound(AudioClip[] clips, float volume = 1f)
		{
			if (!this._isActive)
			{
				return;
			}
			if (clips == null || clips.Length == 0)
			{
				return;
			}
			AudioClip randomClip = this.GetRandomClip(clips);
			if (randomClip == null)
			{
				return;
			}
			float num = 1f + Random.Range(-this.interactionPitchVariation, this.interactionPitchVariation);
			this.PlayPleasureSound(randomClip, volume, num);
		}

		// Token: 0x06008788 RID: 34696 RVA: 0x00377498 File Offset: 0x00375698
		private void PlayInteractionSound(float volumeOverride = -1f)
		{
			if (this._audioSource == null || this.interactionClips == null || this.interactionClips.Length == 0)
			{
				return;
			}
			AudioClip audioClip = (this.useRandomClip ? this.GetRandomClip(this.interactionClips) : this.interactionClips[0]);
			if (audioClip == null)
			{
				return;
			}
			float num = ((volumeOverride >= 0f) ? volumeOverride : this.interactionVolume);
			num = Mathf.Clamp01(num * this.masterVolume);
			float num2 = 1f + Random.Range(-this.interactionPitchVariation, this.interactionPitchVariation);
			this._audioSource.pitch = num2;
			this._audioSource.PlayOneShot(audioClip, num);
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[DildoAudioController] Interaction: {0} | Volume:{1:F2} | Pitch:{2:F2} | Frame:{3}", new object[]
				{
					audioClip.name,
					num,
					num2,
					Time.frameCount
				}), this);
			}
		}

		// Token: 0x06008789 RID: 34697 RVA: 0x00377588 File Offset: 0x00375788
		private AudioClip GetRandomClip(AudioClip[] clips)
		{
			if (clips == null || clips.Length == 0)
			{
				return null;
			}
			if (clips.Length == 1)
			{
				return clips[0];
			}
			int num = 0;
			int num2;
			do
			{
				num2 = Random.Range(0, clips.Length);
				num++;
			}
			while (num2 == this._lastClipIndex && clips.Length > 1 && num < 10);
			this._lastClipIndex = num2;
			return clips[num2];
		}

		// Token: 0x0600878A RID: 34698 RVA: 0x003775D8 File Offset: 0x003757D8
		private void SetupAudioSource()
		{
			if (this._audioSource == null)
			{
				this._audioSource = base.gameObject.AddComponent<AudioSource>();
			}
			this._audioSource.playOnAwake = false;
			this._audioSource.loop = false;
			this._audioSource.spatialBlend = (this.use3DAudio ? 1f : 0f);
			this._audioSource.minDistance = this.minDistance;
			this._audioSource.maxDistance = this.maxDistance;
			this._audioSource.rolloffMode = AudioRolloffMode.Linear;
		}

		// Token: 0x0600878B RID: 34699 RVA: 0x00377669 File Offset: 0x00375869
		private void OnDisable()
		{
			if (this._audioSource != null)
			{
				this._audioSource.Stop();
			}
		}

		// Token: 0x04008A17 RID: 35351
		[Header("Interaction Sounds (Animation Events)")]
		[Tooltip("Audio clips for physical interaction sounds (impact, movement, etc.)")]
		[SerializeField]
		private AudioClip[] interactionClips;

		// Token: 0x04008A18 RID: 35352
		[SerializeField]
		[Range(0f, 1f)]
		private float interactionVolume = 0.7f;

		// Token: 0x04008A19 RID: 35353
		[SerializeField]
		[Range(0f, 0.2f)]
		private float interactionPitchVariation = 0.05f;

		// Token: 0x04008A1A RID: 35354
		[SerializeField]
		[Range(0f, 1f)]
		private float minAnimationWeight = 0.3f;

		// Token: 0x04008A1B RID: 35355
		[SerializeField]
		private bool useRandomClip = true;

		// Token: 0x04008A1C RID: 35356
		[Header("Pleasure Sounds (Code Triggered)")]
		[Tooltip("Default volume for pleasure sounds when not specified")]
		[SerializeField]
		[Range(0f, 1f)]
		private float pleasureVolume = 0.6f;

		// Token: 0x04008A1D RID: 35357
		[Header("3D Audio Settings")]
		[SerializeField]
		private bool use3DAudio = true;

		// Token: 0x04008A1E RID: 35358
		[SerializeField]
		private float minDistance = 0.5f;

		// Token: 0x04008A1F RID: 35359
		[SerializeField]
		private float maxDistance = 10f;

		// Token: 0x04008A20 RID: 35360
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04008A21 RID: 35361
		[SerializeField]
		[Range(0f, 1f)]
		private float masterVolume = 1f;

		// Token: 0x04008A22 RID: 35362
		private AudioSource _audioSource;

		// Token: 0x04008A23 RID: 35363
		private bool _isActive;

		// Token: 0x04008A24 RID: 35364
		private int _lastClipIndex = -1;
	}
}
