using System;

namespace UnityEngine.UIElements.UIR
{
	// Token: 0x0200044D RID: 1101
	internal class BasicNodePool<T> : LinkedPool<BasicNode<T>>
	{
		// Token: 0x06002284 RID: 8836 RVA: 0x00084E5B File Offset: 0x0008305B
		private static void Reset(BasicNode<T> node)
		{
			node.next = null;
			node.data = default(T);
		}

		// Token: 0x06002285 RID: 8837 RVA: 0x00084E74 File Offset: 0x00083074
		private static BasicNode<T> Create()
		{
			return new BasicNode<T>();
		}

		// Token: 0x06002286 RID: 8838 RVA: 0x00084E8B File Offset: 0x0008308B
		public BasicNodePool()
			: base(new Func<BasicNode<T>>(BasicNodePool<T>.Create), new Action<BasicNode<T>>(BasicNodePool<T>.Reset), 10000)
		{
		}
	}
}
