using System;
using Dinopunch.LivingMud;
using Features.MainMenu;
using PoolWaterTools;
using UnityEngine;

namespace Features.Audio
{
	// Token: 0x02001530 RID: 5424
	[RequireComponent(typeof(Animator))]
	public class FootstepController : MonoBehaviour
	{
		// Token: 0x0600A7FC RID: 43004 RVA: 0x00462619 File Offset: 0x00460819
		internal Object ReadPoiGameplayReference(UniversityGameplayField field, int elementIndex)
		{
			if (field == UniversityGameplayField.FootstepController_footstepClips)
			{
				return this.footstepClips[elementIndex];
			}
			throw new InvalidOperationException("Unsupported FootstepController POI binding: " + field.ToString());
		}

		// Token: 0x0600A7FD RID: 43005 RVA: 0x00462645 File Offset: 0x00460845
		internal void WritePoiGameplayReference(UniversityGameplayField field, Object asset, int elementIndex)
		{
			if (field == UniversityGameplayField.FootstepController_footstepClips)
			{
				this.footstepClips[elementIndex] = (AudioClip)asset;
				return;
			}
			throw new InvalidOperationException("Unsupported FootstepController POI binding: " + field.ToString());
		}

		// Token: 0x0600A7FE RID: 43006 RVA: 0x00462677 File Offset: 0x00460877
		internal int GetPoiGameplayElementCount(UniversityGameplayField field)
		{
			if (field != UniversityGameplayField.FootstepController_footstepClips)
			{
				throw new InvalidOperationException("Unsupported FootstepController indexed POI binding: " + field.ToString());
			}
			if (this.footstepClips == null)
			{
				return 0;
			}
			return this.footstepClips.Length;
		}

		// Token: 0x14000124 RID: 292
		// (add) Token: 0x0600A7FF RID: 43007 RVA: 0x004626B0 File Offset: 0x004608B0
		// (remove) Token: 0x0600A800 RID: 43008 RVA: 0x004626E8 File Offset: 0x004608E8
		public event Action Stepped;

		// Token: 0x0600A801 RID: 43009 RVA: 0x0046271D File Offset: 0x0046091D
		internal bool OwnsAudioSource(AudioSource source)
		{
			return source != null && source == this._audioSource;
		}

		// Token: 0x0600A802 RID: 43010 RVA: 0x00462738 File Offset: 0x00460938
		private void Awake()
		{
			this._animator = base.GetComponent<Animator>();
			this._mudController = this.mudControllerOverride;
			if (this._mudController == null)
			{
				this._mudController = base.GetComponent<FootMudController>();
			}
			if (this._mudController == null)
			{
				this._mudController = base.GetComponentInParent<FootMudController>();
			}
			if (this._mudController == null)
			{
				this._mudController = base.GetComponentInChildren<FootMudController>();
			}
			this._audioSource = base.gameObject.AddComponent<AudioSource>();
			this._audioSource.playOnAwake = false;
			this._audioSource.spatialBlend = (this.is3DSound ? 1f : 0f);
			this._audioSource.minDistance = this.minDistance;
			this._audioSource.maxDistance = this.maxDistance;
			this._audioSource.rolloffMode = AudioRolloffMode.Linear;
			GameAudioRouter.Route(this._audioSource, GameAudioRouter.Channel.Sfx);
			if (this.debugMode && this._mudController == null)
			{
				Debug.LogWarning("[Footstep] " + base.gameObject.name + " cannot find FootMudController. Surface-specific clips won't be used.");
			}
		}

		// Token: 0x0600A803 RID: 43011 RVA: 0x00462858 File Offset: 0x00460A58
		public void PlayFootstep(AnimationEvent evt)
		{
			if (evt != null && evt.animatorClipInfo.weight < 0.5f)
			{
				return;
			}
			if (!this.IsFromBaseLayer(evt))
			{
				return;
			}
			this.PlayFootstepInternal(-1f);
		}

