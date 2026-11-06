using System;
using System.Runtime.InteropServices;
using Pathfinding.Collections;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Profiling;
using Unity.Transforms;

namespace Pathfinding.ECS
{
	// Token: 0x02000349 RID: 841
	public struct JobRepairPath : IJobChunk
	{
		// Token: 0x0600151A RID: 5402 RVA: 0x00079040 File Offset: 0x00077240
		public unsafe void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			if (!this.indicesScratch.IsCreated)
			{
				this.nextCornersScratch = new NativeList<float3>(Allocator.Temp);
				this.indicesScratch = new NativeArray<int>(8, Allocator.Temp, NativeArrayOptions.ClearMemory);
			}
			LocalTransform* unsafeReadOnlyPtr = (LocalTransform*)chunk.GetNativeArray<LocalTransform>(ref this.scheduler.LocalTransformTypeHandleRO).GetUnsafeReadOnlyPtr<LocalTransform>();
			MovementState* unsafePtr = (MovementState*)chunk.GetNativeArray<MovementState>(ref this.scheduler.MovementStateTypeHandleRW).GetUnsafePtr<MovementState>();
			AgentCylinderShape* unsafeReadOnlyPtr2 = (AgentCylinderShape*)chunk.GetNativeArray<AgentCylinderShape>(ref this.scheduler.AgentCylinderShapeTypeHandleRO).GetUnsafeReadOnlyPtr<AgentCylinderShape>();
			AutoRepathPolicy* unsafePtr2 = (AutoRepathPolicy*)chunk.GetNativeArray<AutoRepathPolicy>(ref this.scheduler.AutoRepathPolicyRW).GetUnsafePtr<AutoRepathPolicy>();
			DestinationPoint* unsafeReadOnlyPtr3 = (DestinationPoint*)chunk.GetNativeArray<DestinationPoint>(ref this.scheduler.DestinationPointTypeHandleRO).GetUnsafeReadOnlyPtr<DestinationPoint>();
			MovementSettings* unsafeReadOnlyPtr4 = (MovementSettings*)chunk.GetNativeArray<MovementSettings>(ref this.scheduler.MovementSettingsTypeHandleRO).GetUnsafeReadOnlyPtr<MovementSettings>();
			AgentMovementPlane* unsafeReadOnlyPtr5 = (AgentMovementPlane*)chunk.GetNativeArray<AgentMovementPlane>(ref this.scheduler.AgentMovementPlaneTypeHandleRO).GetUnsafeReadOnlyPtr<AgentMovementPlane>();
			EnabledMask enabledMask = chunk.GetEnabledMask<ReadyToTraverseOffMeshLink>(ref this.scheduler.ReadyToTraverseOffMeshLinkTypeHandleRW);
			ManagedComponentAccessor<ManagedState> managedComponentAccessor = chunk.GetManagedComponentAccessor<ManagedState>(ref this.scheduler.ManagedStateTypeHandleRW, (EntityManager)this.scheduler.entityManagerHandle.Target);
			for (int i = 0; i < chunk.Count; i++)
			{
				JobRepairPath.Execute(ref unsafeReadOnlyPtr[i], ref unsafePtr[i], ref unsafeReadOnlyPtr2[i], ref unsafeReadOnlyPtr5[i], ref unsafePtr2[i], ref unsafeReadOnlyPtr3[i], enabledMask.GetEnabledRefRW<ReadyToTraverseOffMeshLink>(i), managedComponentAccessor[i], in unsafeReadOnlyPtr4[i], this.nextCornersScratch, ref this.indicesScratch, Allocator.Temp, this.onlyApplyPendingPaths);
			}
		}

