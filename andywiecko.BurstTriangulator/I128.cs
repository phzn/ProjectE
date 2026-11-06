using System;
using Unity.Mathematics;

namespace andywiecko.BurstTriangulator
{
	// Token: 0x02000014 RID: 20
	internal struct I128
	{
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000065 RID: 101 RVA: 0x00002A7E File Offset: 0x00000C7E
		public bool IsNegative
		{
			get
			{
				return (this.hi & 9223372036854775808UL) > 0UL;
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00002A94 File Offset: 0x00000C94
		public I128(ulong hi, ulong lo)
		{
			this.hi = hi;
			this.lo = lo;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00002AB4 File Offset: 0x00000CB4
		public static I128 operator +(I128 a, I128 b)
		{
			ulong num = a.lo + b.lo;
			return new I128(a.hi + b.hi + ((num < a.lo) ? 1UL : 0UL), num);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00002AF4 File Offset: 0x00000CF4
		public static I128 operator -(I128 a, I128 b)
		{
			ulong num = a.lo - b.lo;
			return new I128(a.hi - b.hi - ((num > a.lo) ? 1UL : 0UL), num);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00002B32 File Offset: 0x00000D32
		public static I128 operator -(I128 a)
		{
			return new I128(~a.hi, ~a.lo) + new I128(0UL, 1UL);
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00002B58 File Offset: 0x00000D58
		public static I128 Multiply(long slhs, long srhs)
		{
			bool flag = (slhs < 0L) ^ (srhs < 0L);
			long num = math.abs(slhs);
			ulong num2 = (ulong)math.abs(srhs);
			ulong num3 = (ulong)((num & (long)((ulong)(-1))) * (long)(num2 & (ulong)(-1)));
			ulong num4 = ((ulong)num >> 32) * (num2 & (ulong)(-1));
			ulong num5 = (ulong)((num & (long)((ulong)(-1))) * (long)(num2 >> 32));
			ulong num6 = ((ulong)num >> 32) * (num2 >> 32);
			ulong num7 = (num3 >> 32) + (num4 & (ulong)(-1)) + num5;
			ulong num8 = (num4 >> 32) + (num7 >> 32) + num6;
			I128 i = new I128(num8, (num7 << 32) | (num3 & (ulong)(-1)));
			i = (flag ? (-i) : i);
			return i;
		}

		// Token: 0x04000048 RID: 72
		private ulong hi;

		// Token: 0x04000049 RID: 73
		private ulong lo;
	}
}
