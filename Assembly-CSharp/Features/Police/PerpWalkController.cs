using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Features.DebugTools;
using Features.Detection;
using Features.GameOver;
using Features.MasturbationSystem;
using Features.NPC;
using Features.Suspicion;
using Invector.vCharacterController;
using Pathfinding;
using Pathfinding.ECS.RVO;
using ProjectExhibitionist;
using UnityEngine;

namespace Features.Police
{
	// Token: 0x02000DEB RID: 3563
	[DisallowMultipleComponent]
	public class PerpWalkController : MonoBehaviour
	{
		// Token: 0x1700115F RID: 4447
		// (get) Token: 0x0600698D RID: 27021 RVA: 0x002A409E File Offset: 0x002A229E
		// (set) Token: 0x0600698E RID: 27022 RVA: 0x002A40A5 File Offset: 0x002A22A5
		public static PerpWalkController Instance { get; private set; }

		// Token: 0x17001160 RID: 4448
		// (get) Token: 0x0600698F RID: 27023 RVA: 0x002A40AD File Offset: 0x002A22AD
		// (set) Token: 0x06006990 RID: 27024 RVA: 0x002A40B5 File Offset: 0x002A22B5
		public bool IsActive { get; private set; }

		// Token: 0x17001161 RID: 4449
		// (get) Token: 0x06006991 RID: 27025 RVA: 0x002A40BE File Offset: 0x002A22BE
		public DetectionCameraController ActiveCameraController
		{
			get
			{
				if (!this.IsActive)
				{
					return null;
				}
				return this.detectionCameraCtrl;
			}
		}

		// Token: 0x140000AE RID: 174
		// (add) Token: 0x06006992 RID: 27026 RVA: 0x002A40D0 File Offset: 0x002A22D0
		// (remove) Token: 0x06006993 RID: 27027 RVA: 0x002A4104 File Offset: 0x002A2304
		public static event Action<bool> OnPerpWalkStarted;

		// Token: 0x140000AF RID: 175
		// (add) Token: 0x06006994 RID: 27028 RVA: 0x002A4138 File Offset: 0x002A2338
		// (remove) Token: 0x06006995 RID: 27029 RVA: 0x002A416C File Offset: 0x002A236C
		public static event Action OnPerpWalkEnded;

		// Token: 0x06006996 RID: 27030 RVA: 0x002A41A0 File Offset: 0x002A23A0
		private void Awake()
		{
			if (PerpWalkController.Instance != null && PerpWalkController.Instance != this)
			{
				Debug.LogWarning("[PerpWalkController] Awake: duplicate instance on '" + base.name + "', destroying.");
				Object.Destroy(this);
				return;
			}
			PerpWalkController.Instance = this;
			if (this.playerInput == null)
			{
				this.playerInput = base.GetComponentInChildren<vThirdPersonInput>();
			}
			if (this.playerTransform == null && this.playerInput != null)
			{
				this.playerTransform = this.playerInput.transform;
			}
		}

		// Token: 0x06006997 RID: 27031 RVA: 0x002A4235 File Offset: 0x002A2435
		private void OnDestroy()
		{
			if (PerpWalkController.Instance == this)
			{
				PerpWalkController.Instance = null;
			}
		}

		// Token: 0x06006998 RID: 27032 RVA: 0x002A424C File Offset: 0x002A244C
		public void BeginPerpWalk(NPCSenses captor, IList<NPCSenses> additionalEscort)
		{
			if (this.IsActive)
			{
				if (this.debugLogs)
				{
					Debug.Log("[PerpWalkController] BeginPerpWalk ignored: already active");
				}
				return;
			}
			if (this.playerTransform == null)
			{
				Debug.LogWarning("[PerpWalkController] BeginPerpWalk: playerTransform null, aborting.");
				return;
			}
			if (captor == null)
			{
				Debug.LogWarning("[PerpWalkController] BeginPerpWalk: captor null, aborting.");
				return;
			}
			this.IsActive = true;
			this._captor = captor;
			this._escortMembers.Clear();
			Action<bool> onPerpWalkStarted = PerpWalkController.OnPerpWalkStarted;
			if (onPerpWalkStarted != null)
			{
				onPerpWalkStarted(false);
			}
			if (GameOverManager.Instance != null)
			{
				GameOverManager.Instance.TriggerCapture(this);
			}
			if (additionalEscort != null)
			{
				foreach (NPCSenses npcsenses in additionalEscort)
				{
					if (npcsenses != null && npcsenses != captor)
					{
						this._escortMembers.Add(npcsenses);
					}
				}
			}
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[PerpWalkController] BeginPerpWalk: captor={0}, escort={1}", captor.name, this._escortMembers.Count));
			}
			this.DisableAllNPCDetection();
			AttentionUIController.GlobalForceHideAll = true;
			if (PoliceManager.Instance != null)
			{
				PoliceManager.Instance.enabled = false;
				if (this.debugLogs)
				{
					Debug.Log("[PerpWalkController] PoliceManager disabled for perp walk");
				}
			}
			PerpWalkController.DisableEscortAutonomy(this._captor);
			foreach (NPCSenses npcsenses2 in this._escortMembers)
			{
				PerpWalkController.DisableEscortAutonomy(npcsenses2);
			}
			if (this.playerInput != null)
			{
				this.playerInput.SetLockAllInput(true);
			}
			this.HideHUD(true);
			this.ActivatePerpCamera(this.cameraPresets, Camera.main);
			this.SwitchToFollowerLocomotion();
			if (this.handcuffsProp != null)
			{
				this.handcuffsProp.gameObject.SetActive(true);
			}
			this.LockDownPlayerForPerpWalk();
			this.ApplyHandcuffsAnimation();
			Vector3 vector;
			if (this.TryPickRandomPOI(out vector))
			{
				this._currentPoi = vector;
			}
			else
			{
				this._currentPoi = this.playerTransform.position + this.playerTransform.forward * this.poiMinDistance;
			}
			if (this._playerFollower != null && this._playerFollower.entityExists)
			{
				this._playerFollower.destination = this._currentPoi;
			}
			this._walkRoutine = base.StartCoroutine(this.WalkRoutine());
		}

