using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace andywiecko.BurstTriangulator.LowLevel.Unsafe
{
	// Token: 0x0200002D RID: 45
	internal readonly struct IntUtils : IUtils<int, int2, long>
	{
		// Token: 0x06000166 RID: 358 RVA: 0x0000918F File Offset: 0x0000738F
		public int Cast(long v)
		{
			return (int)v;
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00009194 File Offset: 0x00007394
		public int2 CircumCenter(int2 a, int2 b, int2 c)
		{
			int2 @int = b - a;
			int2 int2 = c - a;
			long num = (long)@int.x * (long)@int.x + (long)@int.y * (long)@int.y;
			long num2 = (long)int2.x * (long)int2.x + (long)int2.y * (long)int2.y;
			long num3 = (long)@int.x * (long)int2.y - (long)@int.y * (long)int2.x;
			if (num3 != 0L)
			{
				return (int2)math.round(a + 0.5 / (double)num3 * ((double)num * math.double2((double)int2.y, (double)(-(double)int2.x)) + (double)num2 * math.double2((double)(-(double)@int.y), (double)@int.x)));
			}
			return new int2(int.MaxValue);
		}

		// Token: 0x06000168 RID: 360 RVA: 0x0000918F File Offset: 0x0000738F
		public int Const(float v)
		{
			return (int)v;
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00009283 File Offset: 0x00007483
		public long EPSILON()
		{
			return 0L;
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00009288 File Offset: 0x00007488
		public bool InCircle(int2 a, int2 b, int2 c, int2 p)
		{
			a -= p;
			b -= p;
			c -= p;
			long num = (long)a.x * (long)a.x + (long)a.y * (long)a.y;
			long num2 = (long)b.x * (long)b.x + (long)b.y * (long)b.y;
			long num3 = (long)c.x * (long)c.x + (long)c.y * (long)c.y;
			long num4 = (long)b.y * num3 - num2 * (long)c.y;
			long num5 = (long)b.x * num3 - num2 * (long)c.x;
			long num6 = (long)b.x * (long)c.y - (long)b.y * (long)c.x;
			return (I128.Multiply((long)a.x, num4) - I128.Multiply((long)a.y, num5) + I128.Multiply(num, num6)).IsNegative;
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00009393 File Offset: 0x00007593
		public long MaxValue()
		{
			return long.MaxValue;
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0000939E File Offset: 0x0000759E
		public int2 MaxValue2()
		{
			return int.MaxValue;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x000093AA File Offset: 0x000075AA
		public int2 MinValue2()
		{
			return int.MinValue;
		}

		// Token: 0x0600016E RID: 366 RVA: 0x000093B8 File Offset: 0x000075B8
		public bool PointInsideTriangle(int2 p, int2 a, int2 b, int2 c)
		{
			return IntUtils.<PointInsideTriangle>g__cross|8_0(p - a, b - a) >= 0L && IntUtils.<PointInsideTriangle>g__cross|8_0(p - b, c - b) >= 0L && IntUtils.<PointInsideTriangle>g__cross|8_0(p - c, a - c) >= 0L;
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00009412 File Offset: 0x00007612
		public bool SupportsRefinement()
		{
			return false;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00009415 File Offset: 0x00007615
		public int X(int2 a)
		{
			return a.x;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0000941D File Offset: 0x0000761D
		public int Y(int2 a)
		{
			return a.y;
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00009412 File Offset: 0x00007612
		public int Zero()
		{
			return 0;
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00009283 File Offset: 0x00007483
		public long ZeroTBig()
		{
			return 0L;
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00009425 File Offset: 0x00007625
		public int abs(int v)
		{
			return math.abs(v);
		}

		// Token: 0x06000175 RID: 373 RVA: 0x0000942D File Offset: 0x0000762D
		public long abs(long v)
		{
			return math.abs(v);
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00009435 File Offset: 0x00007635
		public int alpha(int concentricShellReferenceRadius, int edgeLengthSq)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00009435 File Offset: 0x00007635
		public bool anygreaterthan(int a, int b, int c, int v)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000178 RID: 376 RVA: 0x0000943C File Offset: 0x0000763C
		public int2 avg(int2 a, int2 b)
		{
			return (a + b) / 2;
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00009435 File Offset: 0x00007635
		public int cos(int v)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00008C43 File Offset: 0x00006E43
		public int diff(int a, int b)
		{
			return a - b;
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00008C43 File Offset: 0x00006E43
		public long diff(long a, long b)
		{
			return a - b;
		}

		// Token: 0x0600017C RID: 380 RVA: 0x0000944B File Offset: 0x0000764B
		public int2 diff(int2 a, int2 b)
		{
			return a - b;
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00009454 File Offset: 0x00007654
		public long distancesq(int2 a, int2 b)
		{
			return (long)(a - b).x * (long)(a - b).x + (long)(a - b).y * (long)(a - b).y;
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00009435 File Offset: 0x00007635
		public int dot(int2 a, int2 b)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600017F RID: 383 RVA: 0x0000948D File Offset: 0x0000768D
		public bool2 eq(int2 v, int2 w)
		{
			return v == w;
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00009496 File Offset: 0x00007696
		public bool2 ge(int2 a, int2 b)
		{
			return a >= b;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00008C75 File Offset: 0x00006E75
		public bool greater(int a, int b)
		{
			return a > b;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00008C75 File Offset: 0x00006E75
		public bool greater(long a, long b)
		{
			return a > b;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0000949F File Offset: 0x0000769F
		public int hashkey(int2 p, int2 c, int hashSize)
		{
			return (int)math.floor(IntUtils.<hashkey>g__pseudoAngle|29_0(p.x - c.x, p.y - c.y) * (double)hashSize) % hashSize;
		}

		// Token: 0x06000184 RID: 388 RVA: 0x000094CB File Offset: 0x000076CB
		public bool2 isfinite(int2 v)
		{
			return true;
		}

		// Token: 0x06000185 RID: 389 RVA: 0x000094D3 File Offset: 0x000076D3
		public bool le(int a, int b)
		{
			return a <= b;
		}

		// Token: 0x06000186 RID: 390 RVA: 0x000094D3 File Offset: 0x000076D3
		public bool le(long a, long b)
		{
			return a <= b;
		}

		// Token: 0x06000187 RID: 391 RVA: 0x000094DC File Offset: 0x000076DC
		public bool2 le(int2 a, int2 b)
		{
			return a <= b;
		}

		// Token: 0x06000188 RID: 392 RVA: 0x000094E8 File Offset: 0x000076E8
		public int2 lerp(int2 a, int2 b, int v)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00008CCB File Offset: 0x00006ECB
		public bool less(long a, long b)
		{
			return a < b;
		}

		// Token: 0x0600018A RID: 394 RVA: 0x000094FA File Offset: 0x000076FA
		public int2 max(int2 v, int2 w)
		{
			return math.max(v, w);
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00009503 File Offset: 0x00007703
		public int2 min(int2 v, int2 w)
		{
			return math.min(v, w);
		}

		// Token: 0x0600018C RID: 396 RVA: 0x0000950C File Offset: 0x0000770C
		public long mul(int a, int b)
		{
			return (long)a * (long)b;
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00009513 File Offset: 0x00007713
		public int2 neg(int2 v)
		{
			return -v;
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000951C File Offset: 0x0000771C
		public int2 normalizesafe(int2 v)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0000952E File Offset: 0x0000772E
		[CompilerGenerated]
		internal static long <PointInsideTriangle>g__cross|8_0(int2 a, int2 b)
		{
			return (long)a.x * (long)b.y - (long)a.y * (long)b.x;
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00009550 File Offset: 0x00007750
		[CompilerGenerated]
		internal static double <hashkey>g__pseudoAngle|29_0(int dx, int dy)
		{
			int num = math.abs(dx) + math.abs(dy);
			if (num == 0)
			{
				return 0.0;
			}
			double num2 = (double)dx / (double)num;
			return ((dy > 0) ? (3.0 - num2) : (1.0 + num2)) / 4.0;
		}
	}
}
