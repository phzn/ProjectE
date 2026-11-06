using System;
using UnityEngine;

namespace RayznGames.VehicleSystem
{
	// Token: 0x02000004 RID: 4
	public class RollerbladeAudioController : MonoBehaviour
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000008 RID: 8 RVA: 0x0000210E File Offset: 0x0000030E
		public bool IsActive
		{
			get
			{
				return this.isActive;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000009 RID: 9 RVA: 0x00002116 File Offset: 0x00000316
		// (set) Token: 0x0600000A RID: 10 RVA: 0x0000211E File Offset: 0x0000031E
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

		// Token: 0x0600000B RID: 11 RVA: 0x0000212C File Offset: 0x0000032C
		public void Activate()
		{
			this.SetupAudioSources();
			this.isActive = true;
			if (this.debugLog)
			{
				Debug.Log("[RollerbladeAudioController] Activated");
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002150 File Offset: 0x00000350
		public void Deactivate()
		{
			this.isActive = false;
			if (this.stepSource != null)
			{
				this.stepSource.Stop();
			}
			if (this.rollingLoopSource != null)
			{
				this.rollingLoopSource.Stop();
			}
			if (this.debugLog)
			{
				Debug.Log("[RollerbladeAudioController] Deactivated");
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000021A8 File Offset: 0x000003A8
		public void UpdateAudio(float speed, bool braking, float moveInput)
		{
			if (!this.isActive)
			{
				return;
			}
			this.currentSpeed = Mathf.Abs(speed);
			this.UpdateRollingLoop();
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000021C8 File Offset: 0x000003C8
		public void PlayStepFromAnimationEvent(AnimationEvent evt)
		{
			if (!this.isActive)
			{
				return;
			}
			if (evt != null && evt.animatorClipInfo.weight < this.minAnimationWeight)
			{
				return;
			}
			this.PlayStepInternal(-1f);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002204 File Offset: 0x00000404
		public void PlayStepWithIntensityFromAnimationEvent(AnimationEvent evt)
		{
			if (!this.isActive)
			{
				return;
			}
			if (evt != null && evt.animatorClipInfo.weight < this.minAnimationWeight)
			{
				return;
			}
			float num = ((evt != null && evt.floatParameter > 0f) ? evt.floatParameter : 1f);
			this.PlayStepInternal(this.stepVolume * Mathf.Clamp(num, 0f, 2f));
		}

		// Token: 0x06000010 RID: 16 RVA: 0x0000226F File Offset: 0x0000046F
		public void SetVolume(float volume)
		{
			this.masterVolume = Mathf.Clamp01(volume);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002280 File Offset: 0x00000480
		private void SetupAudioSources()
		{
			if (this.stepSource == null)
			{
				this.stepSource = base.gameObject.AddComponent<AudioSource>();
			}
			this.stepSource.playOnAwake = false;
			this.stepSource.loop = false;
			this.stepSource.spatialBlend = (this.step3DSound ? 1f : 0f);
			this.stepSource.minDistance = this.stepMinDistance;
			this.stepSource.maxDistance = this.stepMaxDistance;
			this.stepSource.rolloffMode = AudioRolloffMode.Linear;
			if (this.rollingLoopSource != null)
			{
				this.rollingLoopSource.playOnAwake = false;
				this.rollingLoopSource.loop = true;
				this.rollingLoopSource.spatialBlend = 1f;
				if (this.rollingLoopClip != null)
				{
					this.rollingLoopSource.clip = this.rollingLoopClip;
				}
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002368 File Offset: 0x00000568
		private void PlayStepInternal(float volumeOverride = -1f)
		{
			if (this.stepSource == null || this.stepClips == null || this.stepClips.Length == 0)
			{
				return;
			}
			if (this.currentSpeed < this.minSpeedForStepSound)
			{
				return;
			}
			float num = Mathf.Clamp01(this.currentSpeed / Mathf.Max(0.01f, this.maxSpeedReference));
			float num2 = ((volumeOverride > 0f) ? volumeOverride : this.stepVolume);
			if (this.scaleStepVolumeWithSpeed && volumeOverride < 0f)
			{
				num2 = Mathf.Lerp(this.stepVolume * 0.5f, this.stepVolume, num);
			}
			AudioClip randomStepClip = this.GetRandomStepClip();
			if (randomStepClip == null)
			{
				return;
			}
			this.stepSource.pitch = 1f + Random.Range(-this.pitchVariation, this.pitchVariation);
			this.stepSource.PlayOneShot(randomStepClip, Mathf.Clamp01(num2 * this.masterVolume));
			if (this.debugLog)
			{
				Debug.Log(string.Format("[RollerbladeAudioController] Step | Clip:{0} | Speed:{1:F2} | Frame:{2}", randomStepClip.name, this.currentSpeed, Time.frameCount));
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000247C File Offset: 0x0000067C
		private AudioClip GetRandomStepClip()
		{
			if (this.stepClips == null || this.stepClips.Length == 0)
			{
				return null;
			}
			if (this.stepClips.Length == 1)
			{
				return this.stepClips[0];
			}
			int num;
			do
			{
				num = Random.Range(0, this.stepClips.Length);
			}
			while (num == this.lastStepClipIndex && this.stepClips.Length > 1);
			this.lastStepClipIndex = num;
			return this.stepClips[num];
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000024E4 File Offset: 0x000006E4
		private void UpdateRollingLoop()
		{
			if (!this.useRollingLoop || this.rollingLoopSource == null || this.rollingLoopClip == null)
			{
				return;
			}
			float num = Mathf.Clamp01(this.currentSpeed / Mathf.Max(0.01f, this.maxSpeedReference));
			float num2 = ((this.currentSpeed >= this.minSpeedForRollingLoop) ? (Mathf.Lerp(this.rollingVolumeRange.x, this.rollingVolumeRange.y, num) * this.masterVolume) : 0f);
			this.rollingLoopSource.volume = Mathf.MoveTowards(this.rollingLoopSource.volume, num2, Time.deltaTime / Mathf.Max(0.01f, this.rollingLoopFadeDuration));
			this.rollingLoopSource.pitch = Mathf.Lerp(this.rollingPitchRange.x, this.rollingPitchRange.y, num);
			if (num2 > 0.01f && !this.rollingLoopSource.isPlaying)
			{
				this.rollingLoopSource.Play();
				return;
			}
			if (num2 <= 0.01f && this.rollingLoopSource.isPlaying)
			{
				this.rollingLoopSource.Stop();
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002606 File Offset: 0x00000806
		private void OnDisable()
		{
			if (this.stepSource != null)
			{
				this.stepSource.Stop();
			}
			if (this.rollingLoopSource != null)
			{
				this.rollingLoopSource.Stop();
			}
		}

		// Token: 0x04000002 RID: 2
		[Header("Step Audio (Animation Events)")]
		[SerializeField]
		private AudioSource stepSource;

		// Token: 0x04000003 RID: 3
		[SerializeField]
		private AudioClip[] stepClips;

		// Token: 0x04000004 RID: 4
		[SerializeField]
		[Range(0f, 1f)]
		private float stepVolume = 0.6f;

		// Token: 0x04000005 RID: 5
		[SerializeField]
		[Range(0f, 0.2f)]
		private float pitchVariation = 0.08f;

		// Token: 0x04000006 RID: 6
		[SerializeField]
		private bool scaleStepVolumeWithSpeed = true;

		// Token: 0x04000007 RID: 7
		[SerializeField]
		private float minSpeedForStepSound = 0.1f;

		// Token: 0x04000008 RID: 8
		[SerializeField]
		[Range(0f, 1f)]
		private float minAnimationWeight = 0.5f;

		// Token: 0x04000009 RID: 9
		[Header("3D Step Audio")]
		[SerializeField]
		private bool step3DSound = true;

		// Token: 0x0400000A RID: 10
		[SerializeField]
		private float stepMinDistance = 1f;

		// Token: 0x0400000B RID: 11
		[SerializeField]
		private float stepMaxDistance = 15f;

		// Token: 0x0400000C RID: 12
		[Header("Speed Reference")]
		[SerializeField]
		private float maxSpeedReference = 12f;

		// Token: 0x0400000D RID: 13
		[Header("Rolling Loop (Optional)")]
		[SerializeField]
		private AudioSource rollingLoopSource;

		// Token: 0x0400000E RID: 14
		[SerializeField]
		private AudioClip rollingLoopClip;

		// Token: 0x0400000F RID: 15
		[SerializeField]
		private bool useRollingLoop = true;

		// Token: 0x04000010 RID: 16
		[SerializeField]
		private float minSpeedForRollingLoop = 2.5f;

		// Token: 0x04000011 RID: 17
		[SerializeField]
		private float rollingLoopFadeDuration = 0.2f;

		// Token: 0x04000012 RID: 18
		[SerializeField]
		private Vector2 rollingVolumeRange = new Vector2(0.05f, 0.35f);

		// Token: 0x04000013 RID: 19
		[SerializeField]
		private Vector2 rollingPitchRange = new Vector2(0.95f, 1.2f);

		// Token: 0x04000014 RID: 20
		[Header("Debug")]
		[SerializeField]
		private bool debugLog;

		// Token: 0x04000015 RID: 21
		[SerializeField]
		[Range(0f, 1f)]
		private float masterVolume = 1f;

		// Token: 0x04000016 RID: 22
		private bool isActive;

		// Token: 0x04000017 RID: 23
		private float currentSpeed;

		// Token: 0x04000018 RID: 24
		private int lastStepClipIndex = -1;
	}
}
