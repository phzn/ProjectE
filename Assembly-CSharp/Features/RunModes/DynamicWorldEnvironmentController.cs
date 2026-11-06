using System;
using System.Collections;
using System.Collections.Generic;
using Features.Rank;
using ProjectExhibitionist.Core.Rendering;
using UnityEngine;
using UnityEngine.AzureSky;
using UnityEngine.SceneManagement;

namespace Features.RunModes
{
	// Token: 0x02000CA7 RID: 3239
	[DefaultExecutionOrder(-900)]
	public sealed class DynamicWorldEnvironmentController : MonoBehaviour
	{
		// Token: 0x06005E06 RID: 24070 RVA: 0x00252C58 File Offset: 0x00250E58
		public static void ApplyHomeRank(RankDefinitionSO rank, RankProgressionConfigSO config)
		{
			if (DynamicWorldEnvironmentController._instance == null)
			{
				return;
			}
			DynamicWorldEnvironmentController._instance.ApplyHomeRankPolicy(rank, config, SceneManager.GetActiveScene().name);
		}

		// Token: 0x06005E07 RID: 24071 RVA: 0x00252C8C File Offset: 0x00250E8C
		public static bool IsSupportedWorldScene(string sceneName)
		{
			return !string.IsNullOrEmpty(sceneName) && DynamicWorldEnvironmentController.SupportedSceneNames.Contains(sceneName);
		}

		// Token: 0x06005E08 RID: 24072 RVA: 0x00252CA4 File Offset: 0x00250EA4
		public static float EvaluateFogBlend(bool currentFog, bool targetFog, bool isChanging, float transitionProgress)
		{
			if (!isChanging)
			{
				if (!currentFog)
				{
					return 0f;
				}
				return 1f;
			}
			else if (currentFog == targetFog)
			{
				if (!currentFog)
				{
					return 0f;
				}
				return 1f;
			}
			else
			{
				float num = Mathf.Clamp01(transitionProgress);
				if (!targetFog)
				{
					return 1f - num;
				}
				return num;
			}
		}

		// Token: 0x06005E09 RID: 24073 RVA: 0x00252CEC File Offset: 0x00250EEC
		public static float AdvanceFogBlend(float currentBlend, float targetBlend, ref float velocity, float deltaTime)
		{
			currentBlend = Mathf.Clamp01(currentBlend);
			targetBlend = Mathf.Clamp01(targetBlend);
			if (deltaTime <= 0f)
			{
				return currentBlend;
			}
			float num = Mathf.SmoothDamp(currentBlend, targetBlend, ref velocity, 8f, float.PositiveInfinity, deltaTime);
			num = Mathf.Clamp01(num);
			if (Mathf.Abs(num - targetBlend) <= 0.0001f && Mathf.Abs(velocity) <= 0.0001f)
			{
				velocity = 0f;
				return targetBlend;
			}
			return num;
		}

		// Token: 0x06005E0A RID: 24074 RVA: 0x00252D58 File Offset: 0x00250F58
		public static float EvaluateRenderedFogBlend(float simulationBlend)
		{
			float num = Mathf.Clamp01(simulationBlend);
			if (num < 0.02f)
			{
				return 0f;
			}
			return num;
		}

		// Token: 0x06005E0B RID: 24075 RVA: 0x00252D7C File Offset: 0x00250F7C
		public static bool IsFullNightLightingHour(float hour)
		{
			float num = Mathf.Repeat(hour, 24f);
			return num >= 19f || num < 7f;
		}

		// Token: 0x06005E0C RID: 24076 RVA: 0x00252DA8 File Offset: 0x00250FA8
		public static bool IsEnvironmentReady(AzureCoreSystem azure)
		{
			return azure != null && DynamicWorldEnvironmentController._instance != null && DynamicWorldEnvironmentController._instance._worldActive && DynamicWorldEnvironmentController._instance._worldInitialized && DynamicWorldEnvironmentController._instance._baselineCaptured && DynamicWorldEnvironmentController._instance._azure == azure;
		}

