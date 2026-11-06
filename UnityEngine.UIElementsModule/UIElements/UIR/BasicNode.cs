using System;

namespace UnityEngine.UIElements.UIR
{
	// Token: 0x0200044C RID: 1100
	internal class BasicNode<T> : LinkedPoolItem<BasicNode<T>>
	{
		// Token: 0x06002282 RID: 8834 RVA: 0x00084E20 File Offset: 0x00083020
		public void InsertFirst(ref BasicNode<T> first)
		{
			bool flag = first == null;
			if (flag)
			{
				first = this;
			}
			else
			{
				this.next = first.next;
				first.next = this;
			}
		}

		// Token: 0x04000F6A RID: 3946
		public BasicNode<T> next;

		// Token: 0x04000F6B RID: 3947
		public T data;
	}
}
