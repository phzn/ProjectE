using System;

namespace Unity.Collections.LowLevel.Unsafe
{
	// Token: 0x02000122 RID: 290
	[GenerateTestsForBurstCompatibility]
	public struct UnsafeParallelHashMapBucketData
	{
		// Token: 0x06000C3F RID: 3135 RVA: 0x00024826 File Offset: 0x00022A26
		internal unsafe UnsafeParallelHashMapBucketData(byte* v, byte* k, byte* n, byte* b, int bcm)
		{
			this.values = v;
			this.keys = k;
			this.next = n;
			this.buckets = b;
			this.bucketCapacityMask = bcm;
		}

		// Token: 0x040004E7 RID: 1255
		public unsafe readonly byte* values;

		// Token: 0x040004E8 RID: 1256
		public unsafe readonly byte* keys;

		// Token: 0x040004E9 RID: 1257
		public unsafe readonly byte* next;

		// Token: 0x040004EA RID: 1258
		public unsafe readonly byte* buckets;

		// Token: 0x040004EB RID: 1259
		public readonly int bucketCapacityMask;
	}
}