		// Token: 0x06005E0D RID: 24077 RVA: 0x00252DFE File Offset: 0x00250FFE
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void EnsureRuntime()
		{
			if (DynamicWorldEnvironmentController._instance != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("[DynamicWorldEnvironment]");
			Object.DontDestroyOnLoad(gameObject);
			DynamicWorldEnvironmentController._instance = gameObject.AddComponent<DynamicWorldEnvironmentController>();
		}

		// Token: 0x06005E0E RID: 24078 RVA: 0x00252E28 File Offset: 0x00251028
		private void OnEnable()
		{
			SceneManager.activeSceneChanged += this.OnActiveSceneChanged;
			SceneManager.sceneLoaded += this.OnSceneLoaded;
			AzureNotificationCenter.OnWeatherTransitionEnd += this.OnWeatherTransitionEnd;
			RankProgressionService.Changed += this.OnRankProgressionChanged;
			this.RefreshScope(SceneManager.GetActiveScene());
		}

		// Token: 0x06005E0F RID: 24079 RVA: 0x00252E84 File Offset: 0x00251084
		private void OnDisable()
		{
			SceneManager.activeSceneChanged -= this.OnActiveSceneChanged;
			SceneManager.sceneLoaded -= this.OnSceneLoaded;
			AzureNotificationCenter.OnWeatherTransitionEnd -= this.OnWeatherTransitionEnd;
			RankProgressionService.Changed -= this.OnRankProgressionChanged;
			this.RestoreHomeNightClock();
			this.RestoreVisualFxFollowTarget();
			DynamicWorldEnvironmentController.ResetFogShaderState();
		}

		// Token: 0x06005E10 RID: 24080 RVA: 0x00252EE7 File Offset: 0x002510E7
		private void OnActiveSceneChanged(Scene _, Scene current)
		{
			this.RefreshScope(current);
		}

		// Token: 0x06005E11 RID: 24081 RVA: 0x00252EF0 File Offset: 0x002510F0
		private void OnSceneLoaded(Scene scene, LoadSceneMode _)
		{
			if (scene.IsValid() && scene.name == "_Core")
			{
				this.BeginBind();
			}
		}

		// Token: 0x06005E12 RID: 24082 RVA: 0x00252F14 File Offset: 0x00251114
		private void OnRankProgressionChanged()
		{
			if (SceneManager.GetActiveScene().name != "CielHome")
			{
				return;
			}
			this.ApplyHomeRankPolicy(RankProgressionService.CurrentRank, RankProgressionService.Config, "CielHome");
		}

		// Token: 0x06005E13 RID: 24083 RVA: 0x00252F50 File Offset: 0x00251150
		private void OnWeatherTransitionEnd(AzureWeatherSystem weatherSystem)
		{
			if (!this._worldActive || this._azure == null || weatherSystem != this._azure.weatherSystem)
			{
				return;
			}
			EnvironmentLightingRefresh.ForceRequestForActiveScene();
		}

		// Token: 0x06005E14 RID: 24084 RVA: 0x00252F7C File Offset: 0x0025117C
		private void RefreshScope(Scene activeScene)
		{
			if (!activeScene.IsValid() || activeScene.name != "CielHome")
			{
				this.RestoreHomeNightClock();
			}
			bool flag = activeScene.IsValid() && DynamicWorldEnvironmentController.IsSupportedWorldScene(activeScene.name);
			if (this._worldActive == flag)
			{
				if (this._azure == null)
				{
					this.BeginBind();
					return;
				}
				this.Bind(this._azure);
				return;
			}
			else
			{
				this._worldActive = flag;
				DynamicWorldEnvironmentController.ResetFogShaderState();
				this._fogBlend = 0f;
				this._fogBlendVelocity = 0f;
				this._lightingBandInitialized = false;
				if (this._worldActive)
				{
					this._worldInitialized = false;
					this._fogSimulationTime = 0f;
					this._visualFxBaselineCaptured = false;
					if (this._azure != null)
					{
						this._baselineCaptured = false;
						this.CaptureBaseline();
						this.Bind(this._azure);
						return;
					}
					this.BeginBind();
					return;
				}
				else
				{
					if (this._bindRoutine != null)
					{
						base.StopCoroutine(this._bindRoutine);
						this._bindRoutine = null;
					}
					this.RestoreAuthoredEnvironment();
					if (this._azure == null)
					{
						this.BeginBind();
						return;
					}
					this.ApplyHomeRankPolicy(RankProgressionService.CurrentRank, RankProgressionService.Config, activeScene.name);
					return;
				}
			}
		}

		// Token: 0x06005E15 RID: 24085 RVA: 0x002530B6 File Offset: 0x002512B6
		private void BeginBind()
		{
			if (this._bindRoutine != null)
			{
				base.StopCoroutine(this._bindRoutine);
			}
			this._bindRoutine = base.StartCoroutine(this.BindWhenReady());
		}

		// Token: 0x06005E16 RID: 24086 RVA: 0x002530DE File Offset: 0x002512DE
		private IEnumerator BindWhenReady()
		{
			yield return null;
			int num;
			for (int frame = 0; frame < 180; frame = num + 1)
			{
				AzureCoreSystem azureCoreSystem = DynamicWorldEnvironmentController.FindCoreAzure();
				if (azureCoreSystem != null)
				{
					this.Bind(azureCoreSystem);
					this._bindRoutine = null;
					yield break;
				}
				yield return null;
				num = frame;
			}
			this._bindRoutine = null;
			if (this._worldActive)
			{
				Debug.LogWarning("[Environment] AzureCoreSystem in _Core was not available after 180 frames.");
			}
			yield break;
		}

		// Token: 0x06005E17 RID: 24087 RVA: 0x002530F0 File Offset: 0x002512F0
		private void Bind(AzureCoreSystem core)
		{
			int instanceID = core.GetInstanceID();
			if (this._azureInstanceId != instanceID)
			{
				this.RestoreHomeNightClock();
				this._azure = core;
				this._azureInstanceId = instanceID;
				this._worldInitialized = false;
				this._lightingBandInitialized = false;
				this._weatherTuningApplied = false;
				this._weatherTuningSnapshot = null;
				this._baselineCaptured = false;
				this._visualFxFollowTarget = null;
				this._visualFxBaselineCaptured = false;
				this.CaptureBaseline();
			}
			if (this._worldActive && !this._weatherTuningApplied)
			{
				AzureWeatherSystem weatherSystem = this._azure.weatherSystem;
				if (weatherSystem != null)
				{
					this._weatherTuningSnapshot = AzureDynamicWeatherTuning.Capture(weatherSystem);
					AzureDynamicWeatherTuning.Apply(weatherSystem);
					this._weatherTuningApplied = true;
				}
			}
			if (this._runModeRuntime == null)
			{
				this._runModeRuntime = Object.FindObjectOfType<RunModeRuntimeController>();
			}
			this.InitializeWorldOnce();
			DynamicWorldEnvironmentController.EnforceSceneWeatherPolicy(this._azure.weatherSystem, SceneManager.GetActiveScene().name);
			this.ApplyHomeRankPolicy(RankProgressionService.CurrentRank, RankProgressionService.Config, SceneManager.GetActiveScene().name);
			this.BindVisualFxToMainCamera();
			this.UpdateFogFeatureGate(0f);
		}

		// Token: 0x06005E18 RID: 24088 RVA: 0x002531FC File Offset: 0x002513FC
		private void CaptureBaseline()
		{
			this.CaptureVisualFxBaseline();
			if (this._azure == null || this._azure.timeSystem == null)
			{
				return;
			}
			this._baselineTimeline = this._azure.timeSystem.timeline;
			this._baselineStartTime = this._azure.timeSystem.startTime;
			this._baselineDayLength = this._azure.timeSystem.dayLength;
			this._baselineDay = this._azure.timeSystem.day;
			this._baselineMonth = this._azure.timeSystem.month;
			this._baselineYear = this._azure.timeSystem.year;
			this._baselineWeatherIndex = ((this._azure.weatherSystem != null) ? this._azure.weatherSystem.globalWeatherIndex : 0);
			this._baselineWeather = ((this._azure.weatherSystem != null) ? this._azure.weatherSystem.currentWeatherPreset : null);
			this._baselineCaptured = true;
		}

		// Token: 0x06005E19 RID: 24089 RVA: 0x00253304 File Offset: 0x00251504
		private void ApplyHomeRankPolicy(RankDefinitionSO rank, RankProgressionConfigSO config, string sceneName)
		{
			if (this._worldActive || !this._baselineCaptured || this._azure == null || this._azure.timeSystem == null)
			{
				return;
			}
			if (!(sceneName == "CielHome") || !RankNightController.ShouldLockNight(rank, sceneName, RunModeService.CurrentMode))
			{
				if (this.RestoreHomeNightClock() && Application.isPlaying)
				{
					EnvironmentLightingRefresh.ForceRequestForActiveScene();
				}
				return;
			}
			bool flag = !this._homeNightActive;
			if (flag)
			{
				this._homeTimeline = this._azure.timeSystem.timeline;
				this._homeStartTime = this._azure.timeSystem.startTime;
				this._homeDayLength = this._azure.timeSystem.dayLength;
				this._homeNightActive = true;
			}
			RankNightController.ApplyForScene(rank, config, this._azure, sceneName, RunModeService.CurrentMode);
			if (flag && Application.isPlaying)
			{
				EnvironmentLightingRefresh.ForceRequestForActiveScene();
			}
		}

		// Token: 0x06005E1A RID: 24090 RVA: 0x002533E8 File Offset: 0x002515E8
		private bool RestoreHomeNightClock()
		{
			if (!this._homeNightActive)
			{
				return false;
			}
			RankNightController.Release();
			this._homeNightActive = false;
			if (this._azure != null && this._azure.timeSystem != null)
			{
				this._azure.timeSystem.startTime = this._homeStartTime;
				this._azure.timeSystem.timeline = this._homeTimeline;
				this._azure.timeSystem.dayLength = this._homeDayLength;
			}
			return true;
		}

		// Token: 0x06005E1B RID: 24091 RVA: 0x0025346C File Offset: 0x0025166C
		private void InitializeWorldOnce()
		{
			if (this._worldInitialized || !this._worldActive || this._azure == null)
			{
				return;
			}
			bool flag = RunModeService.CurrentMode == RunModeKind.Free;
			if (!flag && this._azure.timeSystem != null)
			{
				float num = (float)(RunModeService.IsLongWayHome ? RunModeService.LongWayHomeStartHour : RunModeService.ClassicStartHour);
				this._azure.timeSystem.startTime = num;
				this._azure.timeSystem.timeline = num;
				this._azure.timeSystem.dayLength = 24f;
				this.ScheduleForecast(true);
			}
			this._worldInitialized = true;
			RankNightController.ApplyForScene(RankProgressionService.CurrentRank, RankProgressionService.Config, this._azure, SceneManager.GetActiveScene().name, RunModeService.CurrentMode);
			this._lightingBandInitialized = false;
			this.RefreshEnvironmentLightingIfNeeded(true);
			Debug.Log(flag ? "[Environment] Procedural world active; Free Mode owns clock and forecast." : (string.Format("[Environment] Procedural world active at {0:00.00}; ", this._azure.timeSystem.timeline) + string.Format("day length {0:0.##} minutes; dynamic forecast enabled.", this._azure.timeSystem.dayLength)));
		}

		// Token: 0x06005E1C RID: 24092 RVA: 0x00253590 File Offset: 0x00251790
		private void RefreshEnvironmentLightingIfNeeded(bool force = false)
		{
			if (!this._worldActive || !this._worldInitialized || this._azure == null || this._azure.timeSystem == null)
			{
				return;
			}
			bool flag = DynamicWorldEnvironmentController.IsFullNightLightingHour(this._azure.timeSystem.timeOfDay);
			if (!force && this._lightingBandInitialized && this._lightingBandWasNight == flag)
			{
				return;
			}
			this._lightingBandInitialized = true;
			this._lightingBandWasNight = flag;
			EnvironmentLightingRefresh.ForceRequestForActiveScene();
		}

		// Token: 0x06005E1D RID: 24093 RVA: 0x00253608 File Offset: 0x00251808
		private void Update()
		{
			if (!this._worldActive)
			{
				return;
			}
			if (this._azure == null)
			{
				if (this._bindRoutine == null)
				{
					this.BeginBind();
				}
				return;
			}
			if (this._runModeRuntime == null)
			{
				this._runModeRuntime = Object.FindObjectOfType<RunModeRuntimeController>();
			}
			this.BindVisualFxToMainCamera();
			DynamicWorldEnvironmentController.EnforceSceneWeatherPolicy(this._azure.weatherSystem, SceneManager.GetActiveScene().name);
			bool flag = Time.timeScale <= 0f || (this._runModeRuntime != null && this._runModeRuntime.SimulationPaused);
			if (!flag)
			{
				this._fogSimulationTime += Time.unscaledDeltaTime;
			}
			this.UpdateFogFeatureGate(flag ? 0f : Time.unscaledDeltaTime);
			if (!this._worldInitialized)
			{
				return;
			}
			this.RefreshEnvironmentLightingIfNeeded(false);
			bool flag2 = RunModeService.CurrentMode == RunModeKind.Free;
			if (flag)
			{
				if (!this._forecastPaused)
				{
					this._forecastPaused = true;
					this._forecastPauseStartedAt = Time.unscaledTime;
					this._weatherTransitionPauseStartedAt = Time.time;
				}
				return;
			}
			if (this._forecastPaused)
			{
				float num = Mathf.Max(0f, Time.unscaledTime - this._forecastPauseStartedAt);
				this._nextWeatherChange += num;
				this._nextThunderTime += num;
				AzureWeatherSystem weatherSystem = this._azure.weatherSystem;
				if (weatherSystem != null)
				{
					weatherSystem.DelayActiveWeatherTransition(Mathf.Max(0f, Time.time - this._weatherTransitionPauseStartedAt));
				}
				this._forecastPaused = false;
			}
			AzureWeatherSystem weatherSystem2 = this._azure.weatherSystem;
			this.UpdateThunder(weatherSystem2);
			if (flag2)
			{
				return;
			}
			if (Time.unscaledTime < this._nextWeatherChange)
			{
				return;
			}
			if (weatherSystem2 == null || weatherSystem2.isWeatherChanging)
			{
				return;
			}
			this.RotateForecast(weatherSystem2);
		}

		// Token: 0x06005E1E RID: 24094 RVA: 0x002537B8 File Offset: 0x002519B8
		public static int ResolveSceneWeatherIndex(AzureWeatherSystem weather, int requestedIndex, string sceneName)
		{
			if (((weather != null) ? weather.globalWeatherList : null) == null)
			{
				return -1;
			}
			if (DynamicWeatherPolicy.AllowsFogInScene(sceneName))
			{
				return requestedIndex;
			}
			string[] array = new string[weather.globalWeatherList.Count];
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array;
				int num = i;
				AzureGlobalWeather azureGlobalWeather = weather.globalWeatherList[i];
				array2[num] = ((((azureGlobalWeather != null) ? azureGlobalWeather.weatherPreset : null) != null) ? weather.globalWeatherList[i].weatherPreset.name : string.Empty);
			}
			return DynamicWeatherPolicy.ResolveAllowedIndex(array, requestedIndex, false);
		}

