using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace MagicaCloth2
{
	// Token: 0x02000104 RID: 260
	public static class MathExtensions
	{
		// Token: 0x06000493 RID: 1171 RVA: 0x0002B7BC File Offset: 0x000299BC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float MC2GetValue(this float4x4 m, int index)
		{
			index = math.clamp(index, 0, 15);
			float4x4 float4x = m;
			return float4x[index / 4][index % 4];
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x0002B7ED File Offset: 0x000299ED
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void MC2SetValue(this float4x4 m, int index, float value)
		{
			index = math.clamp(index, 0, 15);
			m[index / 4][index % 4] = value;
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x0002B80C File Offset: 0x00029A0C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float MC2EvaluateCurveClamp01(this float4x4 m, float time)
		{
			return math.saturate(DataUtility.EvaluateCurve(in m, time));
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x0002B81A File Offset: 0x00029A1A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float MC2EvaluateCurve(this float4x4 m, float time)
		{
			return DataUtility.EvaluateCurve(in m, time);
		}
	}
}
