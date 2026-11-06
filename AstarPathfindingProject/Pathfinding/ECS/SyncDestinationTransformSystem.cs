using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using UnityEngine;

namespace Pathfinding.ECS
{
	// Token: 0x020003A3 RID: 931
	[UpdateBefore(typeof(SchedulePathSearchSystem))]
	[UpdateInGroup(typeof(AIMovementSystemGroup))]
	[RequireMatchingQueriesForUpdate]
	[CompilerGenerated]
	public struct SyncDestinationTransformSystem : ISystem, ISystemCompilerGenerated
	{
		// Token: 0x06001784 RID: 6020 RVA: 0x0007F108 File Offset: 0x0007D308
		public unsafe void OnUpdate(ref SystemState systemState)
		{
			if (!AIMovementSystemGroup.TimeScaledRateManager.IsFirstSubstep)
			{
				return;
			}
			foreach (ValueTuple<InternalCompilerInterface.UncheckedRefRW<DestinationPoint>, SystemAPI.ManagedAPI.UnityEngineComponent<AIDestinationSetter>> valueTuple in SyncDestinationTransformSystem.IFE_315116516_0.Query(this.__query_315116516_0, this.__TypeHandle.__IFE_315116516_0_TypeHandle, ref systemState))
			{
				InternalCompilerInterface.UncheckedRefRW<DestinationPoint> item = valueTuple.Item1;
				SystemAPI.ManagedAPI.UnityEngineComponent<AIDestinationSetter> item2 = valueTuple.Item2;
				AIDestinationSetter value = item2.Value;
				if (value.target != null)
				{
					*item.ValueRW = new DestinationPoint
					{
						destination = value.target.position,
						facingDirection = (value.useRotation ? value.target.forward : Vector3.zero)
					};
				}
			}
		}

		// Token: 0x06001785 RID: 6021 RVA: 0x0007F1F0 File Offset: 0x0007D3F0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			this.__query_315116516_0 = entityQueryBuilder.WithAllRW<DestinationPoint>().WithAllRW<AIDestinationSetter>().Build(ref state);
			entityQueryBuilder.Reset();
			entityQueryBuilder.Dispose();
		}

		// Token: 0x06001786 RID: 6022 RVA: 0x0007F237 File Offset: 0x0007D437
		public void OnCreateForCompiler(ref SystemState state)
		{
			this.__AssignQueries(ref state);
			this.__TypeHandle.__AssignHandles(ref state);
		}

