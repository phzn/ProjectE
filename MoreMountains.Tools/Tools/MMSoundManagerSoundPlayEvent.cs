using System;
using UnityEngine;
using UnityEngine.Audio;

namespace MoreMountains.Tools
{
	// Token: 0x020000CF RID: 207
	public struct MMSoundManagerSoundPlayEvent
	{
		// Token: 0x0600044E RID: 1102 RVA: 0x0001419B File Offset: 0x0001239B
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void RuntimeInitialization()
		{
			MMSoundManagerSoundPlayEvent.OnEvent = null;
		}

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x0600044F RID: 1103 RVA: 0x000141A4 File Offset: 0x000123A4
		// (remove) Token: 0x06000450 RID: 1104 RVA: 0x000141D8 File Offset: 0x000123D8
		private static event MMSoundManagerSoundPlayEvent.Delegate OnEvent;

		// Token: 0x06000451 RID: 1105 RVA: 0x0001420B File Offset: 0x0001240B
		public static void Register(MMSoundManagerSoundPlayEvent.Delegate callback)
		{
			MMSoundManagerSoundPlayEvent.OnEvent += callback;
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x00014213 File Offset: 0x00012413
		public static void Unregister(MMSoundManagerSoundPlayEvent.Delegate callback)
		{
			MMSoundManagerSoundPlayEvent.OnEvent -= callback;
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x0001421B File Offset: 0x0001241B
		public static AudioSource Trigger(AudioClip clip, MMSoundManagerPlayOptions options)
		{
			MMSoundManagerSoundPlayEvent.Delegate onEvent = MMSoundManagerSoundPlayEvent.OnEvent;
			if (onEvent == null)
			{
				return null;
			}
			return onEvent(clip, options);
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x00014230 File Offset: 0x00012430
		public static AudioSource Trigger(AudioClip audioClip, MMSoundManager.MMSoundManagerTracks mmSoundManagerTrack, Vector3 location, bool loop = false, float volume = 1f, int ID = 0, bool fade = false, float fadeInitialVolume = 0f, float fadeDuration = 1f, MMTweenType fadeTween = null, bool persistent = false, AudioSource recycleAudioSource = null, AudioMixerGroup audioGroup = null, float pitch = 1f, float panStereo = 0f, float spatialBlend = 0f, bool soloSingleTrack = false, bool soloAllTracks = false, bool autoUnSoloOnEnd = false, bool bypassEffects = false, bool bypassListenerEffects = false, bool bypassReverbZones = false, int priority = 128, float reverbZoneMix = 1f, float dopplerLevel = 1f, int spread = 0, AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic, float minDistance = 1f, float maxDistance = 500f)
		{
			MMSoundManagerPlayOptions @default = MMSoundManagerPlayOptions.Default;
			@default.MmSoundManagerTrack = mmSoundManagerTrack;
			@default.Location = location;
			@default.Loop = loop;
			@default.Volume = volume;
			@default.ID = ID;
			@default.Fade = fade;
			@default.FadeInitialVolume = fadeInitialVolume;
			@default.FadeDuration = fadeDuration;
			@default.FadeTween = fadeTween;
			@default.Persistent = persistent;
			@default.RecycleAudioSource = recycleAudioSource;
			@default.AudioGroup = audioGroup;
			@default.Pitch = pitch;
			@default.PanStereo = panStereo;
			@default.SpatialBlend = spatialBlend;
			@default.SoloSingleTrack = soloSingleTrack;
			@default.SoloAllTracks = soloAllTracks;
			@default.AutoUnSoloOnEnd = autoUnSoloOnEnd;
			@default.BypassEffects = bypassEffects;
			@default.BypassListenerEffects = bypassListenerEffects;
			@default.BypassReverbZones = bypassReverbZones;
			@default.Priority = priority;
			@default.ReverbZoneMix = reverbZoneMix;
			@default.DopplerLevel = dopplerLevel;
			@default.Spread = spread;
			@default.RolloffMode = rolloffMode;
			@default.MinDistance = minDistance;
			@default.MaxDistance = maxDistance;
			MMSoundManagerSoundPlayEvent.Delegate onEvent = MMSoundManagerSoundPlayEvent.OnEvent;
			if (onEvent == null)
			{
				return null;
			}
			return onEvent(audioClip, @default);
		}

		// Token: 0x02000411 RID: 1041
		// (Invoke) Token: 0x06001997 RID: 6551
		public delegate AudioSource Delegate(AudioClip clip, MMSoundManagerPlayOptions options);
	}
}