		// Token: 0x06005E1F RID: 24095 RVA: 0x00253848 File Offset: 0x00251A48
		public static void EnforceSceneWeatherPolicy(AzureWeatherSystem weather, string sceneName)
		{
			if (weather == null || DynamicWeatherPolicy.AllowsFogInScene(sceneName))
			{
				return;
			}
			bool flag = weather.currentWeatherPreset != null && DynamicWeatherPolicy.IsFogPreset(weather.currentWeatherPreset.name);
			bool flag2 = weather.targetWeatherPreset != null && DynamicWeatherPolicy.IsFogPreset(weather.targetWeatherPreset.name);
			if (!flag && !flag2)
			{
				return;
			}
			int num = DynamicWorldEnvironmentController.ResolveSceneWeatherIndex(weather, weather.globalWeatherIndex, sceneName);
			if (num >= 0)
			{
				weather.SetGlobalWeatherInstantly(num);
			}
		}

		// Token: 0x06005E20 RID: 24096 RVA: 0x002538C4 File Offset: 0x00251AC4
		private void RotateForecast(AzureWeatherSystem weather)
		{
			if (weather.globalWeatherList == null || weather.globalWeatherList.Count < 2)
			{
				this.ScheduleForecast(false);
				return;
			}
			List<string> list = new List<string>(weather.globalWeatherList.Count);
			for (int i = 0; i < weather.globalWeatherList.Count; i++)
			{
				AzureGlobalWeather azureGlobalWeather = weather.globalWeatherList[i];
				list.Add((azureGlobalWeather != null && azureGlobalWeather.weatherPreset != null) ? azureGlobalWeather.weatherPreset.name : string.Empty);
			}
			int num = DynamicWeatherPolicy.SelectNextIndex(list, weather.globalWeatherIndex, Random.value, (this._azure.timeSystem != null) ? this._azure.timeSystem.timeline : 12f, null, DynamicWeatherPolicy.AllowsFogInScene(SceneManager.GetActiveScene().name));
			if (num >= 0 && num < weather.globalWeatherList.Count && num != weather.globalWeatherIndex)
			{
				weather.SetGlobalWeather(num);
			}
			this.ScheduleForecast(false);
		}

