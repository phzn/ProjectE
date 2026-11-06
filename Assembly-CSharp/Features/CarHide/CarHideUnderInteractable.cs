using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Features.DebugTools;
using Features.Detection;
using Features.MainMenu;
using Features.Rank;
using Features.RenderingPrecision;
using Features.RunModes;
using Features.Score;
using Features.Vehicles.Traffic;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using MagicaCloth2;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.FirstPerson;
using ProjectExhibitionist.Interactions;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using VehicleSpawnPlugin;

namespace Features.CarHide
{
	// Token: 0x020014C7 RID: 5319
	[DisallowMultipleComponent]
	public class CarHideUnderInteractable : MonoBehaviour
	{
		// Token: 0x1700191D RID: 6429
		// (get) Token: 0x0600A37A RID: 41850 RVA: 0x0043EC2F File Offset: 0x0043CE2F
		public bool CanStartNewRide
		{
			get
			{
				if (!(this._roofCarrier != null))
				{
					return !this._hideConsumed;
				}
				return this._roofCarrier.CanStartNewRide;
			}
		}

		// Token: 0x1700191E RID: 6430
		// (get) Token: 0x0600A37B RID: 41851 RVA: 0x0043EC54 File Offset: 0x0043CE54
		public float CurrentTravelSpeed
		{
			get
			{
				if (this._hasCarSpeedSample)
				{
					return Mathf.Max(this._measuredCarSpeed, (this._vehicle != null && this._vehicle.CurrentTrack != null) ? this._vehicle.CurrentSpeed : 0f);
				}
				return float.PositiveInfinity;
			}
		}

		// Token: 0x1700191F RID: 6431
		// (get) Token: 0x0600A37C RID: 41852 RVA: 0x0043ECB0 File Offset: 0x0043CEB0
		private Transform CarRoot
		{
			get
			{
				if (this._vehicle != null)
				{
					return this._vehicle.transform;
				}
				if (!(base.transform.parent != null))
				{
					return base.transform;
				}
				return base.transform.parent;
			}
		}

		// Token: 0x0600A37D RID: 41853 RVA: 0x0043ECFC File Offset: 0x0043CEFC
		private void Awake()
		{
			ControllerContextBindings.SetLabel(this.hiddenHudConfig, "X", "B", false);
			ControllerContextBindings.SetLabel(this.hiddenHudConfig, "F", "Y", false);
			ControllerContextBindings.Set(this.exitInput, "B");
			ControllerContextBindings.Set(this.switchCameraInput, "Y");
			if (this.rangeTriggers == null || this.rangeTriggers.Length == 0)
			{
				this.rangeTriggers = base.GetComponentsInChildren<vTriggerGenericAction>(true);
			}
			this._vehicle = base.GetComponentInParent<VehicleAI>();
			if (this._vehicle != null && !this._vehicle.enabled)
			{
				this._vehicle = null;
			}
			if (this._vehicle != null)
			{
				this._rideLimiter = this._vehicle.GetComponent<CarRideLimiter>();
				if (this._rideLimiter == null)
				{
					this._rideLimiter = this._vehicle.gameObject.AddComponent<CarRideLimiter>();
				}
			}
			this._roofCarrier = this.CarRoot.GetComponentInChildren<CarRoofCarrier>(true);
			if (this.promptUI == null)
			{
				this.promptUI = this.CarRoot.GetComponentInChildren<InteractablePromptUI>(true);
			}
			this.AutoResolveAnchors();
			if (this.rangeTriggers == null || this.rangeTriggers.Length == 0)
			{
				Debug.LogError("[CarHide] No vTriggerGenericAction found under '" + base.name + "' for range detection.", this);
			}
			if (this.enterStart == null || this.underCarAnchor == null)
			{
				Debug.LogError("[CarHide] Missing anchors on '" + this.CarRoot.name + "' — run Tools/CarHide/Setup Selected Car.", this);
			}
		}

		// Token: 0x0600A37E RID: 41854 RVA: 0x0043EE84 File Offset: 0x0043D084
		private void AutoResolveAnchors()
		{
			foreach (Transform transform in this.CarRoot.GetComponentsInChildren<Transform>(true))
			{
				if (this.enterStart == null && transform.name == "CarHide_EnterStart")
				{
					this.enterStart = transform;
				}
				else if (this.underCarAnchor == null && transform.name == "CarHide_UnderCar")
				{
					this.underCarAnchor = transform;
				}
			}
			if (this.hideCameras == null || this.hideCameras.Length == 0)
			{
				List<Camera> list = new List<Camera>();
				foreach (Camera camera in this.CarRoot.GetComponentsInChildren<Camera>(true))
				{
					if (camera.name.StartsWith("CarHideCamera"))
					{
						list.Add(camera);
					}
				}
				list.Sort((Camera a, Camera b) => string.CompareOrdinal(a.name, b.name));
				this.hideCameras = list.ToArray();
			}
		}

		// Token: 0x0600A37F RID: 41855 RVA: 0x0043EF88 File Offset: 0x0043D188
		private void OnEnable()
		{
			this._lastCarPos = this.CarRoot.position;
			this._lastSpeedSampleTime = Time.time;
			this._measuredCarSpeed = 0f;
			this._hasCarSpeedSample = false;
			WorldOrigin.Shifted += this.HandleWorldOriginShift;
			if (this.rangeTriggers == null)
			{
				return;
			}
			foreach (vTriggerGenericAction vTriggerGenericAction in this.rangeTriggers)
			{
				if (!(vTriggerGenericAction == null))
				{
					vTriggerGenericAction.OnValidate.AddListener(new UnityAction<GameObject>(this.HandleValidate));
					vTriggerGenericAction.OnInvalidate.AddListener(new UnityAction<GameObject>(this.HandleInvalidate));
					vTriggerGenericAction.OnPlayerExit.AddListener(new UnityAction<GameObject>(this.HandleInvalidate));
				}
			}
		}

		// Token: 0x0600A380 RID: 41856 RVA: 0x0043F044 File Offset: 0x0043D244
		private void OnDisable()
		{
			WorldOrigin.Shifted -= this.HandleWorldOriginShift;
			this._playerInRange = false;
			this._rangeGenericAction = null;
			this._lifecycleGeneration++;
			if (this.rangeTriggers != null)
			{
				foreach (vTriggerGenericAction vTriggerGenericAction in this.rangeTriggers)
				{
					if (!(vTriggerGenericAction == null))
					{
						vTriggerGenericAction.OnValidate.RemoveListener(new UnityAction<GameObject>(this.HandleValidate));
						vTriggerGenericAction.OnInvalidate.RemoveListener(new UnityAction<GameObject>(this.HandleInvalidate));
						vTriggerGenericAction.OnPlayerExit.RemoveListener(new UnityAction<GameObject>(this.HandleInvalidate));
					}
				}
			}
			if (this._state != CarHideUnderInteractable.HideState.None)
			{
				this.ForceAbort();
				return;
			}
			if (this._sequence != null)
			{
				base.StopCoroutine(this._sequence);
				this._sequence = null;
			}
			if (this._restartRoutine != null)
			{
				base.StopCoroutine(this._restartRoutine);
				this._restartRoutine = null;
			}
			this.ReleaseTrafficOwnership();
			this.ReleaseHideContext();
		}

		// Token: 0x0600A381 RID: 41857 RVA: 0x0043F140 File Offset: 0x0043D340
		private void Update()
		{
			if (FlyCamera.IsActive)
			{
				return;
			}
			float num = Time.time - this._lastSpeedSampleTime;
			if (num >= 0.5f)
			{
				Vector3 position = this.CarRoot.position;
				this._measuredCarSpeed = (position - this._lastCarPos).magnitude / num;
				this._lastCarPos = position;
				this._lastSpeedSampleTime = Time.time;
				this._hasCarSpeedSample = true;
			}
			CarHideUnderInteractable.HideState state = this._state;
			if (state != CarHideUnderInteractable.HideState.None)
			{
				if (state != CarHideUnderInteractable.HideState.Hidden)
				{
					return;
				}
				if (!PlayerActivityGate.IsResourceOwned(PlayerContextResource.ModalUI))
				{
					if (Time.time - this._hiddenSince < 0.5f)
					{
						return;
					}
					if (this.exitInput != null && this.exitInput.GetButtonDown())
					{
						this.BeginExit();
						return;
					}
					if (this.switchCameraInput != null && this.switchCameraInput.GetButtonDown())
					{
						this.CycleHideCamera();
					}
				}
			}
			else
			{
				if (GuidedRankGameplayPolicy.CarInteractionsBlocked)
				{
					this.ReleaseProximityHold();
					return;
				}
				this.UpdateProximityHold();
				this.RefreshSelectedRange();
				if (this._playerInRange && this.hideInput != null && this.hideInput.GetButtonDown())
				{
					if (this.CanStartHide())
					{
						this.BeginHide();
						return;
					}
					if (!this.CanAcquireHideContext)
					{
						BlockedActionFeedback.Show("Stop what you're doing first");
						return;
					}
				}
			}
		}

