using System;
using System.Runtime.CompilerServices;
using Unity.Collections;

namespace andywiecko.BurstTriangulator.LowLevel.Unsafe
{
	// Token: 0x02000026 RID: 38
	internal interface ITransform<TSelf, [IsUnmanaged] T, [IsUnmanaged] T2> where T : struct, ValueType where T2 : struct, ValueType
	{
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000C6 RID: 198
		T AreaScalingFactor { get; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000C7 RID: 199
		TSelf Identity { get; }

		// Token: 0x060000C8 RID: 200
		TSelf Inverse();

		// Token: 0x060000C9 RID: 201
		T2 Transform(T2 point);

		// Token: 0x060000CA RID: 202
		TSelf CalculatePCATransformation(NativeArray<T2> positions);

		// Token: 0x060000CB RID: 203
		TSelf CalculateLocalTransformation(NativeArray<T2> positions);
	}
}
