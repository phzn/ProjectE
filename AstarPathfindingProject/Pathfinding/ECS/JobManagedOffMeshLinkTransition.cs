using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
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
	// Token: 0x02000339 RID: 825
	[WithOptions(EntityQueryOptions.IgnoreComponentEnabledState)]
	[CompilerGenerated]
	public struct JobManagedOffMeshLinkTransition : IJobEntity, IJobChunk
	{
		// Token: 0x0600147F RID: 5247 RVA: 0x000775C0 File Offset: 0x000757C0
		public void Execute(Entity entity, ManagedState state, ref LocalTransform transform, ref AgentMovementPlane movementPlane, ref MovementControl movementControl, ref MovementSettings movementSettings, ref AgentOffMeshLinkTraversal linkInfo, ManagedAgentOffMeshLinkTraversal managedLinkInfo, EnabledRefRW<AgentOffMeshLinkMovementDisabled> movementDisabled, EnabledRefRW<AgentOffMeshLinkLocalAvoidanceDisabled> localAvoidanceDisabled)
		{
			if (!JobManagedOffMeshLinkTransition.MoveNext(entity, state, ref transform, ref movementPlane, ref movementControl, ref movementSettings, ref linkInfo, managedLinkInfo, movementDisabled, localAvoidanceDisabled, this.deltaTime))
			{
				this.commandBuffer.RemoveComponent<AgentOffMeshLinkTraversal>(entity);
				this.commandBuffer.RemoveComponent<ManagedAgentOffMeshLinkTraversal>(entity);
				this.commandBuffer.RemoveComponent<AgentOffMeshLinkMovementDisabled>(entity);
				this.commandBuffer.RemoveComponent<AgentOffMeshLinkLocalAvoidanceDisabled>(entity);
			}
		}

		// Token: 0x06001480 RID: 5248 RVA: 0x0007761C File Offset: 0x0007581C
		public static bool MoveNext(Entity entity, ManagedState state, ref LocalTransform transform, ref AgentMovementPlane movementPlane, ref MovementControl movementControl, ref MovementSettings movementSettings, ref AgentOffMeshLinkTraversal linkInfo, ManagedAgentOffMeshLinkTraversal managedLinkInfo, EnabledRefRW<AgentOffMeshLinkMovementDisabled> movementDisabled, EnabledRefRW<AgentOffMeshLinkLocalAvoidanceDisabled> localAvoidanceDisabled, float deltaTime)
		{
			managedLinkInfo.context.SetInternalData(entity, ref transform, ref movementPlane, ref movementControl, ref movementSettings, ref linkInfo, movementDisabled, localAvoidanceDisabled, state, deltaTime);
			if (managedLinkInfo.coroutine == null)
			{
				state.CancelCurrentPathRequest();
				if (managedLinkInfo.stateMachine == null)
				{
					managedLinkInfo.stateMachine = ((managedLinkInfo.handler != null) ? managedLinkInfo.handler.GetOffMeshLinkStateMachine(managedLinkInfo.context) : null);
				}
				IEnumerator enumerator2;
				if (managedLinkInfo.stateMachine == null)
				{
					IEnumerator enumerator = JobStartOffMeshLinkTransition.DefaultOnTraverseOffMeshLink(managedLinkInfo.context).GetEnumerator();
					enumerator2 = enumerator;
				}
				else
				{
					enumerator2 = managedLinkInfo.stateMachine.OnTraverseOffMeshLink(managedLinkInfo.context).GetEnumerator();
				}
				managedLinkInfo.coroutine = enumerator2;
				if (localAvoidanceDisabled.IsValid)
				{
					localAvoidanceDisabled.ValueRW = false;
				}
			}
			bool flag = false;
			bool flag2 = true;
			if (movementDisabled.IsValid)
			{
				movementDisabled.ValueRW = true;
			}
			bool flag3;
			try
			{
				flag3 = !managedLinkInfo.coroutine.MoveNext();
			}
			catch (AgentOffMeshLinkTraversalContext.AbortOffMeshLinkTraversal)
			{
				flag = true;
				flag3 = true;
				flag2 = false;
			}
			catch (Exception ex)
			{
				Debug.LogException(ex, managedLinkInfo.context.gameObject);
				managedLinkInfo.context.Teleport(managedLinkInfo.context.link.relativeEnd);
				flag3 = true;
				flag = true;
			}
			if (flag3)
			{
				try
				{
					if (managedLinkInfo.stateMachine != null)
					{
						if (flag)
						{
							managedLinkInfo.stateMachine.OnAbortTraversingOffMeshLink();
						}
						else
						{
							managedLinkInfo.stateMachine.OnFinishTraversingOffMeshLink(managedLinkInfo.context);
						}
					}
				}
				catch (Exception ex2)
				{
					Debug.LogException(ex2, managedLinkInfo.context.gameObject);
				}
				managedLinkInfo.context.Restore();
				if (flag2)
				{
					state.PopNextLinkFromPath();
				}
			}
			return !flag3;
		}

		// Token: 0x06001481 RID: 5249 RVA: 0x000777C4 File Offset: 0x000759C4
		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			IntPtr intPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, this.__TypeHandle.__Unity_Entities_Entity_TypeHandle);
			ManagedComponentAccessor<ManagedState> managedComponentAccessor = chunk.GetManagedComponentAccessor<ManagedState>(ref this.__TypeHandle.__Pathfinding_ECS_ManagedState_RW_ComponentTypeHandle, this.__EntityManager);
			IntPtr intPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<LocalTransform>(chunk, ref this.__TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
			IntPtr intPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<AgentMovementPlane>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_AgentMovementPlane_RW_ComponentTypeHandle);
			IntPtr intPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<MovementControl>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_MovementControl_RW_ComponentTypeHandle);
			IntPtr intPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<MovementSettings>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_MovementSettings_RW_ComponentTypeHandle);
			IntPtr intPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr<AgentOffMeshLinkTraversal>(chunk, ref this.__TypeHandle.__Pathfinding_ECS_AgentOffMeshLinkTraversal_RW_ComponentTypeHandle);
			ManagedComponentAccessor<ManagedAgentOffMeshLinkTraversal> managedComponentAccessor2 = chunk.GetManagedComponentAccessor<ManagedAgentOffMeshLinkTraversal>(ref this.__TypeHandle.__Pathfinding_ECS_ManagedAgentOffMeshLinkTraversal_RW_ComponentTypeHandle, this.__EntityManager);
			EnabledMask enabledMask = chunk.GetEnabledMask<AgentOffMeshLinkMovementDisabled>(ref this.__TypeHandle.__Pathfinding_ECS_AgentOffMeshLinkMovementDisabled_RW_ComponentTypeHandle);
			EnabledMask enabledMask2 = chunk.GetEnabledMask<AgentOffMeshLinkLocalAvoidanceDisabled>(ref this.__TypeHandle.__Pathfinding_ECS_AgentOffMeshLinkLocalAvoidanceDisabled_RW_ComponentTypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(intPtr, i);
					ManagedState managedState = managedComponentAccessor[i];
					ref LocalTransform ptr = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr2, i);
					ref AgentMovementPlane ptr2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr3, i);
					ref MovementControl ptr3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementControl>(intPtr4, i);
					ref MovementSettings ptr4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementSettings>(intPtr5, i);
					ref AgentOffMeshLinkTraversal ptr5 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentOffMeshLinkTraversal>(intPtr6, i);
					ManagedAgentOffMeshLinkTraversal managedAgentOffMeshLinkTraversal = managedComponentAccessor2[i];
					this.Execute(entity, managedState, ref ptr, ref ptr2, ref ptr3, ref ptr4, ref ptr5, managedAgentOffMeshLinkTraversal, enabledMask.GetEnabledRefRW<AgentOffMeshLinkMovementDisabled>(i), enabledMask2.GetEnabledRefRW<AgentOffMeshLinkLocalAvoidanceDisabled>(i));
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
						ManagedState managedState2 = managedComponentAccessor[j];
						ref LocalTransform ptr6 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr2, j);
						ref AgentMovementPlane ptr7 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr3, j);
						ref MovementControl ptr8 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementControl>(intPtr4, j);
						ref MovementSettings ptr9 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementSettings>(intPtr5, j);
						ref AgentOffMeshLinkTraversal ptr10 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentOffMeshLinkTraversal>(intPtr6, j);
						ManagedAgentOffMeshLinkTraversal managedAgentOffMeshLinkTraversal2 = managedComponentAccessor2[j];
						this.Execute(entity2, managedState2, ref ptr6, ref ptr7, ref ptr8, ref ptr9, ref ptr10, managedAgentOffMeshLinkTraversal2, enabledMask.GetEnabledRefRW<AgentOffMeshLinkMovementDisabled>(j), enabledMask2.GetEnabledRefRW<AgentOffMeshLinkLocalAvoidanceDisabled>(j));
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
					ManagedState managedState3 = managedComponentAccessor[k];
					ref LocalTransform ptr11 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr2, k);
					ref AgentMovementPlane ptr12 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr3, k);
					ref MovementControl ptr13 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementControl>(intPtr4, k);
					ref MovementSettings ptr14 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementSettings>(intPtr5, k);
					ref AgentOffMeshLinkTraversal ptr15 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentOffMeshLinkTraversal>(intPtr6, k);
					ManagedAgentOffMeshLinkTraversal managedAgentOffMeshLinkTraversal3 = managedComponentAccessor2[k];
					this.Execute(entity3, managedState3, ref ptr11, ref ptr12, ref ptr13, ref ptr14, ref ptr15, managedAgentOffMeshLinkTraversal3, enabledMask.GetEnabledRefRW<AgentOffMeshLinkMovementDisabled>(k), enabledMask2.GetEnabledRefRW<AgentOffMeshLinkLocalAvoidanceDisabled>(k));
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
					ManagedState managedState4 = managedComponentAccessor[l];
					ref LocalTransform ptr16 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(intPtr2, l);
					ref AgentMovementPlane ptr17 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentMovementPlane>(intPtr3, l);
					ref MovementControl ptr18 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementControl>(intPtr4, l);
					ref MovementSettings ptr19 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<MovementSettings>(intPtr5, l);
					ref AgentOffMeshLinkTraversal ptr20 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<AgentOffMeshLinkTraversal>(intPtr6, l);
					ManagedAgentOffMeshLinkTraversal managedAgentOffMeshLinkTraversal4 = managedComponentAccessor2[l];
					this.Execute(entity4, managedState4, ref ptr16, ref ptr17, ref ptr18, ref ptr19, ref ptr20, managedAgentOffMeshLinkTraversal4, enabledMask.GetEnabledRefRW<AgentOffMeshLinkMovementDisabled>(l), enabledMask2.GetEnabledRefRW<AgentOffMeshLinkLocalAvoidanceDisabled>(l));
					num++;
				}
				num3 >>= 1;
			}
		}

		// Token: 0x06001482 RID: 5250 RVA: 0x00077BC0 File Offset: 0x00075DC0
		private JobHandle __ThrowCodeGenException()
		{
			throw new Exception("This method should have been replaced by source gen.");
		}

		// Token: 0x06001483 RID: 5251 RVA: 0x00077BD7 File Offset: 0x00075DD7
		public void Run()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001484 RID: 5252 RVA: 0x00077BD7 File Offset: 0x00075DD7
		public void RunByRef()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001485 RID: 5253 RVA: 0x00077BD7 File Offset: 0x00075DD7
		public void Run(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001486 RID: 5254 RVA: 0x00077BD7 File Offset: 0x00075DD7
		public void RunByRef(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001487 RID: 5255 RVA: 0x00077BE0 File Offset: 0x00075DE0
		public JobHandle Schedule(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001488 RID: 5256 RVA: 0x00077BE0 File Offset: 0x00075DE0
		public JobHandle ScheduleByRef(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001489 RID: 5257 RVA: 0x00077BE0 File Offset: 0x00075DE0
		public JobHandle Schedule(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x0600148A RID: 5258 RVA: 0x00077BE0 File Offset: 0x00075DE0
		public JobHandle ScheduleByRef(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x0600148B RID: 5259 RVA: 0x00077BD7 File Offset: 0x00075DD7
		public void Schedule()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x0600148C RID: 5260 RVA: 0x00077BD7 File Offset: 0x00075DD7
		public void ScheduleByRef()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x0600148D RID: 5261 RVA: 0x00077BD7 File Offset: 0x00075DD7
		public void Schedule(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x0600148E RID: 5262 RVA: 0x00077BD7 File Offset: 0x00075DD7
		public void ScheduleByRef(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x0600148F RID: 5263 RVA: 0x00077BE0 File Offset: 0x00075DE0
		public JobHandle ScheduleParallel(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001490 RID: 5264 RVA: 0x00077BE0 File Offset: 0x00075DE0
		public JobHandle ScheduleParallelByRef(JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001491 RID: 5265 RVA: 0x00077BE0 File Offset: 0x00075DE0
		public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001492 RID: 5266 RVA: 0x00077BE0 File Offset: 0x00075DE0
		public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001493 RID: 5267 RVA: 0x00077BE0 File Offset: 0x00075DE0
		public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001494 RID: 5268 RVA: 0x00077BE0 File Offset: 0x00075DE0
		public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
		{
			return this.__ThrowCodeGenException();
		}

		// Token: 0x06001495 RID: 5269 RVA: 0x00077BD7 File Offset: 0x00075DD7
		public void ScheduleParallel()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001496 RID: 5270 RVA: 0x00077BD7 File Offset: 0x00075DD7
		public void ScheduleParallelByRef()
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001497 RID: 5271 RVA: 0x00077BD7 File Offset: 0x00075DD7
		public void ScheduleParallel(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001498 RID: 5272 RVA: 0x00077BD7 File Offset: 0x00075DD7
		public void ScheduleParallelByRef(EntityQuery query)
		{
			this.__ThrowCodeGenException();
		}

		// Token: 0x06001499 RID: 5273 RVA: 0x00077BE8 File Offset: 0x00075DE8
		void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
		}

		// Token: 0x04000E87 RID: 3719
		public EntityCommandBuffer commandBuffer;

		// Token: 0x04000E88 RID: 3720
		public float deltaTime;

		// Token: 0x04000E89 RID: 3721
		private JobManagedOffMeshLinkTransition.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		// Token: 0x04000E8A RID: 3722
		public EntityManager __EntityManager;

		// Token: 0x0200033A RID: 826
		public struct InternalCompilerQueryAndHandleData
		{
			// Token: 0x0600149A RID: 5274 RVA: 0x00077BF8 File Offset: 0x00075DF8
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				this.DefaultQuery = entityQueryBuilder.WithAllRW<ManagedState>().WithAllRW<LocalTransform>().WithAllRW<AgentMovementPlane>()
					.WithAllRW<MovementControl>()
					.WithAllRW<MovementSettings>()
					.WithAllRW<AgentOffMeshLinkTraversal>()
					.WithAllRW<ManagedAgentOffMeshLinkTraversal>()
					.WithAllRW<AgentOffMeshLinkMovementDisabled>()
					.WithAllRW<AgentOffMeshLinkLocalAvoidanceDisabled>()
					.WithOptions(EntityQueryOptions.IgnoreComponentEnabledState)
					.Build(ref state);
				entityQueryBuilder.Reset();
				entityQueryBuilder.Dispose();
			}

			// Token: 0x0600149B RID: 5275 RVA: 0x00077C80 File Offset: 0x00075E80
			public void Init(ref SystemState state, bool assignDefaultQuery)
			{
				if (assignDefaultQuery)
				{
					this.__AssignQueries(ref state);
				}
				this.__TypeHandle.__AssignHandles(ref state);
			}

			// Token: 0x0600149C RID: 5276 RVA: 0x00077C98 File Offset: 0x00075E98
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Run(ref JobManagedOffMeshLinkTransition job, EntityQuery query)
			{
				job.__TypeHandle = this.__TypeHandle;
				InternalCompilerInterface.JobChunkInterface.RunByRefWithoutJobs<JobManagedOffMeshLinkTransition>(ref job, query);
			}

			// Token: 0x0600149D RID: 5277 RVA: 0x00077CAD File Offset: 0x00075EAD
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref JobManagedOffMeshLinkTransition job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = this.__TypeHandle;
				return (ref job).ScheduleByRef<JobManagedOffMeshLinkTransition>(query, dependency);
			}

			// Token: 0x0600149E RID: 5278 RVA: 0x00077CC3 File Offset: 0x00075EC3
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref JobManagedOffMeshLinkTransition job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = this.__TypeHandle;
				return (ref job).ScheduleParallelByRef<JobManagedOffMeshLinkTransition>(query, dependency);
			}

			// Token: 0x0600149F RID: 5279 RVA: 0x00003788 File Offset: 0x00001988
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref JobManagedOffMeshLinkTransition job, EntityQuery query, ref SystemState state)
			{
			}

			// Token: 0x060014A0 RID: 5280 RVA: 0x000743B8 File Offset: 0x000725B8
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref JobManagedOffMeshLinkTransition job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			// Token: 0x060014A1 RID: 5281 RVA: 0x00077CD9 File Offset: 0x00075ED9
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref JobManagedOffMeshLinkTransition job, EntityManager entityManager)
			{
				job.__EntityManager = entityManager;
			}

			// Token: 0x04000E8B RID: 3723
			public JobManagedOffMeshLinkTransition.InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

			// Token: 0x04000E8C RID: 3724
			public EntityQuery DefaultQuery;

			// Token: 0x0200033B RID: 827
			public struct TypeHandle
			{
				// Token: 0x060014A2 RID: 5282 RVA: 0x00077CE4 File Offset: 0x00075EE4
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
					this.__Pathfinding_ECS_ManagedState_RW_ComponentTypeHandle = state.EntityManager.GetComponentTypeHandle<ManagedState>(false);
					this.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>(false);
					this.__Pathfinding_ECS_AgentMovementPlane_RW_ComponentTypeHandle = state.GetComponentTypeHandle<AgentMovementPlane>(false);
					this.__Pathfinding_ECS_MovementControl_RW_ComponentTypeHandle = state.GetComponentTypeHandle<MovementControl>(false);
					this.__Pathfinding_ECS_MovementSettings_RW_ComponentTypeHandle = state.GetComponentTypeHandle<MovementSettings>(false);
					this.__Pathfinding_ECS_AgentOffMeshLinkTraversal_RW_ComponentTypeHandle = state.GetComponentTypeHandle<AgentOffMeshLinkTraversal>(false);
					this.__Pathfinding_ECS_ManagedAgentOffMeshLinkTraversal_RW_ComponentTypeHandle = state.EntityManager.GetComponentTypeHandle<ManagedAgentOffMeshLinkTraversal>(false);
					this.__Pathfinding_ECS_AgentOffMeshLinkMovementDisabled_RW_ComponentTypeHandle = state.GetComponentTypeHandle<AgentOffMeshLinkMovementDisabled>(false);
					this.__Pathfinding_ECS_AgentOffMeshLinkLocalAvoidanceDisabled_RW_ComponentTypeHandle = state.GetComponentTypeHandle<AgentOffMeshLinkLocalAvoidanceDisabled>(false);
				}

				// Token: 0x060014A3 RID: 5283 RVA: 0x00077D84 File Offset: 0x00075F84
				public void Update(ref SystemState state)
				{
					this.__Unity_Entities_Entity_TypeHandle.Update(ref state);
					this.__Pathfinding_ECS_ManagedState_RW_ComponentTypeHandle.Update(ref state);
					this.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_AgentMovementPlane_RW_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_MovementControl_RW_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_MovementSettings_RW_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_AgentOffMeshLinkTraversal_RW_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_ManagedAgentOffMeshLinkTraversal_RW_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_AgentOffMeshLinkMovementDisabled_RW_ComponentTypeHandle.Update(ref state);
					this.__Pathfinding_ECS_AgentOffMeshLinkLocalAvoidanceDisabled_RW_ComponentTypeHandle.Update(ref state);
				}

				// Token: 0x04000E8D RID: 3725
				[ReadOnly]
				public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

				// Token: 0x04000E8E RID: 3726
				public ComponentTypeHandle<ManagedState> __Pathfinding_ECS_ManagedState_RW_ComponentTypeHandle;

				// Token: 0x04000E8F RID: 3727
				public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

				// Token: 0x04000E90 RID: 3728
				public ComponentTypeHandle<AgentMovementPlane> __Pathfinding_ECS_AgentMovementPlane_RW_ComponentTypeHandle;

				// Token: 0x04000E91 RID: 3729
				public ComponentTypeHandle<MovementControl> __Pathfinding_ECS_MovementControl_RW_ComponentTypeHandle;

				// Token: 0x04000E92 RID: 3730
				public ComponentTypeHandle<MovementSettings> __Pathfinding_ECS_MovementSettings_RW_ComponentTypeHandle;

				// Token: 0x04000E93 RID: 3731
				public ComponentTypeHandle<AgentOffMeshLinkTraversal> __Pathfinding_ECS_AgentOffMeshLinkTraversal_RW_ComponentTypeHandle;

				// Token: 0x04000E94 RID: 3732
				public ComponentTypeHandle<ManagedAgentOffMeshLinkTraversal> __Pathfinding_ECS_ManagedAgentOffMeshLinkTraversal_RW_ComponentTypeHandle;

				// Token: 0x04000E95 RID: 3733
				public ComponentTypeHandle<AgentOffMeshLinkMovementDisabled> __Pathfinding_ECS_AgentOffMeshLinkMovementDisabled_RW_ComponentTypeHandle;

				// Token: 0x04000E96 RID: 3734
				public ComponentTypeHandle<AgentOffMeshLinkLocalAvoidanceDisabled> __Pathfinding_ECS_AgentOffMeshLinkLocalAvoidanceDisabled_RW_ComponentTypeHandle;
			}
		}

		// Token: 0x0200033C RID: 828
		public struct InternalCompiler
		{
			// Token: 0x060014A4 RID: 5284 RVA: 0x000768D5 File Offset: 0x00074AD5
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
