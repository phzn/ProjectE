using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Jobs;

namespace Pathfinding.ECS
{
	// Token: 0x020003AC RID: 940
	[UpdateInGroup(typeof(AIMovementSystemGroup))]
	[UpdateBefore(typeof(FollowerControlSystem))]
	[UpdateBefore(typeof(RepairPathSystem))]
	[CompilerGenerated]
	public struct TraverseOffMeshLinkSystem : ISystem, ISystemCompilerGenerated
	{
		// Token: 0x0600179F RID: 6047 RVA: 0x0007F720 File Offset: 0x0007D920
		public void OnCreate(ref SystemState state)
		{
			this.jobRepairPathScheduler = new JobRepairPath.Scheduler(ref state);
			this.entityQueryPrepare = this.jobRepairPathScheduler.GetEntityQuery(Allocator.Temp).WithAll<SimulateMovement, SimulateMovementRepair>().Build(ref state);
			this.entityQueryOffMeshLinkCleanup = state.GetEntityQuery(new ComponentType[]
			{
				ComponentType.ReadOnly<ManagedAgentOffMeshLinkTraversal>(),
				ComponentType.Exclude<AgentOffMeshLinkTraversal>()
			});
		}

		// Token: 0x060017A0 RID: 6048 RVA: 0x0007F786 File Offset: 0x0007D986
		public void OnDestroy(ref SystemState state)
		{
			this.jobRepairPathScheduler.Dispose();
		}

		// Token: 0x060017A1 RID: 6049 RVA: 0x0007F794 File Offset: 0x0007D994
		public void OnUpdate(ref SystemState systemState)
		{
			if (AstarPath.active == null)
			{
				return;
			}
			if (this.__query_1565301199_1.IsEmptyIgnoreFilter)
			{
				return;
			}
			EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(systemState.WorldUpdateAllocator);
			this.StartOffMeshLinkTraversal(ref systemState, entityCommandBuffer);
			entityCommandBuffer.Playback(systemState.EntityManager);
			entityCommandBuffer.Dispose();
			this.ProcessActiveOffMeshLinkTraversal(ref systemState);
		}

		// Token: 0x060017A2 RID: 6050 RVA: 0x0007F7F4 File Offset: 0x0007D9F4
		private void StartOffMeshLinkTraversal(ref SystemState systemState, EntityCommandBuffer commandBuffer)
		{
			foreach (QueryEnumerableWithEntity<ManagedState, ManagedSettings> queryEnumerableWithEntity in TraverseOffMeshLinkSystem.IFE_1565301199_0.Query(this.__query_1565301199_0, this.__TypeHandle.__IFE_1565301199_0_TypeHandle, ref systemState))
			{
				ManagedState managedState;
				ManagedSettings managedSettings;
				Entity entity;
				queryEnumerableWithEntity.Deconstruct(out managedState, out managedSettings, out entity);
				ManagedState managedState2 = managedState;
				ManagedSettings managedSettings2 = managedSettings;
				Entity entity2 = entity;
				OffMeshLinks.OffMeshLinkTracer offMeshLinkTracer = TraverseOffMeshLinkSystem.NextLinkToTraverse(managedState2);
				AgentOffMeshLinkTraversalContext agentOffMeshLinkTraversalContext = new AgentOffMeshLinkTraversalContext(offMeshLinkTracer.link);
				commandBuffer.AddComponent<AgentOffMeshLinkTraversal>(entity2, new AgentOffMeshLinkTraversal(offMeshLinkTracer));
				commandBuffer.AddComponent(entity2, new ManagedAgentOffMeshLinkTraversal(agentOffMeshLinkTraversalContext, TraverseOffMeshLinkSystem.ResolveOffMeshLinkHandler(managedSettings2, agentOffMeshLinkTraversalContext)));
				commandBuffer.AddComponent<AgentOffMeshLinkMovementDisabled>(entity2, default(AgentOffMeshLinkMovementDisabled));
				commandBuffer.AddComponent<AgentOffMeshLinkLocalAvoidanceDisabled>(entity2, default(AgentOffMeshLinkLocalAvoidanceDisabled));
			}
		}

