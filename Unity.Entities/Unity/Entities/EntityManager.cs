using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using JetBrains.Annotations;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Profiling;
using UnityEngine.Scripting;
using UnityEngine.TestTools;

namespace Unity.Entities
{
	// Token: 0x02000171 RID: 369
	[Preserve]
	[NativeContainer]
	[DebuggerTypeProxy(typeof(EntityManagerDebugView))]
	[DebuggerDisplay("{Debugger_GetName}")]
	[GenerateTestsForBurstCompatibility]
	[BurstCompile]
	public struct EntityManager : IEquatable<EntityManager>
	{
		// Token: 0x0600087F RID: 2175 RVA: 0x00002287 File Offset: 0x00000487
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void AssertIsExclusiveTransaction()
		{
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x0002C98D File Offset: 0x0002AB8D
		internal unsafe EntityDataAccess* GetCheckedEntityDataAccess()
		{
			return this.m_EntityDataAccess;
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x0002C995 File Offset: 0x0002AB95
		internal unsafe EntityDataAccess* GetCheckedEntityDataAccessExclusive()
		{
			if (!this.CanBeginExclusiveEntityTransaction())
			{
				this.EndExclusiveEntityTransaction();
			}
			return this.m_EntityDataAccess;
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x0002C98D File Offset: 0x0002AB8D
		internal unsafe EntityDataAccess* GetCheckedEntityDataAccess(SystemHandle system)
		{
			return this.m_EntityDataAccess;
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x0002C995 File Offset: 0x0002AB95
		internal unsafe EntityDataAccess* GetCheckedEntityDataAccessExclusive(SystemHandle system)
		{
			if (!this.CanBeginExclusiveEntityTransaction())
			{
				this.EndExclusiveEntityTransaction();
			}
			return this.m_EntityDataAccess;
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x0002C98D File Offset: 0x0002AB8D
		internal unsafe EntityDataAccess* GetUncheckedEntityDataAccess()
		{
			return this.m_EntityDataAccess;
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x0002C98D File Offset: 0x0002AB8D
		internal unsafe EntityDataAccess* Debugger_GetEntityDataAccess()
		{
			return this.m_EntityDataAccess;
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000886 RID: 2182 RVA: 0x0002C9AC File Offset: 0x0002ABAC
		private unsafe string Debugger_GetName
		{
			get
			{
				EntityDataAccess* ptr = this.Debugger_GetEntityDataAccess();
				if (ptr == null)
				{
					return "Null";
				}
				return ptr->ManagedEntityDataAccess.World.ToString();
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000887 RID: 2183 RVA: 0x0002C9DB File Offset: 0x0002ABDB
		[ExcludeFromBurstCompatTesting("Returns managed World")]
		public unsafe World World
		{
			get
			{
				return this.GetCheckedEntityDataAccess()->ManagedEntityDataAccess.World;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000888 RID: 2184 RVA: 0x0002C9ED File Offset: 0x0002ABED
		public unsafe WorldUnmanaged WorldUnmanaged
		{
			get
			{
				return this.GetCheckedEntityDataAccess()->m_WorldUnmanaged;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000889 RID: 2185 RVA: 0x0002C9FA File Offset: 0x0002ABFA
		public unsafe int EntityOrderVersion
		{
			get
			{
				return this.GetCheckedEntityDataAccess()->EntityComponentStore->EntityOrderVersion;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x0600088A RID: 2186 RVA: 0x0002C9FA File Offset: 0x0002ABFA
		[Obsolete("EntityManager.Version has been deprecated. Use EntityOrderVersion instead (UnityUpgradable) -> EntityOrderVersion")]
		public unsafe int Version
		{
			get
			{
				return this.GetCheckedEntityDataAccess()->EntityComponentStore->EntityOrderVersion;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x0600088B RID: 2187 RVA: 0x0002CA0C File Offset: 0x0002AC0C
		public unsafe uint GlobalSystemVersion
		{
			get
			{
				return this.GetCheckedEntityDataAccess()->EntityComponentStore->GlobalSystemVersion;
			}
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x0002CA20 File Offset: 0x0002AC20
		public unsafe int HighestEntityIndex()
		{
			int num = 0;
			UnsafePtrList<Archetype> archetypes = this.GetCheckedEntityDataAccess()->EntityComponentStore->m_Archetypes;
			int i = 0;
			int length = archetypes.Length;
			while (i < length)
			{
				ArchetypeChunkData chunks = archetypes[i]->Chunks;
				int j = 0;
				int count = chunks.Count;
				while (j < count)
				{
					ChunkIndex chunkIndex = chunks[j];
					Entity* buffer = (Entity*)chunkIndex.Buffer;
					int k = 0;
					int count2 = chunkIndex.Count;
					while (k < count2)
					{
						int index = buffer[k].Index;
						if (index > num)
						{
							num = index;
						}
						k++;
					}
					j++;
				}
				i++;
			}
			return num;
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x0600088D RID: 2189 RVA: 0x0002CAC8 File Offset: 0x0002ACC8
		public unsafe EntityQuery UniversalQueryWithSystems
		{
			get
			{
				return this.GetCheckedEntityDataAccess()->m_UniversalQueryWithSystems;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x0600088E RID: 2190 RVA: 0x0002CAD5 File Offset: 0x0002ACD5
		public unsafe EntityQuery UniversalQuery
		{
			get
			{
				return this.GetCheckedEntityDataAccess()->m_UniversalQuery;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600088F RID: 2191 RVA: 0x0002CAE2 File Offset: 0x0002ACE2
		public EntityManager.EntityManagerDebug Debug
		{
			get
			{
				return new EntityManager.EntityManagerDebug(this);
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000890 RID: 2192 RVA: 0x0002CAEF File Offset: 0x0002ACEF
		// (set) Token: 0x06000891 RID: 2193 RVA: 0x0002CAF6 File Offset: 0x0002ACF6
		public static ulong TotalChunkAddressSpaceInBytes
		{
			get
			{
				return EntityComponentStore.TotalChunkAddressSpaceInBytes;
			}
			set
			{
				EntityComponentStore.TotalChunkAddressSpaceInBytes = value;
			}
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x0002CB00 File Offset: 0x0002AD00
		[ExcludeFromBurstCompatTesting("Takes managed World")]
		internal unsafe void Initialize(World world)
		{
			TypeManager.Initialize();
			ECBInterop.Initialize();
			EntityQueryImpl.Initialize();
			EarlyInitHelpers.FlushEarlyInits();
			SystemBaseRegistry.InitializePendingTypes();
			this.m_EntityDataAccess = (EntityDataAccess*)Memory.Unmanaged.Allocate((long)sizeof(EntityDataAccess), 16, Allocator.Persistent);
			UnsafeUtility.MemClear((void*)this.m_EntityDataAccess, (long)sizeof(EntityDataAccess));
			EntityDataAccess.Initialize(this.m_EntityDataAccess, world);
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x0002CB5E File Offset: 0x0002AD5E
		internal unsafe void PreDisposeCheck()
		{
			this.EndExclusiveEntityTransaction();
			this.GetCheckedEntityDataAccess()->DependencyManager->PreDisposeCheck();
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x0002CB76 File Offset: 0x0002AD76
		[ExcludeFromBurstCompatTesting("Disposes managed lists")]
		internal unsafe void DestroyInstance()
		{
			this.PreDisposeCheck();
			this.GetCheckedEntityDataAccess()->Dispose();
			Memory.Unmanaged.Free<EntityDataAccess>(this.m_EntityDataAccess, Allocator.Persistent);
			this.m_EntityDataAccess = null;
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x0002CBA4 File Offset: 0x0002ADA4
		internal static EntityManager CreateEntityManagerInUninitializedState()
		{
			return default(EntityManager);
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x0002CBBA File Offset: 0x0002ADBA
		public bool Equals(EntityManager other)
		{
			return this.m_EntityDataAccess == other.m_EntityDataAccess;
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x0002CBCC File Offset: 0x0002ADCC
		[ExcludeFromBurstCompatTesting("Takes managed object")]
		public override bool Equals(object obj)
		{
			if (obj is EntityManager)
			{
				EntityManager entityManager = (EntityManager)obj;
				return this.Equals(entityManager);
			}
			return false;
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x0002CBF1 File Offset: 0x0002ADF1
		public override int GetHashCode()
		{
			return this.m_EntityDataAccess;
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x0002CBBA File Offset: 0x0002ADBA
		public static bool operator ==(EntityManager lhs, EntityManager rhs)
		{
			return lhs.m_EntityDataAccess == rhs.m_EntityDataAccess;
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x0002CBFB File Offset: 0x0002ADFB
		public static bool operator !=(EntityManager lhs, EntityManager rhs)
		{
			return lhs.m_EntityDataAccess != rhs.m_EntityDataAccess;
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x0002CC0E File Offset: 0x0002AE0E
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe T GetComponentData<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity entity) where T : struct, ValueType, IComponentData
		{
			return this.GetCheckedEntityDataAccess()->GetComponentData<T>(entity);
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x0002CC1C File Offset: 0x0002AE1C
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe T GetComponentData<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(SystemHandle system) where T : struct, ValueType, IComponentData
		{
			return this.GetCheckedEntityDataAccess(system)->GetComponentData<T>(system.m_Entity);
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x0002CC30 File Offset: 0x0002AE30
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe RefRW<T> GetComponentDataRW<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(SystemHandle system) where T : struct, ValueType, IComponentData
		{
			ref EntityDataAccess checkedEntityDataAccess = ref *this.GetCheckedEntityDataAccess(system);
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			return new RefRW<T>(checkedEntityDataAccess.GetComponentDataRW_AsBytePointer(system.m_Entity, typeIndex));
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x0002CC5C File Offset: 0x0002AE5C
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe void SetComponentData<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity entity, T componentData) where T : struct, ValueType, IComponentData
		{
			ref EntityDataAccess checkedEntityDataAccess = ref *this.GetCheckedEntityDataAccess();
			SystemHandle systemHandle = default(SystemHandle);
			checkedEntityDataAccess.SetComponentData<T>(entity, componentData, in systemHandle);
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x0002CC80 File Offset: 0x0002AE80
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe void SetComponentData<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(SystemHandle system, T componentData) where T : struct, ValueType, IComponentData
		{
			ref EntityDataAccess checkedEntityDataAccess = ref *this.GetCheckedEntityDataAccess(system);
			Entity entity = system.m_Entity;
			SystemHandle systemHandle = default(SystemHandle);
			checkedEntityDataAccess.SetComponentData<T>(entity, componentData, in systemHandle);
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x0002CCAC File Offset: 0x0002AEAC
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public T GetChunkComponentData<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ArchetypeChunk chunk) where T : struct, ValueType, IComponentData
		{
			Entity metaChunkEntity = chunk.m_Chunk.MetaChunkEntity;
			return this.GetComponentData<T>(metaChunkEntity);
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x0002CCD0 File Offset: 0x0002AED0
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe T GetChunkComponentData<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity entity) where T : struct, ValueType, IComponentData
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			Entity metaChunkEntity = checkedEntityDataAccess->EntityComponentStore->GetChunk(entity).MetaChunkEntity;
			return checkedEntityDataAccess->GetComponentData<T>(metaChunkEntity);
		}

		// Token: 0x060008A2 RID: 2210 RVA: 0x0002CD00 File Offset: 0x0002AF00
		[EntityManager.StructuralChangeMethodAttribute]
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public void SetChunkComponentData<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ArchetypeChunk chunk, T componentValue) where T : struct, ValueType, IComponentData
		{
			Entity metaChunkEntity = chunk.m_Chunk.MetaChunkEntity;
			this.SetComponentData<T>(metaChunkEntity, componentValue);
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x0002CD22 File Offset: 0x0002AF22
		[ExcludeFromBurstCompatTesting("Returns managed object")]
		public unsafe T GetComponentObject<T>(Entity entity)
		{
			return (ref *this.GetCheckedEntityDataAccess()).GetComponentObject<T>(entity, ComponentType.ReadWrite<T>());
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x0002CD35 File Offset: 0x0002AF35
		[ExcludeFromBurstCompatTesting("Returns managed object")]
		public unsafe T GetComponentObject<T>(Entity entity, ComponentType componentType)
		{
			return (ref *this.GetCheckedEntityDataAccess()).GetComponentObject<T>(entity, componentType);
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x0002CD44 File Offset: 0x0002AF44
		[ExcludeFromBurstCompatTesting("Returns managed object")]
		public unsafe T GetComponentObject<T>(SystemHandle system)
		{
			return (ref *this.GetCheckedEntityDataAccess(system)).GetComponentObject<T>(system.m_Entity, ComponentType.ReadWrite<T>());
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x0002CD5D File Offset: 0x0002AF5D
		[ExcludeFromBurstCompatTesting("Returns managed object")]
		public unsafe T GetComponentObject<T>(SystemHandle system, ComponentType componentType)
		{
			return (ref *this.GetCheckedEntityDataAccess(system)).GetComponentObject<T>(system.m_Entity, componentType);
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x0002CD72 File Offset: 0x0002AF72
		[ExcludeFromBurstCompatTesting("Accesses managed component store")]
		public unsafe int GetSharedComponentCount()
		{
			return this.GetCheckedEntityDataAccess()->GetSharedComponentCount();
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x00002287 File Offset: 0x00000487
		[Obsolete("Use SetSharedComponentManaged<T> (UnityUpgradable) -> SetSharedComponentManaged<T>(*)", true)]
		public void SetSharedComponentData<T>(Entity entity, T componentData) where T : struct, ISharedComponentData
		{
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x0002CD80 File Offset: 0x0002AF80
		[EntityManager.StructuralChangeMethodAttribute]
		[ExcludeFromBurstCompatTesting("Accesses managed component store")]
		public unsafe void SetSharedComponentManaged<T>(Entity entity, T componentData) where T : struct, ISharedComponentData
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginStructuralChanges();
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			if (TypeManager.IsManagedSharedComponent(typeIndex))
			{
				ref EntityDataAccess ptr = ref *checkedEntityDataAccess;
				T t = componentData;
				SystemHandle systemHandle = default(SystemHandle);
				ptr.SetSharedComponentData_Managed<T>(entity, t, in systemHandle);
			}
			else
			{
				T t2 = default(T);
				ref EntityDataAccess ptr2 = ref *checkedEntityDataAccess;
				TypeIndex typeIndex2 = typeIndex;
				void* ptr3 = UnsafeUtility.AddressOf<T>(ref componentData);
				void* ptr4 = UnsafeUtility.AddressOf<T>(ref t2);
				SystemHandle systemHandle = default(SystemHandle);
				ptr2.SetSharedComponentData_Unmanaged(entity, typeIndex2, ptr3, ptr4, in systemHandle);
			}
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x0002CDEC File Offset: 0x0002AFEC
		[EntityManager.StructuralChangeMethodAttribute]
		[ExcludeFromBurstCompatTesting("Accesses managed component store")]
		public unsafe void SetSharedComponentManaged<T>(NativeArray<Entity> entities, T componentData) where T : struct, ISharedComponentData
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginStructuralChanges();
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			if (TypeManager.IsManagedSharedComponent(typeIndex))
			{
				ref EntityDataAccess ptr = ref *checkedEntityDataAccess;
				T t = componentData;
				SystemHandle systemHandle = default(SystemHandle);
				ptr.SetSharedComponentData_Managed<T>(entities, t, in systemHandle);
			}
			else
			{
				T t2 = default(T);
				ref EntityDataAccess ptr2 = ref *checkedEntityDataAccess;
				TypeIndex typeIndex2 = typeIndex;
				void* ptr3 = UnsafeUtility.AddressOf<T>(ref componentData);
				void* ptr4 = UnsafeUtility.AddressOf<T>(ref t2);
				SystemHandle systemHandle = default(SystemHandle);
				ptr2.SetSharedComponentData_Unmanaged(entities, typeIndex2, ptr3, ptr4, in systemHandle);
			}
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x0002CE58 File Offset: 0x0002B058
		[EntityManager.StructuralChangeMethodAttribute]
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleSharedComponentData) })]
		public unsafe void SetSharedComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(EntityQuery query, T componentData) where T : struct, ValueType, ISharedComponentData
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			EntityQueryImpl* ptr = query._GetImpl();
			if (ptr->IsEmptyIgnoreFilter)
			{
				return;
			}
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginStructuralChanges();
			int num = checkedEntityDataAccess->InsertSharedComponent_Unmanaged<T>(componentData);
			ref EntityDataAccess ptr2 = ref *checkedEntityDataAccess;
			EntityQueryImpl* ptr3 = ptr;
			int num2 = num;
			ComponentType componentType = ComponentType.ReadWrite<T>();
			void* ptr4 = UnsafeUtility.AddressOf<T>(ref componentData);
			SystemHandle systemHandle = default(SystemHandle);
			ptr2.SetSharedComponentDataOnQueryDuringStructuralChange_Unmanaged(ptr3, num2, componentType, ptr4, in systemHandle);
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x00002287 File Offset: 0x00000487
		[Obsolete("Use SetSharedComponentManaged<T> (UnityUpgradable) -> SetSharedComponentManaged<T>(*)", true)]
		public void SetSharedComponentData<T>(EntityQuery query, T componentData) where T : struct, ISharedComponentData
		{
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x0002CEB4 File Offset: 0x0002B0B4
		[EntityManager.StructuralChangeMethodAttribute]
		[ExcludeFromBurstCompatTesting("Accesses managed component store")]
		public unsafe void SetSharedComponentManaged<T>(EntityQuery query, T componentData) where T : struct, ISharedComponentData
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			EntityQueryImpl* ptr = query._GetImpl();
			if (ptr->IsEmptyIgnoreFilter)
			{
				return;
			}
			ComponentType componentType = ComponentType.ReadWrite<T>();
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginStructuralChanges();
			int num = checkedEntityDataAccess->InsertSharedComponent<T>(componentData);
			if (TypeManager.IsManagedSharedComponent(componentType.TypeIndex))
			{
				ref EntityDataAccess ptr2 = ref *checkedEntityDataAccess;
				EntityQueryImpl* ptr3 = ptr;
				int num2 = num;
				ComponentType componentType2 = componentType;
				SystemHandle systemHandle = default(SystemHandle);
				ptr2.SetSharedComponentDataOnQueryDuringStructuralChange(ptr3, num2, componentType2, in systemHandle);
			}
			else
			{
				ref EntityDataAccess ptr4 = ref *checkedEntityDataAccess;
				EntityQueryImpl* ptr5 = ptr;
				int num3 = num;
				ComponentType componentType3 = componentType;
				void* ptr6 = UnsafeUtility.AddressOf<T>(ref componentData);
				SystemHandle systemHandle = default(SystemHandle);
				ptr4.SetSharedComponentDataOnQueryDuringStructuralChange_Unmanaged(ptr5, num3, componentType3, ptr6, in systemHandle);
			}
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x0002CF33 File Offset: 0x0002B133
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleSharedComponentData) })]
		public unsafe T GetSharedComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity entity) where T : struct, ValueType, ISharedComponentData
		{
			return this.GetCheckedEntityDataAccess()->GetSharedComponentData_Unmanaged<T>(entity);
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x0002CF44 File Offset: 0x0002B144
		[Obsolete("Use GetSharedComponent<T> (UnityUpgradable) -> GetSharedComponent<T>(*)", true)]
		public T GetUnmanagedSharedComponentData<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity entity) where T : struct, ValueType, ISharedComponentData
		{
			return default(T);
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x0002CF5C File Offset: 0x0002B15C
		[ExcludeFromBurstCompatTesting("Accesses managed component store")]
		public unsafe int GetSharedComponentIndexManaged<T>(Entity entity) where T : struct, ISharedComponentData
		{
			ref EntityComponentStore entityComponentStore = ref *this.GetCheckedEntityDataAccess()->EntityComponentStore;
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			return entityComponentStore.GetSharedComponentDataIndex(entity, typeIndex);
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x0002CF84 File Offset: 0x0002B184
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleSharedComponentData) })]
		public unsafe int GetSharedComponentIndex<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity entity) where T : struct, ValueType, ISharedComponentData
		{
			ref EntityComponentStore entityComponentStore = ref *this.GetCheckedEntityDataAccess()->EntityComponentStore;
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			return entityComponentStore.GetSharedComponentDataIndex(entity, typeIndex);
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x0002CFA9 File Offset: 0x0002B1A9
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleSharedComponentData) })]
		[Obsolete("Use GetSharedComponentIndex<T> instead. (RemovedAfter Entities 1.0) (UnityUpgradable) -> GetSharedComponentIndex<T>(*)", true)]
		public int GetSharedComponentDataIndex<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity entity) where T : struct, ValueType, ISharedComponentData
		{
			return this.GetSharedComponentIndex<T>(entity);
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x0002CFB2 File Offset: 0x0002B1B2
		[Obsolete("Use GetSharedComponentIndex<T> (UnityUpgradable) -> GetSharedComponentIndex<T>(*)", true)]
		public int GetUnmanagedSharedComponentDataIndex<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity entity) where T : struct, ValueType, ISharedComponentData
		{
			return -1;
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x0002CFB5 File Offset: 0x0002B1B5
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleSharedComponentData) })]
		public unsafe T GetSharedComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(int sharedComponentIndex) where T : struct, ValueType, ISharedComponentData
		{
			return this.GetCheckedEntityDataAccess()->GetSharedComponentData_Unmanaged<T>(sharedComponentIndex);
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x0002CFC4 File Offset: 0x0002B1C4
		[Obsolete("Use GetSharedComponent<T> (UnityUpgradable) -> GetSharedComponent<T>(*)", true)]
		public T GetUnmanagedSharedComponentData<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(int sharedComponentIndex) where T : struct, ValueType, ISharedComponentData
		{
			return default(T);
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x00002287 File Offset: 0x00000487
		[Obsolete("Use SetSharedComponent<T> (UnityUpgradable) -> SetSharedComponent<T>(*)", true)]
		public void SetUnmanagedSharedComponentData<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity entity, T componentData) where T : struct, ValueType, ISharedComponentData
		{
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x0002CFDC File Offset: 0x0002B1DC
		[EntityManager.StructuralChangeMethodAttribute]
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleSharedComponentData) })]
		public unsafe void SetSharedComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity entity, T componentData) where T : struct, ValueType, ISharedComponentData
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginStructuralChanges();
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			T t = default(T);
			TypeIndex typeIndex2 = typeIndex;
			void* ptr = UnsafeUtility.AddressOf<T>(ref componentData);
			void* ptr2 = UnsafeUtility.AddressOf<T>(ref t);
			SystemHandle systemHandle = default(SystemHandle);
			checkedEntityDataAccess->SetSharedComponentData_Unmanaged(entity, typeIndex2, ptr, ptr2, in systemHandle);
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x0002D02C File Offset: 0x0002B22C
		[EntityManager.StructuralChangeMethodAttribute]
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleSharedComponentData) })]
		public unsafe void SetSharedComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(NativeArray<Entity> entities, T componentData) where T : struct, ValueType, ISharedComponentData
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginStructuralChanges();
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			T t = default(T);
			TypeIndex typeIndex2 = typeIndex;
			void* ptr = UnsafeUtility.AddressOf<T>(ref componentData);
			void* ptr2 = UnsafeUtility.AddressOf<T>(ref t);
			SystemHandle systemHandle = default(SystemHandle);
			checkedEntityDataAccess->SetSharedComponentData_Unmanaged(entities, typeIndex2, ptr, ptr2, in systemHandle);
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x0002D07C File Offset: 0x0002B27C
		[EntityManager.StructuralChangeMethodAttribute]
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleSharedComponentData) })]
		internal unsafe void SetSharedComponentNonDefault(Entity entity, void* componentDataAddr, TypeIndex typeIndex)
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginStructuralChanges();
			void* ptr = null;
			SystemHandle systemHandle = default(SystemHandle);
			checkedEntityDataAccess->SetSharedComponentData_Unmanaged(entity, typeIndex, componentDataAddr, ptr, in systemHandle);
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x0002D0B4 File Offset: 0x0002B2B4
		[Obsolete("Use GetSharedComponentManaged<T> (UnityUpgradable) -> GetSharedComponentManaged<T>(*)", true)]
		public T GetSharedComponentData<T>(Entity entity) where T : struct, ISharedComponentData
		{
			return default(T);
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x0002D0CC File Offset: 0x0002B2CC
		[EntityManager.StructuralChangeMethodAttribute]
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleSharedComponentData) })]
		public unsafe void SetSharedComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(ArchetypeChunk chunk, T componentData) where T : struct, ValueType, ISharedComponentData
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginStructuralChanges();
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			T t = default(T);
			TypeIndex typeIndex2 = typeIndex;
			void* ptr = UnsafeUtility.AddressOf<T>(ref componentData);
			void* ptr2 = UnsafeUtility.AddressOf<T>(ref t);
			SystemHandle systemHandle = default(SystemHandle);
			checkedEntityDataAccess->SetSharedComponentData_Unmanaged(chunk, typeIndex2, ptr, ptr2, in systemHandle);
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x0002D11C File Offset: 0x0002B31C
		[EntityManager.StructuralChangeMethodAttribute]
		[ExcludeFromBurstCompatTesting("Accesses managed component store")]
		public unsafe void SetSharedComponentManaged<T>(ArchetypeChunk chunk, T componentData) where T : struct, ISharedComponentData
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginStructuralChanges();
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			if (TypeManager.IsManagedSharedComponent(typeIndex))
			{
				ref EntityDataAccess ptr = ref *checkedEntityDataAccess;
				T t = componentData;
				SystemHandle systemHandle = default(SystemHandle);
				ptr.SetSharedComponentData_Managed<T>(chunk, t, in systemHandle);
			}
			else
			{
				T t2 = default(T);
				ref EntityDataAccess ptr2 = ref *checkedEntityDataAccess;
				TypeIndex typeIndex2 = typeIndex;
				void* ptr3 = UnsafeUtility.AddressOf<T>(ref componentData);
				void* ptr4 = UnsafeUtility.AddressOf<T>(ref t2);
				SystemHandle systemHandle = default(SystemHandle);
				ptr2.SetSharedComponentData_Unmanaged(chunk, typeIndex2, ptr3, ptr4, in systemHandle);
			}
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x0002D187 File Offset: 0x0002B387
		[ExcludeFromBurstCompatTesting("Accesses managed component store")]
		public unsafe T GetSharedComponentManaged<T>(Entity entity) where T : struct, ISharedComponentData
		{
			return this.GetCheckedEntityDataAccess()->GetSharedComponentData<T>(entity);
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x0002D198 File Offset: 0x0002B398
		[Obsolete("Use GetSharedComponentManaged<T> (UnityUpgradable) -> GetSharedComponentManaged<T>(*)", true)]
		public T GetSharedComponentData<T>(int sharedComponentIndex) where T : struct, ISharedComponentData
		{
			return default(T);
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x0002D1B0 File Offset: 0x0002B3B0
		[ExcludeFromBurstCompatTesting("Accesses managed component store")]
		public unsafe T GetSharedComponentManaged<T>(int sharedComponentIndex) where T : struct, ISharedComponentData
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			if (EntityComponentStore.IsUnmanagedSharedComponentIndex(sharedComponentIndex))
			{
				T t = default(T);
				checkedEntityDataAccess->EntityComponentStore->GetSharedComponentData_Unmanaged(sharedComponentIndex, TypeManager.GetTypeIndex<T>(), UnsafeUtility.AddressOf<T>(ref t));
				return t;
			}
			return checkedEntityDataAccess->ManagedComponentStore.GetSharedComponentData_Managed<T>(sharedComponentIndex);
		}

		// Token: 0x060008C0 RID: 2240 RVA: 0x0002D1FA File Offset: 0x0002B3FA
		[ExcludeFromBurstCompatTesting("Accesses managed component store")]
		public unsafe object GetSharedComponentDataBoxed(int sharedComponentIndex, TypeIndex typeIndex)
		{
			return this.GetCheckedEntityDataAccess()->GetSharedComponentDataBoxed(sharedComponentIndex, typeIndex);
		}

		// Token: 0x060008C1 RID: 2241 RVA: 0x00002287 File Offset: 0x00000487
		[Obsolete("Use GetAllUniqueSharedComponentsManaged<T> (UnityUpgradable) -> GetAllUniqueSharedComponentsManaged<T>(*)", true)]
		public void GetAllUniqueSharedComponentData<T>(List<T> sharedComponentValues) where T : struct, ISharedComponentData
		{
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x0002D209 File Offset: 0x0002B409
		[ExcludeFromBurstCompatTesting("Accesses managed component store")]
		public unsafe void GetAllUniqueSharedComponentsManaged<T>(List<T> sharedComponentValues) where T : struct, ISharedComponentData
		{
			this.GetCheckedEntityDataAccess()->GetAllUniqueSharedComponents<T>(sharedComponentValues);
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x00002287 File Offset: 0x00000487
		[Obsolete("Use GetAllUniqueSharedComponentsManaged<T> (UnityUpgradable) -> GetAllUniqueSharedComponentsManaged<T>(*)", true)]
		public void GetAllUniqueSharedComponentData<T>(List<T> sharedComponentValues, List<int> sharedComponentIndices) where T : struct, ISharedComponentData
		{
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x0002D217 File Offset: 0x0002B417
		[ExcludeFromBurstCompatTesting("Accesses managed component store")]
		public unsafe void GetAllUniqueSharedComponentsManaged<T>(List<T> sharedComponentValues, List<int> sharedComponentIndices) where T : struct, ISharedComponentData
		{
			this.GetCheckedEntityDataAccess()->GetAllUniqueSharedComponents<T>(sharedComponentValues, sharedComponentIndices);
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x00002287 File Offset: 0x00000487
		[Obsolete("Use GetAllUniqueSharedComponentsManaged<T> (UnityUpgradable) -> GetAllUniqueSharedComponentsManaged<T>(*)", true)]
		public void GetAllUniqueSharedComponentData<T>(List<T> sharedComponentValues, List<int> sharedComponentIndices, List<int> sharedComponentVersions) where T : struct, ISharedComponentData
		{
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x0002D226 File Offset: 0x0002B426
		[ExcludeFromBurstCompatTesting("Accesses managed component store")]
		public unsafe void GetAllUniqueSharedComponentsManaged<T>(List<T> sharedComponentValues, List<int> sharedComponentIndices, List<int> sharedComponentVersions) where T : struct, ISharedComponentData
		{
			this.GetCheckedEntityDataAccess()->GetAllUniqueSharedComponents<T>(sharedComponentValues, sharedComponentIndices, sharedComponentVersions);
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x0002D236 File Offset: 0x0002B436
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleSharedComponentData) })]
		public unsafe void GetAllUniqueSharedComponents<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(out NativeList<T> sharedComponentValues, AllocatorManager.AllocatorHandle allocator) where T : struct, ValueType, ISharedComponentData
		{
			ref EntityDataAccess checkedEntityDataAccess = ref *this.GetCheckedEntityDataAccess();
			sharedComponentValues = new NativeList<T>(0, allocator);
			sharedComponentValues.m_ListData->Dispose();
			checkedEntityDataAccess.GetAllUniqueSharedComponents_Unmanaged<T>(out *sharedComponentValues.m_ListData, allocator);
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x0001CFE4 File Offset: 0x0001B1E4
		[Obsolete("Use AddSharedComponent<T> (UnityUpgradable) -> AddSharedComponent<T>(*)", true)]
		public bool AddUnmanagedSharedComponentData<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity entity, T componentData) where T : struct, ValueType, ISharedComponentData
		{
			return false;
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x0002D264 File Offset: 0x0002B464
		[EntityManager.StructuralChangeMethodAttribute]
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleSharedComponentData) })]
		public unsafe bool AddSharedComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity entity, T componentData) where T : struct, ValueType, ISharedComponentData
		{
			TypeManager.GetTypeIndex<T>();
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginStructuralChanges();
			T t = default(T);
			bool flag = checkedEntityDataAccess->AddSharedComponentDataDuringStructuralChange_Unmanaged(entity, ComponentType.ReadWrite<T>(), UnsafeUtility.AddressOf<T>(ref componentData), UnsafeUtility.AddressOf<T>(ref t));
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
			return flag;
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x0002D2B0 File Offset: 0x0002B4B0
		[EntityManager.StructuralChangeMethodAttribute]
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleSharedComponentData) })]
		public unsafe void AddSharedComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(NativeArray<Entity> entities, T componentData) where T : struct, ValueType, ISharedComponentData
		{
			TypeManager.GetTypeIndex<T>();
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginStructuralChanges();
			T t = default(T);
			ComponentType componentType = ComponentType.ReadWrite<T>();
			void* ptr = UnsafeUtility.AddressOf<T>(ref componentData);
			void* ptr2 = UnsafeUtility.AddressOf<T>(ref t);
			SystemHandle systemHandle = default(SystemHandle);
			checkedEntityDataAccess->AddSharedComponentDataDuringStructuralChange_Unmanaged(entities, componentType, ptr, ptr2, in systemHandle);
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x0002D302 File Offset: 0x0002B502
		[ExcludeFromBurstCompatTesting("Accesses managed component store")]
		public unsafe int GetSharedComponentOrderVersionManaged<T>(T sharedComponent) where T : struct, ISharedComponentData
		{
			return this.GetCheckedEntityDataAccess()->GetSharedComponentVersion<T>(sharedComponent);
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x0002D310 File Offset: 0x0002B510
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleSharedComponentData) })]
		public unsafe int GetSharedComponentOrderVersion<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(T sharedComponent) where T : struct, ValueType, ISharedComponentData
		{
			return this.GetCheckedEntityDataAccess()->GetSharedComponentVersion_Unmanaged<T>(sharedComponent);
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x0002D31E File Offset: 0x0002B51E
		private unsafe DynamicBuffer<T> GetBufferInternal<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(EntityDataAccess* access, Entity entity, bool isReadOnly = false) where T : struct, ValueType, IBufferElementData
		{
			TypeManager.GetTypeIndex<T>();
			return access->GetBuffer<T>(entity, isReadOnly);
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x0002D330 File Offset: 0x0002B530
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleBufferElement) })]
		public unsafe DynamicBuffer<T> GetBuffer<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity entity, bool isReadOnly = false) where T : struct, ValueType, IBufferElementData
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			return this.GetBufferInternal<T>(checkedEntityDataAccess, entity, isReadOnly);
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x0002D350 File Offset: 0x0002B550
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleBufferElement) })]
		public unsafe DynamicBuffer<T> GetBuffer<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(SystemHandle system, bool isReadOnly = false) where T : struct, ValueType, IBufferElementData
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess(system);
			return this.GetBufferInternal<T>(checkedEntityDataAccess, system.m_Entity, isReadOnly);
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x0002D373 File Offset: 0x0002B573
		public unsafe EntityStorageInfo GetStorageInfo(Entity entity)
		{
			return this.GetCheckedEntityDataAccess()->GetStorageInfo(entity);
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x0002D381 File Offset: 0x0002B581
		[EntityManager.StructuralChangeMethodAttribute]
		public unsafe void SwapComponents(ArchetypeChunk leftChunk, int leftIndex, ArchetypeChunk rightChunk, int rightIndex)
		{
			this.GetCheckedEntityDataAccess()->SwapComponents(leftChunk, leftIndex, rightChunk, rightIndex);
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x0002D394 File Offset: 0x0002B594
		public unsafe ArchetypeChunk GetChunk(Entity entity)
		{
			EntityComponentStore* entityComponentStore = this.GetCheckedEntityDataAccess()->EntityComponentStore;
			return new ArchetypeChunk(entityComponentStore->GetChunk(entity), entityComponentStore);
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x0002D3BA File Offset: 0x0002B5BA
		public unsafe int GetComponentCount(Entity entity)
		{
			return this.GetCheckedEntityDataAccess()->EntityComponentStore->GetArchetype(entity)->TypesCount - 1;
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x0002D3D4 File Offset: 0x0002B5D4
		[EntityManager.StructuralChangeMethodAttribute]
		public unsafe bool AddComponent(Entity entity, ComponentType componentType)
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginStructuralChanges();
			SystemHandle systemHandle = default(SystemHandle);
			bool flag = checkedEntityDataAccess->AddComponentDuringStructuralChange(entity, componentType, in systemHandle);
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
			return flag;
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x0002D408 File Offset: 0x0002B608
		[EntityManager.StructuralChangeMethodAttribute]
		public bool AddComponent(SystemHandle system, ComponentType componentType)
		{
			return this.AddComponent(system.m_Entity, componentType);
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x0002D417 File Offset: 0x0002B617
		[EntityManager.StructuralChangeMethodAttribute]
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public bool AddComponent<T>(Entity entity)
		{
			return this.AddComponent(entity, ComponentType.ReadWrite<T>());
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x0002D425 File Offset: 0x0002B625
		[EntityManager.StructuralChangeMethodAttribute]
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public bool AddComponent<T>(SystemHandle system)
		{
			return this.AddComponent(system.m_Entity, ComponentType.ReadWrite<T>());
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x0002D438 File Offset: 0x0002B638
		[EntityManager.StructuralChangeMethodAttribute]
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public Entity CreateSingleton<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(FixedString64Bytes name = default(FixedString64Bytes)) where T : struct, ValueType, IComponentData
		{
			return this.CreateSingletonEntityInternal<T>(name);
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x0002D444 File Offset: 0x0002B644
		[EntityManager.StructuralChangeMethodAttribute]
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public Entity CreateSingleton<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(T componentData, FixedString64Bytes name = default(FixedString64Bytes)) where T : struct, ValueType, IComponentData
		{
			Entity entity = this.CreateSingletonEntityInternal<T>(name);
			this.SetComponentData<T>(entity, componentData);
			return entity;
		}

		// Token: 0x060008DA RID: 2266 RVA: 0x0002D438 File Offset: 0x0002B638
		[EntityManager.StructuralChangeMethodAttribute]
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleBufferElement) })]
		public Entity CreateSingletonBuffer<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(FixedString64Bytes name = default(FixedString64Bytes)) where T : struct, ValueType, IBufferElementData
		{
			return this.CreateSingletonEntityInternal<T>(name);
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x0002D464 File Offset: 0x0002B664
		[EntityManager.StructuralChangeMethodAttribute]
		private unsafe Entity CreateSingletonEntityInternal<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(FixedString64Bytes name = default(FixedString64Bytes)) where T : struct, ValueType
		{
			ComponentType componentType = ComponentType.ReadWrite<T>();
			Entity entity = this.CreateEntity(this.CreateArchetype(&componentType, 1));
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			if ((in name) == (in fixedString32Bytes))
			{
				FixedString128Bytes fixedString128Bytes = componentType.ToFixedString();
				(ref name).CopyFromTruncated<FixedString64Bytes, FixedString128Bytes>(in fixedString128Bytes);
			}
			this.GetCheckedEntityDataAccess()->SetName(entity, in name);
			return entity;
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x0002D4BC File Offset: 0x0002B6BC
		[EntityManager.StructuralChangeMethodAttribute]
		public unsafe void AddComponent(EntityQuery entityQuery, ComponentType componentType)
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			EntityQueryImpl* ptr = entityQuery._GetImpl();
			if (ptr->IsEmptyIgnoreFilter)
			{
				return;
			}
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginStructuralChanges();
			ref EntityDataAccess ptr2 = ref *checkedEntityDataAccess;
			EntityQueryImpl* ptr3 = ptr;
			SystemHandle systemHandle = default(SystemHandle);
			ptr2.AddComponentToQueryDuringStructuralChange(ptr3, componentType, in systemHandle);
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x0002D504 File Offset: 0x0002B704
		[EntityManager.StructuralChangeMethodAttribute]
		public unsafe void AddComponent(EntityQuery entityQuery, in ComponentTypeSet componentTypeSet)
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			EntityQueryImpl* ptr = entityQuery._GetImpl();
			if (ptr->IsEmptyIgnoreFilter)
			{
				return;
			}
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginStructuralChanges();
			ref EntityDataAccess ptr2 = ref *checkedEntityDataAccess;
			EntityQueryImpl* ptr3 = ptr;
			SystemHandle systemHandle = default(SystemHandle);
			ptr2.AddComponentsToQueryDuringStructuralChange(ptr3, in componentTypeSet, in systemHandle);
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x0002D54C File Offset: 0x0002B74C
		[EntityManager.StructuralChangeMethodAttribute]
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe void AddComponentData<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(EntityQuery entityQuery, NativeArray<T> componentArray) where T : struct, ValueType, IComponentData
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			if (entityQuery.IsEmptyIgnoreFilter)
			{
				return;
			}
			EntityQueryData* queryData = entityQuery._GetImpl()->_QueryData;
			if (queryData->HasEnableableComponents != 0)
			{
				for (int i = 0; i < queryData->EnableableComponentTypeIndexCount; i++)
				{
					checkedEntityDataAccess->DependencyManager->CompleteWriteDependency(queryData->EnableableComponentTypeIndices[i]);
				}
			}
			NativeArray<Entity> nativeArray = entityQuery.ToEntityArray(Allocator.TempJob);
			if (nativeArray.Length != componentArray.Length)
			{
				throw new ArgumentException(string.Format("AddComponentData number of entities in query '{0}' must match componentArray.Length '{1}'.", nativeArray.Length, componentArray.Length));
			}
			this.AddComponent(entityQuery, ComponentType.ReadWrite<T>());
			ComponentLookup<T> componentLookup = this.GetComponentLookup<T>(false);
			for (int num = 0; num != componentArray.Length; num++)
			{
				componentLookup[nativeArray[num]] = componentArray[num];
			}
			nativeArray.Dispose();
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x0002D641 File Offset: 0x0002B841
		[EntityManager.StructuralChangeMethodAttribute]
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public void AddComponent<T>(EntityQuery entityQuery)
		{
			this.AddComponent(entityQuery, ComponentType.ReadWrite<T>());
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x0002D650 File Offset: 0x0002B850
		[EntityManager.StructuralChangeMethodAttribute]
		public unsafe void AddComponent(NativeArray<Entity> entities, ComponentType componentType)
		{
			if (entities.Length == 0)
			{
				return;
			}
			if (componentType.IsChunkComponent)
			{
				throw new ArgumentException(string.Format("Cannot add ChunkComponent {0} on NativeArray of entities.", componentType));
			}
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			EntityComponentStore* entityComponentStore = checkedEntityDataAccess->EntityComponentStore;
			NativeList<EntityBatchInChunk> nativeList = default(NativeList<EntityBatchInChunk>);
			bool flag = entities.Length > 10 && entityComponentStore->CreateEntityBatchList(entities, componentType.IsSharedComponent ? 1 : 0, Allocator.Temp, out nativeList);
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginStructuralChanges();
			if (flag)
			{
				StructuralChange.AddComponentEntitiesBatch(entityComponentStore, (UnsafeList<EntityBatchInChunk>*)NativeListUnsafeUtility.GetInternalListDataPtrUnchecked<EntityBatchInChunk>(ref nativeList), componentType.TypeIndex);
			}
			else
			{
				for (int i = 0; i < entities.Length; i++)
				{
					Entity entity = entities[i];
					StructuralChange.AddComponentEntity(entityComponentStore, &entity, componentType.TypeIndex);
				}
			}
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x0002D720 File Offset: 0x0002B920
		[EntityManager.StructuralChangeMethodAttribute]
		public unsafe void AddComponent(NativeArray<Entity> entities, in ComponentTypeSet componentTypeSet)
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginStructuralChanges();
			SystemHandle systemHandle = default(SystemHandle);
			checkedEntityDataAccess->AddMultipleComponentsDuringStructuralChange(entities, in componentTypeSet, in systemHandle);
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x0002D754 File Offset: 0x0002B954
		[EntityManager.StructuralChangeMethodAttribute]
		public unsafe void RemoveComponent(NativeArray<Entity> entities, in ComponentTypeSet componentTypeSet)
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginStructuralChanges();
			SystemHandle systemHandle = default(SystemHandle);
			checkedEntityDataAccess->RemoveMultipleComponentsDuringStructuralChange(entities, in componentTypeSet, in systemHandle);
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x0002D788 File Offset: 0x0002B988
		[EntityManager.StructuralChangeMethodAttribute]
		public unsafe void RemoveComponent(NativeArray<Entity> entities, ComponentType componentType)
		{
			if (entities.Length == 0)
			{
				return;
			}
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginStructuralChanges();
			NativeArray<Entity> nativeArray = entities;
			SystemHandle systemHandle = default(SystemHandle);
			checkedEntityDataAccess->RemoveComponentDuringStructuralChange(nativeArray, componentType, in systemHandle);
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x0002D7C4 File Offset: 0x0002B9C4
		[EntityManager.StructuralChangeMethodAttribute]
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public void AddComponent<T>(NativeArray<Entity> entities)
		{
			this.AddComponent(entities, ComponentType.ReadWrite<T>());
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x0002D7D4 File Offset: 0x0002B9D4
		[EntityManager.StructuralChangeMethodAttribute]
		public unsafe void AddComponent(SystemHandle system, in ComponentTypeSet componentTypeSet)
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess(system);
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginStructuralChanges();
			Entity entity = system.m_Entity;
			SystemHandle systemHandle = default(SystemHandle);
			checkedEntityDataAccess->AddMultipleComponentsDuringStructuralChange(entity, in componentTypeSet, in systemHandle);
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x0002D80C File Offset: 0x0002BA0C
		[EntityManager.StructuralChangeMethodAttribute]
		public unsafe void AddComponent(Entity entity, in ComponentTypeSet componentTypeSet)
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginStructuralChanges();
			SystemHandle systemHandle = default(SystemHandle);
			checkedEntityDataAccess->AddMultipleComponentsDuringStructuralChange(entity, in componentTypeSet, in systemHandle);
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x0002D83E File Offset: 0x0002BA3E
		[Obsolete("AddComponents() has been renamed to AddComponent(). AddComponents() will be removed in a future package release.", false)]
		public void AddComponents(Entity entity, in ComponentTypeSet componentTypeSet)
		{
			this.AddComponent(entity, in componentTypeSet);
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x0002D848 File Offset: 0x0002BA48
		[EntityManager.StructuralChangeMethodAttribute]
		public unsafe bool RemoveComponent(Entity entity, ComponentType componentType)
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginStructuralChanges();
			SystemHandle systemHandle = default(SystemHandle);
			bool flag = checkedEntityDataAccess->RemoveComponentDuringStructuralChange(entity, componentType, in systemHandle);
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
			return flag;
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x0002D87C File Offset: 0x0002BA7C
		[EntityManager.StructuralChangeMethodAttribute]
		public unsafe bool RemoveComponent(SystemHandle system, ComponentType componentType)
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginStructuralChanges();
			ref EntityDataAccess ptr = ref *checkedEntityDataAccess;
			Entity entity = system.m_Entity;
			SystemHandle systemHandle = default(SystemHandle);
			bool flag = ptr.RemoveComponentDuringStructuralChange(entity, componentType, in systemHandle);
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
			return flag;
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x0002D8B8 File Offset: 0x0002BAB8
		[EntityManager.StructuralChangeMethodAttribute]
		public unsafe void RemoveComponent(Entity entity, in ComponentTypeSet componentTypeSet)
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginStructuralChanges();
			SystemHandle systemHandle = default(SystemHandle);
			checkedEntityDataAccess->RemoveComponentDuringStructuralChange(entity, in componentTypeSet, in systemHandle);
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x0002D8EC File Offset: 0x0002BAEC
		[EntityManager.StructuralChangeMethodAttribute]
		public unsafe void RemoveComponent(EntityQuery entityQuery, ComponentType componentType)
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			EntityQueryImpl* ptr = entityQuery._GetImpl();
			if (ptr->IsEmptyIgnoreFilter)
			{
				return;
			}
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginStructuralChanges();
			ref EntityDataAccess ptr2 = ref *checkedEntityDataAccess;
			EntityQueryImpl* ptr3 = ptr;
			SystemHandle systemHandle = default(SystemHandle);
			ptr2.RemoveComponentFromQueryDuringStructuralChange(ptr3, componentType, in systemHandle);
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x0002D934 File Offset: 0x0002BB34
		[EntityManager.StructuralChangeMethodAttribute]
		public unsafe void RemoveComponent(EntityQuery entityQuery, in ComponentTypeSet componentTypeSet)
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			EntityQueryImpl* ptr = entityQuery._GetImpl();
			if (ptr->IsEmptyIgnoreFilter)
			{
				return;
			}
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginStructuralChanges();
			ref EntityDataAccess ptr2 = ref *checkedEntityDataAccess;
			EntityQueryImpl* ptr3 = ptr;
			SystemHandle systemHandle = default(SystemHandle);
			ptr2.RemoveMultipleComponentsFromQueryDuringStructuralChange(ptr3, in componentTypeSet, in systemHandle);
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x0002D979 File Offset: 0x0002BB79
		[EntityManager.StructuralChangeMethodAttribute]
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public bool RemoveComponent<T>(Entity entity)
		{
			return this.RemoveComponent(entity, ComponentType.ReadWrite<T>());
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x0002D987 File Offset: 0x0002BB87
		[EntityManager.StructuralChangeMethodAttribute]
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public bool RemoveComponent<T>(SystemHandle system)
		{
			return this.RemoveComponent(system.m_Entity, ComponentType.ReadWrite<T>());
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x0002D99A File Offset: 0x0002BB9A
		[EntityManager.StructuralChangeMethodAttribute]
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public void RemoveComponent<T>(EntityQuery entityQuery)
		{
			this.RemoveComponent(entityQuery, ComponentType.ReadWrite<T>());
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x0002D9A8 File Offset: 0x0002BBA8
		[EntityManager.StructuralChangeMethodAttribute]
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public void RemoveComponent<T>(NativeArray<Entity> entities)
		{
			this.RemoveComponent(entities, ComponentType.ReadWrite<T>());
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x0002D9B8 File Offset: 0x0002BBB8
		[EntityManager.StructuralChangeMethodAttribute]
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public bool AddComponentData<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity entity, T componentData) where T : struct, ValueType, IComponentData
		{
			ComponentType componentType = ComponentType.ReadWrite<T>();
			bool flag = this.AddComponent(entity, componentType);
			if (!componentType.IsZeroSized)
			{
				this.SetComponentData<T>(entity, componentData);
			}
			return flag;
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x0002D9E4 File Offset: 0x0002BBE4
		[EntityManager.StructuralChangeMethodAttribute]
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public bool AddComponentData<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(SystemHandle system, T componentData) where T : struct, ValueType, IComponentData
		{
			ComponentType componentType = ComponentType.ReadWrite<T>();
			bool flag = this.AddComponent(system.m_Entity, componentType);
			if (!componentType.IsZeroSized)
			{
				this.SetComponentData<T>(system.m_Entity, componentData);
			}
			return flag;
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x0002DA1A File Offset: 0x0002BC1A
		[EntityManager.StructuralChangeMethodAttribute]
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public bool RemoveChunkComponent<T>(Entity entity)
		{
			return this.RemoveComponent(entity, ComponentType.ChunkComponent<T>());
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x0002DA28 File Offset: 0x0002BC28
		[EntityManager.StructuralChangeMethodAttribute]
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public bool AddChunkComponentData<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity entity) where T : struct, ValueType, IComponentData
		{
			return this.AddComponent(entity, ComponentType.ChunkComponent<T>());
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x0002DA38 File Offset: 0x0002BC38
		[EntityManager.StructuralChangeMethodAttribute]
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe void AddChunkComponentData<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(EntityQuery entityQuery, T componentData) where T : struct, ValueType, IComponentData
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			EntityComponentStore* entityComponentStore = checkedEntityDataAccess->EntityComponentStore;
			if (entityQuery.IsEmptyIgnoreFilter)
			{
				return;
			}
			using (NativeArray<ArchetypeChunk> nativeArray = entityQuery.ToArchetypeChunkArray(Allocator.TempJob))
			{
				if (nativeArray.Length > 0)
				{
					ArchetypeChunk* unsafePtr = (ArchetypeChunk*)nativeArray.GetUnsafePtr<ArchetypeChunk>();
					TypeIndex typeIndex = ComponentType.ReadWrite<T>().TypeIndex;
					TypeIndex typeIndex2 = TypeManager.MakeChunkComponentTypeIndex(typeIndex);
					EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginStructuralChanges();
					StructuralChange.AddComponentChunks(entityComponentStore, unsafePtr, nativeArray.Length, typeIndex2);
					checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
					StructuralChange.SetChunkComponent(entityComponentStore, unsafePtr, nativeArray.Length, (void*)(&componentData), typeIndex);
				}
			}
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x0002DAE4 File Offset: 0x0002BCE4
		[EntityManager.StructuralChangeMethodAttribute]
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public void RemoveChunkComponentData<T>(EntityQuery entityQuery)
		{
			this.RemoveComponent(entityQuery, ComponentType.ChunkComponent<T>());
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x0002DAF2 File Offset: 0x0002BCF2
		[EntityManager.StructuralChangeMethodAttribute]
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleBufferElement) })]
		public DynamicBuffer<T> AddBuffer<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity entity) where T : struct, ValueType, IBufferElementData
		{
			this.AddComponent<T>(entity);
			return this.GetBuffer<T>(entity, false);
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x0002DB04 File Offset: 0x0002BD04
		[EntityManager.StructuralChangeMethodAttribute]
		[ExcludeFromBurstCompatTesting("Takes managed object")]
		public void AddComponentObject(Entity entity, object componentData)
		{
			ComponentType componentType = componentData.GetType();
			this.AddComponent(entity, componentType);
			this.SetComponentObject(entity, componentType, componentData);
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x0002DB30 File Offset: 0x0002BD30
		[EntityManager.StructuralChangeMethodAttribute]
		[ExcludeFromBurstCompatTesting("Takes managed object")]
		public void AddComponentObject(SystemHandle system, object componentData)
		{
			ComponentType componentType = componentData.GetType();
			this.AddComponent(system.m_Entity, componentType);
			this.SetComponentObject(system.m_Entity, componentType, componentData);
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x0001CFE4 File Offset: 0x0001B1E4
		[Obsolete("Use AddSharedComponentManaged (UnityUpgradable) -> AddSharedComponentManaged<T>(*)", true)]
		public bool AddSharedComponentData<T>(Entity entity, T componentData) where T : struct, ISharedComponentData
		{
			return false;
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x0002DB68 File Offset: 0x0002BD68
		[EntityManager.StructuralChangeMethodAttribute]
		[ExcludeFromBurstCompatTesting("Accesses managed component store")]
		public unsafe bool AddSharedComponentManaged<T>(Entity entity, T componentData) where T : struct, ISharedComponentData
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginStructuralChanges();
			ComponentType componentType = ComponentType.ReadWrite<T>();
			bool flag;
			if (componentType.TypeIndex.IsManagedSharedComponent)
			{
				ref EntityDataAccess ptr = ref *checkedEntityDataAccess;
				T t = componentData;
				SystemHandle systemHandle = default(SystemHandle);
				flag = ptr.AddSharedComponentDataDuringStructuralChange_Managed<T>(entity, t, in systemHandle);
			}
			else
			{
				T t2 = default(T);
				flag = checkedEntityDataAccess->AddSharedComponentDataDuringStructuralChange_Unmanaged(entity, componentType, UnsafeUtility.AddressOf<T>(ref componentData), UnsafeUtility.AddressOf<T>(ref t2));
			}
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
			return flag;
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x0002DBD4 File Offset: 0x0002BDD4
		[EntityManager.StructuralChangeMethodAttribute]
		[ExcludeFromBurstCompatTesting("Accesses managed component store")]
		public unsafe void AddSharedComponentManaged<T>(NativeArray<Entity> entities, T componentData) where T : struct, ISharedComponentData
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginStructuralChanges();
			ComponentType componentType = ComponentType.ReadWrite<T>();
			if (TypeManager.IsManagedSharedComponent(componentType.TypeIndex))
			{
				ref EntityDataAccess ptr = ref *checkedEntityDataAccess;
				T t = componentData;
				SystemHandle systemHandle = default(SystemHandle);
				ptr.AddSharedComponentDataDuringStructuralChange_Managed<T>(entities, t, in systemHandle);
			}
			else
			{
				T t2 = default(T);
				ref EntityDataAccess ptr2 = ref *checkedEntityDataAccess;
				ComponentType componentType2 = componentType;
				void* ptr3 = UnsafeUtility.AddressOf<T>(ref componentData);
				void* ptr4 = UnsafeUtility.AddressOf<T>(ref t2);
				SystemHandle systemHandle = default(SystemHandle);
				ptr2.AddSharedComponentDataDuringStructuralChange_Unmanaged(entities, componentType2, ptr3, ptr4, in systemHandle);
			}
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x0002DC44 File Offset: 0x0002BE44
		[EntityManager.StructuralChangeMethodAttribute]
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleSharedComponentData) })]
		public unsafe void AddSharedComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(EntityQuery entityQuery, T componentData) where T : struct, ValueType, ISharedComponentData
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			EntityQueryImpl* ptr = entityQuery._GetImpl();
			if (ptr->IsEmptyIgnoreFilter)
			{
				return;
			}
			ComponentType componentType = ComponentType.ReadWrite<T>();
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginStructuralChanges();
			int num = checkedEntityDataAccess->InsertSharedComponent_Unmanaged<T>(componentData);
			ref EntityDataAccess ptr2 = ref *checkedEntityDataAccess;
			EntityQueryImpl* ptr3 = ptr;
			int num2 = num;
			ComponentType componentType2 = componentType;
			void* ptr4 = UnsafeUtility.AddressOf<T>(ref componentData);
			SystemHandle systemHandle = default(SystemHandle);
			ptr2.AddSharedComponentDataToQueryDuringStructuralChange_Unmanaged(ptr3, num2, componentType2, ptr4, in systemHandle);
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x00002287 File Offset: 0x00000487
		[Obsolete("Use AddSharedComponentManaged (UnityUpgradable) -> AddSharedComponentManaged<T>(*)")]
		public void AddSharedComponentData<T>(EntityQuery entityQuery, T componentData) where T : struct, ISharedComponentData
		{
		}

		// Token: 0x060008FF RID: 2303 RVA: 0x0002DCA4 File Offset: 0x0002BEA4
		[EntityManager.StructuralChangeMethodAttribute]
		[ExcludeFromBurstCompatTesting("Accesses managed component store")]
		public unsafe void AddSharedComponentManaged<T>(EntityQuery entityQuery, T componentData) where T : struct, ISharedComponentData
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			EntityQueryImpl* ptr = entityQuery._GetImpl();
			if (ptr->IsEmptyIgnoreFilter)
			{
				return;
			}
			ComponentType componentType = ComponentType.ReadWrite<T>();
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginStructuralChanges();
			int num = checkedEntityDataAccess->InsertSharedComponent<T>(componentData);
			if (TypeManager.IsManagedSharedComponent(componentType.TypeIndex))
			{
				ref EntityDataAccess ptr2 = ref *checkedEntityDataAccess;
				EntityQueryImpl* ptr3 = ptr;
				int num2 = num;
				ComponentType componentType2 = componentType;
				SystemHandle systemHandle = default(SystemHandle);
				ptr2.AddSharedComponentDataToQueryDuringStructuralChange(ptr3, num2, componentType2, in systemHandle);
			}
			else
			{
				ref EntityDataAccess ptr4 = ref *checkedEntityDataAccess;
				EntityQueryImpl* ptr5 = ptr;
				int num3 = num;
				ComponentType componentType3 = componentType;
				void* ptr6 = UnsafeUtility.AddressOf<T>(ref componentData);
				SystemHandle systemHandle = default(SystemHandle);
				ptr4.AddSharedComponentDataToQueryDuringStructuralChange_Unmanaged(ptr5, num3, componentType3, ptr6, in systemHandle);
			}
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
		}

		// Token: 0x06000900 RID: 2304 RVA: 0x0002DD24 File Offset: 0x0002BF24
		[EntityManager.StructuralChangeMethodAttribute]
		public unsafe void SetArchetype(Entity entity, EntityArchetype archetype)
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			EntityComponentStore* entityComponentStore = checkedEntityDataAccess->EntityComponentStore;
			Archetype* archetype2 = entityComponentStore->GetArchetype(entity);
			Archetype* archetype3 = archetype.Archetype;
			if (archetype2 == archetype3)
			{
				return;
			}
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginStructuralChanges();
			StructuralChange.MoveEntityArchetype(entityComponentStore, &entity, (void*)archetype.Archetype);
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
		}

		// Token: 0x06000901 RID: 2305 RVA: 0x0002DD70 File Offset: 0x0002BF70
		[EntityManager.StructuralChangeMethodAttribute]
		public unsafe void SetEnabled(Entity entity, bool enabled)
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginStructuralChanges();
			checkedEntityDataAccess->SetEnabled(entity, enabled);
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x0002DD99 File Offset: 0x0002BF99
		public unsafe bool IsEnabled(Entity entity)
		{
			return this.GetCheckedEntityDataAccess()->IsEnabled(entity);
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x0002DDA7 File Offset: 0x0002BFA7
		[Obsolete("This function has been renamed to IsEnabled().  (RemovedAfter Entities 1.0) (UnityUpgradable) -> IsEnabled(*)", false)]
		public bool GetEnabled(Entity entity)
		{
			return this.IsEnabled(entity);
		}

		// Token: 0x06000904 RID: 2308 RVA: 0x0002DDB0 File Offset: 0x0002BFB0
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleEnableableComponent) })]
		public unsafe bool IsComponentEnabled<T>(Entity entity) where T : IEnableableComponent
		{
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			if (!checkedEntityDataAccess->IsInExclusiveTransaction)
			{
				checkedEntityDataAccess->DependencyManager->CompleteWriteDependency(typeIndex);
			}
			return checkedEntityDataAccess->IsComponentEnabled(entity, typeIndex);
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x0002DDE6 File Offset: 0x0002BFE6
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleEnableableComponent) })]
		public bool IsComponentEnabled<T>(SystemHandle systemHandle) where T : IEnableableComponent
		{
			return this.IsComponentEnabled<T>(systemHandle.m_Entity);
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x0002DDF4 File Offset: 0x0002BFF4
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleEnableableComponent) })]
		public void SetComponentEnabled<T>(SystemHandle systemHandle, bool value) where T : IEnableableComponent
		{
			this.SetComponentEnabled<T>(systemHandle.m_Entity, value);
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x0002DE04 File Offset: 0x0002C004
		public unsafe bool IsComponentEnabled(Entity entity, ComponentType componentType)
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			if (!checkedEntityDataAccess->IsInExclusiveTransaction)
			{
				checkedEntityDataAccess->DependencyManager->CompleteWriteDependency(componentType.TypeIndex);
			}
			return checkedEntityDataAccess->IsComponentEnabled(entity, componentType.TypeIndex);
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x0002DE40 File Offset: 0x0002C040
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleEnableableComponent) })]
		public unsafe void SetComponentEnabled<T>(Entity entity, bool value) where T : IEnableableComponent
		{
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			if (!checkedEntityDataAccess->IsInExclusiveTransaction)
			{
				checkedEntityDataAccess->DependencyManager->CompleteReadAndWriteDependency(typeIndex);
			}
			checkedEntityDataAccess->SetComponentEnabled(entity, typeIndex, value);
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x0002DE78 File Offset: 0x0002C078
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleEnableableComponent) })]
		public unsafe void SetComponentEnabled<T>(EntityQuery query, bool value) where T : IEnableableComponent
		{
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			query._GetImpl()->SetEnabledBitsOnAllChunks(typeIndex, value);
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x0002DE99 File Offset: 0x0002C099
		public unsafe void SetComponentEnabled(EntityQuery query, ComponentType componentType, bool value)
		{
			query._GetImpl()->SetEnabledBitsOnAllChunks(componentType.TypeIndex, value);
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x0002DEB0 File Offset: 0x0002C0B0
		public unsafe void SetComponentEnabled(Entity entity, ComponentType componentType, bool value)
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			if (!checkedEntityDataAccess->IsInExclusiveTransaction)
			{
				checkedEntityDataAccess->DependencyManager->CompleteReadAndWriteDependency(componentType.TypeIndex);
			}
			checkedEntityDataAccess->SetComponentEnabled(entity, componentType.TypeIndex, value);
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x0002DEEC File Offset: 0x0002C0EC
		[EntityManager.StructuralChangeMethodAttribute]
		public unsafe Entity CreateEntity(EntityArchetype archetype)
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			checkedEntityDataAccess->PrepareForAdditiveStructuralChanges(archetype.Archetype);
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginAdditiveStructuralChanges();
			SystemHandle systemHandle = default(SystemHandle);
			Entity entity = checkedEntityDataAccess->CreateEntityDuringStructuralChange(archetype, in systemHandle);
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
			return entity;
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x0002DF2B File Offset: 0x0002C12B
		[EntityManager.StructuralChangeMethodAttribute]
		[ExcludeFromBurstCompatTesting("Takes managed array")]
		public Entity CreateEntity(params ComponentType[] types)
		{
			return this.CreateEntity(this.CreateArchetype(types));
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x0002DF3A File Offset: 0x0002C13A
		[EntityManager.StructuralChangeMethodAttribute]
		public Entity CreateEntity(ReadOnlySpan<ComponentType> types)
		{
			return this.CreateEntity(this.CreateArchetype(types));
		}

		// Token: 0x0600090F RID: 2319 RVA: 0x0002DF4C File Offset: 0x0002C14C
		[EntityManager.StructuralChangeMethodAttribute]
		public unsafe Entity CreateEntity()
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			checkedEntityDataAccess->PrepareForAdditiveStructuralChanges();
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginAdditiveStructuralChanges();
			SystemHandle systemHandle = default(SystemHandle);
			Entity entity = checkedEntityDataAccess->CreateEntity(in systemHandle);
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
			return entity;
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x0002DF84 File Offset: 0x0002C184
		[EntityManager.StructuralChangeMethodAttribute]
		public unsafe void CreateEntity(EntityArchetype archetype, NativeArray<Entity> entities)
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			checkedEntityDataAccess->PrepareForAdditiveStructuralChanges(archetype.Archetype);
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginAdditiveStructuralChanges();
			SystemHandle systemHandle = default(SystemHandle);
			checkedEntityDataAccess->CreateEntityDuringStructuralChange(archetype, entities, in systemHandle);
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x0002DFC4 File Offset: 0x0002C1C4
		[EntityManager.StructuralChangeMethodAttribute]
		public unsafe NativeArray<Entity> CreateEntity(EntityArchetype archetype, int entityCount, AllocatorManager.AllocatorHandle allocator)
		{
			NativeArray<Entity> nativeArray = CollectionHelper.CreateNativeArray<Entity>(entityCount, allocator, NativeArrayOptions.ClearMemory);
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			checkedEntityDataAccess->PrepareForAdditiveStructuralChanges(archetype.Archetype);
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginAdditiveStructuralChanges();
			NativeArray<Entity> nativeArray2 = nativeArray;
			SystemHandle systemHandle = default(SystemHandle);
			checkedEntityDataAccess->CreateEntityDuringStructuralChange(archetype, nativeArray2, in systemHandle);
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
			return nativeArray;
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x0002E00C File Offset: 0x0002C20C
		[EntityManager.StructuralChangeMethodAttribute]
		public unsafe void CreateEntity(EntityArchetype archetype, int entityCount)
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			checkedEntityDataAccess->PrepareForAdditiveStructuralChanges(archetype.Archetype);
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginAdditiveStructuralChanges();
			Entity* ptr = null;
			SystemHandle systemHandle = default(SystemHandle);
			checkedEntityDataAccess->CreateEntityDuringStructuralChange(archetype, ptr, entityCount, in systemHandle);
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x0002E04C File Offset: 0x0002C24C
		[EntityManager.StructuralChangeMethodAttribute]
		public unsafe void DestroyEntity(EntityQuery entityQuery)
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			EntityQueryImpl* ptr = entityQuery._GetImpl();
			if (ptr->IsEmptyIgnoreFilter)
			{
				return;
			}
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginStructuralChanges();
			ref EntityDataAccess ptr2 = ref *checkedEntityDataAccess;
			EntityQueryImpl* ptr3 = ptr;
			SystemHandle systemHandle = default(SystemHandle);
			ptr2.DestroyEntitiesInQueryDuringStructuralChange(ptr3, in systemHandle);
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x0002E090 File Offset: 0x0002C290
		public void DestroyAndResetAllEntities()
		{
			this.DestroyEntity(this.UniversalQueryWithSystems);
			if (this.Debug.EntityCount != 0)
			{
				throw new ArgumentException("Destroying all entities failed. Some entities couldn't be deleted.");
			}
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x0002E0C4 File Offset: 0x0002C2C4
		[EntityManager.StructuralChangeMethodAttribute]
		public unsafe void DestroyEntity(NativeArray<Entity> entities)
		{
			this.DestroyEntityInternal((Entity*)entities.GetUnsafeReadOnlyPtr<Entity>(), entities.Length);
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x0002E0D9 File Offset: 0x0002C2D9
		[EntityManager.StructuralChangeMethodAttribute]
		public unsafe void DestroyEntity(NativeSlice<Entity> entities)
		{
			this.DestroyEntityInternal((Entity*)entities.GetUnsafeReadOnlyPtr<Entity>(), entities.Length);
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x0002E0EE File Offset: 0x0002C2EE
		[EntityManager.StructuralChangeMethodAttribute]
		public unsafe void DestroyEntity(Entity entity)
		{
			this.DestroyEntityInternal(&entity, 1);
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x0002E0FC File Offset: 0x0002C2FC
		[EntityManager.StructuralChangeMethodAttribute]
		public unsafe Entity Instantiate(Entity srcEntity)
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			Archetype* archetype = checkedEntityDataAccess->EntityComponentStore->GetArchetype(srcEntity);
			checkedEntityDataAccess->PrepareForAdditiveStructuralChanges(archetype->InstantiateArchetype);
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginAdditiveStructuralChanges();
			Entity entity;
			Entity* ptr = &entity;
			int num = 1;
			SystemHandle systemHandle = default(SystemHandle);
			checkedEntityDataAccess->InstantiateInternalDuringStructuralChange(srcEntity, ptr, num, in systemHandle);
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
			return entity;
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x0002E14C File Offset: 0x0002C34C
		[EntityManager.StructuralChangeMethodAttribute]
		public unsafe void Instantiate(Entity srcEntity, NativeArray<Entity> outputEntities)
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			Archetype* archetype = checkedEntityDataAccess->EntityComponentStore->GetArchetype(srcEntity);
			checkedEntityDataAccess->PrepareForAdditiveStructuralChanges(archetype->InstantiateArchetype);
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginAdditiveStructuralChanges();
			Entity* unsafePtr = (Entity*)outputEntities.GetUnsafePtr<Entity>();
			int length = outputEntities.Length;
			SystemHandle systemHandle = default(SystemHandle);
			checkedEntityDataAccess->InstantiateInternalDuringStructuralChange(srcEntity, unsafePtr, length, in systemHandle);
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
		}

		// Token: 0x0600091A RID: 2330 RVA: 0x0002E1A4 File Offset: 0x0002C3A4
		[EntityManager.StructuralChangeMethodAttribute]
		public unsafe NativeArray<Entity> Instantiate(Entity srcEntity, int instanceCount, AllocatorManager.AllocatorHandle allocator)
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			NativeArray<Entity> nativeArray = CollectionHelper.CreateNativeArray<Entity>(instanceCount, allocator, NativeArrayOptions.ClearMemory);
			Archetype* archetype = checkedEntityDataAccess->EntityComponentStore->GetArchetype(srcEntity);
			checkedEntityDataAccess->PrepareForAdditiveStructuralChanges(archetype->InstantiateArchetype);
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginAdditiveStructuralChanges();
			Entity* unsafePtr = (Entity*)nativeArray.GetUnsafePtr<Entity>();
			SystemHandle systemHandle = default(SystemHandle);
			checkedEntityDataAccess->InstantiateInternalDuringStructuralChange(srcEntity, unsafePtr, instanceCount, in systemHandle);
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
			return nativeArray;
		}

		// Token: 0x0600091B RID: 2331 RVA: 0x0002E200 File Offset: 0x0002C400
		[EntityManager.StructuralChangeMethodAttribute]
		[Obsolete("This method will be removed in a future Entities release. If you wish to clone the full hierarchy of every entity in a given array, simply loop through the array to and invoke `EntityManager.Instantiate(Entity entity)` on each entity.")]
		public unsafe void Instantiate(NativeArray<Entity> srcEntities, NativeArray<Entity> outputEntities)
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			checkedEntityDataAccess->PrepareForInstantiateAdditiveStructuralChanges(srcEntities);
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginAdditiveStructuralChanges();
			Entity* unsafeReadOnlyPtr = (Entity*)srcEntities.GetUnsafeReadOnlyPtr<Entity>();
			Entity* unsafePtr = (Entity*)outputEntities.GetUnsafePtr<Entity>();
			int length = srcEntities.Length;
			int length2 = outputEntities.Length;
			bool flag = true;
			SystemHandle systemHandle = default(SystemHandle);
			checkedEntityDataAccess->InstantiateInternalDuringStructuralChange(unsafeReadOnlyPtr, unsafePtr, length, length2, flag, in systemHandle);
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
		}

		// Token: 0x0600091C RID: 2332 RVA: 0x0002E252 File Offset: 0x0002C452
		[EntityManager.StructuralChangeMethodAttribute]
		[ExcludeFromCoverage]
		[Obsolete("This method is not safe to use in some contexts, and will be removed from the public API in a future Entities release. To create copies of a prefab Entity, use EntityManager.Instantiate().")]
		public void CopyEntities(NativeArray<Entity> srcEntities, NativeArray<Entity> outputEntities)
		{
			this.CopyEntitiesInternal(srcEntities, outputEntities);
		}

		// Token: 0x0600091D RID: 2333 RVA: 0x0002E25C File Offset: 0x0002C45C
		internal unsafe void CopyEntitiesInternal(NativeArray<Entity> srcEntities, NativeArray<Entity> outputEntities)
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			checkedEntityDataAccess->PrepareForCopyAdditiveStructuralChanges(srcEntities);
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginAdditiveStructuralChanges();
			Entity* unsafeReadOnlyPtr = (Entity*)srcEntities.GetUnsafeReadOnlyPtr<Entity>();
			Entity* unsafePtr = (Entity*)outputEntities.GetUnsafePtr<Entity>();
			int length = srcEntities.Length;
			int length2 = outputEntities.Length;
			bool flag = false;
			SystemHandle systemHandle = default(SystemHandle);
			checkedEntityDataAccess->InstantiateInternalDuringStructuralChange(unsafeReadOnlyPtr, unsafePtr, length, length2, flag, in systemHandle);
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x0002E2AE File Offset: 0x0002C4AE
		[Obsolete("Consider using a more idiomatic ECS way to detect creation and destruction, like (enableable) tags and cleanup components.")]
		public unsafe JobHandle GetCreatedAndDestroyedEntitiesAsync(NativeList<int> state, NativeList<Entity> createdEntities, NativeList<Entity> destroyedEntities)
		{
			return this.GetCheckedEntityDataAccess()->GetCreatedAndDestroyedEntitiesAsync(state, createdEntities, destroyedEntities);
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x0002E2BE File Offset: 0x0002C4BE
		[Obsolete("Consider using a more idiomatic ECS way to detect creation and destruction, like (enableable) tags and cleanup components.")]
		public unsafe void GetCreatedAndDestroyedEntities(NativeList<int> state, NativeList<Entity> createdEntities, NativeList<Entity> destroyedEntities)
		{
			this.GetCheckedEntityDataAccess()->GetCreatedAndDestroyedEntities(state, createdEntities, destroyedEntities);
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x0002E2D0 File Offset: 0x0002C4D0
		[ExcludeFromBurstCompatTesting("Takes managed array")]
		public unsafe EntityArchetype CreateArchetype(params ComponentType[] types)
		{
			if (types == null)
			{
				throw new NullReferenceException("types");
			}
			ComponentType* ptr;
			if (types == null || types.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &types[0];
			}
			return this.CreateArchetype(ptr, types.Length);
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x0002E310 File Offset: 0x0002C510
		public unsafe EntityArchetype CreateArchetype(ReadOnlySpan<ComponentType> types)
		{
			fixed (ComponentType* pinnableReference = types.GetPinnableReference())
			{
				ComponentType* ptr = pinnableReference;
				return this.CreateArchetype(ptr, types.Length);
			}
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x0002E336 File Offset: 0x0002C536
		public unsafe EntityArchetype CreateArchetype(NativeArray<ComponentType> types)
		{
			return this.CreateArchetype((ComponentType*)types.GetUnsafeReadOnlyPtr<ComponentType>(), types.Length);
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x0002E34C File Offset: 0x0002C54C
		internal unsafe EntityArchetype CreateArchetype(ComponentType* types, int count)
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			checkedEntityDataAccess->PrepareForAdditiveStructuralChanges();
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginAdditiveStructuralChanges();
			EntityArchetype entityArchetype = checkedEntityDataAccess->CreateArchetype(types, count, true);
			checkedEntityDataAccess->CheckIsAdditiveArchetypeStructuralChangePossible(entityArchetype.Archetype);
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
			return entityArchetype;
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x0002E38A File Offset: 0x0002C58A
		internal unsafe EntityArchetype CreateArchetypeWithoutSimulateComponent(NativeArray<ComponentType> types)
		{
			return this.CreateArchetypeWithoutSimulateComponent((ComponentType*)types.GetUnsafeReadOnlyPtr<ComponentType>(), types.Length);
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x0002E3A0 File Offset: 0x0002C5A0
		internal unsafe EntityArchetype CreateArchetypeWithoutSimulateComponent(ComponentType* types, int count)
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			checkedEntityDataAccess->PrepareForAdditiveStructuralChanges();
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginAdditiveStructuralChanges();
			EntityArchetype entityArchetype = checkedEntityDataAccess->CreateArchetype(types, count, false);
			checkedEntityDataAccess->CheckIsAdditiveArchetypeStructuralChangePossible(entityArchetype.Archetype);
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
			return entityArchetype;
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x0002E3E0 File Offset: 0x0002C5E0
		[ExcludeFromBurstCompatTesting("Accesses managed component store")]
		public void CopyEntitiesFrom(EntityManager srcEntityManager, NativeArray<Entity> srcEntities, NativeArray<Entity> outputEntities = default(NativeArray<Entity>))
		{
			using (NativeArray<Entity> nativeArray = new NativeArray<Entity>(srcEntities.Length, Allocator.Temp, NativeArrayOptions.ClearMemory))
			{
				srcEntityManager.CopyEntitiesInternal(srcEntities, nativeArray);
				srcEntityManager.AddComponent(nativeArray, ComponentType.ReadWrite<EntityManager.IsolateCopiedEntities>());
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp).WithAll<EntityManager.IsolateCopiedEntities>().WithOptions(EntityQueryOptions.IncludePrefab | EntityQueryOptions.IncludeDisabledEntities);
				EntityQuery entityQuery = entityQueryBuilder.Build(srcEntityManager);
				EntityQuery entityQuery2 = entityQueryBuilder.Build(this);
				using (NativeArray<EntityRemapUtility.EntityRemapInfo> nativeArray2 = srcEntityManager.CreateEntityRemapArray(Allocator.TempJob))
				{
					this.MoveEntitiesFromInternalQuery(srcEntityManager, entityQuery, nativeArray2);
					if (outputEntities.IsCreated)
					{
						int num = 0;
						int length = nativeArray.Length;
						while (num != length)
						{
							outputEntities[num] = nativeArray2[nativeArray[num].Index].Target;
							num++;
						}
					}
					this.RemoveComponent(entityQuery2, ComponentType.ReadWrite<EntityManager.IsolateCopiedEntities>());
				}
			}
		}

		// Token: 0x06000927 RID: 2343 RVA: 0x0002E4F0 File Offset: 0x0002C6F0
		[Obsolete("This function only works in a narrow set of circumstances and has semantics that differ from a regular copy.", false)]
		[ExcludeFromBurstCompatTesting("Accesses managed component store")]
		public unsafe void CopyAndReplaceEntitiesFrom(EntityManager srcEntityManager, NativeArray<EntityRemapUtility.EntityRemapInfo> remap = default(NativeArray<EntityRemapUtility.EntityRemapInfo>))
		{
			srcEntityManager.CompleteAllTrackedJobs();
			this.CompleteAllTrackedJobs();
			ref EntityDataAccess checkedEntityDataAccess = ref *srcEntityManager.GetCheckedEntityDataAccess();
			EntityDataAccess* checkedEntityDataAccess2 = this.GetCheckedEntityDataAccess();
			JobHandle jobHandle;
			using (NativeList<ArchetypeChunk> nativeList = checkedEntityDataAccess.m_UniversalQueryWithChunksAndSystems.ToArchetypeChunkListAsync(Allocator.TempJob, out jobHandle))
			{
				JobHandle jobHandle2;
				using (NativeList<ArchetypeChunk> nativeList2 = checkedEntityDataAccess2->m_UniversalQueryWithChunksAndSystems.ToArchetypeChunkListAsync(Allocator.TempJob, out jobHandle2))
				{
					jobHandle.Complete();
					jobHandle2.Complete();
					using (EntityDiffer.ArchetypeChunkChanges archetypeChunkChanges = new EntityDiffer.ArchetypeChunkChanges(Allocator.TempJob))
					{
						EntityDiffer.ArchetypeChunkChangeSet createdSrcChunks = archetypeChunkChanges.CreatedSrcChunks;
						EntityDiffer.ArchetypeChunkChangeSet destroyedDstChunks = archetypeChunkChanges.DestroyedDstChunks;
						NativeList<ArchetypeChunk> chunks = createdSrcChunks.Chunks;
						NativeList<EntityDiffer.ArchetypeChunkChangeFlags> flags = createdSrcChunks.Flags;
						NativeList<int> entityCounts = createdSrcChunks.EntityCounts;
						int i = 0;
						int length = nativeList.Length;
						int num = 0;
						while (i < length)
						{
							ArchetypeChunk archetypeChunk = nativeList[i];
							chunks.Add(in archetypeChunk);
							EntityDiffer.ArchetypeChunkChangeFlags archetypeChunkChangeFlags = EntityDiffer.ArchetypeChunkChangeFlags.None;
							flags.Add(in archetypeChunkChangeFlags);
							entityCounts.Add(in num);
							int num2 = num;
							archetypeChunk = nativeList[i];
							num = num2 + archetypeChunk.Count;
							i++;
						}
						NativeList<ArchetypeChunk> chunks2 = destroyedDstChunks.Chunks;
						NativeList<EntityDiffer.ArchetypeChunkChangeFlags> flags2 = destroyedDstChunks.Flags;
						NativeList<int> entityCounts2 = destroyedDstChunks.EntityCounts;
						int j = 0;
						int length2 = nativeList2.Length;
						int num3 = 0;
						while (j < length2)
						{
							ArchetypeChunk archetypeChunk = nativeList2[j];
							chunks2.Add(in archetypeChunk);
							EntityDiffer.ArchetypeChunkChangeFlags archetypeChunkChangeFlags = EntityDiffer.ArchetypeChunkChangeFlags.None;
							flags2.Add(in archetypeChunkChangeFlags);
							entityCounts2.Add(in num3);
							int num4 = num3;
							archetypeChunk = nativeList2[j];
							num3 = num4 + archetypeChunk.Count;
							j++;
						}
						EntityDiffer.CopyAndReplaceChunks(srcEntityManager, this, checkedEntityDataAccess2->m_UniversalQueryWithChunksAndSystems, archetypeChunkChanges, remap);
					}
				}
			}
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x0002E6E4 File Offset: 0x0002C8E4
		[ExcludeFromBurstCompatTesting("Accesses managed component store")]
		public void MoveEntitiesFrom(EntityManager srcEntities)
		{
			using (EntityManager.k_ProfileMoveEntitiesFrom.Auto())
			{
				using (NativeArray<EntityRemapUtility.EntityRemapInfo> nativeArray = srcEntities.CreateEntityRemapArray(Allocator.TempJob))
				{
					this.MoveEntitiesFromInternalAll(srcEntities, nativeArray);
				}
			}
		}

		// Token: 0x06000929 RID: 2345 RVA: 0x0002E750 File Offset: 0x0002C950
		[ExcludeFromBurstCompatTesting("Accesses managed component store")]
		public void MoveEntitiesFrom(out NativeArray<Entity> output, EntityManager srcEntities)
		{
			using (EntityManager.k_ProfileMoveEntitiesFrom.Auto())
			{
				using (NativeArray<EntityRemapUtility.EntityRemapInfo> nativeArray = srcEntities.CreateEntityRemapArray(Allocator.TempJob))
				{
					this.MoveEntitiesFromInternalAll(srcEntities, nativeArray);
					EntityRemapUtility.GetTargets(out output, nativeArray);
				}
			}
		}

		// Token: 0x0600092A RID: 2346 RVA: 0x0002E7C4 File Offset: 0x0002C9C4
		[ExcludeFromBurstCompatTesting("Accesses managed component store")]
		public void MoveEntitiesFrom(out NativeArray<Entity> output, EntityManager srcEntities, NativeArray<EntityRemapUtility.EntityRemapInfo> entityRemapping)
		{
			using (EntityManager.k_ProfileMoveEntitiesFrom.Auto())
			{
				this.MoveEntitiesFromInternalAll(srcEntities, entityRemapping);
				EntityRemapUtility.GetTargets(out output, entityRemapping);
			}
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x0002E810 File Offset: 0x0002CA10
		[ExcludeFromBurstCompatTesting("Accesses managed component store")]
		public void MoveEntitiesFrom(EntityManager srcEntities, NativeArray<EntityRemapUtility.EntityRemapInfo> entityRemapping)
		{
			using (EntityManager.k_ProfileMoveEntitiesFrom.Auto())
			{
				this.MoveEntitiesFromInternalAll(srcEntities, entityRemapping);
			}
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x0002E854 File Offset: 0x0002CA54
		[ExcludeFromBurstCompatTesting("Accesses managed component store")]
		public void MoveEntitiesFrom(EntityManager srcEntities, EntityQuery filter)
		{
			using (EntityManager.k_ProfileMoveEntitiesFrom.Auto())
			{
				using (NativeArray<EntityRemapUtility.EntityRemapInfo> nativeArray = srcEntities.CreateEntityRemapArray(Allocator.TempJob))
				{
					this.MoveEntitiesFromInternalQuery(srcEntities, filter, nativeArray);
				}
			}
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x0002E8C0 File Offset: 0x0002CAC0
		[ExcludeFromBurstCompatTesting("Accesses managed component store")]
		public void MoveEntitiesFrom(out NativeArray<Entity> output, EntityManager srcEntities, EntityQuery filter, NativeArray<EntityRemapUtility.EntityRemapInfo> entityRemapping)
		{
			using (EntityManager.k_ProfileMoveEntitiesFrom.Auto())
			{
				this.MoveEntitiesFromInternalQuery(srcEntities, filter, entityRemapping);
				EntityRemapUtility.GetTargets(out output, entityRemapping);
			}
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x0002E910 File Offset: 0x0002CB10
		[ExcludeFromBurstCompatTesting("Accesses managed component store")]
		public void MoveEntitiesFrom(EntityManager srcEntities, EntityQuery filter, NativeArray<EntityRemapUtility.EntityRemapInfo> entityRemapping)
		{
			using (EntityManager.k_ProfileMoveEntitiesFrom.Auto())
			{
				this.MoveEntitiesFromInternalQuery(srcEntities, filter, entityRemapping);
			}
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x0002E954 File Offset: 0x0002CB54
		[ExcludeFromBurstCompatTesting("Accesses managed component store")]
		public void MoveEntitiesFrom(out NativeArray<Entity> output, EntityManager srcEntities, EntityQuery filter)
		{
			using (EntityManager.k_ProfileMoveEntitiesFrom.Auto())
			{
				using (NativeArray<EntityRemapUtility.EntityRemapInfo> nativeArray = srcEntities.CreateEntityRemapArray(Allocator.TempJob))
				{
					this.MoveEntitiesFromInternalQuery(srcEntities, filter, nativeArray);
					EntityRemapUtility.GetTargets(out output, nativeArray);
				}
			}
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x0002E9C8 File Offset: 0x0002CBC8
		public NativeArray<EntityRemapUtility.EntityRemapInfo> CreateEntityRemapArray(AllocatorManager.AllocatorHandle allocator)
		{
			return CollectionHelper.CreateNativeArray<EntityRemapUtility.EntityRemapInfo>(this.HighestEntityIndex() + 1, allocator, NativeArrayOptions.ClearMemory);
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x0002E9D9 File Offset: 0x0002CBD9
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe int GetComponentOrderVersion<T>()
		{
			return this.GetCheckedEntityDataAccess()->EntityComponentStore->GetComponentTypeOrderVersion(TypeManager.GetTypeIndex<T>());
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x0002E9F0 File Offset: 0x0002CBF0
		public unsafe int GetComponentOrderVersion(ComponentType componentType)
		{
			return this.GetCheckedEntityDataAccess()->EntityComponentStore->GetComponentTypeOrderVersion(componentType.TypeIndex);
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x0002EA08 File Offset: 0x0002CC08
		[ExcludeFromBurstCompatTesting("Accesses managed component store")]
		public unsafe void PrepareForDeserialize()
		{
			if (this.Debug.EntityCount != 0)
			{
				using (NativeArray<Entity> allEntities = this.GetAllEntities(Allocator.Temp))
				{
					throw new ArgumentException(string.Format("PrepareForDeserialize requires the world to be completely empty, but there are {0}.\nFor example: {1}", allEntities.Length, this.Debug.GetEntityInfo(allEntities[0])));
				}
			}
			this.GetCheckedEntityDataAccess()->ManagedComponentStore.PrepareForDeserialize();
		}

		// Token: 0x06000934 RID: 2356 RVA: 0x0002EA90 File Offset: 0x0002CC90
		public unsafe void CompleteAllTrackedJobs()
		{
			this.GetCheckedEntityDataAccess()->CompleteAllTrackedJobs();
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x0002EA90 File Offset: 0x0002CC90
		[Obsolete("CompleteAllJobs() has been deprecated. Use CompleteAllTrackedJobs() instead (RemovedAfter 2023-04-08) (UnityUpgradable) -> CompleteAllTrackedJobs()", true)]
		public unsafe void CompleteAllJobs()
		{
			this.GetCheckedEntityDataAccess()->CompleteAllTrackedJobs();
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x0002EA9D File Offset: 0x0002CC9D
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public ComponentTypeHandle<T> GetComponentTypeHandle<T>(bool isReadOnly)
		{
			return new ComponentTypeHandle<T>(isReadOnly, this.GlobalSystemVersion);
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x0002EAAB File Offset: 0x0002CCAB
		public DynamicComponentTypeHandle GetDynamicComponentTypeHandle(ComponentType componentType)
		{
			return new DynamicComponentTypeHandle(componentType, this.GlobalSystemVersion);
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x0002EAB9 File Offset: 0x0002CCB9
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleBufferElement) })]
		public unsafe BufferTypeHandle<T> GetBufferTypeHandle<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(bool isReadOnly) where T : struct, ValueType, IBufferElementData
		{
			return this.GetCheckedEntityDataAccess()->GetBufferTypeHandle<T>(isReadOnly);
		}

		// Token: 0x06000939 RID: 2361 RVA: 0x0002EAC7 File Offset: 0x0002CCC7
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleSharedComponentData) })]
		public SharedComponentTypeHandle<T> GetSharedComponentTypeHandle<T>() where T : struct, ISharedComponentData
		{
			return new SharedComponentTypeHandle<T>(false);
		}

		// Token: 0x0600093A RID: 2362 RVA: 0x0002EACF File Offset: 0x0002CCCF
		public DynamicSharedComponentTypeHandle GetDynamicSharedComponentTypeHandle(ComponentType componentType)
		{
			return new DynamicSharedComponentTypeHandle(componentType);
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x0002EAD7 File Offset: 0x0002CCD7
		public EntityTypeHandle GetEntityTypeHandle()
		{
			return new EntityTypeHandle(false);
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x0002EADF File Offset: 0x0002CCDF
		public NativeArray<ComponentType> GetComponentTypes(Entity entity, Allocator allocator = Allocator.Temp)
		{
			return this.GetComponentTypes(entity, allocator);
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x0002EAF0 File Offset: 0x0002CCF0
		public unsafe NativeArray<ComponentType> GetComponentTypes(Entity entity, AllocatorManager.AllocatorHandle allocator)
		{
			EntityComponentStore* entityComponentStore = this.GetCheckedEntityDataAccess()->EntityComponentStore;
			return new EntityArchetype
			{
				Archetype = entityComponentStore->GetArchetype(entity)
			}.GetComponentTypes(allocator);
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x0002EB29 File Offset: 0x0002CD29
		public NativeArray<ComponentType> GetComponentTypes(SystemHandle system, Allocator allocator = Allocator.Temp)
		{
			return this.GetComponentTypes(system, allocator);
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x0002EB38 File Offset: 0x0002CD38
		public unsafe NativeArray<ComponentType> GetComponentTypes(SystemHandle system, AllocatorManager.AllocatorHandle allocator)
		{
			EntityComponentStore* entityComponentStore = this.GetCheckedEntityDataAccess()->EntityComponentStore;
			return new EntityArchetype
			{
				Archetype = entityComponentStore->GetArchetype(system.m_Entity)
			}.GetComponentTypes(allocator);
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x0002EB78 File Offset: 0x0002CD78
		[ExcludeFromBurstCompatTesting("Takes managed Type")]
		public List<Type> GetAssignableComponentTypes(Type interfaceType, List<Type> listOut)
		{
			int typeCount = TypeManager.GetTypeCount();
			for (int i = 0; i < typeCount; i++)
			{
				Type type = TypeManager.GetType(new TypeIndex
				{
					Value = i
				});
				if (interfaceType.IsAssignableFrom(type))
				{
					listOut.Add(type);
				}
			}
			return listOut;
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x0002EBBF File Offset: 0x0002CDBF
		[ExcludeFromBurstCompatTesting("Takes managed Type")]
		public List<Type> GetAssignableComponentTypes(Type interfaceType)
		{
			return this.GetAssignableComponentTypes(interfaceType, new List<Type>());
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x0002EBD0 File Offset: 0x0002CDD0
		[ExcludeFromBurstCompatTesting("Takes managed array")]
		public unsafe EntityQuery CreateEntityQuery(params ComponentType[] requiredComponents)
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			ComponentType* ptr;
			if (requiredComponents == null || requiredComponents.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &requiredComponents[0];
			}
			return checkedEntityDataAccess->EntityQueryManager->CreateEntityQuery(checkedEntityDataAccess, ptr, requiredComponents.Length);
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x0002EC0C File Offset: 0x0002CE0C
		[ExcludeFromBurstCompatTesting("Takes managed array")]
		public EntityQuery CreateEntityQuery(params EntityQueryDesc[] queriesDesc)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryManager.ConvertToEntityQueryBuilder(ref entityQueryBuilder, queriesDesc);
			EntityQuery entityQuery = this.CreateEntityQuery(in entityQueryBuilder);
			entityQueryBuilder.Dispose();
			return entityQuery;
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x0002EC3D File Offset: 0x0002CE3D
		public EntityQuery CreateEntityQuery(in EntityQueryBuilder queriesDesc)
		{
			return this.CreateEntityQueryUnowned(in queriesDesc);
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x0002EC48 File Offset: 0x0002CE48
		internal unsafe EntityQuery CreateEntityQueryUnowned(in EntityQueryBuilder queriesDesc)
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			return checkedEntityDataAccess->EntityQueryManager->CreateEntityQuery(checkedEntityDataAccess, queriesDesc);
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x0002EC6E File Offset: 0x0002CE6E
		public NativeArray<ArchetypeChunk> GetAllChunks(Allocator allocator = Allocator.TempJob)
		{
			return this.GetAllChunks(allocator);
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x0002EC7C File Offset: 0x0002CE7C
		public unsafe NativeArray<ArchetypeChunk> GetAllChunks(AllocatorManager.AllocatorHandle allocator)
		{
			EntityQuery universalQuery = this.GetCheckedEntityDataAccess()->m_UniversalQuery;
			return universalQuery.ToArchetypeChunkArray(allocator);
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x0002EC9D File Offset: 0x0002CE9D
		public NativeArray<ArchetypeChunk> GetAllChunksAndMetaChunks(Allocator allocator = Allocator.TempJob)
		{
			return this.GetAllChunksAndMetaChunks(allocator);
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x0002ECAC File Offset: 0x0002CEAC
		public unsafe NativeArray<ArchetypeChunk> GetAllChunksAndMetaChunks(AllocatorManager.AllocatorHandle allocator)
		{
			EntityQuery universalQueryWithChunks = this.GetCheckedEntityDataAccess()->m_UniversalQueryWithChunks;
			return universalQueryWithChunks.ToArchetypeChunkArray(allocator);
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x0002ECD0 File Offset: 0x0002CED0
		private unsafe NativeArray<ArchetypeChunk> GetAllChunksAndMetaChunksWithSystems(AllocatorManager.AllocatorHandle allocator)
		{
			EntityQuery universalQueryWithChunksAndSystems = this.GetCheckedEntityDataAccess()->m_UniversalQueryWithChunksAndSystems;
			return universalQueryWithChunksAndSystems.ToArchetypeChunkArray(allocator);
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x0002ECF4 File Offset: 0x0002CEF4
		private unsafe NativeArray<ArchetypeChunk> GetAllChunksWithSystems(AllocatorManager.AllocatorHandle allocator)
		{
			EntityQuery universalQueryWithSystems = this.GetCheckedEntityDataAccess()->m_UniversalQueryWithSystems;
			return universalQueryWithSystems.ToArchetypeChunkArray(allocator);
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x0002ED18 File Offset: 0x0002CF18
		public unsafe void GetAllArchetypes(NativeList<EntityArchetype> allArchetypes)
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			for (int i = 0; i < checkedEntityDataAccess->EntityComponentStore->m_Archetypes.Length; i++)
			{
				Archetype* ptr = *(IntPtr*)(checkedEntityDataAccess->EntityComponentStore->m_Archetypes.Ptr + (IntPtr)i * (IntPtr)sizeof(Archetype*) / (IntPtr)sizeof(Archetype*));
				EntityArchetype entityArchetype = new EntityArchetype
				{
					Archetype = ptr
				};
				allArchetypes.Add(in entityArchetype);
			}
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x0002ED7F File Offset: 0x0002CF7F
		[Obsolete("Use EntityQuery.GetEntityQueryMask()")]
		public EntityQueryMask GetEntityQueryMask(EntityQuery query)
		{
			return query.GetEntityQueryMask();
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x0002ED88 File Offset: 0x0002CF88
		public unsafe bool Exists(Entity entity)
		{
			return this.GetCheckedEntityDataAccess()->Exists(entity);
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x0002ED96 File Offset: 0x0002CF96
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe bool HasComponent<T>(Entity entity)
		{
			return this.GetCheckedEntityDataAccess()->HasComponent(entity, ComponentType.ReadWrite<T>());
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x0002EDA9 File Offset: 0x0002CFA9
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe bool HasComponent<T>(SystemHandle system)
		{
			return this.GetCheckedEntityDataAccess()->HasComponent(system.m_Entity, ComponentType.ReadWrite<T>());
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x0002ED96 File Offset: 0x0002CF96
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleBufferElement) })]
		public unsafe bool HasBuffer<T>(Entity entity) where T : struct, IBufferElementData
		{
			return this.GetCheckedEntityDataAccess()->HasComponent(entity, ComponentType.ReadWrite<T>());
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x0002EDC1 File Offset: 0x0002CFC1
		public unsafe bool HasComponent(Entity entity, ComponentType type)
		{
			return this.GetCheckedEntityDataAccess()->HasComponent(entity, type);
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x0002EDD0 File Offset: 0x0002CFD0
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe bool HasComponent(SystemHandle system, ComponentType type)
		{
			return this.GetCheckedEntityDataAccess()->HasComponent(system.m_Entity, type);
		}

		// Token: 0x06000954 RID: 2388 RVA: 0x0002EDE4 File Offset: 0x0002CFE4
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public unsafe bool HasChunkComponent<T>(Entity entity)
		{
			return this.GetCheckedEntityDataAccess()->HasComponent(entity, ComponentType.ChunkComponent<T>());
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x0002EDF8 File Offset: 0x0002CFF8
		[ExcludeFromBurstCompatTesting("This unfortunately needs access to the managed world for the ExternalAPIState.")]
		public unsafe T GetAspect<T>(Entity entity) where T : struct, IAspect, IAspectCreate<T>
		{
			T t = default(T);
			return t.CreateAspect(entity, ref *this.World.ExternalAPIState);
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x0002EE26 File Offset: 0x0002D026
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void CompleteDependencyBeforeRW<T>()
		{
			this.GetUncheckedEntityDataAccess()->DependencyManager->CompleteReadAndWriteDependency(TypeManager.GetTypeIndex<T>());
		}

		// Token: 0x06000957 RID: 2391 RVA: 0x0002EE3D File Offset: 0x0002D03D
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void CompleteDependencyBeforeRO<T>()
		{
			this.GetUncheckedEntityDataAccess()->DependencyManager->CompleteWriteDependency(TypeManager.GetTypeIndex<T>());
		}

		// Token: 0x06000958 RID: 2392 RVA: 0x0002EE54 File Offset: 0x0002D054
		[ExcludeFromBurstCompatTesting("Takes managed object")]
		internal void SetSharedComponentDataBoxedDefaultMustBeNull(Entity entity, TypeIndex typeIndex, object componentData)
		{
			int num = 0;
			if (componentData != null)
			{
				num = TypeManager.GetHashCode(componentData, typeIndex);
			}
			this.SetSharedComponentDataBoxedDefaultMustBeNull(entity, typeIndex, num, componentData);
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x0002EE78 File Offset: 0x0002D078
		[ExcludeFromBurstCompatTesting("Takes managed object")]
		private unsafe void SetSharedComponentDataBoxedDefaultMustBeNull(Entity entity, TypeIndex typeIndex, int hashCode, object componentData)
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginStructuralChanges();
			SystemHandle systemHandle = default(SystemHandle);
			checkedEntityDataAccess->SetSharedComponentDataBoxedDefaultMustBeNullDuringStructuralChange(entity, typeIndex, hashCode, componentData, in systemHandle);
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x0002EEB0 File Offset: 0x0002D0B0
		[ExcludeFromBurstCompatTesting("Takes managed object")]
		internal unsafe void SetComponentObject(Entity entity, ComponentType componentType, object componentObject)
		{
			ref EntityDataAccess checkedEntityDataAccess = ref *this.GetCheckedEntityDataAccess();
			SystemHandle systemHandle = default(SystemHandle);
			(ref checkedEntityDataAccess).SetComponentObject(entity, componentType, componentObject, in systemHandle);
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x0002EED4 File Offset: 0x0002D0D4
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		internal ComponentLookup<T> GetComponentLookup<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(bool isReadOnly = false) where T : struct, ValueType, IComponentData
		{
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			return this.GetComponentLookup<T>(typeIndex, isReadOnly);
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x0002EEF0 File Offset: 0x0002D0F0
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		internal unsafe ComponentLookup<T> GetComponentLookup<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(TypeIndex typeIndex, bool isReadOnly) where T : struct, ValueType, IComponentData
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			return new ComponentLookup<T>(typeIndex, checkedEntityDataAccess);
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x0002EF10 File Offset: 0x0002D110
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleBufferElement) })]
		internal BufferLookup<T> GetBufferLookup<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(bool isReadOnly = false) where T : struct, ValueType, IBufferElementData
		{
			return this.GetBufferLookup<T>(TypeManager.GetTypeIndex<T>(), isReadOnly);
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x0002EF20 File Offset: 0x0002D120
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleBufferElement) })]
		internal unsafe BufferLookup<T> GetBufferLookup<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(TypeIndex typeIndex, bool isReadOnly = false) where T : struct, ValueType, IBufferElementData
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			return new BufferLookup<T>(typeIndex, checkedEntityDataAccess, isReadOnly);
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x0002EF3C File Offset: 0x0002D13C
		internal EntityStorageInfoLookup GetEntityStorageInfoLookup()
		{
			return new EntityStorageInfoLookup(this.GetCheckedEntityDataAccess());
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x0002EF4C File Offset: 0x0002D14C
		internal unsafe void SetComponentDataRaw(Entity entity, TypeIndex typeIndex, void* data, int size)
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			EntityComponentStore* entityComponentStore = checkedEntityDataAccess->EntityComponentStore;
			checkedEntityDataAccess->DependencyManager->CompleteReadAndWriteDependency(typeIndex);
			SystemHandle systemHandle = default(SystemHandle);
			checkedEntityDataAccess->SetComponentDataRaw(entity, typeIndex, data, size, in systemHandle);
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x0002EF85 File Offset: 0x0002D185
		internal unsafe void* GetComponentDataRawRW(Entity entity, TypeIndex typeIndex)
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			EntityComponentStore* entityComponentStore = checkedEntityDataAccess->EntityComponentStore;
			checkedEntityDataAccess->DependencyManager->CompleteReadAndWriteDependency(typeIndex);
			return checkedEntityDataAccess->GetComponentDataRawRWEntityHasComponent(entity, typeIndex);
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x0002EFA8 File Offset: 0x0002D1A8
		internal unsafe void* GetComponentDataRawRO(Entity entity, TypeIndex typeIndex)
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			EntityComponentStore* entityComponentStore = checkedEntityDataAccess->EntityComponentStore;
			checkedEntityDataAccess->DependencyManager->CompleteWriteDependency(typeIndex);
			return (void*)entityComponentStore->GetComponentDataWithTypeRO(entity, typeIndex);
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x0002EFD5 File Offset: 0x0002D1D5
		internal unsafe void SetEnableableComponent(Entity entity, TypeIndex typeIndex, bool value)
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			EntityComponentStore* entityComponentStore = checkedEntityDataAccess->EntityComponentStore;
			checkedEntityDataAccess->DependencyManager->CompleteReadAndWriteDependency(typeIndex);
			checkedEntityDataAccess->SetComponentEnabled(entity, typeIndex, value);
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x0002EFF8 File Offset: 0x0002D1F8
		[ExcludeFromBurstCompatTesting("Returns managed object")]
		internal unsafe object GetSharedComponentData(Entity entity, TypeIndex typeIndex)
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			EntityComponentStore* entityComponentStore = checkedEntityDataAccess->EntityComponentStore;
			ManagedComponentStore managedComponentStore = checkedEntityDataAccess->ManagedComponentStore;
			int sharedComponentDataIndex = entityComponentStore->GetSharedComponentDataIndex(entity, typeIndex);
			return this.GetSharedComponentDataBoxed(sharedComponentDataIndex, typeIndex);
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x0002F02C File Offset: 0x0002D22C
		internal unsafe void* GetBufferRawRW(Entity entity, TypeIndex typeIndex)
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			EntityComponentStore* entityComponentStore = checkedEntityDataAccess->EntityComponentStore;
			checkedEntityDataAccess->DependencyManager->CompleteReadAndWriteDependency(typeIndex);
			BufferHeader* componentDataWithTypeRW = (BufferHeader*)entityComponentStore->GetComponentDataWithTypeRW(entity, typeIndex, entityComponentStore->GlobalSystemVersion);
			return (void*)BufferHeader.GetElementPointer(componentDataWithTypeRW);
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x0002F068 File Offset: 0x0002D268
		internal unsafe void* GetBufferRawRO(Entity entity, TypeIndex typeIndex)
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			EntityComponentStore* entityComponentStore = checkedEntityDataAccess->EntityComponentStore;
			checkedEntityDataAccess->DependencyManager->CompleteWriteDependency(typeIndex);
			BufferHeader* componentDataWithTypeRO = (BufferHeader*)entityComponentStore->GetComponentDataWithTypeRO(entity, typeIndex);
			return (void*)BufferHeader.GetElementPointer(componentDataWithTypeRO);
		}

		// Token: 0x06000967 RID: 2407 RVA: 0x0002F09C File Offset: 0x0002D29C
		internal unsafe int GetBufferLength(Entity entity, TypeIndex typeIndex)
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			EntityComponentStore* entityComponentStore = checkedEntityDataAccess->EntityComponentStore;
			checkedEntityDataAccess->DependencyManager->CompleteWriteDependency(typeIndex);
			BufferHeader* componentDataWithTypeRO = (BufferHeader*)entityComponentStore->GetComponentDataWithTypeRO(entity, typeIndex);
			return componentDataWithTypeRO->Length;
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x0002F0D0 File Offset: 0x0002D2D0
		[UsedImplicitly]
		internal void AddComponentRaw(Entity entity, TypeIndex typeIndex)
		{
			this.AddComponent(entity, ComponentType.FromTypeIndex(typeIndex));
		}

		// Token: 0x06000969 RID: 2409 RVA: 0x0002F0E0 File Offset: 0x0002D2E0
		[UsedImplicitly]
		internal void RemoveComponentRaw(Entity entity, TypeIndex typeIndex)
		{
			this.RemoveComponent(entity, ComponentType.FromTypeIndex(typeIndex));
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x0002F0F0 File Offset: 0x0002D2F0
		internal unsafe void DestroyEntityInternal(Entity* entities, int count)
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess->BeginStructuralChanges();
			SystemHandle systemHandle = default(SystemHandle);
			checkedEntityDataAccess->DestroyEntityInternalDuringStructuralChange(entities, count, in systemHandle);
			checkedEntityDataAccess->EndStructuralChanges(ref archetypeChanges);
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x0002F124 File Offset: 0x0002D324
		private unsafe void MoveEntitiesFromInternalQuery(EntityManager srcEntities, EntityQuery filter, NativeArray<EntityRemapUtility.EntityRemapInfo> entityRemapping)
		{
			EntityDataAccess* checkedEntityDataAccess = srcEntities.GetCheckedEntityDataAccess();
			EntityDataAccess* checkedEntityDataAccess2 = this.GetCheckedEntityDataAccess();
			this.BeforeStructuralChange();
			srcEntities.BeforeStructuralChange();
			using (NativeArray<ArchetypeChunk> nativeArray = filter.ToArchetypeChunkArray(Allocator.TempJob))
			{
				EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess2->EntityComponentStore->BeginArchetypeChangeTracking();
				this.MoveChunksFromFiltered(nativeArray, entityRemapping, checkedEntityDataAccess->EntityComponentStore, checkedEntityDataAccess->ManagedComponentStore);
				checkedEntityDataAccess2->EntityComponentStore->EndArchetypeChangeTracking(archetypeChanges, checkedEntityDataAccess2->EntityQueryManager);
				checkedEntityDataAccess2->EntityComponentStore->InvalidateChunkListCacheForChangedArchetypes();
				checkedEntityDataAccess->EntityComponentStore->InvalidateChunkListCacheForChangedArchetypes();
			}
		}

		// Token: 0x0600096C RID: 2412 RVA: 0x0002F1C4 File Offset: 0x0002D3C4
		[ExcludeFromBurstCompatTesting("Accesses managed component store")]
		internal unsafe void MoveEntitiesFromInternalAll(EntityManager srcEntities, NativeArray<EntityRemapUtility.EntityRemapInfo> entityRemapping)
		{
			EntityDataAccess* checkedEntityDataAccess = srcEntities.GetCheckedEntityDataAccess();
			EntityDataAccess* checkedEntityDataAccess2 = this.GetCheckedEntityDataAccess();
			srcEntities.BeforeStructuralChange();
			EntityComponentStore.ArchetypeChanges archetypeChanges = checkedEntityDataAccess2->BeginStructuralChanges();
			this.MoveChunksFromAll(entityRemapping, checkedEntityDataAccess->EntityComponentStore, checkedEntityDataAccess->ManagedComponentStore);
			checkedEntityDataAccess2->EndStructuralChanges(ref archetypeChanges);
			checkedEntityDataAccess->EntityComponentStore->InvalidateChunkListCacheForChangedArchetypes();
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x0002F212 File Offset: 0x0002D412
		[BurstCompile]
		private unsafe static void RemapChunksForFilteredMove(ref NativeArray<ArchetypeChunk> chunks, ref NativeArray<EntityManager.RemapChunk> remapChunks, ref NativeArray<EntityRemapUtility.EntityRemapInfo> entityRemapping, ref NativeList<ChunkIndex> managedChunks, out int managedComponentCount, EntityComponentStore* dstEntityComponentStore, EntityComponentStore* srcEntityComponentStore)
		{
			EntityManager.RemapChunksForFilteredMove_0000089C$BurstDirectCall.Invoke(ref chunks, ref remapChunks, ref entityRemapping, ref managedChunks, out managedComponentCount, dstEntityComponentStore, srcEntityComponentStore);
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x0002F224 File Offset: 0x0002D424
		[ExcludeFromBurstCompatTesting("Accesses managed component store")]
		internal unsafe void MoveChunksFromFiltered(NativeArray<ArchetypeChunk> chunks, NativeArray<EntityRemapUtility.EntityRemapInfo> entityRemapping, EntityComponentStore* srcEntityComponentStore, ManagedComponentStore srcManagedComponentStore)
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			EntityComponentStore* entityComponentStore = checkedEntityDataAccess->EntityComponentStore;
			ManagedComponentStore managedComponentStore = checkedEntityDataAccess->ManagedComponentStore;
			new EntityManager.MoveChunksJob
			{
				srcEntityComponentStore = srcEntityComponentStore,
				dstEntityComponentStore = entityComponentStore,
				entityRemapping = entityRemapping,
				chunks = chunks
			}.Run<EntityManager.MoveChunksJob>();
			int num = 0;
			NativeArray<EntityManager.RemapChunk> nativeArray = new NativeArray<EntityManager.RemapChunk>(chunks.Length, Allocator.TempJob, NativeArrayOptions.ClearMemory);
			NativeList<ChunkIndex> nativeList = new NativeList<ChunkIndex>(0, Allocator.TempJob);
			EntityManager.RemapChunksForFilteredMove(ref chunks, ref nativeArray, ref entityRemapping, ref nativeList, out num, entityComponentStore, srcEntityComponentStore);
			NativeArray<int> nativeArray2 = default(NativeArray<int>);
			NativeArray<int> nativeArray3 = default(NativeArray<int>);
			int num2 = 0;
			if (num > 0)
			{
				nativeArray2 = new NativeArray<int>(num, Allocator.TempJob, NativeArrayOptions.ClearMemory);
				nativeArray3 = new NativeArray<int>(num, Allocator.TempJob, NativeArrayOptions.ClearMemory);
				new EntityManager.GatherManagedComponentIndicesInChunkJob
				{
					SrcEntityComponentStore = srcEntityComponentStore,
					DstEntityComponentStore = entityComponentStore,
					SrcManagedIndices = nativeArray2,
					DstManagedIndices = nativeArray3,
					Chunks = nativeList.AsArray(),
					NonNullCount = &num2
				}.Run<EntityManager.GatherManagedComponentIndicesInChunkJob>();
			}
			managedComponentStore.Playback(ref entityComponentStore->ManagedChangesTracker);
			srcManagedComponentStore.Playback(ref srcEntityComponentStore->ManagedChangesTracker);
			NativeHashMap<int, int> nativeHashMap = checkedEntityDataAccess->MoveSharedComponents(srcEntityComponentStore, srcManagedComponentStore, chunks, Allocator.TempJob);
			if (num > 0)
			{
				managedComponentStore.MoveManagedComponentsFromDifferentWorld(nativeArray2, nativeArray3, num2, srcManagedComponentStore);
				srcEntityComponentStore->m_ManagedComponentFreeIndex.Add(nativeArray2.GetUnsafeReadOnlyPtr<int>(), 4 * nativeArray2.Length);
			}
			new EntityManager.ChunkPatchEntities
			{
				RemapChunks = nativeArray,
				EntityRemapping = entityRemapping,
				EntityComponentStore = entityComponentStore
			}.Run<EntityManager.ChunkPatchEntities>();
			JobHandle jobHandle = new EntityManager.RemapChunksFilteredJob
			{
				dstEntityComponentStore = entityComponentStore,
				remapChunks = nativeArray,
				entityRemapping = entityRemapping,
				chunkHeaderType = TypeManager.GetTypeIndex<ChunkHeader>()
			}.Schedule(nativeArray.Length, 1, default(JobHandle));
			new EntityManager.MoveFilteredChunksBetweenArchetypeJob
			{
				RemapChunks = nativeArray,
				RemapShared = nativeHashMap,
				SrcEntityComponentStore = srcEntityComponentStore
			}.Schedule(jobHandle).Complete();
			managedComponentStore.Playback(ref entityComponentStore->ManagedChangesTracker);
			if (num > 0)
			{
				nativeArray2.Dispose();
				nativeArray3.Dispose();
			}
			nativeList.Dispose();
			nativeHashMap.Dispose();
			nativeArray.Dispose();
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x0002F444 File Offset: 0x0002D644
		[ExcludeFromBurstCompatTesting("Accesses managed component store")]
		internal unsafe void MoveChunksFromAll(NativeArray<EntityRemapUtility.EntityRemapInfo> entityRemapping, EntityComponentStore* srcEntityComponentStore, ManagedComponentStore srcManagedComponentStore)
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			EntityComponentStore* entityComponentStore = checkedEntityDataAccess->EntityComponentStore;
			ManagedComponentStore managedComponentStore = checkedEntityDataAccess->ManagedComponentStore;
			JobHandle jobHandle = new EntityManager.MoveAllChunksJob
			{
				srcEntityComponentStore = srcEntityComponentStore,
				dstEntityComponentStore = entityComponentStore,
				entityRemapping = entityRemapping
			}.Schedule(default(JobHandle));
			JobHandle jobHandle2 = new EntityManager.FreeAllChunksJob
			{
				EntityComponentStore = srcEntityComponentStore
			}.Schedule(jobHandle);
			int managedComponentIndexUsedCount = srcEntityComponentStore->ManagedComponentIndexUsedCount;
			NativeArray<int> nativeArray = default(NativeArray<int>);
			NativeArray<int> nativeArray2 = default(NativeArray<int>);
			JobHandle jobHandle3 = default(JobHandle);
			if (managedComponentIndexUsedCount > 0)
			{
				nativeArray = new NativeArray<int>(managedComponentIndexUsedCount, Allocator.TempJob, NativeArrayOptions.ClearMemory);
				nativeArray2 = new NativeArray<int>(managedComponentIndexUsedCount, Allocator.TempJob, NativeArrayOptions.ClearMemory);
				entityComponentStore->ReserveManagedComponentIndices(managedComponentIndexUsedCount);
				jobHandle3 = new EntityManager.GatherAllManagedComponentIndicesJob
				{
					SrcEntityComponentStore = srcEntityComponentStore,
					DstEntityComponentStore = entityComponentStore,
					SrcManagedIndices = nativeArray,
					DstManagedIndices = nativeArray2
				}.Schedule(default(JobHandle));
			}
			JobHandle.ScheduleBatchedJobs();
			int num = 0;
			for (int i = 0; i < srcEntityComponentStore->m_Archetypes.Length; i++)
			{
				Archetype* ptr = *(IntPtr*)(srcEntityComponentStore->m_Archetypes.Ptr + (IntPtr)i * (IntPtr)sizeof(Archetype*) / (IntPtr)sizeof(Archetype*));
				num += ptr->Chunks.Count;
			}
			NativeArray<EntityManager.RemapChunk> nativeArray3 = new NativeArray<EntityManager.RemapChunk>(num, Allocator.TempJob, NativeArrayOptions.ClearMemory);
			NativeArray<EntityManager.RemapArchetype> nativeArray4 = new NativeArray<EntityManager.RemapArchetype>(srcEntityComponentStore->m_Archetypes.Length, Allocator.TempJob, NativeArrayOptions.ClearMemory);
			int num2 = 0;
			int num3 = 0;
			for (int j = 0; j < srcEntityComponentStore->m_Archetypes.Length; j++)
			{
				Archetype* ptr2 = *(IntPtr*)(srcEntityComponentStore->m_Archetypes.Ptr + (IntPtr)j * (IntPtr)sizeof(Archetype*) / (IntPtr)sizeof(Archetype*));
				if (ptr2->Chunks.Count != 0)
				{
					Archetype* orCreateArchetype = entityComponentStore->GetOrCreateArchetype(ptr2->Types, ptr2->TypesCount);
					nativeArray4[num3] = new EntityManager.RemapArchetype
					{
						srcArchetype = ptr2,
						dstArchetype = orCreateArchetype
					};
					srcEntityComponentStore->m_ChunkListChangesTracker.TrackArchetype(ptr2);
					entityComponentStore->m_ChunkListChangesTracker.TrackArchetype(orCreateArchetype);
					for (int k = 0; k < ptr2->Chunks.Count; k++)
					{
						ChunkIndex chunkIndex = ptr2->Chunks[k];
						nativeArray3[num2] = new EntityManager.RemapChunk
						{
							chunk = chunkIndex,
							dstArchetype = orCreateArchetype
						};
						num2++;
					}
					num3++;
					entityComponentStore->IncrementComponentTypeOrderVersion(orCreateArchetype);
				}
			}
			jobHandle.Complete();
			managedComponentStore.Playback(ref entityComponentStore->ManagedChangesTracker);
			srcManagedComponentStore.Playback(ref srcEntityComponentStore->ManagedChangesTracker);
			NativeParallelHashMap<int, int> nativeParallelHashMap = checkedEntityDataAccess->MoveAllSharedComponents(srcEntityComponentStore, srcManagedComponentStore, Allocator.TempJob);
			jobHandle3.Complete();
			managedComponentStore.MoveManagedComponentsFromDifferentWorld(nativeArray, nativeArray2, nativeArray.Length, srcManagedComponentStore);
			srcEntityComponentStore->m_ManagedComponentFreeIndex.Length = 0;
			srcEntityComponentStore->m_ManagedComponentIndex = 1;
			new EntityManager.ChunkPatchEntities
			{
				RemapChunks = nativeArray3,
				EntityRemapping = entityRemapping,
				EntityComponentStore = entityComponentStore
			}.Run<EntityManager.ChunkPatchEntities>();
			JobHandle jobHandle4 = new EntityManager.RemapAllChunksJob
			{
				dstEntityComponentStore = entityComponentStore,
				remapChunks = nativeArray3,
				entityRemapping = entityRemapping
			}.Schedule(nativeArray3.Length, 1, jobHandle2);
			JobHandle jobHandle5 = new EntityManager.RemapAllArchetypesJob
			{
				remapArchetypes = nativeArray4,
				remapShared = nativeParallelHashMap,
				dstEntityComponentStore = entityComponentStore,
				chunkHeaderType = TypeManager.GetTypeIndex<ChunkHeader>()
			}.Schedule(num3, 1, jobHandle4);
			managedComponentStore.Playback(ref entityComponentStore->ManagedChangesTracker);
			if (managedComponentIndexUsedCount > 0)
			{
				nativeArray.Dispose();
				nativeArray2.Dispose();
			}
			jobHandle5.Complete();
			nativeParallelHashMap.Dispose();
			nativeArray3.Dispose();
			jobHandle2.Complete();
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x0002F7D4 File Offset: 0x0002D9D4
		internal unsafe void DuplicateEntitiesForDiffer(NativeArray<EntityRemapUtility.EntityRemapInfo> entityRemapping, NativeArray<ArchetypeChunk> srcChunks, NativeArray<ArchetypeChunk> dstChunks)
		{
			int i = 0;
			int length = srcChunks.Length;
			while (i < length)
			{
				ChunkIndex chunk = srcChunks[i].m_Chunk;
				ChunkIndex chunk2 = dstChunks[i].m_Chunk;
				Entity* buffer = (Entity*)chunk.Buffer;
				Entity* buffer2 = (Entity*)chunk2.Buffer;
				int count = chunk2.Count;
				EntityComponentStore.s_entityStore.Data.AllocateEntities(buffer2, count, ChunkIndex.Null, 0);
				for (int j = 0; j < count; j++)
				{
					EntityRemapUtility.AddEntityRemapping(ref entityRemapping, buffer[j], buffer2[j]);
				}
				i++;
			}
		}

		// Token: 0x06000971 RID: 2417 RVA: 0x0002F888 File Offset: 0x0002DA88
		internal unsafe void RemapEntitiesForDiffer(NativeArray<EntityRemapUtility.EntityRemapInfo> entityRemapping, NativeArray<ArchetypeChunk> srcChunks, NativeArray<ArchetypeChunk> dstChunks)
		{
			EntityComponentStore* entityComponentStore = this.GetCheckedEntityDataAccess()->EntityComponentStore;
			new EntityManager.RemapEntitiesManagedForDiffer
			{
				DstChunks = dstChunks,
				EntityRemapping = entityRemapping,
				EntityComponentStore = entityComponentStore
			}.Run<EntityManager.RemapEntitiesManagedForDiffer>();
			new EntityManager.RemapEntitiesUnmanagedForDiffer
			{
				dstEntityComponentStore = entityComponentStore,
				DstChunks = dstChunks,
				EntityRemapping = entityRemapping
			}.Run(dstChunks.Length);
		}

		// Token: 0x06000972 RID: 2418 RVA: 0x0002F8F4 File Offset: 0x0002DAF4
		internal unsafe uint GetChunkVersionHash(Entity entity)
		{
			EntityComponentStore* entityComponentStore = this.GetCheckedEntityDataAccess()->EntityComponentStore;
			if (!entityComponentStore->Exists(entity))
			{
				return 0U;
			}
			ChunkIndex chunk = entityComponentStore->GetChunk(entity);
			Archetype* archetype = entityComponentStore->GetArchetype(chunk);
			int typesCount = archetype->TypesCount;
			uint num = 0U;
			ArchetypeChunkData chunks = archetype->Chunks;
			int listIndex = chunk.ListIndex;
			for (int i = 0; i < typesCount; i++)
			{
				num += chunks.GetChangeVersion(i, listIndex);
			}
			return num;
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x0002F95F File Offset: 0x0002DB5F
		internal unsafe void BeforeStructuralChange()
		{
			this.GetCheckedEntityDataAccess()->BeforeStructuralChange();
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x0002F96C File Offset: 0x0002DB6C
		internal unsafe TypeIndex GetComponentTypeIndex(Entity entity, int index)
		{
			Archetype* archetype = this.GetCheckedEntityDataAccess()->EntityComponentStore->GetArchetype(entity);
			if ((ulong)index >= (ulong)((long)archetype->TypesCount))
			{
				return TypeIndex.Null;
			}
			return archetype->Types[index + 1].TypeIndex;
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x0002F9B4 File Offset: 0x0002DBB4
		internal unsafe EntityQuery CreateEntityQuery(ComponentType* requiredComponents, int count)
		{
			EntityDataAccess* checkedEntityDataAccess = this.GetCheckedEntityDataAccess();
			return checkedEntityDataAccess->EntityQueryManager->CreateEntityQuery(checkedEntityDataAccess, requiredComponents, count);
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x0002F9D6 File Offset: 0x0002DBD6
		public unsafe bool IsQueryValid(EntityQuery query)
		{
			return this.GetCheckedEntityDataAccess()->IsQueryValid(query);
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x0002F9E4 File Offset: 0x0002DBE4
		internal unsafe bool HasComponentRaw(Entity entity, TypeIndex typeIndex)
		{
			bool flag;
			return this.GetCheckedEntityDataAccess()->EntityComponentStore->HasComponent(entity, typeIndex, out flag);
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x0002FA05 File Offset: 0x0002DC05
		[ExcludeFromBurstCompatTesting("Returns managed string")]
		public unsafe string GetName(Entity entity)
		{
			return this.GetCheckedEntityDataAccess()->GetName(entity);
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x0002FA13 File Offset: 0x0002DC13
		[GenerateTestsForBurstCompatibility]
		public unsafe void GetName(Entity entity, out FixedString64Bytes name)
		{
			this.GetCheckedEntityDataAccess()->GetName(entity, out name);
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x0002FA22 File Offset: 0x0002DC22
		[GenerateTestsForBurstCompatibility]
		public unsafe void SetName(Entity entity, FixedString64Bytes name)
		{
			this.GetCheckedEntityDataAccess()->SetName(entity, in name);
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x0002FA32 File Offset: 0x0002DC32
		public NativeArray<Entity> GetAllEntities(Allocator allocator = Allocator.Temp)
		{
			return this.GetAllEntities(allocator, EntityManager.GetAllEntitiesOptions.Default);
		}

		// Token: 0x0600097C RID: 2428 RVA: 0x0002FA41 File Offset: 0x0002DC41
		public NativeArray<Entity> GetAllEntities(AllocatorManager.AllocatorHandle allocator)
		{
			return this.GetAllEntities(allocator, EntityManager.GetAllEntitiesOptions.Default);
		}

		// Token: 0x0600097D RID: 2429 RVA: 0x0002FA4C File Offset: 0x0002DC4C
		public NativeArray<Entity> GetAllEntities(AllocatorManager.AllocatorHandle allocator, EntityManager.GetAllEntitiesOptions options)
		{
			NativeArray<ArchetypeChunk> nativeArray = default(NativeArray<ArchetypeChunk>);
			if ((options & EntityManager.GetAllEntitiesOptions.IncludeMeta) != EntityManager.GetAllEntitiesOptions.Default)
			{
				if ((options & EntityManager.GetAllEntitiesOptions.IncludeSystems) != EntityManager.GetAllEntitiesOptions.Default)
				{
					nativeArray = this.GetAllChunksAndMetaChunksWithSystems(allocator);
				}
				else
				{
					nativeArray = this.GetAllChunksAndMetaChunks(allocator);
				}
			}
			else if ((options & EntityManager.GetAllEntitiesOptions.IncludeSystems) != EntityManager.GetAllEntitiesOptions.Default)
			{
				nativeArray = this.GetAllChunksWithSystems(allocator);
			}
			else
			{
				if (options != EntityManager.GetAllEntitiesOptions.Default)
				{
					throw new ArgumentException("Invalid enum value", "options");
				}
				nativeArray = this.GetAllChunks(allocator);
			}
			NativeArray<Entity> nativeArray2 = CollectionHelper.CreateNativeArray<Entity>(ArchetypeChunkArray.TotalEntityCountInChunksIgnoreFiltering(nativeArray), allocator, NativeArrayOptions.ClearMemory);
			EntityTypeHandle entityTypeHandle = this.GetEntityTypeHandle();
			int num = 0;
			for (int i = 0; i < nativeArray.Length; i++)
			{
				NativeArray<Entity> nativeArray3 = nativeArray[i].GetNativeArray(entityTypeHandle);
				nativeArray2.Slice<Entity>(num, nativeArray3.Length).CopyFrom(nativeArray3);
				num += nativeArray3.Length;
			}
			nativeArray.Dispose();
			return nativeArray2;
		}

		// Token: 0x0600097E RID: 2430 RVA: 0x0002FB1D File Offset: 0x0002DD1D
		internal unsafe Entity GetEntityByEntityIndex(int index)
		{
			return this.GetCheckedEntityDataAccess()->GetEntityByEntityIndex(index);
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x0002FB2B File Offset: 0x0002DD2B
		internal unsafe int GetNameIndexByEntityIndex(int index)
		{
			return this.GetCheckedEntityDataAccess()->GetNameIndexByEntityIndex(index);
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000980 RID: 2432 RVA: 0x0002FB39 File Offset: 0x0002DD39
		// (set) Token: 0x06000981 RID: 2433 RVA: 0x0002FB4B File Offset: 0x0002DD4B
		public unsafe JobHandle ExclusiveEntityTransactionDependency
		{
			get
			{
				return this.m_EntityDataAccess->DependencyManager->ExclusiveTransactionDependency;
			}
			set
			{
				this.m_EntityDataAccess->DependencyManager->ExclusiveTransactionDependency = value;
			}
		}

		// Token: 0x06000982 RID: 2434 RVA: 0x0002FB5E File Offset: 0x0002DD5E
		public unsafe bool CanBeginExclusiveEntityTransaction()
		{
			return !this.GetUncheckedEntityDataAccess()->DependencyManager->IsInTransaction;
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x0002FB75 File Offset: 0x0002DD75
		public unsafe ExclusiveEntityTransaction BeginExclusiveEntityTransaction()
		{
			this.GetCheckedEntityDataAccess()->DependencyManager->BeginExclusiveTransaction();
			return new ExclusiveEntityTransaction(this);
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x0002FB92 File Offset: 0x0002DD92
		public unsafe void EndExclusiveEntityTransaction()
		{
			this.m_EntityDataAccess->DependencyManager->PreEndExclusiveTransaction();
			this.m_EntityDataAccess->DependencyManager->EndExclusiveTransaction();
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x0002FBB4 File Offset: 0x0002DDB4
		[ExcludeFromBurstCompatTesting("Accesses managed component store")]
		internal unsafe void AddSharedComponentManaged<T>(NativeArray<ArchetypeChunk> chunks, T componentData) where T : struct, ISharedComponentData
		{
			ComponentType componentType = ComponentType.ReadWrite<T>();
			EntityComponentStore.ArchetypeChanges archetypeChanges = this.m_EntityDataAccess->BeginStructuralChanges();
			int num = this.m_EntityDataAccess->InsertSharedComponent<T>(componentData);
			ref EntityDataAccess entityDataAccess = ref *this.m_EntityDataAccess;
			int num2 = num;
			ComponentType componentType2 = componentType;
			SystemHandle systemHandle = default(SystemHandle);
			entityDataAccess.AddSharedComponentDataDuringStructuralChange(chunks, num2, componentType2, in systemHandle);
			this.m_EntityDataAccess->EndStructuralChanges(ref archetypeChanges);
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x0002FC04 File Offset: 0x0002DE04
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleSharedComponentData) })]
		internal unsafe void AddSharedComponent<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(NativeArray<ArchetypeChunk> chunks, T componentData) where T : struct, ValueType, ISharedComponentData
		{
			ComponentType componentType = ComponentType.ReadWrite<T>();
			EntityComponentStore.ArchetypeChanges archetypeChanges = this.m_EntityDataAccess->BeginStructuralChanges();
			int num = this.m_EntityDataAccess->InsertSharedComponent_Unmanaged<T>(componentData);
			ref EntityDataAccess entityDataAccess = ref *this.m_EntityDataAccess;
			int num2 = num;
			ComponentType componentType2 = componentType;
			SystemHandle systemHandle = default(SystemHandle);
			entityDataAccess.AddSharedComponentDataDuringStructuralChange(chunks, num2, componentType2, in systemHandle);
			this.m_EntityDataAccess->EndStructuralChanges(ref archetypeChanges);
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x0002FC84 File Offset: 0x0002DE84
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void RemapChunksForFilteredMove$BurstManaged(ref NativeArray<ArchetypeChunk> chunks, ref NativeArray<EntityManager.RemapChunk> remapChunks, ref NativeArray<EntityRemapUtility.EntityRemapInfo> entityRemapping, ref NativeList<ChunkIndex> managedChunks, out int managedComponentCount, EntityComponentStore* dstEntityComponentStore, EntityComponentStore* srcEntityComponentStore)
		{
			int length = chunks.Length;
			Archetype* ptr = null;
			Archetype* ptr2 = null;
			managedComponentCount = 0;
			using (NativeList<Entity> nativeList = new NativeList<Entity>(Allocator.TempJob))
			{
				for (int i = 0; i < length; i++)
				{
					ChunkIndex chunk = chunks[i].m_Chunk;
					Archetype* archetype = srcEntityComponentStore->GetArchetype(chunk);
					if (ptr != archetype)
					{
						ptr2 = dstEntityComponentStore->GetOrCreateArchetype(archetype->Types, archetype->TypesCount);
						ptr = archetype;
					}
					remapChunks[i] = new EntityManager.RemapChunk
					{
						chunk = chunk,
						dstArchetype = ptr2
					};
					if (ptr2->NumManagedComponents > 0)
					{
						managedComponentCount += chunk.Count * ptr2->NumManagedComponents;
						managedChunks.Add(in chunk);
					}
					if (archetype->MetaChunkArchetype != null)
					{
						Entity metaChunkEntity = chunk.MetaChunkEntity;
						Entity entity;
						dstEntityComponentStore->CreateEntities(ptr2->MetaChunkArchetype, &entity, 1);
						EntityInChunk entityInChunk = srcEntityComponentStore->GetEntityInChunk(metaChunkEntity);
						EntityInChunk entityInChunk2 = dstEntityComponentStore->GetEntityInChunk(entity);
						Archetype* archetype2 = srcEntityComponentStore->GetArchetype(metaChunkEntity);
						Archetype* archetype3 = dstEntityComponentStore->GetArchetype(entity);
						ChunkDataUtility.CopyComponents(entityInChunk.Chunk, archetype2, entityInChunk.IndexInChunk, entityInChunk2.Chunk, archetype3, entityInChunk2.IndexInChunk, 1, dstEntityComponentStore->GlobalSystemVersion);
						EntityRemapUtility.AddEntityRemapping(ref entityRemapping, metaChunkEntity, entity);
						nativeList.Add(in metaChunkEntity);
					}
				}
				if (nativeList.Length > 0)
				{
					srcEntityComponentStore->DestroyEntities(nativeList.GetUnsafeReadOnlyPtr<Entity>(), nativeList.Length);
				}
			}
		}

		// Token: 0x04000468 RID: 1128
		private static readonly ProfilerMarker k_ProfileMoveSharedComponents = new ProfilerMarker("MoveSharedComponents");

		// Token: 0x04000469 RID: 1129
		private static readonly ProfilerMarker k_ProfileMoveManagedComponents = new ProfilerMarker("MoveManagedComponents");

		// Token: 0x0400046A RID: 1130
		private static readonly ProfilerMarker k_ProfileMoveEntitiesFrom = new ProfilerMarker("EntityManager.MoveEntitiesFrom");

		// Token: 0x0400046B RID: 1131
		[NativeDisableUnsafePtrRestriction]
		private unsafe EntityDataAccess* m_EntityDataAccess;

		// Token: 0x02000172 RID: 370
		[AttributeUsage(AttributeTargets.Method)]
		private class StructuralChangeMethodAttribute : Attribute
		{
		}

		// Token: 0x02000173 RID: 371
		private struct IsolateCopiedEntities : IComponentData, IQueryTypeParameter
		{
		}

		// Token: 0x02000174 RID: 372
		[BurstCompile]
		internal struct RemapEntitiesManagedForDiffer : IJob
		{
			// Token: 0x0600098A RID: 2442 RVA: 0x0002FE24 File Offset: 0x0002E024
			public unsafe void Execute()
			{
				int i = 0;
				int length = this.DstChunks.Length;
				while (i < length)
				{
					ArchetypeChunk archetypeChunk = this.DstChunks[i];
					ChunkIndex chunk = archetypeChunk.m_Chunk;
					Archetype* archetype = archetypeChunk.Archetype.Archetype;
					this.EntityComponentStore->ManagedChangesTracker.PatchEntities(archetype, chunk, chunk.Count, this.EntityRemapping);
					i++;
				}
			}

			// Token: 0x0400046C RID: 1132
			public NativeArray<ArchetypeChunk> DstChunks;

			// Token: 0x0400046D RID: 1133
			public NativeArray<EntityRemapUtility.EntityRemapInfo> EntityRemapping;

			// Token: 0x0400046E RID: 1134
			[NativeDisableUnsafePtrRestriction]
			public unsafe EntityComponentStore* EntityComponentStore;
		}

		// Token: 0x02000175 RID: 373
		[BurstCompile]
		private struct RemapEntitiesUnmanagedForDiffer : IJobParallelFor
		{
			// Token: 0x0600098B RID: 2443 RVA: 0x0002FE8C File Offset: 0x0002E08C
			public unsafe void Execute(int index)
			{
				ArchetypeChunk archetypeChunk = this.DstChunks[index];
				ChunkIndex chunk = archetypeChunk.m_Chunk;
				Archetype* archetype = archetypeChunk.Archetype.Archetype;
				int count = chunk.Count;
				EntityRemapUtility.PatchEntities(archetype->ScalarEntityPatches + 1, archetype->ScalarEntityPatchCount - 1, archetype->BufferEntityPatches, archetype->BufferEntityPatchCount, chunk.Buffer, count, ref this.EntityRemapping);
			}

			// Token: 0x0400046F RID: 1135
			[ReadOnly]
			public NativeArray<EntityRemapUtility.EntityRemapInfo> EntityRemapping;

			// Token: 0x04000470 RID: 1136
			[ReadOnly]
			public NativeArray<ArchetypeChunk> DstChunks;

			// Token: 0x04000471 RID: 1137
			[NativeDisableUnsafePtrRestriction]
			public unsafe EntityComponentStore* dstEntityComponentStore;
		}

		// Token: 0x02000176 RID: 374
		internal struct RemapChunk
		{
			// Token: 0x04000472 RID: 1138
			public ChunkIndex chunk;

			// Token: 0x04000473 RID: 1139
			public unsafe Archetype* dstArchetype;
		}

		// Token: 0x02000177 RID: 375
		internal struct RemapArchetype
		{
			// Token: 0x04000474 RID: 1140
			public unsafe Archetype* srcArchetype;

			// Token: 0x04000475 RID: 1141
			public unsafe Archetype* dstArchetype;
		}

		// Token: 0x02000178 RID: 376
		[BurstCompile]
		internal struct ChunkPatchEntities : IJob
		{
			// Token: 0x0600098C RID: 2444 RVA: 0x0002FEF8 File Offset: 0x0002E0F8
			public unsafe void Execute()
			{
				for (int i = 0; i < this.RemapChunks.Length; i++)
				{
					EntityManager.RemapChunk remapChunk = this.RemapChunks[i];
					ChunkIndex chunk = remapChunk.chunk;
					Archetype* dstArchetype = remapChunk.dstArchetype;
					this.EntityComponentStore->ManagedChangesTracker.PatchEntities(dstArchetype, chunk, chunk.Count, this.EntityRemapping);
				}
			}

			// Token: 0x04000476 RID: 1142
			public NativeArray<EntityManager.RemapChunk> RemapChunks;

			// Token: 0x04000477 RID: 1143
			public NativeArray<EntityRemapUtility.EntityRemapInfo> EntityRemapping;

			// Token: 0x04000478 RID: 1144
			[NativeDisableUnsafePtrRestriction]
			public unsafe EntityComponentStore* EntityComponentStore;
		}

		// Token: 0x02000179 RID: 377
		[BurstCompile]
		internal struct MoveChunksJob : IJob
		{
			// Token: 0x0600098D RID: 2445 RVA: 0x0002FF54 File Offset: 0x0002E154
			public unsafe void Execute()
			{
				int length = this.chunks.Length;
				for (int i = 0; i < length; i++)
				{
					ChunkIndex chunk = this.chunks[i].m_Chunk;
					this.dstEntityComponentStore->AllocateEntitiesForRemapping(chunk, this.srcEntityComponentStore, ref this.entityRemapping);
					this.srcEntityComponentStore->FreeEntities(chunk);
				}
			}

			// Token: 0x04000479 RID: 1145
			[NativeDisableUnsafePtrRestriction]
			public unsafe EntityComponentStore* srcEntityComponentStore;

			// Token: 0x0400047A RID: 1146
			[NativeDisableUnsafePtrRestriction]
			public unsafe EntityComponentStore* dstEntityComponentStore;

			// Token: 0x0400047B RID: 1147
			public NativeArray<EntityRemapUtility.EntityRemapInfo> entityRemapping;

			// Token: 0x0400047C RID: 1148
			[ReadOnly]
			public NativeArray<ArchetypeChunk> chunks;
		}

		// Token: 0x0200017A RID: 378
		[BurstCompile]
		internal struct GatherAllManagedComponentIndicesJob : IJob
		{
			// Token: 0x0600098E RID: 2446 RVA: 0x0002FFB0 File Offset: 0x0002E1B0
			public unsafe void Execute()
			{
				this.DstEntityComponentStore->AllocateManagedComponentIndices((int*)this.DstManagedIndices.GetUnsafePtr<int>(), this.DstManagedIndices.Length);
				int num = 0;
				for (int i = 0; i < this.SrcEntityComponentStore->m_Archetypes.Length; i++)
				{
					Archetype* ptr = *(IntPtr*)(this.SrcEntityComponentStore->m_Archetypes.Ptr + (IntPtr)i * (IntPtr)sizeof(Archetype*) / (IntPtr)sizeof(Archetype*));
					for (int j = 0; j < ptr->Chunks.Count; j++)
					{
						ChunkIndex chunkIndex = ptr->Chunks[j];
						short firstManagedComponent = ptr->FirstManagedComponent;
						int numManagedComponents = ptr->NumManagedComponents;
						for (int k = 0; k < numManagedComponents; k++)
						{
							int num2 = k + (int)firstManagedComponent;
							int* componentDataRO = (int*)ChunkDataUtility.GetComponentDataRO(chunkIndex, ptr, 0, num2);
							int l = 0;
							int count = chunkIndex.Count;
							while (l < count)
							{
								int num3 = componentDataRO[l];
								if (num3 != 0)
								{
									this.SrcManagedIndices[num] = num3;
									componentDataRO[l] = this.DstManagedIndices[num++];
								}
								l++;
							}
						}
					}
				}
			}

			// Token: 0x0400047D RID: 1149
			[NativeDisableUnsafePtrRestriction]
			public unsafe EntityComponentStore* SrcEntityComponentStore;

			// Token: 0x0400047E RID: 1150
			[NativeDisableUnsafePtrRestriction]
			public unsafe EntityComponentStore* DstEntityComponentStore;

			// Token: 0x0400047F RID: 1151
			public NativeArray<int> SrcManagedIndices;

			// Token: 0x04000480 RID: 1152
			public NativeArray<int> DstManagedIndices;
		}

		// Token: 0x0200017B RID: 379
		[BurstCompile]
		private struct GatherManagedComponentIndicesInChunkJob : IJob
		{
			// Token: 0x0600098F RID: 2447 RVA: 0x000300CC File Offset: 0x0002E2CC
			public unsafe void Execute()
			{
				int length = this.DstManagedIndices.Length;
				this.DstEntityComponentStore->AllocateManagedComponentIndices((int*)this.DstManagedIndices.GetUnsafePtr<int>(), length);
				int num = 0;
				for (int i = 0; i < this.Chunks.Length; i++)
				{
					ChunkIndex chunkIndex = this.Chunks[i];
					Archetype* archetype = this.SrcEntityComponentStore->GetArchetype(chunkIndex);
					short firstManagedComponent = archetype->FirstManagedComponent;
					int numManagedComponents = archetype->NumManagedComponents;
					for (int j = 0; j < numManagedComponents; j++)
					{
						int num2 = j + (int)firstManagedComponent;
						int* componentDataRO = (int*)ChunkDataUtility.GetComponentDataRO(chunkIndex, archetype, 0, num2);
						int k = 0;
						int count = chunkIndex.Count;
						while (k < count)
						{
							int num3 = componentDataRO[k];
							if (num3 != 0)
							{
								this.SrcManagedIndices[num] = num3;
								componentDataRO[k] = this.DstManagedIndices[num++];
							}
							k++;
						}
					}
				}
				if (num < length)
				{
					this.DstEntityComponentStore->m_ManagedComponentFreeIndex.Add((void*)((byte*)this.DstManagedIndices.GetUnsafePtr<int>() + (IntPtr)num * 4), (length - num) * 4);
				}
				*this.NonNullCount = num;
			}

			// Token: 0x04000481 RID: 1153
			[NativeDisableUnsafePtrRestriction]
			public unsafe EntityComponentStore* SrcEntityComponentStore;

			// Token: 0x04000482 RID: 1154
			[NativeDisableUnsafePtrRestriction]
			public unsafe EntityComponentStore* DstEntityComponentStore;

			// Token: 0x04000483 RID: 1155
			public NativeArray<ChunkIndex> Chunks;

			// Token: 0x04000484 RID: 1156
			public NativeArray<int> SrcManagedIndices;

			// Token: 0x04000485 RID: 1157
			public NativeArray<int> DstManagedIndices;

			// Token: 0x04000486 RID: 1158
			[NativeDisableUnsafePtrRestriction]
			public unsafe int* NonNullCount;
		}

		// Token: 0x0200017C RID: 380
		[BurstCompile]
		private struct RemapChunksFilteredJob : IJobParallelFor
		{
			// Token: 0x06000990 RID: 2448 RVA: 0x000301F0 File Offset: 0x0002E3F0
			public unsafe void Execute(int index)
			{
				ChunkIndex chunk = this.remapChunks[index].chunk;
				Archetype* dstArchetype = this.remapChunks[index].dstArchetype;
				int count = chunk.Count;
				this.dstEntityComponentStore->RemapChunk(chunk, 0, count, ref this.entityRemapping);
				EntityRemapUtility.PatchEntities(dstArchetype->ScalarEntityPatches + 1, dstArchetype->ScalarEntityPatchCount - 1, dstArchetype->BufferEntityPatches, dstArchetype->BufferEntityPatchCount, chunk.Buffer, count, ref this.entityRemapping);
				if (dstArchetype->HasChunkComponents)
				{
					Archetype* metaChunkArchetype = dstArchetype->MetaChunkArchetype;
					int indexInTypeArray = ChunkDataUtility.GetIndexInTypeArray(metaChunkArchetype, this.chunkHeaderType);
					int num = metaChunkArchetype->Offsets[indexInTypeArray];
					int num2 = sizeof(ChunkHeader);
					Entity metaChunkEntity = chunk.MetaChunkEntity;
					EntityInChunk entityInChunk = this.dstEntityComponentStore->GetEntityInChunk(metaChunkEntity);
					ChunkHeader* ptr = (ChunkHeader*)(entityInChunk.Chunk.Buffer + (num + num2 * entityInChunk.IndexInChunk));
					ptr->ArchetypeChunk = new ArchetypeChunk(chunk, this.dstEntityComponentStore);
				}
			}

			// Token: 0x04000487 RID: 1159
			[ReadOnly]
			public NativeArray<EntityRemapUtility.EntityRemapInfo> entityRemapping;

			// Token: 0x04000488 RID: 1160
			[ReadOnly]
			public NativeArray<EntityManager.RemapChunk> remapChunks;

			// Token: 0x04000489 RID: 1161
			[NativeDisableUnsafePtrRestriction]
			public unsafe EntityComponentStore* dstEntityComponentStore;

			// Token: 0x0400048A RID: 1162
			public TypeIndex chunkHeaderType;
		}

		// Token: 0x0200017D RID: 381
		[BurstCompile]
		private struct MoveFilteredChunksBetweenArchetypeJob : IJob
		{
			// Token: 0x06000991 RID: 2449 RVA: 0x000302E8 File Offset: 0x0002E4E8
			public unsafe void Execute()
			{
				int* ptr = stackalloc int[(UIntPtr)64];
				int length = this.RemapChunks.Length;
				for (int i = 0; i < length; i++)
				{
					ChunkIndex chunk = this.RemapChunks[i].chunk;
					Archetype* archetype = this.SrcEntityComponentStore->GetArchetype(chunk);
					Archetype* dstArchetype = this.RemapChunks[i].dstArchetype;
					int numSharedComponents = dstArchetype->NumSharedComponents;
					SharedComponentValues sharedComponentValues = archetype->Chunks.GetSharedComponentValues(chunk.ListIndex);
					if (numSharedComponents != 0)
					{
						for (int j = 0; j < numSharedComponents; j++)
						{
							int num = *sharedComponentValues[j];
							if (EntityComponentStore.GetElementIndexFromSharedComponentIndex(num) != 0)
							{
								ptr[j] = this.RemapShared[num];
							}
							else
							{
								ptr[j] = 0;
							}
						}
						sharedComponentValues = ptr;
					}
					ChunkDataUtility.MoveArchetype(archetype, chunk, dstArchetype, sharedComponentValues);
				}
			}

			// Token: 0x0400048B RID: 1163
			[ReadOnly]
			public NativeArray<EntityManager.RemapChunk> RemapChunks;

			// Token: 0x0400048C RID: 1164
			[ReadOnly]
			public NativeHashMap<int, int> RemapShared;

			// Token: 0x0400048D RID: 1165
			[NativeDisableUnsafePtrRestriction]
			public unsafe EntityComponentStore* SrcEntityComponentStore;
		}

		// Token: 0x0200017E RID: 382
		[BurstCompile]
		private struct RemapAllChunksJob : IJobParallelFor
		{
			// Token: 0x06000992 RID: 2450 RVA: 0x000303C8 File Offset: 0x0002E5C8
			public unsafe void Execute(int index)
			{
				ChunkIndex chunk = this.remapChunks[index].chunk;
				Archetype* dstArchetype = this.remapChunks[index].dstArchetype;
				int count = chunk.Count;
				this.dstEntityComponentStore->RemapChunk(chunk, 0, count, ref this.entityRemapping);
				EntityRemapUtility.PatchEntities(dstArchetype->ScalarEntityPatches + 1, dstArchetype->ScalarEntityPatchCount - 1, dstArchetype->BufferEntityPatches, dstArchetype->BufferEntityPatchCount, chunk.Buffer, count, ref this.entityRemapping);
				this.dstEntityComponentStore->SetArchetype(chunk, dstArchetype);
				chunk.ListIndex += dstArchetype->Chunks.Count;
				chunk.ListWithEmptySlotsIndex += dstArchetype->ChunksWithEmptySlots.Length;
			}

			// Token: 0x0400048E RID: 1166
			[ReadOnly]
			public NativeArray<EntityRemapUtility.EntityRemapInfo> entityRemapping;

			// Token: 0x0400048F RID: 1167
			[ReadOnly]
			public NativeArray<EntityManager.RemapChunk> remapChunks;

			// Token: 0x04000490 RID: 1168
			[NativeDisableUnsafePtrRestriction]
			public unsafe EntityComponentStore* dstEntityComponentStore;
		}

		// Token: 0x0200017F RID: 383
		[BurstCompile]
		private struct RemapAllArchetypesJob : IJobParallelFor
		{
			// Token: 0x06000993 RID: 2451 RVA: 0x00030488 File Offset: 0x0002E688
			public unsafe void Execute(int index)
			{
				Archetype* srcArchetype = this.remapArchetypes[index].srcArchetype;
				int count = srcArchetype->Chunks.Count;
				Archetype* dstArchetype = this.remapArchetypes[index].dstArchetype;
				int count2 = dstArchetype->Chunks.Count;
				dstArchetype->Chunks.MoveChunks(in srcArchetype->Chunks);
				if (srcArchetype->NumSharedComponents == 0)
				{
					if (srcArchetype->ChunksWithEmptySlots.Length != 0)
					{
						dstArchetype->ChunksWithEmptySlots.SetCapacity(srcArchetype->ChunksWithEmptySlots.Length + dstArchetype->ChunksWithEmptySlots.Length);
						dstArchetype->ChunksWithEmptySlots.AddRange(srcArchetype->ChunksWithEmptySlots);
						srcArchetype->ChunksWithEmptySlots.Resize(0, NativeArrayOptions.UninitializedMemory);
					}
				}
				else
				{
					for (int i = 0; i < dstArchetype->NumSharedComponents; i++)
					{
						int* sharedComponentValueArrayForType = srcArchetype->Chunks.GetSharedComponentValueArrayForType(i);
						int* ptr = dstArchetype->Chunks.GetSharedComponentValueArrayForType(i) + count2;
						for (int j = 0; j < count; j++)
						{
							int num = sharedComponentValueArrayForType[j];
							if (EntityComponentStore.GetElementIndexFromSharedComponentIndex(num) != 0)
							{
								int num2 = this.remapShared[num];
								ptr[j] = num2;
							}
							else
							{
								ptr[j] = 0;
							}
						}
					}
					int chunkCapacity = dstArchetype->ChunkCapacity;
					for (int k = 0; k < count; k++)
					{
						ChunkIndex chunkIndex = dstArchetype->Chunks[k + count2];
						if (chunkIndex.Count < chunkCapacity)
						{
							dstArchetype->FreeChunksBySharedComponents.Add(chunkIndex);
						}
					}
					srcArchetype->FreeChunksBySharedComponents.Init(srcArchetype, 16);
				}
				uint globalSystemVersion = this.dstEntityComponentStore->GlobalSystemVersion;
				for (int l = 0; l < dstArchetype->TypesCount; l++)
				{
					uint* ptr2 = dstArchetype->Chunks.GetChangeVersionArrayForType(l) + count2;
					for (int m = 0; m < count; m++)
					{
						ptr2[m] = globalSystemVersion;
					}
				}
				UnsafeUtility.MemCpy((void*)(dstArchetype->Chunks.GetChunkEntityCountArray() + count2), (void*)srcArchetype->Chunks.GetChunkEntityCountArray(), (long)(4 * count));
				byte* pointerToComponentEnabledArrayForArchetype = srcArchetype->Chunks.GetPointerToComponentEnabledArrayForArchetype();
				byte* componentEnabledMaskArrayForChunk = (byte*)dstArchetype->Chunks.GetComponentEnabledMaskArrayForChunk(count2);
				long num3 = (long)(srcArchetype->Chunks.ComponentEnabledBitsSizeTotalPerChunk * (ulong)((long)count));
				UnsafeUtility.MemCpy((void*)componentEnabledMaskArrayForChunk, (void*)pointerToComponentEnabledArrayForArchetype, num3);
				for (int n = 0; n < count; n++)
				{
					int num4 = count2 + n;
					dstArchetype->Chunks.InitializeDisabledCountForChunk(num4);
					int count3 = dstArchetype->Chunks[num4].Count;
					for (int num5 = 0; num5 < dstArchetype->EnableableTypesCount; num5++)
					{
						int num6 = dstArchetype->EnableableTypeIndexInArchetype[num5];
						int num7 = dstArchetype->TypeIndexInArchetypeToMemoryOrderIndex[num6];
						int num8 = count3 - dstArchetype->Chunks.GetEnabledArrayForTypeInChunk(num7, num4).CountBits(0, count3);
						dstArchetype->Chunks.SetChunkDisabledCountForType(num7, num4, num8);
					}
				}
				if (dstArchetype->HasChunkComponents)
				{
					Archetype* metaChunkArchetype = dstArchetype->MetaChunkArchetype;
					int indexInTypeArray = ChunkDataUtility.GetIndexInTypeArray(metaChunkArchetype, this.chunkHeaderType);
					int num9 = metaChunkArchetype->Offsets[indexInTypeArray];
					ushort num10 = metaChunkArchetype->SizeOfs[indexInTypeArray];
					for (int num11 = 0; num11 < count; num11++)
					{
						ChunkIndex chunkIndex2 = dstArchetype->Chunks[num11 + count2];
						Entity metaChunkEntity = chunkIndex2.MetaChunkEntity;
						EntityInChunk entityInChunk = this.dstEntityComponentStore->GetEntityInChunk(metaChunkEntity);
						ChunkHeader* ptr3 = (ChunkHeader*)(entityInChunk.Chunk.Buffer + (num9 + (int)num10 * entityInChunk.IndexInChunk));
						ptr3->ArchetypeChunk = new ArchetypeChunk(chunkIndex2, this.dstEntityComponentStore);
					}
				}
				dstArchetype->EntityCount += srcArchetype->EntityCount;
				srcArchetype->Chunks.Dispose();
				srcArchetype->EntityCount = 0;
			}

			// Token: 0x04000491 RID: 1169
			[DeallocateOnJobCompletion]
			[ReadOnly]
			public NativeArray<EntityManager.RemapArchetype> remapArchetypes;

			// Token: 0x04000492 RID: 1170
			[NativeDisableUnsafePtrRestriction]
			public unsafe EntityComponentStore* dstEntityComponentStore;

			// Token: 0x04000493 RID: 1171
			[ReadOnly]
			public NativeParallelHashMap<int, int> remapShared;

			// Token: 0x04000494 RID: 1172
			public TypeIndex chunkHeaderType;
		}

		// Token: 0x02000180 RID: 384
		[BurstCompile]
		private struct MoveAllChunksJob : IJob
		{
			// Token: 0x06000994 RID: 2452 RVA: 0x00030827 File Offset: 0x0002EA27
			public unsafe void Execute()
			{
				this.dstEntityComponentStore->AllocateEntitiesForRemapping(this.srcEntityComponentStore, ref this.entityRemapping);
			}

			// Token: 0x04000495 RID: 1173
			[NativeDisableUnsafePtrRestriction]
			public unsafe EntityComponentStore* srcEntityComponentStore;

			// Token: 0x04000496 RID: 1174
			[NativeDisableUnsafePtrRestriction]
			public unsafe EntityComponentStore* dstEntityComponentStore;

			// Token: 0x04000497 RID: 1175
			public NativeArray<EntityRemapUtility.EntityRemapInfo> entityRemapping;
		}

		// Token: 0x02000181 RID: 385
		[BurstCompile]
		internal struct FreeAllChunksJob : IJob
		{
			// Token: 0x06000995 RID: 2453 RVA: 0x00030840 File Offset: 0x0002EA40
			public unsafe void Execute()
			{
				this.EntityComponentStore->FreeAllEntities();
			}

			// Token: 0x04000498 RID: 1176
			[NativeDisableUnsafePtrRestriction]
			public unsafe EntityComponentStore* EntityComponentStore;
		}

		// Token: 0x02000182 RID: 386
		[Flags]
		public enum GetAllEntitiesOptions
		{
			// Token: 0x0400049A RID: 1178
			Default = 0,
			// Token: 0x0400049B RID: 1179
			IncludeMeta = 1,
			// Token: 0x0400049C RID: 1180
			IncludeSystems = 2
		}

		// Token: 0x02000183 RID: 387
		public readonly struct EntityManagerDebug
		{
			// Token: 0x06000996 RID: 2454 RVA: 0x0003084D File Offset: 0x0002EA4D
			public EntityManagerDebug(EntityManager entityManager)
			{
				this.m_Manager = entityManager;
			}

			// Token: 0x06000997 RID: 2455 RVA: 0x00030858 File Offset: 0x0002EA58
			public unsafe void PoisonUnusedDataInAllChunks(EntityArchetype archetype, byte value)
			{
				for (int i = 0; i < archetype.Archetype->Chunks.Count; i++)
				{
					ChunkIndex chunkIndex = archetype.Archetype->Chunks[i];
					ChunkDataUtility.MemsetUnusedChunkData(archetype.Archetype, chunkIndex.Buffer, value, chunkIndex.Count);
				}
			}

			// Token: 0x06000998 RID: 2456 RVA: 0x000308AC File Offset: 0x0002EAAC
			internal unsafe void IncrementGlobalSystemVersion(in SystemHandle handle = null)
			{
				this.m_Manager.GetCheckedEntityDataAccess()->EntityComponentStore->IncrementGlobalSystemVersion(in handle);
			}

			// Token: 0x06000999 RID: 2457 RVA: 0x000308D4 File Offset: 0x0002EAD4
			internal unsafe void SetGlobalSystemVersion(uint version, in SystemHandle handle = null)
			{
				this.m_Manager.GetCheckedEntityDataAccess()->EntityComponentStore->SetGlobalSystemVersion(version, in handle);
			}

			// Token: 0x0600099A RID: 2458 RVA: 0x000308FC File Offset: 0x0002EAFC
			public unsafe bool IsSharedComponentManagerEmpty()
			{
				return this.m_Manager.GetCheckedEntityDataAccess()->ManagedComponentStore.IsEmpty();
			}

			// Token: 0x0600099B RID: 2459 RVA: 0x00030924 File Offset: 0x0002EB24
			internal unsafe static string GetArchetypeDebugString(Archetype* a)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("(");
				for (int i = 0; i < a->TypesCount; i++)
				{
					ComponentTypeInArchetype componentTypeInArchetype = a->Types[i];
					if (i > 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(componentTypeInArchetype.ToString());
				}
				stringBuilder.Append(")");
				return stringBuilder.ToString();
			}

			// Token: 0x170000C0 RID: 192
			// (get) Token: 0x0600099C RID: 2460 RVA: 0x000309A0 File Offset: 0x0002EBA0
			public int EntityCount
			{
				get
				{
					NativeArray<Entity> allEntities = this.m_Manager.GetAllEntities(Allocator.Temp);
					int length = allEntities.Length;
					allEntities.Dispose();
					return length;
				}
			}

			// Token: 0x170000C1 RID: 193
			// (get) Token: 0x0600099D RID: 2461 RVA: 0x000309CC File Offset: 0x0002EBCC
			public unsafe int HighestIndexEntity
			{
				get
				{
					int num = -1;
					UnsafePtrList<Archetype> archetypes = this.m_Manager.GetCheckedEntityDataAccess()->EntityComponentStore->m_Archetypes;
					int i = 0;
					int length = archetypes.Length;
					while (i < length)
					{
						ArchetypeChunkData chunks = archetypes[i]->Chunks;
						int j = 0;
						int count = chunks.Count;
						while (j < count)
						{
							ChunkIndex chunkIndex = chunks[j];
							Entity* buffer = (Entity*)chunkIndex.Buffer;
							int k = 0;
							int count2 = chunkIndex.Count;
							while (k < count2)
							{
								int index = buffer[k].Index;
								num = ((index > num) ? index : num);
								k++;
							}
							j++;
						}
						i++;
					}
					return num;
				}
			}

			// Token: 0x170000C2 RID: 194
			// (get) Token: 0x0600099E RID: 2462 RVA: 0x00030A84 File Offset: 0x0002EC84
			// (set) Token: 0x0600099F RID: 2463 RVA: 0x00030AAC File Offset: 0x0002ECAC
			public unsafe bool UseMemoryInitPattern
			{
				get
				{
					return this.m_Manager.GetCheckedEntityDataAccess()->EntityComponentStore->useMemoryInitPattern > 0;
				}
				set
				{
					this.m_Manager.GetCheckedEntityDataAccess()->EntityComponentStore->useMemoryInitPattern = (value ? 1 : 0);
				}
			}

			// Token: 0x170000C3 RID: 195
			// (get) Token: 0x060009A0 RID: 2464 RVA: 0x00030AD8 File Offset: 0x0002ECD8
			// (set) Token: 0x060009A1 RID: 2465 RVA: 0x00030B00 File Offset: 0x0002ED00
			public unsafe byte MemoryInitPattern
			{
				get
				{
					return this.m_Manager.GetCheckedEntityDataAccess()->EntityComponentStore->memoryInitPattern;
				}
				set
				{
					this.m_Manager.GetCheckedEntityDataAccess()->EntityComponentStore->memoryInitPattern = value;
				}
			}

			// Token: 0x060009A2 RID: 2466 RVA: 0x00030B28 File Offset: 0x0002ED28
			internal Entity GetMetaChunkEntity(Entity entity)
			{
				return this.m_Manager.GetChunk(entity).m_Chunk.MetaChunkEntity;
			}

			// Token: 0x060009A3 RID: 2467 RVA: 0x00030B51 File Offset: 0x0002ED51
			internal Entity GetMetaChunkEntity(ArchetypeChunk chunk)
			{
				return chunk.m_Chunk.MetaChunkEntity;
			}

			// Token: 0x060009A4 RID: 2468 RVA: 0x00030B60 File Offset: 0x0002ED60
			public unsafe static string GetSystemProfilerMarkerName(World world, SystemHandle system)
			{
				SystemState* ptr = world.Unmanaged.ResolveSystemState(system);
				if (ptr == null)
				{
					return null;
				}
				return ptr->GetProfilerMarkerName(world);
			}

			// Token: 0x060009A5 RID: 2469 RVA: 0x00030B8B File Offset: 0x0002ED8B
			public void LogEntityInfo(Entity entity)
			{
				Unity.Debug.Log(this.GetEntityInfo(entity));
			}

			// Token: 0x060009A6 RID: 2470 RVA: 0x00030B9C File Offset: 0x0002ED9C
			public unsafe string GetEntityInfo(Entity entity)
			{
				EntityComponentStore* entityComponentStore = this.m_Manager.GetCheckedEntityDataAccess()->EntityComponentStore;
				if (entity.Index < 0)
				{
					return "Entity.Invalid";
				}
				StringBuilder stringBuilder = new StringBuilder();
				this.GetEntityInfo(entity, false, stringBuilder);
				return stringBuilder.ToString();
			}

			// Token: 0x060009A7 RID: 2471 RVA: 0x00030BE4 File Offset: 0x0002EDE4
			public unsafe static string GetLastWriterSystemName(ArchetypeChunk chunk, ComponentType componentType)
			{
				Archetype* archetype = chunk.Archetype.Archetype;
				int indexInTypeArray = ChunkDataUtility.GetIndexInTypeArray(archetype, componentType.TypeIndex);
				if (indexInTypeArray == -1)
				{
					return string.Format("'{0}' was not present on the chunk.", componentType);
				}
				uint changeVersion = archetype->Chunks.GetChangeVersion(indexInTypeArray, chunk.m_Chunk.ListIndex);
				SystemState* ptr = World.FindSystemStateForChangeVersion(chunk.m_EntityComponentStore, changeVersion);
				if (ptr == null)
				{
					return "Couldn't find the system that modified the chunk.";
				}
				return ptr->DebugName.ToString();
			}

			// Token: 0x060009A8 RID: 2472 RVA: 0x00030C68 File Offset: 0x0002EE68
			internal unsafe void GetEntityInfo(Entity entity, bool includeComponentValues, StringBuilder str)
			{
				Archetype* archetype = this.m_Manager.GetCheckedEntityDataAccess()->EntityComponentStore->GetArchetype(entity);
				str.Append(entity.ToString());
				for (int i = 0; i < archetype->TypesCount; i++)
				{
					ComponentTypeInArchetype componentTypeInArchetype = archetype->Types[i];
					str.AppendFormat("  - {0}", componentTypeInArchetype.ToString());
				}
			}

			// Token: 0x060009A9 RID: 2473 RVA: 0x00030CE4 File Offset: 0x0002EEE4
			public unsafe object GetComponentBoxed(Entity entity, ComponentType type)
			{
				EntityDataAccess* checkedEntityDataAccess = this.m_Manager.GetCheckedEntityDataAccess();
				if (!checkedEntityDataAccess->HasComponent(entity, type))
				{
					throw new ArgumentException(string.Format("Component of type {0} does not exist on the entity.", type));
				}
				return new DebuggerDataAccess(checkedEntityDataAccess->EntityComponentStore).GetComponentBoxedUnchecked(entity, type);
			}

			// Token: 0x060009AA RID: 2474 RVA: 0x00030D34 File Offset: 0x0002EF34
			public unsafe object GetComponentBoxed(Entity entity, Type type)
			{
				EntityDataAccess* checkedEntityDataAccess = this.m_Manager.GetCheckedEntityDataAccess();
				TypeIndex typeIndexFromType = ChunkDataUtility.GetTypeIndexFromType(checkedEntityDataAccess->EntityComponentStore->GetArchetype(entity), type);
				return new DebuggerDataAccess(checkedEntityDataAccess->EntityComponentStore).GetComponentBoxedUnchecked(entity, ComponentType.FromTypeIndex(typeIndexFromType));
			}

			// Token: 0x060009AB RID: 2475 RVA: 0x00002287 File Offset: 0x00000487
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			[Conditional("UNITY_DOTS_DEBUG")]
			public void CheckInternalConsistency()
			{
			}

			// Token: 0x0400049D RID: 1181
			private readonly EntityManager m_Manager;
		}

		// Token: 0x02000184 RID: 388
		// (Invoke) Token: 0x060009AD RID: 2477
		internal unsafe delegate void RemapChunksForFilteredMove_0000089C$PostfixBurstDelegate(ref NativeArray<ArchetypeChunk> chunks, ref NativeArray<EntityManager.RemapChunk> remapChunks, ref NativeArray<EntityRemapUtility.EntityRemapInfo> entityRemapping, ref NativeList<ChunkIndex> managedChunks, out int managedComponentCount, EntityComponentStore* dstEntityComponentStore, EntityComponentStore* srcEntityComponentStore);

		// Token: 0x02000185 RID: 389
		internal static class RemapChunksForFilteredMove_0000089C$BurstDirectCall
		{
			// Token: 0x060009B0 RID: 2480 RVA: 0x00030D7B File Offset: 0x0002EF7B
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (EntityManager.RemapChunksForFilteredMove_0000089C$BurstDirectCall.Pointer == 0)
				{
					EntityManager.RemapChunksForFilteredMove_0000089C$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(EntityManager.RemapChunksForFilteredMove_0000089C$BurstDirectCall.DeferredCompilation, methodof(EntityManager.RemapChunksForFilteredMove$BurstManaged(ref NativeArray<ArchetypeChunk>, ref NativeArray<EntityManager.RemapChunk>, ref NativeArray<EntityRemapUtility.EntityRemapInfo>, ref NativeList<ChunkIndex>, ref int, EntityComponentStore*, EntityComponentStore*)).MethodHandle, typeof(EntityManager.RemapChunksForFilteredMove_0000089C$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = EntityManager.RemapChunksForFilteredMove_0000089C$BurstDirectCall.Pointer;
			}

			// Token: 0x060009B1 RID: 2481 RVA: 0x00030DA8 File Offset: 0x0002EFA8
			private static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				EntityManager.RemapChunksForFilteredMove_0000089C$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x060009B2 RID: 2482 RVA: 0x00030DC0 File Offset: 0x0002EFC0
			public unsafe static void Constructor()
			{
				EntityManager.RemapChunksForFilteredMove_0000089C$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(EntityManager.RemapChunksForFilteredMove(ref NativeArray<ArchetypeChunk>, ref NativeArray<EntityManager.RemapChunk>, ref NativeArray<EntityRemapUtility.EntityRemapInfo>, ref NativeList<ChunkIndex>, ref int, EntityComponentStore*, EntityComponentStore*)).MethodHandle);
			}

			// Token: 0x060009B3 RID: 2483 RVA: 0x00002287 File Offset: 0x00000487
			public static void Initialize()
			{
			}

			// Token: 0x060009B4 RID: 2484 RVA: 0x00030DD1 File Offset: 0x0002EFD1
			// Note: this type is marked as 'beforefieldinit'.
			static RemapChunksForFilteredMove_0000089C$BurstDirectCall()
			{
				EntityManager.RemapChunksForFilteredMove_0000089C$BurstDirectCall.Constructor();
			}

			// Token: 0x060009B5 RID: 2485 RVA: 0x00030DD8 File Offset: 0x0002EFD8
			public unsafe static void Invoke(ref NativeArray<ArchetypeChunk> chunks, ref NativeArray<EntityManager.RemapChunk> remapChunks, ref NativeArray<EntityRemapUtility.EntityRemapInfo> entityRemapping, ref NativeList<ChunkIndex> managedChunks, out int managedComponentCount, EntityComponentStore* dstEntityComponentStore, EntityComponentStore* srcEntityComponentStore)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = EntityManager.RemapChunksForFilteredMove_0000089C$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Collections.NativeArray`1<Unity.Entities.ArchetypeChunk>&,Unity.Collections.NativeArray`1<Unity.Entities.EntityManager/RemapChunk>&,Unity.Collections.NativeArray`1<Unity.Entities.EntityRemapUtility/EntityRemapInfo>&,Unity.Collections.NativeList`1<Unity.Entities.ChunkIndex>&,System.Int32&,Unity.Entities.EntityComponentStore*,Unity.Entities.EntityComponentStore*), ref chunks, ref remapChunks, ref entityRemapping, ref managedChunks, ref managedComponentCount, dstEntityComponentStore, srcEntityComponentStore, functionPointer);
						return;
					}
				}
				EntityManager.RemapChunksForFilteredMove$BurstManaged(ref chunks, ref remapChunks, ref entityRemapping, ref managedChunks, out managedComponentCount, dstEntityComponentStore, srcEntityComponentStore);
			}

			// Token: 0x0400049E RID: 1182
			private static IntPtr Pointer;

			// Token: 0x0400049F RID: 1183
			private static IntPtr DeferredCompilation;
		}
	}
}