		// Token: 0x17001920 RID: 6432
		// (get) Token: 0x0600A382 RID: 41858 RVA: 0x0043F26E File Offset: 0x0043D46E
		public bool IsBusy
		{
			get
			{
				return this._state > CarHideUnderInteractable.HideState.None;
			}
		}

		// Token: 0x0600A383 RID: 41859 RVA: 0x0043F27C File Offset: 0x0043D47C
		public void DebugForceHide()
		{
			if (this._state != CarHideUnderInteractable.HideState.None)
			{
				Debug.LogWarning("[CarHide] DebugForceHide: already hiding.", this);
				return;
			}
			if (!this.CanStartHide())
			{
				vGenericAction vGenericAction = ((this._actionAnimator != null) ? this._actionAnimator.GenericAction : null);
				Debug.LogWarning(string.Concat(new string[]
				{
					"[CarHide] DebugForceHide: CanStartHide()=false. ",
					string.Format("controller={0} animator={1} ", this._controller != null, this._playerAnimator != null),
					string.Format("customAction={0} ", this._controller != null && this._controller.customAction),
					string.Format("jumping={0} ", this._controller != null && this._controller.isJumping),
					string.Format("ragdolled={0} ", this._controller != null && this._controller.ragdolled),
					string.Format("grounded={0} ", this._controller != null && this._controller.isGrounded),
					string.Format("doingAction={0} ", vGenericAction != null && vGenericAction.doingAction),
					string.Format("fading={0} ", ScreenFadeController.Instance != null && ScreenFadeController.Instance.IsFading),
					string.Format("vehicle={0} aiSpeed={1:0.00} ", this._vehicle != null, (this._vehicle != null) ? this._vehicle.CurrentSpeed : (-1f)),
					string.Format("measuredSpeed={0:0.00} ", this._measuredCarSpeed),
					string.Format("anchors={0}", this.enterStart != null && this.underCarAnchor != null)
				}), this);
				return;
			}
			RunModeService.DisqualifyLeaderboardRun("A hiding action was forced with a debug control.");
			this.BeginHide();
		}

		// Token: 0x0600A384 RID: 41860 RVA: 0x0043F4B4 File Offset: 0x0043D6B4
		public void DebugCycleCamera()
		{
			if (this._state != CarHideUnderInteractable.HideState.Hidden)
			{
				Debug.LogWarning(string.Format("[CarHide] DebugCycleCamera: state={0}.", this._state), this);
				return;
			}
			this.CycleHideCamera();
			Debug.Log(string.Format("[CarHide] camera preset -> index {0} ({1})", this._hideCameraIndex, (this._activeHideCamera != null) ? this._activeHideCamera.name : "none"), this);
		}

		// Token: 0x0600A385 RID: 41861 RVA: 0x0043F528 File Offset: 0x0043D728
		public void DebugLogState()
		{
			AnimatorStateInfo animatorStateInfo = ((this._playerAnimator != null) ? this._playerAnimator.GetCurrentAnimatorStateInfo(0) : default(AnimatorStateInfo));
			Debug.Log(string.Format("[CarHide] state={0} inRange={1} ", this._state, this._playerInRange) + string.Format("animState(Enter/Idle/Exit)={0}/{1}/{2} ", animatorStateInfo.IsName("Ciel_CarHide_Enter"), animatorStateInfo.IsName("Ciel_CarHide_Idle"), animatorStateInfo.IsName("Ciel_CarHide_Exit")) + string.Format("nt={0:0.00}", animatorStateInfo.normalizedTime), this);
			if (this._playerAnimator != null)
			{
				StringBuilder stringBuilder = new StringBuilder("[CarHide] layer weights: ");
				for (int i = 0; i < this._playerAnimator.layerCount; i++)
				{
					stringBuilder.Append(string.Format("{0}={1:0.00} ", this._playerAnimator.GetLayerName(i), this._playerAnimator.GetLayerWeight(i)));
				}
				stringBuilder.Append(string.Format("| applyRootMotion={0}", this._playerAnimator.applyRootMotion));
				Debug.Log(stringBuilder.ToString(), this);
			}
		}

		// Token: 0x0600A386 RID: 41862 RVA: 0x0043F663 File Offset: 0x0043D863
		public void DebugForceExit()
		{
			if (this._state != CarHideUnderInteractable.HideState.Hidden)
			{
				Debug.LogWarning(string.Format("[CarHide] DebugForceExit: state={0}.", this._state), this);
				return;
			}
			RunModeService.DisqualifyLeaderboardRun("A hiding action was forced with a debug control.");
			this.BeginExit();
		}

		// Token: 0x17001921 RID: 6433
		// (get) Token: 0x0600A387 RID: 41863 RVA: 0x0043F69A File Offset: 0x0043D89A
		public bool IsPlayerHiddenUnder
		{
			get
			{
				return this._state == CarHideUnderInteractable.HideState.Hidden;
			}
		}

		// Token: 0x0600A388 RID: 41864 RVA: 0x0043F6A5 File Offset: 0x0043D8A5
		public void ForceExit()
		{
			if (this._state != CarHideUnderInteractable.HideState.Hidden)
			{
				return;
			}
			this.BeginExit();
		}

		// Token: 0x0600A389 RID: 41865 RVA: 0x0043F6B8 File Offset: 0x0043D8B8
		public void ForceExitImmediate()
		{
			if (this._state == CarHideUnderInteractable.HideState.None)
			{
				return;
			}
			this._lifecycleGeneration++;
			if (this._sequence != null)
			{
				base.StopCoroutine(this._sequence);
			}
			this._sequence = null;
			this.FinishExit();
			this.ReleaseHideContext();
			ScreenFadeController instance = ScreenFadeController.Instance;
			if (instance == null)
			{
				return;
			}
			instance.SetClear();
		}

		// Token: 0x0600A38A RID: 41866 RVA: 0x0043F712 File Offset: 0x0043D912
		private void HandleValidate(GameObject player)
		{
			this._activePlayer = player;
			this._rangeGenericAction = ((player != null) ? player.GetComponentInChildren<vGenericAction>() : null);
			this.RefreshSelectedRange();
		}

		// Token: 0x0600A38B RID: 41867 RVA: 0x0043F739 File Offset: 0x0043D939
		private void HandleInvalidate(GameObject player)
		{
			this.RefreshSelectedRange();
		}

