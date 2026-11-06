using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x020002E0 RID: 736
	[DebuggerTypeProxy(typeof(ArchetypeChunkDataDebugView))]
	internal struct ArchetypeChunkData
	{
		// Token: 0x1700015B RID: 347
		// (get) Token: 0x060013D5 RID: 5077 RVA: 0x0004CEF6 File Offset: 0x0004B0F6
		// (set) Token: 0x060013D6 RID: 5078 RVA: 0x0004CEFE File Offset: 0x0004B0FE
		public int Capacity { readonly get; private set; }

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x060013D7 RID: 5079 RVA: 0x0004CF07 File Offset: 0x0004B107
		// (set) Token: 0x060013D8 RID: 5080 RVA: 0x0004CF0F File Offset: 0x0004B10F
		public int Count { readonly get; private set; }

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x060013D9 RID: 5081 RVA: 0x0004CF18 File Offset: 0x0004B118
		private unsafe ulong ChunkIndicesSize
		{
			get
			{
				return (ulong)((long)(sizeof(ChunkIndex) * this.Capacity));
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x060013DA RID: 5082 RVA: 0x0004CF28 File Offset: 0x0004B128
		private ulong ChangeVersionSize
		{
			get
			{
				return (ulong)((long)(4 * this.ComponentCount * this.Capacity));
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x060013DB RID: 5083 RVA: 0x0004CF3A File Offset: 0x0004B13A
		private ulong EntityCountSize
		{
			get
			{
				return (ulong)((long)(4 * this.Capacity));
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x060013DC RID: 5084 RVA: 0x0004CF45 File Offset: 0x0004B145
		private ulong SharedComponentValuesSize
		{
			get
			{
				return (ulong)((long)(4 * this.SharedComponentCount * this.Capacity));
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x060013DD RID: 5085 RVA: 0x0004CF57 File Offset: 0x0004B157
		private ulong PaddingForEnabledBitAlignmentSize
		{
			get
			{
				return 16UL - (this.ChunkIndicesSize + this.ChangeVersionSize + this.EntityCountSize + this.SharedComponentValuesSize) % 16UL;
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x060013DE RID: 5086 RVA: 0x0004CF7C File Offset: 0x0004B17C
		public ulong ComponentEnabledBitsSizeTotalPerChunk
		{
			get
			{
				return (ulong)(16L * (long)this.ComponentCount);
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x060013DF RID: 5087 RVA: 0x0004CF89 File Offset: 0x0004B189
		private ulong ComponentEnabledBitsSize
		{
			get
			{
				return this.ComponentEnabledBitsSizeTotalPerChunk * (ulong)((long)this.Capacity);
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x060013E0 RID: 5088 RVA: 0x0004CF99 File Offset: 0x0004B199
		public ulong ComponentEnabledBitsHierarchicalDataSizePerChunk
		{
			get
			{
				return (ulong)((long)(4 * this.ComponentCount));
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x060013E1 RID: 5089 RVA: 0x0004CFA4 File Offset: 0x0004B1A4
		private ulong ComponentEnabledBitsHierarchicalDataSize
		{
			get
			{
				return this.ComponentEnabledBitsHierarchicalDataSizePerChunk * (ulong)((long)this.Capacity);
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x060013E2 RID: 5090 RVA: 0x0004CFB4 File Offset: 0x0004B1B4
		private ulong BufferSize
		{
			get
			{
				return this.ChunkIndicesSize + this.ChangeVersionSize + this.EntityCountSize + this.SharedComponentValuesSize + this.PaddingForEnabledBitAlignmentSize + this.ComponentEnabledBitsSize + this.ComponentEnabledBitsHierarchicalDataSize;
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x060013E3 RID: 5091 RVA: 0x0004CFE6 File Offset: 0x0004B1E6
		private unsafe ChunkIndex* ChunkIndices
		{
			get
			{
				return (ChunkIndex*)this.m_Data;
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x060013E4 RID: 5092 RVA: 0x0004CFEE File Offset: 0x0004B1EE
		private unsafe uint* ChangeVersions
		{
			get
			{
				return (uint*)this.m_Data + this.ChunkIndicesSize / 4UL;
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x060013E5 RID: 5093 RVA: 0x0004CFFF File Offset: 0x0004B1FF
		private unsafe int* EntityCount
		{
			get
			{
				return (int*)this.ChangeVersions + this.ChangeVersionSize / 4UL;
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x060013E6 RID: 5094 RVA: 0x0004D010 File Offset: 0x0004B210
		private unsafe int* SharedComponentValues
		{
			get
			{
				return this.EntityCount + this.EntityCountSize / 4UL;
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x060013E7 RID: 5095 RVA: 0x0004D021 File Offset: 0x0004B221
		private unsafe v128* ComponentEnabledBits
		{
			get
			{
				return (v128*)this.SharedComponentValues + this.SharedComponentValuesSize / (ulong)sizeof(v128) + this.PaddingForEnabledBitAlignmentSize / (ulong)sizeof(v128);
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x060013E8 RID: 5096 RVA: 0x0004D039 File Offset: 0x0004B239
		private unsafe int* ComponentEnabledBitsHierarchicalData
		{
			get
			{
				return (int*)this.ComponentEnabledBits + this.ComponentEnabledBitsSize / 4UL;
			}
		}

		// Token: 0x060013E9 RID: 5097 RVA: 0x0004D04A File Offset: 0x0004B24A
		public ArchetypeChunkData(int componentCount, int sharedComponentCount)
		{
			this.m_Data = null;
			this.Capacity = 0;
			this.Count = 0;
			this.SharedComponentCount = sharedComponentCount;
			this.ComponentCount = componentCount;
		}

		// Token: 0x1700016D RID: 365
		public unsafe ChunkIndex this[int index]
		{
			get
			{
				return this.ChunkIndices[index];
			}
		}

		// Token: 0x060013EB RID: 5099 RVA: 0x0004D088 File Offset: 0x0004B288
		public unsafe void Grow(int nextCapacity)
		{
			ulong num = (ulong)((long)(sizeof(ChunkIndex) * nextCapacity));
			ulong num2 = (ulong)((long)(4 * this.ComponentCount * nextCapacity));
			ulong num3 = (ulong)((long)(4 * nextCapacity));
			ulong num4 = (ulong)((long)(4 * this.SharedComponentCount * nextCapacity));
			ulong num5 = 16UL - (num + num2 + num3 + num4) % 16UL;
			ulong num6 = this.ComponentEnabledBitsSizeTotalPerChunk * (ulong)((long)nextCapacity);
			ulong num7 = this.ComponentEnabledBitsHierarchicalDataSizePerChunk * (ulong)((long)nextCapacity);
			void* ptr = Memory.Unmanaged.Allocate((long)(num + num2 + num3 + num4 + num5 + num6 + num7), 16, Allocator.Persistent);
			void* ptr2 = ptr;
			byte* ptr3 = (byte*)ptr + num;
			uint* ptr4 = (uint*)ptr3;
			byte* ptr5 = (byte*)ptr3 + num2;
			int* ptr6 = (int*)ptr5;
			byte* ptr7 = (byte*)ptr5 + num3;
			int* ptr8 = (int*)ptr7;
			byte* ptr9 = (byte*)((byte*)ptr7 + num4) + num5;
			byte* ptr10 = ptr9;
			int* ptr11 = (int*)((byte*)ptr9 + num6);
			int count = this.Count;
			int capacity = this.Capacity;
			void* data = this.m_Data;
			uint* changeVersions = this.ChangeVersions;
			int* entityCount = this.EntityCount;
			int* sharedComponentValues = this.SharedComponentValues;
			v128* componentEnabledBits = this.ComponentEnabledBits;
			int* componentEnabledBitsHierarchicalData = this.ComponentEnabledBitsHierarchicalData;
			UnsafeUtility.MemCpy(ptr2, data, (long)(sizeof(ChunkIndex) * count));
			for (int i = 0; i < this.ComponentCount; i++)
			{
				UnsafeUtility.MemCpy((void*)(ptr4 + i * nextCapacity), (void*)(changeVersions + i * capacity), (long)(4 * this.Count));
			}
			for (int j = 0; j < this.SharedComponentCount; j++)
			{
				UnsafeUtility.MemCpy((void*)(ptr8 + j * nextCapacity), (void*)(sharedComponentValues + j * capacity), (long)(4 * this.Count));
			}
			UnsafeUtility.MemCpy((void*)ptr6, (void*)entityCount, (long)(4 * this.Count));
			UnsafeUtility.MemCpy((void*)ptr10, (void*)componentEnabledBits, (long)this.ComponentEnabledBitsSize);
			UnsafeUtility.MemCpy((void*)ptr11, (void*)componentEnabledBitsHierarchicalData, (long)this.ComponentEnabledBitsHierarchicalDataSize);
			Memory.Unmanaged.Free(this.m_Data, Allocator.Persistent);
			this.m_Data = ptr2;
			this.Capacity = nextCapacity;
		}

		// Token: 0x060013EC RID: 5100 RVA: 0x0004D23C File Offset: 0x0004B43C
		public bool InsideAllocation(ulong addr)
		{
			ulong num = this.m_Data;
			return addr >= num && addr <= num + this.BufferSize;
		}

		// Token: 0x060013ED RID: 5101 RVA: 0x0004D265 File Offset: 0x0004B465
		public unsafe int* GetSharedComponentValueArrayForType(int sharedComponentIndexInArchetype)
		{
			return this.SharedComponentValues + sharedComponentIndexInArchetype * this.Capacity;
		}

		// Token: 0x060013EE RID: 5102 RVA: 0x0004D279 File Offset: 0x0004B479
		public unsafe int GetSharedComponentValue(int sharedComponentIndexInArchetype, int chunkIndex)
		{
			return this.GetSharedComponentValueArrayForType(sharedComponentIndexInArchetype)[chunkIndex];
		}

		// Token: 0x060013EF RID: 5103 RVA: 0x0004D288 File Offset: 0x0004B488
		public unsafe void SetSharedComponentValue(int sharedComponentIndexInArchetype, int chunkIndex, int value)
		{
			this.GetSharedComponentValueArrayForType(sharedComponentIndexInArchetype)[chunkIndex] = value;
		}

		// Token: 0x060013F0 RID: 5104 RVA: 0x0004D298 File Offset: 0x0004B498
		public SharedComponentValues GetSharedComponentValues(int chunkIndex)
		{
			return new SharedComponentValues
			{
				firstIndex = this.SharedComponentValues + chunkIndex,
				stride = this.Capacity * 4
			};
		}

		// Token: 0x060013F1 RID: 5105 RVA: 0x0004D2CF File Offset: 0x0004B4CF
		public unsafe v128* GetComponentEnabledMaskArrayForChunk(int chunkIndex)
		{
			return this.ComponentEnabledBits + this.ComponentCount * chunkIndex;
		}

		// Token: 0x060013F2 RID: 5106 RVA: 0x0004D2E8 File Offset: 0x0004B4E8
		public int GetComponentEnabledBitsSizePerChunk()
		{
			return (int)this.ComponentEnabledBitsSizeTotalPerChunk;
		}

		// Token: 0x060013F3 RID: 5107 RVA: 0x0004D2F4 File Offset: 0x0004B4F4
		public unsafe UnsafeBitArray GetEnabledArrayForTypeInChunk(int typeMemoryOrderIndexInArchetype, int chunkIndex)
		{
			return new UnsafeBitArray((void*)(this.ComponentEnabledBits + this.ComponentCount * chunkIndex + typeMemoryOrderIndexInArchetype), 16, default(AllocatorManager.AllocatorHandle));
		}

		// Token: 0x060013F4 RID: 5108 RVA: 0x0004D332 File Offset: 0x0004B532
		public unsafe v128* GetComponentEnabledMaskArrayForTypeInChunk(int typeMemoryOrderIndexInArchetype, int chunkIndex)
		{
			return this.ComponentEnabledBits + this.ComponentCount * chunkIndex + typeMemoryOrderIndexInArchetype;
		}

		// Token: 0x060013F5 RID: 5109 RVA: 0x0004D355 File Offset: 0x0004B555
		public unsafe void InitializeDisabledCountForChunk(int chunkIndex)
		{
			UnsafeUtility.MemClear((void*)(this.ComponentEnabledBitsHierarchicalData + chunkIndex * this.ComponentCount), (long)this.ComponentEnabledBitsHierarchicalDataSizePerChunk);
		}

		// Token: 0x060013F6 RID: 5110 RVA: 0x0004D374 File Offset: 0x0004B574
		public unsafe void SetEnabledBitsAndHierarchicalData(int chunkIndex, byte* componentEnabledBitValues, int* perComponentDisabledBitCount)
		{
			UnsafeUtility.MemCpy((void*)(this.ComponentEnabledBits + this.ComponentCount * chunkIndex), (void*)componentEnabledBitValues, (long)this.ComponentEnabledBitsSizeTotalPerChunk);
			UnsafeUtility.MemCpy((void*)(this.ComponentEnabledBitsHierarchicalData + chunkIndex * this.ComponentCount), (void*)perComponentDisabledBitCount, (long)this.ComponentEnabledBitsHierarchicalDataSizePerChunk);
		}

		// Token: 0x060013F7 RID: 5111 RVA: 0x0004D3C2 File Offset: 0x0004B5C2
		public unsafe int* GetChunkDisabledCounts(int chunkIndex)
		{
			return this.ComponentEnabledBitsHierarchicalData + chunkIndex * this.ComponentCount;
		}

		// Token: 0x060013F8 RID: 5112 RVA: 0x0004D3D6 File Offset: 0x0004B5D6
		public unsafe int GetChunkDisabledCountForType(int typeMemoryOrderIndexInArchetype, int chunkIndex)
		{
			return (this.ComponentEnabledBitsHierarchicalData + chunkIndex * this.ComponentCount)[typeMemoryOrderIndexInArchetype];
		}

		// Token: 0x060013F9 RID: 5113 RVA: 0x0004D3F0 File Offset: 0x0004B5F0
		public unsafe byte* GetPointerToComponentEnabledArrayForArchetype()
		{
			return (byte*)this.ComponentEnabledBits;
		}

		// Token: 0x060013FA RID: 5114 RVA: 0x0004D3F8 File Offset: 0x0004B5F8
		public unsafe int* GetPointerToChunkDisabledCountForArchetype()
		{
			return this.ComponentEnabledBitsHierarchicalData;
		}

		// Token: 0x060013FB RID: 5115 RVA: 0x0004D400 File Offset: 0x0004B600
		public unsafe int* GetPointerToChunkDisabledCountForType(int typeMemoryOrderIndexInArchetype, int chunkIndex)
		{
			return this.ComponentEnabledBitsHierarchicalData + chunkIndex * this.ComponentCount + typeMemoryOrderIndexInArchetype;
		}

		// Token: 0x060013FC RID: 5116 RVA: 0x0004D419 File Offset: 0x0004B619
		public unsafe void SetChunkDisabledCountForType(int typeMemoryOrderIndexInArchetype, int chunkIndex, int value)
		{
			(this.ComponentEnabledBitsHierarchicalData + chunkIndex * this.ComponentCount)[typeMemoryOrderIndexInArchetype] = value;
		}

		// Token: 0x060013FD RID: 5117 RVA: 0x0004D434 File Offset: 0x0004B634
		public unsafe void AdjustChunkDisabledCountForType(int typeMemoryOrderIndexInArchetype, int chunkIndex, int value)
		{
			(this.ComponentEnabledBitsHierarchicalData + chunkIndex * this.ComponentCount)[typeMemoryOrderIndexInArchetype] += value;
		}

		// Token: 0x060013FE RID: 5118 RVA: 0x0004D452 File Offset: 0x0004B652
		public unsafe uint* GetChangeVersionArrayForType(int indexInArchetype)
		{
			return this.ChangeVersions + indexInArchetype * this.Capacity;
		}

		// Token: 0x060013FF RID: 5119 RVA: 0x0004D466 File Offset: 0x0004B666
		public unsafe uint GetChangeVersion(int indexInArchetype, int chunkIndex)
		{
			return this.GetChangeVersionArrayForType(indexInArchetype)[chunkIndex];
		}

		// Token: 0x06001400 RID: 5120 RVA: 0x0004D475 File Offset: 0x0004B675
		public uint GetOrderVersion(int chunkIndex)
		{
			return this.GetChangeVersion(0, chunkIndex);
		}

		// Token: 0x06001401 RID: 5121 RVA: 0x0004D47F File Offset: 0x0004B67F
		public unsafe void SetChangeVersion(int indexInArchetype, int chunkIndex, uint version)
		{
			this.GetChangeVersionArrayForType(indexInArchetype)[chunkIndex] = version;
		}

		// Token: 0x06001402 RID: 5122 RVA: 0x0004D490 File Offset: 0x0004B690
		public unsafe void SetAllChangeVersion(int chunkIndex, uint version)
		{
			for (int i = 1; i < this.ComponentCount; i++)
			{
				this.ChangeVersions[i * this.Capacity + chunkIndex] = version;
			}
		}

		// Token: 0x06001403 RID: 5123 RVA: 0x0004D4C4 File Offset: 0x0004B6C4
		public void SetOrderVersion(int chunkIndex, uint changeVersion)
		{
			this.SetChangeVersion(0, chunkIndex, changeVersion);
		}

		// Token: 0x06001404 RID: 5124 RVA: 0x0004D4CF File Offset: 0x0004B6CF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe int* GetChunkEntityCountArray()
		{
			return this.EntityCount;
		}

		// Token: 0x06001405 RID: 5125 RVA: 0x0004D4D7 File Offset: 0x0004B6D7
		public unsafe int GetChunkEntityCount(int chunkIndex)
		{
			return this.EntityCount[chunkIndex];
		}

		// Token: 0x06001406 RID: 5126 RVA: 0x0004D4E5 File Offset: 0x0004B6E5
		public unsafe void SetChunkEntityCount(int chunkIndex, int count)
		{
			this.EntityCount[chunkIndex] = count;
		}

		// Token: 0x06001407 RID: 5127 RVA: 0x0004D4F4 File Offset: 0x0004B6F4
		public unsafe void Add(ChunkIndex chunk, SharedComponentValues sharedComponentIndices, uint changeVersion)
		{
			int count = this.Count;
			this.Count = count + 1;
			int num = count;
			this.ChunkIndices[num] = chunk;
			for (int i = 0; i < this.SharedComponentCount; i++)
			{
				this.SharedComponentValues[i * this.Capacity + num] = *sharedComponentIndices[i];
			}
			for (int j = 0; j < this.ComponentCount; j++)
			{
				this.ChangeVersions[j * this.Capacity + num] = changeVersion;
			}
			this.EntityCount[num] = chunk.Count;
		}

		// Token: 0x06001408 RID: 5128 RVA: 0x0004D594 File Offset: 0x0004B794
		public unsafe void MoveChunks(in ArchetypeChunkData srcChunks)
		{
			if (this.Capacity < this.Count + srcChunks.Count)
			{
				this.Grow(this.Count + srcChunks.Count);
			}
			void* ptr = (void*)(this.ChunkIndices + this.Count);
			ArchetypeChunkData archetypeChunkData = srcChunks;
			UnsafeUtility.MemCpy(ptr, (void*)archetypeChunkData.ChunkIndices, (long)(sizeof(ChunkIndex) * srcChunks.Count));
			this.Count += srcChunks.Count;
		}

		// Token: 0x06001409 RID: 5129 RVA: 0x0004D612 File Offset: 0x0004B812
		public void AddToCachedChunkList(ref UnsafeCachedChunkList chunkList, int matchingArchetypeIndex, int startIndex = 0)
		{
			chunkList.Append(this.ChunkIndices + startIndex, this.Count - startIndex, matchingArchetypeIndex);
		}

		// Token: 0x0600140A RID: 5130 RVA: 0x0004D634 File Offset: 0x0004B834
		public unsafe void RemoveAtSwapBack(int chunkIndex)
		{
			int count = this.Count;
			this.Count = count - 1;
			if (chunkIndex == this.Count)
			{
				return;
			}
			this.ChunkIndices[chunkIndex] = this.ChunkIndices[this.Count];
			for (int i = 0; i < this.SharedComponentCount; i++)
			{
				this.SharedComponentValues[i * this.Capacity + chunkIndex] = this.SharedComponentValues[i * this.Capacity + this.Count];
			}
			for (int j = 0; j < this.ComponentCount; j++)
			{
				this.ChangeVersions[j * this.Capacity + chunkIndex] = this.ChangeVersions[j * this.Capacity + this.Count];
			}
			v128* componentEnabledMaskArrayForChunk = this.GetComponentEnabledMaskArrayForChunk(this.Count);
			UnsafeUtility.MemCpy((void*)this.GetComponentEnabledMaskArrayForChunk(chunkIndex), (void*)componentEnabledMaskArrayForChunk, (long)this.ComponentEnabledBitsSizeTotalPerChunk);
			int* chunkDisabledCounts = this.GetChunkDisabledCounts(this.Count);
			UnsafeUtility.MemCpy((void*)this.GetChunkDisabledCounts(chunkIndex), (void*)chunkDisabledCounts, (long)this.ComponentEnabledBitsHierarchicalDataSizePerChunk);
			this.EntityCount[chunkIndex] = this.EntityCount[this.Count];
		}

		// Token: 0x0600140B RID: 5131 RVA: 0x0004D76D File Offset: 0x0004B96D
		public void Dispose()
		{
			Memory.Unmanaged.Free(this.m_Data, Allocator.Persistent);
			this.m_Data = null;
			this.Capacity = 0;
			this.Count = 0;
		}

		// Token: 0x04000752 RID: 1874
		private unsafe void* m_Data;

		// Token: 0x04000755 RID: 1877
		private readonly int SharedComponentCount;

		// Token: 0x04000756 RID: 1878
		private readonly int ComponentCount;

		// Token: 0x04000757 RID: 1879
		private const ulong ComponentEnabledBitsSizePerComponentInChunk = 16UL;
	}
}
