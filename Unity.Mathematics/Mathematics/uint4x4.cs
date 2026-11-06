using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x0200004C RID: 76
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct uint4x4 : IEquatable<uint4x4>, IFormattable
	{
		// Token: 0x06002452 RID: 9298 RVA: 0x000676AD File Offset: 0x000658AD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint4x4(uint4 c0, uint4 c1, uint4 c2, uint4 c3)
		{
			this.c0 = c0;
			this.c1 = c1;
			this.c2 = c2;
			this.c3 = c3;
		}

		// Token: 0x06002453 RID: 9299 RVA: 0x000676CC File Offset: 0x000658CC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint4x4(uint m00, uint m01, uint m02, uint m03, uint m10, uint m11, uint m12, uint m13, uint m20, uint m21, uint m22, uint m23, uint m30, uint m31, uint m32, uint m33)
		{
			this.c0 = new uint4(m00, m10, m20, m30);
			this.c1 = new uint4(m01, m11, m21, m31);
			this.c2 = new uint4(m02, m12, m22, m32);
			this.c3 = new uint4(m03, m13, m23, m33);
		}

		// Token: 0x06002454 RID: 9300 RVA: 0x00067722 File Offset: 0x00065922
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint4x4(uint v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
			this.c3 = v;
		}

		// Token: 0x06002455 RID: 9301 RVA: 0x00067754 File Offset: 0x00065954
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint4x4(bool v)
		{
			this.c0 = math.select(new uint4(0U), new uint4(1U), v);
			this.c1 = math.select(new uint4(0U), new uint4(1U), v);
			this.c2 = math.select(new uint4(0U), new uint4(1U), v);
			this.c3 = math.select(new uint4(0U), new uint4(1U), v);
		}

		// Token: 0x06002456 RID: 9302 RVA: 0x000677C4 File Offset: 0x000659C4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint4x4(bool4x4 v)
		{
			this.c0 = math.select(new uint4(0U), new uint4(1U), v.c0);
			this.c1 = math.select(new uint4(0U), new uint4(1U), v.c1);
			this.c2 = math.select(new uint4(0U), new uint4(1U), v.c2);
			this.c3 = math.select(new uint4(0U), new uint4(1U), v.c3);
		}

		// Token: 0x06002457 RID: 9303 RVA: 0x00067845 File Offset: 0x00065A45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint4x4(int v)
		{
			this.c0 = (uint4)v;
			this.c1 = (uint4)v;
			this.c2 = (uint4)v;
			this.c3 = (uint4)v;
		}

		// Token: 0x06002458 RID: 9304 RVA: 0x00067878 File Offset: 0x00065A78
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint4x4(int4x4 v)
		{
			this.c0 = (uint4)v.c0;
			this.c1 = (uint4)v.c1;
			this.c2 = (uint4)v.c2;
			this.c3 = (uint4)v.c3;
		}

		// Token: 0x06002459 RID: 9305 RVA: 0x000678C9 File Offset: 0x00065AC9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint4x4(float v)
		{
			this.c0 = (uint4)v;
			this.c1 = (uint4)v;
			this.c2 = (uint4)v;
			this.c3 = (uint4)v;
		}

		// Token: 0x0600245A RID: 9306 RVA: 0x000678FC File Offset: 0x00065AFC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint4x4(float4x4 v)
		{
			this.c0 = (uint4)v.c0;
			this.c1 = (uint4)v.c1;
			this.c2 = (uint4)v.c2;
			this.c3 = (uint4)v.c3;
		}

		// Token: 0x0600245B RID: 9307 RVA: 0x0006794D File Offset: 0x00065B4D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint4x4(double v)
		{
			this.c0 = (uint4)v;
			this.c1 = (uint4)v;
			this.c2 = (uint4)v;
			this.c3 = (uint4)v;
		}

		// Token: 0x0600245C RID: 9308 RVA: 0x00067980 File Offset: 0x00065B80
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint4x4(double4x4 v)
		{
			this.c0 = (uint4)v.c0;
			this.c1 = (uint4)v.c1;
			this.c2 = (uint4)v.c2;
			this.c3 = (uint4)v.c3;
		}

		// Token: 0x0600245D RID: 9309 RVA: 0x000679D1 File Offset: 0x00065BD1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator uint4x4(uint v)
		{
			return new uint4x4(v);
		}

		// Token: 0x0600245E RID: 9310 RVA: 0x000679D9 File Offset: 0x00065BD9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint4x4(bool v)
		{
			return new uint4x4(v);
		}

		// Token: 0x0600245F RID: 9311 RVA: 0x000679E1 File Offset: 0x00065BE1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint4x4(bool4x4 v)
		{
			return new uint4x4(v);
		}

		// Token: 0x06002460 RID: 9312 RVA: 0x000679E9 File Offset: 0x00065BE9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint4x4(int v)
		{
			return new uint4x4(v);
		}

		// Token: 0x06002461 RID: 9313 RVA: 0x000679F1 File Offset: 0x00065BF1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint4x4(int4x4 v)
		{
			return new uint4x4(v);
		}

		// Token: 0x06002462 RID: 9314 RVA: 0x000679F9 File Offset: 0x00065BF9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint4x4(float v)
		{
			return new uint4x4(v);
		}

		// Token: 0x06002463 RID: 9315 RVA: 0x00067A01 File Offset: 0x00065C01
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint4x4(float4x4 v)
		{
			return new uint4x4(v);
		}

		// Token: 0x06002464 RID: 9316 RVA: 0x00067A09 File Offset: 0x00065C09
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint4x4(double v)
		{
			return new uint4x4(v);
		}

		// Token: 0x06002465 RID: 9317 RVA: 0x00067A11 File Offset: 0x00065C11
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint4x4(double4x4 v)
		{
			return new uint4x4(v);
		}

		// Token: 0x06002466 RID: 9318 RVA: 0x00067A1C File Offset: 0x00065C1C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x4 operator *(uint4x4 lhs, uint4x4 rhs)
		{
			return new uint4x4(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1, lhs.c2 * rhs.c2, lhs.c3 * rhs.c3);
		}

		// Token: 0x06002467 RID: 9319 RVA: 0x00067A72 File Offset: 0x00065C72
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x4 operator *(uint4x4 lhs, uint rhs)
		{
			return new uint4x4(lhs.c0 * rhs, lhs.c1 * rhs, lhs.c2 * rhs, lhs.c3 * rhs);
		}

		// Token: 0x06002468 RID: 9320 RVA: 0x00067AA9 File Offset: 0x00065CA9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x4 operator *(uint lhs, uint4x4 rhs)
		{
			return new uint4x4(lhs * rhs.c0, lhs * rhs.c1, lhs * rhs.c2, lhs * rhs.c3);
		}

		// Token: 0x06002469 RID: 9321 RVA: 0x00067AE0 File Offset: 0x00065CE0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x4 operator +(uint4x4 lhs, uint4x4 rhs)
		{
			return new uint4x4(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1, lhs.c2 + rhs.c2, lhs.c3 + rhs.c3);
		}

		// Token: 0x0600246A RID: 9322 RVA: 0x00067B36 File Offset: 0x00065D36
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x4 operator +(uint4x4 lhs, uint rhs)
		{
			return new uint4x4(lhs.c0 + rhs, lhs.c1 + rhs, lhs.c2 + rhs, lhs.c3 + rhs);
		}

		// Token: 0x0600246B RID: 9323 RVA: 0x00067B6D File Offset: 0x00065D6D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x4 operator +(uint lhs, uint4x4 rhs)
		{
			return new uint4x4(lhs + rhs.c0, lhs + rhs.c1, lhs + rhs.c2, lhs + rhs.c3);
		}

		// Token: 0x0600246C RID: 9324 RVA: 0x00067BA4 File Offset: 0x00065DA4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x4 operator -(uint4x4 lhs, uint4x4 rhs)
		{
			return new uint4x4(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1, lhs.c2 - rhs.c2, lhs.c3 - rhs.c3);
		}

		// Token: 0x0600246D RID: 9325 RVA: 0x00067BFA File Offset: 0x00065DFA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x4 operator -(uint4x4 lhs, uint rhs)
		{
			return new uint4x4(lhs.c0 - rhs, lhs.c1 - rhs, lhs.c2 - rhs, lhs.c3 - rhs);
		}

		// Token: 0x0600246E RID: 9326 RVA: 0x00067C31 File Offset: 0x00065E31
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x4 operator -(uint lhs, uint4x4 rhs)
		{
			return new uint4x4(lhs - rhs.c0, lhs - rhs.c1, lhs - rhs.c2, lhs - rhs.c3);
		}

		// Token: 0x0600246F RID: 9327 RVA: 0x00067C68 File Offset: 0x00065E68
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x4 operator /(uint4x4 lhs, uint4x4 rhs)
		{
			return new uint4x4(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1, lhs.c2 / rhs.c2, lhs.c3 / rhs.c3);
		}

		// Token: 0x06002470 RID: 9328 RVA: 0x00067CBE File Offset: 0x00065EBE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x4 operator /(uint4x4 lhs, uint rhs)
		{
			return new uint4x4(lhs.c0 / rhs, lhs.c1 / rhs, lhs.c2 / rhs, lhs.c3 / rhs);
		}

		// Token: 0x06002471 RID: 9329 RVA: 0x00067CF5 File Offset: 0x00065EF5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x4 operator /(uint lhs, uint4x4 rhs)
		{
			return new uint4x4(lhs / rhs.c0, lhs / rhs.c1, lhs / rhs.c2, lhs / rhs.c3);
		}

		// Token: 0x06002472 RID: 9330 RVA: 0x00067D2C File Offset: 0x00065F2C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x4 operator %(uint4x4 lhs, uint4x4 rhs)
		{
			return new uint4x4(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1, lhs.c2 % rhs.c2, lhs.c3 % rhs.c3);
		}

		// Token: 0x06002473 RID: 9331 RVA: 0x00067D82 File Offset: 0x00065F82
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x4 operator %(uint4x4 lhs, uint rhs)
		{
			return new uint4x4(lhs.c0 % rhs, lhs.c1 % rhs, lhs.c2 % rhs, lhs.c3 % rhs);
		}

		// Token: 0x06002474 RID: 9332 RVA: 0x00067DB9 File Offset: 0x00065FB9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x4 operator %(uint lhs, uint4x4 rhs)
		{
			return new uint4x4(lhs % rhs.c0, lhs % rhs.c1, lhs % rhs.c2, lhs % rhs.c3);
		}

		// Token: 0x06002475 RID: 9333 RVA: 0x00067DF0 File Offset: 0x00065FF0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x4 operator ++(uint4x4 val)
		{
			uint4 @uint = uint4.op_Increment(val.c0);
			val.c0 = @uint;
			uint4 uint2 = @uint;
			@uint = uint4.op_Increment(val.c1);
			val.c1 = @uint;
			uint4 uint3 = @uint;
			@uint = uint4.op_Increment(val.c2);
			val.c2 = @uint;
			uint4 uint4 = @uint;
			@uint = uint4.op_Increment(val.c3);
			val.c3 = @uint;
			return new uint4x4(uint2, uint3, uint4, @uint);
		}

		// Token: 0x06002476 RID: 9334 RVA: 0x00067E6C File Offset: 0x0006606C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x4 operator --(uint4x4 val)
		{
			uint4 @uint = uint4.op_Decrement(val.c0);
			val.c0 = @uint;
			uint4 uint2 = @uint;
			@uint = uint4.op_Decrement(val.c1);
			val.c1 = @uint;
			uint4 uint3 = @uint;
			@uint = uint4.op_Decrement(val.c2);
			val.c2 = @uint;
			uint4 uint4 = @uint;
			@uint = uint4.op_Decrement(val.c3);
			val.c3 = @uint;
			return new uint4x4(uint2, uint3, uint4, @uint);
		}

		// Token: 0x06002477 RID: 9335 RVA: 0x00067EE8 File Offset: 0x000660E8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator <(uint4x4 lhs, uint4x4 rhs)
		{
			return new bool4x4(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1, lhs.c2 < rhs.c2, lhs.c3 < rhs.c3);
		}

		// Token: 0x06002478 RID: 9336 RVA: 0x00067F3E File Offset: 0x0006613E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator <(uint4x4 lhs, uint rhs)
		{
			return new bool4x4(lhs.c0 < rhs, lhs.c1 < rhs, lhs.c2 < rhs, lhs.c3 < rhs);
		}

		// Token: 0x06002479 RID: 9337 RVA: 0x00067F75 File Offset: 0x00066175
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator <(uint lhs, uint4x4 rhs)
		{
			return new bool4x4(lhs < rhs.c0, lhs < rhs.c1, lhs < rhs.c2, lhs < rhs.c3);
		}

		// Token: 0x0600247A RID: 9338 RVA: 0x00067FAC File Offset: 0x000661AC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator <=(uint4x4 lhs, uint4x4 rhs)
		{
			return new bool4x4(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1, lhs.c2 <= rhs.c2, lhs.c3 <= rhs.c3);
		}

		// Token: 0x0600247B RID: 9339 RVA: 0x00068002 File Offset: 0x00066202
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator <=(uint4x4 lhs, uint rhs)
		{
			return new bool4x4(lhs.c0 <= rhs, lhs.c1 <= rhs, lhs.c2 <= rhs, lhs.c3 <= rhs);
		}

		// Token: 0x0600247C RID: 9340 RVA: 0x00068039 File Offset: 0x00066239
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator <=(uint lhs, uint4x4 rhs)
		{
			return new bool4x4(lhs <= rhs.c0, lhs <= rhs.c1, lhs <= rhs.c2, lhs <= rhs.c3);
		}

		// Token: 0x0600247D RID: 9341 RVA: 0x00068070 File Offset: 0x00066270
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator >(uint4x4 lhs, uint4x4 rhs)
		{
			return new bool4x4(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1, lhs.c2 > rhs.c2, lhs.c3 > rhs.c3);
		}

		// Token: 0x0600247E RID: 9342 RVA: 0x000680C6 File Offset: 0x000662C6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator >(uint4x4 lhs, uint rhs)
		{
			return new bool4x4(lhs.c0 > rhs, lhs.c1 > rhs, lhs.c2 > rhs, lhs.c3 > rhs);
		}

		// Token: 0x0600247F RID: 9343 RVA: 0x000680FD File Offset: 0x000662FD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator >(uint lhs, uint4x4 rhs)
		{
			return new bool4x4(lhs > rhs.c0, lhs > rhs.c1, lhs > rhs.c2, lhs > rhs.c3);
		}

		// Token: 0x06002480 RID: 9344 RVA: 0x00068134 File Offset: 0x00066334
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator >=(uint4x4 lhs, uint4x4 rhs)
		{
			return new bool4x4(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1, lhs.c2 >= rhs.c2, lhs.c3 >= rhs.c3);
		}

		// Token: 0x06002481 RID: 9345 RVA: 0x0006818A File Offset: 0x0006638A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator >=(uint4x4 lhs, uint rhs)
		{
			return new bool4x4(lhs.c0 >= rhs, lhs.c1 >= rhs, lhs.c2 >= rhs, lhs.c3 >= rhs);
		}

		// Token: 0x06002482 RID: 9346 RVA: 0x000681C1 File Offset: 0x000663C1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator >=(uint lhs, uint4x4 rhs)
		{
			return new bool4x4(lhs >= rhs.c0, lhs >= rhs.c1, lhs >= rhs.c2, lhs >= rhs.c3);
		}

		// Token: 0x06002483 RID: 9347 RVA: 0x000681F8 File Offset: 0x000663F8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x4 operator -(uint4x4 val)
		{
			return new uint4x4(-val.c0, -val.c1, -val.c2, -val.c3);
		}

		// Token: 0x06002484 RID: 9348 RVA: 0x0006822B File Offset: 0x0006642B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x4 operator +(uint4x4 val)
		{
			return new uint4x4(+val.c0, +val.c1, +val.c2, +val.c3);
		}

		// Token: 0x06002485 RID: 9349 RVA: 0x0006825E File Offset: 0x0006645E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x4 operator <<(uint4x4 x, int n)
		{
			return new uint4x4(x.c0 << n, x.c1 << n, x.c2 << n, x.c3 << n);
		}

		// Token: 0x06002486 RID: 9350 RVA: 0x00068295 File Offset: 0x00066495
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x4 operator >>(uint4x4 x, int n)
		{
			return new uint4x4(x.c0 >> n, x.c1 >> n, x.c2 >> n, x.c3 >> n);
		}

		// Token: 0x06002487 RID: 9351 RVA: 0x000682CC File Offset: 0x000664CC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator ==(uint4x4 lhs, uint4x4 rhs)
		{
			return new bool4x4(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2, lhs.c3 == rhs.c3);
		}

		// Token: 0x06002488 RID: 9352 RVA: 0x00068322 File Offset: 0x00066522
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator ==(uint4x4 lhs, uint rhs)
		{
			return new bool4x4(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs, lhs.c3 == rhs);
		}

		// Token: 0x06002489 RID: 9353 RVA: 0x00068359 File Offset: 0x00066559
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator ==(uint lhs, uint4x4 rhs)
		{
			return new bool4x4(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2, lhs == rhs.c3);
		}

		// Token: 0x0600248A RID: 9354 RVA: 0x00068390 File Offset: 0x00066590
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator !=(uint4x4 lhs, uint4x4 rhs)
		{
			return new bool4x4(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2, lhs.c3 != rhs.c3);
		}

		// Token: 0x0600248B RID: 9355 RVA: 0x000683E6 File Offset: 0x000665E6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator !=(uint4x4 lhs, uint rhs)
		{
			return new bool4x4(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs, lhs.c3 != rhs);
		}

		// Token: 0x0600248C RID: 9356 RVA: 0x0006841D File Offset: 0x0006661D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x4 operator !=(uint lhs, uint4x4 rhs)
		{
			return new bool4x4(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2, lhs != rhs.c3);
		}

		// Token: 0x0600248D RID: 9357 RVA: 0x00068454 File Offset: 0x00066654
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x4 operator ~(uint4x4 val)
		{
			return new uint4x4(~val.c0, ~val.c1, ~val.c2, ~val.c3);
		}

		// Token: 0x0600248E RID: 9358 RVA: 0x00068488 File Offset: 0x00066688
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x4 operator &(uint4x4 lhs, uint4x4 rhs)
		{
			return new uint4x4(lhs.c0 & rhs.c0, lhs.c1 & rhs.c1, lhs.c2 & rhs.c2, lhs.c3 & rhs.c3);
		}

		// Token: 0x0600248F RID: 9359 RVA: 0x000684DE File Offset: 0x000666DE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x4 operator &(uint4x4 lhs, uint rhs)
		{
			return new uint4x4(lhs.c0 & rhs, lhs.c1 & rhs, lhs.c2 & rhs, lhs.c3 & rhs);
		}

		// Token: 0x06002490 RID: 9360 RVA: 0x00068515 File Offset: 0x00066715
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x4 operator &(uint lhs, uint4x4 rhs)
		{
			return new uint4x4(lhs & rhs.c0, lhs & rhs.c1, lhs & rhs.c2, lhs & rhs.c3);
		}

		// Token: 0x06002491 RID: 9361 RVA: 0x0006854C File Offset: 0x0006674C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x4 operator |(uint4x4 lhs, uint4x4 rhs)
		{
			return new uint4x4(lhs.c0 | rhs.c0, lhs.c1 | rhs.c1, lhs.c2 | rhs.c2, lhs.c3 | rhs.c3);
		}

		// Token: 0x06002492 RID: 9362 RVA: 0x000685A2 File Offset: 0x000667A2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x4 operator |(uint4x4 lhs, uint rhs)
		{
			return new uint4x4(lhs.c0 | rhs, lhs.c1 | rhs, lhs.c2 | rhs, lhs.c3 | rhs);
		}

		// Token: 0x06002493 RID: 9363 RVA: 0x000685D9 File Offset: 0x000667D9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x4 operator |(uint lhs, uint4x4 rhs)
		{
			return new uint4x4(lhs | rhs.c0, lhs | rhs.c1, lhs | rhs.c2, lhs | rhs.c3);
		}

		// Token: 0x06002494 RID: 9364 RVA: 0x00068610 File Offset: 0x00066810
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x4 operator ^(uint4x4 lhs, uint4x4 rhs)
		{
			return new uint4x4(lhs.c0 ^ rhs.c0, lhs.c1 ^ rhs.c1, lhs.c2 ^ rhs.c2, lhs.c3 ^ rhs.c3);
		}

		// Token: 0x06002495 RID: 9365 RVA: 0x00068666 File Offset: 0x00066866
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x4 operator ^(uint4x4 lhs, uint rhs)
		{
			return new uint4x4(lhs.c0 ^ rhs, lhs.c1 ^ rhs, lhs.c2 ^ rhs, lhs.c3 ^ rhs);
		}

		// Token: 0x06002496 RID: 9366 RVA: 0x0006869D File Offset: 0x0006689D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x4 operator ^(uint lhs, uint4x4 rhs)
		{
			return new uint4x4(lhs ^ rhs.c0, lhs ^ rhs.c1, lhs ^ rhs.c2, lhs ^ rhs.c3);
		}

		// Token: 0x17000B89 RID: 2953
		public unsafe ref uint4 this[int index]
		{
			get
			{
				fixed (uint4x4* ptr = &this)
				{
					return ref *(uint4*)(ptr + (IntPtr)index * (IntPtr)sizeof(uint4) / (IntPtr)sizeof(uint4x4));
				}
			}
		}

		// Token: 0x06002498 RID: 9368 RVA: 0x000686F0 File Offset: 0x000668F0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(uint4x4 rhs)
		{
			return this.c0.Equals(rhs.c0) && this.c1.Equals(rhs.c1) && this.c2.Equals(rhs.c2) && this.c3.Equals(rhs.c3);
		}

		// Token: 0x06002499 RID: 9369 RVA: 0x0006874C File Offset: 0x0006694C
		public override bool Equals(object o)
		{
			if (o is uint4x4)
			{
				uint4x4 uint4x = (uint4x4)o;
				return this.Equals(uint4x);
			}
			return false;
		}

		// Token: 0x0600249A RID: 9370 RVA: 0x00068771 File Offset: 0x00066971
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x0600249B RID: 9371 RVA: 0x00068780 File Offset: 0x00066980
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("uint4x4({0}, {1}, {2}, {3},  {4}, {5}, {6}, {7},  {8}, {9}, {10}, {11},  {12}, {13}, {14}, {15})", new object[]
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

		// Token: 0x0600249C RID: 9372 RVA: 0x000688D8 File Offset: 0x00066AD8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("uint4x4({0}, {1}, {2}, {3},  {4}, {5}, {6}, {7},  {8}, {9}, {10}, {11},  {12}, {13}, {14}, {15})", new object[]
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

		// Token: 0x04000127 RID: 295
		public uint4 c0;

		// Token: 0x04000128 RID: 296
		public uint4 c1;

		// Token: 0x04000129 RID: 297
		public uint4 c2;

		// Token: 0x0400012A RID: 298
		public uint4 c3;

		// Token: 0x0400012B RID: 299
		public static readonly uint4x4 identity = new uint4x4(1U, 0U, 0U, 0U, 0U, 1U, 0U, 0U, 0U, 0U, 1U, 0U, 0U, 0U, 0U, 1U);

		// Token: 0x0400012C RID: 300
		public static readonly uint4x4 zero;
	}
}
