using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace System.Collections.ObjectModel
{
	/// <summary>Provides the base class for a generic read-only collection.</summary>
	/// <typeparam name="T">The type of elements in the collection.</typeparam>
	// Token: 0x02000A81 RID: 2689
	[DebuggerTypeProxy(typeof(ICollectionDebugView<>))]
	[DebuggerDisplay("Count = {Count}")]
	[Serializable]
	public class ReadOnlyCollection<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IList, ICollection, IReadOnlyList<T>, IReadOnlyCollection<T>
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" /> class that is a read-only wrapper around the specified list.</summary>
		/// <param name="list">The list to wrap.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="list" /> is <see langword="null" />.</exception>
		// Token: 0x06006020 RID: 24608 RVA: 0x00142691 File Offset: 0x00140891
		public ReadOnlyCollection(IList<T> list)
		{
			if (list == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.list);
			}
			this.list = list;
		}

		/// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" /> instance.</summary>
		/// <returns>The number of elements contained in the <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" /> instance.</returns>
		// Token: 0x170010ED RID: 4333
		// (get) Token: 0x06006021 RID: 24609 RVA: 0x001426A9 File Offset: 0x001408A9
		public int Count
		{
			get
			{
				return this.list.Count;
			}
		}

		/// <summary>Gets the element at the specified index.</summary>
		/// <param name="index">The zero-based index of the element to get.</param>
		/// <returns>The element at the specified index.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero.  
		/// -or-  
		/// <paramref name="index" /> is equal to or greater than <see cref="P:System.Collections.ObjectModel.ReadOnlyCollection`1.Count" />.</exception>
		// Token: 0x170010EE RID: 4334
		public T this[int index]
		{
			get
			{
				return this.list[index];
			}
		}

		/// <summary>Determines whether an element is in the <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" />.</summary>
		/// <param name="value">The object to locate in the <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" />. The value can be <see langword="null" /> for reference types.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="value" /> is found in the <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06006023 RID: 24611 RVA: 0x001426C4 File Offset: 0x001408C4
		public bool Contains(T value)
		{
			return this.list.Contains(value);
		}

		/// <summary>Copies the entire <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" /> to a compatible one-dimensional <see cref="T:System.Array" />, starting at the specified index of the target array.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.</exception>
		// Token: 0x06006024 RID: 24612 RVA: 0x001426D2 File Offset: 0x001408D2
		public void CopyTo(T[] array, int index)
		{
			this.list.CopyTo(array, index);
		}

		/// <summary>Returns an enumerator that iterates through the <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" />.</summary>
		/// <returns>An <see cref="T:System.Collections.Generic.IEnumerator`1" /> for the <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" />.</returns>
		// Token: 0x06006025 RID: 24613 RVA: 0x001426E1 File Offset: 0x001408E1
		public IEnumerator<T> GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		/// <summary>Searches for the specified object and returns the zero-based index of the first occurrence within the entire <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" />.</summary>
		/// <param name="value">The object to locate in the <see cref="T:System.Collections.Generic.List`1" />. The value can be <see langword="null" /> for reference types.</param>
		/// <returns>The zero-based index of the first occurrence of <paramref name="item" /> within the entire <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" />, if found; otherwise, -1.</returns>
		// Token: 0x06006026 RID: 24614 RVA: 0x001426EE File Offset: 0x001408EE
		public int IndexOf(T value)
		{
			return this.list.IndexOf(value);
		}

		/// <summary>Returns the <see cref="T:System.Collections.Generic.IList`1" /> that the <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" /> wraps.</summary>
		/// <returns>The <see cref="T:System.Collections.Generic.IList`1" /> that the <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" /> wraps.</returns>
		// Token: 0x170010EF RID: 4335
		// (get) Token: 0x06006027 RID: 24615 RVA: 0x001426FC File Offset: 0x001408FC
		protected IList<T> Items
		{
			get
			{
				return this.list;
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.</summary>
		/// <returns>
		///   <see langword="true" /> if the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only; otherwise, <see langword="false" />.  In the default implementation of <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" />, this property always returns <see langword="true" />.</returns>
		// Token: 0x170010F0 RID: 4336
		// (get) Token: 0x06006028 RID: 24616 RVA: 0x000040F7 File Offset: 0x000022F7
		bool ICollection<T>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets the element at the specified index. An <see cref="T:System.NotSupportedException" /> occurs if you try to set the item at the specified index.</summary>
		/// <param name="index">The zero-based index of the element to get.</param>
		/// <returns>The element at the specified index.</returns>
		/// <exception cref="T:System.NotSupportedException">Always thrown if the property is set.</exception>
		// Token: 0x170010F1 RID: 4337
		T IList<T>.this[int index]
		{
			get
			{
				return this.list[index];
			}
			set
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
			}
		}

		/// <summary>Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.  This implementation always throws <see cref="T:System.NotSupportedException" />.</summary>
		/// <param name="value">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
		/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
		// Token: 0x0600602B RID: 24619 RVA: 0x00142704 File Offset: 0x00140904
		void ICollection<T>.Add(T value)
		{
			ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
		}

		/// <summary>Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.  This implementation always throws <see cref="T:System.NotSupportedException" />.</summary>
		/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
		// Token: 0x0600602C RID: 24620 RVA: 0x00142704 File Offset: 0x00140904
		void ICollection<T>.Clear()
		{
			ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
		}

		/// <summary>Inserts an item to the <see cref="T:System.Collections.Generic.IList`1" /> at the specified index.  This implementation always throws <see cref="T:System.NotSupportedException" />.</summary>
		/// <param name="index">The zero-based index at which <paramref name="value" /> should be inserted.</param>
		/// <param name="value">The object to insert into the <see cref="T:System.Collections.Generic.IList`1" />.</param>
		/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
		// Token: 0x0600602D RID: 24621 RVA: 0x00142704 File Offset: 0x00140904
		void IList<T>.Insert(int index, T value)
		{
			ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
		}

		/// <summary>Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.  This implementation always throws <see cref="T:System.NotSupportedException" />.</summary>
		/// <param name="value">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="value" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
		// Token: 0x0600602E RID: 24622 RVA: 0x0014270D File Offset: 0x0014090D
		bool ICollection<T>.Remove(T value)
		{
			ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
			return false;
		}

		/// <summary>Removes the <see cref="T:System.Collections.Generic.IList`1" /> item at the specified index.  This implementation always throws <see cref="T:System.NotSupportedException" />.</summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
		// Token: 0x0600602F RID: 24623 RVA: 0x00142704 File Offset: 0x00140904
		void IList<T>.RemoveAt(int index)
		{
			ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
		}

		/// <summary>Returns an enumerator that iterates through a collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the collection.</returns>
		// Token: 0x06006030 RID: 24624 RVA: 0x00142717 File Offset: 0x00140917
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		/// <summary>Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe).</summary>
		/// <returns>
		///   <see langword="true" /> if access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe); otherwise, <see langword="false" />.  In the default implementation of <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" />, this property always returns <see langword="false" />.</returns>
		// Token: 0x170010F2 RID: 4338
		// (get) Token: 0x06006031 RID: 24625 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.</summary>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.  In the default implementation of <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" />, this property always returns the current instance.</returns>
		// Token: 0x170010F3 RID: 4339
		// (get) Token: 0x06006032 RID: 24626 RVA: 0x00142724 File Offset: 0x00140924
		object ICollection.SyncRoot
		{
			get
			{
				if (this._syncRoot == null)
				{
					ICollection collection = this.list as ICollection;
					if (collection != null)
					{
						this._syncRoot = collection.SyncRoot;
					}
					else
					{
						Interlocked.CompareExchange<object>(ref this._syncRoot, new object(), null);
					}
				}
				return this._syncRoot;
			}
		}

		/// <summary>Copies the elements of the <see cref="T:System.Collections.ICollection" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" /> is multidimensional.  
		/// -or-  
		/// <paramref name="array" /> does not have zero-based indexing.  
		/// -or-  
		/// The number of elements in the source <see cref="T:System.Collections.ICollection" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.  
		/// -or-  
		/// The type of the source <see cref="T:System.Collections.ICollection" /> cannot be cast automatically to the type of the destination <paramref name="array" />.</exception>
		// Token: 0x06006033 RID: 24627 RVA: 0x00142770 File Offset: 0x00140970
		void ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
			}
			if (array.Rank != 1)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
			}
			if (array.GetLowerBound(0) != 0)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_NonZeroLowerBound);
			}
			if (index < 0)
			{
				ThrowHelper.ThrowIndexArgumentOutOfRange_NeedNonNegNumException();
			}
			if (array.Length - index < this.Count)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall);
			}
			T[] array2 = array as T[];
			if (array2 != null)
			{
				this.list.CopyTo(array2, index);
				return;
			}
			Type elementType = array.GetType().GetElementType();
			Type typeFromHandle = typeof(T);
			if (!elementType.IsAssignableFrom(typeFromHandle) && !typeFromHandle.IsAssignableFrom(elementType))
			{
				ThrowHelper.ThrowArgumentException_Argument_InvalidArrayType();
			}
			object[] array3 = array as object[];
			if (array3 == null)
			{
				ThrowHelper.ThrowArgumentException_Argument_InvalidArrayType();
			}
			int count = this.list.Count;
			try
			{
				for (int i = 0; i < count; i++)
				{
					array3[index++] = this.list[i];
				}
			}
			catch (ArrayTypeMismatchException)
			{
				ThrowHelper.ThrowArgumentException_Argument_InvalidArrayType();
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.IList" /> has a fixed size.</summary>
		/// <returns>
		///   <see langword="true" /> if the <see cref="T:System.Collections.IList" /> has a fixed size; otherwise, <see langword="false" />.  In the default implementation of <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" />, this property always returns <see langword="true" />.</returns>
		// Token: 0x170010F4 RID: 4340
		// (get) Token: 0x06006034 RID: 24628 RVA: 0x000040F7 File Offset: 0x000022F7
		bool IList.IsFixedSize
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.IList" /> is read-only.</summary>
		/// <returns>
		///   <see langword="true" /> if the <see cref="T:System.Collections.IList" /> is read-only; otherwise, <see langword="false" />.  In the default implementation of <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" />, this property always returns <see langword="true" />.</returns>
		// Token: 0x170010F5 RID: 4341
		// (get) Token: 0x06006035 RID: 24629 RVA: 0x000040F7 File Offset: 0x000022F7
		bool IList.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets the element at the specified index. A <see cref="T:System.NotSupportedException" /> occurs if you try to set the item at the specified index.</summary>
		/// <param name="index">The zero-based index of the element to get.</param>
		/// <returns>The element at the specified index.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is not a valid index in the <see cref="T:System.Collections.IList" />.</exception>
		/// <exception cref="T:System.NotSupportedException">Always thrown if the property is set.</exception>
		// Token: 0x170010F6 RID: 4342
		object IList.this[int index]
		{
			get
			{
				return this.list[index];
			}
			set
			{
				ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
			}
		}

		/// <summary>Adds an item to the <see cref="T:System.Collections.IList" />.  This implementation always throws <see cref="T:System.NotSupportedException" />.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to add to the <see cref="T:System.Collections.IList" />.</param>
		/// <returns>The position into which the new element was inserted.</returns>
		/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
		// Token: 0x06006038 RID: 24632 RVA: 0x0014287F File Offset: 0x00140A7F
		int IList.Add(object value)
		{
			ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
			return -1;
		}

		/// <summary>Removes all items from the <see cref="T:System.Collections.IList" />.  This implementation always throws <see cref="T:System.NotSupportedException" />.</summary>
		/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
		// Token: 0x06006039 RID: 24633 RVA: 0x00142704 File Offset: 0x00140904
		void IList.Clear()
		{
			ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
		}

		// Token: 0x0600603A RID: 24634 RVA: 0x0014288C File Offset: 0x00140A8C
		private static bool IsCompatibleObject(object value)
		{
			return value is T || (value == null && default(T) == null);
		}

		/// <summary>Determines whether the <see cref="T:System.Collections.IList" /> contains a specific value.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Collections.IList" />.</param>
		/// <returns>
		///   <see langword="true" /> if the <see cref="T:System.Object" /> is found in the <see cref="T:System.Collections.IList" />; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="value" /> is not of the type specified for the generic type parameter <paramref name="T" />.</exception>
		// Token: 0x0600603B RID: 24635 RVA: 0x001428B9 File Offset: 0x00140AB9
		bool IList.Contains(object value)
		{
			return ReadOnlyCollection<T>.IsCompatibleObject(value) && this.Contains((T)((object)value));
		}

		/// <summary>Determines the index of a specific item in the <see cref="T:System.Collections.IList" />.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Collections.IList" />.</param>
		/// <returns>The index of <paramref name="value" /> if found in the list; otherwise, -1.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="value" /> is not of the type specified for the generic type parameter <paramref name="T" />.</exception>
		// Token: 0x0600603C RID: 24636 RVA: 0x001428D1 File Offset: 0x00140AD1
		int IList.IndexOf(object value)
		{
			if (ReadOnlyCollection<T>.IsCompatibleObject(value))
			{
				return this.IndexOf((T)((object)value));
			}
			return -1;
		}

		/// <summary>Inserts an item to the <see cref="T:System.Collections.IList" /> at the specified index.  This implementation always throws <see cref="T:System.NotSupportedException" />.</summary>
		/// <param name="index">The zero-based index at which <paramref name="value" /> should be inserted.</param>
		/// <param name="value">The <see cref="T:System.Object" /> to insert into the <see cref="T:System.Collections.IList" />.</param>
		/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
		// Token: 0x0600603D RID: 24637 RVA: 0x00142704 File Offset: 0x00140904
		void IList.Insert(int index, object value)
		{
			ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
		}

		/// <summary>Removes the first occurrence of a specific object from the <see cref="T:System.Collections.IList" />.  This implementation always throws <see cref="T:System.NotSupportedException" />.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to remove from the <see cref="T:System.Collections.IList" />.</param>
		/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
		// Token: 0x0600603E RID: 24638 RVA: 0x00142704 File Offset: 0x00140904
		void IList.Remove(object value)
		{
			ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
		}

		/// <summary>Removes the <see cref="T:System.Collections.IList" /> item at the specified index.  This implementation always throws <see cref="T:System.NotSupportedException" />.</summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		/// <exception cref="T:System.NotSupportedException">Always thrown.</exception>
		// Token: 0x0600603F RID: 24639 RVA: 0x00142704 File Offset: 0x00140904
		void IList.RemoveAt(int index)
		{
			ThrowHelper.ThrowNotSupportedException(ExceptionResource.NotSupported_ReadOnlyCollection);
		}

		// Token: 0x040039AA RID: 14762
		private IList<T> list;

		// Token: 0x040039AB RID: 14763
		[NonSerialized]
		private object _syncRoot;
	}
}