		// Token: 0x060017A3 RID: 6051 RVA: 0x0007F8CC File Offset: 0x0007DACC
		public static OffMeshLinks.OffMeshLinkTracer NextLinkToTraverse(ManagedState state)
		{
			return state.pathTracer.GetLinkInfo(1);
		}

		// Token: 0x060017A4 RID: 6052 RVA: 0x0007F8DA File Offset: 0x0007DADA
		public static IOffMeshLinkHandler ResolveOffMeshLinkHandler(ManagedSettings settings, AgentOffMeshLinkTraversalContext ctx)
		{
			return settings.onTraverseOffMeshLink ?? ctx.concreteLink.handler;
		}

		// Token: 0x060017A5 RID: 6053 RVA: 0x0007F8F4 File Offset: 0x0007DAF4
		private void ProcessActiveOffMeshLinkTraversal(ref SystemState systemState)
		{
			EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(systemState.WorldUpdateAllocator);
			systemState.CompleteDependency();
			this.__ScheduleViaJobChunkExtension_0(new JobManagedOffMeshLinkTransition
			{
				commandBuffer = entityCommandBuffer,
				deltaTime = AIMovementSystemGroup.TimeScaledRateManager.CheapStepDeltaTime
			}, this.__TypeHandle.__Pathfinding_ECS_JobManagedOffMeshLinkTransition_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, systemState.Dependency, ref systemState, false);
			if (!this.entityQueryOffMeshLinkCleanup.IsEmptyIgnoreFilter)
			{
				this.__ScheduleViaJobChunkExtension_1(default(JobManagedOffMeshLinkTransitionCleanup), this.entityQueryOffMeshLinkCleanup, systemState.Dependency, ref systemState, true);
				entityCommandBuffer.RemoveComponent<ManagedAgentOffMeshLinkTraversal>(this.entityQueryOffMeshLinkCleanup, EntityQueryCaptureMode.AtPlayback);
				entityCommandBuffer.RemoveComponent<AgentOffMeshLinkMovementDisabled>(this.entityQueryOffMeshLinkCleanup, EntityQueryCaptureMode.AtPlayback);
				entityCommandBuffer.RemoveComponent<AgentOffMeshLinkLocalAvoidanceDisabled>(this.entityQueryOffMeshLinkCleanup, EntityQueryCaptureMode.AtPlayback);
			}
			entityCommandBuffer.Playback(systemState.EntityManager);
			entityCommandBuffer.Dispose();
		}

		// Token: 0x060017A6 RID: 6054 RVA: 0x0007F9BC File Offset: 0x0007DBBC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __ScheduleViaJobChunkExtension_0(JobManagedOffMeshLinkTransition job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
		{
			this.__TypeHandle.__Pathfinding_ECS_JobManagedOffMeshLinkTransition_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, ref state);
			this.__TypeHandle.__Pathfinding_ECS_JobManagedOffMeshLinkTransition_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
			this.__TypeHandle.__Pathfinding_ECS_JobManagedOffMeshLinkTransition_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
			this.__TypeHandle.__Pathfinding_ECS_JobManagedOffMeshLinkTransition_WithDefaultQuery_JobEntityTypeHandle.Run(ref job, query);
		}

		// Token: 0x060017A7 RID: 6055 RVA: 0x0007FA24 File Offset: 0x0007DC24
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __ScheduleViaJobChunkExtension_1(JobManagedOffMeshLinkTransitionCleanup job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
		{
			this.__TypeHandle.__Pathfinding_ECS_JobManagedOffMeshLinkTransitionCleanup_WithoutDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, ref state);
			this.__TypeHandle.__Pathfinding_ECS_JobManagedOffMeshLinkTransitionCleanup_WithoutDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
			this.__TypeHandle.__Pathfinding_ECS_JobManagedOffMeshLinkTransitionCleanup_WithoutDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
			this.__TypeHandle.__Pathfinding_ECS_JobManagedOffMeshLinkTransitionCleanup_WithoutDefaultQuery_JobEntityTypeHandle.Run(ref job, query);
		}

