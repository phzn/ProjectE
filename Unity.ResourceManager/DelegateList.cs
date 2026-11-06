using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.Util;

// Token: 0x02000004 RID: 4
internal class DelegateList<T>
{
	// Token: 0x06000003 RID: 3 RVA: 0x00002060 File Offset: 0x00000260
	public DelegateList(Func<Action<T>, LinkedListNode<Action<T>>> acquireFunc, Action<LinkedListNode<Action<T>>> releaseFunc)
	{
		if (acquireFunc == null)
		{
			throw new ArgumentNullException("acquireFunc");
		}
		if (releaseFunc == null)
		{
			throw new ArgumentNullException("releaseFunc");
		}
		this.m_acquireFunc = acquireFunc;
		this.m_releaseFunc = releaseFunc;
	}

	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000004 RID: 4 RVA: 0x00002092 File Offset: 0x00000292
	public int Count
	{
		get
		{
			if (this.m_callbacks != null)
			{
				return this.m_callbacks.Count;
			}
			return 0;
		}
	}

	// Token: 0x06000005 RID: 5 RVA: 0x000020AC File Offset: 0x000002AC
	public void Add(Action<T> action)
	{
		LinkedListNode<Action<T>> linkedListNode = this.m_acquireFunc(action);
		if (this.m_callbacks == null)
		{
			this.m_callbacks = new LinkedList<Action<T>>();
		}
		this.m_callbacks.AddLast(linkedListNode);
	}

	// Token: 0x06000006 RID: 6 RVA: 0x000020E8 File Offset: 0x000002E8
	public void Remove(Action<T> action)
	{
		if (this.m_callbacks == null)
		{
			return;
		}
		LinkedListNode<Action<T>> linkedListNode = this.m_callbacks.First;
		while (linkedListNode != null)
		{
			if (linkedListNode.Value == action)
			{
				if (this.m_invoking)
				{
					linkedListNode.Value = null;
					return;
				}
				this.m_callbacks.Remove(linkedListNode);
				this.m_releaseFunc(linkedListNode);
				return;
			}
			else
			{
				linkedListNode = linkedListNode.Next;
			}
		}
	}

	// Token: 0x06000007 RID: 7 RVA: 0x00002150 File Offset: 0x00000350
	public void Invoke(T res)
	{
		if (this.m_callbacks == null)
		{
			return;
		}
		this.m_invoking = true;
		for (LinkedListNode<Action<T>> linkedListNode = this.m_callbacks.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
		{
			if (linkedListNode.Value != null)
			{
				try
				{
					linkedListNode.Value(res);
				}
				catch (Exception ex)
				{
					Debug.LogException(ex);
				}
			}
		}
		this.m_invoking = false;
		LinkedListNode<Action<T>> next;
		for (LinkedListNode<Action<T>> linkedListNode2 = this.m_callbacks.First; linkedListNode2 != null; linkedListNode2 = next)
		{
			next = linkedListNode2.Next;
			if (linkedListNode2.Value == null)
			{
				this.m_callbacks.Remove(linkedListNode2);
				this.m_releaseFunc(linkedListNode2);
			}
		}
	}

	// Token: 0x06000008 RID: 8 RVA: 0x000021F4 File Offset: 0x000003F4
	public void Clear()
	{
		if (this.m_callbacks == null)
		{
			return;
		}
		LinkedListNode<Action<T>> next;
		for (LinkedListNode<Action<T>> linkedListNode = this.m_callbacks.First; linkedListNode != null; linkedListNode = next)
		{
			next = linkedListNode.Next;
			this.m_callbacks.Remove(linkedListNode);
			this.m_releaseFunc(linkedListNode);
		}
	}

	// Token: 0x06000009 RID: 9 RVA: 0x0000223A File Offset: 0x0000043A
	public static DelegateList<T> CreateWithGlobalCache()
	{
		if (!GlobalLinkedListNodeCache<Action<T>>.CacheExists)
		{
			GlobalLinkedListNodeCache<Action<T>>.SetCacheSize(32);
		}
		return new DelegateList<T>(new Func<Action<T>, LinkedListNode<Action<T>>>(GlobalLinkedListNodeCache<Action<T>>.Acquire), new Action<LinkedListNode<Action<T>>>(GlobalLinkedListNodeCache<Action<T>>.Release));
	}

	// Token: 0x04000001 RID: 1
	private Func<Action<T>, LinkedListNode<Action<T>>> m_acquireFunc;

	// Token: 0x04000002 RID: 2
	private Action<LinkedListNode<Action<T>>> m_releaseFunc;

	// Token: 0x04000003 RID: 3
	private LinkedList<Action<T>> m_callbacks;

	// Token: 0x04000004 RID: 4
	private bool m_invoking;
}