		// Token: 0x06005E21 RID: 24097 RVA: 0x002539BF File Offset: 0x00251BBF
		private void ScheduleForecast(bool initialForecast)
		{
			this._nextWeatherChange = Time.unscaledTime + DynamicWeatherPolicy.GetClassicDelaySeconds(Random.value, initialForecast);
		}

		// Token: 0x06005E22 RID: 24098 RVA: 0x002539D8 File Offset: 0x00251BD8
		private void UpdateThunder(AzureWeatherSystem weather)
		{
			if (!DynamicWeatherPolicy.IsStormPreset((weather != null && weather.currentWeatherPreset != null) ? weather.currentWeatherPreset.name : string.Empty))
			{
				this._nextThunderTime = float.PositiveInfinity;
				return;
			}
			if (float.IsPositiveInfinity(this._nextThunderTime))
			{
				this.ScheduleThunder();
				return;
			}
			if (Time.unscaledTime < this._nextThunderTime)
			{
				return;
			}
			if (weather.thunderSettingsList != null && weather.thunderSettingsList.Count > 0)
			{
				int num = Random.Range(0, weather.thunderSettingsList.Count);
				weather.InstantiateThunderPrefab(num);
			}
			this.ScheduleThunder();
		}

		// Token: 0x06005E23 RID: 24099 RVA: 0x00253A72 File Offset: 0x00251C72
		private void ScheduleThunder()
		{
			this._nextThunderTime = Time.unscaledTime + DynamicWeatherPolicy.GetThunderDelaySeconds(Random.value);
		}

