using System;
using System.Collections;

namespace System.Xml.Serialization
{
	// Token: 0x02000279 RID: 633
	internal class TempAssemblyCache
	{
		// Token: 0x17000459 RID: 1113
		internal TempAssembly this[string ns, object o]
		{
			get
			{
				return (TempAssembly)this.cache[new TempAssemblyCacheKey(ns, o)];
			}
		}

		// Token: 0x06001800 RID: 6144 RVA: 0x0008D084 File Offset: 0x0008B284
		internal void Add(string ns, object o, TempAssembly assembly)
		{
			TempAssemblyCacheKey tempAssemblyCacheKey = new TempAssemblyCacheKey(ns, o);
			lock (this)
			{
				if (this.cache[tempAssemblyCacheKey] != assembly)
				{
					Hashtable hashtable = new Hashtable();
					foreach (object obj in this.cache.Keys)
					{
						hashtable.Add(obj, this.cache[obj]);
					}
					this.cache = hashtable;
					this.cache[tempAssemblyCacheKey] = assembly;
				}
			}
		}

		// Token: 0x0400189D RID: 6301
		private Hashtable cache = new Hashtable();
	}
}
