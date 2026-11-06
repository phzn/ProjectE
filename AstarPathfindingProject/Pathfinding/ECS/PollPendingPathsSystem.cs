using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Pathfinding.Sync;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;

namespace Pathfinding.ECS
{
	// Token: 0x0200038C RID: 908
	[BurstCompile]
	[CompilerGenerated]
	public struct PollPendingPathsSystem : ISystem, ISystemCompilerGenerated
	{
		// Token: 0x060016E1 RID: 5857 RVA: 0x0007DB5C File Offset: 0x0007BD5C
		public void OnCreate(ref SystemState state)
		{
			this.jobRepairPathScheduler = new JobRepairPath.Scheduler(ref state)
			{
				onlyApplyPendingPaths = true
			};
			this.entityQueryPrepare = this.jobRepairPathScheduler.GetEntityQuery(Allocator.Temp).Build(ref state);
			WorldUnmanaged world = state.WorldUnmanaged;
			Action action = delegate
			{
				PollPendingPathsSystem.anyPendingPaths = true;
				try
				{
					world.GetExistingUnmanagedSystem<PollPendingPathsSystem>().Update(world);
				}
				finally
				{
					PollPendingPathsSystem.anyPendingPaths = false;
				}
			};
			AstarPath.OnPathsCalculated = (Action)Delegate.Combine(AstarPath.OnPathsCalculated, action);
			this.onPathsCalculated = GCHandle.Alloc(action);
		}

		// Token: 0x060016E2 RID: 5858 RVA: 0x0007DBD8 File Offset: 0x0007BDD8
		public void OnDestroy(ref SystemState state)
		{
			AstarPath.OnPathsCalculated = (Action)Delegate.Remove(AstarPath.OnPathsCalculated, (Action)this.onPathsCalculated.Target);
			this.onPathsCalculated.Free();
			this.jobRepairPathScheduler.Dispose();
		}

		// Token: 0x060016E3 RID: 5859 RVA: 0x0007DC14 File Offset: 0x0007BE14
		private void OnUpdate(ref SystemState systemState)
		{
			if (!PollPendingPathsSystem.anyPendingPaths)
			{
				return;
			}
			foreach (ManagedState managedState in PollPendingPathsSystem.IFE_1964892062_0.Query(this.__query_1964892062_0, this.__TypeHandle.__IFE_1964892062_0_TypeHandle, ref systemState))
			{
				managedState.CancelCurrentPathRequest();
			}
			RWLock.ReadLockAsync readLockAsync = AstarPath.active.LockGraphDataForReading();
			systemState.Dependency = this.jobRepairPathScheduler.ScheduleParallel(ref systemState, this.entityQueryPrepare, systemState.Dependency);
			readLockAsync.UnlockAfter(systemState.Dependency);
		}

		// Token: 0x060016E4 RID: 5860 RVA: 0x0007DCB8 File Offset: 0x0007BEB8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			this.__query_1964892062_0 = entityQueryBuilder.WithAll<AgentOffMeshLinkTraversal>().WithAllRW<ManagedState>().Build(ref state);
			entityQueryBuilder.Reset();
			entityQueryBuilder.Dispose();
		}

		// Token: 0x060016E5 RID: 5861 RVA: 0x0007DCFF File Offset: 0x0007BEFF
		public void OnCreateForCompiler(ref SystemState state)
		{
			this.__AssignQueries(ref state);
			this.__TypeHandle.__AssignHandles(ref state);
		}