		// Token: 0x06005E24 RID: 24100 RVA: 0x00253A8C File Offset: 0x00251C8C
		private void UpdateFogFeatureGate(float deltaTime)
		{
			if (!DynamicWeatherPolicy.AllowsFogInScene(SceneManager.GetActiveScene().name))
			{
				this._fogBlend = 0f;
				this._fogBlendVelocity = 0f;
				DynamicWorldEnvironmentController.ResetFogShaderState();
				return;
			}
			AzureWeatherSystem azureWeatherSystem = ((this._azure != null) ? this._azure.weatherSystem : null);
			bool flag = azureWeatherSystem != null && DynamicWeatherPolicy.IsFogPreset((azureWeatherSystem.currentWeatherPreset != null) ? azureWeatherSystem.currentWeatherPreset.name : string.Empty);
			bool flag2 = azureWeatherSystem != null && DynamicWeatherPolicy.IsFogPreset((azureWeatherSystem.targetWeatherPreset != null) ? azureWeatherSystem.targetWeatherPreset.name : string.Empty);
			float num = ((this._worldActive && azureWeatherSystem != null) ? DynamicWorldEnvironmentController.EvaluateFogBlend(flag, flag2, azureWeatherSystem.isWeatherChanging, azureWeatherSystem.weatherTransitionProgress) : 0f);
			if (this._worldActive && azureWeatherSystem != null)
			{
				this._fogBlend = DynamicWorldEnvironmentController.AdvanceFogBlend(this._fogBlend, num, ref this._fogBlendVelocity, deltaTime);
			}
			else
			{
				this._fogBlend = 0f;
				this._fogBlendVelocity = 0f;
			}
			float num2 = DynamicWorldEnvironmentController.EvaluateRenderedFogBlend(this._fogBlend);
			bool flag3 = num2 > 0f;
			Shader.SetGlobalFloat(DynamicWorldEnvironmentController.FogFeatureGate, flag3 ? 1f : 0f);
			Shader.SetGlobalFloat(DynamicWorldEnvironmentController.FogBlend, num2);
			Shader.SetGlobalFloat(DynamicWorldEnvironmentController.FogStartDistance, this._worldActive ? 3f : 0f);
			Shader.SetGlobalFloat(DynamicWorldEnvironmentController.FogOpaqueDistance, this._worldActive ? 10f : 0f);
			Shader.SetGlobalFloat(DynamicWorldEnvironmentController.FogNoiseStrength, this._worldActive ? 0.12f : 0f);
			Shader.SetGlobalFloat(DynamicWorldEnvironmentController.FogSimulationTime, this._worldActive ? this._fogSimulationTime : 0f);
		}

