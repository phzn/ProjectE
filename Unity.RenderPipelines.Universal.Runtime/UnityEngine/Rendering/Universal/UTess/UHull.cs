using System;
using Unity.Mathematics;

namespace UnityEngine.Rendering.Universal.UTess
{
	// Token: 0x02000131 RID: 305
	internal struct UHull
	{
		// Token: 0x040008F7 RID: 2295
		public float2 a;

		// Token: 0x040008F8 RID: 2296
		public float2 b;

		// Token: 0x040008F9 RID: 2297
		public int idx;

		// Token: 0x040008FA RID: 2298
		public ArraySlice<int> ilarray;

		// Token: 0x040008FB RID: 2299
		public int ilcount;

		// Token: 0x040008FC RID: 2300
		public ArraySlice<int> iuarray;

		// Token: 0x040008FD RID: 2301
		public int iucount;
	}
}
