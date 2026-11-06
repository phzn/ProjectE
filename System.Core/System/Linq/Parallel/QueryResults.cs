using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Linq.Parallel
{
	// Token: 0x0200018B RID: 395
	internal abstract class QueryResults<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
	{
		// Token: 0x06000A82 RID: 2690
		internal abstract void GivePartitionedStream(IPartitionedStreamRecipient<T> recipient);

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000A83 RID: 2691 RVA: 0x000023D1 File Offset: 0x000005D1
		internal virtual bool IsIndexible
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000A84 RID: 2692 RVA: 0x000080E3 File Offset: 0x000062E3
		internal virtual T GetElement(int index)
		{
			throw new NotSupportedException();
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000A85 RID: 2693 RVA: 0x000080E3 File Offset: 0x000062E3
		internal virtual int ElementsCount
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000A86 RID: 2694 RVA: 0x000080E3 File Offset: 0x000062E3
		int IList<T>.IndexOf(T item)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x000080E3 File Offset: 0x000062E3
		void IList<T>.Insert(int index, T item)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x000080E3 File Offset: 0x000062E3
		void IList<T>.RemoveAt(int index)
		{
			throw new NotSupportedException();
		}

		// Token: 0x1700012B RID: 299
		public T this[int index]
		{
			get
			{
				return this.GetElement(index);
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x000080E3 File Offset: 0x000062E3
		void ICollection<T>.Add(T item)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x000080E3 File Offset: 0x000062E3
		void ICollection<T>.Clear()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000A8D RID: 2701 RVA: 0x000080E3 File Offset: 0x000062E3
		bool ICollection<T>.Contains(T item)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000A8E RID: 2702 RVA: 0x000080E3 File Offset: 0x000062E3
		void ICollection<T>.CopyTo(T[] array, int arrayIndex)
		{
			throw new NotSupportedException();
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000A8F RID: 2703 RVA: 0x00025327 File Offset: 0x00023527
		public int Count
		{
			get
			{
				return this.ElementsCount;
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000A90 RID: 2704 RVA: 0x00007E1D File Offset: 0x0000601D
		bool ICollection<T>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000A91 RID: 2705 RVA: 0x000080E3 File Offset: 0x000062E3
		bool ICollection<T>.Remove(T item)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000A92 RID: 2706 RVA: 0x0002532F File Offset: 0x0002352F
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			int num;
			for (int index = 0; index < this.Count; index = num + 1)
			{
				yield return this[index];
				num = index;
			}
			yield break;
		}

		// Token: 0x06000A93 RID: 2707 RVA: 0x0000817A File Offset: 0x0000637A
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<T>)this).GetEnumerator();
		}
	}
}
