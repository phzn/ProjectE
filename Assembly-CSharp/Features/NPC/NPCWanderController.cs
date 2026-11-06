using System;
using System.Collections.Generic;
using Features.NPC.StateMachine;
using Features.RenderingPrecision;
using Pathfinding;
using ProjectExhibitionist.BenchSeating;
using UnityEngine;

namespace Features.NPC
{
	// Token: 0x02000EFE RID: 3838
	public class NPCWanderController : MonoBehaviour
	{
		// Token: 0x060072DB RID: 29403 RVA: 0x002ECE3A File Offset: 0x002EB03A
		internal Object ReadPoiGameplayReference(UniversityGameplayField field, int elementIndex)
		{
			if (field == UniversityGameplayField.NPCWanderController_actionRegistry)
			{
				return this.actionRegistry;
			}
			throw new InvalidOperationException("Unsupported NPCWanderController POI binding: " + field.ToString());
		}

		// Token: 0x060072DC RID: 29404 RVA: 0x002ECE64 File Offset: 0x002EB064
		internal void WritePoiGameplayReference(UniversityGameplayField field, Object asset, int elementIndex)
		{
			if (field == UniversityGameplayField.NPCWanderController_actionRegistry)
			{
				this.actionRegistry = (NPCActionRegistry)asset;
				return;
			}
			throw new InvalidOperationException("Unsupported NPCWanderController POI binding: " + field.ToString());
		}

		// Token: 0x170012B2 RID: 4786
		// (get) Token: 0x060072DD RID: 29405 RVA: 0x002ECE94 File Offset: 0x002EB094
		public static IReadOnlyList<NPCWanderController> ActiveInstances
		{
			get
			{
				return NPCWanderController.ActiveWanderers;
			}
		}

		// Token: 0x060072DE RID: 29406 RVA: 0x002ECE9B File Offset: 0x002EB09B
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetActiveInstances()
		{
			NPCWanderController.ActiveWanderers.Clear();
		}

		// Token: 0x170012B3 RID: 4787
		// (get) Token: 0x060072DF RID: 29407 RVA: 0x002ECEA7 File Offset: 0x002EB0A7
		public NPCAmbientLife AmbientLife
		{
			get
			{
				return this._ambientLife;
			}
		}

		// Token: 0x170012B4 RID: 4788
		// (get) Token: 0x060072E0 RID: 29408 RVA: 0x002ECEAF File Offset: 0x002EB0AF
		public NPCInterestPoint CurrentInterestPoint
		{
			get
			{
				return this._poi;
			}
		}

		// Token: 0x170012B5 RID: 4789
		// (get) Token: 0x060072E1 RID: 29409 RVA: 0x002ECEB7 File Offset: 0x002EB0B7
		internal bool BenchOwnsBodyPose
		{
			get
			{
				return this._benchSeating != null && this._benchSeating.OwnsBodyPose;
			}
		}

		// Token: 0x060072E2 RID: 29410 RVA: 0x002ECECE File Offset: 0x002EB0CE
		internal void PrepareForExternalAnimationSnapshot()
		{
			NPCBenchSeating benchSeating = this._benchSeating;
			if (benchSeating == null)
			{
				return;
			}
			benchSeating.End(false);
		}

		// Token: 0x170012B6 RID: 4790
		// (get) Token: 0x060072E3 RID: 29411 RVA: 0x002ECEE4 File Offset: 0x002EB0E4
		internal bool CanRunAmbientLife
		{
			get
			{
				return this._isInitialized && this._recoveryOwners.Count == 0 && !this.ExternalControl && this._controller != null && this._controller.enabled && !this._controller.SuppressPOIBehavior && !this._controller.IsNavigationLocked;
			}
		}

		// Token: 0x060072E4 RID: 29412 RVA: 0x002ECF48 File Offset: 0x002EB148
		internal void SetAmbientFacing(Quaternion rotation)
		{
			if (!this.CanRunAmbientLife || this.CurrentState != NPCWanderController.WanderState.Performing)
			{
				return;
			}
			base.transform.rotation = rotation;
			if (this._follower != null && this._follower.entityExists)
			{
				this._follower.rotation = rotation;
			}
		}

		// Token: 0x170012B7 RID: 4791
		// (get) Token: 0x060072E5 RID: 29413 RVA: 0x002ECF9A File Offset: 0x002EB19A
		// (set) Token: 0x060072E6 RID: 29414 RVA: 0x002ECFA2 File Offset: 0x002EB1A2
		public NPCWanderController.WanderState CurrentState { get; private set; }

		// Token: 0x060072E7 RID: 29415 RVA: 0x002ECFAB File Offset: 0x002EB1AB
		private void Awake()
		{
			this.CacheComponents();
		}

		// Token: 0x060072E8 RID: 29416 RVA: 0x002ECFB4 File Offset: 0x002EB1B4
		private void CacheComponents()
		{
			this._controller = base.GetComponent<AStarNPCController>();
			this._follower = base.GetComponent<FollowerEntity>();
			if (this._follower != null)
			{
				this._authoredFollowerSpeed = this._follower.maxSpeed;
			}
			this._fsm = base.GetComponent<NPCStateMachine>();
			this._instanceId = base.gameObject.GetInstanceID();
			this._isInitialized = this._controller != null && this._follower != null;
		}

		// Token: 0x060072E9 RID: 29417 RVA: 0x002ED038 File Offset: 0x002EB238
		private void OnEnable()
		{
			if (!NPCWanderController.ActiveWanderers.Contains(this))
			{
				int num = 0;
				int instanceID = base.GetInstanceID();
				while (num < NPCWanderController.ActiveWanderers.Count && NPCWanderController.ActiveWanderers[num].GetInstanceID() < instanceID)
				{
					num++;
				}
				NPCWanderController.ActiveWanderers.Insert(num, this);
			}
			WorldOrigin.Shifted += this.OnWorldOriginShifted;
			this._simulationPauseActive = false;
			NPCSimulationPause.PauseChanged += this.HandleSimulationPauseChanged;
			this.HandleSimulationPauseChanged(NPCSimulationPause.IsPaused);
			if (!this._isInitialized)
			{
				this.CacheComponents();
			}
			if (this._controller != null && !this._controller.enabled)
			{
				this._controller.enabled = true;
			}
		}

