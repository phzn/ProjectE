using System;
using System.Collections;
using System.Collections.Generic;
using Features.Detection;
using Features.GameOver;
using Features.NPC;
using Features.RunModes;
using Features.TimeFreeze;
using Features.WardrobeCombos;
using Pathfinding;
using PoolWaterTools;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

namespace Features.Police
{
	// Token: 0x02000DF5 RID: 3573
	public class PolicePatrol : MonoBehaviour
	{
		// Token: 0x140000B2 RID: 178
		// (add) Token: 0x06006A38 RID: 27192 RVA: 0x002A95E4 File Offset: 0x002A77E4
		// (remove) Token: 0x06006A39 RID: 27193 RVA: 0x002A9618 File Offset: 0x002A7818
		public static event Action<PolicePatrol> OnChaseStarted;

		// Token: 0x140000B3 RID: 179
		// (add) Token: 0x06006A3A RID: 27194 RVA: 0x002A964C File Offset: 0x002A784C
		// (remove) Token: 0x06006A3B RID: 27195 RVA: 0x002A9680 File Offset: 0x002A7880
		public static event Action<PolicePatrol> OnChaseEscaped;

		// Token: 0x17001174 RID: 4468
		// (get) Token: 0x06006A3C RID: 27196 RVA: 0x002A96B3 File Offset: 0x002A78B3
		// (set) Token: 0x06006A3D RID: 27197 RVA: 0x002A96BB File Offset: 0x002A78BB
		public PolicePatrol.PatrolState CurrentState { get; private set; }

		// Token: 0x17001175 RID: 4469
		// (get) Token: 0x06006A3E RID: 27198 RVA: 0x002A96C4 File Offset: 0x002A78C4
		public NPCSenses Initiator
		{
			get
			{
				return this._initiator;
			}
		}

		// Token: 0x17001176 RID: 4470
		// (get) Token: 0x06006A3F RID: 27199 RVA: 0x002A96CC File Offset: 0x002A78CC
		public IReadOnlyList<NPCSenses> AdditionalActors
		{
			get
			{
				return this._additionalActors;
			}
		}

		// Token: 0x17001177 RID: 4471
		// (get) Token: 0x06006A40 RID: 27200 RVA: 0x002A96D4 File Offset: 0x002A78D4
		public TaserOfficer TaserMember
		{
			get
			{
				return this._taserMember;
			}
		}

		// Token: 0x06006A41 RID: 27201 RVA: 0x002A96DC File Offset: 0x002A78DC
		public void Initialize(NPCSenses initiator, List<NPCSenses> additionalActors, NPCSenses taserMember = null)
		{
			this._initiator = initiator;
			this._additionalActors = additionalActors ?? new List<NPCSenses>();
			this._baselineMaxSpeed.Clear();
			this.CacheBaseline(this._initiator);
			foreach (NPCSenses npcsenses in this._additionalActors)
			{
				this.CacheBaseline(npcsenses);
			}
			PolicePatrol.ApplyTaserRole(this._initiator, this._initiator == taserMember);
			foreach (NPCSenses npcsenses2 in this._additionalActors)
			{
				PolicePatrol.ApplyTaserRole(npcsenses2, npcsenses2 == taserMember);
			}
			this._taserMember = ((taserMember != null) ? taserMember.GetComponent<TaserOfficer>() : null);
			this.CurrentState = PolicePatrol.PatrolState.Roaming;
			this.EnterRoaming();
		}

		// Token: 0x06006A42 RID: 27202 RVA: 0x002A97E0 File Offset: 0x002A79E0
		private static void ApplyTaserRole(NPCSenses npc, bool isTaserMember)
		{
			if (npc == null)
			{
				return;
			}
			TaserOfficer component = npc.GetComponent<TaserOfficer>();
			if (component == null)
			{
				return;
			}
			component.enabled = isTaserMember;
			component.SetTaserVisible(isTaserMember);
		}

		// Token: 0x06006A43 RID: 27203 RVA: 0x002A9818 File Offset: 0x002A7A18
		private void CacheBaseline(NPCSenses npc)
		{
			if (npc == null)
			{
				return;
			}
			FollowerEntity component = npc.GetComponent<FollowerEntity>();
			if (component != null)
			{
				this._baselineMaxSpeed[npc] = component.maxSpeed;
			}
		}

		// Token: 0x06006A44 RID: 27204 RVA: 0x002A9851 File Offset: 0x002A7A51
		private void Start()
		{
			this.TryHookDirector();
		}

		// Token: 0x06006A45 RID: 27205 RVA: 0x002A9859 File Offset: 0x002A7A59
		private void OnEnable()
		{
			this._simulationPauseActive = false;
			NPCSimulationPause.PauseChanged += this.HandleSimulationPauseChanged;
			NPCSimulationPause.RegisterParticipant(this);
			this.HandleSimulationPauseChanged(NPCSimulationPause.IsPaused);
		}

		// Token: 0x06006A46 RID: 27206 RVA: 0x002A9884 File Offset: 0x002A7A84
		private void OnDisable()
		{
			NPCSimulationPause.PauseChanged -= this.HandleSimulationPauseChanged;
			this.UnhookDirector();
		}

		// Token: 0x06006A47 RID: 27207 RVA: 0x002A989D File Offset: 0x002A7A9D
		private void OnDestroy()
		{
			this.UnhookDirector();
		}

