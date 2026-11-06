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

namespace Pathfinding.ECS
{
	// Token: 0x02000313 RID: 787
	[BurstCompile]
	[WithAll(new Type[] { typeof(SimulateMovement) })]
	[CompilerGenerated]
	public struct JobAlignAgentWithMovementDirection : IJobEntity, IJobChunk
	{
		// Token: 0x0600132E RID: 4910 RVA: 0x00073E48 File Offset: 0x00072048
		public void Execute(ref LocalTransform transform, in MovementSettings movementSettings, in MovementState movementState, in AgentCylinderShape shape, in AgentMovementPlane movementPlane, in MovementControl movementControl, ref ResolvedMovement resolvedMovement)
		{
			if (math.lengthsq(movementControl.targetPoint - resolvedMovement.targetPoint) > 0.001f && resolvedMovement.speed > movementSettings.follower.speed * 0.1f)
			{
				float2 @float = movementPlane.value.ToPlane(movementControl.targetPoint - transform.Position);
				float2 float2 = movementPlane.value.ToPlane(resolvedMovement.targetPoint - transform.Position);
				float num;
				if (math.lengthsq(@float) > math.pow(movementSettings.follower.speed * 0.1f, 2f))
				{
					num = math.atan2(@float.y, @float.x);
				}
				else
				{
					num = movementPlane.value.ToPlane(transform.Rotation) + 1.5707964f;
				}
				if (math.lengthsq(float2) > math.pow(movementSettings.follower.speed * 0.1f, 2f))
				{
					float num2 = math.atan2(float2.y, float2.x);
					resolvedMovement.targetRotationOffset = AstarMath.DeltaAngle(num, num2);
					return;
				}
			}
			resolvedMovement.targetRotationOffset = AstarMath.DeltaAngle(0f, resolvedMovement.targetRotationOffset) * (1f - this.dt * 2f);
		}

