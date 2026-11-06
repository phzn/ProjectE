using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000052 RID: 82
	public sealed class ListChangedEventArgs<T> : EventArgs
	{
		// Token: 0x060002AA RID: 682 RVA: 0x0000C542 File Offset: 0x0000A742
		public ListChangedEventArgs(int index, T item)
		{
			this.index = index;
			this.item = item;
		}

		// Token: 0x0400019A RID: 410
		public readonly int index;

		// Token: 0x0400019B RID: 411
		public readonly T item;
	}
}