		// Token: 0x06006A48 RID: 27208 RVA: 0x002A98A8 File Offset: 0x002A7AA8
		private void Update()
		{
			if (DetectionTransitionGate.IsBlocked || NPCSimulationPause.IsPaused)
			{
				this.BeginSimulationPauseIfNeeded();
				return;
			}
			this.ResumeSimulationIfNeeded();
			float frameTimeLoss = NPCSimulationPause.FrameTimeLoss;
			if (frameTimeLoss > 0f && this.CurrentState == PolicePatrol.PatrolState.Chasing)
			{
				this._lastChaseSightTime += frameTimeLoss;
			}
			if (NPCSimulationPause.SimulationScale <= 0.0001f)
			{
				return;
			}
			this.TryHookDirector();
			if (GameOverManager.Instance != null && GameOverManager.Instance.IsGameOver)
			{
				if (this.CurrentState != PolicePatrol.PatrolState.Stopped)
				{
					this.TransitionTo(PolicePatrol.PatrolState.Stopped);
				}
				return;
			}
			if (this._directorActive || (PerpWalkController.Instance != null && PerpWalkController.Instance.IsActive))
			{
				if (this.CurrentState != PolicePatrol.PatrolState.Stopped)
				{
					this.TransitionTo(PolicePatrol.PatrolState.Stopped);
				}
				return;
			}
			if (this._captureFormationRoutine == null && (this._resumeRoamingAfterDirector || this.CurrentState == PolicePatrol.PatrolState.Stopped))
			{
				this._resumeRoamingAfterDirector = false;
				if (this.CurrentState == PolicePatrol.PatrolState.Stopped)
				{
					this.TransitionTo(PolicePatrol.PatrolState.Roaming);
				}
			}
			if (WardrobeComboRuntime.ShouldIgnore(DetectionActorType.Police) || !RunModeService.EncountersEnabled)
			{
				if (this.CurrentState == PolicePatrol.PatrolState.Chasing && this._captureFormationRoutine == null)
				{
					this.TransitionTo(PolicePatrol.PatrolState.Roaming);
				}
				return;
			}
			this.EnsureActiveLeader();
			PolicePatrol.PatrolState currentState = this.CurrentState;
			if (currentState == PolicePatrol.PatrolState.Roaming)
			{
				this.UpdateRoamingDetection();
				this.UpdateRoamingFormation();
				return;
			}
			if (currentState != PolicePatrol.PatrolState.Chasing)
			{
				return;
			}
			this.UpdateChasing();
		}

		// Token: 0x06006A49 RID: 27209 RVA: 0x002A99E4 File Offset: 0x002A7BE4
		private void HandleSimulationPauseChanged(bool paused)
		{
			if (paused)
			{
				this.BeginSimulationPauseIfNeeded();
				return;
			}
			this.ResumeSimulationIfNeeded();
		}

		// Token: 0x06006A4A RID: 27210 RVA: 0x002A99F6 File Offset: 0x002A7BF6
		private void BeginSimulationPauseIfNeeded()
		{
			if (this._simulationPauseActive)
			{
				return;
			}
			this._simulationPauseActive = true;
			this._simulationPauseStartedAt = Time.time;
		}

		// Token: 0x06006A4B RID: 27211 RVA: 0x002A9A13 File Offset: 0x002A7C13
		private void ResumeSimulationIfNeeded()
		{
			if (!this._simulationPauseActive)
			{
				return;
			}
			if (this.CurrentState == PolicePatrol.PatrolState.Chasing)
			{
				this._lastChaseSightTime += Mathf.Max(0f, Time.time - this._simulationPauseStartedAt);
			}
			this._simulationPauseActive = false;
		}

		// Token: 0x17001178 RID: 4472
		// (get) Token: 0x06006A4C RID: 27212 RVA: 0x002A9A51 File Offset: 0x002A7C51
		private static bool IsSimulationStopped
		{
			get
			{
				return NPCSimulationPause.IsPaused || NPCSimulationPause.SimulationScale <= 0.0001f;
			}
		}

		// Token: 0x06006A4D RID: 27213 RVA: 0x002A9A6C File Offset: 0x002A7C6C
		private void TryHookDirector()
		{
			ExhibitionEventDirector instance = ExhibitionEventDirector.Instance;
			if (!this._directorHooked || this._hookedDirector != instance)
			{
				this.UnhookDirector();
				this._hookedDirector = instance;
				if (instance != null)
				{
					UnityEvent<NPCSenses> onDetectionStarted = instance.OnDetectionStarted;
					if (onDetectionStarted != null)
					{
						onDetectionStarted.AddListener(new UnityAction<NPCSenses>(this.HandleDirectorStarted));
					}
					UnityEvent onSequenceComplete = instance.OnSequenceComplete;
					if (onSequenceComplete != null)
					{
						onSequenceComplete.AddListener(new UnityAction(this.HandleDirectorComplete));
					}
					this._directorHooked = true;
				}
			}
			bool flag = instance != null && instance.IsEventActive;
			if (flag && !this._directorActive)
			{
				this.HandleDirectorStarted(null);
				return;
			}
			if (!flag && this._directorActive)
			{
				this.HandleDirectorComplete();
			}
		}

		// Token: 0x06006A4E RID: 27214 RVA: 0x002A9B24 File Offset: 0x002A7D24
		private void UnhookDirector()
		{
			if (!this._directorHooked)
			{
				return;
			}
			ExhibitionEventDirector hookedDirector = this._hookedDirector;
			if (hookedDirector != null)
			{
				if (hookedDirector.OnDetectionStarted != null)
				{
					hookedDirector.OnDetectionStarted.RemoveListener(new UnityAction<NPCSenses>(this.HandleDirectorStarted));
				}
				if (hookedDirector.OnSequenceComplete != null)
				{
					hookedDirector.OnSequenceComplete.RemoveListener(new UnityAction(this.HandleDirectorComplete));
				}
			}
			this._directorHooked = false;
			this._hookedDirector = null;
		}

		// Token: 0x06006A4F RID: 27215 RVA: 0x002A9B96 File Offset: 0x002A7D96
		private void HandleDirectorStarted(NPCSenses _)
		{
			this._directorActive = true;
			if (this._taserMember != null)
			{
				this._taserMember.CancelFire();
			}
			this._activeFireRoutine = null;
		}

		// Token: 0x06006A50 RID: 27216 RVA: 0x002A9BBF File Offset: 0x002A7DBF
		private void HandleDirectorComplete()
		{
			this._directorActive = false;
			this._resumeRoamingAfterDirector = true;
		}

		// Token: 0x06006A51 RID: 27217 RVA: 0x002A9BD0 File Offset: 0x002A7DD0
		private void TransitionTo(PolicePatrol.PatrolState next)
		{
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[PolicePatrol] {0}: {1} → {2}", base.name, this.CurrentState, next));
			}
			this.ExitState(this.CurrentState);
			this.CurrentState = next;
			this.EnterState(next);
		}

