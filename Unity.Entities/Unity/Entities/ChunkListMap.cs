using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Unity.Entities
{
	// Token: 0x02000033 RID: 51
	internal struct ChunkListMap : IDisposable
	{
		// Token: 0x06000119 RID: 281 RVA: 0x000065EC File Offset: 0x000047EC
		private unsafe static uint GetHashCode(SharedComponentValues sharedComponentValues, int numSharedComponents)
		{
			uint num;
			if (sharedComponentValues.stride == 4)
			{
				num = math.hash((void*)sharedComponentValues.firstIndex, numSharedComponents * 4, 0U);
			}
			else
			{
				int* ptr;
				checked
				{
					ptr = stackalloc int[unchecked((UIntPtr)numSharedComponents) * 4];
				}
				for (int i = 0; i < numSharedComponents; i++)
				{
					ptr[i] = *sharedComponentValues[i];
				}
				num = math.hash((void*)ptr, numSharedComponents * 4, 0U);
			}
			if (num == 0U || num == 4294967295U)
			{
				num = 1U;
			}
			return num;
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600011A RID: 282 RVA: 0x0000664C File Offset: 0x0000484C
		public int Size
		{
			get
			{
				return this.hashes.Length;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600011B RID: 283 RVA: 0x00006659 File Offset: 0x00004859
		public int UnoccupiedNodes
		{
			get
			{
				return this.emptyNodes + this.skipNodes;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600011C RID: 284 RVA: 0x00006668 File Offset: 0x00004868
		public int OccupiedNodes
		{
			get
			{
				return this.Size - this.UnoccupiedNodes;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600011D RID: 285 RVA: 0x00006677 File Offset: 0x00004877
		public bool IsEmpty
		{
			get
			{
				return this.OccupiedNodes == 0;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600011E RID: 286 RVA: 0x00006682 File Offset: 0x00004882
		private int hashMask
		{
			get
			{
				return this.Size - 1;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600011F RID: 287 RVA: 0x0000668C File Offset: 0x0000488C
		public int MinimumSize
		{
			get
			{
				return 16;
			}
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00006690 File Offset: 0x00004890
		public void SetCapacity(int capacity)
		{
			if (capacity < this.MinimumSize)
			{
				capacity = this.MinimumSize;
			}
			this.chunks.SetCapacity(capacity);
			this.hashes.SetCapacity(capacity);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x000066BC File Offset: 0x000048BC
		public unsafe void Init(Archetype* archetype, int count)
		{
			this.archetype = archetype;
			if (count < this.MinimumSize)
			{
				count = this.MinimumSize;
			}
			if (this.hashes.IsCreated)
			{
				this.hashes.Clear();
			}
			else
			{
				this.hashes = new UnsafeList<uint>(count, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			}
			this.hashes.Resize(count, NativeArrayOptions.ClearMemory);
			if (this.chunks.IsCreated)
			{
				this.chunks.Clear();
			}
			else
			{
				this.chunks = new UnsafeList<ChunkIndex>(count, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			}
			this.chunks.Resize(count, NativeArrayOptions.ClearMemory);
			this.emptyNodes = count;
			this.skipNodes = 0;
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00006764 File Offset: 0x00004964
		public unsafe void AppendFrom(ref ChunkListMap src)
		{
			for (int i = 0; i < src.Size; i++)
			{
				uint num = src.hashes.Ptr[i];
				if (num != 0U && num != 4294967295U)
				{
					this.Add(src.chunks.Ptr[i]);
				}
			}
		}

		// Token: 0x06000123 RID: 291 RVA: 0x000067BC File Offset: 0x000049BC
		public unsafe ChunkIndex TryGet(SharedComponentValues sharedComponentValues, int numSharedComponents)
		{
			uint hashCode = ChunkListMap.GetHashCode(sharedComponentValues, numSharedComponents);
			int num = (int)(hashCode & (uint)this.hashMask);
			int num2 = 0;
			for (;;)
			{
				uint num3 = this.hashes.Ptr[num];
				if (num3 == 0U)
				{
					break;
				}
				if (num3 == hashCode)
				{
					ChunkIndex chunkIndex = this.chunks.Ptr[num];
					SharedComponentValues sharedComponentValues2 = this.archetype->Chunks.GetSharedComponentValues(chunkIndex.ListIndex);
					if (sharedComponentValues.EqualTo(sharedComponentValues2, numSharedComponents))
					{
						return chunkIndex;
					}
				}
				num = (num + 1) & this.hashMask;
				num2++;
				if (num2 == this.Size)
				{
					goto Block_4;
				}
			}
			return ChunkIndex.Null;
			Block_4:
			return ChunkIndex.Null;
		}

		// Token: 0x06000124 RID: 292 RVA: 0x0000685C File Offset: 0x00004A5C
		public void Resize(int size)
		{
			if (size < this.MinimumSize)
			{
				size = this.MinimumSize;
			}
			if (size == this.Size)
			{
				return;
			}
			ChunkListMap chunkListMap = this;
			this = default(ChunkListMap);
			this.Init(chunkListMap.archetype, size);
			this.AppendFrom(ref chunkListMap);
			chunkListMap.Dispose();
		}

		// Token: 0x06000125 RID: 293 RVA: 0x000068AE File Offset: 0x00004AAE
		public void PossiblyGrow()
		{
			if (this.UnoccupiedNodes < this.Size / 3)
			{
				this.Resize(this.Size * 2);
			}
		}

		// Token: 0x06000126 RID: 294 RVA: 0x000068CE File Offset: 0x00004ACE
		public void PossiblyShrink()
		{
			if (this.OccupiedNodes < this.Size / 3)
			{
				this.Resize(this.Size / 2);
			}
		}

		// Token: 0x06000127 RID: 295 RVA: 0x000068F0 File Offset: 0x00004AF0
		public unsafe void Add(ChunkIndex chunk)
		{
			SharedComponentValues sharedComponentValues = this.archetype->Chunks.GetSharedComponentValues(chunk.ListIndex);
			int numSharedComponents = this.archetype->NumSharedComponents;
			uint hashCode = ChunkListMap.GetHashCode(sharedComponentValues, numSharedComponents);
			int num = (int)(hashCode & (uint)this.hashMask);
			int num2 = 0;
			for (;;)
			{
				uint num3 = this.hashes.Ptr[num];
				if (num3 == 0U)
				{
					break;
				}
				if (num3 == 4294967295U)
				{
					goto Block_2;
				}
				num = (num + 1) & this.hashMask;
				num2++;
			}
			this.hashes.Ptr[num] = hashCode;
			this.chunks.Ptr[num] = chunk;
			chunk.ListWithEmptySlotsIndex = num;
			this.emptyNodes--;
			this.PossiblyGrow();
			return;
			Block_2:
			this.hashes.Ptr[num] = hashCode;
			this.chunks.Ptr[num] = chunk;
			chunk.ListWithEmptySlotsIndex = num;
			this.skipNodes--;
			this.PossiblyGrow();
		}

		// Token: 0x06000128 RID: 296 RVA: 0x000069F8 File Offset: 0x00004BF8
		public unsafe void Remove(ChunkIndex chunk)
		{
			int listWithEmptySlotsIndex = chunk.ListWithEmptySlotsIndex;
			chunk.ListWithEmptySlotsIndex = -1;
			this.hashes.Ptr[listWithEmptySlotsIndex] = uint.MaxValue;
			this.skipNodes++;
			this.PossiblyShrink();
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00006A3C File Offset: 0x00004C3C
		public unsafe bool Contains(ChunkIndex chunk)
		{
			int listWithEmptySlotsIndex = chunk.ListWithEmptySlotsIndex;
			return listWithEmptySlotsIndex != -1 && this.chunks.Ptr[listWithEmptySlotsIndex] == chunk;
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00006A77 File Offset: 0x00004C77
		public void Dispose()
		{
			this.hashes.Dispose();
			this.chunks.Dispose();
			this.emptyNodes = 0;
			this.skipNodes = 0;
		}

		// Token: 0x04000047 RID: 71
		private const uint kAValidHashCode = 1U;

		// Token: 0x04000048 RID: 72
		private const uint kSkipCode = 4294967295U;

		// Token: 0x04000049 RID: 73
		private UnsafeList<uint> hashes;

		// Token: 0x0400004A RID: 74
		private UnsafeList<ChunkIndex> chunks;

		// Token: 0x0400004B RID: 75
		private unsafe Archetype* archetype;

		// Token: 0x0400004C RID: 76
		private int emptyNodes;

		// Token: 0x0400004D RID: 77
		private int skipNodes;
	}
}
