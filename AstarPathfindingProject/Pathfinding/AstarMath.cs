using System;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000051 RID: 81
	public static class AstarMath
	{
		// Token: 0x06000349 RID: 841 RVA: 0x0000EF94 File Offset: 0x0000D194
		public static float ThreadSafeRandomFloat()
		{
			object globalRandomLock = AstarMath.GlobalRandomLock;
			float num;
			lock (globalRandomLock)
			{
				num = AstarMath.GlobalRandom.NextFloat();
			}
			return num;
		}

		// Token: 0x0600034A RID: 842 RVA: 0x0000EFDC File Offset: 0x0000D1DC
		public static float2 ThreadSafeRandomFloat2()
		{
			object globalRandomLock = AstarMath.GlobalRandomLock;
			float2 @float;
			lock (globalRandomLock)
			{
				@float = AstarMath.GlobalRandom.NextFloat2();
			}
			return @float;
		}

		// Token: 0x0600034B RID: 843 RVA: 0x0000F024 File Offset: 0x0000D224
		public static long SaturatingConvertFloatToLong(float v)
		{
			if (v <= 9.223372E+18f)
			{
				return (long)v;
			}
			return long.MaxValue;
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0000F03A File Offset: 0x0000D23A
		public static float MapTo(float startMin, float startMax, float targetMin, float targetMax, float value)
		{
			return Mathf.Lerp(targetMin, targetMax, Mathf.InverseLerp(startMin, startMax, value));
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0000F04C File Offset: 0x0000D24C
		private static int Bit(int a, int b)
		{
			return (a >> b) & 1;
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0000F058 File Offset: 0x0000D258
		public static Color IntToColor(int i, float a)
		{
			float num = (float)(AstarMath.Bit(i, 2) + AstarMath.Bit(i, 3) * 2 + 1);
			int num2 = AstarMath.Bit(i, 1) + AstarMath.Bit(i, 4) * 2 + 1;
			int num3 = AstarMath.Bit(i, 0) + AstarMath.Bit(i, 5) * 2 + 1;
			return new Color(num * 0.25f, (float)num2 * 0.25f, (float)num3 * 0.25f, a);
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0000F0C0 File Offset: 0x0000D2C0
		public static Color HSVToRGB(float h, float s, float v)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = s * v;
			float num5 = h / 60f;
			float num6 = num4 * (1f - Math.Abs(num5 % 2f - 1f));
			if (num5 < 1f)
			{
				num = num4;
				num2 = num6;
			}
			else if (num5 < 2f)
			{
				num = num6;
				num2 = num4;
			}
			else if (num5 < 3f)
			{
				num2 = num4;
				num3 = num6;
			}
			else if (num5 < 4f)
			{
				num2 = num6;
				num3 = num4;
			}
			else if (num5 < 5f)
			{
				num = num6;
				num3 = num4;
			}
			else if (num5 < 6f)
			{
				num = num4;
				num3 = num6;
			}
			float num7 = v - num4;
			num += num7;
			num2 += num7;
			num3 += num7;
			return new Color(num, num2, num3);
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0000F184 File Offset: 0x0000D384
		public static float DeltaAngle(float angle1, float angle2)
		{
			float num = (angle2 - angle1 + 3.1415927f) % 6.2831855f - 3.1415927f;
			return math.select(num, num + 6.2831855f, num < -3.1415927f);
		}

		// Token: 0x040001FC RID: 508
		private static Unity.Mathematics.Random GlobalRandom = Unity.Mathematics.Random.CreateFromIndex(0U);

		// Token: 0x040001FD RID: 509
		private static object GlobalRandomLock = new object();
	}
}
