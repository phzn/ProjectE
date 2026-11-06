using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Unity.Entities
{
	// Token: 0x02000013 RID: 19
	public struct BlobBuilder : IDisposable
	{
		// Token: 0x06000040 RID: 64 RVA: 0x00002360 File Offset: 0x00000560
		public BlobBuilder(AllocatorManager.AllocatorHandle allocator, int chunkSize = 65536)
		{
			this.m_allocator = allocator;
			this.m_allocations = new NativeList<BlobBuilder.BlobAllocation>(16, this.m_allocator);
			this.m_patches = new NativeList<BlobBuilder.OffsetPtrPatch>(16, this.m_allocator);
			this.m_chunkSize = CollectionHelper.Align(chunkSize, 16);
			this.m_currentChunkIndex = -1;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000023B0 File Offset: 0x000005B0
		public ref T ConstructRoot<T>() where T : struct
		{
			BlobBuilder.BlobDataRef blobDataRef = this.Allocate(UnsafeUtility.SizeOf<T>(), UnsafeUtility.AlignOf<T>());
			return UnsafeUtility.AsRef<T>(this.AllocationToPointer(blobDataRef));
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000023DC File Offset: 0x000005DC
		public unsafe BlobBuilderArray<T> Construct<T>(ref BlobArray<T> blobArray, params T[] data) where T : struct
		{
			BlobBuilderArray<T> blobBuilderArray = this.Allocate<T>(ref blobArray, data.Length);
			for (int num = 0; num != data.Length; num++)
			{
				*blobBuilderArray[num] = data[num];
			}
			return blobBuilderArray;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002417 File Offset: 0x00000617
		public BlobBuilderArray<T> Allocate<T>(ref BlobArray<T> ptr, int length) where T : struct
		{
			return this.Allocate<T>(ref ptr, length, UnsafeUtility.AlignOf<T>());
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002428 File Offset: 0x00000628
		public unsafe BlobBuilderArray<T> Allocate<T>(ref BlobArray<T> ptr, int length, int alignment) where T : struct
		{
			if (length <= 0)
			{
				return new BlobBuilderArray<T>(null, 0);
			}
			if (!math.ispow2(alignment))
			{
				throw new ArgumentException("Alignment must be a power of two");
			}
			if (alignment > 16)
			{
				throw new ArgumentException("Alignment larger than 16 is not supported");
			}
			int* ptr2 = (int*)UnsafeUtility.AddressOf<int>(ref ptr.m_OffsetPtr);
			BlobBuilder.BlobDataRef blobDataRef = this.Allocate(UnsafeUtility.SizeOf<T>() * length, alignment);
			BlobBuilder.OffsetPtrPatch offsetPtrPatch = new BlobBuilder.OffsetPtrPatch
			{
				offsetPtr = ptr2,
				target = blobDataRef,
				length = length
			};
			this.m_patches.Add(in offsetPtrPatch);
			return new BlobBuilderArray<T>(this.AllocationToPointer(blobDataRef), length);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000024BC File Offset: 0x000006BC
		public unsafe ref T Allocate<T>(ref BlobPtr<T> ptr) where T : struct
		{
			int* ptr2 = (int*)UnsafeUtility.AddressOf<int>(ref ptr.m_OffsetPtr);
			BlobBuilder.BlobDataRef blobDataRef = this.Allocate(UnsafeUtility.SizeOf<T>(), UnsafeUtility.AlignOf<T>());
			BlobBuilder.OffsetPtrPatch offsetPtrPatch = new BlobBuilder.OffsetPtrPatch
			{
				offsetPtr = ptr2,
				target = blobDataRef,
				length = 0
			};
			this.m_patches.Add(in offsetPtrPatch);
			return UnsafeUtility.AsRef<T>(this.AllocationToPointer(blobDataRef));
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002524 File Offset: 0x00000724
		public unsafe ref T SetPointer<T>(ref BlobPtr<T> ptr, ref T obj) where T : struct
		{
			int* ptr2 = (int*)UnsafeUtility.AddressOf<int>(ref ptr.m_OffsetPtr);
			void* ptr3 = UnsafeUtility.AddressOf<T>(ref obj);
			BlobBuilder.BlobDataRef blobDataRef;
			if (this.GetPatchTarget(ptr3, out blobDataRef))
			{
				BlobBuilder.OffsetPtrPatch offsetPtrPatch = new BlobBuilder.OffsetPtrPatch
				{
					offsetPtr = ptr2,
					target = blobDataRef,
					length = 0
				};
				this.m_patches.Add(in offsetPtrPatch);
				return ref obj;
			}
			throw new ArgumentException("Reference object is not a part of the blob; can only create BlobPtr to objects inside a blob");
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002590 File Offset: 0x00000790
		public unsafe BlobAssetReference<T> CreateBlobAssetReference<[global::System.Runtime.CompilerServices.IsUnmanaged] T>(AllocatorManager.AllocatorHandle allocator) where T : struct, ValueType
		{
			if (this.m_currentChunkIndex != -1)
			{
				this.AlignChunk(this.m_currentChunkIndex);
			}
			NativeArray<int> nativeArray = new NativeArray<int>(this.m_allocations.Length + 1, Allocator.Temp, NativeArrayOptions.ClearMemory);
			NativeArray<BlobBuilder.SortedIndex> nativeArray2 = new NativeArray<BlobBuilder.SortedIndex>(this.m_allocations.Length, Allocator.Temp, NativeArrayOptions.ClearMemory);
			nativeArray[0] = 0;
			for (int i = 0; i < this.m_allocations.Length; i++)
			{
				nativeArray[i + 1] = nativeArray[i] + this.m_allocations[i].size;
				nativeArray2[i] = new BlobBuilder.SortedIndex
				{
					p = this.m_allocations[i].p,
					index = i
				};
			}
			int num = nativeArray[this.m_allocations.Length];
			nativeArray2.Sort<BlobBuilder.SortedIndex>();
			NativeArray<BlobBuilder.SortedIndex> nativeArray3 = new NativeArray<BlobBuilder.SortedIndex>(this.m_patches.Length, Allocator.Temp, NativeArrayOptions.ClearMemory);
			for (int j = 0; j < this.m_patches.Length; j++)
			{
				nativeArray3[j] = new BlobBuilder.SortedIndex
				{
					p = (byte*)this.m_patches[j].offsetPtr,
					index = j
				};
			}
			nativeArray3.Sort<BlobBuilder.SortedIndex>();
			byte* ptr = (byte*)Memory.Unmanaged.Allocate((long)(sizeof(BlobAssetHeader) + num), 16, allocator);
			byte* ptr2 = ptr + sizeof(BlobAssetHeader);
			for (int k = 0; k < this.m_allocations.Length; k++)
			{
				UnsafeUtility.MemCpy((void*)(ptr2 + nativeArray[k]), (void*)this.m_allocations[k].p, (long)this.m_allocations[k].size);
			}
			int num2 = 0;
			byte* ptr3 = this.m_allocations[nativeArray2[0].index].p;
			byte* ptr4 = ptr3 + this.m_allocations[nativeArray2[0].index].size;
			for (int l = 0; l < this.m_patches.Length; l++)
			{
				int index = nativeArray3[l].index;
				int* p = (int*)nativeArray3[l].p;
				while (p >= (int*)ptr4)
				{
					num2++;
					ptr3 = this.m_allocations[nativeArray2[num2].index].p;
					ptr4 = ptr3 + this.m_allocations[nativeArray2[num2].index].size;
				}
				BlobBuilder.OffsetPtrPatch offsetPtrPatch = this.m_patches[index];
				int num3 = nativeArray[nativeArray2[num2].index] + (int)((long)((byte*)p - (byte*)ptr3));
				int num4 = nativeArray[offsetPtrPatch.target.allocIndex] + offsetPtrPatch.target.offset;
				*(int*)(ptr2 + num3) = num4 - num3;
				if (offsetPtrPatch.length != 0)
				{
					*(int*)(ptr2 + num3 + 4) = offsetPtrPatch.length;
				}
			}
			nativeArray3.Dispose();
			nativeArray2.Dispose();
			nativeArray.Dispose();
			BlobAssetHeader* ptr5 = (BlobAssetHeader*)ptr;
			*ptr5 = default(BlobAssetHeader);
			ptr5->Length = num;
			ptr5->Allocator = allocator;
			ptr5->Hash = (ulong)math.hash((void*)(ptr + sizeof(BlobAssetHeader)), num, 0U);
			BlobAssetReference<T> blobAssetReference;
			blobAssetReference.m_data.m_Align8Union = 0L;
			ptr5->ValidationPtr = (void*)(blobAssetReference.m_data.m_Ptr = ptr + sizeof(BlobAssetHeader));
			return blobAssetReference;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x0000290F File Offset: 0x00000B0F
		private unsafe void* AllocationToPointer(BlobBuilder.BlobDataRef blobDataRef)
		{
			return (void*)(this.m_allocations[blobDataRef.allocIndex].p + blobDataRef.offset);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002930 File Offset: 0x00000B30
		private unsafe BlobBuilder.BlobAllocation EnsureEnoughRoomInChunk(int size, int alignment)
		{
			if (this.m_currentChunkIndex == -1)
			{
				return this.AllocateNewChunk();
			}
			BlobBuilder.BlobAllocation blobAllocation = this.m_allocations[this.m_currentChunkIndex];
			int num = CollectionHelper.Align(blobAllocation.size, alignment);
			if (num + size > this.m_chunkSize)
			{
				return this.AllocateNewChunk();
			}
			UnsafeUtility.MemClear((void*)(blobAllocation.p + blobAllocation.size), (long)(num - blobAllocation.size));
			blobAllocation.size = num;
			return blobAllocation;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000029A4 File Offset: 0x00000BA4
		private unsafe BlobBuilder.BlobDataRef Allocate(int size, int alignment)
		{
			if (size > this.m_chunkSize)
			{
				size = CollectionHelper.Align(size, 16);
				int length = this.m_allocations.Length;
				byte* ptr = (byte*)Memory.Unmanaged.Allocate((long)size, alignment, this.m_allocator);
				UnsafeUtility.MemClear((void*)ptr, (long)size);
				BlobBuilder.BlobAllocation blobAllocation = default(BlobBuilder.BlobAllocation);
				blobAllocation.p = ptr;
				blobAllocation.size = size;
				this.m_allocations.Add(in blobAllocation);
				return new BlobBuilder.BlobDataRef
				{
					allocIndex = length,
					offset = 0
				};
			}
			BlobBuilder.BlobAllocation blobAllocation2 = this.EnsureEnoughRoomInChunk(size, alignment);
			int size2 = blobAllocation2.size;
			UnsafeUtility.MemClear((void*)(blobAllocation2.p + blobAllocation2.size), (long)size);
			blobAllocation2.size += size;
			this.m_allocations[this.m_currentChunkIndex] = blobAllocation2;
			return new BlobBuilder.BlobDataRef
			{
				allocIndex = this.m_currentChunkIndex,
				offset = size2
			};
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002A88 File Offset: 0x00000C88
		private unsafe BlobBuilder.BlobAllocation AllocateNewChunk()
		{
			if (this.m_currentChunkIndex != -1)
			{
				this.AlignChunk(this.m_currentChunkIndex);
			}
			this.m_currentChunkIndex = this.m_allocations.Length;
			BlobBuilder.BlobAllocation blobAllocation = new BlobBuilder.BlobAllocation
			{
				p = (byte*)Memory.Unmanaged.Allocate((long)this.m_chunkSize, 16, this.m_allocator),
				size = 0
			};
			this.m_allocations.Add(in blobAllocation);
			return blobAllocation;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002AF8 File Offset: 0x00000CF8
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private unsafe void ValidateAllocation(void* address)
		{
			for (int i = this.m_allocations.Length - 1; i >= 0; i--)
			{
				BlobBuilder.BlobAllocation blobAllocation = this.m_allocations[i];
				if (address >= (void*)blobAllocation.p && address < (void*)(blobAllocation.p + blobAllocation.size))
				{
					return;
				}
			}
			throw new InvalidOperationException("The BlobArray passed to Allocate was not allocated by this BlobBuilder or the struct that embeds it was copied by value instead of by ref.");
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002B50 File Offset: 0x00000D50
		private unsafe bool GetPatchTarget(void* address, out BlobBuilder.BlobDataRef blobDataRef)
		{
			for (int i = this.m_allocations.Length - 1; i >= 0; i--)
			{
				BlobBuilder.BlobAllocation blobAllocation = this.m_allocations[i];
				if (address >= (void*)blobAllocation.p && address < (void*)(blobAllocation.p + blobAllocation.size))
				{
					blobDataRef = new BlobBuilder.BlobDataRef
					{
						allocIndex = i,
						offset = (int)((long)((byte*)address - (byte*)blobAllocation.p))
					};
					return true;
				}
			}
			blobDataRef = default(BlobBuilder.BlobDataRef);
			return false;
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600004E RID: 78 RVA: 0x00002BCF File Offset: 0x00000DCF
		public readonly bool IsCreated
		{
			get
			{
				return this.m_allocations.IsCreated;
			}
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002BDC File Offset: 0x00000DDC
		public void Dispose()
		{
			for (int i = 0; i < this.m_allocations.Length; i++)
			{
				Memory.Unmanaged.Free<byte>(this.m_allocations[i].p, this.m_allocator);
			}
			this.m_allocations.Dispose();
			this.m_patches.Dispose();
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002C34 File Offset: 0x00000E34
		private unsafe void AlignChunk(int chunkIndex)
		{
			BlobBuilder.BlobAllocation blobAllocation = this.m_allocations[chunkIndex];
			int size = blobAllocation.size;
			blobAllocation.size = CollectionHelper.Align(blobAllocation.size, 16);
			this.m_allocations[chunkIndex] = blobAllocation;
			UnsafeUtility.MemSet((void*)(blobAllocation.p + size), 0, (long)(blobAllocation.size - size));
		}

		// Token: 0x04000010 RID: 16
		private AllocatorManager.AllocatorHandle m_allocator;

		// Token: 0x04000011 RID: 17
		private NativeList<BlobBuilder.BlobAllocation> m_allocations;

		// Token: 0x04000012 RID: 18
		private NativeList<BlobBuilder.OffsetPtrPatch> m_patches;

		// Token: 0x04000013 RID: 19
		private int m_currentChunkIndex;

		// Token: 0x04000014 RID: 20
		private int m_chunkSize;

		// Token: 0x02000014 RID: 20
		private struct BlobAllocation
		{
			// Token: 0x04000015 RID: 21
			public int size;

			// Token: 0x04000016 RID: 22
			public unsafe byte* p;
		}

		// Token: 0x02000015 RID: 21
		private struct BlobDataRef
		{
			// Token: 0x04000017 RID: 23
			public int allocIndex;

			// Token: 0x04000018 RID: 24
			public int offset;
		}

		// Token: 0x02000016 RID: 22
		private struct OffsetPtrPatch
		{
			// Token: 0x04000019 RID: 25
			public unsafe int* offsetPtr;

			// Token: 0x0400001A RID: 26
			public BlobBuilder.BlobDataRef target;

			// Token: 0x0400001B RID: 27
			public int length;
		}

		// Token: 0x02000017 RID: 23
		private struct SortedIndex : IComparable<BlobBuilder.SortedIndex>
		{
			// Token: 0x06000051 RID: 81 RVA: 0x00002C90 File Offset: 0x00000E90
			public int CompareTo(BlobBuilder.SortedIndex other)
			{
				return this.p.CompareTo(other.p);
			}

			// Token: 0x0400001C RID: 28
			public unsafe byte* p;

			// Token: 0x0400001D RID: 29
			public int index;
		}
	}
}