		// Token: 0x060072EA RID: 29418 RVA: 0x002ED0F8 File Offset: 0x002EB2F8
		private void OnDisable()
		{
			NPCWanderController.ActiveWanderers.Remove(this);
			NPCAmbientLife ambientLife = this._ambientLife;
			if (ambientLife != null)
			{
				ambientLife.End();
			}
			WorldOrigin.Shifted -= this.OnWorldOriginShifted;
			NPCSimulationPause.PauseChanged -= this.HandleSimulationPauseChanged;
			if (this.CurrentState != NPCWanderController.WanderState.Disabled)
			{
				this.ForceStop();
			}
		}

		// Token: 0x060072EB RID: 29419 RVA: 0x002ED152 File Offset: 0x002EB352
		private void OnWorldOriginShifted(Vector3 delta)
		{
			this._currentNavigationTarget += delta;
			this._walkTarget += delta;
			this._navStuckLastPos += delta;
		}

		// Token: 0x060072EC RID: 29420 RVA: 0x002ED18C File Offset: 0x002EB38C
		private void Update()
		{
			if (this._ambientLife == null && this._isInitialized)
			{
				this._ambientLife = new NPCAmbientLife(this, this._controller, this._fsm);
			}
			if (this._benchSeating == null && this._isInitialized)
			{
				this._benchSeating = new NPCBenchSeating(this, this._controller);
			}
			if (NPCSimulationPause.IsPaused)
			{
				this.BeginSimulationPauseIfNeeded();
				return;
			}
			this.ResumeSimulationIfNeeded();
			float frameTimeLoss = NPCSimulationPause.FrameTimeLoss;
			if (frameTimeLoss > 0f)
			{
				this._timer += frameTimeLoss;
				this._navStuckLastSampleTime += frameTimeLoss;
				this._navStuckStartTime += frameTimeLoss;
				this._skipPoiUntilTime += frameTimeLoss;
				this._nextBenchSearch += frameTimeLoss;
			}
			if (((this._benchSeating != null && this._benchSeating.Active) || (this._poi != null && this._poi.BenchSeat != null)) && (this._poi == null || !this._poi.isActiveAndEnabled || this._poi.BenchSeat == null || !this._poi.BenchSeat.IsReservedBy(this._instanceId) || !this._controller.enabled || this._controller.IsNavigationLocked))
			{
				this.ReleasePOI(true);
				this._controller.SetPosture(NPCInterestPoint.PostureType.Standing);
				this._controller.ExitManualMode(false);
				this.CurrentState = NPCWanderController.WanderState.Disabled;
			}
			if (!this._isInitialized || AstarPath.active == null || this._recoveryOwners.Count > 0 || !this._controller.enabled || this._controller.IsNavigationLocked)
			{
				NPCAmbientLife ambientLife = this._ambientLife;
				if (ambientLife == null)
				{
					return;
				}
				ambientLife.End();
				return;
			}
			else
			{
				if (this.ExternalControl)
				{
					NPCAmbientLife ambientLife2 = this._ambientLife;
					if (ambientLife2 != null)
					{
						ambientLife2.End();
					}
					if (!this._externalControlReleased)
					{
						this.ReleasePOI(false);
						this.CurrentState = NPCWanderController.WanderState.Disabled;
						this._externalControlReleased = true;
					}
					return;
				}
				this._externalControlReleased = false;
				if (!this.ShouldBeActive())
				{
					if (this.CurrentState != NPCWanderController.WanderState.Disabled)
					{
						this.PauseWandering();
					}
					return;
				}
				if (this.CurrentState == NPCWanderController.WanderState.Disabled)
				{
					this.ResumeWandering();
				}
				this.ProcessState();
				NPCAmbientLife ambientLife3 = this._ambientLife;
				if (ambientLife3 == null)
				{
					return;
				}
				ambientLife3.Tick(NPCSimulationPause.SimulationDeltaTime);
				return;
			}
		}

		// Token: 0x060072ED RID: 29421 RVA: 0x002ED3D9 File Offset: 0x002EB5D9
		private void LateUpdate()
		{
			NPCBenchSeating benchSeating = this._benchSeating;
			if (benchSeating == null)
			{
				return;
			}
			benchSeating.Tick(NPCSimulationPause.SimulationDeltaTime);
		}

		// Token: 0x060072EE RID: 29422 RVA: 0x002ED3F0 File Offset: 0x002EB5F0
		private void OnDestroy()
		{
			NPCAmbientLife ambientLife = this._ambientLife;
			if (ambientLife == null)
			{
				return;
			}
			ambientLife.Dispose();
		}

		// Token: 0x060072EF RID: 29423 RVA: 0x002ED402 File Offset: 0x002EB602
		private void HandleSimulationPauseChanged(bool paused)
		{
			if (paused)
			{
				this.BeginSimulationPauseIfNeeded();
				return;
			}
			this.ResumeSimulationIfNeeded();
		}

		// Token: 0x060072F0 RID: 29424 RVA: 0x002ED414 File Offset: 0x002EB614
		private void BeginSimulationPauseIfNeeded()
		{
			if (this._simulationPauseActive)
			{
				return;
			}
			this._simulationPauseActive = true;
			this._simulationPauseStartedAt = Time.time;
		}

		// Token: 0x060072F1 RID: 29425 RVA: 0x002ED434 File Offset: 0x002EB634
		private void ResumeSimulationIfNeeded()
		{
			if (!this._simulationPauseActive)
			{
				return;
			}
			float num = Mathf.Max(0f, Time.time - this._simulationPauseStartedAt);
			this._timer += num;
			this._navStuckLastSampleTime += num;
			this._navStuckStartTime += num;
			this._skipPoiUntilTime += num;
			this._nextBenchSearch += num;
			this._simulationPauseActive = false;
		}

