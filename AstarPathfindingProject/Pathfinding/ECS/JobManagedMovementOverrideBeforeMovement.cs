using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace Pathfinding.ECS
{
	// Token: 0x02000335 RID: 821
	[CompilerGenerated]
	public struct JobManagedMovementOverrideBeforeMovement : IJobEntity, IJobChunk
	{
		// Token: 0x0600145A RID: 5210 RVA: 0x00076F1C File Offset: 0x0007511C
		public void Execute(ManagedMovementOverrideBeforeMovement managedOverride, Entity entity, ref LocalTransform localTransform, ref AgentCylinderShape shape, ref AgentMovementPlane movementPlane, ref DestinationPoint destination, ref MovementState movementState, ref MovementSettings movementSettings, ref MovementControl movementControl, ref ResolvedMovement resolvedMovement)
		{
			if (managedOverride.callback != null)
			{
				managedOverride.callback(entity, this.dt, ref localTransform, ref shape, ref movementPlane, ref destination, ref movementState, ref movementSettings, ref movementControl, ref resolvedMovement);
				movementState.pathTracerVersion -= 1;
			}
		}

		// Token: 0x0600145B RID: 5211 RVA: 0x00076F60 File Offset: 0x00075160
		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			ManagedComponentAccessor<ManagedMovementOverrideBeforeMovement> managedComponentAccessor = chunk.GetManagedComponentAccessor<ManagedMovementOverrideBeforeMovement>(ref this.__TypeHandle.__Pathfinding_ECS_ManagedMovementOverrideBeforeMovement_RW_ComponentTypeHandle, this.__EntityManager);
			IntPtr intPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, this.__TypeHandle.__Unity_Entities_Entity_TypeHandle);
			IntPtr intPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<LocalTransform>(chunk, ref this.__TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
			IntPtr intPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<AgentCylinderShape>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_AgentCylinderShape_RW_ComponentTypeHandle);
			IntPtr intPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<AgentMovementPlane>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_AgentMovementPlane_RW_ComponentTypeHandle);
			IntPtr intPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<DestinationPoint>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_DestinationPoint_RW_ComponentTypeHandle);
			IntPtr intPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<MovementState>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_MovementState_RW_ComponentTypeHandle);
			IntPtr intPtr7 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<MovementSettings>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_MovementSettings_RW_ComponentTypeHandle);
			IntPtr intPtr8 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<MovementControl>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_MovementControl_RW_ComponentTypeHandle);
			IntPtr intPtr9 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<ResolvedMovement>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_ResolvedMovement_RW_ComponentTypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					ManagedMovementOverrideBeforeMovement managedMovementOverrideBeforeMovement = managedComponentAccessor[i];
					Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(intPtr, i);
					ref LocalTransform ptr = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr2, i);
					ref AgentCylinderShape ptr2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr3, i);
					ref AgentMovementPlane ptr3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr4, i);
					ref DestinationPoint ptr4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<DestinationPoint>(intPtr5, i);
					ref MovementState ptr5 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementState>(intPtr6, i);
					ref MovementSettings ptr6 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementSettings>(intPtr7, i);
					ref MovementControl ptr7 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementControl>(intPtr8, i);
					ref ResolvedMovement ptr8 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<ResolvedMovement>(intPtr9, i);
					this.Execute(managedMovementOverrideBeforeMovement, entity, ref ptr, ref ptr2, ref ptr3, ref ptr4, ref ptr5, ref ptr6, ref ptr7, ref ptr8);
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
						ManagedMovementOverrideBeforeMovement managedMovementOverrideBeforeMovement2 = managedComponentAccessor[j];
						Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(intPtr, j);
						ref LocalTransform ptr9 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr2, j);
						ref AgentCylinderShape ptr10 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr3, j);
						ref AgentMovementPlane ptr11 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr4, j);
						ref DestinationPoint ptr12 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<DestinationPoint>(intPtr5, j);
						ref MovementState ptr13 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementState>(intPtr6, j);
						ref MovementSettings ptr14 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementSettings>(intPtr7, j);
						ref MovementControl ptr15 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementControl>(intPtr8, j);
						ref ResolvedMovement ptr16 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<ResolvedMovement>(intPtr9, j);
						this.Execute(managedMovementOverrideBeforeMovement2, entity2, ref ptr9, ref ptr10, ref ptr11, ref ptr12, ref ptr13, ref ptr14, ref ptr15, ref ptr16);
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
					ManagedMovementOverrideBeforeMovement managedMovementOverrideBeforeMovement3 = managedComponentAccessor[k];
					Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(intPtr, k);
					ref LocalTransform ptr17 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr2, k);
					ref AgentCylinderShape ptr18 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr3, k);
					ref AgentMovementPlane ptr19 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr4, k);
					ref DestinationPoint ptr20 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<DestinationPoint>(intPtr5, k);
					ref MovementState ptr21 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementState>(intPtr6, k);
					ref MovementSettings ptr22 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementSettings>(intPtr7, k);
					ref MovementControl ptr23 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementControl>(intPtr8, k);
					ref ResolvedMovement ptr24 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<ResolvedMovement>(intPtr9, k);
					this.Execute(managedMovementOverrideBeforeMovement3, entity3, ref ptr17, ref ptr18, ref ptr19, ref ptr20, ref ptr21, ref ptr22, ref ptr23, ref ptr24);
					num++;
				}
				num3 >>= 1;
			}
			num3 = chunkEnabledMask.ULong1;
			for (int l = 64; l < count; l++)
			{
				if ((num3 & 1UL) != 0UL)
				{
					ManagedMovementOverrideBeforeMovement managedMovementOverrideBeforeMovement4 = managedComponentAccessor[l];
					Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(intPtr, l);
					ref LocalTransform ptr25 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr2, l);
					ref AgentCylinderShape ptr26 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr3, l);
					ref AgentMovementPlane ptr27 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr4, l);
					ref DestinationPoint ptr28 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<DestinationPoint>(intPtr5, l);
					ref MovementState ptr29 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementState>(intPtr6, l);
					ref MovementSettings ptr30 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementSettings>(intPtr7, l);
					ref MovementControl ptr31 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementControl>(intPtr8, l);
					ref ResolvedMovement ptr32 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<ResolvedMovement>(intPtr9, l);
					this.Execute(managedMovementOverrideBeforeMovement4, entity4, ref ptr25, ref ptr26, ref ptr27, ref ptr28, ref ptr29, ref ptr30, ref ptr31, ref ptr32);
					num++;
				}
				num3 >>= 1;
			}
		}

		// Token: 0x0600145C RID: 5212 RVA: 0x00077384 File Offset: 0x00075584
		private JobHandle __ThrowCodeGenException()
		{
			throw new Exception("This method should have been replaced by source gen.");
		}

		// Token: 0x0600145D RID: 5213 RVA: 0x0007739B File Offset: 0x0007559B
		public void Run()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x0600145E RID: 5214 RVA: 0x0007739B File Offset: 0x0007559B
		public void RunByRef()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x0600145F RID: 5215 RVA: 0x0007739B File Offset: 0x0007559B
		public void Run(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001460 RID: 5216 RVA: 0x0007739B File Offset: 0x0007559B
		public void RunByRef(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001461 RID: 5217 RVA: 0x000773A4 File Offset: 0x000755A4
		public JobHandle Schedule(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001462 RID: 5218 RVA: 0x000773A4 File Offset: 0x000755A4
		public JobHandle ScheduleByRef(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001463 RID: 5219 RVA: 0x000773A4 File Offset: 0x000755A4
		public JobHandle Schedule(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001464 RID: 5220 RVA: 0x000773A4 File Offset: 0x000755A4
		public JobHandle ScheduleByRef(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001465 RID: 5221 RVA: 0x0007739B File Offset: 0x0007559B
		public void Schedule()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001466 RID: 5222 RVA: 0x0007739B File Offset: 0x0007559B
		public void ScheduleByRef()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001467 RID: 5223 RVA: 0x0007739B File Offset: 0x0007559B
		public void Schedule(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001468 RID: 5224 RVA: 0x0007739B File Offset: 0x0007559B
		public void ScheduleByRef(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001469 RID: 5225 RVA: 0x000773A4 File Offset: 0x000755A4
		public JobHandle ScheduleParallel(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x0600146A RID: 5226 RVA: 0x000773A4 File Offset: 0x000755A4
		public JobHandle ScheduleParallelByRef(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x0600146B RID: 5227 RVA: 0x000773A4 File Offset: 0x000755A4
		public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x0600146C RID: 5228 RVA: 0x000773A4 File Offset: 0x000755A4
		public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x0600146D RID: 5229 RVA: 0x000773A4 File Offset: 0x000755A4
		public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x0600146E RID: 5230 RVA: 0x000773A4 File Offset: 0x000755A4
		public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x0600146F RID: 5231 RVA: 0x0007739B File Offset: 0x0007559B
		public void ScheduleParallel()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001470 RID: 5232 RVA: 0x0007739B File Offset: 0x0007559B
		public void ScheduleParallelByRef()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001471 RID: 5233 RVA: 0x0007739B File Offset: 0x0007559B
		public void ScheduleParallel(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001472 RID: 5234 RVA: 0x0007739B File Offset: 0x0007559B
		public void ScheduleParallelByRef(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001473 RID: 5235 RVA: 0x000773AC File Offset: 0x000755AC
		void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
		}

		// Token: 0x04000E78 RID: 3704
		public float dt;

		// Token: 0x04000E79 RID: 3705
		private JobManagedMovementOverrideBeforeMovement.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		// Token: 0x04000E7A RID: 3706
		public EntityManager __EntityManager;

		// Token: 0x02000336 RID: 822
		public struct InternalCompilerQueryAndHandleData
		{
			// Token: 0x06001474 RID: 5236 RVA: 0x000773BC File Offset: 0x000755BC
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				this.DefaultQuery = entityQueryBuilder.WithAllRW<ManagedMovementOverrideBeforeMovement>().WithAllRW<LocalTransform>().WithAllRW<AgentCylinderShape>()
					.WithAllRW<AgentMovementPlane>()
					.WithAllRW<DestinationPoint>()
					.WithAllRW<MovementState>()
					.WithAllRW<MovementSettings>()
					.WithAllRW<MovementControl>()
					.WithAllRW<ResolvedMovement>()
					.Build(ref state);
				entityQueryBuilder.Reset();
				entityQueryBuilder.Dispose();
			}

			// Token: 0x06001475 RID: 5237 RVA: 0x0007743B File Offset: 0x0007563B
			public void Init(ref SystemState state, bool assignDefaultQuery)
			{
				if (assignDefaultQuery)
				{
					this.__AssignQueries(ref state);
				}
				this.__TypeHandle.__AssignHandles(ref state);
			}

			// Token: 0x06001476 RID: 5238 RVA: 0x00077453 File Offset: 0x00075653
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Run(ref JobManagedMovementOverrideBeforeMovement job, EntityQuery query)
			{
				job.__TypeHandle = this.__TypeHandle;
				InternalCompilerInterface.JobChunkInterface.RunByRefWithoutJobs<JobManagedMovementOverrideBeforeMovement>(ref job, query);
			}

			// Token: 0x06001477 RID: 5239 RVA: 0x00077468 File Offset: 0x00075668
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref JobManagedMovementOverrideBeforeMovement job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = this.__TypeHandle;
				return (ref job).ScheduleByRef<JobManagedMovementOverrideBeforeMovement>(query, dependency);
			}

			// Token: 0x06001478 RID: 5240 RVA: 0x0007747E File Offset: 0x0007567E
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref JobManagedMovementOverrideBeforeMovement job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = this.__TypeHandle;
				return (ref job).ScheduleParallelByRef<JobManagedMovementOverrideBeforeMovement>(query, dependency);
			}

			// Token: 0x06001479 RID: 5241 RVA: 0x00003788 File Offset: 0x00001988
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref JobManagedMovementOverrideBeforeMovement job, EntityQuery query, ref SystemState state)
			{
			}

			// Token: 0x0600147A RID: 5242 RVA: 0x000743B8 File Offset: 0x000725B8
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref JobManagedMovementOverrideBeforeMovement job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			// Token: 0x0600147B RID: 5243 RVA: 0x00077494 File Offset: 0x00075694
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref JobManagedMovementOverrideBeforeMovement job, EntityManager entityManager)
			{
				job.__EntityManager = entityManager;
			}

			// Token: 0x04000E7B RID: 3707
			public JobManagedMovementOverrideBeforeMovement.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

			// Token: 0x04000E7C RID: 3708
			public EntityQuery DefaultQuery;

			// Token: 0x02000337 RID: 823
			public struct TypeHandle
			{
				// Token: 0x0600147C RID: 5244 RVA: 0x000774A0 File Offset: 0x000756A0
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					this.__Pathfinding_ECS_ManagedMovementOverrideBeforeMovement_RW_ComponentTypeHandle = state.EntityManager.GetComponentTypeHandle<ManagedMovementOverrideBeforeMovement>(false);
					this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
					this.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>(false);
					this.__Pathfinding_ECS_AgentCylinderShape_RW_ComponentTypeHandle = state.GetComponentTypeHandle<AgentCylinderShape>(false);
					this.__Pathfinding_ECS_AgentMovementPlane_RW_ComponentTypeHandle = state.GetComponentTypeHandle<AgentMovementPlane>(false);
					this.__Pathfinding_ECS_DestinationPoint_RW_ComponentTypeHandle = state.GetComponentTypeHandle<DestinationPoint>(false);
					this.__Pathfinding_ECS_MovementState_RW_ComponentTypeHandle = state.GetComponentTypeHandle<MovementState>(false);
					this.__Pathfinding_ECS_MovementSettings_RW_ComponentTypeHandle = state.GetComponentTypeHandle<MovementSettings>(false);
					this.__Pathfinding_ECS_MovementControl_RW_ComponentTypeHandle = state.GetComponentTypeHandle<MovementControl>(false);
					this.__Pathfinding_ECS_ResolvedMovement_RW_ComponentTypeHandle = state.GetComponentTypeHandle<ResolvedMovement>(false);
				}

				// Token: 0x0600147D RID: 5245 RVA: 0x00077538 File Offset: 0x00075738
				public void Update(ref SystemState state)
				{
					this.__Pathfinding_ECS_ManagedMovementOverrideBeforeMovement_RW_ComponentTypeHandle.Update(ref state);
					this.__Unity_Entities_Entity_TypeHandle.Update(ref state);
					this.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_AgentCylinderShape_RW_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_AgentMovementPlane_RW_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_DestinationPoint_RW_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_MovementState_RW_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_MovementSettings_RW_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_MovementControl_RW_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_ResolvedMovement_RW_ComponentTypeHandle.Update(ref state);
				}

				// Token: 0x04000E7D RID: 3709
				public ComponentTypeHandle<ManagedMovementOverrideBeforeMovement> __Pathfinding_ECS_ManagedMovementOverrideBeforeMovement_RW_ComponentTypeHandle;

				// Token: 0x04000E7E RID: 3710
				[ReadOnly]
				public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

				// Token: 0x04000E7F RID: 3711
				public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

				// Token: 0x04000E80 RID: 3712
				public ComponentTypeHandle<AgentCylinderShape> __Pathfinding_ECS_AgentCylinderShape_RW_ComponentTypeHandle;

				// Token: 0x04000E81 RID: 3713
				public ComponentTypeHandle<AgentMovementPlane> __Pathfinding_ECS_AgentMovementPlane_RW_ComponentTypeHandle;

				// Token: 0x04000E82 RID: 3714
				public ComponentTypeHandle<DestinationPoint> __Pathfinding_ECS_DestinationPoint_RW_ComponentTypeHandle;

				// Token: 0x04000E83 RID: 3715
				public ComponentTypeHandle<MovementState> __Pathfinding_ECS_MovementState_RW_ComponentTypeHandle;

				// Token: 0x04000E84 RID: 3716
				public ComponentTypeHandle<MovementSettings> __Pathfinding_ECS_MovementSettings_RW_ComponentTypeHandle;

				// Token: 0x04000E85 RID: 3717
				public ComponentTypeHandle<MovementControl> __Pathfinding_ECS_MovementControl_RW_ComponentTypeHandle;

				// Token: 0x04000E86 RID: 3718
				public ComponentTypeHandle<ResolvedMovement> __Pathfinding_ECS_ResolvedMovement_RW_ComponentTypeHandle;
			}
		}

		// Token: 0x02000338 RID: 824
		public struct InternalCompiler
		{
			// Token: 0x0600147E RID: 5246 RVA: 0x000768D5 File Offset: 0x00074AD5
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void CheckForErrors(int scheduleType)
			{
				if (scheduleType == 2)
				{
					throw new InvalidOperationException("Tried to ScheduleParallel a job with a managed execute signature. Please use .Run or .Schedule instead.");
				}
			}
		}
	}
}
