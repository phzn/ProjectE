using System;
using System.Collections;
using Features.Detection;
using Features.Rank;
using ProjectExhibitionist.Core.Rendering;
using UnityEngine;
using UnityEngine.AzureSky;
using UnityEngine.SceneManagement;

namespace Features.RunModes
{
	// Token: 0x02000D6D RID: 3437
	[DefaultExecutionOrder(-850)]
	public sealed class RunModeRuntimeController : MonoBehaviour
	{
		// Token: 0x170010C2 RID: 4290
		// (get) Token: 0x06006542 RID: 25922 RVA: 0x0028612B File Offset: 0x0028432B
		// (set) Token: 0x06006543 RID: 25923 RVA: 0x00286133 File Offset: 0x00284333
		public bool SimulationPaused
		{
			get
			{
				return this._simulationPaused;
			}
			set
			{
				if (this._simulationPaused == value)
				{
					return;
				}
				this._simulationPaused = value;
			}
		}

		// Token: 0x06006544 RID: 25924 RVA: 0x00286146 File Offset: 0x00284346
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void EnsureRuntime()
		{
			if (RunModeRuntimeController._instance != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("[RunModeRuntime]");
			Object.DontDestroyOnLoad(gameObject);
			RunModeRuntimeController._instance = gameObject.AddComponent<RunModeRuntimeController>();
		}

		// Token: 0x06006545 RID: 25925 RVA: 0x00286170 File Offset: 0x00284370
		private void OnEnable()
		{
			SceneManager.sceneLoaded += this.OnSceneLoaded;
			SceneManager.activeSceneChanged += this.OnActiveSceneChanged;
			RunModeRuntimeController.ClearSessionAtHome(SceneManager.GetActiveScene());
			this.BeginApplyWhenReady();
		}

		// Token: 0x06006546 RID: 25926 RVA: 0x002861A4 File Offset: 0x002843A4
		private void OnDisable()
		{
			SceneManager.sceneLoaded -= this.OnSceneLoaded;
			SceneManager.activeSceneChanged -= this.OnActiveSceneChanged;
			if (this._applyRoutine != null)
			{
				base.StopCoroutine(this._applyRoutine);
			}
			this._applyRoutine = null;
		}

		// Token: 0x06006547 RID: 25927 RVA: 0x002861E4 File Offset: 0x002843E4
		private void OnSceneLoaded(Scene scene, LoadSceneMode __)
		{
			RunModeRuntimeController.ClearSessionAtHome(scene);
			if (scene.IsValid() && scene.name == "CielHome")
			{
				this._azure = null;
				this._azureInstanceId = 0;
				this._azureSwitchesApplied = false;
			}
			if (RunModeRuntimeController.IsSupportedActiveWorld())
			{
				this.BeginApplyWhenReady();
			}
		}

		// Token: 0x06006548 RID: 25928 RVA: 0x00286238 File Offset: 0x00284438
		private void OnActiveSceneChanged(Scene previous, Scene current)
		{
			RunModeRuntimeController.ClearSessionAtHome(current);
			bool flag = previous.IsValid() && DynamicWorldEnvironmentController.IsSupportedWorldScene(previous.name);
			bool flag2 = current.IsValid() && DynamicWorldEnvironmentController.IsSupportedWorldScene(current.name);
			if (flag && !flag2)
			{
				this._azureSwitchesApplied = false;
				this._nextWeatherChange = 0f;
			}
			if (current.IsValid() && current.name == "CielHome")
			{
				this._azure = null;
				this._azureInstanceId = 0;
				this._azureSwitchesApplied = false;
			}
			if (flag2)
			{
				this.BeginApplyWhenReady();
			}
		}

		// Token: 0x06006549 RID: 25929 RVA: 0x002862CD File Offset: 0x002844CD
		private static void ClearSessionAtHome(Scene scene)
		{
			if (scene.IsValid() && scene.name == "CielHome")
			{
				RunModeService.ClearPreparedRun();
				RankProgressionService.CompletePendingPromotionAtHome();
				SensoryManager instance = SensoryManager.Instance;
				if (instance == null)
				{
					return;
				}
				instance.SetRunModeDetectionEnabled(true);
			}
		}

		// Token: 0x0600654A RID: 25930 RVA: 0x00286306 File Offset: 0x00284506
		private void BeginApplyWhenReady()
		{
			if (this._applyRoutine != null)
			{
				base.StopCoroutine(this._applyRoutine);
			}
			this._applyRoutine = base.StartCoroutine(this.ApplyWhenReady());
		}

		// Token: 0x0600654B RID: 25931 RVA: 0x0028632E File Offset: 0x0028452E
		private IEnumerator ApplyWhenReady()
		{
			yield return null;
			if (!RunModeRuntimeController.IsSupportedActiveWorld())
			{
				this._applyRoutine = null;
				yield break;
			}
			int i = 0;
			while (i < 120)
			{
				while (this._simulationPaused)
				{
					yield return null;
				}
				if (!RunModeRuntimeController.IsSupportedActiveWorld())
				{
					this._applyRoutine = null;
					yield break;
				}
				RunModeRuntimeController.ApplyDetectionSwitch();
				if (this._azure == null)
				{
					AzureCoreSystem azureCoreSystem = RunModeRuntimeController.FindCoreAzure();
					if (azureCoreSystem != null)
					{
						int instanceID = azureCoreSystem.GetInstanceID();
						if (this._azureInstanceId != instanceID)
						{
							this._azureInstanceId = instanceID;
							this._azureSwitchesApplied = false;
						}
						this._azure = azureCoreSystem;
					}
				}
				if (!(this._azure != null) || !DynamicWorldEnvironmentController.IsEnvironmentReady(this._azure))
				{
					yield return null;
					int num = i;
					i = num + 1;
				}
				else
				{
					IL_0144:
					while (this._simulationPaused)
					{
						yield return null;
					}
					if (!RunModeRuntimeController.IsSupportedActiveWorld())
					{
						this._applyRoutine = null;
						yield break;
					}
					RunModeRuntimeController.ApplyDetectionSwitch();
					if (this._azure != null && DynamicWorldEnvironmentController.IsEnvironmentReady(this._azure))
					{
						this.ApplyAzureSwitches();
					}
					else
					{
						Debug.LogWarning("[RunModes] Azure environment was not ready after 120 frames.");
					}
					this._applyRoutine = null;
					yield break;
				}
			}
			goto IL_0144;
		}

		// Token: 0x0600654C RID: 25932 RVA: 0x00286340 File Offset: 0x00284540
		private void Update()
		{
			if (this.UpdateEffectivePause() || !RunModeRuntimeController.IsSupportedActiveWorld())
			{
				return;
			}
			RunModeRuntimeController.ApplyDetectionSwitch();
			if (!this._azureSwitchesApplied || !RunModeService.VariableWeather || this._azure == null || !DynamicWorldEnvironmentController.IsEnvironmentReady(this._azure) || Time.unscaledTime < this._nextWeatherChange)
			{
				return;
			}
			AzureWeatherSystem weatherSystem = this._azure.weatherSystem;
			if (weatherSystem == null || weatherSystem.isWeatherChanging)
			{
				return;
			}
			this.RotateWeather();
		}

		// Token: 0x0600654D RID: 25933 RVA: 0x002863B8 File Offset: 0x002845B8
		private bool UpdateEffectivePause()
		{
			if (this._simulationPaused || Time.timeScale <= 0f)
			{
				if (!this._effectivePauseActive)
				{
					this._effectivePauseActive = true;
					this._effectivePauseStartedAt = Time.unscaledTime;
				}
				return true;
			}
			if (this._effectivePauseActive)
			{
				this._nextWeatherChange += Mathf.Max(0f, Time.unscaledTime - this._effectivePauseStartedAt);
				this._effectivePauseActive = false;
			}
			return false;
		}

		// Token: 0x0600654E RID: 25934 RVA: 0x00286430 File Offset: 0x00284630
		private static void ApplyDetectionSwitch()
		{
			if (SensoryManager.Instance != null)
			{
				SensoryManager.Instance.SetRunModeDetectionEnabled(RunModeService.DetectionEnabled);
			}
		}

		// Token: 0x0600654F RID: 25935 RVA: 0x00286450 File Offset: 0x00284650
		private void ApplyAzureSwitches()
		{
			if (this._azure == null || this._azure.timeSystem == null)
			{
				return;
			}
			if (RunModeService.CurrentMode != RunModeKind.Free)
			{
				return;
			}
			if (this._azureSwitchesApplied)
			{
				return;
			}
			this._azure.timeSystem.startTime = (float)RunModeService.StartHour;
			this._azure.timeSystem.timeline = (float)RunModeService.StartHour;
			this._azure.timeSystem.dayLength = (RunModeService.TimeProgressionEnabled ? RunModeService.DayLengthMinutes : 0f);
			AzureWeatherSystem weatherSystem = this._azure.weatherSystem;
			if (weatherSystem != null && weatherSystem.globalWeatherList != null && weatherSystem.globalWeatherList.Count > 0)
			{
				weatherSystem.evaluationTime = this._azure.timeSystem.evaluationTime;
				int num = DynamicWorldEnvironmentController.ResolveSceneWeatherIndex(weatherSystem, RunModeRuntimeController.ResolveWeatherIndex(weatherSystem), SceneManager.GetActiveScene().name);
				if (num >= 0)
				{
					weatherSystem.SetGlobalWeatherInstantly(num);
				}
			}
			if (RunModeService.VariableWeather)
			{
				this._nextWeatherChange = Time.unscaledTime + RunModeService.WeatherChangeIntervalMinutes * 60f;
			}
			this._azureSwitchesApplied = true;
			EnvironmentLightingRefresh.ForceRequestForActiveScene();
			Debug.Log(string.Concat(new string[]
			{
				string.Format("[RunModes] Runtime applied: mode={0}, ", RunModeService.CurrentMode),
				string.Format("detection={0}, time={1}, ", RunModeService.DetectionEnabled, RunModeService.TimeProgressionEnabled),
				"weather=",
				RunModeService.VariableWeather ? "variable" : "fixed",
				", items=",
				RunModeService.UnlimitedItemDurability ? "unlimited" : string.Format("x{0:0.##}", RunModeService.ItemDurationMultiplier),
				", clothes=",
				RunModeService.UnlimitedClothingDurability ? "unlimited" : string.Format("x{0:0.##}", RunModeService.ClothingDurationMultiplier),
				"."
			}));
		}

		// Token: 0x06006550 RID: 25936 RVA: 0x00286630 File Offset: 0x00284830
		private void RotateWeather()
		{
			AzureWeatherSystem weatherSystem = this._azure.weatherSystem;
			if (weatherSystem == null || weatherSystem.globalWeatherList == null || weatherSystem.globalWeatherList.Count < 2)
			{
				this._nextWeatherChange = Time.unscaledTime + RunModeService.WeatherChangeIntervalMinutes * 60f;
				return;
			}
			int globalWeatherIndex = weatherSystem.globalWeatherIndex;
			string[] array = new string[weatherSystem.globalWeatherList.Count];
			for (int i = 0; i < weatherSystem.globalWeatherList.Count; i++)
			{
				AzureGlobalWeather azureGlobalWeather = weatherSystem.globalWeatherList[i];
				array[i] = ((azureGlobalWeather != null && azureGlobalWeather.weatherPreset != null) ? azureGlobalWeather.weatherPreset.name : string.Empty);
			}
			int num = DynamicWeatherPolicy.SelectNextIndex(array, globalWeatherIndex, Random.value, (this._azure.timeSystem != null) ? this._azure.timeSystem.timeline : ((float)RunModeService.StartHour), RunModeService.AllowedWeatherPresetNames, DynamicWeatherPolicy.AllowsFogInScene(SceneManager.GetActiveScene().name));
			if (num >= 0 && num != globalWeatherIndex)
			{
				weatherSystem.SetGlobalWeather(num);
			}
			this._nextWeatherChange = Time.unscaledTime + RunModeService.WeatherChangeIntervalMinutes * 60f;
		}

		// Token: 0x06006551 RID: 25937 RVA: 0x00286758 File Offset: 0x00284958
		private static int ResolveWeatherIndex(AzureWeatherSystem weather)
		{
			string text = RunModeRuntimeController.NormalizeWeatherName(RunModeService.WeatherPresetName);
			string[] allowedWeatherPresetNames = RunModeService.AllowedWeatherPresetNames;
			int num = -1;
			int num2 = -1;
			for (int i = 0; i < weather.globalWeatherList.Count; i++)
			{
				AzureGlobalWeather azureGlobalWeather = weather.globalWeatherList[i];
				if (azureGlobalWeather != null && !(azureGlobalWeather.weatherPreset == null))
				{
					string text2 = RunModeRuntimeController.NormalizeWeatherName(azureGlobalWeather.weatherPreset.name);
					if (text2 == text)
					{
						num = i;
					}
					int j = 0;
					while (j < allowedWeatherPresetNames.Length)
					{
						if (text2 == RunModeRuntimeController.NormalizeWeatherName(allowedWeatherPresetNames[j]))
						{
							if (num2 < 0)
							{
								num2 = i;
								break;
							}
							break;
						}
						else
						{
							j++;
						}
					}
				}
			}
			if (num2 >= 0)
			{
				if (num >= 0)
				{
					string text3 = RunModeRuntimeController.NormalizeWeatherName(weather.globalWeatherList[num].weatherPreset.name);
					for (int k = 0; k < allowedWeatherPresetNames.Length; k++)
					{
						if (text3 == RunModeRuntimeController.NormalizeWeatherName(allowedWeatherPresetNames[k]))
						{
							return num;
						}
					}
				}
				return num2;
			}
			if (num < 0)
			{
				return Mathf.Clamp(RunModeService.WeatherIndex, 0, weather.globalWeatherList.Count - 1);
			}
			return num;
		}

		// Token: 0x06006552 RID: 25938 RVA: 0x00286874 File Offset: 0x00284A74
		private static string NormalizeWeatherName(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				return value.Replace("_", string.Empty).Replace("-", string.Empty).Replace(" ", string.Empty)
					.ToUpperInvariant()
					.Replace("CONSTALLATION", "CONSTELLATION");
			}
			return string.Empty;
		}

		// Token: 0x06006553 RID: 25939 RVA: 0x002868D4 File Offset: 0x00284AD4
		private static bool IsSupportedActiveWorld()
		{
			Scene activeScene = SceneManager.GetActiveScene();
			return activeScene.IsValid() && DynamicWorldEnvironmentController.IsSupportedWorldScene(activeScene.name);
		}

		// Token: 0x06006554 RID: 25940 RVA: 0x00286900 File Offset: 0x00284B00
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

		// Token: 0x040067A1 RID: 26529
		private static RunModeRuntimeController _instance;

		// Token: 0x040067A2 RID: 26530
		private AzureCoreSystem _azure;

		// Token: 0x040067A3 RID: 26531
		private Coroutine _applyRoutine;

		// Token: 0x040067A4 RID: 26532
		private int _azureInstanceId;

		// Token: 0x040067A5 RID: 26533
		private bool _azureSwitchesApplied;

		// Token: 0x040067A6 RID: 26534
		private float _nextWeatherChange;

		// Token: 0x040067A7 RID: 26535
		private bool _simulationPaused;

		// Token: 0x040067A8 RID: 26536
		private bool _effectivePauseActive;

		// Token: 0x040067A9 RID: 26537
		private float _effectivePauseStartedAt;
	}
}