		// Token: 0x060072F2 RID: 29426 RVA: 0x002ED4B0 File Offset: 0x002EB6B0
		public void ForceStop()
		{
			this.Log(string.Format("ForceStop called (was in {0})", this.CurrentState));
			this.ReleasePOI(true);
			if (this._controller != null)
			{
				if (this._controller.IsManualMode)
				{
					this._controller.ExitManualMode(true);
				}
				if (!this._controller.SuppressPOIBehavior)
				{
					this._controller.ResetToDefault();
				}
			}
			this.CurrentState = NPCWanderController.WanderState.Disabled;
		}

		// Token: 0x060072F3 RID: 29427 RVA: 0x002ED525 File Offset: 0x002EB725
		public void ResumeWandering()
		{
			if (this._recoveryOwners.Count > 0)
			{
				return;
			}
			if (this.CurrentState != NPCWanderController.WanderState.Disabled)
			{
				return;
			}
			this.Log("Resuming wandering");
			this.CurrentState = NPCWanderController.WanderState.Idle;
		}

		// Token: 0x060072F4 RID: 29428 RVA: 0x002ED551 File Offset: 0x002EB751
		internal void SuspendForRecovery(Object owner)
		{
			if (owner == null || !this._recoveryOwners.Add(owner))
			{
				return;
			}
			this.ReleasePOI(false);
			this.CurrentState = NPCWanderController.WanderState.Disabled;
		}

		// Token: 0x060072F5 RID: 29429 RVA: 0x002ED57C File Offset: 0x002EB77C
		internal void ResumeAfterRecovery(Object owner, bool restartWander = true)
		{
			if (!this._recoveryOwners.Remove(owner) || this._recoveryOwners.Count > 0)
			{
				return;
			}
			this.CurrentState = NPCWanderController.WanderState.Disabled;
			this._externalControlReleased = false;
			if (!restartWander || this.ExternalControl || this._controller == null || !this._controller.enabled || this._controller.IsNavigationLocked || !this.ShouldBeActive())
			{
				return;
			}
			this._controller.ResetToDefault();
			this.ResumeWandering();
		}

		// Token: 0x170012B8 RID: 4792
		// (get) Token: 0x060072F6 RID: 29430 RVA: 0x002ED601 File Offset: 0x002EB801
		public bool IsActive
		{
			get
			{
				return this.CurrentState > NPCWanderController.WanderState.Disabled;
			}
		}

		// Token: 0x060072F7 RID: 29431 RVA: 0x002ED60C File Offset: 0x002EB80C
		private bool ShouldBeActive()
		{
			if (this._fsm == null || !this._fsm.IsInitialized)
			{
				return true;
			}
			NPCStateType currentStateType = this._fsm.CurrentStateType;
			return currentStateType.IsNormalState() || currentStateType == NPCStateType.LookingAt;
		}

		// Token: 0x060072F8 RID: 29432 RVA: 0x002ED654 File Offset: 0x002EB854
		private void PauseWandering()
		{
			if (this._poi != null)
			{
				string text = "Pausing - releasing POI ({0})";
				NPCStateMachine fsm = this._fsm;
				this.Log(string.Format(text, (fsm != null) ? new NPCStateType?(fsm.CurrentStateType) : null));
				this.ReleasePOI(true);
			}
			if (this._controller != null)
			{
				if (this._controller.IsManualMode)
				{
					this._controller.ExitManualMode(false);
				}
				if (!this._controller.SuppressPOIBehavior)
				{
					this._controller.ResetToDefault();
				}
			}
			this.CurrentState = NPCWanderController.WanderState.Disabled;
		}

		// Token: 0x060072F9 RID: 29433 RVA: 0x002ED6F0 File Offset: 0x002EB8F0
		private void ProcessState()
		{
			switch (this.CurrentState)
			{
			case NPCWanderController.WanderState.Idle:
				this.UpdateIdle();
				return;
			case NPCWanderController.WanderState.Navigating:
				this.UpdateNavigating();
				return;
			case NPCWanderController.WanderState.WaitingManualMode:
				this.UpdateWaitingManualMode();
				return;
			case NPCWanderController.WanderState.WalkingIn:
				this.UpdateWalkingIn();
				return;
			case NPCWanderController.WanderState.Aligning:
				this.UpdateAligning();
				return;
			case NPCWanderController.WanderState.Performing:
				this.UpdatePerforming();
				return;
			case NPCWanderController.WanderState.StandingUp:
				this.UpdateStandingUp();
				return;
			case NPCWanderController.WanderState.WalkingOut:
				this.UpdateWalkingOut();
				return;
			case NPCWanderController.WanderState.WaitingNavMode:
				this.UpdateWaitingNavMode();
				return;
			case NPCWanderController.WanderState.ReleasingBench:
				this.UpdateReleasingBench();
				return;
			default:
				return;
			}
		}

		// Token: 0x060072FA RID: 29434 RVA: 0x002ED77C File Offset: 0x002EB97C
		private void UpdateIdle()
		{
			if (Time.time >= this._skipPoiUntilTime && this.TryFindPOI(out this._poi))
			{
				this.SetRandomLocomotionAction();
				NPCStateMachine fsm = this._fsm;
				if (fsm != null)
				{
					fsm.RequestState(NPCStateType.Wandering, "WanderController");
				}
				this._controller.SetDestination(this._currentNavigationTarget);
				this.CurrentState = NPCWanderController.WanderState.Navigating;
				this.ResetStuckWatchdog();
				this.Log("Navigating to POI: " + this._poi.name);
				return;
			}
			Vector3 vector;
			if (this.TryGetRandomPoint(out vector))
			{
				this.SetRandomLocomotionAction();
				NPCStateMachine fsm2 = this._fsm;
				if (fsm2 != null)
				{
					fsm2.RequestState(NPCStateType.Wandering, "WanderController");
				}
				this._poi = null;
				this._currentNavigationTarget = vector;
				this._controller.SetDestination(vector);
				this._timer = Time.time + this.fallbackWaitTime;
				this.CurrentState = NPCWanderController.WanderState.Navigating;
				this.ResetStuckWatchdog();
				this.Log("Navigating to fallback point");
				return;
			}
			if (this._controller != null)
			{
				this._controller.SetAction(NPCInterestPoint.ActionType.None);
			}
			this._timer = Time.time + 1f;
			this.CurrentState = NPCWanderController.WanderState.Performing;
		}

