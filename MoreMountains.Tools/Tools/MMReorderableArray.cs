using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000131 RID: 305
	[Serializable]
	public abstract class MMReorderableArray<T> : ICloneable, IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
	{
		// Token: 0x060006D6 RID: 1750 RVA: 0x0002205D File Offset: 0x0002025D
		public MMReorderableArray()
			: this(0)
		{
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x00022066 File Offset: 0x00020266
		public MMReorderableArray(int length)
		{
			this.array = new List<T>(length);
		}

		// Token: 0x1700005A RID: 90
		public T this[int index]
		{
			get
			{
				return this.array[index];
			}
			set
			{
				this.array[index] = value;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060006DA RID: 1754 RVA: 0x000220A2 File Offset: 0x000202A2
		public int Length
		{
			get
			{
				return this.array.Count;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060006DB RID: 1755 RVA: 0x000220AF File Offset: 0x000202AF
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060006DC RID: 1756 RVA: 0x000220B2 File Offset: 0x000202B2
		public int Count
		{
			get
			{
				return this.array.Count;
			}
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x000220BF File Offset: 0x000202BF
		public object Clone()
		{
			return new List<T>(this.array);
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x000220CC File Offset: 0x000202CC
		public void CopyFrom(IEnumerable<T> value)
		{
			this.array.Clear();
			this.array.AddRange(value);
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x000220E5 File Offset: 0x000202E5
		public bool Contains(T value)
		{
			return this.array.Contains(value);
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x000220F3 File Offset: 0x000202F3
		public int IndexOf(T value)
		{
			return this.array.IndexOf(value);
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x00022101 File Offset: 0x00020301
		public void Insert(int index, T item)
		{
			this.array.Insert(index, item);
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x00022110 File Offset: 0x00020310
		public void RemoveAt(int index)
		{
			this.array.RemoveAt(index);
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x0002211E File Offset: 0x0002031E
		public void Add(T item)
		{
			this.array.Add(item);
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x0002212C File Offset: 0x0002032C
		public void Clear()
		{
			this.array.Clear();
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x00022139 File Offset: 0x00020339
		public void CopyTo(T[] array, int arrayIndex)
		{
			this.array.CopyTo(array, arrayIndex);
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x00022148 File Offset: 0x00020348
		public bool Remove(T item)
		{
			return this.array.Remove(item);
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x00022156 File Offset: 0x00020356
		public T[] ToArray()
		{
			return this.array.ToArray();
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x00022163 File Offset: 0x00020363
		public IEnumerator<T> GetEnumerator()
		{
			return this.array.GetEnumerator();
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x00022175 File Offset: 0x00020375
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.array.GetEnumerator();
		}

		// Token: 0x04000886 RID: 2182
		[SerializeField]
		private List<T> array = new List<T>();
	}
}
