using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x02000038 RID: 56
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct int4x3 : IEquatable<int4x3>, IFormattable
	{
		// Token: 0x06001D95 RID: 7573 RVA: 0x00050665 File Offset: 0x0004E865
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4x3(int4 c0, int4 c1, int4 c2)
		{
			this.c0 = c0;
			this.c1 = c1;
			this.c2 = c2;
		}

		// Token: 0x06001D96 RID: 7574 RVA: 0x0005067C File Offset: 0x0004E87C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4x3(int m00, int m01, int m02, int m10, int m11, int m12, int m20, int m21, int m22, int m30, int m31, int m32)
		{
			this.c0 = new int4(m00, m10, m20, m30);
			this.c1 = new int4(m01, m11, m21, m31);
			this.c2 = new int4(m02, m12, m22, m32);
		}

		// Token: 0x06001D97 RID: 7575 RVA: 0x000506B4 File Offset: 0x0004E8B4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4x3(int v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
		}

		// Token: 0x06001D98 RID: 7576 RVA: 0x000506DC File Offset: 0x0004E8DC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4x3(bool v)
		{
			this.c0 = math.select(new int4(0), new int4(1), v);
			this.c1 = math.select(new int4(0), new int4(1), v);
			this.c2 = math.select(new int4(0), new int4(1), v);
		}

		// Token: 0x06001D99 RID: 7577 RVA: 0x00050734 File Offset: 0x0004E934
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4x3(bool4x3 v)
		{
			this.c0 = math.select(new int4(0), new int4(1), v.c0);
			this.c1 = math.select(new int4(0), new int4(1), v.c1);
			this.c2 = math.select(new int4(0), new int4(1), v.c2);
		}

		// Token: 0x06001D9A RID: 7578 RVA: 0x00050798 File Offset: 0x0004E998
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4x3(uint v)
		{
			this.c0 = (int4)v;
			this.c1 = (int4)v;
			this.c2 = (int4)v;
		}

		// Token: 0x06001D9B RID: 7579 RVA: 0x000507BE File Offset: 0x0004E9BE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4x3(uint4x3 v)
		{
			this.c0 = (int4)v.c0;
			this.c1 = (int4)v.c1;
			this.c2 = (int4)v.c2;
		}

		// Token: 0x06001D9C RID: 7580 RVA: 0x000507F3 File Offset: 0x0004E9F3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4x3(float v)
		{
			this.c0 = (int4)v;
			this.c1 = (int4)v;
			this.c2 = (int4)v;
		}

		// Token: 0x06001D9D RID: 7581 RVA: 0x00050819 File Offset: 0x0004EA19
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4x3(float4x3 v)
		{
			this.c0 = (int4)v.c0;
			this.c1 = (int4)v.c1;
			this.c2 = (int4)v.c2;
		}

		// Token: 0x06001D9E RID: 7582 RVA: 0x0005084E File Offset: 0x0004EA4E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4x3(double v)
		{
			this.c0 = (int4)v;
			this.c1 = (int4)v;
			this.c2 = (int4)v;
		}

		// Token: 0x06001D9F RID: 7583 RVA: 0x00050874 File Offset: 0x0004EA74
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int4x3(double4x3 v)
		{
			this.c0 = (int4)v.c0;
			this.c1 = (int4)v.c1;
			this.c2 = (int4)v.c2;
		}

		// Token: 0x06001DA0 RID: 7584 RVA: 0x000508A9 File Offset: 0x0004EAA9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator int4x3(int v)
		{
			return new int4x3(v);
		}

		// Token: 0x06001DA1 RID: 7585 RVA: 0x000508B1 File Offset: 0x0004EAB1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int4x3(bool v)
		{
			return new int4x3(v);
		}

		// Token: 0x06001DA2 RID: 7586 RVA: 0x000508B9 File Offset: 0x0004EAB9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int4x3(bool4x3 v)
		{
			return new int4x3(v);
		}

		// Token: 0x06001DA3 RID: 7587 RVA: 0x000508C1 File Offset: 0x0004EAC1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int4x3(uint v)
		{
			return new int4x3(v);
		}

		// Token: 0x06001DA4 RID: 7588 RVA: 0x000508C9 File Offset: 0x0004EAC9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int4x3(uint4x3 v)
		{
			return new int4x3(v);
		}

		// Token: 0x06001DA5 RID: 7589 RVA: 0x000508D1 File Offset: 0x0004EAD1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int4x3(float v)
		{
			return new int4x3(v);
		}

		// Token: 0x06001DA6 RID: 7590 RVA: 0x000508D9 File Offset: 0x0004EAD9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int4x3(float4x3 v)
		{
			return new int4x3(v);
		}

		// Token: 0x06001DA7 RID: 7591 RVA: 0x000508E1 File Offset: 0x0004EAE1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int4x3(double v)
		{
			return new int4x3(v);
		}

		// Token: 0x06001DA8 RID: 7592 RVA: 0x000508E9 File Offset: 0x0004EAE9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int4x3(double4x3 v)
		{
			return new int4x3(v);
		}

		// Token: 0x06001DA9 RID: 7593 RVA: 0x000508F1 File Offset: 0x0004EAF1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x3 operator *(int4x3 lhs, int4x3 rhs)
		{
			return new int4x3(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1, lhs.c2 * rhs.c2);
		}

		// Token: 0x06001DAA RID: 7594 RVA: 0x0005092B File Offset: 0x0004EB2B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x3 operator *(int4x3 lhs, int rhs)
		{
			return new int4x3(lhs.c0 * rhs, lhs.c1 * rhs, lhs.c2 * rhs);
		}

		// Token: 0x06001DAB RID: 7595 RVA: 0x00050956 File Offset: 0x0004EB56
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x3 operator *(int lhs, int4x3 rhs)
		{
			return new int4x3(lhs * rhs.c0, lhs * rhs.c1, lhs * rhs.c2);
		}

		// Token: 0x06001DAC RID: 7596 RVA: 0x00050981 File Offset: 0x0004EB81
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x3 operator +(int4x3 lhs, int4x3 rhs)
		{
			return new int4x3(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1, lhs.c2 + rhs.c2);
		}

		// Token: 0x06001DAD RID: 7597 RVA: 0x000509BB File Offset: 0x0004EBBB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x3 operator +(int4x3 lhs, int rhs)
		{
			return new int4x3(lhs.c0 + rhs, lhs.c1 + rhs, lhs.c2 + rhs);
		}

		// Token: 0x06001DAE RID: 7598 RVA: 0x000509E6 File Offset: 0x0004EBE6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x3 operator +(int lhs, int4x3 rhs)
		{
			return new int4x3(lhs + rhs.c0, lhs + rhs.c1, lhs + rhs.c2);
		}

		// Token: 0x06001DAF RID: 7599 RVA: 0x00050A11 File Offset: 0x0004EC11
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x3 operator -(int4x3 lhs, int4x3 rhs)
		{
			return new int4x3(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1, lhs.c2 - rhs.c2);
		}

		// Token: 0x06001DB0 RID: 7600 RVA: 0x00050A4B File Offset: 0x0004EC4B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x3 operator -(int4x3 lhs, int rhs)
		{
			return new int4x3(lhs.c0 - rhs, lhs.c1 - rhs, lhs.c2 - rhs);
		}

		// Token: 0x06001DB1 RID: 7601 RVA: 0x00050A76 File Offset: 0x0004EC76
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x3 operator -(int lhs, int4x3 rhs)
		{
			return new int4x3(lhs - rhs.c0, lhs - rhs.c1, lhs - rhs.c2);
		}

		// Token: 0x06001DB2 RID: 7602 RVA: 0x00050AA1 File Offset: 0x0004ECA1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x3 operator /(int4x3 lhs, int4x3 rhs)
		{
			return new int4x3(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1, lhs.c2 / rhs.c2);
		}

		// Token: 0x06001DB3 RID: 7603 RVA: 0x00050ADB File Offset: 0x0004ECDB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x3 operator /(int4x3 lhs, int rhs)
		{
			return new int4x3(lhs.c0 / rhs, lhs.c1 / rhs, lhs.c2 / rhs);
		}

		// Token: 0x06001DB4 RID: 7604 RVA: 0x00050B06 File Offset: 0x0004ED06
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x3 operator /(int lhs, int4x3 rhs)
		{
			return new int4x3(lhs / rhs.c0, lhs / rhs.c1, lhs / rhs.c2);
		}

		// Token: 0x06001DB5 RID: 7605 RVA: 0x00050B31 File Offset: 0x0004ED31
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x3 operator %(int4x3 lhs, int4x3 rhs)
		{
			return new int4x3(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1, lhs.c2 % rhs.c2);
		}

		// Token: 0x06001DB6 RID: 7606 RVA: 0x00050B6B File Offset: 0x0004ED6B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x3 operator %(int4x3 lhs, int rhs)
		{
			return new int4x3(lhs.c0 % rhs, lhs.c1 % rhs, lhs.c2 % rhs);
		}

		// Token: 0x06001DB7 RID: 7607 RVA: 0x00050B96 File Offset: 0x0004ED96
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x3 operator %(int lhs, int4x3 rhs)
		{
			return new int4x3(lhs % rhs.c0, lhs % rhs.c1, lhs % rhs.c2);
		}

		// Token: 0x06001DB8 RID: 7608 RVA: 0x00050BC4 File Offset: 0x0004EDC4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x3 operator ++(int4x3 val)
		{
			int4 @int = int4.op_Increment(val.c0);
			val.c0 = @int;
			int4 int2 = @int;
			@int = int4.op_Increment(val.c1);
			val.c1 = @int;
			int4 int3 = @int;
			@int = int4.op_Increment(val.c2);
			val.c2 = @int;
			return new int4x3(int2, int3, @int);
		}

		// Token: 0x06001DB9 RID: 7609 RVA: 0x00050C24 File Offset: 0x0004EE24
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x3 operator --(int4x3 val)
		{
			int4 @int = int4.op_Decrement(val.c0);
			val.c0 = @int;
			int4 int2 = @int;
			@int = int4.op_Decrement(val.c1);
			val.c1 = @int;
			int4 int3 = @int;
			@int = int4.op_Decrement(val.c2);
			val.c2 = @int;
			return new int4x3(int2, int3, @int);
		}

		// Token: 0x06001DBA RID: 7610 RVA: 0x00050C84 File Offset: 0x0004EE84
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator <(int4x3 lhs, int4x3 rhs)
		{
			return new bool4x3(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1, lhs.c2 < rhs.c2);
		}

		// Token: 0x06001DBB RID: 7611 RVA: 0x00050CBE File Offset: 0x0004EEBE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator <(int4x3 lhs, int rhs)
		{
			return new bool4x3(lhs.c0 < rhs, lhs.c1 < rhs, lhs.c2 < rhs);
		}

		// Token: 0x06001DBC RID: 7612 RVA: 0x00050CE9 File Offset: 0x0004EEE9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator <(int lhs, int4x3 rhs)
		{
			return new bool4x3(lhs < rhs.c0, lhs < rhs.c1, lhs < rhs.c2);
		}

		// Token: 0x06001DBD RID: 7613 RVA: 0x00050D14 File Offset: 0x0004EF14
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator <=(int4x3 lhs, int4x3 rhs)
		{
			return new bool4x3(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1, lhs.c2 <= rhs.c2);
		}

		// Token: 0x06001DBE RID: 7614 RVA: 0x00050D4E File Offset: 0x0004EF4E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator <=(int4x3 lhs, int rhs)
		{
			return new bool4x3(lhs.c0 <= rhs, lhs.c1 <= rhs, lhs.c2 <= rhs);
		}

		// Token: 0x06001DBF RID: 7615 RVA: 0x00050D79 File Offset: 0x0004EF79
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator <=(int lhs, int4x3 rhs)
		{
			return new bool4x3(lhs <= rhs.c0, lhs <= rhs.c1, lhs <= rhs.c2);
		}

		// Token: 0x06001DC0 RID: 7616 RVA: 0x00050DA4 File Offset: 0x0004EFA4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator >(int4x3 lhs, int4x3 rhs)
		{
			return new bool4x3(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1, lhs.c2 > rhs.c2);
		}

		// Token: 0x06001DC1 RID: 7617 RVA: 0x00050DDE File Offset: 0x0004EFDE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator >(int4x3 lhs, int rhs)
		{
			return new bool4x3(lhs.c0 > rhs, lhs.c1 > rhs, lhs.c2 > rhs);
		}

		// Token: 0x06001DC2 RID: 7618 RVA: 0x00050E09 File Offset: 0x0004F009
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator >(int lhs, int4x3 rhs)
		{
			return new bool4x3(lhs > rhs.c0, lhs > rhs.c1, lhs > rhs.c2);
		}

		// Token: 0x06001DC3 RID: 7619 RVA: 0x00050E34 File Offset: 0x0004F034
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator >=(int4x3 lhs, int4x3 rhs)
		{
			return new bool4x3(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1, lhs.c2 >= rhs.c2);
		}

		// Token: 0x06001DC4 RID: 7620 RVA: 0x00050E6E File Offset: 0x0004F06E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator >=(int4x3 lhs, int rhs)
		{
			return new bool4x3(lhs.c0 >= rhs, lhs.c1 >= rhs, lhs.c2 >= rhs);
		}

		// Token: 0x06001DC5 RID: 7621 RVA: 0x00050E99 File Offset: 0x0004F099
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator >=(int lhs, int4x3 rhs)
		{
			return new bool4x3(lhs >= rhs.c0, lhs >= rhs.c1, lhs >= rhs.c2);
		}

		// Token: 0x06001DC6 RID: 7622 RVA: 0x00050EC4 File Offset: 0x0004F0C4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x3 operator -(int4x3 val)
		{
			return new int4x3(-val.c0, -val.c1, -val.c2);
		}

		// Token: 0x06001DC7 RID: 7623 RVA: 0x00050EEC File Offset: 0x0004F0EC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x3 operator +(int4x3 val)
		{
			return new int4x3(+val.c0, +val.c1, +val.c2);
		}

		// Token: 0x06001DC8 RID: 7624 RVA: 0x00050F14 File Offset: 0x0004F114
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x3 operator <<(int4x3 x, int n)
		{
			return new int4x3(x.c0 << n, x.c1 << n, x.c2 << n);
		}

		// Token: 0x06001DC9 RID: 7625 RVA: 0x00050F3F File Offset: 0x0004F13F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x3 operator >>(int4x3 x, int n)
		{
			return new int4x3(x.c0 >> n, x.c1 >> n, x.c2 >> n);
		}

		// Token: 0x06001DCA RID: 7626 RVA: 0x00050F6A File Offset: 0x0004F16A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator ==(int4x3 lhs, int4x3 rhs)
		{
			return new bool4x3(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2);
		}

		// Token: 0x06001DCB RID: 7627 RVA: 0x00050FA4 File Offset: 0x0004F1A4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator ==(int4x3 lhs, int rhs)
		{
			return new bool4x3(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs);
		}

		// Token: 0x06001DCC RID: 7628 RVA: 0x00050FCF File Offset: 0x0004F1CF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator ==(int lhs, int4x3 rhs)
		{
			return new bool4x3(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2);
		}

		// Token: 0x06001DCD RID: 7629 RVA: 0x00050FFA File Offset: 0x0004F1FA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator !=(int4x3 lhs, int4x3 rhs)
		{
			return new bool4x3(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2);
		}

		// Token: 0x06001DCE RID: 7630 RVA: 0x00051034 File Offset: 0x0004F234
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator !=(int4x3 lhs, int rhs)
		{
			return new bool4x3(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs);
		}

		// Token: 0x06001DCF RID: 7631 RVA: 0x0005105F File Offset: 0x0004F25F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator !=(int lhs, int4x3 rhs)
		{
			return new bool4x3(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2);
		}

		// Token: 0x06001DD0 RID: 7632 RVA: 0x0005108A File Offset: 0x0004F28A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x3 operator ~(int4x3 val)
		{
			return new int4x3(~val.c0, ~val.c1, ~val.c2);
		}

		// Token: 0x06001DD1 RID: 7633 RVA: 0x000510B2 File Offset: 0x0004F2B2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x3 operator &(int4x3 lhs, int4x3 rhs)
		{
			return new int4x3(lhs.c0 & rhs.c0, lhs.c1 & rhs.c1, lhs.c2 & rhs.c2);
		}

		// Token: 0x06001DD2 RID: 7634 RVA: 0x000510EC File Offset: 0x0004F2EC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x3 operator &(int4x3 lhs, int rhs)
		{
			return new int4x3(lhs.c0 & rhs, lhs.c1 & rhs, lhs.c2 & rhs);
		}

		// Token: 0x06001DD3 RID: 7635 RVA: 0x00051117 File Offset: 0x0004F317
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x3 operator &(int lhs, int4x3 rhs)
		{
			return new int4x3(lhs & rhs.c0, lhs & rhs.c1, lhs & rhs.c2);
		}

		// Token: 0x06001DD4 RID: 7636 RVA: 0x00051142 File Offset: 0x0004F342
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x3 operator |(int4x3 lhs, int4x3 rhs)
		{
			return new int4x3(lhs.c0 | rhs.c0, lhs.c1 | rhs.c1, lhs.c2 | rhs.c2);
		}

		// Token: 0x06001DD5 RID: 7637 RVA: 0x0005117C File Offset: 0x0004F37C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x3 operator |(int4x3 lhs, int rhs)
		{
			return new int4x3(lhs.c0 | rhs, lhs.c1 | rhs, lhs.c2 | rhs);
		}

		// Token: 0x06001DD6 RID: 7638 RVA: 0x000511A7 File Offset: 0x0004F3A7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x3 operator |(int lhs, int4x3 rhs)
		{
			return new int4x3(lhs | rhs.c0, lhs | rhs.c1, lhs | rhs.c2);
		}

		// Token: 0x06001DD7 RID: 7639 RVA: 0x000511D2 File Offset: 0x0004F3D2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x3 operator ^(int4x3 lhs, int4x3 rhs)
		{
			return new int4x3(lhs.c0 ^ rhs.c0, lhs.c1 ^ rhs.c1, lhs.c2 ^ rhs.c2);
		}

		// Token: 0x06001DD8 RID: 7640 RVA: 0x0005120C File Offset: 0x0004F40C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x3 operator ^(int4x3 lhs, int rhs)
		{
			return new int4x3(lhs.c0 ^ rhs, lhs.c1 ^ rhs, lhs.c2 ^ rhs);
		}

		// Token: 0x06001DD9 RID: 7641 RVA: 0x00051237 File Offset: 0x0004F437
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int4x3 operator ^(int lhs, int4x3 rhs)
		{
			return new int4x3(lhs ^ rhs.c0, lhs ^ rhs.c1, lhs ^ rhs.c2);
		}

		// Token: 0x1700099B RID: 2459
		public unsafe ref int4 this[int index]
		{
			get
			{
				fixed (int4x3* ptr = &this)
				{
					return ref *(int4*)(ptr + (IntPtr)index * (IntPtr)sizeof(int4) / (IntPtr)sizeof(int4x3));
				}
			}
		}

		// Token: 0x06001DDB RID: 7643 RVA: 0x0005127F File Offset: 0x0004F47F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(int4x3 rhs)
		{
			return this.c0.Equals(rhs.c0) && this.c1.Equals(rhs.c1) && this.c2.Equals(rhs.c2);
		}

		// Token: 0x06001DDC RID: 7644 RVA: 0x000512BC File Offset: 0x0004F4BC
		public override bool Equals(object o)
		{
			if (o is int4x3)
			{
				int4x3 int4x = (int4x3)o;
				return this.Equals(int4x);
			}
			return false;
		}

		// Token: 0x06001DDD RID: 7645 RVA: 0x000512E1 File Offset: 0x0004F4E1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x06001DDE RID: 7646 RVA: 0x000512F0 File Offset: 0x0004F4F0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("int4x3({0}, {1}, {2},  {3}, {4}, {5},  {6}, {7}, {8},  {9}, {10}, {11})", new object[]
			{
				this.c0.x,
				this.c1.x,
				this.c2.x,
				this.c0.y,
				this.c1.y,
				this.c2.y,
				this.c0.z,
				this.c1.z,
				this.c2.z,
				this.c0.w,
				this.c1.w,
				this.c2.w
			});
		}

		// Token: 0x06001DDF RID: 7647 RVA: 0x000513F8 File Offset: 0x0004F5F8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("int4x3({0}, {1}, {2},  {3}, {4}, {5},  {6}, {7}, {8},  {9}, {10}, {11})", new object[]
			{
				this.c0.x.ToString(format, formatProvider),
				this.c1.x.ToString(format, formatProvider),
				this.c2.x.ToString(format, formatProvider),
				this.c0.y.ToString(format, formatProvider),
				this.c1.y.ToString(format, formatProvider),
				this.c2.y.ToString(format, formatProvider),
				this.c0.z.ToString(format, formatProvider),
				this.c1.z.ToString(format, formatProvider),
				this.c2.z.ToString(format, formatProvider),
				this.c0.w.ToString(format, formatProvider),
				this.c1.w.ToString(format, formatProvider),
				this.c2.w.ToString(format, formatProvider)
			});
		}

		// Token: 0x040000E6 RID: 230
		public int4 c0;

		// Token: 0x040000E7 RID: 231
		public int4 c1;

		// Token: 0x040000E8 RID: 232
		public int4 c2;

		// Token: 0x040000E9 RID: 233
		public static readonly int4x3 zero;
	}
}
