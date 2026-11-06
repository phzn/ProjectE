using System;
using System.Collections;
using Cinemachine;
using Features.Detection;
using Features.Loading;
using Features.SecurityCamera;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Interactions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Features.Prison
{
	// Token: 0x02000DC5 RID: 3525
	[DisallowMultipleComponent]
	public class BedSleepInteractable : MonoBehaviour
	{
		// Token: 0x06006825 RID: 26661 RVA: 0x0029A4F4 File Offset: 0x002986F4
		private void Awake()
		{
			if (this.triggers != null)
			{
				if (Array.Exists<vTriggerGenericAction>(this.triggers, (vTriggerGenericAction trigger) => trigger != null))
				{
					goto IL_0041;
				}
			}
			this.triggers = base.GetComponentsInChildren<vTriggerGenericAction>(true);
			IL_0041:
			if (this.promptUI == null)
			{
				this.promptUI = base.GetComponentInChildren<InteractablePromptUI>(true);
			}
			if (this.triggers == null || this.triggers.Length == 0)
			{
				Debug.LogError("[BedSleepInteractable] Nessun vTriggerGenericAction nella gerarchia di '" + base.name + "'.", this);
			}
			if (this.promptUI != null)
			{
				this.promptUI.Hide();
			}
			if (this.brain == null && Camera.main != null)
			{
				this.brain = Camera.main.GetComponent<CinemachineBrain>();
			}
			if (Camera.main != null)
			{
				this._mainCamT = Camera.main.transform;
			}
			if (this.bedVCam != null)
			{
				this.bedVCam.Priority = this.idleCameraPriority;
			}
			if (this.securityCamera == null)
			{
				this.securityCamera = Object.FindObjectOfType<SecurityCameraController>();
			}
		}

		// Token: 0x06006826 RID: 26662 RVA: 0x0029A624 File Offset: 0x00298824
		private void OnEnable()
		{
			if (this.triggers == null)
			{
				return;
			}
			foreach (vTriggerGenericAction vTriggerGenericAction in this.triggers)
			{
				if (!(vTriggerGenericAction == null))
				{
					vTriggerGenericAction.OnValidate.AddListener(new UnityAction<GameObject>(this.HandleValidate));
					vTriggerGenericAction.OnInvalidate.AddListener(new UnityAction<GameObject>(this.HandleInvalidate));
					vTriggerGenericAction.OnPlayerExit.AddListener(new UnityAction<GameObject>(this.HandleInvalidate));
					vTriggerGenericAction.OnStartAnimation.AddListener(new UnityAction(this.HandleAnimationStart));
					vTriggerGenericAction.OnEndAnimation.AddListener(new UnityAction(this.HandleAnimationEnd));
					vTriggerGenericAction.OnCancelAction.AddListener(new UnityAction(this.HandleAnimationEnd));
				}
			}
		}

		// Token: 0x06006827 RID: 26663 RVA: 0x0029A6F0 File Offset: 0x002988F0
		private void OnDisable()
		{
			this.EndSleep();
			this._promptValidated = false;
			if (this.triggers != null)
			{
				foreach (vTriggerGenericAction vTriggerGenericAction in this.triggers)
				{
					if (!(vTriggerGenericAction == null))
					{
						vTriggerGenericAction.OnValidate.RemoveListener(new UnityAction<GameObject>(this.HandleValidate));
						vTriggerGenericAction.OnInvalidate.RemoveListener(new UnityAction<GameObject>(this.HandleInvalidate));
						vTriggerGenericAction.OnPlayerExit.RemoveListener(new UnityAction<GameObject>(this.HandleInvalidate));
						vTriggerGenericAction.OnStartAnimation.RemoveListener(new UnityAction(this.HandleAnimationStart));
						vTriggerGenericAction.OnEndAnimation.RemoveListener(new UnityAction(this.HandleAnimationEnd));
						vTriggerGenericAction.OnCancelAction.RemoveListener(new UnityAction(this.HandleAnimationEnd));
					}
				}
			}
			if (this._genericActionSubscribed && this._activeGenericAction != null)
			{
				this._activeGenericAction.OnStartAction.RemoveListener(new UnityAction<vTriggerGenericAction>(this.HandlePlayerActionStart));
				this._genericActionSubscribed = false;
			}
			this.ForceRestoreRootMotion();
			this.ReleaseCamera();
			if (this.promptUI != null)
			{
				this.promptUI.Hide();
			}
		}

		// Token: 0x06006828 RID: 26664 RVA: 0x0029A828 File Offset: 0x00298A28
		private void Update()
		{
			if (this._promptValidated && this.promptUI != null && !this.promptUI.IsVisible && this._activeGenericAction != null && !this._activeGenericAction.doingAction && this.IsOwnedTrigger(this._activeGenericAction.triggerAction) && PlayerActivityGate.CanStartWorldInteraction && !InteractablePromptUI.HasVisiblePromptOtherThan(this.promptUI))
			{
				this.promptUI.Show(this.interactionConfig);
			}
			if (this.testResetKey != KeyCode.None && GameplayInput.GetKeyDown(this.testResetKey))
			{
				this.TestResetGetUp();
			}
		}

		// Token: 0x06006829 RID: 26665 RVA: 0x0029A8C8 File Offset: 0x00298AC8
		[ContextMenu("TEST: Reset / Get Up")]
		public void TestResetGetUp()
		{
			this.EndSleep();
			if (this._activeActionAnimator != null)
			{
				this._activeActionAnimator.FinishCurrentAction();
			}
			else if (this._activeGenericAction != null && this._activeGenericAction.doingAction)
			{
				this._activeGenericAction.FinishAction();
			}
			this.ForceRestoreRootMotion();
			this.ReleaseCamera();
			if (this._playerPoseCaptured && this._activePlayerTransform != null)
			{
				this._activePlayerTransform.SetPositionAndRotation(this._playerPrePos, this._playerPreRot);
				Rigidbody component = this._activePlayerTransform.GetComponent<Rigidbody>();
				if (component != null)
				{
					component.velocity = Vector3.zero;
					component.angularVelocity = Vector3.zero;
				}
			}
			this._playerPoseCaptured = false;
			if (this.debugLogs)
			{
				Debug.Log("[BedSleepInteractable] TEST reset: player rialzato e riportato alla posa pre-azione.", this);
			}
		}

		// Token: 0x0600682A RID: 26666 RVA: 0x0029A99C File Offset: 0x00298B9C
		private void HandleValidate(GameObject player)
		{
			this._promptValidated = true;
			this._activePlayer = player;
			this._activeActionAnimator = ((player != null) ? player.GetComponentInChildren<PlayerActionAnimator>() : null);
			vGenericAction vGenericAction = ((player != null) ? player.GetComponentInChildren<vGenericAction>() : null);
			this.EnsureGenericActionSubscription(vGenericAction);
			if (this.promptUI != null)
			{
				this.promptUI.Show(this.interactionConfig);
			}
			if (this.debugLogs)
			{
				Debug.Log("[BedSleepInteractable] Validate: player=" + ((player != null) ? player.name : null) + " actionAnimator=" + ((this._activeActionAnimator != null) ? "OK" : "NULL"), this);
			}
		}

		// Token: 0x0600682B RID: 26667 RVA: 0x0029AA4C File Offset: 0x00298C4C
		private void EnsureGenericActionSubscription(vGenericAction playerGenericAction)
		{
			if (playerGenericAction == this._activeGenericAction && this._genericActionSubscribed)
			{
				return;
			}
			if (this._genericActionSubscribed && this._activeGenericAction != null)
			{
				this._activeGenericAction.OnStartAction.RemoveListener(new UnityAction<vTriggerGenericAction>(this.HandlePlayerActionStart));
				this._genericActionSubscribed = false;
			}
			this._activeGenericAction = playerGenericAction;
			if (this._activeGenericAction != null && this._activeGenericAction.OnStartAction != null)
			{
				this._activeGenericAction.OnStartAction.AddListener(new UnityAction<vTriggerGenericAction>(this.HandlePlayerActionStart));
				this._genericActionSubscribed = true;
			}
		}

		// Token: 0x0600682C RID: 26668 RVA: 0x0029AAEE File Offset: 0x00298CEE
		private void HandlePlayerActionStart(vTriggerGenericAction firingTrigger)
		{
			if (!this.IsOwnedTrigger(firingTrigger))
			{
				return;
			}
			if (!this.TryBlockSleepActivities())
			{
				return;
			}
			this.CapturePlayerPose();
			this.AlignPlayerToTarget(firingTrigger);
			this.EnableRootMotion();
			this.EngageCamera();
			this.LockEverythingForSleep();
			this.StartSleepSequence();
		}

		// Token: 0x0600682D RID: 26669 RVA: 0x0029AB28 File Offset: 0x00298D28
		private void AlignPlayerToTarget(vTriggerGenericAction firingTrigger)
		{
			if (this._activePlayerTransform == null)
			{
				return;
			}
			Transform transform = ((firingTrigger != null) ? firingTrigger.matchTarget : null);
			if (transform == null)
			{
				return;
			}
			Vector3 vector = (this.alignToTargetPositionOnStart ? transform.position : this._activePlayerTransform.position);
			Quaternion quaternion = (this.alignToTargetRotationOnStart ? Quaternion.Euler(0f, transform.eulerAngles.y, 0f) : this._activePlayerTransform.rotation);
			this._activePlayerTransform.SetPositionAndRotation(vector, quaternion);
			Rigidbody component = this._activePlayerTransform.GetComponent<Rigidbody>();
			if (component != null)
			{
				component.position = vector;
				component.rotation = quaternion;
				component.velocity = Vector3.zero;
				component.angularVelocity = Vector3.zero;
			}
			Physics.SyncTransforms();
			if (this.debugLogs)
			{
				Debug.Log("[BedSleepInteractable] Posa iniziale allineata al target: " + string.Format("pos={0}, yaw={1:0.0}°", vector, quaternion.eulerAngles.y), this);
			}
		}

		// Token: 0x0600682E RID: 26670 RVA: 0x0029AC34 File Offset: 0x00298E34
		private void CapturePlayerPose()
		{
			if (this._playerPoseCaptured)
			{
				return;
			}
			if (this._activePlayer == null)
			{
				return;
			}
			this._activePlayerTransform = ((this._activeGenericAction != null) ? this._activeGenericAction.transform : this._activePlayer.transform);
			this._playerPrePos = this._activePlayerTransform.position;
			this._playerPreRot = this._activePlayerTransform.rotation;
			this._playerPoseCaptured = true;
		}

		// Token: 0x0600682F RID: 26671 RVA: 0x0029ACB0 File Offset: 0x00298EB0
		private bool IsOwnedTrigger(vTriggerGenericAction t)
		{
			if (t == null || this.triggers == null)
			{
				return false;
			}
			for (int i = 0; i < this.triggers.Length; i++)
			{
				if (this.triggers[i] == t)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006830 RID: 26672 RVA: 0x0029ACF8 File Offset: 0x00298EF8
		private void EnableRootMotion()
		{
			if (this._rootMotionPushed)
			{
				return;
			}
			if (this._activeActionAnimator == null)
			{
				this._activeActionAnimator = Object.FindObjectOfType<PlayerActionAnimator>();
			}
			if (this._activeActionAnimator == null)
			{
				Debug.LogError("[BedSleepInteractable] OnStartAction fired ma nessun PlayerActionAnimator in scena. Il coricamento giocherà senza root motion.", this);
				return;
			}
			this._activeActionAnimator.DisableEmotionLayers();
			this._activeActionAnimator.EnableRootMotionForAction();
			this._rootMotionPushed = true;
			if (this.debugLogs)
			{
				Debug.Log("[BedSleepInteractable] Root motion ON + emotion layers OFF per il coricamento", this);
			}
		}

		// Token: 0x06006831 RID: 26673 RVA: 0x0029AD74 File Offset: 0x00298F74
		private void HandleInvalidate(GameObject player)
		{
			this._promptValidated = false;
			if (this.promptUI != null)
			{
				this.promptUI.Hide();
			}
			if (this.debugLogs)
			{
				Debug.Log("[BedSleepInteractable] Invalidate (player=" + ((player != null) ? player.name : null) + ")", this);
			}
		}

		// Token: 0x06006832 RID: 26674 RVA: 0x0029ADCC File Offset: 0x00298FCC
		private void HandleAnimationStart()
		{
			if (!this.TryBlockSleepActivities())
			{
				return;
			}
			this._promptValidated = false;
			if (this.promptUI != null)
			{
				this.promptUI.Hide();
			}
			this.EnableRootMotion();
			this.EngageCamera();
			this.LockEverythingForSleep();
			this.StartSleepSequence();
		}

		// Token: 0x06006833 RID: 26675 RVA: 0x0029AE1A File Offset: 0x0029901A
		private void HandleAnimationEnd()
		{
			this.EndSleep();
			this.ForceRestoreRootMotion();
			this.ReleaseCamera();
		}

		// Token: 0x06006834 RID: 26676 RVA: 0x0029AE30 File Offset: 0x00299030
		private void ForceRestoreRootMotion()
		{
			if (!this._rootMotionPushed)
			{
				return;
			}
			if (this._activeActionAnimator != null)
			{
				this._activeActionAnimator.RestoreRootMotionForAction();
				this._activeActionAnimator.RestoreOverrideLayer();
			}
			this._rootMotionPushed = false;
			if (this.debugLogs)
			{
				Debug.Log("[BedSleepInteractable] Root motion + emotion layers ripristinati", this);
			}
		}

		// Token: 0x06006835 RID: 26677 RVA: 0x0029AE84 File Offset: 0x00299084
		private void LockEverythingForSleep()
		{
			if (!this.TryBlockSleepActivities())
			{
				return;
			}
			if (!this.lockEverythingOnSleep || this._sleepLockApplied)
			{
				return;
			}
			this._sleepLockApplied = true;
			this._sleepHud = HUDManager.Instance;
			if (this._sleepHud != null)
			{
				this._savedHudHidden = this._sleepHud.IsHidden;
				this._sleepHud.HideHUD();
			}
			vThirdPersonInput vThirdPersonInput = ((this._activePlayer != null) ? this._activePlayer.GetComponentInChildren<vThirdPersonInput>() : Object.FindObjectOfType<vThirdPersonInput>());
			if (vThirdPersonInput != null)
			{
				this._sleepInput = vThirdPersonInput;
				this._savedInputLock = vThirdPersonInput.lockInput;
				this._savedCameraInputLock = vThirdPersonInput.lockCameraInput;
				vThirdPersonInput.SetLockAllInput(true);
				vThirdPersonInput.SetLockCameraInput(true);
				if (vThirdPersonInput.cc != null)
				{
					vThirdPersonInput.cc.input = Vector2.zero;
					vThirdPersonInput.cc.isSprinting = false;
				}
			}
			if (this.securityCamera == null)
			{
				this.securityCamera = Object.FindObjectOfType<SecurityCameraController>();
			}
			if (this.securityCamera != null)
			{
				this._savedSecurityCameraEnabled = this.securityCamera.enabled;
				this.securityCamera.enabled = false;
			}
			PlayerHeadAim playerHeadAim = ((this._activePlayer != null) ? this._activePlayer.GetComponentInChildren<PlayerHeadAim>(true) : Object.FindObjectOfType<PlayerHeadAim>());
			if (playerHeadAim != null)
			{
				this._sleepHeadAim = playerHeadAim;
				this._savedHeadAimEnabled = playerHeadAim.enabled;
				playerHeadAim.ClearTarget();
				playerHeadAim.enabled = false;
			}
			if (this.debugLogs)
			{
				Debug.Log("[BedSleepInteractable] Sleep lock: HUD off + input/CCTV/head-aim bloccati.", this);
			}
		}

		// Token: 0x06006836 RID: 26678 RVA: 0x0029B010 File Offset: 0x00299210
		private bool TryBlockSleepActivities()
		{
			if (this._sleepActivityBlocked)
			{
				return true;
			}
			if (!PlayerActivityGate.CanStartWorldInteraction || PlayerActivityGate.IsResourceOwned(PlayerContextResource.FullBodyAnimation))
			{
				if (this._activeGenericAction != null && this.IsOwnedTrigger(this._activeGenericAction.triggerAction))
				{
					if (this._activeActionAnimator != null)
					{
						this._activeActionAnimator.CancelCurrentAction();
					}
					else
					{
						this._activeGenericAction.CancelAction();
						if (this._activeGenericAction.doingAction)
						{
							this._activeGenericAction.ResetTriggerSettings(true);
						}
					}
				}
				return false;
			}
			this._sleepActivityReason = "PrisonSleep." + base.GetInstanceID().ToString();
			this._sleepActivityBlocked = true;
			PlayerActivityGate.Push(this._sleepActivityReason, PlayerActivity.AllPlayerActions);
			return true;
		}

		// Token: 0x06006837 RID: 26679 RVA: 0x0029B0D0 File Offset: 0x002992D0
		private void EndSleep()
		{
			if (this._sleepSequenceCo != null)
			{
				base.StopCoroutine(this._sleepSequenceCo);
			}
			this._sleepSequenceCo = null;
			this._sleepSequenceStarted = false;
			if (this._sleepLockApplied)
			{
				this._sleepLockApplied = false;
				if (this._sleepInput != null)
				{
					this._sleepInput.SetLockAllInput(this._savedInputLock);
					this._sleepInput.SetLockCameraInput(this._savedCameraInputLock);
				}
				if (this._sleepHeadAim != null)
				{
					this._sleepHeadAim.enabled = this._savedHeadAimEnabled;
				}
				if (this.securityCamera != null)
				{
					this.securityCamera.enabled = this._savedSecurityCameraEnabled;
				}
				if (this._sleepHud != null && !this._savedHudHidden)
				{
					this._sleepHud.ShowHUD();
				}
				this._sleepInput = null;
				this._sleepHeadAim = null;
				this._sleepHud = null;
			}
			if (this._sleepActivityBlocked)
			{
				this._sleepActivityBlocked = false;
				PlayerActivityGate.Pop(this._sleepActivityReason);
			}
		}

		// Token: 0x06006838 RID: 26680 RVA: 0x0029B1CF File Offset: 0x002993CF
		private void StartSleepSequence()
		{
			if (!this.loadHomeOnSleep)
			{
				return;
			}
			if (this._sleepSequenceStarted)
			{
				return;
			}
			this._sleepSequenceStarted = true;
			this._sleepSequenceCo = base.StartCoroutine(this.SleepThenLoadHomeRoutine());
		}

		// Token: 0x06006839 RID: 26681 RVA: 0x0029B1FC File Offset: 0x002993FC
		private IEnumerator SleepThenLoadHomeRoutine()
		{
			Animator animator = this.ResolvePlayerAnimator();
			if (animator != null && !string.IsNullOrEmpty(this.sleepLoopStateName))
			{
				float timeout = Time.time + this.sleepStateTimeout;
				bool reached = false;
				while (Time.time < timeout)
				{
					if (animator.GetCurrentAnimatorStateInfo(0).IsName(this.sleepLoopStateName))
					{
						reached = true;
						break;
					}
					yield return null;
				}
				if (this.debugLogs)
				{
					Debug.Log(reached ? string.Format("[BedSleepInteractable] Loop di sonno '{0}' raggiunto → hold {1}s, poi fade+load.", this.sleepLoopStateName, this.sleepHoldBeforeFade) : string.Format("[BedSleepInteractable] Timeout attesa '{0}' ({1}s) → fade+load comunque.", this.sleepLoopStateName, this.sleepStateTimeout), this);
				}
			}
			if (this.sleepHoldBeforeFade > 0f)
			{
				yield return new WaitForSecondsRealtime(this.sleepHoldBeforeFade);
			}
			if (ScreenFadeController.Instance != null)
			{
				BedSleepInteractable.<>c__DisplayClass67_0 CS$<>8__locals1 = new BedSleepInteractable.<>c__DisplayClass67_0();
				CS$<>8__locals1.fadeDone = false;
				ScreenFadeController.Instance.FadeOut(this.fadeOutDuration, delegate
				{
					CS$<>8__locals1.fadeDone = true;
				});
				while (!CS$<>8__locals1.fadeDone)
				{
					yield return null;
				}
				CS$<>8__locals1 = null;
			}
			else if (this.debugLogs)
			{
				Debug.LogWarning("[BedSleepInteractable] ScreenFadeController.Instance NULL → nessun fade, carico direttamente.", this);
			}
			if (LoadingScreenController.Instance != null)
			{
				LoadingScreenController.Instance.LoadSceneAsync(this.homeSceneName);
			}
			else
			{
				SceneManager.LoadScene(this.homeSceneName);
			}
			this._sleepSequenceCo = null;
			yield break;
		}

		// Token: 0x0600683A RID: 26682 RVA: 0x0029B20C File Offset: 0x0029940C
		private Animator ResolvePlayerAnimator()
		{
			if (this._activeGenericAction != null && this._activeGenericAction.tpInput != null && this._activeGenericAction.tpInput.cc != null && this._activeGenericAction.tpInput.cc.animator != null)
			{
				return this._activeGenericAction.tpInput.cc.animator;
			}
			if (this._activePlayer != null)
			{
				Animator componentInChildren = this._activePlayer.GetComponentInChildren<Animator>();
				if (componentInChildren != null)
				{
					return componentInChildren;
				}
			}
			return null;
		}

		// Token: 0x0600683B RID: 26683 RVA: 0x0029B2AC File Offset: 0x002994AC
		private void EngageCamera()
		{
			if (this._cameraLive)
			{
				return;
			}
			if (this.bedVCam == null)
			{
				return;
			}
			if (this._mainCamT == null && Camera.main != null)
			{
				this._mainCamT = Camera.main.transform;
			}
			if (this._mainCamT != null && !this._mainCamLocalSaved)
			{
				this._mainCamLocalPos = this._mainCamT.localPosition;
				this._mainCamLocalRot = this._mainCamT.localRotation;
				this._mainCamLocalSaved = true;
			}
			if (this.brain == null && Camera.main != null)
			{
				this.brain = Camera.main.GetComponent<CinemachineBrain>();
			}
			if (this.brain != null)
			{
				this.brain.enabled = true;
			}
			this.bedVCam.Priority = this.liveCameraPriority;
			this._cameraLive = true;
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[BedSleepInteractable] Camera ENGAGE: brain.enabled={0} ", this.brain != null && this.brain.enabled) + string.Format("bedVCam.Priority={0}", this.bedVCam.Priority), this);
			}
		}

		// Token: 0x0600683C RID: 26684 RVA: 0x0029B3F0 File Offset: 0x002995F0
		private void ReleaseCamera()
		{
			if (!this._cameraLive)
			{
				return;
			}
			if (this.bedVCam != null)
			{
				this.bedVCam.Priority = this.idleCameraPriority;
			}
			if (this.brain != null)
			{
				this.brain.enabled = false;
			}
			if (this._mainCamT != null && this._mainCamLocalSaved)
			{
				this._mainCamT.localPosition = this._mainCamLocalPos;
				this._mainCamT.localRotation = this._mainCamLocalRot;
			}
			this._mainCamLocalSaved = false;
			this._cameraLive = false;
			if (this.debugLogs)
			{
				Debug.Log("[BedSleepInteractable] Camera RELEASE", this);
			}
		}

		// Token: 0x04006A89 RID: 27273
		[Header("References")]
		[Tooltip("vTriggerGenericAction che guida l'animazione di coricamento. Auto-risolto da questo GO / figli se vuoto.")]
		[SerializeField]
		private vTriggerGenericAction[] triggers;

		// Token: 0x04006A8A RID: 27274
		[Tooltip("Prompt world-space mostrato quando il player è in range. Auto-risolto dai figli.")]
		[SerializeField]
		private InteractablePromptUI promptUI;

		// Token: 0x04006A8B RID: 27275
		[Header("UI Config")]
		[Tooltip("Testo + tasto mostrati dal prompt (es. 'Bed' / 'E' / 'Lie down'). Richiesto.")]
		[SerializeField]
		private InteractionConfigSO interactionConfig;

		// Token: 0x04006A8C RID: 27276
		[Header("Camera")]
		[Tooltip("VCam Cinemachine dedicata che inquadra il letto. Priority alzata quando il player si corica, abbassata al rilascio.")]
		[SerializeField]
		private CinemachineVirtualCamera bedVCam;

		// Token: 0x04006A8D RID: 27277
		[Tooltip("CinemachineBrain sulla MainCamera. Se null viene preso da Camera.main. In questo progetto il Brain è tenuto DISABILITATO in gameplay (la vThirdPersonCamera Invector controlla la MainCamera) e va acceso solo durante le cutscene.")]
		[SerializeField]
		private CinemachineBrain brain;

		// Token: 0x04006A8E RID: 27278
		[Tooltip("Priority della bedVCam mentre il player è coricato (deve battere le altre VCam).")]
		[SerializeField]
		private int liveCameraPriority = 100;

		// Token: 0x04006A8F RID: 27279
		[Tooltip("Priority della bedVCam a riposo.")]
		[SerializeField]
		private int idleCameraPriority;

		// Token: 0x04006A90 RID: 27280
		[Header("Allineamento iniziale")]
		[Tooltip("Se true, all'AVVIO dell'azione il player viene ruotato (solo yaw/Y) per combaciare con la rotazione del nodo matchTarget, PRIMA che la clip parta. Così la clip di coricamento (root motion puro) inizia già nel verso giusto. È uno snap istantaneo della SOLA rotazione di partenza, indipendente dalla matchRotationCurve (che invece agisce durante l'animazione).")]
		[SerializeField]
		private bool alignToTargetRotationOnStart = true;

		// Token: 0x04006A91 RID: 27281
		[Tooltip("Se true, all'avvio dell'azione il root del player viene anche spostato sul matchTarget. L'animazione parte cosi' sempre dalla stessa posa authored, indipendentemente dal punto del collider dal quale e' stata premuta E.")]
		[SerializeField]
		private bool alignToTargetPositionOnStart = true;

		// Token: 0x04006A92 RID: 27282
		[Header("Sleep → Home transition")]
		[Tooltip("Quando il player si corica e l'animazione raggiunge il loop di sonno, partono (dopo 'sleepHoldBeforeFade') un FadeOut a nero e il caricamento della scena casa.")]
		[SerializeField]
		private bool loadHomeOnSleep = true;

		// Token: 0x04006A93 RID: 27283
		[Tooltip("Nome della scena da caricare (la casa di Ciel). Deve essere nelle Build Settings.")]
		[SerializeField]
		private string homeSceneName = "CielHome";

		// Token: 0x04006A94 RID: 27284
		[Tooltip("Short name dello state Animator del loop di sonno. Quando questo state diventa attivo, le due clip (PrisonSleep → PrisonSleepLoop) si sono concluse e parte la sequenza fade+load.")]
		[SerializeField]
		private string sleepLoopStateName = "PrisonSleepLoop";

		// Token: 0x04006A95 RID: 27285
		[Tooltip("Secondi di 'sonno' visibile dopo l'ingresso nel loop, prima di iniziare il FadeOut.")]
		[Min(0f)]
		[SerializeField]
		private float sleepHoldBeforeFade = 1.5f;

		// Token: 0x04006A96 RID: 27286
		[Tooltip("Durata del FadeOut a nero prima del caricamento.")]
		[Min(0f)]
		[SerializeField]
		private float fadeOutDuration = 1f;

		// Token: 0x04006A97 RID: 27287
		[Tooltip("Safety net: se lo state del loop non diventa mai attivo entro questi secondi dall'avvio dell'azione, la sequenza fade+load parte comunque.")]
		[Min(1f)]
		[SerializeField]
		private float sleepStateTimeout = 6f;

		// Token: 0x04006A98 RID: 27288
		[Header("Lock on Sleep")]
		[Tooltip("Se true, all'avvio dell'azione Sleep nasconde l'HUD e blocca OGNI input (movimento, camera, security camera CCTV, PlayerHeadAim). Stato terminale: non viene ripristinato perché la scena cambia subito dopo.")]
		[SerializeField]
		private bool lockEverythingOnSleep = true;

		// Token: 0x04006A99 RID: 27289
		[Tooltip("SecurityCameraController da disabilitare durante il sonno (blocca CCTV/tasto Tab). Se vuoto viene risolto in scena.")]
		[SerializeField]
		private SecurityCameraController securityCamera;

		// Token: 0x04006A9A RID: 27290
		[Header("Debug / Test")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04006A9B RID: 27291
		[Tooltip("SOLO PER TEST: tasto che fa rialzare/resettare Ciel senza riavviare la scena, così puoi iterare sul posizionamento del nodo 'target' (snap). Mette fine all'azione, ripristina root motion/emotion layers/camera e riporta il player alla posa che aveva PRIMA di coricarsi. Default R.")]
		[SerializeField]
		private KeyCode testResetKey = KeyCode.R;

		// Token: 0x04006A9C RID: 27292
		private GameObject _activePlayer;

		// Token: 0x04006A9D RID: 27293
		private PlayerActionAnimator _activeActionAnimator;

		// Token: 0x04006A9E RID: 27294
		private vGenericAction _activeGenericAction;

		// Token: 0x04006A9F RID: 27295
		private bool _genericActionSubscribed;

		// Token: 0x04006AA0 RID: 27296
		private bool _rootMotionPushed;

		// Token: 0x04006AA1 RID: 27297
		private bool _cameraLive;

		// Token: 0x04006AA2 RID: 27298
		private bool _promptValidated;

		// Token: 0x04006AA3 RID: 27299
		private Transform _activePlayerTransform;

		// Token: 0x04006AA4 RID: 27300
		private Vector3 _playerPrePos;

		// Token: 0x04006AA5 RID: 27301
		private Quaternion _playerPreRot;

		// Token: 0x04006AA6 RID: 27302
		private bool _playerPoseCaptured;

		// Token: 0x04006AA7 RID: 27303
		private Transform _mainCamT;

		// Token: 0x04006AA8 RID: 27304
		private Vector3 _mainCamLocalPos;

		// Token: 0x04006AA9 RID: 27305
		private Quaternion _mainCamLocalRot;

		// Token: 0x04006AAA RID: 27306
		private bool _mainCamLocalSaved;

		// Token: 0x04006AAB RID: 27307
		private bool _sleepSequenceStarted;

		// Token: 0x04006AAC RID: 27308
		private Coroutine _sleepSequenceCo;

		// Token: 0x04006AAD RID: 27309
		private string _sleepActivityReason;

		// Token: 0x04006AAE RID: 27310
		private bool _sleepActivityBlocked;

		// Token: 0x04006AAF RID: 27311
		private bool _sleepLockApplied;

		// Token: 0x04006AB0 RID: 27312
		private vThirdPersonInput _sleepInput;

		// Token: 0x04006AB1 RID: 27313
		private bool _savedInputLock;

		// Token: 0x04006AB2 RID: 27314
		private bool _savedCameraInputLock;

		// Token: 0x04006AB3 RID: 27315
		private HUDManager _sleepHud;

		// Token: 0x04006AB4 RID: 27316
		private bool _savedHudHidden;

		// Token: 0x04006AB5 RID: 27317
		private PlayerHeadAim _sleepHeadAim;

		// Token: 0x04006AB6 RID: 27318
		private bool _savedHeadAimEnabled;

		// Token: 0x04006AB7 RID: 27319
		private bool _savedSecurityCameraEnabled;
	}
}
