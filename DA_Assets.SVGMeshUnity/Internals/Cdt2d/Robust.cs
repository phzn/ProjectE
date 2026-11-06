using System;
using UnityEngine;

namespace DA_Assets.SVGMeshUnity.Internals.Cdt2d
{
	// Token: 0x02000011 RID: 17
	public static class Robust
	{
		// Token: 0x0600007C RID: 124 RVA: 0x000054B1 File Offset: 0x000036B1
		private static float[] GetTemporaryBuffer()
		{
			if (Robust.BufferPoolSize > 0)
			{
				return Robust.BufferPool[--Robust.BufferPoolSize];
			}
			return new float[16];
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000054D8 File Offset: 0x000036D8
		private static void ReleaseTemporaryBuffer(ref float[] buf)
		{
			if (Robust.BufferPool.Length == Robust.BufferPoolSize)
			{
				float[][] array = new float[Robust.BufferPool.Length + 32][];
				Robust.BufferPool.CopyTo(array, 0);
				Robust.BufferPool = array;
			}
			Robust.BufferPool[Robust.BufferPoolSize++] = buf;
			buf = null;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00005530 File Offset: 0x00003730
		public static float Orientation(Vector2 a, Vector2 b, Vector2 c)
		{
			float num = (a.y - c.y) * (b.x - c.x);
			float num2 = (a.x - c.x) * (b.y - c.y);
			float num3 = num - num2;
			float num4;
			if (num > 0f)
			{
				if (num2 <= 0f)
				{
					return num3;
				}
				num4 = num + num2;
			}
			else
			{
				if (num >= 0f)
				{
					return num3;
				}
				if (num2 >= 0f)
				{
					return num3;
				}
				num4 = -(num + num2);
			}
			float num5 = Robust.Errbound3 * num4;
			if (num3 >= num5 || num3 <= -num5)
			{
				return num3;
			}
			float[] temporaryBuffer = Robust.GetTemporaryBuffer();
			float[] temporaryBuffer2 = Robust.GetTemporaryBuffer();
			float[] temporaryBuffer3 = Robust.GetTemporaryBuffer();
			float[] temporaryBuffer4 = Robust.GetTemporaryBuffer();
			float[] temporaryBuffer5 = Robust.GetTemporaryBuffer();
			int num6 = 0;
			int num7 = 0;
			int num8 = 0;
			float[] temporaryBuffer6 = Robust.GetTemporaryBuffer();
			float[] temporaryBuffer7 = Robust.GetTemporaryBuffer();
			int num9 = 0;
			int num10 = 0;
			Robust.TwoProd(b.y, c.x, temporaryBuffer);
			Robust.TwoProd(-c.y, b.x, temporaryBuffer2);
			Robust.Sum(temporaryBuffer, 2, temporaryBuffer2, 2, temporaryBuffer6, out num9);
			Robust.TwoProd(a.y, b.x, temporaryBuffer);
			Robust.TwoProd(-b.y, a.x, temporaryBuffer2);
			Robust.Sum(temporaryBuffer, 2, temporaryBuffer2, 2, temporaryBuffer7, out num10);
			Robust.Sum(temporaryBuffer6, num9, temporaryBuffer7, num10, temporaryBuffer3, out num6);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer6);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer7);
			Robust.TwoProd(a.y, c.x, temporaryBuffer);
			Robust.TwoProd(-c.y, a.x, temporaryBuffer2);
			Robust.Sum(temporaryBuffer, 2, temporaryBuffer2, 2, temporaryBuffer4, out num7);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer2);
			Robust.Sub(temporaryBuffer3, num6, temporaryBuffer4, num7, temporaryBuffer5, out num8);
			float num11 = temporaryBuffer5[num8 - 1];
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer3);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer4);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer5);
			return num11;
		}

		// Token: 0x0600007F RID: 127 RVA: 0x0000571C File Offset: 0x0000391C
		public static float InSphere(Vector2 m0, Vector2 m1, Vector2 m2, Vector2 m3)
		{
			float[] temporaryBuffer = Robust.GetTemporaryBuffer();
			float[] temporaryBuffer2 = Robust.GetTemporaryBuffer();
			float[] temporaryBuffer3 = Robust.GetTemporaryBuffer();
			int num = 0;
			float[] temporaryBuffer4 = Robust.GetTemporaryBuffer();
			float[] temporaryBuffer5 = Robust.GetTemporaryBuffer();
			float[] temporaryBuffer6 = Robust.GetTemporaryBuffer();
			float[] temporaryBuffer7 = Robust.GetTemporaryBuffer();
			float[] temporaryBuffer8 = Robust.GetTemporaryBuffer();
			float[] temporaryBuffer9 = Robust.GetTemporaryBuffer();
			float[] temporaryBuffer10 = Robust.GetTemporaryBuffer();
			float[] temporaryBuffer11 = Robust.GetTemporaryBuffer();
			float[] temporaryBuffer12 = Robust.GetTemporaryBuffer();
			float[] temporaryBuffer13 = Robust.GetTemporaryBuffer();
			float[] temporaryBuffer14 = Robust.GetTemporaryBuffer();
			float[] temporaryBuffer15 = Robust.GetTemporaryBuffer();
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			int num8 = 0;
			int num9 = 0;
			int num10 = 0;
			int num11 = 0;
			int num12 = 0;
			int num13 = 0;
			Robust.TwoProd(m0[0], m0[0], temporaryBuffer2);
			Robust.TwoProd(m0[1], m0[1], temporaryBuffer3);
			Robust.Sum(temporaryBuffer2, 2, temporaryBuffer3, 2, temporaryBuffer, out num);
			Robust.Scale(temporaryBuffer, num, m1[0], temporaryBuffer4, out num2);
			Robust.Scale(temporaryBuffer, num, m2[0], temporaryBuffer5, out num3);
			Robust.Scale(temporaryBuffer, num, m3[0], temporaryBuffer6, out num4);
			Robust.TwoProd(m1[0], m1[0], temporaryBuffer2);
			Robust.TwoProd(m1[1], m1[1], temporaryBuffer3);
			Robust.Sum(temporaryBuffer2, 2, temporaryBuffer3, 2, temporaryBuffer, out num);
			Robust.Scale(temporaryBuffer, num, m0[0], temporaryBuffer7, out num5);
			Robust.Scale(temporaryBuffer, num, m2[0], temporaryBuffer8, out num6);
			Robust.Scale(temporaryBuffer, num, m3[0], temporaryBuffer9, out num7);
			Robust.TwoProd(m2[0], m2[0], temporaryBuffer2);
			Robust.TwoProd(m2[1], m2[1], temporaryBuffer3);
			Robust.Sum(temporaryBuffer2, 2, temporaryBuffer3, 2, temporaryBuffer, out num);
			Robust.Scale(temporaryBuffer, num, m0[0], temporaryBuffer10, out num8);
			Robust.Scale(temporaryBuffer, num, m1[0], temporaryBuffer11, out num9);
			Robust.Scale(temporaryBuffer, num, m3[0], temporaryBuffer12, out num10);
			Robust.TwoProd(m3[0], m3[0], temporaryBuffer2);
			Robust.TwoProd(m3[1], m3[1], temporaryBuffer3);
			Robust.Sum(temporaryBuffer2, 2, temporaryBuffer3, 2, temporaryBuffer, out num);
			Robust.Scale(temporaryBuffer, num, m0[0], temporaryBuffer13, out num11);
			Robust.Scale(temporaryBuffer, num, m1[0], temporaryBuffer14, out num12);
			Robust.Scale(temporaryBuffer, num, m2[0], temporaryBuffer15, out num13);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer2);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer3);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer);
			float[] temporaryBuffer16 = Robust.GetTemporaryBuffer();
			float[] temporaryBuffer17 = Robust.GetTemporaryBuffer();
			float[] temporaryBuffer18 = Robust.GetTemporaryBuffer();
			int num14 = 0;
			int num15 = 0;
			int num16 = 0;
			float[] temporaryBuffer19 = Robust.GetTemporaryBuffer();
			float[] temporaryBuffer20 = Robust.GetTemporaryBuffer();
			int num17 = 0;
			int num18 = 0;
			float[] temporaryBuffer21 = Robust.GetTemporaryBuffer();
			float[] temporaryBuffer22 = Robust.GetTemporaryBuffer();
			float[] temporaryBuffer23 = Robust.GetTemporaryBuffer();
			float[] temporaryBuffer24 = Robust.GetTemporaryBuffer();
			float[] temporaryBuffer25 = Robust.GetTemporaryBuffer();
			float[] temporaryBuffer26 = Robust.GetTemporaryBuffer();
			float[] temporaryBuffer27 = Robust.GetTemporaryBuffer();
			int num19 = 0;
			int num20 = 0;
			int num21 = 0;
			int num22 = 0;
			int num23 = 0;
			int num24 = 0;
			int num25 = 0;
			Robust.Sub(temporaryBuffer15, num13, temporaryBuffer12, num10, temporaryBuffer21, out num19);
			Robust.Scale(temporaryBuffer21, num19, m1[1], temporaryBuffer22, out num20);
			Robust.Sub(temporaryBuffer14, num12, temporaryBuffer9, num7, temporaryBuffer23, out num21);
			Robust.Scale(temporaryBuffer23, num21, -m2[1], temporaryBuffer24, out num22);
			Robust.Sub(temporaryBuffer11, num9, temporaryBuffer8, num6, temporaryBuffer25, out num23);
			Robust.Scale(temporaryBuffer25, num23, m3[1], temporaryBuffer26, out num24);
			Robust.Sum(temporaryBuffer24, num22, temporaryBuffer26, num24, temporaryBuffer27, out num25);
			Robust.Sum(temporaryBuffer22, num20, temporaryBuffer27, num25, temporaryBuffer19, out num17);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer21);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer22);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer23);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer24);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer25);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer26);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer27);
			float[] temporaryBuffer28 = Robust.GetTemporaryBuffer();
			float[] temporaryBuffer29 = Robust.GetTemporaryBuffer();
			float[] temporaryBuffer30 = Robust.GetTemporaryBuffer();
			float[] temporaryBuffer31 = Robust.GetTemporaryBuffer();
			float[] temporaryBuffer32 = Robust.GetTemporaryBuffer();
			float[] temporaryBuffer33 = Robust.GetTemporaryBuffer();
			float[] temporaryBuffer34 = Robust.GetTemporaryBuffer();
			int num26 = 0;
			int num27 = 0;
			int num28 = 0;
			int num29 = 0;
			int num30 = 0;
			int num31 = 0;
			int num32 = 0;
			Robust.Sub(temporaryBuffer14, num12, temporaryBuffer9, num7, temporaryBuffer28, out num26);
			Robust.Scale(temporaryBuffer28, num26, m0[1], temporaryBuffer29, out num27);
			Robust.Sub(temporaryBuffer13, num11, temporaryBuffer6, num4, temporaryBuffer30, out num28);
			Robust.Scale(temporaryBuffer30, num28, -m1[1], temporaryBuffer31, out num29);
			Robust.Sub(temporaryBuffer7, num5, temporaryBuffer4, num2, temporaryBuffer32, out num30);
			Robust.Scale(temporaryBuffer32, num30, m3[1], temporaryBuffer33, out num31);
			Robust.Sum(temporaryBuffer31, num29, temporaryBuffer33, num31, temporaryBuffer34, out num32);
			Robust.Sum(temporaryBuffer29, num27, temporaryBuffer34, num32, temporaryBuffer20, out num18);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer28);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer29);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer30);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer31);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer32);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer33);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer34);
			Robust.Sum(temporaryBuffer19, num17, temporaryBuffer20, num18, temporaryBuffer16, out num14);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer19);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer20);
			float[] temporaryBuffer35 = Robust.GetTemporaryBuffer();
			float[] temporaryBuffer36 = Robust.GetTemporaryBuffer();
			int num33 = 0;
			int num34 = 0;
			float[] temporaryBuffer37 = Robust.GetTemporaryBuffer();
			float[] temporaryBuffer38 = Robust.GetTemporaryBuffer();
			float[] temporaryBuffer39 = Robust.GetTemporaryBuffer();
			float[] temporaryBuffer40 = Robust.GetTemporaryBuffer();
			float[] temporaryBuffer41 = Robust.GetTemporaryBuffer();
			float[] temporaryBuffer42 = Robust.GetTemporaryBuffer();
			float[] temporaryBuffer43 = Robust.GetTemporaryBuffer();
			int num35 = 0;
			int num36 = 0;
			int num37 = 0;
			int num38 = 0;
			int num39 = 0;
			int num40 = 0;
			int num41 = 0;
			Robust.Sub(temporaryBuffer15, num13, temporaryBuffer12, num10, temporaryBuffer37, out num35);
			Robust.Scale(temporaryBuffer37, num35, m0[1], temporaryBuffer38, out num36);
			Robust.Sub(temporaryBuffer13, num11, temporaryBuffer6, num4, temporaryBuffer39, out num37);
			Robust.Scale(temporaryBuffer39, num37, -m2[1], temporaryBuffer40, out num38);
			Robust.Sub(temporaryBuffer10, num8, temporaryBuffer5, num3, temporaryBuffer41, out num39);
			Robust.Scale(temporaryBuffer41, num39, m3[1], temporaryBuffer42, out num40);
			Robust.Sum(temporaryBuffer40, num38, temporaryBuffer42, num40, temporaryBuffer43, out num41);
			Robust.Sum(temporaryBuffer38, num36, temporaryBuffer43, num41, temporaryBuffer35, out num33);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer37);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer38);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer39);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer40);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer41);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer42);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer43);
			float[] temporaryBuffer44 = Robust.GetTemporaryBuffer();
			float[] temporaryBuffer45 = Robust.GetTemporaryBuffer();
			float[] temporaryBuffer46 = Robust.GetTemporaryBuffer();
			float[] temporaryBuffer47 = Robust.GetTemporaryBuffer();
			float[] temporaryBuffer48 = Robust.GetTemporaryBuffer();
			float[] temporaryBuffer49 = Robust.GetTemporaryBuffer();
			float[] temporaryBuffer50 = Robust.GetTemporaryBuffer();
			int num42 = 0;
			int num43 = 0;
			int num44 = 0;
			int num45 = 0;
			int num46 = 0;
			int num47 = 0;
			int num48 = 0;
			Robust.Sub(temporaryBuffer11, num9, temporaryBuffer8, num6, temporaryBuffer44, out num42);
			Robust.Scale(temporaryBuffer44, num42, m0[1], temporaryBuffer45, out num43);
			Robust.Sub(temporaryBuffer10, num8, temporaryBuffer5, num3, temporaryBuffer46, out num44);
			Robust.Scale(temporaryBuffer46, num44, -m1[1], temporaryBuffer47, out num45);
			Robust.Sub(temporaryBuffer7, num5, temporaryBuffer4, num2, temporaryBuffer48, out num46);
			Robust.Scale(temporaryBuffer48, num46, m2[1], temporaryBuffer49, out num47);
			Robust.Sum(temporaryBuffer47, num45, temporaryBuffer49, num47, temporaryBuffer50, out num48);
			Robust.Sum(temporaryBuffer45, num43, temporaryBuffer50, num48, temporaryBuffer36, out num34);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer44);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer45);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer46);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer47);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer48);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer49);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer50);
			Robust.Sum(temporaryBuffer35, num33, temporaryBuffer36, num34, temporaryBuffer17, out num15);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer35);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer36);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer4);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer5);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer6);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer7);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer8);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer9);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer10);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer11);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer12);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer13);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer14);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer15);
			Robust.Sub(temporaryBuffer16, num14, temporaryBuffer17, num15, temporaryBuffer18, out num16);
			float num49 = temporaryBuffer18[num16 - 1];
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer16);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer17);
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer18);
			return num49;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00005EC4 File Offset: 0x000040C4
		private static void TwoProd(float a, float b, float[] result)
		{
			float num = a * b;
			float num2 = Robust.Splitter * a;
			float num3 = num2 - a;
			float num4 = num2 - num3;
			float num5 = a - num4;
			float num6 = Robust.Splitter * b;
			float num7 = num6 - b;
			float num8 = num6 - num7;
			float num9 = b - num8;
			float num10 = num - num4 * num8 - num5 * num8 - num4 * num9;
			float num11 = num5 * num9 - num10;
			result[0] = num11;
			result[1] = num;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00005F24 File Offset: 0x00004124
		private static void TwoSum(float a, float b, float[] result)
		{
			float num = a + b;
			float num2 = num - a;
			float num3 = num - num2;
			float num4 = b - num2;
			float num5 = a - num3;
			result[0] = num5 + num4;
			result[1] = num;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00005F54 File Offset: 0x00004154
		private static void Sum(float[] e, int eN, float[] f, int fN, float[] result, out int resultN)
		{
			if (eN == 1 && fN == 1)
			{
				Robust.ScalarScalar(e[0], f[0], result, out resultN);
				return;
			}
			resultN = 0;
			int i = 0;
			int j = 0;
			float num = e[i];
			float num2 = Mathf.Abs(num);
			float num3 = f[j];
			float num4 = Mathf.Abs(num3);
			float num5;
			if (num2 < num4)
			{
				num5 = num;
				i++;
				if (i < eN)
				{
					num = e[i];
					num2 = Mathf.Abs(num);
				}
			}
			else
			{
				num5 = num3;
				j++;
				if (j < fN)
				{
					num3 = f[j];
					num4 = Mathf.Abs(num3);
				}
			}
			float num6;
			if ((i < eN && num2 < num4) || j >= fN)
			{
				num6 = num;
				i++;
				if (i < eN)
				{
					num = e[i];
					num2 = Mathf.Abs(num);
				}
			}
			else
			{
				num6 = num3;
				j++;
				if (j < fN)
				{
					num3 = f[j];
					num4 = Mathf.Abs(num3);
				}
			}
			float num7 = num6 + num5;
			float num8 = num7 - num6;
			float num9 = num5 - num8;
			float num10 = num9;
			float num11 = num7;
			while (i < eN)
			{
				if (j >= fN)
				{
					break;
				}
				if (num2 < num4)
				{
					num6 = num;
					i++;
					if (i < eN)
					{
						num = e[i];
						num2 = Mathf.Abs(num);
					}
				}
				else
				{
					num6 = num3;
					j++;
					if (j < fN)
					{
						num3 = f[j];
						num4 = Mathf.Abs(num3);
					}
				}
				num5 = num10;
				num7 = num6 + num5;
				num8 = num7 - num6;
				num9 = num5 - num8;
				if (num9 != 0f)
				{
					int num12 = resultN;
					resultN = num12 + 1;
					result[num12] = num9;
				}
				float num13 = num11 + num7;
				float num14 = num13 - num11;
				float num15 = num13 - num14;
				float num16 = num7 - num14;
				num10 = num11 - num15 + num16;
				num11 = num13;
			}
			while (i < eN)
			{
				num6 = num;
				num5 = num10;
				num7 = num6 + num5;
				num8 = num7 - num6;
				num9 = num5 - num8;
				if (num9 != 0f)
				{
					int num12 = resultN;
					resultN = num12 + 1;
					result[num12] = num9;
				}
				float num17 = num11 + num7;
				float num14 = num17 - num11;
				float num15 = num17 - num14;
				float num16 = num7 - num14;
				num10 = num11 - num15 + num16;
				num11 = num17;
				i++;
				if (i < eN)
				{
					num = e[i];
				}
			}
			while (j < fN)
			{
				num6 = num3;
				num5 = num10;
				num7 = num6 + num5;
				num8 = num7 - num6;
				num9 = num5 - num8;
				if (num9 != 0f)
				{
					int num12 = resultN;
					resultN = num12 + 1;
					result[num12] = num9;
				}
				float num18 = num11 + num7;
				float num14 = num18 - num11;
				float num15 = num18 - num14;
				float num16 = num7 - num14;
				num10 = num11 - num15 + num16;
				num11 = num18;
				j++;
				if (j < fN)
				{
					num3 = f[j];
				}
			}
			if (num10 != 0f)
			{
				int num12 = resultN;
				resultN = num12 + 1;
				result[num12] = num10;
			}
			if (num11 != 0f)
			{
				int num12 = resultN;
				resultN = num12 + 1;
				result[num12] = num11;
			}
			if (resultN == 0)
			{
				int num12 = resultN;
				resultN = num12 + 1;
				result[num12] = 0f;
			}
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00006224 File Offset: 0x00004424
		private static void Sub(float[] e, int eN, float[] f, int fN, float[] result, out int resultN)
		{
			if (eN == 1 && fN == 1)
			{
				Robust.ScalarScalar(e[0], -f[0], result, out resultN);
				return;
			}
			resultN = 0;
			int i = 0;
			int j = 0;
			float num = e[i];
			float num2 = Mathf.Abs(num);
			float num3 = -f[j];
			float num4 = Mathf.Abs(num3);
			float num5;
			if (num2 < num4)
			{
				num5 = num;
				i++;
				if (i < eN)
				{
					num = e[i];
					num2 = Mathf.Abs(num);
				}
			}
			else
			{
				num5 = num3;
				j++;
				if (j < fN)
				{
					num3 = -f[j];
					num4 = Mathf.Abs(num3);
				}
			}
			float num6;
			if ((i < eN && num2 < num4) || j >= fN)
			{
				num6 = num;
				i++;
				if (i < eN)
				{
					num = e[i];
					num2 = Mathf.Abs(num);
				}
			}
			else
			{
				num6 = num3;
				j++;
				if (j < fN)
				{
					num3 = -f[j];
					num4 = Mathf.Abs(num3);
				}
			}
			float num7 = num6 + num5;
			float num8 = num7 - num6;
			float num9 = num5 - num8;
			float num10 = num9;
			float num11 = num7;
			while (i < eN)
			{
				if (j >= fN)
				{
					break;
				}
				if (num2 < num4)
				{
					num6 = num;
					i++;
					if (i < eN)
					{
						num = e[i];
						num2 = Mathf.Abs(num);
					}
				}
				else
				{
					num6 = num3;
					j++;
					if (j < fN)
					{
						num3 = -f[j];
						num4 = Mathf.Abs(num3);
					}
				}
				num5 = num10;
				num7 = num6 + num5;
				num8 = num7 - num6;
				num9 = num5 - num8;
				if (num9 != 0f)
				{
					int num12 = resultN;
					resultN = num12 + 1;
					result[num12] = num9;
				}
				float num13 = num11 + num7;
				float num14 = num13 - num11;
				float num15 = num13 - num14;
				float num16 = num7 - num14;
				num10 = num11 - num15 + num16;
				num11 = num13;
			}
			while (i < eN)
			{
				num6 = num;
				num5 = num10;
				num7 = num6 + num5;
				num8 = num7 - num6;
				num9 = num5 - num8;
				if (num9 != 0f)
				{
					int num12 = resultN;
					resultN = num12 + 1;
					result[num12] = num9;
				}
				float num17 = num11 + num7;
				float num14 = num17 - num11;
				float num15 = num17 - num14;
				float num16 = num7 - num14;
				num10 = num11 - num15 + num16;
				num11 = num17;
				i++;
				if (i < eN)
				{
					num = e[i];
				}
			}
			while (j < fN)
			{
				num6 = num3;
				num5 = num10;
				num7 = num6 + num5;
				num8 = num7 - num6;
				num9 = num5 - num8;
				if (num9 != 0f)
				{
					int num12 = resultN;
					resultN = num12 + 1;
					result[num12] = num9;
				}
				float num18 = num11 + num7;
				float num14 = num18 - num11;
				float num15 = num18 - num14;
				float num16 = num7 - num14;
				num10 = num11 - num15 + num16;
				num11 = num18;
				j++;
				if (j < fN)
				{
					num3 = -f[j];
				}
			}
			if (num10 != 0f)
			{
				int num12 = resultN;
				resultN = num12 + 1;
				result[num12] = num10;
			}
			if (num11 != 0f)
			{
				int num12 = resultN;
				resultN = num12 + 1;
				result[num12] = num11;
			}
			if (resultN == 0)
			{
				int num12 = resultN;
				resultN = num12 + 1;
				result[num12] = 0f;
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x000064F8 File Offset: 0x000046F8
		private static void Scale(float[] e, int eN, float scale, float[] result, out int resultN)
		{
			if (eN != 1)
			{
				float[] temporaryBuffer = Robust.GetTemporaryBuffer();
				float[] temporaryBuffer2 = Robust.GetTemporaryBuffer();
				temporaryBuffer[0] = 0.1f;
				temporaryBuffer[1] = 0.1f;
				temporaryBuffer2[0] = 0.1f;
				temporaryBuffer2[1] = 0.1f;
				resultN = 0;
				Robust.TwoProd(e[0], scale, temporaryBuffer);
				if (temporaryBuffer[0] != 0f)
				{
					int num = resultN;
					resultN = num + 1;
					result[num] = temporaryBuffer[0];
				}
				for (int i = 1; i < eN; i++)
				{
					Robust.TwoProd(e[i], scale, temporaryBuffer2);
					Robust.TwoSum(temporaryBuffer[1], temporaryBuffer2[0], temporaryBuffer);
					if (temporaryBuffer[0] != 0f)
					{
						int num = resultN;
						resultN = num + 1;
						result[num] = temporaryBuffer[0];
					}
					float num2 = temporaryBuffer2[1];
					float num3 = temporaryBuffer[1];
					float num4 = num2 + num3;
					float num5 = num4 - num2;
					float num6 = num3 - num5;
					temporaryBuffer[1] = num4;
					if (num6 != 0f)
					{
						int num = resultN;
						resultN = num + 1;
						result[num] = num6;
					}
				}
				if (temporaryBuffer[1] != 0f)
				{
					int num = resultN;
					resultN = num + 1;
					result[num] = temporaryBuffer[1];
				}
				Robust.ReleaseTemporaryBuffer(ref temporaryBuffer);
				Robust.ReleaseTemporaryBuffer(ref temporaryBuffer2);
				if (resultN == 0)
				{
					int num = resultN;
					resultN = num + 1;
					result[num] = 0f;
				}
				return;
			}
			float[] temporaryBuffer3 = Robust.GetTemporaryBuffer();
			Robust.TwoProd(e[0], scale, temporaryBuffer3);
			if (temporaryBuffer3[0] != 0f)
			{
				result[0] = temporaryBuffer3[0];
				result[1] = temporaryBuffer3[1];
				resultN = 2;
				Robust.ReleaseTemporaryBuffer(ref temporaryBuffer3);
				return;
			}
			result[0] = temporaryBuffer3[1];
			resultN = 1;
			Robust.ReleaseTemporaryBuffer(ref temporaryBuffer3);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00006660 File Offset: 0x00004860
		private static void ScalarScalar(float a, float b, float[] result, out int resultN)
		{
			float num = a + b;
			float num2 = num - a;
			float num3 = num - num2;
			float num4 = b - num2;
			float num5 = a - num3 + num4;
			if (num5 != 0f)
			{
				result[0] = num5;
				result[1] = num;
				resultN = 2;
				return;
			}
			result[0] = num;
			resultN = 1;
		}

		// Token: 0x04000033 RID: 51
		private static readonly float Epsilon = 1.110223E-16f;

		// Token: 0x04000034 RID: 52
		private static readonly float Errbound3 = (3f + 16f * Robust.Epsilon) * Robust.Epsilon;

		// Token: 0x04000035 RID: 53
		private static readonly float Splitter = Mathf.Pow(2f, 27f) + 1f;

		// Token: 0x04000036 RID: 54
		private static float[][] BufferPool = new float[32][];

		// Token: 0x04000037 RID: 55
		private static int BufferPoolSize = 0;
	}
}
