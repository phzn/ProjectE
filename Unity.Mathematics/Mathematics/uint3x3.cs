using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x02000047 RID: 71
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct uint3x3 : IEquatable<uint3x3>, IFormattable
	{
		// Token: 0x06002147 RID: 8519 RVA: 0x0005FDE7 File Offset: 0x0005DFE7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3x3(uint3 c0, uint3 c1, uint3 c2)
		{
			this.c0 = c0;
			this.c1 = c1;
			this.c2 = c2;
		}

		// Token: 0x06002148 RID: 8520 RVA: 0x0005FDFE File Offset: 0x0005DFFE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3x3(uint m00, uint m01, uint m02, uint m10, uint m11, uint m12, uint m20, uint m21, uint m22)
		{
			this.c0 = new uint3(m00, m10, m20);
			this.c1 = new uint3(m01, m11, m21);
			this.c2 = new uint3(m02, m12, m22);
		}

		// Token: 0x06002149 RID: 8521 RVA: 0x0005FE30 File Offset: 0x0005E030
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3x3(uint v)
		{
			this.c0 = v;
			this.c1 = v;
			this.c2 = v;
		}

		// Token: 0x0600214A RID: 8522 RVA: 0x0005FE58 File Offset: 0x0005E058
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3x3(bool v)
		{
			this.c0 = math.select(new uint3(0U), new uint3(1U), v);
			this.c1 = math.select(new uint3(0U), new uint3(1U), v);
			this.c2 = math.select(new uint3(0U), new uint3(1U), v);
		}

		// Token: 0x0600214B RID: 8523 RVA: 0x0005FEB0 File Offset: 0x0005E0B0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3x3(bool3x3 v)
		{
			this.c0 = math.select(new uint3(0U), new uint3(1U), v.c0);
			this.c1 = math.select(new uint3(0U), new uint3(1U), v.c1);
			this.c2 = math.select(new uint3(0U), new uint3(1U), v.c2);
		}

		// Token: 0x0600214C RID: 8524 RVA: 0x0005FF14 File Offset: 0x0005E114
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3x3(int v)
		{
			this.c0 = (uint3)v;
			this.c1 = (uint3)v;
			this.c2 = (uint3)v;
		}

		// Token: 0x0600214D RID: 8525 RVA: 0x0005FF3A File Offset: 0x0005E13A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3x3(int3x3 v)
		{
			this.c0 = (uint3)v.c0;
			this.c1 = (uint3)v.c1;
			this.c2 = (uint3)v.c2;
		}

		// Token: 0x0600214E RID: 8526 RVA: 0x0005FF6F File Offset: 0x0005E16F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3x3(float v)
		{
			this.c0 = (uint3)v;
			this.c1 = (uint3)v;
			this.c2 = (uint3)v;
		}

		// Token: 0x0600214F RID: 8527 RVA: 0x0005FF95 File Offset: 0x0005E195
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3x3(float3x3 v)
		{
			this.c0 = (uint3)v.c0;
			this.c1 = (uint3)v.c1;
			this.c2 = (uint3)v.c2;
		}

		// Token: 0x06002150 RID: 8528 RVA: 0x0005FFCA File Offset: 0x0005E1CA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3x3(double v)
		{
			this.c0 = (uint3)v;
			this.c1 = (uint3)v;
			this.c2 = (uint3)v;
		}

		// Token: 0x06002151 RID: 8529 RVA: 0x0005FFF0 File Offset: 0x0005E1F0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint3x3(double3x3 v)
		{
			this.c0 = (uint3)v.c0;
			this.c1 = (uint3)v.c1;
			this.c2 = (uint3)v.c2;
		}

		// Token: 0x06002152 RID: 8530 RVA: 0x00060025 File Offset: 0x0005E225
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator uint3x3(uint v)
		{
			return new uint3x3(v);
		}

		// Token: 0x06002153 RID: 8531 RVA: 0x0006002D File Offset: 0x0005E22D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint3x3(bool v)
		{
			return new uint3x3(v);
		}

		// Token: 0x06002154 RID: 8532 RVA: 0x00060035 File Offset: 0x0005E235
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint3x3(bool3x3 v)
		{
			return new uint3x3(v);
		}

		// Token: 0x06002155 RID: 8533 RVA: 0x0006003D File Offset: 0x0005E23D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint3x3(int v)
		{
			return new uint3x3(v);
		}

		// Token: 0x06002156 RID: 8534 RVA: 0x00060045 File Offset: 0x0005E245
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint3x3(int3x3 v)
		{
			return new uint3x3(v);
		}

		// Token: 0x06002157 RID: 8535 RVA: 0x0006004D File Offset: 0x0005E24D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint3x3(float v)
		{
			return new uint3x3(v);
		}

		// Token: 0x06002158 RID: 8536 RVA: 0x00060055 File Offset: 0x0005E255
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint3x3(float3x3 v)
		{
			return new uint3x3(v);
		}

		// Token: 0x06002159 RID: 8537 RVA: 0x0006005D File Offset: 0x0005E25D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint3x3(double v)
		{
			return new uint3x3(v);
		}

		// Token: 0x0600215A RID: 8538 RVA: 0x00060065 File Offset: 0x0005E265
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator uint3x3(double3x3 v)
		{
			return new uint3x3(v);
		}

		// Token: 0x0600215B RID: 8539 RVA: 0x0006006D File Offset: 0x0005E26D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x3 operator *(uint3x3 lhs, uint3x3 rhs)
		{
			return new uint3x3(lhs.c0 * rhs.c0, lhs.c1 * rhs.c1, lhs.c2 * rhs.c2);
		}

		// Token: 0x0600215C RID: 8540 RVA: 0x000600A7 File Offset: 0x0005E2A7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x3 operator *(uint3x3 lhs, uint rhs)
		{
			return new uint3x3(lhs.c0 * rhs, lhs.c1 * rhs, lhs.c2 * rhs);
		}

		// Token: 0x0600215D RID: 8541 RVA: 0x000600D2 File Offset: 0x0005E2D2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x3 operator *(uint lhs, uint3x3 rhs)
		{
			return new uint3x3(lhs * rhs.c0, lhs * rhs.c1, lhs * rhs.c2);
		}

		// Token: 0x0600215E RID: 8542 RVA: 0x000600FD File Offset: 0x0005E2FD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x3 operator +(uint3x3 lhs, uint3x3 rhs)
		{
			return new uint3x3(lhs.c0 + rhs.c0, lhs.c1 + rhs.c1, lhs.c2 + rhs.c2);
		}

		// Token: 0x0600215F RID: 8543 RVA: 0x00060137 File Offset: 0x0005E337
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x3 operator +(uint3x3 lhs, uint rhs)
		{
			return new uint3x3(lhs.c0 + rhs, lhs.c1 + rhs, lhs.c2 + rhs);
		}

		// Token: 0x06002160 RID: 8544 RVA: 0x00060162 File Offset: 0x0005E362
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x3 operator +(uint lhs, uint3x3 rhs)
		{
			return new uint3x3(lhs + rhs.c0, lhs + rhs.c1, lhs + rhs.c2);
		}

		// Token: 0x06002161 RID: 8545 RVA: 0x0006018D File Offset: 0x0005E38D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x3 operator -(uint3x3 lhs, uint3x3 rhs)
		{
			return new uint3x3(lhs.c0 - rhs.c0, lhs.c1 - rhs.c1, lhs.c2 - rhs.c2);
		}

		// Token: 0x06002162 RID: 8546 RVA: 0x000601C7 File Offset: 0x0005E3C7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x3 operator -(uint3x3 lhs, uint rhs)
		{
			return new uint3x3(lhs.c0 - rhs, lhs.c1 - rhs, lhs.c2 - rhs);
		}

		// Token: 0x06002163 RID: 8547 RVA: 0x000601F2 File Offset: 0x0005E3F2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x3 operator -(uint lhs, uint3x3 rhs)
		{
			return new uint3x3(lhs - rhs.c0, lhs - rhs.c1, lhs - rhs.c2);
		}

		// Token: 0x06002164 RID: 8548 RVA: 0x0006021D File Offset: 0x0005E41D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x3 operator /(uint3x3 lhs, uint3x3 rhs)
		{
			return new uint3x3(lhs.c0 / rhs.c0, lhs.c1 / rhs.c1, lhs.c2 / rhs.c2);
		}

		// Token: 0x06002165 RID: 8549 RVA: 0x00060257 File Offset: 0x0005E457
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x3 operator /(uint3x3 lhs, uint rhs)
		{
			return new uint3x3(lhs.c0 / rhs, lhs.c1 / rhs, lhs.c2 / rhs);
		}

		// Token: 0x06002166 RID: 8550 RVA: 0x00060282 File Offset: 0x0005E482
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x3 operator /(uint lhs, uint3x3 rhs)
		{
			return new uint3x3(lhs / rhs.c0, lhs / rhs.c1, lhs / rhs.c2);
		}

		// Token: 0x06002167 RID: 8551 RVA: 0x000602AD File Offset: 0x0005E4AD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x3 operator %(uint3x3 lhs, uint3x3 rhs)
		{
			return new uint3x3(lhs.c0 % rhs.c0, lhs.c1 % rhs.c1, lhs.c2 % rhs.c2);
		}

		// Token: 0x06002168 RID: 8552 RVA: 0x000602E7 File Offset: 0x0005E4E7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x3 operator %(uint3x3 lhs, uint rhs)
		{
			return new uint3x3(lhs.c0 % rhs, lhs.c1 % rhs, lhs.c2 % rhs);
		}

		// Token: 0x06002169 RID: 8553 RVA: 0x00060312 File Offset: 0x0005E512
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x3 operator %(uint lhs, uint3x3 rhs)
		{
			return new uint3x3(lhs % rhs.c0, lhs % rhs.c1, lhs % rhs.c2);
		}

		// Token: 0x0600216A RID: 8554 RVA: 0x00060340 File Offset: 0x0005E540
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x3 operator ++(uint3x3 val)
		{
			uint3 @uint = uint3.op_Increment(val.c0);
			val.c0 = @uint;
			uint3 uint2 = @uint;
			@uint = uint3.op_Increment(val.c1);
			val.c1 = @uint;
			uint3 uint3 = @uint;
			@uint = uint3.op_Increment(val.c2);
			val.c2 = @uint;
			return new uint3x3(uint2, uint3, @uint);
		}

		// Token: 0x0600216B RID: 8555 RVA: 0x000603A0 File Offset: 0x0005E5A0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x3 operator --(uint3x3 val)
		{
			uint3 @uint = uint3.op_Decrement(val.c0);
			val.c0 = @uint;
			uint3 uint2 = @uint;
			@uint = uint3.op_Decrement(val.c1);
			val.c1 = @uint;
			uint3 uint3 = @uint;
			@uint = uint3.op_Decrement(val.c2);
			val.c2 = @uint;
			return new uint3x3(uint2, uint3, @uint);
		}

		// Token: 0x0600216C RID: 8556 RVA: 0x00060400 File Offset: 0x0005E600
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator <(uint3x3 lhs, uint3x3 rhs)
		{
			return new bool3x3(lhs.c0 < rhs.c0, lhs.c1 < rhs.c1, lhs.c2 < rhs.c2);
		}

		// Token: 0x0600216D RID: 8557 RVA: 0x0006043A File Offset: 0x0005E63A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator <(uint3x3 lhs, uint rhs)
		{
			return new bool3x3(lhs.c0 < rhs, lhs.c1 < rhs, lhs.c2 < rhs);
		}

		// Token: 0x0600216E RID: 8558 RVA: 0x00060465 File Offset: 0x0005E665
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator <(uint lhs, uint3x3 rhs)
		{
			return new bool3x3(lhs < rhs.c0, lhs < rhs.c1, lhs < rhs.c2);
		}

		// Token: 0x0600216F RID: 8559 RVA: 0x00060490 File Offset: 0x0005E690
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator <=(uint3x3 lhs, uint3x3 rhs)
		{
			return new bool3x3(lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1, lhs.c2 <= rhs.c2);
		}

		// Token: 0x06002170 RID: 8560 RVA: 0x000604CA File Offset: 0x0005E6CA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator <=(uint3x3 lhs, uint rhs)
		{
			return new bool3x3(lhs.c0 <= rhs, lhs.c1 <= rhs, lhs.c2 <= rhs);
		}

		// Token: 0x06002171 RID: 8561 RVA: 0x000604F5 File Offset: 0x0005E6F5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator <=(uint lhs, uint3x3 rhs)
		{
			return new bool3x3(lhs <= rhs.c0, lhs <= rhs.c1, lhs <= rhs.c2);
		}

		// Token: 0x06002172 RID: 8562 RVA: 0x00060520 File Offset: 0x0005E720
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator >(uint3x3 lhs, uint3x3 rhs)
		{
			return new bool3x3(lhs.c0 > rhs.c0, lhs.c1 > rhs.c1, lhs.c2 > rhs.c2);
		}

		// Token: 0x06002173 RID: 8563 RVA: 0x0006055A File Offset: 0x0005E75A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator >(uint3x3 lhs, uint rhs)
		{
			return new bool3x3(lhs.c0 > rhs, lhs.c1 > rhs, lhs.c2 > rhs);
		}

		// Token: 0x06002174 RID: 8564 RVA: 0x00060585 File Offset: 0x0005E785
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator >(uint lhs, uint3x3 rhs)
		{
			return new bool3x3(lhs > rhs.c0, lhs > rhs.c1, lhs > rhs.c2);
		}

		// Token: 0x06002175 RID: 8565 RVA: 0x000605B0 File Offset: 0x0005E7B0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator >=(uint3x3 lhs, uint3x3 rhs)
		{
			return new bool3x3(lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1, lhs.c2 >= rhs.c2);
		}

		// Token: 0x06002176 RID: 8566 RVA: 0x000605EA File Offset: 0x0005E7EA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator >=(uint3x3 lhs, uint rhs)
		{
			return new bool3x3(lhs.c0 >= rhs, lhs.c1 >= rhs, lhs.c2 >= rhs);
		}

		// Token: 0x06002177 RID: 8567 RVA: 0x00060615 File Offset: 0x0005E815
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator >=(uint lhs, uint3x3 rhs)
		{
			return new bool3x3(lhs >= rhs.c0, lhs >= rhs.c1, lhs >= rhs.c2);
		}

		// Token: 0x06002178 RID: 8568 RVA: 0x00060640 File Offset: 0x0005E840
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x3 operator -(uint3x3 val)
		{
			return new uint3x3(-val.c0, -val.c1, -val.c2);
		}

		// Token: 0x06002179 RID: 8569 RVA: 0x00060668 File Offset: 0x0005E868
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x3 operator +(uint3x3 val)
		{
			return new uint3x3(+val.c0, +val.c1, +val.c2);
		}

		// Token: 0x0600217A RID: 8570 RVA: 0x00060690 File Offset: 0x0005E890
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x3 operator <<(uint3x3 x, int n)
		{
			return new uint3x3(x.c0 << n, x.c1 << n, x.c2 << n);
		}

		// Token: 0x0600217B RID: 8571 RVA: 0x000606BB File Offset: 0x0005E8BB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x3 operator >>(uint3x3 x, int n)
		{
			return new uint3x3(x.c0 >> n, x.c1 >> n, x.c2 >> n);
		}

		// Token: 0x0600217C RID: 8572 RVA: 0x000606E6 File Offset: 0x0005E8E6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator ==(uint3x3 lhs, uint3x3 rhs)
		{
			return new bool3x3(lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2);
		}

		// Token: 0x0600217D RID: 8573 RVA: 0x00060720 File Offset: 0x0005E920
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator ==(uint3x3 lhs, uint rhs)
		{
			return new bool3x3(lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs);
		}

		// Token: 0x0600217E RID: 8574 RVA: 0x0006074B File Offset: 0x0005E94B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator ==(uint lhs, uint3x3 rhs)
		{
			return new bool3x3(lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2);
		}

		// Token: 0x0600217F RID: 8575 RVA: 0x00060776 File Offset: 0x0005E976
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator !=(uint3x3 lhs, uint3x3 rhs)
		{
			return new bool3x3(lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2);
		}

		// Token: 0x06002180 RID: 8576 RVA: 0x000607B0 File Offset: 0x0005E9B0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator !=(uint3x3 lhs, uint rhs)
		{
			return new bool3x3(lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs);
		}

		// Token: 0x06002181 RID: 8577 RVA: 0x000607DB File Offset: 0x0005E9DB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool3x3 operator !=(uint lhs, uint3x3 rhs)
		{
			return new bool3x3(lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2);
		}

		// Token: 0x06002182 RID: 8578 RVA: 0x00060806 File Offset: 0x0005EA06
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x3 operator ~(uint3x3 val)
		{
			return new uint3x3(~val.c0, ~val.c1, ~val.c2);
		}

		// Token: 0x06002183 RID: 8579 RVA: 0x0006082E File Offset: 0x0005EA2E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x3 operator &(uint3x3 lhs, uint3x3 rhs)
		{
			return new uint3x3(lhs.c0 & rhs.c0, lhs.c1 & rhs.c1, lhs.c2 & rhs.c2);
		}

		// Token: 0x06002184 RID: 8580 RVA: 0x00060868 File Offset: 0x0005EA68
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x3 operator &(uint3x3 lhs, uint rhs)
		{
			return new uint3x3(lhs.c0 & rhs, lhs.c1 & rhs, lhs.c2 & rhs);
		}

		// Token: 0x06002185 RID: 8581 RVA: 0x00060893 File Offset: 0x0005EA93
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x3 operator &(uint lhs, uint3x3 rhs)
		{
			return new uint3x3(lhs & rhs.c0, lhs & rhs.c1, lhs & rhs.c2);
		}

		// Token: 0x06002186 RID: 8582 RVA: 0x000608BE File Offset: 0x0005EABE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x3 operator |(uint3x3 lhs, uint3x3 rhs)
		{
			return new uint3x3(lhs.c0 | rhs.c0, lhs.c1 | rhs.c1, lhs.c2 | rhs.c2);
		}

		// Token: 0x06002187 RID: 8583 RVA: 0x000608F8 File Offset: 0x0005EAF8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x3 operator |(uint3x3 lhs, uint rhs)
		{
			return new uint3x3(lhs.c0 | rhs, lhs.c1 | rhs, lhs.c2 | rhs);
		}

		// Token: 0x06002188 RID: 8584 RVA: 0x00060923 File Offset: 0x0005EB23
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x3 operator |(uint lhs, uint3x3 rhs)
		{
			return new uint3x3(lhs | rhs.c0, lhs | rhs.c1, lhs | rhs.c2);
		}

		// Token: 0x06002189 RID: 8585 RVA: 0x0006094E File Offset: 0x0005EB4E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x3 operator ^(uint3x3 lhs, uint3x3 rhs)
		{
			return new uint3x3(lhs.c0 ^ rhs.c0, lhs.c1 ^ rhs.c1, lhs.c2 ^ rhs.c2);
		}

		// Token: 0x0600218A RID: 8586 RVA: 0x00060988 File Offset: 0x0005EB88
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x3 operator ^(uint3x3 lhs, uint rhs)
		{
			return new uint3x3(lhs.c0 ^ rhs, lhs.c1 ^ rhs, lhs.c2 ^ rhs);
		}

		// Token: 0x0600218B RID: 8587 RVA: 0x000609B3 File Offset: 0x0005EBB3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint3x3 operator ^(uint lhs, uint3x3 rhs)
		{
			return new uint3x3(lhs ^ rhs.c0, lhs ^ rhs.c1, lhs ^ rhs.c2);
		}

		// Token: 0x17000A34 RID: 2612
		public unsafe ref uint3 this[int index]
		{
			get
			{
				fixed (uint3x3* ptr = &this)
				{
					return ref *(uint3*)(ptr + (IntPtr)index * (IntPtr)sizeof(uint3) / (IntPtr)sizeof(uint3x3));
				}
			}
		}

		// Token: 0x0600218D RID: 8589 RVA: 0x000609FB File Offset: 0x0005EBFB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(uint3x3 rhs)
		{
			return this.c0.Equals(rhs.c0) && this.c1.Equals(rhs.c1) && this.c2.Equals(rhs.c2);
		}

		// Token: 0x0600218E RID: 8590 RVA: 0x00060A38 File Offset: 0x0005EC38
		public override bool Equals(object o)
		{
			if (o is uint3x3)
			{
				uint3x3 uint3x = (uint3x3)o;
				return this.Equals(uint3x);
			}
			return false;
		}

		// Token: 0x0600218F RID: 8591 RVA: 0x00060A5D File Offset: 0x0005EC5D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x06002190 RID: 8592 RVA: 0x00060A6C File Offset: 0x0005EC6C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("uint3x3({0}, {1}, {2},  {3}, {4}, {5},  {6}, {7}, {8})", new object[]
			{
				this.c0.x,
				this.c1.x,
				this.c2.x,
				this.c0.y,
				this.c1.y,
				this.c2.y,
				this.c0.z,
				this.c1.z,
				this.c2.z
			});
		}

		// Token: 0x06002191 RID: 8593 RVA: 0x00060B38 File Offset: 0x0005ED38
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("uint3x3({0}, {1}, {2},  {3}, {4}, {5},  {6}, {7}, {8})", new object[]
			{
				this.c0.x.ToString(format, formatProvider),
				this.c1.x.ToString(format, formatProvider),
				this.c2.x.ToString(format, formatProvider),
				this.c0.y.ToString(format, formatProvider),
				this.c1.y.ToString(format, formatProvider),
				this.c2.y.ToString(format, formatProvider),
				this.c0.z.ToString(format, formatProvider),
				this.c1.z.ToString(format, formatProvider),
				this.c2.z.ToString(format, formatProvider)
			});
		}

		// Token: 0x04000111 RID: 273
		public uint3 c0;

		// Token: 0x04000112 RID: 274
		public uint3 c1;

		// Token: 0x04000113 RID: 275
		public uint3 c2;

		// Token: 0x04000114 RID: 276
		public static readonly uint3x3 identity = new uint3x3(1U, 0U, 0U, 0U, 1U, 0U, 0U, 0U, 1U);

		// Token: 0x04000115 RID: 277
		public static readonly uint3x3 zero;
	}
}
