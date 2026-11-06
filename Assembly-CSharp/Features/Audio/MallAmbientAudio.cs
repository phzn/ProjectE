using System;
using System.Collections;
using Features.CielVoice;
using Features.MainMenu;
using Features.TimeFreeze;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.Audio
{
	// Token: 0x02001528 RID: 5416
	[DisallowMultipleComponent]
	public sealed class MallAmbientAudio : MonoBehaviour
	{
		// Token: 0x170019D0 RID: 6608
		// (get) Token: 0x0600A79E RID: 42910 RVA: 0x00461059 File Offset: 0x0045F259
		public float ArrivalVoiceDuckGain
		{
			get
			{
				return this._arrivalDuckGain;
			}
		}

		// Token: 0x170019D1 RID: 6609
		// (get) Token: 0x0600A79F RID: 42911 RVA: 0x00461061 File Offset: 0x0045F261
		public bool IsArrivalVoiceDucking
		{
			get
			{
				return this._arrivalDuckTarget < 0.999f || this._arrivalDuckGain < 0.999f;
			}
		}

		// Token: 0x0600A7A0 RID: 42912 RVA: 0x0046107F File Offset: 0x0045F27F
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			SceneManager.sceneLoaded -= MallAmbientAudio.HandleSceneLoaded;
		}

		// Token: 0x0600A7A1 RID: 42913 RVA: 0x00461092 File Offset: 0x0045F292
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Register()
		{
			SceneManager.sceneLoaded -= MallAmbientAudio.HandleSceneLoaded;
			SceneManager.sceneLoaded += MallAmbientAudio.HandleSceneLoaded;
		}

		// Token: 0x0600A7A2 RID: 42914 RVA: 0x004610B6 File Offset: 0x0045F2B6
		private static void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			MallAmbientAudio.InstallForScene(scene);
		}

		// Token: 0x0600A7A3 RID: 42915 RVA: 0x004610C0 File Offset: 0x0045F2C0
		private static MallAmbientAudio InstallForScene(Scene scene)
		{
			if (!scene.IsValid() || !scene.isLoaded || scene.name != "POI - Mall")
			{
				return null;
			}
			GameObject[] rootGameObjects = scene.GetRootGameObjects();
			for (int i = 0; i < rootGameObjects.Length; i++)
			{
				MallAmbientAudio componentInChildren = rootGameObjects[i].GetComponentInChildren<MallAmbientAudio>(true);
				if (componentInChildren != null)
				{
					return componentInChildren;
				}
			}
			MallAmbientAudio mallAmbientAudio = Resources.Load<MallAmbientAudio>("Audio/MallAmbience");
			if (mallAmbientAudio == null)
			{
				Debug.LogWarning("[MallAmbientAudio] Missing Resources/Audio/MallAmbience");
				return null;
			}
			MallAmbientAudio mallAmbientAudio2 = Object.Instantiate<MallAmbientAudio>(mallAmbientAudio);
			mallAmbientAudio2.name = "MallAmbience";
			SceneManager.MoveGameObjectToScene(mallAmbientAudio2.gameObject, scene);
			return mallAmbientAudio2;
		}

		// Token: 0x0600A7A4 RID: 42916 RVA: 0x0046115C File Offset: 0x0045F35C
		private void Awake()
		{
			this.EnsureInitialized();
		}

		// Token: 0x0600A7A5 RID: 42917 RVA: 0x00461165 File Offset: 0x0045F365
		private void OnEnable()
		{
			this.MaintainVoiceBinding();
		}

		// Token: 0x0600A7A6 RID: 42918 RVA: 0x0046116D File Offset: 0x0045F36D
		private void LateUpdate()
		{
			this.Tick(Time.unscaledDeltaTime);
		}

		// Token: 0x0600A7A7 RID: 42919 RVA: 0x0046117C File Offset: 0x0045F37C
		private void Tick(float deltaTime)
		{
			if (base.gameObject.scene.name != "POI - Mall" || base.gameObject.scene != SceneManager.GetActiveScene())
			{
				this.StopPlayback();
				return;
			}
			this.MaintainVoiceBinding();
			this.AdvanceVoiceDuck(deltaTime);
			if (!this.EnsureInitialized())
			{
				return;
			}
			if (TimeFreezeController.IsAnyActive)
			{
				this._resumeAfterWorldFreeze = true;
				return;
			}
			if (AudioListener.pause)
			{
				return;
			}
			if (this._resumeAfterWorldFreeze)
			{
				if (this._loop.IsPlaying)
				{
					this._loop.Pause();
					this._loop.Resume();
				}
				this._resumeAfterWorldFreeze = false;
			}
			if (!this._loop.IsPlaying)
			{
				this._loop.Play();
			}
			this._level = Mathf.MoveTowards(this._level, 1f, deltaTime / Mathf.Max(0.01f, this.fadeInSeconds));
			this._loop.Tick(Mathf.Clamp01(this.volume) * this._level * this._arrivalDuckGain);
		}

		// Token: 0x0600A7A8 RID: 42920 RVA: 0x0046128C File Offset: 0x0045F48C
		private bool EnsureInitialized()
		{
			if (this._loop != null)
			{
				return true;
			}
			if (this.ambientClip == null)
			{
				return false;
			}
			this._sourceA = this.CreateSource("MallBackgroundA");
			this._sourceB = this.CreateSource("MallBackgroundB");
			this._loop = new CrossfadeLoopPlayer(this._sourceA, this._sourceB, this.ambientClip, this.loopCrossfade, this.loopStart, this.loopEnd);
			return true;
		}

		// Token: 0x0600A7A9 RID: 42921 RVA: 0x00461308 File Offset: 0x0045F508
		private AudioSource CreateSource(string sourceName)
		{
			GameObject gameObject = new GameObject(sourceName);
			gameObject.transform.SetParent(base.transform, false);
			AudioSource audioSource = gameObject.AddComponent<AudioSource>();
			audioSource.playOnAwake = false;
			audioSource.loop = false;
			audioSource.spatialBlend = 0f;
			audioSource.dopplerLevel = 0f;
			audioSource.volume = 0f;
			audioSource.ignoreListenerPause = false;
			GameAudioRouter.Route(audioSource, GameAudioRouter.Channel.Sfx);
			return audioSource;
		}

		// Token: 0x0600A7AA RID: 42922 RVA: 0x0046136F File Offset: 0x0045F56F
		private void StopPlayback()
		{
			CrossfadeLoopPlayer loop = this._loop;
			if (loop != null)
			{
				loop.Stop();
			}
			this._level = 0f;
			this._resumeAfterWorldFreeze = false;
		}

		// Token: 0x0600A7AB RID: 42923 RVA: 0x00461394 File Offset: 0x0045F594
		private void OnDisable()
		{
			this.CancelPendingRestore();
			this.UnbindVoiceController();
			this._arrivalDuckGain = (this._arrivalDuckTarget = 1f);
			this._arrivalDuckSpeed = 0f;
			this.StopPlayback();
		}

		// Token: 0x0600A7AC RID: 42924 RVA: 0x004613D4 File Offset: 0x0045F5D4
		private void MaintainVoiceBinding()
		{
			CielVoiceController instance = CielVoiceController.Instance;
			if (instance == null)
			{
				this.UnbindVoiceController();
				if (this._arrivalDuckTarget < 1f)
				{
					this.CancelPendingRestore();
					this.SetArrivalDuckTarget(1f, this.arrivalVoiceRestore);
				}
				return;
			}
			if (this._voiceController == instance)
			{
				return;
			}
			this.CancelPendingRestore();
			this.UnbindVoiceController();
			this._voiceController = instance;
			instance.OnVoiceStarted += this.HandleVoiceStarted;
			instance.OnVoiceEnded += this.HandleVoiceEnded;
			if (instance.IsSpeaking && MallAmbientAudio.IsMallArrivalVoice(instance.CurrentVoice))
			{
				this.BeginArrivalVoiceDuck();
				return;
			}
			if (this._arrivalDuckTarget < 1f)
			{
				this.SetArrivalDuckTarget(1f, this.arrivalVoiceRestore);
			}
		}

		// Token: 0x0600A7AD RID: 42925 RVA: 0x0046149C File Offset: 0x0045F69C
		private void UnbindVoiceController()
		{
			if (this._voiceController != null)
			{
				this._voiceController.OnVoiceStarted -= this.HandleVoiceStarted;
				this._voiceController.OnVoiceEnded -= this.HandleVoiceEnded;
			}
			this._voiceController = null;
		}

		// Token: 0x0600A7AE RID: 42926 RVA: 0x004614EC File Offset: 0x0045F6EC
		private void HandleVoiceStarted(CielVoiceSO voice)
		{
			if (MallAmbientAudio.IsMallArrivalVoice(voice))
			{
				this.BeginArrivalVoiceDuck();
			}
		}

		// Token: 0x0600A7AF RID: 42927 RVA: 0x004614FC File Offset: 0x0045F6FC
		private void BeginArrivalVoiceDuck()
		{
			this.CancelPendingRestore();
			this.SetArrivalDuckTarget(this.arrivalVoiceAmbientMultiplier, this.arrivalVoiceDuckIn);
		}

		// Token: 0x0600A7B0 RID: 42928 RVA: 0x00461516 File Offset: 0x0045F716
		private void HandleVoiceEnded(CielVoiceSO voice)
		{
			if (!MallAmbientAudio.IsMallArrivalVoice(voice))
			{
				return;
			}
			this.CancelPendingRestore();
			this._voiceRestoreRoutine = base.StartCoroutine(this.RestoreAfterVoicePlayback());
		}

		// Token: 0x0600A7B1 RID: 42929 RVA: 0x00461539 File Offset: 0x0045F739
		private IEnumerator RestoreAfterVoicePlayback()
		{
			yield return null;
			while (this._voiceController != null && this._voiceController.CurrentVoice == null && this._voiceController.VoiceSource != null && this._voiceController.VoiceSource.isPlaying)
			{
				yield return null;
			}
			this.SetArrivalDuckTarget(1f, this.arrivalVoiceRestore);
			this._voiceRestoreRoutine = null;
			yield break;
		}

		// Token: 0x0600A7B2 RID: 42930 RVA: 0x00461548 File Offset: 0x0045F748
		private void CancelPendingRestore()
		{
			if (this._voiceRestoreRoutine == null)
			{
				return;
			}
			base.StopCoroutine(this._voiceRestoreRoutine);
			this._voiceRestoreRoutine = null;
		}

		// Token: 0x0600A7B3 RID: 42931 RVA: 0x00461566 File Offset: 0x0045F766
		private void SetArrivalDuckTarget(float target, float duration)
		{
			this._arrivalDuckTarget = Mathf.Clamp01(target);
			this._arrivalDuckSpeed = Mathf.Abs(this._arrivalDuckTarget - this._arrivalDuckGain) / Mathf.Max(0.01f, duration);
		}

		// Token: 0x0600A7B4 RID: 42932 RVA: 0x00461598 File Offset: 0x0045F798
		private void AdvanceVoiceDuck(float deltaTime)
		{
			this._arrivalDuckGain = Mathf.MoveTowards(this._arrivalDuckGain, this._arrivalDuckTarget, this._arrivalDuckSpeed * Mathf.Max(0f, deltaTime));
		}

		// Token: 0x0600A7B5 RID: 42933 RVA: 0x004615C3 File Offset: 0x0045F7C3
		private static bool IsMallArrivalVoice(CielVoiceSO voice)
		{
			return voice != null && voice.ResolvedId.StartsWith("poi_mall_", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x0400AC58 RID: 44120
		private const string MallSceneName = "POI - Mall";

		// Token: 0x0400AC59 RID: 44121
		private const string PrefabResourcePath = "Audio/MallAmbience";

		// Token: 0x0400AC5A RID: 44122
		[Header("Background recording")]
		[SerializeField]
		private AudioClip ambientClip;

		// Token: 0x0400AC5B RID: 44123
		[SerializeField]
		[Range(0f, 1f)]
		private float volume = 0.1f;

		// Token: 0x0400AC5C RID: 44124
		[SerializeField]
		[Min(0f)]
		private float loopStart = 0.25f;

		// Token: 0x0400AC5D RID: 44125
		[SerializeField]
		[Min(0f)]
		private float loopEnd = 119.8f;

		// Token: 0x0400AC5E RID: 44126
		[SerializeField]
		[Min(0.02f)]
		private float loopCrossfade = 1.5f;

		// Token: 0x0400AC5F RID: 44127
		[SerializeField]
		[Min(0.01f)]
		private float fadeInSeconds = 1.2f;

		// Token: 0x0400AC60 RID: 44128
		[Header("Ciel arrival voice")]
		[SerializeField]
		[Range(0f, 1f)]
		private float arrivalVoiceAmbientMultiplier = 0.1f;

		// Token: 0x0400AC61 RID: 44129
		[SerializeField]
		[Min(0.01f)]
		private float arrivalVoiceDuckIn = 0.12f;

		// Token: 0x0400AC62 RID: 44130
		[SerializeField]
		[Min(0.05f)]
		private float arrivalVoiceRestore = 2.8f;

		// Token: 0x0400AC63 RID: 44131
		private AudioSource _sourceA;

		// Token: 0x0400AC64 RID: 44132
		private AudioSource _sourceB;

		// Token: 0x0400AC65 RID: 44133
		private CrossfadeLoopPlayer _loop;

		// Token: 0x0400AC66 RID: 44134
		private float _level;

		// Token: 0x0400AC67 RID: 44135
		private CielVoiceController _voiceController;

		// Token: 0x0400AC68 RID: 44136
		private Coroutine _voiceRestoreRoutine;

		// Token: 0x0400AC69 RID: 44137
		private float _arrivalDuckGain = 1f;

		// Token: 0x0400AC6A RID: 44138
		private float _arrivalDuckTarget = 1f;

		// Token: 0x0400AC6B RID: 44139
		private float _arrivalDuckSpeed;

		// Token: 0x0400AC6C RID: 44140
		private bool _resumeAfterWorldFreeze;
	}
}
