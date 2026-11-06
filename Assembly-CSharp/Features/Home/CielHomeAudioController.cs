using System;
using System.Collections;
using Features.CielVoice;
using Features.MainMenu;
using UnityEngine;

namespace Features.Home
{
	// Token: 0x02001187 RID: 4487
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(-60)]
	public sealed class CielHomeAudioController : MonoBehaviour
	{
		// Token: 0x170015E9 RID: 5609
		// (get) Token: 0x0600896B RID: 35179 RVA: 0x0038328E File Offset: 0x0038148E
		public bool IsExternalMusicSuppressed
		{
			get
			{
				return this._externalMusicSuppressed;
			}
		}

		// Token: 0x0600896C RID: 35180 RVA: 0x00383298 File Offset: 0x00381498
		private void Awake()
		{
			this._musicSourceA = this.CreateSource("MainHomeMusic A", GameAudioRouter.Channel.Music);
			this._musicSourceB = this.CreateSource("MainHomeMusic B", GameAudioRouter.Channel.Music);
			this._wakeUpSource = this.CreateSource("WakeUpFromBed SFX", GameAudioRouter.Channel.Sfx);
			this._wakeUpSource.volume = 1f;
			this._wakeUpSource.priority = 128;
			if (this.wakeUpFromBed != null && this.wakeUpFromBed.loadState == AudioDataLoadState.Unloaded)
			{
				this.wakeUpFromBed.LoadAudioData();
			}
			this._musicLoop = new CrossfadeLoopPlayer(this._musicSourceA, this._musicSourceB, this.mainHomeMusic, this.loopCrossfadeSeconds, this.loopStartSeconds, this.loopEndSeconds);
		}

		// Token: 0x0600896D RID: 35181 RVA: 0x00383351 File Offset: 0x00381551
		private void OnEnable()
		{
			if (this._musicLoop != null)
			{
				this._startMusicRoutine = base.StartCoroutine(this.StartMusicWhenReady());
			}
		}

		// Token: 0x0600896E RID: 35182 RVA: 0x0038336D File Offset: 0x0038156D
		private void Update()
		{
			CrossfadeLoopPlayer musicLoop = this._musicLoop;
			if (musicLoop == null)
			{
				return;
			}
			musicLoop.Tick(this.musicVolume);
		}

		// Token: 0x0600896F RID: 35183 RVA: 0x00383385 File Offset: 0x00381585
		private void OnDisable()
		{
			if (this._startMusicRoutine != null)
			{
				base.StopCoroutine(this._startMusicRoutine);
				this._startMusicRoutine = null;
			}
			CrossfadeLoopPlayer musicLoop = this._musicLoop;
			if (musicLoop != null)
			{
				musicLoop.Stop();
			}
			AudioSource wakeUpSource = this._wakeUpSource;
			if (wakeUpSource == null)
			{
				return;
			}
			wakeUpSource.Stop();
		}

		// Token: 0x06008970 RID: 35184 RVA: 0x003833C4 File Offset: 0x003815C4
		public void PlayWakeUpFromBed()
		{
			if (this._wakeUpSource == null || this.wakeUpFromBed == null)
			{
				return;
			}
			if (this.wakeUpFromBed.loadState == AudioDataLoadState.Unloaded)
			{
				this.wakeUpFromBed.LoadAudioData();
			}
			this._wakeUpSource.PlayOneShot(this.wakeUpFromBed, this.wakeUpVolume);
		}

		// Token: 0x06008971 RID: 35185 RVA: 0x00383420 File Offset: 0x00381620
		public void SetExternalMusicSuppressed(bool suppressed)
		{
			if (this._externalMusicSuppressed == suppressed)
			{
				return;
			}
			this._externalMusicSuppressed = suppressed;
			if (this._musicLoop == null)
			{
				return;
			}
			if (suppressed)
			{
				this._musicLoop.Pause();
				return;
			}
			if (base.isActiveAndEnabled && this.mainHomeMusic != null && this.mainHomeMusic.loadState == AudioDataLoadState.Loaded)
			{
				this._musicLoop.Resume();
			}
		}

		// Token: 0x06008972 RID: 35186 RVA: 0x00383485 File Offset: 0x00381685
		private IEnumerator StartMusicWhenReady()
		{
			if (this.mainHomeMusic == null)
			{
				yield break;
			}
			if (this.mainHomeMusic.loadState == AudioDataLoadState.Unloaded)
			{
				this.mainHomeMusic.LoadAudioData();
			}
			while (base.isActiveAndEnabled && this.mainHomeMusic.loadState == AudioDataLoadState.Loading)
			{
				yield return null;
			}
			if (base.isActiveAndEnabled && !this._externalMusicSuppressed && this.mainHomeMusic.loadState == AudioDataLoadState.Loaded)
			{
				this._musicLoop.Play();
			}
			else if (this.mainHomeMusic.loadState == AudioDataLoadState.Failed)
			{
				Debug.LogWarning("[CielHomeAudio] MainHomeMusic failed to load.", this);
			}
			this._startMusicRoutine = null;
			yield break;
		}

		// Token: 0x06008973 RID: 35187 RVA: 0x00383494 File Offset: 0x00381694
		private AudioSource CreateSource(string sourceName, GameAudioRouter.Channel channel)
		{
			GameObject gameObject = new GameObject(sourceName);
			gameObject.transform.SetParent(base.transform, false);
			AudioSource audioSource = gameObject.AddComponent<AudioSource>();
			audioSource.playOnAwake = false;
			audioSource.loop = false;
			audioSource.spatialBlend = 0f;
			audioSource.dopplerLevel = 0f;
			audioSource.volume = 0f;
			GameAudioRouter.Route(audioSource, channel);
			return audioSource;
		}

		// Token: 0x04008BF9 RID: 35833
		public const float DefaultMusicVolume = 0.16f;

		// Token: 0x04008BFA RID: 35834
		public const float DefaultWakeUpVolume = 0.35f;

		// Token: 0x04008BFB RID: 35835
		public const float DefaultLoopStartSeconds = 0.41f;

		// Token: 0x04008BFC RID: 35836
		public const float DefaultLoopEndSeconds = 159.1f;

		// Token: 0x04008BFD RID: 35837
		public const float DefaultLoopCrossfadeSeconds = 0.4f;

		// Token: 0x04008BFE RID: 35838
		[Header("Home music")]
		[SerializeField]
		private AudioClip mainHomeMusic;

		// Token: 0x04008BFF RID: 35839
		[SerializeField]
		[Range(0f, 1f)]
		private float musicVolume = 0.16f;

		// Token: 0x04008C00 RID: 35840
		[Tooltip("The source has a quiet false start before this point; playback begins here.")]
		[SerializeField]
		[Min(0f)]
		private float loopStartSeconds = 0.41f;

		// Token: 0x04008C01 RID: 35841
		[Tooltip("The source fades into several seconds of silence after this point.")]
		[SerializeField]
		[Min(0f)]
		private float loopEndSeconds = 159.1f;

		// Token: 0x04008C02 RID: 35842
		[SerializeField]
		[Min(0.02f)]
		private float loopCrossfadeSeconds = 0.4f;

		// Token: 0x04008C03 RID: 35843
		[Header("Wake-up bed sound")]
		[SerializeField]
		private AudioClip wakeUpFromBed;

		// Token: 0x04008C04 RID: 35844
		[SerializeField]
		[Range(0f, 1f)]
		private float wakeUpVolume = 0.35f;

		// Token: 0x04008C05 RID: 35845
		private AudioSource _musicSourceA;

		// Token: 0x04008C06 RID: 35846
		private AudioSource _musicSourceB;

		// Token: 0x04008C07 RID: 35847
		private AudioSource _wakeUpSource;

		// Token: 0x04008C08 RID: 35848
		private CrossfadeLoopPlayer _musicLoop;

		// Token: 0x04008C09 RID: 35849
		private Coroutine _startMusicRoutine;

		// Token: 0x04008C0A RID: 35850
		private bool _externalMusicSuppressed;
	}
}