		// Token: 0x060072FB RID: 29435 RVA: 0x002ED8A0 File Offset: 0x002EBAA0
		private void SetRandomLocomotionAction()
		{
			if (this._controller == null || this._controller.SuppressPOIBehavior)
			{
				return;
			}
			if (this._ambientLife != null && this._ambientLife.BeginTravel())
			{
				this._controller.SetAction(NPCInterestPoint.ActionType.None);
				return;
			}
			if (!(this.actionRegistry != null))
			{
				this._controller.SetAction(NPCInterestPoint.ActionType.None);
				return;
			}
			NPCActionDefinition randomLocomotionAction = this.actionRegistry.GetRandomLocomotionAction();
			if (randomLocomotionAction != null)
			{
				this._controller.SetAction((NPCInterestPoint.ActionType)randomLocomotionAction.animatorIndex);
				this.Log(string.Format("Locomotion action: {0} (index {1})", randomLocomotionAction.displayName, randomLocomotionAction.animatorIndex));
				return;
			}
			this._controller.SetAction(NPCInterestPoint.ActionType.None);
		}

		// Token: 0x060072FC RID: 29436 RVA: 0x002ED95C File Offset: 0x002EBB5C
		private void UpdateNavigating()
		{
			if (this._poi != null && !this._poi.gameObject.activeInHierarchy)
			{
				this.Log("Current POI '" + this._poi.name + "' became inactive — aborting nav");
				this._lastPoi = this._poi;
				this._poi.Release(this._instanceId);
				this._poi = null;
				this._controller.Stop();
				this._controller.SetAction(NPCInterestPoint.ActionType.None);
				this.CurrentState = NPCWanderController.WanderState.Idle;
				return;
			}
			if (this.CheckStuckAndMaybeAbort())
			{
				return;
			}
			if (!this._controller.HasReachedDestination())
			{
				return;
			}
			this._controller.Stop();
			this._controller.SetAction(NPCInterestPoint.ActionType.None);
			if (this._poi == null)
			{
				this.CurrentState = NPCWanderController.WanderState.Performing;
				return;
			}
			this._poi.Occupy(this._instanceId);
			this._targetRotation = this._poi.TargetRotation;
			if (this._poi.HasSeparateApproachPoint)
			{
				this.StartWalkingIn();
				return;
			}
			this.CurrentState = NPCWanderController.WanderState.Aligning;
			this._timer = Time.time;
		}

		// Token: 0x060072FD RID: 29437 RVA: 0x002EDA78 File Offset: 0x002EBC78
		private void UpdateWaitingManualMode()
		{
			if (!this._controller.IsManualModeReady)
			{
				return;
			}
			this._controller.SetSpeedOverride(this.GetCustomSpeed());
			this.CurrentState = NPCWanderController.WanderState.WalkingIn;
			this.Log(string.Format("Manual mode ready, walking in to {0}", this._walkTarget));
		}

		// Token: 0x060072FE RID: 29438 RVA: 0x002EDAC8 File Offset: 0x002EBCC8
		private void UpdateWalkingIn()
		{
			if ((this._benchSeating != null && this._benchSeating.Active) ? this.MoveToBenchEntry(this._walkTarget) : this.MoveTowards(this._walkTarget))
			{
				this._controller.ClearSpeedOverride();
				this.CurrentState = NPCWanderController.WanderState.Aligning;
				this._timer = Time.time;
				this.Log("Walk-in complete");
			}
		}

