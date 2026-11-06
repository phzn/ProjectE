using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Pathfinding.Jobs
{
	// Token: 0x02000178 RID: 376
	[BurstCompile]
	public struct JobCopyRectangle<T> : IJob where T : struct
	{
		// Token: 0x06000BBB RID: 3003 RVA: 0x000410C2 File Offset: 0x0003F2C2
		public void Execute()
		{
			JobCopyRectangle<T>.Copy(this.input, this.output, this.inputSlice, this.outputSlice);
		}

		// Token: 0x06000BBC RID: 3004 RVA: 0x000410E4 File Offset: 0x0003F2E4
		public static void Copy(NativeArray<T> input, NativeArray<T> output, Slice3D inputSlice, Slice3D outputSlice)
		{
			inputSlice.AssertMatchesOuter<T>(input);
			outputSlice.AssertMatchesOuter<T>(output);
			inputSlice.AssertSameSize(outputSlice);
			if (inputSlice.coversEverything && outputSlice.coversEverything)
			{
				input.CopyTo(output);
				return;
			}
			for (int i = 0; i < outputSlice.slice.size.y; i++)
			{
				for (int j = 0; j < outputSlice.slice.size.z; j++)
				{
					int num = inputSlice.InnerCoordinateToOuterIndex(0, i, j);
					int num2 = outputSlice.InnerCoordinateToOuterIndex(0, i, j);
					NativeArray<T>.Copy(input, num, output, num2, outputSlice.slice.size.x);
				}
			}
		}

		// Token: 0x040007A2 RID: 1954
		[ReadOnly]
		[DisableUninitializedReadCheck]
		public NativeArray<T> input;

		// Token: 0x040007A3 RID: 1955
		[WriteOnly]
		public NativeArray<T> output;

		// Token: 0x040007A4 RID: 1956
		public Slice3D inputSlice;

		// Token: 0x040007A5 RID: 1957
		public Slice3D outputSlice;
	}
}
