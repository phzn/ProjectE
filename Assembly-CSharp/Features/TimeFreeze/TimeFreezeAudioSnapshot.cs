using System;
using System.Collections.Generic;
using Features.PlayerEmotes;
using ProjectExhibitionist.Core;
using UnityEngine;

namespace Features.TimeFreeze
{
	// Token: 0x02000C10 RID: 3088
	internal sealed class TimeFreezeAudioSnapshot : IDisposable
	{
		// Token: 0x17000EBA RID: 3770
		// (get) Token: 0x06005955 RID: 22869 RVA: 0x0022EBF5 File Offset: 0x0022CDF5
		public bool IsFrozen
		{
			get
			{
				return this._frozen;
			}
		}

		// Token: 0x17000EBB RID: 3771
		// (get) Token: 0x06005956 RID: 22870 RVA: 0x0022EBFD File Offset: 0x0022CDFD
		public bool IsThawing
		{
			get
			{
				return this._thawing;
			}
		}

		// Token: 0x06005957 RID: 22871 RVA: 0x0022EC08 File Offset: 0x0022CE08
		public TimeFreezeAudioSnapshot(Transform playerRoot, float minimumPitch, Func<AudioSource, bool> additionalLiveSource = null)
		{
			Animator animator = ((playerRoot != null) ? playerRoot.GetComponentInParent<Animator>(true) : null);
			this._playerRoot = ((animator != null) ? animator.transform : playerRoot);
			this._minimumPitch = Mathf.Clamp(minimumPitch, 0.01f, 0.25f);
			this._additionalLiveSource = additionalLiveSource;
		}

		// Token: 0x06005958 RID: 22872 RVA: 0x0022ECA9 File Offset: 0x0022CEA9
		public void Capture()
		{
			if (this._disposed || this._captured)
			{
				return;
			}
			this._captured = true;
			this._listenerWasPaused = AudioListener.pause;
			this.DiscoverSources(false);
		}

		// Token: 0x06005959 RID: 22873 RVA: 0x0022ECD8 File Offset: 0x0022CED8
		public void SetWorldPitchFactor(float factor)
		{
			if (this._disposed)
			{
				return;
			}
			if (!this._captured)
			{
				this.Capture();
			}
			this._pitchFactor = Mathf.Lerp(this._minimumPitch, 1f, Mathf.Clamp01(factor));
			for (int i = 0; i < this._worldSources.Count; i++)
			{
				TimeFreezeAudioSnapshot.WorldSourceState worldSourceState = this._worldSources[i];
				if (worldSourceState.source != null)
				{
					worldSourceState.source.pitch = worldSourceState.pitch * this._pitchFactor;
				}
			}
		}

		// Token: 0x0600595A RID: 22874 RVA: 0x0022ED64 File Offset: 0x0022CF64
		public void Freeze()
		{
			if (this._disposed || this._frozen)
			{
				return;
			}
			if (!this._captured)
			{
				this.Capture();
			}
			this._thawing = false;
			this._hardFreezeStarted = true;
			this._playbackRestored = false;
			this.DiscoverSources(false);
			this.CaptureFreezeFramePlaybackState();
			this._frozen = true;
			this.EnsureRunner();
			for (int i = 0; i < this._liveSources.Count; i++)
			{
				AudioSource source = this._liveSources[i].source;
				if (source != null)
				{
					source.ignoreListenerPause = true;
				}
			}
			this.ScrubWorldSources();
			AudioListener.pause = true;
			this._nextDiscoveryTime = Time.realtimeSinceStartup + 0.5f;
		}

		// Token: 0x0600595B RID: 22875 RVA: 0x0022EE14 File Offset: 0x0022D014
		public void Thaw(float fadeDuration = 0.25f)
		{
			if (this._disposed || !this._frozen)
			{
				return;
			}
			this.DiscoverSources(true);
			this.ScrubWorldSources();
			this._frozen = false;
			this.RestoreLiveSources();
			this.PrepareWorldSourcesForResume(fadeDuration);
			this._playbackRestored = true;
			this.RestoreListenerPause();
			this._fadeDuration = Mathf.Max(0f, fadeDuration);
			this._fadeStartedAt = Time.realtimeSinceStartup;
			this._thawing = this._fadeDuration > 0.001f && this.HasWorldSourceToFade();
			if (!this._thawing)
			{
				this.RestoreWorldVolumes(1f);
				this.ReleaseRunner();
			}
		}

