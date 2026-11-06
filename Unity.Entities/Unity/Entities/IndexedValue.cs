using System;

namespace Unity.Entities
{
	// Token: 0x0200028D RID: 653
	internal struct IndexedValue<T> : IComparable<IndexedValue<T>> where T : struct, IComparable<T>
	{
		// Token: 0x0600115D RID: 4445 RVA: 0x00049549 File Offset: 0x00047749
		public int CompareTo(IndexedValue<T> other)
		{
			return this.Value.CompareTo(other.Value);
		}

		// Token: 0x0400069F RID: 1695
		public T Value;

		// Token: 0x040006A0 RID: 1696
		public int Index;
	}
}
