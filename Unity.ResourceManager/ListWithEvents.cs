using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x02000005 RID: 5
internal class ListWithEvents<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
{
	// Token: 0x14000001 RID: 1
	// (add) Token: 0x0600000A RID: 10 RVA: 0x00002268 File Offset: 0x00000468
	// (remove) Token: 0x0600000B RID: 11 RVA: 0x000022A0 File Offset: 0x000004A0
	public event Action<T> OnElementAdded;

	// Token: 0x14000002 RID: 2
	// (add) Token: 0x0600000C RID: 12 RVA: 0x000022D8 File Offset: 0x000004D8
	// (remove) Token: 0x0600000D RID: 13 RVA: 0x00002310 File Offset: 0x00000510
	public event Action<T> OnElementRemoved;

	// Token: 0x0600000E RID: 14 RVA: 0x00002345 File Offset: 0x00000545
	private void InvokeAdded(T element)
	{
		Action<T> onElementAdded = this.OnElementAdded;
		if (onElementAdded == null)
		{
			return;
		}
		onElementAdded(element);
	}

	// Token: 0x0600000F RID: 15 RVA: 0x00002358 File Offset: 0x00000558
	private void InvokeRemoved(T element)
	{
		Action<T> onElementRemoved = this.OnElementRemoved;
		if (onElementRemoved == null)
		{
			return;
		}
		onElementRemoved(element);
	}

	// Token: 0x17000002 RID: 2
	public T this[int index]
	{
		get
		{
			return this.m_List[index];
		}
		set
		{
			T t = this.m_List[index];
			this.m_List[index] = value;
			this.InvokeRemoved(t);
			this.InvokeAdded(value);
		}
	}

	// Token: 0x17000003 RID: 3
	// (get) Token: 0x06000012 RID: 18 RVA: 0x000023B1 File Offset: 0x000005B1
	public int Count
	{
		get
		{
			return this.m_List.Count;
		}
	}

	// Token: 0x17000004 RID: 4
	// (get) Token: 0x06000013 RID: 19 RVA: 0x000023BE File Offset: 0x000005BE
	public bool IsReadOnly
	{
		get
		{
			return ((ICollection<T>)this.m_List).IsReadOnly;
		}
	}

	// Token: 0x06000014 RID: 20 RVA: 0x000023CB File Offset: 0x000005CB
	public void Add(T item)
	{
		this.m_List.Add(item);
		this.InvokeAdded(item);
	}

	// Token: 0x06000015 RID: 21 RVA: 0x000023E0 File Offset: 0x000005E0
	public void Clear()
	{
		foreach (T t in this.m_List)
		{
			this.InvokeRemoved(t);
		}
		this.m_List.Clear();
	}

	// Token: 0x06000016 RID: 22 RVA: 0x00002440 File Offset: 0x00000640
	public bool Contains(T item)
	{
		return this.m_List.Contains(item);
	}

	// Token: 0x06000017 RID: 23 RVA: 0x0000244E File Offset: 0x0000064E
	public void CopyTo(T[] array, int arrayIndex)
	{
		this.m_List.CopyTo(array, arrayIndex);
	}

	// Token: 0x06000018 RID: 24 RVA: 0x0000245D File Offset: 0x0000065D
	public IEnumerator<T> GetEnumerator()
	{
		return this.m_List.GetEnumerator();
	}

	// Token: 0x06000019 RID: 25 RVA: 0x0000246F File Offset: 0x0000066F
	public int IndexOf(T item)
	{
		return this.m_List.IndexOf(item);
	}

	// Token: 0x0600001A RID: 26 RVA: 0x0000247D File Offset: 0x0000067D
	public void Insert(int index, T item)
	{
		this.m_List.Insert(index, item);
		this.InvokeAdded(item);
	}

	// Token: 0x0600001B RID: 27 RVA: 0x00002493 File Offset: 0x00000693
	public bool Remove(T item)
	{
		bool flag = this.m_List.Remove(item);
		if (flag)
		{
			this.InvokeRemoved(item);
		}
		return flag;
	}

	// Token: 0x0600001C RID: 28 RVA: 0x000024AC File Offset: 0x000006AC
	public void RemoveAt(int index)
	{
		T t = this.m_List[index];
		this.m_List.RemoveAt(index);
		this.InvokeRemoved(t);
	}

	// Token: 0x0600001D RID: 29 RVA: 0x000024D9 File Offset: 0x000006D9
	IEnumerator IEnumerable.GetEnumerator()
	{
		return ((IEnumerable)this.m_List).GetEnumerator();
	}

	// Token: 0x04000005 RID: 5
	private List<T> m_List = new List<T>();
}
