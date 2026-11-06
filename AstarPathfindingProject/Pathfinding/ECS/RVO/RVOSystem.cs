using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Pathfinding.RVO;
using Pathfinding.Sync;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Pathfinding.ECS.RVO
{
	// Token: 0x020003B4 RID: 948
	[BurstCompile]
	[UpdateAfter(typeof(FollowerControlSystem))]
	[UpdateInGroup(typeof(AIMovementSystemGroup))]
	[CompilerGenerated]
	public struct RVOSystem : ISystem, ISystemCompilerGenerated
	{
		// Token: 0x060017C7 RID: 6087 RVA: 0x0007FF3C File Offset: 0x0007E13C
		public void OnCreate(ref SystemState state)
		{
			this.lastSimulator = GCHandle.Alloc(null, GCHandleType.Weak);
			this.agentOffMeshLinkTraversalLookup = state.GetComponentLookup<AgentOffMeshLinkTraversal>(true);
		}

		// Token: 0x060017C8 RID: 6088 RVA: 0x0007FF58 File Offset: 0x0007E158
		public void OnDestroy(ref SystemState state)
		{
			this.lastSimulator.Free();
		}

		// Token: 0x060017C9 RID: 6089 RVA: 0x0007FF68 File Offset: 0x0007E168
		public void OnUpdate(ref SystemState systemState)
		{
			RVOSimulator active = RVOSimulator.active;
			SimulatorBurst simulatorBurst = ((active != null) ? active.GetSimulator() : null);
			if (simulatorBurst != this.lastSimulator.Target)
			{
				this.RemoveAllAgentsFromSimulation(ref systemState);
				this.lastSimulator.Target = simulatorBurst;
			}
			if (simulatorBurst == null)
			{
				return;
			}
			this.AddAndRemoveAgentsFromSimulation(ref systemState, simulatorBurst);
			if (AIMovementSystemGroup.TimeScaledRateManager.CheapSimulationOnly)
			{
				return;
			}
			this.CopyFromEntitiesToRVOSimulator(ref systemState, simulatorBurst, systemState.WorldUnmanaged.Time.DeltaTime);
			simulatorBurst.Update(systemState.Dependency, systemState.WorldUnmanaged.Time.DeltaTime, AIMovementSystemGroup.TimeScaledRateManager.IsLastSubstep, systemState.WorldUpdateAllocator);
			this.CopyFromRVOSimulatorToEntities(ref systemState, simulatorBurst);
		}

		// Token: 0x060017CA RID: 6090 RVA: 0x0008000C File Offset: 0x0007E20C
		private void RemoveAllAgentsFromSimulation(ref SystemState systemState)
		{
			EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
			NativeArray<Entity> nativeArray = this.__query_614662194_0.ToEntityArray(systemState.WorldUpdateAllocator);
			entityCommandBuffer.RemoveComponent<AgentIndex>(nativeArray);
			entityCommandBuffer.Playback(systemState.EntityManager);
			entityCommandBuffer.Dispose();
		}

		// Token: 0x060017CB RID: 6091 RVA: 0x0008005C File Offset: 0x0007E25C
		private void AddAndRemoveAgentsFromSimulation(ref SystemState systemState, SimulatorBurst simulator)
		{
			EntityQuery _query_614662194_ = this.__query_614662194_1;
			EntityQuery _query_614662194_2 = this.__query_614662194_2;
			EntityQuery _query_614662194_3 = this.__query_614662194_3;
			NativeArray<AgentIndex> nativeArray = _query_614662194_.ToComponentDataArray<AgentIndex>(systemState.WorldUpdateAllocator);
			NativeArray<AgentIndex> nativeArray2 = _query_614662194_2.ToComponentDataArray<AgentIndex>(systemState.WorldUpdateAllocator);
			NativeArray<Entity> nativeArray3 = _query_614662194_3.ToEntityArray(systemState.WorldUpdateAllocator);
			if (nativeArray.Length > 0 || nativeArray2.Length > 0 || nativeArray3.Length > 0)
			{
				EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
				entityCommandBuffer.RemoveComponent<AgentIndex>(_query_614662194_, EntityQueryCaptureMode.AtPlayback);
				entityCommandBuffer.RemoveComponent<AgentIndex>(_query_614662194_2, EntityQueryCaptureMode.AtPlayback);
				for (int i = 0; i < nativeArray.Length; i++)
				{
					simulator.RemoveAgent(nativeArray[i], false);
				}
				for (int j = 0; j < nativeArray2.Length; j++)
				{
					simulator.RemoveAgent(nativeArray2[j], true);
				}
				for (int k = 0; k < nativeArray3.Length; k++)
				{
					entityCommandBuffer.AddComponent<AgentIndex>(nativeArray3[k], simulator.AddAgentBurst(Vector3.zero));
				}
				entityCommandBuffer.Playback(systemState.EntityManager);
				entityCommandBuffer.Dispose();
			}
		}

		// Token: 0x060017CC RID: 6092 RVA: 0x00080190 File Offset: 0x0007E390
		private void CopyFromEntitiesToRVOSimulator(ref SystemState systemState, SimulatorBurst simulator, float dt)
		{
			this.agentOffMeshLinkTraversalLookup.Update(ref systemState);
			RWLock.WriteLockAsync writeLockAsync = simulator.LockSimulationDataReadWrite();
			RVOSystem.JobCopyFromEntitiesToRVOSimulator jobCopyFromEntitiesToRVOSimulator = default(RVOSystem.JobCopyFromEntitiesToRVOSimulator);
			jobCopyFromEntitiesToRVOSimulator.agentData = simulator.simulationData;
			jobCopyFromEntitiesToRVOSimulator.agentOutputData = simulator.outputData;
			jobCopyFromEntitiesToRVOSimulator.movementPlaneMode = simulator.movementPlane;
			jobCopyFromEntitiesToRVOSimulator.agentOffMeshLinkTraversalLookup = this.agentOffMeshLinkTraversalLookup;
			jobCopyFromEntitiesToRVOSimulator.dt = dt;
			systemState.Dependency = this.__ScheduleViaJobChunkExtension_0(jobCopyFromEntitiesToRVOSimulator, this.__TypeHandle.__Pathfinding_ECS_RVO_RVOSystem_JobCopyFromEntitiesToRVOSimulator_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, JobHandle.CombineDependencies(writeLockAsync.dependency, systemState.Dependency), ref systemState, false);
			writeLockAsync.UnlockAfter(systemState.Dependency);
		}

		// Token: 0x060017CD RID: 6093 RVA: 0x00080234 File Offset: 0x0007E434
		private void CopyFromRVOSimulatorToEntities(ref SystemState systemState, SimulatorBurst simulator)
		{
			RWLock.WriteLockAsync writeLockAsync = simulator.LockSimulationDataReadWrite();
			RVOSystem.JobCopyFromRVOSimulatorToEntities jobCopyFromRVOSimulatorToEntities = default(RVOSystem.JobCopyFromRVOSimulatorToEntities);
			jobCopyFromRVOSimulatorToEntities.quadtree = simulator.quadtree;
			jobCopyFromRVOSimulatorToEntities.agentDataVersions = simulator.simulationData.version;
			jobCopyFromRVOSimulatorToEntities.agentOutputData = simulator.outputData;
			systemState.Dependency = this.__ScheduleViaJobChunkExtension_1(jobCopyFromRVOSimulatorToEntities, this.__TypeHandle.__Pathfinding_ECS_RVO_RVOSystem_JobCopyFromRVOSimulatorToEntities_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, JobHandle.CombineDependencies(writeLockAsync.dependency, systemState.Dependency), ref systemState, false);
			writeLockAsync.UnlockAfter(systemState.Dependency);
		}

		// Token: 0x060017CE RID: 6094 RVA: 0x000802BC File Offset: 0x0007E4BC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private JobHandle __ScheduleViaJobChunkExtension_0(RVOSystem.JobCopyFromEntitiesToRVOSimulator job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
		{
			dependency = this.__TypeHandle.__Pathfinding_ECS_RVO_RVOSystem_JobCopyFromEntitiesToRVOSimulator_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
			this.__TypeHandle.__Pathfinding_ECS_RVO_RVOSystem_JobCopyFromEntitiesToRVOSimulator_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
			this.__TypeHandle.__Pathfinding_ECS_RVO_RVOSystem_JobCopyFromEntitiesToRVOSimulator_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
			return this.__TypeHandle.__Pathfinding_ECS_RVO_RVOSystem_JobCopyFromEntitiesToRVOSimulator_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
		}

		// Token: 0x060017CF RID: 6095 RVA: 0x00080328 File Offset: 0x0007E528
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private JobHandle __ScheduleViaJobChunkExtension_1(RVOSystem.JobCopyFromRVOSimulatorToEntities job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
		{
			dependency = this.__TypeHandle.__Pathfinding_ECS_RVO_RVOSystem_JobCopyFromRVOSimulatorToEntities_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
			this.__TypeHandle.__Pathfinding_ECS_RVO_RVOSystem_JobCopyFromRVOSimulatorToEntities_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
			this.__TypeHandle.__Pathfinding_ECS_RVO_RVOSystem_JobCopyFromRVOSimulatorToEntities_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
			return this.__TypeHandle.__Pathfinding_ECS_RVO_RVOSystem_JobCopyFromRVOSimulatorToEntities_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
		}

		// Token: 0x060017D0 RID: 6096 RVA: 0x00080394 File Offset: 0x0007E594
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			this.__query_614662194_0 = entityQueryBuilder.WithAllRW<AgentIndex>().Build(ref state);
			entityQueryBuilder.Reset();
			this.__query_614662194_1 = entityQueryBuilder.WithAll<AgentIndex>().WithNone<RVOAgent>().WithOptions(EntityQueryOptions.IncludeDisabledEntities)
				.Build(ref state);
			entityQueryBuilder.Reset();
			this.__query_614662194_2 = entityQueryBuilder.WithAll<AgentIndex, AgentOffMeshLinkLocalAvoidanceDisabled>().WithOptions(EntityQueryOptions.IncludeDisabledEntities).Build(ref state);
			entityQueryBuilder.Reset();
			this.__query_614662194_3 = entityQueryBuilder.WithAll<RVOAgent>().WithNone<AgentIndex, AgentOffMeshLinkLocalAvoidanceDisabled>().Build(ref state);
			entityQueryBuilder.Reset();
			entityQueryBuilder.Dispose();
		}

		// Token: 0x060017D1 RID: 6097 RVA: 0x0008044F File Offset: 0x0007E64F
		public void OnCreateForCompiler(ref SystemState state)
		{
			this.__AssignQueries(ref state);
			this.__TypeHandle.__AssignHandles(ref state);
		}

		// Token: 0x060017D2 RID: 6098 RVA: 0x00080464 File Offset: 0x0007E664
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnCreate(IntPtr self, IntPtr state)
		{
			((RVOSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x060017D3 RID: 6099 RVA: 0x0008047D File Offset: 0x0007E67D
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
		{
			((RVOSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x060017D4 RID: 6100 RVA: 0x00080496 File Offset: 0x0007E696
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnDestroy(IntPtr self, IntPtr state)
		{
			((RVOSystem*)self.ToPointer())->OnDestroy(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x060017D5 RID: 6101 RVA: 0x000804AF File Offset: 0x0007E6AF
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
		{
			((RVOSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x04000FD4 RID: 4052
		private GCHandle lastSimulator;

		// Token: 0x04000FD5 RID: 4053
		private ComponentLookup<AgentOffMeshLinkTraversal> agentOffMeshLinkTraversalLookup;

		// Token: 0x04000FD6 RID: 4054
		private RVOSystem.TypeHandle __TypeHandle;

		// Token: 0x04000FD7 RID: 4055
		private EntityQuery __query_614662194_0;

		// Token: 0x04000FD8 RID: 4056
		private EntityQuery __query_614662194_1;

		// Token: 0x04000FD9 RID: 4057
		private EntityQuery __query_614662194_2;

		// Token: 0x04000FDA RID: 4058
		private EntityQuery __query_614662194_3;

		// Token: 0x020003B5 RID: 949
		[BurstCompile]
		[CompilerGenerated]
		public struct JobCopyFromEntitiesToRVOSimulator : IJobEntity, IJobChunk
		{
			// Token: 0x060017D6 RID: 6102 RVA: 0x000804C8 File Offset: 0x0007E6C8
			public void Execute(Entity entity, in LocalTransform transform, in AgentCylinderShape shape, in AgentMovementPlane movementPlane, in AgentIndex agentIndex, in RVOAgent controller, in MovementControl target)
			{
				float num = math.abs(transform.Scale);
				int num2;
				if (!agentIndex.TryGetIndex(ref this.agentData, out num2))
				{
					throw new InvalidOperationException("RVOAgent has an invalid entity index");
				}
				this.agentData.radius[num2] = math.clamp(shape.radius * num, 0.001f, 100000f);
				this.agentData.agentTimeHorizon[num2] = math.clamp(controller.agentTimeHorizon, 0f, 100000f);
				this.agentData.obstacleTimeHorizon[num2] = math.clamp(controller.obstacleTimeHorizon, 0f, 100000f);
				this.agentData.locked[num2] = controller.locked;
				this.agentData.maxNeighbours[num2] = math.max(controller.maxNeighbours, 0);
				this.agentData.debugFlags[num2] = controller.debug;
				this.agentData.layer[num2] = controller.layer;
				this.agentData.collidesWith[num2] = controller.collidesWith;
				this.agentData.targetPoint[num2] = target.targetPoint;
				this.agentData.desiredSpeed[num2] = math.clamp(target.speed, 0f, 100000f);
				this.agentData.maxSpeed[num2] = math.clamp(target.maxSpeed, 0f, 100000f);
				this.agentData.manuallyControlled[num2] = target.overrideLocalAvoidance;
				this.agentData.endOfPath[num2] = target.endOfPath;
				this.agentData.hierarchicalNodeIndex[num2] = target.hierarchicalNodeIndex;
				this.agentData.movementPlane[num2] = movementPlane.value;
				float num3;
				float2 @float = movementPlane.value.ToPlane(transform.Position, out num3);
				if (this.movementPlaneMode == MovementPlane.XY)
				{
					this.agentData.height[num2] = 1f;
					this.agentData.position[num2] = movementPlane.value.ToWorld(@float, 0f);
				}
				else
				{
					float num4 = 0.5f * shape.height;
					this.agentData.height[num2] = math.clamp(shape.height * num, 0f, 100000f);
					this.agentData.position[num2] = movementPlane.value.ToWorld(@float, num3 + (num4 - 0.5f * shape.height) * num);
				}
				ReachedEndOfPath reachedEndOfPath = this.agentOutputData.effectivelyReachedDestination[num2];
				float num5 = math.clamp(controller.priority * controller.priorityMultiplier, 0f, 100000f);
				float num6 = math.clamp(controller.flowFollowingStrength, 0f, 1f);
				if (reachedEndOfPath == ReachedEndOfPath.Reached)
				{
					num6 = math.lerp(this.agentData.flowFollowingStrength[num2], 1f, 6f * this.dt);
					num5 *= 0.3f;
				}
				else if (reachedEndOfPath == ReachedEndOfPath.ReachedSoon)
				{
					num6 = math.lerp(this.agentData.flowFollowingStrength[num2], 1f, 6f * this.dt);
					num5 *= 0.45f;
				}
				this.agentData.priority[num2] = num5;
				this.agentData.flowFollowingStrength[num2] = num6;
				if (this.agentOffMeshLinkTraversalLookup.HasComponent(entity))
				{
					this.agentData.manuallyControlled[num2] = true;
				}
			}

			// Token: 0x060017D7 RID: 6103 RVA: 0x0008087C File Offset: 0x0007EA7C
			[CompilerGenerated]
			public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
			{
				IntPtr intPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, this.__TypeHandle.__Unity_Entities_Entity_TypeHandle);
				IntPtr intPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<LocalTransform>(chunk, ref this.__TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentTypeHandle);
				IntPtr intPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<AgentCylinderShape>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_AgentCylinderShape_RO_ComponentTypeHandle);
				IntPtr intPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<AgentMovementPlane>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_AgentMovementPlane_RO_ComponentTypeHandle);
				IntPtr intPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<AgentIndex>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_RVO_AgentIndex_RO_ComponentTypeHandle);
				IntPtr intPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<RVOAgent>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_RVO_RVOAgent_RO_ComponentTypeHandle);
				IntPtr intPtr7 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<MovementControl>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_MovementControl_RO_ComponentTypeHandle);
				int count = chunk.Count;
				int num = 0;
				if (!useEnabledMask)
				{
					for (int i = 0; i < count; i++)
					{
						Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(intPtr, i);
						ref LocalTransform ptr = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr2, i);
						ref AgentCylinderShape ptr2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr3, i);
						ref AgentMovementPlane ptr3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr4, i);
						ref AgentIndex ptr4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentIndex>(intPtr5, i);
						ref RVOAgent ptr5 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<RVOAgent>(intPtr6, i);
						ref MovementControl ptr6 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementControl>(intPtr7, i);
						this.Execute(entity, in ptr, in ptr2, in ptr3, in ptr4, in ptr5, in ptr6);
						num++;
					}
					return;
				}
				if (math.countbits(chunkEnabledMask.ULong0 ^ (chunkEnabledMask.ULong0 << 1)) + math.countbits(chunkEnabledMask.ULong1 ^ (chunkEnabledMask.ULong1 << 1)) - 1 <= 4)
				{
					int j = 0;
					int num2 = 0;
					while (InternalCompilerInterface.UnsafeTryGetNextEnabledBitRange(chunkEnabledMask, num2, out j, out num2))
					{
						while (j < num2)
						{
							Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(intPtr, j);
							ref LocalTransform ptr7 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr2, j);
							ref AgentCylinderShape ptr8 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr3, j);
							ref AgentMovementPlane ptr9 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr4, j);
							ref AgentIndex ptr10 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentIndex>(intPtr5, j);
							ref RVOAgent ptr11 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<RVOAgent>(intPtr6, j);
							ref MovementControl ptr12 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementControl>(intPtr7, j);
							this.Execute(entity2, in ptr7, in ptr8, in ptr9, in ptr10, in ptr11, in ptr12);
							j++;
							num++;
						}
					}
					return;
				}
				ulong num3 = chunkEnabledMask.ULong0;
				int num4 = math.min(64, count);
				for (int k = 0; k < num4; k++)
				{
					if ((num3 & 1UL) != 0UL)
					{
						Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(intPtr, k);
						ref LocalTransform ptr13 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr2, k);
						ref AgentCylinderShape ptr14 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr3, k);
						ref AgentMovementPlane ptr15 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr4, k);
						ref AgentIndex ptr16 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentIndex>(intPtr5, k);
						ref RVOAgent ptr17 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<RVOAgent>(intPtr6, k);
						ref MovementControl ptr18 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementControl>(intPtr7, k);
						this.Execute(entity3, in ptr13, in ptr14, in ptr15, in ptr16, in ptr17, in ptr18);
						num++;
					}
					num3 >>= 1;
				}
				num3 = chunkEnabledMask.ULong1;
				for (int l = 64; l < count; l++)
				{
					if ((num3 & 1UL) != 0UL)
					{
						Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(intPtr, l);
						ref LocalTransform ptr19 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr2, l);
						ref AgentCylinderShape ptr20 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr3, l);
						ref AgentMovementPlane ptr21 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr4, l);
						ref AgentIndex ptr22 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentIndex>(intPtr5, l);
						ref RVOAgent ptr23 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<RVOAgent>(intPtr6, l);
						ref MovementControl ptr24 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementControl>(intPtr7, l);
						this.Execute(entity4, in ptr19, in ptr20, in ptr21, in ptr22, in ptr23, in ptr24);
						num++;
					}
					num3 >>= 1;
				}
			}

			// Token: 0x060017D8 RID: 6104 RVA: 0x00080B98 File Offset: 0x0007ED98
			private JobHandle __ThrowCodeGenException()
			{
				throw new Exception("This method should have been replaced by source gen.");
			}

			// Token: 0x060017D9 RID: 6105 RVA: 0x00080BAF File Offset: 0x0007EDAF
			public void Run()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x060017DA RID: 6106 RVA: 0x00080BAF File Offset: 0x0007EDAF
			public void RunByRef()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x060017DB RID: 6107 RVA: 0x00080BAF File Offset: 0x0007EDAF
			public void Run(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x060017DC RID: 6108 RVA: 0x00080BAF File Offset: 0x0007EDAF
			public void RunByRef(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x060017DD RID: 6109 RVA: 0x00080BB8 File Offset: 0x0007EDB8
			public JobHandle Schedule(JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x060017DE RID: 6110 RVA: 0x00080BB8 File Offset: 0x0007EDB8
			public JobHandle ScheduleByRef(JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x060017DF RID: 6111 RVA: 0x00080BB8 File Offset: 0x0007EDB8
			public JobHandle Schedule(EntityQuery query, JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x060017E0 RID: 6112 RVA: 0x00080BB8 File Offset: 0x0007EDB8
			public JobHandle ScheduleByRef(EntityQuery query, JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x060017E1 RID: 6113 RVA: 0x00080BAF File Offset: 0x0007EDAF
			public void Schedule()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x060017E2 RID: 6114 RVA: 0x00080BAF File Offset: 0x0007EDAF
			public void ScheduleByRef()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x060017E3 RID: 6115 RVA: 0x00080BAF File Offset: 0x0007EDAF
			public void Schedule(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x060017E4 RID: 6116 RVA: 0x00080BAF File Offset: 0x0007EDAF
			public void ScheduleByRef(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x060017E5 RID: 6117 RVA: 0x00080BB8 File Offset: 0x0007EDB8
			public JobHandle ScheduleParallel(JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x060017E6 RID: 6118 RVA: 0x00080BB8 File Offset: 0x0007EDB8
			public JobHandle ScheduleParallelByRef(JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x060017E7 RID: 6119 RVA: 0x00080BB8 File Offset: 0x0007EDB8
			public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x060017E8 RID: 6120 RVA: 0x00080BB8 File Offset: 0x0007EDB8
			public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x060017E9 RID: 6121 RVA: 0x00080BB8 File Offset: 0x0007EDB8
			public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x060017EA RID: 6122 RVA: 0x00080BB8 File Offset: 0x0007EDB8
			public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x060017EB RID: 6123 RVA: 0x00080BAF File Offset: 0x0007EDAF
			public void ScheduleParallel()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x060017EC RID: 6124 RVA: 0x00080BAF File Offset: 0x0007EDAF
			public void ScheduleParallelByRef()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x060017ED RID: 6125 RVA: 0x00080BAF File Offset: 0x0007EDAF
			public void ScheduleParallel(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x060017EE RID: 6126 RVA: 0x00080BAF File Offset: 0x0007EDAF
			public void ScheduleParallelByRef(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x060017EF RID: 6127 RVA: 0x00080BC0 File Offset: 0x0007EDC0
			void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
			{
				this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
			}

			// Token: 0x04000FDB RID: 4059
			[NativeDisableParallelForRestriction]
			public SimulatorBurst.AgentData agentData;

			// Token: 0x04000FDC RID: 4060
			[ReadOnly]
			public SimulatorBurst.AgentOutputData agentOutputData;

			// Token: 0x04000FDD RID: 4061
			public MovementPlane movementPlaneMode;

			// Token: 0x04000FDE RID: 4062
			[ReadOnly]
			public ComponentLookup<AgentOffMeshLinkTraversal> agentOffMeshLinkTraversalLookup;

			// Token: 0x04000FDF RID: 4063
			public float dt;

			// Token: 0x04000FE0 RID: 4064
			private RVOSystem.JobCopyFromEntitiesToRVOSimulator.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

			// Token: 0x020003B6 RID: 950
			public struct InternalCompilerQueryAndHandleData
			{
				// Token: 0x060017F0 RID: 6128 RVA: 0x00080BD0 File Offset: 0x0007EDD0
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				private void __AssignQueries(ref SystemState state)
				{
					EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
					this.DefaultQuery = entityQueryBuilder.WithAll<LocalTransform>().WithAll<AgentCylinderShape>().WithAll<AgentMovementPlane>()
						.WithAll<AgentIndex>()
						.WithAll<RVOAgent>()
						.WithAll<MovementControl>()
						.Build(ref state);
					entityQueryBuilder.Reset();
					entityQueryBuilder.Dispose();
				}

				// Token: 0x060017F1 RID: 6129 RVA: 0x00080C37 File Offset: 0x0007EE37
				public void Init(ref SystemState state, bool assignDefaultQuery)
				{
					if (assignDefaultQuery)
					{
						this.__AssignQueries(ref state);
					}
					this.__TypeHandle.__AssignHandles(ref state);
				}

				// Token: 0x060017F2 RID: 6130 RVA: 0x00080C4F File Offset: 0x0007EE4F
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void Run(ref RVOSystem.JobCopyFromEntitiesToRVOSimulator job, EntityQuery query)
				{
					job.__TypeHandle = this.__TypeHandle;
					(ref job).RunByRef<RVOSystem.JobCopyFromEntitiesToRVOSimulator>(query);
				}

				// Token: 0x060017F3 RID: 6131 RVA: 0x00080C64 File Offset: 0x0007EE64
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public JobHandle Schedule(ref RVOSystem.JobCopyFromEntitiesToRVOSimulator job, EntityQuery query, JobHandle dependency)
				{
					job.__TypeHandle = this.__TypeHandle;
					return (ref job).ScheduleByRef<RVOSystem.JobCopyFromEntitiesToRVOSimulator>(query, dependency);
				}

				// Token: 0x060017F4 RID: 6132 RVA: 0x00080C7A File Offset: 0x0007EE7A
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public JobHandle ScheduleParallel(ref RVOSystem.JobCopyFromEntitiesToRVOSimulator job, EntityQuery query, JobHandle dependency)
				{
					job.__TypeHandle = this.__TypeHandle;
					return (ref job).ScheduleParallelByRef<RVOSystem.JobCopyFromEntitiesToRVOSimulator>(query, dependency);
				}

				// Token: 0x060017F5 RID: 6133 RVA: 0x00003788 File Offset: 0x00001988
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void UpdateBaseEntityIndexArray(ref RVOSystem.JobCopyFromEntitiesToRVOSimulator job, EntityQuery query, ref SystemState state)
				{
				}

				// Token: 0x060017F6 RID: 6134 RVA: 0x000743B8 File Offset: 0x000725B8
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public JobHandle UpdateBaseEntityIndexArray(ref RVOSystem.JobCopyFromEntitiesToRVOSimulator job, EntityQuery query, JobHandle dependency, ref SystemState state)
				{
					return dependency;
				}

				// Token: 0x060017F7 RID: 6135 RVA: 0x00003788 File Offset: 0x00001988
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void AssignEntityManager(ref RVOSystem.JobCopyFromEntitiesToRVOSimulator job, EntityManager entityManager)
				{
				}

				// Token: 0x04000FE1 RID: 4065
				public RVOSystem.JobCopyFromEntitiesToRVOSimulator.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

				// Token: 0x04000FE2 RID: 4066
				public EntityQuery DefaultQuery;

				// Token: 0x020003B7 RID: 951
				public struct TypeHandle
				{
					// Token: 0x060017F8 RID: 6136 RVA: 0x00080C90 File Offset: 0x0007EE90
					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public void __AssignHandles(ref SystemState state)
					{
						this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
						this.__Unity_Transforms_LocalTransform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>(true);
						this.__Pathfinding_ECS_AgentCylinderShape_RO_ComponentTypeHandle = state.GetComponentTypeHandle<AgentCylinderShape>(true);
						this.__Pathfinding_ECS_AgentMovementPlane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<AgentMovementPlane>(true);
						this.__Pathfinding_ECS_RVO_AgentIndex_RO_ComponentTypeHandle = state.GetComponentTypeHandle<AgentIndex>(true);
						this.__Pathfinding_ECS_RVO_RVOAgent_RO_ComponentTypeHandle = state.GetComponentTypeHandle<RVOAgent>(true);
						this.__Pathfinding_ECS_MovementControl_RO_ComponentTypeHandle = state.GetComponentTypeHandle<MovementControl>(true);
					}

					// Token: 0x060017F9 RID: 6137 RVA: 0x00080CF8 File Offset: 0x0007EEF8
					public void Update(ref SystemState state)
					{
						this.__Unity_Entities_Entity_TypeHandle.Update(ref state);
						this.__Unity_Transforms_LocalTransform_RO_ComponentTypeHandle.Update(ref state);
						this.__Pathfinding_ECS_AgentCylinderShape_RO_ComponentTypeHandle.Update(ref state);
						this.__Pathfinding_ECS_AgentMovementPlane_RO_ComponentTypeHandle.Update(ref state);
						this.__Pathfinding_ECS_RVO_AgentIndex_RO_ComponentTypeHandle.Update(ref state);
						this.__Pathfinding_ECS_RVO_RVOAgent_RO_ComponentTypeHandle.Update(ref state);
						this.__Pathfinding_ECS_MovementControl_RO_ComponentTypeHandle.Update(ref state);
					}

					// Token: 0x04000FE3 RID: 4067
					[ReadOnly]
					public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

					// Token: 0x04000FE4 RID: 4068
					[ReadOnly]
					public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentTypeHandle;

					// Token: 0x04000FE5 RID: 4069
					[ReadOnly]
					public ComponentTypeHandle<AgentCylinderShape> __Pathfinding_ECS_AgentCylinderShape_RO_ComponentTypeHandle;

					// Token: 0x04000FE6 RID: 4070
					[ReadOnly]
					public ComponentTypeHandle<AgentMovementPlane> __Pathfinding_ECS_AgentMovementPlane_RO_ComponentTypeHandle;

					// Token: 0x04000FE7 RID: 4071
					[ReadOnly]
					public ComponentTypeHandle<AgentIndex> __Pathfinding_ECS_RVO_AgentIndex_RO_ComponentTypeHandle;

					// Token: 0x04000FE8 RID: 4072
					[ReadOnly]
					public ComponentTypeHandle<RVOAgent> __Pathfinding_ECS_RVO_RVOAgent_RO_ComponentTypeHandle;

					// Token: 0x04000FE9 RID: 4073
					[ReadOnly]
					public ComponentTypeHandle<MovementControl> __Pathfinding_ECS_MovementControl_RO_ComponentTypeHandle;
				}
			}

			// Token: 0x020003B8 RID: 952
			public struct InternalCompiler
			{
				// Token: 0x060017FA RID: 6138 RVA: 0x00003788 File Offset: 0x00001988
				[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static void CheckForErrors(int scheduleType)
				{
				}
			}
		}

		// Token: 0x020003B9 RID: 953
		[BurstCompile]
		[CompilerGenerated]
		public struct JobCopyFromRVOSimulatorToEntities : IJobEntity, IJobChunk
		{
			// Token: 0x060017FB RID: 6139 RVA: 0x00080D5C File Offset: 0x0007EF5C
			public void Execute(in LocalTransform transform, in AgentCylinderShape shape, in AgentIndex agentIndex, in RVOAgent controller, in MovementControl control, ref ResolvedMovement resolved)
			{
				int num;
				if (!agentIndex.TryGetIndex(ref this.agentDataVersions, out num))
				{
					return;
				}
				float num2 = math.abs(transform.Scale);
				float num3 = shape.radius * num2 * 3f;
				float num4 = this.quadtree.QueryArea(transform.Position, num3);
				float num5 = math.min(1f, num4 / (2.8491104f * num3 * num3));
				resolved.targetPoint = this.agentOutputData.targetPoint[num];
				resolved.speed = this.agentOutputData.speed[num];
				float num6 = 1f;
				resolved.turningRadiusMultiplier = math.max(1f, math.pow(num5 * 2f, 4f) * num6);
			}

			// Token: 0x060017FC RID: 6140 RVA: 0x00080E1C File Offset: 0x0007F01C
			[CompilerGenerated]
			public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
			{
				IntPtr intPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<LocalTransform>(chunk, ref this.__TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentTypeHandle);
				IntPtr intPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<AgentCylinderShape>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_AgentCylinderShape_RO_ComponentTypeHandle);
				IntPtr intPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<AgentIndex>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_RVO_AgentIndex_RO_ComponentTypeHandle);
				IntPtr intPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<RVOAgent>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_RVO_RVOAgent_RO_ComponentTypeHandle);
				IntPtr intPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<MovementControl>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_MovementControl_RO_ComponentTypeHandle);
				IntPtr intPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<ResolvedMovement>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_ResolvedMovement_RW_ComponentTypeHandle);
				int count = chunk.Count;
				int num = 0;
				if (!useEnabledMask)
				{
					for (int i = 0; i < count; i++)
					{
						ref LocalTransform ptr = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr, i);
						ref AgentCylinderShape ptr2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr2, i);
						ref AgentIndex ptr3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentIndex>(intPtr3, i);
						ref RVOAgent ptr4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<RVOAgent>(intPtr4, i);
						ref MovementControl ptr5 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementControl>(intPtr5, i);
						ref ResolvedMovement ptr6 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<ResolvedMovement>(intPtr6, i);
						this.Execute(in ptr, in ptr2, in ptr3, in ptr4, in ptr5, ref ptr6);
						num++;
					}
					return;
				}
				if (math.countbits(chunkEnabledMask.ULong0 ^ (chunkEnabledMask.ULong0 << 1)) + math.countbits(chunkEnabledMask.ULong1 ^ (chunkEnabledMask.ULong1 << 1)) - 1 <= 4)
				{
					int j = 0;
					int num2 = 0;
					while (InternalCompilerInterface.UnsafeTryGetNextEnabledBitRange(chunkEnabledMask, num2, out j, out num2))
					{
						while (j < num2)
						{
							ref LocalTransform ptr7 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr, j);
							ref AgentCylinderShape ptr8 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr2, j);
							ref AgentIndex ptr9 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentIndex>(intPtr3, j);
							ref RVOAgent ptr10 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<RVOAgent>(intPtr4, j);
							ref MovementControl ptr11 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementControl>(intPtr5, j);
							ref ResolvedMovement ptr12 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<ResolvedMovement>(intPtr6, j);
							this.Execute(in ptr7, in ptr8, in ptr9, in ptr10, in ptr11, ref ptr12);
							j++;
							num++;
						}
					}
					return;
				}
				ulong num3 = chunkEnabledMask.ULong0;
				int num4 = math.min(64, count);
				for (int k = 0; k < num4; k++)
				{
					if ((num3 & 1UL) != 0UL)
					{
						ref LocalTransform ptr13 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr, k);
						ref AgentCylinderShape ptr14 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr2, k);
						ref AgentIndex ptr15 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentIndex>(intPtr3, k);
						ref RVOAgent ptr16 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<RVOAgent>(intPtr4, k);
						ref MovementControl ptr17 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementControl>(intPtr5, k);
						ref ResolvedMovement ptr18 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<ResolvedMovement>(intPtr6, k);
						this.Execute(in ptr13, in ptr14, in ptr15, in ptr16, in ptr17, ref ptr18);
						num++;
					}
					num3 >>= 1;
				}
				num3 = chunkEnabledMask.ULong1;
				for (int l = 64; l < count; l++)
				{
					if ((num3 & 1UL) != 0UL)
					{
						ref LocalTransform ptr19 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr, l);
						ref AgentCylinderShape ptr20 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr2, l);
						ref AgentIndex ptr21 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentIndex>(intPtr3, l);
						ref RVOAgent ptr22 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<RVOAgent>(intPtr4, l);
						ref MovementControl ptr23 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementControl>(intPtr5, l);
						ref ResolvedMovement ptr24 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<ResolvedMovement>(intPtr6, l);
						this.Execute(in ptr19, in ptr20, in ptr21, in ptr22, in ptr23, ref ptr24);
						num++;
					}
					num3 >>= 1;
				}
			}

			// Token: 0x060017FD RID: 6141 RVA: 0x000810E8 File Offset: 0x0007F2E8
			private JobHandle __ThrowCodeGenException()
			{
				throw new Exception("This method should have been replaced by source gen.");
			}

			// Token: 0x060017FE RID: 6142 RVA: 0x000810FF File Offset: 0x0007F2FF
			public void Run()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x060017FF RID: 6143 RVA: 0x000810FF File Offset: 0x0007F2FF
			public void RunByRef()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001800 RID: 6144 RVA: 0x000810FF File Offset: 0x0007F2FF
			public void Run(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001801 RID: 6145 RVA: 0x000810FF File Offset: 0x0007F2FF
			public void RunByRef(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001802 RID: 6146 RVA: 0x00081108 File Offset: 0x0007F308
			public JobHandle Schedule(JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x06001803 RID: 6147 RVA: 0x00081108 File Offset: 0x0007F308
			public JobHandle ScheduleByRef(JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x06001804 RID: 6148 RVA: 0x00081108 File Offset: 0x0007F308
			public JobHandle Schedule(EntityQuery query, JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x06001805 RID: 6149 RVA: 0x00081108 File Offset: 0x0007F308
			public JobHandle ScheduleByRef(EntityQuery query, JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x06001806 RID: 6150 RVA: 0x000810FF File Offset: 0x0007F2FF
			public void Schedule()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001807 RID: 6151 RVA: 0x000810FF File Offset: 0x0007F2FF
			public void ScheduleByRef()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001808 RID: 6152 RVA: 0x000810FF File Offset: 0x0007F2FF
			public void Schedule(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001809 RID: 6153 RVA: 0x000810FF File Offset: 0x0007F2FF
			public void ScheduleByRef(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x0600180A RID: 6154 RVA: 0x00081108 File Offset: 0x0007F308
			public JobHandle ScheduleParallel(JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x0600180B RID: 6155 RVA: 0x00081108 File Offset: 0x0007F308
			public JobHandle ScheduleParallelByRef(JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x0600180C RID: 6156 RVA: 0x00081108 File Offset: 0x0007F308
			public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x0600180D RID: 6157 RVA: 0x00081108 File Offset: 0x0007F308
			public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x0600180E RID: 6158 RVA: 0x00081108 File Offset: 0x0007F308
			public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x0600180F RID: 6159 RVA: 0x00081108 File Offset: 0x0007F308
			public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
			{
				return this.__ThrowCodeGenException();
			}

			// Token: 0x06001810 RID: 6160 RVA: 0x000810FF File Offset: 0x0007F2FF
			public void ScheduleParallel()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001811 RID: 6161 RVA: 0x000810FF File Offset: 0x0007F2FF
			public void ScheduleParallelByRef()
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001812 RID: 6162 RVA: 0x000810FF File Offset: 0x0007F2FF
			public void ScheduleParallel(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001813 RID: 6163 RVA: 0x000810FF File Offset: 0x0007F2FF
			public void ScheduleParallelByRef(EntityQuery query)
			{
				this.__ThrowCodeGenException();
			}

			// Token: 0x06001814 RID: 6164 RVA: 0x00081110 File Offset: 0x0007F310
			void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
			{
				this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
			}

			// Token: 0x04000FEA RID: 4074
			[ReadOnly]
			public NativeArray<AgentIndex> agentDataVersions;

			// Token: 0x04000FEB RID: 4075
			[ReadOnly]
			public RVOQuadtreeBurst quadtree;

			// Token: 0x04000FEC RID: 4076
			[ReadOnly]
			public SimulatorBurst.AgentOutputData agentOutputData;

			// Token: 0x04000FED RID: 4077
			private const float MaximumCirclePackingDensity = 0.9069f;

			// Token: 0x04000FEE RID: 4078
			private RVOSystem.JobCopyFromRVOSimulatorToEntities.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

			// Token: 0x020003BA RID: 954
			public struct InternalCompilerQueryAndHandleData
			{
				// Token: 0x06001815 RID: 6165 RVA: 0x00081120 File Offset: 0x0007F320
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				private void __AssignQueries(ref SystemState state)
				{
					EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
					this.DefaultQuery = entityQueryBuilder.WithAll<LocalTransform>().WithAll<AgentCylinderShape>().WithAll<AgentIndex>()
						.WithAll<RVOAgent>()
						.WithAll<MovementControl>()
						.WithAllRW<ResolvedMovement>()
						.Build(ref state);
					entityQueryBuilder.Reset();
					entityQueryBuilder.Dispose();
				}

				// Token: 0x06001816 RID: 6166 RVA: 0x00081187 File Offset: 0x0007F387
				public void Init(ref SystemState state, bool assignDefaultQuery)
				{
					if (assignDefaultQuery)
					{
						this.__AssignQueries(ref state);
					}
					this.__TypeHandle.__AssignHandles(ref state);
				}

				// Token: 0x06001817 RID: 6167 RVA: 0x0008119F File Offset: 0x0007F39F
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void Run(ref RVOSystem.JobCopyFromRVOSimulatorToEntities job, EntityQuery query)
				{
					job.__TypeHandle = this.__TypeHandle;
					(ref job).RunByRef<RVOSystem.JobCopyFromRVOSimulatorToEntities>(query);
				}

				// Token: 0x06001818 RID: 6168 RVA: 0x000811B4 File Offset: 0x0007F3B4
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public JobHandle Schedule(ref RVOSystem.JobCopyFromRVOSimulatorToEntities job, EntityQuery query, JobHandle dependency)
				{
					job.__TypeHandle = this.__TypeHandle;
					return (ref job).ScheduleByRef<RVOSystem.JobCopyFromRVOSimulatorToEntities>(query, dependency);
				}

				// Token: 0x06001819 RID: 6169 RVA: 0x000811CA File Offset: 0x0007F3CA
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public JobHandle ScheduleParallel(ref RVOSystem.JobCopyFromRVOSimulatorToEntities job, EntityQuery query, JobHandle dependency)
				{
					job.__TypeHandle = this.__TypeHandle;
					return (ref job).ScheduleParallelByRef<RVOSystem.JobCopyFromRVOSimulatorToEntities>(query, dependency);
				}

				// Token: 0x0600181A RID: 6170 RVA: 0x00003788 File Offset: 0x00001988
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void UpdateBaseEntityIndexArray(ref RVOSystem.JobCopyFromRVOSimulatorToEntities job, EntityQuery query, ref SystemState state)
				{
				}

				// Token: 0x0600181B RID: 6171 RVA: 0x000743B8 File Offset: 0x000725B8
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public JobHandle UpdateBaseEntityIndexArray(ref RVOSystem.JobCopyFromRVOSimulatorToEntities job, EntityQuery query, JobHandle dependency, ref SystemState state)
				{
					return dependency;
				}

				// Token: 0x0600181C RID: 6172 RVA: 0x00003788 File Offset: 0x00001988
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void AssignEntityManager(ref RVOSystem.JobCopyFromRVOSimulatorToEntities job, EntityManager entityManager)
				{
				}

				// Token: 0x04000FEF RID: 4079
				public RVOSystem.JobCopyFromRVOSimulatorToEntities.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

				// Token: 0x04000FF0 RID: 4080
				public EntityQuery DefaultQuery;

				// Token: 0x020003BB RID: 955
				public struct TypeHandle
				{
					// Token: 0x0600181D RID: 6173 RVA: 0x000811E0 File Offset: 0x0007F3E0
					[MethodImpl(MethodImplOptions.AggressiveInlining)]
					public void __AssignHandles(ref SystemState state)
					{
						this.__Unity_Transforms_LocalTransform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>(true);
						this.__Pathfinding_ECS_AgentCylinderShape_RO_ComponentTypeHandle = state.GetComponentTypeHandle<AgentCylinderShape>(true);
						this.__Pathfinding_ECS_RVO_AgentIndex_RO_ComponentTypeHandle = state.GetComponentTypeHandle<AgentIndex>(true);
						this.__Pathfinding_ECS_RVO_RVOAgent_RO_ComponentTypeHandle = state.GetComponentTypeHandle<RVOAgent>(true);
						this.__Pathfinding_ECS_MovementControl_RO_ComponentTypeHandle = state.GetComponentTypeHandle<MovementControl>(true);
						this.__Pathfinding_ECS_ResolvedMovement_RW_ComponentTypeHandle = state.GetComponentTypeHandle<ResolvedMovement>(false);
					}

					// Token: 0x0600181E RID: 6174 RVA: 0x0008123C File Offset: 0x0007F43C
					public void Update(ref SystemState state)
					{
						this.__Unity_Transforms_LocalTransform_RO_ComponentTypeHandle.Update(ref state);
						this.__Pathfinding_ECS_AgentCylinderShape_RO_ComponentTypeHandle.Update(ref state);
						this.__Pathfinding_ECS_RVO_AgentIndex_RO_ComponentTypeHandle.Update(ref state);
						this.__Pathfinding_ECS_RVO_RVOAgent_RO_ComponentTypeHandle.Update(ref state);
						this.__Pathfinding_ECS_MovementControl_RO_ComponentTypeHandle.Update(ref state);
						this.__Pathfinding_ECS_ResolvedMovement_RW_ComponentTypeHandle.Update(ref state);
					}

					// Token: 0x04000FF1 RID: 4081
					[ReadOnly]
					public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentTypeHandle;

					// Token: 0x04000FF2 RID: 4082
					[ReadOnly]
					public ComponentTypeHandle<AgentCylinderShape> __Pathfinding_ECS_AgentCylinderShape_RO_ComponentTypeHandle;

					// Token: 0x04000FF3 RID: 4083
					[ReadOnly]
					public ComponentTypeHandle<AgentIndex> __Pathfinding_ECS_RVO_AgentIndex_RO_ComponentTypeHandle;

					// Token: 0x04000FF4 RID: 4084
					[ReadOnly]
					public ComponentTypeHandle<RVOAgent> __Pathfinding_ECS_RVO_RVOAgent_RO_ComponentTypeHandle;

					// Token: 0x04000FF5 RID: 4085
					[ReadOnly]
					public ComponentTypeHandle<MovementControl> __Pathfinding_ECS_MovementControl_RO_ComponentTypeHandle;

					// Token: 0x04000FF6 RID: 4086
					public ComponentTypeHandle<ResolvedMovement> __Pathfinding_ECS_ResolvedMovement_RW_ComponentTypeHandle;
				}
			}

			// Token: 0x020003BC RID: 956
			public struct InternalCompiler
			{
				// Token: 0x0600181F RID: 6175 RVA: 0x00003788 File Offset: 0x00001988
				[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static void CheckForErrors(int scheduleType)
				{
				}
			}
		}

		// Token: 0x020003BD RID: 957
		private struct TypeHandle
		{
			// Token: 0x06001820 RID: 6176 RVA: 0x00081291 File Offset: 0x0007F491
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				this.__Pathfinding_ECS_RVO_RVOSystem_JobCopyFromEntitiesToRVOSimulator_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, true);
				this.__Pathfinding_ECS_RVO_RVOSystem_JobCopyFromRVOSimulatorToEntities_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, true);
			}

			// Token: 0x04000FF7 RID: 4087
			public RVOSystem.JobCopyFromEntitiesToRVOSimulator.InternalCompilerQueryAndHandleData __Pathfinding_ECS_RVO_RVOSystem_JobCopyFromEntitiesToRVOSimulator_WithDefaultQuery_JobEntityTypeHandle;

			// Token: 0x04000FF8 RID: 4088
			public RVOSystem.JobCopyFromRVOSimulatorToEntities.InternalCompilerQueryAndHandleData __Pathfinding_ECS_RVO_RVOSystem_JobCopyFromRVOSimulatorToEntities_WithDefaultQuery_JobEntityTypeHandle;
		}
	}
}
