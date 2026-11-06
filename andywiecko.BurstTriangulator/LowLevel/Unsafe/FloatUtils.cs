using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace andywiecko.BurstTriangulator.LowLevel.Unsafe
{
	// Token: 0x0200002B RID: 43
	internal readonly struct FloatUtils : IUtils<float, float2, float>
	{
		// Token: 0x06000116 RID: 278 RVA: 0x00008A24 File Offset: 0x00006C24
		public float Cast(float v)
		{
			return v;
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00008A28 File Offset: 0x00006C28
		public float2 CircumCenter(float2 a, float2 b, float2 c)
		{
			float2 @float = b - a;
			float2 float2 = c - a;
			float num = math.lengthsq(@float);
			float num2 = math.lengthsq(float2);
			float num3 = 0.5f / (@float.x * float2.y - @float.y * float2.x);
			return a + num3 * (num * math.float2(float2.y, -float2.x) + num2 * math.float2(-@float.y, @float.x));
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00008A24 File Offset: 0x00006C24
		public float Const(float v)
		{
			return v;
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00008AB8 File Offset: 0x00006CB8
		public float EPSILON()
		{
			return 1.1920929E-07f;
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00008AC0 File Offset: 0x00006CC0
		public bool InCircle(float2 a, float2 b, float2 c, float2 p)
		{
			float num = a.x - p.x;
			float num2 = a.y - p.y;
			float num3 = b.x - p.x;
			float num4 = b.y - p.y;
			float num5 = c.x - p.x;
			float num6 = c.y - p.y;
			float num7 = num * num + num2 * num2;
			float num8 = num3 * num3 + num4 * num4;
			float num9 = num5 * num5 + num6 * num6;
			return num * (num4 * num9 - num8 * num6) - num2 * (num3 * num9 - num8 * num5) + num7 * (num3 * num6 - num4 * num5) < 0f;
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00008B6E File Offset: 0x00006D6E
		public float MaxValue()
		{
			return float.MaxValue;
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00008B75 File Offset: 0x00006D75
		public float2 MaxValue2()
		{
			return float.MaxValue;
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00008B81 File Offset: 0x00006D81
		public float2 MinValue2()
		{
			return float.MinValue;
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00008B8D File Offset: 0x00006D8D
		public bool PointInsideTriangle(float2 p, float2 a, float2 b, float2 c)
		{
			return math.cmax(-FloatUtils.<PointInsideTriangle>g__bar|8_1(a, b, c, p)) <= 0f;
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00008946 File Offset: 0x00006B46
		public bool SupportsRefinement()
		{
			return true;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00008BAD File Offset: 0x00006DAD
		public float X(float2 a)
		{
			return a.x;
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00008BB5 File Offset: 0x00006DB5
		public float Y(float2 a)
		{
			return a.y;
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00008BBD File Offset: 0x00006DBD
		public float Zero()
		{
			return 0f;
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00008BBD File Offset: 0x00006DBD
		public float ZeroTBig()
		{
			return 0f;
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00008BC4 File Offset: 0x00006DC4
		public float abs(float v)
		{
			return math.abs(v);
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00008BCC File Offset: 0x00006DCC
		public float alpha(float concentricShellReferenceRadius, float edgeLengthSq)
		{
			float num = math.sqrt(edgeLengthSq);
			int num2 = (int)math.round(math.log2(0.5f * num / concentricShellReferenceRadius));
			return concentricShellReferenceRadius / num * ((num2 < 0) ? math.pow(2f, (float)num2) : ((float)(1 << num2)));
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00008C12 File Offset: 0x00006E12
		public bool anygreaterthan(float a, float b, float c, float v)
		{
			return math.any(math.float3(a, b, c) > v);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00008C28 File Offset: 0x00006E28
		public float2 avg(float2 a, float2 b)
		{
			return 0.5f * (a + b);
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00008C3B File Offset: 0x00006E3B
		public float cos(float v)
		{
			return math.cos(v);
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00008C43 File Offset: 0x00006E43
		public float diff(float a, float b)
		{
			return a - b;
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00008C48 File Offset: 0x00006E48
		public float2 diff(float2 a, float2 b)
		{
			return a - b;
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00008C51 File Offset: 0x00006E51
		public float distancesq(float2 a, float2 b)
		{
			return math.distancesq(a, b);
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00008C5A File Offset: 0x00006E5A
		public float dot(float2 a, float2 b)
		{
			return math.dot(a, b);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00008C63 File Offset: 0x00006E63
		public bool2 eq(float2 v, float2 w)
		{
			return v == w;
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00008C6C File Offset: 0x00006E6C
		public bool2 ge(float2 a, float2 b)
		{
			return a >= b;
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00008C75 File Offset: 0x00006E75
		public bool greater(float a, float b)
		{
			return a > b;
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00008C7B File Offset: 0x00006E7B
		public int hashkey(float2 p, float2 c, int hashSize)
		{
			return (int)math.floor(FloatUtils.<hashkey>g__pseudoAngle|26_0(p.x - c.x, p.y - c.y) * (float)hashSize) % hashSize;
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00008CA7 File Offset: 0x00006EA7
		public bool2 isfinite(float2 v)
		{
			return math.isfinite(v);
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00008CAF File Offset: 0x00006EAF
		public bool le(float a, float b)
		{
			return a <= b;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00008CB8 File Offset: 0x00006EB8
		public bool2 le(float2 a, float2 b)
		{
			return a <= b;
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00008CC1 File Offset: 0x00006EC1
		public float2 lerp(float2 a, float2 b, float v)
		{
			return math.lerp(a, b, v);
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00008CCB File Offset: 0x00006ECB
		public bool less(float a, float b)
		{
			return a < b;
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00008CD1 File Offset: 0x00006ED1
		public float2 max(float2 v, float2 w)
		{
			return math.max(v, w);
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00008CDA File Offset: 0x00006EDA
		public float2 min(float2 v, float2 w)
		{
			return math.min(v, w);
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00008CE3 File Offset: 0x00006EE3
		public float mul(float a, float b)
		{
			return a * b;
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00008CE8 File Offset: 0x00006EE8
		public float2 neg(float2 v)
		{
			return -v;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00008CF0 File Offset: 0x00006EF0
		public float2 normalizesafe(float2 v)
		{
			return math.normalizesafe(v, default(float2));
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00008D0C File Offset: 0x00006F0C
		[CompilerGenerated]
		internal static float <PointInsideTriangle>g__cross|8_0(float2 a, float2 b)
		{
			return a.x * b.y - a.y * b.x;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00008D2C File Offset: 0x00006F2C
		[CompilerGenerated]
		internal static float3 <PointInsideTriangle>g__bar|8_1(float2 a, float2 b, float2 c, float2 p)
		{
			float2 @float = b - a;
			float2 float2 = c - a;
			float2 float3 = p - a;
			float2 float4 = @float;
			float2 float5 = float2;
			float2 float6 = float3;
			float num = 1f / FloatUtils.<PointInsideTriangle>g__cross|8_0(float4, float5);
			float num2 = num * FloatUtils.<PointInsideTriangle>g__cross|8_0(float6, float5);
			float num3 = num * FloatUtils.<PointInsideTriangle>g__cross|8_0(float4, float6);
			return new float3(1f - num2 - num3, num2, num3);
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00008D8C File Offset: 0x00006F8C
		[CompilerGenerated]
		internal static float <hashkey>g__pseudoAngle|26_0(float dx, float dy)
		{
			float num = dx / (math.abs(dx) + math.abs(dy));
			return ((dy > 0f) ? (3f - num) : (1f + num)) / 4f;
		}
	}
}
