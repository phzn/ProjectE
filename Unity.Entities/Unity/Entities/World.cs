using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Core;
using Unity.Profiling;

namespace Unity.Entities
{
	// Token: 0x0200034F RID: 847
	[DebuggerDisplay("{Name} - {Flags} (#{SequenceNumber})")]
	[DebuggerTypeProxy(typeof(WorldDebugView))]
	public class World : IDisposable
	{
		// Token: 0x170001DB RID: 475
		// (get) Token: 0x0600167F RID: 5759 RVA: 0x00055948 File Offset: 0x00053B48
		// (set) Token: 0x06001680 RID: 5760 RVA: 0x0005594F File Offset: 0x00053B4F
		public static World DefaultGameObjectInjectionWorld { get; set; }

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06001681 RID: 5761 RVA: 0x00055957 File Offset: 0x00053B57
		public static World.NoAllocReadOnlyCollection<World> All { get; } = new World.NoAllocReadOnlyCollection<World>(World.s_AllWorlds);

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06001682 RID: 5762 RVA: 0x0005595E File Offset: 0x00053B5E
		public unsafe static ulong NextSequenceNumber
		{
			get
			{
				return *WorldUnmanaged.NextSequenceNumber.Data;
			}
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06001683 RID: 5763 RVA: 0x0005596C File Offset: 0x00053B6C
		// (remove) Token: 0x06001684 RID: 5764 RVA: 0x000559A0 File Offset: 0x00053BA0
		internal static event Action<World, ComponentSystemBase> SystemCreated;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06001685 RID: 5765 RVA: 0x000559D4 File Offset: 0x00053BD4
		// (remove) Token: 0x06001686 RID: 5766 RVA: 0x00055A08 File Offset: 0x00053C08
		internal static event Action<World, ComponentSystemBase> SystemDestroyed;

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06001687 RID: 5767 RVA: 0x00055A3B File Offset: 0x00053C3B
		public World.NoAllocReadOnlyCollection<ComponentSystemBase> Systems { get; }

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06001688 RID: 5768 RVA: 0x00055A43 File Offset: 0x00053C43
		public WorldUnmanaged Unmanaged
		{
			get
			{
				return this.m_Unmanaged;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06001689 RID: 5769 RVA: 0x00055A4B File Offset: 0x00053C4B
		public WorldFlags Flags
		{
			get
			{
				return this.m_Unmanaged.Flags;
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x0600168A RID: 5770 RVA: 0x00055A58 File Offset: 0x00053C58
		public string Name { get; }

		// Token: 0x0600168B RID: 5771 RVA: 0x00055A60 File Offset: 0x00053C60
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x0600168C RID: 5772 RVA: 0x00055A68 File Offset: 0x00053C68
		public int Version
		{
			get
			{
				return this.m_Unmanaged.Version;
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x0600168D RID: 5773 RVA: 0x00055A75 File Offset: 0x00053C75
		public EntityManager EntityManager
		{
			get
			{
				return this.m_Unmanaged.EntityManager;
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x0600168E RID: 5774 RVA: 0x00055A82 File Offset: 0x00053C82
		public bool IsCreated
		{
			get
			{
				return this.m_Unmanaged.IsCreated;
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x0600168F RID: 5775 RVA: 0x00055A8F File Offset: 0x00053C8F
		public ulong SequenceNumber
		{
			get
			{
				return this.m_Unmanaged.SequenceNumber;
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06001690 RID: 5776 RVA: 0x00055A9C File Offset: 0x00053C9C
		public ref TimeData Time
		{
			get
			{
				return this.m_Unmanaged.Time;
			}
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06001691 RID: 5777 RVA: 0x00055AA9 File Offset: 0x00053CA9
		// (set) Token: 0x06001692 RID: 5778 RVA: 0x00055AB6 File Offset: 0x00053CB6
		public bool UpdateAllocatorEnableBlockFree
		{
			get
			{
				return this.m_Unmanaged.UpdateAllocatorEnableBlockFree;
			}
			set
			{
				this.m_Unmanaged.UpdateAllocatorEnableBlockFree = value;
			}
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06001693 RID: 5779 RVA: 0x00055AC4 File Offset: 0x00053CC4
		// (set) Token: 0x06001694 RID: 5780 RVA: 0x00055AD1 File Offset: 0x00053CD1
		public float MaximumDeltaTime
		{
			get
			{
				return this.m_Unmanaged.MaximumDeltaTime;
			}
			set
			{
				this.m_Unmanaged.MaximumDeltaTime = value;
			}
		}

		// Token: 0x06001695 RID: 5781 RVA: 0x00055AE0 File Offset: 0x00053CE0
		public World(string name, WorldFlags flags = WorldFlags.Simulation)
		{
			this.Name = name;
			this.Systems = new World.NoAllocReadOnlyCollection<ComponentSystemBase>(this.m_Systems);
			this.Init(flags, Allocator.Persistent);
		}

		// Token: 0x06001696 RID: 5782 RVA: 0x00055B38 File Offset: 0x00053D38
		public World(string name, WorldFlags flags, AllocatorManager.AllocatorHandle backingAllocatorHandle)
		{
			this.Name = name;
			this.Systems = new World.NoAllocReadOnlyCollection<ComponentSystemBase>(this.m_Systems);
			this.Init(flags, backingAllocatorHandle);
		}

		// Token: 0x06001697 RID: 5783 RVA: 0x00055B8C File Offset: 0x00053D8C
		private void Init(WorldFlags flags, AllocatorManager.AllocatorHandle backingAllocatorHandle)
		{
			this.m_Unmanaged.Create(this, flags, backingAllocatorHandle);
			World.s_AllWorlds.Add(this);
			this.m_TimeSingletonQuery = this.EntityManager.CreateEntityQuery(new ComponentType[]
			{
				ComponentType.ReadWrite<WorldTime>(),
				ComponentType.ReadWrite<WorldTimeQueue>()
			});
		}

		// Token: 0x06001698 RID: 5784 RVA: 0x00055BE4 File Offset: 0x00053DE4
		public void Dispose()
		{
			if (!this.IsCreated)
			{
				throw new ArgumentException("The World has already been Disposed.");
			}
			if (this.Unmanaged.ExecutingSystem != SystemHandle.Null)
			{
				throw new ArgumentException("The World can not be disposed while a system in that world is executing " + this.Unmanaged.ExecutingSystem.m_WorldSeqNo.ToString());
			}
			this.m_Unmanaged.EntityManager.PreDisposeCheck();
			if (this.m_ExternalAPIState != null)
			{
				this.m_Unmanaged.DestroyManagedSystemState(this.m_ExternalAPIState);
			}
			this.m_ExternalAPIState = null;
			this.EntityManager.ExclusiveEntityTransactionDependency.Complete();
			this.EntityManager.EndExclusiveEntityTransaction();
			this.EntityManager.CompleteAllTrackedJobs();
			bool flag;
			this.DestroyAllSystemsAndLogException(out flag);
			this.m_SystemLookup = null;
			this.m_Systems = null;
			this.m_TimeSingletonQuery.Dispose();
			World.s_AllWorlds.Remove(this);
			if (World.DefaultGameObjectInjectionWorld == this)
			{
				World.DefaultGameObjectInjectionWorld = null;
			}
			this.m_Unmanaged.Dispose();
		}

		// Token: 0x06001699 RID: 5785 RVA: 0x00055CF8 File Offset: 0x00053EF8
		public static void DisposeAllWorlds()
		{
			while (World.s_AllWorlds.Count != 0)
			{
				World.s_AllWorlds[0].Dispose();
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x0600169A RID: 5786 RVA: 0x00055D18 File Offset: 0x00053F18
		protected unsafe Entity TimeSingleton
		{
			get
			{
				if (this.m_TimeSingletonQuery.IsEmptyIgnoreFilter)
				{
					ComponentType* ptr;
					checked
					{
						ptr = stackalloc ComponentType[unchecked((UIntPtr)2) * (UIntPtr)sizeof(ComponentType)];
						*ptr = ComponentType.ReadWrite<WorldTime>();
					}
					ptr[1] = ComponentType.ReadWrite<WorldTimeQueue>();
					Entity entity = this.EntityManager.CreateEntity(this.EntityManager.CreateArchetype(ptr, 2));
					this.EntityManager.SetName(entity, "WorldTime");
				}
				return this.m_TimeSingletonQuery.GetSingletonEntity();
			}
		}

		// Token: 0x0600169B RID: 5787 RVA: 0x00055DA0 File Offset: 0x00053FA0
		public unsafe void SetTime(TimeData newTimeData)
		{
			this.EntityManager.SetComponentData<WorldTime>(this.TimeSingleton, new WorldTime
			{
				Time = newTimeData
			});
			*this.Time = newTimeData;
		}

		// Token: 0x0600169C RID: 5788 RVA: 0x00055DE0 File Offset: 0x00053FE0
		public unsafe void PushTime(TimeData newTimeData)
		{
			this.EntityManager.GetBuffer<WorldTimeQueue>(this.TimeSingleton, false).Add(new WorldTimeQueue
			{
				Time = *this.Time
			});
			this.SetTime(newTimeData);
		}

		// Token: 0x0600169D RID: 5789 RVA: 0x00055E30 File Offset: 0x00054030
		public void PopTime()
		{
			DynamicBuffer<WorldTimeQueue> buffer = this.EntityManager.GetBuffer<WorldTimeQueue>(this.TimeSingleton, false);
			WorldTimeQueue worldTimeQueue = buffer[buffer.Length - 1];
			buffer.RemoveAt(buffer.Length - 1);
			this.SetTime(worldTimeQueue.Time);
		}

		// Token: 0x0600169E RID: 5790 RVA: 0x00055E80 File Offset: 0x00054080
		private ComponentSystemBase CreateSystemInternal(SystemTypeIndex type)
		{
			ComponentSystemBase componentSystemBase = this.AllocateSystemInternal(type);
			this.AddSystem_Add_Internal(componentSystemBase);
			this.AddSystem_OnCreate_Internal(componentSystemBase);
			return componentSystemBase;
		}

		// Token: 0x0600169F RID: 5791 RVA: 0x00055EA4 File Offset: 0x000540A4
		private ComponentSystemBase AllocateSystemInternal(SystemTypeIndex type)
		{
			return TypeManager.ConstructSystem(TypeManager.GetSystemType(type));
		}

		// Token: 0x060016A0 RID: 5792 RVA: 0x00055EB4 File Offset: 0x000540B4
		private ComponentSystemBase GetExistingSystemInternal(SystemTypeIndex type)
		{
			ComponentSystemBase componentSystemBase;
			if (this.m_SystemLookup.TryGetValue(TypeManager.GetSystemType(type), out componentSystemBase))
			{
				return componentSystemBase;
			}
			return null;
		}

		// Token: 0x060016A1 RID: 5793 RVA: 0x00055ED9 File Offset: 0x000540D9
		private void AddTypeLookupInternal(Type type, ComponentSystemBase system)
		{
			while (type != typeof(ComponentSystemBase))
			{
				if (!this.m_SystemLookup.ContainsKey(type))
				{
					this.m_SystemLookup.Add(type, system);
				}
				type = type.BaseType;
			}
		}

		// Token: 0x060016A2 RID: 5794 RVA: 0x00055F14 File Offset: 0x00054114
		private void AddSystem_Add_Internal(ComponentSystemBase system)
		{
			this.m_Systems.Add(system);
			Type type = system.GetType();
			system.m_StatePtr = this.m_Unmanaged.AllocateSystemStateForManagedSystem(this, system);
			this.AddTypeLookupInternal(type, system);
			ref WorldUnmanagedImpl impl = ref this.m_Unmanaged.GetImpl();
			PerWorldSystemInfo perWorldSystemInfo = default(PerWorldSystemInfo);
			perWorldSystemInfo.handle = system.SystemHandle;
			perWorldSystemInfo.systemTypeIndex = TypeManager.GetSystemTypeIndex(type);
			impl.sysHandlesInCreationOrder.Add(in perWorldSystemInfo);
		}

		// Token: 0x060016A3 RID: 5795 RVA: 0x00055F90 File Offset: 0x00054190
		private void AddSystem_OnCreate_Internal(ComponentSystemBase system)
		{
			try
			{
				system.CreateInstance(this);
			}
			catch
			{
				this.RemoveSystemInternal(system);
				throw;
			}
			this.m_Unmanaged.BumpVersion();
			Action<World, ComponentSystemBase> systemCreated = World.SystemCreated;
			if (systemCreated == null)
			{
				return;
			}
			systemCreated(this, system);
		}

		// Token: 0x060016A4 RID: 5796 RVA: 0x00055FDC File Offset: 0x000541DC
		private void RemoveSystemInternal(ComponentSystemBase system)
		{
			ref UnsafeList<PerWorldSystemInfo> ptr = ref this.Unmanaged.GetImpl().sysHandlesInCreationOrder;
			int num = -1;
			for (int i = 0; i < ptr.Length; i++)
			{
				if (ptr[i].handle == system.SystemHandle)
				{
					num = i;
					break;
				}
			}
			if (num != -1)
			{
				ptr.RemoveAt(num);
			}
			if (!this.m_Systems.Remove(system))
			{
				throw new ArgumentException("System does not exist in the world");
			}
			this.m_Unmanaged.BumpVersion();
			Type type = system.GetType();
			while (type != typeof(ComponentSystemBase))
			{
				ComponentSystemBase componentSystemBase;
				if (this.m_SystemLookup.TryGetValue(type, out componentSystemBase) && componentSystemBase == system)
				{
					this.m_SystemLookup.Remove(type);
					foreach (ComponentSystemBase componentSystemBase2 in this.m_Systems)
					{
						Type type2 = componentSystemBase2.GetType();
						if (type != type2 && type.IsAssignableFrom(type2))
						{
							this.AddTypeLookupInternal(type2, componentSystemBase2);
							break;
						}
					}
				}
				type = type.BaseType;
			}
		}

		// Token: 0x060016A5 RID: 5797 RVA: 0x00056118 File Offset: 0x00054318
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		internal void CheckGetOrCreateSystem()
		{
			if (!this.IsCreated)
			{
				throw new ObjectDisposedException("The World has already been Disposed.");
			}
		}

		// Token: 0x060016A6 RID: 5798 RVA: 0x00056130 File Offset: 0x00054330
		public SystemHandle GetOrCreateSystem<T>() where T : ComponentSystemBase
		{
			SystemTypeIndex systemTypeIndex = TypeManager.GetSystemTypeIndex<T>();
			ComponentSystemBase existingSystemInternal = this.GetExistingSystemInternal(systemTypeIndex);
			if (existingSystemInternal != null)
			{
				return existingSystemInternal.SystemHandle;
			}
			return this.CreateSystemInternal(systemTypeIndex).SystemHandle;
		}

		// Token: 0x060016A7 RID: 5799 RVA: 0x00056164 File Offset: 0x00054364
		public T GetOrCreateSystemManaged<T>() where T : ComponentSystemBase
		{
			SystemTypeIndex systemTypeIndex = TypeManager.GetSystemTypeIndexNoThrow<T>();
			if (systemTypeIndex == SystemTypeIndex.Null)
			{
				TypeManager.AddSystemTypeToTablesAfterInit(typeof(T));
				systemTypeIndex = TypeManager.GetSystemTypeIndex<T>();
			}
			return (T)((object)this.GetOrCreateSystemManaged(systemTypeIndex));
		}

		// Token: 0x060016A8 RID: 5800 RVA: 0x000561A5 File Offset: 0x000543A5
		public SystemHandle GetOrCreateSystem(Type type)
		{
			return this.GetOrCreateSystem(TypeManager.GetSystemTypeIndex(type));
		}

		// Token: 0x060016A9 RID: 5801 RVA: 0x000561B4 File Offset: 0x000543B4
		public SystemHandle GetOrCreateSystem(SystemTypeIndex type)
		{
			if (!type.IsManaged)
			{
				return this.Unmanaged.GetOrCreateUnmanagedSystem(type);
			}
			ComponentSystemBase existingSystemInternal = this.GetExistingSystemInternal(type);
			if (existingSystemInternal != null)
			{
				return existingSystemInternal.SystemHandle;
			}
			return this.CreateSystemInternal(type).SystemHandle;
		}

		// Token: 0x060016AA RID: 5802 RVA: 0x000561F8 File Offset: 0x000543F8
		public ComponentSystemBase GetOrCreateSystemManaged(Type type)
		{
			return this.GetOrCreateSystemManaged(TypeManager.GetSystemTypeIndex(type));
		}

		// Token: 0x060016AB RID: 5803 RVA: 0x00056206 File Offset: 0x00054406
		public ComponentSystemBase GetOrCreateSystemManaged(SystemTypeIndex type)
		{
			return this.GetExistingSystemInternal(type) ?? this.CreateSystemInternal(type);
		}

		// Token: 0x060016AC RID: 5804 RVA: 0x0005621A File Offset: 0x0005441A
		public SystemHandle CreateSystem<T>() where T : ComponentSystemBase, new()
		{
			return this.CreateSystemInternal(TypeManager.GetSystemTypeIndex<T>()).SystemHandle;
		}

		// Token: 0x060016AD RID: 5805 RVA: 0x0005622C File Offset: 0x0005442C
		public T CreateSystemManaged<T>() where T : ComponentSystemBase, new()
		{
			SystemTypeIndex systemTypeIndex = TypeManager.GetSystemTypeIndexNoThrow<T>();
			if (systemTypeIndex <= 0)
			{
				TypeManager.AddSystemTypeToTablesAfterInit(typeof(T));
				systemTypeIndex = TypeManager.GetSystemTypeIndex<T>();
			}
			return (T)((object)this.CreateSystemManaged(systemTypeIndex));
		}

		// Token: 0x060016AE RID: 5806 RVA: 0x0005626E File Offset: 0x0005446E
		public SystemHandle CreateSystem(Type type)
		{
			return this.CreateSystem(TypeManager.GetSystemTypeIndex(type));
		}

		// Token: 0x060016AF RID: 5807 RVA: 0x0005627C File Offset: 0x0005447C
		public SystemHandle CreateSystem(SystemTypeIndex type)
		{
			if (type.IsManaged)
			{
				return this.CreateSystemInternal(type).SystemHandle;
			}
			return this.Unmanaged.GetOrCreateUnmanagedSystem(type);
		}

		// Token: 0x060016B0 RID: 5808 RVA: 0x000562AE File Offset: 0x000544AE
		public ComponentSystemBase CreateSystemManaged(Type type)
		{
			return this.CreateSystemManaged(TypeManager.GetSystemTypeIndex(type));
		}

		// Token: 0x060016B1 RID: 5809 RVA: 0x000562BC File Offset: 0x000544BC
		public ComponentSystemBase CreateSystemManaged(SystemTypeIndex type)
		{
			return this.CreateSystemInternal(type);
		}

		// Token: 0x060016B2 RID: 5810 RVA: 0x000562C5 File Offset: 0x000544C5
		[Obsolete("(UnityUpgradable) -> AddSystemManaged<T>(*)", true)]
		public T AddSystem<T>(T system) where T : ComponentSystemBase
		{
			return this.AddSystemManaged<T>(system);
		}

		// Token: 0x060016B3 RID: 5811 RVA: 0x000562D0 File Offset: 0x000544D0
		public T AddSystemManaged<T>(T system) where T : ComponentSystemBase
		{
			SystemTypeIndex systemTypeIndex = TypeManager.GetSystemTypeIndexNoThrow<T>();
			if (systemTypeIndex <= 0)
			{
				TypeManager.AddSystemTypeToTablesAfterInit(typeof(T));
				systemTypeIndex = TypeManager.GetSystemTypeIndex<T>();
			}
			if (this.GetExistingSystemInternal(systemTypeIndex) != null)
			{
				throw new Exception(string.Format("Attempting to add system '{0}' which has already been added to world '{1}'", TypeManager.GetSystemName(systemTypeIndex), this.Name));
			}
			this.AddSystem_Add_Internal(system);
			this.AddSystem_OnCreate_Internal(system);
			return system;
		}

		// Token: 0x060016B4 RID: 5812 RVA: 0x00056349 File Offset: 0x00054549
		public SystemHandle GetExistingSystem<T>() where T : ComponentSystemBase
		{
			return this.GetExistingSystem(TypeManager.GetSystemTypeIndex<T>());
		}

		// Token: 0x060016B5 RID: 5813 RVA: 0x00056356 File Offset: 0x00054556
		public T GetExistingSystemManaged<T>() where T : ComponentSystemBase
		{
			return (T)((object)this.GetExistingSystemManaged(typeof(T)));
		}

		// Token: 0x060016B6 RID: 5814 RVA: 0x0005636D File Offset: 0x0005456D
		public SystemHandle GetExistingSystem(Type type)
		{
			return this.GetExistingSystem(TypeManager.GetSystemTypeIndex(type));
		}

		// Token: 0x060016B7 RID: 5815 RVA: 0x0005637C File Offset: 0x0005457C
		public SystemHandle GetExistingSystem(SystemTypeIndex type)
		{
			if (!type.IsManaged)
			{
				return this.Unmanaged.GetExistingUnmanagedSystem(type);
			}
			ComponentSystemBase existingSystemInternal = this.GetExistingSystemInternal(type);
			if (existingSystemInternal != null)
			{
				return existingSystemInternal.SystemHandle;
			}
			return default(SystemHandle);
		}

		// Token: 0x060016B8 RID: 5816 RVA: 0x000563BD File Offset: 0x000545BD
		public ComponentSystemBase GetExistingSystemManaged(Type type)
		{
			return this.GetExistingSystemManaged(TypeManager.GetSystemTypeIndex(type));
		}

		// Token: 0x060016B9 RID: 5817 RVA: 0x000563CB File Offset: 0x000545CB
		public ComponentSystemBase GetExistingSystemManaged(SystemTypeIndex type)
		{
			return this.GetExistingSystemInternal(type);
		}

		// Token: 0x060016BA RID: 5818 RVA: 0x000563D4 File Offset: 0x000545D4
		public unsafe void DestroySystem(SystemHandle sysHandle)
		{
			SystemState* ptr = this.Unmanaged.ResolveSystemStateChecked(sysHandle);
			if (ptr != null && ptr->m_ManagedSystem.IsAllocated)
			{
				this.DestroySystemManaged(ptr->ManagedSystem);
				return;
			}
			this.Unmanaged.DestroyUnmanagedSystem(sysHandle);
		}

		// Token: 0x060016BB RID: 5819 RVA: 0x0005641F File Offset: 0x0005461F
		public void DestroySystemManaged(ComponentSystemBase system)
		{
			Action<World, ComponentSystemBase> systemDestroyed = World.SystemDestroyed;
			if (systemDestroyed != null)
			{
				systemDestroyed(this, system);
			}
			this.RemoveSystemInternal(system);
			system.DestroyInstance();
		}

		// Token: 0x060016BC RID: 5820 RVA: 0x00056440 File Offset: 0x00054640
		[Obsolete("This function now takes an out bool parameter to report if any exceptions were logged.", false)]
		public void DestroyAllSystemsAndLogException()
		{
			bool flag;
			this.DestroyAllSystemsAndLogException(out flag);
		}

		// Token: 0x060016BD RID: 5821 RVA: 0x00056458 File Offset: 0x00054658
		public unsafe void DestroyAllSystemsAndLogException(out bool loggedException)
		{
			loggedException = false;
			if (!this.IsCreated)
			{
				return;
			}
			if (this.Unmanaged.ExecutingSystem != default(SystemHandle))
			{
				throw new ArgumentException("DestroyAllSystemsAndLogException while another system is running on the same world is not allowed.");
			}
			UnsafeList<PerWorldSystemInfo> sysHandlesInCreationOrder = this.Unmanaged.GetImpl().sysHandlesInCreationOrder;
			for (int i = sysHandlesInCreationOrder.Length - 1; i >= 0; i--)
			{
				SystemHandle handle = sysHandlesInCreationOrder[i].handle;
				SystemState* ptr = this.Unmanaged.ResolveSystemState(handle);
				if (ptr != null)
				{
					if (ptr->m_ManagedSystem.IsAllocated)
					{
						try
						{
							Action<World, ComponentSystemBase> systemDestroyed = World.SystemDestroyed;
							if (systemDestroyed != null)
							{
								systemDestroyed(this, this.m_Systems[i]);
							}
							ptr->ManagedSystem.OnBeforeDestroyInternal();
							goto IL_00F1;
						}
						catch (Exception ex)
						{
							loggedException = true;
							Debug.LogException(ex);
							goto IL_00F1;
						}
					}
					if (ptr->PreviouslyEnabled && (TypeManager.GetSystemTypeFlags(sysHandlesInCreationOrder[i].systemTypeIndex) & 268435456) != 0)
					{
						SystemBaseRegistry.CallOnStopRunning(ptr);
					}
				}
				IL_00F1:;
			}
			for (int j = sysHandlesInCreationOrder.Length - 1; j >= 0; j--)
			{
				SystemHandle handle2 = sysHandlesInCreationOrder[j].handle;
				try
				{
					SystemState* ptr2 = this.Unmanaged.ResolveSystemState(handle2);
					if (ptr2 != null)
					{
						if (ptr2->m_ManagedSystem.IsAllocated)
						{
							ptr2->ManagedSystem.OnDestroy_Internal();
						}
						else
						{
							SystemBaseRegistry.CallOnDestroy(ptr2);
						}
					}
				}
				catch (Exception ex2)
				{
					loggedException = true;
					Debug.LogException(ex2);
				}
			}
			for (int k = sysHandlesInCreationOrder.Length - 1; k >= 0; k--)
			{
				SystemHandle handle3 = sysHandlesInCreationOrder[k].handle;
				try
				{
					SystemState* ptr3 = this.Unmanaged.ResolveSystemState(handle3);
					if (ptr3 != null)
					{
						if (ptr3->m_ManagedSystem.IsAllocated)
						{
							ptr3->ManagedSystem.OnAfterDestroyInternal();
						}
						else
						{
							ref WorldUnmanagedImpl impl = ref this.m_Unmanaged.GetImpl();
							impl.InvalidateSystemHandle(handle3);
							impl.FreeSlotWithoutOnDestroy(handle3.m_Handle, ptr3);
						}
					}
				}
				catch (Exception ex3)
				{
					loggedException = true;
					Debug.LogException(ex3);
				}
			}
			this.m_Systems.Clear();
			this.Unmanaged.GetImpl().sysHandlesInCreationOrder.Clear();
			this.m_SystemLookup.Clear();
		}

		// Token: 0x060016BE RID: 5822 RVA: 0x000566B4 File Offset: 0x000548B4
		internal unsafe NativeList<SystemHandle> GetOrCreateSystemsAndLogException(NativeList<SystemTypeIndex> types, int typesCount, AllocatorManager.AllocatorHandle allocator)
		{
			NativeList<SystemHandle> nativeList = new NativeList<SystemHandle>(typesCount, allocator);
			int length = nativeList.Length;
			NativeList<SystemTypeIndex> nativeList2 = new NativeList<SystemTypeIndex>(16, Allocator.Temp);
			for (int i = 0; i < types.Length; i++)
			{
				SystemTypeIndex systemTypeIndex = types[i];
				SystemHandle systemHandle = SystemHandle.Null;
				try
				{
					if (!systemTypeIndex.IsManaged)
					{
						systemHandle = this.m_Unmanaged.GetExistingUnmanagedSystem(systemTypeIndex);
						if (!(systemHandle != default(SystemHandle)))
						{
							systemHandle = this.Unmanaged.CreateUnmanagedSystem(systemTypeIndex, false);
							nativeList2.Add(in systemTypeIndex);
						}
					}
					else
					{
						ComponentSystemBase componentSystemBase = this.GetExistingSystemInternal(systemTypeIndex);
						if (componentSystemBase != null)
						{
							systemHandle = componentSystemBase.SystemHandle;
						}
						else
						{
							componentSystemBase = this.AllocateSystemInternal(systemTypeIndex);
							if (componentSystemBase != null)
							{
								this.AddSystem_Add_Internal(componentSystemBase);
								systemHandle = componentSystemBase.SystemHandle;
								nativeList2.Add(in systemTypeIndex);
							}
						}
					}
				}
				catch (Exception ex)
				{
					Debug.LogException(ex);
				}
				finally
				{
					nativeList.Add(in systemHandle);
					if (nativeList2.Length < nativeList.Length)
					{
						SystemTypeIndex @null = SystemTypeIndex.Null;
						nativeList2.Add(in @null);
					}
				}
			}
			for (int num = length; num != length + nativeList2.Length; num++)
			{
				try
				{
					SystemTypeIndex systemTypeIndex2 = nativeList2[num - length];
					if (!(systemTypeIndex2 == SystemTypeIndex.Null))
					{
						if (!systemTypeIndex2.IsManaged)
						{
							SystemHandle existingUnmanagedSystem = this.m_Unmanaged.GetExistingUnmanagedSystem(systemTypeIndex2);
							SystemState* ptr = this.m_Unmanaged.ResolveSystemState(existingUnmanagedSystem);
							if (ptr != null)
							{
								this.m_Unmanaged.GetImplPtr()->CallSystemOnCreateWithCleanup(ptr);
							}
						}
						else
						{
							ComponentSystemBase componentSystemBase2;
							this.m_SystemLookup.TryGetValue(TypeManager.GetSystemType(systemTypeIndex2), out componentSystemBase2);
							this.AddSystem_OnCreate_Internal(componentSystemBase2);
						}
					}
				}
				catch (Exception ex2)
				{
					nativeList[num - length] = default(SystemHandle);
					Debug.LogException(ex2);
				}
			}
			return nativeList;
		}

		// Token: 0x060016BF RID: 5823 RVA: 0x000568B0 File Offset: 0x00054AB0
		public NativeList<SystemHandle> GetOrCreateSystemsAndLogException(NativeList<SystemTypeIndex> types, AllocatorManager.AllocatorHandle allocator)
		{
			return this.GetOrCreateSystemsAndLogException(types, types.Length, allocator);
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x060016C0 RID: 5824 RVA: 0x000568C1 File Offset: 0x00054AC1
		// (set) Token: 0x060016C1 RID: 5825 RVA: 0x000568C9 File Offset: 0x00054AC9
		public bool QuitUpdate { get; set; }

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x060016C2 RID: 5826 RVA: 0x000568D4 File Offset: 0x00054AD4
		public ref RewindableAllocator UpdateAllocator
		{
			get
			{
				return this.Unmanaged.UpdateAllocator;
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x060016C3 RID: 5827 RVA: 0x000568EF File Offset: 0x00054AEF
		public unsafe DoubleRewindableAllocators* CurrentGroupAllocators
		{
			get
			{
				return this.m_Unmanaged.GetImpl().DoubleUpdateAllocators;
			}
		}

		// Token: 0x060016C4 RID: 5828 RVA: 0x00056901 File Offset: 0x00054B01
		public unsafe void SetGroupAllocator(DoubleRewindableAllocators* newGroupAllocators)
		{
			this.m_Unmanaged.GetImpl().SetGroupAllocator(newGroupAllocators);
		}

		// Token: 0x060016C5 RID: 5829 RVA: 0x00056914 File Offset: 0x00054B14
		public unsafe void RestoreGroupAllocator(DoubleRewindableAllocators* oldGroupAllocators)
		{
			this.m_Unmanaged.GetImpl().RestoreGroupAllocator(oldGroupAllocators);
		}

		// Token: 0x060016C6 RID: 5830 RVA: 0x00056928 File Offset: 0x00054B28
		public void Update()
		{
			this.m_Unmanaged.GetImpl().m_WorldAllocatorHelper.Allocator.Update();
			InitializationSystemGroup existingSystemManaged = this.GetExistingSystemManaged<InitializationSystemGroup>();
			if (existingSystemManaged != null)
			{
				existingSystemManaged.Update();
			}
			SimulationSystemGroup existingSystemManaged2 = this.GetExistingSystemManaged<SimulationSystemGroup>();
			if (existingSystemManaged2 != null)
			{
				existingSystemManaged2.Update();
			}
			PresentationSystemGroup existingSystemManaged3 = this.GetExistingSystemManaged<PresentationSystemGroup>();
			if (existingSystemManaged3 == null)
			{
				return;
			}
			existingSystemManaged3.Update();
		}

		// Token: 0x060016C7 RID: 5831 RVA: 0x00056984 File Offset: 0x00054B84
		internal unsafe static SystemState* FindSystemStateForChangeVersion(EntityComponentStore* componentStore, uint changeVersion)
		{
			foreach (World world in World.All)
			{
				if (world.EntityManager.GetUncheckedEntityDataAccess()->EntityComponentStore == componentStore)
				{
					if (changeVersion == componentStore->GlobalSystemVersion)
					{
						SystemState* ptr = world.m_Unmanaged.ResolveSystemState(world.m_Unmanaged.ExecutingSystem);
						if (ptr != null)
						{
							return ptr;
						}
					}
					foreach (ComponentSystemBase componentSystemBase in world.Systems)
					{
						if (componentSystemBase != null)
						{
							SystemState* ptr2 = componentSystemBase.CheckedState();
							if (ptr2->m_LastSystemVersion == changeVersion)
							{
								return ptr2;
							}
						}
					}
				}
			}
			return null;
		}

		// Token: 0x060016C8 RID: 5832 RVA: 0x00056A80 File Offset: 0x00054C80
		internal unsafe static SystemState* FindSystemStateForId(int systemId)
		{
			foreach (World world in World.All)
			{
				SystemState* ptr = world.Unmanaged.TryGetSystemStateForId(systemId);
				if (ptr != null)
				{
					return ptr;
				}
			}
			return null;
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x060016C9 RID: 5833 RVA: 0x00056AEC File Offset: 0x00054CEC
		[ExcludeFromBurstCompatTesting("accesses managed stub system")]
		internal unsafe SystemState* ExternalAPIState
		{
			get
			{
				if (this.m_ExternalAPIState == null)
				{
					this.m_ExternalAPIState = this.Unmanaged.AllocateSystemStateForManagedSystem(this, new World.SystemStub());
				}
				return this.m_ExternalAPIState;
			}
		}

		// Token: 0x04000882 RID: 2178
		internal static readonly List<World> s_AllWorlds = new List<World>();

		// Token: 0x04000884 RID: 2180
		internal Dictionary<Type, ComponentSystemBase> m_SystemLookup = new Dictionary<Type, ComponentSystemBase>();

		// Token: 0x04000888 RID: 2184
		private List<ComponentSystemBase> m_Systems = new List<ComponentSystemBase>();

		// Token: 0x0400088A RID: 2186
		private static ProfilerMarker s_NewWorldMarker = new ProfilerMarker("new World()");

		// Token: 0x0400088B RID: 2187
		private static ProfilerMarker s_DisposeWorldMarker = new ProfilerMarker("World Dispose()");

		// Token: 0x0400088C RID: 2188
		private WorldUnmanaged m_Unmanaged;

		// Token: 0x0400088E RID: 2190
		private EntityQuery m_TimeSingletonQuery;

		// Token: 0x04000890 RID: 2192
		[NativeDisableUnsafePtrRestriction]
		private unsafe SystemState* m_ExternalAPIState = null;

		// Token: 0x02000350 RID: 848
		public struct NoAllocReadOnlyCollection<T> : IEnumerable<T>, IEnumerable
		{
			// Token: 0x060016CB RID: 5835 RVA: 0x00056B5C File Offset: 0x00054D5C
			public NoAllocReadOnlyCollection(List<T> source)
			{
				this.m_Source = source;
			}

			// Token: 0x170001EE RID: 494
			// (get) Token: 0x060016CC RID: 5836 RVA: 0x00056B65 File Offset: 0x00054D65
			public int Count
			{
				get
				{
					return this.m_Source.Count;
				}
			}

			// Token: 0x170001EF RID: 495
			public T this[int index]
			{
				get
				{
					return this.m_Source[index];
				}
			}

			// Token: 0x060016CE RID: 5838 RVA: 0x00056B80 File Offset: 0x00054D80
			public List<T>.Enumerator GetEnumerator()
			{
				return this.m_Source.GetEnumerator();
			}

			// Token: 0x060016CF RID: 5839 RVA: 0x00056B8D File Offset: 0x00054D8D
			public bool Contains(T item)
			{
				return this.m_Source.Contains(item);
			}

			// Token: 0x060016D0 RID: 5840 RVA: 0x00056B9B File Offset: 0x00054D9B
			IEnumerator<T> IEnumerable<T>.GetEnumerator()
			{
				throw new NotSupportedException("To avoid boxing, do not cast NoAllocReadOnlyCollection to IEnumerable<T>.");
			}

			// Token: 0x060016D1 RID: 5841 RVA: 0x00056BA7 File Offset: 0x00054DA7
			IEnumerator IEnumerable.GetEnumerator()
			{
				throw new NotSupportedException("To avoid boxing, do not cast NoAllocReadOnlyCollection to IEnumerable.");
			}

			// Token: 0x04000891 RID: 2193
			private readonly List<T> m_Source;
		}

		// Token: 0x02000351 RID: 849
		[DisableAutoCreation]
		private class SystemStub : ComponentSystemBase
		{
			// Token: 0x060016D2 RID: 5842 RVA: 0x00038687 File Offset: 0x00036887
			public override void Update()
			{
				throw new NotImplementedException();
			}
		}
	}
}
