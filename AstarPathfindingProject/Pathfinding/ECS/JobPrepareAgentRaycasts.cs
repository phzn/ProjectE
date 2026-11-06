using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Pathfinding.ECS
{
	// Token: 0x02000345 RID: 837
	[BurstCompile]
	[CompilerGenerated]
	public struct JobPrepareAgentRaycasts : IJobEntity, IJobChunk
	{
		// Token: 0x060014F3 RID: 5363 RVA: 0x000788C9 File Offset: 0x00076AC9
		private void ApplyGravity(ref GravityState gravityState)
		{
			gravityState.verticalVelocity += this.gravity * this.dt;
		}

		// Token: 0x060014F4 RID: 5364 RVA: 0x000788E4 File Offset: 0x00076AE4
		private RaycastCommand CalculateRaycastCommands(ref LocalTransform transform, in AgentCylinderShape shape, in AgentMovementPlane movementPlane, in MovementSettings movementSettings, ref GravityState gravityState, PhysicsScene physicsScene)
		{
			float num;
			movementPlane.value.ToPlane(transform.Position, out num);
			float num2 = gravityState.verticalVelocity * this.dt;
			float num3;
			float2 @float = movementPlane.value.ToPlane(transform.Position, out num3);
			float num4 = math.max(num3 + num2, num) + shape.height * 0.5f;
			float num5 = math.min(num3 + num2, num);
			float num6 = num4 - num5;
			float3 float2 = movementPlane.value.ToWorld(0, -1f);
			this.raycastQueryParameters.layerMask = movementSettings.groundMask;
			return new RaycastCommand(physicsScene, movementPlane.value.ToWorld(@float, num4), float2, this.raycastQueryParameters, num6);
		}

		// Token: 0x060014F5 RID: 5365 RVA: 0x000789AC File Offset: 0x00076BAC
		public void Execute(ref LocalTransform transform, in AgentCylinderShape shape, in AgentMovementPlane movementPlane, ref MovementState state, in MovementSettings movementSettings, ref ResolvedMovement resolvedMovement, ref MovementStatistics movementStatistics, ref GravityState gravityState, in PhysicsSceneRef physicsWorldIndex, [EntityIndexInQuery] int entityIndexInQuery)
		{
			this.ApplyGravity(ref gravityState);
			this.raycastCommands[entityIndexInQuery] = this.CalculateRaycastCommands(ref transform, in shape, in movementPlane, in movementSettings, ref gravityState, physicsWorldIndex.physicsScene);
		}

		// Token: 0x060014F6 RID: 5366 RVA: 0x000789E4 File Offset: 0x00076BE4
		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			IntPtr intPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<LocalTransform>(chunk, ref this.__TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
			IntPtr intPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<AgentCylinderShape>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_AgentCylinderShape_RO_ComponentTypeHandle);
			IntPtr intPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<AgentMovementPlane>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_AgentMovementPlane_RO_ComponentTypeHandle);
			IntPtr intPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<MovementState>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_MovementState_RW_ComponentTypeHandle);
			IntPtr intPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<MovementSettings>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_MovementSettings_RO_ComponentTypeHandle);
			IntPtr intPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<ResolvedMovement>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_ResolvedMovement_RW_ComponentTypeHandle);
			IntPtr intPtr7 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<MovementStatistics>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_MovementStatistics_RW_ComponentTypeHandle);
			IntPtr intPtr8 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<GravityState>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_GravityState_RW_ComponentTypeHandle);
			PhysicsSceneRef sharedComponent = chunk.GetSharedComponent<PhysicsSceneRef>(this.__TypeHandle.__Pathfinding_ECS_PhysicsSceneRef_SharedComponentTypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					ref LocalTransform ptr = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr, i);
					ref AgentCylinderShape ptr2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr2, i);
					ref AgentMovementPlane ptr3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr3, i);
					ref MovementState ptr4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementState>(intPtr4, i);
					ref MovementSettings ptr5 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementSettings>(intPtr5, i);
					ref ResolvedMovement ptr6 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<ResolvedMovement>(intPtr6, i);
					ref MovementStatistics ptr7 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementStatistics>(intPtr7, i);
					ref GravityState ptr8 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<GravityState>(intPtr8, i);
					int num2 = this.__ChunkBaseEntityIndices[chunkIndexInQuery] + num;
					this.Execute(ref ptr, in ptr2, in ptr3, ref ptr4, in ptr5, ref ptr6, ref ptr7, ref ptr8, in sharedComponent, num2);
					num++;
				}
				return;
			}
			if (math.countbits(chunkEnabledMask.ULong0 ^ (chunkEnabledMask.ULong0 << 1)) + math.countbits(chunkEnabledMask.ULong1 ^ (chunkEnabledMask.ULong1 << 1)) - 1 <= 4)
			{
				int j = 0;
				int num3 = 0;
				while (InternalCompilerInterface.UnsafeTryGetNextEnabledBitRange(chunkEnabledMask, num3, out j, out num3))
				{
					while (j < num3)
					{
						ref LocalTransform ptr9 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr, j);
						ref AgentCylinderShape ptr10 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr2, j);
						ref AgentMovementPlane ptr11 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr3, j);
						ref MovementState ptr12 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementState>(intPtr4, j);
						ref MovementSettings ptr13 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementSettings>(intPtr5, j);
						ref ResolvedMovement ptr14 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<ResolvedMovement>(intPtr6, j);
						ref MovementStatistics ptr15 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementStatistics>(intPtr7, j);
						ref GravityState ptr16 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<GravityState>(intPtr8, j);
						int num4 = this.__ChunkBaseEntityIndices[chunkIndexInQuery] + num;
						this.Execute(ref ptr9, in ptr10, in ptr11, ref ptr12, in ptr13, ref ptr14, ref ptr15, ref ptr16, in sharedComponent, num4);
						j++;
						num++;
					}
				}
				return;
			}
			ulong num5 = chunkEnabledMask.ULong0;
			int num6 = math.min(64, count);
			for (int k = 0; k < num6; k++)
			{
				if ((num5 & 1UL) != 0UL)
				{
					ref LocalTransform ptr17 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr, k);
					ref AgentCylinderShape ptr18 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr2, k);
					ref AgentMovementPlane ptr19 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr3, k);
					ref MovementState ptr20 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementState>(intPtr4, k);
					ref MovementSettings ptr21 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementSettings>(intPtr5, k);
					ref ResolvedMovement ptr22 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<ResolvedMovement>(intPtr6, k);
					ref MovementStatistics ptr23 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementStatistics>(intPtr7, k);
					ref GravityState ptr24 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<GravityState>(intPtr8, k);
					int num7 = this.__ChunkBaseEntityIndices[chunkIndexInQuery] + num;
					this.Execute(ref ptr17, in ptr18, in ptr19, ref ptr20, in ptr21, ref ptr22, ref ptr23, ref ptr24, in sharedComponent, num7);
					num++;
				}
				num5 >>= 1;
			}
			num5 = chunkEnabledMask.ULong1;
			for (int l = 64; l < count; l++)
			{
				if ((num5 & 1UL) != 0UL)
				{
					ref LocalTransform ptr25 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr, l);
					ref AgentCylinderShape ptr26 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr2, l);
					ref AgentMovementPlane ptr27 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr3, l);
					ref MovementState ptr28 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementState>(intPtr4, l);
					ref MovementSettings ptr29 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementSettings>(intPtr5, l);
					ref ResolvedMovement ptr30 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<ResolvedMovement>(intPtr6, l);
					ref MovementStatistics ptr31 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementStatistics>(intPtr7, l);
					ref GravityState ptr32 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<GravityState>(intPtr8, l);
					int num8 = this.__ChunkBaseEntityIndices[chunkIndexInQuery] + num;
					this.Execute(ref ptr25, in ptr26, in ptr27, ref ptr28, in ptr29, ref ptr30, ref ptr31, ref ptr32, in sharedComponent, num8);
					num++;
				}
				num5 >>= 1;
			}
		}

		// Token: 0x060014F7 RID: 5367 RVA: 0x00078DD4 File Offset: 0x00076FD4
		private JobHandle __ThrowCodeGenException()
		{
			throw new Exception("This method should have been replaced by source gen.");
		}

		// Token: 0x060014F8 RID: 5368 RVA: 0x00078DEB File Offset: 0x00076FEB
		public void Run()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060014F9 RID: 5369 RVA: 0x00078DEB File Offset: 0x00076FEB
		public void RunByRef()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060014FA RID: 5370 RVA: 0x00078DEB File Offset: 0x00076FEB
		public void Run(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060014FB RID: 5371 RVA: 0x00078DEB File Offset: 0x00076FEB
		public void RunByRef(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060014FC RID: 5372 RVA: 0x00078DF4 File Offset: 0x00076FF4
		public JobHandle Schedule(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x060014FD RID: 5373 RVA: 0x00078DF4 File Offset: 0x00076FF4
		public JobHandle ScheduleByRef(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x060014FE RID: 5374 RVA: 0x00078DF4 File Offset: 0x00076FF4
		public JobHandle Schedule(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x060014FF RID: 5375 RVA: 0x00078DF4 File Offset: 0x00076FF4
		public JobHandle ScheduleByRef(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001500 RID: 5376 RVA: 0x00078DEB File Offset: 0x00076FEB
		public void Schedule()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001501 RID: 5377 RVA: 0x00078DEB File Offset: 0x00076FEB
		public void ScheduleByRef()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001502 RID: 5378 RVA: 0x00078DEB File Offset: 0x00076FEB
		public void Schedule(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001503 RID: 5379 RVA: 0x00078DEB File Offset: 0x00076FEB
		public void ScheduleByRef(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001504 RID: 5380 RVA: 0x00078DF4 File Offset: 0x00076FF4
		public JobHandle ScheduleParallel(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001505 RID: 5381 RVA: 0x00078DF4 File Offset: 0x00076FF4
		public JobHandle ScheduleParallelByRef(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001506 RID: 5382 RVA: 0x00078DF4 File Offset: 0x00076FF4
		public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001507 RID: 5383 RVA: 0x00078DF4 File Offset: 0x00076FF4
		public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001508 RID: 5384 RVA: 0x00078DF4 File Offset: 0x00076FF4
		public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001509 RID: 5385 RVA: 0x00078DF4 File Offset: 0x00076FF4
		public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x0600150A RID: 5386 RVA: 0x00078DEB File Offset: 0x00076FEB
		public void ScheduleParallel()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x0600150B RID: 5387 RVA: 0x00078DEB File Offset: 0x00076FEB
		public void ScheduleParallelByRef()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x0600150C RID: 5388 RVA: 0x00078DEB File Offset: 0x00076FEB
		public void ScheduleParallel(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x0600150D RID: 5389 RVA: 0x00078DEB File Offset: 0x00076FEB
		public void ScheduleParallelByRef(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x0600150E RID: 5390 RVA: 0x00078DFC File Offset: 0x00076FFC
		void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
		}

		// Token: 0x04000EA7 RID: 3751
		public NativeArray<RaycastCommand> raycastCommands;

		// Token: 0x04000EA8 RID: 3752
		public QueryParameters raycastQueryParameters;

		// Token: 0x04000EA9 RID: 3753
		public float dt;

		// Token: 0x04000EAA RID: 3754
		public float gravity;

		// Token: 0x04000EAB RID: 3755
		private JobPrepareAgentRaycasts.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		// Token: 0x04000EAC RID: 3756
		[ReadOnly]
		public NativeArray<int> __ChunkBaseEntityIndices;

		// Token: 0x02000346 RID: 838
		public struct InternalCompilerQueryAndHandleData
		{
			// Token: 0x0600150F RID: 5391 RVA: 0x00078E0C File Offset: 0x0007700C
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				this.DefaultQuery = entityQueryBuilder.WithAll<AgentCylinderShape>().WithAll<AgentMovementPlane>().WithAll<MovementSettings>()
					.WithAll<PhysicsSceneRef>()
					.WithAllRW<LocalTransform>()
					.WithAllRW<MovementState>()
					.WithAllRW<ResolvedMovement>()
					.WithAllRW<MovementStatistics>()
					.WithAllRW<GravityState>()
					.Build(ref state);
				entityQueryBuilder.Reset();
				entityQueryBuilder.Dispose();
			}

			// Token: 0x06001510 RID: 5392 RVA: 0x00078E8B File Offset: 0x0007708B
			public void Init(ref SystemState state, bool assignDefaultQuery)
			{
				if (assignDefaultQuery)
				{
					this.__AssignQueries(ref state);
				}
				this.__TypeHandle.__AssignHandles(ref state);
			}

			// Token: 0x06001511 RID: 5393 RVA: 0x00078EA3 File Offset: 0x000770A3
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Run(ref JobPrepareAgentRaycasts job, EntityQuery query)
			{
				job.__TypeHandle = this.__TypeHandle;
				(ref job).RunByRef<JobPrepareAgentRaycasts>(query);
			}

			// Token: 0x06001512 RID: 5394 RVA: 0x00078EB8 File Offset: 0x000770B8
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref JobPrepareAgentRaycasts job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = this.__TypeHandle;
				return (ref job).ScheduleByRef<JobPrepareAgentRaycasts>(query, dependency);
			}

			// Token: 0x06001513 RID: 5395 RVA: 0x00078ECE File Offset: 0x000770CE
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref JobPrepareAgentRaycasts job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = this.__TypeHandle;
				return InternalCompilerInterface.JobChunkInterface.ScheduleParallelByRef<JobPrepareAgentRaycasts>(ref job, query, dependency, job.__ChunkBaseEntityIndices);
			}

			// Token: 0x06001514 RID: 5396 RVA: 0x00078EEC File Offset: 0x000770EC
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref JobPrepareAgentRaycasts job, EntityQuery query, ref SystemState state)
			{
				NativeArray<int> nativeArray = query.CalculateBaseEntityIndexArray(state.WorldUpdateAllocator);
				job.__ChunkBaseEntityIndices = nativeArray;
			}

			// Token: 0x06001515 RID: 5397 RVA: 0x00078F14 File Offset: 0x00077114
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref JobPrepareAgentRaycasts job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				JobHandle jobHandle;
				NativeArray<int> nativeArray = query.CalculateBaseEntityIndexArrayAsync(state.WorldUpdateAllocator, dependency, out jobHandle);
				job.__ChunkBaseEntityIndices = nativeArray;
				return jobHandle;
			}

			// Token: 0x06001516 RID: 5398 RVA: 0x00003788 File Offset: 0x00001988
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref JobPrepareAgentRaycasts job, EntityManager entityManager)
			{
			}

			// Token: 0x04000EAD RID: 3757
			public JobPrepareAgentRaycasts.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

			// Token: 0x04000EAE RID: 3758
			public EntityQuery DefaultQuery;

			// Token: 0x02000347 RID: 839
			public struct TypeHandle
			{
				// Token: 0x06001517 RID: 5399 RVA: 0x00078F40 File Offset: 0x00077140
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					this.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>(false);
					this.__Pathfinding_ECS_AgentCylinderShape_RO_ComponentTypeHandle = state.GetComponentTypeHandle<AgentCylinderShape>(true);
					this.__Pathfinding_ECS_AgentMovementPlane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<AgentMovementPlane>(true);
					this.__Pathfinding_ECS_MovementState_RW_ComponentTypeHandle = state.GetComponentTypeHandle<MovementState>(false);
					this.__Pathfinding_ECS_MovementSettings_RO_ComponentTypeHandle = state.GetComponentTypeHandle<MovementSettings>(true);
					this.__Pathfinding_ECS_ResolvedMovement_RW_ComponentTypeHandle = state.GetComponentTypeHandle<ResolvedMovement>(false);
					this.__Pathfinding_ECS_MovementStatistics_RW_ComponentTypeHandle = state.GetComponentTypeHandle<MovementStatistics>(false);
					this.__Pathfinding_ECS_GravityState_RW_ComponentTypeHandle = state.GetComponentTypeHandle<GravityState>(false);
					this.__Pathfinding_ECS_PhysicsSceneRef_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<PhysicsSceneRef>();
				}

				// Token: 0x06001518 RID: 5400 RVA: 0x00078FC4 File Offset: 0x000771C4
				public void Update(ref SystemState state)
				{
					this.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_AgentCylinderShape_RO_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_AgentMovementPlane_RO_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_MovementState_RW_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_MovementSettings_RO_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_ResolvedMovement_RW_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_MovementStatistics_RW_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_GravityState_RW_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_PhysicsSceneRef_SharedComponentTypeHandle.Update(ref state);
				}

				// Token: 0x04000EAF RID: 3759
				public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

				// Token: 0x04000EB0 RID: 3760
				[ReadOnly]
				public ComponentTypeHandle<AgentCylinderShape> __Pathfinding_ECS_AgentCylinderShape_RO_ComponentTypeHandle;

				// Token: 0x04000EB1 RID: 3761
				[ReadOnly]
				public ComponentTypeHandle<AgentMovementPlane> __Pathfinding_ECS_AgentMovementPlane_RO_ComponentTypeHandle;

				// Token: 0x04000EB2 RID: 3762
				public ComponentTypeHandle<MovementState> __Pathfinding_ECS_MovementState_RW_ComponentTypeHandle;

				// Token: 0x04000EB3 RID: 3763
				[ReadOnly]
				public ComponentTypeHandle<MovementSettings> __Pathfinding_ECS_MovementSettings_RO_ComponentTypeHandle;

				// Token: 0x04000EB4 RID: 3764
				public ComponentTypeHandle<ResolvedMovement> __Pathfinding_ECS_ResolvedMovement_RW_ComponentTypeHandle;

				// Token: 0x04000EB5 RID: 3765
				public ComponentTypeHandle<MovementStatistics> __Pathfinding_ECS_MovementStatistics_RW_ComponentTypeHandle;

				// Token: 0x04000EB6 RID: 3766
				public ComponentTypeHandle<GravityState> __Pathfinding_ECS_GravityState_RW_ComponentTypeHandle;

				// Token: 0x04000EB7 RID: 3767
				[ReadOnly]
				public SharedComponentTypeHandle<PhysicsSceneRef> __Pathfinding_ECS_PhysicsSceneRef_SharedComponentTypeHandle;
			}
		}

		// Token: 0x02000348 RID: 840
		public struct InternalCompiler
		{
			// Token: 0x06001519 RID: 5401 RVA: 0x00003788 File Offset: 0x00001988
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void CheckForErrors(int scheduleType)
			{
			}
		}
	}
}