		// Token: 0x0600A804 RID: 43012 RVA: 0x00462894 File Offset: 0x00460A94
		private bool IsFromBaseLayer(AnimationEvent evt)
		{
			if (evt == null || this._animator == null)
			{
				return true;
			}
			if (this._animator.layerCount <= 1)
			{
				return true;
			}
			int fullPathHash = evt.animatorStateInfo.fullPathHash;
			return fullPathHash == 0 || fullPathHash == this._animator.GetCurrentAnimatorStateInfo(0).fullPathHash || (this._animator.IsInTransition(0) && fullPathHash == this._animator.GetNextAnimatorStateInfo(0).fullPathHash);
		}

		// Token: 0x0600A805 RID: 43013 RVA: 0x00462918 File Offset: 0x00460B18
		public void PlayFootstepManual()
		{
			this.PlayFootstepInternal(-1f);
		}

		// Token: 0x0600A806 RID: 43014 RVA: 0x00462928 File Offset: 0x00460B28
		public void PlayFootstepWithVolume(AnimationEvent evt)
		{
			if (evt != null && evt.animatorClipInfo.weight < 0.5f)
			{
				return;
			}
			if (!this.IsFromBaseLayer(evt))
			{
				return;
			}
			this.PlayFootstepInternal((evt != null && evt.floatParameter > 0f) ? evt.floatParameter : this.volume);
		}

		// Token: 0x0600A807 RID: 43015 RVA: 0x0046297C File Offset: 0x00460B7C
		private void PlayFootstepInternal(float volumeOverride = -1f)
		{
			float currentSpeed = this.GetCurrentSpeed();
			if (currentSpeed < this.minSpeedForSound)
			{
				return;
			}
			Action stepped = this.Stepped;
			if (stepped != null)
			{
				stepped();
			}
			if (ShallowPoolWater.TryPlayFootstep(base.transform, currentSpeed))
			{
				return;
			}
			if (this._audioSource == null)
			{
				return;
			}
			float num = ((volumeOverride > 0f) ? volumeOverride : this.volume);
			if (this.scaleVolumeWithSpeed && volumeOverride < 0f)
			{
				num = Mathf.Lerp(this.volume * 0.5f, this.volume, Mathf.Clamp01(currentSpeed / 2f));
			}
			AudioClip[] array = this.footstepClips;
			float num2 = 1f;
			if (this._mudController != null)
			{
				bool isInMud = this._mudController.IsInMud;
				MudType currentMudType = this._mudController.CurrentMudType;
				if (this.debugMode)
				{
					Debug.Log(string.Format("[Footstep] {0} | IsInMud: {1} | MudType: {2} | Intensity: {3:F2}", new object[]
					{
						base.gameObject.name,
						isInMud,
						currentMudType,
						this._mudController.CurrentIntensity
					}));
				}
				if (isInMud)
				{
					if ((currentMudType & MudType.Watery) != (MudType)0)
					{
						if (this.waterFootstepClips != null && this.waterFootstepClips.Length != 0)
						{
							array = this.waterFootstepClips;
							num2 = this.waterVolumeMultiplier;
							if (this.debugMode)
							{
								Debug.Log("[Footstep] Switched to WATER clips");
							}
						}
					}
					else if ((currentMudType & MudType.Sloppy) != (MudType)0 && this.mudFootstepClips != null && this.mudFootstepClips.Length != 0)
					{
						array = this.mudFootstepClips;
						num2 = this.mudVolumeMultiplier;
						if (this.debugMode)
						{
							Debug.Log("[Footstep] Switched to MUD clips");
						}
					}
				}
			}
			num *= num2;
			if (array == null || array.Length == 0)
			{
				if (this.debugMode)
				{
					Debug.LogWarning("[Footstep] No clips found to play!");
				}
				return;
			}
			AudioClip randomClip = this.GetRandomClip(array);
			if (randomClip == null)
			{
				return;
			}
			this._audioSource.pitch = 1f + Random.Range(-this.pitchVariation, this.pitchVariation);
			this._audioSource.PlayOneShot(randomClip, num);
			if (this.debugMode)
			{
				Debug.Log(string.Format("[Footstep] {0} | Playing: {1} | FinalVol: {2:F2}", base.gameObject.name, randomClip.name, num));
			}
		}

