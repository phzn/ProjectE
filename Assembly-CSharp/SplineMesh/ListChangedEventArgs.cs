using System;
using System.Collections.Generic;

namespace SplineMesh
{
	// Token: 0x02000184 RID: 388
	public class ListChangedEventArgs<T> : EventArgs
	{
		// Token: 0x04000C1C RID: 3100
		public ListChangeType type;

		// Token: 0x04000C1D RID: 3101
		public List<T> newItems;

		// Token: 0x04000C1E RID: 3102
		public List<T> removedItems;

		// Token: 0x04000C1F RID: 3103
		public int insertIndex;

		// Token: 0x04000C20 RID: 3104
		public int removeIndex;
	}
}
