using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Core;

namespace Unity.Entities
{
	// Token: 0x02000364 RID: 868
	[GenerateTestsForBurstCompatibility]
	public struct WorldUnmanaged
	{
		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06001736 RID: 5942 RVA: 0x00057DCD File Offset: 0x00055FCD
		public EntityManager EntityManager
		{
			get
			{
				return this.GetImpl().m_EntityManager;
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06001737 RID: 5943 RVA: 0x00057DDA File Offset: 0x00055FDA
		public FixedString128Bytes Name
		{
			get
			{
				return this.GetImpl().Name;
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06001738 RID: 5944 RVA: 0x00057DE7 File Offset: 0x00055FE7
		// (set) Token: 0x06001739 RID: 5945 RVA: 0x00057DF4 File Offset: 0x00055FF4
		internal SystemHandle ExecutingSystem
		{
			get
			{
				return this.GetImpl().ExecutingSystem;
			}
			set
			{
				this.GetImpl().ExecutingSystem = value;
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x0600173A RID: 5946 RVA: 0x00057E02 File Offset: 0x00056002
		public ref TimeData Time
		{
			get
			{
				return ref this.GetImpl().CurrentTime;
			}
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x0600173B RID: 5947 RVA: 0x00057E0F File Offset: 0x0005600F
		public WorldFlags Flags
		{
			get
			{
				return this.GetImpl().Flags;
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x0600173C RID: 5948 RVA: 0x00057E1C File Offset: 0x0005601C
		// (set) Token: 0x0600173D RID: 5949 RVA: 0x00057E29 File Offset: 0x00056029
		public float MaximumDeltaTime
		{
			get
			{
				return this.GetImpl().MaximumDeltaTime;
			}
			set
			{
				this.GetImpl().MaximumDeltaTime = value;
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x0600173E RID: 5950 RVA: 0x00057E37 File Offset: 0x00056037
		public ulong SequenceNumber
		{
			get
			{
				return this.GetImpl().SequenceNumber;
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x0600173F RID: 5951 RVA: 0x00057E44 File Offset: 0x00056044
		public int Version
		{
			get
			{
				return this.GetImpl().Version;
			}
		}

		// Token: 0x06001740 RID: 5952 RVA: 0x00057E51 File Offset: 0x00056051
		internal void BumpVersion()
		{
			this.GetImpl().BumpVersion();
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06001741 RID: 5953 RVA: 0x00057E5E File Offset: 0x0005605E
		// (set) Token: 0x06001742 RID: 5954 RVA: 0x00057E6B File Offset: 0x0005606B
		internal unsafe bool UpdateAllocatorEnableBlockFree
		{
			get
			{
				return this.m_Impl->UpdateAllocatorEnableBlockFree;
			}
			set
			{
				this.m_Impl->UpdateAllocatorEnableBlockFree = value;
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06001743 RID: 5955 RVA: 0x00057E79 File Offset: 0x00056079
		public bool IsCreated
		{
			get
			{
				return this.m_Impl != null;
			}
		}

		// Token: 0x06001744 RID: 5956 RVA: 0x00057E88 File Offset: 0x00056088
		[ExcludeFromBurstCompatTesting("Takes managed World")]
		internal unsafe void Create(World world, WorldFlags flags, AllocatorManager.AllocatorHandle backingAllocatorHandle)
		{
			AllocatorHelper<AutoFreeAllocator> allocatorHelper = new AllocatorHelper<AutoFreeAllocator>(backingAllocatorHandle, false, 0);
			allocatorHelper.Allocator.Initialize(backingAllocatorHandle);
			AllocatorManager.AllocatorHandle handle = allocatorHelper.Allocator.Handle;
			void* ptr = (ref handle).Allocate<AllocatorManager.AllocatorHandle>(sizeof(WorldUnmanagedImpl), 16, 1);
			this.m_Impl = (WorldUnmanagedImpl*)ptr;
			ref WorldUnmanagedImpl ptr2 = ref UnsafeUtility.AsRef<WorldUnmanagedImpl>((void*)this.m_Impl);
			ref ulong data = ref WorldUnmanaged.NextSequenceNumber.Data;
			ulong num = data;
			data = num + 1UL;
			ptr2 = new WorldUnmanagedImpl(world, num, flags, allocatorHelper, world.Name);
			this.m_Impl->m_EntityManager.Initialize(world);
		}

		// Token: 0x06001745 RID: 5957 RVA: 0x00057F14 File Offset: 0x00056114
		[ExcludeFromBurstCompatTesting("AllocatorManager accesses managed delegates")]
		internal unsafe void Dispose()
		{
			AllocatorManager.AllocatorHandle handle = this.m_Impl->m_WorldAllocatorHelper.Allocator.Handle;
			AllocatorHelper<AutoFreeAllocator> worldAllocatorHelper = this.m_Impl->m_WorldAllocatorHelper;
			UnsafeUtility.AsRef<WorldUnmanagedImpl>((void*)this.m_Impl).Dispose();
			(ref handle).Free<AllocatorManager.AllocatorHandle>((void*)this.m_Impl, sizeof(WorldUnmanagedImpl), 16, 1);
			this.m_Impl = null;
			worldAllocatorHelper.Allocator.Dispose();
			worldAllocatorHelper.Dispose();
			*WorldUnmanaged.NextSequenceNumber.Data += 1UL;
		}

		// Token: 0x06001746 RID: 5958 RVA: 0x00057F94 File Offset: 0x00056194
		internal unsafe ref WorldUnmanagedImpl GetImpl()
		{
			return UnsafeUtility.AsRef<WorldUnmanagedImpl>((void*)this.m_Impl);
		}

		// Token: 0x06001747 RID: 5959 RVA: 0x00057FA1 File Offset: 0x000561A1
		internal unsafe WorldUnmanagedImpl* GetImplPtr()
		{
			return this.m_Impl;
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06001748 RID: 5960 RVA: 0x00057FA9 File Offset: 0x000561A9
		public unsafe ref RewindableAllocator UpdateAllocator
		{
			get
			{
				return this.GetImpl().DoubleUpdateAllocators->Allocator;
			}
		}

		// Token: 0x06001749 RID: 5961 RVA: 0x00057FBB File Offset: 0x000561BB
		public unsafe void ResetUpdateAllocator()
		{
			this.GetImpl().DoubleUpdateAllocators->Update();
		}

		// Token: 0x0600174A RID: 5962 RVA: 0x00057FD0 File Offset: 0x000561D0
		[GenerateTestsForBurstCompatibility]
		internal unsafe SystemState* TryGetSystemStateForId(int systemId)
		{
			IntPtr intPtr;
			if (!this.GetImpl().m_SystemStatePtrMap.TryGetValue(systemId, out intPtr))
			{
				return null;
			}
			return (SystemState*)(void*)intPtr;
		}

		// Token: 0x0600174B RID: 5963 RVA: 0x00057FFB File Offset: 0x000561FB
		[ExcludeFromBurstCompatTesting("Takes managed system")]
		internal unsafe SystemState* AllocateSystemStateForManagedSystem(World self, ComponentSystemBase system)
		{
			return this.GetImpl().AllocateSystemStateForManagedSystem(self, system);
		}

		// Token: 0x0600174C RID: 5964 RVA: 0x0005800A File Offset: 0x0005620A
		[ExcludeFromBurstCompatTesting("Takes managed World")]
		internal SystemHandle CreateUnmanagedSystem<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(World self, bool callOnCreate) where T : struct, ValueType, ISystem
		{
			return this.GetImpl().CreateUnmanagedSystem<T>(self, callOnCreate);
		}

		// Token: 0x0600174D RID: 5965 RVA: 0x00058019 File Offset: 0x00056219
		[ExcludeFromBurstCompatTesting("Uses managed World under the hood")]
		internal SystemHandle GetOrCreateUnmanagedSystem<[global::System.Runtime.CompilerServices.IsUnmanaged] T>() where T : struct, ValueType, ISystem
		{
			return this.GetImpl().GetOrCreateUnmanagedSystem<T>(true);
		}

		// Token: 0x0600174E RID: 5966 RVA: 0x00058027 File Offset: 0x00056227
		[ExcludeFromBurstCompatTesting("Uses managed World under the hood")]
		internal SystemHandle GetOrCreateUnmanagedSystem(SystemTypeIndex unmanagedType)
		{
			return this.GetImpl().GetOrCreateUnmanagedSystem(unmanagedType, true);
		}

		// Token: 0x0600174F RID: 5967 RVA: 0x00058036 File Offset: 0x00056236
		[ExcludeFromBurstCompatTesting("Uses managed World under the hood")]
		internal SystemHandle CreateUnmanagedSystem(SystemTypeIndex unmanagedType, bool callOnCreate)
		{
			return this.GetImpl().CreateUnmanagedSystem(unmanagedType, callOnCreate);
		}

		// Token: 0x06001750 RID: 5968 RVA: 0x00058045 File Offset: 0x00056245
		[ExcludeFromBurstCompatTesting("accesses managed World")]
		internal void DestroyUnmanagedSystem(SystemHandle sysHandle)
		{
			this.GetImpl().DestroyUnmanagedSystem(sysHandle);
		}

		// Token: 0x06001751 RID: 5969 RVA: 0x00058053 File Offset: 0x00056253
		[ExcludeFromBurstCompatTesting("accesses managed World")]
		internal unsafe void DestroyManagedSystemState(SystemState* state)
		{
			this.GetImpl().DestroyManagedSystem(state);
		}

		// Token: 0x06001752 RID: 5970 RVA: 0x00058061 File Offset: 0x00056261
		internal unsafe SystemState* ResolveSystemStateChecked(SystemHandle id)
		{
			return this.GetImpl().ResolveSystemStateChecked(id);
		}

		// Token: 0x06001753 RID: 5971 RVA: 0x0005806F File Offset: 0x0005626F
		internal unsafe SystemState* ResolveSystemState(SystemHandle id)
		{
			return this.GetImpl().ResolveSystemState(id);
		}

		// Token: 0x06001754 RID: 5972 RVA: 0x00058061 File Offset: 0x00056261
		public unsafe ref SystemState ResolveSystemStateRef(SystemHandle id)
		{
			return ref *this.GetImpl().ResolveSystemStateChecked(id);
		}

		// Token: 0x06001755 RID: 5973 RVA: 0x0005807D File Offset: 0x0005627D
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleSystem) })]
		public ref T GetUnsafeSystemRef<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(SystemHandle id) where T : struct, ValueType, ISystem
		{
			return UnsafeUtility.AsRef<T>(this.ResolveSystemStateRef(id).m_SystemPtr);
		}

		// Token: 0x06001756 RID: 5974 RVA: 0x00058090 File Offset: 0x00056290
		[ExcludeFromBurstCompatTesting("Returns managed Type")]
		internal unsafe Type GetTypeOfSystem(SystemHandle SystemHandle)
		{
			SystemState* ptr = this.ResolveSystemState(SystemHandle);
			if (ptr == null)
			{
				return null;
			}
			if (ptr->m_ManagedSystem.IsAllocated)
			{
				return ptr->m_ManagedSystem.Target.GetType();
			}
			return SystemBaseRegistry.GetStructType(ptr->UnmanagedMetaIndex);
		}

		// Token: 0x06001757 RID: 5975 RVA: 0x000580D5 File Offset: 0x000562D5
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleSystem) })]
		public SystemHandle GetExistingUnmanagedSystem<[global::System.Runtime.CompilerServices.IsUnmanaged] T>() where T : struct, ValueType, ISystem
		{
			return this.GetImpl().GetExistingUnmanagedSystem<T>();
		}

		// Token: 0x06001758 RID: 5976 RVA: 0x000580E2 File Offset: 0x000562E2
		[GenerateTestsForBurstCompatibility(GenericTypeArguments = new Type[] { typeof(BurstCompatibleSystem) })]
		public ref SystemState GetExistingSystemState<T>()
		{
			return this.GetImpl().GetExistingSystemState<T>();
		}

		// Token: 0x06001759 RID: 5977 RVA: 0x000580EF File Offset: 0x000562EF
		[ExcludeFromBurstCompatTesting("Takes System.Type")]
		public SystemHandle GetExistingUnmanagedSystem(Type type)
		{
			return this.GetImpl().GetExistingUnmanagedSystem(type);
		}

		// Token: 0x0600175A RID: 5978 RVA: 0x000580FD File Offset: 0x000562FD
		public SystemHandle GetExistingUnmanagedSystem(SystemTypeIndex type)
		{
			return this.GetImpl().GetExistingUnmanagedSystem(type);
		}

		// Token: 0x0600175B RID: 5979 RVA: 0x0005810B File Offset: 0x0005630B
		public bool IsSystemValid(SystemHandle id)
		{
			return this.GetImpl().IsSystemValid(id);
		}

		// Token: 0x0600175C RID: 5980 RVA: 0x00058119 File Offset: 0x00056319
		[Obsolete("Use GetUnsafeSystemRef (UnityUpgradable) -> GetUnsafeSystemRef<T>(*)", true)]
		public unsafe ref T ResolveSystem<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(SystemHandle systemHandle) where T : struct, ValueType, ISystem
		{
			return UnsafeUtility.AsRef<T>(this.ResolveSystemState(systemHandle)->m_SystemPtr);
		}

		// Token: 0x0600175D RID: 5981 RVA: 0x0005812C File Offset: 0x0005632C
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		internal unsafe void CheckSystemReference(SystemState* ptr)
		{
			if (ptr == null)
			{
				throw new InvalidOperationException("System reference is not valid");
			}
		}

		// Token: 0x0600175E RID: 5982 RVA: 0x0005813E File Offset: 0x0005633E
		public NativeArray<SystemHandle> GetAllUnmanagedSystems(AllocatorManager.AllocatorHandle a)
		{
			return this.GetImpl().GetAllUnmanagedSystems(a);
		}

		// Token: 0x0600175F RID: 5983 RVA: 0x0005814C File Offset: 0x0005634C
		public NativeArray<SystemHandle> GetAllSystems(AllocatorManager.AllocatorHandle a)
		{
			return this.GetImpl().GetAllSystems(a);
		}

		// Token: 0x06001760 RID: 5984 RVA: 0x0005815C File Offset: 0x0005635C
		[ExcludeFromBurstCompatTesting("Accesses managed World under the hood")]
		internal unsafe NativeArray<SystemHandle> GetOrCreateUnmanagedSystems(NativeList<SystemTypeIndex> unmanagedTypes)
		{
			int length = unmanagedTypes.Length;
			NativeArray<SystemHandle> nativeArray = new NativeArray<SystemHandle>(length, Allocator.Temp, NativeArrayOptions.ClearMemory);
			WorldUnmanagedImpl* implPtr = this.GetImplPtr();
			for (int i = 0; i < length; i++)
			{
				nativeArray[i] = implPtr->GetOrCreateUnmanagedSystem(unmanagedTypes[i], false);
			}
			for (int j = 0; j < length; j++)
			{
				SystemState* ptr = implPtr->ResolveSystemState(nativeArray[j]);
				if (ptr != null)
				{
					implPtr->CallSystemOnCreateWithCleanup(ptr);
				}
			}
			return nativeArray;
		}

		// Token: 0x040008CC RID: 2252
		private unsafe WorldUnmanagedImpl* m_Impl;

		// Token: 0x040008CD RID: 2253
		internal static readonly SharedStatic<ulong> NextSequenceNumber = SharedStatic<ulong>.GetOrCreateUnsafe(0U, 8130544679624763247L, 0L);
	}
}
