using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x02000048 RID: 72
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct uint3x4 : IEquatable<uint3x4>, IFormattable
	{
		// Token: 0x06002193 RID: 8595 RVA: 0x00060C34 File Offset: 0x0005EE34
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3x4(uint3 c0, uint3 c1, uint3 c2, uint3 c3)
		{
			this.c0 = c0;
			this.c1 = c1;
			this.c2 = c2;
			this.c3 = c3;
		}

		// Token: 0x06002194 RID: 8596 RVA: 0x00060C54 File Offset: 0x0005EE54
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3x4(uint m00, uint m01, uint m02, uint m03, uint m10, uint m11, uint m12, uint m13, uint m20, uint m21, uint m22, uint m23)
		{
			this.c0 = new uint3(m00, m10, m20);
			this.c1 = new uint3(m01, m11, m21);
			this.c2 = new uint3(m02, m12, m22);
			this.c3 = new uint3(m03, m13, m23);
		}

		// Token: 0x06002195 RID: 8597 RVA: 0x00060CA2 File Offset: 0x0005EEA2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3x4(uint v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
			this.c3 = v;
		}

		// Token: 0x06002196 RID: 8598 RVA: 0x00060CD4 File Offset: 0x0005EED4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3x4(bool v)
		{
			this.c0 = math.select(new uint3(0U), new uint3(1U), v);
			this.c1 = math.select(new uint3(0U), new uint3(1U), v);
			this.c2 = math.select(new uint3(0U), new uint3(1U), v);
			this.c3 = math.select(new uint3(0U), new uint3(1U), v);
		}

		// Token: 0x06002197 RID: 8599 RVA: 0x00060D44 File Offset: 0x0005EF44
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3x4(bool3x4 v)
		{
			this.c0 = math.select(new uint3(0U), new uint3(1U), v.c0);
			this.c1 = math.select(new uint3(0U), new uint3(1U), v.c1);
			this.c2 = math.select(new uint3(0U), new uint3(1U), v.c2);
			this.c3 = math.select(new uint3(0U), new uint3(1U), v.c3);
		}

		// Token: 0x06002198 RID: 8600 RVA: 0x00060DC5 File Offset: 0x0005EFC5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3x4(int v)
		{
			this.c0 = (uint3)v;
			this.c1 = (uint3)v;
			this.c2 = (uint3)v;
			this.c3 = (uint3)v;
		}

		// Token: 0x06002199 RID: 8601 RVA: 0x00060DF8 File Offset: 0x0005EFF8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3x4(int3x4 v)
		{
			this.c0 = (uint3)v.c0;
			this.c1 = (uint3)v.c1;
			this.c2 = (uint3)v.c2;
			this.c3 = (uint3)v.c3;
		}

		// Token: 0x0600219A RID: 8602 RVA: 0x00060E49 File Offset: 0x0005F049
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3x4(float v)
		{
			this.c0 = (uint3)v;
			this.c1 = (uint3)v;
			this.c2 = (uint3)v;
			this.c3 = (uint3)v;
		}

		// Token: 0x0600219B RID: 8603 RVA: 0x00060E7C File Offset: 0x0005F07C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3x4(float3x4 v)
		{
			this.c0 = (uint3)v.c0;
			this.c1 = (uint3)v.c1;
			this.c2 = (uint3)v.c2;
			this.c3 = (uint3)v.c3;
		}

		// Token: 0x0600219C RID: 8604 RVA: 0x00060ECD File Offset: 0x0005F0CD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3x4(double v)
		{
			this.c0 = (uint3)v;
			this.c1 = (uint3)v;
			this.c2 = (uint3)v;
			this.c3 = (uint3)v;
		}

		// Token: 0x0600219D RID: 8605 RVA: 0x00060F00 File Offset: 0x0005F100
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3x4(double3x4 v)
		{
			this.c0 = (uint3)v.c0;
			this.c1 = (uint3)v.c1;
			this.c2 = (uint3)v.c2;
			this.c3 = (uint3)v.c3;
		}

		// Token: 0x0600219E RID: 8606 RVA: 0x00060F51 File Offset: 0x0005F151
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator uint3x4(uint v)
		{
			return new uint3x4(v);
		}

		// Token: 0x0600219F RID: 8607 RVA: 0x00060F59 File Offset: 0x0005F159
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint3x4(bool v)
		{
			return new uint3x4(v);
		}

		// Token: 0x060021A0 RID: 8608 RVA: 0x00060F61 File Offset: 0x0005F161
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint3x4(bool3x4 v)
		{
			return new uint3x4(v);
		}

		// Token: 0x060021A1 RID: 8609 RVA: 0x00060F69 File Offset: 0x0005F169
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint3x4(int v)
		{
			return new uint3x4(v);
		}

		// Token: 0x060021A2 RID: 8610 RVA: 0x00060F71 File Offset: 0x0005F171
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint3x4(int3x4 v)
		{
			return new uint3x4(v);
		}

		// Token: 0x060021A3 RID: 8611 RVA: 0x00060F79 File Offset: 0x0005F179
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint3x4(float v)
		{
			return new uint3x4(v);
		}

		// Token: 0x060021A4 RID: 8612 RVA: 0x00060F81 File Offset: 0x0005F181
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint3x4(float3x4 v)
		{
			return new uint3x4(v);
		}

		// Token: 0x060021A5 RID: 8613 RVA: 0x00060F89 File Offset: 0x0005F189
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint3x4(double v)
		{
			return new uint3x4(v);
		}

		// Token: 0x060021A6 RID: 8614 RVA: 0x00060F91 File Offset: 0x0005F191
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint3x4(double3x4 v)
		{
			return new uint3x4(v);
		}

		// Token: 0x060021A7 RID: 8615 RVA: 0x00060F9C File Offset: 0x0005F19C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x4 operator *(uint3x4 lhs, uint3x4 rhs)
		{
			return new uint3x4(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1, lhs.c2 * rhs.c2, lhs.c3 * rhs.c3);
		}

		// Token: 0x060021A8 RID: 8616 RVA: 0x00060FF2 File Offset: 0x0005F1F2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x4 operator *(uint3x4 lhs, uint rhs)
		{
			return new uint3x4(lhs.c0 * rhs, lhs.c1 * rhs, lhs.c2 * rhs, lhs.c3 * rhs);
		}

		// Token: 0x060021A9 RID: 8617 RVA: 0x00061029 File Offset: 0x0005F229
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x4 operator *(uint lhs, uint3x4 rhs)
		{
			return new uint3x4(lhs * rhs.c0, lhs * rhs.c1, lhs * rhs.c2, lhs * rhs.c3);
		}

		// Token: 0x060021AA RID: 8618 RVA: 0x00061060 File Offset: 0x0005F260
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x4 operator +(uint3x4 lhs, uint3x4 rhs)
		{
			return new uint3x4(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1, lhs.c2 + rhs.c2, lhs.c3 + rhs.c3);
		}

		// Token: 0x060021AB RID: 8619 RVA: 0x000610B6 File Offset: 0x0005F2B6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x4 operator +(uint3x4 lhs, uint rhs)
		{
			return new uint3x4(lhs.c0 + rhs, lhs.c1 + rhs, lhs.c2 + rhs, lhs.c3 + rhs);
		}

		// Token: 0x060021AC RID: 8620 RVA: 0x000610ED File Offset: 0x0005F2ED
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x4 operator +(uint lhs, uint3x4 rhs)
		{
			return new uint3x4(lhs + rhs.c0, lhs + rhs.c1, lhs + rhs.c2, lhs + rhs.c3);
		}

		// Token: 0x060021AD RID: 8621 RVA: 0x00061124 File Offset: 0x0005F324
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x4 operator -(uint3x4 lhs, uint3x4 rhs)
		{
			return new uint3x4(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1, lhs.c2 - rhs.c2, lhs.c3 - rhs.c3);
		}

		// Token: 0x060021AE RID: 8622 RVA: 0x0006117A File Offset: 0x0005F37A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x4 operator -(uint3x4 lhs, uint rhs)
		{
			return new uint3x4(lhs.c0 - rhs, lhs.c1 - rhs, lhs.c2 - rhs, lhs.c3 - rhs);
		}

		// Token: 0x060021AF RID: 8623 RVA: 0x000611B1 File Offset: 0x0005F3B1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x4 operator -(uint lhs, uint3x4 rhs)
		{
			return new uint3x4(lhs - rhs.c0, lhs - rhs.c1, lhs - rhs.c2, lhs - rhs.c3);
		}

		// Token: 0x060021B0 RID: 8624 RVA: 0x000611E8 File Offset: 0x0005F3E8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x4 operator /(uint3x4 lhs, uint3x4 rhs)
		{
			return new uint3x4(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1, lhs.c2 / rhs.c2, lhs.c3 / rhs.c3);
		}

		// Token: 0x060021B1 RID: 8625 RVA: 0x0006123E File Offset: 0x0005F43E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x4 operator /(uint3x4 lhs, uint rhs)
		{
			return new uint3x4(lhs.c0 / rhs, lhs.c1 / rhs, lhs.c2 / rhs, lhs.c3 / rhs);
		}

		// Token: 0x060021B2 RID: 8626 RVA: 0x00061275 File Offset: 0x0005F475
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x4 operator /(uint lhs, uint3x4 rhs)
		{
			return new uint3x4(lhs / rhs.c0, lhs / rhs.c1, lhs / rhs.c2, lhs / rhs.c3);
		}

		// Token: 0x060021B3 RID: 8627 RVA: 0x000612AC File Offset: 0x0005F4AC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x4 operator %(uint3x4 lhs, uint3x4 rhs)
		{
			return new uint3x4(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1, lhs.c2 % rhs.c2, lhs.c3 % rhs.c3);
		}

		// Token: 0x060021B4 RID: 8628 RVA: 0x00061302 File Offset: 0x0005F502
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x4 operator %(uint3x4 lhs, uint rhs)
		{
			return new uint3x4(lhs.c0 % rhs, lhs.c1 % rhs, lhs.c2 % rhs, lhs.c3 % rhs);
		}

		// Token: 0x060021B5 RID: 8629 RVA: 0x00061339 File Offset: 0x0005F539
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x4 operator %(uint lhs, uint3x4 rhs)
		{
			return new uint3x4(lhs % rhs.c0, lhs % rhs.c1, lhs % rhs.c2, lhs % rhs.c3);
		}

		// Token: 0x060021B6 RID: 8630 RVA: 0x00061370 File Offset: 0x0005F570
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x4 operator ++(uint3x4 val)
		{
			uint3 @uint = uint3.op_Increment(val.c0);
			val.c0 = @uint;
			uint3 uint2 = @uint;
			@uint = uint3.op_Increment(val.c1);
			val.c1 = @uint;
			uint3 uint3 = @uint;
			@uint = uint3.op_Increment(val.c2);
			val.c2 = @uint;
			uint3 uint4 = @uint;
			@uint = uint3.op_Increment(val.c3);
			val.c3 = @uint;
			return new uint3x4(uint2, uint3, uint4, @uint);
		}

		// Token: 0x060021B7 RID: 8631 RVA: 0x000613EC File Offset: 0x0005F5EC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x4 operator --(uint3x4 val)
		{
			uint3 @uint = uint3.op_Decrement(val.c0);
			val.c0 = @uint;
			uint3 uint2 = @uint;
			@uint = uint3.op_Decrement(val.c1);
			val.c1 = @uint;
			uint3 uint3 = @uint;
			@uint = uint3.op_Decrement(val.c2);
			val.c2 = @uint;
			uint3 uint4 = @uint;
			@uint = uint3.op_Decrement(val.c3);
			val.c3 = @uint;
			return new uint3x4(uint2, uint3, uint4, @uint);
		}

		// Token: 0x060021B8 RID: 8632 RVA: 0x00061468 File Offset: 0x0005F668
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator <(uint3x4 lhs, uint3x4 rhs)
		{
			return new bool3x4(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1, lhs.c2 < rhs.c2, lhs.c3 < rhs.c3);
		}

		// Token: 0x060021B9 RID: 8633 RVA: 0x000614BE File Offset: 0x0005F6BE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator <(uint3x4 lhs, uint rhs)
		{
			return new bool3x4(lhs.c0 < rhs, lhs.c1 < rhs, lhs.c2 < rhs, lhs.c3 < rhs);
		}

		// Token: 0x060021BA RID: 8634 RVA: 0x000614F5 File Offset: 0x0005F6F5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator <(uint lhs, uint3x4 rhs)
		{
			return new bool3x4(lhs < rhs.c0, lhs < rhs.c1, lhs < rhs.c2, lhs < rhs.c3);
		}

		// Token: 0x060021BB RID: 8635 RVA: 0x0006152C File Offset: 0x0005F72C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator <=(uint3x4 lhs, uint3x4 rhs)
		{
			return new bool3x4(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1, lhs.c2 <= rhs.c2, lhs.c3 <= rhs.c3);
		}

		// Token: 0x060021BC RID: 8636 RVA: 0x00061582 File Offset: 0x0005F782
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator <=(uint3x4 lhs, uint rhs)
		{
			return new bool3x4(lhs.c0 <= rhs, lhs.c1 <= rhs, lhs.c2 <= rhs, lhs.c3 <= rhs);
		}

		// Token: 0x060021BD RID: 8637 RVA: 0x000615B9 File Offset: 0x0005F7B9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator <=(uint lhs, uint3x4 rhs)
		{
			return new bool3x4(lhs <= rhs.c0, lhs <= rhs.c1, lhs <= rhs.c2, lhs <= rhs.c3);
		}

		// Token: 0x060021BE RID: 8638 RVA: 0x000615F0 File Offset: 0x0005F7F0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator >(uint3x4 lhs, uint3x4 rhs)
		{
			return new bool3x4(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1, lhs.c2 > rhs.c2, lhs.c3 > rhs.c3);
		}

		// Token: 0x060021BF RID: 8639 RVA: 0x00061646 File Offset: 0x0005F846
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator >(uint3x4 lhs, uint rhs)
		{
			return new bool3x4(lhs.c0 > rhs, lhs.c1 > rhs, lhs.c2 > rhs, lhs.c3 > rhs);
		}

		// Token: 0x060021C0 RID: 8640 RVA: 0x0006167D File Offset: 0x0005F87D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator >(uint lhs, uint3x4 rhs)
		{
			return new bool3x4(lhs > rhs.c0, lhs > rhs.c1, lhs > rhs.c2, lhs > rhs.c3);
		}

		// Token: 0x060021C1 RID: 8641 RVA: 0x000616B4 File Offset: 0x0005F8B4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator >=(uint3x4 lhs, uint3x4 rhs)
		{
			return new bool3x4(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1, lhs.c2 >= rhs.c2, lhs.c3 >= rhs.c3);
		}

		// Token: 0x060021C2 RID: 8642 RVA: 0x0006170A File Offset: 0x0005F90A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator >=(uint3x4 lhs, uint rhs)
		{
			return new bool3x4(lhs.c0 >= rhs, lhs.c1 >= rhs, lhs.c2 >= rhs, lhs.c3 >= rhs);
		}

		// Token: 0x060021C3 RID: 8643 RVA: 0x00061741 File Offset: 0x0005F941
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator >=(uint lhs, uint3x4 rhs)
		{
			return new bool3x4(lhs >= rhs.c0, lhs >= rhs.c1, lhs >= rhs.c2, lhs >= rhs.c3);
		}

		// Token: 0x060021C4 RID: 8644 RVA: 0x00061778 File Offset: 0x0005F978
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x4 operator -(uint3x4 val)
		{
			return new uint3x4(-val.c0, -val.c1, -val.c2, -val.c3);
		}

		// Token: 0x060021C5 RID: 8645 RVA: 0x000617AB File Offset: 0x0005F9AB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x4 operator +(uint3x4 val)
		{
			return new uint3x4(+val.c0, +val.c1, +val.c2, +val.c3);
		}

		// Token: 0x060021C6 RID: 8646 RVA: 0x000617DE File Offset: 0x0005F9DE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x4 operator <<(uint3x4 x, int n)
		{
			return new uint3x4(x.c0 << n, x.c1 << n, x.c2 << n, x.c3 << n);
		}

		// Token: 0x060021C7 RID: 8647 RVA: 0x00061815 File Offset: 0x0005FA15
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x4 operator >>(uint3x4 x, int n)
		{
			return new uint3x4(x.c0 >> n, x.c1 >> n, x.c2 >> n, x.c3 >> n);
		}

		// Token: 0x060021C8 RID: 8648 RVA: 0x0006184C File Offset: 0x0005FA4C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator ==(uint3x4 lhs, uint3x4 rhs)
		{
			return new bool3x4(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2, lhs.c3 == rhs.c3);
		}

		// Token: 0x060021C9 RID: 8649 RVA: 0x000618A2 File Offset: 0x0005FAA2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator ==(uint3x4 lhs, uint rhs)
		{
			return new bool3x4(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs, lhs.c3 == rhs);
		}

		// Token: 0x060021CA RID: 8650 RVA: 0x000618D9 File Offset: 0x0005FAD9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator ==(uint lhs, uint3x4 rhs)
		{
			return new bool3x4(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2, lhs == rhs.c3);
		}

		// Token: 0x060021CB RID: 8651 RVA: 0x00061910 File Offset: 0x0005FB10
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator !=(uint3x4 lhs, uint3x4 rhs)
		{
			return new bool3x4(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2, lhs.c3 != rhs.c3);
		}

		// Token: 0x060021CC RID: 8652 RVA: 0x00061966 File Offset: 0x0005FB66
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator !=(uint3x4 lhs, uint rhs)
		{
			return new bool3x4(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs, lhs.c3 != rhs);
		}

		// Token: 0x060021CD RID: 8653 RVA: 0x0006199D File Offset: 0x0005FB9D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x4 operator !=(uint lhs, uint3x4 rhs)
		{
			return new bool3x4(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2, lhs != rhs.c3);
		}

		// Token: 0x060021CE RID: 8654 RVA: 0x000619D4 File Offset: 0x0005FBD4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x4 operator ~(uint3x4 val)
		{
			return new uint3x4(~val.c0, ~val.c1, ~val.c2, ~val.c3);
		}

		// Token: 0x060021CF RID: 8655 RVA: 0x00061A08 File Offset: 0x0005FC08
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x4 operator &(uint3x4 lhs, uint3x4 rhs)
		{
			return new uint3x4(lhs.c0 & rhs.c0, lhs.c1 & rhs.c1, lhs.c2 & rhs.c2, lhs.c3 & rhs.c3);
		}

		// Token: 0x060021D0 RID: 8656 RVA: 0x00061A5E File Offset: 0x0005FC5E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x4 operator &(uint3x4 lhs, uint rhs)
		{
			return new uint3x4(lhs.c0 & rhs, lhs.c1 & rhs, lhs.c2 & rhs, lhs.c3 & rhs);
		}

		// Token: 0x060021D1 RID: 8657 RVA: 0x00061A95 File Offset: 0x0005FC95
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x4 operator &(uint lhs, uint3x4 rhs)
		{
			return new uint3x4(lhs & rhs.c0, lhs & rhs.c1, lhs & rhs.c2, lhs & rhs.c3);
		}

		// Token: 0x060021D2 RID: 8658 RVA: 0x00061ACC File Offset: 0x0005FCCC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x4 operator |(uint3x4 lhs, uint3x4 rhs)
		{
			return new uint3x4(lhs.c0 | rhs.c0, lhs.c1 | rhs.c1, lhs.c2 | rhs.c2, lhs.c3 | rhs.c3);
		}

		// Token: 0x060021D3 RID: 8659 RVA: 0x00061B22 File Offset: 0x0005FD22
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x4 operator |(uint3x4 lhs, uint rhs)
		{
			return new uint3x4(lhs.c0 | rhs, lhs.c1 | rhs, lhs.c2 | rhs, lhs.c3 | rhs);
		}

		// Token: 0x060021D4 RID: 8660 RVA: 0x00061B59 File Offset: 0x0005FD59
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x4 operator |(uint lhs, uint3x4 rhs)
		{
			return new uint3x4(lhs | rhs.c0, lhs | rhs.c1, lhs | rhs.c2, lhs | rhs.c3);
		}

		// Token: 0x060021D5 RID: 8661 RVA: 0x00061B90 File Offset: 0x0005FD90
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x4 operator ^(uint3x4 lhs, uint3x4 rhs)
		{
			return new uint3x4(lhs.c0 ^ rhs.c0, lhs.c1 ^ rhs.c1, lhs.c2 ^ rhs.c2, lhs.c3 ^ rhs.c3);
		}

		// Token: 0x060021D6 RID: 8662 RVA: 0x00061BE6 File Offset: 0x0005FDE6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x4 operator ^(uint3x4 lhs, uint rhs)
		{
			return new uint3x4(lhs.c0 ^ rhs, lhs.c1 ^ rhs, lhs.c2 ^ rhs, lhs.c3 ^ rhs);
		}

		// Token: 0x060021D7 RID: 8663 RVA: 0x00061C1D File Offset: 0x0005FE1D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x4 operator ^(uint lhs, uint3x4 rhs)
		{
			return new uint3x4(lhs ^ rhs.c0, lhs ^ rhs.c1, lhs ^ rhs.c2, lhs ^ rhs.c3);
		}

		// Token: 0x17000A35 RID: 2613
		public unsafe ref uint3 this[int index]
		{
			get
			{
				fixed (uint3x4* ptr = &this)
				{
					return ref *(uint3*)(ptr + (IntPtr)index * (IntPtr)sizeof(uint3) / (IntPtr)sizeof(uint3x4));
				}
			}
		}

		// Token: 0x060021D9 RID: 8665 RVA: 0x00061C70 File Offset: 0x0005FE70
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(uint3x4 rhs)
		{
			return this.c0.Equals(rhs.c0) && this.c1.Equals(rhs.c1) && this.c2.Equals(rhs.c2) && this.c3.Equals(rhs.c3);
		}

		// Token: 0x060021DA RID: 8666 RVA: 0x00061CCC File Offset: 0x0005FECC
		public override bool Equals(object o)
		{
			if (o is uint3x4)
			{
				uint3x4 uint3x = (uint3x4)o;
				return this.Equals(uint3x);
			}
			return false;
		}

		// Token: 0x060021DB RID: 8667 RVA: 0x00061CF1 File Offset: 0x0005FEF1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x060021DC RID: 8668 RVA: 0x00061D00 File Offset: 0x0005FF00
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("uint3x4({0}, {1}, {2}, {3},  {4}, {5}, {6}, {7},  {8}, {9}, {10}, {11})", new object[]
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
				this.c3.z
			});
		}

		// Token: 0x060021DD RID: 8669 RVA: 0x00061E08 File Offset: 0x00060008
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("uint3x4({0}, {1}, {2}, {3},  {4}, {5}, {6}, {7},  {8}, {9}, {10}, {11})", new object[]
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
				this.c3.z.ToString(format, formatProvider)
			});
		}

		// Token: 0x04000116 RID: 278
		public uint3 c0;

		// Token: 0x04000117 RID: 279
		public uint3 c1;

		// Token: 0x04000118 RID: 280
		public uint3 c2;

		// Token: 0x04000119 RID: 281
		public uint3 c3;

		// Token: 0x0400011A RID: 282
		public static readonly uint3x4 zero;
	}
}
