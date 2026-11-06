using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace andywiecko.BurstTriangulator.LowLevel.Unsafe
{
	// Token: 0x0200002A RID: 42
	internal interface IUtils<[IsUnmanaged] T, [IsUnmanaged] T2, [IsUnmanaged] TBig> where T : struct, ValueType where T2 : struct, ValueType where TBig : struct, ValueType
	{
		// Token: 0x060000ED RID: 237
		T Cast(TBig v);

		// Token: 0x060000EE RID: 238
		T2 CircumCenter(T2 a, T2 b, T2 c);

		// Token: 0x060000EF RID: 239
		T Const(float v);

		// Token: 0x060000F0 RID: 240
		TBig EPSILON();

		// Token: 0x060000F1 RID: 241
		bool InCircle(T2 a, T2 b, T2 c, T2 p);

		// Token: 0x060000F2 RID: 242
		TBig MaxValue();

		// Token: 0x060000F3 RID: 243
		T2 MaxValue2();

		// Token: 0x060000F4 RID: 244
		T2 MinValue2();

		// Token: 0x060000F5 RID: 245
		bool PointInsideTriangle(T2 p, T2 a, T2 b, T2 c);

		// Token: 0x060000F6 RID: 246
		bool SupportsRefinement();

		// Token: 0x060000F7 RID: 247
		T X(T2 v);

		// Token: 0x060000F8 RID: 248
		T Y(T2 v);

		// Token: 0x060000F9 RID: 249
		T Zero();

		// Token: 0x060000FA RID: 250
		TBig ZeroTBig();

		// Token: 0x060000FB RID: 251
		T abs(T v);

		// Token: 0x060000FC RID: 252
		TBig abs(TBig v);

		// Token: 0x060000FD RID: 253
		T alpha(T concentricShellReferenceRadius, T dSquare);

		// Token: 0x060000FE RID: 254
		bool anygreaterthan(T a, T b, T c, T v);

		// Token: 0x060000FF RID: 255
		T2 avg(T2 a, T2 b);

		// Token: 0x06000100 RID: 256
		T cos(T v);

		// Token: 0x06000101 RID: 257
		T diff(T a, T b);

		// Token: 0x06000102 RID: 258
		TBig diff(TBig a, TBig b);

		// Token: 0x06000103 RID: 259
		T2 diff(T2 a, T2 b);

		// Token: 0x06000104 RID: 260
		TBig distancesq(T2 a, T2 b);

		// Token: 0x06000105 RID: 261
		T dot(T2 a, T2 b);

		// Token: 0x06000106 RID: 262
		bool2 eq(T2 v, T2 w);

		// Token: 0x06000107 RID: 263
		bool2 ge(T2 a, T2 b);

		// Token: 0x06000108 RID: 264
		bool greater(T a, T b);

		// Token: 0x06000109 RID: 265
		bool greater(TBig a, TBig b);

		// Token: 0x0600010A RID: 266
		int hashkey(T2 p, T2 c, int hashSize);

		// Token: 0x0600010B RID: 267
		bool2 isfinite(T2 v);

		// Token: 0x0600010C RID: 268
		bool le(T a, T b);

		// Token: 0x0600010D RID: 269
		bool le(TBig a, TBig b);

		// Token: 0x0600010E RID: 270
		bool2 le(T2 a, T2 b);

		// Token: 0x0600010F RID: 271
		T2 lerp(T2 a, T2 b, T v);

		// Token: 0x06000110 RID: 272
		bool less(TBig a, TBig b);

		// Token: 0x06000111 RID: 273
		T2 max(T2 v, T2 w);

		// Token: 0x06000112 RID: 274
		T2 min(T2 v, T2 w);

		// Token: 0x06000113 RID: 275
		TBig mul(T a, T b);

		// Token: 0x06000114 RID: 276
		T2 neg(T2 v);

		// Token: 0x06000115 RID: 277
		T2 normalizesafe(T2 v);
	}
}
