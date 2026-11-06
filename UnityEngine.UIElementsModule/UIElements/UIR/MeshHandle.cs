using System;

namespace UnityEngine.UIElements.UIR
{
	// Token: 0x0200042C RID: 1068
	internal class MeshHandle : LinkedPoolItem<MeshHandle>
	{
		// Token: 0x04000E80 RID: 3712
		internal Alloc allocVerts;

		// Token: 0x04000E81 RID: 3713
		internal Alloc allocIndices;

		// Token: 0x04000E82 RID: 3714
		internal uint triangleCount;

		// Token: 0x04000E83 RID: 3715
		internal Page allocPage;

		// Token: 0x04000E84 RID: 3716
		internal uint allocTime;

		// Token: 0x04000E85 RID: 3717
		internal uint updateAllocID;
	}
}
