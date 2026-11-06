using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace System.Collections.Concurrent
{
	/// <summary>Represents a thread-safe last in-first out (LIFO) collection.</summary>
	/// <typeparam name="T">The type of the elements contained in the stack.</typeparam>
	// Token: 0x02000A5E RID: 2654
	[DebuggerTypeProxy(typeof(IProducerConsumerCollectionDebugView<>))]
	[DebuggerDisplay("Count = {Count}")]
	[Serializable]
	public class ConcurrentStack<T> : IProducerConsumerCollection<T>, IEnumerable<T>, IEnumerable, ICollection, IReadOnlyCollection<T>
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" /> class.</summary>
		// Token: 0x06005F50 RID: 24400 RVA: 0x0000259F File Offset: 0x0000079F
		public ConcurrentStack()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" /> class that contains elements copied from the specified collection</summary>
		/// <param name="collection">The collection whose elements are copied to the new <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" />.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="collection" /> argument is null.</exception>
		// Token: 0x06005F51 RID: 24401 RVA: 0x001409A8 File Offset: 0x0013EBA8
		public ConcurrentStack(IEnumerable<T> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			this.InitializeFromCollection(collection);
		}

		// Token: 0x06005F52 RID: 24402 RVA: 0x001409C8 File Offset: 0x0013EBC8
		private void InitializeFromCollection(IEnumerable<T> collection)
		{
			ConcurrentStack<T>.Node node = null;
			foreach (T t in collection)
			{
				node = new ConcurrentStack<T>.Node(t)
				{
					_next = node
				};
			}
			this._head = node;
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" /> is empty.</summary>
		/// <returns>
		///   <see langword="true" /> if the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" /> is empty; otherwise, <see langword="false" />.</returns>
		// Token: 0x170010BF RID: 4287
		// (get) Token: 0x06005F53 RID: 24403 RVA: 0x00140A20 File Offset: 0x0013EC20
		public bool IsEmpty
		{
			get
			{
				return this._head == null;
			}
		}

		/// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" />.</summary>
		/// <returns>The number of elements contained in the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" />.</returns>
		// Token: 0x170010C0 RID: 4288
		// (get) Token: 0x06005F54 RID: 24404 RVA: 0x00140A30 File Offset: 0x0013EC30
		public int Count
		{
			get
			{
				int num = 0;
				for (ConcurrentStack<T>.Node node = this._head; node != null; node = node._next)
				{
					num++;
				}
				return num;
			}
		}

		/// <summary>Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" /> is synchronized with the SyncRoot.</summary>
		/// <returns>Always returns <see langword="false" /> to indicate access is not synchronized.</returns>
		// Token: 0x170010C1 RID: 4289
		// (get) Token: 0x06005F55 RID: 24405 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />. This property is not supported.</summary>
		/// <returns>Returns null (Nothing in Visual Basic).</returns>
		/// <exception cref="T:System.NotSupportedException">The SyncRoot property is not supported</exception>
		// Token: 0x170010C2 RID: 4290
		// (get) Token: 0x06005F56 RID: 24406 RVA: 0x0013E2AF File Offset: 0x0013C4AF
		object ICollection.SyncRoot
		{
			get
			{
				throw new NotSupportedException("The SyncRoot property may not be used for the synchronization of concurrent collections.");
			}
		}

		/// <summary>Removes all objects from the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" />.</summary>
		// Token: 0x06005F57 RID: 24407 RVA: 0x00140A59 File Offset: 0x0013EC59
		public void Clear()
		{
			this._head = null;
		}

		/// <summary>Copies the elements of the <see cref="T:System.Collections.ICollection" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is a null reference (Nothing in Visual Basic).</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" /> is multidimensional. -or- <paramref name="array" /> does not have zero-based indexing. -or- <paramref name="index" /> is equal to or greater than the length of the <paramref name="array" /> -or- The number of elements in the source <see cref="T:System.Collections.ICollection" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />. -or- The type of the source <see cref="T:System.Collections.ICollection" /> cannot be cast automatically to the type of the destination <paramref name="array" />.</exception>
		// Token: 0x06005F58 RID: 24408 RVA: 0x00140A64 File Offset: 0x0013EC64
		void ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			((ICollection)this.ToList()).CopyTo(array, index);
		}

		/// <summary>Copies the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" /> elements to an existing one-dimensional <see cref="T:System.Array" />, starting at the specified array index.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is a null reference (Nothing in Visual Basic).</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="index" /> is equal to or greater than the length of the <paramref name="array" /> -or- The number of elements in the source <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.</exception>
		// Token: 0x06005F59 RID: 24409 RVA: 0x00140A81 File Offset: 0x0013EC81
		public void CopyTo(T[] array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			this.ToList().CopyTo(array, index);
		}

		/// <summary>Inserts an object at the top of the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" />.</summary>
		/// <param name="item">The object to push onto the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" />. The value can be a null reference (Nothing in Visual Basic) for reference types.</param>
		// Token: 0x06005F5A RID: 24410 RVA: 0x00140AA0 File Offset: 0x0013ECA0
		public void Push(T item)
		{
			ConcurrentStack<T>.Node node = new ConcurrentStack<T>.Node(item);
			node._next = this._head;
			if (Interlocked.CompareExchange<ConcurrentStack<T>.Node>(ref this._head, node, node._next) == node._next)
			{
				return;
			}
			this.PushCore(node, node);
		}

		/// <summary>Inserts multiple objects at the top of the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" /> atomically.</summary>
		/// <param name="items">The objects to push onto the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="items" /> is a null reference (Nothing in Visual Basic).</exception>
		// Token: 0x06005F5B RID: 24411 RVA: 0x00140AE5 File Offset: 0x0013ECE5
		public void PushRange(T[] items)
		{
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}
			this.PushRange(items, 0, items.Length);
		}

		/// <summary>Inserts multiple objects at the top of the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" /> atomically.</summary>
		/// <param name="items">The objects to push onto the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" />.</param>
		/// <param name="startIndex">The zero-based offset in <paramref name="items" /> at which to begin inserting elements onto the top of the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" />.</param>
		/// <param name="count">The number of elements to be inserted onto the top of the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="items" /> is a null reference (Nothing in Visual Basic).</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> or <paramref name="count" /> is negative. Or <paramref name="startIndex" /> is greater than or equal to the length of <paramref name="items" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="startIndex" /> + <paramref name="count" /> is greater than the length of <paramref name="items" />.</exception>
		// Token: 0x06005F5C RID: 24412 RVA: 0x00140B00 File Offset: 0x0013ED00
		public void PushRange(T[] items, int startIndex, int count)
		{
			ConcurrentStack<T>.ValidatePushPopRangeInput(items, startIndex, count);
			if (count == 0)
			{
				return;
			}
			ConcurrentStack<T>.Node node2;
			ConcurrentStack<T>.Node node = (node2 = new ConcurrentStack<T>.Node(items[startIndex]));
			for (int i = startIndex + 1; i < startIndex + count; i++)
			{
				node2 = new ConcurrentStack<T>.Node(items[i])
				{
					_next = node2
				};
			}
			node._next = this._head;
			if (Interlocked.CompareExchange<ConcurrentStack<T>.Node>(ref this._head, node2, node._next) == node._next)
			{
				return;
			}
			this.PushCore(node2, node);
		}

		// Token: 0x06005F5D RID: 24413 RVA: 0x00140B80 File Offset: 0x0013ED80
		private void PushCore(ConcurrentStack<T>.Node head, ConcurrentStack<T>.Node tail)
		{
			SpinWait spinWait = default(SpinWait);
			do
			{
				spinWait.SpinOnce();
				tail._next = this._head;
			}
			while (Interlocked.CompareExchange<ConcurrentStack<T>.Node>(ref this._head, head, tail._next) != tail._next);
			if (CDSCollectionETWBCLProvider.Log.IsEnabled())
			{
				CDSCollectionETWBCLProvider.Log.ConcurrentStack_FastPushFailed(spinWait.Count);
			}
		}

		// Token: 0x06005F5E RID: 24414 RVA: 0x00140BE4 File Offset: 0x0013EDE4
		private static void ValidatePushPopRangeInput(T[] items, int startIndex, int count)
		{
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "The count argument must be greater than or equal to zero.");
			}
			int num = items.Length;
			if (startIndex >= num || startIndex < 0)
			{
				throw new ArgumentOutOfRangeException("startIndex", "The startIndex argument must be greater than or equal to zero.");
			}
			if (num - count < startIndex)
			{
				throw new ArgumentException("The sum of the startIndex and count arguments must be less than or equal to the collection's Count.");
			}
		}

		/// <summary>Attempts to add an object to the <see cref="T:System.Collections.Concurrent.IProducerConsumerCollection`1" />.</summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Concurrent.IProducerConsumerCollection`1" />. The value can be a null reference (Nothing in Visual Basic) for reference types.</param>
		/// <returns>true if the object was added successfully; otherwise, false.</returns>
		// Token: 0x06005F5F RID: 24415 RVA: 0x00140C40 File Offset: 0x0013EE40
		bool IProducerConsumerCollection<T>.TryAdd(T item)
		{
			this.Push(item);
			return true;
		}

		/// <summary>Attempts to return an object from the top of the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" /> without removing it.</summary>
		/// <param name="result">When this method returns, <paramref name="result" /> contains an object from the top of the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" /> or an unspecified value if the operation failed.</param>
		/// <returns>true if and object was returned successfully; otherwise, false.</returns>
		// Token: 0x06005F60 RID: 24416 RVA: 0x00140C4C File Offset: 0x0013EE4C
		public bool TryPeek(out T result)
		{
			ConcurrentStack<T>.Node head = this._head;
			if (head == null)
			{
				result = default(T);
				return false;
			}
			result = head._value;
			return true;
		}

		/// <summary>Attempts to pop and return the object at the top of the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" />.</summary>
		/// <param name="result">When this method returns, if the operation was successful, <paramref name="result" /> contains the object removed. If no object was available to be removed, the value is unspecified.</param>
		/// <returns>true if an element was removed and returned from the top of the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" /> successfully; otherwise, false.</returns>
		// Token: 0x06005F61 RID: 24417 RVA: 0x00140C7C File Offset: 0x0013EE7C
		public bool TryPop(out T result)
		{
			ConcurrentStack<T>.Node head = this._head;
			if (head == null)
			{
				result = default(T);
				return false;
			}
			if (Interlocked.CompareExchange<ConcurrentStack<T>.Node>(ref this._head, head._next, head) == head)
			{
				result = head._value;
				return true;
			}
			return this.TryPopCore(out result);
		}

		/// <summary>Attempts to pop and return multiple objects from the top of the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" /> atomically.</summary>
		/// <param name="items">The <see cref="T:System.Array" /> to which objects popped from the top of the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" /> will be added.</param>
		/// <returns>The number of objects successfully popped from the top of the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" /> and inserted in <paramref name="items" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="items" /> is a null argument (Nothing in Visual Basic).</exception>
		// Token: 0x06005F62 RID: 24418 RVA: 0x00140CC8 File Offset: 0x0013EEC8
		public int TryPopRange(T[] items)
		{
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}
			return this.TryPopRange(items, 0, items.Length);
		}

		/// <summary>Attempts to pop and return multiple objects from the top of the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" /> atomically.</summary>
		/// <param name="items">The <see cref="T:System.Array" /> to which objects popped from the top of the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" /> will be added.</param>
		/// <param name="startIndex">The zero-based offset in <paramref name="items" /> at which to begin inserting elements from the top of the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" />.</param>
		/// <param name="count">The number of elements to be popped from top of the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" /> and inserted into <paramref name="items" />.</param>
		/// <returns>The number of objects successfully popped from the top of the stack and inserted in <paramref name="items" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="items" /> is a null reference (Nothing in Visual Basic).</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> or <paramref name="count" /> is negative. Or <paramref name="startIndex" /> is greater than or equal to the length of <paramref name="items" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="startIndex" /> + <paramref name="count" /> is greater than the length of <paramref name="items" />.</exception>
		// Token: 0x06005F63 RID: 24419 RVA: 0x00140CE4 File Offset: 0x0013EEE4
		public int TryPopRange(T[] items, int startIndex, int count)
		{
			ConcurrentStack<T>.ValidatePushPopRangeInput(items, startIndex, count);
			if (count == 0)
			{
				return 0;
			}
			ConcurrentStack<T>.Node node;
			int num = this.TryPopCore(count, out node);
			if (num > 0)
			{
				ConcurrentStack<T>.CopyRemovedItems(node, items, startIndex, num);
			}
			return num;
		}

		// Token: 0x06005F64 RID: 24420 RVA: 0x00140D18 File Offset: 0x0013EF18
		private bool TryPopCore(out T result)
		{
			ConcurrentStack<T>.Node node;
			if (this.TryPopCore(1, out node) == 1)
			{
				result = node._value;
				return true;
			}
			result = default(T);
			return false;
		}

		// Token: 0x06005F65 RID: 24421 RVA: 0x00140D48 File Offset: 0x0013EF48
		private int TryPopCore(int count, out ConcurrentStack<T>.Node poppedHead)
		{
			SpinWait spinWait = default(SpinWait);
			int num = 1;
			Random random = null;
			ConcurrentStack<T>.Node head;
			int num2;
			for (;;)
			{
				head = this._head;
				if (head == null)
				{
					break;
				}
				ConcurrentStack<T>.Node node = head;
				num2 = 1;
				while (num2 < count && node._next != null)
				{
					node = node._next;
					num2++;
				}
				if (Interlocked.CompareExchange<ConcurrentStack<T>.Node>(ref this._head, node._next, head) == head)
				{
					goto Block_5;
				}
				for (int i = 0; i < num; i++)
				{
					spinWait.SpinOnce();
				}
				if (spinWait.NextSpinWillYield)
				{
					if (random == null)
					{
						random = new Random();
					}
					num = random.Next(1, 8);
				}
				else
				{
					num *= 2;
				}
			}
			if (count == 1 && CDSCollectionETWBCLProvider.Log.IsEnabled())
			{
				CDSCollectionETWBCLProvider.Log.ConcurrentStack_FastPopFailed(spinWait.Count);
			}
			poppedHead = null;
			return 0;
			Block_5:
			if (count == 1 && CDSCollectionETWBCLProvider.Log.IsEnabled())
			{
				CDSCollectionETWBCLProvider.Log.ConcurrentStack_FastPopFailed(spinWait.Count);
			}
			poppedHead = head;
			return num2;
		}

		// Token: 0x06005F66 RID: 24422 RVA: 0x00140E34 File Offset: 0x0013F034
		private static void CopyRemovedItems(ConcurrentStack<T>.Node head, T[] collection, int startIndex, int nodesCount)
		{
			ConcurrentStack<T>.Node node = head;
			for (int i = startIndex; i < startIndex + nodesCount; i++)
			{
				collection[i] = node._value;
				node = node._next;
			}
		}

		/// <summary>Attempts to remove and return an object from the <see cref="T:System.Collections.Concurrent.IProducerConsumerCollection`1" />.</summary>
		/// <param name="item">When this method returns, if the operation was successful, <paramref name="item" /> contains the object removed. If no object was available to be removed, the value is unspecified.</param>
		/// <returns>true if an element was removed and returned succesfully; otherwise, false.</returns>
		// Token: 0x06005F67 RID: 24423 RVA: 0x00140E65 File Offset: 0x0013F065
		bool IProducerConsumerCollection<T>.TryTake(out T item)
		{
			return this.TryPop(out item);
		}

		/// <summary>Copies the items stored in the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" /> to a new array.</summary>
		/// <returns>A new array containing a snapshot of elements copied from the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" />.</returns>
		// Token: 0x06005F68 RID: 24424 RVA: 0x00140E70 File Offset: 0x0013F070
		public T[] ToArray()
		{
			ConcurrentStack<T>.Node head = this._head;
			if (head != null)
			{
				return this.ToList(head).ToArray();
			}
			return Array.Empty<T>();
		}

		// Token: 0x06005F69 RID: 24425 RVA: 0x00140E9B File Offset: 0x0013F09B
		private List<T> ToList()
		{
			return this.ToList(this._head);
		}

		// Token: 0x06005F6A RID: 24426 RVA: 0x00140EAC File Offset: 0x0013F0AC
		private List<T> ToList(ConcurrentStack<T>.Node curr)
		{
			List<T> list = new List<T>();
			while (curr != null)
			{
				list.Add(curr._value);
				curr = curr._next;
			}
			return list;
		}

		/// <summary>Returns an enumerator that iterates through the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" />.</summary>
		/// <returns>An enumerator for the <see cref="T:System.Collections.Concurrent.ConcurrentStack`1" />.</returns>
		// Token: 0x06005F6B RID: 24427 RVA: 0x00140ED9 File Offset: 0x0013F0D9
		public IEnumerator<T> GetEnumerator()
		{
			return this.GetEnumerator(this._head);
		}

		// Token: 0x06005F6C RID: 24428 RVA: 0x00140EE9 File Offset: 0x0013F0E9
		private IEnumerator<T> GetEnumerator(ConcurrentStack<T>.Node head)
		{
			for (ConcurrentStack<T>.Node current = head; current != null; current = current._next)
			{
				yield return current._value;
			}
			yield break;
		}

		/// <summary>Returns an enumerator that iterates through a collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the collection.</returns>
		// Token: 0x06005F6D RID: 24429 RVA: 0x0013E2BB File Offset: 0x0013C4BB
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<T>)this).GetEnumerator();
		}

		// Token: 0x04003956 RID: 14678
		private volatile ConcurrentStack<T>.Node _head;

		// Token: 0x04003957 RID: 14679
		private const int BACKOFF_MAX_YIELDS = 8;

		// Token: 0x02000A5F RID: 2655
		[Serializable]
		private class Node
		{
			// Token: 0x06005F6E RID: 24430 RVA: 0x00140EF8 File Offset: 0x0013F0F8
			internal Node(T value)
			{
				this._value = value;
				this._next = null;
			}

			// Token: 0x04003958 RID: 14680
			internal readonly T _value;

			// Token: 0x04003959 RID: 14681
			internal ConcurrentStack<T>.Node _next;
		}
	}
}
