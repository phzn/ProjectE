using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000C7 RID: 199
	public class MMSMPlaylistManager : MMMonoBehaviour
	{
		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000412 RID: 1042 RVA: 0x000134EA File Offset: 0x000116EA
		public virtual bool IsPlaying
		{
			get
			{
				return this._currentlyPlayingAudioSource != null && this._currentlyPlayingAudioSource.isPlaying;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000413 RID: 1043 RVA: 0x00013507 File Offset: 0x00011707
		public static bool HasInstance
		{
			get
			{
				return MMSMPlaylistManager._instance != null;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000414 RID: 1044 RVA: 0x00013514 File Offset: 0x00011714
		public static MMSMPlaylistManager Current
		{
			get
			{
				return MMSMPlaylistManager._instance;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000415 RID: 1045 RVA: 0x0001351C File Offset: 0x0001171C
		public static MMSMPlaylistManager Instance
		{
			get
			{
				if (MMSMPlaylistManager._instance == null)
				{
					MMSMPlaylistManager._instance = Object.FindObjectOfType<MMSMPlaylistManager>();
					if (MMSMPlaylistManager._instance == null)
					{
						MMSMPlaylistManager._instance = new GameObject
						{
							name = typeof(MMPlaylist).Name + "_AutoCreated"
						}.AddComponent<MMSMPlaylistManager>();
					}
				}
				return MMSMPlaylistManager._instance;
			}
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x00013580 File Offset: 0x00011780
		protected virtual void Awake()
		{
			this.InitializeSingleton();
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x00013588 File Offset: 0x00011788
		protected virtual void InitializeSingleton()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			if (!this.Persistent)
			{
				return;
			}
			if (this.AutomaticallyUnparentOnAwake)
			{
				base.transform.SetParent(null);
			}
			if (MMSMPlaylistManager._instance == null)
			{
				MMSMPlaylistManager._instance = this;
				Object.DontDestroyOnLoad(base.transform.gameObject);
				return;
			}
			if (this != MMSMPlaylistManager._instance)
			{
				Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x000135F6 File Offset: 0x000117F6
		protected virtual void Start()
		{
			this.Initialization();
			if (this.PlayOnStart)
			{
				this.PlayFirstSong();
			}
			if (!this._listeningToEvents)
			{
				this.StartListening();
			}
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x0001361A File Offset: 0x0001181A
		protected virtual void Initialization()
		{
			this.InitializeRandomSeed();
			this.Playlist.Initialization();
			this.InitializePlaylistManagerState();
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x00013633 File Offset: 0x00011833
		protected virtual void InitializeRandomSeed()
		{
			if ((this.Playlist.PlayOrder == MMSMPlaylist.PlayOrders.Random || this.Playlist.PlayOrder == MMSMPlaylist.PlayOrders.RandomUnique) && this.Playlist.RandomizeOrderSeed)
			{
				Random.InitState(Environment.TickCount);
			}
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x00013668 File Offset: 0x00011868
		protected virtual void InitializePlaylistManagerState()
		{
			this.PlaylistManagerState = new MMStateMachine<MMSMPlaylistManager.PlaylistManagerStates>(base.gameObject, true);
			this.ChangePlaylistManagerState(MMSMPlaylistManager.PlaylistManagerStates.Idle);
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x00013683 File Offset: 0x00011883
		protected virtual void ChangePlaylistManagerState(MMSMPlaylistManager.PlaylistManagerStates newManagerState)
		{
			this.PlaylistManagerState.ChangeState(newManagerState);
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x00013691 File Offset: 0x00011891
		protected virtual void Update()
		{
			if (this.PlaylistManagerState.CurrentState == MMSMPlaylistManager.PlaylistManagerStates.Idle)
			{
				base.enabled = false;
				return;
			}
			this.UpdateTimeAndProgress();
			this.HandleTimescale();
			this.HandleEndOfSong();
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x000136BC File Offset: 0x000118BC
		protected virtual void HandleTimescale()
		{
			if (this.BindPitchToTimeScale)
			{
				float num = MMMaths.Remap(Time.timeScale, this.TimescaleRemapFrom.x, this.TimescaleRemapFrom.y, this.TimescaleRemapTo.x, this.TimescaleRemapTo.y);
				this.SetPitchMultiplier(num);
			}
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x00013710 File Offset: 0x00011910
		protected virtual void UpdateTimeAndProgress()
		{
			this.CurrentTime = this._currentlyPlayingAudioSource.time;
			this.CurrentTimeLeft = this._currentlyPlayingAudioSource.clip.length - this._currentlyPlayingAudioSource.time;
			this.CurrentProgress = this.CurrentTime / this.CurrentClipDuration;
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x00013763 File Offset: 0x00011963
		protected virtual void PlayFirstSong()
		{
			this.Playlist.Initialization();
			this.CurrentSongIndex = -1;
			this.HandleNextSong(1, false);
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x00013780 File Offset: 0x00011980
		protected virtual void HandleEndOfSong()
		{
			if (this.PlaylistManagerState.CurrentState != MMSMPlaylistManager.PlaylistManagerStates.Playing)
			{
				return;
			}
			if (this._currentlyPlayingAudioSource.isPlaying)
			{
				if (this.FadeIn && this.FadeOut && this.CurrentTimeLeft < this.FadeDuration)
				{
					this.HandleNextSong(1, false);
				}
				return;
			}
			this.HandleNextSong(1, false);
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x000137D8 File Offset: 0x000119D8
		protected virtual void HandleNextSong(int direction, bool bypassLoop)
		{
			if (this.IsPlaying)
			{
				MMSMPlaylistManager.PlaylistEvent onSongEnd = this.OnSongEnd;
				if (onSongEnd != null)
				{
					onSongEnd();
				}
			}
			int num = this.Playlist.PickNextIndex(direction, this.CurrentSongIndex, ref this._queuedSongIndex, bypassLoop);
			if (num == -1)
			{
				this.ChangePlaylistManagerState(MMSMPlaylistManager.PlaylistManagerStates.Idle);
			}
			if (num == -2)
			{
				this.HandleEndOfPlaylist();
				return;
			}
			if (num >= 0 && num < this.Playlist.Songs.Count)
			{
				this.PlaySongAt(num);
			}
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x0001384D File Offset: 0x00011A4D
		protected virtual void HandleEndOfPlaylist()
		{
			MMSMPlaylistManager.PlaylistEvent onPlaylistEnd = this.OnPlaylistEnd;
			if (onPlaylistEnd != null)
			{
				onPlaylistEnd();
			}
			if (this.Playlist.NextPlaylist != null)
			{
				this.ChangePlaylistAndPlay(this.Playlist.NextPlaylist);
				return;
			}
			this.ChangePlaylistManagerState(MMSMPlaylistManager.PlaylistManagerStates.Idle);
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x0001388C File Offset: 0x00011A8C
		public virtual void Play()
		{
			switch (this.PlaylistManagerState.CurrentState)
			{
			case MMSMPlaylistManager.PlaylistManagerStates.Idle:
				this.PlayFirstSong();
				return;
			case MMSMPlaylistManager.PlaylistManagerStates.Playing:
				break;
			case MMSMPlaylistManager.PlaylistManagerStates.Paused:
				MMPersistentSingleton<MMSoundManager>.Instance.ResumeSound(this._currentlyPlayingAudioSource);
				this.ChangePlaylistManagerState(MMSMPlaylistManager.PlaylistManagerStates.Playing);
				break;
			default:
				return;
			}
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x000138D8 File Offset: 0x00011AD8
		public virtual void PlaySongAt(int songIndex)
		{
			base.enabled = true;
			if (this.Playlist.Songs.Count == 0)
			{
				return;
			}
			this.Stop();
			this._options = this.Playlist.Songs[songIndex].Options;
			this._options.MmSoundManagerTrack = this.Playlist.Track;
			this._options.Volume = this._options.Volume * this.VolumeMultiplier;
			this._options.Pitch = this._options.Pitch * this.PitchMultiplier;
			this._options.Persistent = this.Persistent;
			this._currentlyPlayingAudioSource = MMSoundManagerSoundPlayEvent.Trigger(this.Playlist.Songs[songIndex].Clip, this._options);
			MMSMPlaylistManager.PlaylistEvent onSongStart = this.OnSongStart;
			if (onSongStart != null)
			{
				onSongStart();
			}
			if (this.FadeIn)
			{
				MMPersistentSingleton<MMSoundManager>.Instance.FadeSound(this._currentlyPlayingAudioSource, this.FadeDuration, 0f, this._currentlyPlayingAudioSource.volume, this.FadeTween, false);
			}
			this.ChangePlaylistManagerState(MMSMPlaylistManager.PlaylistManagerStates.Playing);
			this.CurrentSongIndex = songIndex;
			this.CurrentClipDuration = this._currentlyPlayingAudioSource.clip.length;
			this.CurrentSongName = this.Playlist.Songs[songIndex].Name;
			this.Playlist.Songs[songIndex].PlayCount++;
			this.Playlist.PlayCount++;
			MMPlaylistNewSongStartedEvent.Trigger(this.Channel);
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x00013A5B File Offset: 0x00011C5B
		public virtual void Pause()
		{
			if (this.PlaylistManagerState.CurrentState != MMSMPlaylistManager.PlaylistManagerStates.Playing)
			{
				return;
			}
			MMPersistentSingleton<MMSoundManager>.Instance.PauseSound(this._currentlyPlayingAudioSource);
			this.ChangePlaylistManagerState(MMSMPlaylistManager.PlaylistManagerStates.Paused);
			MMSMPlaylistManager.PlaylistEvent onPause = this.OnPause;
			if (onPause == null)
			{
				return;
			}
			onPause();
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x00013A94 File Offset: 0x00011C94
		public virtual void Stop()
		{
			if (this._currentlyPlayingAudioSource == null || !this._currentlyPlayingAudioSource.isPlaying)
			{
				return;
			}
			if (this.FadeOut)
			{
				if (MMPersistentSingleton<MMSoundManager>.Instance.SoundIsFadingOut(this._currentlyPlayingAudioSource))
				{
					return;
				}
				MMPersistentSingleton<MMSoundManager>.Instance.FadeSound(this._currentlyPlayingAudioSource, this.FadeDuration, this._currentlyPlayingAudioSource.volume, 0f, this.FadeTween, true);
			}
			else
			{
				MMPersistentSingleton<MMSoundManager>.Instance.FreeSound(this._currentlyPlayingAudioSource);
			}
			this.ChangePlaylistManagerState(MMSMPlaylistManager.PlaylistManagerStates.Idle);
			MMSMPlaylistManager.PlaylistEvent onStop = this.OnStop;
			if (onStop == null)
			{
				return;
			}
			onStop();
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x00013B30 File Offset: 0x00011D30
		public virtual void StopWithFade(bool withFade = true)
		{
			if (this.PlaylistManagerState.CurrentState == MMSMPlaylistManager.PlaylistManagerStates.Idle)
			{
				return;
			}
			if (!withFade)
			{
				MMPersistentSingleton<MMSoundManager>.Instance.FreeSound(this._currentlyPlayingAudioSource);
				MMSMPlaylistManager.PlaylistEvent onStop = this.OnStop;
				if (onStop != null)
				{
					onStop();
				}
			}
			else
			{
				this.Stop();
			}
			this.CurrentSongIndex = -1;
			this.ChangePlaylistManagerState(MMSMPlaylistManager.PlaylistManagerStates.Idle);
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x00013B85 File Offset: 0x00011D85
		public virtual void SetCurrentSongLoop(bool loop)
		{
			this._currentlyPlayingAudioSource.loop = loop;
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x00013B93 File Offset: 0x00011D93
		public virtual void PlayNextSong()
		{
			this.Stop();
			this.HandleNextSong(1, true);
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x00013BA3 File Offset: 0x00011DA3
		public virtual void PlayPreviousSong()
		{
			this.Stop();
			this.HandleNextSong(-1, true);
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x00013BB3 File Offset: 0x00011DB3
		public virtual void QueueSongAtIndex(int songIndex)
		{
			this._queuedSongIndex = songIndex;
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x00013BBC File Offset: 0x00011DBC
		public virtual void ChangePlaylist(MMSMPlaylist newPlaylist)
		{
			this.Playlist = newPlaylist;
			this.Playlist.Initialization();
			this.CurrentSongIndex = -1;
			MMSMPlaylistManager.PlaylistEvent onPlaylistChange = this.OnPlaylistChange;
			if (onPlaylistChange == null)
			{
				return;
			}
			onPlaylistChange();
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x00013BE7 File Offset: 0x00011DE7
		public virtual void ChangePlaylistAndPlay(MMSMPlaylist newPlaylist)
		{
			this.ChangePlaylist(newPlaylist);
			this.PlayFirstSong();
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x00013BF6 File Offset: 0x00011DF6
		public virtual void ResetPlayCount()
		{
			this.Playlist.ResetPlayCount();
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x00013C04 File Offset: 0x00011E04
		public virtual void SetVolumeMultiplier(float newVolumeMultiplier)
		{
			float num = Mathf.Clamp(newVolumeMultiplier, 0f, 2f);
			MMPlaylistVolumeMultiplierEvent.Trigger(this.Channel, num, true);
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x00013C30 File Offset: 0x00011E30
		public virtual void SetPitchMultiplier(float newPitchMultiplier)
		{
			float num = Mathf.Clamp(newPitchMultiplier, 0f, 20f);
			MMPlaylistPitchMultiplierEvent.Trigger(this.Channel, num, true);
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x00013C5B File Offset: 0x00011E5B
		protected virtual void SetTargetPlaylist()
		{
			this.ChangePlaylist(this.TestPlaylist);
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x00013C69 File Offset: 0x00011E69
		protected virtual void PlayTargetPlaylist()
		{
			this.ChangePlaylistAndPlay(this.TestPlaylist);
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x00013C78 File Offset: 0x00011E78
		protected virtual void QueueTargetSong()
		{
			int num = Mathf.Clamp(this.TargetSongIndex, 0, this.Playlist.Songs.Count - 1);
			this.QueueSongAtIndex(num);
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x00013CAC File Offset: 0x00011EAC
		protected virtual void PlayTargetSong()
		{
			int num = Mathf.Clamp(this.TargetSongIndex, 0, this.Playlist.Songs.Count - 1);
			this.PlaySongAt(num);
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x00013CDF File Offset: 0x00011EDF
		protected virtual void SetCurrentSongToLoop()
		{
			this.SetCurrentSongLoop(true);
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x00013CE8 File Offset: 0x00011EE8
		protected virtual void StopCurrentSongFromLooping()
		{
			this.SetCurrentSongLoop(false);
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x00013CF1 File Offset: 0x00011EF1
		protected virtual void OnPlayEvent(int channel)
		{
			if (channel != this.Channel)
			{
				return;
			}
			this.Play();
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x00013D03 File Offset: 0x00011F03
		protected virtual void OnPauseEvent(int channel)
		{
			if (channel != this.Channel)
			{
				return;
			}
			this.Pause();
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x00013D15 File Offset: 0x00011F15
		protected virtual void OnStopEvent(int channel)
		{
			if (channel != this.Channel)
			{
				return;
			}
			this.Stop();
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x00013D27 File Offset: 0x00011F27
		protected virtual void OnPlayNextEvent(int channel)
		{
			if (channel != this.Channel)
			{
				return;
			}
			this.PlayNextSong();
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00013D39 File Offset: 0x00011F39
		protected virtual void OnPlayPreviousEvent(int channel)
		{
			if (channel != this.Channel)
			{
				return;
			}
			this.PlayPreviousSong();
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x00013D4B File Offset: 0x00011F4B
		protected virtual void OnPlayIndexEvent(int channel, int index)
		{
			if (channel != this.Channel)
			{
				return;
			}
			this.PlaySongAt(index);
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x00013D60 File Offset: 0x00011F60
		protected virtual void OnMMPlaylistVolumeMultiplierEvent(int channel, float newVolumeMultiplier, bool applyVolumeMultiplierInstantly = false)
		{
			if (channel != this.Channel)
			{
				return;
			}
			this.VolumeMultiplier = newVolumeMultiplier;
			if (applyVolumeMultiplierInstantly)
			{
				this._currentlyPlayingAudioSource.volume = this.Playlist.Songs[this.CurrentSongIndex].Options.Volume * this.VolumeMultiplier;
			}
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x00013DB4 File Offset: 0x00011FB4
		protected virtual void OnMMPlaylistPitchMultiplierEvent(int channel, float newPitchMultiplier, bool applyPitchMultiplierInstantly = false)
		{
			if (channel != this.Channel)
			{
				return;
			}
			this.PitchMultiplier = newPitchMultiplier;
			if (applyPitchMultiplierInstantly)
			{
				this._currentlyPlayingAudioSource.pitch = this.Playlist.Songs[this.CurrentSongIndex].Options.Pitch * this.PitchMultiplier;
			}
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x00013E07 File Offset: 0x00012007
		protected virtual void OnMMPlaylistChangeEvent(int channel, MMSMPlaylist newPlaylist, bool andPlay)
		{
			if (channel != this.Channel)
			{
				return;
			}
			if (andPlay)
			{
				this.ChangePlaylistAndPlay(newPlaylist);
				return;
			}
			this.ChangePlaylist(newPlaylist);
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x00013E28 File Offset: 0x00012028
		public virtual void StartListening()
		{
			this._listeningToEvents = true;
			MMPlaylistPauseEvent.Register(new MMPlaylistPauseEvent.Delegate(this.OnPauseEvent));
			MMPlaylistPlayEvent.Register(new MMPlaylistPlayEvent.Delegate(this.OnPlayEvent));
			MMPlaylistPlayNextEvent.Register(new MMPlaylistPlayNextEvent.Delegate(this.OnPlayNextEvent));
			MMPlaylistPlayPreviousEvent.Register(new MMPlaylistPlayPreviousEvent.Delegate(this.OnPlayPreviousEvent));
			MMPlaylistStopEvent.Register(new MMPlaylistStopEvent.Delegate(this.OnStopEvent));
			MMPlaylistPlayIndexEvent.Register(new MMPlaylistPlayIndexEvent.Delegate(this.OnPlayIndexEvent));
			MMPlaylistVolumeMultiplierEvent.Register(new MMPlaylistVolumeMultiplierEvent.Delegate(this.OnMMPlaylistVolumeMultiplierEvent));
			MMPlaylistPitchMultiplierEvent.Register(new MMPlaylistPitchMultiplierEvent.Delegate(this.OnMMPlaylistPitchMultiplierEvent));
			MMPlaylistChangeEvent.Register(new MMPlaylistChangeEvent.Delegate(this.OnMMPlaylistChangeEvent));
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x00013EE0 File Offset: 0x000120E0
		public virtual void StopListening()
		{
			this._listeningToEvents = false;
			MMPlaylistPauseEvent.Unregister(new MMPlaylistPauseEvent.Delegate(this.OnPauseEvent));
			MMPlaylistPlayEvent.Unregister(new MMPlaylistPlayEvent.Delegate(this.OnPlayEvent));
			MMPlaylistPlayNextEvent.Unregister(new MMPlaylistPlayNextEvent.Delegate(this.OnPlayNextEvent));
			MMPlaylistPlayPreviousEvent.Unregister(new MMPlaylistPlayPreviousEvent.Delegate(this.OnPlayPreviousEvent));
			MMPlaylistStopEvent.Unregister(new MMPlaylistStopEvent.Delegate(this.OnStopEvent));
			MMPlaylistPlayIndexEvent.Unregister(new MMPlaylistPlayIndexEvent.Delegate(this.OnPlayIndexEvent));
			MMPlaylistVolumeMultiplierEvent.Unregister(new MMPlaylistVolumeMultiplierEvent.Delegate(this.OnMMPlaylistVolumeMultiplierEvent));
			MMPlaylistPitchMultiplierEvent.Unregister(new MMPlaylistPitchMultiplierEvent.Delegate(this.OnMMPlaylistPitchMultiplierEvent));
			MMPlaylistChangeEvent.Unregister(new MMPlaylistChangeEvent.Delegate(this.OnMMPlaylistChangeEvent));
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x00013F96 File Offset: 0x00012196
		protected virtual void OnDestroy()
		{
			this.StopListening();
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00013FA0 File Offset: 0x000121A0
		protected virtual void OnApplicationPause(bool pauseStatus)
		{
			if (!this.AutoHandleApplicationPause)
			{
				return;
			}
			if (pauseStatus && this.PlaylistManagerState.CurrentState == MMSMPlaylistManager.PlaylistManagerStates.Playing)
			{
				this.Pause();
				this._shouldResumeOnApplicationPause = true;
			}
			if (!pauseStatus && this._shouldResumeOnApplicationPause)
			{
				this._shouldResumeOnApplicationPause = false;
				this.Play();
			}
		}

		// Token: 0x04000560 RID: 1376
		[MMInspectorGroup("Settings", true, 18, false)]
		[Tooltip("the channel used to target this playlist manager by playlist remote or playlist feedbacks")]
		public int Channel;

		// Token: 0x04000561 RID: 1377
		[Tooltip("the current playlist this manager will play")]
		public MMSMPlaylist Playlist;

		// Token: 0x04000562 RID: 1378
		[Tooltip("whether this playlist manager should auto play on start or not")]
		public bool PlayOnStart;

		// Token: 0x04000563 RID: 1379
		[Tooltip("a global volume multiplier to apply when playing a song")]
		[Range(0f, 1f)]
		public float VolumeMultiplier = 1f;

		// Token: 0x04000564 RID: 1380
		[Tooltip("a pitch multiplier to apply to all songs when playing them")]
		[Range(0f, 20f)]
		public float PitchMultiplier = 1f;

		// Token: 0x04000565 RID: 1381
		[Tooltip("if this is true, this playlist manager will persist from scene to scene and will keep playing")]
		public bool Persistent;

		// Token: 0x04000566 RID: 1382
		[Tooltip("if this is true, this singleton will auto detach if it finds itself parented on awake")]
		[MMCondition("Persistent", true)]
		public bool AutomaticallyUnparentOnAwake = true;

		// Token: 0x04000567 RID: 1383
		[Tooltip("if this is true, this playlist will automatically pause/resume OnApplicationPause, useful if you've prevented your game from running in the background")]
		public bool AutoHandleApplicationPause = true;

		// Token: 0x04000568 RID: 1384
		[MMInspectorGroup("Fade", true, 12, false)]
		[Tooltip("whether or not sounds should fade in when they start playing")]
		public bool FadeIn;

		// Token: 0x04000569 RID: 1385
		[Tooltip("whether or not sounds should fade out when they stop playing")]
		public bool FadeOut;

		// Token: 0x0400056A RID: 1386
		[Tooltip("the duration of the fade, in seconds")]
		public float FadeDuration = 1f;

		// Token: 0x0400056B RID: 1387
		[Tooltip("the tween to use when fading the sound")]
		public MMTweenType FadeTween = new MMTweenType(MMTween.MMTweenCurve.EaseInCubic);

		// Token: 0x0400056C RID: 1388
		[MMInspectorGroup("Time", true, 20, false)]
		[Tooltip("whether or not the playlist manager should have its pitch multiplier value driven by the current timescale. If set to true, songs would appear to slow down when time is slowed down, and to speed up when time scale is higher than normal")]
		public bool BindPitchToTimeScale;

		// Token: 0x0400056D RID: 1389
		[Tooltip("the values to remap timescale from (min and max) - when timescale is equal to TimescaleRemapFrom.x, the pitch multiplier will be TimescaleRemapTo.x")]
		[MMCondition("BindPitchToTimeScale", true)]
		public Vector2 TimescaleRemapFrom = new Vector2(0f, 2f);

		// Token: 0x0400056E RID: 1390
		[Tooltip("the values to remap timescale to (min and max) - when timescale is equal to TimescaleRemapFrom.x, the pitch multiplier will be TimescaleRemapTo.x")]
		[MMCondition("BindPitchToTimeScale", true)]
		public Vector2 TimescaleRemapTo = new Vector2(0.8f, 1.2f);

		// Token: 0x0400056F RID: 1391
		[MMInspectorGroup("Status", true, 14, false)]
		[Tooltip("the current state of the playlist, debug display only")]
		[MMReadOnly]
		public MMSMPlaylistManager.PlaylistManagerStates DebugCurrentManagerState;

		// Token: 0x04000570 RID: 1392
		[Tooltip("the index we're currently playing")]
		[MMReadOnly]
		public int CurrentSongIndex = -1;

		// Token: 0x04000571 RID: 1393
		[Tooltip("the name of the song that is currently playing")]
		[MMReadOnly]
		public string CurrentSongName;

		// Token: 0x04000572 RID: 1394
		[MMReadOnly]
		public MMStateMachine<MMSMPlaylistManager.PlaylistManagerStates> PlaylistManagerState;

		// Token: 0x04000573 RID: 1395
		[Tooltip("the time of the currently playing song")]
		[MMReadOnly]
		public float CurrentTime;

		// Token: 0x04000574 RID: 1396
		[Tooltip("the time (in seconds) left on the song currently playing")]
		[MMReadOnly]
		public float CurrentTimeLeft;

		// Token: 0x04000575 RID: 1397
		[Tooltip("the total duration of the song currently playing")]
		[MMReadOnly]
		public float CurrentClipDuration;

		// Token: 0x04000576 RID: 1398
		[Tooltip("the current normalized progress of the song currently playing")]
		[Range(0f, 1f)]
		public float CurrentProgress;

		// Token: 0x04000577 RID: 1399
		[MMInspectorGroup("Test Controls", true, 15, false)]
		[MMInspectorButton("Play")]
		public bool PlayButton;

		// Token: 0x04000578 RID: 1400
		[MMInspectorButton("Stop")]
		public bool StopButton;

		// Token: 0x04000579 RID: 1401
		[MMInspectorButton("Pause")]
		public bool PauseButton;

		// Token: 0x0400057A RID: 1402
		[MMInspectorButton("PlayPreviousSong")]
		public bool PreviousButton;

		// Token: 0x0400057B RID: 1403
		[MMInspectorButton("PlayNextSong")]
		public bool NextButton;

		// Token: 0x0400057C RID: 1404
		[Tooltip("the index of the song to play when pressing the PlayTargetSong button")]
		public int TargetSongIndex;

		// Token: 0x0400057D RID: 1405
		[MMInspectorButton("PlayTargetSong")]
		public bool TargetSongButton;

		// Token: 0x0400057E RID: 1406
		[MMInspectorButton("QueueTargetSong")]
		public bool QueueTargetSongButton;

		// Token: 0x0400057F RID: 1407
		[MMInspectorButton("SetCurrentSongToLoop")]
		public bool SetLoopTargetSongButton;

		// Token: 0x04000580 RID: 1408
		[MMInspectorButton("StopCurrentSongFromLooping")]
		public bool StopLoopTargetSongButton;

		// Token: 0x04000581 RID: 1409
		[Tooltip("a playlist you can set to use with the SetTargetPlaylist and PlayTargetPlaylist buttons")]
		public MMSMPlaylist TestPlaylist;

		// Token: 0x04000582 RID: 1410
		[MMInspectorButton("SetTargetPlaylist")]
		public bool SetTargetPlaylistButton;

		// Token: 0x04000583 RID: 1411
		[MMInspectorButton("PlayTargetPlaylist")]
		public bool PlayTargetPlaylistButton;

		// Token: 0x04000584 RID: 1412
		[MMInspectorButton("ResetPlayCount")]
		public bool ResetPlayCountButton;

		// Token: 0x04000585 RID: 1413
		[Tooltip("a slider used to test volume control")]
		[Range(0f, 2f)]
		public float TestVolumeControl = 1f;

		// Token: 0x04000586 RID: 1414
		[Tooltip("a slider used to test speed control")]
		[Range(0f, 20f)]
		public float TestPlaybackSpeedControl = 1f;

		// Token: 0x04000587 RID: 1415
		public MMSMPlaylistManager.PlaylistEvent OnSongStart;

		// Token: 0x04000588 RID: 1416
		public MMSMPlaylistManager.PlaylistEvent OnSongEnd;

		// Token: 0x04000589 RID: 1417
		public MMSMPlaylistManager.PlaylistEvent OnPause;

		// Token: 0x0400058A RID: 1418
		public MMSMPlaylistManager.PlaylistEvent OnStop;

		// Token: 0x0400058B RID: 1419
		public MMSMPlaylistManager.PlaylistEvent OnPlaylistChange;

		// Token: 0x0400058C RID: 1420
		public MMSMPlaylistManager.PlaylistEvent OnPlaylistEnd;

		// Token: 0x0400058D RID: 1421
		protected bool _shouldResumeOnApplicationPause;

		// Token: 0x0400058E RID: 1422
		protected static MMSMPlaylistManager _instance;

		// Token: 0x0400058F RID: 1423
		protected int _queuedSongIndex = -1;

		// Token: 0x04000590 RID: 1424
		protected AudioSource _currentlyPlayingAudioSource;

		// Token: 0x04000591 RID: 1425
		protected MMSoundManagerPlayOptions _options;

		// Token: 0x04000592 RID: 1426
		protected float _lastTestVolumeControl = 1f;

		// Token: 0x04000593 RID: 1427
		protected float _lastTestPlaybackSpeedControl = 1f;

		// Token: 0x04000594 RID: 1428
		internal bool _listeningToEvents;

		// Token: 0x0200040E RID: 1038
		public enum PlaylistManagerStates
		{
			// Token: 0x04001EDD RID: 7901
			Idle,
			// Token: 0x04001EDE RID: 7902
			Playing,
			// Token: 0x04001EDF RID: 7903
			Paused
		}

		// Token: 0x0200040F RID: 1039
		// (Invoke) Token: 0x06001993 RID: 6547
		public delegate void PlaylistEvent();
	}
}
