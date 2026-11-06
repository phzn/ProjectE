using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities.Internal;
using Unity.Jobs;

namespace Unity.Entities
{
	// Token: 0x0200021A RID: 538
	[GenerateTestsForBurstCompatibility]
	[DebuggerTypeProxy(typeof(EntityQueryDebugView))]
	public struct EntityQuery : IDisposable, IEquatable<EntityQuery>
	{
		// Token: 0x06000CD5 RID: 3285 RVA: 0x0003B54C File Offset: 0x0003974C
		internal unsafe bool HasComponentsRequiredForExecuteMethodToRun(ref Span<ComponentType> componentsUsedInExecuteMethod)
		{
			NativeHashMap<int, ComponentType.AccessMode> nativeHashMap = new NativeHashMap<int, ComponentType.AccessMode>(8, Allocator.Temp);
			EntityQueryData* queryData = this.__impl->_QueryData;
			for (int i = 0; i < queryData->RequiredComponentsCount; i++)
			{
				ComponentType componentType = queryData->RequiredComponents[i];
				nativeHashMap.Add(componentType.TypeIndex, componentType.AccessModeType);
			}
			Span<ComponentType> span = componentsUsedInExecuteMethod;
			for (int j = 0; j < span.Length; j++)
			{
				ComponentType componentType2 = *span[j];
				ComponentType.AccessMode accessMode;
				if (!nativeHashMap.TryGetValue(componentType2.TypeIndex, out accessMode))
				{
					return false;
				}
				if (componentType2.AccessModeType == ComponentType.AccessMode.ReadOnly && accessMode == ComponentType.AccessMode.Exclude)
				{
					return false;
				}
			}
			nativeHashMap.Dispose();
			return true;
		}

		// Token: 0x06000CD6 RID: 3286 RVA: 0x0003B612 File Offset: 0x00039812
		public bool Equals(EntityQuery other)
		{
			return this.__impl == other.__impl;
		}

		// Token: 0x06000CD7 RID: 3287 RVA: 0x0003B624 File Offset: 0x00039824
		[ExcludeFromBurstCompatTesting("Takes managed object")]
		public override bool Equals(object obj)
		{
			if (obj is EntityQuery)
			{
				EntityQuery entityQuery = (EntityQuery)obj;
				return this.Equals(entityQuery);
			}
			return false;
		}

		// Token: 0x06000CD8 RID: 3288 RVA: 0x0003B649 File Offset: 0x00039849
		public override int GetHashCode()
		{
			return this.__impl;
		}

