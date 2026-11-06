using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Unity.Entities
{
	// Token: 0x020002F2 RID: 754
	[StructLayout(LayoutKind.Explicit)]
	internal struct Chunk
	{
		// Token: 0x04000771 RID: 1905
		[FieldOffset(0)]
		public int ArchetypeIndexForSerialization;

		// Token: 0x04000772 RID: 1906
		[FieldOffset(4)]
		public int CountForSerialization;

		// Token: 0x04000773 RID: 1907
		[FieldOffset(8)]
		public Entity metaChunkEntity;

		// Token: 0x04000774 RID: 1908
		[FieldOffset(16)]
		public int ListWithEmptySlotsIndex;

		// Token: 0x04000775 RID: 1909
		[FieldOffset(20)]
		public uint Flags;

		// Token: 0x04000776 RID: 1910
		[FieldOffset(24)]
		public ulong SequenceNumber;

		// Token: 0x04000777 RID: 1911
		public const int kSerializedHeaderSize = 24;

		// Token: 0x04000778 RID: 1912
		public const int kBufferOffset = 64;

		// Token: 0x04000779 RID: 1913
		[FixedBuffer(typeof(byte), 4)]
		[FieldOffset(64)]
		public Chunk.<Buffer>e__FixedBuffer Buffer;

		// Token: 0x0400077A RID: 1914
		public const int kChunkSize = 16384;

		// Token: 0x0400077B RID: 1915
		public const int kBufferSize = 16320;

		// Token: 0x0400077C RID: 1916
		public const int kMaximumEntitiesPerChunk = 128;

		// Token: 0x0400077D RID: 1917
		public const int kChunkBufferSize = 16320;

		// Token: 0x020002F3 RID: 755
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(LayoutKind.Sequential, Size = 4)]
		public struct <Buffer>e__FixedBuffer
		{
			// Token: 0x0400077E RID: 1918
			public byte FixedElementField;
		}
	}
}
