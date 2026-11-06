using System;

namespace System.Collections.Generic
{
	// Token: 0x02000AC3 RID: 2755
	[Serializable]
	internal class ObjectComparer<T> : Comparer<T>
	{
		// Token: 0x06006282 RID: 25218 RVA: 0x00149CFA File Offset: 0x00147EFA
		public override int Compare(T x, T y)
		{
			return Comparer.Default.Compare(x, y);
		}

		// Token: 0x06006283 RID: 25219 RVA: 0x00149D12 File Offset: 0x00147F12
		public override bool Equals(object obj)
		{
			return obj is ObjectComparer<T>;
		}

		// Token: 0x06006284 RID: 25220 RVA: 0x00149C92 File Offset: 0x00147E92
		public override int GetHashCode()
		{
			return base.GetType().Name.GetHashCode();
		}
	}
}