		// Token: 0x0600595C RID: 22876 RVA: 0x0022EEB4 File Offset: 0x0022D0B4
		public void RestoreImmediate()
		{
			if (this._disposed)
			{
				return;
			}
			if (!this._hardFreezeStarted)
			{
				this._pitchFactor = 1f;
				this.RestoreWorldPitches();
				this.RestoreListenerPause();
				this.ReleaseRunner();
				return;
			}
			if (this._playbackRestored && !this._frozen)
			{
				this._thawing = false;
				this._pitchFactor = 1f;
				this.RestoreLiveSources();
				this.RestoreWorldFinalMixValues();
				this.RestoreListenerPause();
				this.ReleaseRunner();
				return;
			}
			if (this._frozen)
			{
				this.DiscoverSources(true);
				this.ScrubWorldSources();
			}
			this._frozen = false;
			this._thawing = false;
			this._pitchFactor = 1f;
			this.RestoreLiveSources();
			this.RestoreWorldSources(true, false);
			this._playbackRestored = true;
			this.RestoreListenerPause();
			this.ReleaseRunner();
		}

		// Token: 0x0600595D RID: 22877 RVA: 0x0022EF7C File Offset: 0x0022D17C
		public void Dispose()
		{
			if (this._disposed)
			{
				return;
			}
			this.RestoreImmediate();
			this._disposed = true;
			this._worldSources.Clear();
			this._liveSources.Clear();
			this._worldById.Clear();
			this._liveById.Clear();
		}

		// Token: 0x0600595E RID: 22878 RVA: 0x0022EFCC File Offset: 0x0022D1CC
		internal void GuardLateTick()
		{
			if (this._disposed)
			{
				return;
			}
			if (this._frozen)
			{
				float realtimeSinceStartup = Time.realtimeSinceStartup;
				if (realtimeSinceStartup >= this._nextDiscoveryTime)
				{
					this.DiscoverSources(true);
					this.ScrubWorldSources();
					this._nextDiscoveryTime = realtimeSinceStartup + 0.5f;
				}
				for (int i = 0; i < this._liveSources.Count; i++)
				{
					AudioSource source = this._liveSources[i].source;
					if (source != null && !source.ignoreListenerPause)
					{
						source.ignoreListenerPause = true;
					}
				}
				if (!AudioListener.pause)
				{
					AudioListener.pause = true;
				}
				return;
			}
			if (!this._thawing)
			{
				return;
			}
			float num = Time.realtimeSinceStartup - this._fadeStartedAt;
			float num2 = ((this._fadeDuration > 0f) ? Mathf.Clamp01(num / this._fadeDuration) : 1f);
			float num3 = num2 * num2 * (3f - 2f * num2);
			this.RestoreWorldVolumes(num3);
			if (num2 < 1f)
			{
				return;
			}
			this._thawing = false;
			this.ReleaseRunner();
		}

