using System;
using Features.MainMenu;
using ProjectExhibitionist.PoiTravel;
using UnityEngine;
using UnityEngine.AzureSky;
using UnityEngine.SceneManagement;

namespace Features.Audio
{
	// Token: 0x02001526 RID: 5414
	public class AmbientAudioController : MonoBehaviour
	{
		// Token: 0x0600A788 RID: 42888 RVA: 0x00460B0C File Offset: 0x0045ED0C
		private void Awake()
		{
			this._daySource = this.CreateAudioSource("DayAmbient");
			this._nightSource = this.CreateAudioSource("NightAmbient");
		}

		// Token: 0x0600A789 RID: 42889 RVA: 0x00460B30 File Offset: 0x0045ED30
		private void OnEnable()
		{
			SceneManager.activeSceneChanged += this.HandleActiveSceneChanged;
			this.RefreshSceneGain(SceneManager.GetActiveScene(), true);
		}

		// Token: 0x0600A78A RID: 42890 RVA: 0x00460B4F File Offset: 0x0045ED4F
		private void OnDisable()
		{
			SceneManager.activeSceneChanged -= this.HandleActiveSceneChanged;
		}

		// Token: 0x0600A78B RID: 42891 RVA: 0x00460B64 File Offset: 0x0045ED64
		private AudioSource CreateAudioSource(string name)
		{
			GameObject gameObject = new GameObject(name);
			gameObject.transform.SetParent(base.transform);
			AudioSource audioSource = gameObject.AddComponent<AudioSource>();
			audioSource.playOnAwake = false;
			audioSource.loop = true;
			audioSource.spatialBlend = 0f;
			audioSource.volume = 0f;
			GameAudioRouter.Route(audioSource, GameAudioRouter.Channel.Sfx);
			return audioSource;
		}

		// Token: 0x0600A78C RID: 42892 RVA: 0x00460BB8 File Offset: 0x0045EDB8
		private void Start()
		{
			if (this.azureSystem == null)
			{
				this.azureSystem = Object.FindObjectOfType<AzureCoreSystem>();
			}
			if (this.dayAmbient != null)
			{
				this._daySource.clip = this.dayAmbient;
				this._daySource.Play();
			}
			if (this.nightAmbient != null)
			{
				this._nightSource.clip = this.nightAmbient;
				this._nightSource.Play();
			}
			this.UpdateTimeState(true);
		}

		// Token: 0x0600A78D RID: 42893 RVA: 0x00460C39 File Offset: 0x0045EE39
		private void Update()
		{
			this.UpdateTimeState(false);
			this.UpdateSceneGain();
			this.UpdateVolumes();
		}

		// Token: 0x0600A78E RID: 42894 RVA: 0x00460C4E File Offset: 0x0045EE4E
		private void HandleActiveSceneChanged(Scene _, Scene current)
		{
			this.RefreshSceneGain(current, false);
		}

		// Token: 0x0600A78F RID: 42895 RVA: 0x00460C58 File Offset: 0x0045EE58
		private void RefreshSceneGain(Scene scene, bool immediate)
		{
			this._sceneGainTarget = AmbientAudioController.EvaluateSceneGain(scene.IsValid() ? scene.name : string.Empty, this.poiVolumeMultiplier);
			if (immediate)
			{
				this._sceneGain = this._sceneGainTarget;
			}
		}

		// Token: 0x0600A790 RID: 42896 RVA: 0x00460C91 File Offset: 0x0045EE91
		private void UpdateSceneGain()
		{
			if (this.sceneFadeSeconds <= 0f)
			{
				this._sceneGain = this._sceneGainTarget;
				return;
			}
			this._sceneGain = Mathf.MoveTowards(this._sceneGain, this._sceneGainTarget, Time.unscaledDeltaTime / this.sceneFadeSeconds);
		}

