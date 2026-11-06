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
	// Token: 0x02000331 RID: 817
	[CompilerGenerated]
	public struct JobManagedMovementOverrideAfterControl : IJobEntity, IJobChunk
	{
		// Token: 0x06001435 RID: 5173 RVA: 0x000768E8 File Offset: 0x00074AE8
		public void Execute(ManagedMovementOverrideAfterControl managedOverride, Entity entity, ref LocalTransform localTransform, ref AgentCylinderShape shape, ref AgentMovementPlane movementPlane, ref DestinationPoint destination, ref MovementState movementState, ref MovementSettings movementSettings, ref MovementControl movementControl)
		{
			if (managedOverride.callback != null)
			{
				managedOverride.callback(entity, this.dt, ref localTransform, ref shape, ref movementPlane, ref destination, ref movementState, ref movementSettings, ref movementControl);
				movementState.pathTracerVersion -= 1;
			}
		}

		// Token: 0x06001436 RID: 5174 RVA: 0x0007692C File Offset: 0x00074B2C
		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			ManagedComponentAccessor<ManagedMovementOverrideAfterControl> managedComponentAccessor = chunk.GetManagedComponentAccessor<ManagedMovementOverrideAfterControl>(ref this.__TypeHandle.__Pathfinding_ECS_ManagedMovementOverrideAfterControl_RW_ComponentTypeHandle, this.__EntityManager);
			IntPtr intPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, this.__TypeHandle.__Unity_Entities_Entity_TypeHandle);
			IntPtr intPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<LocalTransform>(chunk, ref this.__TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
			IntPtr intPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<AgentCylinderShape>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_AgentCylinderShape_RW_ComponentTypeHandle);
			IntPtr intPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<AgentMovementPlane>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_AgentMovementPlane_RW_ComponentTypeHandle);
			IntPtr intPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<DestinationPoint>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_DestinationPoint_RW_ComponentTypeHandle);
			IntPtr intPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<MovementState>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_MovementState_RW_ComponentTypeHandle);
			IntPtr intPtr7 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<MovementSettings>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_MovementSettings_RW_ComponentTypeHandle);
			IntPtr intPtr8 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<MovementControl>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_MovementControl_RW_ComponentTypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					ManagedMovementOverrideAfterControl managedMovementOverrideAfterControl = managedComponentAccessor[i];
					Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(intPtr, i);
					ref LocalTransform ptr = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr2, i);
					ref AgentCylinderShape ptr2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr3, i);
					ref AgentMovementPlane ptr3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr4, i);
					ref DestinationPoint ptr4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<DestinationPoint>(intPtr5, i);
					ref MovementState ptr5 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementState>(intPtr6, i);
					ref MovementSettings ptr6 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementSettings>(intPtr7, i);
					ref MovementControl ptr7 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementControl>(intPtr8, i);
					this.Execute(managedMovementOverrideAfterControl, entity, ref ptr, ref ptr2, ref ptr3, ref ptr4, ref ptr5, ref ptr6, ref ptr7);
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
						ManagedMovementOverrideAfterControl managedMovementOverrideAfterControl2 = managedComponentAccessor[j];
						Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(intPtr, j);
						ref LocalTransform ptr8 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr2, j);
						ref AgentCylinderShape ptr9 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr3, j);
						ref AgentMovementPlane ptr10 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr4, j);
						ref DestinationPoint ptr11 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<DestinationPoint>(intPtr5, j);
						ref MovementState ptr12 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementState>(intPtr6, j);
						ref MovementSettings ptr13 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementSettings>(intPtr7, j);
						ref MovementControl ptr14 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementControl>(intPtr8, j);
						this.Execute(managedMovementOverrideAfterControl2, entity2, ref ptr8, ref ptr9, ref ptr10, ref ptr11, ref ptr12, ref ptr13, ref ptr14);
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
					ManagedMovementOverrideAfterControl managedMovementOverrideAfterControl3 = managedComponentAccessor[k];
					Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(intPtr, k);
					ref LocalTransform ptr15 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr2, k);
					ref AgentCylinderShape ptr16 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr3, k);
					ref AgentMovementPlane ptr17 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr4, k);
					ref DestinationPoint ptr18 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<DestinationPoint>(intPtr5, k);
					ref MovementState ptr19 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementState>(intPtr6, k);
					ref MovementSettings ptr20 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementSettings>(intPtr7, k);
					ref MovementControl ptr21 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementControl>(intPtr8, k);
					this.Execute(managedMovementOverrideAfterControl3, entity3, ref ptr15, ref ptr16, ref ptr17, ref ptr18, ref ptr19, ref ptr20, ref ptr21);
					num++;
				}
				num3 >>= 1;
			}
			num3 = chunkEnabledMask.ULong1;
			for (int l = 64; l < count; l++)
			{
				if ((num3 & 1UL) != 0UL)
				{
					ManagedMovementOverrideAfterControl managedMovementOverrideAfterControl4 = managedComponentAccessor[l];
					Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(intPtr, l);
					ref LocalTransform ptr22 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr2, l);
					ref AgentCylinderShape ptr23 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr3, l);
					ref AgentMovementPlane ptr24 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr4, l);
					ref DestinationPoint ptr25 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<DestinationPoint>(intPtr5, l);
					ref MovementState ptr26 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementState>(intPtr6, l);
					ref MovementSettings ptr27 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementSettings>(intPtr7, l);
					ref MovementControl ptr28 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementControl>(intPtr8, l);
					this.Execute(managedMovementOverrideAfterControl4, entity4, ref ptr22, ref ptr23, ref ptr24, ref ptr25, ref ptr26, ref ptr27, ref ptr28);
					num++;
				}
				num3 >>= 1;
			}
		}

		// Token: 0x06001437 RID: 5175 RVA: 0x00076D00 File Offset: 0x00074F00
		private JobHandle __ThrowCodeGenException()
		{
			throw new Exception("This method should have been replaced by source gen.");
		}

		// Token: 0x06001438 RID: 5176 RVA: 0x00076D17 File Offset: 0x00074F17
		public void Run()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001439 RID: 5177 RVA: 0x00076D17 File Offset: 0x00074F17
		public void RunByRef()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x0600143A RID: 5178 RVA: 0x00076D17 File Offset: 0x00074F17
		public void Run(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x0600143B RID: 5179 RVA: 0x00076D17 File Offset: 0x00074F17
		public void RunByRef(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x0600143C RID: 5180 RVA: 0x00076D20 File Offset: 0x00074F20
		public JobHandle Schedule(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x0600143D RID: 5181 RVA: 0x00076D20 File Offset: 0x00074F20
		public JobHandle ScheduleByRef(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x0600143E RID: 5182 RVA: 0x00076D20 File Offset: 0x00074F20
		public JobHandle Schedule(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x0600143F RID: 5183 RVA: 0x00076D20 File Offset: 0x00074F20
		public JobHandle ScheduleByRef(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001440 RID: 5184 RVA: 0x00076D17 File Offset: 0x00074F17
		public void Schedule()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001441 RID: 5185 RVA: 0x00076D17 File Offset: 0x00074F17
		public void ScheduleByRef()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001442 RID: 5186 RVA: 0x00076D17 File Offset: 0x00074F17
		public void Schedule(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001443 RID: 5187 RVA: 0x00076D17 File Offset: 0x00074F17
		public void ScheduleByRef(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001444 RID: 5188 RVA: 0x00076D20 File Offset: 0x00074F20
		public JobHandle ScheduleParallel(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001445 RID: 5189 RVA: 0x00076D20 File Offset: 0x00074F20
		public JobHandle ScheduleParallelByRef(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001446 RID: 5190 RVA: 0x00076D20 File Offset: 0x00074F20
		public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001447 RID: 5191 RVA: 0x00076D20 File Offset: 0x00074F20
		public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001448 RID: 5192 RVA: 0x00076D20 File Offset: 0x00074F20
		public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001449 RID: 5193 RVA: 0x00076D20 File Offset: 0x00074F20
		public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x0600144A RID: 5194 RVA: 0x00076D17 File Offset: 0x00074F17
		public void ScheduleParallel()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x0600144B RID: 5195 RVA: 0x00076D17 File Offset: 0x00074F17
		public void ScheduleParallelByRef()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x0600144C RID: 5196 RVA: 0x00076D17 File Offset: 0x00074F17
		public void ScheduleParallel(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x0600144D RID: 5197 RVA: 0x00076D17 File Offset: 0x00074F17
		public void ScheduleParallelByRef(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x0600144E RID: 5198 RVA: 0x00076D28 File Offset: 0x00074F28
		void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
		}

		// Token: 0x04000E6A RID: 3690
		public float dt;

		// Token: 0x04000E6B RID: 3691
		private JobManagedMovementOverrideAfterControl.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		// Token: 0x04000E6C RID: 3692
		public EntityManager __EntityManager;

		// Token: 0x02000332 RID: 818
		public struct InternalCompilerQueryAndHandleData
		{
			// Token: 0x0600144F RID: 5199 RVA: 0x00076D38 File Offset: 0x00074F38
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				this.DefaultQuery = entityQueryBuilder.WithAllRW<ManagedMovementOverrideAfterControl>().WithAllRW<LocalTransform>().WithAllRW<AgentCylinderShape>()
					.WithAllRW<AgentMovementPlane>()
					.WithAllRW<DestinationPoint>()
					.WithAllRW<MovementState>()
					.WithAllRW<MovementSettings>()
					.WithAllRW<MovementControl>()
					.Build(ref state);
				entityQueryBuilder.Reset();
				entityQueryBuilder.Dispose();
			}

			// Token: 0x06001450 RID: 5200 RVA: 0x00076DAF File Offset: 0x00074FAF
			public void Init(ref SystemState state, bool assignDefaultQuery)
			{
				if (assignDefaultQuery)
				{
					this.__AssignQueries(ref state);
				}
				this.__TypeHandle.__AssignHandles(ref state);
			}

			// Token: 0x06001451 RID: 5201 RVA: 0x00076DC7 File Offset: 0x00074FC7
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Run(ref JobManagedMovementOverrideAfterControl job, EntityQuery query)
			{
				job.__TypeHandle = this.__TypeHandle;
				InternalCompilerInterface.JobChunkInterface.RunByRefWithoutJobs<JobManagedMovementOverrideAfterControl>(ref job, query);
			}

			// Token: 0x06001452 RID: 5202 RVA: 0x00076DDC File Offset: 0x00074FDC
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref JobManagedMovementOverrideAfterControl job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = this.__TypeHandle;
				return (ref job).ScheduleByRef<JobManagedMovementOverrideAfterControl>(query, dependency);
			}

			// Token: 0x06001453 RID: 5203 RVA: 0x00076DF2 File Offset: 0x00074FF2
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref JobManagedMovementOverrideAfterControl job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = this.__TypeHandle;
				return (ref job).ScheduleParallelByRef<JobManagedMovementOverrideAfterControl>(query, dependency);
			}

			// Token: 0x06001454 RID: 5204 RVA: 0x00003788 File Offset: 0x00001988
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref JobManagedMovementOverrideAfterControl job, EntityQuery query, ref SystemState state)
			{
			}

			// Token: 0x06001455 RID: 5205 RVA: 0x000743B8 File Offset: 0x000725B8
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref JobManagedMovementOverrideAfterControl job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			// Token: 0x06001456 RID: 5206 RVA: 0x00076E08 File Offset: 0x00075008
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref JobManagedMovementOverrideAfterControl job, EntityManager entityManager)
			{
				job.__EntityManager = entityManager;
			}

			// Token: 0x04000E6D RID: 3693
			public JobManagedMovementOverrideAfterControl.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

			// Token: 0x04000E6E RID: 3694
			public EntityQuery DefaultQuery;

			// Token: 0x02000333 RID: 819
			public struct TypeHandle
			{
				// Token: 0x06001457 RID: 5207 RVA: 0x00076E14 File Offset: 0x00075014
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					this.__Pathfinding_ECS_ManagedMovementOverrideAfterControl_RW_ComponentTypeHandle = state.EntityManager.GetComponentTypeHandle<ManagedMovementOverrideAfterControl>(false);
					this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
					this.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>(false);
					this.__Pathfinding_ECS_AgentCylinderShape_RW_ComponentTypeHandle = state.GetComponentTypeHandle<AgentCylinderShape>(false);
					this.__Pathfinding_ECS_AgentMovementPlane_RW_ComponentTypeHandle = state.GetComponentTypeHandle<AgentMovementPlane>(false);
					this.__Pathfinding_ECS_DestinationPoint_RW_ComponentTypeHandle = state.GetComponentTypeHandle<DestinationPoint>(false);
					this.__Pathfinding_ECS_MovementState_RW_ComponentTypeHandle = state.GetComponentTypeHandle<MovementState>(false);
					this.__Pathfinding_ECS_MovementSettings_RW_ComponentTypeHandle = state.GetComponentTypeHandle<MovementSettings>(false);
					this.__Pathfinding_ECS_MovementControl_RW_ComponentTypeHandle = state.GetComponentTypeHandle<MovementControl>(false);
				}

				// Token: 0x06001458 RID: 5208 RVA: 0x00076EA0 File Offset: 0x000750A0
				public void Update(ref SystemState state)
				{
					this.__Pathfinding_ECS_ManagedMovementOverrideAfterControl_RW_ComponentTypeHandle.Update(ref state);
					this.__Unity_Entities_Entity_TypeHandle.Update(ref state);
					this.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_AgentCylinderShape_RW_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_AgentMovementPlane_RW_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_DestinationPoint_RW_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_MovementState_RW_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_MovementSettings_RW_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_MovementControl_RW_ComponentTypeHandle.Update(ref state);
				}

				// Token: 0x04000E6F RID: 3695
				public ComponentTypeHandle<ManagedMovementOverrideAfterControl> __Pathfinding_ECS_ManagedMovementOverrideAfterControl_RW_ComponentTypeHandle;

				// Token: 0x04000E70 RID: 3696
				[ReadOnly]
				public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

				// Token: 0x04000E71 RID: 3697
				public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

				// Token: 0x04000E72 RID: 3698
				public ComponentTypeHandle<AgentCylinderShape> __Pathfinding_ECS_AgentCylinderShape_RW_ComponentTypeHandle;

				// Token: 0x04000E73 RID: 3699
				public ComponentTypeHandle<AgentMovementPlane> __Pathfinding_ECS_AgentMovementPlane_RW_ComponentTypeHandle;

				// Token: 0x04000E74 RID: 3700
				public ComponentTypeHandle<DestinationPoint> __Pathfinding_ECS_DestinationPoint_RW_ComponentTypeHandle;

				// Token: 0x04000E75 RID: 3701
				public ComponentTypeHandle<MovementState> __Pathfinding_ECS_MovementState_RW_ComponentTypeHandle;

				// Token: 0x04000E76 RID: 3702
				public ComponentTypeHandle<MovementSettings> __Pathfinding_ECS_MovementSettings_RW_ComponentTypeHandle;

				// Token: 0x04000E77 RID: 3703
				public ComponentTypeHandle<MovementControl> __Pathfinding_ECS_MovementControl_RW_ComponentTypeHandle;
			}
		}

		// Token: 0x02000334 RID: 820
		public struct InternalCompiler
		{
			// Token: 0x06001459 RID: 5209 RVA: 0x000768D5 File Offset: 0x00074AD5
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
