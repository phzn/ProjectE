using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x02000031 RID: 49
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct int2x4 : IEquatable<int2x4>, IFormattable
	{
		// Token: 0x06001970 RID: 6512 RVA: 0x000466D3 File Offset: 0x000448D3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2x4(int2 c0, int2 c1, int2 c2, int2 c3)
		{
			this.c0 = c0;
			this.c1 = c1;
			this.c2 = c2;
			this.c3 = c3;
		}

		// Token: 0x06001971 RID: 6513 RVA: 0x000466F2 File Offset: 0x000448F2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2x4(int m00, int m01, int m02, int m03, int m10, int m11, int m12, int m13)
		{
			this.c0 = new int2(m00, m10);
			this.c1 = new int2(m01, m11);
			this.c2 = new int2(m02, m12);
			this.c3 = new int2(m03, m13);
		}

		// Token: 0x06001972 RID: 6514 RVA: 0x0004672D File Offset: 0x0004492D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2x4(int v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
			this.c3 = v;
		}

		// Token: 0x06001973 RID: 6515 RVA: 0x00046760 File Offset: 0x00044960
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2x4(bool v)
		{
			this.c0 = math.select(new int2(0), new int2(1), v);
			this.c1 = math.select(new int2(0), new int2(1), v);
			this.c2 = math.select(new int2(0), new int2(1), v);
			this.c3 = math.select(new int2(0), new int2(1), v);
		}

		// Token: 0x06001974 RID: 6516 RVA: 0x000467D0 File Offset: 0x000449D0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2x4(bool2x4 v)
		{
			this.c0 = math.select(new int2(0), new int2(1), v.c0);
			this.c1 = math.select(new int2(0), new int2(1), v.c1);
			this.c2 = math.select(new int2(0), new int2(1), v.c2);
			this.c3 = math.select(new int2(0), new int2(1), v.c3);
		}

		// Token: 0x06001975 RID: 6517 RVA: 0x00046851 File Offset: 0x00044A51
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2x4(uint v)
		{
			this.c0 = (int2)v;
			this.c1 = (int2)v;
			this.c2 = (int2)v;
			this.c3 = (int2)v;
		}

		// Token: 0x06001976 RID: 6518 RVA: 0x00046884 File Offset: 0x00044A84
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2x4(uint2x4 v)
		{
			this.c0 = (int2)v.c0;
			this.c1 = (int2)v.c1;
			this.c2 = (int2)v.c2;
			this.c3 = (int2)v.c3;
		}

		// Token: 0x06001977 RID: 6519 RVA: 0x000468D5 File Offset: 0x00044AD5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2x4(float v)
		{
			this.c0 = (int2)v;
			this.c1 = (int2)v;
			this.c2 = (int2)v;
			this.c3 = (int2)v;
		}

		// Token: 0x06001978 RID: 6520 RVA: 0x00046908 File Offset: 0x00044B08
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2x4(float2x4 v)
		{
			this.c0 = (int2)v.c0;
			this.c1 = (int2)v.c1;
			this.c2 = (int2)v.c2;
			this.c3 = (int2)v.c3;
		}

		// Token: 0x06001979 RID: 6521 RVA: 0x00046959 File Offset: 0x00044B59
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2x4(double v)
		{
			this.c0 = (int2)v;
			this.c1 = (int2)v;
			this.c2 = (int2)v;
			this.c3 = (int2)v;
		}

		// Token: 0x0600197A RID: 6522 RVA: 0x0004698C File Offset: 0x00044B8C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2x4(double2x4 v)
		{
			this.c0 = (int2)v.c0;
			this.c1 = (int2)v.c1;
			this.c2 = (int2)v.c2;
			this.c3 = (int2)v.c3;
		}

		// Token: 0x0600197B RID: 6523 RVA: 0x000469DD File Offset: 0x00044BDD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator int2x4(int v)
		{
			return new int2x4(v);
		}

		// Token: 0x0600197C RID: 6524 RVA: 0x000469E5 File Offset: 0x00044BE5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int2x4(bool v)
		{
			return new int2x4(v);
		}

		// Token: 0x0600197D RID: 6525 RVA: 0x000469ED File Offset: 0x00044BED
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int2x4(bool2x4 v)
		{
			return new int2x4(v);
		}

		// Token: 0x0600197E RID: 6526 RVA: 0x000469F5 File Offset: 0x00044BF5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int2x4(uint v)
		{
			return new int2x4(v);
		}

		// Token: 0x0600197F RID: 6527 RVA: 0x000469FD File Offset: 0x00044BFD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int2x4(uint2x4 v)
		{
			return new int2x4(v);
		}

		// Token: 0x06001980 RID: 6528 RVA: 0x00046A05 File Offset: 0x00044C05
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int2x4(float v)
		{
			return new int2x4(v);
		}

		// Token: 0x06001981 RID: 6529 RVA: 0x00046A0D File Offset: 0x00044C0D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int2x4(float2x4 v)
		{
			return new int2x4(v);
		}

		// Token: 0x06001982 RID: 6530 RVA: 0x00046A15 File Offset: 0x00044C15
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int2x4(double v)
		{
			return new int2x4(v);
		}

		// Token: 0x06001983 RID: 6531 RVA: 0x00046A1D File Offset: 0x00044C1D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int2x4(double2x4 v)
		{
			return new int2x4(v);
		}

		// Token: 0x06001984 RID: 6532 RVA: 0x00046A28 File Offset: 0x00044C28
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x4 operator *(int2x4 lhs, int2x4 rhs)
		{
			return new int2x4(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1, lhs.c2 * rhs.c2, lhs.c3 * rhs.c3);
		}

		// Token: 0x06001985 RID: 6533 RVA: 0x00046A7E File Offset: 0x00044C7E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x4 operator *(int2x4 lhs, int rhs)
		{
			return new int2x4(lhs.c0 * rhs, lhs.c1 * rhs, lhs.c2 * rhs, lhs.c3 * rhs);
		}

		// Token: 0x06001986 RID: 6534 RVA: 0x00046AB5 File Offset: 0x00044CB5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x4 operator *(int lhs, int2x4 rhs)
		{
			return new int2x4(lhs * rhs.c0, lhs * rhs.c1, lhs * rhs.c2, lhs * rhs.c3);
		}

		// Token: 0x06001987 RID: 6535 RVA: 0x00046AEC File Offset: 0x00044CEC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x4 operator +(int2x4 lhs, int2x4 rhs)
		{
			return new int2x4(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1, lhs.c2 + rhs.c2, lhs.c3 + rhs.c3);
		}

		// Token: 0x06001988 RID: 6536 RVA: 0x00046B42 File Offset: 0x00044D42
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x4 operator +(int2x4 lhs, int rhs)
		{
			return new int2x4(lhs.c0 + rhs, lhs.c1 + rhs, lhs.c2 + rhs, lhs.c3 + rhs);
		}

		// Token: 0x06001989 RID: 6537 RVA: 0x00046B79 File Offset: 0x00044D79
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x4 operator +(int lhs, int2x4 rhs)
		{
			return new int2x4(lhs + rhs.c0, lhs + rhs.c1, lhs + rhs.c2, lhs + rhs.c3);
		}

		// Token: 0x0600198A RID: 6538 RVA: 0x00046BB0 File Offset: 0x00044DB0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x4 operator -(int2x4 lhs, int2x4 rhs)
		{
			return new int2x4(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1, lhs.c2 - rhs.c2, lhs.c3 - rhs.c3);
		}

		// Token: 0x0600198B RID: 6539 RVA: 0x00046C06 File Offset: 0x00044E06
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x4 operator -(int2x4 lhs, int rhs)
		{
			return new int2x4(lhs.c0 - rhs, lhs.c1 - rhs, lhs.c2 - rhs, lhs.c3 - rhs);
		}

		// Token: 0x0600198C RID: 6540 RVA: 0x00046C3D File Offset: 0x00044E3D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x4 operator -(int lhs, int2x4 rhs)
		{
			return new int2x4(lhs - rhs.c0, lhs - rhs.c1, lhs - rhs.c2, lhs - rhs.c3);
		}

		// Token: 0x0600198D RID: 6541 RVA: 0x00046C74 File Offset: 0x00044E74
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x4 operator /(int2x4 lhs, int2x4 rhs)
		{
			return new int2x4(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1, lhs.c2 / rhs.c2, lhs.c3 / rhs.c3);
		}

		// Token: 0x0600198E RID: 6542 RVA: 0x00046CCA File Offset: 0x00044ECA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x4 operator /(int2x4 lhs, int rhs)
		{
			return new int2x4(lhs.c0 / rhs, lhs.c1 / rhs, lhs.c2 / rhs, lhs.c3 / rhs);
		}

		// Token: 0x0600198F RID: 6543 RVA: 0x00046D01 File Offset: 0x00044F01
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x4 operator /(int lhs, int2x4 rhs)
		{
			return new int2x4(lhs / rhs.c0, lhs / rhs.c1, lhs / rhs.c2, lhs / rhs.c3);
		}

		// Token: 0x06001990 RID: 6544 RVA: 0x00046D38 File Offset: 0x00044F38
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x4 operator %(int2x4 lhs, int2x4 rhs)
		{
			return new int2x4(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1, lhs.c2 % rhs.c2, lhs.c3 % rhs.c3);
		}

		// Token: 0x06001991 RID: 6545 RVA: 0x00046D8E File Offset: 0x00044F8E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x4 operator %(int2x4 lhs, int rhs)
		{
			return new int2x4(lhs.c0 % rhs, lhs.c1 % rhs, lhs.c2 % rhs, lhs.c3 % rhs);
		}

		// Token: 0x06001992 RID: 6546 RVA: 0x00046DC5 File Offset: 0x00044FC5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x4 operator %(int lhs, int2x4 rhs)
		{
			return new int2x4(lhs % rhs.c0, lhs % rhs.c1, lhs % rhs.c2, lhs % rhs.c3);
		}

		// Token: 0x06001993 RID: 6547 RVA: 0x00046DFC File Offset: 0x00044FFC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x4 operator ++(int2x4 val)
		{
			int2 @int = int2.op_Increment(val.c0);
			val.c0 = @int;
			int2 int2 = @int;
			@int = int2.op_Increment(val.c1);
			val.c1 = @int;
			int2 int3 = @int;
			@int = int2.op_Increment(val.c2);
			val.c2 = @int;
			int2 int4 = @int;
			@int = int2.op_Increment(val.c3);
			val.c3 = @int;
			return new int2x4(int2, int3, int4, @int);
		}

		// Token: 0x06001994 RID: 6548 RVA: 0x00046E78 File Offset: 0x00045078
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x4 operator --(int2x4 val)
		{
			int2 @int = int2.op_Decrement(val.c0);
			val.c0 = @int;
			int2 int2 = @int;
			@int = int2.op_Decrement(val.c1);
			val.c1 = @int;
			int2 int3 = @int;
			@int = int2.op_Decrement(val.c2);
			val.c2 = @int;
			int2 int4 = @int;
			@int = int2.op_Decrement(val.c3);
			val.c3 = @int;
			return new int2x4(int2, int3, int4, @int);
		}

		// Token: 0x06001995 RID: 6549 RVA: 0x00046EF4 File Offset: 0x000450F4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator <(int2x4 lhs, int2x4 rhs)
		{
			return new bool2x4(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1, lhs.c2 < rhs.c2, lhs.c3 < rhs.c3);
		}

		// Token: 0x06001996 RID: 6550 RVA: 0x00046F4A File Offset: 0x0004514A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator <(int2x4 lhs, int rhs)
		{
			return new bool2x4(lhs.c0 < rhs, lhs.c1 < rhs, lhs.c2 < rhs, lhs.c3 < rhs);
		}

		// Token: 0x06001997 RID: 6551 RVA: 0x00046F81 File Offset: 0x00045181
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator <(int lhs, int2x4 rhs)
		{
			return new bool2x4(lhs < rhs.c0, lhs < rhs.c1, lhs < rhs.c2, lhs < rhs.c3);
		}

		// Token: 0x06001998 RID: 6552 RVA: 0x00046FB8 File Offset: 0x000451B8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator <=(int2x4 lhs, int2x4 rhs)
		{
			return new bool2x4(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1, lhs.c2 <= rhs.c2, lhs.c3 <= rhs.c3);
		}

		// Token: 0x06001999 RID: 6553 RVA: 0x0004700E File Offset: 0x0004520E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator <=(int2x4 lhs, int rhs)
		{
			return new bool2x4(lhs.c0 <= rhs, lhs.c1 <= rhs, lhs.c2 <= rhs, lhs.c3 <= rhs);
		}

		// Token: 0x0600199A RID: 6554 RVA: 0x00047045 File Offset: 0x00045245
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator <=(int lhs, int2x4 rhs)
		{
			return new bool2x4(lhs <= rhs.c0, lhs <= rhs.c1, lhs <= rhs.c2, lhs <= rhs.c3);
		}

		// Token: 0x0600199B RID: 6555 RVA: 0x0004707C File Offset: 0x0004527C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator >(int2x4 lhs, int2x4 rhs)
		{
			return new bool2x4(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1, lhs.c2 > rhs.c2, lhs.c3 > rhs.c3);
		}

		// Token: 0x0600199C RID: 6556 RVA: 0x000470D2 File Offset: 0x000452D2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator >(int2x4 lhs, int rhs)
		{
			return new bool2x4(lhs.c0 > rhs, lhs.c1 > rhs, lhs.c2 > rhs, lhs.c3 > rhs);
		}

		// Token: 0x0600199D RID: 6557 RVA: 0x00047109 File Offset: 0x00045309
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator >(int lhs, int2x4 rhs)
		{
			return new bool2x4(lhs > rhs.c0, lhs > rhs.c1, lhs > rhs.c2, lhs > rhs.c3);
		}

		// Token: 0x0600199E RID: 6558 RVA: 0x00047140 File Offset: 0x00045340
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator >=(int2x4 lhs, int2x4 rhs)
		{
			return new bool2x4(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1, lhs.c2 >= rhs.c2, lhs.c3 >= rhs.c3);
		}

		// Token: 0x0600199F RID: 6559 RVA: 0x00047196 File Offset: 0x00045396
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator >=(int2x4 lhs, int rhs)
		{
			return new bool2x4(lhs.c0 >= rhs, lhs.c1 >= rhs, lhs.c2 >= rhs, lhs.c3 >= rhs);
		}

		// Token: 0x060019A0 RID: 6560 RVA: 0x000471CD File Offset: 0x000453CD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator >=(int lhs, int2x4 rhs)
		{
			return new bool2x4(lhs >= rhs.c0, lhs >= rhs.c1, lhs >= rhs.c2, lhs >= rhs.c3);
		}

		// Token: 0x060019A1 RID: 6561 RVA: 0x00047204 File Offset: 0x00045404
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x4 operator -(int2x4 val)
		{
			return new int2x4(-val.c0, -val.c1, -val.c2, -val.c3);
		}

		// Token: 0x060019A2 RID: 6562 RVA: 0x00047237 File Offset: 0x00045437
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x4 operator +(int2x4 val)
		{
			return new int2x4(+val.c0, +val.c1, +val.c2, +val.c3);
		}

		// Token: 0x060019A3 RID: 6563 RVA: 0x0004726A File Offset: 0x0004546A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x4 operator <<(int2x4 x, int n)
		{
			return new int2x4(x.c0 << n, x.c1 << n, x.c2 << n, x.c3 << n);
		}

		// Token: 0x060019A4 RID: 6564 RVA: 0x000472A1 File Offset: 0x000454A1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x4 operator >>(int2x4 x, int n)
		{
			return new int2x4(x.c0 >> n, x.c1 >> n, x.c2 >> n, x.c3 >> n);
		}

		// Token: 0x060019A5 RID: 6565 RVA: 0x000472D8 File Offset: 0x000454D8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator ==(int2x4 lhs, int2x4 rhs)
		{
			return new bool2x4(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2, lhs.c3 == rhs.c3);
		}

		// Token: 0x060019A6 RID: 6566 RVA: 0x0004732E File Offset: 0x0004552E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator ==(int2x4 lhs, int rhs)
		{
			return new bool2x4(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs, lhs.c3 == rhs);
		}

		// Token: 0x060019A7 RID: 6567 RVA: 0x00047365 File Offset: 0x00045565
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator ==(int lhs, int2x4 rhs)
		{
			return new bool2x4(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2, lhs == rhs.c3);
		}

		// Token: 0x060019A8 RID: 6568 RVA: 0x0004739C File Offset: 0x0004559C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator !=(int2x4 lhs, int2x4 rhs)
		{
			return new bool2x4(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2, lhs.c3 != rhs.c3);
		}

		// Token: 0x060019A9 RID: 6569 RVA: 0x000473F2 File Offset: 0x000455F2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator !=(int2x4 lhs, int rhs)
		{
			return new bool2x4(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs, lhs.c3 != rhs);
		}

		// Token: 0x060019AA RID: 6570 RVA: 0x00047429 File Offset: 0x00045629
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator !=(int lhs, int2x4 rhs)
		{
			return new bool2x4(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2, lhs != rhs.c3);
		}

		// Token: 0x060019AB RID: 6571 RVA: 0x00047460 File Offset: 0x00045660
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x4 operator ~(int2x4 val)
		{
			return new int2x4(~val.c0, ~val.c1, ~val.c2, ~val.c3);
		}

		// Token: 0x060019AC RID: 6572 RVA: 0x00047494 File Offset: 0x00045694
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x4 operator &(int2x4 lhs, int2x4 rhs)
		{
			return new int2x4(lhs.c0 & rhs.c0, lhs.c1 & rhs.c1, lhs.c2 & rhs.c2, lhs.c3 & rhs.c3);
		}

		// Token: 0x060019AD RID: 6573 RVA: 0x000474EA File Offset: 0x000456EA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x4 operator &(int2x4 lhs, int rhs)
		{
			return new int2x4(lhs.c0 & rhs, lhs.c1 & rhs, lhs.c2 & rhs, lhs.c3 & rhs);
		}

		// Token: 0x060019AE RID: 6574 RVA: 0x00047521 File Offset: 0x00045721
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x4 operator &(int lhs, int2x4 rhs)
		{
			return new int2x4(lhs & rhs.c0, lhs & rhs.c1, lhs & rhs.c2, lhs & rhs.c3);
		}

		// Token: 0x060019AF RID: 6575 RVA: 0x00047558 File Offset: 0x00045758
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x4 operator |(int2x4 lhs, int2x4 rhs)
		{
			return new int2x4(lhs.c0 | rhs.c0, lhs.c1 | rhs.c1, lhs.c2 | rhs.c2, lhs.c3 | rhs.c3);
		}

		// Token: 0x060019B0 RID: 6576 RVA: 0x000475AE File Offset: 0x000457AE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x4 operator |(int2x4 lhs, int rhs)
		{
			return new int2x4(lhs.c0 | rhs, lhs.c1 | rhs, lhs.c2 | rhs, lhs.c3 | rhs);
		}

		// Token: 0x060019B1 RID: 6577 RVA: 0x000475E5 File Offset: 0x000457E5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x4 operator |(int lhs, int2x4 rhs)
		{
			return new int2x4(lhs | rhs.c0, lhs | rhs.c1, lhs | rhs.c2, lhs | rhs.c3);
		}

		// Token: 0x060019B2 RID: 6578 RVA: 0x0004761C File Offset: 0x0004581C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x4 operator ^(int2x4 lhs, int2x4 rhs)
		{
			return new int2x4(lhs.c0 ^ rhs.c0, lhs.c1 ^ rhs.c1, lhs.c2 ^ rhs.c2, lhs.c3 ^ rhs.c3);
		}

		// Token: 0x060019B3 RID: 6579 RVA: 0x00047672 File Offset: 0x00045872
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x4 operator ^(int2x4 lhs, int rhs)
		{
			return new int2x4(lhs.c0 ^ rhs, lhs.c1 ^ rhs, lhs.c2 ^ rhs, lhs.c3 ^ rhs);
		}

		// Token: 0x060019B4 RID: 6580 RVA: 0x000476A9 File Offset: 0x000458A9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2x4 operator ^(int lhs, int2x4 rhs)
		{
			return new int2x4(lhs ^ rhs.c0, lhs ^ rhs.c1, lhs ^ rhs.c2, lhs ^ rhs.c3);
		}

		// Token: 0x170007CF RID: 1999
		public unsafe ref int2 this[int index]
		{
			get
			{
				fixed (int2x4* ptr = &this)
				{
					return ref *(int2*)(ptr + (IntPtr)index * (IntPtr)sizeof(int2) / (IntPtr)sizeof(int2x4));
				}
			}
		}

		// Token: 0x060019B6 RID: 6582 RVA: 0x000476FC File Offset: 0x000458FC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(int2x4 rhs)
		{
			return this.c0.Equals(rhs.c0) && this.c1.Equals(rhs.c1) && this.c2.Equals(rhs.c2) && this.c3.Equals(rhs.c3);
		}

		// Token: 0x060019B7 RID: 6583 RVA: 0x00047758 File Offset: 0x00045958
		public override bool Equals(object o)
		{
			if (o is int2x4)
			{
				int2x4 int2x = (int2x4)o;
				return this.Equals(int2x);
			}
			return false;
		}

		// Token: 0x060019B8 RID: 6584 RVA: 0x0004777D File Offset: 0x0004597D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x060019B9 RID: 6585 RVA: 0x0004778C File Offset: 0x0004598C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("int2x4({0}, {1}, {2}, {3},  {4}, {5}, {6}, {7})", new object[]
			{
				this.c0.x,
				this.c1.x,
				this.c2.x,
				this.c3.x,
				this.c0.y,
				this.c1.y,
				this.c2.y,
				this.c3.y
			});
		}

		// Token: 0x060019BA RID: 6586 RVA: 0x00047844 File Offset: 0x00045A44
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("int2x4({0}, {1}, {2}, {3},  {4}, {5}, {6}, {7})", new object[]
			{
				this.c0.x.ToString(format, formatProvider),
				this.c1.x.ToString(format, formatProvider),
				this.c2.x.ToString(format, formatProvider),
				this.c3.x.ToString(format, formatProvider),
				this.c0.y.ToString(format, formatProvider),
				this.c1.y.ToString(format, formatProvider),
				this.c2.y.ToString(format, formatProvider),
				this.c3.y.ToString(format, formatProvider)
			});
		}

		// Token: 0x040000C8 RID: 200
		public int2 c0;

		// Token: 0x040000C9 RID: 201
		public int2 c1;

		// Token: 0x040000CA RID: 202
		public int2 c2;

		// Token: 0x040000CB RID: 203
		public int2 c3;

		// Token: 0x040000CC RID: 204
		public static readonly int2x4 zero;
	}
}
