using System;
using System.Collections.Concurrent;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020006BC RID: 1724
	internal sealed class NameCache
	{
		// Token: 0x06003FC0 RID: 16320 RVA: 0x000DF8CC File Offset: 0x000DDACC
		internal object GetCachedValue(string name)
		{
			this.name = name;
			object obj;
			if (!NameCache.ht.TryGetValue(name, out obj))
			{
				return null;
			}
			return obj;
		}

		// Token: 0x06003FC1 RID: 16321 RVA: 0x000DF8F2 File Offset: 0x000DDAF2
		internal void SetCachedValue(object value)
		{
			NameCache.ht[this.name] = value;
		}

		// Token: 0x040029B1 RID: 10673
		private static ConcurrentDictionary<string, object> ht = new ConcurrentDictionary<string, object>();

		// Token: 0x040029B2 RID: 10674
		private string name;
	}
}
