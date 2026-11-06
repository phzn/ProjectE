using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x0200004B RID: 75
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct uint4x3 : IEquatable<uint4x3>, IFormattable
	{
		// Token: 0x06002407 RID: 9223 RVA: 0x000667FD File Offset: 0x000649FD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint4x3(uint4 c0, uint4 c1, uint4 c2)
		{
			this.c0 = c0;
			this.c1 = c1;
			this.c2 = c2;
		}

		// Token: 0x06002408 RID: 9224 RVA: 0x00066814 File Offset: 0x00064A14
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint4x3(uint m00, uint m01, uint m02, uint m10, uint m11, uint m12, uint m20, uint m21, uint m22, uint m30, uint m31, uint m32)
		{
			this.c0 = new uint4(m00, m10, m20, m30);
			this.c1 = new uint4(m01, m11, m21, m31);
			this.c2 = new uint4(m02, m12, m22, m32);
		}

		// Token: 0x06002409 RID: 9225 RVA: 0x0006684C File Offset: 0x00064A4C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint4x3(uint v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
		}

		// Token: 0x0600240A RID: 9226 RVA: 0x00066874 File Offset: 0x00064A74
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint4x3(bool v)
		{
			this.c0 = math.select(new uint4(0U), new uint4(1U), v);
			this.c1 = math.select(new uint4(0U), new uint4(1U), v);
			this.c2 = math.select(new uint4(0U), new uint4(1U), v);
		}

		// Token: 0x0600240B RID: 9227 RVA: 0x000668CC File Offset: 0x00064ACC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint4x3(bool4x3 v)
		{
			this.c0 = math.select(new uint4(0U), new uint4(1U), v.c0);
			this.c1 = math.select(new uint4(0U), new uint4(1U), v.c1);
			this.c2 = math.select(new uint4(0U), new uint4(1U), v.c2);
		}

		// Token: 0x0600240C RID: 9228 RVA: 0x00066930 File Offset: 0x00064B30
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint4x3(int v)
		{
			this.c0 = (uint4)v;
			this.c1 = (uint4)v;
			this.c2 = (uint4)v;
		}

		// Token: 0x0600240D RID: 9229 RVA: 0x00066956 File Offset: 0x00064B56
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint4x3(int4x3 v)
		{
			this.c0 = (uint4)v.c0;
			this.c1 = (uint4)v.c1;
			this.c2 = (uint4)v.c2;
		}

		// Token: 0x0600240E RID: 9230 RVA: 0x0006698B File Offset: 0x00064B8B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint4x3(float v)
		{
			this.c0 = (uint4)v;
			this.c1 = (uint4)v;
			this.c2 = (uint4)v;
		}

		// Token: 0x0600240F RID: 9231 RVA: 0x000669B1 File Offset: 0x00064BB1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint4x3(float4x3 v)
		{
			this.c0 = (uint4)v.c0;
			this.c1 = (uint4)v.c1;
			this.c2 = (uint4)v.c2;
		}

		// Token: 0x06002410 RID: 9232 RVA: 0x000669E6 File Offset: 0x00064BE6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint4x3(double v)
		{
			this.c0 = (uint4)v;
			this.c1 = (uint4)v;
			this.c2 = (uint4)v;
		}

		// Token: 0x06002411 RID: 9233 RVA: 0x00066A0C File Offset: 0x00064C0C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint4x3(double4x3 v)
		{
			this.c0 = (uint4)v.c0;
			this.c1 = (uint4)v.c1;
			this.c2 = (uint4)v.c2;
		}

		// Token: 0x06002412 RID: 9234 RVA: 0x00066A41 File Offset: 0x00064C41
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator uint4x3(uint v)
		{
			return new uint4x3(v);
		}

		// Token: 0x06002413 RID: 9235 RVA: 0x00066A49 File Offset: 0x00064C49
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint4x3(bool v)
		{
			return new uint4x3(v);
		}

		// Token: 0x06002414 RID: 9236 RVA: 0x00066A51 File Offset: 0x00064C51
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint4x3(bool4x3 v)
		{
			return new uint4x3(v);
		}

		// Token: 0x06002415 RID: 9237 RVA: 0x00066A59 File Offset: 0x00064C59
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint4x3(int v)
		{
			return new uint4x3(v);
		}

		// Token: 0x06002416 RID: 9238 RVA: 0x00066A61 File Offset: 0x00064C61
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint4x3(int4x3 v)
		{
			return new uint4x3(v);
		}

		// Token: 0x06002417 RID: 9239 RVA: 0x00066A69 File Offset: 0x00064C69
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint4x3(float v)
		{
			return new uint4x3(v);
		}

		// Token: 0x06002418 RID: 9240 RVA: 0x00066A71 File Offset: 0x00064C71
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint4x3(float4x3 v)
		{
			return new uint4x3(v);
		}

		// Token: 0x06002419 RID: 9241 RVA: 0x00066A79 File Offset: 0x00064C79
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint4x3(double v)
		{
			return new uint4x3(v);
		}

		// Token: 0x0600241A RID: 9242 RVA: 0x00066A81 File Offset: 0x00064C81
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint4x3(double4x3 v)
		{
			return new uint4x3(v);
		}

		// Token: 0x0600241B RID: 9243 RVA: 0x00066A89 File Offset: 0x00064C89
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x3 operator *(uint4x3 lhs, uint4x3 rhs)
		{
			return new uint4x3(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1, lhs.c2 * rhs.c2);
		}

		// Token: 0x0600241C RID: 9244 RVA: 0x00066AC3 File Offset: 0x00064CC3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x3 operator *(uint4x3 lhs, uint rhs)
		{
			return new uint4x3(lhs.c0 * rhs, lhs.c1 * rhs, lhs.c2 * rhs);
		}

		// Token: 0x0600241D RID: 9245 RVA: 0x00066AEE File Offset: 0x00064CEE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x3 operator *(uint lhs, uint4x3 rhs)
		{
			return new uint4x3(lhs * rhs.c0, lhs * rhs.c1, lhs * rhs.c2);
		}

		// Token: 0x0600241E RID: 9246 RVA: 0x00066B19 File Offset: 0x00064D19
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x3 operator +(uint4x3 lhs, uint4x3 rhs)
		{
			return new uint4x3(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1, lhs.c2 + rhs.c2);
		}

		// Token: 0x0600241F RID: 9247 RVA: 0x00066B53 File Offset: 0x00064D53
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x3 operator +(uint4x3 lhs, uint rhs)
		{
			return new uint4x3(lhs.c0 + rhs, lhs.c1 + rhs, lhs.c2 + rhs);
		}

		// Token: 0x06002420 RID: 9248 RVA: 0x00066B7E File Offset: 0x00064D7E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x3 operator +(uint lhs, uint4x3 rhs)
		{
			return new uint4x3(lhs + rhs.c0, lhs + rhs.c1, lhs + rhs.c2);
		}

		// Token: 0x06002421 RID: 9249 RVA: 0x00066BA9 File Offset: 0x00064DA9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x3 operator -(uint4x3 lhs, uint4x3 rhs)
		{
			return new uint4x3(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1, lhs.c2 - rhs.c2);
		}

		// Token: 0x06002422 RID: 9250 RVA: 0x00066BE3 File Offset: 0x00064DE3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x3 operator -(uint4x3 lhs, uint rhs)
		{
			return new uint4x3(lhs.c0 - rhs, lhs.c1 - rhs, lhs.c2 - rhs);
		}

		// Token: 0x06002423 RID: 9251 RVA: 0x00066C0E File Offset: 0x00064E0E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x3 operator -(uint lhs, uint4x3 rhs)
		{
			return new uint4x3(lhs - rhs.c0, lhs - rhs.c1, lhs - rhs.c2);
		}

		// Token: 0x06002424 RID: 9252 RVA: 0x00066C39 File Offset: 0x00064E39
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x3 operator /(uint4x3 lhs, uint4x3 rhs)
		{
			return new uint4x3(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1, lhs.c2 / rhs.c2);
		}

		// Token: 0x06002425 RID: 9253 RVA: 0x00066C73 File Offset: 0x00064E73
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x3 operator /(uint4x3 lhs, uint rhs)
		{
			return new uint4x3(lhs.c0 / rhs, lhs.c1 / rhs, lhs.c2 / rhs);
		}

		// Token: 0x06002426 RID: 9254 RVA: 0x00066C9E File Offset: 0x00064E9E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x3 operator /(uint lhs, uint4x3 rhs)
		{
			return new uint4x3(lhs / rhs.c0, lhs / rhs.c1, lhs / rhs.c2);
		}

		// Token: 0x06002427 RID: 9255 RVA: 0x00066CC9 File Offset: 0x00064EC9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x3 operator %(uint4x3 lhs, uint4x3 rhs)
		{
			return new uint4x3(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1, lhs.c2 % rhs.c2);
		}

		// Token: 0x06002428 RID: 9256 RVA: 0x00066D03 File Offset: 0x00064F03
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x3 operator %(uint4x3 lhs, uint rhs)
		{
			return new uint4x3(lhs.c0 % rhs, lhs.c1 % rhs, lhs.c2 % rhs);
		}

		// Token: 0x06002429 RID: 9257 RVA: 0x00066D2E File Offset: 0x00064F2E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x3 operator %(uint lhs, uint4x3 rhs)
		{
			return new uint4x3(lhs % rhs.c0, lhs % rhs.c1, lhs % rhs.c2);
		}

		// Token: 0x0600242A RID: 9258 RVA: 0x00066D5C File Offset: 0x00064F5C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x3 operator ++(uint4x3 val)
		{
			uint4 @uint = uint4.op_Increment(val.c0);
			val.c0 = @uint;
			uint4 uint2 = @uint;
			@uint = uint4.op_Increment(val.c1);
			val.c1 = @uint;
			uint4 uint3 = @uint;
			@uint = uint4.op_Increment(val.c2);
			val.c2 = @uint;
			return new uint4x3(uint2, uint3, @uint);
		}

		// Token: 0x0600242B RID: 9259 RVA: 0x00066DBC File Offset: 0x00064FBC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x3 operator --(uint4x3 val)
		{
			uint4 @uint = uint4.op_Decrement(val.c0);
			val.c0 = @uint;
			uint4 uint2 = @uint;
			@uint = uint4.op_Decrement(val.c1);
			val.c1 = @uint;
			uint4 uint3 = @uint;
			@uint = uint4.op_Decrement(val.c2);
			val.c2 = @uint;
			return new uint4x3(uint2, uint3, @uint);
		}

		// Token: 0x0600242C RID: 9260 RVA: 0x00066E1C File Offset: 0x0006501C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator <(uint4x3 lhs, uint4x3 rhs)
		{
			return new bool4x3(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1, lhs.c2 < rhs.c2);
		}

		// Token: 0x0600242D RID: 9261 RVA: 0x00066E56 File Offset: 0x00065056
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator <(uint4x3 lhs, uint rhs)
		{
			return new bool4x3(lhs.c0 < rhs, lhs.c1 < rhs, lhs.c2 < rhs);
		}

		// Token: 0x0600242E RID: 9262 RVA: 0x00066E81 File Offset: 0x00065081
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator <(uint lhs, uint4x3 rhs)
		{
			return new bool4x3(lhs < rhs.c0, lhs < rhs.c1, lhs < rhs.c2);
		}

		// Token: 0x0600242F RID: 9263 RVA: 0x00066EAC File Offset: 0x000650AC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator <=(uint4x3 lhs, uint4x3 rhs)
		{
			return new bool4x3(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1, lhs.c2 <= rhs.c2);
		}

		// Token: 0x06002430 RID: 9264 RVA: 0x00066EE6 File Offset: 0x000650E6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator <=(uint4x3 lhs, uint rhs)
		{
			return new bool4x3(lhs.c0 <= rhs, lhs.c1 <= rhs, lhs.c2 <= rhs);
		}

		// Token: 0x06002431 RID: 9265 RVA: 0x00066F11 File Offset: 0x00065111
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator <=(uint lhs, uint4x3 rhs)
		{
			return new bool4x3(lhs <= rhs.c0, lhs <= rhs.c1, lhs <= rhs.c2);
		}

		// Token: 0x06002432 RID: 9266 RVA: 0x00066F3C File Offset: 0x0006513C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator >(uint4x3 lhs, uint4x3 rhs)
		{
			return new bool4x3(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1, lhs.c2 > rhs.c2);
		}

		// Token: 0x06002433 RID: 9267 RVA: 0x00066F76 File Offset: 0x00065176
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator >(uint4x3 lhs, uint rhs)
		{
			return new bool4x3(lhs.c0 > rhs, lhs.c1 > rhs, lhs.c2 > rhs);
		}

		// Token: 0x06002434 RID: 9268 RVA: 0x00066FA1 File Offset: 0x000651A1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator >(uint lhs, uint4x3 rhs)
		{
			return new bool4x3(lhs > rhs.c0, lhs > rhs.c1, lhs > rhs.c2);
		}

		// Token: 0x06002435 RID: 9269 RVA: 0x00066FCC File Offset: 0x000651CC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator >=(uint4x3 lhs, uint4x3 rhs)
		{
			return new bool4x3(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1, lhs.c2 >= rhs.c2);
		}

		// Token: 0x06002436 RID: 9270 RVA: 0x00067006 File Offset: 0x00065206
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator >=(uint4x3 lhs, uint rhs)
		{
			return new bool4x3(lhs.c0 >= rhs, lhs.c1 >= rhs, lhs.c2 >= rhs);
		}

		// Token: 0x06002437 RID: 9271 RVA: 0x00067031 File Offset: 0x00065231
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator >=(uint lhs, uint4x3 rhs)
		{
			return new bool4x3(lhs >= rhs.c0, lhs >= rhs.c1, lhs >= rhs.c2);
		}

		// Token: 0x06002438 RID: 9272 RVA: 0x0006705C File Offset: 0x0006525C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x3 operator -(uint4x3 val)
		{
			return new uint4x3(-val.c0, -val.c1, -val.c2);
		}

		// Token: 0x06002439 RID: 9273 RVA: 0x00067084 File Offset: 0x00065284
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x3 operator +(uint4x3 val)
		{
			return new uint4x3(+val.c0, +val.c1, +val.c2);
		}

		// Token: 0x0600243A RID: 9274 RVA: 0x000670AC File Offset: 0x000652AC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x3 operator <<(uint4x3 x, int n)
		{
			return new uint4x3(x.c0 << n, x.c1 << n, x.c2 << n);
		}

		// Token: 0x0600243B RID: 9275 RVA: 0x000670D7 File Offset: 0x000652D7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x3 operator >>(uint4x3 x, int n)
		{
			return new uint4x3(x.c0 >> n, x.c1 >> n, x.c2 >> n);
		}

		// Token: 0x0600243C RID: 9276 RVA: 0x00067102 File Offset: 0x00065302
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator ==(uint4x3 lhs, uint4x3 rhs)
		{
			return new bool4x3(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2);
		}

		// Token: 0x0600243D RID: 9277 RVA: 0x0006713C File Offset: 0x0006533C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator ==(uint4x3 lhs, uint rhs)
		{
			return new bool4x3(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs);
		}

		// Token: 0x0600243E RID: 9278 RVA: 0x00067167 File Offset: 0x00065367
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator ==(uint lhs, uint4x3 rhs)
		{
			return new bool4x3(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2);
		}

		// Token: 0x0600243F RID: 9279 RVA: 0x00067192 File Offset: 0x00065392
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator !=(uint4x3 lhs, uint4x3 rhs)
		{
			return new bool4x3(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2);
		}

		// Token: 0x06002440 RID: 9280 RVA: 0x000671CC File Offset: 0x000653CC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator !=(uint4x3 lhs, uint rhs)
		{
			return new bool4x3(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs);
		}

		// Token: 0x06002441 RID: 9281 RVA: 0x000671F7 File Offset: 0x000653F7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool4x3 operator !=(uint lhs, uint4x3 rhs)
		{
			return new bool4x3(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2);
		}

		// Token: 0x06002442 RID: 9282 RVA: 0x00067222 File Offset: 0x00065422
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x3 operator ~(uint4x3 val)
		{
			return new uint4x3(~val.c0, ~val.c1, ~val.c2);
		}

		// Token: 0x06002443 RID: 9283 RVA: 0x0006724A File Offset: 0x0006544A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x3 operator &(uint4x3 lhs, uint4x3 rhs)
		{
			return new uint4x3(lhs.c0 & rhs.c0, lhs.c1 & rhs.c1, lhs.c2 & rhs.c2);
		}

		// Token: 0x06002444 RID: 9284 RVA: 0x00067284 File Offset: 0x00065484
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x3 operator &(uint4x3 lhs, uint rhs)
		{
			return new uint4x3(lhs.c0 & rhs, lhs.c1 & rhs, lhs.c2 & rhs);
		}

		// Token: 0x06002445 RID: 9285 RVA: 0x000672AF File Offset: 0x000654AF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x3 operator &(uint lhs, uint4x3 rhs)
		{
			return new uint4x3(lhs & rhs.c0, lhs & rhs.c1, lhs & rhs.c2);
		}

		// Token: 0x06002446 RID: 9286 RVA: 0x000672DA File Offset: 0x000654DA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x3 operator |(uint4x3 lhs, uint4x3 rhs)
		{
			return new uint4x3(lhs.c0 | rhs.c0, lhs.c1 | rhs.c1, lhs.c2 | rhs.c2);
		}

		// Token: 0x06002447 RID: 9287 RVA: 0x00067314 File Offset: 0x00065514
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x3 operator |(uint4x3 lhs, uint rhs)
		{
			return new uint4x3(lhs.c0 | rhs, lhs.c1 | rhs, lhs.c2 | rhs);
		}

		// Token: 0x06002448 RID: 9288 RVA: 0x0006733F File Offset: 0x0006553F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x3 operator |(uint lhs, uint4x3 rhs)
		{
			return new uint4x3(lhs | rhs.c0, lhs | rhs.c1, lhs | rhs.c2);
		}

		// Token: 0x06002449 RID: 9289 RVA: 0x0006736A File Offset: 0x0006556A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x3 operator ^(uint4x3 lhs, uint4x3 rhs)
		{
			return new uint4x3(lhs.c0 ^ rhs.c0, lhs.c1 ^ rhs.c1, lhs.c2 ^ rhs.c2);
		}

		// Token: 0x0600244A RID: 9290 RVA: 0x000673A4 File Offset: 0x000655A4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x3 operator ^(uint4x3 lhs, uint rhs)
		{
			return new uint4x3(lhs.c0 ^ rhs, lhs.c1 ^ rhs, lhs.c2 ^ rhs);
		}

		// Token: 0x0600244B RID: 9291 RVA: 0x000673CF File Offset: 0x000655CF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint4x3 operator ^(uint lhs, uint4x3 rhs)
		{
			return new uint4x3(lhs ^ rhs.c0, lhs ^ rhs.c1, lhs ^ rhs.c2);
		}

		// Token: 0x17000B88 RID: 2952
		public unsafe ref uint4 this[int index]
		{
			get
			{
				fixed (uint4x3* ptr = &this)
				{
					return ref *(uint4*)(ptr + (IntPtr)index * (IntPtr)sizeof(uint4) / (IntPtr)sizeof(uint4x3));
				}
			}
		}

		// Token: 0x0600244D RID: 9293 RVA: 0x00067417 File Offset: 0x00065617
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(uint4x3 rhs)
		{
			return this.c0.Equals(rhs.c0) && this.c1.Equals(rhs.c1) && this.c2.Equals(rhs.c2);
		}

		// Token: 0x0600244E RID: 9294 RVA: 0x00067454 File Offset: 0x00065654
		public override bool Equals(object o)
		{
			if (o is uint4x3)
			{
				uint4x3 uint4x = (uint4x3)o;
				return this.Equals(uint4x);
			}
			return false;
		}

		// Token: 0x0600244F RID: 9295 RVA: 0x00067479 File Offset: 0x00065679
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x06002450 RID: 9296 RVA: 0x00067488 File Offset: 0x00065688
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("uint4x3({0}, {1}, {2},  {3}, {4}, {5},  {6}, {7}, {8},  {9}, {10}, {11})", new object[]
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

		// Token: 0x06002451 RID: 9297 RVA: 0x00067590 File Offset: 0x00065790
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("uint4x3({0}, {1}, {2},  {3}, {4}, {5},  {6}, {7}, {8},  {9}, {10}, {11})", new object[]
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

		// Token: 0x04000123 RID: 291
		public uint4 c0;

		// Token: 0x04000124 RID: 292
		public uint4 c1;

		// Token: 0x04000125 RID: 293
		public uint4 c2;

		// Token: 0x04000126 RID: 294
		public static readonly uint4x3 zero;
	}
}
