using System;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Audio;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002A5 RID: 677
	[CreateAssetMenu(menuName = "MoreMountains/Audio/MMF_SoundData")]
	[Serializable]
	public class MMF_MMSoundManagerSoundData : ScriptableObject
	{
		// Token: 0x040015CA RID: 5578
		[Header("Sound")]
		[Tooltip("the sound clip to play")]
		public AudioClip Sfx;

		// Token: 0x040015CB RID: 5579
		[Header("Random Sound")]
		[Tooltip("an array to pick a random sfx from")]
		public AudioClip[] RandomSfx;

		// Token: 0x040015CC RID: 5580
		[Tooltip("if this is true, random sfx audio clips will be played in sequential order instead of at random")]
		public bool SequentialOrder;

		// Token: 0x040015CD RID: 5581
		[Tooltip("if we're in sequential order, determines whether or not to hold at the last index, until either a cooldown is met, or the ResetSequentialIndex method is called")]
		[MMFCondition("SequentialOrder", true)]
		public bool SequentialOrderHoldLast;

		// Token: 0x040015CE RID: 5582
		[Tooltip("if we're in sequential order hold last mode, index will reset to 0 automatically after this duration, unless it's 0, in which case it'll be ignored")]
		[MMFCondition("SequentialOrderHoldLast", true)]
		public float SequentialOrderHoldCooldownDuration = 2f;

		// Token: 0x040015CF RID: 5583
		[Tooltip("if this is true, sfx will be picked at random until all have been played. once this happens, the list is shuffled again, and it starts over")]
		public bool RandomUnique;

		// Token: 0x040015D0 RID: 5584
		[Header("Sound Properties")]
		[Header("Volume")]
		[Tooltip("the minimum volume to play the sound at")]
		[Range(0f, 2f)]
		public float MinVolume = 1f;

		// Token: 0x040015D1 RID: 5585
		[Tooltip("the maximum volume to play the sound at")]
		[Range(0f, 2f)]
		public float MaxVolume = 1f;

		// Token: 0x040015D2 RID: 5586
		[Header("Pitch")]
		[Tooltip("the minimum pitch to play the sound at")]
		[Range(-3f, 3f)]
		public float MinPitch = 1f;

		// Token: 0x040015D3 RID: 5587
		[Tooltip("the maximum pitch to play the sound at")]
		[Range(-3f, 3f)]
		public float MaxPitch = 1f;

		// Token: 0x040015D4 RID: 5588
		[Header("Time")]
		[Tooltip("a timestamp (in seconds, randomized between the defined min and max) at which the sound will start playing, equivalent to the Audiosource API's Time)")]
		[MMFVector(new string[] { "Min", "Max" })]
		public Vector2 PlaybackTime = new Vector2(0f, 0f);

		// Token: 0x040015D5 RID: 5589
		[Tooltip("a duration (in seconds, randomized between the defined min and max) for which the sound will play before stopping. Ignored if min and max are zero.")]
		[MMVector(new string[] { "Min", "Max" })]
		public Vector2 PlaybackDuration = new Vector2(0f, 0f);

		// Token: 0x040015D6 RID: 5590
		[Header("Sound Manager Options")]
		[Tooltip("the track on which to play the sound. Pick the one that matches the nature of your sound")]
		public MMSoundManager.MMSoundManagerTracks MmSoundManagerTrack;

		// Token: 0x040015D7 RID: 5591
		[Tooltip("the ID of the sound. This is useful if you plan on using sound control feedbacks on it afterwards.")]
		public int ID;

		// Token: 0x040015D8 RID: 5592
		[Tooltip("the AudioGroup on which to play the sound. If you're already targeting a preset track, you can leave it blank, otherwise the group you specify here will override it.")]
		public AudioMixerGroup AudioGroup;

		// Token: 0x040015D9 RID: 5593
		[Tooltip("if (for some reason) you've already got an audiosource and wouldn't like to use the built-in pool system, you can specify it here")]
		public AudioSource RecycleAudioSource;

		// Token: 0x040015DA RID: 5594
		[Tooltip("whether or not this sound should loop")]
		public bool Loop;

		// Token: 0x040015DB RID: 5595
		[Tooltip("whether or not this sound should continue playing when transitioning to another scene")]
		public bool Persistent;

		// Token: 0x040015DC RID: 5596
		[Tooltip("whether or not this sound should play if the same sound clip is already playing")]
		public bool DoNotPlayIfClipAlreadyPlaying;

		// Token: 0x040015DD RID: 5597
		[Tooltip("if this is true, this sound will stop playing when stopping the feedback")]
		public bool StopSoundOnFeedbackStop;

		// Token: 0x040015DE RID: 5598
		[Header("Fade")]
		[Tooltip("whether or not to fade this sound in when playing it")]
		public bool Fade;

		// Token: 0x040015DF RID: 5599
		[Tooltip("if fading, the volume at which to start the fade")]
		[MMCondition("Fade", true)]
		public float FadeInitialVolume;

		// Token: 0x040015E0 RID: 5600
		[Tooltip("if fading, the duration of the fade, in seconds")]
		[MMCondition("Fade", true)]
		public float FadeDuration = 1f;

		// Token: 0x040015E1 RID: 5601
		[Tooltip("if fading, the tween over which to fade the sound ")]
		[MMCondition("Fade", true)]
		public MMTweenType FadeTween = new MMTweenType(MMTween.MMTweenCurve.EaseInOutQuartic);

		// Token: 0x040015E2 RID: 5602
		[Header("Solo")]
		[Tooltip("whether or not this sound should play in solo mode over its destination track. If yes, all other sounds on that track will be muted when this sound starts playing")]
		public bool SoloSingleTrack;

		// Token: 0x040015E3 RID: 5603
		[Tooltip("whether or not this sound should play in solo mode over all other tracks. If yes, all other tracks will be muted when this sound starts playing")]
		public bool SoloAllTracks;

		// Token: 0x040015E4 RID: 5604
		[Tooltip("if in any of the above solo modes, AutoUnSoloOnEnd will unmute the track(s) automatically once that sound stops playing")]
		public bool AutoUnSoloOnEnd;

		// Token: 0x040015E5 RID: 5605
		[Header("Spatial Settings")]
		[Tooltip("Pans a playing sound in a stereo way (left or right). This only applies to sounds that are Mono or Stereo.")]
		[Range(-1f, 1f)]
		public float PanStereo;

		// Token: 0x040015E6 RID: 5606
		[Tooltip("Sets how much this AudioSource is affected by 3D spatialisation calculations (attenuation, doppler etc). 0.0 makes the sound full 2D, 1.0 makes it full 3D.")]
		[Range(0f, 1f)]
		public float SpatialBlend;

		// Token: 0x040015E7 RID: 5607
		[Tooltip("a Transform this sound can 'attach' to and follow it along as it plays")]
		public Transform AttachToTransform;

		// Token: 0x040015E8 RID: 5608
		[Header("Effects")]
		[Tooltip("Bypass effects (Applied from filter components or global listener filters).")]
		public bool BypassEffects;

		// Token: 0x040015E9 RID: 5609
		[Tooltip("When set global effects on the AudioListener will not be applied to the audio signal generated by the AudioSource. Does not apply if the AudioSource is playing into a mixer group.")]
		public bool BypassListenerEffects;

		// Token: 0x040015EA RID: 5610
		[Tooltip("When set doesn't route the signal from an AudioSource into the global reverb associated with reverb zones.")]
		public bool BypassReverbZones;

		// Token: 0x040015EB RID: 5611
		[Tooltip("Sets the priority of the AudioSource.")]
		[Range(0f, 256f)]
		public int Priority = 128;

		// Token: 0x040015EC RID: 5612
		[Tooltip("The amount by which the signal from the AudioSource will be mixed into the global reverb associated with the Reverb Zones.")]
		[Range(0f, 1.1f)]
		public float ReverbZoneMix = 1f;

		// Token: 0x040015ED RID: 5613
		[Header("3D Sound Settings")]
		[Tooltip("Sets the Doppler scale for this AudioSource.")]
		[Range(0f, 5f)]
		public float DopplerLevel = 1f;

		// Token: 0x040015EE RID: 5614
		[Tooltip("Sets the spread angle (in degrees) of a 3d stereo or multichannel sound in speaker space.")]
		[Range(0f, 360f)]
		public int Spread;

		// Token: 0x040015EF RID: 5615
		[Tooltip("Sets/Gets how the AudioSource attenuates over distance.")]
		public AudioRolloffMode RolloffMode;

		// Token: 0x040015F0 RID: 5616
		[Tooltip("Within the Min distance the AudioSource will cease to grow louder in volume.")]
		public float MinDistance = 1f;

		// Token: 0x040015F1 RID: 5617
		[Tooltip("(Logarithmic rolloff) MaxDistance is the distance a sound stops attenuating at.")]
		public float MaxDistance = 500f;

		// Token: 0x040015F2 RID: 5618
		[Tooltip("whether or not to use a custom curve for custom volume rolloff")]
		public bool UseCustomRolloffCurve;

		// Token: 0x040015F3 RID: 5619
		[Tooltip("the curve to use for custom volume rolloff if UseCustomRolloffCurve is true")]
		[MMCondition("UseCustomRolloffCurve", true)]
		public AnimationCurve CustomRolloffCurve;

		// Token: 0x040015F4 RID: 5620
		[Tooltip("whether or not to use a custom curve for spatial blend")]
		public bool UseSpatialBlendCurve;

		// Token: 0x040015F5 RID: 5621
		[Tooltip("the curve to use for custom spatial blend if UseSpatialBlendCurve is true")]
		[MMCondition("UseSpatialBlendCurve", true)]
		public AnimationCurve SpatialBlendCurve;

		// Token: 0x040015F6 RID: 5622
		[Tooltip("whether or not to use a custom curve for reverb zone mix")]
		public bool UseReverbZoneMixCurve;

		// Token: 0x040015F7 RID: 5623
		[Tooltip("the curve to use for custom reverb zone mix if UseReverbZoneMixCurve is true")]
		[MMCondition("UseReverbZoneMixCurve", true)]
		public AnimationCurve ReverbZoneMixCurve;

		// Token: 0x040015F8 RID: 5624
		[Tooltip("whether or not to use a custom curve for spread")]
		public bool UseSpreadCurve;

		// Token: 0x040015F9 RID: 5625
		[Tooltip("the curve to use for custom spread if UseSpreadCurve is true")]
		[MMCondition("UseSpreadCurve", true)]
		public AnimationCurve SpreadCurve;
	}
}
