using System;
using System.Threading.Tasks;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002B2 RID: 690
	[ExecuteAlways]
	[AddComponentMenu("")]
	[FeedbackPath("Audio/Sound")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.MMTools", null)]
	[FeedbackHelp("WARNING: this is a very simple feedback, that will let you play a sound. Nothing wrong with it being simple of course, but if you want more features, you'll want to look at the MMSoundManager Sound feedback.\n\nThis feedback lets you play the specified AudioClip, either via event (you'll need something in your scene to catch a MMSfxEvent, for example a MMSoundManager), or cached (AudioSource gets created on init, and is then ready to be played), or on demand (instantiated on Play). For all these methods you can define a random volume between min/max boundaries (just set the same value in both fields if you don't want randomness), random pitch, and an optional AudioMixerGroup.")]
	public class MMF_Sound : MMF_Feedback
	{
		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06001361 RID: 4961 RVA: 0x00053BFB File Offset: 0x00051DFB
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06001362 RID: 4962 RVA: 0x00053BFE File Offset: 0x00051DFE
		public override float FeedbackDuration
		{
			get
			{
				return this.GetDuration();
			}
		}

		// Token: 0x06001363 RID: 4963 RVA: 0x00053C08 File Offset: 0x00051E08
		public override void InitializeCustomAttributes()
		{
			base.InitializeCustomAttributes();
			this.TestPlayButton = new MMF_Button("Debug Play Sound", new MMF_Button.ButtonMethod(this.TestPlaySound));
			this.TestStopButton = new MMF_Button("Debug Stop Sound", new MMF_Button.ButtonMethod(this.TestStopSound));
		}

		// Token: 0x06001364 RID: 4964 RVA: 0x00053C58 File Offset: 0x00051E58
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			if (this.PlayMethod == MMF_Sound.PlayMethods.Cached)
			{
				this._cachedAudioSource = this.CreateAudioSource(owner.gameObject, "CachedFeedbackAudioSource");
			}
			this._lastPlayedClip = null;
			if (this.PlayMethod == MMF_Sound.PlayMethods.Pool)
			{
				this._pool = new AudioSource[this.PoolSize];
				for (int i = 0; i < this.PoolSize; i++)
				{
					this._pool[i] = this.CreateAudioSource(owner.gameObject, "PooledAudioSource" + i.ToString());
				}
			}
		}

		// Token: 0x06001365 RID: 4965 RVA: 0x00053CE4 File Offset: 0x00051EE4
		protected virtual AudioSource CreateAudioSource(GameObject owner, string audioSourceName)
		{
			GameObject gameObject = new GameObject(audioSourceName);
			SceneManager.MoveGameObjectToScene(gameObject.gameObject, this.Owner.gameObject.scene);
			gameObject.transform.position = owner.transform.position;
			gameObject.transform.SetParent(owner.transform);
			this._tempAudioSource = gameObject.AddComponent<AudioSource>();
			this._tempAudioSource.playOnAwake = false;
			return this._tempAudioSource;
		}

		// Token: 0x06001366 RID: 4966 RVA: 0x00053D58 File Offset: 0x00051F58
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Sound.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			if (this.Sfx != null)
			{
				this._duration = this.Sfx.length;
				this.PlaySound(this.Sfx, position, num);
				return;
			}
			if (this.RandomSfx.Length != 0)
			{
				this._randomClip = this.RandomSfx[Random.Range(0, this.RandomSfx.Length)];
				if (this._randomClip != null)
				{
					this._duration = this._randomClip.length;
					this.PlaySound(this._randomClip, position, num);
				}
			}
		}

		// Token: 0x06001367 RID: 4967 RVA: 0x00053E00 File Offset: 0x00052000
		protected virtual float GetDuration()
		{
			if (this.Sfx != null)
			{
				return this.Sfx.length;
			}
			float num = 0f;
			if (this.RandomSfx == null || this.RandomSfx.Length == 0)
			{
				return 0f;
			}
			if (this._lastPlayedClip != null)
			{
				return this._lastPlayedClip.length;
			}
			foreach (AudioClip audioClip in this.RandomSfx)
			{
				if (audioClip != null && audioClip.length > num)
				{
					num = audioClip.length;
				}
			}
			return num;
		}

		// Token: 0x06001368 RID: 4968 RVA: 0x00053E94 File Offset: 0x00052094
		protected virtual void PlaySound(AudioClip sfx, Vector3 position, float intensity)
		{
			float num = Random.Range(this.MinVolume, this.MaxVolume);
			if (!this.Timing.ConstantIntensity)
			{
				num *= intensity;
			}
			float num2 = Random.Range(this.MinPitch, this.MaxPitch);
			int num3 = (this.NormalPlayDirection ? 0 : (sfx.samples - 1));
			if (!this.NormalPlayDirection)
			{
				num2 = -num2;
			}
			this._lastPlayedClip = sfx;
			this.Owner.ComputeCachedTotalDuration();
			switch (this.PlayMethod)
			{
			case MMF_Sound.PlayMethods.Event:
			{
				if (this.UseLegacyEventsMode)
				{
					MMSfxEvent.Trigger(sfx, this.SfxAudioMixerGroup, num, num2, this.Priority);
					return;
				}
				MMSoundManagerPlayOptions mmsoundManagerPlayOptions = default(MMSoundManagerPlayOptions);
				mmsoundManagerPlayOptions = MMSoundManagerPlayOptions.Default;
				mmsoundManagerPlayOptions.Location = this.Owner.transform.position;
				mmsoundManagerPlayOptions.AudioGroup = this.SfxAudioMixerGroup;
				mmsoundManagerPlayOptions.DoNotAutoRecycleIfNotDonePlaying = true;
				mmsoundManagerPlayOptions.Volume = num;
				mmsoundManagerPlayOptions.Pitch = num2;
				mmsoundManagerPlayOptions.PanStereo = this.PanStereo;
				mmsoundManagerPlayOptions.SpatialBlend = this.SpatialBlend;
				mmsoundManagerPlayOptions.Priority = this.Priority;
				mmsoundManagerPlayOptions.DopplerLevel = this.DopplerLevel;
				mmsoundManagerPlayOptions.Spread = this.Spread;
				mmsoundManagerPlayOptions.RolloffMode = this.RolloffMode;
				mmsoundManagerPlayOptions.MinDistance = this.MinDistance;
				mmsoundManagerPlayOptions.MaxDistance = this.MaxDistance;
				mmsoundManagerPlayOptions.UseSpreadCurve = this.UseSpreadCurve;
				mmsoundManagerPlayOptions.SpreadCurve = this.SpreadCurve;
				mmsoundManagerPlayOptions.UseCustomRolloffCurve = this.UseCustomRolloffCurve;
				mmsoundManagerPlayOptions.CustomRolloffCurve = this.CustomRolloffCurve;
				mmsoundManagerPlayOptions.UseSpatialBlendCurve = this.UseSpatialBlendCurve;
				mmsoundManagerPlayOptions.SpatialBlendCurve = this.SpatialBlendCurve;
				mmsoundManagerPlayOptions.UseReverbZoneMixCurve = this.UseReverbZoneMixCurve;
				mmsoundManagerPlayOptions.ReverbZoneMixCurve = this.ReverbZoneMixCurve;
				if (this.Priority >= 0)
				{
					mmsoundManagerPlayOptions.Priority = Mathf.Min(this.Priority, 256);
				}
				mmsoundManagerPlayOptions.MmSoundManagerTrack = MMSoundManager.MMSoundManagerTracks.Sfx;
				mmsoundManagerPlayOptions.Loop = false;
				this._audioSource = MMSoundManagerSoundPlayEvent.Trigger(sfx, mmsoundManagerPlayOptions);
				return;
			}
			case MMF_Sound.PlayMethods.Cached:
				this.PlayAudioSource(this._cachedAudioSource, sfx, num, num2, num3, this.SfxAudioMixerGroup, this.Priority);
				return;
			case MMF_Sound.PlayMethods.OnDemand:
			{
				GameObject gameObject = new GameObject("TempAudio");
				SceneManager.MoveGameObjectToScene(gameObject.gameObject, this.Owner.gameObject.scene);
				gameObject.transform.position = position;
				AudioSource audioSource = gameObject.AddComponent<AudioSource>();
				this.PlayAudioSource(audioSource, sfx, num, num2, num3, this.SfxAudioMixerGroup, this.Priority);
				this.Owner.ProxyDestroy(gameObject, sfx.length * Time.timeScale);
				return;
			}
			case MMF_Sound.PlayMethods.Pool:
				this._tempAudioSource = this.GetAudioSourceFromPool();
				if (this._tempAudioSource != null)
				{
					this.PlayAudioSource(this._tempAudioSource, sfx, num, num2, num3, this.SfxAudioMixerGroup, this.Priority);
				}
				return;
			default:
				return;
			}
		}

		// Token: 0x06001369 RID: 4969 RVA: 0x0005415C File Offset: 0x0005235C
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Sound.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this.StopSoundOnFeedbackStop && this._audioSource != null)
			{
				this._audioSource.Stop();
			}
		}

		// Token: 0x0600136A RID: 4970 RVA: 0x00054190 File Offset: 0x00052390
		protected virtual void PlayAudioSource(AudioSource audioSource, AudioClip sfx, float volume, float pitch, int timeSamples, AudioMixerGroup audioMixerGroup = null, int priority = 128)
		{
			this._audioSource = audioSource;
			audioSource.clip = sfx;
			audioSource.timeSamples = timeSamples;
			audioSource.volume = volume;
			audioSource.pitch = pitch;
			audioSource.priority = priority;
			audioSource.panStereo = this.PanStereo;
			audioSource.spatialBlend = this.SpatialBlend;
			audioSource.dopplerLevel = this.DopplerLevel;
			audioSource.spread = (float)this.Spread;
			audioSource.rolloffMode = this.RolloffMode;
			audioSource.minDistance = this.MinDistance;
			audioSource.maxDistance = this.MaxDistance;
			if (this.UseSpreadCurve)
			{
				audioSource.SetCustomCurve(AudioSourceCurveType.Spread, this.SpreadCurve);
			}
			if (this.UseCustomRolloffCurve)
			{
				audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, this.CustomRolloffCurve);
			}
			if (this.UseSpatialBlendCurve)
			{
				audioSource.SetCustomCurve(AudioSourceCurveType.SpatialBlend, this.SpatialBlendCurve);
			}
			if (this.UseReverbZoneMixCurve)
			{
				audioSource.SetCustomCurve(AudioSourceCurveType.ReverbZoneMix, this.ReverbZoneMixCurve);
			}
			audioSource.loop = false;
			if (audioMixerGroup != null)
			{
				audioSource.outputAudioMixerGroup = audioMixerGroup;
			}
			audioSource.Play();
		}

		// Token: 0x0600136B RID: 4971 RVA: 0x00054294 File Offset: 0x00052494
		protected virtual AudioSource GetAudioSourceFromPool()
		{
			for (int i = 0; i < this.PoolSize; i++)
			{
				if (!this._pool[i].isPlaying)
				{
					return this._pool[i];
				}
			}
			return null;
		}

		// Token: 0x0600136C RID: 4972 RVA: 0x000542CC File Offset: 0x000524CC
		protected virtual async void TestPlaySound()
		{
			AudioClip audioClip = null;
			if (this.Sfx != null)
			{
				audioClip = this.Sfx;
			}
			if (this.RandomSfx.Length != 0)
			{
				audioClip = this.RandomSfx[Random.Range(0, this.RandomSfx.Length)];
			}
			if (audioClip == null)
			{
				Debug.LogError(this.Label + " on " + this.Owner.gameObject.name + " can't play in editor mode, you haven't set its Sfx.");
			}
			else
			{
				float num = Random.Range(this.MinVolume, this.MaxVolume);
				float num2 = Random.Range(this.MinPitch, this.MaxPitch);
				GameObject temporaryAudioHost = new GameObject("EditorTestAS_WillAutoDestroy");
				SceneManager.MoveGameObjectToScene(temporaryAudioHost.gameObject, this.Owner.gameObject.scene);
				temporaryAudioHost.transform.position = this.Owner.transform.position;
				this._editorAudioSource = temporaryAudioHost.AddComponent<AudioSource>();
				this.PlayAudioSource(this._editorAudioSource, audioClip, num, num2, 0, null, 128);
				await Task.Delay((int)(1000f * audioClip.length));
				this.Owner.ProxyDestroyImmediate(temporaryAudioHost);
			}
		}

		// Token: 0x0600136D RID: 4973 RVA: 0x00054303 File Offset: 0x00052503
		protected virtual void TestStopSound()
		{
			if (this._editorAudioSource != null)
			{
				this._editorAudioSource.Stop();
			}
		}

		// Token: 0x0600136E RID: 4974 RVA: 0x00054320 File Offset: 0x00052520
		public override void AutomaticShakerSetup()
		{
			if (this.PlayMethod != MMF_Sound.PlayMethods.Event)
			{
				return;
			}
			if ((MMSoundManager)Object.FindObjectOfType(typeof(MMSoundManager)) == null)
			{
				new GameObject("MMSoundManager").AddComponent<MMSoundManager>();
				MMDebug.DebugLogInfo("Added a MMSoundManager to the scene. You're all set.", "", 3, true);
			}
		}

		// Token: 0x0400165D RID: 5725
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x0400165E RID: 5726
		[MMFInspectorGroup("Sound", true, 14, true, false)]
		[Tooltip("the sound clip to play")]
		public AudioClip Sfx;

		// Token: 0x0400165F RID: 5727
		[Tooltip("an array to pick a random sfx from")]
		public AudioClip[] RandomSfx;

		// Token: 0x04001660 RID: 5728
		public MMF_Button TestPlayButton;

		// Token: 0x04001661 RID: 5729
		public MMF_Button TestStopButton;

		// Token: 0x04001662 RID: 5730
		[MMFInspectorGroup("Play Method", true, 27, false, false)]
		[Tooltip("the play method to use when playing the sound (event, cached or on demand)")]
		public MMF_Sound.PlayMethods PlayMethod;

		// Token: 0x04001663 RID: 5731
		[Tooltip("the size of the pool when in Pool mode")]
		[MMFEnumCondition("PlayMethod", new int[] { 3 })]
		public int PoolSize = 10;

		// Token: 0x04001664 RID: 5732
		[Tooltip("in event mode, whether to use legacy events (MMSfxEvent) or the current events (MMSoundManagerSoundPlayEvent)")]
		[MMFEnumCondition("PlayMethod", new int[] { 0 })]
		public bool UseLegacyEventsMode;

		// Token: 0x04001665 RID: 5733
		[Tooltip("if this is true, calling Stop on this feedback will also stop the sound from playing further")]
		public bool StopSoundOnFeedbackStop = true;

		// Token: 0x04001666 RID: 5734
		[MMFInspectorGroup("Sound Properties", true, 28, false, false)]
		[Header("Volume")]
		[Tooltip("the minimum volume to play the sound at")]
		[Range(0f, 2f)]
		public float MinVolume = 1f;

		// Token: 0x04001667 RID: 5735
		[Tooltip("the maximum volume to play the sound at")]
		[Range(0f, 2f)]
		public float MaxVolume = 1f;

		// Token: 0x04001668 RID: 5736
		[Header("Pitch")]
		[Tooltip("the minimum pitch to play the sound at")]
		[Range(-3f, 3f)]
		public float MinPitch = 1f;

		// Token: 0x04001669 RID: 5737
		[Tooltip("the maximum pitch to play the sound at")]
		[Range(-3f, 3f)]
		public float MaxPitch = 1f;

		// Token: 0x0400166A RID: 5738
		[Header("Mixer")]
		[Tooltip("the audiomixer to play the sound with (optional)")]
		public AudioMixerGroup SfxAudioMixerGroup;

		// Token: 0x0400166B RID: 5739
		[Tooltip("the audiosource priority, to be specified if needed between 0 (highest) and 256")]
		public int Priority = 128;

		// Token: 0x0400166C RID: 5740
		[MMFInspectorGroup("Spatial Settings", true, 33, false, true)]
		[Tooltip("Pans a playing sound in a stereo way (left or right). This only applies to sounds that are Mono or Stereo.")]
		[Range(-1f, 1f)]
		public float PanStereo;

		// Token: 0x0400166D RID: 5741
		[Tooltip("Sets how much this AudioSource is affected by 3D spatialisation calculations (attenuation, doppler etc). 0.0 makes the sound full 2D, 1.0 makes it full 3D.")]
		[Range(0f, 1f)]
		public float SpatialBlend;

		// Token: 0x0400166E RID: 5742
		[MMFInspectorGroup("3D Sound Settings", true, 37, false, true)]
		[Tooltip("Sets the Doppler scale for this AudioSource.")]
		[Range(0f, 5f)]
		public float DopplerLevel = 1f;

		// Token: 0x0400166F RID: 5743
		[Tooltip("Sets the spread angle (in degrees) of a 3d stereo or multichannel sound in speaker space.")]
		[Range(0f, 360f)]
		public int Spread;

		// Token: 0x04001670 RID: 5744
		[Tooltip("Sets/Gets how the AudioSource attenuates over distance.")]
		public AudioRolloffMode RolloffMode;

		// Token: 0x04001671 RID: 5745
		[Tooltip("Within the Min distance the AudioSource will cease to grow louder in volume.")]
		public float MinDistance = 1f;

		// Token: 0x04001672 RID: 5746
		[Tooltip("(Logarithmic rolloff) MaxDistance is the distance a sound stops attenuating at.")]
		public float MaxDistance = 500f;

		// Token: 0x04001673 RID: 5747
		[Tooltip("whether or not to use a custom curve for custom volume rolloff")]
		public bool UseCustomRolloffCurve;

		// Token: 0x04001674 RID: 5748
		[Tooltip("the curve to use for custom volume rolloff if UseCustomRolloffCurve is true")]
		[MMFCondition("UseCustomRolloffCurve", true)]
		public AnimationCurve CustomRolloffCurve;

		// Token: 0x04001675 RID: 5749
		[Tooltip("whether or not to use a custom curve for spatial blend")]
		public bool UseSpatialBlendCurve;

		// Token: 0x04001676 RID: 5750
		[Tooltip("the curve to use for custom spatial blend if UseSpatialBlendCurve is true")]
		[MMFCondition("UseSpatialBlendCurve", true)]
		public AnimationCurve SpatialBlendCurve;

		// Token: 0x04001677 RID: 5751
		[Tooltip("whether or not to use a custom curve for reverb zone mix")]
		public bool UseReverbZoneMixCurve;

		// Token: 0x04001678 RID: 5752
		[Tooltip("the curve to use for custom reverb zone mix if UseReverbZoneMixCurve is true")]
		[MMFCondition("UseReverbZoneMixCurve", true)]
		public AnimationCurve ReverbZoneMixCurve;

		// Token: 0x04001679 RID: 5753
		[Tooltip("whether or not to use a custom curve for spread")]
		public bool UseSpreadCurve;

		// Token: 0x0400167A RID: 5754
		[Tooltip("the curve to use for custom spread if UseSpreadCurve is true")]
		[MMFCondition("UseSpreadCurve", true)]
		public AnimationCurve SpreadCurve;

		// Token: 0x0400167B RID: 5755
		protected AudioClip _randomClip;

		// Token: 0x0400167C RID: 5756
		protected AudioSource _cachedAudioSource;

		// Token: 0x0400167D RID: 5757
		protected AudioSource[] _pool;

		// Token: 0x0400167E RID: 5758
		protected AudioSource _tempAudioSource;

		// Token: 0x0400167F RID: 5759
		protected float _duration;

		// Token: 0x04001680 RID: 5760
		protected AudioSource _editorAudioSource;

		// Token: 0x04001681 RID: 5761
		protected AudioSource _audioSource;

		// Token: 0x04001682 RID: 5762
		protected AudioClip _lastPlayedClip;

		// Token: 0x02000542 RID: 1346
		public enum PlayMethods
		{
			// Token: 0x04002429 RID: 9257
			Event,
			// Token: 0x0400242A RID: 9258
			Cached,
			// Token: 0x0400242B RID: 9259
			OnDemand,
			// Token: 0x0400242C RID: 9260
			Pool
		}
	}
}
