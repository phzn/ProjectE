using System;
using UnityEngine;
using UnityEngine.Audio;

namespace MoreMountains.Tools
{
	// Token: 0x020000D7 RID: 215
	[CreateAssetMenu(menuName = "MoreMountains/Audio/MMSoundManagerSettings")]
	[Serializable]
	public class MMSoundManagerSettingsSO : ScriptableObject
	{
		// Token: 0x060004A5 RID: 1189 RVA: 0x00015E81 File Offset: 0x00014081
		public virtual void SaveSoundSettings()
		{
			MMSaveLoadManager.Save(this.Settings, "mmsound.settings", "MMSoundManager/");
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x00015E98 File Offset: 0x00014098
		public virtual void LoadSoundSettings()
		{
			if (this.Settings.OverrideMixerSettings)
			{
				MMSoundManagerSettings mmsoundManagerSettings = (MMSoundManagerSettings)MMSaveLoadManager.Load(typeof(MMSoundManagerSettings), "mmsound.settings", "MMSoundManager/");
				if (mmsoundManagerSettings != null)
				{
					this.Settings = mmsoundManagerSettings;
					this.ApplyTrackVolumes();
				}
				MMSoundManagerEvent.Trigger(MMSoundManagerEventTypes.SettingsLoaded);
			}
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x00015EE7 File Offset: 0x000140E7
		public virtual void ResetSoundSettings()
		{
			MMSaveLoadManager.DeleteSave("mmsound.settings", "MMSoundManager/");
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x00015EF8 File Offset: 0x000140F8
		public virtual void SetTrackVolume(MMSoundManager.MMSoundManagerTracks track, float volume)
		{
			if (volume <= 0f)
			{
				volume = 0.0001f;
			}
			switch (track)
			{
			case MMSoundManager.MMSoundManagerTracks.Sfx:
				this.TargetAudioMixer.SetFloat(this.Settings.SfxVolumeParameter, this.NormalizedToMixerVolume(volume));
				this.Settings.SfxVolume = volume;
				break;
			case MMSoundManager.MMSoundManagerTracks.Music:
				this.TargetAudioMixer.SetFloat(this.Settings.MusicVolumeParameter, this.NormalizedToMixerVolume(volume));
				this.Settings.MusicVolume = volume;
				break;
			case MMSoundManager.MMSoundManagerTracks.UI:
				this.TargetAudioMixer.SetFloat(this.Settings.UIVolumeParameter, this.NormalizedToMixerVolume(volume));
				this.Settings.UIVolume = volume;
				break;
			case MMSoundManager.MMSoundManagerTracks.Master:
				this.TargetAudioMixer.SetFloat(this.Settings.MasterVolumeParameter, this.NormalizedToMixerVolume(volume));
				this.Settings.MasterVolume = volume;
				break;
			}
			if (this.Settings.AutoSave)
			{
				this.SaveSoundSettings();
			}
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x00015FF4 File Offset: 0x000141F4
		public virtual float GetTrackVolume(MMSoundManager.MMSoundManagerTracks track)
		{
			float num = 1f;
			switch (track)
			{
			case MMSoundManager.MMSoundManagerTracks.Sfx:
				this.TargetAudioMixer.GetFloat(this.Settings.SfxVolumeParameter, out num);
				break;
			case MMSoundManager.MMSoundManagerTracks.Music:
				this.TargetAudioMixer.GetFloat(this.Settings.MusicVolumeParameter, out num);
				break;
			case MMSoundManager.MMSoundManagerTracks.UI:
				this.TargetAudioMixer.GetFloat(this.Settings.UIVolumeParameter, out num);
				break;
			case MMSoundManager.MMSoundManagerTracks.Master:
				this.TargetAudioMixer.GetFloat(this.Settings.MasterVolumeParameter, out num);
				break;
			}
			return this.MixerVolumeToNormalized(num);
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x00016090 File Offset: 0x00014290
		public virtual void GetTrackVolumes()
		{
			this.Settings.MasterVolume = this.GetTrackVolume(MMSoundManager.MMSoundManagerTracks.Master);
			this.Settings.MusicVolume = this.GetTrackVolume(MMSoundManager.MMSoundManagerTracks.Music);
			this.Settings.SfxVolume = this.GetTrackVolume(MMSoundManager.MMSoundManagerTracks.Sfx);
			this.Settings.UIVolume = this.GetTrackVolume(MMSoundManager.MMSoundManagerTracks.UI);
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x000160E8 File Offset: 0x000142E8
		protected virtual void ApplyTrackVolumes()
		{
			if (this.Settings.OverrideMixerSettings)
			{
				this.TargetAudioMixer.SetFloat(this.Settings.MasterVolumeParameter, this.NormalizedToMixerVolume(this.Settings.MasterVolume));
				this.TargetAudioMixer.SetFloat(this.Settings.MusicVolumeParameter, this.NormalizedToMixerVolume(this.Settings.MusicVolume));
				this.TargetAudioMixer.SetFloat(this.Settings.SfxVolumeParameter, this.NormalizedToMixerVolume(this.Settings.SfxVolume));
				this.TargetAudioMixer.SetFloat(this.Settings.UIVolumeParameter, this.NormalizedToMixerVolume(this.Settings.UIVolume));
				if (!this.Settings.MasterOn)
				{
					this.TargetAudioMixer.SetFloat(this.Settings.MasterVolumeParameter, -80f);
				}
				if (!this.Settings.MusicOn)
				{
					this.TargetAudioMixer.SetFloat(this.Settings.MusicVolumeParameter, -80f);
				}
				if (!this.Settings.SfxOn)
				{
					this.TargetAudioMixer.SetFloat(this.Settings.SfxVolumeParameter, -80f);
				}
				if (!this.Settings.UIOn)
				{
					this.TargetAudioMixer.SetFloat(this.Settings.UIVolumeParameter, -80f);
				}
				if (this.Settings.AutoSave)
				{
					this.SaveSoundSettings();
				}
			}
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x0001625C File Offset: 0x0001445C
		public virtual float NormalizedToMixerVolume(float normalizedVolume)
		{
			return Mathf.Log10(normalizedVolume) * this.MixerValuesMultiplier;
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x0001626B File Offset: 0x0001446B
		public virtual float MixerVolumeToNormalized(float mixerVolume)
		{
			return (float)Math.Pow(10.0, (double)(mixerVolume / this.MixerValuesMultiplier));
		}

		// Token: 0x04000612 RID: 1554
		[Header("Audio Mixer")]
		[Tooltip("the audio mixer to use when playing sounds")]
		public AudioMixer TargetAudioMixer;

		// Token: 0x04000613 RID: 1555
		[Tooltip("the master group")]
		public AudioMixerGroup MasterAudioMixerGroup;

		// Token: 0x04000614 RID: 1556
		[Tooltip("the group on which to play all music sounds")]
		public AudioMixerGroup MusicAudioMixerGroup;

		// Token: 0x04000615 RID: 1557
		[Tooltip("the group on which to play all sound effects")]
		public AudioMixerGroup SfxAudioMixerGroup;

		// Token: 0x04000616 RID: 1558
		[Tooltip("the group on which to play all UI sounds")]
		public AudioMixerGroup UIAudioMixerGroup;

		// Token: 0x04000617 RID: 1559
		[Tooltip("the multiplier to apply when converting normalized volume values to audio mixer values")]
		public float MixerValuesMultiplier = 20f;

		// Token: 0x04000618 RID: 1560
		[Header("Settings Unfold")]
		[Tooltip("the full settings for this MMSoundManager")]
		public MMSoundManagerSettings Settings;

		// Token: 0x04000619 RID: 1561
		protected const string _saveFolderName = "MMSoundManager/";

		// Token: 0x0400061A RID: 1562
		protected const string _saveFileName = "mmsound.settings";
	}
}
