using System;
using System.Collections.Generic;
using Features.DebugTools;
using Features.Detection;
using Features.RenderingPrecision;
using Invector;
using Invector.vCamera;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using Invector.vCover;
using ProjectExhibitionist.CharacterCustomization.Core.Runtime;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.FirstPerson;
using UnityEngine;

namespace RayznGames.VehicleSystem
{
	// Token: 0x02000777 RID: 1911
	[DefaultExecutionOrder(500)]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(CielRagdollBootstrap))]
	public sealed class CielRagdollRecovery : MonoBehaviour
	{
		// Token: 0x17000976 RID: 2422
		// (get) Token: 0x060031DC RID: 12764 RVA: 0x00119ECF File Offset: 0x001180CF
		public bool IsRecovering
		{
			get
			{
				return this.phase > CielRagdollRecovery.RecoveryPhase.None;
			}
		}

		// Token: 0x17000977 RID: 2423
		// (get) Token: 0x060031DD RID: 12765 RVA: 0x00119EDA File Offset: 0x001180DA
		public bool AreRagdollEyesForcedClosed
		{
			get
			{
				return this.ragdollEyesForcedClosed && this.ragdollEyeOverrides.Count > 0;
			}
		}

		// Token: 0x17000978 RID: 2424
		// (get) Token: 0x060031DE RID: 12766 RVA: 0x00119EF4 File Offset: 0x001180F4
		public bool HasStartedGetUpAnimation
		{
			get
			{
				return this.phase == CielRagdollRecovery.RecoveryPhase.GettingUp && this.getUpAnimationForced;
			}
		}

		// Token: 0x17000979 RID: 2425
		// (get) Token: 0x060031DF RID: 12767 RVA: 0x00119F07 File Offset: 0x00118107
		// (set) Token: 0x060031E0 RID: 12768 RVA: 0x00119F0F File Offset: 0x0011810F
		public bool CompletedLastGetUpToIdleBlend { get; private set; }

		// Token: 0x1700097A RID: 2426
		// (get) Token: 0x060031E1 RID: 12769 RVA: 0x00119F18 File Offset: 0x00118118
		// (set) Token: 0x060031E2 RID: 12770 RVA: 0x00119F20 File Offset: 0x00118120
		public bool LastGetUpNeutralStateResolved { get; private set; }

		// Token: 0x060031E3 RID: 12771 RVA: 0x00119F2C File Offset: 0x0011812C
		private void Awake()
		{
			if (base.GetComponent<CustomizationPreviewPhysicsGuard>() != null)
			{
				base.enabled = false;
				return;
			}
			this.bootstrap = base.GetComponent<CielRagdollBootstrap>();
			this.character = base.GetComponent<vThirdPersonController>();
			this.input = base.GetComponent<vThirdPersonInput>();
			this.animator = base.GetComponent<Animator>();
			if (base.GetComponent<CielOnFootNpcCrashRagdoll>() == null)
			{
				base.gameObject.AddComponent<CielOnFootNpcCrashRagdoll>();
			}
			this.ResolveRagdoll();
			this.CacheHumanoidPhysics();
			this.CacheAnimatorRecoveryData();
			this.SnapshotControlState();
		}

		// Token: 0x060031E4 RID: 12772 RVA: 0x00119FB1 File Offset: 0x001181B1
		private void OnEnable()
		{
			WorldOrigin.Shifted += this.ApplyWorldOriginShift;
		}

		// Token: 0x060031E5 RID: 12773 RVA: 0x00119FC4 File Offset: 0x001181C4
		private void ApplyWorldOriginShift(Vector3 delta)
		{
			if (this.hasPhysicalGetUpHipsPosition)
			{
				this.physicalGetUpHipsPosition += delta;
			}
			if (this.hasRecoveryGround)
			{
				this.recoveryGroundPoint += delta;
			}
		}

		// Token: 0x060031E6 RID: 12774 RVA: 0x00119FFC File Offset: 0x001181FC
		private void Update()
		{
			if (FlyCamera.IsGlobalPauseActive)
			{
				this.EnforceEmotionLayerSuppression();
				return;
			}
			this.ResolveRagdoll();
			if (this.ragdoll == null)
			{
				return;
			}
			if (this.phase != CielRagdollRecovery.RecoveryPhase.None)
			{
				if (this.phase == CielRagdollRecovery.RecoveryPhase.Ragdolled)
				{
					this.HoldRecoveryActivity();
					this.UpdateRagdolledPhase();
				}
				else
				{
					this.HoldRecoveryActivity();
					this.UpdateGetUpPhase();
				}
				this.EnforceEmotionLayerSuppression();
				return;
			}
			if (this.ragdoll.isActive)
			{
				this.BeginRecovery();
				return;
			}
			this.SnapshotControlState();
		}

		// Token: 0x060031E7 RID: 12775 RVA: 0x0011A078 File Offset: 0x00118278
		private void EnforceEmotionLayerSuppression()
		{
			if (!this.emotionLayersHeld || this.emotionController == null || this.animator == null || !this.animator.isActiveAndEnabled)
			{
				return;
			}
			if (!this.emotionLayerIndicesResolved)
			{
				this.suppressedConfidentLayerIndex = (string.IsNullOrEmpty(this.emotionController.ConfidentLayerName) ? (-1) : this.animator.GetLayerIndex(this.emotionController.ConfidentLayerName));
				this.suppressedEmbarrassedLayerIndex = (string.IsNullOrEmpty(this.emotionController.EmbarrassedLayerName) ? (-1) : this.animator.GetLayerIndex(this.emotionController.EmbarrassedLayerName));
				this.emotionLayerIndicesResolved = true;
			}
			if (this.suppressedConfidentLayerIndex >= 0)
			{
				this.animator.SetLayerWeight(this.suppressedConfidentLayerIndex, 0f);
			}
			if (this.suppressedEmbarrassedLayerIndex >= 0)
			{
				this.animator.SetLayerWeight(this.suppressedEmbarrassedLayerIndex, 0f);
			}
		}

		// Token: 0x060031E8 RID: 12776 RVA: 0x0011A168 File Offset: 0x00118368
		private void LateUpdate()
		{
			if (this.ragdollEyesForcedClosed)
			{
				this.ApplyRagdollEyeOverrides();
			}
			if (FlyCamera.IsGlobalPauseActive)
			{
				return;
			}
			if (this.phase == CielRagdollRecovery.RecoveryPhase.None)
			{
				this.UpdatePostRecoveryGrounding();
				return;
			}
			this.UpdateCameraTarget();
			if (this.pendingGetUpHipsAlignment && this.ragdoll != null && this.ragdoll.state == vRagdoll.RagdollState.animated)
			{
				this.AlignAnimatedHipsToPhysicalPose();
				this.pendingGetUpHipsAlignment = false;
			}
		}

		// Token: 0x060031E9 RID: 12777 RVA: 0x0011A1D0 File Offset: 0x001183D0
		public void BeginCrashRecovery()
		{
			if (this.phase == CielRagdollRecovery.RecoveryPhase.None)
			{
				this.BeginRecovery();
			}
		}

		// Token: 0x060031EA RID: 12778 RVA: 0x0011A1E0 File Offset: 0x001183E0
		private void BeginRecovery()
		{
			this.ResolveRagdoll();
			if (this.ragdoll == null || this.animator == null || this.character == null)
			{
				return;
			}
			foreach (vVehicleController vVehicleController in Object.FindObjectsOfType<vVehicleController>())
			{
				if (vVehicleController.IsBeingRidden && vVehicleController.CurrentRider == this.character)
				{
					vVehicleController.ForceDismount();
				}
			}
			foreach (SkateboardInteractable skateboardInteractable in Object.FindObjectsOfType<SkateboardInteractable>())
			{
				if (skateboardInteractable.IsBeingWorn && skateboardInteractable.CurrentRider == this.character)
				{
					skateboardInteractable.ForceCrashDismount();
				}
			}
			foreach (SimpleRollerSkatesInteractable simpleRollerSkatesInteractable in Object.FindObjectsOfType<SimpleRollerSkatesInteractable>())
			{
				if (simpleRollerSkatesInteractable.IsBeingWorn && simpleRollerSkatesInteractable.CurrentRider == this.character)
				{
					simpleRollerSkatesInteractable.ForceCrashDismount();
				}
			}
			vBasicCoverController component = base.GetComponent<vBasicCoverController>();
			if (component != null)
			{
				component.InterruptForEncounter();
			}
			this.SnapshotControlState();
			this.CaptureRecoveryGround();
			this.phase = CielRagdollRecovery.RecoveryPhase.Ragdolled;
			this.HoldRecoveryActivity();
			this.ragdollStartedTime = Time.time;
			this.phaseStartedTime = Time.time;
			this.stableTime = 0f;
			this.recoveryRequested = false;
			this.sawGetUpAnimation = false;
			this.getUpAnimationForced = false;
			this.getUpToIdleBlendStarted = false;
			this.CompletedLastGetUpToIdleBlend = false;
			this.LastGetUpNeutralStateResolved = false;
			this.hasPhysicalGetUpHipsPosition = false;
			this.pendingGetUpHipsAlignment = false;
			this.postRecoveryGroundingEndTime = 0f;
			this.ragdoll.CancelInvoke("ResetStayRagdolled");
			this.ragdoll.keepRagdolled = true;
			if (this.input != null)
			{
				this.input.lockInput = true;
			}
			this.DisableAnimationCullingForRecovery();
			this.CaptureFullBodyLayerWeight();
			this.SetBoneInterpolation(RigidbodyInterpolation.Interpolate);
			this.SuppressEmotionLayers();
			this.CloseEyesForRagdoll();
			this.StartRagdollCamera();
			if (this.logRecovery)
			{
				Debug.Log("[CielRagdollRecovery] Recovery tracking started.", this);
			}
		}

		// Token: 0x060031EB RID: 12779 RVA: 0x0011A3DC File Offset: 0x001185DC
		private void UpdateRagdolledPhase()
		{
			if (this.ragdoll.state != vRagdoll.RagdollState.ragdolled)
			{
				this.phase = CielRagdollRecovery.RecoveryPhase.GettingUp;
				this.phaseStartedTime = Time.time;
				return;
			}
			if (this.recoveryRequested)
			{
				return;
			}
			float num = Time.time - this.ragdollStartedTime;
			float recoveryBodySpeed = this.GetRecoveryBodySpeed();
			if (num >= this.minimumRecoveryDelay && recoveryBodySpeed <= this.stableVelocity)
			{
				this.stableTime += Time.deltaTime;
			}
			else
			{
				this.stableTime = 0f;
			}
			if (this.stableTime >= this.stableDuration || num >= this.forcedRecoveryDelay)
			{
				this.RequestGetUp(num >= this.forcedRecoveryDelay);
			}
		}

		// Token: 0x060031EC RID: 12780 RVA: 0x0011A484 File Offset: 0x00118684
		private void RequestGetUp(bool forced)
		{
			this.recoveryRequested = true;
			this.CorrectPhysicalPenetration();
			this.SetBoneInterpolation(RigidbodyInterpolation.None);
			this.physicalGetUpHipsPosition = ((this.hipsBody != null) ? this.hipsBody.position : ((this.hips != null) ? this.hips.position : base.transform.position));
			this.hasPhysicalGetUpHipsPosition = this.hips != null;
			this.getUpFromBack = this.hips != null && this.hips.TransformDirection(this.hipsLocalCharacterForward).y > 0f;
			this.ragdoll.ResetStayRagdolled();
			this.ragdoll.inStabilize = false;
			if (this.logRecovery)
			{
				string text = (forced ? "hard timeout" : "body stable");
				Debug.Log("[CielRagdollRecovery] Get-up requested (" + text + ").", this);
			}
		}

		// Token: 0x060031ED RID: 12781 RVA: 0x0011A578 File Offset: 0x00118778
		private void CacheAnimatorRecoveryData()
		{
			if (this.animator == null)
			{
				return;
			}
			this.fullBodyLayerIndex = this.animator.GetLayerIndex("FullBody");
			if (this.hips != null)
			{
				this.hipsLocalCharacterForward = this.hips.InverseTransformDirection(base.transform.forward);
			}
		}

		// Token: 0x060031EE RID: 12782 RVA: 0x0011A5D4 File Offset: 0x001187D4
		private void ForceResolvedGetUpAnimation()
		{
			if (this.getUpAnimationForced || this.animator == null || !this.animator.isActiveAndEnabled)
			{
				return;
			}
			this.OpenEyesForGetUp();
			this.PlayResolvedGetUpAnimationAtStart();
			this.animator.Update(0f);
			this.AlignAnimatedHipsToPhysicalPose();
			this.pendingGetUpHipsAlignment = true;
			this.getUpAnimationForced = true;
			if (this.logRecovery)
			{
				string text = (this.getUpFromBack ? "back" : "belly");
				Debug.Log("[CielRagdollRecovery] Forced " + text + " get-up on Animator layer " + string.Format("'{0}' ({1}).", "FullBody", this.fullBodyLayerIndex), this);
			}
		}

		// Token: 0x060031EF RID: 12783 RVA: 0x0011A684 File Offset: 0x00118884
		private void AlignAnimatedHipsToPhysicalPose()
		{
			if (!this.hasPhysicalGetUpHipsPosition || this.hips == null)
			{
				return;
			}
			Vector3 vector = this.physicalGetUpHipsPosition - this.hips.position;
			base.transform.position += vector;
			Physics.SyncTransforms();
			if (this.logRecovery)
			{
				Debug.Log(string.Format("[CielRagdollRecovery] Get-up hips realigned by {0}.", vector), this);
			}
		}

		// Token: 0x060031F0 RID: 12784 RVA: 0x0011A6F9 File Offset: 0x001188F9
		private void HoldResolvedGetUpAnimationAtStart()
		{
			if (this.animator == null || !this.animator.isActiveAndEnabled)
			{
				return;
			}
			this.PlayResolvedGetUpAnimationAtStart();
		}

		// Token: 0x060031F1 RID: 12785 RVA: 0x0011A720 File Offset: 0x00118920
		private void PlayResolvedGetUpAnimationAtStart()
		{
			int num = (this.getUpFromBack ? CielRagdollRecovery.GetUpFromBackPathHash : CielRagdollRecovery.GetUpFromBellyPathHash);
			int num2 = (this.getUpFromBack ? CielRagdollRecovery.GetUpFromBackHash : CielRagdollRecovery.GetUpFromBellyHash);
			if (this.fullBodyLayerIndex >= 0 && this.fullBodyLayerIndex < this.animator.layerCount)
			{
				this.animator.SetLayerWeight(this.fullBodyLayerIndex, 1f);
				int num3 = (this.animator.HasState(this.fullBodyLayerIndex, num) ? num : num2);
				this.animator.Play(num3, this.fullBodyLayerIndex, 0f);
				return;
			}
			this.animator.Play(num2, -1, 0f);
		}

		// Token: 0x060031F2 RID: 12786 RVA: 0x0011A7CC File Offset: 0x001189CC
		private void UpdateGetUpPhase()
		{
			if (this.ragdoll.state == vRagdoll.RagdollState.ragdolled)
			{
				this.phase = CielRagdollRecovery.RecoveryPhase.Ragdolled;
				return;
			}
			if (this.ragdoll.state == vRagdoll.RagdollState.blendToAnim)
			{
				this.HoldResolvedGetUpAnimationAtStart();
				return;
			}
			if (!this.getUpAnimationForced)
			{
				this.phaseStartedTime = Time.time;
				this.ForceResolvedGetUpAnimation();
			}
			if (this.getUpToIdleBlendStarted)
			{
				this.UpdateGetUpToIdleBlend();
				return;
			}
			bool flag = false;
			float num = 0f;
			if (this.animator != null && this.animator.isActiveAndEnabled)
			{
				for (int i = 0; i < this.animator.layerCount; i++)
				{
					AnimatorStateInfo currentAnimatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(i);
					if (currentAnimatorStateInfo.shortNameHash == CielRagdollRecovery.GetUpFromBackHash || currentAnimatorStateInfo.shortNameHash == CielRagdollRecovery.GetUpFromBellyHash)
					{
						flag = true;
						this.sawGetUpAnimation = true;
						num = Mathf.Max(num, currentAnimatorStateInfo.normalizedTime);
					}
				}
			}
			bool flag2 = this.character == null || !this.character.ragdolled;
			bool flag3 = this.sawGetUpAnimation && (!flag || num >= this.getUpCompletion);
			float num2 = (this.getUpFromBack ? this.getUpFromBackIdleBlendStart : this.getUpFromBellyIdleBlendStart);
			if (this.sawGetUpAnimation && flag2 && ((flag && num >= num2) || flag3))
			{
				this.BeginGetUpToIdleBlend();
				this.UpdateGetUpToIdleBlend();
				return;
			}
			if (Time.time - this.phaseStartedTime >= this.maximumGetUpDuration)
			{
				if (this.ragdoll.isActive || (this.character != null && this.character.ragdolled))
				{
					this.ragdoll.RestoreRagdoll();
				}
				if (this.sawGetUpAnimation)
				{
					this.BeginGetUpToIdleBlend();
					this.UpdateGetUpToIdleBlend();
					return;
				}
				this.FinishRecovery();
			}
		}

		// Token: 0x060031F3 RID: 12787 RVA: 0x0011A990 File Offset: 0x00118B90
		private void CaptureFullBodyLayerWeight()
		{
			this.fullBodyLayerWeightCaptured = false;
			if (this.animator == null || this.fullBodyLayerIndex < 0 || this.fullBodyLayerIndex >= this.animator.layerCount)
			{
				return;
			}
			this.fullBodyLayerWeightBeforeRecovery = this.animator.GetLayerWeight(this.fullBodyLayerIndex);
			this.fullBodyLayerWeightCaptured = true;
		}

		// Token: 0x060031F4 RID: 12788 RVA: 0x0011A9F0 File Offset: 0x00118BF0
		private void BeginGetUpToIdleBlend()
		{
			if (this.getUpToIdleBlendStarted)
			{
				return;
			}
			this.getUpToIdleBlendStarted = true;
			this.getUpToIdleBlendStartedTime = Time.time;
			this.getUpToIdleInitialLayerWeight = ((this.animator != null && this.fullBodyLayerIndex >= 0 && this.fullBodyLayerIndex < this.animator.layerCount) ? this.animator.GetLayerWeight(this.fullBodyLayerIndex) : 0f);
			if (this.logRecovery)
			{
				string text = (this.getUpFromBack ? "back" : "belly");
				Debug.Log("[CielRagdollRecovery] Blending " + text + " get-up into Ciel locomotion.", this);
			}
		}

		// Token: 0x060031F5 RID: 12789 RVA: 0x0011AA94 File Offset: 0x00118C94
		private void UpdateGetUpToIdleBlend()
		{
			if (!this.getUpToIdleBlendStarted)
			{
				return;
			}
			if (this.animator == null || this.fullBodyLayerIndex < 0 || this.fullBodyLayerIndex >= this.animator.layerCount)
			{
				this.FinishRecovery();
				return;
			}
			float num = Mathf.Max(0.05f, this.getUpToIdleBlendDuration);
			float num2 = Mathf.Clamp01((Time.time - this.getUpToIdleBlendStartedTime) / num);
			this.animator.SetLayerWeight(this.fullBodyLayerIndex, Mathf.Lerp(this.getUpToIdleInitialLayerWeight, 0f, num2));
			if (num2 < 1f)
			{
				return;
			}
			this.LastGetUpNeutralStateResolved = this.PlayFullBodyNeutralState();
			this.animator.Update(0f);
			this.animator.SetLayerWeight(this.fullBodyLayerIndex, (this.LastGetUpNeutralStateResolved && this.fullBodyLayerWeightCaptured) ? this.fullBodyLayerWeightBeforeRecovery : 0f);
			this.CompletedLastGetUpToIdleBlend = true;
			this.FinishRecovery();
		}

		// Token: 0x060031F6 RID: 12790 RVA: 0x0011AB84 File Offset: 0x00118D84
		private bool PlayFullBodyNeutralState()
		{
			foreach (int num in new int[]
			{
				CielRagdollRecovery.FullBodyNeutralPathHash,
				CielRagdollRecovery.FullBodyNeutralTwoLevelPathHash,
				CielRagdollRecovery.FullBodyNeutralOneLevelPathHash
			})
			{
				if (this.animator.HasState(this.fullBodyLayerIndex, num))
				{
					this.animator.Play(num, this.fullBodyLayerIndex, 0f);
					return true;
				}
			}
			Debug.LogWarning("[CielRagdollRecovery] FullBody neutral state was not found; the layer remains visually suppressed to avoid the bad stand-up tail.", this);
			return false;
		}

		// Token: 0x060031F7 RID: 12791 RVA: 0x0011ABFC File Offset: 0x00118DFC
		private void RestoreFullBodyLayerAfterInterruptedRecovery()
		{
			if (this.animator == null || !this.animator.isActiveAndEnabled || this.fullBodyLayerIndex < 0 || this.fullBodyLayerIndex >= this.animator.layerCount || (!this.getUpAnimationForced && !this.getUpToIdleBlendStarted))
			{
				return;
			}
			bool flag = this.PlayFullBodyNeutralState();
			this.animator.Update(0f);
			this.animator.SetLayerWeight(this.fullBodyLayerIndex, (flag && this.fullBodyLayerWeightCaptured) ? this.fullBodyLayerWeightBeforeRecovery : 0f);
		}

		// Token: 0x060031F8 RID: 12792 RVA: 0x0011AC94 File Offset: 0x00118E94
		private void FinishRecovery()
		{
			FlyCamera.StopForRagdollRecovery(this);
			this.OpenEyesForGetUp();
			this.SetBoneInterpolation(RigidbodyInterpolation.None);
			this.SnapAnimatedRootToGround(true);
			this.RestoreControlState();
			this.StopRagdollCamera(true);
			this.RestoreEmotionLayers();
			this.RestoreAnimationCulling();
			this.postRecoveryGroundingEndTime = Time.time + this.postRecoveryGroundingDuration;
			this.phase = CielRagdollRecovery.RecoveryPhase.None;
			this.recoveryRequested = false;
			this.stableTime = 0f;
			if (this.logRecovery)
			{
				Debug.Log("[CielRagdollRecovery] Get-up completed and control restored.", this);
			}
			this.ReleaseRecoveryActivity();
		}

		// Token: 0x060031F9 RID: 12793 RVA: 0x0011AD18 File Offset: 0x00118F18
		private void HoldRecoveryActivity()
		{
			if (this.recoveryActivityReason == null)
			{
				this.recoveryActivityReason = "Ciel.RagdollRecovery#" + base.GetInstanceID().ToString();
			}
			if (!PlayerActivityGate.HasLock(this.recoveryActivityReason))
			{
				PlayerActivityGate.Push(this.recoveryActivityReason, PlayerActivity.Emote | PlayerActivity.Cover | PlayerActivity.Flash | PlayerActivity.RapidHUD | PlayerActivity.Interact | PlayerActivity.StanceSwitch | PlayerActivity.Undress | PlayerActivity.LingerieOpen | PlayerActivity.Inventory | PlayerActivity.Phone | PlayerActivity.Crouch | PlayerActivity.ArmsPose);
			}
		}

		// Token: 0x060031FA RID: 12794 RVA: 0x0011AD68 File Offset: 0x00118F68
		private void ReleaseRecoveryActivity()
		{
			if (this.recoveryActivityReason != null)
			{
				PlayerActivityGate.Pop(this.recoveryActivityReason);
			}
		}

		// Token: 0x060031FB RID: 12795 RVA: 0x0011AD80 File Offset: 0x00118F80
		private void SnapshotControlState()
		{
			if (this.character != null)
			{
				this.previousLockMovement = this.character.lockMovement;
				this.previousLockRotation = this.character.lockRotation;
				this.previousLockAnimMovement = this.character.lockAnimMovement;
				this.previousLockAnimRotation = this.character.lockAnimRotation;
			}
			if (this.input != null)
			{
				this.previousInputLock = this.input.lockInput;
			}
		}

		// Token: 0x060031FC RID: 12796 RVA: 0x0011AE00 File Offset: 0x00119000
		private void RestoreControlState()
		{
			if (this.character != null)
			{
				this.character.lockMovement = this.previousLockMovement;
				this.character.lockRotation = this.previousLockRotation;
				this.character.lockAnimMovement = this.previousLockAnimMovement;
				this.character.lockAnimRotation = this.previousLockAnimRotation;
			}
			if (this.input != null)
			{
				this.input.lockInput = this.previousInputLock;
			}
		}

		// Token: 0x060031FD RID: 12797 RVA: 0x0011AE80 File Offset: 0x00119080
		private float GetRecoveryBodySpeed()
		{
			float num = ((this.hipsBody != null) ? this.hipsBody.velocity.magnitude : 0f);
			float num2 = ((this.chestBody != null) ? this.chestBody.velocity.magnitude : num);
			return Mathf.Max(num, num2);
		}

		// Token: 0x060031FE RID: 12798 RVA: 0x0011AEE4 File Offset: 0x001190E4
		private void CacheHumanoidPhysics()
		{
			this.ragdollBodies.Clear();
			this.ragdollColliders.Clear();
			this.ownColliders.Clear();
			if (this.animator == null || !this.animator.isHuman)
			{
				return;
			}
			Collider[] componentsInChildren = base.GetComponentsInChildren<Collider>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i] != null)
				{
					this.ownColliders.Add(componentsInChildren[i]);
				}
			}
			HumanBodyBones[] array = new HumanBodyBones[]
			{
				HumanBodyBones.Hips,
				HumanBodyBones.Chest,
				HumanBodyBones.Head,
				HumanBodyBones.LeftUpperLeg,
				HumanBodyBones.LeftLowerLeg,
				HumanBodyBones.RightUpperLeg,
				HumanBodyBones.RightLowerLeg,
				HumanBodyBones.LeftUpperArm,
				HumanBodyBones.LeftLowerArm,
				HumanBodyBones.RightUpperArm,
				HumanBodyBones.RightLowerArm
			};
			for (int j = 0; j < array.Length; j++)
			{
				Transform boneTransform = this.animator.GetBoneTransform(array[j]);
				if (!(boneTransform == null))
				{
					Rigidbody component = boneTransform.GetComponent<Rigidbody>();
					if (component != null && !this.ragdollBodies.Contains(component))
					{
						this.ragdollBodies.Add(component);
					}
					Collider[] components = boneTransform.GetComponents<Collider>();
					for (int k = 0; k < components.Length; k++)
					{
						if (components[k] != null)
						{
							this.ragdollColliders.Add(components[k]);
						}
					}
				}
			}
			this.hips = this.animator.GetBoneTransform(HumanBodyBones.Hips);
			this.chest = this.animator.GetBoneTransform(HumanBodyBones.Chest);
			this.hipsBody = ((this.hips != null) ? this.hips.GetComponent<Rigidbody>() : null);
			this.chestBody = ((this.chest != null) ? this.chest.GetComponent<Rigidbody>() : null);
		}

		// Token: 0x060031FF RID: 12799 RVA: 0x0011B070 File Offset: 0x00119270
		private void SetBoneInterpolation(RigidbodyInterpolation interpolation)
		{
			for (int i = 0; i < this.ragdollBodies.Count; i++)
			{
				Rigidbody rigidbody = this.ragdollBodies[i];
				if (rigidbody != null)
				{
					rigidbody.interpolation = interpolation;
				}
			}
		}

		// Token: 0x06003200 RID: 12800 RVA: 0x0011B0B0 File Offset: 0x001192B0
		private void DisableAnimationCullingForRecovery()
		{
			if (this.animationCullingHeld || this.animator == null)
			{
				return;
			}
			this.previousAnimatorCullingMode = this.animator.cullingMode;
			this.animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
			this.rendererOffscreenStates.Clear();
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in base.GetComponentsInChildren<SkinnedMeshRenderer>(true))
			{
				if (!(skinnedMeshRenderer == null) && !this.rendererOffscreenStates.ContainsKey(skinnedMeshRenderer))
				{
					this.rendererOffscreenStates.Add(skinnedMeshRenderer, skinnedMeshRenderer.updateWhenOffscreen);
					skinnedMeshRenderer.updateWhenOffscreen = true;
				}
			}
			this.animationCullingHeld = true;
		}

		// Token: 0x06003201 RID: 12801 RVA: 0x0011B150 File Offset: 0x00119350
		private void RestoreAnimationCulling()
		{
			if (!this.animationCullingHeld)
			{
				return;
			}
			if (this.animator != null)
			{
				this.animator.cullingMode = this.previousAnimatorCullingMode;
			}
			foreach (KeyValuePair<SkinnedMeshRenderer, bool> keyValuePair in this.rendererOffscreenStates)
			{
				if (keyValuePair.Key != null)
				{
					keyValuePair.Key.updateWhenOffscreen = keyValuePair.Value;
				}
			}
			this.rendererOffscreenStates.Clear();
			this.animationCullingHeld = false;
		}

		// Token: 0x06003202 RID: 12802 RVA: 0x0011B1F8 File Offset: 0x001193F8
		private void CorrectPhysicalPenetration()
		{
			float num;
			if (!this.TryGetGroundHeight(this.GetBodyFocusPosition(), out num))
			{
				float num2;
				this.TryRestoreRecoveryGround(true, out num2);
				return;
			}
			float num3 = float.PositiveInfinity;
			foreach (Collider collider in this.ragdollColliders)
			{
				if (collider != null && collider.enabled && !collider.isTrigger)
				{
					num3 = Mathf.Min(num3, collider.bounds.min.y);
				}
			}
			if (float.IsPositiveInfinity(num3) || num3 >= num - this.allowedGroundPenetration)
			{
				return;
			}
			float num4 = num + this.groundLiftPadding - num3;
			for (int i = 0; i < this.ragdollBodies.Count; i++)
			{
				Rigidbody rigidbody = this.ragdollBodies[i];
				if (!(rigidbody == null))
				{
					rigidbody.position += Vector3.up * num4;
					rigidbody.WakeUp();
				}
			}
		}

		// Token: 0x06003203 RID: 12803 RVA: 0x0011B318 File Offset: 0x00119518
		private void CorrectAnimatedRootHeight(bool settleToGround = false)
		{
			float num;
			if (!this.TryGetGroundHeight(base.transform.position, out num))
			{
				return;
			}
			float num2 = num + this.groundLiftPadding;
			if (base.transform.position.y < num - this.allowedGroundPenetration)
			{
				Vector3 position = base.transform.position;
				position.y = num2;
				base.transform.position = position;
				return;
			}
			if (!settleToGround)
			{
				return;
			}
			float num3 = 1f - Mathf.Exp(-this.animatedGroundingSharpness * Time.deltaTime);
			Vector3 position2 = base.transform.position;
			position2.y = Mathf.Lerp(position2.y, num2, num3);
			base.transform.position = position2;
		}

		// Token: 0x06003204 RID: 12804 RVA: 0x0011B3CC File Offset: 0x001195CC
		private void SnapAnimatedRootToGround(bool allowRecoveryGroundFallback = false)
		{
			float num;
			if (this.TryGetGroundHeight(base.transform.position, out num))
			{
				Vector3 position = base.transform.position;
				float y = position.y;
				position.y = num + this.groundLiftPadding;
				base.transform.position = position;
				if (this.logRecovery)
				{
					Debug.Log(string.Format("[CielRagdollRecovery] Final grounding root {0:F3} -> {1:F3}, ground {2:F3}.", y, position.y, num), this);
				}
				return;
			}
			float num2;
			if (allowRecoveryGroundFallback && this.TryRestoreRecoveryGround(false, out num2))
			{
				return;
			}
			if (this.logRecovery)
			{
				Debug.LogWarning("[CielRagdollRecovery] Final grounding found no valid ground.", this);
			}
		}

		// Token: 0x06003205 RID: 12805 RVA: 0x0011B46E File Offset: 0x0011966E
		private void UpdatePostRecoveryGrounding()
		{
			if (this.postRecoveryGroundingEndTime <= 0f)
			{
				return;
			}
			if (Time.time < this.postRecoveryGroundingEndTime)
			{
				this.CorrectAnimatedRootHeight(true);
				return;
			}
			this.SnapAnimatedRootToGround(false);
			this.postRecoveryGroundingEndTime = 0f;
		}

		// Token: 0x06003206 RID: 12806 RVA: 0x0011B4A8 File Offset: 0x001196A8
		private bool TryGetGroundHeight(Vector3 focus, out float groundHeight)
		{
			RaycastHit raycastHit;
			bool flag = this.TryGetGroundHit(focus, out raycastHit, float.PositiveInfinity);
			groundHeight = (flag ? raycastHit.point.y : 0f);
			return flag;
		}

		// Token: 0x06003207 RID: 12807 RVA: 0x0011B4E0 File Offset: 0x001196E0
		private bool TryGetGroundHit(Vector3 focus, out RaycastHit groundHit, float maximumHeight = float.PositiveInfinity)
		{
			groundHit = default(RaycastHit);
			if (!CielRagdollRecovery.IsFinite(focus))
			{
				return false;
			}
			int num = ((this.character != null) ? this.character.groundLayer.value : (-5));
			Vector3 vector = focus + Vector3.up * this.groundProbeHeight;
			RaycastHit[] array = this.groundHits;
			int num2 = Physics.RaycastNonAlloc(vector, Vector3.down, array, this.groundProbeDistance, num, QueryTriggerInteraction.Ignore);
			if (num2 == array.Length)
			{
				array = Physics.RaycastAll(vector, Vector3.down, this.groundProbeDistance, num, QueryTriggerInteraction.Ignore);
				num2 = array.Length;
			}
			bool flag = false;
			float num3 = float.NegativeInfinity;
			for (int i = 0; i < num2; i++)
			{
				RaycastHit raycastHit = array[i];
				if (this.IsValidGroundHit(raycastHit) && raycastHit.point.y <= maximumHeight && raycastHit.point.y > num3)
				{
					num3 = raycastHit.point.y;
					groundHit = raycastHit;
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x06003208 RID: 12808 RVA: 0x0011B5DC File Offset: 0x001197DC
		private bool IsValidGroundHit(RaycastHit hit)
		{
			Collider collider = hit.collider;
			return collider != null && collider.enabled && collider.gameObject.activeInHierarchy && !collider.isTrigger && !this.ownColliders.Contains(collider) && !collider.transform.IsChildOf(base.transform) && CielRagdollRecovery.IsFinite(hit.point) && CielRagdollRecovery.IsFinite(hit.normal) && hit.normal.y > 0.01f;
		}

		// Token: 0x06003209 RID: 12809 RVA: 0x0011B66C File Offset: 0x0011986C
		private static bool IsFinite(Vector3 value)
		{
			return !float.IsNaN(value.x) && !float.IsInfinity(value.x) && !float.IsNaN(value.y) && !float.IsInfinity(value.y) && !float.IsNaN(value.z) && !float.IsInfinity(value.z);
		}

		// Token: 0x0600320A RID: 12810 RVA: 0x0011B6CC File Offset: 0x001198CC
		private void CaptureRecoveryGround()
		{
			this.hasRecoveryGround = false;
			this.recoveryGroundCollider = null;
			RaycastHit raycastHit;
			if (!this.TryGetGroundHit(base.transform.position, out raycastHit, base.transform.position.y + this.allowedGroundPenetration))
			{
				return;
			}
			this.recoveryGroundCollider = raycastHit.collider;
			this.recoveryGroundPoint = raycastHit.point;
			this.hasRecoveryGround = true;
		}

		// Token: 0x0600320B RID: 12811 RVA: 0x0011B738 File Offset: 0x00119938
		private bool TryRestoreRecoveryGround(bool physicalPose, out float groundHeight)
		{
			groundHeight = 0f;
			int num = ((this.character != null) ? this.character.groundLayer.value : (-5));
			if (!this.hasRecoveryGround || this.recoveryGroundCollider == null || (num & (1 << this.recoveryGroundCollider.gameObject.layer)) == 0 || !CielRagdollRecovery.IsFinite(this.recoveryGroundPoint) || !CielRagdollRecovery.IsFinite(base.transform.position))
			{
				return false;
			}
			Ray ray = new Ray(this.recoveryGroundPoint + Vector3.up * this.groundProbeHeight, Vector3.down);
			RaycastHit raycastHit;
			if (!this.recoveryGroundCollider.Raycast(ray, out raycastHit, this.groundProbeDistance) || !this.IsValidGroundHit(raycastHit))
			{
				return false;
			}
			Vector3 vector = (physicalPose ? this.GetBodyFocusPosition() : base.transform.position);
			if (!CielRagdollRecovery.IsFinite(vector))
			{
				return false;
			}
			float num2 = base.transform.position.y;
			if (physicalPose)
			{
				num2 = float.PositiveInfinity;
				foreach (Collider collider in this.ragdollColliders)
				{
					if (collider != null && collider.enabled && !collider.isTrigger)
					{
						num2 = Mathf.Min(num2, collider.bounds.min.y);
					}
				}
				if (float.IsPositiveInfinity(num2))
				{
					num2 = vector.y;
				}
			}
			Vector3 vector2 = new Vector3(raycastHit.point.x - vector.x, raycastHit.point.y + this.groundLiftPadding - num2, raycastHit.point.z - vector.z);
			if (!CielRagdollRecovery.IsFinite(vector2))
			{
				return false;
			}
			this.relocationBodyPositions.Clear();
			for (int i = 0; i < this.ragdollBodies.Count; i++)
			{
				this.relocationBodyPositions.Add((this.ragdollBodies[i] != null) ? this.ragdollBodies[i].position : Vector3.zero);
			}
			base.transform.position += vector2;
			Rigidbody component = base.GetComponent<Rigidbody>();
			if (component != null)
			{
				component.position = base.transform.position;
				CielRagdollRecovery.StopRecoveryVelocity(component);
			}
			for (int j = 0; j < this.ragdollBodies.Count; j++)
			{
				Rigidbody rigidbody = this.ragdollBodies[j];
				if (!(rigidbody == null))
				{
					rigidbody.position = this.relocationBodyPositions[j] + vector2;
					CielRagdollRecovery.StopRecoveryVelocity(rigidbody);
				}
			}
			if (this.hasPhysicalGetUpHipsPosition)
			{
				this.physicalGetUpHipsPosition += vector2;
			}
			if (this.character != null)
			{
				this.character.verticalVelocity = 0f;
				this.character.targetVelocity = Vector3.zero;
			}
			groundHeight = raycastHit.point.y;
			return true;
		}

		// Token: 0x0600320C RID: 12812 RVA: 0x0011BA70 File Offset: 0x00119C70
		private static void StopRecoveryVelocity(Rigidbody body)
		{
			if (body.isKinematic)
			{
				return;
			}
			body.velocity = Vector3.zero;
			body.angularVelocity = Vector3.zero;
		}

		// Token: 0x0600320D RID: 12813 RVA: 0x0011BA94 File Offset: 0x00119C94
		private Vector3 GetBodyFocusPosition()
		{
			if (this.hips != null && this.chest != null)
			{
				return Vector3.Lerp(this.hips.position, this.chest.position, 0.5f);
			}
			if (this.hips != null)
			{
				return this.hips.position;
			}
			return base.transform.position + Vector3.up * 0.5f;
		}

		// Token: 0x0600320E RID: 12814 RVA: 0x0011BB18 File Offset: 0x00119D18
		private void StartRagdollCamera()
		{
			if (this.IsFirstPersonCameraActive() || this.ragdollCameraProfileHeld)
			{
				return;
			}
			if (this.input != null)
			{
				this.thirdPersonCamera = this.input.tpCamera;
			}
			if (this.thirdPersonCamera == null)
			{
				return;
			}
			this.previousIgnoreTpCamera = this.input != null && this.input.ignoreTpCamera;
			if (this.input != null)
			{
				this.input.ignoreTpCamera = true;
			}
			this.previousCameraStateName = ((!string.IsNullOrEmpty(this.thirdPersonCamera.currentStateName)) ? this.thirdPersonCamera.currentStateName : ((this.thirdPersonCamera.currentState != null) ? this.thirdPersonCamera.currentState.Name : "Default"));
			this.previousCameraSmoothBetweenState = this.thirdPersonCamera.smoothBetweenState;
			this.EnsureCameraTarget();
			this.previousCameraTarget = this.thirdPersonCamera.mainTarget;
			this.cameraTarget.position = this.GetCameraTargetPosition();
			this.cameraTarget.rotation = Quaternion.Euler(0f, base.transform.eulerAngles.y, 0f);
			this.cameraTarget.gameObject.SetActive(true);
			this.thirdPersonCamera.SetMainTarget(this.cameraTarget);
			this.ragdollCameraState = new vThirdPersonCameraState("__CielRagdollRuntime");
			if (this.thirdPersonCamera.currentState != null)
			{
				this.ragdollCameraState.CopyState(this.thirdPersonCamera.currentState);
			}
			else if (this.thirdPersonCamera.lerpState != null)
			{
				this.ragdollCameraState.CopyState(this.thirdPersonCamera.lerpState);
			}
			this.ragdollCameraState.Name = "__CielRagdollRuntime";
			this.ragdollCameraState.defaultDistance = this.ragdollCameraDistance;
			this.ragdollCameraState.minDistance = this.ragdollCameraDistance;
			this.ragdollCameraState.maxDistance = this.ragdollCameraDistance;
			this.ragdollCameraState.height = this.ragdollCameraHeight;
			this.ragdollCameraState.cullingHeight = Mathf.Max(this.ragdollCameraHeight, this.minimumCameraTargetHeight);
			this.ragdollCameraState.cullingMinDist = 0.1f;
			this.ragdollCameraState.fov = this.ragdollCameraFieldOfView;
			this.ragdollCameraState.useZoom = false;
			this.ragdollCameraState.smooth = this.ragdollCameraTransitionSharpness;
			this.thirdPersonCamera.smoothBetweenState = this.ragdollCameraTransitionSharpness;
			this.thirdPersonCamera.lerpState = this.ragdollCameraState;
			this.thirdPersonCamera.useSmooth = true;
			this.thirdPersonCamera.ResetAngle();
			this.ragdollCameraProfileHeld = true;
		}

		// Token: 0x0600320F RID: 12815 RVA: 0x0011BDB4 File Offset: 0x00119FB4
		private void UpdateCameraTarget()
		{
			if (this.IsFirstPersonCameraActive())
			{
				if (this.ragdollCameraProfileHeld)
				{
					this.StopRagdollCamera(false);
				}
				return;
			}
			if (!this.ragdollCameraProfileHeld)
			{
				this.StartRagdollCamera();
			}
			if (this.cameraTarget == null || this.thirdPersonCamera == null)
			{
				return;
			}
			Vector3 cameraTargetPosition = this.GetCameraTargetPosition();
			float num = 1f - Mathf.Exp(-this.cameraFollowSharpness * Time.unscaledDeltaTime);
			this.cameraTarget.position = Vector3.Lerp(this.cameraTarget.position, cameraTargetPosition, num);
			this.cameraTarget.rotation = Quaternion.Euler(0f, base.transform.eulerAngles.y, 0f);
		}

		// Token: 0x06003210 RID: 12816 RVA: 0x0011BE6C File Offset: 0x0011A06C
		private Vector3 GetCameraTargetPosition()
		{
			Vector3 bodyFocusPosition = this.GetBodyFocusPosition();
			float num;
			if (this.TryGetGroundHeight(bodyFocusPosition, out num))
			{
				bodyFocusPosition.y = Mathf.Max(bodyFocusPosition.y, num + this.minimumCameraTargetHeight);
			}
			return bodyFocusPosition;
		}

		// Token: 0x06003211 RID: 12817 RVA: 0x0011BEA8 File Offset: 0x0011A0A8
		private bool IsFirstPersonCameraActive()
		{
			if (this.firstPerson == null)
			{
				this.firstPerson = base.GetComponent<FirstPersonController>();
			}
			return this.firstPerson != null && this.firstPerson.isActiveAndEnabled && this.firstPerson.IsFirstPerson;
		}

		// Token: 0x06003212 RID: 12818 RVA: 0x0011BEF8 File Offset: 0x0011A0F8
		private void StopRagdollCamera(bool restoreCameraState = true)
		{
			if (!this.ragdollCameraProfileHeld)
			{
				return;
			}
			if (this.thirdPersonCamera != null)
			{
				Transform transform = ((this.previousCameraTarget != null && this.previousCameraTarget != this.cameraTarget) ? this.previousCameraTarget : base.transform);
				this.thirdPersonCamera.SetMainTarget(transform);
				this.thirdPersonCamera.smoothBetweenState = this.previousCameraSmoothBetweenState;
				if (restoreCameraState)
				{
					this.thirdPersonCamera.ChangeState(string.IsNullOrEmpty(this.previousCameraStateName) ? "Default" : this.previousCameraStateName, true);
				}
			}
			if (this.input != null && this.ragdollCameraProfileHeld)
			{
				this.input.ignoreTpCamera = this.previousIgnoreTpCamera;
			}
			if (this.cameraTarget != null)
			{
				this.cameraTarget.gameObject.SetActive(false);
			}
			this.previousCameraTarget = null;
			this.previousCameraStateName = null;
			this.ragdollCameraState = null;
			this.ragdollCameraProfileHeld = false;
		}

		// Token: 0x06003213 RID: 12819 RVA: 0x0011BFF4 File Offset: 0x0011A1F4
		private void EnsureCameraTarget()
		{
			if (this.cameraTarget != null)
			{
				return;
			}
			this.cameraTarget = new GameObject("__CielRagdollCameraTarget")
			{
				hideFlags = HideFlags.DontSave
			}.transform;
		}

		// Token: 0x06003214 RID: 12820 RVA: 0x0011C030 File Offset: 0x0011A230
		private void SuppressEmotionLayers()
		{
			if (this.emotionLayersHeld)
			{
				return;
			}
			if (this.emotionController == null)
			{
				this.emotionController = base.GetComponent<PlayerEmotionController>();
			}
			if (this.emotionController == null)
			{
				this.emotionController = PlayerEmotionController.Instance;
			}
			if (this.emotionController != null)
			{
				this.emotionController.DisableEmotionLayersForAction();
				this.emotionLayersHeld = true;
			}
		}

		// Token: 0x06003215 RID: 12821 RVA: 0x0011C099 File Offset: 0x0011A299
		private void RestoreEmotionLayers()
		{
			if (!this.emotionLayersHeld)
			{
				return;
			}
			if (this.emotionController != null)
			{
				this.emotionController.RestoreEmotionLayers();
			}
			this.emotionLayersHeld = false;
		}

		// Token: 0x06003216 RID: 12822 RVA: 0x0011C0C4 File Offset: 0x0011A2C4
		private void CloseEyesForRagdoll()
		{
			if (this.ragdollEyesForcedClosed)
			{
				return;
			}
			this.ragdollEyeOverrides.Clear();
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in base.GetComponentsInChildren<SkinnedMeshRenderer>(true))
			{
				Mesh mesh = ((skinnedMeshRenderer != null) ? skinnedMeshRenderer.sharedMesh : null);
				if (!(mesh == null))
				{
					this.CacheEyeBlendShapes(skinnedMeshRenderer, mesh, CielRagdollRecovery.ClosedEyeBlendShapes, 100f);
					this.CacheEyeBlendShapes(skinnedMeshRenderer, mesh, CielRagdollRecovery.NeutralizedEyeBlendShapes, 0f);
				}
			}
			this.ragdollEyesForcedClosed = this.ragdollEyeOverrides.Count > 0;
			this.ApplyRagdollEyeOverrides();
			if (this.logRecovery && !this.ragdollEyesForcedClosed)
			{
				Debug.LogWarning("[CielRagdollRecovery] No compatible eye blend shapes were found.", this);
			}
		}

		// Token: 0x06003217 RID: 12823 RVA: 0x0011C174 File Offset: 0x0011A374
		private void CacheEyeBlendShapes(SkinnedMeshRenderer renderer, Mesh mesh, string[] blendShapeNames, float forcedWeight)
		{
			for (int i = 0; i < blendShapeNames.Length; i++)
			{
				int blendShapeIndex = mesh.GetBlendShapeIndex(blendShapeNames[i]);
				if (blendShapeIndex >= 0)
				{
					this.ragdollEyeOverrides.Add(new CielRagdollRecovery.EyeBlendShapeOverride
					{
						Renderer = renderer,
						Index = blendShapeIndex,
						PreviousWeight = renderer.GetBlendShapeWeight(blendShapeIndex),
						ForcedWeight = forcedWeight
					});
				}
			}
		}

		// Token: 0x06003218 RID: 12824 RVA: 0x0011C1D0 File Offset: 0x0011A3D0
		private void ApplyRagdollEyeOverrides()
		{
			for (int i = 0; i < this.ragdollEyeOverrides.Count; i++)
			{
				CielRagdollRecovery.EyeBlendShapeOverride eyeBlendShapeOverride = this.ragdollEyeOverrides[i];
				if (eyeBlendShapeOverride.Renderer != null)
				{
					eyeBlendShapeOverride.Renderer.SetBlendShapeWeight(eyeBlendShapeOverride.Index, eyeBlendShapeOverride.ForcedWeight);
				}
			}
		}

		// Token: 0x06003219 RID: 12825 RVA: 0x0011C228 File Offset: 0x0011A428
		private void OpenEyesForGetUp()
		{
			if (!this.ragdollEyesForcedClosed)
			{
				return;
			}
			for (int i = 0; i < this.ragdollEyeOverrides.Count; i++)
			{
				CielRagdollRecovery.EyeBlendShapeOverride eyeBlendShapeOverride = this.ragdollEyeOverrides[i];
				if (eyeBlendShapeOverride.Renderer != null)
				{
					eyeBlendShapeOverride.Renderer.SetBlendShapeWeight(eyeBlendShapeOverride.Index, 0f);
				}
			}
			this.ragdollEyeOverrides.Clear();
			this.ragdollEyesForcedClosed = false;
		}

		// Token: 0x0600321A RID: 12826 RVA: 0x0011C298 File Offset: 0x0011A498
		private void RestoreRagdollEyes()
		{
			if (!this.ragdollEyesForcedClosed)
			{
				return;
			}
			for (int i = 0; i < this.ragdollEyeOverrides.Count; i++)
			{
				CielRagdollRecovery.EyeBlendShapeOverride eyeBlendShapeOverride = this.ragdollEyeOverrides[i];
				if (eyeBlendShapeOverride.Renderer != null)
				{
					eyeBlendShapeOverride.Renderer.SetBlendShapeWeight(eyeBlendShapeOverride.Index, eyeBlendShapeOverride.PreviousWeight);
				}
			}
			this.ragdollEyeOverrides.Clear();
			this.ragdollEyesForcedClosed = false;
		}

		// Token: 0x0600321B RID: 12827 RVA: 0x0011C308 File Offset: 0x0011A508
		private void ResolveRagdoll()
		{
			if (this.ragdoll == null)
			{
				this.ragdoll = ((this.bootstrap != null) ? this.bootstrap.Ragdoll : base.GetComponent<vRagdoll>());
			}
		}

		// Token: 0x0600321C RID: 12828 RVA: 0x0011C33F File Offset: 0x0011A53F
		private void OnDisable()
		{
			WorldOrigin.Shifted -= this.ApplyWorldOriginShift;
			this.InterruptForUnstuck();
		}

		// Token: 0x0600321D RID: 12829 RVA: 0x0011C358 File Offset: 0x0011A558
		public void InterruptForUnstuck()
		{
			FlyCamera.StopForRagdollRecovery(this);
			this.postRecoveryGroundingEndTime = 0f;
			this.hasRecoveryGround = false;
			this.recoveryGroundCollider = null;
			this.RestoreRagdollEyes();
			this.RestoreFullBodyLayerAfterInterruptedRecovery();
			this.ResolveRagdoll();
			bool flag = this.phase > CielRagdollRecovery.RecoveryPhase.None;
			if (flag || (this.ragdoll != null && (this.ragdoll.isActive || this.ragdoll.state == vRagdoll.RagdollState.blendToAnim)))
			{
				if (this.ragdoll != null)
				{
					this.ragdoll.StopAllCoroutines();
					this.ragdoll.CancelInvoke("ResetStayRagdolled");
					this.ragdoll.ResetStayRagdolled();
					this.ragdoll.RestoreRagdoll();
					if (this.ragdoll.state == vRagdoll.RagdollState.blendToAnim)
					{
						this.ragdoll.state = vRagdoll.RagdollState.animated;
						this.ragdoll.inStabilize = false;
						vThirdPersonController vThirdPersonController = this.character;
						if (vThirdPersonController != null)
						{
							vThirdPersonController.ResetRagdoll();
						}
					}
				}
				this.StopRagdollCamera(true);
				this.RestoreEmotionLayers();
				this.SetBoneInterpolation(RigidbodyInterpolation.None);
				if (flag)
				{
					this.RestoreControlState();
				}
				this.phase = CielRagdollRecovery.RecoveryPhase.None;
				this.recoveryRequested = false;
			}
			this.RestoreAnimationCulling();
			this.ReleaseRecoveryActivity();
		}

		// Token: 0x0600321E RID: 12830 RVA: 0x0011C47F File Offset: 0x0011A67F
		private void OnDestroy()
		{
			this.ReleaseRecoveryActivity();
			this.RestoreRagdollEyes();
			if (this.cameraTarget != null)
			{
				Object.Destroy(this.cameraTarget.gameObject);
			}
		}

		// Token: 0x04003280 RID: 12928
		[Header("Automatic recovery")]
		[SerializeField]
		[Min(0f)]
		private float minimumRecoveryDelay = 2.25f;

		// Token: 0x04003281 RID: 12929
		[SerializeField]
		[Min(0f)]
		private float stableVelocity = 0.45f;

		// Token: 0x04003282 RID: 12930
		[SerializeField]
		[Min(0f)]
		private float stableDuration = 0.55f;

		// Token: 0x04003283 RID: 12931
		[SerializeField]
		[Min(0f)]
		private float forcedRecoveryDelay = 5.5f;

		// Token: 0x04003284 RID: 12932
		[SerializeField]
		[Min(0f)]
		private float maximumGetUpDuration = 4f;

		// Token: 0x04003285 RID: 12933
		[SerializeField]
		[Range(0.5f, 1f)]
		private float getUpCompletion = 1f;

		// Token: 0x04003286 RID: 12934
		[Tooltip("The Invector clips become visibly unnatural near their endings. Blend to Ciel's live locomotion before that tail is shown.")]
		[SerializeField]
		[Range(0.5f, 0.95f)]
		private float getUpFromBackIdleBlendStart = 0.72f;

		// Token: 0x04003287 RID: 12935
		[SerializeField]
		[Range(0.5f, 0.95f)]
		private float getUpFromBellyIdleBlendStart = 0.72f;

		// Token: 0x04003288 RID: 12936
		[SerializeField]
		[Min(0.05f)]
		private float getUpToIdleBlendDuration = 0.18f;

		// Token: 0x04003289 RID: 12937
		[Header("Ground safety")]
		[SerializeField]
		[Min(0.1f)]
		private float groundProbeHeight = 2.5f;

		// Token: 0x0400328A RID: 12938
		[SerializeField]
		[Min(0.1f)]
		private float groundProbeDistance = 6f;

		// Token: 0x0400328B RID: 12939
		[SerializeField]
		[Min(0f)]
		private float allowedGroundPenetration = 0.04f;

		// Token: 0x0400328C RID: 12940
		[SerializeField]
		[Min(0f)]
		private float groundLiftPadding = 0.025f;

		// Token: 0x0400328D RID: 12941
		[SerializeField]
		[Min(0f)]
		private float animatedGroundingSharpness = 20f;

		// Token: 0x0400328E RID: 12942
		[SerializeField]
		[Min(0f)]
		private float postRecoveryGroundingDuration = 0.35f;

		// Token: 0x0400328F RID: 12943
		[Header("Ragdoll camera")]
		[SerializeField]
		[Min(0f)]
		private float cameraFollowSharpness = 12f;

		// Token: 0x04003290 RID: 12944
		[SerializeField]
		[Min(0f)]
		private float minimumCameraTargetHeight = 0.28f;

		// Token: 0x04003291 RID: 12945
		[SerializeField]
		[Min(0.5f)]
		private float ragdollCameraDistance = 1.75f;

		// Token: 0x04003292 RID: 12946
		[SerializeField]
		[Min(0f)]
		private float ragdollCameraHeight = 0.18f;

		// Token: 0x04003293 RID: 12947
		[SerializeField]
		[Range(30f, 90f)]
		private float ragdollCameraFieldOfView = 55f;

		// Token: 0x04003294 RID: 12948
		[SerializeField]
		[Min(0f)]
		private float ragdollCameraTransitionSharpness = 14f;

		// Token: 0x04003295 RID: 12949
		[Header("Debug")]
		[SerializeField]
		private bool logRecovery;

		// Token: 0x04003296 RID: 12950
		private const string FullBodyLayerName = "FullBody";

		// Token: 0x04003297 RID: 12951
		private const string GetUpFromBackPath = "FullBody.StandUpFromBack.StandUp@FromBack";

		// Token: 0x04003298 RID: 12952
		private const string GetUpFromBellyPath = "FullBody.StandUpFromBelly.StandUp@FromBelly";

		// Token: 0x04003299 RID: 12953
		private const string FullBodyNeutralPath = "FullBody.Null.Null.Null";

		// Token: 0x0400329A RID: 12954
		private static readonly int GetUpFromBackHash = Animator.StringToHash("StandUp@FromBack");

		// Token: 0x0400329B RID: 12955
		private static readonly int GetUpFromBellyHash = Animator.StringToHash("StandUp@FromBelly");

		// Token: 0x0400329C RID: 12956
		private static readonly int GetUpFromBackPathHash = Animator.StringToHash("FullBody.StandUpFromBack.StandUp@FromBack");

		// Token: 0x0400329D RID: 12957
		private static readonly int GetUpFromBellyPathHash = Animator.StringToHash("FullBody.StandUpFromBelly.StandUp@FromBelly");

		// Token: 0x0400329E RID: 12958
		private static readonly int FullBodyNeutralPathHash = Animator.StringToHash("FullBody.Null.Null.Null");

		// Token: 0x0400329F RID: 12959
		private static readonly int FullBodyNeutralTwoLevelPathHash = Animator.StringToHash("FullBody.Null.Null");

		// Token: 0x040032A0 RID: 12960
		private static readonly int FullBodyNeutralOneLevelPathHash = Animator.StringToHash("FullBody.Null");

		// Token: 0x040032A1 RID: 12961
		private static readonly string[] ClosedEyeBlendShapes = new string[] { "eye_close_sleep_L", "eye_close_sleep_R" };

		// Token: 0x040032A2 RID: 12962
		private static readonly string[] NeutralizedEyeBlendShapes = new string[]
		{
			"eye_zito_L", "eye_zito_R", "eye_zito2_L", "eye_zito2_R", "eye_pleasure_L", "eye_pleasure_R", "eye_under_up_L", "eye_under_up_R", "vrc.blink", "blink",
			"eye_close_smile_L", "eye_close_smile_R", "eye_close_nagomi_L", "eye_close_nagomi_R", "eye_close_nagomi2_L", "eye_close_nagomi2_R", "eye_surprise_L", "eye_surprise_R", "eye_sad_L", "eye_sad_R",
			"eye_angry_L", "eye_angry_R", "eye_tare_L", "eye_tare_R", "eye_><", "eye_under_down_L", "eye_under_down_R", "eye_under_down2_L", "eye_under_down2_R", "eye_under_up2_L",
			"eye_under_up2_R", "eye_under_up3_L", "eye_under_up3_R"
		};

		// Token: 0x040032A3 RID: 12963
		private readonly List<Rigidbody> ragdollBodies = new List<Rigidbody>();

		// Token: 0x040032A4 RID: 12964
		private readonly HashSet<Collider> ragdollColliders = new HashSet<Collider>();

		// Token: 0x040032A5 RID: 12965
		private readonly HashSet<Collider> ownColliders = new HashSet<Collider>();

		// Token: 0x040032A6 RID: 12966
		private readonly RaycastHit[] groundHits = new RaycastHit[32];

		// Token: 0x040032A7 RID: 12967
		private readonly List<Vector3> relocationBodyPositions = new List<Vector3>(16);

		// Token: 0x040032A8 RID: 12968
		private Collider recoveryGroundCollider;

		// Token: 0x040032A9 RID: 12969
		private Vector3 recoveryGroundPoint;

		// Token: 0x040032AA RID: 12970
		private bool hasRecoveryGround;

		// Token: 0x040032AB RID: 12971
		private readonly Dictionary<SkinnedMeshRenderer, bool> rendererOffscreenStates = new Dictionary<SkinnedMeshRenderer, bool>();

		// Token: 0x040032AC RID: 12972
		private readonly List<CielRagdollRecovery.EyeBlendShapeOverride> ragdollEyeOverrides = new List<CielRagdollRecovery.EyeBlendShapeOverride>();

		// Token: 0x040032AD RID: 12973
		private CielRagdollBootstrap bootstrap;

		// Token: 0x040032AE RID: 12974
		private vRagdoll ragdoll;

		// Token: 0x040032AF RID: 12975
		private vThirdPersonController character;

		// Token: 0x040032B0 RID: 12976
		private vThirdPersonInput input;

		// Token: 0x040032B1 RID: 12977
		private Animator animator;

		// Token: 0x040032B2 RID: 12978
		private Transform hips;

		// Token: 0x040032B3 RID: 12979
		private Transform chest;

		// Token: 0x040032B4 RID: 12980
		private Rigidbody hipsBody;

		// Token: 0x040032B5 RID: 12981
		private Rigidbody chestBody;

		// Token: 0x040032B6 RID: 12982
		private vThirdPersonCamera thirdPersonCamera;

		// Token: 0x040032B7 RID: 12983
		private Transform cameraTarget;

		// Token: 0x040032B8 RID: 12984
		private Transform previousCameraTarget;

		// Token: 0x040032B9 RID: 12985
		private vThirdPersonCameraState ragdollCameraState;

		// Token: 0x040032BA RID: 12986
		private string previousCameraStateName;

		// Token: 0x040032BB RID: 12987
		private float previousCameraSmoothBetweenState;

		// Token: 0x040032BC RID: 12988
		private bool previousIgnoreTpCamera;

		// Token: 0x040032BD RID: 12989
		private bool ragdollCameraProfileHeld;

		// Token: 0x040032BE RID: 12990
		private PlayerEmotionController emotionController;

		// Token: 0x040032BF RID: 12991
		private CielRagdollRecovery.RecoveryPhase phase;

		// Token: 0x040032C0 RID: 12992
		private float ragdollStartedTime;

		// Token: 0x040032C1 RID: 12993
		private float phaseStartedTime;

		// Token: 0x040032C2 RID: 12994
		private float stableTime;

		// Token: 0x040032C3 RID: 12995
		private bool recoveryRequested;

		// Token: 0x040032C4 RID: 12996
		private bool sawGetUpAnimation;

		// Token: 0x040032C5 RID: 12997
		private bool emotionLayersHeld;

		// Token: 0x040032C6 RID: 12998
		private bool emotionLayerIndicesResolved;

		// Token: 0x040032C7 RID: 12999
		private int suppressedConfidentLayerIndex = -1;

		// Token: 0x040032C8 RID: 13000
		private int suppressedEmbarrassedLayerIndex = -1;

		// Token: 0x040032C9 RID: 13001
		private bool getUpFromBack;

		// Token: 0x040032CA RID: 13002
		private bool getUpAnimationForced;

		// Token: 0x040032CB RID: 13003
		private bool animationCullingHeld;

		// Token: 0x040032CC RID: 13004
		private float postRecoveryGroundingEndTime;

		// Token: 0x040032CD RID: 13005
		private int fullBodyLayerIndex = -1;

		// Token: 0x040032CE RID: 13006
		private Vector3 hipsLocalCharacterForward;

		// Token: 0x040032CF RID: 13007
		private Vector3 physicalGetUpHipsPosition;

		// Token: 0x040032D0 RID: 13008
		private bool hasPhysicalGetUpHipsPosition;

		// Token: 0x040032D1 RID: 13009
		private bool pendingGetUpHipsAlignment;

		// Token: 0x040032D2 RID: 13010
		private AnimatorCullingMode previousAnimatorCullingMode;

		// Token: 0x040032D3 RID: 13011
		private bool ragdollEyesForcedClosed;

		// Token: 0x040032D4 RID: 13012
		private bool getUpToIdleBlendStarted;

		// Token: 0x040032D5 RID: 13013
		private float getUpToIdleBlendStartedTime;

		// Token: 0x040032D6 RID: 13014
		private float getUpToIdleInitialLayerWeight;

		// Token: 0x040032D7 RID: 13015
		private float fullBodyLayerWeightBeforeRecovery = 1f;

		// Token: 0x040032D8 RID: 13016
		private bool fullBodyLayerWeightCaptured;

		// Token: 0x040032D9 RID: 13017
		private bool previousInputLock;

		// Token: 0x040032DA RID: 13018
		private bool previousLockMovement;

		// Token: 0x040032DB RID: 13019
		private bool previousLockRotation;

		// Token: 0x040032DC RID: 13020
		private bool previousLockAnimMovement;

		// Token: 0x040032DD RID: 13021
		private bool previousLockAnimRotation;

		// Token: 0x040032DE RID: 13022
		private FirstPersonController firstPerson;

		// Token: 0x040032DF RID: 13023
		private string recoveryActivityReason;

		// Token: 0x02000778 RID: 1912
		private sealed class EyeBlendShapeOverride
		{
			// Token: 0x040032E2 RID: 13026
			public SkinnedMeshRenderer Renderer;

			// Token: 0x040032E3 RID: 13027
			public int Index;

			// Token: 0x040032E4 RID: 13028
			public float PreviousWeight;

			// Token: 0x040032E5 RID: 13029
			public float ForcedWeight;
		}

		// Token: 0x02000779 RID: 1913
		private enum RecoveryPhase
		{
			// Token: 0x040032E7 RID: 13031
			None,
			// Token: 0x040032E8 RID: 13032
			Ragdolled,
			// Token: 0x040032E9 RID: 13033
			GettingUp
		}
	}
}
