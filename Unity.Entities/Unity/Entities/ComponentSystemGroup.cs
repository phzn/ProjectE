using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine.Scripting;

namespace Unity.Entities
{
	// Token: 0x0200003A RID: 58
	[DebuggerTypeProxy(typeof(ComponentSystemGroupDebugView))]
	public abstract class ComponentSystemGroup : SystemBase
	{
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x000080BE File Offset: 0x000062BE
		// (set) Token: 0x060001A6 RID: 422 RVA: 0x000080C6 File Offset: 0x000062C6
		public bool EnableSystemSorting
		{
			get
			{
				return this.m_EnableSystemSorting;
			}
			protected set
			{
				if (value && !this.m_EnableSystemSorting)
				{
					this.m_systemSortDirty = true;
				}
				this.m_EnableSystemSorting = value;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x000080E1 File Offset: 0x000062E1
		// (set) Token: 0x060001A8 RID: 424 RVA: 0x000080E9 File Offset: 0x000062E9
		public bool Created { get; private set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x000080F2 File Offset: 0x000062F2
		public virtual IReadOnlyList<ComponentSystemBase> ManagedSystems
		{
			get
			{
				return this.m_managedSystemsToUpdate;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060001AA RID: 426 RVA: 0x000080FA File Offset: 0x000062FA
		internal UnsafeList<SystemHandle> UnmanagedSystems
		{
			get
			{
				return this.m_UnmanagedSystemsToUpdate;
			}
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00008104 File Offset: 0x00006304
		public NativeList<SystemHandle> GetUnmanagedSystems(Allocator allocator = Allocator.Temp)
		{
			NativeList<SystemHandle> nativeList = new NativeList<SystemHandle>(this.m_UnmanagedSystemsToUpdate.Length, allocator);
			nativeList.CopyFrom(in this.m_UnmanagedSystemsToUpdate);
			return nativeList;
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00008138 File Offset: 0x00006338
		public NativeList<SystemHandle> GetAllSystems(Allocator allocator = Allocator.Temp)
		{
			NativeList<SystemHandle> nativeList = new NativeList<SystemHandle>(16, allocator);
			for (int i = 0; i < this.m_MasterUpdateList.Length; i++)
			{
				UpdateIndex updateIndex = this.m_MasterUpdateList[i];
				SystemHandle systemHandle = (updateIndex.IsManaged ? this.m_managedSystemsToUpdate[updateIndex.Index].SystemHandle : this.m_UnmanagedSystemsToUpdate[updateIndex.Index]);
				nativeList.Add(in systemHandle);
			}
			return nativeList;
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060001AD RID: 429 RVA: 0x000081B6 File Offset: 0x000063B6
		// (set) Token: 0x060001AE RID: 430 RVA: 0x000081BE File Offset: 0x000063BE
		internal byte RateGroupAllocatorsCreated { get; set; }

		// Token: 0x060001AF RID: 431 RVA: 0x000081C8 File Offset: 0x000063C8
		[Preserve]
		protected override void OnCreate()
		{
			base.OnCreate();
			this.m_UnmanagedSystemsToUpdate = new UnsafeList<SystemHandle>(0, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			this.m_UnmanagedSystemsToRemove = new UnsafeList<SystemHandle>(0, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			this.m_MasterUpdateList = new UnsafeList<UpdateIndex>(0, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			this.Created = true;
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x0000821C File Offset: 0x0000641C
		[Preserve]
		protected override void OnDestroy()
		{
			if (this.RateGroupAllocatorsCreated == 1)
			{
				this.DestroyRateGroupAllocators();
			}
			this.m_MasterUpdateList.Dispose();
			this.m_UnmanagedSystemsToRemove.Dispose();
			this.m_UnmanagedSystemsToUpdate.Dispose();
			base.OnDestroy();
			this.Created = false;
			this.m_managedSystemsToUpdate.Clear();
			this.m_managedSystemsToRemove.Clear();
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x0000827C File Offset: 0x0000647C
		private void CheckCreated()
		{
			if (!this.Created)
			{
				throw new InvalidOperationException(string.Format("Group of type {0} has not been created, either the derived class forgot to call base.OnCreate(), or it has been destroyed", base.GetType()));
			}
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x0000829C File Offset: 0x0000649C
		public void AddSystemToUpdateList(ComponentSystemBase sys)
		{
			this.CheckCreated();
			if (sys != null)
			{
				if (this == sys)
				{
					throw new ArgumentException(string.Format("Can't add {0} to its own update list", TypeManager.GetSystemName(base.GetType())));
				}
				if (this.m_managedSystemsToUpdate.IndexOf(sys) >= 0)
				{
					if (this.m_managedSystemsToRemove.Contains(sys))
					{
						this.m_managedSystemsToRemove.Remove(sys);
					}
					return;
				}
				UpdateIndex updateIndex = new UpdateIndex(this.m_managedSystemsToUpdate.Count, true);
				this.m_MasterUpdateList.Add(in updateIndex);
				this.m_managedSystemsToUpdate.Add(sys);
				this.m_systemSortDirty = true;
			}
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00008338 File Offset: 0x00006538
		private unsafe int UnmanagedSystemIndex(SystemHandle sysHandle)
		{
			int length = this.m_UnmanagedSystemsToUpdate.Length;
			SystemHandle* ptr = this.m_UnmanagedSystemsToUpdate.Ptr;
			for (int i = 0; i < length; i++)
			{
				if (ptr[i] == sysHandle)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00008384 File Offset: 0x00006584
		public unsafe void AddSystemToUpdateList(SystemHandle sysHandle)
		{
			this.CheckCreated();
			if (sysHandle == default(SystemHandle))
			{
				return;
			}
			SystemState* ptr = base.World.Unmanaged.ResolveSystemState(sysHandle);
			if (ptr->m_ManagedSystem.IsAllocated)
			{
				ComponentSystemBase componentSystemBase = (ComponentSystemBase)ptr->m_ManagedSystem.Target;
				this.AddSystemToUpdateList(componentSystemBase);
			}
			else
			{
				if (-1 != this.UnmanagedSystemIndex(sysHandle))
				{
					int num = this.m_UnmanagedSystemsToRemove.IndexOf(sysHandle);
					if (-1 != num)
					{
						this.m_UnmanagedSystemsToRemove.RemoveAt(num);
					}
					return;
				}
				UpdateIndex updateIndex = new UpdateIndex(this.m_UnmanagedSystemsToUpdate.Length, false);
				this.m_MasterUpdateList.Add(in updateIndex);
				this.m_UnmanagedSystemsToUpdate.Add(in sysHandle);
			}
			this.m_systemSortDirty = true;
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00008444 File Offset: 0x00006644
		public void RemoveSystemFromUpdateList(ComponentSystemBase sys)
		{
			this.CheckCreated();
			if (this.m_managedSystemsToUpdate.Contains(sys) && !this.m_managedSystemsToRemove.Contains(sys))
			{
				this.m_systemSortDirty = true;
				this.m_managedSystemsToRemove.Add(sys);
			}
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x0000847B File Offset: 0x0000667B
		public void RemoveSystemFromUpdateList(SystemHandle sys)
		{
			this.CheckCreated();
			if (this.m_UnmanagedSystemsToUpdate.Contains(sys) && !this.m_UnmanagedSystemsToRemove.Contains(sys))
			{
				this.m_systemSortDirty = true;
				this.m_UnmanagedSystemsToRemove.Add(in sys);
			}
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x000084B4 File Offset: 0x000066B4
		private void RemovePending()
		{
			if (this.m_managedSystemsToRemove.Count > 0)
			{
				foreach (ComponentSystemBase componentSystemBase in this.m_managedSystemsToRemove)
				{
					this.m_managedSystemsToUpdate.Remove(componentSystemBase);
				}
				this.m_managedSystemsToRemove.Clear();
			}
			for (int i = 0; i < this.m_UnmanagedSystemsToRemove.Length; i++)
			{
				SystemHandle systemHandle = this.m_UnmanagedSystemsToRemove[i];
				this.m_UnmanagedSystemsToUpdate.RemoveAt(this.m_UnmanagedSystemsToUpdate.IndexOf(systemHandle));
			}
			this.m_UnmanagedSystemsToRemove.Clear();
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x0000856C File Offset: 0x0000676C
		private unsafe void RemoveSystemsFromUnsortedUpdateList()
		{
			if (this.m_managedSystemsToRemove.Count <= 0 && this.m_UnmanagedSystemsToRemove.Length <= 0)
			{
				return;
			}
			WorldUnmanaged unmanaged = base.World.Unmanaged;
			int num = 0;
			foreach (ComponentSystemBase componentSystemBase in this.m_managedSystemsToUpdate)
			{
				num = math.max(num, componentSystemBase.CheckedState()->m_SystemID);
			}
			foreach (SystemHandle systemHandle in this.m_UnmanagedSystemsToUpdate)
			{
				num = math.max(num, unmanaged.ResolveSystemState(systemHandle)->m_SystemID);
			}
			NativeArray<int> nativeArray = new NativeArray<int>(num + 1, Allocator.Temp, NativeArrayOptions.ClearMemory);
			NativeArray<byte> nativeArray2 = new NativeArray<byte>(num + 1, Allocator.Temp, NativeArrayOptions.ClearMemory);
			foreach (ComponentSystemBase componentSystemBase2 in this.m_managedSystemsToRemove)
			{
				nativeArray2[componentSystemBase2.CheckedState()->m_SystemID] = 1;
			}
			foreach (SystemHandle systemHandle2 in this.m_UnmanagedSystemsToRemove)
			{
				nativeArray2[unmanaged.ResolveSystemState(systemHandle2)->m_SystemID] = 1;
			}
			List<ComponentSystemBase> list = new List<ComponentSystemBase>(this.m_managedSystemsToUpdate.Count);
			UnsafeList<SystemHandle> unsafeList = new UnsafeList<SystemHandle>(this.m_UnmanagedSystemsToUpdate.Length, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			foreach (ComponentSystemBase componentSystemBase3 in this.m_managedSystemsToUpdate)
			{
				int systemID = componentSystemBase3.CheckedState()->m_SystemID;
				if (nativeArray2[systemID] == 0)
				{
					nativeArray[systemID] = list.Count;
					list.Add(componentSystemBase3);
				}
			}
			foreach (SystemHandle systemHandle3 in this.m_UnmanagedSystemsToUpdate)
			{
				int systemID2 = unmanaged.ResolveSystemState(systemHandle3)->m_SystemID;
				if (nativeArray2[systemID2] == 0)
				{
					nativeArray[systemID2] = unsafeList.Length;
					unsafeList.Add(in systemHandle3);
				}
			}
			UnsafeList<UpdateIndex> unsafeList2 = new UnsafeList<UpdateIndex>(list.Count + unsafeList.Length, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			foreach (UpdateIndex updateIndex in this.m_MasterUpdateList)
			{
				if (updateIndex.IsManaged)
				{
					int systemID3 = this.m_managedSystemsToUpdate[updateIndex.Index].CheckedState()->m_SystemID;
					if (nativeArray2[systemID3] == 0)
					{
						UpdateIndex updateIndex2 = new UpdateIndex(nativeArray[systemID3], true);
						unsafeList2.Add(in updateIndex2);
					}
				}
				else
				{
					SystemHandle systemHandle4 = this.m_UnmanagedSystemsToUpdate[updateIndex.Index];
					int systemID4 = unmanaged.ResolveSystemState(systemHandle4)->m_SystemID;
					if (nativeArray2[systemID4] == 0)
					{
						UpdateIndex updateIndex2 = new UpdateIndex(nativeArray[systemID4], false);
						unsafeList2.Add(in updateIndex2);
					}
				}
			}
			nativeArray.Dispose();
			nativeArray2.Dispose();
			this.m_managedSystemsToUpdate = list;
			this.m_managedSystemsToRemove.Clear();
			this.m_UnmanagedSystemsToUpdate.Dispose();
			this.m_UnmanagedSystemsToUpdate = unsafeList;
			this.m_UnmanagedSystemsToRemove.Clear();
			this.m_MasterUpdateList.Dispose();
			this.m_MasterUpdateList = unsafeList2;
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0000895C File Offset: 0x00006B5C
		private void RecurseUpdate()
		{
			if (!this.EnableSystemSorting)
			{
				this.RemoveSystemsFromUnsortedUpdateList();
			}
			else if (this.m_systemSortDirty)
			{
				this.GenerateMasterUpdateList();
			}
			this.m_systemSortDirty = false;
			foreach (ComponentSystemBase componentSystemBase in this.m_managedSystemsToUpdate)
			{
				if (TypeManager.IsSystemAGroup(componentSystemBase.GetType()))
				{
					(componentSystemBase as ComponentSystemGroup).RecurseUpdate();
				}
			}
		}

		// Token: 0x060001BA RID: 442 RVA: 0x000089E8 File Offset: 0x00006BE8
		private unsafe void GenerateMasterUpdateList()
		{
			this.RemovePending();
			SystemTypeIndex systemTypeIndex = this.m_StatePtr->m_SystemTypeIndex;
			int num = this.m_managedSystemsToUpdate.Count + this.m_UnmanagedSystemsToUpdate.Length;
			UnsafeList<ComponentSystemSorter.SystemElement> unsafeList = new UnsafeList<ComponentSystemSorter.SystemElement>(num, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			unsafeList.Length = num;
			int[] array = new int[3];
			for (int i = 0; i < this.m_managedSystemsToUpdate.Count; i++)
			{
				SystemTypeIndex systemTypeIndex2 = this.m_managedSystemsToUpdate[i].m_StatePtr->m_SystemTypeIndex;
				int num2 = ComponentSystemGroup.ComputeSystemOrdering(systemTypeIndex2, systemTypeIndex);
				unsafeList[i] = new ComponentSystemSorter.SystemElement
				{
					SystemTypeIndex = systemTypeIndex2,
					Index = new UpdateIndex(i, true),
					OrderingBucket = num2,
					updateBefore = new NativeList<int>(16, Allocator.Temp),
					nAfter = 0
				};
				array[num2]++;
			}
			for (int j = 0; j < this.m_UnmanagedSystemsToUpdate.Length; j++)
			{
				SystemTypeIndex systemTypeIndex3 = base.World.Unmanaged.ResolveSystemState(this.m_UnmanagedSystemsToUpdate[j])->m_SystemTypeIndex;
				int num3 = ComponentSystemGroup.ComputeSystemOrdering(systemTypeIndex3, systemTypeIndex);
				unsafeList[this.m_managedSystemsToUpdate.Count + j] = new ComponentSystemSorter.SystemElement
				{
					SystemTypeIndex = systemTypeIndex3,
					Index = new UpdateIndex(j, false),
					OrderingBucket = num3,
					updateBefore = new NativeList<int>(16, Allocator.Temp),
					nAfter = 0
				};
				array[num3]++;
			}
			NativeHashMap<int, int> nativeHashMap = new NativeHashMap<int, int>(16, Allocator.Temp);
			NativeHashMap<SystemTypeIndex, int>* ptr = (NativeHashMap<SystemTypeIndex, int>*)UnsafeUtility.AddressOf<NativeHashMap<int, int>>(ref nativeHashMap);
			NativeHashSet<SystemTypeIndex> nativeHashSet = new NativeHashSet<SystemTypeIndex>(16, Allocator.Temp);
			NativeHashSet<SystemTypeIndex>* ptr2 = (NativeHashSet<SystemTypeIndex>*)UnsafeUtility.AddressOf<NativeHashSet<SystemTypeIndex>>(ref nativeHashSet);
			ComponentSystemSorter.FindConstraints(systemTypeIndex, (UnsafeList<ComponentSystemSorter.SystemElement>*)UnsafeUtility.AddressOf<UnsafeList<ComponentSystemSorter.SystemElement>>(ref unsafeList), ptr, TypeManager.SystemAttributeKind.UpdateAfter, TypeManager.SystemAttributeKind.UpdateBefore, ptr2);
			if (nativeHashSet.Count > 0)
			{
				NativeArray<SystemTypeIndex> nativeArray = nativeHashSet.ToNativeArray(Allocator.Temp);
				for (int k = 0; k < nativeArray.Length; k++)
				{
					ComponentSystemSorter.WarnAboutAnySystemAttributeBadness(nativeArray[k], this);
				}
				nativeArray.Dispose();
			}
			nativeHashSet.Clear();
			UnsafeList<ComponentSystemSorter.SystemElement>[] array2 = new UnsafeList<ComponentSystemSorter.SystemElement>[]
			{
				new UnsafeList<ComponentSystemSorter.SystemElement>(array[0], Allocator.Temp, NativeArrayOptions.UninitializedMemory),
				new UnsafeList<ComponentSystemSorter.SystemElement>(array[1], Allocator.Temp, NativeArrayOptions.UninitializedMemory),
				new UnsafeList<ComponentSystemSorter.SystemElement>(array[2], Allocator.Temp, NativeArrayOptions.UninitializedMemory)
			};
			array2[0].Length = array[0];
			array2[1].Length = array[1];
			array2[2].Length = array[2];
			int[] array3 = new int[3];
			for (int l = 0; l < unsafeList.Length; l++)
			{
				int orderingBucket = unsafeList[l].OrderingBucket;
				int[] array4 = array3;
				int num4 = orderingBucket;
				int num5 = array4[num4];
				array4[num4] = num5 + 1;
				int num6 = num5;
				array2[orderingBucket][num6] = unsafeList[l];
			}
			for (int m = 0; m < 3; m++)
			{
				if (array2[m].Length > 0)
				{
					ComponentSystemSorter.Sort((UnsafeList<ComponentSystemSorter.SystemElement>*)UnsafeUtility.AddressOf<UnsafeList<ComponentSystemSorter.SystemElement>>(ref array2[m]), ptr);
				}
			}
			List<ComponentSystemBase> managedSystemsToUpdate = this.m_managedSystemsToUpdate;
			this.m_managedSystemsToUpdate = new List<ComponentSystemBase>(managedSystemsToUpdate.Count);
			for (int n = 0; n < 3; n++)
			{
				foreach (ComponentSystemSorter.SystemElement systemElement in array2[n])
				{
					UpdateIndex index = systemElement.Index;
					if (index.IsManaged)
					{
						this.m_managedSystemsToUpdate.Add(managedSystemsToUpdate[index.Index]);
					}
				}
			}
			this.m_MasterUpdateList.Clear();
			this.m_MasterUpdateList.SetCapacity(unsafeList.Length);
			int num7 = 0;
			for (int num8 = 0; num8 < 3; num8++)
			{
				foreach (ComponentSystemSorter.SystemElement systemElement2 in array2[num8])
				{
					UpdateIndex updateIndex = systemElement2.Index;
					if (updateIndex.IsManaged)
					{
						updateIndex = new UpdateIndex(num7++, true);
						this.m_MasterUpdateList.Add(in updateIndex);
					}
					else
					{
						this.m_MasterUpdateList.Add(in systemElement2.Index);
					}
				}
			}
		}

		// Token: 0x060001BB RID: 443 RVA: 0x00008E94 File Offset: 0x00007094
		internal static int ComputeSystemOrdering(SystemTypeIndex sysType, SystemTypeIndex ourTypeIndex)
		{
			if (ourTypeIndex == -1 || sysType == -1)
			{
				return 1;
			}
			NativeList<TypeManager.SystemAttribute> systemAttributes = TypeManager.GetSystemAttributes(sysType, TypeManager.SystemAttributeKind.UpdateInGroup, Allocator.Temp);
			for (int i = 0; i < systemAttributes.Length; i++)
			{
				TypeManager.SystemAttribute systemAttribute = systemAttributes[i];
				if (systemAttribute.TargetSystemTypeIndex == ourTypeIndex)
				{
					if ((systemAttribute.Flags & 1) != 0)
					{
						return 0;
					}
					if ((systemAttribute.Flags & 2) != 0)
					{
						return 2;
					}
				}
			}
			return 1;
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00008F0A File Offset: 0x0000710A
		public void SortSystems()
		{
			this.CheckCreated();
			this.RecurseUpdate();
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00008F18 File Offset: 0x00007118
		internal unsafe override void OnStopRunningInternal()
		{
			this.OnStopRunning();
			foreach (ComponentSystemBase componentSystemBase in this.m_managedSystemsToUpdate)
			{
				if (componentSystemBase != null && componentSystemBase.m_StatePtr != null && componentSystemBase.m_StatePtr->PreviouslyEnabled)
				{
					componentSystemBase.m_StatePtr->PreviouslyEnabled = false;
					componentSystemBase.OnStopRunningInternal();
				}
			}
			for (int i = 0; i < this.m_UnmanagedSystemsToUpdate.Length; i++)
			{
				SystemState* ptr = base.World.Unmanaged.ResolveSystemState(this.m_UnmanagedSystemsToUpdate[i]);
				if (ptr != null && ptr->PreviouslyEnabled)
				{
					ptr->PreviouslyEnabled = false;
					SystemBaseRegistry.CallOnStopRunning(ptr);
				}
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060001BE RID: 446 RVA: 0x00008FE8 File Offset: 0x000071E8
		// (set) Token: 0x060001BF RID: 447 RVA: 0x00008FF0 File Offset: 0x000071F0
		[Obsolete("This property has been renamed to RateManager. (RemovedAfter Entities 1.0) (UnityUpgradable) -> RateManager")]
		public IRateManager FixedRateManager
		{
			get
			{
				return this.m_RateManager;
			}
			set
			{
				this.m_RateManager = value;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x00008FE8 File Offset: 0x000071E8
		// (set) Token: 0x060001C1 RID: 449 RVA: 0x00008FF0 File Offset: 0x000071F0
		public IRateManager RateManager
		{
			get
			{
				return this.m_RateManager;
			}
			set
			{
				this.m_RateManager = value;
			}
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00008FF9 File Offset: 0x000071F9
		public void SetRateManagerCreateAllocator(IRateManager rateManager)
		{
			this.CreateRateGroupAllocators();
			this.m_RateManager = rateManager;
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x00009008 File Offset: 0x00007208
		public unsafe DoubleRewindableAllocators* RateGroupAllocators
		{
			get
			{
				return this.m_RateGroupAllocators;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x00009010 File Offset: 0x00007210
		// (set) Token: 0x060001C5 RID: 453 RVA: 0x00009018 File Offset: 0x00007218
		internal unsafe DoubleRewindableAllocators* CurrentGroupAllocators { get; set; } = null;

		// Token: 0x060001C6 RID: 454 RVA: 0x00009021 File Offset: 0x00007221
		[Preserve]
		protected override void OnUpdate()
		{
			this.CheckCreated();
			if (this.RateManager == null)
			{
				this.UpdateAllSystems();
				return;
			}
			while (this.RateManager.ShouldGroupUpdate(this))
			{
				this.UpdateAllSystems();
			}
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0000904C File Offset: 0x0000724C
		private void UpdateAllSystems()
		{
			if (this.m_systemSortDirty)
			{
				this.SortSystems();
			}
			ref WorldUnmanagedImpl impl = ref base.World.Unmanaged.GetImpl();
			int length = this.m_MasterUpdateList.Length;
			for (int i = 0; i < length; i++)
			{
				try
				{
					UpdateIndex updateIndex = this.m_MasterUpdateList[i];
					if (!updateIndex.IsManaged)
					{
						SystemHandle systemHandle = this.m_UnmanagedSystemsToUpdate[updateIndex.Index];
						impl.UpdateSystem(systemHandle);
					}
					else
					{
						this.m_managedSystemsToUpdate[updateIndex.Index].Update();
					}
				}
				catch (Exception ex)
				{
					Debug.LogException(ex);
				}
				if (base.World.QuitUpdate)
				{
					break;
				}
			}
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x00009108 File Offset: 0x00007308
		internal unsafe void CreateRateGroupAllocators()
		{
			if (this.RateGroupAllocatorsCreated == 0)
			{
				this.m_RateGroupAllocators = Memory.Unmanaged.Allocate<DoubleRewindableAllocators>(Allocator.Persistent);
				this.m_RateGroupAllocators->Initialize(Allocator.Persistent, 131072);
				this.RateGroupAllocatorsCreated = 1;
			}
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00009140 File Offset: 0x00007340
		private unsafe void DestroyRateGroupAllocators()
		{
			this.m_RateGroupAllocators->Dispose();
			Memory.Unmanaged.Free<DoubleRewindableAllocators>(this.m_RateGroupAllocators, Allocator.Persistent);
			this.m_RateGroupAllocators = null;
			this.RateGroupAllocatorsCreated = 0;
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000916D File Offset: 0x0000736D
		[Preserve]
		protected ComponentSystemGroup()
		{
		}

		// Token: 0x04000066 RID: 102
		private bool m_systemSortDirty;

		// Token: 0x04000067 RID: 103
		private const int InitialSystemGroupAllocatorBlockSizeInBytes = 131072;

		// Token: 0x04000068 RID: 104
		private bool m_EnableSystemSorting = true;

		// Token: 0x0400006A RID: 106
		internal List<ComponentSystemBase> m_managedSystemsToUpdate = new List<ComponentSystemBase>();

		// Token: 0x0400006B RID: 107
		internal List<ComponentSystemBase> m_managedSystemsToRemove = new List<ComponentSystemBase>();

		// Token: 0x0400006C RID: 108
		internal UnsafeList<UpdateIndex> m_MasterUpdateList;

		// Token: 0x0400006D RID: 109
		internal UnsafeList<SystemHandle> m_UnmanagedSystemsToUpdate;

		// Token: 0x0400006E RID: 110
		internal UnsafeList<SystemHandle> m_UnmanagedSystemsToRemove;

		// Token: 0x0400006F RID: 111
		internal unsafe DoubleRewindableAllocators* m_RateGroupAllocators = null;

		// Token: 0x04000071 RID: 113
		private IRateManager m_RateManager;
	}
}
