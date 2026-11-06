using System;
using System.Runtime.CompilerServices;
using Unity.Collections;

namespace andywiecko.BurstTriangulator.LowLevel.Unsafe
{
	// Token: 0x02000015 RID: 21
	public struct InputData<[IsUnmanaged] T2> where T2 : struct, ValueType
	{
		// Token: 0x0400004A RID: 74
		public NativeArray<T2> Positions;

		// Token: 0x0400004B RID: 75
		public NativeArray<int> ConstraintEdges;

		// Token: 0x0400004C RID: 76
		public NativeArray<ConstraintType> ConstraintEdgeTypes;

		// Token: 0x0400004D RID: 77
		public NativeArray<T2> HoleSeeds;
	}
}
