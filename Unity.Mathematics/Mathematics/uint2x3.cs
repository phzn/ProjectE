using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x02000043 RID: 67
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct uint2x3 : IEquatable<uint2x3>, IFormattable
	{
		// Token: 0x06001F97 RID: 8087 RVA: 0x0005BAB1 File Offset: 0x00059CB1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2x3(uint2 c0, uint2 c1, uint2 c2)
		{
			this.c0 = c0;
			this.c1 = c1;
			this.c2 = c2;
		}

		// Token: 0x06001F98 RID: 8088 RVA: 0x0005BAC8 File Offset: 0x00059CC8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2x3(uint m00, uint m01, uint m02, uint m10, uint m11, uint m12)
		{
			this.c0 = new uint2(m00, m10);
			this.c1 = new uint2(m01, m11);
			this.c2 = new uint2(m02, m12);
		}

		// Token: 0x06001F99 RID: 8089 RVA: 0x0005BAF4 File Offset: 0x00059CF4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2x3(uint v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
		}

		// Token: 0x06001F9A RID: 8090 RVA: 0x0005BB1C File Offset: 0x00059D1C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2x3(bool v)
		{
			this.c0 = math.select(new uint2(0U), new uint2(1U), v);
			this.c1 = math.select(new uint2(0U), new uint2(1U), v);
			this.c2 = math.select(new uint2(0U), new uint2(1U), v);
		}

		// Token: 0x06001F9B RID: 8091 RVA: 0x0005BB74 File Offset: 0x00059D74
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2x3(bool2x3 v)
		{
			this.c0 = math.select(new uint2(0U), new uint2(1U), v.c0);
			this.c1 = math.select(new uint2(0U), new uint2(1U), v.c1);
			this.c2 = math.select(new uint2(0U), new uint2(1U), v.c2);
		}

		// Token: 0x06001F9C RID: 8092 RVA: 0x0005BBD8 File Offset: 0x00059DD8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2x3(int v)
		{
			this.c0 = (uint2)v;
			this.c1 = (uint2)v;
			this.c2 = (uint2)v;
		}

		// Token: 0x06001F9D RID: 8093 RVA: 0x0005BBFE File Offset: 0x00059DFE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2x3(int2x3 v)
		{
			this.c0 = (uint2)v.c0;
			this.c1 = (uint2)v.c1;
			this.c2 = (uint2)v.c2;
		}

		// Token: 0x06001F9E RID: 8094 RVA: 0x0005BC33 File Offset: 0x00059E33
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2x3(float v)
		{
			this.c0 = (uint2)v;
			this.c1 = (uint2)v;
			this.c2 = (uint2)v;
		}

		// Token: 0x06001F9F RID: 8095 RVA: 0x0005BC59 File Offset: 0x00059E59
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2x3(float2x3 v)
		{
			this.c0 = (uint2)v.c0;
			this.c1 = (uint2)v.c1;
			this.c2 = (uint2)v.c2;
		}

		// Token: 0x06001FA0 RID: 8096 RVA: 0x0005BC8E File Offset: 0x00059E8E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2x3(double v)
		{
			this.c0 = (uint2)v;
			this.c1 = (uint2)v;
			this.c2 = (uint2)v;
		}

		// Token: 0x06001FA1 RID: 8097 RVA: 0x0005BCB4 File Offset: 0x00059EB4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2x3(double2x3 v)
		{
			this.c0 = (uint2)v.c0;
			this.c1 = (uint2)v.c1;
			this.c2 = (uint2)v.c2;
		}

		// Token: 0x06001FA2 RID: 8098 RVA: 0x0005BCE9 File Offset: 0x00059EE9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator uint2x3(uint v)
		{
			return new uint2x3(v);
		}

		// Token: 0x06001FA3 RID: 8099 RVA: 0x0005BCF1 File Offset: 0x00059EF1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint2x3(bool v)
		{
			return new uint2x3(v);
		}

		// Token: 0x06001FA4 RID: 8100 RVA: 0x0005BCF9 File Offset: 0x00059EF9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint2x3(bool2x3 v)
		{
			return new uint2x3(v);
		}

		// Token: 0x06001FA5 RID: 8101 RVA: 0x0005BD01 File Offset: 0x00059F01
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint2x3(int v)
		{
			return new uint2x3(v);
		}

		// Token: 0x06001FA6 RID: 8102 RVA: 0x0005BD09 File Offset: 0x00059F09
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint2x3(int2x3 v)
		{
			return new uint2x3(v);
		}

		// Token: 0x06001FA7 RID: 8103 RVA: 0x0005BD11 File Offset: 0x00059F11
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint2x3(float v)
		{
			return new uint2x3(v);
		}

		// Token: 0x06001FA8 RID: 8104 RVA: 0x0005BD19 File Offset: 0x00059F19
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint2x3(float2x3 v)
		{
			return new uint2x3(v);
		}

		// Token: 0x06001FA9 RID: 8105 RVA: 0x0005BD21 File Offset: 0x00059F21
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint2x3(double v)
		{
			return new uint2x3(v);
		}

		// Token: 0x06001FAA RID: 8106 RVA: 0x0005BD29 File Offset: 0x00059F29
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint2x3(double2x3 v)
		{
			return new uint2x3(v);
		}

		// Token: 0x06001FAB RID: 8107 RVA: 0x0005BD31 File Offset: 0x00059F31
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x3 operator *(uint2x3 lhs, uint2x3 rhs)
		{
			return new uint2x3(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1, lhs.c2 * rhs.c2);
		}

		// Token: 0x06001FAC RID: 8108 RVA: 0x0005BD6B File Offset: 0x00059F6B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x3 operator *(uint2x3 lhs, uint rhs)
		{
			return new uint2x3(lhs.c0 * rhs, lhs.c1 * rhs, lhs.c2 * rhs);
		}

		// Token: 0x06001FAD RID: 8109 RVA: 0x0005BD96 File Offset: 0x00059F96
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x3 operator *(uint lhs, uint2x3 rhs)
		{
			return new uint2x3(lhs * rhs.c0, lhs * rhs.c1, lhs * rhs.c2);
		}

		// Token: 0x06001FAE RID: 8110 RVA: 0x0005BDC1 File Offset: 0x00059FC1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x3 operator +(uint2x3 lhs, uint2x3 rhs)
		{
			return new uint2x3(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1, lhs.c2 + rhs.c2);
		}

		// Token: 0x06001FAF RID: 8111 RVA: 0x0005BDFB File Offset: 0x00059FFB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x3 operator +(uint2x3 lhs, uint rhs)
		{
			return new uint2x3(lhs.c0 + rhs, lhs.c1 + rhs, lhs.c2 + rhs);
		}

		// Token: 0x06001FB0 RID: 8112 RVA: 0x0005BE26 File Offset: 0x0005A026
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x3 operator +(uint lhs, uint2x3 rhs)
		{
			return new uint2x3(lhs + rhs.c0, lhs + rhs.c1, lhs + rhs.c2);
		}

		// Token: 0x06001FB1 RID: 8113 RVA: 0x0005BE51 File Offset: 0x0005A051
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x3 operator -(uint2x3 lhs, uint2x3 rhs)
		{
			return new uint2x3(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1, lhs.c2 - rhs.c2);
		}

		// Token: 0x06001FB2 RID: 8114 RVA: 0x0005BE8B File Offset: 0x0005A08B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x3 operator -(uint2x3 lhs, uint rhs)
		{
			return new uint2x3(lhs.c0 - rhs, lhs.c1 - rhs, lhs.c2 - rhs);
		}

		// Token: 0x06001FB3 RID: 8115 RVA: 0x0005BEB6 File Offset: 0x0005A0B6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x3 operator -(uint lhs, uint2x3 rhs)
		{
			return new uint2x3(lhs - rhs.c0, lhs - rhs.c1, lhs - rhs.c2);
		}

		// Token: 0x06001FB4 RID: 8116 RVA: 0x0005BEE1 File Offset: 0x0005A0E1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x3 operator /(uint2x3 lhs, uint2x3 rhs)
		{
			return new uint2x3(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1, lhs.c2 / rhs.c2);
		}

		// Token: 0x06001FB5 RID: 8117 RVA: 0x0005BF1B File Offset: 0x0005A11B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x3 operator /(uint2x3 lhs, uint rhs)
		{
			return new uint2x3(lhs.c0 / rhs, lhs.c1 / rhs, lhs.c2 / rhs);
		}

		// Token: 0x06001FB6 RID: 8118 RVA: 0x0005BF46 File Offset: 0x0005A146
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x3 operator /(uint lhs, uint2x3 rhs)
		{
			return new uint2x3(lhs / rhs.c0, lhs / rhs.c1, lhs / rhs.c2);
		}

		// Token: 0x06001FB7 RID: 8119 RVA: 0x0005BF71 File Offset: 0x0005A171
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x3 operator %(uint2x3 lhs, uint2x3 rhs)
		{
			return new uint2x3(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1, lhs.c2 % rhs.c2);
		}

		// Token: 0x06001FB8 RID: 8120 RVA: 0x0005BFAB File Offset: 0x0005A1AB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x3 operator %(uint2x3 lhs, uint rhs)
		{
			return new uint2x3(lhs.c0 % rhs, lhs.c1 % rhs, lhs.c2 % rhs);
		}

		// Token: 0x06001FB9 RID: 8121 RVA: 0x0005BFD6 File Offset: 0x0005A1D6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x3 operator %(uint lhs, uint2x3 rhs)
		{
			return new uint2x3(lhs % rhs.c0, lhs % rhs.c1, lhs % rhs.c2);
		}

		// Token: 0x06001FBA RID: 8122 RVA: 0x0005C004 File Offset: 0x0005A204
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x3 operator ++(uint2x3 val)
		{
			uint2 @uint = uint2.op_Increment(val.c0);
			val.c0 = @uint;
			uint2 uint2 = @uint;
			@uint = uint2.op_Increment(val.c1);
			val.c1 = @uint;
			uint2 uint3 = @uint;
			@uint = uint2.op_Increment(val.c2);
			val.c2 = @uint;
			return new uint2x3(uint2, uint3, @uint);
		}

		// Token: 0x06001FBB RID: 8123 RVA: 0x0005C064 File Offset: 0x0005A264
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x3 operator --(uint2x3 val)
		{
			uint2 @uint = uint2.op_Decrement(val.c0);
			val.c0 = @uint;
			uint2 uint2 = @uint;
			@uint = uint2.op_Decrement(val.c1);
			val.c1 = @uint;
			uint2 uint3 = @uint;
			@uint = uint2.op_Decrement(val.c2);
			val.c2 = @uint;
			return new uint2x3(uint2, uint3, @uint);
		}

		// Token: 0x06001FBC RID: 8124 RVA: 0x0005C0C4 File Offset: 0x0005A2C4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator <(uint2x3 lhs, uint2x3 rhs)
		{
			return new bool2x3(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1, lhs.c2 < rhs.c2);
		}

		// Token: 0x06001FBD RID: 8125 RVA: 0x0005C0FE File Offset: 0x0005A2FE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator <(uint2x3 lhs, uint rhs)
		{
			return new bool2x3(lhs.c0 < rhs, lhs.c1 < rhs, lhs.c2 < rhs);
		}

		// Token: 0x06001FBE RID: 8126 RVA: 0x0005C129 File Offset: 0x0005A329
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator <(uint lhs, uint2x3 rhs)
		{
			return new bool2x3(lhs < rhs.c0, lhs < rhs.c1, lhs < rhs.c2);
		}

		// Token: 0x06001FBF RID: 8127 RVA: 0x0005C154 File Offset: 0x0005A354
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator <=(uint2x3 lhs, uint2x3 rhs)
		{
			return new bool2x3(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1, lhs.c2 <= rhs.c2);
		}

		// Token: 0x06001FC0 RID: 8128 RVA: 0x0005C18E File Offset: 0x0005A38E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator <=(uint2x3 lhs, uint rhs)
		{
			return new bool2x3(lhs.c0 <= rhs, lhs.c1 <= rhs, lhs.c2 <= rhs);
		}

		// Token: 0x06001FC1 RID: 8129 RVA: 0x0005C1B9 File Offset: 0x0005A3B9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator <=(uint lhs, uint2x3 rhs)
		{
			return new bool2x3(lhs <= rhs.c0, lhs <= rhs.c1, lhs <= rhs.c2);
		}

		// Token: 0x06001FC2 RID: 8130 RVA: 0x0005C1E4 File Offset: 0x0005A3E4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator >(uint2x3 lhs, uint2x3 rhs)
		{
			return new bool2x3(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1, lhs.c2 > rhs.c2);
		}

		// Token: 0x06001FC3 RID: 8131 RVA: 0x0005C21E File Offset: 0x0005A41E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator >(uint2x3 lhs, uint rhs)
		{
			return new bool2x3(lhs.c0 > rhs, lhs.c1 > rhs, lhs.c2 > rhs);
		}

		// Token: 0x06001FC4 RID: 8132 RVA: 0x0005C249 File Offset: 0x0005A449
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator >(uint lhs, uint2x3 rhs)
		{
			return new bool2x3(lhs > rhs.c0, lhs > rhs.c1, lhs > rhs.c2);
		}

		// Token: 0x06001FC5 RID: 8133 RVA: 0x0005C274 File Offset: 0x0005A474
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator >=(uint2x3 lhs, uint2x3 rhs)
		{
			return new bool2x3(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1, lhs.c2 >= rhs.c2);
		}

		// Token: 0x06001FC6 RID: 8134 RVA: 0x0005C2AE File Offset: 0x0005A4AE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator >=(uint2x3 lhs, uint rhs)
		{
			return new bool2x3(lhs.c0 >= rhs, lhs.c1 >= rhs, lhs.c2 >= rhs);
		}

		// Token: 0x06001FC7 RID: 8135 RVA: 0x0005C2D9 File Offset: 0x0005A4D9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator >=(uint lhs, uint2x3 rhs)
		{
			return new bool2x3(lhs >= rhs.c0, lhs >= rhs.c1, lhs >= rhs.c2);
		}

		// Token: 0x06001FC8 RID: 8136 RVA: 0x0005C304 File Offset: 0x0005A504
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x3 operator -(uint2x3 val)
		{
			return new uint2x3(-val.c0, -val.c1, -val.c2);
		}

		// Token: 0x06001FC9 RID: 8137 RVA: 0x0005C32C File Offset: 0x0005A52C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x3 operator +(uint2x3 val)
		{
			return new uint2x3(+val.c0, +val.c1, +val.c2);
		}

		// Token: 0x06001FCA RID: 8138 RVA: 0x0005C354 File Offset: 0x0005A554
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x3 operator <<(uint2x3 x, int n)
		{
			return new uint2x3(x.c0 << n, x.c1 << n, x.c2 << n);
		}

		// Token: 0x06001FCB RID: 8139 RVA: 0x0005C37F File Offset: 0x0005A57F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x3 operator >>(uint2x3 x, int n)
		{
			return new uint2x3(x.c0 >> n, x.c1 >> n, x.c2 >> n);
		}

		// Token: 0x06001FCC RID: 8140 RVA: 0x0005C3AA File Offset: 0x0005A5AA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator ==(uint2x3 lhs, uint2x3 rhs)
		{
			return new bool2x3(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2);
		}

		// Token: 0x06001FCD RID: 8141 RVA: 0x0005C3E4 File Offset: 0x0005A5E4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator ==(uint2x3 lhs, uint rhs)
		{
			return new bool2x3(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs);
		}

		// Token: 0x06001FCE RID: 8142 RVA: 0x0005C40F File Offset: 0x0005A60F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator ==(uint lhs, uint2x3 rhs)
		{
			return new bool2x3(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2);
		}

		// Token: 0x06001FCF RID: 8143 RVA: 0x0005C43A File Offset: 0x0005A63A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator !=(uint2x3 lhs, uint2x3 rhs)
		{
			return new bool2x3(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2);
		}

		// Token: 0x06001FD0 RID: 8144 RVA: 0x0005C474 File Offset: 0x0005A674
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator !=(uint2x3 lhs, uint rhs)
		{
			return new bool2x3(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs);
		}

		// Token: 0x06001FD1 RID: 8145 RVA: 0x0005C49F File Offset: 0x0005A69F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x3 operator !=(uint lhs, uint2x3 rhs)
		{
			return new bool2x3(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2);
		}

		// Token: 0x06001FD2 RID: 8146 RVA: 0x0005C4CA File Offset: 0x0005A6CA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x3 operator ~(uint2x3 val)
		{
			return new uint2x3(~val.c0, ~val.c1, ~val.c2);
		}

		// Token: 0x06001FD3 RID: 8147 RVA: 0x0005C4F2 File Offset: 0x0005A6F2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x3 operator &(uint2x3 lhs, uint2x3 rhs)
		{
			return new uint2x3(lhs.c0 & rhs.c0, lhs.c1 & rhs.c1, lhs.c2 & rhs.c2);
		}

		// Token: 0x06001FD4 RID: 8148 RVA: 0x0005C52C File Offset: 0x0005A72C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x3 operator &(uint2x3 lhs, uint rhs)
		{
			return new uint2x3(lhs.c0 & rhs, lhs.c1 & rhs, lhs.c2 & rhs);
		}

		// Token: 0x06001FD5 RID: 8149 RVA: 0x0005C557 File Offset: 0x0005A757
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x3 operator &(uint lhs, uint2x3 rhs)
		{
			return new uint2x3(lhs & rhs.c0, lhs & rhs.c1, lhs & rhs.c2);
		}

		// Token: 0x06001FD6 RID: 8150 RVA: 0x0005C582 File Offset: 0x0005A782
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x3 operator |(uint2x3 lhs, uint2x3 rhs)
		{
			return new uint2x3(lhs.c0 | rhs.c0, lhs.c1 | rhs.c1, lhs.c2 | rhs.c2);
		}

		// Token: 0x06001FD7 RID: 8151 RVA: 0x0005C5BC File Offset: 0x0005A7BC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x3 operator |(uint2x3 lhs, uint rhs)
		{
			return new uint2x3(lhs.c0 | rhs, lhs.c1 | rhs, lhs.c2 | rhs);
		}

		// Token: 0x06001FD8 RID: 8152 RVA: 0x0005C5E7 File Offset: 0x0005A7E7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x3 operator |(uint lhs, uint2x3 rhs)
		{
			return new uint2x3(lhs | rhs.c0, lhs | rhs.c1, lhs | rhs.c2);
		}

		// Token: 0x06001FD9 RID: 8153 RVA: 0x0005C612 File Offset: 0x0005A812
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x3 operator ^(uint2x3 lhs, uint2x3 rhs)
		{
			return new uint2x3(lhs.c0 ^ rhs.c0, lhs.c1 ^ rhs.c1, lhs.c2 ^ rhs.c2);
		}

		// Token: 0x06001FDA RID: 8154 RVA: 0x0005C64C File Offset: 0x0005A84C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x3 operator ^(uint2x3 lhs, uint rhs)
		{
			return new uint2x3(lhs.c0 ^ rhs, lhs.c1 ^ rhs, lhs.c2 ^ rhs);
		}

		// Token: 0x06001FDB RID: 8155 RVA: 0x0005C677 File Offset: 0x0005A877
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x3 operator ^(uint lhs, uint2x3 rhs)
		{
			return new uint2x3(lhs ^ rhs.c0, lhs ^ rhs.c1, lhs ^ rhs.c2);
		}

		// Token: 0x170009BB RID: 2491
		public unsafe ref uint2 this[int index]
		{
			get
			{
				fixed (uint2x3* ptr = &this)
				{
					return ref *(uint2*)(ptr + (IntPtr)index * (IntPtr)sizeof(uint2) / (IntPtr)sizeof(uint2x3));
				}
			}
		}

		// Token: 0x06001FDD RID: 8157 RVA: 0x0005C6BF File Offset: 0x0005A8BF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(uint2x3 rhs)
		{
			return this.c0.Equals(rhs.c0) && this.c1.Equals(rhs.c1) && this.c2.Equals(rhs.c2);
		}

		// Token: 0x06001FDE RID: 8158 RVA: 0x0005C6FC File Offset: 0x0005A8FC
		public override bool Equals(object o)
		{
			if (o is uint2x3)
			{
				uint2x3 uint2x = (uint2x3)o;
				return this.Equals(uint2x);
			}
			return false;
		}

		// Token: 0x06001FDF RID: 8159 RVA: 0x0005C721 File Offset: 0x0005A921
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x06001FE0 RID: 8160 RVA: 0x0005C730 File Offset: 0x0005A930
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("uint2x3({0}, {1}, {2},  {3}, {4}, {5})", new object[]
			{
				this.c0.x,
				this.c1.x,
				this.c2.x,
				this.c0.y,
				this.c1.y,
				this.c2.y
			});
		}

		// Token: 0x06001FE1 RID: 8161 RVA: 0x0005C7C0 File Offset: 0x0005A9C0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("uint2x3({0}, {1}, {2},  {3}, {4}, {5})", new object[]
			{
				this.c0.x.ToString(format, formatProvider),
				this.c1.x.ToString(format, formatProvider),
				this.c2.x.ToString(format, formatProvider),
				this.c0.y.ToString(format, formatProvider),
				this.c1.y.ToString(format, formatProvider),
				this.c2.y.ToString(format, formatProvider)
			});
		}

		// Token: 0x04000101 RID: 257
		public uint2 c0;

		// Token: 0x04000102 RID: 258
		public uint2 c1;

		// Token: 0x04000103 RID: 259
		public uint2 c2;

		// Token: 0x04000104 RID: 260
		public static readonly uint2x3 zero;
	}
}
