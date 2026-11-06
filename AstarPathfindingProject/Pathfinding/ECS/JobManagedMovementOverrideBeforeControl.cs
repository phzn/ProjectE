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
	// Token: 0x0200032D RID: 813
	[CompilerGenerated]
	public struct JobManagedMovementOverrideBeforeControl : IJobEntity, IJobChunk
	{
		// Token: 0x06001410 RID: 5136 RVA: 0x00076324 File Offset: 0x00074524
		public void Execute(ManagedMovementOverrideBeforeControl managedOverride, Entity entity, ref LocalTransform localTransform, ref AgentCylinderShape shape, ref AgentMovementPlane movementPlane, ref DestinationPoint destination, ref MovementState movementState, ref MovementSettings movementSettings)
		{
			if (managedOverride.callback != null)
			{
				managedOverride.callback(entity, this.dt, ref localTransform, ref shape, ref movementPlane, ref destination, ref movementState, ref movementSettings);
				movementState.pathTracerVersion -= 1;
			}
		}

		// Token: 0x06001411 RID: 5137 RVA: 0x00076364 File Offset: 0x00074564
		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			ManagedComponentAccessor<ManagedMovementOverrideBeforeControl> managedComponentAccessor = chunk.GetManagedComponentAccessor<ManagedMovementOverrideBeforeControl>(ref this.__TypeHandle.__Pathfinding_ECS_ManagedMovementOverrideBeforeControl_RW_ComponentTypeHandle, this.__EntityManager);
			IntPtr intPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, this.__TypeHandle.__Unity_Entities_Entity_TypeHandle);
			IntPtr intPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<LocalTransform>(chunk, ref this.__TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
			IntPtr intPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<AgentCylinderShape>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_AgentCylinderShape_RW_ComponentTypeHandle);
			IntPtr intPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<AgentMovementPlane>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_AgentMovementPlane_RW_ComponentTypeHandle);
			IntPtr intPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<DestinationPoint>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_DestinationPoint_RW_ComponentTypeHandle);
			IntPtr intPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<MovementState>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_MovementState_RW_ComponentTypeHandle);
			IntPtr intPtr7 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<MovementSettings>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_MovementSettings_RW_ComponentTypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					ManagedMovementOverrideBeforeControl managedMovementOverrideBeforeControl = managedComponentAccessor[i];
					Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(intPtr, i);
					ref LocalTransform ptr = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr2, i);
					ref AgentCylinderShape ptr2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr3, i);
					ref AgentMovementPlane ptr3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr4, i);
					ref DestinationPoint ptr4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<DestinationPoint>(intPtr5, i);
					ref MovementState ptr5 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementState>(intPtr6, i);
					ref MovementSettings ptr6 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementSettings>(intPtr7, i);
					this.Execute(managedMovementOverrideBeforeControl, entity, ref ptr, ref ptr2, ref ptr3, ref ptr4, ref ptr5, ref ptr6);
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
						ManagedMovementOverrideBeforeControl managedMovementOverrideBeforeControl2 = managedComponentAccessor[j];
						Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(intPtr, j);
						ref LocalTransform ptr7 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr2, j);
						ref AgentCylinderShape ptr8 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr3, j);
						ref AgentMovementPlane ptr9 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr4, j);
						ref DestinationPoint ptr10 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<DestinationPoint>(intPtr5, j);
						ref MovementState ptr11 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementState>(intPtr6, j);
						ref MovementSettings ptr12 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementSettings>(intPtr7, j);
						this.Execute(managedMovementOverrideBeforeControl2, entity2, ref ptr7, ref ptr8, ref ptr9, ref ptr10, ref ptr11, ref ptr12);
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
					ManagedMovementOverrideBeforeControl managedMovementOverrideBeforeControl3 = managedComponentAccessor[k];
					Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(intPtr, k);
					ref LocalTransform ptr13 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr2, k);
					ref AgentCylinderShape ptr14 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr3, k);
					ref AgentMovementPlane ptr15 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr4, k);
					ref DestinationPoint ptr16 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<DestinationPoint>(intPtr5, k);
					ref MovementState ptr17 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementState>(intPtr6, k);
					ref MovementSettings ptr18 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementSettings>(intPtr7, k);
					this.Execute(managedMovementOverrideBeforeControl3, entity3, ref ptr13, ref ptr14, ref ptr15, ref ptr16, ref ptr17, ref ptr18);
					num++;
				}
				num3 >>= 1;
			}
			num3 = chunkEnabledMask.ULong1;
			for (int l = 64; l < count; l++)
			{
				if ((num3 & 1UL) != 0UL)
				{
					ManagedMovementOverrideBeforeControl managedMovementOverrideBeforeControl4 = managedComponentAccessor[l];
					Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(intPtr, l);
					ref LocalTransform ptr19 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr2, l);
					ref AgentCylinderShape ptr20 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr3, l);
					ref AgentMovementPlane ptr21 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr4, l);
					ref DestinationPoint ptr22 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<DestinationPoint>(intPtr5, l);
					ref MovementState ptr23 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementState>(intPtr6, l);
					ref MovementSettings ptr24 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementSettings>(intPtr7, l);
					this.Execute(managedMovementOverrideBeforeControl4, entity4, ref ptr19, ref ptr20, ref ptr21, ref ptr22, ref ptr23, ref ptr24);
					num++;
				}
				num3 >>= 1;
			}
		}

		// Token: 0x06001412 RID: 5138 RVA: 0x000766E0 File Offset: 0x000748E0
		private JobHandle __ThrowCodeGenException()
		{
			throw new Exception("This method should have been replaced by source gen.");
		}

		// Token: 0x06001413 RID: 5139 RVA: 0x000766F7 File Offset: 0x000748F7
		public void Run()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001414 RID: 5140 RVA: 0x000766F7 File Offset: 0x000748F7
		public void RunByRef()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001415 RID: 5141 RVA: 0x000766F7 File Offset: 0x000748F7
		public void Run(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001416 RID: 5142 RVA: 0x000766F7 File Offset: 0x000748F7
		public void RunByRef(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001417 RID: 5143 RVA: 0x00076700 File Offset: 0x00074900
		public JobHandle Schedule(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001418 RID: 5144 RVA: 0x00076700 File Offset: 0x00074900
		public JobHandle ScheduleByRef(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001419 RID: 5145 RVA: 0x00076700 File Offset: 0x00074900
		public JobHandle Schedule(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x0600141A RID: 5146 RVA: 0x00076700 File Offset: 0x00074900
		public JobHandle ScheduleByRef(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x0600141B RID: 5147 RVA: 0x000766F7 File Offset: 0x000748F7
		public void Schedule()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x0600141C RID: 5148 RVA: 0x000766F7 File Offset: 0x000748F7
		public void ScheduleByRef()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x0600141D RID: 5149 RVA: 0x000766F7 File Offset: 0x000748F7
		public void Schedule(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x0600141E RID: 5150 RVA: 0x000766F7 File Offset: 0x000748F7
		public void ScheduleByRef(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x0600141F RID: 5151 RVA: 0x00076700 File Offset: 0x00074900
		public JobHandle ScheduleParallel(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001420 RID: 5152 RVA: 0x00076700 File Offset: 0x00074900
		public JobHandle ScheduleParallelByRef(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001421 RID: 5153 RVA: 0x00076700 File Offset: 0x00074900
		public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001422 RID: 5154 RVA: 0x00076700 File Offset: 0x00074900
		public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001423 RID: 5155 RVA: 0x00076700 File Offset: 0x00074900
		public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001424 RID: 5156 RVA: 0x00076700 File Offset: 0x00074900
		public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001425 RID: 5157 RVA: 0x000766F7 File Offset: 0x000748F7
		public void ScheduleParallel()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001426 RID: 5158 RVA: 0x000766F7 File Offset: 0x000748F7
		public void ScheduleParallelByRef()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001427 RID: 5159 RVA: 0x000766F7 File Offset: 0x000748F7
		public void ScheduleParallel(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001428 RID: 5160 RVA: 0x000766F7 File Offset: 0x000748F7
		public void ScheduleParallelByRef(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001429 RID: 5161 RVA: 0x00076708 File Offset: 0x00074908
		void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
		}

		// Token: 0x04000E5D RID: 3677
		public float dt;

		// Token: 0x04000E5E RID: 3678
		private JobManagedMovementOverrideBeforeControl.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		// Token: 0x04000E5F RID: 3679
		public EntityManager __EntityManager;

		// Token: 0x0200032E RID: 814
		public struct InternalCompilerQueryAndHandleData
		{
			// Token: 0x0600142A RID: 5162 RVA: 0x00076718 File Offset: 0x00074918
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				this.DefaultQuery = entityQueryBuilder.WithAllRW<ManagedMovementOverrideBeforeControl>().WithAllRW<LocalTransform>().WithAllRW<AgentCylinderShape>()
					.WithAllRW<AgentMovementPlane>()
					.WithAllRW<DestinationPoint>()
					.WithAllRW<MovementState>()
					.WithAllRW<MovementSettings>()
					.Build(ref state);
				entityQueryBuilder.Reset();
				entityQueryBuilder.Dispose();
			}

			// Token: 0x0600142B RID: 5163 RVA: 0x00076787 File Offset: 0x00074987
			public void Init(ref SystemState state, bool assignDefaultQuery)
			{
				if (assignDefaultQuery)
				{
					this.__AssignQueries(ref state);
				}
				this.__TypeHandle.__AssignHandles(ref state);
			}

			// Token: 0x0600142C RID: 5164 RVA: 0x0007679F File Offset: 0x0007499F
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Run(ref JobManagedMovementOverrideBeforeControl job, EntityQuery query)
			{
				job.__TypeHandle = this.__TypeHandle;
				InternalCompilerInterface.JobChunkInterface.RunByRefWithoutJobs<JobManagedMovementOverrideBeforeControl>(ref job, query);
			}

			// Token: 0x0600142D RID: 5165 RVA: 0x000767B4 File Offset: 0x000749B4
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref JobManagedMovementOverrideBeforeControl job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = this.__TypeHandle;
				return (ref job).ScheduleByRef<JobManagedMovementOverrideBeforeControl>(query, dependency);
			}

			// Token: 0x0600142E RID: 5166 RVA: 0x000767CA File Offset: 0x000749CA
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref JobManagedMovementOverrideBeforeControl job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = this.__TypeHandle;
				return (ref job).ScheduleParallelByRef<JobManagedMovementOverrideBeforeControl>(query, dependency);
			}

			// Token: 0x0600142F RID: 5167 RVA: 0x00003788 File Offset: 0x00001988
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref JobManagedMovementOverrideBeforeControl job, EntityQuery query, ref SystemState state)
			{
			}

			// Token: 0x06001430 RID: 5168 RVA: 0x000743B8 File Offset: 0x000725B8
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref JobManagedMovementOverrideBeforeControl job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			// Token: 0x06001431 RID: 5169 RVA: 0x000767E0 File Offset: 0x000749E0
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref JobManagedMovementOverrideBeforeControl job, EntityManager entityManager)
			{
				job.__EntityManager = entityManager;
			}

			// Token: 0x04000E60 RID: 3680
			public JobManagedMovementOverrideBeforeControl.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

			// Token: 0x04000E61 RID: 3681
			public EntityQuery DefaultQuery;

			// Token: 0x0200032F RID: 815
			public struct TypeHandle
			{
				// Token: 0x06001432 RID: 5170 RVA: 0x000767EC File Offset: 0x000749EC
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					this.__Pathfinding_ECS_ManagedMovementOverrideBeforeControl_RW_ComponentTypeHandle = state.EntityManager.GetComponentTypeHandle<ManagedMovementOverrideBeforeControl>(false);
					this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
					this.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>(false);
					this.__Pathfinding_ECS_AgentCylinderShape_RW_ComponentTypeHandle = state.GetComponentTypeHandle<AgentCylinderShape>(false);
					this.__Pathfinding_ECS_AgentMovementPlane_RW_ComponentTypeHandle = state.GetComponentTypeHandle<AgentMovementPlane>(false);
					this.__Pathfinding_ECS_DestinationPoint_RW_ComponentTypeHandle = state.GetComponentTypeHandle<DestinationPoint>(false);
					this.__Pathfinding_ECS_MovementState_RW_ComponentTypeHandle = state.GetComponentTypeHandle<MovementState>(false);
					this.__Pathfinding_ECS_MovementSettings_RW_ComponentTypeHandle = state.GetComponentTypeHandle<MovementSettings>(false);
				}

				// Token: 0x06001433 RID: 5171 RVA: 0x00076868 File Offset: 0x00074A68
				public void Update(ref SystemState state)
				{
					this.__Pathfinding_ECS_ManagedMovementOverrideBeforeControl_RW_ComponentTypeHandle.Update(ref state);
					this.__Unity_Entities_Entity_TypeHandle.Update(ref state);
					this.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_AgentCylinderShape_RW_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_AgentMovementPlane_RW_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_DestinationPoint_RW_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_MovementState_RW_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_MovementSettings_RW_ComponentTypeHandle.Update(ref state);
				}

				// Token: 0x04000E62 RID: 3682
				public ComponentTypeHandle<ManagedMovementOverrideBeforeControl> __Pathfinding_ECS_ManagedMovementOverrideBeforeControl_RW_ComponentTypeHandle;

				// Token: 0x04000E63 RID: 3683
				[ReadOnly]
				public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

				// Token: 0x04000E64 RID: 3684
				public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

				// Token: 0x04000E65 RID: 3685
				public ComponentTypeHandle<AgentCylinderShape> __Pathfinding_ECS_AgentCylinderShape_RW_ComponentTypeHandle;

				// Token: 0x04000E66 RID: 3686
				public ComponentTypeHandle<AgentMovementPlane> __Pathfinding_ECS_AgentMovementPlane_RW_ComponentTypeHandle;

				// Token: 0x04000E67 RID: 3687
				public ComponentTypeHandle<DestinationPoint> __Pathfinding_ECS_DestinationPoint_RW_ComponentTypeHandle;

				// Token: 0x04000E68 RID: 3688
				public ComponentTypeHandle<MovementState> __Pathfinding_ECS_MovementState_RW_ComponentTypeHandle;

				// Token: 0x04000E69 RID: 3689
				public ComponentTypeHandle<MovementSettings> __Pathfinding_ECS_MovementSettings_RW_ComponentTypeHandle;
			}
		}

		// Token: 0x02000330 RID: 816
		public struct InternalCompiler
		{
			// Token: 0x06001434 RID: 5172 RVA: 0x000768D5 File Offset: 0x00074AD5
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
