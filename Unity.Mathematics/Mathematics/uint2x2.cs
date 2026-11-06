using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x02000042 RID: 66
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct uint2x2 : IEquatable<uint2x2>, IFormattable
	{
		// Token: 0x06001F4B RID: 8011 RVA: 0x0005B0CE File Offset: 0x000592CE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2x2(uint2 c0, uint2 c1)
		{
			this.c0 = c0;
			this.c1 = c1;
		}

		// Token: 0x06001F4C RID: 8012 RVA: 0x0005B0DE File Offset: 0x000592DE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2x2(uint m00, uint m01, uint m10, uint m11)
		{
			this.c0 = new uint2(m00, m10);
			this.c1 = new uint2(m01, m11);
		}

		// Token: 0x06001F4D RID: 8013 RVA: 0x0005B0FB File Offset: 0x000592FB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2x2(uint v)
		{
			this.c0 = v;
			this.c1 = v;
		}

		// Token: 0x06001F4E RID: 8014 RVA: 0x0005B115 File Offset: 0x00059315
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2x2(bool v)
		{
			this.c0 = math.select(new uint2(0U), new uint2(1U), v);
			this.c1 = math.select(new uint2(0U), new uint2(1U), v);
		}

		// Token: 0x06001F4F RID: 8015 RVA: 0x0005B147 File Offset: 0x00059347
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2x2(bool2x2 v)
		{
			this.c0 = math.select(new uint2(0U), new uint2(1U), v.c0);
			this.c1 = math.select(new uint2(0U), new uint2(1U), v.c1);
		}

		// Token: 0x06001F50 RID: 8016 RVA: 0x0005B183 File Offset: 0x00059383
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2x2(int v)
		{
			this.c0 = (uint2)v;
			this.c1 = (uint2)v;
		}

		// Token: 0x06001F51 RID: 8017 RVA: 0x0005B19D File Offset: 0x0005939D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2x2(int2x2 v)
		{
			this.c0 = (uint2)v.c0;
			this.c1 = (uint2)v.c1;
		}

		// Token: 0x06001F52 RID: 8018 RVA: 0x0005B1C1 File Offset: 0x000593C1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2x2(float v)
		{
			this.c0 = (uint2)v;
			this.c1 = (uint2)v;
		}

		// Token: 0x06001F53 RID: 8019 RVA: 0x0005B1DB File Offset: 0x000593DB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2x2(float2x2 v)
		{
			this.c0 = (uint2)v.c0;
			this.c1 = (uint2)v.c1;
		}

		// Token: 0x06001F54 RID: 8020 RVA: 0x0005B1FF File Offset: 0x000593FF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2x2(double v)
		{
			this.c0 = (uint2)v;
			this.c1 = (uint2)v;
		}

		// Token: 0x06001F55 RID: 8021 RVA: 0x0005B219 File Offset: 0x00059419
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2x2(double2x2 v)
		{
			this.c0 = (uint2)v.c0;
			this.c1 = (uint2)v.c1;
		}

		// Token: 0x06001F56 RID: 8022 RVA: 0x0005B23D File Offset: 0x0005943D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator uint2x2(uint v)
		{
			return new uint2x2(v);
		}

		// Token: 0x06001F57 RID: 8023 RVA: 0x0005B245 File Offset: 0x00059445
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint2x2(bool v)
		{
			return new uint2x2(v);
		}

		// Token: 0x06001F58 RID: 8024 RVA: 0x0005B24D File Offset: 0x0005944D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint2x2(bool2x2 v)
		{
			return new uint2x2(v);
		}

		// Token: 0x06001F59 RID: 8025 RVA: 0x0005B255 File Offset: 0x00059455
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint2x2(int v)
		{
			return new uint2x2(v);
		}

		// Token: 0x06001F5A RID: 8026 RVA: 0x0005B25D File Offset: 0x0005945D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint2x2(int2x2 v)
		{
			return new uint2x2(v);
		}

		// Token: 0x06001F5B RID: 8027 RVA: 0x0005B265 File Offset: 0x00059465
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint2x2(float v)
		{
			return new uint2x2(v);
		}

		// Token: 0x06001F5C RID: 8028 RVA: 0x0005B26D File Offset: 0x0005946D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint2x2(float2x2 v)
		{
			return new uint2x2(v);
		}

		// Token: 0x06001F5D RID: 8029 RVA: 0x0005B275 File Offset: 0x00059475
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint2x2(double v)
		{
			return new uint2x2(v);
		}

		// Token: 0x06001F5E RID: 8030 RVA: 0x0005B27D File Offset: 0x0005947D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint2x2(double2x2 v)
		{
			return new uint2x2(v);
		}

		// Token: 0x06001F5F RID: 8031 RVA: 0x0005B285 File Offset: 0x00059485
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x2 operator *(uint2x2 lhs, uint2x2 rhs)
		{
			return new uint2x2(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1);
		}

		// Token: 0x06001F60 RID: 8032 RVA: 0x0005B2AE File Offset: 0x000594AE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x2 operator *(uint2x2 lhs, uint rhs)
		{
			return new uint2x2(lhs.c0 * rhs, lhs.c1 * rhs);
		}

		// Token: 0x06001F61 RID: 8033 RVA: 0x0005B2CD File Offset: 0x000594CD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x2 operator *(uint lhs, uint2x2 rhs)
		{
			return new uint2x2(lhs * rhs.c0, lhs * rhs.c1);
		}

		// Token: 0x06001F62 RID: 8034 RVA: 0x0005B2EC File Offset: 0x000594EC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x2 operator +(uint2x2 lhs, uint2x2 rhs)
		{
			return new uint2x2(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1);
		}

		// Token: 0x06001F63 RID: 8035 RVA: 0x0005B315 File Offset: 0x00059515
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x2 operator +(uint2x2 lhs, uint rhs)
		{
			return new uint2x2(lhs.c0 + rhs, lhs.c1 + rhs);
		}

		// Token: 0x06001F64 RID: 8036 RVA: 0x0005B334 File Offset: 0x00059534
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x2 operator +(uint lhs, uint2x2 rhs)
		{
			return new uint2x2(lhs + rhs.c0, lhs + rhs.c1);
		}

		// Token: 0x06001F65 RID: 8037 RVA: 0x0005B353 File Offset: 0x00059553
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x2 operator -(uint2x2 lhs, uint2x2 rhs)
		{
			return new uint2x2(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1);
		}

		// Token: 0x06001F66 RID: 8038 RVA: 0x0005B37C File Offset: 0x0005957C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x2 operator -(uint2x2 lhs, uint rhs)
		{
			return new uint2x2(lhs.c0 - rhs, lhs.c1 - rhs);
		}

		// Token: 0x06001F67 RID: 8039 RVA: 0x0005B39B File Offset: 0x0005959B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x2 operator -(uint lhs, uint2x2 rhs)
		{
			return new uint2x2(lhs - rhs.c0, lhs - rhs.c1);
		}

		// Token: 0x06001F68 RID: 8040 RVA: 0x0005B3BA File Offset: 0x000595BA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x2 operator /(uint2x2 lhs, uint2x2 rhs)
		{
			return new uint2x2(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1);
		}

		// Token: 0x06001F69 RID: 8041 RVA: 0x0005B3E3 File Offset: 0x000595E3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x2 operator /(uint2x2 lhs, uint rhs)
		{
			return new uint2x2(lhs.c0 / rhs, lhs.c1 / rhs);
		}

		// Token: 0x06001F6A RID: 8042 RVA: 0x0005B402 File Offset: 0x00059602
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x2 operator /(uint lhs, uint2x2 rhs)
		{
			return new uint2x2(lhs / rhs.c0, lhs / rhs.c1);
		}

		// Token: 0x06001F6B RID: 8043 RVA: 0x0005B421 File Offset: 0x00059621
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x2 operator %(uint2x2 lhs, uint2x2 rhs)
		{
			return new uint2x2(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1);
		}

		// Token: 0x06001F6C RID: 8044 RVA: 0x0005B44A File Offset: 0x0005964A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x2 operator %(uint2x2 lhs, uint rhs)
		{
			return new uint2x2(lhs.c0 % rhs, lhs.c1 % rhs);
		}

		// Token: 0x06001F6D RID: 8045 RVA: 0x0005B469 File Offset: 0x00059669
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x2 operator %(uint lhs, uint2x2 rhs)
		{
			return new uint2x2(lhs % rhs.c0, lhs % rhs.c1);
		}

		// Token: 0x06001F6E RID: 8046 RVA: 0x0005B488 File Offset: 0x00059688
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x2 operator ++(uint2x2 val)
		{
			uint2 @uint = uint2.op_Increment(val.c0);
			val.c0 = @uint;
			uint2 uint2 = @uint;
			@uint = uint2.op_Increment(val.c1);
			val.c1 = @uint;
			return new uint2x2(uint2, @uint);
		}

		// Token: 0x06001F6F RID: 8047 RVA: 0x0005B4D0 File Offset: 0x000596D0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x2 operator --(uint2x2 val)
		{
			uint2 @uint = uint2.op_Decrement(val.c0);
			val.c0 = @uint;
			uint2 uint2 = @uint;
			@uint = uint2.op_Decrement(val.c1);
			val.c1 = @uint;
			return new uint2x2(uint2, @uint);
		}

		// Token: 0x06001F70 RID: 8048 RVA: 0x0005B516 File Offset: 0x00059716
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator <(uint2x2 lhs, uint2x2 rhs)
		{
			return new bool2x2(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1);
		}

		// Token: 0x06001F71 RID: 8049 RVA: 0x0005B53F File Offset: 0x0005973F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator <(uint2x2 lhs, uint rhs)
		{
			return new bool2x2(lhs.c0 < rhs, lhs.c1 < rhs);
		}

		// Token: 0x06001F72 RID: 8050 RVA: 0x0005B55E File Offset: 0x0005975E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator <(uint lhs, uint2x2 rhs)
		{
			return new bool2x2(lhs < rhs.c0, lhs < rhs.c1);
		}

		// Token: 0x06001F73 RID: 8051 RVA: 0x0005B57D File Offset: 0x0005977D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator <=(uint2x2 lhs, uint2x2 rhs)
		{
			return new bool2x2(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1);
		}

		// Token: 0x06001F74 RID: 8052 RVA: 0x0005B5A6 File Offset: 0x000597A6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator <=(uint2x2 lhs, uint rhs)
		{
			return new bool2x2(lhs.c0 <= rhs, lhs.c1 <= rhs);
		}

		// Token: 0x06001F75 RID: 8053 RVA: 0x0005B5C5 File Offset: 0x000597C5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator <=(uint lhs, uint2x2 rhs)
		{
			return new bool2x2(lhs <= rhs.c0, lhs <= rhs.c1);
		}

		// Token: 0x06001F76 RID: 8054 RVA: 0x0005B5E4 File Offset: 0x000597E4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator >(uint2x2 lhs, uint2x2 rhs)
		{
			return new bool2x2(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1);
		}

		// Token: 0x06001F77 RID: 8055 RVA: 0x0005B60D File Offset: 0x0005980D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator >(uint2x2 lhs, uint rhs)
		{
			return new bool2x2(lhs.c0 > rhs, lhs.c1 > rhs);
		}

		// Token: 0x06001F78 RID: 8056 RVA: 0x0005B62C File Offset: 0x0005982C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator >(uint lhs, uint2x2 rhs)
		{
			return new bool2x2(lhs > rhs.c0, lhs > rhs.c1);
		}

		// Token: 0x06001F79 RID: 8057 RVA: 0x0005B64B File Offset: 0x0005984B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator >=(uint2x2 lhs, uint2x2 rhs)
		{
			return new bool2x2(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1);
		}

		// Token: 0x06001F7A RID: 8058 RVA: 0x0005B674 File Offset: 0x00059874
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator >=(uint2x2 lhs, uint rhs)
		{
			return new bool2x2(lhs.c0 >= rhs, lhs.c1 >= rhs);
		}

		// Token: 0x06001F7B RID: 8059 RVA: 0x0005B693 File Offset: 0x00059893
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator >=(uint lhs, uint2x2 rhs)
		{
			return new bool2x2(lhs >= rhs.c0, lhs >= rhs.c1);
		}

		// Token: 0x06001F7C RID: 8060 RVA: 0x0005B6B2 File Offset: 0x000598B2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x2 operator -(uint2x2 val)
		{
			return new uint2x2(-val.c0, -val.c1);
		}

		// Token: 0x06001F7D RID: 8061 RVA: 0x0005B6CF File Offset: 0x000598CF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x2 operator +(uint2x2 val)
		{
			return new uint2x2(+val.c0, +val.c1);
		}

		// Token: 0x06001F7E RID: 8062 RVA: 0x0005B6EC File Offset: 0x000598EC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x2 operator <<(uint2x2 x, int n)
		{
			return new uint2x2(x.c0 << n, x.c1 << n);
		}

		// Token: 0x06001F7F RID: 8063 RVA: 0x0005B70B File Offset: 0x0005990B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x2 operator >>(uint2x2 x, int n)
		{
			return new uint2x2(x.c0 >> n, x.c1 >> n);
		}

		// Token: 0x06001F80 RID: 8064 RVA: 0x0005B72A File Offset: 0x0005992A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator ==(uint2x2 lhs, uint2x2 rhs)
		{
			return new bool2x2(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1);
		}

		// Token: 0x06001F81 RID: 8065 RVA: 0x0005B753 File Offset: 0x00059953
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator ==(uint2x2 lhs, uint rhs)
		{
			return new bool2x2(lhs.c0 == rhs, lhs.c1 == rhs);
		}

		// Token: 0x06001F82 RID: 8066 RVA: 0x0005B772 File Offset: 0x00059972
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator ==(uint lhs, uint2x2 rhs)
		{
			return new bool2x2(lhs == rhs.c0, lhs == rhs.c1);
		}

		// Token: 0x06001F83 RID: 8067 RVA: 0x0005B791 File Offset: 0x00059991
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator !=(uint2x2 lhs, uint2x2 rhs)
		{
			return new bool2x2(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1);
		}

		// Token: 0x06001F84 RID: 8068 RVA: 0x0005B7BA File Offset: 0x000599BA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator !=(uint2x2 lhs, uint rhs)
		{
			return new bool2x2(lhs.c0 != rhs, lhs.c1 != rhs);
		}

		// Token: 0x06001F85 RID: 8069 RVA: 0x0005B7D9 File Offset: 0x000599D9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x2 operator !=(uint lhs, uint2x2 rhs)
		{
			return new bool2x2(lhs != rhs.c0, lhs != rhs.c1);
		}

		// Token: 0x06001F86 RID: 8070 RVA: 0x0005B7F8 File Offset: 0x000599F8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x2 operator ~(uint2x2 val)
		{
			return new uint2x2(~val.c0, ~val.c1);
		}

		// Token: 0x06001F87 RID: 8071 RVA: 0x0005B815 File Offset: 0x00059A15
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x2 operator &(uint2x2 lhs, uint2x2 rhs)
		{
			return new uint2x2(lhs.c0 & rhs.c0, lhs.c1 & rhs.c1);
		}

		// Token: 0x06001F88 RID: 8072 RVA: 0x0005B83E File Offset: 0x00059A3E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x2 operator &(uint2x2 lhs, uint rhs)
		{
			return new uint2x2(lhs.c0 & rhs, lhs.c1 & rhs);
		}

		// Token: 0x06001F89 RID: 8073 RVA: 0x0005B85D File Offset: 0x00059A5D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x2 operator &(uint lhs, uint2x2 rhs)
		{
			return new uint2x2(lhs & rhs.c0, lhs & rhs.c1);
		}

		// Token: 0x06001F8A RID: 8074 RVA: 0x0005B87C File Offset: 0x00059A7C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x2 operator |(uint2x2 lhs, uint2x2 rhs)
		{
			return new uint2x2(lhs.c0 | rhs.c0, lhs.c1 | rhs.c1);
		}

		// Token: 0x06001F8B RID: 8075 RVA: 0x0005B8A5 File Offset: 0x00059AA5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x2 operator |(uint2x2 lhs, uint rhs)
		{
			return new uint2x2(lhs.c0 | rhs, lhs.c1 | rhs);
		}

		// Token: 0x06001F8C RID: 8076 RVA: 0x0005B8C4 File Offset: 0x00059AC4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x2 operator |(uint lhs, uint2x2 rhs)
		{
			return new uint2x2(lhs | rhs.c0, lhs | rhs.c1);
		}

		// Token: 0x06001F8D RID: 8077 RVA: 0x0005B8E3 File Offset: 0x00059AE3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x2 operator ^(uint2x2 lhs, uint2x2 rhs)
		{
			return new uint2x2(lhs.c0 ^ rhs.c0, lhs.c1 ^ rhs.c1);
		}

		// Token: 0x06001F8E RID: 8078 RVA: 0x0005B90C File Offset: 0x00059B0C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x2 operator ^(uint2x2 lhs, uint rhs)
		{
			return new uint2x2(lhs.c0 ^ rhs, lhs.c1 ^ rhs);
		}

		// Token: 0x06001F8F RID: 8079 RVA: 0x0005B92B File Offset: 0x00059B2B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x2 operator ^(uint lhs, uint2x2 rhs)
		{
			return new uint2x2(lhs ^ rhs.c0, lhs ^ rhs.c1);
		}

		// Token: 0x170009BA RID: 2490
		public unsafe ref uint2 this[int index]
		{
			get
			{
				fixed (uint2x2* ptr = &this)
				{
					return ref *(uint2*)(ptr + (IntPtr)index * (IntPtr)sizeof(uint2) / (IntPtr)sizeof(uint2x2));
				}
			}
		}

		// Token: 0x06001F91 RID: 8081 RVA: 0x0005B967 File Offset: 0x00059B67
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(uint2x2 rhs)
		{
			return this.c0.Equals(rhs.c0) && this.c1.Equals(rhs.c1);
		}

		// Token: 0x06001F92 RID: 8082 RVA: 0x0005B990 File Offset: 0x00059B90
		public override bool Equals(object o)
		{
			if (o is uint2x2)
			{
				uint2x2 uint2x = (uint2x2)o;
				return this.Equals(uint2x);
			}
			return false;
		}

		// Token: 0x06001F93 RID: 8083 RVA: 0x0005B9B5 File Offset: 0x00059BB5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x06001F94 RID: 8084 RVA: 0x0005B9C4 File Offset: 0x00059BC4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("uint2x2({0}, {1},  {2}, {3})", new object[]
			{
				this.c0.x,
				this.c1.x,
				this.c0.y,
				this.c1.y
			});
		}

		// Token: 0x06001F95 RID: 8085 RVA: 0x0005BA30 File Offset: 0x00059C30
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("uint2x2({0}, {1},  {2}, {3})", new object[]
			{
				this.c0.x.ToString(format, formatProvider),
				this.c1.x.ToString(format, formatProvider),
				this.c0.y.ToString(format, formatProvider),
				this.c1.y.ToString(format, formatProvider)
			});
		}

		// Token: 0x040000FD RID: 253
		public uint2 c0;

		// Token: 0x040000FE RID: 254
		public uint2 c1;

		// Token: 0x040000FF RID: 255
		public static readonly uint2x2 identity = new uint2x2(1U, 0U, 0U, 1U);

		// Token: 0x04000100 RID: 256
		public static readonly uint2x2 zero;
	}
}