		// Token: 0x06005E25 RID: 24101 RVA: 0x00253C58 File Offset: 0x00251E58
		private void RestoreAuthoredEnvironment()
		{
			RankNightController.Release();
			this._nextWeatherChange = float.PositiveInfinity;
			this._nextThunderTime = float.PositiveInfinity;
			this._forecastPaused = false;
			this._worldInitialized = false;
			this._lightingBandInitialized = false;
			AzureDynamicWeatherTuning.Snapshot weatherTuningSnapshot = this._weatherTuningSnapshot;
			if (weatherTuningSnapshot != null)
			{
				weatherTuningSnapshot.Restore();
			}
			this._weatherTuningSnapshot = null;
			this._weatherTuningApplied = false;
			this.RestoreVisualFxFollowTarget();
			if (!this._baselineCaptured || this._azure == null || this._azure.timeSystem == null)
			{
				return;
			}
			this._azure.timeSystem.SetDate(this._baselineDay, this._baselineMonth, this._baselineYear);
			this._azure.timeSystem.startTime = this._baselineStartTime;
			this._azure.timeSystem.timeline = this._baselineTimeline;
			this._azure.timeSystem.dayLength = this._baselineDayLength;
			AzureWeatherSystem weatherSystem = this._azure.weatherSystem;
			if (weatherSystem != null && this._baselineWeather != null)
			{
				weatherSystem.evaluationTime = this._azure.timeSystem.evaluationTime;
				weatherSystem.SetGlobalWeatherInstantly(this._baselineWeather, this._baselineWeatherIndex);
			}
		}

		// Token: 0x06005E26 RID: 24102 RVA: 0x00253D88 File Offset: 0x00251F88
		private void CaptureVisualFxBaseline()
		{
			if (this._visualFxBaselineCaptured || this._azure == null || this._azure.followTargetList == null)
			{
				return;
			}
			for (int i = 0; i < this._azure.followTargetList.Count; i++)
			{
				AzureFollowTarget azureFollowTarget = this._azure.followTargetList[i];
				if (azureFollowTarget != null && !(azureFollowTarget.follower == null) && !(azureFollowTarget.follower.name != "VisualFX"))
				{
					this._visualFxFollowTarget = azureFollowTarget;
					this._baselineVisualFxTarget = azureFollowTarget.target;
					this._baselineVisualFxPosition = azureFollowTarget.follower.position;
					this._visualFxBaselineCaptured = true;
					return;
				}
			}
		}

