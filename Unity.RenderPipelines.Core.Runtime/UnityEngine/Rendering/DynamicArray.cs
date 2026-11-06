using System;
using System.Diagnostics;

namespace UnityEngine.Rendering
{
	// Token: 0x0200003F RID: 63
	[DebuggerDisplay("Size = {size} Capacity = {capacity}")]
	public class DynamicArray<T> where T : new()
	{
		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600023D RID: 573 RVA: 0x0000B2A6 File Offset: 0x000094A6
		// (set) Token: 0x0600023E RID: 574 RVA: 0x0000B2AE File Offset: 0x000094AE
		public int size { get; private set; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600023F RID: 575 RVA: 0x0000B2B7 File Offset: 0x000094B7
		public int capacity
		{
			get
			{
				return this.m_Array.Length;
			}
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000B2C1 File Offset: 0x000094C1
		public DynamicArray()
		{
			this.m_Array = new T[32];
			this.size = 0;
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000B2DD File Offset: 0x000094DD
		public DynamicArray(int size)
		{
			this.m_Array = new T[size];
			this.size = size;
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000B2F8 File Offset: 0x000094F8
		public void Clear()
		{
			this.size = 0;
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000B301 File Offset: 0x00009501
		public bool Contains(T item)
		{
			return this.IndexOf(item) != -1;
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000B310 File Offset: 0x00009510
		public int Add(in T value)
		{
			int size = this.size;
			if (size >= this.m_Array.Length)
			{
				T[] array = new T[this.m_Array.Length * 2];
				Array.Copy(this.m_Array, array, this.m_Array.Length);
				this.m_Array = array;
			}
			this.m_Array[size] = value;
			int size2 = this.size;
			this.size = size2 + 1;
			this.BumpVersion();
			return size;
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000B384 File Offset: 0x00009584
		public unsafe void AddRange(DynamicArray<T> array)
		{
			this.Reserve(this.size + array.size, true);
			for (int i = 0; i < array.size; i++)
			{
				T[] array2 = this.m_Array;
				int size = this.size;
				this.size = size + 1;
				array2[size] = *array[i];
			}
			this.BumpVersion();
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000B3E4 File Offset: 0x000095E4
		public bool Remove(T item)
		{
			int num = this.IndexOf(item);
			if (num != -1)
			{
				this.RemoveAt(num);
				return true;
			}
			return false;
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0000B408 File Offset: 0x00009608
		public void RemoveAt(int index)
		{
			if (index < 0 || index >= this.size)
			{
				throw new IndexOutOfRangeException();
			}
			if (index != this.size - 1)
			{
				Array.Copy(this.m_Array, index + 1, this.m_Array, index, this.size - index - 1);
			}
			int size = this.size;
			this.size = size - 1;
			this.BumpVersion();
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000B468 File Offset: 0x00009668
		public void RemoveRange(int index, int count)
		{
			if (count == 0)
			{
				return;
			}
			if (index < 0 || index >= this.size || count < 0 || index + count > this.size)
			{
				throw new ArgumentOutOfRangeException();
			}
			Array.Copy(this.m_Array, index + count, this.m_Array, index, this.size - index - count);
			this.size -= count;
			this.BumpVersion();
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000B4D0 File Offset: 0x000096D0
		public int FindIndex(int startIndex, int count, Predicate<T> match)
		{
			for (int i = startIndex; i < this.size; i++)
			{
				if (match(this.m_Array[i]))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000B508 File Offset: 0x00009708
		public int IndexOf(T item, int index, int count)
		{
			int num = index;
			while (num < this.size && count > 0)
			{
				if (this.m_Array[num].Equals(item))
				{
					return num;
				}
				num++;
				count--;
			}
			return -1;
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000B554 File Offset: 0x00009754
		public int IndexOf(T item, int index)
		{
			for (int i = index; i < this.size; i++)
			{
				if (this.m_Array[i].Equals(item))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000B596 File Offset: 0x00009796
		public int IndexOf(T item)
		{
			return this.IndexOf(item, 0);
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000B5A0 File Offset: 0x000097A0
		public void Resize(int newSize, bool keepContent = false)
		{
			this.Reserve(newSize, keepContent);
			this.size = newSize;
			this.BumpVersion();
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000B5B8 File Offset: 0x000097B8
		public void Reserve(int newCapacity, bool keepContent = false)
		{
			if (newCapacity > this.m_Array.Length)
			{
				if (keepContent)
				{
					T[] array = new T[newCapacity];
					Array.Copy(this.m_Array, array, this.m_Array.Length);
					this.m_Array = array;
					return;
				}
				this.m_Array = new T[newCapacity];
			}
		}

		// Token: 0x17000042 RID: 66
		public ref T this[int index]
		{
			get
			{
				return ref this.m_Array[index];
			}
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000B610 File Offset: 0x00009810
		public static implicit operator T[](DynamicArray<T> array)
		{
			return array.m_Array;
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000B618 File Offset: 0x00009818
		public DynamicArray<T>.Iterator GetEnumerator()
		{
			return new DynamicArray<T>.Iterator(this);
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000B620 File Offset: 0x00009820
		public DynamicArray<T>.RangeEnumerable SubRange(int first, int numItems)
		{
			return new DynamicArray<T>.RangeEnumerable
			{
				iterator = new DynamicArray<T>.RangeEnumerable.RangeIterator(this, first, numItems)
			};
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000B645 File Offset: 0x00009845
		internal void BumpVersion()
		{
		}

		// Token: 0x04000148 RID: 328
		private T[] m_Array;

		// Token: 0x02000159 RID: 345
		public struct Iterator
		{
			// Token: 0x060009DC RID: 2524 RVA: 0x0002C3EE File Offset: 0x0002A5EE
			public Iterator(DynamicArray<T> setOwner)
			{
				this.owner = setOwner;
				this.index = -1;
			}

			// Token: 0x1700014C RID: 332
			// (get) Token: 0x060009DD RID: 2525 RVA: 0x0002C3FE File Offset: 0x0002A5FE
			public ref T Current
			{
				get
				{
					return this.owner[this.index];
				}
			}

			// Token: 0x060009DE RID: 2526 RVA: 0x0002C411 File Offset: 0x0002A611
			public bool MoveNext()
			{
				this.index++;
				return this.index < this.owner.size;
			}

			// Token: 0x060009DF RID: 2527 RVA: 0x0002C434 File Offset: 0x0002A634
			public void Reset()
			{
				this.index = -1;
			}

			// Token: 0x040005DF RID: 1503
			private readonly DynamicArray<T> owner;

			// Token: 0x040005E0 RID: 1504
			private int index;
		}

		// Token: 0x0200015A RID: 346
		public struct RangeEnumerable
		{
			// Token: 0x060009E0 RID: 2528 RVA: 0x0002C43D File Offset: 0x0002A63D
			public DynamicArray<T>.RangeEnumerable.RangeIterator GetEnumerator()
			{
				return this.iterator;
			}

			// Token: 0x040005E1 RID: 1505
			public DynamicArray<T>.RangeEnumerable.RangeIterator iterator;

			// Token: 0x020001EB RID: 491
			public struct RangeIterator
			{
				// Token: 0x06000B95 RID: 2965 RVA: 0x0003048D File Offset: 0x0002E68D
				public RangeIterator(DynamicArray<T> setOwner, int first, int numItems)
				{
					this.owner = setOwner;
					this.first = first;
					this.index = first - 1;
					this.last = first + numItems;
				}

				// Token: 0x17000194 RID: 404
				// (get) Token: 0x06000B96 RID: 2966 RVA: 0x000304AF File Offset: 0x0002E6AF
				public ref T Current
				{
					get
					{
						return this.owner[this.index];
					}
				}

				// Token: 0x06000B97 RID: 2967 RVA: 0x000304C2 File Offset: 0x0002E6C2
				public bool MoveNext()
				{
					this.index++;
					return this.index < this.last;
				}

				// Token: 0x06000B98 RID: 2968 RVA: 0x000304E0 File Offset: 0x0002E6E0
				public void Reset()
				{
					this.index = this.first - 1;
				}

				// Token: 0x040007C0 RID: 1984
				private readonly DynamicArray<T> owner;

				// Token: 0x040007C1 RID: 1985
				private int index;

				// Token: 0x040007C2 RID: 1986
				private int first;

				// Token: 0x040007C3 RID: 1987
				private int last;
			}
		}
	}
}
