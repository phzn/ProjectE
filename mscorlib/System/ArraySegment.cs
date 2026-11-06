using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics.Hashing;

namespace System
{
	/// <summary>Delimits a section of a one-dimensional array.</summary>
	/// <typeparam name="T">The type of the elements in the array segment.</typeparam>
	// Token: 0x020000F7 RID: 247
	[Serializable]
	public readonly struct ArraySegment<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IReadOnlyList<T>, IReadOnlyCollection<T>
	{
		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000719 RID: 1817 RVA: 0x0002126E File Offset: 0x0001F46E
		public static ArraySegment<T> Empty { get; } = new ArraySegment<T>(new T[0]);

		/// <summary>Initializes a new instance of the <see cref="T:System.ArraySegment`1" /> structure that delimits all the elements in the specified array.</summary>
		/// <param name="array">The array to wrap.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		// Token: 0x0600071A RID: 1818 RVA: 0x00021275 File Offset: 0x0001F475
		public ArraySegment(T[] array)
		{
			if (array == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
			}
			this._array = array;
			this._offset = 0;
			this._count = array.Length;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ArraySegment`1" /> structure that delimits the specified range of the elements in the specified array.</summary>
		/// <param name="array">The array containing the range of elements to delimit.</param>
		/// <param name="offset">The zero-based index of the first element in the range.</param>
		/// <param name="count">The number of elements in the range.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> or <paramref name="count" /> is less than 0.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="offset" /> and <paramref name="count" /> do not specify a valid range in <paramref name="array" />.</exception>
		// Token: 0x0600071B RID: 1819 RVA: 0x00021297 File Offset: 0x0001F497
		public ArraySegment(T[] array, int offset, int count)
		{
			if (array == null || offset > array.Length || count > array.Length - offset)
			{
				ThrowHelper.ThrowArraySegmentCtorValidationFailedExceptions(array, offset, count);
			}
			this._array = array;
			this._offset = offset;
			this._count = count;
		}

		/// <summary>Gets the original array containing the range of elements that the array segment delimits.</summary>
		/// <returns>The original array that was passed to the constructor, and that contains the range delimited by the <see cref="T:System.ArraySegment`1" />.</returns>
		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600071C RID: 1820 RVA: 0x000212C7 File Offset: 0x0001F4C7
		public T[] Array
		{
			get
			{
				return this._array;
			}
		}

		/// <summary>Gets the position of the first element in the range delimited by the array segment, relative to the start of the original array.</summary>
		/// <returns>The position of the first element in the range delimited by the <see cref="T:System.ArraySegment`1" />, relative to the start of the original array.</returns>
		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x0600071D RID: 1821 RVA: 0x000212CF File Offset: 0x0001F4CF
		public int Offset
		{
			get
			{
				return this._offset;
			}
		}

		/// <summary>Gets the number of elements in the range delimited by the array segment.</summary>
		/// <returns>The number of elements in the range delimited by the <see cref="T:System.ArraySegment`1" />.</returns>
		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x0600071E RID: 1822 RVA: 0x000212D7 File Offset: 0x0001F4D7
		public int Count
		{
			get
			{
				return this._count;
			}
		}

		// Token: 0x170000A3 RID: 163
		public T this[int index]
		{
			get
			{
				if (index >= this._count)
				{
					ThrowHelper.ThrowArgumentOutOfRange_IndexException();
				}
				return this._array[this._offset + index];
			}
			set
			{
				if (index >= this._count)
				{
					ThrowHelper.ThrowArgumentOutOfRange_IndexException();
				}
				this._array[this._offset + index] = value;
			}
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x00021326 File Offset: 0x0001F526
		public ArraySegment<T>.Enumerator GetEnumerator()
		{
			this.ThrowInvalidOperationIfDefault();
			return new ArraySegment<T>.Enumerator(this);
		}

		/// <summary>Returns the hash code for the current instance.</summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		// Token: 0x06000722 RID: 1826 RVA: 0x00021339 File Offset: 0x0001F539
		public override int GetHashCode()
		{
			if (this._array == null)
			{
				return 0;
			}
			return global::System.Numerics.Hashing.HashHelpers.Combine(global::System.Numerics.Hashing.HashHelpers.Combine(5381, this._offset), this._count) ^ this._array.GetHashCode();
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x0002136C File Offset: 0x0001F56C
		public void CopyTo(T[] destination)
		{
			this.CopyTo(destination, 0);
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x00021376 File Offset: 0x0001F576
		public void CopyTo(T[] destination, int destinationIndex)
		{
			this.ThrowInvalidOperationIfDefault();
			global::System.Array.Copy(this._array, this._offset, destination, destinationIndex, this._count);
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x00021398 File Offset: 0x0001F598
		public void CopyTo(ArraySegment<T> destination)
		{
			this.ThrowInvalidOperationIfDefault();
			destination.ThrowInvalidOperationIfDefault();
			if (this._count > destination._count)
			{
				ThrowHelper.ThrowArgumentException_DestinationTooShort();
			}
			global::System.Array.Copy(this._array, this._offset, destination._array, destination._offset, this._count);
		}

		/// <summary>Determines whether the specified object is equal to the current instance.</summary>
		/// <param name="obj">The object to be compared with the current instance.</param>
		/// <returns>
		///   <see langword="true" /> if the specified object is a <see cref="T:System.ArraySegment`1" /> structure and is equal to the current instance; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000726 RID: 1830 RVA: 0x000213E8 File Offset: 0x0001F5E8
		public override bool Equals(object obj)
		{
			return obj is ArraySegment<T> && this.Equals((ArraySegment<T>)obj);
		}

		/// <summary>Determines whether the specified <see cref="T:System.ArraySegment`1" /> structure is equal to the current instance.</summary>
		/// <param name="obj">The structure to compare with the current instance.</param>
		/// <returns>
		///   <see langword="true" /> if the specified <see cref="T:System.ArraySegment`1" /> structure is equal to the current instance; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000727 RID: 1831 RVA: 0x00021400 File Offset: 0x0001F600
		public bool Equals(ArraySegment<T> obj)
		{
			return obj._array == this._array && obj._offset == this._offset && obj._count == this._count;
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x0002142E File Offset: 0x0001F62E
		public ArraySegment<T> Slice(int index)
		{
			this.ThrowInvalidOperationIfDefault();
			if (index > this._count)
			{
				ThrowHelper.ThrowArgumentOutOfRange_IndexException();
			}
			return new ArraySegment<T>(this._array, this._offset + index, this._count - index);
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x0002145F File Offset: 0x0001F65F
		public ArraySegment<T> Slice(int index, int count)
		{
			this.ThrowInvalidOperationIfDefault();
			if (index > this._count || count > this._count - index)
			{
				ThrowHelper.ThrowArgumentOutOfRange_IndexException();
			}
			return new ArraySegment<T>(this._array, this._offset + index, count);
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x00021494 File Offset: 0x0001F694
		public T[] ToArray()
		{
			this.ThrowInvalidOperationIfDefault();
			if (this._count == 0)
			{
				return ArraySegment<T>.Empty._array;
			}
			T[] array = new T[this._count];
			global::System.Array.Copy(this._array, this._offset, array, 0, this._count);
			return array;
		}

		/// <summary>Indicates whether two <see cref="T:System.ArraySegment`1" /> structures are equal.</summary>
		/// <param name="a">The  structure on the left side of the equality operator.</param>
		/// <param name="b">The structure on the right side of the equality operator.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="a" /> is equal to <paramref name="b" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600072B RID: 1835 RVA: 0x000214E0 File Offset: 0x0001F6E0
		public static bool operator ==(ArraySegment<T> a, ArraySegment<T> b)
		{
			return a.Equals(b);
		}

		/// <summary>Indicates whether two <see cref="T:System.ArraySegment`1" /> structures are unequal.</summary>
		/// <param name="a">The structure on the left side of the inequality operator.</param>
		/// <param name="b">The structure on the right side of the inequality operator.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="a" /> is not equal to <paramref name="b" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600072C RID: 1836 RVA: 0x000214EA File Offset: 0x0001F6EA
		public static bool operator !=(ArraySegment<T> a, ArraySegment<T> b)
		{
			return !(a == b);
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x000214F8 File Offset: 0x0001F6F8
		public static implicit operator ArraySegment<T>(T[] array)
		{
			if (array == null)
			{
				return default(ArraySegment<T>);
			}
			return new ArraySegment<T>(array);
		}

		/// <summary>Gets or sets the element at the specified index.</summary>
		/// <param name="index">The zero-based index of the element to get or set.</param>
		/// <returns>The element at the specified index.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is not a valid index in the <see cref="T:System.ArraySegment`1" />.</exception>
		/// <exception cref="T:System.NotSupportedException">The property is set and the array segment is read-only.</exception>
		// Token: 0x170000A4 RID: 164
		T IList<T>.this[int index]
		{
			get
			{
				this.ThrowInvalidOperationIfDefault();
				if (index < 0 || index >= this._count)
				{
					ThrowHelper.ThrowArgumentOutOfRange_IndexException();
				}
				return this._array[this._offset + index];
			}
			set
			{
				this.ThrowInvalidOperationIfDefault();
				if (index < 0 || index >= this._count)
				{
					ThrowHelper.ThrowArgumentOutOfRange_IndexException();
				}
				this._array[this._offset + index] = value;
			}
		}

		/// <summary>Determines the index of a specific item in the array segment.</summary>
		/// <param name="item">The object to locate in the array segment.</param>
		/// <returns>The index of <paramref name="item" /> if found in the list; otherwise, -1.</returns>
		// Token: 0x06000730 RID: 1840 RVA: 0x00021574 File Offset: 0x0001F774
		int IList<T>.IndexOf(T item)
		{
			this.ThrowInvalidOperationIfDefault();
			int num = global::System.Array.IndexOf<T>(this._array, item, this._offset, this._count);
			if (num < 0)
			{
				return -1;
			}
			return num - this._offset;
		}

		/// <summary>Inserts an item into the array segment at the specified index.</summary>
		/// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
		/// <param name="item">The object to insert into the array segment.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is not a valid index in the array segment.</exception>
		/// <exception cref="T:System.NotSupportedException">The array segment is read-only.</exception>
		// Token: 0x06000731 RID: 1841 RVA: 0x000215AE File Offset: 0x0001F7AE
		void IList<T>.Insert(int index, T item)
		{
			ThrowHelper.ThrowNotSupportedException();
		}

		/// <summary>Removes the array segment item at the specified index.</summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is not a valid index in the array segment.</exception>
		/// <exception cref="T:System.NotSupportedException">The array segment is read-only.</exception>
		// Token: 0x06000732 RID: 1842 RVA: 0x000215AE File Offset: 0x0001F7AE
		void IList<T>.RemoveAt(int index)
		{
			ThrowHelper.ThrowNotSupportedException();
		}

		/// <summary>Gets the element at the specified index of the array segment.</summary>
		/// <param name="index">The zero-based index of the element to get.</param>
		/// <returns>The element at the specified index.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is not a valid index in the <see cref="T:System.ArraySegment`1" />.</exception>
		/// <exception cref="T:System.NotSupportedException">The property is set.</exception>
		// Token: 0x170000A5 RID: 165
		T IReadOnlyList<T>.this[int index]
		{
			get
			{
				this.ThrowInvalidOperationIfDefault();
				if (index < 0 || index >= this._count)
				{
					ThrowHelper.ThrowArgumentOutOfRange_IndexException();
				}
				return this._array[this._offset + index];
			}
		}

		/// <summary>Gets a value that indicates whether the array segment  is read-only.</summary>
		/// <returns>
		///   <see langword="true" /> if the array segment is read-only; otherwise, <see langword="false" />.</returns>
		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000734 RID: 1844 RVA: 0x000040F7 File Offset: 0x000022F7
		bool ICollection<T>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		/// <summary>Adds an item to the array segment.</summary>
		/// <param name="item">The object to add to the array segment.</param>
		/// <exception cref="T:System.NotSupportedException">The array segment is read-only.</exception>
		// Token: 0x06000735 RID: 1845 RVA: 0x000215AE File Offset: 0x0001F7AE
		void ICollection<T>.Add(T item)
		{
			ThrowHelper.ThrowNotSupportedException();
		}

		/// <summary>Removes all items from the array segment.</summary>
		/// <exception cref="T:System.NotSupportedException">The array segment is read-only.</exception>
		// Token: 0x06000736 RID: 1846 RVA: 0x000215AE File Offset: 0x0001F7AE
		void ICollection<T>.Clear()
		{
			ThrowHelper.ThrowNotSupportedException();
		}

		/// <summary>Determines whether the array segment contains a specific value.</summary>
		/// <param name="item">The object to locate in the array segment.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="item" /> is found in the array segment; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000737 RID: 1847 RVA: 0x000215B5 File Offset: 0x0001F7B5
		bool ICollection<T>.Contains(T item)
		{
			this.ThrowInvalidOperationIfDefault();
			return global::System.Array.IndexOf<T>(this._array, item, this._offset, this._count) >= 0;
		}

		/// <summary>Removes the first occurrence of a specific object from the array segment.</summary>
		/// <param name="item">The object to remove from the array segment.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="item" /> was successfully removed from the array segment; otherwise, <see langword="false" />. This method also returns <see langword="false" /> if <paramref name="item" /> is not found in the array segment.</returns>
		/// <exception cref="T:System.NotSupportedException">The array segment is read-only.</exception>
		// Token: 0x06000738 RID: 1848 RVA: 0x000215DB File Offset: 0x0001F7DB
		bool ICollection<T>.Remove(T item)
		{
			ThrowHelper.ThrowNotSupportedException();
			return false;
		}

		/// <summary>Returns an enumerator that iterates through the array segment.</summary>
		/// <returns>An enumerator that can be used to iterate through the array segment.</returns>
		// Token: 0x06000739 RID: 1849 RVA: 0x000215E3 File Offset: 0x0001F7E3
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		/// <summary>Returns an enumerator that iterates through an array segment.</summary>
		/// <returns>An enumerator that can be used to iterate through the array segment.</returns>
		// Token: 0x0600073A RID: 1850 RVA: 0x000215E3 File Offset: 0x0001F7E3
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x000215F0 File Offset: 0x0001F7F0
		private void ThrowInvalidOperationIfDefault()
		{
			if (this._array == null)
			{
				ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_NullArray);
			}
		}

		// Token: 0x0400104B RID: 4171
		private readonly T[] _array;

		// Token: 0x0400104C RID: 4172
		private readonly int _offset;

		// Token: 0x0400104D RID: 4173
		private readonly int _count;

		// Token: 0x020000F8 RID: 248
		public struct Enumerator : IEnumerator<T>, IDisposable, IEnumerator
		{
			// Token: 0x0600073D RID: 1853 RVA: 0x00021613 File Offset: 0x0001F813
			internal Enumerator(ArraySegment<T> arraySegment)
			{
				this._array = arraySegment.Array;
				this._start = arraySegment.Offset;
				this._end = arraySegment.Offset + arraySegment.Count;
				this._current = arraySegment.Offset - 1;
			}

			// Token: 0x0600073E RID: 1854 RVA: 0x00021653 File Offset: 0x0001F853
			public bool MoveNext()
			{
				if (this._current < this._end)
				{
					this._current++;
					return this._current < this._end;
				}
				return false;
			}

			// Token: 0x170000A7 RID: 167
			// (get) Token: 0x0600073F RID: 1855 RVA: 0x00021681 File Offset: 0x0001F881
			public T Current
			{
				get
				{
					if (this._current < this._start)
					{
						ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumNotStarted();
					}
					if (this._current >= this._end)
					{
						ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumEnded();
					}
					return this._array[this._current];
				}
			}

			// Token: 0x170000A8 RID: 168
			// (get) Token: 0x06000740 RID: 1856 RVA: 0x000216BA File Offset: 0x0001F8BA
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x06000741 RID: 1857 RVA: 0x000216C7 File Offset: 0x0001F8C7
			void IEnumerator.Reset()
			{
				this._current = this._start - 1;
			}

			// Token: 0x06000742 RID: 1858 RVA: 0x00004BF9 File Offset: 0x00002DF9
			public void Dispose()
			{
			}

			// Token: 0x0400104E RID: 4174
			private readonly T[] _array;

			// Token: 0x0400104F RID: 4175
			private readonly int _start;

			// Token: 0x04001050 RID: 4176
			private readonly int _end;

			// Token: 0x04001051 RID: 4177
			private int _current;
		}
	}
}
