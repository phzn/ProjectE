using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Linq.Parallel
{
	// Token: 0x020001AB RID: 427
	internal class OrderedGroupByGrouping<TGroupKey, TOrderKey, TElement> : IGrouping<TGroupKey, TElement>, IEnumerable<TElement>, IEnumerable
	{
		// Token: 0x06000B16 RID: 2838 RVA: 0x00026F57 File Offset: 0x00025157
		internal OrderedGroupByGrouping(TGroupKey groupKey, IComparer<TOrderKey> orderComparer)
		{
			this._groupKey = groupKey;
			this._values = new GrowingArray<TElement>();
			this._orderKeys = new GrowingArray<TOrderKey>();
			this._orderComparer = orderComparer;
			this._wrappedComparer = new OrderedGroupByGrouping<TGroupKey, TOrderKey, TElement>.KeyAndValuesComparer(this._orderComparer);
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000B17 RID: 2839 RVA: 0x00026F94 File Offset: 0x00025194
		TGroupKey IGrouping<TGroupKey, TElement>.Key
		{
			get
			{
				return this._groupKey;
			}
		}

		// Token: 0x06000B18 RID: 2840 RVA: 0x00026F9C File Offset: 0x0002519C
		IEnumerator<TElement> IEnumerable<TElement>.GetEnumerator()
		{
			int valueCount = this._values.Count;
			TElement[] valueArray = this._values.InternalArray;
			int num;
			for (int i = 0; i < valueCount; i = num + 1)
			{
				yield return valueArray[i];
				num = i;
			}
			yield break;
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x00026FAB File Offset: 0x000251AB
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<TElement>)this).GetEnumerator();
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x00026FB3 File Offset: 0x000251B3
		internal void Add(TElement value, TOrderKey orderKey)
		{
			this._values.Add(value);
			this._orderKeys.Add(orderKey);
		}

		// Token: 0x06000B1B RID: 2843 RVA: 0x00026FD0 File Offset: 0x000251D0
		internal void DoneAdding()
		{
			List<KeyValuePair<TOrderKey, TElement>> list = new List<KeyValuePair<TOrderKey, TElement>>();
			for (int i = 0; i < this._orderKeys.InternalArray.Length; i++)
			{
				list.Add(new KeyValuePair<TOrderKey, TElement>(this._orderKeys.InternalArray[i], this._values.InternalArray[i]));
			}
			list.Sort(0, this._values.Count, this._wrappedComparer);
			for (int j = 0; j < this._values.InternalArray.Length; j++)
			{
				this._orderKeys.InternalArray[j] = list[j].Key;
				this._values.InternalArray[j] = list[j].Value;
			}
		}

		// Token: 0x040007AC RID: 1964
		private TGroupKey _groupKey;

		// Token: 0x040007AD RID: 1965
		private GrowingArray<TElement> _values;

		// Token: 0x040007AE RID: 1966
		private GrowingArray<TOrderKey> _orderKeys;

		// Token: 0x040007AF RID: 1967
		private IComparer<TOrderKey> _orderComparer;

		// Token: 0x040007B0 RID: 1968
		private OrderedGroupByGrouping<TGroupKey, TOrderKey, TElement>.KeyAndValuesComparer _wrappedComparer;

		// Token: 0x020001AC RID: 428
		private class KeyAndValuesComparer : IComparer<KeyValuePair<TOrderKey, TElement>>
		{
			// Token: 0x06000B1C RID: 2844 RVA: 0x00027098 File Offset: 0x00025298
			public KeyAndValuesComparer(IComparer<TOrderKey> comparer)
			{
				this.myComparer = comparer;
			}

			// Token: 0x06000B1D RID: 2845 RVA: 0x000270A7 File Offset: 0x000252A7
			public int Compare(KeyValuePair<TOrderKey, TElement> x, KeyValuePair<TOrderKey, TElement> y)
			{
				return this.myComparer.Compare(x.Key, y.Key);
			}

			// Token: 0x040007B1 RID: 1969
			private IComparer<TOrderKey> myComparer;
		}
	}
}
