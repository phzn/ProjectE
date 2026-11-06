using System;
using Unity.Collections;
using Unity.Mathematics;

namespace andywiecko.BurstTriangulator.LowLevel.Unsafe
{
	// Token: 0x02000029 RID: 41
	internal readonly struct TransformInt : ITransform<TransformInt, int, int2>
	{
		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x0000893A File Offset: 0x00006B3A
		public TransformInt Identity
		{
			get
			{
				return new TransformInt(int2.zero);
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x00008946 File Offset: 0x00006B46
		public int AreaScalingFactor
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00008949 File Offset: 0x00006B49
		public TransformInt(int2 translation)
		{
			this.translation = translation;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00008952 File Offset: 0x00006B52
		public TransformInt Inverse()
		{
			return new TransformInt(-this.translation);
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00008964 File Offset: 0x00006B64
		public int2 Transform(int2 point)
		{
			return point + this.translation;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00008974 File Offset: 0x00006B74
		public TransformInt CalculatePCATransformation(NativeArray<int2> positions)
		{
			throw new NotImplementedException("PCA is not implemented for int2 coordinates!");
		}

		// Token: 0x060000EC RID: 236 RVA: 0x0000898C File Offset: 0x00006B8C
		public TransformInt CalculateLocalTransformation(NativeArray<int2> positions)
		{
			int2 @int = int.MaxValue;
			int2 int2 = int.MinValue;
			int2 int3 = 0;
			foreach (int2 int4 in positions)
			{
				@int = math.min(int4, @int);
				int2 = math.max(int4, int2);
				int3 += int4;
			}
			return new TransformInt(-int3 / positions.Length);
		}

		// Token: 0x040000AE RID: 174
		private readonly int2 translation;
	}
}
