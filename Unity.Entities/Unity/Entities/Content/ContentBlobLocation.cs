using System;
using Unity.Entities.Serialization;

namespace Unity.Entities.Content
{
	// Token: 0x02000484 RID: 1156
	[Serializable]
	internal struct ContentBlobLocation
	{
		// Token: 0x04000A12 RID: 2578
		public UnsafeUntypedWeakReferenceId ObjectId;

		// Token: 0x04000A13 RID: 2579
		public int FileIndex;

		// Token: 0x04000A14 RID: 2580
		public long Offset;

		// Token: 0x04000A15 RID: 2581
		public long Length;
	}
}
