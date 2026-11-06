using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Profiling;

namespace Unity.Transforms
{
	// Token: 0x02000005 RID: 5
	[WorldSystemFilter(WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor, WorldSystemFilterFlags.Default)]
	[UpdateInGroup(typeof(TransformSystemGroup))]
	[BurstCompile]
	[RequireMatchingQueriesForUpdate]
	[CompilerGenerated]
	public struct ParentSystem : ISystem, ISystemCompilerGenerated
	{
		// Token: 0x06000004 RID: 4 RVA: 0x000020C8 File Offset: 0x000002C8
		private int FindChildIndex(DynamicBuffer<Child> children, Entity entity)
		{
			for (int i = 0; i < children.Length; i++)
			{
				if (children[i].Value == entity)
				{
					return i;
				}
			}
			throw new InvalidOperationException("Child entity not in parent");
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002108 File Offset: 0x00000308
		private void RemoveChildFromParent(ref SystemState state, Entity childEntity, Entity parentEntity)
		{
			if (!state.EntityManager.HasComponent<Child>(parentEntity))
			{
				return;
			}
			DynamicBuffer<Child> buffer = state.EntityManager.GetBuffer<Child>(parentEntity, false);
			int num = this.FindChildIndex(buffer, childEntity);
			buffer.RemoveAt(num);
			if (buffer.Length == 0)
			{
				state.EntityManager.RemoveComponent(parentEntity, ComponentType.FromTypeIndex(TypeManager.GetTypeIndex<Child>()));
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000216C File Offset: 0x0000036C
		[BurstCompile]
		public void OnCreate(ref SystemState state)
		{
			this._childLookupRo = state.GetBufferLookup<Child>(true);
			this._childLookupRw = state.GetBufferLookup<Child>(false);
			this.ParentFromEntityRO = state.GetComponentLookup<Parent>(true);
			this.PreviousParentTypeHandleRW = state.GetComponentTypeHandle<PreviousParent>(false);
			this.ParentTypeHandleRO = state.GetComponentTypeHandle<Parent>(true);
			this.EntityTypeHandle = state.GetEntityTypeHandle();
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp).WithAll<Parent>().WithNone<PreviousParent>().WithOptions(EntityQueryOptions.FilterWriteGroup);
			this.m_NewParentsQuery = state.GetEntityQuery(in entityQueryBuilder);
			EntityQueryBuilder entityQueryBuilder2 = new EntityQueryBuilder(Allocator.Temp).WithAllRW<PreviousParent>().WithNone<Parent>().WithOptions(EntityQueryOptions.FilterWriteGroup);
			this.m_RemovedParentsQuery = state.GetEntityQuery(in entityQueryBuilder2);
			EntityQueryBuilder entityQueryBuilder3 = new EntityQueryBuilder(Allocator.Temp).WithAll<Parent>().WithAllRW<PreviousParent>().WithOptions(EntityQueryOptions.FilterWriteGroup);
			this.m_ExistingParentsQuery = state.GetEntityQuery(in entityQueryBuilder3);
			this.m_ExistingParentsQuery.ResetFilter();
			this.m_ExistingParentsQuery.AddChangedVersionFilter(ComponentType.ReadWrite<Parent>());
			this.m_ExistingParentsQuery.AddChangedVersionFilter(ComponentType.ReadWrite<PreviousParent>());
			EntityQueryBuilder entityQueryBuilder4 = new EntityQueryBuilder(Allocator.Temp).WithAllRW<Child>().WithNone<LocalToWorld>().WithOptions(EntityQueryOptions.FilterWriteGroup);
			this.m_DeletedParentsQuery = state.GetEntityQuery(in entityQueryBuilder4);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000022CC File Offset: 0x000004CC
		private void UpdateNewParents(ref SystemState state)
		{
			if (this.m_NewParentsQuery.IsEmptyIgnoreFilter)
			{
				return;
			}
			state.EntityManager.AddComponent(this.m_NewParentsQuery, ComponentType.FromTypeIndex(TypeManager.GetTypeIndex<PreviousParent>()));
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002308 File Offset: 0x00000508
		private void UpdateRemoveParents(ref SystemState state)
		{
			if (this.m_RemovedParentsQuery.IsEmptyIgnoreFilter)
			{
				return;
			}
			NativeArray<Entity> nativeArray = this.m_RemovedParentsQuery.ToEntityArray(state.WorldUnmanaged.UpdateAllocator.ToAllocator);
			NativeArray<PreviousParent> nativeArray2 = this.m_RemovedParentsQuery.ToComponentDataArray<PreviousParent>(state.WorldUnmanaged.UpdateAllocator.ToAllocator);
			for (int i = 0; i < nativeArray.Length; i++)
			{
				Entity entity = nativeArray[i];
				Entity value = nativeArray2[i].Value;
				this.RemoveChildFromParent(ref state, entity, value);
			}
			state.EntityManager.RemoveComponent(this.m_RemovedParentsQuery, ComponentType.FromTypeIndex(TypeManager.GetTypeIndex<PreviousParent>()));
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000023C0 File Offset: 0x000005C0
		private void UpdateChangeParents(ref SystemState state)
		{
			if (this.m_ExistingParentsQuery.IsEmptyIgnoreFilter)
			{
				return;
			}
			int num = this.m_ExistingParentsQuery.CalculateEntityCount() * 2;
			if (num == 0)
			{
				return;
			}
			NativeParallelMultiHashMap<Entity, Entity> nativeParallelMultiHashMap = new NativeParallelMultiHashMap<Entity, Entity>(num, state.WorldUnmanaged.UpdateAllocator.ToAllocator);
			NativeParallelMultiHashMap<Entity, Entity> nativeParallelMultiHashMap2 = new NativeParallelMultiHashMap<Entity, Entity>(num, state.WorldUnmanaged.UpdateAllocator.ToAllocator);
			NativeParallelHashSet<Entity> nativeParallelHashSet = new NativeParallelHashSet<Entity>(num, state.WorldUnmanaged.UpdateAllocator.ToAllocator);
			NativeParallelHashMap<Entity, int> nativeParallelHashMap = new NativeParallelHashMap<Entity, int>(num, state.WorldUnmanaged.UpdateAllocator.ToAllocator);
			this.ParentTypeHandleRO.Update(ref state);
			this.PreviousParentTypeHandleRW.Update(ref state);
			this.EntityTypeHandle.Update(ref state);
			this._childLookupRw.Update(ref state);
			ParentSystem.GatherChangedParents gatherChangedParents = default(ParentSystem.GatherChangedParents);
			gatherChangedParents.ParentChildrenToAdd = nativeParallelMultiHashMap.AsParallelWriter();
			gatherChangedParents.ParentChildrenToRemove = nativeParallelMultiHashMap2.AsParallelWriter();
			gatherChangedParents.ChildParentToRemove = nativeParallelHashSet.AsParallelWriter();
			gatherChangedParents.UniqueParents = nativeParallelHashMap.AsParallelWriter();
			gatherChangedParents.PreviousParentTypeHandle = this.PreviousParentTypeHandleRW;
			gatherChangedParents.ChildLookup = this._childLookupRw;
			gatherChangedParents.EntityStorageInfoLookup = state.GetEntityStorageInfoLookup();
			gatherChangedParents.ParentTypeHandle = this.ParentTypeHandleRO;
			gatherChangedParents.EntityTypeHandle = this.EntityTypeHandle;
			gatherChangedParents.LastSystemVersion = state.LastSystemVersion;
			gatherChangedParents.ScheduleParallel(this.m_ExistingParentsQuery, state.Dependency).Complete();
			NativeArray<Entity> nativeArray = nativeParallelHashSet.ToNativeArray(state.WorldUnmanaged.UpdateAllocator.ToAllocator);
			state.EntityManager.RemoveComponent(nativeArray, ComponentType.ReadWrite<Parent>());
			NativeList<Entity> nativeList = new NativeList<Entity>(state.WorldUnmanaged.UpdateAllocator.ToAllocator);
			this._childLookupRo.Update(ref state);
			ParentSystem.FindMissingChild findMissingChild = default(ParentSystem.FindMissingChild);
			findMissingChild.UniqueParents = nativeParallelHashMap;
			findMissingChild.ChildLookup = this._childLookupRo;
			findMissingChild.ParentsMissingChild = nativeList;
			findMissingChild.Schedule(default(JobHandle)).Complete();
			ComponentTypeSet componentTypeSet = new ComponentTypeSet(ComponentType.ReadWrite<Child>());
			state.EntityManager.AddComponent(nativeList.AsArray(), in componentTypeSet);
			this._childLookupRw.Update(ref state);
			ParentSystem.FixupChangedChildren fixupChangedChildren = default(ParentSystem.FixupChangedChildren);
			fixupChangedChildren.ParentChildrenToAdd = nativeParallelMultiHashMap;
			fixupChangedChildren.ParentChildrenToRemove = nativeParallelMultiHashMap2;
			fixupChangedChildren.UniqueParents = nativeParallelHashMap;
			fixupChangedChildren.ChildLookup = this._childLookupRw;
			fixupChangedChildren.Schedule(default(JobHandle)).Complete();
			foreach (Entity entity in nativeParallelHashMap.GetKeyArray(Allocator.Temp))
			{
				if (state.EntityManager.GetBuffer<Child>(entity, false).Length == 0)
				{
					ComponentTypeSet componentTypeSet2 = new ComponentTypeSet(ComponentType.ReadWrite<Child>());
					state.EntityManager.RemoveComponent(entity, in componentTypeSet2);
				}
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000026F0 File Offset: 0x000008F0
		private void UpdateDeletedParents(ref SystemState state)
		{
			if (this.m_DeletedParentsQuery.IsEmptyIgnoreFilter)
			{
				return;
			}
			NativeArray<Entity> nativeArray = this.m_DeletedParentsQuery.ToEntityArray(state.WorldUnmanaged.UpdateAllocator.ToAllocator);
			NativeList<Entity> nativeList = new NativeList<Entity>(state.WorldUnmanaged.UpdateAllocator.ToAllocator);
			this._childLookupRo.Update(ref state);
			this.ParentFromEntityRO.Update(ref state);
			ParentSystem.GatherChildEntities gatherChildEntities = default(ParentSystem.GatherChildEntities);
			gatherChildEntities.Parents = nativeArray;
			gatherChildEntities.Children = nativeList;
			gatherChildEntities.ChildLookup = this._childLookupRo;
			gatherChildEntities.ParentFromEntity = this.ParentFromEntityRO;
			gatherChildEntities.Schedule(default(JobHandle)).Complete();
			EntityManager entityManager = state.EntityManager;
			NativeArray<Entity> nativeArray2 = nativeList.AsArray();
			ComponentTypeSet componentTypeSet = new ComponentTypeSet(ComponentType.FromTypeIndex(TypeManager.GetTypeIndex<Parent>()), ComponentType.FromTypeIndex(TypeManager.GetTypeIndex<PreviousParent>()));
			entityManager.RemoveComponent(nativeArray2, in componentTypeSet);
			state.EntityManager.RemoveComponent(this.m_DeletedParentsQuery, ComponentType.FromTypeIndex(TypeManager.GetTypeIndex<Child>()));
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002804 File Offset: 0x00000A04
		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			state.Dependency.Complete();
			this.UpdateDeletedParents(ref state);
			this.UpdateRemoveParents(ref state);
			this.UpdateNewParents(ref state);
			this.UpdateChangeParents(ref state);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000283B File Offset: 0x00000A3B
		public void OnCreateForCompiler(ref SystemState state)
		{
		}

		// Token: 0x0600000E RID: 14 RVA: 0x0000287B File Offset: 0x00000A7B
		[BurstCompile]
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal static void __codegen__OnCreate(IntPtr self, IntPtr state)
		{
			ParentSystem.__codegen__OnCreate_0000000E$BurstDirectCall.Invoke(self, state);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002884 File Offset: 0x00000A84
		[BurstCompile]
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal static void __codegen__OnUpdate(IntPtr self, IntPtr state)
		{
			ParentSystem.__codegen__OnUpdate_0000000F$BurstDirectCall.Invoke(self, state);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x0000288D File Offset: 0x00000A8D
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
		{
			((ParentSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000028A6 File Offset: 0x00000AA6
		[BurstCompile]
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void __codegen__OnCreate$BurstManaged(IntPtr self, IntPtr state)
		{
			((ParentSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000028BF File Offset: 0x00000ABF
		[BurstCompile]
		[MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void __codegen__OnUpdate$BurstManaged(IntPtr self, IntPtr state)
		{
			((ParentSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
		}

		// Token: 0x04000006 RID: 6
		private EntityQuery m_NewParentsQuery;

		// Token: 0x04000007 RID: 7
		private EntityQuery m_RemovedParentsQuery;

		// Token: 0x04000008 RID: 8
		private EntityQuery m_ExistingParentsQuery;

		// Token: 0x04000009 RID: 9
		private EntityQuery m_DeletedParentsQuery;

		// Token: 0x0400000A RID: 10
		private static readonly ProfilerMarker k_ProfileDeletedParents = new ProfilerMarker("ParentSystem.DeletedParents");

		// Token: 0x0400000B RID: 11
		private static readonly ProfilerMarker k_ProfileRemoveParents = new ProfilerMarker("ParentSystem.RemoveParents");

		// Token: 0x0400000C RID: 12
		private static readonly ProfilerMarker k_ProfileChangeParents = new ProfilerMarker("ParentSystem.ChangeParents");

		// Token: 0x0400000D RID: 13
		private static readonly ProfilerMarker k_ProfileNewParents = new ProfilerMarker("ParentSystem.NewParents");

		// Token: 0x0400000E RID: 14
		private BufferLookup<Child> _childLookupRo;

		// Token: 0x0400000F RID: 15
		private BufferLookup<Child> _childLookupRw;

		// Token: 0x04000010 RID: 16
		private ComponentLookup<Parent> ParentFromEntityRO;

		// Token: 0x04000011 RID: 17
		private ComponentTypeHandle<PreviousParent> PreviousParentTypeHandleRW;

		// Token: 0x04000012 RID: 18
		private EntityTypeHandle EntityTypeHandle;

		// Token: 0x04000013 RID: 19
		private ComponentTypeHandle<Parent> ParentTypeHandleRO;

		// Token: 0x02000006 RID: 6
		[BurstCompile]
		private struct GatherChangedParents : IJobChunk
		{
			// Token: 0x06000013 RID: 19 RVA: 0x000028D8 File Offset: 0x00000AD8
			public void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
			{
				if (chunk.DidChange<Parent>(ref this.ParentTypeHandle, this.LastSystemVersion) || chunk.DidChange<PreviousParent>(ref this.PreviousParentTypeHandle, this.LastSystemVersion))
				{
					NativeArray<PreviousParent> nativeArray = chunk.GetNativeArray<PreviousParent>(ref this.PreviousParentTypeHandle);
					NativeArray<Parent> nativeArray2 = chunk.GetNativeArray<Parent>(ref this.ParentTypeHandle);
					NativeArray<Entity> nativeArray3 = chunk.GetNativeArray(this.EntityTypeHandle);
					int i = 0;
					int count = chunk.Count;
					while (i < count)
					{
						if (nativeArray2[i].Value != nativeArray[i].Value)
						{
							Entity entity = nativeArray3[i];
							Entity value = nativeArray2[i].Value;
							Entity value2 = nativeArray[i].Value;
							if (!this.EntityStorageInfoLookup.Exists(value))
							{
								this.ChildParentToRemove.Add(entity);
							}
							else
							{
								this.ParentChildrenToAdd.Add(value, entity);
								this.UniqueParents.TryAdd(value, 0);
								if (this.ChildLookup.HasBuffer(value2))
								{
									this.ParentChildrenToRemove.Add(value2, entity);
									this.UniqueParents.TryAdd(value2, 0);
								}
								nativeArray[i] = new PreviousParent
								{
									Value = value
								};
							}
						}
						i++;
					}
				}
			}

			// Token: 0x06000014 RID: 20 RVA: 0x00002A23 File Offset: 0x00000C23
			void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
			{
				this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
			}

			// Token: 0x04000014 RID: 20
			public NativeParallelMultiHashMap<Entity, Entity>.ParallelWriter ParentChildrenToAdd;

			// Token: 0x04000015 RID: 21
			public NativeParallelMultiHashMap<Entity, Entity>.ParallelWriter ParentChildrenToRemove;

			// Token: 0x04000016 RID: 22
			public NativeParallelHashSet<Entity>.ParallelWriter ChildParentToRemove;

			// Token: 0x04000017 RID: 23
			public NativeParallelHashMap<Entity, int>.ParallelWriter UniqueParents;

			// Token: 0x04000018 RID: 24
			public ComponentTypeHandle<PreviousParent> PreviousParentTypeHandle;

			// Token: 0x04000019 RID: 25
			public EntityStorageInfoLookup EntityStorageInfoLookup;

			// Token: 0x0400001A RID: 26
			[ReadOnly]
			public BufferLookup<Child> ChildLookup;

			// Token: 0x0400001B RID: 27
			[ReadOnly]
			public ComponentTypeHandle<Parent> ParentTypeHandle;

			// Token: 0x0400001C RID: 28
			[ReadOnly]
			public EntityTypeHandle EntityTypeHandle;

			// Token: 0x0400001D RID: 29
			public uint LastSystemVersion;
		}

		// Token: 0x02000007 RID: 7
		[BurstCompile]
		private struct FindMissingChild : IJob
		{
			// Token: 0x06000015 RID: 21 RVA: 0x00002A30 File Offset: 0x00000C30
			public void Execute()
			{
				NativeArray<Entity> keyArray = this.UniqueParents.GetKeyArray(Allocator.Temp);
				for (int i = 0; i < keyArray.Length; i++)
				{
					Entity entity = keyArray[i];
					if (!this.ChildLookup.HasBuffer(entity))
					{
						this.ParentsMissingChild.Add(in entity);
					}
				}
			}

			// Token: 0x0400001E RID: 30
			[ReadOnly]
			public NativeParallelHashMap<Entity, int> UniqueParents;

			// Token: 0x0400001F RID: 31
			[ReadOnly]
			public BufferLookup<Child> ChildLookup;

			// Token: 0x04000020 RID: 32
			public NativeList<Entity> ParentsMissingChild;
		}

		// Token: 0x02000008 RID: 8
		[BurstCompile]
		private struct FixupChangedChildren : IJob
		{
			// Token: 0x06000016 RID: 22 RVA: 0x00002A85 File Offset: 0x00000C85
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			private static void ThrowChildEntityNotInParent()
			{
				throw new InvalidOperationException("Child entity not in parent");
			}

			// Token: 0x06000017 RID: 23 RVA: 0x00002A94 File Offset: 0x00000C94
			private int FindChildIndex(DynamicBuffer<Child> children, Entity entity)
			{
				for (int i = 0; i < children.Length; i++)
				{
					if (children[i].Value == entity)
					{
						return i;
					}
				}
				return -1;
			}

			// Token: 0x06000018 RID: 24 RVA: 0x00002ACC File Offset: 0x00000CCC
			private void RemoveChildrenFromParent(Entity parent, DynamicBuffer<Child> children)
			{
				Entity entity;
				NativeParallelMultiHashMapIterator<Entity> nativeParallelMultiHashMapIterator;
				if (this.ParentChildrenToRemove.TryGetFirstValue(parent, out entity, out nativeParallelMultiHashMapIterator))
				{
					do
					{
						int num = this.FindChildIndex(children, entity);
						children.RemoveAt(num);
					}
					while (this.ParentChildrenToRemove.TryGetNextValue(out entity, ref nativeParallelMultiHashMapIterator));
				}
			}

			// Token: 0x06000019 RID: 25 RVA: 0x00002B10 File Offset: 0x00000D10
			private void AddChildrenToParent(Entity parent, DynamicBuffer<Child> children)
			{
				Entity entity;
				NativeParallelMultiHashMapIterator<Entity> nativeParallelMultiHashMapIterator;
				if (this.ParentChildrenToAdd.TryGetFirstValue(parent, out entity, out nativeParallelMultiHashMapIterator))
				{
					do
					{
						children.Add(new Child
						{
							Value = entity
						});
					}
					while (this.ParentChildrenToAdd.TryGetNextValue(out entity, ref nativeParallelMultiHashMapIterator));
				}
			}

			// Token: 0x0600001A RID: 26 RVA: 0x00002B5C File Offset: 0x00000D5C
			public void Execute()
			{
				NativeArray<Entity> keyArray = this.UniqueParents.GetKeyArray(Allocator.Temp);
				for (int i = 0; i < keyArray.Length; i++)
				{
					Entity entity = keyArray[i];
					DynamicBuffer<Child> dynamicBuffer;
					if (this.ChildLookup.TryGetBuffer(entity, out dynamicBuffer))
					{
						this.RemoveChildrenFromParent(entity, dynamicBuffer);
						this.AddChildrenToParent(entity, dynamicBuffer);
					}
				}
			}

			// Token: 0x04000021 RID: 33
			[ReadOnly]
			public NativeParallelMultiHashMap<Entity, Entity> ParentChildrenToAdd;

			// Token: 0x04000022 RID: 34
			[ReadOnly]
			public NativeParallelMultiHashMap<Entity, Entity> ParentChildrenToRemove;

			// Token: 0x04000023 RID: 35
			[ReadOnly]
			public NativeParallelHashMap<Entity, int> UniqueParents;

			// Token: 0x04000024 RID: 36
			public BufferLookup<Child> ChildLookup;
		}

		// Token: 0x02000009 RID: 9
		[BurstCompile]
		private struct GatherChildEntities : IJob
		{
			// Token: 0x0600001B RID: 27 RVA: 0x00002BB8 File Offset: 0x00000DB8
			public void Execute()
			{
				for (int i = 0; i < this.Parents.Length; i++)
				{
					Entity entity = this.Parents[i];
					NativeArray<Child> nativeArray = this.ChildLookup[entity].AsNativeArray();
					for (int j = 0; j < nativeArray.Length; j++)
					{
						Entity value = nativeArray[j].Value;
						Parent parent;
						if (this.ParentFromEntity.TryGetComponent(value, out parent) && parent.Value == entity)
						{
							this.Children.Add(in nativeArray[j].Value);
						}
					}
				}
			}

			// Token: 0x04000025 RID: 37
			[ReadOnly]
			public NativeArray<Entity> Parents;

			// Token: 0x04000026 RID: 38
			public NativeList<Entity> Children;

			// Token: 0x04000027 RID: 39
			[ReadOnly]
			public BufferLookup<Child> ChildLookup;

			// Token: 0x04000028 RID: 40
			[ReadOnly]
			public ComponentLookup<Parent> ParentFromEntity;
		}

		// Token: 0x0200000A RID: 10
		// (Invoke) Token: 0x0600001D RID: 29
		internal delegate void __codegen__OnCreate_0000000E$PostfixBurstDelegate(IntPtr self, IntPtr state);

		// Token: 0x0200000B RID: 11
		internal static class __codegen__OnCreate_0000000E$BurstDirectCall
		{
			// Token: 0x06000020 RID: 32 RVA: 0x00002C66 File Offset: 0x00000E66
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (ParentSystem.__codegen__OnCreate_0000000E$BurstDirectCall.Pointer == 0)
				{
					ParentSystem.__codegen__OnCreate_0000000E$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(ParentSystem.__codegen__OnCreate_0000000E$BurstDirectCall.DeferredCompilation, methodof(ParentSystem.__codegen__OnCreate$BurstManaged(IntPtr, IntPtr)).MethodHandle, typeof(ParentSystem.__codegen__OnCreate_0000000E$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = ParentSystem.__codegen__OnCreate_0000000E$BurstDirectCall.Pointer;
			}

			// Token: 0x06000021 RID: 33 RVA: 0x00002C94 File Offset: 0x00000E94
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				ParentSystem.__codegen__OnCreate_0000000E$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x06000022 RID: 34 RVA: 0x00002CAC File Offset: 0x00000EAC
			public static void Constructor()
			{
				ParentSystem.__codegen__OnCreate_0000000E$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(ParentSystem.__codegen__OnCreate(IntPtr, IntPtr)).MethodHandle);
			}

			// Token: 0x06000023 RID: 35 RVA: 0x0000283B File Offset: 0x00000A3B
			public static void Initialize()
			{
			}

			// Token: 0x06000024 RID: 36 RVA: 0x00002CBD File Offset: 0x00000EBD
			// Note: this type is marked as 'beforefieldinit'.
			static __codegen__OnCreate_0000000E$BurstDirectCall()
			{
				ParentSystem.__codegen__OnCreate_0000000E$BurstDirectCall.Constructor();
			}

			// Token: 0x06000025 RID: 37 RVA: 0x00002CC4 File Offset: 0x00000EC4
			public static void Invoke(IntPtr self, IntPtr state)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = ParentSystem.__codegen__OnCreate_0000000E$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(System.IntPtr,System.IntPtr), self, state, functionPointer);
						return;
					}
				}
				ParentSystem.__codegen__OnCreate$BurstManaged(self, state);
			}

			// Token: 0x04000029 RID: 41
			private static IntPtr Pointer;

			// Token: 0x0400002A RID: 42
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x0200000C RID: 12
		// (Invoke) Token: 0x06000027 RID: 39
		internal delegate void __codegen__OnUpdate_0000000F$PostfixBurstDelegate(IntPtr self, IntPtr state);

		// Token: 0x0200000D RID: 13
		internal static class __codegen__OnUpdate_0000000F$BurstDirectCall
		{
			// Token: 0x0600002A RID: 42 RVA: 0x00002CF7 File Offset: 0x00000EF7
			[BurstDiscard]
			private static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (ParentSystem.__codegen__OnUpdate_0000000F$BurstDirectCall.Pointer == 0)
				{
					ParentSystem.__codegen__OnUpdate_0000000F$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(ParentSystem.__codegen__OnUpdate_0000000F$BurstDirectCall.DeferredCompilation, methodof(ParentSystem.__codegen__OnUpdate$BurstManaged(IntPtr, IntPtr)).MethodHandle, typeof(ParentSystem.__codegen__OnUpdate_0000000F$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = ParentSystem.__codegen__OnUpdate_0000000F$BurstDirectCall.Pointer;
			}

			// Token: 0x0600002B RID: 43 RVA: 0x00002D24 File Offset: 0x00000F24
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				ParentSystem.__codegen__OnUpdate_0000000F$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600002C RID: 44 RVA: 0x00002D3C File Offset: 0x00000F3C
			public static void Constructor()
			{
				ParentSystem.__codegen__OnUpdate_0000000F$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(ParentSystem.__codegen__OnUpdate(IntPtr, IntPtr)).MethodHandle);
			}

			// Token: 0x0600002D RID: 45 RVA: 0x0000283B File Offset: 0x00000A3B
			public static void Initialize()
			{
			}

			// Token: 0x0600002E RID: 46 RVA: 0x00002D4D File Offset: 0x00000F4D
			// Note: this type is marked as 'beforefieldinit'.
			static __codegen__OnUpdate_0000000F$BurstDirectCall()
			{
				ParentSystem.__codegen__OnUpdate_0000000F$BurstDirectCall.Constructor();
			}

			// Token: 0x0600002F RID: 47 RVA: 0x00002D54 File Offset: 0x00000F54
			public static void Invoke(IntPtr self, IntPtr state)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = ParentSystem.__codegen__OnUpdate_0000000F$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(System.IntPtr,System.IntPtr), self, state, functionPointer);
						return;
					}
				}
				ParentSystem.__codegen__OnUpdate$BurstManaged(self, state);
			}

			// Token: 0x0400002B RID: 43
			private static IntPtr Pointer;

			// Token: 0x0400002C RID: 44
			private static IntPtr DeferredCompilation;
		}
	}
}
