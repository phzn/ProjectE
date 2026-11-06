using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AzureSky;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace ProjectExhibitionist.Core.Rendering
{
	// Token: 0x02000A8F RID: 2703
	[DefaultExecutionOrder(-31990)]
	public sealed class EnvironmentLightingRefresh : MonoBehaviour
	{
		// Token: 0x17000CFF RID: 3327
		// (get) Token: 0x06004CE4 RID: 19684 RVA: 0x001DDFCC File Offset: 0x001DC1CC
		// (set) Token: 0x06004CE5 RID: 19685 RVA: 0x001DDFD3 File Offset: 0x001DC1D3
		public static bool IsReady { get; private set; } = true;

		// Token: 0x06004CE6 RID: 19686 RVA: 0x001DDFDB File Offset: 0x001DC1DB
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			EnvironmentLightingRefresh._instance = null;
			EnvironmentLightingRefresh.IsReady = true;
		}

		// Token: 0x06004CE7 RID: 19687 RVA: 0x001DDFE9 File Offset: 0x001DC1E9
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Bootstrap()
		{
			EnvironmentLightingRefresh.EnsureInstance();
		}

		// Token: 0x06004CE8 RID: 19688 RVA: 0x001DDFF1 File Offset: 0x001DC1F1
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void RefreshInitialScene()
		{
			EnvironmentLightingRefresh.RequestForActiveScene();
		}

		// Token: 0x06004CE9 RID: 19689 RVA: 0x001DDFF8 File Offset: 0x001DC1F8
		public static void RequestForActiveScene()
		{
			EnvironmentLightingRefresh.EnsureInstance().Request(SceneManager.GetActiveScene(), false);
		}

		// Token: 0x06004CEA RID: 19690 RVA: 0x001DE00A File Offset: 0x001DC20A
		public static void ForceRequestForActiveScene()
		{
			EnvironmentLightingRefresh.EnsureInstance().Request(SceneManager.GetActiveScene(), true);
		}

		// Token: 0x06004CEB RID: 19691 RVA: 0x001DE01C File Offset: 0x001DC21C
		public static IEnumerator WaitUntilReady(float timeoutSeconds = 12f)
		{
			EnvironmentLightingRefresh.EnsureInstance();
			float deadline = Time.realtimeSinceStartup + Mathf.Max(1f, timeoutSeconds);
			while (!EnvironmentLightingRefresh.IsReady)
			{
				if (Time.realtimeSinceStartup >= deadline)
				{
					Debug.LogError("[EnvironmentLightingRefresh] Global safety deadline reached; releasing the transition while the refresh remains best-effort.");
					EnvironmentLightingRefresh.IsReady = true;
					yield break;
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x06004CEC RID: 19692 RVA: 0x001DE02C File Offset: 0x001DC22C
		private static EnvironmentLightingRefresh EnsureInstance()
		{
			if (EnvironmentLightingRefresh._instance != null)
			{
				return EnvironmentLightingRefresh._instance;
			}
			EnvironmentLightingRefresh._instance = Object.FindObjectOfType<EnvironmentLightingRefresh>(true);
			if (EnvironmentLightingRefresh._instance != null)
			{
				return EnvironmentLightingRefresh._instance;
			}
			GameObject gameObject = new GameObject("EnvironmentLightingRefresh");
			gameObject.hideFlags = HideFlags.HideInHierarchy;
			Object.DontDestroyOnLoad(gameObject);
			EnvironmentLightingRefresh._instance = gameObject.AddComponent<EnvironmentLightingRefresh>();
			return EnvironmentLightingRefresh._instance;
		}

		// Token: 0x06004CED RID: 19693 RVA: 0x001DE090 File Offset: 0x001DC290
		private void OnEnable()
		{
			SceneManager.activeSceneChanged += this.HandleActiveSceneChanged;
		}

		// Token: 0x06004CEE RID: 19694 RVA: 0x001DE0A3 File Offset: 0x001DC2A3
		private void OnDisable()
		{
			SceneManager.activeSceneChanged -= this.HandleActiveSceneChanged;
			this.CancelRefresh();
		}

		// Token: 0x06004CEF RID: 19695 RVA: 0x001DE0BC File Offset: 0x001DC2BC
		private void OnDestroy()
		{
			this.DetachCoreUpdateWait();
			this.DetachGameCameraRenderWait();
			if (EnvironmentLightingRefresh._instance == this)
			{
				EnvironmentLightingRefresh._instance = null;
			}
		}

		// Token: 0x06004CF0 RID: 19696 RVA: 0x001DE0DD File Offset: 0x001DC2DD
		private void HandleActiveSceneChanged(Scene previous, Scene next)
		{
			this.Request(next, false);
		}

		// Token: 0x06004CF1 RID: 19697 RVA: 0x001DE0E8 File Offset: 0x001DC2E8
		private void Request(Scene scene, bool force = false)
		{
			if (!EnvironmentLightingRefresh.RequiresExplicitRefresh(scene))
			{
				this.CancelRefresh();
				return;
			}
			if (!force && this._requestedSceneHandle == scene.handle && (this._refreshRoutine != null || this._lastRefreshedSceneHandle == scene.handle))
			{
				return;
			}
			this.DetachCoreUpdateWait();
			this.DetachGameCameraRenderWait();
			if (this._refreshRoutine != null)
			{
				base.StopCoroutine(this._refreshRoutine);
			}
			this._refreshRoutine = null;
			if (force)
			{
				this._lastRefreshedSceneHandle = -1;
			}
			this._requestedSceneHandle = scene.handle;
			EnvironmentLightingRefresh.IsReady = false;
			this._refreshRoutine = base.StartCoroutine(this.RefreshEnvironment(scene.handle));
		}

		// Token: 0x06004CF2 RID: 19698 RVA: 0x001DE18B File Offset: 0x001DC38B
		private IEnumerator RefreshEnvironment(int sceneHandle)
		{
			while (!EnvironmentLightingRefresh.IsCoreLoaded())
			{
				if (!this.IsStillRequested(sceneHandle))
				{
					this.CompleteCancelledRefresh();
					yield break;
				}
				yield return null;
			}
			AzureCoreSystem azure = EnvironmentLightingRefresh.FindCoreAzure();
			float azureDeadline = Time.realtimeSinceStartup + 1f;
			while (azure == null && Time.realtimeSinceStartup < azureDeadline)
			{
				if (!this.IsStillRequested(sceneHandle))
				{
					this.CompleteCancelledRefresh();
					yield break;
				}
				yield return null;
				azure = EnvironmentLightingRefresh.FindCoreAzure();
			}
			if (this.BeginCoreUpdateWait(azure))
			{
				float coreDeadline = Time.realtimeSinceStartup + EnvironmentLightingRefresh.GetCoreUpdateWaitTimeout(azure.refreshRate);
				while (!this._coreUpdated && Time.realtimeSinceStartup < coreDeadline)
				{
					if (!this.IsStillRequested(sceneHandle))
					{
						this.CompleteCancelledRefresh();
						yield break;
					}
					yield return null;
				}
				bool coreUpdated = this._coreUpdated;
				this.DetachCoreUpdateWait();
				if (!coreUpdated && !EnvironmentLightingRefresh.TryPublishAzureState(azure))
				{
					Debug.LogWarning("[EnvironmentLightingRefresh] Azure Core did not publish a tick and its state could not be synchronized before the one-shot refresh; continuing best-effort.");
				}
			}
			else
			{
				yield return null;
				yield return null;
			}
			if (!this.IsStillRequested(sceneHandle))
			{
				this.CompleteCancelledRefresh();
				yield break;
			}
			EnvironmentLightingRefresh.LogStep("UpdateEnvironment (initial) start");
			DynamicGI.UpdateEnvironment();
			EnvironmentLightingRefresh.LogStep("UpdateEnvironment (initial) end");
			yield return this.WaitForGameCameraRender(sceneHandle, "initial environment publication");
			if (!this.IsStillRequested(sceneHandle))
			{
				this.CompleteCancelledRefresh();
				yield break;
			}
			EnvironmentLightingRefresh.LogStep("UpdateEnvironment (final) start");
			DynamicGI.UpdateEnvironment();
			EnvironmentLightingRefresh.LogStep("UpdateEnvironment (final) end");
			ChunkManager chunkManager = ChunkManager.Instance;
			if (QualitySettings.realtimeReflectionProbes && chunkManager != null && chunkManager.IsAwaitingCatalog)
			{
				EnvironmentLightingRefresh.LogStep("Map catalog wait before probe start");
				while (QualitySettings.realtimeReflectionProbes && chunkManager != null && chunkManager.IsAwaitingCatalog)
				{
					if (!this.IsStillRequested(sceneHandle))
					{
						this.CompleteCancelledRefresh();
						yield break;
					}
					yield return null;
					chunkManager = ChunkManager.Instance;
				}
				EnvironmentLightingRefresh.LogStep("Map catalog wait before probe end");
			}
			if (!this.IsStillRequested(sceneHandle))
			{
				this.CompleteCancelledRefresh();
				yield break;
			}
			ReflectionProbe skyProbe = ((azure != null) ? azure.reflectionProbe : null);
			bool probeReady = !QualitySettings.realtimeReflectionProbes;
			if (!probeReady && skyProbe != null && skyProbe.enabled && skyProbe.gameObject.activeInHierarchy && skyProbe.mode == ReflectionProbeMode.Realtime)
			{
				int attempt = 0;
				while (attempt < 2 && !probeReady)
				{
					EnvironmentLightingRefresh.LogStep(string.Format("RenderProbe attempt {0} start", attempt + 1));
					int renderId = skyProbe.RenderProbe();
					if (renderId >= 0)
					{
						float coreDeadline = Time.realtimeSinceStartup + 5f;
						while (QualitySettings.realtimeReflectionProbes && skyProbe != null && !skyProbe.IsFinishedRendering(renderId) && Time.realtimeSinceStartup < coreDeadline)
						{
							if (!this.IsStillRequested(sceneHandle))
							{
								this.CompleteCancelledRefresh();
								yield break;
							}
							yield return null;
						}
						probeReady = !QualitySettings.realtimeReflectionProbes || (skyProbe != null && skyProbe.IsFinishedRendering(renderId));
					}
					EnvironmentLightingRefresh.LogStep(string.Format("RenderProbe attempt {0} end (id={1}, finished={2})", attempt + 1, renderId, probeReady));
					if (!probeReady && attempt + 1 < 2)
					{
						float coreDeadline = Time.realtimeSinceStartup + 0.25f;
						while (Time.realtimeSinceStartup < coreDeadline)
						{
							yield return null;
						}
					}
					int num = attempt;
					attempt = num + 1;
				}
				if (!probeReady)
				{
					Debug.LogWarning("[EnvironmentLightingRefresh] Azure sky probe did not finish after two one-shot attempts; the loading safety timeout was reached.");
				}
			}
			else if (!probeReady)
			{
				Debug.LogWarning("[EnvironmentLightingRefresh] Azure sky probe is missing or not an active Realtime probe; only the default environment was refreshed.");
			}
			yield return this.WaitForGameCameraRender(sceneHandle, "final environment publication");
			if (!this.IsStillRequested(sceneHandle))
			{
				this.CompleteCancelledRefresh();
				yield break;
			}
			this._lastRefreshedSceneHandle = (probeReady ? sceneHandle : (-1));
			this._refreshRoutine = null;
			EnvironmentLightingRefresh.IsReady = true;
			yield break;
		}

		// Token: 0x06004CF3 RID: 19699 RVA: 0x001DE1A1 File Offset: 0x001DC3A1
		public static float GetCoreUpdateWaitTimeout(float refreshRate)
		{
			return Mathf.Max(0.2f, Mathf.Max(0f, refreshRate) * 3f);
		}

		// Token: 0x06004CF4 RID: 19700 RVA: 0x001DE1C0 File Offset: 0x001DC3C0
		private static bool TryPublishAzureState(AzureCoreSystem azure)
		{
			if (azure == null || azure.timeSystem == null || azure.weatherSystem == null)
			{
				return false;
			}
			azure.timeSystem.UpdateTimeOfDay();
			azure.weatherSystem.evaluationTime = azure.timeSystem.evaluationTime;
			azure.timeSystem.UpdateCelestialBodies();
			azure.weatherSystem.UpdateWeatherSystem();
			azure.weatherSystem.OverrideTargetProperties();
			return true;
		}

		// Token: 0x06004CF5 RID: 19701 RVA: 0x001DE22C File Offset: 0x001DC42C
		private bool BeginCoreUpdateWait(AzureCoreSystem azure)
		{
			this.DetachCoreUpdateWait();
			if (azure == null || azure.onCoreUpdateEvent == null)
			{
				return false;
			}
			this._awaitedAzure = azure;
			this._coreUpdated = false;
			this._awaitedCoreUpdate = new UnityAction(this.HandleCoreUpdated);
			this._awaitedAzure.onCoreUpdateEvent.AddListener(this._awaitedCoreUpdate);
			return true;
		}

		// Token: 0x06004CF6 RID: 19702 RVA: 0x001DE289 File Offset: 0x001DC489
		private void HandleCoreUpdated()
		{
			this._coreUpdated = true;
		}

		// Token: 0x06004CF7 RID: 19703 RVA: 0x001DE294 File Offset: 0x001DC494
		private void DetachCoreUpdateWait()
		{
			if (this._awaitedAzure != null && this._awaitedCoreUpdate != null && this._awaitedAzure.onCoreUpdateEvent != null)
			{
				this._awaitedAzure.onCoreUpdateEvent.RemoveListener(this._awaitedCoreUpdate);
			}
			this._awaitedAzure = null;
			this._awaitedCoreUpdate = null;
			this._coreUpdated = false;
		}

		// Token: 0x06004CF8 RID: 19704 RVA: 0x001DE2EF File Offset: 0x001DC4EF
		private IEnumerator WaitForGameCameraRender(int sceneHandle, string stage)
		{
			EnvironmentLightingRefresh.LogStep("Game Camera render wait (" + stage + ") start");
			this.BeginGameCameraRenderWait();
			float deadline = Time.realtimeSinceStartup + 1.5f;
			while (!this._gameCameraRendered && Time.realtimeSinceStartup < deadline && this.IsStillRequested(sceneHandle))
			{
				yield return null;
			}
			bool gameCameraRendered = this._gameCameraRendered;
			this.DetachGameCameraRenderWait();
			EnvironmentLightingRefresh.LogStep(string.Format("Game Camera render wait ({0}) end (rendered={1})", stage, gameCameraRendered));
			if (!gameCameraRendered && this.IsStillRequested(sceneHandle))
			{
				Debug.LogWarning("[EnvironmentLightingRefresh] No active Game Camera rendered during " + stage + "; continuing after the realtime safety timeout.");
			}
			yield break;
		}

		// Token: 0x06004CF9 RID: 19705 RVA: 0x001DE30C File Offset: 0x001DC50C
		private static void LogStep(string step)
		{
			Debug.Log(string.Format("[EnvironmentLightingRefresh] {0}, t={1:F1}s", step, Time.realtimeSinceStartup));
		}

		// Token: 0x06004CFA RID: 19706 RVA: 0x001DE328 File Offset: 0x001DC528
		private void BeginGameCameraRenderWait()
		{
			this.DetachGameCameraRenderWait();
			this._gameCameraRendered = false;
			this._gameCameraRenderWaitAttached = true;
			Camera.onPostRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPostRender, new Camera.CameraCallback(this.HandleBuiltInCameraRendered));
			RenderPipelineManager.endCameraRendering += this.HandleSrpCameraRendered;
		}

		// Token: 0x06004CFB RID: 19707 RVA: 0x001DE37A File Offset: 0x001DC57A
		private void HandleBuiltInCameraRendered(Camera camera)
		{
			this.MarkGameCameraRendered(camera);
		}

		// Token: 0x06004CFC RID: 19708 RVA: 0x001DE383 File Offset: 0x001DC583
		private void HandleSrpCameraRendered(ScriptableRenderContext _, Camera camera)
		{
			this.MarkGameCameraRendered(camera);
		}

		// Token: 0x06004CFD RID: 19709 RVA: 0x001DE38C File Offset: 0x001DC58C
		private void MarkGameCameraRendered(Camera camera)
		{
			if (this._gameCameraRenderWaitAttached && EnvironmentLightingRefresh.IsEligibleGameCamera(camera))
			{
				this._gameCameraRendered = true;
			}
		}

		// Token: 0x06004CFE RID: 19710 RVA: 0x001DE3A5 File Offset: 0x001DC5A5
		public static bool IsEligibleGameCamera(Camera camera)
		{
			return camera != null && camera.cameraType == CameraType.Game && camera.isActiveAndEnabled && camera.gameObject.activeInHierarchy && camera.targetTexture == null;
		}

		// Token: 0x06004CFF RID: 19711 RVA: 0x001DE3DC File Offset: 0x001DC5DC
		private void DetachGameCameraRenderWait()
		{
			if (this._gameCameraRenderWaitAttached)
			{
				Camera.onPostRender = (Camera.CameraCallback)Delegate.Remove(Camera.onPostRender, new Camera.CameraCallback(this.HandleBuiltInCameraRendered));
				RenderPipelineManager.endCameraRendering -= this.HandleSrpCameraRendered;
			}
			this._gameCameraRenderWaitAttached = false;
			this._gameCameraRendered = false;
		}

		// Token: 0x06004D00 RID: 19712 RVA: 0x001DE430 File Offset: 0x001DC630
		private static AzureCoreSystem FindCoreAzure()
		{
			foreach (AzureCoreSystem azureCoreSystem in Object.FindObjectsOfType<AzureCoreSystem>(true))
			{
				if (azureCoreSystem != null && azureCoreSystem.isActiveAndEnabled && azureCoreSystem.gameObject.scene.IsValid() && azureCoreSystem.gameObject.scene.name == "_Core")
				{
					return azureCoreSystem;
				}
			}
			return null;
		}

		// Token: 0x06004D01 RID: 19713 RVA: 0x001DE4A0 File Offset: 0x001DC6A0
		private bool IsStillRequested(int sceneHandle)
		{
			Scene activeScene = SceneManager.GetActiveScene();
			return this._requestedSceneHandle == sceneHandle && activeScene.IsValid() && activeScene.isLoaded && activeScene.handle == sceneHandle;
		}

		// Token: 0x06004D02 RID: 19714 RVA: 0x001DE4DC File Offset: 0x001DC6DC
		private static bool IsCoreLoaded()
		{
			Scene sceneByName = SceneManager.GetSceneByName("_Core");
			return sceneByName.IsValid() && sceneByName.isLoaded;
		}

		// Token: 0x06004D03 RID: 19715 RVA: 0x001DE506 File Offset: 0x001DC706
		private static bool RequiresExplicitRefresh(Scene scene)
		{
			return scene.IsValid() && scene.isLoaded && (scene.name == "Game - Current" || scene.name.StartsWith("POI - ", StringComparison.Ordinal));
		}

		// Token: 0x06004D04 RID: 19716 RVA: 0x001DE543 File Offset: 0x001DC743
		private void CancelRefresh()
		{
			this.DetachCoreUpdateWait();
			this.DetachGameCameraRenderWait();
			if (this._refreshRoutine != null)
			{
				base.StopCoroutine(this._refreshRoutine);
			}
			this._refreshRoutine = null;
			this._requestedSceneHandle = -1;
			this._lastRefreshedSceneHandle = -1;
			EnvironmentLightingRefresh.IsReady = true;
		}

		// Token: 0x06004D05 RID: 19717 RVA: 0x001DE580 File Offset: 0x001DC780
		private void CompleteCancelledRefresh()
		{
			this.DetachCoreUpdateWait();
			this.DetachGameCameraRenderWait();
			this._refreshRoutine = null;
			EnvironmentLightingRefresh.IsReady = true;
		}

		// Token: 0x04004E6A RID: 20074
		private const string CoreSceneName = "_Core";

		// Token: 0x04004E6B RID: 20075
		private const float ProbeRenderSafetyTimeout = 5f;

		// Token: 0x04004E6C RID: 20076
		private const float GameCameraRenderSafetyTimeout = 1.5f;

		// Token: 0x04004E6D RID: 20077
		private static EnvironmentLightingRefresh _instance;

		// Token: 0x04004E6F RID: 20079
		private Coroutine _refreshRoutine;

		// Token: 0x04004E70 RID: 20080
		private int _requestedSceneHandle = -1;

		// Token: 0x04004E71 RID: 20081
		private int _lastRefreshedSceneHandle = -1;

		// Token: 0x04004E72 RID: 20082
		private AzureCoreSystem _awaitedAzure;

		// Token: 0x04004E73 RID: 20083
		private UnityAction _awaitedCoreUpdate;

		// Token: 0x04004E74 RID: 20084
		private bool _coreUpdated;

		// Token: 0x04004E75 RID: 20085
		private bool _gameCameraRenderWaitAttached;

		// Token: 0x04004E76 RID: 20086
		private bool _gameCameraRendered;
	}
}