		// Token: 0x06005E27 RID: 24103 RVA: 0x00253E3C File Offset: 0x0025203C
		private void BindVisualFxToMainCamera()
		{
			if (!this._worldActive || this._azure == null)
			{
				return;
			}
			if (!this._visualFxBaselineCaptured)
			{
				this.CaptureVisualFxBaseline();
			}
			if (this._visualFxFollowTarget == null)
			{
				return;
			}
			Camera main = Camera.main;
			if (main == null)
			{
				return;
			}
			Transform transform = main.transform;
			this._visualFxFollowTarget.target = transform;
			if (this._visualFxFollowTarget.follower != null)
			{
				this._visualFxFollowTarget.follower.position = transform.position;
			}
		}

		// Token: 0x06005E28 RID: 24104 RVA: 0x00253EC4 File Offset: 0x002520C4
		private void RestoreVisualFxFollowTarget()
		{
			if (this._visualFxBaselineCaptured && this._visualFxFollowTarget != null)
			{
				this._visualFxFollowTarget.target = this._baselineVisualFxTarget;
				if (this._visualFxFollowTarget.follower != null)
				{
					this._visualFxFollowTarget.follower.position = this._baselineVisualFxPosition;
				}
			}
			this._visualFxFollowTarget = null;
			this._baselineVisualFxTarget = null;
			this._visualFxBaselineCaptured = false;
		}

		// Token: 0x06005E29 RID: 24105 RVA: 0x00253F30 File Offset: 0x00252130
		private static void ResetFogShaderState()
		{
			Shader.SetGlobalFloat(DynamicWorldEnvironmentController.FogFeatureGate, 0f);
			Shader.SetGlobalFloat(DynamicWorldEnvironmentController.FogStartDistance, 0f);
			Shader.SetGlobalFloat(DynamicWorldEnvironmentController.FogNoiseStrength, 0f);
			Shader.SetGlobalFloat(DynamicWorldEnvironmentController.FogSimulationTime, 0f);
			Shader.SetGlobalFloat(DynamicWorldEnvironmentController.FogBlend, 0f);
			Shader.SetGlobalFloat(DynamicWorldEnvironmentController.FogOpaqueDistance, 0f);
		}

		// Token: 0x06005E2A RID: 24106 RVA: 0x00253F98 File Offset: 0x00252198
		private static AzureCoreSystem FindCoreAzure()
		{
			foreach (AzureCoreSystem azureCoreSystem in Object.FindObjectsOfType<AzureCoreSystem>(true))
			{
				if (azureCoreSystem != null && azureCoreSystem.gameObject.scene.IsValid() && azureCoreSystem.gameObject.scene.name == "_Core")
				{
					return azureCoreSystem;
				}
			}
			return null;
		}

		// Token: 0x04005F03 RID: 24323
		public const float ClassicStartHour = 12f;

		// Token: 0x04005F04 RID: 24324
		public const float ClassicDayLengthMinutes = 24f;

		// Token: 0x04005F05 RID: 24325
		public const string FogFeatureGateName = "_Project_AzureFogScatteringEnabled";

		// Token: 0x04005F06 RID: 24326
		public const string FogStartDistanceName = "_Project_AzureFogStartDistance";

		// Token: 0x04005F07 RID: 24327
		public const string FogNoiseStrengthName = "_Project_AzureFogNoiseStrength";

		// Token: 0x04005F08 RID: 24328
		public const string FogSimulationTimeName = "_Project_AzureFogTime";

		// Token: 0x04005F09 RID: 24329
		public const string FogBlendName = "_Project_AzureFogBlend";

		// Token: 0x04005F0A RID: 24330
		public const string FogOpaqueDistanceName = "_Project_AzureFogOpaqueDistance";

		// Token: 0x04005F0B RID: 24331
		public const float DenseFogStartDistanceMeters = 3f;

		// Token: 0x04005F0C RID: 24332
		public const float DenseFogNinetyPercentDistanceMeters = 10f;

		// Token: 0x04005F0D RID: 24333
		public const float DenseFogOpaqueDistanceMeters = 10f;

		// Token: 0x04005F0E RID: 24334
		public const float FogBlendSmoothTimeSeconds = 8f;

		// Token: 0x04005F0F RID: 24335
		public const float FogRenderingBlendCutoff = 0.02f;

		// Token: 0x04005F10 RID: 24336
		public const float DenseFogNoiseStrength = 0.12f;

