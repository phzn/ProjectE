using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace MoreMountains.Tools
{
	// Token: 0x020000D3 RID: 211
	[AddComponentMenu("More Mountains/Tools/Audio/MMSoundManager")]
	public class MMSoundManager : MMPersistentSingleton<MMSoundManager>, MMEventListener<MMSoundManagerTrackEvent>, MMEventListenerBase, MMEventListener<MMSoundManagerEvent>, MMEventListener<MMSoundManagerSoundControlEvent>, MMEventListener<MMSoundManagerSoundFadeEvent>, MMEventListener<MMSoundManagerAllSoundsControlEvent>, MMEventListener<MMSoundManagerTrackFadeEvent>
	{
		// Token: 0x06000459 RID: 1113 RVA: 0x0001440B File Offset: 0x0001260B
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		protected static void InitializeStatics()
		{
			MMPersistentSingleton<MMSoundManager>._instance = null;
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x00014413 File Offset: 0x00012613
		protected override void Awake()
		{
			base.Awake();
			this.InitializeSoundManager();
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x00014421 File Offset: 0x00012621
		protected virtual void Start()
		{
			if (this.settingsSo != null && this.settingsSo.Settings.AutoLoad)
			{
				this.settingsSo.LoadSoundSettings();
			}
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x00014450 File Offset: 0x00012650
		protected virtual void InitializeSoundManager()
		{
			if (this._pool == null)
			{
				this._pool = new MMSoundManagerAudioPool();
			}
			this._sounds = new List<MMSoundManagerSound>();
			this._pool.FillAudioSourcePool(this.AudioSourcePoolSize, base.transform);
			this._fadeInSoundCoroutines = new Dictionary<AudioSource, Coroutine>();
			this._fadeOutSoundCoroutines = new Dictionary<AudioSource, Coroutine>();
			this._fadeTrackCoroutines = new Dictionary<MMSoundManager.MMSoundManagerTracks, Coroutine>();
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x000144B4 File Offset: 0x000126B4
		public virtual AudioSource PlaySound(AudioClip audioClip, MMSoundManagerPlayOptions options)
		{
			return this.PlaySound(audioClip, options.MmSoundManagerTrack, options.Location, options.Loop, options.Volume, options.ID, options.Fade, options.FadeInitialVolume, options.FadeDuration, options.FadeTween, options.Persistent, options.RecycleAudioSource, options.AudioGroup, options.Pitch, options.PanStereo, options.SpatialBlend, options.SoloSingleTrack, options.SoloAllTracks, options.AutoUnSoloOnEnd, options.BypassEffects, options.BypassListenerEffects, options.BypassReverbZones, options.Priority, options.ReverbZoneMix, options.DopplerLevel, options.Spread, options.RolloffMode, options.MinDistance, options.MaxDistance, options.DoNotAutoRecycleIfNotDonePlaying, options.PlaybackTime, options.PlaybackDuration, options.AttachToTransform, options.UseSpreadCurve, options.SpreadCurve, options.UseCustomRolloffCurve, options.CustomRolloffCurve, options.UseSpatialBlendCurve, options.SpatialBlendCurve, options.UseReverbZoneMixCurve, options.ReverbZoneMixCurve);
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x000145B8 File Offset: 0x000127B8
		public virtual AudioSource PlaySound(AudioClip audioClip, MMSoundManager.MMSoundManagerTracks mmSoundManagerTrack, Vector3 location, bool loop = false, float volume = 1f, int ID = 0, bool fade = false, float fadeInitialVolume = 0f, float fadeDuration = 1f, MMTweenType fadeTween = null, bool persistent = false, AudioSource recycleAudioSource = null, AudioMixerGroup audioGroup = null, float pitch = 1f, float panStereo = 0f, float spatialBlend = 0f, bool soloSingleTrack = false, bool soloAllTracks = false, bool autoUnSoloOnEnd = false, bool bypassEffects = false, bool bypassListenerEffects = false, bool bypassReverbZones = false, int priority = 128, float reverbZoneMix = 1f, float dopplerLevel = 1f, int spread = 0, AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic, float minDistance = 1f, float maxDistance = 500f, bool doNotAutoRecycleIfNotDonePlaying = false, float playbackTime = 0f, float playbackDuration = 0f, Transform attachToTransform = null, bool useSpreadCurve = false, AnimationCurve spreadCurve = null, bool useCustomRolloffCurve = false, AnimationCurve customRolloffCurve = null, bool useSpatialBlendCurve = false, AnimationCurve spatialBlendCurve = null, bool useReverbZoneMixCurve = false, AnimationCurve reverbZoneMixCurve = null)
		{
			if (this == null)
			{
				return null;
			}
			if (!audioClip)
			{
				return null;
			}
			AudioSource audioSource = recycleAudioSource;
			if (!audioSource)
			{
				audioSource = this._pool.GetAvailableAudioSource(this.PoolCanExpand, base.transform);
				audioSource.clip = audioClip;
				if (audioSource && !loop)
				{
					recycleAudioSource = audioSource;
					base.StartCoroutine(this._pool.AutoDisableAudioSource(audioClip.length / Mathf.Abs(pitch), audioSource, audioClip, doNotAutoRecycleIfNotDonePlaying, playbackTime, playbackDuration));
				}
			}
			if (!audioSource)
			{
				this._tempAudioSourceGameObject = new GameObject("MMAudio_" + audioClip.name);
				SceneManager.MoveGameObjectToScene(this._tempAudioSourceGameObject, base.gameObject.scene);
				audioSource = this._tempAudioSourceGameObject.AddComponent<AudioSource>();
			}
			audioSource.transform.position = location;
			audioSource.clip = audioClip;
			audioSource.pitch = pitch;
			audioSource.spatialBlend = spatialBlend;
			audioSource.panStereo = panStereo;
			audioSource.loop = loop;
			audioSource.bypassEffects = bypassEffects;
			audioSource.bypassListenerEffects = bypassListenerEffects;
			audioSource.bypassReverbZones = bypassReverbZones;
			audioSource.priority = priority;
			audioSource.reverbZoneMix = reverbZoneMix;
			audioSource.dopplerLevel = dopplerLevel;
			audioSource.spread = (float)spread;
			audioSource.rolloffMode = rolloffMode;
			audioSource.minDistance = minDistance;
			audioSource.maxDistance = maxDistance;
			audioSource.time = playbackTime;
			if (useSpreadCurve)
			{
				audioSource.SetCustomCurve(AudioSourceCurveType.Spread, spreadCurve);
			}
			if (useCustomRolloffCurve)
			{
				audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, customRolloffCurve);
			}
			if (useSpatialBlendCurve)
			{
				audioSource.SetCustomCurve(AudioSourceCurveType.SpatialBlend, spatialBlendCurve);
			}
			if (useReverbZoneMixCurve)
			{
				audioSource.SetCustomCurve(AudioSourceCurveType.ReverbZoneMix, reverbZoneMixCurve);
			}
			if (attachToTransform != null)
			{
				MMFollowTarget mmfollowTarget = audioSource.gameObject.MMGetComponentNoAlloc<MMFollowTarget>();
				if (mmfollowTarget == null)
				{
					mmfollowTarget = audioSource.gameObject.AddComponent<MMFollowTarget>();
				}
				mmfollowTarget.Target = attachToTransform;
				mmfollowTarget.InterpolatePosition = false;
				mmfollowTarget.InterpolateRotation = false;
				mmfollowTarget.InterpolateScale = false;
				mmfollowTarget.FollowRotation = false;
				mmfollowTarget.FollowScale = false;
				mmfollowTarget.enabled = true;
			}
			if (this.settingsSo != null)
			{
				audioSource.outputAudioMixerGroup = this.settingsSo.MasterAudioMixerGroup;
				switch (mmSoundManagerTrack)
				{
				case MMSoundManager.MMSoundManagerTracks.Sfx:
					audioSource.outputAudioMixerGroup = this.settingsSo.SfxAudioMixerGroup;
					break;
				case MMSoundManager.MMSoundManagerTracks.Music:
					audioSource.outputAudioMixerGroup = this.settingsSo.MusicAudioMixerGroup;
					break;
				case MMSoundManager.MMSoundManagerTracks.UI:
					audioSource.outputAudioMixerGroup = this.settingsSo.UIAudioMixerGroup;
					break;
				case MMSoundManager.MMSoundManagerTracks.Master:
					audioSource.outputAudioMixerGroup = this.settingsSo.MasterAudioMixerGroup;
					break;
				}
			}
			if (audioGroup)
			{
				audioSource.outputAudioMixerGroup = audioGroup;
			}
			audioSource.volume = volume;
			audioSource.Play();
			if (!loop && !recycleAudioSource)
			{
				float num = ((playbackDuration > 0f) ? playbackDuration : (audioClip.length - playbackTime));
				Object.Destroy(this._tempAudioSourceGameObject, num);
			}
			if (fade)
			{
				this.FadeSound(audioSource, fadeDuration, fadeInitialVolume, volume, fadeTween, false);
			}
			if (soloSingleTrack)
			{
				this.MuteSoundsOnTrack(mmSoundManagerTrack, true, 0f);
				audioSource.mute = false;
				if (autoUnSoloOnEnd)
				{
					this.MuteSoundsOnTrack(mmSoundManagerTrack, false, audioClip.length);
				}
			}
			else if (soloAllTracks)
			{
				this.MuteAllSounds(true);
				audioSource.mute = false;
				if (autoUnSoloOnEnd)
				{
					base.StartCoroutine(this.MuteAllSoundsCoroutine(audioClip.length - playbackTime, false));
				}
			}
			this._sound.ID = ID;
			this._sound.Track = mmSoundManagerTrack;
			this._sound.Source = audioSource;
			this._sound.Persistent = persistent;
			this._sound.PlaybackTime = playbackTime;
			this._sound.PlaybackDuration = playbackDuration;
			bool flag = false;
			for (int i = 0; i < this._sounds.Count; i++)
			{
				if (this._sounds[i].Source == audioSource)
				{
					this._sounds[i] = this._sound;
					flag = true;
				}
			}
			if (!flag)
			{
				this._sounds.Add(this._sound);
			}
			return audioSource;
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00014986 File Offset: 0x00012B86
		public virtual void PauseSound(AudioSource source)
		{
			source.Pause();
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x0001498E File Offset: 0x00012B8E
		public virtual void ResumeSound(AudioSource source)
		{
			source.Play();
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x00014996 File Offset: 0x00012B96
		public virtual void StopSound(AudioSource source)
		{
			source.Stop();
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x0001499E File Offset: 0x00012B9E
		public virtual void FreeSound(AudioSource source)
		{
			source.Stop();
			if (!this._pool.FreeSound(source))
			{
				Object.Destroy(source.gameObject);
			}
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x000149BF File Offset: 0x00012BBF
		public virtual void MuteTrack(MMSoundManager.MMSoundManagerTracks track)
		{
			this.ControlTrack(track, MMSoundManager.ControlTrackModes.Mute, 0f);
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x000149CE File Offset: 0x00012BCE
		public virtual void UnmuteTrack(MMSoundManager.MMSoundManagerTracks track)
		{
			this.ControlTrack(track, MMSoundManager.ControlTrackModes.Unmute, 0f);
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x000149DD File Offset: 0x00012BDD
		public virtual void SetTrackVolume(MMSoundManager.MMSoundManagerTracks track, float volume)
		{
			this.ControlTrack(track, MMSoundManager.ControlTrackModes.SetVolume, volume);
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x000149E8 File Offset: 0x00012BE8
		public virtual float GetTrackVolume(MMSoundManager.MMSoundManagerTracks track, bool mutedVolume)
		{
			switch (track)
			{
			case MMSoundManager.MMSoundManagerTracks.Sfx:
				if (mutedVolume)
				{
					return this.settingsSo.Settings.MutedSfxVolume;
				}
				return this.settingsSo.Settings.SfxVolume;
			case MMSoundManager.MMSoundManagerTracks.Music:
				if (mutedVolume)
				{
					return this.settingsSo.Settings.MutedMusicVolume;
				}
				return this.settingsSo.Settings.MusicVolume;
			case MMSoundManager.MMSoundManagerTracks.UI:
				if (mutedVolume)
				{
					return this.settingsSo.Settings.MutedUIVolume;
				}
				return this.settingsSo.Settings.UIVolume;
			case MMSoundManager.MMSoundManagerTracks.Master:
				if (mutedVolume)
				{
					return this.settingsSo.Settings.MutedMasterVolume;
				}
				return this.settingsSo.Settings.MasterVolume;
			default:
				return 1f;
			}
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x00014AAC File Offset: 0x00012CAC
		public virtual void PauseTrack(MMSoundManager.MMSoundManagerTracks track)
		{
			foreach (MMSoundManagerSound mmsoundManagerSound in this._sounds)
			{
				if (mmsoundManagerSound.Track == track)
				{
					mmsoundManagerSound.Source.Pause();
				}
			}
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x00014B0C File Offset: 0x00012D0C
		public virtual void PlayTrack(MMSoundManager.MMSoundManagerTracks track)
		{
			foreach (MMSoundManagerSound mmsoundManagerSound in this._sounds)
			{
				if (mmsoundManagerSound.Track == track)
				{
					mmsoundManagerSound.Source.Play();
				}
			}
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x00014B6C File Offset: 0x00012D6C
		public virtual void StopTrack(MMSoundManager.MMSoundManagerTracks track)
		{
			foreach (MMSoundManagerSound mmsoundManagerSound in this._sounds)
			{
				if (mmsoundManagerSound.Track == track)
				{
					mmsoundManagerSound.Source.Stop();
				}
			}
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x00014BCC File Offset: 0x00012DCC
		public virtual bool HasSoundsPlaying(MMSoundManager.MMSoundManagerTracks track)
		{
			foreach (MMSoundManagerSound mmsoundManagerSound in this._sounds)
			{
				if (mmsoundManagerSound.Track == track && mmsoundManagerSound.Source.isPlaying)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x00014C38 File Offset: 0x00012E38
		public virtual List<MMSoundManagerSound> GetSoundsPlaying(MMSoundManager.MMSoundManagerTracks track)
		{
			List<MMSoundManagerSound> list = new List<MMSoundManagerSound>();
			foreach (MMSoundManagerSound mmsoundManagerSound in this._sounds)
			{
				if (mmsoundManagerSound.Track == track && mmsoundManagerSound.Source.isPlaying)
				{
					list.Add(mmsoundManagerSound);
				}
			}
			return list;
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x00014CA8 File Offset: 0x00012EA8
		public virtual void FreeTrack(MMSoundManager.MMSoundManagerTracks track)
		{
			foreach (MMSoundManagerSound mmsoundManagerSound in this._sounds)
			{
				if (mmsoundManagerSound.Track == track)
				{
					mmsoundManagerSound.Source.Stop();
					mmsoundManagerSound.Source.gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x00014D1C File Offset: 0x00012F1C
		public virtual void MuteMusic()
		{
			this.MuteTrack(MMSoundManager.MMSoundManagerTracks.Music);
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x00014D25 File Offset: 0x00012F25
		public virtual void UnmuteMusic()
		{
			this.UnmuteTrack(MMSoundManager.MMSoundManagerTracks.Music);
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00014D2E File Offset: 0x00012F2E
		public virtual void MuteSfx()
		{
			this.MuteTrack(MMSoundManager.MMSoundManagerTracks.Sfx);
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x00014D37 File Offset: 0x00012F37
		public virtual void UnmuteSfx()
		{
			this.UnmuteTrack(MMSoundManager.MMSoundManagerTracks.Sfx);
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00014D40 File Offset: 0x00012F40
		public virtual void MuteUI()
		{
			this.MuteTrack(MMSoundManager.MMSoundManagerTracks.UI);
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x00014D49 File Offset: 0x00012F49
		public virtual void UnmuteUI()
		{
			this.UnmuteTrack(MMSoundManager.MMSoundManagerTracks.UI);
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00014D52 File Offset: 0x00012F52
		public virtual void MuteMaster()
		{
			this.MuteTrack(MMSoundManager.MMSoundManagerTracks.Master);
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x00014D5B File Offset: 0x00012F5B
		public virtual void UnmuteMaster()
		{
			this.UnmuteTrack(MMSoundManager.MMSoundManagerTracks.Master);
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x00014D64 File Offset: 0x00012F64
		public virtual void SetVolumeMusic(float newVolume)
		{
			this.SetTrackVolume(MMSoundManager.MMSoundManagerTracks.Music, newVolume);
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x00014D6E File Offset: 0x00012F6E
		public virtual void SetVolumeSfx(float newVolume)
		{
			this.SetTrackVolume(MMSoundManager.MMSoundManagerTracks.Sfx, newVolume);
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x00014D78 File Offset: 0x00012F78
		public virtual void SetVolumeUI(float newVolume)
		{
			this.SetTrackVolume(MMSoundManager.MMSoundManagerTracks.UI, newVolume);
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x00014D82 File Offset: 0x00012F82
		public virtual void SetVolumeMaster(float newVolume)
		{
			this.SetTrackVolume(MMSoundManager.MMSoundManagerTracks.Master, newVolume);
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x00014D8C File Offset: 0x00012F8C
		public virtual bool IsMuted(MMSoundManager.MMSoundManagerTracks track)
		{
			switch (track)
			{
			case MMSoundManager.MMSoundManagerTracks.Sfx:
				return !this.settingsSo.Settings.SfxOn;
			case MMSoundManager.MMSoundManagerTracks.Music:
				return !this.settingsSo.Settings.MusicOn;
			case MMSoundManager.MMSoundManagerTracks.UI:
				return !this.settingsSo.Settings.UIOn;
			case MMSoundManager.MMSoundManagerTracks.Master:
				return !this.settingsSo.Settings.MasterOn;
			default:
				return false;
			}
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x00014E04 File Offset: 0x00013004
		protected virtual void ControlTrack(MMSoundManager.MMSoundManagerTracks track, MMSoundManager.ControlTrackModes trackMode, float volume = 0.5f)
		{
			float num = 0f;
			switch (track)
			{
			case MMSoundManager.MMSoundManagerTracks.Sfx:
			{
				string text = this.settingsSo.Settings.SfxVolumeParameter;
				if (trackMode == MMSoundManager.ControlTrackModes.Mute)
				{
					this.settingsSo.TargetAudioMixer.GetFloat(text, out this.settingsSo.Settings.MutedSfxVolume);
					this.settingsSo.Settings.SfxOn = false;
				}
				else if (trackMode == MMSoundManager.ControlTrackModes.Unmute)
				{
					num = this.settingsSo.Settings.MutedSfxVolume;
					this.settingsSo.Settings.SfxOn = true;
				}
				break;
			}
			case MMSoundManager.MMSoundManagerTracks.Music:
			{
				string text = this.settingsSo.Settings.MusicVolumeParameter;
				if (trackMode == MMSoundManager.ControlTrackModes.Mute)
				{
					this.settingsSo.TargetAudioMixer.GetFloat(text, out this.settingsSo.Settings.MutedMusicVolume);
					this.settingsSo.Settings.MusicOn = false;
				}
				else if (trackMode == MMSoundManager.ControlTrackModes.Unmute)
				{
					num = this.settingsSo.Settings.MutedMusicVolume;
					this.settingsSo.Settings.MusicOn = true;
				}
				break;
			}
			case MMSoundManager.MMSoundManagerTracks.UI:
			{
				string text = this.settingsSo.Settings.UIVolumeParameter;
				if (trackMode == MMSoundManager.ControlTrackModes.Mute)
				{
					this.settingsSo.TargetAudioMixer.GetFloat(text, out this.settingsSo.Settings.MutedUIVolume);
					this.settingsSo.Settings.UIOn = false;
				}
				else if (trackMode == MMSoundManager.ControlTrackModes.Unmute)
				{
					num = this.settingsSo.Settings.MutedUIVolume;
					this.settingsSo.Settings.UIOn = true;
				}
				break;
			}
			case MMSoundManager.MMSoundManagerTracks.Master:
			{
				string text = this.settingsSo.Settings.MasterVolumeParameter;
				if (trackMode == MMSoundManager.ControlTrackModes.Mute)
				{
					this.settingsSo.TargetAudioMixer.GetFloat(text, out this.settingsSo.Settings.MutedMasterVolume);
					this.settingsSo.Settings.MasterOn = false;
				}
				else if (trackMode == MMSoundManager.ControlTrackModes.Unmute)
				{
					num = this.settingsSo.Settings.MutedMasterVolume;
					this.settingsSo.Settings.MasterOn = true;
				}
				break;
			}
			}
			switch (trackMode)
			{
			case MMSoundManager.ControlTrackModes.Mute:
				this.settingsSo.SetTrackVolume(track, 0f);
				break;
			case MMSoundManager.ControlTrackModes.Unmute:
				this.settingsSo.SetTrackVolume(track, this.settingsSo.MixerVolumeToNormalized(num));
				break;
			case MMSoundManager.ControlTrackModes.SetVolume:
				this.settingsSo.SetTrackVolume(track, volume);
				break;
			}
			this.settingsSo.GetTrackVolumes();
			if (this.settingsSo.Settings.AutoSave)
			{
				this.settingsSo.SaveSoundSettings();
			}
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x00015088 File Offset: 0x00013288
		public virtual void FadeTrack(MMSoundManager.MMSoundManagerTracks track, float duration, float initialVolume = 0f, float finalVolume = 1f, MMTweenType tweenType = null)
		{
			Coroutine coroutine = base.StartCoroutine(this.FadeTrackCoroutine(track, duration, initialVolume, finalVolume, tweenType));
			this._fadeTrackCoroutines[track] = coroutine;
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x000150B8 File Offset: 0x000132B8
		public virtual void FadeSound(AudioSource source, float duration, float initialVolume, float finalVolume, MMTweenType tweenType, bool freeAfterFade = false)
		{
			Coroutine coroutine = base.StartCoroutine(this.FadeCoroutine(source, duration, initialVolume, finalVolume, tweenType, freeAfterFade));
			if (initialVolume < finalVolume)
			{
				this._fadeInSoundCoroutines[source] = coroutine;
				return;
			}
			this._fadeOutSoundCoroutines[source] = coroutine;
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x000150FC File Offset: 0x000132FC
		public virtual bool SoundIsFadingIn(AudioSource source)
		{
			Coroutine coroutine;
			return this._fadeInSoundCoroutines.TryGetValue(source, out coroutine) && this._fadeInSoundCoroutines[source] != null;
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x0001512C File Offset: 0x0001332C
		public virtual bool SoundIsFadingOut(AudioSource source)
		{
			Coroutine coroutine;
			return this._fadeOutSoundCoroutines.TryGetValue(source, out coroutine) && this._fadeOutSoundCoroutines[source] != null;
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x0001515C File Offset: 0x0001335C
		public virtual void StopFadeTrack(MMSoundManager.MMSoundManagerTracks track)
		{
			Coroutine coroutine;
			if (this._fadeTrackCoroutines.TryGetValue(track, out coroutine))
			{
				base.StopCoroutine(coroutine);
				this._fadeTrackCoroutines.Remove(track);
			}
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x00015190 File Offset: 0x00013390
		public virtual void StopFadeSound(AudioSource source)
		{
			Coroutine coroutine;
			if (source != null && this._fadeInSoundCoroutines.TryGetValue(source, out coroutine) && coroutine != null)
			{
				base.StopCoroutine(coroutine);
				this._fadeInSoundCoroutines.Remove(source);
			}
			if (source != null && this._fadeOutSoundCoroutines.TryGetValue(source, out coroutine) && coroutine != null)
			{
				base.StopCoroutine(coroutine);
				this._fadeOutSoundCoroutines.Remove(source);
			}
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x000151FD File Offset: 0x000133FD
		protected virtual IEnumerator FadeTrackCoroutine(MMSoundManager.MMSoundManagerTracks track, float duration, float initialVolume, float finalVolume, MMTweenType tweenType)
		{
			float startedAt = Time.unscaledTime;
			if (tweenType == null)
			{
				tweenType = new MMTweenType(MMTween.MMTweenCurve.EaseInOutQuartic);
			}
			while (Time.unscaledTime - startedAt <= duration)
			{
				float num = MMTween.Tween(Time.unscaledTime - startedAt, 0f, duration, initialVolume, finalVolume, tweenType);
				this.settingsSo.SetTrackVolume(track, num);
				yield return null;
			}
			this.settingsSo.SetTrackVolume(track, finalVolume);
			yield break;
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x00015231 File Offset: 0x00013431
		protected virtual IEnumerator FadeCoroutine(AudioSource source, float duration, float initialVolume, float finalVolume, MMTweenType tweenType, bool freeAfterFade = false)
		{
			float startedAt = Time.unscaledTime;
			if (tweenType == null)
			{
				tweenType = new MMTweenType(MMTween.MMTweenCurve.EaseInOutQuartic);
			}
			while (Time.unscaledTime - startedAt <= duration)
			{
				float num = MMTween.Tween(Time.unscaledTime - startedAt, 0f, duration, initialVolume, finalVolume, tweenType);
				source.volume = num;
				yield return null;
			}
			source.volume = finalVolume;
			if (freeAfterFade)
			{
				this.FreeSound(source);
			}
			if (initialVolume < finalVolume)
			{
				this._fadeInSoundCoroutines[source] = null;
			}
			else
			{
				this._fadeOutSoundCoroutines[source] = null;
			}
			yield break;
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x0001526D File Offset: 0x0001346D
		public virtual void MuteSoundsOnTrack(MMSoundManager.MMSoundManagerTracks track, bool mute, float delay = 0f)
		{
			base.StartCoroutine(this.MuteSoundsOnTrackCoroutine(track, mute, delay));
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x0001527F File Offset: 0x0001347F
		public virtual void MuteAllSounds(bool mute = true)
		{
			base.StartCoroutine(this.MuteAllSoundsCoroutine(0f, mute));
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x00015294 File Offset: 0x00013494
		protected virtual IEnumerator MuteSoundsOnTrackCoroutine(MMSoundManager.MMSoundManagerTracks track, bool mute, float delay)
		{
			if (delay > 0f)
			{
				yield return MMCoroutine.WaitForUnscaled(delay);
			}
			using (List<MMSoundManagerSound>.Enumerator enumerator = this._sounds.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MMSoundManagerSound mmsoundManagerSound = enumerator.Current;
					if (mmsoundManagerSound.Track == track)
					{
						mmsoundManagerSound.Source.mute = mute;
					}
				}
				yield break;
			}
			yield break;
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x000152B8 File Offset: 0x000134B8
		protected virtual IEnumerator MuteAllSoundsCoroutine(float delay, bool mute = true)
		{
			if (delay > 0f)
			{
				yield return MMCoroutine.WaitForUnscaled(delay);
			}
			using (List<MMSoundManagerSound>.Enumerator enumerator = this._sounds.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MMSoundManagerSound mmsoundManagerSound = enumerator.Current;
					mmsoundManagerSound.Source.mute = mute;
				}
				yield break;
			}
			yield break;
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x000152D8 File Offset: 0x000134D8
		public virtual AudioSource FindByID(int ID)
		{
			foreach (MMSoundManagerSound mmsoundManagerSound in this._sounds)
			{
				if (mmsoundManagerSound.ID == ID)
				{
					return mmsoundManagerSound.Source;
				}
			}
			return null;
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x0001533C File Offset: 0x0001353C
		public virtual AudioSource FindByClip(AudioClip clip)
		{
			foreach (MMSoundManagerSound mmsoundManagerSound in this._sounds)
			{
				if (mmsoundManagerSound.Source.clip == clip)
				{
					return mmsoundManagerSound.Source;
				}
			}
			return null;
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x000153A8 File Offset: 0x000135A8
		public virtual void PauseAllSounds()
		{
			foreach (MMSoundManagerSound mmsoundManagerSound in this._sounds)
			{
				mmsoundManagerSound.Source.Pause();
			}
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x00015400 File Offset: 0x00013600
		public virtual void PlayAllSounds()
		{
			foreach (MMSoundManagerSound mmsoundManagerSound in this._sounds)
			{
				if (mmsoundManagerSound.Source.isActiveAndEnabled)
				{
					mmsoundManagerSound.Source.Play();
				}
			}
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x00015464 File Offset: 0x00013664
		public virtual void StopAllSounds()
		{
			foreach (MMSoundManagerSound mmsoundManagerSound in this._sounds)
			{
				mmsoundManagerSound.Source.Stop();
			}
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x000154BC File Offset: 0x000136BC
		public virtual void FreeAllSounds()
		{
			foreach (MMSoundManagerSound mmsoundManagerSound in this._sounds)
			{
				if (mmsoundManagerSound.Source != null)
				{
					this.FreeSound(mmsoundManagerSound.Source);
				}
			}
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x00015524 File Offset: 0x00013724
		public virtual void FreeAllSoundsButPersistent()
		{
			foreach (MMSoundManagerSound mmsoundManagerSound in this._sounds)
			{
				if (!mmsoundManagerSound.Persistent && mmsoundManagerSound.Source != null)
				{
					this.FreeSound(mmsoundManagerSound.Source);
				}
			}
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x00015594 File Offset: 0x00013794
		public virtual void FreeAllLoopingSounds()
		{
			foreach (MMSoundManagerSound mmsoundManagerSound in this._sounds)
			{
				if (mmsoundManagerSound.Source.loop && mmsoundManagerSound.Source != null)
				{
					this.FreeSound(mmsoundManagerSound.Source);
				}
			}
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x00015608 File Offset: 0x00013808
		protected virtual void OnSceneLoaded(Scene arg0, LoadSceneMode loadSceneMode)
		{
			this.FreeAllSoundsButPersistent();
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x00015610 File Offset: 0x00013810
		public virtual void OnMMEvent(MMSoundManagerTrackEvent soundManagerTrackEvent)
		{
			switch (soundManagerTrackEvent.TrackEventType)
			{
			case MMSoundManagerTrackEventTypes.MuteTrack:
				this.MuteTrack(soundManagerTrackEvent.Track);
				return;
			case MMSoundManagerTrackEventTypes.UnmuteTrack:
				this.UnmuteTrack(soundManagerTrackEvent.Track);
				return;
			case MMSoundManagerTrackEventTypes.SetVolumeTrack:
				this.SetTrackVolume(soundManagerTrackEvent.Track, soundManagerTrackEvent.Volume);
				return;
			case MMSoundManagerTrackEventTypes.PlayTrack:
				this.PlayTrack(soundManagerTrackEvent.Track);
				return;
			case MMSoundManagerTrackEventTypes.PauseTrack:
				this.PauseTrack(soundManagerTrackEvent.Track);
				return;
			case MMSoundManagerTrackEventTypes.StopTrack:
				this.StopTrack(soundManagerTrackEvent.Track);
				return;
			case MMSoundManagerTrackEventTypes.FreeTrack:
				this.FreeTrack(soundManagerTrackEvent.Track);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x000156A8 File Offset: 0x000138A8
		public virtual void OnMMEvent(MMSoundManagerEvent soundManagerEvent)
		{
			switch (soundManagerEvent.EventType)
			{
			case MMSoundManagerEventTypes.SaveSettings:
				this.SaveSettings();
				return;
			case MMSoundManagerEventTypes.LoadSettings:
				this.settingsSo.LoadSoundSettings();
				return;
			case MMSoundManagerEventTypes.ResetSettings:
				this.settingsSo.ResetSoundSettings();
				return;
			default:
				return;
			}
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x000156ED File Offset: 0x000138ED
		public virtual void SaveSettings()
		{
			this.settingsSo.SaveSoundSettings();
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x000156FA File Offset: 0x000138FA
		public virtual void LoadSettings()
		{
			this.settingsSo.LoadSoundSettings();
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x00015707 File Offset: 0x00013907
		public virtual void ResetSettings()
		{
			this.settingsSo.ResetSoundSettings();
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x00015714 File Offset: 0x00013914
		public virtual void OnMMEvent(MMSoundManagerSoundControlEvent soundControlEvent)
		{
			if (soundControlEvent.TargetSource == null)
			{
				this._tempAudioSource = this.FindByID(soundControlEvent.SoundID);
			}
			else
			{
				this._tempAudioSource = soundControlEvent.TargetSource;
			}
			if (this._tempAudioSource != null)
			{
				switch (soundControlEvent.MMSoundManagerSoundControlEventType)
				{
				case MMSoundManagerSoundControlEventTypes.Pause:
					this.PauseSound(this._tempAudioSource);
					return;
				case MMSoundManagerSoundControlEventTypes.Resume:
					this.ResumeSound(this._tempAudioSource);
					return;
				case MMSoundManagerSoundControlEventTypes.Stop:
					this.StopSound(this._tempAudioSource);
					return;
				case MMSoundManagerSoundControlEventTypes.Free:
					this.FreeSound(this._tempAudioSource);
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x000157B0 File Offset: 0x000139B0
		public virtual void OnMMEvent(MMSoundManagerTrackFadeEvent trackFadeEvent)
		{
			MMSoundManagerTrackFadeEvent.Modes mode = trackFadeEvent.Mode;
			if (mode == MMSoundManagerTrackFadeEvent.Modes.PlayFade)
			{
				this.FadeTrack(trackFadeEvent.Track, trackFadeEvent.FadeDuration, this.settingsSo.GetTrackVolume(trackFadeEvent.Track), trackFadeEvent.FinalVolume, trackFadeEvent.FadeTween);
				return;
			}
			if (mode != MMSoundManagerTrackFadeEvent.Modes.StopFade)
			{
				return;
			}
			this.StopFadeTrack(trackFadeEvent.Track);
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x00015808 File Offset: 0x00013A08
		public virtual void OnMMEvent(MMSoundManagerSoundFadeEvent soundFadeEvent)
		{
			this._tempAudioSource = this.FindByID(soundFadeEvent.SoundID);
			MMSoundManagerSoundFadeEvent.Modes mode = soundFadeEvent.Mode;
			if (mode != MMSoundManagerSoundFadeEvent.Modes.PlayFade)
			{
				if (mode != MMSoundManagerSoundFadeEvent.Modes.StopFade)
				{
					return;
				}
				this.StopFadeSound(this._tempAudioSource);
			}
			else if (this._tempAudioSource != null)
			{
				this.FadeSound(this._tempAudioSource, soundFadeEvent.FadeDuration, this._tempAudioSource.volume, soundFadeEvent.FinalVolume, soundFadeEvent.FadeTween, false);
				return;
			}
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x0001587C File Offset: 0x00013A7C
		public virtual void OnMMEvent(MMSoundManagerAllSoundsControlEvent allSoundsControlEvent)
		{
			switch (allSoundsControlEvent.EventType)
			{
			case MMSoundManagerAllSoundsControlEventTypes.Pause:
				this.PauseAllSounds();
				return;
			case MMSoundManagerAllSoundsControlEventTypes.Play:
				this.PlayAllSounds();
				return;
			case MMSoundManagerAllSoundsControlEventTypes.Stop:
				this.StopAllSounds();
				return;
			case MMSoundManagerAllSoundsControlEventTypes.Free:
				this.FreeAllSounds();
				return;
			case MMSoundManagerAllSoundsControlEventTypes.FreeAllButPersistent:
				this.FreeAllSoundsButPersistent();
				return;
			case MMSoundManagerAllSoundsControlEventTypes.FreeAllLooping:
				this.FreeAllLoopingSounds();
				return;
			default:
				return;
			}
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x000158D8 File Offset: 0x00013AD8
		public virtual void OnMMSfxEvent(AudioClip clipToPlay, AudioMixerGroup audioGroup = null, float volume = 1f, float pitch = 1f, int priority = 128)
		{
			MMSoundManagerPlayOptions @default = MMSoundManagerPlayOptions.Default;
			@default.Location = base.transform.position;
			@default.AudioGroup = audioGroup;
			@default.Volume = volume;
			@default.Pitch = pitch;
			if (priority >= 0)
			{
				@default.Priority = Mathf.Min(priority, 256);
			}
			@default.MmSoundManagerTrack = MMSoundManager.MMSoundManagerTracks.Sfx;
			@default.Loop = false;
			this.PlaySound(clipToPlay, @default);
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x00015947 File Offset: 0x00013B47
		public virtual AudioSource OnMMSoundManagerSoundPlayEvent(AudioClip clip, MMSoundManagerPlayOptions options)
		{
			return this.PlaySound(clip, options);
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x00015954 File Offset: 0x00013B54
		protected virtual void OnEnable()
		{
			MMSfxEvent.Register(new MMSfxEvent.Delegate(this.OnMMSfxEvent));
			MMSoundManagerSoundPlayEvent.Register(new MMSoundManagerSoundPlayEvent.Delegate(this.OnMMSoundManagerSoundPlayEvent));
			this.MMEventStartListening<MMSoundManagerEvent>();
			this.MMEventStartListening<MMSoundManagerTrackEvent>();
			this.MMEventStartListening<MMSoundManagerSoundControlEvent>();
			this.MMEventStartListening<MMSoundManagerTrackFadeEvent>();
			this.MMEventStartListening<MMSoundManagerSoundFadeEvent>();
			this.MMEventStartListening<MMSoundManagerAllSoundsControlEvent>();
			SceneManager.sceneLoaded += this.OnSceneLoaded;
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x000159BC File Offset: 0x00013BBC
		protected virtual void OnDisable()
		{
			if (this._enabled)
			{
				MMSfxEvent.Unregister(new MMSfxEvent.Delegate(this.OnMMSfxEvent));
				MMSoundManagerSoundPlayEvent.Unregister(new MMSoundManagerSoundPlayEvent.Delegate(this.OnMMSoundManagerSoundPlayEvent));
				this.MMEventStopListening<MMSoundManagerEvent>();
				this.MMEventStopListening<MMSoundManagerTrackEvent>();
				this.MMEventStopListening<MMSoundManagerSoundControlEvent>();
				this.MMEventStopListening<MMSoundManagerTrackFadeEvent>();
				this.MMEventStopListening<MMSoundManagerSoundFadeEvent>();
				this.MMEventStopListening<MMSoundManagerAllSoundsControlEvent>();
				SceneManager.sceneLoaded -= this.OnSceneLoaded;
			}
		}

		// Token: 0x040005C7 RID: 1479
		[Header("Settings")]
		[Tooltip("the current sound settings ")]
		public MMSoundManagerSettingsSO settingsSo;

		// Token: 0x040005C8 RID: 1480
		[Header("Pool")]
		[Tooltip("the size of the AudioSource pool, a reserve of ready-to-use sources that will get recycled. Should be approximately equal to the maximum amount of sounds that you expect to be playing at once")]
		public int AudioSourcePoolSize = 10;

		// Token: 0x040005C9 RID: 1481
		[Tooltip("whether or not the pool can expand (create new audiosources on demand). In a perfect world you'd want to avoid this, and have a sufficiently big pool, to avoid costly runtime creations.")]
		public bool PoolCanExpand = true;

		// Token: 0x040005CA RID: 1482
		protected MMSoundManagerAudioPool _pool;

		// Token: 0x040005CB RID: 1483
		protected GameObject _tempAudioSourceGameObject;

		// Token: 0x040005CC RID: 1484
		protected MMSoundManagerSound _sound;

		// Token: 0x040005CD RID: 1485
		protected List<MMSoundManagerSound> _sounds;

		// Token: 0x040005CE RID: 1486
		protected AudioSource _tempAudioSource;

		// Token: 0x040005CF RID: 1487
		protected Dictionary<AudioSource, Coroutine> _fadeInSoundCoroutines;

		// Token: 0x040005D0 RID: 1488
		protected Dictionary<AudioSource, Coroutine> _fadeOutSoundCoroutines;

		// Token: 0x040005D1 RID: 1489
		protected Dictionary<MMSoundManager.MMSoundManagerTracks, Coroutine> _fadeTrackCoroutines;

		// Token: 0x02000413 RID: 1043
		public enum MMSoundManagerTracks
		{
			// Token: 0x04001EE7 RID: 7911
			Sfx,
			// Token: 0x04001EE8 RID: 7912
			Music,
			// Token: 0x04001EE9 RID: 7913
			UI,
			// Token: 0x04001EEA RID: 7914
			Master,
			// Token: 0x04001EEB RID: 7915
			Other
		}

		// Token: 0x02000414 RID: 1044
		public enum ControlTrackModes
		{
			// Token: 0x04001EED RID: 7917
			Mute,
			// Token: 0x04001EEE RID: 7918
			Unmute,
			// Token: 0x04001EEF RID: 7919
			SetVolume
		}
	}
}
