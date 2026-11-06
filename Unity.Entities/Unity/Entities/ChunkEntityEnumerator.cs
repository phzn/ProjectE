using System;
using System.Runtime.CompilerServices;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Mathematics;

namespace Unity.Entities
{
	// Token: 0x020001DE RID: 478
	[GenerateTestsForBurstCompatibility]
	public struct ChunkEntityEnumerator
	{
		// Token: 0x06000B18 RID: 2840 RVA: 0x00034F21 File Offset: 0x00033121
		public ChunkEntityEnumerator(bool useEnabledMask, v128 chunkEnabledMask, int chunkEntityCount)
		{
			this.UseEnabledMask = useEnabledMask;
			this.Iter = 0;
			this.ChunkEntityCount = chunkEntityCount;
			this.mask64_0 = chunkEnabledMask.ULong0;
			this.mask64_1 = chunkEnabledMask.ULong1;
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x00034F50 File Offset: 0x00033150
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool NextEntityIndex(out int nextIndex)
		{
			if (!this.UseEnabledMask)
			{
				nextIndex = this.Iter;
				this.Iter++;
				return this.Iter <= this.ChunkEntityCount;
			}
			return this.NextEntityEnabledMask(out nextIndex);
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x00034F8C File Offset: 0x0003318C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal bool NextEntityEnabledMask(out int nextIndex)
		{
			int num = math.min(64, this.ChunkEntityCount);
			while (this.Iter < num)
			{
				if ((this.mask64_0 & 1UL) != 0UL)
				{
					nextIndex = this.Iter;
					this.Iter++;
					this.mask64_0 >>= 1;
					return true;
				}
				this.mask64_0 >>= 1;
				this.Iter++;
			}
			while (this.Iter < this.ChunkEntityCount)
			{
				if ((this.mask64_1 & 1UL) != 0UL)
				{
					nextIndex = this.Iter;
					this.Iter++;
					this.mask64_1 >>= 1;
					return true;
				}
				this.mask64_1 >>= 1;
				this.Iter++;
			}
			nextIndex = this.Iter;
			return false;
		}

		// Token: 0x04000525 RID: 1317
		private int Iter;

		// Token: 0x04000526 RID: 1318
		private ulong mask64_0;

		// Token: 0x04000527 RID: 1319
		private ulong mask64_1;

		// Token: 0x04000528 RID: 1320
		private readonly bool UseEnabledMask;

		// Token: 0x04000529 RID: 1321
		private readonly int ChunkEntityCount;
	}
}