		// Token: 0x060017A8 RID: 6056 RVA: 0x0007FA8C File Offset: 0x0007DC8C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			this.__query_1565301199_0 = entityQueryBuilder.WithNone<AgentOffMeshLinkTraversal>().WithAll<ReadyToTraverseOffMeshLink>().WithAllRW<ManagedState>()
				.WithAllRW<ManagedSettings>()
				.Build(ref state);
			entityQueryBuilder.Reset();
			this.__query_1565301199_1 = entityQueryBuilder.WithAny<AgentOffMeshLinkTraversal, ReadyToTraverseOffMeshLink>().Build(ref state);
			entityQueryBuilder.Reset();
			entityQueryBuilder.Dispose();
		}

		// Token: 0x060017A9 RID: 6057 RVA: 0x0007FB01 File Offset: 0x0007DD01
		public void OnCreateForCompiler(ref SystemState state)
		{
			this.__AssignQueries(ref state);
			this.__TypeHandle.__AssignHandles(ref state);
		}

		// Token: 0x060017AA RID: 6058 RVA: 0x0007FB16 File Offset: 0x0007DD16
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnCreate(IntPtr self, IntPtr state)
		{
			((TraverseOffMeshLinkSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x060017AB RID: 6059 RVA: 0x0007FB2F File Offset: 0x0007DD2F
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
		{
			((TraverseOffMeshLinkSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x060017AC RID: 6060 RVA: 0x0007FB48 File Offset: 0x0007DD48
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnDestroy(IntPtr self, IntPtr state)
		{
			((TraverseOffMeshLinkSystem*)self.ToPointer())->OnDestroy(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x060017AD RID: 6061 RVA: 0x0007FB61 File Offset: 0x0007DD61
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
		{
			((TraverseOffMeshLinkSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x04000FAF RID: 4015
		private EntityQuery entityQueryPrepare;

		// Token: 0x04000FB0 RID: 4016
		private EntityQuery entityQueryOffMeshLinkCleanup;

		// Token: 0x04000FB1 RID: 4017
		public JobRepairPath.Scheduler jobRepairPathScheduler;

		// Token: 0x04000FB2 RID: 4018
		private TraverseOffMeshLinkSystem.TypeHandle __TypeHandle;

		// Token: 0x04000FB3 RID: 4019
		private EntityQuery __query_1565301199_0;

		// Token: 0x04000FB4 RID: 4020
		private EntityQuery __query_1565301199_1;

		// Token: 0x020003AD RID: 941
		private readonly struct IFE_1565301199_0
		{
			// Token: 0x060017AE RID: 6062 RVA: 0x0007FB7A File Offset: 0x0007DD7A
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static TraverseOffMeshLinkSystem.IFE_1565301199_0.Enumerator Query(EntityQuery entityQuery, TraverseOffMeshLinkSystem.IFE_1565301199_0.TypeHandle typeHandle, ref SystemState state)
			{
				return new TraverseOffMeshLinkSystem.IFE_1565301199_0.Enumerator(entityQuery, typeHandle, ref state);
			}

			// Token: 0x060017AF RID: 6063 RVA: 0x0007FB84 File Offset: 0x0007DD84
			public static void CompleteDependencies(ref SystemState state)
			{
				state.EntityManager.CompleteDependencyBeforeRW<ManagedState>();
				state.EntityManager.CompleteDependencyBeforeRW<ManagedSettings>();
			}

			// Token: 0x020003AE RID: 942
			public struct ResolvedChunk
			{
				// Token: 0x060017B0 RID: 6064 RVA: 0x0007FBAD File Offset: 0x0007DDAD
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public QueryEnumerableWithEntity<ManagedState, ManagedSettings> Get(int index)
				{
					return new QueryEnumerableWithEntity<ManagedState, ManagedSettings>(this.item1_ManagedComponentAccessor[index], this.item2_ManagedComponentAccessor[index], InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(this.Entity_IntPtr, index));
				}

				// Token: 0x04000FB5 RID: 4021
				public ManagedComponentAccessor<ManagedState> item1_ManagedComponentAccessor;

				// Token: 0x04000FB6 RID: 4022
				public ManagedComponentAccessor<ManagedSettings> item2_ManagedComponentAccessor;

				// Token: 0x04000FB7 RID: 4023
				public IntPtr Entity_IntPtr;
			}

			// Token: 0x020003AF RID: 943
			public struct TypeHandle
			{
				// Token: 0x060017B1 RID: 6065 RVA: 0x0007FBD8 File Offset: 0x0007DDD8
				public TypeHandle(ref SystemState systemState)
				{
					this._entityManager = systemState.EntityManager;
					this.item1_ManagedComponentTypeHandle_RO = systemState.EntityManager.GetComponentTypeHandle<ManagedState>(false);
					this.item2_ManagedComponentTypeHandle_RO = systemState.EntityManager.GetComponentTypeHandle<ManagedSettings>(false);
					this.Entity_TypeHandle = systemState.GetEntityTypeHandle();
				}

				// Token: 0x060017B2 RID: 6066 RVA: 0x0007FC27 File Offset: 0x0007DE27
				public void Update(ref SystemState systemState)
				{
					this.item1_ManagedComponentTypeHandle_RO.Update(ref systemState);
					this.item2_ManagedComponentTypeHandle_RO.Update(ref systemState);
					this.Entity_TypeHandle.Update(ref systemState);
				}

				// Token: 0x060017B3 RID: 6067 RVA: 0x0007FC50 File Offset: 0x0007DE50
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public TraverseOffMeshLinkSystem.IFE_1565301199_0.ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
				{
					return new TraverseOffMeshLinkSystem.IFE_1565301199_0.ResolvedChunk
					{
						item1_ManagedComponentAccessor = archetypeChunk.GetManagedComponentAccessor<ManagedState>(ref this.item1_ManagedComponentTypeHandle_RO, this._entityManager),
						item2_ManagedComponentAccessor = archetypeChunk.GetManagedComponentAccessor<ManagedSettings>(ref this.item2_ManagedComponentTypeHandle_RO, this._entityManager),
						Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, this.Entity_TypeHandle)
					};
				}

				// Token: 0x04000FB8 RID: 4024
				public EntityManager _entityManager;

				// Token: 0x04000FB9 RID: 4025
				[ReadOnly]
				private ComponentTypeHandle<ManagedState> item1_ManagedComponentTypeHandle_RO;

				// Token: 0x04000FBA RID: 4026
				[ReadOnly]
				private ComponentTypeHandle<ManagedSettings> item2_ManagedComponentTypeHandle_RO;

				// Token: 0x04000FBB RID: 4027
				private EntityTypeHandle Entity_TypeHandle;
			}

			// Token: 0x020003B0 RID: 944
			public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<ManagedState, ManagedSettings>>, IEnumerator, IDisposable
			{
				// Token: 0x060017B4 RID: 6068 RVA: 0x0007FCB0 File Offset: 0x0007DEB0
				public Enumerator(EntityQuery entityQuery, TraverseOffMeshLinkSystem.IFE_1565301199_0.TypeHandle typeHandle, ref SystemState state)
				{
					if (!entityQuery.IsEmptyIgnoreFilter)
					{
						TraverseOffMeshLinkSystem.IFE_1565301199_0.CompleteDependencies(ref state);
						typeHandle.Update(ref state);
					}
					this._entityQueryEnumerator = new InternalEntityQueryEnumerator(entityQuery);
					this._currentEntityIndex = -1;
					this._endEntityIndex = -1;
					this._typeHandle = typeHandle;
					this._resolvedChunk = default(TraverseOffMeshLinkSystem.IFE_1565301199_0.ResolvedChunk);
				}

				// Token: 0x060017B5 RID: 6069 RVA: 0x0007FD01 File Offset: 0x0007DF01
				public void Dispose()
				{
					this._entityQueryEnumerator.Dispose();
				}

				// Token: 0x060017B6 RID: 6070 RVA: 0x0007FD10 File Offset: 0x0007DF10
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public bool MoveNext()
				{
					this._currentEntityIndex++;
					if (this._currentEntityIndex < this._endEntityIndex)
					{
						return true;
					}
					bool flag;
					ArchetypeChunk archetypeChunk;
					int num;
					int num2;
					if (this._entityQueryEnumerator.MoveNextEntityRange(out flag, out archetypeChunk, out num, out num2))
					{
						if (flag)
						{
							this._resolvedChunk = this._typeHandle.Resolve(archetypeChunk);
						}
						this._currentEntityIndex = num;
						this._endEntityIndex = num2;
						return true;
					}
					return false;
				}

				// Token: 0x1700032A RID: 810
				// (get) Token: 0x060017B7 RID: 6071 RVA: 0x0007FD76 File Offset: 0x0007DF76
				public QueryEnumerableWithEntity<ManagedState, ManagedSettings> Current
				{
					get
					{
						return this._resolvedChunk.Get(this._currentEntityIndex);
					}
				}

				// Token: 0x060017B8 RID: 6072 RVA: 0x0007FD89 File Offset: 0x0007DF89
				public TraverseOffMeshLinkSystem.IFE_1565301199_0.Enumerator GetEnumerator()
				{
					return this;
				}

				// Token: 0x060017B9 RID: 6073 RVA: 0x0001C8BA File Offset: 0x0001AABA
				public void Reset()
				{
					throw new NotImplementedException();
				}

				// Token: 0x1700032B RID: 811
				// (get) Token: 0x060017BA RID: 6074 RVA: 0x0001C8BA File Offset: 0x0001AABA
				object IEnumerator.Current
				{
					get
					{
						throw new NotImplementedException();
					}
				}

				// Token: 0x04000FBC RID: 4028
				private InternalEntityQueryEnumerator _entityQueryEnumerator;

				// Token: 0x04000FBD RID: 4029
				private TraverseOffMeshLinkSystem.IFE_1565301199_0.TypeHandle _typeHandle;

				// Token: 0x04000FBE RID: 4030
				private TraverseOffMeshLinkSystem.IFE_1565301199_0.ResolvedChunk _resolvedChunk;

				// Token: 0x04000FBF RID: 4031
				private int _currentEntityIndex;

				// Token: 0x04000FC0 RID: 4032
				private int _endEntityIndex;
			}
		}

		// Token: 0x020003B1 RID: 945
		private struct TypeHandle
		{
			// Token: 0x060017BB RID: 6075 RVA: 0x0007FD91 File Offset: 0x0007DF91
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				this.__IFE_1565301199_0_TypeHandle = new TraverseOffMeshLinkSystem.IFE_1565301199_0.TypeHandle(ref state);
				this.__Pathfinding_ECS_JobManagedOffMeshLinkTransition_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, true);
				this.__Pathfinding_ECS_JobManagedOffMeshLinkTransitionCleanup_WithoutDefaultQuery_JobEntityTypeHandle.Init(ref state, false);
			}

			// Token: 0x04000FC1 RID: 4033
			public TraverseOffMeshLinkSystem.IFE_1565301199_0.TypeHandle __IFE_1565301199_0_TypeHandle;

			// Token: 0x04000FC2 RID: 4034
			public JobManagedOffMeshLinkTransition.InternalCompilerQueryAndHandleData __Pathfinding_ECS_JobManagedOffMeshLinkTransition_WithDefaultQuery_JobEntityTypeHandle;

			// Token: 0x04000FC3 RID: 4035
			public JobManagedOffMeshLinkTransitionCleanup.InternalCompilerQueryAndHandleData __Pathfinding_ECS_JobManagedOffMeshLinkTransitionCleanup_WithoutDefaultQuery_JobEntityTypeHandle;
		}
	}
}
