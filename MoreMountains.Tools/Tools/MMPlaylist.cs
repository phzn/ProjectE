using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000C3 RID: 195
	[AddComponentMenu("More Mountains/Tools/Audio/MMPlaylist")]
	[MMRequiresConstantRepaint]
	public class MMPlaylist : MMMonoBehaviour
	{
		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060003DB RID: 987 RVA: 0x000128DF File Offset: 0x00010ADF
		public static bool HasInstance
		{
			get
			{
				return MMPlaylist._instance != null;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060003DC RID: 988 RVA: 0x000128EC File Offset: 0x00010AEC
		public static MMPlaylist Current
		{
			get
			{
				return MMPlaylist._instance;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060003DD RID: 989 RVA: 0x000128F4 File Offset: 0x00010AF4
		public static MMPlaylist Instance
		{
			get
			{
				if (MMPlaylist._instance == null)
				{
					MMPlaylist._instance = Object.FindObjectOfType<MMPlaylist>();
					if (MMPlaylist._instance == null)
					{
						MMPlaylist._instance = new GameObject
						{
							name = typeof(MMPlaylist).Name + "_AutoCreated"
						}.AddComponent<MMPlaylist>();
					}
				}
				return MMPlaylist._instance;
			}
		}

		// Token: 0x060003DE RID: 990 RVA: 0x00012958 File Offset: 0x00010B58
		protected virtual void Awake()
		{
			this.InitializeSingleton();
		}

		// Token: 0x060003DF RID: 991 RVA: 0x00012960 File Offset: 0x00010B60
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
			if (MMPlaylist._instance == null)
			{
				MMPlaylist._instance = this;
				Object.DontDestroyOnLoad(base.transform.gameObject);
				this._enabled = true;
				return;
			}
			if (this != MMPlaylist._instance)
			{
				Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x000129D5 File Offset: 0x00010BD5
		protected virtual void Start()
		{
			this.Initialization();
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x000129E0 File Offset: 0x00010BE0
		protected virtual void Initialization()
		{
			if (this.RandomOrder && this.RandomizeOrderSeed)
			{
				Random.InitState(Environment.TickCount);
			}
			this._songsPlayedSoFar = 0;
			this.PlaylistState = new MMStateMachine<MMPlaylist.PlaylistStates>(base.gameObject, true);
			this.ChangePlaylistState(MMPlaylist.PlaylistStates.Idle);
			if (this.Songs.Count == 0)
			{
				return;
			}
			if (this.PlayOnStart)
			{
				this.PlayFirstSong();
			}
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x00012A43 File Offset: 0x00010C43
		protected virtual void ChangePlaylistState(MMPlaylist.PlaylistStates newState)
		{
			this.PlaylistState.ChangeState(newState);
			this.DebugCurrentState = newState;
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x00012A58 File Offset: 0x00010C58
		protected virtual void PlayFirstSong()
		{
			this._songsPlayedThisCycle = 0;
			this.CurrentlyPlayingIndex = -1;
			int num = this.PickNextIndex();
			this._coroutine = base.StartCoroutine(this.PlaySong(num));
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x00012A8D File Offset: 0x00010C8D
		protected virtual IEnumerator PlaySong(int index)
		{
			if (this.Songs.Count == 0)
			{
				yield break;
			}
			if (!this.Endless && this._songsPlayedThisCycle > this.Songs.Count)
			{
				yield break;
			}
			if (this._coroutine != null)
			{
				base.StopCoroutine(this._coroutine);
			}
			if (this.PlaylistState.CurrentState == MMPlaylist.PlaylistStates.Playing && index >= 0 && index < this.Songs.Count && !this.Songs[index].TargetAudioSource.isPlaying)
			{
				base.StartCoroutine(this.Fade(this.CurrentlyPlayingIndex, Random.Range(this.Songs[index].CrossFadeDuration.x, this.Songs[index].CrossFadeDuration.y), this.Songs[this.CurrentlyPlayingIndex].Volume.y * this.VolumeMultiplier, this.Songs[this.CurrentlyPlayingIndex].Volume.x * this.VolumeMultiplier, true));
			}
			if (this.CurrentlyPlayingIndex >= 0 && this.Songs.Count > this.CurrentlyPlayingIndex)
			{
				foreach (MMPlaylistSong mmplaylistSong in this.Songs)
				{
					if (mmplaylistSong != this.Songs[this.CurrentlyPlayingIndex])
					{
						mmplaylistSong.Fading = false;
					}
				}
			}
			if (index < 0 || index >= this.Songs.Count)
			{
				yield break;
			}
			yield return MMCoroutine.WaitFor(Random.Range(this.Songs[index].InitialDelay.x, this.Songs[index].InitialDelay.y));
			if (this.Songs[index].TargetAudioSource == null)
			{
				Debug.LogError(base.name + " : the playlist song you're trying to play is null");
				yield break;
			}
			this.Songs[index].TargetAudioSource.pitch = Random.Range(this.Songs[index].Pitch.x, this.Songs[index].Pitch.y);
			this.Songs[index].TargetAudioSource.panStereo = this.Songs[index].StereoPan;
			this.Songs[index].TargetAudioSource.spatialBlend = this.Songs[index].SpatialBlend;
			this.Songs[index].TargetAudioSource.loop = this.Songs[index].Loop;
			base.StartCoroutine(this.Fade(index, Random.Range(this.Songs[index].CrossFadeDuration.x, this.Songs[index].CrossFadeDuration.y), this.Songs[index].Volume.x * this.VolumeMultiplier, this.Songs[index].Volume.y * this.VolumeMultiplier, false));
			this.Songs[index].TargetAudioSource.Play();
			this.CurrentSongName = this.Songs[index].TargetAudioSource.clip.name;
			this.ChangePlaylistState(MMPlaylist.PlaylistStates.Playing);
			this.Songs[index].Playing = true;
			this.CurrentlyPlayingIndex = index;
			this._songsPlayedSoFar++;
			this._songsPlayedThisCycle++;
			while (this.Songs[index].TargetAudioSource.isPlaying || this.PlaylistState.CurrentState == MMPlaylist.PlaylistStates.Paused || this._shouldResumeOnApplicationPause)
			{
				yield return null;
			}
			if (this.PlaylistState.CurrentState != MMPlaylist.PlaylistStates.Playing)
			{
				yield break;
			}
			if (this._songsPlayedSoFar < this.Songs.Count)
			{
				this._coroutine = base.StartCoroutine(this.PlaySong(this.PickNextIndex()));
			}
			else if (this.Endless)
			{
				this._coroutine = base.StartCoroutine(this.PlaySong(this.PickNextIndex()));
			}
			else
			{
				this.ChangePlaylistState(MMPlaylist.PlaylistStates.Idle);
			}
			yield break;
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x00012AA3 File Offset: 0x00010CA3
		protected virtual IEnumerator Fade(int index, float duration, float initialVolume, float endVolume, bool stopAtTheEnd)
		{
			if (index < 0 || index >= this.Songs.Count)
			{
				yield break;
			}
			float startTimestamp = Time.time;
			this.Songs[index].Fading = true;
			while (Time.time - startTimestamp < duration && this.Songs[index].Fading)
			{
				float num = MMMaths.Remap(Time.time - startTimestamp, 0f, duration, 0f, 1f);
				this.Songs[index].TargetAudioSource.volume = Mathf.Lerp(initialVolume, endVolume, num);
				yield return null;
			}
			this.Songs[index].TargetAudioSource.volume = endVolume;
			if (stopAtTheEnd)
			{
				this.Songs[index].TargetAudioSource.Stop();
				this.Songs[index].Playing = false;
				this.Songs[index].Fading = false;
			}
			yield break;
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x00012AD8 File Offset: 0x00010CD8
		protected virtual int PickNextIndex()
		{
			if (this.Songs.Count == 0)
			{
				return -1;
			}
			if (this._queuedSong != -1)
			{
				int queuedSong = this._queuedSong;
				this._queuedSong = -1;
				return queuedSong;
			}
			int num = this.CurrentlyPlayingIndex;
			if (this.RandomOrder)
			{
				while (num == this.CurrentlyPlayingIndex)
				{
					num = Random.Range(0, this.Songs.Count);
				}
			}
			else
			{
				num = (this.CurrentlyPlayingIndex + 1) % this.Songs.Count;
			}
			return num;
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x00012B50 File Offset: 0x00010D50
		protected virtual int PickPreviousIndex()
		{
			if (this.Songs.Count == 0)
			{
				return -1;
			}
			int num = this.CurrentlyPlayingIndex;
			if (this.RandomOrder)
			{
				while (num == this.CurrentlyPlayingIndex)
				{
					num = Random.Range(0, this.Songs.Count);
				}
			}
			else
			{
				num = this.CurrentlyPlayingIndex - 1;
				if (num < 0)
				{
					num = this.Songs.Count - 1;
				}
			}
			return num;
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x00012BB8 File Offset: 0x00010DB8
		public virtual void Play()
		{
			switch (this.PlaylistState.CurrentState)
			{
			case MMPlaylist.PlaylistStates.Idle:
				this.PlayFirstSong();
				return;
			case MMPlaylist.PlaylistStates.Playing:
				break;
			case MMPlaylist.PlaylistStates.Paused:
				this.Songs[this.CurrentlyPlayingIndex].TargetAudioSource.UnPause();
				this.ChangePlaylistState(MMPlaylist.PlaylistStates.Playing);
				break;
			default:
				return;
			}
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x00012C0D File Offset: 0x00010E0D
		public virtual void PlayAtIndex(int songIndex)
		{
			this._coroutine = base.StartCoroutine(this.PlaySong(songIndex));
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x00012C22 File Offset: 0x00010E22
		public virtual void QueueSongAtIndex(int songIndex)
		{
			this._queuedSong = songIndex;
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x00012C2B File Offset: 0x00010E2B
		public virtual void Pause()
		{
			if (this.PlaylistState.CurrentState != MMPlaylist.PlaylistStates.Playing)
			{
				return;
			}
			this.Songs[this.CurrentlyPlayingIndex].TargetAudioSource.Pause();
			this.ChangePlaylistState(MMPlaylist.PlaylistStates.Paused);
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x00012C60 File Offset: 0x00010E60
		public virtual void Stop()
		{
			if (this.PlaylistState.CurrentState != MMPlaylist.PlaylistStates.Playing)
			{
				return;
			}
			this.Songs[this.CurrentlyPlayingIndex].TargetAudioSource.Stop();
			this.Songs[this.CurrentlyPlayingIndex].Playing = false;
			this.Songs[this.CurrentlyPlayingIndex].Fading = false;
			this.CurrentlyPlayingIndex = -1;
			this.ChangePlaylistState(MMPlaylist.PlaylistStates.Idle);
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x00012CD3 File Offset: 0x00010ED3
		public virtual void SetLoop(bool loop)
		{
			this.Songs[this.CurrentlyPlayingIndex].TargetAudioSource.loop = loop;
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x00012CF4 File Offset: 0x00010EF4
		public virtual void PlayNextSong()
		{
			int num = this.PickNextIndex();
			this._coroutine = base.StartCoroutine(this.PlaySong(num));
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x00012D1C File Offset: 0x00010F1C
		public virtual void PlayPreviousSong()
		{
			int num = this.PickPreviousIndex();
			this._coroutine = base.StartCoroutine(this.PlaySong(num));
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x00012D44 File Offset: 0x00010F44
		protected virtual void PlayTargetSong()
		{
			int num = Mathf.Clamp(this.TargetSongIndex, 0, this.Songs.Count - 1);
			this.PlayAtIndex(num);
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x00012D74 File Offset: 0x00010F74
		protected virtual void QueueTargetSong()
		{
			int num = Mathf.Clamp(this.TargetSongIndex, 0, this.Songs.Count - 1);
			this.QueueSongAtIndex(num);
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x00012DA2 File Offset: 0x00010FA2
		protected virtual void SetLoopTargetSong()
		{
			this.SetLoop(true);
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x00012DAB File Offset: 0x00010FAB
		protected virtual void StopLoopTargetSong()
		{
			this.SetLoop(false);
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x00012DB4 File Offset: 0x00010FB4
		protected virtual void OnPlayEvent(int channel)
		{
			if (channel != this.Channel)
			{
				return;
			}
			this.Play();
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x00012DC6 File Offset: 0x00010FC6
		protected virtual void OnPauseEvent(int channel)
		{
			if (channel != this.Channel)
			{
				return;
			}
			this.Pause();
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x00012DD8 File Offset: 0x00010FD8
		protected virtual void OnStopEvent(int channel)
		{
			if (channel != this.Channel)
			{
				return;
			}
			this.Stop();
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x00012DEA File Offset: 0x00010FEA
		protected virtual void OnPlayNextEvent(int channel)
		{
			if (channel != this.Channel)
			{
				return;
			}
			this.PlayNextSong();
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x00012DFC File Offset: 0x00010FFC
		protected virtual void OnPlayPreviousEvent(int channel)
		{
			if (channel != this.Channel)
			{
				return;
			}
			this.PlayPreviousSong();
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x00012E0E File Offset: 0x0001100E
		protected virtual void OnPlayIndexEvent(int channel, int index)
		{
			if (channel != this.Channel)
			{
				return;
			}
			this._coroutine = base.StartCoroutine(this.PlaySong(index));
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x00012E30 File Offset: 0x00011030
		protected virtual void OnMMPlaylistVolumeMultiplierEvent(int channel, float newVolumeMultiplier, bool applyVolumeMultiplierInstantly = false)
		{
			if (channel != this.Channel)
			{
				return;
			}
			this.VolumeMultiplier = newVolumeMultiplier;
			if (applyVolumeMultiplierInstantly)
			{
				this.Songs[this.CurrentlyPlayingIndex].TargetAudioSource.volume = this.Songs[this.CurrentlyPlayingIndex].Volume.y * this.VolumeMultiplier;
			}
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x00012E90 File Offset: 0x00011090
		protected virtual void OnEnable()
		{
			MMPlaylistPauseEvent.Register(new MMPlaylistPauseEvent.Delegate(this.OnPauseEvent));
			MMPlaylistPlayEvent.Register(new MMPlaylistPlayEvent.Delegate(this.OnPlayEvent));
			MMPlaylistPlayNextEvent.Register(new MMPlaylistPlayNextEvent.Delegate(this.OnPlayNextEvent));
			MMPlaylistPlayPreviousEvent.Register(new MMPlaylistPlayPreviousEvent.Delegate(this.OnPlayPreviousEvent));
			MMPlaylistStopEvent.Register(new MMPlaylistStopEvent.Delegate(this.OnStopEvent));
			MMPlaylistPlayIndexEvent.Register(new MMPlaylistPlayIndexEvent.Delegate(this.OnPlayIndexEvent));
			MMPlaylistVolumeMultiplierEvent.Register(new MMPlaylistVolumeMultiplierEvent.Delegate(this.OnMMPlaylistVolumeMultiplierEvent));
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x00012F1C File Offset: 0x0001111C
		protected virtual void OnDisable()
		{
			MMPlaylistPauseEvent.Unregister(new MMPlaylistPauseEvent.Delegate(this.OnPauseEvent));
			MMPlaylistPlayEvent.Unregister(new MMPlaylistPlayEvent.Delegate(this.OnPlayEvent));
			MMPlaylistPlayNextEvent.Unregister(new MMPlaylistPlayNextEvent.Delegate(this.OnPlayNextEvent));
			MMPlaylistPlayPreviousEvent.Unregister(new MMPlaylistPlayPreviousEvent.Delegate(this.OnPlayPreviousEvent));
			MMPlaylistStopEvent.Unregister(new MMPlaylistStopEvent.Delegate(this.OnStopEvent));
			MMPlaylistPlayIndexEvent.Unregister(new MMPlaylistPlayIndexEvent.Delegate(this.OnPlayIndexEvent));
			MMPlaylistVolumeMultiplierEvent.Unregister(new MMPlaylistVolumeMultiplierEvent.Delegate(this.OnMMPlaylistVolumeMultiplierEvent));
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x00012FA8 File Offset: 0x000111A8
		protected virtual void OnValidate()
		{
			if (!this._firstDeserialization)
			{
				if (this.Songs.Count != this._listCount)
				{
					if (this.Songs.Count > this._listCount)
					{
						foreach (MMPlaylistSong mmplaylistSong in this.Songs)
						{
							mmplaylistSong.Initialization();
						}
					}
					this._listCount = this.Songs.Count;
				}
				return;
			}
			if (this.Songs == null)
			{
				this._listCount = 0;
				this._firstDeserialization = false;
				return;
			}
			this._listCount = this.Songs.Count;
			this._firstDeserialization = false;
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x00013068 File Offset: 0x00011268
		protected virtual void OnApplicationPause(bool pauseStatus)
		{
			if (!this.AutoHandleApplicationPause)
			{
				return;
			}
			if (pauseStatus && this.PlaylistState.CurrentState == MMPlaylist.PlaylistStates.Playing)
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

		// Token: 0x04000529 RID: 1321
		[MMInspectorGroup("Playlist Songs", true, 18, false)]
		[Tooltip("the channel on which to broadcast orders for this playlist")]
		public int Channel;

		// Token: 0x0400052A RID: 1322
		[Tooltip("the songs that this playlist will play")]
		public List<MMPlaylistSong> Songs;

		// Token: 0x0400052B RID: 1323
		[MMInspectorGroup("Settings", true, 13, false)]
		[Tooltip("whether this should play in random order or not")]
		public bool RandomOrder;

		// Token: 0x0400052C RID: 1324
		[Tooltip("if this is true, random seed will be randomized by the system clock")]
		[MMCondition("RandomOrder", true)]
		public bool RandomizeOrderSeed = true;

		// Token: 0x0400052D RID: 1325
		[Tooltip("whether this playlist should play and loop as a whole forever or not")]
		public bool Endless = true;

		// Token: 0x0400052E RID: 1326
		[Tooltip("whether this playlist should auto play on start or not")]
		public bool PlayOnStart = true;

		// Token: 0x0400052F RID: 1327
		[Tooltip("a global volume multiplier to apply when playing a song")]
		public float VolumeMultiplier = 1f;

		// Token: 0x04000530 RID: 1328
		[Tooltip("if this is true, this playlist will automatically pause/resume OnApplicationPause, useful if you've prevented your game from running in the background")]
		public bool AutoHandleApplicationPause = true;

		// Token: 0x04000531 RID: 1329
		[MMInspectorGroup("Persistence", true, 32, false)]
		[Tooltip("if this is true, this playlist will persist from scene to scene")]
		public bool Persistent;

		// Token: 0x04000532 RID: 1330
		[Tooltip("if this is true, this singleton will auto detach if it finds itself parented on awake")]
		[MMCondition("Persistent", true)]
		public bool AutomaticallyUnparentOnAwake = true;

		// Token: 0x04000533 RID: 1331
		[MMInspectorGroup("Status", true, 14, false)]
		[Tooltip("the current state of the playlist, debug display only")]
		[MMReadOnly]
		public MMPlaylist.PlaylistStates DebugCurrentState;

		// Token: 0x04000534 RID: 1332
		[Tooltip("the index we're currently playing")]
		[MMReadOnly]
		public int CurrentlyPlayingIndex = -1;

		// Token: 0x04000535 RID: 1333
		[Tooltip("the name of the song that is currently playing")]
		[MMReadOnly]
		public string CurrentSongName;

		// Token: 0x04000536 RID: 1334
		[MMReadOnly]
		public MMStateMachine<MMPlaylist.PlaylistStates> PlaylistState;

		// Token: 0x04000537 RID: 1335
		[MMInspectorGroup("Tests", true, 15, false)]
		[MMInspectorButton("Play")]
		public bool PlayButton;

		// Token: 0x04000538 RID: 1336
		[MMInspectorButton("Pause")]
		public bool PauseButton;

		// Token: 0x04000539 RID: 1337
		[MMInspectorButton("Stop")]
		public bool StopButton;

		// Token: 0x0400053A RID: 1338
		[MMInspectorButton("PlayNextSong")]
		public bool NextButton;

		// Token: 0x0400053B RID: 1339
		[Tooltip("the index of the song to play when pressing the PlayTargetSong button")]
		public int TargetSongIndex;

		// Token: 0x0400053C RID: 1340
		[MMInspectorButton("PlayTargetSong")]
		public bool TargetSongButton;

		// Token: 0x0400053D RID: 1341
		[MMInspectorButton("QueueTargetSong")]
		public bool QueueTargetSongButton;

		// Token: 0x0400053E RID: 1342
		[MMInspectorButton("SetLoopTargetSong")]
		public bool SetLoopTargetSongButton;

		// Token: 0x0400053F RID: 1343
		[MMInspectorButton("StopLoopTargetSong")]
		public bool StopLoopTargetSongButton;

		// Token: 0x04000540 RID: 1344
		protected int _songsPlayedSoFar;

		// Token: 0x04000541 RID: 1345
		protected int _songsPlayedThisCycle;

		// Token: 0x04000542 RID: 1346
		protected Coroutine _coroutine;

		// Token: 0x04000543 RID: 1347
		protected bool _shouldResumeOnApplicationPause;

		// Token: 0x04000544 RID: 1348
		protected static MMPlaylist _instance;

		// Token: 0x04000545 RID: 1349
		protected bool _enabled;

		// Token: 0x04000546 RID: 1350
		protected int _queuedSong = -1;

		// Token: 0x04000547 RID: 1351
		protected bool _firstDeserialization = true;

		// Token: 0x04000548 RID: 1352
		protected int _listCount;

		// Token: 0x02000409 RID: 1033
		public enum PlaylistStates
		{
			// Token: 0x04001EC3 RID: 7875
			Idle,
			// Token: 0x04001EC4 RID: 7876
			Playing,
			// Token: 0x04001EC5 RID: 7877
			Paused
		}
	}
}
