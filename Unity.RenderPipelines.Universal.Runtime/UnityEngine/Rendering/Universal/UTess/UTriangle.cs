using System;
using Unity.Mathematics;

namespace UnityEngine.Rendering.Universal.UTess
{
	// Token: 0x02000135 RID: 309
	internal struct UTriangle
	{
		// Token: 0x04000904 RID: 2308
		public float2 va;

		// Token: 0x04000905 RID: 2309
		public float2 vb;

		// Token: 0x04000906 RID: 2310
		public float2 vc;

		// Token: 0x04000907 RID: 2311
		public UCircle c;

		// Token: 0x04000908 RID: 2312
		public float area;

		// Token: 0x04000909 RID: 2313
		public int3 indices;
	}
}