		// Token: 0x060072FF RID: 29439 RVA: 0x002EDB30 File Offset: 0x002EBD30
		private void UpdateAligning()
		{
			if (this._poi == null || !this._poi.RequiresAlignment)
			{
				this.StartPerforming();
				return;
			}
			float num = Quaternion.Angle(base.transform.rotation, this._targetRotation);
			float num2 = Time.time - this._timer;
			if (this._benchSeating != null && this._benchSeating.Active)
			{
				if (num <= 1f)
				{
					this.StartPerforming();
					return;
				}
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, this._targetRotation, this.rotationSpeed * NPCSimulationPause.SimulationDeltaTime);
				return;
			}
			else
			{
				if (num < 2f || num2 > 2f)
				{
					base.transform.rotation = this._targetRotation;
					this.StartPerforming();
					return;
				}
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, this._targetRotation, this.rotationSpeed * NPCSimulationPause.SimulationDeltaTime);
				return;
			}
		}

		// Token: 0x06007300 RID: 29440 RVA: 0x002EDC2C File Offset: 0x002EBE2C
		private void UpdatePerforming()
		{
			if (Time.time < this._timer || (this._ambientLife != null && this._ambientLife.KeepConversation))
			{
				return;
			}
			NPCAmbientLife ambientLife = this._ambientLife;
			if (ambientLife != null)
			{
				ambientLife.End();
			}
			if (this._poi != null && this._poi.posture == NPCInterestPoint.PostureType.Sitting)
			{
				this.StartStandingUp();
				return;
			}
			if (this._poi != null && this._poi.HasSeparateApproachPoint)
			{
				this.StartWalkingOut();
				return;
			}
			this._controller.DisableRootMotion();
			this._controller.ClearSpeedOverride();
			this._controller.ExitManualMode(false);
			this.StartWaitingNavMode();
		}

		// Token: 0x06007301 RID: 29441 RVA: 0x002EDCDC File Offset: 0x002EBEDC
		private void UpdateStandingUp()
		{
			if (Time.time < this._timer)
			{
				return;
			}
			if (this._benchSeating != null && this._benchSeating.Active)
			{
				if (!this._benchSeating.HasFinishedStanding)
				{
					return;
				}
				if (this._poi == null || this._poi.BenchSeat == null || !this._poi.BenchSeat.HasStandingClearance(base.transform.position, base.transform, false))
				{
					this._timer = Time.time + 0.5f;
					return;
				}
				this._benchSeating.BeginRelease();
				this.CurrentState = NPCWanderController.WanderState.ReleasingBench;
				return;
			}
			else
			{
				NPCBenchSeating benchSeating = this._benchSeating;
				if (benchSeating != null)
				{
					benchSeating.CompleteStanding(false);
				}
				this._controller.DisableRootMotion();
				if (this._poi != null && this._poi.HasSeparateApproachPoint)
				{
					this._controller.SetSpeedOverride(this.GetCustomSpeed());
					this._walkTarget = this._poi.NavigationPosition;
					this.CurrentState = NPCWanderController.WanderState.WalkingOut;
					this.Log(string.Format("Stand-up complete, walking out to {0}", this._walkTarget));
					return;
				}
				this._controller.ClearSpeedOverride();
				this._controller.ExitManualMode(false);
				this.StartWaitingNavMode();
				return;
			}
		}

		// Token: 0x06007302 RID: 29442 RVA: 0x002EDE22 File Offset: 0x002EC022
		private void UpdateWalkingOut()
		{
			if (this.MoveTowards(this._walkTarget))
			{
				this._controller.ClearSpeedOverride();
				this._controller.ExitManualMode(false);
				this.StartWaitingNavMode();
				this.Log("Walk-out complete");
			}
		}

		// Token: 0x06007303 RID: 29443 RVA: 0x002EDE5A File Offset: 0x002EC05A
		private void UpdateWaitingNavMode()
		{
			if (!this._controller.IsNavigationReady)
			{
				return;
			}
			this.FinishInteraction();
			this.Log("Navigation mode ready");
		}

		// Token: 0x06007304 RID: 29444 RVA: 0x002EDE7C File Offset: 0x002EC07C
		private void UpdateReleasingBench()
		{
			if (this._benchSeating == null || !this._benchSeating.HasFinishedRelease)
			{
				return;
			}
			this._benchSeating.CompleteStanding(false);
			this._controller.DisableRootMotion();
			this._controller.ClearSpeedOverride();
			this._controller.ExitManualMode(false);
			this.StartWaitingNavMode();
		}

		// Token: 0x06007305 RID: 29445 RVA: 0x002EDED4 File Offset: 0x002EC0D4
		private void StartWalkingIn()
		{
			if (this._poi.BenchSeat != null && (this._benchSeating == null || (!this._benchSeating.Active && !this._benchSeating.Begin(this._poi.BenchSeat))))
			{
				this.ForceStop();
				return;
			}
			this._controller.EnterManualMode();
			bool flag = this._benchSeating != null && this._benchSeating.Active;
			this._walkTarget = (flag ? this._benchSeating.EntryPosition : this._poi.TargetPosition);
			if (flag)
			{
				this._targetRotation = this._benchSeating.EntryRotation;
			}
			this.CurrentState = NPCWanderController.WanderState.WaitingManualMode;
			this.Log("Waiting for manual mode...");
		}

		// Token: 0x06007306 RID: 29446 RVA: 0x002EDF94 File Offset: 0x002EC194
		private void StartPerforming()
		{
			if (this._poi != null)
			{
				if (this._poi.BenchSeat != null)
				{
					NPCAmbientLife ambientLife = this._ambientLife;
					if (ambientLife != null)
					{
						ambientLife.End();
					}
					float num;
					if (this._benchSeating == null || !this._benchSeating.BeginSitting(out num))
					{
						this.ForceStop();
						return;
					}
					float num2 = Mathf.Max(45f, this._poi.waitTimeRange.x);
					float num3 = Mathf.Max(num2, Mathf.Max(90f, this._poi.waitTimeRange.y));
					this._timer = Time.time + num + Random.Range(num2, num3);
					this.CurrentState = NPCWanderController.WanderState.Performing;
					return;
				}
				else
				{
					this._controller.EnableRootMotion();
					bool flag = this._ambientLife != null && this._ambientLife.ChoosePointActivity(this._poi);
					this._controller.ConfigureForPOI(this._poi, flag);
					float num4 = this._poi.GetRandomWaitTime();
					if (this._ambientLife != null)
					{
						num4 = this._ambientLife.BeginPoint(this._poi, num4);
					}
					this._timer = Time.time + num4;
					this.Log(string.Format("Performing: {0}, wait {1:F1}s", this._poi.posture, this._timer - Time.time));
				}
			}
			else
			{
				this._timer = Time.time + this.fallbackWaitTime;
			}
			this.CurrentState = NPCWanderController.WanderState.Performing;
		}

		// Token: 0x06007307 RID: 29447 RVA: 0x002EE10C File Offset: 0x002EC30C
		private void StartStandingUp()
		{
			if (this._benchSeating == null || !this._benchSeating.Active)
			{
				this._controller.SetPosture(NPCInterestPoint.PostureType.Standing);
				this._timer = Time.time + 1.5f;
				this.CurrentState = NPCWanderController.WanderState.StandingUp;
				this.Log("Standing up");
				return;
			}
			Vector3 vector;
			Quaternion quaternion;
			if (this._poi == null || this._poi.BenchSeat == null || !this._poi.BenchSeat.HasStandingClearance(this._benchSeating.ExitPosition, base.transform, false) || !this._poi.BenchSeat.TryGetExitPose(base.transform, out vector, out quaternion, false))
			{
				this._timer = Time.time + 1f;
				return;
			}
			float num;
			if (!this._benchSeating.BeginStanding(out num))
			{
				this.ForceStop();
				return;
			}
			this._timer = Time.time + num;
			this.CurrentState = NPCWanderController.WanderState.StandingUp;
		}

		// Token: 0x06007308 RID: 29448 RVA: 0x002EE204 File Offset: 0x002EC404
		private void StartWalkingOut()
		{
			this._controller.DisableRootMotion();
			this._controller.SetPosture(NPCInterestPoint.PostureType.Standing);
			this._controller.SetSpeedOverride(this.GetCustomSpeed());
			this._walkTarget = this._poi.NavigationPosition;
			this.CurrentState = NPCWanderController.WanderState.WalkingOut;
			this.Log(string.Format("Walking out to {0}", this._walkTarget));
		}

		// Token: 0x06007309 RID: 29449 RVA: 0x002EE26C File Offset: 0x002EC46C
		private void StartWaitingNavMode()
		{
			this.CurrentState = NPCWanderController.WanderState.WaitingNavMode;
			this.Log("Waiting for navigation mode...");
		}

		// Token: 0x0600730A RID: 29450 RVA: 0x002EE284 File Offset: 0x002EC484
		private void FinishInteraction()
		{
			if (this._controller != null && !this._controller.SuppressPOIBehavior)
			{
				this._controller.ResetToDefault();
			}
			this.ReleasePOI(true);
			this.CurrentState = NPCWanderController.WanderState.Idle;
			this.Log("Continuing to next POI");
		}

		// Token: 0x0600730B RID: 29451 RVA: 0x002EE2D0 File Offset: 0x002EC4D0
		private bool MoveToBenchEntry(Vector3 target)
		{
			Vector3 vector = target - base.transform.position;
			if (vector.sqrMagnitude <= 2.5E-05f)
			{
				return true;
			}
			float simulationDeltaTime = NPCSimulationPause.SimulationDeltaTime;
			if (simulationDeltaTime <= 0f)
			{
				return false;
			}
			float num = Mathf.Min(this.GetCustomSpeed() * simulationDeltaTime, vector.magnitude);
			base.transform.position += vector.normalized * num;
			this._controller.SetSpeedOverride(num / simulationDeltaTime);
			Vector3 vector2 = Vector3.ProjectOnPlane(vector, Vector3.up);
			if (vector2.sqrMagnitude > 1E-05f)
			{
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.LookRotation(vector2), this.rotationSpeed * simulationDeltaTime);
			}
			return false;
		}

		// Token: 0x0600730C RID: 29452 RVA: 0x002EE39C File Offset: 0x002EC59C
		private bool MoveTowards(Vector3 target)
		{
			if (Vector3.Distance(base.transform.position, target) < 0.1f)
			{
				base.transform.position = target;
				return true;
			}
			float num = this.GetCustomSpeed() * NPCSimulationPause.SimulationDeltaTime;
			base.transform.position = Vector3.MoveTowards(base.transform.position, target, num);
			Vector3 vector = target - base.transform.position;
			vector.y = 0f;
			if (vector.sqrMagnitude > 0.001f)
			{
				Quaternion quaternion = Quaternion.LookRotation(vector);
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, quaternion, this.rotationSpeed * NPCSimulationPause.SimulationDeltaTime);
			}
			return false;
		}

		// Token: 0x0600730D RID: 29453 RVA: 0x002EE455 File Offset: 0x002EC655
		private float GetCustomSpeed()
		{
			if (this.useFollowerSpeed && this._follower != null)
			{
				return this._authoredFollowerSpeed;
			}
			return this.customWalkSpeed;
		}

		// Token: 0x0600730E RID: 29454 RVA: 0x002EE47C File Offset: 0x002EC67C
		private bool TryFindPOI(out NPCInterestPoint poi)
		{
			poi = null;
			if (base.transform.position.y < -500f)
			{
				return false;
			}
			if (this.TryFindBench(out poi))
			{
				return true;
			}
			if (NPCZoneManager.Instance == null)
			{
				return false;
			}
			for (int i = 0; i < this.maxSearchAttempts; i++)
			{
				NPCInterestPoint npcinterestPoint = ((this._ambientLife != null && this._ambientLife.Supported) ? NPCZoneManager.Instance.GetAmbientInterestPoint(base.transform.position, this.searchRadius, this._lastPoi, this._ambientLife.CanSeekCompany) : NPCZoneManager.Instance.GetRandomInterestPoint(base.transform.position, this.searchRadius));
				if (npcinterestPoint == this._lastPoi)
				{
					this.Log("Skipping last POI: " + ((npcinterestPoint != null) ? npcinterestPoint.name : null));
				}
				else if ((!(npcinterestPoint != null) || !(npcinterestPoint.BenchSeat != null)) && npcinterestPoint != null && npcinterestPoint.TryReserve(this._instanceId))
				{
					Vector3 vector = ((this._ambientLife != null && this._ambientLife.Supported) ? npcinterestPoint.GetAmbientNavigationPosition(this._instanceId) : npcinterestPoint.NavigationPosition);
					if (this.IsReachableNavigationTarget(vector))
					{
						this._currentNavigationTarget = vector;
						poi = npcinterestPoint;
						this.Log("Reserved POI: " + poi.name);
						return true;
					}
					npcinterestPoint.Release(this._instanceId);
				}
			}
			return false;
		}

		// Token: 0x0600730F RID: 29455 RVA: 0x002EE5F8 File Offset: 0x002EC7F8
		private bool CanUseDynamicBenches()
		{
			return base.isActiveAndEnabled && this._controller != null && this._controller.enabled && !this._controller.SuppressPOIBehavior && this._controller.animator != null && this._controller.animator.isHuman && base.GetComponent<NPCAnimationStance>() != null && !this.ExternalControl && base.GetComponent<StaticPosePreview>() == null;
		}

		// Token: 0x06007310 RID: 29456 RVA: 0x002EE680 File Offset: 0x002EC880
		private bool TryFindBench(out NPCInterestPoint point)
		{
			point = null;
			if (Time.time < this._nextBenchSearch || !this.CanUseDynamicBenches())
			{
				return false;
			}
			this._nextBenchSearch = Time.time + Random.Range(20f, 35f);
			if (Random.value > 0.35f)
			{
				return false;
			}
			float num = Mathf.Min(this.searchRadius, 28f);
			float num2 = num + 3f;
			num2 *= num2;
			num *= num;
			BenchSeat benchSeat = null;
			Vector3 position = base.transform.position;
			IReadOnlyList<BenchSeat> activeSeats = BenchSeat.ActiveSeats;
			for (int i = 0; i < activeSeats.Count; i++)
			{
				BenchSeat benchSeat2 = activeSeats[i];
				if (!(benchSeat2 == null) && !(benchSeat2.Animations == null) && (benchSeat2.SeatPosition - position).sqrMagnitude < num2)
				{
					Vector3 approachPosition = benchSeat2.ApproachPosition;
					float sqrMagnitude = (approachPosition - position).sqrMagnitude;
					if (sqrMagnitude < num && benchSeat2.IsAvailable)
					{
						NPCInterestPoint component = benchSeat2.GetComponent<NPCInterestPoint>();
						if (!(component == null) && component.isActiveAndEnabled && !(component == this._lastPoi) && this.IsReachableNavigationTarget(approachPosition))
						{
							num = sqrMagnitude;
							benchSeat = benchSeat2;
							point = component;
						}
					}
				}
			}
			if (benchSeat == null || point == null || !point.TryReserve(this._instanceId))
			{
				point = null;
				return false;
			}
			if (this._benchSeating == null)
			{
				this._benchSeating = new NPCBenchSeating(this, this._controller);
			}
			if (!this._benchSeating.Begin(benchSeat) || !this.IsReachableNavigationTarget(this._benchSeating.EntryPosition))
			{
				this._benchSeating.End(false);
				point.Release(this._instanceId);
				point = null;
				return false;
			}
			this._currentNavigationTarget = this._benchSeating.EntryPosition;
			return true;
		}

		// Token: 0x06007311 RID: 29457 RVA: 0x002EE860 File Offset: 0x002ECA60
		private bool TryGetRandomPoint(out Vector3 result)
		{
			result = Vector3.zero;
			if (AstarPath.active == null)
			{
				return false;
			}
			NNInfo nearest = AstarPath.active.GetNearest(base.transform.position, NearestNodeConstraint.Walkable);
			if (nearest.node == null)
			{
				return false;
			}
			for (int i = 0; i < 5; i++)
			{
				Vector3 vector = base.transform.position + Random.insideUnitSphere * this.fallbackWanderRadius;
				vector.y = base.transform.position.y;
				NNInfo nearest2 = AstarPath.active.GetNearest(vector, NearestNodeConstraint.Walkable);
				if (nearest2.node != null && nearest2.node.Area == nearest.node.Area)
				{
					result = nearest2.position;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007312 RID: 29458 RVA: 0x002EE930 File Offset: 0x002ECB30
		private bool IsReachableNavigationTarget(Vector3 target)
		{
			if (AstarPath.active == null)
			{
				return false;
			}
			NearestNodeConstraint walkable = NearestNodeConstraint.Walkable;
			NNInfo nearest = AstarPath.active.GetNearest(base.transform.position, walkable);
			NNInfo nearest2 = AstarPath.active.GetNearest(target, walkable);
			if (nearest.node == null || nearest2.node == null)
			{
				return false;
			}
			if (nearest.node.Area != nearest2.node.Area)
			{
				return false;
			}
			Vector3 position = nearest2.position;
			Vector2 vector = new Vector2(position.x - target.x, position.z - target.z);
			return vector.sqrMagnitude <= 4f && Mathf.Abs(position.y - target.y) <= 2f;
		}

		// Token: 0x06007313 RID: 29459 RVA: 0x002EE9F8 File Offset: 0x002ECBF8
		private void ReleasePOI(bool relocateFromBench = true)
		{
			NPCAmbientLife ambientLife = this._ambientLife;
			if (ambientLife != null)
			{
				ambientLife.End();
			}
			NPCBenchSeating benchSeating = this._benchSeating;
			if (benchSeating != null)
			{
				benchSeating.End(relocateFromBench);
			}
			if (this._poi != null)
			{
				this._lastPoi = this._poi;
				this._poi.Release(this._instanceId);
				this.Log("Released POI: " + this._lastPoi.name);
				this._poi = null;
			}
		}

		// Token: 0x06007314 RID: 29460 RVA: 0x002EEA75 File Offset: 0x002ECC75
		private void ResetStuckWatchdog()
		{
			this._navStuckLastPos = base.transform.position;
			this._navStuckLastSampleTime = Time.time;
			this._navStuckStartTime = Time.time;
		}

		// Token: 0x06007315 RID: 29461 RVA: 0x002EEAA0 File Offset: 0x002ECCA0
		private bool CheckStuckAndMaybeAbort()
		{
			float time = Time.time;
			if (time - this._navStuckLastSampleTime < this.navStuckSampleInterval)
			{
				return false;
			}
			float num = Vector3.Distance(base.transform.position, this._navStuckLastPos);
			this._navStuckLastPos = base.transform.position;
			this._navStuckLastSampleTime = time;
			if (num >= this.navStuckMinMovement)
			{
				this._navStuckStartTime = time;
				return false;
			}
			float num2 = time - this._navStuckStartTime;
			if (num2 < this.navStuckTimeout)
			{
				return false;
			}
			if (this.logStuckDiagnostics && NPCDebug.Verbose)
			{
				string text = ((this._poi != null) ? this._poi.GetDiagnosticSnapshot() : "(no POI)");
				Vector3 vector = ((this._follower != null) ? this._follower.destination : Vector3.zero);
				float num3 = Vector3.Distance(base.transform.position, this._currentNavigationTarget);
				Debug.LogWarning(string.Format("[WanderController] STUCK '{0}' for {1:F1}s in {2} | ", base.gameObject.name, num2, this.CurrentState) + string.Format("distToDest={0:F1}m | poi={1} | dest={2}", num3, text, vector));
			}
			if (this._poi != null)
			{
				this._poi.Release(this._instanceId);
			}
			this._lastPoi = this._poi;
			this._poi = null;
			this._controller.Stop();
			this._controller.SetAction(NPCInterestPoint.ActionType.None);
			this.RecoverOntoCurrentGraph();
			this._skipPoiUntilTime = Time.time + this.postStuckSkipPoiDuration;
			this._timer = Time.time + 1.5f;
			this.CurrentState = NPCWanderController.WanderState.Performing;
			return true;
		}

		// Token: 0x06007316 RID: 29462 RVA: 0x002EEC4C File Offset: 0x002ECE4C
		private void RecoverOntoCurrentGraph()
		{
			if (this._follower == null || !this._follower.entityExists || AstarPath.active == null)
			{
				return;
			}
			NNInfo nearest = AstarPath.active.GetNearest(base.transform.position, NearestNodeConstraint.Walkable);
			if (nearest.node == null)
			{
				return;
			}
			Vector3 vector = nearest.position - base.transform.position;
			Vector2 vector2 = new Vector2(vector.x, vector.z);
			if (vector2.sqrMagnitude > 4f || Mathf.Abs(vector.y) > 2f)
			{
				return;
			}
			this._follower.SetPath(null, true);
		}

		// Token: 0x06007317 RID: 29463 RVA: 0x002EECFE File Offset: 0x002ECEFE
		private void Log(string msg)
		{
			bool flag = this.debugLog;
		}

		// Token: 0x0400761D RID: 30237
		private const float MinimumBenchRestSeconds = 45f;

		// Token: 0x0400761E RID: 30238
		private const float MaximumBenchRestSeconds = 90f;

		// Token: 0x0400761F RID: 30239
		private const float BenchSeatApproachMargin = 3f;

		// Token: 0x04007620 RID: 30240
		private static readonly List<NPCWanderController> ActiveWanderers = new List<NPCWanderController>(64);

		// Token: 0x04007621 RID: 30241
		[Header("Search")]
		[SerializeField]
		private float searchRadius = 80f;

		// Token: 0x04007622 RID: 30242
		[SerializeField]
		private int maxSearchAttempts = 5;

		// Token: 0x04007623 RID: 30243
		[Header("Action Registry")]
		[SerializeField]
		private NPCActionRegistry actionRegistry;

		// Token: 0x04007624 RID: 30244
		[Header("Fallback")]
		[SerializeField]
		private float fallbackWanderRadius = 20f;

		// Token: 0x04007625 RID: 30245
		[SerializeField]
		private float fallbackWaitTime = 3f;

		// Token: 0x04007626 RID: 30246
		[Header("Movement")]
		[SerializeField]
		private bool useFollowerSpeed = true;

		// Token: 0x04007627 RID: 30247
		[SerializeField]
		private float customWalkSpeed = 1.5f;

		// Token: 0x04007628 RID: 30248
		[SerializeField]
		private float rotationSpeed = 180f;

		// Token: 0x04007629 RID: 30249
		[Header("Debug")]
		[SerializeField]
		private bool debugLog;

		// Token: 0x0400762A RID: 30250
		private AStarNPCController _controller;

		// Token: 0x0400762B RID: 30251
		private FollowerEntity _follower;

		// Token: 0x0400762C RID: 30252
		private float _authoredFollowerSpeed;

		// Token: 0x0400762D RID: 30253
		private NPCStateMachine _fsm;

		// Token: 0x0400762E RID: 30254
		private NPCAmbientLife _ambientLife;

		// Token: 0x0400762F RID: 30255
		private NPCBenchSeating _benchSeating;

		// Token: 0x04007630 RID: 30256
		private float _nextBenchSearch;

		// Token: 0x04007632 RID: 30258
		public bool ExternalControl;

		// Token: 0x04007633 RID: 30259
		private bool _externalControlReleased;

		// Token: 0x04007634 RID: 30260
		private readonly HashSet<Object> _recoveryOwners = new HashSet<Object>();

		// Token: 0x04007635 RID: 30261
		private NPCInterestPoint _poi;

		// Token: 0x04007636 RID: 30262
		private NPCInterestPoint _lastPoi;

		// Token: 0x04007637 RID: 30263
		private Vector3 _currentNavigationTarget;

		// Token: 0x04007638 RID: 30264
		private int _instanceId;

		// Token: 0x04007639 RID: 30265
		private float _timer;

		// Token: 0x0400763A RID: 30266
		private Vector3 _walkTarget;

		// Token: 0x0400763B RID: 30267
		private Quaternion _targetRotation;

		// Token: 0x0400763C RID: 30268
		private bool _isInitialized;

		// Token: 0x0400763D RID: 30269
		[Header("Stuck Watchdog")]
		[SerializeField]
		private float navStuckTimeout = 4f;

		// Token: 0x0400763E RID: 30270
		[SerializeField]
		private float navStuckMinMovement = 0.5f;

		// Token: 0x0400763F RID: 30271
		[SerializeField]
		private float navStuckSampleInterval = 1f;

		// Token: 0x04007640 RID: 30272
		[SerializeField]
		private bool logStuckDiagnostics = true;

		// Token: 0x04007641 RID: 30273
		[Tooltip("After a stuck event, force a random-wander fallback for this many seconds before allowing POI selection again. Prevents getting stuck on the same POI repeatedly.")]
		[SerializeField]
		private float postStuckSkipPoiDuration = 6f;

		// Token: 0x04007642 RID: 30274
		private Vector3 _navStuckLastPos;

		// Token: 0x04007643 RID: 30275
		private float _navStuckLastSampleTime;

		// Token: 0x04007644 RID: 30276
		private float _navStuckStartTime;

		// Token: 0x04007645 RID: 30277
		private float _skipPoiUntilTime;

		// Token: 0x04007646 RID: 30278
		private bool _simulationPauseActive;

		// Token: 0x04007647 RID: 30279
		private float _simulationPauseStartedAt;

		// Token: 0x02000EFF RID: 3839
		public enum WanderState
		{
			// Token: 0x04007649 RID: 30281
			Disabled,
			// Token: 0x0400764A RID: 30282
			Idle,
			// Token: 0x0400764B RID: 30283
			Navigating,
			// Token: 0x0400764C RID: 30284
			WaitingManualMode,
			// Token: 0x0400764D RID: 30285
			WalkingIn,
			// Token: 0x0400764E RID: 30286
			Aligning,
			// Token: 0x0400764F RID: 30287
			Performing,
			// Token: 0x04007650 RID: 30288
			StandingUp,
			// Token: 0x04007651 RID: 30289
			WalkingOut,
			// Token: 0x04007652 RID: 30290
			WaitingNavMode,
			// Token: 0x04007653 RID: 30291
			ReleasingBench
		}
	}
}
