using System;

namespace System.Runtime.Collections
{
	// Token: 0x02000053 RID: 83
	internal abstract class ObjectCacheItem<T> where T : class
	{
		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600030D RID: 781
		public abstract T Value { get; }

		// Token: 0x0600030E RID: 782
		public abstract bool TryAddReference();

		// Token: 0x0600030F RID: 783
		public abstract void ReleaseReference();
	}
}