		// Token: 0x04005F11 RID: 24337
		public const float FullDayLightingHour = 7f;

		// Token: 0x04005F12 RID: 24338
		public const float FullNightLightingHour = 19f;

		// Token: 0x04005F13 RID: 24339
		private static readonly HashSet<string> SupportedSceneNames = new HashSet<string> { "Game - Current", "POI - Mall", "POI - Art Gallery", "POI - Pool", "POI - University", "POI - Stadium" };

		// Token: 0x04005F14 RID: 24340
		private static readonly int FogFeatureGate = Shader.PropertyToID("_Project_AzureFogScatteringEnabled");

		// Token: 0x04005F15 RID: 24341
		private static readonly int FogStartDistance = Shader.PropertyToID("_Project_AzureFogStartDistance");

		// Token: 0x04005F16 RID: 24342
		private static readonly int FogNoiseStrength = Shader.PropertyToID("_Project_AzureFogNoiseStrength");

		// Token: 0x04005F17 RID: 24343
		private static readonly int FogSimulationTime = Shader.PropertyToID("_Project_AzureFogTime");

		// Token: 0x04005F18 RID: 24344
		private static readonly int FogBlend = Shader.PropertyToID("_Project_AzureFogBlend");

		// Token: 0x04005F19 RID: 24345
		private static readonly int FogOpaqueDistance = Shader.PropertyToID("_Project_AzureFogOpaqueDistance");

		// Token: 0x04005F1A RID: 24346
		private static DynamicWorldEnvironmentController _instance;

		// Token: 0x04005F1B RID: 24347
		private AzureCoreSystem _azure;

		// Token: 0x04005F1C RID: 24348
		private RunModeRuntimeController _runModeRuntime;

		// Token: 0x04005F1D RID: 24349
		private Coroutine _bindRoutine;

		// Token: 0x04005F1E RID: 24350
		private int _azureInstanceId;

		// Token: 0x04005F1F RID: 24351
		private bool _worldActive;

		// Token: 0x04005F20 RID: 24352
		private bool _worldInitialized;

		// Token: 0x04005F21 RID: 24353
		private bool _weatherTuningApplied;

		// Token: 0x04005F22 RID: 24354
		private bool _baselineCaptured;

		// Token: 0x04005F23 RID: 24355
		private float _baselineTimeline;

		// Token: 0x04005F24 RID: 24356
		private float _baselineStartTime;

		// Token: 0x04005F25 RID: 24357
		private float _baselineDayLength;

		// Token: 0x04005F26 RID: 24358
		private int _baselineDay;

		// Token: 0x04005F27 RID: 24359
		private int _baselineMonth;

		// Token: 0x04005F28 RID: 24360
		private int _baselineYear;

		// Token: 0x04005F29 RID: 24361
		private int _baselineWeatherIndex;

		// Token: 0x04005F2A RID: 24362
		private AzureWeatherPreset _baselineWeather;

		// Token: 0x04005F2B RID: 24363
		private AzureDynamicWeatherTuning.Snapshot _weatherTuningSnapshot;

		// Token: 0x04005F2C RID: 24364
		private float _nextWeatherChange = float.PositiveInfinity;

		// Token: 0x04005F2D RID: 24365
		private float _nextThunderTime = float.PositiveInfinity;

		// Token: 0x04005F2E RID: 24366
		private bool _forecastPaused;

		// Token: 0x04005F2F RID: 24367
		private float _forecastPauseStartedAt;

		// Token: 0x04005F30 RID: 24368
		private float _weatherTransitionPauseStartedAt;

		// Token: 0x04005F31 RID: 24369
		private float _fogSimulationTime;

		// Token: 0x04005F32 RID: 24370
		private float _fogBlend;

		// Token: 0x04005F33 RID: 24371
		private float _fogBlendVelocity;

		// Token: 0x04005F34 RID: 24372
		private AzureFollowTarget _visualFxFollowTarget;

		// Token: 0x04005F35 RID: 24373
		private Transform _baselineVisualFxTarget;

		// Token: 0x04005F36 RID: 24374
		private Vector3 _baselineVisualFxPosition;

		// Token: 0x04005F37 RID: 24375
		private bool _visualFxBaselineCaptured;

		// Token: 0x04005F38 RID: 24376
		private bool _lightingBandInitialized;

		// Token: 0x04005F39 RID: 24377
		private bool _lightingBandWasNight;

		// Token: 0x04005F3A RID: 24378
		private bool _homeNightActive;

		// Token: 0x04005F3B RID: 24379
		private float _homeTimeline;

		// Token: 0x04005F3C RID: 24380
		private float _homeStartTime;

		// Token: 0x04005F3D RID: 24381
		private float _homeDayLength;
	}
}
