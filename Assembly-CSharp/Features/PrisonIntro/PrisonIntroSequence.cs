using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Features.Detection;
using Features.PlayerEmotions;
using Features.Prison;
using Features.Rank;
using Features.WardrobeFeedback;
using Invector.vCamera;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using ProjectExhibitionist;
using ProjectExhibitionist.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace Features.PrisonIntro
{
	// Token: 0x02000DCD RID: 3533
	[ExecuteAlways]
	public class PrisonIntroSequence : MonoBehaviour
	{
		// Token: 0x1700112C RID: 4396
		// (get) Token: 0x06006866 RID: 26726 RVA: 0x0029BE81 File Offset: 0x0029A081
		public bool HasFinished
		{
			get
			{
				return this._finished;
			}
		}

		// Token: 0x1700112D RID: 4397
		// (get) Token: 0x06006867 RID: 26727 RVA: 0x0029BE89 File Offset: 0x0029A089
		public bool IsRunning
		{
			get
			{
				return this._running && !this._finished;
			}
		}

		// Token: 0x1700112E RID: 4398
		// (get) Token: 0x06006868 RID: 26728 RVA: 0x0029BEA0 File Offset: 0x0029A0A0
		public static bool IsAnyRunning
		{
			get
			{
				foreach (PrisonIntroSequence prisonIntroSequence in PrisonIntroSequence.RunningSequences)
				{
					if (prisonIntroSequence != null && prisonIntroSequence.IsRunning)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06006869 RID: 26729 RVA: 0x0029BF04 File Offset: 0x0029A104
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetRunningSequences()
		{
			PrisonIntroSequence.RunningSequences.Clear();
		}

		// Token: 0x1700112F RID: 4399
		// (get) Token: 0x0600686A RID: 26730 RVA: 0x0029BF10 File Offset: 0x0029A110
		// (set) Token: 0x0600686B RID: 26731 RVA: 0x0029BF18 File Offset: 0x0029A118
		public bool CompletedSuccessfully { get; private set; }

		// Token: 0x0600686C RID: 26732 RVA: 0x0029BF24 File Offset: 0x0029A124
		private void Awake()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			if (this.playOnStart)
			{
				this.AcquireWardrobePresentationSuppression();
			}
			this.ResolveReferences();
			this.ConfigurePrisonEmotionSystem();
			if (this.playOnStart && this.useFade)
			{
				if (ScreenFadeController.Instance != null)
				{
					ScreenFadeController.Instance.SetBlack();
				}
				else if (this.debugLogs)
				{
					Debug.LogWarning("[PrisonIntro] Awake: ScreenFadeController.Instance NULL → nero non applicato in Awake.");
				}
			}
			if (this.playOnStart && this.hideHudDuringCutscene && HUDManager.Instance != null)
			{
				HUDManager.Instance.HideHUD();
			}
			this.CapturePlayerStartPose();
			this.CaptureMainCameraLocal();
			if (this.configurePrisonEmotions || this.playOnStart)
			{
				this.DisableFaceLayerAtStart();
			}
		}

		// Token: 0x0600686D RID: 26733 RVA: 0x0029BFD8 File Offset: 0x0029A1D8
		private void CapturePlayerStartPose()
		{
			if (this._hasPlayerStartPose)
			{
				return;
			}
			Transform transform = ((this.playerInput != null) ? this.playerInput.transform : null);
			if (transform == null)
			{
				return;
			}
			this._playerStartPos = transform.position;
			this._playerStartRot = transform.rotation;
			this._hasPlayerStartPose = true;
		}

		// Token: 0x0600686E RID: 26734 RVA: 0x0029C034 File Offset: 0x0029A234
		private void CaptureMainCameraLocal()
		{
			if (this._hasMainCamLocal)
			{
				return;
			}
			if (Camera.main == null)
			{
				return;
			}
			this._mainCamT = Camera.main.transform;
			this._mainCamLocalPos = this._mainCamT.localPosition;
			this._mainCamLocalRot = this._mainCamT.localRotation;
			this._hasMainCamLocal = true;
		}

		// Token: 0x0600686F RID: 26735 RVA: 0x0029C094 File Offset: 0x0029A294
		private void ResetPlayerToStart()
		{
			if (!this._hasPlayerStartPose || this.playerInput == null)
			{
				return;
			}
			this.playerInput.transform.SetPositionAndRotation(this._playerStartPos, this._playerStartRot);
			Rigidbody component = this.playerInput.GetComponent<Rigidbody>();
			if (component != null)
			{
				component.velocity = Vector3.zero;
				component.angularVelocity = Vector3.zero;
				component.position = this._playerStartPos;
				component.rotation = this._playerStartRot;
			}
			Physics.SyncTransforms();
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[PrisonIntro] Player reset a pos={0} rot={1}", this._playerStartPos, this._playerStartRot.eulerAngles));
			}
		}

		// Token: 0x06006870 RID: 26736 RVA: 0x0029C150 File Offset: 0x0029A350
		private void OnEnable()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			Camera.onPreCull = (Camera.CameraCallback)Delegate.Remove(Camera.onPreCull, new Camera.CameraCallback(this.HandleCameraPreCull));
			Camera.onPreCull = (Camera.CameraCallback)Delegate.Combine(Camera.onPreCull, new Camera.CameraCallback(this.HandleCameraPreCull));
			RenderPipelineManager.beginCameraRendering -= this.HandleBeginCameraRendering;
			RenderPipelineManager.beginCameraRendering += this.HandleBeginCameraRendering;
			this.ResolveReferences();
			this.ConfigurePrisonEmotionSystem();
			this.SubscribeActionEvents(true);
		}

		// Token: 0x06006871 RID: 26737 RVA: 0x0029C1DC File Offset: 0x0029A3DC
		private void OnDisable()
		{
			if (Application.isPlaying && this._running)
			{
				this.CancelSequence();
			}
			PrisonIntroSequence.RunningSequences.Remove(this);
			Camera.onPreCull = (Camera.CameraCallback)Delegate.Remove(Camera.onPreCull, new Camera.CameraCallback(this.HandleCameraPreCull));
			RenderPipelineManager.beginCameraRendering -= this.HandleBeginCameraRendering;
			this._rollStabilizerArmed = false;
			this._rollFrameValid = false;
			this.SubscribeActionEvents(false);
			this.ReleasePrisonRankGate();
			this.ReleaseWardrobePresentationSuppression();
		}

		// Token: 0x06006872 RID: 26738 RVA: 0x0029C25C File Offset: 0x0029A45C
		private void AcquireWardrobePresentationSuppression()
		{
			if (this._wardrobePresentationSuppressed)
			{
				return;
			}
			WearAdmireAnimation.SetPresentationSuppressed(this, true);
			this._wardrobePresentationSuppressed = true;
		}

		// Token: 0x06006873 RID: 26739 RVA: 0x0029C275 File Offset: 0x0029A475
		private void ReleaseWardrobePresentationSuppression()
		{
			if (!this._wardrobePresentationSuppressed)
			{
				return;
			}
			WearAdmireAnimation.SetPresentationSuppressed(this, false);
			this._wardrobePresentationSuppressed = false;
		}

		// Token: 0x06006874 RID: 26740 RVA: 0x0029C28E File Offset: 0x0029A48E
		private void OnValidate()
		{
			if (this.cinematicVCam != null)
			{
				this._dolly = this.cinematicVCam.GetCinemachineComponent<CinemachineTrackedDolly>();
			}
		}

		// Token: 0x06006875 RID: 26741 RVA: 0x0029C2B0 File Offset: 0x0029A4B0
		private void ResolveReferences()
		{
			if (this.playerInput == null)
			{
				this.playerInput = Object.FindObjectOfType<vThirdPersonInput>();
			}
			if (this.actionAnimator == null && this.playerInput != null)
			{
				this.actionAnimator = this.playerInput.GetComponent<PlayerActionAnimator>();
			}
			if (this._genericAction == null && this.actionAnimator != null)
			{
				this._genericAction = this.actionAnimator.GenericAction;
			}
			if (this._genericAction == null && this.playerInput != null)
			{
				this._genericAction = this.playerInput.GetComponent<vGenericAction>();
			}
			if (this.cinematicVCam != null)
			{
				this._dolly = this.cinematicVCam.GetCinemachineComponent<CinemachineTrackedDolly>();
			}
			if (this.brain == null && Camera.main != null)
			{
				this.brain = Camera.main.GetComponent<CinemachineBrain>();
			}
			if (this._tpCamera == null)
			{
				this._tpCamera = Object.FindObjectOfType<vThirdPersonCamera>();
			}
			if (this._coverPose == null && this.playerInput != null)
			{
				this._coverPose = this.playerInput.GetComponentInChildren<CoverPoseController>(true);
			}
		}

		// Token: 0x06006876 RID: 26742 RVA: 0x0029C3F0 File Offset: 0x0029A5F0
		private void ConfigurePrisonEmotionSystem()
		{
			if (!this.configurePrisonEmotions)
			{
				return;
			}
			this._prisonEmotionManager = EmotionManager.Instance;
			if (this._prisonEmotionManager == null)
			{
				GameObject gameObject = new GameObject("[PrisonEmotionManager]");
				gameObject.transform.SetParent(base.transform, false);
				this._prisonEmotionManager = gameObject.AddComponent<EmotionManager>();
			}
			RankDefinitionSO currentRank = RankProgressionService.CurrentRank;
			this._rankDefaultStance = EmotionStance.Embarassed;
			this._rankAllowsStanceSwitch = currentRank == null || currentRank.allowStanceSwitch;
			this._prisonEmotionManager.EnableDecay = false;
			this._prisonEmotionManager.EnableStanceSwitchInput = this._rankAllowsStanceSwitch;
			this._prisonEmotionManager.SetFrozen(false);
			if (this._prisonEmotionManager.ClothingController == null)
			{
				this._prisonEmotionManager.ClothingController = Object.FindObjectOfType<ClothingToggleController>();
			}
			this._prisonEmotionManager.BaseBalance = -100f;
			this.ApplyPrisonArrivalMood();
			this.ReleasePrisonRankGate();
			if (!this._rankAllowsStanceSwitch)
			{
				PlayerActivityGate.Push("Prison.RankStance", PlayerActivity.StanceSwitch);
				this._rankGatePushed = true;
			}
		}

		// Token: 0x06006877 RID: 26743 RVA: 0x0029C4F0 File Offset: 0x0029A6F0
		private void ApplyPrisonArrivalMood()
		{
			if (this._prisonEmotionManager == null)
			{
				return;
			}
			this._prisonEmotionManager.EnableStanceSwitchInput = this._rankAllowsStanceSwitch && !this._running;
			this._prisonEmotionManager.SetStanceLock(null);
			this._prisonEmotionManager.SetFrozen(false);
			this._prisonEmotionManager.SetBalance(-100f, "PrisonArrival");
			this._prisonEmotionManager.SetStanceLock(this._rankAllowsStanceSwitch ? null : new EmotionStance?(this._rankDefaultStance));
		}

		// Token: 0x06006878 RID: 26744 RVA: 0x0029C589 File Offset: 0x0029A789
		private void ReleasePrisonRankGate()
		{
			if (!this._rankGatePushed)
			{
				return;
			}
			PlayerActivityGate.Pop("Prison.RankStance");
			this._rankGatePushed = false;
		}

		// Token: 0x06006879 RID: 26745 RVA: 0x0029C5A8 File Offset: 0x0029A7A8
		private void SubscribeActionEvents(bool subscribe)
		{
			if (this._genericAction == null)
			{
				return;
			}
			if (subscribe && !this._subscribed)
			{
				this._genericAction.OnEndAction.AddListener(new UnityAction<vTriggerGenericAction>(this.HandleActionEnded));
				this._subscribed = true;
				return;
			}
			if (!subscribe && this._subscribed)
			{
				this._genericAction.OnEndAction.RemoveListener(new UnityAction<vTriggerGenericAction>(this.HandleActionEnded));
				this._subscribed = false;
			}
		}

		// Token: 0x0600687A RID: 26746 RVA: 0x0029C621 File Offset: 0x0029A821
		private void Start()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			if (this.playOnStart)
			{
				this.PlaySequence();
			}
		}

		// Token: 0x0600687B RID: 26747 RVA: 0x0029C63C File Offset: 0x0029A83C
		private void Update()
		{
			this.HandlePathPreview();
			if (!Application.isPlaying)
			{
				return;
			}
			if (this.configurePrisonEmotions && !this._running && !this._rankAllowsStanceSwitch && this.playerInput != null && !this.playerInput.lockInput && PlayerActivityGate.CanStartWorldInteraction && GameplayInput.GetKeyDown(KeyCode.Tab))
			{
				BlockedActionFeedback.Show("Mood switching is unavailable in prison at this rank");
			}
			if (this.triggerKey != KeyCode.None && GameplayInput.GetKeyDown(this.triggerKey) && !this.previewPath && PlayerActivityGate.CanStartWorldInteraction)
			{
				if (this.debugLogs)
				{
					Debug.Log(string.Format("[PrisonIntro] Trigger key '{0}' → restart sequenza.", this.triggerKey));
				}
				this.PlaySequence();
			}
			if (this._running && !this._finished)
			{
				this.KeepCinematicLayersOff();
				if (this._cameraAcquired)
				{
					if (this.brain != null && !this.brain.enabled)
					{
						this.brain.enabled = true;
					}
					if (this.cinematicVCam != null && this.cinematicVCam.Priority < this.cinematicPriority)
					{
						this.cinematicVCam.Priority = this.cinematicPriority;
					}
				}
			}
		}

		// Token: 0x0600687C RID: 26748 RVA: 0x0029C768 File Offset: 0x0029A968
		public void ConfigureForGameplay(vThirdPersonInput input, Transform lookAt)
		{
			this.configurePrisonEmotions = false;
			this.playOnStart = false;
			this.triggerKey = KeyCode.None;
			this.forceEmbarrassedOnFinish = false;
			this.playerInput = input;
			this.actionAnimator = ((input != null) ? input.GetComponent<PlayerActionAnimator>() : null);
			this._genericAction = ((this.actionAnimator != null) ? this.actionAnimator.GenericAction : null);
			if (this.cinematicVCam != null)
			{
				this.cinematicVCam.LookAt = lookAt;
			}
			this._hasPlayerStartPose = false;
			this._hasMainCamLocal = false;
		}

		// Token: 0x0600687D RID: 26749 RVA: 0x0029C7FC File Offset: 0x0029A9FC
		public void CancelSequence()
		{
			this._cancelled = true;
			this.CompletedSuccessfully = false;
			base.StopAllCoroutines();
			this._dollyCo = null;
			this._watchdogCo = null;
			if (this._actionTriggered && this.actionAnimator != null && this._genericAction != null && this.IsGetUpTrigger(this._genericAction.triggerAction))
			{
				this.actionAnimator.CancelCurrentAction();
			}
			if (this._running && !this._finished)
			{
				this.OnGetUpFinished();
			}
		}

		// Token: 0x0600687E RID: 26750 RVA: 0x0029C884 File Offset: 0x0029AA84
		[ContextMenu("Play Sequence")]
		public void PlaySequence()
		{
			if (!Application.isPlaying)
			{
				Debug.LogWarning("[PrisonIntro] PlaySequence funziona solo in Play mode.");
				return;
			}
			if (this._running)
			{
				this.CancelSequence();
			}
			this.AcquireWardrobePresentationSuppression();
			this.ResolveReferences();
			this.CapturePlayerStartPose();
			this.SubscribeActionEvents(true);
			base.StopAllCoroutines();
			this._dollyCo = null;
			this._watchdogCo = null;
			this._running = false;
			this._finished = false;
			this.CompletedSuccessfully = false;
			this._actionStartedObserved = false;
			this._actionTriggered = false;
			this._cancelled = false;
			this._rollStabilizerArmed = false;
			this._rollFrameValid = false;
			this._cameraAcquired = false;
			this.previewPath = false;
			if (this.useFade && ScreenFadeController.Instance != null)
			{
				ScreenFadeController.Instance.SetBlack();
			}
			this.ResetPlayerToStart();
			base.StartCoroutine(this.RunIntro());
		}

		// Token: 0x0600687F RID: 26751 RVA: 0x0029C956 File Offset: 0x0029AB56
		private IEnumerator RunIntro()
		{
			this._running = true;
			PrisonIntroSequence.RunningSequences.Add(this);
			this._savedBrainEnabled = this.brain != null && this.brain.enabled;
			if (this._prisonEmotionManager != null)
			{
				this._prisonEmotionManager.EnableStanceSwitchInput = false;
			}
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[PrisonIntro] RunIntro start. timeScale={0} actionAnimator={1} trigger={2} genericAction={3} subscribed={4}", new object[]
				{
					Time.timeScale,
					this.actionAnimator != null,
					this.getUpTrigger != null,
					this._genericAction != null,
					this._subscribed
				}));
			}
			if (this.playerInput != null)
			{
				this._savedInputLock = this.playerInput.lockInput;
				this._savedCameraInputLock = this.playerInput.lockCameraInput;
				this._savedForceCameraLock = vThirdPersonInput.ForceCameraLock;
				this.playerInput.SetLockAllInput(true);
				this.playerInput.SetLockCameraInput(true);
				if (!this.configurePrisonEmotions)
				{
					vThirdPersonInput.ForceCameraLock = true;
				}
			}
			if (this._coverPose != null)
			{
				this._coverPoseWasEnabled = this._coverPose.enabled;
				this._coverPose.enabled = false;
			}
			if (this.hideHudDuringCutscene && HUDManager.Instance != null)
			{
				HUDManager.Instance.HideHUD();
			}
			this.AcquireCinematicFace();
			this.DisableFaceLayerAtStart();
			if (this.useFade && ScreenFadeController.Instance != null)
			{
				ScreenFadeController.Instance.SetBlack();
			}
			if (this.startDelay > 0f)
			{
				yield return new WaitForSecondsRealtime(this.startDelay);
			}
			PrisonNakedOverride prisonArrival = ((this.configurePrisonEmotions && this.playerInput != null) ? this.playerInput.GetComponent<PrisonNakedOverride>() : null);
			float arrivalDeadline = Time.unscaledTime + 10f;
			while (prisonArrival != null && prisonArrival.enabled && !prisonArrival.IsReady && Time.unscaledTime < arrivalDeadline)
			{
				yield return null;
			}
			if (prisonArrival != null && !prisonArrival.IsReady)
			{
				this.AbortStartup("[PrisonIntro] Prison appearance preparation did not complete; cancelling intro.");
				yield break;
			}
			if (this._coverPose != null)
			{
				this._coverPose.enabled = false;
			}
			float readyDeadline = Time.unscaledTime + 3f;
			while (Time.unscaledTime < readyDeadline)
			{
				bool flag = this.playerInput != null && this.playerInput.cc != null && this.playerInput.cc.animator != null;
				bool flag2 = this._genericAction != null;
				bool flag3 = false;
				if (flag2 && flag)
				{
					try
					{
						flag3 = this._genericAction.actionConditions;
					}
					catch
					{
						flag3 = false;
					}
				}
				if (flag && flag2 && flag3)
				{
					break;
				}
				yield return null;
			}
			if (this.actionAnimator == null || this.getUpTrigger == null)
			{
				this.AbortStartup("[PrisonIntro] actionAnimator o getUpTrigger non assegnati: impossibile avviare il get-up.");
				yield break;
			}
			this._actionTriggered = true;
			if (this.enableRootMotion)
			{
				this.actionAnimator.EnableRootMotionForAction();
			}
			this.actionAnimator.TriggerAction(new PlayerActionEntry
			{
				ActionId = this.actionId,
				DisplayName = this.actionId,
				Duration = this.actionDuration,
				TriggerPrefab = this.getUpTrigger
			});
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[PrisonIntro] TriggerAction(get-up). doingAction={0} rootMotion={1}", this._genericAction != null && this._genericAction.doingAction, this.playerInput != null && this.playerInput.cc != null && this.playerInput.cc.useRootMotion));
			}
			yield return this.WaitForGetUpStarted();
			if (!this._actionStartedObserved)
			{
				this.AbortStartup("[PrisonIntro] Get-up did not enter its animation state; cancelling intro.");
				yield break;
			}
			yield return this.PrewarmCinematicCamera();
			if (this.fadeStartDelay > 0f)
			{
				yield return new WaitForSecondsRealtime(this.fadeStartDelay);
			}
			if (this.useFade && ScreenFadeController.Instance != null)
			{
				ScreenFadeController.Instance.FadeIn(this.fadeInDuration, null);
			}
			if (this._dollyCo != null)
			{
				base.StopCoroutine(this._dollyCo);
			}
			this._dollyCo = base.StartCoroutine(this.DollyTravel());
			if (this._watchdogCo != null)
			{
				base.StopCoroutine(this._watchdogCo);
			}
			this._watchdogCo = base.StartCoroutine(this.GetUpEndWatchdog());
			yield break;
		}

		// Token: 0x06006880 RID: 26752 RVA: 0x0029C968 File Offset: 0x0029AB68
		private void AbortStartup(string warning)
		{
			Debug.LogWarning(warning, this);
			this.CancelSequence();
			if (this.configurePrisonEmotions)
			{
				this.ApplyPrisonArrivalMood();
				if (this.useFade && ScreenFadeController.Instance != null)
				{
					ScreenFadeController.Instance.FadeIn(this.fadeInDuration, null);
				}
			}
		}

		// Token: 0x06006881 RID: 26753 RVA: 0x0029C9B8 File Offset: 0x0029ABB8
		private void HandleActionEnded(vTriggerGenericAction trigger)
		{
			if (!this._running)
			{
				return;
			}
			if (!this.IsGetUpTrigger(trigger))
			{
				return;
			}
			if (trigger != null && !trigger.endActionManualy && this.actionAnimator != null)
			{
				this.actionAnimator.FinishCurrentAction();
			}
			this.OnGetUpFinished();
		}

		// Token: 0x06006882 RID: 26754 RVA: 0x0029CA08 File Offset: 0x0029AC08
		private IEnumerator WaitForGetUpStarted()
		{
			Animator animator = ((this.playerInput != null && this.playerInput.cc != null) ? this.playerInput.cc.animator : null);
			if (animator == null)
			{
				yield break;
			}
			int hash = Animator.StringToHash("PrisonGetUp");
			float timeout = Time.unscaledTime + 3f;
			while (Time.unscaledTime < timeout)
			{
				AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
				if ((currentAnimatorStateInfo.shortNameHash == hash || currentAnimatorStateInfo.IsName("PrisonGetUp")) && currentAnimatorStateInfo.normalizedTime > 0f)
				{
					this._actionStartedObserved = true;
					if (this.debugLogs)
					{
						Debug.Log(string.Format("[PrisonIntro] PrisonGetUp attivo (normalizedTime={0:F3}) → sync FadeIn+Dolly.", currentAnimatorStateInfo.normalizedTime));
					}
					yield break;
				}
				yield return null;
			}
			if (this.debugLogs)
			{
				Debug.LogWarning("[PrisonIntro] WaitForGetUpStarted: timeout, lo state PrisonGetUp non è diventato attivo.");
			}
			yield break;
		}

		// Token: 0x06006883 RID: 26755 RVA: 0x0029CA17 File Offset: 0x0029AC17
		private IEnumerator PrewarmCinematicCamera()
		{
			this._cameraAcquired = true;
			if (this.cinematicVCam != null)
			{
				this.cinematicVCam.gameObject.SetActive(true);
				this.cinematicVCam.Priority = this.cinematicPriority;
				this.cinematicVCam.PreviousStateIsValid = false;
			}
			if (this._dolly != null)
			{
				this._dolly.m_PathPosition = this.dollyCurve.Evaluate(0f);
			}
			if (this.brain == null)
			{
				yield break;
			}
			CinemachineBlendDefinition savedBlend = this.brain.m_DefaultBlend;
			this._savedPrewarmBlend = savedBlend;
			this._prewarmBlendOverridden = true;
			this.brain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.Cut, 0f);
			this.brain.enabled = true;
			this.brain.ManualUpdate();
			yield return null;
			if (this.cinematicVCam != null)
			{
				this.cinematicVCam.PreviousStateIsValid = false;
			}
			this.brain.ManualUpdate();
			this.brain.m_DefaultBlend = savedBlend;
			this._prewarmBlendOverridden = false;
			this.SeedCinematicRollFrame();
			yield break;
		}

		// Token: 0x06006884 RID: 26756 RVA: 0x0029CA28 File Offset: 0x0029AC28
		private void SeedCinematicRollFrame()
		{
			this._rollFrameValid = false;
			this._rollStabilizerArmed = false;
			if (!this.stabilizeCinematicRoll || this._mainCamT == null)
			{
				return;
			}
			this._cinematicInitialUp = this._mainCamT.up.normalized;
			this._transportedCameraUp = this._cinematicInitialUp;
			this._transportedCameraRight = this._mainCamT.right.normalized;
			this._rollFrameValid = true;
			this._rollStabilizerArmed = true;
		}

		// Token: 0x06006885 RID: 26757 RVA: 0x0029CAA6 File Offset: 0x0029ACA6
		private void HandleCameraPreCull(Camera renderingCamera)
		{
			this.StabilizeRenderedCamera(renderingCamera);
		}

		// Token: 0x06006886 RID: 26758 RVA: 0x0029CAAF File Offset: 0x0029ACAF
		private void HandleBeginCameraRendering(ScriptableRenderContext context, Camera renderingCamera)
		{
			this.StabilizeRenderedCamera(renderingCamera);
		}

		// Token: 0x06006887 RID: 26759 RVA: 0x0029CAB8 File Offset: 0x0029ACB8
		private void StabilizeRenderedCamera(Camera renderingCamera)
		{
			if (!this._running || !this._rollStabilizerArmed || !this._rollFrameValid || renderingCamera == null || this._mainCamT == null || renderingCamera.transform != this._mainCamT)
			{
				return;
			}
			Vector3 normalized = this._mainCamT.forward.normalized;
			float num = ((this._dolly != null) ? this._dolly.m_PathPosition : 1f);
			float num2 = Mathf.InverseLerp(Mathf.Min(this.worldUpBlendStart, this.worldUpBlendEnd), Mathf.Max(this.worldUpBlendStart, this.worldUpBlendEnd), num);
			num2 = Mathf.SmoothStep(0f, 1f, num2);
			Vector3 vector = Vector3.ProjectOnPlane(Vector3.Slerp(this._cinematicInitialUp, Vector3.up, num2).normalized, normalized);
			if (vector.sqrMagnitude < 1E-06f)
			{
				Vector3 vector2 = Vector3.ProjectOnPlane(this._transportedCameraRight, normalized);
				if (vector2.sqrMagnitude >= 1E-06f)
				{
					vector = Vector3.Cross(normalized, vector2.normalized);
				}
				else
				{
					vector = Vector3.ProjectOnPlane(this._mainCamT.up, normalized);
				}
			}
			if (vector.sqrMagnitude < 1E-06f)
			{
				return;
			}
			Quaternion quaternion = Quaternion.LookRotation(normalized, vector.normalized);
			this._mainCamT.rotation = quaternion;
			this._transportedCameraUp = quaternion * Vector3.up;
			this._transportedCameraRight = quaternion * Vector3.right;
		}

		// Token: 0x06006888 RID: 26760 RVA: 0x0029CC34 File Offset: 0x0029AE34
		private IEnumerator GetUpEndWatchdog()
		{
			float timeout = Time.time + Mathf.Max(this.actionDuration, 1f) + 2f;
			Animator animator = ((this.playerInput != null && this.playerInput.cc != null) ? this.playerInput.cc.animator : null);
			int hash = Animator.StringToHash("PrisonGetUp");
			while (Time.time < timeout && animator != null)
			{
				AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
				if ((currentAnimatorStateInfo.shortNameHash == hash || currentAnimatorStateInfo.IsName("PrisonGetUp")) && currentAnimatorStateInfo.normalizedTime >= 0.95f)
				{
					break;
				}
				yield return null;
			}
			if (!this._finished)
			{
				if (!this.configurePrisonEmotions && Time.time >= timeout)
				{
					this._cancelled = true;
				}
				if (this.debugLogs)
				{
					Debug.Log("[PrisonIntro] Watchdog → fine animazione, FinishCurrentAction.");
				}
				if (this.actionAnimator != null)
				{
					this.actionAnimator.FinishCurrentAction();
				}
				if (!this._finished)
				{
					this.OnGetUpFinished();
				}
			}
			this._watchdogCo = null;
			yield break;
		}

		// Token: 0x06006889 RID: 26761 RVA: 0x0029CC44 File Offset: 0x0029AE44
		private bool IsGetUpTrigger(vTriggerGenericAction trigger)
		{
			return trigger == null || (this.getUpTrigger != null && trigger == this.getUpTrigger) || (!string.IsNullOrEmpty(trigger.playAnimation) && trigger.playAnimation == ((this.getUpTrigger != null) ? this.getUpTrigger.playAnimation : "PrisonGetUp"));
		}

		// Token: 0x0600688A RID: 26762 RVA: 0x0029CCB4 File Offset: 0x0029AEB4
		private IEnumerator DollyTravel()
		{
			if (this._dolly == null || this.dollyTravelDuration <= 0f)
			{
				yield break;
			}
			if (this.debugLogs)
			{
				ICinemachineCamera cinemachineCamera = ((this.brain != null) ? this.brain.ActiveVirtualCamera : null);
				Debug.Log(string.Format("[PrisonIntro] Dolly start. brain.enabled={0} activeVCam={1} vcamPrio={2}", this.brain != null && this.brain.enabled, (cinemachineCamera != null) ? cinemachineCamera.Name : "<null>", (this.cinematicVCam != null) ? this.cinematicVCam.Priority : (-1)));
			}
			float elapsed = 0f;
			while (elapsed < this.dollyTravelDuration)
			{
				elapsed += Time.unscaledDeltaTime;
				float num = Mathf.Clamp01(elapsed / this.dollyTravelDuration);
				this._dolly.m_PathPosition = this.dollyCurve.Evaluate(num);
				yield return null;
			}
			this._dolly.m_PathPosition = this.dollyCurve.Evaluate(1f);
			if (this.debugLogs)
			{
				Debug.Log("[PrisonIntro] Dolly travel completato, camera fissa fino a fine azione.");
			}
			yield break;
		}

		// Token: 0x0600688B RID: 26763 RVA: 0x0029CCC4 File Offset: 0x0029AEC4
		private void OnGetUpFinished()
		{
			if (this._finished)
			{
				return;
			}
			PrisonIntroSequence.RunningSequences.Remove(this);
			this._finished = true;
			this.CompletedSuccessfully = this._actionStartedObserved && !this._cancelled;
			this._running = false;
			this._cameraAcquired = false;
			this._rollStabilizerArmed = false;
			this._rollFrameValid = false;
			this.ReleaseWardrobePresentationSuppression();
			if (this.debugLogs)
			{
				Debug.Log("[PrisonIntro] OnGetUpFinished → handoff camera.");
			}
			if (this._actionTriggered && this.actionAnimator != null)
			{
				this.actionAnimator.RestoreRootMotionForAction();
				this.actionAnimator.RestoreOverrideLayer();
			}
			this._actionTriggered = false;
			this.ReleaseCinematicFace();
			if (!this.configurePrisonEmotions)
			{
				Animator cielAnimator = this.GetCielAnimator();
				if (cielAnimator != null && this._faceLayerIndex >= 0)
				{
					cielAnimator.SetLayerWeight(this._faceLayerIndex, this._savedFaceLayerWeight);
				}
			}
			if (this.cinematicVCam != null)
			{
				this.cinematicVCam.Priority = this.releasedPriority;
			}
			if (this.disableBrainOnFinish && this.brain != null)
			{
				this.brain.enabled = false;
			}
			if (!this.configurePrisonEmotions && this.brain != null)
			{
				this.brain.enabled = this._savedBrainEnabled;
			}
			if (this._prewarmBlendOverridden && this.brain != null)
			{
				this.brain.m_DefaultBlend = this._savedPrewarmBlend;
			}
			this._prewarmBlendOverridden = false;
			if (this._hasMainCamLocal && this._mainCamT != null)
			{
				this._mainCamT.localPosition = this._mainCamLocalPos;
				this._mainCamT.localRotation = this._mainCamLocalRot;
			}
			if (this._tpCamera == null)
			{
				this._tpCamera = Object.FindObjectOfType<vThirdPersonCamera>();
			}
			if (this._tpCamera != null)
			{
				this._tpCamera.RemoveLockTarget();
				this._tpCamera.ResetAngle();
			}
			if (this.playerInput != null)
			{
				this.playerInput.SetLockAllInput(this._savedInputLock);
				this.playerInput.SetLockCameraInput(this._savedCameraInputLock);
			}
			vThirdPersonInput.ForceCameraLock = this._savedForceCameraLock;
			if (this._coverPose != null)
			{
				this._coverPose.enabled = this._coverPoseWasEnabled;
			}
			if (!this._cancelled)
			{
				this.ForceEmbarrassedSmooth();
			}
			if (this.hideHudDuringCutscene && HUDManager.Instance != null)
			{
				HUDManager.Instance.ShowHUD();
			}
		}

		// Token: 0x0600688C RID: 26764 RVA: 0x0029CF3C File Offset: 0x0029B13C
		private Animator GetCielAnimator()
		{
			if (this.playerInput != null && this.playerInput.cc != null && this.playerInput.cc.animator != null)
			{
				return this.playerInput.cc.animator;
			}
			if (this.actionAnimator != null)
			{
				Animator component = this.actionAnimator.GetComponent<Animator>();
				if (component != null)
				{
					return component;
				}
			}
			if (!(this.playerInput != null))
			{
				return null;
			}
			return this.playerInput.GetComponentInChildren<Animator>();
		}

		// Token: 0x0600688D RID: 26765 RVA: 0x0029CFD4 File Offset: 0x0029B1D4
		private void ResolveEmotionLayerIndices(Animator animator)
		{
			if (animator == null)
			{
				return;
			}
			if (this._faceLayerIndex < 0)
			{
				this._faceLayerIndex = animator.GetLayerIndex(this.faceLayerName);
			}
			if (this._embLayerIndex < 0)
			{
				this._embLayerIndex = animator.GetLayerIndex(this.embarrassedLayerName);
			}
		}

		// Token: 0x0600688E RID: 26766 RVA: 0x0029D024 File Offset: 0x0029B224
		private void AcquireCinematicFace()
		{
			this._cinematicAnimator = this.GetCielAnimator();
			if (this._cinematicAnimator == null)
			{
				return;
			}
			this.ResolveEmotionLayerIndices(this._cinematicAnimator);
			if (this._savedCinematicLayerWeights == null)
			{
				this._savedCinematicLayerWeights = new float[this._cinematicAnimator.layerCount];
				for (int i = 0; i < this._savedCinematicLayerWeights.Length; i++)
				{
					this._savedCinematicLayerWeights[i] = this._cinematicAnimator.GetLayerWeight(i);
				}
				this._savedFaceLayerWeight = ((this._faceLayerIndex >= 0) ? this._cinematicAnimator.GetLayerWeight(this._faceLayerIndex) : 1f);
			}
			this._emotionController = ((this.playerInput != null) ? this.playerInput.GetComponent<PlayerEmotionController>() : null);
			if (this._emotionController != null && !this._emotionController.IsFacialExpressionOverriddenForAction)
			{
				this._emotionController.DisableFacialExpressionForAction();
				this._gameplayFaceSuppressed = true;
			}
			this.KeepCinematicLayersOff();
		}

		// Token: 0x0600688F RID: 26767 RVA: 0x0029D11C File Offset: 0x0029B31C
		private void KeepCinematicLayersOff()
		{
			if (this._cinematicAnimator == null || this._savedCinematicLayerWeights == null)
			{
				return;
			}
			for (int i = 1; i < this._cinematicAnimator.layerCount; i++)
			{
				this._cinematicAnimator.SetLayerWeight(i, 0f);
			}
		}

		// Token: 0x06006890 RID: 26768 RVA: 0x0029D167 File Offset: 0x0029B367
		private void LateUpdate()
		{
			if (this.IsRunning)
			{
				this.KeepCinematicLayersOff();
			}
		}

		// Token: 0x06006891 RID: 26769 RVA: 0x0029D178 File Offset: 0x0029B378
		private void ReleaseCinematicFace()
		{
			if (this._cinematicAnimator != null && this._savedCinematicLayerWeights != null)
			{
				for (int i = 1; i < Mathf.Min(this._cinematicAnimator.layerCount, this._savedCinematicLayerWeights.Length); i++)
				{
					this._cinematicAnimator.SetLayerWeight(i, this._savedCinematicLayerWeights[i]);
				}
			}
			this._savedCinematicLayerWeights = null;
			this._cinematicAnimator = null;
			if (this._gameplayFaceSuppressed && this._emotionController != null)
			{
				this._emotionController.RestoreFacialExpression();
			}
			this._gameplayFaceSuppressed = false;
		}

		// Token: 0x06006892 RID: 26770 RVA: 0x0029D208 File Offset: 0x0029B408
		private void DisableFaceLayerAtStart()
		{
			if (!this.disableFaceLayerAtStart)
			{
				return;
			}
			Animator cielAnimator = this.GetCielAnimator();
			if (cielAnimator == null)
			{
				return;
			}
			this.ResolveEmotionLayerIndices(cielAnimator);
			if (this._faceLayerIndex < 0)
			{
				if (this.debugLogs)
				{
					Debug.LogWarning("[PrisonIntro] Layer '" + this.faceLayerName + "' non trovato sull'Animator.");
				}
				return;
			}
			cielAnimator.SetLayerWeight(this._faceLayerIndex, 0f);
			this._faceDisabledAtStart = true;
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[PrisonIntro] Layer '{0}' (idx {1}) disattivato all'avvio.", this.faceLayerName, this._faceLayerIndex));
			}
		}

		// Token: 0x06006893 RID: 26771 RVA: 0x0029D2A4 File Offset: 0x0029B4A4
		private void ForceEmbarrassedSmooth()
		{
			if (!this.forceEmbarrassedOnFinish)
			{
				return;
			}
			Animator cielAnimator = this.GetCielAnimator();
			if (cielAnimator == null)
			{
				return;
			}
			this.ResolveEmotionLayerIndices(cielAnimator);
			if (this._prisonEmotionManager != null)
			{
				if (this._faceLayerIndex >= 0)
				{
					cielAnimator.SetLayerWeight(this._faceLayerIndex, 1f);
				}
				this.ApplyPrisonArrivalMood();
				return;
			}
			if (this._emotionController == null)
			{
				this._emotionController = Object.FindObjectOfType<PlayerEmotionController>();
			}
			if (this._emotionController != null)
			{
				this._emotionController.DisableFacialExpressionForAction();
				this._emotionController.DisableEmotionLayersForAction();
			}
			if (!string.IsNullOrEmpty(this.faceExpressionParam))
			{
				cielAnimator.SetInteger(this.faceExpressionParam, this.embSupremeExpressionIndex);
			}
			if (!string.IsNullOrEmpty(this.emotionBlendParam))
			{
				cielAnimator.SetFloat(this.emotionBlendParam, -1f);
			}
			if (this._embBlendCo != null)
			{
				base.StopCoroutine(this._embBlendCo);
			}
			this._embBlendCo = base.StartCoroutine(this.EmbarrassedBlend(cielAnimator, this.embBlendDuration));
		}

		// Token: 0x06006894 RID: 26772 RVA: 0x0029D3A8 File Offset: 0x0029B5A8
		private IEnumerator EmbarrassedBlend(Animator animator, float duration)
		{
			float faceStart = ((this._faceLayerIndex >= 0) ? animator.GetLayerWeight(this._faceLayerIndex) : 0f);
			float embStart = ((this._embLayerIndex >= 0) ? animator.GetLayerWeight(this._embLayerIndex) : 0f);
			if (duration <= 0f)
			{
				if (this._faceLayerIndex >= 0)
				{
					animator.SetLayerWeight(this._faceLayerIndex, 1f);
				}
				if (this._embLayerIndex >= 0)
				{
					animator.SetLayerWeight(this._embLayerIndex, 1f);
				}
			}
			else
			{
				float t = 0f;
				while (t < duration)
				{
					t += Time.deltaTime;
					float num = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(t / duration));
					if (this._faceLayerIndex >= 0)
					{
						animator.SetLayerWeight(this._faceLayerIndex, Mathf.Lerp(faceStart, 1f, num));
					}
					if (this._embLayerIndex >= 0)
					{
						animator.SetLayerWeight(this._embLayerIndex, Mathf.Lerp(embStart, 1f, num));
					}
					yield return null;
				}
				if (this._faceLayerIndex >= 0)
				{
					animator.SetLayerWeight(this._faceLayerIndex, 1f);
				}
				if (this._embLayerIndex >= 0)
				{
					animator.SetLayerWeight(this._embLayerIndex, 1f);
				}
			}
			this._embBlendCo = null;
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[PrisonIntro] Embarassed (idx {0}) + Face (idx {1}) ", this._embLayerIndex, this._faceLayerIndex) + string.Format("forzati a peso 1, faccia EmbSupreme (idx {0}).", this.embSupremeExpressionIndex));
			}
			yield break;
		}

		// Token: 0x06006895 RID: 26773 RVA: 0x0029D3C8 File Offset: 0x0029B5C8
		private void HandlePathPreview()
		{
			if (this._dolly == null && this.cinematicVCam != null)
			{
				this._dolly = this.cinematicVCam.GetCinemachineComponent<CinemachineTrackedDolly>();
			}
			if (this.previewPath)
			{
				this.EnsurePreviewLive(true);
				if (this._dolly != null)
				{
					if (this.loopPathPreview && Application.isPlaying)
					{
						this._loopTimer += Time.deltaTime;
						float num = Mathf.Max(0.01f, this.loopPreviewDuration);
						float num2 = Mathf.PingPong(this._loopTimer / num, 1f);
						this._dolly.m_PathPosition = this.dollyCurve.Evaluate(num2);
					}
					else
					{
						this._dolly.m_PathPosition = this.dollyCurve.Evaluate(this.pathPreview);
					}
				}
				this._previewActive = true;
				return;
			}
			if (this._previewActive)
			{
				this.EnsurePreviewLive(false);
				this._loopTimer = 0f;
				this._previewActive = false;
			}
		}

		// Token: 0x06006896 RID: 26774 RVA: 0x0029D4C8 File Offset: 0x0029B6C8
		private void EnsurePreviewLive(bool live)
		{
			if (this.cinematicVCam != null)
			{
				this.cinematicVCam.Priority = (live ? this.cinematicPriority : this.releasedPriority);
			}
			if (this.brain != null)
			{
				this.brain.enabled = live;
			}
		}

		// Token: 0x04006AD6 RID: 27350
		[Header("Player")]
		[Tooltip("Bridge verso il sistema Action di Invector. Se null viene preso dal player.")]
		[SerializeField]
		private PlayerActionAnimator actionAnimator;

		// Token: 0x04006AD7 RID: 27351
		[Tooltip("Input Invector del Player. Se null viene cercato in scena.")]
		[SerializeField]
		private vThirdPersonInput playerInput;

		// Token: 0x04006AD8 RID: 27352
		[Header("Get-Up Action")]
		[Tooltip("Prefab vTriggerGenericAction che descrive l'azione di get-up (playAnimation='PrisonGetUp'). Es: Assets/Features/PrisonIntro/Prefabs/PrisonGetUp_Action.")]
		[SerializeField]
		private vTriggerGenericAction getUpTrigger;

		// Token: 0x04006AD9 RID: 27353
		[Tooltip("Id logico dell'azione (per il PlayerActionEntry). Libero, usato solo per debug/HUD.")]
		[SerializeField]
		private string actionId = "PrisonGetUp";

		// Token: 0x04006ADA RID: 27354
		[Tooltip("Durata di fallback dell'azione (s). Usata solo se il sistema non ricava la durata dall'exitTime; il flusso normale termina via vGenericAction.OnEndAction.")]
		[SerializeField]
		private float actionDuration = 8f;

		// Token: 0x04006ADB RID: 27355
		[Tooltip("Abilita il root motion durante il get-up (di default Invector lo tiene spento). La clip di alzata ha traslazione baked, quindi va attivato temporaneamente. Salvato/ripristinato automaticamente (come GameplayEmoteController).")]
		[SerializeField]
		private bool enableRootMotion = true;

		// Token: 0x04006ADC RID: 27356
		[Header("Cinemachine")]
		[Tooltip("VCam cinematica con Body=Tracked Dolly agganciata al path.")]
		[SerializeField]
		private CinemachineVirtualCamera cinematicVCam;

		// Token: 0x04006ADD RID: 27357
		[Tooltip("Priority della VCam cinematica mentre è live (deve battere la cam di gameplay).")]
		[SerializeField]
		private int cinematicPriority = 100;

		// Token: 0x04006ADE RID: 27358
		[Tooltip("Priority a cui torna la VCam cinematica una volta rilasciata.")]
		[SerializeField]
		private int releasedPriority;

		// Token: 0x04006ADF RID: 27359
		[Tooltip("CinemachineBrain sulla MainCamera. Se null viene preso da Camera.main. Nel progetto il Brain è tenuto DISABILITATO (Invector vThirdPersonCamera controlla la MainCamera): viene abilitato durante la cutscene e ri-disabilitato alla fine.")]
		[SerializeField]
		private CinemachineBrain brain;

		// Token: 0x04006AE0 RID: 27360
		[Tooltip("Se true, alla fine della cutscene il Brain viene disabilitato (Invector riprende la camera).")]
		[SerializeField]
		private bool disableBrainOnFinish = true;

		// Token: 0x04006AE1 RID: 27361
		[Tooltip("Mantiene l'inquadratura iniziale ma elimina il roll aggiunto mentre LookAt e dolly attraversano la vista quasi verticale. Posizione e forward restano Cinemachine.")]
		[SerializeField]
		private bool stabilizeCinematicRoll = true;

		// Token: 0x04006AE2 RID: 27362
		[Tooltip("Posizione normalizzata del dolly alla quale inizia la transizione controllata dall'up iniziale all'orizzonte World Up.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float worldUpBlendStart = 0.05f;

		// Token: 0x04006AE3 RID: 27363
		[Tooltip("Posizione normalizzata del dolly alla quale World Up diventa definitivo.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float worldUpBlendEnd = 0.32f;

		// Token: 0x04006AE4 RID: 27364
		[Header("Dolly Travel")]
		[Tooltip("Durata (s) della corsa della camera lungo il path, dallo zoom faccia al waypoint finale.")]
		[SerializeField]
		private float dollyTravelDuration = 2.5f;

		// Token: 0x04006AE5 RID: 27365
		[Tooltip("Curva di easing del movimento lungo il path (X=tempo 0..1, Y=posizione path 0..1).")]
		[SerializeField]
		private AnimationCurve dollyCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		// Token: 0x04006AE6 RID: 27366
		[Header("Fade")]
		[Tooltip("Durata del FadeIn iniziale (nero → trasparente).")]
		[SerializeField]
		private float fadeInDuration = 1.5f;

		// Token: 0x04006AE7 RID: 27367
		[Tooltip("Offset (s) AGGIUNTIVO dopo che lo state PrisonGetUp è realmente partito, prima di far scattare FadeIn + movimento dolly. Ora la sincronizzazione si aggancia all'animazione vera (non più a un timer stimato), quindi di norma può stare a 0; alza solo se vuoi nascondere qualche frame extra del primissimo istante del get-up.")]
		[SerializeField]
		private float fadeStartDelay;

		// Token: 0x04006AE8 RID: 27368
		[Tooltip("Ritardo (s) iniziale prima di tutto (per stabilizzare la scena al primo frame).")]
		[SerializeField]
		private float startDelay = 0.1f;

		// Token: 0x04006AE9 RID: 27369
		[Tooltip("Se true la sequenza fa il fade da nero. Disattivalo mentre testi il path.")]
		[SerializeField]
		private bool useFade = true;

		// Token: 0x04006AEA RID: 27370
		[Header("Testing")]
		[Tooltip("Se true la sequenza parte automaticamente all'avvio della scena.")]
		[SerializeField]
		private bool playOnStart = true;

		// Token: 0x04006AEB RID: 27371
		[Tooltip("In Play: premi questo tasto per (ri)avviare la sequenza di get-up dall'inizio.")]
		[SerializeField]
		private KeyCode triggerKey = KeyCode.G;

		// Token: 0x04006AEC RID: 27372
		[Tooltip("SCRUB MANUALE del dolly lungo il path (0=inizio, 1=fine). Funziona anche fuori da Play. Attivo solo quando 'Preview Path' è ON.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float pathPreview;

		// Token: 0x04006AED RID: 27373
		[Tooltip("Quando ON, la VCam diventa live e il dolly segue 'Path Preview' (scrub manuale). Usalo in editor per vedere cosa inquadra. RICORDA di spegnerlo prima del gioco.")]
		[SerializeField]
		private bool previewPath;

		// Token: 0x04006AEE RID: 27374
		[Tooltip("In Play, con Preview Path ON: fa scorrere il dolly avanti-indietro in continuo.")]
		[SerializeField]
		private bool loopPathPreview;

		// Token: 0x04006AEF RID: 27375
		[Tooltip("Durata (s) di una passata del loop preview.")]
		[SerializeField]
		private float loopPreviewDuration = 3f;

		// Token: 0x04006AF0 RID: 27376
		[Header("Face / Emotion")]
		[Tooltip("Apply the prison scene's persistent emotion/rank setup. Disable when reusing this shot in gameplay.")]
		[SerializeField]
		private bool configurePrisonEmotions = true;

		// Token: 0x04006AF1 RID: 27377
		[Tooltip("All'avvio scena porta a 0 il peso del layer 'Face': durante la cutscene si usa la faccia BAKED dell'animazione di get-up. A fine cutscene viene forzata l'espressione Embarassed + EmbSupreme con un blend (vedi sotto).")]
		[SerializeField]
		private bool disableFaceLayerAtStart = true;

		// Token: 0x04006AF2 RID: 27378
		[Tooltip("A fine cutscene forza il Player nello stato Embarassed (layer body) + faccia EmbSupreme (FaceExpressionIndex), con blend dei pesi. Forzatura PERENNE: in questa scena non c'e' EmotionManager, quindi lo stato non viene piu' sovrascritto.")]
		[SerializeField]
		private bool forceEmbarrassedOnFinish = true;

		// Token: 0x04006AF3 RID: 27379
		[Tooltip("Nome del layer facciale nell'Animator Controller di Ciel.")]
		[SerializeField]
		private string faceLayerName = "Face";

		// Token: 0x04006AF4 RID: 27380
		[Tooltip("Nome del layer body 'Embarassed' (vergogna).")]
		[SerializeField]
		private string embarrassedLayerName = "Embarassed";

		// Token: 0x04006AF5 RID: 27381
		[Tooltip("Parametro Int dell'Animator che seleziona la faccia. EmbSupreme = 4 (vedi PlayerEmotionController).")]
		[SerializeField]
		private string faceExpressionParam = "FaceExpressionIndex";

		// Token: 0x04006AF6 RID: 27382
		[Tooltip("Indice faccia EmbSupreme (Emb Face Supreme).")]
		[SerializeField]
		private int embSupremeExpressionIndex = 4;

		// Token: 0x04006AF7 RID: 27383
		[Tooltip("Parametro Float EmotionBlend (-1 vergogna .. +1 esibizione). Impostato a -1 per coerenza con la faccia di vergogna. Lascia vuoto per non toccarlo.")]
		[SerializeField]
		private string emotionBlendParam = "EmotionBlend";

		// Token: 0x04006AF8 RID: 27384
		[Tooltip("Durata (s) del blend con cui, a fine cutscene, i layer Embarassed e Face salgono 0 -> 1.")]
		[SerializeField]
		private float embBlendDuration = 0.8f;

		// Token: 0x04006AF9 RID: 27385
		[Header("HUD")]
		[Tooltip("Se true, nasconde l'HUD (HUDManager.HideHUD) per tutta la cinematica e lo ripristina (ShowHUD) a fine get-up. HUDManager risolto da .Instance.")]
		[SerializeField]
		private bool hideHudDuringCutscene = true;

		// Token: 0x04006AFA RID: 27386
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04006AFB RID: 27387
		private vThirdPersonCamera _tpCamera;

		// Token: 0x04006AFC RID: 27388
		private vGenericAction _genericAction;

		// Token: 0x04006AFD RID: 27389
		private CoverPoseController _coverPose;

		// Token: 0x04006AFE RID: 27390
		private bool _coverPoseWasEnabled;

		// Token: 0x04006AFF RID: 27391
		private CinemachineTrackedDolly _dolly;

		// Token: 0x04006B00 RID: 27392
		private Coroutine _dollyCo;

		// Token: 0x04006B01 RID: 27393
		private Coroutine _watchdogCo;

		// Token: 0x04006B02 RID: 27394
		private bool _running;

		// Token: 0x04006B03 RID: 27395
		private bool _finished;

		// Token: 0x04006B04 RID: 27396
		private bool _subscribed;

		// Token: 0x04006B05 RID: 27397
		private bool _wardrobePresentationSuppressed;

		// Token: 0x04006B06 RID: 27398
		private float _loopTimer;

		// Token: 0x04006B07 RID: 27399
		private bool _previewActive;

		// Token: 0x04006B08 RID: 27400
		private bool _actionStartedObserved;

		// Token: 0x04006B09 RID: 27401
		private bool _actionTriggered;

		// Token: 0x04006B0A RID: 27402
		private bool _cancelled;

		// Token: 0x04006B0B RID: 27403
		private bool _gameplayFaceSuppressed;

		// Token: 0x04006B0C RID: 27404
		private float _savedFaceLayerWeight;

		// Token: 0x04006B0D RID: 27405
		private float[] _savedCinematicLayerWeights;

		// Token: 0x04006B0E RID: 27406
		private Animator _cinematicAnimator;

		// Token: 0x04006B0F RID: 27407
		private bool _savedInputLock;

		// Token: 0x04006B10 RID: 27408
		private bool _savedCameraInputLock;

		// Token: 0x04006B11 RID: 27409
		private bool _savedForceCameraLock;

		// Token: 0x04006B12 RID: 27410
		private bool _savedBrainEnabled;

		// Token: 0x04006B13 RID: 27411
		private bool _prewarmBlendOverridden;

		// Token: 0x04006B14 RID: 27412
		private CinemachineBlendDefinition _savedPrewarmBlend;

		// Token: 0x04006B15 RID: 27413
		private bool _cameraAcquired;

		// Token: 0x04006B16 RID: 27414
		private static readonly HashSet<PrisonIntroSequence> RunningSequences = new HashSet<PrisonIntroSequence>();

		// Token: 0x04006B18 RID: 27416
		private int _faceLayerIndex = -1;

		// Token: 0x04006B19 RID: 27417
		private int _embLayerIndex = -1;

		// Token: 0x04006B1A RID: 27418
		private bool _faceDisabledAtStart;

		// Token: 0x04006B1B RID: 27419
		private Coroutine _embBlendCo;

		// Token: 0x04006B1C RID: 27420
		private PlayerEmotionController _emotionController;

		// Token: 0x04006B1D RID: 27421
		private EmotionManager _prisonEmotionManager;

		// Token: 0x04006B1E RID: 27422
		private EmotionStance _rankDefaultStance = EmotionStance.Embarassed;

		// Token: 0x04006B1F RID: 27423
		private bool _rankAllowsStanceSwitch;

		// Token: 0x04006B20 RID: 27424
		private bool _rankGatePushed;

		// Token: 0x04006B21 RID: 27425
		private const string PrisonRankGateReason = "Prison.RankStance";

		// Token: 0x04006B22 RID: 27426
		private Vector3 _playerStartPos;

		// Token: 0x04006B23 RID: 27427
		private Quaternion _playerStartRot;

		// Token: 0x04006B24 RID: 27428
		private bool _hasPlayerStartPose;

		// Token: 0x04006B25 RID: 27429
		private Transform _mainCamT;

		// Token: 0x04006B26 RID: 27430
		private Vector3 _mainCamLocalPos;

		// Token: 0x04006B27 RID: 27431
		private Quaternion _mainCamLocalRot;

		// Token: 0x04006B28 RID: 27432
		private bool _hasMainCamLocal;

		// Token: 0x04006B29 RID: 27433
		private bool _rollStabilizerArmed;

		// Token: 0x04006B2A RID: 27434
		private bool _rollFrameValid;

		// Token: 0x04006B2B RID: 27435
		private Vector3 _cinematicInitialUp;

		// Token: 0x04006B2C RID: 27436
		private Vector3 _transportedCameraUp;

		// Token: 0x04006B2D RID: 27437
		private Vector3 _transportedCameraRight;
	}
}
