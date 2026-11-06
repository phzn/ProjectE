using System;
using System.Runtime.CompilerServices;
using Unity.Collections;

namespace andywiecko.BurstTriangulator.LowLevel.Unsafe
{
	// Token: 0x02000016 RID: 22
	public struct OutputData<[IsUnmanaged] T2> where T2 : struct, ValueType
	{
		// Token: 0x0400004E RID: 78
		public NativeList<T2> Positions;

		// Token: 0x0400004F RID: 79
		public NativeList<int> Triangles;

		// Token: 0x04000050 RID: 80
		public NativeReference<Status> Status;

		// Token: 0x04000051 RID: 81
		public NativeList<int> Halfedges;

		// Token: 0x04000052 RID: 82
		public NativeList<HalfedgeState> ConstrainedHalfedges;
	}
}
