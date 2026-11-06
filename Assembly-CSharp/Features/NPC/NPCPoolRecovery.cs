using System;
using Features.Detection;
using Features.NPC.StateMachine;
using Features.Police;
using Features.RenderingPrecision;
using Pathfinding;
using PoolWaterTools;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Features.NPC
{
	// Token: 0x02000EDB RID: 3803
	[DefaultExecutionOrder(500)]
	[DisallowMultipleComponent]
	public sealed class NPCPoolRecovery : MonoBehaviour
	{
		// Token: 0x17001297 RID: 4759
		// (get) Token: 0x060071CD RID: 29133 RVA: 0x002E3BDB File Offset: 0x002E1DDB
		// (set) Token: 0x060071CE RID: 29134 RVA: 0x002E3BE3 File Offset: 0x002E1DE3
		public NPCPoolRecovery.RecoveryState State { get; private set; }

		// Token: 0x17001298 RID: 4760
		// (get) Token: 0x060071CF RID: 29135 RVA: 0x002E3BEC File Offset: 0x002E1DEC
		public bool IsRecovering
		{
			get
			{
				return this.State > NPCPoolRecovery.RecoveryState.Inactive;
			}
		}

		// Token: 0x060071D0 RID: 29136 RVA: 0x002E3BF8 File Offset: 0x002E1DF8
		public void Configure(AnimationClip swim, AnimationClip exit)
		{
			this.swimClip = swim;
			this.climbClip = exit;
			this.controller = base.GetComponent<AStarNPCController>();
			this.follower = base.GetComponent<FollowerEntity>();
			this.animator = ((this.controller != null && this.controller.animator != null) ? this.controller.animator : base.GetComponentInChildren<Animator>(true));
			this.ragdoll = base.GetComponent<NPCRagdollRecovery>();
			this.diver = base.GetComponent<NPCDiveController>();
			this.wander = base.GetComponent<NPCWanderController>();
			this.senses = base.GetComponent<NPCSenses>();
			this.stateMachine = base.GetComponent<NPCStateMachine>();
			this.navigator = base.GetComponent<SimplePOINavigator>();
			this.lookAt = base.GetComponent<NPCSimpleLookAt>();
		}

		// Token: 0x060071D1 RID: 29137 RVA: 0x002E3CBA File Offset: 0x002E1EBA
		private void OnEnable()
		{
			WorldOrigin.Shifted += this.OnOriginShifted;
		}

		// Token: 0x060071D2 RID: 29138 RVA: 0x002E3CCD File Offset: 0x002E1ECD
		private void OnDisable()
		{
			WorldOrigin.Shifted -= this.OnOriginShifted;
			this.ReleaseControl(false);
			this.ragdollWaterTime = 0f;
			this.cachedDestinationValid = false;
			this.hasCachedDestination = false;
			this.cachedDestinationWater = null;
		}

		// Token: 0x060071D3 RID: 29139 RVA: 0x002E3D08 File Offset: 0x002E1F08
		private void Update()
		{
			if (NPCSimulationPause.IsPaused)
			{
				return;
			}
			float simulationDeltaTime = NPCSimulationPause.SimulationDeltaTime;
			if (this.IsRecovering)
			{
				if (this.senses != null)
				{
					this.senses.enabled = false;
				}
				if (this.water == null)
				{
					this.ReleaseControl(true);
					return;
				}
				this.TickRecovery(simulationDeltaTime);
				return;
			}
			else
			{
				if (TaserOfficer.MustAbortAttack)
				{
					return;
				}
				if (this.controller == null || this.animator == null || this.swimClip == null || this.climbClip == null)
				{
					return;
				}
				if (this.ragdoll == null)
				{
					this.ragdoll = base.GetComponent<NPCRagdollRecovery>();
				}
				if (this.diver == null)
				{
					this.diver = base.GetComponent<NPCDiveController>();
				}
				if (this.ragdoll != null && this.ragdoll.IsRecovering)
				{
					Collider collider;
					if (!this.ragdoll.IsPhysicallyRagdolled || !PoolWaterRecovery.TryFindWater(this.ragdoll.PhysicalPosition, out collider, 0f, false))
					{
						this.ragdollWaterTime = 0f;
						return;
					}
					this.ragdollWaterTime += simulationDeltaTime;
					if (this.ragdollWaterTime < 0.65f)
					{
						return;
					}
					if (Time.time < this.nextCheck)
					{
						return;
					}
					this.nextCheck = Time.time + 0.5f;
					Vector3 vector = this.ragdoll.PhysicalPosition - Vector3.up * 0.9f;
					if (!this.TryPlan(vector))
					{
						return;
					}
					this.ragdoll.CompleteForWaterRecovery(vector);
					this.BeginRecovery();
					return;
				}
				else
				{
					this.ragdollWaterTime = 0f;
					if (this.diver != null && this.diver.IsBusy)
					{
						return;
					}
					if (Time.time < this.nextCheck)
					{
						return;
					}
					this.nextCheck = Time.time + 0.5f;
					if (this.TryPlan(base.transform.position))
					{
						this.BeginRecovery();
					}
					return;
				}
			}
		}

		// Token: 0x060071D4 RID: 29140 RVA: 0x002E3F03 File Offset: 0x002E2103
		private void LateUpdate()
		{
			if (!this.IsRecovering || !this.graph.IsValid() || NPCSimulationPause.IsPaused)
			{
				return;
			}
			this.graph.Evaluate(0f);
		}

		// Token: 0x060071D5 RID: 29141 RVA: 0x002E3F34 File Offset: 0x002E2134
		private bool TryPlan(Vector3 root)
		{
			Collider collider;
			if (!PoolWaterRecovery.TryFindWater(root, out collider, 0f, false))
			{
				return false;
			}
			float num = PoolWaterRecovery.SurfaceY(collider);
			if (root.y > num - 0.25f)
			{
				return false;
			}
			PoolClimbPoint poolClimbPoint;
			Vector3 vector;
			bool flag = PoolWaterRecovery.TryFindClimbExit(collider, root, base.transform, out poolClimbPoint, out vector);
			if (!flag && !PoolWaterRecovery.TryFindDryDeck(collider, root, base.transform, out vector, 0.8f))
			{
				return false;
			}
			if (!NPCPoolRecovery.TryGetDeckNode(collider, vector, out vector))
			{
				return false;
			}
			this.water = collider;
			this.climb = (flag ? poolClimbPoint : null);
			this.deck = vector;
			float num2 = 1.8f;
			CapsuleCollider component = base.GetComponent<CapsuleCollider>();
			if (component != null)
			{
				num2 = component.height * Mathf.Abs(base.transform.lossyScale.y);
			}
			this.surfaceRootY = num - (num2 * 0.5f + 0.55f);
			if (this.climb != null)
			{
				this.approach = this.climb.SwimApproachPosition;
			}
			else
			{
				Vector3 vector2 = collider.ClosestPoint(new Vector3(this.deck.x, num - 0.1f, this.deck.z));
				Vector3 vector3 = collider.bounds.center - vector2;
				vector3.y = 0f;
				this.approach = vector2 + vector3.normalized * 0.55f;
			}
			this.approach.y = this.surfaceRootY;
			return true;
		}

		// Token: 0x060071D6 RID: 29142 RVA: 0x002E40B0 File Offset: 0x002E22B0
		private void BeginRecovery()
		{
			if (TaserOfficer.MustAbortAttack)
			{
				return;
			}
			bool flag = this.controller.IsManualMode || (this.diver != null && this.diver.IsBusy);
			if (this.diver != null && this.diver.IsBusy)
			{
				this.diver.CancelForWaterRecovery();
			}
			NPCWanderController npcwanderController = this.wander;
			if (npcwanderController != null)
			{
				npcwanderController.PrepareForExternalAnimationSnapshot();
			}
			this.followerEnabled = this.follower != null && (this.follower.enabled || flag);
			this.followerStopped = this.follower != null && this.follower.entityExists && this.follower.isStopped;
			this.controllerEnabled = this.controller.enabled;
			this.navigationLocked = this.controller.IsNavigationLocked;
			this.navigationLockedByExternalActivity = this.navigationLocked && this.wander != null && this.wander.ExternalControl;
			this.suppressRootMotion = this.controller.SuppressRootMotionPosition;
			this.sensesEnabled = this.senses != null && this.senses.enabled;
			this.stateMachineEnabled = this.stateMachine != null && this.stateMachine.enabled;
			this.navigatorEnabled = this.navigator != null && this.navigator.enabled;
			this.lookAtEnabled = this.lookAt != null && this.lookAt.enabled;
			this.applyRootMotion = this.animator.applyRootMotion;
			this.culling = this.animator.cullingMode;
			this.State = NPCPoolRecovery.RecoveryState.Rising;
			this.phaseTime = 0f;
			if (this.wander != null)
			{
				this.wander.SuspendForRecovery(this);
			}
			if (this.stateMachine != null)
			{
				this.stateMachine.enabled = false;
			}
			if (this.navigator != null)
			{
				this.navigator.enabled = false;
			}
			if (this.lookAt != null)
			{
				this.lookAt.enabled = false;
			}
			if (this.senses != null)
			{
				this.senses.CurrentAttention = 0f;
				this.senses.IsDetected = (this.senses.IsAlert = (this.senses.HasLineOfSight = (this.senses.HasAudioContact = false)));
				this.senses.enabled = false;
			}
			TaserOfficer component = base.GetComponent<TaserOfficer>();
			if (component != null)
			{
				component.CancelFire();
			}
			this.controller.StopAndLock();
			this.controller.CancelManualModeForWaterRecovery();
			this.controller.SuppressRootMotionPosition = true;
			this.controller.enabled = false;
			if (this.follower != null)
			{
				this.follower.enabled = false;
			}
			this.animator.applyRootMotion = false;
			this.animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
			this.PlayClip(this.swimClip);
		}

		// Token: 0x060071D7 RID: 29143 RVA: 0x002E43E0 File Offset: 0x002E25E0
		private void PlayClip(AnimationClip clip)
		{
			if (this.graph.IsValid())
			{
				this.graph.Destroy();
			}
			this.clipPlayable = AnimationPlayableUtilities.PlayClip(this.animator, clip, out this.graph);
			this.graph.SetTimeUpdateMode(DirectorUpdateMode.Manual);
			this.clipPlayable.SetSpeed(0.0);
			this.clipPlayable.SetTime(0.0);
			this.phaseTime = 0f;
		}

		// Token: 0x060071D8 RID: 29144 RVA: 0x002E445C File Offset: 0x002E265C
		private void TickRecovery(float dt)
		{
			this.phaseTime += dt;
			if (this.clipPlayable.IsValid<AnimationClipPlayable>())
			{
				float num = ((this.State == NPCPoolRecovery.RecoveryState.Climbing) ? Mathf.Min(this.phaseTime * 0.65f, this.climbClip.length) : Mathf.Repeat(this.phaseTime, Mathf.Max(0.01f, this.swimClip.length)));
				this.clipPlayable.SetTime((double)num);
			}
			switch (this.State)
			{
			case NPCPoolRecovery.RecoveryState.Rising:
			{
				Vector3 position = base.transform.position;
				position.y = Mathf.MoveTowards(position.y, this.surfaceRootY, 2f * dt);
				base.transform.position = position;
				this.Face(this.approach, dt);
				if (Mathf.Abs(position.y - this.surfaceRootY) < 0.02f)
				{
					this.State = NPCPoolRecovery.RecoveryState.Swimming;
					return;
				}
				break;
			}
			case NPCPoolRecovery.RecoveryState.Swimming:
				this.Face(this.approach, dt);
				base.transform.position = Vector3.MoveTowards(base.transform.position, this.approach, 1.35f * dt);
				if ((base.transform.position - this.approach).sqrMagnitude < 0.025f)
				{
					this.climbStart = base.transform.position;
					this.State = NPCPoolRecovery.RecoveryState.Climbing;
					this.PlayClip(this.climbClip);
					return;
				}
				break;
			case NPCPoolRecovery.RecoveryState.Climbing:
			{
				this.Face(this.deck, dt);
				float num2 = Mathf.Clamp01(this.phaseTime * 0.65f / Mathf.Max(0.1f, this.climbClip.length));
				Vector3 vector = Vector3.Lerp(this.climbStart, this.deck, Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(0.2f, 1f, num2)));
				vector.y = Mathf.Lerp(this.climbStart.y, this.deck.y, Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(num2 / 0.65f)));
				base.transform.position = vector;
				if (num2 >= 1f)
				{
					this.ReleaseControl(true);
				}
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x060071D9 RID: 29145 RVA: 0x002E46A0 File Offset: 0x002E28A0
		private void Face(Vector3 destination, float dt)
		{
			Vector3 vector = destination - base.transform.position;
			vector.y = 0f;
			if (vector.sqrMagnitude > 0.001f)
			{
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.LookRotation(vector), 240f * dt);
			}
		}

		// Token: 0x060071DA RID: 29146 RVA: 0x002E4704 File Offset: 0x002E2904
		private void ReleaseControl(bool resume)
		{
			if (!this.IsRecovering)
			{
				return;
			}
			if (this.graph.IsValid())
			{
				this.graph.Destroy();
			}
			this.State = NPCPoolRecovery.RecoveryState.Inactive;
			if (this.animator != null)
			{
				this.animator.applyRootMotion = this.applyRootMotion;
				this.animator.cullingMode = this.culling;
			}
			if (this.controller != null)
			{
				this.controller.SuppressRootMotionPosition = this.suppressRootMotion;
				this.controller.enabled = this.controllerEnabled;
				bool flag = this.navigationLockedByExternalActivity && this.wander != null && !this.wander.ExternalControl;
				if (this.navigationLocked && !flag)
				{
					this.controller.StopAndLock();
				}
				else
				{
					this.controller.UnlockNavigation(true);
				}
			}
			if (this.follower != null)
			{
				this.follower.enabled = this.followerEnabled;
				if (resume && this.followerEnabled)
				{
					this.follower.Teleport(base.transform.position, true);
					this.follower.SetDestination(base.transform.position, default(float3));
				}
				if (this.follower.entityExists)
				{
					this.follower.isStopped = this.followerStopped;
				}
			}
			if (this.navigator != null)
			{
				this.navigator.enabled = this.navigatorEnabled;
			}
			if (this.stateMachine != null)
			{
				this.stateMachine.enabled = this.stateMachineEnabled;
			}
			if (this.lookAt != null)
			{
				this.lookAt.enabled = this.lookAtEnabled;
			}
			if (this.senses != null)
			{
				this.senses.enabled = this.sensesEnabled;
			}
			if (this.wander != null)
			{
				this.wander.ResumeAfterRecovery(this, true);
			}
			this.nextCheck = Time.time + 0.5f;
			this.water = null;
		}

		// Token: 0x060071DB RID: 29147 RVA: 0x002E4918 File Offset: 0x002E2B18
		private void OnOriginShifted(Vector3 delta)
		{
			this.hasCachedDestination = false;
			this.lastWetDestination += delta;
			this.cachedDryDestination += delta;
			if (!this.IsRecovering)
			{
				return;
			}
			this.deck += delta;
			this.approach += delta;
			this.climbStart += delta;
			this.surfaceRootY += delta.y;
		}

		// Token: 0x060071DC RID: 29148 RVA: 0x002E49A4 File Offset: 0x002E2BA4
		public static bool IsActorRecovering(Component actor)
		{
			NPCPoolRecovery npcpoolRecovery = ((actor != null) ? actor.GetComponent<NPCPoolRecovery>() : null);
			return npcpoolRecovery != null && npcpoolRecovery.IsRecovering;
		}

		// Token: 0x060071DD RID: 29149 RVA: 0x002E49D8 File Offset: 0x002E2BD8
		public static bool IsUnavailableForEncounter(Component actor)
		{
			if (actor == null)
			{
				return false;
			}
			AStarNPCController componentInParent = actor.GetComponentInParent<AStarNPCController>(true);
			Collider collider;
			return !(componentInParent == null) && (NPCPoolRecovery.IsActorRecovering(componentInParent) || (PoolNpcConfigurator.IsActive && PoolWaterRecovery.TryFindWater(componentInParent.transform.position, out collider, 0f, false) && componentInParent.transform.position.y <= PoolWaterRecovery.SurfaceY(collider) - 0.25f));
		}

		// Token: 0x060071DE RID: 29150 RVA: 0x002E4A54 File Offset: 0x002E2C54
		public static bool DeferSenses(Component actor, bool enabled)
		{
			NPCPoolRecovery npcpoolRecovery = ((actor != null) ? actor.GetComponent<NPCPoolRecovery>() : null);
			if (npcpoolRecovery == null || !npcpoolRecovery.IsRecovering)
			{
				return false;
			}
			npcpoolRecovery.sensesEnabled = enabled;
			return true;
		}

		// Token: 0x060071DF RID: 29151 RVA: 0x002E4A90 File Offset: 0x002E2C90
		public static bool DeferNavigator(Component actor, bool enabled)
		{
			NPCPoolRecovery npcpoolRecovery = ((actor != null) ? actor.GetComponent<NPCPoolRecovery>() : null);
			if (npcpoolRecovery == null || !npcpoolRecovery.IsRecovering)
			{
				return false;
			}
			npcpoolRecovery.navigatorEnabled = enabled;
			return true;
		}

		// Token: 0x060071E0 RID: 29152 RVA: 0x002E4ACC File Offset: 0x002E2CCC
		public static bool TryConstrainDestination(Transform actor, ref Vector3 destination)
		{
			if (!PoolNpcConfigurator.IsActive || actor == null)
			{
				return true;
			}
			if (NPCPoolRecovery.IsActorRecovering(actor))
			{
				return false;
			}
			Collider collider;
			if (!PoolWaterRecovery.TryFindWater(destination, out collider, 0.35f, false))
			{
				return true;
			}
			NPCPoolRecovery component = actor.GetComponent<NPCPoolRecovery>();
			if (component != null && component.hasCachedDestination && component.cachedDestinationWater == collider && Time.time < component.nextDestinationCheck && (destination - component.lastWetDestination).sqrMagnitude < 1f)
			{
				if (!component.cachedDestinationValid)
				{
					return false;
				}
				destination = component.cachedDryDestination;
				return true;
			}
			else
			{
				if (component != null)
				{
					component.hasCachedDestination = true;
					component.cachedDestinationValid = false;
					component.cachedDestinationWater = collider;
					component.lastWetDestination = destination;
					component.nextDestinationCheck = Time.time + 0.5f;
				}
				Vector3 vector;
				if (!PoolWaterRecovery.TryFindDryDeck(collider, destination, actor, out vector, 0.8f) || !NPCPoolRecovery.TryGetDeckNode(collider, vector, out vector))
				{
					return false;
				}
				NNInfo nearest = AstarPath.active.GetNearest(actor.position, NNConstraint.Walkable);
				NNInfo nearest2 = AstarPath.active.GetNearest(vector, NNConstraint.Walkable);
				if (nearest.node == null || !PathUtilities.IsPathPossible(nearest.node, nearest2.node))
				{
					return false;
				}
				if (component != null)
				{
					component.cachedDryDestination = vector;
					component.cachedDestinationValid = true;
				}
				destination = vector;
				return true;
			}
		}

		// Token: 0x060071E1 RID: 29153 RVA: 0x002E4C3C File Offset: 0x002E2E3C
		private static bool TryGetDeckNode(Collider waterVolume, Vector3 requested, out Vector3 grounded)
		{
			grounded = requested;
			if (AstarPath.active == null)
			{
				return false;
			}
			NNInfo nearest = AstarPath.active.GetNearest(requested, NNConstraint.Walkable);
			if (nearest.node == null || !nearest.node.Walkable || Vector3.Distance(nearest.position, requested) > 1f || Mathf.Abs(nearest.position.y - requested.y) > 0.4f)
			{
				return false;
			}
			if (PoolWaterRecovery.ContainsHorizontal(waterVolume, nearest.position, 0f))
			{
				return false;
			}
			grounded = nearest.position;
			return true;
		}

		// Token: 0x040074B5 RID: 29877
		private AnimationClip swimClip;

		// Token: 0x040074B6 RID: 29878
		private AnimationClip climbClip;

		// Token: 0x040074B7 RID: 29879
		private AStarNPCController controller;

		// Token: 0x040074B8 RID: 29880
		private FollowerEntity follower;

		// Token: 0x040074B9 RID: 29881
		private Animator animator;

		// Token: 0x040074BA RID: 29882
		private NPCRagdollRecovery ragdoll;

		// Token: 0x040074BB RID: 29883
		private NPCDiveController diver;

		// Token: 0x040074BC RID: 29884
		private NPCWanderController wander;

		// Token: 0x040074BD RID: 29885
		private NPCSenses senses;

		// Token: 0x040074BE RID: 29886
		private NPCStateMachine stateMachine;

		// Token: 0x040074BF RID: 29887
		private SimplePOINavigator navigator;

		// Token: 0x040074C0 RID: 29888
		private NPCSimpleLookAt lookAt;

		// Token: 0x040074C1 RID: 29889
		private Collider water;

		// Token: 0x040074C2 RID: 29890
		private PoolClimbPoint climb;

		// Token: 0x040074C3 RID: 29891
		private Vector3 deck;

		// Token: 0x040074C4 RID: 29892
		private Vector3 approach;

		// Token: 0x040074C5 RID: 29893
		private Vector3 climbStart;

		// Token: 0x040074C6 RID: 29894
		private float surfaceRootY;

		// Token: 0x040074C7 RID: 29895
		private float phaseTime;

		// Token: 0x040074C8 RID: 29896
		private float ragdollWaterTime;

		// Token: 0x040074C9 RID: 29897
		private float nextCheck;

		// Token: 0x040074CA RID: 29898
		private float nextDestinationCheck;

		// Token: 0x040074CB RID: 29899
		private Vector3 lastWetDestination;

		// Token: 0x040074CC RID: 29900
		private Vector3 cachedDryDestination;

		// Token: 0x040074CD RID: 29901
		private bool hasCachedDestination;

		// Token: 0x040074CE RID: 29902
		private bool cachedDestinationValid;

		// Token: 0x040074CF RID: 29903
		private Collider cachedDestinationWater;

		// Token: 0x040074D0 RID: 29904
		private bool followerEnabled;

		// Token: 0x040074D1 RID: 29905
		private bool followerStopped;

		// Token: 0x040074D2 RID: 29906
		private bool controllerEnabled;

		// Token: 0x040074D3 RID: 29907
		private bool navigationLocked;

		// Token: 0x040074D4 RID: 29908
		private bool navigationLockedByExternalActivity;

		// Token: 0x040074D5 RID: 29909
		private bool sensesEnabled;

		// Token: 0x040074D6 RID: 29910
		private bool stateMachineEnabled;

		// Token: 0x040074D7 RID: 29911
		private bool navigatorEnabled;

		// Token: 0x040074D8 RID: 29912
		private bool lookAtEnabled;

		// Token: 0x040074D9 RID: 29913
		private bool applyRootMotion;

		// Token: 0x040074DA RID: 29914
		private bool suppressRootMotion;

		// Token: 0x040074DB RID: 29915
		private AnimatorCullingMode culling;

		// Token: 0x040074DC RID: 29916
		private PlayableGraph graph;

		// Token: 0x040074DD RID: 29917
		private AnimationClipPlayable clipPlayable;

		// Token: 0x02000EDC RID: 3804
		public enum RecoveryState
		{
			// Token: 0x040074DF RID: 29919
			Inactive,
			// Token: 0x040074E0 RID: 29920
			Rising,
			// Token: 0x040074E1 RID: 29921
			Swimming,
			// Token: 0x040074E2 RID: 29922
			Climbing
		}
	}
}
