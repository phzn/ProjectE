using System;
using System.Collections.Generic;
using Features.Detection;
using Features.NPC.StateMachine;
using Features.RenderingPrecision;
using Pathfinding;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Features.NPC
{
	// Token: 0x02000EDE RID: 3806
	[DefaultExecutionOrder(450)]
	[DisallowMultipleComponent]
	public sealed class NPCRagdollRecovery : MonoBehaviour
	{
		// Token: 0x1700129A RID: 4762
		// (get) Token: 0x060071E5 RID: 29157 RVA: 0x002E4CF8 File Offset: 0x002E2EF8
		// (set) Token: 0x060071E6 RID: 29158 RVA: 0x002E4D00 File Offset: 0x002E2F00
		public bool IsReady { get; private set; }

		// Token: 0x1700129B RID: 4763
		// (get) Token: 0x060071E7 RID: 29159 RVA: 0x002E4D09 File Offset: 0x002E2F09
		public bool IsRecovering
		{
			get
			{
				return this.phase > NPCRagdollRecovery.RecoveryPhase.None;
			}
		}

		// Token: 0x1700129C RID: 4764
		// (get) Token: 0x060071E8 RID: 29160 RVA: 0x002E4D14 File Offset: 0x002E2F14
		public bool IsPhysicallyRagdolled
		{
			get
			{
				return this.phase == NPCRagdollRecovery.RecoveryPhase.Ragdolled;
			}
		}

		// Token: 0x1700129D RID: 4765
		// (get) Token: 0x060071E9 RID: 29161 RVA: 0x002E4D1F File Offset: 0x002E2F1F
		public Vector3 PhysicalPosition
		{
			get
			{
				if (!(this.hips != null))
				{
					return base.transform.position;
				}
				return this.hips.position;
			}
		}

		// Token: 0x1700129E RID: 4766
		// (get) Token: 0x060071EA RID: 29162 RVA: 0x002E4D46 File Offset: 0x002E2F46
		public bool IsPlayingGetUp
		{
			get
			{
				return this.phase == NPCRagdollRecovery.RecoveryPhase.BlendingToGetUp || this.phase == NPCRagdollRecovery.RecoveryPhase.GettingUp;
			}
		}

		// Token: 0x1700129F RID: 4767
		// (get) Token: 0x060071EB RID: 29163 RVA: 0x002E4D5C File Offset: 0x002E2F5C
		public float CurrentGetUpNormalizedTime
		{
			get
			{
				return this.currentGetUpNormalizedTime;
			}
		}

		// Token: 0x060071EC RID: 29164 RVA: 0x002E4D64 File Offset: 0x002E2F64
		private void Awake()
		{
			this.ReinitializeAfterRigChange();
		}

		// Token: 0x060071ED RID: 29165 RVA: 0x002E4D6D File Offset: 0x002E2F6D
		private void OnEnable()
		{
			WorldOrigin.Shifted += this.OnWorldOriginShifted;
			NPCSimulationPause.PauseChanged += this.HandleSimulationPauseChanged;
			NPCSimulationPause.RegisterParticipant(this);
			this.HandleSimulationPauseChanged(NPCSimulationPause.IsPaused);
		}

		// Token: 0x060071EE RID: 29166 RVA: 0x002E4DA4 File Offset: 0x002E2FA4
		private void Update()
		{
			if (NPCSimulationPause.IsPaused)
			{
				this.BeginSimulationPauseIfNeeded();
				return;
			}
			this.ResumeSimulationIfNeeded();
			this.ApplyGetUpPlayableSimulationScale();
			if (this.phase != NPCRagdollRecovery.RecoveryPhase.None)
			{
				this.phaseStartedTime += NPCSimulationPause.FrameTimeLoss;
			}
			if (NPCSimulationPause.SimulationScale <= 0.0001f)
			{
				return;
			}
			if (this.phase == NPCRagdollRecovery.RecoveryPhase.Ragdolled)
			{
				this.UpdatePhysicalRecovery();
				return;
			}
			if (this.phase == NPCRagdollRecovery.RecoveryPhase.BlendingToGetUp)
			{
				this.UpdatePoseBlend();
				return;
			}
			if (this.phase == NPCRagdollRecovery.RecoveryPhase.GettingUp)
			{
				this.UpdateGetUp();
			}
		}

		// Token: 0x060071EF RID: 29167 RVA: 0x002E4E24 File Offset: 0x002E3024
		private void LateUpdate()
		{
			if (NPCSimulationPause.IsPaused || NPCSimulationPause.SimulationScale <= 0.0001f)
			{
				return;
			}
			if (this.phase != NPCRagdollRecovery.RecoveryPhase.BlendingToGetUp)
			{
				return;
			}
			float num = Mathf.Max(0.0001f, this.poseBlendDuration);
			float num2 = 1f - Mathf.Clamp01((Time.time - this.phaseStartedTime) / num);
			for (int i = 0; i < this.bodyParts.Count; i++)
			{
				NPCRagdollRecovery.BodyPart bodyPart = this.bodyParts[i];
				if (!(bodyPart.Transform == null))
				{
					bodyPart.Transform.rotation = Quaternion.Slerp(bodyPart.Transform.rotation, bodyPart.StoredRotation, num2);
					if (bodyPart.Transform == this.hips)
					{
						bodyPart.Transform.position = Vector3.Lerp(bodyPart.Transform.position, bodyPart.StoredPosition, num2);
					}
				}
			}
		}

		// Token: 0x060071F0 RID: 29168 RVA: 0x002E4F08 File Offset: 0x002E3108
		public bool ReinitializeAfterRigChange()
		{
			if (this.IsRecovering)
			{
				return this.IsReady;
			}
			this.animator = base.GetComponent<Animator>();
			this.rootCollider = base.GetComponent<CapsuleCollider>();
			this.follower = base.GetComponent<FollowerEntity>();
			this.npcController = base.GetComponent<AStarNPCController>();
			this.wanderController = base.GetComponent<NPCWanderController>();
			this.stateMachine = base.GetComponent<NPCStateMachine>();
			this.stumbleHandler = base.GetComponent<NPCStumbleHandler>();
			this.lookAt = base.GetComponent<NPCSimpleLookAt>();
			if (this.animationProfile == null)
			{
				this.animationProfile = Resources.Load<NPCRagdollAnimationProfile>("NPCRagdollAnimationProfile");
			}
			NPCRagdollRecovery.HumanoidBones humanoidBones = null;
			this.IsReady = this.animator != null && this.animator.isHuman && this.TryGetRequiredBones(out humanoidBones);
			if (!this.IsReady)
			{
				this.physicalRigReady = false;
				return false;
			}
			this.hips = humanoidBones.Hips;
			this.chest = humanoidBones.Chest;
			this.head = humanoidBones.Head;
			this.leftFoot = humanoidBones.LeftFoot;
			this.rightFoot = humanoidBones.RightFoot;
			this.hipsLocalCharacterForward = this.hips.InverseTransformDirection(base.transform.forward);
			this.physicalRigReady = false;
			this.bodyParts.Clear();
			this.ownColliders.Clear();
			this.hipsBody = null;
			this.chestBody = null;
			return true;
		}

		// Token: 0x060071F1 RID: 29169 RVA: 0x002E5068 File Offset: 0x002E3268
		public bool ActivateRagdoll(Vector3 launchVelocity, Vector3 impactPoint, Vector3 angularVelocity)
		{
			if (this.IsRecovering || NPCPoolRecovery.IsActorRecovering(this))
			{
				return false;
			}
			if (!this.IsReady && !this.ReinitializeAfterRigChange())
			{
				Debug.LogWarning("[NPCRagdollRecovery] '" + base.name + "' has no valid humanoid rig.", this);
				return false;
			}
			if (this.animationProfile == null || !this.animationProfile.IsValid)
			{
				Debug.LogError("[NPCRagdollRecovery] Missing Resources/NPCRagdollAnimationProfile.", this);
				return false;
			}
			if (!this.EnsurePhysicalRig())
			{
				Debug.LogError("[NPCRagdollRecovery] Could not build the physical rig for '" + base.name + "'.", this);
				return false;
			}
			NPCAmbientSuspension component = base.GetComponent<NPCAmbientSuspension>();
			if (component != null)
			{
				component.SetSuspended(false);
			}
			NPCSimulationPause.PrepareParticipantForRagdoll(this);
			this.SnapshotAndSuspendNpcSystems();
			this.HoldAnimationCulling();
			this.animator.enabled = false;
			this.SetPhysicalState(true);
			for (int i = 0; i < this.bodyParts.Count; i++)
			{
				Rigidbody rigidbody = this.bodyParts[i].Rigidbody;
				if (!(rigidbody == null))
				{
					rigidbody.velocity = launchVelocity;
					rigidbody.angularVelocity = angularVelocity;
					rigidbody.WakeUp();
				}
			}
			if (this.chestBody != null)
			{
				Vector3 vector = launchVelocity * 0.16f;
				this.chestBody.AddForceAtPosition(vector, impactPoint, ForceMode.VelocityChange);
			}
			this.phase = NPCRagdollRecovery.RecoveryPhase.Ragdolled;
			this.phaseStartedTime = Time.time;
			this.stableTime = 0f;
			this.currentGetUpNormalizedTime = 0f;
			if (this.simulationPauseActive)
			{
				this.simulationPauseStartedAt = this.phaseStartedTime;
			}
			if (this.logRecovery)
			{
				Debug.Log("[NPCRagdollRecovery] Ragdoll started on '" + base.name + "'.", this);
			}
			return true;
		}

		// Token: 0x060071F2 RID: 29170 RVA: 0x002E5208 File Offset: 0x002E3408
		private void UpdatePhysicalRecovery()
		{
			float num = Time.time - this.phaseStartedTime;
			float recoveryBodySpeed = this.GetRecoveryBodySpeed();
			if (num >= this.minimumRecoveryDelay && recoveryBodySpeed <= this.stableVelocity)
			{
				this.stableTime += NPCSimulationPause.SimulationDeltaTime;
			}
			else
			{
				this.stableTime = 0f;
			}
			if (this.stableTime >= this.stableDuration || num >= this.forcedRecoveryDelay)
			{
				this.BeginGetUp();
			}
		}

		// Token: 0x060071F3 RID: 29171 RVA: 0x002E5278 File Offset: 0x002E3478
		public void CompleteForWaterRecovery(Vector3 rootPosition)
		{
			if (!this.IsPhysicallyRagdolled)
			{
				return;
			}
			this.DestroyGetUpGraph();
			this.SetPhysicalState(false);
			base.transform.position = rootPosition;
			if (this.animator != null)
			{
				this.animator.enabled = true;
				this.animator.Rebind();
				this.animator.Update(0f);
			}
			if (this.npcController != null)
			{
				this.npcController.SetPosture(this.npcController.CurrentPosture);
				this.npcController.SetAction(this.npcController.CurrentAction);
			}
			this.RestoreNpcSystems(false);
			this.RestoreAnimationCulling();
			this.phase = NPCRagdollRecovery.RecoveryPhase.None;
			this.stableTime = 0f;
			NPCSimulationPause.RestoreParticipantAfterRagdoll(this);
		}

		// Token: 0x060071F4 RID: 29172 RVA: 0x002E533B File Offset: 0x002E353B
		private void HandleSimulationPauseChanged(bool paused)
		{
			if (paused)
			{
				this.BeginSimulationPauseIfNeeded();
				return;
			}
			this.ResumeSimulationIfNeeded();
		}

		// Token: 0x060071F5 RID: 29173 RVA: 0x002E5350 File Offset: 0x002E3550
		private void BeginSimulationPauseIfNeeded()
		{
			if (this.simulationPauseActive)
			{
				return;
			}
			this.simulationPauseActive = true;
			this.simulationPauseStartedAt = Time.time;
			if (this.getUpPlayable.IsValid<AnimationClipPlayable>())
			{
				this.getUpPlayableSpeedBeforeSimulationPause = this.getUpPlayable.GetSpeed<AnimationClipPlayable>();
				this.getUpPlayable.SetSpeed(0.0);
				this.getUpPlayablePausedBySimulation = true;
			}
		}

		// Token: 0x060071F6 RID: 29174 RVA: 0x002E53B4 File Offset: 0x002E35B4
		private void ResumeSimulationIfNeeded()
		{
			if (!this.simulationPauseActive)
			{
				return;
			}
			if (this.phase != NPCRagdollRecovery.RecoveryPhase.None)
			{
				this.phaseStartedTime += Mathf.Max(0f, Time.time - this.simulationPauseStartedAt);
			}
			if (this.getUpPlayablePausedBySimulation && this.getUpPlayable.IsValid<AnimationClipPlayable>())
			{
				double num = ((this.phase == NPCRagdollRecovery.RecoveryPhase.GettingUp) ? ((double)NPCSimulationPause.SimulationScale) : this.getUpPlayableSpeedBeforeSimulationPause);
				this.getUpPlayable.SetSpeed(num);
			}
			this.getUpPlayablePausedBySimulation = false;
			this.simulationPauseActive = false;
		}

		// Token: 0x060071F7 RID: 29175 RVA: 0x002E543C File Offset: 0x002E363C
		private void BeginGetUp()
		{
			this.CorrectPhysicalPenetration();
			this.StorePhysicalPose();
			this.getUpFromBack = this.hips.TransformDirection(this.hipsLocalCharacterForward).y > 0f;
			this.DestroyGetUpGraph();
			this.activeGetUpClip = (this.getUpFromBack ? this.animationProfile.standUpFromBack : this.animationProfile.standUpFromBelly);
			this.SetPhysicalState(false);
			this.animator.enabled = true;
			this.getUpPlayable = AnimationPlayableUtilities.PlayClip(this.animator, this.activeGetUpClip, out this.getUpGraph);
			this.getUpPlayable.SetTime(0.0);
			this.getUpPlayable.SetSpeed(0.0);
			this.getUpGraph.Evaluate(0f);
			this.AlignAnimatedRootToPhysicalPose();
			this.getUpGraph.Evaluate(0f);
			this.AlignAnimatedHipsToStoredPosition();
			this.phase = NPCRagdollRecovery.RecoveryPhase.BlendingToGetUp;
			this.phaseStartedTime = Time.time;
			if (this.logRecovery)
			{
				string text = (this.getUpFromBack ? "back" : "belly");
				Debug.Log(string.Concat(new string[] { "[NPCRagdollRecovery] Starting ", text, " get-up on '", base.name, "'." }), this);
			}
		}

		// Token: 0x060071F8 RID: 29176 RVA: 0x002E5590 File Offset: 0x002E3790
		private void UpdatePoseBlend()
		{
			if (Time.time - this.phaseStartedTime < this.poseBlendDuration)
			{
				return;
			}
			this.getUpPlayable.SetTime(0.0);
			this.getUpPlayable.SetSpeed((double)NPCSimulationPause.SimulationScale);
			this.getUpGraph.Play();
			this.phase = NPCRagdollRecovery.RecoveryPhase.GettingUp;
			this.phaseStartedTime = Time.time;
		}

		// Token: 0x060071F9 RID: 29177 RVA: 0x002E55F4 File Offset: 0x002E37F4
		private void UpdateGetUp()
		{
			if (this.activeGetUpClip == null || !this.getUpGraph.IsValid())
			{
				this.FinishRecovery();
				return;
			}
			this.currentGetUpNormalizedTime = ((this.activeGetUpClip.length > 0f) ? ((float)(this.getUpPlayable.GetTime<AnimationClipPlayable>() / (double)this.activeGetUpClip.length)) : 1f);
			if (this.currentGetUpNormalizedTime >= this.getUpCompletion || Time.time - this.phaseStartedTime >= this.maximumGetUpDuration)
			{
				this.FinishRecovery();
			}
		}

		// Token: 0x060071FA RID: 29178 RVA: 0x002E5684 File Offset: 0x002E3884
		private void FinishRecovery()
		{
			this.DestroyGetUpGraph();
			this.SnapRootToGround();
			if (this.animator != null)
			{
				this.animator.Rebind();
				this.animator.Update(0f);
			}
			NPCSenses component = base.GetComponent<NPCSenses>();
			if (component != null)
			{
				component.SetAttentionState(Mathf.Max(0.5f, component.CurrentAttention));
			}
			this.RestoreNpcSystems(true);
			this.RestoreAnimationCulling();
			this.phase = NPCRagdollRecovery.RecoveryPhase.None;
			this.stableTime = 0f;
			this.currentGetUpNormalizedTime = 1f;
			NPCSimulationPause.RestoreParticipantAfterRagdoll(this);
			if (this.logRecovery)
			{
				Debug.Log("[NPCRagdollRecovery] Recovery completed on '" + base.name + "'.", this);
			}
		}

		// Token: 0x060071FB RID: 29179 RVA: 0x002E5740 File Offset: 0x002E3940
		private void StorePhysicalPose()
		{
			this.storedPhysicalHipsPosition = this.hips.position;
			this.storedPhysicalHeadPosition = this.head.position;
			this.storedPhysicalLeftFootPosition = this.leftFoot.position;
			this.storedPhysicalRightFootPosition = this.rightFoot.position;
			for (int i = 0; i < this.bodyParts.Count; i++)
			{
				NPCRagdollRecovery.BodyPart bodyPart = this.bodyParts[i];
				bodyPart.StoredPosition = bodyPart.Transform.position;
				bodyPart.StoredRotation = bodyPart.Transform.rotation;
			}
		}

		// Token: 0x060071FC RID: 29180 RVA: 0x002E57D4 File Offset: 0x002E39D4
		private void AlignAnimatedRootToPhysicalPose()
		{
			Vector3 vector = this.storedPhysicalHeadPosition - 0.5f * (this.storedPhysicalLeftFootPosition + this.storedPhysicalRightFootPosition);
			vector.y = 0f;
			Vector3 vector2 = this.head.position - 0.5f * (this.leftFoot.position + this.rightFoot.position);
			vector2.y = 0f;
			if (vector2.sqrMagnitude > 0.001f && vector.sqrMagnitude > 0.001f)
			{
				base.transform.rotation = Quaternion.FromToRotation(vector2, vector) * base.transform.rotation;
			}
		}

		// Token: 0x060071FD RID: 29181 RVA: 0x002E5894 File Offset: 0x002E3A94
		private void AlignAnimatedHipsToStoredPosition()
		{
			base.transform.position += this.storedPhysicalHipsPosition - this.hips.position;
			Physics.SyncTransforms();
		}

		// Token: 0x060071FE RID: 29182 RVA: 0x002E58C8 File Offset: 0x002E3AC8
		private void SnapshotAndSuspendNpcSystems()
		{
			this.previousFollowerEnabled = this.follower != null && this.follower.enabled;
			this.previousFollowerStopped = this.follower != null && this.follower.entityExists && this.follower.isStopped;
			this.previousControllerEnabled = this.npcController != null && this.npcController.enabled;
			this.previousNavigationLocked = this.npcController != null && this.npcController.IsNavigationLocked;
			this.navigationLockedByExternalActivity = this.previousNavigationLocked && this.wanderController != null && this.wanderController.ExternalControl;
			this.previousStateMachineEnabled = this.stateMachine != null && this.stateMachine.enabled;
			this.previousStumbleEnabled = this.stumbleHandler != null && this.stumbleHandler.enabled;
			this.previousLookAtEnabled = this.lookAt != null && this.lookAt.enabled;
			this.previousRootColliderEnabled = this.rootCollider != null && this.rootCollider.enabled;
			if (this.wanderController != null)
			{
				this.wanderController.SuspendForRecovery(this);
			}
			if (this.npcController != null)
			{
				this.npcController.StopAndLock();
			}
			if (this.follower != null)
			{
				if (this.follower.entityExists)
				{
					this.follower.isStopped = true;
				}
				this.follower.enabled = false;
			}
			if (this.npcController != null)
			{
				this.npcController.enabled = false;
			}
			if (this.stateMachine != null)
			{
				this.stateMachine.enabled = false;
			}
			if (this.lookAt != null)
			{
				this.lookAt.enabled = false;
			}
			if (this.stumbleHandler != null)
			{
				this.stumbleHandler.enabled = false;
			}
			if (this.rootCollider != null)
			{
				this.rootCollider.enabled = false;
			}
		}

		// Token: 0x060071FF RID: 29183 RVA: 0x002E5B04 File Offset: 0x002E3D04
		private void RestoreNpcSystems(bool restartWander = true)
		{
			if (this.rootCollider != null)
			{
				this.rootCollider.enabled = this.previousRootColliderEnabled;
			}
			if (this.follower != null)
			{
				this.follower.enabled = this.previousFollowerEnabled;
				if (this.previousFollowerEnabled)
				{
					this.follower.Teleport(base.transform.position, true);
				}
			}
			if (this.npcController != null)
			{
				this.npcController.enabled = this.previousControllerEnabled;
				bool flag = this.navigationLockedByExternalActivity && this.wanderController != null && !this.wanderController.ExternalControl;
				if (this.previousNavigationLocked && !flag)
				{
					this.npcController.StopAndLock();
				}
				else
				{
					this.npcController.UnlockNavigation(true);
				}
			}
			if (this.follower != null && this.follower.entityExists)
			{
				this.follower.isStopped = this.previousFollowerStopped;
			}
			if (this.stateMachine != null)
			{
				this.stateMachine.enabled = this.previousStateMachineEnabled;
			}
			if (this.stumbleHandler != null)
			{
				this.stumbleHandler.enabled = this.previousStumbleEnabled;
			}
			if (this.lookAt != null)
			{
				this.lookAt.enabled = this.previousLookAtEnabled;
			}
			if (this.wanderController != null)
			{
				this.wanderController.ResumeAfterRecovery(this, restartWander);
			}
		}

		// Token: 0x06007200 RID: 29184 RVA: 0x002E5C80 File Offset: 0x002E3E80
		private void HoldAnimationCulling()
		{
			this.previousCullingMode = this.animator.cullingMode;
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
		}

		// Token: 0x06007201 RID: 29185 RVA: 0x002E5D00 File Offset: 0x002E3F00
		private void RestoreAnimationCulling()
		{
			if (this.animator != null)
			{
				this.animator.cullingMode = this.previousCullingMode;
			}
			foreach (KeyValuePair<SkinnedMeshRenderer, bool> keyValuePair in this.rendererOffscreenStates)
			{
				if (keyValuePair.Key != null)
				{
					keyValuePair.Key.updateWhenOffscreen = keyValuePair.Value;
				}
			}
			this.rendererOffscreenStates.Clear();
		}

		// Token: 0x06007202 RID: 29186 RVA: 0x002E5D98 File Offset: 0x002E3F98
		private float GetRecoveryBodySpeed()
		{
			float num = 0f;
			if (this.hipsBody != null)
			{
				num = Mathf.Max(num, this.hipsBody.velocity.magnitude);
			}
			if (this.chestBody != null)
			{
				num = Mathf.Max(num, this.chestBody.velocity.magnitude);
			}
			return num;
		}

		// Token: 0x06007203 RID: 29187 RVA: 0x002E5DFC File Offset: 0x002E3FFC
		private void SetPhysicalState(bool physical)
		{
			for (int i = 0; i < this.bodyParts.Count; i++)
			{
				NPCRagdollRecovery.BodyPart bodyPart = this.bodyParts[i];
				if (bodyPart.Rigidbody != null)
				{
					bodyPart.Rigidbody.isKinematic = !physical;
					bodyPart.Rigidbody.interpolation = (physical ? RigidbodyInterpolation.Interpolate : RigidbodyInterpolation.None);
				}
				if (bodyPart.Collider != null)
				{
					bodyPart.Collider.enabled = physical;
				}
			}
		}

		// Token: 0x06007204 RID: 29188 RVA: 0x002E5E78 File Offset: 0x002E4078
		private void CorrectPhysicalPenetration()
		{
			float num;
			if (!this.TryGetGroundHeight(this.GetBodyFocusPosition(), out num))
			{
				return;
			}
			float num2 = float.PositiveInfinity;
			for (int i = 0; i < this.bodyParts.Count; i++)
			{
				Collider collider = this.bodyParts[i].Collider;
				if (collider != null && collider.enabled)
				{
					num2 = Mathf.Min(num2, collider.bounds.min.y);
				}
			}
			if (float.IsPositiveInfinity(num2) || num2 >= num - this.allowedGroundPenetration)
			{
				return;
			}
			float num3 = num + this.groundLiftPadding - num2;
			for (int j = 0; j < this.bodyParts.Count; j++)
			{
				Rigidbody rigidbody = this.bodyParts[j].Rigidbody;
				if (rigidbody != null)
				{
					rigidbody.position += Vector3.up * num3;
				}
			}
			Physics.SyncTransforms();
		}

		// Token: 0x06007205 RID: 29189 RVA: 0x002E5F70 File Offset: 0x002E4170
		private void SnapRootToGround()
		{
			float num;
			if (!this.TryGetGroundHeight(base.transform.position, out num))
			{
				return;
			}
			Vector3 position = base.transform.position;
			position.y = num + this.groundLiftPadding;
			base.transform.position = position;
			Physics.SyncTransforms();
		}

		// Token: 0x06007206 RID: 29190 RVA: 0x002E5FC0 File Offset: 0x002E41C0
		private Vector3 GetBodyFocusPosition()
		{
			if (this.hips != null && this.chest != null)
			{
				return Vector3.Lerp(this.hips.position, this.chest.position, 0.5f);
			}
			if (!(this.hips != null))
			{
				return base.transform.position + Vector3.up;
			}
			return this.hips.position;
		}

		// Token: 0x06007207 RID: 29191 RVA: 0x002E603C File Offset: 0x002E423C
		private bool TryGetGroundHeight(Vector3 focus, out float groundHeight)
		{
			groundHeight = 0f;
			RaycastHit[] array = Physics.RaycastAll(focus + Vector3.up * this.groundProbeHeight, Vector3.down, this.groundProbeDistance, this.groundLayers, QueryTriggerInteraction.Ignore);
			bool flag = false;
			float num = float.NegativeInfinity;
			foreach (RaycastHit raycastHit in array)
			{
				if (!(raycastHit.collider == null) && !this.ownColliders.Contains(raycastHit.collider) && raycastHit.point.y > num)
				{
					num = raycastHit.point.y;
					flag = true;
				}
			}
			if (flag)
			{
				groundHeight = num;
			}
			return flag;
		}

		// Token: 0x06007208 RID: 29192 RVA: 0x002E60EC File Offset: 0x002E42EC
		private bool EnsurePhysicalRig()
		{
			if (this.physicalRigReady && this.hipsBody != null && this.chestBody != null && this.bodyParts.Count >= 10)
			{
				return true;
			}
			NPCRagdollRecovery.HumanoidBones humanoidBones;
			if (!this.TryGetRequiredBones(out humanoidBones))
			{
				return false;
			}
			this.BuildOrRepairRagdoll(humanoidBones);
			this.CacheBodyParts();
			this.SetPhysicalState(false);
			this.physicalRigReady = this.hipsBody != null && this.chestBody != null && this.bodyParts.Count >= 10;
			return this.physicalRigReady;
		}

		// Token: 0x06007209 RID: 29193 RVA: 0x002E618C File Offset: 0x002E438C
		private void CacheBodyParts()
		{
			this.bodyParts.Clear();
			this.ownColliders.Clear();
			foreach (Rigidbody rigidbody in this.hips.GetComponentsInChildren<Rigidbody>(true))
			{
				Collider component = rigidbody.GetComponent<Collider>();
				this.bodyParts.Add(new NPCRagdollRecovery.BodyPart
				{
					Transform = rigidbody.transform,
					Rigidbody = rigidbody,
					Collider = component
				});
				if (component != null)
				{
					this.ownColliders.Add(component);
				}
			}
			Collider[] componentsInChildren2 = base.GetComponentsInChildren<Collider>(true);
			for (int j = 0; j < componentsInChildren2.Length; j++)
			{
				this.ownColliders.Add(componentsInChildren2[j]);
			}
			this.hipsBody = this.hips.GetComponent<Rigidbody>();
			this.chestBody = this.chest.GetComponent<Rigidbody>();
		}

		// Token: 0x0600720A RID: 29194 RVA: 0x002E6268 File Offset: 0x002E4468
		private bool TryGetRequiredBones(out NPCRagdollRecovery.HumanoidBones bones)
		{
			bones = null;
			if (this.animator == null || !this.animator.isHuman)
			{
				return false;
			}
			NPCRagdollRecovery.HumanoidBones humanoidBones = new NPCRagdollRecovery.HumanoidBones
			{
				Hips = this.animator.GetBoneTransform(HumanBodyBones.Hips),
				Chest = this.animator.GetBoneTransform(HumanBodyBones.Chest),
				Head = this.animator.GetBoneTransform(HumanBodyBones.Head),
				LeftUpperLeg = this.animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg),
				LeftLowerLeg = this.animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg),
				LeftFoot = this.animator.GetBoneTransform(HumanBodyBones.LeftFoot),
				RightUpperLeg = this.animator.GetBoneTransform(HumanBodyBones.RightUpperLeg),
				RightLowerLeg = this.animator.GetBoneTransform(HumanBodyBones.RightLowerLeg),
				RightFoot = this.animator.GetBoneTransform(HumanBodyBones.RightFoot),
				LeftUpperArm = this.animator.GetBoneTransform(HumanBodyBones.LeftUpperArm),
				LeftLowerArm = this.animator.GetBoneTransform(HumanBodyBones.LeftLowerArm),
				LeftHand = this.animator.GetBoneTransform(HumanBodyBones.LeftHand),
				RightUpperArm = this.animator.GetBoneTransform(HumanBodyBones.RightUpperArm),
				RightLowerArm = this.animator.GetBoneTransform(HumanBodyBones.RightLowerArm),
				RightHand = this.animator.GetBoneTransform(HumanBodyBones.RightHand)
			};
			bool flag = humanoidBones.Hips != null && humanoidBones.Chest != null && humanoidBones.Head != null && humanoidBones.LeftUpperLeg != null && humanoidBones.LeftLowerLeg != null && humanoidBones.LeftFoot != null && humanoidBones.RightUpperLeg != null && humanoidBones.RightLowerLeg != null && humanoidBones.RightFoot != null && humanoidBones.LeftUpperArm != null && humanoidBones.LeftLowerArm != null && humanoidBones.LeftHand != null && humanoidBones.RightUpperArm != null && humanoidBones.RightLowerArm != null && humanoidBones.RightHand != null;
			if (flag)
			{
				bones = humanoidBones;
			}
			return flag;
		}

		// Token: 0x0600720B RID: 29195 RVA: 0x002E6498 File Offset: 0x002E4698
		private void BuildOrRepairRagdoll(NPCRagdollRecovery.HumanoidBones b)
		{
			int num = LayerMask.NameToLayer(this.bodyPartLayer);
			Rigidbody rigidbody = this.EnsureBody(b.Hips, 10f, num);
			Rigidbody rigidbody2 = this.EnsureBody(b.Chest, 14f, num);
			this.EnsureBody(b.Head, 5f, num);
			Rigidbody rigidbody3 = this.EnsureBody(b.LeftUpperLeg, 6f, num);
			this.EnsureBody(b.LeftLowerLeg, 4f, num);
			Rigidbody rigidbody4 = this.EnsureBody(b.RightUpperLeg, 6f, num);
			this.EnsureBody(b.RightLowerLeg, 4f, num);
			Rigidbody rigidbody5 = this.EnsureBody(b.LeftUpperArm, 2f, num);
			this.EnsureBody(b.LeftLowerArm, 1.5f, num);
			Rigidbody rigidbody6 = this.EnsureBody(b.RightUpperArm, 2f, num);
			this.EnsureBody(b.RightLowerArm, 1.5f, num);
			NPCRagdollRecovery.EnsureCapsule(b.LeftUpperLeg, b.LeftLowerLeg, 0.3f);
			NPCRagdollRecovery.EnsureCapsule(b.LeftLowerLeg, b.LeftFoot, 0.25f);
			NPCRagdollRecovery.EnsureCapsule(b.RightUpperLeg, b.RightLowerLeg, 0.3f);
			NPCRagdollRecovery.EnsureCapsule(b.RightLowerLeg, b.RightFoot, 0.25f);
			NPCRagdollRecovery.EnsureCapsule(b.LeftUpperArm, b.LeftLowerArm, 0.25f);
			NPCRagdollRecovery.EnsureCapsule(b.LeftLowerArm, b.LeftHand, 0.22f);
			NPCRagdollRecovery.EnsureCapsule(b.RightUpperArm, b.RightLowerArm, 0.25f);
			NPCRagdollRecovery.EnsureCapsule(b.RightLowerArm, b.RightHand, 0.22f);
			NPCRagdollRecovery.EnsurePelvisBox(b);
			NPCRagdollRecovery.EnsureChestBox(b);
			NPCRagdollRecovery.EnsureHeadSphere(b);
			Vector3 right = base.transform.right;
			Vector3 up = base.transform.up;
			Vector3 forward = base.transform.forward;
			NPCRagdollRecovery.EnsureJoint(b.LeftUpperLeg, rigidbody, right, forward, -25f, 65f, 30f);
			NPCRagdollRecovery.EnsureJoint(b.LeftLowerLeg, rigidbody3, right, forward, -85f, 5f, 8f);
			NPCRagdollRecovery.EnsureJoint(b.RightUpperLeg, rigidbody, right, forward, -25f, 65f, 30f);
			NPCRagdollRecovery.EnsureJoint(b.RightLowerLeg, rigidbody4, right, forward, -85f, 5f, 8f);
			NPCRagdollRecovery.EnsureJoint(b.Chest, rigidbody, right, forward, -25f, 25f, 18f);
			NPCRagdollRecovery.EnsureJoint(b.LeftUpperArm, rigidbody2, up, forward, -75f, 20f, 55f);
			NPCRagdollRecovery.EnsureJoint(b.LeftLowerArm, rigidbody5, forward, up, -100f, 5f, 8f);
			NPCRagdollRecovery.EnsureJoint(b.RightUpperArm, rigidbody2, up, forward, -75f, 20f, 55f);
			NPCRagdollRecovery.EnsureJoint(b.RightLowerArm, rigidbody6, forward, up, -100f, 5f, 8f);
			NPCRagdollRecovery.EnsureJoint(b.Head, rigidbody2, right, forward, -40f, 30f, 30f);
		}

		// Token: 0x0600720C RID: 29196 RVA: 0x002E67A8 File Offset: 0x002E49A8
		private Rigidbody EnsureBody(Transform bone, float mass, int layer)
		{
			Rigidbody rigidbody = bone.GetComponent<Rigidbody>();
			if (rigidbody == null)
			{
				rigidbody = bone.gameObject.AddComponent<Rigidbody>();
			}
			rigidbody.mass = mass;
			rigidbody.drag = this.bodyDrag;
			rigidbody.angularDrag = this.bodyAngularDrag;
			rigidbody.maxAngularVelocity = this.maxAngularVelocity;
			rigidbody.useGravity = true;
			rigidbody.isKinematic = true;
			rigidbody.interpolation = RigidbodyInterpolation.None;
			rigidbody.collisionDetectionMode = this.collisionDetection;
			rigidbody.constraints = RigidbodyConstraints.None;
			if (layer >= 0)
			{
				bone.gameObject.layer = layer;
			}
			return rigidbody;
		}

		// Token: 0x0600720D RID: 29197 RVA: 0x002E6838 File Offset: 0x002E4A38
		private static void EnsureCapsule(Transform bone, Transform endBone, float radiusScale)
		{
			CapsuleCollider capsuleCollider = bone.GetComponent<CapsuleCollider>();
			if (capsuleCollider == null)
			{
				capsuleCollider = bone.gameObject.AddComponent<CapsuleCollider>();
			}
			Vector3 vector = bone.InverseTransformPoint(endBone.position);
			int num = NPCRagdollRecovery.LargestComponent(vector);
			float num2 = vector[num];
			float num3 = Mathf.Abs(num2);
			float num4 = Mathf.Max(0.025f, num3 * radiusScale);
			Vector3 zero = Vector3.zero;
			zero[num] = num2 * 0.5f;
			capsuleCollider.direction = num;
			capsuleCollider.center = zero;
			capsuleCollider.radius = num4;
			capsuleCollider.height = Mathf.Max(num3, num4 * 2f);
			capsuleCollider.isTrigger = false;
			capsuleCollider.enabled = false;
		}

		// Token: 0x0600720E RID: 29198 RVA: 0x002E68E8 File Offset: 0x002E4AE8
		private static void EnsurePelvisBox(NPCRagdollRecovery.HumanoidBones b)
		{
			BoxCollider boxCollider = b.Hips.GetComponent<BoxCollider>();
			if (boxCollider == null)
			{
				boxCollider = b.Hips.gameObject.AddComponent<BoxCollider>();
			}
			Bounds bounds = NPCRagdollRecovery.BoundsFromWorldPoints(b.Hips, new Vector3[]
			{
				b.LeftUpperLeg.position,
				b.RightUpperLeg.position,
				Vector3.Lerp(b.Hips.position, b.Chest.position, 0.48f)
			});
			NPCRagdollRecovery.ExpandThinAxis(ref bounds, 0.55f, 0.12f);
			boxCollider.center = bounds.center;
			boxCollider.size = NPCRagdollRecovery.MaxComponents(bounds.size, new Vector3(0.16f, 0.14f, 0.12f));
			boxCollider.isTrigger = false;
			boxCollider.enabled = false;
		}

		// Token: 0x0600720F RID: 29199 RVA: 0x002E69CC File Offset: 0x002E4BCC
		private static void EnsureChestBox(NPCRagdollRecovery.HumanoidBones b)
		{
			BoxCollider boxCollider = b.Chest.GetComponent<BoxCollider>();
			if (boxCollider == null)
			{
				boxCollider = b.Chest.gameObject.AddComponent<BoxCollider>();
			}
			Bounds bounds = NPCRagdollRecovery.BoundsFromWorldPoints(b.Chest, new Vector3[]
			{
				Vector3.Lerp(b.Hips.position, b.Chest.position, 0.52f),
				Vector3.Lerp(b.Chest.position, b.Head.position, 0.48f),
				b.LeftUpperArm.position,
				b.RightUpperArm.position
			});
			NPCRagdollRecovery.ExpandThinAxis(ref bounds, 0.45f, 0.14f);
			boxCollider.center = bounds.center;
			boxCollider.size = NPCRagdollRecovery.MaxComponents(bounds.size, new Vector3(0.18f, 0.18f, 0.14f));
			boxCollider.isTrigger = false;
			boxCollider.enabled = false;
		}

		// Token: 0x06007210 RID: 29200 RVA: 0x002E6AD8 File Offset: 0x002E4CD8
		private static void EnsureHeadSphere(NPCRagdollRecovery.HumanoidBones b)
		{
			SphereCollider sphereCollider = b.Head.GetComponent<SphereCollider>();
			if (sphereCollider == null)
			{
				sphereCollider = b.Head.gameObject.AddComponent<SphereCollider>();
			}
			float num = Mathf.Clamp(Vector3.Distance(b.LeftUpperArm.position, b.RightUpperArm.position) * 0.22f, 0.09f, 0.18f);
			float num2 = Mathf.Max(new float[]
			{
				Mathf.Abs(b.Head.lossyScale.x),
				Mathf.Abs(b.Head.lossyScale.y),
				Mathf.Abs(b.Head.lossyScale.z)
			});
			sphereCollider.radius = num / Mathf.Max(num2, 0.0001f);
			Vector3 vector = b.Head.InverseTransformDirection((b.Head.position - b.Chest.position).normalized);
			sphereCollider.center = vector * sphereCollider.radius * 0.35f;
			sphereCollider.isTrigger = false;
			sphereCollider.enabled = false;
		}

		// Token: 0x06007211 RID: 29201 RVA: 0x002E6C00 File Offset: 0x002E4E00
		private static void EnsureJoint(Transform bone, Rigidbody connectedBody, Vector3 worldTwistAxis, Vector3 worldSwingAxis, float lowTwist, float highTwist, float swing)
		{
			CharacterJoint characterJoint = bone.GetComponent<CharacterJoint>();
			if (characterJoint == null)
			{
				characterJoint = bone.gameObject.AddComponent<CharacterJoint>();
			}
			characterJoint.connectedBody = connectedBody;
			characterJoint.anchor = Vector3.zero;
			characterJoint.axis = NPCRagdollRecovery.DominantAxis(bone.InverseTransformDirection(worldTwistAxis));
			characterJoint.swingAxis = NPCRagdollRecovery.DominantAxis(bone.InverseTransformDirection(worldSwingAxis));
			characterJoint.enableProjection = true;
			characterJoint.enableCollision = false;
			SoftJointLimit softJointLimit = characterJoint.lowTwistLimit;
			NPCRagdollRecovery.SetLimit(ref softJointLimit, lowTwist);
			characterJoint.lowTwistLimit = softJointLimit;
			softJointLimit = characterJoint.highTwistLimit;
			NPCRagdollRecovery.SetLimit(ref softJointLimit, highTwist);
			characterJoint.highTwistLimit = softJointLimit;
			softJointLimit = characterJoint.swing1Limit;
			NPCRagdollRecovery.SetLimit(ref softJointLimit, swing);
			characterJoint.swing1Limit = softJointLimit;
			softJointLimit = characterJoint.swing2Limit;
			NPCRagdollRecovery.SetLimit(ref softJointLimit, swing * 0.65f);
			characterJoint.swing2Limit = softJointLimit;
		}

		// Token: 0x06007212 RID: 29202 RVA: 0x002E6CD0 File Offset: 0x002E4ED0
		private static Bounds BoundsFromWorldPoints(Transform relativeTo, params Vector3[] points)
		{
			Bounds bounds = new Bounds(relativeTo.InverseTransformPoint(points[0]), Vector3.zero);
			for (int i = 1; i < points.Length; i++)
			{
				bounds.Encapsulate(relativeTo.InverseTransformPoint(points[i]));
			}
			return bounds;
		}

		// Token: 0x06007213 RID: 29203 RVA: 0x002E6D1C File Offset: 0x002E4F1C
		private static void ExpandThinAxis(ref Bounds bounds, float fractionOfLargest, float minimum)
		{
			Vector3 size = bounds.size;
			int num = NPCRagdollRecovery.SmallestComponent(size);
			int num2 = NPCRagdollRecovery.LargestComponent(size);
			size[num] = Mathf.Max(size[num], Mathf.Max(size[num2] * fractionOfLargest, minimum));
			bounds.size = size;
		}

		// Token: 0x06007214 RID: 29204 RVA: 0x00119C8A File Offset: 0x00117E8A
		private static Vector3 MaxComponents(Vector3 value, Vector3 minimum)
		{
			return new Vector3(Mathf.Max(value.x, minimum.x), Mathf.Max(value.y, minimum.y), Mathf.Max(value.z, minimum.z));
		}

		// Token: 0x06007215 RID: 29205 RVA: 0x002E6D6C File Offset: 0x002E4F6C
		private static int LargestComponent(Vector3 value)
		{
			int num = 0;
			if (Mathf.Abs(value.y) > Mathf.Abs(value.x))
			{
				num = 1;
			}
			if (Mathf.Abs(value.z) > Mathf.Abs(value[num]))
			{
				num = 2;
			}
			return num;
		}

		// Token: 0x06007216 RID: 29206 RVA: 0x002E6DB4 File Offset: 0x002E4FB4
		private static int SmallestComponent(Vector3 value)
		{
			int num = 0;
			if (Mathf.Abs(value.y) < Mathf.Abs(value.x))
			{
				num = 1;
			}
			if (Mathf.Abs(value.z) < Mathf.Abs(value[num]))
			{
				num = 2;
			}
			return num;
		}

		// Token: 0x06007217 RID: 29207 RVA: 0x002E6DFC File Offset: 0x002E4FFC
		private static Vector3 DominantAxis(Vector3 value)
		{
			int num = NPCRagdollRecovery.LargestComponent(value);
			Vector3 zero = Vector3.zero;
			zero[num] = ((value[num] >= 0f) ? 1f : (-1f));
			return zero;
		}

		// Token: 0x06007218 RID: 29208 RVA: 0x00119D92 File Offset: 0x00117F92
		private static void SetLimit(ref SoftJointLimit limit, float value)
		{
			limit.limit = value;
		}

		// Token: 0x06007219 RID: 29209 RVA: 0x002E6E3A File Offset: 0x002E503A
		private void DestroyGetUpGraph()
		{
			if (this.getUpGraph.IsValid())
			{
				this.getUpGraph.Destroy();
			}
			this.activeGetUpClip = null;
		}

		// Token: 0x0600721A RID: 29210 RVA: 0x002E6E5B File Offset: 0x002E505B
		private void ApplyGetUpPlayableSimulationScale()
		{
			if (this.phase == NPCRagdollRecovery.RecoveryPhase.GettingUp && this.getUpPlayable.IsValid<AnimationClipPlayable>())
			{
				this.getUpPlayable.SetSpeed((double)NPCSimulationPause.SimulationScale);
			}
		}

		// Token: 0x0600721B RID: 29211 RVA: 0x002E6E84 File Offset: 0x002E5084
		private void OnDisable()
		{
			WorldOrigin.Shifted -= this.OnWorldOriginShifted;
			NPCSimulationPause.PauseChanged -= this.HandleSimulationPauseChanged;
			this.DestroyGetUpGraph();
			if (this.IsRecovering)
			{
				this.SetPhysicalState(false);
				if (this.animator != null)
				{
					this.animator.enabled = true;
				}
				this.RestoreNpcSystems(true);
				this.RestoreAnimationCulling();
				this.phase = NPCRagdollRecovery.RecoveryPhase.None;
				NPCSimulationPause.RestoreParticipantAfterRagdoll(this);
			}
		}

		// Token: 0x0600721C RID: 29212 RVA: 0x002E6EFC File Offset: 0x002E50FC
		private void OnWorldOriginShifted(Vector3 delta)
		{
			if (!this.IsRecovering)
			{
				return;
			}
			this.storedPhysicalHipsPosition += delta;
			this.storedPhysicalHeadPosition += delta;
			this.storedPhysicalLeftFootPosition += delta;
			this.storedPhysicalRightFootPosition += delta;
			for (int i = 0; i < this.bodyParts.Count; i++)
			{
				this.bodyParts[i].StoredPosition += delta;
			}
		}

		// Token: 0x040074E5 RID: 29925
		[Header("Automatic recovery")]
		[SerializeField]
		[Min(0f)]
		private float minimumRecoveryDelay = 2.1f;

		// Token: 0x040074E6 RID: 29926
		[SerializeField]
		[Min(0f)]
		private float stableVelocity = 0.4f;

		// Token: 0x040074E7 RID: 29927
		[SerializeField]
		[Min(0f)]
		private float stableDuration = 0.55f;

		// Token: 0x040074E8 RID: 29928
		[SerializeField]
		[Min(0f)]
		private float forcedRecoveryDelay = 5.25f;

		// Token: 0x040074E9 RID: 29929
		[SerializeField]
		[Min(0f)]
		private float poseBlendDuration = 0.25f;

		// Token: 0x040074EA RID: 29930
		[SerializeField]
		[Min(0f)]
		private float maximumGetUpDuration = 4.5f;

		// Token: 0x040074EB RID: 29931
		[SerializeField]
		[Range(0.5f, 1f)]
		private float getUpCompletion = 0.98f;

		// Token: 0x040074EC RID: 29932
		[Header("Physics")]
		[SerializeField]
		private string bodyPartLayer = "BodyPart";

		// Token: 0x040074ED RID: 29933
		[SerializeField]
		private CollisionDetectionMode collisionDetection = CollisionDetectionMode.ContinuousSpeculative;

		// Token: 0x040074EE RID: 29934
		[SerializeField]
		[Min(0f)]
		private float bodyDrag = 0.02f;

		// Token: 0x040074EF RID: 29935
		[SerializeField]
		[Min(0f)]
		private float bodyAngularDrag = 0.12f;

		// Token: 0x040074F0 RID: 29936
		[SerializeField]
		[Min(1f)]
		private float maxAngularVelocity = 25f;

		// Token: 0x040074F1 RID: 29937
		[Header("Ground safety")]
		[SerializeField]
		private LayerMask groundLayers = -1;

		// Token: 0x040074F2 RID: 29938
		[SerializeField]
		[Min(0.1f)]
		private float groundProbeHeight = 2.5f;

		// Token: 0x040074F3 RID: 29939
		[SerializeField]
		[Min(0.1f)]
		private float groundProbeDistance = 6f;

		// Token: 0x040074F4 RID: 29940
		[SerializeField]
		[Min(0f)]
		private float allowedGroundPenetration = 0.04f;

		// Token: 0x040074F5 RID: 29941
		[SerializeField]
		[Min(0f)]
		private float groundLiftPadding = 0.025f;

		// Token: 0x040074F6 RID: 29942
		[Header("Animation")]
		[SerializeField]
		private NPCRagdollAnimationProfile animationProfile;

		// Token: 0x040074F7 RID: 29943
		[Header("Debug")]
		[SerializeField]
		private bool logRecovery;

		// Token: 0x040074F8 RID: 29944
		private const string AnimationProfileResourceName = "NPCRagdollAnimationProfile";

		// Token: 0x040074F9 RID: 29945
		private readonly List<NPCRagdollRecovery.BodyPart> bodyParts = new List<NPCRagdollRecovery.BodyPart>();

		// Token: 0x040074FA RID: 29946
		private readonly HashSet<Collider> ownColliders = new HashSet<Collider>();

		// Token: 0x040074FB RID: 29947
		private readonly Dictionary<SkinnedMeshRenderer, bool> rendererOffscreenStates = new Dictionary<SkinnedMeshRenderer, bool>();

		// Token: 0x040074FC RID: 29948
		private Animator animator;

		// Token: 0x040074FD RID: 29949
		private CapsuleCollider rootCollider;

		// Token: 0x040074FE RID: 29950
		private FollowerEntity follower;

		// Token: 0x040074FF RID: 29951
		private AStarNPCController npcController;

		// Token: 0x04007500 RID: 29952
		private NPCWanderController wanderController;

		// Token: 0x04007501 RID: 29953
		private NPCStateMachine stateMachine;

		// Token: 0x04007502 RID: 29954
		private NPCStumbleHandler stumbleHandler;

		// Token: 0x04007503 RID: 29955
		private NPCSimpleLookAt lookAt;

		// Token: 0x04007504 RID: 29956
		private Transform hips;

		// Token: 0x04007505 RID: 29957
		private Transform chest;

		// Token: 0x04007506 RID: 29958
		private Transform head;

		// Token: 0x04007507 RID: 29959
		private Transform leftFoot;

		// Token: 0x04007508 RID: 29960
		private Transform rightFoot;

		// Token: 0x04007509 RID: 29961
		private Rigidbody hipsBody;

		// Token: 0x0400750A RID: 29962
		private Rigidbody chestBody;

		// Token: 0x0400750B RID: 29963
		private Vector3 hipsLocalCharacterForward;

		// Token: 0x0400750C RID: 29964
		private bool physicalRigReady;

		// Token: 0x0400750D RID: 29965
		private NPCRagdollRecovery.RecoveryPhase phase;

		// Token: 0x0400750E RID: 29966
		private float phaseStartedTime;

		// Token: 0x0400750F RID: 29967
		private float stableTime;

		// Token: 0x04007510 RID: 29968
		private bool getUpFromBack;

		// Token: 0x04007511 RID: 29969
		private float currentGetUpNormalizedTime;

		// Token: 0x04007512 RID: 29970
		private Vector3 storedPhysicalHipsPosition;

		// Token: 0x04007513 RID: 29971
		private Vector3 storedPhysicalHeadPosition;

		// Token: 0x04007514 RID: 29972
		private Vector3 storedPhysicalLeftFootPosition;

		// Token: 0x04007515 RID: 29973
		private Vector3 storedPhysicalRightFootPosition;

		// Token: 0x04007516 RID: 29974
		private PlayableGraph getUpGraph;

		// Token: 0x04007517 RID: 29975
		private AnimationClipPlayable getUpPlayable;

		// Token: 0x04007518 RID: 29976
		private AnimationClip activeGetUpClip;

		// Token: 0x04007519 RID: 29977
		private bool previousFollowerEnabled;

		// Token: 0x0400751A RID: 29978
		private bool previousFollowerStopped;

		// Token: 0x0400751B RID: 29979
		private bool previousControllerEnabled;

		// Token: 0x0400751C RID: 29980
		private bool previousNavigationLocked;

		// Token: 0x0400751D RID: 29981
		private bool navigationLockedByExternalActivity;

		// Token: 0x0400751E RID: 29982
		private bool previousStateMachineEnabled;

		// Token: 0x0400751F RID: 29983
		private bool previousStumbleEnabled;

		// Token: 0x04007520 RID: 29984
		private bool previousLookAtEnabled;

		// Token: 0x04007521 RID: 29985
		private bool previousRootColliderEnabled;

		// Token: 0x04007522 RID: 29986
		private AnimatorCullingMode previousCullingMode;

		// Token: 0x04007523 RID: 29987
		private bool simulationPauseActive;

		// Token: 0x04007524 RID: 29988
		private float simulationPauseStartedAt;

		// Token: 0x04007525 RID: 29989
		private bool getUpPlayablePausedBySimulation;

		// Token: 0x04007526 RID: 29990
		private double getUpPlayableSpeedBeforeSimulationPause;

		// Token: 0x02000EDF RID: 3807
		private enum RecoveryPhase
		{
			// Token: 0x04007529 RID: 29993
			None,
			// Token: 0x0400752A RID: 29994
			Ragdolled,
			// Token: 0x0400752B RID: 29995
			BlendingToGetUp,
			// Token: 0x0400752C RID: 29996
			GettingUp
		}

		// Token: 0x02000EE0 RID: 3808
		private sealed class BodyPart
		{
			// Token: 0x0400752D RID: 29997
			public Transform Transform;

			// Token: 0x0400752E RID: 29998
			public Rigidbody Rigidbody;

			// Token: 0x0400752F RID: 29999
			public Collider Collider;

			// Token: 0x04007530 RID: 30000
			public Vector3 StoredPosition;

			// Token: 0x04007531 RID: 30001
			public Quaternion StoredRotation;
		}

		// Token: 0x02000EE1 RID: 3809
		private sealed class HumanoidBones
		{
			// Token: 0x04007532 RID: 30002
			public Transform Hips;

			// Token: 0x04007533 RID: 30003
			public Transform Chest;

			// Token: 0x04007534 RID: 30004
			public Transform Head;

			// Token: 0x04007535 RID: 30005
			public Transform LeftUpperLeg;

			// Token: 0x04007536 RID: 30006
			public Transform LeftLowerLeg;

			// Token: 0x04007537 RID: 30007
			public Transform LeftFoot;

			// Token: 0x04007538 RID: 30008
			public Transform RightUpperLeg;

			// Token: 0x04007539 RID: 30009
			public Transform RightLowerLeg;

			// Token: 0x0400753A RID: 30010
			public Transform RightFoot;

			// Token: 0x0400753B RID: 30011
			public Transform LeftUpperArm;

			// Token: 0x0400753C RID: 30012
			public Transform LeftLowerArm;

			// Token: 0x0400753D RID: 30013
			public Transform LeftHand;

			// Token: 0x0400753E RID: 30014
			public Transform RightUpperArm;

			// Token: 0x0400753F RID: 30015
			public Transform RightLowerArm;

			// Token: 0x04007540 RID: 30016
			public Transform RightHand;
		}
	}
}
