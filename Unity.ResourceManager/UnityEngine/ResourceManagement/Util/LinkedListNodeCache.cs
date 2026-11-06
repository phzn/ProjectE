using System;
using System.Collections.Generic;

namespace UnityEngine.ResourceManagement.Util
{
	// Token: 0x02000024 RID: 36
	public class LinkedListNodeCache<T>
	{
		// Token: 0x060000F1 RID: 241 RVA: 0x00004D18 File Offset: 0x00002F18
		public LinkedListNode<T> Acquire(T val)
		{
			if (this.m_NodeCache != null)
			{
				LinkedListNode<T> first = this.m_NodeCache.First;
				if (first != null)
				{
					this.m_NodeCache.RemoveFirst();
					first.Value = val;
					return first;
				}
			}
			this.m_NodesCreated++;
			return new LinkedListNode<T>(val);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00004D64 File Offset: 0x00002F64
		public void Release(LinkedListNode<T> node)
		{
			if (this.m_NodeCache == null)
			{
				this.m_NodeCache = new LinkedList<T>();
			}
			node.Value = default(T);
			this.m_NodeCache.AddLast(node);
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x00004D9F File Offset: 0x00002F9F
		internal int CreatedNodeCount
		{
			get
			{
				return this.m_NodesCreated;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x00004DA7 File Offset: 0x00002FA7
		// (set) Token: 0x060000F5 RID: 245 RVA: 0x00004DC0 File Offset: 0x00002FC0
		internal int CachedNodeCount
		{
			get
			{
				if (this.m_NodeCache != null)
				{
					return this.m_NodeCache.Count;
				}
				return 0;
			}
			set
			{
				if (this.m_NodeCache == null)
				{
					this.m_NodeCache = new LinkedList<T>();
				}
				while (value < this.m_NodeCache.Count)
				{
					this.m_NodeCache.RemoveLast();
				}
				while (value > this.m_NodeCache.Count)
				{
					this.m_NodeCache.AddLast(new LinkedListNode<T>(default(T)));
				}
			}
		}

		// Token: 0x04000057 RID: 87
		private int m_NodesCreated;

		// Token: 0x04000058 RID: 88
		private LinkedList<T> m_NodeCache;
	}
}