		// Token: 0x0600151B RID: 5403 RVA: 0x000791F4 File Offset: 0x000773F4
		public static void Execute(ref LocalTransform transform, ref MovementState state, ref AgentCylinderShape shape, ref AgentMovementPlane movementPlane, ref AutoRepathPolicy autoRepathPolicy, ref DestinationPoint destination, EnabledRefRW<ReadyToTraverseOffMeshLink> readyToTraverseOffMeshLink, ManagedState managedState, in MovementSettings settings, NativeList<float3> nextCornersScratch, ref NativeArray<int> indicesScratch, Allocator allocator, bool onlyApplyPendingPaths)
		{
			if (onlyApplyPendingPaths)
			{
				if (managedState.pendingPath == null || !managedState.pendingPath.IsDone())
				{
					return;
				}
				autoRepathPolicy.OnPathCalculated(managedState.pendingPath.error);
				ManagedState.SetPath(managedState.pendingPath, managedState, in movementPlane, ref destination);
			}
			ref PathTracer ptr = ref managedState.pathTracer;
			if (ptr.hasPath)
			{
				state.closestOnNavmesh = ptr.UpdateStart(transform.Position, PathTracer.RepairQuality.High, movementPlane.value);
				state.endOfPath = ptr.UpdateEnd(destination.destination, PathTracer.RepairQuality.High, movementPlane.value);
				if (state.pathTracerVersion != ptr.version)
				{
					nextCornersScratch.Clear();
					bool flag;
					int nextCornerIndices = ptr.GetNextCornerIndices(ref indicesScratch, ptr.desiredCornersForGoodSimplification, allocator, true, out flag);
					ptr.ConvertCornerIndicesToPathProjected(indicesScratch, nextCornerIndices, flag, nextCornersScratch, movementPlane.value.up);
					JobRepairPathHelpers.PathTracerInfo pathTracerInfo = new JobRepairPathHelpers.PathTracerInfo
					{
						endPointOfFirstPart = ptr.endPointOfFirstPart,
						hasValidEndPoint = ptr.hasValidEndPoint,
						partCount = ptr.partCount,
						isStale = ptr.isStale
					};
					UnsafeSpan<float3> unsafeSpan = nextCornersScratch.AsUnsafeSpan<float3>();
					JobRepairPathHelpers.UpdateReachedEndInfo(ref unsafeSpan, ref state, ref movementPlane, ref transform, ref shape, ref destination, settings.stopDistance, ref pathTracerInfo);
					state.pathTracerVersion = ptr.version;
				}
				else
				{
					JobRepairPathHelpers.UpdateReachedOrientation(ref state, ref transform, ref movementPlane, ref destination);
				}
				if (ptr.startNode != null && !ptr.startNode.Destroyed && ptr.startNode.Walkable)
				{
					state.graphIndex = ptr.startNode.GraphIndex;
					state.hierarchicalNodeIndex = ptr.startNode.HierarchicalNodeIndex;
				}
				else
				{
					state.graphIndex = 255U;
					state.hierarchicalNodeIndex = -1;
				}
			}
			else
			{
				state.SetPathIsEmpty(transform.Position);
			}
			if (readyToTraverseOffMeshLink.IsValid)
			{
				readyToTraverseOffMeshLink.ValueRW = state.reachedEndOfPart && managedState.pathTracer.isNextPartValidLink;
			}
		}

