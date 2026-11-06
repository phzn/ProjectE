using System;
using System.Runtime.CompilerServices;
using Unity.Burst.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x02000208 RID: 520
	[NativeContainer]
	public struct ComponentLookup<[global::System.Runtime.CompilerServices.IsUnmanaged] T> where T : struct, ValueType, IComponentData
	{
		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000BFE RID: 3070 RVA: 0x00037BF1 File Offset: 0x00035DF1
		internal uint GlobalSystemVersion
		{
			get
			{
				return this.m_GlobalSystemVersion;
			}
		}

		// Token: 0x06000BFF RID: 3071 RVA: 0x00037BFC File Offset: 0x00035DFC
		internal unsafe ComponentLookup(int typeIndex, EntityDataAccess* access)
		{
			this.m_TypeIndex = typeIndex;
			this.m_Access = access;
			this.m_Cache = default(LookupCache);
			this.m_GlobalSystemVersion = access->EntityComponentStore->GlobalSystemVersion;
			this.m_IsZeroSized = (ComponentType.FromTypeIndex(typeIndex).IsZeroSized ? 1 : 0);
		}

		// Token: 0x06000C00 RID: 3072 RVA: 0x00037C58 File Offset: 0x00035E58
		public unsafe void Update(SystemBase system)
		{
			this.Update(ref *system.m_StatePtr);
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x00037C66 File Offset: 0x00035E66
		public unsafe void Update(ref SystemState systemState)
		{
			this.m_GlobalSystemVersion = systemState.m_EntityComponentStore->GlobalSystemVersion;
		}

		// Token: 0x06000C02 RID: 3074 RVA: 0x00037C79 File Offset: 0x00035E79
		public unsafe bool EntityExists(Entity entity)
		{
			return this.m_Access->EntityComponentStore->Exists(entity);
		}

		// Token: 0x06000C03 RID: 3075 RVA: 0x00037C8C File Offset: 0x00035E8C
		public bool HasComponent(Entity entity)
		{
			bool flag;
			return this.HasComponent(entity, out flag);
		}

		// Token: 0x06000C04 RID: 3076 RVA: 0x00037CA2 File Offset: 0x00035EA2
		public unsafe bool HasComponent(Entity entity, out bool entityExists)
		{
			return this.m_Access->EntityComponentStore->HasComponent(entity, this.m_TypeIndex, ref this.m_Cache, out entityExists);
		}

		// Token: 0x06000C05 RID: 3077 RVA: 0x00037CC4 File Offset: 0x00035EC4
		public unsafe bool HasComponent(SystemHandle system)
		{
			bool flag;
			return this.m_Access->EntityComponentStore->HasComponent(system.m_Entity, this.m_TypeIndex, ref this.m_Cache, out flag);
		}

		// Token: 0x06000C06 RID: 3078 RVA: 0x00037CF8 File Offset: 0x00035EF8
		public unsafe bool TryGetComponent(Entity entity, out T componentData, out bool entityExists)
		{
			EntityComponentStore* entityComponentStore = this.m_Access->EntityComponentStore;
			if (this.m_IsZeroSized != 0)
			{
				componentData = default(T);
				return entityComponentStore->HasComponent(entity, this.m_TypeIndex, ref this.m_Cache, out entityExists);
			}
			entityExists = entityComponentStore->Exists(entity);
			if (Hint.Unlikely(!entityExists))
			{
				componentData = default(T);
				return false;
			}
			void* optionalComponentDataWithTypeRO = (void*)entityComponentStore->GetOptionalComponentDataWithTypeRO(entity, this.m_TypeIndex, ref this.m_Cache);
			if (optionalComponentDataWithTypeRO == null)
			{
				componentData = default(T);
				return false;
			}
			UnsafeUtility.CopyPtrToStructure<T>(optionalComponentDataWithTypeRO, out componentData);
			return true;
		}

		// Token: 0x06000C07 RID: 3079 RVA: 0x00037D80 File Offset: 0x00035F80
		public bool TryGetComponent(Entity entity, out T componentData)
		{
			bool flag;
			return this.TryGetComponent(entity, out componentData, out flag);
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x00037D98 File Offset: 0x00035F98
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

		// Token: 0x170000DF RID: 223
		public unsafe T this[Entity entity]
		{
			get
			{
				EntityComponentStore* entityComponentStore = this.m_Access->EntityComponentStore;
				if (this.m_IsZeroSized != 0)
				{
					return default(T);
				}
				void* componentDataWithTypeRO = (void*)entityComponentStore->GetComponentDataWithTypeRO(entity, this.m_TypeIndex, ref this.m_Cache);
				T t;
				UnsafeUtility.CopyPtrToStructure<T>(componentDataWithTypeRO, out t);
				return t;
			}
			set
			{
				EntityComponentStore* entityComponentStore = this.m_Access->EntityComponentStore;
				if (this.m_IsZeroSized != 0)
				{
					return;
				}
				void* componentDataWithTypeRW = (void*)entityComponentStore->GetComponentDataWithTypeRW(entity, this.m_TypeIndex, this.m_GlobalSystemVersion, ref this.m_Cache);
				UnsafeUtility.CopyStructureToPtr<T>(ref value, componentDataWithTypeRW);
			}
		}

		// Token: 0x170000E0 RID: 224
		public T this[SystemHandle system]
		{
			get
			{
				return this[system.m_Entity];
			}
			set
			{
				this[system.m_Entity] = value;
			}
		}

		// Token: 0x06000C0D RID: 3085 RVA: 0x00037EBD File Offset: 0x000360BD
		public unsafe bool IsComponentEnabled(Entity entity)
		{
			return this.m_Access->IsComponentEnabled(entity, this.m_TypeIndex, ref this.m_Cache);
		}

		// Token: 0x06000C0E RID: 3086 RVA: 0x00037ED7 File Offset: 0x000360D7
		public unsafe bool IsComponentEnabled(SystemHandle systemHandle)
		{
			return this.m_Access->IsComponentEnabled(systemHandle.m_Entity, this.m_TypeIndex, ref this.m_Cache);
		}

		// Token: 0x06000C0F RID: 3087 RVA: 0x00037EF6 File Offset: 0x000360F6
		public unsafe void SetComponentEnabled(SystemHandle systemHandle, bool value)
		{
			this.m_Access->SetComponentEnabled(systemHandle.m_Entity, this.m_TypeIndex, value, ref this.m_Cache);
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x00037F16 File Offset: 0x00036116
		public unsafe void SetComponentEnabled(Entity entity, bool value)
		{
			this.m_Access->SetComponentEnabled(entity, this.m_TypeIndex, value, ref this.m_Cache);
		}

		// Token: 0x06000C11 RID: 3089 RVA: 0x00037F34 File Offset: 0x00036134
		public unsafe RefRW<T> GetRefRW(SystemHandle system)
		{
			EntityComponentStore* entityComponentStore = this.m_Access->EntityComponentStore;
			if (this.m_IsZeroSized != 0)
			{
				return default(RefRW<T>);
			}
			void* componentDataWithTypeRW = (void*)entityComponentStore->GetComponentDataWithTypeRW(system.m_Entity, this.m_TypeIndex, this.m_GlobalSystemVersion, ref this.m_Cache);
			return new RefRW<T>(componentDataWithTypeRW);
		}

		// Token: 0x06000C12 RID: 3090 RVA: 0x00037F84 File Offset: 0x00036184
		public unsafe RefRW<T> GetRefRW(Entity entity)
		{
			EntityComponentStore* entityComponentStore = this.m_Access->EntityComponentStore;
			if (this.m_IsZeroSized != 0)
			{
				return default(RefRW<T>);
			}
			void* componentDataWithTypeRW = (void*)entityComponentStore->GetComponentDataWithTypeRW(entity, this.m_TypeIndex, this.m_GlobalSystemVersion, ref this.m_Cache);
			return new RefRW<T>(componentDataWithTypeRW);
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x00037FD0 File Offset: 0x000361D0
		public unsafe RefRO<T> GetRefRO(Entity entity)
		{
			EntityComponentStore* entityComponentStore = this.m_Access->EntityComponentStore;
			if (this.m_IsZeroSized != 0)
			{
				return default(RefRO<T>);
			}
			void* componentDataWithTypeRO = (void*)entityComponentStore->GetComponentDataWithTypeRO(entity, this.m_TypeIndex, ref this.m_Cache);
			return new RefRO<T>(componentDataWithTypeRO);
		}

		// Token: 0x06000C14 RID: 3092 RVA: 0x00038018 File Offset: 0x00036218
		public unsafe RefRW<T> GetRefRWOptional(Entity entity)
		{
			if (!this.HasComponent(entity))
			{
				return new RefRW<T>(null);
			}
			if (this.m_IsZeroSized != 0)
			{
				return default(RefRW<T>);
			}
			void* componentDataWithTypeRW = (void*)this.m_Access->EntityComponentStore->GetComponentDataWithTypeRW(entity, this.m_TypeIndex, this.m_GlobalSystemVersion, ref this.m_Cache);
			return new RefRW<T>(componentDataWithTypeRW);
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x00038074 File Offset: 0x00036274
		public unsafe RefRO<T> GetRefROOptional(Entity entity)
		{
			if (!this.HasComponent(entity))
			{
				return new RefRO<T>(null);
			}
			if (this.m_IsZeroSized != 0)
			{
				return default(RefRO<T>);
			}
			void* componentDataWithTypeRO = (void*)this.m_Access->EntityComponentStore->GetComponentDataWithTypeRO(entity, this.m_TypeIndex, ref this.m_Cache);
			return new RefRO<T>(componentDataWithTypeRO);
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x000344A8 File Offset: 0x000326A8
		private unsafe SafeBitRef MakeSafeBitRef(ulong* ptr, int offsetInBits)
		{
			return new SafeBitRef(ptr, offsetInBits);
		}

		// Token: 0x06000C17 RID: 3095 RVA: 0x000380C8 File Offset: 0x000362C8
		public unsafe EnabledRefRW<T2> GetEnabledRefRW<[global::System.Runtime.CompilerServices.IsUnmanaged] T2>(Entity entity) where T2 : struct, ValueType, IEnableableComponent, IComponentData
		{
			int num;
			int* ptr;
			ulong* enabledRawRW = this.m_Access->EntityComponentStore->GetEnabledRawRW(entity, this.m_TypeIndex, ref this.m_Cache, this.m_GlobalSystemVersion, out num, out ptr);
			return new EnabledRefRW<T2>(this.MakeSafeBitRef(enabledRawRW, num), ptr);
		}

		// Token: 0x06000C18 RID: 3096 RVA: 0x0003810C File Offset: 0x0003630C
		public unsafe EnabledRefRW<T2> GetEnabledRefRWOptional<[global::System.Runtime.CompilerServices.IsUnmanaged] T2>(Entity entity) where T2 : struct, ValueType, IComponentData, IEnableableComponent
		{
			if (!this.HasComponent(entity))
			{
				return new EnabledRefRW<T2>(default(SafeBitRef), null);
			}
			int num;
			int* ptr;
			ulong* enabledRawRW = this.m_Access->EntityComponentStore->GetEnabledRawRW(entity, this.m_TypeIndex, ref this.m_Cache, this.m_GlobalSystemVersion, out num, out ptr);
			return new EnabledRefRW<T2>(this.MakeSafeBitRef(enabledRawRW, num), ptr);
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x00038169 File Offset: 0x00036369
		[Obsolete("This method has been renamed to GetEnabledRefRWOptional<T>. (RemovedAfter Entities 1.0)", false)]
		public EnabledRefRW<T2> GetComponentEnabledRefRWOptional<[global::System.Runtime.CompilerServices.IsUnmanaged] T2>(Entity entity) where T2 : struct, ValueType, IComponentData, IEnableableComponent
		{
			return this.GetEnabledRefRWOptional<T2>(entity);
		}

		// Token: 0x06000C1A RID: 3098 RVA: 0x00038174 File Offset: 0x00036374
		public unsafe EnabledRefRO<T2> GetEnabledRefRO<[global::System.Runtime.CompilerServices.IsUnmanaged] T2>(Entity entity) where T2 : struct, ValueType, IEnableableComponent, IComponentData
		{
			int num;
			int* ptr;
			ulong* enabledRawRO = this.m_Access->EntityComponentStore->GetEnabledRawRO(entity, this.m_TypeIndex, ref this.m_Cache, out num, out ptr);
			return new EnabledRefRO<T2>(this.MakeSafeBitRef(enabledRawRO, num));
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x000381B0 File Offset: 0x000363B0
		public unsafe EnabledRefRO<T2> GetEnabledRefROOptional<[global::System.Runtime.CompilerServices.IsUnmanaged] T2>(Entity entity) where T2 : struct, ValueType, IComponentData, IEnableableComponent
		{
			if (!this.HasComponent(entity))
			{
				return new EnabledRefRO<T2>(default(SafeBitRef));
			}
			int num;
			int* ptr;
			ulong* enabledRawRO = this.m_Access->EntityComponentStore->GetEnabledRawRO(entity, this.m_TypeIndex, ref this.m_Cache, out num, out ptr);
			return new EnabledRefRO<T2>(this.MakeSafeBitRef(enabledRawRO, num));
		}

		// Token: 0x06000C1C RID: 3100 RVA: 0x00038204 File Offset: 0x00036404
		[Obsolete("This method has been renamed to GetEnabledRefROOptional<T>. (RemovedAfter Entities 1.0)", false)]
		public EnabledRefRO<T2> GetComponentEnabledRefROOptional<[global::System.Runtime.CompilerServices.IsUnmanaged] T2>(Entity entity) where T2 : struct, ValueType, IComponentData, IEnableableComponent
		{
			return this.GetEnabledRefROOptional<T2>(entity);
		}

		// Token: 0x04000556 RID: 1366
		[NativeDisableUnsafePtrRestriction]
		private unsafe readonly EntityDataAccess* m_Access;

		// Token: 0x04000557 RID: 1367
		private LookupCache m_Cache;

		// Token: 0x04000558 RID: 1368
		private readonly TypeIndex m_TypeIndex;

		// Token: 0x04000559 RID: 1369
		private uint m_GlobalSystemVersion;

		// Token: 0x0400055A RID: 1370
		private readonly byte m_IsZeroSized;
	}
}
