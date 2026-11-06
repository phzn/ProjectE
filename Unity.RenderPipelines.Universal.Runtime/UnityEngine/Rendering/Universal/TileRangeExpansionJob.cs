using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000DE RID: 222
	[BurstCompile(FloatMode = FloatMode.Fast, DisableSafetyChecks = true, OptimizeFor = OptimizeFor.Performance)]
	internal struct TileRangeExpansionJob : IJobFor
	{
		// Token: 0x060006ED RID: 1773 RVA: 0x0002971C File Offset: 0x0002791C
		public void Execute(int jobIndex)
		{
			int num = jobIndex % this.tileResolution.y;
			int num2 = jobIndex / this.tileResolution.y;
			int num3 = 0;
			NativeArray<short> nativeArray = new NativeArray<short>(this.itemsPerTile, Allocator.Temp, NativeArrayOptions.ClearMemory);
			NativeArray<InclusiveRange> nativeArray2 = new NativeArray<InclusiveRange>(this.itemsPerTile, Allocator.Temp, NativeArrayOptions.ClearMemory);
			for (int i = 0; i < this.itemsPerTile; i++)
			{
				InclusiveRange inclusiveRange = this.tileRanges[num2 * this.rangesPerItem * this.itemsPerTile + i * this.rangesPerItem + 1 + num];
				if (!inclusiveRange.isEmpty)
				{
					nativeArray[num3] = (short)i;
					nativeArray2[num3] = inclusiveRange;
					num3++;
				}
			}
			int num4 = num2 * this.wordsPerTile * this.tileResolution.x * this.tileResolution.y + num * this.wordsPerTile * this.tileResolution.x;
			for (int j = 0; j < this.tileResolution.x; j++)
			{
				int num5 = num4 + j * this.wordsPerTile;
				for (int k = 0; k < num3; k++)
				{
					int num6 = (int)nativeArray[k];
					int num7 = num6 / 32;
					uint num8 = 1U << num6 % 32;
					if (nativeArray2[k].Contains((short)j))
					{
						ref NativeArray<uint> ptr = ref this.tileMasks;
						int num9 = num5 + num7;
						ptr[num9] |= num8;
					}
				}
			}
			nativeArray.Dispose();
			nativeArray2.Dispose();
		}

		// Token: 0x04000576 RID: 1398
		[ReadOnly]
		public NativeArray<InclusiveRange> tileRanges;

		// Token: 0x04000577 RID: 1399
		[NativeDisableParallelForRestriction]
		public NativeArray<uint> tileMasks;

		// Token: 0x04000578 RID: 1400
		public int rangesPerItem;

		// Token: 0x04000579 RID: 1401
		public int itemsPerTile;

		// Token: 0x0400057A RID: 1402
		public int wordsPerTile;

		// Token: 0x0400057B RID: 1403
		public int2 tileResolution;
	}
}