		// Token: 0x06006A52 RID: 27218 RVA: 0x002A9C25 File Offset: 0x002A7E25
		private void EnterState(PolicePatrol.PatrolState s)
		{
			switch (s)
			{
			case PolicePatrol.PatrolState.Roaming:
				this.EnterRoaming();
				return;
			case PolicePatrol.PatrolState.Chasing:
				this.EnterChasing();
				return;
			case PolicePatrol.PatrolState.Stopped:
				this.EnterStopped();
				return;
			default:
				return;
			}
		}

		// Token: 0x06006A53 RID: 27219 RVA: 0x00004F31 File Offset: 0x00003131
		private void ExitState(PolicePatrol.PatrolState s)
		{
		}

		// Token: 0x06006A54 RID: 27220 RVA: 0x002A9C4E File Offset: 0x002A7E4E
		private void EnterRoaming()
		{
			this.ConfigureRoamingRoles();
		}

		// Token: 0x06006A55 RID: 27221 RVA: 0x002A9C58 File Offset: 0x002A7E58
		private void ConfigureRoamingRoles()
		{
			this.RestoreRoamingMovement(this._initiator);
			PolicePatrol.SetSensesEnabled(this._initiator, true);
			PolicePatrol.SetPOINavigatorEnabled(this._initiator, true);
			foreach (NPCSenses npcsenses in this._additionalActors)
			{
				this.RestoreRoamingMovement(npcsenses);
				PolicePatrol.SetSensesEnabled(npcsenses, true);
				PolicePatrol.SetPOINavigatorEnabled(npcsenses, false);
			}
		}

		// Token: 0x06006A56 RID: 27222 RVA: 0x002A9CE0 File Offset: 0x002A7EE0
		private void UpdateRoamingDetection()
		{
			if (PlayerStealthBridge.IsFullyHidden)
			{
				return;
			}
			SensoryManager instance = SensoryManager.Instance;
			if (instance == null || instance.DisableFinalDetection || !RunModeService.EncountersEnabled || WardrobeComboRuntime.ShouldIgnore(DetectionActorType.Police))
			{
				return;
			}
			NPCSenses npcsenses = PolicePatrol.FindFirstDetector(this._initiator, this._additionalActors);
			if (npcsenses == null)
			{
				return;
			}
			if (npcsenses != this._initiator)
			{
				this.PromoteToLeader(npcsenses);
			}
			this.TransitionTo(PolicePatrol.PatrolState.Chasing);
		}

		// Token: 0x06006A57 RID: 27223 RVA: 0x002A9D54 File Offset: 0x002A7F54
		private static NPCSenses FindFirstDetector(NPCSenses leader, IList<NPCSenses> followers)
		{
			if (PolicePatrol.IsValidDetector(leader) && leader.HasLineOfSight)
			{
				return leader;
			}
			if (followers != null)
			{
				for (int i = 0; i < followers.Count; i++)
				{
					NPCSenses npcsenses = followers[i];
					if (PolicePatrol.IsValidDetector(npcsenses) && npcsenses.HasLineOfSight)
					{
						return npcsenses;
					}
				}
			}
			if (PolicePatrol.IsValidDetector(leader) && leader.HasAudioContact)
			{
				return leader;
			}
			if (followers != null)
			{
				for (int j = 0; j < followers.Count; j++)
				{
					NPCSenses npcsenses2 = followers[j];
					if (PolicePatrol.IsValidDetector(npcsenses2) && npcsenses2.HasAudioContact)
					{
						return npcsenses2;
					}
				}
			}
			return null;
		}

		// Token: 0x06006A58 RID: 27224 RVA: 0x002A9DE2 File Offset: 0x002A7FE2
		private static bool IsValidDetector(NPCSenses npc)
		{
			return npc != null && npc.isActiveAndEnabled && !npc.IsInCooldown && !npc.BlocksFinalDetectionWhileSeated && !NPCPoolRecovery.IsUnavailableForEncounter(npc);
		}

