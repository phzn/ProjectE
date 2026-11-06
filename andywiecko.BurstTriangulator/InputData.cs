using System;
using System.Runtime.CompilerServices;
using Unity.Collections;

namespace andywiecko.BurstTriangulator
{
	// Token: 0x0200000D RID: 13
	public class InputData<[IsUnmanaged] T2> where T2 : struct, ValueType
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000032 RID: 50 RVA: 0x00002539 File Offset: 0x00000739
		// (set) Token: 0x06000033 RID: 51 RVA: 0x00002541 File Offset: 0x00000741
		public NativeArray<T2> Positions { get; set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000034 RID: 52 RVA: 0x0000254A File Offset: 0x0000074A
		// (set) Token: 0x06000035 RID: 53 RVA: 0x00002552 File Offset: 0x00000752
		public NativeArray<int> ConstraintEdges { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000036 RID: 54 RVA: 0x0000255B File Offset: 0x0000075B
		// (set) Token: 0x06000037 RID: 55 RVA: 0x00002563 File Offset: 0x00000763
		public NativeArray<ConstraintType> ConstraintEdgeTypes { get; set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000038 RID: 56 RVA: 0x0000256C File Offset: 0x0000076C
		// (set) Token: 0x06000039 RID: 57 RVA: 0x00002574 File Offset: 0x00000774
		public NativeArray<T2> HoleSeeds { get; set; }
	}
}
