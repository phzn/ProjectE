using System;
using System.Collections;
using Features.Detection;
using Features.FlexiPenis;
using Features.GachaMachine;
using Features.Home;
using Features.Inventory;
using Features.Inventory.Actions;
using Features.Inventory.Placement;
using Features.Loading;
using Features.MainMenu;
using Features.MatchRecap;
using Features.NPC;
using Features.OxGram;
using Features.PhoneCompanion;
using Features.PlayerEmotions;
using Features.Police;
using Features.RunModes;
using Features.Score;
using Features.Suspicion;
using Features.TimeFreeze;
using Invector.vCharacterController;
using PoolWaterTools;
using ProjectExhibitionist.CharacterCustomization.Core.Presets;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.Interactions;
using ProjectExhibitionist.Minimap;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Features.GameOver
{
	// Token: 0x020011D1 RID: 4561
	public class GameOverManager : MonoBehaviour
	{
		// Token: 0x1700165D RID: 5725
		// (get) Token: 0x06008C15 RID: 35861 RVA: 0x00393CB5 File Offset: 0x00391EB5
		// (set) Token: 0x06008C16 RID: 35862 RVA: 0x00393CBC File Offset: 0x00391EBC
		public static GameOverManager Instance { get; private set; }

		// Token: 0x1700165E RID: 5726
		// (get) Token: 0x06008C17 RID: 35863 RVA: 0x00393CC4 File Offset: 0x00391EC4
		// (set) Token: 0x06008C18 RID: 35864 RVA: 0x00393CCC File Offset: 0x00391ECC
		public bool IsGameOver { get; private set; }

		// Token: 0x1700165F RID: 5727
		// (get) Token: 0x06008C19 RID: 35865 RVA: 0x00393CD5 File Offset: 0x00391ED5
		// (set) Token: 0x06008C1A RID: 35866 RVA: 0x00393CDD File Offset: 0x00391EDD
		public GameOverReason CurrentReason { get; private set; }

		// Token: 0x17001660 RID: 5728
		// (get) Token: 0x06008C1B RID: 35867 RVA: 0x00393CE6 File Offset: 0x00391EE6
		// (set) Token: 0x06008C1C RID: 35868 RVA: 0x00393CEE File Offset: 0x00391EEE
		public int GameOverSequenceId { get; private set; }

		// Token: 0x17001661 RID: 5729
		// (get) Token: 0x06008C1D RID: 35869 RVA: 0x00393CF7 File Offset: 0x00391EF7
		// (set) Token: 0x06008C1E RID: 35870 RVA: 0x00393CFF File Offset: 0x00391EFF
		public float StartedAtRealtime { get; private set; }

		// Token: 0x17001662 RID: 5730
		// (get) Token: 0x06008C1F RID: 35871 RVA: 0x00393D08 File Offset: 0x00391F08
		public bool HasPendingCapture
		{
			get
			{
				return this._capturePendingRecovery;
			}
		}

		// Token: 0x06008C20 RID: 35872 RVA: 0x00393D10 File Offset: 0x00391F10
		public void ReturnToHome()
		{
			if (this.IsGameOver && this.CurrentReason == GameOverReason.ReturnedHome)
			{
				ScreenFadeController instance = ScreenFadeController.Instance;
				if (instance != null)
				{
					instance.SetBlack();
				}
			}
			this.ResetRunState();
			SceneTransitionManager instance2 = SceneTransitionManager.Instance;
			if (instance2 != null)
			{
				if (this.debugLogs)
				{
					Debug.Log("[GameOverManager] Try Again -> additive swap verso '" + this.homeSceneName + "'.");
				}
				instance2.LoadGameplayScene(this.homeSceneName);
				return;
			}
			if (LoadingScreenController.Instance != null)
			{
				if (this.debugLogs)
				{
					Debug.Log("[GameOverManager] Try Again -> fallback Single load di '" + this.homeSceneName + "'.");
				}
				LoadingScreenController.Instance.LoadSceneAsync(this.homeSceneName);
				return;
			}
			SceneManager.LoadScene(this.homeSceneName);
		}

		// Token: 0x06008C21 RID: 35873 RVA: 0x00393DCD File Offset: 0x00391FCD
		private static void ApplyHomeWardrobeAfterRetry(Transform player, CustomizationPresetData preset)
		{
			HomePlayerStateRestoration.Restore(player, preset);
		}

		// Token: 0x06008C22 RID: 35874 RVA: 0x00393DD8 File Offset: 0x00391FD8
		public void ResetRunState()
		{
			IDisposable homeReturnNpcPause = this._homeReturnNpcPause;
			if (homeReturnNpcPause != null)
			{
				homeReturnNpcPause.Dispose();
			}
			this._homeReturnNpcPause = null;
			this.ClearPendingCapture();
			Transform transform = PlayerRegistry.Player;
			if (transform == null)
			{
				vThirdPersonInput vThirdPersonInput = Object.FindObjectOfType<vThirdPersonInput>(true);
				if (vThirdPersonInput != null)
				{
					transform = vThirdPersonInput.transform;
				}
			}
			DetectionPlayerNormalizer.ForceDismount(transform);
			PlayerStunExecutor instance = PlayerStunExecutor.Instance;
			if (instance != null)
			{
				instance.CancelForSceneTransition();
			}
			PlayerActivityGate.Pop("GameOver");
			if (PerpWalkController.Instance != null)
			{
				PerpWalkController.Instance.StopAndRestore();
			}
			if (this._cachedPlayerInput == null)
			{
				this._cachedPlayerInput = Object.FindObjectOfType<vThirdPersonInput>();
			}
			if (this._cachedPlayerInput != null)
			{
				this._cachedPlayerInput.SetLockAllInput(false);
				this._cachedPlayerInput.SetLockCameraInput(false);
			}
			if (SensoryManager.Instance != null)
			{
				SensoryManager.Instance.SetPaused(false);
			}
			if (EmotionManager.Instance != null)
			{
				EmotionManager.Instance.SetFrozen(false);
				EmotionManager.Instance.SetStanceLock(null);
				EmotionManager.Instance.ResetToBaseValues();
			}
			if (SuspicionManager.Instance != null)
			{
				SuspicionManager.Instance.enabled = true;
				SuspicionManager.Instance.ResetSuspicion();
			}
			if (ReporterTroupeManager.Instance != null)
			{
				ReporterTroupeManager.Instance.enabled = true;
			}
			this.StopGameOverLoop(0.5f);
			if (MatchRecapController.Instance != null)
			{
				MatchRecapController.Instance.Hide();
			}
			GameActionItemsHUD gameActionItemsHUD = Object.FindObjectOfType<GameActionItemsHUD>(true);
			if (gameActionItemsHUD != null)
			{
				gameActionItemsHUD.Hide();
			}
			NpcPhotoSessionArchive.Clear();
			this.IsGameOver = false;
			if (this.debugLogs)
			{
				Debug.Log("[GameOverManager] ResetRunState: stato di run azzerato (player, emozioni, suspicion, UI).");
			}
		}

		// Token: 0x06008C23 RID: 35875 RVA: 0x00393F7C File Offset: 0x0039217C
		private void Awake()
		{
			if (GameOverManager.Instance != null && GameOverManager.Instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			GameOverManager.Instance = this;
			this.InitializeAmbientSource();
		}

		// Token: 0x06008C24 RID: 35876 RVA: 0x00393FB0 File Offset: 0x003921B0
		private void InitializeAmbientSource()
		{
			if (this.ambientSource == null)
			{
				GameObject gameObject = new GameObject("GameOverAmbientSource");
				gameObject.transform.SetParent(base.transform);
				this.ambientSource = gameObject.AddComponent<AudioSource>();
			}
			this.ambientSource.loop = true;
			this.ambientSource.playOnAwake = false;
			this.ambientSource.spatialBlend = 0f;
			this.ambientSource.volume = 0f;
		}

		// Token: 0x06008C25 RID: 35877 RVA: 0x0039402B File Offset: 0x0039222B
		private void OnDestroy()
		{
			IDisposable homeReturnNpcPause = this._homeReturnNpcPause;
			if (homeReturnNpcPause != null)
			{
				homeReturnNpcPause.Dispose();
			}
			this._homeReturnNpcPause = null;
			if (GameOverManager.Instance == this)
			{
				GameOverManager.Instance = null;
			}
		}

		// Token: 0x06008C26 RID: 35878 RVA: 0x00394058 File Offset: 0x00392258
		private void OnDisable()
		{
			this.ClearPendingCapture();
		}

		// Token: 0x06008C27 RID: 35879 RVA: 0x00394060 File Offset: 0x00392260
		private void Update()
		{
			if (DetectionTransitionGate.IsBlocked)
			{
				this.ClearPendingCapture();
				return;
			}
			if (TimeFreezeController.IsAnyActive)
			{
				return;
			}
			if (!this._capturePendingRecovery || DetectionRecoveryGate.MustWaitForPlayer)
			{
				return;
			}
			if (ExhibitionEventDirector.Instance != null && ExhibitionEventDirector.Instance.IsEventActive)
			{
				return;
			}
			MonoBehaviour pendingRecoveryCaptor = this._pendingRecoveryCaptor;
			this.ClearPendingCapture();
			if (!this.IsGameOver && pendingRecoveryCaptor != null)
			{
				this.TriggerCapture(pendingRecoveryCaptor);
			}
		}

		// Token: 0x06008C28 RID: 35880 RVA: 0x003940D2 File Offset: 0x003922D2
		private void ClearPendingCapture()
		{
			this._capturePendingRecovery = false;
			this._pendingRecoveryCaptor = null;
		}

		// Token: 0x06008C29 RID: 35881 RVA: 0x003940E4 File Offset: 0x003922E4
		public void TriggerCapture(MonoBehaviour by)
		{
			if (DetectionTransitionGate.IsBlocked || TimeFreezeController.IsAnyActive)
			{
				return;
			}
			if (this.IsGameOver)
			{
				return;
			}
			if (RunModeService.IsLongWayHome)
			{
				if (LongWayHomeRuntimeController.IsPoliceCapturePending && !(by is PerpWalkController))
				{
					return;
				}
				if (!LongWayHomeRuntimeController.IsPoliceCapturePending && !LongWayHomeRuntimeController.TryBeginPoliceDefeat())
				{
					return;
				}
				this.ClearPendingCapture();
				this.TriggerGameOver(GameOverReason.PoliceCapture, by);
				return;
			}
			else
			{
				if (DetectionRecoveryGate.MustWaitForPlayer)
				{
					if (!this._capturePendingRecovery)
					{
						this._pendingRecoveryCaptor = by;
					}
					this._capturePendingRecovery = true;
					return;
				}
				this.ClearPendingCapture();
				this.TriggerGameOver(GameOverReason.PoliceCapture, by);
				return;
			}
		}

		// Token: 0x06008C2A RID: 35882 RVA: 0x0039416C File Offset: 0x0039236C
		public void TriggerStun(MonoBehaviour by)
		{
			if (DetectionTransitionGate.IsBlocked || TimeFreezeController.IsAnyActive)
			{
				return;
			}
			Transform transform = PlayerRegistry.Player;
			if (transform == null && PlayerStunExecutor.Instance != null)
			{
				transform = PlayerStunExecutor.Instance.transform;
			}
			if (transform == null)
			{
				if (this._cachedPlayerInput == null)
				{
					this._cachedPlayerInput = Object.FindObjectOfType<vThirdPersonInput>();
				}
				if (this._cachedPlayerInput != null)
				{
					transform = this._cachedPlayerInput.transform;
				}
			}
			if (PoolWaterRecovery.IsPlayerInWater(transform))
			{
				return;
			}
			if (!RunModeService.IsLongWayHome)
			{
				this.TriggerGameOver(GameOverReason.TaserStun, by);
				return;
			}
			if (!LongWayHomeRuntimeController.TryBeginPoliceDefeat())
			{
				return;
			}
			this.ClearPendingCapture();
			this.TriggerGameOver(GameOverReason.TaserStun, by);
		}

		// Token: 0x06008C2B RID: 35883 RVA: 0x0039421C File Offset: 0x0039241C
		public void TriggerCensorLeak()
		{
			if (!OxGramPhotoPrivacy.CanLeakInCurrentScene)
			{
				CensorLeakState.Reset();
				return;
			}
			if (this.IsGameOver)
			{
				return;
			}
			this.TriggerGameOver(GameOverReason.CensorLeak, null);
			PerpWalkController instance = PerpWalkController.Instance;
			if (instance != null)
			{
				instance.BeginPerpRun();
				return;
			}
			if (this.debugLogs)
			{
				Debug.LogWarning("[GameOverManager] TriggerCensorLeak: no PerpWalkController.Instance — PerpRun skipped.");
			}
		}

		// Token: 0x06008C2C RID: 35884 RVA: 0x00394270 File Offset: 0x00392470
		public void TriggerRankDefeat()
		{
			if (this.IsGameOver)
			{
				return;
			}
			this.TriggerGameOver(GameOverReason.RankDefeat, null);
			PerpWalkController instance = PerpWalkController.Instance;
			if (instance != null)
			{
				instance.BeginPerpRun();
				return;
			}
			if (this.debugLogs)
			{
				Debug.LogWarning("[GameOverManager] TriggerRankDefeat: no PerpWalkController.Instance — PerpRun skipped.");
			}
		}

		// Token: 0x06008C2D RID: 35885 RVA: 0x003942B6 File Offset: 0x003924B6
		public void TriggerLongWayHomeSpotted(MonoBehaviour witness)
		{
			if (!RunModeService.IsLongWayHome || this.IsGameOver)
			{
				return;
			}
			this.TriggerGameOver(GameOverReason.LongWayHomeSpotted, witness);
			PerpWalkController instance = PerpWalkController.Instance;
			if (instance == null)
			{
				return;
			}
			instance.BeginPerpRun();
		}

		// Token: 0x06008C2E RID: 35886 RVA: 0x003942E0 File Offset: 0x003924E0
		internal bool TryCompleteHomeReturn()
		{
			if (this.IsGameOver || this.HasPendingCapture || DetectionTransitionGate.IsBlocked || DetectionRecoveryGate.IsPlayerRecovering || Time.timeScale <= 0f || !InteractionCandidateRegistry.CanStartWorldInteraction || (ExhibitionEventDirector.Instance != null && (ExhibitionEventDirector.Instance.IsEventActive || ExhibitionEventDirector.Instance.HasPendingEncounter)) || !RunModeService.HomeReturnEnabled || SceneManager.GetActiveScene().name != "Game - Current")
			{
				return false;
			}
			if (!Application.CanStreamedLevelBeLoaded(this.homeSceneName))
			{
				return false;
			}
			MatchStatsService instance = MatchStatsService.Instance;
			if (instance == null || !instance.IsRunTrackingActive || instance.IsMocked)
			{
				return false;
			}
			if (RunModeService.CurrentMode == RunModeKind.Free)
			{
				SceneTransitionManager instance2 = SceneTransitionManager.Instance;
				if (instance2 != null && instance2.IsTransitioning)
				{
					return false;
				}
				instance.CompleteRun();
				this.ReturnToHome();
				return true;
			}
			else
			{
				GameOverManager.EnsureMatchRecapActive();
				if (MatchRecapController.Instance == null || !MatchRecapController.Instance.isActiveAndEnabled)
				{
					return false;
				}
				if (!instance.TryAwardHomeReturn())
				{
					return false;
				}
				this.TriggerGameOver(GameOverReason.ReturnedHome, null);
				instance.CompleteRun();
				return this.IsGameOver && this.CurrentReason == GameOverReason.ReturnedHome;
			}
		}

		// Token: 0x06008C2F RID: 35887 RVA: 0x00394410 File Offset: 0x00392610
		private void LateUpdate()
		{
			if (!this.IsGameOver || this.CurrentReason != GameOverReason.ReturnedHome || this._cachedPlayerInput == null || this._cachedPlayerInput.cc == null)
			{
				return;
			}
			this._cachedPlayerInput.cc.StopCharacter();
			this._cachedPlayerInput.cc.isRunning = false;
			this._cachedPlayerInput.cc.ResetInputAnimatorParameters();
		}

		// Token: 0x06008C30 RID: 35888 RVA: 0x00394484 File Offset: 0x00392684
		private void TriggerGameOver(GameOverReason reason, MonoBehaviour by)
		{
			if (this.IsGameOver)
			{
				return;
			}
			this.IsGameOver = true;
			this.CurrentReason = reason;
			int i = this.GameOverSequenceId;
			this.GameOverSequenceId = i + 1;
			this.StartedAtRealtime = Time.realtimeSinceStartup;
			if (reason == GameOverReason.ReturnedHome)
			{
				ScreenFadeController instance = ScreenFadeController.Instance;
				if (instance != null)
				{
					instance.FadeOut(0.65f, null);
				}
			}
			MapPinController.ClearForTransition();
			DetectionPlayerNormalizer.InterruptGameplayOverlays();
			GachaMachineController[] array = Object.FindObjectsOfType<GachaMachineController>(true);
			for (i = 0; i < array.Length; i++)
			{
				array[i].InterruptForEncounter();
			}
			Transform transform = PlayerRegistry.Player;
			if (transform == null)
			{
				vThirdPersonInput vThirdPersonInput = Object.FindObjectOfType<vThirdPersonInput>(true);
				if (vThirdPersonInput != null)
				{
					transform = vThirdPersonInput.transform;
				}
			}
			DetectionPlayerNormalizer.ForceDismount(transform);
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[GameOverManager] Game over: {0} by {1}", reason, (by != null) ? by.name : "<null>"));
			}
			SmartphoneController smartphoneController = Object.FindObjectOfType<SmartphoneController>(true);
			if (smartphoneController != null && smartphoneController.IsPhoneOut)
			{
				smartphoneController.ExitToGameplay();
			}
			DildoPlacementSystem[] array2 = Object.FindObjectsOfType<DildoPlacementSystem>(true);
			for (i = 0; i < array2.Length; i++)
			{
				array2[i].InterruptForEncounter();
			}
			FlexiWallHeldActions[] array3 = Object.FindObjectsOfType<FlexiWallHeldActions>(true);
			for (i = 0; i < array3.Length; i++)
			{
				array3[i].InterruptForEncounter();
			}
			DildoActionExecutor[] array4 = Object.FindObjectsOfType<DildoActionExecutor>(true);
			for (i = 0; i < array4.Length; i++)
			{
				array4[i].CancelAction();
			}
			PlayerInventoryManager instance2 = PlayerInventoryManager.Instance;
			if (instance2 != null)
			{
				instance2.StowHeldItemForInteraction();
			}
			PlayerActivityGate.Push("GameOver", PlayerActivity.AllPlayerActions);
			this._cachedPlayerInput = Object.FindObjectOfType<vThirdPersonInput>();
			if (this._cachedPlayerInput != null)
			{
				this._cachedPlayerInput.SetLockAllInput(true);
				this._cachedPlayerInput.cc.input = Vector2.zero;
				this._cachedPlayerInput.cc.isSprinting = false;
				if (this._cachedPlayerInput.cc.animator != null)
				{
					this._cachedPlayerInput.cc.animator.SetFloat("InputHorizontal", 0f);
					this._cachedPlayerInput.cc.animator.SetFloat("InputVertical", 0f);
					this._cachedPlayerInput.cc.animator.SetFloat("InputMagnitude", 0f);
				}
			}
			if (SensoryManager.Instance != null)
			{
				SensoryManager.Instance.SetPaused(true);
			}
			if (EmotionManager.Instance != null)
			{
				EmotionManager.Instance.SetFrozen(true);
			}
			GameOverManager.EnsureMatchRecapActive();
			if (reason == GameOverReason.ReturnedHome)
			{
				if (this._homeReturnNpcPause == null)
				{
					this._homeReturnNpcPause = NPCSimulationPause.AcquirePause();
				}
				GameObject gameObject = Resources.Load<GameObject>("LongWayHomeArrivalDoor");
				if (gameObject != null)
				{
					GameObject gameObject2 = Object.Instantiate<GameObject>(gameObject, base.transform);
					AudioSource component = gameObject2.GetComponent<AudioSource>();
					if (component != null && component.clip != null)
					{
						GameAudioRouter.Route(component, GameAudioRouter.Channel.Sfx);
						component.PlayOneShot(component.clip, 0.14f);
						Object.Destroy(gameObject2, component.clip.length + 0.1f);
					}
					else
					{
						Object.Destroy(gameObject2);
					}
				}
			}
			else
			{
				this.StartGameOverLoop();
			}
			UnityEvent<GameOverReason> onGameOver = this.OnGameOver;
			if (onGameOver != null)
			{
				onGameOver.Invoke(reason);
			}
			base.StartCoroutine(this.GameOverSequence(reason));
		}

		// Token: 0x06008C31 RID: 35889 RVA: 0x003947C8 File Offset: 0x003929C8
		private static void EnsureMatchRecapActive()
		{
			if (MatchRecapController.Instance != null && MatchRecapController.Instance.gameObject.activeInHierarchy)
			{
				return;
			}
			foreach (MatchRecapController matchRecapController in Resources.FindObjectsOfTypeAll<MatchRecapController>())
			{
				if (!(matchRecapController == null) && matchRecapController.gameObject.scene.IsValid())
				{
					GameObject gameObject = matchRecapController.gameObject;
					if (!gameObject.activeSelf)
					{
						gameObject.SetActive(true);
					}
					Transform transform = gameObject.transform.parent;
					while (transform != null)
					{
						if (!transform.gameObject.activeSelf)
						{
							transform.gameObject.SetActive(true);
						}
						transform = transform.parent;
					}
				}
			}
		}

		// Token: 0x06008C32 RID: 35890 RVA: 0x00394880 File Offset: 0x00392A80
		private void StartGameOverLoop()
		{
			if (this.ambientSource == null || this.gameOverLoop == null)
			{
				return;
			}
			if (this._ambientFadeCoroutine != null)
			{
				base.StopCoroutine(this._ambientFadeCoroutine);
			}
			this.ambientSource.clip = this.gameOverLoop;
			this.ambientSource.Play();
			this._ambientFadeCoroutine = base.StartCoroutine(this.FadeAmbient(this.gameOverLoopVolume, this.gameOverLoopFadeIn));
		}

		// Token: 0x06008C33 RID: 35891 RVA: 0x003948F8 File Offset: 0x00392AF8
		public void StopGameOverLoop(float fadeOutDuration = 0.5f)
		{
			if (this.ambientSource == null)
			{
				return;
			}
			if (this._ambientFadeCoroutine != null)
			{
				base.StopCoroutine(this._ambientFadeCoroutine);
			}
			this._ambientFadeCoroutine = base.StartCoroutine(this.FadeOutAndStopAmbient(fadeOutDuration));
		}

		// Token: 0x06008C34 RID: 35892 RVA: 0x00394930 File Offset: 0x00392B30
		private IEnumerator FadeAmbient(float targetVolume, float duration)
		{
			float startVolume = this.ambientSource.volume;
			float elapsed = 0f;
			while (elapsed < duration)
			{
				elapsed += Time.unscaledDeltaTime;
				this.ambientSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsed / duration);
				yield return null;
			}
			this.ambientSource.volume = targetVolume;
			yield break;
		}

		// Token: 0x06008C35 RID: 35893 RVA: 0x0039494D File Offset: 0x00392B4D
		private IEnumerator FadeOutAndStopAmbient(float duration)
		{
			float startVolume = this.ambientSource.volume;
			float elapsed = 0f;
			while (elapsed < duration)
			{
				elapsed += Time.unscaledDeltaTime;
				this.ambientSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / duration);
				yield return null;
			}
			this.ambientSource.volume = 0f;
			this.ambientSource.Stop();
			yield break;
		}

		// Token: 0x06008C36 RID: 35894 RVA: 0x00394963 File Offset: 0x00392B63
		private IEnumerator GameOverSequence(GameOverReason reason)
		{
			if (reason == GameOverReason.ReturnedHome)
			{
				yield break;
			}
			bool fadeDone = false;
			if (ScreenFadeController.Instance != null && this.fadeDuration > 0f)
			{
				ScreenFadeController.Instance.FadeOut(this.fadeDuration, delegate
				{
					fadeDone = true;
				});
				while (!fadeDone)
				{
					yield return null;
				}
			}
			else
			{
				yield return new WaitForSeconds(this.fadeDuration);
			}
			if (this.autoReloadScene && this.reloadDelay > 0f)
			{
				yield return new WaitForSeconds(this.reloadDelay);
				NpcPhotoSessionArchive.Clear();
				Scene activeScene = SceneManager.GetActiveScene();
				if (this.debugLogs)
				{
					Debug.Log("[GameOverManager] Reloading scene '" + activeScene.name + "'");
				}
				SceneManager.LoadScene(activeScene.buildIndex);
			}
			yield break;
		}

		// Token: 0x04008F19 RID: 36633
		private const string GameplayLockReason = "GameOver";

		// Token: 0x04008F1B RID: 36635
		[Header("Behavior")]
		[Tooltip("Seconds to wait after game-over starts before reloading the scene. Set to 0 (or negative) to never auto-reload (useful while iterating). Default flow: MatchRecapController prende il controllo dopo OnGameOver, quindi NON serve auto-reload qui — lasciare a 0.")]
		public float reloadDelay;

		// Token: 0x04008F1C RID: 36636
		[Tooltip("If true, the active scene is reloaded after reloadDelay seconds. Default false: il restart e' delegato al bottone 'Try Again' del MatchRecapController.")]
		public bool autoReloadScene;

		// Token: 0x04008F1D RID: 36637
		[Tooltip("Duration of the fade-to-black before the optional auto-reload. Default 0: niente fade per consentire al MatchRecapController di mostrare il pannello mentre Ciel viene scortata via dalla Polizia.")]
		public float fadeDuration;

		// Token: 0x04008F1E RID: 36638
		[Header("Ambient Audio")]
		[Tooltip("Looping background track that starts as soon as game-over begins. Kept low to sit under the recap UI and PerpWalk sequence.")]
		[SerializeField]
		private AudioClip gameOverLoop;

		// Token: 0x04008F1F RID: 36639
		[Tooltip("Optional pre-assigned AudioSource. If null, one is created at runtime as a child of this GO.")]
		[SerializeField]
		private AudioSource ambientSource;

		// Token: 0x04008F20 RID: 36640
		[Tooltip("Target volume for the game-over loop. Keep low (0.15–0.30) so it sits behind UI/voice.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float gameOverLoopVolume = 0.2f;

		// Token: 0x04008F21 RID: 36641
		[Tooltip("Fade-in duration when the loop starts.")]
		[SerializeField]
		private float gameOverLoopFadeIn = 1f;

		// Token: 0x04008F22 RID: 36642
		[Header("Debug")]
		public bool debugLogs;

		// Token: 0x04008F23 RID: 36643
		[Header("Events")]
		[Tooltip("Fired when game-over begins. Argument: reason (PoliceCapture / TaserStun).")]
		public UnityEvent<GameOverReason> OnGameOver = new UnityEvent<GameOverReason>();

		// Token: 0x04008F28 RID: 36648
		private vThirdPersonInput _cachedPlayerInput;

		// Token: 0x04008F29 RID: 36649
		private Coroutine _ambientFadeCoroutine;

		// Token: 0x04008F2A RID: 36650
		private bool _capturePendingRecovery;

		// Token: 0x04008F2B RID: 36651
		private MonoBehaviour _pendingRecoveryCaptor;

		// Token: 0x04008F2C RID: 36652
		private IDisposable _homeReturnNpcPause;

		// Token: 0x04008F2D RID: 36653
		[Header("Flow")]
		[Tooltip("Scena hub (casa) verso cui torna il bottone 'Try Again'. Caricata via additive-swap (SceneTransitionManager) mantenendo _Core persistente.")]
		[SerializeField]
		private string homeSceneName = "CielHome";
	}
}
