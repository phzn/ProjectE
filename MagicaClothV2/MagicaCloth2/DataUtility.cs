using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x020000EC RID: 236
	public class DataUtility
	{
		// Token: 0x06000407 RID: 1031 RVA: 0x00029F70 File Offset: 0x00028170
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 PackInt2(int d0, int d1)
		{
			if (d0 >= d1)
			{
				return new int2(d1, d0);
			}
			return new int2(d0, d1);
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x00029F85 File Offset: 0x00028185
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 PackInt2(in int2 d)
		{
			return DataUtility.PackInt2(d.x, d.y);
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x00029F98 File Offset: 0x00028198
		public static int3 PackInt3(int d0, int d1, int d2)
		{
			if (d0 < d1 && d0 < d2)
			{
				if (d1 < d2)
				{
					return new int3(d0, d1, d2);
				}
				return new int3(d0, d2, d1);
			}
			else if (d1 < d2)
			{
				if (d0 < d2)
				{
					return new int3(d1, d0, d2);
				}
				return new int3(d1, d2, d0);
			}
			else
			{
				if (d0 < d1)
				{
					return new int3(d2, d0, d1);
				}
				return new int3(d2, d1, d0);
			}
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x00029FF2 File Offset: 0x000281F2
		public static int3 PackInt3(in int3 d)
		{
			return DataUtility.PackInt3(d.x, d.y, d.z);
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x0002A00B File Offset: 0x0002820B
		public static int4 PackInt4(int d0, int d1, int d2, int d3)
		{
			if (d0 > d3)
			{
				int num = d0;
				d0 = d3;
				d3 = num;
			}
			if (d1 > d2)
			{
				int num2 = d1;
				d1 = d2;
				d2 = num2;
			}
			if (d0 > d1)
			{
				int num3 = d0;
				d0 = d1;
				d1 = num3;
			}
			if (d2 > d3)
			{
				int num4 = d2;
				d2 = d3;
				d3 = num4;
			}
			if (d1 > d2)
			{
				int num5 = d1;
				d1 = d2;
				d2 = num5;
			}
			return new int4(d0, d1, d2, d3);
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x0002A048 File Offset: 0x00028248
		public static int4 PackInt4(int4 d)
		{
			return DataUtility.PackInt4(d.x, d.y, d.z, d.w);
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x0002A067 File Offset: 0x00028267
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint Pack32(int hi, int low)
		{
			return (uint)((hi << 16) | (low & 65535));
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x0002A075 File Offset: 0x00028275
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint Pack32Sort(int a, int b)
		{
			if (a > b)
			{
				return (uint)((b << 16) | (a & 65535));
			}
			return (uint)((a << 16) | (b & 65535));
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x0002A094 File Offset: 0x00028294
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Unpack32Hi(uint pack)
		{
			return (int)((pack >> 16) & 65535U);
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0002A0A0 File Offset: 0x000282A0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Unpack32Low(uint pack)
		{
			return (int)(pack & 65535U);
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x0002A0A9 File Offset: 0x000282A9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint Pack12_20(int hi, int low)
		{
			return (uint)((hi << 20) | (low & 1048575));
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x0002A0B7 File Offset: 0x000282B7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Unpack12_20Hi(uint pack)
		{
			return (int)((pack >> 20) & 4095U);
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x0002A0C3 File Offset: 0x000282C3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Unpack12_20Low(uint pack)
		{
			return (int)(pack & 1048575U);
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x0002A0CC File Offset: 0x000282CC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Unpack12_20(uint pack, out int hi, out int low)
		{
			hi = (int)((pack >> 20) & 4095U);
			low = (int)(pack & 1048575U);
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x0002A0E3 File Offset: 0x000282E3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong Pack64(int x, int y, int z, int w)
		{
			return (ulong)((((long)x & 65535L) << 48) | (((long)y & 65535L) << 32) | (((long)z & 65535L) << 16) | ((long)w & 65535L));
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x0002A115 File Offset: 0x00028315
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong Pack64(in int4 a)
		{
			return DataUtility.Pack64(a.x, a.y, a.z, a.w);
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x0002A134 File Offset: 0x00028334
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 Unpack64(in ulong pack)
		{
			return new int4((int)((pack >> 48) & 65535UL), (int)((pack >> 32) & 65535UL), (int)((pack >> 16) & 65535UL), (int)(pack & 65535UL));
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x0002A16C File Offset: 0x0002836C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Unpack64X(in ulong pack)
		{
			return (int)((pack >> 48) & 65535UL);
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x0002A17B File Offset: 0x0002837B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Unpack64Y(in ulong pack)
		{
			return (int)((pack >> 32) & 65535UL);
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x0002A18A File Offset: 0x0002838A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Unpack64Z(in ulong pack)
		{
			return (int)((pack >> 16) & 65535UL);
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x0002A199 File Offset: 0x00028399
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Unpack64W(in ulong pack)
		{
			return (int)(pack & 65535UL);
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x0002A1A5 File Offset: 0x000283A5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint Pack32(int x, int y, int z, int w)
		{
			return (uint)(((x & 255) << 24) | ((y & 255) << 16) | ((z & 255) << 8) | (w & 255));
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x0002A115 File Offset: 0x00028315
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong Pack32(in int4 a)
		{
			return DataUtility.Pack64(a.x, a.y, a.z, a.w);
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x0002A1CE File Offset: 0x000283CE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4 Unpack32(in uint pack)
		{
			return new int4((int)((pack >> 24) & 255U), (int)((pack >> 16) & 255U), (int)((pack >> 8) & 255U), (int)(pack & 255U));
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x0002A200 File Offset: 0x00028400
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int RemainingData(in int3 data, in int2 use)
		{
			if (data.x != use.x && data.x != use.y)
			{
				return data.x;
			}
			if (data.y != use.x && data.y != use.y)
			{
				return data.y;
			}
			return data.z;
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x0002A25C File Offset: 0x0002845C
		public static float4x4 ConvertAnimationCurve(AnimationCurve curve)
		{
			float4x4 float4x = 0;
			for (int i = 0; i < 16; i++)
			{
				float num = (float)i / 15f;
				float num2 = curve.Evaluate(num);
				(ref float4x).MC2SetValue(i, num2);
			}
			return float4x;
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x0002A298 File Offset: 0x00028498
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float EvaluateCurve(in float4x4 curve, float time)
		{
			int num = (int)(math.saturate(time) * 15f);
			time -= (float)num * 0.06666667f;
			float num2 = time / 0.06666667f;
			return math.lerp((in curve).MC2GetValue(num), (in curve).MC2GetValue(num + 1), num2);
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x0002A2DD File Offset: 0x000284DD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ExBitFlag16 SetColliderType(ExBitFlag16 flag, ColliderManager.ColliderType ctype)
		{
			flag.Value = (flag.Value & 65520) | (ushort)ctype;
			return flag;
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x0002A2F6 File Offset: 0x000284F6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ColliderManager.ColliderType GetColliderType(in ExBitFlag16 flag)
		{
			return (ColliderManager.ColliderType)(flag.Value & 15);
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x0002A302 File Offset: 0x00028502
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ExBitFlag16 SetSymmetryType(ExBitFlag16 flag, ColliderManager.SymmetryType stype)
		{
			flag.Value = (flag.Value & 65295) | (ushort)(stype << 4);
			return flag;
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x0002A31D File Offset: 0x0002851D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ColliderManager.SymmetryType GetSymmetryType(in ExBitFlag16 flag)
		{
			return (ColliderManager.SymmetryType)((flag.Value & 240) >> 4);
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x0002A32E File Offset: 0x0002852E
		public static void ArrayCopy<T>(T[] src, ref T[] dst)
		{
			if (src == null)
			{
				dst = null;
				return;
			}
			if (src.Length == 0)
			{
				dst = new T[0];
				return;
			}
			dst = new T[src.Length];
			Array.Copy(src, dst, src.Length);
		}
	}
}
