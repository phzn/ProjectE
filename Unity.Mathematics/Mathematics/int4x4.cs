using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x02000039 RID: 57
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct int4x4 : IEquatable<int4x4>, IFormattable
	{
		// Token: 0x06001DE0 RID: 7648 RVA: 0x00051515 File Offset: 0x0004F715
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4x4(int4 c0, int4 c1, int4 c2, int4 c3)
		{
			this.c0 = c0;
			this.c1 = c1;
			this.c2 = c2;
			this.c3 = c3;
		}

		// Token: 0x06001DE1 RID: 7649 RVA: 0x00051534 File Offset: 0x0004F734
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4x4(int m00, int m01, int m02, int m03, int m10, int m11, int m12, int m13, int m20, int m21, int m22, int m23, int m30, int m31, int m32, int m33)
		{
			this.c0 = new int4(m00, m10, m20, m30);
			this.c1 = new int4(m01, m11, m21, m31);
			this.c2 = new int4(m02, m12, m22, m32);
			this.c3 = new int4(m03, m13, m23, m33);
		}

		// Token: 0x06001DE2 RID: 7650 RVA: 0x0005158A File Offset: 0x0004F78A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4x4(int v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
			this.c3 = v;
		}

		// Token: 0x06001DE3 RID: 7651 RVA: 0x000515BC File Offset: 0x0004F7BC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4x4(bool v)
		{
			this.c0 = math.select(new int4(0), new int4(1), v);
			this.c1 = math.select(new int4(0), new int4(1), v);
			this.c2 = math.select(new int4(0), new int4(1), v);
			this.c3 = math.select(new int4(0), new int4(1), v);
		}

		// Token: 0x06001DE4 RID: 7652 RVA: 0x0005162C File Offset: 0x0004F82C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4x4(bool4x4 v)
		{
			this.c0 = math.select(new int4(0), new int4(1), v.c0);
			this.c1 = math.select(new int4(0), new int4(1), v.c1);
			this.c2 = math.select(new int4(0), new int4(1), v.c2);
			this.c3 = math.select(new int4(0), new int4(1), v.c3);
		}

		// Token: 0x06001DE5 RID: 7653 RVA: 0x000516AD File Offset: 0x0004F8AD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4x4(uint v)
		{
			this.c0 = (int4)v;
			this.c1 = (int4)v;
			this.c2 = (int4)v;
			this.c3 = (int4)v;
		}

		// Token: 0x06001DE6 RID: 7654 RVA: 0x000516E0 File Offset: 0x0004F8E0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4x4(uint4x4 v)
		{
			this.c0 = (int4)v.c0;
			this.c1 = (int4)v.c1;
			this.c2 = (int4)v.c2;
			this.c3 = (int4)v.c3;
		}

		// Token: 0x06001DE7 RID: 7655 RVA: 0x00051731 File Offset: 0x0004F931
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4x4(float v)
		{
			this.c0 = (int4)v;
			this.c1 = (int4)v;
			this.c2 = (int4)v;
			this.c3 = (int4)v;
		}

		// Token: 0x06001DE8 RID: 7656 RVA: 0x00051764 File Offset: 0x0004F964
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4x4(float4x4 v)
		{
			this.c0 = (int4)v.c0;
			this.c1 = (int4)v.c1;
			this.c2 = (int4)v.c2;
			this.c3 = (int4)v.c3;
		}

		// Token: 0x06001DE9 RID: 7657 RVA: 0x000517B5 File Offset: 0x0004F9B5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4x4(double v)
		{
			this.c0 = (int4)v;
			this.c1 = (int4)v;
			this.c2 = (int4)v;
			this.c3 = (int4)v;
		}

		// Token: 0x06001DEA RID: 7658 RVA: 0x000517E8 File Offset: 0x0004F9E8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4x4(double4x4 v)
		{
			this.c0 = (int4)v.c0;
			this.c1 = (int4)v.c1;
			this.c2 = (int4)v.c2;
			this.c3 = (int4)v.c3;
		}

		// Token: 0x06001DEB RID: 7659 RVA: 0x00051839 File Offset: 0x0004FA39
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator int4x4(int v)
		{
			return new int4x4(v);
		}

		// Token: 0x06001DEC RID: 7660 RVA: 0x00051841 File Offset: 0x0004FA41
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int4x4(bool v)
		{
			return new int4x4(v);
		}

		// Token: 0x06001DED RID: 7661 RVA: 0x00051849 File Offset: 0x0004FA49
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int4x4(bool4x4 v)
		{
			return new int4x4(v);
		}

		// Token: 0x06001DEE RID: 7662 RVA: 0x00051851 File Offset: 0x0004FA51
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int4x4(uint v)
		{
			return new int4x4(v);
		}

		// Token: 0x06001DEF RID: 7663 RVA: 0x00051859 File Offset: 0x0004FA59
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int4x4(uint4x4 v)
		{
			return new int4x4(v);
		}

		// Token: 0x06001DF0 RID: 7664 RVA: 0x00051861 File Offset: 0x0004FA61
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int4x4(float v)
		{
			return new int4x4(v);
		}

		// Token: 0x06001DF1 RID: 7665 RVA: 0x00051869 File Offset: 0x0004FA69
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int4x4(float4x4 v)
		{
			return new int4x4(v);
		}

		// Token: 0x06001DF2 RID: 7666 RVA: 0x00051871 File Offset: 0x0004FA71
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int4x4(double v)
		{
			return new int4x4(v);
		}

		// Token: 0x06001DF3 RID: 7667 RVA: 0x00051879 File Offset: 0x0004FA79
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int4x4(double4x4 v)
		{
			return new int4x4(v);
		}

		// Token: 0x06001DF4 RID: 7668 RVA: 0x00051884 File Offset: 0x0004FA84
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x4 operator *(int4x4 lhs, int4x4 rhs)
		{
			return new int4x4(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1, lhs.c2 * rhs.c2, lhs.c3 * rhs.c3);
		}

		// Token: 0x06001DF5 RID: 7669 RVA: 0x000518DA File Offset: 0x0004FADA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x4 operator *(int4x4 lhs, int rhs)
		{
			return new int4x4(lhs.c0 * rhs, lhs.c1 * rhs, lhs.c2 * rhs, lhs.c3 * rhs);
		}

		// Token: 0x06001DF6 RID: 7670 RVA: 0x00051911 File Offset: 0x0004FB11
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x4 operator *(int lhs, int4x4 rhs)
		{
			return new int4x4(lhs * rhs.c0, lhs * rhs.c1, lhs * rhs.c2, lhs * rhs.c3);
		}

		// Token: 0x06001DF7 RID: 7671 RVA: 0x00051948 File Offset: 0x0004FB48
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x4 operator +(int4x4 lhs, int4x4 rhs)
		{
			return new int4x4(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1, lhs.c2 + rhs.c2, lhs.c3 + rhs.c3);
		}

		// Token: 0x06001DF8 RID: 7672 RVA: 0x0005199E File Offset: 0x0004FB9E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x4 operator +(int4x4 lhs, int rhs)
		{
			return new int4x4(lhs.c0 + rhs, lhs.c1 + rhs, lhs.c2 + rhs, lhs.c3 + rhs);
		}

		// Token: 0x06001DF9 RID: 7673 RVA: 0x000519D5 File Offset: 0x0004FBD5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x4 operator +(int lhs, int4x4 rhs)
		{
			return new int4x4(lhs + rhs.c0, lhs + rhs.c1, lhs + rhs.c2, lhs + rhs.c3);
		}

		// Token: 0x06001DFA RID: 7674 RVA: 0x00051A0C File Offset: 0x0004FC0C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x4 operator -(int4x4 lhs, int4x4 rhs)
		{
			return new int4x4(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1, lhs.c2 - rhs.c2, lhs.c3 - rhs.c3);
		}

		// Token: 0x06001DFB RID: 7675 RVA: 0x00051A62 File Offset: 0x0004FC62
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x4 operator -(int4x4 lhs, int rhs)
		{
			return new int4x4(lhs.c0 - rhs, lhs.c1 - rhs, lhs.c2 - rhs, lhs.c3 - rhs);
		}

		// Token: 0x06001DFC RID: 7676 RVA: 0x00051A99 File Offset: 0x0004FC99
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x4 operator -(int lhs, int4x4 rhs)
		{
			return new int4x4(lhs - rhs.c0, lhs - rhs.c1, lhs - rhs.c2, lhs - rhs.c3);
		}

		// Token: 0x06001DFD RID: 7677 RVA: 0x00051AD0 File Offset: 0x0004FCD0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x4 operator /(int4x4 lhs, int4x4 rhs)
		{
			return new int4x4(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1, lhs.c2 / rhs.c2, lhs.c3 / rhs.c3);
		}

		// Token: 0x06001DFE RID: 7678 RVA: 0x00051B26 File Offset: 0x0004FD26
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x4 operator /(int4x4 lhs, int rhs)
		{
			return new int4x4(lhs.c0 / rhs, lhs.c1 / rhs, lhs.c2 / rhs, lhs.c3 / rhs);
		}

		// Token: 0x06001DFF RID: 7679 RVA: 0x00051B5D File Offset: 0x0004FD5D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x4 operator /(int lhs, int4x4 rhs)
		{
			return new int4x4(lhs / rhs.c0, lhs / rhs.c1, lhs / rhs.c2, lhs / rhs.c3);
		}

		// Token: 0x06001E00 RID: 7680 RVA: 0x00051B94 File Offset: 0x0004FD94
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x4 operator %(int4x4 lhs, int4x4 rhs)
		{
			return new int4x4(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1, lhs.c2 % rhs.c2, lhs.c3 % rhs.c3);
		}

		// Token: 0x06001E01 RID: 7681 RVA: 0x00051BEA File Offset: 0x0004FDEA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x4 operator %(int4x4 lhs, int rhs)
		{
			return new int4x4(lhs.c0 % rhs, lhs.c1 % rhs, lhs.c2 % rhs, lhs.c3 % rhs);
		}

		// Token: 0x06001E02 RID: 7682 RVA: 0x00051C21 File Offset: 0x0004FE21
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x4 operator %(int lhs, int4x4 rhs)
		{
			return new int4x4(lhs % rhs.c0, lhs % rhs.c1, lhs % rhs.c2, lhs % rhs.c3);
		}

		// Token: 0x06001E03 RID: 7683 RVA: 0x00051C58 File Offset: 0x0004FE58
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x4 operator ++(int4x4 val)
		{
			int4 @int = int4.op_Increment(val.c0);
			val.c0 = @int;
			int4 int2 = @int;
			@int = int4.op_Increment(val.c1);
			val.c1 = @int;
			int4 int3 = @int;
			@int = int4.op_Increment(val.c2);
			val.c2 = @int;
			int4 int4 = @int;
			@int = int4.op_Increment(val.c3);
			val.c3 = @int;
			return new int4x4(int2, int3, int4, @int);
		}

		// Token: 0x06001E04 RID: 7684 RVA: 0x00051CD4 File Offset: 0x0004FED4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x4 operator --(int4x4 val)
		{
			int4 @int = int4.op_Decrement(val.c0);
			val.c0 = @int;
			int4 int2 = @int;
			@int = int4.op_Decrement(val.c1);
			val.c1 = @int;
			int4 int3 = @int;
			@int = int4.op_Decrement(val.c2);
			val.c2 = @int;
			int4 int4 = @int;
			@int = int4.op_Decrement(val.c3);
			val.c3 = @int;
			return new int4x4(int2, int3, int4, @int);
		}

		// Token: 0x06001E05 RID: 7685 RVA: 0x00051D50 File Offset: 0x0004FF50
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator <(int4x4 lhs, int4x4 rhs)
		{
			return new bool4x4(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1, lhs.c2 < rhs.c2, lhs.c3 < rhs.c3);
		}

		// Token: 0x06001E06 RID: 7686 RVA: 0x00051DA6 File Offset: 0x0004FFA6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator <(int4x4 lhs, int rhs)
		{
			return new bool4x4(lhs.c0 < rhs, lhs.c1 < rhs, lhs.c2 < rhs, lhs.c3 < rhs);
		}

		// Token: 0x06001E07 RID: 7687 RVA: 0x00051DDD File Offset: 0x0004FFDD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator <(int lhs, int4x4 rhs)
		{
			return new bool4x4(lhs < rhs.c0, lhs < rhs.c1, lhs < rhs.c2, lhs < rhs.c3);
		}

		// Token: 0x06001E08 RID: 7688 RVA: 0x00051E14 File Offset: 0x00050014
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator <=(int4x4 lhs, int4x4 rhs)
		{
			return new bool4x4(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1, lhs.c2 <= rhs.c2, lhs.c3 <= rhs.c3);
		}

		// Token: 0x06001E09 RID: 7689 RVA: 0x00051E6A File Offset: 0x0005006A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator <=(int4x4 lhs, int rhs)
		{
			return new bool4x4(lhs.c0 <= rhs, lhs.c1 <= rhs, lhs.c2 <= rhs, lhs.c3 <= rhs);
		}

		// Token: 0x06001E0A RID: 7690 RVA: 0x00051EA1 File Offset: 0x000500A1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator <=(int lhs, int4x4 rhs)
		{
			return new bool4x4(lhs <= rhs.c0, lhs <= rhs.c1, lhs <= rhs.c2, lhs <= rhs.c3);
		}

		// Token: 0x06001E0B RID: 7691 RVA: 0x00051ED8 File Offset: 0x000500D8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator >(int4x4 lhs, int4x4 rhs)
		{
			return new bool4x4(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1, lhs.c2 > rhs.c2, lhs.c3 > rhs.c3);
		}

		// Token: 0x06001E0C RID: 7692 RVA: 0x00051F2E File Offset: 0x0005012E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator >(int4x4 lhs, int rhs)
		{
			return new bool4x4(lhs.c0 > rhs, lhs.c1 > rhs, lhs.c2 > rhs, lhs.c3 > rhs);
		}

		// Token: 0x06001E0D RID: 7693 RVA: 0x00051F65 File Offset: 0x00050165
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator >(int lhs, int4x4 rhs)
		{
			return new bool4x4(lhs > rhs.c0, lhs > rhs.c1, lhs > rhs.c2, lhs > rhs.c3);
		}

		// Token: 0x06001E0E RID: 7694 RVA: 0x00051F9C File Offset: 0x0005019C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator >=(int4x4 lhs, int4x4 rhs)
		{
			return new bool4x4(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1, lhs.c2 >= rhs.c2, lhs.c3 >= rhs.c3);
		}

		// Token: 0x06001E0F RID: 7695 RVA: 0x00051FF2 File Offset: 0x000501F2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator >=(int4x4 lhs, int rhs)
		{
			return new bool4x4(lhs.c0 >= rhs, lhs.c1 >= rhs, lhs.c2 >= rhs, lhs.c3 >= rhs);
		}

		// Token: 0x06001E10 RID: 7696 RVA: 0x00052029 File Offset: 0x00050229
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator >=(int lhs, int4x4 rhs)
		{
			return new bool4x4(lhs >= rhs.c0, lhs >= rhs.c1, lhs >= rhs.c2, lhs >= rhs.c3);
		}

		// Token: 0x06001E11 RID: 7697 RVA: 0x00052060 File Offset: 0x00050260
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x4 operator -(int4x4 val)
		{
			return new int4x4(-val.c0, -val.c1, -val.c2, -val.c3);
		}

		// Token: 0x06001E12 RID: 7698 RVA: 0x00052093 File Offset: 0x00050293
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x4 operator +(int4x4 val)
		{
			return new int4x4(+val.c0, +val.c1, +val.c2, +val.c3);
		}

		// Token: 0x06001E13 RID: 7699 RVA: 0x000520C6 File Offset: 0x000502C6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x4 operator <<(int4x4 x, int n)
		{
			return new int4x4(x.c0 << n, x.c1 << n, x.c2 << n, x.c3 << n);
		}

		// Token: 0x06001E14 RID: 7700 RVA: 0x000520FD File Offset: 0x000502FD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x4 operator >>(int4x4 x, int n)
		{
			return new int4x4(x.c0 >> n, x.c1 >> n, x.c2 >> n, x.c3 >> n);
		}

		// Token: 0x06001E15 RID: 7701 RVA: 0x00052134 File Offset: 0x00050334
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator ==(int4x4 lhs, int4x4 rhs)
		{
			return new bool4x4(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2, lhs.c3 == rhs.c3);
		}

		// Token: 0x06001E16 RID: 7702 RVA: 0x0005218A File Offset: 0x0005038A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator ==(int4x4 lhs, int rhs)
		{
			return new bool4x4(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs, lhs.c3 == rhs);
		}

		// Token: 0x06001E17 RID: 7703 RVA: 0x000521C1 File Offset: 0x000503C1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator ==(int lhs, int4x4 rhs)
		{
			return new bool4x4(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2, lhs == rhs.c3);
		}

		// Token: 0x06001E18 RID: 7704 RVA: 0x000521F8 File Offset: 0x000503F8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator !=(int4x4 lhs, int4x4 rhs)
		{
			return new bool4x4(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2, lhs.c3 != rhs.c3);
		}

		// Token: 0x06001E19 RID: 7705 RVA: 0x0005224E File Offset: 0x0005044E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator !=(int4x4 lhs, int rhs)
		{
			return new bool4x4(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs, lhs.c3 != rhs);
		}

		// Token: 0x06001E1A RID: 7706 RVA: 0x00052285 File Offset: 0x00050485
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator !=(int lhs, int4x4 rhs)
		{
			return new bool4x4(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2, lhs != rhs.c3);
		}

		// Token: 0x06001E1B RID: 7707 RVA: 0x000522BC File Offset: 0x000504BC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x4 operator ~(int4x4 val)
		{
			return new int4x4(~val.c0, ~val.c1, ~val.c2, ~val.c3);
		}

		// Token: 0x06001E1C RID: 7708 RVA: 0x000522F0 File Offset: 0x000504F0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x4 operator &(int4x4 lhs, int4x4 rhs)
		{
			return new int4x4(lhs.c0 & rhs.c0, lhs.c1 & rhs.c1, lhs.c2 & rhs.c2, lhs.c3 & rhs.c3);
		}

		// Token: 0x06001E1D RID: 7709 RVA: 0x00052346 File Offset: 0x00050546
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x4 operator &(int4x4 lhs, int rhs)
		{
			return new int4x4(lhs.c0 & rhs, lhs.c1 & rhs, lhs.c2 & rhs, lhs.c3 & rhs);
		}

		// Token: 0x06001E1E RID: 7710 RVA: 0x0005237D File Offset: 0x0005057D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x4 operator &(int lhs, int4x4 rhs)
		{
			return new int4x4(lhs & rhs.c0, lhs & rhs.c1, lhs & rhs.c2, lhs & rhs.c3);
		}

		// Token: 0x06001E1F RID: 7711 RVA: 0x000523B4 File Offset: 0x000505B4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x4 operator |(int4x4 lhs, int4x4 rhs)
		{
			return new int4x4(lhs.c0 | rhs.c0, lhs.c1 | rhs.c1, lhs.c2 | rhs.c2, lhs.c3 | rhs.c3);
		}

		// Token: 0x06001E20 RID: 7712 RVA: 0x0005240A File Offset: 0x0005060A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x4 operator |(int4x4 lhs, int rhs)
		{
			return new int4x4(lhs.c0 | rhs, lhs.c1 | rhs, lhs.c2 | rhs, lhs.c3 | rhs);
		}

		// Token: 0x06001E21 RID: 7713 RVA: 0x00052441 File Offset: 0x00050641
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x4 operator |(int lhs, int4x4 rhs)
		{
			return new int4x4(lhs | rhs.c0, lhs | rhs.c1, lhs | rhs.c2, lhs | rhs.c3);
		}

		// Token: 0x06001E22 RID: 7714 RVA: 0x00052478 File Offset: 0x00050678
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x4 operator ^(int4x4 lhs, int4x4 rhs)
		{
			return new int4x4(lhs.c0 ^ rhs.c0, lhs.c1 ^ rhs.c1, lhs.c2 ^ rhs.c2, lhs.c3 ^ rhs.c3);
		}

		// Token: 0x06001E23 RID: 7715 RVA: 0x000524CE File Offset: 0x000506CE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x4 operator ^(int4x4 lhs, int rhs)
		{
			return new int4x4(lhs.c0 ^ rhs, lhs.c1 ^ rhs, lhs.c2 ^ rhs, lhs.c3 ^ rhs);
		}

		// Token: 0x06001E24 RID: 7716 RVA: 0x00052505 File Offset: 0x00050705
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x4 operator ^(int lhs, int4x4 rhs)
		{
			return new int4x4(lhs ^ rhs.c0, lhs ^ rhs.c1, lhs ^ rhs.c2, lhs ^ rhs.c3);
		}

		// Token: 0x1700099C RID: 2460
		public unsafe ref int4 this[int index]
		{
			get
			{
				fixed (int4x4* ptr = &this)
				{
					return ref *(int4*)(ptr + (IntPtr)index * (IntPtr)sizeof(int4) / (IntPtr)sizeof(int4x4));
				}
			}
		}

		// Token: 0x06001E26 RID: 7718 RVA: 0x00052558 File Offset: 0x00050758
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(int4x4 rhs)
		{
			return this.c0.Equals(rhs.c0) && this.c1.Equals(rhs.c1) && this.c2.Equals(rhs.c2) && this.c3.Equals(rhs.c3);
		}

		// Token: 0x06001E27 RID: 7719 RVA: 0x000525B4 File Offset: 0x000507B4
		public override bool Equals(object o)
		{
			if (o is int4x4)
			{
				int4x4 int4x = (int4x4)o;
				return this.Equals(int4x);
			}
			return false;
		}

		// Token: 0x06001E28 RID: 7720 RVA: 0x000525D9 File Offset: 0x000507D9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x06001E29 RID: 7721 RVA: 0x000525E8 File Offset: 0x000507E8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("int4x4({0}, {1}, {2}, {3},  {4}, {5}, {6}, {7},  {8}, {9}, {10}, {11},  {12}, {13}, {14}, {15})", new object[]
			{
				this.c0.x,
				this.c1.x,
				this.c2.x,
				this.c3.x,
				this.c0.y,
				this.c1.y,
				this.c2.y,
				this.c3.y,
				this.c0.z,
				this.c1.z,
				this.c2.z,
				this.c3.z,
				this.c0.w,
				this.c1.w,
				this.c2.w,
				this.c3.w
			});
		}

		// Token: 0x06001E2A RID: 7722 RVA: 0x00052740 File Offset: 0x00050940
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("int4x4({0}, {1}, {2}, {3},  {4}, {5}, {6}, {7},  {8}, {9}, {10}, {11},  {12}, {13}, {14}, {15})", new object[]
			{
				this.c0.x.ToString(format, formatProvider),
				this.c1.x.ToString(format, formatProvider),
				this.c2.x.ToString(format, formatProvider),
				this.c3.x.ToString(format, formatProvider),
				this.c0.y.ToString(format, formatProvider),
				this.c1.y.ToString(format, formatProvider),
				this.c2.y.ToString(format, formatProvider),
				this.c3.y.ToString(format, formatProvider),
				this.c0.z.ToString(format, formatProvider),
				this.c1.z.ToString(format, formatProvider),
				this.c2.z.ToString(format, formatProvider),
				this.c3.z.ToString(format, formatProvider),
				this.c0.w.ToString(format, formatProvider),
				this.c1.w.ToString(format, formatProvider),
				this.c2.w.ToString(format, formatProvider),
				this.c3.w.ToString(format, formatProvider)
			});
		}

		// Token: 0x040000EA RID: 234
		public int4 c0;

		// Token: 0x040000EB RID: 235
		public int4 c1;

		// Token: 0x040000EC RID: 236
		public int4 c2;

		// Token: 0x040000ED RID: 237
		public int4 c3;

		// Token: 0x040000EE RID: 238
		public static readonly int4x4 identity = new int4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);

		// Token: 0x040000EF RID: 239
		public static readonly int4x4 zero;
	}
}
