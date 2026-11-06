using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x0200013D RID: 317
	internal struct ManagedDeferredCommands : IDisposable
	{
		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000658 RID: 1624 RVA: 0x000218E9 File Offset: 0x0001FAE9
		public bool Empty
		{
			get
			{
				return this.CommandBuffer.IsEmpty;
			}
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x000218F6 File Offset: 0x0001FAF6
		public unsafe void Init(EntityComponentStore* ecs)
		{
			this.ECS = ecs;
			this.CommandBuffer = new UnsafeAppendBuffer(1024, 16, Allocator.Persistent);
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x00021917 File Offset: 0x0001FB17
		public void Dispose()
		{
			this.CommandBuffer.Dispose();
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x00021924 File Offset: 0x0001FB24
		public void Reset()
		{
			this.CommandBuffer.Reset();
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x00021934 File Offset: 0x0001FB34
		public unsafe void IncrementComponentOrderVersion(Archetype* archetype, SharedComponentValues sharedComponentValues)
		{
			for (int i = 0; i < archetype->NumSharedComponents; i++)
			{
				int num = *sharedComponentValues[i];
				if (EntityComponentStore.IsUnmanagedSharedComponentIndex(num))
				{
					this.ECS->IncrementSharedComponentVersion_Unmanaged(num);
				}
				else
				{
					this.CommandBuffer.Add<int>(0);
					this.CommandBuffer.Add<int>(num);
				}
			}
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x0002198C File Offset: 0x0001FB8C
		public unsafe void PatchEntities(Archetype* archetype, ChunkIndex chunk, int entityCount, NativeArray<EntityRemapUtility.EntityRemapInfo> remapping)
		{
			this.CommandBuffer.Add<int>(1);
			this.CommandBuffer.Add<IntPtr>((IntPtr)((void*)archetype));
			this.CommandBuffer.Add<ChunkIndex>(chunk);
			this.CommandBuffer.Add<int>(entityCount);
			this.CommandBuffer.Add<IntPtr>((IntPtr)remapping.GetUnsafePtr<EntityRemapUtility.EntityRemapInfo>());
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x000219E8 File Offset: 0x0001FBE8
		public unsafe void PatchEntitiesForPrefab(Archetype* archetype, ChunkIndex chunk, int indexInChunk, int allocatedCount, Entity* remapSrc, Entity* remapDst, int remappingCount, AllocatorManager.AllocatorHandle allocator)
		{
			int numManagedComponents = archetype->NumManagedComponents;
			int num = numManagedComponents * allocatedCount;
			int num2 = UnsafeUtility.SizeOf<Entity>() * remappingCount;
			int num3 = UnsafeUtility.SizeOf<Entity>() * remappingCount * allocatedCount;
			int num4 = num * 4;
			byte* ptr = (byte*)Memory.Unmanaged.Allocate((long)(num2 + num3 + num4), 16, Allocator.Temp);
			byte* ptr2 = ptr + num2;
			int* ptr3 = (int*)(ptr2 + num3);
			UnsafeUtility.MemCpy((void*)ptr, (void*)remapSrc, (long)num2);
			UnsafeUtility.MemCpy((void*)ptr2, (void*)remapDst, (long)num3);
			short firstManagedComponent = archetype->FirstManagedComponent;
			for (int i = 0; i < numManagedComponents; i++)
			{
				int num5 = i + (int)firstManagedComponent;
				if (archetype->Types[num5].HasEntityReferences)
				{
					int* componentDataRO = (int*)ChunkDataUtility.GetComponentDataRO(chunk, archetype, 0, num5);
					for (int j = 0; j < allocatedCount; j++)
					{
						ptr3[j * numManagedComponents + i] = componentDataRO[j + indexInChunk];
					}
				}
				else
				{
					for (int k = 0; k < allocatedCount; k++)
					{
						ptr3[k * numManagedComponents + i] = 0;
					}
				}
			}
			this.CommandBuffer.Add<int>(2);
			this.CommandBuffer.Add<IntPtr>((IntPtr)((void*)ptr));
			this.CommandBuffer.Add<int>(allocatedCount);
			this.CommandBuffer.Add<int>(remappingCount);
			this.CommandBuffer.Add<int>(archetype->NumManagedComponents);
			this.CommandBuffer.Add<int>(allocator.Value);
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x00021B37 File Offset: 0x0001FD37
		public void AddReference(int index, int numRefs = 1)
		{
			if (index == 0)
			{
				return;
			}
			this.CommandBuffer.Add<int>(3);
			this.CommandBuffer.Add<int>(index);
			this.CommandBuffer.Add<int>(numRefs);
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x00021B61 File Offset: 0x0001FD61
		public void RemoveReference(int index, int numRefs = 1)
		{
			if (index == 0)
			{
				return;
			}
			this.CommandBuffer.Add<int>(4);
			this.CommandBuffer.Add<int>(index);
			this.CommandBuffer.Add<int>(numRefs);
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x00021B8B File Offset: 0x0001FD8B
		public unsafe void CloneManagedComponentBegin(int* srcIndices, int componentCount, int instanceCount)
		{
			this.CommandBuffer.Add<int>(5);
			this.CommandBuffer.AddArray<int>((void*)srcIndices, componentCount);
			this.CommandBuffer.Add<int>(instanceCount);
			this.CommandBuffer.Add<int>(instanceCount * componentCount);
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x00021BC0 File Offset: 0x0001FDC0
		public unsafe void CloneManagedComponentAddDstIndices(int* dstIndices, int count)
		{
			this.CommandBuffer.Add((void*)dstIndices, count * 4);
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x00021BD4 File Offset: 0x0001FDD4
		public unsafe void CloneCompanionComponentBegin(int* srcIndices, int componentCount, Entity* dstEntities, int instanceCount, int* dstCompanionReferenceIndices, int* dstCompanionLinkIds)
		{
			this.CommandBuffer.Add<int>(6);
			this.CommandBuffer.AddArray<int>((void*)srcIndices, componentCount);
			this.CommandBuffer.AddArray<Entity>((void*)dstEntities, instanceCount);
			this.CommandBuffer.AddArray<int>((void*)dstCompanionReferenceIndices, (dstCompanionReferenceIndices == null) ? 0 : instanceCount);
			this.CommandBuffer.AddArray<int>((void*)dstCompanionLinkIds, (dstCompanionLinkIds == null) ? 0 : instanceCount);
			this.CommandBuffer.Add<int>(instanceCount * componentCount);
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x00021BC0 File Offset: 0x0001FDC0
		public unsafe void CloneCompanionComponentAddDstIndices(int* dstIndices, int count)
		{
			this.CommandBuffer.Add((void*)dstIndices, count * 4);
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x00021C47 File Offset: 0x0001FE47
		public int BeginFreeManagedComponentCommand()
		{
			this.CommandBuffer.Add<int>(7);
			this.CommandBuffer.Add<int>(-1);
			return this.CommandBuffer.Length - 4;
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x00021C6E File Offset: 0x0001FE6E
		public void AddToFreeManagedComponentCommand(int managedComponentIndex)
		{
			this.CommandBuffer.Add<int>(managedComponentIndex);
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x00021C7C File Offset: 0x0001FE7C
		public unsafe void EndDeallocateManagedComponentCommand(int handle)
		{
			int num = (this.CommandBuffer.Length - handle) / 4 - 1;
			if (num == 0)
			{
				this.CommandBuffer.Length = this.CommandBuffer.Length - 8;
				return;
			}
			int* ptr = (int*)(this.CommandBuffer.Ptr + handle);
			*ptr = num;
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x00021CC0 File Offset: 0x0001FEC0
		public void SetManagedComponentCapacity(int capacity)
		{
			this.CommandBuffer.Add<int>(8);
			this.CommandBuffer.Add<int>(capacity);
		}

		// Token: 0x04000387 RID: 903
		private unsafe EntityComponentStore* ECS;

		// Token: 0x04000388 RID: 904
		public UnsafeAppendBuffer CommandBuffer;

		// Token: 0x0200013E RID: 318
		public enum Command
		{
			// Token: 0x0400038A RID: 906
			IncrementSharedComponentVersion,
			// Token: 0x0400038B RID: 907
			PatchManagedEntities,
			// Token: 0x0400038C RID: 908
			PatchManagedEntitiesForPrefabs,
			// Token: 0x0400038D RID: 909
			AddReference,
			// Token: 0x0400038E RID: 910
			RemoveReference,
			// Token: 0x0400038F RID: 911
			CloneManagedComponents,
			// Token: 0x04000390 RID: 912
			CloneCompanionComponents,
			// Token: 0x04000391 RID: 913
			FreeManagedComponents,
			// Token: 0x04000392 RID: 914
			SetManagedComponentCapacity
		}
	}
}