		// Token: 0x0600A808 RID: 43016 RVA: 0x00462BA0 File Offset: 0x00460DA0
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
			int num;
			do
			{
				num = Random.Range(0, clips.Length);
			}
			while (num == this._lastClipIndex && clips.Length > 1);
			this._lastClipIndex = num;
			return clips[num];
		}

		// Token: 0x0600A809 RID: 43017 RVA: 0x00462BE4 File Offset: 0x00460DE4
		private float GetCurrentSpeed()
		{
			if (this._animator == null)
			{
				return 1f;
			}
			if (!string.IsNullOrEmpty(this.speedParameterName) && this._animator.HasParameter(this.speedParameterName))
			{
				return Mathf.Abs(this._animator.GetFloat(this.speedParameterName));
			}
			if (this._animator.HasParameter("InputMagnitude"))
			{
				return Mathf.Abs(this._animator.GetFloat("InputMagnitude"));
			}
			if (this._animator.HasParameter("VerticalSpeed"))
			{
				return Mathf.Abs(this._animator.GetFloat("VerticalSpeed"));
			}
			if (this._animator.HasParameter("Speed"))
			{
				return Mathf.Abs(this._animator.GetFloat("Speed"));
			}
			return 1f;
		}

		// Token: 0x0400ACAD RID: 44205
		[Header("Audio")]
		[SerializeField]
		private AudioClip[] footstepClips;

		// Token: 0x0400ACAE RID: 44206
		[SerializeField]
		[Range(0f, 1f)]
		private float volume = 0.5f;

		// Token: 0x0400ACAF RID: 44207
		[SerializeField]
		[Range(0f, 0.2f)]
		private float pitchVariation = 0.1f;

		// Token: 0x0400ACB0 RID: 44208
		[Header("Settings")]
		[SerializeField]
		private bool scaleVolumeWithSpeed = true;

		// Token: 0x0400ACB1 RID: 44209
		[SerializeField]
		private float minSpeedForSound = 0.1f;

		// Token: 0x0400ACB2 RID: 44210
		[SerializeField]
		private string speedParameterName = "";

		// Token: 0x0400ACB3 RID: 44211
		[Header("Surface Overrides")]
		[Tooltip("Optional explicit reference. If null, the controller is auto-resolved in parent/children hierarchy.")]
		[SerializeField]
		private FootMudController mudControllerOverride;

		// Token: 0x0400ACB4 RID: 44212
		[SerializeField]
		private AudioClip[] mudFootstepClips;

		// Token: 0x0400ACB5 RID: 44213
		[SerializeField]
		private AudioClip[] waterFootstepClips;

		// Token: 0x0400ACB6 RID: 44214
		[SerializeField]
		[Range(0f, 2f)]
		private float mudVolumeMultiplier = 1f;

		// Token: 0x0400ACB7 RID: 44215
		[SerializeField]
		[Range(0f, 2f)]
		private float waterVolumeMultiplier = 1f;

		// Token: 0x0400ACB8 RID: 44216
		[Header("3D Sound")]
		[SerializeField]
		private bool is3DSound = true;

		// Token: 0x0400ACB9 RID: 44217
		[SerializeField]
		private float minDistance = 1f;

		// Token: 0x0400ACBA RID: 44218
		[SerializeField]
		private float maxDistance = 15f;

		// Token: 0x0400ACBB RID: 44219
		[Header("Debug")]
		[SerializeField]
		private bool debugMode;

		// Token: 0x0400ACBC RID: 44220
		private AudioSource _audioSource;

		// Token: 0x0400ACBD RID: 44221
		private Animator _animator;

		// Token: 0x0400ACBE RID: 44222
		private int _lastClipIndex = -1;

		// Token: 0x0400ACBF RID: 44223
		private FootMudController _mudController;
	}
}
