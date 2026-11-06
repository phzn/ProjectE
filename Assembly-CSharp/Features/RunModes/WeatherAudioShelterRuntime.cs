using System;
using System.Collections.Generic;
using ProjectExhibitionist.Core.SceneManagement;
using UnityEngine;
using UnityEngine.AzureSky;
using UnityEngine.SceneManagement;

namespace Features.RunModes
{
	// Token: 0x02000D7D RID: 3453
	[DefaultExecutionOrder(10050)]
	public sealed class WeatherAudioShelterRuntime : MonoBehaviour
	{
		// Token: 0x170010D4 RID: 4308
		// (get) Token: 0x060065AA RID: 26026 RVA: 0x00289215 File Offset: 0x00287415
		public static float CurrentBlend
		{
			get
			{
				if (!(WeatherAudioShelterRuntime._instance != null))
				{
					return 0f;
				}
				return WeatherAudioShelterRuntime._instance._blend;
			}
		}

		// Token: 0x060065AB RID: 26027 RVA: 0x00289234 File Offset: 0x00287434
		public static void SetArrivalVoiceDucking(Object owner, bool active)
		{
			if (owner == null)
			{
				return;
			}
			if (WeatherAudioShelterRuntime._instance == null && active && Application.isPlaying)
			{
				WeatherAudioShelterRuntime.EnsureRuntime();
			}
			if (WeatherAudioShelterRuntime._instance == null)
			{
				return;
			}
			if (active)
			{
				WeatherAudioShelterRuntime._instance._voiceOwners.Add(owner);
			}
			else
			{
				WeatherAudioShelterRuntime._instance._voiceOwners.Remove(owner);
			}
			if (active)
			{
				WeatherAudioShelterRuntime._instance._voiceGain = 0.25f;
				foreach (WeatherAudioShelterRuntime.SourceState sourceState in WeatherAudioShelterRuntime._instance._sources.Values)
				{
					sourceState.Apply(WeatherAudioShelterRuntime._instance._blend, WeatherAudioShelterRuntime._instance._voiceGain);
				}
			}
		}

		// Token: 0x060065AC RID: 26028 RVA: 0x00289310 File Offset: 0x00287510
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetInstance()
		{
			WeatherAudioShelterRuntime._instance = null;
		}

