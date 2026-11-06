using System;
using Pathfinding.Util;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Properties;
using Unity.Transforms;
using UnityEngine;

namespace Pathfinding.ECS
{
	// Token: 0x020002EC RID: 748
	public class AgentOffMeshLinkTraversalContext : ICloneable
	{
		// Token: 0x17000303 RID: 771
		// (get) Token: 0x060012BC RID: 4796 RVA: 0x00072C84 File Offset: 0x00070E84
		public virtual GameObject gameObject
		{
			get
			{
				if (this.gameObjectCache == null)
				{
					FollowerEntity followerEntity = BatchedEvents.Find<FollowerEntity, Entity>(this.entity, (FollowerEntity follower, Entity entity) => follower.entity == entity);
					if (followerEntity != null)
					{
						this.gameObjectCache = followerEntity.gameObject;
					}
				}
				return this.gameObjectCache;
			}
		}

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x060012BD RID: 4797 RVA: 0x00072CE5 File Offset: 0x00070EE5
		public unsafe ref LocalTransform transform
		{
			get
			{
				return ref *this.transformPtr;
			}
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x060012BE RID: 4798 RVA: 0x00072CED File Offset: 0x00070EED
		public unsafe ref MovementSettings movementSettings
		{
			get
			{
				return ref *this.movementSettingsPtr;
			}
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x060012BF RID: 4799 RVA: 0x00072CF5 File Offset: 0x00070EF5
		public unsafe ref MovementControl movementControl
		{
			get
			{
				return ref *this.movementControlPtr;
			}
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x060012C0 RID: 4800 RVA: 0x00072CFD File Offset: 0x00070EFD
		public unsafe OffMeshLinks.OffMeshLinkTracer link
		{
			get
			{
				return new OffMeshLinks.OffMeshLinkTracer(this.concreteLink, this.linkInfoPtr->relativeStart, this.linkInfoPtr->relativeEnd, this.linkInfoPtr->isReverse);
			}
		}

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x060012C1 RID: 4801 RVA: 0x00072D35 File Offset: 0x00070F35
		[Obsolete("Use the link property instead")]
		public unsafe AgentOffMeshLinkTraversal linkInfo
		{
			get
			{
				return *this.linkInfoPtr;
			}
		}

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x060012C2 RID: 4802 RVA: 0x00072D42 File Offset: 0x00070F42
		public unsafe ref NativeMovementPlane movementPlane
		{
			get
			{
				return ref this.movementPlanePtr->value;
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x060012C3 RID: 4803 RVA: 0x00072D4F File Offset: 0x00070F4F
		// (set) Token: 0x060012C4 RID: 4804 RVA: 0x00072D6E File Offset: 0x00070F6E
		public bool enableBuiltInMovement
		{
			get
			{
				return !this.movementDisabled.IsValid || !this.movementDisabled.ValueRW;
			}
			set
			{
				if (this.movementDisabled.IsValid)
				{
					this.movementDisabled.ValueRW = !value;
				}
			}
		}

		// Token: 0x060012C5 RID: 4805 RVA: 0x00072D8C File Offset: 0x00070F8C
		public AgentOffMeshLinkTraversalContext(OffMeshLinks.OffMeshLinkConcrete link)
		{
			this.concreteLink = link;
		}

		// Token: 0x060012C6 RID: 4806 RVA: 0x00072DA8 File Offset: 0x00070FA8
		public unsafe virtual void SetInternalData(Entity entity, ref LocalTransform transform, ref AgentMovementPlane movementPlane, ref MovementControl movementControl, ref MovementSettings movementSettings, ref AgentOffMeshLinkTraversal linkInfo, EnabledRefRW<AgentOffMeshLinkMovementDisabled> movementDisabled, EnabledRefRW<AgentOffMeshLinkLocalAvoidanceDisabled> localAvoidanceDisabled, ManagedState state, float deltaTime)
		{
			this.linkInfoPtr = (AgentOffMeshLinkTraversal*)UnsafeUtility.AddressOf<AgentOffMeshLinkTraversal>(ref linkInfo);
			this.movementControlPtr = (MovementControl*)UnsafeUtility.AddressOf<MovementControl>(ref movementControl);
			this.movementSettingsPtr = (MovementSettings*)UnsafeUtility.AddressOf<MovementSettings>(ref movementSettings);
			this.transformPtr = (LocalTransform*)UnsafeUtility.AddressOf<LocalTransform>(ref transform);
			this.movementPlanePtr = (AgentMovementPlane*)UnsafeUtility.AddressOf<AgentMovementPlane>(ref movementPlane);
			this.movementDisabled = movementDisabled;
			this.localAvoidanceDisabled = localAvoidanceDisabled;
			this.managedState = state;
			this.deltaTime = deltaTime;
			this.entity = entity;
		}

		// Token: 0x060012C7 RID: 4807 RVA: 0x00072E1B File Offset: 0x0007101B
		public void DisableLocalAvoidance()
		{
			if (this.localAvoidanceDisabled.IsValid)
			{
				this.localAvoidanceDisabled.ValueRW = true;
			}
		}

		// Token: 0x060012C8 RID: 4808 RVA: 0x00072E38 File Offset: 0x00071038
		public void DisableRotationSmoothing()
		{
			if (float.IsNaN(this.backupRotationSmoothing) && this.movementSettings.rotationSmoothing > 0f)
			{
				this.backupRotationSmoothing = this.movementSettings.rotationSmoothing;
				this.movementSettings.rotationSmoothing = 0f;
			}
		}

		// Token: 0x060012C9 RID: 4809 RVA: 0x00072E85 File Offset: 0x00071085
		public virtual void Restore()
		{
			if (!float.IsNaN(this.backupRotationSmoothing))
			{
				this.movementSettings.rotationSmoothing = this.backupRotationSmoothing;
				this.backupRotationSmoothing = float.NaN;
			}
		}

		// Token: 0x060012CA RID: 4810 RVA: 0x00072EB0 File Offset: 0x000710B0
		public virtual void Teleport(float3 position)
		{
			this.transform.Position = position;
		}

		// Token: 0x060012CB RID: 4811 RVA: 0x00072EBE File Offset: 0x000710BE
		public virtual void Abort(bool teleportToStart = true)
		{
			if (teleportToStart)
			{
				this.Teleport(this.link.relativeStart);
			}
			this.managedState.pathTracer.RemoveAllButFirstNode();
			throw new AgentOffMeshLinkTraversalContext.AbortOffMeshLinkTraversal();
		}

		// Token: 0x060012CC RID: 4812 RVA: 0x00072EF0 File Offset: 0x000710F0
		public unsafe virtual MovementTarget MoveTowards(float3 position, quaternion rotation, bool gravity, bool slowdown)
		{
			this.DisableRotationSmoothing();
			this.enableBuiltInMovement = true;
			float num = math.length(this.movementPlane.ToPlane(position - this.transform.Position));
			float num2 = this.movementSettings.follower.Speed(slowdown ? num : float.PositiveInfinity);
			float num3 = this.movementSettings.follower.Accelerate(this.movementControl.speed, this.movementSettings.follower.slowdownTime, this.deltaTime);
			num3 = math.min(num3, num2);
			float num4 = this.movementPlane.ToPlane(rotation);
			float num5 = Mathf.Abs(AstarMath.DeltaAngle(this.movementPlane.ToPlane(this.transform.Rotation), num4));
			*this.movementControl = new MovementControl
			{
				targetPoint = position,
				endOfPath = position,
				speed = num3,
				maxSpeed = num3 * 1.1f,
				hierarchicalNodeIndex = -1,
				overrideLocalAvoidance = true,
				targetRotation = num4,
				targetRotationHint = num4,
				targetRotationOffset = 0f,
				rotationSpeed = math.radians(this.movementSettings.follower.rotationSpeed)
			};
			return new MovementTarget
			{
				isReached = (num <= (slowdown ? 0.01f : (num3 * 0.033333335f)) && num5 < math.radians(1f))
			};
		}

		// Token: 0x060012CD RID: 4813 RVA: 0x00073070 File Offset: 0x00071270
		public virtual object Clone()
		{
			AgentOffMeshLinkTraversalContext agentOffMeshLinkTraversalContext = (AgentOffMeshLinkTraversalContext)base.MemberwiseClone();
			agentOffMeshLinkTraversalContext.entity = Entity.Null;
			agentOffMeshLinkTraversalContext.gameObjectCache = null;
			agentOffMeshLinkTraversalContext.managedState = null;
			agentOffMeshLinkTraversalContext.movementDisabled = default(EnabledRefRW<AgentOffMeshLinkMovementDisabled>);
			agentOffMeshLinkTraversalContext.localAvoidanceDisabled = default(EnabledRefRW<AgentOffMeshLinkLocalAvoidanceDisabled>);
			agentOffMeshLinkTraversalContext.linkInfoPtr = null;
			agentOffMeshLinkTraversalContext.movementControlPtr = null;
			agentOffMeshLinkTraversalContext.movementSettingsPtr = null;
			agentOffMeshLinkTraversalContext.transformPtr = null;
			agentOffMeshLinkTraversalContext.movementPlanePtr = null;
			return agentOffMeshLinkTraversalContext;
		}

		// Token: 0x04000DAB RID: 3499
		internal unsafe AgentOffMeshLinkTraversal* linkInfoPtr;

		// Token: 0x04000DAC RID: 3500
		internal unsafe MovementControl* movementControlPtr;

		// Token: 0x04000DAD RID: 3501
		internal unsafe MovementSettings* movementSettingsPtr;

		// Token: 0x04000DAE RID: 3502
		internal unsafe LocalTransform* transformPtr;

		// Token: 0x04000DAF RID: 3503
		internal unsafe AgentMovementPlane* movementPlanePtr;

		// Token: 0x04000DB0 RID: 3504
		internal EnabledRefRW<AgentOffMeshLinkMovementDisabled> movementDisabled;

		// Token: 0x04000DB1 RID: 3505
		internal EnabledRefRW<AgentOffMeshLinkLocalAvoidanceDisabled> localAvoidanceDisabled;

		// Token: 0x04000DB2 RID: 3506
		public Entity entity;

		// Token: 0x04000DB3 RID: 3507
		[DontCreateProperty]
		public ManagedState managedState;

		// Token: 0x04000DB4 RID: 3508
		[DontCreateProperty]
		internal OffMeshLinks.OffMeshLinkConcrete concreteLink;

		// Token: 0x04000DB5 RID: 3509
		protected float backupRotationSmoothing = float.NaN;

		// Token: 0x04000DB6 RID: 3510
		public float deltaTime;

		// Token: 0x04000DB7 RID: 3511
		protected GameObject gameObjectCache;

		// Token: 0x020002ED RID: 749
		public class AbortOffMeshLinkTraversal : Exception
		{
		}
	}
}
