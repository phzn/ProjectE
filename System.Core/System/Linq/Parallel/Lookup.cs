using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Linq.Parallel
{
	// Token: 0x020001F4 RID: 500
	internal class Lookup<TKey, TElement> : ILookup<TKey, TElement>, IEnumerable<IGrouping<TKey, TElement>>, IEnumerable
	{
		// Token: 0x06000C43 RID: 3139 RVA: 0x0002AF53 File Offset: 0x00029153
		internal Lookup(IEqualityComparer<TKey> comparer)
		{
			this._comparer = comparer;
			this._dict = new Dictionary<TKey, IGrouping<TKey, TElement>>(this._comparer);
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000C44 RID: 3140 RVA: 0x0002AF74 File Offset: 0x00029174
		public int Count
		{
			get
			{
				int num = this._dict.Count;
				if (this._defaultKeyGrouping != null)
				{
					num++;
				}
				return num;
			}
		}

		// Token: 0x17000173 RID: 371
		public IEnumerable<TElement> this[TKey key]
		{
			get
			{
				if (this._comparer.Equals(key, default(TKey)))
				{
					if (this._defaultKeyGrouping != null)
					{
						return this._defaultKeyGrouping;
					}
					return Enumerable.Empty<TElement>();
				}
				else
				{
					IGrouping<TKey, TElement> grouping;
					if (this._dict.TryGetValue(key, out grouping))
					{
						return grouping;
					}
					return Enumerable.Empty<TElement>();
				}
			}
		}

		// Token: 0x06000C46 RID: 3142 RVA: 0x0002AFEC File Offset: 0x000291EC
		public bool Contains(TKey key)
		{
			if (this._comparer.Equals(key, default(TKey)))
			{
				return this._defaultKeyGrouping != null;
			}
			return this._dict.ContainsKey(key);
		}

		// Token: 0x06000C47 RID: 3143 RVA: 0x0002B028 File Offset: 0x00029228
		internal void Add(IGrouping<TKey, TElement> grouping)
		{
			if (this._comparer.Equals(grouping.Key, default(TKey)))
			{
				this._defaultKeyGrouping = grouping;
				return;
			}
			this._dict.Add(grouping.Key, grouping);
		}

		// Token: 0x06000C48 RID: 3144 RVA: 0x0002B06B File Offset: 0x0002926B
		public IEnumerator<IGrouping<TKey, TElement>> GetEnumerator()
		{
			foreach (IGrouping<TKey, TElement> grouping in this._dict.Values)
			{
				yield return grouping;
			}
			IEnumerator<IGrouping<TKey, TElement>> enumerator = null;
			if (this._defaultKeyGrouping != null)
			{
				yield return this._defaultKeyGrouping;
			}
			yield break;
			yield break;
		}

		// Token: 0x06000C49 RID: 3145 RVA: 0x0002B07A File Offset: 0x0002927A
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IGrouping<TKey, TElement>>)this).GetEnumerator();
		}

		// Token: 0x040008AC RID: 2220
		private IDictionary<TKey, IGrouping<TKey, TElement>> _dict;

		// Token: 0x040008AD RID: 2221
		private IEqualityComparer<TKey> _comparer;

		// Token: 0x040008AE RID: 2222
		private IGrouping<TKey, TElement> _defaultKeyGrouping;
	}
}
