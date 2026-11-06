using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Core;
using Unity.Jobs;

namespace Unity.Entities
{
	// Token: 0x020002DC RID: 732
	[GenerateTestsForBurstCompatibility]
	[DebuggerTypeProxy(typeof(SystemState_))]
	[DebuggerDisplay("{SystemState_.GetName(m_Handle, World)}")]
	public ref struct SystemState
	{
		// Token: 0x1700012D RID: 301
		// (get) Token: 0x0600135F RID: 4959 RVA: 0x0004BC74 File Offset: 0x00049E74
		private unsafe void* This
		{
			get
			{
				fixed (UnsafeList<EntityQuery>* ptr = &this.m_EntityQueries)
				{
					return (void*)ptr;
				}
			}
		}

		// Token: 0x06001360 RID: 4960 RVA: 0x0004BC8C File Offset: 0x00049E8C
		private void CheckThis()
		{
			if (this.m_Self != this.This)
			{
				throw new InvalidOperationException("This is a value copy of the system state that will lead to memory corruption in the original system state data");
			}
		}

		// Token: 0x06001361 RID: 4961 RVA: 0x0004BCA7 File Offset: 0x00049EA7
		private void SetFlag(uint mask, bool value)
		{
			this.m_Flags = (value ? (this.m_Flags | mask) : (this.m_Flags & ~mask));
		}

		// Token: 0x06001362 RID: 4962 RVA: 0x00002287 File Offset: 0x00000487
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		internal void DisableIsExecutingOnUpdate()
		{
		}

		// Token: 0x06001363 RID: 4963 RVA: 0x00002287 File Offset: 0x00000487
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		internal void EnableIsExecutingISystemOnUpdate()
		{
		}

		// Token: 0x06001364 RID: 4964 RVA: 0x0004BCC5 File Offset: 0x00049EC5
		internal void SetWasUsingBurstProfilerMarker(bool enabled)
		{
			this.SetFlag(64U, enabled);
		}

		// Token: 0x06001365 RID: 4965 RVA: 0x0004BCD0 File Offset: 0x00049ED0
		internal bool WasUsingBurstProfilerMarker()
		{
			return (this.m_Flags & 64U) > 0U;
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06001366 RID: 4966 RVA: 0x0004BCDE File Offset: 0x00049EDE
		public int UnmanagedMetaIndex
		{
			get
			{
				return this.m_UnmanagedMetaIndex;
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06001367 RID: 4967 RVA: 0x0004BCE6 File Offset: 0x00049EE6
		public NativeText.ReadOnly DebugName
		{
			get
			{
				return this.m_DebugName;
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06001368 RID: 4968 RVA: 0x0004BCF0 File Offset: 0x00049EF0
		internal unsafe ref UnsafeList<EntityQuery> EntityQueries
		{
			get
			{
				fixed (UnsafeList<EntityQuery>* ptr = &this.m_EntityQueries)
				{
					void* ptr2 = (void*)ptr;
					return UnsafeUtility.AsRef<UnsafeList<EntityQuery>>(ptr2);
				}
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06001369 RID: 4969 RVA: 0x0004BD10 File Offset: 0x00049F10
		internal unsafe ref UnsafeList<EntityQuery> RequiredEntityQueries
		{
			get
			{
				fixed (UnsafeList<EntityQuery>* ptr = &this.m_RequiredEntityQueries)
				{
					void* ptr2 = (void*)ptr;
					return UnsafeUtility.AsRef<UnsafeList<EntityQuery>>(ptr2);
				}
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x0600136A RID: 4970 RVA: 0x0004BD2D File Offset: 0x00049F2D
		// (set) Token: 0x0600136B RID: 4971 RVA: 0x0004BD3A File Offset: 0x00049F3A
		public bool Enabled
		{
			get
			{
				return (this.m_Flags & 1U) > 0U;
			}
			set
			{
				this.SetFlag(1U, value);
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x0600136C RID: 4972 RVA: 0x0004BD44 File Offset: 0x00049F44
		// (set) Token: 0x0600136D RID: 4973 RVA: 0x0004BD51 File Offset: 0x00049F51
		internal bool RequireMatchingQueriesForUpdate
		{
			get
			{
				return (this.m_Flags & 2U) > 0U;
			}
			set
			{
				this.SetFlag(2U, value);
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x0600136E RID: 4974 RVA: 0x0004BD5B File Offset: 0x00049F5B
		// (set) Token: 0x0600136F RID: 4975 RVA: 0x0004BD68 File Offset: 0x00049F68
		internal bool PreviouslyEnabled
		{
			get
			{
				return (this.m_Flags & 4U) > 0U;
			}
			set
			{
				this.SetFlag(4U, value);
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06001370 RID: 4976 RVA: 0x0004BD72 File Offset: 0x00049F72
		// (set) Token: 0x06001371 RID: 4977 RVA: 0x0004BD7F File Offset: 0x00049F7F
		private bool NeedToGetDependencyFromSafetyManager
		{
			get
			{
				return (this.m_Flags & 8U) > 0U;
			}
			set
			{
				this.SetFlag(8U, value);
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06001372 RID: 4978 RVA: 0x0004BD89 File Offset: 0x00049F89
		public unsafe uint GlobalSystemVersion
		{
			get
			{
				return this.m_EntityComponentStore->GlobalSystemVersion;
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06001373 RID: 4979 RVA: 0x0004BD96 File Offset: 0x00049F96
		public uint LastSystemVersion
		{
			get
			{
				return this.m_LastSystemVersion;
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06001374 RID: 4980 RVA: 0x0004BD9E File Offset: 0x00049F9E
		public EntityManager EntityManager
		{
			get
			{
				return this.m_EntityManager;
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06001375 RID: 4981 RVA: 0x0004BDA6 File Offset: 0x00049FA6
		[ExcludeFromBurstCompatTesting("Returns managed World")]
		public World World
		{
			get
			{
				return (World)this.m_World.Target;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06001376 RID: 4982 RVA: 0x0004BDB8 File Offset: 0x00049FB8
		public WorldUnmanaged WorldUnmanaged
		{
			get
			{
				return this.m_WorldUnmanaged;
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06001377 RID: 4983 RVA: 0x0004BDC0 File Offset: 0x00049FC0
		public Allocator WorldUpdateAllocator
		{
			get
			{
				return this.m_WorldUnmanaged.UpdateAllocator.ToAllocator;
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06001378 RID: 4984 RVA: 0x0004BDD2 File Offset: 0x00049FD2
		internal ref RewindableAllocator WorldRewindableAllocator
		{
			get
			{
				return this.m_WorldUnmanaged.UpdateAllocator;
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06001379 RID: 4985 RVA: 0x0004BDDF File Offset: 0x00049FDF
		public SystemHandle SystemHandle
		{
			get
			{
				return this.m_Handle;
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x0600137A RID: 4986 RVA: 0x0004BDDF File Offset: 0x00049FDF
		[Obsolete("SystemHandle has been renamed to SystemHandle. (UnityUpgradable) -> SystemHandle", true)]
		public SystemHandle SystemHandleUntyped
		{
			get
			{
				return this.m_Handle;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x0600137B RID: 4987 RVA: 0x0004BDE8 File Offset: 0x00049FE8
		[Obsolete("Time has been deprecated as duplicate. Use SystemAPI.Time or WorldUnmanaged.Time instead (RemovedAfter 2023-08-08)", true)]
		public readonly ref TimeData Time
		{
			get
			{
				return this.WorldUnmanaged.Time;
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x0600137C RID: 4988 RVA: 0x0004BE03 File Offset: 0x0004A003
		[ExcludeFromBurstCompatTesting("Returns managed system")]
		internal ComponentSystemBase ManagedSystem
		{
			get
			{
				if (!this.m_ManagedSystem.IsAllocated)
				{
					return null;
				}
				return this.m_ManagedSystem.Target as ComponentSystemBase;
			}
		}

		// Token: 0x0600137D RID: 4989 RVA: 0x0004BE24 File Offset: 0x0004A024
		[ExcludeFromBurstCompatTesting("Takes managed World")]
		internal void InitManaged(World world, SystemHandle handle, Type managedType, ComponentSystemBase system)
		{
			this.m_UnmanagedMetaIndex = -1;
			this.m_ManagedSystem = GCHandle.Alloc(system, GCHandleType.Normal);
			SystemTypeIndex systemTypeIndex = TypeManager.GetSystemTypeIndex(managedType);
			this.m_DebugName = TypeManager.GetSystemName(systemTypeIndex);
			this.CommonInit(world, handle, systemTypeIndex);
			if (managedType != null)
			{
				Type typeFromHandle = typeof(RequireMatchingQueriesForUpdateAttribute);
				if (TypeManager.GetSystemAttributes(managedType, typeFromHandle).Length != 0)
				{
					this.RequireMatchingQueriesForUpdate = true;
				}
			}
		}

		// Token: 0x0600137E RID: 4990 RVA: 0x0004BE8C File Offset: 0x0004A08C
		private unsafe void CommonInit(World world, SystemHandle handle, int systemTypeIndex)
		{
			this.Enabled = true;
			this.m_SystemTypeIndex = systemTypeIndex;
			this.m_SystemID = ++SystemState.ms_SystemIDAllocator;
			this.m_World = GCHandle.Alloc(world);
			this.m_WorldUnmanaged = world.Unmanaged;
			this.m_EntityManager = world.EntityManager;
			this.m_EntityComponentStore = this.m_EntityManager.GetCheckedEntityDataAccess()->EntityComponentStore;
			this.m_DependencyManager = this.m_EntityManager.GetCheckedEntityDataAccess()->DependencyManager;
			this.m_Handle = handle;
			*this.EntityQueries = new UnsafeList<EntityQuery>(0, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			*this.RequiredEntityQueries = new UnsafeList<EntityQuery>(0, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			this.m_JobDependencyForReadingSystems = new UnsafeList<TypeIndex>(0, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			this.m_JobDependencyForWritingSystems = new UnsafeList<TypeIndex>(0, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			this.RequireMatchingQueriesForUpdate = false;
		}

		// Token: 0x0600137F RID: 4991 RVA: 0x0004BF74 File Offset: 0x0004A174
		[ExcludeFromBurstCompatTesting("Takes managed World")]
		internal unsafe void InitUnmanaged(World world, SystemHandle handle, int unmanagedMetaIndex, void* systemptr)
		{
			this.Enabled = true;
			this.m_UnmanagedMetaIndex = unmanagedMetaIndex;
			this.m_SystemPtr = systemptr;
			this.m_Self = this.This;
			SystemTypeIndex systemTypeIndex = TypeManager.GetSystemTypeIndex(SystemBaseRegistry.GetStructType(unmanagedMetaIndex));
			this.CommonInit(world, handle, systemTypeIndex);
			this.m_DebugName = TypeManager.GetSystemName(systemTypeIndex);
			if (TypeManager.GetSystemAttributes(systemTypeIndex, TypeManager.SystemAttributeKind.RequireMatchingQueriesForUpdate, Allocator.Temp).Length > 0)
			{
				this.RequireMatchingQueriesForUpdate = true;
			}
		}

		// Token: 0x06001380 RID: 4992 RVA: 0x0004BFE3 File Offset: 0x0004A1E3
		[ExcludeFromBurstCompatTesting("Returns managed string")]
		internal string GetProfilerMarkerName(World world)
		{
			return string.Format("{0} {1}", world.Name, this.m_DebugName);
		}

		// Token: 0x06001381 RID: 4993 RVA: 0x0004C000 File Offset: 0x0004A200
		internal unsafe void Dispose()
		{
			this.DisposeQueries(this.EntityQueries);
			this.DisposeQueries(this.RequiredEntityQueries);
			this.EntityQueries.Dispose();
			*this.EntityQueries = default(UnsafeList<EntityQuery>);
			this.RequiredEntityQueries.Dispose();
			*this.RequiredEntityQueries = default(UnsafeList<EntityQuery>);
			if (this.m_World.IsAllocated)
			{
				this.m_World.Free();
			}
			if (this.m_ManagedSystem.IsAllocated)
			{
				this.m_ManagedSystem.Free();
			}
			this.m_JobDependencyForReadingSystems.Dispose();
			this.m_JobDependencyForWritingSystems.Dispose();
		}

		// Token: 0x06001382 RID: 4994 RVA: 0x0004C09C File Offset: 0x0004A29C
		private void DisposeQueries(ref UnsafeList<EntityQuery> queries)
		{
			for (int i = 0; i < queries.Length; i++)
			{
				EntityQuery entityQuery = queries[i];
				if (this.m_EntityManager.IsQueryValid(entityQuery))
				{
					entityQuery.Dispose();
				}
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06001383 RID: 4995 RVA: 0x0004C0D8 File Offset: 0x0004A2D8
		// (set) Token: 0x06001384 RID: 4996 RVA: 0x0004C13A File Offset: 0x0004A33A
		public unsafe JobHandle Dependency
		{
			get
			{
				if (this.NeedToGetDependencyFromSafetyManager)
				{
					ComponentDependencyManager* dependencyManager = this.m_DependencyManager;
					this.NeedToGetDependencyFromSafetyManager = false;
					this.m_JobHandle = dependencyManager->GetDependency(this.m_JobDependencyForReadingSystems.Ptr, this.m_JobDependencyForReadingSystems.Length, this.m_JobDependencyForWritingSystems.Ptr, this.m_JobDependencyForWritingSystems.Length, false);
				}
				return this.m_JobHandle;
			}
			set
			{
				this.NeedToGetDependencyFromSafetyManager = false;
				this.m_JobHandle = value;
			}
		}

		// Token: 0x06001385 RID: 4997 RVA: 0x0004C14A File Offset: 0x0004A34A
		public void CompleteDependency()
		{
			this.m_JobHandle.Complete();
			if (this.NeedToGetDependencyFromSafetyManager)
			{
				this.NeedToGetDependencyFromSafetyManager = false;
				this.CompleteDependencyInternal();
			}
		}

		// Token: 0x06001386 RID: 4998 RVA: 0x0004C16C File Offset: 0x0004A36C
		internal unsafe void CompleteDependencyInternal()
		{
			this.m_DependencyManager->CompleteDependenciesNoChecks(this.m_JobDependencyForReadingSystems.Ptr, this.m_JobDependencyForReadingSystems.Length, this.m_JobDependencyForWritingSystems.Ptr, this.m_JobDependencyForWritingSystems.Length);
		}

		// Token: 0x06001387 RID: 4999 RVA: 0x0004C1A8 File Offset: 0x0004A3A8
		internal unsafe void BeforeUpdateVersioning()
		{
			this.m_EntityComponentStore->IncrementGlobalSystemVersion(in this.m_Handle);
			ref UnsafeList<EntityQuery> entityQueries = ref this.EntityQueries;
			for (int i = 0; i < entityQueries.Length; i++)
			{
				entityQueries[i].SetChangedFilterRequiredVersion(this.m_LastSystemVersion);
			}
		}

		// Token: 0x06001388 RID: 5000 RVA: 0x0004C1F4 File Offset: 0x0004A3F4
		internal unsafe void AfterUpdateVersioning()
		{
			this.m_LastSystemVersion = this.m_EntityComponentStore->GlobalSystemVersion;
			ref EntityComponentStore entityComponentStore = ref *this.m_EntityComponentStore;
			SystemHandle systemHandle = default(SystemHandle);
			entityComponentStore.IncrementGlobalSystemVersion(in systemHandle);
		}

		// Token: 0x06001389 RID: 5001 RVA: 0x00002287 File Offset: 0x00000487
		[Conditional("UNITY_ENTITIES_RUNTIME_TOOLING")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void BeforeUpdateResetRunTracker()
		{
		}

		// Token: 0x0600138A RID: 5002 RVA: 0x00002287 File Offset: 0x00000487
		[Conditional("UNITY_ENTITIES_RUNTIME_TOOLING")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void BeforeUpdateRecordTiming()
		{
		}

		// Token: 0x0600138B RID: 5003 RVA: 0x00002287 File Offset: 0x00000487
		[Conditional("UNITY_ENTITIES_RUNTIME_TOOLING")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void AfterUpdateRecordTiming()
		{
		}

		// Token: 0x0600138C RID: 5004 RVA: 0x00002287 File Offset: 0x00000487
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void CheckOnUpdate_Query()
		{
		}

		// Token: 0x0600138D RID: 5005 RVA: 0x00002287 File Offset: 0x00000487
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void CheckOnUpdate_Handle()
		{
		}

		// Token: 0x0600138E RID: 5006 RVA: 0x00002287 File Offset: 0x00000487
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void CheckOnUpdate_Lookup()
		{
		}

		// Token: 0x0600138F RID: 5007 RVA: 0x0004C226 File Offset: 0x0004A426
		internal void BeforeOnUpdate()
		{
			this.BeforeUpdateVersioning();
			this.m_JobHandle.Complete();
			this.NeedToGetDependencyFromSafetyManager = true;
		}

		// Token: 0x06001390 RID: 5008 RVA: 0x0004C240 File Offset: 0x0004A440
		internal unsafe void AfterOnUpdate()
		{
			this.AfterUpdateVersioning();
			if (!this.m_JobHandle.Equals(default(JobHandle)))
			{
				JobHandle.ScheduleBatchedJobs();
				this.m_JobHandle = this.m_DependencyManager->AddDependency(this.m_JobDependencyForReadingSystems.Ptr, this.m_JobDependencyForReadingSystems.Length, this.m_JobDependencyForWritingSystems.Ptr, this.m_JobDependencyForWritingSystems.Length, this.m_JobHandle);
			}
		}

		// Token: 0x06001391 RID: 5009 RVA: 0x0004C2B4 File Offset: 0x0004A4B4
		public bool ShouldRunSystem()
		{
			ref UnsafeList<EntityQuery> requiredEntityQueries = ref this.RequiredEntityQueries;
			if (requiredEntityQueries.Length > 0)
			{
				for (int num = 0; num != requiredEntityQueries.Length; num++)
				{
					if (requiredEntityQueries[num].IsEmptyIgnoreFilter)
					{
						return false;
					}
				}
				return true;
			}
			if (this.RequireMatchingQueriesForUpdate)
			{
				ref UnsafeList<EntityQuery> entityQueries = ref this.EntityQueries;
				int length = this.EntityQueries.Length;
				for (int num2 = 0; num2 != length; num2++)
				{
					if (!entityQueries[num2].IsEmptyIgnoreFilter)
					{
						return true;
					}
				}
				return false;
			}
			return true;
		}

		// Token: 0x06001392 RID: 5010 RVA: 0x0004C33C File Offset: 0x0004A53C
		internal unsafe EntityQuery GetEntityQueryInternal(ComponentType* componentTypes, int count)
		{
			EntityQuery entityQueryInternal;
			using (EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp, componentTypes, count))
			{
				entityQueryInternal = this.GetEntityQueryInternal(entityQueryBuilder);
			}
			return entityQueryInternal;
		}

		// Token: 0x06001393 RID: 5011 RVA: 0x0004C37C File Offset: 0x0004A57C
		internal void AddReaderWriter(ComponentType componentType)
		{
			if (CalculateReaderWriterDependency.Add(componentType, ref this.m_JobDependencyForReadingSystems, ref this.m_JobDependencyForWritingSystems))
			{
				this.CompleteDependencyInternal();
			}
		}

		// Token: 0x06001394 RID: 5012 RVA: 0x0004C398 File Offset: 0x0004A598
		internal void AddReaderWriters(EntityQuery query)
		{
			if (query.AddReaderWritersToLists(ref this.m_JobDependencyForReadingSystems, ref this.m_JobDependencyForWritingSystems))
			{
				this.CompleteDependencyInternal();
			}
		}

		// Token: 0x06001395 RID: 5013 RVA: 0x0004C3B5 File Offset: 0x0004A5B5
		private void AfterQueryCreated(EntityQuery query)
		{
			query.SetChangedFilterRequiredVersion(this.m_LastSystemVersion);
			this.EntityQueries.Add(in query);
		}

		// Token: 0x06001396 RID: 5014 RVA: 0x0004C3D4 File Offset: 0x0004A5D4
		internal unsafe EntityQuery GetSingletonEntityQueryInternal(ComponentType type)
		{
			ref UnsafeList<EntityQuery> entityQueries = ref this.EntityQueries;
			for (int num = 0; num != entityQueries.Length; num++)
			{
				EntityQuery entityQuery = entityQueries[num];
				EntityQueryData* queryData = entityQuery._GetImpl()->_QueryData;
				if (2 == queryData->RequiredComponentsCount && !(queryData->RequiredComponents[1] != type))
				{
					return entityQuery;
				}
			}
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp, &type, 1).WithOptions(EntityQueryOptions.IncludeSystems);
			EntityQuery entityQuery3;
			try
			{
				EntityQuery entityQuery2 = this.EntityManager.CreateEntityQueryUnowned(in entityQueryBuilder);
				this.AddReaderWriters(entityQuery2);
				this.AfterQueryCreated(entityQuery2);
				entityQuery3 = entityQuery2;
			}
			finally
			{
				entityQueryBuilder.Dispose();
			}
			return entityQuery3;
		}

		// Token: 0x06001397 RID: 5015 RVA: 0x0004C494 File Offset: 0x0004A694
		[ExcludeFromBurstCompatTesting("Takes managed array")]
		internal EntityQuery GetEntityQueryInternal(EntityQueryDesc[] desc)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryManager.ConvertToEntityQueryBuilder(ref entityQueryBuilder, desc);
			EntityQuery entityQueryInternal = this.GetEntityQueryInternal(entityQueryBuilder);
			entityQueryBuilder.Dispose();
			return entityQueryInternal;
		}

		// Token: 0x06001398 RID: 5016 RVA: 0x0004C4C4 File Offset: 0x0004A6C4
		internal EntityQuery GetEntityQueryInternal(EntityQueryBuilder desc)
		{
			desc.FinalizeQueryInternal();
			ref UnsafeList<EntityQuery> entityQueries = ref this.EntityQueries;
			for (int num = 0; num != entityQueries.Length; num++)
			{
				EntityQuery entityQuery = entityQueries[num];
				if (entityQuery.CompareQuery(in desc))
				{
					return entityQuery;
				}
			}
			EntityQuery entityQuery2 = this.EntityManager.CreateEntityQueryUnowned(in desc);
			this.AddReaderWriters(entityQuery2);
			this.AfterQueryCreated(entityQuery2);
			return entityQuery2;
		}

		// Token: 0x06001399 RID: 5017 RVA: 0x0004C528 File Offset: 0x0004A728
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleBufferElement) })]
		internal DynamicBuffer<T> GetBuffer<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(Entity entity, bool isReadOnly = false) where T : struct, ValueType, IBufferElementData
		{
			this.AddReaderWriter(isReadOnly ? ComponentType.ReadOnly<T>() : ComponentType.ReadWrite<T>());
			return this.EntityManager.GetBuffer<T>(entity, isReadOnly);
		}

		// Token: 0x0600139A RID: 5018 RVA: 0x0004C55C File Offset: 0x0004A75C
		[ExcludeFromBurstCompatTesting("Takes managed array")]
		public unsafe EntityQuery GetEntityQuery(params ComponentType[] componentTypes)
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

		// Token: 0x0600139B RID: 5019 RVA: 0x0004C58B File Offset: 0x0004A78B
		public unsafe EntityQuery GetEntityQuery(ComponentType componentType)
		{
			return this.GetEntityQueryInternal(&componentType, 1);
		}

		// Token: 0x0600139C RID: 5020 RVA: 0x0004C597 File Offset: 0x0004A797
		public unsafe EntityQuery GetEntityQuery(NativeArray<ComponentType> componentTypes)
		{
			return this.GetEntityQueryInternal((ComponentType*)componentTypes.GetUnsafeReadOnlyPtr<ComponentType>(), componentTypes.Length);
		}

		// Token: 0x0600139D RID: 5021 RVA: 0x0004C5AC File Offset: 0x0004A7AC
		[ExcludeFromBurstCompatTesting("Takes managed array")]
		public EntityQuery GetEntityQuery(params EntityQueryDesc[] queryDesc)
		{
			return this.GetEntityQueryInternal(queryDesc);
		}

		// Token: 0x0600139E RID: 5022 RVA: 0x0004C5B5 File Offset: 0x0004A7B5
		public EntityQuery GetEntityQuery(in EntityQueryBuilder builder)
		{
			return this.GetEntityQueryInternal(builder);
		}

		// Token: 0x0600139F RID: 5023 RVA: 0x0004C5C4 File Offset: 0x0004A7C4
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public ComponentTypeHandle<T> GetComponentTypeHandle<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(bool isReadOnly = false) where T : struct, ValueType, IComponentData
		{
			this.AddReaderWriter(isReadOnly ? ComponentType.ReadOnly<T>() : ComponentType.ReadWrite<T>());
			return this.EntityManager.GetComponentTypeHandle<T>(isReadOnly);
		}

		// Token: 0x060013A0 RID: 5024 RVA: 0x0004C5F8 File Offset: 0x0004A7F8
		public DynamicComponentTypeHandle GetDynamicComponentTypeHandle(ComponentType componentType)
		{
			this.AddReaderWriter(componentType);
			return this.EntityManager.GetDynamicComponentTypeHandle(componentType);
		}

		// Token: 0x060013A1 RID: 5025 RVA: 0x0004C61C File Offset: 0x0004A81C
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleBufferElement) })]
		public BufferTypeHandle<T> GetBufferTypeHandle<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(bool isReadOnly = false) where T : struct, ValueType, IBufferElementData
		{
			this.AddReaderWriter(isReadOnly ? ComponentType.ReadOnly<T>() : ComponentType.ReadWrite<T>());
			return this.EntityManager.GetBufferTypeHandle<T>(isReadOnly);
		}

		// Token: 0x060013A2 RID: 5026 RVA: 0x0004C650 File Offset: 0x0004A850
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleSharedComponentData) })]
		public SharedComponentTypeHandle<T> GetSharedComponentTypeHandle<T>() where T : struct, ISharedComponentData
		{
			return this.EntityManager.GetSharedComponentTypeHandle<T>();
		}

		// Token: 0x060013A3 RID: 5027 RVA: 0x0004C66C File Offset: 0x0004A86C
		public DynamicSharedComponentTypeHandle GetDynamicSharedComponentTypeHandle(ComponentType componentType)
		{
			return this.EntityManager.GetDynamicSharedComponentTypeHandle(componentType);
		}

		// Token: 0x060013A4 RID: 5028 RVA: 0x0004C688 File Offset: 0x0004A888
		public EntityTypeHandle GetEntityTypeHandle()
		{
			return this.EntityManager.GetEntityTypeHandle();
		}

		// Token: 0x060013A5 RID: 5029 RVA: 0x0004C6A4 File Offset: 0x0004A8A4
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		public ComponentLookup<T> GetComponentLookup<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(bool isReadOnly = false) where T : struct, ValueType, IComponentData
		{
			this.AddReaderWriter(isReadOnly ? ComponentType.ReadOnly<T>() : ComponentType.ReadWrite<T>());
			return this.EntityManager.GetComponentLookup<T>(isReadOnly);
		}

		// Token: 0x060013A6 RID: 5030 RVA: 0x0004C6D5 File Offset: 0x0004A8D5
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleComponentData) })]
		[Obsolete("This method has been renamed to GetComponentLookup<T>(). (RemovedAFter Entities 1.0) (UnityUpgradable) -> GetComponentLookup<T>(*)", false)]
		public ComponentLookup<T> GetComponentDataFromEntity<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(bool isReadOnly = false) where T : struct, ValueType, IComponentData
		{
			return this.GetComponentLookup<T>(isReadOnly);
		}

		// Token: 0x060013A7 RID: 5031 RVA: 0x0004C6E0 File Offset: 0x0004A8E0
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleBufferElement) })]
		public BufferLookup<T> GetBufferLookup<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(bool isReadOnly = false) where T : struct, ValueType, IBufferElementData
		{
			this.AddReaderWriter(isReadOnly ? ComponentType.ReadOnly<T>() : ComponentType.ReadWrite<T>());
			return this.EntityManager.GetBufferLookup<T>(isReadOnly);
		}

		// Token: 0x060013A8 RID: 5032 RVA: 0x0004C711 File Offset: 0x0004A911
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleBufferElement) })]
		[Obsolete("This method has been renamed to GetBufferLookup<T>(). (RemovedAFter Entities 1.0) (UnityUpgradable) -> GetBufferLookup<T>(*)", false)]
		public BufferLookup<T> GetBufferFromEntity<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(bool isReadOnly = false) where T : struct, ValueType, IBufferElementData
		{
			return this.GetBufferLookup<T>(isReadOnly);
		}

		// Token: 0x060013A9 RID: 5033 RVA: 0x0004C71C File Offset: 0x0004A91C
		[GenerateTestsForBurstCompatibility]
		public EntityStorageInfoLookup GetEntityStorageInfoLookup()
		{
			return this.EntityManager.GetEntityStorageInfoLookup();
		}

		// Token: 0x060013AA RID: 5034 RVA: 0x0004C738 File Offset: 0x0004A938
		[GenerateTestsForBurstCompatibility]
		[Obsolete("This method has been renamed to GetEntityStorageInfoLookup(). (RemovedAFter Entities 1.0) (UnityUpgradable) -> GetEntityStorageInfoLookup(*)", false)]
		public EntityStorageInfoLookup GetStorageInfoFromEntity()
		{
			return this.EntityManager.GetEntityStorageInfoLookup();
		}

		// Token: 0x060013AB RID: 5035 RVA: 0x0004C753 File Offset: 0x0004A953
		public unsafe void RequireForUpdate(EntityQuery query)
		{
			if (!(*this.RequiredEntityQueries).Contains(query))
			{
				this.RequiredEntityQueries.Add(in query);
			}
		}

		// Token: 0x060013AC RID: 5036 RVA: 0x0004C778 File Offset: 0x0004A978
		[ExcludeFromBurstCompatTesting("Eventually accesses managed World")]
		public unsafe void RequireForUpdate<T>()
		{
			ComponentType componentType = ComponentType.ReadOnly<T>();
			using (EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp, &componentType, 1).WithOptions(EntityQueryOptions.IncludeSystems))
			{
				this.RequireForUpdate(this.GetEntityQueryInternal(entityQueryBuilder));
			}
		}

		// Token: 0x060013AD RID: 5037 RVA: 0x0004C7CC File Offset: 0x0004A9CC
		[ExcludeFromBurstCompatTesting("Takes a managed array params")]
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
				this.RequireAnyForUpdate(ptr, queries.Length);
			}
		}

		// Token: 0x060013AE RID: 5038 RVA: 0x0004C7FD File Offset: 0x0004A9FD
		[GenerateTestsForBurstCompatibility]
		public unsafe void RequireAnyForUpdate(NativeArray<EntityQuery> queries)
		{
			this.RequireAnyForUpdate((EntityQuery*)queries.GetUnsafeReadOnlyPtr<EntityQuery>(), queries.Length);
		}

		// Token: 0x060013AF RID: 5039 RVA: 0x0004C814 File Offset: 0x0004AA14
		[GenerateTestsForBurstCompatibility]
		internal unsafe void RequireAnyForUpdate(EntityQuery* queries, int queryCount)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			for (int i = 0; i < queryCount; i++)
			{
				EntityQueryData* queryData = queries[i]._GetImpl()->_QueryData;
				int archetypeQueryCount = queryData->ArchetypeQueryCount;
				for (int j = 0; j < archetypeQueryCount; j++)
				{
					ArchetypeQuery archetypeQuery = queryData->ArchetypeQueries[j];
					for (int k = 0; k < archetypeQuery.AllCount; k++)
					{
						ComponentType componentType = new ComponentType
						{
							TypeIndex = archetypeQuery.All[k],
							AccessModeType = (ComponentType.AccessMode)archetypeQuery.AllAccessMode[k]
						};
						entityQueryBuilder.WithAll(&componentType, 1);
					}
					for (int l = 0; l < archetypeQuery.AnyCount; l++)
					{
						ComponentType componentType2 = new ComponentType
						{
							TypeIndex = archetypeQuery.Any[l],
							AccessModeType = (ComponentType.AccessMode)archetypeQuery.AnyAccessMode[l]
						};
						entityQueryBuilder.WithAny(&componentType2, 1);
					}
					for (int m = 0; m < archetypeQuery.NoneCount; m++)
					{
						ComponentType componentType3 = new ComponentType
						{
							TypeIndex = archetypeQuery.None[m],
							AccessModeType = (ComponentType.AccessMode)archetypeQuery.NoneAccessMode[m]
						};
						entityQueryBuilder.WithNone(&componentType3, 1);
					}
					for (int n = 0; n < archetypeQuery.AbsentCount; n++)
					{
						ComponentType componentType4 = new ComponentType
						{
							TypeIndex = archetypeQuery.Absent[n],
							AccessModeType = (ComponentType.AccessMode)archetypeQuery.AbsentAccessMode[n]
						};
						entityQueryBuilder.WithAbsent(&componentType4, 1);
					}
					for (int num = 0; num < archetypeQuery.DisabledCount; num++)
					{
						ComponentType componentType5 = new ComponentType
						{
							TypeIndex = archetypeQuery.Disabled[num],
							AccessModeType = (ComponentType.AccessMode)archetypeQuery.DisabledAccessMode[num]
						};
						entityQueryBuilder.WithDisabled(&componentType5, 1);
					}
					for (int num2 = 0; num2 < archetypeQuery.PresentCount; num2++)
					{
						ComponentType componentType6 = new ComponentType
						{
							TypeIndex = archetypeQuery.Present[num2],
							AccessModeType = (ComponentType.AccessMode)archetypeQuery.PresentAccessMode[num2]
						};
						entityQueryBuilder.WithPresent(&componentType6, 1);
					}
					entityQueryBuilder.WithOptions(archetypeQuery.Options);
					entityQueryBuilder.FinalizeQueryInternal();
				}
			}
			EntityQuery entityQuery = this.GetEntityQuery(in entityQueryBuilder);
			this.RequireForUpdate(entityQuery);
		}

		// Token: 0x060013B0 RID: 5040 RVA: 0x0004CAE8 File Offset: 0x0004ACE8
		[Obsolete("RequireSingletonForUpdate has been renamed. Use RequireForUpdate<T>() instead. (RemovedAFter Entities 1.0) (UnityUpgradable) -> RequireForUpdate<T>()", true)]
		public void RequireSingletonForUpdate<T>()
		{
			this.RequireForUpdate<T>();
		}

		// Token: 0x04000705 RID: 1797
		private static int ms_SystemIDAllocator;

		// Token: 0x04000706 RID: 1798
		internal unsafe void* m_SystemPtr;

		// Token: 0x04000707 RID: 1799
		internal JobHandle m_JobHandle;

		// Token: 0x04000708 RID: 1800
		private uint m_Flags;

		// Token: 0x04000709 RID: 1801
		internal unsafe ComponentDependencyManager* m_DependencyManager;

		// Token: 0x0400070A RID: 1802
		internal unsafe EntityComponentStore* m_EntityComponentStore;

		// Token: 0x0400070B RID: 1803
		internal uint m_LastSystemVersion;

		// Token: 0x0400070C RID: 1804
		private unsafe void* m_Self;

		// Token: 0x0400070D RID: 1805
		internal SystemTypeIndex m_SystemTypeIndex;

		// Token: 0x0400070E RID: 1806
		internal int m_SystemID;

		// Token: 0x0400070F RID: 1807
		internal EntityManager m_EntityManager;

		// Token: 0x04000710 RID: 1808
		internal UnsafeList<TypeIndex> m_JobDependencyForReadingSystems;

		// Token: 0x04000711 RID: 1809
		internal UnsafeList<TypeIndex> m_JobDependencyForWritingSystems;

		// Token: 0x04000712 RID: 1810
		private UnsafeList<EntityQuery> m_EntityQueries;

		// Token: 0x04000713 RID: 1811
		private UnsafeList<EntityQuery> m_RequiredEntityQueries;

		// Token: 0x04000714 RID: 1812
		internal WorldUnmanaged m_WorldUnmanaged;

		// Token: 0x04000715 RID: 1813
		internal SystemHandle m_Handle;

		// Token: 0x04000716 RID: 1814
		private int m_UnmanagedMetaIndex;

		// Token: 0x04000717 RID: 1815
		internal GCHandle m_World;

		// Token: 0x04000718 RID: 1816
		internal GCHandle m_ManagedSystem;

		// Token: 0x04000719 RID: 1817
		private NativeText.ReadOnly m_DebugName;

		// Token: 0x0400071A RID: 1818
		private const uint kEnabledMask = 1U;

		// Token: 0x0400071B RID: 1819
		private const uint kRequireMatchingQueriesForUpdateMask = 2U;

		// Token: 0x0400071C RID: 1820
		private const uint kPreviouslyEnabledMask = 4U;

		// Token: 0x0400071D RID: 1821
		private const uint kNeedToGetDependencyFromSafetyManagerMask = 8U;

		// Token: 0x0400071E RID: 1822
		private const uint kWasUsingBurstProfilerMarker = 64U;
	}
}