		// Token: 0x0600151D RID: 5405 RVA: 0x00079421 File Offset: 0x00077621
		void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
		}

		// Token: 0x04000EB8 RID: 3768
		public JobRepairPath.Scheduler scheduler;

		// Token: 0x04000EB9 RID: 3769
		[NativeDisableContainerSafetyRestriction]
		public NativeArray<int> indicesScratch;

		// Token: 0x04000EBA RID: 3770
		[NativeDisableContainerSafetyRestriction]
		public NativeList<float3> nextCornersScratch;

		// Token: 0x04000EBB RID: 3771
		public bool onlyApplyPendingPaths;

		// Token: 0x04000EBC RID: 3772
		private static readonly ProfilerMarker MarkerRepair = new ProfilerMarker("Repair");

		// Token: 0x04000EBD RID: 3773
		private static readonly ProfilerMarker MarkerGetNextCorners = new ProfilerMarker("GetNextCorners");

		// Token: 0x04000EBE RID: 3774
		private static readonly ProfilerMarker MarkerUpdateReachedEndInfo = new ProfilerMarker("UpdateReachedEndInfo");

		// Token: 0x0200034A RID: 842
		public struct Scheduler
		{
			// Token: 0x0600151E RID: 5406 RVA: 0x00079430 File Offset: 0x00077630
			public EntityQueryBuilder GetEntityQuery(Allocator allocator)
			{
				return new EntityQueryBuilder(Allocator.Temp).WithAllRW<MovementState>().WithAllRW<ManagedState>().WithAll<LocalTransform>()
					.WithAll<MovementSettings, AutoRepathPolicy, DestinationPoint, AgentMovementPlane, AgentCylinderShape>()
					.WithAbsent<AgentOffMeshLinkTraversal>();
			}

			// Token: 0x0600151F RID: 5407 RVA: 0x00079470 File Offset: 0x00077670
			public Scheduler(ref SystemState systemState)
			{
				this.entityManagerHandle = GCHandle.Alloc(systemState.EntityManager);
				this.LocalTransformTypeHandleRO = systemState.GetComponentTypeHandle<LocalTransform>(true);
				this.MovementStateTypeHandleRW = systemState.GetComponentTypeHandle<MovementState>(false);
				this.AgentCylinderShapeTypeHandleRO = systemState.GetComponentTypeHandle<AgentCylinderShape>(true);
				this.AutoRepathPolicyRW = systemState.GetComponentTypeHandle<AutoRepathPolicy>(false);
				this.DestinationPointTypeHandleRO = systemState.GetComponentTypeHandle<DestinationPoint>(true);
				this.AgentMovementPlaneTypeHandleRO = systemState.GetComponentTypeHandle<AgentMovementPlane>(true);
				this.MovementSettingsTypeHandleRO = systemState.GetComponentTypeHandle<MovementSettings>(true);
				this.ReadyToTraverseOffMeshLinkTypeHandleRW = systemState.GetComponentTypeHandle<ReadyToTraverseOffMeshLink>(false);
				this.ManagedStateTypeHandleRW = systemState.EntityManager.GetComponentTypeHandle<ManagedState>(false);
				this.onlyApplyPendingPaths = false;
			}

			// Token: 0x06001520 RID: 5408 RVA: 0x00079517 File Offset: 0x00077717
			public void Dispose()
			{
				this.entityManagerHandle.Free();
			}

			// Token: 0x06001521 RID: 5409 RVA: 0x00079524 File Offset: 0x00077724
			public void Update(ref SystemState systemState)
			{
				this.LocalTransformTypeHandleRO.Update(ref systemState);
				this.MovementStateTypeHandleRW.Update(ref systemState);
				this.AgentCylinderShapeTypeHandleRO.Update(ref systemState);
				this.AutoRepathPolicyRW.Update(ref systemState);
				this.DestinationPointTypeHandleRO.Update(ref systemState);
				this.ManagedStateTypeHandleRW.Update(ref systemState);
				this.MovementSettingsTypeHandleRO.Update(ref systemState);
				this.AgentMovementPlaneTypeHandleRO.Update(ref systemState);
				this.ReadyToTraverseOffMeshLinkTypeHandleRW.Update(ref systemState);
			}

			// Token: 0x06001522 RID: 5410 RVA: 0x000795A0 File Offset: 0x000777A0
			public JobHandle ScheduleParallel(ref SystemState systemState, EntityQuery query, JobHandle dependency)
			{
				this.Update(ref systemState);
				return new JobRepairPath
				{
					scheduler = this,
					onlyApplyPendingPaths = this.onlyApplyPendingPaths
				}.ScheduleParallel(query, dependency);
			}

			// Token: 0x04000EBF RID: 3775
			[ReadOnly]
			public ComponentTypeHandle<LocalTransform> LocalTransformTypeHandleRO;

			// Token: 0x04000EC0 RID: 3776
			public ComponentTypeHandle<MovementState> MovementStateTypeHandleRW;

			// Token: 0x04000EC1 RID: 3777
			[ReadOnly]
			public ComponentTypeHandle<AgentCylinderShape> AgentCylinderShapeTypeHandleRO;

			// Token: 0x04000EC2 RID: 3778
			[NativeDisableContainerSafetyRestriction]
			public ComponentTypeHandle<ManagedState> ManagedStateTypeHandleRW;

			// Token: 0x04000EC3 RID: 3779
			[ReadOnly]
			public ComponentTypeHandle<MovementSettings> MovementSettingsTypeHandleRO;

			// Token: 0x04000EC4 RID: 3780
			public ComponentTypeHandle<AutoRepathPolicy> AutoRepathPolicyRW;

			// Token: 0x04000EC5 RID: 3781
			[ReadOnly]
			public ComponentTypeHandle<DestinationPoint> DestinationPointTypeHandleRO;

			// Token: 0x04000EC6 RID: 3782
			[ReadOnly]
			public ComponentTypeHandle<AgentMovementPlane> AgentMovementPlaneTypeHandleRO;

			// Token: 0x04000EC7 RID: 3783
			public ComponentTypeHandle<ReadyToTraverseOffMeshLink> ReadyToTraverseOffMeshLinkTypeHandleRW;

			// Token: 0x04000EC8 RID: 3784
			public GCHandle entityManagerHandle;

			// Token: 0x04000EC9 RID: 3785
			public bool onlyApplyPendingPaths;
		}
	}
}
