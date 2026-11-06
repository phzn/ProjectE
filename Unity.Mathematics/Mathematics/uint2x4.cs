using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x02000044 RID: 68
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct uint2x4 : IEquatable<uint2x4>, IFormattable
	{
		// Token: 0x06001FE2 RID: 8162 RVA: 0x0005C85B File Offset: 0x0005AA5B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2x4(uint2 c0, uint2 c1, uint2 c2, uint2 c3)
		{
			this.c0 = c0;
			this.c1 = c1;
			this.c2 = c2;
			this.c3 = c3;
		}

		// Token: 0x06001FE3 RID: 8163 RVA: 0x0005C87A File Offset: 0x0005AA7A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2x4(uint m00, uint m01, uint m02, uint m03, uint m10, uint m11, uint m12, uint m13)
		{
			this.c0 = new uint2(m00, m10);
			this.c1 = new uint2(m01, m11);
			this.c2 = new uint2(m02, m12);
			this.c3 = new uint2(m03, m13);
		}

		// Token: 0x06001FE4 RID: 8164 RVA: 0x0005C8B5 File Offset: 0x0005AAB5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2x4(uint v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
			this.c3 = v;
		}

		// Token: 0x06001FE5 RID: 8165 RVA: 0x0005C8E8 File Offset: 0x0005AAE8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2x4(bool v)
		{
			this.c0 = math.select(new uint2(0U), new uint2(1U), v);
			this.c1 = math.select(new uint2(0U), new uint2(1U), v);
			this.c2 = math.select(new uint2(0U), new uint2(1U), v);
			this.c3 = math.select(new uint2(0U), new uint2(1U), v);
		}

		// Token: 0x06001FE6 RID: 8166 RVA: 0x0005C958 File Offset: 0x0005AB58
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2x4(bool2x4 v)
		{
			this.c0 = math.select(new uint2(0U), new uint2(1U), v.c0);
			this.c1 = math.select(new uint2(0U), new uint2(1U), v.c1);
			this.c2 = math.select(new uint2(0U), new uint2(1U), v.c2);
			this.c3 = math.select(new uint2(0U), new uint2(1U), v.c3);
		}

		// Token: 0x06001FE7 RID: 8167 RVA: 0x0005C9D9 File Offset: 0x0005ABD9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2x4(int v)
		{
			this.c0 = (uint2)v;
			this.c1 = (uint2)v;
			this.c2 = (uint2)v;
			this.c3 = (uint2)v;
		}

		// Token: 0x06001FE8 RID: 8168 RVA: 0x0005CA0C File Offset: 0x0005AC0C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2x4(int2x4 v)
		{
			this.c0 = (uint2)v.c0;
			this.c1 = (uint2)v.c1;
			this.c2 = (uint2)v.c2;
			this.c3 = (uint2)v.c3;
		}

		// Token: 0x06001FE9 RID: 8169 RVA: 0x0005CA5D File Offset: 0x0005AC5D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2x4(float v)
		{
			this.c0 = (uint2)v;
			this.c1 = (uint2)v;
			this.c2 = (uint2)v;
			this.c3 = (uint2)v;
		}

		// Token: 0x06001FEA RID: 8170 RVA: 0x0005CA90 File Offset: 0x0005AC90
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2x4(float2x4 v)
		{
			this.c0 = (uint2)v.c0;
			this.c1 = (uint2)v.c1;
			this.c2 = (uint2)v.c2;
			this.c3 = (uint2)v.c3;
		}

		// Token: 0x06001FEB RID: 8171 RVA: 0x0005CAE1 File Offset: 0x0005ACE1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2x4(double v)
		{
			this.c0 = (uint2)v;
			this.c1 = (uint2)v;
			this.c2 = (uint2)v;
			this.c3 = (uint2)v;
		}

		// Token: 0x06001FEC RID: 8172 RVA: 0x0005CB14 File Offset: 0x0005AD14
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint2x4(double2x4 v)
		{
			this.c0 = (uint2)v.c0;
			this.c1 = (uint2)v.c1;
			this.c2 = (uint2)v.c2;
			this.c3 = (uint2)v.c3;
		}

		// Token: 0x06001FED RID: 8173 RVA: 0x0005CB65 File Offset: 0x0005AD65
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator uint2x4(uint v)
		{
			return new uint2x4(v);
		}

		// Token: 0x06001FEE RID: 8174 RVA: 0x0005CB6D File Offset: 0x0005AD6D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint2x4(bool v)
		{
			return new uint2x4(v);
		}

		// Token: 0x06001FEF RID: 8175 RVA: 0x0005CB75 File Offset: 0x0005AD75
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint2x4(bool2x4 v)
		{
			return new uint2x4(v);
		}

		// Token: 0x06001FF0 RID: 8176 RVA: 0x0005CB7D File Offset: 0x0005AD7D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint2x4(int v)
		{
			return new uint2x4(v);
		}

		// Token: 0x06001FF1 RID: 8177 RVA: 0x0005CB85 File Offset: 0x0005AD85
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint2x4(int2x4 v)
		{
			return new uint2x4(v);
		}

		// Token: 0x06001FF2 RID: 8178 RVA: 0x0005CB8D File Offset: 0x0005AD8D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint2x4(float v)
		{
			return new uint2x4(v);
		}

		// Token: 0x06001FF3 RID: 8179 RVA: 0x0005CB95 File Offset: 0x0005AD95
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint2x4(float2x4 v)
		{
			return new uint2x4(v);
		}

		// Token: 0x06001FF4 RID: 8180 RVA: 0x0005CB9D File Offset: 0x0005AD9D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint2x4(double v)
		{
			return new uint2x4(v);
		}

		// Token: 0x06001FF5 RID: 8181 RVA: 0x0005CBA5 File Offset: 0x0005ADA5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint2x4(double2x4 v)
		{
			return new uint2x4(v);
		}

		// Token: 0x06001FF6 RID: 8182 RVA: 0x0005CBB0 File Offset: 0x0005ADB0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x4 operator *(uint2x4 lhs, uint2x4 rhs)
		{
			return new uint2x4(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1, lhs.c2 * rhs.c2, lhs.c3 * rhs.c3);
		}

		// Token: 0x06001FF7 RID: 8183 RVA: 0x0005CC06 File Offset: 0x0005AE06
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x4 operator *(uint2x4 lhs, uint rhs)
		{
			return new uint2x4(lhs.c0 * rhs, lhs.c1 * rhs, lhs.c2 * rhs, lhs.c3 * rhs);
		}

		// Token: 0x06001FF8 RID: 8184 RVA: 0x0005CC3D File Offset: 0x0005AE3D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x4 operator *(uint lhs, uint2x4 rhs)
		{
			return new uint2x4(lhs * rhs.c0, lhs * rhs.c1, lhs * rhs.c2, lhs * rhs.c3);
		}

		// Token: 0x06001FF9 RID: 8185 RVA: 0x0005CC74 File Offset: 0x0005AE74
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x4 operator +(uint2x4 lhs, uint2x4 rhs)
		{
			return new uint2x4(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1, lhs.c2 + rhs.c2, lhs.c3 + rhs.c3);
		}

		// Token: 0x06001FFA RID: 8186 RVA: 0x0005CCCA File Offset: 0x0005AECA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x4 operator +(uint2x4 lhs, uint rhs)
		{
			return new uint2x4(lhs.c0 + rhs, lhs.c1 + rhs, lhs.c2 + rhs, lhs.c3 + rhs);
		}

		// Token: 0x06001FFB RID: 8187 RVA: 0x0005CD01 File Offset: 0x0005AF01
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x4 operator +(uint lhs, uint2x4 rhs)
		{
			return new uint2x4(lhs + rhs.c0, lhs + rhs.c1, lhs + rhs.c2, lhs + rhs.c3);
		}

		// Token: 0x06001FFC RID: 8188 RVA: 0x0005CD38 File Offset: 0x0005AF38
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x4 operator -(uint2x4 lhs, uint2x4 rhs)
		{
			return new uint2x4(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1, lhs.c2 - rhs.c2, lhs.c3 - rhs.c3);
		}

		// Token: 0x06001FFD RID: 8189 RVA: 0x0005CD8E File Offset: 0x0005AF8E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x4 operator -(uint2x4 lhs, uint rhs)
		{
			return new uint2x4(lhs.c0 - rhs, lhs.c1 - rhs, lhs.c2 - rhs, lhs.c3 - rhs);
		}

		// Token: 0x06001FFE RID: 8190 RVA: 0x0005CDC5 File Offset: 0x0005AFC5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x4 operator -(uint lhs, uint2x4 rhs)
		{
			return new uint2x4(lhs - rhs.c0, lhs - rhs.c1, lhs - rhs.c2, lhs - rhs.c3);
		}

		// Token: 0x06001FFF RID: 8191 RVA: 0x0005CDFC File Offset: 0x0005AFFC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x4 operator /(uint2x4 lhs, uint2x4 rhs)
		{
			return new uint2x4(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1, lhs.c2 / rhs.c2, lhs.c3 / rhs.c3);
		}

		// Token: 0x06002000 RID: 8192 RVA: 0x0005CE52 File Offset: 0x0005B052
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x4 operator /(uint2x4 lhs, uint rhs)
		{
			return new uint2x4(lhs.c0 / rhs, lhs.c1 / rhs, lhs.c2 / rhs, lhs.c3 / rhs);
		}

		// Token: 0x06002001 RID: 8193 RVA: 0x0005CE89 File Offset: 0x0005B089
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x4 operator /(uint lhs, uint2x4 rhs)
		{
			return new uint2x4(lhs / rhs.c0, lhs / rhs.c1, lhs / rhs.c2, lhs / rhs.c3);
		}

		// Token: 0x06002002 RID: 8194 RVA: 0x0005CEC0 File Offset: 0x0005B0C0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x4 operator %(uint2x4 lhs, uint2x4 rhs)
		{
			return new uint2x4(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1, lhs.c2 % rhs.c2, lhs.c3 % rhs.c3);
		}

		// Token: 0x06002003 RID: 8195 RVA: 0x0005CF16 File Offset: 0x0005B116
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x4 operator %(uint2x4 lhs, uint rhs)
		{
			return new uint2x4(lhs.c0 % rhs, lhs.c1 % rhs, lhs.c2 % rhs, lhs.c3 % rhs);
		}

		// Token: 0x06002004 RID: 8196 RVA: 0x0005CF4D File Offset: 0x0005B14D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x4 operator %(uint lhs, uint2x4 rhs)
		{
			return new uint2x4(lhs % rhs.c0, lhs % rhs.c1, lhs % rhs.c2, lhs % rhs.c3);
		}

		// Token: 0x06002005 RID: 8197 RVA: 0x0005CF84 File Offset: 0x0005B184
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x4 operator ++(uint2x4 val)
		{
			uint2 @uint = uint2.op_Increment(val.c0);
			val.c0 = @uint;
			uint2 uint2 = @uint;
			@uint = uint2.op_Increment(val.c1);
			val.c1 = @uint;
			uint2 uint3 = @uint;
			@uint = uint2.op_Increment(val.c2);
			val.c2 = @uint;
			uint2 uint4 = @uint;
			@uint = uint2.op_Increment(val.c3);
			val.c3 = @uint;
			return new uint2x4(uint2, uint3, uint4, @uint);
		}

		// Token: 0x06002006 RID: 8198 RVA: 0x0005D000 File Offset: 0x0005B200
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x4 operator --(uint2x4 val)
		{
			uint2 @uint = uint2.op_Decrement(val.c0);
			val.c0 = @uint;
			uint2 uint2 = @uint;
			@uint = uint2.op_Decrement(val.c1);
			val.c1 = @uint;
			uint2 uint3 = @uint;
			@uint = uint2.op_Decrement(val.c2);
			val.c2 = @uint;
			uint2 uint4 = @uint;
			@uint = uint2.op_Decrement(val.c3);
			val.c3 = @uint;
			return new uint2x4(uint2, uint3, uint4, @uint);
		}

		// Token: 0x06002007 RID: 8199 RVA: 0x0005D07C File Offset: 0x0005B27C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator <(uint2x4 lhs, uint2x4 rhs)
		{
			return new bool2x4(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1, lhs.c2 < rhs.c2, lhs.c3 < rhs.c3);
		}

		// Token: 0x06002008 RID: 8200 RVA: 0x0005D0D2 File Offset: 0x0005B2D2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator <(uint2x4 lhs, uint rhs)
		{
			return new bool2x4(lhs.c0 < rhs, lhs.c1 < rhs, lhs.c2 < rhs, lhs.c3 < rhs);
		}

		// Token: 0x06002009 RID: 8201 RVA: 0x0005D109 File Offset: 0x0005B309
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator <(uint lhs, uint2x4 rhs)
		{
			return new bool2x4(lhs < rhs.c0, lhs < rhs.c1, lhs < rhs.c2, lhs < rhs.c3);
		}

		// Token: 0x0600200A RID: 8202 RVA: 0x0005D140 File Offset: 0x0005B340
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator <=(uint2x4 lhs, uint2x4 rhs)
		{
			return new bool2x4(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1, lhs.c2 <= rhs.c2, lhs.c3 <= rhs.c3);
		}

		// Token: 0x0600200B RID: 8203 RVA: 0x0005D196 File Offset: 0x0005B396
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator <=(uint2x4 lhs, uint rhs)
		{
			return new bool2x4(lhs.c0 <= rhs, lhs.c1 <= rhs, lhs.c2 <= rhs, lhs.c3 <= rhs);
		}

		// Token: 0x0600200C RID: 8204 RVA: 0x0005D1CD File Offset: 0x0005B3CD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator <=(uint lhs, uint2x4 rhs)
		{
			return new bool2x4(lhs <= rhs.c0, lhs <= rhs.c1, lhs <= rhs.c2, lhs <= rhs.c3);
		}

		// Token: 0x0600200D RID: 8205 RVA: 0x0005D204 File Offset: 0x0005B404
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator >(uint2x4 lhs, uint2x4 rhs)
		{
			return new bool2x4(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1, lhs.c2 > rhs.c2, lhs.c3 > rhs.c3);
		}

		// Token: 0x0600200E RID: 8206 RVA: 0x0005D25A File Offset: 0x0005B45A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator >(uint2x4 lhs, uint rhs)
		{
			return new bool2x4(lhs.c0 > rhs, lhs.c1 > rhs, lhs.c2 > rhs, lhs.c3 > rhs);
		}

		// Token: 0x0600200F RID: 8207 RVA: 0x0005D291 File Offset: 0x0005B491
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator >(uint lhs, uint2x4 rhs)
		{
			return new bool2x4(lhs > rhs.c0, lhs > rhs.c1, lhs > rhs.c2, lhs > rhs.c3);
		}

		// Token: 0x06002010 RID: 8208 RVA: 0x0005D2C8 File Offset: 0x0005B4C8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator >=(uint2x4 lhs, uint2x4 rhs)
		{
			return new bool2x4(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1, lhs.c2 >= rhs.c2, lhs.c3 >= rhs.c3);
		}

		// Token: 0x06002011 RID: 8209 RVA: 0x0005D31E File Offset: 0x0005B51E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator >=(uint2x4 lhs, uint rhs)
		{
			return new bool2x4(lhs.c0 >= rhs, lhs.c1 >= rhs, lhs.c2 >= rhs, lhs.c3 >= rhs);
		}

		// Token: 0x06002012 RID: 8210 RVA: 0x0005D355 File Offset: 0x0005B555
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator >=(uint lhs, uint2x4 rhs)
		{
			return new bool2x4(lhs >= rhs.c0, lhs >= rhs.c1, lhs >= rhs.c2, lhs >= rhs.c3);
		}

		// Token: 0x06002013 RID: 8211 RVA: 0x0005D38C File Offset: 0x0005B58C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x4 operator -(uint2x4 val)
		{
			return new uint2x4(-val.c0, -val.c1, -val.c2, -val.c3);
		}

		// Token: 0x06002014 RID: 8212 RVA: 0x0005D3BF File Offset: 0x0005B5BF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x4 operator +(uint2x4 val)
		{
			return new uint2x4(+val.c0, +val.c1, +val.c2, +val.c3);
		}

		// Token: 0x06002015 RID: 8213 RVA: 0x0005D3F2 File Offset: 0x0005B5F2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x4 operator <<(uint2x4 x, int n)
		{
			return new uint2x4(x.c0 << n, x.c1 << n, x.c2 << n, x.c3 << n);
		}

		// Token: 0x06002016 RID: 8214 RVA: 0x0005D429 File Offset: 0x0005B629
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x4 operator >>(uint2x4 x, int n)
		{
			return new uint2x4(x.c0 >> n, x.c1 >> n, x.c2 >> n, x.c3 >> n);
		}

		// Token: 0x06002017 RID: 8215 RVA: 0x0005D460 File Offset: 0x0005B660
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator ==(uint2x4 lhs, uint2x4 rhs)
		{
			return new bool2x4(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2, lhs.c3 == rhs.c3);
		}

		// Token: 0x06002018 RID: 8216 RVA: 0x0005D4B6 File Offset: 0x0005B6B6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator ==(uint2x4 lhs, uint rhs)
		{
			return new bool2x4(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs, lhs.c3 == rhs);
		}

		// Token: 0x06002019 RID: 8217 RVA: 0x0005D4ED File Offset: 0x0005B6ED
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator ==(uint lhs, uint2x4 rhs)
		{
			return new bool2x4(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2, lhs == rhs.c3);
		}

		// Token: 0x0600201A RID: 8218 RVA: 0x0005D524 File Offset: 0x0005B724
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator !=(uint2x4 lhs, uint2x4 rhs)
		{
			return new bool2x4(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2, lhs.c3 != rhs.c3);
		}

		// Token: 0x0600201B RID: 8219 RVA: 0x0005D57A File Offset: 0x0005B77A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator !=(uint2x4 lhs, uint rhs)
		{
			return new bool2x4(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs, lhs.c3 != rhs);
		}

		// Token: 0x0600201C RID: 8220 RVA: 0x0005D5B1 File Offset: 0x0005B7B1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2x4 operator !=(uint lhs, uint2x4 rhs)
		{
			return new bool2x4(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2, lhs != rhs.c3);
		}

		// Token: 0x0600201D RID: 8221 RVA: 0x0005D5E8 File Offset: 0x0005B7E8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x4 operator ~(uint2x4 val)
		{
			return new uint2x4(~val.c0, ~val.c1, ~val.c2, ~val.c3);
		}

		// Token: 0x0600201E RID: 8222 RVA: 0x0005D61C File Offset: 0x0005B81C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x4 operator &(uint2x4 lhs, uint2x4 rhs)
		{
			return new uint2x4(lhs.c0 & rhs.c0, lhs.c1 & rhs.c1, lhs.c2 & rhs.c2, lhs.c3 & rhs.c3);
		}

		// Token: 0x0600201F RID: 8223 RVA: 0x0005D672 File Offset: 0x0005B872
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x4 operator &(uint2x4 lhs, uint rhs)
		{
			return new uint2x4(lhs.c0 & rhs, lhs.c1 & rhs, lhs.c2 & rhs, lhs.c3 & rhs);
		}

		// Token: 0x06002020 RID: 8224 RVA: 0x0005D6A9 File Offset: 0x0005B8A9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x4 operator &(uint lhs, uint2x4 rhs)
		{
			return new uint2x4(lhs & rhs.c0, lhs & rhs.c1, lhs & rhs.c2, lhs & rhs.c3);
		}

		// Token: 0x06002021 RID: 8225 RVA: 0x0005D6E0 File Offset: 0x0005B8E0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x4 operator |(uint2x4 lhs, uint2x4 rhs)
		{
			return new uint2x4(lhs.c0 | rhs.c0, lhs.c1 | rhs.c1, lhs.c2 | rhs.c2, lhs.c3 | rhs.c3);
		}

		// Token: 0x06002022 RID: 8226 RVA: 0x0005D736 File Offset: 0x0005B936
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x4 operator |(uint2x4 lhs, uint rhs)
		{
			return new uint2x4(lhs.c0 | rhs, lhs.c1 | rhs, lhs.c2 | rhs, lhs.c3 | rhs);
		}

		// Token: 0x06002023 RID: 8227 RVA: 0x0005D76D File Offset: 0x0005B96D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x4 operator |(uint lhs, uint2x4 rhs)
		{
			return new uint2x4(lhs | rhs.c0, lhs | rhs.c1, lhs | rhs.c2, lhs | rhs.c3);
		}

		// Token: 0x06002024 RID: 8228 RVA: 0x0005D7A4 File Offset: 0x0005B9A4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x4 operator ^(uint2x4 lhs, uint2x4 rhs)
		{
			return new uint2x4(lhs.c0 ^ rhs.c0, lhs.c1 ^ rhs.c1, lhs.c2 ^ rhs.c2, lhs.c3 ^ rhs.c3);
		}

		// Token: 0x06002025 RID: 8229 RVA: 0x0005D7FA File Offset: 0x0005B9FA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x4 operator ^(uint2x4 lhs, uint rhs)
		{
			return new uint2x4(lhs.c0 ^ rhs, lhs.c1 ^ rhs, lhs.c2 ^ rhs, lhs.c3 ^ rhs);
		}

		// Token: 0x06002026 RID: 8230 RVA: 0x0005D831 File Offset: 0x0005BA31
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint2x4 operator ^(uint lhs, uint2x4 rhs)
		{
			return new uint2x4(lhs ^ rhs.c0, lhs ^ rhs.c1, lhs ^ rhs.c2, lhs ^ rhs.c3);
		}

		// Token: 0x170009BC RID: 2492
		public unsafe ref uint2 this[int index]
		{
			get
			{
				fixed (uint2x4* ptr = &this)
				{
					return ref *(uint2*)(ptr + (IntPtr)index * (IntPtr)sizeof(uint2) / (IntPtr)sizeof(uint2x4));
				}
			}
		}

		// Token: 0x06002028 RID: 8232 RVA: 0x0005D884 File Offset: 0x0005BA84
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(uint2x4 rhs)
		{
			return this.c0.Equals(rhs.c0) && this.c1.Equals(rhs.c1) && this.c2.Equals(rhs.c2) && this.c3.Equals(rhs.c3);
		}

		// Token: 0x06002029 RID: 8233 RVA: 0x0005D8E0 File Offset: 0x0005BAE0
		public override bool Equals(object o)
		{
			if (o is uint2x4)
			{
				uint2x4 uint2x = (uint2x4)o;
				return this.Equals(uint2x);
			}
			return false;
		}

		// Token: 0x0600202A RID: 8234 RVA: 0x0005D905 File Offset: 0x0005BB05
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x0600202B RID: 8235 RVA: 0x0005D914 File Offset: 0x0005BB14
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("uint2x4({0}, {1}, {2}, {3},  {4}, {5}, {6}, {7})", new object[]
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

		// Token: 0x0600202C RID: 8236 RVA: 0x0005D9CC File Offset: 0x0005BBCC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("uint2x4({0}, {1}, {2}, {3},  {4}, {5}, {6}, {7})", new object[]
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

		// Token: 0x04000105 RID: 261
		public uint2 c0;

		// Token: 0x04000106 RID: 262
		public uint2 c1;

		// Token: 0x04000107 RID: 263
		public uint2 c2;

		// Token: 0x04000108 RID: 264
		public uint2 c3;

		// Token: 0x04000109 RID: 265
		public static readonly uint2x4 zero;
	}
}
