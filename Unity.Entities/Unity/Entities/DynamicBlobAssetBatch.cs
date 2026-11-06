using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x02000072 RID: 114
	internal struct DynamicBlobAssetBatch
	{
		// Token: 0x060002A8 RID: 680 RVA: 0x0000C2A8 File Offset: 0x0000A4A8
		public unsafe static DynamicBlobAssetBatch* Allocate(AllocatorManager.AllocatorHandle allocator)
		{
			DynamicBlobAssetBatch* ptr = (DynamicBlobAssetBatch*)Memory.Unmanaged.Allocate((long)sizeof(DynamicBlobAssetBatch), UnsafeUtility.AlignOf<DynamicBlobAssetBatch>(), allocator);
			ptr->m_FramesToRetainBlobAssets = 1;
			ptr->m_Allocator = allocator;
			ptr->m_BlobAssets = UnsafeList<BlobAssetPtr>.Create(1, allocator, NativeArrayOptions.UninitializedMemory);
			return ptr;
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000C2E8 File Offset: 0x0000A4E8
		public unsafe static void Free(DynamicBlobAssetBatch* batch)
		{
			BlobAssetPtr* ptr = batch->m_BlobAssets->Ptr;
			for (int i = 0; i < batch->m_BlobAssets->Length; i++)
			{
				Memory.Unmanaged.Free<BlobAssetHeader>(ptr[i].Header, batch->m_Allocator);
			}
			UnsafeList<BlobAssetPtr>.Destroy(batch->m_BlobAssets);
			Memory.Unmanaged.Free<DynamicBlobAssetBatch>(batch, batch->m_Allocator);
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000C349 File Offset: 0x0000A549
		public void SetFramesToRetainBlobAssets(int framesToRetainBlobAssets)
		{
			this.m_FramesToRetainBlobAssets = framesToRetainBlobAssets;
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000C354 File Offset: 0x0000A554
		public unsafe NativeList<BlobAssetPtr> ToNativeList(AllocatorManager.AllocatorHandle allocator)
		{
			NativeList<BlobAssetPtr> nativeList = new NativeList<BlobAssetPtr>(this.m_BlobAssets->Length, allocator);
			nativeList.ResizeUninitialized(this.m_BlobAssets->Length);
			UnsafeUtility.MemCpy((void*)nativeList.GetUnsafePtr<BlobAssetPtr>(), (void*)this.m_BlobAssets->Ptr, (long)(sizeof(BlobAssetPtr) * this.m_BlobAssets->Length));
			return nativeList;
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000C3B0 File Offset: 0x0000A5B0
		public unsafe BlobAssetPtr AllocateBlobAsset(void* data, int length, ulong hash)
		{
			BlobAssetHeader* ptr = (BlobAssetHeader*)Memory.Unmanaged.Allocate((long)(length + sizeof(BlobAssetHeader)), 16, this.m_Allocator);
			ptr->Length = length;
			ptr->ValidationPtr = (void*)(ptr + 1);
			ptr->Allocator = Allocator.None;
			ptr->Hash = hash;
			UnsafeUtility.MemCpy((void*)(ptr + 1), data, (long)length);
			ref UnsafeList<BlobAssetPtr> blobAssets = ref *this.m_BlobAssets;
			BlobAssetPtr blobAssetPtr = new BlobAssetPtr(ptr);
			blobAssets.Add(in blobAssetPtr);
			return new BlobAssetPtr(ptr);
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000C428 File Offset: 0x0000A628
		public unsafe void SortByHash()
		{
			NativeSortExtension.Sort<BlobAssetPtr, BlobAssetPtrHashComparer>(this.m_BlobAssets->Ptr, this.m_BlobAssets->Length, default(BlobAssetPtrHashComparer));
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000C45C File Offset: 0x0000A65C
		public unsafe bool TryGetBlobAsset(ulong hash, out BlobAssetPtr blobAssetPtr)
		{
			BlobAssetPtr* ptr = this.m_BlobAssets->Ptr;
			for (int i = 0; i < this.m_BlobAssets->Length; i++)
			{
				if (ptr[i].Header->Hash == hash)
				{
					blobAssetPtr = new BlobAssetPtr(ptr[i].Header);
					return true;
				}
			}
			blobAssetPtr = default(BlobAssetPtr);
			return false;
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000C4C8 File Offset: 0x0000A6C8
		public unsafe void ReleaseBlobAsset(EntityManager entityManager, ulong hash)
		{
			BlobAssetPtr* ptr = this.m_BlobAssets->Ptr;
			for (int i = 0; i < this.m_BlobAssets->Length; i++)
			{
				if (ptr[i].Hash == hash)
				{
					Entity entity = entityManager.CreateEntity(new ComponentType[]
					{
						ComponentType.ReadWrite<RetainBlobAssets>(),
						ComponentType.ReadWrite<RetainBlobAssetPtr>()
					});
					entityManager.SetComponentData<RetainBlobAssets>(entity, new RetainBlobAssets
					{
						FramesToRetainBlobAssets = this.m_FramesToRetainBlobAssets
					});
					entityManager.SetComponentData<RetainBlobAssetPtr>(entity, new RetainBlobAssetPtr
					{
						BlobAsset = ptr[i].Header
					});
					entityManager.DestroyEntity(entity);
					this.m_BlobAssets->RemoveAtSwapBack(i);
					return;
				}
			}
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000C598 File Offset: 0x0000A798
		public unsafe void ReleaseBlobAssetImmediately(ulong hash)
		{
			BlobAssetPtr* ptr = this.m_BlobAssets->Ptr;
			for (int i = 0; i < this.m_BlobAssets->Length; i++)
			{
				if (ptr[i].Hash == hash)
				{
					Memory.Unmanaged.Free<BlobAssetHeader>(ptr[i].Header, this.m_Allocator);
					this.m_BlobAssets->RemoveAtSwapBack(i);
					return;
				}
			}
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000C604 File Offset: 0x0000A804
		public unsafe void RemoveUnusedBlobAssets(NativeParallelHashMap<ulong, int> usedBlobAssets)
		{
			BlobAssetPtr* ptr = this.m_BlobAssets->Ptr;
			for (int i = 0; i < this.m_BlobAssets->Length; i++)
			{
				if (!usedBlobAssets.ContainsKey(ptr[i].Hash))
				{
					Memory.Unmanaged.Free<BlobAssetHeader>(ptr[i].Header, this.m_Allocator);
					this.m_BlobAssets->RemoveAtSwapBack(i--);
				}
			}
		}

		// Token: 0x040000B5 RID: 181
		private AllocatorManager.AllocatorHandle m_Allocator;

		// Token: 0x040000B6 RID: 182
		private int m_FramesToRetainBlobAssets;

		// Token: 0x040000B7 RID: 183
		private unsafe UnsafeList<BlobAssetPtr>* m_BlobAssets;
	}
}
