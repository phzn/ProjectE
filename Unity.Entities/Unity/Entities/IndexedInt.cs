using System;

namespace Unity.Entities
{
	// Token: 0x0200028E RID: 654
	internal struct IndexedInt : IComparable<IndexedInt>
	{
		// Token: 0x0600115E RID: 4446 RVA: 0x00049562 File Offset: 0x00047762
		public int CompareTo(IndexedInt other)
		{
			return this.Value.CompareTo(other.Value);
		}

		// Token: 0x040006A1 RID: 1697
		public int Value;

		// Token: 0x040006A2 RID: 1698
		public int Index;
	}
}
