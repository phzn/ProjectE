using System;
using ExhibitionistDemo.Movement;
using Features.DebugTools;
using Features.Detection;
using Features.Flash;
using Features.PhoneCompanion;
using Features.RenderingPrecision;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.FirstPerson;
using ProjectExhibitionist.Interactions;
using RayznGames.VehicleSystem;
using UnityEngine;

namespace ProjectExhibitionist.BenchSeating
{
	// Token: 0x02000B31 RID: 2865
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(100)]
	public sealed class BenchPlayerSeating : MonoBehaviour
	{
		// Token: 0x17000D8C RID: 3468
		// (get) Token: 0x06005151 RID: 20817 RVA: 0x001F80D2 File Offset: 0x001F62D2
		public bool IsBusy
		{
			get
			{
				return this._phase > BenchPlayerSeating.Phase.None;
			}
		}

		// Token: 0x17000D8D RID: 3469
		// (get) Token: 0x06005152 RID: 20818 RVA: 0x001F80DD File Offset: 0x001F62DD
		public bool IsSeated
		{
			get
			{
				return this._phase == BenchPlayerSeating.Phase.Seated;
			}
		}

		// Token: 0x17000D8E RID: 3470
		// (get) Token: 0x06005153 RID: 20819 RVA: 0x001F80E8 File Offset: 0x001F62E8
		public BenchSeat CurrentSeat
		{
			get
			{
				return this._seat;
			}
		}

		// Token: 0x17000D8F RID: 3471
		// (get) Token: 0x06005154 RID: 20820 RVA: 0x001F80F0 File Offset: 0x001F62F0
		private bool IsStandBlockedByCamera
		{
			get
			{
				return SmartphoneController.IsPhoneOutFor(this._animator) || FlyCamera.IsActive;
			}
		}

		// Token: 0x06005155 RID: 20821 RVA: 0x001F8108 File Offset: 0x001F6308
		public static BenchPlayerSeating EnsureFor(GameObject player)
		{
			if (player == null)
			{
				return null;
			}
			BenchPlayerSeating component = player.GetComponent<BenchPlayerSeating>();
			if (!(component != null))
			{
				return player.AddComponent<BenchPlayerSeating>();
			}
			return component;
		}

		// Token: 0x06005156 RID: 20822 RVA: 0x001F8138 File Offset: 0x001F6338
		public static void CancelForPlayer(GameObject player, bool restoreStandingPose = true)
		{
			if (player != null)
			{
				BenchPlayerSeating component = player.GetComponent<BenchPlayerSeating>();
				if (component == null)
				{
					return;
				}
				component.CancelSeating(restoreStandingPose);
			}
		}

		// Token: 0x06005157 RID: 20823 RVA: 0x001F8154 File Offset: 0x001F6354
		public static bool CanStart(vThirdPersonController controller)
		{
			if (controller == null || !controller.isActiveAndEnabled || !controller.isGrounded || controller.ragdolled || controller.isDead || controller.isJumping || controller.isRolling || controller.customAction || controller.isCrouching || controller.lockMovement || controller.lockAnimMovement || controller.lockAnimRotation || controller.disableAnimations || controller.disableCheckGround || controller.animator == null || !controller.animator.isHuman)
			{
				return false;
			}
			vThirdPersonInput component = controller.GetComponent<vThirdPersonInput>();
			vGenericAction component2 = controller.GetComponent<vGenericAction>();
			BenchPlayerSeating component3 = controller.GetComponent<BenchPlayerSeating>();
			Rigidbody component4 = controller.GetComponent<Rigidbody>();
			CapsuleCollider component5 = controller.GetComponent<CapsuleCollider>();
			return component != null && component.isActiveAndEnabled && !component.lockInput && !component.lockMoveInput && component4 != null && !component4.isKinematic && component5 != null && component5.enabled && (component2 == null || !component2.doingAction) && (component3 == null || !component3.IsBusy);
		}

		// Token: 0x06005158 RID: 20824 RVA: 0x001F8282 File Offset: 0x001F6482
		private void OnEnable()
		{
			WorldOrigin.Shifted += this.OnWorldOriginShift;
		}

		// Token: 0x06005159 RID: 20825 RVA: 0x001F8295 File Offset: 0x001F6495
		private void OnDisable()
		{
			WorldOrigin.Shifted -= this.OnWorldOriginShift;
			this.CancelSeating(true);
		}

		// Token: 0x0600515A RID: 20826 RVA: 0x001F82B0 File Offset: 0x001F64B0
		public bool TrySit(BenchSeat seat, InteractionConfigSO standConfig)
		{
			this._controller = base.GetComponent<vThirdPersonController>();
			if (this.IsBusy || !BenchPlayerSeating.CanStart(this._controller) || seat == null || !seat.isActiveAndEnabled || seat.Animations == null || !seat.Animations.IsValid(true, false) || !PlayerActivityGate.CanStartWorldInteraction || InteractionCandidateRegistry.HasInteractedThisFrame)
			{
				return false;
			}
			float num;
			if (!BenchInteractable.CanReachSeat(base.transform, seat, 1.15f, 1.65f, this._reachHits, out num))
			{
				return false;
			}
			if (!seat.TryReserve(base.gameObject))
			{
				return false;
			}
			if (!PlayerActivityGate.TryAcquire("BenchSeating#" + base.GetInstanceID().ToString(), this, PlayerActivity.Interact, PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.FullBodyAnimation, PlayerActivity.AllPlayerActions, out this._lease, false, null))
			{
				seat.Release(base.gameObject);
				return false;
			}
			this._seat = seat;
			this._standConfig = standConfig;
			this._entryPosition = (this._lastAppliedPosition = base.transform.position);
			this._entryRotation = base.transform.rotation;
			this._standRequested = false;
			this._phase = BenchPlayerSeating.Phase.Approaching;
			this._phaseElapsed = 0f;
			this.CaptureApproachControls();
			float num2 = this._capsule.radius * Mathf.Max(Mathf.Abs(base.transform.lossyScale.x), Mathf.Abs(base.transform.lossyScale.z));
			float num3 = (this._controller.useSphereCast ? this._controller.sphereCastRadius : 0f);
			float num4 = Mathf.Max(new float[]
			{
				num2,
				num3,
				this._controller.planeSize
			}) + 0.06f;
			this._animation = new BenchAnimationPlayer(this._animator, seat, true, num4, false);
			if (!this._animation.IsValid || (this._animation.EntryPosition - base.transform.position).sqrMagnitude > 9f || !this.HasStandingFloor(this._animation.EntryPosition, false))
			{
				this.CancelSeating(false);
				return false;
			}
			InteractionCandidateRegistry.MarkInteract();
			return true;
		}

		// Token: 0x0600515B RID: 20827 RVA: 0x001F84DC File Offset: 0x001F66DC
		public void RequestStand()
		{
			if (!this.IsBusy || this._phase == BenchPlayerSeating.Phase.StandingUp || this._phase == BenchPlayerSeating.Phase.Releasing)
			{
				return;
			}
			if (this.IsStandBlockedByCamera)
			{
				this._standRequested = false;
				return;
			}
			if (this._phase == BenchPlayerSeating.Phase.Approaching)
			{
				this.CancelSeating(false);
				return;
			}
			this._standRequested = true;
			if (this._phase == BenchPlayerSeating.Phase.Seated)
			{
				this.BeginStand();
			}
		}

		// Token: 0x0600515C RID: 20828 RVA: 0x001F853C File Offset: 0x001F673C
		public bool TryPrepareForEncounter()
		{
			if (!this.IsBusy)
			{
				return true;
			}
			if (this._phase == BenchPlayerSeating.Phase.Approaching)
			{
				this.FinishSession(false);
				return true;
			}
			Vector3 vector;
			Quaternion quaternion;
			if (this._seat == null || !this._seat.TryGetExitPose(base.transform, out vector, out quaternion, true))
			{
				return false;
			}
			this.ApplyPose(vector, quaternion);
			this.FinishSession(false);
			return true;
		}

		// Token: 0x0600515D RID: 20829 RVA: 0x001F85A0 File Offset: 0x001F67A0
		private void Update()
		{
			if (!this.IsBusy)
			{
				return;
			}
			bool flag = (base.transform.position - this._lastAppliedPosition).sqrMagnitude > 2.25f;
			bool flag2 = this._controller == null || !this._controller.isActiveAndEnabled || this._controller.ragdolled || this._controller.isDead || this._input == null || !this._input.isActiveAndEnabled;
			bool flag3 = SceneTransitionManager.Instance != null && SceneTransitionManager.Instance.IsTransitioning;
			bool flag4 = ExhibitionEventDirector.Instance != null && ExhibitionEventDirector.Instance.IsEventActive;
			if (flag || flag2 || flag3 || flag4 || this._lease == null || !this._lease.IsActive)
			{
				this.CancelSeating(!flag && !flag2 && !flag3 && !flag4);
				return;
			}
			if (this._seat == null || !this._seat.isActiveAndEnabled || !this._seat.IsReservedBy(base.gameObject))
			{
				this.CancelSeating(true);
				return;
			}
			if (!GameplayInputGate.BlocksGameplayInput && Time.timeScale > 0f && !FlyCamera.IsActive && !TextInputFocus.BlocksGameplayInput && this.TryConsumeStandInput())
			{
				InteractionCandidateRegistry.MarkInteract();
				this.RequestStand();
			}
			this._phaseElapsed += Time.deltaTime;
			switch (this._phase)
			{
			case BenchPlayerSeating.Phase.Approaching:
				if (this._phaseElapsed >= this.approachTimeout)
				{
					this.CancelSeating(false);
					return;
				}
				break;
			case BenchPlayerSeating.Phase.SittingDown:
				if (this._animation.IsComplete)
				{
					this._animation.PlayIdle();
					this._phase = BenchPlayerSeating.Phase.Seated;
					this._phaseElapsed = 0f;
					this.SetSeatedActivities(true);
					if (this._standRequested)
					{
						this.BeginStand();
						return;
					}
				}
				break;
			case BenchPlayerSeating.Phase.Seated:
				this.MaintainHud();
				if (this._standRequested)
				{
					this.BeginStand();
					return;
				}
				break;
			case BenchPlayerSeating.Phase.StandingUp:
				if (this._animation.IsComplete && this.HasClearAuthoredExit())
				{
					this.PrepareStandingAnimator();
					this._controller.disableAnimations = this._savedDisableAnimations;
					this._animation.BeginRelease();
					this._phase = BenchPlayerSeating.Phase.Releasing;
					this._phaseElapsed = 0f;
					return;
				}
				break;
			case BenchPlayerSeating.Phase.Releasing:
				if (this._animation.IsComplete && this.HasClearAuthoredExit())
				{
					this.FinishSession(true);
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x0600515E RID: 20830 RVA: 0x001F881C File Offset: 0x001F6A1C
		private void LateUpdate()
		{
			if (!this.IsBusy || this._seat == null)
			{
				return;
			}
			if (this._phase == BenchPlayerSeating.Phase.Approaching)
			{
				this._lastAppliedPosition = base.transform.position;
				return;
			}
			if (this.IsSeated && SmartphoneController.IsPlayerPoseFrozenFor(this._animator))
			{
				return;
			}
			this._animation.SetUpperBodyEnabled(this.IsSeated && ((this._cover != null && this._cover.IsCovering) || SmartphoneController.IsHandheldPhoneOutFor(this._animator) || FlashGateRegistry.IsSeatedClothingActionActiveFor(this._animator)), (this._cover != null && this._cover.IsCovering) ? this._cover.ArmsPoseTransitionKey : 0, this._cover == null || !this._cover.IsCovering || this._cover.HasReadyArmsAnimation);
			float num = ((this._phase == BenchPlayerSeating.Phase.StandingUp && !this.HasClearAuthoredExit()) ? 0f : Time.deltaTime);
			this._animation.Tick(num);
			this.ApplyPose(this._animation.RootPosition, this._animation.RootRotation);
			if (this.IsSeated && this._coverRig != null && ((this._cover != null && this._cover.IsCovering) || SmartphoneController.IsHandheldPhoneOutFor(this._animator)))
			{
				this._coverRig.ApplyCoverHandIKToCurrentPose();
			}
			this._animation.CompleteUpperBodyPose();
			BenchCameraFraming cameraFraming = this._cameraFraming;
			if (cameraFraming == null)
			{
				return;
			}
			cameraFraming.ApplyPose();
		}

		// Token: 0x0600515F RID: 20831 RVA: 0x001F89B9 File Offset: 0x001F6BB9
		private bool TryConsumeStandInput()
		{
			return !this.IsStandBlockedByCamera && this.standInput.GetButtonDown() && InteractionCandidateRegistry.TryConsumeContextInput(this.standInput);
		}

		// Token: 0x06005160 RID: 20832 RVA: 0x001F89E0 File Offset: 0x001F6BE0
		private void BeginStand()
		{
			if (this.IsStandBlockedByCamera)
			{
				this._standRequested = false;
				return;
			}
			if (!this.HasClearAuthoredExit())
			{
				return;
			}
			this._standRequested = false;
			SmartphoneController.CloseHandheldForBenchTransition(this._animator);
			FlashGateRegistry.InterruptGarmentsForBodyHandoff(base.transform);
			this.SetSeatedActivities(false);
			if (this._animation == null || !this._animation.PlayStandUp())
			{
				this.CancelSeating(true);
				return;
			}
			this._phase = BenchPlayerSeating.Phase.StandingUp;
			this._phaseElapsed = 0f;
			this.HideHud();
		}

		// Token: 0x06005161 RID: 20833 RVA: 0x001F8A60 File Offset: 0x001F6C60
		private void SetSeatedActivities(bool seated)
		{
			if (this._lease != null && this._lease.IsActive)
			{
				PlayerActivityGate.TryUpdate(this._lease, PlayerActivity.None, PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.FullBodyAnimation, seated ? (PlayerActivity.Emote | PlayerActivity.Interact | PlayerActivity.StanceSwitch | PlayerActivity.Undress | PlayerActivity.Inventory | PlayerActivity.Crouch) : PlayerActivity.AllPlayerActions);
			}
			if (this._cover != null)
			{
				this._cover.SetSeatedHandTargets(seated);
			}
		}

		// Token: 0x06005162 RID: 20834 RVA: 0x001F8ABC File Offset: 0x001F6CBC
		public void CancelSeating(bool restoreStandingPose = true)
		{
			if (!this.IsBusy && !this._controlsCaptured && this._lease == null)
			{
				return;
			}
			if (restoreStandingPose && this._controlsCaptured && this._phase != BenchPlayerSeating.Phase.Approaching)
			{
				Vector3 vector = this._entryPosition;
				Quaternion quaternion = this._entryRotation;
				if (!(this._seat != null) || !this._seat.TryGetExitPose(base.transform, out vector, out quaternion, true))
				{
					vector = this._entryPosition;
					quaternion = this._entryRotation;
				}
				this.ApplyPose(vector, quaternion);
			}
			this.FinishSession(false);
		}

		// Token: 0x06005163 RID: 20835 RVA: 0x001F8B4C File Offset: 0x001F6D4C
		private void CaptureApproachControls()
		{
			AutoLocomotionController component = base.GetComponent<AutoLocomotionController>();
			if (component != null)
			{
				component.CancelAutoLocomotion();
			}
			this._input = base.GetComponent<vThirdPersonInput>();
			this._animator = this._controller.animator;
			this._body = base.GetComponent<Rigidbody>();
			this._capsule = base.GetComponent<CapsuleCollider>();
			this._firstPerson = base.GetComponentInChildren<FirstPersonController>(true);
			this._emotion = PlayerEmotionController.Instance;
			this._cover = base.GetComponentInChildren<CoverPoseController>(true);
			if (this._cover == null && CoverPoseController.Instance != null && CoverPoseController.Instance.PlayerAnimator == this._animator)
			{
				this._cover = CoverPoseController.Instance;
			}
			this._coverRig = ((this._animator != null) ? this._animator.GetComponent<CharacterRigController>() : null);
			this._savedLockInput = this._input.lockInput;
			this._savedLockMoveInput = this._input.lockMoveInput;
			this._savedMoveDirectionLock = this._input.lockUpdateMoveDirection;
			this._savedLockMovement = this._controller.lockMovement;
			this._savedLockRotation = this._controller.lockRotation;
			this._savedCheckGround = this._controller.disableCheckGround;
			this._savedDisableAnimations = this._controller.disableAnimations;
			this._savedUseRootMotion = this._controller.useRootMotion;
			this._savedAlwaysWalk = this._controller.alwaysWalkByDefault;
			this._savedMoveToDirection = this._controller.moveToDirectionInFree;
			this._savedStrafing = this._controller.isStrafing;
			this._savedLocomotionType = this._controller.locomotionType;
			if (this._body != null)
			{
				this._savedKinematic = this._body.isKinematic;
				this._savedGravity = this._body.useGravity;
			}
			if (this._capsule != null)
			{
				this._savedCapsuleEnabled = this._capsule.enabled;
			}
			this._controlsCaptured = true;
			this._controller.StopCharacter();
			this._input.lockInput = true;
			this._input.lockMoveInput = true;
			this._input.lockUpdateMoveDirection = true;
			this._input.onFixedUpdate += this.GuideApproach;
			this._controller.lockMovement = false;
			this._controller.lockRotation = true;
			this._controller.disableCheckGround = false;
			this._controller.disableAnimations = false;
			this._controller.useRootMotion = false;
			this._controller.alwaysWalkByDefault = true;
			this._controller.moveToDirectionInFree = true;
			this._controller.isStrafing = false;
			this._controller.locomotionType = vThirdPersonMotor.LocomotionType.OnlyFree;
			if (this._emotion != null)
			{
				this._emotion.DisableEmotionLayersForAction();
				this._emotionSuppressed = true;
			}
			if (!SmartphoneController.IsStableTripodCameraOwnerFor(this._animator))
			{
				FirstPersonController firstPerson = this._firstPerson;
				if (firstPerson == null)
				{
					return;
				}
				firstPerson.BeginInteractionView(this, false, 80f, false, true, base.transform, new Vector3?(Vector3.up * this._firstPerson.confidentPreset.eyeHeightOffset), null);
			}
		}

		// Token: 0x06005164 RID: 20836 RVA: 0x001F8E6C File Offset: 0x001F706C
		private void GuideApproach()
		{
			if (this._phase != BenchPlayerSeating.Phase.Approaching || this._animation == null || Time.timeScale <= 0f)
			{
				return;
			}
			Vector3 vector = ((this._body != null) ? this._body.position : base.transform.position);
			Vector3 vector2 = this._animation.EntryPosition - vector;
			Vector3 vector3 = Vector3.ProjectOnPlane(vector2, Vector3.up);
			float magnitude = vector3.magnitude;
			bool flag = magnitude <= 0.02f;
			Quaternion quaternion = (flag ? this._animation.EntryRotation : Quaternion.LookRotation(vector3));
			Quaternion quaternion2 = Quaternion.RotateTowards(base.transform.rotation, quaternion, this.approachTurnSpeed * Time.fixedDeltaTime);
			base.transform.rotation = quaternion2;
			if (this._body != null)
			{
				this._body.rotation = quaternion2;
			}
			if (flag)
			{
				this._controller.input = (this._controller.moveDirection = (this._controller.targetVelocity = Vector3.zero));
				if (this._body != null && !this._body.isKinematic)
				{
					this._body.velocity = Vector3.zero;
				}
				if (Quaternion.Angle(quaternion2, quaternion) <= 0.5f && Mathf.Abs(vector2.y) <= 0.2f && this._controller.isGrounded)
				{
					this.BeginSitDown();
				}
				return;
			}
			float num = Mathf.Clamp01(Vector3.Dot(quaternion2 * Vector3.forward, vector3.normalized));
			float num2 = Mathf.Clamp01(magnitude / 0.09f) * num;
			Vector3 vector4 = vector3.normalized * num2;
			this._controller.moveDirection = vector4;
			this._controller.input = base.transform.InverseTransformDirection(vector4);
			this._controller.isRunning = (this._controller.isSprinting = false);
			float num3 = Mathf.Min(this._controller.targetVelocity.magnitude, magnitude / Time.fixedDeltaTime);
			this._controller.targetVelocity = vector3.normalized * (num3 * num);
		}

		// Token: 0x06005165 RID: 20837 RVA: 0x001F90A4 File Offset: 0x001F72A4
		private void BeginSitDown()
		{
			if (this._seat == null || !this._seat.Occupy(base.gameObject))
			{
				this.CancelSeating(false);
				return;
			}
			this._controller.StopCharacter();
			this._controller.lockMovement = true;
			this._controller.disableCheckGround = true;
			this._controller.disableAnimations = true;
			if (this._body != null)
			{
				if (!this._body.isKinematic)
				{
					this._body.velocity = Vector3.zero;
					this._body.angularVelocity = Vector3.zero;
				}
				this._body.isKinematic = true;
				this._body.useGravity = false;
			}
			if (this._capsule != null)
			{
				this._capsule.enabled = false;
			}
			this._cameraFraming = new BenchCameraFraming(this, base.transform, this._animator.GetBoneTransform(HumanBodyBones.Head), (this._input != null) ? this._input.tpCamera : null);
			if (!this._animation.PlaySitDown())
			{
				this.CancelSeating(false);
				return;
			}
			this._phase = BenchPlayerSeating.Phase.SittingDown;
			this._phaseElapsed = 0f;
			this.ApplyPose(this._animation.RootPosition, this._animation.RootRotation);
		}

		// Token: 0x06005166 RID: 20838 RVA: 0x001F91F4 File Offset: 0x001F73F4
		private bool HasStandingFloor(Vector3 position, bool ignoreNpcs = false)
		{
			RaycastHit raycastHit;
			return this.TryGetStandingFloor(position, out raycastHit, ignoreNpcs);
		}

		// Token: 0x06005167 RID: 20839 RVA: 0x001F920C File Offset: 0x001F740C
		private bool TryGetStandingFloor(Vector3 position, out RaycastHit floor, bool ignoreNpcs = false)
		{
			floor = default(RaycastHit);
			if (this._seat == null || !this._seat.HasStandingClearance(position, base.transform, ignoreNpcs))
			{
				return false;
			}
			int num;
			while ((num = Physics.RaycastNonAlloc(position + Vector3.up * 0.3f, Vector3.down, this._reachHits, 0.55f, this._seat.clearanceLayers, QueryTriggerInteraction.Ignore)) == this._reachHits.Length)
			{
				Array.Resize<RaycastHit>(ref this._reachHits, this._reachHits.Length * 2);
			}
			int num2 = -1;
			for (int i = 0; i < num; i++)
			{
				if (!BenchSeat.ShouldIgnoreExitCollider(this._reachHits[i].collider, base.transform, ignoreNpcs) && (num2 < 0 || this._reachHits[i].distance < this._reachHits[num2].distance))
				{
					num2 = i;
				}
			}
			if (num2 < 0 || this._reachHits[num2].normal.y < 0.65f || Mathf.Abs(this._reachHits[num2].point.y - position.y) > 0.2f)
			{
				return false;
			}
			floor = this._reachHits[num2];
			return true;
		}

		// Token: 0x06005168 RID: 20840 RVA: 0x001F9358 File Offset: 0x001F7558
		private bool HasClearAuthoredExit()
		{
			if (this._animation == null || !this.HasStandingFloor(this._animation.ExitPosition, true))
			{
				return false;
			}
			Vector3 exitProbeOrigin = this._seat.GetExitProbeOrigin(base.transform.position, 0.85f, 0.18f);
			Vector3 vector = this._animation.ExitPosition + Vector3.up * 0.85f - exitProbeOrigin;
			float magnitude = vector.magnitude;
			if (magnitude < 0.01f)
			{
				return true;
			}
			int num;
			while ((num = Physics.SphereCastNonAlloc(exitProbeOrigin, 0.18f, vector / magnitude, this._reachHits, magnitude, this._seat.clearanceLayers, QueryTriggerInteraction.Ignore)) == this._reachHits.Length)
			{
				Array.Resize<RaycastHit>(ref this._reachHits, this._reachHits.Length * 2);
			}
			for (int i = 0; i < num; i++)
			{
				if (!BenchSeat.ShouldIgnoreExitCollider(this._reachHits[i].collider, base.transform, true))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005169 RID: 20841 RVA: 0x001F945C File Offset: 0x001F765C
		private void FinishSession(bool completedStand = false)
		{
			bool flag = this._controlsCaptured && this._phase != BenchPlayerSeating.Phase.Approaching;
			SmartphoneController.CloseHandheldForBenchTransition(this._animator);
			FlashGateRegistry.InterruptGarmentsForBodyHandoff(base.transform);
			this._phase = BenchPlayerSeating.Phase.None;
			this._standRequested = false;
			this.HideHud();
			if (this._cover != null)
			{
				this._cover.SetSeatedHandTargets(false);
			}
			BenchCameraFraming cameraFraming = this._cameraFraming;
			if (cameraFraming != null)
			{
				cameraFraming.Dispose();
			}
			this._cameraFraming = null;
			BenchAnimationPlayer animation = this._animation;
			if (animation != null)
			{
				animation.Dispose();
			}
			this._animation = null;
			FirstPersonController firstPerson = this._firstPerson;
			if (firstPerson != null)
			{
				firstPerson.EndInteractionView(this);
			}
			if (this._emotionSuppressed && this._emotion != null)
			{
				this._emotion.RestoreEmotionLayers();
			}
			this._emotionSuppressed = false;
			if (this._controlsCaptured)
			{
				bool flag2 = this._controller != null && (this._controller.isDead || this._controller.ragdolled);
				if (this._controller != null)
				{
					this.ClearGuidedLocomotion();
					this._controller.lockMovement = this._savedLockMovement;
					this._controller.lockRotation = this._savedLockRotation;
					this._controller.disableCheckGround = this._savedCheckGround;
					this._controller.disableAnimations = this._savedDisableAnimations;
					this._controller.useRootMotion = this._savedUseRootMotion;
					this._controller.alwaysWalkByDefault = this._savedAlwaysWalk;
					this._controller.moveToDirectionInFree = this._savedMoveToDirection;
					this._controller.isStrafing = this._savedStrafing;
					this._controller.locomotionType = this._savedLocomotionType;
				}
				if (this._input != null)
				{
					this._input.lockInput = this._savedLockInput || flag2;
					this._input.lockMoveInput = this._savedLockMoveInput;
					this._input.lockUpdateMoveDirection = this._savedMoveDirectionLock;
					this._input.onFixedUpdate -= this.GuideApproach;
				}
				if (this._capsule != null && !flag2)
				{
					this._capsule.enabled = this._savedCapsuleEnabled;
				}
				if (flag && !flag2 && this._capsule != null && this._capsule.enabled)
				{
					BenchNpcCollisionRelease.ProtectStandingCapsule(this._capsule);
				}
				if (this._body != null)
				{
					this._body.isKinematic = this._savedKinematic;
					this._body.useGravity = this._savedGravity;
					if (!this._body.isKinematic)
					{
						this._body.velocity = Vector3.zero;
						this._body.angularVelocity = Vector3.zero;
						this._body.WakeUp();
					}
				}
				if (completedStand && !flag2 && this._controller != null && this._controller.isActiveAndEnabled && !this._savedCheckGround && !this._savedDisableAnimations && this._body != null && !this._body.isKinematic && this._capsule != null && this._capsule.enabled)
				{
					this.RestoreStandingMotor();
				}
				this._controlsCaptured = false;
			}
			if (this._seat != null)
			{
				this._seat.Release(base.gameObject);
			}
			this._seat = null;
			this._standConfig = null;
			PlayerActivityLease lease = this._lease;
			if (lease != null)
			{
				lease.Dispose();
			}
			this._lease = null;
		}

		// Token: 0x0600516A RID: 20842 RVA: 0x001F97E0 File Offset: 0x001F79E0
		private void ClearGuidedLocomotion()
		{
			this._controller.input = (this._controller.inputSmooth = (this._controller.moveDirection = (this._controller.targetVelocity = Vector3.zero)));
			this._controller.inputMagnitude = (this._controller.horizontalSpeed = (this._controller.verticalSpeed = (this._controller.rotationMagnitude = (this._controller.moveSpeed = 0f))));
			this._controller.isRunning = (this._controller.isSprinting = false);
			this._controller.lastCharacterAngle = base.transform.eulerAngles;
		}

		// Token: 0x0600516B RID: 20843 RVA: 0x001F98A8 File Offset: 0x001F7AA8
		private void PrepareStandingAnimator()
		{
			this.ClearGuidedLocomotion();
			if (this._animator == null || !this._animator.isActiveAndEnabled || this._animator.runtimeAnimatorController == null)
			{
				return;
			}
			this._controller.ResetInputAnimatorParameters();
			this._animator.SetBool(vAnimatorParameters.IsRunning, false);
			this._animator.SetFloat(vAnimatorParameters.RotationMagnitude, 0f);
			this._animator.SetFloat(vAnimatorParameters.VerticalVelocity, 0f);
		}

		// Token: 0x0600516C RID: 20844 RVA: 0x001F9930 File Offset: 0x001F7B30
		private void RestoreStandingMotor()
		{
			RaycastHit raycastHit;
			if (!this.TryGetStandingFloor(base.transform.position, out raycastHit, true) || (this._controller.groundLayer.value & (1 << raycastHit.collider.gameObject.layer)) == 0)
			{
				return;
			}
			this._controller.groundHit = raycastHit;
			this._controller.groundDistance = Mathf.Max(0f, -base.transform.InverseTransformPoint(raycastHit.point).y);
			this._controller.snapDistance = Vector3.Dot(this._controller.rayOrigin - raycastHit.point, base.transform.up);
			this._controller.validSnapPoint = true;
			this._controller.isGrounded = true;
			this._controller.heightReached = base.transform.position.y;
			this._controller.verticalVelocity = 0f;
			this.PrepareStandingAnimator();
			if (this._animator == null || !this._animator.isActiveAndEnabled || this._animator.runtimeAnimatorController == null)
			{
				return;
			}
			Vector3 position = base.transform.position;
			Quaternion rotation = base.transform.rotation;
			this._animator.Update(0f);
			this.ApplyPose(position, rotation);
		}

		// Token: 0x0600516D RID: 20845 RVA: 0x001F9A90 File Offset: 0x001F7C90
		private void MaintainHud()
		{
			if (this.IsStandBlockedByCamera)
			{
				this.HideHud();
				return;
			}
			InteractionHUDController instance = InteractionHUDController.Instance;
			if (instance != null && this._standConfig != null && instance.CurrentConfig == null)
			{
				instance.Show(this._standConfig);
			}
		}

		// Token: 0x0600516E RID: 20846 RVA: 0x001F9AE3 File Offset: 0x001F7CE3
		private void HideHud()
		{
			if (InteractionHUDController.Instance != null)
			{
				InteractionHUDController.Instance.HideIfCurrent(this._standConfig);
			}
		}

		// Token: 0x0600516F RID: 20847 RVA: 0x001F9B03 File Offset: 0x001F7D03
		private void ApplyPose(Vector3 position, Quaternion rotation)
		{
			base.transform.SetPositionAndRotation(position, rotation);
			if (this._body != null)
			{
				this._body.position = position;
				this._body.rotation = rotation;
			}
			this._lastAppliedPosition = position;
		}

		// Token: 0x06005170 RID: 20848 RVA: 0x001F9B3F File Offset: 0x001F7D3F
		private void OnWorldOriginShift(Vector3 delta)
		{
			if (!this.IsBusy)
			{
				return;
			}
			this._entryPosition += delta;
			this._lastAppliedPosition += delta;
		}

		// Token: 0x040051F3 RID: 20979
		private const PlayerContextResource SeatResources = PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.FullBodyAnimation;

		// Token: 0x040051F4 RID: 20980
		private const PlayerActivity SeatedBlockedActivities = PlayerActivity.Emote | PlayerActivity.Interact | PlayerActivity.StanceSwitch | PlayerActivity.Undress | PlayerActivity.Inventory | PlayerActivity.Crouch;

		// Token: 0x040051F5 RID: 20981
		[SerializeField]
		private GenericInput standInput = new GenericInput("E", "A", "A");

		// Token: 0x040051F6 RID: 20982
		[SerializeField]
		[Min(1f)]
		private float approachTimeout = 8f;

		// Token: 0x040051F7 RID: 20983
		[SerializeField]
		[Min(30f)]
		private float approachTurnSpeed = 480f;

		// Token: 0x040051F8 RID: 20984
		private BenchPlayerSeating.Phase _phase;

		// Token: 0x040051F9 RID: 20985
		private BenchSeat _seat;

		// Token: 0x040051FA RID: 20986
		private BenchAnimationPlayer _animation;

		// Token: 0x040051FB RID: 20987
		private BenchCameraFraming _cameraFraming;

		// Token: 0x040051FC RID: 20988
		private PlayerActivityLease _lease;

		// Token: 0x040051FD RID: 20989
		private InteractionConfigSO _standConfig;

		// Token: 0x040051FE RID: 20990
		private vThirdPersonController _controller;

		// Token: 0x040051FF RID: 20991
		private vThirdPersonInput _input;

		// Token: 0x04005200 RID: 20992
		private Animator _animator;

		// Token: 0x04005201 RID: 20993
		private Rigidbody _body;

		// Token: 0x04005202 RID: 20994
		private CapsuleCollider _capsule;

		// Token: 0x04005203 RID: 20995
		private FirstPersonController _firstPerson;

		// Token: 0x04005204 RID: 20996
		private PlayerEmotionController _emotion;

		// Token: 0x04005205 RID: 20997
		private CoverPoseController _cover;

		// Token: 0x04005206 RID: 20998
		private CharacterRigController _coverRig;

		// Token: 0x04005207 RID: 20999
		private float _phaseElapsed;

		// Token: 0x04005208 RID: 21000
		private bool _standRequested;

		// Token: 0x04005209 RID: 21001
		private bool _controlsCaptured;

		// Token: 0x0400520A RID: 21002
		private bool _emotionSuppressed;

		// Token: 0x0400520B RID: 21003
		private bool _savedLockInput;

		// Token: 0x0400520C RID: 21004
		private bool _savedLockMoveInput;

		// Token: 0x0400520D RID: 21005
		private bool _savedMoveDirectionLock;

		// Token: 0x0400520E RID: 21006
		private bool _savedLockMovement;

		// Token: 0x0400520F RID: 21007
		private bool _savedLockRotation;

		// Token: 0x04005210 RID: 21008
		private bool _savedCheckGround;

		// Token: 0x04005211 RID: 21009
		private bool _savedDisableAnimations;

		// Token: 0x04005212 RID: 21010
		private bool _savedUseRootMotion;

		// Token: 0x04005213 RID: 21011
		private bool _savedKinematic;

		// Token: 0x04005214 RID: 21012
		private bool _savedGravity;

		// Token: 0x04005215 RID: 21013
		private bool _savedCapsuleEnabled;

		// Token: 0x04005216 RID: 21014
		private bool _savedAlwaysWalk;

		// Token: 0x04005217 RID: 21015
		private bool _savedMoveToDirection;

		// Token: 0x04005218 RID: 21016
		private bool _savedStrafing;

		// Token: 0x04005219 RID: 21017
		private vThirdPersonMotor.LocomotionType _savedLocomotionType;

		// Token: 0x0400521A RID: 21018
		private Vector3 _entryPosition;

		// Token: 0x0400521B RID: 21019
		private Vector3 _lastAppliedPosition;

		// Token: 0x0400521C RID: 21020
		private Quaternion _entryRotation;

		// Token: 0x0400521D RID: 21021
		private RaycastHit[] _reachHits = new RaycastHit[24];

		// Token: 0x02000B32 RID: 2866
		private enum Phase
		{
			// Token: 0x0400521F RID: 21023
			None,
			// Token: 0x04005220 RID: 21024
			Approaching,
			// Token: 0x04005221 RID: 21025
			SittingDown,
			// Token: 0x04005222 RID: 21026
			Seated,
			// Token: 0x04005223 RID: 21027
			StandingUp,
			// Token: 0x04005224 RID: 21028
			Releasing
		}
	}
}
