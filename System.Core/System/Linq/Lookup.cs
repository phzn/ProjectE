using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity;

namespace System.Linq
{
	/// <summary>Represents a collection of keys each mapped to one or more values.</summary>
	/// <typeparam name="TKey">The type of the keys in the <see cref="T:System.Linq.Lookup`2" />.</typeparam>
	/// <typeparam name="TElement">The type of the elements of each <see cref="T:System.Collections.Generic.IEnumerable`1" /> value in the <see cref="T:System.Linq.Lookup`2" />.</typeparam>
	// Token: 0x020000D8 RID: 216
	[DebuggerDisplay("Count = {Count}")]
	[DebuggerTypeProxy(typeof(SystemLinq_LookupDebugView<, >))]
	public class Lookup<TKey, TElement> : ILookup<TKey, TElement>, IEnumerable<IGrouping<TKey, TElement>>, IEnumerable, IIListProvider<IGrouping<TKey, TElement>>
	{
		// Token: 0x060007AB RID: 1963 RVA: 0x0001B020 File Offset: 0x00019220
		internal static Lookup<TKey, TElement> Create<TSource>(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
		{
			Lookup<TKey, TElement> lookup = new Lookup<TKey, TElement>(comparer);
			foreach (TSource tsource in source)
			{
				lookup.GetGrouping(keySelector(tsource), true).Add(elementSelector(tsource));
			}
			return lookup;
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x0001B084 File Offset: 0x00019284
		internal static Lookup<TKey, TElement> Create(IEnumerable<TElement> source, Func<TElement, TKey> keySelector, IEqualityComparer<TKey> comparer)
		{
			Lookup<TKey, TElement> lookup = new Lookup<TKey, TElement>(comparer);
			foreach (TElement telement in source)
			{
				lookup.GetGrouping(keySelector(telement), true).Add(telement);
			}
			return lookup;
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x0001B0E4 File Offset: 0x000192E4
		internal static Lookup<TKey, TElement> CreateForJoin(IEnumerable<TElement> source, Func<TElement, TKey> keySelector, IEqualityComparer<TKey> comparer)
		{
			Lookup<TKey, TElement> lookup = new Lookup<TKey, TElement>(comparer);
			foreach (TElement telement in source)
			{
				TKey tkey = keySelector(telement);
				if (tkey != null)
				{
					lookup.GetGrouping(tkey, true).Add(telement);
				}
			}
			return lookup;
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x0001B14C File Offset: 0x0001934C
		private Lookup(IEqualityComparer<TKey> comparer)
		{
			this._comparer = comparer ?? EqualityComparer<TKey>.Default;
			this._groupings = new Grouping<TKey, TElement>[7];
		}

		/// <summary>Gets the number of key/value collection pairs in the <see cref="T:System.Linq.Lookup`2" />.</summary>
		/// <returns>The number of key/value collection pairs in the <see cref="T:System.Linq.Lookup`2" />.</returns>
		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060007AF RID: 1967 RVA: 0x0001B170 File Offset: 0x00019370
		public int Count
		{
			get
			{
				return this._count;
			}
		}

		/// <summary>Gets the collection of values indexed by the specified key.</summary>
		/// <param name="key">The key of the desired collection of values.</param>
		/// <returns>The collection of values indexed by the specified key.</returns>
		// Token: 0x170000E2 RID: 226
		public IEnumerable<TElement> this[TKey key]
		{
			get
			{
				Grouping<TKey, TElement> grouping = this.GetGrouping(key, false);
				if (grouping != null)
				{
					return grouping;
				}
				return Array.Empty<TElement>();
			}
		}

		/// <summary>Determines whether a specified key is in the <see cref="T:System.Linq.Lookup`2" />.</summary>
		/// <param name="key">The key to find in the <see cref="T:System.Linq.Lookup`2" />.</param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="key" /> is in the <see cref="T:System.Linq.Lookup`2" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x060007B1 RID: 1969 RVA: 0x0001B198 File Offset: 0x00019398
		public bool Contains(TKey key)
		{
			return this.GetGrouping(key, false) != null;
		}

		/// <summary>Returns a generic enumerator that iterates through the <see cref="T:System.Linq.Lookup`2" />.</summary>
		/// <returns>An enumerator for the <see cref="T:System.Linq.Lookup`2" />.</returns>
		// Token: 0x060007B2 RID: 1970 RVA: 0x0001B1A5 File Offset: 0x000193A5
		public IEnumerator<IGrouping<TKey, TElement>> GetEnumerator()
		{
			Grouping<TKey, TElement> g = this._lastGrouping;
			if (g != null)
			{
				do
				{
					g = g._next;
					yield return g;
				}
				while (g != this._lastGrouping);
			}
			yield break;
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x0001B1B4 File Offset: 0x000193B4
		IGrouping<TKey, TElement>[] IIListProvider<IGrouping<TKey, TElement>>.ToArray()
		{
			IGrouping<TKey, TElement>[] array = new IGrouping<TKey, TElement>[this._count];
			int num = 0;
			Grouping<TKey, TElement> grouping = this._lastGrouping;
			if (grouping != null)
			{
				do
				{
					grouping = grouping._next;
					array[num] = grouping;
					num++;
				}
				while (grouping != this._lastGrouping);
			}
			return array;
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x0001B1F4 File Offset: 0x000193F4
		internal TResult[] ToArray<TResult>(Func<TKey, IEnumerable<TElement>, TResult> resultSelector)
		{
			TResult[] array = new TResult[this._count];
			int num = 0;
			Grouping<TKey, TElement> grouping = this._lastGrouping;
			if (grouping != null)
			{
				do
				{
					grouping = grouping._next;
					grouping.Trim();
					array[num] = resultSelector(grouping._key, grouping._elements);
					num++;
				}
				while (grouping != this._lastGrouping);
			}
			return array;
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x0001B250 File Offset: 0x00019450
		List<IGrouping<TKey, TElement>> IIListProvider<IGrouping<TKey, TElement>>.ToList()
		{
			List<IGrouping<TKey, TElement>> list = new List<IGrouping<TKey, TElement>>(this._count);
			Grouping<TKey, TElement> grouping = this._lastGrouping;
			if (grouping != null)
			{
				do
				{
					grouping = grouping._next;
					list.Add(grouping);
				}
				while (grouping != this._lastGrouping);
			}
			return list;
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x0001B28C File Offset: 0x0001948C
		internal List<TResult> ToList<TResult>(Func<TKey, IEnumerable<TElement>, TResult> resultSelector)
		{
			List<TResult> list = new List<TResult>(this._count);
			Grouping<TKey, TElement> grouping = this._lastGrouping;
			if (grouping != null)
			{
				do
				{
					grouping = grouping._next;
					grouping.Trim();
					list.Add(resultSelector(grouping._key, grouping._elements));
				}
				while (grouping != this._lastGrouping);
			}
			return list;
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x0001B170 File Offset: 0x00019370
		int IIListProvider<IGrouping<TKey, TElement>>.GetCount(bool onlyIfCheap)
		{
			return this._count;
		}

		/// <summary>Applies a transform function to each key and its associated values and returns the results.</summary>
		/// <param name="resultSelector">A function to project a result value from each key and its associated values.</param>
		/// <typeparam name="TResult">The type of the result values produced by <paramref name="resultSelector" />.</typeparam>
		/// <returns>A collection that contains one value for each key/value collection pair in the <see cref="T:System.Linq.Lookup`2" />.</returns>
		// Token: 0x060007B8 RID: 1976 RVA: 0x0001B2DE File Offset: 0x000194DE
		public IEnumerable<TResult> ApplyResultSelector<TResult>(Func<TKey, IEnumerable<TElement>, TResult> resultSelector)
		{
			Grouping<TKey, TElement> g = this._lastGrouping;
			if (g != null)
			{
				do
				{
					g = g._next;
					g.Trim();
					yield return resultSelector(g._key, g._elements);
				}
				while (g != this._lastGrouping);
			}
			yield break;
		}

		/// <summary>Returns an enumerator that iterates through the <see cref="T:System.Linq.Lookup`2" />. This class cannot be inherited.</summary>
		/// <returns>An enumerator for the <see cref="T:System.Linq.Lookup`2" />.</returns>
		// Token: 0x060007B9 RID: 1977 RVA: 0x0001B2F5 File Offset: 0x000194F5
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x0001B2FD File Offset: 0x000194FD
		private int InternalGetHashCode(TKey key)
		{
			if (key != null)
			{
				return this._comparer.GetHashCode(key) & int.MaxValue;
			}
			return 0;
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x0001B31C File Offset: 0x0001951C
		internal Grouping<TKey, TElement> GetGrouping(TKey key, bool create)
		{
			int num = this.InternalGetHashCode(key);
			for (Grouping<TKey, TElement> grouping = this._groupings[num % this._groupings.Length]; grouping != null; grouping = grouping._hashNext)
			{
				if (grouping._hashCode == num && this._comparer.Equals(grouping._key, key))
				{
					return grouping;
				}
			}
			if (create)
			{
				if (this._count == this._groupings.Length)
				{
					this.Resize();
				}
				int num2 = num % this._groupings.Length;
				Grouping<TKey, TElement> grouping2 = new Grouping<TKey, TElement>();
				grouping2._key = key;
				grouping2._hashCode = num;
				grouping2._elements = new TElement[1];
				grouping2._hashNext = this._groupings[num2];
				this._groupings[num2] = grouping2;
				if (this._lastGrouping == null)
				{
					grouping2._next = grouping2;
				}
				else
				{
					grouping2._next = this._lastGrouping._next;
					this._lastGrouping._next = grouping2;
				}
				this._lastGrouping = grouping2;
				this._count++;
				return grouping2;
			}
			return null;
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x0001B414 File Offset: 0x00019614
		private void Resize()
		{
			int num = checked(this._count * 2 + 1);
			Grouping<TKey, TElement>[] array = new Grouping<TKey, TElement>[num];
			Grouping<TKey, TElement> grouping = this._lastGrouping;
			do
			{
				grouping = grouping._next;
				int num2 = grouping._hashCode % num;
				grouping._hashNext = array[num2];
				array[num2] = grouping;
			}
			while (grouping != this._lastGrouping);
			this._groupings = array;
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x0000235B File Offset: 0x0000055B
		internal Lookup()
		{
			global::Unity.ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x0400058C RID: 1420
		private readonly IEqualityComparer<TKey> _comparer;

		// Token: 0x0400058D RID: 1421
		private Grouping<TKey, TElement>[] _groupings;

		// Token: 0x0400058E RID: 1422
		private Grouping<TKey, TElement> _lastGrouping;

		// Token: 0x0400058F RID: 1423
		private int _count;
	}
}
