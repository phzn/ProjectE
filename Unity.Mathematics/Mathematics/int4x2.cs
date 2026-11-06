using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x02000037 RID: 55
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct int4x2 : IEquatable<int4x2>, IFormattable
	{
		// Token: 0x06001D4A RID: 7498 RVA: 0x0004FBE9 File Offset: 0x0004DDE9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4x2(int4 c0, int4 c1)
		{
			this.c0 = c0;
			this.c1 = c1;
		}

		// Token: 0x06001D4B RID: 7499 RVA: 0x0004FBF9 File Offset: 0x0004DDF9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4x2(int m00, int m01, int m10, int m11, int m20, int m21, int m30, int m31)
		{
			this.c0 = new int4(m00, m10, m20, m30);
			this.c1 = new int4(m01, m11, m21, m31);
		}

		// Token: 0x06001D4C RID: 7500 RVA: 0x0004FC1E File Offset: 0x0004DE1E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4x2(int v)
		{
			this.c0 = v;
			this.c1 = v;
		}

		// Token: 0x06001D4D RID: 7501 RVA: 0x0004FC38 File Offset: 0x0004DE38
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4x2(bool v)
		{
			this.c0 = math.select(new int4(0), new int4(1), v);
			this.c1 = math.select(new int4(0), new int4(1), v);
		}

		// Token: 0x06001D4E RID: 7502 RVA: 0x0004FC6A File Offset: 0x0004DE6A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4x2(bool4x2 v)
		{
			this.c0 = math.select(new int4(0), new int4(1), v.c0);
			this.c1 = math.select(new int4(0), new int4(1), v.c1);
		}

		// Token: 0x06001D4F RID: 7503 RVA: 0x0004FCA6 File Offset: 0x0004DEA6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4x2(uint v)
		{
			this.c0 = (int4)v;
			this.c1 = (int4)v;
		}

		// Token: 0x06001D50 RID: 7504 RVA: 0x0004FCC0 File Offset: 0x0004DEC0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4x2(uint4x2 v)
		{
			this.c0 = (int4)v.c0;
			this.c1 = (int4)v.c1;
		}

		// Token: 0x06001D51 RID: 7505 RVA: 0x0004FCE4 File Offset: 0x0004DEE4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4x2(float v)
		{
			this.c0 = (int4)v;
			this.c1 = (int4)v;
		}

		// Token: 0x06001D52 RID: 7506 RVA: 0x0004FCFE File Offset: 0x0004DEFE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4x2(float4x2 v)
		{
			this.c0 = (int4)v.c0;
			this.c1 = (int4)v.c1;
		}

		// Token: 0x06001D53 RID: 7507 RVA: 0x0004FD22 File Offset: 0x0004DF22
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4x2(double v)
		{
			this.c0 = (int4)v;
			this.c1 = (int4)v;
		}

		// Token: 0x06001D54 RID: 7508 RVA: 0x0004FD3C File Offset: 0x0004DF3C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4x2(double4x2 v)
		{
			this.c0 = (int4)v.c0;
			this.c1 = (int4)v.c1;
		}

		// Token: 0x06001D55 RID: 7509 RVA: 0x0004FD60 File Offset: 0x0004DF60
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator int4x2(int v)
		{
			return new int4x2(v);
		}

		// Token: 0x06001D56 RID: 7510 RVA: 0x0004FD68 File Offset: 0x0004DF68
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int4x2(bool v)
		{
			return new int4x2(v);
		}

		// Token: 0x06001D57 RID: 7511 RVA: 0x0004FD70 File Offset: 0x0004DF70
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int4x2(bool4x2 v)
		{
			return new int4x2(v);
		}

		// Token: 0x06001D58 RID: 7512 RVA: 0x0004FD78 File Offset: 0x0004DF78
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int4x2(uint v)
		{
			return new int4x2(v);
		}

		// Token: 0x06001D59 RID: 7513 RVA: 0x0004FD80 File Offset: 0x0004DF80
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int4x2(uint4x2 v)
		{
			return new int4x2(v);
		}

		// Token: 0x06001D5A RID: 7514 RVA: 0x0004FD88 File Offset: 0x0004DF88
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int4x2(float v)
		{
			return new int4x2(v);
		}

		// Token: 0x06001D5B RID: 7515 RVA: 0x0004FD90 File Offset: 0x0004DF90
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int4x2(float4x2 v)
		{
			return new int4x2(v);
		}

		// Token: 0x06001D5C RID: 7516 RVA: 0x0004FD98 File Offset: 0x0004DF98
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int4x2(double v)
		{
			return new int4x2(v);
		}

		// Token: 0x06001D5D RID: 7517 RVA: 0x0004FDA0 File Offset: 0x0004DFA0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int4x2(double4x2 v)
		{
			return new int4x2(v);
		}

		// Token: 0x06001D5E RID: 7518 RVA: 0x0004FDA8 File Offset: 0x0004DFA8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x2 operator *(int4x2 lhs, int4x2 rhs)
		{
			return new int4x2(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1);
		}

		// Token: 0x06001D5F RID: 7519 RVA: 0x0004FDD1 File Offset: 0x0004DFD1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x2 operator *(int4x2 lhs, int rhs)
		{
			return new int4x2(lhs.c0 * rhs, lhs.c1 * rhs);
		}

		// Token: 0x06001D60 RID: 7520 RVA: 0x0004FDF0 File Offset: 0x0004DFF0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x2 operator *(int lhs, int4x2 rhs)
		{
			return new int4x2(lhs * rhs.c0, lhs * rhs.c1);
		}

		// Token: 0x06001D61 RID: 7521 RVA: 0x0004FE0F File Offset: 0x0004E00F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x2 operator +(int4x2 lhs, int4x2 rhs)
		{
			return new int4x2(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1);
		}

		// Token: 0x06001D62 RID: 7522 RVA: 0x0004FE38 File Offset: 0x0004E038
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x2 operator +(int4x2 lhs, int rhs)
		{
			return new int4x2(lhs.c0 + rhs, lhs.c1 + rhs);
		}

		// Token: 0x06001D63 RID: 7523 RVA: 0x0004FE57 File Offset: 0x0004E057
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x2 operator +(int lhs, int4x2 rhs)
		{
			return new int4x2(lhs + rhs.c0, lhs + rhs.c1);
		}

		// Token: 0x06001D64 RID: 7524 RVA: 0x0004FE76 File Offset: 0x0004E076
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x2 operator -(int4x2 lhs, int4x2 rhs)
		{
			return new int4x2(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1);
		}

		// Token: 0x06001D65 RID: 7525 RVA: 0x0004FE9F File Offset: 0x0004E09F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x2 operator -(int4x2 lhs, int rhs)
		{
			return new int4x2(lhs.c0 - rhs, lhs.c1 - rhs);
		}

		// Token: 0x06001D66 RID: 7526 RVA: 0x0004FEBE File Offset: 0x0004E0BE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x2 operator -(int lhs, int4x2 rhs)
		{
			return new int4x2(lhs - rhs.c0, lhs - rhs.c1);
		}

		// Token: 0x06001D67 RID: 7527 RVA: 0x0004FEDD File Offset: 0x0004E0DD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x2 operator /(int4x2 lhs, int4x2 rhs)
		{
			return new int4x2(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1);
		}

		// Token: 0x06001D68 RID: 7528 RVA: 0x0004FF06 File Offset: 0x0004E106
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x2 operator /(int4x2 lhs, int rhs)
		{
			return new int4x2(lhs.c0 / rhs, lhs.c1 / rhs);
		}

		// Token: 0x06001D69 RID: 7529 RVA: 0x0004FF25 File Offset: 0x0004E125
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x2 operator /(int lhs, int4x2 rhs)
		{
			return new int4x2(lhs / rhs.c0, lhs / rhs.c1);
		}

		// Token: 0x06001D6A RID: 7530 RVA: 0x0004FF44 File Offset: 0x0004E144
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x2 operator %(int4x2 lhs, int4x2 rhs)
		{
			return new int4x2(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1);
		}

		// Token: 0x06001D6B RID: 7531 RVA: 0x0004FF6D File Offset: 0x0004E16D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x2 operator %(int4x2 lhs, int rhs)
		{
			return new int4x2(lhs.c0 % rhs, lhs.c1 % rhs);
		}

		// Token: 0x06001D6C RID: 7532 RVA: 0x0004FF8C File Offset: 0x0004E18C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x2 operator %(int lhs, int4x2 rhs)
		{
			return new int4x2(lhs % rhs.c0, lhs % rhs.c1);
		}

		// Token: 0x06001D6D RID: 7533 RVA: 0x0004FFAC File Offset: 0x0004E1AC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x2 operator ++(int4x2 val)
		{
			int4 @int = int4.op_Increment(val.c0);
			val.c0 = @int;
			int4 int2 = @int;
			@int = int4.op_Increment(val.c1);
			val.c1 = @int;
			return new int4x2(int2, @int);
		}

		// Token: 0x06001D6E RID: 7534 RVA: 0x0004FFF4 File Offset: 0x0004E1F4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x2 operator --(int4x2 val)
		{
			int4 @int = int4.op_Decrement(val.c0);
			val.c0 = @int;
			int4 int2 = @int;
			@int = int4.op_Decrement(val.c1);
			val.c1 = @int;
			return new int4x2(int2, @int);
		}

		// Token: 0x06001D6F RID: 7535 RVA: 0x0005003A File Offset: 0x0004E23A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator <(int4x2 lhs, int4x2 rhs)
		{
			return new bool4x2(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1);
		}

		// Token: 0x06001D70 RID: 7536 RVA: 0x00050063 File Offset: 0x0004E263
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator <(int4x2 lhs, int rhs)
		{
			return new bool4x2(lhs.c0 < rhs, lhs.c1 < rhs);
		}

		// Token: 0x06001D71 RID: 7537 RVA: 0x00050082 File Offset: 0x0004E282
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator <(int lhs, int4x2 rhs)
		{
			return new bool4x2(lhs < rhs.c0, lhs < rhs.c1);
		}

		// Token: 0x06001D72 RID: 7538 RVA: 0x000500A1 File Offset: 0x0004E2A1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator <=(int4x2 lhs, int4x2 rhs)
		{
			return new bool4x2(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1);
		}

		// Token: 0x06001D73 RID: 7539 RVA: 0x000500CA File Offset: 0x0004E2CA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator <=(int4x2 lhs, int rhs)
		{
			return new bool4x2(lhs.c0 <= rhs, lhs.c1 <= rhs);
		}

		// Token: 0x06001D74 RID: 7540 RVA: 0x000500E9 File Offset: 0x0004E2E9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator <=(int lhs, int4x2 rhs)
		{
			return new bool4x2(lhs <= rhs.c0, lhs <= rhs.c1);
		}

		// Token: 0x06001D75 RID: 7541 RVA: 0x00050108 File Offset: 0x0004E308
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator >(int4x2 lhs, int4x2 rhs)
		{
			return new bool4x2(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1);
		}

		// Token: 0x06001D76 RID: 7542 RVA: 0x00050131 File Offset: 0x0004E331
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator >(int4x2 lhs, int rhs)
		{
			return new bool4x2(lhs.c0 > rhs, lhs.c1 > rhs);
		}

		// Token: 0x06001D77 RID: 7543 RVA: 0x00050150 File Offset: 0x0004E350
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator >(int lhs, int4x2 rhs)
		{
			return new bool4x2(lhs > rhs.c0, lhs > rhs.c1);
		}

		// Token: 0x06001D78 RID: 7544 RVA: 0x0005016F File Offset: 0x0004E36F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator >=(int4x2 lhs, int4x2 rhs)
		{
			return new bool4x2(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1);
		}

		// Token: 0x06001D79 RID: 7545 RVA: 0x00050198 File Offset: 0x0004E398
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator >=(int4x2 lhs, int rhs)
		{
			return new bool4x2(lhs.c0 >= rhs, lhs.c1 >= rhs);
		}

		// Token: 0x06001D7A RID: 7546 RVA: 0x000501B7 File Offset: 0x0004E3B7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator >=(int lhs, int4x2 rhs)
		{
			return new bool4x2(lhs >= rhs.c0, lhs >= rhs.c1);
		}

		// Token: 0x06001D7B RID: 7547 RVA: 0x000501D6 File Offset: 0x0004E3D6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x2 operator -(int4x2 val)
		{
			return new int4x2(-val.c0, -val.c1);
		}

		// Token: 0x06001D7C RID: 7548 RVA: 0x000501F3 File Offset: 0x0004E3F3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x2 operator +(int4x2 val)
		{
			return new int4x2(+val.c0, +val.c1);
		}

		// Token: 0x06001D7D RID: 7549 RVA: 0x00050210 File Offset: 0x0004E410
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x2 operator <<(int4x2 x, int n)
		{
			return new int4x2(x.c0 << n, x.c1 << n);
		}

		// Token: 0x06001D7E RID: 7550 RVA: 0x0005022F File Offset: 0x0004E42F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x2 operator >>(int4x2 x, int n)
		{
			return new int4x2(x.c0 >> n, x.c1 >> n);
		}

		// Token: 0x06001D7F RID: 7551 RVA: 0x0005024E File Offset: 0x0004E44E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator ==(int4x2 lhs, int4x2 rhs)
		{
			return new bool4x2(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1);
		}

		// Token: 0x06001D80 RID: 7552 RVA: 0x00050277 File Offset: 0x0004E477
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator ==(int4x2 lhs, int rhs)
		{
			return new bool4x2(lhs.c0 == rhs, lhs.c1 == rhs);
		}

		// Token: 0x06001D81 RID: 7553 RVA: 0x00050296 File Offset: 0x0004E496
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator ==(int lhs, int4x2 rhs)
		{
			return new bool4x2(lhs == rhs.c0, lhs == rhs.c1);
		}

		// Token: 0x06001D82 RID: 7554 RVA: 0x000502B5 File Offset: 0x0004E4B5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator !=(int4x2 lhs, int4x2 rhs)
		{
			return new bool4x2(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1);
		}

		// Token: 0x06001D83 RID: 7555 RVA: 0x000502DE File Offset: 0x0004E4DE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator !=(int4x2 lhs, int rhs)
		{
			return new bool4x2(lhs.c0 != rhs, lhs.c1 != rhs);
		}

		// Token: 0x06001D84 RID: 7556 RVA: 0x000502FD File Offset: 0x0004E4FD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator !=(int lhs, int4x2 rhs)
		{
			return new bool4x2(lhs != rhs.c0, lhs != rhs.c1);
		}

		// Token: 0x06001D85 RID: 7557 RVA: 0x0005031C File Offset: 0x0004E51C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x2 operator ~(int4x2 val)
		{
			return new int4x2(~val.c0, ~val.c1);
		}

		// Token: 0x06001D86 RID: 7558 RVA: 0x00050339 File Offset: 0x0004E539
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x2 operator &(int4x2 lhs, int4x2 rhs)
		{
			return new int4x2(lhs.c0 & rhs.c0, lhs.c1 & rhs.c1);
		}

		// Token: 0x06001D87 RID: 7559 RVA: 0x00050362 File Offset: 0x0004E562
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x2 operator &(int4x2 lhs, int rhs)
		{
			return new int4x2(lhs.c0 & rhs, lhs.c1 & rhs);
		}

		// Token: 0x06001D88 RID: 7560 RVA: 0x00050381 File Offset: 0x0004E581
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x2 operator &(int lhs, int4x2 rhs)
		{
			return new int4x2(lhs & rhs.c0, lhs & rhs.c1);
		}

		// Token: 0x06001D89 RID: 7561 RVA: 0x000503A0 File Offset: 0x0004E5A0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x2 operator |(int4x2 lhs, int4x2 rhs)
		{
			return new int4x2(lhs.c0 | rhs.c0, lhs.c1 | rhs.c1);
		}

		// Token: 0x06001D8A RID: 7562 RVA: 0x000503C9 File Offset: 0x0004E5C9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x2 operator |(int4x2 lhs, int rhs)
		{
			return new int4x2(lhs.c0 | rhs, lhs.c1 | rhs);
		}

		// Token: 0x06001D8B RID: 7563 RVA: 0x000503E8 File Offset: 0x0004E5E8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x2 operator |(int lhs, int4x2 rhs)
		{
			return new int4x2(lhs | rhs.c0, lhs | rhs.c1);
		}

		// Token: 0x06001D8C RID: 7564 RVA: 0x00050407 File Offset: 0x0004E607
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x2 operator ^(int4x2 lhs, int4x2 rhs)
		{
			return new int4x2(lhs.c0 ^ rhs.c0, lhs.c1 ^ rhs.c1);
		}

		// Token: 0x06001D8D RID: 7565 RVA: 0x00050430 File Offset: 0x0004E630
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x2 operator ^(int4x2 lhs, int rhs)
		{
			return new int4x2(lhs.c0 ^ rhs, lhs.c1 ^ rhs);
		}

		// Token: 0x06001D8E RID: 7566 RVA: 0x0005044F File Offset: 0x0004E64F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x2 operator ^(int lhs, int4x2 rhs)
		{
			return new int4x2(lhs ^ rhs.c0, lhs ^ rhs.c1);
		}

		// Token: 0x1700099A RID: 2458
		public unsafe ref int4 this[int index]
		{
			get
			{
				fixed (int4x2* ptr = &this)
				{
					return ref *(int4*)(ptr + (IntPtr)index * (IntPtr)sizeof(int4) / (IntPtr)sizeof(int4x2));
				}
			}
		}

		// Token: 0x06001D90 RID: 7568 RVA: 0x0005048B File Offset: 0x0004E68B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(int4x2 rhs)
		{
			return this.c0.Equals(rhs.c0) && this.c1.Equals(rhs.c1);
		}

		// Token: 0x06001D91 RID: 7569 RVA: 0x000504B4 File Offset: 0x0004E6B4
		public override bool Equals(object o)
		{
			if (o is int4x2)
			{
				int4x2 int4x = (int4x2)o;
				return this.Equals(int4x);
			}
			return false;
		}

		// Token: 0x06001D92 RID: 7570 RVA: 0x000504D9 File Offset: 0x0004E6D9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x06001D93 RID: 7571 RVA: 0x000504E8 File Offset: 0x0004E6E8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("int4x2({0}, {1},  {2}, {3},  {4}, {5},  {6}, {7})", new object[]
			{
				this.c0.x,
				this.c1.x,
				this.c0.y,
				this.c1.y,
				this.c0.z,
				this.c1.z,
				this.c0.w,
				this.c1.w
			});
		}

		// Token: 0x06001D94 RID: 7572 RVA: 0x000505A0 File Offset: 0x0004E7A0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("int4x2({0}, {1},  {2}, {3},  {4}, {5},  {6}, {7})", new object[]
			{
				this.c0.x.ToString(format, formatProvider),
				this.c1.x.ToString(format, formatProvider),
				this.c0.y.ToString(format, formatProvider),
				this.c1.y.ToString(format, formatProvider),
				this.c0.z.ToString(format, formatProvider),
				this.c1.z.ToString(format, formatProvider),
				this.c0.w.ToString(format, formatProvider),
				this.c1.w.ToString(format, formatProvider)
			});
		}

		// Token: 0x040000E3 RID: 227
		public int4 c0;

		// Token: 0x040000E4 RID: 228
		public int4 c1;

		// Token: 0x040000E5 RID: 229
		public static readonly int4x2 zero;
	}
}
