using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Features.CielOS.VFS;
using Features.Home;
using Features.MainMenu;
using ProjectExhibitionist.Core.SceneManagement;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace Features.CielOS
{
	// Token: 0x02001378 RID: 4984
	public class CielOSAudioService : MonoBehaviour
	{
		// Token: 0x17001895 RID: 6293
		// (get) Token: 0x06009A49 RID: 39497 RVA: 0x003F22E7 File Offset: 0x003F04E7
		// (set) Token: 0x06009A4A RID: 39498 RVA: 0x003F22EE File Offset: 0x003F04EE
		public static CielOSAudioService Instance { get; private set; }

		// Token: 0x14000111 RID: 273
		// (add) Token: 0x06009A4B RID: 39499 RVA: 0x003F22F8 File Offset: 0x003F04F8
		// (remove) Token: 0x06009A4C RID: 39500 RVA: 0x003F2330 File Offset: 0x003F0530
		public event Action TrackChanged;

		// Token: 0x14000112 RID: 274
		// (add) Token: 0x06009A4D RID: 39501 RVA: 0x003F2368 File Offset: 0x003F0568
		// (remove) Token: 0x06009A4E RID: 39502 RVA: 0x003F23A0 File Offset: 0x003F05A0
		public event Action StateChanged;

		// Token: 0x17001896 RID: 6294
		// (get) Token: 0x06009A4F RID: 39503 RVA: 0x003F23D5 File Offset: 0x003F05D5
		public IReadOnlyList<string> Tracks
		{
			get
			{
				return this._tracks;
			}
		}

		// Token: 0x17001897 RID: 6295
		// (get) Token: 0x06009A50 RID: 39504 RVA: 0x003F23DD File Offset: 0x003F05DD
		public int Index
		{
			get
			{
				return this._index;
			}
		}

		// Token: 0x17001898 RID: 6296
		// (get) Token: 0x06009A51 RID: 39505 RVA: 0x003F23E5 File Offset: 0x003F05E5
		public bool IsPlaying
		{
			get
			{
				return this._audio != null && this._audio.isPlaying;
			}
		}

		// Token: 0x17001899 RID: 6297
		// (get) Token: 0x06009A52 RID: 39506 RVA: 0x003F2402 File Offset: 0x003F0602
		public bool IsPaused
		{
			get
			{
				return this._paused;
			}
		}

		// Token: 0x1700189A RID: 6298
		// (get) Token: 0x06009A53 RID: 39507 RVA: 0x003F240A File Offset: 0x003F060A
		public bool IsLoading
		{
			get
			{
				return this._loading;
			}
		}

		// Token: 0x1700189B RID: 6299
		// (get) Token: 0x06009A54 RID: 39508 RVA: 0x003F2414 File Offset: 0x003F0614
		public float PlaybackProgress
		{
			get
			{
				if (!(this._audio != null) || !(this._audio.clip != null) || this._audio.clip.length <= 0f)
				{
					return 0f;
				}
				return Mathf.Clamp01(this._audio.time / this._audio.clip.length);
			}
		}

		// Token: 0x1700189C RID: 6300
		// (get) Token: 0x06009A55 RID: 39509 RVA: 0x003F2480 File Offset: 0x003F0680
		public float PlaybackSeconds
		{
			get
			{
				if (!(this._audio != null) || !(this._audio.clip != null))
				{
					return 0f;
				}
				return this._audio.time;
			}
		}

		// Token: 0x1700189D RID: 6301
		// (get) Token: 0x06009A56 RID: 39510 RVA: 0x003F24B4 File Offset: 0x003F06B4
		public float DurationSeconds
		{
			get
			{
				if (!(this._audio != null) || !(this._audio.clip != null))
				{
					return 0f;
				}
				return this._audio.clip.length;
			}
		}

		// Token: 0x1700189E RID: 6302
		// (get) Token: 0x06009A57 RID: 39511 RVA: 0x003F24ED File Offset: 0x003F06ED
		public bool IsHomeMusicSuppressed
		{
			get
			{
				return this._homeMusicSuppressed;
			}
		}

		// Token: 0x1700189F RID: 6303
		// (get) Token: 0x06009A58 RID: 39512 RVA: 0x003F24F5 File Offset: 0x003F06F5
		public bool HasPlaybackRetention
		{
			get
			{
				return this._playbackRetentionOwners.Count > 0;
			}
		}

		// Token: 0x170018A0 RID: 6304
		// (get) Token: 0x06009A59 RID: 39513 RVA: 0x003F2505 File Offset: 0x003F0705
		// (set) Token: 0x06009A5A RID: 39514 RVA: 0x003F250D File Offset: 0x003F070D
		public bool Shuffle
		{
			get
			{
				return this._shuffle;
			}
			set
			{
				this._shuffle = value;
				Action stateChanged = this.StateChanged;
				if (stateChanged == null)
				{
					return;
				}
				stateChanged();
			}
		}

		// Token: 0x170018A1 RID: 6305
		// (get) Token: 0x06009A5B RID: 39515 RVA: 0x003F2526 File Offset: 0x003F0726
		// (set) Token: 0x06009A5C RID: 39516 RVA: 0x003F252E File Offset: 0x003F072E
		public CielOSAudioService.RepeatMode Repeat
		{
			get
			{
				return this._repeat;
			}
			set
			{
				this._repeat = value;
				Action stateChanged = this.StateChanged;
				if (stateChanged == null)
				{
					return;
				}
				stateChanged();
			}
		}

		// Token: 0x170018A2 RID: 6306
		// (get) Token: 0x06009A5D RID: 39517 RVA: 0x003F2547 File Offset: 0x003F0747
		// (set) Token: 0x06009A5E RID: 39518 RVA: 0x003F2568 File Offset: 0x003F0768
		public float Volume
		{
			get
			{
				if (!(this._audio != null))
				{
					return 1f;
				}
				return this._audio.volume;
			}
			set
			{
				if (this._audio != null)
				{
					this._audio.volume = Mathf.Clamp01(value);
					PlayerPrefs.SetFloat("CielOS.Volume", this._audio.volume);
				}
				Action stateChanged = this.StateChanged;
				if (stateChanged == null)
				{
					return;
				}
				stateChanged();
			}
		}

		// Token: 0x170018A3 RID: 6307
		// (get) Token: 0x06009A5F RID: 39519 RVA: 0x003F25B9 File Offset: 0x003F07B9
		public string CurrentName
		{
			get
			{
				if (this._index < 0 || this._index >= this._tracks.Count)
				{
					return null;
				}
				return Path.GetFileName(this._tracks[this._index]);
			}
		}

		// Token: 0x06009A60 RID: 39520 RVA: 0x003F25F0 File Offset: 0x003F07F0
		public static CielOSAudioService Ensure(Transform canvas)
		{
			if (CielOSAudioService.Instance != null)
			{
				CielOSAudioService.Instance.AttachToPersistentPlayer();
				return CielOSAudioService.Instance;
			}
			GameObject gameObject = new GameObject("CielOS Music AudioService");
			Transform player = PlayerRegistry.Player;
			if (player != null)
			{
				gameObject.transform.SetParent(player, false);
			}
			else if (canvas != null)
			{
				gameObject.transform.SetParent(canvas, false);
			}
			CielOSAudioService.Instance = gameObject.AddComponent<CielOSAudioService>();
			if (player == null && canvas == null)
			{
				Object.DontDestroyOnLoad(gameObject);
			}
			return CielOSAudioService.Instance;
		}

		// Token: 0x06009A61 RID: 39521 RVA: 0x003F2684 File Offset: 0x003F0884
		private void Awake()
		{
			if (CielOSAudioService.Instance != null && CielOSAudioService.Instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			CielOSAudioService.Instance = this;
			base.gameObject.name = "CielOS Music AudioService";
			this._audio = base.gameObject.AddComponent<AudioSource>();
			this._audio.playOnAwake = false;
			this._audio.loop = false;
			this._audio.spatialBlend = 0f;
			this._audio.volume = PlayerPrefs.GetFloat("CielOS.Volume", 0.8f);
			GameAudioRouter.Route(this._audio, GameAudioRouter.Channel.Music);
			SceneManager.sceneLoaded += this.HandleSceneLoaded;
			this.AttachToPersistentPlayer();
		}

		// Token: 0x06009A62 RID: 39522 RVA: 0x003F2744 File Offset: 0x003F0944
		private void Update()
		{
			if (this._audio != null && AudioListener.pause && !this._audio.ignoreListenerPause)
			{
				return;
			}
			if (this._audio != null && this._audio.clip != null && !this._audio.isPlaying && !this._paused && this._wasPlaying && this._loadCo == null)
			{
				this._wasPlaying = false;
				this.OnTrackFinished();
			}
			if (this._audio != null && this._audio.isPlaying)
			{
				this._wasPlaying = true;
			}
		}

		// Token: 0x06009A63 RID: 39523 RVA: 0x003F27EC File Offset: 0x003F09EC
		public void Rescan()
		{
			string playing = this.CurrentName;
			this._tracks.Clear();
			foreach (CielOSFileSystem.Entry entry in CielOSFileSystem.Instance.List("/Music"))
			{
				if (!entry.isDirectory)
				{
					string text = entry.name.ToLowerInvariant();
					if (text.EndsWith(".wav") || text.EndsWith(".ogg") || text.EndsWith(".mp3"))
					{
						this._tracks.Add(entry.virtualPath);
					}
				}
			}
			this._tracks.Sort(StringComparer.OrdinalIgnoreCase);
			if (playing != null)
			{
				this._index = this._tracks.FindIndex((string p) => Path.GetFileName(p) == playing);
			}
			Action trackChanged = this.TrackChanged;
			if (trackChanged == null)
			{
				return;
			}
			trackChanged();
		}

		// Token: 0x06009A64 RID: 39524 RVA: 0x003F28F0 File Offset: 0x003F0AF0
		public void PlayIndex(int i)
		{
			if (i < 0 || i >= this._tracks.Count)
			{
				return;
			}
			if (this._loadCo != null)
			{
				base.StopCoroutine(this._loadCo);
				this._loadCo = null;
			}
			if (this._audio != null)
			{
				this._audio.Stop();
				this._audio.clip = null;
			}
			this.ReleaseRuntimeClip();
			this._index = i;
			this._paused = false;
			this._wasPlaying = false;
			this._loading = true;
			Action trackChanged = this.TrackChanged;
			if (trackChanged != null)
			{
				trackChanged();
			}
			Action stateChanged = this.StateChanged;
			if (stateChanged != null)
			{
				stateChanged();
			}
			this._loadCo = base.StartCoroutine(this.LoadAndPlay(this._tracks[i]));
		}

		// Token: 0x06009A65 RID: 39525 RVA: 0x003F29B4 File Offset: 0x003F0BB4
		public void TogglePlayPause()
		{
			if (this._loading)
			{
				if (this._loadCo != null)
				{
					base.StopCoroutine(this._loadCo);
				}
				this._loadCo = null;
				this._loading = false;
				this._paused = true;
				this._wasPlaying = false;
				this.SetHomeMusicSuppressed(false);
				Action stateChanged = this.StateChanged;
				if (stateChanged == null)
				{
					return;
				}
				stateChanged();
				return;
			}
			else
			{
				if (this._audio == null)
				{
					return;
				}
				if (this._audio.clip == null)
				{
					if (this._tracks.Count > 0)
					{
						this.PlayIndex(Mathf.Max(0, this._index));
					}
					return;
				}
				if (this._audio.isPlaying)
				{
					this._audio.Pause();
					this._paused = true;
					this.SetHomeMusicSuppressed(false);
				}
				else
				{
					this._audio.UnPause();
					this._paused = !this._audio.isPlaying;
					this.SetHomeMusicSuppressed(this._audio.isPlaying);
				}
				Action stateChanged2 = this.StateChanged;
				if (stateChanged2 == null)
				{
					return;
				}
				stateChanged2();
				return;
			}
		}

		// Token: 0x06009A66 RID: 39526 RVA: 0x003F2ABC File Offset: 0x003F0CBC
		public void Next()
		{
			if (this._tracks.Count == 0)
			{
				return;
			}
			this.PlayIndex(this._shuffle ? this.RandomIndex() : ((this._index + 1) % this._tracks.Count));
		}

		// Token: 0x06009A67 RID: 39527 RVA: 0x003F2AF8 File Offset: 0x003F0CF8
		public void Prev()
		{
			if (this._tracks.Count == 0)
			{
				return;
			}
			if (this._audio != null && this._audio.time > 3f)
			{
				this._audio.time = 0f;
				return;
			}
			int num = ((this._index < 0) ? (this._tracks.Count - 1) : ((this._index - 1 + this._tracks.Count) % this._tracks.Count));
			this.PlayIndex(this._shuffle ? this.RandomIndex() : num);
		}

		// Token: 0x06009A68 RID: 39528 RVA: 0x003F2B94 File Offset: 0x003F0D94
		public void SkipPrevious()
		{
			if (this._tracks.Count == 0)
			{
				return;
			}
			int num = ((this._index < 0) ? (this._tracks.Count - 1) : ((this._index - 1 + this._tracks.Count) % this._tracks.Count));
			this.PlayIndex(this._shuffle ? this.RandomIndex() : num);
		}

		// Token: 0x06009A69 RID: 39529 RVA: 0x003F2BFF File Offset: 0x003F0DFF
		public void AcquirePlaybackRetention(Object owner)
		{
			if (owner != null)
			{
				this._playbackRetentionOwners.Add(owner.GetInstanceID());
			}
		}

		// Token: 0x06009A6A RID: 39530 RVA: 0x003F2C1C File Offset: 0x003F0E1C
		public void ReleasePlaybackRetention(Object owner)
		{
			if (owner != null)
			{
				this._playbackRetentionOwners.Remove(owner.GetInstanceID());
			}
		}

		// Token: 0x06009A6B RID: 39531 RVA: 0x003F2C3C File Offset: 0x003F0E3C
		public void Stop()
		{
			if (this._loadCo != null)
			{
				base.StopCoroutine(this._loadCo);
				this._loadCo = null;
			}
			this._loading = false;
			if (this._audio != null)
			{
				this._audio.Stop();
				this._audio.clip = null;
			}
			this.ReleaseRuntimeClip();
			this._index = -1;
			this._paused = false;
			this._wasPlaying = false;
			this.SetHomeMusicSuppressed(false);
			Action trackChanged = this.TrackChanged;
			if (trackChanged != null)
			{
				trackChanged();
			}
			Action stateChanged = this.StateChanged;
			if (stateChanged == null)
			{
				return;
			}
			stateChanged();
		}

		// Token: 0x06009A6C RID: 39532 RVA: 0x003F2CD3 File Offset: 0x003F0ED3
		public void ToggleShuffle()
		{
			this._shuffle = !this._shuffle;
			Action stateChanged = this.StateChanged;
			if (stateChanged == null)
			{
				return;
			}
			stateChanged();
		}

		// Token: 0x06009A6D RID: 39533 RVA: 0x003F2CF4 File Offset: 0x003F0EF4
		public void CycleRepeat()
		{
			this._repeat = ((this._repeat == CielOSAudioService.RepeatMode.Off) ? CielOSAudioService.RepeatMode.RepeatOne : CielOSAudioService.RepeatMode.Off);
			Action stateChanged = this.StateChanged;
			if (stateChanged == null)
			{
				return;
			}
			stateChanged();
		}

		// Token: 0x06009A6E RID: 39534 RVA: 0x003F2D18 File Offset: 0x003F0F18
		private int RandomIndex()
		{
			if (this._tracks.Count <= 1)
			{
				return 0;
			}
			int num;
			do
			{
				num = this._rng.Next(this._tracks.Count);
			}
			while (num == this._index);
			return num;
		}

		// Token: 0x06009A6F RID: 39535 RVA: 0x003F2D58 File Offset: 0x003F0F58
		private void OnTrackFinished()
		{
			if (this._tracks.Count == 0)
			{
				this.SetHomeMusicSuppressed(false);
				return;
			}
			if (this._repeat == CielOSAudioService.RepeatMode.RepeatOne)
			{
				this.PlayIndex(this._index);
				return;
			}
			if (this._shuffle)
			{
				this.PlayIndex(this.RandomIndex());
				return;
			}
			if (this._index + 1 < this._tracks.Count)
			{
				this.PlayIndex(this._index + 1);
				return;
			}
			this.Stop();
		}

		// Token: 0x06009A70 RID: 39536 RVA: 0x003F2DCF File Offset: 0x003F0FCF
		private IEnumerator LoadAndPlay(string virtualPath)
		{
			string text = CielOSFileSystem.Instance.ToReal(virtualPath);
			AudioType audioType = CielOSAudioService.GuessType(text);
			string absoluteUri;
			try
			{
				absoluteUri = new Uri(text).AbsoluteUri;
			}
			catch (UriFormatException ex)
			{
				Debug.LogWarning("[CielOSAudioService] Invalid audio path: " + ex.Message);
				this._loadCo = null;
				this._loading = false;
				this._wasPlaying = this._audio != null && this._audio.isPlaying;
				this.SetHomeMusicSuppressed(this._audio != null && this._audio.isPlaying);
				Action stateChanged = this.StateChanged;
				if (stateChanged != null)
				{
					stateChanged();
				}
				yield break;
			}
			using (UnityWebRequest req = UnityWebRequestMultimedia.GetAudioClip(absoluteUri, audioType))
			{
				DownloadHandlerAudioClip downloadHandlerAudioClip = req.downloadHandler as DownloadHandlerAudioClip;
				if (downloadHandlerAudioClip != null)
				{
					downloadHandlerAudioClip.streamAudio = true;
				}
				yield return req.SendWebRequest();
				if (req.result != UnityWebRequest.Result.Success)
				{
					Debug.LogWarning("[CielOSAudioService] Failed: " + req.error);
					this._loadCo = null;
					this._loading = false;
					this._wasPlaying = this._audio != null && this._audio.isPlaying;
					this.SetHomeMusicSuppressed(this._audio != null && this._audio.isPlaying);
					Action stateChanged2 = this.StateChanged;
					if (stateChanged2 != null)
					{
						stateChanged2();
					}
					yield break;
				}
				AudioClip content = DownloadHandlerAudioClip.GetContent(req);
				if (content == null)
				{
					this._loadCo = null;
					this._loading = false;
					this._wasPlaying = this._audio != null && this._audio.isPlaying;
					this.SetHomeMusicSuppressed(this._audio != null && this._audio.isPlaying);
					Action stateChanged3 = this.StateChanged;
					if (stateChanged3 != null)
					{
						stateChanged3();
					}
					yield break;
				}
				if (this._audio.isPlaying)
				{
					this._audio.Stop();
				}
				this.ReleaseRuntimeClip();
				this._runtimeClip = content;
				this._audio.clip = content;
				this._audio.Play();
				this._paused = false;
				this._wasPlaying = this._audio.isPlaying;
				this.SetHomeMusicSuppressed(this._audio.isPlaying);
				this._loadCo = null;
				this._loading = false;
				Action stateChanged4 = this.StateChanged;
				if (stateChanged4 != null)
				{
					stateChanged4();
				}
			}
			UnityWebRequest req = null;
			yield break;
			yield break;
		}

		// Token: 0x06009A71 RID: 39537 RVA: 0x003F2DE8 File Offset: 0x003F0FE8
		private void OnDestroy()
		{
			SceneManager.sceneLoaded -= this.HandleSceneLoaded;
			this.SetHomeMusicSuppressed(false);
			if (this._audio != null)
			{
				PlayerPrefs.SetFloat("CielOS.Volume", this._audio.volume);
			}
			this.ReleaseRuntimeClip();
			if (CielOSAudioService.Instance == this)
			{
				CielOSAudioService.Instance = null;
			}
		}

		// Token: 0x06009A72 RID: 39538 RVA: 0x003F2E49 File Offset: 0x003F1049
		private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			this.AttachToPersistentPlayer();
			this._homeAudio = null;
			if (this._audio != null && this._audio.isPlaying)
			{
				this.SetHomeMusicSuppressed(true);
			}
		}

		// Token: 0x06009A73 RID: 39539 RVA: 0x003F2E7C File Offset: 0x003F107C
		private void AttachToPersistentPlayer()
		{
			Transform player = PlayerRegistry.Player;
			if (player != null && base.transform.parent != player)
			{
				base.transform.SetParent(player, false);
			}
		}

		// Token: 0x06009A74 RID: 39540 RVA: 0x003F2EB8 File Offset: 0x003F10B8
		private void SetHomeMusicSuppressed(bool suppressed)
		{
			if (this._homeMusicSuppressed == suppressed && (this._homeAudio != null || !suppressed))
			{
				return;
			}
			this._homeMusicSuppressed = suppressed;
			if (this._homeAudio == null)
			{
				this._homeAudio = Object.FindObjectOfType<CielHomeAudioController>(true);
			}
			if (this._homeAudio != null)
			{
				this._homeAudio.SetExternalMusicSuppressed(suppressed);
			}
		}

		// Token: 0x06009A75 RID: 39541 RVA: 0x003F2F1B File Offset: 0x003F111B
		private void ReleaseRuntimeClip()
		{
			if (this._runtimeClip == null)
			{
				return;
			}
			if (Application.isPlaying)
			{
				Object.Destroy(this._runtimeClip);
			}
			else
			{
				Object.DestroyImmediate(this._runtimeClip);
			}
			this._runtimeClip = null;
		}

		// Token: 0x06009A76 RID: 39542 RVA: 0x003F2F54 File Offset: 0x003F1154
		private static AudioType GuessType(string path)
		{
			string text = path.ToLowerInvariant();
			if (text.EndsWith(".wav"))
			{
				return AudioType.WAV;
			}
			if (text.EndsWith(".ogg"))
			{
				return AudioType.OGGVORBIS;
			}
			if (text.EndsWith(".mp3"))
			{
				return AudioType.MPEG;
			}
			return AudioType.UNKNOWN;
		}

		// Token: 0x04009F34 RID: 40756
		private AudioSource _audio;

		// Token: 0x04009F35 RID: 40757
		private AudioClip _runtimeClip;

		// Token: 0x04009F36 RID: 40758
		private readonly List<string> _tracks = new List<string>();

		// Token: 0x04009F37 RID: 40759
		private int _index = -1;

		// Token: 0x04009F38 RID: 40760
		private Coroutine _loadCo;

		// Token: 0x04009F39 RID: 40761
		private bool _shuffle;

		// Token: 0x04009F3A RID: 40762
		private CielOSAudioService.RepeatMode _repeat;

		// Token: 0x04009F3B RID: 40763
		private Random _rng = new Random(12345);

		// Token: 0x04009F3C RID: 40764
		private CielHomeAudioController _homeAudio;

		// Token: 0x04009F3D RID: 40765
		private bool _homeMusicSuppressed;

		// Token: 0x04009F3E RID: 40766
		private bool _paused;

		// Token: 0x04009F3F RID: 40767
		private bool _loading;

		// Token: 0x04009F40 RID: 40768
		private readonly HashSet<int> _playbackRetentionOwners = new HashSet<int>();

		// Token: 0x04009F41 RID: 40769
		public const string MusicDir = "/Music";

		// Token: 0x04009F44 RID: 40772
		private bool _wasPlaying;

		// Token: 0x02001379 RID: 4985
		public enum RepeatMode
		{
			// Token: 0x04009F46 RID: 40774
			Off,
			// Token: 0x04009F47 RID: 40775
			RepeatOne
		}
	}
}