		// Token: 0x0600595F RID: 22879 RVA: 0x0022F0D4 File Offset: 0x0022D2D4
		private void DiscoverSources(bool discoveredDuringFreeze)
		{
			foreach (AudioSource audioSource in Object.FindObjectsByType<AudioSource>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
			{
				if (!(audioSource == null) && audioSource.gameObject.scene.IsValid())
				{
					int instanceID = audioSource.GetInstanceID();
					if (!this._worldById.ContainsKey(instanceID) && !this._liveById.ContainsKey(instanceID))
					{
						if (this.ShouldStayLive(audioSource))
						{
							TimeFreezeAudioSnapshot.LiveSourceState liveSourceState = new TimeFreezeAudioSnapshot.LiveSourceState
							{
								source = audioSource,
								ignoreListenerPause = audioSource.ignoreListenerPause
							};
							this._liveSources.Add(liveSourceState);
							this._liveById.Add(instanceID, liveSourceState);
							if (this._frozen)
							{
								audioSource.ignoreListenerPause = true;
							}
						}
						else
						{
							TimeFreezeAudioSnapshot.WorldSourceState worldSourceState = new TimeFreezeAudioSnapshot.WorldSourceState
							{
								source = audioSource,
								clip = audioSource.clip,
								timeSamples = TimeFreezeAudioSnapshot.SafeGetTimeSamples(audioSource),
								volume = audioSource.volume,
								pitch = audioSource.pitch,
								mute = audioSource.mute,
								loop = audioSource.loop,
								ignoreListenerPause = audioSource.ignoreListenerPause,
								wasPlaying = (!discoveredDuringFreeze && audioSource.isPlaying),
								discoveredDuringFreeze = discoveredDuringFreeze
							};
							this._worldSources.Add(worldSourceState);
							this._worldById.Add(instanceID, worldSourceState);
							if (this._frozen)
							{
								TimeFreezeAudioSnapshot.ScrubWorldSource(worldSourceState);
							}
						}
					}
				}
			}
		}

		// Token: 0x06005960 RID: 22880 RVA: 0x0022F24C File Offset: 0x0022D44C
		private bool ShouldStayLive(AudioSource source)
		{
			Transform transform = source.transform;
			if (this.IsUnderPlayer(transform))
			{
				return true;
			}
			if (source.GetComponentInParent<GameplayEmoteController>(true) != null || source.GetComponentInParent<EmotePropHandler>(true) != null)
			{
				return true;
			}
			Canvas componentInParent = transform.GetComponentInParent<Canvas>(true);
			return (componentInParent != null && componentInParent.renderMode != RenderMode.WorldSpace) || (this._additionalLiveSource != null && this._additionalLiveSource(source));
		}

		// Token: 0x06005961 RID: 22881 RVA: 0x0022F2BD File Offset: 0x0022D4BD
		private bool IsUnderPlayer(Transform target)
		{
			return target != null && this._playerRoot != null && (target == this._playerRoot || target.IsChildOf(this._playerRoot));
		}

		// Token: 0x06005962 RID: 22882 RVA: 0x0022F2F4 File Offset: 0x0022D4F4
		private void ScrubWorldSources()
		{
			for (int i = 0; i < this._worldSources.Count; i++)
			{
				TimeFreezeAudioSnapshot.ScrubWorldSource(this._worldSources[i]);
			}
		}

		// Token: 0x06005963 RID: 22883 RVA: 0x0022F328 File Offset: 0x0022D528
		private void CaptureFreezeFramePlaybackState()
		{
			for (int i = 0; i < this._worldSources.Count; i++)
			{
				TimeFreezeAudioSnapshot.WorldSourceState worldSourceState = this._worldSources[i];
				AudioSource source = worldSourceState.source;
				if (!(source == null))
				{
					worldSourceState.clip = source.clip;
					worldSourceState.timeSamples = TimeFreezeAudioSnapshot.SafeGetTimeSamples(source);
					worldSourceState.volume = source.volume;
					worldSourceState.mute = source.mute;
					worldSourceState.loop = source.loop;
					worldSourceState.ignoreListenerPause = source.ignoreListenerPause;
					worldSourceState.wasPlaying = source.isPlaying;
				}
			}
		}

		// Token: 0x06005964 RID: 22884 RVA: 0x0022F3C0 File Offset: 0x0022D5C0
		private static void ScrubWorldSource(TimeFreezeAudioSnapshot.WorldSourceState state)
		{
			AudioSource source = state.source;
			if (source == null)
			{
				return;
			}
			source.Stop();
			if (!source.mute)
			{
				source.mute = true;
			}
			if (!Mathf.Approximately(source.volume, 0f))
			{
				source.volume = 0f;
			}
			if (source.ignoreListenerPause)
			{
				source.ignoreListenerPause = false;
			}
		}

		// Token: 0x06005965 RID: 22885 RVA: 0x0022F41F File Offset: 0x0022D61F
		private void PrepareWorldSourcesForResume(float fadeDuration)
		{
			this.RestoreWorldSources(true, fadeDuration > 0.001f && !this._listenerWasPaused);
		}

		// Token: 0x06005966 RID: 22886 RVA: 0x0022F43C File Offset: 0x0022D63C
		private void RestoreWorldSources(bool resumePlayback, bool fadeFromSilence)
		{
			for (int i = 0; i < this._worldSources.Count; i++)
			{
				TimeFreezeAudioSnapshot.WorldSourceState worldSourceState = this._worldSources[i];
				AudioSource source = worldSourceState.source;
				if (!(source == null))
				{
					source.Stop();
					source.clip = worldSourceState.clip;
					source.loop = worldSourceState.loop;
					source.pitch = worldSourceState.pitch * this._pitchFactor;
					source.ignoreListenerPause = worldSourceState.ignoreListenerPause;
					source.mute = worldSourceState.mute;
					source.volume = ((fadeFromSilence && !worldSourceState.mute) ? 0f : worldSourceState.volume);
					TimeFreezeAudioSnapshot.SafeSetTimeSamples(source, worldSourceState.clip, worldSourceState.timeSamples);
					if (resumePlayback && !worldSourceState.discoveredDuringFreeze && worldSourceState.wasPlaying && worldSourceState.clip != null && source.enabled && source.gameObject.activeInHierarchy)
					{
						source.Play();
						TimeFreezeAudioSnapshot.SafeSetTimeSamples(source, worldSourceState.clip, worldSourceState.timeSamples);
					}
				}
			}
		}

		// Token: 0x06005967 RID: 22887 RVA: 0x0022F550 File Offset: 0x0022D750
		private void RestoreLiveSources()
		{
			for (int i = 0; i < this._liveSources.Count; i++)
			{
				TimeFreezeAudioSnapshot.LiveSourceState liveSourceState = this._liveSources[i];
				if (liveSourceState.source != null)
				{
					liveSourceState.source.ignoreListenerPause = liveSourceState.ignoreListenerPause;
				}
			}
		}

		// Token: 0x06005968 RID: 22888 RVA: 0x0022F5A0 File Offset: 0x0022D7A0
		private bool HasWorldSourceToFade()
		{
			for (int i = 0; i < this._worldSources.Count; i++)
			{
				TimeFreezeAudioSnapshot.WorldSourceState worldSourceState = this._worldSources[i];
				if (worldSourceState.source != null && !worldSourceState.mute && worldSourceState.volume > 0f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005969 RID: 22889 RVA: 0x0022F5F8 File Offset: 0x0022D7F8
		private void RestoreWorldVolumes(float factor)
		{
			factor = Mathf.Clamp01(factor);
			for (int i = 0; i < this._worldSources.Count; i++)
			{
				TimeFreezeAudioSnapshot.WorldSourceState worldSourceState = this._worldSources[i];
				if (worldSourceState.source != null)
				{
					worldSourceState.source.volume = worldSourceState.volume * factor;
				}
			}
		}

		// Token: 0x0600596A RID: 22890 RVA: 0x0022F654 File Offset: 0x0022D854
		private void RestoreWorldPitches()
		{
			for (int i = 0; i < this._worldSources.Count; i++)
			{
				TimeFreezeAudioSnapshot.WorldSourceState worldSourceState = this._worldSources[i];
				if (worldSourceState.source != null)
				{
					worldSourceState.source.pitch = worldSourceState.pitch;
				}
			}
		}

		// Token: 0x0600596B RID: 22891 RVA: 0x0022F6A4 File Offset: 0x0022D8A4
		private void RestoreWorldFinalMixValues()
		{
			for (int i = 0; i < this._worldSources.Count; i++)
			{
				TimeFreezeAudioSnapshot.WorldSourceState worldSourceState = this._worldSources[i];
				AudioSource source = worldSourceState.source;
				if (!(source == null))
				{
					source.pitch = worldSourceState.pitch;
					source.volume = worldSourceState.volume;
					source.mute = worldSourceState.mute;
					source.ignoreListenerPause = worldSourceState.ignoreListenerPause;
				}
			}
		}

		// Token: 0x0600596C RID: 22892 RVA: 0x0022F714 File Offset: 0x0022D914
		private void EnsureRunner()
		{
			if (this._runner != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("[TimeFreeze Audio Guard]")
			{
				hideFlags = HideFlags.HideAndDontSave
			};
			this._runner = gameObject.AddComponent<TimeFreezeAudioGuardRunner>();
			this._runner.Bind(this);
		}

		// Token: 0x0600596D RID: 22893 RVA: 0x0022F75C File Offset: 0x0022D95C
		private void ReleaseRunner()
		{
			if (this._runner == null)
			{
				return;
			}
			TimeFreezeAudioGuardRunner runner = this._runner;
			this._runner = null;
			runner.Detach();
			if (Application.isPlaying)
			{
				Object.Destroy(runner.gameObject);
				return;
			}
			Object.DestroyImmediate(runner.gameObject);
		}

		// Token: 0x0600596E RID: 22894 RVA: 0x0022F7AC File Offset: 0x0022D9AC
		internal void OnGuardRunnerDestroyed(TimeFreezeAudioGuardRunner runner)
		{
			if (this._runner != runner)
			{
				return;
			}
			this._runner = null;
			if (!this._disposed && (this._frozen || this._thawing))
			{
				bool flag = this._playbackRestored && !this._frozen;
				this._frozen = false;
				this._thawing = false;
				this._pitchFactor = 1f;
				this.RestoreLiveSources();
				if (flag)
				{
					this.RestoreWorldFinalMixValues();
				}
				else
				{
					this.RestoreWorldSources(false, false);
				}
				this.RestoreListenerPause();
			}
		}

		// Token: 0x0600596F RID: 22895 RVA: 0x0022F832 File Offset: 0x0022DA32
		private void RestoreListenerPause()
		{
			AudioListener.pause = GameplayAudioPause.ResolveAfterExternalRestore(this._listenerWasPaused);
		}

		// Token: 0x06005970 RID: 22896 RVA: 0x0022F844 File Offset: 0x0022DA44
		private static int SafeGetTimeSamples(AudioSource source)
		{
			if (source == null || source.clip == null)
			{
				return 0;
			}
			int num;
			try
			{
				num = Mathf.Max(0, source.timeSamples);
			}
			catch (UnityException)
			{
				num = 0;
			}
			return num;
		}

		// Token: 0x06005971 RID: 22897 RVA: 0x0022F890 File Offset: 0x0022DA90
		private static void SafeSetTimeSamples(AudioSource source, AudioClip clip, int sample)
		{
			if (source == null || clip == null || clip.samples <= 0)
			{
				return;
			}
			try
			{
				source.timeSamples = Mathf.Clamp(sample, 0, clip.samples - 1);
			}
			catch (UnityException)
			{
			}
		}

		// Token: 0x040059F5 RID: 23029
		private const float FrozenMaintenanceInterval = 0.5f;

		// Token: 0x040059F6 RID: 23030
		private readonly Transform _playerRoot;

		// Token: 0x040059F7 RID: 23031
		private readonly Func<AudioSource, bool> _additionalLiveSource;

		// Token: 0x040059F8 RID: 23032
		private readonly float _minimumPitch;

		// Token: 0x040059F9 RID: 23033
		private readonly List<TimeFreezeAudioSnapshot.WorldSourceState> _worldSources = new List<TimeFreezeAudioSnapshot.WorldSourceState>(128);

		// Token: 0x040059FA RID: 23034
		private readonly List<TimeFreezeAudioSnapshot.LiveSourceState> _liveSources = new List<TimeFreezeAudioSnapshot.LiveSourceState>(32);

		// Token: 0x040059FB RID: 23035
		private readonly Dictionary<int, TimeFreezeAudioSnapshot.WorldSourceState> _worldById = new Dictionary<int, TimeFreezeAudioSnapshot.WorldSourceState>(128);

		// Token: 0x040059FC RID: 23036
		private readonly Dictionary<int, TimeFreezeAudioSnapshot.LiveSourceState> _liveById = new Dictionary<int, TimeFreezeAudioSnapshot.LiveSourceState>(32);

		// Token: 0x040059FD RID: 23037
		private TimeFreezeAudioGuardRunner _runner;

		// Token: 0x040059FE RID: 23038
		private bool _captured;

		// Token: 0x040059FF RID: 23039
		private bool _frozen;

		// Token: 0x04005A00 RID: 23040
		private bool _thawing;

		// Token: 0x04005A01 RID: 23041
		private bool _hardFreezeStarted;

		// Token: 0x04005A02 RID: 23042
		private bool _playbackRestored;

		// Token: 0x04005A03 RID: 23043
		private bool _disposed;

		// Token: 0x04005A04 RID: 23044
		private bool _listenerWasPaused;

		// Token: 0x04005A05 RID: 23045
		private float _pitchFactor = 1f;

		// Token: 0x04005A06 RID: 23046
		private float _nextDiscoveryTime;

		// Token: 0x04005A07 RID: 23047
		private float _fadeStartedAt;

		// Token: 0x04005A08 RID: 23048
		private float _fadeDuration;

		// Token: 0x02000C11 RID: 3089
		private sealed class WorldSourceState
		{
			// Token: 0x04005A09 RID: 23049
			public AudioSource source;

			// Token: 0x04005A0A RID: 23050
			public AudioClip clip;

			// Token: 0x04005A0B RID: 23051
			public int timeSamples;

			// Token: 0x04005A0C RID: 23052
			public float volume;

			// Token: 0x04005A0D RID: 23053
			public float pitch;

			// Token: 0x04005A0E RID: 23054
			public bool mute;

			// Token: 0x04005A0F RID: 23055
			public bool loop;

			// Token: 0x04005A10 RID: 23056
			public bool ignoreListenerPause;

			// Token: 0x04005A11 RID: 23057
			public bool wasPlaying;

			// Token: 0x04005A12 RID: 23058
			public bool discoveredDuringFreeze;
		}

		// Token: 0x02000C12 RID: 3090
		private sealed class LiveSourceState
		{
			// Token: 0x04005A13 RID: 23059
			public AudioSource source;

			// Token: 0x04005A14 RID: 23060
			public bool ignoreListenerPause;
		}
	}
}
