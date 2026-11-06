using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Core;
using UnityEngine.Scripting;

namespace Unity.Entities
{
	// Token: 0x02000037 RID: 55
	[RequireDerived]
	public abstract class ComponentSystemBase
	{
		// Token: 0x06000159 RID: 345 RVA: 0x0000222E File Offset: 0x0000042E
		[RequiredMember]
		public ComponentSystemBase()
		{
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600015A RID: 346 RVA: 0x00007895 File Offset: 0x00005A95
		private SystemState_ SystemState
		{
			get
			{
				return SystemState_.FromPointer(this.m_StatePtr);
			}
		}

		// Token: 0x0600015B RID: 347 RVA: 0x000078A2 File Offset: 0x00005AA2
		internal unsafe SystemState* CheckedState()
		{
			SystemState* statePtr = this.m_StatePtr;
			if (statePtr == null)
			{
				throw new InvalidOperationException("system state is not initialized or has already been destroyed");
			}
			return statePtr;
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600015C RID: 348 RVA: 0x000078BA File Offset: 0x00005ABA
		// (set) Token: 0x0600015D RID: 349 RVA: 0x000078C7 File Offset: 0x00005AC7
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public unsafe bool Enabled
		{
			get
			{
				return this.CheckedState()->Enabled;
			}
			set
			{
				this.CheckedState()->Enabled = value;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600015E RID: 350 RVA: 0x000078D5 File Offset: 0x00005AD5
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public unsafe EntityQuery[] EntityQueries
		{
			get
			{
				return ComponentSystemBase.UnsafeListToRefArray(this.CheckedState()->EntityQueries);
			}
		}

		// Token: 0x0600015F RID: 351 RVA: 0x000078E8 File Offset: 0x00005AE8
		internal unsafe static EntityQuery[] UnsafeListToRefArray(ref UnsafeList<EntityQuery> objs)
		{
			EntityQuery[] array = new EntityQuery[objs.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = objs.Ptr[i];
			}
			return array;
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000160 RID: 352 RVA: 0x0000792C File Offset: 0x00005B2C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public uint GlobalSystemVersion
		{
			get
			{
				return this.EntityManager.GlobalSystemVersion;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000161 RID: 353 RVA: 0x00007947 File Offset: 0x00005B47
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public unsafe uint LastSystemVersion
		{
			get
			{
				return this.CheckedState()->m_LastSystemVersion;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000162 RID: 354 RVA: 0x00007954 File Offset: 0x00005B54
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public unsafe EntityManager EntityManager
		{
			get
			{
				return this.CheckedState()->m_EntityManager;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000163 RID: 355 RVA: 0x00007961 File Offset: 0x00005B61
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public unsafe World World
		{
			get
			{
				if (this.m_StatePtr == null)
				{
					return null;
				}
				return (World)this.m_StatePtr->m_World.Target;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000164 RID: 356 RVA: 0x00007984 File Offset: 0x00005B84
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public unsafe SystemHandle SystemHandle
		{
			get
			{
				if (this.m_StatePtr == null)
				{
					return default(SystemHandle);
				}
				return this.m_StatePtr->SystemHandle;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000165 RID: 357 RVA: 0x000079B0 File Offset: 0x00005BB0
		[Obsolete("SystemHandleUntyped has been renamed to SystemHandle. (UnityUpgradable) -> SystemHandle", true)]
		public SystemHandle SystemHandleUntyped
		{
			get
			{
				return this.SystemHandle;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000166 RID: 358 RVA: 0x000079B8 File Offset: 0x00005BB8
		[Obsolete("Time has been deprecated as duplicate. Use SystemAPI.Time or World.Time instead (RemovedAfter 2023-08-08)", true)]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public readonly ref TimeData Time
		{
			get
			{
				return this.World.Time;
			}
		}

		// Token: 0x06000167 RID: 359 RVA: 0x000079C5 File Offset: 0x00005BC5
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void CheckSystemState()
		{
			if (this.m_StatePtr == null)
			{
				throw new InvalidOperationException("System state pointer is null.");
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000168 RID: 360 RVA: 0x000079DC File Offset: 0x00005BDC
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public unsafe Allocator WorldUpdateAllocator
		{
			get
			{
				return this.m_StatePtr->WorldUpdateAllocator;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000169 RID: 361 RVA: 0x000079E9 File Offset: 0x00005BE9
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal unsafe ref RewindableAllocator WorldRewindableAllocator
		{
			get
			{
				return this.m_StatePtr->WorldRewindableAllocator;
			}
		}

		// Token: 0x0600016A RID: 362 RVA: 0x000079F8 File Offset: 0x00005BF8
		internal unsafe void CreateInstance(World world)
		{
			ref WorldUnmanagedImpl impl = ref this.World.Unmanaged.GetImpl();
			WorldUnmanagedImpl.PreviousSystemGlobalState previousSystemGlobalState = new WorldUnmanagedImpl.PreviousSystemGlobalState(ref impl, this.m_StatePtr);
			this.OnBeforeCreateInternal(world);
			try
			{
				this.m_StatePtr->m_EntityComponentStore->IncrementGlobalSystemVersion(in this.m_StatePtr->m_Handle);
				this.OnCreateForCompiler();
				this.OnCreate();
				previousSystemGlobalState.Restore(ref impl, this.m_StatePtr);
				ref EntityComponentStore entityComponentStore = ref *this.m_StatePtr->m_EntityComponentStore;
				SystemHandle systemHandle = default(SystemHandle);
				entityComponentStore.IncrementGlobalSystemVersion(in systemHandle);
			}
			catch
			{
				previousSystemGlobalState.Restore(ref impl, this.m_StatePtr);
				this.OnBeforeDestroyInternal();
				this.OnAfterDestroyInternal();
				throw;
			}
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00002287 File Offset: 0x00000487
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected virtual void OnCreateForCompiler()
		{
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00007AAC File Offset: 0x00005CAC
		internal void DestroyInstance()
		{
			WorldUnmanagedImpl.PreviousSystemGlobalState previousSystemGlobalState = new WorldUnmanagedImpl.PreviousSystemGlobalState(this.World.Unmanaged.GetImpl(), this.m_StatePtr);
			try
			{
				this.OnBeforeDestroyInternal();
				this.OnDestroy();
			}
			finally
			{
				previousSystemGlobalState.Restore(this.World.Unmanaged.GetImpl(), this.m_StatePtr);
				this.OnAfterDestroyInternal();
			}
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00002287 File Offset: 0x00000487
		[RequiredMember]
		protected virtual void OnCreate()
		{
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00002287 File Offset: 0x00000487
		[RequiredMember]
		protected virtual void OnStartRunning()
		{
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00002287 File Offset: 0x00000487
		[RequiredMember]
		protected virtual void OnStopRunning()
		{
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00007B20 File Offset: 0x00005D20
		internal virtual void OnStopRunningInternal()
		{
			this.OnStopRunning();
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00002287 File Offset: 0x00000487
		[RequiredMember]
		protected virtual void OnDestroy()
		{
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00007B28 File Offset: 0x00005D28
		internal void OnDestroy_Internal()
		{
			this.OnDestroy();
		}

		// Token: 0x06000173 RID: 371
		public abstract void Update();

		// Token: 0x06000174 RID: 372 RVA: 0x00002287 File Offset: 0x00000487
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void CheckExists()
		{
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00007B30 File Offset: 0x00005D30
		public unsafe bool ShouldRunSystem()
		{
			return this.CheckedState()->ShouldRunSystem();
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00002287 File Offset: 0x00000487
		internal virtual void OnBeforeCreateInternal(World world)
		{
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00007B40 File Offset: 0x00005D40
		internal unsafe void OnAfterDestroyInternal()
		{
			SystemState* ptr = this.CheckedState();
			this.World.Unmanaged.DestroyManagedSystemState(ptr);
			this.m_StatePtr = null;
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00007B70 File Offset: 0x00005D70
		private static void DisposeQueries(ref UnsafeList<GCHandle> queries)
		{
			for (int i = 0; i < queries.Length; i++)
			{
				((EntityQuery)queries[i].Target).Dispose();
				queries[i].Free();
			}
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00007BBC File Offset: 0x00005DBC
		internal unsafe virtual void OnBeforeDestroyInternal()
		{
			SystemState* ptr = this.CheckedState();
			if (ptr->PreviouslyEnabled)
			{
				ptr->PreviouslyEnabled = false;
				this.OnStopRunning();
			}
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00007BE8 File Offset: 0x00005DE8
		internal unsafe void BeforeUpdateVersioning()
		{
			SystemState* ptr = this.CheckedState();
			ref EntityComponentStore entityComponentStore = ref *ptr->m_EntityManager.GetCheckedEntityDataAccess()->EntityComponentStore;
			SystemHandle systemHandle = ptr->SystemHandle;
			entityComponentStore.IncrementGlobalSystemVersion(in systemHandle);
			ref UnsafeList<EntityQuery> entityQueries = ref ptr->EntityQueries;
			for (int i = 0; i < entityQueries.Length; i++)
			{
				entityQueries[i].SetChangedFilterRequiredVersion(ptr->m_LastSystemVersion);
			}
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00007C48 File Offset: 0x00005E48
		internal unsafe void AfterUpdateVersioning()
		{
			SystemState* ptr = this.CheckedState();
			EntityComponentStore* entityComponentStore = ptr->m_EntityManager.GetCheckedEntityDataAccess()->EntityComponentStore;
			ptr->m_LastSystemVersion = entityComponentStore->GlobalSystemVersion;
			ref EntityComponentStore ptr2 = ref *entityComponentStore;
			SystemHandle systemHandle = default(SystemHandle);
			ptr2.IncrementGlobalSystemVersion(in systemHandle);
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00007C86 File Offset: 0x00005E86
		internal unsafe void CompleteDependencyInternal()
		{
			this.CheckedState()->CompleteDependencyInternal();
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00007C93 File Offset: 0x00005E93
		public unsafe ComponentTypeHandle<T> GetComponentTypeHandle<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(bool isReadOnly = false) where T : struct, ValueType, IComponentData
		{
			return this.CheckedState()->GetComponentTypeHandle<T>(isReadOnly);
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00007CA1 File Offset: 0x00005EA1
		public unsafe DynamicComponentTypeHandle GetDynamicComponentTypeHandle(ComponentType componentType)
		{
			return this.CheckedState()->GetDynamicComponentTypeHandle(componentType);
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00007CAF File Offset: 0x00005EAF
		public unsafe BufferTypeHandle<T> GetBufferTypeHandle<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(bool isReadOnly = false) where T : struct, ValueType, IBufferElementData
		{
			return this.CheckedState()->GetBufferTypeHandle<T>(isReadOnly);
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00007CBD File Offset: 0x00005EBD
		public unsafe SharedComponentTypeHandle<T> GetSharedComponentTypeHandle<T>() where T : struct, ISharedComponentData
		{
			return this.CheckedState()->GetSharedComponentTypeHandle<T>();
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00007CCA File Offset: 0x00005ECA
		public unsafe DynamicSharedComponentTypeHandle GetDynamicSharedComponentTypeHandle(ComponentType componentType)
		{
			return this.CheckedState()->GetDynamicSharedComponentTypeHandle(componentType);
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00007CD8 File Offset: 0x00005ED8
		public unsafe EntityTypeHandle GetEntityTypeHandle()
		{
			return this.CheckedState()->GetEntityTypeHandle();
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00007CE5 File Offset: 0x00005EE5
		public unsafe ComponentLookup<T> GetComponentLookup<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(bool isReadOnly = false) where T : struct, ValueType, IComponentData
		{
			return this.CheckedState()->GetComponentLookup<T>(isReadOnly);
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00007CE5 File Offset: 0x00005EE5
		[Obsolete("This method has been renamed to GetComponentLookup. (RemovedAfter Entities 1.0) (UnityUpgradable) -> GetComponentLookup<T>(*)", false)]
		public unsafe ComponentLookup<T> GetComponentDataFromEntity<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(bool isReadOnly = false) where T : struct, ValueType, IComponentData
		{
			return this.CheckedState()->GetComponentLookup<T>(isReadOnly);
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00007CF3 File Offset: 0x00005EF3
		public unsafe BufferLookup<T> GetBufferLookup<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(bool isReadOnly = false) where T : struct, ValueType, IBufferElementData
		{
			return this.CheckedState()->GetBufferLookup<T>(isReadOnly);
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00007CF3 File Offset: 0x00005EF3
		[Obsolete("This method has been renamed to GetBufferLookup. (RemovedAfter Entities 1.0) (UnityUpgradable) -> GetBufferLookup<T>(*)", false)]
		public unsafe BufferLookup<T> GetBufferFromEntity<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(bool isReadOnly = false) where T : struct, ValueType, IBufferElementData
		{
			return this.CheckedState()->GetBufferLookup<T>(isReadOnly);
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00007D01 File Offset: 0x00005F01
		public unsafe EntityStorageInfoLookup GetEntityStorageInfoLookup()
		{
			return this.CheckedState()->GetEntityStorageInfoLookup();
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00007D01 File Offset: 0x00005F01
		[Obsolete("This method has been renamed to GetEntityStorageInfoLookup. (RemovedAfter Entities 1.0) (UnityUpgradable) -> GetEntityStorageInfoLookup(*)", false)]
		public unsafe EntityStorageInfoLookup GetStorageInfoFromEntity()
		{
			return this.CheckedState()->GetEntityStorageInfoLookup();
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00007D0E File Offset: 0x00005F0E
		public unsafe void RequireForUpdate(EntityQuery query)
		{
			this.CheckedState()->RequireForUpdate(query);
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00007D1C File Offset: 0x00005F1C
		public unsafe void RequireAnyForUpdate(params EntityQuery[] queries)
		{
			fixed (EntityQuery[] array = queries)
			{
				EntityQuery* ptr;
				if (queries == null || array.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array[0];
				}
				this.CheckedState()->RequireAnyForUpdate(ptr, queries.Length);
			}
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00007D52 File Offset: 0x00005F52
		public unsafe void RequireAnyForUpdate(NativeArray<EntityQuery> queries)
		{
			this.CheckedState()->RequireAnyForUpdate(queries);
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00007D60 File Offset: 0x00005F60
		public unsafe void RequireForUpdate<T>()
		{
			this.CheckedState()->RequireForUpdate<T>();
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00007D6D File Offset: 0x00005F6D
		[Obsolete("RequireSingletonForUpdate has been renamed. Use RequireForUpdate<T>() instead. (RemovedAfter Entities 1.0) (UnityUpgradable) -> RequireForUpdate<T>()", true)]
		public void RequireSingletonForUpdate<T>()
		{
			this.RequireForUpdate<T>();
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00007D78 File Offset: 0x00005F78
		[Obsolete("Use SystemAPI.HasSingleton instead (RemovedAfter Entities 1.0)")]
		public unsafe bool HasSingleton<T>()
		{
			ComponentType componentType = ComponentType.ReadOnly<T>();
			return this.CheckedState()->GetSingletonEntityQueryInternal(componentType).CalculateEntityCount() == 1;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00007DA4 File Offset: 0x00005FA4
		[Obsolete("Use SystemAPI.GetSingleton instead (RemovedAfter Entities 1.0)")]
		public unsafe T GetSingleton<[global::System.Runtime.CompilerServices.IsUnmanaged] T>() where T : struct, ValueType, IComponentData
		{
			ComponentType componentType = ComponentType.ReadOnly<T>();
			return this.CheckedState()->GetSingletonEntityQueryInternal(componentType).GetSingleton<T>();
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00007DCC File Offset: 0x00005FCC
		[Obsolete("Use SystemAPI.GetSingletonRW instead (RemovedAfter Entities 1.0)")]
		public unsafe RefRW<T> GetSingletonRW<[global::System.Runtime.CompilerServices.IsUnmanaged] T>() where T : struct, ValueType, IComponentData
		{
			ComponentType componentType = ComponentType.ReadWrite<T>();
			return this.CheckedState()->GetSingletonEntityQueryInternal(componentType).GetSingletonRW<T>();
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00007DF4 File Offset: 0x00005FF4
		[Obsolete("Use SystemAPI.GetSingletonBuffer instead (RemovedAfter Entities 1.0)")]
		public unsafe DynamicBuffer<T> GetSingletonBuffer<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(bool isReadOnly = false) where T : struct, ValueType, IBufferElementData
		{
			ComponentType componentType = ComponentType.ReadOnly<T>();
			return this.CheckedState()->GetSingletonEntityQueryInternal(componentType).GetSingletonBuffer<T>(isReadOnly);
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00007E1C File Offset: 0x0000601C
		[Obsolete("Use SystemAPI.TryGetSingleton instead (RemovedAfter Entities 1.0)")]
		public unsafe bool TryGetSingleton<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(out T value) where T : struct, ValueType, IComponentData
		{
			ComponentType componentType = ComponentType.ReadOnly<T>();
			EntityQuery singletonEntityQueryInternal = this.CheckedState()->GetSingletonEntityQueryInternal(componentType);
			bool flag = singletonEntityQueryInternal.CalculateEntityCount() == 1;
			value = (flag ? singletonEntityQueryInternal.GetSingleton<T>() : default(T));
			return flag;
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00007E64 File Offset: 0x00006064
		[Obsolete("Use SystemAPI.TryGetSingletonBuffer instead (RemovedAfter Entities 1.0)")]
		public unsafe bool TryGetSingletonBuffer<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(out DynamicBuffer<T> value) where T : struct, ValueType, IBufferElementData
		{
			ComponentType componentType = ComponentType.ReadOnly<T>();
			bool flag = this.CheckedState()->GetSingletonEntityQueryInternal(componentType).CalculateEntityCount() == 1;
			value = (flag ? this.GetSingletonBuffer<T>(false) : default(DynamicBuffer<T>));
			return flag;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00007EAC File Offset: 0x000060AC
		[Obsolete("Use SystemAPI.SetSingleton instead (RemovedAfter Entities 1.0)")]
		public unsafe void SetSingleton<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(T value) where T : struct, ValueType, IComponentData
		{
			ComponentType componentType = ComponentType.ReadWrite<T>();
			this.CheckedState()->GetSingletonEntityQueryInternal(componentType).SetSingleton<T>(value);
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00007ED4 File Offset: 0x000060D4
		[Obsolete("Use SystemAPI.GetSingletonEntity instead (RemovedAfter Entities 1.0)")]
		public unsafe Entity GetSingletonEntity<T>()
		{
			ComponentType componentType = ComponentType.ReadOnly<T>();
			return this.CheckedState()->GetSingletonEntityQueryInternal(componentType).GetSingletonEntity();
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00007EFC File Offset: 0x000060FC
		[Obsolete("Use SystemAPI.TryGetSingletonEntity instead (RemovedAfter Entities 1.0)")]
		public unsafe bool TryGetSingletonEntity<T>(out Entity value)
		{
			ComponentType componentType = ComponentType.ReadOnly<T>();
			EntityQuery singletonEntityQueryInternal = this.CheckedState()->GetSingletonEntityQueryInternal(componentType);
			bool flag = singletonEntityQueryInternal.CalculateEntityCount() == 1;
			value = (flag ? singletonEntityQueryInternal.GetSingletonEntity() : Entity.Null);
			return flag;
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00007F3F File Offset: 0x0000613F
		internal unsafe EntityQuery GetEntityQueryInternal(ComponentType* componentTypes, int count)
		{
			return this.CheckedState()->GetEntityQueryInternal(componentTypes, count);
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00007F50 File Offset: 0x00006150
		internal unsafe EntityQuery GetEntityQueryInternal(ComponentType[] componentTypes)
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
			return this.GetEntityQueryInternal(ptr, componentTypes.Length);
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00007F7F File Offset: 0x0000617F
		internal unsafe EntityQuery GetEntityQueryInternal(EntityQueryDesc[] desc)
		{
			return this.CheckedState()->GetEntityQueryInternal(desc);
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00007F8D File Offset: 0x0000618D
		protected internal EntityQuery GetEntityQuery(params ComponentType[] componentTypes)
		{
			return this.GetEntityQueryInternal(componentTypes);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00007F96 File Offset: 0x00006196
		protected unsafe EntityQuery GetEntityQuery(NativeArray<ComponentType> componentTypes)
		{
			return this.GetEntityQueryInternal((ComponentType*)componentTypes.GetUnsafeReadOnlyPtr<ComponentType>(), componentTypes.Length);
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00007FAB File Offset: 0x000061AB
		protected internal EntityQuery GetEntityQuery(params EntityQueryDesc[] queryDesc)
		{
			return this.GetEntityQueryInternal(queryDesc);
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00007FB4 File Offset: 0x000061B4
		public unsafe EntityQuery GetEntityQuery(in EntityQueryBuilder builder)
		{
			return this.CheckedState()->GetEntityQueryInternal(builder);
		}

		// Token: 0x04000064 RID: 100
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal unsafe SystemState* m_StatePtr;
	}
}
