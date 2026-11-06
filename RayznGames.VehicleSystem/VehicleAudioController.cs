using System;
using UnityEngine;

namespace RayznGames.VehicleSystem
{
	// Token: 0x02000005 RID: 5
	public class VehicleAudioController : MonoBehaviour
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000017 RID: 23 RVA: 0x00002703 File Offset: 0x00000903
		public bool IsActive
		{
			get
			{
				return this.isActive;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000018 RID: 24 RVA: 0x0000270B File Offset: 0x0000090B
		// (set) Token: 0x06000019 RID: 25 RVA: 0x00002713 File Offset: 0x00000913
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

		// Token: 0x0600001A RID: 26 RVA: 0x00002721 File Offset: 0x00000921
		public void Activate()
		{
			if (this.audioPreset == null)
			{
				Debug.LogWarning("[VehicleAudioController] No audio preset assigned!");
				return;
			}
			this.SetupAudioSources();
			this.isActive = true;
			if (this.debugLog)
			{
				Debug.Log("[VehicleAudioController] Activated");
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x0000275B File Offset: 0x0000095B
		public void Deactivate()
		{
			this.StopAllSounds();
			this.isActive = false;
			if (this.debugLog)
			{
				Debug.Log("[VehicleAudioController] Deactivated");
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x0000277C File Offset: 0x0000097C
		public void UpdateAudio(float speed, bool braking, float verticalInput)
		{
			if (!this.isActive || this.audioPreset == null)
			{
				return;
			}
			this.previousSpeed = this.currentSpeed;
			this.currentSpeed = Mathf.Abs(speed);
			this.isBraking = braking;
			this.isAccelerating = verticalInput > 0.1f;
			this.UpdateEngineSound();
			this.UpdateTireSound();
			this.UpdateBrakeSound();
			this.UpdateWindSound();
			this.CheckMovementEvents();
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000027EC File Offset: 0x000009EC
		public void PlayHorn()
		{
			if (!this.isActive || this.audioPreset == null)
			{
				return;
			}
			VehicleAudioPreset.SoundClipSettings hornSound = this.audioPreset.hornSound;
			if (hornSound.clip != null && this.oneShotSource != null)
			{
				this.oneShotSource.pitch = 1f;
				this.oneShotSource.PlayOneShot(hornSound.clip, hornSound.volumeCorrection * this.masterVolume);
				if (this.debugLog)
				{
					Debug.Log("[VehicleAudioController] Horn played");
				}
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002878 File Offset: 0x00000A78
		public void SetVolume(float volume)
		{
			this.masterVolume = Mathf.Clamp01(volume);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002888 File Offset: 0x00000A88
		private void SetupAudioSources()
		{
			if (this.engineSource != null)
			{
				this.engineSource.loop = true;
				this.engineSource.playOnAwake = false;
				this.engineSource.spatialBlend = 1f;
				if (this.audioPreset.accelerationSound.clip != null)
				{
					this.engineSource.clip = this.audioPreset.accelerationSound.clip;
				}
			}
			if (this.tireSource != null)
			{
				this.tireSource.loop = true;
				this.tireSource.playOnAwake = false;
				this.tireSource.spatialBlend = 1f;
				if (this.audioPreset.tireSound.clip != null)
				{
					this.tireSource.clip = this.audioPreset.tireSound.clip;
				}
			}
			if (this.brakeSource != null)
			{
				this.brakeSource.loop = true;
				this.brakeSource.playOnAwake = false;
				this.brakeSource.spatialBlend = 1f;
				if (this.audioPreset.brakeSound.clip != null)
				{
					this.brakeSource.clip = this.audioPreset.brakeSound.clip;
				}
			}
			if (this.windSource != null)
			{
				this.windSource.loop = true;
				this.windSource.playOnAwake = false;
				this.windSource.spatialBlend = 0.5f;
				if (this.audioPreset.windSound.clip != null)
				{
					this.windSource.clip = this.audioPreset.windSound.clip;
				}
			}
			if (this.oneShotSource != null)
			{
				this.oneShotSource.loop = false;
				this.oneShotSource.playOnAwake = false;
				this.oneShotSource.spatialBlend = 1f;
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002A70 File Offset: 0x00000C70
		private void UpdateEngineSound()
		{
			if (this.engineSource == null || this.audioPreset.accelerationSound.clip == null)
			{
				return;
			}
			VehicleAudioPreset.SoundClipSettings soundClipSettings = (this.isAccelerating ? this.audioPreset.accelerationSound : this.audioPreset.decelerationSound);
			if (this.isAccelerating && this.audioPreset.accelerationSound.clip != null)
			{
				if (this.engineSource.clip != this.audioPreset.accelerationSound.clip)
				{
					this.engineSource.clip = this.audioPreset.accelerationSound.clip;
				}
			}
			else if (!this.isAccelerating && this.audioPreset.decelerationSound.clip != null && this.engineSource.clip != this.audioPreset.decelerationSound.clip)
			{
				this.engineSource.clip = this.audioPreset.decelerationSound.clip;
			}
			float num = this.audioPreset.CalculateVolume(soundClipSettings, this.currentSpeed) * this.masterVolume;
			float num2 = this.audioPreset.CalculatePitch(soundClipSettings, this.currentSpeed);
			this.engineSource.volume = Mathf.MoveTowards(this.engineSource.volume, num, Time.deltaTime / soundClipSettings.fadeDuration);
			this.engineSource.pitch = Mathf.Lerp(this.engineSource.pitch, num2, Time.deltaTime * 5f);
			if (num > 0.01f && !this.engineSource.isPlaying)
			{
				this.engineSource.Play();
				return;
			}
			if (num <= 0.01f && this.engineSource.isPlaying)
			{
				this.engineSource.Stop();
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002C40 File Offset: 0x00000E40
		private void UpdateTireSound()
		{
			if (this.tireSource == null || this.audioPreset.tireSound.clip == null)
			{
				return;
			}
			VehicleAudioPreset.SoundClipSettings tireSound = this.audioPreset.tireSound;
			float num = this.audioPreset.CalculateVolume(tireSound, this.currentSpeed) * this.masterVolume;
			float num2 = this.audioPreset.CalculatePitch(tireSound, this.currentSpeed);
			this.tireSource.volume = Mathf.MoveTowards(this.tireSource.volume, num, Time.deltaTime / tireSound.fadeDuration);
			this.tireSource.pitch = Mathf.Lerp(this.tireSource.pitch, num2, Time.deltaTime * 5f);
			if (num > 0.01f && !this.tireSource.isPlaying)
			{
				this.tireSource.Play();
				return;
			}
			if (num <= 0.01f && this.tireSource.isPlaying)
			{
				this.tireSource.Stop();
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002D40 File Offset: 0x00000F40
		private void UpdateBrakeSound()
		{
			if (this.brakeSource == null || this.audioPreset.brakeSound.clip == null)
			{
				return;
			}
			VehicleAudioPreset.SoundClipSettings brakeSound = this.audioPreset.brakeSound;
			float num = ((this.isBraking && this.currentSpeed > 0.5f) ? (this.audioPreset.CalculateVolume(brakeSound, this.currentSpeed) * this.masterVolume) : 0f);
			float num2 = this.audioPreset.CalculatePitch(brakeSound, this.currentSpeed);
			this.brakeSource.volume = Mathf.MoveTowards(this.brakeSource.volume, num, Time.deltaTime / brakeSound.fadeDuration);
			this.brakeSource.pitch = Mathf.Lerp(this.brakeSource.pitch, num2, Time.deltaTime * 5f);
			if (num > 0.01f && !this.brakeSource.isPlaying)
			{
				this.brakeSource.Play();
				return;
			}
			if (num <= 0.01f && this.brakeSource.isPlaying)
			{
				this.brakeSource.Stop();
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002E5C File Offset: 0x0000105C
		private void UpdateWindSound()
		{
			if (this.windSource == null || this.audioPreset.windSound.clip == null)
			{
				return;
			}
			VehicleAudioPreset.SoundClipSettings windSound = this.audioPreset.windSound;
			float num = ((this.currentSpeed > this.audioPreset.windSpeedThreshold) ? (this.audioPreset.CalculateVolume(windSound, this.currentSpeed) * this.masterVolume) : 0f);
			float num2 = this.audioPreset.CalculatePitch(windSound, this.currentSpeed);
			this.windSource.volume = Mathf.MoveTowards(this.windSource.volume, num, Time.deltaTime / windSound.fadeDuration);
			this.windSource.pitch = Mathf.Lerp(this.windSource.pitch, num2, Time.deltaTime * 3f);
			if (num > 0.01f && !this.windSource.isPlaying)
			{
				this.windSource.Play();
				return;
			}
			if (num <= 0.01f && this.windSource.isPlaying)
			{
				this.windSource.Stop();
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002F74 File Offset: 0x00001174
		private void CheckMovementEvents()
		{
			if (this.previousSpeed < 0.1f && this.currentSpeed >= 0.5f)
			{
				this.PlayStartSound();
				return;
			}
			if (this.previousSpeed >= 0.5f && this.currentSpeed < 0.1f)
			{
				this.PlayStopSound();
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002FC4 File Offset: 0x000011C4
		private void PlayStartSound()
		{
			if (this.oneShotSource == null)
			{
				return;
			}
			VehicleAudioPreset.SoundClipSettings startSound = this.audioPreset.startSound;
			if (startSound.clip != null)
			{
				this.oneShotSource.PlayOneShot(startSound.clip, startSound.volumeCorrection * this.masterVolume);
				if (this.debugLog)
				{
					Debug.Log("[VehicleAudioController] Start sound played");
				}
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x0000302C File Offset: 0x0000122C
		private void PlayStopSound()
		{
			if (this.oneShotSource == null)
			{
				return;
			}
			VehicleAudioPreset.SoundClipSettings stopSound = this.audioPreset.stopSound;
			if (stopSound.clip != null)
			{
				this.oneShotSource.PlayOneShot(stopSound.clip, stopSound.volumeCorrection * this.masterVolume);
				if (this.debugLog)
				{
					Debug.Log("[VehicleAudioController] Stop sound played");
				}
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00003094 File Offset: 0x00001294
		private void StopAllSounds()
		{
			if (this.engineSource != null)
			{
				this.engineSource.Stop();
			}
			if (this.tireSource != null)
			{
				this.tireSource.Stop();
			}
			if (this.brakeSource != null)
			{
				this.brakeSource.Stop();
			}
			if (this.windSource != null)
			{
				this.windSource.Stop();
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00003105 File Offset: 0x00001305
		private void OnDisable()
		{
			this.StopAllSounds();
		}

		// Token: 0x04000019 RID: 25
		[Header("Configuration")]
		[SerializeField]
		private VehicleAudioPreset audioPreset;

		// Token: 0x0400001A RID: 26
		[Header("Audio Sources")]
		[Tooltip("Audio source for engine/pedaling sounds (place near crankset/engine)")]
		[SerializeField]
		private AudioSource engineSource;

		// Token: 0x0400001B RID: 27
		[Tooltip("Audio source for tire/wheel sounds (place near rear wheel)")]
		[SerializeField]
		private AudioSource tireSource;

		// Token: 0x0400001C RID: 28
		[Tooltip("Audio source for brake sounds (place near front wheel)")]
		[SerializeField]
		private AudioSource brakeSource;

		// Token: 0x0400001D RID: 29
		[Tooltip("Audio source for wind sounds (place near rider head position)")]
		[SerializeField]
		private AudioSource windSource;

		// Token: 0x0400001E RID: 30
		[Tooltip("Audio source for one-shot sounds like horn/bell")]
		[SerializeField]
		private AudioSource oneShotSource;

		// Token: 0x0400001F RID: 31
		[Header("Debug")]
		[SerializeField]
		private bool debugLog;

		// Token: 0x04000020 RID: 32
		[SerializeField]
		[Range(0f, 1f)]
		private float masterVolume = 1f;

		// Token: 0x04000021 RID: 33
		private float currentSpeed;

		// Token: 0x04000022 RID: 34
		private float previousSpeed;

		// Token: 0x04000023 RID: 35
		private bool isBraking;

		// Token: 0x04000024 RID: 36
		private bool isAccelerating;

		// Token: 0x04000025 RID: 37
		private bool isActive;

		// Token: 0x04000026 RID: 38
		private float engineFadeTarget;

		// Token: 0x04000027 RID: 39
		private float tireFadeTarget;

		// Token: 0x04000028 RID: 40
		private float brakeFadeTarget;

		// Token: 0x04000029 RID: 41
		private float windFadeTarget;
	}
}
