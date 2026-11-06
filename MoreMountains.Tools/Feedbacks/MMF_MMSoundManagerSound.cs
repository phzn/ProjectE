using System;
using System.Threading.Tasks;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002A3 RID: 675
	[ExecuteAlways]
	[AddComponentMenu("")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.MMTools", null)]
	[FeedbackPath("Audio/MMSoundManager Sound")]
	[FeedbackHelp("This feedback will let you play a sound via the MMSoundManager. You will need a game object in your scene with a MMSoundManager object on it for this to work.")]
	public class MMF_MMSoundManagerSound : MMF_Feedback
	{
		// Token: 0x17000205 RID: 517
		// (get) Token: 0x060012F4 RID: 4852 RVA: 0x0005198B File Offset: 0x0004FB8B
		public override float FeedbackDuration
		{
			get
			{
				return this.GetDuration();
			}
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x060012F5 RID: 4853 RVA: 0x00051993 File Offset: 0x0004FB93
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060012F6 RID: 4854 RVA: 0x00051998 File Offset: 0x0004FB98
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			this.HandleSO();
			this._lastPlayedClip = null;
			if (this.RandomUnique)
			{
				this._randomUniqueShuffleBag = new MMShufflebag<int>(this.RandomSfx.Length);
				for (int i = 0; i < this.RandomSfx.Length; i++)
				{
					this._randomUniqueShuffleBag.Add(i, 1);
				}
			}
		}

		// Token: 0x060012F7 RID: 4855 RVA: 0x000519F4 File Offset: 0x0004FBF4
		public override void InitializeCustomAttributes()
		{
			base.InitializeCustomAttributes();
			this.TestPlayButton = new MMF_Button("Debug Play Sound", new MMF_Button.ButtonMethod(this.TestPlaySound));
			this.TestStopButton = new MMF_Button("Debug Stop Sound", new MMF_Button.ButtonMethod(this.TestStopSound));
			this.ResetSequentialIndexButton = new MMF_Button("Reset Sequential Index", new MMF_Button.ButtonMethod(this.ResetSequentialIndex));
		}

		// Token: 0x060012F8 RID: 4856 RVA: 0x00051A60 File Offset: 0x0004FC60
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_MMSoundManagerSound.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			if (this.RandomSfx.Length != 0)
			{
				this._randomClip = this.PickRandomClip();
				if (this._randomClip != null)
				{
					this.PlaySound(this._randomClip, position, num);
					return;
				}
			}
			if (this.Sfx != null)
			{
				this.PlaySound(this.Sfx, position, num);
				return;
			}
		}

		// Token: 0x060012F9 RID: 4857 RVA: 0x00051AD8 File Offset: 0x0004FCD8
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_MMSoundManagerSound.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this.StopSoundOnFeedbackStop && this._playedAudioSource != null)
			{
				this._playedAudioSource.Stop();
				MMPersistentSingleton<MMSoundManager>.Instance.FreeSound(this._playedAudioSource);
			}
		}

		// Token: 0x060012FA RID: 4858 RVA: 0x00051B28 File Offset: 0x0004FD28
		protected virtual void HandleSO()
		{
			if (this.SoundDataSO == null)
			{
				return;
			}
			this.Sfx = this.SoundDataSO.Sfx;
			this.RandomSfx = this.SoundDataSO.RandomSfx;
			this.SequentialOrder = this.SoundDataSO.SequentialOrder;
			this.SequentialOrderHoldLast = this.SoundDataSO.SequentialOrderHoldLast;
			this.SequentialOrderHoldCooldownDuration = this.SoundDataSO.SequentialOrderHoldCooldownDuration;
			this.RandomUnique = this.SoundDataSO.RandomUnique;
			this.MinVolume = this.SoundDataSO.MinVolume;
			this.MaxVolume = this.SoundDataSO.MaxVolume;
			this.MinPitch = this.SoundDataSO.MinPitch;
			this.MaxPitch = this.SoundDataSO.MaxPitch;
			this.PlaybackTime = this.SoundDataSO.PlaybackTime;
			this.PlaybackDuration = this.SoundDataSO.PlaybackDuration;
			this.MmSoundManagerTrack = this.SoundDataSO.MmSoundManagerTrack;
			this.ID = this.SoundDataSO.ID;
			this.AudioGroup = this.SoundDataSO.AudioGroup;
			this.RecycleAudioSource = this.SoundDataSO.RecycleAudioSource;
			this.Loop = this.SoundDataSO.Loop;
			this.Persistent = this.SoundDataSO.Persistent;
			this.DoNotPlayIfClipAlreadyPlaying = this.SoundDataSO.DoNotPlayIfClipAlreadyPlaying;
			this.StopSoundOnFeedbackStop = this.SoundDataSO.StopSoundOnFeedbackStop;
			this.Fade = this.SoundDataSO.Fade;
			this.FadeInitialVolume = this.SoundDataSO.FadeInitialVolume;
			this.FadeDuration = this.SoundDataSO.FadeDuration;
			this.FadeTween = this.SoundDataSO.FadeTween;
			this.SoloSingleTrack = this.SoundDataSO.SoloSingleTrack;
			this.SoloAllTracks = this.SoundDataSO.SoloAllTracks;
			this.AutoUnSoloOnEnd = this.SoundDataSO.AutoUnSoloOnEnd;
			this.PanStereo = this.SoundDataSO.PanStereo;
			this.SpatialBlend = this.SoundDataSO.SpatialBlend;
			this.AttachToTransform = this.SoundDataSO.AttachToTransform;
			this.BypassEffects = this.SoundDataSO.BypassEffects;
			this.BypassListenerEffects = this.SoundDataSO.BypassListenerEffects;
			this.BypassReverbZones = this.SoundDataSO.BypassReverbZones;
			this.Priority = this.SoundDataSO.Priority;
			this.ReverbZoneMix = this.SoundDataSO.ReverbZoneMix;
			this.DopplerLevel = this.SoundDataSO.DopplerLevel;
			this.Spread = this.SoundDataSO.Spread;
			this.RolloffMode = this.SoundDataSO.RolloffMode;
			this.MinDistance = this.SoundDataSO.MinDistance;
			this.MaxDistance = this.SoundDataSO.MaxDistance;
			this.UseCustomRolloffCurve = this.SoundDataSO.UseCustomRolloffCurve;
			this.CustomRolloffCurve = this.SoundDataSO.CustomRolloffCurve;
			this.UseSpatialBlendCurve = this.SoundDataSO.UseSpatialBlendCurve;
			this.SpatialBlendCurve = this.SoundDataSO.SpatialBlendCurve;
			this.UseReverbZoneMixCurve = this.SoundDataSO.UseReverbZoneMixCurve;
			this.ReverbZoneMixCurve = this.SoundDataSO.ReverbZoneMixCurve;
			this.UseSpreadCurve = this.SoundDataSO.UseSpreadCurve;
			this.SpreadCurve = this.SoundDataSO.SpreadCurve;
		}

		// Token: 0x060012FB RID: 4859 RVA: 0x00051E74 File Offset: 0x00050074
		public virtual void RandomizeTimes()
		{
			this._randomPlaybackTime = Random.Range(this.PlaybackTime.x, this.PlaybackTime.y);
			this._randomPlaybackDuration = Random.Range(this.PlaybackDuration.x, this.PlaybackDuration.y);
			this.Owner.ComputeCachedTotalDuration();
		}

		// Token: 0x060012FC RID: 4860 RVA: 0x00051ED0 File Offset: 0x000500D0
		protected virtual void PlaySound(AudioClip sfx, Vector3 position, float intensity)
		{
			if (this.DoNotPlayIfClipAlreadyPlaying && MMPersistentSingleton<MMSoundManager>.Instance.FindByClip(sfx) != null && MMPersistentSingleton<MMSoundManager>.Instance.FindByClip(sfx).isPlaying)
			{
				return;
			}
			float num = Random.Range(this.MinVolume, this.MaxVolume);
			if (!this.Timing.ConstantIntensity)
			{
				num *= intensity;
			}
			float num2 = Random.Range(this.MinPitch, this.MaxPitch);
			this.RandomizeTimes();
			if (!this.NormalPlayDirection)
			{
				int samples = sfx.samples;
			}
			this._options.MmSoundManagerTrack = this.MmSoundManagerTrack;
			this._options.Location = position;
			this._options.Loop = this.Loop;
			this._options.Volume = num;
			this._options.ID = this.ID;
			this._options.Fade = this.Fade;
			this._options.FadeInitialVolume = this.FadeInitialVolume;
			this._options.FadeDuration = this.FadeDuration;
			this._options.FadeTween = this.FadeTween;
			this._options.Persistent = this.Persistent;
			this._options.RecycleAudioSource = this.RecycleAudioSource;
			this._options.AudioGroup = this.AudioGroup;
			this._options.Pitch = num2;
			this._options.PlaybackTime = this._randomPlaybackTime;
			this._options.PlaybackDuration = this._randomPlaybackDuration;
			this._options.PanStereo = this.PanStereo;
			this._options.SpatialBlend = this.SpatialBlend;
			this._options.SoloSingleTrack = this.SoloSingleTrack;
			this._options.SoloAllTracks = this.SoloAllTracks;
			this._options.AutoUnSoloOnEnd = this.AutoUnSoloOnEnd;
			this._options.BypassEffects = this.BypassEffects;
			this._options.BypassListenerEffects = this.BypassListenerEffects;
			this._options.BypassReverbZones = this.BypassReverbZones;
			this._options.Priority = this.Priority;
			this._options.ReverbZoneMix = this.ReverbZoneMix;
			this._options.DopplerLevel = this.DopplerLevel;
			this._options.Spread = this.Spread;
			this._options.RolloffMode = this.RolloffMode;
			this._options.MinDistance = this.MinDistance;
			this._options.MaxDistance = this.MaxDistance;
			this._options.AttachToTransform = this.AttachToTransform;
			this._options.UseSpreadCurve = this.UseSpreadCurve;
			this._options.SpreadCurve = this.SpreadCurve;
			this._options.UseCustomRolloffCurve = this.UseCustomRolloffCurve;
			this._options.CustomRolloffCurve = this.CustomRolloffCurve;
			this._options.UseSpatialBlendCurve = this.UseSpatialBlendCurve;
			this._options.SpatialBlendCurve = this.SpatialBlendCurve;
			this._options.UseReverbZoneMixCurve = this.UseReverbZoneMixCurve;
			this._options.ReverbZoneMixCurve = this.ReverbZoneMixCurve;
			this._options.DoNotAutoRecycleIfNotDonePlaying = true;
			this._playedAudioSource = MMSoundManagerSoundPlayEvent.Trigger(sfx, this._options);
			this._lastPlayTimestamp = this.FeedbackTime;
			this._lastPlayedClip = sfx;
		}

		// Token: 0x060012FD RID: 4861 RVA: 0x0005220E File Offset: 0x0005040E
		protected virtual float GetDuration()
		{
			if (this.SoundDataSO != null)
			{
				return this.ComputeDuration(this.SoundDataSO.Sfx, this.SoundDataSO.RandomSfx);
			}
			return this.ComputeDuration(this.Sfx, this.RandomSfx);
		}

		// Token: 0x060012FE RID: 4862 RVA: 0x00052250 File Offset: 0x00050450
		protected virtual float ComputeDuration(AudioClip sfx, AudioClip[] randomSfx)
		{
			if (sfx != null)
			{
				if (this._randomPlaybackDuration <= 0f)
				{
					return sfx.length - this._randomPlaybackTime;
				}
				return this._randomPlaybackDuration;
			}
			else
			{
				float num = 0f;
				if (randomSfx == null || randomSfx.Length == 0)
				{
					return 0f;
				}
				if (this._lastPlayedClip != null)
				{
					return this._lastPlayedClip.length;
				}
				foreach (AudioClip audioClip in randomSfx)
				{
					if (audioClip != null && audioClip.length > num)
					{
						num = audioClip.length;
					}
				}
				if (this._randomPlaybackDuration <= 0f)
				{
					return num - this._randomPlaybackTime;
				}
				return this._randomPlaybackDuration;
			}
		}

		// Token: 0x060012FF RID: 4863 RVA: 0x00052300 File Offset: 0x00050500
		public override void OnDrawGizmosSelectedHandler()
		{
			if (!this.DrawGizmos)
			{
				return;
			}
			this._gizmoCenter = ((this.GizmosCenter != null) ? this.GizmosCenter.position : this.Owner.transform.position);
			Gizmos.color = this.MinDistanceColor;
			Gizmos.DrawWireSphere(this._gizmoCenter, this.MinDistance);
			Gizmos.color = this.MaxDistanceColor;
			Gizmos.DrawWireSphere(this._gizmoCenter, this.MaxDistance);
		}

		// Token: 0x06001300 RID: 4864 RVA: 0x0005237F File Offset: 0x0005057F
		public override void AutomaticShakerSetup()
		{
			if ((MMSoundManager)Object.FindObjectOfType(typeof(MMSoundManager)) == null)
			{
				new GameObject("MMSoundManager").AddComponent<MMSoundManager>();
				MMDebug.DebugLogInfo("Added a MMSoundManager to the scene. You're all set.", "", 3, true);
			}
		}

		// Token: 0x06001301 RID: 4865 RVA: 0x000523C0 File Offset: 0x000505C0
		protected virtual async void TestPlaySound()
		{
			AudioClip audioClip = null;
			if (this.Sfx != null)
			{
				audioClip = this.Sfx;
			}
			if (this.RandomSfx != null && this.RandomSfx.Length != 0)
			{
				audioClip = this.PickRandomClip();
			}
			if (audioClip == null)
			{
				Debug.LogError(this.Label + " on " + this.Owner.gameObject.name + " can't play in editor mode, you haven't set its Sfx.");
			}
			else
			{
				float num = Random.Range(this.MinVolume, this.MaxVolume);
				float num2 = Random.Range(this.MinPitch, this.MaxPitch);
				this.RandomizeTimes();
				GameObject temporaryAudioHost = new GameObject("EditorTestAS_WillAutoDestroy");
				SceneManager.MoveGameObjectToScene(temporaryAudioHost.gameObject, this.Owner.gameObject.scene);
				temporaryAudioHost.transform.position = this.Owner.transform.position;
				this._editorAudioSource = temporaryAudioHost.AddComponent<AudioSource>();
				this.PlayAudioSource(this._editorAudioSource, audioClip, num, num2, this._randomPlaybackTime, this._randomPlaybackDuration);
				this._lastPlayTimestamp = this.FeedbackTime;
				this._lastPlayedClip = audioClip;
				await Task.Delay((int)(((this._randomPlaybackDuration > 0f) ? this._randomPlaybackDuration : audioClip.length) * 1000f / Mathf.Abs(num2)));
				Object.DestroyImmediate(temporaryAudioHost);
			}
		}

		// Token: 0x06001302 RID: 4866 RVA: 0x000523F7 File Offset: 0x000505F7
		protected virtual void TestStopSound()
		{
			if (this._editorAudioSource != null)
			{
				this._editorAudioSource.Stop();
			}
		}

		// Token: 0x06001303 RID: 4867 RVA: 0x00052412 File Offset: 0x00050612
		protected virtual void PlayAudioSource(AudioSource audioSource, AudioClip sfx, float volume, float pitch, float time, float playbackDuration)
		{
			audioSource.clip = sfx;
			audioSource.time = time;
			audioSource.volume = volume;
			audioSource.pitch = pitch;
			audioSource.loop = false;
			audioSource.Play();
		}

		// Token: 0x06001304 RID: 4868 RVA: 0x00052440 File Offset: 0x00050640
		protected virtual AudioClip PickRandomClip()
		{
			int num;
			if (!this.SequentialOrder)
			{
				if (this.RandomUnique)
				{
					num = this._randomUniqueShuffleBag.Pick();
				}
				else
				{
					num = Random.Range(0, this.RandomSfx.Length);
				}
			}
			else
			{
				num = this._currentIndex;
				if (num >= this.RandomSfx.Length)
				{
					if (this.SequentialOrderHoldLast)
					{
						num--;
						if (this.SequentialOrderHoldCooldownDuration > 0f && this.FeedbackTime - this._lastPlayTimestamp > this.SequentialOrderHoldCooldownDuration)
						{
							num = 0;
						}
					}
					else
					{
						num = 0;
					}
				}
				this._currentIndex = num + 1;
			}
			return this.RandomSfx[num];
		}

		// Token: 0x06001305 RID: 4869 RVA: 0x000524D5 File Offset: 0x000506D5
		public virtual void ResetSequentialIndex()
		{
			this._currentIndex = 0;
		}

		// Token: 0x06001306 RID: 4870 RVA: 0x000524DE File Offset: 0x000506DE
		public virtual void SetSequentialIndex(int newIndex)
		{
			this._currentIndex = newIndex;
		}

		// Token: 0x06001307 RID: 4871 RVA: 0x000524E8 File Offset: 0x000506E8
		public override void OnValidate()
		{
			base.OnValidate();
			this.RandomizeTimes();
			if (this.RandomSfx != null && this.RandomSfx.Length != 0)
			{
				this._randomUniqueShuffleBag = new MMShufflebag<int>(this.RandomSfx.Length);
				for (int i = 0; i < this.RandomSfx.Length; i++)
				{
					this._randomUniqueShuffleBag.Add(i, 1);
				}
			}
		}

		// Token: 0x04001583 RID: 5507
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001584 RID: 5508
		[MMFInspectorGroup("Sound", true, 14, true, false)]
		[Tooltip("the sound clip to play")]
		public AudioClip Sfx;

		// Token: 0x04001585 RID: 5509
		[MMFInspectorGroup("Random Sound", true, 34, true, false)]
		[Tooltip("an array to pick a random sfx from")]
		public AudioClip[] RandomSfx;

		// Token: 0x04001586 RID: 5510
		[Tooltip("if this is true, random sfx audio clips will be played in sequential order instead of at random")]
		public bool SequentialOrder;

		// Token: 0x04001587 RID: 5511
		[Tooltip("if we're in sequential order, determines whether or not to hold at the last index, until either a cooldown is met, or the ResetSequentialIndex method is called")]
		[MMFCondition("SequentialOrder", true)]
		public bool SequentialOrderHoldLast;

		// Token: 0x04001588 RID: 5512
		[Tooltip("if we're in sequential order hold last mode, index will reset to 0 automatically after this duration, unless it's 0, in which case it'll be ignored")]
		[MMFCondition("SequentialOrderHoldLast", true)]
		public float SequentialOrderHoldCooldownDuration = 2f;

		// Token: 0x04001589 RID: 5513
		[Tooltip("if this is true, sfx will be picked at random until all have been played. once this happens, the list is shuffled again, and it starts over")]
		public bool RandomUnique;

		// Token: 0x0400158A RID: 5514
		[MMFInspectorGroup("Scriptable Object", true, 14, true, false)]
		[Tooltip("a scriptable object (created via the Create/MoreMountains/Audio/MMF_SoundData menu) to define settings that will override all other settings on this feedback")]
		public MMF_MMSoundManagerSoundData SoundDataSO;

		// Token: 0x0400158B RID: 5515
		[MMFInspectorGroup("Sound Properties", true, 24, false, false)]
		[Header("Volume")]
		[Tooltip("the minimum volume to play the sound at")]
		[Range(0f, 2f)]
		public float MinVolume = 1f;

		// Token: 0x0400158C RID: 5516
		[Tooltip("the maximum volume to play the sound at")]
		[Range(0f, 2f)]
		public float MaxVolume = 1f;

		// Token: 0x0400158D RID: 5517
		[Header("Pitch")]
		[Tooltip("the minimum pitch to play the sound at")]
		[Range(-3f, 3f)]
		public float MinPitch = 1f;

		// Token: 0x0400158E RID: 5518
		[Tooltip("the maximum pitch to play the sound at")]
		[Range(-3f, 3f)]
		public float MaxPitch = 1f;

		// Token: 0x0400158F RID: 5519
		[MMFInspectorGroup("SoundManager Options", true, 28, false, false)]
		[Tooltip("the track on which to play the sound. Pick the one that matches the nature of your sound")]
		public MMSoundManager.MMSoundManagerTracks MmSoundManagerTrack;

		// Token: 0x04001590 RID: 5520
		[Tooltip("the ID of the sound. This is useful if you plan on using sound control feedbacks on it afterwards.")]
		public int ID;

		// Token: 0x04001591 RID: 5521
		[Tooltip("the AudioGroup on which to play the sound. If you're already targeting a preset track, you can leave it blank, otherwise the group you specify here will override it.")]
		public AudioMixerGroup AudioGroup;

		// Token: 0x04001592 RID: 5522
		[Tooltip("if (for some reason) you've already got an audiosource and wouldn't like to use the built-in pool system, you can specify it here")]
		public AudioSource RecycleAudioSource;

		// Token: 0x04001593 RID: 5523
		[Tooltip("whether or not this sound should loop")]
		public bool Loop;

		// Token: 0x04001594 RID: 5524
		[Tooltip("whether or not this sound should continue playing when transitioning to another scene")]
		public bool Persistent;

		// Token: 0x04001595 RID: 5525
		[Tooltip("whether or not this sound should play if the same sound clip is already playing")]
		public bool DoNotPlayIfClipAlreadyPlaying;

		// Token: 0x04001596 RID: 5526
		[Tooltip("if this is true, this sound will stop playing when stopping the feedback")]
		public bool StopSoundOnFeedbackStop;

		// Token: 0x04001597 RID: 5527
		[MMFInspectorGroup("Fade", true, 30, false, false)]
		[Tooltip("whether or not to fade this sound in when playing it")]
		public bool Fade;

		// Token: 0x04001598 RID: 5528
		[Tooltip("if fading, the volume at which to start the fade")]
		[MMCondition("Fade", true)]
		public float FadeInitialVolume;

		// Token: 0x04001599 RID: 5529
		[Tooltip("if fading, the duration of the fade, in seconds")]
		[MMCondition("Fade", true)]
		public float FadeDuration = 1f;

		// Token: 0x0400159A RID: 5530
		[Tooltip("if fading, the tween over which to fade the sound ")]
		[MMCondition("Fade", true)]
		public MMTweenType FadeTween = new MMTweenType(MMTween.MMTweenCurve.EaseInOutQuartic);

		// Token: 0x0400159B RID: 5531
		[MMFInspectorGroup("Solo", true, 32, false, false)]
		[Tooltip("whether or not this sound should play in solo mode over its destination track. If yes, all other sounds on that track will be muted when this sound starts playing")]
		public bool SoloSingleTrack;

		// Token: 0x0400159C RID: 5532
		[Tooltip("whether or not this sound should play in solo mode over all other tracks. If yes, all other tracks will be muted when this sound starts playing")]
		public bool SoloAllTracks;

		// Token: 0x0400159D RID: 5533
		[Tooltip("if in any of the above solo modes, AutoUnSoloOnEnd will unmute the track(s) automatically once that sound stops playing")]
		public bool AutoUnSoloOnEnd;

		// Token: 0x0400159E RID: 5534
		[MMFInspectorGroup("Spatial Settings", true, 33, false, false)]
		[Tooltip("Pans a playing sound in a stereo way (left or right). This only applies to sounds that are Mono or Stereo.")]
		[Range(-1f, 1f)]
		public float PanStereo;

		// Token: 0x0400159F RID: 5535
		[Tooltip("Sets how much this AudioSource is affected by 3D spatialisation calculations (attenuation, doppler etc). 0.0 makes the sound full 2D, 1.0 makes it full 3D.")]
		[Range(0f, 1f)]
		public float SpatialBlend;

		// Token: 0x040015A0 RID: 5536
		[Tooltip("a Transform this sound can 'attach' to and follow it along as it plays")]
		public Transform AttachToTransform;

		// Token: 0x040015A1 RID: 5537
		[MMFInspectorGroup("Effects", true, 36, false, false)]
		[Tooltip("Bypass effects (Applied from filter components or global listener filters).")]
		public bool BypassEffects;

		// Token: 0x040015A2 RID: 5538
		[Tooltip("When set global effects on the AudioListener will not be applied to the audio signal generated by the AudioSource. Does not apply if the AudioSource is playing into a mixer group.")]
		public bool BypassListenerEffects;

		// Token: 0x040015A3 RID: 5539
		[Tooltip("When set doesn't route the signal from an AudioSource into the global reverb associated with reverb zones.")]
		public bool BypassReverbZones;

		// Token: 0x040015A4 RID: 5540
		[Tooltip("Sets the priority of the AudioSource.")]
		[Range(0f, 256f)]
		public int Priority = 128;

		// Token: 0x040015A5 RID: 5541
		[Tooltip("The amount by which the signal from the AudioSource will be mixed into the global reverb associated with the Reverb Zones.")]
		[Range(0f, 1.1f)]
		public float ReverbZoneMix = 1f;

		// Token: 0x040015A6 RID: 5542
		[MMFInspectorGroup("Time Options", true, 15, false, false)]
		[Tooltip("a timestamp (in seconds, randomized between the defined min and max) at which the sound will start playing, equivalent to the Audiosource API's Time)")]
		[MMVector(new string[] { "Min", "Max" })]
		public Vector2 PlaybackTime = new Vector2(0f, 0f);

		// Token: 0x040015A7 RID: 5543
		[Tooltip("a duration (in seconds, randomized between the defined min and max) for which the sound will play before stopping. Ignored if min and max are zero.")]
		[MMVector(new string[] { "Min", "Max" })]
		public Vector2 PlaybackDuration = new Vector2(0f, 0f);

		// Token: 0x040015A8 RID: 5544
		[MMFInspectorGroup("3D Sound Settings", true, 37, false, false)]
		[Tooltip("Sets the Doppler scale for this AudioSource.")]
		[Range(0f, 5f)]
		public float DopplerLevel = 1f;

		// Token: 0x040015A9 RID: 5545
		[Tooltip("Sets the spread angle (in degrees) of a 3d stereo or multichannel sound in speaker space.")]
		[Range(0f, 360f)]
		public int Spread;

		// Token: 0x040015AA RID: 5546
		[Tooltip("Sets/Gets how the AudioSource attenuates over distance.")]
		public AudioRolloffMode RolloffMode;

		// Token: 0x040015AB RID: 5547
		[Tooltip("Within the Min distance the AudioSource will cease to grow louder in volume.")]
		public float MinDistance = 1f;

		// Token: 0x040015AC RID: 5548
		[Tooltip("(Logarithmic rolloff) MaxDistance is the distance a sound stops attenuating at.")]
		public float MaxDistance = 500f;

		// Token: 0x040015AD RID: 5549
		[Tooltip("whether or not to use a custom curve for custom volume rolloff")]
		public bool UseCustomRolloffCurve;

		// Token: 0x040015AE RID: 5550
		[Tooltip("the curve to use for custom volume rolloff if UseCustomRolloffCurve is true")]
		[MMFCondition("UseCustomRolloffCurve", true)]
		public AnimationCurve CustomRolloffCurve;

		// Token: 0x040015AF RID: 5551
		[Tooltip("whether or not to use a custom curve for spatial blend")]
		public bool UseSpatialBlendCurve;

		// Token: 0x040015B0 RID: 5552
		[Tooltip("the curve to use for custom spatial blend if UseSpatialBlendCurve is true")]
		[MMFCondition("UseSpatialBlendCurve", true)]
		public AnimationCurve SpatialBlendCurve;

		// Token: 0x040015B1 RID: 5553
		[Tooltip("whether or not to use a custom curve for reverb zone mix")]
		public bool UseReverbZoneMixCurve;

		// Token: 0x040015B2 RID: 5554
		[Tooltip("the curve to use for custom reverb zone mix if UseReverbZoneMixCurve is true")]
		[MMFCondition("UseReverbZoneMixCurve", true)]
		public AnimationCurve ReverbZoneMixCurve;

		// Token: 0x040015B3 RID: 5555
		[Tooltip("whether or not to use a custom curve for spread")]
		public bool UseSpreadCurve;

		// Token: 0x040015B4 RID: 5556
		[Tooltip("the curve to use for custom spread if UseSpreadCurve is true")]
		[MMFCondition("UseSpreadCurve", true)]
		public AnimationCurve SpreadCurve;

		// Token: 0x040015B5 RID: 5557
		[MMFInspectorGroup("Debug", true, 31, false, false)]
		[Tooltip("whether or not to draw sound falloff gizmos when this MMF Player is selected")]
		public bool DrawGizmos;

		// Token: 0x040015B6 RID: 5558
		[Tooltip("an object to use as the center of the gizmos. If left empty, this MMF Player's position will be used.")]
		[MMFCondition("DrawGizmos", true)]
		public Transform GizmosCenter;

		// Token: 0x040015B7 RID: 5559
		[Tooltip("the color to use to draw the min distance sphere of the sound falloff gizmos")]
		[MMFCondition("DrawGizmos", true)]
		public Color MinDistanceColor = MMColors.CadetBlue;

		// Token: 0x040015B8 RID: 5560
		[Tooltip("the color to use to draw the max distance sphere of the sound falloff gizmos")]
		[MMFCondition("DrawGizmos", true)]
		public Color MaxDistanceColor = MMColors.Orangered;

		// Token: 0x040015B9 RID: 5561
		public MMF_Button TestPlayButton;

		// Token: 0x040015BA RID: 5562
		public MMF_Button TestStopButton;

		// Token: 0x040015BB RID: 5563
		public MMF_Button ResetSequentialIndexButton;

		// Token: 0x040015BC RID: 5564
		protected AudioClip _randomClip;

		// Token: 0x040015BD RID: 5565
		protected AudioSource _editorAudioSource;

		// Token: 0x040015BE RID: 5566
		protected MMSoundManagerPlayOptions _options;

		// Token: 0x040015BF RID: 5567
		protected AudioSource _playedAudioSource;

		// Token: 0x040015C0 RID: 5568
		protected float _randomPlaybackTime;

		// Token: 0x040015C1 RID: 5569
		protected float _randomPlaybackDuration;

		// Token: 0x040015C2 RID: 5570
		protected int _currentIndex;

		// Token: 0x040015C3 RID: 5571
		protected Vector3 _gizmoCenter;

		// Token: 0x040015C4 RID: 5572
		protected MMShufflebag<int> _randomUniqueShuffleBag;

		// Token: 0x040015C5 RID: 5573
		protected AudioClip _lastPlayedClip;
	}
}
