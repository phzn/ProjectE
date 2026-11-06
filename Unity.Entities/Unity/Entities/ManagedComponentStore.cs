using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x02000241 RID: 577
	internal class ManagedComponentStore
	{
		// Token: 0x06000EE1 RID: 3809 RVA: 0x00042DEA File Offset: 0x00040FEA
		public void SetManagedComponentCapacity(int newCapacity)
		{
			Array.Resize<object>(ref this.m_ManagedComponentData, newCapacity);
		}

		// Token: 0x06000EE2 RID: 3810 RVA: 0x00042DF8 File Offset: 0x00040FF8
		public object GetManagedComponent(int index)
		{
			return this.m_ManagedComponentData[index];
		}

		// Token: 0x06000EE3 RID: 3811 RVA: 0x00042E02 File Offset: 0x00041002
		public object Debugger_GetManagedComponent(int index)
		{
			if (index < 0 || index >= this.m_ManagedComponentData.Length)
			{
				return null;
			}
			return this.m_ManagedComponentData[index];
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000EE4 RID: 3812 RVA: 0x00042E1D File Offset: 0x0004101D
		private unsafe ManagedComponentStore.SharedComponentInfo* SharedComponentInfoPtr
		{
			get
			{
				return this.m_SharedComponentInfo.Ptr;
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000EE5 RID: 3813 RVA: 0x00042E2A File Offset: 0x0004102A
		// (set) Token: 0x06000EE6 RID: 3814 RVA: 0x00042E3B File Offset: 0x0004103B
		public unsafe static TypeIndex CompanionReferenceTypeIndex
		{
			get
			{
				return *ManagedComponentStore.CompanionReferenceTypeIndexStatic.Ref.Data;
			}
			set
			{
				*ManagedComponentStore.CompanionReferenceTypeIndexStatic.Ref.Data = value;
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000EE7 RID: 3815 RVA: 0x00042E4D File Offset: 0x0004104D
		// (set) Token: 0x06000EE8 RID: 3816 RVA: 0x00042E5E File Offset: 0x0004105E
		public unsafe static TypeIndex CompanionLinkTypeIndex
		{
			get
			{
				return *ManagedComponentStore.CompanionLinkTypeIndexStatic.Ref.Data;
			}
			set
			{
				*ManagedComponentStore.CompanionLinkTypeIndexStatic.Ref.Data = value;
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000EE9 RID: 3817 RVA: 0x00042E70 File Offset: 0x00041070
		// (set) Token: 0x06000EEA RID: 3818 RVA: 0x00042E81 File Offset: 0x00041081
		public unsafe static TypeIndex CompanionLinkTransformTypeIndex
		{
			get
			{
				return *ManagedComponentStore.CompanionLinkTransformTypeIndexStatic.Ref.Data;
			}
			set
			{
				*ManagedComponentStore.CompanionLinkTransformTypeIndexStatic.Ref.Data = value;
			}
		}

		// Token: 0x06000EEB RID: 3819 RVA: 0x00042E94 File Offset: 0x00041094
		public unsafe ManagedComponentStore(EntityComponentStore* entityComponentStore)
		{
			this.m_EntityComponentStore = entityComponentStore;
			this.ResetSharedComponentData();
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x00042F0C File Offset: 0x0004110C
		public void Dispose()
		{
			for (int num = 1; num != this.m_SharedComponentData.Count; num++)
			{
				IRefCounted refCounted = this.m_SharedComponentData[num] as IRefCounted;
				if (refCounted != null)
				{
					refCounted.Release();
				}
			}
			for (int num2 = 0; num2 != this.m_ManagedComponentData.Length; num2++)
			{
				ManagedComponentStore.DisposeManagedComponentData(this.m_ManagedComponentData[num2]);
			}
			this.m_SharedComponentInfo.Dispose();
			this.m_SharedComponentData.Clear();
			this.m_SharedComponentData = null;
			this.m_HashLookup.Dispose();
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x00042F94 File Offset: 0x00041194
		private void ResetSharedComponentData()
		{
			this.m_HashLookup.Clear();
			this.m_SharedComponentData.Clear();
			this.m_SharedComponentInfo.Clear();
			this.m_SharedComponentData.Add(null);
			ManagedComponentStore.SharedComponentInfo sharedComponentInfo = default(ManagedComponentStore.SharedComponentInfo);
			sharedComponentInfo.RefCount = 1;
			sharedComponentInfo.ComponentType = TypeIndex.Null;
			sharedComponentInfo.Version = 1;
			sharedComponentInfo.HashCode = 0;
			this.m_SharedComponentInfo.Add(in sharedComponentInfo);
			this.m_FreeListIndex = -1;
		}

		// Token: 0x06000EEE RID: 3822 RVA: 0x00043010 File Offset: 0x00041210
		public unsafe void GetAllUniqueSharedComponents_Managed<T>(List<T> sharedComponentValues) where T : struct, ISharedComponentData
		{
			sharedComponentValues.Add(default(T));
			int length = this.m_SharedComponentInfo.Length;
			ManagedComponentStore.SharedComponentInfo* sharedComponentInfoPtr = this.SharedComponentInfoPtr;
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			for (int num = 1; num != length; num++)
			{
				if (sharedComponentInfoPtr[num].ComponentType == typeIndex)
				{
					object obj = this.m_SharedComponentData[num];
					if (obj != null)
					{
						sharedComponentValues.Add((T)((object)obj));
					}
				}
			}
		}

		// Token: 0x06000EEF RID: 3823 RVA: 0x00043090 File Offset: 0x00041290
		public unsafe void GetAllUniqueSharedComponents_Managed<T>(List<T> sharedComponentValues, List<int> sharedComponentIndices) where T : struct, ISharedComponentData
		{
			sharedComponentValues.Add(default(T));
			sharedComponentIndices.Add(0);
			int length = this.m_SharedComponentInfo.Length;
			ManagedComponentStore.SharedComponentInfo* sharedComponentInfoPtr = this.SharedComponentInfoPtr;
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			for (int num = 1; num != length; num++)
			{
				if (sharedComponentInfoPtr[num].ComponentType == typeIndex)
				{
					object obj = this.m_SharedComponentData[num];
					if (obj != null)
					{
						sharedComponentValues.Add((T)((object)obj));
						sharedComponentIndices.Add(num);
					}
				}
			}
		}

		// Token: 0x06000EF0 RID: 3824 RVA: 0x00043120 File Offset: 0x00041320
		public unsafe void GetAllUniqueSharedComponents_Managed<T>(List<T> sharedComponentValues, List<int> sharedComponentIndices, List<int> sharedComponentVersions) where T : struct, ISharedComponentData
		{
			sharedComponentValues.Add(default(T));
			sharedComponentIndices.Add(0);
			sharedComponentVersions.Add(0);
			int length = this.m_SharedComponentInfo.Length;
			ManagedComponentStore.SharedComponentInfo* sharedComponentInfoPtr = this.SharedComponentInfoPtr;
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			for (int num = 1; num != length; num++)
			{
				if (sharedComponentInfoPtr[num].ComponentType == typeIndex)
				{
					object obj = this.m_SharedComponentData[num];
					if (obj != null)
					{
						sharedComponentValues.Add((T)((object)obj));
						sharedComponentIndices.Add(num);
						sharedComponentVersions.Add(sharedComponentInfoPtr[num].Version);
					}
				}
			}
		}

		// Token: 0x06000EF1 RID: 3825 RVA: 0x000431CB File Offset: 0x000413CB
		public int GetSharedComponentCount()
		{
			return this.m_SharedComponentData.Count;
		}

		// Token: 0x06000EF2 RID: 3826 RVA: 0x000431D8 File Offset: 0x000413D8
		public unsafe int InsertSharedComponent_Managed<T>(T newData) where T : struct
		{
			TypeIndex typeIndex = TypeManager.GetTypeIndex<T>();
			int num = this.FindSharedComponentIndex<T>(TypeManager.GetTypeIndex<T>(), newData);
			if (num == 0)
			{
				return 0;
			}
			if (num != -1)
			{
				this.SharedComponentInfoPtr[num].RefCount++;
				return num;
			}
			int hashCode = TypeManager.GetHashCode<T>(ref newData);
			object obj = newData;
			IRefCounted refCounted = obj as IRefCounted;
			if (refCounted != null)
			{
				refCounted.Retain();
			}
			return this.Add(typeIndex, hashCode, obj);
		}

		// Token: 0x06000EF3 RID: 3827 RVA: 0x00043248 File Offset: 0x00041448
		private int FindSharedComponentIndex<T>(TypeIndex typeIndex, T newData) where T : struct
		{
			T t = default(T);
			if (TypeManager.Equals<T>(ref t, ref newData))
			{
				return 0;
			}
			return this.FindNonDefaultSharedComponentIndex<T>(typeIndex, TypeManager.GetHashCode<T>(ref newData), ref newData);
		}

		// Token: 0x06000EF4 RID: 3828 RVA: 0x0004327C File Offset: 0x0004147C
		private unsafe int FindNonDefaultSharedComponentIndex<T>(TypeIndex typeIndex, int hashCode, ref T newData) where T : struct
		{
			int num;
			NativeParallelMultiHashMapIterator<ulong> nativeParallelMultiHashMapIterator;
			if (!this.m_HashLookup.TryGetFirstValue(EntityComponentStore.GetSharedComponentHashKey(typeIndex, hashCode), out num, out nativeParallelMultiHashMapIterator))
			{
				return -1;
			}
			ManagedComponentStore.SharedComponentInfo* sharedComponentInfoPtr = this.SharedComponentInfoPtr;
			for (;;)
			{
				object obj = this.m_SharedComponentData[num];
				if (obj != null && sharedComponentInfoPtr[num].ComponentType == typeIndex)
				{
					T t = (T)((object)obj);
					if (TypeManager.Equals<T>(ref t, ref newData))
					{
						break;
					}
				}
				if (!this.m_HashLookup.TryGetNextValue(out num, ref nativeParallelMultiHashMapIterator))
				{
					return -1;
				}
			}
			return num;
		}

		// Token: 0x06000EF5 RID: 3829 RVA: 0x000432F8 File Offset: 0x000414F8
		private unsafe int FindNonDefaultSharedComponentIndex(TypeIndex typeIndex, int hashCode, object newData)
		{
			int num;
			NativeParallelMultiHashMapIterator<ulong> nativeParallelMultiHashMapIterator;
			if (!this.m_HashLookup.TryGetFirstValue(EntityComponentStore.GetSharedComponentHashKey(typeIndex, hashCode), out num, out nativeParallelMultiHashMapIterator))
			{
				return -1;
			}
			ManagedComponentStore.SharedComponentInfo* sharedComponentInfoPtr = this.SharedComponentInfoPtr;
			for (;;)
			{
				object obj = this.m_SharedComponentData[num];
				if (obj != null && sharedComponentInfoPtr[num].ComponentType == typeIndex && TypeManager.Equals(obj, newData, typeIndex))
				{
					break;
				}
				if (!this.m_HashLookup.TryGetNextValue(out num, ref nativeParallelMultiHashMapIterator))
				{
					return -1;
				}
			}
			return num;
		}

		// Token: 0x06000EF6 RID: 3830 RVA: 0x0004336C File Offset: 0x0004156C
		internal unsafe int CloneSharedComponentNonDefault(ManagedComponentStore srcManagedComponents, int srcSharedComponentIndex)
		{
			ManagedComponentStore.SharedComponentInfo* sharedComponentInfoPtr = srcManagedComponents.SharedComponentInfoPtr;
			object obj = srcManagedComponents.m_SharedComponentData[srcSharedComponentIndex];
			TypeIndex componentType = sharedComponentInfoPtr[srcSharedComponentIndex].ComponentType;
			int hashCode = sharedComponentInfoPtr[srcSharedComponentIndex].HashCode;
			return this.InsertSharedComponentAssumeNonDefault(componentType, hashCode, obj);
		}

		// Token: 0x06000EF7 RID: 3831 RVA: 0x000433B8 File Offset: 0x000415B8
		internal unsafe int InsertSharedComponentAssumeNonDefault(TypeIndex typeIndex, int hashCode, object newData)
		{
			int num = this.FindNonDefaultSharedComponentIndex(typeIndex, hashCode, newData);
			if (-1 == num)
			{
				IRefCounted refCounted = newData as IRefCounted;
				if (refCounted != null)
				{
					refCounted.Retain();
				}
				num = this.Add(typeIndex, hashCode, newData);
			}
			else
			{
				this.SharedComponentInfoPtr[num].RefCount++;
			}
			return num;
		}

		// Token: 0x06000EF8 RID: 3832 RVA: 0x0004340C File Offset: 0x0004160C
		internal unsafe int InsertSharedComponentAssumeNonDefaultMove(TypeIndex typeIndex, int hashCode, object newData)
		{
			int num = this.FindNonDefaultSharedComponentIndex(typeIndex, hashCode, newData);
			if (-1 == num)
			{
				num = this.Add(typeIndex, hashCode, newData);
			}
			else
			{
				this.SharedComponentInfoPtr[num].RefCount++;
			}
			return num;
		}

		// Token: 0x06000EF9 RID: 3833 RVA: 0x00043450 File Offset: 0x00041650
		private unsafe int Add(TypeIndex typeIndex, int hashCode, object newData)
		{
			this.m_EntityComponentStore->m_SharedComponentVersion++;
			ManagedComponentStore.SharedComponentInfo sharedComponentInfo = new ManagedComponentStore.SharedComponentInfo
			{
				RefCount = 1,
				Version = this.m_EntityComponentStore->m_SharedComponentVersion,
				ComponentType = typeIndex,
				HashCode = hashCode
			};
			if (this.m_FreeListIndex != -1)
			{
				ManagedComponentStore.SharedComponentInfo* sharedComponentInfoPtr = this.SharedComponentInfoPtr;
				int freeListIndex = this.m_FreeListIndex;
				this.m_FreeListIndex = sharedComponentInfoPtr[freeListIndex].Version;
				this.m_HashLookup.Add(EntityComponentStore.GetSharedComponentHashKey(typeIndex, hashCode), freeListIndex);
				this.m_SharedComponentData[freeListIndex] = newData;
				sharedComponentInfoPtr[freeListIndex] = sharedComponentInfo;
				return freeListIndex;
			}
			int count = this.m_SharedComponentData.Count;
			this.m_HashLookup.Add(EntityComponentStore.GetSharedComponentHashKey(typeIndex, hashCode), count);
			this.m_SharedComponentData.Add(newData);
			this.m_SharedComponentInfo.Add(in sharedComponentInfo);
			return count;
		}

		// Token: 0x06000EFA RID: 3834 RVA: 0x00043540 File Offset: 0x00041740
		public unsafe void IncrementSharedComponentVersion_Managed(int index)
		{
			EntityComponentStore* entityComponentStore = this.m_EntityComponentStore;
			int num = entityComponentStore->m_SharedComponentVersion + 1;
			entityComponentStore->m_SharedComponentVersion = num;
			int num2 = num;
			if (index == 0)
			{
				this.m_EntityComponentStore->m_SharedComponentGlobalVersion = num2;
				return;
			}
			this.SharedComponentInfoPtr[index].Version = num2;
		}

		// Token: 0x06000EFB RID: 3835 RVA: 0x00043588 File Offset: 0x00041788
		public unsafe int GetSharedComponentVersion_Managed<T>(T sharedData) where T : struct
		{
			int num = this.FindSharedComponentIndex<T>(TypeManager.GetTypeIndex<T>(), sharedData);
			if (num <= 0)
			{
				return this.m_EntityComponentStore->m_SharedComponentGlobalVersion;
			}
			return this.SharedComponentInfoPtr[num].Version;
		}

		// Token: 0x06000EFC RID: 3836 RVA: 0x000435C8 File Offset: 0x000417C8
		public T GetSharedComponentData_Managed<T>(int index) where T : struct
		{
			if (index == 0)
			{
				return default(T);
			}
			return (T)((object)this.m_SharedComponentData[index]);
		}

		// Token: 0x06000EFD RID: 3837 RVA: 0x000435F3 File Offset: 0x000417F3
		public object GetSharedComponentDataBoxed(int index, TypeIndex typeIndex)
		{
			if (index == 0)
			{
				return Activator.CreateInstance(TypeManager.GetType(typeIndex));
			}
			return this.m_SharedComponentData[index];
		}

		// Token: 0x06000EFE RID: 3838 RVA: 0x00043610 File Offset: 0x00041810
		public object GetSharedComponentDataNonDefaultBoxed(int index)
		{
			return this.m_SharedComponentData[index];
		}

		// Token: 0x06000EFF RID: 3839 RVA: 0x0004361E File Offset: 0x0004181E
		public unsafe void AddSharedComponentReference_Managed(int index, int numRefs = 1)
		{
			if (index == 0)
			{
				return;
			}
			this.SharedComponentInfoPtr[index].RefCount += numRefs;
		}

		// Token: 0x06000F00 RID: 3840 RVA: 0x00043640 File Offset: 0x00041840
		public unsafe void RemoveSharedComponentReference_Managed(int index, int numRefs = 1)
		{
			if (index == 0)
			{
				return;
			}
			ManagedComponentStore.SharedComponentInfo* sharedComponentInfoPtr = this.SharedComponentInfoPtr;
			if ((sharedComponentInfoPtr[index].RefCount -= numRefs) != 0)
			{
				return;
			}
			this.IncrementSharedComponentVersion_Managed(0);
			int hashCode = sharedComponentInfoPtr[index].HashCode;
			IRefCounted refCounted = this.m_SharedComponentData[index] as IRefCounted;
			if (refCounted != null)
			{
				refCounted.Release();
			}
			this.m_SharedComponentData[index] = null;
			TypeIndex componentType = sharedComponentInfoPtr[index].ComponentType;
			sharedComponentInfoPtr[index].ComponentType = TypeIndex.Null;
			sharedComponentInfoPtr[index].Version = this.m_FreeListIndex;
			this.m_FreeListIndex = index;
			int num;
			NativeParallelMultiHashMapIterator<ulong> nativeParallelMultiHashMapIterator;
			if (this.m_HashLookup.TryGetFirstValue(EntityComponentStore.GetSharedComponentHashKey(componentType, hashCode), out num, out nativeParallelMultiHashMapIterator))
			{
				while (num != index)
				{
					if (!this.m_HashLookup.TryGetNextValue(out num, ref nativeParallelMultiHashMapIterator))
					{
						return;
					}
				}
				this.m_HashLookup.Remove(nativeParallelMultiHashMapIterator);
				return;
			}
		}

		// Token: 0x06000F01 RID: 3841 RVA: 0x00043734 File Offset: 0x00041934
		public unsafe void CheckInternalConsistency()
		{
			ManagedComponentStore.SharedComponentInfo* sharedComponentInfoPtr = this.SharedComponentInfoPtr;
			int num = 0;
			for (int i = 0; i < this.m_SharedComponentData.Count; i++)
			{
				if (this.m_SharedComponentData[i] != null)
				{
					num++;
					int hashCode = sharedComponentInfoPtr[i].HashCode;
					int num2;
					NativeParallelMultiHashMapIterator<ulong> nativeParallelMultiHashMapIterator;
					if (this.m_HashLookup.TryGetFirstValue(EntityComponentStore.GetSharedComponentHashKey(sharedComponentInfoPtr[i].ComponentType, hashCode), out num2, out nativeParallelMultiHashMapIterator))
					{
						while (this.m_HashLookup.TryGetNextValue(out num2, ref nativeParallelMultiHashMapIterator))
						{
						}
					}
				}
			}
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x000437C0 File Offset: 0x000419C0
		public unsafe bool IsEmpty()
		{
			ManagedComponentStore.SharedComponentInfo* sharedComponentInfoPtr = this.SharedComponentInfoPtr;
			for (int i = 1; i < this.m_SharedComponentData.Count; i++)
			{
				if (this.m_SharedComponentData[i] != null)
				{
					return false;
				}
				if (sharedComponentInfoPtr[i].ComponentType != TypeIndex.Null)
				{
					return false;
				}
				if (sharedComponentInfoPtr[i].RefCount != 0)
				{
					return false;
				}
			}
			return this.m_SharedComponentData[0] == null && this.m_HashLookup.Count() == 0;
		}

		// Token: 0x06000F03 RID: 3843 RVA: 0x0004384C File Offset: 0x00041A4C
		public unsafe bool AllSharedComponentReferencesAreFromChunks(UnsafeParallelHashMap<int, int> refCountMap)
		{
			ManagedComponentStore.SharedComponentInfo* sharedComponentInfoPtr = this.SharedComponentInfoPtr;
			for (int i = 1; i < this.m_SharedComponentInfo.Length; i++)
			{
				int num;
				if (refCountMap.TryGetValue(i, out num))
				{
					int refCount = sharedComponentInfoPtr[i].RefCount;
					if (num != refCount)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06000F04 RID: 3844 RVA: 0x0004389C File Offset: 0x00041A9C
		public unsafe void MoveAllSharedComponents_Managed(ManagedComponentStore srcManagedComponents, ref NativeParallelHashMap<int, int> remap, int numSharedComponents)
		{
			remap[0] = 0;
			ManagedComponentStore.SharedComponentInfo* sharedComponentInfoPtr = srcManagedComponents.SharedComponentInfoPtr;
			for (int i = 1; i < numSharedComponents; i++)
			{
				object obj = srcManagedComponents.m_SharedComponentData[i];
				if (obj != null)
				{
					TypeIndex componentType = sharedComponentInfoPtr[i].ComponentType;
					int hashCode = sharedComponentInfoPtr[i].HashCode;
					int num = this.InsertSharedComponentAssumeNonDefaultMove(componentType, hashCode, obj);
					this.SharedComponentInfoPtr[num].RefCount += sharedComponentInfoPtr[i].RefCount - 1;
					this.IncrementSharedComponentVersion_Managed(num);
					remap[i] = num;
				}
			}
			srcManagedComponents.ResetSharedComponentData();
		}

		// Token: 0x06000F05 RID: 3845 RVA: 0x0004394C File Offset: 0x00041B4C
		public unsafe NativeArray<int> MoveSharedComponents_Managed(ManagedComponentStore srcManagedComponents, NativeArray<ArchetypeChunk> chunks, AllocatorManager.AllocatorHandle allocator)
		{
			NativeArray<int> nativeArray = new NativeArray<int>(srcManagedComponents.GetSharedComponentCount(), allocator.ToAllocator, NativeArrayOptions.ClearMemory);
			int* unsafePtr = (int*)nativeArray.GetUnsafePtr<int>();
			for (int i = 0; i < chunks.Length; i++)
			{
				ChunkIndex chunk = chunks[i].m_Chunk;
				Archetype* archetype = chunks[i].Archetype.Archetype;
				SharedComponentValues sharedComponentValues = archetype->Chunks.GetSharedComponentValues(chunk.ListIndex);
				for (int j = 0; j < archetype->NumSharedComponents; j++)
				{
					unsafePtr[*sharedComponentValues[j]]++;
				}
			}
			nativeArray[0] = 0;
			ManagedComponentStore.SharedComponentInfo* sharedComponentInfoPtr = srcManagedComponents.SharedComponentInfoPtr;
			for (int k = 1; k < nativeArray.Length; k++)
			{
				if (unsafePtr[k] != 0)
				{
					object obj = srcManagedComponents.m_SharedComponentData[k];
					TypeIndex componentType = sharedComponentInfoPtr[k].ComponentType;
					int hashCode = sharedComponentInfoPtr[k].HashCode;
					int num = this.InsertSharedComponentAssumeNonDefault(componentType, hashCode, obj);
					int num2 = unsafePtr[k];
					this.SharedComponentInfoPtr[num].RefCount += num2 - 1;
					srcManagedComponents.RemoveSharedComponentReference_Managed(k, num2);
					this.IncrementSharedComponentVersion_Managed(num);
					unsafePtr[k] = num;
				}
			}
			return nativeArray;
		}

		// Token: 0x06000F06 RID: 3846 RVA: 0x00043AB0 File Offset: 0x00041CB0
		public void MoveManagedComponentsFromDifferentWorld(NativeArray<int> srcIndices, NativeArray<int> dstIndices, int count, ManagedComponentStore srcManagedComponentStore)
		{
			for (int i = 0; i < count; i++)
			{
				int num = srcIndices[i];
				int num2 = dstIndices[i];
				this.m_ManagedComponentData[num2] = srcManagedComponentStore.m_ManagedComponentData[num];
				srcManagedComponentStore.m_ManagedComponentData[num] = null;
			}
		}

		// Token: 0x06000F07 RID: 3847 RVA: 0x00043AF6 File Offset: 0x00041CF6
		public void PrepareForDeserialize()
		{
			this.ResetSharedComponentData();
		}

		// Token: 0x06000F08 RID: 3848 RVA: 0x00043B00 File Offset: 0x00041D00
		public unsafe void PatchEntities(Archetype* archetype, ChunkIndex chunk, int entityCount, EntityRemapUtility.EntityRemapInfo* remapping)
		{
			short firstManagedComponent = archetype->FirstManagedComponent;
			int numManagedComponents = archetype->NumManagedComponents;
			for (int i = 0; i < numManagedComponents; i++)
			{
				int num = i + (int)firstManagedComponent;
				if (archetype->Types[num].HasEntityReferences)
				{
					int* componentDataRO = (int*)ChunkDataUtility.GetComponentDataRO(chunk, archetype, 0, num);
					for (int j = 0; j < entityCount; j++)
					{
						if (componentDataRO[j] != 0)
						{
							object obj = this.m_ManagedComponentData[componentDataRO[j]];
							this.m_ManagedObjectRemap.RemapEntityReferences(ref obj, remapping);
						}
					}
				}
			}
			this.m_ManagedObjectRemap.ClearGCRefs();
		}

		// Token: 0x06000F09 RID: 3849 RVA: 0x00043B94 File Offset: 0x00041D94
		private unsafe void PatchEntitiesForPrefab(int* managedComponents, int numManagedComponents, int allocatedCount, int remappingCount, Entity* remapSrc, Entity* remapDst)
		{
			for (int i = 0; i < allocatedCount; i++)
			{
				for (int j = 0; j < numManagedComponents; j++)
				{
					int num = managedComponents[j];
					if (num != 0)
					{
						object obj = this.m_ManagedComponentData[num];
						this.m_ManagedObjectRemap.RemapEntityReferencesForPrefab(ref obj, remapSrc, remapDst, remappingCount);
					}
				}
				managedComponents += numManagedComponents;
				remapDst += remappingCount;
			}
			this.m_ManagedObjectRemap.ClearGCRefs();
		}

		// Token: 0x06000F0A RID: 3850 RVA: 0x00043C04 File Offset: 0x00041E04
		public unsafe void Playback(ref ManagedDeferredCommands managedDeferredCommands)
		{
			UnsafeAppendBuffer.Reader reader = new UnsafeAppendBuffer.Reader(ref managedDeferredCommands.CommandBuffer);
			while (!reader.EndOfBuffer)
			{
				switch (reader.ReadNext<int>())
				{
				case 0:
				{
					int num = reader.ReadNext<int>();
					this.IncrementSharedComponentVersion_Managed(num);
					break;
				}
				case 1:
				{
					Archetype* ptr = (Archetype*)(void*)reader.ReadNext<IntPtr>();
					ChunkIndex chunkIndex = reader.ReadNext<ChunkIndex>();
					int num2 = reader.ReadNext<int>();
					EntityRemapUtility.EntityRemapInfo* ptr2 = (EntityRemapUtility.EntityRemapInfo*)(void*)reader.ReadNext<IntPtr>();
					this.PatchEntities(ptr, chunkIndex, num2, ptr2);
					break;
				}
				case 2:
				{
					byte* ptr3 = (byte*)(void*)reader.ReadNext<IntPtr>();
					int num3 = reader.ReadNext<int>();
					int num4 = reader.ReadNext<int>();
					int num5 = reader.ReadNext<int>();
					Allocator allocator = (Allocator)reader.ReadNext<int>();
					int num6 = UnsafeUtility.SizeOf<Entity>() * num4;
					int num7 = UnsafeUtility.SizeOf<Entity>() * num4 * num3;
					byte* ptr4 = ptr3 + num6;
					byte* ptr5 = ptr4 + num7;
					this.PatchEntitiesForPrefab((int*)ptr5, num5, num3, num4, (Entity*)ptr3, (Entity*)ptr4);
					Memory.Unmanaged.Free<byte>(ptr3, allocator);
					break;
				}
				case 3:
				{
					int num8 = reader.ReadNext<int>();
					int num9 = reader.ReadNext<int>();
					this.AddSharedComponentReference_Managed(num8, num9);
					break;
				}
				case 4:
				{
					int num10 = reader.ReadNext<int>();
					int num11 = reader.ReadNext<int>();
					this.RemoveSharedComponentReference_Managed(num10, num11);
					break;
				}
				case 5:
				{
					int num12;
					int* ptr6 = (int*)reader.ReadNextArray<int>(out num12);
					int num13 = reader.ReadNext<int>();
					int num14;
					int* ptr7 = (int*)reader.ReadNextArray<int>(out num14);
					this.CloneManagedComponents(ptr6, num12, ptr7, num13);
					break;
				}
				case 6:
				{
					int num15;
					int* ptr8 = (int*)reader.ReadNextArray<int>(out num15);
					int num16;
					Entity* ptr9 = (Entity*)reader.ReadNextArray<Entity>(out num16);
					int num14;
					int* ptr10 = (int*)reader.ReadNextArray<int>(out num14);
					int* ptr11 = (int*)reader.ReadNextArray<int>(out num14);
					int* ptr12 = (int*)reader.ReadNextArray<int>(out num14);
					if (ManagedComponentStore.InstantiateCompanionComponent != null)
					{
						ManagedComponentStore.InstantiateCompanionComponent(ptr8, num15, ptr9, ptr10, ptr11, ptr12, num16, this);
					}
					else
					{
						for (int i = 0; i < num15; i++)
						{
							object obj = this.m_ManagedComponentData[ptr8[i]];
							for (int j = 0; j < num16; j++)
							{
								this.m_ManagedComponentData[ptr12[j]] = obj;
							}
							ptr12 += num16;
						}
					}
					break;
				}
				case 7:
				{
					int num17 = reader.ReadNext<int>();
					for (int k = 0; k < num17; k++)
					{
						int num18 = reader.ReadNext<int>();
						ManagedComponentStore.DisposeManagedComponentData(this.m_ManagedComponentData[num18]);
						this.m_ManagedComponentData[num18] = null;
					}
					break;
				}
				case 8:
				{
					int num19 = reader.ReadNext<int>();
					this.SetManagedComponentCapacity(num19);
					break;
				}
				}
			}
			managedDeferredCommands.Reset();
		}

		// Token: 0x06000F0B RID: 3851 RVA: 0x00043EAC File Offset: 0x000420AC
		private unsafe void CloneManagedComponents(int* srcArray, int componentCount, int* dstArray, int instanceCount)
		{
			for (int i = 0; i < componentCount; i++)
			{
				object obj = this.m_ManagedComponentData[srcArray[i]];
				for (int j = 0; j < instanceCount; j++)
				{
					this.m_ManagedComponentData[dstArray[j]] = this.m_ManagedObjectClone.Clone(obj);
				}
				dstArray += instanceCount;
			}
			this.m_ManagedObjectClone.ClearGCRefs();
		}

		// Token: 0x06000F0C RID: 3852 RVA: 0x00043F0F File Offset: 0x0004210F
		internal void SetManagedComponentValue(int index, object componentObject)
		{
			this.m_ManagedComponentData[index] = componentObject;
		}

		// Token: 0x06000F0D RID: 3853 RVA: 0x00043F1C File Offset: 0x0004211C
		public void ReserveManagedComponentIndicesDirect(int count, ref EntityComponentStore entityComponentStore)
		{
			int managedComponentFreeCount = entityComponentStore.ManagedComponentFreeCount;
			if (managedComponentFreeCount >= count)
			{
				return;
			}
			int num = entityComponentStore.GrowManagedComponentCapacity(count - managedComponentFreeCount);
			this.SetManagedComponentCapacity(num);
		}

		// Token: 0x06000F0E RID: 3854 RVA: 0x00043F48 File Offset: 0x00042148
		public unsafe void UpdateManagedComponentValue(int* index, object value, ref EntityComponentStore entityComponentStore)
		{
			int num = *index;
			if (num != 0 && value != this.m_ManagedComponentData[num])
			{
				object obj = this.m_ManagedComponentData[num];
				IRefCounted refCounted = obj as IRefCounted;
				if (refCounted != null)
				{
					refCounted.Release();
				}
				IDisposable disposable = obj as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
			if (value != null)
			{
				if (num == 0)
				{
					this.ReserveManagedComponentIndicesDirect(1, ref entityComponentStore);
					num = (*index = entityComponentStore.AllocateManagedComponentIndex());
				}
			}
			else
			{
				if (num == 0)
				{
					return;
				}
				*index = 0;
				entityComponentStore.FreeManagedComponentIndex(num);
			}
			this.m_ManagedComponentData[num] = value;
		}

		// Token: 0x06000F0F RID: 3855 RVA: 0x00043FC4 File Offset: 0x000421C4
		public unsafe void CloneManagedComponentsFromDifferentWorld(int* indices, int count, ManagedComponentStore srcManagedComponentStore, ref EntityComponentStore dstEntityComponentStore)
		{
			this.ReserveManagedComponentIndicesDirect(count, ref dstEntityComponentStore);
			for (int i = 0; i < count; i++)
			{
				object obj = srcManagedComponentStore.m_ManagedComponentData[indices[i]];
				object obj2 = this.m_ManagedObjectClone.Clone(obj);
				int num = dstEntityComponentStore.AllocateManagedComponentIndex();
				indices[i] = num;
				ManagedComponentStore.DisposeManagedComponentData(this.m_ManagedComponentData[num]);
				this.m_ManagedComponentData[num] = obj2;
			}
			this.m_ManagedObjectClone.ClearGCRefs();
		}

		// Token: 0x06000F10 RID: 3856 RVA: 0x00044033 File Offset: 0x00042233
		public void ResetManagedComponentStoreForDeserialization(int managedComponentCount, ref EntityComponentStore entityComponentStore)
		{
			managedComponentCount++;
			entityComponentStore.m_ManagedComponentFreeIndex.Length = 0;
			entityComponentStore.m_ManagedComponentIndex = managedComponentCount;
			if (managedComponentCount > entityComponentStore.m_ManagedComponentIndexCapacity)
			{
				entityComponentStore.m_ManagedComponentIndexCapacity = managedComponentCount;
				this.SetManagedComponentCapacity(managedComponentCount);
			}
		}

		// Token: 0x06000F11 RID: 3857 RVA: 0x00044064 File Offset: 0x00042264
		public static void DisposeManagedComponentData(object obj)
		{
			IDisposable disposable = obj as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}

		// Token: 0x040005F9 RID: 1529
		private unsafe readonly EntityComponentStore* m_EntityComponentStore;

		// Token: 0x040005FA RID: 1530
		private readonly ManagedObjectClone m_ManagedObjectClone = new ManagedObjectClone();

		// Token: 0x040005FB RID: 1531
		private readonly ManagedObjectRemap m_ManagedObjectRemap = new ManagedObjectRemap();

		// Token: 0x040005FC RID: 1532
		private UnsafeParallelMultiHashMap<ulong, int> m_HashLookup = new UnsafeParallelMultiHashMap<ulong, int>(128, Allocator.Persistent);

		// Token: 0x040005FD RID: 1533
		internal List<object> m_SharedComponentData = new List<object>();

		// Token: 0x040005FE RID: 1534
		private UnsafeList<ManagedComponentStore.SharedComponentInfo> m_SharedComponentInfo = new UnsafeList<ManagedComponentStore.SharedComponentInfo>(0, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);

		// Token: 0x040005FF RID: 1535
		internal object[] m_ManagedComponentData = new object[64];

		// Token: 0x04000600 RID: 1536
		private int m_FreeListIndex;

		// Token: 0x04000601 RID: 1537
		internal static ManagedComponentStore.InstantiateCompanionComponentDelegate InstantiateCompanionComponent;

		// Token: 0x04000602 RID: 1538
		internal static ManagedComponentStore.AssignCompanionComponentsToCompanionGameObjectsDelegate AssignCompanionComponentsToCompanionGameObjects;

		// Token: 0x02000242 RID: 578
		private struct SharedComponentInfo
		{
			// Token: 0x04000603 RID: 1539
			public int RefCount;

			// Token: 0x04000604 RID: 1540
			public TypeIndex ComponentType;

			// Token: 0x04000605 RID: 1541
			public int Version;

			// Token: 0x04000606 RID: 1542
			public int HashCode;
		}

		// Token: 0x02000243 RID: 579
		// (Invoke) Token: 0x06000F13 RID: 3859
		internal unsafe delegate void InstantiateCompanionComponentDelegate(int* srcArray, int componentCount, Entity* dstEntities, int* dstComponentReferenceIndices, int* dstComponentLinkIds, int* dstArray, int instanceCount, ManagedComponentStore managedComponentStore);

		// Token: 0x02000244 RID: 580
		// (Invoke) Token: 0x06000F17 RID: 3863
		internal delegate void AssignCompanionComponentsToCompanionGameObjectsDelegate(EntityManager entityManager, NativeArray<Entity> entities);

		// Token: 0x02000245 RID: 581
		private sealed class ManagedComponentStoreKeyContext
		{
		}

		// Token: 0x02000246 RID: 582
		private sealed class CompanionReferenceTypeIndexStatic
		{
			// Token: 0x04000607 RID: 1543
			public static readonly SharedStatic<TypeIndex> Ref = SharedStatic<TypeIndex>.GetOrCreateUnsafe(0U, 4526686003341605478L, 7236005522764457664L);
		}

		// Token: 0x02000247 RID: 583
		private sealed class CompanionLinkTypeIndexStatic
		{
			// Token: 0x04000608 RID: 1544
			public static readonly SharedStatic<TypeIndex> Ref = SharedStatic<TypeIndex>.GetOrCreateUnsafe(0U, 4526686003341605478L, -4995373134158615941L);
		}

		// Token: 0x02000248 RID: 584
		private sealed class CompanionLinkTransformTypeIndexStatic
		{
			// Token: 0x04000609 RID: 1545
			public static readonly SharedStatic<TypeIndex> Ref = SharedStatic<TypeIndex>.GetOrCreateUnsafe(0U, 4526686003341605478L, 6382223317753802431L);
		}
	}
}
