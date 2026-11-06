using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace System.Linq
{
	// Token: 0x020000D1 RID: 209
	[DebuggerDisplay("Key = {Key}")]
	[DebuggerTypeProxy(typeof(SystemLinq_GroupingDebugView<, >))]
	internal class Grouping<TKey, TElement> : IGrouping<TKey, TElement>, IEnumerable<TElement>, IEnumerable, IList<TElement>, ICollection<TElement>
	{
		// Token: 0x06000778 RID: 1912 RVA: 0x00002162 File Offset: 0x00000362
		internal Grouping()
		{
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x0001AA98 File Offset: 0x00018C98
		internal void Add(TElement element)
		{
			if (this._elements.Length == this._count)
			{
				Array.Resize<TElement>(ref this._elements, checked(this._count * 2));
			}
			this._elements[this._count] = element;
			this._count++;
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x0001AAE8 File Offset: 0x00018CE8
		internal void Trim()
		{
			if (this._elements.Length != this._count)
			{
				Array.Resize<TElement>(ref this._elements, this._count);
			}
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x0001AB0B File Offset: 0x00018D0B
		public IEnumerator<TElement> GetEnumerator()
		{
			int num;
			for (int i = 0; i < this._count; i = num + 1)
			{
				yield return this._elements[i];
				num = i;
			}
			yield break;
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x0001AB1A File Offset: 0x00018D1A
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x0600077D RID: 1917 RVA: 0x0001AB22 File Offset: 0x00018D22
		public TKey Key
		{
			get
			{
				return this._key;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x0600077E RID: 1918 RVA: 0x0001AB2A File Offset: 0x00018D2A
		int ICollection<TElement>.Count
		{
			get
			{
				return this._count;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x0600077F RID: 1919 RVA: 0x00007E1D File Offset: 0x0000601D
		bool ICollection<TElement>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x0001585F File Offset: 0x00013A5F
		void ICollection<TElement>.Add(TElement item)
		{
			throw Error.NotSupported();
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x0001585F File Offset: 0x00013A5F
		void ICollection<TElement>.Clear()
		{
			throw Error.NotSupported();
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x0001AB32 File Offset: 0x00018D32
		bool ICollection<TElement>.Contains(TElement item)
		{
			return Array.IndexOf<TElement>(this._elements, item, 0, this._count) >= 0;
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x0001AB4D File Offset: 0x00018D4D
		void ICollection<TElement>.CopyTo(TElement[] array, int arrayIndex)
		{
			Array.Copy(this._elements, 0, array, arrayIndex, this._count);
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x0001585F File Offset: 0x00013A5F
		bool ICollection<TElement>.Remove(TElement item)
		{
			throw Error.NotSupported();
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x0001AB63 File Offset: 0x00018D63
		int IList<TElement>.IndexOf(TElement item)
		{
			return Array.IndexOf<TElement>(this._elements, item, 0, this._count);
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x0001585F File Offset: 0x00013A5F
		void IList<TElement>.Insert(int index, TElement item)
		{
			throw Error.NotSupported();
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x0001585F File Offset: 0x00013A5F
		void IList<TElement>.RemoveAt(int index)
		{
			throw Error.NotSupported();
		}

		// Token: 0x170000DC RID: 220
		TElement IList<TElement>.this[int index]
		{
			get
			{
				if (index < 0 || index >= this._count)
				{
					throw Error.ArgumentOutOfRange("index");
				}
				return this._elements[index];
			}
			set
			{
				throw Error.NotSupported();
			}
		}

		// Token: 0x04000572 RID: 1394
		internal TKey _key;

		// Token: 0x04000573 RID: 1395
		internal int _hashCode;

		// Token: 0x04000574 RID: 1396
		internal TElement[] _elements;

		// Token: 0x04000575 RID: 1397
		internal int _count;

		// Token: 0x04000576 RID: 1398
		internal Grouping<TKey, TElement> _hashNext;

		// Token: 0x04000577 RID: 1399
		internal Grouping<TKey, TElement> _next;
	}
}
