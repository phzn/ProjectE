using System;
using System.Runtime.CompilerServices;
using Unity.Burst.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x020001D0 RID: 464
	[NativeContainer]
	public struct BufferLookup<[global::System.Runtime.CompilerServices.IsUnmanaged] T> where T : struct, ValueType, IBufferElementData
	{
		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000AEF RID: 2799 RVA: 0x0003420A File Offset: 0x0003240A
		internal uint GlobalSystemVersion
		{
			get
			{
				return this.m_GlobalSystemVersion;
			}
		}

		// Token: 0x06000AF0 RID: 2800 RVA: 0x00034214 File Offset: 0x00032414
		internal unsafe BufferLookup(TypeIndex typeIndex, EntityDataAccess* access, bool isReadOnly)
		{
			this.m_TypeIndex = typeIndex;
			this.m_Access = access;
			this.m_IsReadOnly = (isReadOnly ? 1 : 0);
			this.m_Cache = default(LookupCache);
			this.m_GlobalSystemVersion = access->EntityComponentStore->GlobalSystemVersion;
			this.m_InternalCapacity = TypeManager.GetTypeInfo<T>().BufferCapacity;
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x0003426C File Offset: 0x0003246C
		public bool TryGetBuffer(Entity entity, out DynamicBuffer<T> bufferData)
		{
			bool flag;
			return this.TryGetBuffer(entity, out bufferData, out flag);
		}

		// Token: 0x06000AF2 RID: 2802 RVA: 0x00034284 File Offset: 0x00032484
		public unsafe bool TryGetBuffer(Entity entity, out DynamicBuffer<T> bufferData, out bool entityExists)
		{
			EntityComponentStore* entityComponentStore = this.m_Access->EntityComponentStore;
			entityExists = entityComponentStore->Exists(entity);
			if (Hint.Unlikely(!entityExists))
			{
				bufferData = default(DynamicBuffer<T>);
				return false;
			}
			BufferHeader* ptr = (BufferHeader*)((this.m_IsReadOnly != 0) ? entityComponentStore->GetOptionalComponentDataWithTypeRO(entity, this.m_TypeIndex, ref this.m_Cache) : entityComponentStore->GetOptionalComponentDataWithTypeRW(entity, this.m_TypeIndex, this.m_GlobalSystemVersion, ref this.m_Cache));
			if (ptr != null)
			{
				bufferData = new DynamicBuffer<T>(ptr, this.m_InternalCapacity);
				return true;
			}
			bufferData = default(DynamicBuffer<T>);
			return false;
		}

		// Token: 0x06000AF3 RID: 2803 RVA: 0x00034313 File Offset: 0x00032513
		public unsafe bool EntityExists(Entity entity)
		{
			return this.m_Access->EntityComponentStore->Exists(entity);
		}

		// Token: 0x06000AF4 RID: 2804 RVA: 0x00034328 File Offset: 0x00032528
		public bool HasBuffer(Entity entity)
		{
			bool flag;
			return this.HasBuffer(entity, out flag);
		}

		// Token: 0x06000AF5 RID: 2805 RVA: 0x0003433E File Offset: 0x0003253E
		public unsafe bool HasBuffer(Entity entity, out bool entityExists)
		{
			return this.m_Access->EntityComponentStore->HasComponent(entity, this.m_TypeIndex, ref this.m_Cache, out entityExists);
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x0003435E File Offset: 0x0003255E
		[Obsolete("This method has been renamed to HasBuffer(). (RemovedAfter Entities 1.0)", false)]
		public bool HasComponent(Entity entity)
		{
			return this.HasBuffer(entity);
		}

		// Token: 0x06000AF7 RID: 2807 RVA: 0x00034368 File Offset: 0x00032568
		public unsafe bool DidChange(Entity entity, uint version)
		{
			EntityComponentStore* entityComponentStore = this.m_Access->EntityComponentStore;
			ChunkIndex chunk = entityComponentStore->GetChunk(entity);
			Archetype* archetype = entityComponentStore->GetArchetype(chunk);
			if (Hint.Unlikely(archetype != this.m_Cache.Archetype))
			{
				this.m_Cache.Update(archetype, this.m_TypeIndex);
			}
			short indexInArchetype = this.m_Cache.IndexInArchetype;
			return indexInArchetype != -1 && ChangeVersionUtility.DidChange(archetype->Chunks.GetChangeVersion((int)indexInArchetype, chunk.ListIndex), version);
		}

		// Token: 0x170000DD RID: 221
		public unsafe DynamicBuffer<T> this[Entity entity]
		{
			get
			{
				EntityComponentStore* entityComponentStore = this.m_Access->EntityComponentStore;
				return new DynamicBuffer<T>((BufferHeader*)((this.m_IsReadOnly != 0) ? entityComponentStore->GetComponentDataWithTypeRO(entity, this.m_TypeIndex, ref this.m_Cache) : entityComponentStore->GetComponentDataWithTypeRW(entity, this.m_TypeIndex, this.m_GlobalSystemVersion, ref this.m_Cache)), this.m_InternalCapacity);
			}
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x0003443E File Offset: 0x0003263E
		public unsafe bool IsBufferEnabled(Entity entity)
		{
			return this.m_Access->IsComponentEnabled(entity, this.m_TypeIndex, ref this.m_Cache);
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x00034458 File Offset: 0x00032658
		[Obsolete("Use SetBufferEnabled (RemovedAfter: Entities pre-1.0) (UnityUpgradeable) -> IsBufferEnabled(*)")]
		public void IsComponentEnabled(Entity entity)
		{
			this.IsBufferEnabled(entity);
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x00034462 File Offset: 0x00032662
		[Obsolete("Use SetBufferEnabled (RemovedAfter: Entities pre-1.0) (UnityUpgradeable) -> SetBufferEnabled(*)")]
		public void SetComponentEnabled(Entity entity, bool value)
		{
			this.SetBufferEnabled(entity, value);
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x0003446C File Offset: 0x0003266C
		public unsafe void SetBufferEnabled(Entity entity, bool value)
		{
			this.m_Access->SetComponentEnabled(entity, this.m_TypeIndex, value, ref this.m_Cache);
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x00034487 File Offset: 0x00032687
		public unsafe void Update(SystemBase system)
		{
			this.Update(ref *system.m_StatePtr);
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x00034495 File Offset: 0x00032695
		public unsafe void Update(ref SystemState systemState)
		{
			this.m_GlobalSystemVersion = systemState.m_EntityComponentStore->GlobalSystemVersion;
		}

		// Token: 0x06000AFF RID: 2815 RVA: 0x000344A8 File Offset: 0x000326A8
		private unsafe SafeBitRef MakeSafeBitRef(ulong* ptr, int offsetInBits)
		{
			return new SafeBitRef(ptr, offsetInBits);
		}

		// Token: 0x06000B00 RID: 2816 RVA: 0x000344B4 File Offset: 0x000326B4
		public unsafe EnabledRefRW<T2> GetEnabledRefRW<[global::System.Runtime.CompilerServices.IsUnmanaged] T2>(Entity entity) where T2 : struct, ValueType, IEnableableComponent, IBufferElementData
		{
			int num;
			int* ptr;
			ulong* enabledRawRW = this.m_Access->EntityComponentStore->GetEnabledRawRW(entity, this.m_TypeIndex, ref this.m_Cache, this.m_GlobalSystemVersion, out num, out ptr);
			return new EnabledRefRW<T2>(this.MakeSafeBitRef(enabledRawRW, num), ptr);
		}

		// Token: 0x06000B01 RID: 2817 RVA: 0x000344F8 File Offset: 0x000326F8
		public unsafe EnabledRefRW<T2> GetEnabledRefRWOptional<[global::System.Runtime.CompilerServices.IsUnmanaged] T2>(Entity entity) where T2 : struct, ValueType, IBufferElementData, IEnableableComponent
		{
			if (!this.HasBuffer(entity))
			{
				return new EnabledRefRW<T2>(default(SafeBitRef), null);
			}
			int num;
			int* ptr;
			ulong* enabledRawRW = this.m_Access->EntityComponentStore->GetEnabledRawRW(entity, this.m_TypeIndex, ref this.m_Cache, this.m_GlobalSystemVersion, out num, out ptr);
			return new EnabledRefRW<T2>(this.MakeSafeBitRef(enabledRawRW, num), ptr);
		}

		// Token: 0x06000B02 RID: 2818 RVA: 0x00034555 File Offset: 0x00032755
		[Obsolete("This method has been renamed to GetEnabledRefRWOptional<T>. (RemovedAfter Entities 1.0)", false)]
		public EnabledRefRW<T2> GetComponentEnabledRefRWOptional<[global::System.Runtime.CompilerServices.IsUnmanaged] T2>(Entity entity) where T2 : struct, ValueType, IBufferElementData, IEnableableComponent
		{
			return this.GetEnabledRefRWOptional<T2>(entity);
		}

		// Token: 0x06000B03 RID: 2819 RVA: 0x00034560 File Offset: 0x00032760
		public unsafe EnabledRefRO<T2> GetEnabledRefRO<[global::System.Runtime.CompilerServices.IsUnmanaged] T2>(Entity entity) where T2 : struct, ValueType, IEnableableComponent, IBufferElementData
		{
			int num;
			int* ptr;
			ulong* enabledRawRO = this.m_Access->EntityComponentStore->GetEnabledRawRO(entity, this.m_TypeIndex, ref this.m_Cache, out num, out ptr);
			return new EnabledRefRO<T2>(this.MakeSafeBitRef(enabledRawRO, num));
		}

		// Token: 0x06000B04 RID: 2820 RVA: 0x0003459C File Offset: 0x0003279C
		public unsafe EnabledRefRO<T2> GetEnabledRefROOptional<[global::System.Runtime.CompilerServices.IsUnmanaged] T2>(Entity entity) where T2 : struct, ValueType, IBufferElementData, IEnableableComponent
		{
			if (!this.HasBuffer(entity))
			{
				return new EnabledRefRO<T2>(default(SafeBitRef));
			}
			int num;
			int* ptr;
			ulong* enabledRawRO = this.m_Access->EntityComponentStore->GetEnabledRawRO(entity, this.m_TypeIndex, ref this.m_Cache, out num, out ptr);
			return new EnabledRefRO<T2>(this.MakeSafeBitRef(enabledRawRO, num));
		}

		// Token: 0x06000B05 RID: 2821 RVA: 0x000345F0 File Offset: 0x000327F0
		[Obsolete("This method has been renamed to GetEnabledRefROOptional<T>. (RemovedAfter Entities 1.0)", false)]
		public EnabledRefRO<T2> GetComponentEnabledRefROOptional<[global::System.Runtime.CompilerServices.IsUnmanaged] T2>(Entity entity) where T2 : struct, ValueType, IBufferElementData, IEnableableComponent
		{
			return this.GetEnabledRefROOptional<T2>(entity);
		}

		// Token: 0x040004EC RID: 1260
		[NativeDisableUnsafePtrRestriction]
		private unsafe readonly EntityDataAccess* m_Access;

		// Token: 0x040004ED RID: 1261
		private LookupCache m_Cache;

		// Token: 0x040004EE RID: 1262
		private readonly TypeIndex m_TypeIndex;

		// Token: 0x040004EF RID: 1263
		private uint m_GlobalSystemVersion;

		// Token: 0x040004F0 RID: 1264
		private int m_InternalCapacity;

		// Token: 0x040004F1 RID: 1265
		private readonly byte m_IsReadOnly;
	}
}
