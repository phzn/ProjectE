using System;
using System.Diagnostics;

namespace System.Collections.Generic
{
	// Token: 0x02000AB7 RID: 2743
	internal sealed class DictionaryDebugView<K, V>
	{
		// Token: 0x0600621A RID: 25114 RVA: 0x0014808C File Offset: 0x0014628C
		public DictionaryDebugView(IDictionary<K, V> dictionary)
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException("dictionary");
			}
			this._dict = dictionary;
		}

		// Token: 0x17001172 RID: 4466
		// (get) Token: 0x0600621B RID: 25115 RVA: 0x001480AC File Offset: 0x001462AC
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public KeyValuePair<K, V>[] Items
		{
			get
			{
				KeyValuePair<K, V>[] array = new KeyValuePair<K, V>[this._dict.Count];
				this._dict.CopyTo(array, 0);
				return array;
			}
		}

		// Token: 0x04003A27 RID: 14887
		private readonly IDictionary<K, V> _dict;
	}
}