		// Token: 0x0600132F RID: 4911 RVA: 0x00073F94 File Offset: 0x00072194
		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			IntPtr intPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<LocalTransform>(chunk, ref this.__TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
			IntPtr intPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<MovementSettings>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_MovementSettings_RO_ComponentTypeHandle);
			IntPtr intPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<MovementState>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_MovementState_RO_ComponentTypeHandle);
			IntPtr intPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<AgentCylinderShape>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_AgentCylinderShape_RO_ComponentTypeHandle);
			IntPtr intPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<AgentMovementPlane>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_AgentMovementPlane_RO_ComponentTypeHandle);
			IntPtr intPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<MovementControl>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_MovementControl_RO_ComponentTypeHandle);
			IntPtr intPtr7 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<ResolvedMovement>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_ResolvedMovement_RW_ComponentTypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					ref LocalTransform ptr = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr, i);
					ref MovementSettings ptr2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementSettings>(intPtr2, i);
					ref MovementState ptr3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementState>(intPtr3, i);
					ref AgentCylinderShape ptr4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr4, i);
					ref AgentMovementPlane ptr5 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr5, i);
					ref MovementControl ptr6 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementControl>(intPtr6, i);
					ref ResolvedMovement ptr7 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<ResolvedMovement>(intPtr7, i);
					this.Execute(ref ptr, in ptr2, in ptr3, in ptr4, in ptr5, in ptr6, ref ptr7);
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
						ref LocalTransform ptr8 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr, j);
						ref MovementSettings ptr9 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementSettings>(intPtr2, j);
						ref MovementState ptr10 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementState>(intPtr3, j);
						ref AgentCylinderShape ptr11 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr4, j);
						ref AgentMovementPlane ptr12 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr5, j);
						ref MovementControl ptr13 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementControl>(intPtr6, j);
						ref ResolvedMovement ptr14 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<ResolvedMovement>(intPtr7, j);
						this.Execute(ref ptr8, in ptr9, in ptr10, in ptr11, in ptr12, in ptr13, ref ptr14);
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
					ref LocalTransform ptr15 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr, k);
					ref MovementSettings ptr16 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementSettings>(intPtr2, k);
					ref MovementState ptr17 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementState>(intPtr3, k);
					ref AgentCylinderShape ptr18 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr4, k);
					ref AgentMovementPlane ptr19 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr5, k);
					ref MovementControl ptr20 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementControl>(intPtr6, k);
					ref ResolvedMovement ptr21 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<ResolvedMovement>(intPtr7, k);
					this.Execute(ref ptr15, in ptr16, in ptr17, in ptr18, in ptr19, in ptr20, ref ptr21);
					num++;
				}
				num3 >>= 1;
			}
			num3 = chunkEnabledMask.ULong1;
			for (int l = 64; l < count; l++)
			{
				if ((num3 & 1UL) != 0UL)
				{
					ref LocalTransform ptr22 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr, l);
					ref MovementSettings ptr23 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementSettings>(intPtr2, l);
					ref MovementState ptr24 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementState>(intPtr3, l);
					ref AgentCylinderShape ptr25 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr4, l);
					ref AgentMovementPlane ptr26 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr5, l);
					ref MovementControl ptr27 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementControl>(intPtr6, l);
					ref ResolvedMovement ptr28 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<ResolvedMovement>(intPtr7, l);
					this.Execute(ref ptr22, in ptr23, in ptr24, in ptr25, in ptr26, in ptr27, ref ptr28);
					num++;
				}
				num3 >>= 1;
			}
		}

		// Token: 0x06001330 RID: 4912 RVA: 0x000742B0 File Offset: 0x000724B0
		private JobHandle __ThrowCodeGenException()
		{
			throw new Exception("This method should have been replaced by source gen.");
		}

		// Token: 0x06001331 RID: 4913 RVA: 0x000742C7 File Offset: 0x000724C7
		public void Run()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001332 RID: 4914 RVA: 0x000742C7 File Offset: 0x000724C7
		public void RunByRef()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001333 RID: 4915 RVA: 0x000742C7 File Offset: 0x000724C7
		public void Run(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001334 RID: 4916 RVA: 0x000742C7 File Offset: 0x000724C7
		public void RunByRef(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001335 RID: 4917 RVA: 0x000742D0 File Offset: 0x000724D0
		public JobHandle Schedule(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001336 RID: 4918 RVA: 0x000742D0 File Offset: 0x000724D0
		public JobHandle ScheduleByRef(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001337 RID: 4919 RVA: 0x000742D0 File Offset: 0x000724D0
		public JobHandle Schedule(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001338 RID: 4920 RVA: 0x000742D0 File Offset: 0x000724D0
		public JobHandle ScheduleByRef(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001339 RID: 4921 RVA: 0x000742C7 File Offset: 0x000724C7
		public void Schedule()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x0600133A RID: 4922 RVA: 0x000742C7 File Offset: 0x000724C7
		public void ScheduleByRef()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x0600133B RID: 4923 RVA: 0x000742C7 File Offset: 0x000724C7
		public void Schedule(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x0600133C RID: 4924 RVA: 0x000742C7 File Offset: 0x000724C7
		public void ScheduleByRef(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x0600133D RID: 4925 RVA: 0x000742D0 File Offset: 0x000724D0
		public JobHandle ScheduleParallel(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x0600133E RID: 4926 RVA: 0x000742D0 File Offset: 0x000724D0
		public JobHandle ScheduleParallelByRef(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x0600133F RID: 4927 RVA: 0x000742D0 File Offset: 0x000724D0
		public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001340 RID: 4928 RVA: 0x000742D0 File Offset: 0x000724D0
		public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001341 RID: 4929 RVA: 0x000742D0 File Offset: 0x000724D0
		public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001342 RID: 4930 RVA: 0x000742D0 File Offset: 0x000724D0
		public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001343 RID: 4931 RVA: 0x000742C7 File Offset: 0x000724C7
		public void ScheduleParallel()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001344 RID: 4932 RVA: 0x000742C7 File Offset: 0x000724C7
		public void ScheduleParallelByRef()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001345 RID: 4933 RVA: 0x000742C7 File Offset: 0x000724C7
		public void ScheduleParallel(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001346 RID: 4934 RVA: 0x000742C7 File Offset: 0x000724C7
		public void ScheduleParallelByRef(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001347 RID: 4935 RVA: 0x000742D8 File Offset: 0x000724D8
		void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
		}

		// Token: 0x04000E12 RID: 3602
		public float dt;

		// Token: 0x04000E13 RID: 3603
		private JobAlignAgentWithMovementDirection.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		// Token: 0x02000314 RID: 788
		public struct InternalCompilerQueryAndHandleData
		{
			// Token: 0x06001348 RID: 4936 RVA: 0x000742E8 File Offset: 0x000724E8
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				this.DefaultQuery = entityQueryBuilder.WithAll<MovementSettings>().WithAll<MovementState>().WithAll<AgentCylinderShape>()
					.WithAll<AgentMovementPlane>()
					.WithAll<MovementControl>()
					.WithAll<SimulateMovement>()
					.WithAllRW<LocalTransform>()
					.WithAllRW<ResolvedMovement>()
					.Build(ref state);
				entityQueryBuilder.Reset();
				entityQueryBuilder.Dispose();
			}

			// Token: 0x06001349 RID: 4937 RVA: 0x0007435F File Offset: 0x0007255F
			public void Init(ref SystemState state, bool assignDefaultQuery)
			{
				if (assignDefaultQuery)
				{
					this.__AssignQueries(ref state);
				}
				this.__TypeHandle.__AssignHandles(ref state);
			}

			// Token: 0x0600134A RID: 4938 RVA: 0x00074377 File Offset: 0x00072577
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Run(ref JobAlignAgentWithMovementDirection job, EntityQuery query)
			{
				job.__TypeHandle = this.__TypeHandle;
				(ref job).RunByRef<JobAlignAgentWithMovementDirection>(query);
			}

			// Token: 0x0600134B RID: 4939 RVA: 0x0007438C File Offset: 0x0007258C
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref JobAlignAgentWithMovementDirection job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = this.__TypeHandle;
				return (ref job).ScheduleByRef<JobAlignAgentWithMovementDirection>(query, dependency);
			}

			// Token: 0x0600134C RID: 4940 RVA: 0x000743A2 File Offset: 0x000725A2
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref JobAlignAgentWithMovementDirection job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = this.__TypeHandle;
				return (ref job).ScheduleParallelByRef<JobAlignAgentWithMovementDirection>(query, dependency);
			}

			// Token: 0x0600134D RID: 4941 RVA: 0x00003788 File Offset: 0x00001988
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref JobAlignAgentWithMovementDirection job, EntityQuery query, ref SystemState state)
			{
			}

			// Token: 0x0600134E RID: 4942 RVA: 0x000743B8 File Offset: 0x000725B8
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref JobAlignAgentWithMovementDirection job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			// Token: 0x0600134F RID: 4943 RVA: 0x00003788 File Offset: 0x00001988
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref JobAlignAgentWithMovementDirection job, EntityManager entityManager)
			{
			}

			// Token: 0x04000E14 RID: 3604
			public JobAlignAgentWithMovementDirection.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

			// Token: 0x04000E15 RID: 3605
			public EntityQuery DefaultQuery;

			// Token: 0x02000315 RID: 789
			public struct TypeHandle
			{
				// Token: 0x06001350 RID: 4944 RVA: 0x000743BC File Offset: 0x000725BC
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					this.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>(false);
					this.__Pathfinding_ECS_MovementSettings_RO_ComponentTypeHandle = state.GetComponentTypeHandle<MovementSettings>(true);
					this.__Pathfinding_ECS_MovementState_RO_ComponentTypeHandle = state.GetComponentTypeHandle<MovementState>(true);
					this.__Pathfinding_ECS_AgentCylinderShape_RO_ComponentTypeHandle = state.GetComponentTypeHandle<AgentCylinderShape>(true);
					this.__Pathfinding_ECS_AgentMovementPlane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<AgentMovementPlane>(true);
					this.__Pathfinding_ECS_MovementControl_RO_ComponentTypeHandle = state.GetComponentTypeHandle<MovementControl>(true);
					this.__Pathfinding_ECS_ResolvedMovement_RW_ComponentTypeHandle = state.GetComponentTypeHandle<ResolvedMovement>(false);
				}

				// Token: 0x06001351 RID: 4945 RVA: 0x00074424 File Offset: 0x00072624
				public void Update(ref SystemState state)
				{
					this.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_MovementSettings_RO_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_MovementState_RO_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_AgentCylinderShape_RO_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_AgentMovementPlane_RO_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_MovementControl_RO_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_ResolvedMovement_RW_ComponentTypeHandle.Update(ref state);
				}

				// Token: 0x04000E16 RID: 3606
				public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

				// Token: 0x04000E17 RID: 3607
				[ReadOnly]
				public ComponentTypeHandle<MovementSettings> __Pathfinding_ECS_MovementSettings_RO_ComponentTypeHandle;

				// Token: 0x04000E18 RID: 3608
				[ReadOnly]
				public ComponentTypeHandle<MovementState> __Pathfinding_ECS_MovementState_RO_ComponentTypeHandle;

				// Token: 0x04000E19 RID: 3609
				[ReadOnly]
				public ComponentTypeHandle<AgentCylinderShape> __Pathfinding_ECS_AgentCylinderShape_RO_ComponentTypeHandle;

				// Token: 0x04000E1A RID: 3610
				[ReadOnly]
				public ComponentTypeHandle<AgentMovementPlane> __Pathfinding_ECS_AgentMovementPlane_RO_ComponentTypeHandle;

				// Token: 0x04000E1B RID: 3611
				[ReadOnly]
				public ComponentTypeHandle<MovementControl> __Pathfinding_ECS_MovementControl_RO_ComponentTypeHandle;

				// Token: 0x04000E1C RID: 3612
				public ComponentTypeHandle<ResolvedMovement> __Pathfinding_ECS_ResolvedMovement_RW_ComponentTypeHandle;
			}
		}

		// Token: 0x02000316 RID: 790
		public struct InternalCompiler
		{
			// Token: 0x06001352 RID: 4946 RVA: 0x00003788 File Offset: 0x00001988
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void CheckForErrors(int scheduleType)
			{
			}
		}
	}
}