		// Token: 0x060065AD RID: 26029 RVA: 0x00289318 File Offset: 0x00287518
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void EnsureRuntime()
		{
			if (WeatherAudioShelterRuntime._instance != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("[WeatherAudioShelterRuntime]");
			Object.DontDestroyOnLoad(gameObject);
			gameObject.AddComponent<WeatherAudioShelterRuntime>();
		}

		// Token: 0x060065AE RID: 26030 RVA: 0x0028933E File Offset: 0x0028753E
		private void Awake()
		{
			WeatherAudioShelterRuntime._instance = this;
		}

		// Token: 0x060065AF RID: 26031 RVA: 0x00289348 File Offset: 0x00287548
		private void OnEnable()
		{
			WeatherAudioShelterRuntime._instance = this;
			AzureSoundFX.SourceVolumeApplied -= this.OnSourceVolumeApplied;
			AzureSoundFX.SourceVolumeApplied += this.OnSourceVolumeApplied;
			SceneManager.activeSceneChanged -= this.OnActiveSceneChanged;
			SceneManager.activeSceneChanged += this.OnActiveSceneChanged;
			this._nextSample = 0f;
			foreach (WeatherAudioShelterRuntime.SourceState sourceState in this._sources.Values)
			{
				sourceState.Resume(this._blend, this._voiceGain);
			}
		}

		// Token: 0x060065B0 RID: 26032 RVA: 0x00289400 File Offset: 0x00287600
		private void OnActiveSceneChanged(Scene previous, Scene current)
		{
			this._nextSample = 0f;
		}

		// Token: 0x060065B1 RID: 26033 RVA: 0x0028940D File Offset: 0x0028760D
		private void Update()
		{
			if (Time.unscaledTime >= this._nextSample)
			{
				this.SampleShelter();
			}
			this.AdvanceAudioBlend(Time.unscaledDeltaTime);
		}

		// Token: 0x060065B2 RID: 26034 RVA: 0x00289430 File Offset: 0x00287630
		private void SampleShelter()
		{
			Transform player = PlayerRegistry.Player;
			this._target = WeatherAudioShelterRuntime.EvaluateShelterTarget(SceneManager.GetActiveScene().name, (player != null) ? new Vector3?(player.position) : null);
			this._nextSample = Time.unscaledTime + 0.2f;
			this._expired.Clear();
			foreach (KeyValuePair<AudioSource, WeatherAudioShelterRuntime.SourceState> keyValuePair in this._sources)
			{
				if (keyValuePair.Key == null)
				{
					this._expired.Add(keyValuePair.Key);
				}
			}
			for (int i = 0; i < this._expired.Count; i++)
			{
				this._sources.Remove(this._expired[i]);
			}
			if (this._hasSample)
			{
				return;
			}
			this._hasSample = true;
			this._blend = this._target;
		}

		// Token: 0x060065B3 RID: 26035 RVA: 0x00289548 File Offset: 0x00287748
		public static float EvaluateShelterTarget(string sceneName, Vector3? playerPosition)
		{
			if (sceneName == "POI - Stadium")
			{
				return 0f;
			}
			if (RainShelterRuntime.IsClosedPoiShelterScene(sceneName))
			{
				return 1f;
			}
			if (playerPosition == null || !RainShelterRuntime.IsShelteredInScene(sceneName, playerPosition.Value))
			{
				return 0f;
			}
			return 1f;
		}

		// Token: 0x060065B4 RID: 26036 RVA: 0x0028959C File Offset: 0x0028779C
		private void OnSourceVolumeApplied(AudioSource source, float authoredVolume)
		{
			if (source == null)
			{
				return;
			}
			if (Time.unscaledTime >= this._nextSample)
			{
				this.SampleShelter();
			}
			WeatherAudioShelterRuntime.SourceState sourceState;
			if (!this._sources.TryGetValue(source, out sourceState))
			{
				sourceState = new WeatherAudioShelterRuntime.SourceState(source);
				this._sources.Add(source, sourceState);
			}
			sourceState.AuthoredVolume = Mathf.Clamp01(authoredVolume);
			sourceState.Apply(this._blend, this._voiceGain);
		}

		// Token: 0x060065B5 RID: 26037 RVA: 0x00289608 File Offset: 0x00287808
		private void AdvanceAudioBlend(float deltaTime)
		{
			float blend = this._blend;
			float voiceGain = this._voiceGain;
			this._voiceOwners.RemoveWhere((Object owner) => owner == null);
			float num = ((this._voiceOwners.Count > 0) ? 0.25f : 1f);
			this._voiceGain = Mathf.MoveTowards(this._voiceGain, num, Mathf.Max(0f, deltaTime) / 0.4f);
			this._blend = RainShelterRuntime.AdvanceShelterBlend(this._blend, this._target, ref this._velocity, deltaTime);
			if (this._blend == blend && this._voiceGain == voiceGain)
			{
				return;
			}
			foreach (WeatherAudioShelterRuntime.SourceState sourceState in this._sources.Values)
			{
				sourceState.Apply(this._blend, this._voiceGain);
			}
		}

		// Token: 0x060065B6 RID: 26038 RVA: 0x00289714 File Offset: 0x00287914
		private void OnDisable()
		{
			AzureSoundFX.SourceVolumeApplied -= this.OnSourceVolumeApplied;
			SceneManager.activeSceneChanged -= this.OnActiveSceneChanged;
			foreach (WeatherAudioShelterRuntime.SourceState sourceState in this._sources.Values)
			{
				sourceState.Restore();
			}
		}

		// Token: 0x060065B7 RID: 26039 RVA: 0x0028978C File Offset: 0x0028798C
		private void OnDestroy()
		{
			foreach (WeatherAudioShelterRuntime.SourceState sourceState in this._sources.Values)
			{
				sourceState.Dispose();
			}
			this._sources.Clear();
			if (WeatherAudioShelterRuntime._instance == this)
			{
				WeatherAudioShelterRuntime._instance = null;
			}
		}

		// Token: 0x04006847 RID: 26695
		public const float IndoorCutoffHz = 900f;

		// Token: 0x04006848 RID: 26696
		public const float OutdoorCutoffHz = 22000f;

		// Token: 0x04006849 RID: 26697
		public const float ArrivalVoiceVolumeMultiplier = 0.25f;

		// Token: 0x0400684A RID: 26698
		private const float SampleInterval = 0.2f;

		// Token: 0x0400684B RID: 26699
		private static WeatherAudioShelterRuntime _instance;

		// Token: 0x0400684C RID: 26700
		private readonly Dictionary<AudioSource, WeatherAudioShelterRuntime.SourceState> _sources = new Dictionary<AudioSource, WeatherAudioShelterRuntime.SourceState>();

		// Token: 0x0400684D RID: 26701
		private readonly List<AudioSource> _expired = new List<AudioSource>();

		// Token: 0x0400684E RID: 26702
		private float _blend;

		// Token: 0x0400684F RID: 26703
		private float _target;

		// Token: 0x04006850 RID: 26704
		private float _velocity;

		// Token: 0x04006851 RID: 26705
		private float _nextSample;

		// Token: 0x04006852 RID: 26706
		private bool _hasSample;

		// Token: 0x04006853 RID: 26707
		private readonly HashSet<Object> _voiceOwners = new HashSet<Object>();

		// Token: 0x04006854 RID: 26708
		private float _voiceGain = 1f;

		// Token: 0x02000D7E RID: 3454
		private sealed class SourceState
		{
			// Token: 0x060065B9 RID: 26041 RVA: 0x00289834 File Offset: 0x00287A34
			public SourceState(AudioSource source)
			{
				this._source = source;
				this._filter = source.GetComponent<AudioLowPassFilter>();
				this._originalEnabled = this._filter != null && this._filter.enabled;
				this._originalCutoff = ((this._filter != null) ? this._filter.cutoffFrequency : 22000f);
				this.AuthoredVolume = source.volume;
			}

			// Token: 0x060065BA RID: 26042 RVA: 0x002898B0 File Offset: 0x00287AB0
			public void Apply(float blend, float voiceGain)
			{
				if (this._source == null)
				{
					return;
				}
				this._source.volume = RainShelterRuntime.EvaluateShelteredRainVolume(this.AuthoredVolume, blend) * voiceGain;
				if (blend <= 0f)
				{
					this.RestoreFilter();
					return;
				}
				if (this._filter == null)
				{
					this._filter = this._source.gameObject.AddComponent<AudioLowPassFilter>();
					this._ownsFilter = true;
				}
				float num = Mathf.Exp(Mathf.Lerp(Mathf.Log(22000f), Mathf.Log(900f), blend));
				this._filter.cutoffFrequency = (this._originalEnabled ? Mathf.Min(this._originalCutoff, num) : num);
				this._filter.enabled = true;
			}

			// Token: 0x060065BB RID: 26043 RVA: 0x0028996D File Offset: 0x00287B6D
			public void Restore()
			{
				if (this._source != null)
				{
					this._source.volume = this.AuthoredVolume;
				}
				this.RestoreFilter();
			}

			// Token: 0x060065BC RID: 26044 RVA: 0x00289994 File Offset: 0x00287B94
			public void Resume(float blend, float voiceGain)
			{
				if (this._source == null)
				{
					return;
				}
				this.AuthoredVolume = this._source.volume;
				this.Apply(blend, voiceGain);
			}

			// Token: 0x060065BD RID: 26045 RVA: 0x002899BE File Offset: 0x00287BBE
			private void RestoreFilter()
			{
				if (this._filter == null)
				{
					return;
				}
				this._filter.cutoffFrequency = this._originalCutoff;
				this._filter.enabled = this._originalEnabled;
			}

			// Token: 0x060065BE RID: 26046 RVA: 0x002899F1 File Offset: 0x00287BF1
			public void Dispose()
			{
				this.Restore();
				if (!this._ownsFilter || this._filter == null)
				{
					return;
				}
				if (Application.isPlaying)
				{
					Object.Destroy(this._filter);
					return;
				}
				Object.DestroyImmediate(this._filter);
			}

			// Token: 0x04006855 RID: 26709
			private readonly AudioSource _source;

			// Token: 0x04006856 RID: 26710
			private AudioLowPassFilter _filter;

			// Token: 0x04006857 RID: 26711
			private readonly bool _originalEnabled;

			// Token: 0x04006858 RID: 26712
			private readonly float _originalCutoff;

			// Token: 0x04006859 RID: 26713
			private bool _ownsFilter;

			// Token: 0x0400685A RID: 26714
			public float AuthoredVolume;
		}
	}
}
