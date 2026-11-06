using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200000E RID: 14
	internal struct Int128
	{
		// Token: 0x0600005A RID: 90 RVA: 0x00003108 File Offset: 0x00001308
		public Int128(long _lo)
		{
			this.lo = (ulong)_lo;
			if (_lo < 0L)
			{
				this.hi = -1L;
				return;
			}
			this.hi = 0L;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003127 File Offset: 0x00001327
		public Int128(long _hi, ulong _lo)
		{
			this.lo = _lo;
			this.hi = _hi;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00003137 File Offset: 0x00001337
		public Int128(Int128 val)
		{
			this.hi = val.hi;
			this.lo = val.lo;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00003151 File Offset: 0x00001351
		public bool IsNegative()
		{
			return this.hi < 0L;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00003160 File Offset: 0x00001360
		public static bool operator ==(Int128 val1, Int128 val2)
		{
			return val1 == val2 || (val1 != null && val2 != null && val1.hi == val2.hi && val1.lo == val2.lo);
		}

		// Token: 0x0600005F RID: 95 RVA: 0x000031AD File Offset: 0x000013AD
		public static bool operator !=(Int128 val1, Int128 val2)
		{
			return !(val1 == val2);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000031BC File Offset: 0x000013BC
		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is Int128))
			{
				return false;
			}
			Int128 @int = (Int128)obj;
			return @int.hi == this.hi && @int.lo == this.lo;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000031FB File Offset: 0x000013FB
		public override int GetHashCode()
		{
			return this.hi.GetHashCode() ^ this.lo.GetHashCode();
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00003214 File Offset: 0x00001414
		public static bool operator >(Int128 val1, Int128 val2)
		{
			if (val1.hi != val2.hi)
			{
				return val1.hi > val2.hi;
			}
			return val1.lo > val2.lo;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00003241 File Offset: 0x00001441
		public static bool operator <(Int128 val1, Int128 val2)
		{
			if (val1.hi != val2.hi)
			{
				return val1.hi < val2.hi;
			}
			return val1.lo < val2.lo;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x0000326E File Offset: 0x0000146E
		public static Int128 operator +(Int128 lhs, Int128 rhs)
		{
			lhs.hi += rhs.hi;
			lhs.lo += rhs.lo;
			if (lhs.lo < rhs.lo)
			{
				lhs.hi += 1L;
			}
			return lhs;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000032AE File Offset: 0x000014AE
		public static Int128 operator -(Int128 lhs, Int128 rhs)
		{
			return lhs + -rhs;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000032BC File Offset: 0x000014BC
		public static Int128 operator -(Int128 val)
		{
			if (val.lo == 0UL)
			{
				return new Int128(-val.hi, 0UL);
			}
			return new Int128(~val.hi, ~val.lo + 1UL);
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000032EC File Offset: 0x000014EC
		public static explicit operator double(Int128 val)
		{
			if (val.hi >= 0L)
			{
				return val.lo + (double)val.hi * 1.8446744073709552E+19;
			}
			if (val.lo == 0UL)
			{
				return (double)val.hi * 1.8446744073709552E+19;
			}
			return -(~val.lo + (double)(~(double)val.hi) * 1.8446744073709552E+19);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003358 File Offset: 0x00001558
		public static Int128 Int128Mul(long lhs, long rhs)
		{
			bool flag = lhs < 0L != rhs < 0L;
			if (lhs < 0L)
			{
				lhs = -lhs;
			}
			if (rhs < 0L)
			{
				rhs = -rhs;
			}
			ulong num = (ulong)lhs >> 32;
			ulong num2 = (ulong)(lhs & (long)((ulong)(-1)));
			ulong num3 = (ulong)rhs >> 32;
			ulong num4 = (ulong)(rhs & (long)((ulong)(-1)));
			ulong num5 = num * num3;
			ulong num6 = num2 * num4;
			ulong num7 = num * num4 + num2 * num3;
			long num8 = (long)(num5 + (num7 >> 32));
			ulong num9 = (num7 << 32) + num6;
			if (num9 < num6)
			{
				num8 += 1L;
			}
			Int128 @int = new Int128(num8, num9);
			if (!flag)
			{
				return @int;
			}
			return -@int;
		}

		// Token: 0x0400002D RID: 45
		private long hi;

		// Token: 0x0400002E RID: 46
		private ulong lo;
	}
}