		// Token: 0x06000CD9 RID: 3289 RVA: 0x0003B654 File Offset: 0x00039854
		internal unsafe static EntityQuery Construct(EntityQueryData* queryData, EntityDataAccess* access)
		{
			EntityQuery entityQuery = default(EntityQuery);
			EntityQueryImpl* ptr = EntityQueryImpl.Allocate();
			ref ulong data = ref WorldUnmanaged.NextSequenceNumber.Data;
			ulong num = data;
			data = num + 1UL;
			entityQuery.__seqno = num;
			ptr->Construct(queryData, access, entityQuery.__seqno);
			entityQuery.__impl = ptr;
			return entityQuery;
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000CDA RID: 3290 RVA: 0x0003B6A0 File Offset: 0x000398A0
		public unsafe bool IsEmpty
		{
			get
			{
				return this._GetImpl()->IsEmpty;
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000CDB RID: 3291 RVA: 0x0003B6AD File Offset: 0x000398AD
		public unsafe bool IsEmptyIgnoreFilter
		{
			get
			{
				return this._GetImpl()->IsEmptyIgnoreFilter;
			}
		}

		// Token: 0x06000CDC RID: 3292 RVA: 0x0003B6BA File Offset: 0x000398BA
		[ExcludeFromBurstCompatTesting("Returns managed array")]
		internal unsafe ComponentType[] GetQueryTypes()
		{
			return this._GetImpl()->GetQueryTypes();
		}

		// Token: 0x06000CDD RID: 3293 RVA: 0x0003B6C7 File Offset: 0x000398C7
		[ExcludeFromBurstCompatTesting("Returns managed array")]
		internal unsafe ComponentType[] GetReadAndWriteTypes()
		{
			return this._GetImpl()->GetReadAndWriteTypes();
		}

		// Token: 0x06000CDE RID: 3294 RVA: 0x0003B6D4 File Offset: 0x000398D4
		public unsafe void Dispose()
		{
			EntityQueryImpl* ptr = this._GetImpl();
			ptr->Dispose();
			EntityQueryImpl.Free(ptr);
			this.__impl = null;
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000CDF RID: 3295 RVA: 0x0003B6F0 File Offset: 0x000398F0
		// (set) Token: 0x06000CE0 RID: 3296 RVA: 0x0003B724 File Offset: 0x00039924
		[ExcludeFromBurstCompatTesting("Returning interface value boxes")]
		internal unsafe IDisposable _CachedState
		{
			[ExcludeFromBurstCompatTesting("Returning interface value boxes")]
			get
			{
				EntityQueryImpl* ptr = this._GetImpl();
				if (!ptr->_CachedState.IsAllocated)
				{
					return null;
				}
				return (IDisposable)ptr->_CachedState.Target;
			}
			[ExcludeFromBurstCompatTesting("Taking interface value boxes")]
			set
			{
				EntityQueryImpl* ptr = this._GetImpl();
				if (!ptr->_CachedState.IsAllocated)
				{
					ptr->_CachedState = GCHandle.Alloc(value);
					return;
				}
				ptr->_CachedState.Target = value;
			}
		}

		// Token: 0x06000CE1 RID: 3297 RVA: 0x0003B75E File Offset: 0x0003995E
		public unsafe int CalculateEntityCount()
		{
			return this._GetImpl()->CalculateEntityCount();
		}

		// Token: 0x06000CE2 RID: 3298 RVA: 0x0003B76B File Offset: 0x0003996B
		public unsafe int CalculateEntityCountWithoutFiltering()
		{
			return this._GetImpl()->CalculateEntityCountWithoutFiltering();
		}

		// Token: 0x06000CE3 RID: 3299 RVA: 0x0003B778 File Offset: 0x00039978
		public unsafe int CalculateChunkCount()
		{
			return this._GetImpl()->CalculateChunkCount();
		}

		// Token: 0x06000CE4 RID: 3300 RVA: 0x0003B785 File Offset: 0x00039985
		public unsafe int CalculateChunkCountWithoutFiltering()
		{
			return this._GetImpl()->CalculateChunkCountWithoutFiltering();
		}

		// Token: 0x06000CE5 RID: 3301 RVA: 0x0003B792 File Offset: 0x00039992
		public unsafe NativeArray<int> CalculateFilteredChunkIndexArray(AllocatorManager.AllocatorHandle allocator)
		{
			return this._GetImpl()->CalculateFilteredChunkIndexArray(allocator);
		}

		// Token: 0x06000CE6 RID: 3302 RVA: 0x0003B7A0 File Offset: 0x000399A0
		public unsafe NativeArray<int> CalculateFilteredChunkIndexArrayAsync(AllocatorManager.AllocatorHandle allocator, JobHandle additionalInputDep, out JobHandle outJobHandle)
		{
			return this._GetImpl()->CalculateFilteredChunkIndexArrayAsync(allocator, additionalInputDep, out outJobHandle);
		}

		// Token: 0x06000CE7 RID: 3303 RVA: 0x0003B7B0 File Offset: 0x000399B0
		public unsafe NativeArray<int> CalculateBaseEntityIndexArray(AllocatorManager.AllocatorHandle allocator)
		{
			return this._GetImpl()->CalculateBaseEntityIndexArray(allocator);
		}

		// Token: 0x06000CE8 RID: 3304 RVA: 0x0003B7BE File Offset: 0x000399BE
		public unsafe NativeArray<int> CalculateBaseEntityIndexArrayAsync(AllocatorManager.AllocatorHandle allocator, JobHandle additionalInputDep, out JobHandle outJobHandle)
		{
			return this._GetImpl()->CalculateBaseEntityIndexArrayAsync(allocator, additionalInputDep, out outJobHandle);
		}

		// Token: 0x06000CE9 RID: 3305 RVA: 0x0003B7CE File Offset: 0x000399CE
		internal unsafe int GetIndexInEntityQuery(TypeIndex componentType)
		{
			return this._GetImpl()->GetIndexInEntityQuery(componentType);
		}

		// Token: 0x06000CEA RID: 3306 RVA: 0x0003B7DC File Offset: 0x000399DC
		[Obsolete("This method is not actually asynchronous. Use ToArchetypeChunkListAsync() instead. (RemovedAfter Entities 1.0)")]
		public unsafe NativeArray<ArchetypeChunk> CreateArchetypeChunkArrayAsync(AllocatorManager.AllocatorHandle allocator, out JobHandle outJobHandle)
		{
			return this._GetImpl()->CreateArchetypeChunkArrayAsync(allocator, out outJobHandle);
		}

		// Token: 0x06000CEB RID: 3307 RVA: 0x0003B7EC File Offset: 0x000399EC
		public unsafe NativeList<ArchetypeChunk> ToArchetypeChunkListAsync(AllocatorManager.AllocatorHandle allocator, out JobHandle outJobHandle)
		{
			return this._GetImpl()->ToArchetypeChunkListAsync(allocator, default(JobHandle), out outJobHandle);
		}

		// Token: 0x06000CEC RID: 3308 RVA: 0x0003B80F File Offset: 0x00039A0F
		public unsafe NativeList<ArchetypeChunk> ToArchetypeChunkListAsync(AllocatorManager.AllocatorHandle allocator, JobHandle additionalInputDep, out JobHandle outJobHandle)
		{
			return this._GetImpl()->ToArchetypeChunkListAsync(allocator, additionalInputDep, out outJobHandle);
		}

		// Token: 0x06000CED RID: 3309 RVA: 0x0003B81F File Offset: 0x00039A1F
		public unsafe NativeArray<ArchetypeChunk> ToArchetypeChunkArray(AllocatorManager.AllocatorHandle allocator)
		{
			return this._GetImpl()->ToArchetypeChunkArray(allocator);
		}

		// Token: 0x06000CEE RID: 3310 RVA: 0x0003B81F File Offset: 0x00039A1F
		[Obsolete("This method has been renamed to ToArchetypeChunkArray. (RemovedAfter Entities 1.0) (UnityUpgradable) -> ToArchetypeChunkArray(*)")]
		public unsafe NativeArray<ArchetypeChunk> CreateArchetypeChunkArray(AllocatorManager.AllocatorHandle allocator)
		{
			return this._GetImpl()->ToArchetypeChunkArray(allocator);
		}

		// Token: 0x06000CEF RID: 3311 RVA: 0x0003B82D File Offset: 0x00039A2D
		public unsafe void ToArchetypeChunkList(NativeList<ArchetypeChunk> archetypeChunkList)
		{
			this._GetImpl()->ToArchetypeChunkList(archetypeChunkList);
		}

		// Token: 0x06000CF0 RID: 3312 RVA: 0x0003B83B File Offset: 0x00039A3B
		[Obsolete("This method does not correctly support enableable components, and is generally unsafe. Use ToEntityListAsync() instead. (RemovedAfter Entities 1.0)")]
		public unsafe NativeArray<Entity> ToEntityArrayAsync(AllocatorManager.AllocatorHandle allocator, out JobHandle jobhandle)
		{
			return this._GetImpl()->ToEntityArrayAsync(allocator, out jobhandle, this);
		}

		// Token: 0x06000CF1 RID: 3313 RVA: 0x0003B850 File Offset: 0x00039A50
		public unsafe NativeList<Entity> ToEntityListAsync(AllocatorManager.AllocatorHandle allocator, out JobHandle outJobHandle)
		{
			return this._GetImpl()->ToEntityListAsync(allocator, this, default(JobHandle), out outJobHandle);
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x0003B879 File Offset: 0x00039A79
		public unsafe NativeList<Entity> ToEntityListAsync(AllocatorManager.AllocatorHandle allocator, JobHandle additionalInputDep, out JobHandle outJobHandle)
		{
			return this._GetImpl()->ToEntityListAsync(allocator, this, additionalInputDep, out outJobHandle);
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x0003B88F File Offset: 0x00039A8F
		public unsafe NativeArray<Entity> ToEntityArray(AllocatorManager.AllocatorHandle allocator)
		{
			return this._GetImpl()->ToEntityArray(allocator, this);
		}

		// Token: 0x06000CF4 RID: 3316 RVA: 0x0003B8A3 File Offset: 0x00039AA3
		internal unsafe void GatherEntitiesToArray(out InternalGatherEntitiesResult result)
		{
			this._GetImpl()->GatherEntitiesToArray(out result, this);
		}

		// Token: 0x06000CF5 RID: 3317 RVA: 0x0003B8B7 File Offset: 0x00039AB7
		internal unsafe void ReleaseGatheredEntities(ref InternalGatherEntitiesResult result)
		{
			this._GetImpl()->ReleaseGatheredEntities(ref result);
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x0003B8C5 File Offset: 0x00039AC5
		[Obsolete("This method does not correctly support enableable components, and is generally unsafe. Use ToComponentDataListAsync() instead. (RemovedAfter Entities 1.0)")]
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe NativeArray<T> ToComponentDataArrayAsync<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(AllocatorManager.AllocatorHandle allocator, out JobHandle jobhandle) where T : struct, ValueType, IComponentData
		{
			return this._GetImpl()->ToComponentDataArrayAsync<T>(allocator, out jobhandle, this);
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x0003B8DC File Offset: 0x00039ADC
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe NativeList<T> ToComponentDataListAsync<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(AllocatorManager.AllocatorHandle allocator, out JobHandle outJobHandle) where T : struct, ValueType, IComponentData
		{
			return this._GetImpl()->ToComponentDataListAsync<T>(allocator, this, default(JobHandle), out outJobHandle);
		}

		// Token: 0x06000CF8 RID: 3320 RVA: 0x0003B905 File Offset: 0x00039B05
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe NativeList<T> ToComponentDataListAsync<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(AllocatorManager.AllocatorHandle allocator, JobHandle additionalInputDep, out JobHandle outJobHandle) where T : struct, ValueType, IComponentData
		{
			return this._GetImpl()->ToComponentDataListAsync<T>(allocator, this, additionalInputDep, out outJobHandle);
		}

		// Token: 0x06000CF9 RID: 3321 RVA: 0x0003B91B File Offset: 0x00039B1B
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe NativeArray<T> ToComponentDataArray<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(AllocatorManager.AllocatorHandle allocator) where T : struct, ValueType, IComponentData
		{
			return this._GetImpl()->ToComponentDataArray<T>(allocator, this);
		}

		// Token: 0x06000CFA RID: 3322 RVA: 0x0003B92F File Offset: 0x00039B2F
		[ExcludeFromBurstCompatTesting("Returns managed array")]
		public unsafe T[] ToComponentDataArray<T>() where T : class, IComponentData, new()
		{
			return this._GetImpl()->ToComponentDataArray<T>();
		}

		// Token: 0x06000CFB RID: 3323 RVA: 0x0003B93C File Offset: 0x00039B3C
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe void CopyFromComponentDataArray<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(NativeArray<T> componentDataArray) where T : struct, ValueType, IComponentData
		{
			this._GetImpl()->CopyFromComponentDataArray<T>(componentDataArray, this);
		}

		// Token: 0x06000CFC RID: 3324 RVA: 0x0003B950 File Offset: 0x00039B50
		[Obsolete("This method does not correctly support enableable components, and is generally unsafe. Use CopyFromComponentDataListAsync() instead. (RemovedAfter Entities 1.0)")]
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe void CopyFromComponentDataArrayAsync<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(NativeArray<T> componentDataArray, out JobHandle jobHandle) where T : struct, ValueType, IComponentData
		{
			this._GetImpl()->CopyFromComponentDataArrayAsync<T>(componentDataArray, out jobHandle, this);
		}

		// Token: 0x06000CFD RID: 3325 RVA: 0x0003B968 File Offset: 0x00039B68
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe void CopyFromComponentDataListAsync<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(NativeList<T> componentDataList, out JobHandle outJobHandle) where T : struct, ValueType, IComponentData
		{
			this._GetImpl()->CopyFromComponentDataListAsync<T>(componentDataList, this, default(JobHandle), out outJobHandle);
		}

		// Token: 0x06000CFE RID: 3326 RVA: 0x0003B991 File Offset: 0x00039B91
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe void CopyFromComponentDataListAsync<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(NativeList<T> componentDataList, JobHandle additionalInputDep, out JobHandle outJobHandle) where T : struct, ValueType, IComponentData
		{
			this._GetImpl()->CopyFromComponentDataListAsync<T>(componentDataList, this, additionalInputDep, out outJobHandle);
		}

		// Token: 0x06000CFF RID: 3327 RVA: 0x0003B9A7 File Offset: 0x00039BA7
		public unsafe Entity GetSingletonEntity()
		{
			return this._GetImpl()->GetSingletonEntity();
		}

		// Token: 0x06000D00 RID: 3328 RVA: 0x0003B9B4 File Offset: 0x00039BB4
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe T GetSingleton<[global::System.Runtime.CompilerServices.IsUnmanaged] T>() where T : struct, ValueType, IComponentData
		{
			return this._GetImpl()->GetSingleton<T>();
		}

		// Token: 0x06000D01 RID: 3329 RVA: 0x0003B9C1 File Offset: 0x00039BC1
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe RefRW<T> GetSingletonRW<[global::System.Runtime.CompilerServices.IsUnmanaged] T>() where T : struct, ValueType, IComponentData
		{
			return this._GetImpl()->GetSingletonRW<T>();
		}

		// Token: 0x06000D02 RID: 3330 RVA: 0x0003B9CE File Offset: 0x00039BCE
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe bool TryGetSingleton<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(out T value) where T : struct, ValueType, IComponentData
		{
			return this._GetImpl()->TryGetSingleton<T>(out value);
		}

		// Token: 0x06000D03 RID: 3331 RVA: 0x0003B9DC File Offset: 0x00039BDC
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe bool TryGetSingletonRW<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(out RefRW<T> value) where T : struct, ValueType, IComponentData
		{
			return this._GetImpl()->TryGetSingletonRW<T>(out value);
		}

		// Token: 0x06000D04 RID: 3332 RVA: 0x0003B9EA File Offset: 0x00039BEA
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe bool HasSingleton<T>()
		{
			return this._GetImpl()->HasSingleton<T>();
		}

		// Token: 0x06000D05 RID: 3333 RVA: 0x0003B9F7 File Offset: 0x00039BF7
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleBufferElement) })]
		public unsafe bool TryGetSingletonBuffer<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(out DynamicBuffer<T> value, bool isReadOnly = false) where T : struct, ValueType, IBufferElementData
		{
			return this._GetImpl()->TryGetSingletonBuffer<T>(out value, isReadOnly);
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x0003BA06 File Offset: 0x00039C06
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe bool TryGetSingletonEntity<T>(out Entity value)
		{
			return this._GetImpl()->TryGetSingletonEntity<T>(out value);
		}

		// Token: 0x06000D07 RID: 3335 RVA: 0x0003BA14 File Offset: 0x00039C14
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleBufferElement) })]
		public unsafe DynamicBuffer<T> GetSingletonBuffer<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(bool isReadOnly = false) where T : struct, ValueType, IBufferElementData
		{
			return this._GetImpl()->GetSingletonBuffer<T>(isReadOnly);
		}

		// Token: 0x06000D08 RID: 3336 RVA: 0x0003BA22 File Offset: 0x00039C22
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe void SetSingleton<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(T value) where T : struct, ValueType, IComponentData
		{
			this._GetImpl()->SetSingleton<T>(value);
		}

		// Token: 0x06000D09 RID: 3337 RVA: 0x0003BA30 File Offset: 0x00039C30
		[Obsolete("This method has been deprecated. Use EntityManager.SetComponentEnabled<T>(Unity.Entities.EntityQuery, bool) instead. (RemovedAfter Entities 1.0)")]
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleEnableableComponent) })]
		public unsafe void SetEnabledBitsOnAllChunks<T>(bool value) where T : IEnableableComponent
		{
			this._GetImpl()->SetEnabledBitsOnAllChunks(TypeManager.GetTypeIndex<T>(), value);
		}

		// Token: 0x06000D0A RID: 3338 RVA: 0x0003BA44 File Offset: 0x00039C44
		[Obsolete("This method is not Burst-compatible. Use CompareQuery(in EntityQueryBuilder queryDesc) instead. (RemovedAfter Entities 1.0)")]
		public unsafe bool CompareComponents(ComponentType[] componentTypes)
		{
			ComponentType* ptr;
			if (componentTypes == null || componentTypes.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &componentTypes[0];
			}
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp, ptr, componentTypes.Length);
			entityQueryBuilder.FinalizeQueryInternal();
			return this._GetImpl()->CompareQuery(in entityQueryBuilder);
		}

		// Token: 0x06000D0B RID: 3339 RVA: 0x0003BA90 File Offset: 0x00039C90
		[Obsolete("Use CompareQuery(in EntityQueryBuilder queryDesc) instead. (RemovedAfter Entities 1.0)")]
		public unsafe bool CompareComponents(NativeArray<ComponentType> componentTypes)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp, (ComponentType*)componentTypes.GetUnsafeReadOnlyPtr<ComponentType>(), componentTypes.Length);
			entityQueryBuilder.FinalizeQueryInternal();
			return this._GetImpl()->CompareQuery(in entityQueryBuilder);
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x0003BACC File Offset: 0x00039CCC
		public unsafe bool CompareQuery(in EntityQueryBuilder queryDesc)
		{
			return this._GetImpl()->CompareQuery(in queryDesc);
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x0003BADA File Offset: 0x00039CDA
		public unsafe void ResetFilter()
		{
			this._GetImpl()->ResetFilter();
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x0003BAE7 File Offset: 0x00039CE7
		[ExcludeFromBurstCompatTesting("Uses managed objects")]
		public unsafe void SetSharedComponentFilterManaged<SharedComponent>(SharedComponent sharedComponent) where SharedComponent : struct, ISharedComponentData
		{
			this._GetImpl()->SetSharedComponentFilter<SharedComponent>(sharedComponent);
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x0003BAF5 File Offset: 0x00039CF5
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleSharedComponentData) })]
		public unsafe void SetSharedComponentFilter<[global::System.Runtime.CompilerServices.IsUnmanaged] SharedComponent>(SharedComponent sharedComponent) where SharedComponent : struct, ValueType, ISharedComponentData
		{
			this._GetImpl()->SetSharedComponentFilterUnmanaged<SharedComponent>(sharedComponent);
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x0003BB03 File Offset: 0x00039D03
		[ExcludeFromBurstCompatTesting("Contains managed shared component code path")]
		public unsafe void SetSharedComponentFilterManaged<SharedComponent1, SharedComponent2>(SharedComponent1 sharedComponent1, SharedComponent2 sharedComponent2) where SharedComponent1 : struct, ISharedComponentData where SharedComponent2 : struct, ISharedComponentData
		{
			this._GetImpl()->SetSharedComponentFilter<SharedComponent1, SharedComponent2>(sharedComponent1, sharedComponent2);
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x0003BB12 File Offset: 0x00039D12
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[]
		{
			typeof(BurstCompatibleSharedComponentData),
			typeof(BurstCompatibleSharedComponentData)
		})]
		public unsafe void SetSharedComponentFilter<[global::System.Runtime.CompilerServices.IsUnmanaged] SharedComponent1, [global::System.Runtime.CompilerServices.IsUnmanaged] SharedComponent2>(SharedComponent1 sharedComponent1, SharedComponent2 sharedComponent2) where SharedComponent1 : struct, ValueType, ISharedComponentData where SharedComponent2 : struct, ValueType, ISharedComponentData
		{
			this._GetImpl()->SetSharedComponentFilterUnmanaged<SharedComponent1, SharedComponent2>(sharedComponent1, sharedComponent2);
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x0003BB21 File Offset: 0x00039D21
		public unsafe void SetChangedVersionFilter(ComponentType componentType)
		{
			this._GetImpl()->SetChangedVersionFilter(componentType);
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x0003BB2F File Offset: 0x00039D2F
		internal unsafe void SetChangedFilterRequiredVersion(uint requiredVersion)
		{
			this._GetImpl()->SetChangedFilterRequiredVersion(requiredVersion);
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x0003BB3D File Offset: 0x00039D3D
		[ExcludeFromBurstCompatTesting("Takes managed array")]
		public unsafe void SetChangedVersionFilter(ComponentType[] componentType)
		{
			this._GetImpl()->SetChangedVersionFilter(componentType);
		}

		// Token: 0x06000D15 RID: 3349 RVA: 0x0003BB4B File Offset: 0x00039D4B
		public unsafe void AddChangedVersionFilter(ComponentType componentType)
		{
			this._GetImpl()->AddChangedVersionFilter(componentType);
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x0003BB59 File Offset: 0x00039D59
		[ExcludeFromBurstCompatTesting("Contains managed shared component code path")]
		public unsafe void AddSharedComponentFilterManaged<SharedComponent>(SharedComponent sharedComponent) where SharedComponent : struct, ISharedComponentData
		{
			this._GetImpl()->AddSharedComponentFilter<SharedComponent>(sharedComponent);
		}

		// Token: 0x06000D17 RID: 3351 RVA: 0x0003BB67 File Offset: 0x00039D67
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleSharedComponentData) })]
		public unsafe void AddSharedComponentFilter<[global::System.Runtime.CompilerServices.IsUnmanaged] SharedComponent>(SharedComponent sharedComponent) where SharedComponent : struct, ValueType, ISharedComponentData
		{
			this._GetImpl()->AddSharedComponentFilterUnmanaged<SharedComponent>(sharedComponent);
		}

		// Token: 0x06000D18 RID: 3352 RVA: 0x0003BB75 File Offset: 0x00039D75
		public unsafe void SetOrderVersionFilter()
		{
			this._GetImpl()->SetOrderVersionFilter();
		}

		// Token: 0x06000D19 RID: 3353 RVA: 0x0003BB82 File Offset: 0x00039D82
		public unsafe void AddOrderVersionFilter()
		{
			this._GetImpl()->AddOrderVersionFilter();
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x0003BB8F File Offset: 0x00039D8F
		public unsafe void CompleteDependency()
		{
			this._GetImpl()->CompleteDependency();
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x0003BB9C File Offset: 0x00039D9C
		public unsafe JobHandle GetDependency()
		{
			return this._GetImpl()->GetDependency();
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x0003BBA9 File Offset: 0x00039DA9
		public unsafe JobHandle AddDependency(JobHandle job)
		{
			return this._GetImpl()->AddDependency(job);
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x0003BBB7 File Offset: 0x00039DB7
		[Obsolete("This function is deprecated, and will be removed in a future release. Use the new variant instead, with includeEntityType set to true.")]
		public unsafe int GetCombinedComponentOrderVersion()
		{
			return this._GetImpl()->GetCombinedComponentOrderVersion(true);
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x0003BBC5 File Offset: 0x00039DC5
		public unsafe int GetCombinedComponentOrderVersion(bool includeEntityType)
		{
			return this._GetImpl()->GetCombinedComponentOrderVersion(includeEntityType);
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x0003BBD3 File Offset: 0x00039DD3
		internal unsafe bool AddReaderWritersToLists(ref UnsafeList<TypeIndex> reading, ref UnsafeList<TypeIndex> writing)
		{
			return this._GetImpl()->AddReaderWritersToLists(ref reading, ref writing);
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x0003BBE2 File Offset: 0x00039DE2
		public unsafe bool HasFilter()
		{
			return this._GetImpl()->HasFilter();
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x0003BBEF File Offset: 0x00039DEF
		public unsafe EntityQueryMask GetEntityQueryMask()
		{
			return this._GetImpl()->GetEntityQueryMask();
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x0003BBFC File Offset: 0x00039DFC
		public unsafe bool Matches(Entity e)
		{
			return this._GetImpl()->Matches(e);
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x0003BC0A File Offset: 0x00039E0A
		public unsafe bool MatchesIgnoreFilter(Entity e)
		{
			return this._GetImpl()->MatchesIgnoreFilter(e);
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x0003BC0A File Offset: 0x00039E0A
		[Obsolete("This function has been renamed to MatchesIgnoreFilter(). (RemovedAfter Entities 1.0) (UnityUpgradable) -> MatchesIgnoreFilter(*)", true)]
		public unsafe bool MatchesNoFilter(Entity e)
		{
			return this._GetImpl()->MatchesIgnoreFilter(e);
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x0003BC18 File Offset: 0x00039E18
		[ExcludeFromBurstCompatTesting("Returns managed array of classes")]
		public unsafe EntityQueryDesc[] GetEntityQueryDescs()
		{
			return this._GetImpl()->GetEntityQueryDescs();
		}

		// Token: 0x06000D26 RID: 3366 RVA: 0x0003BC25 File Offset: 0x00039E25
		[ExcludeFromBurstCompatTesting("Returns class")]
		[Obsolete("A query can have multiple descriptions; this method only returns the first one. Use GetEntityQueryDescs() to return them all. (RemovedAfter Entities 2.0)", false)]
		public unsafe EntityQueryDesc GetEntityQueryDesc()
		{
			return this._GetImpl()->GetEntityQueryDesc();
		}

		// Token: 0x06000D27 RID: 3367 RVA: 0x0003BC32 File Offset: 0x00039E32
		internal unsafe void InvalidateCache()
		{
			this._GetImpl()->_QueryData->InvalidateChunkCache();
		}

		// Token: 0x06000D28 RID: 3368 RVA: 0x0003BC44 File Offset: 0x00039E44
		internal unsafe void ForceUpdateCache()
		{
			this._GetImpl()->UpdateMatchingChunkCache();
		}

		// Token: 0x06000D29 RID: 3369 RVA: 0x0003BC51 File Offset: 0x00039E51
		internal unsafe void CheckChunkListCacheConsistency(bool forceCheckInvalidCache = false)
		{
			this._GetImpl()->_QueryData->CheckChunkListCacheConsistency(forceCheckInvalidCache);
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000D2A RID: 3370 RVA: 0x0003BC64 File Offset: 0x00039E64
		internal unsafe bool IsCacheValid
		{
			get
			{
				return this._GetImpl()->_QueryData->IsChunkCacheValid();
			}
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x0003BC76 File Offset: 0x00039E76
		internal unsafe EntityQueryImpl* _Debugger_GetImpl()
		{
			return this.__impl;
		}

		// Token: 0x06000D2C RID: 3372 RVA: 0x0003BC76 File Offset: 0x00039E76
		internal unsafe EntityQueryImpl* _GetImpl()
		{
			return this.__impl;
		}

		// Token: 0x06000D2D RID: 3373 RVA: 0x0003BC7E File Offset: 0x00039E7E
		internal unsafe EntityComponentStore* GetEntityComponentStore()
		{
			return this.__impl->_Access->EntityComponentStore;
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x00002287 File Offset: 0x00000487
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		private void _CheckSafetyHandle()
		{
		}

		// Token: 0x06000D2F RID: 3375 RVA: 0x0003BC90 File Offset: 0x00039E90
		public static bool operator ==(EntityQuery lhs, EntityQuery rhs)
		{
			return lhs.__seqno == rhs.__seqno;
		}

		// Token: 0x06000D30 RID: 3376 RVA: 0x0003BCA0 File Offset: 0x00039EA0
		public static bool operator !=(EntityQuery lhs, EntityQuery rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x04000585 RID: 1413
		internal unsafe EntityQueryImpl* __impl;

		// Token: 0x04000586 RID: 1414
		internal ulong __seqno;
	}
}