		// Token: 0x0600A791 RID: 42897 RVA: 0x00460CD0 File Offset: 0x0045EED0
		private void UpdateTimeState(bool immediate)
		{
			float currentHour = this.GetCurrentHour();
			AmbientAudioController.TimeState timeState;
			float num3;
			if (this.IsNightTime(currentHour))
			{
				float num = this.nightStartHour;
				float num2 = this.nightStartHour + this.crossfadeDuration;
				if (currentHour >= num && currentHour < num2)
				{
					timeState = AmbientAudioController.TimeState.TransitionToNight;
					num3 = (currentHour - num) / this.crossfadeDuration;
				}
				else
				{
					timeState = AmbientAudioController.TimeState.Night;
					num3 = 1f;
				}
			}
			else
			{
				float num4 = this.dayStartHour;
				float num5 = this.dayStartHour + this.crossfadeDuration;
				if (currentHour >= num4 && currentHour < num5)
				{
					timeState = AmbientAudioController.TimeState.TransitionToDay;
					num3 = (currentHour - num4) / this.crossfadeDuration;
				}
				else
				{
					timeState = AmbientAudioController.TimeState.Day;
					num3 = 0f;
				}
			}
			if (immediate)
			{
				this._currentState = timeState;
				this._transitionProgress = num3;
			}
			else
			{
				this._currentState = timeState;
				this._transitionProgress = num3;
			}
			if (this.showDebugInfo && Time.frameCount % 120 == 0)
			{
				Debug.Log(string.Format("[AmbientAudio] Hour: {0:F1} | State: {1} | Progress: {2:F2}", currentHour, this._currentState, this._transitionProgress));
			}
		}

		// Token: 0x0600A792 RID: 42898 RVA: 0x00460DC4 File Offset: 0x0045EFC4
		private void UpdateVolumes()
		{
			float num = 0f;
			float num2 = 0f;
			switch (this._currentState)
			{
			case AmbientAudioController.TimeState.Day:
				num = 1f;
				num2 = 0f;
				break;
			case AmbientAudioController.TimeState.TransitionToNight:
				num = 1f - this._transitionProgress;
				num2 = this._transitionProgress;
				break;
			case AmbientAudioController.TimeState.Night:
				num = 0f;
				num2 = 1f;
				break;
			case AmbientAudioController.TimeState.TransitionToDay:
				num = this._transitionProgress;
				num2 = 1f - this._transitionProgress;
				break;
			}
			this._daySource.volume = num * this.dayVolume * this.masterVolume * this._sceneGain;
			this._nightSource.volume = num2 * this.nightVolume * this.masterVolume * this._sceneGain;
		}

		// Token: 0x0600A793 RID: 42899 RVA: 0x00460E84 File Offset: 0x0045F084
		public static float EvaluateSceneGain(string sceneName, float poiMultiplier)
		{
			if (!PoiTravelState.IsPoiSceneName(sceneName))
			{
				return 1f;
			}
			return Mathf.Clamp01(poiMultiplier);
		}

		// Token: 0x0600A794 RID: 42900 RVA: 0x00460E9C File Offset: 0x0045F09C
		private float GetCurrentHour()
		{
			if (this.azureSystem != null)
			{
				return this.azureSystem.timeSystem.timeOfDay;
			}
			return (float)DateTime.Now.Hour + (float)DateTime.Now.Minute / 60f;
		}

		// Token: 0x0600A795 RID: 42901 RVA: 0x00460EEB File Offset: 0x0045F0EB
		private bool IsNightTime(float hour)
		{
			if (this.nightStartHour > this.dayStartHour)
			{
				return hour >= this.nightStartHour || hour < this.dayStartHour;
			}
			return hour >= this.nightStartHour && hour < this.dayStartHour;
		}

		// Token: 0x0600A796 RID: 42902 RVA: 0x00460F24 File Offset: 0x0045F124
		public void SetMasterVolume(float volume)
		{
			this.masterVolume = Mathf.Clamp01(volume);
		}

		// Token: 0x0600A797 RID: 42903 RVA: 0x00460F32 File Offset: 0x0045F132
		public void SetDayVolume(float volume)
		{
			this.dayVolume = Mathf.Clamp01(volume);
		}

		// Token: 0x0600A798 RID: 42904 RVA: 0x00460F40 File Offset: 0x0045F140
		public void SetNightVolume(float volume)
		{
			this.nightVolume = Mathf.Clamp01(volume);
		}

