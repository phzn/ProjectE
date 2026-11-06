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
	// Token: 0x02000341 RID: 833
	[BurstCompile]
	[WithAll(new Type[]
	{
		typeof(SimulateMovement),
		typeof(SimulateMovementFinalize)
	})]
	[WithNone(new Type[] { typeof(AgentOffMeshLinkMovementDisabled) })]
	[CompilerGenerated]
	public struct JobMoveAgent : IJobEntity, IJobChunk
	{
		// Token: 0x060014CA RID: 5322 RVA: 0x00078080 File Offset: 0x00076280
		public static quaternion ResolveRotation(quaternion rotation, ref MovementState state, in ResolvedMovement resolvedMovement, in MovementSettings movementSettings, in AgentMovementPlane movementPlane, float dt)
		{
			float num = movementPlane.value.ToPlane(rotation);
			float num2 = num - state.rotationOffset - state.rotationOffset2;
			float num3 = math.clamp(AstarMath.DeltaAngle(num2, resolvedMovement.targetRotation), -resolvedMovement.rotationSpeed * dt, resolvedMovement.rotationSpeed * dt);
			float num4 = math.radians(movementSettings.follower.maxRotationSpeed) * 0.5f;
			float num5 = math.clamp(AstarMath.DeltaAngle(state.rotationOffset, resolvedMovement.targetRotationOffset), -num4 * dt, num4 * dt);
			float num6 = num2 + state.rotationOffset;
			float num7 = num2 + num3;
			state.rotationOffset += num5;
			state.rotationOffset = AstarMath.DeltaAngle(0f, state.rotationOffset);
			float num8 = num7 + state.rotationOffset;
			if (movementSettings.rotationSmoothing > 0f)
			{
				state.rotationOffset2 += num6 - num8;
				state.rotationOffset2 = AstarMath.DeltaAngle(0f, state.rotationOffset2);
				float num9 = math.abs(AstarMath.DeltaAngle(num, resolvedMovement.targetRotationHint)) / movementSettings.rotationSmoothing * dt;
				float num10 = 0.1f / movementSettings.rotationSmoothing * dt;
				if (math.abs(state.rotationOffset2) > 0f)
				{
					state.rotationOffset2 *= math.max(0f, 1f - num9 - num10 / math.abs(state.rotationOffset2));
				}
			}
			else if (state.rotationOffset2 != 0f)
			{
				state.rotationOffset2 += math.clamp(-state.rotationOffset2, -num4 * dt, num4 * dt);
			}
			return movementPlane.value.ToWorldRotation(num7 + state.rotationOffset + state.rotationOffset2);
		}

		// Token: 0x060014CB RID: 5323 RVA: 0x0007822C File Offset: 0x0007642C
		public static float3 MoveWithoutGravity(float3 position, in ResolvedMovement resolvedMovement, in AgentMovementPlane movementPlane, float dt)
		{
			float2 @float = movementPlane.value.ToPlane(resolvedMovement.targetPoint - position);
			float num = math.length(@float);
			float2 float2 = math.select(@float, @float * math.clamp(resolvedMovement.speed * dt / num, 0f, 1f), num > 0.0001f);
			return movementPlane.value.ToWorld(float2, 0f);
		}

		// Token: 0x060014CC RID: 5324 RVA: 0x00078298 File Offset: 0x00076498
		public static void ResolvePositionSmoothing(float3 movementDelta, ref MovementState state, in MovementSettings movementSettings, float dt)
		{
			if (movementSettings.positionSmoothing > 0f)
			{
				state.positionOffset -= movementDelta;
				float num = 1f / movementSettings.positionSmoothing * dt;
				float num2 = 0.1f / movementSettings.positionSmoothing * dt;
				float num3 = math.length(state.positionOffset);
				if (num3 > 0f)
				{
					state.positionOffset *= math.max(0f, 1f - num - num2 / num3);
					return;
				}
			}
			else
			{
				state.positionOffset = float3.zero;
			}
		}

		// Token: 0x060014CD RID: 5325 RVA: 0x00078332 File Offset: 0x00076532
		public void Execute(ref LocalTransform transform, in AgentCylinderShape shape, in AgentMovementPlane movementPlane, ref MovementState state, in MovementSettings movementSettings, in ResolvedMovement resolvedMovement, ref MovementStatistics movementStatistics)
		{
			JobMoveAgent.MoveAgent(ref transform, in shape, in movementPlane, ref state, in movementSettings, in resolvedMovement, ref movementStatistics, this.dt);
		}

		// Token: 0x060014CE RID: 5326 RVA: 0x0007834C File Offset: 0x0007654C
		public static void MoveAgent(ref LocalTransform transform, in AgentCylinderShape shape, in AgentMovementPlane movementPlane, ref MovementState state, in MovementSettings movementSettings, in ResolvedMovement resolvedMovement, ref MovementStatistics movementStatistics, float dt)
		{
			float3 @float = JobMoveAgent.MoveWithoutGravity(transform.Position, in resolvedMovement, in movementPlane, dt);
			transform.Position += @float;
			if (math.abs(transform.Position.z) < 1E-05f)
			{
				transform.Position.z = 0f;
			}
			JobMoveAgent.ResolvePositionSmoothing(@float, ref state, in movementSettings, dt);
			transform.Rotation = JobMoveAgent.ResolveRotation(transform.Rotation, ref state, in resolvedMovement, in movementSettings, in movementPlane, dt);
		}

		// Token: 0x060014CF RID: 5327 RVA: 0x000783CC File Offset: 0x000765CC
		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			IntPtr intPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<LocalTransform>(chunk, ref this.__TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
			IntPtr intPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<AgentCylinderShape>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_AgentCylinderShape_RO_ComponentTypeHandle);
			IntPtr intPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<AgentMovementPlane>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_AgentMovementPlane_RO_ComponentTypeHandle);
			IntPtr intPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<MovementState>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_MovementState_RW_ComponentTypeHandle);
			IntPtr intPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<MovementSettings>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_MovementSettings_RO_ComponentTypeHandle);
			IntPtr intPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr<ResolvedMovement>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_ResolvedMovement_RO_ComponentTypeHandle);
			IntPtr intPtr7 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<MovementStatistics>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_MovementStatistics_RW_ComponentTypeHandle);
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
					this.Execute(ref ptr, in ptr2, in ptr3, ref ptr4, in ptr5, in ptr6, ref ptr7);
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
						ref AgentCylinderShape ptr9 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr2, j);
						ref AgentMovementPlane ptr10 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr3, j);
						ref MovementState ptr11 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementState>(intPtr4, j);
						ref MovementSettings ptr12 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementSettings>(intPtr5, j);
						ref ResolvedMovement ptr13 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<ResolvedMovement>(intPtr6, j);
						ref MovementStatistics ptr14 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementStatistics>(intPtr7, j);
						this.Execute(ref ptr8, in ptr9, in ptr10, ref ptr11, in ptr12, in ptr13, ref ptr14);
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
					ref AgentCylinderShape ptr16 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr2, k);
					ref AgentMovementPlane ptr17 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr3, k);
					ref MovementState ptr18 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementState>(intPtr4, k);
					ref MovementSettings ptr19 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementSettings>(intPtr5, k);
					ref ResolvedMovement ptr20 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<ResolvedMovement>(intPtr6, k);
					ref MovementStatistics ptr21 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementStatistics>(intPtr7, k);
					this.Execute(ref ptr15, in ptr16, in ptr17, ref ptr18, in ptr19, in ptr20, ref ptr21);
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
					ref AgentCylinderShape ptr23 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentCylinderShape>(intPtr2, l);
					ref AgentMovementPlane ptr24 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr3, l);
					ref MovementState ptr25 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementState>(intPtr4, l);
					ref MovementSettings ptr26 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementSettings>(intPtr5, l);
					ref ResolvedMovement ptr27 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<ResolvedMovement>(intPtr6, l);
					ref MovementStatistics ptr28 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementStatistics>(intPtr7, l);
					this.Execute(ref ptr22, in ptr23, in ptr24, ref ptr25, in ptr26, in ptr27, ref ptr28);
					num++;
				}
				num3 >>= 1;
			}
		}

		// Token: 0x060014D0 RID: 5328 RVA: 0x000786E8 File Offset: 0x000768E8
		private JobHandle __ThrowCodeGenException()
		{
			throw new Exception("This method should have been replaced by source gen.");
		}

		// Token: 0x060014D1 RID: 5329 RVA: 0x000786FF File Offset: 0x000768FF
		public void Run()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060014D2 RID: 5330 RVA: 0x000786FF File Offset: 0x000768FF
		public void RunByRef()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060014D3 RID: 5331 RVA: 0x000786FF File Offset: 0x000768FF
		public void Run(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060014D4 RID: 5332 RVA: 0x000786FF File Offset: 0x000768FF
		public void RunByRef(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060014D5 RID: 5333 RVA: 0x00078708 File Offset: 0x00076908
		public JobHandle Schedule(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x060014D6 RID: 5334 RVA: 0x00078708 File Offset: 0x00076908
		public JobHandle ScheduleByRef(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x060014D7 RID: 5335 RVA: 0x00078708 File Offset: 0x00076908
		public JobHandle Schedule(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x060014D8 RID: 5336 RVA: 0x00078708 File Offset: 0x00076908
		public JobHandle ScheduleByRef(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x060014D9 RID: 5337 RVA: 0x000786FF File Offset: 0x000768FF
		public void Schedule()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060014DA RID: 5338 RVA: 0x000786FF File Offset: 0x000768FF
		public void ScheduleByRef()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060014DB RID: 5339 RVA: 0x000786FF File Offset: 0x000768FF
		public void Schedule(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060014DC RID: 5340 RVA: 0x000786FF File Offset: 0x000768FF
		public void ScheduleByRef(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060014DD RID: 5341 RVA: 0x00078708 File Offset: 0x00076908
		public JobHandle ScheduleParallel(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x060014DE RID: 5342 RVA: 0x00078708 File Offset: 0x00076908
		public JobHandle ScheduleParallelByRef(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x060014DF RID: 5343 RVA: 0x00078708 File Offset: 0x00076908
		public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x060014E0 RID: 5344 RVA: 0x00078708 File Offset: 0x00076908
		public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x060014E1 RID: 5345 RVA: 0x00078708 File Offset: 0x00076908
		public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x060014E2 RID: 5346 RVA: 0x00078708 File Offset: 0x00076908
		public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x060014E3 RID: 5347 RVA: 0x000786FF File Offset: 0x000768FF
		public void ScheduleParallel()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060014E4 RID: 5348 RVA: 0x000786FF File Offset: 0x000768FF
		public void ScheduleParallelByRef()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060014E5 RID: 5349 RVA: 0x000786FF File Offset: 0x000768FF
		public void ScheduleParallel(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060014E6 RID: 5350 RVA: 0x000786FF File Offset: 0x000768FF
		public void ScheduleParallelByRef(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x060014E7 RID: 5351 RVA: 0x00078710 File Offset: 0x00076910
		void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
		}

		// Token: 0x04000E9C RID: 3740
		public float dt;

		// Token: 0x04000E9D RID: 3741
		private JobMoveAgent.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		// Token: 0x02000342 RID: 834
		public struct InternalCompilerQueryAndHandleData
		{
			// Token: 0x060014E8 RID: 5352 RVA: 0x00078720 File Offset: 0x00076920
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				this.DefaultQuery = entityQueryBuilder.WithNone<AgentOffMeshLinkMovementDisabled>().WithAll<AgentCylinderShape>().WithAll<AgentMovementPlane>()
					.WithAll<MovementSettings>()
					.WithAll<ResolvedMovement>()
					.WithAll<SimulateMovement>()
					.WithAll<SimulateMovementFinalize>()
					.WithAllRW<LocalTransform>()
					.WithAllRW<MovementState>()
					.WithAllRW<MovementStatistics>()
					.Build(ref state);
				entityQueryBuilder.Reset();
				entityQueryBuilder.Dispose();
			}

			// Token: 0x060014E9 RID: 5353 RVA: 0x000787A7 File Offset: 0x000769A7
			public void Init(ref SystemState state, bool assignDefaultQuery)
			{
				if (assignDefaultQuery)
				{
					this.__AssignQueries(ref state);
				}
				this.__TypeHandle.__AssignHandles(ref state);
			}

			// Token: 0x060014EA RID: 5354 RVA: 0x000787BF File Offset: 0x000769BF
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Run(ref JobMoveAgent job, EntityQuery query)
			{
				job.__TypeHandle = this.__TypeHandle;
				(ref job).RunByRef<JobMoveAgent>(query);
			}

			// Token: 0x060014EB RID: 5355 RVA: 0x000787D4 File Offset: 0x000769D4
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref JobMoveAgent job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = this.__TypeHandle;
				return (ref job).ScheduleByRef<JobMoveAgent>(query, dependency);
			}

			// Token: 0x060014EC RID: 5356 RVA: 0x000787EA File Offset: 0x000769EA
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref JobMoveAgent job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = this.__TypeHandle;
				return (ref job).ScheduleParallelByRef<JobMoveAgent>(query, dependency);
			}

			// Token: 0x060014ED RID: 5357 RVA: 0x00003788 File Offset: 0x00001988
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref JobMoveAgent job, EntityQuery query, ref SystemState state)
			{
			}

			// Token: 0x060014EE RID: 5358 RVA: 0x000743B8 File Offset: 0x000725B8
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref JobMoveAgent job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			// Token: 0x060014EF RID: 5359 RVA: 0x00003788 File Offset: 0x00001988
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref JobMoveAgent job, EntityManager entityManager)
			{
			}

			// Token: 0x04000E9E RID: 3742
			public JobMoveAgent.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

			// Token: 0x04000E9F RID: 3743
			public EntityQuery DefaultQuery;

			// Token: 0x02000343 RID: 835
			public struct TypeHandle
			{
				// Token: 0x060014F0 RID: 5360 RVA: 0x00078800 File Offset: 0x00076A00
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					this.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>(false);
					this.__Pathfinding_ECS_AgentCylinderShape_RO_ComponentTypeHandle = state.GetComponentTypeHandle<AgentCylinderShape>(true);
					this.__Pathfinding_ECS_AgentMovementPlane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<AgentMovementPlane>(true);
					this.__Pathfinding_ECS_MovementState_RW_ComponentTypeHandle = state.GetComponentTypeHandle<MovementState>(false);
					this.__Pathfinding_ECS_MovementSettings_RO_ComponentTypeHandle = state.GetComponentTypeHandle<MovementSettings>(true);
					this.__Pathfinding_ECS_ResolvedMovement_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ResolvedMovement>(true);
					this.__Pathfinding_ECS_MovementStatistics_RW_ComponentTypeHandle = state.GetComponentTypeHandle<MovementStatistics>(false);
				}

				// Token: 0x060014F1 RID: 5361 RVA: 0x00078868 File Offset: 0x00076A68
				public void Update(ref SystemState state)
				{
					this.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_AgentCylinderShape_RO_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_AgentMovementPlane_RO_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_MovementState_RW_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_MovementSettings_RO_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_ResolvedMovement_RO_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_MovementStatistics_RW_ComponentTypeHandle.Update(ref state);
				}

				// Token: 0x04000EA0 RID: 3744
				public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

				// Token: 0x04000EA1 RID: 3745
				[ReadOnly]
				public ComponentTypeHandle<AgentCylinderShape> __Pathfinding_ECS_AgentCylinderShape_RO_ComponentTypeHandle;

				// Token: 0x04000EA2 RID: 3746
				[ReadOnly]
				public ComponentTypeHandle<AgentMovementPlane> __Pathfinding_ECS_AgentMovementPlane_RO_ComponentTypeHandle;

				// Token: 0x04000EA3 RID: 3747
				public ComponentTypeHandle<MovementState> __Pathfinding_ECS_MovementState_RW_ComponentTypeHandle;

				// Token: 0x04000EA4 RID: 3748
				[ReadOnly]
				public ComponentTypeHandle<MovementSettings> __Pathfinding_ECS_MovementSettings_RO_ComponentTypeHandle;

				// Token: 0x04000EA5 RID: 3749
				[ReadOnly]
				public ComponentTypeHandle<ResolvedMovement> __Pathfinding_ECS_ResolvedMovement_RO_ComponentTypeHandle;

				// Token: 0x04000EA6 RID: 3750
				public ComponentTypeHandle<MovementStatistics> __Pathfinding_ECS_MovementStatistics_RW_ComponentTypeHandle;
			}
		}

		// Token: 0x02000344 RID: 836
		public struct InternalCompiler
		{
			// Token: 0x060014F2 RID: 5362 RVA: 0x00003788 File Offset: 0x00001988
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void CheckForErrors(int scheduleType)
			{
			}
		}
	}
}
