using System;
using System.Runtime.InteropServices;
using Unity.Collections;

namespace Unity.Entities
{
	// Token: 0x02000022 RID: 34
	[StructLayout(LayoutKind.Explicit, Size = 32)]
	internal struct BlobAssetHeader
	{
		// Token: 0x06000078 RID: 120 RVA: 0x00002F1C File Offset: 0x0000111C
		internal static BlobAssetHeader CreateForSerialize(int length, ulong hash)
		{
			return new BlobAssetHeader
			{
				ValidationPtr = null,
				Length = length,
				Allocator = Unity.Collections.Allocator.None,
				Hash = hash,
				Padding = 0UL
			};
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00002F61 File Offset: 0x00001161
		public void Invalidate()
		{
			this.ValidationPtr = -2459565876494606883L;
		}

		// Token: 0x0400002C RID: 44
		[FieldOffset(0)]
		public unsafe void* ValidationPtr;

		// Token: 0x0400002D RID: 45
		[FieldOffset(8)]
		public int Length;

		// Token: 0x0400002E RID: 46
		[FieldOffset(12)]
		public AllocatorManager.AllocatorHandle Allocator;

		// Token: 0x0400002F RID: 47
		[FieldOffset(16)]
		public ulong Hash;

		// Token: 0x04000030 RID: 48
		[FieldOffset(24)]
		private ulong Padding;
	}
}