		// Token: 0x0600A799 RID: 42905 RVA: 0x00460F4E File Offset: 0x0045F14E
		public void SetDayAmbient(AudioClip clip)
		{
			this.dayAmbient = clip;
			bool isPlaying = this._daySource.isPlaying;
			this._daySource.clip = clip;
			if (isPlaying && clip != null)
			{
				this._daySource.Play();
			}
		}

		// Token: 0x0600A79A RID: 42906 RVA: 0x00460F84 File Offset: 0x0045F184
		public void SetNightAmbient(AudioClip clip)
		{
			this.nightAmbient = clip;
			bool isPlaying = this._nightSource.isPlaying;
			this._nightSource.clip = clip;
			if (isPlaying && clip != null)
			{
				this._nightSource.Play();
			}
		}

		// Token: 0x170019CE RID: 6606
		// (get) Token: 0x0600A79B RID: 42907 RVA: 0x00460FBA File Offset: 0x0045F1BA
		public float MasterVolume
		{
			get
			{
				return this.masterVolume;
			}
		}

		// Token: 0x170019CF RID: 6607
		// (get) Token: 0x0600A79C RID: 42908 RVA: 0x00460FC2 File Offset: 0x0045F1C2
		public bool IsNight
		{
			get
			{
				return this._currentState == AmbientAudioController.TimeState.Night || this._currentState == AmbientAudioController.TimeState.TransitionToNight;
			}
		}

		// Token: 0x0400AC41 RID: 44097
		[Header("Audio Clips")]
		[SerializeField]
		private AudioClip dayAmbient;

		// Token: 0x0400AC42 RID: 44098
		[SerializeField]
		private AudioClip nightAmbient;

		// Token: 0x0400AC43 RID: 44099
		[Header("Volume")]
		[SerializeField]
		[Range(0f, 1f)]
		private float masterVolume = 0.5f;

		// Token: 0x0400AC44 RID: 44100
		[SerializeField]
		[Range(0f, 1f)]
		private float dayVolume = 1f;

		// Token: 0x0400AC45 RID: 44101
		[SerializeField]
		[Range(0f, 1f)]
		private float nightVolume = 0.176f;

		// Token: 0x0400AC46 RID: 44102
		[Header("Scene Context")]
		[Tooltip("Outdoor ambience multiplier while the active gameplay scene is a POI interior.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float poiVolumeMultiplier = 0.05f;

		// Token: 0x0400AC47 RID: 44103
		[SerializeField]
		[Min(0f)]
		private float sceneFadeSeconds = 0.35f;

		// Token: 0x0400AC48 RID: 44104
		[Header("Time Settings")]
		[Tooltip("Hour when night starts (e.g., 18.5 = 6:30 PM)")]
		[SerializeField]
		private float nightStartHour = 18.5f;

		// Token: 0x0400AC49 RID: 44105
		[Tooltip("Hour when day starts (e.g., 6 = 6:00 AM)")]
		[SerializeField]
		private float dayStartHour = 6f;

		// Token: 0x0400AC4A RID: 44106
		[Tooltip("Duration of crossfade in game hours")]
		[SerializeField]
		private float crossfadeDuration = 1f;

		// Token: 0x0400AC4B RID: 44107
		[Header("References")]
		[SerializeField]
		private AzureCoreSystem azureSystem;

		// Token: 0x0400AC4C RID: 44108
		[Header("Debug")]
		[SerializeField]
		private bool showDebugInfo;

		// Token: 0x0400AC4D RID: 44109
		private AudioSource _daySource;

		// Token: 0x0400AC4E RID: 44110
		private AudioSource _nightSource;

		// Token: 0x0400AC4F RID: 44111
		private AmbientAudioController.TimeState _currentState;

		// Token: 0x0400AC50 RID: 44112
		private float _transitionProgress;

		// Token: 0x0400AC51 RID: 44113
		private float _sceneGain = 1f;

		// Token: 0x0400AC52 RID: 44114
		private float _sceneGainTarget = 1f;

		// Token: 0x02001527 RID: 5415
		private enum TimeState
		{
			// Token: 0x0400AC54 RID: 44116
			Day,
			// Token: 0x0400AC55 RID: 44117
			TransitionToNight,
			// Token: 0x0400AC56 RID: 44118
			Night,
			// Token: 0x0400AC57 RID: 44119
			TransitionToDay
		}
	}
}
