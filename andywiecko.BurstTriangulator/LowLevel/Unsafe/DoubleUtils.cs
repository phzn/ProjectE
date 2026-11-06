using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace andywiecko.BurstTriangulator.LowLevel.Unsafe
{
	// Token: 0x0200002C RID: 44
	internal readonly struct DoubleUtils : IUtils<double, double2, double>
	{
		// Token: 0x0600013E RID: 318 RVA: 0x00008A24 File Offset: 0x00006C24
		public double Cast(double v)
		{
			return v;
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00008DC8 File Offset: 0x00006FC8
		public double2 CircumCenter(double2 a, double2 b, double2 c)
		{
			double2 @double = b - a;
			double2 double2 = c - a;
			double num = math.lengthsq(@double);
			double num2 = math.lengthsq(double2);
			double num3 = 0.5 / (@double.x * double2.y - @double.y * double2.x);
			return a + num3 * (num * math.double2(double2.y, -double2.x) + num2 * math.double2(-@double.y, @double.x));
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00008E5C File Offset: 0x0000705C
		public double Const(float v)
		{
			return (double)v;
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00008E60 File Offset: 0x00007060
		public double EPSILON()
		{
			return 2.220446049250313E-16;
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00008E6C File Offset: 0x0000706C
		public bool InCircle(double2 a, double2 b, double2 c, double2 p)
		{
			double num = a.x - p.x;
			double num2 = a.y - p.y;
			double num3 = b.x - p.x;
			double num4 = b.y - p.y;
			double num5 = c.x - p.x;
			double num6 = c.y - p.y;
			double num7 = num * num + num2 * num2;
			double num8 = num3 * num3 + num4 * num4;
			double num9 = num5 * num5 + num6 * num6;
			return num * (num4 * num9 - num8 * num6) - num2 * (num3 * num9 - num8 * num5) + num7 * (num3 * num6 - num4 * num5) < 0.0;
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00008F1E File Offset: 0x0000711E
		public double MaxValue()
		{
			return double.MaxValue;
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00008F29 File Offset: 0x00007129
		public double2 MaxValue2()
		{
			return double.MaxValue;
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00008F39 File Offset: 0x00007139
		public double2 MinValue2()
		{
			return double.MinValue;
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00008F49 File Offset: 0x00007149
		public bool PointInsideTriangle(double2 p, double2 a, double2 b, double2 c)
		{
			return math.cmax(-DoubleUtils.<PointInsideTriangle>g__bar|8_1(a, b, c, p)) <= 0.0;
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00008946 File Offset: 0x00006B46
		public bool SupportsRefinement()
		{
			return true;
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00008F6D File Offset: 0x0000716D
		public double X(double2 a)
		{
			return a.x;
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00008F75 File Offset: 0x00007175
		public double Y(double2 a)
		{
			return a.y;
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00008F7D File Offset: 0x0000717D
		public double Zero()
		{
			return 0.0;
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00008F7D File Offset: 0x0000717D
		public double ZeroTBig()
		{
			return 0.0;
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00008F88 File Offset: 0x00007188
		public double abs(double v)
		{
			return math.abs(v);
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00008F90 File Offset: 0x00007190
		public double alpha(double concentricShellReferenceRadius, double edgeLengthSq)
		{
			double num = math.sqrt(edgeLengthSq);
			int num2 = (int)math.round(math.log2(0.5 * num / concentricShellReferenceRadius));
			return concentricShellReferenceRadius / num * (double)((num2 < 0) ? math.pow(2f, (float)num2) : ((float)(1 << num2)));
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00008FDB File Offset: 0x000071DB
		public bool anygreaterthan(double a, double b, double c, double v)
		{
			return math.any(math.double3(a, b, c) > v);
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00008FF1 File Offset: 0x000071F1
		public double2 avg(double2 a, double2 b)
		{
			return 0.5 * (a + b);
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00009008 File Offset: 0x00007208
		public double cos(double v)
		{
			return math.cos(v);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00008C43 File Offset: 0x00006E43
		public double diff(double a, double b)
		{
			return a - b;
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00009010 File Offset: 0x00007210
		public double2 diff(double2 a, double2 b)
		{
			return a - b;
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00009019 File Offset: 0x00007219
		public double distancesq(double2 a, double2 b)
		{
			return math.distancesq(a, b);
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00009022 File Offset: 0x00007222
		public double dot(double2 a, double2 b)
		{
			return math.dot(a, b);
		}

		// Token: 0x06000155 RID: 341 RVA: 0x0000902B File Offset: 0x0000722B
		public bool2 eq(double2 v, double2 w)
		{
			return v == w;
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00009034 File Offset: 0x00007234
		public bool2 ge(double2 a, double2 b)
		{
			return a >= b;
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00008C75 File Offset: 0x00006E75
		public bool greater(double a, double b)
		{
			return a > b;
		}

		// Token: 0x06000158 RID: 344 RVA: 0x0000903D File Offset: 0x0000723D
		public int hashkey(double2 p, double2 c, int hashSize)
		{
			return (int)math.floor(DoubleUtils.<hashkey>g__pseudoAngle|26_0(p.x - c.x, p.y - c.y) * (double)hashSize) % hashSize;
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00009069 File Offset: 0x00007269
		public bool2 isfinite(double2 v)
		{
			return math.isfinite(v);
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00008CAF File Offset: 0x00006EAF
		public bool le(double a, double b)
		{
			return a <= b;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00009071 File Offset: 0x00007271
		public bool2 le(double2 a, double2 b)
		{
			return a <= b;
		}

		// Token: 0x0600015C RID: 348 RVA: 0x0000907A File Offset: 0x0000727A
		public double2 lerp(double2 a, double2 b, double v)
		{
			return math.lerp(a, b, v);
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00008CCB File Offset: 0x00006ECB
		public bool less(double a, double b)
		{
			return a < b;
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00009084 File Offset: 0x00007284
		public double2 max(double2 v, double2 w)
		{
			return math.max(v, w);
		}

		// Token: 0x0600015F RID: 351 RVA: 0x0000908D File Offset: 0x0000728D
		public double2 min(double2 v, double2 w)
		{
			return math.min(v, w);
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00008CE3 File Offset: 0x00006EE3
		public double mul(double a, double b)
		{
			return a * b;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00009096 File Offset: 0x00007296
		public double2 neg(double2 v)
		{
			return -v;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x000090A0 File Offset: 0x000072A0
		public double2 normalizesafe(double2 v)
		{
			return math.normalizesafe(v, default(double2));
		}

		// Token: 0x06000163 RID: 355 RVA: 0x000090BC File Offset: 0x000072BC
		[CompilerGenerated]
		internal static double <PointInsideTriangle>g__cross|8_0(double2 a, double2 b)
		{
			return a.x * b.y - a.y * b.x;
		}

		// Token: 0x06000164 RID: 356 RVA: 0x000090DC File Offset: 0x000072DC
		[CompilerGenerated]
		internal static double3 <PointInsideTriangle>g__bar|8_1(double2 a, double2 b, double2 c, double2 p)
		{
			double2 @double = b - a;
			double2 double2 = c - a;
			double2 double3 = p - a;
			double2 double4 = @double;
			double2 double5 = double2;
			double2 double6 = double3;
			double num = 1.0 / DoubleUtils.<PointInsideTriangle>g__cross|8_0(double4, double5);
			double num2 = num * DoubleUtils.<PointInsideTriangle>g__cross|8_0(double6, double5);
			double num3 = num * DoubleUtils.<PointInsideTriangle>g__cross|8_0(double4, double6);
			return new double3(1.0 - num2 - num3, num2, num3);
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00009144 File Offset: 0x00007344
		[CompilerGenerated]
		internal static double <hashkey>g__pseudoAngle|26_0(double dx, double dy)
		{
			double num = dx / (math.abs(dx) + math.abs(dy));
			return ((dy > 0.0) ? (3.0 - num) : (1.0 + num)) / 4.0;
		}
	}
}
