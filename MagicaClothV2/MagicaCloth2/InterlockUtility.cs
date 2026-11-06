using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.Mathematics;

namespace MagicaCloth2
{
	// Token: 0x020000F0 RID: 240
	public static class InterlockUtility
	{
		// Token: 0x06000443 RID: 1091 RVA: 0x0002A820 File Offset: 0x00028A20
		internal unsafe static void AddFloat3(int index, float3 add, int* cntPt, int* sumPt)
		{
			Interlocked.Increment(ref cntPt[index]);
			int3 @int = (int3)(add * 1000000f);
			index *= 3;
			int i = 0;
			while (i < 3)
			{
				if (@int[i] != 0)
				{
					Interlocked.Add(ref sumPt[index], @int[i]);
				}
				i++;
				index++;
			}
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x0002A880 File Offset: 0x00028A80
		internal unsafe static void AddFloat3(int index, float3 add, int* sumPt)
		{
			int3 @int = (int3)(add * 1000000f);
			index *= 3;
			int i = 0;
			while (i < 3)
			{
				if (@int[i] != 0)
				{
					Interlocked.Add(ref sumPt[index], @int[i]);
				}
				i++;
				index++;
			}
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x0002A8D4 File Offset: 0x00028AD4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void Max(int index, float value, int* pt)
		{
			int num = (int)value * 1000000;
			int num2 = pt[index];
			int num3 = num2 + 1;
			while (num > num2 && num2 != num3)
			{
				num3 = num2;
				num2 = Interlocked.CompareExchange(ref pt[index], num, num2);
			}
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x0002A910 File Offset: 0x00028B10
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static float3 ReadAverageFloat3(int index, int* cntPt, int* sumPt)
		{
			int num = cntPt[index];
			if (num == 0)
			{
				return 0;
			}
			int num2 = index * 3;
			return new float3((float)sumPt[num2], (float)sumPt[num2 + 1], (float)sumPt[num2 + 2]) / (float)num * 1E-06f;
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x0002A968 File Offset: 0x00028B68
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static float3 ReadFloat3(int index, int* vecPt)
		{
			int num = index * 3;
			return new float3((float)vecPt[num], (float)vecPt[num + 1], (float)vecPt[num + 2]) * 1E-06f;
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x0002A9A4 File Offset: 0x00028BA4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static float ReadFloat(int index, int* floatPt)
		{
			return (float)floatPt[index] * 1E-06f;
		}

		// Token: 0x04000868 RID: 2152
		internal const int ToFixed = 1000000;

		// Token: 0x04000869 RID: 2153
		internal const float ToFloat = 1E-06f;
	}
}