		// Token: 0x06006999 RID: 27033 RVA: 0x002A44C8 File Offset: 0x002A26C8
		public void BeginPerpRun()
		{
			if (this.IsActive)
			{
				if (this.debugLogs)
				{
					Debug.Log("[PerpWalkController] BeginPerpRun ignored: already active");
				}
				return;
			}
			if (this.playerTransform == null)
			{
				Debug.LogWarning("[PerpWalkController] BeginPerpRun: playerTransform null, aborting.");
				return;
			}
			this.IsActive = true;
			this._isRunMode = true;
			this._captor = null;
			this._escortMembers.Clear();
			Action<bool> onPerpWalkStarted = PerpWalkController.OnPerpWalkStarted;
			if (onPerpWalkStarted != null)
			{
				onPerpWalkStarted(true);
			}
			if (this.debugLogs)
			{
				Debug.Log("[PerpWalkController] BeginPerpRun: censor-leak flee (no escort, no handcuffs).");
			}
			this.DisableAllNPCDetection();
			AttentionUIController.GlobalForceHideAll = true;
			if (PoliceManager.Instance != null)
			{
				PoliceManager.Instance.enabled = false;
			}
			if (this.playerInput != null)
			{
				this.playerInput.SetLockAllInput(true);
			}
			this.HideHUD(true);
			this.ActivatePerpCamera((this.perpRunCameraPresets != null) ? this.perpRunCameraPresets : this.cameraPresets, Camera.main);
			this.SwitchToFollowerLocomotion();
			this.LockDownPlayerForPerpWalk();
			this.ApplyHandcuffsAnimation();
			Vector3 vector;
			if (this.TryPickRandomPOI(out vector))
			{
				this._currentPoi = vector;
			}
			else
			{
				this._currentPoi = this.playerTransform.position + this.playerTransform.forward * this.poiMinDistance;
			}
			if (this._playerFollower != null && this._playerFollower.entityExists)
			{
				this._playerFollower.destination = this._currentPoi;
			}
			this._walkRoutine = base.StartCoroutine(this.WalkRoutine());
		}

		// Token: 0x0600699A RID: 27034 RVA: 0x002A4648 File Offset: 0x002A2848
		private void ActivatePerpCamera(DetectionCameraPresetCollectionSO presets, Camera output)
		{
			this.detectionCameraCtrl = this.ResolvePerpCameraController(output);
			if (this.detectionCameraCtrl == null || presets == null || presets.Count == 0)
			{
				Debug.LogWarning("[PerpWalkController] Cannot start the game-over camera: main-view controller or presets missing.");
				return;
			}
			if (output != null && !this._hasSavedCameraState)
			{
				this._savedCameraLocalPosition = output.transform.localPosition;
				this._savedCameraLocalRotation = output.transform.localRotation;
				this._hasSavedCameraState = true;
			}
			this.detectionCameraCtrl.enabled = true;
			this.detectionCameraCtrl.ActivateForEmotePhase(presets, this.playerTransform, null);
			this.detectionCameraCtrl.PrepareActiveCameraForReveal();
		}

		// Token: 0x0600699B RID: 27035 RVA: 0x002A46F0 File Offset: 0x002A28F0
		private DetectionCameraController ResolvePerpCameraController(Camera output)
		{
			ExhibitionEventDirector instance = ExhibitionEventDirector.Instance;
			if (instance != null && PerpWalkController.IsMainViewController(instance.DetectionCameraCtrl, output))
			{
				return instance.DetectionCameraCtrl;
			}
			if (PerpWalkController.IsMainViewController(this.detectionCameraCtrl, output))
			{
				return this.detectionCameraCtrl;
			}
			foreach (DetectionCameraController detectionCameraController in Object.FindObjectsByType<DetectionCameraController>(FindObjectsSortMode.None))
			{
				if (PerpWalkController.IsMainViewController(detectionCameraController, output))
				{
					return detectionCameraController;
				}
			}
			return null;
		}

		// Token: 0x0600699C RID: 27036 RVA: 0x002A475B File Offset: 0x002A295B
		private static bool IsMainViewController(DetectionCameraController controller, Camera output)
		{
			return controller != null && controller.gameObject.activeInHierarchy && (controller.OutputCameraOverride == null || controller.OutputCameraOverride == output);
		}

