using System;
using Unity.Entities.Serialization;

namespace Unity.Entities.Content
{
	// Token: 0x02000482 RID: 1154
	[Serializable]
	internal struct ContentObjectLocation
	{
		// Token: 0x04000A0C RID: 2572
		public UnsafeUntypedWeakReferenceId ObjectId;

		// Token: 0x04000A0D RID: 2573
		public int FileIndex;

		// Token: 0x04000A0E RID: 2574
		public long LocalIdentifierInFile;
	}
}
