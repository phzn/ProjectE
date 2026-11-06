using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x0200004A RID: 74
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct uint4x2 : IEquatable<uint4x2>, IFormattable
	{
		// Token: 0x060023BC RID: 9148 RVA: 0x00065D81 File Offset: 0x00063F81
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint4x2(uint4 c0, uint4 c1)
		{
			this.c0 = c0;
			this.c1 = c1;
		}

		// Token: 0x060023BD RID: 9149 RVA: 0x00065D91 File Offset: 0x00063F91
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint4x2(uint m00, uint m01, uint m10, uint m11, uint m20, uint m21, uint m30, uint m31)
		{
			this.c0 = new uint4(m00, m10, m20, m30);
			this.c1 = new uint4(m01, m11, m21, m31);
		}

		// Token: 0x060023BE RID: 9150 RVA: 0x00065DB6 File Offset: 0x00063FB6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint4x2(uint v)
		{
			this.c0 = v;
			this.c1 = v;
		}

		// Token: 0x060023BF RID: 9151 RVA: 0x00065DD0 File Offset: 0x00063FD0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint4x2(bool v)
		{
			this.c0 = math.select(new uint4(0U), new uint4(1U), v);
			this.c1 = math.select(new uint4(0U), new uint4(1U), v);
		}

		// Token: 0x060023C0 RID: 9152 RVA: 0x00065E02 File Offset: 0x00064002
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint4x2(bool4x2 v)
		{
			this.c0 = math.select(new uint4(0U), new uint4(1U), v.c0);
			this.c1 = math.select(new uint4(0U), new uint4(1U), v.c1);
		}

		// Token: 0x060023C1 RID: 9153 RVA: 0x00065E3E File Offset: 0x0006403E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint4x2(int v)
		{
			this.c0 = (uint4)v;
			this.c1 = (uint4)v;
		}

		// Token: 0x060023C2 RID: 9154 RVA: 0x00065E58 File Offset: 0x00064058
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint4x2(int4x2 v)
		{
			this.c0 = (uint4)v.c0;
			this.c1 = (uint4)v.c1;
		}

		// Token: 0x060023C3 RID: 9155 RVA: 0x00065E7C File Offset: 0x0006407C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint4x2(float v)
		{
			this.c0 = (uint4)v;
			this.c1 = (uint4)v;
		}

		// Token: 0x060023C4 RID: 9156 RVA: 0x00065E96 File Offset: 0x00064096
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint4x2(float4x2 v)
		{
			this.c0 = (uint4)v.c0;
			this.c1 = (uint4)v.c1;
		}

		// Token: 0x060023C5 RID: 9157 RVA: 0x00065EBA File Offset: 0x000640BA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint4x2(double v)
		{
			this.c0 = (uint4)v;
			this.c1 = (uint4)v;
		}

		// Token: 0x060023C6 RID: 9158 RVA: 0x00065ED4 File Offset: 0x000640D4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint4x2(double4x2 v)
		{
			this.c0 = (uint4)v.c0;
			this.c1 = (uint4)v.c1;
		}

		// Token: 0x060023C7 RID: 9159 RVA: 0x00065EF8 File Offset: 0x000640F8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator uint4x2(uint v)
		{
			return new uint4x2(v);
		}

		// Token: 0x060023C8 RID: 9160 RVA: 0x00065F00 File Offset: 0x00064100
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint4x2(bool v)
		{
			return new uint4x2(v);
		}

		// Token: 0x060023C9 RID: 9161 RVA: 0x00065F08 File Offset: 0x00064108
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint4x2(bool4x2 v)
		{
			return new uint4x2(v);
		}

		// Token: 0x060023CA RID: 9162 RVA: 0x00065F10 File Offset: 0x00064110
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint4x2(int v)
		{
			return new uint4x2(v);
		}

		// Token: 0x060023CB RID: 9163 RVA: 0x00065F18 File Offset: 0x00064118
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint4x2(int4x2 v)
		{
			return new uint4x2(v);
		}

		// Token: 0x060023CC RID: 9164 RVA: 0x00065F20 File Offset: 0x00064120
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint4x2(float v)
		{
			return new uint4x2(v);
		}

		// Token: 0x060023CD RID: 9165 RVA: 0x00065F28 File Offset: 0x00064128
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint4x2(float4x2 v)
		{
			return new uint4x2(v);
		}

		// Token: 0x060023CE RID: 9166 RVA: 0x00065F30 File Offset: 0x00064130
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint4x2(double v)
		{
			return new uint4x2(v);
		}

		// Token: 0x060023CF RID: 9167 RVA: 0x00065F38 File Offset: 0x00064138
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint4x2(double4x2 v)
		{
			return new uint4x2(v);
		}

		// Token: 0x060023D0 RID: 9168 RVA: 0x00065F40 File Offset: 0x00064140
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x2 operator *(uint4x2 lhs, uint4x2 rhs)
		{
			return new uint4x2(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1);
		}

		// Token: 0x060023D1 RID: 9169 RVA: 0x00065F69 File Offset: 0x00064169
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x2 operator *(uint4x2 lhs, uint rhs)
		{
			return new uint4x2(lhs.c0 * rhs, lhs.c1 * rhs);
		}

		// Token: 0x060023D2 RID: 9170 RVA: 0x00065F88 File Offset: 0x00064188
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x2 operator *(uint lhs, uint4x2 rhs)
		{
			return new uint4x2(lhs * rhs.c0, lhs * rhs.c1);
		}

		// Token: 0x060023D3 RID: 9171 RVA: 0x00065FA7 File Offset: 0x000641A7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x2 operator +(uint4x2 lhs, uint4x2 rhs)
		{
			return new uint4x2(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1);
		}

		// Token: 0x060023D4 RID: 9172 RVA: 0x00065FD0 File Offset: 0x000641D0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x2 operator +(uint4x2 lhs, uint rhs)
		{
			return new uint4x2(lhs.c0 + rhs, lhs.c1 + rhs);
		}

		// Token: 0x060023D5 RID: 9173 RVA: 0x00065FEF File Offset: 0x000641EF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x2 operator +(uint lhs, uint4x2 rhs)
		{
			return new uint4x2(lhs + rhs.c0, lhs + rhs.c1);
		}

		// Token: 0x060023D6 RID: 9174 RVA: 0x0006600E File Offset: 0x0006420E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x2 operator -(uint4x2 lhs, uint4x2 rhs)
		{
			return new uint4x2(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1);
		}

		// Token: 0x060023D7 RID: 9175 RVA: 0x00066037 File Offset: 0x00064237
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x2 operator -(uint4x2 lhs, uint rhs)
		{
			return new uint4x2(lhs.c0 - rhs, lhs.c1 - rhs);
		}

		// Token: 0x060023D8 RID: 9176 RVA: 0x00066056 File Offset: 0x00064256
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x2 operator -(uint lhs, uint4x2 rhs)
		{
			return new uint4x2(lhs - rhs.c0, lhs - rhs.c1);
		}

		// Token: 0x060023D9 RID: 9177 RVA: 0x00066075 File Offset: 0x00064275
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x2 operator /(uint4x2 lhs, uint4x2 rhs)
		{
			return new uint4x2(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1);
		}

		// Token: 0x060023DA RID: 9178 RVA: 0x0006609E File Offset: 0x0006429E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x2 operator /(uint4x2 lhs, uint rhs)
		{
			return new uint4x2(lhs.c0 / rhs, lhs.c1 / rhs);
		}

		// Token: 0x060023DB RID: 9179 RVA: 0x000660BD File Offset: 0x000642BD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x2 operator /(uint lhs, uint4x2 rhs)
		{
			return new uint4x2(lhs / rhs.c0, lhs / rhs.c1);
		}

		// Token: 0x060023DC RID: 9180 RVA: 0x000660DC File Offset: 0x000642DC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x2 operator %(uint4x2 lhs, uint4x2 rhs)
		{
			return new uint4x2(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1);
		}

		// Token: 0x060023DD RID: 9181 RVA: 0x00066105 File Offset: 0x00064305
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x2 operator %(uint4x2 lhs, uint rhs)
		{
			return new uint4x2(lhs.c0 % rhs, lhs.c1 % rhs);
		}

		// Token: 0x060023DE RID: 9182 RVA: 0x00066124 File Offset: 0x00064324
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x2 operator %(uint lhs, uint4x2 rhs)
		{
			return new uint4x2(lhs % rhs.c0, lhs % rhs.c1);
		}

		// Token: 0x060023DF RID: 9183 RVA: 0x00066144 File Offset: 0x00064344
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x2 operator ++(uint4x2 val)
		{
			uint4 @uint = uint4.op_Increment(val.c0);
			val.c0 = @uint;
			uint4 uint2 = @uint;
			@uint = uint4.op_Increment(val.c1);
			val.c1 = @uint;
			return new uint4x2(uint2, @uint);
		}

		// Token: 0x060023E0 RID: 9184 RVA: 0x0006618C File Offset: 0x0006438C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x2 operator --(uint4x2 val)
		{
			uint4 @uint = uint4.op_Decrement(val.c0);
			val.c0 = @uint;
			uint4 uint2 = @uint;
			@uint = uint4.op_Decrement(val.c1);
			val.c1 = @uint;
			return new uint4x2(uint2, @uint);
		}

		// Token: 0x060023E1 RID: 9185 RVA: 0x000661D2 File Offset: 0x000643D2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator <(uint4x2 lhs, uint4x2 rhs)
		{
			return new bool4x2(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1);
		}

		// Token: 0x060023E2 RID: 9186 RVA: 0x000661FB File Offset: 0x000643FB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator <(uint4x2 lhs, uint rhs)
		{
			return new bool4x2(lhs.c0 < rhs, lhs.c1 < rhs);
		}

		// Token: 0x060023E3 RID: 9187 RVA: 0x0006621A File Offset: 0x0006441A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator <(uint lhs, uint4x2 rhs)
		{
			return new bool4x2(lhs < rhs.c0, lhs < rhs.c1);
		}

		// Token: 0x060023E4 RID: 9188 RVA: 0x00066239 File Offset: 0x00064439
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator <=(uint4x2 lhs, uint4x2 rhs)
		{
			return new bool4x2(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1);
		}

		// Token: 0x060023E5 RID: 9189 RVA: 0x00066262 File Offset: 0x00064462
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator <=(uint4x2 lhs, uint rhs)
		{
			return new bool4x2(lhs.c0 <= rhs, lhs.c1 <= rhs);
		}

		// Token: 0x060023E6 RID: 9190 RVA: 0x00066281 File Offset: 0x00064481
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator <=(uint lhs, uint4x2 rhs)
		{
			return new bool4x2(lhs <= rhs.c0, lhs <= rhs.c1);
		}

		// Token: 0x060023E7 RID: 9191 RVA: 0x000662A0 File Offset: 0x000644A0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator >(uint4x2 lhs, uint4x2 rhs)
		{
			return new bool4x2(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1);
		}

		// Token: 0x060023E8 RID: 9192 RVA: 0x000662C9 File Offset: 0x000644C9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator >(uint4x2 lhs, uint rhs)
		{
			return new bool4x2(lhs.c0 > rhs, lhs.c1 > rhs);
		}

		// Token: 0x060023E9 RID: 9193 RVA: 0x000662E8 File Offset: 0x000644E8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator >(uint lhs, uint4x2 rhs)
		{
			return new bool4x2(lhs > rhs.c0, lhs > rhs.c1);
		}

		// Token: 0x060023EA RID: 9194 RVA: 0x00066307 File Offset: 0x00064507
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator >=(uint4x2 lhs, uint4x2 rhs)
		{
			return new bool4x2(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1);
		}

		// Token: 0x060023EB RID: 9195 RVA: 0x00066330 File Offset: 0x00064530
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator >=(uint4x2 lhs, uint rhs)
		{
			return new bool4x2(lhs.c0 >= rhs, lhs.c1 >= rhs);
		}

		// Token: 0x060023EC RID: 9196 RVA: 0x0006634F File Offset: 0x0006454F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator >=(uint lhs, uint4x2 rhs)
		{
			return new bool4x2(lhs >= rhs.c0, lhs >= rhs.c1);
		}

		// Token: 0x060023ED RID: 9197 RVA: 0x0006636E File Offset: 0x0006456E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x2 operator -(uint4x2 val)
		{
			return new uint4x2(-val.c0, -val.c1);
		}

		// Token: 0x060023EE RID: 9198 RVA: 0x0006638B File Offset: 0x0006458B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x2 operator +(uint4x2 val)
		{
			return new uint4x2(+val.c0, +val.c1);
		}

		// Token: 0x060023EF RID: 9199 RVA: 0x000663A8 File Offset: 0x000645A8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x2 operator <<(uint4x2 x, int n)
		{
			return new uint4x2(x.c0 << n, x.c1 << n);
		}

		// Token: 0x060023F0 RID: 9200 RVA: 0x000663C7 File Offset: 0x000645C7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x2 operator >>(uint4x2 x, int n)
		{
			return new uint4x2(x.c0 >> n, x.c1 >> n);
		}

		// Token: 0x060023F1 RID: 9201 RVA: 0x000663E6 File Offset: 0x000645E6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator ==(uint4x2 lhs, uint4x2 rhs)
		{
			return new bool4x2(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1);
		}

		// Token: 0x060023F2 RID: 9202 RVA: 0x0006640F File Offset: 0x0006460F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator ==(uint4x2 lhs, uint rhs)
		{
			return new bool4x2(lhs.c0 == rhs, lhs.c1 == rhs);
		}

		// Token: 0x060023F3 RID: 9203 RVA: 0x0006642E File Offset: 0x0006462E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator ==(uint lhs, uint4x2 rhs)
		{
			return new bool4x2(lhs == rhs.c0, lhs == rhs.c1);
		}

		// Token: 0x060023F4 RID: 9204 RVA: 0x0006644D File Offset: 0x0006464D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator !=(uint4x2 lhs, uint4x2 rhs)
		{
			return new bool4x2(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1);
		}

		// Token: 0x060023F5 RID: 9205 RVA: 0x00066476 File Offset: 0x00064676
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator !=(uint4x2 lhs, uint rhs)
		{
			return new bool4x2(lhs.c0 != rhs, lhs.c1 != rhs);
		}

		// Token: 0x060023F6 RID: 9206 RVA: 0x00066495 File Offset: 0x00064695
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x2 operator !=(uint lhs, uint4x2 rhs)
		{
			return new bool4x2(lhs != rhs.c0, lhs != rhs.c1);
		}

		// Token: 0x060023F7 RID: 9207 RVA: 0x000664B4 File Offset: 0x000646B4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x2 operator ~(uint4x2 val)
		{
			return new uint4x2(~val.c0, ~val.c1);
		}

		// Token: 0x060023F8 RID: 9208 RVA: 0x000664D1 File Offset: 0x000646D1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x2 operator &(uint4x2 lhs, uint4x2 rhs)
		{
			return new uint4x2(lhs.c0 & rhs.c0, lhs.c1 & rhs.c1);
		}

		// Token: 0x060023F9 RID: 9209 RVA: 0x000664FA File Offset: 0x000646FA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x2 operator &(uint4x2 lhs, uint rhs)
		{
			return new uint4x2(lhs.c0 & rhs, lhs.c1 & rhs);
		}

		// Token: 0x060023FA RID: 9210 RVA: 0x00066519 File Offset: 0x00064719
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x2 operator &(uint lhs, uint4x2 rhs)
		{
			return new uint4x2(lhs & rhs.c0, lhs & rhs.c1);
		}

		// Token: 0x060023FB RID: 9211 RVA: 0x00066538 File Offset: 0x00064738
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x2 operator |(uint4x2 lhs, uint4x2 rhs)
		{
			return new uint4x2(lhs.c0 | rhs.c0, lhs.c1 | rhs.c1);
		}

		// Token: 0x060023FC RID: 9212 RVA: 0x00066561 File Offset: 0x00064761
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x2 operator |(uint4x2 lhs, uint rhs)
		{
			return new uint4x2(lhs.c0 | rhs, lhs.c1 | rhs);
		}

		// Token: 0x060023FD RID: 9213 RVA: 0x00066580 File Offset: 0x00064780
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x2 operator |(uint lhs, uint4x2 rhs)
		{
			return new uint4x2(lhs | rhs.c0, lhs | rhs.c1);
		}

		// Token: 0x060023FE RID: 9214 RVA: 0x0006659F File Offset: 0x0006479F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x2 operator ^(uint4x2 lhs, uint4x2 rhs)
		{
			return new uint4x2(lhs.c0 ^ rhs.c0, lhs.c1 ^ rhs.c1);
		}

		// Token: 0x060023FF RID: 9215 RVA: 0x000665C8 File Offset: 0x000647C8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x2 operator ^(uint4x2 lhs, uint rhs)
		{
			return new uint4x2(lhs.c0 ^ rhs, lhs.c1 ^ rhs);
		}

		// Token: 0x06002400 RID: 9216 RVA: 0x000665E7 File Offset: 0x000647E7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x2 operator ^(uint lhs, uint4x2 rhs)
		{
			return new uint4x2(lhs ^ rhs.c0, lhs ^ rhs.c1);
		}

		// Token: 0x17000B87 RID: 2951
		public unsafe ref uint4 this[int index]
		{
			get
			{
				fixed (uint4x2* ptr = &this)
				{
					return ref *(uint4*)(ptr + (IntPtr)index * (IntPtr)sizeof(uint4) / (IntPtr)sizeof(uint4x2));
				}
			}
		}

		// Token: 0x06002402 RID: 9218 RVA: 0x00066623 File Offset: 0x00064823
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(uint4x2 rhs)
		{
			return this.c0.Equals(rhs.c0) && this.c1.Equals(rhs.c1);
		}

		// Token: 0x06002403 RID: 9219 RVA: 0x0006664C File Offset: 0x0006484C
		public override bool Equals(object o)
		{
			if (o is uint4x2)
			{
				uint4x2 uint4x = (uint4x2)o;
				return this.Equals(uint4x);
			}
			return false;
		}

		// Token: 0x06002404 RID: 9220 RVA: 0x00066671 File Offset: 0x00064871
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x06002405 RID: 9221 RVA: 0x00066680 File Offset: 0x00064880
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("uint4x2({0}, {1},  {2}, {3},  {4}, {5},  {6}, {7})", new object[]
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

		// Token: 0x06002406 RID: 9222 RVA: 0x00066738 File Offset: 0x00064938
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("uint4x2({0}, {1},  {2}, {3},  {4}, {5},  {6}, {7})", new object[]
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

		// Token: 0x04000120 RID: 288
		public uint4 c0;

		// Token: 0x04000121 RID: 289
		public uint4 c1;

		// Token: 0x04000122 RID: 290
		public static readonly uint4x2 zero;
	}
}
