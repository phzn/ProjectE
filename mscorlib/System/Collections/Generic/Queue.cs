using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System.Collections.Generic
{
	// Token: 0x02000AB0 RID: 2736
	[TypeForwardedFrom("System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
	[DebuggerDisplay("Count = {Count}")]
	[DebuggerTypeProxy(typeof(QueueDebugView<>))]
	[Serializable]
	public class Queue<T> : IEnumerable<T>, IEnumerable, ICollection, IReadOnlyCollection<T>
	{
		// Token: 0x060061D9 RID: 25049 RVA: 0x00147111 File Offset: 0x00145311
		public Queue()
		{
			this._array = Array.Empty<T>();
		}

		// Token: 0x060061DA RID: 25050 RVA: 0x00147124 File Offset: 0x00145324
		public Queue(int capacity)
		{
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException("capacity", capacity, "Non-negative number required.");
			}
			this._array = new T[capacity];
		}

		// Token: 0x060061DB RID: 25051 RVA: 0x00147154 File Offset: 0x00145354
		public Queue(IEnumerable<T> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			this._array = EnumerableHelpers.ToArray<T>(collection, out this._size);
			if (this._size != this._array.Length)
			{
				this._tail = this._size;
			}
		}

		// Token: 0x17001165 RID: 4453
		// (get) Token: 0x060061DC RID: 25052 RVA: 0x001471A3 File Offset: 0x001453A3
		public int Count
		{
			get
			{
				return this._size;
			}
		}

		// Token: 0x17001166 RID: 4454
		// (get) Token: 0x060061DD RID: 25053 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001167 RID: 4455
		// (get) Token: 0x060061DE RID: 25054 RVA: 0x001471AB File Offset: 0x001453AB
		object ICollection.SyncRoot
		{
			get
			{
				if (this._syncRoot == null)
				{
					Interlocked.CompareExchange<object>(ref this._syncRoot, new object(), null);
				}
				return this._syncRoot;
			}
		}

		// Token: 0x060061DF RID: 25055 RVA: 0x001471D0 File Offset: 0x001453D0
		public void Clear()
		{
			if (this._size != 0)
			{
				if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
				{
					if (this._head < this._tail)
					{
						Array.Clear(this._array, this._head, this._size);
					}
					else
					{
						Array.Clear(this._array, this._head, this._array.Length - this._head);
						Array.Clear(this._array, 0, this._tail);
					}
				}
				this._size = 0;
			}
			this._head = 0;
			this._tail = 0;
			this._version++;
		}

		// Token: 0x060061E0 RID: 25056 RVA: 0x00147268 File Offset: 0x00145468
		public void CopyTo(T[] array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (arrayIndex < 0 || arrayIndex > array.Length)
			{
				throw new ArgumentOutOfRangeException("arrayIndex", arrayIndex, "Index was out of range. Must be non-negative and less than the size of the collection.");
			}
			if (array.Length - arrayIndex < this._size)
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
			int num = this._size;
			if (num == 0)
			{
				return;
			}
			int num2 = Math.Min(this._array.Length - this._head, num);
			Array.Copy(this._array, this._head, array, arrayIndex, num2);
			num -= num2;
			if (num > 0)
			{
				Array.Copy(this._array, 0, array, arrayIndex + this._array.Length - this._head, num);
			}
		}

		// Token: 0x060061E1 RID: 25057 RVA: 0x00147318 File Offset: 0x00145518
		void ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank != 1)
			{
				throw new ArgumentException("Only single dimensional arrays are supported for the requested action.", "array");
			}
			if (array.GetLowerBound(0) != 0)
			{
				throw new ArgumentException("The lower bound of target array must be zero.", "array");
			}
			int length = array.Length;
			if (index < 0 || index > length)
			{
				throw new ArgumentOutOfRangeException("index", index, "Index was out of range. Must be non-negative and less than the size of the collection.");
			}
			if (length - index < this._size)
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
			int num = this._size;
			if (num == 0)
			{
				return;
			}
			try
			{
				int num2 = ((this._array.Length - this._head < num) ? (this._array.Length - this._head) : num);
				Array.Copy(this._array, this._head, array, index, num2);
				num -= num2;
				if (num > 0)
				{
					Array.Copy(this._array, 0, array, index + this._array.Length - this._head, num);
				}
			}
			catch (ArrayTypeMismatchException)
			{
				throw new ArgumentException("Target array type is not compatible with the type of items in the collection.", "array");
			}
		}

		// Token: 0x060061E2 RID: 25058 RVA: 0x00147430 File Offset: 0x00145630
		public void Enqueue(T item)
		{
			if (this._size == this._array.Length)
			{
				int num = (int)((long)this._array.Length * 200L / 100L);
				if (num < this._array.Length + 4)
				{
					num = this._array.Length + 4;
				}
				this.SetCapacity(num);
			}
			this._array[this._tail] = item;
			this.MoveNext(ref this._tail);
			this._size++;
			this._version++;
		}

		// Token: 0x060061E3 RID: 25059 RVA: 0x001474BC File Offset: 0x001456BC
		public Queue<T>.Enumerator GetEnumerator()
		{
			return new Queue<T>.Enumerator(this);
		}

		// Token: 0x060061E4 RID: 25060 RVA: 0x001474C4 File Offset: 0x001456C4
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return new Queue<T>.Enumerator(this);
		}

		// Token: 0x060061E5 RID: 25061 RVA: 0x001474C4 File Offset: 0x001456C4
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new Queue<T>.Enumerator(this);
		}

		// Token: 0x060061E6 RID: 25062 RVA: 0x001474D4 File Offset: 0x001456D4
		public T Dequeue()
		{
			int head = this._head;
			T[] array = this._array;
			if (this._size == 0)
			{
				this.ThrowForEmptyQueue();
			}
			T t = array[head];
			if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
			{
				array[head] = default(T);
			}
			this.MoveNext(ref this._head);
			this._size--;
			this._version++;
			return t;
		}

		// Token: 0x060061E7 RID: 25063 RVA: 0x00147544 File Offset: 0x00145744
		public bool TryDequeue(out T result)
		{
			int head = this._head;
			T[] array = this._array;
			if (this._size == 0)
			{
				result = default(T);
				return false;
			}
			result = array[head];
			if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
			{
				array[head] = default(T);
			}
			this.MoveNext(ref this._head);
			this._size--;
			this._version++;
			return true;
		}

		// Token: 0x060061E8 RID: 25064 RVA: 0x001475BD File Offset: 0x001457BD
		public T Peek()
		{
			if (this._size == 0)
			{
				this.ThrowForEmptyQueue();
			}
			return this._array[this._head];
		}

		// Token: 0x060061E9 RID: 25065 RVA: 0x001475DE File Offset: 0x001457DE
		public bool TryPeek(out T result)
		{
			if (this._size == 0)
			{
				result = default(T);
				return false;
			}
			result = this._array[this._head];
			return true;
		}

		// Token: 0x060061EA RID: 25066 RVA: 0x0014760C File Offset: 0x0014580C
		public bool Contains(T item)
		{
			if (this._size == 0)
			{
				return false;
			}
			if (this._head < this._tail)
			{
				return Array.IndexOf<T>(this._array, item, this._head, this._size) >= 0;
			}
			return Array.IndexOf<T>(this._array, item, this._head, this._array.Length - this._head) >= 0 || Array.IndexOf<T>(this._array, item, 0, this._tail) >= 0;
		}

		// Token: 0x060061EB RID: 25067 RVA: 0x00147690 File Offset: 0x00145890
		public T[] ToArray()
		{
			if (this._size == 0)
			{
				return Array.Empty<T>();
			}
			T[] array = new T[this._size];
			if (this._head < this._tail)
			{
				Array.Copy(this._array, this._head, array, 0, this._size);
			}
			else
			{
				Array.Copy(this._array, this._head, array, 0, this._array.Length - this._head);
				Array.Copy(this._array, 0, array, this._array.Length - this._head, this._tail);
			}
			return array;
		}

		// Token: 0x060061EC RID: 25068 RVA: 0x00147728 File Offset: 0x00145928
		private void SetCapacity(int capacity)
		{
			T[] array = new T[capacity];
			if (this._size > 0)
			{
				if (this._head < this._tail)
				{
					Array.Copy(this._array, this._head, array, 0, this._size);
				}
				else
				{
					Array.Copy(this._array, this._head, array, 0, this._array.Length - this._head);
					Array.Copy(this._array, 0, array, this._array.Length - this._head, this._tail);
				}
			}
			this._array = array;
			this._head = 0;
			this._tail = ((this._size == capacity) ? 0 : this._size);
			this._version++;
		}

		// Token: 0x060061ED RID: 25069 RVA: 0x001477E8 File Offset: 0x001459E8
		private void MoveNext(ref int index)
		{
			int num = index + 1;
			if (num == this._array.Length)
			{
				num = 0;
			}
			index = num;
		}

		// Token: 0x060061EE RID: 25070 RVA: 0x0014780A File Offset: 0x00145A0A
		private void ThrowForEmptyQueue()
		{
			throw new InvalidOperationException("Queue empty.");
		}

		// Token: 0x060061EF RID: 25071 RVA: 0x00147818 File Offset: 0x00145A18
		public void TrimExcess()
		{
			int num = (int)((double)this._array.Length * 0.9);
			if (this._size < num)
			{
				this.SetCapacity(this._size);
			}
		}

		// Token: 0x04003A0F RID: 14863
		private T[] _array;

		// Token: 0x04003A10 RID: 14864
		private int _head;

		// Token: 0x04003A11 RID: 14865
		private int _tail;

		// Token: 0x04003A12 RID: 14866
		private int _size;

		// Token: 0x04003A13 RID: 14867
		private int _version;

		// Token: 0x04003A14 RID: 14868
		[NonSerialized]
		private object _syncRoot;

		// Token: 0x04003A15 RID: 14869
		private const int MinimumGrow = 4;

		// Token: 0x04003A16 RID: 14870
		private const int GrowFactor = 200;

		// Token: 0x02000AB1 RID: 2737
		[Serializable]
		public struct Enumerator : IEnumerator<T>, IDisposable, IEnumerator
		{
			// Token: 0x060061F0 RID: 25072 RVA: 0x0014784F File Offset: 0x00145A4F
			internal Enumerator(Queue<T> q)
			{
				this._q = q;
				this._version = q._version;
				this._index = -1;
				this._currentElement = default(T);
			}

			// Token: 0x060061F1 RID: 25073 RVA: 0x00147877 File Offset: 0x00145A77
			public void Dispose()
			{
				this._index = -2;
				this._currentElement = default(T);
			}

			// Token: 0x060061F2 RID: 25074 RVA: 0x00147890 File Offset: 0x00145A90
			public bool MoveNext()
			{
				if (this._version != this._q._version)
				{
					throw new InvalidOperationException("Collection was modified; enumeration operation may not execute.");
				}
				if (this._index == -2)
				{
					return false;
				}
				this._index++;
				if (this._index == this._q._size)
				{
					this._index = -2;
					this._currentElement = default(T);
					return false;
				}
				T[] array = this._q._array;
				int num = array.Length;
				int num2 = this._q._head + this._index;
				if (num2 >= num)
				{
					num2 -= num;
				}
				this._currentElement = array[num2];
				return true;
			}

			// Token: 0x17001168 RID: 4456
			// (get) Token: 0x060061F3 RID: 25075 RVA: 0x00147937 File Offset: 0x00145B37
			public T Current
			{
				get
				{
					if (this._index < 0)
					{
						this.ThrowEnumerationNotStartedOrEnded();
					}
					return this._currentElement;
				}
			}

			// Token: 0x060061F4 RID: 25076 RVA: 0x0014794E File Offset: 0x00145B4E
			private void ThrowEnumerationNotStartedOrEnded()
			{
				throw new InvalidOperationException((this._index == -1) ? "Enumeration has not started. Call MoveNext." : "Enumeration already finished.");
			}

			// Token: 0x17001169 RID: 4457
			// (get) Token: 0x060061F5 RID: 25077 RVA: 0x0014796A File Offset: 0x00145B6A
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x060061F6 RID: 25078 RVA: 0x00147977 File Offset: 0x00145B77
			void IEnumerator.Reset()
			{
				if (this._version != this._q._version)
				{
					throw new InvalidOperationException("Collection was modified; enumeration operation may not execute.");
				}
				this._index = -1;
				this._currentElement = default(T);
			}

			// Token: 0x04003A17 RID: 14871
			private readonly Queue<T> _q;

			// Token: 0x04003A18 RID: 14872
			private readonly int _version;

			// Token: 0x04003A19 RID: 14873
			private int _index;

			// Token: 0x04003A1A RID: 14874
			private T _currentElement;
		}
	}
}
