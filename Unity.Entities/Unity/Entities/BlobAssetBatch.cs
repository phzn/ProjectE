using System;
using System.Runtime.InteropServices;
using System.Threading;
using Unity.Collections;

namespace Unity.Entities
{
	// Token: 0x02000020 RID: 32
	[StructLayout(LayoutKind.Explicit, Size = 16)]
	internal struct BlobAssetBatch
	{
		// Token: 0x0600006E RID: 110 RVA: 0x00002E44 File Offset: 0x00001044
		internal static BlobAssetBatch CreateForSerialize(int blobAssetCount, int totalDataSize)
		{
			return new BlobAssetBatch
			{
				BlobAssetHeaderCount = blobAssetCount,
				TotalDataSize = totalDataSize,
				RefCount = 1,
				Padding = 0
			};
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00002E7C File Offset: 0x0000107C
		internal unsafe static BlobAssetBatch* CreateFromMemory(void* buffer, int expectedTotalDataSize)
		{
			return (BlobAssetBatch*)buffer;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00002E8C File Offset: 0x0000108C
		public unsafe static void Retain(BlobAssetBatch* batch)
		{
			Interlocked.Increment(ref batch->RefCount);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00002E9A File Offset: 0x0000109A
		public unsafe static void Release(BlobAssetBatch* batch)
		{
			if (Interlocked.Decrement(ref batch->RefCount) <= 0)
			{
				Memory.Unmanaged.Free<BlobAssetBatch>(batch, Allocator.Persistent);
			}
		}

		// Token: 0x04000027 RID: 39
		[FieldOffset(0)]
		private int TotalDataSize;

		// Token: 0x04000028 RID: 40
		[FieldOffset(4)]
		private int BlobAssetHeaderCount;

		// Token: 0x04000029 RID: 41
		[FieldOffset(8)]
		private int RefCount;

		// Token: 0x0400002A RID: 42
		[FieldOffset(12)]
		private int Padding;
	}
}
