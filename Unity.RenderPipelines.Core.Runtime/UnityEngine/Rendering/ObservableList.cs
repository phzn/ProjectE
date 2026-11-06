using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine.Rendering
{
	// Token: 0x02000054 RID: 84
	public class ObservableList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
	{
		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060002AF RID: 687 RVA: 0x0000C558 File Offset: 0x0000A758
		// (remove) Token: 0x060002B0 RID: 688 RVA: 0x0000C590 File Offset: 0x0000A790
		public event ListChangedEventHandler<T> ItemAdded;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060002B1 RID: 689 RVA: 0x0000C5C8 File Offset: 0x0000A7C8
		// (remove) Token: 0x060002B2 RID: 690 RVA: 0x0000C600 File Offset: 0x0000A800
		public event ListChangedEventHandler<T> ItemRemoved;

		// Token: 0x17000050 RID: 80
		public T this[int index]
		{
			get
			{
				return this.m_List[index];
			}
			set
			{
				this.OnEvent(this.ItemRemoved, index, this.m_List[index]);
				this.m_List[index] = value;
				this.OnEvent(this.ItemAdded, index, value);
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060002B5 RID: 693 RVA: 0x0000C679 File Offset: 0x0000A879
		public int Count
		{
			get
			{
				return this.m_List.Count;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060002B6 RID: 694 RVA: 0x0000C686 File Offset: 0x0000A886
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000C689 File Offset: 0x0000A889
		public ObservableList()
			: this(0)
		{
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000C692 File Offset: 0x0000A892
		public ObservableList(int capacity)
		{
			this.m_List = new List<T>(capacity);
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000C6A6 File Offset: 0x0000A8A6
		public ObservableList(IEnumerable<T> collection)
		{
			this.m_List = new List<T>(collection);
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000C6BA File Offset: 0x0000A8BA
		private void OnEvent(ListChangedEventHandler<T> e, int index, T item)
		{
			if (e != null)
			{
				e(this, new ListChangedEventArgs<T>(index, item));
			}
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000C6CD File Offset: 0x0000A8CD
		public bool Contains(T item)
		{
			return this.m_List.Contains(item);
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000C6DB File Offset: 0x0000A8DB
		public int IndexOf(T item)
		{
			return this.m_List.IndexOf(item);
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000C6E9 File Offset: 0x0000A8E9
		public void Add(T item)
		{
			this.m_List.Add(item);
			this.OnEvent(this.ItemAdded, this.m_List.IndexOf(item), item);
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000C710 File Offset: 0x0000A910
		public void Add(params T[] items)
		{
			foreach (T t in items)
			{
				this.Add(t);
			}
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000C73C File Offset: 0x0000A93C
		public void Insert(int index, T item)
		{
			this.m_List.Insert(index, item);
			this.OnEvent(this.ItemAdded, index, item);
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000C75C File Offset: 0x0000A95C
		public bool Remove(T item)
		{
			int num = this.m_List.IndexOf(item);
			bool flag = this.m_List.Remove(item);
			if (flag)
			{
				this.OnEvent(this.ItemRemoved, num, item);
			}
			return flag;
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000C794 File Offset: 0x0000A994
		public int Remove(params T[] items)
		{
			if (items == null)
			{
				return 0;
			}
			int num = 0;
			foreach (T t in items)
			{
				num += (this.Remove(t) ? 1 : 0);
			}
			return num;
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000C7D4 File Offset: 0x0000A9D4
		public void RemoveAt(int index)
		{
			T t = this.m_List[index];
			this.m_List.RemoveAt(index);
			this.OnEvent(this.ItemRemoved, index, t);
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000C808 File Offset: 0x0000AA08
		public void Clear()
		{
			while (this.Count > 0)
			{
				this.RemoveAt(this.Count - 1);
			}
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000C823 File Offset: 0x0000AA23
		public void CopyTo(T[] array, int arrayIndex)
		{
			this.m_List.CopyTo(array, arrayIndex);
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000C832 File Offset: 0x0000AA32
		public IEnumerator<T> GetEnumerator()
		{
			return this.m_List.GetEnumerator();
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000C83F File Offset: 0x0000AA3F
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0400019C RID: 412
		private IList<T> m_List;
	}
}