		// Token: 0x0600A38C RID: 41868 RVA: 0x0043F744 File Offset: 0x0043D944
		private void RefreshSelectedRange()
		{
			this._playerInRange = false;
			vTriggerGenericAction vTriggerGenericAction = ((this._rangeGenericAction != null) ? this._rangeGenericAction.triggerAction : null);
			if (vTriggerGenericAction == null || !vTriggerGenericAction.isActiveAndEnabled || !vTriggerGenericAction.CanDoAction || this.rangeTriggers == null)
			{
				return;
			}
			vTriggerGenericAction[] array = this.rangeTriggers;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == vTriggerGenericAction)
				{
					this._playerInRange = true;
					return;
				}
			}
		}

		// Token: 0x0600A38D RID: 41869 RVA: 0x0043F7C0 File Offset: 0x0043D9C0
		private bool CanStartHide()
		{
			if (GuidedRankGameplayPolicy.CarInteractionsBlocked || !this.CanStartNewRide)
			{
				return false;
			}
			this.ResolvePlayerRefs();
			if (this._controller == null || this._playerAnimator == null)
			{
				return false;
			}
			if (this._controller.customAction || this._controller.isJumping || this._controller.ragdolled)
			{
				return false;
			}
			if (!this._controller.isGrounded)
			{
				return false;
			}
			vGenericAction vGenericAction = ((this._actionAnimator != null) ? this._actionAnimator.GenericAction : null);
			return (!(vGenericAction != null) || !vGenericAction.doingAction) && (!(ScreenFadeController.Instance != null) || !ScreenFadeController.Instance.IsFading) && (!(this._vehicle != null) || (this.CurrentTravelSpeed <= Mathf.Min(this.maxHideStartSpeed, 0.35f) && (!(this._rideLimiter != null) || !this._rideLimiter.IsConfronting))) && (this.enterStart != null && this.underCarAnchor != null) && this.CanAcquireHideContext;
		}

		// Token: 0x17001922 RID: 6434
		// (get) Token: 0x0600A38E RID: 41870 RVA: 0x0043F8EC File Offset: 0x0043DAEC
		private string HideGateReason
		{
			get
			{
				return "CarHide#" + base.GetInstanceID().ToString();
			}
		}

		// Token: 0x17001923 RID: 6435
		// (get) Token: 0x0600A38F RID: 41871 RVA: 0x0043F911 File Offset: 0x0043DB11
		private bool CanAcquireHideContext
		{
			get
			{
				return !GuidedRankGameplayPolicy.CarInteractionsBlocked && FullBodyHideActivityContext.CanAcquire && !PlayerActivityGate.IsResourceOwned(PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.FullBodyAnimation | PlayerContextResource.Rider | PlayerContextResource.HeldItem);
			}
		}

		// Token: 0x0600A390 RID: 41872 RVA: 0x0043F930 File Offset: 0x0043DB30
		private bool TryAcquireHideContext()
		{
			if (GuidedRankGameplayPolicy.CarInteractionsBlocked || !this.CanStartNewRide)
			{
				return false;
			}
			if (this._hideLease != null)
			{
				if (this._hideLease.IsActive)
				{
					return true;
				}
				this._hideLease = null;
			}
			if (!this.CanAcquireHideContext)
			{
				return false;
			}
			if (!PlayerActivityGate.TryAcquire(this.HideGateReason, this, PlayerActivity.Interact, PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.FullBodyAnimation | PlayerContextResource.Rider | PlayerContextResource.HeldItem, PlayerActivity.AllPlayerActions, out this._hideLease, true, null))
			{
				return false;
			}
			InteractionCandidateRegistry.MarkInteract();
			if (this._firstPersonController == null)
			{
				Transform transform = this.ResolvePlayerTransform();
				if (transform != null)
				{
					this._firstPersonController = transform.GetComponentInChildren<FirstPersonController>(true);
				}
			}
			FirstPersonController firstPersonController = this._firstPersonController;
			if (firstPersonController != null)
			{
				firstPersonController.SuspendForInteraction(this);
			}
			return true;
		}

		// Token: 0x0600A391 RID: 41873 RVA: 0x0043F9DA File Offset: 0x0043DBDA
		private void ReleaseHideContext()
		{
			PlayerActivityLease hideLease = this._hideLease;
			this._hideLease = null;
			if (hideLease != null)
			{
				hideLease.Dispose();
			}
			FirstPersonController firstPersonController = this._firstPersonController;
			if (firstPersonController != null)
			{
				firstPersonController.ResumeAfterInteraction(this);
			}
			this._firstPersonController = null;
		}

		// Token: 0x0600A392 RID: 41874 RVA: 0x0043FA0D File Offset: 0x0043DC0D
		private bool SetHiddenFreeCameraAvailable(bool available)
		{
			return PlayerActivityGate.TryUpdate(this._hideLease, PlayerActivity.None, PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.FullBodyAnimation | PlayerContextResource.Rider | PlayerContextResource.HeldItem, available ? (PlayerActivity.Emote | PlayerActivity.Cover | PlayerActivity.Flash | PlayerActivity.RapidHUD | PlayerActivity.Interact | PlayerActivity.StanceSwitch | PlayerActivity.Undress | PlayerActivity.LingerieOpen | PlayerActivity.Inventory | PlayerActivity.Phone | PlayerActivity.Crouch | PlayerActivity.ArmsPose) : PlayerActivity.AllPlayerActions);
		}

		// Token: 0x0600A393 RID: 41875 RVA: 0x0043FA2C File Offset: 0x0043DC2C
		private bool BeginHide()
		{
			if (!this.TryAcquireHideContext())
			{
				BlockedActionFeedback.Show("Stop what you're doing first");
				return false;
			}
			if (this._roofCarrier != null && !this._roofCarrier.TryConsumeRide())
			{
				this.ReleaseHideContext();
				return false;
			}
			this._hideConsumed = true;
			this._lifecycleGeneration++;
			this._state = CarHideUnderInteractable.HideState.Entering;
			this._playerAnimator.ResetTrigger("CarHideEnding");
			Transform transform = this.ResolvePlayerTransform();
			if (transform != null)
			{
				this._preHideLocalPos = this.CarRoot.InverseTransformPoint(transform.position);
				this._preHideLocalRot = Quaternion.Inverse(this.CarRoot.rotation) * Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
				this._preHidePoseCaptured = true;
			}
			if (this._vehicle != null)
			{
				if (this._restartRoutine != null)
				{
					base.StopCoroutine(this._restartRoutine);
					this._restartRoutine = null;
				}
				this._holdingForPlayer = false;
				this._vehicle.SetInteractionBlocked(this, true);
				this._blockedByUs = true;
				this._vehicle.HasPassenger = true;
				this._vehicle.PassengerRoot = transform;
			}
			this.SnapPlayer(this.enterStart.position, Quaternion.Euler(0f, this.enterStart.eulerAngles.y, 0f));
			this.SetPlayerPhysicsDisabled(true);
			this.LockControls(true);
			if (this._actionAnimator != null)
			{
				this._actionAnimator.DisableEmotionLayers();
				this._actionAnimator.EnableRootMotionForAction();
				this._rootMotionPushed = true;
			}
			this.BeginForcedLayerWeights();
			if (this.promptUI != null && this.promptUI.gameObject.activeSelf)
			{
				this.promptUI.gameObject.SetActive(false);
			}
			this._playerAnimator.CrossFadeInFixedTime("Ciel_CarHide_Enter", 0.15f, 0);
			if (this._sequence != null)
			{
				base.StopCoroutine(this._sequence);
			}
			this._sequence = base.StartCoroutine(this.EnterRoutine());
			if (this.debugLogs)
			{
				Debug.Log("[CarHide] F pressed → crawl-in started from EnterStart.", this);
			}
			return true;
		}

		// Token: 0x0600A394 RID: 41876 RVA: 0x0043FC4B File Offset: 0x0043DE4B
		private IEnumerator EnterRoutine()
		{
			float t0 = Time.time;
			while (Time.time - t0 < 2f && !this._playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Ciel_CarHide_Enter"))
			{
				yield return null;
			}
			t0 = Time.time;
			while (Time.time - t0 < 10f)
			{
				AnimatorStateInfo currentAnimatorStateInfo = this._playerAnimator.GetCurrentAnimatorStateInfo(0);
				if (!currentAnimatorStateInfo.IsName("Ciel_CarHide_Enter") || (1f - Mathf.Clamp01(currentAnimatorStateInfo.normalizedTime)) * currentAnimatorStateInfo.length <= this.enterFadeLead)
				{
					break;
				}
				yield return null;
			}
			bool middleDone = false;
			this.Fade(delegate
			{
				this.SnapPlayer(this.underCarAnchor.position, this.underCarAnchor.rotation);
				this._playerAnimator.Play("Ciel_CarHide_Idle", 0, 0f);
				this.SetHiddenCamera(true);
				this.SetSupineBoobs(true);
				this.AttachPlayerToCar(true);
				middleDone = true;
			});
			while (!middleDone)
			{
				yield return null;
			}
			this._state = CarHideUnderInteractable.HideState.Hidden;
			this._hiddenSince = Time.time;
			CarRideLimiter rideLimiter = this._rideLimiter;
			if (rideLimiter != null)
			{
				rideLimiter.NotifyHiddenRider(this.ResolvePlayerTransform());
			}
			ScoreGameplaySignals.NotifyHidingStarted(ScoreHidingSpotKind.UnderCar, this);
			PlayerStealthBridge.SetHiddenSource(this, true);
			this.ShowHud();
			if (this._vehicle != null && this._blockedByUs)
			{
				yield return new WaitForSeconds(0.75f);
				this._vehicle.SetInteractionBlocked(this, false);
				this._blockedByUs = false;
			}
			while (ScreenFadeController.Instance != null && ScreenFadeController.Instance.IsFading)
			{
				yield return null;
			}
			this.SetHiddenFreeCameraAvailable(true);
			this._sequence = null;
			if (this.debugLogs)
			{
				Debug.Log("[CarHide] Hidden under the car (supine, clinging).", this);
			}
			yield break;
		}

		// Token: 0x0600A395 RID: 41877 RVA: 0x0043FC5C File Offset: 0x0043DE5C
		private void BeginExit()
		{
			if (FlyCamera.IsActive)
			{
				return;
			}
			this.SetHiddenFreeCameraAvailable(false);
			this._state = CarHideUnderInteractable.HideState.Exiting;
			this.HideHud();
			if (this._vehicle != null)
			{
				this._vehicle.SetInteractionBlocked(this, true);
				this._blockedByUs = true;
			}
			if (this._sequence != null)
			{
				base.StopCoroutine(this._sequence);
			}
			this._sequence = base.StartCoroutine(this.ExitRoutine());
			if (this.debugLogs)
			{
				Debug.Log("[CarHide] X pressed → stopping car, then crawling out.", this);
			}
		}

		// Token: 0x0600A396 RID: 41878 RVA: 0x0043FCE1 File Offset: 0x0043DEE1
		private IEnumerator ExitRoutine()
		{
			float tStop = Time.time;
			while (this._vehicle != null && Time.time - tStop < 8f && this.CurrentTravelSpeed > 0.35f)
			{
				yield return null;
			}
			if (this._vehicle != null && this.CurrentTravelSpeed > 0.35f)
			{
				this._vehicle.SetInitialSpeed(0f);
			}
			this._playerAnimator.Play("Ciel_CarHide_Exit", 0, 0f);
			float t0 = Time.time;
			while (Time.time - t0 < 2f && !this._playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Ciel_CarHide_Exit"))
			{
				yield return null;
			}
			t0 = Time.time;
			while (Time.time - t0 < 10f)
			{
				AnimatorStateInfo currentAnimatorStateInfo = this._playerAnimator.GetCurrentAnimatorStateInfo(0);
				if (!currentAnimatorStateInfo.IsName("Ciel_CarHide_Exit") || Mathf.Clamp01(currentAnimatorStateInfo.normalizedTime) * currentAnimatorStateInfo.length >= this.exitFadeAfterSeconds)
				{
					break;
				}
				yield return null;
			}
			bool middleDone = false;
			this.Fade(delegate
			{
				this.FinishExit();
				middleDone = true;
			});
			while (!middleDone)
			{
				yield return null;
			}
			while (ScreenFadeController.Instance != null && ScreenFadeController.Instance.IsFading)
			{
				yield return null;
			}
			this.ReleaseHideContext();
			this._sequence = null;
			yield break;
		}

		// Token: 0x0600A397 RID: 41879 RVA: 0x0043FCF0 File Offset: 0x0043DEF0
		private void FinishExit()
		{
			this.HideHud();
			PlayerStealthBridge.SetHiddenSource(this, false);
			this.SetHiddenCamera(false);
			this.SetSupineBoobs(false);
			this.AttachPlayerToCar(false);
			Vector3 vector;
			Quaternion quaternion;
			if (this._preHidePoseCaptured)
			{
				vector = this.CarRoot.TransformPoint(this._preHideLocalPos);
				quaternion = Quaternion.Euler(0f, (this.CarRoot.rotation * this._preHideLocalRot).eulerAngles.y, 0f);
			}
			else
			{
				vector = this.enterStart.position;
				quaternion = Quaternion.Euler(0f, this.enterStart.eulerAngles.y + 180f, 0f);
			}
			this.SnapPlayer(vector, quaternion);
			this.RestorePlayerState();
			if (this._vehicle != null)
			{
				if (this._restartRoutine != null)
				{
					base.StopCoroutine(this._restartRoutine);
				}
				this._restartRoutine = base.StartCoroutine(this.ReleaseCarAfterDelay());
			}
			int num = CarHideUnderInteractable.ResolveLocomotionStateHash(this._playerAnimator);
			if (num != 0)
			{
				this._playerAnimator.Play(num, 0, 0f);
			}
			this._state = CarHideUnderInteractable.HideState.None;
			if (this.debugLogs)
			{
				Debug.Log("[CarHide] Exit complete: control returned to locomotion.", this);
			}
		}

		// Token: 0x0600A398 RID: 41880 RVA: 0x0043FE20 File Offset: 0x0043E020
		private void SetHiddenCamera(bool on)
		{
			if (on == this._cameraStatePushed)
			{
				return;
			}
			if (!on && this._activeHideCamera != null)
			{
				FlyCamera.StopForGameplayCamera(this._activeHideCamera);
			}
			this._cameraStatePushed = on;
			if (this.HasHideCameras())
			{
				if (on)
				{
					this._disabledMainCamera = Camera.main;
					if (this._disabledMainCamera != null)
					{
						this._disabledMainCamera.enabled = false;
					}
					this._hideCameraIndex = 0;
					this.ActivateHideCamera(this._hideCameraIndex);
					return;
				}
				this.DeactivateHideCamera();
				if (this._disabledMainCamera != null)
				{
					this._disabledMainCamera.enabled = true;
				}
				this._disabledMainCamera = null;
				return;
			}
			else
			{
				if (string.IsNullOrEmpty(this.hiddenCameraState) || this._tpInput == null)
				{
					return;
				}
				if (on)
				{
					this._tpInput.ChangeCameraState(this.hiddenCameraState, true);
					return;
				}
				this._tpInput.ResetCameraState();
				return;
			}
		}

		// Token: 0x0600A399 RID: 41881 RVA: 0x0043FF04 File Offset: 0x0043E104
		private bool HasHideCameras()
		{
			if (this.hideCameras == null)
			{
				return false;
			}
			for (int i = 0; i < this.hideCameras.Length; i++)
			{
				if (this.hideCameras[i] != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600A39A RID: 41882 RVA: 0x0043FF44 File Offset: 0x0043E144
		private void ActivateHideCamera(int index)
		{
			this.DeactivateHideCamera();
			if (this.hideCameras == null || this.hideCameras.Length == 0)
			{
				return;
			}
			for (int i = 0; i < this.hideCameras.Length; i++)
			{
				int num = (index + i) % this.hideCameras.Length;
				if (!(this.hideCameras[num] == null))
				{
					this._hideCameraIndex = num;
					this._activeHideCamera = this.hideCameras[num];
					this._activeHideCamBasePos = this._activeHideCamera.transform.localPosition;
					this._activeHideCamBaseRot = this._activeHideCamera.transform.localRotation;
					this._lookYaw = 0f;
					this._lookPitch = 0f;
					this._orbitActive = this._activeHideCamera.name == this.orbitCameraName;
					if (this._orbitActive)
					{
						Vector3 vector = this._activeHideCamera.transform.position - this.OrbitTarget();
						this._orbitDistance = Mathf.Max(vector.magnitude, 1f);
						this._orbitYaw = Mathf.Atan2(vector.x, vector.z) * 57.29578f;
						this._orbitPitch = Mathf.Clamp(Mathf.Asin(Mathf.Clamp(vector.y / this._orbitDistance, -1f, 1f)) * 57.29578f, this.orbitPitchMin, this.orbitPitchMax);
					}
					this._activeHideCamera.gameObject.SetActive(true);
					this._activeHideCamera.enabled = true;
					return;
				}
			}
		}

		// Token: 0x0600A39B RID: 41883 RVA: 0x004400CC File Offset: 0x0043E2CC
		private Vector3 OrbitTarget()
		{
			return this.CarRoot.position + Vector3.up * this.orbitTargetHeight;
		}

		// Token: 0x0600A39C RID: 41884 RVA: 0x004400F0 File Offset: 0x0043E2F0
		private void DeactivateHideCamera()
		{
			if (this._activeHideCamera == null)
			{
				return;
			}
			this._activeHideCamera.transform.localPosition = this._activeHideCamBasePos;
			this._activeHideCamera.transform.localRotation = this._activeHideCamBaseRot;
			this._activeHideCamera.gameObject.SetActive(false);
			this._activeHideCamera = null;
			this._orbitActive = false;
		}

		// Token: 0x0600A39D RID: 41885 RVA: 0x00440157 File Offset: 0x0043E357
		private void CycleHideCamera()
		{
			if (!this.HasHideCameras() || this._activeHideCamera == null)
			{
				return;
			}
			this.ActivateHideCamera((this._hideCameraIndex + 1) % this.hideCameras.Length);
		}

		// Token: 0x0600A39E RID: 41886 RVA: 0x00440188 File Offset: 0x0043E388
		private void SetSupineBoobs(bool on)
		{
			if (!this.swapBoobPhysics)
			{
				return;
			}
			Transform transform = this.ResolvePlayerTransform();
			if (transform == null)
			{
				return;
			}
			Transform[] componentsInChildren = transform.root.GetComponentsInChildren<Transform>(true);
			foreach (Transform transform2 in componentsInChildren)
			{
				if (CarHideUnderInteractable.MatchesName(this.normalClothNames, transform2.name))
				{
					transform2.gameObject.SetActive(!on);
				}
			}
			foreach (Transform transform3 in componentsInChildren)
			{
				if (CarHideUnderInteractable.MatchesName(this.supineClothNames, transform3.name))
				{
					MagicaCloth component = transform3.GetComponent<MagicaCloth>();
					if (component == null || component.SerializeData == null)
					{
						transform3.gameObject.SetActive(on);
					}
					else
					{
						if (on)
						{
							CarHideUnderInteractable.EnsureBoneClothTips(component);
						}
						transform3.gameObject.SetActive(on);
						if (on)
						{
							float num = (transform3.name.Contains("R") ? 1f : (-1f));
							if (this.invertLateralBias)
							{
								num = -num;
							}
							Vector3 normalized = (Vector3.down + transform.right * (num * this.boobLateralBias)).normalized;
							component.SerializeData.gravityDirection = new float3(normalized.x, normalized.y, normalized.z);
							component.SetParameterChange();
						}
					}
				}
			}
		}

		// Token: 0x0600A39F RID: 41887 RVA: 0x004402F8 File Offset: 0x0043E4F8
		private static void EnsureBoneClothTips(MagicaCloth cloth)
		{
			List<Transform> rootBones = cloth.SerializeData.rootBones;
			if (rootBones == null)
			{
				return;
			}
			foreach (Transform transform in rootBones)
			{
				if (!(transform == null) && transform.childCount <= 0)
				{
					Vector3 vector = ((transform.parent != null) ? (transform.position - transform.parent.position).normalized : transform.forward);
					Transform transform2 = new GameObject(transform.name + "_CarHideTip").transform;
					transform2.SetParent(transform, false);
					transform2.position = transform.position + vector * 0.09f;
				}
			}
		}

		// Token: 0x0600A3A0 RID: 41888 RVA: 0x004403DC File Offset: 0x0043E5DC
		private static bool MatchesName(string[] names, string candidate)
		{
			if (names == null)
			{
				return false;
			}
			for (int i = 0; i < names.Length; i++)
			{
				if (names[i] == candidate)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600A3A1 RID: 41889 RVA: 0x0044040C File Offset: 0x0043E60C
		private void LateUpdate()
		{
			if (this._layersForced && this._playerAnimator != null)
			{
				this.ApplyForcedLayerWeights();
			}
			if (this._activeHideCamera == null || FlyCamera.IsActive)
			{
				return;
			}
			Vector2 vector = new Vector2(GameplayInput.GetAxis("Mouse X"), GameplayInput.GetAxis("Mouse Y"));
			if (!ControllerInput.IsActive)
			{
				vector = GameSettingsService.ApplyMouseLookInversion(vector);
			}
			if (this._orbitActive)
			{
				this._orbitYaw += vector.x * this.orbitSensitivity;
				this._orbitPitch = Mathf.Clamp(this._orbitPitch + vector.y * this.orbitSensitivity, this.orbitPitchMin, this.orbitPitchMax);
				Vector3 vector2 = this.OrbitTarget();
				Quaternion quaternion = Quaternion.Euler(this._orbitPitch, this._orbitYaw, 0f);
				Vector3 vector3 = vector2 + quaternion * (Vector3.back * this._orbitDistance);
				this._activeHideCamera.transform.SetPositionAndRotation(vector3, Quaternion.LookRotation((vector2 - vector3).normalized, Vector3.up));
				return;
			}
			this._lookYaw = Mathf.Clamp(this._lookYaw + vector.x * this.lookSensitivity, -this.lookYawRange, this.lookYawRange);
			this._lookPitch = Mathf.Clamp(this._lookPitch + vector.y * this.lookSensitivity, -this.lookPitchRange, this.lookPitchRange);
			this._activeHideCamera.transform.localRotation = this._activeHideCamBaseRot * Quaternion.Euler(-this._lookPitch, this._lookYaw, 0f);
		}

		// Token: 0x0600A3A2 RID: 41890 RVA: 0x004405B8 File Offset: 0x0043E7B8
		private void RestorePlayerState()
		{
			this.SetPlayerPhysicsDisabled(false);
			this.LockControls(false);
			this.EndForcedLayerWeights();
			if (this._rootMotionPushed && this._actionAnimator != null)
			{
				this._actionAnimator.RestoreRootMotionForAction();
				this._actionAnimator.RestoreOverrideLayer();
			}
			this._rootMotionPushed = false;
		}

		// Token: 0x0600A3A3 RID: 41891 RVA: 0x0044060C File Offset: 0x0043E80C
		private void HandleWorldOriginShift(Vector3 delta)
		{
			this._lastCarPos += delta;
		}

		// Token: 0x0600A3A4 RID: 41892 RVA: 0x00440620 File Offset: 0x0043E820
		private void UpdateProximityHold()
		{
			if (GuidedRankGameplayPolicy.CarInteractionsBlocked || !this.CanStartNewRide)
			{
				this.ReleaseProximityHold();
				return;
			}
			if (this._vehicle == null || this.holdWhilePlayerNearRadius <= 0f)
			{
				return;
			}
			if (this._vehicle.HasPassenger)
			{
				this.ReleaseProximityHold();
				return;
			}
			Transform player = PlayerRegistry.Player;
			bool flag = false;
			if (player != null)
			{
				Vector3 vector = player.position - this.CarRoot.position;
				vector.y = 0f;
				flag = vector.sqrMagnitude <= this.holdWhilePlayerNearRadius * this.holdWhilePlayerNearRadius;
			}
			if (!this._holdingForPlayer && flag && this.CurrentTravelSpeed <= this.maxHideStartSpeed)
			{
				this._vehicle.SetInteractionBlocked(this, true);
				this._holdingForPlayer = true;
				return;
			}
			if (this._holdingForPlayer && !flag)
			{
				if (!this._blockedByUs)
				{
					this._vehicle.SetInteractionBlocked(this, false);
				}
				this._holdingForPlayer = false;
			}
		}

		// Token: 0x0600A3A5 RID: 41893 RVA: 0x00440718 File Offset: 0x0043E918
		private void ReleaseProximityHold()
		{
			if (!this._holdingForPlayer)
			{
				return;
			}
			if (this._vehicle != null && !this._blockedByUs)
			{
				this._vehicle.SetInteractionBlocked(this, false);
			}
			this._holdingForPlayer = false;
		}

		// Token: 0x0600A3A6 RID: 41894 RVA: 0x00440750 File Offset: 0x0043E950
		private void AttachPlayerToCar(bool attach)
		{
			Transform transform = this.ResolvePlayerTransform();
			if (attach == this._playerParented)
			{
				return;
			}
			if (!attach)
			{
				Action<VehicleAI, Transform> onPlayerDismounted = CarHideUnderInteractable.OnPlayerDismounted;
				if (onPlayerDismounted != null)
				{
					onPlayerDismounted(this._vehicle, transform);
				}
			}
			if (transform == null)
			{
				this._playerParented = false;
				return;
			}
			if (attach)
			{
				this._originalPlayerParent = transform.parent;
				transform.SetParent(this.CarRoot, true);
			}
			else
			{
				transform.SetParent(this._originalPlayerParent, true);
				this._originalPlayerParent = null;
			}
			this._playerParented = attach;
			if (attach)
			{
				Action<VehicleAI, Transform> onPlayerMounted = CarHideUnderInteractable.OnPlayerMounted;
				if (onPlayerMounted == null)
				{
					return;
				}
				onPlayerMounted(this._vehicle, transform);
			}
		}

		// Token: 0x1400011C RID: 284
		// (add) Token: 0x0600A3A7 RID: 41895 RVA: 0x004407EC File Offset: 0x0043E9EC
		// (remove) Token: 0x0600A3A8 RID: 41896 RVA: 0x00440820 File Offset: 0x0043EA20
		public static event Action<VehicleAI, Transform> OnPlayerMounted;

		// Token: 0x1400011D RID: 285
		// (add) Token: 0x0600A3A9 RID: 41897 RVA: 0x00440854 File Offset: 0x0043EA54
		// (remove) Token: 0x0600A3AA RID: 41898 RVA: 0x00440888 File Offset: 0x0043EA88
		public static event Action<VehicleAI, Transform> OnPlayerDismounted;

		// Token: 0x0600A3AB RID: 41899 RVA: 0x004408BB File Offset: 0x0043EABB
		private IEnumerator ReleaseCarAfterDelay()
		{
			yield return new WaitForSeconds(this.restartDelay);
			if (this._vehicle != null)
			{
				if (!this._holdingForPlayer)
				{
					this._vehicle.SetInteractionBlocked(this, false);
				}
				if (this._rideLimiter == null || !this._rideLimiter.IsConfronting)
				{
					this._vehicle.HasPassenger = this._rideLimiter != null && this._rideLimiter.HasRoofRider;
					if (!this._vehicle.HasPassenger)
					{
						this._vehicle.PassengerRoot = null;
					}
				}
			}
			this._blockedByUs = false;
			this._restartRoutine = null;
			yield break;
		}

		// Token: 0x0600A3AC RID: 41900 RVA: 0x004408CC File Offset: 0x0043EACC
		private void ReleaseTrafficOwnership()
		{
			if (this._vehicle != null)
			{
				this._vehicle.SetInteractionBlocked(this, false);
				if (this._rideLimiter == null || !this._rideLimiter.IsConfronting)
				{
					this._vehicle.HasPassenger = this._rideLimiter != null && this._rideLimiter.HasRoofRider;
					if (!this._vehicle.HasPassenger)
					{
						this._vehicle.PassengerRoot = null;
					}
				}
			}
			this._blockedByUs = false;
			this._holdingForPlayer = false;
		}

		// Token: 0x0600A3AD RID: 41901 RVA: 0x00440960 File Offset: 0x0043EB60
		private void BeginForcedLayerWeights()
		{
			if (this._playerAnimator == null || this._layersForced)
			{
				return;
			}
			this._savedLayerWeights = new float[this._playerAnimator.layerCount];
			for (int i = 1; i < this._playerAnimator.layerCount; i++)
			{
				this._savedLayerWeights[i] = this._playerAnimator.GetLayerWeight(i);
			}
			this._layersForced = true;
			this.ApplyForcedLayerWeights();
		}

		// Token: 0x0600A3AE RID: 41902 RVA: 0x004409D4 File Offset: 0x0043EBD4
		private void ApplyForcedLayerWeights()
		{
			for (int i = 1; i < this._playerAnimator.layerCount; i++)
			{
				if (!(this._playerAnimator.GetLayerName(i) == "Face"))
				{
					this._playerAnimator.SetLayerWeight(i, 0f);
				}
			}
		}

		// Token: 0x0600A3AF RID: 41903 RVA: 0x00440A20 File Offset: 0x0043EC20
		private void EndForcedLayerWeights()
		{
			if (!this._layersForced)
			{
				return;
			}
			this._layersForced = false;
			if (this._playerAnimator == null || this._savedLayerWeights == null)
			{
				return;
			}
			int num = Mathf.Min(this._playerAnimator.layerCount, this._savedLayerWeights.Length);
			for (int i = 1; i < num; i++)
			{
				if (!(this._playerAnimator.GetLayerName(i) == "Face"))
				{
					this._playerAnimator.SetLayerWeight(i, this._savedLayerWeights[i]);
				}
			}
			this._savedLayerWeights = null;
		}

		// Token: 0x0600A3B0 RID: 41904 RVA: 0x00440AAC File Offset: 0x0043ECAC
		private void ForceAbort()
		{
			this._lifecycleGeneration++;
			Debug.LogWarning(string.Format("[CarHide] ForceAbort! state={0} activeInHierarchy={1} ", this._state, base.gameObject.activeInHierarchy) + string.Format("enabled={0} carActive={1}", base.enabled, this.CarRoot.gameObject.activeInHierarchy), this);
			if (this._sequence != null)
			{
				base.StopCoroutine(this._sequence);
				this._sequence = null;
			}
			if (this._restartRoutine != null)
			{
				base.StopCoroutine(this._restartRoutine);
				this._restartRoutine = null;
			}
			PlayerStealthBridge.SetHiddenSource(this, false);
			this.SetHiddenCamera(false);
			this.SetSupineBoobs(false);
			this.AttachPlayerToCar(false);
			this.HideHud();
			if (this._controller != null && this.enterStart != null)
			{
				Vector3 vector = (this._preHidePoseCaptured ? this.CarRoot.TransformPoint(this._preHideLocalPos) : this.enterStart.position);
				Quaternion quaternion = (this._preHidePoseCaptured ? Quaternion.Euler(0f, (this.CarRoot.rotation * this._preHideLocalRot).eulerAngles.y, 0f) : Quaternion.Euler(0f, this.enterStart.eulerAngles.y, 0f));
				this.SnapPlayer(vector, quaternion);
			}
			this.RestorePlayerState();
			this.ReleaseTrafficOwnership();
			if (this._playerAnimator != null)
			{
				int num = CarHideUnderInteractable.ResolveLocomotionStateHash(this._playerAnimator);
				if (num != 0)
				{
					this._playerAnimator.Play(num, 0, 0f);
				}
			}
			this._state = CarHideUnderInteractable.HideState.None;
			this.ReleaseHideContext();
		}

		// Token: 0x0600A3B1 RID: 41905 RVA: 0x00440C70 File Offset: 0x0043EE70
		private void Fade(Action middle)
		{
			int generation = this._lifecycleGeneration;
			Action action = delegate
			{
				if (generation != this._lifecycleGeneration || this._state == CarHideUnderInteractable.HideState.None || !this.isActiveAndEnabled)
				{
					return;
				}
				Action middle2 = middle;
				if (middle2 == null)
				{
					return;
				}
				middle2();
			};
			if (ScreenFadeController.Instance != null)
			{
				ScreenFadeController.Instance.FadeOutIn(action, this.fadeDuration, this.fadeDuration, this.fadeHold, null);
				return;
			}
			action();
		}

		// Token: 0x0600A3B2 RID: 41906 RVA: 0x00440CDC File Offset: 0x0043EEDC
		private void SetPlayerPhysicsDisabled(bool disabled)
		{
			if (this._controller == null || disabled == this._physicsPushed)
			{
				return;
			}
			if (disabled)
			{
				this._controller._rigidbody.useGravity = false;
				if (!this._controller._rigidbody.isKinematic)
				{
					this._controller._rigidbody.velocity = Vector3.zero;
				}
				this._controller._rigidbody.isKinematic = true;
				this._controller._capsuleCollider.isTrigger = true;
			}
			else
			{
				this._controller._rigidbody.useGravity = true;
				this._controller._rigidbody.isKinematic = false;
				this._controller._capsuleCollider.isTrigger = false;
			}
			this._physicsPushed = disabled;
		}

		// Token: 0x0600A3B3 RID: 41907 RVA: 0x00440D9A File Offset: 0x0043EF9A
		private void LockControls(bool locked)
		{
			if (this._controller == null || locked == this._controlsLocked)
			{
				return;
			}
			this._controller.lockMovement = locked;
			this._controller.lockRotation = locked;
			this._controlsLocked = locked;
		}

		// Token: 0x0600A3B4 RID: 41908 RVA: 0x00440DD4 File Offset: 0x0043EFD4
		private void SnapPlayer(Vector3 pos, Quaternion rot)
		{
			Transform transform = this.ResolvePlayerTransform();
			if (transform == null)
			{
				return;
			}
			transform.SetPositionAndRotation(pos, rot);
			Rigidbody component = transform.GetComponent<Rigidbody>();
			if (component != null)
			{
				component.position = pos;
				component.rotation = rot;
				if (!component.isKinematic)
				{
					component.velocity = Vector3.zero;
					component.angularVelocity = Vector3.zero;
				}
			}
		}

		// Token: 0x0600A3B5 RID: 41909 RVA: 0x00440E36 File Offset: 0x0043F036
		private Transform ResolvePlayerTransform()
		{
			if (PlayerRegistry.Player != null)
			{
				return PlayerRegistry.Player;
			}
			if (!(this._activePlayer != null))
			{
				return null;
			}
			return this._activePlayer.transform;
		}

		// Token: 0x0600A3B6 RID: 41910 RVA: 0x00440E68 File Offset: 0x0043F068
		private void ResolvePlayerRefs()
		{
			GameObject gameObject = ((PlayerRegistry.Player != null) ? PlayerRegistry.Player.gameObject : this._activePlayer);
			if (gameObject != null)
			{
				this._actionAnimator = gameObject.GetComponentInChildren<PlayerActionAnimator>();
				this._playerAnimator = gameObject.GetComponentInChildren<Animator>();
				this._controller = gameObject.GetComponentInChildren<vThirdPersonController>();
				this._tpInput = gameObject.GetComponentInChildren<vThirdPersonInput>();
			}
			if (this._actionAnimator == null)
			{
				this._actionAnimator = Object.FindObjectOfType<PlayerActionAnimator>();
			}
			if (this._actionAnimator != null && this._actionAnimator.GenericAction != null)
			{
				if (this._playerAnimator == null)
				{
					this._playerAnimator = this._actionAnimator.GenericAction.GetComponentInChildren<Animator>();
				}
				if (this._controller == null)
				{
					this._controller = this._actionAnimator.GenericAction.GetComponent<vThirdPersonController>();
				}
				if (this._tpInput == null)
				{
					this._tpInput = this._actionAnimator.GenericAction.GetComponent<vThirdPersonInput>();
				}
			}
		}

		// Token: 0x0600A3B7 RID: 41911 RVA: 0x00440F74 File Offset: 0x0043F174
		private static int ResolveLocomotionStateHash(Animator animator)
		{
			string[] array = new string[] { "Free Movement", "Locomotion", "Idle", "Grounded" };
			for (int i = 0; i < array.Length; i++)
			{
				int num = Animator.StringToHash(array[i]);
				if (animator.HasState(0, num))
				{
					return num;
				}
			}
			return 0;
		}

		// Token: 0x0600A3B8 RID: 41912 RVA: 0x00440FCC File Offset: 0x0043F1CC
		private void ShowHud()
		{
			if (this.hiddenHudConfig != null && InteractionHUDController.Instance != null)
			{
				InteractionHUDController.Instance.ShowSupplemental(this, this.hiddenHudConfig);
			}
		}

		// Token: 0x0600A3B9 RID: 41913 RVA: 0x00440FFA File Offset: 0x0043F1FA
		private void HideHud()
		{
			if (InteractionHUDController.Instance != null)
			{
				InteractionHUDController.Instance.HideSupplemental(this);
			}
		}

		// Token: 0x0400A741 RID: 42817
		private const string EnterStateName = "Ciel_CarHide_Enter";

		// Token: 0x0400A742 RID: 42818
		private const string IdleStateName = "Ciel_CarHide_Idle";

		// Token: 0x0400A743 RID: 42819
		private const string ExitStateName = "Ciel_CarHide_Exit";

		// Token: 0x0400A744 RID: 42820
		private const string EndingTrigger = "CarHideEnding";

		// Token: 0x0400A745 RID: 42821
		[Header("References")]
		[Tooltip("Trigger zones used ONLY for range detection (when can F be pressed). Auto-resolved from siblings at Awake — these are the same 4 climb triggers.")]
		[SerializeField]
		private vTriggerGenericAction[] rangeTriggers;

		// Token: 0x0400A746 RID: 42822
		[Tooltip("Shared world prompt (the one ClimbUpInteractable drives). Only needed so we can hide it while the hide action runs. Auto-resolved from the car hierarchy.")]
		[SerializeField]
		private InteractablePromptUI promptUI;

		// Token: 0x0400A747 RID: 42823
		[Tooltip("Anchor BEHIND the car, facing it: Ciel is snapped here at the F press, the crawl-in plays from here. Auto-resolved by name 'CarHide_EnterStart'.")]
		[SerializeField]
		private Transform enterStart;

		// Token: 0x0400A748 RID: 42824
		[Tooltip("Anchor at the car's centre, under the body, ROTATED 180° roll: the clinging pose while hidden. Auto-resolved by name 'CarHide_UnderCar'.")]
		[SerializeField]
		private Transform underCarAnchor;

		// Token: 0x0400A749 RID: 42825
		[Header("UI Config")]
		[Tooltip("Config for the shared right-side HUD shown WHILE HIDDEN (needs the 'Esci' / X usable action).")]
		[SerializeField]
		private InteractionConfigSO hiddenHudConfig;

		// Token: 0x0400A74A RID: 42826
		[Header("Input")]
		[Tooltip("Pressed (while in range of a climb trigger) to crawl under the car.")]
		[SerializeField]
		private GenericInput hideInput = new GenericInput("F", "X", "X");

		// Token: 0x0400A74B RID: 42827
		[Tooltip("Pressed while hidden to crawl back out.")]
		[SerializeField]
		private GenericInput exitInput = new GenericInput("X", "B", "B");

		// Token: 0x0400A74C RID: 42828
		[Header("Timing")]
		[Tooltip("Seconds BEFORE the crawl-in clip ends at which the black fade starts (the snap under the car happens while the screen is dark).")]
		[SerializeField]
		private float enterFadeLead = 0.25f;

		// Token: 0x0400A74D RID: 42829
		[Tooltip("Seconds AFTER the crawl-out clip starts at which the black fade begins. The crawl-out plays SUPINE in place under the car (mirrored by the 180° roll), so the fade must cut away before the clip's stand-up portion — flipped, that part would push her head-first into the asphalt. 3.18s = designer-tuned on the MCU_am_HideProne_Trans_Ready clip.")]
		[SerializeField]
		private float exitFadeAfterSeconds = 3.18f;

		// Token: 0x0400A74E RID: 42830
		[Tooltip("Fade out / fade in duration used for both transitions.")]
		[SerializeField]
		private float fadeDuration = 0.25f;

		// Token: 0x0400A74F RID: 42831
		[Tooltip("Seconds the screen stays black while snapping.")]
		[SerializeField]
		private float fadeHold = 0.15f;

		// Token: 0x0400A750 RID: 42832
		[Header("Camera")]
		[Tooltip("Dedicated camera PRESETS parked under the car, framing Ciel while she's hidden (the main camera is disabled for the duration; the switch key cycles them). Created by the setup tool as 'CarHideCamera', 'CarHideCamera2', … children of the car — SELECT AND MOVE THEM IN PLAY MODE to tune the framing, then bake the values. Auto-resolved by name. When empty, falls back to the camera state below.")]
		[SerializeField]
		private Camera[] hideCameras;

		// Token: 0x0400A751 RID: 42833
		[Tooltip("Pressed while hidden to cycle through the camera presets (F is free under the car — outside it starts the hide).")]
		[SerializeField]
		private GenericInput switchCameraInput = new GenericInput("F", "Y", "Y");

		// Token: 0x0400A752 RID: 42834
		[Tooltip("Mouse look range (degrees, ±) around the active preset's authored rotation — left/right.")]
		[SerializeField]
		private float lookYawRange = 10f;

		// Token: 0x0400A753 RID: 42835
		[Tooltip("Mouse look range (degrees, ±) around the active preset's authored rotation — up/down.")]
		[SerializeField]
		private float lookPitchRange = 8f;

		// Token: 0x0400A754 RID: 42836
		[Tooltip("Degrees per mouse-axis unit.")]
		[SerializeField]
		private float lookSensitivity = 2f;

		// Token: 0x0400A755 RID: 42837
		[Tooltip("The preset with this name behaves like a classic THIRD PERSON camera targeting the CAR: the mouse orbits it 360° around the vehicle (pitch clamped below). Orbit distance = wherever the preset is parked, so tune it by moving the camera.")]
		[SerializeField]
		private string orbitCameraName = "CarHideCamera3";

		// Token: 0x0400A756 RID: 42838
		[SerializeField]
		private float orbitSensitivity = 3f;

		// Token: 0x0400A757 RID: 42839
		[SerializeField]
		private float orbitPitchMin = 4f;

		// Token: 0x0400A758 RID: 42840
		[SerializeField]
		private float orbitPitchMax = 60f;

		// Token: 0x0400A759 RID: 42841
		[Tooltip("Orbit target height above the car root (roughly the body centre).")]
		[SerializeField]
		private float orbitTargetHeight = 0.7f;

		// Token: 0x0400A75A RID: 42842
		[Tooltip("FALLBACK vThirdPersonCamera state used while hidden when no hideCameras are assigned (very close orbit so the camera slips under the car with her). Added by Tools/CarHide/Setup CarHide Camera State. Empty = keep the default state.")]
		[SerializeField]
		private string hiddenCameraState = "CarHide";

		// Token: 0x0400A75B RID: 42843
		[Header("Moving car (traffic)")]
		[Tooltip("Max car speed (m/s) at which F is accepted — same gating idea as CarClimbBridge. Ignored for cars without a VehicleAI.")]
		[SerializeField]
		private float maxHideStartSpeed = 1.5f;

		// Token: 0x0400A75C RID: 42844
		[Tooltip("Seconds the car stays blocked after she crawls out, before driving off again.")]
		[SerializeField]
		private float restartDelay = 2f;

		// Token: 0x0400A75D RID: 42845
		[Tooltip("While the player stands within this range of a stopped/slow car (ANY side — the built-in courtesy stop resumes after ~2s and drove off before she could hook), the car is HELD blocked so there's always time to press F. Released the moment the player walks away. 0 = disabled.")]
		[SerializeField]
		private float holdWhilePlayerNearRadius = 4.5f;

		// Token: 0x0400A75E RID: 42846
		[Header("Supine boob physics (MagicaCloth swap)")]
		[Tooltip("DISABLED for now (designer call 2026-07-16): the classic presets stay active under the car too. The swap machinery remains for a future pass — it needs the _CarHideSupine variants from Tools/CarHide/Setup Supine Boob Physics.")]
		[SerializeField]
		private bool swapBoobPhysics;

		// Token: 0x0400A75F RID: 42847
		[Tooltip("Names of the regular cloth GameObjects on the player (disabled while hidden).")]
		[SerializeField]
		private string[] normalClothNames = new string[] { "BoobR", "BoobL" };

		// Token: 0x0400A760 RID: 42848
		[Tooltip("Names of the supine variants (enabled while hidden). Index-matched by side via the trailing R/L in the name.")]
		[SerializeField]
		private string[] supineClothNames = new string[] { "BoobR_CarHideSupine", "BoobL_CarHideSupine" };

		// Token: 0x0400A761 RID: 42849
		[Tooltip("Sideways lean mixed into each supine cloth's gravity direction so the breasts fall OUTWARD (toward the armpits) instead of just sinking into the chest. 0 = straight world-down.")]
		[SerializeField]
		private float boobLateralBias = 0.35f;

		// Token: 0x0400A762 RID: 42850
		[Tooltip("Flip if the breasts spread INWARD instead of outward (the supine 180° roll makes the transform's right/left mapping non-obvious).")]
		[SerializeField]
		private bool invertLateralBias;

		// Token: 0x0400A763 RID: 42851
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x0400A764 RID: 42852
		private CarHideUnderInteractable.HideState _state;

		// Token: 0x0400A765 RID: 42853
		private bool _playerInRange;

		// Token: 0x0400A766 RID: 42854
		private vGenericAction _rangeGenericAction;

		// Token: 0x0400A767 RID: 42855
		private CarRoofCarrier _roofCarrier;

		// Token: 0x0400A768 RID: 42856
		private bool _hideConsumed;

		// Token: 0x0400A769 RID: 42857
		private GameObject _activePlayer;

		// Token: 0x0400A76A RID: 42858
		private float _hiddenSince;

		// Token: 0x0400A76B RID: 42859
		private PlayerActivityLease _hideLease;

		// Token: 0x0400A76C RID: 42860
		private FirstPersonController _firstPersonController;

		// Token: 0x0400A76D RID: 42861
		private int _lifecycleGeneration;

		// Token: 0x0400A76E RID: 42862
		private const PlayerContextResource HideResources = PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.FullBodyAnimation | PlayerContextResource.Rider | PlayerContextResource.HeldItem;

		// Token: 0x0400A76F RID: 42863
		private PlayerActionAnimator _actionAnimator;

		// Token: 0x0400A770 RID: 42864
		private Animator _playerAnimator;

		// Token: 0x0400A771 RID: 42865
		private vThirdPersonController _controller;

		// Token: 0x0400A772 RID: 42866
		private vThirdPersonInput _tpInput;

		// Token: 0x0400A773 RID: 42867
		private Vector3 _preHideLocalPos;

		// Token: 0x0400A774 RID: 42868
		private Quaternion _preHideLocalRot;

		// Token: 0x0400A775 RID: 42869
		private bool _preHidePoseCaptured;

		// Token: 0x0400A776 RID: 42870
		private bool _cameraStatePushed;

		// Token: 0x0400A777 RID: 42871
		private VehicleAI _vehicle;

		// Token: 0x0400A778 RID: 42872
		private CarRideLimiter _rideLimiter;

		// Token: 0x0400A779 RID: 42873
		private bool _blockedByUs;

		// Token: 0x0400A77A RID: 42874
		private bool _holdingForPlayer;

		// Token: 0x0400A77B RID: 42875
		private Transform _originalPlayerParent;

		// Token: 0x0400A77C RID: 42876
		private bool _playerParented;

		// Token: 0x0400A77D RID: 42877
		private Coroutine _restartRoutine;

		// Token: 0x0400A77E RID: 42878
		private const float SpeedSampleWindow = 0.5f;

		// Token: 0x0400A77F RID: 42879
		private Vector3 _lastCarPos;

		// Token: 0x0400A780 RID: 42880
		private float _lastSpeedSampleTime;

		// Token: 0x0400A781 RID: 42881
		private float _measuredCarSpeed;

		// Token: 0x0400A782 RID: 42882
		private bool _hasCarSpeedSample;

		// Token: 0x0400A783 RID: 42883
		private Camera _disabledMainCamera;

		// Token: 0x0400A784 RID: 42884
		private float[] _savedLayerWeights;

		// Token: 0x0400A785 RID: 42885
		private bool _layersForced;

		// Token: 0x0400A786 RID: 42886
		private int _hideCameraIndex;

		// Token: 0x0400A787 RID: 42887
		private Camera _activeHideCamera;

		// Token: 0x0400A788 RID: 42888
		private Vector3 _activeHideCamBasePos;

		// Token: 0x0400A789 RID: 42889
		private Quaternion _activeHideCamBaseRot;

		// Token: 0x0400A78A RID: 42890
		private float _lookYaw;

		// Token: 0x0400A78B RID: 42891
		private float _lookPitch;

		// Token: 0x0400A78C RID: 42892
		private bool _orbitActive;

		// Token: 0x0400A78D RID: 42893
		private float _orbitYaw;

		// Token: 0x0400A78E RID: 42894
		private float _orbitPitch;

		// Token: 0x0400A78F RID: 42895
		private float _orbitDistance;

		// Token: 0x0400A790 RID: 42896
		private bool _physicsPushed;

		// Token: 0x0400A791 RID: 42897
		private bool _rootMotionPushed;

		// Token: 0x0400A792 RID: 42898
		private bool _controlsLocked;

		// Token: 0x0400A793 RID: 42899
		private Coroutine _sequence;

		// Token: 0x020014C8 RID: 5320
		private enum HideState
		{
			// Token: 0x0400A797 RID: 42903
			None,
			// Token: 0x0400A798 RID: 42904
			Entering,
			// Token: 0x0400A799 RID: 42905
			Hidden,
			// Token: 0x0400A79A RID: 42906
			Exiting
		}
	}
}
