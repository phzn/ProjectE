using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000E2 RID: 226
	[BurstCompile(FloatMode = FloatMode.Fast, DisableSafetyChecks = true, OptimizeFor = OptimizeFor.Performance)]
	internal struct ZBinningJob : IJobFor
	{
		// Token: 0x06000707 RID: 1799 RVA: 0x0002C76D File Offset: 0x0002A96D
		private static uint EncodeHeader(uint min, uint max)
		{
			return (min & 65535U) | ((max & 65535U) << 16);
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x0002C781 File Offset: 0x0002A981
		private static ValueTuple<uint, uint> DecodeHeader(uint zBin)
		{
			return new ValueTuple<uint, uint>(zBin & 65535U, (zBin >> 16) & 65535U);
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x0002C79C File Offset: 0x0002A99C
		public void Execute(int jobIndex)
		{
			int num = jobIndex % this.batchCount;
			int num2 = jobIndex / this.batchCount;
			int num3 = 128 * num;
			int num4 = math.min(num3 + 128, this.binCount) - 1;
			int num5 = num2 * this.binCount;
			uint num6 = ZBinningJob.EncodeHeader(65535U, 0U);
			for (int i = num3; i <= num4; i++)
			{
				this.bins[(num5 + i) * (2 + this.wordsPerTile)] = num6;
				this.bins[(num5 + i) * (2 + this.wordsPerTile) + 1] = num6;
			}
			this.FillZBins(num3, num4, 0, this.lightCount, 0, num2 * this.lightCount, num5);
			this.FillZBins(num3, num4, this.lightCount, this.lightCount + this.reflectionProbeCount, 1, this.lightCount * (this.viewCount - 1) + num2 * this.reflectionProbeCount, num5);
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x0002C888 File Offset: 0x0002AA88
		private void FillZBins(int binStart, int binEnd, int itemStart, int itemEnd, int headerIndex, int itemOffset, int binOffset)
		{
			for (int i = itemStart; i < itemEnd; i++)
			{
				float2 @float = this.minMaxZs[itemOffset + i];
				int num = math.max((int)((this.isOrthographic ? @float.x : math.log2(@float.x)) * this.zBinScale + this.zBinOffset), binStart);
				int num2 = math.min((int)((this.isOrthographic ? @float.y : math.log2(@float.y)) * this.zBinScale + this.zBinOffset), binEnd);
				int num3 = i / 32;
				uint num4 = 1U << i % 32;
				for (int j = num; j <= num2; j++)
				{
					int num5 = (binOffset + j) * (2 + this.wordsPerTile);
					ValueTuple<uint, uint> valueTuple = ZBinningJob.DecodeHeader(this.bins[num5 + headerIndex]);
					uint num6 = valueTuple.Item1;
					uint num7 = valueTuple.Item2;
					num6 = math.min(num6, (uint)i);
					num7 = math.max(num7, (uint)i);
					this.bins[num5 + headerIndex] = ZBinningJob.EncodeHeader(num6, num7);
					ref NativeArray<uint> ptr = ref this.bins;
					int num8 = num5 + 2 + num3;
					ptr[num8] |= num4;
				}
			}
		}

		// Token: 0x04000595 RID: 1429
		public const int batchSize = 128;

		// Token: 0x04000596 RID: 1430
		public const int headerLength = 2;

		// Token: 0x04000597 RID: 1431
		[NativeDisableParallelForRestriction]
		public NativeArray<uint> bins;

		// Token: 0x04000598 RID: 1432
		[ReadOnly]
		public NativeArray<float2> minMaxZs;

		// Token: 0x04000599 RID: 1433
		public float zBinScale;

		// Token: 0x0400059A RID: 1434
		public float zBinOffset;

		// Token: 0x0400059B RID: 1435
		public int binCount;

		// Token: 0x0400059C RID: 1436
		public int wordsPerTile;

		// Token: 0x0400059D RID: 1437
		public int lightCount;

		// Token: 0x0400059E RID: 1438
		public int reflectionProbeCount;

		// Token: 0x0400059F RID: 1439
		public int batchCount;

		// Token: 0x040005A0 RID: 1440
		public int viewCount;

		// Token: 0x040005A1 RID: 1441
		public bool isOrthographic;
	}
}