		// Token: 0x060016E6 RID: 5862 RVA: 0x0007DD14 File Offset: 0x0007BF14
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnCreate(IntPtr self, IntPtr state)
		{
			((PollPendingPathsSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x060016E7 RID: 5863 RVA: 0x0007DD2D File Offset: 0x0007BF2D
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
		{
			((PollPendingPathsSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x060016E8 RID: 5864 RVA: 0x0007DD46 File Offset: 0x0007BF46
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnDestroy(IntPtr self, IntPtr state)
		{
			((PollPendingPathsSystem*)self.ToPointer())->OnDestroy(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x060016E9 RID: 5865 RVA: 0x0007DD5F File Offset: 0x0007BF5F
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
		{
			((PollPendingPathsSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x04000F5A RID: 3930
		private GCHandle onPathsCalculated;

		// Token: 0x04000F5B RID: 3931
		private static bool anyPendingPaths;

		// Token: 0x04000F5C RID: 3932
		private JobRepairPath.Scheduler jobRepairPathScheduler;

		// Token: 0x04000F5D RID: 3933
		private EntityQuery entityQueryPrepare;

		// Token: 0x04000F5E RID: 3934
		private PollPendingPathsSystem.TypeHandle __TypeHandle;

		// Token: 0x04000F5F RID: 3935
		private EntityQuery __query_1964892062_0;

		// Token: 0x0200038D RID: 909
		private readonly struct IFE_1964892062_0
		{
			// Token: 0x060016EA RID: 5866 RVA: 0x0007DD78 File Offset: 0x0007BF78
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static PollPendingPathsSystem.IFE_1964892062_0.Enumerator Query(EntityQuery entityQuery, PollPendingPathsSystem.IFE_1964892062_0.TypeHandle typeHandle, ref SystemState state)
			{
				return new PollPendingPathsSystem.IFE_1964892062_0.Enumerator(entityQuery, typeHandle, ref state);
			}

			// Token: 0x060016EB RID: 5867 RVA: 0x0007DD84 File Offset: 0x0007BF84
			public static void CompleteDependencies(ref SystemState state)
			{
				state.EntityManager.CompleteDependencyBeforeRW<ManagedState>();
			}

			// Token: 0x0200038E RID: 910
			public struct ResolvedChunk
			{
				// Token: 0x060016EC RID: 5868 RVA: 0x0007DD9F File Offset: 0x0007BF9F
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public ManagedState Get(int index)
				{
					return this.item1_ManagedComponentAccessor[index];
				}

				// Token: 0x04000F60 RID: 3936
				public ManagedComponentAccessor<ManagedState> item1_ManagedComponentAccessor;
			}

			// Token: 0x0200038F RID: 911
			public struct TypeHandle
			{
				// Token: 0x060016ED RID: 5869 RVA: 0x0007DDB0 File Offset: 0x0007BFB0
				public TypeHandle(ref SystemState systemState)
				{
					this._entityManager = systemState.EntityManager;
					this.item1_ManagedComponentTypeHandle_RO = systemState.EntityManager.GetComponentTypeHandle<ManagedState>(false);
				}

				// Token: 0x060016EE RID: 5870 RVA: 0x0007DDDE File Offset: 0x0007BFDE
				public void Update(ref SystemState systemState)
				{
					this.item1_ManagedComponentTypeHandle_RO.Update(ref systemState);
				}

				// Token: 0x060016EF RID: 5871 RVA: 0x0007DDEC File Offset: 0x0007BFEC
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public PollPendingPathsSystem.IFE_1964892062_0.ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
				{
					return new PollPendingPathsSystem.IFE_1964892062_0.ResolvedChunk
					{
						item1_ManagedComponentAccessor = archetypeChunk.GetManagedComponentAccessor<ManagedState>(ref this.item1_ManagedComponentTypeHandle_RO, this._entityManager)
					};
				}

				// Token: 0x04000F61 RID: 3937
				public EntityManager _entityManager;

				// Token: 0x04000F62 RID: 3938
				[ReadOnly]
				private ComponentTypeHandle<ManagedState> item1_ManagedComponentTypeHandle_RO;
			}

			// Token: 0x02000390 RID: 912
			public struct Enumerator : IEnumerator<ManagedState>, IEnumerator, IDisposable
			{
				// Token: 0x060016F0 RID: 5872 RVA: 0x0007DE1C File Offset: 0x0007C01C
				public Enumerator(EntityQuery entityQuery, PollPendingPathsSystem.IFE_1964892062_0.TypeHandle typeHandle, ref SystemState state)
				{
					if (!entityQuery.IsEmptyIgnoreFilter)
					{
						PollPendingPathsSystem.IFE_1964892062_0.CompleteDependencies(ref state);
						typeHandle.Update(ref state);
					}
					this._entityQueryEnumerator = new InternalEntityQueryEnumerator(entityQuery);
					this._currentEntityIndex = -1;
					this._endEntityIndex = -1;
					this._typeHandle = typeHandle;
					this._resolvedChunk = default(PollPendingPathsSystem.IFE_1964892062_0.ResolvedChunk);
				}

				// Token: 0x060016F1 RID: 5873 RVA: 0x0007DE6D File Offset: 0x0007C06D
				public void Dispose()
				{
					this._entityQueryEnumerator.Dispose();
				}

				// Token: 0x060016F2 RID: 5874 RVA: 0x0007DE7C File Offset: 0x0007C07C
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

				// Token: 0x17000326 RID: 806
				// (get) Token: 0x060016F3 RID: 5875 RVA: 0x0007DEE2 File Offset: 0x0007C0E2
				public ManagedState Current
				{
					get
					{
						return this._resolvedChunk.Get(this._currentEntityIndex);
					}
				}

				// Token: 0x060016F4 RID: 5876 RVA: 0x0007DEF5 File Offset: 0x0007C0F5
				public PollPendingPathsSystem.IFE_1964892062_0.Enumerator GetEnumerator()
				{
					return this;
				}

				// Token: 0x060016F5 RID: 5877 RVA: 0x0001C8BA File Offset: 0x0001AABA
				public void Reset()
				{
					throw new NotImplementedException();
				}

				// Token: 0x17000327 RID: 807
				// (get) Token: 0x060016F6 RID: 5878 RVA: 0x0001C8BA File Offset: 0x0001AABA
				object IEnumerator.Current
				{
					get
					{
						throw new NotImplementedException();
					}
				}

				// Token: 0x04000F63 RID: 3939
				private InternalEntityQueryEnumerator _entityQueryEnumerator;

				// Token: 0x04000F64 RID: 3940
				private PollPendingPathsSystem.IFE_1964892062_0.TypeHandle _typeHandle;

				// Token: 0x04000F65 RID: 3941
				private PollPendingPathsSystem.IFE_1964892062_0.ResolvedChunk _resolvedChunk;

				// Token: 0x04000F66 RID: 3942
				private int _currentEntityIndex;

				// Token: 0x04000F67 RID: 3943
				private int _endEntityIndex;
			}
		}

		// Token: 0x02000391 RID: 913
		private struct TypeHandle
		{
			// Token: 0x060016F7 RID: 5879 RVA: 0x0007DEFD File Offset: 0x0007C0FD
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				this.__IFE_1964892062_0_TypeHandle = new PollPendingPathsSystem.IFE_1964892062_0.TypeHandle(ref state);
			}

			// Token: 0x04000F68 RID: 3944
			public PollPendingPathsSystem.IFE_1964892062_0.TypeHandle __IFE_1964892062_0_TypeHandle;
		}
	}
}
