using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Collections.Concurrent
{
	/// <summary>Represents a thread-safe first in-first out (FIFO) collection.</summary>
	/// <typeparam name="T">The type of the elements contained in the queue.</typeparam>
	// Token: 0x02000A52 RID: 2642
	[DebuggerDisplay("Count = {Count}")]
	[DebuggerTypeProxy(typeof(IProducerConsumerCollectionDebugView<>))]
	[Serializable]
	public class ConcurrentQueue<T> : IProducerConsumerCollection<T>, IEnumerable<T>, IEnumerable, ICollection, IReadOnlyCollection<T>
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" /> class.</summary>
		// Token: 0x06005EC4 RID: 24260 RVA: 0x0013E180 File Offset: 0x0013C380
		public ConcurrentQueue()
		{
			this._crossSegmentLock = new object();
			this._tail = (this._head = new ConcurrentQueue<T>.Segment(32));
		}

		// Token: 0x06005EC5 RID: 24261 RVA: 0x0013E1B8 File Offset: 0x0013C3B8
		private void InitializeFromCollection(IEnumerable<T> collection)
		{
			this._crossSegmentLock = new object();
			int num = 32;
			ICollection<T> collection2 = collection as ICollection<T>;
			if (collection2 != null)
			{
				int count = collection2.Count;
				if (count > num)
				{
					num = Math.Min(ConcurrentQueue<T>.Segment.RoundUpToPowerOf2(count), 1048576);
				}
			}
			this._tail = (this._head = new ConcurrentQueue<T>.Segment(num));
			foreach (T t in collection)
			{
				this.Enqueue(t);
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" /> class that contains elements copied from the specified collection</summary>
		/// <param name="collection">The collection whose elements are copied to the new <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" />.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="collection" /> argument is null.</exception>
		// Token: 0x06005EC6 RID: 24262 RVA: 0x0013E254 File Offset: 0x0013C454
		public ConcurrentQueue(IEnumerable<T> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			this.InitializeFromCollection(collection);
		}

		/// <summary>Copies the elements of the <see cref="T:System.Collections.ICollection" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from the <see cref="T:System.Collections.Concurrent.ConcurrentBag`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is a null reference (Nothing in Visual Basic).</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="array" /> is multidimensional. -or- <paramref name="array" /> does not have zero-based indexing. -or- <paramref name="index" /> is equal to or greater than the length of the <paramref name="array" /> -or- The number of elements in the source <see cref="T:System.Collections.ICollection" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />. -or- The type of the source <see cref="T:System.Collections.ICollection" /> cannot be cast automatically to the type of the destination <paramref name="array" />.</exception>
		// Token: 0x06005EC7 RID: 24263 RVA: 0x0013E274 File Offset: 0x0013C474
		void ICollection.CopyTo(Array array, int index)
		{
			T[] array2 = array as T[];
			if (array2 != null)
			{
				this.CopyTo(array2, index);
				return;
			}
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			this.ToArray().CopyTo(array, index);
		}

		/// <summary>Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" /> is synchronized with the SyncRoot.</summary>
		/// <returns>Always returns <see langword="false" /> to indicate access is not synchronized.</returns>
		// Token: 0x170010A0 RID: 4256
		// (get) Token: 0x06005EC8 RID: 24264 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />. This property is not supported.</summary>
		/// <returns>Returns null  (Nothing in Visual Basic).</returns>
		/// <exception cref="T:System.NotSupportedException">The SyncRoot property is not supported.</exception>
		// Token: 0x170010A1 RID: 4257
		// (get) Token: 0x06005EC9 RID: 24265 RVA: 0x0013E2AF File Offset: 0x0013C4AF
		object ICollection.SyncRoot
		{
			get
			{
				throw new NotSupportedException("The SyncRoot property may not be used for the synchronization of concurrent collections.");
			}
		}

		/// <summary>Returns an enumerator that iterates through a collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the collection.</returns>
		// Token: 0x06005ECA RID: 24266 RVA: 0x0013E2BB File Offset: 0x0013C4BB
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<T>)this).GetEnumerator();
		}

		/// <summary>Attempts to add an object to the <see cref="T:System.Collections.Concurrent.IProducerConsumerCollection`1" />.</summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Concurrent.IProducerConsumerCollection`1" />. The value can be a null reference (Nothing in Visual Basic) for reference types.</param>
		/// <returns>true if the object was added successfully; otherwise, false.</returns>
		// Token: 0x06005ECB RID: 24267 RVA: 0x0013E2C3 File Offset: 0x0013C4C3
		bool IProducerConsumerCollection<T>.TryAdd(T item)
		{
			this.Enqueue(item);
			return true;
		}

		/// <summary>Attempts to remove and return an object from the <see cref="T:System.Collections.Concurrent.IProducerConsumerCollection`1" />.</summary>
		/// <param name="item">When this method returns, if the operation was successful, <paramref name="item" /> contains the object removed. If no object was available to be removed, the value is unspecified.</param>
		/// <returns>true if an element was removed and returned succesfully; otherwise, false.</returns>
		// Token: 0x06005ECC RID: 24268 RVA: 0x0007E93B File Offset: 0x0007CB3B
		bool IProducerConsumerCollection<T>.TryTake(out T item)
		{
			return this.TryDequeue(out item);
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" /> is empty.</summary>
		/// <returns>
		///   <see langword="true" /> if the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" /> is empty; otherwise, <see langword="false" />.</returns>
		// Token: 0x170010A2 RID: 4258
		// (get) Token: 0x06005ECD RID: 24269 RVA: 0x0013E2D0 File Offset: 0x0013C4D0
		public bool IsEmpty
		{
			get
			{
				T t;
				return !this.TryPeek(out t, false);
			}
		}

		/// <summary>Copies the elements stored in the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" /> to a new array.</summary>
		/// <returns>A new array containing a snapshot of elements copied from the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" />.</returns>
		// Token: 0x06005ECE RID: 24270 RVA: 0x0013E2EC File Offset: 0x0013C4EC
		public T[] ToArray()
		{
			ConcurrentQueue<T>.Segment segment;
			int num;
			ConcurrentQueue<T>.Segment segment2;
			int num2;
			this.SnapForObservation(out segment, out num, out segment2, out num2);
			T[] array = new T[ConcurrentQueue<T>.GetCount(segment, num, segment2, num2)];
			using (IEnumerator<T> enumerator = this.Enumerate(segment, num, segment2, num2))
			{
				int num3 = 0;
				while (enumerator.MoveNext())
				{
					T t = enumerator.Current;
					array[num3++] = t;
				}
			}
			return array;
		}

		/// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" />.</summary>
		/// <returns>The number of elements contained in the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" />.</returns>
		// Token: 0x170010A3 RID: 4259
		// (get) Token: 0x06005ECF RID: 24271 RVA: 0x0013E368 File Offset: 0x0013C568
		public int Count
		{
			get
			{
				SpinWait spinWait = default(SpinWait);
				ConcurrentQueue<T>.Segment head;
				ConcurrentQueue<T>.Segment tail;
				int num;
				int num2;
				int num3;
				int num4;
				for (;;)
				{
					head = this._head;
					tail = this._tail;
					num = Volatile.Read(ref head._headAndTail.Head);
					num2 = Volatile.Read(ref head._headAndTail.Tail);
					if (head == tail)
					{
						if (head == this._head && head == this._tail && num == Volatile.Read(ref head._headAndTail.Head) && num2 == Volatile.Read(ref head._headAndTail.Tail))
						{
							break;
						}
					}
					else
					{
						if (head._nextSegment != tail)
						{
							goto IL_013C;
						}
						num3 = Volatile.Read(ref tail._headAndTail.Head);
						num4 = Volatile.Read(ref tail._headAndTail.Tail);
						if (head == this._head && tail == this._tail && num == Volatile.Read(ref head._headAndTail.Head) && num2 == Volatile.Read(ref head._headAndTail.Tail) && num3 == Volatile.Read(ref tail._headAndTail.Head) && num4 == Volatile.Read(ref tail._headAndTail.Tail))
						{
							goto Block_12;
						}
					}
					spinWait.SpinOnce();
				}
				return ConcurrentQueue<T>.GetCount(head, num, num2);
				Block_12:
				return ConcurrentQueue<T>.GetCount(head, num, num2) + ConcurrentQueue<T>.GetCount(tail, num3, num4);
				IL_013C:
				this.SnapForObservation(out head, out num, out tail, out num4);
				return (int)ConcurrentQueue<T>.GetCount(head, num, tail, num4);
			}
		}

		// Token: 0x06005ED0 RID: 24272 RVA: 0x0013E4D6 File Offset: 0x0013C6D6
		private static int GetCount(ConcurrentQueue<T>.Segment s, int head, int tail)
		{
			if (head == tail || head == tail - s.FreezeOffset)
			{
				return 0;
			}
			head &= s._slotsMask;
			tail &= s._slotsMask;
			if (head >= tail)
			{
				return s._slots.Length - head + tail;
			}
			return tail - head;
		}

		// Token: 0x06005ED1 RID: 24273 RVA: 0x0013E514 File Offset: 0x0013C714
		private static long GetCount(ConcurrentQueue<T>.Segment head, int headHead, ConcurrentQueue<T>.Segment tail, int tailTail)
		{
			long num = 0L;
			int num2 = ((head == tail) ? tailTail : Volatile.Read(ref head._headAndTail.Tail)) - head.FreezeOffset;
			if (headHead < num2)
			{
				headHead &= head._slotsMask;
				num2 &= head._slotsMask;
				num += (long)((headHead < num2) ? (num2 - headHead) : (head._slots.Length - headHead + num2));
			}
			if (head != tail)
			{
				for (ConcurrentQueue<T>.Segment segment = head._nextSegment; segment != tail; segment = segment._nextSegment)
				{
					num += (long)(segment._headAndTail.Tail - segment.FreezeOffset);
				}
				num += (long)(tailTail - tail.FreezeOffset);
			}
			return num;
		}

		/// <summary>Copies the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" /> elements to an existing one-dimensional <see cref="T:System.Array" />, starting at the specified array index.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is a null reference (Nothing in Visual Basic).</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="index" /> is equal to or greater than the length of the <paramref name="array" /> -or- The number of elements in the source <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" /> is greater than the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.</exception>
		// Token: 0x06005ED2 RID: 24274 RVA: 0x0013E5B0 File Offset: 0x0013C7B0
		public void CopyTo(T[] array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", "The index argument must be greater than or equal zero.");
			}
			ConcurrentQueue<T>.Segment segment;
			int num;
			ConcurrentQueue<T>.Segment segment2;
			int num2;
			this.SnapForObservation(out segment, out num, out segment2, out num2);
			long count = ConcurrentQueue<T>.GetCount(segment, num, segment2, num2);
			if ((long)index > (long)array.Length - count)
			{
				throw new ArgumentException("The number of elements in the collection is greater than the available space from index to the end of the destination array.");
			}
			int num3 = index;
			using (IEnumerator<T> enumerator = this.Enumerate(segment, num, segment2, num2))
			{
				while (enumerator.MoveNext())
				{
					T t = enumerator.Current;
					array[num3++] = t;
				}
			}
		}

		/// <summary>Returns an enumerator that iterates through the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" />.</summary>
		/// <returns>An enumerator for the contents of the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" />.</returns>
		// Token: 0x06005ED3 RID: 24275 RVA: 0x0013E65C File Offset: 0x0013C85C
		public IEnumerator<T> GetEnumerator()
		{
			ConcurrentQueue<T>.Segment segment;
			int num;
			ConcurrentQueue<T>.Segment segment2;
			int num2;
			this.SnapForObservation(out segment, out num, out segment2, out num2);
			return this.Enumerate(segment, num, segment2, num2);
		}

		// Token: 0x06005ED4 RID: 24276 RVA: 0x0013E684 File Offset: 0x0013C884
		private void SnapForObservation(out ConcurrentQueue<T>.Segment head, out int headHead, out ConcurrentQueue<T>.Segment tail, out int tailTail)
		{
			object crossSegmentLock = this._crossSegmentLock;
			lock (crossSegmentLock)
			{
				head = this._head;
				tail = this._tail;
				ConcurrentQueue<T>.Segment segment = head;
				for (;;)
				{
					segment._preservedForObservation = true;
					if (segment == tail)
					{
						break;
					}
					segment = segment._nextSegment;
				}
				tail.EnsureFrozenForEnqueues();
				headHead = Volatile.Read(ref head._headAndTail.Head);
				tailTail = Volatile.Read(ref tail._headAndTail.Tail);
			}
		}

		// Token: 0x06005ED5 RID: 24277 RVA: 0x0013E718 File Offset: 0x0013C918
		private T GetItemWhenAvailable(ConcurrentQueue<T>.Segment segment, int i)
		{
			int num = (i + 1) & segment._slotsMask;
			if ((segment._slots[i].SequenceNumber & segment._slotsMask) != num)
			{
				SpinWait spinWait = default(SpinWait);
				while ((Volatile.Read(ref segment._slots[i].SequenceNumber) & segment._slotsMask) != num)
				{
					spinWait.SpinOnce();
				}
			}
			return segment._slots[i].Item;
		}

		// Token: 0x06005ED6 RID: 24278 RVA: 0x0013E78D File Offset: 0x0013C98D
		private IEnumerator<T> Enumerate(ConcurrentQueue<T>.Segment head, int headHead, ConcurrentQueue<T>.Segment tail, int tailTail)
		{
			int headTail = ((head == tail) ? tailTail : Volatile.Read(ref head._headAndTail.Tail)) - head.FreezeOffset;
			if (headHead < headTail)
			{
				headHead &= head._slotsMask;
				headTail &= head._slotsMask;
				if (headHead < headTail)
				{
					int num;
					for (int i = headHead; i < headTail; i = num + 1)
					{
						yield return this.GetItemWhenAvailable(head, i);
						num = i;
					}
				}
				else
				{
					int num;
					for (int i = headHead; i < head._slots.Length; i = num + 1)
					{
						yield return this.GetItemWhenAvailable(head, i);
						num = i;
					}
					for (int i = 0; i < headTail; i = num + 1)
					{
						yield return this.GetItemWhenAvailable(head, i);
						num = i;
					}
				}
			}
			if (head != tail)
			{
				int num;
				ConcurrentQueue<T>.Segment s;
				for (s = head._nextSegment; s != tail; s = s._nextSegment)
				{
					int i = s._headAndTail.Tail - s.FreezeOffset;
					for (int j = 0; j < i; j = num + 1)
					{
						yield return this.GetItemWhenAvailable(s, j);
						num = j;
					}
				}
				s = null;
				tailTail -= tail.FreezeOffset;
				for (int i = 0; i < tailTail; i = num + 1)
				{
					yield return this.GetItemWhenAvailable(tail, i);
					num = i;
				}
			}
			yield break;
		}

		/// <summary>Adds an object to the end of the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" />.</summary>
		/// <param name="item">The object to add to the end of the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" />. The value can be a null reference (Nothing in Visual Basic) for reference types.</param>
		// Token: 0x06005ED7 RID: 24279 RVA: 0x0013E7B9 File Offset: 0x0013C9B9
		public void Enqueue(T item)
		{
			if (!this._tail.TryEnqueue(item))
			{
				this.EnqueueSlow(item);
			}
		}

		// Token: 0x06005ED8 RID: 24280 RVA: 0x0013E7D4 File Offset: 0x0013C9D4
		private void EnqueueSlow(T item)
		{
			for (;;)
			{
				ConcurrentQueue<T>.Segment tail = this._tail;
				if (tail.TryEnqueue(item))
				{
					break;
				}
				object crossSegmentLock = this._crossSegmentLock;
				lock (crossSegmentLock)
				{
					if (tail == this._tail)
					{
						tail.EnsureFrozenForEnqueues();
						ConcurrentQueue<T>.Segment segment = new ConcurrentQueue<T>.Segment(tail._preservedForObservation ? 32 : Math.Min(tail.Capacity * 2, 1048576));
						tail._nextSegment = segment;
						this._tail = segment;
					}
				}
			}
		}

		/// <summary>Tries to remove and return the object at the beginning of the concurrent queue.</summary>
		/// <param name="result">When this method returns, if the operation was successful, <paramref name="result" /> contains the object removed. If no object was available to be removed, the value is unspecified.</param>
		/// <returns>
		///   <see langword="true" /> if an element was removed and returned from the beginning of the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" /> successfully; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005ED9 RID: 24281 RVA: 0x0013E874 File Offset: 0x0013CA74
		public bool TryDequeue(out T result)
		{
			return this._head.TryDequeue(out result) || this.TryDequeueSlow(out result);
		}

		// Token: 0x06005EDA RID: 24282 RVA: 0x0013E890 File Offset: 0x0013CA90
		private bool TryDequeueSlow(out T item)
		{
			for (;;)
			{
				ConcurrentQueue<T>.Segment head = this._head;
				if (head.TryDequeue(out item))
				{
					break;
				}
				if (head._nextSegment == null)
				{
					goto Block_1;
				}
				if (head.TryDequeue(out item))
				{
					return true;
				}
				object crossSegmentLock = this._crossSegmentLock;
				lock (crossSegmentLock)
				{
					if (head == this._head)
					{
						this._head = head._nextSegment;
					}
				}
			}
			return true;
			Block_1:
			item = default(T);
			return false;
		}

		/// <summary>Tries to return an object from the beginning of the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" /> without removing it.</summary>
		/// <param name="result">When this method returns, <paramref name="result" /> contains an object from the beginning of the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" /> or an unspecified value if the operation failed.</param>
		/// <returns>
		///   <see langword="true" /> if an object was returned successfully; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005EDB RID: 24283 RVA: 0x0013E920 File Offset: 0x0013CB20
		public bool TryPeek(out T result)
		{
			return this.TryPeek(out result, true);
		}

		// Token: 0x06005EDC RID: 24284 RVA: 0x0013E92C File Offset: 0x0013CB2C
		private bool TryPeek(out T result, bool resultUsed)
		{
			ConcurrentQueue<T>.Segment segment = this._head;
			for (;;)
			{
				ConcurrentQueue<T>.Segment segment2 = Volatile.Read<ConcurrentQueue<T>.Segment>(ref segment._nextSegment);
				if (segment.TryPeek(out result, resultUsed))
				{
					break;
				}
				if (segment2 != null)
				{
					segment = segment2;
				}
				else if (Volatile.Read<ConcurrentQueue<T>.Segment>(ref segment._nextSegment) == null)
				{
					goto Block_3;
				}
			}
			return true;
			Block_3:
			result = default(T);
			return false;
		}

		// Token: 0x06005EDD RID: 24285 RVA: 0x0013E978 File Offset: 0x0013CB78
		public void Clear()
		{
			object crossSegmentLock = this._crossSegmentLock;
			lock (crossSegmentLock)
			{
				this._tail.EnsureFrozenForEnqueues();
				this._tail = (this._head = new ConcurrentQueue<T>.Segment(32));
			}
		}

		// Token: 0x0400391C RID: 14620
		private const int InitialSegmentLength = 32;

		// Token: 0x0400391D RID: 14621
		private const int MaxSegmentLength = 1048576;

		// Token: 0x0400391E RID: 14622
		private object _crossSegmentLock;

		// Token: 0x0400391F RID: 14623
		private volatile ConcurrentQueue<T>.Segment _tail;

		// Token: 0x04003920 RID: 14624
		private volatile ConcurrentQueue<T>.Segment _head;

		// Token: 0x02000A53 RID: 2643
		[DebuggerDisplay("Capacity = {Capacity}")]
		internal sealed class Segment
		{
			// Token: 0x06005EDE RID: 24286 RVA: 0x0013E9DC File Offset: 0x0013CBDC
			public Segment(int boundedLength)
			{
				this._slots = new ConcurrentQueue<T>.Segment.Slot[boundedLength];
				this._slotsMask = boundedLength - 1;
				for (int i = 0; i < this._slots.Length; i++)
				{
					this._slots[i].SequenceNumber = i;
				}
			}

			// Token: 0x06005EDF RID: 24287 RVA: 0x0013EA29 File Offset: 0x0013CC29
			internal static int RoundUpToPowerOf2(int i)
			{
				i--;
				i |= i >> 1;
				i |= i >> 2;
				i |= i >> 4;
				i |= i >> 8;
				i |= i >> 16;
				return i + 1;
			}

			// Token: 0x170010A4 RID: 4260
			// (get) Token: 0x06005EE0 RID: 24288 RVA: 0x0013EA57 File Offset: 0x0013CC57
			internal int Capacity
			{
				get
				{
					return this._slots.Length;
				}
			}

			// Token: 0x170010A5 RID: 4261
			// (get) Token: 0x06005EE1 RID: 24289 RVA: 0x0013EA61 File Offset: 0x0013CC61
			internal int FreezeOffset
			{
				get
				{
					return this._slots.Length * 2;
				}
			}

			// Token: 0x06005EE2 RID: 24290 RVA: 0x0013EA70 File Offset: 0x0013CC70
			internal void EnsureFrozenForEnqueues()
			{
				if (!this._frozenForEnqueues)
				{
					this._frozenForEnqueues = true;
					SpinWait spinWait = default(SpinWait);
					for (;;)
					{
						int num = Volatile.Read(ref this._headAndTail.Tail);
						if (Interlocked.CompareExchange(ref this._headAndTail.Tail, num + this.FreezeOffset, num) == num)
						{
							break;
						}
						spinWait.SpinOnce();
					}
				}
			}

			// Token: 0x06005EE3 RID: 24291 RVA: 0x0013EACC File Offset: 0x0013CCCC
			public bool TryDequeue(out T item)
			{
				SpinWait spinWait = default(SpinWait);
				int num;
				int num2;
				for (;;)
				{
					num = Volatile.Read(ref this._headAndTail.Head);
					num2 = num & this._slotsMask;
					int num3 = Volatile.Read(ref this._slots[num2].SequenceNumber) - (num + 1);
					if (num3 == 0)
					{
						if (Interlocked.CompareExchange(ref this._headAndTail.Head, num + 1, num) == num)
						{
							break;
						}
					}
					else if (num3 < 0)
					{
						bool frozenForEnqueues = this._frozenForEnqueues;
						int num4 = Volatile.Read(ref this._headAndTail.Tail);
						if (num4 - num <= 0 || (frozenForEnqueues && num4 - this.FreezeOffset - num <= 0))
						{
							goto IL_00EE;
						}
					}
					spinWait.SpinOnce();
				}
				item = this._slots[num2].Item;
				if (!Volatile.Read(ref this._preservedForObservation))
				{
					this._slots[num2].Item = default(T);
					Volatile.Write(ref this._slots[num2].SequenceNumber, num + this._slots.Length);
				}
				return true;
				IL_00EE:
				item = default(T);
				return false;
			}

			// Token: 0x06005EE4 RID: 24292 RVA: 0x0013EBDC File Offset: 0x0013CDDC
			public bool TryPeek(out T result, bool resultUsed)
			{
				if (resultUsed)
				{
					this._preservedForObservation = true;
					Interlocked.MemoryBarrier();
				}
				SpinWait spinWait = default(SpinWait);
				int num2;
				for (;;)
				{
					int num = Volatile.Read(ref this._headAndTail.Head);
					num2 = num & this._slotsMask;
					int num3 = Volatile.Read(ref this._slots[num2].SequenceNumber) - (num + 1);
					if (num3 == 0)
					{
						break;
					}
					if (num3 < 0)
					{
						bool frozenForEnqueues = this._frozenForEnqueues;
						int num4 = Volatile.Read(ref this._headAndTail.Tail);
						if (num4 - num <= 0 || (frozenForEnqueues && num4 - this.FreezeOffset - num <= 0))
						{
							goto IL_00AE;
						}
					}
					spinWait.SpinOnce();
				}
				result = (resultUsed ? this._slots[num2].Item : default(T));
				return true;
				IL_00AE:
				result = default(T);
				return false;
			}

			// Token: 0x06005EE5 RID: 24293 RVA: 0x0013ECAC File Offset: 0x0013CEAC
			public bool TryEnqueue(T item)
			{
				SpinWait spinWait = default(SpinWait);
				int num;
				int num2;
				for (;;)
				{
					num = Volatile.Read(ref this._headAndTail.Tail);
					num2 = num & this._slotsMask;
					int num3 = Volatile.Read(ref this._slots[num2].SequenceNumber) - num;
					if (num3 == 0)
					{
						if (Interlocked.CompareExchange(ref this._headAndTail.Tail, num + 1, num) == num)
						{
							break;
						}
					}
					else if (num3 < 0)
					{
						return false;
					}
					spinWait.SpinOnce();
				}
				this._slots[num2].Item = item;
				Volatile.Write(ref this._slots[num2].SequenceNumber, num + 1);
				return true;
			}

			// Token: 0x04003921 RID: 14625
			internal readonly ConcurrentQueue<T>.Segment.Slot[] _slots;

			// Token: 0x04003922 RID: 14626
			internal readonly int _slotsMask;

			// Token: 0x04003923 RID: 14627
			internal PaddedHeadAndTail _headAndTail;

			// Token: 0x04003924 RID: 14628
			internal bool _preservedForObservation;

			// Token: 0x04003925 RID: 14629
			internal bool _frozenForEnqueues;

			// Token: 0x04003926 RID: 14630
			internal ConcurrentQueue<T>.Segment _nextSegment;

			// Token: 0x02000A54 RID: 2644
			[DebuggerDisplay("Item = {Item}, SequenceNumber = {SequenceNumber}")]
			[StructLayout(LayoutKind.Auto)]
			internal struct Slot
			{
				// Token: 0x04003927 RID: 14631
				public T Item;

				// Token: 0x04003928 RID: 14632
				public int SequenceNumber;
			}
		}
	}
}