		// Token: 0x06001787 RID: 6023 RVA: 0x0007F24C File Offset: 0x0007D44C
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
		{
			((SyncDestinationTransformSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x06001788 RID: 6024 RVA: 0x0007F265 File Offset: 0x0007D465
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
		{
			((SyncDestinationTransformSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x04000F94 RID: 3988
		private SyncDestinationTransformSystem.TypeHandle __TypeHandle;

		// Token: 0x04000F95 RID: 3989
		private EntityQuery __query_315116516_0;

		// Token: 0x020003A4 RID: 932
		private readonly struct IFE_315116516_0
		{
			// Token: 0x06001789 RID: 6025 RVA: 0x0007F27E File Offset: 0x0007D47E
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static SyncDestinationTransformSystem.IFE_315116516_0.Enumerator Query(EntityQuery entityQuery, SyncDestinationTransformSystem.IFE_315116516_0.TypeHandle typeHandle, ref SystemState state)
			{
				return new SyncDestinationTransformSystem.IFE_315116516_0.Enumerator(entityQuery, typeHandle, ref state);
			}

			// Token: 0x0600178A RID: 6026 RVA: 0x0007F288 File Offset: 0x0007D488
			public static void CompleteDependencies(ref SystemState state)
			{
				state.EntityManager.CompleteDependencyBeforeRW<DestinationPoint>();
				state.EntityManager.CompleteDependencyBeforeRW<AIDestinationSetter>();
			}

			// Token: 0x020003A5 RID: 933
			public struct ResolvedChunk
			{
				// Token: 0x0600178B RID: 6027 RVA: 0x0007F2B1 File Offset: 0x0007D4B1
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public ValueTuple<InternalCompilerInterface.UncheckedRefRW<DestinationPoint>, SystemAPI.ManagedAPI.UnityEngineComponent<AIDestinationSetter>> Get(int index)
				{
					return new ValueTuple<InternalCompilerInterface.UncheckedRefRW<DestinationPoint>, SystemAPI.ManagedAPI.UnityEngineComponent<AIDestinationSetter>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<DestinationPoint>(this.item1_IntPtr, index), new SystemAPI.ManagedAPI.UnityEngineComponent<AIDestinationSetter>(this.item2_ManagedComponentAccessor[index]));
				}

				// Token: 0x04000F96 RID: 3990
				public IntPtr item1_IntPtr;

				// Token: 0x04000F97 RID: 3991
				public ManagedComponentAccessor<AIDestinationSetter> item2_ManagedComponentAccessor;
			}

			// Token: 0x020003A6 RID: 934
			public struct TypeHandle
			{
				// Token: 0x0600178C RID: 6028 RVA: 0x0007F2D8 File Offset: 0x0007D4D8
				public TypeHandle(ref SystemState systemState)
				{
					this._entityManager = systemState.EntityManager;
					this.item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<DestinationPoint>(false);
					this.item2_ManagedComponentTypeHandle_RO = systemState.EntityManager.GetComponentTypeHandle<AIDestinationSetter>(false);
				}

				// Token: 0x0600178D RID: 6029 RVA: 0x0007F313 File Offset: 0x0007D513
				public void Update(ref SystemState systemState)
				{
					this.item1_ComponentTypeHandle_RW.Update(ref systemState);
					this.item2_ManagedComponentTypeHandle_RO.Update(ref systemState);
				}

				// Token: 0x0600178E RID: 6030 RVA: 0x0007F330 File Offset: 0x0007D530
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public SyncDestinationTransformSystem.IFE_315116516_0.ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
				{
					return new SyncDestinationTransformSystem.IFE_315116516_0.ResolvedChunk
					{
						item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks<DestinationPoint>(in archetypeChunk, ref this.item1_ComponentTypeHandle_RW),
						item2_ManagedComponentAccessor = archetypeChunk.GetManagedComponentAccessor<AIDestinationSetter>(ref this.item2_ManagedComponentTypeHandle_RO, this._entityManager)
					};
				}

				// Token: 0x04000F98 RID: 3992
				public EntityManager _entityManager;

				// Token: 0x04000F99 RID: 3993
				private ComponentTypeHandle<DestinationPoint> item1_ComponentTypeHandle_RW;

				// Token: 0x04000F9A RID: 3994
				[ReadOnly]
				private ComponentTypeHandle<AIDestinationSetter> item2_ManagedComponentTypeHandle_RO;
			}

			// Token: 0x020003A7 RID: 935
			public struct Enumerator : IEnumerator<ValueTuple<InternalCompilerInterface.UncheckedRefRW<DestinationPoint>, SystemAPI.ManagedAPI.UnityEngineComponent<AIDestinationSetter>>>, IEnumerator, IDisposable
			{
				// Token: 0x0600178F RID: 6031 RVA: 0x0007F374 File Offset: 0x0007D574
				public Enumerator(EntityQuery entityQuery, SyncDestinationTransformSystem.IFE_315116516_0.TypeHandle typeHandle, ref SystemState state)
				{
					if (!entityQuery.IsEmptyIgnoreFilter)
					{
						SyncDestinationTransformSystem.IFE_315116516_0.CompleteDependencies(ref state);
						typeHandle.Update(ref state);
					}
					this._entityQueryEnumerator = new InternalEntityQueryEnumerator(entityQuery);
					this._currentEntityIndex = -1;
					this._endEntityIndex = -1;
					this._typeHandle = typeHandle;
					this._resolvedChunk = default(SyncDestinationTransformSystem.IFE_315116516_0.ResolvedChunk);
				}

				// Token: 0x06001790 RID: 6032 RVA: 0x0007F3C5 File Offset: 0x0007D5C5
				public void Dispose()
				{
					this._entityQueryEnumerator.Dispose();
				}

				// Token: 0x06001791 RID: 6033 RVA: 0x0007F3D4 File Offset: 0x0007D5D4
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

				// Token: 0x17000328 RID: 808
				// (get) Token: 0x06001792 RID: 6034 RVA: 0x0007F43A File Offset: 0x0007D63A
				public ValueTuple<InternalCompilerInterface.UncheckedRefRW<DestinationPoint>, SystemAPI.ManagedAPI.UnityEngineComponent<AIDestinationSetter>> Current
				{
					get
					{
						return this._resolvedChunk.Get(this._currentEntityIndex);
					}
				}

				// Token: 0x06001793 RID: 6035 RVA: 0x0007F44D File Offset: 0x0007D64D
				public SyncDestinationTransformSystem.IFE_315116516_0.Enumerator GetEnumerator()
				{
					return this;
				}

				// Token: 0x06001794 RID: 6036 RVA: 0x0001C8BA File Offset: 0x0001AABA
				public void Reset()
				{
					throw new NotImplementedException();
				}

				// Token: 0x17000329 RID: 809
				// (get) Token: 0x06001795 RID: 6037 RVA: 0x0001C8BA File Offset: 0x0001AABA
				object IEnumerator.Current
				{
					get
					{
						throw new NotImplementedException();
					}
				}

				// Token: 0x04000F9B RID: 3995
				private InternalEntityQueryEnumerator _entityQueryEnumerator;

				// Token: 0x04000F9C RID: 3996
				private SyncDestinationTransformSystem.IFE_315116516_0.TypeHandle _typeHandle;

				// Token: 0x04000F9D RID: 3997
				private SyncDestinationTransformSystem.IFE_315116516_0.ResolvedChunk _resolvedChunk;

				// Token: 0x04000F9E RID: 3998
				private int _currentEntityIndex;

				// Token: 0x04000F9F RID: 3999
				private int _endEntityIndex;
			}
		}

		// Token: 0x020003A8 RID: 936
		private struct TypeHandle
		{
			// Token: 0x06001796 RID: 6038 RVA: 0x0007F455 File Offset: 0x0007D655
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				this.__IFE_315116516_0_TypeHandle = new SyncDestinationTransformSystem.IFE_315116516_0.TypeHandle(ref state);
			}

			// Token: 0x04000FA0 RID: 4000
			public SyncDestinationTransformSystem.IFE_315116516_0.TypeHandle __IFE_315116516_0_TypeHandle;
		}
	}
}