		// Token: 0x0600699D RID: 27037 RVA: 0x002A4794 File Offset: 0x002A2994
		private void SwitchToFollowerLocomotion()
		{
			if (this.playerTransform == null)
			{
				return;
			}
			if (this.playerInput != null)
			{
				this.playerInput.enabled = false;
			}
			vThirdPersonController vThirdPersonController = ((this.playerInput != null) ? this.playerInput.cc : null);
			if (vThirdPersonController != null)
			{
				vThirdPersonController.enabled = false;
			}
			this._playerRigidbody = this.playerTransform.GetComponent<Rigidbody>();
			if (this._playerRigidbody != null)
			{
				this._playerRigidbodyWasKinematic = this._playerRigidbody.isKinematic;
				if (!this._playerRigidbody.isKinematic)
				{
					this._playerRigidbody.velocity = Vector3.zero;
					this._playerRigidbody.angularVelocity = Vector3.zero;
				}
				this._playerRigidbody.isKinematic = true;
				this._playerRigidbody.useGravity = false;
			}
			this._playerCapsule = this.playerTransform.GetComponent<CapsuleCollider>();
			if (this._playerCapsule != null)
			{
				this._playerCapsule.enabled = false;
			}
			this._playerFollower = this.playerTransform.GetComponent<FollowerEntity>();
			if (this._playerFollower == null)
			{
				this._playerFollower = this.playerTransform.gameObject.AddComponent<FollowerEntity>();
			}
			this._playerFollower.enabled = true;
			this._playerFollower.maxSpeed = (this._isRunMode ? this.playerRunSpeed : this.playerWalkSpeed);
			this._playerFollower.rotationSpeed = this.playerRotationSpeed;
			this._playerFollower.isStopped = false;
			try
			{
				this._playerFollower.enableLocalAvoidance = true;
				RVOAgent rvoSettings = this._playerFollower.rvoSettings;
				rvoSettings.priority = 1f;
				this._playerFollower.rvoSettings = rvoSettings;
				if (this.debugLogs)
				{
					Debug.Log("[PerpWalkController] RVO local avoidance enabled, priority=1.0 (max).");
				}
			}
			catch (Exception ex)
			{
				if (this.debugLogs)
				{
					Debug.LogWarning("[PerpWalkController] RVO not configurable: " + ex.Message);
				}
			}
			if (this._isRunMode)
			{
				this._playerAnimator = ((this.playerInput != null && this.playerInput.cc != null) ? this.playerInput.cc.animator : null);
				if (this._playerAnimator == null && this.playerTransform != null)
				{
					this._playerAnimator = this.playerTransform.GetComponentInChildren<Animator>(true);
				}
				if (this._playerAnimator != null)
				{
					this._savedApplyRootMotion = this._playerAnimator.applyRootMotion;
					this._hasSavedApplyRootMotion = true;
					this._playerAnimator.applyRootMotion = false;
					if (this.debugLogs)
					{
						Debug.Log("[PerpWalkController] PerpRun: applyRootMotion disabled (A* drives movement).");
					}
				}
			}
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[PerpWalkController] Switched player locomotion to FollowerEntity (speed={0}).", this._isRunMode ? this.playerRunSpeed : this.playerWalkSpeed));
			}
		}

		// Token: 0x0600699E RID: 27038 RVA: 0x002A4A74 File Offset: 0x002A2C74
		private IEnumerator WalkRoutine()
		{
			Vector3 lastPos = this.playerTransform.position;
			float stuckTimer = 0f;
			while (this.IsActive)
			{
				if (FlyCamera.IsGlobalPauseActive)
				{
					yield return null;
				}
				else
				{
					bool flag = false;
					if (this._playerFollower != null && this._playerFollower.entityExists)
					{
						flag = this._playerFollower.reachedDestination;
					}
					if (!flag)
					{
						Vector3 vector = this._currentPoi - this.playerTransform.position;
						vector.y = 0f;
						flag = vector.sqrMagnitude <= this.arrivalDistance * this.arrivalDistance;
					}
					float sqrMagnitude = (this.playerTransform.position - lastPos).sqrMagnitude;
					if (!flag && sqrMagnitude < 0.0001f)
					{
						stuckTimer += Time.deltaTime;
						if (stuckTimer >= this.poiStuckTimeout)
						{
							if (this.debugLogs)
							{
								Debug.LogWarning(string.Format("[PerpWalkController] Stuck detected ({0:F1}s no movement) — repicking POI.", stuckTimer));
							}
							flag = true;
							stuckTimer = 0f;
						}
					}
					else
					{
						stuckTimer = 0f;
					}
					lastPos = this.playerTransform.position;
					if (flag)
					{
						Vector3 vector2;
						if (this.TryPickRandomPOI(out vector2))
						{
							this._currentPoi = vector2;
							if (this._playerFollower != null && this._playerFollower.entityExists)
							{
								this._playerFollower.destination = this._currentPoi;
							}
							if (this.debugLogs)
							{
								Debug.Log(string.Format("[PerpWalkController] New POI: {0} (dist {1:F1}m)", vector2, Vector3.Distance(vector2, this.playerTransform.position)));
							}
						}
						else if (this.debugLogs)
						{
							Debug.LogWarning("[PerpWalkController] No POI available.");
						}
					}
					if (Time.time >= this._nextEscortRetargetAt)
					{
						this._nextEscortRetargetAt = Time.time + this.escortRetargetInterval;
						this.UpdateEscortFormation();
					}
					yield return null;
				}
			}
			yield break;
		}

		// Token: 0x0600699F RID: 27039 RVA: 0x002A4A84 File Offset: 0x002A2C84
		private void UpdateEscortFormation()
		{
			Vector3 position = this.playerTransform.position;
			Vector3 forward = this.playerTransform.forward;
			forward.y = 0f;
			forward.Normalize();
			Vector3 right = this.playerTransform.right;
			right.y = 0f;
			right.Normalize();
			this.ApplyEscortDestination(this._captor, position - forward * this.escortLeaderFollowDistance);
			for (int i = 0; i < this._escortMembers.Count; i++)
			{
				NPCSenses npcsenses = this._escortMembers[i];
				if (!(npcsenses == null))
				{
					float num = ((i % 2 == 0) ? (-1f) : 1f);
					int num2 = i / 2 + 1;
					Vector3 vector = position - forward * (this.escortLeaderFollowDistance + (float)num2 * 0.6f) + right * (num * this.escortLateralSpacing);
					this.ApplyEscortDestination(npcsenses, vector);
				}
			}
		}

		// Token: 0x060069A0 RID: 27040 RVA: 0x002A4B80 File Offset: 0x002A2D80
		private void ApplyEscortDestination(NPCSenses npc, Vector3 dest)
		{
			if (npc == null)
			{
				return;
			}
			float num = this.playerWalkSpeed * this.escortSpeedMultiplier;
			FollowerEntity component = npc.GetComponent<FollowerEntity>();
			if (component != null)
			{
				component.isStopped = false;
				component.maxSpeed = num;
				component.destination = dest;
			}
			if (this.forceEscortWalkAnimation)
			{
				AStarNPCController component2 = npc.GetComponent<AStarNPCController>();
				if (component2 != null)
				{
					component2.SetSpeedOverride(num);
				}
			}
		}

		// Token: 0x060069A1 RID: 27041 RVA: 0x002A4BEC File Offset: 0x002A2DEC
		private static void DisableEscortAutonomy(NPCSenses npc)
		{
			if (npc == null)
			{
				return;
			}
			PoliceOfficerChaser component = npc.GetComponent<PoliceOfficerChaser>();
			if (component != null)
			{
				component.enabled = false;
			}
			SimplePOINavigator component2 = npc.GetComponent<SimplePOINavigator>();
			if (component2 != null)
			{
				component2.enabled = false;
			}
		}

		// Token: 0x060069A2 RID: 27042 RVA: 0x002A4C34 File Offset: 0x002A2E34
		private void ApplyHandcuffsAnimation()
		{
			Animator animator = ((this.playerInput != null && this.playerInput.cc != null) ? this.playerInput.cc.animator : null);
			if (animator == null)
			{
				if (this.debugLogs)
				{
					Debug.LogWarning("[PerpWalkController] No animator on player — handcuffs anim skipped.");
				}
				return;
			}
			string text = (this._isRunMode ? this.perpRunAnimatorState : this.handcuffsAnimatorState);
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			if (!this._isRunMode && this.handcuffsClip != null && animator.runtimeAnimatorController != null)
			{
				RuntimeAnimatorController runtimeAnimatorController = animator.runtimeAnimatorController;
				AnimatorOverrideController animatorOverrideController = runtimeAnimatorController as AnimatorOverrideController;
				AnimatorOverrideController animatorOverrideController2 = new AnimatorOverrideController((animatorOverrideController != null) ? animatorOverrideController.runtimeAnimatorController : runtimeAnimatorController);
				List<KeyValuePair<AnimationClip, AnimationClip>> list = new List<KeyValuePair<AnimationClip, AnimationClip>>();
				animatorOverrideController2.GetOverrides(list);
				bool flag = false;
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].Key == null || list[i].Key.name == this.handcuffsAnimatorState || list[i].Key.name.Contains("Handcuffs") || list[i].Key.name.Contains("PerpWalk"))
					{
						list[i] = new KeyValuePair<AnimationClip, AnimationClip>(list[i].Key, this.handcuffsClip);
						flag = true;
					}
				}
				if (!flag)
				{
					for (int j = 0; j < list.Count; j++)
					{
						if (list[j].Key == null)
						{
							list[j] = new KeyValuePair<AnimationClip, AnimationClip>(list[j].Key, this.handcuffsClip);
							flag = true;
							break;
						}
					}
				}
				animatorOverrideController2.ApplyOverrides(list);
				animator.runtimeAnimatorController = animatorOverrideController2;
				if (this.debugLogs)
				{
					Debug.Log(string.Format("[PerpWalkController] Handcuffs clip override applied (replaced={0}).", flag));
				}
			}
			int num = this.handcuffsAnimatorLayerIndex;
			if (!string.IsNullOrEmpty(this.handcuffsAnimatorLayerName))
			{
				int layerIndex = animator.GetLayerIndex(this.handcuffsAnimatorLayerName);
				if (layerIndex >= 0)
				{
					num = layerIndex;
				}
				else if (this.debugLogs)
				{
					Debug.LogWarning(string.Format("[PerpWalkController] Layer '{0}' not found, using index {1}.", this.handcuffsAnimatorLayerName, this.handcuffsAnimatorLayerIndex));
				}
			}
			if (this.forceLayerWeightToOne && num > 0)
			{
				animator.SetLayerWeight(num, 1f);
				if (this.debugLogs)
				{
					Debug.Log(string.Format("[PerpWalkController] Layer {0} ('{1}') weight forced to 1.", num, this.handcuffsAnimatorLayerName));
				}
			}
			if (this.handcuffsCrossfadeDuration > 0f)
			{
				animator.CrossFadeInFixedTime(text, this.handcuffsCrossfadeDuration, num);
			}
			else
			{
				animator.Play(text, num);
			}
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[PerpWalkController] CrossFade → '{0}' on layer {1} ('{2}').", text, num, this.handcuffsAnimatorLayerName));
			}
		}

		// Token: 0x060069A3 RID: 27043 RVA: 0x002A4F48 File Offset: 0x002A3148
		private void LockDownPlayerForPerpWalk()
		{
			PlayerEmotionController instance = PlayerEmotionController.Instance;
			if (instance != null)
			{
				instance.DisableEmotionLayersForAction();
				instance.DisableFacialExpressionForAction();
				instance.SetExpression(this.perpWalkFaceExpressionIndex);
				instance.enabled = false;
			}
			this.HideHUD(true);
			if (this.playerTransform != null)
			{
				PerpWalkController.DisableComponentIfPresent<PlayerActionAnimator>(this.playerTransform);
				PerpWalkController.DisableComponentIfPresent<CoverPoseController>(this.playerTransform);
				PerpWalkController.DisableComponentIfPresent<PlayerStealthBridge>(this.playerTransform);
				PerpWalkController.DisableComponentIfPresent<BreathController>(this.playerTransform);
				PerpWalkController.DisableComponentIfPresent<ClothingToggleController>(this.playerTransform);
				PerpWalkController.DisableComponentIfPresent<PlayerEyeLookAt>(this.playerTransform);
				PerpWalkController.DisableComponentIfPresent<vThirdPersonInput>(this.playerTransform);
			}
			if (this.additionalSystemsToDisable != null)
			{
				for (int i = 0; i < this.additionalSystemsToDisable.Length; i++)
				{
					MonoBehaviour monoBehaviour = this.additionalSystemsToDisable[i];
					if (monoBehaviour != null)
					{
						monoBehaviour.enabled = false;
					}
				}
			}
			if (SuspicionManager.Instance != null)
			{
				SuspicionManager.Instance.enabled = false;
			}
			if (ReporterProceduralSpawner.Instance != null)
			{
				ReporterProceduralSpawner.Instance.enabled = false;
			}
			if (ReporterTroupeManager.Instance != null)
			{
				ReporterTroupeManager.Instance.enabled = false;
			}
			if (NPCZoneManager.Instance != null)
			{
				NPCZoneManager.Instance.enabled = false;
			}
			NPCRuntimeSpawner npcruntimeSpawner = Object.FindFirstObjectByType<NPCRuntimeSpawner>();
			if (npcruntimeSpawner != null)
			{
				npcruntimeSpawner.enabled = false;
			}
			if (this.debugLogs)
			{
				Debug.Log("[PerpWalkController] Lockdown applied: emotion off, HUD hidden, player systems disabled, world managers frozen.");
			}
		}

		// Token: 0x060069A4 RID: 27044 RVA: 0x002A50A4 File Offset: 0x002A32A4
		private static void DisableComponentIfPresent<T>(Transform root) where T : MonoBehaviour
		{
			if (root == null)
			{
				return;
			}
			T componentInChildren = root.GetComponentInChildren<T>(true);
			if (componentInChildren != null)
			{
				componentInChildren.enabled = false;
			}
		}

		// Token: 0x060069A5 RID: 27045 RVA: 0x002A50E0 File Offset: 0x002A32E0
		private static void EnableComponentIfPresent<T>(Transform root) where T : MonoBehaviour
		{
			if (root == null)
			{
				return;
			}
			T componentInChildren = root.GetComponentInChildren<T>(true);
			if (componentInChildren != null)
			{
				componentInChildren.enabled = true;
			}
		}

		// Token: 0x060069A6 RID: 27046 RVA: 0x002A511C File Offset: 0x002A331C
		public void StopAndRestore()
		{
			if (this.IsActive)
			{
				FlyCamera.StopGlobalPause();
			}
			if (this._walkRoutine != null)
			{
				base.StopCoroutine(this._walkRoutine);
				this._walkRoutine = null;
			}
			if (this._playerFollower != null)
			{
				this._playerFollower.isStopped = true;
				this._playerFollower.enabled = false;
			}
			if (this._playerCapsule != null)
			{
				this._playerCapsule.enabled = true;
			}
			if (this._playerRigidbody != null)
			{
				this._playerRigidbody.isKinematic = this._playerRigidbodyWasKinematic;
				this._playerRigidbody.useGravity = true;
			}
			if (this.playerInput != null)
			{
				if (this.playerInput.cc != null)
				{
					this.playerInput.cc.enabled = true;
				}
				this.playerInput.enabled = true;
				this.playerInput.SetLockAllInput(false);
				this.playerInput.SetLockCameraInput(false);
			}
			if (this.playerTransform != null)
			{
				PerpWalkController.EnableComponentIfPresent<PlayerActionAnimator>(this.playerTransform);
				PerpWalkController.EnableComponentIfPresent<CoverPoseController>(this.playerTransform);
				PerpWalkController.EnableComponentIfPresent<PlayerStealthBridge>(this.playerTransform);
				PerpWalkController.EnableComponentIfPresent<BreathController>(this.playerTransform);
				PerpWalkController.EnableComponentIfPresent<ClothingToggleController>(this.playerTransform);
				PerpWalkController.EnableComponentIfPresent<PlayerEyeLookAt>(this.playerTransform);
			}
			if (this.additionalSystemsToDisable != null)
			{
				for (int i = 0; i < this.additionalSystemsToDisable.Length; i++)
				{
					if (this.additionalSystemsToDisable[i] != null)
					{
						this.additionalSystemsToDisable[i].enabled = true;
					}
				}
			}
			PlayerEmotionController instance = PlayerEmotionController.Instance;
			if (instance != null)
			{
				instance.enabled = true;
				instance.RestoreEmotionLayers();
				instance.RestoreFacialExpression();
				instance.ResetToNeutral();
			}
			if (this.handcuffsProp != null)
			{
				this.handcuffsProp.gameObject.SetActive(false);
			}
			Animator animator = ((this.playerInput != null && this.playerInput.cc != null) ? this.playerInput.cc.animator : null);
			if (animator != null)
			{
				AnimatorOverrideController animatorOverrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
				if (animatorOverrideController != null && animatorOverrideController.runtimeAnimatorController != null)
				{
					animator.runtimeAnimatorController = animatorOverrideController.runtimeAnimatorController;
				}
				if (this._hasSavedApplyRootMotion && this._playerAnimator != null)
				{
					this._playerAnimator.applyRootMotion = this._savedApplyRootMotion;
					this._hasSavedApplyRootMotion = false;
					this._playerAnimator = null;
				}
				int num = this.handcuffsAnimatorLayerIndex;
				if (!string.IsNullOrEmpty(this.handcuffsAnimatorLayerName))
				{
					int layerIndex = animator.GetLayerIndex(this.handcuffsAnimatorLayerName);
					if (layerIndex >= 0)
					{
						num = layerIndex;
					}
				}
				if (num > 0)
				{
					animator.SetLayerWeight(num, 0f);
				}
			}
			this.HideHUD(false);
			if (this.detectionCameraCtrl != null)
			{
				this.detectionCameraCtrl.Deactivate();
			}
			CinemachineBrain cinemachineBrain = ((Camera.main != null) ? Camera.main.GetComponent<CinemachineBrain>() : null);
			if (cinemachineBrain != null)
			{
				cinemachineBrain.enabled = false;
			}
			if (this._hasSavedCameraState && Camera.main != null)
			{
				Camera.main.transform.localPosition = this._savedCameraLocalPosition;
				Camera.main.transform.localRotation = this._savedCameraLocalRotation;
				this._hasSavedCameraState = false;
			}
			AttentionUIController.GlobalForceHideAll = false;
			this._playerFollower = null;
			this._playerCapsule = null;
			this._playerRigidbody = null;
			this._captor = null;
			this._escortMembers.Clear();
			this.IsActive = false;
			this._isRunMode = false;
			Action onPerpWalkEnded = PerpWalkController.OnPerpWalkEnded;
			if (onPerpWalkEnded != null)
			{
				onPerpWalkEnded();
			}
			if (this.debugLogs)
			{
				Debug.Log("[PerpWalkController] StopAndRestore: player ripristinato a stato gameplay pulito.");
			}
		}

		// Token: 0x060069A7 RID: 27047 RVA: 0x002A54B4 File Offset: 0x002A36B4
		private void DisableAllNPCDetection()
		{
			int num = 0;
			if (SensoryManager.Instance != null)
			{
				IReadOnlyList<NPCSenses> registeredNPCs = SensoryManager.Instance.GetRegisteredNPCs();
				NPCSenses[] array = new NPCSenses[registeredNPCs.Count];
				for (int i = 0; i < registeredNPCs.Count; i++)
				{
					array[i] = registeredNPCs[i];
				}
				foreach (NPCSenses npcsenses in array)
				{
					if (!(npcsenses == null))
					{
						npcsenses.enabled = false;
						num++;
					}
				}
			}
			int num2 = 0;
			foreach (PolicePatrol policePatrol in Object.FindObjectsByType<PolicePatrol>(FindObjectsSortMode.None))
			{
				if (policePatrol != null)
				{
					policePatrol.enabled = false;
					num2++;
				}
			}
			foreach (PoliceOfficerChaser policeOfficerChaser in Object.FindObjectsByType<PoliceOfficerChaser>(FindObjectsSortMode.None))
			{
				if (policeOfficerChaser != null)
				{
					policeOfficerChaser.enabled = false;
					num2++;
				}
			}
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[PerpWalkController] Detection shutdown: {0} NPCSenses + {1} FSM disabled.", num, num2));
			}
		}

		// Token: 0x060069A8 RID: 27048 RVA: 0x002A55D0 File Offset: 0x002A37D0
		private bool TryPickRandomPOI(out Vector3 result)
		{
			result = Vector3.zero;
			if (NPCZoneManager.Instance == null)
			{
				return this.TryPickRandomFallback(out result);
			}
			NPCInterestPoint npcinterestPoint = null;
			NPCInterestPoint npcinterestPoint2 = null;
			float num = -1f;
			Vector3 position = this.playerTransform.position;
			float num2 = this.poiMinDistance * this.poiMinDistance;
			for (int i = 0; i < this.poiSearchAttempts; i++)
			{
				NPCInterestPoint randomInterestPoint = NPCZoneManager.Instance.GetRandomInterestPoint(position, this.poiSearchRadius);
				if (!(randomInterestPoint == null) && !(randomInterestPoint == this._lastPoi))
				{
					float sqrMagnitude = (randomInterestPoint.NavigationPosition - position).sqrMagnitude;
					if (sqrMagnitude >= num2)
					{
						npcinterestPoint = randomInterestPoint;
						break;
					}
					if (sqrMagnitude > num)
					{
						num = sqrMagnitude;
						npcinterestPoint2 = randomInterestPoint;
					}
				}
			}
			NPCInterestPoint npcinterestPoint3 = ((npcinterestPoint != null) ? npcinterestPoint : npcinterestPoint2);
			if (npcinterestPoint3 != null)
			{
				this._lastPoi = npcinterestPoint3;
				result = npcinterestPoint3.NavigationPosition;
				return true;
			}
			return this.TryPickRandomFallback(out result);
		}

		// Token: 0x060069A9 RID: 27049 RVA: 0x002A56C8 File Offset: 0x002A38C8
		private bool TryPickRandomFallback(out Vector3 result)
		{
			result = Vector3.zero;
			if (AstarPath.active == null)
			{
				return false;
			}
			for (int i = 0; i < this.poiSearchAttempts; i++)
			{
				float num = Random.Range(0f, 6.2831855f);
				float num2 = Random.Range(this.poiMinDistance, this.poiSearchRadius);
				Vector3 vector = this.playerTransform.position + new Vector3(Mathf.Cos(num) * num2, 0f, Mathf.Sin(num) * num2);
				NNInfo nearest = AstarPath.active.GetNearest(vector);
				if (nearest.node != null && nearest.node.Walkable)
				{
					result = nearest.position;
					return true;
				}
			}
			return false;
		}

		// Token: 0x060069AA RID: 27050 RVA: 0x002A5788 File Offset: 0x002A3988
		private void HideHUD(bool hide)
		{
			if (HUDManager.Instance != null)
			{
				if (hide)
				{
					HUDManager.Instance.HideHUD();
				}
				else
				{
					HUDManager.Instance.ShowHUD();
				}
			}
			foreach (GameObject gameObject in this.additionalHUDToHide)
			{
				if (gameObject != null)
				{
					gameObject.SetActive(!hide);
				}
			}
		}

		// Token: 0x04006C62 RID: 27746
		[Header("Refs (assegna in Inspector)")]
		[Tooltip("vThirdPersonInput sul player (per mantenere input loccato e simulare cc.input).")]
		[SerializeField]
		private vThirdPersonInput playerInput;

		// Token: 0x04006C63 RID: 27747
		[Tooltip("Transform del player (auto-fetched da playerInput se null).")]
		[SerializeField]
		private Transform playerTransform;

		// Token: 0x04006C64 RID: 27748
		[Tooltip("Camera controller per i preset (lo stesso usato dal Director). Quando attivo, il tasto C cicla i preset automaticamente.")]
		[SerializeField]
		private DetectionCameraController detectionCameraCtrl;

		// Token: 0x04006C65 RID: 27749
		[Tooltip("Collection di preset camera usati durante il PerpWalk (cattura polizia, manette).")]
		[SerializeField]
		private DetectionCameraPresetCollectionSO cameraPresets;

		// Token: 0x04006C66 RID: 27750
		[Tooltip("Collection di preset camera usati durante il PerpRun (game-over censura, fuga). Se null, ripiega su 'cameraPresets' (i preset del PerpWalk).")]
		[SerializeField]
		private DetectionCameraPresetCollectionSO perpRunCameraPresets;

		// Token: 0x04006C67 RID: 27751
		[Header("HUD")]
		[Tooltip("HUD root da nascondere durante il perp walk (es. minimap, suspicion meter). Lasciare vuoto se HUDManager.Instance.HideHUD() basta.")]
		[SerializeField]
		private List<GameObject> additionalHUDToHide = new List<GameObject>();

		// Token: 0x04006C68 RID: 27752
		[Header("Player Auto-Walk")]
		[Tooltip("Velocita' max del player durante PerpWalk (m/s). Passata a FollowerEntity.maxSpeed.")]
		[SerializeField]
		private float playerWalkSpeed = 1.4f;

		// Token: 0x04006C69 RID: 27753
		[Tooltip("Velocita' max del player durante il PerpRun (m/s, game-over censura). Piu' alta del walk: Ciel scappa di corsa coprendosi il viso.")]
		[SerializeField]
		private float playerRunSpeed = 4f;

		// Token: 0x04006C6A RID: 27754
		[Tooltip("Rotazione massima del player verso la direzione di movimento (deg/s). FollowerEntity gestisce automaticamente la rotazione, questo e' il cap.")]
		[SerializeField]
		private float playerRotationSpeed = 360f;

		// Token: 0x04006C6B RID: 27755
		[Header("Handcuffs Animation")]
		[Tooltip("Path completo dello state Animator (nel formato 'SubStateMachine.StateName' o solo 'StateName' se nel root). Lo state PerpWalk di Ciel vive nella sub-state machine 'GameOver' del Base Layer, quindi il path corretto e' 'GameOver.PerpWalk'. CrossFade e Play accettano questo formato.")]
		[SerializeField]
		private string handcuffsAnimatorState = "GameOver.PerpWalk";

		// Token: 0x04006C6C RID: 27756
		[Tooltip("Nome del layer Animator su cui sta lo state PerpWalk. Lo state PerpWalk vive in 'GameOver' sub-state machine del 'Base Layer' di Ciel, quindi il layer e' 0. Lasciare vuoto per usare handcuffsAnimatorLayerIndex.")]
		[SerializeField]
		private string handcuffsAnimatorLayerName = "";

		// Token: 0x04006C6D RID: 27757
		[Tooltip("Indice del layer Animator. Default 0 = Base Layer (dove sta la sub-state machine GameOver).")]
		[SerializeField]
		private int handcuffsAnimatorLayerIndex;

		// Token: 0x04006C6E RID: 27758
		[Tooltip("Se true, forza il weight del layer handcuffs a 1.0 prima del CrossFade. Inutile sul Base Layer (sempre weight=1) ma utile se metti lo state in un layer override.")]
		[SerializeField]
		private bool forceLayerWeightToOne;

		// Token: 0x04006C6F RID: 27759
		[Tooltip("Durata del crossfade verso lo state handcuffs (s). 0 = cut istantaneo.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float handcuffsCrossfadeDuration = 0.2f;

		// Token: 0x04006C70 RID: 27760
		[Tooltip("Clip handcuffs walk (es. SCR_Walk_With_Handcuffs_Loop_IP). Se assegnata, viene applicata runtime via AnimatorOverrideController sostituendo la motion dello state 'handcuffsAnimatorState'. Comodo se la motion nello state e' broken/missing (es. GUID orfano). Lascia null se la clip e' gia' assegnata correttamente nello state via inspector.")]
		[SerializeField]
		private AnimationClip handcuffsClip;

		// Token: 0x04006C71 RID: 27761
		[Tooltip("Transform del prop manette gia' presente sul rig del player. Lo attiviamo (SetActive(true)) all'inizio del PerpWalk. Se null, niente visual manette ma l'animazione gira lo stesso.")]
		[SerializeField]
		private Transform handcuffsProp;

		// Token: 0x04006C72 RID: 27762
		[Header("PerpRun Animation (censor leak game-over)")]
		[Tooltip("State Animator usato dal PerpRun (formato 'SubStateMachine.StateName'). Gemello di 'GameOver.PerpWalk' ma con la clip di corsa Run EMB 01 (anim nativa con le braccia che coprono il viso) assegnata staticamente. La copertura viso e' nella clip, quindi la CoverPose viene disabilitata come nel PerpWalk.")]
		[SerializeField]
		private string perpRunAnimatorState = "GameOver.PerpRun";

		// Token: 0x04006C73 RID: 27763
		[Header("Player Lockdown (terminal state)")]
		[Tooltip("Indice della face expression da settare durante il PerpWalk. 4 = EmbSupreme (default — espressione di umiliazione massima per la cattura).")]
		[SerializeField]
		private int perpWalkFaceExpressionIndex = 4;

		// Token: 0x04006C74 RID: 27764
		[Tooltip("MonoBehaviour aggiuntivi sul player (o ovunque) da disabilitare durante il PerpWalk. Wira qui qualsiasi sistema custom che durante lo state terminale non deve girare (es. nuovi sistemi inventory, interaction triggers, post-process autonomi). I sistemi noti (PlayerEmotionController, PlayerActionAnimator, CoverPoseController, BreathController, PlayerStealthBridge, ClothingToggleController, vThirdPersonInput) sono gia' disabilitati automaticamente — non e' necessario re-elencarli qui.")]
		[SerializeField]
		private MonoBehaviour[] additionalSystemsToDisable;

		// Token: 0x04006C75 RID: 27765
		[Tooltip("Raggio di ricerca POI tramite NPCZoneManager. Distanze maggiori = camminata piu' lunga tra una destinazione e l'altra (camminata fluida, no fermate ravvicinate). NPCZoneManager usa hardcap interno 1.5x come fallback se non trova POI nel raggio.")]
		[SerializeField]
		private float poiSearchRadius = 35f;

		// Token: 0x04006C76 RID: 27766
		[Tooltip("Distanza minima desiderata di un POI rispetto al player. POI piu' vicini di questo valore vengono scartati per evitare camminate troppo brevi. Se nessun POI rispetta la soglia, ripiega sul POI piu' vicino al player accettando la distanza ridotta.")]
		[SerializeField]
		private float poiMinDistance = 12f;

		// Token: 0x04006C77 RID: 27767
		[Tooltip("Distanza (m) sotto la quale il player 'arriva' al POI e ne sceglie uno nuovo.")]
		[SerializeField]
		private float arrivalDistance = 1.5f;

		// Token: 0x04006C78 RID: 27768
		[Tooltip("Tentativi max di pesca POI (per evitare di ripescare l'ultimo o uno troppo vicino).")]
		[SerializeField]
		private int poiSearchAttempts = 8;

		// Token: 0x04006C79 RID: 27769
		[Tooltip("Secondi senza movimento dopo i quali consideriamo il player 'stuck' (es. POI irragiungibile, muro nel path) e ripeschiamo un nuovo POI. 0 disabilita lo stuck-detect.")]
		[SerializeField]
		private float poiStuckTimeout = 2.5f;

		// Token: 0x04006C7A RID: 27770
		private const float poiStuckMoveThresholdSq = 0.0001f;

		// Token: 0x04006C7B RID: 27771
		[Header("Escort (officer/patrol)")]
		[Tooltip("Distanza dietro al player dove l'arrestatore (initiator) si posiziona.")]
		[SerializeField]
		private float escortLeaderFollowDistance = 1.2f;

		// Token: 0x04006C7C RID: 27772
		[Tooltip("Spacing laterale per i membri patrol aggiuntivi (in formazione a V dietro al leader).")]
		[SerializeField]
		private float escortLateralSpacing = 0.9f;

		// Token: 0x04006C7D RID: 27773
		[Tooltip("Quanto spesso ri-impostare la destinazione degli escort (s). Tradeoff CPU/precisione.")]
		[SerializeField]
		private float escortRetargetInterval = 0.4f;

		// Token: 0x04006C7E RID: 27774
		[Tooltip("Multiplier sulla player walk speed applicato agli escort. 1.0 = stessa velocita' del player (escort restano staccati a distanza fissa, NESSUN flickering del blend tree NPC). Valori >1 fanno attaccare gli escort piu' velocemente ma causano il blending Idle↔Walk↔Run scattoso quando devono rallentare per non superare il player.")]
		[Range(0.8f, 1.5f)]
		[SerializeField]
		private float escortSpeedMultiplier = 1f;

		// Token: 0x04006C7F RID: 27775
		[Tooltip("Se true, forza il blend tree degli NPC escort a 'Walk' fisso via AStarNPCController.SetSpeedOverride. Evita il flickering Idle/Walk/Run quando la velocita' effettiva oscilla per il path-following (es. quando rallentano in curva).")]
		[SerializeField]
		private bool forceEscortWalkAnimation = true;

		// Token: 0x04006C80 RID: 27776
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04006C84 RID: 27780
		private bool _isRunMode;

		// Token: 0x04006C85 RID: 27781
		private NPCSenses _captor;

		// Token: 0x04006C86 RID: 27782
		private List<NPCSenses> _escortMembers = new List<NPCSenses>();

		// Token: 0x04006C87 RID: 27783
		private Vector3 _currentPoi;

		// Token: 0x04006C88 RID: 27784
		private NPCInterestPoint _lastPoi;

		// Token: 0x04006C89 RID: 27785
		private float _nextEscortRetargetAt;

		// Token: 0x04006C8A RID: 27786
		private Coroutine _walkRoutine;

		// Token: 0x04006C8B RID: 27787
		private FollowerEntity _playerFollower;

		// Token: 0x04006C8C RID: 27788
		private Rigidbody _playerRigidbody;

		// Token: 0x04006C8D RID: 27789
		private CapsuleCollider _playerCapsule;

		// Token: 0x04006C8E RID: 27790
		private bool _playerRigidbodyWasKinematic;

		// Token: 0x04006C8F RID: 27791
		private Animator _playerAnimator;

		// Token: 0x04006C90 RID: 27792
		private bool _savedApplyRootMotion;

		// Token: 0x04006C91 RID: 27793
		private bool _hasSavedApplyRootMotion;

		// Token: 0x04006C92 RID: 27794
		private Vector3 _savedCameraLocalPosition;

		// Token: 0x04006C93 RID: 27795
		private Quaternion _savedCameraLocalRotation;

		// Token: 0x04006C94 RID: 27796
		private bool _hasSavedCameraState;
	}
}
