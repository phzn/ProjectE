using System;
using UnityEngine;

namespace RayznGames.VehicleSystem
{
	// Token: 0x02000006 RID: 6
	[CreateAssetMenu(fileName = "NewVehicleAudioPreset", menuName = "RayznGames/Vehicles/Audio Preset")]
	public class VehicleAudioPreset : ScriptableObject
	{
		// Token: 0x0600002A RID: 42 RVA: 0x00003120 File Offset: 0x00001320
		public float CalculatePitch(VehicleAudioPreset.SoundClipSettings settings, float currentSpeed)
		{
			float num = Mathf.Clamp01(currentSpeed / this.maxSpeedReference);
			return Mathf.Lerp(settings.pitchRange.x, settings.pitchRange.y, num);
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00003157 File Offset: 0x00001357
		public float CalculateVolume(VehicleAudioPreset.SoundClipSettings settings, float currentSpeed)
		{
			if (currentSpeed < settings.minSpeedThreshold)
			{
				return 0f;
			}
			return Mathf.Clamp01(currentSpeed / this.maxSpeedReference) * settings.volumeCorrection * this.globalVolume;
		}

		// Token: 0x0400002A RID: 42
		[Header("Engine/Pedaling Sounds")]
		[Tooltip("Sound when accelerating")]
		public VehicleAudioPreset.SoundClipSettings accelerationSound = new VehicleAudioPreset.SoundClipSettings();

		// Token: 0x0400002B RID: 43
		[Tooltip("Sound when decelerating/coasting")]
		public VehicleAudioPreset.SoundClipSettings decelerationSound = new VehicleAudioPreset.SoundClipSettings();

		// Token: 0x0400002C RID: 44
		[Header("Movement Sounds")]
		[Tooltip("Sound of tires/wheels rolling")]
		public VehicleAudioPreset.SoundClipSettings tireSound = new VehicleAudioPreset.SoundClipSettings();

		// Token: 0x0400002D RID: 45
		[Tooltip("Sound when braking")]
		public VehicleAudioPreset.SoundClipSettings brakeSound = new VehicleAudioPreset.SoundClipSettings();

		// Token: 0x0400002E RID: 46
		[Header("Additional Sounds")]
		[Tooltip("Sound when vehicle starts moving")]
		public VehicleAudioPreset.SoundClipSettings startSound = new VehicleAudioPreset.SoundClipSettings();

		// Token: 0x0400002F RID: 47
		[Tooltip("Sound when vehicle stops")]
		public VehicleAudioPreset.SoundClipSettings stopSound = new VehicleAudioPreset.SoundClipSettings();

		// Token: 0x04000030 RID: 48
		[Tooltip("Bell/horn sound")]
		public VehicleAudioPreset.SoundClipSettings hornSound = new VehicleAudioPreset.SoundClipSettings();

		// Token: 0x04000031 RID: 49
		[Header("Wind Sound")]
		[Tooltip("Wind sound at high speeds")]
		public VehicleAudioPreset.SoundClipSettings windSound = new VehicleAudioPreset.SoundClipSettings();

		// Token: 0x04000032 RID: 50
		[Tooltip("Speed at which wind sound starts")]
		public float windSpeedThreshold = 5f;

		// Token: 0x04000033 RID: 51
		[Header("Global Settings")]
		[Tooltip("Maximum speed for pitch/volume calculations")]
		public float maxSpeedReference = 15f;

		// Token: 0x04000034 RID: 52
		[Tooltip("Global volume multiplier")]
		[Range(0f, 1f)]
		public float globalVolume = 1f;

		// Token: 0x02000022 RID: 34
		[Serializable]
		public class SoundClipSettings
		{
			// Token: 0x04000157 RID: 343
			[Tooltip("The audio clip to play")]
			public AudioClip clip;

			// Token: 0x04000158 RID: 344
			[Tooltip("Pitch range based on vehicle speed (x = min pitch at 0 speed, y = max pitch at max speed)")]
			public Vector2 pitchRange = new Vector2(0.8f, 1.2f);

			// Token: 0x04000159 RID: 345
			[Tooltip("Volume correction multiplier")]
			[Range(0f, 2f)]
			public float volumeCorrection = 1f;

			// Token: 0x0400015A RID: 346
			[Tooltip("Minimum speed to play this sound")]
			public float minSpeedThreshold;

			// Token: 0x0400015B RID: 347
			[Tooltip("Fade in/out duration")]
			public float fadeDuration = 0.2f;
		}
	}
}
