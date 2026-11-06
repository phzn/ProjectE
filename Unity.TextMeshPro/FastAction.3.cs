using System;
using System.Collections.Generic;

namespace TMPro
{
	// Token: 0x02000005 RID: 5
	public class FastAction<A, B>
	{
		// Token: 0x0600000B RID: 11 RVA: 0x00002223 File Offset: 0x00000423
		public void Add(Action<A, B> rhs)
		{
			if (this.lookup.ContainsKey(rhs))
			{
				return;
			}
			this.lookup[rhs] = this.delegates.AddLast(rhs);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000224C File Offset: 0x0000044C
		public void Remove(Action<A, B> rhs)
		{
			LinkedListNode<Action<A, B>> linkedListNode;
			if (this.lookup.TryGetValue(rhs, out linkedListNode))
			{
				this.lookup.Remove(rhs);
				this.delegates.Remove(linkedListNode);
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002284 File Offset: 0x00000484
		public void Call(A a, B b)
		{
			for (LinkedListNode<Action<A, B>> linkedListNode = this.delegates.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				linkedListNode.Value(a, b);
			}
		}

		// Token: 0x04000005 RID: 5
		private LinkedList<Action<A, B>> delegates = new LinkedList<Action<A, B>>();

		// Token: 0x04000006 RID: 6
		private Dictionary<Action<A, B>, LinkedListNode<Action<A, B>>> lookup = new Dictionary<Action<A, B>, LinkedListNode<Action<A, B>>>();
	}
}
