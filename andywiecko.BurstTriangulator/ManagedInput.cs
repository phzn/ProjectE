using System;
using System.Runtime.CompilerServices;
using Unity.Collections;

namespace andywiecko.BurstTriangulator
{
	// Token: 0x0200000E RID: 14
	[Obsolete("Use AsNativeArray(out Handle) instead! You can learn more in the project manual.")]
	public class ManagedInput<[IsUnmanaged] T2> where T2 : struct, ValueType
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600003B RID: 59 RVA: 0x0000257D File Offset: 0x0000077D
		// (set) Token: 0x0600003C RID: 60 RVA: 0x00002585 File Offset: 0x00000785
		public T2[] Positions { get; set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600003D RID: 61 RVA: 0x0000258E File Offset: 0x0000078E
		// (set) Token: 0x0600003E RID: 62 RVA: 0x00002596 File Offset: 0x00000796
		public int[] ConstraintEdges { get; set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600003F RID: 63 RVA: 0x0000259F File Offset: 0x0000079F
		// (set) Token: 0x06000040 RID: 64 RVA: 0x000025A7 File Offset: 0x000007A7
		public T2[] HoleSeeds { get; set; }

		// Token: 0x06000041 RID: 65 RVA: 0x000025B0 File Offset: 0x000007B0
		public static implicit operator InputData<T2>(ManagedInput<T2> input)
		{
			return new InputData<T2>
			{
				Positions = ((input.Positions == null) ? default(NativeArray<T2>) : input.Positions.AsNativeArray<T2>()),
				ConstraintEdges = ((input.ConstraintEdges == null) ? default(NativeArray<int>) : input.ConstraintEdges.AsNativeArray<int>()),
				HoleSeeds = ((input.HoleSeeds == null) ? default(NativeArray<T2>) : input.HoleSeeds.AsNativeArray<T2>())
			};
		}
	}
}
