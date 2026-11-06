using System;
using System.Collections.Generic;

namespace TMPro
{
	// Token: 0x02000004 RID: 4
	public class FastAction<A>
	{
		// Token: 0x06000007 RID: 7 RVA: 0x00002172 File Offset: 0x00000372
		public void Add(Action<A> rhs)
		{
			if (this.lookup.ContainsKey(rhs))
			{
				return;
			}
			this.lookup[rhs] = this.delegates.AddLast(rhs);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000219C File Offset: 0x0000039C
		public void Remove(Action<A> rhs)
		{
			LinkedListNode<Action<A>> linkedListNode;
			if (this.lookup.TryGetValue(rhs, out linkedListNode))
			{
				this.lookup.Remove(rhs);
				this.delegates.Remove(linkedListNode);
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000021D4 File Offset: 0x000003D4
		public void Call(A a)
		{
			for (LinkedListNode<Action<A>> linkedListNode = this.delegates.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				linkedListNode.Value(a);
			}
		}

		// Token: 0x04000003 RID: 3
		private LinkedList<Action<A>> delegates = new LinkedList<Action<A>>();

		// Token: 0x04000004 RID: 4
		private Dictionary<Action<A>, LinkedListNode<Action<A>>> lookup = new Dictionary<Action<A>, LinkedListNode<Action<A>>>();
	}
}