		// Token: 0x06006A59 RID: 27225 RVA: 0x002A9E10 File Offset: 0x002A8010
		private bool AnyMemberHasContact()
		{
			if (PlayerStealthBridge.IsFullyHidden)
			{
				return false;
			}
			if (this._initiator != null && (this._initiator.HasLineOfSight || this._initiator.HasAudioContact))
			{
				return true;
			}
			for (int i = 0; i < this._additionalActors.Count; i++)
			{
				NPCSenses npcsenses = this._additionalActors[i];
				if (!(npcsenses == null) && (npcsenses.HasLineOfSight || npcsenses.HasAudioContact))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006A5A RID: 27226 RVA: 0x002A9E90 File Offset: 0x002A8090
		private void PromoteToLeader(NPCSenses newLeader)
		{
			if (newLeader == null || newLeader == this._initiator)
			{
				return;
			}
			int num = this._additionalActors.IndexOf(newLeader);
			if (num < 0)
			{
				return;
			}
			NPCSenses initiator = this._initiator;
			this._initiator = newLeader;
			this._additionalActors[num] = initiator;
			PolicePatrol.SetPOINavigatorEnabled(this._initiator, this.CurrentState == PolicePatrol.PatrolState.Roaming);
			PolicePatrol.SetPOINavigatorEnabled(initiator, false);
			if (this.debugLogs)
			{
				Debug.Log(string.Concat(new string[]
				{
					"[PolicePatrol] Leader swap: ",
					(initiator != null) ? initiator.name : "<null>",
					" → ",
					this._initiator.name,
					" (spotted player first)"
				}));
			}
		}

		// Token: 0x06006A5B RID: 27227 RVA: 0x002A9F54 File Offset: 0x002A8154
		private void EnsureActiveLeader()
		{
			if (this._initiator != null && this._initiator.gameObject.activeInHierarchy)
			{
				return;
			}
			for (int i = 0; i < this._additionalActors.Count; i++)
			{
				NPCSenses npcsenses = this._additionalActors[i];
				if (!(npcsenses == null) && npcsenses.gameObject.activeInHierarchy)
				{
					this.PromoteToLeader(npcsenses);
					return;
				}
			}
		}

		// Token: 0x06006A5C RID: 27228 RVA: 0x002A9FC4 File Offset: 0x002A81C4
		private void UpdateRoamingFormation()
		{
			if (this._initiator == null || this._additionalActors == null || this._additionalActors.Count == 0)
			{
				return;
			}
			Vector3 position = this._initiator.transform.position;
			Vector3 forward = this._initiator.transform.forward;
			Vector3 right = this._initiator.transform.right;
			forward.y = 0f;
			forward.Normalize();
			right.y = 0f;
			right.Normalize();
			for (int i = 0; i < this._additionalActors.Count; i++)
			{
				NPCSenses npcsenses = this._additionalActors[i];
				if (!(npcsenses == null))
				{
					float num = ((i % 2 == 0) ? (-1f) : 1f);
					int num2 = i / 2 + 1;
					Vector3 vector = -forward * ((float)num2 * this.roamingFollowDistance) + right * (num * this.roamingLateralSpacing);
					Vector3 vector2 = position + vector;
					AStarNPCController component = npcsenses.GetComponent<AStarNPCController>();
					if (!(component == null) && !component.IsNavigationLocked && !NPCPoolRecovery.IsActorRecovering(npcsenses))
					{
						FollowerEntity component2 = npcsenses.GetComponent<FollowerEntity>();
						if (!(component2 == null))
						{
							if (Vector3.Distance(component2.destination, vector2) > this.roamingFormationUpdateThreshold)
							{
								component.SetDestination(vector2);
							}
							if (Vector3.Distance(npcsenses.transform.position, vector2) <= this.roamingFaceLeaderDistance)
							{
								component2.isStopped = true;
								Vector3 vector3 = position - npcsenses.transform.position;
								vector3.y = 0f;
								if (vector3.sqrMagnitude > 0.0001f)
								{
									Quaternion quaternion = Quaternion.LookRotation(vector3.normalized, Vector3.up);
									npcsenses.transform.rotation = Quaternion.RotateTowards(npcsenses.transform.rotation, quaternion, this.roamingFaceLeaderSpeed * NPCSimulationPause.SimulationDeltaTime);
								}
							}
							else
							{
								component2.isStopped = false;
							}
						}
					}
				}
			}
		}

		// Token: 0x06006A5D RID: 27229 RVA: 0x002AA1D0 File Offset: 0x002A83D0
		private void EnterChasing()
		{
			Action<PolicePatrol> onChaseStarted = PolicePatrol.OnChaseStarted;
			if (onChaseStarted != null)
			{
				onChaseStarted(this);
			}
			PolicePatrol.SetPOINavigatorEnabled(this._initiator, false);
			this._lastChaseSightTime = Time.time;
			this.SendPatrolRunningTowardPlayer();
		}

		// Token: 0x06006A5E RID: 27230 RVA: 0x002AA200 File Offset: 0x002A8400
		private void UpdateChasing()
		{
			this.EnsureActiveLeader();
			if (this._initiator == null)
			{
				return;
			}
			if (this._captureFormationRoutine != null)
			{
				return;
			}
			SensoryManager instance = SensoryManager.Instance;
			if (instance == null)
			{
				return;
			}
			StealthActor player = instance.GetPlayer();
			if (player == null)
			{
				return;
			}
			if (this.AnyMemberHasContact())
			{
				this._lastChaseSightTime = Time.time;
			}
			NPCSenses npcsenses = (PlayerStealthBridge.IsFullyHidden ? null : this.FindCatcher(player.Position));
			if (npcsenses != null && RunModeService.EncountersEnabled && !TimeFreezeController.IsAnyActive)
			{
				if (this._taserMember != null)
				{
					this._taserMember.CancelFire();
				}
				this._activeFireRoutine = null;
				if (npcsenses != this._initiator)
				{
					this.PromoteToLeader(npcsenses);
				}
				if (this.debugLogs)
				{
					Debug.Log(string.Concat(new string[] { "[PolicePatrol.", base.name, "] Catch by ", npcsenses.name, "!" }));
				}
				PlayerStunExecutor instance2 = PlayerStunExecutor.Instance;
				if (instance2 != null && instance2.IsStunned)
				{
					if (this._captureFormationRoutine == null)
					{
						this._captureFormationRoutine = base.StartCoroutine(this.DelayedCaptureAfterStun());
					}
					return;
				}
				if (this.TriggerCaptureEncounter())
				{
					this.TransitionTo(PolicePatrol.PatrolState.Stopped);
					return;
				}
			}
			if (this._taserMember != null && this._activeFireRoutine == null && player != null && this._captureFormationRoutine == null && !PlayerStealthBridge.IsFullyHidden && this._taserMember.CanFire(player))
			{
				this.StopTaserMemberMovement();
				this._activeFireRoutine = this._taserMember.FireAt(player);
				base.StartCoroutine(this.TrackFireCompletion());
			}
			if (this.chaseLoseSightTimeout > 0f && Time.time - this._lastChaseSightTime > this.chaseLoseSightTimeout)
			{
				if (this.debugLogs)
				{
					Debug.Log(string.Format("[PolicePatrol.{0}] Chasing → Roaming (lost sight {1}s)", base.name, this.chaseLoseSightTimeout));
				}
				if (this._taserMember != null)
				{
					this._taserMember.CancelFire();
				}
				this._activeFireRoutine = null;
				if (this.cooldownOnChaseEscape)
				{
					this.ForEachMember(delegate(NPCSenses npc)
					{
						if (npc != null && npc.gameObject.activeInHierarchy)
						{
							npc.StartCooldown();
						}
					});
				}
				Action<PolicePatrol> onChaseEscaped = PolicePatrol.OnChaseEscaped;
				if (onChaseEscaped != null)
				{
					onChaseEscaped(this);
				}
				this.TransitionTo(PolicePatrol.PatrolState.Roaming);
				return;
			}
			if (this.chasingReaimEveryFrames > 0 && Time.frameCount % this.chasingReaimEveryFrames == 0)
			{
				this.SendPatrolRunningTowardPlayer();
			}
		}

		// Token: 0x06006A5F RID: 27231 RVA: 0x002AA482 File Offset: 0x002A8682
		private IEnumerator DelayedCaptureAfterStun()
		{
			PlayerStunExecutor stunExec = PlayerStunExecutor.Instance;
			int waterEntryGeneration = ((stunExec != null) ? stunExec.WaterEntryGeneration : 0);
			if (SensoryManager.Instance != null)
			{
				SensoryManager.Instance.SetPaused(true);
			}
			AttentionUIController.GlobalForceHideAll = true;
			if (stunExec != null)
			{
				stunExec.SetExternalLockHold(true);
			}
			int num = 1;
			List<NPCSenses> additionalActors = this._additionalActors;
			List<NPCSenses> members = new List<NPCSenses>(num + ((additionalActors != null) ? additionalActors.Count : 0));
			if (this._initiator != null)
			{
				members.Add(this._initiator);
			}
			if (this._additionalActors != null)
			{
				for (int i = 0; i < this._additionalActors.Count; i++)
				{
					if (this._additionalActors[i] != null && this._additionalActors[i] != this._initiator)
					{
						members.Add(this._additionalActors[i]);
					}
				}
			}
			SensoryManager instance = SensoryManager.Instance;
			StealthActor stealthActor = ((instance != null) ? instance.GetPlayer() : null);
			Transform transform = ((stealthActor != null) ? stealthActor.transform : null);
			this.EnterPostStunPose(members, transform);
			if (this._taserMember != null && this._activeFireRoutine != null)
			{
				this._taserMember.CancelFire();
				this._activeFireRoutine = null;
			}
			while (stunExec != null)
			{
				if (!stunExec.IsStunned)
				{
					break;
				}
				if (this.CancelPostStunCaptureInWater(stunExec, members, waterEntryGeneration))
				{
					yield break;
				}
				if (PolicePatrol.IsSimulationStopped)
				{
					yield return null;
				}
				else
				{
					for (int j = 0; j < members.Count; j++)
					{
						NPCSenses npcsenses = members[j];
						FollowerEntity followerEntity = ((npcsenses != null) ? npcsenses.GetComponent<FollowerEntity>() : null);
						if (!(followerEntity == null))
						{
							if (!followerEntity.isStopped)
							{
								followerEntity.isStopped = true;
							}
							if (followerEntity.canMove)
							{
								followerEntity.canMove = false;
							}
						}
					}
					yield return null;
				}
			}
			while (PolicePatrol.IsSimulationStopped || TimeFreezeController.IsAnyActive)
			{
				if (this.CancelPostStunCaptureInWater(stunExec, members, waterEntryGeneration))
				{
					yield break;
				}
				yield return null;
			}
			if (this.CancelPostStunCaptureInWater(stunExec, members, waterEntryGeneration))
			{
				yield break;
			}
			ExhibitionEventDirector instance2 = ExhibitionEventDirector.Instance;
			if (GameOverManager.Instance != null && GameOverManager.Instance.IsGameOver)
			{
				this.ExitPostStunPose(members);
				AttentionUIController.GlobalForceHideAll = false;
				if (SensoryManager.Instance != null)
				{
					SensoryManager.Instance.SetPaused(false);
				}
				if (stunExec != null)
				{
					stunExec.SetExternalLockHold(false);
				}
				this._captureFormationRoutine = null;
				yield break;
			}
			if (instance2 != null && instance2.IsEventActive)
			{
				this.ExitPostStunPose(members);
				this._captureFormationRoutine = null;
				yield break;
			}
			this.ExitPostStunPose(members);
			bool flag = this.TriggerCaptureEncounter();
			AttentionUIController.GlobalForceHideAll = false;
			if (SensoryManager.Instance != null)
			{
				SensoryManager.Instance.SetPaused(false);
			}
			if (stunExec != null)
			{
				stunExec.SetExternalLockHold(false);
			}
			if (flag)
			{
				this.TransitionTo(PolicePatrol.PatrolState.Stopped);
			}
			else
			{
				this.SendPatrolRunningTowardPlayer();
			}
			this._captureFormationRoutine = null;
			yield break;
		}

		// Token: 0x06006A60 RID: 27232 RVA: 0x002AA494 File Offset: 0x002A8694
		private bool CancelPostStunCaptureInWater(PlayerStunExecutor stunExec, List<NPCSenses> members, int waterEntryGeneration)
		{
			if (stunExec == null || (stunExec.WaterEntryGeneration == waterEntryGeneration && !PoolWaterRecovery.IsPlayerInWater(stunExec.transform)))
			{
				return false;
			}
			this.ExitPostStunPose(members);
			bool flag = TaserOfficer.IsEncounterProtected || (GameOverManager.Instance != null && GameOverManager.Instance.IsGameOver);
			if (!flag)
			{
				AttentionUIController.GlobalForceHideAll = false;
				if (SensoryManager.Instance != null)
				{
					SensoryManager.Instance.SetPaused(false);
				}
			}
			if (stunExec.ExternalLockHold)
			{
				stunExec.SetExternalLockHold(false);
			}
			this._captureFormationRoutine = null;
			if (!flag)
			{
				this.SendPatrolRunningTowardPlayer();
			}
			return true;
		}

		// Token: 0x06006A61 RID: 27233 RVA: 0x002AA530 File Offset: 0x002A8730
		private void EnterPostStunPose(List<NPCSenses> members, Transform playerTransform)
		{
			if (members == null)
			{
				return;
			}
			for (int i = 0; i < members.Count; i++)
			{
				NPCSenses npcsenses = members[i];
				if (!(npcsenses == null))
				{
					FollowerEntity component = npcsenses.GetComponent<FollowerEntity>();
					if (component != null)
					{
						component.isStopped = true;
						component.canMove = false;
						component.maxSpeed = 0f;
						component.SetDestination(npcsenses.transform.position, default(float3));
					}
					AStarNPCController component2 = npcsenses.GetComponent<AStarNPCController>();
					if (component2 != null)
					{
						component2.SetSpeedOverride(0f);
					}
					if (playerTransform != null)
					{
						Vector3 vector = playerTransform.position - npcsenses.transform.position;
						vector.y = 0f;
						if (vector.sqrMagnitude > 0.0001f)
						{
							npcsenses.transform.rotation = Quaternion.LookRotation(vector.normalized, Vector3.up);
						}
					}
					Animator componentInChildren = npcsenses.GetComponentInChildren<Animator>();
					if (componentInChildren != null)
					{
						if (1 < componentInChildren.layerCount)
						{
							componentInChildren.SetLayerWeight(1, 1f);
						}
						componentInChildren.SetInteger(PolicePatrol.PostStunReactionIdleHash, this.postStunReactionIdle);
						componentInChildren.SetInteger(PolicePatrol.PostStunFaceExpressionHash, this.postStunFaceExpressionIndex);
					}
					if (playerTransform != null)
					{
						NPCSimpleLookAt component3 = npcsenses.GetComponent<NPCSimpleLookAt>();
						if (component3 != null)
						{
							component3.StartLookingAt(playerTransform);
						}
					}
				}
			}
		}

		// Token: 0x06006A62 RID: 27234 RVA: 0x002AA698 File Offset: 0x002A8898
		private void ExitPostStunPose(List<NPCSenses> members)
		{
			if (members == null)
			{
				return;
			}
			for (int i = 0; i < members.Count; i++)
			{
				NPCSenses npcsenses = members[i];
				if (!(npcsenses == null))
				{
					FollowerEntity component = npcsenses.GetComponent<FollowerEntity>();
					if (component != null)
					{
						component.canMove = true;
						float num;
						if (this._baselineMaxSpeed.TryGetValue(npcsenses, out num))
						{
							component.maxSpeed = num;
						}
					}
					Animator componentInChildren = npcsenses.GetComponentInChildren<Animator>();
					if (componentInChildren != null)
					{
						componentInChildren.SetInteger(PolicePatrol.PostStunReactionIdleHash, 0);
					}
					NPCSimpleLookAt component2 = npcsenses.GetComponent<NPCSimpleLookAt>();
					if (component2 != null)
					{
						component2.StopLooking();
					}
				}
			}
		}

		// Token: 0x17001179 RID: 4473
		// (get) Token: 0x06006A63 RID: 27235 RVA: 0x002AA732 File Offset: 0x002A8932
		public bool IsAwaitingPostStunCapture
		{
			get
			{
				return this._captureFormationRoutine != null;
			}
		}

		// Token: 0x06006A64 RID: 27236 RVA: 0x002AA740 File Offset: 0x002A8940
		private bool TriggerCaptureEncounter()
		{
			if (!RunModeService.EncountersEnabled)
			{
				return false;
			}
			if (TimeFreezeController.IsAnyActive || DetectionTransitionGate.IsBlocked || PlayerStealthBridge.IsFullyHidden)
			{
				return false;
			}
			ExhibitionEventDirector instance = ExhibitionEventDirector.Instance;
			if ((instance != null && instance.IsEventActive) || (PerpWalkController.Instance != null && PerpWalkController.Instance.IsActive))
			{
				return true;
			}
			if (this.captureProfile != null && instance != null && this._initiator != null)
			{
				NPCSenses capturedInitiator = this._initiator;
				List<NPCSenses> capturedEscort = ((this._additionalActors != null) ? new List<NPCSenses>(this._additionalActors) : null);
				EncounterRequest encounterRequest = new EncounterRequest
				{
					initiator = capturedInitiator,
					additionalActors = capturedEscort,
					profile = this.captureProfile,
					onSceneEnded = delegate
					{
						if (PerpWalkController.Instance != null)
						{
							PerpWalkController.Instance.BeginPerpWalk(capturedInitiator, capturedEscort);
						}
					}
				};
				if (this.debugLogs)
				{
					Debug.Log(string.Format("[PolicePatrol.{0}] TriggerCaptureEncounter via Director (profile='{1}', members={2})", base.name, this.captureProfile.name, (this._additionalActors != null) ? (this._additionalActors.Count + 1) : 1));
				}
				instance.TriggerEncounter(encounterRequest);
				return instance.IsEventActive;
			}
			if (this.debugLogs)
			{
				Debug.LogWarning("[PolicePatrol." + base.name + "] No captureProfile or Director → fallback GameOver");
			}
			if (GameOverManager.Instance != null)
			{
				GameOverManager.Instance.TriggerCapture(this);
			}
			return GameOverManager.Instance != null && GameOverManager.Instance.IsGameOver;
		}

		// Token: 0x06006A65 RID: 27237 RVA: 0x002AA8E0 File Offset: 0x002A8AE0
		private NPCSenses FindCatcher(Vector3 playerPos)
		{
			NPCSenses npcsenses = null;
			float num = this.catchDistance * this.catchDistance;
			PolicePatrol.ConsiderCatcher(this._initiator, playerPos, ref npcsenses, ref num);
			for (int i = 0; i < this._additionalActors.Count; i++)
			{
				PolicePatrol.ConsiderCatcher(this._additionalActors[i], playerPos, ref npcsenses, ref num);
			}
			return npcsenses;
		}

		// Token: 0x06006A66 RID: 27238 RVA: 0x002AA93C File Offset: 0x002A8B3C
		private static void ConsiderCatcher(NPCSenses member, Vector3 playerPos, ref NPCSenses catcher, ref float closestSqr)
		{
			if (!PolicePatrol.IsValidDetector(member))
			{
				return;
			}
			float sqrMagnitude = (member.transform.position - playerPos).sqrMagnitude;
			if (sqrMagnitude > closestSqr)
			{
				return;
			}
			closestSqr = sqrMagnitude;
			catcher = member;
		}

		// Token: 0x06006A67 RID: 27239 RVA: 0x002AA978 File Offset: 0x002A8B78
		private IEnumerator TrackFireCompletion()
		{
			Coroutine trackedRoutine = this._activeFireRoutine;
			TaserOfficer officer = this._taserMember;
			while (this._activeFireRoutine == trackedRoutine && officer != null)
			{
				if (!officer.IsFireSequenceActive)
				{
					break;
				}
				yield return null;
			}
			while (PolicePatrol.IsSimulationStopped)
			{
				yield return null;
			}
			if (this._activeFireRoutine != trackedRoutine)
			{
				yield break;
			}
			this._activeFireRoutine = null;
			if (this._taserMember != null && this.CurrentState == PolicePatrol.PatrolState.Chasing && this._captureFormationRoutine == null && !TaserOfficer.MustAbortAttack)
			{
				NPCSenses component = this._taserMember.GetComponent<NPCSenses>();
				SensoryManager instance = SensoryManager.Instance;
				StealthActor stealthActor = ((instance != null) ? instance.GetPlayer() : null);
				if (component != null && stealthActor != null)
				{
					this.ApplyRunDestinationFor(component, stealthActor.Position);
				}
			}
			yield break;
		}

		// Token: 0x06006A68 RID: 27240 RVA: 0x002AA988 File Offset: 0x002A8B88
		private void StopTaserMemberMovement()
		{
			if (this._taserMember == null)
			{
				return;
			}
			if (NPCPoolRecovery.IsActorRecovering(this._taserMember))
			{
				return;
			}
			NPCSenses component = this._taserMember.GetComponent<NPCSenses>();
			if (component == null)
			{
				return;
			}
			FollowerEntity component2 = component.GetComponent<FollowerEntity>();
			if (component2 != null)
			{
				component2.isStopped = true;
			}
			SensoryManager instance = SensoryManager.Instance;
			StealthActor stealthActor = ((instance != null) ? instance.GetPlayer() : null);
			if (stealthActor != null)
			{
				Vector3 vector = stealthActor.Position - component.transform.position;
				vector.y = 0f;
				if (vector.sqrMagnitude > 0.0001f)
				{
					component.transform.rotation = Quaternion.LookRotation(vector.normalized, Vector3.up);
				}
			}
		}

		// Token: 0x06006A69 RID: 27241 RVA: 0x002AAA50 File Offset: 0x002A8C50
		private void SendPatrolRunningTowardPlayer()
		{
			SensoryManager instance = SensoryManager.Instance;
			StealthActor stealthActor = ((instance != null) ? instance.GetPlayer() : null);
			if (stealthActor == null)
			{
				return;
			}
			this.ApplyRunDestinationFor(this._initiator, stealthActor.Position);
			for (int i = 0; i < this._additionalActors.Count; i++)
			{
				NPCSenses npcsenses = this._additionalActors[i];
				if (!(npcsenses == null) && (!(this._taserMember != null) || !(npcsenses == this._taserMember.GetComponent<NPCSenses>()) || this._activeFireRoutine == null))
				{
					this.ApplyRunDestinationFor(npcsenses, stealthActor.Position);
				}
			}
		}

		// Token: 0x06006A6A RID: 27242 RVA: 0x002AAAF4 File Offset: 0x002A8CF4
		private void ApplyRunDestinationFor(NPCSenses npc, Vector3 playerPos)
		{
			if (npc == null || PolicePatrol.IsSimulationStopped || NPCPoolRecovery.IsActorRecovering(npc))
			{
				return;
			}
			FollowerEntity component = npc.GetComponent<FollowerEntity>();
			if (component == null || !component.enabled || !component.entityExists)
			{
				return;
			}
			if (!NPCPoolRecovery.TryConstrainDestination(npc.transform, ref playerPos))
			{
				AStarNPCController component2 = npc.GetComponent<AStarNPCController>();
				if (component2 == null)
				{
					return;
				}
				component2.Stop();
				return;
			}
			else
			{
				if (component != null)
				{
					component.isStopped = false;
					component.maxSpeed = this.chasingRunSpeed;
					component.SetDestination(playerPos, default(float3));
				}
				AStarNPCController component3 = npc.GetComponent<AStarNPCController>();
				if (component3 == null)
				{
					return;
				}
				component3.SetSpeedOverride(this.chasingRunSpeed);
				return;
			}
		}

		// Token: 0x06006A6B RID: 27243 RVA: 0x002AABA2 File Offset: 0x002A8DA2
		private void EnterStopped()
		{
			this.ForEachMember(delegate(NPCSenses npc)
			{
				if (npc == null)
				{
					return;
				}
				FollowerEntity component = npc.GetComponent<FollowerEntity>();
				if (component != null)
				{
					component.isStopped = true;
				}
				AStarNPCController component2 = npc.GetComponent<AStarNPCController>();
				if (component2 != null)
				{
					component2.ClearSpeedOverride();
				}
				float num;
				if (this._baselineMaxSpeed.TryGetValue(npc, out num) && component != null)
				{
					component.maxSpeed = num;
				}
			});
			PolicePatrol.SetPOINavigatorEnabled(this._initiator, false);
		}

		// Token: 0x06006A6C RID: 27244 RVA: 0x002AABC4 File Offset: 0x002A8DC4
		private void RestoreRoamingMovement(NPCSenses npc)
		{
			if (npc == null || NPCPoolRecovery.IsActorRecovering(npc))
			{
				return;
			}
			FollowerEntity component = npc.GetComponent<FollowerEntity>();
			AStarNPCController component2 = npc.GetComponent<AStarNPCController>();
			if (component != null)
			{
				component.isStopped = false;
			}
			if (component2 != null)
			{
				component2.ClearSpeedOverride();
			}
			float num;
			if (component != null && this._baselineMaxSpeed.TryGetValue(npc, out num))
			{
				component.maxSpeed = num;
			}
		}

		// Token: 0x06006A6D RID: 27245 RVA: 0x002AAC2B File Offset: 0x002A8E2B
		private static void SetSensesEnabled(NPCSenses npc, bool enabled)
		{
			if (npc == null)
			{
				return;
			}
			if (NPCPoolRecovery.DeferSenses(npc, enabled))
			{
				return;
			}
			npc.enabled = enabled;
		}

		// Token: 0x06006A6E RID: 27246 RVA: 0x002AAC48 File Offset: 0x002A8E48
		private static void SetPOINavigatorEnabled(NPCSenses npc, bool enabled)
		{
			if (npc == null)
			{
				return;
			}
			if (NPCPoolRecovery.DeferNavigator(npc, enabled))
			{
				return;
			}
			SimplePOINavigator component = npc.GetComponent<SimplePOINavigator>();
			if (component != null)
			{
				component.enabled = enabled;
			}
		}

		// Token: 0x06006A6F RID: 27247 RVA: 0x002AAC80 File Offset: 0x002A8E80
		private void ForEachMember(Action<NPCSenses> action)
		{
			action(this._initiator);
			foreach (NPCSenses npcsenses in this._additionalActors)
			{
				action(npcsenses);
			}
		}

		// Token: 0x04006CFA RID: 27898
		[Header("Chase Behavior")]
		[Tooltip("Distance under which any patrol member catches the player, including the taser officer.")]
		public float catchDistance = 2.5f;

		// Token: 0x04006CFB RID: 27899
		[Tooltip("Run speed (m/s) used during Chasing for all members. Overrides FollowerEntity.maxSpeed.")]
		public float chasingRunSpeed = 2f;

		// Token: 0x04006CFC RID: 27900
		[Tooltip("How often (frames) the chase re-aims at the player's moving position.")]
		public int chasingReaimEveryFrames = 10;

		// Token: 0x04006CFD RID: 27901
		[Tooltip("Circle radius around the player where patrol members converge during chase.")]
		public float chaseFormationRadius = 1.2f;

		// Token: 0x04006CFE RID: 27902
		[Tooltip("Seconds the leader can lose line-of-sight before abandoning the chase. 0 = chase forever.")]
		public float chaseLoseSightTimeout = 5f;

		// Token: 0x04006CFF RID: 27903
		[Tooltip("If true, on chase escape the initiator enters NPCSenses cooldown so the patrol can't re-trigger immediately.")]
		public bool cooldownOnChaseEscape = true;

		// Token: 0x04006D00 RID: 27904
		[Header("Roaming Formation")]
		[Tooltip("Distance behind the leader for the first row of followers (V formation).")]
		public float roamingFollowDistance = 1.5f;

		// Token: 0x04006D01 RID: 27905
		[Tooltip("Lateral distance between followers in the V formation.")]
		public float roamingLateralSpacing = 1.2f;

		// Token: 0x04006D02 RID: 27906
		[Tooltip("Drift threshold (m) before re-issuing SetDestination on a follower (avoids per-frame path spam).")]
		public float roamingFormationUpdateThreshold = 0.4f;

		// Token: 0x04006D03 RID: 27907
		[Tooltip("Distance (m) under which a follower stops moving and faces the leader.")]
		public float roamingFaceLeaderDistance = 0.6f;

		// Token: 0x04006D04 RID: 27908
		[Tooltip("Rotation speed (deg/s) toward the leader once the follower reached its slot.")]
		public float roamingFaceLeaderSpeed = 360f;

		// Token: 0x04006D05 RID: 27909
		[Header("Capture (Director encounter)")]
		[Tooltip("EncounterProfileSO usato quando la patrol cattura il player (catch by distance). Tipicamente EncounterProfile_PoliceCapture (skip choice → emote Police). Se null, ripiega su GameOverManager.TriggerCapture (vecchio comportamento, reload scena).")]
		public EncounterProfileSO captureProfile;

		// Token: 0x04006D06 RID: 27910
		[Header("Post-Stun Wait Pose (mentre il player si rialza)")]
		[Tooltip("Valore di FaceExpressionIndex da settare sugli officer durante l'attesa post-stun (Reactions layer / facial blendshape). 2 = espressione 'attento/serio'.")]
		public int postStunFaceExpressionIndex = 2;

		// Token: 0x04006D07 RID: 27911
		[Tooltip("Valore di ReactionIdle da settare sull'animator degli officer durante l'attesa post-stun. 1 = idle reattivo (stesso che usa il Director in TriggerNPCIdleState).")]
		public int postStunReactionIdle = 1;

		// Token: 0x04006D08 RID: 27912
		[Tooltip("Velocita' di rotazione (deg/s) per orientare gli officer verso il player all'inizio dell'attesa.")]
		public float postStunFacePlayerSpeed = 540f;

		// Token: 0x04006D09 RID: 27913
		public Action<PolicePatrol> OnPatrolDisbanding;

		// Token: 0x04006D0A RID: 27914
		private NPCSenses _initiator;

		// Token: 0x04006D0B RID: 27915
		private List<NPCSenses> _additionalActors = new List<NPCSenses>();

		// Token: 0x04006D0C RID: 27916
		private TaserOfficer _taserMember;

		// Token: 0x04006D0D RID: 27917
		private readonly Dictionary<NPCSenses, float> _baselineMaxSpeed = new Dictionary<NPCSenses, float>();

		// Token: 0x04006D0E RID: 27918
		private float _lastChaseSightTime;

		// Token: 0x04006D0F RID: 27919
		private Coroutine _activeFireRoutine;

		// Token: 0x04006D10 RID: 27920
		private Coroutine _captureFormationRoutine;

		// Token: 0x04006D11 RID: 27921
		private bool _directorActive;

		// Token: 0x04006D12 RID: 27922
		private bool _directorHooked;

		// Token: 0x04006D13 RID: 27923
		private ExhibitionEventDirector _hookedDirector;

		// Token: 0x04006D14 RID: 27924
		private bool _resumeRoamingAfterDirector;

		// Token: 0x04006D15 RID: 27925
		private bool _simulationPauseActive;

		// Token: 0x04006D16 RID: 27926
		private float _simulationPauseStartedAt;

		// Token: 0x04006D17 RID: 27927
		[Header("Debug")]
		public bool debugLogs;

		// Token: 0x04006D18 RID: 27928
		private static readonly int PostStunReactionIdleHash = Animator.StringToHash("ReactionIdle");

		// Token: 0x04006D19 RID: 27929
		private static readonly int PostStunFaceExpressionHash = Animator.StringToHash("FaceExpressionIndex");

		// Token: 0x02000DF6 RID: 3574
		public enum PatrolState
		{
			// Token: 0x04006D1B RID: 27931
			Roaming,
			// Token: 0x04006D1C RID: 27932
			Chasing,
			// Token: 0x04006D1D RID: 27933
			Stopped
		}
	}
}
