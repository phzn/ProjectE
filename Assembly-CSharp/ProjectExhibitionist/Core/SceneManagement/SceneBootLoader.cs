using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Features.Loading;
using Features.Rank;
using Features.RunModes;
using Invector.vCamera;
using ProjectExhibitionist.Core.Rendering;
using ProjectExhibitionist.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectExhibitionist.Core.SceneManagement
{
	// Token: 0x02000A7C RID: 2684
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(-32000)]
	public class SceneBootLoader : MonoBehaviour
	{
		// Token: 0x17000CE1 RID: 3297
		// (get) Token: 0x06004C5C RID: 19548 RVA: 0x001DAD92 File Offset: 0x001D8F92
		// (set) Token: 0x06004C5D RID: 19549 RVA: 0x001DAD99 File Offset: 0x001D8F99
		public static bool CoreReady { get; private set; }

		// Token: 0x1400007D RID: 125
		// (add) Token: 0x06004C5E RID: 19550 RVA: 0x001DADA4 File Offset: 0x001D8FA4
		// (remove) Token: 0x06004C5F RID: 19551 RVA: 0x001DADD8 File Offset: 0x001D8FD8
		public static event Action OnCoreReady;

		// Token: 0x17000CE2 RID: 3298
		// (get) Token: 0x06004C60 RID: 19552 RVA: 0x001DAE0B File Offset: 0x001D900B
		public static int InitializationWorkVersion
		{
			get
			{
				return SceneBootLoader._coreLoadWorkVersion;
			}
		}

		// Token: 0x06004C61 RID: 19553 RVA: 0x001DAE12 File Offset: 0x001D9012
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			SceneBootLoader.CoreReady = false;
			SceneBootLoader._readyCoreSceneHandle = -1;
			SceneBootLoader.PendingCoreLoads.Clear();
			SceneBootLoader.PendingCoreLoadProgress.Clear();
			SceneBootLoader._coreLoadWorkVersion = 0;
			SceneBootLoader.OnCoreReady = null;
			SceneManager.sceneUnloaded -= SceneBootLoader.OnCoreSceneUnloaded;
		}

		// Token: 0x06004C62 RID: 19554 RVA: 0x001DAE54 File Offset: 0x001D9054
		private void Awake()
		{
			SceneTransitionManager instance = SceneTransitionManager.Instance;
			if (instance == null || !instance.IsTransitioning)
			{
				GameSystemsBootstrapper.InvalidateForSceneTransition();
			}
			LoadingScreenController instance2 = LoadingScreenController.Instance;
			if (instance2 != null && !instance2.IsLoadingActive)
			{
				instance2.BeginManualLoad();
				this._ownsBootLoading = true;
			}
			base.StartCoroutine(this.EnsureCoreLoaded());
			if (this._ownsBootLoading)
			{
				base.StartCoroutine(this.EndOwnedLoadingWhenReady());
			}
		}

		// Token: 0x06004C63 RID: 19555 RVA: 0x001DAEC3 File Offset: 0x001D90C3
		private IEnumerator EndOwnedLoadingWhenReady()
		{
			double waitStartedAt = Time.realtimeSinceStartupAsDouble;
			LoadingReadinessWatchdog watchdog = new LoadingReadinessWatchdog(Mathf.Max(5f, this.readinessSafetyTimeout));
			LoadingScreenController loading = LoadingScreenController.Instance;
			bool gameplayReadyReported = false;
			bool degraded = false;
			try
			{
				while (!GameSystemsBootstrapper.GameplayReadinessFailed)
				{
					bool flag = RankRuntimeController.InitialPresentationReady || Object.FindObjectOfType<RankRuntimeController>() == null;
					LongWayHomeRestaurantSequence presenterForScene = LongWayHomeRestaurantSequence.GetPresenterForScene(base.gameObject.scene);
					bool flag2 = presenterForScene == null || !presenterForScene.OwnsDestinationReveal || presenterForScene.IsPreparedForReveal;
					ILongWayHomePoiProloguePresenter presenterForScene2 = LongWayHomePoiPrologue.GetPresenterForScene(base.gameObject.scene);
					flag2 &= presenterForScene2 == null || !presenterForScene2.OwnsDestinationReveal || presenterForScene2.IsPreparedForReveal;
					if (!gameplayReadyReported && GameSystemsBootstrapper.GameplayReady)
					{
						gameplayReadyReported = true;
						MemoryReport.Log(string.Format("boot-gameplay-ready t={0:F1}s", Time.realtimeSinceStartup));
					}
					if (SceneBootLoader.CoreReady && GameSystemsBootstrapper.GameplayReady && flag && EnvironmentLightingRefresh.IsReady && flag2)
					{
						LoadingStallMonitor.Report(this, false, null);
					}
					else
					{
						float num = (float)(Time.realtimeSinceStartupAsDouble - waitStartedAt);
						if (loading != null)
						{
							float num2 = ((!SceneBootLoader.CoreReady) ? Mathf.Lerp(0.02f, 0.08f, Mathf.Clamp01(num / 2f)) : ((!GameSystemsBootstrapper.GameplayReady) ? (0.08f + GameSystemsBootstrapper.InitializationProgress * 0.86f) : 0.97f));
							loading.ReportProgress(num2);
						}
						int num3 = (SceneBootLoader.CoreReady ? 1 : 0) | (GameSystemsBootstrapper.GameplayReady ? 2 : 0) | (flag ? 4 : 0) | (EnvironmentLightingRefresh.IsReady ? 8 : 0) | (flag2 ? 16 : 0);
						if (!watchdog.Tick(Time.realtimeSinceStartupAsDouble, GameSystemsBootstrapper.InitializationProgress, GameSystemsBootstrapper.InitializationWorkVersion, num3))
						{
							LoadingStallMonitor.Report(this, false, null);
							yield return null;
							continue;
						}
						string stall = string.Format("SceneBootLoader stalled {0:0.#}s after {1:0.#}s total", watchdog.StalledSeconds, num);
						bool flag3 = GameSystemsBootstrapper.ForceCompleteGameplayReadiness(stall);
						if (!flag3 && !GameSystemsBootstrapper.GameplayReadinessFailed)
						{
							LoadingStallMonitor.Report(this, true, () => stall);
							yield return null;
							continue;
						}
						if (!flag3 || !SceneBootLoader.CoreReady)
						{
							GameSystemsBootstrapper.FailGameplayReadiness("Core or required gameplay dependencies remain unavailable.");
							LoadingScreenController loadingScreenController = loading;
							if (loadingScreenController != null)
							{
								loadingScreenController.ShowLoadFailure();
							}
							yield break;
						}
						if (!flag2)
						{
							SceneBootLoader.FailPendingOpening(presenterForScene, presenterForScene2);
						}
						Debug.LogError(string.Concat(new string[]
						{
							string.Format("[SceneBootLoader] No initialization progress for {0:0.#}s ", watchdog.StalledSeconds),
							string.Format("({0:0.#}s total); releasing owned ", num),
							string.Format("loading in degraded mode (core={0}, ", SceneBootLoader.CoreReady),
							string.Format("gameplay={0}, ", GameSystemsBootstrapper.GameplayReady),
							string.Format("rank={0}, lighting={1}, restaurant={2}).", flag, EnvironmentLightingRefresh.IsReady, flag2)
						}));
						degraded = true;
					}
					goto JumpOutOfTryFinally-3;
				}
				LoadingScreenController loadingScreenController2 = loading;
				if (loadingScreenController2 != null)
				{
					loadingScreenController2.ShowLoadFailure();
				}
				yield break;
			}
			finally
			{
				LoadingStallMonitor.Remove(this);
			}
			JumpOutOfTryFinally-3:
			MemoryReport.Log(string.Format("{0} t={1:F1}s", degraded ? "boot-ready-degraded" : "boot-ready", Time.realtimeSinceStartup));
			if (loading != null)
			{
				yield return loading.WaitForLongWayHomeOpening();
				loading.EndManualLoad();
			}
			yield break;
			yield break;
		}

		// Token: 0x06004C64 RID: 19556 RVA: 0x001DAED2 File Offset: 0x001D90D2
		private static void FailPendingOpening(LongWayHomeRestaurantSequence restaurantPresenter, ILongWayHomePoiProloguePresenter poiPresenter)
		{
			if (restaurantPresenter != null)
			{
				restaurantPresenter.FailPreparation("The opening presentation did not finish before the boot loading deadline.");
			}
			if (poiPresenter != null)
			{
				poiPresenter.FailPreparation("The opening presentation did not finish before the boot loading deadline.");
			}
		}

		// Token: 0x06004C65 RID: 19557 RVA: 0x001DAEF0 File Offset: 0x001D90F0
		private IEnumerator EnsureCoreLoaded()
		{
			SceneBootLoader.CoreReady = false;
			yield return null;
			if (this.IsCoreLoaded())
			{
				if (this.enableDebugLogs)
				{
					Debug.Log("[SceneBootLoader] '" + this.coreSceneName + "' già caricata. Nessuna azione.");
				}
				if (this.makeThisSceneActive && base.gameObject.scene.IsValid() && base.gameObject.scene.isLoaded)
				{
					SceneManager.SetActiveScene(base.gameObject.scene);
				}
				SceneTransitionManager instance = SceneTransitionManager.Instance;
				if (!(instance != null) || !instance.IsTransitioning)
				{
					this.RepositionPlayer();
				}
				this.MarkReady();
				yield break;
			}
			Stopwatch swCore = Stopwatch.StartNew();
			AsyncOperation op;
			if (!SceneBootLoader.PendingCoreLoads.TryGetValue(this.coreSceneName, out op) || op == null || op.isDone)
			{
				if (this.enableDebugLogs)
				{
					Debug.Log("[SceneBootLoader] Carico '" + this.coreSceneName + "' in additive...");
				}
				op = SceneManager.LoadSceneAsync(this.coreSceneName, LoadSceneMode.Additive);
				if (op != null)
				{
					SceneBootLoader.PendingCoreLoads[this.coreSceneName] = op;
					SceneBootLoader.PendingCoreLoadProgress.Remove(this.coreSceneName);
				}
			}
			if (op == null)
			{
				Debug.LogError("[SceneBootLoader] Impossibile caricare '" + this.coreSceneName + "'. È nelle Build Settings? È scritta correttamente?");
				yield break;
			}
			long msFile = -1L;
			while (!op.isDone)
			{
				SceneBootLoader.ObserveOwnedCoreLoadProgress(this.coreSceneName, op);
				if (msFile < 0L && op.progress >= 0.9f)
				{
					msFile = swCore.ElapsedMilliseconds;
				}
				yield return null;
			}
			SceneBootLoader.ObserveOwnedCoreLoadProgress(this.coreSceneName, op);
			AsyncOperation asyncOperation;
			if (SceneBootLoader.PendingCoreLoads.TryGetValue(this.coreSceneName, out asyncOperation) && asyncOperation == op)
			{
				SceneBootLoader.PendingCoreLoads.Remove(this.coreSceneName);
				SceneBootLoader.PendingCoreLoadProgress.Remove(this.coreSceneName);
			}
			if (msFile < 0L)
			{
				msFile = swCore.ElapsedMilliseconds;
			}
			Debug.Log(string.Format("[SceneBootLoader] '{0}': file load {1:F1}s, ", this.coreSceneName, (float)msFile / 1000f) + string.Format("activation {0:F1}s.", (float)(swCore.ElapsedMilliseconds - msFile) / 1000f));
			MemoryReport.Log(string.Format("boot-core-activated t={0:F1}s", Time.realtimeSinceStartup));
			if (this.makeThisSceneActive)
			{
				SceneManager.SetActiveScene(base.gameObject.scene);
			}
			if (this.enableDebugLogs)
			{
				Debug.Log(string.Concat(new string[]
				{
					"[SceneBootLoader] '",
					this.coreSceneName,
					"' caricata. Scena attiva: '",
					SceneManager.GetActiveScene().name,
					"'."
				}));
			}
			this.RepositionPlayer();
			this.MarkReady();
			yield break;
		}

		// Token: 0x06004C66 RID: 19558 RVA: 0x001DAF00 File Offset: 0x001D9100
		private static void ObserveOwnedCoreLoadProgress(string sceneName, AsyncOperation operation)
		{
			AsyncOperation asyncOperation;
			if (SceneBootLoader.PendingCoreLoads.TryGetValue(sceneName, out asyncOperation) && asyncOperation == operation)
			{
				SceneBootLoader.ObserveCoreLoadProgress(sceneName, operation.progress);
			}
		}

		// Token: 0x06004C67 RID: 19559 RVA: 0x001DAF2C File Offset: 0x001D912C
		private static void ObserveCoreLoadProgress(string sceneName, float progress)
		{
			float num;
			SceneBootLoader.PendingCoreLoadProgress.TryGetValue(sceneName, out num);
			if (progress <= num + 1E-06f)
			{
				return;
			}
			SceneBootLoader.PendingCoreLoadProgress[sceneName] = progress;
			if (SceneBootLoader._coreLoadWorkVersion < 2147483647)
			{
				SceneBootLoader._coreLoadWorkVersion++;
			}
		}

		// Token: 0x06004C68 RID: 19560 RVA: 0x001DAF76 File Offset: 0x001D9176
		private void RepositionPlayer()
		{
			SceneBootLoader.RepositionPlayerToCurrentContext(this.enableDebugLogs);
		}

		// Token: 0x06004C69 RID: 19561 RVA: 0x001DAF84 File Offset: 0x001D9184
		public static void RepositionPlayerToCurrentContext(bool enableDebugLogs = false)
		{
			SceneContext sceneContext = SceneContext.Current;
			if (sceneContext == null)
			{
				if (enableDebugLogs)
				{
					Debug.LogWarning("[SceneBootLoader] Nessun SceneContext: il player resta dov'è.");
				}
				return;
			}
			Vector3 vector;
			Quaternion quaternion;
			if (!sceneContext.TryGetPlayerSpawn(out vector, out quaternion))
			{
				if (enableDebugLogs)
				{
					Debug.Log("[SceneBootLoader] SceneContext senza spawn point: il player resta dov'è.");
				}
				return;
			}
			GameObject gameObject = PlayerRegistry.PlayerObject;
			if (gameObject == null)
			{
				gameObject = GameObject.FindGameObjectWithTag("Player");
			}
			if (gameObject == null)
			{
				Debug.LogWarning("[SceneBootLoader] Player non trovato (né PlayerRegistry né tag 'Player'). Impossibile riposizionarlo.");
				return;
			}
			global::CharacterController component = gameObject.GetComponent<global::CharacterController>();
			bool flag = component != null && component.enabled;
			if (flag)
			{
				component.enabled = false;
			}
			Rigidbody component2 = gameObject.GetComponent<Rigidbody>();
			if (component2 != null)
			{
				component2.velocity = Vector3.zero;
				component2.angularVelocity = Vector3.zero;
			}
			gameObject.transform.SetPositionAndRotation(vector, quaternion);
			if (component2 != null)
			{
				component2.position = vector;
				component2.rotation = quaternion;
			}
			Physics.SyncTransforms();
			if (flag)
			{
				component.enabled = true;
			}
			vThirdPersonCamera vThirdPersonCamera = Object.FindObjectOfType<vThirdPersonCamera>();
			if (vThirdPersonCamera != null)
			{
				vThirdPersonCamera.ResetAngleWithoutSmooth();
			}
			if (enableDebugLogs)
			{
				Debug.Log("[SceneBootLoader] Player '" + gameObject.name + "' riposizionato su " + string.Format("spawn '{0}' ({1}).", sceneContext.PlayerSpawnPoint.name, vector));
			}
		}

		// Token: 0x06004C6A RID: 19562 RVA: 0x001DB0D0 File Offset: 0x001D92D0
		private bool IsCoreLoaded()
		{
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				Scene sceneAt = SceneManager.GetSceneAt(i);
				if (sceneAt.name == this.coreSceneName && sceneAt.isLoaded)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004C6B RID: 19563 RVA: 0x001DB114 File Offset: 0x001D9314
		private void MarkReady()
		{
			Scene sceneByName = SceneManager.GetSceneByName(this.coreSceneName);
			if (!sceneByName.IsValid() || !sceneByName.isLoaded)
			{
				return;
			}
			SceneBootLoader._readyCoreSceneHandle = sceneByName.handle;
			SceneManager.sceneUnloaded -= SceneBootLoader.OnCoreSceneUnloaded;
			SceneManager.sceneUnloaded += SceneBootLoader.OnCoreSceneUnloaded;
			EnvironmentLightingRefresh.RequestForActiveScene();
			SceneBootLoader.CoreReady = true;
			Action onCoreReady = SceneBootLoader.OnCoreReady;
			if (onCoreReady == null)
			{
				return;
			}
			onCoreReady();
		}

		// Token: 0x06004C6C RID: 19564 RVA: 0x001DB188 File Offset: 0x001D9388
		private static void OnCoreSceneUnloaded(Scene scene)
		{
			if (scene.handle != SceneBootLoader._readyCoreSceneHandle)
			{
				return;
			}
			SceneBootLoader.CoreReady = false;
			SceneBootLoader._readyCoreSceneHandle = -1;
		}

		// Token: 0x04004DD4 RID: 19924
		[Tooltip("Nome della scena core persistente (senza estensione). Deve essere nelle Build Settings.")]
		[SerializeField]
		private string coreSceneName = "_Core";

		// Token: 0x04004DD5 RID: 19925
		[Tooltip("Se true, la scena di gioco corrente diventa quella attiva una volta caricato il core (lighting/skybox della scena di gioco restano i riferimenti).")]
		[SerializeField]
		private bool makeThisSceneActive = true;

		// Token: 0x04004DD6 RID: 19926
		[SerializeField]
		private bool enableDebugLogs = true;

		// Token: 0x04004DD7 RID: 19927
		[Tooltip("Seconds without initialization progress before offering recovery for missing required dependencies.")]
		[SerializeField]
		[Min(5f)]
		private float readinessSafetyTimeout = 45f;

		// Token: 0x04004DDA RID: 19930
		private static int _readyCoreSceneHandle = -1;

		// Token: 0x04004DDB RID: 19931
		private static readonly Dictionary<string, AsyncOperation> PendingCoreLoads = new Dictionary<string, AsyncOperation>();

		// Token: 0x04004DDC RID: 19932
		private static readonly Dictionary<string, float> PendingCoreLoadProgress = new Dictionary<string, float>();

		// Token: 0x04004DDD RID: 19933
		private static int _coreLoadWorkVersion;

		// Token: 0x04004DDE RID: 19934
		private bool _ownsBootLoading;
	}
}
