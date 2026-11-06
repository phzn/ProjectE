using System;
using System.Collections.Generic;
using Features.NPC;
using Features.NPC.Audio;
using Features.NPC.StateMachine;
using Features.RenderingPrecision;
using Features.WardrobeCombos;
using Pathfinding;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x020012B4 RID: 4788
	[DefaultExecutionOrder(-90)]
	public sealed class GhostPerkFleeController : MonoBehaviour
	{
		// Token: 0x0600947E RID: 38014 RVA: 0x003D081A File Offset: 0x003CEA1A
		private void OnEnable()
		{
			WorldOrigin.Shifted += this.OnWorldOriginShifted;
		}

		// Token: 0x0600947F RID: 38015 RVA: 0x003D0830 File Offset: 0x003CEA30
		private void OnWorldOriginShifted(Vector3 delta)
		{
			if (!this._started || this._cleaned)
			{
				return;
			}
			this._threatPosition += delta;
			this._lastPlannedThreat += delta;
			this._destination += delta;
			this._movementSample += delta;
			if (this._manualWaypoints != null)
			{
				for (int i = 0; i < this._manualWaypoints.Count; i++)
				{
					List<Vector3> manualWaypoints = this._manualWaypoints;
					int num = i;
					manualWaypoints[num] += delta;
				}
			}
			this._manualPathRequestVersion++;
			if (this._manualPathPending)
			{
				this.RequestManualPath();
			}
		}

		// Token: 0x06009480 RID: 38016 RVA: 0x003D08F0 File Offset: 0x003CEAF0
		public static bool BeginOrRefresh(NPCSenses senses, Vector3 threatPosition)
		{
			if (senses == null || !senses.gameObject.activeInHierarchy)
			{
				return false;
			}
			NPCRagdollRecovery component = senses.GetComponent<NPCRagdollRecovery>();
			if (component != null && component.IsRecovering)
			{
				senses.SuppressDetectionForGhost();
				return false;
			}
			GhostPerkFleeController ghostPerkFleeController = senses.GetComponent<GhostPerkFleeController>();
			if (ghostPerkFleeController == null)
			{
				ghostPerkFleeController = senses.gameObject.AddComponent<GhostPerkFleeController>();
			}
			bool flag = !ghostPerkFleeController._started;
			if (flag)
			{
				ghostPerkFleeController.Begin(senses, threatPosition);
				return flag;
			}
			ghostPerkFleeController._threatPosition = threatPosition;
			return flag;
		}

		// Token: 0x06009481 RID: 38017 RVA: 0x003D0970 File Offset: 0x003CEB70
		private void Begin(NPCSenses senses, Vector3 threatPosition)
		{
			this._senses = senses;
			this._threatPosition = threatPosition;
			this._wander = base.GetComponent<NPCWanderController>();
			this._simpleNavigator = base.GetComponent<SimplePOINavigator>();
			this._controller = base.GetComponent<AStarNPCController>();
			this._follower = base.GetComponent<FollowerEntity>();
			this._fsm = base.GetComponent<NPCStateMachine>();
			this._lookAt = base.GetComponent<NPCSimpleLookAt>();
			this._ragdollRecovery = base.GetComponent<NPCRagdollRecovery>();
			this._previousWanderExternalControl = this._wander != null && this._wander.ExternalControl;
			this._previousSimpleNavigatorEnabled = this._simpleNavigator != null && this._simpleNavigator.enabled;
			this._previousFollowerEnabled = this._follower != null && this._follower.enabled;
			this._previousFollowerStopped = this._follower != null && this._follower.isStopped;
			this._previousFollowerMaxSpeed = ((this._follower != null) ? this._follower.maxSpeed : (-1f));
			this._started = true;
			this._cleaned = false;
			this._manualMovement = false;
			this._manualPathPending = false;
			this._manualPathRequestVersion++;
			this._startedAt = Time.time;
			this._movementSample = base.transform.position;
			this._nextStuckCheck = Time.time + 1.4f;
			this._nextPlanTime = 0f;
			this._senses.IsGhostFleeing = true;
			this._senses.SuppressDetectionForGhost();
			if (this._wander != null)
			{
				this._wander.ForceStop();
				this._wander.ExternalControl = true;
			}
			if (this._simpleNavigator != null)
			{
				this._simpleNavigator.enabled = false;
			}
			NPCSimpleLookAt lookAt = this._lookAt;
			if (lookAt != null)
			{
				lookAt.StopLooking();
			}
			if (this._controller != null)
			{
				if (this._controller.IsManualMode)
				{
					this._controller.ExitManualMode(true);
				}
				this._controller.UnlockNavigation(true);
				this._controller.ClearSpeedOverride();
			}
			if (this._follower != null)
			{
				this._follower.enabled = true;
				this._follower.maxSpeed = 5.5f;
				this._follower.isStopped = false;
			}
			if (this._fsm != null && this._fsm.IsInitialized)
			{
				this._fsm.ForceState(NPCStateType.Wandering, "GhostPerk");
				this._fsm.Context.SetFacialExpression(ReactionType.Flee);
			}
			NPCReactionAudioPlayer component = base.GetComponent<NPCReactionAudioPlayer>();
			if (component != null)
			{
				component.PlayStopAndStareAudio();
			}
			this.PlanFleeDestination();
			if (this._controller == null || this._follower == null)
			{
				this.BeginManualMovement();
			}
		}

		// Token: 0x06009482 RID: 38018 RVA: 0x003D0C44 File Offset: 0x003CEE44
		private void Update()
		{
			if (!this._started || this._cleaned)
			{
				return;
			}
			if (this.IsRagdollRecovering())
			{
				this._movementSample = base.transform.position;
				this._nextStuckCheck = Time.time + 1.4f;
				NPCSenses senses = this._senses;
				if (senses == null)
				{
					return;
				}
				senses.SuppressDetectionForGhost();
				return;
			}
			else
			{
				if (!WardrobeComboRuntime.HasGhost)
				{
					this.Finish();
					return;
				}
				if (NPCSimulationPause.IsPaused || NPCSimulationPause.SimulationScale <= 0.0001f)
				{
					return;
				}
				NPCSenses senses2 = this._senses;
				if (senses2 != null)
				{
					senses2.SuppressDetectionForGhost();
				}
				if (this._wander != null)
				{
					this._wander.ExternalControl = true;
				}
				if (this._simpleNavigator != null && this._simpleNavigator.enabled)
				{
					this._simpleNavigator.enabled = false;
				}
				float num = GhostPerkFleeController.PlanarDistance(base.transform.position, this._threatPosition);
				float num2 = Time.time - this._startedAt;
				if ((num2 >= 1.5f && num >= 25f) || num2 >= 14f)
				{
					this.Finish();
					return;
				}
				if (!this._manualMovement)
				{
					if (Time.time >= this._nextPlanTime && GhostPerkFleeController.PlanarDistance(this._lastPlannedThreat, this._threatPosition) >= 3f)
					{
						this.PlanFleeDestination();
					}
					if (this._controller != null && this._controller.HasReachedDestination() && num2 >= 1.5f)
					{
						this.Finish();
						return;
					}
					if (Time.time >= this._nextStuckCheck)
					{
						float num3 = GhostPerkFleeController.PlanarDistance(base.transform.position, this._movementSample);
						this._movementSample = base.transform.position;
						this._nextStuckCheck = Time.time + 1.4f;
						if (num3 < 0.45f)
						{
							this.BeginManualMovement();
							return;
						}
					}
				}
				else
				{
					this.StepManualMovement();
				}
				return;
			}
		}

		// Token: 0x06009483 RID: 38019 RVA: 0x003D0E0D File Offset: 0x003CF00D
		private void LateUpdate()
		{
			if (this._started && !this._cleaned && this._manualMovement && this._follower != null && !this.IsRagdollRecovering())
			{
				this._follower.enabled = false;
			}
		}

		// Token: 0x06009484 RID: 38020 RVA: 0x003D0E4C File Offset: 0x003CF04C
		private void PlanFleeDestination()
		{
			Vector3 vector = base.transform.position - this._threatPosition;
			vector.y = 0f;
			if (vector.sqrMagnitude < 0.001f)
			{
				vector = -base.transform.forward;
			}
			vector.Normalize();
			Vector3 normalized = Vector3.Cross(Vector3.up, vector).normalized;
			this._destination = base.transform.position + vector * 30f;
			AstarPath active = AstarPath.active;
			if (active != null)
			{
				NNInfo nearest = active.GetNearest(base.transform.position);
				Vector3[] array = new Vector3[]
				{
					base.transform.position + vector * 30f,
					base.transform.position + vector * 24f + normalized * 8f,
					base.transform.position + vector * 24f - normalized * 8f,
					base.transform.position + Quaternion.AngleAxis(35f, Vector3.up) * vector * 24f,
					base.transform.position + Quaternion.AngleAxis(-35f, Vector3.up) * vector * 24f
				};
				if (nearest.node != null && nearest.node.Walkable)
				{
					for (int i = 0; i < array.Length; i++)
					{
						NNInfo nearest2 = active.GetNearest(array[i]);
						if (nearest2.node != null && nearest2.node.Walkable && PathUtilities.IsPathPossible(nearest.node, nearest2.node))
						{
							this._destination = nearest2.position;
							break;
						}
					}
				}
			}
			this._lastPlannedThreat = this._threatPosition;
			this._nextPlanTime = Time.time + 1.25f;
			if (this._controller != null)
			{
				this._controller.UnlockNavigation(false);
				this._controller.SetDestination(this._destination);
			}
			if (this._follower != null)
			{
				this._follower.maxSpeed = 5.5f;
				this._follower.isStopped = false;
			}
		}

		// Token: 0x06009485 RID: 38021 RVA: 0x003D10E0 File Offset: 0x003CF2E0
		private void BeginManualMovement()
		{
			if (this._manualMovement)
			{
				return;
			}
			this._manualMovement = true;
			this._manualPathPending = false;
			this._manualWaypoints = null;
			this._manualWaypointIndex = 0;
			this._manualPathDeadline = Time.time + 0.75f;
			AStarNPCController controller = this._controller;
			if (controller != null)
			{
				controller.Stop();
			}
			AStarNPCController controller2 = this._controller;
			if (controller2 != null)
			{
				controller2.SetSpeedOverride(5.5f);
			}
			if (this._follower != null)
			{
				this._follower.enabled = false;
			}
			if (AstarPath.active != null)
			{
				this.RequestManualPath();
				return;
			}
			this._manualWaypoints = new List<Vector3> { this._destination };
		}

		// Token: 0x06009486 RID: 38022 RVA: 0x003D1190 File Offset: 0x003CF390
		private void RequestManualPath()
		{
			GhostPerkFleeController.<>c__DisplayClass43_0 CS$<>8__locals1 = new GhostPerkFleeController.<>c__DisplayClass43_0();
			CS$<>8__locals1.<>4__this = this;
			GhostPerkFleeController.<>c__DisplayClass43_0 CS$<>8__locals2 = CS$<>8__locals1;
			int num = this._manualPathRequestVersion + 1;
			this._manualPathRequestVersion = num;
			CS$<>8__locals2.version = num;
			this._manualPathPending = true;
			AstarPath.StartPath(ABPath.Construct(base.transform.position, this._destination, delegate(Path result)
			{
				if (CS$<>8__locals1.<>4__this != null && CS$<>8__locals1.version == CS$<>8__locals1.<>4__this._manualPathRequestVersion)
				{
					CS$<>8__locals1.<>4__this.OnManualPathReady(result);
				}
			}), false, false);
		}

		// Token: 0x06009487 RID: 38023 RVA: 0x003D11F4 File Offset: 0x003CF3F4
		private void OnManualPathReady(Path path)
		{
			if (!this._started || this._cleaned)
			{
				return;
			}
			path.Claim(this);
			this._manualPathPending = false;
			if (!path.error && path.vectorPath != null && path.vectorPath.Count > 0)
			{
				this._manualWaypoints = new List<Vector3>(path.vectorPath);
			}
			else
			{
				this._manualWaypoints = new List<Vector3> { this._destination };
			}
			path.Release(this, false);
		}

		// Token: 0x06009488 RID: 38024 RVA: 0x003D1270 File Offset: 0x003CF470
		private void StepManualMovement()
		{
			if (this.IsRagdollRecovering())
			{
				return;
			}
			if (this._manualPathPending && Time.time < this._manualPathDeadline)
			{
				return;
			}
			if (this._manualWaypoints == null)
			{
				this._manualPathPending = false;
				this._manualWaypoints = new List<Vector3> { this._destination };
			}
			while (this._manualWaypointIndex < this._manualWaypoints.Count && GhostPerkFleeController.PlanarDistance(base.transform.position, this._manualWaypoints[this._manualWaypointIndex]) <= 0.25f)
			{
				this._manualWaypointIndex++;
			}
			if (this._manualWaypointIndex >= this._manualWaypoints.Count)
			{
				this.Finish();
				return;
			}
			Vector3 position = base.transform.position;
			Vector3 vector = this._manualWaypoints[this._manualWaypointIndex] - position;
			vector.y = 0f;
			if (vector.sqrMagnitude < 0.001f)
			{
				return;
			}
			vector.Normalize();
			RaycastHit raycastHit;
			if (Physics.Raycast(position + Vector3.up * 1.2f, vector, out raycastHit, 0.75f, -1, QueryTriggerInteraction.Ignore) && !raycastHit.collider.transform.IsChildOf(base.transform) && !raycastHit.collider.CompareTag("Player") && !raycastHit.collider.CompareTag("NPC"))
			{
				this.Finish();
				return;
			}
			float simulationDeltaTime = NPCSimulationPause.SimulationDeltaTime;
			base.transform.position = position + vector * (5.5f * simulationDeltaTime);
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.LookRotation(vector, Vector3.up), 720f * simulationDeltaTime);
		}

		// Token: 0x06009489 RID: 38025 RVA: 0x003D142A File Offset: 0x003CF62A
		private void Finish()
		{
			if (this._cleaned)
			{
				return;
			}
			this.Cleanup(true);
			Object.Destroy(this);
		}

		// Token: 0x0600948A RID: 38026 RVA: 0x003D1442 File Offset: 0x003CF642
		private void OnDisable()
		{
			WorldOrigin.Shifted -= this.OnWorldOriginShifted;
			this._manualPathRequestVersion++;
			if (!this._cleaned)
			{
				this.Cleanup(false);
			}
		}

		// Token: 0x0600948B RID: 38027 RVA: 0x003D1474 File Offset: 0x003CF674
		private void Cleanup(bool restoreBehaviour)
		{
			this._cleaned = true;
			this._started = false;
			this._manualPathPending = false;
			if (this._controller != null)
			{
				this._controller.ClearSpeedOverride();
				this._controller.UnlockNavigation(true);
			}
			if (this._follower != null)
			{
				this._follower.enabled = restoreBehaviour || this._previousFollowerEnabled;
				if (this._previousFollowerMaxSpeed >= 0f)
				{
					this._follower.maxSpeed = this._previousFollowerMaxSpeed;
				}
				this._follower.isStopped = !restoreBehaviour && this._previousFollowerStopped;
			}
			if (this._wander != null)
			{
				this._wander.ExternalControl = this._previousWanderExternalControl;
				if (restoreBehaviour && !this._previousWanderExternalControl)
				{
					this._wander.ResumeWandering();
				}
			}
			if (this._simpleNavigator != null)
			{
				this._simpleNavigator.enabled = restoreBehaviour || this._previousSimpleNavigatorEnabled;
			}
			if (this._fsm != null && this._fsm.IsInitialized)
			{
				this._fsm.Context.ResetFacialExpression();
				if (restoreBehaviour)
				{
					this._fsm.ForceState(NPCStateType.Idle, "GhostPerkEnd");
				}
			}
			Animator componentInChildren = base.GetComponentInChildren<Animator>();
			if (componentInChildren != null && componentInChildren.isActiveAndEnabled)
			{
				componentInChildren.SetInteger("ReactionType", 0);
			}
			if (this._senses != null)
			{
				this._senses.IsGhostFleeing = false;
				this._senses.SuppressDetectionForGhost();
			}
		}

		// Token: 0x0600948C RID: 38028 RVA: 0x001AA7C8 File Offset: 0x001A89C8
		private static float PlanarDistance(Vector3 a, Vector3 b)
		{
			a.y = 0f;
			b.y = 0f;
			return Vector3.Distance(a, b);
		}

		// Token: 0x0600948D RID: 38029 RVA: 0x003D15F8 File Offset: 0x003CF7F8
		private bool IsRagdollRecovering()
		{
			if (this._ragdollRecovery == null)
			{
				this._ragdollRecovery = base.GetComponent<NPCRagdollRecovery>();
			}
			return this._ragdollRecovery != null && this._ragdollRecovery.IsRecovering;
		}

		// Token: 0x0400997D RID: 39293
		private const float FleeSpeed = 5.5f;

		// Token: 0x0400997E RID: 39294
		private const float FleeDistance = 30f;

		// Token: 0x0400997F RID: 39295
		private const float SafeDistance = 25f;

		// Token: 0x04009980 RID: 39296
		private const float MaximumFleeTime = 14f;

		// Token: 0x04009981 RID: 39297
		private const float RepathInterval = 1.25f;

		// Token: 0x04009982 RID: 39298
		private const float StuckCheckInterval = 1.4f;

		// Token: 0x04009983 RID: 39299
		private const float MinimumMovement = 0.45f;

		// Token: 0x04009984 RID: 39300
		private NPCSenses _senses;

		// Token: 0x04009985 RID: 39301
		private NPCWanderController _wander;

		// Token: 0x04009986 RID: 39302
		private SimplePOINavigator _simpleNavigator;

		// Token: 0x04009987 RID: 39303
		private AStarNPCController _controller;

		// Token: 0x04009988 RID: 39304
		private FollowerEntity _follower;

		// Token: 0x04009989 RID: 39305
		private NPCStateMachine _fsm;

		// Token: 0x0400998A RID: 39306
		private NPCSimpleLookAt _lookAt;

		// Token: 0x0400998B RID: 39307
		private NPCRagdollRecovery _ragdollRecovery;

		// Token: 0x0400998C RID: 39308
		private bool _started;

		// Token: 0x0400998D RID: 39309
		private bool _cleaned;

		// Token: 0x0400998E RID: 39310
		private bool _manualMovement;

		// Token: 0x0400998F RID: 39311
		private bool _manualPathPending;

		// Token: 0x04009990 RID: 39312
		private bool _previousWanderExternalControl;

		// Token: 0x04009991 RID: 39313
		private bool _previousSimpleNavigatorEnabled;

		// Token: 0x04009992 RID: 39314
		private bool _previousFollowerEnabled;

		// Token: 0x04009993 RID: 39315
		private bool _previousFollowerStopped;

		// Token: 0x04009994 RID: 39316
		private float _previousFollowerMaxSpeed;

		// Token: 0x04009995 RID: 39317
		private Vector3 _threatPosition;

		// Token: 0x04009996 RID: 39318
		private Vector3 _lastPlannedThreat;

		// Token: 0x04009997 RID: 39319
		private Vector3 _destination;

		// Token: 0x04009998 RID: 39320
		private Vector3 _movementSample;

		// Token: 0x04009999 RID: 39321
		private float _startedAt;

		// Token: 0x0400999A RID: 39322
		private float _nextPlanTime;

		// Token: 0x0400999B RID: 39323
		private float _nextStuckCheck;

		// Token: 0x0400999C RID: 39324
		private float _manualPathDeadline;

		// Token: 0x0400999D RID: 39325
		private List<Vector3> _manualWaypoints;

		// Token: 0x0400999E RID: 39326
		private int _manualWaypointIndex;

		// Token: 0x0400999F RID: 39327
		private int _manualPathRequestVersion;
	}
}
